// Decompiled with JetBrains decompiler
// Type: Pathfinding.RVO.RVONavmesh
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.RVO
{
  [AddComponentMenu("Pathfinding/Local Avoidance/RVO Navmesh")]
  public class RVONavmesh : GraphModifier
  {
    public float wallHeight = 5f;
    private List<ObstacleVertex> obstacles = new List<ObstacleVertex>();
    private Simulator lastSim;

    public override void OnPostCacheLoad()
    {
      this.OnLatePostScan();
    }

    public override void OnLatePostScan()
    {
      if (!Application.isPlaying)
        return;
      this.RemoveObstacles();
      NavGraph[] graphs = AstarPath.active.graphs;
      RVOSimulator rvoSimulator = UnityEngine.Object.FindObjectOfType(typeof (RVOSimulator)) as RVOSimulator;
      if ((UnityEngine.Object) rvoSimulator == (UnityEngine.Object) null)
        throw new NullReferenceException("No RVOSimulator could be found in the scene. Please add one to any GameObject");
      Simulator simulator = rvoSimulator.GetSimulator();
      for (int index = 0; index < graphs.Length; ++index)
        this.AddGraphObstacles(simulator, graphs[index]);
      simulator.UpdateObstacles();
    }

    public void RemoveObstacles()
    {
      if (this.lastSim == null)
        return;
      Simulator simulator = this.lastSim;
      this.lastSim = (Simulator) null;
      for (int index = 0; index < this.obstacles.Count; ++index)
        simulator.RemoveObstacle(this.obstacles[index]);
      this.obstacles.Clear();
    }

    public void AddGraphObstacles(Simulator sim, NavGraph graph)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      RVONavmesh.\u003CAddGraphObstacles\u003Ec__AnonStorey36 obstaclesCAnonStorey36 = new RVONavmesh.\u003CAddGraphObstacles\u003Ec__AnonStorey36();
      // ISSUE: reference to a compiler-generated field
      obstaclesCAnonStorey36.sim = sim;
      // ISSUE: reference to a compiler-generated field
      obstaclesCAnonStorey36.\u003C\u003Ef__this = this;
      // ISSUE: reference to a compiler-generated field
      if (this.obstacles.Count > 0 && this.lastSim != null && this.lastSim != obstaclesCAnonStorey36.sim)
      {
        Debug.LogError((object) "Simulator has changed but some old obstacles are still added for the previous simulator. Deleting previous obstacles.");
        this.RemoveObstacles();
      }
      // ISSUE: reference to a compiler-generated field
      this.lastSim = obstaclesCAnonStorey36.sim;
      INavmesh navmesh = graph as INavmesh;
      if (navmesh == null)
        return;
      // ISSUE: reference to a compiler-generated field
      obstaclesCAnonStorey36.uses = new int[20];
      // ISSUE: reference to a compiler-generated method
      navmesh.GetNodes(new GraphNodeDelegateCancelable(obstaclesCAnonStorey36.\u003C\u003Em__2D));
    }
  }
}
