// Decompiled with JetBrains decompiler
// Type: Pathfinding.FloodPathTracer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

namespace Pathfinding
{
  public class FloodPathTracer : ABPath
  {
    protected FloodPath flood;

    [Obsolete("Use the Construct method instead")]
    public FloodPathTracer(Vector3 start, FloodPath flood, OnPathDelegate callbackDelegate)
    {
      throw new Exception("This constructor is obsolete");
    }

    public FloodPathTracer()
    {
    }

    public static FloodPathTracer Construct(Vector3 start, FloodPath flood, OnPathDelegate callback = null)
    {
      FloodPathTracer path = PathPool<FloodPathTracer>.GetPath();
      path.Setup(start, flood, callback);
      return path;
    }

    protected void Setup(Vector3 start, FloodPath flood, OnPathDelegate callback)
    {
      this.flood = flood;
      if (flood == null || flood.GetState() < PathState.Returned)
        throw new ArgumentException("You must supply a calculated FloodPath to the 'flood' argument");
      this.Setup(start, flood.originalStartPoint, callback);
      this.nnConstraint = (NNConstraint) new FloodPathConstraint(flood);
      this.hasEndPoint = false;
    }

    public override void Reset()
    {
      base.Reset();
      this.flood = (FloodPath) null;
    }

    protected override void Recycle()
    {
      PathPool<FloodPathTracer>.Recycle(this);
    }

    public override void Initialize()
    {
      if (this.startNode != null && this.flood.HasPathTo(this.startNode))
      {
        this.Trace(this.startNode);
        this.CompleteState = PathCompleteState.Complete;
      }
      else
      {
        this.Error();
        this.LogError("Could not find valid start node");
      }
    }

    public override void CalculateStep(long targetTick)
    {
      if (this.IsDone())
        return;
      this.Error();
      this.LogError("Something went wrong. At this point the path should be completed");
    }

    public void Trace(GraphNode from)
    {
      GraphNode node = from;
      int num = 0;
      while (node != null)
      {
        this.path.Add(node);
        this.vectorPath.Add((Vector3) node.position);
        node = this.flood.GetParent(node);
        ++num;
        if (num > 1024)
        {
          Debug.LogWarning((object) "Inifinity loop? >1024 node path. Remove this message if you really have that long paths (FloodPathTracer.cs, Trace function)");
          break;
        }
      }
    }
  }
}
