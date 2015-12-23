// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamAPI
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

namespace Steamworks
{
  public static class SteamAPI
  {
    public static bool RestartAppIfNecessary(AppId_t unOwnAppID)
    {
      InteropHelp.TestIfPlatformSupported();
      return NativeMethods.SteamAPI_RestartAppIfNecessary(unOwnAppID);
    }

    public static bool InitSafe()
    {
      return SteamAPI.Init();
    }

    public static bool Init()
    {
      InteropHelp.TestIfPlatformSupported();
      return NativeMethods.SteamAPI_InitSafe();
    }

    public static void Shutdown()
    {
      InteropHelp.TestIfPlatformSupported();
      NativeMethods.SteamAPI_Shutdown();
    }

    public static void RunCallbacks()
    {
      InteropHelp.TestIfPlatformSupported();
      NativeMethods.SteamAPI_RunCallbacks();
    }

    public static bool IsSteamRunning()
    {
      InteropHelp.TestIfPlatformSupported();
      return NativeMethods.SteamAPI_IsSteamRunning();
    }

    public static HSteamUser GetHSteamUserCurrent()
    {
      InteropHelp.TestIfPlatformSupported();
      return (HSteamUser) NativeMethods.Steam_GetHSteamUserCurrent();
    }

    public static HSteamPipe GetHSteamPipe()
    {
      InteropHelp.TestIfPlatformSupported();
      return (HSteamPipe) NativeMethods.SteamAPI_GetHSteamPipe();
    }

    public static HSteamUser GetHSteamUser()
    {
      InteropHelp.TestIfPlatformSupported();
      return (HSteamUser) NativeMethods.SteamAPI_GetHSteamUser();
    }
  }
}
