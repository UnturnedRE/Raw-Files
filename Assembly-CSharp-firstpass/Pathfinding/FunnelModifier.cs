// Decompiled with JetBrains decompiler
// Type: Pathfinding.FunnelModifier
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
  [AddComponentMenu("Pathfinding/Modifiers/Funnel")]
  [Serializable]
  public class FunnelModifier : MonoModifier
  {
    public override ModifierData input
    {
      get
      {
        return ModifierData.StrictVectorPath;
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
      List<GraphNode> list1 = p.path;
      List<Vector3> list2 = p.vectorPath;
      if (list1 == null || list1.Count == 0 || (list2 == null || list2.Count != list1.Count))
        return;
      List<Vector3> funnelPath = ListPool<Vector3>.Claim();
      List<Vector3> list3 = ListPool<Vector3>.Claim(list1.Count + 1);
      List<Vector3> list4 = ListPool<Vector3>.Claim(list1.Count + 1);
      list3.Add(list2[0]);
      list4.Add(list2[0]);
      for (int index = 0; index < list1.Count - 1; ++index)
      {
        if (!list1[index].GetPortal(list1[index + 1], list3, list4, false))
        {
          list3.Add((Vector3) list1[index].position);
          list4.Add((Vector3) list1[index].position);
          list3.Add((Vector3) list1[index + 1].position);
          list4.Add((Vector3) list1[index + 1].position);
        }
      }
      list3.Add(list2[list2.Count - 1]);
      list4.Add(list2[list2.Count - 1]);
      if (!this.RunFunnel(list3, list4, funnelPath))
      {
        funnelPath.Add(list2[0]);
        funnelPath.Add(list2[list2.Count - 1]);
      }
      ListPool<Vector3>.Release(p.vectorPath);
      p.vectorPath = funnelPath;
      ListPool<Vector3>.Release(list3);
      ListPool<Vector3>.Release(list4);
    }

    public bool RunFunnel(List<Vector3> left, List<Vector3> right, List<Vector3> funnelPath)
    {
      if (left == null)
        throw new ArgumentNullException("left");
      if (right == null)
        throw new ArgumentNullException("right");
      if (funnelPath == null)
        throw new ArgumentNullException("funnelPath");
      if (left.Count != right.Count)
        throw new ArgumentException("left and right lists must have equal length");
      if (left.Count <= 3)
        return false;
      while (left[1] == left[2] && right[1] == right[2])
      {
        left.RemoveAt(1);
        right.RemoveAt(1);
        if (left.Count <= 3)
          return false;
      }
      Vector3 p = left[2];
      if (p == left[1])
        p = right[2];
      while (Polygon.IsColinear(left[0], left[1], right[1]) || Polygon.Left(left[1], right[1], p) == Polygon.Left(left[1], right[1], left[0]))
      {
        left.RemoveAt(1);
        right.RemoveAt(1);
        if (left.Count <= 3)
          return false;
        p = left[2];
        if (p == left[1])
          p = right[2];
      }
      if (!Polygon.IsClockwise(left[0], left[1], right[1]) && !Polygon.IsColinear(left[0], left[1], right[1]))
      {
        List<Vector3> list = left;
        left = right;
        right = list;
      }
      funnelPath.Add(left[0]);
      Vector3 a = left[0];
      Vector3 b1 = left[1];
      Vector3 b2 = right[1];
      int num1 = 1;
      int num2 = 1;
      for (int index = 2; index < left.Count; ++index)
      {
        if (funnelPath.Count > 2000)
        {
          Debug.LogWarning((object) "Avoiding infinite loop. Remove this check if you have this long paths.");
          break;
        }
        Vector3 c1 = left[index];
        Vector3 c2 = right[index];
        if ((double) Polygon.TriangleArea2(a, b2, c2) >= 0.0)
        {
          if (a == b2 || (double) Polygon.TriangleArea2(a, b1, c2) <= 0.0)
          {
            b2 = c2;
            num1 = index;
          }
          else
          {
            funnelPath.Add(b1);
            a = b1;
            int num3 = num2;
            b1 = a;
            b2 = a;
            num2 = num3;
            num1 = num3;
            index = num3;
            continue;
          }
        }
        if ((double) Polygon.TriangleArea2(a, b1, c1) <= 0.0)
        {
          if (a == b1 || (double) Polygon.TriangleArea2(a, b2, c1) >= 0.0)
          {
            b1 = c1;
            num2 = index;
          }
          else
          {
            funnelPath.Add(b2);
            a = b2;
            int num3 = num1;
            b1 = a;
            b2 = a;
            num2 = num3;
            num1 = num3;
            index = num3;
          }
        }
      }
      funnelPath.Add(left[left.Count - 1]);
      return true;
    }
  }
}
