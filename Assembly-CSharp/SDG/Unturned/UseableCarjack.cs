// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.UseableCarjack
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
  public class UseableCarjack : Useable
  {
    private float startedUse;
    private float useTime;
    private bool isUsing;
    private bool isJacking;
    private InteractableVehicle vehicle;

    private bool isUseable
    {
      get
      {
        return (double) Time.realtimeSinceStartup - (double) this.startedUse > (double) this.useTime;
      }
    }

    private bool isJackable
    {
      get
      {
        return (double) Time.realtimeSinceStartup - (double) this.startedUse > (double) this.useTime * 0.75;
      }
    }

    private void pull()
    {
      this.startedUse = Time.realtimeSinceStartup;
      this.isUsing = true;
      this.player.animator.play("Use", false);
      if (Dedicator.isDedicated)
        return;
      this.player.playSound(((ItemToolAsset) this.player.equipment.asset).use);
    }

    [SteamCall]
    public void askPull(CSteamID steamID)
    {
      if (!this.channel.checkServer(steamID) || !this.player.equipment.isEquipped)
        return;
      this.pull();
    }

    private bool fire()
    {
      if (this.channel.isOwner)
      {
        RaycastInfo info = DamageTool.raycast(new Ray(this.player.look.aim.position, this.player.look.aim.forward), 3f, RayMasks.DAMAGE_CLIENT);
        if ((Object) info.vehicle == (Object) null || !info.vehicle.isEmpty)
          return false;
        this.player.input.sendRaycast(info);
      }
      if (Provider.isServer)
      {
        if (this.player.input.inputs == null || this.player.input.inputs.Count == 0)
          return false;
        InputInfo inputInfo = this.player.input.inputs.Dequeue();
        if (inputInfo == null || ((double) (inputInfo.point - this.player.look.aim.position).sqrMagnitude > 49.0 || (Object) inputInfo.vehicle == (Object) null || !inputInfo.vehicle.isEmpty))
          return false;
        this.isJacking = true;
        this.vehicle = inputInfo.vehicle;
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
      this.pull();
      if (!Provider.isServer)
        return;
      this.channel.send("askPull", ESteamCall.NOT_OWNER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER);
    }

    public override void equip()
    {
      this.player.animator.play("Equip", true);
      this.useTime = this.player.animator.getAnimationLength("Use");
    }

    public override void simulate(uint simulation)
    {
      if (this.isJacking && this.isJackable)
      {
        this.isJacking = false;
        if ((Object) this.vehicle != (Object) null && this.vehicle.isEmpty)
        {
          this.vehicle.GetComponent<Rigidbody>().AddForce(Random.Range(-32f, 32f), Random.Range(480f, 544f) * (this.player.skills.boost != EPlayerBoost.FLIGHT ? 1f : 4f), Random.Range(-32f, 32f));
          this.vehicle.GetComponent<Rigidbody>().AddTorque(Random.Range(-64f, 64f), Random.Range(-64f, 64f), Random.Range(-64f, 64f));
          this.vehicle = (InteractableVehicle) null;
        }
      }
      if (!this.isUsing || !this.isUseable)
        return;
      this.player.equipment.isBusy = false;
      this.isUsing = false;
    }
  }
}
