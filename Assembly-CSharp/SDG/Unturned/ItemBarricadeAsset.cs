// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ItemBarricadeAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

namespace SDG.Unturned
{
  public class ItemBarricadeAsset : ItemAsset
  {
    protected GameObject _barricade;
    protected GameObject _clip;
    protected GameObject _nav;
    protected AudioClip _use;
    protected EBuild _build;
    protected ushort _health;
    protected float _range;
    protected float _radius;
    protected float _offset;
    protected ushort _explosion;
    protected bool _isLocked;
    protected bool _isVulnerable;

    public GameObject barricade
    {
      get
      {
        return this._barricade;
      }
    }

    public GameObject clip
    {
      get
      {
        return this._clip;
      }
    }

    public GameObject nav
    {
      get
      {
        return this._nav;
      }
    }

    public AudioClip use
    {
      get
      {
        return this._use;
      }
    }

    public EBuild build
    {
      get
      {
        return this._build;
      }
    }

    public ushort health
    {
      get
      {
        return this._health;
      }
    }

    public float range
    {
      get
      {
        return this._range;
      }
    }

    public float radius
    {
      get
      {
        return this._radius;
      }
    }

    public float offset
    {
      get
      {
        return this._offset;
      }
    }

    public ushort explosion
    {
      get
      {
        return this._explosion;
      }
    }

    public bool isLocked
    {
      get
      {
        return this._isLocked;
      }
    }

    public bool isVulnerable
    {
      get
      {
        return this._isVulnerable;
      }
    }

    public ItemBarricadeAsset(Bundle bundle, Data data, ushort id)
      : base(bundle, data, id)
    {
      this._barricade = (GameObject) bundle.load("Barricade");
      this._clip = (GameObject) bundle.load("Clip");
      this._nav = (GameObject) bundle.load("Nav");
      this._use = (AudioClip) bundle.load("Use");
      this._build = (EBuild) Enum.Parse(typeof (EBuild), data.readString("Build"), true);
      this._health = data.readUInt16("Health");
      this._range = data.readSingle("Range");
      this._radius = data.readSingle("Radius");
      this._offset = data.readSingle("Offset");
      this._explosion = data.readUInt16("Explosion");
      this._isLocked = data.has("Locked");
      this._isVulnerable = data.has("Vulnerable");
      bundle.unload();
    }

    public override byte[] getState(bool isFull)
    {
      if (this.build == EBuild.DOOR || this.build == EBuild.GATE)
        return new byte[17];
      if (this.build == EBuild.BED)
        return new byte[8];
      if (this.build == EBuild.STORAGE)
        return new byte[17];
      if (this.build == EBuild.FARM)
        return new byte[4];
      if (this.build == EBuild.TORCH || this.build == EBuild.CAMPFIRE || (this.build == EBuild.SPOT || this.build == EBuild.SAFEZONE))
        return new byte[1];
      if (this.build == EBuild.GENERATOR)
        return new byte[3];
      if (this.build == EBuild.SIGN)
        return new byte[17];
      return new byte[0];
    }
  }
}
