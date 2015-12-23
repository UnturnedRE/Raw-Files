// Decompiled with JetBrains decompiler
// Type: Pathfinding.EuclideanEmbedding
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
  [Serializable]
  public class EuclideanEmbedding
  {
    public int spreadOutCount = 1;
    private uint[] costs = new uint[8];
    private uint ra = 12820163U;
    private uint rc = 1140671485U;
    private object lockObj = new object();
    public HeuristicOptimizationMode mode;
    public int seed;
    public Transform pivotPointRoot;
    private int maxNodeIndex;
    private int pivotCount;
    [NonSerialized]
    public bool dirty;
    private GraphNode[] pivots;
    private uint rval;

    public uint GetRandom()
    {
      this.rval = this.ra * this.rval + this.rc;
      return this.rval;
    }

    private void EnsureCapacity(int index)
    {
      if (index <= this.maxNodeIndex)
        return;
      lock (this.lockObj)
      {
        if (index <= this.maxNodeIndex)
          return;
        if (index >= this.costs.Length)
        {
          uint[] local_1 = new uint[Math.Max(index * 2, this.pivots.Length * 2)];
          for (int local_2 = 0; local_2 < this.costs.Length; ++local_2)
            local_1[local_2] = this.costs[local_2];
          this.costs = local_1;
        }
        this.maxNodeIndex = index;
      }
    }

    public uint GetHeuristic(int nodeIndex1, int nodeIndex2)
    {
      nodeIndex1 *= this.pivotCount;
      nodeIndex2 *= this.pivotCount;
      if (nodeIndex1 >= this.costs.Length || nodeIndex2 >= this.costs.Length)
        this.EnsureCapacity(nodeIndex1 <= nodeIndex2 ? nodeIndex2 : nodeIndex1);
      uint num1 = 0U;
      for (int index = 0; index < this.pivotCount; ++index)
      {
        uint num2 = (uint) Math.Abs((int) this.costs[nodeIndex1 + index] - (int) this.costs[nodeIndex2 + index]);
        if (num2 > num1)
          num1 = num2;
      }
      return num1;
    }

    private void GetClosestWalkableNodesToChildrenRecursively(Transform tr, List<GraphNode> nodes)
    {
      foreach (Transform transform in tr)
      {
        NNInfo nearest = AstarPath.active.GetNearest(transform.position, NNConstraint.Default);
        if (nearest.node != null && nearest.node.Walkable)
          nodes.Add(nearest.node);
        this.GetClosestWalkableNodesToChildrenRecursively(tr, nodes);
      }
    }

    public void RecalculatePivots()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      EuclideanEmbedding.\u003CRecalculatePivots\u003Ec__AnonStorey2F pivotsCAnonStorey2F = new EuclideanEmbedding.\u003CRecalculatePivots\u003Ec__AnonStorey2F();
      // ISSUE: reference to a compiler-generated field
      pivotsCAnonStorey2F.\u003C\u003Ef__this = this;
      if (this.mode == HeuristicOptimizationMode.None)
      {
        this.pivotCount = 0;
        this.pivots = (GraphNode[]) null;
      }
      else
      {
        this.rval = (uint) this.seed;
        NavGraph[] graphs = AstarPath.active.graphs;
        // ISSUE: reference to a compiler-generated field
        pivotsCAnonStorey2F.pivotList = ListPool<GraphNode>.Claim();
        if (this.mode == HeuristicOptimizationMode.Custom)
        {
          if ((UnityEngine.Object) this.pivotPointRoot == (UnityEngine.Object) null)
            throw new Exception("Grid Graph -> heuristicOptimizationMode is HeuristicOptimizationMode.Custom, but no 'customHeuristicOptimizationPivotsRoot' is set");
          // ISSUE: reference to a compiler-generated field
          this.GetClosestWalkableNodesToChildrenRecursively(this.pivotPointRoot, pivotsCAnonStorey2F.pivotList);
        }
        else if (this.mode == HeuristicOptimizationMode.Random)
        {
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          EuclideanEmbedding.\u003CRecalculatePivots\u003Ec__AnonStorey2E pivotsCAnonStorey2E = new EuclideanEmbedding.\u003CRecalculatePivots\u003Ec__AnonStorey2E();
          // ISSUE: reference to a compiler-generated field
          pivotsCAnonStorey2E.\u003C\u003Ef__ref\u002447 = pivotsCAnonStorey2F;
          // ISSUE: reference to a compiler-generated field
          pivotsCAnonStorey2E.\u003C\u003Ef__this = this;
          // ISSUE: reference to a compiler-generated field
          pivotsCAnonStorey2E.n = 0;
          for (int index = 0; index < graphs.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated method
            graphs[index].GetNodes(new GraphNodeDelegateCancelable(pivotsCAnonStorey2E.\u003C\u003Em__24));
          }
        }
        else
        {
          if (this.mode != HeuristicOptimizationMode.RandomSpreadOut)
            throw new Exception("Invalid HeuristicOptimizationMode: " + (object) this.mode);
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          EuclideanEmbedding.\u003CRecalculatePivots\u003Ec__AnonStorey30 pivotsCAnonStorey30 = new EuclideanEmbedding.\u003CRecalculatePivots\u003Ec__AnonStorey30();
          // ISSUE: reference to a compiler-generated field
          pivotsCAnonStorey30.first = (GraphNode) null;
          if ((UnityEngine.Object) this.pivotPointRoot != (UnityEngine.Object) null)
          {
            // ISSUE: reference to a compiler-generated field
            this.GetClosestWalkableNodesToChildrenRecursively(this.pivotPointRoot, pivotsCAnonStorey2F.pivotList);
          }
          else
          {
            for (int index = 0; index < graphs.Length; ++index)
            {
              // ISSUE: reference to a compiler-generated method
              graphs[index].GetNodes(new GraphNodeDelegateCancelable(pivotsCAnonStorey30.\u003C\u003Em__25));
            }
            // ISSUE: reference to a compiler-generated field
            if (pivotsCAnonStorey30.first != null)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              pivotsCAnonStorey2F.pivotList.Add(pivotsCAnonStorey30.first);
            }
            else
            {
              Debug.LogError((object) "Could not find any walkable node in any of the graphs.");
              // ISSUE: reference to a compiler-generated field
              ListPool<GraphNode>.Release(pivotsCAnonStorey2F.pivotList);
              return;
            }
          }
          for (int index = 0; index < this.spreadOutCount; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            pivotsCAnonStorey2F.pivotList.Add((GraphNode) null);
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.pivots = pivotsCAnonStorey2F.pivotList.ToArray();
        // ISSUE: reference to a compiler-generated field
        ListPool<GraphNode>.Release(pivotsCAnonStorey2F.pivotList);
      }
    }

    public void RecalculateCosts()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      EuclideanEmbedding.\u003CRecalculateCosts\u003Ec__AnonStorey33 costsCAnonStorey33 = new EuclideanEmbedding.\u003CRecalculateCosts\u003Ec__AnonStorey33();
      // ISSUE: reference to a compiler-generated field
      costsCAnonStorey33.\u003C\u003Ef__this = this;
      if (this.pivots == null)
        this.RecalculatePivots();
      if (this.mode == HeuristicOptimizationMode.None)
        return;
      this.pivotCount = 0;
      for (int index = 0; index < this.pivots.Length; ++index)
      {
        if (this.pivots[index] != null && (this.pivots[index].Destroyed || !this.pivots[index].Walkable))
          throw new Exception("Invalid pivot nodes (destroyed or unwalkable)");
      }
      if (this.mode != HeuristicOptimizationMode.RandomSpreadOut)
      {
        for (int index = 0; index < this.pivots.Length; ++index)
        {
          if (this.pivots[index] == null)
            throw new Exception("Invalid pivot nodes (null)");
        }
      }
      Debug.Log((object) "Recalculating costs...");
      this.pivotCount = this.pivots.Length;
      // ISSUE: reference to a compiler-generated field
      costsCAnonStorey33.startCostCalculation = (Action<int>) null;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      costsCAnonStorey33.startCostCalculation = new Action<int>(costsCAnonStorey33.\u003C\u003Em__26);
      if (this.mode != HeuristicOptimizationMode.RandomSpreadOut)
      {
        for (int index = 0; index < this.pivots.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          costsCAnonStorey33.startCostCalculation(index);
        }
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        costsCAnonStorey33.startCostCalculation(0);
      }
      this.dirty = false;
    }

    public void OnDrawGizmos()
    {
      if (this.pivots == null)
        return;
      for (int index = 0; index < this.pivots.Length; ++index)
      {
        Gizmos.color = new Color(0.6235294f, 0.3686275f, 0.7607843f, 0.8f);
        if (this.pivots[index] != null && !this.pivots[index].Destroyed)
          Gizmos.DrawCube((Vector3) this.pivots[index].position, Vector3.one);
      }
    }
  }
}
