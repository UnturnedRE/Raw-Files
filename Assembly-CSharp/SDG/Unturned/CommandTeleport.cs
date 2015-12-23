// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.CommandTeleport
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
  public class CommandTeleport : Command
  {
    public CommandTeleport(Local newLocalization)
    {
      this.localization = newLocalization;
      this._command = this.localization.format("TeleportCommandText");
      this._info = this.localization.format("TeleportInfoText");
      this._help = this.localization.format("TeleportHelpText");
    }

    protected override void execute(CSteamID executorID, string parameter)
    {
      if (!Provider.isServer)
      {
        CommandWindow.LogError((object) this.localization.format("NotRunningErrorText"));
      }
      else
      {
        string[] componentsFromSerial = Parser.getComponentsFromSerial(parameter, '/');
        if (componentsFromSerial.Length != 2)
        {
          CommandWindow.LogError((object) this.localization.format("InvalidParameterErrorText"));
        }
        else
        {
          SteamPlayer player1;
          if (!PlayerTool.tryGetSteamPlayer(componentsFromSerial[0], out player1))
            CommandWindow.LogError((object) this.localization.format("NoPlayerErrorText", (object) componentsFromSerial[0]));
          else if ((Object) player1.player.movement.getVehicle() != (Object) null)
          {
            CommandWindow.LogError((object) this.localization.format("NoVehicleErrorText"));
          }
          else
          {
            SteamPlayer player2;
            if (PlayerTool.tryGetSteamPlayer(componentsFromSerial[1], out player2))
            {
              player1.player.sendTeleport(player2.player.transform.position, MeasurementTool.angleToByte(player2.player.transform.rotation.eulerAngles.y));
              CommandWindow.Log((object) this.localization.format("TeleportText", (object) player1.playerID.playerName, (object) player2.playerID.playerName));
            }
            else
            {
              Node node = (Node) null;
              for (int index = 0; index < LevelNodes.nodes.Count; ++index)
              {
                if (LevelNodes.nodes[index].type == ENodeType.LOCATION && NameTool.checkNames(componentsFromSerial[1], ((LocationNode) LevelNodes.nodes[index]).name))
                {
                  node = LevelNodes.nodes[index];
                  break;
                }
              }
              if (node != null)
              {
                player1.player.sendTeleport(node.point + new Vector3(0.0f, 0.5f, 0.0f), MeasurementTool.angleToByte(player1.player.transform.rotation.eulerAngles.y));
                CommandWindow.Log((object) this.localization.format("TeleportText", (object) player1.playerID.playerName, (object) ((LocationNode) node).name));
              }
              else
                CommandWindow.LogError((object) this.localization.format("NoLocationErrorText", (object) componentsFromSerial[1]));
            }
          }
        }
      }
    }
  }
}
