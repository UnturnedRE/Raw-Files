﻿// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamEncryptedAppTicket
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
  public static class SteamEncryptedAppTicket
  {
    public static bool BDecryptTicket(byte[] rgubTicketEncrypted, uint cubTicketEncrypted, byte[] rgubTicketDecrypted, ref uint pcubTicketDecrypted, byte[] rgubKey, int cubKey)
    {
      InteropHelp.TestIfPlatformSupported();
      return NativeMethods.BDecryptTicket(rgubTicketEncrypted, cubTicketEncrypted, rgubTicketDecrypted, ref pcubTicketDecrypted, rgubKey, cubKey);
    }

    public static bool BIsTicketForApp(byte[] rgubTicketDecrypted, uint cubTicketDecrypted, AppId_t nAppID)
    {
      InteropHelp.TestIfPlatformSupported();
      return NativeMethods.BIsTicketForApp(rgubTicketDecrypted, cubTicketDecrypted, nAppID);
    }

    public static uint GetTicketIssueTime(byte[] rgubTicketDecrypted, uint cubTicketDecrypted)
    {
      InteropHelp.TestIfPlatformSupported();
      return NativeMethods.GetTicketIssueTime(rgubTicketDecrypted, cubTicketDecrypted);
    }

    public static void GetTicketSteamID(byte[] rgubTicketDecrypted, uint cubTicketDecrypted, out CSteamID psteamID)
    {
      InteropHelp.TestIfPlatformSupported();
      NativeMethods.GetTicketSteamID(rgubTicketDecrypted, cubTicketDecrypted, out psteamID);
    }

    public static uint GetTicketAppID(byte[] rgubTicketDecrypted, uint cubTicketDecrypted)
    {
      InteropHelp.TestIfPlatformSupported();
      return NativeMethods.GetTicketAppID(rgubTicketDecrypted, cubTicketDecrypted);
    }

    public static bool BUserOwnsAppInTicket(byte[] rgubTicketDecrypted, uint cubTicketDecrypted, AppId_t nAppID)
    {
      InteropHelp.TestIfPlatformSupported();
      return NativeMethods.BUserOwnsAppInTicket(rgubTicketDecrypted, cubTicketDecrypted, nAppID);
    }

    public static bool BUserIsVacBanned(byte[] rgubTicketDecrypted, uint cubTicketDecrypted)
    {
      InteropHelp.TestIfPlatformSupported();
      return NativeMethods.BUserIsVacBanned(rgubTicketDecrypted, cubTicketDecrypted);
    }

    public static byte[] GetUserVariableData(byte[] rgubTicketDecrypted, uint cubTicketDecrypted, out uint pcubUserData)
    {
      InteropHelp.TestIfPlatformSupported();
      IntPtr userVariableData = NativeMethods.GetUserVariableData(rgubTicketDecrypted, cubTicketDecrypted, out pcubUserData);
      byte[] destination = new byte[(IntPtr) pcubUserData];
      Marshal.Copy(userVariableData, destination, 0, (int) pcubUserData);
      return destination;
    }
  }
}
