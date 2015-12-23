// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.CommandPort
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;

namespace SDG.Unturned
{
  public class CommandPort : Command
  {
    public CommandPort(Local newLocalization)
    {
      this.localization = newLocalization;
      this._command = this.localization.format("PortCommandText");
      this._info = this.localization.format("PortInfoText");
      this._help = this.localization.format("PortHelpText");
    }

    protected override void execute(CSteamID executorID, string parameter)
    {
      if (!Dedicator.isDedicated)
        return;
      ushort result;
      if (!ushort.TryParse(parameter, out result))
        CommandWindow.LogError((object) this.localization.format("InvalidNumberErrorText", (object) parameter));
      else if (Provider.isServer)
      {
        CommandWindow.LogError((object) this.localization.format("RunningErrorText"));
      }
      else
      {
        Provider.port = result;
        CommandWindow.Log((object) this.localization.format("PortText", (object) result));
      }
    }
  }
}
