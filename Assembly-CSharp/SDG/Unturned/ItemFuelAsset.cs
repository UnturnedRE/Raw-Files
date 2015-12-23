// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ItemFuelAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class ItemFuelAsset : ItemAsset
  {
    protected AudioClip _use;
    protected ushort _fuel;
    private byte _durability;

    public AudioClip use
    {
      get
      {
        return this._use;
      }
    }

    public ushort fuel
    {
      get
      {
        return this._fuel;
      }
    }

    public byte durability
    {
      get
      {
        return this._durability;
      }
    }

    public override bool showQuality
    {
      get
      {
        return true;
      }
    }

    public ItemFuelAsset(Bundle bundle, Data data, ushort id)
      : base(bundle, data, id)
    {
      this._use = (AudioClip) bundle.load("Use");
      this._fuel = data.readUInt16("Fuel");
      this._durability = data.readByte("Durability");
      bundle.unload();
    }

    public override byte[] getState(bool isFull)
    {
      return new byte[1];
    }
  }
}
