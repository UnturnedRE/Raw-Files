// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.CommandTime
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;

namespace SDG.Unturned
{
  public class CommandTime : Command
  {
    private static readonly uint MAX_NUMBER = 86400U;
    private static readonly uint MIN_NUMBER;

    public CommandTime(Local newLocalization)
    {
      this.localization = newLocalization;
      this._command = this.localization.format("TimeCommandText");
      this._info = this.localization.format("TimeInfoText");
      this._help = this.localization.format("TimeHelpText");
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
        uint result;
        if (!uint.TryParse(parameter, out result))
          CommandWindow.LogError((object) this.localization.format("InvalidNumberErrorText", (object) parameter));
        else if (result < CommandTime.MIN_NUMBER)
          CommandWindow.LogError((object) this.localization.format("MinNumberErrorText", (object) CommandTime.MIN_NUMBER));
        else if (result > CommandTime.MAX_NUMBER)
        {
          CommandWindow.LogError((object) this.localization.format("MaxNumberErrorText", (object) CommandTime.MAX_NUMBER));
        }
        else
        {
          LightingManager.time = result;
          CommandWindow.Log((object) this.localization.format("TimeText", (object) result));
        }
      }
    }
  }
}
