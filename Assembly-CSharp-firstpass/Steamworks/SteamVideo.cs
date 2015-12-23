// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamVideo
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

namespace Steamworks
{
  public static class SteamVideo
  {
    public static void GetVideoURL(AppId_t unVideoAppID)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamVideo_GetVideoURL(unVideoAppID);
    }

    public static bool IsBroadcasting(out int pnNumViewers)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamVideo_IsBroadcasting(out pnNumViewers);
    }
  }
}
