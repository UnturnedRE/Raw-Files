// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ItemThrowableAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class ItemThrowableAsset : ItemWeaponAsset
  {
    protected AudioClip _use;
    protected GameObject _throwable;
    private bool _isExplosive;
    private bool _isSticky;

    public AudioClip use
    {
      get
      {
        return this._use;
      }
    }

    public GameObject throwable
    {
      get
      {
        return this._throwable;
      }
    }

    public bool isExplosive
    {
      get
      {
        return this._isExplosive;
      }
    }

    public bool isSticky
    {
      get
      {
        return this._isSticky;
      }
    }

    public ItemThrowableAsset(Bundle bundle, Data data, ushort id)
      : base(bundle, data, id)
    {
      this._use = (AudioClip) bundle.load("Use");
      this._throwable = (GameObject) bundle.load("Throwable");
      this._isExplosive = data.has("Explosive");
      this._isSticky = data.has("Sticky");
      bundle.unload();
    }
  }
}
