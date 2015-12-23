// Decompiled with JetBrains decompiler
// Type: Pathfinding.RandomPath
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

namespace Pathfinding
{
  public class RandomPath : ABPath
  {
    private System.Random rnd = new System.Random();
    public int searchLength;
    public int spread;
    public bool uniform;
    public float aimStrength;
    private PathNode chosenNodeR;
    private PathNode maxGScoreNodeR;
    private int maxGScore;
    public Vector3 aim;
    private int nodesEvaluatedRep;

    public override bool FloodingPath
    {
      get
      {
        return true;
      }
    }

    public RandomPath()
    {
    }

    public RandomPath(Vector3 start, int length, OnPathDelegate callback = null)
    {
      throw new Exception("This constructor is obsolete. Please use the pooling API and the Setup methods");
    }

    public override void Reset()
    {
      base.Reset();
      this.searchLength = 5000;
      this.spread = 5000;
      this.uniform = true;
      this.aimStrength = 0.0f;
      this.chosenNodeR = (PathNode) null;
      this.maxGScoreNodeR = (PathNode) null;
      this.maxGScore = 0;
      this.aim = Vector3.zero;
      this.nodesEvaluatedRep = 0;
      this.hasEndPoint = false;
    }

    protected override void Recycle()
    {
      PathPool<RandomPath>.Recycle(this);
    }

    public static RandomPath Construct(Vector3 start, int length, OnPathDelegate callback = null)
    {
      RandomPath path = PathPool<RandomPath>.GetPath();
      path.Setup(start, length, callback);
      return path;
    }

    protected RandomPath Setup(Vector3 start, int length, OnPathDelegate callback)
    {
      this.callback = callback;
      this.searchLength = length;
      this.originalStartPoint = start;
      this.originalEndPoint = Vector3.zero;
      this.startPoint = start;
      this.endPoint = Vector3.zero;
      this.startIntPoint = (Int3) start;
      this.hasEndPoint = false;
      return this;
    }

    public override void ReturnPath()
    {
      if (this.path != null && this.path.Count > 0)
      {
        this.endNode = this.path[this.path.Count - 1];
        this.endPoint = (Vector3) this.endNode.position;
        this.originalEndPoint = this.endPoint;
        this.hTarget = this.endNode.position;
      }
      if (this.callback == null)
        return;
      this.callback((Path) this);
    }

    public override void Prepare()
    {
      this.nnConstraint.tags = this.enabledTags;
      NNInfo nearest = AstarPath.active.GetNearest(this.startPoint, this.nnConstraint, this.startHint);
      this.startPoint = nearest.clampedPosition;
      this.endPoint = this.startPoint;
      this.startIntPoint = (Int3) this.startPoint;
      this.hTarget = (Int3) this.aim;
      this.startNode = nearest.node;
      this.endNode = this.startNode;
      if (this.startNode == null || this.endNode == null)
      {
        this.LogError("Couldn't find close nodes to the start point");
        this.Error();
      }
      else if (!this.startNode.Walkable)
      {
        this.LogError("The node closest to the start point is not walkable");
        this.Error();
      }
      else
        this.heuristicScale = this.aimStrength;
    }

    public override void Initialize()
    {
      PathNode pathNode = this.pathHandler.GetPathNode(this.startNode);
      pathNode.node = this.startNode;
      if (this.searchLength + this.spread <= 0)
      {
        this.CompleteState = PathCompleteState.Complete;
        this.Trace(pathNode);
      }
      else
      {
        pathNode.pathID = this.pathID;
        pathNode.parent = (PathNode) null;
        pathNode.cost = 0U;
        pathNode.G = this.GetTraversalCost(this.startNode);
        pathNode.H = this.CalculateHScore(this.startNode);
        this.startNode.Open((Path) this, pathNode, this.pathHandler);
        ++this.searchedNodes;
        if (this.pathHandler.HeapEmpty())
        {
          this.LogError("No open points, the start node didn't open any nodes");
          this.Error();
        }
        else
          this.currentR = this.pathHandler.PopNode();
      }
    }

    public override void CalculateStep(long targetTick)
    {
      int num = 0;
      while (this.CompleteState == PathCompleteState.NotCalculated)
      {
        ++this.searchedNodes;
        if ((long) this.currentR.G >= (long) this.searchLength)
        {
          ++this.nodesEvaluatedRep;
          if (this.chosenNodeR == null)
            this.chosenNodeR = this.currentR;
          else if (this.rnd.NextDouble() <= 1.0 / (double) this.nodesEvaluatedRep)
            this.chosenNodeR = this.currentR;
          if ((long) this.currentR.G >= (long) (this.searchLength + this.spread))
          {
            this.CompleteState = PathCompleteState.Complete;
            break;
          }
        }
        else if ((long) this.currentR.G > (long) this.maxGScore)
        {
          this.maxGScore = (int) this.currentR.G;
          this.maxGScoreNodeR = this.currentR;
        }
        this.currentR.node.Open((Path) this, this.currentR, this.pathHandler);
        if (this.pathHandler.HeapEmpty())
        {
          if (this.chosenNodeR != null)
          {
            this.CompleteState = PathCompleteState.Complete;
            break;
          }
          if (this.maxGScoreNodeR != null)
          {
            this.chosenNodeR = this.maxGScoreNodeR;
            this.CompleteState = PathCompleteState.Complete;
            break;
          }
          this.LogError("Not a single node found to search");
          this.Error();
          break;
        }
        this.currentR = this.pathHandler.PopNode();
        if (num > 500)
        {
          if (DateTime.UtcNow.Ticks >= targetTick)
            return;
          num = 0;
          if (this.searchedNodes > 1000000)
            throw new Exception("Probable infinite loop. Over 1,000,000 nodes searched");
        }
        ++num;
      }
      if (this.CompleteState != PathCompleteState.Complete)
        return;
      this.Trace(this.chosenNodeR);
    }
  }
}
