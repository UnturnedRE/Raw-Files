// Decompiled with JetBrains decompiler
// Type: Pathfinding.LayerGridGraph
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding.Serialization;
using Pathfinding.Serialization.JsonFx;
using System;
using UnityEngine;

namespace Pathfinding
{
  public class LayerGridGraph : GridGraph, IUpdatableGraph, IRaycastableGraph
  {
    [JsonMember]
    public float mergeSpanRange = 0.5f;
    [JsonMember]
    public float characterHeight = 0.4f;
    public int[] nodeCellIndices;
    [JsonMember]
    public int layerCount;
    public LevelGridNode[] nodes;

    public override bool uniformWidthDepthGrid
    {
      get
      {
        return false;
      }
    }

    public override void OnDestroy()
    {
      base.OnDestroy();
      this.RemoveGridGraphFromStatic();
    }

    public new void RemoveGridGraphFromStatic()
    {
      LevelGridNode.SetGridGraph(this.active.astarData.GetGraphIndex((NavGraph) this), (LayerGridGraph) null);
    }

    public override void GetNodes(GraphNodeDelegateCancelable del)
    {
      if (this.nodes == null)
        return;
      int index = 0;
      while (index < this.nodes.Length && (this.nodes[index] == null || del((GraphNode) this.nodes[index])))
        ++index;
    }

