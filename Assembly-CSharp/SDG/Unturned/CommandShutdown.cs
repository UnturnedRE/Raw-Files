// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.CommandShutdown
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;

namespace SDG.Unturned
{
  public class CommandShutdown : Command
  {
    public CommandShutdown(Local newLocalization)
    {
      this.localization = newLocalization;
      this._command = this.localization.format("ShutdownCommandText");
      this._info = this.localization.format("ShutdownInfoText");
      this._help = this.localization.format("ShutdownHelpText");
    }

    protected override void execute(CSteamID executorID, string parameter)
    {
      if (!Dedicator.isDedicated)
        return;
      Provider.shutdown();
    }
  }
}
