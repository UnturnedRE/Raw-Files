// Decompiled with JetBrains decompiler
// Type: Pathfinding.MultiTargetPath
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding.Util;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Pathfinding
{
  public class MultiTargetPath : ABPath
  {
    public bool pathsForAll = true;
    public int chosenTarget = -1;
    public MultiTargetPath.HeuristicMode heuristicMode = MultiTargetPath.HeuristicMode.Sequential;
    public bool inverted = true;
    public OnPathDelegate[] callbacks;
    public GraphNode[] targetNodes;
    protected int targetNodeCount;
    public bool[] targetsFound;
    public Vector3[] targetPoints;
    public Vector3[] originalTargetPoints;
    public List<Vector3>[] vectorPaths;
    public List<GraphNode>[] nodePaths;
    public int endsFound;
    public int sequentialTarget;

    public MultiTargetPath()
    {
    }

    [Obsolete("Please use the Construct method instead")]
    public MultiTargetPath(Vector3[] startPoints, Vector3 target, OnPathDelegate[] callbackDelegates, OnPathDelegate callbackDelegate = null)
      : this(target, startPoints, callbackDelegates, callbackDelegate)
    {
      this.inverted = true;
    }

    [Obsolete("Please use the Construct method instead")]
    public MultiTargetPath(Vector3 start, Vector3[] targets, OnPathDelegate[] callbackDelegates, OnPathDelegate callbackDelegate = null)
    {
    }

    public static MultiTargetPath Construct(Vector3[] startPoints, Vector3 target, OnPathDelegate[] callbackDelegates, OnPathDelegate callback = null)
    {
      MultiTargetPath multiTargetPath = MultiTargetPath.Construct(target, startPoints, callbackDelegates, callback);
      multiTargetPath.inverted = true;
      return multiTargetPath;
    }

    public static MultiTargetPath Construct(Vector3 start, Vector3[] targets, OnPathDelegate[] callbackDelegates, OnPathDelegate callback = null)
    {
      MultiTargetPath path = PathPool<MultiTargetPath>.GetPath();
      path.Setup(start, targets, callbackDelegates, callback);
      return path;
    }

    protected void Setup(Vector3 start, Vector3[] targets, OnPathDelegate[] callbackDelegates, OnPathDelegate callback)
    {
      this.inverted = false;
      this.callback = callback;
      this.callbacks = callbackDelegates;
      this.targetPoints = targets;
      this.originalStartPoint = start;
      this.startPoint = start;
      this.startIntPoint = (Int3) start;
      if (targets.Length == 0)
      {
        this.Error();
        this.LogError("No targets were assigned to the MultiTargetPath");
      }
      else
      {
        this.endPoint = targets[0];
        this.originalTargetPoints = new Vector3[this.targetPoints.Length];
        for (int index = 0; index < this.targetPoints.Length; ++index)
          this.originalTargetPoints[index] = this.targetPoints[index];
      }
    }

    protected override void Recycle()
    {
      PathPool<MultiTargetPath>.Recycle(this);
    }

    public override void OnEnterPool()
    {
      if (this.vectorPaths != null)
      {
        for (int index = 0; index < this.vectorPaths.Length; ++index)
        {
          if (this.vectorPaths[index] != null)
            ListPool<Vector3>.Release(this.vectorPaths[index]);
        }
      }
      this.vectorPaths = (List<Vector3>[]) null;
      this.vectorPath = (List<Vector3>) null;
      if (this.nodePaths != null)
      {
        for (int index = 0; index < this.nodePaths.Length; ++index)
        {
          if (this.nodePaths[index] != null)
            ListPool<GraphNode>.Release(this.nodePaths[index]);
        }
      }
      this.nodePaths = (List<GraphNode>[]) null;
      this.path = (List<GraphNode>) null;
      base.OnEnterPool();
    }

    public override void ReturnPath()
    {
      if (this.error)
      {
        if (this.callbacks != null)
        {
          for (int index = 0; index < this.callbacks.Length; ++index)
          {
            if (this.callbacks[index] != null)
              this.callbacks[index]((Path) this);
          }
        }
        if (this.callback == null)
          return;
        this.callback((Path) this);
      }
      else
      {
        bool flag = false;
        Vector3 vector3_1 = this.originalStartPoint;
        Vector3 vector3_2 = this.startPoint;
        GraphNode graphNode = this.startNode;
        for (int index = 0; index < this.nodePaths.Length; ++index)
        {
          this.path = this.nodePaths[index];
          if (this.path != null)
          {
            this.CompleteState = PathCompleteState.Complete;
            flag = true;
          }
          else
            this.CompleteState = PathCompleteState.Error;
          if (this.callbacks != null && this.callbacks[index] != null)
          {
            this.vectorPath = this.vectorPaths[index];
            if (this.inverted)
            {
              this.endPoint = vector3_2;
              this.endNode = graphNode;
              this.startNode = this.targetNodes[index];
              this.startPoint = this.targetPoints[index];
              this.originalEndPoint = vector3_1;
              this.originalStartPoint = this.originalTargetPoints[index];
            }
            else
            {
              this.endPoint = this.targetPoints[index];
              this.originalEndPoint = this.originalTargetPoints[index];
              this.endNode = this.targetNodes[index];
            }
            this.callbacks[index]((Path) this);
            this.vectorPaths[index] = this.vectorPath;
          }
        }
        if (flag)
        {
          this.CompleteState = PathCompleteState.Complete;
          if (!this.pathsForAll)
          {
            this.path = this.nodePaths[this.chosenTarget];
            this.vectorPath = this.vectorPaths[this.chosenTarget];
            if (this.inverted)
            {
              this.endPoint = vector3_2;
              this.endNode = graphNode;
              this.startNode = this.targetNodes[this.chosenTarget];
              this.startPoint = this.targetPoints[this.chosenTarget];
              this.originalEndPoint = vector3_1;
              this.originalStartPoint = this.originalTargetPoints[this.chosenTarget];
            }
            else
            {
              this.endPoint = this.targetPoints[this.chosenTarget];
              this.originalEndPoint = this.originalTargetPoints[this.chosenTarget];
              this.endNode = this.targetNodes[this.chosenTarget];
            }
          }
        }
        else
          this.CompleteState = PathCompleteState.Error;
        if (this.callback == null)
          return;
        this.callback((Path) this);
      }
    }

    public void FoundTarget(PathNode nodeR, int i)
    {
      nodeR.flag1 = false;
      this.Trace(nodeR);
      this.vectorPaths[i] = this.vectorPath;
      this.nodePaths[i] = this.path;
      this.vectorPath = ListPool<Vector3>.Claim();
      this.path = ListPool<GraphNode>.Claim();
      this.targetsFound[i] = true;
      --this.targetNodeCount;
      if (!this.pathsForAll)
      {
        this.CompleteState = PathCompleteState.Complete;
        this.chosenTarget = i;
        this.targetNodeCount = 0;
      }
      else if (this.targetNodeCount <= 0)
        this.CompleteState = PathCompleteState.Complete;
      else if (this.heuristicMode == MultiTargetPath.HeuristicMode.MovingAverage)
      {
        Vector3 zero = Vector3.zero;
        int num = 0;
        for (int index = 0; index < this.targetPoints.Length; ++index)
        {
          if (!this.targetsFound[index])
          {
            zero += (Vector3) this.targetNodes[index].position;
            ++num;
          }
        }
        if (num > 0)
          zero /= (float) num;
        this.hTarget = (Int3) zero;
        this.RebuildOpenList();
      }
      else if (this.heuristicMode == MultiTargetPath.HeuristicMode.MovingMidpoint)
      {
        Vector3 rhs1 = Vector3.zero;
        Vector3 rhs2 = Vector3.zero;
        bool flag = false;
        for (int index = 0; index < this.targetPoints.Length; ++index)
        {
          if (!this.targetsFound[index])
          {
            if (!flag)
            {
              rhs1 = (Vector3) this.targetNodes[index].position;
              rhs2 = (Vector3) this.targetNodes[index].position;
              flag = true;
            }
            else
            {
              rhs1 = Vector3.Min((Vector3) this.targetNodes[index].position, rhs1);
              rhs2 = Vector3.Max((Vector3) this.targetNodes[index].position, rhs2);
            }
          }
        }
        this.hTarget = (Int3) ((rhs1 + rhs2) * 0.5f);
        this.RebuildOpenList();
      }
      else
      {
        if (this.heuristicMode != MultiTargetPath.HeuristicMode.Sequential || this.sequentialTarget != i)
          return;
        float num = 0.0f;
        for (int index = 0; index < this.targetPoints.Length; ++index)
        {
          if (!this.targetsFound[index])
          {
            float sqrMagnitude = (this.targetNodes[index].position - this.startNode.position).sqrMagnitude;
            if ((double) sqrMagnitude > (double) num)
            {
              num = sqrMagnitude;
              this.hTarget = (Int3) this.targetPoints[index];
              this.sequentialTarget = index;
            }
          }
        }
        this.RebuildOpenList();
      }
    }

    protected void RebuildOpenList()
    {
      BinaryHeapM heap = this.pathHandler.GetHeap();
      for (int i = 0; i < heap.numberOfItems; ++i)
      {
        PathNode node = heap.GetNode(i);
        node.H = this.CalculateHScore(node.node);
        heap.SetF(i, node.F);
      }
      this.pathHandler.RebuildHeap();
    }

    public override void Prepare()
    {
      this.nnConstraint.tags = this.enabledTags;
      NNInfo nearest1 = AstarPath.active.GetNearest(this.startPoint, this.nnConstraint, this.startHint);
      this.startNode = nearest1.node;
      if (this.startNode == null)
      {
        this.LogError("Could not find start node for multi target path");
        this.Error();
      }
      else if (!this.startNode.Walkable)
      {
        this.LogError("Nearest node to the start point is not walkable");
        this.Error();
      }
      else
      {
        PathNNConstraint pathNnConstraint = this.nnConstraint as PathNNConstraint;
        if (pathNnConstraint != null)
          pathNnConstraint.SetStart(nearest1.node);
        this.vectorPaths = new List<Vector3>[this.targetPoints.Length];
        this.nodePaths = new List<GraphNode>[this.targetPoints.Length];
        this.targetNodes = new GraphNode[this.targetPoints.Length];
        this.targetsFound = new bool[this.targetPoints.Length];
        this.targetNodeCount = this.targetPoints.Length;
        bool flag1 = false;
        bool flag2 = false;
        bool flag3 = false;
        for (int index = 0; index < this.targetPoints.Length; ++index)
        {
          NNInfo nearest2 = AstarPath.active.GetNearest(this.targetPoints[index], this.nnConstraint);
          this.targetNodes[index] = nearest2.node;
          this.targetPoints[index] = nearest2.clampedPosition;
          if (this.targetNodes[index] != null)
          {
            flag3 = true;
            this.endNode = this.targetNodes[index];
          }
          bool flag4 = false;
          if (nearest2.node != null && nearest2.node.Walkable)
            flag1 = true;
          else
            flag4 = true;
          if (nearest2.node != null && (int) nearest2.node.Area == (int) this.startNode.Area)
            flag2 = true;
          else
            flag4 = true;
          if (flag4)
          {
            this.targetsFound[index] = true;
            --this.targetNodeCount;
          }
        }
        this.startPoint = nearest1.clampedPosition;
        this.startIntPoint = (Int3) this.startPoint;
        if (this.startNode == null || !flag3)
        {
          this.LogError("Couldn't find close nodes to either the start or the end (start = " + (this.startNode == null ? "not found" : "found") + " end = " + (!flag3 ? "none found" : "at least one found") + ")");
          this.Error();
        }
        else if (!this.startNode.Walkable)
        {
          this.LogError("The node closest to the start point is not walkable");
          this.Error();
        }
        else if (!flag1)
        {
          this.LogError("No target nodes were walkable");
          this.Error();
        }
        else if (!flag2)
        {
          this.LogError("There are no valid paths to the targets");
          this.Error();
        }
        else if (this.pathsForAll)
        {
          if (this.heuristicMode == MultiTargetPath.HeuristicMode.None)
          {
            this.heuristic = Heuristic.None;
            this.heuristicScale = 0.0f;
          }
          else if (this.heuristicMode == MultiTargetPath.HeuristicMode.Average || this.heuristicMode == MultiTargetPath.HeuristicMode.MovingAverage)
          {
            Vector3 zero = Vector3.zero;
            for (int index = 0; index < this.targetNodes.Length; ++index)
              zero += (Vector3) this.targetNodes[index].position;
            this.hTarget = (Int3) (zero / (float) this.targetNodes.Length);
          }
          else if (this.heuristicMode == MultiTargetPath.HeuristicMode.Midpoint || this.heuristicMode == MultiTargetPath.HeuristicMode.MovingMidpoint)
          {
            Vector3 rhs1 = Vector3.zero;
            Vector3 rhs2 = Vector3.zero;
            bool flag4 = false;
            for (int index = 0; index < this.targetPoints.Length; ++index)
            {
              if (!this.targetsFound[index])
              {
                if (!flag4)
                {
                  rhs1 = (Vector3) this.targetNodes[index].position;
                  rhs2 = (Vector3) this.targetNodes[index].position;
                  flag4 = true;
                }
                else
                {
                  rhs1 = Vector3.Min((Vector3) this.targetNodes[index].position, rhs1);
                  rhs2 = Vector3.Max((Vector3) this.targetNodes[index].position, rhs2);
                }
              }
            }
            this.hTarget = (Int3) ((rhs1 + rhs2) * 0.5f);
          }
          else
          {
            if (this.heuristicMode != MultiTargetPath.HeuristicMode.Sequential)
              return;
            float num = 0.0f;
            for (int index = 0; index < this.targetNodes.Length; ++index)
            {
              if (!this.targetsFound[index])
              {
                float sqrMagnitude = (this.targetNodes[index].position - this.startNode.position).sqrMagnitude;
                if ((double) sqrMagnitude > (double) num)
                {
                  num = sqrMagnitude;
                  this.hTarget = (Int3) this.targetPoints[index];
                  this.sequentialTarget = index;
                }
              }
            }
          }
        }
        else
        {
          this.heuristic = Heuristic.None;
          this.heuristicScale = 0.0f;
        }
      }
    }

    public override void Initialize()
    {
      for (int i = 0; i < this.targetNodes.Length; ++i)
      {
        if (this.startNode == this.targetNodes[i])
          this.FoundTarget(this.pathHandler.GetPathNode(this.startNode), i);
        else if (this.targetNodes[i] != null)
          this.pathHandler.GetPathNode(this.targetNodes[i]).flag1 = true;
      }
      AstarPath.OnPathPostSearch += new OnPathDelegate(this.ResetFlags);
      if (this.targetNodeCount <= 0)
      {
        this.CompleteState = PathCompleteState.Complete;
      }
      else
      {
        PathNode pathNode = this.pathHandler.GetPathNode(this.startNode);
        pathNode.node = this.startNode;
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

    public void ResetFlags(Path p)
    {
      AstarPath.OnPathPostSearch -= new OnPathDelegate(this.ResetFlags);
      if (p != this)
        Debug.LogError((object) "This should have been cleared after it was called on 'this' path. Was it not called? Or did the delegate reset not work?");
      for (int index = 0; index < this.targetNodes.Length; ++index)
      {
        if (this.targetNodes[index] != null)
          this.pathHandler.GetPathNode(this.targetNodes[index]).flag1 = false;
      }
    }

    public override void CalculateStep(long targetTick)
    {
      int num = 0;
      while (this.CompleteState == PathCompleteState.NotCalculated)
      {
        ++this.searchedNodes;
        if (this.currentR.flag1)
        {
          for (int i = 0; i < this.targetNodes.Length; ++i)
          {
            if (!this.targetsFound[i] && this.currentR.node == this.targetNodes[i])
            {
              this.FoundTarget(this.currentR, i);
              if (this.CompleteState != PathCompleteState.NotCalculated)
                break;
            }
          }
          if (this.targetNodeCount <= 0)
          {
            this.CompleteState = PathCompleteState.Complete;
            break;
          }
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
        }
        ++num;
      }
    }

    protected override void Trace(PathNode node)
    {
      base.Trace(node);
      if (!this.inverted)
        return;
      int num = this.path.Count / 2;
      for (int index = 0; index < num; ++index)
      {
        GraphNode graphNode = this.path[index];
        this.path[index] = this.path[this.path.Count - index - 1];
        this.path[this.path.Count - index - 1] = graphNode;
      }
      for (int index = 0; index < num; ++index)
      {
        Vector3 vector3 = this.vectorPath[index];
        this.vectorPath[index] = this.vectorPath[this.vectorPath.Count - index - 1];
        this.vectorPath[this.vectorPath.Count - index - 1] = vector3;
      }
    }

    public override string DebugString(PathLog logMode)
    {
      if (logMode == PathLog.None || !this.error && logMode == PathLog.OnlyErrors)
        return string.Empty;
      StringBuilder stringBuilder = this.pathHandler.DebugStringBuilder;
      stringBuilder.Length = 0;
      stringBuilder.Append(!this.error ? "Path Completed : " : "Path Failed : ");
      stringBuilder.Append("Computation Time ");
      stringBuilder.Append(this.duration.ToString(logMode != PathLog.Heavy ? "0.00" : "0.000"));
      stringBuilder.Append(" ms Searched Nodes ");
      stringBuilder.Append(this.searchedNodes);
      if (!this.error)
      {
        stringBuilder.Append("\nLast Found Path Length ");
        stringBuilder.Append(this.path != null ? this.path.Count.ToString() : "Null");
        if (logMode == PathLog.Heavy)
        {
          stringBuilder.Append("\nSearch Iterations " + (object) this.searchIterations);
          stringBuilder.Append("\nPaths (").Append(this.targetsFound.Length).Append("):");
          for (int index = 0; index < this.targetsFound.Length; ++index)
          {
            stringBuilder.Append("\n\n\tPath " + (object) index).Append(" Found: ").Append(this.targetsFound[index]);
            GraphNode graphNode = this.nodePaths[index] != null ? this.nodePaths[index][this.nodePaths[index].Count - 1] : (GraphNode) null;
            if (this.nodePaths[index] != null)
            {
              stringBuilder.Append("\n\t\tLength: ");
              stringBuilder.Append(this.nodePaths[index].Count);
              if (graphNode != null)
              {
                PathNode pathNode = this.pathHandler.GetPathNode(this.endNode);
                if (pathNode != null)
                {
                  stringBuilder.Append("\n\t\tEnd Node");
                  stringBuilder.Append("\n\t\t\tG: ");
                  stringBuilder.Append(pathNode.G);
                  stringBuilder.Append("\n\t\t\tH: ");
                  stringBuilder.Append(pathNode.H);
                  stringBuilder.Append("\n\t\t\tF: ");
                  stringBuilder.Append(pathNode.F);
                  stringBuilder.Append("\n\t\t\tPoint: ");
                  stringBuilder.Append(this.endPoint.ToString());
                  stringBuilder.Append("\n\t\t\tGraph: ");
                  stringBuilder.Append(this.endNode.GraphIndex);
                }
                else
                  stringBuilder.Append("\n\t\tEnd Node: Null");
              }
            }
          }
          stringBuilder.Append("\nStart Node");
          stringBuilder.Append("\n\tPoint: ");
          stringBuilder.Append(this.endPoint.ToString());
          stringBuilder.Append("\n\tGraph: ");
          stringBuilder.Append(this.startNode.GraphIndex);
          stringBuilder.Append("\nBinary Heap size at completion: ");
          stringBuilder.AppendLine(this.pathHandler.GetHeap() != null ? (this.pathHandler.GetHeap().numberOfItems - 2).ToString() : "Null");
        }
      }
      if (this.error)
      {
        stringBuilder.Append("\nError: ");
        stringBuilder.Append(this.errorLog);
        stringBuilder.AppendLine();
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

    public enum HeuristicMode
    {
      None,
      Average,
      MovingAverage,
      Midpoint,
      MovingMidpoint,
      Sequential,
    }
  }
}
