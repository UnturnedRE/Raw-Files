// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.SteamWhitelist
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using System.Collections.Generic;

namespace SDG.Unturned
{
  public class SteamWhitelist
  {
    public static readonly byte SAVEDATA_VERSION = (byte) 2;
    private static List<SteamWhitelistID> _list;

    public static List<SteamWhitelistID> list
    {
      get
      {
        return SteamWhitelist._list;
      }
    }

    public static void whitelist(CSteamID steamID, string tag, CSteamID judgeID)
    {
      for (int index = 0; index < SteamWhitelist.list.Count; ++index)
      {
        if (SteamWhitelist.list[index].steamID == steamID)
        {
          SteamWhitelist.list[index].tag = tag;
          SteamWhitelist.list[index].judgeID = judgeID;
          return;
        }
      }
      SteamWhitelist.list.Add(new SteamWhitelistID(steamID, tag, judgeID));
    }

    public static bool unwhitelist(CSteamID steamID)
    {
      for (int index = 0; index < SteamWhitelist.list.Count; ++index)
      {
        if (SteamWhitelist.list[index].steamID == steamID)
        {
          Provider.kick(steamID, "Removed from whitelist.");
          SteamWhitelist.list.RemoveAt(index);
          return true;
        }
      }
      return false;
    }

    public static bool checkWhitelisted(CSteamID steamID)
    {
      if (SteamWhitelist.list.Count == 0)
        return true;
      for (int index = 0; index < SteamWhitelist.list.Count; ++index)
      {
        if (SteamWhitelist.list[index].steamID == steamID)
          return true;
      }
      return false;
    }

    public static void load()
    {
      SteamWhitelist._list = new List<SteamWhitelistID>();
      if (!ServerSavedata.fileExists("/Server/Whitelist.dat"))
        return;
      River river = ServerSavedata.openRiver("/Server/Whitelist.dat", true);
      if ((int) river.readByte() <= 1)
        return;
      ushort num = river.readUInt16();
      for (ushort index = (ushort) 0; (int) index < (int) num; ++index)
        SteamWhitelist.list.Add(new SteamWhitelistID(river.readSteamID(), river.readString(), river.readSteamID()));
    }

    public static void save()
    {
      River river = ServerSavedata.openRiver("/Server/Whitelist.dat", false);
      river.writeByte(SteamWhitelist.SAVEDATA_VERSION);
      river.writeUInt16((ushort) SteamWhitelist.list.Count);
      for (ushort index = (ushort) 0; (int) index < SteamWhitelist.list.Count; ++index)
      {
        SteamWhitelistID steamWhitelistId = SteamWhitelist.list[(int) index];
        river.writeSteamID(steamWhitelistId.steamID);
        river.writeString(steamWhitelistId.tag);
        river.writeSteamID(steamWhitelistId.judgeID);
      }
      river.closeRiver();
    }
  }
}
