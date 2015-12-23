// Decompiled with JetBrains decompiler
// Type: Pathfinding.MeshNode
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding.Serialization;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
  public abstract class MeshNode : GraphNode
  {
    public GraphNode[] connections;
    public uint[] connectionCosts;

    public MeshNode(AstarPath astar)
      : base(astar)
    {
    }

    public abstract Int3 GetVertex(int i);

    public abstract int GetVertexCount();

    public abstract Vector3 ClosestPointOnNode(Vector3 p);

    public abstract Vector3 ClosestPointOnNodeXZ(Vector3 p);

    public override void ClearConnections(bool alsoReverse)
    {
      if (alsoReverse && this.connections != null)
      {
        for (int index = 0; index < this.connections.Length; ++index)
          this.connections[index].RemoveConnection((GraphNode) this);
      }
      this.connections = (GraphNode[]) null;
      this.connectionCosts = (uint[]) null;
    }

    public override void GetConnections(GraphNodeDelegate del)
    {
      if (this.connections == null)
        return;
      for (int index = 0; index < this.connections.Length; ++index)
        del(this.connections[index]);
    }

    public override void FloodFill(Stack<GraphNode> stack, uint region)
    {
      if (this.connections == null)
        return;
      for (int index = 0; index < this.connections.Length; ++index)
      {
        GraphNode graphNode = this.connections[index];
        if ((int) graphNode.Area != (int) region)
        {
          graphNode.Area = region;
          stack.Push(graphNode);
        }
      }
    }

    public override bool ContainsConnection(GraphNode node)
    {
      for (int index = 0; index < this.connections.Length; ++index)
      {
        if (this.connections[index] == node)
          return true;
      }
      return false;
    }

    public override void UpdateRecursiveG(Path path, PathNode pathNode, PathHandler handler)
    {
      this.UpdateG(path, pathNode);
      handler.PushNode(pathNode);
      for (int index = 0; index < this.connections.Length; ++index)
      {
        GraphNode node = this.connections[index];
        PathNode pathNode1 = handler.GetPathNode(node);
        if (pathNode1.parent == pathNode && (int) pathNode1.pathID == (int) handler.PathID)
          node.UpdateRecursiveG(path, pathNode1, handler);
      }
    }

    public override void AddConnection(GraphNode node, uint cost)
    {
      if (this.connections != null)
      {
        for (int index = 0; index < this.connections.Length; ++index)
        {
          if (this.connections[index] == node)
          {
            this.connectionCosts[index] = cost;
            return;
          }
        }
      }
      int index1 = this.connections == null ? 0 : this.connections.Length;
      GraphNode[] graphNodeArray = new GraphNode[index1 + 1];
      uint[] numArray = new uint[index1 + 1];
      for (int index2 = 0; index2 < index1; ++index2)
      {
        graphNodeArray[index2] = this.connections[index2];
        numArray[index2] = this.connectionCosts[index2];
      }
      graphNodeArray[index1] = node;
      numArray[index1] = cost;
      this.connections = graphNodeArray;
      this.connectionCosts = numArray;
    }

    public override void RemoveConnection(GraphNode node)
    {
      if (this.connections == null)
        return;
      for (int index1 = 0; index1 < this.connections.Length; ++index1)
      {
        if (this.connections[index1] == node)
        {
          int length = this.connections.Length;
          GraphNode[] graphNodeArray = new GraphNode[length - 1];
          uint[] numArray = new uint[length - 1];
          for (int index2 = 0; index2 < index1; ++index2)
          {
            graphNodeArray[index2] = this.connections[index2];
            numArray[index2] = this.connectionCosts[index2];
          }
          for (int index2 = index1 + 1; index2 < length; ++index2)
          {
            graphNodeArray[index2 - 1] = this.connections[index2];
            numArray[index2 - 1] = this.connectionCosts[index2];
          }
          this.connections = graphNodeArray;
          this.connectionCosts = numArray;
          break;
        }
      }
    }

    public virtual bool ContainsPoint(Int3 p)
    {
      bool flag = false;
      int vertexCount = this.GetVertexCount();
      int i1 = 0;
      int i2 = vertexCount - 1;
      for (; i1 < vertexCount; i2 = i1++)
      {
        if ((this.GetVertex(i1).z <= p.z && p.z < this.GetVertex(i2).z || this.GetVertex(i2).z <= p.z && p.z < this.GetVertex(i1).z) && p.x < (this.GetVertex(i2).x - this.GetVertex(i1).x) * (p.z - this.GetVertex(i1).z) / (this.GetVertex(i2).z - this.GetVertex(i1).z) + this.GetVertex(i1).x)
          flag = !flag;
      }
      return flag;
    }

    public override void SerializeReferences(GraphSerializationContext ctx)
    {
      if (this.connections == null)
      {
        ctx.writer.Write(-1);
      }
      else
      {
        ctx.writer.Write(this.connections.Length);
        for (int index = 0; index < this.connections.Length; ++index)
        {
          ctx.writer.Write(ctx.GetNodeIdentifier(this.connections[index]));
          ctx.writer.Write(this.connectionCosts[index]);
        }
      }
    }

    public override void DeserializeReferences(GraphSerializationContext ctx)
    {
      int length = ctx.reader.ReadInt32();
      if (length == -1)
      {
        this.connections = (GraphNode[]) null;
        this.connectionCosts = (uint[]) null;
      }
      else
      {
        this.connections = new GraphNode[length];
        this.connectionCosts = new uint[length];
        for (int index = 0; index < length; ++index)
        {
          this.connections[index] = ctx.GetNodeFromIdentifier(ctx.reader.ReadInt32());
          this.connectionCosts[index] = ctx.reader.ReadUInt32();
        }
      }
    }
  }
}
