// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.InteractableTrap
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
  public class InteractableTrap : Interactable
  {
    private float playerDamage;
    private float zombieDamage;
    private float animalDamage;
    private float barricadeDamage;
    private float structureDamage;
    private float vehicleDamage;
    private float resourceDamage;
    private bool isBroken;
    private bool isExplosive;

    public override void updateState(Asset asset, byte[] state)
    {
      this.playerDamage = ((ItemTrapAsset) asset).playerDamage;
      this.zombieDamage = ((ItemTrapAsset) asset).zombieDamage;
      this.animalDamage = ((ItemTrapAsset) asset).animalDamage;
      this.barricadeDamage = ((ItemTrapAsset) asset).barricadeDamage;
      this.structureDamage = ((ItemTrapAsset) asset).structureDamage;
      this.vehicleDamage = ((ItemTrapAsset) asset).vehicleDamage;
      this.resourceDamage = ((ItemTrapAsset) asset).resourceDamage;
      this.isBroken = ((ItemTrapAsset) asset).isBroken;
      this.isExplosive = ((ItemTrapAsset) asset).isExplosive;
    }

    public override bool checkInteractable()
    {
      return false;
    }

    private void OnTriggerEnter(Collider other)
    {
      if ((Object) other.transform == (Object) this.transform.parent || !Provider.isServer)
        return;
      if (this.isExplosive)
      {
        if (other.transform.tag == "Player")
        {
          if (!Provider.isPvP)
            return;
          EffectManager.sendEffect((ushort) 34, EffectManager.LARGE, this.transform.position);
          DamageTool.explode(this.transform.position, 8f, EDeathCause.LANDMINE, this.playerDamage, this.zombieDamage, this.animalDamage, this.barricadeDamage, this.structureDamage, this.vehicleDamage, this.resourceDamage);
        }
        else
        {
          EffectManager.sendEffect((ushort) 34, EffectManager.LARGE, this.transform.position);
          DamageTool.explode(this.transform.position, 8f, EDeathCause.LANDMINE, this.playerDamage, this.zombieDamage, this.animalDamage, this.barricadeDamage, this.structureDamage, this.vehicleDamage, this.resourceDamage);
        }
      }
      else if (other.transform.tag == "Player")
      {
        if (!Provider.isPvP)
          return;
        Player player = DamageTool.getPlayer(other.transform);
        if (!((Object) player != (Object) null))
          return;
        EPlayerKill kill;
        DamageTool.damage(player, EDeathCause.SHRED, ELimb.SPINE, CSteamID.Nil, Vector3.up, this.playerDamage, 1f, out kill);
        if (this.isBroken)
          player.life.breakLegs();
        EffectManager.sendEffect((ushort) 5, EffectManager.SMALL, this.transform.position + Vector3.up, Vector3.down);
        BarricadeManager.damage(this.transform.parent, 1f, 1f);
      }
      else
      {
        if (!(other.transform.tag == "Agent"))
          return;
        Zombie zombie = DamageTool.getZombie(other.transform);
        if ((Object) zombie != (Object) null)
        {
          EPlayerKill kill;
          DamageTool.damage(zombie, this.transform.forward, this.zombieDamage, 1f, out kill);
          EffectManager.sendEffect((ushort) 5, EffectManager.SMALL, this.transform.position + Vector3.up, Vector3.down);
          BarricadeManager.damage(this.transform.parent, 1f, 1f);
        }
        else
        {
          Animal animal = DamageTool.getAnimal(other.transform);
          if (!((Object) animal != (Object) null))
            return;
          EPlayerKill kill;
          DamageTool.damage(animal, this.transform.forward, this.animalDamage, 1f, out kill);
          EffectManager.sendEffect((ushort) 5, EffectManager.SMALL, this.transform.position + Vector3.up, Vector3.down);
          BarricadeManager.damage(this.transform.parent, 1f, 1f);
        }
      }
    }
  }
}
