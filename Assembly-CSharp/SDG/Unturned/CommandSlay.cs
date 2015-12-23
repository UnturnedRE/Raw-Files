// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.CommandSlay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
  public class CommandSlay : Command
  {
    public CommandSlay(Local newLocalization)
    {
      this.localization = newLocalization;
      this._command = this.localization.format("SlayCommandText");
      this._info = this.localization.format("SlayInfoText");
      this._help = this.localization.format("SlayHelpText");
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
            if ((Object) player.player != (Object) null)
            {
              EPlayerKill kill;
              player.player.life.askDamage((byte) 200, Vector3.up * 200f, EDeathCause.KILL, ELimb.SKULL, executorID, out kill);
            }
            if (componentsFromSerial.Length == 1)
              SteamBlacklist.ban(player.playerID.steamID, executorID, this.localization.format("SlayTextReason"), SteamBlacklist.PERMANENT);
            else if (componentsFromSerial.Length == 2)
              SteamBlacklist.ban(player.playerID.steamID, executorID, componentsFromSerial[1], SteamBlacklist.PERMANENT);
            CommandWindow.Log((object) this.localization.format("SlayText", (object) player.playerID.playerName));
          }
        }
      }
    }
  }
}
