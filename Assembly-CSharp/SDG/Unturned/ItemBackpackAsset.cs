// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ItemBackpackAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class ItemBackpackAsset : ItemBagAsset
  {
    protected GameObject _backpack;

    public GameObject backpack
    {
      get
      {
        return this._backpack;
      }
    }

    public ItemBackpackAsset(Bundle bundle, Data data, ushort id)
      : base(bundle, data, id)
    {
      if (!Dedicator.isDedicated)
        this._backpack = (GameObject) bundle.load("Backpack");
      bundle.unload();
    }
  }
}
