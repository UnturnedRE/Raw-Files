// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.CommandMaxPlayers
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;

namespace SDG.Unturned
{
  public class CommandMaxPlayers : Command
  {
    public static readonly byte MIN_NUMBER = (byte) 1;
    public static readonly byte MAX_NUMBER = (byte) 48;

    public CommandMaxPlayers(Local newLocalization)
    {
      this.localization = newLocalization;
      this._command = this.localization.format("MaxPlayersCommandText");
      this._info = this.localization.format("MaxPlayersInfoText");
      this._help = this.localization.format("MaxPlayersHelpText");
    }

    protected override void execute(CSteamID executorID, string parameter)
    {
      if (!Dedicator.isDedicated)
        return;
      byte result;
      if (!byte.TryParse(parameter, out result))
        CommandWindow.LogError((object) this.localization.format("InvalidNumberErrorText", (object) parameter));
      else if ((int) result < (int) CommandMaxPlayers.MIN_NUMBER)
        CommandWindow.LogError((object) this.localization.format("MinNumberErrorText", (object) CommandMaxPlayers.MIN_NUMBER));
      else if ((int) result > (int) CommandMaxPlayers.MAX_NUMBER)
      {
        CommandWindow.LogError((object) this.localization.format("MaxNumberErrorText", (object) CommandMaxPlayers.MAX_NUMBER));
      }
      else
      {
        if ((int) result > (int) CommandMaxPlayers.MAX_NUMBER / 2)
          CommandWindow.LogWarning((object) this.localization.format("RecommendedNumberErrorText", (object) ((int) CommandMaxPlayers.MAX_NUMBER / 2)));
        Provider.maxPlayers = result;
        CommandWindow.Log((object) this.localization.format("MaxPlayersText", (object) result));
      }
    }
  }
}
