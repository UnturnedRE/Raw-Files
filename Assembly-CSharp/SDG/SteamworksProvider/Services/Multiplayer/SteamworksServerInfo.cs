// Decompiled with JetBrains decompiler
// Type: SDG.SteamworksProvider.Services.Multiplayer.SteamworksServerInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using SDG.Provider.Services.Community;
using SDG.Provider.Services.Multiplayer;
using SDG.SteamworksProvider.Services.Community;
using Steamworks;

namespace SDG.SteamworksProvider.Services.Multiplayer
{
  public class SteamworksServerInfo : IServerInfo
  {
    public ICommunityEntity entity { get; protected set; }

    public string name { get; protected set; }

    public byte players { get; protected set; }

    public byte capacity { get; protected set; }

    public int ping { get; protected set; }

    public SteamworksServerInfo(gameserveritem_t server)
    {
      this.entity = (ICommunityEntity) new SteamworksCommunityEntity(server.m_steamID);
      this.name = server.GetServerName();
      this.players = (byte) server.m_nPlayers;
      this.capacity = (byte) server.m_nMaxPlayers;
      this.ping = server.m_nPing;
    }
  }
}
