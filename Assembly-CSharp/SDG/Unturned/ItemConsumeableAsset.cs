// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ItemConsumeableAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class ItemConsumeableAsset : ItemAsset
  {
    protected AudioClip _use;
    private byte _health;
    private byte _food;
    private byte _water;
    private byte _virus;
    private byte _disinfectant;
    private byte _energy;
    private byte _vision;
    private uint _warmth;
    private bool _hasBleeding;
    private bool _hasBroken;
    private bool _hasAid;

    public AudioClip use
    {
      get
      {
        return this._use;
      }
    }

    public byte health
    {
      get
      {
        return this._health;
      }
    }

    public byte food
    {
      get
      {
        return this._food;
      }
    }

    public byte water
    {
      get
      {
        return this._water;
      }
    }

    public byte virus
    {
      get
      {
        return this._virus;
      }
    }

    public byte disinfectant
    {
      get
      {
        return this._disinfectant;
      }
    }

    public byte energy
    {
      get
      {
        return this._energy;
      }
    }

    public byte vision
    {
      get
      {
        return this._vision;
      }
    }

    public uint warmth
    {
      get
      {
        return this._warmth;
      }
    }

    public bool hasBleeding
    {
      get
      {
        return this._hasBleeding;
      }
    }

    public bool hasBroken
    {
      get
      {
        return this._hasBroken;
      }
    }

    public bool hasAid
    {
      get
      {
        return this._hasAid;
      }
    }

    public override bool showQuality
    {
      get
      {
        if (this.type != EItemType.FOOD)
          return this.type == EItemType.WATER;
        return true;
      }
    }

    public ItemConsumeableAsset(Bundle bundle, Data data, ushort id)
      : base(bundle, data, id)
    {
      this._use = (AudioClip) bundle.load("Use");
      this._health = data.readByte("Health");
      this._food = data.readByte("Food");
      this._water = data.readByte("Water");
      this._virus = data.readByte("Virus");
      this._disinfectant = data.readByte("Disinfectant");
      this._energy = data.readByte("Energy");
      this._vision = data.readByte("Vision");
      this._warmth = data.readUInt32("Warmth");
      this._hasBleeding = data.has("Bleeding");
      this._hasBroken = data.has("Broken");
      this._hasAid = data.has("Aid");
    }
  }
}
