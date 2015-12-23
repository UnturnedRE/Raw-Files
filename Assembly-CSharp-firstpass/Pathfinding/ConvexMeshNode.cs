// Decompiled with JetBrains decompiler
// Type: Pathfinding.ConvexMeshNode
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

namespace Pathfinding
{
  public class ConvexMeshNode : MeshNode
  {
    protected static INavmeshHolder[] navmeshHolders = new INavmeshHolder[0];
    private int[] indices;

    public ConvexMeshNode(AstarPath astar)
      : base(astar)
    {
      this.indices = new int[0];
    }

    protected static INavmeshHolder GetNavmeshHolder(uint graphIndex)
    {
      return ConvexMeshNode.navmeshHolders[(int) graphIndex];
    }

    public void SetPosition(Int3 p)
    {
      this.position = p;
    }

    public int GetVertexIndex(int i)
    {
      return this.indices[i];
    }

    public override Int3 GetVertex(int i)
    {
      return ConvexMeshNode.GetNavmeshHolder(this.GraphIndex).GetVertex(this.GetVertexIndex(i));
    }

    public override int GetVertexCount()
    {
      return this.indices.Length;
    }

    public override Vector3 ClosestPointOnNode(Vector3 p)
    {
      throw new NotImplementedException();
    }

    public override Vector3 ClosestPointOnNodeXZ(Vector3 p)
    {
      throw new NotImplementedException();
    }

    public override void GetConnections(GraphNodeDelegate del)
    {
      if (this.connections == null)
        return;
      for (int index = 0; index < this.connections.Length; ++index)
        del(this.connections[index]);
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
