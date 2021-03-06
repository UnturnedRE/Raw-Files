﻿// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ItemToolAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class ItemToolAsset : ItemAsset
  {
    protected AudioClip _use;

    public AudioClip use
    {
      get
      {
        return this._use;
      }
    }

    public ItemToolAsset(Bundle bundle, Data data, ushort id)
      : base(bundle, data, id)
    {
      this._use = (AudioClip) bundle.load("Use");
      bundle.unload();
    }
  }
}
