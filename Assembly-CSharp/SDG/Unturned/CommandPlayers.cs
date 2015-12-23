// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.CommandPlayers
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;

namespace SDG.Unturned
{
  public class CommandPlayers : Command
  {
    public CommandPlayers(Local newLocalization)
    {
      this.localization = newLocalization;
      this._command = this.localization.format("PlayersCommandText");
      this._info = this.localization.format("PlayersInfoText");
      this._help = this.localization.format("PlayersHelpText");
    }

    protected override void execute(CSteamID executorID, string parameter)
    {
      if (!Dedicator.isDedicated)
        return;
      if (Provider.clients.Count == 0)
      {
        CommandWindow.LogError((object) this.localization.format("NoPlayersErrorText"));
      }
      else
      {
        CommandWindow.Log((object) this.localization.format("PlayersText"));
        for (int index = 0; index < Provider.clients.Count; ++index)
        {
          SteamPlayer steamPlayer = Provider.clients[index];
          CommandWindow.Log((object) this.localization.format("PlayerIDText", (object) steamPlayer.playerID.steamID, (object) steamPlayer.playerID.playerName, (object) steamPlayer.playerID.characterName, (object) (int) ((double) steamPlayer.ping * 1000.0)));
        }
      }
    }
  }
}
