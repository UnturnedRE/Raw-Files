// Decompiled with JetBrains decompiler
// Type: Seeker
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[AddComponentMenu("Pathfinding/Seeker")]
public class Seeker : MonoBehaviour
{
  public bool drawGizmos = true;
  [HideInInspector]
  public bool saveGetNearestHints = true;
  public StartEndModifier startEndModifier = new StartEndModifier();
  [HideInInspector]
  public TagMask traversableTags = new TagMask(-1, -1);
  [HideInInspector]
  public int[] tagPenalties = new int[32];
  private List<IPathModifier> modifiers = new List<IPathModifier>();
  public bool detailedGizmos;
  public OnPathDelegate pathCallback;
  public OnPathDelegate preProcessPath;
  public OnPathDelegate postProcessOriginalPath;
  public OnPathDelegate postProcessPath;
  [NonSerialized]
  public List<Vector3> lastCompletedVectorPath;
  [NonSerialized]
  public List<GraphNode> lastCompletedNodePath;
  [NonSerialized]
  protected Path path;
  private Path prevPath;
  private GraphNode startHint;
  private GraphNode endHint;
  private OnPathDelegate onPathDelegate;
  private OnPathDelegate onPartialPathDelegate;
  private OnPathDelegate tmpPathCallback;
  protected uint lastPathID;

  public Path GetCurrentPath()
  {
    return this.path;
  }

  public void Awake()
  {
    this.onPathDelegate = new OnPathDelegate(this.OnPathComplete);
    this.onPartialPathDelegate = new OnPathDelegate(this.OnPartialPathComplete);
    this.startEndModifier.Awake(this);
  }

  public void OnDestroy()
  {
    this.ReleaseClaimedPath();
    this.startEndModifier.OnDestroy(this);
  }

  public void ReleaseClaimedPath()
  {
    if (this.prevPath == null)
      return;
    this.prevPath.ReleaseSilent((object) this);
    this.prevPath = (Path) null;
  }

  public void RegisterModifier(IPathModifier mod)
  {
    if (this.modifiers == null)
      this.modifiers = new List<IPathModifier>(1);
    this.modifiers.Add(mod);
  }

  public void DeregisterModifier(IPathModifier mod)
  {
    if (this.modifiers == null)
      return;
    this.modifiers.Remove(mod);
  }

  public void PostProcess(Path p)
  {
    this.RunModifiers(Seeker.ModifierPass.PostProcess, p);
  }

  public void RunModifiers(Seeker.ModifierPass pass, Path p)
  {
    bool flag = true;
    while (flag)
    {
      flag = false;
      for (int index = 0; index < this.modifiers.Count - 1; ++index)
      {
        if (this.modifiers[index].Priority < this.modifiers[index + 1].Priority)
        {
          IPathModifier pathModifier = this.modifiers[index];
          this.modifiers[index] = this.modifiers[index + 1];
          this.modifiers[index + 1] = pathModifier;
          flag = true;
        }
      }
    }
    switch (pass)
    {
      case Seeker.ModifierPass.PreProcess:
        if (this.preProcessPath != null)
        {
          this.preProcessPath(p);
          break;
        }
        break;
      case Seeker.ModifierPass.PostProcessOriginal:
        if (this.postProcessOriginalPath != null)
        {
          this.postProcessOriginalPath(p);
          break;
        }
        break;
      case Seeker.ModifierPass.PostProcess:
        if (this.postProcessPath != null)
        {
          this.postProcessPath(p);
          break;
        }
        break;
    }
    if (this.modifiers.Count == 0)
      return;
    ModifierData input = ModifierData.All;
    IPathModifier pathModifier1 = this.modifiers[0];
    for (int index = 0; index < this.modifiers.Count; ++index)
    {
      MonoModifier monoModifier = this.modifiers[index] as MonoModifier;
      if (!((UnityEngine.Object) monoModifier != (UnityEngine.Object) null) || monoModifier.enabled)
      {
        switch (pass)
        {
          case Seeker.ModifierPass.PreProcess:
            this.modifiers[index].PreProcess(p);
            break;
          case Seeker.ModifierPass.PostProcessOriginal:
            this.modifiers[index].ApplyOriginal(p);
            break;
          case Seeker.ModifierPass.PostProcess:
            ModifierData source = ModifierConverter.Convert(p, input, this.modifiers[index].input);
            if (source != ModifierData.None)
            {
              this.modifiers[index].Apply(p, source);
              input = this.modifiers[index].output;
            }
            else
            {
              UnityEngine.Debug.Log((object) ("Error converting " + (index <= 0 ? "original" : pathModifier1.GetType().Name) + "'s output to " + this.modifiers[index].GetType().Name + "'s input.\nTry rearranging the modifier priorities on the Seeker."));
              input = ModifierData.None;
            }
            pathModifier1 = this.modifiers[index];
            break;
        }
        if (input == ModifierData.None)
          break;
      }
    }
  }

  public bool IsDone()
  {
    if (this.path != null)
      return this.path.GetState() >= PathState.Returned;
    return true;
  }

  public void OnPathComplete(Path p)
  {
    this.OnPathComplete(p, true, true);
  }

  public void OnPathComplete(Path p, bool runModifiers, bool sendCallbacks)
  {
    if (p != null && p != this.path && sendCallbacks || ((UnityEngine.Object) this == (UnityEngine.Object) null || p == null || p != this.path))
      return;
    if (!this.path.error && runModifiers)
    {
      this.RunModifiers(Seeker.ModifierPass.PostProcessOriginal, this.path);
      this.RunModifiers(Seeker.ModifierPass.PostProcess, this.path);
    }
    if (!sendCallbacks)
      return;
    p.Claim((object) this);
    this.lastCompletedNodePath = p.path;
    this.lastCompletedVectorPath = p.vectorPath;
    if (this.tmpPathCallback != null)
      this.tmpPathCallback(p);
    if (this.pathCallback != null)
      this.pathCallback(p);
    if (this.prevPath != null)
      this.prevPath.ReleaseSilent((object) this);
    this.prevPath = p;
    if (this.drawGizmos)
      return;
    this.ReleaseClaimedPath();
  }

