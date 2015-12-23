// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.CommandName
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;

namespace SDG.Unturned
{
  public class CommandName : Command
  {
    private static readonly byte MIN_LENGTH = (byte) 5;
    private static readonly byte MAX_LENGTH = (byte) 50;

    public CommandName(Local newLocalization)
    {
      this.localization = newLocalization;
      this._command = this.localization.format("NameCommandText");
      this._info = this.localization.format("NameInfoText");
      this._help = this.localization.format("NameHelpText");
    }

    protected override void execute(CSteamID executorID, string parameter)
    {
      if (!Dedicator.isDedicated)
        return;
      if (parameter.Length < (int) CommandName.MIN_LENGTH)
        CommandWindow.LogError((object) this.localization.format("MinLengthErrorText", (object) CommandName.MIN_LENGTH));
      else if (parameter.Length > (int) CommandName.MAX_LENGTH)
      {
        CommandWindow.LogError((object) this.localization.format("MaxLengthErrorText", (object) CommandName.MAX_LENGTH));
      }
      else
      {
        Provider.serverName = parameter;
        CommandWindow.Log((object) this.localization.format("NameText", (object) parameter));
      }
    }
  }
}
