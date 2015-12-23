// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.CommandPassword
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;

namespace SDG.Unturned
{
  public class CommandPassword : Command
  {
    public CommandPassword(Local newLocalization)
    {
      this.localization = newLocalization;
      this._command = this.localization.format("PasswordCommandText");
      this._info = this.localization.format("PasswordInfoText");
      this._help = this.localization.format("PasswordHelpText");
    }

    protected override void execute(CSteamID executorID, string parameter)
    {
      if (!Dedicator.isDedicated)
        return;
      if (Provider.isServer)
        CommandWindow.LogError((object) this.localization.format("RunningErrorText"));
      else if (parameter.Length == 0)
      {
        Provider.serverPassword = string.Empty;
        CommandWindow.Log((object) this.localization.format("DisableText"));
      }
      else
      {
        Provider.serverPassword = parameter;
        CommandWindow.Log((object) this.localization.format("PasswordText", (object) parameter));
      }
    }
  }
}
