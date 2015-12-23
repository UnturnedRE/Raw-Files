// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.InteractableSpot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class InteractableSpot : InteractablePower
  {
    private bool _isPowered;
    private Transform spot;

    public bool isPowered
    {
      get
      {
        return this._isPowered;
      }
    }

    protected override void updateWired()
    {
      if (!((Object) this.spot != (Object) null))
        return;
      this.spot.gameObject.SetActive(this.isWired && this.isPowered);
    }

    public void updatePowered(bool newPowered)
    {
      this._isPowered = newPowered;
      if (!((Object) this.spot != (Object) null))
        return;
      this.spot.gameObject.SetActive(this.isWired && this.isPowered);
    }

    public override void updateState(Asset asset, byte[] state)
    {
      base.updateState(asset, state);
      this._isPowered = (int) state[0] == 1;
      if (Dedicator.isDedicated)
        return;
      this.spot = this.transform.FindChild("Spots");
    }

    public override void use()
    {
      BarricadeManager.toggleSpot(this.transform);
    }

    public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
    {
      message = !this.isPowered ? EPlayerMessage.SPOT_ON : EPlayerMessage.SPOT_OFF;
      text = string.Empty;
      color = Color.white;
      return true;
    }
  }
}
