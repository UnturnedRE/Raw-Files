// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ItemCaliberAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class ItemCaliberAsset : ItemAsset
  {
    private byte[] _calibers;
    private float _recoil_x;
    private float _recoil_y;
    private float _spread;
    private float _shake;
    private float _damage;
    private byte _firerate;
    protected bool _isPaintable;
    protected Texture2D _albedoBase;
    protected Texture2D _metallicBase;
    protected Texture2D _emissionBase;

    public byte[] calibers
    {
      get
      {
        return this._calibers;
      }
    }

    public float recoil_x
    {
      get
      {
        return this._recoil_x;
      }
    }

    public float recoil_y
    {
      get
      {
        return this._recoil_y;
      }
    }

    public float spread
    {
      get
      {
        return this._spread;
      }
    }

    public float shake
    {
      get
      {
        return this._shake;
      }
    }

    public float damage
    {
      get
      {
        return this._damage;
      }
    }

    public byte firerate
    {
      get
      {
        return this._firerate;
      }
    }

    public bool isPaintable
    {
      get
      {
        return this._isPaintable;
      }
    }

    public Texture albedoBase
    {
      get
      {
        return (Texture) this._albedoBase;
      }
    }

    public Texture metallicBase
    {
      get
      {
        return (Texture) this._metallicBase;
      }
    }

    public Texture emissionBase
    {
      get
      {
        return (Texture) this._emissionBase;
      }
    }

    public ItemCaliberAsset(Bundle bundle, Data data, ushort id)
      : base(bundle, data, id)
    {
      this._calibers = new byte[(int) data.readByte("Calibers")];
      for (byte index = (byte) 0; (int) index < this.calibers.Length; ++index)
        this._calibers[(int) index] = data.readByte("Caliber_" + (object) index);
      this._recoil_x = data.readSingle("Recoil_X");
      if ((double) this.recoil_x < 0.01)
        this._recoil_x = 1f;
      this._recoil_y = data.readSingle("Recoil_Y");
      if ((double) this.recoil_y < 0.01)
        this._recoil_y = 1f;
      this._spread = data.readSingle("Spread");
      if ((double) this.spread < 0.01)
        this._spread = 1f;
      this._shake = data.readSingle("Shake");
      if ((double) this.shake < 0.01)
        this._shake = 1f;
      this._damage = data.readSingle("Damage");
      if ((double) this.damage < 0.01)
        this._damage = 1f;
      this._firerate = data.readByte("Firerate");
      this._isPaintable = data.has("Paintable");
      if (Dedicator.isDedicated)
        return;
      this._albedoBase = (Texture2D) bundle.load("Albedo_Base");
      this._metallicBase = (Texture2D) bundle.load("Metallic_Base");
      this._emissionBase = (Texture2D) bundle.load("Emission_Base");
    }
  }
}
