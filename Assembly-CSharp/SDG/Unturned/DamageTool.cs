// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.DamageTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
  public class DamageTool
  {
    public static ELimb getLimb(Transform limb)
    {
      if (limb.tag == "Player" || limb.tag == "Enemy" || (limb.tag == "Zombie" || limb.tag == "Animal"))
      {
        string name = limb.name;
        if (name != null)
        {
          // ISSUE: reference to a compiler-generated field
          if (DamageTool.\u003C\u003Ef__switch\u0024map3 == null)
          {
            // ISSUE: reference to a compiler-generated field
            DamageTool.\u003C\u003Ef__switch\u0024map3 = new Dictionary<string, int>(14)
            {
              {
                "Left_Foot",
                0
              },
              {
                "Left_Leg",
                1
              },
              {
                "Right_Foot",
                2
              },
              {
                "Right_Leg",
                3
              },
              {
                "Left_Hand",
                4
              },
              {
                "Left_Arm",
                5
              },
              {
                "Right_Hand",
                6
              },
              {
                "Right_Arm",
                7
              },
              {
                "Left_Back",
                8
              },
              {
                "Right_Back",
                9
              },
              {
                "Left_Front",
                10
              },
              {
                "Right_Front",
                11
              },
              {
                "Spine",
                12
              },
              {
                "Skull",
                13
              }
            };
          }
          int num;
          // ISSUE: reference to a compiler-generated field
          if (DamageTool.\u003C\u003Ef__switch\u0024map3.TryGetValue(name, out num))
          {
            switch (num)
            {
              case 0:
                return ELimb.LEFT_FOOT;
              case 1:
                return ELimb.LEFT_LEG;
              case 2:
                return ELimb.RIGHT_FOOT;
              case 3:
                return ELimb.RIGHT_LEG;
              case 4:
                return ELimb.LEFT_HAND;
              case 5:
                return ELimb.LEFT_ARM;
              case 6:
                return ELimb.RIGHT_HAND;
              case 7:
                return ELimb.RIGHT_ARM;
              case 8:
                return ELimb.LEFT_BACK;
              case 9:
                return ELimb.RIGHT_BACK;
              case 10:
                return ELimb.LEFT_FRONT;
              case 11:
                return ELimb.RIGHT_FRONT;
              case 12:
                return ELimb.SPINE;
              case 13:
                return ELimb.SKULL;
            }
          }
        }
      }
      return ELimb.SPINE;
    }

    public static Player getPlayer(Transform limb)
    {
      Player player = (Player) null;
      if (limb.tag == "Player")
      {
        player = limb.GetComponent<Player>();
      }
      else
      {
        string name = limb.name;
        if (name != null)
        {
          // ISSUE: reference to a compiler-generated field
          if (DamageTool.\u003C\u003Ef__switch\u0024map4 == null)
          {
            // ISSUE: reference to a compiler-generated field
            DamageTool.\u003C\u003Ef__switch\u0024map4 = new Dictionary<string, int>(10)
            {
              {
                "Left_Foot",
                0
              },
              {
                "Left_Leg",
                1
              },
              {
                "Right_Foot",
                2
              },
              {
                "Right_Leg",
                3
              },
              {
                "Left_Hand",
                4
              },
              {
                "Left_Arm",
                5
              },
              {
                "Right_Hand",
                6
              },
              {
                "Right_Arm",
                7
              },
              {
                "Spine",
                8
              },
              {
                "Skull",
                9
              }
            };
          }
          int num;
          // ISSUE: reference to a compiler-generated field
          if (DamageTool.\u003C\u003Ef__switch\u0024map4.TryGetValue(name, out num))
          {
            switch (num)
            {
              case 0:
                player = limb.parent.parent.parent.parent.parent.GetComponent<Player>();
                break;
              case 1:
                player = limb.parent.parent.parent.parent.GetComponent<Player>();
                break;
              case 2:
                player = limb.parent.parent.parent.parent.parent.GetComponent<Player>();
                break;
              case 3:
                player = limb.parent.parent.parent.parent.GetComponent<Player>();
                break;
              case 4:
                player = limb.parent.parent.parent.parent.parent.parent.GetComponent<Player>();
                break;
              case 5:
                player = limb.parent.parent.parent.parent.parent.GetComponent<Player>();
                break;
              case 6:
                player = limb.parent.parent.parent.parent.parent.parent.GetComponent<Player>();
                break;
              case 7:
                player = limb.parent.parent.parent.parent.parent.GetComponent<Player>();
                break;
              case 8:
                player = limb.parent.parent.parent.GetComponent<Player>();
                break;
              case 9:
                player = limb.parent.parent.parent.parent.GetComponent<Player>();
                break;
            }
          }
        }
      }
      if ((Object) player != (Object) null && player.life.isDead)
        player = (Player) null;
      return player;
    }

    public static Zombie getZombie(Transform limb)
    {
      Zombie zombie = (Zombie) null;
      if (limb.tag == "Agent")
      {
        zombie = limb.GetComponent<Zombie>();
      }
      else
      {
        string name = limb.name;
        if (name != null)
        {
          // ISSUE: reference to a compiler-generated field
          if (DamageTool.\u003C\u003Ef__switch\u0024map5 == null)
          {
            // ISSUE: reference to a compiler-generated field
            DamageTool.\u003C\u003Ef__switch\u0024map5 = new Dictionary<string, int>(10)
            {
              {
                "Left_Foot",
                0
              },
              {
                "Left_Leg",
                1
              },
              {
                "Right_Foot",
                2
              },
              {
                "Right_Leg",
                3
              },
              {
                "Left_Hand",
                4
              },
              {
                "Left_Arm",
                5
              },
              {
                "Right_Hand",
                6
              },
              {
                "Right_Arm",
                7
              },
              {
                "Spine",
                8
              },
              {
                "Skull",
                9
              }
            };
          }
          int num;
          // ISSUE: reference to a compiler-generated field
          if (DamageTool.\u003C\u003Ef__switch\u0024map5.TryGetValue(name, out num))
          {
            switch (num)
            {
              case 0:
                zombie = limb.parent.parent.parent.parent.parent.GetComponent<Zombie>();
                break;
              case 1:
                zombie = limb.parent.parent.parent.parent.GetComponent<Zombie>();
                break;
              case 2:
                zombie = limb.parent.parent.parent.parent.parent.GetComponent<Zombie>();
                break;
              case 3:
                zombie = limb.parent.parent.parent.parent.GetComponent<Zombie>();
                break;
              case 4:
                zombie = limb.parent.parent.parent.parent.parent.parent.GetComponent<Zombie>();
                break;
              case 5:
                zombie = limb.parent.parent.parent.parent.parent.GetComponent<Zombie>();
                break;
              case 6:
                zombie = limb.parent.parent.parent.parent.parent.parent.GetComponent<Zombie>();
                break;
              case 7:
                zombie = limb.parent.parent.parent.parent.parent.GetComponent<Zombie>();
                break;
              case 8:
                zombie = limb.parent.parent.parent.GetComponent<Zombie>();
                break;
              case 9:
                zombie = limb.parent.parent.parent.parent.GetComponent<Zombie>();
                break;
            }
          }
        }
      }
      if ((Object) zombie != (Object) null && zombie.isDead)
        zombie = (Zombie) null;
      return zombie;
    }

    public static Animal getAnimal(Transform limb)
    {
      Animal animal = (Animal) null;
      if (limb.tag == "Agent")
      {
        animal = limb.GetComponent<Animal>();
      }
      else
      {
        string name = limb.name;
        if (name != null)
        {
          // ISSUE: reference to a compiler-generated field
          if (DamageTool.\u003C\u003Ef__switch\u0024map6 == null)
          {
            // ISSUE: reference to a compiler-generated field
            DamageTool.\u003C\u003Ef__switch\u0024map6 = new Dictionary<string, int>(6)
            {
              {
                "Left_Back",
                0
              },
              {
                "Right_Back",
                1
              },
              {
                "Left_Front",
                2
              },
              {
                "Right_Front",
                3
              },
              {
                "Spine",
                4
              },
              {
                "Skull",
                5
              }
            };
          }
          int num;
          // ISSUE: reference to a compiler-generated field
          if (DamageTool.\u003C\u003Ef__switch\u0024map6.TryGetValue(name, out num))
          {
            switch (num)
            {
              case 0:
                animal = limb.parent.parent.parent.GetComponent<Animal>();
                break;
              case 1:
                animal = limb.parent.parent.parent.GetComponent<Animal>();
                break;
              case 2:
                animal = limb.parent.parent.parent.parent.GetComponent<Animal>();
                break;
              case 3:
                animal = limb.parent.parent.parent.parent.GetComponent<Animal>();
                break;
              case 4:
                animal = limb.parent.parent.parent.GetComponent<Animal>();
                break;
              case 5:
                animal = limb.parent.parent.parent.parent.GetComponent<Animal>();
                break;
            }
          }
        }
      }
      if ((Object) animal != (Object) null && animal.isDead)
        animal = (Animal) null;
      return animal;
    }

    public static InteractableVehicle getVehicle(Transform model)
    {
      return model.GetComponent<InteractableVehicle>();
    }

    public static void damage(Player player, EDeathCause cause, ELimb limb, CSteamID killer, Vector3 direction, float damage, float times, out EPlayerKill kill)
    {
      if ((Object) player == (Object) null)
      {
        kill = EPlayerKill.NONE;
      }
      else
      {
        byte amount = (byte) ((double) damage * (double) times);
        player.life.askDamage(amount, direction * (float) amount, cause, limb, killer, out kill);
      }
    }

    public static void damage(Player player, EDeathCause cause, ELimb limb, CSteamID killer, Vector3 direction, PlayerDamageMultiplier multiplier, float times, bool armor, out EPlayerKill kill)
    {
      if ((Object) player == (Object) null)
      {
        kill = EPlayerKill.NONE;
      }
      else
      {
        if (armor)
          times *= multiplier.armor(limb, player);
        DamageTool.damage(player, cause, limb, killer, direction, multiplier.multiply(limb), times, out kill);
      }
    }

    public static void damage(Zombie zombie, Vector3 direction, float damage, float times, out EPlayerKill kill)
    {
      if ((Object) zombie == (Object) null)
      {
        kill = EPlayerKill.NONE;
      }
      else
      {
        byte amount = (byte) ((double) damage * (double) times);
        zombie.askDamage(amount, direction * (float) amount, out kill);
      }
    }

    public static void damage(Zombie zombie, ELimb limb, Vector3 direction, ZombieDamageMultiplier multiplier, float times, bool armor, out EPlayerKill kill)
    {
      if ((Object) zombie == (Object) null)
      {
        kill = EPlayerKill.NONE;
      }
      else
      {
        if (armor)
        {
          times *= multiplier.armor(limb, zombie);
          if ((double) Vector3.Dot(zombie.transform.forward, direction) > 0.5)
            times *= 1.25f;
        }
        DamageTool.damage(zombie, direction, multiplier.multiply(limb), times, out kill);
      }
    }

    public static void damage(Animal animal, Vector3 direction, float damage, float times, out EPlayerKill kill)
    {
      if ((Object) animal == (Object) null)
      {
        kill = EPlayerKill.NONE;
      }
      else
      {
        byte amount = (byte) ((double) damage * (double) times);
        animal.askDamage(amount, direction * (float) amount, out kill);
      }
    }

    public static void damage(Animal animal, ELimb limb, Vector3 direction, AnimalDamageMultiplier multiplier, float times, out EPlayerKill kill)
    {
      if ((Object) animal == (Object) null)
        kill = EPlayerKill.NONE;
      else
        DamageTool.damage(animal, direction, multiplier.multiply(limb), times, out kill);
    }

    public static void damage(InteractableVehicle vehicle, bool isRepairing, float vehicleDamage, float times, bool canRepair, out EPlayerKill kill)
    {
      kill = EPlayerKill.NONE;
      if ((Object) vehicle == (Object) null)
        return;
      if (isRepairing)
      {
        if (vehicle.isExploded || vehicle.isRepaired)
          return;
        VehicleManager.repair(vehicle, vehicleDamage, times);
      }
      else
      {
        if (vehicle.isDead)
          return;
        VehicleManager.damage(vehicle, vehicleDamage, times, canRepair);
      }
    }

    public static void damage(Transform barricade, float barricadeDamage, float times, out EPlayerKill kill)
    {
      kill = EPlayerKill.NONE;
      if ((Object) barricade == (Object) null)
        return;
      BarricadeManager.damage(barricade, barricadeDamage, times);
    }

    public static void damage(Transform structure, Vector3 direction, float structureDamage, float times, out EPlayerKill kill)
    {
      kill = EPlayerKill.NONE;
      if ((Object) structure == (Object) null)
        return;
      StructureManager.damage(structure, direction, structureDamage, times);
    }

    public static void damage(Transform resource, Vector3 direction, float resourceDamage, float times, float drops, out EPlayerKill kill)
    {
      if ((Object) resource == (Object) null)
        kill = EPlayerKill.NONE;
      else
        ResourceManager.damage(resource, direction, resourceDamage, times, drops, out kill);
    }

    public static void explode(Vector3 point, float radius, EDeathCause cause, float playerDamage, float zombieDamage, float animalDamage, float barricadeDamage, float structureDamage, float vehicleDamage, float resourceDamage)
    {
      Collider[] colliderArray = Physics.OverlapSphere(point, radius, RayMasks.DAMAGE_EXPLOSION);
      for (int index = 0; index < colliderArray.Length; ++index)
      {
        Vector3 direction = colliderArray[index].transform.position + Vector3.up - point;
        float magnitude = direction.magnitude;
        if ((double) magnitude <= (double) radius)
        {
          if ((double) magnitude > 1.0 && colliderArray[index].tag != "Resource")
          {
            RaycastHit hitInfo;
            Physics.Raycast(point, direction, out hitInfo, magnitude - 1f, RayMasks.BLOCK_EXPLOSION);
            if ((Object) hitInfo.transform != (Object) null && (Object) hitInfo.transform != (Object) colliderArray[index].transform)
              continue;
          }
          EPlayerKill kill;
          if (colliderArray[index].tag == "Player")
          {
            if (Provider.isPvP)
            {
              Vector3 normalized = direction.normalized;
              EffectManager.sendEffect((ushort) 5, EffectManager.SMALL, colliderArray[index].transform.position + Vector3.up, -normalized);
              EffectManager.sendEffect((ushort) 5, EffectManager.SMALL, colliderArray[index].transform.position + Vector3.up, Vector3.up);
              DamageTool.damage(DamageTool.getPlayer(colliderArray[index].transform), cause, ELimb.SPINE, Provider.server, normalized, playerDamage, 1f - Mathf.Pow(magnitude / radius, 2f), out kill);
            }
          }
          else if (colliderArray[index].tag == "Agent")
          {
            Vector3 normalized = direction.normalized;
            EffectManager.sendEffect((ushort) 5, EffectManager.SMALL, colliderArray[index].transform.position + Vector3.up, -normalized);
            EffectManager.sendEffect((ushort) 5, EffectManager.SMALL, colliderArray[index].transform.position + Vector3.up, Vector3.up);
            Zombie zombie = DamageTool.getZombie(colliderArray[index].transform);
            if ((Object) zombie != (Object) null)
            {
              DamageTool.damage(zombie, normalized, zombieDamage, (float) (1.0 - (double) magnitude / (double) radius), out kill);
            }
            else
            {
              Animal animal = DamageTool.getAnimal(colliderArray[index].transform);
              if ((Object) animal != (Object) null)
                DamageTool.damage(animal, normalized, animalDamage, (float) (1.0 - (double) magnitude / (double) radius), out kill);
            }
          }
          else if (colliderArray[index].tag == "Vehicle")
            VehicleManager.damage(DamageTool.getVehicle(colliderArray[index].transform), vehicleDamage, (float) (1.0 - (double) magnitude / (double) radius), false);
          else if (colliderArray[index].tag == "Barricade")
          {
            if (colliderArray[index].name == "Hinge")
              BarricadeManager.damage(colliderArray[index].transform.parent.parent, barricadeDamage, (float) (1.0 - (double) magnitude / (double) radius));
            else
              BarricadeManager.damage(colliderArray[index].transform, barricadeDamage, (float) (1.0 - (double) magnitude / (double) radius));
          }
          else if (colliderArray[index].tag == "Structure")
            StructureManager.damage(colliderArray[index].transform, direction.normalized, structureDamage, (float) (1.0 - (double) magnitude / (double) radius));
          else if (colliderArray[index].tag == "Resource")
            ResourceManager.damage(colliderArray[index].transform, direction.normalized, resourceDamage, (float) (1.0 - (double) magnitude / (double) radius), 1f, out kill);
        }
      }
      AlertTool.alert(point, 32f);
    }

    public static EPhysicsMaterial getMaterial(Vector3 point, Transform transform, Collider collider)
    {
      if ((double) LevelLighting.seaLevel < 0.990000009536743 && (double) point.y < (double) LevelLighting.seaLevel * (double) Level.TERRAIN)
        return EPhysicsMaterial.WATER_STATIC;
      if (transform.tag == "Ground")
        return PhysicsTool.checkMaterial(point);
      return PhysicsTool.checkMaterial(collider);
    }

    public static void impact(Vector3 point, Vector3 normal, EPhysicsMaterial material, bool forceDynamic)
    {
      if (material == EPhysicsMaterial.NONE)
        return;
      if (material == EPhysicsMaterial.CLOTH_DYNAMIC || material == EPhysicsMaterial.TILE_DYNAMIC || material == EPhysicsMaterial.CONCRETE_DYNAMIC)
        EffectManager.sendEffect((ushort) 38, EffectManager.SMALL, point + normal * Random.Range(0.04f, 0.06f), normal);
      else if (material == EPhysicsMaterial.CLOTH_STATIC || material == EPhysicsMaterial.TILE_STATIC || material == EPhysicsMaterial.CONCRETE_STATIC)
        EffectManager.sendEffect(!forceDynamic ? (ushort) 13 : (ushort) 38, EffectManager.SMALL, point + normal * Random.Range(0.04f, 0.06f), normal);
      else if (material == EPhysicsMaterial.FLESH_DYNAMIC)
        EffectManager.sendEffect((ushort) 5, EffectManager.SMALL, point + normal * Random.Range(0.04f, 0.06f), normal);
      else if (material == EPhysicsMaterial.GRAVEL_DYNAMIC)
        EffectManager.sendEffect((ushort) 44, EffectManager.SMALL, point + normal * Random.Range(0.04f, 0.06f), normal);
      else if (material == EPhysicsMaterial.GRAVEL_STATIC)
        EffectManager.sendEffect(!forceDynamic ? (ushort) 14 : (ushort) 44, EffectManager.SMALL, point + normal * Random.Range(0.04f, 0.06f), normal);
      else if (material == EPhysicsMaterial.METAL_DYNAMIC)
        EffectManager.sendEffect((ushort) 18, EffectManager.SMALL, point + normal * Random.Range(0.04f, 0.06f), normal);
      else if (material == EPhysicsMaterial.METAL_STATIC)
        EffectManager.sendEffect(!forceDynamic ? (ushort) 12 : (ushort) 18, EffectManager.SMALL, point + normal * Random.Range(0.04f, 0.06f), normal);
      else if (material == EPhysicsMaterial.WOOD_DYNAMIC)
        EffectManager.sendEffect((ushort) 17, EffectManager.SMALL, point + normal * Random.Range(0.04f, 0.06f), normal);
      else if (material == EPhysicsMaterial.WOOD_STATIC)
        EffectManager.sendEffect(!forceDynamic ? (ushort) 2 : (ushort) 17, EffectManager.SMALL, point + normal * Random.Range(0.04f, 0.06f), normal);
      else if (material == EPhysicsMaterial.FOLIAGE_STATIC)
        EffectManager.sendEffect((ushort) 15, EffectManager.SMALL, point + normal * Random.Range(0.04f, 0.06f), normal);
      else if (material == EPhysicsMaterial.SNOW_STATIC || material == EPhysicsMaterial.ICE_STATIC)
      {
        EffectManager.sendEffect((ushort) 41, EffectManager.SMALL, point + normal * Random.Range(0.04f, 0.06f), normal);
      }
      else
      {
        if (material != EPhysicsMaterial.WATER_STATIC)
          return;
        EffectManager.sendEffect((ushort) 16, EffectManager.SMALL, point + normal * Random.Range(0.04f, 0.06f), normal);
      }
    }

    public static RaycastInfo raycast(Ray ray, float range, int mask)
    {
      RaycastHit hitInfo;
      Physics.Raycast(ray, out hitInfo, range, mask);
      RaycastInfo raycastInfo = new RaycastInfo(hitInfo);
      raycastInfo.direction = ray.direction;
      if ((Object) hitInfo.transform != (Object) null)
      {
        if (hitInfo.transform.tag == "Enemy")
          raycastInfo.player = DamageTool.getPlayer(raycastInfo.transform);
        if (hitInfo.transform.tag == "Zombie")
          raycastInfo.zombie = DamageTool.getZombie(raycastInfo.transform);
        if (hitInfo.transform.tag == "Animal")
          raycastInfo.animal = DamageTool.getAnimal(raycastInfo.transform);
        raycastInfo.limb = DamageTool.getLimb(raycastInfo.transform);
        if (hitInfo.transform.tag == "Vehicle")
          raycastInfo.vehicle = DamageTool.getVehicle(raycastInfo.transform);
        raycastInfo.material = DamageTool.getMaterial(hitInfo.point, hitInfo.transform, hitInfo.collider);
      }
      return raycastInfo;
    }
  }
}
