// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.LevelZombies
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
  public class LevelZombies
  {
    public static readonly byte SAVEDATA_TABLE_VERSION = (byte) 5;
    public static readonly byte SAVEDATA_SPAWN_VERSION = (byte) 1;
    private static Transform _models;
    private static List<ZombieTable> _tables;
    private static List<ZombieSpawnpoint>[] _zombies;
    private static List<ZombieSpawnpoint>[,] _spawns;

    public static Transform models
    {
      get
      {
        return LevelZombies._models;
      }
    }

    public static List<ZombieTable> tables
    {
      get
      {
        return LevelZombies._tables;
      }
    }

    public static List<ZombieSpawnpoint>[] zombies
    {
      get
      {
        return LevelZombies._zombies;
      }
    }

    public static List<ZombieSpawnpoint>[,] spawns
    {
      get
      {
        return LevelZombies._spawns;
      }
    }

    public static void setEnabled(bool isEnabled)
    {
      for (byte index1 = (byte) 0; (int) index1 < (int) Regions.WORLD_SIZE; ++index1)
      {
        for (byte index2 = (byte) 0; (int) index2 < (int) Regions.WORLD_SIZE; ++index2)
        {
          for (int index3 = 0; index3 < LevelZombies.spawns[(int) index1, (int) index2].Count; ++index3)
            LevelZombies.spawns[(int) index1, (int) index2][index3].setEnabled(isEnabled);
        }
      }
    }

    public static void addTable(string name)
    {
      if (LevelZombies.tables.Count == (int) byte.MaxValue)
        return;
      LevelZombies.tables.Add(new ZombieTable(name));
    }

    public static void removeTable()
    {
      LevelZombies.tables.RemoveAt((int) EditorSpawns.selectedZombie);
      for (byte index1 = (byte) 0; (int) index1 < (int) Regions.WORLD_SIZE; ++index1)
      {
        for (byte index2 = (byte) 0; (int) index2 < (int) Regions.WORLD_SIZE; ++index2)
        {
          List<ZombieSpawnpoint> list = new List<ZombieSpawnpoint>();
          for (int index3 = 0; index3 < LevelZombies.spawns[(int) index1, (int) index2].Count; ++index3)
          {
            ZombieSpawnpoint zombieSpawnpoint = LevelZombies.spawns[(int) index1, (int) index2][index3];
            if ((int) zombieSpawnpoint.type == (int) EditorSpawns.selectedZombie)
            {
              Object.Destroy((Object) zombieSpawnpoint.node.gameObject);
            }
            else
            {
              if ((int) zombieSpawnpoint.type > (int) EditorSpawns.selectedZombie)
                --zombieSpawnpoint.type;
              list.Add(zombieSpawnpoint);
            }
          }
          LevelZombies._spawns[(int) index1, (int) index2] = list;
        }
      }
      EditorSpawns.selectedZombie = (byte) 0;
      if ((int) EditorSpawns.selectedZombie >= LevelZombies.tables.Count)
        return;
      EditorSpawns.zombieSpawn.GetComponent<Renderer>().material.color = LevelZombies.tables[(int) EditorSpawns.selectedZombie].color;
    }

    public static void addSpawn(Vector3 point)
    {
      byte x;
      byte y;
      if (!Regions.tryGetCoordinate(point, out x, out y) || (int) EditorSpawns.selectedZombie >= LevelZombies.tables.Count)
        return;
      LevelZombies.spawns[(int) x, (int) y].Add(new ZombieSpawnpoint(EditorSpawns.selectedZombie, point));
    }

    public static void removeSpawn(Vector3 point, float radius)
    {
      radius *= radius;
      for (byte index1 = (byte) 0; (int) index1 < (int) Regions.WORLD_SIZE; ++index1)
      {
        for (byte index2 = (byte) 0; (int) index2 < (int) Regions.WORLD_SIZE; ++index2)
        {
          List<ZombieSpawnpoint> list = new List<ZombieSpawnpoint>();
          for (int index3 = 0; index3 < LevelZombies.spawns[(int) index1, (int) index2].Count; ++index3)
          {
            ZombieSpawnpoint zombieSpawnpoint = LevelZombies.spawns[(int) index1, (int) index2][index3];
            if ((double) (zombieSpawnpoint.point - point).sqrMagnitude < (double) radius)
              Object.Destroy((Object) zombieSpawnpoint.node.gameObject);
            else
              list.Add(zombieSpawnpoint);
          }
          LevelZombies._spawns[(int) index1, (int) index2] = list;
        }
      }
    }

    public static void load()
    {
      LevelZombies._models = new GameObject().transform;
      LevelZombies.models.name = "Zombies";
      LevelZombies.models.parent = Level.spawns;
      LevelZombies.models.tag = "Logic";
      LevelZombies.models.gameObject.layer = LayerMasks.LOGIC;
      LevelZombies._tables = new List<ZombieTable>();
      if (ReadWrite.fileExists(Level.info.path + "/Spawns/Zombies.dat", false, false))
      {
        Block block = ReadWrite.readBlock(Level.info.path + "/Spawns/Zombies.dat", false, false, (byte) 0);
        byte num1 = block.readByte();
        if ((int) num1 > 3 && (int) num1 < 5)
          block.readSteamID();
        if ((int) num1 > 2)
        {
          byte num2 = block.readByte();
          for (byte index1 = (byte) 0; (int) index1 < (int) num2; ++index1)
          {
            Color newColor = block.readColor();
            string newName = block.readString();
            bool newMega = block.readBoolean();
            ushort newHealth = block.readUInt16();
            byte newDamage = block.readByte();
            byte newLoot = block.readByte();
            ZombieSlot[] newSlots = new ZombieSlot[4];
            byte num3 = block.readByte();
            for (byte index2 = (byte) 0; (int) index2 < (int) num3; ++index2)
            {
              List<ZombieCloth> newTable = new List<ZombieCloth>();
              float newChance = block.readSingle();
              byte num4 = block.readByte();
              for (byte index3 = (byte) 0; (int) index3 < (int) num4; ++index3)
              {
                ushort num5 = block.readUInt16();
                if ((ItemAsset) Assets.find(EAssetType.ITEM, num5) != null)
                  newTable.Add(new ZombieCloth(num5));
              }
              newSlots[(int) index2] = new ZombieSlot(newChance, newTable);
            }
            LevelZombies.tables.Add(new ZombieTable(newSlots, newColor, newName, newMega, newHealth, newDamage, newLoot));
          }
        }
        else
        {
          byte num2 = block.readByte();
          for (byte index1 = (byte) 0; (int) index1 < (int) num2; ++index1)
          {
            Color newColor = block.readColor();
            string newName = block.readString();
            byte newLoot = block.readByte();
            ZombieSlot[] newSlots = new ZombieSlot[4];
            byte num3 = block.readByte();
            for (byte index2 = (byte) 0; (int) index2 < (int) num3; ++index2)
            {
              List<ZombieCloth> newTable = new List<ZombieCloth>();
              float newChance = block.readSingle();
              byte num4 = block.readByte();
              for (byte index3 = (byte) 0; (int) index3 < (int) num4; ++index3)
              {
                ushort num5 = block.readUInt16();
                if ((ItemAsset) Assets.find(EAssetType.ITEM, num5) != null)
                  newTable.Add(new ZombieCloth(num5));
              }
              newSlots[(int) index2] = new ZombieSlot(newChance, newTable);
            }
            LevelZombies.tables.Add(new ZombieTable(newSlots, newColor, newName, false, (ushort) 100, (byte) 15, newLoot));
          }
        }
      }
      LevelZombies._spawns = new List<ZombieSpawnpoint>[(int) Regions.WORLD_SIZE, (int) Regions.WORLD_SIZE];
      for (byte index1 = (byte) 0; (int) index1 < (int) Regions.WORLD_SIZE; ++index1)
      {
        for (byte index2 = (byte) 0; (int) index2 < (int) Regions.WORLD_SIZE; ++index2)
          LevelZombies.spawns[(int) index1, (int) index2] = new List<ZombieSpawnpoint>();
      }
      if (Level.isEditor)
      {
        if (ReadWrite.fileExists(Level.info.path + "/Spawns/Animals.dat", false, false))
        {
          River river = new River(Level.info.path + "/Spawns/Animals.dat", false);
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
                  LevelZombies.spawns[(int) index1, (int) index2].Add(new ZombieSpawnpoint(newType, newPoint));
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
              LevelZombies.spawns[(int) index1, (int) index2] = new List<ZombieSpawnpoint>();
              if (ReadWrite.fileExists(Level.info.path + (object) "/Spawns/Animals_" + (string) (object) index1 + "_" + (string) (object) index2 + ".dat", false, false))
              {
                River river = new River(Level.info.path + (object) "/Spawns/Animals_" + (string) (object) index1 + "_" + (string) (object) index2 + ".dat", false);
                if ((int) river.readByte() > 0)
                {
                  ushort num = river.readUInt16();
                  for (ushort index3 = (ushort) 0; (int) index3 < (int) num; ++index3)
                  {
                    byte newType = river.readByte();
                    Vector3 newPoint = river.readSingleVector3();
                    LevelZombies.spawns[(int) index1, (int) index2].Add(new ZombieSpawnpoint(newType, newPoint));
                  }
                  river.closeRiver();
                }
              }
            }
          }
        }
      }
      else
      {
        if (!Provider.isServer)
          return;
        LevelZombies._zombies = new List<ZombieSpawnpoint>[LevelNavigation.bounds.Count];
        for (int index = 0; index < LevelZombies.zombies.Length; ++index)
          LevelZombies.zombies[index] = new List<ZombieSpawnpoint>();
        if (ReadWrite.fileExists(Level.info.path + "/Spawns/Animals.dat", false, false))
        {
          River river = new River(Level.info.path + "/Spawns/Animals.dat", false);
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
                  Vector3 vector3 = river.readSingleVector3();
                  byte bound;
                  if (LevelNavigation.tryGetBounds(vector3, out bound) && LevelNavigation.checkNavigation(vector3))
                    LevelZombies.zombies[(int) bound].Add(new ZombieSpawnpoint(newType, vector3));
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
              if (ReadWrite.fileExists(Level.info.path + (object) "/Spawns/Animals_" + (string) (object) index1 + "_" + (string) (object) index2 + ".dat", false, false))
              {
                River river = new River(Level.info.path + (object) "/Spawns/Animals_" + (string) (object) index1 + "_" + (string) (object) index2 + ".dat", false);
                if ((int) river.readByte() > 0)
                {
                  ushort num = river.readUInt16();
                  for (ushort index3 = (ushort) 0; (int) index3 < (int) num; ++index3)
                  {
                    byte newType = river.readByte();
                    Vector3 vector3 = river.readSingleVector3();
                    byte bound;
                    if (LevelNavigation.tryGetBounds(vector3, out bound) && LevelNavigation.checkNavigation(vector3))
                      LevelZombies.zombies[(int) bound].Add(new ZombieSpawnpoint(newType, vector3));
                  }
                  river.closeRiver();
                }
              }
            }
          }
        }
      }
    }

    public static void save()
    {
      Block block = new Block();
      block.writeByte(LevelZombies.SAVEDATA_TABLE_VERSION);
      block.writeByte((byte) LevelZombies.tables.Count);
      for (byte index1 = (byte) 0; (int) index1 < LevelZombies.tables.Count; ++index1)
      {
        ZombieTable zombieTable = LevelZombies.tables[(int) index1];
        block.writeColor(zombieTable.color);
        block.writeString(zombieTable.name);
        block.writeBoolean(zombieTable.isMega);
        block.writeUInt16(zombieTable.health);
        block.writeByte(zombieTable.damage);
        block.writeByte(zombieTable.loot);
        block.write((object) (byte) zombieTable.slots.Length);
        for (byte index2 = (byte) 0; (int) index2 < zombieTable.slots.Length; ++index2)
        {
          ZombieSlot zombieSlot = zombieTable.slots[(int) index2];
          block.writeSingle(zombieSlot.chance);
          block.writeByte((byte) zombieSlot.table.Count);
          for (byte index3 = (byte) 0; (int) index3 < zombieSlot.table.Count; ++index3)
          {
            ZombieCloth zombieCloth = zombieSlot.table[(int) index3];
            block.writeUInt16(zombieCloth.item);
          }
        }
      }
      ReadWrite.writeBlock(Level.info.path + "/Spawns/Zombies.dat", false, false, block);
      River river = new River(Level.info.path + "/Spawns/Animals.dat", false);
      river.writeByte(LevelZombies.SAVEDATA_SPAWN_VERSION);
      for (byte index1 = (byte) 0; (int) index1 < (int) Regions.WORLD_SIZE; ++index1)
      {
        for (byte index2 = (byte) 0; (int) index2 < (int) Regions.WORLD_SIZE; ++index2)
        {
          List<ZombieSpawnpoint> list = LevelZombies.spawns[(int) index1, (int) index2];
          river.writeUInt16((ushort) list.Count);
          for (ushort index3 = (ushort) 0; (int) index3 < list.Count; ++index3)
          {
            ZombieSpawnpoint zombieSpawnpoint = list[(int) index3];
            river.writeByte(zombieSpawnpoint.type);
            river.writeSingleVector3(zombieSpawnpoint.point);
          }
        }
      }
      river.closeRiver();
    }
  }
}
