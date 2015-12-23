// Decompiled with JetBrains decompiler
// Type: SDG.SteamworksProvider.Services.Statistics.SteamworksStatisticsService
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using SDG.Provider.Services;
using SDG.Provider.Services.Statistics;
using SDG.Provider.Services.Statistics.Global;
using SDG.Provider.Services.Statistics.User;
using SDG.SteamworksProvider.Services.Statistics.Global;
using SDG.SteamworksProvider.Services.Statistics.User;

namespace SDG.SteamworksProvider.Services.Statistics
{
  public class SteamworksStatisticsService : IService, IStatisticsService
  {
    public IUserStatisticsService userStatisticsService { get; protected set; }

    public IGlobalStatisticsService globalStatisticsService { get; protected set; }

    public SteamworksStatisticsService()
    {
      this.userStatisticsService = (IUserStatisticsService) new SteamworksUserStatisticsService();
      this.globalStatisticsService = (IGlobalStatisticsService) new SteamworksGlobalStatisticsService();
    }

    public void initialize()
    {
      this.userStatisticsService.initialize();
      this.globalStatisticsService.initialize();
    }

    public void update()
    {
      this.userStatisticsService.update();
      this.globalStatisticsService.update();
    }

    public void shutdown()
    {
      this.userStatisticsService.shutdown();
      this.globalStatisticsService.shutdown();
    }
  }
}
