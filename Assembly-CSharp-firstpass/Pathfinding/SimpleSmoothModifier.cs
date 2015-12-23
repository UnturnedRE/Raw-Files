// Decompiled with JetBrains decompiler
// Type: Pathfinding.SimpleSmoothModifier
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
  [AddComponentMenu("Pathfinding/Modifiers/Simple Smooth")]
  [Serializable]
  public class SimpleSmoothModifier : MonoModifier
  {
    public int subdivisions = 2;
    public int iterations = 2;
    public float strength = 0.5f;
    public bool uniformLength = true;
    public float maxSegmentLength = 2f;
    public float bezierTangentLength = 0.4f;
    public float offset = 0.2f;
    public float factor = 0.1f;
    public SimpleSmoothModifier.SmoothType smoothType;

    public override ModifierData input
    {
      get
      {
        return ModifierData.All;
      }
    }

    public override ModifierData output
    {
      get
      {
        ModifierData modifierData = ModifierData.VectorPath;
        if (this.iterations == 0 && this.smoothType == SimpleSmoothModifier.SmoothType.Simple && !this.uniformLength)
          modifierData |= ModifierData.StrictVectorPath;
        return modifierData;
      }
    }

    public override void Apply(Path p, ModifierData source)
    {
      if (p.vectorPath == null)
      {
        Debug.LogWarning((object) "Can't process NULL path (has another modifier logged an error?)");
      }
      else
      {
        List<Vector3> list = (List<Vector3>) null;
        switch (this.smoothType)
        {
          case SimpleSmoothModifier.SmoothType.Simple:
            list = this.SmoothSimple(p.vectorPath);
            break;
          case SimpleSmoothModifier.SmoothType.Bezier:
            list = this.SmoothBezier(p.vectorPath);
            break;
          case SimpleSmoothModifier.SmoothType.OffsetSimple:
            list = this.SmoothOffsetSimple(p.vectorPath);
            break;
          case SimpleSmoothModifier.SmoothType.CurvedNonuniform:
            list = this.CurvedNonuniform(p.vectorPath);
            break;
        }
        if (list == p.vectorPath)
          return;
        ListPool<Vector3>.Release(p.vectorPath);
        p.vectorPath = list;
      }
    }

    public List<Vector3> CurvedNonuniform(List<Vector3> path)
    {
      if ((double) this.maxSegmentLength <= 0.0)
      {
        Debug.LogWarning((object) "Max Segment Length is <= 0 which would cause DivByZero-exception or other nasty errors (avoid this)");
        return path;
      }
      int capacity = 0;
      for (int index = 0; index < path.Count - 1; ++index)
      {
        float magnitude = (path[index] - path[index + 1]).magnitude;
        float num = 0.0f;
        while ((double) num <= (double) magnitude)
        {
          ++capacity;
          num += this.maxSegmentLength;
        }
      }
      List<Vector3> list = ListPool<Vector3>.Claim(capacity);
      Vector3 vector3_1 = (path[1] - path[0]).normalized;
      for (int index = 0; index < path.Count - 1; ++index)
      {
        float magnitude = (path[index] - path[index + 1]).magnitude;
        Vector3 vector3_2 = vector3_1;
        Vector3 vector3_3 = index >= path.Count - 2 ? (path[index + 1] - path[index]).normalized : ((path[index + 2] - path[index + 1]).normalized - (path[index] - path[index + 1]).normalized).normalized;
        Vector3 tan1 = vector3_2 * magnitude * this.factor;
        Vector3 tan2 = vector3_3 * magnitude * this.factor;
        Vector3 a = path[index];
        Vector3 b = path[index + 1];
        float num1 = 1f / magnitude;
        float num2 = 0.0f;
        while ((double) num2 <= (double) magnitude)
        {
          float t = num2 * num1;
          list.Add(SimpleSmoothModifier.GetPointOnCubic(a, b, tan1, tan2, t));
          num2 += this.maxSegmentLength;
        }
        vector3_1 = vector3_3;
      }
      list[list.Count - 1] = path[path.Count - 1];
      return list;
    }

    public static Vector3 GetPointOnCubic(Vector3 a, Vector3 b, Vector3 tan1, Vector3 tan2, float t)
    {
      float num1 = t * t;
      float num2 = num1 * t;
      float num3 = (float) (2.0 * (double) num2 - 3.0 * (double) num1 + 1.0);
      float num4 = (float) (-2.0 * (double) num2 + 3.0 * (double) num1);
      float num5 = num2 - 2f * num1 + t;
      float num6 = num2 - num1;
      return num3 * a + num4 * b + num5 * tan1 + num6 * tan2;
    }

    public List<Vector3> SmoothOffsetSimple(List<Vector3> path)
    {
      if (path.Count <= 2 || this.iterations <= 0)
        return path;
      if (this.iterations > 12)
      {
        Debug.LogWarning((object) "A very high iteration count was passed, won't let this one through");
        return path;
      }
      int capacity = (path.Count - 2) * (int) Mathf.Pow(2f, (float) this.iterations) + 2;
      List<Vector3> list1 = ListPool<Vector3>.Claim(capacity);
      List<Vector3> list2 = ListPool<Vector3>.Claim(capacity);
      for (int index = 0; index < capacity; ++index)
      {
        list1.Add(Vector3.zero);
        list2.Add(Vector3.zero);
      }
      for (int index = 0; index < path.Count; ++index)
        list1[index] = path[index];
      for (int index1 = 0; index1 < this.iterations; ++index1)
      {
        int num1 = (path.Count - 2) * (int) Mathf.Pow(2f, (float) index1) + 2;
        List<Vector3> list3 = list1;
        list1 = list2;
        list2 = list3;
        float num2 = 1f;
        for (int index2 = 0; index2 < num1 - 1; ++index2)
        {
          Vector3 a = list2[index2];
          Vector3 b = list2[index2 + 1];
          Vector3 normalized = Vector3.Cross(b - a, Vector3.up).normalized;
          bool flag1 = false;
          bool flag2 = false;
          bool flag3 = false;
          bool flag4 = false;
          if (index2 != 0 && !Polygon.IsColinear(a, b, list2[index2 - 1]))
          {
            flag3 = true;
            flag1 = Polygon.Left(a, b, list2[index2 - 1]);
          }
          if (index2 < num1 - 1 && !Polygon.IsColinear(a, b, list2[index2 + 2]))
          {
            flag4 = true;
            flag2 = Polygon.Left(a, b, list2[index2 + 2]);
          }
          list1[index2 * 2] = !flag3 ? a : a + (!flag1 ? -normalized * this.offset * num2 : normalized * this.offset * num2);
          list1[index2 * 2 + 1] = !flag4 ? b : b + (!flag2 ? -normalized * this.offset * num2 : normalized * this.offset * num2);
        }
        list1[(path.Count - 2) * (int) Mathf.Pow(2f, (float) (index1 + 1)) + 2 - 1] = list2[num1 - 1];
      }
      ListPool<Vector3>.Release(list2);
      return list1;
    }

    public List<Vector3> SmoothSimple(List<Vector3> path)
    {
      if (path.Count < 2)
        return path;
      if (this.uniformLength)
      {
        int num1 = 0;
        this.maxSegmentLength = (double) this.maxSegmentLength >= 0.00499999988824129 ? this.maxSegmentLength : 0.005f;
        for (int index = 0; index < path.Count - 1; ++index)
        {
          float num2 = Vector3.Distance(path[index], path[index + 1]);
          num1 += Mathf.FloorToInt(num2 / this.maxSegmentLength);
        }
        List<Vector3> list = ListPool<Vector3>.Claim(num1 + 1);
        int num3 = 0;
        float num4 = 0.0f;
        for (int index1 = 0; index1 < path.Count - 1; ++index1)
        {
          float num2 = Vector3.Distance(path[index1], path[index1 + 1]);
          int num5 = Mathf.FloorToInt((num2 + num4) / this.maxSegmentLength);
          float num6 = num4 / num2;
          Vector3 vector3 = path[index1 + 1] - path[index1];
          for (int index2 = 0; index2 < num5; ++index2)
          {
            list.Add(vector3 * Math.Max(0.0f, (float) index2 / (float) num5 - num6) + path[index1]);
            ++num3;
          }
          num4 = (num2 + num4) % this.maxSegmentLength;
        }
        list.Add(path[path.Count - 1]);
        if ((double) this.strength != 0.0)
        {
          for (int index1 = 0; index1 < this.iterations; ++index1)
          {
            Vector3 vector3 = list[0];
            for (int index2 = 1; index2 < list.Count - 1; ++index2)
            {
              Vector3 a = list[index2];
              list[index2] = Vector3.Lerp(a, (vector3 + list[index2 + 1]) / 2f, this.strength);
              vector3 = a;
            }
          }
        }
        return list;
      }
      List<Vector3> list1 = ListPool<Vector3>.Claim();
      if (this.subdivisions < 0)
        this.subdivisions = 0;
      int num = 1 << this.subdivisions;
      for (int index1 = 0; index1 < path.Count - 1; ++index1)
      {
        for (int index2 = 0; index2 < num; ++index2)
          list1.Add(Vector3.Lerp(path[index1], path[index1 + 1], (float) index2 / (float) num));
      }
      for (int index1 = 0; index1 < this.iterations; ++index1)
      {
        Vector3 vector3 = list1[0];
        for (int index2 = 1; index2 < list1.Count - 1; ++index2)
        {
          Vector3 a = list1[index2];
          list1[index2] = Vector3.Lerp(a, (vector3 + list1[index2 + 1]) / 2f, this.strength);
          vector3 = a;
        }
      }
      return list1;
    }

    public List<Vector3> SmoothBezier(List<Vector3> path)
    {
      if (this.subdivisions < 0)
        this.subdivisions = 0;
      int num = 1 << this.subdivisions;
      List<Vector3> list = ListPool<Vector3>.Claim();
      for (int index1 = 0; index1 < path.Count - 1; ++index1)
      {
        Vector3 zero1 = Vector3.zero;
        Vector3 zero2 = Vector3.zero;
        Vector3 vector3_1 = index1 != 0 ? path[index1 + 1] - path[index1 - 1] : path[index1 + 1] - path[index1];
        Vector3 vector3_2 = index1 != path.Count - 2 ? path[index1] - path[index1 + 2] : path[index1] - path[index1 + 1];
        Vector3 vector3_3 = vector3_1 * this.bezierTangentLength;
        Vector3 vector3_4 = vector3_2 * this.bezierTangentLength;
        Vector3 p0 = path[index1];
        Vector3 p1 = p0 + vector3_3;
        Vector3 p3 = path[index1 + 1];
        Vector3 p2 = p3 + vector3_4;
        for (int index2 = 0; index2 < num; ++index2)
          list.Add(AstarMath.CubicBezier(p0, p1, p2, p3, (float) index2 / (float) num));
      }
      list.Add(path[path.Count - 1]);
      return list;
    }

    public enum SmoothType
    {
      Simple,
      Bezier,
      OffsetSimple,
      CurvedNonuniform,
    }
  }
}
