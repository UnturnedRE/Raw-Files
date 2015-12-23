// Decompiled with JetBrains decompiler
// Type: Pathfinding.Path
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;

namespace Pathfinding
{
  public abstract class Path
  {
    private static readonly int[] ZeroTagPenalties = new int[32];
    private object stateLock = new object();
    private string _errorLog = string.Empty;
    public NNConstraint nnConstraint = (NNConstraint) PathNNConstraint.Default;
    public int walkabilityMask = -1;
    public float heuristicScale = 1f;
    public int enabledTags = -1;
    private List<object> claimed = new List<object>();
    public PathHandler pathHandler;
    public OnPathDelegate callback;
    public OnPathDelegate immediateCallback;
    private PathState state;
    private PathCompleteState pathCompleteState;
    private GraphNode[] _path;
    private Vector3[] _vectorPath;
    public List<GraphNode> path;
    public List<Vector3> vectorPath;
    protected float maxFrameTime;
    protected PathNode currentR;
    public float duration;
    public int searchIterations;
    public int searchedNodes;
    public DateTime callTime;
    public bool recycled;
    protected bool hasBeenReset;
    public Path next;
    public int radius;
    public int height;
    public int turnRadius;
    public int speed;
    public Heuristic heuristic;
    public ushort pathID;
    protected GraphNode hTargetNode;
    protected Int3 hTarget;
    protected int[] internalTagPenalties;
    protected int[] manualTagPenalties;
    private bool releasedNotSilent;

    public PathCompleteState CompleteState
    {
      get
      {
        return this.pathCompleteState;
      }
      protected set
      {
        this.pathCompleteState = value;
      }
    }

    public bool error
    {
      get
      {
        return this.CompleteState == PathCompleteState.Error;
      }
    }

    public string errorLog
    {
      get
      {
        return this._errorLog;
      }
    }

    public int[] tagPenalties
    {
      get
      {
        return this.manualTagPenalties;
      }
      set
      {
        if (value == null || value.Length != 32)
        {
          this.manualTagPenalties = (int[]) null;
          this.internalTagPenalties = Path.ZeroTagPenalties;
        }
        else
        {
          this.manualTagPenalties = value;
          this.internalTagPenalties = value;
        }
      }
    }

    public virtual bool FloodingPath
    {
      get
      {
        return false;
      }
    }

    public float GetTotalLength()
    {
      if (this.vectorPath == null)
        return float.PositiveInfinity;
      float num = 0.0f;
      for (int index = 0; index < this.vectorPath.Count - 1; ++index)
        num += Vector3.Distance(this.vectorPath[index], this.vectorPath[index + 1]);
      return num;
    }

    [DebuggerHidden]
    public IEnumerator WaitForPath()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Path.\u003CWaitForPath\u003Ec__Iterator9()
      {
        \u003C\u003Ef__this = this
      };
    }

    public uint CalculateHScore(GraphNode node)
    {
      switch (this.heuristic)
      {
        case Heuristic.Manhattan:
          Int3 int3_1 = node.position;
          return Math.Max((uint) ((double) (Math.Abs(this.hTarget.x - int3_1.x) + Math.Abs(this.hTarget.y - int3_1.y) + Math.Abs(this.hTarget.z - int3_1.z)) * (double) this.heuristicScale), this.hTargetNode == null ? 0U : AstarPath.active.euclideanEmbedding.GetHeuristic(node.NodeIndex, this.hTargetNode.NodeIndex));
        case Heuristic.DiagonalManhattan:
          Int3 int3_2 = this.GetHTarget() - node.position;
          int3_2.x = Math.Abs(int3_2.x);
          int3_2.y = Math.Abs(int3_2.y);
          int3_2.z = Math.Abs(int3_2.z);
          int num1 = Math.Min(int3_2.x, int3_2.z);
          int num2 = Math.Max(int3_2.x, int3_2.z);
          return Math.Max((uint) ((double) (14 * num1 / 10 + (num2 - num1) + int3_2.y) * (double) this.heuristicScale), this.hTargetNode == null ? 0U : AstarPath.active.euclideanEmbedding.GetHeuristic(node.NodeIndex, this.hTargetNode.NodeIndex));
        case Heuristic.Euclidean:
          return Math.Max((uint) ((double) (this.GetHTarget() - node.position).costMagnitude * (double) this.heuristicScale), this.hTargetNode == null ? 0U : AstarPath.active.euclideanEmbedding.GetHeuristic(node.NodeIndex, this.hTargetNode.NodeIndex));
        default:
          return 0U;
      }
    }

    public uint GetTagPenalty(int tag)
    {
      return (uint) this.internalTagPenalties[tag];
    }

