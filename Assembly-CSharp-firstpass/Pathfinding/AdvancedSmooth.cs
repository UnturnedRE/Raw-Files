﻿// Decompiled with JetBrains decompiler
// Type: Pathfinding.AdvancedSmooth
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
  [AddComponentMenu("Pathfinding/Modifiers/Advanced Smooth")]
  [Serializable]
  public class AdvancedSmooth : MonoModifier
  {
    public float turningRadius = 1f;
    public AdvancedSmooth.MaxTurn turnConstruct1 = new AdvancedSmooth.MaxTurn();
    public AdvancedSmooth.ConstantTurn turnConstruct2 = new AdvancedSmooth.ConstantTurn();

    public override ModifierData input
    {
      get
      {
        return ModifierData.VectorPath;
      }
    }

    public override ModifierData output
    {
      get
      {
        return ModifierData.VectorPath;
      }
    }

    public override void Apply(Path p, ModifierData source)
    {
      Vector3[] vectorPath = p.vectorPath.ToArray();
      if (vectorPath == null || vectorPath.Length <= 2)
        return;
      List<Vector3> output = new List<Vector3>();
      output.Add(vectorPath[0]);
      AdvancedSmooth.TurnConstructor.turningRadius = this.turningRadius;
      for (int i = 1; i < vectorPath.Length - 1; ++i)
      {
        List<AdvancedSmooth.Turn> turnList = new List<AdvancedSmooth.Turn>();
        AdvancedSmooth.TurnConstructor.Setup(i, vectorPath);
        this.turnConstruct1.Prepare(i, vectorPath);
        this.turnConstruct2.Prepare(i, vectorPath);
        AdvancedSmooth.TurnConstructor.PostPrepare();
        if (i == 1)
        {
          this.turnConstruct1.PointToTangent(turnList);
          this.turnConstruct2.PointToTangent(turnList);
        }
        else
        {
          this.turnConstruct1.TangentToTangent(turnList);
          this.turnConstruct2.TangentToTangent(turnList);
        }
        this.EvaluatePaths(turnList, output);
        if (i == vectorPath.Length - 2)
        {
          this.turnConstruct1.TangentToPoint(turnList);
          this.turnConstruct2.TangentToPoint(turnList);
        }
        this.EvaluatePaths(turnList, output);
      }
      output.Add(vectorPath[vectorPath.Length - 1]);
      p.vectorPath = output;
    }

    private void EvaluatePaths(List<AdvancedSmooth.Turn> turnList, List<Vector3> output)
    {
      turnList.Sort();
      for (int index = 0; index < turnList.Count; ++index)
      {
        if (index == 0)
          turnList[index].GetPath(output);
      }
      turnList.Clear();
      if (!AdvancedSmooth.TurnConstructor.changedPreviousTangent)
        return;
      this.turnConstruct1.OnTangentUpdate();
      this.turnConstruct2.OnTangentUpdate();
    }

    [Serializable]
    public class MaxTurn : AdvancedSmooth.TurnConstructor
    {
      private Vector3 preRightCircleCenter = Vector3.zero;
      private Vector3 preLeftCircleCenter = Vector3.zero;
      private Vector3 rightCircleCenter;
      private Vector3 leftCircleCenter;
      private double vaRight;
      private double vaLeft;
      private double preVaLeft;
      private double preVaRight;
      private double gammaLeft;
      private double gammaRight;
      private double betaRightRight;
      private double betaRightLeft;
      private double betaLeftRight;
      private double betaLeftLeft;
      private double deltaRightLeft;
      private double deltaLeftRight;
      private double alfaRightRight;
      private double alfaLeftLeft;
      private double alfaRightLeft;
      private double alfaLeftRight;

      public override void OnTangentUpdate()
      {
        this.rightCircleCenter = AdvancedSmooth.TurnConstructor.current + AdvancedSmooth.TurnConstructor.normal * AdvancedSmooth.TurnConstructor.turningRadius;
        this.leftCircleCenter = AdvancedSmooth.TurnConstructor.current - AdvancedSmooth.TurnConstructor.normal * AdvancedSmooth.TurnConstructor.turningRadius;
        this.vaRight = this.Atan2(AdvancedSmooth.TurnConstructor.current - this.rightCircleCenter);
        this.vaLeft = this.vaRight + Math.PI;
      }

      public override void Prepare(int i, Vector3[] vectorPath)
      {
        this.preRightCircleCenter = this.rightCircleCenter;
        this.preLeftCircleCenter = this.leftCircleCenter;
        this.rightCircleCenter = AdvancedSmooth.TurnConstructor.current + AdvancedSmooth.TurnConstructor.normal * AdvancedSmooth.TurnConstructor.turningRadius;
        this.leftCircleCenter = AdvancedSmooth.TurnConstructor.current - AdvancedSmooth.TurnConstructor.normal * AdvancedSmooth.TurnConstructor.turningRadius;
        this.preVaRight = this.vaRight;
        this.preVaLeft = this.vaLeft;
        this.vaRight = this.Atan2(AdvancedSmooth.TurnConstructor.current - this.rightCircleCenter);
        this.vaLeft = this.vaRight + Math.PI;
      }

      public override void TangentToTangent(List<AdvancedSmooth.Turn> turnList)
      {
        this.alfaRightRight = this.Atan2(this.rightCircleCenter - this.preRightCircleCenter);
        this.alfaLeftLeft = this.Atan2(this.leftCircleCenter - this.preLeftCircleCenter);
        this.alfaRightLeft = this.Atan2(this.leftCircleCenter - this.preRightCircleCenter);
        this.alfaLeftRight = this.Atan2(this.rightCircleCenter - this.preLeftCircleCenter);
        double num1 = (double) (this.leftCircleCenter - this.preRightCircleCenter).magnitude;
        double num2 = (double) (this.rightCircleCenter - this.preLeftCircleCenter).magnitude;
        bool flag1 = false;
        bool flag2 = false;
        if (num1 < (double) AdvancedSmooth.TurnConstructor.turningRadius * 2.0)
        {
          num1 = (double) AdvancedSmooth.TurnConstructor.turningRadius * 2.0;
          flag1 = true;
        }
        if (num2 < (double) AdvancedSmooth.TurnConstructor.turningRadius * 2.0)
        {
          num2 = (double) AdvancedSmooth.TurnConstructor.turningRadius * 2.0;
          flag2 = true;
        }
        this.deltaRightLeft = !flag1 ? Math.PI / 2.0 - Math.Asin((double) AdvancedSmooth.TurnConstructor.turningRadius * 2.0 / num1) : 0.0;
        this.deltaLeftRight = !flag2 ? Math.PI / 2.0 - Math.Asin((double) AdvancedSmooth.TurnConstructor.turningRadius * 2.0 / num2) : 0.0;
        this.betaRightRight = this.ClockwiseAngle(this.preVaRight, this.alfaRightRight - Math.PI / 2.0);
        this.betaRightLeft = this.ClockwiseAngle(this.preVaRight, this.alfaRightLeft - this.deltaRightLeft);
        this.betaLeftRight = this.CounterClockwiseAngle(this.preVaLeft, this.alfaLeftRight + this.deltaLeftRight);
        this.betaLeftLeft = this.CounterClockwiseAngle(this.preVaLeft, this.alfaLeftLeft + Math.PI / 2.0);
        this.betaRightRight += this.ClockwiseAngle(this.alfaRightRight - Math.PI / 2.0, this.vaRight);
        this.betaRightLeft += this.CounterClockwiseAngle(this.alfaRightLeft + this.deltaRightLeft, this.vaLeft);
        this.betaLeftRight += this.ClockwiseAngle(this.alfaLeftRight - this.deltaLeftRight, this.vaRight);
        this.betaLeftLeft += this.CounterClockwiseAngle(this.alfaLeftLeft + Math.PI / 2.0, this.vaLeft);
        this.betaRightRight = this.GetLengthFromAngle(this.betaRightRight, (double) AdvancedSmooth.TurnConstructor.turningRadius);
        this.betaRightLeft = this.GetLengthFromAngle(this.betaRightLeft, (double) AdvancedSmooth.TurnConstructor.turningRadius);
        this.betaLeftRight = this.GetLengthFromAngle(this.betaLeftRight, (double) AdvancedSmooth.TurnConstructor.turningRadius);
        this.betaLeftLeft = this.GetLengthFromAngle(this.betaLeftLeft, (double) AdvancedSmooth.TurnConstructor.turningRadius);
        Vector3 vector3_1 = this.AngleToVector(this.alfaRightRight - Math.PI / 2.0) * AdvancedSmooth.TurnConstructor.turningRadius + this.preRightCircleCenter;
        Vector3 vector3_2 = this.AngleToVector(this.alfaRightLeft - this.deltaRightLeft) * AdvancedSmooth.TurnConstructor.turningRadius + this.preRightCircleCenter;
        Vector3 vector3_3 = this.AngleToVector(this.alfaLeftRight + this.deltaLeftRight) * AdvancedSmooth.TurnConstructor.turningRadius + this.preLeftCircleCenter;
        Vector3 vector3_4 = this.AngleToVector(this.alfaLeftLeft + Math.PI / 2.0) * AdvancedSmooth.TurnConstructor.turningRadius + this.preLeftCircleCenter;
        Vector3 vector3_5 = this.AngleToVector(this.alfaRightRight - Math.PI / 2.0) * AdvancedSmooth.TurnConstructor.turningRadius + this.rightCircleCenter;
        Vector3 vector3_6 = this.AngleToVector(this.alfaRightLeft - this.deltaRightLeft + Math.PI) * AdvancedSmooth.TurnConstructor.turningRadius + this.leftCircleCenter;
        Vector3 vector3_7 = this.AngleToVector(this.alfaLeftRight + this.deltaLeftRight + Math.PI) * AdvancedSmooth.TurnConstructor.turningRadius + this.rightCircleCenter;
        Vector3 vector3_8 = this.AngleToVector(this.alfaLeftLeft + Math.PI / 2.0) * AdvancedSmooth.TurnConstructor.turningRadius + this.leftCircleCenter;
        this.betaRightRight += (double) (vector3_1 - vector3_5).magnitude;
        this.betaRightLeft += (double) (vector3_2 - vector3_6).magnitude;
        this.betaLeftRight += (double) (vector3_3 - vector3_7).magnitude;
        this.betaLeftLeft += (double) (vector3_4 - vector3_8).magnitude;
        if (flag1)
          this.betaRightLeft += 10000000.0;
        if (flag2)
          this.betaLeftRight += 10000000.0;
        turnList.Add(new AdvancedSmooth.Turn((float) this.betaRightRight, (AdvancedSmooth.TurnConstructor) this, 2));
        turnList.Add(new AdvancedSmooth.Turn((float) this.betaRightLeft, (AdvancedSmooth.TurnConstructor) this, 3));
        turnList.Add(new AdvancedSmooth.Turn((float) this.betaLeftRight, (AdvancedSmooth.TurnConstructor) this, 4));
        turnList.Add(new AdvancedSmooth.Turn((float) this.betaLeftLeft, (AdvancedSmooth.TurnConstructor) this, 5));
      }

      public override void PointToTangent(List<AdvancedSmooth.Turn> turnList)
      {
        bool flag1 = false;
        bool flag2 = false;
        float magnitude1 = (AdvancedSmooth.TurnConstructor.prev - this.rightCircleCenter).magnitude;
        float magnitude2 = (AdvancedSmooth.TurnConstructor.prev - this.leftCircleCenter).magnitude;
        if ((double) magnitude1 < (double) AdvancedSmooth.TurnConstructor.turningRadius)
          flag1 = true;
        if ((double) magnitude2 < (double) AdvancedSmooth.TurnConstructor.turningRadius)
          flag2 = true;
        this.gammaRight = (!flag1 ? this.Atan2(AdvancedSmooth.TurnConstructor.prev - this.rightCircleCenter) : 0.0) + (!flag1 ? Math.PI / 2.0 - Math.Asin((double) AdvancedSmooth.TurnConstructor.turningRadius / (double) (AdvancedSmooth.TurnConstructor.prev - this.rightCircleCenter).magnitude) : 0.0);
        double num1 = !flag1 ? this.ClockwiseAngle(this.gammaRight, this.vaRight) : 0.0;
        this.gammaLeft = (!flag2 ? this.Atan2(AdvancedSmooth.TurnConstructor.prev - this.leftCircleCenter) : 0.0) - (!flag2 ? Math.PI / 2.0 - Math.Asin((double) AdvancedSmooth.TurnConstructor.turningRadius / (double) (AdvancedSmooth.TurnConstructor.prev - this.leftCircleCenter).magnitude) : 0.0);
        double num2 = !flag2 ? this.CounterClockwiseAngle(this.gammaLeft, this.vaLeft) : 0.0;
        if (!flag1)
          turnList.Add(new AdvancedSmooth.Turn((float) num1, (AdvancedSmooth.TurnConstructor) this, 0));
        if (flag2)
          return;
        turnList.Add(new AdvancedSmooth.Turn((float) num2, (AdvancedSmooth.TurnConstructor) this, 1));
      }

      public override void TangentToPoint(List<AdvancedSmooth.Turn> turnList)
      {
        bool flag1 = false;
        bool flag2 = false;
        float magnitude1 = (AdvancedSmooth.TurnConstructor.next - this.rightCircleCenter).magnitude;
        float magnitude2 = (AdvancedSmooth.TurnConstructor.next - this.leftCircleCenter).magnitude;
        if ((double) magnitude1 < (double) AdvancedSmooth.TurnConstructor.turningRadius)
          flag1 = true;
        if ((double) magnitude2 < (double) AdvancedSmooth.TurnConstructor.turningRadius)
          flag2 = true;
        if (!flag1)
        {
          this.gammaRight = this.Atan2(AdvancedSmooth.TurnConstructor.next - this.rightCircleCenter) - (Math.PI / 2.0 - Math.Asin((double) AdvancedSmooth.TurnConstructor.turningRadius / (double) magnitude1));
          double num = this.ClockwiseAngle(this.vaRight, this.gammaRight);
          turnList.Add(new AdvancedSmooth.Turn((float) num, (AdvancedSmooth.TurnConstructor) this, 6));
        }
        if (flag2)
          return;
        this.gammaLeft = this.Atan2(AdvancedSmooth.TurnConstructor.next - this.leftCircleCenter) + (Math.PI / 2.0 - Math.Asin((double) AdvancedSmooth.TurnConstructor.turningRadius / (double) magnitude2));
        double num1 = this.CounterClockwiseAngle(this.vaLeft, this.gammaLeft);
        turnList.Add(new AdvancedSmooth.Turn((float) num1, (AdvancedSmooth.TurnConstructor) this, 7));
      }

      public override void GetPath(AdvancedSmooth.Turn turn, List<Vector3> output)
      {
        switch (turn.id)
        {
          case 0:
            this.AddCircleSegment(this.gammaRight, this.vaRight, true, this.rightCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
            break;
          case 1:
            this.AddCircleSegment(this.gammaLeft, this.vaLeft, false, this.leftCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
            break;
          case 2:
            this.AddCircleSegment(this.preVaRight, this.alfaRightRight - Math.PI / 2.0, true, this.preRightCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
            this.AddCircleSegment(this.alfaRightRight - Math.PI / 2.0, this.vaRight, true, this.rightCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
            break;
          case 3:
            this.AddCircleSegment(this.preVaRight, this.alfaRightLeft - this.deltaRightLeft, true, this.preRightCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
            this.AddCircleSegment(this.alfaRightLeft - this.deltaRightLeft + Math.PI, this.vaLeft, false, this.leftCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
            break;
          case 4:
            this.AddCircleSegment(this.preVaLeft, this.alfaLeftRight + this.deltaLeftRight, false, this.preLeftCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
            this.AddCircleSegment(this.alfaLeftRight + this.deltaLeftRight + Math.PI, this.vaRight, true, this.rightCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
            break;
          case 5:
            this.AddCircleSegment(this.preVaLeft, this.alfaLeftLeft + Math.PI / 2.0, false, this.preLeftCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
            this.AddCircleSegment(this.alfaLeftLeft + Math.PI / 2.0, this.vaLeft, false, this.leftCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
            break;
          case 6:
            this.AddCircleSegment(this.vaRight, this.gammaRight, true, this.rightCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
            break;
          case 7:
            this.AddCircleSegment(this.vaLeft, this.gammaLeft, false, this.leftCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
            break;
        }
      }
    }

    [Serializable]
    public class ConstantTurn : AdvancedSmooth.TurnConstructor
    {
      private Vector3 circleCenter;
      private double gamma1;
      private double gamma2;
      private bool clockwise;

      public override void Prepare(int i, Vector3[] vectorPath)
      {
      }

      public override void TangentToTangent(List<AdvancedSmooth.Turn> turnList)
      {
        Vector3 dir1 = Vector3.Cross(AdvancedSmooth.TurnConstructor.t1, Vector3.up);
        Vector3 lhs = AdvancedSmooth.TurnConstructor.current - AdvancedSmooth.TurnConstructor.prev;
        Vector3 start2 = lhs * 0.5f + AdvancedSmooth.TurnConstructor.prev;
        Vector3 dir2 = Vector3.Cross(lhs, Vector3.up);
        bool intersects;
        this.circleCenter = Polygon.IntersectionPointOptimized(AdvancedSmooth.TurnConstructor.prev, dir1, start2, dir2, out intersects);
        if (!intersects)
          return;
        this.gamma1 = this.Atan2(AdvancedSmooth.TurnConstructor.prev - this.circleCenter);
        this.gamma2 = this.Atan2(AdvancedSmooth.TurnConstructor.current - this.circleCenter);
        this.clockwise = !Polygon.Left(this.circleCenter, AdvancedSmooth.TurnConstructor.prev, AdvancedSmooth.TurnConstructor.prev + AdvancedSmooth.TurnConstructor.t1);
        double lengthFromAngle = this.GetLengthFromAngle(!this.clockwise ? this.CounterClockwiseAngle(this.gamma1, this.gamma2) : this.ClockwiseAngle(this.gamma1, this.gamma2), (double) (this.circleCenter - AdvancedSmooth.TurnConstructor.current).magnitude);
        turnList.Add(new AdvancedSmooth.Turn((float) lengthFromAngle, (AdvancedSmooth.TurnConstructor) this, 0));
      }

      public override void GetPath(AdvancedSmooth.Turn turn, List<Vector3> output)
      {
        this.AddCircleSegment(this.gamma1, this.gamma2, this.clockwise, this.circleCenter, output, (this.circleCenter - AdvancedSmooth.TurnConstructor.current).magnitude);
        AdvancedSmooth.TurnConstructor.normal = (AdvancedSmooth.TurnConstructor.current - this.circleCenter).normalized;
        AdvancedSmooth.TurnConstructor.t2 = Vector3.Cross(AdvancedSmooth.TurnConstructor.normal, Vector3.up).normalized;
        AdvancedSmooth.TurnConstructor.normal = -AdvancedSmooth.TurnConstructor.normal;
        if (!this.clockwise)
        {
          AdvancedSmooth.TurnConstructor.t2 = -AdvancedSmooth.TurnConstructor.t2;
          AdvancedSmooth.TurnConstructor.normal = -AdvancedSmooth.TurnConstructor.normal;
        }
        AdvancedSmooth.TurnConstructor.changedPreviousTangent = true;
      }
    }

    public abstract class TurnConstructor
    {
      public static float turningRadius = 1f;
      public float factorBias = 1f;
      public const double ThreeSixtyRadians = 6.28318530717959;
      public float constantBias;
      public static Vector3 prev;
      public static Vector3 current;
      public static Vector3 next;
      public static Vector3 t1;
      public static Vector3 t2;
      public static Vector3 normal;
      public static Vector3 prevNormal;
      public static bool changedPreviousTangent;

      public abstract void Prepare(int i, Vector3[] vectorPath);

      public virtual void OnTangentUpdate()
      {
      }

      public virtual void PointToTangent(List<AdvancedSmooth.Turn> turnList)
      {
      }

      public virtual void TangentToPoint(List<AdvancedSmooth.Turn> turnList)
      {
      }

      public virtual void TangentToTangent(List<AdvancedSmooth.Turn> turnList)
      {
      }

      public abstract void GetPath(AdvancedSmooth.Turn turn, List<Vector3> output);

      public static void Setup(int i, Vector3[] vectorPath)
      {
        AdvancedSmooth.TurnConstructor.current = vectorPath[i];
        AdvancedSmooth.TurnConstructor.prev = vectorPath[i - 1];
        AdvancedSmooth.TurnConstructor.next = vectorPath[i + 1];
        AdvancedSmooth.TurnConstructor.prev.y = AdvancedSmooth.TurnConstructor.current.y;
        AdvancedSmooth.TurnConstructor.next.y = AdvancedSmooth.TurnConstructor.current.y;
        AdvancedSmooth.TurnConstructor.t1 = AdvancedSmooth.TurnConstructor.t2;
        AdvancedSmooth.TurnConstructor.t2 = (AdvancedSmooth.TurnConstructor.next - AdvancedSmooth.TurnConstructor.current).normalized - (AdvancedSmooth.TurnConstructor.prev - AdvancedSmooth.TurnConstructor.current).normalized;
        AdvancedSmooth.TurnConstructor.t2 = AdvancedSmooth.TurnConstructor.t2.normalized;
        AdvancedSmooth.TurnConstructor.prevNormal = AdvancedSmooth.TurnConstructor.normal;
        AdvancedSmooth.TurnConstructor.normal = Vector3.Cross(AdvancedSmooth.TurnConstructor.t2, Vector3.up);
        AdvancedSmooth.TurnConstructor.normal = AdvancedSmooth.TurnConstructor.normal.normalized;
      }

      public static void PostPrepare()
      {
        AdvancedSmooth.TurnConstructor.changedPreviousTangent = false;
      }

      public void AddCircleSegment(double startAngle, double endAngle, bool clockwise, Vector3 center, List<Vector3> output, float radius)
      {
        double num = Math.PI / 50.0;
        if (clockwise)
        {
          while (endAngle > startAngle + 2.0 * Math.PI)
            endAngle -= 2.0 * Math.PI;
          while (endAngle < startAngle)
            endAngle += 2.0 * Math.PI;
        }
        else
        {
          while (endAngle < startAngle - 2.0 * Math.PI)
            endAngle += 2.0 * Math.PI;
          while (endAngle > startAngle)
            endAngle -= 2.0 * Math.PI;
        }
        if (clockwise)
        {
          double a = startAngle;
          while (a < endAngle)
          {
            output.Add(this.AngleToVector(a) * radius + center);
            a += num;
          }
        }
        else
        {
          double a = startAngle;
          while (a > endAngle)
          {
            output.Add(this.AngleToVector(a) * radius + center);
            a -= num;
          }
        }
        output.Add(this.AngleToVector(endAngle) * radius + center);
      }

      public void DebugCircleSegment(Vector3 center, double startAngle, double endAngle, double radius, Color color)
      {
        double num = Math.PI / 50.0;
        while (endAngle < startAngle)
          endAngle += 2.0 * Math.PI;
        Vector3 start = this.AngleToVector(startAngle) * (float) radius + center;
        double a = startAngle + num;
        while (a < endAngle)
        {
          Debug.DrawLine(start, this.AngleToVector(a) * (float) radius + center);
          a += num;
        }
        Debug.DrawLine(start, this.AngleToVector(endAngle) * (float) radius + center);
      }

      public void DebugCircle(Vector3 center, double radius, Color color)
      {
        double num = Math.PI / 50.0;
        Vector3 start = this.AngleToVector(-num) * (float) radius + center;
        double a = 0.0;
        while (a < 2.0 * Math.PI)
        {
          Vector3 end = this.AngleToVector(a) * (float) radius + center;
          Debug.DrawLine(start, end, color);
          start = end;
          a += num;
        }
      }

      public double GetLengthFromAngle(double angle, double radius)
      {
        return radius * angle;
      }

      public double ClockwiseAngle(double from, double to)
      {
        return this.ClampAngle(to - from);
      }

      public double CounterClockwiseAngle(double from, double to)
      {
        return this.ClampAngle(from - to);
      }

      public Vector3 AngleToVector(double a)
      {
        return new Vector3((float) Math.Cos(a), 0.0f, (float) Math.Sin(a));
      }

      public double ToDegrees(double rad)
      {
        return rad * 57.2957801818848;
      }

      public double ClampAngle(double a)
      {
        while (a < 0.0)
          a += 2.0 * Math.PI;
        while (a > 2.0 * Math.PI)
          a -= 2.0 * Math.PI;
        return a;
      }

      public double Atan2(Vector3 v)
      {
        return Math.Atan2((double) v.z, (double) v.x);
      }
    }

    public struct Turn : IComparable<AdvancedSmooth.Turn>
    {
      public float length;
      public int id;
      public AdvancedSmooth.TurnConstructor constructor;

      public float score
      {
        get
        {
          return this.length * this.constructor.factorBias + this.constructor.constantBias;
        }
      }

      public Turn(float length, AdvancedSmooth.TurnConstructor constructor, int id = 0)
      {
        this.length = length;
        this.id = id;
        this.constructor = constructor;
      }

      public static bool operator <(AdvancedSmooth.Turn lhs, AdvancedSmooth.Turn rhs)
      {
        return (double) lhs.score < (double) rhs.score;
      }

      public static bool operator >(AdvancedSmooth.Turn lhs, AdvancedSmooth.Turn rhs)
      {
        return (double) lhs.score > (double) rhs.score;
      }

      public void GetPath(List<Vector3> output)
      {
        this.constructor.GetPath(this, output);
      }

      public int CompareTo(AdvancedSmooth.Turn t)
      {
        if ((double) t.score > (double) this.score)
          return -1;
        return (double) t.score < (double) this.score ? 1 : 0;
      }
    }
  }
}
