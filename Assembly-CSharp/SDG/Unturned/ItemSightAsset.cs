// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ItemSightAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class ItemSightAsset : ItemCaliberAsset
  {
    protected GameObject _sight;
    private float _zoom;

    public GameObject sight
    {
      get
      {
        return this._sight;
      }
    }

    public float zoom
    {
      get
      {
        return this._zoom;
      }
    }

    public ItemSightAsset(Bundle bundle, Data data, ushort id)
      : base(bundle, data, id)
    {
      this._sight = (GameObject) bundle.load("Sight");
      this._zoom = 90f / (float) data.readByte("Zoom");
      bundle.unload();
    }
  }
}
