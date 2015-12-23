// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.LevelAnimals
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
  public class LevelAnimals
  {
    public static readonly byte SAVEDATA_VERSION = (byte) 2;
    private static Transform _models;
    private static List<AnimalTable> _tables;
    private static List<AnimalSpawnpoint> _spawns;

    public static Transform models
    {
      get
      {
        return LevelAnimals._models;
      }
    }

    public static List<AnimalTable> tables
    {
      get
      {
        return LevelAnimals._tables;
      }
    }

    public static List<AnimalSpawnpoint> spawns
    {
      get
      {
        return LevelAnimals._spawns;
      }
    }

    public static void setEnabled(bool isEnabled)
    {
      for (int index = 0; index < LevelAnimals.spawns.Count; ++index)
        LevelAnimals.spawns[index].setEnabled(isEnabled);
    }

    public static void addTable(string name)
    {
      if (LevelAnimals.tables.Count == (int) byte.MaxValue)
        return;
      LevelAnimals.tables.Add(new AnimalTable(name));
    }

    public static void removeTable()
    {
      LevelAnimals.tables.RemoveAt((int) EditorSpawns.selectedAnimal);
      List<AnimalSpawnpoint> list = new List<AnimalSpawnpoint>();
      for (int index = 0; index < LevelAnimals.spawns.Count; ++index)
      {
        AnimalSpawnpoint animalSpawnpoint = LevelAnimals.spawns[index];
        if ((int) animalSpawnpoint.type == (int) EditorSpawns.selectedAnimal)
        {
          Object.Destroy((Object) animalSpawnpoint.node.gameObject);
        }
        else
        {
          if ((int) animalSpawnpoint.type > (int) EditorSpawns.selectedAnimal)
            --animalSpawnpoint.type;
          list.Add(animalSpawnpoint);
        }
      }
      LevelAnimals._spawns = list;
      EditorSpawns.selectedAnimal = (byte) 0;
      if ((int) EditorSpawns.selectedAnimal >= LevelAnimals.tables.Count)
        return;
      EditorSpawns.animalSpawn.GetComponent<Renderer>().material.color = LevelAnimals.tables[(int) EditorSpawns.selectedAnimal].color;
    }

    public static void addSpawn(Vector3 point)
    {
      if ((int) EditorSpawns.selectedAnimal >= LevelAnimals.tables.Count)
        return;
      LevelAnimals.spawns.Add(new AnimalSpawnpoint(EditorSpawns.selectedAnimal, point));
    }

    public static void removeSpawn(Vector3 point, float radius)
    {
      radius *= radius;
      List<AnimalSpawnpoint> list = new List<AnimalSpawnpoint>();
      for (int index = 0; index < LevelAnimals.spawns.Count; ++index)
      {
        AnimalSpawnpoint animalSpawnpoint = LevelAnimals.spawns[index];
        if ((double) (animalSpawnpoint.point - point).sqrMagnitude < (double) radius)
          Object.Destroy((Object) animalSpawnpoint.node.gameObject);
        else
          list.Add(animalSpawnpoint);
      }
      LevelAnimals._spawns = list;
    }

    public static ushort getAnimal(AnimalSpawnpoint spawn)
    {
      return LevelAnimals.getAnimal(spawn.type);
    }

    public static ushort getAnimal(byte type)
    {
      return LevelAnimals.tables[(int) type].getAnimal();
    }

    public static void load()
    {
      LevelAnimals._models = new GameObject().transform;
      LevelAnimals.models.name = "Animals";
      LevelAnimals.models.parent = Level.spawns;
      LevelAnimals.models.tag = "Logic";
      LevelAnimals.models.gameObject.layer = LayerMasks.LOGIC;
      if (!Level.isEditor && !Provider.isServer)
        return;
      LevelAnimals._tables = new List<AnimalTable>();
      LevelAnimals._spawns = new List<AnimalSpawnpoint>();
      if (!ReadWrite.fileExists(Level.info.path + "/Spawns/Fauna.dat", false, false))
        return;
      River river = new River(Level.info.path + "/Spawns/Fauna.dat", false);
      river.readByte();
      byte num1 = river.readByte();
      for (byte index1 = (byte) 0; (int) index1 < (int) num1; ++index1)
      {
        Color newColor = river.readColor();
        string newName1 = river.readString();
        List<AnimalTier> newTiers = new List<AnimalTier>();
        byte num2 = river.readByte();
        for (byte index2 = (byte) 0; (int) index2 < (int) num2; ++index2)
        {
          string newName2 = river.readString();
          float newChance = river.readSingle();
          List<AnimalSpawn> newTable = new List<AnimalSpawn>();
          byte num3 = river.readByte();
          for (byte index3 = (byte) 0; (int) index3 < (int) num3; ++index3)
          {
            ushort newAnimal = river.readUInt16();
            newTable.Add(new AnimalSpawn(newAnimal));
          }
          newTiers.Add(new AnimalTier(newTable, newName2, newChance));
        }
        LevelAnimals.tables.Add(new AnimalTable(newTiers, newColor, newName1));
        if (!Level.isEditor)
          LevelAnimals.tables[(int) index1].buildTable();
      }
      ushort num4 = river.readUInt16();
      for (int index = 0; index < (int) num4; ++index)
        LevelAnimals.spawns.Add(new AnimalSpawnpoint(river.readByte(), river.readSingleVector3()));
      river.closeRiver();
    }

    public static void save()
    {
      River river = new River(Level.info.path + "/Spawns/Fauna.dat", false);
      river.writeByte(LevelAnimals.SAVEDATA_VERSION);
      river.writeByte((byte) LevelAnimals.tables.Count);
      for (byte index1 = (byte) 0; (int) index1 < LevelAnimals.tables.Count; ++index1)
      {
        AnimalTable animalTable = LevelAnimals.tables[(int) index1];
        river.writeColor(animalTable.color);
        river.writeString(animalTable.name);
        river.writeByte((byte) animalTable.tiers.Count);
        for (byte index2 = (byte) 0; (int) index2 < animalTable.tiers.Count; ++index2)
        {
          AnimalTier animalTier = animalTable.tiers[(int) index2];
          river.writeString(animalTier.name);
          river.writeSingle(animalTier.chance);
          river.writeByte((byte) animalTier.table.Count);
          for (byte index3 = (byte) 0; (int) index3 < animalTier.table.Count; ++index3)
          {
            AnimalSpawn animalSpawn = animalTier.table[(int) index3];
            river.writeUInt16(animalSpawn.animal);
          }
        }
      }
      river.writeUInt16((ushort) LevelAnimals.spawns.Count);
      for (int index = 0; index < LevelAnimals.spawns.Count; ++index)
      {
        AnimalSpawnpoint animalSpawnpoint = LevelAnimals.spawns[index];
        river.writeByte(animalSpawnpoint.type);
        river.writeSingleVector3(animalSpawnpoint.point);
      }
      river.closeRiver();
    }
  }
}
