// Decompiled with JetBrains decompiler
// Type: Pathfinding.LevelGridNode
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding.Serialization;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
  public class LevelGridNode : GraphNode
  {
    private static LayerGridGraph[] _gridGraphs = new LayerGridGraph[0];
    private const int GridFlagsWalkableErosionOffset = 8;
    private const int GridFlagsWalkableErosionMask = 256;
    private const int GridFlagsWalkableTmpOffset = 9;
    private const int GridFlagsWalkableTmpMask = 512;
    public const int NoConnection = 255;
    public const int ConnectionMask = 255;
    private const int ConnectionStride = 8;
    public const int MaxLayerCount = 255;
    protected ushort gridFlags;
    protected int nodeInGridIndex;
    protected uint gridConnections;
    protected static LayerGridGraph[] gridGraphs;

    public bool WalkableErosion
    {
      get
      {
        return ((int) this.gridFlags & 256) != 0;
      }
      set
      {
        this.gridFlags = (ushort) ((int) this.gridFlags & -257 | (!value ? 0 : 256));
      }
    }

    public bool TmpWalkable
    {
      get
      {
        return ((int) this.gridFlags & 512) != 0;
      }
      set
      {
        this.gridFlags = (ushort) ((int) this.gridFlags & -513 | (!value ? 0 : 512));
      }
    }

    public int NodeInGridIndex
    {
      get
      {
        return this.nodeInGridIndex;
      }
      set
      {
        this.nodeInGridIndex = value;
      }
    }

    public LevelGridNode(AstarPath astar)
      : base(astar)
    {
    }

    public static LayerGridGraph GetGridGraph(uint graphIndex)
    {
      return LevelGridNode._gridGraphs[(int) graphIndex];
    }

    public static void SetGridGraph(int graphIndex, LayerGridGraph graph)
    {
      if (LevelGridNode._gridGraphs.Length <= graphIndex)
      {
        LayerGridGraph[] layerGridGraphArray = new LayerGridGraph[graphIndex + 1];
        for (int index = 0; index < LevelGridNode._gridGraphs.Length; ++index)
          layerGridGraphArray[index] = LevelGridNode._gridGraphs[index];
        LevelGridNode._gridGraphs = layerGridGraphArray;
      }
      LevelGridNode._gridGraphs[graphIndex] = graph;
    }

    public void ResetAllGridConnections()
    {
      this.gridConnections = uint.MaxValue;
    }

    public bool HasAnyGridConnections()
    {
      return (int) this.gridConnections != -1;
    }

    public void SetPosition(Int3 position)
    {
      this.position = position;
    }

    public override void ClearConnections(bool alsoReverse)
    {
      if (alsoReverse)
      {
        LayerGridGraph gridGraph = LevelGridNode.GetGridGraph(this.GraphIndex);
        int[] numArray = gridGraph.neighbourOffsets;
        LevelGridNode[] levelGridNodeArray = gridGraph.nodes;
        for (int dir = 0; dir < 4; ++dir)
        {
          int connectionValue = this.GetConnectionValue(dir);
          if (connectionValue != (int) byte.MaxValue)
          {
            LevelGridNode levelGridNode = levelGridNodeArray[this.NodeInGridIndex + numArray[dir] + gridGraph.width * gridGraph.depth * connectionValue];
            if (levelGridNode != null)
              levelGridNode.SetConnectionValue(dir >= 4 ? 7 : (dir + 2) % 4, (int) byte.MaxValue);
          }
        }
      }
      this.ResetAllGridConnections();
    }

    public override void GetConnections(GraphNodeDelegate del)
    {
      int nodeInGridIndex = this.NodeInGridIndex;
      LayerGridGraph gridGraph = LevelGridNode.GetGridGraph(this.GraphIndex);
      int[] numArray = gridGraph.neighbourOffsets;
      LevelGridNode[] levelGridNodeArray = gridGraph.nodes;
      for (int dir = 0; dir < 4; ++dir)
      {
        int connectionValue = this.GetConnectionValue(dir);
        if (connectionValue != (int) byte.MaxValue)
        {
          LevelGridNode levelGridNode = levelGridNodeArray[nodeInGridIndex + numArray[dir] + gridGraph.width * gridGraph.depth * connectionValue];
          if (levelGridNode != null)
            del((GraphNode) levelGridNode);
        }
      }
    }

    public override void FloodFill(Stack<GraphNode> stack, uint region)
    {
      int nodeInGridIndex = this.NodeInGridIndex;
      LayerGridGraph gridGraph = LevelGridNode.GetGridGraph(this.GraphIndex);
      int[] numArray = gridGraph.neighbourOffsets;
      LevelGridNode[] levelGridNodeArray = gridGraph.nodes;
      for (int dir = 0; dir < 4; ++dir)
      {
        int connectionValue = this.GetConnectionValue(dir);
        if (connectionValue != (int) byte.MaxValue)
        {
          LevelGridNode levelGridNode = levelGridNodeArray[nodeInGridIndex + numArray[dir] + gridGraph.width * gridGraph.depth * connectionValue];
          if (levelGridNode != null && (int) levelGridNode.Area != (int) region)
          {
            levelGridNode.Area = region;
            stack.Push((GraphNode) levelGridNode);
          }
        }
      }
    }

    public override void AddConnection(GraphNode node, uint cost)
    {
      throw new NotImplementedException("Layered Grid Nodes do not have support for adding manual connections");
    }

    public override void RemoveConnection(GraphNode node)
    {
      throw new NotImplementedException("Layered Grid Nodes do not have support for adding manual connections");
    }

    public bool GetConnection(int i)
    {
      return ((int) (this.gridConnections >> i * 8) & (int) byte.MaxValue) != (int) byte.MaxValue;
    }

    public void SetConnectionValue(int dir, int value)
    {
      this.gridConnections = (uint) ((int) this.gridConnections & ~((int) byte.MaxValue << dir * 8) | value << dir * 8);
    }

    public int GetConnectionValue(int dir)
    {
      return (int) (this.gridConnections >> dir * 8) & (int) byte.MaxValue;
    }

    public override bool GetPortal(GraphNode other, List<Vector3> left, List<Vector3> right, bool backwards)
    {
      if (backwards)
        return true;
      LayerGridGraph gridGraph = LevelGridNode.GetGridGraph(this.GraphIndex);
      int[] numArray = gridGraph.neighbourOffsets;
      LevelGridNode[] levelGridNodeArray = gridGraph.nodes;
      int nodeInGridIndex = this.NodeInGridIndex;
      for (int dir = 0; dir < 4; ++dir)
      {
        int connectionValue = this.GetConnectionValue(dir);
        if (connectionValue != (int) byte.MaxValue && other == levelGridNodeArray[nodeInGridIndex + numArray[dir] + gridGraph.width * gridGraph.depth * connectionValue])
        {
          Vector3 vector3_1 = (Vector3) (this.position + other.position) * 0.5f;
          Vector3 vector3_2 = Vector3.Cross(gridGraph.collision.up, (Vector3) (other.position - this.position));
          vector3_2.Normalize();
          Vector3 vector3_3 = vector3_2 * (gridGraph.nodeSize * 0.5f);
          left.Add(vector3_1 - vector3_3);
          right.Add(vector3_1 + vector3_3);
          return true;
        }
      }
      return false;
    }

    public override void UpdateRecursiveG(Path path, PathNode pathNode, PathHandler handler)
    {
      handler.PushNode(pathNode);
      this.UpdateG(path, pathNode);
      LayerGridGraph gridGraph = LevelGridNode.GetGridGraph(this.GraphIndex);
      int[] numArray = gridGraph.neighbourOffsets;
      LevelGridNode[] levelGridNodeArray = gridGraph.nodes;
      int nodeInGridIndex = this.NodeInGridIndex;
      for (int dir = 0; dir < 4; ++dir)
      {
        int connectionValue = this.GetConnectionValue(dir);
        if (connectionValue != (int) byte.MaxValue)
        {
          LevelGridNode levelGridNode = levelGridNodeArray[nodeInGridIndex + numArray[dir] + gridGraph.width * gridGraph.depth * connectionValue];
          PathNode pathNode1 = handler.GetPathNode((GraphNode) levelGridNode);
          if (pathNode1 != null && pathNode1.parent == pathNode && (int) pathNode1.pathID == (int) handler.PathID)
            levelGridNode.UpdateRecursiveG(path, pathNode1, handler);
        }
      }
    }

    public override void Open(Path path, PathNode pathNode, PathHandler handler)
    {
      LayerGridGraph gridGraph = LevelGridNode.GetGridGraph(this.GraphIndex);
      int[] numArray1 = gridGraph.neighbourOffsets;
      uint[] numArray2 = gridGraph.neighbourCosts;
      LevelGridNode[] levelGridNodeArray = gridGraph.nodes;
      int nodeInGridIndex = this.NodeInGridIndex;
      for (int dir = 0; dir < 4; ++dir)
      {
        int connectionValue = this.GetConnectionValue(dir);
        if (connectionValue != (int) byte.MaxValue)
        {
          GraphNode node = (GraphNode) levelGridNodeArray[nodeInGridIndex + numArray1[dir] + gridGraph.width * gridGraph.depth * connectionValue];
          if (path.CanTraverse(node))
          {
            PathNode pathNode1 = handler.GetPathNode(node);
            if ((int) pathNode1.pathID != (int) handler.PathID)
            {
              pathNode1.parent = pathNode;
              pathNode1.pathID = handler.PathID;
              pathNode1.cost = numArray2[dir];
              pathNode1.H = path.CalculateHScore(node);
              node.UpdateG(path, pathNode1);
              handler.PushNode(pathNode1);
            }
            else
            {
              uint num = numArray2[dir];
              if (pathNode.G + num + path.GetTraversalCost(node) < pathNode1.G)
              {
                pathNode1.cost = num;
                pathNode1.parent = pathNode;
                node.UpdateRecursiveG(path, pathNode1, handler);
              }
              else if (pathNode1.G + num + path.GetTraversalCost((GraphNode) this) < pathNode.G)
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

    public override void SerializeNode(GraphSerializationContext ctx)
    {
      base.SerializeNode(ctx);
      ctx.writer.Write(this.position.x);
      ctx.writer.Write(this.position.y);
      ctx.writer.Write(this.position.z);
      ctx.writer.Write(this.gridFlags);
      ctx.writer.Write(this.gridConnections);
    }

    public override void DeserializeNode(GraphSerializationContext ctx)
    {
      base.DeserializeNode(ctx);
      this.position = new Int3(ctx.reader.ReadInt32(), ctx.reader.ReadInt32(), ctx.reader.ReadInt32());
      this.gridFlags = ctx.reader.ReadUInt16();
      this.gridConnections = ctx.reader.ReadUInt32();
    }
  }
}
