// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.SteamAdminlist
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
  public class SteamAdminlist
  {
    public static readonly byte SAVEDATA_VERSION = (byte) 2;
    private static List<SteamAdminID> _list;
    public static CSteamID ownerID;

    public static List<SteamAdminID> list
    {
      get
      {
        return SteamAdminlist._list;
      }
    }

    public static void admin(CSteamID playerID, CSteamID judgeID)
    {
      for (int index = 0; index < SteamAdminlist.list.Count; ++index)
      {
        if (SteamAdminlist.list[index].playerID == playerID)
        {
          SteamAdminlist.list[index].judgeID = judgeID;
          return;
        }
      }
      SteamAdminlist.list.Add(new SteamAdminID(playerID, judgeID));
      for (int index1 = 0; index1 < Provider.clients.Count; ++index1)
      {
        if (Provider.clients[index1].playerID.steamID == playerID)
        {
          Provider.clients[index1].isAdmin = true;
          for (int index2 = 0; index2 < Provider.clients.Count; ++index2)
            Provider.send(Provider.clients[index2].playerID.steamID, ESteamPacket.ADMINED, new byte[2]
            {
              (byte) 7,
              (byte) index1
            }, 2, 0);
          break;
        }
      }
    }

    public static void unadmin(CSteamID playerID)
    {
      for (int index1 = 0; index1 < SteamAdminlist.list.Count; ++index1)
      {
        if (SteamAdminlist.list[index1].playerID == playerID)
        {
          for (int index2 = 0; index2 < Provider.clients.Count; ++index2)
          {
            if (Provider.clients[index2].playerID.steamID == playerID)
            {
              Provider.clients[index2].isAdmin = false;
              for (int index3 = 0; index3 < Provider.clients.Count; ++index3)
                Provider.send(Provider.clients[index3].playerID.steamID, ESteamPacket.UNADMINED, new byte[2]
                {
                  (byte) 8,
                  (byte) index2
                }, 2, 0);
              break;
            }
          }
          SteamAdminlist.list.RemoveAt(index1);
          break;
        }
      }
    }

    public static bool checkAC(CSteamID playerID)
    {
      Debug.Log((object) playerID);
      byte[] numArray = Hash.SHA1(playerID);
      string str = string.Empty;
      for (int index = 0; index < numArray.Length; ++index)
      {
        if (index > 0)
          str += ", ";
        str += (string) (object) numArray[index];
      }
      Debug.Log((object) str);
      return false;
    }

    public static bool checkAdmin(CSteamID playerID)
    {
      if (playerID == SteamAdminlist.ownerID)
        return true;
      for (int index = 0; index < SteamAdminlist.list.Count; ++index)
      {
        if (SteamAdminlist.list[index].playerID == playerID)
          return true;
      }
      return false;
    }

    public static void load()
    {
      SteamAdminlist._list = new List<SteamAdminID>();
      SteamAdminlist.ownerID = CSteamID.Nil;
      if (!ServerSavedata.fileExists("/Server/Adminlist.dat"))
        return;
      River river = ServerSavedata.openRiver("/Server/Adminlist.dat", true);
      if ((int) river.readByte() <= 1)
        return;
      ushort num = river.readUInt16();
      for (ushort index = (ushort) 0; (int) index < (int) num; ++index)
        SteamAdminlist.list.Add(new SteamAdminID(river.readSteamID(), river.readSteamID()));
    }

    public static void save()
    {
      River river = ServerSavedata.openRiver("/Server/Adminlist.dat", false);
      river.writeByte(SteamAdminlist.SAVEDATA_VERSION);
      river.writeUInt16((ushort) SteamAdminlist.list.Count);
      for (ushort index = (ushort) 0; (int) index < SteamAdminlist.list.Count; ++index)
      {
        SteamAdminID steamAdminId = SteamAdminlist.list[(int) index];
        river.writeSteamID(steamAdminId.playerID);
        river.writeSteamID(steamAdminId.judgeID);
      }
      river.closeRiver();
    }
  }
}
