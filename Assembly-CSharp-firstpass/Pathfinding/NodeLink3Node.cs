// Decompiled with JetBrains decompiler
// Type: Pathfinding.NodeLink3Node
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
  public class NodeLink3Node : PointNode
  {
    public NodeLink3 link;
    public Vector3 portalA;
    public Vector3 portalB;

    public NodeLink3Node(AstarPath active)
      : base(active)
    {
    }

    public override bool GetPortal(GraphNode other, List<Vector3> left, List<Vector3> right, bool backwards)
    {
      if (this.connections.Length < 2)
        return false;
      if (this.connections.Length != 2)
        throw new Exception("Invalid NodeLink3Node. Expected 2 connections, found " + (object) this.connections.Length);
      if (left != null)
      {
        left.Add(this.portalA);
        right.Add(this.portalB);
      }
      return true;
    }

    public GraphNode GetOther(GraphNode a)
    {
      if (this.connections.Length < 2)
        return (GraphNode) null;
      if (this.connections.Length != 2)
        throw new Exception("Invalid NodeLink3Node. Expected 2 connections, found " + (object) this.connections.Length);
      if (a == this.connections[0])
        return (this.connections[1] as NodeLink3Node).GetOtherInternal((GraphNode) this);
      return (this.connections[0] as NodeLink3Node).GetOtherInternal((GraphNode) this);
    }

    private GraphNode GetOtherInternal(GraphNode a)
    {
      if (this.connections.Length < 2)
        return (GraphNode) null;
      if (a == this.connections[0])
        return this.connections[1];
      return this.connections[0];
    }
  }
}
