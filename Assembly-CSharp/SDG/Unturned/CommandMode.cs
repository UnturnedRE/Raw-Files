// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.CommandMode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;

namespace SDG.Unturned
{
  public class CommandMode : Command
  {
    public CommandMode(Local newLocalization)
    {
      this.localization = newLocalization;
      this._command = this.localization.format("ModeCommandText");
      this._info = this.localization.format("ModeInfoText");
      this._help = this.localization.format("ModeHelpText");
    }

    protected override void execute(CSteamID executorID, string parameter)
    {
      if (!Dedicator.isDedicated)
        return;
      string str = parameter.ToLower();
      EGameMode egameMode;
      if (str == this.localization.format("ModeEasy").ToLower())
        egameMode = EGameMode.EASY;
      else if (str == this.localization.format("ModeNormal").ToLower())
        egameMode = EGameMode.NORMAL;
      else if (str == this.localization.format("ModeHard").ToLower())
        egameMode = EGameMode.HARD;
      else if (str == this.localization.format("ModePro").ToLower())
      {
        egameMode = EGameMode.PRO;
      }
      else
      {
        CommandWindow.LogError((object) this.localization.format("NoModeErrorText", (object) str));
        return;
      }
      if (Provider.isServer)
      {
        CommandWindow.LogError((object) this.localization.format("RunningErrorText"));
      }
      else
      {
        Provider.mode = egameMode;
        CommandWindow.Log((object) this.localization.format("ModeText", (object) str));
      }
    }
  }
}