    public new void UpdateArea(GraphUpdateObject o)
    {
      if (this.nodes == null || this.nodes.Length != this.width * this.depth * this.layerCount)
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
        bool flag1 = o.updatePhysics || o.modifyWalkability;
        bool flag2 = o is LayerGridGraphUpdate && ((LayerGridGraphUpdate) o).recalculateNodes;
        bool preserveExistingNodes = !(o is LayerGridGraphUpdate) || ((LayerGridGraphUpdate) o).preserveExistingNodes;
        int range = !o.updateErosion ? 0 : this.erodeIterations;
        if (o.trackChangedNodes && flag2)
        {
          Debug.LogError((object) "Cannot track changed nodes when creating or deleting nodes.\nWill not update LayerGridGraph");
        }
        else
        {
          if (o.updatePhysics && !o.modifyWalkability && this.collision.collisionCheck)
          {
            Vector3 vector3_1 = new Vector3(this.collision.diameter, 0.0f, this.collision.diameter) * 0.5f;
            Vector3 vector3_2 = min - vector3_1 * 1.02f;
            max += vector3_1 * 1.02f;
            intRect2 = new IntRect(Mathf.RoundToInt(vector3_2.x - 0.5f), Mathf.RoundToInt(vector3_2.z - 0.5f), Mathf.RoundToInt(max.x - 0.5f), Mathf.RoundToInt(max.z - 0.5f));
            intRect1 = IntRect.Union(intRect2, intRect1);
          }
          if (flag1 || range > 0)
            intRect1 = intRect1.Expand(range + 1);
          IntRect intRect3 = IntRect.Intersection(intRect1, b);
          if (!flag2)
          {
            for (int index1 = intRect3.xmin; index1 <= intRect3.xmax; ++index1)
            {
              for (int index2 = intRect3.ymin; index2 <= intRect3.ymax; ++index2)
              {
                for (int index3 = 0; index3 < this.layerCount; ++index3)
                  o.WillUpdateNode((GraphNode) this.nodes[index3 * this.width * this.depth + index2 * this.width + index1]);
              }
            }
          }
          if (o.updatePhysics && !o.modifyWalkability)
          {
            this.collision.Initialize(this.matrix, this.nodeSize);
            IntRect intRect4 = IntRect.Intersection(intRect2, b);
            bool flag3 = false;
            for (int x = intRect4.xmin; x <= intRect4.xmax; ++x)
            {
              for (int z = intRect4.ymin; z <= intRect4.ymax; ++z)
                flag3 |= this.RecalculateCell(x, z, preserveExistingNodes);
            }
            for (int x = intRect4.xmin; x <= intRect4.xmax; ++x)
            {
              for (int z = intRect4.ymin; z <= intRect4.ymax; ++z)
              {
                for (int layerIndex = 0; layerIndex < this.layerCount; ++layerIndex)
                {
                  LevelGridNode levelGridNode = this.nodes[layerIndex * this.width * this.depth + z * this.width + x];
                  if (levelGridNode != null)
                    this.CalculateConnections((GraphNode[]) this.nodes, (GraphNode) levelGridNode, x, z, layerIndex);
                }
              }
            }
          }
          IntRect intRect5 = IntRect.Intersection(a1, b);
          for (int index1 = intRect5.xmin; index1 <= intRect5.xmax; ++index1)
          {
            for (int index2 = intRect5.ymin; index2 <= intRect5.ymax; ++index2)
            {
              for (int index3 = 0; index3 < this.layerCount; ++index3)
              {
                LevelGridNode levelGridNode = this.nodes[index3 * this.width * this.depth + index2 * this.width + index1];
                if (levelGridNode != null)
                {
                  if (flag1)
                  {
                    levelGridNode.Walkable = levelGridNode.WalkableErosion;
                    if (o.bounds.Contains((Vector3) levelGridNode.position))
                      o.Apply((GraphNode) levelGridNode);
                    levelGridNode.WalkableErosion = levelGridNode.Walkable;
                  }
                  else if (o.bounds.Contains((Vector3) levelGridNode.position))
                    o.Apply((GraphNode) levelGridNode);
                }
              }
            }
          }
          if (flag1 && range == 0)
          {
            intRect5 = IntRect.Intersection(intRect1, b);
            for (int x = intRect5.xmin; x <= intRect5.xmax; ++x)
            {
              for (int z = intRect5.ymin; z <= intRect5.ymax; ++z)
              {
                for (int layerIndex = 0; layerIndex < this.layerCount; ++layerIndex)
                {
                  LevelGridNode levelGridNode = this.nodes[layerIndex * this.width * this.depth + z * this.width + x];
                  if (levelGridNode != null)
                    this.CalculateConnections((GraphNode[]) this.nodes, (GraphNode) levelGridNode, x, z, layerIndex);
                }
              }
            }
          }
          else
          {
            if (!flag1 || range <= 0)
              return;
            intRect5 = IntRect.Union(a1, intRect2);
            IntRect a2 = intRect5.Expand(range);
            IntRect a3 = a2.Expand(range);
            a2 = IntRect.Intersection(a2, b);
            IntRect intRect4 = IntRect.Intersection(a3, b);
            for (int x = intRect4.xmin; x <= intRect4.xmax; ++x)
            {
              for (int y = intRect4.ymin; y <= intRect4.ymax; ++y)
              {
                for (int index = 0; index < this.layerCount; ++index)
                {
                  LevelGridNode levelGridNode = this.nodes[index * this.width * this.depth + y * this.width + x];
                  if (levelGridNode != null)
                  {
                    bool walkable = levelGridNode.Walkable;
                    levelGridNode.Walkable = levelGridNode.WalkableErosion;
                    if (!a2.Contains(x, y))
                      levelGridNode.TmpWalkable = walkable;
                  }
                }
              }
            }
            for (int x = intRect4.xmin; x <= intRect4.xmax; ++x)
            {
              for (int z = intRect4.ymin; z <= intRect4.ymax; ++z)
              {
                for (int layerIndex = 0; layerIndex < this.layerCount; ++layerIndex)
                {
                  LevelGridNode levelGridNode = this.nodes[layerIndex * this.width * this.depth + z * this.width + x];
                  if (levelGridNode != null)
                    this.CalculateConnections((GraphNode[]) this.nodes, (GraphNode) levelGridNode, x, z, layerIndex);
                }
              }
            }
            this.ErodeWalkableArea(intRect4.xmin, intRect4.ymin, intRect4.xmax + 1, intRect4.ymax + 1);
            for (int x = intRect4.xmin; x <= intRect4.xmax; ++x)
            {
              for (int y = intRect4.ymin; y <= intRect4.ymax; ++y)
              {
                if (!a2.Contains(x, y))
                {
                  for (int index = 0; index < this.layerCount; ++index)
                  {
                    LevelGridNode levelGridNode = this.nodes[index * this.width * this.depth + y * this.width + x];
                    if (levelGridNode != null)
                      levelGridNode.Walkable = levelGridNode.TmpWalkable;
                  }
                }
              }
            }
            for (int x = intRect4.xmin; x <= intRect4.xmax; ++x)
            {
              for (int z = intRect4.ymin; z <= intRect4.ymax; ++z)
              {
                for (int layerIndex = 0; layerIndex < this.layerCount; ++layerIndex)
                {
                  LevelGridNode levelGridNode = this.nodes[layerIndex * this.width * this.depth + z * this.width + x];
                  if (levelGridNode != null)
                    this.CalculateConnections((GraphNode[]) this.nodes, (GraphNode) levelGridNode, x, z, layerIndex);
                }
              }
            }
          }
        }
      }
    }

