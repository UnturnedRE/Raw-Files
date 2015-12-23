// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.CommandFilter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;

namespace SDG.Unturned
{
  public class CommandFilter : Command
  {
    public CommandFilter(Local newLocalization)
    {
      this.localization = newLocalization;
      this._command = this.localization.format("FilterCommandText");
      this._info = this.localization.format("FilterInfoText");
      this._help = this.localization.format("FilterHelpText");
    }

    protected override void execute(CSteamID executorID, string parameter)
    {
      if (!Dedicator.isDedicated)
        return;
      if (Provider.isServer)
      {
        CommandWindow.LogError((object) this.localization.format("RunningErrorText"));
      }
      else
      {
        Provider.filterName = true;
        CommandWindow.Log((object) this.localization.format("FilterText"));
      }
    }
  }
}
