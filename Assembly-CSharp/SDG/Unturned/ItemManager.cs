// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ItemManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
  public class ItemManager : SteamCaller
  {
    public static readonly byte ITEM_REGIONS = (byte) 1;
    private static readonly float CHANCE_EASY = 0.35f;
    private static readonly float CHANCE_NORMAL = 0.35f;
    private static readonly float CHANCE_HARD = 0.15f;
    private static readonly float CHANCE_PRO = 0.45f;
    private static readonly float DESPAWN_DROPPED = 600f;
    private static readonly float DESPAWN_SPAWNED = 900f;
    private static readonly float RESPAWN_EASY = 30f;
    private static readonly float RESPAWN_NORMAL = 45f;
    private static readonly float RESPAWN_HARD = 60f;
    private static readonly float RESPAWN_PRO = 15f;
    private static ItemManager manager;
    private static ItemRegion[,] regions;
    private static byte despawnItems_X;
    private static byte despawnItems_Y;
    private static byte respawnItems_X;
    private static byte respawnItems_Y;

    private static float chance
    {
      get
      {
        if (Provider.mode == EGameMode.EASY)
          return ItemManager.CHANCE_EASY;
        if (Provider.mode == EGameMode.NORMAL)
          return ItemManager.CHANCE_NORMAL;
        if (Provider.mode == EGameMode.HARD)
          return ItemManager.CHANCE_HARD;
        if (Provider.mode == EGameMode.PRO)
          return ItemManager.CHANCE_PRO;
        return 0.0f;
      }
    }

    private static float respawn
    {
      get
      {
        if (Provider.mode == EGameMode.EASY)
          return ItemManager.RESPAWN_EASY;
        if (Provider.mode == EGameMode.NORMAL)
          return ItemManager.RESPAWN_NORMAL;
        if (Provider.mode == EGameMode.HARD)
          return ItemManager.RESPAWN_HARD;
        if (Provider.mode == EGameMode.PRO)
          return ItemManager.RESPAWN_PRO;
        return 10f;
      }
    }

    public static void takeItem(Transform item)
    {
      byte x;
      byte y;
      if (!Regions.tryGetCoordinate(item.position, out x, out y))
        return;
      ItemRegion itemRegion = ItemManager.regions[(int) x, (int) y];
      ItemManager.manager.channel.send("askTakeItem", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[3]
      {
        (object) x,
        (object) y,
        (object) item.position
      });
    }

    public static void dropItem(Item item, Vector3 point, bool player, bool isDropped, bool wide)
    {
      byte x;
      byte y;
      if (!Regions.tryGetCoordinate(point, out x, out y))
        return;
      ItemAsset itemAsset = (ItemAsset) Assets.find(EAssetType.ITEM, item.id);
      if (itemAsset == null || itemAsset.isPro)
        return;
      if (player)
        EffectManager.sendEffect((ushort) 6, EffectManager.SMALL, point);
      if (wide)
      {
        point.x += Random.Range(-0.75f, 0.75f);
        point.z += Random.Range(-0.75f, 0.75f);
      }
      else
      {
        point.x += Random.Range(-0.125f, 0.125f);
        point.z += Random.Range(-0.125f, 0.125f);
      }
      RaycastHit hitInfo;
      Physics.Raycast(point + Vector3.up, Vector3.down, out hitInfo, 8f, RayMasks.BLOCK_ITEM);
      if ((Object) hitInfo.collider != (Object) null)
        point.y = hitInfo.point.y;
      ItemManager.regions[(int) x, (int) y].items.Add(new ItemData(item, point, isDropped));
      ItemManager.manager.channel.send("tellItem", ESteamCall.CLIENTS, x, y, ItemManager.ITEM_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, (object) x, (object) y, (object) item.id, (object) item.amount, (object) item.quality, (object) item.state, (object) point);
    }

    [SteamCall]
    public void tellTakeItem(CSteamID steamID, byte x, byte y, Vector3 point)
    {
      if (!this.channel.checkServer(steamID) || !Provider.isServer && !ItemManager.regions[(int) x, (int) y].isNetworked)
        return;
      ItemRegion itemRegion = ItemManager.regions[(int) x, (int) y];
      for (ushort index = (ushort) 0; (int) index < itemRegion.models.Count; ++index)
      {
        if ((double) (itemRegion.models[(int) index].position - point).sqrMagnitude < (double) Provider.EPSILON)
        {
          Object.Destroy((Object) itemRegion.models[(int) index].gameObject);
          itemRegion.models.RemoveAt((int) index);
          break;
        }
      }
    }

    [SteamCall]
    public void askTakeItem(CSteamID steamID, byte x, byte y, Vector3 point)
    {
      if (!Provider.isServer || !Regions.checkSafe(x, y))
        return;
      Player player = PlayerTool.getPlayer(steamID);
      if ((Object) player == (Object) null || player.life.isDead)
        return;
      ItemRegion itemRegion = ItemManager.regions[(int) x, (int) y];
      for (ushort index = (ushort) 0; (int) index < itemRegion.items.Count; ++index)
      {
        if ((double) (itemRegion.items[(int) index].point - point).sqrMagnitude < (double) Provider.EPSILON)
        {
          if ((double) (itemRegion.items[(int) index].point - player.transform.position).sqrMagnitude > 400.0)
            break;
          if (player.inventory.tryAddItem(ItemManager.regions[(int) x, (int) y].items[(int) index].item, true))
          {
            if (!player.equipment.isSelected)
              player.animator.sendGesture(EPlayerGesture.PICKUP, true);
            EffectManager.sendEffect((ushort) 7, EffectManager.SMALL, ItemManager.regions[(int) x, (int) y].items[(int) index].point);
            ItemManager.regions[(int) x, (int) y].items.RemoveAt((int) index);
            player.sendStat(EPlayerStat.FOUND_ITEMS);
            this.channel.send("tellTakeItem", ESteamCall.CLIENTS, x, y, ItemManager.ITEM_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, (object) x, (object) y, (object) point);
            break;
          }
          player.sendMessage(EPlayerMessage.SPACE);
          break;
        }
      }
    }

    private void spawnItem(byte x, byte y, ushort id, byte amount, byte quality, byte[] state, Vector3 point)
    {
      ItemAsset itemAsset = (ItemAsset) Assets.find(EAssetType.ITEM, id);
      if (itemAsset == null)
        return;
      Transform transform1 = new GameObject().transform;
      transform1.name = id.ToString();
      transform1.transform.parent = LevelItems.models;
      transform1.transform.position = point;
      Transform transform2 = ItemTool.getItem(id, (ushort) 0, quality, state, false, itemAsset);
      transform2.parent = transform1;
      InteractableItem interactableItem = transform2.gameObject.AddComponent<InteractableItem>();
      interactableItem.item = new Item(id, amount, quality, state);
      interactableItem.asset = itemAsset;
      if (OptionsSettings.physics)
      {
        transform2.position = point + Vector3.up * 0.75f;
        transform2.rotation = Quaternion.Euler((float) (Random.Range(-15, 15) - 90), (float) Random.Range(0, 360), (float) Random.Range(-15, 15));
        transform2.gameObject.AddComponent<Rigidbody>();
        transform2.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;
        transform2.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Discrete;
        transform2.GetComponent<Rigidbody>().drag = 0.5f;
        transform2.GetComponent<Rigidbody>().angularDrag = 0.1f;
      }
      else
      {
        transform2.position = point + Vector3.up * 0.25f;
        transform2.rotation = Quaternion.Euler(-90f, (float) Random.Range(0, 360), 0.0f);
      }
      ItemManager.regions[(int) x, (int) y].models.Add(transform1);
    }

    [SteamCall]
    public void tellItem(CSteamID steamID, byte x, byte y, ushort id, byte amount, byte quality, byte[] state, Vector3 point)
    {
      if (!this.channel.checkServer(steamID) || !Regions.checkSafe(x, y) || !Provider.isServer && !ItemManager.regions[(int) x, (int) y].isNetworked)
        return;
      this.spawnItem(x, y, id, amount, quality, state, point);
    }

    [SteamCall]
    public void tellItems(CSteamID steamID)
    {
      if (!this.channel.checkServer(steamID))
        return;
      byte x = (byte) this.channel.read(Types.BYTE_TYPE);
      byte y = (byte) this.channel.read(Types.BYTE_TYPE);
      if (!Regions.checkSafe(x, y) || ItemManager.regions[(int) x, (int) y].isNetworked)
        return;
      ItemManager.regions[(int) x, (int) y].isNetworked = true;
      ushort num = (ushort) this.channel.read(Types.UINT16_TYPE);
      for (int index = 0; index < (int) num; ++index)
      {
        object[] objArray = this.channel.read(Types.UINT16_TYPE, Types.BYTE_TYPE, Types.BYTE_TYPE, Types.BYTE_ARRAY_TYPE, Types.VECTOR3_TYPE);
        this.spawnItem(x, y, (ushort) objArray[0], (byte) objArray[1], (byte) objArray[2], (byte[]) objArray[3], (Vector3) objArray[4]);
      }
    }

    public void askItems(CSteamID steamID, byte x, byte y)
    {
      this.channel.openWrite();
      this.channel.write((object) x);
      this.channel.write((object) y);
      this.channel.write((object) (ushort) ItemManager.regions[(int) x, (int) y].items.Count);
      for (int index = 0; index < ItemManager.regions[(int) x, (int) y].items.Count; ++index)
      {
        ItemData itemData = ItemManager.regions[(int) x, (int) y].items[index];
        this.channel.write((object) itemData.item.id, (object) itemData.item.amount, (object) itemData.item.quality, (object) itemData.item.state, (object) itemData.point);
      }
      this.channel.closeWrite("tellItems", steamID, ESteamPacket.UPDATE_RELIABLE_CHUNK_BUFFER);
    }

    private void despawnItems()
    {
      if (ItemManager.regions[(int) ItemManager.despawnItems_X, (int) ItemManager.despawnItems_Y].items.Count <= 0)
        return;
      for (int index = 0; index < ItemManager.regions[(int) ItemManager.despawnItems_X, (int) ItemManager.despawnItems_Y].items.Count; ++index)
      {
        if ((double) Time.realtimeSinceStartup - (double) ItemManager.regions[(int) ItemManager.despawnItems_X, (int) ItemManager.despawnItems_Y].items[index].lastDropped > (!ItemManager.regions[(int) ItemManager.despawnItems_X, (int) ItemManager.despawnItems_Y].items[index].isDropped ? (double) ItemManager.DESPAWN_SPAWNED : (double) ItemManager.DESPAWN_DROPPED))
        {
          Vector3 point = ItemManager.regions[(int) ItemManager.despawnItems_X, (int) ItemManager.despawnItems_Y].items[index].point;
          ItemManager.regions[(int) ItemManager.despawnItems_X, (int) ItemManager.despawnItems_Y].items.RemoveAt(index);
          this.channel.send("tellTakeItem", ESteamCall.CLIENTS, ItemManager.despawnItems_X, ItemManager.despawnItems_Y, ItemManager.ITEM_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, (object) ItemManager.despawnItems_X, (object) ItemManager.despawnItems_Y, (object) point);
          break;
        }
      }
    }

    private bool respawnItems()
    {
      if (LevelItems.spawns[(int) ItemManager.respawnItems_X, (int) ItemManager.respawnItems_Y].Count <= 0)
        return true;
      if ((double) Time.realtimeSinceStartup - (double) ItemManager.regions[(int) ItemManager.respawnItems_X, (int) ItemManager.respawnItems_Y].lastRespawn > (double) ItemManager.respawn && (double) ItemManager.regions[(int) ItemManager.respawnItems_X, (int) ItemManager.respawnItems_Y].items.Count < (double) LevelItems.spawns[(int) ItemManager.respawnItems_X, (int) ItemManager.respawnItems_Y].Count * (double) ItemManager.chance)
      {
        ItemManager.regions[(int) ItemManager.respawnItems_X, (int) ItemManager.respawnItems_Y].lastRespawn = Time.realtimeSinceStartup;
        ItemSpawnpoint spawn = LevelItems.spawns[(int) ItemManager.respawnItems_X, (int) ItemManager.respawnItems_Y][Random.Range(0, LevelItems.spawns[(int) ItemManager.respawnItems_X, (int) ItemManager.respawnItems_Y].Count)];
        if (!SafezoneManager.checkPointValid(spawn.point))
          return false;
        for (ushort index = (ushort) 0; (int) index < ItemManager.regions[(int) ItemManager.respawnItems_X, (int) ItemManager.respawnItems_Y].items.Count; ++index)
        {
          if ((double) (ItemManager.regions[(int) ItemManager.respawnItems_X, (int) ItemManager.respawnItems_Y].items[(int) index].point - spawn.point).sqrMagnitude < 4.0)
            return false;
        }
        ushort newID = LevelItems.getItem(spawn);
        if ((int) newID != 0)
        {
          Item newItem = new Item(newID, false);
          ItemManager.regions[(int) ItemManager.respawnItems_X, (int) ItemManager.respawnItems_Y].items.Add(new ItemData(newItem, spawn.point, false));
          ItemManager.manager.channel.send("tellItem", ESteamCall.CLIENTS, ESteamPacket.UPDATE_RELIABLE_BUFFER, (object) ItemManager.respawnItems_X, (object) ItemManager.respawnItems_Y, (object) newItem.id, (object) newItem.quality, (object) newItem.state, (object) spawn.point);
        }
      }
      return false;
    }

    private void generateItems(byte x, byte y)
    {
      List<ItemData> list1 = new List<ItemData>();
      if (LevelItems.spawns[(int) x, (int) y].Count > 0)
      {
        List<ItemSpawnpoint> list2 = new List<ItemSpawnpoint>();
        for (int index = 0; index < LevelItems.spawns[(int) x, (int) y].Count; ++index)
        {
          ItemSpawnpoint itemSpawnpoint = LevelItems.spawns[(int) x, (int) y][index];
          if (SafezoneManager.checkPointValid(itemSpawnpoint.point))
            list2.Add(itemSpawnpoint);
        }
        while ((double) list1.Count < (double) LevelItems.spawns[(int) x, (int) y].Count * (double) ItemManager.chance && list2.Count > 0)
        {
          int index = Random.Range(0, list2.Count);
          ItemSpawnpoint spawn = list2[index];
          list2.RemoveAt(index);
          ushort newID = LevelItems.getItem(spawn);
          if ((int) newID != 0)
          {
            Item newItem = new Item(newID, false);
            list1.Add(new ItemData(newItem, spawn.point, false));
          }
        }
      }
      for (int index = 0; index < ItemManager.regions[(int) x, (int) y].items.Count; ++index)
      {
        if (ItemManager.regions[(int) x, (int) y].items[index].isDropped)
          list1.Add(ItemManager.regions[(int) x, (int) y].items[index]);
      }
      ItemManager.regions[(int) x, (int) y].items = list1;
    }

    private void onLevelLoaded(int level)
    {
      if (level <= Level.SETUP)
        return;
      ItemManager.regions = new ItemRegion[(int) Regions.WORLD_SIZE, (int) Regions.WORLD_SIZE];
      for (byte index1 = (byte) 0; (int) index1 < (int) Regions.WORLD_SIZE; ++index1)
      {
        for (byte index2 = (byte) 0; (int) index2 < (int) Regions.WORLD_SIZE; ++index2)
          ItemManager.regions[(int) index1, (int) index2] = new ItemRegion();
      }
      ItemManager.despawnItems_X = (byte) 0;
      ItemManager.despawnItems_Y = (byte) 0;
      ItemManager.respawnItems_X = (byte) 0;
      ItemManager.respawnItems_Y = (byte) 0;
      if (!Dedicator.isDedicated)
        return;
      for (byte x = (byte) 0; (int) x < (int) Regions.WORLD_SIZE; ++x)
      {
        for (byte y = (byte) 0; (int) y < (int) Regions.WORLD_SIZE; ++y)
          this.generateItems(x, y);
      }
    }

    private void onRegionUpdated(Player player, byte old_x, byte old_y, byte new_x, byte new_y)
    {
      for (byte x_0 = (byte) 0; (int) x_0 < (int) Regions.WORLD_SIZE; ++x_0)
      {
        for (byte y_0 = (byte) 0; (int) y_0 < (int) Regions.WORLD_SIZE; ++y_0)
        {
          if (player.channel.isOwner && ItemManager.regions[(int) x_0, (int) y_0].isNetworked && !Regions.checkArea(x_0, y_0, new_x, new_y, ItemManager.ITEM_REGIONS))
          {
            ItemManager.regions[(int) x_0, (int) y_0].destroy();
            ItemManager.regions[(int) x_0, (int) y_0].isNetworked = false;
          }
          if (Provider.isServer && player.movement.loadedRegions[(int) x_0, (int) y_0].isItemsLoaded && !Regions.checkArea(x_0, y_0, new_x, new_y, ItemManager.ITEM_REGIONS))
            player.movement.loadedRegions[(int) x_0, (int) y_0].isItemsLoaded = false;
        }
      }
      if (!Provider.isServer || !Regions.checkSafe(new_x, new_y))
        return;
      for (int index1 = (int) new_x - (int) ItemManager.ITEM_REGIONS; index1 <= (int) new_x + (int) ItemManager.ITEM_REGIONS; ++index1)
      {
        for (int index2 = (int) new_y - (int) ItemManager.ITEM_REGIONS; index2 <= (int) new_y + (int) ItemManager.ITEM_REGIONS; ++index2)
        {
          if (Regions.checkSafe((byte) index1, (byte) index2) && !player.movement.loadedRegions[index1, index2].isItemsLoaded)
          {
            if (player.channel.isOwner)
              this.generateItems((byte) index1, (byte) index2);
            player.movement.loadedRegions[index1, index2].isItemsLoaded = true;
            this.askItems(player.channel.owner.playerID.steamID, (byte) index1, (byte) index2);
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
      if (!Dedicator.isDedicated || !Level.isLoaded)
        return;
      this.despawnItems();
      ++ItemManager.despawnItems_X;
      if ((int) ItemManager.despawnItems_X >= (int) Regions.WORLD_SIZE)
      {
        ItemManager.despawnItems_X = (byte) 0;
        ++ItemManager.despawnItems_Y;
        if ((int) ItemManager.despawnItems_Y >= (int) Regions.WORLD_SIZE)
          ItemManager.despawnItems_Y = (byte) 0;
      }
      bool flag = true;
      while (flag)
      {
        flag = this.respawnItems();
        ++ItemManager.respawnItems_X;
        if ((int) ItemManager.respawnItems_X >= (int) Regions.WORLD_SIZE)
        {
          ItemManager.respawnItems_X = (byte) 0;
          ++ItemManager.respawnItems_Y;
          if ((int) ItemManager.respawnItems_Y >= (int) Regions.WORLD_SIZE)
          {
            ItemManager.respawnItems_Y = (byte) 0;
            flag = false;
          }
        }
      }
    }

    private void Start()
    {
      ItemManager.manager = this;
      Level.onLevelLoaded += new LevelLoaded(this.onLevelLoaded);
      Player.onPlayerCreated += new PlayerCreated(this.onPlayerCreated);
    }
  }
}
