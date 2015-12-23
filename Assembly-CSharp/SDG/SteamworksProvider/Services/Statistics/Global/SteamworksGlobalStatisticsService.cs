// Decompiled with JetBrains decompiler
// Type: SDG.SteamworksProvider.Services.Statistics.Global.SteamworksGlobalStatisticsService
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using SDG.Provider.Services;
using SDG.Provider.Services.Statistics.Global;
using Steamworks;
using System;

namespace SDG.SteamworksProvider.Services.Statistics.Global
{
  public class SteamworksGlobalStatisticsService : Service, IService, IGlobalStatisticsService
  {
    private Callback<GlobalStatsReceived_t> globalStatsReceived;

    public event GlobalStatisticsRequestReady onGlobalStatisticsRequestReady;

    public SteamworksGlobalStatisticsService()
    {
      this.globalStatsReceived = Callback<GlobalStatsReceived_t>.Create(new Callback<GlobalStatsReceived_t>.DispatchDelegate(this.onGlobalStatsReceived));
    }

    private void triggerGlobalStatisticsRequestReady()
    {
      if (this.onGlobalStatisticsRequestReady == null)
        return;
      this.onGlobalStatisticsRequestReady();
    }

    public bool getStatistic(string name, out long data)
    {
      if (name == null)
        throw new ArgumentNullException("name");
      return SteamUserStats.GetGlobalStat(name, out data);
    }

    public bool getStatistic(string name, out double data)
    {
      if (name == null)
        throw new ArgumentNullException("name");
      return SteamUserStats.GetGlobalStat(name, out data);
    }

    public bool requestStatistics()
    {
      SteamUserStats.RequestGlobalStats(0);
      return true;
    }

    private void onGlobalStatsReceived(GlobalStatsReceived_t callback)
    {
      if ((long) callback.m_nGameID != (long) SteamUtils.GetAppID().m_AppId)
        return;
      this.triggerGlobalStatisticsRequestReady();
    }
  }
}
