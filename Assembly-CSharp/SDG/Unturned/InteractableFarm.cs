// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.InteractableFarm
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

namespace SDG.Unturned
{
  public class InteractableFarm : Interactable
  {
    private uint _planted;
    private uint _growth;
    private ushort _grow;
    private bool isGrown;

    public uint planted
    {
      get
      {
        return this._planted;
      }
    }

    public uint growth
    {
      get
      {
        return this._growth;
      }
    }

    public ushort grow
    {
      get
      {
        return this._grow;
      }
    }

    public void updatePlanted(uint newPlanted)
    {
      this._planted = newPlanted;
    }

    public override void updateState(Asset asset, byte[] state)
    {
      this._growth = ((ItemFarmAsset) asset).growth;
      this._grow = ((ItemFarmAsset) asset).grow;
      this._planted = BitConverter.ToUInt32(state, 0);
    }

    public bool checkFarm()
    {
      if (this.planted > 0U)
        return Provider.time - this.planted > this.growth;
      return false;
    }

    public override bool checkUseable()
    {
      return this.checkFarm();
    }

    public override void use()
    {
      BarricadeManager.farm(this.transform);
    }

    public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
    {
      message = !this.checkUseable() ? EPlayerMessage.GROW : EPlayerMessage.FARM;
      text = string.Empty;
      color = Color.white;
      return true;
    }

    private void FixedUpdate()
    {
      if (Dedicator.isDedicated || this.isGrown || !this.checkFarm())
        return;
      this.isGrown = true;
      this.transform.FindChild("Foliage_0").gameObject.SetActive(false);
      this.transform.FindChild("Foliage_1").gameObject.SetActive(true);
    }
  }
}
