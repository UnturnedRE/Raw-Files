﻿// Decompiled with JetBrains decompiler
// Type: Pathfinding.TileHandlerHelper
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
  public class TileHandlerHelper : MonoBehaviour
  {
    private float lastUpdateTime = -999f;
    private List<Bounds> forcedReloadBounds = new List<Bounds>();
    private TileHandler handler;
    public float updateInterval;

    public void UseSpecifiedHandler(TileHandler handler)
    {
      this.handler = handler;
    }

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
      if (UnityEngine.Object.FindObjectsOfType(typeof (TileHandlerHelper)).Length > 1)
      {
        Debug.LogError((object) "There should only be one TileHandlerHelper per scene. Destroying.");
        UnityEngine.Object.Destroy((UnityEngine.Object) this);
      }
      else
      {
        if (this.handler != null)
          return;
        if ((UnityEngine.Object) AstarPath.active == (UnityEngine.Object) null || AstarPath.active.astarData.recastGraph == null)
          Debug.LogWarning((object) "No AstarPath object in the scene or no RecastGraph on that AstarPath object");
        if (AstarPath.active.astarData.recastGraph == null)
          return;
        this.handler = new TileHandler(AstarPath.active.astarData.recastGraph);
        this.handler.CreateTileTypesFromGraph();
      }
    }

    private void HandleOnDestroyCallback(NavmeshCut obj)
    {
      Debug.Log((object) ("adding " + (object) obj.LastBounds));
      this.forcedReloadBounds.Add(obj.LastBounds);
      this.lastUpdateTime = -999f;
    }

    private void Update()
    {
      if ((double) this.updateInterval == -1.0 || (double) Time.realtimeSinceStartup - (double) this.lastUpdateTime < (double) this.updateInterval || this.handler == null)
        return;
      this.ForceUpdate();
    }

    public void ForceUpdate()
    {
      if (this.handler == null)
        throw new Exception("Cannot update graphs. No TileHandler. Do not call this method in Awake.");
      this.lastUpdateTime = Time.realtimeSinceStartup;
      List<NavmeshCut> all = NavmeshCut.GetAll();
      if (this.forcedReloadBounds.Count == 0)
      {
        int num = 0;
        for (int index = 0; index < all.Count; ++index)
        {
          if (all[index].RequiresUpdate())
          {
            ++num;
            break;
          }
        }
        if (num == 0)
          return;
      }
      bool flag = this.handler.StartBatchLoad();
      for (int index = 0; index < this.forcedReloadBounds.Count; ++index)
        this.handler.ReloadInBounds(this.forcedReloadBounds[index]);
      this.forcedReloadBounds.Clear();
      for (int index = 0; index < all.Count; ++index)
      {
        if (all[index].enabled)
        {
          if (all[index].RequiresUpdate())
          {
            this.handler.ReloadInBounds(all[index].LastBounds);
            this.handler.ReloadInBounds(all[index].GetBounds());
          }
        }
        else if (all[index].RequiresUpdate())
          this.handler.ReloadInBounds(all[index].LastBounds);
      }
      for (int index = 0; index < all.Count; ++index)
      {
        if (all[index].RequiresUpdate())
          all[index].NotifyUpdated();
      }
      if (!flag)
        return;
      this.handler.EndBatchLoad();
    }
  }
}
