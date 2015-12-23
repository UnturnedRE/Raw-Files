// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.CommandLoadout
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;

namespace SDG.Unturned
{
  public class CommandLoadout : Command
  {
    public CommandLoadout(Local newLocalization)
    {
      this.localization = newLocalization;
      this._command = this.localization.format("LoadoutCommandText");
      this._info = this.localization.format("LoadoutInfoText");
      this._help = this.localization.format("LoadoutHelpText");
    }

    protected override void execute(CSteamID executorID, string parameter)
    {
      if (!Dedicator.isDedicated)
        return;
      string[] componentsFromSerial = Parser.getComponentsFromSerial(parameter, '/');
      ushort[] numArray = new ushort[componentsFromSerial.Length];
      for (int index = 0; index < componentsFromSerial.Length; ++index)
      {
        ushort result;
        if (!ushort.TryParse(componentsFromSerial[index], out result))
        {
          CommandWindow.LogError((object) this.localization.format("InvalidItemIDErrorText", (object) componentsFromSerial[index]));
          return;
        }
        numArray[index] = result;
      }
      PlayerInventory.loadout = numArray;
      CommandWindow.Log((object) this.localization.format("LoadoutText"));
    }
  }
}
