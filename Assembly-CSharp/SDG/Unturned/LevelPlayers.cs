// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.LevelPlayers
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
  public class LevelPlayers
  {
    public static readonly byte SAVEDATA_VERSION = (byte) 3;
    private static Transform _models;
    private static List<PlayerSpawnpoint> _spawns;

    public static Transform models
    {
      get
      {
        return LevelPlayers._models;
      }
    }

    public static List<PlayerSpawnpoint> spawns
    {
      get
      {
        return LevelPlayers._spawns;
      }
    }

    public static void setEnabled(bool isEnabled)
    {
      for (int index = 0; index < LevelPlayers.spawns.Count; ++index)
        LevelPlayers.spawns[index].setEnabled(isEnabled);
    }

    public static void addSpawn(Vector3 point, float angle)
    {
      LevelPlayers.spawns.Add(new PlayerSpawnpoint(point, angle));
    }

    public static void removeSpawn(Vector3 point, float radius)
    {
      radius *= radius;
      List<PlayerSpawnpoint> list = new List<PlayerSpawnpoint>();
      for (int index = 0; index < LevelPlayers.spawns.Count; ++index)
      {
        PlayerSpawnpoint playerSpawnpoint = LevelPlayers.spawns[index];
        if ((double) (playerSpawnpoint.point - point).sqrMagnitude < (double) radius)
          Object.Destroy((Object) playerSpawnpoint.node.gameObject);
        else
          list.Add(playerSpawnpoint);
      }
      LevelPlayers._spawns = list;
    }

    public static PlayerSpawnpoint getSpawn()
    {
      if (LevelPlayers.spawns.Count == 0)
        return new PlayerSpawnpoint(new Vector3(0.0f, 256f, 0.0f), 0.0f);
      return LevelPlayers.spawns[Random.Range(0, LevelPlayers.spawns.Count)];
    }

    public static void load()
    {
      LevelPlayers._models = new GameObject().transform;
      LevelPlayers.models.name = "Players";
      LevelPlayers.models.parent = Level.spawns;
      LevelPlayers.models.tag = "Logic";
      LevelPlayers.models.gameObject.layer = LayerMasks.LOGIC;
      if (!Level.isEditor && !Provider.isServer)
        return;
      LevelPlayers._spawns = new List<PlayerSpawnpoint>();
      if (!ReadWrite.fileExists(Level.info.path + "/Spawns/Players.dat", false, false))
        return;
      River river = new River(Level.info.path + "/Spawns/Players.dat", false);
      byte num1 = river.readByte();
      if ((int) num1 > 1 && (int) num1 < 3)
        river.readSteamID();
      byte num2 = river.readByte();
      for (int index = 0; index < (int) num2; ++index)
        LevelPlayers.addSpawn(river.readSingleVector3(), (float) ((int) river.readByte() * 2));
      river.closeRiver();
    }

    public static void save()
    {
      River river = new River(Level.info.path + "/Spawns/Players.dat", false);
      river.writeByte(LevelPlayers.SAVEDATA_VERSION);
      river.writeByte((byte) LevelPlayers.spawns.Count);
      for (int index = 0; index < LevelPlayers.spawns.Count; ++index)
      {
        PlayerSpawnpoint playerSpawnpoint = LevelPlayers.spawns[index];
        river.writeSingleVector3(playerSpawnpoint.point);
        river.writeByte(MeasurementTool.angleToByte(playerSpawnpoint.angle));
      }
      river.closeRiver();
    }
  }
}
