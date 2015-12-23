// Decompiled with JetBrains decompiler
// Type: Pathfinding.TileHandlerHelpers
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
  public class TileHandlerHelpers : MonoBehaviour
  {
    private List<TileHandler> handlers = new List<TileHandler>();
    private float lastUpdateTime = -999f;
    private List<Bounds> forcedReloadBounds = new List<Bounds>();
    public float updateInterval;

    private void OnEnable()
    {
      NavmeshCut.OnDestroyCallback += new Action<NavmeshCut>(this.HandleOnDestroyCallback);
    }

    private void OnDisable()
    {
      NavmeshCut.OnDestroyCallback -= new Action<NavmeshCut>(this.HandleOnDestroyCallback);
    }

    public void DiscardPending()
    {
      List<NavmeshCut> all = NavmeshCut.GetAll();
      for (int index = 0; index < all.Count; ++index)
      {
        if (all[index].RequiresUpdate())
          all[index].NotifyUpdated();
      }
    }

    private void Start()
    {
      if ((UnityEngine.Object) AstarPath.active == (UnityEngine.Object) null)
        Debug.LogWarning((object) "No AstarPath object in the scene or no RecastGraph on that AstarPath object");
      for (int index = 0; index < AstarPath.active.astarData.graphs.Length; ++index)
      {
        TileHandler tileHandler = new TileHandler((RecastGraph) AstarPath.active.astarData.graphs[index]);
        tileHandler.CreateTileTypesFromGraph();
        this.handlers.Add(tileHandler);
      }
    }

    private void HandleOnDestroyCallback(NavmeshCut obj)
    {
      this.forcedReloadBounds.Add(obj.LastBounds);
      this.lastUpdateTime = -999f;
    }

    private void Update()
    {
      if ((double) this.updateInterval == -1.0 || (double) Time.realtimeSinceStartup - (double) this.lastUpdateTime < (double) this.updateInterval || this.handlers.Count == 0)
        return;
      this.ForceUpdate();
    }

    public void ForceUpdate()
    {
      if (this.handlers.Count == 0)
        throw new Exception("Cannot update graphs. No TileHandler. Do not call this method in Awake.");
      this.lastUpdateTime = Time.realtimeSinceStartup;
      for (int index1 = 0; index1 < this.handlers.Count; ++index1)
      {
        TileHandler tileHandler = this.handlers[index1];
        List<NavmeshCut> allInRange = NavmeshCut.GetAllInRange(tileHandler.graph.forcedBounds);
        if (this.forcedReloadBounds.Count == 0)
        {
          int num = 0;
          for (int index2 = 0; index2 < allInRange.Count; ++index2)
          {
            if (allInRange[index2].RequiresUpdate())
            {
              ++num;
              break;
            }
          }
          if (num == 0)
            continue;
        }
        bool flag = tileHandler.StartBatchLoad();
        for (int index2 = 0; index2 < this.forcedReloadBounds.Count; ++index2)
          tileHandler.ReloadInBounds(this.forcedReloadBounds[index2]);
        for (int index2 = 0; index2 < allInRange.Count; ++index2)
        {
          if (allInRange[index2].enabled)
          {
            if (allInRange[index2].RequiresUpdate())
            {
              tileHandler.ReloadInBounds(allInRange[index2].LastBounds);
              tileHandler.ReloadInBounds(allInRange[index2].GetBounds());
            }
          }
          else if (allInRange[index2].RequiresUpdate())
            tileHandler.ReloadInBounds(allInRange[index2].LastBounds);
        }
        for (int index2 = 0; index2 < allInRange.Count; ++index2)
        {
          if (allInRange[index2].RequiresUpdate())
            allInRange[index2].NotifyUpdated();
        }
        if (flag)
          tileHandler.EndBatchLoad();
      }
      this.forcedReloadBounds.Clear();
    }
  }
}
