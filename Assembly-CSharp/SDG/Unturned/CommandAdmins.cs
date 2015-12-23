// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.CommandAdmins
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;

namespace SDG.Unturned
{
  public class CommandAdmins : Command
  {
    public CommandAdmins(Local newLocalization)
    {
      this.localization = newLocalization;
      this._command = this.localization.format("AdminsCommandText");
      this._info = this.localization.format("AdminsInfoText");
      this._help = this.localization.format("AdminsHelpText");
    }

    protected override void execute(CSteamID executorID, string parameter)
    {
      if (!Dedicator.isDedicated)
        return;
      if (SteamAdminlist.list.Count == 0)
      {
        CommandWindow.LogError((object) this.localization.format("NoAdminsErrorText"));
      }
      else
      {
        CommandWindow.Log((object) this.localization.format("AdminsText"));
        for (int index = 0; index < SteamAdminlist.list.Count; ++index)
        {
          SteamAdminID steamAdminId = SteamAdminlist.list[index];
          CommandWindow.Log((object) this.localization.format("AdminNameText", (object) steamAdminId.playerID));
          CommandWindow.Log((object) this.localization.format("AdminJudgeText", (object) steamAdminId.judgeID));
        }
      }
    }
  }
}
