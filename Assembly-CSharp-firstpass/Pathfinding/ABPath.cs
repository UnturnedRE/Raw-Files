// Decompiled with JetBrains decompiler
// Type: Pathfinding.ABPath
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Text;
using UnityEngine;

namespace Pathfinding
{
  public class ABPath : Path
  {
    public bool recalcStartEndCosts = true;
    protected bool hasEndPoint = true;
    public GraphNode startNode;
    public GraphNode endNode;
    public GraphNode startHint;
    public GraphNode endHint;
    public Vector3 originalStartPoint;
    public Vector3 originalEndPoint;
    public Vector3 startPoint;
    public Vector3 endPoint;
    public Int3 startIntPoint;
    public bool calculatePartial;
    protected PathNode partialBestTarget;
    protected int[] endNodeCosts;

    [Obsolete("Use PathPool<T>.GetPath instead")]
    public ABPath(Vector3 start, Vector3 end, OnPathDelegate callbackDelegate)
    {
      this.Reset();
      this.Setup(start, end, callbackDelegate);
    }

    public ABPath()
    {
    }

    public static ABPath Construct(Vector3 start, Vector3 end, OnPathDelegate callback = null)
    {
      ABPath path = PathPool<ABPath>.GetPath();
      path.Setup(start, end, callback);
      return path;
    }

    protected void Setup(Vector3 start, Vector3 end, OnPathDelegate callbackDelegate)
    {
      this.callback = callbackDelegate;
      this.UpdateStartEnd(start, end);
    }

    protected void UpdateStartEnd(Vector3 start, Vector3 end)
    {
      this.originalStartPoint = start;
      this.originalEndPoint = end;
      this.startPoint = start;
      this.endPoint = end;
      this.startIntPoint = (Int3) start;
      this.hTarget = (Int3) end;
    }

    public override uint GetConnectionSpecialCost(GraphNode a, GraphNode b, uint currentCost)
    {
      if (this.startNode != null && this.endNode != null)
      {
        if (a == this.startNode)
          return (uint) ((double) (this.startIntPoint - (b != this.endNode ? b.position : this.hTarget)).costMagnitude * ((double) currentCost * 1.0 / (double) (a.position - b.position).costMagnitude));
        if (b == this.startNode)
          return (uint) ((double) (this.startIntPoint - (a != this.endNode ? a.position : this.hTarget)).costMagnitude * ((double) currentCost * 1.0 / (double) (a.position - b.position).costMagnitude));
        if (a == this.endNode)
          return (uint) ((double) (this.hTarget - b.position).costMagnitude * ((double) currentCost * 1.0 / (double) (a.position - b.position).costMagnitude));
        if (b == this.endNode)
          return (uint) ((double) (this.hTarget - a.position).costMagnitude * ((double) currentCost * 1.0 / (double) (a.position - b.position).costMagnitude));
      }
      else
      {
        if (a == this.startNode)
          return (uint) ((double) (this.startIntPoint - b.position).costMagnitude * ((double) currentCost * 1.0 / (double) (a.position - b.position).costMagnitude));
        if (b == this.startNode)
          return (uint) ((double) (this.startIntPoint - a.position).costMagnitude * ((double) currentCost * 1.0 / (double) (a.position - b.position).costMagnitude));
      }
      return currentCost;
    }

    public override void Reset()
    {
      base.Reset();
      this.startNode = (GraphNode) null;
      this.endNode = (GraphNode) null;
      this.startHint = (GraphNode) null;
      this.endHint = (GraphNode) null;
      this.originalStartPoint = Vector3.zero;
      this.originalEndPoint = Vector3.zero;
      this.startPoint = Vector3.zero;
      this.endPoint = Vector3.zero;
      this.calculatePartial = false;
      this.partialBestTarget = (PathNode) null;
      this.hasEndPoint = true;
      this.startIntPoint = new Int3();
      this.hTarget = new Int3();
      this.endNodeCosts = (int[]) null;
    }

