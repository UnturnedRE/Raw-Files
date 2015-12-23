// Decompiled with JetBrains decompiler
// Type: Pathfinding.NodeLink
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

namespace Pathfinding
{
  [AddComponentMenu("Pathfinding/Link")]
  public class NodeLink : GraphModifier
  {
    public float costFactor = 1f;
    public Transform end;
    public bool oneWay;
    public bool deleteConnection;

    public Transform Start
    {
      get
      {
        return this.transform;
      }
    }

    public Transform End
    {
      get
      {
        return this.end;
      }
    }

    public override void OnPostScan()
    {
      if (AstarPath.active.isScanning)
        this.InternalOnPostScan();
      else
        AstarPath.active.AddWorkItem(new AstarPath.AstarWorkItem((Func<bool, bool>) (force =>
        {
          this.InternalOnPostScan();
          return true;
        })));
    }

    public void InternalOnPostScan()
    {
      this.Apply();
    }

    public override void OnGraphsPostUpdate()
    {
      if (AstarPath.active.isScanning)
        return;
      AstarPath.active.AddWorkItem(new AstarPath.AstarWorkItem((Func<bool, bool>) (force =>
      {
        this.InternalOnPostScan();
        return true;
      })));
    }

    public virtual void Apply()
    {
      if ((UnityEngine.Object) this.Start == (UnityEngine.Object) null || (UnityEngine.Object) this.End == (UnityEngine.Object) null || (UnityEngine.Object) AstarPath.active == (UnityEngine.Object) null)
        return;
      GraphNode node1 = AstarPath.active.GetNearest(this.Start.position).node;
      GraphNode node2 = AstarPath.active.GetNearest(this.End.position).node;
      if (node1 == null || node2 == null)
        return;
      if (this.deleteConnection)
      {
        node1.RemoveConnection(node2);
        if (this.oneWay)
          return;
        node2.RemoveConnection(node1);
      }
      else
      {
        uint cost = (uint) Math.Round((double) (node1.position - node2.position).costMagnitude * (double) this.costFactor);
        node1.AddConnection(node2, cost);
        if (this.oneWay)
          return;
        node2.AddConnection(node1, cost);
      }
    }

    public void OnDrawGizmos()
    {
      if ((UnityEngine.Object) this.Start == (UnityEngine.Object) null || (UnityEngine.Object) this.End == (UnityEngine.Object) null)
        return;
      Vector3 position1 = this.Start.position;
      Vector3 position2 = this.End.position;
      Gizmos.color = !this.deleteConnection ? Color.green : Color.red;
      this.DrawGizmoBezier(position1, position2);
    }

    private void DrawGizmoBezier(Vector3 p1, Vector3 p2)
    {
      Vector3 vector3_1 = p2 - p1;
      if (vector3_1 == Vector3.zero)
        return;
      Vector3 rhs = Vector3.Cross(Vector3.up, vector3_1);
      Vector3 vector3_2 = Vector3.Cross(vector3_1, rhs);
      vector3_2 = vector3_2.normalized;
      vector3_2 *= vector3_1.magnitude * 0.1f;
      Vector3 p1_1 = p1 + vector3_2;
      Vector3 p2_1 = p2 + vector3_2;
      Vector3 from = p1;
      for (int index = 1; index <= 20; ++index)
      {
        float t = (float) index / 20f;
        Vector3 to = AstarMath.CubicBezier(p1, p1_1, p2_1, p2, t);
        Gizmos.DrawLine(from, to);
        from = to;
      }
    }
  }
}
