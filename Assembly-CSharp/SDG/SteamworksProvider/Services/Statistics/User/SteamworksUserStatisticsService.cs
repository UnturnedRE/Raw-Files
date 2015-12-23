// Decompiled with JetBrains decompiler
// Type: SDG.SteamworksProvider.Services.Statistics.User.SteamworksUserStatisticsService
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using SDG.Provider.Services;
using SDG.Provider.Services.Community;
using SDG.Provider.Services.Statistics.User;
using SDG.SteamworksProvider.Services.Community;
using Steamworks;
using System;

namespace SDG.SteamworksProvider.Services.Statistics.User
{
  public class SteamworksUserStatisticsService : Service, IService, IUserStatisticsService
  {
    private Callback<UserStatsReceived_t> userStatsReceivedCallback;

    public event UserStatisticsRequestReady onUserStatisticsRequestReady;

    public SteamworksUserStatisticsService()
    {
      this.userStatsReceivedCallback = Callback<UserStatsReceived_t>.Create(new Callback<UserStatsReceived_t>.DispatchDelegate(this.onUserStatsReceived));
    }

    private void triggerUserStatisticsRequestReady(ICommunityEntity entityID)
    {
      if (this.onUserStatisticsRequestReady == null)
        return;
      this.onUserStatisticsRequestReady(entityID);
    }

    public bool getStatistic(string name, out int data)
    {
      if (name == null)
        throw new ArgumentNullException("name");
      return SteamUserStats.GetStat(name, out data);
    }

    public bool setStatistic(string name, int data)
    {
      if (name == null)
        throw new ArgumentNullException("name");
      bool flag = SteamUserStats.SetStat(name, data);
      SteamUserStats.StoreStats();
      return flag;
    }

    public bool getStatistic(string name, out float data)
    {
      if (name == null)
        throw new ArgumentNullException("name");
      return SteamUserStats.GetStat(name, out data);
    }

    public bool setStatistic(string name, float data)
    {
      if (name == null)
        throw new ArgumentNullException("name");
      bool flag = SteamUserStats.SetStat(name, data);
      SteamUserStats.StoreStats();
      return flag;
    }

    public bool requestStatistics()
    {
      SteamUserStats.RequestCurrentStats();
      return true;
    }

    private void onUserStatsReceived(UserStatsReceived_t callback)
    {
      if ((long) callback.m_nGameID != (long) SteamUtils.GetAppID().m_AppId)
        return;
      this.triggerUserStatisticsRequestReady((ICommunityEntity) new SteamworksCommunityEntity(callback.m_steamIDUser));
    }
  }
}
