// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.CommandGive
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;

namespace SDG.Unturned
{
  public class CommandGive : Command
  {
    public CommandGive(Local newLocalization)
    {
      this.localization = newLocalization;
      this._command = this.localization.format("GiveCommandText");
      this._info = this.localization.format("GiveInfoText");
      this._help = this.localization.format("GiveHelpText");
    }

    protected override void execute(CSteamID executorID, string parameter)
    {
      if (!Provider.isServer)
      {
        CommandWindow.LogError((object) this.localization.format("NotRunningErrorText"));
      }
      else
      {
        string[] componentsFromSerial = Parser.getComponentsFromSerial(parameter, '/');
        if (componentsFromSerial.Length != 2 && componentsFromSerial.Length != 3)
        {
          CommandWindow.LogError((object) this.localization.format("InvalidParameterErrorText"));
        }
        else
        {
          SteamPlayer player;
          if (!PlayerTool.tryGetSteamPlayer(componentsFromSerial[0], out player))
            CommandWindow.LogError((object) this.localization.format("NoPlayerErrorText", (object) componentsFromSerial[0]));
          else if (componentsFromSerial.Length == 2)
          {
            ushort result;
            if (!ushort.TryParse(componentsFromSerial[1], out result))
            {
              CommandWindow.LogError((object) this.localization.format("InvalidItemIDErrorText", (object) componentsFromSerial[1]));
            }
            else
            {
              byte amount = (byte) 1;
              if (!ItemTool.tryForceGiveItem(player.player, result, amount))
                CommandWindow.LogError((object) this.localization.format("NoItemIDErrorText", (object) result));
              else
                CommandWindow.Log((object) this.localization.format("GiveText", (object) player.playerID.playerName, (object) result, (object) amount));
            }
          }
          else
          {
            if (componentsFromSerial.Length != 3)
              return;
            ushort result1;
            if (!ushort.TryParse(componentsFromSerial[1], out result1))
            {
              CommandWindow.LogError((object) this.localization.format("InvalidItemIDErrorText", (object) componentsFromSerial[1]));
            }
            else
            {
              byte result2;
              if (!byte.TryParse(componentsFromSerial[2], out result2))
                CommandWindow.LogError((object) this.localization.format("InvalidNumberErrorText", (object) componentsFromSerial[2]));
              else if (!ItemTool.tryForceGiveItem(player.player, result1, result2))
                CommandWindow.LogError((object) this.localization.format("NoItemIDErrorText", (object) result1));
              else
                CommandWindow.Log((object) this.localization.format("GiveText", (object) player.playerID.playerName, (object) result1, (object) result2));
            }
          }
        }
      }
    }
  }
}
