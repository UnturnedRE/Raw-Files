// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ItemHatAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class ItemHatAsset : ItemGearAsset
  {
    protected GameObject _hat;

    public GameObject hat
    {
      get
      {
        return this._hat;
      }
    }

    public ItemHatAsset(Bundle bundle, Data data, ushort id)
      : base(bundle, data, id)
    {
      if (!Dedicator.isDedicated)
        this._hat = (GameObject) bundle.load("Hat");
      bundle.unload();
    }
  }
}
