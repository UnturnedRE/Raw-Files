// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.CommandAdmin
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;

namespace SDG.Unturned
{
  public class CommandAdmin : Command
  {
    public CommandAdmin(Local newLocalization)
    {
      this.localization = newLocalization;
      this._command = this.localization.format("AdminCommandText");
      this._info = this.localization.format("AdminInfoText");
      this._help = this.localization.format("AdminHelpText");
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
        CSteamID steamID;
        if (!PlayerTool.tryGetSteamID(parameter, out steamID))
        {
          CommandWindow.LogError((object) this.localization.format("NoPlayerErrorText", (object) parameter));
        }
        else
        {
          SteamAdminlist.admin(steamID, executorID);
          CommandWindow.Log((object) this.localization.format("AdminText", (object) steamID));
        }
      }
    }
  }
}
