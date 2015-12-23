// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.InteractableSafezone
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class InteractableSafezone : InteractablePower
  {
    private bool _isPowered;
    private Transform engine;
    private SafezoneBubble bubble;

    public bool isPowered
    {
      get
      {
        return this._isPowered;
      }
    }

    protected override void updateWired()
    {
      if ((Object) this.engine != (Object) null)
        this.engine.gameObject.SetActive(this.isWired && this.isPowered);
      this.updateBubble();
    }

    public void updatePowered(bool newPowered)
    {
      this._isPowered = newPowered;
      if ((Object) this.engine != (Object) null)
        this.engine.gameObject.SetActive(this.isWired && this.isPowered);
      this.updateBubble();
    }

    private void updateBubble()
    {
      if (this.isWired && this.isPowered)
        this.registerBubble();
      else
        this.deregisterBubble();
    }

    public override void updateState(Asset asset, byte[] state)
    {
      base.updateState(asset, state);
      this._isPowered = (int) state[0] == 1;
      if (Dedicator.isDedicated)
        return;
      this.engine = this.transform.FindChild("Engine");
    }

    public override void use()
    {
      BarricadeManager.toggleSafezone(this.transform);
    }

    public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
    {
      message = !this.isPowered ? EPlayerMessage.SPOT_ON : EPlayerMessage.SPOT_OFF;
      text = string.Empty;
      color = Color.white;
      return true;
    }

    private void registerBubble()
    {
      if (!Provider.isServer || this.bubble != null || this.isPlant)
        return;
      this.bubble = SafezoneManager.registerBubble(this.transform.position, 16f);
    }

    private void deregisterBubble()
    {
      if (!Provider.isServer || this.bubble == null)
        return;
      SafezoneManager.deregisterBubble(this.bubble);
      this.bubble = (SafezoneBubble) null;
    }

    private void OnDestroy()
    {
      this.deregisterBubble();
    }
  }
}
