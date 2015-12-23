// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.UseableFuel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
  public class UseableFuel : Useable
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

    private bool isFull
    {
      get
      {
        if (this.player.equipment.state != null && this.player.equipment.state.Length > 0)
          return (int) this.player.equipment.state[0] != 0;
        return false;
      }
    }

    private void glug()
    {
      this.startedUse = Time.realtimeSinceStartup;
      this.isUsing = true;
      this.player.animator.play("Use", false);
      if (Dedicator.isDedicated)
        return;
      this.player.playSound(((ItemFuelAsset) this.player.equipment.asset).use);
    }

    [SteamCall]
    public void askGlug(CSteamID steamID)
    {
      if (!this.channel.checkServer(steamID) || !this.player.equipment.isEquipped)
        return;
      this.glug();
    }

    private bool fire()
    {
      Ray ray = new Ray(this.player.look.aim.position, this.player.look.aim.forward);
      if (this.isFull)
      {
        if (this.channel.isOwner)
        {
          RaycastInfo info = DamageTool.raycast(ray, 3f, RayMasks.DAMAGE_CLIENT);
          if ((Object) info.transform == (Object) null)
            return false;
          InteractableGenerator component = info.transform.GetComponent<InteractableGenerator>();
          if (((Object) info.vehicle == (Object) null || !info.vehicle.isRefillable) && ((Object) component == (Object) null || !component.isRefillable))
            return false;
          this.player.input.sendRaycast(info);
        }
        if (Provider.isServer)
        {
          if (this.player.input.inputs == null || this.player.input.inputs.Count == 0)
            return false;
          InputInfo inputInfo = this.player.input.inputs.Dequeue();
          if (inputInfo == null || ((double) (inputInfo.point - this.player.look.aim.position).sqrMagnitude > 49.0 || (Object) inputInfo.transform == (Object) null))
            return false;
          float num = (int) this.player.equipment.quality >= 50 ? 1f : (float) (1.0 - (1.0 - (double) this.player.equipment.quality / 50.0) * 0.5);
          if ((Object) inputInfo.vehicle != (Object) null)
          {
            if (!inputInfo.vehicle.isRefillable)
              return false;
            inputInfo.vehicle.askFill((ushort) ((double) ((ItemFuelAsset) this.player.equipment.asset).fuel * (double) num));
          }
          else
          {
            InteractableGenerator component = inputInfo.transform.GetComponent<InteractableGenerator>();
            if ((Object) component == (Object) null || !component.isRefillable)
              return false;
            component.askFill((ushort) ((double) ((ItemFuelAsset) this.player.equipment.asset).fuel * (double) num));
            BarricadeManager.sendFuel(inputInfo.transform, component.fuel);
          }
        }
        return true;
      }
      RaycastHit hitInfo;
      Physics.Raycast(ray, out hitInfo, 3f, RayMasks.DAMAGE_PHYSICS);
      if ((Object) hitInfo.transform != (Object) null && (hitInfo.transform.tag == "Large" || hitInfo.transform.tag == "Medium"))
      {
        ushort id = LevelObjects.getID(hitInfo.transform);
        if ((int) id != 0)
        {
          ObjectAsset objectAsset = (ObjectAsset) Assets.find(EAssetType.OBJECT, id);
          if (objectAsset != null && objectAsset.isFuel)
          {
            if (Provider.isServer && Provider.mode != EGameMode.EASY && (int) this.player.equipment.quality > 0)
            {
              if ((int) this.player.equipment.quality > (int) ((ItemFuelAsset) this.player.equipment.asset).durability)
                this.player.equipment.quality -= ((ItemFuelAsset) this.player.equipment.asset).durability;
              else
                this.player.equipment.quality = (byte) 0;
              this.player.equipment.sendUpdateQuality();
            }
            return true;
          }
        }
      }
      return false;
    }

    public override void startPrimary()
    {
      if (this.player.equipment.isBusy || !this.isUseable || !this.fire())
        return;
      if (this.isFull)
        this.player.equipment.state[0] = (byte) 0;
      else
        this.player.equipment.state[0] = (byte) 1;
      this.player.equipment.updateState();
      this.player.equipment.isBusy = true;
      this.startedUse = Time.realtimeSinceStartup;
      this.isUsing = true;
      this.glug();
      if (Provider.isServer)
        this.channel.send("askGlug", ESteamCall.NOT_OWNER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER);
      if (!this.channel.isOwner)
        return;
      PlayerUI.message(!this.isFull ? EPlayerMessage.EMPTY : EPlayerMessage.FULL, string.Empty);
    }

    public override void equip()
    {
      this.player.animator.play("Equip", true);
      this.useTime = this.player.animator.getAnimationLength("Use");
      if (!this.channel.isOwner)
        return;
      PlayerUI.message(!this.isFull ? EPlayerMessage.EMPTY : EPlayerMessage.FULL, string.Empty);
    }

    public override void simulate(uint simulation)
    {
      if (!this.isUsing || !this.isUseable)
        return;
      this.player.equipment.isBusy = false;
      this.isUsing = false;
    }
  }
}
