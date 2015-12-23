// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ItemGripAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class ItemGripAsset : ItemCaliberAsset
  {
    protected GameObject _grip;
    private bool _isBipod;

    public GameObject grip
    {
      get
      {
        return this._grip;
      }
    }

    public bool isBipod
    {
      get
      {
        return this._isBipod;
      }
    }

    public ItemGripAsset(Bundle bundle, Data data, ushort id)
      : base(bundle, data, id)
    {
      this._grip = (GameObject) bundle.load("Grip");
      this._isBipod = data.has("Bipod");
      bundle.unload();
    }
  }
}
