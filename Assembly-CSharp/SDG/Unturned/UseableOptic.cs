// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.UseableOptic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class UseableOptic : Useable
  {
    private bool isZoomed;

    public override void startSecondary()
    {
      if (!this.channel.isOwner || this.isZoomed || this.player.look.perspective != EPlayerPerspective.FIRST)
        return;
      this.isZoomed = true;
      this.startZoom();
    }

    public override void stopSecondary()
    {
      if (!this.channel.isOwner || !this.isZoomed)
        return;
      this.isZoomed = false;
      this.stopZoom();
    }

    private void startZoom()
    {
      this.player.animator.viewOffset = Vector3.up;
      this.player.animator.multiplier = 0.0f;
      this.player.look.enableZoom(((ItemOpticAsset) this.player.equipment.asset).zoom);
      this.player.look.sensitivity = ((ItemOpticAsset) this.player.equipment.asset).zoom / 90f;
      PlayerUI.updateBinoculars(true);
    }

    private void stopZoom()
    {
      this.player.animator.viewOffset = Vector3.zero;
      this.player.animator.multiplier = 1f;
      this.player.look.disableZoom();
      this.player.look.sensitivity = 1f;
      PlayerUI.updateBinoculars(false);
    }

    private void onPerspectiveUpdated(EPlayerPerspective newPerspective)
    {
      if (!this.isZoomed || newPerspective != EPlayerPerspective.THIRD)
        return;
      this.stopZoom();
    }

    public override void equip()
    {
      this.player.animator.play("Equip", true);
      if (!this.channel.isOwner)
        return;
      this.player.look.onPerspectiveUpdated += new PerspectiveUpdated(this.onPerspectiveUpdated);
    }

    public override void dequip()
    {
      if (!this.channel.isOwner)
        return;
      if (this.isZoomed)
        this.stopZoom();
      this.player.look.onPerspectiveUpdated -= new PerspectiveUpdated(this.onPerspectiveUpdated);
    }
  }
}
