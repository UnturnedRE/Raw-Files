// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.UseableConsumeable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
  public class UseableConsumeable : Useable
  {
    private float startedUse;
    private float useTime;
    private float aidTime;
    private bool isUsing;
    private EConsumeMode consumeMode;
    private Player enemy;
    private bool hasAid;

    private bool isUseable
    {
      get
      {
        if (this.consumeMode == EConsumeMode.USE)
          return (double) Time.realtimeSinceStartup - (double) this.startedUse > (double) this.useTime;
        if (this.consumeMode == EConsumeMode.AID)
          return (double) Time.realtimeSinceStartup - (double) this.startedUse > (double) this.aidTime;
        return false;
      }
    }

    private void consume()
    {
      if (this.consumeMode == EConsumeMode.USE)
        this.player.animator.play("Use", false);
      else if (this.consumeMode == EConsumeMode.AID && this.hasAid)
        this.player.animator.play("Aid", false);
      if (Dedicator.isDedicated)
        return;
      this.player.playSound(((ItemConsumeableAsset) this.player.equipment.asset).use);
    }

    [SteamCall]
    public void askConsume(CSteamID steamID, byte mode)
    {
      if (!this.channel.checkServer(steamID) || !this.player.equipment.isEquipped)
        return;
      this.consumeMode = (EConsumeMode) mode;
      this.consume();
    }

    public override void startPrimary()
    {
      if (this.player.equipment.isBusy)
        return;
      this.player.equipment.isBusy = true;
      this.startedUse = Time.realtimeSinceStartup;
      this.isUsing = true;
      this.consumeMode = EConsumeMode.USE;
      this.consume();
      if (!Provider.isServer)
        return;
      this.channel.send("askConsume", ESteamCall.NOT_OWNER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[1]
      {
        (object) (byte) this.consumeMode
      });
    }

    public override void startSecondary()
    {
      if (this.player.equipment.isBusy || !this.hasAid)
        return;
      if (this.channel.isOwner)
      {
        RaycastInfo info = DamageTool.raycast(new Ray(this.player.look.aim.position, this.player.look.aim.forward), 3f, RayMasks.DAMAGE_CLIENT);
        this.player.input.sendRaycast(info);
        if (!Provider.isServer && (Object) info.player != (Object) null)
        {
          this.player.equipment.isBusy = true;
          this.startedUse = Time.realtimeSinceStartup;
          this.isUsing = true;
          this.consumeMode = EConsumeMode.AID;
          this.consume();
        }
      }
      if (!Provider.isServer || this.player.input.inputs == null || this.player.input.inputs.Count == 0)
        return;
      InputInfo inputInfo = this.player.input.inputs.Dequeue();
      if (inputInfo == null || !((Object) inputInfo.player != (Object) null))
        return;
      this.enemy = inputInfo.player;
      this.player.equipment.isBusy = true;
      this.startedUse = Time.realtimeSinceStartup;
      this.isUsing = true;
      this.consumeMode = EConsumeMode.AID;
      this.consume();
      this.channel.send("askConsume", ESteamCall.NOT_OWNER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[1]
      {
        (object) (byte) this.consumeMode
      });
    }

    public override void equip()
    {
      this.player.animator.play("Equip", true);
      this.hasAid = ((ItemConsumeableAsset) this.player.equipment.asset).hasAid;
      this.useTime = this.player.animator.getAnimationLength("Use");
      if (!this.hasAid)
        return;
      this.aidTime = this.player.animator.getAnimationLength("Aid");
    }

    public override void simulate(uint simulation)
    {
      if (!this.isUsing || !this.isUseable)
        return;
      this.player.equipment.isBusy = false;
      this.isUsing = false;
      ItemConsumeableAsset consumeableAsset = (ItemConsumeableAsset) this.player.equipment.asset;
      if (this.consumeMode == EConsumeMode.AID)
      {
        if (!Provider.isServer)
          return;
        if (consumeableAsset != null && (Object) this.enemy != (Object) null)
        {
          this.enemy.life.askHeal((byte) ((double) consumeableAsset.health * (1.0 + (double) this.player.skills.mastery(2, 0) * 0.5)), consumeableAsset.hasBleeding, consumeableAsset.hasBroken);
          this.enemy.life.askEat((byte) ((double) consumeableAsset.food * ((double) this.player.equipment.quality / 100.0)));
          this.enemy.life.askDrink((byte) ((double) consumeableAsset.water * ((double) this.player.equipment.quality / 100.0)));
          this.enemy.life.askInfect((byte) ((double) consumeableAsset.virus * (1.0 - (double) this.enemy.skills.mastery(1, 2) * 0.5)));
          this.enemy.life.askDisinfect((byte) ((double) consumeableAsset.disinfectant * (1.0 + (double) this.enemy.skills.mastery(2, 0) * 0.5)));
          if ((int) this.player.equipment.quality < 50)
            this.enemy.life.askInfect((byte) ((double) ((int) consumeableAsset.food + (int) consumeableAsset.water) * 0.5 * (1.0 - (double) this.player.equipment.quality / 50.0) * (1.0 - (double) this.enemy.skills.mastery(1, 2) * 0.5)));
        }
        this.player.equipment.use();
      }
      else
      {
        if (consumeableAsset != null)
        {
          this.player.life.askRest(consumeableAsset.energy);
          this.player.life.askView((byte) ((double) consumeableAsset.vision * (1.0 - (double) this.player.skills.mastery(1, 2))));
          this.player.life.askWarm(consumeableAsset.warmth);
          bool has;
          if (this.channel.isOwner && (int) consumeableAsset.vision > 0 && (Provider.provider.achievementsService.getAchievement("Berries", out has) && !has))
            Provider.provider.achievementsService.setAchievement("Berries");
        }
        if (!Provider.isServer)
          return;
        if (consumeableAsset != null)
        {
          this.player.life.askHeal((byte) ((double) consumeableAsset.health * (1.0 + (double) this.player.skills.mastery(2, 0) * 0.5)), consumeableAsset.hasBleeding, consumeableAsset.hasBroken);
          this.player.life.askEat((byte) ((double) consumeableAsset.food * ((double) this.player.equipment.quality / 100.0)));
          this.player.life.askDrink((byte) ((double) consumeableAsset.water * ((double) this.player.equipment.quality / 100.0)));
          this.player.life.askInfect((byte) ((double) consumeableAsset.virus * (1.0 - (double) this.player.skills.mastery(1, 2) * 0.5)));
          this.player.life.askDisinfect((byte) ((double) consumeableAsset.disinfectant * (1.0 + (double) this.player.skills.mastery(2, 0) * 0.5)));
          this.player.life.askWarm(consumeableAsset.warmth);
          if ((int) this.player.equipment.quality < 50)
            this.player.life.askInfect((byte) ((double) ((int) consumeableAsset.food + (int) consumeableAsset.water) * 0.5 * (1.0 - (double) this.player.equipment.quality / 50.0) * (1.0 - (double) this.player.skills.mastery(1, 2) * 0.5)));
        }
        this.player.equipment.use();
      }
    }
  }
}
