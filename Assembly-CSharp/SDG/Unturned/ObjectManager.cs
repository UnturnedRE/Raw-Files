// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ObjectManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
  public class ObjectManager : SteamCaller
  {
    public static readonly byte SAVEDATA_VERSION = (byte) 1;
    public static readonly byte OBJECT_REGIONS = (byte) 2;
    private static ObjectManager manager;
    private static ObjectRegion[,] regions;

    public static void useObjectDropper(Transform transform)
    {
      byte x;
      byte y;
      ushort index;
      if (!ObjectManager.tryGetRegion(transform, out x, out y, out index))
        return;
      ObjectManager.manager.channel.send("askUseObjectDropper", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[3]
      {
        (object) x,
        (object) y,
        (object) index
      });
    }

    [SteamCall]
    public void askUseObjectDropper(CSteamID steamID, byte x, byte y, ushort index)
    {
      if (!Provider.isServer || !Regions.checkSafe(x, y))
        return;
      Player player = PlayerTool.getPlayer(steamID);
      if ((Object) player == (Object) null || player.life.isDead || (int) index >= LevelObjects.objects[(int) x, (int) y].Count)
        return;
      InteractableObjectDropper component = LevelObjects.objects[(int) x, (int) y][(int) index].transform.GetComponent<InteractableObjectDropper>();
      if (!((Object) component != (Object) null) || !component.isUsable)
        return;
      component.drop();
    }

    public static void toggleObjectBinaryState(Transform transform)
    {
      byte x;
      byte y;
      ushort index;
      if (!ObjectManager.tryGetRegion(transform, out x, out y, out index))
        return;
      ObjectManager.manager.channel.send("askToggleObjectBinaryState", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[3]
      {
        (object) x,
        (object) y,
        (object) index
      });
    }

    [SteamCall]
    public void tellToggleObjectBinaryState(CSteamID steamID, byte x, byte y, ushort index, bool isUsed)
    {
      if (!this.channel.checkServer(steamID) || !Regions.checkSafe(x, y) || (!Provider.isServer && !ObjectManager.regions[(int) x, (int) y].isNetworked || (int) index >= LevelObjects.objects[(int) x, (int) y].Count))
        return;
      InteractableObjectBinaryState component = LevelObjects.objects[(int) x, (int) y][(int) index].transform.GetComponent<InteractableObjectBinaryState>();
      if (!((Object) component != (Object) null))
        return;
      component.updateToggle(isUsed);
    }

    [SteamCall]
    public void askToggleObjectBinaryState(CSteamID steamID, byte x, byte y, ushort index)
    {
      if (!Provider.isServer || !Regions.checkSafe(x, y))
        return;
      Player player = PlayerTool.getPlayer(steamID);
      if ((Object) player == (Object) null || player.life.isDead || (int) index >= LevelObjects.objects[(int) x, (int) y].Count)
        return;
      InteractableObjectBinaryState component = LevelObjects.objects[(int) x, (int) y][(int) index].transform.GetComponent<InteractableObjectBinaryState>();
      if (!((Object) component != (Object) null) || !component.isUsable)
        return;
      ObjectManager.manager.channel.send("tellToggleObjectBinaryState", ESteamCall.ALL, x, y, ObjectManager.OBJECT_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, (object) x, (object) y, (object) index, (object) (bool) (!component.isUsed ? 1 : 0));
      LevelObjects.objects[(int) x, (int) y][(int) index].state[0] = !component.isUsed ? (byte) 0 : (byte) 1;
    }

    [SteamCall]
    public void tellObjects(CSteamID steamID)
    {
      if (!this.channel.checkServer(steamID))
        return;
      byte x = (byte) this.channel.read(Types.BYTE_TYPE);
      byte y = (byte) this.channel.read(Types.BYTE_TYPE);
      if (!Regions.checkSafe(x, y) || ObjectManager.regions[(int) x, (int) y].isNetworked)
        return;
      ObjectManager.regions[(int) x, (int) y].isNetworked = true;
      while (true)
      {
        ushort num = (ushort) this.channel.read(Types.UINT16_TYPE);
        if ((int) num != (int) ushort.MaxValue)
        {
          byte[] state = (byte[]) this.channel.read(Types.BYTE_ARRAY_TYPE);
          LevelObjects.objects[(int) x, (int) y][(int) num].transform.GetComponent<Interactable>().updateState((Asset) LevelObjects.objects[(int) x, (int) y][(int) num].asset, state);
        }
        else
          break;
      }
    }

    public void askObjects(CSteamID steamID, byte x, byte y)
    {
      this.channel.openWrite();
      this.channel.write((object) x);
      this.channel.write((object) y);
      for (ushort index = (ushort) 0; (int) index < LevelObjects.objects[(int) x, (int) y].Count; ++index)
      {
        LevelObject levelObject = LevelObjects.objects[(int) x, (int) y][(int) index];
        if (levelObject.state != null && levelObject.state.Length > 0)
        {
          this.channel.write((object) index);
          this.channel.write((object) levelObject.state);
        }
      }
      this.channel.write((object) ushort.MaxValue);
      this.channel.closeWrite("tellObjects", steamID, ESteamPacket.UPDATE_RELIABLE_CHUNK_BUFFER);
    }

    public static Transform getObject(byte x, byte y, ushort index)
    {
      List<LevelObject> list = LevelObjects.objects[(int) x, (int) y];
      if ((int) index >= list.Count)
        return (Transform) null;
      return list[(int) index].transform;
    }

    public static bool tryGetRegion(Transform transform, out byte x, out byte y, out ushort index)
    {
      x = (byte) 0;
      y = (byte) 0;
      index = (ushort) 0;
      if (Regions.tryGetCoordinate(transform.position, out x, out y))
      {
        List<LevelObject> list = LevelObjects.objects[(int) x, (int) y];
        index = (ushort) 0;
        while ((int) index < list.Count)
        {
          if ((Object) transform == (Object) list[(int) index].transform)
            return true;
          index = (ushort) ((uint) index + 1U);
        }
      }
      return false;
    }

    private void onLevelLoaded(int level)
    {
      if (level <= Level.SETUP)
        return;
      ObjectManager.regions = new ObjectRegion[(int) Regions.WORLD_SIZE, (int) Regions.WORLD_SIZE];
      for (byte index1 = (byte) 0; (int) index1 < (int) Regions.WORLD_SIZE; ++index1)
      {
        for (byte index2 = (byte) 0; (int) index2 < (int) Regions.WORLD_SIZE; ++index2)
          ObjectManager.regions[(int) index1, (int) index2] = new ObjectRegion();
      }
      if (!Provider.isServer)
        return;
      ObjectManager.load();
    }

    private void onRegionUpdated(Player player, byte old_x, byte old_y, byte new_x, byte new_y)
    {
      for (byte x_0 = (byte) 0; (int) x_0 < (int) Regions.WORLD_SIZE; ++x_0)
      {
        for (byte y_0 = (byte) 0; (int) y_0 < (int) Regions.WORLD_SIZE; ++y_0)
        {
          if (Provider.isServer)
          {
            if (player.movement.loadedRegions[(int) x_0, (int) y_0].isObjectsLoaded && !Regions.checkArea(x_0, y_0, new_x, new_y, ObjectManager.OBJECT_REGIONS))
              player.movement.loadedRegions[(int) x_0, (int) y_0].isObjectsLoaded = false;
          }
          else if (player.channel.isOwner && ObjectManager.regions[(int) x_0, (int) y_0].isNetworked && !Regions.checkArea(x_0, y_0, new_x, new_y, ObjectManager.OBJECT_REGIONS))
            ObjectManager.regions[(int) x_0, (int) y_0].isNetworked = false;
        }
      }
      if (!Dedicator.isDedicated || !Regions.checkSafe(new_x, new_y))
        return;
      for (int index1 = (int) new_x - (int) ObjectManager.OBJECT_REGIONS; index1 <= (int) new_x + (int) ObjectManager.OBJECT_REGIONS; ++index1)
      {
        for (int index2 = (int) new_y - (int) ObjectManager.OBJECT_REGIONS; index2 <= (int) new_y + (int) ObjectManager.OBJECT_REGIONS; ++index2)
        {
          if (Regions.checkSafe((byte) index1, (byte) index2) && !player.movement.loadedRegions[index1, index2].isObjectsLoaded)
          {
            player.movement.loadedRegions[index1, index2].isObjectsLoaded = true;
            this.askObjects(player.channel.owner.playerID.steamID, (byte) index1, (byte) index2);
          }
        }
      }
    }

    private void onPlayerCreated(Player player)
    {
      player.movement.onRegionUpdated += new PlayerRegionUpdated(this.onRegionUpdated);
    }

    private void Start()
    {
      ObjectManager.manager = this;
      Level.onLevelLoaded += new LevelLoaded(this.onLevelLoaded);
      Player.onPlayerCreated += new PlayerCreated(this.onPlayerCreated);
    }

    public static void load()
    {
      if (!LevelSavedata.fileExists("/Objects.dat") || Level.info.type != ELevelType.SURVIVAL)
        return;
      River river = LevelSavedata.openRiver("/Objects.dat", true);
      river.readByte();
      for (byte index1 = (byte) 0; (int) index1 < (int) Regions.WORLD_SIZE; ++index1)
      {
        for (byte index2 = (byte) 0; (int) index2 < (int) Regions.WORLD_SIZE; ++index2)
          ObjectManager.loadRegion(river, LevelObjects.objects[(int) index1, (int) index2]);
      }
    }

    public static void save()
    {
      River river = LevelSavedata.openRiver("/Objects.dat", false);
      river.writeByte(ObjectManager.SAVEDATA_VERSION);
      for (byte index1 = (byte) 0; (int) index1 < (int) Regions.WORLD_SIZE; ++index1)
      {
        for (byte index2 = (byte) 0; (int) index2 < (int) Regions.WORLD_SIZE; ++index2)
          ObjectManager.saveRegion(river, LevelObjects.objects[(int) index1, (int) index2]);
      }
      river.closeRiver();
    }

    private static void loadRegion(River river, List<LevelObject> objects)
    {
      while (true)
      {
        byte[] state;
        LevelObject levelObject;
        Interactable component;
        do
        {
          do
          {
            ushort num1;
            do
            {
              ushort num2 = river.readUInt16();
              if ((int) num2 != (int) ushort.MaxValue)
              {
                num1 = river.readUInt16();
                state = river.readBytes();
                if ((int) num2 >= objects.Count)
                  return;
                levelObject = objects[(int) num2];
              }
              else
                goto label_2;
            }
            while ((int) num1 != (int) levelObject.id);
            levelObject.state = state;
          }
          while ((Object) levelObject.transform == (Object) null);
          component = levelObject.transform.gameObject.GetComponent<Interactable>();
        }
        while ((Object) component == (Object) null);
        component.updateState((Asset) levelObject.asset, state);
      }
label_2:;
    }

    private static void saveRegion(River river, List<LevelObject> objects)
    {
      for (ushort index = (ushort) 0; (int) index < objects.Count; ++index)
      {
        LevelObject levelObject = objects[(int) index];
        if (levelObject.state != null && levelObject.state.Length > 0)
        {
          river.writeUInt16(index);
          river.writeUInt16(levelObject.id);
          river.writeBytes(levelObject.state);
        }
      }
      river.writeUInt16(ushort.MaxValue);
    }
  }
}
