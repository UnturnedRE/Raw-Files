// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.PlayerTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
  public class PlayerTool
  {
    public static SteamPlayer[] getSteamPlayers()
    {
      return Provider.clients.ToArray();
    }

    public static SteamPlayer getSteamPlayer(string name)
    {
      for (int index = 0; index < Provider.clients.Count; ++index)
      {
        if (NameTool.checkNames(name, Provider.clients[index].playerID.playerName) || NameTool.checkNames(name, Provider.clients[index].playerID.characterName))
          return Provider.clients[index];
      }
      return (SteamPlayer) null;
    }

    public static SteamPlayer getSteamPlayer(ulong steamID)
    {
      for (int index = 0; index < Provider.clients.Count; ++index)
      {
        if ((long) Provider.clients[index].playerID.steamID.m_SteamID == (long) steamID)
          return Provider.clients[index];
      }
      return (SteamPlayer) null;
    }

    public static SteamPlayer getSteamPlayer(CSteamID steamID)
    {
      for (int index = 0; index < Provider.clients.Count; ++index)
      {
        if (Provider.clients[index].playerID.steamID == steamID)
          return Provider.clients[index];
      }
      return (SteamPlayer) null;
    }

    public static Transform getPlayerModel(CSteamID steamID)
    {
      SteamPlayer steamPlayer = PlayerTool.getSteamPlayer(steamID);
      if (steamPlayer != null && (Object) steamPlayer.model != (Object) null)
        return steamPlayer.model;
      return (Transform) null;
    }

    public static Player getPlayer(CSteamID steamID)
    {
      SteamPlayer steamPlayer = PlayerTool.getSteamPlayer(steamID);
      if (steamPlayer != null && (Object) steamPlayer.player != (Object) null)
        return steamPlayer.player;
      return (Player) null;
    }

    public static Transform getPlayerModel(string name)
    {
      SteamPlayer steamPlayer = PlayerTool.getSteamPlayer(name);
      if (steamPlayer != null && (Object) steamPlayer.model != (Object) null)
        return steamPlayer.model;
      return (Transform) null;
    }

    public static Player getPlayer(string name)
    {
      SteamPlayer steamPlayer = PlayerTool.getSteamPlayer(name);
      if (steamPlayer != null && (Object) steamPlayer.player != (Object) null)
        return steamPlayer.player;
      return (Player) null;
    }

    public static bool tryGetSteamPlayer(string input, out SteamPlayer player)
    {
      player = (SteamPlayer) null;
      ulong result;
      if (ulong.TryParse(input, out result))
      {
        player = PlayerTool.getSteamPlayer(result);
        return player != null;
      }
      player = PlayerTool.getSteamPlayer(input);
      return player != null;
    }

    public static bool tryGetSteamID(string input, out CSteamID steamID)
    {
      steamID = CSteamID.Nil;
      ulong result;
      if (ulong.TryParse(input, out result))
      {
        steamID = new CSteamID(result);
        return true;
      }
      SteamPlayer steamPlayer = PlayerTool.getSteamPlayer(input);
      if (steamPlayer == null)
        return false;
      steamID = steamPlayer.playerID.steamID;
      return true;
    }
  }
}
