// Decompiled with JetBrains decompiler
// Type: SDG.SteamworksProvider.Services.Multiplayer.SteamworksMultiplayerService
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using SDG.Provider.Services;
using SDG.Provider.Services.Multiplayer;
using SDG.Provider.Services.Multiplayer.Client;
using SDG.Provider.Services.Multiplayer.Server;
using SDG.SteamworksProvider;
using SDG.SteamworksProvider.Services.Multiplayer.Client;
using SDG.SteamworksProvider.Services.Multiplayer.Server;

namespace SDG.SteamworksProvider.Services.Multiplayer
{
  public class SteamworksMultiplayerService : IService, IMultiplayerService
  {
    private SteamworksAppInfo appInfo;

    public IClientMultiplayerService clientMultiplayerService { get; protected set; }

    public IServerMultiplayerService serverMultiplayerService { get; protected set; }

    public SteamworksMultiplayerService(SteamworksAppInfo newAppInfo)
    {
      this.appInfo = newAppInfo;
      this.serverMultiplayerService = (IServerMultiplayerService) new SteamworksServerMultiplayerService(this.appInfo);
      if (this.appInfo.isDedicated)
        return;
      this.clientMultiplayerService = (IClientMultiplayerService) new SteamworksClientMultiplayerService();
    }

    public void initialize()
    {
      this.serverMultiplayerService.initialize();
      if (this.appInfo.isDedicated)
        return;
      this.clientMultiplayerService.initialize();
    }

    public void update()
    {
      this.serverMultiplayerService.update();
      if (this.appInfo.isDedicated)
        return;
      this.clientMultiplayerService.update();
    }

    public void shutdown()
    {
      this.serverMultiplayerService.shutdown();
      if (this.appInfo.isDedicated)
        return;
      this.clientMultiplayerService.shutdown();
    }
  }
}