    public Int3 GetHTarget()
    {
      return this.hTarget;
    }

    public bool CanTraverse(GraphNode node)
    {
      if (node.Walkable)
        return (this.enabledTags >> (int) node.Tag & 1) != 0;
      return false;
    }

    public uint GetTraversalCost(GraphNode node)
    {
      return this.GetTagPenalty((int) node.Tag) + node.Penalty;
    }

    public virtual uint GetConnectionSpecialCost(GraphNode a, GraphNode b, uint currentCost)
    {
      return currentCost;
    }

    public bool IsDone()
    {
      return this.CompleteState != PathCompleteState.NotCalculated;
    }

    public void AdvanceState(PathState s)
    {
      lock (this.stateLock)
        this.state = (PathState) Math.Max((int) this.state, (int) s);
    }

    public PathState GetState()
    {
      return this.state;
    }

    public void LogError(string msg)
    {
      if (AstarPath.isEditor || AstarPath.active.logPathResults != PathLog.None)
        this._errorLog += msg;
      if (AstarPath.active.logPathResults == PathLog.None || AstarPath.active.logPathResults == PathLog.InGame)
        return;
      UnityEngine.Debug.LogWarning((object) msg);
    }

    public void ForceLogError(string msg)
    {
      this.Error();
      this._errorLog += msg;
      UnityEngine.Debug.LogError((object) msg);
    }

    public void Log(string msg)
    {
      if (!AstarPath.isEditor && AstarPath.active.logPathResults == PathLog.None)
        return;
      this._errorLog += msg;
    }

    public void Error()
    {
      this.CompleteState = PathCompleteState.Error;
    }

    private void ErrorCheck()
    {
      if (!this.hasBeenReset)
        throw new Exception("The path has never been reset. Use pooling API or call Reset() after creating the path with the default constructor.");
      if (this.recycled)
        throw new Exception("The path is currently in a path pool. Are you sending the path for calculation twice?");
      if (this.pathHandler == null)
        throw new Exception("Field pathHandler is not set. Please report this bug.");
      if (this.GetState() > PathState.Processing)
        throw new Exception("This path has already been processed. Do not request a path with the same path object twice.");
    }

    public virtual void OnEnterPool()
    {
      if (this.vectorPath != null)
        ListPool<Vector3>.Release(this.vectorPath);
      if (this.path != null)
        ListPool<GraphNode>.Release(this.path);
      this.vectorPath = (List<Vector3>) null;
      this.path = (List<GraphNode>) null;
    }

    public virtual void Reset()
    {
      if (object.ReferenceEquals((object) AstarPath.active, (object) null))
        throw new NullReferenceException("No AstarPath object found in the scene. Make sure there is one or do not create paths in Awake");
      this.hasBeenReset = true;
      this.state = PathState.Created;
      this.releasedNotSilent = false;
      this.pathHandler = (PathHandler) null;
      this.callback = (OnPathDelegate) null;
      this._errorLog = string.Empty;
      this.pathCompleteState = PathCompleteState.NotCalculated;
      this.path = ListPool<GraphNode>.Claim();
      this.vectorPath = ListPool<Vector3>.Claim();
      this.currentR = (PathNode) null;
      this.duration = 0.0f;
      this.searchIterations = 0;
      this.searchedNodes = 0;
      this.nnConstraint = (NNConstraint) PathNNConstraint.Default;
      this.next = (Path) null;
      this.radius = 0;
      this.walkabilityMask = -1;
      this.height = 0;
      this.turnRadius = 0;
      this.speed = 0;
      this.heuristic = AstarPath.active.heuristic;
      this.heuristicScale = AstarPath.active.heuristicScale;
      this.pathID = (ushort) 0;
      this.enabledTags = -1;
      this.tagPenalties = (int[]) null;
      this.callTime = DateTime.UtcNow;
      this.pathID = AstarPath.active.GetNextPathID();
      this.hTarget = Int3.zero;
      this.hTargetNode = (GraphNode) null;
    }

    protected bool HasExceededTime(int searchedNodes, long targetTime)
    {
      return DateTime.UtcNow.Ticks >= targetTime;
    }

    protected abstract void Recycle();

    public void Claim(object o)
    {
      if (object.ReferenceEquals(o, (object) null))
        throw new ArgumentNullException("o");
      for (int index = 0; index < this.claimed.Count; ++index)
      {
        if (object.ReferenceEquals(this.claimed[index], o))
          throw new ArgumentException("You have already claimed the path with that object (" + o.ToString() + "). Are you claiming the path with the same object twice?");
      }
      this.claimed.Add(o);
    }

