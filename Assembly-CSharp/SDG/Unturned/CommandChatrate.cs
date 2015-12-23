// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.CommandChatrate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;

namespace SDG.Unturned
{
  public class CommandChatrate : Command
  {
    private static readonly float MAX_NUMBER = 60f;
    private static readonly float MIN_NUMBER;

    public CommandChatrate(Local newLocalization)
    {
      this.localization = newLocalization;
      this._command = this.localization.format("ChatrateCommandText");
      this._info = this.localization.format("ChatrateInfoText");
      this._help = this.localization.format("ChatrateHelpText");
    }

    protected override void execute(CSteamID executorID, string parameter)
    {
      if (!Dedicator.isDedicated)
        return;
      float result;
      if (!float.TryParse(parameter, out result))
        CommandWindow.LogError((object) this.localization.format("InvalidNumberErrorText", (object) parameter));
      else if ((double) result < (double) CommandChatrate.MIN_NUMBER)
        CommandWindow.LogError((object) this.localization.format("MinNumberErrorText", (object) CommandChatrate.MIN_NUMBER));
      else if ((double) result > (double) CommandChatrate.MAX_NUMBER)
      {
        CommandWindow.LogError((object) this.localization.format("MaxNumberErrorText", (object) CommandChatrate.MAX_NUMBER));
      }
      else
      {
        ChatManager.chatrate = result;
        CommandWindow.Log((object) this.localization.format("ChatrateText", (object) result));
      }
    }
  }
}
