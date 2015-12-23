// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.CommandHelp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;

namespace SDG.Unturned
{
  public class CommandHelp : Command
  {
    public CommandHelp(Local newLocalization)
    {
      this.localization = newLocalization;
      this._command = this.localization.format("HelpCommandText");
      this._info = this.localization.format("HelpInfoText");
      this._help = this.localization.format("HelpHelpText");
    }

    protected override void execute(CSteamID executorID, string parameter)
    {
      if (!Dedicator.isDedicated)
        return;
      if (parameter == string.Empty)
      {
        CommandWindow.Log((object) this.localization.format("HelpText"));
        string str = string.Empty;
        for (int index = 0; index < Commander.commands.Count; ++index)
        {
          str += Commander.commands[index].info;
          if (index < Commander.commands.Count - 1)
            str += "\n";
        }
        CommandWindow.Log((object) str);
      }
      else
      {
        for (int index = 0; index < Commander.commands.Count; ++index)
        {
          if (parameter.ToLower() == Commander.commands[index].command.ToLower())
          {
            CommandWindow.Log((object) Commander.commands[index].help);
            return;
          }
        }
        CommandWindow.Log((object) this.localization.format("NoCommandErrorText", (object) parameter));
      }
    }
  }
}
