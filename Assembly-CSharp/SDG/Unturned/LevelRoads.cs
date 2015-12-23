// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.LevelRoads
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
  public class LevelRoads
  {
    public static readonly byte SAVEDATA_ROADS_VERSION = (byte) 1;
    public static readonly byte SAVEDATA_PATHS_VERSION = (byte) 2;
    private static Transform _models;
    private static RoadMaterial[] _materials;
    private static List<Road> roads;

    public static Transform models
    {
      get
      {
        return LevelRoads._models;
      }
    }

    public static RoadMaterial[] materials
    {
      get
      {
        return LevelRoads._materials;
      }
    }

    public static void setEnabled(bool isEnabled)
    {
      for (int index = 0; index < LevelRoads.roads.Count; ++index)
        LevelRoads.roads[index].setEnabled(isEnabled);
    }

    public static Transform addRoad(Vector3 point)
    {
      LevelRoads.roads.Add(new Road(EditorRoads.selected));
      return LevelRoads.roads[LevelRoads.roads.Count - 1].addPoint((Transform) null, point);
    }

    public static void removeRoad(Transform select)
    {
      for (int index1 = 0; index1 < LevelRoads.roads.Count; ++index1)
      {
        for (int index2 = 0; index2 < LevelRoads.roads[index1].points.Count; ++index2)
        {
          if ((Object) LevelRoads.roads[index1].paths[index2] == (Object) select)
          {
            LevelRoads.roads[index1].remove();
            LevelRoads.roads.RemoveAt(index1);
            return;
          }
        }
      }
    }

    public static Road getRoad(Transform select)
    {
      for (int index1 = 0; index1 < LevelRoads.roads.Count; ++index1)
      {
        for (int index2 = 0; index2 < LevelRoads.roads[index1].points.Count; ++index2)
        {
          if ((Object) LevelRoads.roads[index1].paths[index2] == (Object) select)
            return LevelRoads.roads[index1];
        }
      }
      return (Road) null;
    }

    public static void bakeRoads()
    {
      for (int index = 0; index < LevelRoads.roads.Count; ++index)
        LevelRoads.roads[index].settlePoints();
      LevelRoads.buildMeshes();
    }

    public static void load()
    {
      LevelRoads._models = new GameObject().transform;
      LevelRoads.models.name = "Roads";
      LevelRoads.models.parent = Level.level;
      LevelRoads.models.tag = "Logic";
      LevelRoads.models.gameObject.layer = LayerMasks.LOGIC;
      if (ReadWrite.fileExists(Level.info.path + "/Environment/Roads.unity3d", false, false))
      {
        Bundle bundle = Bundles.getBundle(Level.info.path + "/Environment/Roads.unity3d", false);
        Object[] objectArray = bundle.load();
        bundle.unload();
        LevelRoads._materials = new RoadMaterial[objectArray.Length];
        for (int index = 0; index < LevelRoads.materials.Length; ++index)
          LevelRoads.materials[index] = new RoadMaterial((Texture2D) objectArray[index]);
      }
      else
        LevelRoads._materials = new RoadMaterial[0];
      LevelRoads.roads = new List<Road>();
      if (ReadWrite.fileExists(Level.info.path + "/Environment/Roads.dat", false, false))
      {
        River river = new River(Level.info.path + "/Environment/Roads.dat", false);
        if ((int) river.readByte() > 0)
        {
          byte num = river.readByte();
          for (byte index = (byte) 0; (int) index < (int) num && (int) index < LevelRoads.materials.Length; ++index)
          {
            LevelRoads.materials[(int) index].width = river.readSingle();
            LevelRoads.materials[(int) index].height = river.readSingle();
            LevelRoads.materials[(int) index].depth = river.readSingle();
            LevelRoads.materials[(int) index].isConcrete = river.readBoolean();
          }
        }
        river.closeRiver();
      }
      if (ReadWrite.fileExists(Level.info.path + "/Environment/Paths.dat", false, false))
      {
        River river = new River(Level.info.path + "/Environment/Paths.dat", false);
        byte num1 = river.readByte();
        if ((int) num1 > 1)
        {
          ushort num2 = river.readUInt16();
          for (ushort index1 = (ushort) 0; (int) index1 < (int) num2; ++index1)
          {
            ushort num3 = river.readUInt16();
            byte newMaterial = river.readByte();
            List<Vector3> newPoints = new List<Vector3>();
            for (ushort index2 = (ushort) 0; (int) index2 < (int) num3; ++index2)
            {
              Vector3 vector3 = river.readSingleVector3();
              newPoints.Add(vector3);
            }
            LevelRoads.roads.Add(new Road(newMaterial, newPoints));
          }
        }
        else if ((int) num1 > 0)
        {
          byte num2 = river.readByte();
          for (byte index1 = (byte) 0; (int) index1 < (int) num2; ++index1)
          {
            byte num3 = river.readByte();
            byte newMaterial = river.readByte();
            List<Vector3> newPoints = new List<Vector3>();
            for (byte index2 = (byte) 0; (int) index2 < (int) num3; ++index2)
            {
              Vector3 vector3 = river.readSingleVector3();
              newPoints.Add(vector3);
            }
            LevelRoads.roads.Add(new Road(newMaterial, newPoints));
          }
        }
        river.closeRiver();
      }
      LevelRoads.buildMeshes();
    }

    public static void save()
    {
      River river1 = new River(Level.info.path + "/Environment/Roads.dat", false);
      river1.writeByte(LevelRoads.SAVEDATA_ROADS_VERSION);
      river1.writeByte((byte) LevelRoads.materials.Length);
      for (byte index = (byte) 0; (int) index < LevelRoads.materials.Length; ++index)
      {
        river1.writeSingle(LevelRoads.materials[(int) index].width);
        river1.writeSingle(LevelRoads.materials[(int) index].height);
        river1.writeSingle(LevelRoads.materials[(int) index].depth);
        river1.writeBoolean(LevelRoads.materials[(int) index].isConcrete);
      }
      river1.closeRiver();
      River river2 = new River(Level.info.path + "/Environment/Paths.dat", false);
      river2.writeByte(LevelRoads.SAVEDATA_PATHS_VERSION);
      ushort num = (ushort) 0;
      for (ushort index = (ushort) 0; (int) index < LevelRoads.roads.Count; ++index)
      {
        if (LevelRoads.roads[(int) index].points.Count > 1)
          ++num;
      }
      river2.writeUInt16(num);
      for (byte index1 = (byte) 0; (int) index1 < LevelRoads.roads.Count; ++index1)
      {
        List<Vector3> points = LevelRoads.roads[(int) index1].points;
        if (points.Count > 1)
        {
          river2.writeUInt16((ushort) points.Count);
          river2.writeByte(LevelRoads.roads[(int) index1].material);
          for (ushort index2 = (ushort) 0; (int) index2 < points.Count; ++index2)
            river2.writeSingleVector3(points[(int) index2]);
        }
      }
      river2.closeRiver();
    }

    private static void buildMeshes()
    {
      for (int index = 0; index < LevelRoads.roads.Count; ++index)
        LevelRoads.roads[index].buildMesh();
    }
  }
}
