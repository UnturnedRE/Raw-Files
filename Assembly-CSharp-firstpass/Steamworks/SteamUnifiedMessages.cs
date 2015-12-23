// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamUnifiedMessages
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

namespace Steamworks
{
  public static class SteamUnifiedMessages
  {
    public static ClientUnifiedMessageHandle SendMethod(string pchServiceMethod, byte[] pRequestBuffer, uint unRequestBufferSize, ulong unContext)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchServiceMethod1 = new InteropHelp.UTF8StringHandle(pchServiceMethod))
        return (ClientUnifiedMessageHandle) NativeMethods.ISteamUnifiedMessages_SendMethod(pchServiceMethod1, pRequestBuffer, unRequestBufferSize, unContext);
    }

    public static bool GetMethodResponseInfo(ClientUnifiedMessageHandle hHandle, out uint punResponseSize, out EResult peResult)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamUnifiedMessages_GetMethodResponseInfo(hHandle, out punResponseSize, out peResult);
    }

    public static bool GetMethodResponseData(ClientUnifiedMessageHandle hHandle, byte[] pResponseBuffer, uint unResponseBufferSize, bool bAutoRelease)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamUnifiedMessages_GetMethodResponseData(hHandle, pResponseBuffer, unResponseBufferSize, bAutoRelease);
    }

    public static bool ReleaseMethod(ClientUnifiedMessageHandle hHandle)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamUnifiedMessages_ReleaseMethod(hHandle);
    }

    public static bool SendNotification(string pchServiceNotification, byte[] pNotificationBuffer, uint unNotificationBufferSize)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchServiceNotification1 = new InteropHelp.UTF8StringHandle(pchServiceNotification))
        return NativeMethods.ISteamUnifiedMessages_SendNotification(pchServiceNotification1, pNotificationBuffer, unNotificationBufferSize);
    }
  }
}
