// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.UseableRefill
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
  public class UseableRefill : Useable
  {
    private float startedUse;
    private float useTime;
    private float refillTime;
    private bool isUsing;
    private ERefillMode refillMode;

    private bool isUseable
    {
      get
      {
        if (this.refillMode == ERefillMode.USE)
          return (double) Time.realtimeSinceStartup - (double) this.startedUse > (double) this.useTime;
        if (this.refillMode == ERefillMode.REFILL)
          return (double) Time.realtimeSinceStartup - (double) this.startedUse > (double) this.refillTime;
        return false;
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

    private void use()
    {
      this.startedUse = Time.realtimeSinceStartup;
      this.isUsing = true;
      this.player.animator.play("Use", false);
      if (Dedicator.isDedicated)
        return;
      this.player.playSound(((ItemRefillAsset) this.player.equipment.asset).use);
    }

    [SteamCall]
    public void askUse(CSteamID steamID)
    {
      if (!this.channel.checkServer(steamID) || !this.player.equipment.isEquipped)
        return;
      this.use();
    }

    private void refill()
    {
      this.startedUse = Time.realtimeSinceStartup;
      this.isUsing = true;
      this.player.animator.play("Refill", false);
    }

    [SteamCall]
    public void askRefill(CSteamID steamID)
    {
      if (!this.channel.checkServer(steamID) || !this.player.equipment.isEquipped)
        return;
      this.refill();
    }

    private bool fire()
    {
      RaycastHit hitInfo;
      Physics.Raycast(new Ray(this.player.look.aim.position, this.player.look.aim.forward), out hitInfo, 3f, RayMasks.DAMAGE_PHYSICS);
      if ((Object) hitInfo.transform != (Object) null)
      {
        if ((double) hitInfo.point.y < (double) LevelLighting.seaLevel * (double) Level.TERRAIN)
          return true;
        if (hitInfo.transform.tag == "Large" || hitInfo.transform.tag == "Medium")
        {
          ushort id = LevelObjects.getID(hitInfo.transform);
          if ((int) id != 0)
          {
            ObjectAsset objectAsset = (ObjectAsset) Assets.find(EAssetType.OBJECT, id);
            if (objectAsset != null && objectAsset.isRefill)
              return true;
          }
        }
      }
      return false;
    }

    public override void startPrimary()
    {
      if (this.player.equipment.isBusy || !this.isUseable)
        return;
      if (this.isFull)
      {
        this.player.equipment.isBusy = true;
        this.startedUse = Time.realtimeSinceStartup;
        this.isUsing = true;
        this.refillMode = ERefillMode.USE;
        this.use();
        this.player.equipment.state[0] = (byte) 0;
        this.player.equipment.updateState();
        if (Provider.isServer)
          this.channel.send("askUse", ESteamCall.NOT_OWNER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER);
        if (!this.channel.isOwner)
          return;
        PlayerUI.message(!this.isFull ? EPlayerMessage.EMPTY : EPlayerMessage.FULL, string.Empty);
      }
      else
      {
        if (!this.fire())
          return;
        this.player.equipment.isBusy = true;
        this.startedUse = Time.realtimeSinceStartup;
        this.isUsing = true;
        this.refillMode = ERefillMode.REFILL;
        this.refill();
        this.player.equipment.state[0] = (byte) 1;
        this.player.equipment.updateState();
        if (Provider.isServer)
        {
          if (Provider.mode != EGameMode.EASY && (int) this.player.equipment.quality > 0)
          {
            if ((int) this.player.equipment.quality > (int) ((ItemRefillAsset) this.player.equipment.asset).durability)
              this.player.equipment.quality -= ((ItemRefillAsset) this.player.equipment.asset).durability;
            else
              this.player.equipment.quality = (byte) 0;
            this.player.equipment.sendUpdateQuality();
          }
          this.channel.send("askRefill", ESteamCall.NOT_OWNER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER);
        }
        if (!this.channel.isOwner)
          return;
        PlayerUI.message(!this.isFull ? EPlayerMessage.EMPTY : EPlayerMessage.FULL, string.Empty);
      }
    }

    public override void equip()
    {
      this.player.animator.play("Equip", true);
      this.useTime = this.player.animator.getAnimationLength("Use");
      this.refillTime = this.player.animator.getAnimationLength("Refill");
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
      if (this.refillMode != ERefillMode.USE || !Provider.isServer)
        return;
      this.player.life.askDrink((byte) ((double) ((ItemRefillAsset) this.player.equipment.asset).water * ((int) this.player.equipment.quality >= 50 ? 1.0 : 1.0 - (1.0 - (double) this.player.equipment.quality / 50.0) * 0.5)));
    }
  }
}
