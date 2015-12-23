// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.CommandUnpermit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;

namespace SDG.Unturned
{
  public class CommandUnpermit : Command
  {
    public CommandUnpermit(Local newLocalization)
    {
      this.localization = newLocalization;
      this._command = this.localization.format("UnpermitCommandText");
      this._info = this.localization.format("UnpermitInfoText");
      this._help = this.localization.format("UnpermitHelpText");
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
          CommandWindow.LogError((object) this.localization.format("InvalidSteamIDErrorText", (object) parameter));
        else if (!SteamWhitelist.unwhitelist(steamID))
          CommandWindow.LogError((object) this.localization.format("NoPlayerErrorText", (object) steamID));
        else
          CommandWindow.Log((object) this.localization.format("UnpermitText", (object) steamID));
      }
    }
  }
}
