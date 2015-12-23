// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.Bumper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
  public class Bumper : MonoBehaviour
  {
    private static readonly float DAMAGE_PLAYER = 10f;
    private static readonly float DAMAGE_ZOMBIE = 15f;
    private static readonly float DAMAGE_ANIMAL = 15f;
    private static readonly float DAMAGE_VEHICLE = 4f;
    private static readonly float DAMAGE_RESOURCE = 85f;
    private InteractableVehicle vehicle;

    public void init(InteractableVehicle newVehicle)
    {
      this.vehicle = newVehicle;
    }

    private void OnTriggerEnter(Collider other)
    {
      if (!Provider.isServer)
        return;
      float times = Mathf.Clamp(Mathf.Abs(this.vehicle.speed), 0.0f, 10f);
      if ((double) times < 3.0)
        return;
      if (other.transform.tag == "Player")
      {
        if (!Provider.isPvP)
          return;
        Player player = DamageTool.getPlayer(other.transform);
        if (!((Object) player != (Object) null) || !((Object) player.movement.getVehicle() == (Object) null) || this.vehicle.passengers[0].player != null && !(this.vehicle.passengers[0].player.playerID.group == CSteamID.Nil) && !(this.vehicle.passengers[0].player.playerID.group != player.channel.owner.playerID.group))
          return;
        EPlayerKill kill;
        DamageTool.damage(player, EDeathCause.ROADKILL, ELimb.SPINE, !this.vehicle.isDriven ? CSteamID.Nil : this.vehicle.passengers[0].player.playerID.steamID, this.transform.forward, Bumper.DAMAGE_PLAYER, times, out kill);
        EffectManager.sendEffect((ushort) 5, EffectManager.SMALL, other.transform.position + other.transform.up, -this.transform.forward);
        this.vehicle.askDamage((ushort) 2, true);
      }
      else if (other.transform.tag == "Agent")
      {
        Zombie zombie = DamageTool.getZombie(other.transform);
        if ((Object) zombie != (Object) null)
        {
          EPlayerKill kill;
          DamageTool.damage(zombie, this.transform.forward, Bumper.DAMAGE_ZOMBIE, times, out kill);
          EffectManager.sendEffect((ushort) 5, EffectManager.SMALL, other.transform.position + other.transform.up, -this.transform.forward);
          this.vehicle.askDamage((ushort) 2, true);
        }
        else
        {
          Animal animal = DamageTool.getAnimal(other.transform);
          if (!((Object) animal != (Object) null))
            return;
          EPlayerKill kill;
          DamageTool.damage(animal, this.transform.forward, Bumper.DAMAGE_ANIMAL, times, out kill);
          EffectManager.sendEffect((ushort) 5, EffectManager.SMALL, other.transform.position + other.transform.up, -this.transform.forward);
          this.vehicle.askDamage((ushort) 2, true);
        }
      }
      else
      {
        if (other.transform.tag == "Barricade" || other.transform.tag == "Structure")
          return;
        if (other.transform.tag == "Resource")
        {
          DamageTool.impact(this.transform.position + this.transform.forward * ((BoxCollider) this.transform.GetComponent<Collider>()).size.z / 2f, -this.transform.forward, DamageTool.getMaterial(this.transform.position, other.transform, other.GetComponent<Collider>()), true);
          EPlayerKill kill;
          ResourceManager.damage(other.transform, this.transform.forward, Bumper.DAMAGE_RESOURCE, times, 1f, out kill);
          this.vehicle.askDamage((ushort) ((double) Bumper.DAMAGE_VEHICLE * (double) times), true);
        }
        else
        {
          ushort id = LevelObjects.getID(other.transform);
          if ((int) id == 0)
            return;
          ObjectAsset objectAsset = (ObjectAsset) Assets.find(EAssetType.OBJECT, id);
          if (objectAsset == null || objectAsset.isSoft)
            return;
          DamageTool.impact(this.transform.position + this.transform.forward * ((BoxCollider) this.transform.GetComponent<Collider>()).size.z / 2f, -this.transform.forward, DamageTool.getMaterial(this.transform.position, other.transform, other.GetComponent<Collider>()), true);
          this.vehicle.askDamage((ushort) ((double) Bumper.DAMAGE_VEHICLE * (double) times), true);
        }
      }
    }
  }
}
