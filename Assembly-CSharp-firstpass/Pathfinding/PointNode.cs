// Decompiled with JetBrains decompiler
// Type: Pathfinding.PointNode
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding.Serialization;
using UnityEngine;

namespace Pathfinding
{
  public class PointNode : GraphNode
  {
    public GraphNode[] connections;
    public uint[] connectionCosts;
    public GameObject gameObject;
    public PointNode next;

    public PointNode(AstarPath astar)
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

    public override bool ContainsConnection(GraphNode node)
    {
      for (int index = 0; index < this.connections.Length; ++index)
      {
        if (this.connections[index] == node)
          return true;
      }
      return false;
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

    public override void SerializeNode(GraphSerializationContext ctx)
    {
      base.SerializeNode(ctx);
      ctx.writer.Write(this.position.x);
      ctx.writer.Write(this.position.y);
      ctx.writer.Write(this.position.z);
    }

    public override void DeserializeNode(GraphSerializationContext ctx)
    {
      base.DeserializeNode(ctx);
      this.position = new Int3(ctx.reader.ReadInt32(), ctx.reader.ReadInt32(), ctx.reader.ReadInt32());
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
