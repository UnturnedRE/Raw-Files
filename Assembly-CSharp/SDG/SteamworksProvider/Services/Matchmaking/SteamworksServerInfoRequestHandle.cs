// Decompiled with JetBrains decompiler
// Type: SDG.SteamworksProvider.Services.Matchmaking.SteamworksServerInfoRequestHandle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using SDG.Provider.Services.Matchmaking;
using SDG.SteamworksProvider.Services.Multiplayer;
using Steamworks;

namespace SDG.SteamworksProvider.Services.Matchmaking
{
  public class SteamworksServerInfoRequestHandle : IServerInfoRequestHandle
  {
    public HServerQuery query;
    public ISteamMatchmakingPingResponse pingResponse;
    private ServerInfoRequestReadyCallback callback;

    public SteamworksServerInfoRequestHandle(ServerInfoRequestReadyCallback newCallback)
    {
      this.callback = newCallback;
    }

    public void onServerResponded(gameserveritem_t server)
    {
      this.triggerCallback((IServerInfoRequestResult) new SteamworksServerInfoRequestResult(new SteamworksServerInfo(server)));
      this.cleanupQuery();
      SteamworksMatchmakingService.serverInfoRequestHandles.Remove(this);
    }

    public void onServerFailedToRespond()
    {
      this.triggerCallback((IServerInfoRequestResult) new SteamworksServerInfoRequestResult((SteamworksServerInfo) null));
      this.cleanupQuery();
      SteamworksMatchmakingService.serverInfoRequestHandles.Remove(this);
    }

    public void triggerCallback(IServerInfoRequestResult result)
    {
      if (this.callback == null)
        return;
      this.callback((IServerInfoRequestHandle) this, result);
    }

    private void cleanupQuery()
    {
      SteamMatchmakingServers.CancelServerQuery(this.query);
      this.query = HServerQuery.Invalid;
    }
  }
}
