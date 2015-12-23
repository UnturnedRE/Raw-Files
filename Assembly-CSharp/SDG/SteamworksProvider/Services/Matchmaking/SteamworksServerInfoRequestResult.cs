// Decompiled with JetBrains decompiler
// Type: SDG.SteamworksProvider.Services.Matchmaking.SteamworksServerInfoRequestResult
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using SDG.Provider.Services.Matchmaking;
using SDG.Provider.Services.Multiplayer;
using SDG.SteamworksProvider.Services.Multiplayer;

namespace SDG.SteamworksProvider.Services.Matchmaking
{
  public class SteamworksServerInfoRequestResult : IServerInfoRequestResult
  {
    public IServerInfo serverInfo { get; protected set; }

    public SteamworksServerInfoRequestResult(SteamworksServerInfo newServerInfo)
    {
      this.serverInfo = (IServerInfo) newServerInfo;
    }
  }
}
