// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.CommandCycle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;

namespace SDG.Unturned
{
  public class CommandCycle : Command
  {
    private static readonly uint MAX_NUMBER = 86400U;
    private static readonly uint MIN_NUMBER;

    public CommandCycle(Local newLocalization)
    {
      this.localization = newLocalization;
      this._command = this.localization.format("CycleCommandText");
      this._info = this.localization.format("CycleInfoText");
      this._help = this.localization.format("CycleHelpText");
    }

    protected override void execute(CSteamID executorID, string parameter)
    {
      uint result;
      if (!uint.TryParse(parameter, out result))
        CommandWindow.LogError((object) this.localization.format("InvalidNumberErrorText", (object) parameter));
      else if (Provider.isServer && Level.info.type == ELevelType.HORDE)
        CommandWindow.LogError((object) this.localization.format("HordeErrorText"));
      else if (result < CommandCycle.MIN_NUMBER)
        CommandWindow.LogError((object) this.localization.format("MinNumberErrorText", (object) CommandCycle.MIN_NUMBER));
      else if (result > CommandCycle.MAX_NUMBER)
      {
        CommandWindow.LogError((object) this.localization.format("MaxNumberErrorText", (object) CommandCycle.MAX_NUMBER));
      }
      else
      {
        LightingManager.cycle = result;
        CommandWindow.Log((object) this.localization.format("CycleText", (object) result));
      }
    }
  }
}
