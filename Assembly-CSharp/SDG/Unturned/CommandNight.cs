// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.CommandNight
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;

namespace SDG.Unturned
{
  public class CommandNight : Command
  {
    public CommandNight(Local newLocalization)
    {
      this.localization = newLocalization;
      this._command = this.localization.format("NightCommandText");
      this._info = this.localization.format("NightInfoText");
      this._help = this.localization.format("NightHelpText");
    }

    protected override void execute(CSteamID executorID, string parameter)
    {
      if (!Provider.isServer)
        CommandWindow.LogError((object) this.localization.format("NotRunningErrorText"));
      else if (Provider.isServer && Level.info.type == ELevelType.HORDE)
      {
        CommandWindow.LogError((object) this.localization.format("HordeErrorText"));
      }
      else
      {
        LightingManager.time = (uint) ((double) LightingManager.cycle * ((double) LevelLighting.bias + (double) LevelLighting.transition));
        CommandWindow.Log((object) this.localization.format("NightText"));
      }
    }
  }
}
