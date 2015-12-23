﻿// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamMatchmakingServers
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
  public static class SteamMatchmakingServers
  {
    public static HServerListRequest RequestInternetServerList(AppId_t iApp, MatchMakingKeyValuePair_t[] ppchFilters, uint nFilters, ISteamMatchmakingServerListResponse pRequestServersResponse)
    {
      InteropHelp.TestIfAvailableClient();
      return (HServerListRequest) NativeMethods.ISteamMatchmakingServers_RequestInternetServerList(iApp, (IntPtr) new MMKVPMarshaller(ppchFilters), nFilters, (IntPtr) pRequestServersResponse);
    }

    public static HServerListRequest RequestLANServerList(AppId_t iApp, ISteamMatchmakingServerListResponse pRequestServersResponse)
    {
      InteropHelp.TestIfAvailableClient();
      return (HServerListRequest) NativeMethods.ISteamMatchmakingServers_RequestLANServerList(iApp, (IntPtr) pRequestServersResponse);
    }

    public static HServerListRequest RequestFriendsServerList(AppId_t iApp, MatchMakingKeyValuePair_t[] ppchFilters, uint nFilters, ISteamMatchmakingServerListResponse pRequestServersResponse)
    {
      InteropHelp.TestIfAvailableClient();
      return (HServerListRequest) NativeMethods.ISteamMatchmakingServers_RequestFriendsServerList(iApp, (IntPtr) new MMKVPMarshaller(ppchFilters), nFilters, (IntPtr) pRequestServersResponse);
    }

    public static HServerListRequest RequestFavoritesServerList(AppId_t iApp, MatchMakingKeyValuePair_t[] ppchFilters, uint nFilters, ISteamMatchmakingServerListResponse pRequestServersResponse)
    {
      InteropHelp.TestIfAvailableClient();
      return (HServerListRequest) NativeMethods.ISteamMatchmakingServers_RequestFavoritesServerList(iApp, (IntPtr) new MMKVPMarshaller(ppchFilters), nFilters, (IntPtr) pRequestServersResponse);
    }

    public static HServerListRequest RequestHistoryServerList(AppId_t iApp, MatchMakingKeyValuePair_t[] ppchFilters, uint nFilters, ISteamMatchmakingServerListResponse pRequestServersResponse)
    {
      InteropHelp.TestIfAvailableClient();
      return (HServerListRequest) NativeMethods.ISteamMatchmakingServers_RequestHistoryServerList(iApp, (IntPtr) new MMKVPMarshaller(ppchFilters), nFilters, (IntPtr) pRequestServersResponse);
    }

    public static HServerListRequest RequestSpectatorServerList(AppId_t iApp, MatchMakingKeyValuePair_t[] ppchFilters, uint nFilters, ISteamMatchmakingServerListResponse pRequestServersResponse)
    {
      InteropHelp.TestIfAvailableClient();
      return (HServerListRequest) NativeMethods.ISteamMatchmakingServers_RequestSpectatorServerList(iApp, (IntPtr) new MMKVPMarshaller(ppchFilters), nFilters, (IntPtr) pRequestServersResponse);
    }

    public static void ReleaseRequest(HServerListRequest hServerListRequest)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamMatchmakingServers_ReleaseRequest(hServerListRequest);
    }

    public static gameserveritem_t GetServerDetails(HServerListRequest hRequest, int iServer)
    {
      InteropHelp.TestIfAvailableClient();
      return (gameserveritem_t) Marshal.PtrToStructure(NativeMethods.ISteamMatchmakingServers_GetServerDetails(hRequest, iServer), typeof (gameserveritem_t));
    }

    public static void CancelQuery(HServerListRequest hRequest)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamMatchmakingServers_CancelQuery(hRequest);
    }

    public static void RefreshQuery(HServerListRequest hRequest)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamMatchmakingServers_RefreshQuery(hRequest);
    }

    public static bool IsRefreshing(HServerListRequest hRequest)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamMatchmakingServers_IsRefreshing(hRequest);
    }

    public static int GetServerCount(HServerListRequest hRequest)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamMatchmakingServers_GetServerCount(hRequest);
    }

    public static void RefreshServer(HServerListRequest hRequest, int iServer)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamMatchmakingServers_RefreshServer(hRequest, iServer);
    }

    public static HServerQuery PingServer(uint unIP, ushort usPort, ISteamMatchmakingPingResponse pRequestServersResponse)
    {
      InteropHelp.TestIfAvailableClient();
      return (HServerQuery) NativeMethods.ISteamMatchmakingServers_PingServer(unIP, usPort, (IntPtr) pRequestServersResponse);
    }

    public static HServerQuery PlayerDetails(uint unIP, ushort usPort, ISteamMatchmakingPlayersResponse pRequestServersResponse)
    {
      InteropHelp.TestIfAvailableClient();
      return (HServerQuery) NativeMethods.ISteamMatchmakingServers_PlayerDetails(unIP, usPort, (IntPtr) pRequestServersResponse);
    }

    public static HServerQuery ServerRules(uint unIP, ushort usPort, ISteamMatchmakingRulesResponse pRequestServersResponse)
    {
      InteropHelp.TestIfAvailableClient();
      return (HServerQuery) NativeMethods.ISteamMatchmakingServers_ServerRules(unIP, usPort, (IntPtr) pRequestServersResponse);
    }

    public static void CancelServerQuery(HServerQuery hServerQuery)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamMatchmakingServers_CancelServerQuery(hServerQuery);
    }
  }
}
