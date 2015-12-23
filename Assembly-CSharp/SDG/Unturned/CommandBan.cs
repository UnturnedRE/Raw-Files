// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.CommandBan
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;

namespace SDG.Unturned
{
  public class CommandBan : Command
  {
    public CommandBan(Local newLocalization)
    {
      this.localization = newLocalization;
      this._command = this.localization.format("BanCommandText");
      this._info = this.localization.format("BanInfoText");
      this._help = this.localization.format("BanHelpText");
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
        if (componentsFromSerial.Length != 1 && componentsFromSerial.Length != 2 && componentsFromSerial.Length != 3)
        {
          CommandWindow.LogError((object) this.localization.format("InvalidParameterErrorText"));
        }
        else
        {
          CSteamID steamID;
          if (!PlayerTool.tryGetSteamID(componentsFromSerial[0], out steamID))
            CommandWindow.LogError((object) this.localization.format("NoPlayerErrorText", (object) componentsFromSerial[0]));
          else if (componentsFromSerial.Length == 1)
          {
            SteamBlacklist.ban(steamID, executorID, this.localization.format("BanTextReason"), SteamBlacklist.PERMANENT);
            CommandWindow.Log((object) this.localization.format("BanTextPermanent", (object) steamID));
          }
          else if (componentsFromSerial.Length == 2)
          {
            SteamBlacklist.ban(steamID, executorID, componentsFromSerial[1], SteamBlacklist.PERMANENT);
            CommandWindow.Log((object) this.localization.format("BanTextPermanent", (object) steamID));
          }
          else
          {
            uint result;
            if (!uint.TryParse(componentsFromSerial[2], out result))
            {
              CommandWindow.LogError((object) this.localization.format("InvalidNumberErrorText", (object) componentsFromSerial[2]));
            }
            else
            {
              SteamBlacklist.ban(steamID, executorID, componentsFromSerial[1], result);
              CommandWindow.Log((object) this.localization.format("BanText", (object) steamID, (object) result));
            }
          }
        }
      }
    }
  }
}
