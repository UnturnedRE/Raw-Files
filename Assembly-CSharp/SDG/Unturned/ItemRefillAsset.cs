// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ItemRefillAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class ItemRefillAsset : ItemAsset
  {
    protected AudioClip _use;
    protected byte _water;
    private byte _durability;

    public AudioClip use
    {
      get
      {
        return this._use;
      }
    }

    public byte water
    {
      get
      {
        return this._water;
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

    public ItemRefillAsset(Bundle bundle, Data data, ushort id)
      : base(bundle, data, id)
    {
      this._use = (AudioClip) bundle.load("Use");
      this._water = data.readByte("Water");
      this._durability = data.readByte("Durability");
      bundle.unload();
    }

    public override byte[] getState(bool isFull)
    {
      return new byte[1];
    }
  }
}
