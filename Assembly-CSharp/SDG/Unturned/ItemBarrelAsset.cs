// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ItemBarrelAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class ItemBarrelAsset : ItemCaliberAsset
  {
    protected AudioClip _shoot;
    protected GameObject _barrel;
    private bool _isBraked;
    private bool _isSilenced;
    private float _volume;

    public AudioClip shoot
    {
      get
      {
        return this._shoot;
      }
    }

    public GameObject barrel
    {
      get
      {
        return this._barrel;
      }
    }

    public bool isBraked
    {
      get
      {
        return this._isBraked;
      }
    }

    public bool isSilenced
    {
      get
      {
        return this._isSilenced;
      }
    }

    public float volume
    {
      get
      {
        return this._volume;
      }
    }

    public ItemBarrelAsset(Bundle bundle, Data data, ushort id)
      : base(bundle, data, id)
    {
      this._shoot = (AudioClip) bundle.load("Shoot");
      this._barrel = (GameObject) bundle.load("Barrel");
      this._isBraked = data.has("Braked");
      this._isSilenced = data.has("Silenced");
      this._volume = data.readSingle("Volume");
      bundle.unload();
    }
  }
}