    public override void ScanInternal(OnScanStatus status)
    {
      // ISSUE: unable to decompile the method.
    }

    public bool RecalculateCell(int x, int z, bool preserveExistingNodes)
    {
      // ISSUE: unable to decompile the method.
    }

    public void AddLayers(int count)
    {
      int num = this.layerCount + count;
      if (num > (int) byte.MaxValue)
      {
        Debug.LogError((object) ("Too many layers, a maximum of LevelGridNode.MaxLayerCount are allowed (required " + (object) num + ")"));
      }
      else
      {
        LevelGridNode[] levelGridNodeArray = this.nodes;
        this.nodes = new LevelGridNode[this.width * this.depth * num];
        for (int index = 0; index < levelGridNodeArray.Length; ++index)
          this.nodes[index] = levelGridNodeArray[index];
        this.layerCount = num;
      }
    }

    public virtual void UpdatePenalty(LevelGridNode node)
    {
      node.Penalty = 0U;
      node.Penalty = this.initialPenalty;
      if (!this.penaltyPosition)
        return;
      node.Penalty += (uint) Mathf.RoundToInt(((float) node.position.y - this.penaltyPositionOffset) * this.penaltyPositionFactor);
    }

    public override void ErodeWalkableArea(int xmin, int zmin, int xmax, int zmax)
    {
      xmin = xmin >= 0 ? (xmin <= this.width ? xmin : this.width) : 0;
      xmax = xmax >= 0 ? (xmax <= this.width ? xmax : this.width) : 0;
      zmin = zmin >= 0 ? (zmin <= this.depth ? zmin : this.depth) : 0;
      zmax = zmax >= 0 ? (zmax <= this.depth ? zmax : this.depth) : 0;
      if (this.erosionUseTags)
        Debug.LogError((object) "Erosion Uses Tags is not supported for LayerGridGraphs yet");
      for (int index1 = 0; index1 < this.erodeIterations; ++index1)
      {
        for (int index2 = 0; index2 < this.layerCount; ++index2)
        {
          for (int index3 = zmin; index3 < zmax; ++index3)
          {
            for (int index4 = xmin; index4 < xmax; ++index4)
            {
              LevelGridNode levelGridNode = this.nodes[index3 * this.width + index4 + this.width * this.depth * index2];
              if (levelGridNode != null && levelGridNode.Walkable)
              {
                bool flag = false;
                for (int i = 0; i < 4; ++i)
                {
                  if (!levelGridNode.GetConnection(i))
                  {
                    flag = true;
                    break;
                  }
                }
                if (flag)
                  levelGridNode.Walkable = false;
              }
            }
          }
        }
        for (int layerIndex = 0; layerIndex < this.layerCount; ++layerIndex)
        {
          for (int z = zmin; z < zmax; ++z)
          {
            for (int x = xmin; x < xmax; ++x)
            {
              LevelGridNode levelGridNode = this.nodes[z * this.width + x + this.width * this.depth * layerIndex];
              if (levelGridNode != null)
                this.CalculateConnections((GraphNode[]) this.nodes, (GraphNode) levelGridNode, x, z, layerIndex);
            }
          }
        }
      }
    }

    public void CalculateConnections(GraphNode[] nodes, GraphNode node, int x, int z, int layerIndex)
    {
      // ISSUE: unable to decompile the method.
    }

    public override NNInfo GetNearest(Vector3 position, NNConstraint constraint, GraphNode hint = null)
    {
      if (this.nodes == null || this.depth * this.width * this.layerCount != this.nodes.Length)
        return new NNInfo();
      position = this.inverseMatrix.MultiplyPoint3x4(position);
      int num1 = Mathf.Clamp(Mathf.RoundToInt(position.x - 0.5f), 0, this.width - 1);
      int num2 = this.width * Mathf.Clamp(Mathf.RoundToInt(position.z - 0.5f), 0, this.depth - 1) + num1;
      float num3 = float.PositiveInfinity;
      LevelGridNode levelGridNode1 = (LevelGridNode) null;
      for (int index = 0; index < this.layerCount; ++index)
      {
        LevelGridNode levelGridNode2 = this.nodes[num2 + this.width * this.depth * index];
        if (levelGridNode2 != null)
        {
          float sqrMagnitude = ((Vector3) levelGridNode2.position - position).sqrMagnitude;
          if ((double) sqrMagnitude < (double) num3)
          {
            num3 = sqrMagnitude;
            levelGridNode1 = levelGridNode2;
          }
        }
      }
      return new NNInfo((GraphNode) levelGridNode1);
    }

