// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ItemStructureAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

namespace SDG.Unturned
{
  public class ItemStructureAsset : ItemAsset
  {
    protected GameObject _structure;
    protected GameObject _clip;
    protected GameObject _nav;
    protected AudioClip _use;
    protected EConstruct _construct;
    protected ushort _health;
    protected float _range;
    protected ushort _explosion;
    protected bool _isVulnerable;

    public GameObject structure
    {
      get
      {
        return this._structure;
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

    public EConstruct construct
    {
      get
      {
        return this._construct;
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

    public ushort explosion
    {
      get
      {
        return this._explosion;
      }
    }

    public bool isVulnerable
    {
      get
      {
        return this._isVulnerable;
      }
    }

    public ItemStructureAsset(Bundle bundle, Data data, ushort id)
      : base(bundle, data, id)
    {
      this._structure = (GameObject) bundle.load("Structure");
      this._clip = (GameObject) bundle.load("Clip");
      this._nav = (GameObject) bundle.load("Nav");
      this._use = (AudioClip) bundle.load("Use");
      this._construct = (EConstruct) Enum.Parse(typeof (EConstruct), data.readString("Construct"), true);
      this._health = data.readUInt16("Health");
      this._range = data.readSingle("Range");
      this._explosion = data.readUInt16("Explosion");
      this._isVulnerable = data.has("Vulnerable");
      bundle.unload();
    }
  }
}