    public override void Prepare()
    {
      this.nnConstraint.tags = this.enabledTags;
      NNInfo nearest1 = AstarPath.active.GetNearest(this.startPoint, this.nnConstraint, this.startHint);
      PathNNConstraint pathNnConstraint = this.nnConstraint as PathNNConstraint;
      if (pathNnConstraint != null)
        pathNnConstraint.SetStart(nearest1.node);
      this.startPoint = nearest1.clampedPosition;
      this.startIntPoint = (Int3) this.startPoint;
      this.startNode = nearest1.node;
      if (this.hasEndPoint)
      {
        NNInfo nearest2 = AstarPath.active.GetNearest(this.endPoint, this.nnConstraint, this.endHint);
        this.endPoint = nearest2.clampedPosition;
        this.hTarget = (Int3) this.endPoint;
        this.endNode = nearest2.node;
        this.hTargetNode = this.endNode;
      }
      if (this.startNode == null && this.hasEndPoint && this.endNode == null)
      {
        this.Error();
        this.LogError("Couldn't find close nodes to the start point or the end point");
      }
      else if (this.startNode == null)
      {
        this.Error();
        this.LogError("Couldn't find a close node to the start point");
      }
      else if (this.endNode == null && this.hasEndPoint)
      {
        this.Error();
        this.LogError("Couldn't find a close node to the end point");
      }
      else if (!this.startNode.Walkable)
      {
        this.Error();
        this.LogError("The node closest to the start point is not walkable");
      }
      else if (this.hasEndPoint && !this.endNode.Walkable)
      {
        this.Error();
        this.LogError("The node closest to the end point is not walkable");
      }
      else
      {
        if (!this.hasEndPoint || (int) this.startNode.Area == (int) this.endNode.Area)
          return;
        this.Error();
        this.LogError("There is no valid path to the target (start area: " + (object) this.startNode.Area + ", target area: " + (string) (object) this.endNode.Area + ")");
      }
    }

    public override void Initialize()
    {
      if (this.startNode != null)
        this.pathHandler.GetPathNode(this.startNode).flag2 = true;
      if (this.endNode != null)
        this.pathHandler.GetPathNode(this.endNode).flag2 = true;
      if (this.hasEndPoint && this.startNode == this.endNode)
      {
        PathNode pathNode = this.pathHandler.GetPathNode(this.endNode);
        pathNode.node = this.endNode;
        pathNode.parent = (PathNode) null;
        pathNode.H = 0U;
        pathNode.G = 0U;
        this.Trace(pathNode);
        this.CompleteState = PathCompleteState.Complete;
      }
      else
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
        this.partialBestTarget = pathNode;
        if (this.pathHandler.HeapEmpty())
        {
          if (this.calculatePartial)
          {
            this.CompleteState = PathCompleteState.Partial;
            this.Trace(this.partialBestTarget);
          }
          else
          {
            this.Error();
            this.LogError("No open points, the start node didn't open any nodes");
            return;
          }
        }
        this.currentR = this.pathHandler.PopNode();
      }
    }

    public override void Cleanup()
    {
      if (this.startNode != null)
        this.pathHandler.GetPathNode(this.startNode).flag2 = false;
      if (this.endNode == null)
        return;
      this.pathHandler.GetPathNode(this.endNode).flag2 = false;
    }

