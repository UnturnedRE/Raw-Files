// Decompiled with JetBrains decompiler
// Type: AstarPath
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding;
using Pathfinding.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

[AddComponentMenu("Pathfinding/Pathfinder")]
public class AstarPath : MonoBehaviour
{
  public static readonly AstarPath.AstarDistribution Distribution = AstarPath.AstarDistribution.WebsiteDownload;
  public static readonly string Branch = "master_Pro";
  public static readonly bool HasPro = true;
  public static int PathsCompleted = 0;
  public static long TotalSearchedNodes = 0L;
  public static long TotalSearchTime = 0L;
  private static PathThreadInfo[] threadInfos = new PathThreadInfo[0];
  private static LockFreeStack pathReturnStack = new LockFreeStack();
  public static bool isEditor = true;
  private static readonly object safeUpdateLock = new object();
  private static int waitForPathDepth = 0;
  public bool showNavGraphs = true;
  public bool showUnwalkableNodes = true;
  public float debugRoof = 20000f;
  public float unwalkableNodeDebugSize = 0.3f;
  public PathLog logPathResults = PathLog.Normal;
  public float maxNearestNodeDistance = 100f;
  public bool scanOnStartup = true;
  public float prioritizeGraphsLimit = 1f;
  public Heuristic heuristic = Heuristic.Euclidean;
  public float heuristicScale = 1f;
  public float maxFrameTime = 1f;
  public bool limitGraphUpdates = true;
  public float maxGraphUpdateFreq = 0.2f;
  private ThreadControlQueue pathQueue = new ThreadControlQueue(0);
  public EuclideanEmbedding euclideanEmbedding = new EuclideanEmbedding();
  private float lastGraphUpdate = -9999f;
  private ushort nextFreePathID = (ushort) 1;
  private Queue<AstarPath.AstarWorkItem> workItems = new Queue<AstarPath.AstarWorkItem>();
  private AutoResetEvent graphUpdateAsyncEvent = new AutoResetEvent(false);
  private ManualResetEvent processingGraphUpdatesAsync = new ManualResetEvent(true);
  private Queue<AstarPath.GUOSingle> graphUpdateQueueAsync = new Queue<AstarPath.GUOSingle>();
  private Queue<AstarPath.GUOSingle> graphUpdateQueueRegular = new Queue<AstarPath.GUOSingle>();
  private int nextNodeIndex = 1;
  private Stack<int> nodeIndexPool = new Stack<int>();
  public AstarData astarData;
  public static AstarPath active;
  public GraphDebugMode debugMode;
  public float debugFloor;
  public bool showSearchTree;
  public bool fullGetNearestSearch;
  public bool prioritizeGraphs;
  public AstarColor colorSettings;
  [SerializeField]
  protected string[] tagNames;
  public ThreadCount threadCount;
  public int minAreaSize;
  public float lastScanTime;
  public Path debugPath;
  public string inGameDebugPath;
  public bool isScanning;
  private bool graphUpdateRoutineRunning;
  private bool isRegisteredForUpdate;
  public static OnVoidDelegate OnAwakeSettings;
  public static OnGraphDelegate OnGraphPreScan;
  public static OnGraphDelegate OnGraphPostScan;
  public static OnPathDelegate OnPathPreSearch;
  public static OnPathDelegate OnPathPostSearch;
  public static OnScanDelegate OnPreScan;
  public static OnScanDelegate OnPostScan;
  public static OnScanDelegate OnLatePostScan;
  public static OnScanDelegate OnGraphsUpdated;
  public static OnVoidDelegate On65KOverflow;
  private static OnVoidDelegate OnThreadSafeCallback;
  public OnVoidDelegate OnDrawGizmosCallback;
  [Obsolete]
  public OnVoidDelegate OnGraphsWillBeUpdated;
  [Obsolete]
  public OnVoidDelegate OnGraphsWillBeUpdated2;
  [NonSerialized]
  public Queue<GraphUpdateObject> graphUpdateQueue;
  [NonSerialized]
  public Stack<GraphNode> floodStack;
  private static Thread[] threads;
  private Thread graphUpdateThread;
  private static IEnumerator threadEnumerator;
  public bool showGraphs;
  public uint lastUniqueAreaIndex;
  private bool workItemsQueued;
  private bool queuedWorkItemFloodFill;
  private bool processingWorkItems;
  private Path pathReturnPop;

  public static Version Version
  {
    get
    {
      return new Version(3, 5, 9, 1);
    }
  }

  public System.Type[] graphTypes
  {
    get
    {
      return this.astarData.graphTypes;
    }
  }

  public NavGraph[] graphs
  {
    get
    {
      if (this.astarData == null)
        this.astarData = new AstarData();
      return this.astarData.graphs;
    }
    set
    {
      if (this.astarData == null)
        this.astarData = new AstarData();
      this.astarData.graphs = value;
    }
  }

  public float maxNearestNodeDistanceSqr
  {
    get
    {
      return this.maxNearestNodeDistance * this.maxNearestNodeDistance;
    }
  }

  public PathHandler debugPathData
  {
    get
    {
      if (this.debugPath == null)
        return (PathHandler) null;
      return this.debugPath.pathHandler;
    }
  }

  public static int NumParallelThreads
  {
    get
    {
      if (AstarPath.threadInfos != null)
        return AstarPath.threadInfos.Length;
      return 0;
    }
  }

  public static bool IsUsingMultithreading
  {
    get
    {
      if (AstarPath.threads != null && AstarPath.threads.Length > 0)
        return true;
      if (AstarPath.threads != null && AstarPath.threads.Length == 0 && AstarPath.threadEnumerator != null || !Application.isPlaying)
        return false;
      throw new Exception(string.Concat(new object[4]
      {
        (object) "Not 'using threading' and not 'not using threading'... Are you sure pathfinding is set up correctly?\nIf scripts are reloaded in unity editor during play this could happen.\n",
        (object) (AstarPath.threads == null ? "NULL" : string.Empty + (object) AstarPath.threads.Length),
        (object) " ",
        (object) (bool) (AstarPath.threadEnumerator != null ? 1 : 0)
      }));
    }
  }

  public bool IsAnyGraphUpdatesQueued
  {
    get
    {
      if (this.graphUpdateQueue != null)
        return this.graphUpdateQueue.Count > 0;
      return false;
    }
  }

  public string[] GetTagNames()
  {
    if (this.tagNames == null || this.tagNames.Length != 32)
    {
      this.tagNames = new string[32];
      for (int index = 0; index < this.tagNames.Length; ++index)
        this.tagNames[index] = string.Empty + (object) index;
      this.tagNames[0] = "Basic Ground";
    }
    return this.tagNames;
  }

  public static string[] FindTagNames()
  {
    if ((UnityEngine.Object) AstarPath.active != (UnityEngine.Object) null)
      return AstarPath.active.GetTagNames();
    AstarPath astarPath = UnityEngine.Object.FindObjectOfType(typeof (AstarPath)) as AstarPath;
    if ((UnityEngine.Object) astarPath != (UnityEngine.Object) null)
    {
      AstarPath.active = astarPath;
      return astarPath.GetTagNames();
    }
    return new string[1]
    {
      "There is no AstarPath component in the scene"
    };
  }

  public ushort GetNextPathID()
  {
    if ((int) this.nextFreePathID == 0)
    {
      ++this.nextFreePathID;
      if (AstarPath.On65KOverflow != null)
      {
        OnVoidDelegate onVoidDelegate = AstarPath.On65KOverflow;
        AstarPath.On65KOverflow = (OnVoidDelegate) null;
        onVoidDelegate();
      }
    }
    return this.nextFreePathID++;
  }

  private void OnDrawGizmos()
  {
    if ((UnityEngine.Object) AstarPath.active == (UnityEngine.Object) null)
      AstarPath.active = this;
    else if ((UnityEngine.Object) AstarPath.active != (UnityEngine.Object) this)
      return;
    if (this.graphs == null || this.pathQueue != null && this.pathQueue.AllReceiversBlocked && this.workItems.Count > 0)
      return;
    for (int index = 0; index < this.graphs.Length; ++index)
    {
      if (this.graphs[index] != null && this.graphs[index].drawGizmos)
        this.graphs[index].OnDrawGizmos(this.showNavGraphs);
    }
    if (this.showNavGraphs)
      this.euclideanEmbedding.OnDrawGizmos();
    if (this.showUnwalkableNodes && this.showNavGraphs)
    {
      Gizmos.color = AstarColor.UnwalkableNode;
      GraphNodeDelegateCancelable del = new GraphNodeDelegateCancelable(this.DrawUnwalkableNode);
      for (int index = 0; index < this.graphs.Length; ++index)
      {
        if (this.graphs[index] != null)
          this.graphs[index].GetNodes(del);
      }
    }
    if (this.OnDrawGizmosCallback == null)
      return;
    this.OnDrawGizmosCallback();
  }

