// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.WalkingPlayerInputPacket
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
  public class WalkingPlayerInputPacket : PlayerInputPacket
  {
    public List<RaycastInfo> clientsideInputs;
    public Queue<InputInfo> serversideInputs;
    public byte keys;
    public Vector3 position;
    public float yaw;
    public float pitch;

    public override void read(SteamChannel channel)
    {
      base.read(channel);
      this.keys = (byte) channel.read(Types.BYTE_TYPE);
      this.position = (Vector3) channel.read(Types.VECTOR3_TYPE);
      this.yaw = (float) channel.read(Types.SINGLE_TYPE);
      this.pitch = (float) channel.read(Types.SINGLE_TYPE);
      byte num1 = (byte) channel.read(Types.BYTE_TYPE);
      if ((int) num1 <= 0)
        return;
      this.serversideInputs = new Queue<InputInfo>();
      for (byte index = (byte) 0; (int) index < (int) num1; ++index)
      {
        ERaycastInfoType eraycastInfoType = (ERaycastInfoType) (byte) channel.read(Types.BYTE_TYPE);
        InputInfo inputInfo = new InputInfo();
        switch (eraycastInfoType)
        {
          case ERaycastInfoType.NONE:
            inputInfo.point = (Vector3) channel.read(Types.VECTOR3_TYPE);
            inputInfo.normal = (Vector3) channel.read(Types.VECTOR3_TYPE);
            inputInfo.material = (EPhysicsMaterial) (byte) channel.read(Types.BYTE_TYPE);
            break;
          case ERaycastInfoType.PLAYER:
            inputInfo.point = (Vector3) channel.read(Types.VECTOR3_TYPE);
            inputInfo.direction = (Vector3) channel.read(Types.VECTOR3_TYPE);
            inputInfo.normal = (Vector3) channel.read(Types.VECTOR3_TYPE);
            inputInfo.limb = (ELimb) (byte) channel.read(Types.BYTE_TYPE);
            Player player = PlayerTool.getPlayer((CSteamID) channel.read(Types.STEAM_ID_TYPE));
            if ((Object) player != (Object) null && (double) (inputInfo.point - player.transform.position).sqrMagnitude < 256.0)
            {
              inputInfo.material = EPhysicsMaterial.FLESH_DYNAMIC;
              inputInfo.player = player;
              inputInfo.transform = player.transform;
              break;
            }
            inputInfo = (InputInfo) null;
            break;
          case ERaycastInfoType.ZOMBIE:
            inputInfo.point = (Vector3) channel.read(Types.VECTOR3_TYPE);
            inputInfo.direction = (Vector3) channel.read(Types.VECTOR3_TYPE);
            inputInfo.normal = (Vector3) channel.read(Types.VECTOR3_TYPE);
            inputInfo.limb = (ELimb) (byte) channel.read(Types.BYTE_TYPE);
            ushort id = (ushort) channel.read(Types.UINT16_TYPE);
            Zombie zombie = ZombieManager.getZombie(inputInfo.point, id);
            if ((Object) zombie != (Object) null && (double) (inputInfo.point - zombie.transform.position).sqrMagnitude < 256.0)
            {
              inputInfo.material = EPhysicsMaterial.FLESH_DYNAMIC;
              inputInfo.zombie = zombie;
              inputInfo.transform = zombie.transform;
              break;
            }
            inputInfo = (InputInfo) null;
            break;
          case ERaycastInfoType.ANIMAL:
            inputInfo.point = (Vector3) channel.read(Types.VECTOR3_TYPE);
            inputInfo.direction = (Vector3) channel.read(Types.VECTOR3_TYPE);
            inputInfo.normal = (Vector3) channel.read(Types.VECTOR3_TYPE);
            inputInfo.limb = (ELimb) (byte) channel.read(Types.BYTE_TYPE);
            Animal animal = AnimalManager.getAnimal((ushort) channel.read(Types.UINT16_TYPE));
            if ((Object) animal != (Object) null && (double) (inputInfo.point - animal.transform.position).sqrMagnitude < 256.0)
            {
              inputInfo.material = EPhysicsMaterial.FLESH_DYNAMIC;
              inputInfo.animal = animal;
              inputInfo.transform = animal.transform;
              break;
            }
            inputInfo = (InputInfo) null;
            break;
          case ERaycastInfoType.VEHICLE:
            inputInfo.point = (Vector3) channel.read(Types.VECTOR3_TYPE);
            inputInfo.normal = (Vector3) channel.read(Types.VECTOR3_TYPE);
            inputInfo.material = (EPhysicsMaterial) (byte) channel.read(Types.BYTE_TYPE);
            InteractableVehicle vehicle = VehicleManager.getVehicle((ushort) channel.read(Types.UINT16_TYPE));
            if ((Object) vehicle != (Object) null && (double) (inputInfo.point - vehicle.transform.position).sqrMagnitude < 4096.0)
            {
              inputInfo.vehicle = vehicle;
              inputInfo.transform = vehicle.transform;
              break;
            }
            inputInfo = (InputInfo) null;
            break;
          case ERaycastInfoType.BARRICADE:
            inputInfo.point = (Vector3) channel.read(Types.VECTOR3_TYPE);
            inputInfo.normal = (Vector3) channel.read(Types.VECTOR3_TYPE);
            inputInfo.material = (EPhysicsMaterial) (byte) channel.read(Types.BYTE_TYPE);
            byte x1 = (byte) channel.read(Types.BYTE_TYPE);
            byte y1 = (byte) channel.read(Types.BYTE_TYPE);
            ushort plant = (ushort) channel.read(Types.UINT16_TYPE);
            ushort num2 = (ushort) channel.read(Types.UINT16_TYPE);
            BarricadeRegion region1;
            if (BarricadeManager.tryGetRegion(x1, y1, plant, out region1) && (int) num2 < region1.models.Count)
            {
              Transform transform = region1.models[(int) num2];
              if ((Object) transform != (Object) null && (double) (inputInfo.point - transform.transform.position).sqrMagnitude < 256.0)
              {
                inputInfo.transform = transform;
                break;
              }
              inputInfo = (InputInfo) null;
              break;
            }
            inputInfo = (InputInfo) null;
            break;
          case ERaycastInfoType.STRUCTURE:
            inputInfo.point = (Vector3) channel.read(Types.VECTOR3_TYPE);
            inputInfo.direction = (Vector3) channel.read(Types.VECTOR3_TYPE);
            inputInfo.normal = (Vector3) channel.read(Types.VECTOR3_TYPE);
            inputInfo.material = (EPhysicsMaterial) (byte) channel.read(Types.BYTE_TYPE);
            byte x2 = (byte) channel.read(Types.BYTE_TYPE);
            byte y2 = (byte) channel.read(Types.BYTE_TYPE);
            ushort num3 = (ushort) channel.read(Types.UINT16_TYPE);
            StructureRegion region2;
            if (StructureManager.tryGetRegion(x2, y2, out region2) && (int) num3 < region2.models.Count)
            {
              Transform transform = region2.models[(int) num3];
              if ((Object) transform != (Object) null && (double) (inputInfo.point - transform.transform.position).sqrMagnitude < 256.0)
              {
                inputInfo.transform = transform;
                break;
              }
              inputInfo = (InputInfo) null;
              break;
            }
            inputInfo = (InputInfo) null;
            break;
          case ERaycastInfoType.RESOURCE:
            inputInfo.point = (Vector3) channel.read(Types.VECTOR3_TYPE);
            inputInfo.direction = (Vector3) channel.read(Types.VECTOR3_TYPE);
            inputInfo.normal = (Vector3) channel.read(Types.VECTOR3_TYPE);
            inputInfo.material = (EPhysicsMaterial) (byte) channel.read(Types.BYTE_TYPE);
            Transform resource = ResourceManager.getResource((byte) channel.read(Types.BYTE_TYPE), (byte) channel.read(Types.BYTE_TYPE), (ushort) channel.read(Types.UINT16_TYPE));
            if ((Object) resource != (Object) null && (double) (inputInfo.point - resource.transform.position).sqrMagnitude < 256.0)
            {
              inputInfo.transform = resource;
              break;
            }
            inputInfo = (InputInfo) null;
            break;
        }
        if (inputInfo != null)
          this.serversideInputs.Enqueue(inputInfo);
      }
    }

    public override void write(SteamChannel channel)
    {
      base.write(channel);
      channel.write((object) this.keys);
      channel.write((object) this.position);
      channel.write((object) this.yaw);
      channel.write((object) this.pitch);
      if (this.clientsideInputs == null)
      {
        channel.write((object) 0);
      }
      else
      {
        channel.write((object) (byte) this.clientsideInputs.Count);
        using (List<RaycastInfo>.Enumerator enumerator = this.clientsideInputs.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            RaycastInfo current = enumerator.Current;
            if ((Object) current.player != (Object) null)
            {
              channel.write((object) 1);
              channel.write((object) current.point);
              channel.write((object) current.direction);
              channel.write((object) current.normal);
              channel.write((object) (byte) current.limb);
              channel.write((object) current.player.channel.owner.playerID.steamID);
            }
            else if ((Object) current.zombie != (Object) null)
            {
              channel.write((object) 2);
              channel.write((object) current.point);
              channel.write((object) current.direction);
              channel.write((object) current.normal);
              channel.write((object) (byte) current.limb);
              channel.write((object) current.zombie.id);
            }
            else if ((Object) current.animal != (Object) null)
            {
              channel.write((object) 3);
              channel.write((object) current.point);
              channel.write((object) current.direction);
              channel.write((object) current.normal);
              channel.write((object) (byte) current.limb);
              channel.write((object) current.animal.index);
            }
            else if ((Object) current.vehicle != (Object) null)
            {
              channel.write((object) 4);
              channel.write((object) current.point);
              channel.write((object) current.normal);
              channel.write((object) (byte) current.material);
              channel.write((object) current.vehicle.index);
            }
            else if ((Object) current.transform != (Object) null)
            {
              if (current.transform.tag == "Barricade")
              {
                channel.write((object) 5);
                byte x;
                byte y;
                ushort plant;
                ushort index;
                BarricadeRegion region;
                if (BarricadeManager.tryGetInfo(!(current.transform.name == "Hinge") ? current.transform : current.transform.parent.parent, out x, out y, out plant, out index, out region))
                {
                  channel.write((object) current.point);
                  channel.write((object) current.normal);
                  channel.write((object) (byte) current.material);
                  channel.write((object) x);
                  channel.write((object) y);
                  channel.write((object) plant);
                  channel.write((object) index);
                }
              }
              else if (current.transform.tag == "Structure")
              {
                channel.write((object) 6);
                byte x;
                byte y;
                ushort index;
                StructureRegion region;
                if (StructureManager.tryGetInfo(current.transform, out x, out y, out index, out region))
                {
                  channel.write((object) current.point);
                  channel.write((object) current.direction);
                  channel.write((object) current.normal);
                  channel.write((object) (byte) current.material);
                  channel.write((object) x);
                  channel.write((object) y);
                  channel.write((object) index);
                }
              }
              else if (current.transform.tag == "Resource")
              {
                channel.write((object) 7);
                byte x;
                byte y;
                ushort index;
                if (ResourceManager.tryGetRegion(current.transform, out x, out y, out index))
                {
                  channel.write((object) current.point);
                  channel.write((object) current.direction);
                  channel.write((object) current.normal);
                  channel.write((object) (byte) current.material);
                  channel.write((object) x);
                  channel.write((object) y);
                  channel.write((object) index);
                }
              }
              else
              {
                channel.write((object) 0);
                channel.write((object) current.point);
                channel.write((object) current.normal);
                channel.write((object) (byte) current.material);
              }
            }
          }
        }
      }
    }
  }
}
