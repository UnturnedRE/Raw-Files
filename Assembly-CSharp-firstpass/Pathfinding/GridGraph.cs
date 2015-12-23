// Decompiled with JetBrains decompiler
// Type: Pathfinding.GridGraph
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding.Serialization;
using Pathfinding.Serialization.JsonFx;
using Pathfinding.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
  [JsonOptIn]
  public class GridGraph : NavGraph, IUpdatableGraph, IRaycastableGraph
  {
    [JsonMember]
    public float aspectRatio = 1f;
    [JsonMember]
    public float nodeSize = 1f;
    [JsonMember]
    public float maxClimb = 0.4f;
    [JsonMember]
    public int maxClimbAxis = 1;
    [JsonMember]
    public float maxSlope = 90f;
    [JsonMember]
    public int erosionFirstTag = 1;
    [JsonMember]
    public float autoLinkDistLimit = 10f;
    [JsonMember]
    public NumNeighbours neighbours = NumNeighbours.Eight;
    [JsonMember]
    public bool cutCorners = true;
    [JsonMember]
    public float penaltyPositionFactor = 1f;
    [JsonMember]
    public float penaltyAngleFactor = 100f;
    [JsonMember]
    public float penaltyAnglePower = 1f;
    [JsonMember]
    public GridGraph.TextureData textureData = new GridGraph.TextureData();
    [NonSerialized]
    public readonly int[] neighbourOffsets = new int[8];
    [NonSerialized]
    public readonly uint[] neighbourCosts = new uint[8];
    [NonSerialized]
    public readonly int[] neighbourXOffsets = new int[8];
    [NonSerialized]
    public readonly int[] neighbourZOffsets = new int[8];
    public const int getNearestForceOverlap = 2;
    public int width;
    public int depth;
    [JsonMember]
    public float isometricAngle;
    [JsonMember]
    public Vector3 rotation;
    public Bounds bounds;
    [JsonMember]
    public Vector3 center;
    [JsonMember]
    public Vector2 unclampedSize;
    [JsonMember]
    public GraphCollision collision;
    [JsonMember]
    public int erodeIterations;
    [JsonMember]
    public bool erosionUseTags;
    [JsonMember]
    public bool autoLinkGrids;
    [JsonMember]
    public float penaltyPositionOffset;
    [JsonMember]
    public bool penaltyPosition;
    [JsonMember]
    public bool penaltyAngle;
    [JsonMember]
    public bool useJumpPointSearch;
    public Vector2 size;
    public Matrix4x4 boundsMatrix;
    public Matrix4x4 boundsMatrix2;
    public int scans;
    public GridNode[] nodes;
    [NonSerialized]
    protected int[] corners;

    public virtual bool uniformWidthDepthGrid
    {
      get
      {
        return true;
      }
    }

    public bool useRaycastNormal
    {
      get
      {
        return (double) Math.Abs(90f - this.maxSlope) > 1.40129846432482E-45;
      }
    }

    public int Width
    {
      get
      {
        return this.width;
      }
      set
      {
        this.width = value;
      }
    }

    public int Depth
    {
      get
      {
        return this.depth;
      }
      set
      {
        this.depth = value;
      }
    }

    public GridGraph()
    {
      this.unclampedSize = new Vector2(10f, 10f);
      this.nodeSize = 1f;
      this.collision = new GraphCollision();
    }

    public override void OnDestroy()
    {
      base.OnDestroy();
      this.RemoveGridGraphFromStatic();
    }

    public void RemoveGridGraphFromStatic()
    {
      GridNode.SetGridGraph(AstarPath.active.astarData.GetGraphIndex((NavGraph) this), (GridGraph) null);
    }

    public override void GetNodes(GraphNodeDelegateCancelable del)
    {
      if (this.nodes == null)
        return;
      int index = 0;
      while (index < this.nodes.Length && del((GraphNode) this.nodes[index]))
        ++index;
    }

    public Int3 GetNodePosition(int index, int yOffset)
    {
      // ISSUE: unable to decompile the method.
    }

    public uint GetConnectionCost(int dir)
    {
      return this.neighbourCosts[dir];
    }

    public GridNode GetNodeConnection(GridNode node, int dir)
    {
      if (!node.GetConnectionInternal(dir))
        return (GridNode) null;
      if (!node.EdgeNode)
        return this.nodes[node.NodeInGridIndex + this.neighbourOffsets[dir]];
      int nodeInGridIndex = node.NodeInGridIndex;
      int z = nodeInGridIndex / this.Width;
      int x = nodeInGridIndex - z * this.Width;
      return this.GetNodeConnection(nodeInGridIndex, x, z, dir);
    }

    public bool HasNodeConnection(GridNode node, int dir)
    {
      if (!node.GetConnectionInternal(dir))
        return false;
      if (!node.EdgeNode)
        return true;
      int nodeInGridIndex = node.NodeInGridIndex;
      int z = nodeInGridIndex / this.Width;
      int x = nodeInGridIndex - z * this.Width;
      return this.HasNodeConnection(nodeInGridIndex, x, z, dir);
    }

    public void SetNodeConnection(GridNode node, int dir, bool value)
    {
      int nodeInGridIndex = node.NodeInGridIndex;
      int z = nodeInGridIndex / this.Width;
      int x = nodeInGridIndex - z * this.Width;
      this.SetNodeConnection(nodeInGridIndex, x, z, dir, value);
    }

    private GridNode GetNodeConnection(int index, int x, int z, int dir)
    {
      if (!this.nodes[index].GetConnectionInternal(dir))
        return (GridNode) null;
      int num1 = x + this.neighbourXOffsets[dir];
      if (num1 < 0 || num1 >= this.Width)
        return (GridNode) null;
      int num2 = z + this.neighbourZOffsets[dir];
      if (num2 < 0 || num2 >= this.Depth)
        return (GridNode) null;
      return this.nodes[index + this.neighbourOffsets[dir]];
    }

    public void SetNodeConnection(int index, int x, int z, int dir, bool value)
    {
      this.nodes[index].SetConnectionInternal(dir, value);
    }

    public bool HasNodeConnection(int index, int x, int z, int dir)
    {
      if (!this.nodes[index].GetConnectionInternal(dir))
        return false;
      int num1 = x + this.neighbourXOffsets[dir];
      if (num1 < 0 || num1 >= this.Width)
        return false;
      int num2 = z + this.neighbourZOffsets[dir];
      return num2 >= 0 && num2 < this.Depth;
    }

    public void UpdateSizeFromWidthDepth()
    {
      this.unclampedSize = new Vector2((float) this.width, (float) this.depth) * this.nodeSize;
      this.GenerateMatrix();
    }

    public void GenerateMatrix()
    {
      // ISSUE: unable to decompile the method.
    }

    public override NNInfo GetNearest(Vector3 position, NNConstraint constraint, GraphNode hint)
    {
      if (this.nodes == null || this.depth * this.width != this.nodes.Length)
        return new NNInfo();
      position = this.inverseMatrix.MultiplyPoint3x4(position);
      float f1 = position.x - 0.5f;
      float f2 = position.z - 0.5f;
      int num1 = Mathf.Clamp(Mathf.RoundToInt(f1), 0, this.width - 1);
      int num2 = Mathf.Clamp(Mathf.RoundToInt(f2), 0, this.depth - 1);
      NNInfo nnInfo = new NNInfo((GraphNode) this.nodes[num2 * this.width + num1]);
      float y = this.inverseMatrix.MultiplyPoint3x4((Vector3) this.nodes[num2 * this.width + num1].position).y;
      nnInfo.clampedPosition = this.matrix.MultiplyPoint3x4(new Vector3(Mathf.Clamp(f1, (float) num1 - 0.5f, (float) num1 + 0.5f) + 0.5f, y, Mathf.Clamp(f2, (float) num2 - 0.5f, (float) num2 + 0.5f) + 0.5f));
      return nnInfo;
    }

    public override NNInfo GetNearestForce(Vector3 position, NNConstraint constraint)
    {
      if (this.nodes == null || this.depth * this.width != this.nodes.Length)
        return new NNInfo();
      Vector3 vector3_1 = position;
      position = this.inverseMatrix.MultiplyPoint3x4(position);
      float f1 = position.x - 0.5f;
      float f2 = position.z - 0.5f;
      int num1 = Mathf.Clamp(Mathf.RoundToInt(f1), 0, this.width - 1);
      int num2 = Mathf.Clamp(Mathf.RoundToInt(f2), 0, this.depth - 1);
      GridNode gridNode1 = this.nodes[num1 + num2 * this.width];
      GridNode gridNode2 = (GridNode) null;
      float num3 = float.PositiveInfinity;
      int num4 = 2;
      Vector3 vector3_2 = Vector3.zero;
      NNInfo nnInfo = new NNInfo((GraphNode) null);
      if (constraint.Suitable((GraphNode) gridNode1))
      {
        gridNode2 = gridNode1;
        num3 = ((Vector3) gridNode2.position - vector3_1).sqrMagnitude;
        float y = this.inverseMatrix.MultiplyPoint3x4((Vector3) gridNode1.position).y;
        vector3_2 = this.matrix.MultiplyPoint3x4(new Vector3(Mathf.Clamp(f1, (float) num1 - 0.5f, (float) num1 + 0.5f) + 0.5f, y, Mathf.Clamp(f2, (float) num2 - 0.5f, (float) num2 + 0.5f) + 0.5f));
      }
      if (gridNode2 != null)
      {
        nnInfo.node = (GraphNode) gridNode2;
        nnInfo.clampedPosition = vector3_2;
        if (num4 == 0)
          return nnInfo;
        --num4;
      }
      float num5 = !constraint.constrainDistance ? float.PositiveInfinity : AstarPath.active.maxNearestNodeDistance;
      float num6 = num5 * num5;
      for (int index1 = 1; (double) this.nodeSize * (double) index1 <= (double) num5; ++index1)
      {
        bool flag = false;
        int num7 = num2 + index1;
        int num8 = num7 * this.width;
        for (int index2 = num1 - index1; index2 <= num1 + index1; ++index2)
        {
          if (index2 >= 0 && num7 >= 0 && (index2 < this.width && num7 < this.depth))
          {
            flag = true;
            if (constraint.Suitable((GraphNode) this.nodes[index2 + num8]))
            {
              float sqrMagnitude = ((Vector3) this.nodes[index2 + num8].position - vector3_1).sqrMagnitude;
              if ((double) sqrMagnitude < (double) num3 && (double) sqrMagnitude < (double) num6)
              {
                num3 = sqrMagnitude;
                gridNode2 = this.nodes[index2 + num8];
                vector3_2 = this.matrix.MultiplyPoint3x4(new Vector3(Mathf.Clamp(f1, (float) index2 - 0.5f, (float) index2 + 0.5f) + 0.5f, this.inverseMatrix.MultiplyPoint3x4((Vector3) gridNode2.position).y, Mathf.Clamp(f2, (float) num7 - 0.5f, (float) num7 + 0.5f) + 0.5f));
              }
            }
          }
        }
        int num9 = num2 - index1;
        int num10 = num9 * this.width;
        for (int index2 = num1 - index1; index2 <= num1 + index1; ++index2)
        {
          if (index2 >= 0 && num9 >= 0 && (index2 < this.width && num9 < this.depth))
          {
            flag = true;
            if (constraint.Suitable((GraphNode) this.nodes[index2 + num10]))
            {
              float sqrMagnitude = ((Vector3) this.nodes[index2 + num10].position - vector3_1).sqrMagnitude;
              if ((double) sqrMagnitude < (double) num3 && (double) sqrMagnitude < (double) num6)
              {
                num3 = sqrMagnitude;
                gridNode2 = this.nodes[index2 + num10];
                vector3_2 = this.matrix.MultiplyPoint3x4(new Vector3(Mathf.Clamp(f1, (float) index2 - 0.5f, (float) index2 + 0.5f) + 0.5f, this.inverseMatrix.MultiplyPoint3x4((Vector3) gridNode2.position).y, Mathf.Clamp(f2, (float) num9 - 0.5f, (float) num9 + 0.5f) + 0.5f));
              }
            }
          }
        }
        int num11 = num1 - index1;
        int num12 = num2 - index1 + 1;
        for (int index2 = num2 - index1 + 1; index2 <= num2 + index1 - 1; ++index2)
        {
          if (num11 >= 0 && index2 >= 0 && (num11 < this.width && index2 < this.depth))
          {
            flag = true;
            if (constraint.Suitable((GraphNode) this.nodes[num11 + index2 * this.width]))
            {
              float sqrMagnitude = ((Vector3) this.nodes[num11 + index2 * this.width].position - vector3_1).sqrMagnitude;
              if ((double) sqrMagnitude < (double) num3 && (double) sqrMagnitude < (double) num6)
              {
                num3 = sqrMagnitude;
                gridNode2 = this.nodes[num11 + index2 * this.width];
                vector3_2 = this.matrix.MultiplyPoint3x4(new Vector3(Mathf.Clamp(f1, (float) num11 - 0.5f, (float) num11 + 0.5f) + 0.5f, this.inverseMatrix.MultiplyPoint3x4((Vector3) gridNode2.position).y, Mathf.Clamp(f2, (float) index2 - 0.5f, (float) index2 + 0.5f) + 0.5f));
              }
            }
          }
        }
        int num13 = num1 + index1;
        for (int index2 = num2 - index1 + 1; index2 <= num2 + index1 - 1; ++index2)
        {
          if (num13 >= 0 && index2 >= 0 && (num13 < this.width && index2 < this.depth))
          {
            flag = true;
            if (constraint.Suitable((GraphNode) this.nodes[num13 + index2 * this.width]))
            {
              float sqrMagnitude = ((Vector3) this.nodes[num13 + index2 * this.width].position - vector3_1).sqrMagnitude;
              if ((double) sqrMagnitude < (double) num3 && (double) sqrMagnitude < (double) num6)
              {
                num3 = sqrMagnitude;
                gridNode2 = this.nodes[num13 + index2 * this.width];
                vector3_2 = this.matrix.MultiplyPoint3x4(new Vector3(Mathf.Clamp(f1, (float) num13 - 0.5f, (float) num13 + 0.5f) + 0.5f, this.inverseMatrix.MultiplyPoint3x4((Vector3) gridNode2.position).y, Mathf.Clamp(f2, (float) index2 - 0.5f, (float) index2 + 0.5f) + 0.5f));
              }
            }
          }
        }
        if (gridNode2 != null)
        {
          if (num4 == 0)
          {
            nnInfo.node = (GraphNode) gridNode2;
            nnInfo.clampedPosition = vector3_2;
            return nnInfo;
          }
          --num4;
        }
        if (!flag)
        {
          nnInfo.node = (GraphNode) gridNode2;
          nnInfo.clampedPosition = vector3_2;
          return nnInfo;
        }
      }
      nnInfo.node = (GraphNode) gridNode2;
      nnInfo.clampedPosition = vector3_2;
      return nnInfo;
    }

    public virtual void SetUpOffsetsAndCosts()
    {
      this.neighbourOffsets[0] = -this.width;
      this.neighbourOffsets[1] = 1;
      this.neighbourOffsets[2] = this.width;
      this.neighbourOffsets[3] = -1;
      this.neighbourOffsets[4] = -this.width + 1;
      this.neighbourOffsets[5] = this.width + 1;
      this.neighbourOffsets[6] = this.width - 1;
      this.neighbourOffsets[7] = -this.width - 1;
      uint num1 = (uint) Mathf.RoundToInt(this.nodeSize * 1000f);
      uint num2 = (uint) Mathf.RoundToInt((float) ((double) this.nodeSize * (double) Mathf.Sqrt(2f) * 1000.0));
      this.neighbourCosts[0] = num1;
      this.neighbourCosts[1] = num1;
      this.neighbourCosts[2] = num1;
      this.neighbourCosts[3] = num1;
      this.neighbourCosts[4] = num2;
      this.neighbourCosts[5] = num2;
      this.neighbourCosts[6] = num2;
      this.neighbourCosts[7] = num2;
      this.neighbourXOffsets[0] = 0;
      this.neighbourXOffsets[1] = 1;
      this.neighbourXOffsets[2] = 0;
      this.neighbourXOffsets[3] = -1;
      this.neighbourXOffsets[4] = 1;
      this.neighbourXOffsets[5] = 1;
      this.neighbourXOffsets[6] = -1;
      this.neighbourXOffsets[7] = -1;
      this.neighbourZOffsets[0] = -1;
      this.neighbourZOffsets[1] = 0;
      this.neighbourZOffsets[2] = 1;
      this.neighbourZOffsets[3] = 0;
      this.neighbourZOffsets[4] = -1;
      this.neighbourZOffsets[5] = 1;
      this.neighbourZOffsets[6] = 1;
      this.neighbourZOffsets[7] = -1;
    }

    public override void ScanInternal(OnScanStatus statusCallback)
    {
      AstarPath.OnPostScan += new OnScanDelegate(this.OnPostScan);
      ++this.scans;
      if ((double) this.nodeSize <= 0.0)
        return;
      this.GenerateMatrix();
      if (this.width > 1024 || this.depth > 1024)
      {
        Debug.LogError((object) "One of the grid's sides is longer than 1024 nodes");
      }
      else
      {
        if (this.useJumpPointSearch)
          Debug.LogError((object) "Trying to use Jump Point Search, but support for it is not enabled. Please enable it in the inspector (Grid Graph settings).");
        this.SetUpOffsetsAndCosts();
        int graphIndex = AstarPath.active.astarData.GetGraphIndex((NavGraph) this);
        GridNode.SetGridGraph(graphIndex, this);
        this.nodes = new GridNode[this.width * this.depth];
        for (int index = 0; index < this.nodes.Length; ++index)
        {
          this.nodes[index] = new GridNode(this.active);
          this.nodes[index].GraphIndex = (uint) graphIndex;
        }
        if (this.collision == null)
          this.collision = new GraphCollision();
        this.collision.Initialize(this.matrix, this.nodeSize);
        this.textureData.Initialize();
        for (int z = 0; z < this.depth; ++z)
        {
          for (int x = 0; x < this.width; ++x)
          {
            GridNode node = this.nodes[z * this.width + x];
            node.NodeInGridIndex = z * this.width + x;
            this.UpdateNodePositionCollision(node, x, z, true);
            this.textureData.Apply(node, x, z);
          }
        }
        for (int z = 0; z < this.depth; ++z)
        {
          for (int x = 0; x < this.width; ++x)
          {
            GridNode node = this.nodes[z * this.width + x];
            this.CalculateConnections(this.nodes, x, z, node);
          }
        }
        this.ErodeWalkableArea();
      }
    }

    public virtual void UpdateNodePositionCollision(GridNode node, int x, int z, bool resetPenalty = true)
    {
      // ISSUE: unable to decompile the method.
    }

    public virtual void ErodeWalkableArea()
    {
      this.ErodeWalkableArea(0, 0, this.Width, this.Depth);
    }

    public virtual void ErodeWalkableArea(int xmin, int zmin, int xmax, int zmax)
    {
      xmin = xmin >= 0 ? (xmin <= this.Width ? xmin : this.Width) : 0;
      xmax = xmax >= 0 ? (xmax <= this.Width ? xmax : this.Width) : 0;
      zmin = zmin >= 0 ? (zmin <= this.Depth ? zmin : this.Depth) : 0;
      zmax = zmax >= 0 ? (zmax <= this.Depth ? zmax : this.Depth) : 0;
      if (!this.erosionUseTags)
      {
        for (int index1 = 0; index1 < this.erodeIterations; ++index1)
        {
          for (int index2 = zmin; index2 < zmax; ++index2)
          {
            for (int index3 = xmin; index3 < xmax; ++index3)
            {
              GridNode node = this.nodes[index2 * this.Width + index3];
              if (node.Walkable)
              {
                bool flag = false;
                for (int dir = 0; dir < 4; ++dir)
                {
                  if (!this.HasNodeConnection(node, dir))
                  {
                    flag = true;
                    break;
                  }
                }
                if (flag)
                  node.Walkable = false;
              }
            }
          }
          for (int z = zmin; z < zmax; ++z)
          {
            for (int x = xmin; x < xmax; ++x)
            {
              GridNode node = this.nodes[z * this.Width + x];
              this.CalculateConnections(this.nodes, x, z, node);
            }
          }
        }
      }
      else if (this.erodeIterations + this.erosionFirstTag > 31)
        Debug.LogError((object) ("Too few tags available for " + (object) this.erodeIterations + " erode iterations and starting with tag " + (string) (object) this.erosionFirstTag + " (erodeIterations+erosionFirstTag > 31)"));
      else if (this.erosionFirstTag <= 0)
      {
        Debug.LogError((object) "First erosion tag must be greater or equal to 1");
      }
      else
      {
        for (int index1 = 0; index1 < this.erodeIterations; ++index1)
        {
          for (int index2 = zmin; index2 < zmax; ++index2)
          {
            for (int index3 = xmin; index3 < xmax; ++index3)
            {
              GridNode node = this.nodes[index2 * this.width + index3];
              if (node.Walkable && (long) node.Tag >= (long) this.erosionFirstTag && (long) node.Tag < (long) (this.erosionFirstTag + index1))
              {
                for (int dir = 0; dir < 4; ++dir)
                {
                  GridNode nodeConnection = this.GetNodeConnection(node, dir);
                  if (nodeConnection != null)
                  {
                    uint tag = nodeConnection.Tag;
                    if ((long) tag > (long) (this.erosionFirstTag + index1) || (long) tag < (long) this.erosionFirstTag)
                      nodeConnection.Tag = (uint) (this.erosionFirstTag + index1);
                  }
                }
              }
              else if (node.Walkable && index1 == 0)
              {
                bool flag = false;
                for (int dir = 0; dir < 4; ++dir)
                {
                  if (!this.HasNodeConnection(node, dir))
                  {
                    flag = true;
                    break;
                  }
                }
                if (flag)
                  node.Tag = (uint) (this.erosionFirstTag + index1);
              }
            }
          }
        }
      }
    }

    public virtual bool IsValidConnection(GridNode n1, GridNode n2)
    {
      return n1.Walkable && n2.Walkable && ((double) this.maxClimb == 0.0 || (double) Mathf.Abs(n1.position[this.maxClimbAxis] - n2.position[this.maxClimbAxis]) <= (double) this.maxClimb * 1000.0);
    }

    public static void CalculateConnections(GridNode node)
    {
      GridGraph gridGraph = AstarData.GetGraph((GraphNode) node) as GridGraph;
      if (gridGraph == null)
        return;
      int nodeInGridIndex = node.NodeInGridIndex;
      int x = nodeInGridIndex % gridGraph.width;
      int z = nodeInGridIndex / gridGraph.width;
      gridGraph.CalculateConnections(gridGraph.nodes, x, z, node);
    }

    public virtual void CalculateConnections(GridNode[] nodes, int x, int z, GridNode node)
    {
      node.ResetConnectionsInternal();
      if (!node.Walkable)
        return;
      int nodeInGridIndex = node.NodeInGridIndex;
      if (this.corners == null)
      {
        this.corners = new int[4];
      }
      else
      {
        for (int index = 0; index < 4; ++index)
          this.corners[index] = 0;
      }
      int dir = 0;
      int index1 = 3;
      for (; dir < 4; ++dir)
      {
        int num1 = x + this.neighbourXOffsets[dir];
        int num2 = z + this.neighbourZOffsets[dir];
        if (num1 >= 0 && num2 >= 0 && (num1 < this.width && num2 < this.depth))
        {
          GridNode n2 = nodes[nodeInGridIndex + this.neighbourOffsets[dir]];
          if (this.IsValidConnection(node, n2))
          {
            node.SetConnectionInternal(dir, true);
            ++this.corners[dir];
            ++this.corners[index1];
          }
          else
            node.SetConnectionInternal(dir, false);
        }
        index1 = dir;
      }
      if (this.neighbours != NumNeighbours.Eight)
        return;
      if (this.cutCorners)
      {
        for (int index2 = 0; index2 < 4; ++index2)
        {
          if (this.corners[index2] >= 1)
          {
            int num1 = x + this.neighbourXOffsets[index2 + 4];
            int num2 = z + this.neighbourZOffsets[index2 + 4];
            if (num1 >= 0 && num2 >= 0 && (num1 < this.width && num2 < this.depth))
            {
              GridNode n2 = nodes[nodeInGridIndex + this.neighbourOffsets[index2 + 4]];
              node.SetConnectionInternal(index2 + 4, this.IsValidConnection(node, n2));
            }
          }
        }
      }
      else
      {
        for (int index2 = 0; index2 < 4; ++index2)
        {
          if (this.corners[index2] == 2)
          {
            GridNode n2 = nodes[nodeInGridIndex + this.neighbourOffsets[index2 + 4]];
            node.SetConnectionInternal(index2 + 4, this.IsValidConnection(node, n2));
          }
        }
      }
    }

    public void OnPostScan(AstarPath script)
    {
      AstarPath.OnPostScan -= new OnScanDelegate(this.OnPostScan);
      if (this.autoLinkGrids && (double) this.autoLinkDistLimit > 0.0)
        throw new NotSupportedException();
    }

    public override void OnDrawGizmos(bool drawNodes)
    {
      Gizmos.matrix = this.boundsMatrix;
      Gizmos.color = Color.white;
      Gizmos.DrawWireCube(Vector3.zero, new Vector3(this.size.x, 0.0f, this.size.y));
      Gizmos.matrix = Matrix4x4.identity;
      if (!drawNodes || this.nodes == null || this.depth * this.width != this.nodes.Length)
        return;
      PathHandler debugPathData = AstarPath.active.debugPathData;
      for (int index1 = 0; index1 < this.depth; ++index1)
      {
        for (int index2 = 0; index2 < this.width; ++index2)
        {
          GridNode node = this.nodes[index1 * this.width + index2];
          if (node.Walkable)
          {
            Gizmos.color = this.NodeColor((GraphNode) node, AstarPath.active.debugPathData);
            if (AstarPath.active.showSearchTree && debugPathData != null)
            {
              if (this.InSearchTree((GraphNode) node, AstarPath.active.debugPath))
              {
                PathNode pathNode = debugPathData.GetPathNode((GraphNode) node);
                if (pathNode != null && pathNode.parent != null)
                  Gizmos.DrawLine((Vector3) node.position, (Vector3) pathNode.parent.node.position);
              }
            }
            else
            {
              for (int dir = 0; dir < 8; ++dir)
              {
                GridNode nodeConnection = this.GetNodeConnection(node, dir);
                if (nodeConnection != null)
                  Gizmos.DrawLine((Vector3) node.position, (Vector3) nodeConnection.position);
              }
            }
          }
        }
      }
    }

    public void GetBoundsMinMax(Bounds b, Matrix4x4 matrix, out Vector3 min, out Vector3 max)
    {
      Vector3[] vector3Array = new Vector3[8]
      {
        matrix.MultiplyPoint3x4(b.center + new Vector3(b.extents.x, b.extents.y, b.extents.z)),
        matrix.MultiplyPoint3x4(b.center + new Vector3(b.extents.x, b.extents.y, -b.extents.z)),
        matrix.MultiplyPoint3x4(b.center + new Vector3(b.extents.x, -b.extents.y, b.extents.z)),
        matrix.MultiplyPoint3x4(b.center + new Vector3(b.extents.x, -b.extents.y, -b.extents.z)),
        matrix.MultiplyPoint3x4(b.center + new Vector3(-b.extents.x, b.extents.y, b.extents.z)),
        matrix.MultiplyPoint3x4(b.center + new Vector3(-b.extents.x, b.extents.y, -b.extents.z)),
        matrix.MultiplyPoint3x4(b.center + new Vector3(-b.extents.x, -b.extents.y, b.extents.z)),
        matrix.MultiplyPoint3x4(b.center + new Vector3(-b.extents.x, -b.extents.y, -b.extents.z))
      };
      min = vector3Array[0];
      max = vector3Array[0];
      for (int index = 1; index < 8; ++index)
      {
        min = Vector3.Min(min, vector3Array[index]);
        max = Vector3.Max(max, vector3Array[index]);
      }
    }

    public List<GraphNode> GetNodesInArea(Bounds b)
    {
      return this.GetNodesInArea(b, (GraphUpdateShape) null);
    }

    public List<GraphNode> GetNodesInArea(GraphUpdateShape shape)
    {
      return this.GetNodesInArea(shape.GetBounds(), shape);
    }

    private List<GraphNode> GetNodesInArea(Bounds b, GraphUpdateShape shape)
    {
      if (this.nodes == null || this.width * this.depth != this.nodes.Length)
        return (List<GraphNode>) null;
      List<GraphNode> list = ListPool<GraphNode>.Claim();
      Vector3 min;
      Vector3 max;
      this.GetBoundsMinMax(b, this.inverseMatrix, out min, out max);
      int xmin = Mathf.RoundToInt(min.x - 0.5f);
      int xmax = Mathf.RoundToInt(max.x - 0.5f);
      int ymin = Mathf.RoundToInt(min.z - 0.5f);
      int ymax = Mathf.RoundToInt(max.z - 0.5f);
      IntRect intRect = IntRect.Intersection(new IntRect(xmin, ymin, xmax, ymax), new IntRect(0, 0, this.width - 1, this.depth - 1));
      for (int index1 = intRect.xmin; index1 <= intRect.xmax; ++index1)
      {
        for (int index2 = intRect.ymin; index2 <= intRect.ymax; ++index2)
        {
          GraphNode graphNode = (GraphNode) this.nodes[index2 * this.width + index1];
          if (b.Contains((Vector3) graphNode.position) && (shape == null || shape.Contains((Vector3) graphNode.position)))
            list.Add(graphNode);
        }
      }
      return list;
    }

    public GraphUpdateThreading CanUpdateAsync(GraphUpdateObject o)
    {
      return GraphUpdateThreading.UnityThread;
    }

    public void UpdateAreaInit(GraphUpdateObject o)
    {
    }

    public void UpdateArea(GraphUpdateObject o)
    {
      if (this.nodes == null || this.nodes.Length != this.width * this.depth)
      {
        Debug.LogWarning((object) "The Grid Graph is not scanned, cannot update area ");
      }
      else
      {
        Vector3 min;
        Vector3 max;
        this.GetBoundsMinMax(o.bounds, this.inverseMatrix, out min, out max);
        int xmin = Mathf.RoundToInt(min.x - 0.5f);
        int xmax = Mathf.RoundToInt(max.x - 0.5f);
        int ymin = Mathf.RoundToInt(min.z - 0.5f);
        int ymax = Mathf.RoundToInt(max.z - 0.5f);
        IntRect a1 = new IntRect(xmin, ymin, xmax, ymax);
        IntRect intRect1 = a1;
        IntRect b = new IntRect(0, 0, this.width - 1, this.depth - 1);
        IntRect intRect2 = a1;
        int range = !o.updateErosion ? 0 : this.erodeIterations;
        bool flag = o.updatePhysics || o.modifyWalkability;
        if (o.updatePhysics && !o.modifyWalkability && this.collision.collisionCheck)
        {
          Vector3 vector3_1 = new Vector3(this.collision.diameter, 0.0f, this.collision.diameter) * 0.5f;
          Vector3 vector3_2 = min - vector3_1 * 1.02f;
          Vector3 vector3_3 = max + vector3_1 * 1.02f;
          intRect2 = new IntRect(Mathf.RoundToInt(vector3_2.x - 0.5f), Mathf.RoundToInt(vector3_2.z - 0.5f), Mathf.RoundToInt(vector3_3.x - 0.5f), Mathf.RoundToInt(vector3_3.z - 0.5f));
          intRect1 = IntRect.Union(intRect2, intRect1);
        }
        if (flag || range > 0)
          intRect1 = intRect1.Expand(range + 1);
        IntRect intRect3 = IntRect.Intersection(intRect1, b);
        for (int index1 = intRect3.xmin; index1 <= intRect3.xmax; ++index1)
        {
          for (int index2 = intRect3.ymin; index2 <= intRect3.ymax; ++index2)
            o.WillUpdateNode((GraphNode) this.nodes[index2 * this.width + index1]);
        }
        if (o.updatePhysics && !o.modifyWalkability)
        {
          this.collision.Initialize(this.matrix, this.nodeSize);
          intRect3 = IntRect.Intersection(intRect2, b);
          for (int x = intRect3.xmin; x <= intRect3.xmax; ++x)
          {
            for (int z = intRect3.ymin; z <= intRect3.ymax; ++z)
              this.UpdateNodePositionCollision(this.nodes[z * this.width + x], x, z, o.resetPenaltyOnPhysics);
          }
        }
        intRect3 = IntRect.Intersection(a1, b);
        for (int index1 = intRect3.xmin; index1 <= intRect3.xmax; ++index1)
        {
          for (int index2 = intRect3.ymin; index2 <= intRect3.ymax; ++index2)
          {
            GridNode gridNode = this.nodes[index2 * this.width + index1];
            if (flag)
            {
              gridNode.Walkable = gridNode.WalkableErosion;
              if (o.bounds.Contains((Vector3) gridNode.position))
                o.Apply((GraphNode) gridNode);
              gridNode.WalkableErosion = gridNode.Walkable;
            }
            else if (o.bounds.Contains((Vector3) gridNode.position))
              o.Apply((GraphNode) gridNode);
          }
        }
        if (flag && range == 0)
        {
          intRect3 = IntRect.Intersection(intRect1, b);
          for (int x = intRect3.xmin; x <= intRect3.xmax; ++x)
          {
            for (int z = intRect3.ymin; z <= intRect3.ymax; ++z)
            {
              GridNode node = this.nodes[z * this.width + x];
              this.CalculateConnections(this.nodes, x, z, node);
            }
          }
        }
        else
        {
          if (!flag || range <= 0)
            return;
          intRect3 = IntRect.Union(a1, intRect2);
          IntRect a2 = intRect3.Expand(range);
          IntRect a3 = a2.Expand(range);
          IntRect intRect4 = IntRect.Intersection(a2, b);
          IntRect intRect5 = IntRect.Intersection(a3, b);
          for (int x = intRect5.xmin; x <= intRect5.xmax; ++x)
          {
            for (int y = intRect5.ymin; y <= intRect5.ymax; ++y)
            {
              GridNode gridNode = this.nodes[y * this.width + x];
              bool walkable = gridNode.Walkable;
              gridNode.Walkable = gridNode.WalkableErosion;
              if (!intRect4.Contains(x, y))
                gridNode.TmpWalkable = walkable;
            }
          }
          for (int x = intRect5.xmin; x <= intRect5.xmax; ++x)
          {
            for (int z = intRect5.ymin; z <= intRect5.ymax; ++z)
            {
              GridNode node = this.nodes[z * this.width + x];
              this.CalculateConnections(this.nodes, x, z, node);
            }
          }
          this.ErodeWalkableArea(intRect5.xmin, intRect5.ymin, intRect5.xmax + 1, intRect5.ymax + 1);
          for (int x = intRect5.xmin; x <= intRect5.xmax; ++x)
          {
            for (int y = intRect5.ymin; y <= intRect5.ymax; ++y)
            {
              if (!intRect4.Contains(x, y))
              {
                GridNode gridNode = this.nodes[y * this.width + x];
                gridNode.Walkable = gridNode.TmpWalkable;
              }
            }
          }
          for (int x = intRect5.xmin; x <= intRect5.xmax; ++x)
          {
            for (int z = intRect5.ymin; z <= intRect5.ymax; ++z)
            {
              GridNode node = this.nodes[z * this.width + x];
              this.CalculateConnections(this.nodes, x, z, node);
            }
          }
        }
      }
    }

    public bool Linecast(Vector3 _a, Vector3 _b)
    {
      GraphHitInfo hit;
      return this.Linecast(_a, _b, (GraphNode) null, out hit);
    }

    public bool Linecast(Vector3 _a, Vector3 _b, GraphNode hint)
    {
      GraphHitInfo hit;
      return this.Linecast(_a, _b, hint, out hit);
    }

    public bool Linecast(Vector3 _a, Vector3 _b, GraphNode hint, out GraphHitInfo hit)
    {
      return this.Linecast(_a, _b, hint, out hit, (List<GraphNode>) null);
    }

    public bool Linecast(Vector3 _a, Vector3 _b, GraphNode hint, out GraphHitInfo hit, List<GraphNode> trace)
    {
      hit = new GraphHitInfo();
      _a = this.inverseMatrix.MultiplyPoint3x4(_a);
      _a.x -= 0.5f;
      _a.z -= 0.5f;
      _b = this.inverseMatrix.MultiplyPoint3x4(_b);
      _b.x -= 0.5f;
      _b.z -= 0.5f;
      if ((double) _a.x < -0.5 || (double) _a.z < -0.5 || ((double) _a.x >= (double) this.width - 0.5 || (double) _a.z >= (double) this.depth - 0.5) || ((double) _b.x < -0.5 || (double) _b.z < -0.5 || ((double) _b.x >= (double) this.width - 0.5 || (double) _b.z >= (double) this.depth - 0.5)))
      {
        Vector3 vector3_1 = new Vector3(-0.5f, 0.0f, -0.5f);
        Vector3 vector3_2 = new Vector3(-0.5f, 0.0f, (float) this.depth - 0.5f);
        Vector3 vector3_3 = new Vector3((float) this.width - 0.5f, 0.0f, (float) this.depth - 0.5f);
        Vector3 vector3_4 = new Vector3((float) this.width - 0.5f, 0.0f, -0.5f);
        int num = 0;
        bool intersects = false;
        Vector3 vector3_5 = Polygon.SegmentIntersectionPoint(vector3_1, vector3_2, _a, _b, out intersects);
        if (intersects)
        {
          ++num;
          if (!Polygon.Left(vector3_1, vector3_2, _a))
            _a = vector3_5;
          else
            _b = vector3_5;
        }
        Vector3 vector3_6 = Polygon.SegmentIntersectionPoint(vector3_2, vector3_3, _a, _b, out intersects);
        if (intersects)
        {
          ++num;
          if (!Polygon.Left(vector3_2, vector3_3, _a))
            _a = vector3_6;
          else
            _b = vector3_6;
        }
        Vector3 vector3_7 = Polygon.SegmentIntersectionPoint(vector3_3, vector3_4, _a, _b, out intersects);
        if (intersects)
        {
          ++num;
          if (!Polygon.Left(vector3_3, vector3_4, _a))
            _a = vector3_7;
          else
            _b = vector3_7;
        }
        Vector3 vector3_8 = Polygon.SegmentIntersectionPoint(vector3_4, vector3_1, _a, _b, out intersects);
        if (intersects)
        {
          ++num;
          if (!Polygon.Left(vector3_4, vector3_1, _a))
            _a = vector3_8;
          else
            _b = vector3_8;
        }
        if (num == 0)
          return false;
      }
      Vector3 vector3_9 = _b - _a;
      float magnitude = vector3_9.magnitude;
      if ((double) magnitude == 0.0)
        return false;
      float num1 = this.nodeSize * 0.2f - this.nodeSize * 0.02f;
      Vector3 vector3_10 = vector3_9 / magnitude * num1;
      int num2 = (int) ((double) magnitude / (double) num1);
      Vector3 vector3_11 = _a + vector3_10 * this.nodeSize * 0.01f;
      GraphNode graphNode1 = (GraphNode) null;
      for (int index = 0; index <= num2; ++index)
      {
        Vector3 vector3_1 = vector3_11 + vector3_10 * (float) index;
        int num3 = Mathf.RoundToInt(vector3_1.x);
        int num4 = Mathf.RoundToInt(vector3_1.z);
        int num5 = num3 >= 0 ? (num3 < this.width ? num3 : this.width - 1) : 0;
        GraphNode graphNode2 = (GraphNode) this.nodes[(num4 >= 0 ? (num4 < this.depth ? num4 : this.depth - 1) : 0) * this.width + num5];
        if (graphNode2 != graphNode1)
        {
          if (!graphNode2.Walkable)
          {
            hit.point = index <= 0 ? this.matrix.MultiplyPoint3x4(_a + new Vector3(0.5f, 0.0f, 0.5f)) : this.matrix.MultiplyPoint3x4(vector3_11 + vector3_10 * (float) (index - 1) + new Vector3(0.5f, 0.0f, 0.5f));
            hit.origin = this.matrix.MultiplyPoint3x4(_a + new Vector3(0.5f, 0.0f, 0.5f));
            hit.node = graphNode2;
            return true;
          }
          if (index > num2 - 1 && ((double) Mathf.Abs(vector3_1.x - _b.x) <= 0.500010013580322 || (double) Mathf.Abs(vector3_1.z - _b.z) <= 0.500010013580322))
            return false;
          if (trace != null)
            trace.Add(graphNode2);
          graphNode1 = graphNode2;
        }
      }
      return false;
    }

    public bool SnappedLinecast(Vector3 _a, Vector3 _b, GraphNode hint, out GraphHitInfo hit)
    {
      hit = new GraphHitInfo();
      GraphNode graphNode1 = this.GetNearest(_a, NNConstraint.None).node;
      GraphNode graphNode2 = this.GetNearest(_b, NNConstraint.None).node;
      _a = this.inverseMatrix.MultiplyPoint3x4((Vector3) graphNode1.position);
      _a.x -= 0.5f;
      _a.z -= 0.5f;
      _b = this.inverseMatrix.MultiplyPoint3x4((Vector3) graphNode2.position);
      _b.x -= 0.5f;
      _b.z -= 0.5f;
      Int3 int3_1 = new Int3(Mathf.RoundToInt(_a.x), Mathf.RoundToInt(_a.y), Mathf.RoundToInt(_a.z));
      Int3 int3_2 = new Int3(Mathf.RoundToInt(_b.x), Mathf.RoundToInt(_b.y), Mathf.RoundToInt(_b.z));
      hit.origin = (Vector3) int3_1;
      if (!this.nodes[int3_1.z * this.width + int3_1.x].Walkable)
      {
        hit.node = (GraphNode) this.nodes[int3_1.z * this.width + int3_1.x];
        hit.point = this.matrix.MultiplyPoint3x4(new Vector3((float) int3_1.x + 0.5f, 0.0f, (float) int3_1.z + 0.5f));
        hit.point.y = (Vector3) hit.node.position.y;
        return true;
      }
      int num1 = Mathf.Abs(int3_1.x - int3_2.x);
      int num2 = Mathf.Abs(int3_1.z - int3_2.z);
      int num3 = int3_1.x >= int3_2.x ? -1 : 1;
      int num4 = int3_1.z >= int3_2.z ? -1 : 1;
      int num5 = num1 - num2;
      while (int3_1.x != int3_2.x || int3_1.z != int3_2.z)
      {
        int num6 = num5 * 2;
        int num7 = 0;
        Int3 int3_3 = int3_1;
        if (num6 > -num2)
        {
          num5 -= num2;
          num7 = num3;
          int3_3.x += num3;
        }
        if (num6 < num1)
        {
          num5 += num1;
          num7 += this.width * num4;
          int3_3.z += num4;
        }
        if (num7 == 0)
        {
          Debug.LogError((object) "Offset is zero, this should not happen");
          return false;
        }
        for (int dir = 0; dir < this.neighbourOffsets.Length; ++dir)
        {
          if (this.neighbourOffsets[dir] == num7)
          {
            if (this.CheckConnection(this.nodes[int3_1.z * this.width + int3_1.x], dir))
            {
              if (!this.nodes[int3_3.z * this.width + int3_3.x].Walkable)
              {
                hit.node = (GraphNode) this.nodes[int3_1.z * this.width + int3_1.x];
                hit.point = this.matrix.MultiplyPoint3x4(new Vector3((float) int3_1.x + 0.5f, 0.0f, (float) int3_1.z + 0.5f));
                hit.point.y = (Vector3) hit.node.position.y;
                return true;
              }
              int3_1 = int3_3;
              break;
            }
            hit.node = (GraphNode) this.nodes[int3_1.z * this.width + int3_1.x];
            hit.point = this.matrix.MultiplyPoint3x4(new Vector3((float) int3_1.x + 0.5f, 0.0f, (float) int3_1.z + 0.5f));
            hit.point.y = (Vector3) hit.node.position.y;
            return true;
          }
        }
      }
      return false;
    }

    public bool CheckConnection(GridNode node, int dir)
    {
      if (this.neighbours == NumNeighbours.Eight)
        return this.HasNodeConnection(node, dir);
      int dir1 = dir - 4 - 1 & 3;
      int dir2 = dir - 4 + 1 & 3;
      if (!this.HasNodeConnection(node, dir1) || !this.HasNodeConnection(node, dir2))
        return false;
      GridNode node1 = this.nodes[node.NodeInGridIndex + this.neighbourOffsets[dir1]];
      GridNode node2 = this.nodes[node.NodeInGridIndex + this.neighbourOffsets[dir2]];
      return node1.Walkable && node2.Walkable && (this.HasNodeConnection(node2, dir1) && this.HasNodeConnection(node1, dir2));
    }

    public override void SerializeExtraInfo(GraphSerializationContext ctx)
    {
      if (this.nodes == null)
      {
        ctx.writer.Write(-1);
      }
      else
      {
        ctx.writer.Write(this.nodes.Length);
        for (int index = 0; index < this.nodes.Length; ++index)
          this.nodes[index].SerializeNode(ctx);
      }
    }

    public override void DeserializeExtraInfo(GraphSerializationContext ctx)
    {
      int length = ctx.reader.ReadInt32();
      if (length == -1)
      {
        this.nodes = (GridNode[]) null;
      }
      else
      {
        this.nodes = new GridNode[length];
        for (int index = 0; index < this.nodes.Length; ++index)
        {
          this.nodes[index] = new GridNode(this.active);
          this.nodes[index].DeserializeNode(ctx);
        }
      }
    }

    public override void PostDeserialization()
    {
      this.GenerateMatrix();
      this.SetUpOffsetsAndCosts();
      if (this.nodes == null || this.nodes.Length == 0)
        return;
      if (this.width * this.depth != this.nodes.Length)
      {
        Debug.LogWarning((object) "Node data did not match with bounds data. Probably a change to the bounds/width/depth data was made after scanning the graph just prior to saving it. Nodes will be discarded");
        this.nodes = new GridNode[0];
      }
      else
      {
        GridNode.SetGridGraph(AstarPath.active.astarData.GetGraphIndex((NavGraph) this), this);
        for (int index1 = 0; index1 < this.depth; ++index1)
        {
          for (int index2 = 0; index2 < this.width; ++index2)
          {
            GridNode gridNode = this.nodes[index1 * this.width + index2];
            if (gridNode == null)
            {
              Debug.LogError((object) "Deserialization Error : Couldn't cast the node to the appropriate type - GridGenerator. Check the CreateNodes function");
              return;
            }
            gridNode.NodeInGridIndex = index1 * this.width + index2;
          }
        }
      }
    }

    public class TextureData
    {
      public float[] factors = new float[3];
      public GridGraph.TextureData.ChannelUse[] channels = new GridGraph.TextureData.ChannelUse[3];
      public bool enabled;
      public Texture2D source;
      private Color32[] data;

      public void Initialize()
      {
        if (!this.enabled || !((UnityEngine.Object) this.source != (UnityEngine.Object) null))
          return;
        for (int index = 0; index < this.channels.Length; ++index)
        {
          if (this.channels[index] != GridGraph.TextureData.ChannelUse.None)
          {
            try
            {
              this.data = this.source.GetPixels32();
              break;
            }
            catch (UnityException ex)
            {
              Debug.LogWarning((object) ex.ToString());
              this.data = (Color32[]) null;
              break;
            }
          }
        }
      }

      public void Apply(GridNode node, int x, int z)
      {
        if (!this.enabled || this.data == null || (x >= this.source.width || z >= this.source.height))
          return;
        Color32 color32 = this.data[z * this.source.width + x];
        if (this.channels[0] != GridGraph.TextureData.ChannelUse.None)
          this.ApplyChannel(node, x, z, (int) color32.r, this.channels[0], this.factors[0]);
        if (this.channels[1] != GridGraph.TextureData.ChannelUse.None)
          this.ApplyChannel(node, x, z, (int) color32.g, this.channels[1], this.factors[1]);
        if (this.channels[2] == GridGraph.TextureData.ChannelUse.None)
          return;
        this.ApplyChannel(node, x, z, (int) color32.b, this.channels[2], this.factors[2]);
      }

      private void ApplyChannel(GridNode node, int x, int z, int value, GridGraph.TextureData.ChannelUse channelUse, float factor)
      {
        switch (channelUse)
        {
          case GridGraph.TextureData.ChannelUse.Penalty:
            node.Penalty += (uint) Mathf.RoundToInt((float) value * factor);
            break;
          case GridGraph.TextureData.ChannelUse.Position:
            node.position = GridNode.GetGridGraph(node.GraphIndex).GetNodePosition(node.NodeInGridIndex, Mathf.RoundToInt((float) ((double) value * (double) factor * 1000.0)));
            break;
          case GridGraph.TextureData.ChannelUse.WalkablePenalty:
            if (value == 0)
            {
              node.Walkable = false;
              break;
            }
            node.Penalty += (uint) Mathf.RoundToInt((float) (value - 1) * factor);
            break;
        }
      }

      public enum ChannelUse
      {
        None,
        Penalty,
        Position,
        WalkablePenalty,
      }
    }
  }
}
