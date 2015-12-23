// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.EffectAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

namespace SDG.Unturned
{
  public class EffectAsset : Asset
  {
    protected GameObject _effect;
    protected GameObject[] _splatters;
    private bool _gore;
    private byte _splatter;
    private float _lifetime;
    private bool _isStatic;

    public GameObject effect
    {
      get
      {
        return this._effect;
      }
    }

    public GameObject[] splatters
    {
      get
      {
        return this._splatters;
      }
    }

    public bool gore
    {
      get
      {
        return this._gore;
      }
    }

    public byte splatter
    {
      get
      {
        return this._splatter;
      }
    }

    public float lifetime
    {
      get
      {
        return this._lifetime;
      }
    }

    public bool isStatic
    {
      get
      {
        return this._isStatic;
      }
    }

    public EffectAsset(Bundle bundle, Data data, ushort id)
      : base(bundle, id)
    {
      if ((int) id < 200 && !bundle.hasResource)
        throw new NotSupportedException();
      this._effect = (GameObject) bundle.load("Effect");
      this._gore = data.has("Gore");
      this._splatters = new GameObject[(int) data.readByte("Splatter")];
      for (int index = 0; index < this.splatters.Length; ++index)
        this.splatters[index] = (GameObject) bundle.load("Splatter_" + (object) index);
      this._splatter = data.readByte("Splatters");
      this._lifetime = data.readSingle("Lifetime");
      this._isStatic = data.has("Static");
      bundle.unload();
    }
  }
}
