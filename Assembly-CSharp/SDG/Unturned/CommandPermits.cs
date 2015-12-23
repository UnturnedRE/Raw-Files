// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.CommandPermits
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;

namespace SDG.Unturned
{
  public class CommandPermits : Command
  {
    public CommandPermits(Local newLocalization)
    {
      this.localization = newLocalization;
      this._command = this.localization.format("PermitsCommandText");
      this._info = this.localization.format("PermitsInfoText");
      this._help = this.localization.format("PermitsHelpText");
    }

    protected override void execute(CSteamID executorID, string parameter)
    {
      if (!Dedicator.isDedicated)
        return;
      if (SteamWhitelist.list.Count == 0)
      {
        CommandWindow.LogError((object) this.localization.format("NoPermitsErrorText"));
      }
      else
      {
        CommandWindow.Log((object) this.localization.format("PermitsText"));
        for (int index = 0; index < SteamWhitelist.list.Count; ++index)
        {
          SteamWhitelistID steamWhitelistId = SteamWhitelist.list[index];
          CommandWindow.Log((object) this.localization.format("PermitNameText", (object) steamWhitelistId.steamID, (object) steamWhitelistId.tag));
          CommandWindow.Log((object) this.localization.format("PermitJudgeText", (object) steamWhitelistId.judgeID));
        }
      }
    }
  }
}
