// Decompiled with JetBrains decompiler
// Type: RadiusModifier
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding;
using Pathfinding.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Pathfinding/Modifiers/Radius Offset")]
public class RadiusModifier : MonoModifier
{
  public float radius = 1f;
  public float detail = 10f;
  private float[] radi = new float[10];
  private float[] a1 = new float[10];
  private float[] a2 = new float[10];
  private bool[] dir = new bool[10];

  public override ModifierData input
  {
    get
    {
      return ModifierData.Vector;
    }
  }

  public override ModifierData output
  {
    get
    {
      return ModifierData.VectorPath;
    }
  }

  private bool CalculateCircleInner(Vector3 p1, Vector3 p2, float r1, float r2, out float a, out float sigma)
  {
    float magnitude = (p1 - p2).magnitude;
    if ((double) r1 + (double) r2 > (double) magnitude)
    {
      a = 0.0f;
      sigma = 0.0f;
      return false;
    }
    a = (float) Math.Acos(((double) r1 + (double) r2) / (double) magnitude);
    sigma = (float) Math.Atan2((double) p2.z - (double) p1.z, (double) p2.x - (double) p1.x);
    return true;
  }

  private bool CalculateCircleOuter(Vector3 p1, Vector3 p2, float r1, float r2, out float a, out float sigma)
  {
    float magnitude = (p1 - p2).magnitude;
    if ((double) Math.Abs(r1 - r2) > (double) magnitude)
    {
      a = 0.0f;
      sigma = 0.0f;
      return false;
    }
    a = (float) Math.Acos(((double) r1 - (double) r2) / (double) magnitude);
    sigma = (float) Math.Atan2((double) p2.z - (double) p1.z, (double) p2.x - (double) p1.x);
    return true;
  }

