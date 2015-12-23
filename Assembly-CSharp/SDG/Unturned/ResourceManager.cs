// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ResourceManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
  public class ResourceManager : SteamCaller
  {
    public static readonly byte RESOURCE_REGIONS = (byte) 1;
    private static readonly float RESPAWN = 600f;
    private static ResourceManager manager;
    private static ResourceRegion[,] regions;
    private static byte respawnResources_X;
    private static byte respawnResources_Y;

    public static void damage(Transform resource, Vector3 direction, float damage, float times, float drop, out EPlayerKill kill)
    {
      kill = EPlayerKill.NONE;
      byte x;
      byte y;
      if (!Regions.tryGetCoordinate(resource.position, out x, out y))
        return;
      List<ResourceSpawnpoint> list = LevelGround.trees[(int) x, (int) y];
      for (ushort index1 = (ushort) 0; (int) index1 < list.Count; ++index1)
      {
        if ((Object) resource == (Object) list[(int) index1].model)
        {
          if (list[(int) index1].isDead)
            break;
          ushort amount = (ushort) ((double) damage * (double) times);
          list[(int) index1].askDamage(amount);
          if (!list[(int) index1].isDead)
            break;
          kill = EPlayerKill.RESOURCE;
          ResourceAsset resourceAsset = (ResourceAsset) Assets.find(EAssetType.RESOURCE, LevelGround.resources[(int) list[(int) index1].type].id);
          if (resourceAsset != null && (int) resourceAsset.explosion != 0)
            EffectManager.sendEffect(resourceAsset.explosion, x, y, ResourceManager.RESOURCE_REGIONS, resource.position + Vector3.up * 8f);
          if ((int) resourceAsset.log != 0)
          {
            int num = (int) ((double) Random.Range(3, 7) * (double) drop);
            for (int index2 = 0; index2 < num; ++index2)
              ItemManager.dropItem(new Item(resourceAsset.log, true), resource.position + direction * (float) (2 + index2 * 2) + Vector3.up, false, Dedicator.isDedicated, true);
          }
          if ((int) resourceAsset.stick != 0)
          {
            int num = (int) ((double) Random.Range(2, 5) * (double) drop);
            for (int index2 = 0; index2 < num; ++index2)
            {
              float f = Random.Range(0.0f, 6.283185f);
              ItemManager.dropItem(new Item(resourceAsset.stick, true), resource.position + new Vector3(Mathf.Sin(f) * 3f, 1f, Mathf.Cos(f) * 3f), false, Dedicator.isDedicated, true);
            }
          }
          ResourceManager.manager.channel.send("tellResourceDead", ESteamCall.ALL, x, y, ResourceManager.RESOURCE_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, (object) x, (object) y, (object) index1, (object) (direction * (float) amount));
          break;
        }
      }
    }

    public static void forage(Transform resource)
    {
      byte x;
      byte y;
      if (!Regions.tryGetCoordinate(resource.position, out x, out y))
        return;
      List<ResourceSpawnpoint> list = LevelGround.trees[(int) x, (int) y];
      for (ushort index = (ushort) 0; (int) index < list.Count; ++index)
      {
        if ((Object) resource == (Object) list[(int) index].model)
        {
          ResourceManager.manager.channel.send("askForage", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[3]
          {
            (object) x,
            (object) y,
            (object) index
          });
          break;
        }
      }
    }

    [SteamCall]
    public void askForage(CSteamID steamID, byte x, byte y, ushort index)
    {
      if (!Provider.isServer || !Regions.checkSafe(x, y))
        return;
      Player player = PlayerTool.getPlayer(steamID);
      if ((Object) player == (Object) null || player.life.isDead)
        return;
      List<ResourceSpawnpoint> list = LevelGround.trees[(int) x, (int) y];
      if ((int) index >= list.Count || list[(int) index].isDead)
        return;
      ResourceAsset resourceAsset = (ResourceAsset) Assets.find(EAssetType.RESOURCE, LevelGround.resources[(int) list[(int) index].type].id);
      if (resourceAsset == null || !resourceAsset.isForage)
        return;
      list[(int) index].askDamage((ushort) 1);
      if ((int) resourceAsset.explosion != 0)
        EffectManager.sendEffect(resourceAsset.explosion, x, y, ResourceManager.RESOURCE_REGIONS, list[(int) index].point);
      if ((int) resourceAsset.log != 0)
      {
        player.inventory.forceAddItem(new Item(resourceAsset.log, true), true);
        if ((double) Random.value < (double) player.skills.mastery(2, 5))
          player.inventory.forceAddItem(new Item(resourceAsset.log, true), true);
        player.skills.askAward(1U);
      }
      ResourceManager.manager.channel.send("tellResourceDead", ESteamCall.ALL, x, y, ResourceManager.RESOURCE_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, (object) x, (object) y, (object) index, (object) Vector3.zero);
    }

    [SteamCall]
    public void tellResourceDead(CSteamID steamID, byte x, byte y, ushort index, Vector3 ragdoll)
    {
      if (!this.channel.checkServer(steamID) || (int) index >= LevelGround.trees[(int) x, (int) y].Count || !Provider.isServer && !ResourceManager.regions[(int) x, (int) y].isNetworked)
        return;
      LevelGround.trees[(int) x, (int) y][(int) index].kill(ragdoll);
    }

    [SteamCall]
    public void tellResourceAlive(CSteamID steamID, byte x, byte y, ushort index)
    {
      if (!this.channel.checkServer(steamID) || (int) index >= LevelGround.trees[(int) x, (int) y].Count || !Provider.isServer && !ResourceManager.regions[(int) x, (int) y].isNetworked)
        return;
      LevelGround.trees[(int) x, (int) y][(int) index].revive();
    }

    [SteamCall]
    public void tellResources(CSteamID steamID, byte x, byte y, bool[] resources)
    {
      if (!this.channel.checkServer(steamID) || !Regions.checkSafe(x, y) || ResourceManager.regions[(int) x, (int) y].isNetworked)
        return;
      ResourceManager.regions[(int) x, (int) y].isNetworked = true;
      for (ushort index = (ushort) 0; (int) index < resources.Length; ++index)
      {
        if (resources[(int) index])
          LevelGround.trees[(int) x, (int) y][(int) index].wipe();
      }
    }

    [SteamCall]
    public void askResources(CSteamID steamID, byte x, byte y)
    {
      bool[] flagArray = new bool[LevelGround.trees[(int) x, (int) y].Count];
      for (ushort index = (ushort) 0; (int) index < flagArray.Length; ++index)
        flagArray[(int) index] = LevelGround.trees[(int) x, (int) y][(int) index].isDead;
      this.channel.send("tellResources", steamID, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[3]
      {
        (object) x,
        (object) y,
        (object) flagArray
      });
    }

    public static Transform getResource(byte x, byte y, ushort index)
    {
      List<ResourceSpawnpoint> list = LevelGround.trees[(int) x, (int) y];
      if ((int) index >= list.Count)
        return (Transform) null;
      return list[(int) index].model;
    }

    public static bool tryGetRegion(Transform resource, out byte x, out byte y, out ushort index)
    {
      x = (byte) 0;
      y = (byte) 0;
      index = (ushort) 0;
      if (Regions.tryGetCoordinate(resource.position, out x, out y))
      {
        List<ResourceSpawnpoint> list = LevelGround.trees[(int) x, (int) y];
        index = (ushort) 0;
        while ((int) index < list.Count)
        {
          if ((Object) resource == (Object) list[(int) index].model)
            return true;
          index = (ushort) ((uint) index + 1U);
        }
      }
      return false;
    }

    private bool respawnResources()
    {
      if (LevelGround.trees[(int) ResourceManager.respawnResources_X, (int) ResourceManager.respawnResources_Y].Count <= 0)
        return true;
      if ((int) ResourceManager.regions[(int) ResourceManager.respawnResources_X, (int) ResourceManager.respawnResources_Y].respawnResourceIndex >= LevelGround.trees[(int) ResourceManager.respawnResources_X, (int) ResourceManager.respawnResources_Y].Count)
        ResourceManager.regions[(int) ResourceManager.respawnResources_X, (int) ResourceManager.respawnResources_Y].respawnResourceIndex = (ushort) (LevelGround.trees[(int) ResourceManager.respawnResources_X, (int) ResourceManager.respawnResources_Y].Count - 1);
      ResourceSpawnpoint resourceSpawnpoint = LevelGround.trees[(int) ResourceManager.respawnResources_X, (int) ResourceManager.respawnResources_Y][(int) ResourceManager.regions[(int) ResourceManager.respawnResources_X, (int) ResourceManager.respawnResources_Y].respawnResourceIndex];
      if (resourceSpawnpoint.isDead && (double) Time.realtimeSinceStartup - (double) resourceSpawnpoint.lastDead > (double) ResourceManager.RESPAWN)
        ResourceManager.manager.channel.send("tellResourceAlive", ESteamCall.ALL, ResourceManager.respawnResources_X, ResourceManager.respawnResources_Y, ResourceManager.RESOURCE_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, (object) ResourceManager.respawnResources_X, (object) ResourceManager.respawnResources_Y, (object) ResourceManager.regions[(int) ResourceManager.respawnResources_X, (int) ResourceManager.respawnResources_Y].respawnResourceIndex);
      ++ResourceManager.regions[(int) ResourceManager.respawnResources_X, (int) ResourceManager.respawnResources_Y].respawnResourceIndex;
      if ((int) ResourceManager.regions[(int) ResourceManager.respawnResources_X, (int) ResourceManager.respawnResources_Y].respawnResourceIndex >= LevelGround.trees[(int) ResourceManager.respawnResources_X, (int) ResourceManager.respawnResources_Y].Count)
        ResourceManager.regions[(int) ResourceManager.respawnResources_X, (int) ResourceManager.respawnResources_Y].respawnResourceIndex = (ushort) 0;
      return false;
    }

    private void onLevelLoaded(int level)
    {
      if (level <= Level.SETUP)
        return;
      ResourceManager.regions = new ResourceRegion[(int) Regions.WORLD_SIZE, (int) Regions.WORLD_SIZE];
      for (byte index1 = (byte) 0; (int) index1 < (int) Regions.WORLD_SIZE; ++index1)
      {
        for (byte index2 = (byte) 0; (int) index2 < (int) Regions.WORLD_SIZE; ++index2)
          ResourceManager.regions[(int) index1, (int) index2] = new ResourceRegion();
      }
      ResourceManager.respawnResources_X = (byte) 0;
      ResourceManager.respawnResources_Y = (byte) 0;
    }

    private void onRegionUpdated(Player player, byte old_x, byte old_y, byte new_x, byte new_y)
    {
      for (byte x_0 = (byte) 0; (int) x_0 < (int) Regions.WORLD_SIZE; ++x_0)
      {
        for (byte y_0 = (byte) 0; (int) y_0 < (int) Regions.WORLD_SIZE; ++y_0)
        {
          if (Provider.isServer)
          {
            if (player.movement.loadedRegions[(int) x_0, (int) y_0].isResourcesLoaded && !Regions.checkArea(x_0, y_0, new_x, new_y, ResourceManager.RESOURCE_REGIONS))
              player.movement.loadedRegions[(int) x_0, (int) y_0].isResourcesLoaded = false;
          }
          else if (player.channel.isOwner && ResourceManager.regions[(int) x_0, (int) y_0].isNetworked && !Regions.checkArea(x_0, y_0, new_x, new_y, ResourceManager.RESOURCE_REGIONS))
            ResourceManager.regions[(int) x_0, (int) y_0].isNetworked = false;
        }
      }
      if (!Dedicator.isDedicated || !Regions.checkSafe(new_x, new_y))
        return;
      for (int index1 = (int) new_x - (int) ResourceManager.RESOURCE_REGIONS; index1 <= (int) new_x + (int) ResourceManager.RESOURCE_REGIONS; ++index1)
      {
        for (int index2 = (int) new_y - (int) ResourceManager.RESOURCE_REGIONS; index2 <= (int) new_y + (int) ResourceManager.RESOURCE_REGIONS; ++index2)
        {
          if (Regions.checkSafe((byte) index1, (byte) index2) && !player.movement.loadedRegions[index1, index2].isResourcesLoaded)
          {
            player.movement.loadedRegions[index1, index2].isResourcesLoaded = true;
            this.askResources(player.channel.owner.playerID.steamID, (byte) index1, (byte) index2);
          }
        }
      }
    }

    private void onPlayerCreated(Player player)
    {
      player.movement.onRegionUpdated += new PlayerRegionUpdated(this.onRegionUpdated);
    }

    private void FixedUpdate()
    {
      if (!Provider.isServer || !Level.isLoaded)
        return;
      bool flag = true;
      while (flag)
      {
        flag = this.respawnResources();
        ++ResourceManager.respawnResources_X;
        if ((int) ResourceManager.respawnResources_X >= (int) Regions.WORLD_SIZE)
        {
          ResourceManager.respawnResources_X = (byte) 0;
          ++ResourceManager.respawnResources_Y;
          if ((int) ResourceManager.respawnResources_Y >= (int) Regions.WORLD_SIZE)
          {
            ResourceManager.respawnResources_Y = (byte) 0;
            flag = false;
          }
        }
      }
    }

    private void Start()
    {
      ResourceManager.manager = this;
      Level.onLevelLoaded += new LevelLoaded(this.onLevelLoaded);
      Player.onPlayerCreated += new PlayerCreated(this.onPlayerCreated);
    }
  }
}