    public override void CalculateStep(long targetTick)
    {
      int num = 0;
      while (this.CompleteState == PathCompleteState.NotCalculated)
      {
        ++this.searchedNodes;
        if (this.currentR.node == this.endNode)
        {
          this.CompleteState = PathCompleteState.Complete;
          break;
        }
        if (this.currentR.H < this.partialBestTarget.H)
          this.partialBestTarget = this.currentR;
        this.currentR.node.Open((Path) this, this.currentR, this.pathHandler);
        if (this.pathHandler.HeapEmpty())
        {
          this.Error();
          this.LogError("No open points, whole area searched");
          return;
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
      if (this.CompleteState == PathCompleteState.Complete)
      {
        this.Trace(this.currentR);
      }
      else
      {
        if (!this.calculatePartial || this.partialBestTarget == null)
          return;
        this.CompleteState = PathCompleteState.Partial;
        this.Trace(this.partialBestTarget);
      }
    }

    public void ResetCosts(Path p)
    {
    }

    public override string DebugString(PathLog logMode)
    {
      if (logMode == PathLog.None || !this.error && logMode == PathLog.OnlyErrors)
        return string.Empty;
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(!this.error ? "Path Completed : " : "Path Failed : ");
      stringBuilder.Append("Computation Time ");
      stringBuilder.Append(this.duration.ToString(logMode != PathLog.Heavy ? "0.00" : "0.000"));
      stringBuilder.Append(" ms Searched Nodes ");
      stringBuilder.Append(this.searchedNodes);
      if (!this.error)
      {
        stringBuilder.Append(" Path Length ");
        stringBuilder.Append(this.path != null ? this.path.Count.ToString() : "Null");
        if (logMode == PathLog.Heavy)
        {
          stringBuilder.Append("\nSearch Iterations " + (object) this.searchIterations);
          if (this.hasEndPoint && this.endNode != null)
          {
            PathNode pathNode = this.pathHandler.GetPathNode(this.endNode);
            stringBuilder.Append("\nEnd Node\n\tG: ");
            stringBuilder.Append(pathNode.G);
            stringBuilder.Append("\n\tH: ");
            stringBuilder.Append(pathNode.H);
            stringBuilder.Append("\n\tF: ");
            stringBuilder.Append(pathNode.F);
            stringBuilder.Append("\n\tPoint: ");
            stringBuilder.Append(this.endPoint.ToString());
            stringBuilder.Append("\n\tGraph: ");
            stringBuilder.Append(this.endNode.GraphIndex);
          }
          stringBuilder.Append("\nStart Node");
          stringBuilder.Append("\n\tPoint: ");
          stringBuilder.Append(this.startPoint.ToString());
          stringBuilder.Append("\n\tGraph: ");
          if (this.startNode != null)
            stringBuilder.Append(this.startNode.GraphIndex);
          else
            stringBuilder.Append("< null startNode >");
        }
      }
      if (this.error)
      {
        stringBuilder.Append("\nError: ");
        stringBuilder.Append(this.errorLog);
      }
      if (logMode == PathLog.Heavy && !AstarPath.IsUsingMultithreading)
      {
        stringBuilder.Append("\nCallback references ");
        if (this.callback != null)
          stringBuilder.Append(this.callback.Target.GetType().FullName).AppendLine();
        else
          stringBuilder.AppendLine("NULL");
      }
      stringBuilder.Append("\nPath Number ");
      stringBuilder.Append(this.pathID);
      return stringBuilder.ToString();
    }

    protected override void Recycle()
    {
      PathPool<ABPath>.Recycle(this);
    }

    public Vector3 GetMovementVector(Vector3 point)
    {
      if (this.vectorPath == null || this.vectorPath.Count == 0)
        return Vector3.zero;
      if (this.vectorPath.Count == 1)
        return this.vectorPath[0] - point;
      float num1 = float.PositiveInfinity;
      int num2 = 0;
      for (int index = 0; index < this.vectorPath.Count - 1; ++index)
      {
        float sqrMagnitude = (AstarMath.NearestPointStrict(this.vectorPath[index], this.vectorPath[index + 1], point) - point).sqrMagnitude;
        if ((double) sqrMagnitude < (double) num1)
        {
          num1 = sqrMagnitude;
          num2 = index;
        }
      }
      return this.vectorPath[num2 + 1] - point;
    }
  }
}
