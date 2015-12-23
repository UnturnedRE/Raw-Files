// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamGameServerStats
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

namespace Steamworks
{
  public static class SteamGameServerStats
  {
    public static SteamAPICall_t RequestUserStats(CSteamID steamIDUser)
    {
      InteropHelp.TestIfAvailableGameServer();
      return (SteamAPICall_t) NativeMethods.ISteamGameServerStats_RequestUserStats(steamIDUser);
    }

    public static bool GetUserStat(CSteamID steamIDUser, string pchName, out int pData)
    {
      InteropHelp.TestIfAvailableGameServer();
      using (InteropHelp.UTF8StringHandle pchName1 = new InteropHelp.UTF8StringHandle(pchName))
        return NativeMethods.ISteamGameServerStats_GetUserStat(steamIDUser, pchName1, out pData);
    }

    public static bool GetUserStat(CSteamID steamIDUser, string pchName, out float pData)
    {
      InteropHelp.TestIfAvailableGameServer();
      using (InteropHelp.UTF8StringHandle pchName1 = new InteropHelp.UTF8StringHandle(pchName))
        return NativeMethods.ISteamGameServerStats_GetUserStat_(steamIDUser, pchName1, out pData);
    }

    public static bool GetUserAchievement(CSteamID steamIDUser, string pchName, out bool pbAchieved)
    {
      InteropHelp.TestIfAvailableGameServer();
      using (InteropHelp.UTF8StringHandle pchName1 = new InteropHelp.UTF8StringHandle(pchName))
        return NativeMethods.ISteamGameServerStats_GetUserAchievement(steamIDUser, pchName1, out pbAchieved);
    }

    public static bool SetUserStat(CSteamID steamIDUser, string pchName, int nData)
    {
      InteropHelp.TestIfAvailableGameServer();
      using (InteropHelp.UTF8StringHandle pchName1 = new InteropHelp.UTF8StringHandle(pchName))
        return NativeMethods.ISteamGameServerStats_SetUserStat(steamIDUser, pchName1, nData);
    }

    public static bool SetUserStat(CSteamID steamIDUser, string pchName, float fData)
    {
      InteropHelp.TestIfAvailableGameServer();
      using (InteropHelp.UTF8StringHandle pchName1 = new InteropHelp.UTF8StringHandle(pchName))
        return NativeMethods.ISteamGameServerStats_SetUserStat_(steamIDUser, pchName1, fData);
    }

    public static bool UpdateUserAvgRateStat(CSteamID steamIDUser, string pchName, float flCountThisSession, double dSessionLength)
    {
      InteropHelp.TestIfAvailableGameServer();
      using (InteropHelp.UTF8StringHandle pchName1 = new InteropHelp.UTF8StringHandle(pchName))
        return NativeMethods.ISteamGameServerStats_UpdateUserAvgRateStat(steamIDUser, pchName1, flCountThisSession, dSessionLength);
    }

    public static bool SetUserAchievement(CSteamID steamIDUser, string pchName)
    {
      InteropHelp.TestIfAvailableGameServer();
      using (InteropHelp.UTF8StringHandle pchName1 = new InteropHelp.UTF8StringHandle(pchName))
        return NativeMethods.ISteamGameServerStats_SetUserAchievement(steamIDUser, pchName1);
    }

    public static bool ClearUserAchievement(CSteamID steamIDUser, string pchName)
    {
      InteropHelp.TestIfAvailableGameServer();
      using (InteropHelp.UTF8StringHandle pchName1 = new InteropHelp.UTF8StringHandle(pchName))
        return NativeMethods.ISteamGameServerStats_ClearUserAchievement(steamIDUser, pchName1);
    }

    public static SteamAPICall_t StoreUserStats(CSteamID steamIDUser)
    {
      InteropHelp.TestIfAvailableGameServer();
      return (SteamAPICall_t) NativeMethods.ISteamGameServerStats_StoreUserStats(steamIDUser);
    }
  }
}
