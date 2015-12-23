// Decompiled with JetBrains decompiler
// Type: SDG.Provider.TempSteamworksMatchmaking
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using SDG.Unturned;
using Steamworks;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Provider
{
  public class TempSteamworksMatchmaking
  {
    private HServerQuery serverQuery = HServerQuery.Invalid;
    private HServerListRequest serverListRequest = HServerListRequest.Invalid;
    private IComparer<SteamServerInfo> serverInfoComparer = (IComparer<SteamServerInfo>) new SteamServerInfoPingComparator();
    public TempSteamworksMatchmaking.MasterServerAdded onMasterServerAdded;
    public TempSteamworksMatchmaking.MasterServerRemoved onMasterServerRemoved;
    public TempSteamworksMatchmaking.MasterServerResorted onMasterServerResorted;
    public TempSteamworksMatchmaking.MasterServerRefreshed onMasterServerRefreshed;
    public TempSteamworksMatchmaking.AttemptUpdated onAttemptUpdated;
    public TempSteamworksMatchmaking.TimedOut onTimedOut;
    private SteamConnectionInfo connectionInfo;
    private ESteamServerList currentList;
    private List<SteamServerInfo> _serverList;
    private List<MatchMakingKeyValuePair_t> filters;
    private ISteamMatchmakingPingResponse serverPingResponse;
    private ISteamMatchmakingServerListResponse serverListResponse;
    private int serverQueryAttempts;

    public List<SteamServerInfo> serverList
    {
      get
      {
        return this._serverList;
      }
    }

    public TempSteamworksMatchmaking()
    {
      this.serverPingResponse = new ISteamMatchmakingPingResponse(new ISteamMatchmakingPingResponse.ServerResponded(this.onPingResponded), new ISteamMatchmakingPingResponse.ServerFailedToRespond(this.onPingFailedToRespond));
      this.serverListResponse = new ISteamMatchmakingServerListResponse(new ISteamMatchmakingServerListResponse.ServerResponded(this.onServerListResponded), new ISteamMatchmakingServerListResponse.ServerFailedToRespond(this.onServerListFailedToRespond), new ISteamMatchmakingServerListResponse.RefreshComplete(this.onRefreshComplete));
    }

    public void sortMasterServer(IComparer<SteamServerInfo> newServerInfoComparer)
    {
      this.serverInfoComparer = newServerInfoComparer;
      this.serverList.Sort(this.serverInfoComparer);
      if (this.onMasterServerResorted == null)
        return;
      this.onMasterServerResorted();
    }

    private void cleanupServerQuery()
    {
      if (this.serverQuery == HServerQuery.Invalid)
        return;
      SteamMatchmakingServers.CancelServerQuery(this.serverQuery);
      this.serverQuery = HServerQuery.Invalid;
    }

    private void cleanupServerListRequest()
    {
      if (this.serverListRequest == HServerListRequest.Invalid)
        return;
      SteamMatchmakingServers.ReleaseRequest(this.serverListRequest);
      this.serverListRequest = HServerListRequest.Invalid;
    }

    public void connect(SteamConnectionInfo info)
    {
      if (Provider.isConnected)
        return;
      this.connectionInfo = info;
      this.serverQueryAttempts = 0;
      this.attemptServerQuery();
    }

    private void attemptServerQuery()
    {
      this.cleanupServerQuery();
      this.serverQuery = SteamMatchmakingServers.PingServer(this.connectionInfo.ip, (ushort) ((uint) this.connectionInfo.port + 1U), this.serverPingResponse);
      ++this.serverQueryAttempts;
      if (this.onAttemptUpdated == null)
        return;
      this.onAttemptUpdated(this.serverQueryAttempts);
    }

    public void refreshMasterServer(ESteamServerList list, string filterMap, EPassword filterPassword, EWorkshop filterWorkshop, EAttendance filterAttendance, EProtection filterProtection, ECombat filterCombat, EGameMode filterMode, ECameraMode filterCamera)
    {
      this.currentList = list;
      if (this.onMasterServerRemoved != null)
        this.onMasterServerRemoved();
      this.cleanupServerListRequest();
      this._serverList = new List<SteamServerInfo>();
      if (list == ESteamServerList.HISTORY)
        this.serverListRequest = SteamMatchmakingServers.RequestHistoryServerList(Provider.APP_ID, new MatchMakingKeyValuePair_t[0], 0U, this.serverListResponse);
      else if (list == ESteamServerList.FAVORITES)
        this.serverListRequest = SteamMatchmakingServers.RequestFavoritesServerList(Provider.APP_ID, new MatchMakingKeyValuePair_t[0], 0U, this.serverListResponse);
      else if (list == ESteamServerList.LAN)
      {
        this.serverListRequest = SteamMatchmakingServers.RequestLANServerList(Provider.APP_ID, this.serverListResponse);
      }
      else
      {
        this.filters = new List<MatchMakingKeyValuePair_t>();
        this.filters.Add(new MatchMakingKeyValuePair_t()
        {
          m_szKey = "gamedir",
          m_szValue = "unturned"
        });
        if (filterMap.Length > 0)
          this.filters.Add(new MatchMakingKeyValuePair_t()
          {
            m_szKey = "map",
            m_szValue = filterMap.ToLower()
          });
        if (filterAttendance == EAttendance.EMPTY)
          this.filters.Add(new MatchMakingKeyValuePair_t()
          {
            m_szKey = "noplayers",
            m_szValue = "1"
          });
        else if (filterAttendance == EAttendance.SPACE)
        {
          this.filters.Add(new MatchMakingKeyValuePair_t()
          {
            m_szKey = "notfull",
            m_szValue = "1"
          });
          this.filters.Add(new MatchMakingKeyValuePair_t()
          {
            m_szKey = "hasplayers",
            m_szValue = "1"
          });
        }
        MatchMakingKeyValuePair_t makingKeyValuePairT1 = new MatchMakingKeyValuePair_t();
        makingKeyValuePairT1.m_szKey = "gamedataand";
        if (filterPassword == EPassword.YES)
          makingKeyValuePairT1.m_szValue = "PASS";
        else if (filterPassword == EPassword.NO)
          makingKeyValuePairT1.m_szValue = "SSAP";
        if (filterProtection == EProtection.SECURE)
        {
          // ISSUE: explicit reference operation
          // ISSUE: explicit reference operation
          (^@makingKeyValuePairT1).m_szValue += ",";
          // ISSUE: explicit reference operation
          // ISSUE: explicit reference operation
          (^@makingKeyValuePairT1).m_szValue += "VAC";
          this.filters.Add(new MatchMakingKeyValuePair_t()
          {
            m_szKey = "secure",
            m_szValue = "1"
          });
        }
        else if (filterProtection == EProtection.INSECURE)
        {
          // ISSUE: explicit reference operation
          // ISSUE: explicit reference operation
          (^@makingKeyValuePairT1).m_szValue += ",";
          // ISSUE: explicit reference operation
          // ISSUE: explicit reference operation
          (^@makingKeyValuePairT1).m_szValue += "CAV";
        }
        // ISSUE: explicit reference operation
        // ISSUE: explicit reference operation
        (^@makingKeyValuePairT1).m_szValue += ",";
        // ISSUE: explicit reference operation
        // ISSUE: explicit reference operation
        (^@makingKeyValuePairT1).m_szValue += Provider.APP_VERSION;
        this.filters.Add(makingKeyValuePairT1);
        MatchMakingKeyValuePair_t makingKeyValuePairT2 = new MatchMakingKeyValuePair_t();
        makingKeyValuePairT2.m_szKey = "gametagsand";
        if (filterWorkshop == EWorkshop.YES)
          makingKeyValuePairT2.m_szValue = "WORK";
        else if (filterWorkshop == EWorkshop.NO)
          makingKeyValuePairT2.m_szValue = "KROW";
        if (filterCombat == ECombat.PVP)
        {
          // ISSUE: explicit reference operation
          // ISSUE: explicit reference operation
          (^@makingKeyValuePairT2).m_szValue += ",PVP";
        }
        else if (filterCombat == ECombat.PVE)
        {
          // ISSUE: explicit reference operation
          // ISSUE: explicit reference operation
          (^@makingKeyValuePairT2).m_szValue += ",PVE";
        }
        if (filterMode != EGameMode.ANY)
        {
          // ISSUE: explicit reference operation
          // ISSUE: variable of a reference type
          MatchMakingKeyValuePair_t& local = @makingKeyValuePairT2;
          // ISSUE: explicit reference operation
          string str = (^local).m_szValue + "," + filterMode.ToString();
          // ISSUE: explicit reference operation
          (^local).m_szValue = str;
        }
        if (filterCamera != ECameraMode.ANY)
        {
          // ISSUE: explicit reference operation
          // ISSUE: variable of a reference type
          MatchMakingKeyValuePair_t& local = @makingKeyValuePairT2;
          // ISSUE: explicit reference operation
          string str = (^local).m_szValue + "," + filterCamera.ToString();
          // ISSUE: explicit reference operation
          (^local).m_szValue = str;
        }
        this.filters.Add(makingKeyValuePairT2);
        if (list == ESteamServerList.INTERNET)
        {
          this.serverListRequest = SteamMatchmakingServers.RequestInternetServerList(Provider.APP_ID, this.filters.ToArray(), (uint) this.filters.Count, this.serverListResponse);
        }
        else
        {
          if (list != ESteamServerList.FRIENDS)
            return;
          this.serverListRequest = SteamMatchmakingServers.RequestFriendsServerList(Provider.APP_ID, this.filters.ToArray(), (uint) this.filters.Count, this.serverListResponse);
        }
      }
    }

    private void onPingResponded(gameserveritem_t data)
    {
      this.cleanupServerQuery();
      if ((int) data.m_nAppID == (int) Provider.APP_ID.m_AppId)
      {
        SteamServerInfo info = new SteamServerInfo(data);
        if (info.mode != EGameMode.PRO || Provider.isPro)
        {
          if (!info.isPassworded || this.connectionInfo.password != string.Empty)
          {
            if (info.players < info.maxPlayers && info.maxPlayers >= (int) CommandMaxPlayers.MIN_NUMBER && info.maxPlayers <= (int) CommandMaxPlayers.MAX_NUMBER)
            {
              Provider.connect(info, this.connectionInfo.password);
              return;
            }
            Provider._connectionFailureInfo = ESteamConnectionFailureInfo.FULL;
          }
          else
            Provider._connectionFailureInfo = ESteamConnectionFailureInfo.PASSWORD;
        }
        else
          Provider._connectionFailureInfo = ESteamConnectionFailureInfo.PRO;
      }
      else
        Provider._connectionFailureInfo = ESteamConnectionFailureInfo.TIMED_OUT;
      if (this.onTimedOut == null)
        return;
      this.onTimedOut();
    }

    private void onPingFailedToRespond()
    {
      if (this.serverQueryAttempts < 10)
      {
        this.attemptServerQuery();
      }
      else
      {
        this.cleanupServerQuery();
        Provider._connectionFailureInfo = ESteamConnectionFailureInfo.TIMED_OUT;
        if (this.onTimedOut == null)
          return;
        this.onTimedOut();
      }
    }

    private void onServerListResponded(HServerListRequest request, int index)
    {
      if (request != this.serverListRequest)
        return;
      SteamServerInfo server = new SteamServerInfo(SteamMatchmakingServers.GetServerDetails(request, index));
      if (server.maxPlayers < (int) CommandMaxPlayers.MIN_NUMBER)
        return;
      if (this.currentList == ESteamServerList.INTERNET)
      {
        if (server.maxPlayers > (int) CommandMaxPlayers.MAX_NUMBER / 2)
          return;
      }
      else if (server.maxPlayers > (int) CommandMaxPlayers.MAX_NUMBER)
        return;
      int num = this.serverList.BinarySearch(server, this.serverInfoComparer);
      if (num < 0)
        num = ~num;
      this.serverList.Insert(num, server);
      if (this.onMasterServerAdded == null)
        return;
      this.onMasterServerAdded(num, server);
    }

    private void onServerListFailedToRespond(HServerListRequest request, int index)
    {
    }

    private void onRefreshComplete(HServerListRequest request, EMatchMakingServerResponse response)
    {
      if (request == this.serverListRequest)
      {
        this.cleanupServerListRequest();
        if (this.onMasterServerRefreshed != null)
          this.onMasterServerRefreshed(response);
      }
      if (response == EMatchMakingServerResponse.eNoServersListedOnMasterServer || this.serverList.Count == 0)
        Debug.Log((object) "No servers found on the master server.");
      else if (response == EMatchMakingServerResponse.eServerFailedToRespond)
      {
        Debug.LogError((object) "Failed to connect to the master server.");
      }
      else
      {
        if (response != EMatchMakingServerResponse.eServerResponded)
          return;
        Debug.Log((object) "Successfully refreshed the master server.");
      }
    }

    public delegate void MasterServerAdded(int insert, SteamServerInfo server);

    public delegate void MasterServerRemoved();

    public delegate void MasterServerResorted();

    public delegate void MasterServerRefreshed(EMatchMakingServerResponse response);

    public delegate void AttemptUpdated(int attempt);

    public delegate void TimedOut();
  }
}
