// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.CommandBind
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;

namespace SDG.Unturned
{
  public class CommandBind : Command
  {
    public CommandBind(Local newLocalization)
    {
      this.localization = newLocalization;
      this._command = this.localization.format("BindCommandText");
      this._info = this.localization.format("BindInfoText");
      this._help = this.localization.format("BindHelpText");
    }

    protected override void execute(CSteamID executorID, string parameter)
    {
      if (!Dedicator.isDedicated)
        return;
      if (!Parser.checkIP(parameter))
        CommandWindow.LogError((object) this.localization.format("InvalidIPErrorText", (object) parameter));
      else if (Provider.isServer)
      {
        CommandWindow.LogError((object) this.localization.format("RunningErrorText"));
      }
      else
      {
        Provider.ip = Parser.getUInt32FromIP(parameter);
        CommandWindow.Log((object) this.localization.format("BindText", (object) parameter));
      }
    }
  }
}