  public void OnPartialPathComplete(Path p)
  {
    this.OnPathComplete(p, true, false);
  }

  public void OnMultiPathComplete(Path p)
  {
    this.OnPathComplete(p, false, true);
  }

  public ABPath GetNewPath(Vector3 start, Vector3 end)
  {
    return ABPath.Construct(start, end, (OnPathDelegate) null);
  }

  public Path StartPath(Vector3 start, Vector3 end)
  {
    return this.StartPath(start, end, (OnPathDelegate) null, -1);
  }

  public Path StartPath(Vector3 start, Vector3 end, OnPathDelegate callback)
  {
    return this.StartPath(start, end, callback, -1);
  }

  public Path StartPath(Vector3 start, Vector3 end, OnPathDelegate callback, int graphMask)
  {
    return this.StartPath((Path) this.GetNewPath(start, end), callback, graphMask);
  }

  public Path StartPath(Path p, OnPathDelegate callback = null, int graphMask = -1)
  {
    p.enabledTags = this.traversableTags.tagsChange;
    p.tagPenalties = this.tagPenalties;
    if (this.path != null && this.path.GetState() <= PathState.Processing && (int) this.lastPathID == (int) this.path.pathID)
    {
      this.path.Error();
      this.path.LogError("Canceled path because a new one was requested.\nThis happens when a new path is requested from the seeker when one was already being calculated.\nFor example if a unit got a new order, you might request a new path directly instead of waiting for the now invalid path to be calculated. Which is probably what you want.\nIf you are getting this a lot, you might want to consider how you are scheduling path requests.");
    }
    this.path = p;
    this.path.callback += this.onPathDelegate;
    this.path.nnConstraint.graphMask = graphMask;
    this.tmpPathCallback = callback;
    this.lastPathID = (uint) this.path.pathID;
    this.RunModifiers(Seeker.ModifierPass.PreProcess, this.path);
    AstarPath.StartPath(this.path, false);
    return this.path;
  }

  public MultiTargetPath StartMultiTargetPath(Vector3 start, Vector3[] endPoints, bool pathsForAll, OnPathDelegate callback = null, int graphMask = -1)
  {
    MultiTargetPath p = MultiTargetPath.Construct(start, endPoints, (OnPathDelegate[]) null, (OnPathDelegate) null);
    p.pathsForAll = pathsForAll;
    return this.StartMultiTargetPath(p, callback, graphMask);
  }

  public MultiTargetPath StartMultiTargetPath(Vector3[] startPoints, Vector3 end, bool pathsForAll, OnPathDelegate callback = null, int graphMask = -1)
  {
    MultiTargetPath p = MultiTargetPath.Construct(startPoints, end, (OnPathDelegate[]) null, (OnPathDelegate) null);
    p.pathsForAll = pathsForAll;
    return this.StartMultiTargetPath(p, callback, graphMask);
  }

  public MultiTargetPath StartMultiTargetPath(MultiTargetPath p, OnPathDelegate callback = null, int graphMask = -1)
  {
    if (this.path != null && this.path.GetState() <= PathState.Processing && (int) this.lastPathID == (int) this.path.pathID)
      this.path.ForceLogError("Canceled path because a new one was requested");
    OnPathDelegate[] onPathDelegateArray = new OnPathDelegate[p.targetPoints.Length];
    for (int index = 0; index < onPathDelegateArray.Length; ++index)
      onPathDelegateArray[index] = this.onPartialPathDelegate;
    p.callbacks = onPathDelegateArray;
    MultiTargetPath multiTargetPath = p;
    OnPathDelegate onPathDelegate = multiTargetPath.callback + new OnPathDelegate(this.OnMultiPathComplete);
    multiTargetPath.callback = onPathDelegate;
    p.nnConstraint.graphMask = graphMask;
    this.path = (Path) p;
    this.tmpPathCallback = callback;
    this.lastPathID = (uint) this.path.pathID;
    this.RunModifiers(Seeker.ModifierPass.PreProcess, this.path);
    AstarPath.StartPath(this.path, false);
    return p;
  }

  [DebuggerHidden]
  public IEnumerator DelayPathStart(Path p)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new Seeker.\u003CDelayPathStart\u003Ec__Iterator2()
    {
      p = p,
      \u003C\u0024\u003Ep = p,
      \u003C\u003Ef__this = this
    };
  }

  public void OnDrawGizmos()
  {
    if (this.lastCompletedNodePath == null || !this.drawGizmos)
      return;
    if (this.detailedGizmos)
    {
      Gizmos.color = new Color(0.7f, 0.5f, 0.1f, 0.5f);
      if (this.lastCompletedNodePath != null)
      {
        for (int index = 0; index < this.lastCompletedNodePath.Count - 1; ++index)
          Gizmos.DrawLine((Vector3) this.lastCompletedNodePath[index].position, (Vector3) this.lastCompletedNodePath[index + 1].position);
      }
    }
    Gizmos.color = new Color(0.0f, 1f, 0.0f, 1f);
    if (this.lastCompletedVectorPath == null)
      return;
    for (int index = 0; index < this.lastCompletedVectorPath.Count - 1; ++index)
      Gizmos.DrawLine(this.lastCompletedVectorPath[index], this.lastCompletedVectorPath[index + 1]);
  }

  public enum ModifierPass
  {
    PreProcess,
    PostProcessOriginal,
    PostProcess,
  }
}
