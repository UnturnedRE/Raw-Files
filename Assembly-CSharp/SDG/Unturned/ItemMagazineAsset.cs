// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ItemMagazineAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class ItemMagazineAsset : ItemCaliberAsset
  {
    protected GameObject _magazine;
    private byte _pellets;
    private byte _stuck;

    public GameObject magazine
    {
      get
      {
        return this._magazine;
      }
    }

    public byte pellets
    {
      get
      {
        return this._pellets;
      }
    }

    public byte stuck
    {
      get
      {
        return this._stuck;
      }
    }

    public override bool showQuality
    {
      get
      {
        return (int) this.stuck > 0;
      }
    }

    public ItemMagazineAsset(Bundle bundle, Data data, ushort id)
      : base(bundle, data, id)
    {
      this._magazine = (GameObject) bundle.load("Magazine");
      this._pellets = data.readByte("Pellets");
      if ((int) this.pellets < 1)
        this._pellets = (byte) 1;
      this._stuck = data.readByte("Stuck");
      bundle.unload();
    }
  }
}
