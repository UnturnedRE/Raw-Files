// Decompiled with JetBrains decompiler
// Type: SDG.SteamworksProvider.Services.Matchmaking.SteamworksMatchmakingService
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using SDG.Provider.Services;
using SDG.Provider.Services.Matchmaking;
using Steamworks;
using System.Collections.Generic;

namespace SDG.SteamworksProvider.Services.Matchmaking
{
  public class SteamworksMatchmakingService : Service, IService, IMatchmakingService
  {
    public static List<SteamworksServerInfoRequestHandle> serverInfoRequestHandles;

    public IServerInfoRequestHandle requestServerInfo(uint ip, ushort port, ServerInfoRequestReadyCallback callback)
    {
      SteamworksServerInfoRequestHandle infoRequestHandle = new SteamworksServerInfoRequestHandle(callback);
      ISteamMatchmakingPingResponse pRequestServersResponse = new ISteamMatchmakingPingResponse(new ISteamMatchmakingPingResponse.ServerResponded(infoRequestHandle.onServerResponded), new ISteamMatchmakingPingResponse.ServerFailedToRespond(infoRequestHandle.onServerFailedToRespond));
      infoRequestHandle.pingResponse = pRequestServersResponse;
      HServerQuery hserverQuery = SteamMatchmakingServers.PingServer(ip, (ushort) ((uint) port + 1U), pRequestServersResponse);
      infoRequestHandle.query = hserverQuery;
      SteamworksMatchmakingService.serverInfoRequestHandles.Add(infoRequestHandle);
      return (IServerInfoRequestHandle) infoRequestHandle;
    }
  }
}
