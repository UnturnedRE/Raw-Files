// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.CommandTimeout
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;

namespace SDG.Unturned
{
  public class CommandTimeout : Command
  {
    private static readonly ushort MIN_NUMBER = (ushort) 50;
    private static readonly ushort MAX_NUMBER = (ushort) 10000;

    public CommandTimeout(Local newLocalization)
    {
      this.localization = newLocalization;
      this._command = this.localization.format("TimeoutCommandText");
      this._info = this.localization.format("TimeoutInfoText");
      this._help = this.localization.format("TimeoutHelpText");
    }

    protected override void execute(CSteamID executorID, string parameter)
    {
      if (!Dedicator.isDedicated)
        return;
      ushort result;
      if (!ushort.TryParse(parameter, out result))
        CommandWindow.LogError((object) this.localization.format("InvalidNumberErrorText", (object) parameter));
      else if ((int) result < (int) CommandTimeout.MIN_NUMBER)
        CommandWindow.LogError((object) this.localization.format("MinNumberErrorText", (object) CommandTimeout.MIN_NUMBER));
      else if ((int) result > (int) CommandTimeout.MAX_NUMBER)
      {
        CommandWindow.LogError((object) this.localization.format("MaxNumberErrorText", (object) CommandTimeout.MAX_NUMBER));
      }
      else
      {
        Provider.timeout = (float) result / 1000f;
        CommandWindow.Log((object) this.localization.format("TimeoutText", (object) result));
      }
    }
  }
}
