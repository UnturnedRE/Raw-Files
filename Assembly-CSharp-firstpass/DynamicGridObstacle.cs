// Decompiled with JetBrains decompiler
// Type: DynamicGridObstacle
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class DynamicGridObstacle : MonoBehaviour
{
  public float updateError = 1f;
  public float checkTime = 0.2f;
  private Collider col;
  private Bounds prevBounds;
  private bool isWaitingForUpdate;

  private void Start()
  {
    this.col = this.GetComponent<Collider>();
    if ((UnityEngine.Object) this.GetComponent<Collider>() == (UnityEngine.Object) null)
      UnityEngine.Debug.LogError((object) "A collider must be attached to the GameObject for DynamicGridObstacle to work");
    this.StartCoroutine(this.UpdateGraphs());
  }

  [DebuggerHidden]
  private IEnumerator UpdateGraphs()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new DynamicGridObstacle.\u003CUpdateGraphs\u003Ec__Iterator10()
    {
      \u003C\u003Ef__this = this
    };
  }

  public void OnDestroy()
  {
    if (!((UnityEngine.Object) AstarPath.active != (UnityEngine.Object) null))
      return;
    GraphUpdateObject ob = new GraphUpdateObject(this.prevBounds);
    AstarPath.active.UpdateGraphs(ob);
  }

  public void DoUpdateGraphs()
  {
    if ((UnityEngine.Object) this.col == (UnityEngine.Object) null)
      return;
    this.isWaitingForUpdate = false;
    Bounds bounds1 = this.col.bounds;
    Bounds bounds2 = bounds1;
    bounds2.Encapsulate(this.prevBounds);
    if ((double) this.BoundsVolume(bounds2) < (double) this.BoundsVolume(bounds1) + (double) this.BoundsVolume(this.prevBounds))
    {
      AstarPath.active.UpdateGraphs(bounds2);
    }
    else
    {
      AstarPath.active.UpdateGraphs(this.prevBounds);
      AstarPath.active.UpdateGraphs(bounds1);
    }
    this.prevBounds = bounds1;
  }

  public float BoundsVolume(Bounds b)
  {
    return Math.Abs(b.size.x * b.size.y * b.size.z);
  }
}
