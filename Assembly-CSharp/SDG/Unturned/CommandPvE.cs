﻿// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.CommandPvE
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;

namespace SDG.Unturned
{
  public class CommandPvE : Command
  {
    public CommandPvE(Local newLocalization)
    {
      this.localization = newLocalization;
      this._command = this.localization.format("PvECommandText");
      this._info = this.localization.format("PvEInfoText");
      this._help = this.localization.format("PvEHelpText");
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
        Provider.isPvP = false;
        CommandWindow.Log((object) this.localization.format("PvEText"));
      }
    }
  }
}
