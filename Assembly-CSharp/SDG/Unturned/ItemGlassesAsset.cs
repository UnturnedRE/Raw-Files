// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ItemGlassesAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

namespace SDG.Unturned
{
  public class ItemGlassesAsset : ItemGearAsset
  {
    protected GameObject _glasses;
    private ELightingVision _vision;

    public GameObject glasses
    {
      get
      {
        return this._glasses;
      }
    }

    public ELightingVision vision
    {
      get
      {
        return this._vision;
      }
    }

    public ItemGlassesAsset(Bundle bundle, Data data, ushort id)
      : base(bundle, data, id)
    {
      if (!Dedicator.isDedicated)
        this._glasses = (GameObject) bundle.load("Glasses");
      this._vision = !data.has("Vision") ? ELightingVision.NONE : (ELightingVision) Enum.Parse(typeof (ELightingVision), data.readString("Vision"), true);
      bundle.unload();
    }

    public override byte[] getState(bool isFull)
    {
      if (this.vision == ELightingVision.NONE)
        return new byte[0];
      return new byte[1]
      {
        (byte) 1
      };
    }
  }
}