  private bool DrawUnwalkableNode(GraphNode node)
  {
    if (!node.Walkable)
      Gizmos.DrawCube((Vector3) node.position, Vector3.one * this.unwalkableNodeDebugSize);
    return true;
  }

  private void OnGUI()
  {
    if (this.logPathResults != PathLog.InGame || !(this.inGameDebugPath != string.Empty))
      return;
    GUI.Label(new Rect(5f, 5f, 400f, 600f), this.inGameDebugPath);
  }

  private static void AstarLog(string s)
  {
    if (object.ReferenceEquals((object) AstarPath.active, (object) null))
    {
      UnityEngine.Debug.Log((object) ("No AstarPath object was found : " + s));
    }
    else
    {
      if (AstarPath.active.logPathResults == PathLog.None || AstarPath.active.logPathResults == PathLog.OnlyErrors || !Application.isEditor)
        return;
      UnityEngine.Debug.Log((object) s);
    }
  }

  private static void AstarLogError(string s)
  {
    if ((UnityEngine.Object) AstarPath.active == (UnityEngine.Object) null)
    {
      UnityEngine.Debug.Log((object) ("No AstarPath object was found : " + s));
    }
    else
    {
      if (AstarPath.active.logPathResults == PathLog.None)
        return;
      UnityEngine.Debug.LogError((object) s);
    }
  }

  private void LogPathResults(Path p)
  {
    if (this.logPathResults == PathLog.None || this.logPathResults == PathLog.OnlyErrors && !p.error)
      return;
    string str = p.DebugString(this.logPathResults);
    if (this.logPathResults == PathLog.InGame)
      this.inGameDebugPath = str;
    else
      UnityEngine.Debug.Log((object) str);
  }

  private void Update()
  {
    this.PerformBlockingActions(false, true);
    if (AstarPath.threadEnumerator != null)
    {
      try
      {
        AstarPath.threadEnumerator.MoveNext();
      }
      catch (Exception ex)
      {
        AstarPath.threadEnumerator = (IEnumerator) null;
        if (!(ex is ThreadControlQueue.QueueTerminationException))
        {
          UnityEngine.Debug.LogException(ex);
          UnityEngine.Debug.LogError((object) "Unhandled exception during pathfinding. Terminating.");
          this.pathQueue.TerminateReceivers();
          try
          {
            this.pathQueue.PopNoBlock(false);
          }
          catch
          {
          }
        }
      }
    }
    this.ReturnPaths(true);
  }

  private void PerformBlockingActions(bool force = false, bool unblockOnComplete = true)
  {
    if (!this.pathQueue.AllReceiversBlocked)
      return;
    this.ReturnPaths(false);
    if (AstarPath.OnThreadSafeCallback != null)
    {
      OnVoidDelegate onVoidDelegate = AstarPath.OnThreadSafeCallback;
      AstarPath.OnThreadSafeCallback = (OnVoidDelegate) null;
      onVoidDelegate();
    }
    if (this.ProcessWorkItems(force) != 2)
      return;
    this.workItemsQueued = false;
    if (!unblockOnComplete)
      return;
    if (this.euclideanEmbedding.dirty)
      this.euclideanEmbedding.RecalculateCosts();
    this.pathQueue.Unblock();
  }

  public void QueueWorkItemFloodFill()
  {
    if (!this.pathQueue.AllReceiversBlocked)
      throw new Exception("You are calling QueueWorkItemFloodFill from outside a WorkItem. This might cause unexpected behaviour.");
    this.queuedWorkItemFloodFill = true;
  }

  public void EnsureValidFloodFill()
  {
    if (!this.queuedWorkItemFloodFill)
      return;
    this.FloodFill();
  }

  public void AddWorkItem(AstarPath.AstarWorkItem itm)
  {
    this.workItems.Enqueue(itm);
    if (this.workItemsQueued)
      return;
    this.workItemsQueued = true;
    if (this.isScanning)
      return;
    AstarPath.InterruptPathfinding();
  }

  private int ProcessWorkItems(bool force)
  {
    if (!this.pathQueue.AllReceiversBlocked)
      return 0;
    if (this.processingWorkItems)
      throw new Exception("Processing work items recursively. Please do not wait for other work items to be completed inside work items. If you think this is not caused by any of your scripts, this might be a bug.");
    this.processingWorkItems = true;
    while (this.workItems.Count > 0)
    {
      AstarPath.AstarWorkItem astarWorkItem = this.workItems.Peek();
      if (astarWorkItem.init != null)
      {
        astarWorkItem.init();
        astarWorkItem.init = (OnVoidDelegate) null;
      }
      bool flag;
      try
      {
        flag = astarWorkItem.update == null || astarWorkItem.update(force);
      }
      catch
      {
        this.workItems.Dequeue();
        this.processingWorkItems = false;
        throw;
      }
      if (!flag)
      {
        if (force)
          UnityEngine.Debug.LogError((object) "Misbehaving WorkItem. 'force'=true but the work item did not complete.\nIf force=true is passed to a WorkItem it should always return true.");
        this.processingWorkItems = false;
        return 1;
      }
      this.workItems.Dequeue();
    }
    this.EnsureValidFloodFill();
    this.processingWorkItems = false;
    return 2;
  }

  public void QueueGraphUpdates()
  {
    if (this.isRegisteredForUpdate)
      return;
    this.isRegisteredForUpdate = true;
    this.AddWorkItem(new AstarPath.AstarWorkItem()
    {
      init = new OnVoidDelegate(this.QueueGraphUpdatesInternal),
      update = new Func<bool, bool>(this.ProcessGraphUpdates)
    });
  }

