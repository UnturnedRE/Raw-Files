// Decompiled with JetBrains decompiler
// Type: Pathfinding.RichSpecial
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

namespace Pathfinding
{
  public class RichSpecial : RichPathPart
  {
    public NodeLink2 nodeLink;
    public Transform first;
    public Transform second;
    public bool reverse;

    public override void OnEnterPool()
    {
      this.nodeLink = (NodeLink2) null;
    }

    public RichSpecial Initialize(NodeLink2 nodeLink, GraphNode first)
    {
      this.nodeLink = nodeLink;
      if (first == nodeLink.StartNode)
      {
        this.first = nodeLink.StartTransform;
        this.second = nodeLink.EndTransform;
        this.reverse = false;
      }
      else
      {
        this.first = nodeLink.EndTransform;
        this.second = nodeLink.StartTransform;
        this.reverse = true;
      }
      return this;
    }
  }
}