  private RadiusModifier.TangentType CalculateTangentType(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
  {
    return (RadiusModifier.TangentType) (1 << (!Polygon.Left(p1, p2, p3) ? 0 : 2) + (!Polygon.Left(p2, p3, p4) ? 0 : 1));
  }

  private RadiusModifier.TangentType CalculateTangentTypeSimple(Vector3 p1, Vector3 p2, Vector3 p3)
  {
    bool flag = Polygon.Left(p1, p2, p3);
    return (RadiusModifier.TangentType) (1 << (!flag ? 0 : 2) + (!flag ? 0 : 1));
  }

  private void DrawCircle(Vector3 p1, float rad, Color col, float start = 0, float end = 6.283185f)
  {
    Vector3 start1 = new Vector3((float) Math.Cos((double) start), 0.0f, (float) Math.Sin((double) start)) * rad + p1;
    float num = start;
    while ((double) num < (double) end)
    {
      Vector3 end1 = new Vector3((float) Math.Cos((double) num), 0.0f, (float) Math.Sin((double) num)) * rad + p1;
      Debug.DrawLine(start1, end1, col);
      start1 = end1;
      num += 0.03141593f;
    }
    if ((double) end != 2.0 * Math.PI)
      return;
    Vector3 end2 = new Vector3((float) Math.Cos((double) start), 0.0f, (float) Math.Sin((double) start)) * rad + p1;
    Debug.DrawLine(start1, end2, col);
  }

  public override void Apply(Path p, ModifierData source)
  {
    List<Vector3> vs = p.vectorPath;
    List<Vector3> list = this.Apply(vs);
    if (list == vs)
      return;
    ListPool<Vector3>.Release(p.vectorPath);
    p.vectorPath = list;
  }

  public List<Vector3> Apply(List<Vector3> vs)
  {
    if (vs == null || vs.Count < 3)
      return vs;
    if (this.radi.Length < vs.Count)
    {
      this.radi = new float[vs.Count];
      this.a1 = new float[vs.Count];
      this.a2 = new float[vs.Count];
      this.dir = new bool[vs.Count];
    }
    for (int index = 0; index < vs.Count; ++index)
      this.radi[index] = this.radius;
    this.radi[0] = 0.0f;
    this.radi[vs.Count - 1] = 0.0f;
    int num1 = 0;
    for (int index = 0; index < vs.Count - 1; ++index)
    {
      ++num1;
      if (num1 > 2 * vs.Count)
      {
        Debug.LogWarning((object) "Could not resolve radiuses, the path is too complex. Try reducing the base radius");
        break;
      }
      RadiusModifier.TangentType tangentType = index != 0 ? (index != vs.Count - 2 ? this.CalculateTangentType(vs[index - 1], vs[index], vs[index + 1], vs[index + 2]) : this.CalculateTangentTypeSimple(vs[index - 1], vs[index], vs[index + 1])) : this.CalculateTangentTypeSimple(vs[index], vs[index + 1], vs[index + 2]);
      if ((tangentType & RadiusModifier.TangentType.Inner) != (RadiusModifier.TangentType) 0)
      {
        float a;
        float sigma;
        if (!this.CalculateCircleInner(vs[index], vs[index + 1], this.radi[index], this.radi[index + 1], out a, out sigma))
        {
          float magnitude = (vs[index + 1] - vs[index]).magnitude;
          this.radi[index] = magnitude * (this.radi[index] / (this.radi[index] + this.radi[index + 1]));
          this.radi[index + 1] = magnitude - this.radi[index];
          this.radi[index] *= 0.99f;
          this.radi[index + 1] *= 0.99f;
          index -= 2;
        }
        else if (tangentType == RadiusModifier.TangentType.InnerRightLeft)
        {
          this.a2[index] = sigma - a;
          this.a1[index + 1] = (float) ((double) sigma - (double) a + 3.14159274101257);
          this.dir[index] = true;
        }
        else
        {
          this.a2[index] = sigma + a;
          this.a1[index + 1] = (float) ((double) sigma + (double) a + 3.14159274101257);
          this.dir[index] = false;
        }
      }
      else
      {
        float a;
        float sigma;
        if (!this.CalculateCircleOuter(vs[index], vs[index + 1], this.radi[index], this.radi[index + 1], out a, out sigma))
        {
          if (index == vs.Count - 2)
          {
            this.radi[index] = (vs[index + 1] - vs[index]).magnitude;
            this.radi[index] *= 0.99f;
            --index;
          }
          else
          {
            this.radi[index + 1] = (double) this.radi[index] <= (double) this.radi[index + 1] ? this.radi[index] + (vs[index + 1] - vs[index]).magnitude : this.radi[index] - (vs[index + 1] - vs[index]).magnitude;
            this.radi[index + 1] *= 0.99f;
          }
          --index;
        }
        else if (tangentType == RadiusModifier.TangentType.OuterRight)
        {
          this.a2[index] = sigma - a;
          this.a1[index + 1] = sigma - a;
          this.dir[index] = true;
        }
        else
        {
          this.a2[index] = sigma + a;
          this.a1[index + 1] = sigma + a;
          this.dir[index] = false;
        }
      }
    }
    List<Vector3> list = ListPool<Vector3>.Claim();
    list.Add(vs[0]);
    if ((double) this.detail < 1.0)
      this.detail = 1f;
    float num2 = 6.283185f / this.detail;
    for (int index = 1; index < vs.Count - 1; ++index)
    {
      float num3 = this.a1[index];
      float num4 = this.a2[index];
      float num5 = this.radi[index];
      if (this.dir[index])
      {
        if ((double) num4 < (double) num3)
          num4 += 6.283185f;
        float num6 = num3;
        while ((double) num6 < (double) num4)
        {
          list.Add(new Vector3((float) Math.Cos((double) num6), 0.0f, (float) Math.Sin((double) num6)) * num5 + vs[index]);
          num6 += num2;
        }
      }
      else
      {
        if ((double) num3 < (double) num4)
          num3 += 6.283185f;
        float num6 = num3;
        while ((double) num6 > (double) num4)
        {
          list.Add(new Vector3((float) Math.Cos((double) num6), 0.0f, (float) Math.Sin((double) num6)) * num5 + vs[index]);
          num6 -= num2;
        }
      }
    }
    list.Add(vs[vs.Count - 1]);
    return list;
  }

  private enum TangentType
  {
    OuterRight = 1,
    InnerRightLeft = 2,
    InnerLeftRight = 4,
    Inner = 6,
    OuterLeft = 8,
    Outer = 9,
  }
}
