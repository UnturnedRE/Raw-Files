// Decompiled with JetBrains decompiler
// Type: Pathfinding.ConstantPath
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
  public class ConstantPath : Path
  {
    public GraphNode startNode;
    public Vector3 startPoint;
    public Vector3 originalStartPoint;
    public List<GraphNode> allNodes;
    public PathEndingCondition endingCondition;

    public override bool FloodingPath
    {
      get
      {
        return true;
      }
    }

    public ConstantPath()
    {
    }

    [Obsolete("Please use the Construct method instead")]
    public ConstantPath(Vector3 start, OnPathDelegate callbackDelegate)
    {
      throw new Exception("This constructor is obsolete, please use the Construct method instead");
    }

    [Obsolete("Please use the Construct method instead")]
    public ConstantPath(Vector3 start, int maxGScore, OnPathDelegate callbackDelegate)
    {
      throw new Exception("This constructor is obsolete, please use the Construct method instead");
    }

    public static ConstantPath Construct(Vector3 start, int maxGScore, OnPathDelegate callback = null)
    {
      ConstantPath path = PathPool<ConstantPath>.GetPath();
      path.Setup(start, maxGScore, callback);
      return path;
    }

    protected void Setup(Vector3 start, int maxGScore, OnPathDelegate callback)
    {
      this.callback = callback;
      this.startPoint = start;
      this.originalStartPoint = this.startPoint;
      this.endingCondition = (PathEndingCondition) new EndingConditionDistance((Path) this, maxGScore);
    }

    public override void OnEnterPool()
    {
      base.OnEnterPool();
      if (this.allNodes == null)
        return;
      ListPool<GraphNode>.Release(this.allNodes);
    }

    protected override void Recycle()
    {
      PathPool<ConstantPath>.Recycle(this);
    }

    public override void Reset()
    {
      base.Reset();
      this.allNodes = ListPool<GraphNode>.Claim();
      this.endingCondition = (PathEndingCondition) null;
      this.originalStartPoint = Vector3.zero;
      this.startPoint = Vector3.zero;
      this.startNode = (GraphNode) null;
      this.heuristic = Heuristic.None;
    }

    public override void Prepare()
    {
      this.nnConstraint.tags = this.enabledTags;
      this.startNode = AstarPath.active.GetNearest(this.startPoint, this.nnConstraint).node;
      if (this.startNode != null)
        return;
      this.Error();
      this.LogError("Could not find close node to the start point");
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
      this.startNode.Open((Path) this, pathNode, this.pathHandler);
      ++this.searchedNodes;
      pathNode.flag1 = true;
      this.allNodes.Add(this.startNode);
      if (this.pathHandler.HeapEmpty())
        this.CompleteState = PathCompleteState.Complete;
      else
        this.currentR = this.pathHandler.PopNode();
    }

    public override void Cleanup()
    {
      int count = this.allNodes.Count;
      for (int index = 0; index < count; ++index)
        this.pathHandler.GetPathNode(this.allNodes[index]).flag1 = false;
    }

    public override void CalculateStep(long targetTick)
    {
      int num = 0;
      while (this.CompleteState == PathCompleteState.NotCalculated)
      {
        ++this.searchedNodes;
        if (this.endingCondition.TargetFound(this.currentR))
        {
          this.CompleteState = PathCompleteState.Complete;
          break;
        }
        if (!this.currentR.flag1)
        {
          this.allNodes.Add(this.currentR.node);
          this.currentR.flag1 = true;
        }
        this.currentR.node.Open((Path) this, this.currentR, this.pathHandler);
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
