// Decompiled with JetBrains decompiler
// Type: Pathfinding.RaycastModifier
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Pathfinding
{
  [AddComponentMenu("Pathfinding/Modifiers/Raycast Simplifier")]
  [Serializable]
  public class RaycastModifier : MonoModifier
  {
    [HideInInspector]
    public bool useRaycasting = true;
    [HideInInspector]
    public LayerMask mask = (LayerMask) -1;
    [HideInInspector]
    public Vector3 raycastOffset = Vector3.zero;
    public int iterations = 2;
    [HideInInspector]
    public bool thickRaycast;
    [HideInInspector]
    public float thickRaycastRadius;
    [HideInInspector]
    public bool subdivideEveryIter;
    [HideInInspector]
    public bool useGraphRaycasting;
    private static List<Vector3> nodes;

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

    public override void Apply(Path p, ModifierData source)
    {
      if (this.iterations <= 0)
        return;
      if (RaycastModifier.nodes == null)
        RaycastModifier.nodes = new List<Vector3>(p.vectorPath.Count);
      else
        RaycastModifier.nodes.Clear();
      RaycastModifier.nodes.AddRange((IEnumerable<Vector3>) p.vectorPath);
      for (int index1 = 0; index1 < this.iterations; ++index1)
      {
        if (this.subdivideEveryIter && index1 != 0)
        {
          if (RaycastModifier.nodes.Capacity < RaycastModifier.nodes.Count * 3)
            RaycastModifier.nodes.Capacity = RaycastModifier.nodes.Count * 3;
          int count = RaycastModifier.nodes.Count;
          for (int index2 = 0; index2 < count - 1; ++index2)
          {
            RaycastModifier.nodes.Add(Vector3.zero);
            RaycastModifier.nodes.Add(Vector3.zero);
          }
          for (int index2 = count - 1; index2 > 0; --index2)
          {
            Vector3 a = RaycastModifier.nodes[index2];
            Vector3 b = RaycastModifier.nodes[index2 + 1];
            RaycastModifier.nodes[index2 * 3] = RaycastModifier.nodes[index2];
            if (index2 != count - 1)
            {
              RaycastModifier.nodes[index2 * 3 + 1] = Vector3.Lerp(a, b, 0.33f);
              RaycastModifier.nodes[index2 * 3 + 2] = Vector3.Lerp(a, b, 0.66f);
            }
          }
        }
        int index3 = 0;
        while (index3 < RaycastModifier.nodes.Count - 2)
        {
          Vector3 v1 = RaycastModifier.nodes[index3];
          Vector3 v2 = RaycastModifier.nodes[index3 + 2];
          Stopwatch stopwatch = new Stopwatch();
          stopwatch.Start();
          if (this.ValidateLine((GraphNode) null, (GraphNode) null, v1, v2))
            RaycastModifier.nodes.RemoveAt(index3 + 1);
          else
            ++index3;
          stopwatch.Stop();
        }
      }
      p.vectorPath.Clear();
      p.vectorPath.AddRange((IEnumerable<Vector3>) RaycastModifier.nodes);
    }

    public bool ValidateLine(GraphNode n1, GraphNode n2, Vector3 v1, Vector3 v2)
    {
      if (this.useRaycasting)
      {
        if (this.thickRaycast && (double) this.thickRaycastRadius > 0.0)
        {
          RaycastHit hitInfo;
          if (Physics.SphereCast(v1 + this.raycastOffset, this.thickRaycastRadius, v2 - v1, out hitInfo, (v2 - v1).magnitude, (int) this.mask))
            return false;
        }
        else
        {
          RaycastHit hitInfo;
          if (Physics.Linecast(v1 + this.raycastOffset, v2 + this.raycastOffset, out hitInfo, (int) this.mask))
            return false;
        }
      }
      if (this.useGraphRaycasting && n1 == null)
      {
        n1 = AstarPath.active.GetNearest(v1).node;
        n2 = AstarPath.active.GetNearest(v2).node;
      }
      if (this.useGraphRaycasting && n1 != null && n2 != null)
      {
        NavGraph graph1 = AstarData.GetGraph(n1);
        NavGraph graph2 = AstarData.GetGraph(n2);
        if (graph1 != graph2)
          return false;
        if (graph1 != null)
        {
          IRaycastableGraph raycastableGraph = graph1 as IRaycastableGraph;
          if (raycastableGraph != null && raycastableGraph.Linecast(v1, v2, n1))
            return false;
        }
      }
      return true;
    }
  }
}
