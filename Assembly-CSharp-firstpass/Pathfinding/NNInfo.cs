// Decompiled with JetBrains decompiler
// Type: Pathfinding.NNInfo
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

namespace Pathfinding
{
  public struct NNInfo
  {
    public GraphNode node;
    public GraphNode constrainedNode;
    public Vector3 clampedPosition;
    public Vector3 constClampedPosition;

    public NNInfo(GraphNode node)
    {
      this.node = node;
      this.constrainedNode = (GraphNode) null;
      this.constClampedPosition = Vector3.zero;
      if (node != null)
        this.clampedPosition = (Vector3) node.position;
      else
        this.clampedPosition = Vector3.zero;
    }

    public static explicit operator Vector3(NNInfo ob)
    {
      return ob.clampedPosition;
    }

    public static explicit operator GraphNode(NNInfo ob)
    {
      return ob.node;
    }

    public static explicit operator NNInfo(GraphNode ob)
    {
      return new NNInfo(ob);
    }

    public void SetConstrained(GraphNode constrainedNode, Vector3 clampedPosition)
    {
      this.constrainedNode = constrainedNode;
      this.constClampedPosition = clampedPosition;
    }

    public void UpdateInfo()
    {
      this.clampedPosition = this.node == null ? Vector3.zero : (Vector3) this.node.position;
      if (this.constrainedNode != null)
        this.constClampedPosition = (Vector3) this.constrainedNode.position;
      else
        this.constClampedPosition = Vector3.zero;
    }
  }
}