    private LevelGridNode GetNearestNode(Vector3 position, int x, int z, NNConstraint constraint)
    {
      int num1 = this.width * z + x;
      float num2 = float.PositiveInfinity;
      LevelGridNode levelGridNode1 = (LevelGridNode) null;
      for (int index = 0; index < this.layerCount; ++index)
      {
        LevelGridNode levelGridNode2 = this.nodes[num1 + this.width * this.depth * index];
        if (levelGridNode2 != null)
        {
          float sqrMagnitude = ((Vector3) levelGridNode2.position - position).sqrMagnitude;
          if ((double) sqrMagnitude < (double) num2 && constraint.Suitable((GraphNode) levelGridNode2))
          {
            num2 = sqrMagnitude;
            levelGridNode1 = levelGridNode2;
          }
        }
      }
      return levelGridNode1;
    }

    public override NNInfo GetNearestForce(Vector3 position, NNConstraint constraint)
    {
      if (this.nodes == null || this.depth * this.width * this.layerCount != this.nodes.Length || this.layerCount == 0)
        return new NNInfo();
      Vector3 position1 = position;
      position = this.inverseMatrix.MultiplyPoint3x4(position);
      int x1 = Mathf.Clamp(Mathf.RoundToInt(position.x - 0.5f), 0, this.width - 1);
      int z1 = Mathf.Clamp(Mathf.RoundToInt(position.z - 0.5f), 0, this.depth - 1);
      float num1 = float.PositiveInfinity;
      int num2 = 2;
      LevelGridNode levelGridNode = this.GetNearestNode(position1, x1, z1, constraint);
      if (levelGridNode != null)
        num1 = ((Vector3) levelGridNode.position - position1).sqrMagnitude;
      if (levelGridNode != null)
      {
        if (num2 == 0)
          return new NNInfo((GraphNode) levelGridNode);
        --num2;
      }
      float num3 = !constraint.constrainDistance ? float.PositiveInfinity : AstarPath.active.maxNearestNodeDistance;
      float num4 = num3 * num3;
      int num5 = 1;
      while (true)
      {
        int z2 = z1 + num5;
        if ((double) this.nodeSize * (double) num5 <= (double) num3)
        {
          for (int x2 = x1 - num5; x2 <= x1 + num5; ++x2)
          {
            if (x2 >= 0 && z2 >= 0 && (x2 < this.width && z2 < this.depth))
            {
              LevelGridNode nearestNode = this.GetNearestNode(position1, x2, z2, constraint);
              if (nearestNode != null)
              {
                float sqrMagnitude = ((Vector3) nearestNode.position - position1).sqrMagnitude;
                if ((double) sqrMagnitude < (double) num1 && (double) sqrMagnitude < (double) num4)
                {
                  num1 = sqrMagnitude;
                  levelGridNode = nearestNode;
                }
              }
            }
          }
          int z3 = z1 - num5;
          for (int x2 = x1 - num5; x2 <= x1 + num5; ++x2)
          {
            if (x2 >= 0 && z3 >= 0 && (x2 < this.width && z3 < this.depth))
            {
              LevelGridNode nearestNode = this.GetNearestNode(position1, x2, z3, constraint);
              if (nearestNode != null)
              {
                float sqrMagnitude = ((Vector3) nearestNode.position - position1).sqrMagnitude;
                if ((double) sqrMagnitude < (double) num1 && (double) sqrMagnitude < (double) num4)
                {
                  num1 = sqrMagnitude;
                  levelGridNode = nearestNode;
                }
              }
            }
          }
          int x3 = x1 - num5;
          int num6 = z1 - num5 + 1;
          for (int z4 = z1 - num5 + 1; z4 <= z1 + num5 - 1; ++z4)
          {
            if (x3 >= 0 && z4 >= 0 && (x3 < this.width && z4 < this.depth))
            {
              LevelGridNode nearestNode = this.GetNearestNode(position1, x3, z4, constraint);
              if (nearestNode != null)
              {
                float sqrMagnitude = ((Vector3) nearestNode.position - position1).sqrMagnitude;
                if ((double) sqrMagnitude < (double) num1 && (double) sqrMagnitude < (double) num4)
                {
                  num1 = sqrMagnitude;
                  levelGridNode = nearestNode;
                }
              }
            }
          }
          int x4 = x1 + num5;
          for (int z4 = z1 - num5 + 1; z4 <= z1 + num5 - 1; ++z4)
          {
            if (x4 >= 0 && z4 >= 0 && (x4 < this.width && z4 < this.depth))
            {
              LevelGridNode nearestNode = this.GetNearestNode(position1, x4, z4, constraint);
              if (nearestNode != null)
              {
                float sqrMagnitude = ((Vector3) nearestNode.position - position1).sqrMagnitude;
                if ((double) sqrMagnitude < (double) num1 && (double) sqrMagnitude < (double) num4)
                {
                  num1 = sqrMagnitude;
                  levelGridNode = nearestNode;
                }
              }
            }
          }
          if (levelGridNode != null)
          {
            if (num2 != 0)
              --num2;
            else
              goto label_41;
          }
          ++num5;
        }
        else
          break;
      }
      return new NNInfo((GraphNode) levelGridNode);
label_41:
      return new NNInfo((GraphNode) levelGridNode);
    }

