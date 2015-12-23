// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.CommandSave
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;

namespace SDG.Unturned
{
  public class CommandSave : Command
  {
    public CommandSave(Local newLocalization)
    {
      this.localization = newLocalization;
      this._command = this.localization.format("SaveCommandText");
      this._info = this.localization.format("SaveInfoText");
      this._help = this.localization.format("SaveHelpText");
    }

    protected override void execute(CSteamID executorID, string parameter)
    {
      SaveManager.save();
      CommandWindow.Log((object) this.localization.format("SaveText"));
    }
  }
}
