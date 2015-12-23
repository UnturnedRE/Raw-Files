// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ZombieRegion
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
  public class ZombieRegion
  {
    private List<Zombie> _zombies;
    public ushort updates;
    public ushort respawnZombieIndex;
    public int alive;
    public bool isNetworked;
    public float lastMega;
    public bool hasMega;

    public List<Zombie> zombies
    {
      get
      {
        return this._zombies;
      }
    }

    public ZombieRegion()
    {
      this._zombies = new List<Zombie>();
      this.updates = (ushort) 0;
      this.respawnZombieIndex = (ushort) 0;
      this.alive = 0;
      this.isNetworked = false;
      this.lastMega = -1000f;
      this.hasMega = false;
    }

    public void destroy()
    {
      for (ushort index = (ushort) 0; (int) index < this.zombies.Count; ++index)
        Object.Destroy((Object) this.zombies[(int) index].gameObject);
      this.zombies.Clear();
      this.hasMega = false;
    }
  }
}
