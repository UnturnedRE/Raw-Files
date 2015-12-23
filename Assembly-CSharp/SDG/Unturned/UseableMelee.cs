// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.UseableMelee
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
  public class UseableMelee : Useable
  {
    private uint startedUse;
    private float startedSwing;
    private uint weakTime;
    private uint strongTime;
    private bool isUsing;
    private bool isSwinging;
    private ESwingMode swingMode;
    private ParticleSystem firstEmitter;
    private ParticleSystem thirdEmitter;
    private Transform firstLightHook;
    private Transform thirdLightHook;
    private bool interact;

    private bool isUseable
    {
      get
      {
        if (this.swingMode == ESwingMode.WEAK)
          return this.player.input.simulation - this.startedUse > this.weakTime;
        if (this.swingMode == ESwingMode.STRONG)
          return this.player.input.simulation - this.startedUse > this.strongTime;
        return false;
      }
    }

    private bool isDamageable
    {
      get
      {
        if (this.swingMode == ESwingMode.WEAK)
          return (double) (this.player.input.simulation - this.startedUse) > (double) this.weakTime * (double) ((ItemMeleeAsset) this.player.equipment.asset).weak;
        if (this.swingMode == ESwingMode.STRONG)
          return (double) (this.player.input.simulation - this.startedUse) > (double) this.strongTime * (double) ((ItemMeleeAsset) this.player.equipment.asset).strong;
        return false;
      }
    }

    private void swing()
    {
      this.startedUse = this.player.input.simulation;
      this.startedSwing = Time.realtimeSinceStartup;
      this.isUsing = true;
      this.isSwinging = true;
      if (this.swingMode == ESwingMode.WEAK)
      {
        this.player.animator.play("Weak", false);
      }
      else
      {
        if (this.swingMode != ESwingMode.STRONG)
          return;
        this.player.animator.play("Strong", false);
      }
    }

    private void startSwing()
    {
      this.startedUse = this.player.input.simulation;
      this.startedSwing = Time.realtimeSinceStartup;
      this.isUsing = true;
      this.isSwinging = true;
      this.player.animator.play("Start_Swing", false);
    }

    private void stopSwing()
    {
      this.isUsing = false;
      this.isSwinging = false;
      this.player.animator.play("Stop_Swing", false);
    }

    [SteamCall]
    public void askInteractMelee(CSteamID steamID)
    {
      if (!this.channel.checkOwner(steamID) || !Provider.isServer || (this.player.equipment.isBusy || !((ItemMeleeAsset) this.player.equipment.asset).isLight))
        return;
      this.interact = !this.interact;
      this.player.equipment.state[0] = !this.interact ? (byte) 0 : (byte) 1;
      this.player.equipment.sendUpdateState();
      EffectManager.sendEffect((ushort) 8, EffectManager.SMALL, this.transform.position);
    }

    [SteamCall]
    public void askSwingStart(CSteamID steamID)
    {
      if (!this.channel.checkServer(steamID) || !this.player.equipment.isEquipped)
        return;
      this.startSwing();
    }

    [SteamCall]
    public void askSwingStop(CSteamID steamID)
    {
      if (!this.channel.checkServer(steamID) || !this.player.equipment.isEquipped)
        return;
      this.stopSwing();
    }

    [SteamCall]
    public void askSwing(CSteamID steamID, byte mode)
    {
      if (!this.channel.checkServer(steamID) || !this.player.equipment.isEquipped)
        return;
      this.swingMode = (ESwingMode) mode;
      this.swing();
    }

    private void fire()
    {
      float num = (float) this.player.equipment.quality / 100f;
      if (Provider.isServer)
      {
        AlertTool.alert(this.transform.position, 8f);
        if (Provider.mode != EGameMode.EASY && (int) this.player.equipment.quality > 0 && (double) Random.value < (double) ((ItemWeaponAsset) this.player.equipment.asset).durability)
        {
          --this.player.equipment.quality;
          this.player.equipment.sendUpdateQuality();
        }
      }
      if (this.channel.isOwner)
      {
        RaycastInfo info = DamageTool.raycast(new Ray(this.player.look.aim.position, this.player.look.aim.forward), ((ItemWeaponAsset) this.player.equipment.asset).range, RayMasks.DAMAGE_CLIENT);
        if ((Object) info.player != (Object) null && (double) ((ItemWeaponAsset) this.player.equipment.asset).playerDamageMultiplier.damage > 1.0 && (this.channel.owner.playerID.group == CSteamID.Nil || info.player.channel.owner.playerID.group != this.channel.owner.playerID.group) && Provider.isPvP)
          PlayerUI.hitmark(info.limb != ELimb.SKULL ? EPlayerHit.ENTITIY : EPlayerHit.CRITICAL);
        else if ((Object) info.zombie != (Object) null && (double) ((ItemWeaponAsset) this.player.equipment.asset).zombieDamageMultiplier.damage > 1.0 || (Object) info.animal != (Object) null && (double) ((ItemWeaponAsset) this.player.equipment.asset).animalDamageMultiplier.damage > 1.0)
          PlayerUI.hitmark(info.limb != ELimb.SKULL ? EPlayerHit.ENTITIY : EPlayerHit.CRITICAL);
        else if ((Object) info.vehicle != (Object) null && (double) ((ItemWeaponAsset) this.player.equipment.asset).vehicleDamage > 1.0)
        {
          if (((ItemMeleeAsset) this.player.equipment.asset).isRepair)
          {
            if (!info.vehicle.isExploded && !info.vehicle.isRepaired)
              PlayerUI.hitmark(EPlayerHit.BUILD);
          }
          else if (!info.vehicle.isDead)
            PlayerUI.hitmark(EPlayerHit.BUILD);
        }
        else if ((Object) info.transform != (Object) null && info.transform.tag == "Barricade" && (double) ((ItemWeaponAsset) this.player.equipment.asset).barricadeDamage > 1.0)
        {
          ushort result;
          if (ushort.TryParse(!(info.transform.name == "Hinge") ? info.transform.name : info.transform.parent.parent.name, out result))
          {
            ItemBarricadeAsset itemBarricadeAsset = (ItemBarricadeAsset) Assets.find(EAssetType.ITEM, result);
            if (itemBarricadeAsset != null && itemBarricadeAsset.isVulnerable)
              PlayerUI.hitmark(EPlayerHit.BUILD);
          }
        }
        else if ((Object) info.transform != (Object) null && info.transform.tag == "Structure" && (double) ((ItemWeaponAsset) this.player.equipment.asset).structureDamage > 1.0)
        {
          ushort result;
          if (ushort.TryParse(info.transform.name, out result))
          {
            ItemStructureAsset itemStructureAsset = (ItemStructureAsset) Assets.find(EAssetType.ITEM, result);
            if (itemStructureAsset != null && itemStructureAsset.isVulnerable)
              PlayerUI.hitmark(EPlayerHit.BUILD);
          }
        }
        else if ((Object) info.transform != (Object) null && info.transform.tag == "Resource" && (double) ((ItemWeaponAsset) this.player.equipment.asset).resourceDamage > 1.0)
          PlayerUI.hitmark(EPlayerHit.BUILD);
        this.player.input.sendRaycast(info);
      }
      if (!Provider.isServer || this.player.input.inputs == null || this.player.input.inputs.Count == 0)
        return;
      InputInfo inputInfo = this.player.input.inputs.Dequeue();
      if (inputInfo == null || (double) (inputInfo.point - this.player.look.aim.position).sqrMagnitude > (double) Mathf.Pow(((ItemWeaponAsset) this.player.equipment.asset).range + 4f, 2f))
        return;
      if (!((ItemMeleeAsset) this.player.equipment.asset).isRepair)
        DamageTool.impact(inputInfo.point, inputInfo.normal, inputInfo.material, (Object) inputInfo.player != (Object) null || (Object) inputInfo.zombie != (Object) null || ((Object) inputInfo.animal != (Object) null || (Object) inputInfo.vehicle != (Object) null) || (Object) inputInfo.transform != (Object) null && (inputInfo.transform.tag == "Barricade" || inputInfo.transform.tag == "Structure" || inputInfo.transform.tag == "Resource" || inputInfo.transform.tag == "Debris"));
      EPlayerKill kill = EPlayerKill.NONE;
      float times1 = (float) ((double) (1f * (float) (1.0 + (double) this.channel.owner.player.skills.mastery(0, 0) * 0.5)) * (this.swingMode != ESwingMode.STRONG ? 1.0 : (double) ((ItemMeleeAsset) this.player.equipment.asset).strength) * ((double) num >= 0.5 ? 1.0 : 0.5 + (double) num));
      if ((Object) inputInfo.player != (Object) null)
      {
        if ((this.channel.owner.playerID.group == CSteamID.Nil || inputInfo.player.channel.owner.playerID.group != this.channel.owner.playerID.group) && Provider.isPvP)
          DamageTool.damage(inputInfo.player, EDeathCause.MELEE, inputInfo.limb, this.channel.owner.playerID.steamID, inputInfo.direction, ((ItemWeaponAsset) this.player.equipment.asset).playerDamageMultiplier, times1, true, out kill);
      }
      else if ((Object) inputInfo.zombie != (Object) null)
      {
        DamageTool.damage(inputInfo.zombie, inputInfo.limb, inputInfo.direction, ((ItemWeaponAsset) this.player.equipment.asset).zombieDamageMultiplier, times1, true, out kill);
        if ((int) this.player.movement.nav != (int) byte.MaxValue)
          inputInfo.zombie.alert(this.transform.position);
      }
      else if ((Object) inputInfo.animal != (Object) null)
      {
        DamageTool.damage(inputInfo.animal, inputInfo.limb, inputInfo.direction, ((ItemWeaponAsset) this.player.equipment.asset).animalDamageMultiplier, times1, out kill);
        inputInfo.animal.alert(this.transform.position);
      }
      else if ((Object) inputInfo.vehicle != (Object) null)
      {
        if (((ItemMeleeAsset) this.player.equipment.asset).isRepair)
          times1 *= (float) (1.0 + (double) this.channel.owner.player.skills.mastery(2, 6) * 2.0);
        DamageTool.damage(inputInfo.vehicle, ((ItemMeleeAsset) this.player.equipment.asset).isRepair, ((ItemWeaponAsset) this.player.equipment.asset).vehicleDamage, times1, true, out kill);
      }
      else if ((Object) inputInfo.transform != (Object) null)
      {
        if (inputInfo.transform.tag == "Barricade")
        {
          ushort result;
          if (ushort.TryParse(inputInfo.transform.name, out result))
          {
            ItemBarricadeAsset itemBarricadeAsset = (ItemBarricadeAsset) Assets.find(EAssetType.ITEM, result);
            if (itemBarricadeAsset != null && itemBarricadeAsset.isVulnerable)
              DamageTool.damage(inputInfo.transform, ((ItemWeaponAsset) this.player.equipment.asset).barricadeDamage, times1, out kill);
          }
        }
        else if (inputInfo.transform.tag == "Structure")
        {
          ushort result;
          if (ushort.TryParse(inputInfo.transform.name, out result))
          {
            ItemStructureAsset itemStructureAsset = (ItemStructureAsset) Assets.find(EAssetType.ITEM, result);
            if (itemStructureAsset != null && itemStructureAsset.isVulnerable)
              DamageTool.damage(inputInfo.transform, inputInfo.direction, ((ItemWeaponAsset) this.player.equipment.asset).structureDamage, times1, out kill);
          }
        }
        else if (inputInfo.transform.tag == "Resource")
        {
          float times2 = times1 * (float) (1.0 + (double) this.channel.owner.player.skills.mastery(2, 2) * 0.5);
          DamageTool.damage(inputInfo.transform, inputInfo.direction, ((ItemWeaponAsset) this.player.equipment.asset).resourceDamage, times2, (float) (1.0 + (double) this.channel.owner.player.skills.mastery(2, 2) * 0.5), out kill);
        }
      }
      if (Level.info.type == ELevelType.HORDE)
      {
        if ((Object) inputInfo.zombie != (Object) null)
        {
          if (inputInfo.limb == ELimb.SKULL)
            this.player.skills.askAward(10U);
          else
            this.player.skills.askAward(5U);
        }
        if (kill != EPlayerKill.ZOMBIE)
          return;
        if (inputInfo.limb == ELimb.SKULL)
          this.player.skills.askAward(50U);
        else
          this.player.skills.askAward(25U);
      }
      else if (kill == EPlayerKill.PLAYER)
        this.player.sendStat(EPlayerStat.KILLS_PLAYERS);
      else if (kill == EPlayerKill.ZOMBIE)
      {
        this.player.sendStat(EPlayerStat.KILLS_ZOMBIES_NORMAL);
        this.player.skills.askAward(3U);
      }
      else if (kill == EPlayerKill.MEGA)
      {
        this.player.sendStat(EPlayerStat.KILLS_ZOMBIES_MEGA);
        this.player.skills.askAward(48U);
      }
      else if (kill == EPlayerKill.ANIMAL)
      {
        this.player.sendStat(EPlayerStat.KILLS_ANIMALS);
        this.player.skills.askAward(12U);
      }
      else
      {
        if (kill != EPlayerKill.RESOURCE)
          return;
        this.player.sendStat(EPlayerStat.FOUND_RESOURCES);
        this.player.skills.askAward(2U);
      }
    }

    public override void startPrimary()
    {
      if (this.player.equipment.isBusy)
        return;
      if (((ItemMeleeAsset) this.player.equipment.asset).isRepeated)
      {
        if (this.isSwinging)
          return;
        this.swingMode = ESwingMode.WEAK;
        this.startSwing();
        if (!Provider.isServer)
          return;
        this.channel.send("askSwingStart", ESteamCall.NOT_OWNER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER);
      }
      else
      {
        if (!this.isUseable)
          return;
        this.player.equipment.isBusy = true;
        this.startedUse = this.player.input.simulation;
        this.startedSwing = Time.realtimeSinceStartup;
        this.isUsing = true;
        this.swingMode = ESwingMode.WEAK;
        this.swing();
        if (!Provider.isServer)
          return;
        this.channel.send("askSwing", ESteamCall.NOT_OWNER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[1]
        {
          (object) 0
        });
      }
    }

    public override void stopPrimary()
    {
      if (this.player.equipment.isBusy || !((ItemMeleeAsset) this.player.equipment.asset).isRepeated || !this.isSwinging)
        return;
      this.stopSwing();
      if (!Provider.isServer)
        return;
      this.channel.send("askSwingStop", ESteamCall.NOT_OWNER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER);
    }

    public override void startSecondary()
    {
      if (this.player.equipment.isBusy || ((ItemMeleeAsset) this.player.equipment.asset).isRepeated || (!this.isUseable || (double) this.player.life.stamina < (double) ((ItemMeleeAsset) this.player.equipment.asset).stamina * (1.0 - (double) this.player.skills.mastery(0, 4) * 0.75)))
        return;
      this.player.life.askTire((byte) ((double) ((ItemMeleeAsset) this.player.equipment.asset).stamina * (1.0 - (double) this.player.skills.mastery(0, 4) * 0.5)));
      this.player.equipment.isBusy = true;
      this.swingMode = ESwingMode.STRONG;
      this.swing();
      if (!Provider.isServer)
        return;
      this.channel.send("askSwing", ESteamCall.NOT_OWNER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[1]
      {
        (object) 1
      });
    }

    public override void equip()
    {
      this.player.animator.play("Equip", true);
      if (((ItemMeleeAsset) this.player.equipment.asset).isLight)
      {
        this.interact = (int) this.player.equipment.state[0] == 1;
        if (this.channel.isOwner)
        {
          this.firstLightHook = this.player.equipment.firstModel.FindChild("Model_0").FindChild("Light");
          this.firstLightHook.tag = "Viewmodel";
          this.firstLightHook.gameObject.layer = LayerMasks.VIEWMODEL;
          PlayerUI.message(EPlayerMessage.LIGHT, string.Empty);
        }
        this.thirdLightHook = this.player.equipment.thirdModel.FindChild("Model_0").FindChild("Light");
      }
      else
      {
        this.firstLightHook = (Transform) null;
        this.thirdLightHook = (Transform) null;
      }
      this.updateAttachments();
      if (((ItemMeleeAsset) this.player.equipment.asset).isRepeated)
      {
        if (this.channel.isOwner && (Object) this.player.equipment.firstModel.FindChild("Hit") != (Object) null)
        {
          this.firstEmitter = this.player.equipment.firstModel.FindChild("Hit").GetComponent<ParticleSystem>();
          this.firstEmitter.tag = "Viewmodel";
          this.firstEmitter.gameObject.layer = LayerMasks.VIEWMODEL;
        }
        if ((Object) this.player.equipment.thirdModel.FindChild("Hit") != (Object) null)
          this.thirdEmitter = this.player.equipment.thirdModel.FindChild("Hit").GetComponent<ParticleSystem>();
        this.weakTime = (uint) ((double) this.player.animator.getAnimationLength("Start_Swing") / (double) PlayerInput.RATE);
        this.strongTime = (uint) ((double) this.player.animator.getAnimationLength("Stop_Swing") / (double) PlayerInput.RATE);
      }
      else
      {
        this.weakTime = (uint) ((double) this.player.animator.getAnimationLength("Weak") / (double) PlayerInput.RATE);
        this.strongTime = (uint) ((double) this.player.animator.getAnimationLength("Strong") / (double) PlayerInput.RATE);
      }
    }

    public override void dequip()
    {
      if (!Dedicator.isDedicated)
        this.player.updateSpot(false);
      if (!this.channel.isOwner)
        return;
      this.player.animator.viewOffset = Vector3.zero;
    }

    public override void updateState(byte[] newState)
    {
      if (((ItemMeleeAsset) this.player.equipment.asset).isLight)
        this.interact = (int) newState[0] == 1;
      this.updateAttachments();
    }

    public override void tick()
    {
      if (!this.player.equipment.isEquipped)
        return;
      if (!Dedicator.isDedicated && this.isSwinging)
      {
        if (((ItemMeleeAsset) this.player.equipment.asset).isRepeated)
        {
          if ((double) Time.realtimeSinceStartup - (double) this.startedSwing > 0.1)
          {
            this.startedSwing = Time.realtimeSinceStartup;
            if (this.channel.isOwner && (Object) this.firstEmitter != (Object) null)
              this.firstEmitter.Emit(4);
            if ((Object) this.thirdEmitter != (Object) null)
              this.thirdEmitter.Emit(4);
            if (((ItemMeleeAsset) this.player.equipment.asset).isRepair)
              this.player.playSound(((ItemMeleeAsset) this.player.equipment.asset).use, 0.1f);
            else
              this.player.playSound(((ItemMeleeAsset) this.player.equipment.asset).use, 0.5f);
          }
        }
        else if (this.isDamageable)
        {
          if (this.swingMode == ESwingMode.WEAK)
            this.player.playSound(((ItemMeleeAsset) this.player.equipment.asset).use, 0.5f);
          else if (this.swingMode == ESwingMode.STRONG)
            this.player.playSound(((ItemMeleeAsset) this.player.equipment.asset).use, 0.5f, 0.7f, 0.1f);
          this.isSwinging = false;
        }
      }
      if (!this.channel.isOwner)
        return;
      if (this.isSwinging)
      {
        if (((ItemMeleeAsset) this.player.equipment.asset).isRepeated && !((ItemMeleeAsset) this.player.equipment.asset).isRepair)
          this.player.animator.viewOffset = new Vector3(Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f));
        else
          this.player.animator.viewOffset = Vector3.zero;
      }
      if (!Input.GetKeyDown(ControlsSettings.tactical) || !((ItemMeleeAsset) this.player.equipment.asset).isLight)
        return;
      this.channel.send("askInteractMelee", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER);
    }

    public override void simulate(uint simulation)
    {
      if (!this.isUsing || !this.isDamageable)
        return;
      if (((ItemMeleeAsset) this.player.equipment.asset).isRepeated)
      {
        this.startedUse = this.player.input.simulation;
      }
      else
      {
        this.player.equipment.isBusy = false;
        this.isUsing = false;
      }
      this.fire();
    }

    private void updateAttachments()
    {
      if (Dedicator.isDedicated || !((ItemMeleeAsset) this.player.equipment.asset).isLight)
        return;
      if (this.channel.isOwner && (Object) this.firstLightHook != (Object) null)
        this.firstLightHook.gameObject.SetActive(this.interact);
      if ((Object) this.thirdLightHook != (Object) null)
        this.thirdLightHook.gameObject.SetActive(this.interact);
      this.player.updateSpot(this.interact);
    }
  }
}
