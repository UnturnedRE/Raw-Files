// Decompiled with JetBrains decompiler
// Type: SDG.Provider.IProvider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using SDG.Provider.Services.Achievements;
using SDG.Provider.Services.Browser;
using SDG.Provider.Services.Cloud;
using SDG.Provider.Services.Community;
using SDG.Provider.Services.Multiplayer;
using SDG.Provider.Services.Statistics;
using SDG.Provider.Services.Store;
using SDG.Provider.Services.Translation;
using SDG.Provider.Services.Web;

namespace SDG.Provider
{
  public interface IProvider
  {
    IAchievementsService achievementsService { get; }

    IBrowserService browserService { get; }

    ICloudService cloudService { get; }

    ICommunityService communityService { get; }

    TempSteamworksEconomy economyService { get; }

    TempSteamworksMatchmaking matchmakingService { get; }

    IMultiplayerService multiplayerService { get; }

    IStatisticsService statisticsService { get; }

    IStoreService storeService { get; }

    ITranslationService translationService { get; }

    IWebService webService { get; }

    TempSteamworksWorkshop workshopService { get; }

    void intialize();

    void update();

    void shutdown();
  }
}
