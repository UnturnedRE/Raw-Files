// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.UseableThrowable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
  public class UseableThrowable : Useable
  {
    private float startedUse;
    private float useTime;
    private bool isUsing;
    private bool isSwinging;

    private bool isUseable
    {
      get
      {
        return (double) Time.realtimeSinceStartup - (double) this.startedUse > (double) this.useTime;
      }
    }

    private bool isThrowable
    {
      get
      {
        return (double) Time.realtimeSinceStartup - (double) this.startedUse > (double) this.useTime * 0.600000023841858;
      }
    }

    private void toss(Vector3 origin, Vector3 direction)
    {
      Transform transform = Object.Instantiate<GameObject>(((ItemThrowableAsset) this.player.equipment.asset).throwable).transform;
      transform.name = "Throwable";
      transform.parent = Level.effects;
      transform.position = origin + direction;
      transform.rotation = Quaternion.LookRotation(direction);
      transform.GetComponent<Rigidbody>().AddForce(direction * (this.player.skills.boost != EPlayerBoost.OLYMPIC ? 750f : 1500f));
      if (((ItemThrowableAsset) this.player.equipment.asset).isExplosive)
      {
        if (Provider.isServer)
        {
          Grenade grenade = transform.gameObject.AddComponent<Grenade>();
          grenade.playerDamage = ((ItemWeaponAsset) this.player.equipment.asset).playerDamageMultiplier.damage;
          grenade.zombieDamage = ((ItemWeaponAsset) this.player.equipment.asset).zombieDamageMultiplier.damage;
          grenade.animalDamage = ((ItemWeaponAsset) this.player.equipment.asset).animalDamageMultiplier.damage;
          grenade.barricadeDamage = ((ItemWeaponAsset) this.player.equipment.asset).barricadeDamage;
          grenade.structureDamage = ((ItemWeaponAsset) this.player.equipment.asset).structureDamage;
          grenade.vehicleDamage = ((ItemWeaponAsset) this.player.equipment.asset).vehicleDamage;
          grenade.resourceDamage = ((ItemWeaponAsset) this.player.equipment.asset).resourceDamage;
        }
        else
          Object.Destroy((Object) transform.gameObject, 2.5f);
      }
      else
      {
        transform.gameObject.AddComponent<Distraction>();
        Object.Destroy((Object) transform.gameObject, 180f);
      }
      if (!((ItemThrowableAsset) this.player.equipment.asset).isSticky)
        return;
      transform.gameObject.AddComponent<Sticky>();
    }

    private void swing()
    {
      this.isSwinging = true;
      this.player.animator.play("Use", false);
      if (Dedicator.isDedicated)
        return;
      this.player.playSound(((ItemThrowableAsset) this.player.equipment.asset).use);
    }

    [SteamCall]
    public void askToss(CSteamID steamID, Vector3 origin, Vector3 direction)
    {
      if (!this.channel.checkServer(steamID) || !this.player.equipment.isEquipped)
        return;
      this.toss(origin, direction);
    }

    [SteamCall]
    public void askSwing(CSteamID steamID)
    {
      if (!this.channel.checkServer(steamID) || !this.player.equipment.isEquipped)
        return;
      this.swing();
    }

    public override void startPrimary()
    {
      if (this.player.equipment.isBusy)
        return;
      this.player.equipment.isBusy = true;
      this.startedUse = Time.realtimeSinceStartup;
      this.isUsing = true;
      this.swing();
      if (!Provider.isServer)
        return;
      this.channel.send("askSwing", ESteamCall.NOT_OWNER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER);
    }

    public override void equip()
    {
      this.player.animator.play("Equip", true);
      this.useTime = this.player.animator.getAnimationLength("Use");
    }

    public override void tick()
    {
      if (!this.player.equipment.isEquipped || !this.channel.isOwner && !Provider.isServer || (!this.isSwinging || !this.isThrowable))
        return;
      this.toss(this.player.look.aim.position, this.player.look.aim.forward);
      if (!this.channel.isOwner)
        this.channel.send("askToss", ESteamCall.NOT_OWNER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[2]
        {
          (object) this.player.look.aim.position,
          (object) this.player.look.aim.forward
        });
      this.isSwinging = false;
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