    public new bool Linecast(Vector3 _a, Vector3 _b)
    {
      GraphHitInfo hit;
      return this.Linecast(_a, _b, (GraphNode) null, out hit);
    }

    public new bool Linecast(Vector3 _a, Vector3 _b, GraphNode hint)
    {
      GraphHitInfo hit;
      return this.Linecast(_a, _b, hint, out hit);
    }

    public new bool Linecast(Vector3 _a, Vector3 _b, GraphNode hint, out GraphHitInfo hit)
    {
      return this.SnappedLinecast(_a, _b, hint, out hit);
    }

    public new bool SnappedLinecast(Vector3 _a, Vector3 _b, GraphNode hint, out GraphHitInfo hit)
    {
      hit = new GraphHitInfo();
      LevelGridNode levelGridNode1 = this.GetNearest(_a, NNConstraint.None).node as LevelGridNode;
      LevelGridNode levelGridNode2 = this.GetNearest(_b, NNConstraint.None).node as LevelGridNode;
      if (levelGridNode1 == null || levelGridNode2 == null)
      {
        hit.node = (GraphNode) null;
        hit.point = _a;
        return true;
      }
      _a = this.inverseMatrix.MultiplyPoint3x4((Vector3) levelGridNode1.position);
      _a.x -= 0.5f;
      _a.z -= 0.5f;
      _b = this.inverseMatrix.MultiplyPoint3x4((Vector3) levelGridNode2.position);
      _b.x -= 0.5f;
      _b.z -= 0.5f;
      Int3 int3_1 = new Int3(Mathf.RoundToInt(_a.x), Mathf.RoundToInt(_a.y), Mathf.RoundToInt(_a.z));
      Int3 int3_2 = new Int3(Mathf.RoundToInt(_b.x), Mathf.RoundToInt(_b.y), Mathf.RoundToInt(_b.z));
      hit.origin = (Vector3) int3_1;
      if (!levelGridNode1.Walkable)
      {
        hit.node = (GraphNode) levelGridNode1;
        hit.point = this.matrix.MultiplyPoint3x4(new Vector3((float) int3_1.x + 0.5f, 0.0f, (float) int3_1.z + 0.5f));
        hit.point.y = (Vector3) hit.node.position.y;
        return true;
      }
      Mathf.Abs(int3_1.x - int3_2.x);
      Mathf.Abs(int3_1.z - int3_2.z);
      LevelGridNode levelGridNode3;
      for (LevelGridNode node = levelGridNode1; node != levelGridNode2; node = levelGridNode3)
      {
        if (node.NodeInGridIndex == levelGridNode2.NodeInGridIndex)
        {
          hit.node = (GraphNode) node;
          hit.point = (Vector3) node.position;
          return true;
        }
        int num1 = Math.Abs(int3_1.x - int3_2.x);
        int num2 = Math.Abs(int3_1.z - int3_2.z);
        int dir = 0;
        if (num1 >= num2)
          dir = int3_2.x <= int3_1.x ? 3 : 1;
        else if (num2 > num1)
          dir = int3_2.z <= int3_1.z ? 0 : 2;
        if (this.CheckConnection(node, dir))
        {
          levelGridNode3 = this.nodes[node.NodeInGridIndex + this.neighbourOffsets[dir] + this.width * this.depth * node.GetConnectionValue(dir)];
          if (!levelGridNode3.Walkable)
          {
            hit.node = (GraphNode) levelGridNode3;
            hit.point = (Vector3) levelGridNode3.position;
            return true;
          }
          int3_1 = (Int3) this.inverseMatrix.MultiplyPoint3x4((Vector3) levelGridNode3.position);
        }
        else
        {
          hit.node = (GraphNode) node;
          hit.point = (Vector3) node.position;
          return true;
        }
      }
      return false;
    }

