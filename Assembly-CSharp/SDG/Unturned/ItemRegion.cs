// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ItemRegion
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
  public class ItemRegion
  {
    private List<Transform> _models;
    public List<ItemData> items;
    public bool isNetworked;
    public ushort despawnItemIndex;
    public ushort respawnItemIndex;
    public float lastRespawn;

    public List<Transform> models
    {
      get
      {
        return this._models;
      }
    }

    public ItemRegion()
    {
      this._models = new List<Transform>();
      this.items = new List<ItemData>();
      this.isNetworked = false;
      this.lastRespawn = Time.realtimeSinceStartup;
      this.despawnItemIndex = (ushort) 0;
      this.respawnItemIndex = (ushort) 0;
    }

    public void destroy()
    {
      for (ushort index = (ushort) 0; (int) index < this.models.Count; ++index)
        Object.Destroy((Object) this.models[(int) index].gameObject);
      this.models.Clear();
    }
  }
}
