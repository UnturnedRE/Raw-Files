// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.StructureRegion
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
  public class StructureRegion
  {
    private List<Transform> _models;
    private List<StructureData> _structures;
    public bool isNetworked;

    public List<Transform> models
    {
      get
      {
        return this._models;
      }
    }

    public List<StructureData> structures
    {
      get
      {
        return this._structures;
      }
    }

    public StructureRegion()
    {
      this._models = new List<Transform>();
      this._structures = new List<StructureData>();
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
