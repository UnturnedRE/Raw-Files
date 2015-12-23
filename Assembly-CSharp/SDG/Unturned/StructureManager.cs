// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.StructureManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
  public class StructureManager : SteamCaller
  {
    public static readonly byte SAVEDATA_VERSION = (byte) 3;
    public static readonly byte STRUCTURE_REGIONS = (byte) 2;
    public static readonly float WALL = 3f;
    public static readonly float PILLAR = 3.1f;
    public static readonly float HEIGHT = 2.125f;
    private static StructureManager manager;
    private static StructureRegion[,] regions;

    public static void salvageStructure(Transform structure)
    {
      byte x;
      byte y;
      ushort index;
      StructureRegion region;
      if (!StructureManager.tryGetInfo(structure, out x, out y, out index, out region))
        return;
      StructureManager.manager.channel.send("askSalvageStructure", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[3]
      {
        (object) x,
        (object) y,
        (object) index
      });
    }

    [SteamCall]
    public void askSalvageStructure(CSteamID steamID, byte x, byte y, ushort index)
    {
      StructureRegion region;
      if (!Provider.isServer || !StructureManager.tryGetRegion(x, y, out region))
        return;
      Player player = PlayerTool.getPlayer(steamID);
      if ((Object) player == (Object) null || player.life.isDead || ((int) index >= region.models.Count || !OwnershipTool.checkToggle(player.channel.owner.playerID.steamID, region.structures[(int) index].owner, player.channel.owner.playerID.group, region.structures[(int) index].group)))
        return;
      ItemStructureAsset itemStructureAsset = (ItemStructureAsset) Assets.find(EAssetType.ITEM, region.structures[(int) index].structure.id);
      if (itemStructureAsset != null)
      {
        if ((int) region.structures[(int) index].structure.health == (int) itemStructureAsset.health)
        {
          player.inventory.forceAddItem(new Item(region.structures[(int) index].structure.id, true), true);
        }
        else
        {
          for (int index1 = 0; index1 < itemStructureAsset.blueprints.Length; ++index1)
          {
            Blueprint blueprint = itemStructureAsset.blueprints[index1];
            if ((int) blueprint.product == (int) itemStructureAsset.id)
            {
              ushort id = blueprint.supplies[Random.Range(0, blueprint.supplies.Length)].id;
              player.inventory.forceAddItem(new Item(id, true), true);
              break;
            }
          }
        }
      }
      region.structures.RemoveAt((int) index);
      StructureManager.manager.channel.send("tellTakeStructure", ESteamCall.ALL, x, y, StructureManager.STRUCTURE_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, (object) x, (object) y, (object) index);
    }

    public static void damage(Transform structure, Vector3 direction, float damage, float times)
    {
      byte x;
      byte y;
      ushort index;
      StructureRegion region;
      if (!StructureManager.tryGetInfo(structure, out x, out y, out index, out region) || region.structures[(int) index].structure.isDead)
        return;
      ushort amount = (ushort) ((double) damage * (double) times);
      region.structures[(int) index].structure.askDamage(amount);
      if (!region.structures[(int) index].structure.isDead)
        return;
      ItemStructureAsset itemStructureAsset = (ItemStructureAsset) Assets.find(EAssetType.ITEM, region.structures[(int) index].structure.id);
      if (itemStructureAsset != null && (int) itemStructureAsset.explosion != 0)
        EffectManager.sendEffect(itemStructureAsset.explosion, EffectManager.SMALL, structure.position + Vector3.down * StructureManager.HEIGHT);
      region.structures.RemoveAt((int) index);
      StructureManager.manager.channel.send("tellTakeStructure", ESteamCall.ALL, x, y, StructureManager.STRUCTURE_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, (object) x, (object) y, (object) index, (object) (direction * (float) amount));
    }

    public static bool tryGetInfo(Transform structure, out byte x, out byte y, out ushort index, out StructureRegion region)
    {
      x = (byte) 0;
      y = (byte) 0;
      index = (ushort) 0;
      region = (StructureRegion) null;
      if (StructureManager.tryGetRegion(structure, out x, out y, out region))
      {
        index = (ushort) 0;
        while ((int) index < region.models.Count)
        {
          if ((Object) structure == (Object) region.models[(int) index])
            return true;
          index = (ushort) ((uint) index + 1U);
        }
      }
      return false;
    }

    public static bool tryGetRegion(Transform structure, out byte x, out byte y, out StructureRegion region)
    {
      x = (byte) 0;
      y = (byte) 0;
      region = (StructureRegion) null;
      if ((Object) structure == (Object) null || !Regions.tryGetCoordinate(structure.position, out x, out y))
        return false;
      region = StructureManager.regions[(int) x, (int) y];
      return true;
    }

    public static bool tryGetRegion(byte x, byte y, out StructureRegion region)
    {
      region = (StructureRegion) null;
      if (!Regions.checkSafe(x, y))
        return false;
      region = StructureManager.regions[(int) x, (int) y];
      return true;
    }

    public static void dropStructure(Structure structure, Vector3 point, float angle, ulong owner, ulong group)
    {
      byte x;
      byte y;
      StructureRegion region;
      if ((ItemStructureAsset) Assets.find(EAssetType.ITEM, structure.id) == null || !Regions.tryGetCoordinate(point, out x, out y) || !StructureManager.tryGetRegion(x, y, out region))
        return;
      region.structures.Add(new StructureData(structure, point, MeasurementTool.angleToByte(angle), owner, group));
      StructureManager.manager.channel.send("tellStructure", ESteamCall.ALL, x, y, StructureManager.STRUCTURE_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, (object) x, (object) y, (object) structure.id, (object) point, (object) MeasurementTool.angleToByte(angle), (object) owner, (object) group);
    }

    [SteamCall]
    public void tellTakeStructure(CSteamID steamID, byte x, byte y, ushort index, Vector3 ragdoll)
    {
      StructureRegion region;
      if (!this.channel.checkServer(steamID) || !StructureManager.tryGetRegion(x, y, out region) || !Provider.isServer && !region.isNetworked || (int) index >= region.models.Count)
        return;
      if (Dedicator.isDedicated || GraphicsSettings.effectQuality == EGraphicQuality.OFF)
      {
        Object.Destroy((Object) region.models[(int) index].gameObject);
      }
      else
      {
        ItemStructureAsset itemStructureAsset = (ItemStructureAsset) Assets.find(EAssetType.ITEM, ushort.Parse(region.models[(int) index].name));
        if (itemStructureAsset != null && itemStructureAsset.construct != EConstruct.FLOOR && itemStructureAsset.construct != EConstruct.ROOF)
        {
          ragdoll.y += 8f;
          ragdoll.x += Random.Range(-16f, 16f);
          ragdoll.z += Random.Range(-16f, 16f);
          ragdoll *= 2f;
          region.models[(int) index].parent = Level.effects;
          MeshCollider component = region.models[(int) index].GetComponent<MeshCollider>();
          if ((Object) component != (Object) null)
            component.convex = true;
          region.models[(int) index].tag = "Debris";
          region.models[(int) index].gameObject.layer = LayerMasks.DEBRIS;
          region.models[(int) index].gameObject.AddComponent<Rigidbody>();
          region.models[(int) index].GetComponent<Rigidbody>().AddForce(ragdoll);
          region.models[(int) index].GetComponent<Rigidbody>().drag = 0.5f;
          region.models[(int) index].GetComponent<Rigidbody>().angularDrag = 0.1f;
          region.models[(int) index].localScale *= 0.75f;
          Object.Destroy((Object) region.models[(int) index].gameObject, 8f);
          if (Provider.isServer)
            Object.Destroy((Object) region.models[(int) index].FindChild("Nav").gameObject);
        }
        else
          Object.Destroy((Object) region.models[(int) index].gameObject);
      }
      region.models.RemoveAt((int) index);
    }

    private void spawnStructure(StructureRegion region, ushort id, Vector3 point, byte angle, bool hasOwnership)
    {
      ItemStructureAsset itemStructureAsset = (ItemStructureAsset) Assets.find(EAssetType.ITEM, id);
      if (itemStructureAsset != null)
      {
        Transform structure = StructureTool.getStructure(id, hasOwnership);
        structure.parent = LevelStructures.models;
        structure.position = point;
        structure.rotation = Quaternion.Euler(-90f, (float) ((int) angle * 2), 0.0f);
        if (!Dedicator.isDedicated && itemStructureAsset.construct == EConstruct.FLOOR)
          LevelGround.bewilder(point);
        region.models.Add(structure);
      }
      else
      {
        if (Provider.isServer)
          return;
        Provider.disconnect();
      }
    }

    [SteamCall]
    public void tellStructure(CSteamID steamID, byte x, byte y, ushort id, Vector3 point, byte angle, ulong owner, ulong group)
    {
      StructureRegion region;
      if (!this.channel.checkServer(steamID) || !StructureManager.tryGetRegion(x, y, out region) || !Provider.isServer && !region.isNetworked)
        return;
      this.spawnStructure(region, id, point, angle, OwnershipTool.checkToggle(owner, group));
    }

    [SteamCall]
    public void tellStructures(CSteamID steamID)
    {
      StructureRegion region;
      if (!this.channel.checkServer(steamID) || !StructureManager.tryGetRegion((byte) this.channel.read(Types.BYTE_TYPE), (byte) this.channel.read(Types.BYTE_TYPE), out region))
        return;
      if ((int) (byte) this.channel.read(Types.BYTE_TYPE) == 0)
      {
        if (region.isNetworked)
          return;
      }
      else if (!region.isNetworked)
        return;
      region.isNetworked = true;
      ushort num = (ushort) this.channel.read(Types.UINT16_TYPE);
      for (int index = 0; index < (int) num; ++index)
      {
        object[] objArray = this.channel.read(Types.UINT16_TYPE, Types.VECTOR3_TYPE, Types.BYTE_TYPE, Types.BOOLEAN_TYPE);
        this.spawnStructure(region, (ushort) objArray[0], (Vector3) objArray[1], (byte) objArray[2], (bool) objArray[3]);
      }
      Level.isLoadingStructures = false;
    }

    [SteamCall]
    public void askStructures(CSteamID steamID, byte x, byte y)
    {
      SteamPlayer steamPlayer = PlayerTool.getSteamPlayer(steamID);
      StructureRegion region;
      if (!StructureManager.tryGetRegion(x, y, out region))
        return;
      if (region.structures.Count > 0)
      {
        byte num1 = (byte) 0;
        int index = 0;
        int num2 = 0;
        while (index < region.structures.Count)
        {
          int num3 = 0;
          while (num2 < region.structures.Count)
          {
            num3 += 16;
            ++num2;
            if (num3 > Block.BUFFER_SIZE / 2)
              break;
          }
          this.channel.openWrite();
          this.channel.write((object) x);
          this.channel.write((object) y);
          this.channel.write((object) num1);
          this.channel.write((object) (ushort) (num2 - index));
          for (; index < num2; ++index)
          {
            StructureData structureData = region.structures[index];
            this.channel.write((object) structureData.structure.id, (object) structureData.point, (object) structureData.angle, (object) (bool) (OwnershipTool.checkToggle(steamID, structureData.owner, steamPlayer.playerID.group, structureData.group) ? 1 : 0));
          }
          this.channel.closeWrite("tellStructures", steamID, ESteamPacket.UPDATE_RELIABLE_CHUNK_BUFFER);
          ++num1;
        }
      }
      else
      {
        this.channel.openWrite();
        this.channel.write((object) x);
        this.channel.write((object) y);
        this.channel.write((object) 0);
        this.channel.write((object) 0);
        this.channel.closeWrite("tellStructures", steamID, ESteamPacket.UPDATE_RELIABLE_CHUNK_BUFFER);
      }
    }

    private void onLevelLoaded(int level)
    {
      if (level <= Level.SETUP)
        return;
      StructureManager.regions = new StructureRegion[(int) Regions.WORLD_SIZE, (int) Regions.WORLD_SIZE];
      for (byte index1 = (byte) 0; (int) index1 < (int) Regions.WORLD_SIZE; ++index1)
      {
        for (byte index2 = (byte) 0; (int) index2 < (int) Regions.WORLD_SIZE; ++index2)
          StructureManager.regions[(int) index1, (int) index2] = new StructureRegion();
      }
      if (!Provider.isServer)
        return;
      StructureManager.load();
    }

    private void onRegionUpdated(Player player, byte old_x, byte old_y, byte new_x, byte new_y)
    {
      for (byte x_0 = (byte) 0; (int) x_0 < (int) Regions.WORLD_SIZE; ++x_0)
      {
        for (byte y_0 = (byte) 0; (int) y_0 < (int) Regions.WORLD_SIZE; ++y_0)
        {
          if (Provider.isServer)
          {
            if (player.movement.loadedRegions[(int) x_0, (int) y_0].isStructuresLoaded && !Regions.checkArea(x_0, y_0, new_x, new_y, StructureManager.STRUCTURE_REGIONS))
              player.movement.loadedRegions[(int) x_0, (int) y_0].isStructuresLoaded = false;
          }
          else if (player.channel.isOwner && StructureManager.regions[(int) x_0, (int) y_0].isNetworked && !Regions.checkArea(x_0, y_0, new_x, new_y, StructureManager.STRUCTURE_REGIONS))
          {
            StructureManager.regions[(int) x_0, (int) y_0].destroy();
            StructureManager.regions[(int) x_0, (int) y_0].isNetworked = false;
          }
        }
      }
      if (!Dedicator.isDedicated || !Regions.checkSafe(new_x, new_y))
        return;
      for (int index1 = (int) new_x - (int) StructureManager.STRUCTURE_REGIONS; index1 <= (int) new_x + (int) StructureManager.STRUCTURE_REGIONS; ++index1)
      {
        for (int index2 = (int) new_y - (int) StructureManager.STRUCTURE_REGIONS; index2 <= (int) new_y + (int) StructureManager.STRUCTURE_REGIONS; ++index2)
        {
          if (Regions.checkSafe((byte) index1, (byte) index2) && !player.movement.loadedRegions[index1, index2].isStructuresLoaded)
          {
            player.movement.loadedRegions[index1, index2].isStructuresLoaded = true;
            this.askStructures(player.channel.owner.playerID.steamID, (byte) index1, (byte) index2);
          }
        }
      }
    }

    private void onPlayerCreated(Player player)
    {
      player.movement.onRegionUpdated += new PlayerRegionUpdated(this.onRegionUpdated);
    }

    private void Start()
    {
      StructureManager.manager = this;
      Level.onLevelLoaded += new LevelLoaded(this.onLevelLoaded);
      Player.onPlayerCreated += new PlayerCreated(this.onPlayerCreated);
    }

    public static void load()
    {
      if (LevelSavedata.fileExists("/Structures.dat") && Level.info.type == ELevelType.SURVIVAL)
      {
        River river = LevelSavedata.openRiver("/Structures.dat", true);
        byte version = river.readByte();
        if ((int) version > 1)
        {
          for (byte index1 = (byte) 0; (int) index1 < (int) Regions.WORLD_SIZE; ++index1)
          {
            for (byte index2 = (byte) 0; (int) index2 < (int) Regions.WORLD_SIZE; ++index2)
            {
              StructureRegion region = StructureManager.regions[(int) index1, (int) index2];
              StructureManager.loadRegion(version, river, region);
            }
          }
        }
        else
        {
          ushort num1 = river.readUInt16();
          for (ushort index = (ushort) 0; (int) index < (int) num1; ++index)
          {
            ushort num2 = river.readUInt16();
            ushort newHealth = river.readUInt16();
            river.readBytes();
            Vector3 vector3 = river.readSingleVector3();
            byte num3 = river.readByte();
            byte x;
            byte y;
            StructureRegion region;
            if (Regions.tryGetCoordinate(vector3, out x, out y) && StructureManager.tryGetRegion(x, y, out region))
            {
              region.structures.Add(new StructureData(new Structure(num2, newHealth), vector3, num3, 0UL, 0UL));
              StructureManager.manager.spawnStructure(region, num2, vector3, num3, false);
            }
          }
        }
      }
      Level.isLoadingStructures = false;
    }

    public static void save()
    {
      River river = LevelSavedata.openRiver("/Structures.dat", false);
      river.writeByte(StructureManager.SAVEDATA_VERSION);
      for (byte index1 = (byte) 0; (int) index1 < (int) Regions.WORLD_SIZE; ++index1)
      {
        for (byte index2 = (byte) 0; (int) index2 < (int) Regions.WORLD_SIZE; ++index2)
        {
          StructureRegion region = StructureManager.regions[(int) index1, (int) index2];
          StructureManager.saveRegion(river, region);
        }
      }
      river.closeRiver();
    }

    private static void loadRegion(byte version, River river, StructureRegion region)
    {
      ushort num1 = river.readUInt16();
      for (ushort index = (ushort) 0; (int) index < (int) num1; ++index)
      {
        ushort num2 = river.readUInt16();
        ushort newHealth = river.readUInt16();
        Vector3 vector3 = river.readSingleVector3();
        byte num3 = river.readByte();
        ulong num4 = 0UL;
        ulong num5 = 0UL;
        if ((int) version > 2)
        {
          num4 = river.readUInt64();
          num5 = river.readUInt64();
        }
        region.structures.Add(new StructureData(new Structure(num2, newHealth), vector3, num3, num4, num5));
        StructureManager.manager.spawnStructure(region, num2, vector3, num3, OwnershipTool.checkToggle(num4, num5));
      }
    }

    private static void saveRegion(River river, StructureRegion region)
    {
      river.writeUInt16((ushort) region.structures.Count);
      for (ushort index = (ushort) 0; (int) index < region.structures.Count; ++index)
      {
        StructureData structureData = region.structures[(int) index];
        river.writeUInt16(structureData.structure.id);
        river.writeUInt16(structureData.structure.health);
        river.writeSingleVector3(structureData.point);
        river.writeByte(structureData.angle);
        river.writeUInt64(structureData.owner);
        river.writeUInt64(structureData.group);
      }
    }
  }
}
