// Decompiled with JetBrains decompiler
// Type: Pathfinding.FloodPath
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
  public class FloodPath : Path
  {
    public bool saveParents = true;
    public Vector3 originalStartPoint;
    public Vector3 startPoint;
    public GraphNode startNode;
    protected Dictionary<GraphNode, GraphNode> parents;

    public override bool FloodingPath
    {
      get
      {
        return true;
      }
    }

    [Obsolete("Please use the Construct method instead")]
    public FloodPath(Vector3 start, OnPathDelegate callbackDelegate)
    {
      this.Setup(start, callbackDelegate);
      this.heuristic = Heuristic.None;
    }

    public FloodPath()
    {
    }

    public bool HasPathTo(GraphNode node)
    {
      if (this.parents != null)
        return this.parents.ContainsKey(node);
      return false;
    }

    public GraphNode GetParent(GraphNode node)
    {
      return this.parents[node];
    }

    public static FloodPath Construct(Vector3 start, OnPathDelegate callback = null)
    {
      FloodPath path = PathPool<FloodPath>.GetPath();
      path.Setup(start, callback);
      return path;
    }

    public static FloodPath Construct(GraphNode start, OnPathDelegate callback = null)
    {
      if (start == null)
        throw new ArgumentNullException("start");
      FloodPath path = PathPool<FloodPath>.GetPath();
      path.Setup(start, callback);
      return path;
    }

    protected void Setup(Vector3 start, OnPathDelegate callback)
    {
      this.callback = callback;
      this.originalStartPoint = start;
      this.startPoint = start;
      this.heuristic = Heuristic.None;
    }

    protected void Setup(GraphNode start, OnPathDelegate callback)
    {
      this.callback = callback;
      this.originalStartPoint = (Vector3) start.position;
      this.startNode = start;
      this.startPoint = (Vector3) start.position;
      this.heuristic = Heuristic.None;
    }

    public override void Reset()
    {
      base.Reset();
      this.originalStartPoint = Vector3.zero;
      this.startPoint = Vector3.zero;
      this.startNode = (GraphNode) null;
      this.parents = new Dictionary<GraphNode, GraphNode>();
      this.saveParents = true;
    }

    protected override void Recycle()
    {
      PathPool<FloodPath>.Recycle(this);
    }

    public override void Prepare()
    {
      if (this.startNode == null)
      {
        this.nnConstraint.tags = this.enabledTags;
        NNInfo nearest = AstarPath.active.GetNearest(this.originalStartPoint, this.nnConstraint);
        this.startPoint = nearest.clampedPosition;
        this.startNode = nearest.node;
      }
      else
        this.startPoint = (Vector3) this.startNode.position;
      if (this.startNode == null)
      {
        this.Error();
        this.LogError("Couldn't find a close node to the start point");
      }
      else
      {
        if (this.startNode.Walkable)
          return;
        this.Error();
        this.LogError("The node closest to the start point is not walkable");
      }
    }

    public override void Initialize()
    {
      PathNode pathNode = this.pathHandler.GetPathNode(this.startNode);
      pathNode.node = this.startNode;
      pathNode.pathID = this.pathHandler.PathID;
      pathNode.parent = (PathNode) null;
      pathNode.cost = 0U;
      pathNode.G = this.GetTraversalCost(this.startNode);
      pathNode.H = this.CalculateHScore(this.startNode);
      this.parents[this.startNode] = (GraphNode) null;
      this.startNode.Open((Path) this, pathNode, this.pathHandler);
      ++this.searchedNodes;
      if (this.pathHandler.HeapEmpty())
        this.CompleteState = PathCompleteState.Complete;
      this.currentR = this.pathHandler.PopNode();
    }

    public override void CalculateStep(long targetTick)
    {
      int num = 0;
      while (this.CompleteState == PathCompleteState.NotCalculated)
      {
        ++this.searchedNodes;
        this.currentR.node.Open((Path) this, this.currentR, this.pathHandler);
        if (this.saveParents)
          this.parents[this.currentR.node] = this.currentR.parent.node;
        if (this.pathHandler.HeapEmpty())
        {
          this.CompleteState = PathCompleteState.Complete;
          break;
        }
        this.currentR = this.pathHandler.PopNode();
        if (num > 500)
        {
          if (DateTime.UtcNow.Ticks >= targetTick)
            break;
          num = 0;
          if (this.searchedNodes > 1000000)
            throw new Exception("Probable infinite loop. Over 1,000,000 nodes searched");
        }
        ++num;
      }
    }
  }
}
