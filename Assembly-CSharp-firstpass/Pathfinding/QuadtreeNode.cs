// Decompiled with JetBrains decompiler
// Type: Pathfinding.QuadtreeNode
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Pathfinding
{
  public class QuadtreeNode : GraphNode
  {
    public GraphNode[] connections;
    public uint[] connectionCosts;

    public QuadtreeNode(AstarPath astar)
      : base(astar)
    {
    }

    public void SetPosition(Int3 value)
    {
      this.position = value;
    }

    public override void GetConnections(GraphNodeDelegate del)
    {
      if (this.connections == null)
        return;
      for (int index = 0; index < this.connections.Length; ++index)
        del(this.connections[index]);
    }

    public override void AddConnection(GraphNode node, uint cost)
    {
      throw new NotImplementedException("QuadTree Nodes do not have support for adding manual connections");
    }

    public override void RemoveConnection(GraphNode node)
    {
      throw new NotImplementedException("QuadTree Nodes do not have support for adding manual connections");
    }

    public override void ClearConnections(bool alsoReverse)
    {
      if (alsoReverse)
      {
        for (int index = 0; index < this.connections.Length; ++index)
          this.connections[index].RemoveConnection((GraphNode) this);
      }
      this.connections = (GraphNode[]) null;
      this.connectionCosts = (uint[]) null;
    }

    public override void Open(Path path, PathNode pathNode, PathHandler handler)
    {
      if (this.connections == null)
        return;
      for (int index = 0; index < this.connections.Length; ++index)
      {
        GraphNode node = this.connections[index];
        if (path.CanTraverse(node))
        {
          PathNode pathNode1 = handler.GetPathNode(node);
          if ((int) pathNode1.pathID != (int) handler.PathID)
          {
            pathNode1.node = node;
            pathNode1.parent = pathNode;
            pathNode1.pathID = handler.PathID;
            pathNode1.cost = this.connectionCosts[index];
            pathNode1.H = path.CalculateHScore(node);
            node.UpdateG(path, pathNode1);
            handler.PushNode(pathNode1);
          }
          else
          {
            uint num = this.connectionCosts[index];
            if (pathNode.G + num + path.GetTraversalCost(node) < pathNode1.G)
            {
              pathNode1.cost = num;
              pathNode1.parent = pathNode;
              node.UpdateRecursiveG(path, pathNode1, handler);
            }
            else if (pathNode1.G + num + path.GetTraversalCost((GraphNode) this) < pathNode.G && node.ContainsConnection((GraphNode) this))
            {
              pathNode.parent = pathNode1;
              pathNode.cost = num;
              this.UpdateRecursiveG(path, pathNode, handler);
            }
          }
        }
      }
    }
  }
}
