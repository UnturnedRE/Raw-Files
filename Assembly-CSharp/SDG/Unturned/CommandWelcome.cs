// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.CommandWelcome
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
  public class CommandWelcome : Command
  {
    public CommandWelcome(Local newLocalization)
    {
      this.localization = newLocalization;
      this._command = this.localization.format("WelcomeCommandText");
      this._info = this.localization.format("WelcomeInfoText");
      this._help = this.localization.format("WelcomeHelpText");
    }

    protected override void execute(CSteamID executorID, string parameter)
    {
      if (!Dedicator.isDedicated)
        return;
      string[] componentsFromSerial = Parser.getComponentsFromSerial(parameter, '/');
      if (componentsFromSerial.Length != 1 && componentsFromSerial.Length != 4)
      {
        CommandWindow.LogError((object) this.localization.format("InvalidParameterErrorText"));
      }
      else
      {
        ChatManager.welcomeText = componentsFromSerial[0];
        if (componentsFromSerial.Length == 1)
          ChatManager.welcomeColor = Palette.SERVER;
        else if (componentsFromSerial.Length == 4)
        {
          byte result1;
          if (!byte.TryParse(componentsFromSerial[1], out result1))
          {
            CommandWindow.LogError((object) this.localization.format("InvalidNumberErrorText", (object) componentsFromSerial[0]));
            return;
          }
          byte result2;
          if (!byte.TryParse(componentsFromSerial[2], out result2))
          {
            CommandWindow.LogError((object) this.localization.format("InvalidNumberErrorText", (object) componentsFromSerial[1]));
            return;
          }
          byte result3;
          if (!byte.TryParse(componentsFromSerial[3], out result3))
          {
            CommandWindow.LogError((object) this.localization.format("InvalidNumberErrorText", (object) componentsFromSerial[2]));
            return;
          }
          ChatManager.welcomeColor = new Color((float) result1 / (float) byte.MaxValue, (float) result2 / (float) byte.MaxValue, (float) result3 / (float) byte.MaxValue);
        }
        CommandWindow.Log((object) this.localization.format("WelcomeText", (object) componentsFromSerial[0]));
      }
    }
  }
}
