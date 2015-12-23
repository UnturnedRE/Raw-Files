// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.CommandBans
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;

namespace SDG.Unturned
{
  public class CommandBans : Command
  {
    public CommandBans(Local newLocalization)
    {
      this.localization = newLocalization;
      this._command = this.localization.format("BansCommandText");
      this._info = this.localization.format("BansInfoText");
      this._help = this.localization.format("BansHelpText");
    }

    protected override void execute(CSteamID executorID, string parameter)
    {
      if (!Dedicator.isDedicated)
        return;
      if (SteamBlacklist.list.Count == 0)
      {
        CommandWindow.LogError((object) this.localization.format("NoBansErrorText"));
      }
      else
      {
        CommandWindow.Log((object) this.localization.format("BansText"));
        for (int index = 0; index < SteamBlacklist.list.Count; ++index)
        {
          SteamBlacklistID steamBlacklistId = SteamBlacklist.list[index];
          CommandWindow.Log((object) this.localization.format("BanNameText", (object) steamBlacklistId.playerID));
          CommandWindow.Log((object) this.localization.format("BanJudgeText", (object) steamBlacklistId.judgeID));
          CommandWindow.Log((object) this.localization.format("BanStatusText", (object) steamBlacklistId.reason, (object) steamBlacklistId.duration, (object) steamBlacklistId.getTime()));
        }
      }
    }
  }
}
