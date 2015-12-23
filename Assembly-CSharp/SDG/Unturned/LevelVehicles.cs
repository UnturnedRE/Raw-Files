// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.LevelVehicles
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
  public class LevelVehicles
  {
    public static readonly byte SAVEDATA_VERSION = (byte) 3;
    private static Transform _models;
    private static List<VehicleTable> _tables;
    private static List<VehicleSpawnpoint> _spawns;

    public static Transform models
    {
      get
      {
        return LevelVehicles._models;
      }
    }

    public static List<VehicleTable> tables
    {
      get
      {
        return LevelVehicles._tables;
      }
    }

    public static List<VehicleSpawnpoint> spawns
    {
      get
      {
        return LevelVehicles._spawns;
      }
    }

    public static void setEnabled(bool isEnabled)
    {
      for (int index = 0; index < LevelVehicles.spawns.Count; ++index)
        LevelVehicles.spawns[index].setEnabled(isEnabled);
    }

    public static void addTable(string name)
    {
      if (LevelVehicles.tables.Count == (int) byte.MaxValue)
        return;
      LevelVehicles.tables.Add(new VehicleTable(name));
    }

    public static void removeTable()
    {
      LevelVehicles.tables.RemoveAt((int) EditorSpawns.selectedVehicle);
      List<VehicleSpawnpoint> list = new List<VehicleSpawnpoint>();
      for (int index = 0; index < LevelVehicles.spawns.Count; ++index)
      {
        VehicleSpawnpoint vehicleSpawnpoint = LevelVehicles.spawns[index];
        if ((int) vehicleSpawnpoint.type == (int) EditorSpawns.selectedVehicle)
        {
          Object.Destroy((Object) vehicleSpawnpoint.node.gameObject);
        }
        else
        {
          if ((int) vehicleSpawnpoint.type > (int) EditorSpawns.selectedVehicle)
            --vehicleSpawnpoint.type;
          list.Add(vehicleSpawnpoint);
        }
      }
      LevelVehicles._spawns = list;
      EditorSpawns.selectedVehicle = (byte) 0;
      if ((int) EditorSpawns.selectedVehicle >= LevelVehicles.tables.Count)
        return;
      EditorSpawns.vehicleSpawn.GetComponent<Renderer>().material.color = LevelVehicles.tables[(int) EditorSpawns.selectedVehicle].color;
    }

    public static void addSpawn(Vector3 point, float angle)
    {
      if ((int) EditorSpawns.selectedVehicle >= LevelVehicles.tables.Count)
        return;
      LevelVehicles.spawns.Add(new VehicleSpawnpoint(EditorSpawns.selectedVehicle, point, angle));
    }

    public static void removeSpawn(Vector3 point, float radius)
    {
      radius *= radius;
      List<VehicleSpawnpoint> list = new List<VehicleSpawnpoint>();
      for (int index = 0; index < LevelVehicles.spawns.Count; ++index)
      {
        VehicleSpawnpoint vehicleSpawnpoint = LevelVehicles.spawns[index];
        if ((double) (vehicleSpawnpoint.point - point).sqrMagnitude < (double) radius)
          Object.Destroy((Object) vehicleSpawnpoint.node.gameObject);
        else
          list.Add(vehicleSpawnpoint);
      }
      LevelVehicles._spawns = list;
    }

    public static ushort getVehicle(VehicleSpawnpoint spawn)
    {
      return LevelVehicles.getVehicle(spawn.type);
    }

    public static ushort getVehicle(byte type)
    {
      return LevelVehicles.tables[(int) type].getVehicle();
    }

    public static void load()
    {
      LevelVehicles._models = new GameObject().transform;
      LevelVehicles.models.name = "Vehicles";
      LevelVehicles.models.parent = Level.spawns;
      LevelVehicles.models.tag = "Logic";
      LevelVehicles.models.gameObject.layer = LayerMasks.LOGIC;
      if (!Level.isEditor && !Provider.isServer)
        return;
      LevelVehicles._tables = new List<VehicleTable>();
      LevelVehicles._spawns = new List<VehicleSpawnpoint>();
      if (!ReadWrite.fileExists(Level.info.path + "/Spawns/Vehicles.dat", false, false))
        return;
      River river = new River(Level.info.path + "/Spawns/Vehicles.dat", false);
      byte num1 = river.readByte();
      if ((int) num1 > 1 && (int) num1 < 3)
        river.readSteamID();
      byte num2 = river.readByte();
      for (byte index1 = (byte) 0; (int) index1 < (int) num2; ++index1)
      {
        Color newColor = river.readColor();
        string newName1 = river.readString();
        List<VehicleTier> newTiers = new List<VehicleTier>();
        byte num3 = river.readByte();
        for (byte index2 = (byte) 0; (int) index2 < (int) num3; ++index2)
        {
          string newName2 = river.readString();
          float newChance = river.readSingle();
          List<VehicleSpawn> newTable = new List<VehicleSpawn>();
          byte num4 = river.readByte();
          for (byte index3 = (byte) 0; (int) index3 < (int) num4; ++index3)
          {
            ushort newVehicle = river.readUInt16();
            newTable.Add(new VehicleSpawn(newVehicle));
          }
          newTiers.Add(new VehicleTier(newTable, newName2, newChance));
        }
        LevelVehicles.tables.Add(new VehicleTable(newTiers, newColor, newName1));
        if (!Level.isEditor)
          LevelVehicles.tables[(int) index1].buildTable();
      }
      ushort num5 = river.readUInt16();
      for (int index = 0; index < (int) num5; ++index)
        LevelVehicles.spawns.Add(new VehicleSpawnpoint(river.readByte(), river.readSingleVector3(), (float) ((int) river.readByte() * 2)));
      river.closeRiver();
    }

    public static void save()
    {
      River river = new River(Level.info.path + "/Spawns/Vehicles.dat", false);
      river.writeByte(LevelVehicles.SAVEDATA_VERSION);
      river.writeByte((byte) LevelVehicles.tables.Count);
      for (byte index1 = (byte) 0; (int) index1 < LevelVehicles.tables.Count; ++index1)
      {
        VehicleTable vehicleTable = LevelVehicles.tables[(int) index1];
        river.writeColor(vehicleTable.color);
        river.writeString(vehicleTable.name);
        river.writeByte((byte) vehicleTable.tiers.Count);
        for (byte index2 = (byte) 0; (int) index2 < vehicleTable.tiers.Count; ++index2)
        {
          VehicleTier vehicleTier = vehicleTable.tiers[(int) index2];
          river.writeString(vehicleTier.name);
          river.writeSingle(vehicleTier.chance);
          river.writeByte((byte) vehicleTier.table.Count);
          for (byte index3 = (byte) 0; (int) index3 < vehicleTier.table.Count; ++index3)
          {
            VehicleSpawn vehicleSpawn = vehicleTier.table[(int) index3];
            river.writeUInt16(vehicleSpawn.vehicle);
          }
        }
      }
      river.writeUInt16((ushort) LevelVehicles.spawns.Count);
      for (int index = 0; index < LevelVehicles.spawns.Count; ++index)
      {
        VehicleSpawnpoint vehicleSpawnpoint = LevelVehicles.spawns[index];
        river.writeByte(vehicleSpawnpoint.type);
        river.writeSingleVector3(vehicleSpawnpoint.point);
        river.writeByte(MeasurementTool.angleToByte(vehicleSpawnpoint.angle));
      }
      river.closeRiver();
    }
  }
}
