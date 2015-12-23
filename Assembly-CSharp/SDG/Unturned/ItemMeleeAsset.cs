// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ItemMeleeAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class ItemMeleeAsset : ItemWeaponAsset
  {
    protected AudioClip _use;
    private float _strength;
    private float _weak;
    private float _strong;
    private byte _stamina;
    private bool _isRepair;
    private bool _isRepeated;
    private bool _isLight;

    public AudioClip use
    {
      get
      {
        return this._use;
      }
    }

    public float strength
    {
      get
      {
        return this._strength;
      }
    }

    public float weak
    {
      get
      {
        return this._weak;
      }
    }

    public float strong
    {
      get
      {
        return this._strong;
      }
    }

    public byte stamina
    {
      get
      {
        return this._stamina;
      }
    }

    public bool isRepair
    {
      get
      {
        return this._isRepair;
      }
    }

    public bool isRepeated
    {
      get
      {
        return this._isRepeated;
      }
    }

    public bool isLight
    {
      get
      {
        return this._isLight;
      }
    }

    public override bool showQuality
    {
      get
      {
        return true;
      }
    }

    public ItemMeleeAsset(Bundle bundle, Data data, ushort id)
      : base(bundle, data, id)
    {
      this._use = (AudioClip) bundle.load("Use");
      this._strength = data.readSingle("Strength");
      this._weak = data.readSingle("Weak");
      if ((double) this.weak < 0.01)
        this._weak = 0.5f;
      this._strong = data.readSingle("Strong");
      if ((double) this.strong < 0.01)
        this._strong = 0.33f;
      this._stamina = data.readByte("Stamina");
      this._isRepair = data.has("Repair");
      this._isRepeated = data.has("Repeated");
      this._isLight = data.has("Light");
      bundle.unload();
    }

    public override byte[] getState(bool isFull)
    {
      if (!this.isLight)
        return new byte[0];
      return new byte[1]
      {
        (byte) 1
      };
    }
  }
}
