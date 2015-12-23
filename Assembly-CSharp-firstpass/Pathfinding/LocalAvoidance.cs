// Decompiled with JetBrains decompiler
// Type: Pathfinding.LocalAvoidance
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
  [RequireComponent(typeof (CharacterController))]
  public class LocalAvoidance : MonoBehaviour
  {
    public float speed = 2f;
    public float delta = 1f;
    public float responability = 0.5f;
    public LocalAvoidance.ResolutionType resType = LocalAvoidance.ResolutionType.Geometric;
    public float radius = 0.5f;
    public float maxSpeedScale = 1.5f;
    public float sampleScale = 1f;
    public float circleScale = 0.5f;
    public float circlePoint = 0.5f;
    private List<LocalAvoidance.VO> vos = new List<LocalAvoidance.VO>();
    public const float Rad2Deg = 57.29578f;
    private const int maxVOCounter = 50;
    private Vector3 velocity;
    public Vector3[] samples;
    public bool drawGizmos;
    protected CharacterController controller;
    protected LocalAvoidance[] agents;
    private Vector3 preVelocity;

    private void Start()
    {
      this.controller = this.GetComponent<CharacterController>();
      this.agents = UnityEngine.Object.FindObjectsOfType(typeof (LocalAvoidance)) as LocalAvoidance[];
    }

    public void Update()
    {
      this.SimpleMove(this.transform.forward * this.speed);
    }

    public Vector3 GetVelocity()
    {
      return this.preVelocity;
    }

    public void LateUpdate()
    {
      this.preVelocity = this.velocity;
    }

    public void SimpleMove(Vector3 desiredMovement)
    {
      Vector3 vector3_1 = UnityEngine.Random.insideUnitSphere * 0.1f;
      vector3_1.y = 0.0f;
      Vector3 vector3_2 = this.ClampMovement(desiredMovement + vector3_1);
      if (vector3_2 != Vector3.zero)
        vector3_2 /= this.delta;
      if (this.drawGizmos)
      {
        Debug.DrawRay(this.transform.position, desiredMovement, Color.magenta);
        Debug.DrawRay(this.transform.position, vector3_2, Color.yellow);
        Debug.DrawRay(this.transform.position + vector3_2, Vector3.up, Color.yellow);
      }
      this.controller.SimpleMove(vector3_2);
      this.velocity = this.controller.velocity;
      Debug.DrawRay(this.transform.position, this.velocity, Color.blue);
    }

    public Vector3 ClampMovement(Vector3 direction)
    {
      Vector3 vector3_1 = direction * this.delta;
      Vector3 target = this.transform.position + direction;
      Vector3 vector3_2 = target;
      float num1 = 0.0f;
      int num2 = 0;
      this.vos.Clear();
      float magnitude1 = this.velocity.magnitude;
      foreach (LocalAvoidance localAvoidance in this.agents)
      {
        if (!((UnityEngine.Object) localAvoidance == (UnityEngine.Object) this) && !((UnityEngine.Object) localAvoidance == (UnityEngine.Object) null))
        {
          Vector3 vector3_3 = localAvoidance.transform.position - this.transform.position;
          float magnitude2 = vector3_3.magnitude;
          float num3 = this.radius + localAvoidance.radius;
          if ((double) magnitude2 <= (double) vector3_1.magnitude * (double) this.delta + (double) num3 + (double) magnitude1 + (double) localAvoidance.GetVelocity().magnitude && num2 <= 50)
          {
            ++num2;
            LocalAvoidance.VO vo = new LocalAvoidance.VO();
            vo.origin = this.transform.position + Vector3.Lerp(this.velocity * this.delta, localAvoidance.GetVelocity() * this.delta, this.responability);
            vo.direction = vector3_3.normalized;
            vo.angle = (double) num3 <= (double) vector3_3.magnitude ? (float) Math.Asin((double) num3 / (double) magnitude2) : 1.570796f;
            vo.limit = magnitude2 - num3;
            if ((double) vo.limit < 0.0)
            {
              vo.origin += vo.direction * vo.limit;
              vo.limit = 0.0f;
            }
            float num4 = Mathf.Atan2(vo.direction.z, vo.direction.x);
            vo.pRight = new Vector3(Mathf.Cos(num4 + vo.angle), 0.0f, Mathf.Sin(num4 + vo.angle));
            vo.pLeft = new Vector3(Mathf.Cos(num4 - vo.angle), 0.0f, Mathf.Sin(num4 - vo.angle));
            vo.nLeft = new Vector3(Mathf.Cos((float) ((double) num4 + (double) vo.angle - 1.57079637050629)), 0.0f, Mathf.Sin((float) ((double) num4 + (double) vo.angle - 1.57079637050629)));
            vo.nRight = new Vector3(Mathf.Cos((float) ((double) num4 - (double) vo.angle + 1.57079637050629)), 0.0f, Mathf.Sin((float) ((double) num4 - (double) vo.angle + 1.57079637050629)));
            this.vos.Add(vo);
          }
        }
      }
      if (this.resType == LocalAvoidance.ResolutionType.Geometric)
      {
        for (int index = 0; index < this.vos.Count; ++index)
        {
          if (this.vos[index].Contains(vector3_2))
          {
            num1 = float.PositiveInfinity;
            if (this.drawGizmos)
              Debug.DrawRay(vector3_2, Vector3.down, Color.red);
            vector3_2 = this.transform.position;
            break;
          }
        }
        if (this.drawGizmos)
        {
          for (int index = 0; index < this.vos.Count; ++index)
            this.vos[index].Draw(Color.black);
        }
        if ((double) num1 == 0.0)
          return vector3_1;
        List<LocalAvoidance.VOLine> list = new List<LocalAvoidance.VOLine>();
        for (int index1 = 0; index1 < this.vos.Count; ++index1)
        {
          LocalAvoidance.VO vo = this.vos[index1];
          float num3 = (float) Math.Atan2((double) vo.direction.z, (double) vo.direction.x);
          Vector3 vector3_3 = vo.origin + new Vector3((float) Math.Cos((double) num3 + (double) vo.angle), 0.0f, (float) Math.Sin((double) num3 + (double) vo.angle)) * vo.limit;
          Vector3 vector3_4 = vo.origin + new Vector3((float) Math.Cos((double) num3 - (double) vo.angle), 0.0f, (float) Math.Sin((double) num3 - (double) vo.angle)) * vo.limit;
          Vector3 end1 = vector3_3 + new Vector3((float) Math.Cos((double) num3 + (double) vo.angle), 0.0f, (float) Math.Sin((double) num3 + (double) vo.angle)) * 100f;
          Vector3 end2 = vector3_4 + new Vector3((float) Math.Cos((double) num3 - (double) vo.angle), 0.0f, (float) Math.Sin((double) num3 - (double) vo.angle)) * 100f;
          int num4 = !Polygon.Left(vo.origin, vo.origin + vo.direction, this.transform.position + this.velocity) ? 2 : 1;
          list.Add(new LocalAvoidance.VOLine(vo, vector3_3, end1, true, 1, num4 == 1));
          list.Add(new LocalAvoidance.VOLine(vo, vector3_4, end2, true, 2, num4 == 2));
          list.Add(new LocalAvoidance.VOLine(vo, vector3_3, vector3_4, false, 3, false));
          bool inside1 = false;
          bool inside2 = false;
          if (!inside1)
          {
            for (int index2 = 0; index2 < this.vos.Count; ++index2)
            {
              if (index2 != index1 && this.vos[index2].Contains(vector3_3))
              {
                inside1 = true;
                break;
              }
            }
          }
          if (!inside2)
          {
            for (int index2 = 0; index2 < this.vos.Count; ++index2)
            {
              if (index2 != index1 && this.vos[index2].Contains(vector3_4))
              {
                inside2 = true;
                break;
              }
            }
          }
          vo.AddInt(0.0f, inside1, 1);
          vo.AddInt(0.0f, inside2, 2);
          vo.AddInt(0.0f, inside1, 3);
          vo.AddInt(1f, inside2, 3);
        }
        for (int index1 = 0; index1 < list.Count; ++index1)
        {
          for (int index2 = index1 + 1; index2 < list.Count; ++index2)
          {
            LocalAvoidance.VOLine voLine1 = list[index1];
            LocalAvoidance.VOLine voLine2 = list[index2];
            float factor1;
            float factor2;
            if (voLine1.vo != voLine2.vo && Polygon.IntersectionFactor(voLine1.start, voLine1.end, voLine2.start, voLine2.end, out factor1, out factor2) && ((double) factor1 >= 0.0 && (double) factor2 >= 0.0) && ((voLine1.inf || (double) factor1 <= 1.0) && (voLine2.inf || (double) factor2 <= 1.0)))
            {
              Vector3 p = voLine1.start + (voLine1.end - voLine1.start) * factor1;
              bool inside = voLine1.wrongSide || voLine2.wrongSide;
              if (!inside)
              {
                for (int index3 = 0; index3 < this.vos.Count; ++index3)
                {
                  if (this.vos[index3] != voLine1.vo && this.vos[index3] != voLine2.vo && this.vos[index3].Contains(p))
                  {
                    inside = true;
                    break;
                  }
                }
              }
              voLine1.vo.AddInt(factor1, inside, voLine1.id);
              voLine2.vo.AddInt(factor2, inside, voLine2.id);
              if (this.drawGizmos)
                Debug.DrawRay(voLine1.start + (voLine1.end - voLine1.start) * factor1, Vector3.up, !inside ? Color.green : Color.magenta);
            }
          }
        }
        for (int index = 0; index < this.vos.Count; ++index)
        {
          Vector3 closest;
          if (this.vos[index].FinalInts(target, this.transform.position + this.velocity, this.drawGizmos, out closest))
          {
            float sqrMagnitude = (closest - target).sqrMagnitude;
            if ((double) sqrMagnitude < (double) num1)
            {
              vector3_2 = closest;
              num1 = sqrMagnitude;
              if (this.drawGizmos)
                Debug.DrawLine(target + Vector3.up, vector3_2 + Vector3.up, Color.red);
            }
          }
        }
        if (this.drawGizmos)
          Debug.DrawLine(target + Vector3.up, vector3_2 + Vector3.up, Color.red);
        return Vector3.ClampMagnitude(vector3_2 - this.transform.position, vector3_1.magnitude * this.maxSpeedScale);
      }
      if (this.resType != LocalAvoidance.ResolutionType.Sampled)
        return Vector3.zero;
      Vector3 vector3_5 = vector3_1;
      Vector3 normalized = vector3_5.normalized;
      Vector3 vector3_6 = Vector3.Cross(normalized, Vector3.up);
      int num5 = 10;
      int num6 = 0;
      while (num6 < 10)
      {
        float num3 = 3.141593f * this.circlePoint / (float) num5;
        float num4 = (float) (Math.PI - (double) this.circlePoint * Math.PI) * 0.5f;
        for (int index = 0; index < num5; ++index)
        {
          float num7 = num3 * (float) index;
          Vector3 sample = this.transform.position + vector3_1 - vector3_5 * (float) Math.Sin((double) num7 + (double) num4) * (float) num6 * this.circleScale + vector3_6 * (float) Math.Cos((double) num7 + (double) num4) * (float) num6 * this.circleScale;
          if (this.CheckSample(sample, this.vos))
            return sample - this.transform.position;
        }
        ++num6;
        num5 += 2;
      }
      for (int index = 0; index < this.samples.Length; ++index)
      {
        Vector3 sample = this.transform.position + this.samples[index].x * vector3_6 + this.samples[index].z * normalized + this.samples[index].y * vector3_5;
        if (this.CheckSample(sample, this.vos))
          return sample - this.transform.position;
      }
      return Vector3.zero;
    }

    public bool CheckSample(Vector3 sample, List<LocalAvoidance.VO> vos)
    {
      bool flag = false;
      for (int index = 0; index < vos.Count; ++index)
      {
        if (vos[index].Contains(sample))
        {
          if (this.drawGizmos)
            Debug.DrawRay(sample, Vector3.up, Color.red);
          flag = true;
          break;
        }
      }
      if (this.drawGizmos && !flag)
        Debug.DrawRay(sample, Vector3.up, Color.yellow);
      return !flag;
    }

    public enum ResolutionType
    {
      Sampled,
      Geometric,
    }

    public struct VOLine
    {
      public LocalAvoidance.VO vo;
      public Vector3 start;
      public Vector3 end;
      public bool inf;
      public int id;
      public bool wrongSide;

      public VOLine(LocalAvoidance.VO vo, Vector3 start, Vector3 end, bool inf, int id, bool wrongSide)
      {
        this.vo = vo;
        this.start = start;
        this.end = end;
        this.inf = inf;
        this.id = id;
        this.wrongSide = wrongSide;
      }
    }

    public struct VOIntersection
    {
      public LocalAvoidance.VO vo1;
      public LocalAvoidance.VO vo2;
      public float factor1;
      public float factor2;
      public bool inside;

      public VOIntersection(LocalAvoidance.VO vo1, LocalAvoidance.VO vo2, float factor1, float factor2, bool inside = false)
      {
        this.vo1 = vo1;
        this.vo2 = vo2;
        this.factor1 = factor1;
        this.factor2 = factor2;
        this.inside = inside;
      }
    }

    public class HalfPlane
    {
      public Vector3 point;
      public Vector3 normal;

      public bool Contains(Vector3 p)
      {
        p -= this.point;
        return (double) Vector3.Dot(this.normal, p) >= 0.0;
      }

      public Vector3 ClosestPoint(Vector3 p)
      {
        p -= this.point;
        Vector3 lhs = Vector3.Cross(this.normal, Vector3.up);
        float num = Vector3.Dot(lhs, p);
        return this.point + lhs * num;
      }

      public Vector3 ClosestPoint(Vector3 p, float minX, float maxX)
      {
        p -= this.point;
        Vector3 lhs = Vector3.Cross(this.normal, Vector3.up);
        if ((double) lhs.x < 0.0)
          lhs = -lhs;
        float num = Mathf.Clamp(Vector3.Dot(lhs, p), (minX - this.point.x) / lhs.x, (maxX - this.point.x) / lhs.x);
        return this.point + lhs * num;
      }

      public Vector3 Intersection(LocalAvoidance.HalfPlane hp)
      {
        Vector3 dir1 = Vector3.Cross(this.normal, Vector3.up);
        Vector3 dir2 = Vector3.Cross(hp.normal, Vector3.up);
        return Polygon.IntersectionPointOptimized(this.point, dir1, hp.point, dir2);
      }

      public void DrawBounds(float left, float right)
      {
        Vector3 vector3 = Vector3.Cross(this.normal, Vector3.up);
        if ((double) vector3.x < 0.0)
          vector3 = -vector3;
        float num1 = (left - this.point.x) / vector3.x;
        float num2 = (right - this.point.x) / vector3.x;
        Debug.DrawLine(this.point + vector3 * num1 + Vector3.up * 0.1f, this.point + vector3 * num2 + Vector3.up * 0.1f, Color.yellow);
      }

      public void Draw()
      {
        Vector3 vector3 = Vector3.Cross(this.normal, Vector3.up);
        Debug.DrawLine(this.point - vector3 * 10f, this.point + vector3 * 10f, Color.blue);
        Debug.DrawRay(this.point, this.normal, new Color(0.8f, 0.1f, 0.2f));
      }
    }

    public enum IntersectionState
    {
      Inside,
      Outside,
      Enter,
      Exit,
    }

    public struct IntersectionPair : IComparable<LocalAvoidance.IntersectionPair>
    {
      public float factor;
      public LocalAvoidance.IntersectionState state;

      public IntersectionPair(float factor, bool inside)
      {
        this.factor = factor;
        this.state = !inside ? LocalAvoidance.IntersectionState.Outside : LocalAvoidance.IntersectionState.Inside;
      }

      public void SetState(LocalAvoidance.IntersectionState s)
      {
        this.state = s;
      }

      public int CompareTo(LocalAvoidance.IntersectionPair o)
      {
        if ((double) o.factor < (double) this.factor)
          return 1;
        return (double) o.factor > (double) this.factor ? -1 : 0;
      }
    }

    public class VO
    {
      public List<LocalAvoidance.IntersectionPair> ints1 = new List<LocalAvoidance.IntersectionPair>();
      public List<LocalAvoidance.IntersectionPair> ints2 = new List<LocalAvoidance.IntersectionPair>();
      public List<LocalAvoidance.IntersectionPair> ints3 = new List<LocalAvoidance.IntersectionPair>();
      public Vector3 origin;
      public Vector3 direction;
      public float angle;
      public float limit;
      public Vector3 pLeft;
      public Vector3 pRight;
      public Vector3 nLeft;
      public Vector3 nRight;

      public static explicit operator LocalAvoidance.HalfPlane(LocalAvoidance.VO vo)
      {
        return new LocalAvoidance.HalfPlane()
        {
          point = vo.origin + vo.direction * vo.limit,
          normal = -vo.direction
        };
      }

      public void AddInt(float factor, bool inside, int id)
      {
        switch (id)
        {
          case 1:
            this.ints1.Add(new LocalAvoidance.IntersectionPair(factor, inside));
            break;
          case 2:
            this.ints2.Add(new LocalAvoidance.IntersectionPair(factor, inside));
            break;
          case 3:
            this.ints3.Add(new LocalAvoidance.IntersectionPair(factor, inside));
            break;
        }
      }

      public bool FinalInts(Vector3 target, Vector3 closeEdgeConstraint, bool drawGizmos, out Vector3 closest)
      {
        this.ints1.Sort();
        this.ints2.Sort();
        this.ints3.Sort();
        float num1 = (float) Math.Atan2((double) this.direction.z, (double) this.direction.x);
        Vector3 rhs = Vector3.Cross(this.direction, Vector3.up);
        Vector3 vector3_1 = rhs * (float) Math.Tan((double) this.angle) * this.limit;
        Vector3 vector3_2 = this.origin + this.direction * this.limit + vector3_1;
        Vector3 vector3_3 = this.origin + this.direction * this.limit - vector3_1;
        Vector3 vector3_4 = vector3_2 + new Vector3((float) Math.Cos((double) num1 + (double) this.angle), 0.0f, (float) Math.Sin((double) num1 + (double) this.angle)) * 100f;
        Vector3 vector3_5 = vector3_3 + new Vector3((float) Math.Cos((double) num1 - (double) this.angle), 0.0f, (float) Math.Sin((double) num1 - (double) this.angle)) * 100f;
        bool flag1 = false;
        closest = Vector3.zero;
        int num2 = (double) Vector3.Dot(closeEdgeConstraint - this.origin, rhs) <= 0.0 ? 1 : 2;
        for (int index1 = 1; index1 <= 3; ++index1)
        {
          if (index1 != num2)
          {
            List<LocalAvoidance.IntersectionPair> list = index1 != 1 ? (index1 != 2 ? this.ints3 : this.ints2) : this.ints1;
            Vector3 lineStart = index1 == 1 || index1 == 3 ? vector3_2 : vector3_3;
            Vector3 lineEnd = index1 != 1 ? (index1 != 2 ? vector3_3 : vector3_5) : vector3_4;
            float num3 = AstarMath.NearestPointFactor(lineStart, lineEnd, target);
            float num4 = float.PositiveInfinity;
            float num5 = float.NegativeInfinity;
            bool flag2 = false;
            for (int index2 = 0; index2 < list.Count - (index1 != 3 ? 0 : 1); ++index2)
            {
              if (drawGizmos)
                Debug.DrawRay(lineStart + (lineEnd - lineStart) * list[index2].factor, Vector3.down, list[index2].state != LocalAvoidance.IntersectionState.Outside ? Color.red : Color.green);
              if (list[index2].state == LocalAvoidance.IntersectionState.Outside && (index2 == list.Count - 1 && (index2 == 0 || list[index2 - 1].state != LocalAvoidance.IntersectionState.Outside) || index2 < list.Count - 1 && list[index2 + 1].state == LocalAvoidance.IntersectionState.Outside))
              {
                flag2 = true;
                float num6 = list[index2].factor;
                float num7 = index2 != list.Count - 1 ? list[index2 + 1].factor : (index1 != 3 ? float.PositiveInfinity : 1f);
                if (drawGizmos)
                  Debug.DrawLine(lineStart + (lineEnd - lineStart) * num6 + Vector3.up, lineStart + (lineEnd - lineStart) * Mathf.Clamp01(num7) + Vector3.up, Color.green);
                if ((double) num6 <= (double) num3 && (double) num7 >= (double) num3)
                {
                  num4 = num3;
                  num5 = num3;
                  break;
                }
                if ((double) num7 < (double) num3 && (double) num7 > (double) num5)
                  num5 = num7;
                else if ((double) num6 > (double) num3 && (double) num6 < (double) num4)
                  num4 = num6;
              }
            }
            if (flag2)
            {
              float num6 = (double) num4 != double.NegativeInfinity ? ((double) num5 != double.PositiveInfinity ? ((double) Mathf.Abs(num3 - num4) >= (double) Mathf.Abs(num3 - num5) ? num5 : num4) : num4) : num5;
              Vector3 vector3_6 = lineStart + (lineEnd - lineStart) * num6;
              if (!flag1 || (double) (vector3_6 - target).sqrMagnitude < (double) (closest - target).sqrMagnitude)
                closest = vector3_6;
              if (drawGizmos)
                Debug.DrawLine(target, closest, Color.yellow);
              flag1 = true;
            }
          }
        }
        return flag1;
      }

      public bool Contains(Vector3 p)
      {
        if ((double) Vector3.Dot(this.nLeft, p - this.origin) > 0.0 && (double) Vector3.Dot(this.nRight, p - this.origin) > 0.0)
          return (double) Vector3.Dot(this.direction, p - this.origin) > (double) this.limit;
        return false;
      }

      public float ScoreContains(Vector3 p)
      {
        return 0.0f;
      }

      public void Draw(Color c)
      {
        float num = (float) Math.Atan2((double) this.direction.z, (double) this.direction.x);
        Vector3 vector3 = Vector3.Cross(this.direction, Vector3.up) * (float) Math.Tan((double) this.angle) * this.limit;
        Debug.DrawLine(this.origin + this.direction * this.limit + vector3, this.origin + this.direction * this.limit - vector3, c);
        Debug.DrawRay(this.origin + this.direction * this.limit + vector3, new Vector3((float) Math.Cos((double) num + (double) this.angle), 0.0f, (float) Math.Sin((double) num + (double) this.angle)) * 10f, c);
        Debug.DrawRay(this.origin + this.direction * this.limit - vector3, new Vector3((float) Math.Cos((double) num - (double) this.angle), 0.0f, (float) Math.Sin((double) num - (double) this.angle)) * 10f, c);
      }
    }
  }
}
