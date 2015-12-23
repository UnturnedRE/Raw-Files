// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.CommandDebug
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
  public class CommandDebug : Command
  {
    public CommandDebug(Local newLocalization)
    {
      this.localization = newLocalization;
      this._command = this.localization.format("DebugCommandText");
      this._info = this.localization.format("DebugInfoText");
      this._help = this.localization.format("DebugHelpText");
    }

    protected override void execute(CSteamID executorID, string parameter)
    {
      if (!Dedicator.isDedicated)
        return;
      CommandWindow.Log((object) this.localization.format("DebugText"));
      CommandWindow.Log((object) this.localization.format("DebugIPPortText", (object) SteamGameServer.GetSteamID(), (object) Parser.getIPFromUInt32(SteamGameServer.GetPublicIP()), (object) Provider.port));
      CommandWindow.Log((object) this.localization.format("DebugBytesSentText", (object) ((string) (object) Provider.bytesSent + (object) "B")));
      CommandWindow.Log((object) this.localization.format("DebugBytesReceivedText", (object) ((string) (object) Provider.bytesReceived + (object) "B")));
      CommandWindow.Log((object) this.localization.format("DebugAverageBytesSentText", (object) ((string) (object) (uint) ((double) Provider.bytesSent / (double) Time.realtimeSinceStartup) + (object) "B")));
      CommandWindow.Log((object) this.localization.format("DebugAverageBytesReceivedText", (object) ((string) (object) (uint) ((double) Provider.bytesReceived / (double) Time.realtimeSinceStartup) + (object) "B")));
      CommandWindow.Log((object) this.localization.format("DebugPacketsSentText", (object) Provider.packetsSent));
      CommandWindow.Log((object) this.localization.format("DebugPacketsReceivedText", (object) Provider.packetsReceived));
    }
  }
}
