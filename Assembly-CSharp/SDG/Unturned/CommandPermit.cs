// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.CommandPermit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;

namespace SDG.Unturned
{
  public class CommandPermit : Command
  {
    public CommandPermit(Local newLocalization)
    {
      this.localization = newLocalization;
      this._command = this.localization.format("PermitCommandText");
      this._info = this.localization.format("PermitInfoText");
      this._help = this.localization.format("PermitHelpText");
    }

    protected override void execute(CSteamID executorID, string parameter)
    {
      if (!Dedicator.isDedicated)
        return;
      if (!Provider.isServer)
      {
        CommandWindow.LogError((object) this.localization.format("NotRunningErrorText"));
      }
      else
      {
        string[] componentsFromSerial = Parser.getComponentsFromSerial(parameter, '/');
        if (componentsFromSerial.Length != 2)
        {
          CommandWindow.LogError((object) this.localization.format("InvalidParameterErrorText"));
        }
        else
        {
          CSteamID steamID;
          if (!PlayerTool.tryGetSteamID(componentsFromSerial[0], out steamID))
          {
            CommandWindow.LogError((object) this.localization.format("InvalidSteamIDErrorText", (object) componentsFromSerial[0]));
          }
          else
          {
            SteamWhitelist.whitelist(steamID, componentsFromSerial[1], executorID);
            CommandWindow.Log((object) this.localization.format("PermitText", (object) steamID, (object) componentsFromSerial[1]));
          }
        }
      }
    }
  }
}
