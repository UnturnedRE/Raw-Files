﻿// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamScreenshots
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

namespace Steamworks
{
  public static class SteamScreenshots
  {
    public static ScreenshotHandle WriteScreenshot(byte[] pubRGB, uint cubRGB, int nWidth, int nHeight)
    {
      InteropHelp.TestIfAvailableClient();
      return (ScreenshotHandle) NativeMethods.ISteamScreenshots_WriteScreenshot(pubRGB, cubRGB, nWidth, nHeight);
    }

    public static ScreenshotHandle AddScreenshotToLibrary(string pchFilename, string pchThumbnailFilename, int nWidth, int nHeight)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchFilename1 = new InteropHelp.UTF8StringHandle(pchFilename))
      {
        using (InteropHelp.UTF8StringHandle pchThumbnailFilename1 = new InteropHelp.UTF8StringHandle(pchThumbnailFilename))
          return (ScreenshotHandle) NativeMethods.ISteamScreenshots_AddScreenshotToLibrary(pchFilename1, pchThumbnailFilename1, nWidth, nHeight);
      }
    }

    public static void TriggerScreenshot()
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamScreenshots_TriggerScreenshot();
    }

    public static void HookScreenshots(bool bHook)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamScreenshots_HookScreenshots(bHook);
    }

    public static bool SetLocation(ScreenshotHandle hScreenshot, string pchLocation)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchLocation1 = new InteropHelp.UTF8StringHandle(pchLocation))
        return NativeMethods.ISteamScreenshots_SetLocation(hScreenshot, pchLocation1);
    }

    public static bool TagUser(ScreenshotHandle hScreenshot, CSteamID steamID)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamScreenshots_TagUser(hScreenshot, steamID);
    }

    public static bool TagPublishedFile(ScreenshotHandle hScreenshot, PublishedFileId_t unPublishedFileID)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamScreenshots_TagPublishedFile(hScreenshot, unPublishedFileID);
    }
  }
}
