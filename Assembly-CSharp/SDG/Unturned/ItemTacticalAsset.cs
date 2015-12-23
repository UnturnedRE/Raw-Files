// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ItemTacticalAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class ItemTacticalAsset : ItemCaliberAsset
  {
    protected GameObject _tactical;
    private bool _isLaser;
    private bool _isLight;
    private bool _isRangefinder;

    public GameObject tactical
    {
      get
      {
        return this._tactical;
      }
    }

    public bool isLaser
    {
      get
      {
        return this._isLaser;
      }
    }

    public bool isLight
    {
      get
      {
        return this._isLight;
      }
    }

    public bool isRangefinder
    {
      get
      {
        return this._isRangefinder;
      }
    }

    public ItemTacticalAsset(Bundle bundle, Data data, ushort id)
      : base(bundle, data, id)
    {
      this._tactical = (GameObject) bundle.load("Tactical");
      this._isLaser = data.has("Laser");
      this._isLight = data.has("Light");
      this._isRangefinder = data.has("Rangefinder");
      bundle.unload();
    }
  }
}
