// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamController
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

namespace Steamworks
{
  public static class SteamController
  {
    public static bool Init(string pchAbsolutePathToControllerConfigVDF)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchAbsolutePathToControllerConfigVDF1 = new InteropHelp.UTF8StringHandle(pchAbsolutePathToControllerConfigVDF))
        return NativeMethods.ISteamController_Init(pchAbsolutePathToControllerConfigVDF1);
    }

    public static bool Shutdown()
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamController_Shutdown();
    }

    public static void RunFrame()
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamController_RunFrame();
    }

    public static bool GetControllerState(uint unControllerIndex, out SteamControllerState_t pState)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamController_GetControllerState(unControllerIndex, out pState);
    }

    public static void TriggerHapticPulse(uint unControllerIndex, ESteamControllerPad eTargetPad, ushort usDurationMicroSec)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamController_TriggerHapticPulse(unControllerIndex, eTargetPad, usDurationMicroSec);
    }

    public static void SetOverrideMode(string pchMode)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchMode1 = new InteropHelp.UTF8StringHandle(pchMode))
        NativeMethods.ISteamController_SetOverrideMode(pchMode1);
    }
  }
}
