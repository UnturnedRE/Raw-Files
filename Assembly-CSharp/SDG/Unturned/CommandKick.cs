// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.CommandKick
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;

namespace SDG.Unturned
{
  public class CommandKick : Command
  {
    public CommandKick(Local newLocalization)
    {
      this.localization = newLocalization;
      this._command = this.localization.format("KickCommandText");
      this._info = this.localization.format("KickInfoText");
      this._help = this.localization.format("KickHelpText");
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
        if (componentsFromSerial.Length != 1 && componentsFromSerial.Length != 2)
        {
          CommandWindow.LogError((object) this.localization.format("InvalidParameterErrorText"));
        }
        else
        {
          SteamPlayer player;
          if (!PlayerTool.tryGetSteamPlayer(componentsFromSerial[0], out player))
          {
            CommandWindow.LogError((object) this.localization.format("NoPlayerErrorText", (object) componentsFromSerial[0]));
          }
          else
          {
            if (componentsFromSerial.Length == 1)
              Provider.kick(player.playerID.steamID, this.localization.format("KickTextReason"));
            else if (componentsFromSerial.Length == 2)
              Provider.kick(player.playerID.steamID, componentsFromSerial[1]);
            CommandWindow.Log((object) this.localization.format("KickText", (object) player.playerID.playerName));
          }
        }
      }
    }
  }
}
