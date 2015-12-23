// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.CommandExperience
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;

namespace SDG.Unturned
{
  public class CommandExperience : Command
  {
    public CommandExperience(Local newLocalization)
    {
      this.localization = newLocalization;
      this._command = this.localization.format("ExperienceCommandText");
      this._info = this.localization.format("ExperienceInfoText");
      this._help = this.localization.format("ExperienceHelpText");
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
        if (componentsFromSerial.Length != 2)
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
            uint result;
            if (!uint.TryParse(componentsFromSerial[1], out result))
            {
              CommandWindow.LogError((object) this.localization.format("InvalidNumberErrorText", (object) componentsFromSerial[1]));
            }
            else
            {
              player.player.skills.askAward(result);
              CommandWindow.Log((object) this.localization.format("ExperienceText", (object) player.playerID.playerName, (object) result));
            }
          }
        }
      }
    }
  }
}