    public bool CheckConnection(LevelGridNode node, int dir)
    {
      return node.GetConnection(dir);
    }

    public override void OnDrawGizmos(bool drawNodes)
    {
      if (!drawNodes)
        return;
      base.OnDrawGizmos(false);
      if (this.nodes == null)
        return;
      PathHandler debugPathData = AstarPath.active.debugPathData;
      for (int index1 = 0; index1 < this.nodes.Length; ++index1)
      {
        LevelGridNode levelGridNode = this.nodes[index1];
        if (levelGridNode != null && levelGridNode.Walkable)
        {
          Gizmos.color = this.NodeColor((GraphNode) levelGridNode, AstarPath.active.debugPathData);
          if (AstarPath.active.showSearchTree && AstarPath.active.debugPathData != null)
          {
            if (this.InSearchTree((GraphNode) levelGridNode, AstarPath.active.debugPath))
            {
              PathNode pathNode = debugPathData.GetPathNode((GraphNode) levelGridNode);
              if (pathNode != null && pathNode.parent != null)
                Gizmos.DrawLine((Vector3) levelGridNode.position, (Vector3) pathNode.parent.node.position);
            }
          }
          else
          {
            for (int dir = 0; dir < 4; ++dir)
            {
              int connectionValue = levelGridNode.GetConnectionValue(dir);
              if (connectionValue != (int) byte.MaxValue)
              {
                int index2 = levelGridNode.NodeInGridIndex + this.neighbourOffsets[dir] + this.width * this.depth * connectionValue;
                if (index2 >= 0 && index2 <= this.nodes.Length)
                {
                  GraphNode graphNode = (GraphNode) this.nodes[index2];
                  if (graphNode != null)
                    Gizmos.DrawLine((Vector3) levelGridNode.position, (Vector3) graphNode.position);
                }
              }
            }
          }
        }
      }
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
        {
          if (this.nodes[index] == null)
          {
            ctx.writer.Write(-1);
          }
          else
          {
            ctx.writer.Write(0);
            this.nodes[index].SerializeNode(ctx);
          }
        }
      }
    }

    public override void DeserializeExtraInfo(GraphSerializationContext ctx)
    {
      int length = ctx.reader.ReadInt32();
      if (length == -1)
      {
        this.nodes = (LevelGridNode[]) null;
      }
      else
      {
        this.nodes = new LevelGridNode[length];
        for (int index = 0; index < this.nodes.Length; ++index)
        {
          if (ctx.reader.ReadInt32() != -1)
          {
            this.nodes[index] = new LevelGridNode(this.active);
            this.nodes[index].DeserializeNode(ctx);
          }
          else
            this.nodes[index] = (LevelGridNode) null;
        }
      }
    }

    public override void PostDeserialization()
    {
      this.GenerateMatrix();
      this.SetUpOffsetsAndCosts();
      if (this.nodes == null || this.nodes.Length == 0)
        return;
      LevelGridNode.SetGridGraph(AstarPath.active.astarData.GetGraphIndex((NavGraph) this), this);
      for (int index1 = 0; index1 < this.depth; ++index1)
      {
        for (int index2 = 0; index2 < this.width; ++index2)
        {
          for (int index3 = 0; index3 < this.layerCount; ++index3)
          {
            LevelGridNode levelGridNode = this.nodes[index1 * this.width + index2 + this.width * this.depth * index3];
            if (levelGridNode != null)
              levelGridNode.NodeInGridIndex = index1 * this.width + index2;
          }
        }
      }
    }
  }
}
