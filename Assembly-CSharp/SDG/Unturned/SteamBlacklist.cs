// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.SteamBlacklist
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using System.Collections.Generic;

namespace SDG.Unturned
{
  public class SteamBlacklist
  {
    public static readonly byte SAVEDATA_VERSION = (byte) 2;
    public static readonly uint PERMANENT = 31536000U;
    private static List<SteamBlacklistID> _list;

    public static List<SteamBlacklistID> list
    {
      get
      {
        return SteamBlacklist._list;
      }
    }

    public static void ban(CSteamID playerID, CSteamID judgeID, string reason, uint duration)
    {
      Provider.ban(playerID, reason, duration);
      for (int index = 0; index < SteamBlacklist.list.Count; ++index)
      {
        if (SteamBlacklist.list[index].playerID == playerID)
        {
          SteamBlacklist.list[index].judgeID = judgeID;
          SteamBlacklist.list[index].reason = reason;
          SteamBlacklist.list[index].duration = duration;
          SteamBlacklist.list[index].banned = Provider.time;
          return;
        }
      }
      SteamBlacklist.list.Add(new SteamBlacklistID(playerID, judgeID, reason, duration, Provider.time));
    }

    public static bool unban(CSteamID playerID)
    {
      for (int index = 0; index < SteamBlacklist.list.Count; ++index)
      {
        if (SteamBlacklist.list[index].playerID == playerID)
        {
          SteamBlacklist.list.RemoveAt(index);
          return true;
        }
      }
      return false;
    }

    public static bool checkBanned(CSteamID playerID, out SteamBlacklistID blacklistID)
    {
      blacklistID = (SteamBlacklistID) null;
      if (SteamBlacklist.list.Count == 0)
        return false;
      for (int index = 0; index < SteamBlacklist.list.Count; ++index)
      {
        if (SteamBlacklist.list[index].playerID == playerID)
        {
          if (SteamBlacklist.list[index].isExpired)
          {
            SteamBlacklist.list.RemoveAt(index);
            return false;
          }
          blacklistID = SteamBlacklist.list[index];
          return true;
        }
      }
      return false;
    }

    public static void load()
    {
      SteamBlacklist._list = new List<SteamBlacklistID>();
      if (!ServerSavedata.fileExists("/Server/Blacklist.dat"))
        return;
      River river = ServerSavedata.openRiver("/Server/Blacklist.dat", true);
      if ((int) river.readByte() <= 1)
        return;
      ushort num = river.readUInt16();
      for (ushort index = (ushort) 0; (int) index < (int) num; ++index)
      {
        SteamBlacklistID steamBlacklistId = new SteamBlacklistID(river.readSteamID(), river.readSteamID(), river.readString(), river.readUInt32(), river.readUInt32());
        if (!steamBlacklistId.isExpired)
          SteamBlacklist.list.Add(steamBlacklistId);
      }
    }

    public static void save()
    {
      River river = ServerSavedata.openRiver("/Server/Blacklist.dat", false);
      river.writeByte(SteamBlacklist.SAVEDATA_VERSION);
      river.writeUInt16((ushort) SteamBlacklist.list.Count);
      for (ushort index = (ushort) 0; (int) index < SteamBlacklist.list.Count; ++index)
      {
        SteamBlacklistID steamBlacklistId = SteamBlacklist.list[(int) index];
        river.writeSteamID(steamBlacklistId.playerID);
        river.writeSteamID(steamBlacklistId.judgeID);
        river.writeString(steamBlacklistId.reason);
        river.writeUInt32(steamBlacklistId.duration);
        river.writeUInt32(steamBlacklistId.banned);
      }
      river.closeRiver();
    }
  }
}
