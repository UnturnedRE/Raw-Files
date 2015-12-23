// Decompiled with JetBrains decompiler
// Type: Pathfinding.StartEndModifier
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

namespace Pathfinding
{
  [Serializable]
  public class StartEndModifier : PathModifier
  {
    public StartEndModifier.Exactness exactStartPoint = StartEndModifier.Exactness.ClosestOnNode;
    public StartEndModifier.Exactness exactEndPoint = StartEndModifier.Exactness.ClosestOnNode;
    public LayerMask mask = (LayerMask) -1;
    public bool addPoints;
    public bool useRaycasting;
    public bool useGraphRaycasting;

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
        return (ModifierData) ((!this.addPoints ? 4 : 0) | 8);
      }
    }

    public override void Apply(Path _p, ModifierData source)
    {
      ABPath abPath = _p as ABPath;
      if (abPath == null || abPath.vectorPath.Count == 0)
        return;
      if (abPath.vectorPath.Count < 2 && !this.addPoints)
        abPath.vectorPath.Add(abPath.vectorPath[0]);
      Vector3 zero1 = Vector3.zero;
      Vector3 zero2 = Vector3.zero;
      Vector3 vector3_1;
      if (this.exactStartPoint == StartEndModifier.Exactness.Original)
        vector3_1 = this.GetClampedPoint((Vector3) abPath.path[0].position, abPath.originalStartPoint, abPath.path[0]);
      else if (this.exactStartPoint == StartEndModifier.Exactness.ClosestOnNode)
        vector3_1 = this.GetClampedPoint((Vector3) abPath.path[0].position, abPath.startPoint, abPath.path[0]);
      else if (this.exactStartPoint == StartEndModifier.Exactness.Interpolate)
      {
        Vector3 clampedPoint = this.GetClampedPoint((Vector3) abPath.path[0].position, abPath.originalStartPoint, abPath.path[0]);
        vector3_1 = AstarMath.NearestPointStrict((Vector3) abPath.path[0].position, (Vector3) abPath.path[1 < abPath.path.Count ? 1 : 0].position, clampedPoint);
      }
      else
        vector3_1 = (Vector3) abPath.path[0].position;
      Vector3 vector3_2;
      if (this.exactEndPoint == StartEndModifier.Exactness.Original)
        vector3_2 = this.GetClampedPoint((Vector3) abPath.path[abPath.path.Count - 1].position, abPath.originalEndPoint, abPath.path[abPath.path.Count - 1]);
      else if (this.exactEndPoint == StartEndModifier.Exactness.ClosestOnNode)
        vector3_2 = this.GetClampedPoint((Vector3) abPath.path[abPath.path.Count - 1].position, abPath.endPoint, abPath.path[abPath.path.Count - 1]);
      else if (this.exactEndPoint == StartEndModifier.Exactness.Interpolate)
      {
        Vector3 clampedPoint = this.GetClampedPoint((Vector3) abPath.path[abPath.path.Count - 1].position, abPath.originalEndPoint, abPath.path[abPath.path.Count - 1]);
        vector3_2 = AstarMath.NearestPointStrict((Vector3) abPath.path[abPath.path.Count - 1].position, (Vector3) abPath.path[abPath.path.Count - 2 >= 0 ? abPath.path.Count - 2 : 0].position, clampedPoint);
      }
      else
        vector3_2 = (Vector3) abPath.path[abPath.path.Count - 1].position;
      if (!this.addPoints)
      {
        abPath.vectorPath[0] = vector3_1;
        abPath.vectorPath[abPath.vectorPath.Count - 1] = vector3_2;
      }
      else
      {
        if (this.exactStartPoint != StartEndModifier.Exactness.SnapToNode)
          abPath.vectorPath.Insert(0, vector3_1);
        if (this.exactEndPoint == StartEndModifier.Exactness.SnapToNode)
          return;
        abPath.vectorPath.Add(vector3_2);
      }
    }

    public Vector3 GetClampedPoint(Vector3 from, Vector3 to, GraphNode hint)
    {
      Vector3 end = to;
      RaycastHit hitInfo;
      if (this.useRaycasting && Physics.Linecast(from, to, out hitInfo, (int) this.mask))
        end = hitInfo.point;
      if (this.useGraphRaycasting && hint != null)
      {
        NavGraph graph = AstarData.GetGraph(hint);
        if (graph != null)
        {
          IRaycastableGraph raycastableGraph = graph as IRaycastableGraph;
          GraphHitInfo hit;
          if (raycastableGraph != null && raycastableGraph.Linecast(from, end, hint, out hit))
            end = hit.point;
        }
      }
      return end;
    }

    public enum Exactness
    {
      SnapToNode,
      Original,
      Interpolate,
      ClosestOnNode,
    }
  }
}