    public void ReleaseSilent(object o)
    {
      if (o == null)
        throw new ArgumentNullException("o");
      for (int index = 0; index < this.claimed.Count; ++index)
      {
        if (object.ReferenceEquals(this.claimed[index], o))
        {
          this.claimed.RemoveAt(index);
          if (!this.releasedNotSilent || this.claimed.Count != 0)
            return;
          this.Recycle();
          return;
        }
      }
      if (this.claimed.Count == 0)
        throw new ArgumentException("You are releasing a path which is not claimed at all (most likely it has been pooled already). Are you releasing the path with the same object (" + o.ToString() + ") twice?");
      throw new ArgumentException("You are releasing a path which has not been claimed with this object (" + o.ToString() + "). Are you releasing the path with the same object twice?");
    }

    public void Release(object o)
    {
      if (o == null)
        throw new ArgumentNullException("o");
      for (int index = 0; index < this.claimed.Count; ++index)
      {
        if (object.ReferenceEquals(this.claimed[index], o))
        {
          this.claimed.RemoveAt(index);
          this.releasedNotSilent = true;
          if (this.claimed.Count != 0)
            return;
          this.Recycle();
          return;
        }
      }
      if (this.claimed.Count == 0)
        throw new ArgumentException("You are releasing a path which is not claimed at all (most likely it has been pooled already). Are you releasing the path with the same object (" + o.ToString() + ") twice?");
      throw new ArgumentException("You are releasing a path which has not been claimed with this object (" + o.ToString() + "). Are you releasing the path with the same object twice?");
    }

    protected virtual void Trace(PathNode from)
    {
      int num1 = 0;
      PathNode pathNode1 = from;
      while (pathNode1 != null)
      {
        pathNode1 = pathNode1.parent;
        ++num1;
        if (num1 > 1024)
        {
          UnityEngine.Debug.LogWarning((object) "Inifinity loop? >1024 node path. Remove this message if you really have that long paths (Path.cs, Trace function)");
          break;
        }
      }
      if (this.path.Capacity < num1)
        this.path.Capacity = num1;
      if (this.vectorPath.Capacity < num1)
        this.vectorPath.Capacity = num1;
      PathNode pathNode2 = from;
      for (int index = 0; index < num1; ++index)
      {
        this.path.Add(pathNode2.node);
        pathNode2 = pathNode2.parent;
      }
      int num2 = num1 / 2;
      for (int index = 0; index < num2; ++index)
      {
        GraphNode graphNode = this.path[index];
        this.path[index] = this.path[num1 - index - 1];
        this.path[num1 - index - 1] = graphNode;
      }
      for (int index = 0; index < num1; ++index)
        this.vectorPath.Add((Vector3) this.path[index].position);
    }

    public virtual string DebugString(PathLog logMode)
    {
      if (logMode == PathLog.None || !this.error && logMode == PathLog.OnlyErrors)
        return string.Empty;
      StringBuilder stringBuilder = this.pathHandler.DebugStringBuilder;
      stringBuilder.Length = 0;
      stringBuilder.Append(!this.error ? "Path Completed : " : "Path Failed : ");
      stringBuilder.Append("Computation Time ");
      stringBuilder.Append(this.duration.ToString(logMode != PathLog.Heavy ? "0.00 ms " : "0.000 ms "));
      stringBuilder.Append("Searched Nodes ");
      stringBuilder.Append(this.searchedNodes);
      if (!this.error)
      {
        stringBuilder.Append(" Path Length ");
        stringBuilder.Append(this.path != null ? this.path.Count.ToString() : "Null");
        if (logMode == PathLog.Heavy)
          stringBuilder.Append("\nSearch Iterations " + (object) this.searchIterations);
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

    public virtual void ReturnPath()
    {
      if (this.callback == null)
        return;
      this.callback(this);
    }

    public void PrepareBase(PathHandler pathHandler)
    {
      if ((int) pathHandler.PathID > (int) this.pathID)
        pathHandler.ClearPathIDs();
      this.pathHandler = pathHandler;
      pathHandler.InitializeForPath(this);
      if (this.internalTagPenalties != null)
      {
        if (this.internalTagPenalties.Length == 32)
          goto label_5;
      }
      this.internalTagPenalties = Path.ZeroTagPenalties;
label_5:
      try
      {
        this.ErrorCheck();
      }
      catch (Exception ex)
      {
        this.ForceLogError(string.Concat(new object[4]
        {
          (object) "Exception in path ",
          (object) this.pathID,
          (object) "\n",
          (object) ex.ToString()
        }));
      }
    }

    public abstract void Prepare();

    public virtual void Cleanup()
    {
    }

    public abstract void Initialize();

    public abstract void CalculateStep(long targetTick);
  }
}
