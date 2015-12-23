// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.UseableGun
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
  public class UseableGun : Useable
  {
    private static readonly float RECOIL_CROUCH = 0.85f;
    private static readonly float RECOIL_PRONE = 0.7f;
    private static readonly float SPREAD_SPRINT = 1.25f;
    private static readonly float SPREAD_CROUCH = 0.85f;
    private static readonly float SPREAD_PRONE = 0.7f;
    private static readonly float SHAKE_CROUCH = 0.85f;
    private static readonly float SHAKE_PRONE = 0.7f;
    private Local localization;
    private Bundle icons;
    private SleekButtonIcon sightButton;
    private SleekJars sightJars;
    private SleekButtonIcon tacticalButton;
    private SleekJars tacticalJars;
    private SleekButtonIcon gripButton;
    private SleekJars gripJars;
    private SleekButtonIcon barrelButton;
    private SleekJars barrelJars;
    private SleekButtonIcon magazineButton;
    private SleekJars magazineJars;
    private SleekLabel rangeLabel;
    private SleekBox infoBox;
    private SleekLabel ammoLabel;
    private SleekLabel firemodeLabel;
    private SleekLabel attachLabel;
    private Attachments firstAttachments;
    private ParticleSystem firstShellEmitter;
    private ParticleSystem firstMuzzleEmitter;
    private Attachments thirdAttachments;
    private ParticleSystem thirdShellEmitter;
    private ParticleSystem thirdMuzzleEmitter;
    private AudioSource sound;
    private Vector3 sightOffset;
    private Vector3 scopeOffset;
    private bool isShooting;
    private bool isSprinting;
    private bool isReloading;
    private bool isHammering;
    private bool isAttaching;
    private float lastShot;
    private uint lastFire;
    private bool isFired;
    private float startedReload;
    private float startedHammer;
    private float reloadTime;
    private float hammerTime;
    private bool needsHammer;
    private bool needsRechamber;
    private bool needsEject;
    private bool needsUnload;
    private bool needsReplace;
    private bool interact;
    private byte ammo;
    private EFiremode firemode;
    private List<InventorySearch> sightSearch;
    private List<InventorySearch> tacticalSearch;
    private List<InventorySearch> gripSearch;
    private List<InventorySearch> barrelSearch;
    private List<InventorySearch> magazineSearch;
    private float zoom;
    private float crosshair;
    private Transform laser;
    private bool wasLaser;
    private bool wasLight;
    private bool wasRange;
    private bool inRange;
    private RaycastHit contact;

    public bool isAiming { get; protected set; }

    public override bool canInspect
    {
      get
      {
        if (!this.isShooting && !this.isReloading && (!this.isHammering && !this.isSprinting) && !this.isAttaching)
          return !this.isAiming;
        return false;
      }
    }

    [SteamCall]
    public void askFiremode(CSteamID steamID, byte id)
    {
      if (!this.channel.checkOwner(steamID) || !Provider.isServer || (this.player.equipment.isBusy || this.isFired) || (this.isReloading || this.isHammering))
        return;
      EFiremode efiremode = (EFiremode) id;
      switch (efiremode)
      {
        case EFiremode.SAFETY:
          if (((ItemGunAsset) this.player.equipment.asset).hasSafety)
          {
            this.firemode = efiremode;
            break;
          }
          break;
        case EFiremode.SEMI:
          if (((ItemGunAsset) this.player.equipment.asset).hasSemi)
          {
            this.firemode = efiremode;
            break;
          }
          break;
        case EFiremode.AUTO:
          if (((ItemGunAsset) this.player.equipment.asset).hasAuto)
          {
            this.firemode = efiremode;
            break;
          }
          break;
      }
      this.player.equipment.state[11] = (byte) this.firemode;
      this.player.equipment.sendUpdateState();
      EffectManager.sendEffect((ushort) 8, EffectManager.SMALL, this.transform.position);
    }

    [SteamCall]
    public void askInteractGun(CSteamID steamID)
    {
      if (!this.channel.checkOwner(steamID) || !Provider.isServer || (this.player.equipment.isBusy || this.isFired) || (this.isReloading || this.isHammering))
        return;
      this.interact = !this.interact;
      this.player.equipment.state[12] = !this.interact ? (byte) 0 : (byte) 1;
      this.player.equipment.sendUpdateState();
      EffectManager.sendEffect((ushort) 8, EffectManager.SMALL, this.transform.position);
    }

    private void project(Vector3 origin, Vector3 direction)
    {
      if ((UnityEngine.Object) this.sound != (UnityEngine.Object) null)
      {
        this.sound.pitch = UnityEngine.Random.Range(0.975f, 1.025f);
        this.sound.PlayOneShot(this.sound.clip);
      }
      Transform transform = UnityEngine.Object.Instantiate<GameObject>(((ItemGunAsset) this.player.equipment.asset).projectile).transform;
      transform.name = "Projectile";
      transform.parent = Level.effects;
      transform.position = origin + direction * 2f;
      transform.rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(90f, 0.0f, 0.0f);
      transform.GetComponent<Rigidbody>().AddForce(direction * 2000f);
      if (this.channel.isOwner)
        transform.GetComponent<AudioSource>().maxDistance = 256f;
      Rocket rocket = transform.gameObject.AddComponent<Rocket>();
      if (Provider.isServer)
      {
        rocket.playerDamage = ((ItemWeaponAsset) this.player.equipment.asset).playerDamageMultiplier.damage;
        rocket.zombieDamage = ((ItemWeaponAsset) this.player.equipment.asset).zombieDamageMultiplier.damage;
        rocket.animalDamage = ((ItemWeaponAsset) this.player.equipment.asset).animalDamageMultiplier.damage;
        rocket.barricadeDamage = ((ItemWeaponAsset) this.player.equipment.asset).barricadeDamage;
        rocket.structureDamage = ((ItemWeaponAsset) this.player.equipment.asset).structureDamage;
        rocket.vehicleDamage = ((ItemWeaponAsset) this.player.equipment.asset).vehicleDamage;
        rocket.resourceDamage = ((ItemWeaponAsset) this.player.equipment.asset).resourceDamage;
      }
      UnityEngine.Object.Destroy((UnityEngine.Object) transform.gameObject, 5f);
      this.lastShot = Time.realtimeSinceStartup;
    }

    [SteamCall]
    public void askProject(CSteamID steamID, Vector3 origin, Vector3 direction)
    {
      if (!this.channel.checkServer(steamID) || !this.player.equipment.isEquipped)
        return;
      this.project(origin, direction);
    }

    private void shoot()
    {
      if ((UnityEngine.Object) this.sound != (UnityEngine.Object) null)
      {
        this.sound.pitch = UnityEngine.Random.Range(0.975f, 1.025f);
        this.sound.PlayOneShot(this.sound.clip);
      }
      if (((ItemGunAsset) this.player.equipment.asset).action == EAction.Trigger)
      {
        if ((UnityEngine.Object) this.firstShellEmitter != (UnityEngine.Object) null && this.player.look.perspective == EPlayerPerspective.FIRST)
          this.firstShellEmitter.Emit(1);
        if ((UnityEngine.Object) this.thirdShellEmitter != (UnityEngine.Object) null && (!this.channel.isOwner || this.player.look.perspective == EPlayerPerspective.THIRD))
          this.thirdShellEmitter.Emit(1);
      }
      if ((UnityEngine.Object) this.thirdAttachments.barrelModel == (UnityEngine.Object) null || !this.thirdAttachments.barrelAsset.isBraked)
      {
        if ((UnityEngine.Object) this.firstMuzzleEmitter != (UnityEngine.Object) null && this.player.look.perspective == EPlayerPerspective.FIRST)
        {
          this.firstMuzzleEmitter.Emit(1);
          this.firstMuzzleEmitter.GetComponent<Light>().enabled = true;
        }
        if ((UnityEngine.Object) this.thirdMuzzleEmitter != (UnityEngine.Object) null && (!this.channel.isOwner || this.player.look.perspective == EPlayerPerspective.THIRD))
        {
          this.thirdMuzzleEmitter.Emit(1);
          this.thirdMuzzleEmitter.GetComponent<Light>().enabled = true;
        }
      }
      this.lastShot = Time.realtimeSinceStartup;
      if (((ItemGunAsset) this.player.equipment.asset).action != EAction.Bolt && ((ItemGunAsset) this.player.equipment.asset).action != EAction.Pump)
        return;
      this.needsRechamber = true;
    }

    [SteamCall]
    public void askShoot(CSteamID steamID)
    {
      if (!this.channel.checkServer(steamID) || !this.player.equipment.isEquipped)
        return;
      this.shoot();
    }

    private void fire()
    {
      float num1 = (float) this.player.equipment.quality / 100f;
      if (this.thirdAttachments.magazineAsset == null)
        return;
      --this.ammo;
      if (this.channel.isOwner && (int) this.ammo == 0)
        PlayerUI.message(EPlayerMessage.RELOAD, string.Empty);
      if (!this.isAiming)
        this.player.equipment.uninspect();
      if (Provider.isServer && ((ItemGunAsset) this.player.equipment.asset).action != EAction.String)
      {
        this.player.equipment.state[10] = this.ammo;
        this.player.equipment.updateState();
      }
      if (Provider.isServer)
      {
        this.channel.send("askShoot", ESteamCall.NOT_OWNER, this.transform.position, EffectManager.INSANE, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[0]);
        this.lastShot = Time.realtimeSinceStartup;
        if (((ItemGunAsset) this.player.equipment.asset).action == EAction.Bolt || ((ItemGunAsset) this.player.equipment.asset).action == EAction.Pump)
          this.needsRechamber = true;
        if (this.thirdAttachments.barrelAsset == null || !this.thirdAttachments.barrelAsset.isSilenced)
          AlertTool.alert(this.transform.position, 48f);
        if (Provider.mode != EGameMode.EASY && (int) this.player.equipment.quality > 0 && (double) UnityEngine.Random.value < (double) ((ItemWeaponAsset) this.player.equipment.asset).durability)
        {
          --this.player.equipment.quality;
          this.player.equipment.sendUpdateQuality();
        }
      }
      if (this.channel.isOwner)
      {
        float num2 = (float) ((!this.isAiming ? 1.0 : (double) ((ItemGunAsset) this.player.equipment.asset).spreadAim) * ((double) num1 >= 0.5 ? 1.0 : 1.0 + (1.0 - (double) num1 * 2.0))) * (float) (1.0 - (double) this.player.skills.mastery(0, 1) * 0.5);
        if (this.thirdAttachments.sightAsset != null && this.isAiming)
          num2 *= this.thirdAttachments.sightAsset.spread;
        if (this.thirdAttachments.tacticalAsset != null && this.interact)
          num2 *= this.thirdAttachments.tacticalAsset.spread;
        if (this.thirdAttachments.gripAsset != null && (!this.thirdAttachments.gripAsset.isBipod || this.player.stance.stance == EPlayerStance.PRONE))
          num2 *= this.thirdAttachments.gripAsset.spread;
        if (this.thirdAttachments.barrelAsset != null)
          num2 *= this.thirdAttachments.barrelAsset.spread;
        if (this.thirdAttachments.magazineAsset != null)
          num2 *= this.thirdAttachments.magazineAsset.spread;
        if (this.player.stance.stance == EPlayerStance.CROUCH)
          num2 *= UseableGun.SPREAD_CROUCH;
        else if (this.player.stance.stance == EPlayerStance.PRONE)
          num2 *= UseableGun.SPREAD_PRONE;
        EPlayerHit newHit = EPlayerHit.NONE;
        if (!this.player.look.isCam && this.player.look.perspective == EPlayerPerspective.THIRD)
        {
          RaycastHit hitInfo;
          Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, 512f, RayMasks.DAMAGE_CLIENT);
          if ((UnityEngine.Object) hitInfo.transform != (UnityEngine.Object) null)
            this.player.look.aim.rotation = Quaternion.LookRotation(hitInfo.point - this.player.look.aim.position);
        }
        if ((UnityEngine.Object) ((ItemGunAsset) this.player.equipment.asset).projectile == (UnityEngine.Object) null)
        {
          byte pellets = this.thirdAttachments.magazineAsset.pellets;
          for (byte index = (byte) 0; (int) index < (int) pellets; ++index)
          {
            Vector3 direction = this.player.look.aim.forward + this.player.look.aim.right * UnityEngine.Random.Range(-((ItemGunAsset) this.player.equipment.asset).spreadHip, ((ItemGunAsset) this.player.equipment.asset).spreadHip) * num2 + this.player.look.aim.up * UnityEngine.Random.Range(-((ItemGunAsset) this.player.equipment.asset).spreadHip, ((ItemGunAsset) this.player.equipment.asset).spreadHip) * num2;
            direction.Normalize();
            RaycastInfo info = DamageTool.raycast(new Ray(this.player.look.aim.position, direction), ((ItemWeaponAsset) this.player.equipment.asset).range, RayMasks.DAMAGE_CLIENT);
            if ((UnityEngine.Object) info.player != (UnityEngine.Object) null && (double) ((ItemWeaponAsset) this.player.equipment.asset).playerDamageMultiplier.damage > 1.0 && (this.channel.owner.playerID.group == CSteamID.Nil || info.player.channel.owner.playerID.group != this.channel.owner.playerID.group) && Provider.isPvP)
            {
              if (newHit != EPlayerHit.CRITICAL)
                newHit = info.limb != ELimb.SKULL ? EPlayerHit.ENTITIY : EPlayerHit.CRITICAL;
            }
            else if ((UnityEngine.Object) info.zombie != (UnityEngine.Object) null && (double) ((ItemWeaponAsset) this.player.equipment.asset).zombieDamageMultiplier.damage > 1.0 || (UnityEngine.Object) info.animal != (UnityEngine.Object) null && (double) ((ItemWeaponAsset) this.player.equipment.asset).animalDamageMultiplier.damage > 1.0)
            {
              if (newHit != EPlayerHit.CRITICAL)
                newHit = info.limb != ELimb.SKULL ? EPlayerHit.ENTITIY : EPlayerHit.CRITICAL;
            }
            else if ((UnityEngine.Object) info.transform != (UnityEngine.Object) null && info.transform.tag == "Barricade" && (double) ((ItemWeaponAsset) this.player.equipment.asset).barricadeDamage > 1.0)
            {
              ushort result;
              if (ushort.TryParse(!(info.transform.name == "Hinge") ? info.transform.name : info.transform.parent.parent.name, out result))
              {
                ItemBarricadeAsset itemBarricadeAsset = (ItemBarricadeAsset) Assets.find(EAssetType.ITEM, result);
                if (itemBarricadeAsset != null && itemBarricadeAsset.isVulnerable && newHit == EPlayerHit.NONE)
                  newHit = EPlayerHit.BUILD;
              }
            }
            else if ((UnityEngine.Object) info.transform != (UnityEngine.Object) null && info.transform.tag == "Structure" && (double) ((ItemWeaponAsset) this.player.equipment.asset).structureDamage > 1.0)
            {
              ushort result;
              if (ushort.TryParse(info.transform.name, out result))
              {
                ItemStructureAsset itemStructureAsset = (ItemStructureAsset) Assets.find(EAssetType.ITEM, result);
                if (itemStructureAsset != null && itemStructureAsset.isVulnerable && newHit == EPlayerHit.NONE)
                  newHit = EPlayerHit.BUILD;
              }
            }
            else if (((UnityEngine.Object) info.vehicle != (UnityEngine.Object) null && !info.vehicle.isDead && (double) ((ItemWeaponAsset) this.player.equipment.asset).vehicleDamage > 1.0 || (UnityEngine.Object) info.transform != (UnityEngine.Object) null && info.transform.tag == "Resource" && (double) ((ItemWeaponAsset) this.player.equipment.asset).resourceDamage > 1.0) && newHit == EPlayerHit.NONE)
              newHit = EPlayerHit.BUILD;
            this.player.input.sendRaycast(info);
          }
        }
        else
        {
          Vector3 direction = this.player.look.aim.forward + this.player.look.aim.right * UnityEngine.Random.Range(-((ItemGunAsset) this.player.equipment.asset).spreadHip, ((ItemGunAsset) this.player.equipment.asset).spreadHip) * num2 + this.player.look.aim.up * UnityEngine.Random.Range(-((ItemGunAsset) this.player.equipment.asset).spreadHip, ((ItemGunAsset) this.player.equipment.asset).spreadHip) * num2;
          direction.Normalize();
          RaycastInfo info = DamageTool.raycast(new Ray(this.player.look.aim.position, direction), 512f, RayMasks.DAMAGE_CLIENT);
          if ((UnityEngine.Object) info.transform != (UnityEngine.Object) null)
            this.player.input.sendRaycast(info);
          this.project(this.player.look.aim.position, direction);
        }
        if (newHit != EPlayerHit.NONE)
          PlayerUI.hitmark(newHit);
        float num3 = UnityEngine.Random.Range(((ItemGunAsset) this.player.equipment.asset).recoilMin_x, ((ItemGunAsset) this.player.equipment.asset).recoilMax_x) * ((double) num1 >= 0.5 ? 1f : (float) (1.0 + (1.0 - (double) num1 * 2.0)));
        float num4 = UnityEngine.Random.Range(((ItemGunAsset) this.player.equipment.asset).recoilMin_y, ((ItemGunAsset) this.player.equipment.asset).recoilMax_y) * ((double) num1 >= 0.5 ? 1f : (float) (1.0 + (1.0 - (double) num1 * 2.0)));
        float shake_x = UnityEngine.Random.Range(((ItemGunAsset) this.player.equipment.asset).shakeMin_x, ((ItemGunAsset) this.player.equipment.asset).shakeMax_x);
        float shake_y = UnityEngine.Random.Range(((ItemGunAsset) this.player.equipment.asset).shakeMin_y, ((ItemGunAsset) this.player.equipment.asset).shakeMax_y);
        float shake_z = UnityEngine.Random.Range(((ItemGunAsset) this.player.equipment.asset).shakeMin_z, ((ItemGunAsset) this.player.equipment.asset).shakeMax_z);
        float x = num3 * (float) (1.0 - (double) this.player.skills.mastery(0, 1) * 0.5);
        float y = num4 * (float) (1.0 - (double) this.player.skills.mastery(0, 1) * 0.5);
        if (this.thirdAttachments.sightAsset != null)
        {
          x *= this.thirdAttachments.sightAsset.recoil_x;
          y *= this.thirdAttachments.sightAsset.recoil_y;
          shake_x *= this.thirdAttachments.sightAsset.shake;
          shake_y *= this.thirdAttachments.sightAsset.shake;
          shake_z *= this.thirdAttachments.sightAsset.shake;
        }
        if (this.thirdAttachments.tacticalAsset != null && this.interact)
        {
          x *= this.thirdAttachments.tacticalAsset.recoil_x;
          y *= this.thirdAttachments.tacticalAsset.recoil_y;
          shake_x *= this.thirdAttachments.tacticalAsset.shake;
          shake_y *= this.thirdAttachments.tacticalAsset.shake;
          shake_z *= this.thirdAttachments.tacticalAsset.shake;
        }
        if (this.thirdAttachments.gripAsset != null && (!this.thirdAttachments.gripAsset.isBipod || this.player.stance.stance == EPlayerStance.PRONE))
        {
          x *= this.thirdAttachments.gripAsset.recoil_x;
          y *= this.thirdAttachments.gripAsset.recoil_y;
          shake_x *= this.thirdAttachments.gripAsset.shake;
          shake_y *= this.thirdAttachments.gripAsset.shake;
          shake_z *= this.thirdAttachments.gripAsset.shake;
        }
        if (this.thirdAttachments.barrelAsset != null)
        {
          x *= this.thirdAttachments.barrelAsset.recoil_x;
          y *= this.thirdAttachments.barrelAsset.recoil_y;
          shake_x *= this.thirdAttachments.barrelAsset.shake;
          shake_y *= this.thirdAttachments.barrelAsset.shake;
          shake_z *= this.thirdAttachments.barrelAsset.shake;
        }
        if (this.thirdAttachments.magazineAsset != null)
        {
          x *= this.thirdAttachments.magazineAsset.recoil_x;
          y *= this.thirdAttachments.magazineAsset.recoil_y;
          shake_x *= this.thirdAttachments.magazineAsset.shake;
          shake_y *= this.thirdAttachments.magazineAsset.shake;
          shake_z *= this.thirdAttachments.magazineAsset.shake;
        }
        if (this.player.stance.stance == EPlayerStance.CROUCH)
        {
          x *= UseableGun.RECOIL_CROUCH;
          y *= UseableGun.RECOIL_CROUCH;
          shake_x *= UseableGun.SHAKE_CROUCH;
          shake_y *= UseableGun.SHAKE_CROUCH;
          shake_z *= UseableGun.SHAKE_CROUCH;
        }
        else if (this.player.stance.stance == EPlayerStance.PRONE)
        {
          x *= UseableGun.RECOIL_PRONE;
          y *= UseableGun.RECOIL_PRONE;
          shake_x *= UseableGun.SHAKE_PRONE;
          shake_y *= UseableGun.SHAKE_PRONE;
          shake_z *= UseableGun.SHAKE_PRONE;
        }
        this.player.look.recoil(x, y, ((ItemGunAsset) this.player.equipment.asset).recover_x, ((ItemGunAsset) this.player.equipment.asset).recover_y);
        this.player.animator.shake(shake_x, shake_y, shake_z);
        this.updateInfo();
        if ((UnityEngine.Object) ((ItemGunAsset) this.player.equipment.asset).projectile == (UnityEngine.Object) null)
          this.shoot();
      }
      if (!Provider.isServer)
        return;
      if ((UnityEngine.Object) ((ItemGunAsset) this.player.equipment.asset).projectile == (UnityEngine.Object) null)
      {
        byte pellets = this.thirdAttachments.magazineAsset.pellets;
        for (byte index = (byte) 0; (int) index < (int) pellets && (this.player.input.inputs != null && this.player.input.inputs.Count != 0); ++index)
        {
          InputInfo inputInfo = this.player.input.inputs.Dequeue();
          if (inputInfo == null || (double) (inputInfo.point - this.player.look.aim.position).sqrMagnitude > (double) Mathf.Pow(((ItemWeaponAsset) this.player.equipment.asset).range + 4f, 2f))
            break;
          DamageTool.impact(inputInfo.point, inputInfo.normal, inputInfo.material, (UnityEngine.Object) inputInfo.player != (UnityEngine.Object) null || (UnityEngine.Object) inputInfo.zombie != (UnityEngine.Object) null || ((UnityEngine.Object) inputInfo.animal != (UnityEngine.Object) null || (UnityEngine.Object) inputInfo.vehicle != (UnityEngine.Object) null) || (UnityEngine.Object) inputInfo.transform != (UnityEngine.Object) null && (inputInfo.transform.tag == "Barricade" || inputInfo.transform.tag == "Structure" || inputInfo.transform.tag == "Resource" || inputInfo.transform.tag == "Debris"));
          EPlayerKill kill = EPlayerKill.NONE;
          float num2 = (float) (1.0 * ((double) num1 >= 0.5 ? 1.0 : 0.5 + (double) num1));
          if ((UnityEngine.Object) inputInfo.player != (UnityEngine.Object) null)
          {
            if ((this.channel.owner.playerID.group == CSteamID.Nil || inputInfo.player.channel.owner.playerID.group != this.channel.owner.playerID.group) && Provider.isPvP)
              DamageTool.damage(inputInfo.player, EDeathCause.GUN, inputInfo.limb, this.channel.owner.playerID.steamID, inputInfo.direction * Mathf.Ceil((float) this.thirdAttachments.magazineAsset.pellets / 2f), ((ItemWeaponAsset) this.player.equipment.asset).playerDamageMultiplier, 1f, true, out kill);
          }
          else if ((UnityEngine.Object) inputInfo.zombie != (UnityEngine.Object) null)
          {
            DamageTool.damage(inputInfo.zombie, inputInfo.limb, inputInfo.direction * Mathf.Ceil((float) this.thirdAttachments.magazineAsset.pellets / 2f), ((ItemWeaponAsset) this.player.equipment.asset).zombieDamageMultiplier, 1f, true, out kill);
            if ((int) this.player.movement.nav != (int) byte.MaxValue)
              inputInfo.zombie.alert(this.transform.position);
          }
          else if ((UnityEngine.Object) inputInfo.animal != (UnityEngine.Object) null)
          {
            DamageTool.damage(inputInfo.animal, inputInfo.limb, inputInfo.direction * Mathf.Ceil((float) this.thirdAttachments.magazineAsset.pellets / 2f), ((ItemWeaponAsset) this.player.equipment.asset).animalDamageMultiplier, 1f, out kill);
            inputInfo.animal.alert(this.transform.position);
          }
          else if ((UnityEngine.Object) inputInfo.vehicle != (UnityEngine.Object) null)
            DamageTool.damage(inputInfo.vehicle, false, ((ItemWeaponAsset) this.player.equipment.asset).vehicleDamage, 1f, true, out kill);
          else if ((UnityEngine.Object) inputInfo.transform != (UnityEngine.Object) null)
          {
            if (inputInfo.transform.tag == "Barricade")
            {
              ushort result;
              if (ushort.TryParse(inputInfo.transform.name, out result))
              {
                ItemBarricadeAsset itemBarricadeAsset = (ItemBarricadeAsset) Assets.find(EAssetType.ITEM, result);
                if (itemBarricadeAsset != null && itemBarricadeAsset.isVulnerable)
                  DamageTool.damage(inputInfo.transform, ((ItemWeaponAsset) this.player.equipment.asset).barricadeDamage, 1f, out kill);
              }
            }
            else if (inputInfo.transform.tag == "Structure")
            {
              ushort result;
              if (ushort.TryParse(inputInfo.transform.name, out result))
              {
                ItemStructureAsset itemStructureAsset = (ItemStructureAsset) Assets.find(EAssetType.ITEM, result);
                if (itemStructureAsset != null && itemStructureAsset.isVulnerable)
                  DamageTool.damage(inputInfo.transform, inputInfo.direction * Mathf.Ceil((float) this.thirdAttachments.magazineAsset.pellets / 2f), ((ItemWeaponAsset) this.player.equipment.asset).structureDamage, 1f, out kill);
              }
            }
            else if (inputInfo.transform.tag == "Resource")
              DamageTool.damage(inputInfo.transform, inputInfo.direction * Mathf.Ceil((float) this.thirdAttachments.magazineAsset.pellets / 2f), ((ItemWeaponAsset) this.player.equipment.asset).resourceDamage, 1f, 1f, out kill);
          }
          if (Level.info.type == ELevelType.HORDE)
          {
            if ((UnityEngine.Object) inputInfo.zombie != (UnityEngine.Object) null)
            {
              if (inputInfo.limb == ELimb.SKULL)
                this.player.skills.askAward(10U);
              else
                this.player.skills.askAward(5U);
            }
            if (kill == EPlayerKill.ZOMBIE)
            {
              if (inputInfo.limb == ELimb.SKULL)
                this.player.skills.askAward(50U);
              else
                this.player.skills.askAward(25U);
            }
          }
          else if (kill == EPlayerKill.PLAYER)
            this.player.sendStat(EPlayerStat.KILLS_PLAYERS);
          else if (kill == EPlayerKill.ZOMBIE)
          {
            this.player.sendStat(EPlayerStat.KILLS_ZOMBIES_NORMAL);
            this.player.skills.askAward(2U);
          }
          else if (kill == EPlayerKill.MEGA)
          {
            this.player.sendStat(EPlayerStat.KILLS_ZOMBIES_MEGA);
            this.player.skills.askAward(32U);
          }
          else if (kill == EPlayerKill.ANIMAL)
          {
            this.player.sendStat(EPlayerStat.KILLS_ANIMALS);
            this.player.skills.askAward(8U);
          }
          else if (kill == EPlayerKill.RESOURCE)
          {
            this.player.sendStat(EPlayerStat.FOUND_RESOURCES);
            this.player.skills.askAward(4U);
          }
          if (((ItemGunAsset) this.player.equipment.asset).action == EAction.String)
          {
            if ((int) this.player.equipment.state[17] > 0 && (double) inputInfo.point.sqrMagnitude > 0.100000001490116)
            {
              if ((int) this.player.equipment.state[17] > (int) this.thirdAttachments.magazineAsset.stuck)
                this.player.equipment.state[17] -= this.thirdAttachments.magazineAsset.stuck;
              else
                this.player.equipment.state[17] = (byte) 0;
              ItemManager.dropItem(new Item(this.thirdAttachments.magazineID, this.player.equipment.state[10], this.player.equipment.state[17]), inputInfo.point, false, Dedicator.isDedicated, false);
            }
            this.player.equipment.state[8] = (byte) 0;
            this.player.equipment.state[9] = (byte) 0;
            this.player.equipment.state[10] = (byte) 0;
            this.player.equipment.sendUpdateState();
          }
        }
      }
      else
      {
        if (this.player.input.inputs != null && this.player.input.inputs.Count > 0)
        {
          InputInfo inputInfo = this.player.input.inputs.Dequeue();
          if (inputInfo != null)
            this.player.look.aim.LookAt(inputInfo.point);
        }
        if (!this.channel.isOwner)
        {
          this.project(this.player.look.aim.position, this.player.look.aim.forward);
          this.channel.send("askProject", ESteamCall.NOT_OWNER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[2]
          {
            (object) this.player.look.aim.position,
            (object) this.player.look.aim.forward
          });
        }
        this.player.equipment.state[8] = (byte) 0;
        this.player.equipment.state[9] = (byte) 0;
        this.player.equipment.state[10] = (byte) 0;
        this.player.equipment.sendUpdateState();
      }
    }

    [SteamCall]
    public void askAttachSight(CSteamID steamID, byte page, byte x, byte y)
    {
      if (!this.channel.checkOwner(steamID) || !Provider.isServer || (this.player.equipment.isBusy || this.isFired) || (this.isReloading || this.isHammering || !((ItemGunAsset) this.player.equipment.asset).hasSight))
        return;
      Item obj = (Item) null;
      if (this.thirdAttachments.sightAsset != null)
        obj = new Item(this.thirdAttachments.sightID, false, this.player.equipment.state[13]);
      if ((int) page != (int) byte.MaxValue)
      {
        byte index1 = this.player.inventory.getIndex(page, x, y);
        if ((int) index1 != (int) byte.MaxValue)
        {
          ItemJar itemJar = this.player.inventory.getItem(page, index1);
          ItemCaliberAsset itemCaliberAsset = (ItemCaliberAsset) Assets.find(EAssetType.ITEM, itemJar.item.id);
          if (itemCaliberAsset == null)
            return;
          if (itemCaliberAsset.calibers.Length != 0)
          {
            bool flag = false;
            for (byte index2 = (byte) 0; (int) index2 < itemCaliberAsset.calibers.Length; ++index2)
            {
              if ((int) itemCaliberAsset.calibers[(int) index2] == (int) ((ItemGunAsset) this.player.equipment.asset).caliber)
              {
                flag = true;
                break;
              }
            }
            if (!flag)
              return;
          }
          Buffer.BlockCopy((Array) BitConverter.GetBytes(itemJar.item.id), 0, (Array) this.player.equipment.state, 0, 2);
          this.player.equipment.state[13] = itemJar.item.quality;
          this.player.inventory.removeItem(page, index1);
          if (obj != null)
            this.player.inventory.forceAddItem(obj, true);
          this.player.equipment.sendUpdateState();
          EffectManager.sendEffect((ushort) 8, EffectManager.SMALL, this.transform.position);
          return;
        }
      }
      if (obj != null)
        this.player.inventory.forceAddItem(obj, true);
      this.player.equipment.state[0] = (byte) 0;
      this.player.equipment.state[1] = (byte) 0;
      this.player.equipment.sendUpdateState();
      EffectManager.sendEffect((ushort) 8, EffectManager.SMALL, this.transform.position);
    }

    [SteamCall]
    public void askAttachTactical(CSteamID steamID, byte page, byte x, byte y)
    {
      if (!this.channel.checkOwner(steamID) || !Provider.isServer || (this.player.equipment.isBusy || this.isFired) || (this.isReloading || this.isHammering || !((ItemGunAsset) this.player.equipment.asset).hasTactical))
        return;
      Item obj = (Item) null;
      if (this.thirdAttachments.tacticalAsset != null)
        obj = new Item(this.thirdAttachments.tacticalID, false, this.player.equipment.state[14]);
      if ((int) page != (int) byte.MaxValue)
      {
        byte index1 = this.player.inventory.getIndex(page, x, y);
        if ((int) index1 != (int) byte.MaxValue)
        {
          ItemJar itemJar = this.player.inventory.getItem(page, index1);
          ItemCaliberAsset itemCaliberAsset = (ItemCaliberAsset) Assets.find(EAssetType.ITEM, itemJar.item.id);
          if (itemCaliberAsset == null)
            return;
          if (itemCaliberAsset.calibers.Length != 0)
          {
            bool flag = false;
            for (byte index2 = (byte) 0; (int) index2 < itemCaliberAsset.calibers.Length; ++index2)
            {
              if ((int) itemCaliberAsset.calibers[(int) index2] == (int) ((ItemGunAsset) this.player.equipment.asset).caliber)
              {
                flag = true;
                break;
              }
            }
            if (!flag)
              return;
          }
          Buffer.BlockCopy((Array) BitConverter.GetBytes(itemJar.item.id), 0, (Array) this.player.equipment.state, 2, 2);
          this.player.equipment.state[14] = itemJar.item.quality;
          this.player.inventory.removeItem(page, index1);
          if (obj != null)
            this.player.inventory.forceAddItem(obj, true);
          this.player.equipment.sendUpdateState();
          EffectManager.sendEffect((ushort) 8, EffectManager.SMALL, this.transform.position);
          return;
        }
      }
      if (obj != null)
        this.player.inventory.forceAddItem(obj, true);
      this.player.equipment.state[2] = (byte) 0;
      this.player.equipment.state[3] = (byte) 0;
      this.player.equipment.sendUpdateState();
      EffectManager.sendEffect((ushort) 8, EffectManager.SMALL, this.transform.position);
    }

    [SteamCall]
    public void askAttachGrip(CSteamID steamID, byte page, byte x, byte y)
    {
      if (!this.channel.checkOwner(steamID) || !Provider.isServer || (this.player.equipment.isBusy || this.isFired) || (this.isReloading || this.isHammering || !((ItemGunAsset) this.player.equipment.asset).hasGrip))
        return;
      Item obj = (Item) null;
      if (this.thirdAttachments.gripAsset != null)
        obj = new Item(this.thirdAttachments.gripID, false, this.player.equipment.state[15]);
      if ((int) page != (int) byte.MaxValue)
      {
        byte index1 = this.player.inventory.getIndex(page, x, y);
        if ((int) index1 != (int) byte.MaxValue)
        {
          ItemJar itemJar = this.player.inventory.getItem(page, index1);
          ItemCaliberAsset itemCaliberAsset = (ItemCaliberAsset) Assets.find(EAssetType.ITEM, itemJar.item.id);
          if (itemCaliberAsset == null)
            return;
          if (itemCaliberAsset.calibers.Length != 0)
          {
            bool flag = false;
            for (byte index2 = (byte) 0; (int) index2 < itemCaliberAsset.calibers.Length; ++index2)
            {
              if ((int) itemCaliberAsset.calibers[(int) index2] == (int) ((ItemGunAsset) this.player.equipment.asset).caliber)
              {
                flag = true;
                break;
              }
            }
            if (!flag)
              return;
          }
          Buffer.BlockCopy((Array) BitConverter.GetBytes(itemJar.item.id), 0, (Array) this.player.equipment.state, 4, 2);
          this.player.equipment.state[15] = itemJar.item.quality;
          this.player.inventory.removeItem(page, index1);
          if (obj != null)
            this.player.inventory.forceAddItem(obj, true);
          this.player.equipment.sendUpdateState();
          EffectManager.sendEffect((ushort) 8, EffectManager.SMALL, this.transform.position);
          return;
        }
      }
      if (obj != null)
        this.player.inventory.forceAddItem(obj, true);
      this.player.equipment.state[4] = (byte) 0;
      this.player.equipment.state[5] = (byte) 0;
      this.player.equipment.sendUpdateState();
      EffectManager.sendEffect((ushort) 8, EffectManager.SMALL, this.transform.position);
    }

    [SteamCall]
    public void askAttachBarrel(CSteamID steamID, byte page, byte x, byte y)
    {
      if (!this.channel.checkOwner(steamID) || !Provider.isServer || (this.player.equipment.isBusy || this.isFired) || (this.isReloading || this.isHammering || !((ItemGunAsset) this.player.equipment.asset).hasBarrel))
        return;
      Item obj = (Item) null;
      if (this.thirdAttachments.barrelAsset != null)
        obj = new Item(this.thirdAttachments.barrelID, false, this.player.equipment.state[16]);
      if ((int) page != (int) byte.MaxValue)
      {
        byte index1 = this.player.inventory.getIndex(page, x, y);
        if ((int) index1 != (int) byte.MaxValue)
        {
          ItemJar itemJar = this.player.inventory.getItem(page, index1);
          ItemCaliberAsset itemCaliberAsset = (ItemCaliberAsset) Assets.find(EAssetType.ITEM, itemJar.item.id);
          if (itemCaliberAsset == null)
            return;
          if (itemCaliberAsset.calibers.Length != 0)
          {
            bool flag = false;
            for (byte index2 = (byte) 0; (int) index2 < itemCaliberAsset.calibers.Length; ++index2)
            {
              if ((int) itemCaliberAsset.calibers[(int) index2] == (int) ((ItemGunAsset) this.player.equipment.asset).caliber)
              {
                flag = true;
                break;
              }
            }
            if (!flag)
              return;
          }
          Buffer.BlockCopy((Array) BitConverter.GetBytes(itemJar.item.id), 0, (Array) this.player.equipment.state, 6, 2);
          this.player.equipment.state[16] = itemJar.item.quality;
          this.player.inventory.removeItem(page, index1);
          if (obj != null)
            this.player.inventory.forceAddItem(obj, true);
          this.player.equipment.sendUpdateState();
          EffectManager.sendEffect((ushort) 8, EffectManager.SMALL, this.transform.position);
          return;
        }
      }
      if (obj != null)
        this.player.inventory.forceAddItem(obj, true);
      this.player.equipment.state[6] = (byte) 0;
      this.player.equipment.state[7] = (byte) 0;
      this.player.equipment.sendUpdateState();
      EffectManager.sendEffect((ushort) 8, EffectManager.SMALL, this.transform.position);
    }

    [SteamCall]
    public void askAttachMagazine(CSteamID steamID, byte page, byte x, byte y)
    {
      if (!this.channel.checkOwner(steamID) || !Provider.isServer || (this.player.equipment.isBusy || this.isFired) || (this.isReloading || this.isHammering))
        return;
      Item obj = (Item) null;
      if (this.thirdAttachments.magazineAsset != null && (((ItemGunAsset) this.player.equipment.asset).action != EAction.Pump && ((ItemGunAsset) this.player.equipment.asset).action != EAction.Rail && (((ItemGunAsset) this.player.equipment.asset).action != EAction.String && ((ItemGunAsset) this.player.equipment.asset).action != EAction.Rocket) && ((ItemGunAsset) this.player.equipment.asset).action != EAction.Break || (int) this.ammo > 0))
        obj = new Item(this.thirdAttachments.magazineID, this.player.equipment.state[10], this.player.equipment.state[17]);
      if ((int) page != (int) byte.MaxValue)
      {
        byte index1 = this.player.inventory.getIndex(page, x, y);
        if ((int) index1 != (int) byte.MaxValue)
        {
          ItemJar itemJar = this.player.inventory.getItem(page, index1);
          ItemCaliberAsset itemCaliberAsset = (ItemCaliberAsset) Assets.find(EAssetType.ITEM, itemJar.item.id);
          if (itemCaliberAsset == null)
            return;
          if (itemCaliberAsset.calibers.Length != 0)
          {
            bool flag = false;
            for (byte index2 = (byte) 0; (int) index2 < itemCaliberAsset.calibers.Length; ++index2)
            {
              if ((int) itemCaliberAsset.calibers[(int) index2] == (int) ((ItemGunAsset) this.player.equipment.asset).caliber)
              {
                flag = true;
                break;
              }
            }
            if (!flag)
              return;
          }
          bool flag1 = (int) this.ammo == 0;
          this.ammo = itemJar.item.amount;
          Buffer.BlockCopy((Array) BitConverter.GetBytes(itemJar.item.id), 0, (Array) this.player.equipment.state, 8, 2);
          this.player.equipment.state[10] = itemJar.item.amount;
          this.player.equipment.state[17] = itemJar.item.quality;
          this.player.inventory.removeItem(page, index1);
          if (obj != null)
            this.player.inventory.forceAddItem(obj, true);
          this.player.equipment.sendUpdateState();
          this.channel.send("askReload", ESteamCall.ALL, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[1]
          {
            (object) (bool) (!flag1 ? 0 : ((UnityEngine.Object) ((ItemGunAsset) this.player.equipment.asset).hammer != (UnityEngine.Object) null ? 1 : 0))
          });
          EffectManager.sendEffect((ushort) 8, EffectManager.SMALL, this.transform.position);
          return;
        }
      }
      if (obj != null)
        this.player.inventory.forceAddItem(obj, true);
      this.player.equipment.state[8] = (byte) 0;
      this.player.equipment.state[9] = (byte) 0;
      this.player.equipment.state[10] = (byte) 0;
      this.player.equipment.sendUpdateState();
      this.channel.send("askReload", ESteamCall.ALL, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[1]
      {
        (object) (bool) ((UnityEngine.Object) ((ItemGunAsset) this.player.equipment.asset).hammer != (UnityEngine.Object) null ? 1 : 0)
      });
      EffectManager.sendEffect((ushort) 8, EffectManager.SMALL, this.transform.position);
    }

    private void hammer()
    {
      this.player.equipment.isBusy = true;
      this.isHammering = true;
      this.startedHammer = Time.realtimeSinceStartup;
      this.player.playSound(((ItemGunAsset) this.player.equipment.asset).hammer, (float) (1.0 + (double) this.player.skills.mastery(0, 2) * 0.5), 0.05f);
      if (this.isAiming)
      {
        this.isAiming = false;
        this.stopAim();
        if (((ItemGunAsset) this.player.equipment.asset).action == EAction.Bolt || ((ItemGunAsset) this.player.equipment.asset).action == EAction.Pump)
        {
          this.player.animator.play("Scope", false);
          return;
        }
      }
      this.player.animator.play("Hammer", false);
    }

    [SteamCall]
    public void askReload(CSteamID steamID, bool newHammer)
    {
      if (!this.channel.checkServer(steamID) || !this.player.equipment.isEquipped)
        return;
      if (this.isAiming)
      {
        this.isAiming = false;
        this.stopAim();
      }
      if (this.isAttaching)
      {
        this.isAttaching = false;
        this.stopAttach();
      }
      this.isShooting = false;
      this.isSprinting = false;
      this.player.equipment.isBusy = true;
      this.needsHammer = newHammer;
      this.isReloading = true;
      this.startedReload = Time.realtimeSinceStartup;
      this.player.playSound(((ItemGunAsset) this.player.equipment.asset).reload, (float) (1.0 + (double) this.player.skills.mastery(0, 2) * 0.5), 0.05f);
      this.player.animator.play("Reload", false);
      this.needsReplace = true;
      if (this.channel.isOwner && (UnityEngine.Object) this.firstAttachments.magazineModel != (UnityEngine.Object) null)
        this.firstAttachments.magazineModel.gameObject.SetActive(false);
      if ((UnityEngine.Object) this.thirdAttachments.magazineModel != (UnityEngine.Object) null)
        this.thirdAttachments.magazineModel.gameObject.SetActive(false);
      if (((ItemGunAsset) this.player.equipment.asset).action != EAction.Break)
        return;
      this.needsUnload = true;
    }

    [SteamCall]
    public void askAimStart(CSteamID steamID)
    {
      if (!this.channel.checkServer(steamID) || !this.player.equipment.isEquipped)
        return;
      this.startAim();
    }

    [SteamCall]
    public void askAimStop(CSteamID steamID)
    {
      if (!this.channel.checkServer(steamID) || !this.player.equipment.isEquipped)
        return;
      this.stopAim();
    }

    public override void startPrimary()
    {
      if (this.isShooting || this.isReloading || (this.isHammering || this.isSprinting) || (this.isAttaching || this.firemode == EFiremode.SAFETY || this.player.equipment.isBusy) || ((ItemGunAsset) this.player.equipment.asset).action == EAction.String && !((UnityEngine.Object) this.thirdAttachments.nockHook != (UnityEngine.Object) null) && !this.isAiming)
        return;
      this.isShooting = true;
    }

    public override void stopPrimary()
    {
      if (!this.isShooting)
        return;
      this.isShooting = false;
    }

    public override void startSecondary()
    {
      if (this.isAiming || this.isReloading || (this.isHammering || this.isSprinting) || this.isAttaching)
        return;
      this.isAiming = true;
      this.startAim();
      if (!Provider.isServer)
        return;
      this.channel.send("askAimStart", ESteamCall.NOT_OWNER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER);
    }

    public override void stopSecondary()
    {
      if (!this.isAiming)
        return;
      this.isAiming = false;
      this.stopAim();
      if (!Provider.isServer)
        return;
      this.channel.send("askAimStop", ESteamCall.NOT_OWNER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER);
    }

    public override void equip()
    {
      this.lastShot = float.MaxValue;
      if (!Dedicator.isDedicated)
      {
        this.sound = !this.channel.isOwner ? this.player.equipment.thirdModel.gameObject.AddComponent<AudioSource>() : this.player.gameObject.AddComponent<AudioSource>();
        this.sound.clip = (AudioClip) null;
        this.sound.spatialBlend = 1f;
        this.sound.rolloffMode = AudioRolloffMode.Linear;
        this.sound.volume = 1f;
        this.sound.minDistance = 8f;
        this.sound.maxDistance = ((ItemGunAsset) this.player.equipment.asset).action == EAction.String || ((ItemGunAsset) this.player.equipment.asset).action == EAction.Rocket ? 16f : 256f;
        this.sound.playOnAwake = false;
      }
      if (this.channel.isOwner)
      {
        this.firstAttachments = this.player.equipment.firstModel.gameObject.GetComponent<Attachments>();
        if ((UnityEngine.Object) this.firstAttachments.rope != (UnityEngine.Object) null)
          this.firstAttachments.rope.gameObject.SetActive(true);
        if ((UnityEngine.Object) this.firstAttachments.ejectHook != (UnityEngine.Object) null && ((ItemGunAsset) this.player.equipment.asset).action != EAction.String && ((ItemGunAsset) this.player.equipment.asset).action != EAction.Rocket)
        {
          EffectAsset effectAsset = (EffectAsset) null;
          if (((ItemGunAsset) this.player.equipment.asset).action == EAction.Pump || ((ItemGunAsset) this.player.equipment.asset).action == EAction.Break)
            effectAsset = (EffectAsset) Assets.find(EAssetType.EFFECT, (ushort) 33);
          else if (((ItemGunAsset) this.player.equipment.asset).action != EAction.Rail)
            effectAsset = (EffectAsset) Assets.find(EAssetType.EFFECT, (ushort) 1);
          if (effectAsset != null)
          {
            Transform transform = UnityEngine.Object.Instantiate<GameObject>(effectAsset.effect).transform;
            transform.name = "Emitter";
            transform.parent = this.firstAttachments.ejectHook;
            transform.localPosition = Vector3.zero;
            transform.localRotation = !this.channel.owner.hand ? Quaternion.identity : Quaternion.Euler(0.0f, 180f, 0.0f);
            transform.tag = "Viewmodel";
            transform.gameObject.layer = LayerMasks.VIEWMODEL;
            this.firstShellEmitter = transform.GetComponent<ParticleSystem>();
          }
        }
        if ((UnityEngine.Object) this.firstAttachments.barrelHook != (UnityEngine.Object) null)
        {
          EffectAsset effectAsset = (EffectAsset) Assets.find(EAssetType.EFFECT, ((ItemGunAsset) this.player.equipment.asset).muzzle);
          if (effectAsset != null)
          {
            Transform transform = UnityEngine.Object.Instantiate<GameObject>(effectAsset.effect).transform;
            transform.name = "Emitter";
            transform.parent = this.firstAttachments.barrelHook;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.tag = "Viewmodel";
            transform.gameObject.layer = LayerMasks.VIEWMODEL;
            this.firstMuzzleEmitter = transform.GetComponent<ParticleSystem>();
            this.firstMuzzleEmitter.simulationSpace = ParticleSystemSimulationSpace.Local;
          }
        }
      }
      this.thirdAttachments = this.player.equipment.thirdModel.gameObject.GetComponent<Attachments>();
      if ((UnityEngine.Object) this.thirdAttachments.ejectHook != (UnityEngine.Object) null && ((ItemGunAsset) this.player.equipment.asset).action != EAction.String && ((ItemGunAsset) this.player.equipment.asset).action != EAction.Rocket)
      {
        EffectAsset effectAsset = (EffectAsset) null;
        if (((ItemGunAsset) this.player.equipment.asset).action == EAction.Pump || ((ItemGunAsset) this.player.equipment.asset).action == EAction.Break)
          effectAsset = (EffectAsset) Assets.find(EAssetType.EFFECT, (ushort) 33);
        else if (((ItemGunAsset) this.player.equipment.asset).action != EAction.Rail)
          effectAsset = (EffectAsset) Assets.find(EAssetType.EFFECT, (ushort) 1);
        if (effectAsset != null)
        {
          Transform transform = UnityEngine.Object.Instantiate<GameObject>(effectAsset.effect).transform;
          transform.name = "Emitter";
          transform.parent = this.thirdAttachments.ejectHook;
          transform.localPosition = Vector3.zero;
          transform.localRotation = !this.channel.owner.hand ? Quaternion.identity : Quaternion.Euler(0.0f, 180f, 0.0f);
          this.thirdShellEmitter = transform.GetComponent<ParticleSystem>();
        }
      }
      if ((UnityEngine.Object) this.thirdAttachments.barrelHook != (UnityEngine.Object) null)
      {
        EffectAsset effectAsset = (EffectAsset) Assets.find(EAssetType.EFFECT, ((ItemGunAsset) this.player.equipment.asset).muzzle);
        if (effectAsset != null)
        {
          Transform transform = UnityEngine.Object.Instantiate<GameObject>(effectAsset.effect).transform;
          transform.name = "Emitter";
          transform.parent = this.thirdAttachments.barrelHook;
          transform.localPosition = Vector3.zero;
          transform.localRotation = Quaternion.identity;
          this.thirdMuzzleEmitter = transform.GetComponent<ParticleSystem>();
        }
      }
      this.ammo = this.player.equipment.state[10];
      this.firemode = (EFiremode) this.player.equipment.state[11];
      this.interact = (int) this.player.equipment.state[12] == 1;
      this.updateAttachments();
      this.startedReload = float.MaxValue;
      this.startedHammer = float.MaxValue;
      this.player.animator.setAnimationSpeed("Scope", (float) (1.0 + (double) this.player.skills.mastery(0, 2) * 0.5));
      this.player.animator.setAnimationSpeed("Reload", (float) (1.0 + (double) this.player.skills.mastery(0, 2) * 0.5));
      this.player.animator.setAnimationSpeed("Hammer", (float) (1.0 + (double) this.player.skills.mastery(0, 2) * 0.5));
      this.reloadTime = this.player.animator.getAnimationLength("Reload");
      this.hammerTime = this.player.animator.getAnimationLength("Hammer");
      if (this.channel.isOwner)
      {
        if (this.firemode == EFiremode.SAFETY)
          PlayerUI.message(EPlayerMessage.SAFETY, string.Empty);
        else if ((int) this.ammo == 0)
          PlayerUI.message(EPlayerMessage.RELOAD, string.Empty);
        this.player.animator.lockView();
        this.player.animator.getAnimationSample("Aim_Start", 1f);
        this.sightOffset = !((UnityEngine.Object) this.firstAttachments.sightHook != (UnityEngine.Object) null) ? Vector3.zero : this.player.animator.view.InverseTransformPoint(this.firstAttachments.sightHook.position);
        if ((UnityEngine.Object) this.firstAttachments.aimHook != (UnityEngine.Object) null)
        {
          Vector3 vector3 = this.firstAttachments.aimHook.localPosition + this.firstAttachments.aimHook.parent.localPosition;
          this.scopeOffset = new Vector3(vector3.x, vector3.z, vector3.y);
        }
        else
          this.scopeOffset = new Vector3(0.0f, 0.01f, -0.04f);
        this.player.animator.unlockView();
        this.localization = Localization.read("/Player/Useable/PlayerUseableGun.dat");
        if (this.icons != null)
          this.icons.unload();
        this.icons = Bundles.getBundle("/Bundles/Textures/Player/Icons/Useable/PlayerUseableGun/PlayerUseableGun.unity3d");
        if (((ItemGunAsset) this.player.equipment.asset).hasSight)
        {
          this.sightButton = new SleekButtonIcon((Texture2D) this.icons.load("Sight"));
          this.sightButton.sizeOffset_X = 50;
          this.sightButton.sizeOffset_Y = 50;
          this.sightButton.tooltip = this.localization.format("Sight_Hook_Tooltip");
          this.sightButton.onClickedButton = new ClickedButton(this.onClickedSightHookButton);
          PlayerUI.container.add((Sleek) this.sightButton);
          this.sightButton.isVisible = false;
        }
        if (((ItemGunAsset) this.player.equipment.asset).hasTactical)
        {
          this.tacticalButton = new SleekButtonIcon((Texture2D) this.icons.load("Tactical"));
          this.tacticalButton.sizeOffset_X = 50;
          this.tacticalButton.sizeOffset_Y = 50;
          this.tacticalButton.tooltip = this.localization.format("Tactical_Hook_Tooltip");
          this.tacticalButton.onClickedButton = new ClickedButton(this.onClickedTacticalHookButton);
          PlayerUI.container.add((Sleek) this.tacticalButton);
          this.tacticalButton.isVisible = false;
        }
        if (((ItemGunAsset) this.player.equipment.asset).hasGrip)
        {
          this.gripButton = new SleekButtonIcon((Texture2D) this.icons.load("Grip"));
          this.gripButton.sizeOffset_X = 50;
          this.gripButton.sizeOffset_Y = 50;
          this.gripButton.tooltip = this.localization.format("Grip_Hook_Tooltip");
          this.gripButton.onClickedButton = new ClickedButton(this.onClickedGripHookButton);
          PlayerUI.container.add((Sleek) this.gripButton);
          this.gripButton.isVisible = false;
        }
        if (((ItemGunAsset) this.player.equipment.asset).hasBarrel)
        {
          this.barrelButton = new SleekButtonIcon((Texture2D) this.icons.load("Barrel"));
          this.barrelButton.sizeOffset_X = 50;
          this.barrelButton.sizeOffset_Y = 50;
          this.barrelButton.tooltip = this.localization.format("Barrel_Hook_Tooltip");
          this.barrelButton.onClickedButton = new ClickedButton(this.onClickedBarrelHookButton);
          PlayerUI.container.add((Sleek) this.barrelButton);
          this.barrelButton.isVisible = false;
        }
        if ((UnityEngine.Object) this.thirdAttachments.magazineHook != (UnityEngine.Object) null)
        {
          this.magazineButton = new SleekButtonIcon((Texture2D) this.icons.load("Magazine"));
          this.magazineButton.sizeOffset_X = 50;
          this.magazineButton.sizeOffset_Y = 50;
          this.magazineButton.tooltip = this.localization.format("Magazine_Hook_Tooltip");
          this.magazineButton.onClickedButton = new ClickedButton(this.onClickedMagazineHookButton);
          PlayerUI.container.add((Sleek) this.magazineButton);
          this.magazineButton.isVisible = false;
        }
        this.icons.unload();
        this.infoBox = new SleekBox();
        this.infoBox.positionOffset_Y = -70;
        this.infoBox.positionScale_X = 0.7f;
        this.infoBox.positionScale_Y = 1f;
        this.infoBox.sizeOffset_Y = 70;
        this.infoBox.sizeScale_X = 0.3f;
        PlayerLifeUI.container.add((Sleek) this.infoBox);
        this.ammoLabel = new SleekLabel();
        this.ammoLabel.sizeScale_X = 0.35f;
        this.ammoLabel.sizeScale_Y = 1f;
        this.ammoLabel.fontSize = 24;
        this.infoBox.add((Sleek) this.ammoLabel);
        this.firemodeLabel = new SleekLabel();
        this.firemodeLabel.positionOffset_Y = 5;
        this.firemodeLabel.positionScale_X = 0.35f;
        this.firemodeLabel.sizeScale_X = 0.65f;
        this.firemodeLabel.sizeScale_Y = 0.5f;
        this.infoBox.add((Sleek) this.firemodeLabel);
        this.attachLabel = new SleekLabel();
        this.attachLabel.positionOffset_Y = -5;
        this.attachLabel.positionScale_X = 0.35f;
        this.attachLabel.positionScale_Y = 0.5f;
        this.attachLabel.sizeScale_X = 0.65f;
        this.attachLabel.sizeScale_Y = 0.5f;
        this.infoBox.add((Sleek) this.attachLabel);
        this.updateInfo();
      }
      this.player.animator.play("Equip", true);
      if (!this.player.channel.isOwner)
        return;
      PlayerUI.disableDot();
      this.player.stance.onStanceUpdated += new StanceUpdated(this.updateCrosshair);
      this.player.look.onPerspectiveUpdated += new PerspectiveUpdated(this.onPerspectiveUpdated);
    }

    public override void dequip()
    {
      if (this.infoBox != null)
      {
        if (this.sightButton != null)
          PlayerUI.container.remove((Sleek) this.sightButton);
        if (this.tacticalButton != null)
          PlayerUI.container.remove((Sleek) this.tacticalButton);
        if (this.gripButton != null)
          PlayerUI.container.remove((Sleek) this.gripButton);
        if (this.barrelButton != null)
          PlayerUI.container.remove((Sleek) this.barrelButton);
        if (this.magazineButton != null)
          PlayerUI.container.remove((Sleek) this.magazineButton);
        if (this.rangeLabel != null)
          PlayerLifeUI.container.remove((Sleek) this.rangeLabel);
        PlayerLifeUI.container.remove((Sleek) this.infoBox);
      }
      if (!Dedicator.isDedicated)
        this.player.updateSpot(false);
      if (this.channel.isOwner || Provider.isServer)
        this.player.movement.multiplier = 1f;
      if (!this.channel.isOwner)
        return;
      if ((UnityEngine.Object) this.sound != (UnityEngine.Object) null)
        UnityEngine.Object.Destroy((UnityEngine.Object) this.sound);
      if ((UnityEngine.Object) this.laser != (UnityEngine.Object) null)
        UnityEngine.Object.Destroy((UnityEngine.Object) this.laser.gameObject);
      if (this.isAiming)
        this.stopAim();
      PlayerUI.isLocked = false;
      if (this.isAttaching)
        PlayerLifeUI.open();
      if ((UnityEngine.Object) this.player.movement.getVehicle() == (UnityEngine.Object) null)
        PlayerUI.enableDot();
      PlayerUI.disableCrosshair();
      this.player.look.disableScope();
      this.player.stance.onStanceUpdated -= new StanceUpdated(this.updateCrosshair);
      this.player.look.onPerspectiveUpdated -= new PerspectiveUpdated(this.onPerspectiveUpdated);
    }

    public override void tick()
    {
      if (this.channel.isOwner && (UnityEngine.Object) this.firstAttachments.rope != (UnityEngine.Object) null)
      {
        this.firstAttachments.rope.SetPosition(0, this.firstAttachments.leftHook.position);
        if ((UnityEngine.Object) this.firstAttachments.nockHook != (UnityEngine.Object) null)
        {
          if ((UnityEngine.Object) this.firstAttachments.magazineModel != (UnityEngine.Object) null && this.firstAttachments.magazineModel.gameObject.activeSelf)
            this.firstAttachments.rope.SetPosition(1, this.firstAttachments.nockHook.position);
          else
            this.firstAttachments.rope.SetPosition(1, this.firstAttachments.restHook.position);
        }
        else if (this.isAiming)
          this.firstAttachments.rope.SetPosition(1, this.player.equipment.firstRightHook.position);
        else if ((this.isAttaching || this.isSprinting || this.player.equipment.isInspecting) && ((UnityEngine.Object) this.firstAttachments.magazineModel != (UnityEngine.Object) null && this.firstAttachments.magazineModel.gameObject.activeSelf))
          this.firstAttachments.rope.SetPosition(1, this.firstAttachments.restHook.position);
        else
          this.firstAttachments.rope.SetPosition(1, this.firstAttachments.leftHook.position);
        this.firstAttachments.rope.SetPosition(2, this.firstAttachments.rightHook.position);
      }
      if (!this.player.equipment.isEquipped)
        return;
      if ((double) Time.realtimeSinceStartup - (double) this.lastShot > 0.05)
      {
        if ((UnityEngine.Object) this.firstMuzzleEmitter != (UnityEngine.Object) null)
          this.firstMuzzleEmitter.GetComponent<Light>().enabled = false;
        if ((UnityEngine.Object) this.thirdMuzzleEmitter != (UnityEngine.Object) null)
          this.thirdMuzzleEmitter.GetComponent<Light>().enabled = false;
      }
      if (this.player.stance.stance == EPlayerStance.SPRINT && this.player.movement.isMoving || this.firemode == EFiremode.SAFETY)
      {
        if (!this.isShooting && !this.isSprinting && (!this.isReloading && !this.isHammering) && (!this.isAttaching && !this.isAiming))
        {
          this.isSprinting = true;
          this.player.animator.play("Sprint_Start", false);
        }
      }
      else if (this.isSprinting)
      {
        this.isSprinting = false;
        this.player.animator.play("Sprint_Stop", false);
      }
      if (this.channel.isOwner)
      {
        if (Input.GetKeyUp(ControlsSettings.attach) && this.isAttaching)
        {
          this.isAttaching = false;
          this.player.animator.play("Attach_Stop", false);
          this.stopAttach();
        }
        if (!PlayerUI.window.showCursor)
        {
          if (Input.GetKeyDown(ControlsSettings.attach) && !this.isShooting && (!this.isAttaching && !this.isSprinting) && (!this.isReloading && !this.isHammering && !this.isAiming))
          {
            this.isAttaching = true;
            this.player.animator.play("Attach_Start", false);
            this.updateAttach();
            this.startAttach();
          }
          if (Input.GetKeyDown(ControlsSettings.reload) && !this.isShooting && (!this.isReloading && !this.isHammering) && (!this.isSprinting && !this.isAttaching && !this.isAiming))
          {
            this.magazineSearch = this.player.inventory.search(this.player.inventory.search(EItemType.MAGAZINE, ((ItemGunAsset) this.player.equipment.asset).caliber));
            if (this.magazineSearch.Count > 0)
            {
              byte num1 = (byte) 0;
              byte num2 = byte.MaxValue;
              for (byte index = (byte) 0; (int) index < this.magazineSearch.Count; ++index)
              {
                if ((int) this.magazineSearch[(int) index].jar.item.amount > (int) num1)
                {
                  num1 = this.magazineSearch[(int) index].jar.item.amount;
                  num2 = index;
                }
              }
              if ((int) num2 != (int) byte.MaxValue)
                this.channel.send("askAttachMagazine", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[3]
                {
                  (object) this.magazineSearch[(int) num2].page,
                  (object) this.magazineSearch[(int) num2].jar.x,
                  (object) this.magazineSearch[(int) num2].jar.y
                });
            }
          }
          if (Input.GetKeyDown(ControlsSettings.tactical) && this.thirdAttachments.tacticalAsset != null && (this.thirdAttachments.tacticalAsset.isLight || this.thirdAttachments.tacticalAsset.isLaser || this.thirdAttachments.tacticalAsset.isRangefinder))
            this.channel.send("askInteractGun", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER);
          if (Input.GetKeyDown(ControlsSettings.firemode) && !this.isAiming)
          {
            if (this.firemode == EFiremode.SAFETY)
            {
              if (((ItemGunAsset) this.player.equipment.asset).hasSemi)
                this.channel.send("askFiremode", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[1]
                {
                  (object) 1
                });
              else if (((ItemGunAsset) this.player.equipment.asset).hasAuto)
                this.channel.send("askFiremode", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[1]
                {
                  (object) 2
                });
              PlayerUI.message(EPlayerMessage.NONE, string.Empty);
            }
            else if (this.firemode == EFiremode.SEMI)
            {
              if (((ItemGunAsset) this.player.equipment.asset).hasAuto)
                this.channel.send("askFiremode", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[1]
                {
                  (object) 2
                });
              else if (((ItemGunAsset) this.player.equipment.asset).hasSafety)
              {
                this.channel.send("askFiremode", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[1]
                {
                  (object) 0
                });
                PlayerUI.message(EPlayerMessage.SAFETY, string.Empty);
              }
            }
            else if (this.firemode == EFiremode.AUTO)
            {
              if (((ItemGunAsset) this.player.equipment.asset).hasSafety)
              {
                this.channel.send("askFiremode", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[1]
                {
                  (object) 0
                });
                PlayerUI.message(EPlayerMessage.SAFETY, string.Empty);
              }
              else if (((ItemGunAsset) this.player.equipment.asset).hasSemi)
                this.channel.send("askFiremode", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[1]
                {
                  (object) 1
                });
            }
          }
        }
        if (this.isAttaching)
        {
          if (this.sightButton != null)
          {
            if (this.player.look.perspective == EPlayerPerspective.FIRST)
            {
              Vector2 vector2 = (Vector2) this.player.animator.view.GetComponent<Camera>().WorldToScreenPoint(this.firstAttachments.sightHook.position + this.firstAttachments.sightHook.up * 0.05f + this.firstAttachments.sightHook.forward * 0.05f);
              this.sightButton.positionOffset_X = (int) ((double) vector2.x - 25.0);
              this.sightButton.positionOffset_Y = (int) ((double) Screen.height - (double) vector2.y - 25.0);
              this.sightButton.positionScale_X = 0.0f;
              this.sightButton.positionScale_Y = 0.0f;
            }
            else
            {
              this.sightButton.positionOffset_X = -25;
              this.sightButton.positionOffset_Y = -25;
              this.sightButton.positionScale_X = 0.667f;
              this.sightButton.positionScale_Y = 0.75f;
            }
          }
          if (this.tacticalButton != null)
          {
            if (this.player.look.perspective == EPlayerPerspective.FIRST)
            {
              Vector2 vector2 = (Vector2) this.player.animator.view.GetComponent<Camera>().WorldToScreenPoint(this.firstAttachments.tacticalHook.position);
              this.tacticalButton.positionOffset_X = (int) ((double) vector2.x - 25.0);
              this.tacticalButton.positionOffset_Y = (int) ((double) Screen.height - (double) vector2.y - 25.0);
              this.tacticalButton.positionScale_X = 0.0f;
              this.tacticalButton.positionScale_Y = 0.0f;
            }
            else
            {
              this.tacticalButton.positionOffset_X = -25;
              this.tacticalButton.positionOffset_Y = -25;
              this.tacticalButton.positionScale_X = 0.5f;
              this.tacticalButton.positionScale_Y = 0.25f;
            }
          }
          if (this.gripButton != null)
          {
            if (this.player.look.perspective == EPlayerPerspective.FIRST)
            {
              Vector2 vector2 = (Vector2) this.player.animator.view.GetComponent<Camera>().WorldToScreenPoint(this.firstAttachments.gripHook.position + this.firstAttachments.gripHook.forward * -0.05f);
              this.gripButton.positionOffset_X = (int) ((double) vector2.x - 25.0);
              this.gripButton.positionOffset_Y = (int) ((double) Screen.height - (double) vector2.y - 25.0);
              this.gripButton.positionScale_X = 0.0f;
              this.gripButton.positionScale_Y = 0.0f;
            }
            else
            {
              this.gripButton.positionOffset_X = -25;
              this.gripButton.positionOffset_Y = -25;
              this.gripButton.positionScale_X = 0.75f;
              this.gripButton.positionScale_Y = 0.25f;
            }
          }
          if (this.barrelButton != null)
          {
            if (this.player.look.perspective == EPlayerPerspective.FIRST)
            {
              Vector2 vector2 = (Vector2) this.player.animator.view.GetComponent<Camera>().WorldToScreenPoint(this.firstAttachments.barrelHook.position + this.firstAttachments.barrelHook.up * 0.05f);
              this.barrelButton.positionOffset_X = (int) ((double) vector2.x - 25.0);
              this.barrelButton.positionOffset_Y = (int) ((double) Screen.height - (double) vector2.y - 25.0);
              this.barrelButton.positionScale_X = 0.0f;
              this.barrelButton.positionScale_Y = 0.0f;
            }
            else
            {
              this.barrelButton.positionOffset_X = -25;
              this.barrelButton.positionOffset_Y = -25;
              this.barrelButton.positionScale_X = 0.25f;
              this.barrelButton.positionScale_Y = 0.25f;
            }
          }
          if (this.magazineButton != null)
          {
            if (this.player.look.perspective == EPlayerPerspective.FIRST)
            {
              Vector2 vector2 = (Vector2) this.player.animator.view.GetComponent<Camera>().WorldToScreenPoint(this.firstAttachments.magazineHook.position + this.firstAttachments.magazineHook.forward * -0.1f);
              this.magazineButton.positionOffset_X = (int) ((double) vector2.x - 25.0);
              this.magazineButton.positionOffset_Y = (int) ((double) Screen.height - (double) vector2.y - 25.0);
              this.magazineButton.positionScale_X = 0.0f;
              this.magazineButton.positionScale_Y = 0.0f;
            }
            else
            {
              this.magazineButton.positionOffset_X = -25;
              this.magazineButton.positionOffset_Y = -25;
              this.magazineButton.positionScale_X = 0.334f;
              this.magazineButton.positionScale_Y = 0.75f;
            }
          }
        }
        if (this.rangeLabel != null)
        {
          if (this.player.look.perspective == EPlayerPerspective.FIRST)
          {
            Vector2 vector2 = (Vector2) this.player.animator.view.GetComponent<Camera>().WorldToScreenPoint(this.firstAttachments.lightHook.position);
            this.rangeLabel.positionOffset_X = (int) ((double) vector2.x - 100.0);
            this.rangeLabel.positionOffset_Y = (int) ((double) Screen.height - (double) vector2.y - 15.0);
            this.rangeLabel.isVisible = true;
          }
          else
            this.rangeLabel.isVisible = false;
        }
      }
      if (this.needsRechamber && (double) Time.realtimeSinceStartup - (double) this.lastShot > 0.25)
      {
        this.needsRechamber = false;
        this.needsEject = true;
        this.hammer();
      }
      if (this.needsEject && (double) Time.realtimeSinceStartup - (double) this.lastShot > 0.699999988079071)
      {
        this.needsEject = false;
        if ((UnityEngine.Object) this.firstShellEmitter != (UnityEngine.Object) null && this.player.look.perspective == EPlayerPerspective.FIRST)
          this.firstShellEmitter.Emit(1);
        if ((UnityEngine.Object) this.thirdShellEmitter != (UnityEngine.Object) null && (!this.channel.isOwner || this.player.look.perspective == EPlayerPerspective.THIRD))
          this.thirdShellEmitter.Emit(1);
      }
      if (this.needsUnload && (double) Time.realtimeSinceStartup - (double) this.startedReload > 0.5)
      {
        this.needsUnload = false;
        if ((UnityEngine.Object) this.firstShellEmitter != (UnityEngine.Object) null && this.player.look.perspective == EPlayerPerspective.FIRST)
          this.firstShellEmitter.Emit(2);
        if ((UnityEngine.Object) this.thirdShellEmitter != (UnityEngine.Object) null && (!this.channel.isOwner || this.player.look.perspective == EPlayerPerspective.THIRD))
          this.thirdShellEmitter.Emit(2);
      }
      if (this.needsReplace && (double) Time.realtimeSinceStartup - (double) this.startedReload > (double) this.reloadTime * (double) ((ItemGunAsset) this.player.equipment.asset).replace)
      {
        this.needsReplace = false;
        if (this.channel.isOwner && (UnityEngine.Object) this.firstAttachments.magazineModel != (UnityEngine.Object) null)
          this.firstAttachments.magazineModel.gameObject.SetActive(true);
        if ((UnityEngine.Object) this.thirdAttachments.magazineModel != (UnityEngine.Object) null)
          this.thirdAttachments.magazineModel.gameObject.SetActive(true);
      }
      if (this.isReloading && (double) Time.realtimeSinceStartup - (double) this.startedReload > (double) this.reloadTime)
      {
        this.isReloading = false;
        if (this.needsHammer)
          this.hammer();
        else
          this.player.equipment.isBusy = false;
      }
      if (!this.isHammering || (double) Time.realtimeSinceStartup - (double) this.startedHammer <= (double) this.hammerTime)
        return;
      this.isHammering = false;
      this.player.equipment.isBusy = false;
    }

    public override void tock(uint clock)
    {
      if (this.isShooting)
      {
        if (this.firemode == EFiremode.SAFETY)
        {
          this.isShooting = false;
          return;
        }
        if (this.isReloading || this.isHammering || (this.isSprinting || this.isAttaching))
        {
          this.isShooting = false;
          return;
        }
        if (this.firemode == EFiremode.SEMI)
          this.isShooting = false;
        if ((long) (clock - this.lastFire) > (long) ((int) ((ItemGunAsset) this.player.equipment.asset).firerate - (this.thirdAttachments.tacticalAsset == null ? 0 : (int) this.thirdAttachments.tacticalAsset.firerate)) && (int) this.ammo > 0)
        {
          this.isFired = true;
          this.lastFire = clock;
          this.player.equipment.isBusy = true;
          this.fire();
        }
      }
      if (!this.isFired || clock - this.lastFire <= 6U)
        return;
      this.isFired = false;
      this.player.equipment.isBusy = false;
    }

    public override void updateState(byte[] newState)
    {
      this.ammo = newState[10];
      this.firemode = (EFiremode) newState[11];
      this.interact = (int) newState[12] == 1;
      if (this.channel.isOwner)
        this.firstAttachments.updateAttachments(newState, true);
      this.thirdAttachments.updateAttachments(newState, false);
      this.updateAttachments();
      if (this.channel.isOwner)
      {
        if ((UnityEngine.Object) this.firstAttachments.aimHook != (UnityEngine.Object) null)
        {
          Vector3 vector3 = this.firstAttachments.aimHook.localPosition + this.firstAttachments.aimHook.parent.localPosition;
          this.scopeOffset = new Vector3(vector3.x, vector3.z, vector3.y);
        }
        else
          this.scopeOffset = new Vector3(0.0f, 0.01f, -0.04f);
      }
      if (this.infoBox == null)
        return;
      if (this.isAttaching)
        this.updateAttach();
      this.updateInfo();
    }

    private void updateAttachments()
    {
      if ((UnityEngine.Object) this.sound != (UnityEngine.Object) null)
      {
        if (this.thirdAttachments.barrelAsset != null && this.thirdAttachments.barrelAsset.isSilenced)
        {
          this.sound.clip = this.thirdAttachments.barrelAsset.shoot;
          this.sound.volume = this.thirdAttachments.barrelAsset.volume;
        }
        else
        {
          this.sound.clip = ((ItemGunAsset) this.player.equipment.asset).shoot;
          this.sound.volume = 1f;
        }
      }
      if (this.channel.isOwner)
      {
        if (this.firstAttachments.tacticalAsset != null)
        {
          if (this.firstAttachments.tacticalAsset.isLaser)
          {
            if (!this.wasLaser)
              PlayerUI.message(EPlayerMessage.LASER, string.Empty);
            this.wasLaser = true;
          }
          else
            this.wasLaser = false;
          if (this.firstAttachments.tacticalAsset.isLight)
          {
            if (!this.wasLight)
              PlayerUI.message(EPlayerMessage.LIGHT, string.Empty);
            this.wasLight = true;
          }
          else
            this.wasLight = false;
          if (this.firstAttachments.tacticalAsset.isRangefinder)
          {
            if (!this.wasRange)
              PlayerUI.message(EPlayerMessage.RANGEFINDER, string.Empty);
            this.wasRange = true;
          }
          else
            this.wasRange = false;
        }
        else
        {
          this.wasLaser = false;
          this.wasLight = false;
          this.wasRange = false;
        }
        if (this.firstAttachments.tacticalAsset != null && this.firstAttachments.tacticalAsset.isLaser && this.interact)
        {
          if ((UnityEngine.Object) this.laser == (UnityEngine.Object) null)
          {
            this.laser = ((GameObject) UnityEngine.Object.Instantiate(Resources.Load("Guns/Laser"))).transform;
            this.laser.name = "Laser";
            this.laser.parent = Level.effects;
            this.laser.position = Vector3.zero;
            this.laser.rotation = Quaternion.identity;
          }
        }
        else if ((UnityEngine.Object) this.laser != (UnityEngine.Object) null)
        {
          UnityEngine.Object.Destroy((UnityEngine.Object) this.laser.gameObject);
          this.laser = (Transform) null;
        }
        if (this.firstAttachments.tacticalAsset != null && this.firstAttachments.tacticalAsset.isRangefinder && this.interact)
        {
          if (this.rangeLabel == null)
          {
            this.rangeLabel = new SleekLabel();
            this.rangeLabel.sizeOffset_X = 200;
            this.rangeLabel.sizeOffset_Y = 30;
            PlayerLifeUI.container.add((Sleek) this.rangeLabel);
            this.rangeLabel.isVisible = false;
          }
        }
        else if (this.rangeLabel != null)
        {
          PlayerLifeUI.container.remove((Sleek) this.rangeLabel);
          this.rangeLabel = (SleekLabel) null;
        }
      }
      if ((UnityEngine.Object) this.firstMuzzleEmitter != (UnityEngine.Object) null)
      {
        if ((UnityEngine.Object) this.firstAttachments.barrelModel != (UnityEngine.Object) null)
          this.firstMuzzleEmitter.transform.localPosition = Vector3.up * 0.25f;
        else
          this.firstMuzzleEmitter.transform.localPosition = Vector3.zero;
      }
      if ((UnityEngine.Object) this.thirdMuzzleEmitter != (UnityEngine.Object) null)
      {
        if ((UnityEngine.Object) this.thirdAttachments.barrelModel != (UnityEngine.Object) null)
          this.thirdMuzzleEmitter.transform.localPosition = Vector3.up * 0.25f;
        else
          this.thirdMuzzleEmitter.transform.localPosition = Vector3.zero;
      }
      if (this.isReloading)
      {
        if (this.channel.isOwner && (UnityEngine.Object) this.firstAttachments.magazineModel != (UnityEngine.Object) null)
          this.firstAttachments.magazineModel.gameObject.SetActive(false);
        if ((UnityEngine.Object) this.thirdAttachments.magazineModel != (UnityEngine.Object) null)
          this.thirdAttachments.magazineModel.gameObject.SetActive(false);
      }
      if (!Dedicator.isDedicated)
      {
        if (this.thirdAttachments.tacticalAsset != null)
        {
          if (this.thirdAttachments.tacticalAsset.isLight || this.thirdAttachments.tacticalAsset.isLaser)
          {
            if (this.channel.isOwner && (UnityEngine.Object) this.firstAttachments.lightHook != (UnityEngine.Object) null)
              this.firstAttachments.lightHook.gameObject.SetActive(this.interact);
            if ((UnityEngine.Object) this.thirdAttachments.lightHook != (UnityEngine.Object) null)
              this.thirdAttachments.lightHook.gameObject.SetActive(this.interact);
          }
          else if (this.thirdAttachments.tacticalAsset.isRangefinder && this.channel.isOwner && (UnityEngine.Object) this.firstAttachments.lightHook != (UnityEngine.Object) null)
          {
            this.firstAttachments.lightHook.gameObject.SetActive(this.inRange && this.interact);
            this.firstAttachments.light2Hook.gameObject.SetActive(!this.inRange && this.interact);
          }
        }
        this.player.updateSpot(this.thirdAttachments.tacticalAsset != null && this.thirdAttachments.tacticalAsset.isLight && this.interact);
      }
      if (!this.channel.isOwner)
        return;
      if ((UnityEngine.Object) this.firstAttachments.scopeHook != (UnityEngine.Object) null)
      {
        this.zoom = this.firstAttachments.sightAsset.zoom;
        this.player.look.enableScope(this.zoom);
        if (GraphicsSettings.scopeQuality == EGraphicQuality.OFF)
          this.firstAttachments.scopeHook.GetComponent<Renderer>().enabled = false;
        else
          this.firstAttachments.scopeHook.GetComponent<Renderer>().enabled = true;
        this.firstAttachments.scopeHook.GetComponent<Renderer>().material.mainTexture = (Texture) this.player.look.scopeCamera.targetTexture;
        this.firstAttachments.scopeHook.gameObject.SetActive(true);
        if (this.channel.owner.hand)
        {
          Vector3 localScale = this.firstAttachments.scopeHook.localScale;
          localScale.x *= -1f;
          this.firstAttachments.scopeHook.localScale = localScale;
        }
      }
      else
      {
        this.zoom = 90f;
        this.player.look.disableScope();
      }
      this.updateCrosshair();
    }

    private void updateCrosshair()
    {
      this.crosshair = ((ItemGunAsset) this.player.equipment.asset).spreadHip;
      this.crosshair *= (float) (1.0 - (double) this.player.skills.mastery(0, 1) * 0.5);
      if (this.thirdAttachments.tacticalAsset != null && this.interact)
        this.crosshair *= this.thirdAttachments.tacticalAsset.spread;
      if (this.thirdAttachments.gripAsset != null && (!this.thirdAttachments.gripAsset.isBipod || this.player.stance.stance == EPlayerStance.PRONE))
        this.crosshair *= this.thirdAttachments.gripAsset.spread;
      if (this.thirdAttachments.barrelAsset != null)
        this.crosshair *= this.thirdAttachments.barrelAsset.spread;
      if (this.thirdAttachments.magazineAsset != null)
        this.crosshair *= this.thirdAttachments.magazineAsset.spread;
      if (this.player.stance.stance == EPlayerStance.SPRINT)
        this.crosshair *= UseableGun.SPREAD_SPRINT;
      else if (this.player.stance.stance == EPlayerStance.CROUCH)
        this.crosshair *= UseableGun.SPREAD_CROUCH;
      else if (this.player.stance.stance == EPlayerStance.PRONE)
        this.crosshair *= UseableGun.SPREAD_PRONE;
      if (this.isAiming)
      {
        if (this.player.look.perspective == EPlayerPerspective.FIRST)
          this.crosshair *= ((ItemGunAsset) this.player.equipment.asset).spreadAim + 0.2f;
        else
          this.crosshair *= 0.5f;
      }
      PlayerUI.updateCrosshair(this.crosshair);
      if (this.isAiming && this.player.look.perspective == EPlayerPerspective.FIRST && (((ItemGunAsset) this.player.equipment.asset).action != EAction.String || (UnityEngine.Object) this.thirdAttachments.sightHook != (UnityEngine.Object) null) || (this.isAttaching || (UnityEngine.Object) this.player.movement.getVehicle() != (UnityEngine.Object) null && this.player.look.perspective != EPlayerPerspective.FIRST))
        PlayerUI.disableCrosshair();
      else
        PlayerUI.enableCrosshair();
    }

    private void updateAttach()
    {
      if (this.sightButton != null)
      {
        this.sightSearch = this.player.inventory.search(this.player.inventory.search(EItemType.SIGHT, ((ItemGunAsset) this.player.equipment.asset).caliber));
        if (this.sightJars != null)
          this.sightButton.remove((Sleek) this.sightJars);
        this.sightJars = new SleekJars(100f, this.sightSearch);
        this.sightJars.sizeScale_X = 1f;
        this.sightJars.sizeScale_Y = 1f;
        this.sightJars.onClickedJar = new ClickedJar(this.onClickedSightJar);
        this.sightButton.add((Sleek) this.sightJars);
      }
      if (this.tacticalButton != null)
      {
        this.tacticalSearch = this.player.inventory.search(this.player.inventory.search(EItemType.TACTICAL, ((ItemGunAsset) this.player.equipment.asset).caliber));
        if (this.tacticalJars != null)
          this.tacticalButton.remove((Sleek) this.tacticalJars);
        this.tacticalJars = new SleekJars(100f, this.tacticalSearch);
        this.tacticalJars.sizeScale_X = 1f;
        this.tacticalJars.sizeScale_Y = 1f;
        this.tacticalJars.onClickedJar = new ClickedJar(this.onClickedTacticalJar);
        this.tacticalButton.add((Sleek) this.tacticalJars);
      }
      if (this.gripButton != null)
      {
        this.gripSearch = this.player.inventory.search(this.player.inventory.search(EItemType.GRIP, ((ItemGunAsset) this.player.equipment.asset).caliber));
        if (this.gripJars != null)
          this.gripButton.remove((Sleek) this.gripJars);
        this.gripJars = new SleekJars(100f, this.gripSearch);
        this.gripJars.sizeScale_X = 1f;
        this.gripJars.sizeScale_Y = 1f;
        this.gripJars.onClickedJar = new ClickedJar(this.onClickedGripJar);
        this.gripButton.add((Sleek) this.gripJars);
      }
      if (this.barrelButton != null)
      {
        this.barrelSearch = this.player.inventory.search(this.player.inventory.search(EItemType.BARREL, ((ItemGunAsset) this.player.equipment.asset).caliber));
        if (this.barrelJars != null)
          this.barrelButton.remove((Sleek) this.barrelJars);
        this.barrelJars = new SleekJars(100f, this.barrelSearch);
        this.barrelJars.sizeScale_X = 1f;
        this.barrelJars.sizeScale_Y = 1f;
        this.barrelJars.onClickedJar = new ClickedJar(this.onClickedBarrelJar);
        this.barrelButton.add((Sleek) this.barrelJars);
      }
      if (this.magazineButton == null)
        return;
      this.magazineSearch = this.player.inventory.search(this.player.inventory.search(EItemType.MAGAZINE, ((ItemGunAsset) this.player.equipment.asset).caliber));
      if (this.magazineJars != null)
        this.magazineButton.remove((Sleek) this.magazineJars);
      this.magazineJars = new SleekJars(100f, this.magazineSearch);
      this.magazineJars.sizeScale_X = 1f;
      this.magazineJars.sizeScale_Y = 1f;
      this.magazineJars.onClickedJar = new ClickedJar(this.onClickedMagazineJar);
      this.magazineButton.add((Sleek) this.magazineJars);
    }

    private void updateInfo()
    {
      if ((int) this.ammo == 0)
        this.ammoLabel.foregroundColor = Palette.COLOR_R;
      else
        this.ammoLabel.foregroundColor = Color.white;
      this.ammoLabel.text = this.localization.format("Ammo", (object) this.ammo, (object) (this.thirdAttachments.magazineAsset == null ? 0 : (int) this.thirdAttachments.magazineAsset.amount));
      if (this.firemode == EFiremode.SAFETY)
        this.firemodeLabel.text = this.localization.format("Firemode", (object) this.localization.format("Safety"), (object) ControlsSettings.firemode);
      else if (this.firemode == EFiremode.SEMI)
        this.firemodeLabel.text = this.localization.format("Firemode", (object) this.localization.format("Semi"), (object) ControlsSettings.firemode);
      else if (this.firemode == EFiremode.AUTO)
        this.firemodeLabel.text = this.localization.format("Firemode", (object) this.localization.format("Auto"), (object) ControlsSettings.firemode);
      this.attachLabel.text = this.localization.format("Attach", (object) (this.thirdAttachments.magazineAsset == null ? this.localization.format("None") : this.thirdAttachments.magazineAsset.itemName), (object) ControlsSettings.attach);
    }

    private void onPerspectiveUpdated(EPlayerPerspective newPerspective)
    {
      if (newPerspective == EPlayerPerspective.THIRD)
      {
        if (this.isAiming)
        {
          PlayerUI.updateScope(false);
          this.player.look.enableZoom(OptionsSettings.view * 0.8f);
        }
        else
          this.player.look.disableZoom();
        if ((UnityEngine.Object) this.player.movement.getVehicle() != (UnityEngine.Object) null)
          PlayerUI.disableCrosshair();
        else
          PlayerUI.enableCrosshair();
      }
      else if (this.isAiming)
      {
        if (GraphicsSettings.scopeQuality == EGraphicQuality.OFF && (UnityEngine.Object) PlayerLifeUI.scopeImage.texture != (UnityEngine.Object) null)
        {
          PlayerUI.updateScope(true);
          this.player.look.enableZoom(this.zoom * 2f);
        }
        else
          this.player.look.disableZoom();
        PlayerUI.disableCrosshair();
      }
      else
      {
        this.player.look.disableZoom();
        PlayerUI.enableCrosshair();
      }
    }

    private void onClickedSightHookButton(SleekButton button)
    {
      this.channel.send("askAttachSight", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[3]
      {
        (object) (int) byte.MaxValue,
        (object) (int) byte.MaxValue,
        (object) (int) byte.MaxValue
      });
    }

    private void onClickedTacticalHookButton(SleekButton button)
    {
      this.channel.send("askAttachTactical", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[3]
      {
        (object) (int) byte.MaxValue,
        (object) (int) byte.MaxValue,
        (object) (int) byte.MaxValue
      });
    }

    private void onClickedGripHookButton(SleekButton button)
    {
      this.channel.send("askAttachGrip", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[3]
      {
        (object) (int) byte.MaxValue,
        (object) (int) byte.MaxValue,
        (object) (int) byte.MaxValue
      });
    }

    private void onClickedBarrelHookButton(SleekButton button)
    {
      this.channel.send("askAttachBarrel", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[3]
      {
        (object) (int) byte.MaxValue,
        (object) (int) byte.MaxValue,
        (object) (int) byte.MaxValue
      });
    }

    private void onClickedMagazineHookButton(SleekButton button)
    {
      this.channel.send("askAttachMagazine", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[3]
      {
        (object) (int) byte.MaxValue,
        (object) (int) byte.MaxValue,
        (object) (int) byte.MaxValue
      });
    }

    private void onClickedSightJar(SleekJars jars, int index)
    {
      this.channel.send("askAttachSight", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[3]
      {
        (object) this.sightSearch[index].page,
        (object) this.sightSearch[index].jar.x,
        (object) this.sightSearch[index].jar.y
      });
    }

    private void onClickedTacticalJar(SleekJars jars, int index)
    {
      this.channel.send("askAttachTactical", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[3]
      {
        (object) this.tacticalSearch[index].page,
        (object) this.tacticalSearch[index].jar.x,
        (object) this.tacticalSearch[index].jar.y
      });
    }

    private void onClickedGripJar(SleekJars jars, int index)
    {
      this.channel.send("askAttachGrip", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[3]
      {
        (object) this.gripSearch[index].page,
        (object) this.gripSearch[index].jar.x,
        (object) this.gripSearch[index].jar.y
      });
    }

    private void onClickedBarrelJar(SleekJars jars, int index)
    {
      this.channel.send("askAttachBarrel", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[3]
      {
        (object) this.barrelSearch[index].page,
        (object) this.barrelSearch[index].jar.x,
        (object) this.barrelSearch[index].jar.y
      });
    }

    private void onClickedMagazineJar(SleekJars jars, int index)
    {
      this.channel.send("askAttachMagazine", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[3]
      {
        (object) this.magazineSearch[index].page,
        (object) this.magazineSearch[index].jar.x,
        (object) this.magazineSearch[index].jar.y
      });
    }

    private void startAim()
    {
      if (this.channel.isOwner || Provider.isServer)
        this.player.movement.multiplier = 0.75f;
      if (this.channel.isOwner)
      {
        this.player.animator.multiplier = 0.1f;
        if (GraphicsSettings.scopeQuality == EGraphicQuality.OFF && (UnityEngine.Object) this.firstAttachments.sightModel != (UnityEngine.Object) null && ((UnityEngine.Object) this.firstAttachments.scopeHook != (UnityEngine.Object) null && (UnityEngine.Object) this.firstAttachments.scopeHook.FindChild("Reticule") != (UnityEngine.Object) null))
        {
          Texture mainTexture = this.firstAttachments.scopeHook.FindChild("Reticule").GetComponent<Renderer>().material.mainTexture;
          PlayerLifeUI.scopeImage.positionOffset_X = -mainTexture.width / 2;
          PlayerLifeUI.scopeImage.positionOffset_Y = -mainTexture.height / 2;
          PlayerLifeUI.scopeImage.sizeOffset_X = mainTexture.width;
          PlayerLifeUI.scopeImage.sizeOffset_Y = mainTexture.height;
          PlayerLifeUI.scopeImage.texture = mainTexture;
          this.player.animator.viewOffset = Vector3.up;
          this.player.look.sensitivity = this.zoom / 45f;
        }
        else
        {
          PlayerLifeUI.scopeImage.texture = (Texture) null;
          this.player.animator.viewOffset = this.sightOffset + this.scopeOffset;
          this.player.look.sensitivity = this.zoom / 90f;
        }
        if (this.player.look.perspective == EPlayerPerspective.FIRST)
        {
          if (GraphicsSettings.scopeQuality == EGraphicQuality.OFF && (UnityEngine.Object) PlayerLifeUI.scopeImage.texture != (UnityEngine.Object) null)
          {
            PlayerUI.updateScope(true);
            this.player.look.enableZoom(this.zoom * 2f);
          }
        }
        else if (this.player.look.perspective == EPlayerPerspective.THIRD)
          this.player.look.enableZoom(OptionsSettings.view * 0.8f);
        this.updateCrosshair();
        PlayerGroupUI.container.isVisible = false;
      }
      this.player.playSound(((ItemGunAsset) this.player.equipment.asset).aim);
      this.player.animator.play("Aim_Start", false);
    }

    private void stopAim()
    {
      if (this.channel.isOwner || Provider.isServer)
        this.player.movement.multiplier = 1f;
      if (this.channel.isOwner)
      {
        this.player.animator.viewOffset = Vector3.zero;
        this.player.animator.multiplier = 1f;
        PlayerUI.updateScope(false);
        this.player.look.sensitivity = 1f;
        this.player.look.disableZoom();
        this.updateCrosshair();
        PlayerGroupUI.container.isVisible = true;
      }
      this.player.animator.play("Aim_Stop", false);
    }

    private void startAttach()
    {
      PlayerUI.isLocked = true;
      PlayerLifeUI.close();
      if (this.sightButton != null)
        this.sightButton.isVisible = true;
      if (this.tacticalButton != null)
        this.tacticalButton.isVisible = true;
      if (this.gripButton != null)
        this.gripButton.isVisible = true;
      if (this.barrelButton != null)
        this.barrelButton.isVisible = true;
      if (this.magazineButton != null)
        this.magazineButton.isVisible = true;
      this.updateCrosshair();
    }

    private void stopAttach()
    {
      PlayerUI.isLocked = false;
      PlayerLifeUI.open();
      if (this.sightButton != null)
        this.sightButton.isVisible = false;
      if (this.tacticalButton != null)
        this.tacticalButton.isVisible = false;
      if (this.gripButton != null)
        this.gripButton.isVisible = false;
      if (this.barrelButton != null)
        this.barrelButton.isVisible = false;
      if (this.magazineButton != null)
        this.magazineButton.isVisible = false;
      this.updateCrosshair();
    }

    private void Update()
    {
      if (!this.channel.isOwner)
        return;
      if ((UnityEngine.Object) this.laser != (UnityEngine.Object) null)
      {
        if (this.player.look.perspective == EPlayerPerspective.FIRST && Physics.Raycast(this.player.look.aim.position, this.player.look.aim.forward, out this.contact, 256f, RayMasks.BLOCK_LASER))
          this.laser.position = this.contact.point + this.player.look.aim.forward * -0.05f;
        else
          this.laser.position = Vector3.zero;
      }
      else
      {
        if (this.firstAttachments.tacticalAsset == null || !this.firstAttachments.tacticalAsset.isRangefinder)
          return;
        bool flag = false;
        if (this.player.look.perspective == EPlayerPerspective.FIRST)
          flag = Physics.Raycast(this.player.look.aim.position, this.player.look.aim.forward, out this.contact, ((ItemWeaponAsset) this.player.equipment.asset).range, RayMasks.BLOCK_LASER);
        if (this.rangeLabel != null)
        {
          this.rangeLabel.text = !this.inRange ? (!OptionsSettings.metric ? "? yd" : "? m") : (!OptionsSettings.metric ? (string) (object) (int) MeasurementTool.MtoYd(this.contact.distance) + (object) " yd" : (string) (object) (int) this.contact.distance + (object) " m");
          this.rangeLabel.backgroundColor = !this.inRange ? Palette.COLOR_R : Palette.COLOR_G;
          this.rangeLabel.foregroundColor = this.rangeLabel.backgroundColor;
        }
        if (flag == this.inRange)
          return;
        this.inRange = flag;
        this.firstAttachments.lightHook.gameObject.SetActive(this.inRange && this.interact);
        this.firstAttachments.light2Hook.gameObject.SetActive(!this.inRange && this.interact);
      }
    }
  }
}