  [DebuggerHidden]
  private IEnumerator DelayedGraphUpdate()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new AstarPath.\u003CDelayedGraphUpdate\u003Ec__Iterator6()
    {
      \u003C\u003Ef__this = this
    };
  }

  public void UpdateGraphs(Bounds bounds, float t)
  {
    this.UpdateGraphs(new GraphUpdateObject(bounds), t);
  }

  public void UpdateGraphs(GraphUpdateObject ob, float t)
  {
    this.StartCoroutine(this.UpdateGraphsInteral(ob, t));
  }

  [DebuggerHidden]
  private IEnumerator UpdateGraphsInteral(GraphUpdateObject ob, float t)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new AstarPath.\u003CUpdateGraphsInteral\u003Ec__Iterator7()
    {
      t = t,
      ob = ob,
      \u003C\u0024\u003Et = t,
      \u003C\u0024\u003Eob = ob,
      \u003C\u003Ef__this = this
    };
  }

  public void UpdateGraphs(Bounds bounds)
  {
    this.UpdateGraphs(new GraphUpdateObject(bounds));
  }

  public void UpdateGraphs(GraphUpdateObject ob)
  {
    if (this.graphUpdateQueue == null)
      this.graphUpdateQueue = new Queue<GraphUpdateObject>();
    this.graphUpdateQueue.Enqueue(ob);
    if (this.limitGraphUpdates && (double) Time.time - (double) this.lastGraphUpdate < (double) this.maxGraphUpdateFreq)
    {
      if (this.graphUpdateRoutineRunning)
        return;
      this.StartCoroutine(this.DelayedGraphUpdate());
    }
    else
      this.QueueGraphUpdates();
  }

  public void FlushGraphUpdates()
  {
    if (!this.IsAnyGraphUpdatesQueued)
      return;
    this.QueueGraphUpdates();
    this.FlushWorkItems(true, true);
  }

  public void FlushWorkItems(bool unblockOnComplete = true, bool block = false)
  {
    this.BlockUntilPathQueueBlocked();
    this.PerformBlockingActions(block, unblockOnComplete);
  }

  private void QueueGraphUpdatesInternal()
  {
    this.isRegisteredForUpdate = false;
    bool flag = false;
    while (this.graphUpdateQueue.Count > 0)
    {
      GraphUpdateObject graphUpdateObject = this.graphUpdateQueue.Dequeue();
      if (graphUpdateObject.requiresFloodFill)
        flag = true;
      foreach (IUpdatableGraph updatableGraph in this.astarData.GetUpdateableGraphs())
      {
        NavGraph graph = updatableGraph as NavGraph;
        if (graphUpdateObject.nnConstraint == null || graphUpdateObject.nnConstraint.SuitableGraph(AstarPath.active.astarData.GetGraphIndex(graph), graph))
          this.graphUpdateQueueRegular.Enqueue(new AstarPath.GUOSingle()
          {
            order = AstarPath.GraphUpdateOrder.GraphUpdate,
            obj = graphUpdateObject,
            graph = updatableGraph
          });
      }
    }
    if (flag)
      this.graphUpdateQueueRegular.Enqueue(new AstarPath.GUOSingle()
      {
        order = AstarPath.GraphUpdateOrder.FloodFill
      });
    this.debugPath = (Path) null;
    GraphModifier.TriggerEvent(GraphModifier.EventType.PreUpdate);
  }

  private bool ProcessGraphUpdates(bool force)
  {
    if (force)
      this.processingGraphUpdatesAsync.WaitOne();
    else if (!this.processingGraphUpdatesAsync.WaitOne(0))
      return false;
    if (this.graphUpdateQueueAsync.Count != 0)
      throw new Exception("Queue should be empty at this stage");
    while (this.graphUpdateQueueRegular.Count > 0)
    {
      AstarPath.GUOSingle guoSingle = this.graphUpdateQueueRegular.Peek();
      GraphUpdateThreading graphUpdateThreading = guoSingle.order != AstarPath.GraphUpdateOrder.FloodFill ? guoSingle.graph.CanUpdateAsync(guoSingle.obj) : GraphUpdateThreading.SeparateThread;
      bool flag = force;
      if (!Application.isPlaying || this.graphUpdateThread == null || !this.graphUpdateThread.IsAlive)
        flag = true;
      if (!flag && graphUpdateThreading == GraphUpdateThreading.SeparateAndUnityInit)
      {
        if (this.graphUpdateQueueAsync.Count > 0)
        {
          this.processingGraphUpdatesAsync.Reset();
          this.graphUpdateAsyncEvent.Set();
          return false;
        }
        guoSingle.graph.UpdateAreaInit(guoSingle.obj);
        this.graphUpdateQueueRegular.Dequeue();
        this.graphUpdateQueueAsync.Enqueue(guoSingle);
        this.processingGraphUpdatesAsync.Reset();
        this.graphUpdateAsyncEvent.Set();
        return false;
      }
      if (!flag && graphUpdateThreading == GraphUpdateThreading.SeparateThread)
      {
        this.graphUpdateQueueRegular.Dequeue();
        this.graphUpdateQueueAsync.Enqueue(guoSingle);
      }
      else
      {
        if (this.graphUpdateQueueAsync.Count > 0)
        {
          if (force)
            throw new Exception("This should not happen");
          this.processingGraphUpdatesAsync.Reset();
          this.graphUpdateAsyncEvent.Set();
          return false;
        }
        this.graphUpdateQueueRegular.Dequeue();
        if (guoSingle.order == AstarPath.GraphUpdateOrder.FloodFill)
        {
          this.FloodFill();
        }
        else
        {
          if (graphUpdateThreading == GraphUpdateThreading.SeparateAndUnityInit)
          {
            try
            {
              guoSingle.graph.UpdateAreaInit(guoSingle.obj);
            }
            catch (Exception ex)
            {
              UnityEngine.Debug.LogError((object) ("Error while initializing GraphUpdates\n" + (object) ex));
            }
          }
          try
          {
            guoSingle.graph.UpdateArea(guoSingle.obj);
          }
          catch (Exception ex)
          {
            UnityEngine.Debug.LogError((object) ("Error while updating graphs\n" + (object) ex));
          }
        }
      }
    }
    if (this.graphUpdateQueueAsync.Count > 0)
    {
      this.processingGraphUpdatesAsync.Reset();
      this.graphUpdateAsyncEvent.Set();
      return false;
    }
    GraphModifier.TriggerEvent(GraphModifier.EventType.PostUpdate);
    if (AstarPath.OnGraphsUpdated != null)
      AstarPath.OnGraphsUpdated(this);
    return true;
  }

  private void ProcessGraphUpdatesAsync(object _astar)
  {
    AstarPath astarPath = _astar as AstarPath;
    if (object.ReferenceEquals((object) astarPath, (object) null))
    {
      UnityEngine.Debug.LogError((object) "ProcessGraphUpdatesAsync started with invalid parameter _astar (was no AstarPath object)");
    }
    else
    {
      while (!astarPath.pathQueue.IsTerminating)
      {
        this.graphUpdateAsyncEvent.WaitOne();
        if (astarPath.pathQueue.IsTerminating)
        {
          this.graphUpdateQueueAsync.Clear();
          this.processingGraphUpdatesAsync.Set();
          break;
        }
        while (this.graphUpdateQueueAsync.Count > 0)
        {
          AstarPath.GUOSingle guoSingle = this.graphUpdateQueueAsync.Dequeue();
          try
          {
            if (guoSingle.order == AstarPath.GraphUpdateOrder.GraphUpdate)
            {
              guoSingle.graph.UpdateArea(guoSingle.obj);
            }
            else
            {
              if (guoSingle.order != AstarPath.GraphUpdateOrder.FloodFill)
                throw new NotSupportedException(string.Empty + (object) guoSingle.order);
              astarPath.FloodFill();
            }
          }
          catch (Exception ex)
          {
            UnityEngine.Debug.LogError((object) ("Exception while updating graphs:\n" + (object) ex));
          }
        }
        this.processingGraphUpdatesAsync.Set();
      }
    }
  }

  public void FlushThreadSafeCallbacks()
  {
    if (AstarPath.OnThreadSafeCallback == null)
      return;
    this.BlockUntilPathQueueBlocked();
    this.PerformBlockingActions(false, true);
  }

  [ContextMenu("Log Profiler")]
  public void LogProfiler()
  {
  }

  [ContextMenu("Reset Profiler")]
  public void ResetProfiler()
  {
  }

  public static int CalculateThreadCount(ThreadCount count)
  {
    if (count != ThreadCount.AutomaticLowLoad && count != ThreadCount.AutomaticHighLoad)
      return (int) count;
    int val1_1 = Mathf.Max(1, SystemInfo.processorCount);
    int num = SystemInfo.systemMemorySize;
    if (num <= 0)
    {
      UnityEngine.Debug.LogError((object) "Machine reporting that is has <= 0 bytes of RAM. This is definitely not true, assuming 1 GiB");
      num = 1024;
    }
    if (val1_1 <= 1 || num <= 512)
      return 0;
    if (count == ThreadCount.AutomaticHighLoad)
    {
      if (num <= 1024)
        val1_1 = Math.Min(val1_1, 2);
    }
    else
    {
      int val1_2 = Mathf.Max(1, val1_1 / 2);
      if (num <= 1024)
        val1_2 = Math.Min(val1_2, 2);
      val1_1 = Math.Min(val1_2, 6);
    }
    return val1_1;
  }

  public void Awake()
  {
    AstarPath.active = this;
    if (UnityEngine.Object.FindObjectsOfType(typeof (AstarPath)).Length > 1)
      UnityEngine.Debug.LogError((object) "You should NOT have more than one AstarPath component in the scene at any time.\nThis can cause serious errors since the AstarPath component builds around a singleton pattern.");
    this.useGUILayout = false;
    AstarPath.isEditor = Application.isEditor;
    if (AstarPath.OnAwakeSettings != null)
      AstarPath.OnAwakeSettings();
    GraphModifier.FindAllModifiers();
    RelevantGraphSurface.FindAllGraphSurfaces();
    int val1 = AstarPath.CalculateThreadCount(this.threadCount);
    AstarPath.threads = new Thread[val1];
    AstarPath.threadInfos = new PathThreadInfo[Math.Max(val1, 1)];
    this.pathQueue = new ThreadControlQueue(AstarPath.threadInfos.Length);
    for (int index = 0; index < AstarPath.threadInfos.Length; ++index)
      AstarPath.threadInfos[index] = new PathThreadInfo(index, this, new PathHandler(index, AstarPath.threadInfos.Length));
    for (int index = 0; index < AstarPath.threads.Length; ++index)
    {
      AstarPath.threads[index] = new Thread(new ParameterizedThreadStart(AstarPath.CalculatePathsThreaded));
      AstarPath.threads[index].Name = "Pathfinding Thread " + (object) index;
      AstarPath.threads[index].IsBackground = true;
    }
    AstarPath.threadEnumerator = val1 != 0 ? (IEnumerator) null : AstarPath.CalculatePaths((object) AstarPath.threadInfos[0]);
    for (int index = 0; index < AstarPath.threads.Length; ++index)
    {
      if (this.logPathResults == PathLog.Heavy)
        UnityEngine.Debug.Log((object) ("Starting pathfinding thread " + (object) index));
      AstarPath.threads[index].Start((object) AstarPath.threadInfos[index]);
    }
    if (val1 != 0)
    {
      this.graphUpdateThread = new Thread(new ParameterizedThreadStart(this.ProcessGraphUpdatesAsync));
      this.graphUpdateThread.IsBackground = true;
      this.graphUpdateThread.Start((object) this);
    }
    this.Initialize();
    this.FlushWorkItems(true, false);
    this.euclideanEmbedding.dirty = true;
    if (!this.scanOnStartup || this.astarData.cacheStartup && !((UnityEngine.Object) this.astarData.file_cachedStartup == (UnityEngine.Object) null))
      return;
    this.Scan();
  }

  public void VerifyIntegrity()
  {
    if ((UnityEngine.Object) AstarPath.active != (UnityEngine.Object) this)
      throw new Exception("Singleton pattern broken. Make sure you only have one AstarPath object in the scene");
    if (this.astarData == null)
      throw new NullReferenceException("AstarData is null... Astar not set up correctly?");
    if (this.astarData.graphs == null)
      this.astarData.graphs = new NavGraph[0];
    if (this.pathQueue == null && !Application.isPlaying)
      this.pathQueue = new ThreadControlQueue(0);
    if (AstarPath.threadInfos == null && !Application.isPlaying)
      AstarPath.threadInfos = new PathThreadInfo[0];
    if (AstarPath.IsUsingMultithreading)
      ;
  }

  public void SetUpReferences()
  {
    AstarPath.active = this;
    if (this.astarData == null)
      this.astarData = new AstarData();
    if (this.astarData.userConnections == null)
      this.astarData.userConnections = new UserConnection[0];
    if (this.colorSettings == null)
      this.colorSettings = new AstarColor();
    this.colorSettings.OnEnable();
  }

  private void Initialize()
  {
    this.SetUpReferences();
    this.astarData.FindGraphTypes();
    this.astarData.Awake();
    this.astarData.UpdateShortcuts();
    for (int index = 0; index < this.astarData.graphs.Length; ++index)
    {
      if (this.astarData.graphs[index] != null)
        this.astarData.graphs[index].Awake();
    }
  }

  public void OnDestroy()
  {
    if (this.logPathResults == PathLog.Heavy)
      UnityEngine.Debug.Log((object) "+++ AstarPath Component Destroyed - Cleaning Up Pathfinding Data +++");
    if ((UnityEngine.Object) AstarPath.active != (UnityEngine.Object) this)
      return;
    this.pathQueue.TerminateReceivers();
    this.BlockUntilPathQueueBlocked();
    this.euclideanEmbedding.dirty = false;
    this.FlushWorkItems(true, false);
    if (this.logPathResults == PathLog.Heavy)
      UnityEngine.Debug.Log((object) "Processing Eventual Work Items");
    this.graphUpdateAsyncEvent.Set();
    if (AstarPath.threads != null)
    {
      for (int index = 0; index < AstarPath.threads.Length; ++index)
      {
        if (!AstarPath.threads[index].Join(50))
        {
          UnityEngine.Debug.LogError((object) ("Could not terminate pathfinding thread[" + (object) index + "] in 50ms, trying Thread.Abort"));
          AstarPath.threads[index].Abort();
        }
      }
    }
    if (this.logPathResults == PathLog.Heavy)
      UnityEngine.Debug.Log((object) "Returning Paths");
    this.ReturnPaths(false);
    AstarPath.pathReturnStack.PopAll();
    if (this.logPathResults == PathLog.Heavy)
      UnityEngine.Debug.Log((object) "Destroying Graphs");
    this.astarData.OnDestroy();
    if (this.logPathResults == PathLog.Heavy)
      UnityEngine.Debug.Log((object) "Cleaning up variables");
    this.floodStack = (Stack<GraphNode>) null;
    this.graphUpdateQueue = (Queue<GraphUpdateObject>) null;
    this.OnDrawGizmosCallback = (OnVoidDelegate) null;
    AstarPath.OnAwakeSettings = (OnVoidDelegate) null;
    AstarPath.OnGraphPreScan = (OnGraphDelegate) null;
    AstarPath.OnGraphPostScan = (OnGraphDelegate) null;
    AstarPath.OnPathPreSearch = (OnPathDelegate) null;
    AstarPath.OnPathPostSearch = (OnPathDelegate) null;
    AstarPath.OnPreScan = (OnScanDelegate) null;
    AstarPath.OnPostScan = (OnScanDelegate) null;
    AstarPath.OnLatePostScan = (OnScanDelegate) null;
    AstarPath.On65KOverflow = (OnVoidDelegate) null;
    AstarPath.OnGraphsUpdated = (OnScanDelegate) null;
    AstarPath.OnThreadSafeCallback = (OnVoidDelegate) null;
    AstarPath.threads = (Thread[]) null;
    AstarPath.threadInfos = (PathThreadInfo[]) null;
    AstarPath.PathsCompleted = 0;
    AstarPath.active = (AstarPath) null;
  }

  public void FloodFill(GraphNode seed)
  {
    this.FloodFill(seed, this.lastUniqueAreaIndex + 1U);
    ++this.lastUniqueAreaIndex;
  }

  public void FloodFill(GraphNode seed, uint area)
  {
    if (area > 131071U)
      UnityEngine.Debug.LogError((object) ("Too high area index - The maximum area index is " + (object) 131071));
    else if (area < 0U)
    {
      UnityEngine.Debug.LogError((object) "Too low area index - The minimum area index is 0");
    }
    else
    {
      if (this.floodStack == null)
        this.floodStack = new Stack<GraphNode>(1024);
      Stack<GraphNode> stack = this.floodStack;
      stack.Clear();
      stack.Push(seed);
      seed.Area = area;
      while (stack.Count > 0)
        stack.Pop().FloodFill(stack, area);
    }
  }

  [ContextMenu("Flood Fill Graphs")]
  public void FloodFill()
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    AstarPath.\u003CFloodFill\u003Ec__AnonStorey14 fillCAnonStorey14 = new AstarPath.\u003CFloodFill\u003Ec__AnonStorey14();
    // ISSUE: reference to a compiler-generated field
    fillCAnonStorey14.\u003C\u003Ef__this = this;
    this.queuedWorkItemFloodFill = false;
    if (this.astarData.graphs == null)
      return;
    // ISSUE: reference to a compiler-generated field
    fillCAnonStorey14.area = 0U;
    this.lastUniqueAreaIndex = 0U;
    if (this.floodStack == null)
      this.floodStack = new Stack<GraphNode>(1024);
    // ISSUE: reference to a compiler-generated field
    fillCAnonStorey14.stack = this.floodStack;
    for (int index = 0; index < this.graphs.Length; ++index)
    {
      NavGraph navGraph = this.graphs[index];
      if (navGraph != null)
        navGraph.GetNodes((GraphNodeDelegateCancelable) (node =>
        {
          node.Area = 0U;
          return true;
        }));
    }
    // ISSUE: reference to a compiler-generated field
    fillCAnonStorey14.smallAreasDetected = 0;
    // ISSUE: reference to a compiler-generated field
    fillCAnonStorey14.warnAboutAreas = false;
    // ISSUE: reference to a compiler-generated field
    fillCAnonStorey14.smallAreaList = ListPool<GraphNode>.Claim();
    for (int index = 0; index < this.graphs.Length; ++index)
    {
      NavGraph navGraph = this.graphs[index];
      if (navGraph != null)
      {
        // ISSUE: reference to a compiler-generated method
        GraphNodeDelegateCancelable del = new GraphNodeDelegateCancelable(fillCAnonStorey14.\u003C\u003Em__4);
        navGraph.GetNodes(del);
      }
    }
    // ISSUE: reference to a compiler-generated field
    this.lastUniqueAreaIndex = fillCAnonStorey14.area;
    // ISSUE: reference to a compiler-generated field
    if (fillCAnonStorey14.warnAboutAreas)
      UnityEngine.Debug.LogError((object) ("Too many areas - The maximum number of areas is " + (object) 131071 + ". Try raising the A* Inspector -> Settings -> Min Area Size value. Enable the optimization ASTAR_MORE_AREAS under the Optimizations tab."));
    // ISSUE: reference to a compiler-generated field
    if (fillCAnonStorey14.smallAreasDetected > 0)
    {
      // ISSUE: reference to a compiler-generated field
      AstarPath.AstarLog((string) (object) fillCAnonStorey14.smallAreasDetected + (object) " small areas were detected (fewer than " + (string) (object) this.minAreaSize + " nodes),these might have the same IDs as other areas, but it shouldn't affect pathfinding in any significant way (you might get All Nodes Searched as a reason for path failure).\nWhich areas are defined as 'small' is controlled by the 'Min Area Size' variable, it can be changed in the A* inspector-->Settings-->Min Area Size\nThe small areas will use the area id " + (string) (object) 131071);
    }
    // ISSUE: reference to a compiler-generated field
    ListPool<GraphNode>.Release(fillCAnonStorey14.smallAreaList);
  }

  public int GetNewNodeIndex()
  {
    if (this.nodeIndexPool.Count > 0)
      return this.nodeIndexPool.Pop();
    return this.nextNodeIndex++;
  }

  public void InitializeNode(GraphNode node)
  {
    if (!this.pathQueue.AllReceiversBlocked)
      throw new Exception("Trying to initialize a node when it is not safe to initialize any nodes. Must be done during a graph update");
    if (AstarPath.threadInfos == null)
      AstarPath.threadInfos = new PathThreadInfo[0];
    for (int index = 0; index < AstarPath.threadInfos.Length; ++index)
      AstarPath.threadInfos[index].runData.InitializeNode(node);
  }

  public void DestroyNode(GraphNode node)
  {
    if (node.NodeIndex == -1)
      return;
    this.nodeIndexPool.Push(node.NodeIndex);
    if (AstarPath.threadInfos == null)
      AstarPath.threadInfos = new PathThreadInfo[0];
    for (int index = 0; index < AstarPath.threadInfos.Length; ++index)
      AstarPath.threadInfos[index].runData.DestroyNode(node);
  }

  public void BlockUntilPathQueueBlocked()
  {
    if (this.pathQueue == null)
      return;
    this.pathQueue.Block();
    while (!this.pathQueue.AllReceiversBlocked)
    {
      if (AstarPath.IsUsingMultithreading)
        Thread.Sleep(1);
      else
        AstarPath.threadEnumerator.MoveNext();
    }
  }

  public void Scan()
  {
    this.ScanLoop((OnScanStatus) null);
  }

  public void ScanSpecific(NavGraph graph)
  {
    this.ScanSpecific(graph, (OnScanStatus) null);
  }

  public void ScanSpecific(NavGraph graph, OnScanStatus statusCallback)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    AstarPath.\u003CScanSpecific\u003Ec__AnonStorey16 specificCAnonStorey16 = new AstarPath.\u003CScanSpecific\u003Ec__AnonStorey16();
    // ISSUE: reference to a compiler-generated field
    specificCAnonStorey16.statusCallback = statusCallback;
    if (this.graphs == null)
      return;
    this.isScanning = true;
    this.euclideanEmbedding.dirty = false;
    this.VerifyIntegrity();
    this.BlockUntilPathQueueBlocked();
    if (!Application.isPlaying)
    {
      GraphModifier.FindAllModifiers();
      RelevantGraphSurface.FindAllGraphSurfaces();
    }
    RelevantGraphSurface.UpdateAllPositions();
    this.astarData.UpdateShortcuts();
    // ISSUE: reference to a compiler-generated field
    if (specificCAnonStorey16.statusCallback != null)
    {
      // ISSUE: reference to a compiler-generated field
      specificCAnonStorey16.statusCallback(new Progress(0.05f, "Pre processing graphs"));
    }
    if (AstarPath.OnPreScan != null)
      AstarPath.OnPreScan(this);
    GraphModifier.TriggerEvent(GraphModifier.EventType.PreScan);
    DateTime utcNow = DateTime.UtcNow;
    for (int index = 0; index < this.graphs.Length; ++index)
    {
      if (this.graphs[index] != null)
        this.graphs[index].GetNodes((GraphNodeDelegateCancelable) (node =>
        {
          node.Destroy();
          return true;
        }));
    }
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    AstarPath.\u003CScanSpecific\u003Ec__AnonStorey17 specificCAnonStorey17 = new AstarPath.\u003CScanSpecific\u003Ec__AnonStorey17();
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    for (specificCAnonStorey17.i = 0; specificCAnonStorey17.i < this.graphs.Length; specificCAnonStorey17.i = specificCAnonStorey17.i + 1)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.graphs[specificCAnonStorey17.i] == graph)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        AstarPath.\u003CScanSpecific\u003Ec__AnonStorey15 specificCAnonStorey15 = new AstarPath.\u003CScanSpecific\u003Ec__AnonStorey15();
        // ISSUE: reference to a compiler-generated field
        specificCAnonStorey15.\u003C\u003Ef__ref\u002422 = specificCAnonStorey16;
        // ISSUE: reference to a compiler-generated field
        specificCAnonStorey15.\u003C\u003Ef__ref\u002423 = specificCAnonStorey17;
        if (graph == null)
        {
          // ISSUE: reference to a compiler-generated field
          if (specificCAnonStorey16.statusCallback != null)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            specificCAnonStorey16.statusCallback(new Progress(AstarMath.MapTo(0.05f, 0.7f, ((float) specificCAnonStorey17.i + 0.5f) / (float) (this.graphs.Length + 1)), "Skipping graph " + (object) (specificCAnonStorey17.i + 1) + " of " + (string) (object) this.graphs.Length + " because it is null"));
          }
        }
        else
        {
          if (AstarPath.OnGraphPreScan != null)
          {
            // ISSUE: reference to a compiler-generated field
            if (specificCAnonStorey16.statusCallback != null)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              specificCAnonStorey16.statusCallback(new Progress(AstarMath.MapToRange(0.1f, 0.7f, (float) specificCAnonStorey17.i / (float) this.graphs.Length), "Scanning graph " + (object) (specificCAnonStorey17.i + 1) + " of " + (string) (object) this.graphs.Length + " - Pre processing"));
            }
            AstarPath.OnGraphPreScan(graph);
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          specificCAnonStorey15.minp = AstarMath.MapToRange(0.1f, 0.7f, (float) specificCAnonStorey17.i / (float) this.graphs.Length);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          specificCAnonStorey15.maxp = AstarMath.MapToRange(0.1f, 0.7f, ((float) specificCAnonStorey17.i + 0.95f) / (float) this.graphs.Length);
          // ISSUE: reference to a compiler-generated field
          if (specificCAnonStorey16.statusCallback != null)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            specificCAnonStorey16.statusCallback(new Progress(specificCAnonStorey15.minp, string.Concat(new object[4]
            {
              (object) "Scanning graph ",
              (object) (specificCAnonStorey17.i + 1),
              (object) " of ",
              (object) this.graphs.Length
            })));
          }
          OnScanStatus statusCallback1 = (OnScanStatus) null;
          // ISSUE: reference to a compiler-generated field
          if (specificCAnonStorey16.statusCallback != null)
          {
            // ISSUE: reference to a compiler-generated method
            statusCallback1 = new OnScanStatus(specificCAnonStorey15.\u003C\u003Em__6);
          }
          graph.ScanInternal(statusCallback1);
          // ISSUE: reference to a compiler-generated method
          graph.GetNodes(new GraphNodeDelegateCancelable(specificCAnonStorey15.\u003C\u003Em__7));
          if (AstarPath.OnGraphPostScan != null)
          {
            // ISSUE: reference to a compiler-generated field
            if (specificCAnonStorey16.statusCallback != null)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              specificCAnonStorey16.statusCallback(new Progress(AstarMath.MapToRange(0.1f, 0.7f, ((float) specificCAnonStorey17.i + 0.95f) / (float) this.graphs.Length), "Scanning graph " + (object) (specificCAnonStorey17.i + 1) + " of " + (string) (object) this.graphs.Length + " - Post processing"));
            }
            AstarPath.OnGraphPostScan(graph);
            break;
          }
          break;
        }
      }
    }
    // ISSUE: reference to a compiler-generated field
    if (specificCAnonStorey16.statusCallback != null)
    {
      // ISSUE: reference to a compiler-generated field
      specificCAnonStorey16.statusCallback(new Progress(0.8f, "Post processing graphs"));
    }
    if (AstarPath.OnPostScan != null)
      AstarPath.OnPostScan(this);
    GraphModifier.TriggerEvent(GraphModifier.EventType.PostScan);
    this.ApplyLinks();
    try
    {
      this.FlushWorkItems(false, true);
    }
    catch (Exception ex)
    {
      UnityEngine.Debug.LogException(ex);
    }
    this.isScanning = false;
    // ISSUE: reference to a compiler-generated field
    if (specificCAnonStorey16.statusCallback != null)
    {
      // ISSUE: reference to a compiler-generated field
      specificCAnonStorey16.statusCallback(new Progress(0.9f, "Computing areas"));
    }
    this.FloodFill();
    this.VerifyIntegrity();
    // ISSUE: reference to a compiler-generated field
    if (specificCAnonStorey16.statusCallback != null)
    {
      // ISSUE: reference to a compiler-generated field
      specificCAnonStorey16.statusCallback(new Progress(0.95f, "Late post processing"));
    }
    if (AstarPath.OnLatePostScan != null)
      AstarPath.OnLatePostScan(this);
    GraphModifier.TriggerEvent(GraphModifier.EventType.LatePostScan);
    this.euclideanEmbedding.dirty = true;
    this.euclideanEmbedding.RecalculatePivots();
    this.PerformBlockingActions(true, true);
    this.lastScanTime = (float) (DateTime.UtcNow - utcNow).TotalSeconds;
    GC.Collect();
    AstarPath.AstarLog("Scanning - Process took " + (this.lastScanTime * 1000f).ToString("0") + " ms to complete");
  }

  public void ScanLoop(OnScanStatus statusCallback)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    AstarPath.\u003CScanLoop\u003Ec__AnonStorey19 loopCAnonStorey19 = new AstarPath.\u003CScanLoop\u003Ec__AnonStorey19();
    // ISSUE: reference to a compiler-generated field
    loopCAnonStorey19.statusCallback = statusCallback;
    if (this.graphs == null)
      return;
    this.isScanning = true;
    this.euclideanEmbedding.dirty = false;
    this.VerifyIntegrity();
    this.BlockUntilPathQueueBlocked();
    if (!Application.isPlaying)
    {
      GraphModifier.FindAllModifiers();
      RelevantGraphSurface.FindAllGraphSurfaces();
    }
    RelevantGraphSurface.UpdateAllPositions();
    this.astarData.UpdateShortcuts();
    // ISSUE: reference to a compiler-generated field
    if (loopCAnonStorey19.statusCallback != null)
    {
      // ISSUE: reference to a compiler-generated field
      loopCAnonStorey19.statusCallback(new Progress(0.05f, "Pre processing graphs"));
    }
    if (AstarPath.OnPreScan != null)
      AstarPath.OnPreScan(this);
    GraphModifier.TriggerEvent(GraphModifier.EventType.PreScan);
    DateTime utcNow = DateTime.UtcNow;
    for (int index = 0; index < this.graphs.Length; ++index)
    {
      if (this.graphs[index] != null)
        this.graphs[index].GetNodes((GraphNodeDelegateCancelable) (node =>
        {
          node.Destroy();
          return true;
        }));
    }
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    AstarPath.\u003CScanLoop\u003Ec__AnonStorey1A loopCAnonStorey1A = new AstarPath.\u003CScanLoop\u003Ec__AnonStorey1A();
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    for (loopCAnonStorey1A.i = 0; loopCAnonStorey1A.i < this.graphs.Length; loopCAnonStorey1A.i = loopCAnonStorey1A.i + 1)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AstarPath.\u003CScanLoop\u003Ec__AnonStorey18 loopCAnonStorey18 = new AstarPath.\u003CScanLoop\u003Ec__AnonStorey18();
      // ISSUE: reference to a compiler-generated field
      loopCAnonStorey18.\u003C\u003Ef__ref\u002425 = loopCAnonStorey19;
      // ISSUE: reference to a compiler-generated field
      loopCAnonStorey18.\u003C\u003Ef__ref\u002426 = loopCAnonStorey1A;
      // ISSUE: reference to a compiler-generated field
      NavGraph graph = this.graphs[loopCAnonStorey1A.i];
      if (graph == null)
      {
        // ISSUE: reference to a compiler-generated field
        if (loopCAnonStorey19.statusCallback != null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          loopCAnonStorey19.statusCallback(new Progress(AstarMath.MapTo(0.05f, 0.7f, ((float) loopCAnonStorey1A.i + 0.5f) / (float) (this.graphs.Length + 1)), "Skipping graph " + (object) (loopCAnonStorey1A.i + 1) + " of " + (string) (object) this.graphs.Length + " because it is null"));
        }
      }
      else
      {
        if (AstarPath.OnGraphPreScan != null)
        {
          // ISSUE: reference to a compiler-generated field
          if (loopCAnonStorey19.statusCallback != null)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            loopCAnonStorey19.statusCallback(new Progress(AstarMath.MapToRange(0.1f, 0.7f, (float) loopCAnonStorey1A.i / (float) this.graphs.Length), "Scanning graph " + (object) (loopCAnonStorey1A.i + 1) + " of " + (string) (object) this.graphs.Length + " - Pre processing"));
          }
          AstarPath.OnGraphPreScan(graph);
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        loopCAnonStorey18.minp = AstarMath.MapToRange(0.1f, 0.7f, (float) loopCAnonStorey1A.i / (float) this.graphs.Length);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        loopCAnonStorey18.maxp = AstarMath.MapToRange(0.1f, 0.7f, ((float) loopCAnonStorey1A.i + 0.95f) / (float) this.graphs.Length);
        // ISSUE: reference to a compiler-generated field
        if (loopCAnonStorey19.statusCallback != null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          loopCAnonStorey19.statusCallback(new Progress(loopCAnonStorey18.minp, string.Concat(new object[4]
          {
            (object) "Scanning graph ",
            (object) (loopCAnonStorey1A.i + 1),
            (object) " of ",
            (object) this.graphs.Length
          })));
        }
        OnScanStatus statusCallback1 = (OnScanStatus) null;
        // ISSUE: reference to a compiler-generated field
        if (loopCAnonStorey19.statusCallback != null)
        {
          // ISSUE: reference to a compiler-generated method
          statusCallback1 = new OnScanStatus(loopCAnonStorey18.\u003C\u003Em__9);
        }
        graph.ScanInternal(statusCallback1);
        // ISSUE: reference to a compiler-generated method
        graph.GetNodes(new GraphNodeDelegateCancelable(loopCAnonStorey18.\u003C\u003Em__A));
        if (AstarPath.OnGraphPostScan != null)
        {
          // ISSUE: reference to a compiler-generated field
          if (loopCAnonStorey19.statusCallback != null)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            loopCAnonStorey19.statusCallback(new Progress(AstarMath.MapToRange(0.1f, 0.7f, ((float) loopCAnonStorey1A.i + 0.95f) / (float) this.graphs.Length), "Scanning graph " + (object) (loopCAnonStorey1A.i + 1) + " of " + (string) (object) this.graphs.Length + " - Post processing"));
          }
          AstarPath.OnGraphPostScan(graph);
        }
      }
    }
    // ISSUE: reference to a compiler-generated field
    if (loopCAnonStorey19.statusCallback != null)
    {
      // ISSUE: reference to a compiler-generated field
      loopCAnonStorey19.statusCallback(new Progress(0.8f, "Post processing graphs"));
    }
    if (AstarPath.OnPostScan != null)
      AstarPath.OnPostScan(this);
    GraphModifier.TriggerEvent(GraphModifier.EventType.PostScan);
    this.ApplyLinks();
    try
    {
      this.FlushWorkItems(false, true);
    }
    catch (Exception ex)
    {
      UnityEngine.Debug.LogException(ex);
    }
    this.isScanning = false;
    // ISSUE: reference to a compiler-generated field
    if (loopCAnonStorey19.statusCallback != null)
    {
      // ISSUE: reference to a compiler-generated field
      loopCAnonStorey19.statusCallback(new Progress(0.9f, "Computing areas"));
    }
    this.FloodFill();
    this.VerifyIntegrity();
    // ISSUE: reference to a compiler-generated field
    if (loopCAnonStorey19.statusCallback != null)
    {
      // ISSUE: reference to a compiler-generated field
      loopCAnonStorey19.statusCallback(new Progress(0.95f, "Late post processing"));
    }
    if (AstarPath.OnLatePostScan != null)
      AstarPath.OnLatePostScan(this);
    GraphModifier.TriggerEvent(GraphModifier.EventType.LatePostScan);
    this.euclideanEmbedding.dirty = true;
    this.euclideanEmbedding.RecalculatePivots();
    this.PerformBlockingActions(true, true);
    this.lastScanTime = (float) (DateTime.UtcNow - utcNow).TotalSeconds;
    GC.Collect();
    AstarPath.AstarLog("Scanning - Process took " + (this.lastScanTime * 1000f).ToString("0") + " ms to complete");
  }

  public void ApplyLinks()
  {
    if (this.astarData.userConnections != null && this.astarData.userConnections.Length > 0)
    {
      UnityEngine.Debug.LogWarning((object) "<b>Deleting all links now</b>, but saving graph data in backup variable.\nCreating replacement links using the new system, stored under the <i>Links</i> GameObject.");
      GameObject gameObject1 = new GameObject("Links");
      Dictionary<Int3, GameObject> dictionary = new Dictionary<Int3, GameObject>();
      for (int index = 0; index < this.astarData.userConnections.Length; ++index)
      {
        UserConnection userConnection = this.astarData.userConnections[index];
        GameObject gameObject2 = !dictionary.ContainsKey((Int3) userConnection.p1) ? new GameObject("Link " + (object) index) : dictionary[(Int3) userConnection.p1];
        GameObject gameObject3 = !dictionary.ContainsKey((Int3) userConnection.p2) ? new GameObject("Link " + (object) index) : dictionary[(Int3) userConnection.p2];
        gameObject2.transform.parent = gameObject1.transform;
        gameObject3.transform.parent = gameObject1.transform;
        dictionary[(Int3) userConnection.p1] = gameObject2;
        dictionary[(Int3) userConnection.p2] = gameObject3;
        gameObject2.transform.position = userConnection.p1;
        gameObject3.transform.position = userConnection.p2;
        NodeLink nodeLink = gameObject2.AddComponent<NodeLink>();
        nodeLink.end = gameObject3.transform;
        nodeLink.deleteConnection = !userConnection.enable;
      }
      this.astarData.userConnections = (UserConnection[]) null;
      this.astarData.data_backup = this.astarData.GetData();
      throw new NotSupportedException("<b>Links have been deprecated</b>. Please use the component <b>NodeLink</b> instead. Create two GameObjects around the points you want to link, then press <b>Cmd+Alt+L</b> ( <b>Ctrl+Alt+L</b> on windows) to link them. See <b>Menubar -> Edit -> Pathfinding</b>.");
    }
  }

  public static void WaitForPath(Path p)
  {
    if ((UnityEngine.Object) AstarPath.active == (UnityEngine.Object) null)
      throw new Exception("Pathfinding is not correctly initialized in this scene (yet?). AstarPath.active is null.\nDo not call this function in Awake");
    if (p == null)
      throw new ArgumentNullException("Path must not be null");
    if (AstarPath.active.pathQueue.IsTerminating)
      return;
    if (p.GetState() == PathState.Created)
      throw new Exception("The specified path has not been started yet.");
    ++AstarPath.waitForPathDepth;
    if (AstarPath.waitForPathDepth == 5)
      UnityEngine.Debug.LogError((object) "You are calling the WaitForPath function recursively (maybe from a path callback). Please don't do this.");
    if (p.GetState() < PathState.ReturnQueue)
    {
      if (AstarPath.IsUsingMultithreading)
      {
        while (p.GetState() < PathState.ReturnQueue)
        {
          if (AstarPath.active.pathQueue.IsTerminating)
          {
            --AstarPath.waitForPathDepth;
            throw new Exception("Pathfinding Threads seems to have crashed.");
          }
          Thread.Sleep(1);
          AstarPath.active.PerformBlockingActions(false, true);
        }
      }
      else
      {
        while (p.GetState() < PathState.ReturnQueue)
        {
          if (AstarPath.active.pathQueue.IsEmpty && p.GetState() != PathState.Processing)
          {
            --AstarPath.waitForPathDepth;
            throw new Exception("Critical error. Path Queue is empty but the path state is '" + (object) p.GetState() + "'");
          }
          AstarPath.threadEnumerator.MoveNext();
          AstarPath.active.PerformBlockingActions(false, true);
        }
      }
    }
    AstarPath.active.ReturnPaths(false);
    --AstarPath.waitForPathDepth;
  }

  [Obsolete("The threadSafe parameter has been deprecated")]
  public static void RegisterSafeUpdate(OnVoidDelegate callback, bool threadSafe)
  {
    AstarPath.RegisterSafeUpdate(callback);
  }

  public static void RegisterSafeUpdate(OnVoidDelegate callback)
  {
    if (callback == null || !Application.isPlaying)
      return;
    if (AstarPath.active.pathQueue.AllReceiversBlocked)
    {
      AstarPath.active.pathQueue.Lock();
      try
      {
        if (AstarPath.active.pathQueue.AllReceiversBlocked)
        {
          callback();
          return;
        }
      }
      finally
      {
        AstarPath.active.pathQueue.Unlock();
      }
    }
    lock (AstarPath.safeUpdateLock)
      AstarPath.OnThreadSafeCallback += callback;
    AstarPath.active.pathQueue.Block();
  }

  private static void InterruptPathfinding()
  {
    AstarPath.active.pathQueue.Block();
  }

  public static void StartPath(Path p, bool pushToFront = false)
  {
    if (object.ReferenceEquals((object) AstarPath.active, (object) null))
    {
      UnityEngine.Debug.LogError((object) "There is no AstarPath object in the scene");
    }
    else
    {
      if (p.GetState() != PathState.Created)
        throw new Exception("The path has an invalid state. Expected " + (object) PathState.Created + " found " + (string) (object) p.GetState() + "\nMake sure you are not requesting the same path twice");
      if (AstarPath.active.pathQueue.IsTerminating)
      {
        p.Error();
        p.LogError("No new paths are accepted");
      }
      else if (AstarPath.active.graphs == null || AstarPath.active.graphs.Length == 0)
      {
        UnityEngine.Debug.LogError((object) "There are no graphs in the scene");
        p.Error();
        p.LogError("There are no graphs in the scene");
        UnityEngine.Debug.LogError((object) p.errorLog);
      }
      else
      {
        p.Claim((object) AstarPath.active);
        p.AdvanceState(PathState.PathQueue);
        if (pushToFront)
          AstarPath.active.pathQueue.PushFront(p);
        else
          AstarPath.active.pathQueue.Push(p);
      }
    }
  }

  public void OnApplicationQuit()
  {
    if (this.logPathResults == PathLog.Heavy)
      UnityEngine.Debug.Log((object) "+++ Application Quitting - Cleaning Up +++");
    this.OnDestroy();
    if (AstarPath.threads == null)
      return;
    for (int index = 0; index < AstarPath.threads.Length; ++index)
    {
      if (AstarPath.threads[index] != null && AstarPath.threads[index].IsAlive)
        AstarPath.threads[index].Abort();
    }
  }

  public void ReturnPaths(bool timeSlice)
  {
    Path path1 = AstarPath.pathReturnStack.PopAll();
    if (this.pathReturnPop == null)
    {
      this.pathReturnPop = path1;
    }
    else
    {
      Path path2 = this.pathReturnPop;
      while (path2.next != null)
        path2 = path2.next;
      path2.next = path1;
    }
    long num1 = !timeSlice ? 0L : DateTime.UtcNow.Ticks + 10000L;
    int num2 = 0;
    while (this.pathReturnPop != null)
    {
      Path path2 = this.pathReturnPop;
      this.pathReturnPop = this.pathReturnPop.next;
      path2.next = (Path) null;
      path2.ReturnPath();
      path2.AdvanceState(PathState.Returned);
      path2.ReleaseSilent((object) this);
      ++num2;
      if (num2 > 5 && timeSlice)
      {
        num2 = 0;
        if (DateTime.UtcNow.Ticks >= num1)
          break;
      }
    }
  }

  private static void CalculatePathsThreaded(object _threadInfo)
  {
    PathThreadInfo pathThreadInfo;
    try
    {
      pathThreadInfo = (PathThreadInfo) _threadInfo;
    }
    catch (Exception ex)
    {
      UnityEngine.Debug.LogError((object) ("Arguments to pathfinding threads must be of type ThreadStartInfo\n" + (object) ex));
      throw new ArgumentException("Argument must be of type ThreadStartInfo", ex);
    }
    AstarPath astarPath = pathThreadInfo.astar;
    try
    {
      PathHandler pathHandler = pathThreadInfo.runData;
      if (pathHandler.nodes == null)
        throw new NullReferenceException("NodeRuns must be assigned to the threadInfo.runData.nodes field before threads are started\nthreadInfo is an argument to the thread functions");
      long targetTick = DateTime.UtcNow.Ticks + (long) ((double) astarPath.maxFrameTime * 10000.0);
      while (true)
      {
        long num1;
        do
        {
          Path p = astarPath.pathQueue.Pop();
          num1 = (long) ((double) astarPath.maxFrameTime * 10000.0);
          p.PrepareBase(pathHandler);
          p.AdvanceState(PathState.Processing);
          if (AstarPath.OnPathPreSearch != null)
            AstarPath.OnPathPreSearch(p);
          long ticks = DateTime.UtcNow.Ticks;
          long num2 = 0L;
          p.Prepare();
          if (!p.IsDone())
          {
            astarPath.debugPath = p;
            p.Initialize();
            while (!p.IsDone())
            {
              p.CalculateStep(targetTick);
              ++p.searchIterations;
              if (!p.IsDone())
              {
                num2 += DateTime.UtcNow.Ticks - ticks;
                Thread.Sleep(0);
                ticks = DateTime.UtcNow.Ticks;
                targetTick = ticks + num1;
                if (astarPath.pathQueue.IsTerminating)
                  p.Error();
              }
              else
                break;
            }
            long num3 = num2 + (DateTime.UtcNow.Ticks - ticks);
            p.duration = (float) num3 * 0.0001f;
          }
          p.Cleanup();
          astarPath.LogPathResults(p);
          if (p.immediateCallback != null)
            p.immediateCallback(p);
          if (AstarPath.OnPathPostSearch != null)
            AstarPath.OnPathPostSearch(p);
          AstarPath.pathReturnStack.Push(p);
          p.AdvanceState(PathState.ReturnQueue);
        }
        while (DateTime.UtcNow.Ticks <= targetTick);
        Thread.Sleep(1);
        targetTick = DateTime.UtcNow.Ticks + num1;
      }
    }
    catch (Exception ex)
    {
      if (ex is ThreadAbortException || ex is ThreadControlQueue.QueueTerminationException)
      {
        if (astarPath.logPathResults != PathLog.Heavy)
          return;
        UnityEngine.Debug.LogWarning((object) ("Shutting down pathfinding thread #" + (object) pathThreadInfo.threadIndex + " with Thread.Abort call"));
        return;
      }
      UnityEngine.Debug.LogException(ex);
      UnityEngine.Debug.LogError((object) "Unhandled exception during pathfinding. Terminating.");
      astarPath.pathQueue.TerminateReceivers();
    }
    UnityEngine.Debug.LogError((object) "Error : This part should never be reached.");
    astarPath.pathQueue.ReceiverTerminated();
  }

  [DebuggerHidden]
  private static IEnumerator CalculatePaths(object _threadInfo)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new AstarPath.\u003CCalculatePaths\u003Ec__Iterator8()
    {
      _threadInfo = _threadInfo,
      \u003C\u0024\u003E_threadInfo = _threadInfo
    };
  }

  public NNInfo GetNearest(Vector3 position)
  {
    return this.GetNearest(position, NNConstraint.None);
  }

  public NNInfo GetNearest(Vector3 position, NNConstraint constraint)
  {
    return this.GetNearest(position, constraint, (GraphNode) null);
  }

  public NNInfo GetNearest(Vector3 position, NNConstraint constraint, GraphNode hint)
  {
    if (this.graphs == null)
      return new NNInfo();
    float num = float.PositiveInfinity;
    NNInfo nnInfo1 = new NNInfo();
    int index = -1;
    for (int graphIndex = 0; graphIndex < this.graphs.Length; ++graphIndex)
    {
      NavGraph graph = this.graphs[graphIndex];
      if (graph != null && constraint.SuitableGraph(graphIndex, graph))
      {
        NNInfo nnInfo2 = !this.fullGetNearestSearch ? graph.GetNearest(position, constraint) : graph.GetNearestForce(position, constraint);
        if (nnInfo2.node != null)
        {
          float magnitude = (nnInfo2.clampedPosition - position).magnitude;
          if (this.prioritizeGraphs && (double) magnitude < (double) this.prioritizeGraphsLimit)
          {
            nnInfo1 = nnInfo2;
            index = graphIndex;
            break;
          }
          if ((double) magnitude < (double) num)
          {
            num = magnitude;
            nnInfo1 = nnInfo2;
            index = graphIndex;
          }
        }
      }
    }
    if (index == -1)
      return nnInfo1;
    if (nnInfo1.constrainedNode != null)
    {
      nnInfo1.node = nnInfo1.constrainedNode;
      nnInfo1.clampedPosition = nnInfo1.constClampedPosition;
    }
    if (!this.fullGetNearestSearch && nnInfo1.node != null && !constraint.Suitable(nnInfo1.node))
    {
      NNInfo nearestForce = this.graphs[index].GetNearestForce(position, constraint);
      if (nearestForce.node != null)
        nnInfo1 = nearestForce;
    }
    if (!constraint.Suitable(nnInfo1.node) || constraint.constrainDistance && (double) (nnInfo1.clampedPosition - position).sqrMagnitude > (double) this.maxNearestNodeDistanceSqr)
      return new NNInfo();
    return nnInfo1;
  }

  public GraphNode GetNearest(Ray ray)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    AstarPath.\u003CGetNearest\u003Ec__AnonStorey1B nearestCAnonStorey1B = new AstarPath.\u003CGetNearest\u003Ec__AnonStorey1B();
    if (this.graphs == null)
      return (GraphNode) null;
    // ISSUE: reference to a compiler-generated field
    nearestCAnonStorey1B.minDist = float.PositiveInfinity;
    // ISSUE: reference to a compiler-generated field
    nearestCAnonStorey1B.nearestNode = (GraphNode) null;
    // ISSUE: reference to a compiler-generated field
    nearestCAnonStorey1B.lineDirection = ray.direction;
    // ISSUE: reference to a compiler-generated field
    nearestCAnonStorey1B.lineOrigin = ray.origin;
    for (int index = 0; index < this.graphs.Length; ++index)
    {
      // ISSUE: reference to a compiler-generated method
      this.graphs[index].GetNodes(new GraphNodeDelegateCancelable(nearestCAnonStorey1B.\u003C\u003Em__B));
    }
    // ISSUE: reference to a compiler-generated field
    return nearestCAnonStorey1B.nearestNode;
  }

  public enum AstarDistribution
  {
    WebsiteDownload,
    AssetStore,
  }

  public struct AstarWorkItem
  {
    public OnVoidDelegate init;
    public Func<bool, bool> update;

    public AstarWorkItem(Func<bool, bool> update)
    {
      this.init = (OnVoidDelegate) null;
      this.update = update;
    }

    public AstarWorkItem(OnVoidDelegate init, Func<bool, bool> update)
    {
      this.init = init;
      this.update = update;
    }
  }

  private enum GraphUpdateOrder
  {
    GraphUpdate,
    FloodFill,
  }

  private struct GUOSingle
  {
    public AstarPath.GraphUpdateOrder order;
    public IUpdatableGraph graph;
    public GraphUpdateObject obj;
  }
}
