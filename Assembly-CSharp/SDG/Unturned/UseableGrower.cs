// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.UseableGrower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
  public class UseableGrower : Useable
  {
    private float startedUse;
    private float useTime;
    private bool isUsing;

    private bool isUseable
    {
      get
      {
        return (double) Time.realtimeSinceStartup - (double) this.startedUse > (double) this.useTime;
      }
    }

    private void grow()
    {
      this.startedUse = Time.realtimeSinceStartup;
      this.isUsing = true;
      this.player.animator.play("Use", false);
      if (Dedicator.isDedicated)
        return;
      this.player.playSound(((ItemGrowerAsset) this.player.equipment.asset).use);
    }

    [SteamCall]
    public void askGrow(CSteamID steamID)
    {
      if (!this.channel.checkServer(steamID) || !this.player.equipment.isEquipped)
        return;
      this.grow();
    }

    private bool fire()
    {
      if (this.channel.isOwner)
      {
        RaycastInfo info = DamageTool.raycast(new Ray(this.player.look.aim.position, this.player.look.aim.forward), 3f, RayMasks.DAMAGE_CLIENT);
        if ((Object) info.transform == (Object) null || info.transform.tag != "Barricade" || (Object) info.transform.GetComponent<InteractableFarm>() == (Object) null)
          return false;
        this.player.input.sendRaycast(info);
      }
      if (Provider.isServer)
      {
        if (this.player.input.inputs == null || this.player.input.inputs.Count == 0)
          return false;
        InputInfo inputInfo = this.player.input.inputs.Dequeue();
        if (inputInfo == null || ((double) (inputInfo.point - this.player.look.aim.position).sqrMagnitude > 49.0 || (Object) inputInfo.transform == (Object) null || (Object) inputInfo.transform.GetComponent<InteractableFarm>() == (Object) null))
          return false;
        BarricadeManager.updateFarm(inputInfo.transform, 1U);
      }
      return true;
    }

    public override void startPrimary()
    {
      if (this.player.equipment.isBusy || !this.isUseable || !this.fire())
        return;
      this.player.equipment.isBusy = true;
      this.startedUse = Time.realtimeSinceStartup;
      this.isUsing = true;
      this.grow();
      if (!Provider.isServer)
        return;
      this.channel.send("askGrow", ESteamCall.NOT_OWNER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER);
    }

    public override void equip()
    {
      this.player.animator.play("Equip", true);
      this.useTime = this.player.animator.getAnimationLength("Use");
    }

    public override void simulate(uint simulation)
    {
      if (!this.isUsing || !this.isUseable)
        return;
      this.player.equipment.isBusy = false;
      this.isUsing = false;
      if (!Provider.isServer)
        return;
      this.player.equipment.use();
    }
  }
}
