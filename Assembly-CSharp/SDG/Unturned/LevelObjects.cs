// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.LevelObjects
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
  public class LevelObjects : MonoBehaviour
  {
    public static readonly byte SAVEDATA_VERSION = (byte) 3;
    public static readonly byte OBJECT_REGIONS = (byte) 4;
    private static IReun[] reun;
    private static int frame;
    private static Transform _models;
    private static List<LevelObject>[,] _objects;
    private static byte[] _hash;
    private static bool[,] _regions;

    public static Transform models
    {
      get
      {
        return LevelObjects._models;
      }
    }

    public static List<LevelObject>[,] objects
    {
      get
      {
        return LevelObjects._objects;
      }
    }

    public static byte[] hash
    {
      get
      {
        return LevelObjects._hash;
      }
    }

    public static bool[,] regions
    {
      get
      {
        return LevelObjects._regions;
      }
    }

    public static void undo()
    {
      if (LevelObjects.frame >= LevelObjects.reun.Length - 1)
        return;
      if (LevelObjects.reun[LevelObjects.frame] != null)
        LevelObjects.reun[LevelObjects.frame].undo();
      if (LevelObjects.reun[LevelObjects.frame + 1] == null)
        return;
      ++LevelObjects.frame;
    }

    public static void redo()
    {
      if (LevelObjects.frame < 0)
        return;
      if (LevelObjects.reun[LevelObjects.frame] != null)
        LevelObjects.reun[LevelObjects.frame].redo();
      if (LevelObjects.frame <= 0 || LevelObjects.reun[LevelObjects.frame - 1] == null)
        return;
      --LevelObjects.frame;
    }

    public static Transform register(IReun newReun)
    {
      if (LevelObjects.frame > 0)
      {
        LevelObjects.reun = new IReun[LevelObjects.reun.Length];
        LevelObjects.frame = 0;
      }
      for (int index = LevelObjects.reun.Length - 1; index > 0; --index)
        LevelObjects.reun[index] = LevelObjects.reun[index - 1];
      LevelObjects.reun[0] = newReun;
      return LevelObjects.reun[0].redo();
    }

    public static void transformObject(Transform select, Vector3 toPosition, Quaternion toRotation, Vector3 fromPosition, Quaternion fromRotation)
    {
      byte x1;
      byte y1;
      if (Regions.tryGetCoordinate(fromPosition, out x1, out y1))
      {
        byte x2;
        byte y2;
        if (Regions.tryGetCoordinate(toPosition, out x2, out y2))
        {
          if ((int) x1 != (int) x2 || (int) y1 != (int) y2)
          {
            LevelObject levelObject = (LevelObject) null;
            for (int index = 0; index < LevelObjects.objects[(int) x1, (int) y1].Count; ++index)
            {
              if ((Object) LevelObjects.objects[(int) x1, (int) y1][index].transform == (Object) select)
              {
                levelObject = LevelObjects.objects[(int) x1, (int) y1][index];
                LevelObjects.objects[(int) x1, (int) y1].RemoveAt(index);
                break;
              }
            }
            if (levelObject != null)
              LevelObjects.objects[(int) x2, (int) y2].Add(levelObject);
          }
          select.position = toPosition;
          select.rotation = toRotation;
        }
        else
        {
          select.position = fromPosition;
          select.rotation = fromRotation;
        }
      }
      else
      {
        select.position = fromPosition;
        select.rotation = fromRotation;
      }
    }

    public static void registerTransformObject(Transform select, Vector3 toPosition, Quaternion toRotation, Vector3 fromPosition, Quaternion fromRotation)
    {
      LevelObjects.register((IReun) new ReunObjectTransform(select, fromPosition, fromRotation, toPosition, toRotation));
    }

    public static Transform addObject(Vector3 position, Quaternion rotation, ushort id)
    {
      byte x;
      byte y;
      if (!Regions.tryGetCoordinate(position, out x, out y))
        return (Transform) null;
      LevelObject levelObject = new LevelObject(position, rotation, id);
      levelObject.enable();
      LevelObjects.objects[(int) x, (int) y].Add(levelObject);
      return levelObject.transform;
    }

    public static Transform registerAddObject(Vector3 position, Quaternion rotation, ushort id)
    {
      return LevelObjects.register((IReun) new ReunObjectAdd(id, position, rotation));
    }

    public static void removeObject(Transform select)
    {
      byte x;
      byte y;
      if (!Regions.tryGetCoordinate(select.position, out x, out y))
        return;
      for (int index = 0; index < LevelObjects.objects[(int) x, (int) y].Count; ++index)
      {
        if ((Object) LevelObjects.objects[(int) x, (int) y][index].transform == (Object) select)
        {
          Object.Destroy((Object) LevelObjects.objects[(int) x, (int) y][index].transform.gameObject);
          LevelObjects.objects[(int) x, (int) y].RemoveAt(index);
          break;
        }
      }
    }

    public static void registerRemoveObject(Transform select)
    {
      byte x;
      byte y;
      if (!Regions.tryGetCoordinate(select.position, out x, out y))
        return;
      for (int index = 0; index < LevelObjects.objects[(int) x, (int) y].Count; ++index)
      {
        if ((Object) LevelObjects.objects[(int) x, (int) y][index].transform == (Object) select)
        {
          LevelObjects.register((IReun) new ReunObjectRemove(select, LevelObjects.objects[(int) x, (int) y][index].id, select.position, select.rotation));
          break;
        }
      }
    }

    public static ushort getID(Transform select)
    {
      byte x;
      byte y;
      if (Regions.tryGetCoordinate(select.position, out x, out y))
      {
        for (int index = 0; index < LevelObjects.objects[(int) x, (int) y].Count; ++index)
        {
          if ((Object) LevelObjects.objects[(int) x, (int) y][index].transform == (Object) select)
            return LevelObjects.objects[(int) x, (int) y][index].id;
        }
      }
      return (ushort) 0;
    }

    public static void load()
    {
      LevelObjects._models = new GameObject().transform;
      LevelObjects.models.name = "Objects";
      LevelObjects.models.parent = Level.level;
      LevelObjects.models.tag = "Logic";
      LevelObjects.models.gameObject.layer = LayerMasks.LOGIC;
      LevelObjects._objects = new List<LevelObject>[(int) Regions.WORLD_SIZE, (int) Regions.WORLD_SIZE];
      LevelObjects._regions = new bool[(int) Regions.WORLD_SIZE, (int) Regions.WORLD_SIZE];
      for (byte index1 = (byte) 0; (int) index1 < (int) Regions.WORLD_SIZE; ++index1)
      {
        for (byte index2 = (byte) 0; (int) index2 < (int) Regions.WORLD_SIZE; ++index2)
          LevelObjects.objects[(int) index1, (int) index2] = new List<LevelObject>();
      }
      if (ReadWrite.fileExists(Level.info.path + "/Level/Objects.dat", false, false))
      {
        River river = new River(Level.info.path + "/Level/Objects.dat", false);
        byte num1 = river.readByte();
        bool flag = true;
        if ((int) num1 > 0)
        {
          if ((int) num1 > 1 && (int) num1 < 3)
            river.readSteamID();
          for (byte index1 = (byte) 0; (int) index1 < (int) Regions.WORLD_SIZE; ++index1)
          {
            for (byte index2 = (byte) 0; (int) index2 < (int) Regions.WORLD_SIZE; ++index2)
            {
              ushort num2 = river.readUInt16();
              for (ushort index3 = (ushort) 0; (int) index3 < (int) num2; ++index3)
              {
                Vector3 newPoint = river.readSingleVector3();
                Quaternion newRotation = river.readSingleQuaternion();
                ushort newID = river.readUInt16();
                if ((int) newID != 0)
                {
                  LevelObject levelObject = new LevelObject(newPoint, newRotation, newID);
                  if (levelObject.asset == null)
                    flag = false;
                  LevelObjects.objects[(int) index1, (int) index2].Add(levelObject);
                }
              }
            }
          }
        }
        LevelObjects._hash = !flag ? new byte[20] : river.getHash();
        river.closeRiver();
      }
      else
      {
        for (byte index1 = (byte) 0; (int) index1 < (int) Regions.WORLD_SIZE; ++index1)
        {
          for (byte index2 = (byte) 0; (int) index2 < (int) Regions.WORLD_SIZE; ++index2)
          {
            if (ReadWrite.fileExists(Level.info.path + (object) "/Objects/Objects_" + (string) (object) index1 + "_" + (string) (object) index2 + ".dat", false, false))
            {
              River river = new River(Level.info.path + (object) "/Objects/Objects_" + (string) (object) index1 + "_" + (string) (object) index2 + ".dat", false);
              if ((int) river.readByte() > 0)
              {
                ushort num = river.readUInt16();
                for (ushort index3 = (ushort) 0; (int) index3 < (int) num; ++index3)
                {
                  Vector3 position = river.readSingleVector3();
                  Quaternion rotation = river.readSingleQuaternion();
                  ushort id = river.readUInt16();
                  if ((int) id != 0)
                    LevelObjects.addObject(position, rotation, id);
                }
              }
              river.closeRiver();
            }
          }
        }
        LevelObjects._hash = new byte[20];
      }
      if (!Level.isEditor)
        return;
      LevelObjects.reun = new IReun[8];
      LevelObjects.frame = 0;
    }

    public static void save()
    {
      River river = new River(Level.info.path + "/Level/Objects.dat", false);
      river.writeByte(LevelObjects.SAVEDATA_VERSION);
      for (byte index1 = (byte) 0; (int) index1 < (int) Regions.WORLD_SIZE; ++index1)
      {
        for (byte index2 = (byte) 0; (int) index2 < (int) Regions.WORLD_SIZE; ++index2)
        {
          List<LevelObject> list = LevelObjects.objects[(int) index1, (int) index2];
          river.writeUInt16((ushort) list.Count);
          for (ushort index3 = (ushort) 0; (int) index3 < list.Count; ++index3)
          {
            LevelObject levelObject = list[(int) index3];
            if (levelObject != null && (Object) levelObject.transform != (Object) null && (int) levelObject.id != 0)
            {
              river.writeSingleVector3(levelObject.transform.position);
              river.writeSingleQuaternion(levelObject.transform.rotation);
              river.writeUInt16(levelObject.id);
            }
            else
            {
              river.writeSingleVector3(Vector3.zero);
              river.writeSingleQuaternion(Quaternion.identity);
              river.writeUInt16((ushort) 0);
              Debug.LogError((object) ("Found invalid object at " + (object) index1 + ", " + (string) (object) index2 + " with model: " + (string) (object) levelObject.transform + " and ID: " + (string) (object) levelObject.id));
            }
          }
        }
      }
      river.closeRiver();
    }

    private static void onRegionUpdated(byte old_x, byte old_y, byte new_x, byte new_y)
    {
      LevelObjects.onRegionUpdated((Player) null, old_x, old_y, new_x, new_y);
    }

    private static void onRegionUpdated(Player player, byte old_x, byte old_y, byte new_x, byte new_y)
    {
      for (byte x_0 = (byte) 0; (int) x_0 < (int) Regions.WORLD_SIZE; ++x_0)
      {
        for (byte y_0 = (byte) 0; (int) y_0 < (int) Regions.WORLD_SIZE; ++y_0)
        {
          if (LevelObjects.regions[(int) x_0, (int) y_0] && !Regions.checkArea(x_0, y_0, new_x, new_y, LevelObjects.OBJECT_REGIONS))
          {
            List<LevelObject> list = LevelObjects.objects[(int) x_0, (int) y_0];
            for (int index = 0; index < list.Count; ++index)
              list[index].disable();
            LevelObjects.regions[(int) x_0, (int) y_0] = false;
          }
        }
      }
      if (Regions.checkSafe(new_x, new_y))
      {
        for (int index1 = (int) new_x - (int) LevelObjects.OBJECT_REGIONS; index1 <= (int) new_x + (int) LevelObjects.OBJECT_REGIONS; ++index1)
        {
          for (int index2 = (int) new_y - (int) LevelObjects.OBJECT_REGIONS; index2 <= (int) new_y + (int) LevelObjects.OBJECT_REGIONS; ++index2)
          {
            if (Regions.checkSafe((byte) index1, (byte) index2) && !LevelObjects.regions[index1, index2])
            {
              List<LevelObject> list = LevelObjects.objects[index1, index2];
              for (int index3 = 0; index3 < list.Count; ++index3)
                list[index3].enable();
              LevelObjects.regions[index1, index2] = true;
            }
          }
        }
      }
      Level.isLoadingArea = false;
    }

    private static void onPlayerCreated(Player player)
    {
      if (!player.channel.isOwner)
        return;
      Player.player.movement.onRegionUpdated += new PlayerRegionUpdated(LevelObjects.onRegionUpdated);
    }

    private static void onEditorCreated()
    {
      Editor.editor.movement.onRegionUpdated += new EditorRegionUpdated(LevelObjects.onRegionUpdated);
    }

    public void Start()
    {
      Player.onPlayerCreated += new PlayerCreated(LevelObjects.onPlayerCreated);
      Editor.onEditorCreated += new EditorCreated(LevelObjects.onEditorCreated);
    }
  }
}
