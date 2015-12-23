// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ItemFisherAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class ItemFisherAsset : ItemAsset
  {
    private AudioClip _cast;
    private AudioClip _reel;
    private AudioClip _tug;
    private byte _durability;
    private ushort[] _fishes;

    public AudioClip cast
    {
      get
      {
        return this._cast;
      }
    }

    public AudioClip reel
    {
      get
      {
        return this._reel;
      }
    }

    public AudioClip tug
    {
      get
      {
        return this._tug;
      }
    }

    public byte durability
    {
      get
      {
        return this._durability;
      }
    }

    public ushort[] fishes
    {
      get
      {
        return this._fishes;
      }
    }

    public override bool showQuality
    {
      get
      {
        return true;
      }
    }

    public ItemFisherAsset(Bundle bundle, Data data, ushort id)
      : base(bundle, data, id)
    {
      this._cast = (AudioClip) bundle.load("Cast");
      this._reel = (AudioClip) bundle.load("Reel");
      this._tug = (AudioClip) bundle.load("Tug");
      this._durability = data.readByte("Durability");
      this._fishes = new ushort[(int) data.readByte("Fishes")];
      for (byte index = (byte) 0; (int) index < this.fishes.Length; ++index)
        this._fishes[(int) index] = data.readUInt16("Fish_" + (object) index);
      bundle.unload();
    }
  }
}
