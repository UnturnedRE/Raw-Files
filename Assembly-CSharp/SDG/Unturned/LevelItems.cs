// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.LevelItems
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
  public class LevelItems
  {
    public static readonly byte SAVEDATA_VERSION = (byte) 3;
    private static Transform _models;
    private static List<ItemTable> _tables;
    private static List<ItemSpawnpoint>[,] _spawns;

    public static Transform models
    {
      get
      {
        return LevelItems._models;
      }
    }

    public static List<ItemTable> tables
    {
      get
      {
        return LevelItems._tables;
      }
    }

    public static List<ItemSpawnpoint>[,] spawns
    {
      get
      {
        return LevelItems._spawns;
      }
    }

    public static void setEnabled(bool isEnabled)
    {
      for (byte index1 = (byte) 0; (int) index1 < (int) Regions.WORLD_SIZE; ++index1)
      {
        for (byte index2 = (byte) 0; (int) index2 < (int) Regions.WORLD_SIZE; ++index2)
        {
          for (int index3 = 0; index3 < LevelItems.spawns[(int) index1, (int) index2].Count; ++index3)
            LevelItems.spawns[(int) index1, (int) index2][index3].setEnabled(isEnabled);
        }
      }
    }

    public static void addTable(string name)
    {
      if (LevelItems.tables.Count == (int) byte.MaxValue)
        return;
      LevelItems.tables.Add(new ItemTable(name));
    }

    public static void removeTable()
    {
      LevelItems.tables.RemoveAt((int) EditorSpawns.selectedItem);
      for (byte index1 = (byte) 0; (int) index1 < (int) Regions.WORLD_SIZE; ++index1)
      {
        for (byte index2 = (byte) 0; (int) index2 < (int) Regions.WORLD_SIZE; ++index2)
        {
          List<ItemSpawnpoint> list = new List<ItemSpawnpoint>();
          for (int index3 = 0; index3 < LevelItems.spawns[(int) index1, (int) index2].Count; ++index3)
          {
            ItemSpawnpoint itemSpawnpoint = LevelItems.spawns[(int) index1, (int) index2][index3];
            if ((int) itemSpawnpoint.type == (int) EditorSpawns.selectedItem)
            {
              Object.Destroy((Object) itemSpawnpoint.node.gameObject);
            }
            else
            {
              if ((int) itemSpawnpoint.type > (int) EditorSpawns.selectedItem)
                --itemSpawnpoint.type;
              list.Add(itemSpawnpoint);
            }
          }
          LevelItems._spawns[(int) index1, (int) index2] = list;
        }
      }
      for (int index = 0; index < LevelZombies.tables.Count; ++index)
      {
        ZombieTable zombieTable = LevelZombies.tables[index];
        if ((int) zombieTable.loot > (int) EditorSpawns.selectedItem)
          --zombieTable.loot;
      }
      EditorSpawns.selectedItem = (byte) 0;
      if ((int) EditorSpawns.selectedItem >= LevelItems.tables.Count)
        return;
      EditorSpawns.itemSpawn.GetComponent<Renderer>().material.color = LevelItems.tables[(int) EditorSpawns.selectedItem].color;
    }

    public static void addSpawn(Vector3 point)
    {
      byte x;
      byte y;
      if (!Regions.tryGetCoordinate(point, out x, out y) || (int) EditorSpawns.selectedItem >= LevelItems.tables.Count)
        return;
      LevelItems.spawns[(int) x, (int) y].Add(new ItemSpawnpoint(EditorSpawns.selectedItem, point));
    }

    public static void removeSpawn(Vector3 point, float radius)
    {
      radius *= radius;
      for (byte index1 = (byte) 0; (int) index1 < (int) Regions.WORLD_SIZE; ++index1)
      {
        for (byte index2 = (byte) 0; (int) index2 < (int) Regions.WORLD_SIZE; ++index2)
        {
          List<ItemSpawnpoint> list = new List<ItemSpawnpoint>();
          for (int index3 = 0; index3 < LevelItems.spawns[(int) index1, (int) index2].Count; ++index3)
          {
            ItemSpawnpoint itemSpawnpoint = LevelItems.spawns[(int) index1, (int) index2][index3];
            if ((double) (itemSpawnpoint.point - point).sqrMagnitude < (double) radius)
              Object.Destroy((Object) itemSpawnpoint.node.gameObject);
            else
              list.Add(itemSpawnpoint);
          }
          LevelItems._spawns[(int) index1, (int) index2] = list;
        }
      }
    }

    public static ushort getItem(ItemSpawnpoint spawn)
    {
      return LevelItems.getItem(spawn.type);
    }

    public static ushort getItem(byte type)
    {
      return LevelItems.tables[(int) type].getItem();
    }

    public static void load()
    {
      LevelItems._models = new GameObject().transform;
      LevelItems.models.name = "Items";
      LevelItems.models.parent = Level.spawns;
      LevelItems.models.tag = "Logic";
      LevelItems.models.gameObject.layer = LayerMasks.LOGIC;
      if (!Level.isEditor && !Provider.isServer)
        return;
      LevelItems._tables = new List<ItemTable>();
      LevelItems._spawns = new List<ItemSpawnpoint>[(int) Regions.WORLD_SIZE, (int) Regions.WORLD_SIZE];
      if (ReadWrite.fileExists(Level.info.path + "/Spawns/Items.dat", false, false))
      {
        Block block = ReadWrite.readBlock(Level.info.path + "/Spawns/Items.dat", false, false, (byte) 0);
        byte num1 = block.readByte();
        if ((int) num1 > 1 && (int) num1 < 3)
          block.readSteamID();
        byte num2 = block.readByte();
        for (byte index1 = (byte) 0; (int) index1 < (int) num2; ++index1)
        {
          Color newColor = block.readColor();
          string newName1 = block.readString();
          List<ItemTier> newTiers = new List<ItemTier>();
          byte num3 = block.readByte();
          for (byte index2 = (byte) 0; (int) index2 < (int) num3; ++index2)
          {
            string newName2 = block.readString();
            float newChance = block.readSingle();
            List<ItemSpawn> newTable = new List<ItemSpawn>();
            byte num4 = block.readByte();
            for (byte index3 = (byte) 0; (int) index3 < (int) num4; ++index3)
            {
              ushort num5 = block.readUInt16();
              ItemAsset itemAsset = (ItemAsset) Assets.find(EAssetType.ITEM, num5);
              if (itemAsset != null && !itemAsset.isPro)
                newTable.Add(new ItemSpawn(num5));
            }
            if (newTable.Count > 0)
              newTiers.Add(new ItemTier(newTable, newName2, newChance));
          }
          LevelItems.tables.Add(new ItemTable(newTiers, newColor, newName1));
          if (!Level.isEditor)
            LevelItems.tables[(int) index1].buildTable();
        }
      }
      for (byte index1 = (byte) 0; (int) index1 < (int) Regions.WORLD_SIZE; ++index1)
      {
        for (byte index2 = (byte) 0; (int) index2 < (int) Regions.WORLD_SIZE; ++index2)
          LevelItems.spawns[(int) index1, (int) index2] = new List<ItemSpawnpoint>();
      }
      if (ReadWrite.fileExists(Level.info.path + "/Spawns/Jars.dat", false, false))
      {
        River river = new River(Level.info.path + "/Spawns/Jars.dat", false);
        if ((int) river.readByte() > 0)
        {
          for (byte index1 = (byte) 0; (int) index1 < (int) Regions.WORLD_SIZE; ++index1)
          {
            for (byte index2 = (byte) 0; (int) index2 < (int) Regions.WORLD_SIZE; ++index2)
            {
              ushort num = river.readUInt16();
              for (ushort index3 = (ushort) 0; (int) index3 < (int) num; ++index3)
              {
                byte newType = river.readByte();
                Vector3 newPoint = river.readSingleVector3();
                LevelItems.spawns[(int) index1, (int) index2].Add(new ItemSpawnpoint(newType, newPoint));
              }
            }
          }
        }
        river.closeRiver();
      }
      else
      {
        for (byte index1 = (byte) 0; (int) index1 < (int) Regions.WORLD_SIZE; ++index1)
        {
          for (byte index2 = (byte) 0; (int) index2 < (int) Regions.WORLD_SIZE; ++index2)
          {
            LevelItems.spawns[(int) index1, (int) index2] = new List<ItemSpawnpoint>();
            if (ReadWrite.fileExists(Level.info.path + (object) "/Spawns/Items_" + (string) (object) index1 + "_" + (string) (object) index2 + ".dat", false, false))
            {
              River river = new River(Level.info.path + (object) "/Spawns/Items_" + (string) (object) index1 + "_" + (string) (object) index2 + ".dat", false);
              if ((int) river.readByte() > 0)
              {
                ushort num = river.readUInt16();
                for (ushort index3 = (ushort) 0; (int) index3 < (int) num; ++index3)
                {
                  byte newType = river.readByte();
                  Vector3 newPoint = river.readSingleVector3();
                  LevelItems.spawns[(int) index1, (int) index2].Add(new ItemSpawnpoint(newType, newPoint));
                }
              }
              river.closeRiver();
            }
          }
        }
      }
    }

    public static void save()
    {
      Block block = new Block();
      block.writeByte(LevelItems.SAVEDATA_VERSION);
      block.writeByte((byte) LevelItems.tables.Count);
      for (byte index1 = (byte) 0; (int) index1 < LevelItems.tables.Count; ++index1)
      {
        ItemTable itemTable = LevelItems.tables[(int) index1];
        block.writeColor(itemTable.color);
        block.writeString(itemTable.name);
        block.write((object) (byte) itemTable.tiers.Count);
        for (byte index2 = (byte) 0; (int) index2 < itemTable.tiers.Count; ++index2)
        {
          ItemTier itemTier = itemTable.tiers[(int) index2];
          block.writeString(itemTier.name);
          block.writeSingle(itemTier.chance);
          block.writeByte((byte) itemTier.table.Count);
          for (byte index3 = (byte) 0; (int) index3 < itemTier.table.Count; ++index3)
          {
            ItemSpawn itemSpawn = itemTier.table[(int) index3];
            block.writeUInt16(itemSpawn.item);
          }
        }
      }
      ReadWrite.writeBlock(Level.info.path + "/Spawns/Items.dat", false, false, block);
      River river = new River(Level.info.path + "/Spawns/Jars.dat", false);
      river.writeByte(LevelItems.SAVEDATA_VERSION);
      for (byte index1 = (byte) 0; (int) index1 < (int) Regions.WORLD_SIZE; ++index1)
      {
        for (byte index2 = (byte) 0; (int) index2 < (int) Regions.WORLD_SIZE; ++index2)
        {
          List<ItemSpawnpoint> list = LevelItems.spawns[(int) index1, (int) index2];
          river.writeUInt16((ushort) list.Count);
          for (ushort index3 = (ushort) 0; (int) index3 < list.Count; ++index3)
          {
            ItemSpawnpoint itemSpawnpoint = list[(int) index3];
            river.writeByte(itemSpawnpoint.type);
            river.writeSingleVector3(itemSpawnpoint.point);
          }
        }
      }
      river.closeRiver();
    }
  }
}
