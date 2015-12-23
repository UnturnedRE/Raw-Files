// Decompiled with JetBrains decompiler
// Type: Pathfinding.GridNode
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding.Serialization;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
  public class GridNode : GraphNode
  {
    private static GridGraph[] _gridGraphs = new GridGraph[0];
    private const int GridFlagsConnectionOffset = 0;
    private const int GridFlagsConnectionBit0 = 1;
    private const int GridFlagsConnectionMask = 255;
    private const int GridFlagsWalkableErosionOffset = 8;
    private const int GridFlagsWalkableErosionMask = 256;
    private const int GridFlagsWalkableTmpOffset = 9;
    private const int GridFlagsWalkableTmpMask = 512;
    private const int GridFlagsEdgeNodeOffset = 10;
    private const int GridFlagsEdgeNodeMask = 1024;
    protected int nodeInGridIndex;
    protected ushort gridFlags;

    internal ushort InternalGridFlags
    {
      get
      {
        return this.gridFlags;
      }
      set
      {
        this.gridFlags = value;
      }
    }

    public bool EdgeNode
    {
      get
      {
        return ((int) this.gridFlags & 1024) != 0;
      }
      set
      {
        this.gridFlags = (ushort) ((int) this.gridFlags & -1025 | (!value ? 0 : 1024));
      }
    }

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

    public GridNode(AstarPath astar)
      : base(astar)
    {
    }

    public static GridGraph GetGridGraph(uint graphIndex)
    {
      return GridNode._gridGraphs[(int) graphIndex];
    }

    public static void SetGridGraph(int graphIndex, GridGraph graph)
    {
      if (GridNode._gridGraphs.Length <= graphIndex)
      {
        GridGraph[] gridGraphArray = new GridGraph[graphIndex + 1];
        for (int index = 0; index < GridNode._gridGraphs.Length; ++index)
          gridGraphArray[index] = GridNode._gridGraphs[index];
        GridNode._gridGraphs = gridGraphArray;
      }
      GridNode._gridGraphs[graphIndex] = graph;
    }

    [Obsolete("This method has been deprecated. Please use NodeInGridIndex instead.", true)]
    public int GetIndex()
    {
      return 0;
    }

    public bool GetConnectionInternal(int dir)
    {
      return ((int) this.gridFlags >> dir & 1) != 0;
    }

    public void SetConnectionInternal(int dir, bool value)
    {
      this.gridFlags = (ushort) ((int) this.gridFlags & ~(1 << dir) | (!value ? 0 : 1) << dir);
    }

    public void ResetConnectionsInternal()
    {
      this.gridFlags = (ushort) ((uint) this.gridFlags & 4294967040U);
    }

    public override void ClearConnections(bool alsoReverse)
    {
      if (alsoReverse)
      {
        GridGraph gridGraph = GridNode.GetGridGraph(this.GraphIndex);
        for (int dir = 0; dir < 8; ++dir)
        {
          GridNode nodeConnection = gridGraph.GetNodeConnection(this, dir);
          if (nodeConnection != null)
            nodeConnection.SetConnectionInternal(dir >= 4 ? 7 : (dir + 2) % 4, false);
        }
      }
      this.ResetConnectionsInternal();
    }

    public override void GetConnections(GraphNodeDelegate del)
    {
      GridGraph gridGraph = GridNode.GetGridGraph(this.GraphIndex);
      int[] numArray = gridGraph.neighbourOffsets;
      GridNode[] gridNodeArray = gridGraph.nodes;
      for (int dir = 0; dir < 8; ++dir)
      {
        if (this.GetConnectionInternal(dir))
        {
          GridNode gridNode = gridNodeArray[this.nodeInGridIndex + numArray[dir]];
          if (gridNode != null)
            del((GraphNode) gridNode);
        }
      }
    }

    public override bool GetPortal(GraphNode other, List<Vector3> left, List<Vector3> right, bool backwards)
    {
      if (backwards)
        return true;
      GridGraph gridGraph = GridNode.GetGridGraph(this.GraphIndex);
      int[] numArray = gridGraph.neighbourOffsets;
      GridNode[] gridNodeArray = gridGraph.nodes;
      for (int dir = 0; dir < 4; ++dir)
      {
        if (this.GetConnectionInternal(dir) && other == gridNodeArray[this.nodeInGridIndex + numArray[dir]])
        {
          Vector3 vector3_1 = (Vector3) (this.position + other.position) * 0.5f;
          Vector3 vector3_2 = Vector3.Cross(gridGraph.collision.up, (Vector3) (other.position - this.position));
          vector3_2.Normalize();
          vector3_2 *= gridGraph.nodeSize * 0.5f;
          left.Add(vector3_1 - vector3_2);
          right.Add(vector3_1 + vector3_2);
          return true;
        }
      }
      for (int dir = 4; dir < 8; ++dir)
      {
        if (this.GetConnectionInternal(dir) && other == gridNodeArray[this.nodeInGridIndex + numArray[dir]])
        {
          bool flag1 = false;
          bool flag2 = false;
          if (this.GetConnectionInternal(dir - 4))
          {
            GridNode gridNode = gridNodeArray[this.nodeInGridIndex + numArray[dir - 4]];
            if (gridNode.Walkable && gridNode.GetConnectionInternal((dir - 4 + 1) % 4))
              flag1 = true;
          }
          if (this.GetConnectionInternal((dir - 4 + 1) % 4))
          {
            GridNode gridNode = gridNodeArray[this.nodeInGridIndex + numArray[(dir - 4 + 1) % 4]];
            if (gridNode.Walkable && gridNode.GetConnectionInternal(dir - 4))
              flag2 = true;
          }
          Vector3 vector3_1 = (Vector3) (this.position + other.position) * 0.5f;
          Vector3 vector3_2 = Vector3.Cross(gridGraph.collision.up, (Vector3) (other.position - this.position));
          vector3_2.Normalize();
          vector3_2 *= gridGraph.nodeSize * 1.4142f;
          left.Add(vector3_1 - (!flag2 ? Vector3.zero : vector3_2));
          right.Add(vector3_1 + (!flag1 ? Vector3.zero : vector3_2));
          return true;
        }
      }
      return false;
    }

    public override void FloodFill(Stack<GraphNode> stack, uint region)
    {
      GridGraph gridGraph = GridNode.GetGridGraph(this.GraphIndex);
      int[] numArray = gridGraph.neighbourOffsets;
      GridNode[] gridNodeArray = gridGraph.nodes;
      for (int dir = 0; dir < 8; ++dir)
      {
        if (this.GetConnectionInternal(dir))
        {
          GridNode gridNode = gridNodeArray[this.nodeInGridIndex + numArray[dir]];
          if (gridNode != null && (int) gridNode.Area != (int) region)
          {
            gridNode.Area = region;
            stack.Push((GraphNode) gridNode);
          }
        }
      }
    }

    public override void AddConnection(GraphNode node, uint cost)
    {
      throw new NotImplementedException("GridNodes do not have support for adding manual connections");
    }

    public override void RemoveConnection(GraphNode node)
    {
      throw new NotImplementedException("GridNodes do not have support for adding manual connections");
    }

    public override void UpdateRecursiveG(Path path, PathNode pathNode, PathHandler handler)
    {
      GridGraph gridGraph = GridNode.GetGridGraph(this.GraphIndex);
      int[] numArray = gridGraph.neighbourOffsets;
      GridNode[] gridNodeArray = gridGraph.nodes;
      this.UpdateG(path, pathNode);
      handler.PushNode(pathNode);
      ushort pathId = handler.PathID;
      for (int dir = 0; dir < 8; ++dir)
      {
        if (this.GetConnectionInternal(dir))
        {
          GridNode gridNode = gridNodeArray[this.nodeInGridIndex + numArray[dir]];
          PathNode pathNode1 = handler.GetPathNode((GraphNode) gridNode);
          if (pathNode1.parent == pathNode && (int) pathNode1.pathID == (int) pathId)
            gridNode.UpdateRecursiveG(path, pathNode1, handler);
        }
      }
    }

    public override void Open(Path path, PathNode pathNode, PathHandler handler)
    {
      GridGraph gridGraph = GridNode.GetGridGraph(this.GraphIndex);
      ushort pathId = handler.PathID;
      int[] numArray1 = gridGraph.neighbourOffsets;
      uint[] numArray2 = gridGraph.neighbourCosts;
      GridNode[] gridNodeArray = gridGraph.nodes;
      for (int dir = 0; dir < 8; ++dir)
      {
        if (this.GetConnectionInternal(dir))
        {
          GridNode gridNode = gridNodeArray[this.nodeInGridIndex + numArray1[dir]];
          if (path.CanTraverse((GraphNode) gridNode))
          {
            PathNode pathNode1 = handler.GetPathNode((GraphNode) gridNode);
            uint num = numArray2[dir];
            if ((int) pathNode1.pathID != (int) pathId)
            {
              pathNode1.parent = pathNode;
              pathNode1.pathID = pathId;
              pathNode1.cost = num;
              pathNode1.H = path.CalculateHScore((GraphNode) gridNode);
              gridNode.UpdateG(path, pathNode1);
              handler.PushNode(pathNode1);
            }
            else if (pathNode.G + num + path.GetTraversalCost((GraphNode) gridNode) < pathNode1.G)
            {
              pathNode1.cost = num;
              pathNode1.parent = pathNode;
              gridNode.UpdateRecursiveG(path, pathNode1, handler);
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

    public override void SerializeNode(GraphSerializationContext ctx)
    {
      base.SerializeNode(ctx);
      ctx.writer.Write(this.position.x);
      ctx.writer.Write(this.position.y);
      ctx.writer.Write(this.position.z);
      ctx.writer.Write(this.gridFlags);
    }

    public override void DeserializeNode(GraphSerializationContext ctx)
    {
      base.DeserializeNode(ctx);
      this.position = new Int3(ctx.reader.ReadInt32(), ctx.reader.ReadInt32(), ctx.reader.ReadInt32());
      this.gridFlags = ctx.reader.ReadUInt16();
    }
  }
}
