// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.BarricadeRegion
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
  public class BarricadeRegion
  {
    private List<Transform> _models;
    private List<BarricadeData> _barricades;
    private Transform _parent;
    public bool isNetworked;

    public List<Transform> models
    {
      get
      {
        return this._models;
      }
    }

    public List<BarricadeData> barricades
    {
      get
      {
        return this._barricades;
      }
    }

    public Transform parent
    {
      get
      {
        return this._parent;
      }
    }

    public BarricadeRegion(Transform newParent)
    {
      this._models = new List<Transform>();
      this._barricades = new List<BarricadeData>();
      this._parent = newParent;
      this.isNetworked = false;
    }

    public void destroy()
    {
      for (ushort index = (ushort) 0; (int) index < this.models.Count; ++index)
        Object.Destroy((Object) this.models[(int) index].gameObject);
      this.models.Clear();
    }
  }
}
