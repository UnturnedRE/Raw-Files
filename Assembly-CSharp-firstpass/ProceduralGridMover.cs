// Decompiled with JetBrains decompiler
// Type: ProceduralGridMover
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class ProceduralGridMover : MonoBehaviour
{
  public float updateDistance = 5f;
  public Transform target;
  public bool floodFill;
  private GridGraph graph;
  private GridNode[] tmp;

  public void Start()
  {
    if ((UnityEngine.Object) AstarPath.active == (UnityEngine.Object) null)
      throw new Exception("There is no AstarPath object in the scene");
    this.graph = AstarPath.active.astarData.gridGraph;
    if (this.graph == null)
      throw new Exception("The AstarPath object has no GridGraph");
    this.UpdateGraph();
  }

  private void Update()
  {
    if ((double) (this.target.position - this.graph.center).sqrMagnitude <= (double) this.updateDistance * (double) this.updateDistance)
      return;
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: reference to a compiler-generated method
    AstarPath.active.AddWorkItem(new AstarPath.AstarWorkItem(new Func<bool, bool>(new ProceduralGridMover.\u003CUpdate\u003Ec__AnonStorey25()
    {
      ie = this.UpdateGraph()
    }.\u003C\u003Em__19)));
  }

  [DebuggerHidden]
  public IEnumerator UpdateGraph()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new ProceduralGridMover.\u003CUpdateGraph\u003Ec__IteratorB()
    {
      \u003C\u003Ef__this = this
    };
  }
}
