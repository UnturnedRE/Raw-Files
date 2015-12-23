// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.InteractableFire
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class InteractableFire : Interactable
  {
    private bool _isLit;
    private Transform fire;

    public bool isLit
    {
      get
      {
        return this._isLit;
      }
    }

    public void updateLit(bool newLit)
    {
      this._isLit = newLit;
      if (!((Object) this.fire != (Object) null))
        return;
      this.fire.gameObject.SetActive(this.isLit);
    }

    public override void updateState(Asset asset, byte[] state)
    {
      this._isLit = (int) state[0] == 1;
      this.fire = this.transform.FindChild("Fire");
      if (!((Object) this.fire != (Object) null))
        return;
      this.fire.gameObject.SetActive(this.isLit);
    }

    public override void use()
    {
      BarricadeManager.toggleFire(this.transform);
    }

    public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
    {
      message = !this.isLit ? EPlayerMessage.FIRE_ON : EPlayerMessage.FIRE_OFF;
      text = string.Empty;
      color = Color.white;
      return true;
    }
  }
}
