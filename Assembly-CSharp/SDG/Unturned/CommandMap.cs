// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.CommandMap
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;

namespace SDG.Unturned
{
  public class CommandMap : Command
  {
    public CommandMap(Local newLocalization)
    {
      this.localization = newLocalization;
      this._command = this.localization.format("MapCommandText");
      this._info = this.localization.format("MapInfoText");
      this._help = this.localization.format("MapHelpText");
    }

    protected override void execute(CSteamID executorID, string parameter)
    {
      if (!Dedicator.isDedicated)
        return;
      if (!Level.exists(parameter))
        CommandWindow.LogError((object) this.localization.format("NoMapErrorText", (object) parameter));
      else if (Provider.isServer)
      {
        CommandWindow.LogError((object) this.localization.format("RunningErrorText"));
      }
      else
      {
        Provider.map = parameter;
        CommandWindow.Log((object) this.localization.format("MapText", (object) parameter));
      }
    }
  }
}
