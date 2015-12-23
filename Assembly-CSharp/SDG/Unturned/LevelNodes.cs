// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.LevelNodes
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
  public class LevelNodes
  {
    public static readonly byte SAVEDATA_VERSION = (byte) 1;
    private static Transform _models;
    private static List<Node> _nodes;

    public static Transform models
    {
      get
      {
        return LevelNodes._models;
      }
    }

    public static List<Node> nodes
    {
      get
      {
        return LevelNodes._nodes;
      }
    }

    public static void setEnabled(bool isEnabled)
    {
      if (LevelNodes.nodes == null)
        return;
      for (int index = 0; index < LevelNodes.nodes.Count; ++index)
        LevelNodes.nodes[index].setEnabled(isEnabled);
    }

    public static Transform addNode(Vector3 point, ENodeType type)
    {
      if (type == ENodeType.LOCATION)
        LevelNodes.nodes.Add((Node) new LocationNode(point));
      else if (type == ENodeType.SAFEZONE)
        LevelNodes.nodes.Add((Node) new SafezoneNode(point));
      else if (type == ENodeType.PURCHASE)
        LevelNodes.nodes.Add((Node) new PurchaseNode(point));
      return LevelNodes.nodes[LevelNodes.nodes.Count - 1].model;
    }

    public static void removeNode(Transform select)
    {
      for (int index = 0; index < LevelNodes.nodes.Count; ++index)
      {
        if ((Object) LevelNodes.nodes[index].model == (Object) select)
        {
          LevelNodes.nodes[index].remove();
          LevelNodes.nodes.RemoveAt(index);
          break;
        }
      }
    }

    public static Node getNode(Transform select)
    {
      for (int index = 0; index < LevelNodes.nodes.Count; ++index)
      {
        if ((Object) LevelNodes.nodes[index].model == (Object) select)
          return LevelNodes.nodes[index];
      }
      return (Node) null;
    }

    public static void load()
    {
      LevelNodes._models = new GameObject().transform;
      LevelNodes.models.name = "Nodes";
      LevelNodes.models.parent = Level.level;
      LevelNodes.models.tag = "Logic";
      LevelNodes.models.gameObject.layer = LayerMasks.LOGIC;
      LevelNodes._nodes = new List<Node>();
      if (!ReadWrite.fileExists(Level.info.path + "/Environment/Nodes.dat", false, false))
        return;
      River river = new River(Level.info.path + "/Environment/Nodes.dat", false);
      if ((int) river.readByte() > 0)
      {
        ushort num = (ushort) river.readByte();
        for (ushort index = (ushort) 0; (int) index < (int) num; ++index)
        {
          Vector3 newPoint = river.readSingleVector3();
          switch ((ENodeType) river.readByte())
          {
            case ENodeType.LOCATION:
              string newName = river.readString();
              LevelNodes.nodes.Add((Node) new LocationNode(newPoint, newName));
              break;
            case ENodeType.SAFEZONE:
              float newRadius1 = river.readSingle();
              LevelNodes.nodes.Add((Node) new SafezoneNode(newPoint, newRadius1));
              break;
            case ENodeType.PURCHASE:
              float newRadius2 = river.readSingle();
              ushort newID = river.readUInt16();
              uint newCost = river.readUInt32();
              LevelNodes.nodes.Add((Node) new PurchaseNode(newPoint, newRadius2, newID, newCost));
              break;
          }
        }
      }
      river.closeRiver();
    }

    public static void save()
    {
      River river = new River(Level.info.path + "/Environment/Nodes.dat", false);
      river.writeByte(LevelNodes.SAVEDATA_VERSION);
      byte num = (byte) 0;
      for (ushort index = (ushort) 0; (int) index < LevelNodes.nodes.Count; ++index)
      {
        if (LevelNodes.nodes[(int) index].type != ENodeType.LOCATION || ((LocationNode) LevelNodes.nodes[(int) index]).name.Length > 0)
          ++num;
      }
      river.writeByte(num);
      for (byte index = (byte) 0; (int) index < LevelNodes.nodes.Count; ++index)
      {
        if (LevelNodes.nodes[(int) index].type != ENodeType.LOCATION || ((LocationNode) LevelNodes.nodes[(int) index]).name.Length > 0)
        {
          river.writeSingleVector3(LevelNodes.nodes[(int) index].point);
          river.writeByte((byte) LevelNodes.nodes[(int) index].type);
          if (LevelNodes.nodes[(int) index].type == ENodeType.LOCATION)
            river.writeString(((LocationNode) LevelNodes.nodes[(int) index]).name);
          else if (LevelNodes.nodes[(int) index].type == ENodeType.SAFEZONE)
            river.writeSingle(((SafezoneNode) LevelNodes.nodes[(int) index]).radius);
          else if (LevelNodes.nodes[(int) index].type == ENodeType.PURCHASE)
          {
            river.writeSingle(((PurchaseNode) LevelNodes.nodes[(int) index]).radius);
            river.writeUInt16(((PurchaseNode) LevelNodes.nodes[(int) index]).id);
            river.writeUInt32(((PurchaseNode) LevelNodes.nodes[(int) index]).cost);
          }
        }
      }
      river.closeRiver();
    }
  }
}
