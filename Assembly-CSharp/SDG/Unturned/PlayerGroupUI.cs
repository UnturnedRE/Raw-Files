// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.PlayerGroupUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using System.Collections.Generic;

namespace SDG.Unturned
{
  public class PlayerGroupUI
  {
    private static Sleek _container;
    private static List<SleekLabel> _groups;

    public static Sleek container
    {
      get
      {
        return PlayerGroupUI._container;
      }
    }

    public static List<SleekLabel> groups
    {
      get
      {
        return PlayerGroupUI._groups;
      }
    }

    public PlayerGroupUI()
    {
      PlayerGroupUI._container = new Sleek();
      PlayerGroupUI.container.sizeScale_X = 1f;
      PlayerGroupUI.container.sizeScale_Y = 1f;
      PlayerUI.container.add(PlayerGroupUI.container);
      PlayerGroupUI._groups = new List<SleekLabel>();
      if (Characters.active.group != CSteamID.Nil)
      {
        for (int index = 0; index < Provider.clients.Count; ++index)
          PlayerGroupUI.addGroup(Provider.clients[index]);
      }
      Provider.onEnemyConnected = new Provider.EnemyConnected(PlayerGroupUI.onEnemyConnected);
      Provider.onEnemyDisconnected = new Provider.EnemyDisconnected(PlayerGroupUI.onEnemyDisconnected);
    }

    private static void addGroup(SteamPlayer player)
    {
      SleekLabel sleekLabel = new SleekLabel();
      sleekLabel.sizeOffset_X = 200;
      sleekLabel.sizeOffset_Y = 30;
      sleekLabel.text = !(player.playerID.nickName == string.Empty) ? player.playerID.characterName : player.playerID.nickName;
      PlayerGroupUI.container.add((Sleek) sleekLabel);
      sleekLabel.isVisible = false;
      PlayerGroupUI.groups.Add(sleekLabel);
    }

    private static void onEnemyConnected(SteamPlayer player)
    {
      if (Characters.active.group == CSteamID.Nil)
        return;
      PlayerGroupUI.addGroup(player);
    }

    private static void onEnemyDisconnected(SteamPlayer player)
    {
      if (Characters.active.group == CSteamID.Nil)
        return;
      for (int index = 0; index < Provider.clients.Count; ++index)
      {
        if (Provider.clients[index] == player)
        {
          PlayerGroupUI.container.remove((Sleek) PlayerGroupUI.groups[index]);
          PlayerGroupUI.groups.RemoveAt(index);
        }
      }
    }
  }
}
