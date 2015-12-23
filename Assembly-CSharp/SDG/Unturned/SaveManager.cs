// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.SaveManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
  public class SaveManager : SteamCaller
  {
    public static void save()
    {
      if (Provider.isClient)
        PlayerInventory.loadout = PlayerInventory.LOADOUT;
      if (Level.info.type == ELevelType.SURVIVAL)
      {
        for (int index = 0; index < Provider.clients.Count; ++index)
        {
          if ((Object) Provider.clients[index].model != (Object) null)
            Provider.clients[index].model.GetComponent<Player>().save();
        }
        VehicleManager.save();
        BarricadeManager.save();
        StructureManager.save();
        ObjectManager.save();
      }
      if (Dedicator.isDedicated)
      {
        SteamWhitelist.save();
        SteamBlacklist.save();
        SteamAdminlist.save();
        ObjectManager.save();
      }
      else
        LightingManager.save();
    }

    private static void onServerShutdown()
    {
      if (!Provider.isServer || !Level.isLoaded)
        return;
      SaveManager.save();
    }

    private static void onServerDisconnected(CSteamID steamID)
    {
      if (!Provider.isServer || !Level.isLoaded)
        return;
      Player player = PlayerTool.getPlayer(steamID);
      if (!((Object) player != (Object) null))
        return;
      player.save();
    }

    private void Start()
    {
      Provider.onServerShutdown += new Provider.ServerShutdown(SaveManager.onServerShutdown);
      Provider.onServerDisconnected += new Provider.ServerDisconnected(SaveManager.onServerDisconnected);
    }
  }
}
