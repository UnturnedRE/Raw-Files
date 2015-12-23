// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.CommandCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;

namespace SDG.Unturned
{
  public class CommandCamera : Command
  {
    public CommandCamera(Local newLocalization)
    {
      this.localization = newLocalization;
      this._command = this.localization.format("CameraCommandText");
      this._info = this.localization.format("CameraInfoText");
      this._help = this.localization.format("CameraHelpText");
    }

    protected override void execute(CSteamID executorID, string parameter)
    {
      if (!Dedicator.isDedicated)
        return;
      string str = parameter.ToLower();
      ECameraMode ecameraMode;
      if (str == this.localization.format("CameraFirst").ToLower())
        ecameraMode = ECameraMode.FIRST;
      else if (str == this.localization.format("CameraThird").ToLower())
        ecameraMode = ECameraMode.THIRD;
      else if (str == this.localization.format("CameraBoth").ToLower())
      {
        ecameraMode = ECameraMode.BOTH;
      }
      else
      {
        CommandWindow.LogError((object) this.localization.format("NoCameraErrorText", (object) str));
        return;
      }
      if (Provider.isServer)
      {
        CommandWindow.LogError((object) this.localization.format("RunningErrorText"));
      }
      else
      {
        Provider.camera = ecameraMode;
        CommandWindow.Log((object) this.localization.format("CameraText", (object) str));
      }
    }
  }
}
