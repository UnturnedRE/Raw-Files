﻿// Decompiled with JetBrains decompiler
// Type: Steamworks.NativeMethods
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
  internal static class NativeMethods
  {
    internal const string NativeLibraryName = "CSteamworks";

    [DllImport("CSteamworks", EntryPoint = "Shutdown", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SteamAPI_Shutdown();

    [DllImport("CSteamworks", EntryPoint = "IsSteamRunning", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool SteamAPI_IsSteamRunning();

    [DllImport("CSteamworks", EntryPoint = "RestartAppIfNecessary", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool SteamAPI_RestartAppIfNecessary(AppId_t unOwnAppID);

    [DllImport("CSteamworks", EntryPoint = "WriteMiniDump", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SteamAPI_WriteMiniDump(uint uStructuredExceptionCode, IntPtr pvExceptionInfo, uint uBuildID);

    [DllImport("CSteamworks", EntryPoint = "SetMiniDumpComment", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SteamAPI_SetMiniDumpComment(InteropHelp.UTF8StringHandle pchMsg);

    [DllImport("CSteamworks", EntryPoint = "SteamClient_", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr SteamClient();

    [DllImport("CSteamworks", EntryPoint = "InitSafe", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool SteamAPI_InitSafe();

    [DllImport("CSteamworks", EntryPoint = "RunCallbacks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SteamAPI_RunCallbacks();

    [DllImport("CSteamworks", EntryPoint = "RegisterCallback", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SteamAPI_RegisterCallback(IntPtr pCallback, int iCallback);

    [DllImport("CSteamworks", EntryPoint = "UnregisterCallback", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SteamAPI_UnregisterCallback(IntPtr pCallback);

    [DllImport("CSteamworks", EntryPoint = "RegisterCallResult", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SteamAPI_RegisterCallResult(IntPtr pCallback, ulong hAPICall);

    [DllImport("CSteamworks", EntryPoint = "UnregisterCallResult", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SteamAPI_UnregisterCallResult(IntPtr pCallback, ulong hAPICall);

    [DllImport("CSteamworks", EntryPoint = "Steam_RunCallbacks_", CallingConvention = CallingConvention.Cdecl)]
    public static extern void Steam_RunCallbacks(HSteamPipe hSteamPipe, [MarshalAs(UnmanagedType.I1)] bool bGameServerCallbacks);

    [DllImport("CSteamworks", EntryPoint = "Steam_RegisterInterfaceFuncs_", CallingConvention = CallingConvention.Cdecl)]
    public static extern void Steam_RegisterInterfaceFuncs(IntPtr hModule);

    [DllImport("CSteamworks", EntryPoint = "Steam_GetHSteamUserCurrent_", CallingConvention = CallingConvention.Cdecl)]
    public static extern int Steam_GetHSteamUserCurrent();

    [DllImport("CSteamworks", EntryPoint = "GetSteamInstallPath", CallingConvention = CallingConvention.Cdecl)]
    public static extern int SteamAPI_GetSteamInstallPath();

    [DllImport("CSteamworks", EntryPoint = "GetHSteamPipe_", CallingConvention = CallingConvention.Cdecl)]
    public static extern int SteamAPI_GetHSteamPipe();

    [DllImport("CSteamworks", EntryPoint = "SetTryCatchCallbacks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SteamAPI_SetTryCatchCallbacks([MarshalAs(UnmanagedType.I1)] bool bTryCatchCallbacks);

    [DllImport("CSteamworks", EntryPoint = "GetHSteamUser_", CallingConvention = CallingConvention.Cdecl)]
    public static extern int SteamAPI_GetHSteamUser();

    [DllImport("CSteamworks", EntryPoint = "UseBreakpadCrashHandler", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SteamAPI_UseBreakpadCrashHandler(InteropHelp.UTF8StringHandle pchVersion, InteropHelp.UTF8StringHandle pchDate, InteropHelp.UTF8StringHandle pchTime, [MarshalAs(UnmanagedType.I1)] bool bFullMemoryDumps, IntPtr pvContext, IntPtr m_pfnPreMinidumpCallback);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr SteamUser();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr SteamFriends();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr SteamUtils();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr SteamMatchmaking();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr SteamUserStats();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr SteamApps();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr SteamNetworking();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr SteamMatchmakingServers();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr SteamRemoteStorage();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr SteamScreenshots();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr SteamHTTP();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr SteamUnifiedMessages();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr SteamController();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr SteamUGC();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr SteamAppList();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr SteamMusic();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr SteamMusicRemote();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr SteamHTMLSurface();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr SteamInventory();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr SteamVideo();

    [DllImport("CSteamworks", EntryPoint = "GameServer_InitSafe", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool SteamGameServer_InitSafe(uint unIP, ushort usSteamPort, ushort usGamePort, ushort usQueryPort, EServerMode eServerMode, InteropHelp.UTF8StringHandle pchVersionString);

    [DllImport("CSteamworks", EntryPoint = "GameServer_Shutdown", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SteamGameServer_Shutdown();

    [DllImport("CSteamworks", EntryPoint = "GameServer_RunCallbacks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SteamGameServer_RunCallbacks();

    [DllImport("CSteamworks", EntryPoint = "GameServer_BSecure", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool SteamGameServer_BSecure();

    [DllImport("CSteamworks", EntryPoint = "GameServer_GetSteamID", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong SteamGameServer_GetSteamID();

    [DllImport("CSteamworks", EntryPoint = "GameServer_GetHSteamPipe", CallingConvention = CallingConvention.Cdecl)]
    public static extern int SteamGameServer_GetHSteamPipe();

    [DllImport("CSteamworks", EntryPoint = "GameServer_GetHSteamUser", CallingConvention = CallingConvention.Cdecl)]
    public static extern int SteamGameServer_GetHSteamUser();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr SteamClientGameServer();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr SteamGameServer();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr SteamGameServerUtils();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr SteamGameServerNetworking();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr SteamGameServerStats();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr SteamGameServerHTTP();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr SteamGameServerInventory();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr SteamGameServerUGC();

    [DllImport("sdkencryptedappticket", EntryPoint = "SteamEncryptedAppTicket_BDecryptTicket", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool BDecryptTicket([In, Out] byte[] rgubTicketEncrypted, uint cubTicketEncrypted, [In, Out] byte[] rgubTicketDecrypted, ref uint pcubTicketDecrypted, [MarshalAs(UnmanagedType.LPArray, SizeConst = 32)] byte[] rgubKey, int cubKey);

    [DllImport("sdkencryptedappticket", EntryPoint = "SteamEncryptedAppTicket_BIsTicketForApp", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool BIsTicketForApp([In, Out] byte[] rgubTicketDecrypted, uint cubTicketDecrypted, AppId_t nAppID);

    [DllImport("sdkencryptedappticket", EntryPoint = "SteamEncryptedAppTicket_GetTicketIssueTime", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint GetTicketIssueTime([In, Out] byte[] rgubTicketDecrypted, uint cubTicketDecrypted);

    [DllImport("sdkencryptedappticket", EntryPoint = "SteamEncryptedAppTicket_GetTicketSteamID", CallingConvention = CallingConvention.Cdecl)]
    public static extern void GetTicketSteamID([In, Out] byte[] rgubTicketDecrypted, uint cubTicketDecrypted, out CSteamID psteamID);

    [DllImport("sdkencryptedappticket", EntryPoint = "SteamEncryptedAppTicket_GetTicketAppID", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint GetTicketAppID([In, Out] byte[] rgubTicketDecrypted, uint cubTicketDecrypted);

    [DllImport("sdkencryptedappticket", EntryPoint = "SteamEncryptedAppTicket_BUserOwnsAppInTicket", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool BUserOwnsAppInTicket([In, Out] byte[] rgubTicketDecrypted, uint cubTicketDecrypted, AppId_t nAppID);

    [DllImport("sdkencryptedappticket", EntryPoint = "SteamEncryptedAppTicket_BUserIsVacBanned", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool BUserIsVacBanned([In, Out] byte[] rgubTicketDecrypted, uint cubTicketDecrypted);

    [DllImport("sdkencryptedappticket", EntryPoint = "SteamEncryptedAppTicket_GetUserVariableData", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr GetUserVariableData([In, Out] byte[] rgubTicketDecrypted, uint cubTicketDecrypted, out uint pcubUserData);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ISteamAppList_GetNumInstalledApps();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ISteamAppList_GetInstalledApps([In, Out] AppId_t[] pvecAppID, uint unMaxAppIDs);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ISteamAppList_GetAppName(AppId_t nAppID, IntPtr pchName, int cchNameMax);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ISteamAppList_GetAppInstallDir(AppId_t nAppID, IntPtr pchDirectory, int cchNameMax);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ISteamAppList_GetAppBuildId(AppId_t nAppID);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamApps_BIsSubscribed();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamApps_BIsLowViolence();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamApps_BIsCybercafe();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamApps_BIsVACBanned();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ISteamApps_GetCurrentGameLanguage();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ISteamApps_GetAvailableGameLanguages();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamApps_BIsSubscribedApp(AppId_t appID);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamApps_BIsDlcInstalled(AppId_t appID);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ISteamApps_GetEarliestPurchaseUnixTime(AppId_t nAppID);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamApps_BIsSubscribedFromFreeWeekend();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ISteamApps_GetDLCCount();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamApps_BGetDLCDataByIndex(int iDLC, out AppId_t pAppID, out bool pbAvailable, IntPtr pchName, int cchNameBufferSize);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamApps_InstallDLC(AppId_t nAppID);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamApps_UninstallDLC(AppId_t nAppID);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamApps_RequestAppProofOfPurchaseKey(AppId_t nAppID);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamApps_GetCurrentBetaName(IntPtr pchName, int cchNameBufferSize);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamApps_MarkContentCorrupt([MarshalAs(UnmanagedType.I1)] bool bMissingFilesOnly);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ISteamApps_GetInstalledDepots(AppId_t appID, [In, Out] DepotId_t[] pvecDepots, uint cMaxDepots);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ISteamApps_GetAppInstallDir(AppId_t appID, IntPtr pchFolder, uint cchFolderBufferSize);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamApps_BIsAppInstalled(AppId_t appID);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamApps_GetAppOwner();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ISteamApps_GetLaunchQueryParam(InteropHelp.UTF8StringHandle pchKey);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamApps_GetDlcDownloadProgress(AppId_t nAppID, out ulong punBytesDownloaded, out ulong punBytesTotal);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ISteamApps_GetAppBuildId();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ISteamClient_CreateSteamPipe();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamClient_BReleaseSteamPipe(HSteamPipe hSteamPipe);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ISteamClient_ConnectToGlobalUser(HSteamPipe hSteamPipe);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ISteamClient_CreateLocalUser(out HSteamPipe phSteamPipe, EAccountType eAccountType);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamClient_ReleaseUser(HSteamPipe hSteamPipe, HSteamUser hUser);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ISteamClient_GetISteamUser(HSteamUser hSteamUser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ISteamClient_GetISteamGameServer(HSteamUser hSteamUser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamClient_SetLocalIPBinding(uint unIP, ushort usPort);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ISteamClient_GetISteamFriends(HSteamUser hSteamUser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ISteamClient_GetISteamUtils(HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ISteamClient_GetISteamMatchmaking(HSteamUser hSteamUser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ISteamClient_GetISteamMatchmakingServers(HSteamUser hSteamUser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ISteamClient_GetISteamGenericInterface(HSteamUser hSteamUser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ISteamClient_GetISteamUserStats(HSteamUser hSteamUser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ISteamClient_GetISteamGameServerStats(HSteamUser hSteamuser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ISteamClient_GetISteamApps(HSteamUser hSteamUser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ISteamClient_GetISteamNetworking(HSteamUser hSteamUser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ISteamClient_GetISteamRemoteStorage(HSteamUser hSteamuser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ISteamClient_GetISteamScreenshots(HSteamUser hSteamuser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamClient_RunFrame();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ISteamClient_GetIPCCallCount();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamClient_SetWarningMessageHook(SteamAPIWarningMessageHook_t pFunction);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamClient_BShutdownIfAllPipesClosed();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ISteamClient_GetISteamHTTP(HSteamUser hSteamuser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ISteamClient_GetISteamUnifiedMessages(HSteamUser hSteamuser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ISteamClient_GetISteamController(HSteamUser hSteamUser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ISteamClient_GetISteamUGC(HSteamUser hSteamUser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ISteamClient_GetISteamAppList(HSteamUser hSteamUser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ISteamClient_GetISteamMusic(HSteamUser hSteamuser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ISteamClient_GetISteamMusicRemote(HSteamUser hSteamuser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ISteamClient_GetISteamHTMLSurface(HSteamUser hSteamuser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamClient_Set_SteamAPI_CPostAPIResultInProcess(SteamAPI_PostAPIResultInProcess_t func);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamClient_Remove_SteamAPI_CPostAPIResultInProcess(SteamAPI_PostAPIResultInProcess_t func);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamClient_Set_SteamAPI_CCheckCallbackRegisteredInProcess(SteamAPI_CheckCallbackRegistered_t func);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ISteamClient_GetISteamInventory(HSteamUser hSteamuser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ISteamClient_GetISteamVideo(HSteamUser hSteamuser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamController_Init(InteropHelp.UTF8StringHandle pchAbsolutePathToControllerConfigVDF);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamController_Shutdown();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamController_RunFrame();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamController_GetControllerState(uint unControllerIndex, out SteamControllerState_t pState);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamController_TriggerHapticPulse(uint unControllerIndex, ESteamControllerPad eTargetPad, ushort usDurationMicroSec);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamController_SetOverrideMode(InteropHelp.UTF8StringHandle pchMode);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ISteamFriends_GetPersonaName();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamFriends_SetPersonaName(InteropHelp.UTF8StringHandle pchPersonaName);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern EPersonaState ISteamFriends_GetPersonaState();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ISteamFriends_GetFriendCount(EFriendFlags iFriendFlags);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamFriends_GetFriendByIndex(int iFriend, EFriendFlags iFriendFlags);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern EFriendRelationship ISteamFriends_GetFriendRelationship(CSteamID steamIDFriend);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern EPersonaState ISteamFriends_GetFriendPersonaState(CSteamID steamIDFriend);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ISteamFriends_GetFriendPersonaName(CSteamID steamIDFriend);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamFriends_GetFriendGamePlayed(CSteamID steamIDFriend, out FriendGameInfo_t pFriendGameInfo);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ISteamFriends_GetFriendPersonaNameHistory(CSteamID steamIDFriend, int iPersonaName);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ISteamFriends_GetFriendSteamLevel(CSteamID steamIDFriend);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ISteamFriends_GetPlayerNickname(CSteamID steamIDPlayer);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ISteamFriends_GetFriendsGroupCount();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern short ISteamFriends_GetFriendsGroupIDByIndex(int iFG);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ISteamFriends_GetFriendsGroupName(FriendsGroupID_t friendsGroupID);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ISteamFriends_GetFriendsGroupMembersCount(FriendsGroupID_t friendsGroupID);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamFriends_GetFriendsGroupMembersList(FriendsGroupID_t friendsGroupID, [In, Out] CSteamID[] pOutSteamIDMembers, int nMembersCount);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamFriends_HasFriend(CSteamID steamIDFriend, EFriendFlags iFriendFlags);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ISteamFriends_GetClanCount();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamFriends_GetClanByIndex(int iClan);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ISteamFriends_GetClanName(CSteamID steamIDClan);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ISteamFriends_GetClanTag(CSteamID steamIDClan);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamFriends_GetClanActivityCounts(CSteamID steamIDClan, out int pnOnline, out int pnInGame, out int pnChatting);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamFriends_DownloadClanActivityCounts([In, Out] CSteamID[] psteamIDClans, int cClansToRequest);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ISteamFriends_GetFriendCountFromSource(CSteamID steamIDSource);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamFriends_GetFriendFromSourceByIndex(CSteamID steamIDSource, int iFriend);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamFriends_IsUserInSource(CSteamID steamIDUser, CSteamID steamIDSource);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamFriends_SetInGameVoiceSpeaking(CSteamID steamIDUser, [MarshalAs(UnmanagedType.I1)] bool bSpeaking);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamFriends_ActivateGameOverlay(InteropHelp.UTF8StringHandle pchDialog);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamFriends_ActivateGameOverlayToUser(InteropHelp.UTF8StringHandle pchDialog, CSteamID steamID);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamFriends_ActivateGameOverlayToWebPage(InteropHelp.UTF8StringHandle pchURL);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamFriends_ActivateGameOverlayToStore(AppId_t nAppID, EOverlayToStoreFlag eFlag);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamFriends_SetPlayedWith(CSteamID steamIDUserPlayedWith);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamFriends_ActivateGameOverlayInviteDialog(CSteamID steamIDLobby);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ISteamFriends_GetSmallFriendAvatar(CSteamID steamIDFriend);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ISteamFriends_GetMediumFriendAvatar(CSteamID steamIDFriend);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ISteamFriends_GetLargeFriendAvatar(CSteamID steamIDFriend);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamFriends_RequestUserInformation(CSteamID steamIDUser, [MarshalAs(UnmanagedType.I1)] bool bRequireNameOnly);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamFriends_RequestClanOfficerList(CSteamID steamIDClan);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamFriends_GetClanOwner(CSteamID steamIDClan);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ISteamFriends_GetClanOfficerCount(CSteamID steamIDClan);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamFriends_GetClanOfficerByIndex(CSteamID steamIDClan, int iOfficer);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ISteamFriends_GetUserRestrictions();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamFriends_SetRichPresence(InteropHelp.UTF8StringHandle pchKey, InteropHelp.UTF8StringHandle pchValue);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamFriends_ClearRichPresence();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ISteamFriends_GetFriendRichPresence(CSteamID steamIDFriend, InteropHelp.UTF8StringHandle pchKey);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ISteamFriends_GetFriendRichPresenceKeyCount(CSteamID steamIDFriend);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ISteamFriends_GetFriendRichPresenceKeyByIndex(CSteamID steamIDFriend, int iKey);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamFriends_RequestFriendRichPresence(CSteamID steamIDFriend);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamFriends_InviteUserToGame(CSteamID steamIDFriend, InteropHelp.UTF8StringHandle pchConnectString);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ISteamFriends_GetCoplayFriendCount();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamFriends_GetCoplayFriend(int iCoplayFriend);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ISteamFriends_GetFriendCoplayTime(CSteamID steamIDFriend);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ISteamFriends_GetFriendCoplayGame(CSteamID steamIDFriend);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamFriends_JoinClanChatRoom(CSteamID steamIDClan);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamFriends_LeaveClanChatRoom(CSteamID steamIDClan);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ISteamFriends_GetClanChatMemberCount(CSteamID steamIDClan);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamFriends_GetChatMemberByIndex(CSteamID steamIDClan, int iUser);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamFriends_SendClanChatMessage(CSteamID steamIDClanChat, InteropHelp.UTF8StringHandle pchText);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ISteamFriends_GetClanChatMessage(CSteamID steamIDClanChat, int iMessage, IntPtr prgchText, int cchTextMax, out EChatEntryType peChatEntryType, out CSteamID psteamidChatter);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamFriends_IsClanChatAdmin(CSteamID steamIDClanChat, CSteamID steamIDUser);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamFriends_IsClanChatWindowOpenInSteam(CSteamID steamIDClanChat);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamFriends_OpenClanChatWindowInSteam(CSteamID steamIDClanChat);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamFriends_CloseClanChatWindowInSteam(CSteamID steamIDClanChat);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamFriends_SetListenForFriendsMessages([MarshalAs(UnmanagedType.I1)] bool bInterceptEnabled);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamFriends_ReplyToFriendMessage(CSteamID steamIDFriend, InteropHelp.UTF8StringHandle pchMsgToSend);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ISteamFriends_GetFriendMessage(CSteamID steamIDFriend, int iMessageID, IntPtr pvData, int cubData, out EChatEntryType peChatEntryType);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamFriends_GetFollowerCount(CSteamID steamID);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamFriends_IsFollowing(CSteamID steamID);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamFriends_EnumerateFollowingList(uint unStartIndex);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServer_InitGameServer(uint unIP, ushort usGamePort, ushort usQueryPort, uint unFlags, AppId_t nGameAppId, InteropHelp.UTF8StringHandle pchVersionString);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamGameServer_SetProduct(InteropHelp.UTF8StringHandle pszProduct);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamGameServer_SetGameDescription(InteropHelp.UTF8StringHandle pszGameDescription);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamGameServer_SetModDir(InteropHelp.UTF8StringHandle pszModDir);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamGameServer_SetDedicatedServer([MarshalAs(UnmanagedType.I1)] bool bDedicated);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamGameServer_LogOn(InteropHelp.UTF8StringHandle pszToken);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamGameServer_LogOnAnonymous();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamGameServer_LogOff();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServer_BLoggedOn();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServer_BSecure();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamGameServer_GetSteamID();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServer_WasRestartRequested();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamGameServer_SetMaxPlayerCount(int cPlayersMax);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamGameServer_SetBotPlayerCount(int cBotplayers);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamGameServer_SetServerName(InteropHelp.UTF8StringHandle pszServerName);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamGameServer_SetMapName(InteropHelp.UTF8StringHandle pszMapName);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamGameServer_SetPasswordProtected([MarshalAs(UnmanagedType.I1)] bool bPasswordProtected);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamGameServer_SetSpectatorPort(ushort unSpectatorPort);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamGameServer_SetSpectatorServerName(InteropHelp.UTF8StringHandle pszSpectatorServerName);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamGameServer_ClearAllKeyValues();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamGameServer_SetKeyValue(InteropHelp.UTF8StringHandle pKey, InteropHelp.UTF8StringHandle pValue);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamGameServer_SetGameTags(InteropHelp.UTF8StringHandle pchGameTags);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamGameServer_SetGameData(InteropHelp.UTF8StringHandle pchGameData);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamGameServer_SetRegion(InteropHelp.UTF8StringHandle pszRegion);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServer_SendUserConnectAndAuthenticate(uint unIPClient, [In, Out] byte[] pvAuthBlob, uint cubAuthBlobSize, out CSteamID pSteamIDUser);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamGameServer_CreateUnauthenticatedUserConnection();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamGameServer_SendUserDisconnect(CSteamID steamIDUser);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServer_BUpdateUserData(CSteamID steamIDUser, InteropHelp.UTF8StringHandle pchPlayerName, uint uScore);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ISteamGameServer_GetAuthSessionTicket([In, Out] byte[] pTicket, int cbMaxTicket, out uint pcbTicket);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern EBeginAuthSessionResult ISteamGameServer_BeginAuthSession([In, Out] byte[] pAuthTicket, int cbAuthTicket, CSteamID steamID);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamGameServer_EndAuthSession(CSteamID steamID);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamGameServer_CancelAuthTicket(HAuthTicket hAuthTicket);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern EUserHasLicenseForAppResult ISteamGameServer_UserHasLicenseForApp(CSteamID steamID, AppId_t appID);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServer_RequestUserGroupStatus(CSteamID steamIDUser, CSteamID steamIDGroup);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamGameServer_GetGameplayStats();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamGameServer_GetServerReputation();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ISteamGameServer_GetPublicIP();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServer_HandleIncomingPacket([In, Out] byte[] pData, int cbData, uint srcIP, ushort srcPort);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ISteamGameServer_GetNextOutgoingPacket([In, Out] byte[] pOut, int cbMaxOut, out uint pNetAdr, out ushort pPort);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamGameServer_EnableHeartbeats([MarshalAs(UnmanagedType.I1)] bool bActive);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamGameServer_SetHeartbeatInterval(int iHeartbeatInterval);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamGameServer_ForceHeartbeat();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamGameServer_AssociateWithClan(CSteamID steamIDClan);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamGameServer_ComputeNewPlayerCompatibility(CSteamID steamIDNewPlayer);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamGameServerStats_RequestUserStats(CSteamID steamIDUser);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerStats_GetUserStat(CSteamID steamIDUser, InteropHelp.UTF8StringHandle pchName, out int pData);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerStats_GetUserStat_(CSteamID steamIDUser, InteropHelp.UTF8StringHandle pchName, out float pData);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerStats_GetUserAchievement(CSteamID steamIDUser, InteropHelp.UTF8StringHandle pchName, out bool pbAchieved);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerStats_SetUserStat(CSteamID steamIDUser, InteropHelp.UTF8StringHandle pchName, int nData);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerStats_SetUserStat_(CSteamID steamIDUser, InteropHelp.UTF8StringHandle pchName, float fData);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerStats_UpdateUserAvgRateStat(CSteamID steamIDUser, InteropHelp.UTF8StringHandle pchName, float flCountThisSession, double dSessionLength);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerStats_SetUserAchievement(CSteamID steamIDUser, InteropHelp.UTF8StringHandle pchName);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerStats_ClearUserAchievement(CSteamID steamIDUser, InteropHelp.UTF8StringHandle pchName);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamGameServerStats_StoreUserStats(CSteamID steamIDUser);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamHTMLSurface_Init();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamHTMLSurface_Shutdown();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamHTMLSurface_CreateBrowser(InteropHelp.UTF8StringHandle pchUserAgent, InteropHelp.UTF8StringHandle pchUserCSS);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamHTMLSurface_RemoveBrowser(HHTMLBrowser unBrowserHandle);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamHTMLSurface_LoadURL(HHTMLBrowser unBrowserHandle, InteropHelp.UTF8StringHandle pchURL, InteropHelp.UTF8StringHandle pchPostData);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamHTMLSurface_SetSize(HHTMLBrowser unBrowserHandle, uint unWidth, uint unHeight);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamHTMLSurface_StopLoad(HHTMLBrowser unBrowserHandle);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamHTMLSurface_Reload(HHTMLBrowser unBrowserHandle);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamHTMLSurface_GoBack(HHTMLBrowser unBrowserHandle);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamHTMLSurface_GoForward(HHTMLBrowser unBrowserHandle);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamHTMLSurface_AddHeader(HHTMLBrowser unBrowserHandle, InteropHelp.UTF8StringHandle pchKey, InteropHelp.UTF8StringHandle pchValue);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamHTMLSurface_ExecuteJavascript(HHTMLBrowser unBrowserHandle, InteropHelp.UTF8StringHandle pchScript);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamHTMLSurface_MouseUp(HHTMLBrowser unBrowserHandle, EHTMLMouseButton eMouseButton);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamHTMLSurface_MouseDown(HHTMLBrowser unBrowserHandle, EHTMLMouseButton eMouseButton);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamHTMLSurface_MouseDoubleClick(HHTMLBrowser unBrowserHandle, EHTMLMouseButton eMouseButton);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamHTMLSurface_MouseMove(HHTMLBrowser unBrowserHandle, int x, int y);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamHTMLSurface_MouseWheel(HHTMLBrowser unBrowserHandle, int nDelta);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamHTMLSurface_KeyDown(HHTMLBrowser unBrowserHandle, uint nNativeKeyCode, EHTMLKeyModifiers eHTMLKeyModifiers);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamHTMLSurface_KeyUp(HHTMLBrowser unBrowserHandle, uint nNativeKeyCode, EHTMLKeyModifiers eHTMLKeyModifiers);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamHTMLSurface_KeyChar(HHTMLBrowser unBrowserHandle, uint cUnicodeChar, EHTMLKeyModifiers eHTMLKeyModifiers);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamHTMLSurface_SetHorizontalScroll(HHTMLBrowser unBrowserHandle, uint nAbsolutePixelScroll);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamHTMLSurface_SetVerticalScroll(HHTMLBrowser unBrowserHandle, uint nAbsolutePixelScroll);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamHTMLSurface_SetKeyFocus(HHTMLBrowser unBrowserHandle, [MarshalAs(UnmanagedType.I1)] bool bHasKeyFocus);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamHTMLSurface_ViewSource(HHTMLBrowser unBrowserHandle);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamHTMLSurface_CopyToClipboard(HHTMLBrowser unBrowserHandle);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamHTMLSurface_PasteFromClipboard(HHTMLBrowser unBrowserHandle);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamHTMLSurface_Find(HHTMLBrowser unBrowserHandle, InteropHelp.UTF8StringHandle pchSearchStr, [MarshalAs(UnmanagedType.I1)] bool bCurrentlyInFind, [MarshalAs(UnmanagedType.I1)] bool bReverse);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamHTMLSurface_StopFind(HHTMLBrowser unBrowserHandle);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamHTMLSurface_GetLinkAtPosition(HHTMLBrowser unBrowserHandle, int x, int y);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamHTMLSurface_SetCookie(InteropHelp.UTF8StringHandle pchHostname, InteropHelp.UTF8StringHandle pchKey, InteropHelp.UTF8StringHandle pchValue, InteropHelp.UTF8StringHandle pchPath, uint nExpires, [MarshalAs(UnmanagedType.I1)] bool bSecure, [MarshalAs(UnmanagedType.I1)] bool bHTTPOnly);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamHTMLSurface_SetPageScaleFactor(HHTMLBrowser unBrowserHandle, float flZoom, int nPointX, int nPointY);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamHTMLSurface_SetBackgroundMode(HHTMLBrowser unBrowserHandle, [MarshalAs(UnmanagedType.I1)] bool bBackgroundMode);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamHTMLSurface_AllowStartRequest(HHTMLBrowser unBrowserHandle, [MarshalAs(UnmanagedType.I1)] bool bAllowed);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamHTMLSurface_JSDialogResponse(HHTMLBrowser unBrowserHandle, [MarshalAs(UnmanagedType.I1)] bool bResult);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamHTMLSurface_FileLoadDialogResponse(HHTMLBrowser unBrowserHandle, IntPtr pchSelectedFiles);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ISteamHTTP_CreateHTTPRequest(EHTTPMethod eHTTPRequestMethod, InteropHelp.UTF8StringHandle pchAbsoluteURL);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamHTTP_SetHTTPRequestContextValue(HTTPRequestHandle hRequest, ulong ulContextValue);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamHTTP_SetHTTPRequestNetworkActivityTimeout(HTTPRequestHandle hRequest, uint unTimeoutSeconds);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamHTTP_SetHTTPRequestHeaderValue(HTTPRequestHandle hRequest, InteropHelp.UTF8StringHandle pchHeaderName, InteropHelp.UTF8StringHandle pchHeaderValue);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamHTTP_SetHTTPRequestGetOrPostParameter(HTTPRequestHandle hRequest, InteropHelp.UTF8StringHandle pchParamName, InteropHelp.UTF8StringHandle pchParamValue);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamHTTP_SendHTTPRequest(HTTPRequestHandle hRequest, out SteamAPICall_t pCallHandle);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamHTTP_SendHTTPRequestAndStreamResponse(HTTPRequestHandle hRequest, out SteamAPICall_t pCallHandle);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamHTTP_DeferHTTPRequest(HTTPRequestHandle hRequest);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamHTTP_PrioritizeHTTPRequest(HTTPRequestHandle hRequest);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamHTTP_GetHTTPResponseHeaderSize(HTTPRequestHandle hRequest, InteropHelp.UTF8StringHandle pchHeaderName, out uint unResponseHeaderSize);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamHTTP_GetHTTPResponseHeaderValue(HTTPRequestHandle hRequest, InteropHelp.UTF8StringHandle pchHeaderName, [In, Out] byte[] pHeaderValueBuffer, uint unBufferSize);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamHTTP_GetHTTPResponseBodySize(HTTPRequestHandle hRequest, out uint unBodySize);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamHTTP_GetHTTPResponseBodyData(HTTPRequestHandle hRequest, [In, Out] byte[] pBodyDataBuffer, uint unBufferSize);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamHTTP_GetHTTPStreamingResponseBodyData(HTTPRequestHandle hRequest, uint cOffset, [In, Out] byte[] pBodyDataBuffer, uint unBufferSize);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamHTTP_ReleaseHTTPRequest(HTTPRequestHandle hRequest);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamHTTP_GetHTTPDownloadProgressPct(HTTPRequestHandle hRequest, out float pflPercentOut);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamHTTP_SetHTTPRequestRawPostBody(HTTPRequestHandle hRequest, InteropHelp.UTF8StringHandle pchContentType, [In, Out] byte[] pubBody, uint unBodyLen);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ISteamHTTP_CreateCookieContainer([MarshalAs(UnmanagedType.I1)] bool bAllowResponsesToModify);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamHTTP_ReleaseCookieContainer(HTTPCookieContainerHandle hCookieContainer);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamHTTP_SetCookie(HTTPCookieContainerHandle hCookieContainer, InteropHelp.UTF8StringHandle pchHost, InteropHelp.UTF8StringHandle pchUrl, InteropHelp.UTF8StringHandle pchCookie);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamHTTP_SetHTTPRequestCookieContainer(HTTPRequestHandle hRequest, HTTPCookieContainerHandle hCookieContainer);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamHTTP_SetHTTPRequestUserAgentInfo(HTTPRequestHandle hRequest, InteropHelp.UTF8StringHandle pchUserAgentInfo);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamHTTP_SetHTTPRequestRequiresVerifiedCertificate(HTTPRequestHandle hRequest, [MarshalAs(UnmanagedType.I1)] bool bRequireVerifiedCertificate);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamHTTP_SetHTTPRequestAbsoluteTimeoutMS(HTTPRequestHandle hRequest, uint unMilliseconds);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamHTTP_GetHTTPRequestWasTimedOut(HTTPRequestHandle hRequest, out bool pbWasTimedOut);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern EResult ISteamInventory_GetResultStatus(SteamInventoryResult_t resultHandle);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamInventory_GetResultItems(SteamInventoryResult_t resultHandle, [In, Out] SteamItemDetails_t[] pOutItemsArray, ref uint punOutItemsArraySize);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ISteamInventory_GetResultTimestamp(SteamInventoryResult_t resultHandle);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamInventory_CheckResultSteamID(SteamInventoryResult_t resultHandle, CSteamID steamIDExpected);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamInventory_DestroyResult(SteamInventoryResult_t resultHandle);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamInventory_GetAllItems(out SteamInventoryResult_t pResultHandle);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamInventory_GetItemsByID(out SteamInventoryResult_t pResultHandle, [In, Out] SteamItemInstanceID_t[] pInstanceIDs, uint unCountInstanceIDs);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamInventory_SerializeResult(SteamInventoryResult_t resultHandle, [In, Out] byte[] pOutBuffer, out uint punOutBufferSize);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamInventory_DeserializeResult(out SteamInventoryResult_t pOutResultHandle, [In, Out] byte[] pBuffer, uint unBufferSize, [MarshalAs(UnmanagedType.I1)] bool bRESERVED_MUST_BE_FALSE);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamInventory_GenerateItems(out SteamInventoryResult_t pResultHandle, [In, Out] SteamItemDef_t[] pArrayItemDefs, [In, Out] uint[] punArrayQuantity, uint unArrayLength);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamInventory_GrantPromoItems(out SteamInventoryResult_t pResultHandle);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamInventory_AddPromoItem(out SteamInventoryResult_t pResultHandle, SteamItemDef_t itemDef);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamInventory_AddPromoItems(out SteamInventoryResult_t pResultHandle, [In, Out] SteamItemDef_t[] pArrayItemDefs, uint unArrayLength);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamInventory_ConsumeItem(out SteamInventoryResult_t pResultHandle, SteamItemInstanceID_t itemConsume, uint unQuantity);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamInventory_ExchangeItems(out SteamInventoryResult_t pResultHandle, [In, Out] SteamItemDef_t[] pArrayGenerate, [In, Out] uint[] punArrayGenerateQuantity, uint unArrayGenerateLength, [In, Out] SteamItemInstanceID_t[] pArrayDestroy, [In, Out] uint[] punArrayDestroyQuantity, uint unArrayDestroyLength);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamInventory_TransferItemQuantity(out SteamInventoryResult_t pResultHandle, SteamItemInstanceID_t itemIdSource, uint unQuantity, SteamItemInstanceID_t itemIdDest);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamInventory_SendItemDropHeartbeat();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamInventory_TriggerItemDrop(out SteamInventoryResult_t pResultHandle, SteamItemDef_t dropListDefinition);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamInventory_TradeItems(out SteamInventoryResult_t pResultHandle, CSteamID steamIDTradePartner, [In, Out] SteamItemInstanceID_t[] pArrayGive, [In, Out] uint[] pArrayGiveQuantity, uint nArrayGiveLength, [In, Out] SteamItemInstanceID_t[] pArrayGet, [In, Out] uint[] pArrayGetQuantity, uint nArrayGetLength);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamInventory_LoadItemDefinitions();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamInventory_GetItemDefinitionIDs([In, Out] SteamItemDef_t[] pItemDefIDs, out uint punItemDefIDsArraySize);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamInventory_GetItemDefinitionProperty(SteamItemDef_t iDefinition, InteropHelp.UTF8StringHandle pchPropertyName, IntPtr pchValueBuffer, ref uint punValueBufferSize);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ISteamMatchmaking_GetFavoriteGameCount();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamMatchmaking_GetFavoriteGame(int iGame, out AppId_t pnAppID, out uint pnIP, out ushort pnConnPort, out ushort pnQueryPort, out uint punFlags, out uint pRTime32LastPlayedOnServer);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ISteamMatchmaking_AddFavoriteGame(AppId_t nAppID, uint nIP, ushort nConnPort, ushort nQueryPort, uint unFlags, uint rTime32LastPlayedOnServer);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamMatchmaking_RemoveFavoriteGame(AppId_t nAppID, uint nIP, ushort nConnPort, ushort nQueryPort, uint unFlags);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamMatchmaking_RequestLobbyList();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamMatchmaking_AddRequestLobbyListStringFilter(InteropHelp.UTF8StringHandle pchKeyToMatch, InteropHelp.UTF8StringHandle pchValueToMatch, ELobbyComparison eComparisonType);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamMatchmaking_AddRequestLobbyListNumericalFilter(InteropHelp.UTF8StringHandle pchKeyToMatch, int nValueToMatch, ELobbyComparison eComparisonType);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamMatchmaking_AddRequestLobbyListNearValueFilter(InteropHelp.UTF8StringHandle pchKeyToMatch, int nValueToBeCloseTo);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamMatchmaking_AddRequestLobbyListFilterSlotsAvailable(int nSlotsAvailable);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamMatchmaking_AddRequestLobbyListDistanceFilter(ELobbyDistanceFilter eLobbyDistanceFilter);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamMatchmaking_AddRequestLobbyListResultCountFilter(int cMaxResults);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamMatchmaking_AddRequestLobbyListCompatibleMembersFilter(CSteamID steamIDLobby);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamMatchmaking_GetLobbyByIndex(int iLobby);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamMatchmaking_CreateLobby(ELobbyType eLobbyType, int cMaxMembers);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamMatchmaking_JoinLobby(CSteamID steamIDLobby);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamMatchmaking_LeaveLobby(CSteamID steamIDLobby);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamMatchmaking_InviteUserToLobby(CSteamID steamIDLobby, CSteamID steamIDInvitee);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ISteamMatchmaking_GetNumLobbyMembers(CSteamID steamIDLobby);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamMatchmaking_GetLobbyMemberByIndex(CSteamID steamIDLobby, int iMember);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ISteamMatchmaking_GetLobbyData(CSteamID steamIDLobby, InteropHelp.UTF8StringHandle pchKey);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamMatchmaking_SetLobbyData(CSteamID steamIDLobby, InteropHelp.UTF8StringHandle pchKey, InteropHelp.UTF8StringHandle pchValue);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ISteamMatchmaking_GetLobbyDataCount(CSteamID steamIDLobby);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamMatchmaking_GetLobbyDataByIndex(CSteamID steamIDLobby, int iLobbyData, IntPtr pchKey, int cchKeyBufferSize, IntPtr pchValue, int cchValueBufferSize);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamMatchmaking_DeleteLobbyData(CSteamID steamIDLobby, InteropHelp.UTF8StringHandle pchKey);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ISteamMatchmaking_GetLobbyMemberData(CSteamID steamIDLobby, CSteamID steamIDUser, InteropHelp.UTF8StringHandle pchKey);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamMatchmaking_SetLobbyMemberData(CSteamID steamIDLobby, InteropHelp.UTF8StringHandle pchKey, InteropHelp.UTF8StringHandle pchValue);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamMatchmaking_SendLobbyChatMsg(CSteamID steamIDLobby, [In, Out] byte[] pvMsgBody, int cubMsgBody);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ISteamMatchmaking_GetLobbyChatEntry(CSteamID steamIDLobby, int iChatID, out CSteamID pSteamIDUser, [In, Out] byte[] pvData, int cubData, out EChatEntryType peChatEntryType);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamMatchmaking_RequestLobbyData(CSteamID steamIDLobby);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamMatchmaking_SetLobbyGameServer(CSteamID steamIDLobby, uint unGameServerIP, ushort unGameServerPort, CSteamID steamIDGameServer);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamMatchmaking_GetLobbyGameServer(CSteamID steamIDLobby, out uint punGameServerIP, out ushort punGameServerPort, out CSteamID psteamIDGameServer);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamMatchmaking_SetLobbyMemberLimit(CSteamID steamIDLobby, int cMaxMembers);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ISteamMatchmaking_GetLobbyMemberLimit(CSteamID steamIDLobby);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamMatchmaking_SetLobbyType(CSteamID steamIDLobby, ELobbyType eLobbyType);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamMatchmaking_SetLobbyJoinable(CSteamID steamIDLobby, [MarshalAs(UnmanagedType.I1)] bool bLobbyJoinable);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamMatchmaking_GetLobbyOwner(CSteamID steamIDLobby);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamMatchmaking_SetLobbyOwner(CSteamID steamIDLobby, CSteamID steamIDNewOwner);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamMatchmaking_SetLinkedLobby(CSteamID steamIDLobby, CSteamID steamIDLobbyDependent);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ISteamMatchmakingServers_RequestInternetServerList(AppId_t iApp, IntPtr ppchFilters, uint nFilters, IntPtr pRequestServersResponse);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ISteamMatchmakingServers_RequestLANServerList(AppId_t iApp, IntPtr pRequestServersResponse);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ISteamMatchmakingServers_RequestFriendsServerList(AppId_t iApp, IntPtr ppchFilters, uint nFilters, IntPtr pRequestServersResponse);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ISteamMatchmakingServers_RequestFavoritesServerList(AppId_t iApp, IntPtr ppchFilters, uint nFilters, IntPtr pRequestServersResponse);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ISteamMatchmakingServers_RequestHistoryServerList(AppId_t iApp, IntPtr ppchFilters, uint nFilters, IntPtr pRequestServersResponse);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ISteamMatchmakingServers_RequestSpectatorServerList(AppId_t iApp, IntPtr ppchFilters, uint nFilters, IntPtr pRequestServersResponse);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamMatchmakingServers_ReleaseRequest(HServerListRequest hServerListRequest);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ISteamMatchmakingServers_GetServerDetails(HServerListRequest hRequest, int iServer);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamMatchmakingServers_CancelQuery(HServerListRequest hRequest);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamMatchmakingServers_RefreshQuery(HServerListRequest hRequest);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamMatchmakingServers_IsRefreshing(HServerListRequest hRequest);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ISteamMatchmakingServers_GetServerCount(HServerListRequest hRequest);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamMatchmakingServers_RefreshServer(HServerListRequest hRequest, int iServer);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ISteamMatchmakingServers_PingServer(uint unIP, ushort usPort, IntPtr pRequestServersResponse);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ISteamMatchmakingServers_PlayerDetails(uint unIP, ushort usPort, IntPtr pRequestServersResponse);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ISteamMatchmakingServers_ServerRules(uint unIP, ushort usPort, IntPtr pRequestServersResponse);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamMatchmakingServers_CancelServerQuery(HServerQuery hServerQuery);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamMusic_BIsEnabled();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamMusic_BIsPlaying();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern AudioPlayback_Status ISteamMusic_GetPlaybackStatus();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamMusic_Play();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamMusic_Pause();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamMusic_PlayPrevious();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamMusic_PlayNext();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamMusic_SetVolume(float flVolume);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern float ISteamMusic_GetVolume();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamMusicRemote_RegisterSteamMusicRemote(InteropHelp.UTF8StringHandle pchName);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamMusicRemote_DeregisterSteamMusicRemote();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamMusicRemote_BIsCurrentMusicRemote();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamMusicRemote_BActivationSuccess([MarshalAs(UnmanagedType.I1)] bool bValue);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamMusicRemote_SetDisplayName(InteropHelp.UTF8StringHandle pchDisplayName);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamMusicRemote_SetPNGIcon_64x64([In, Out] byte[] pvBuffer, uint cbBufferLength);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamMusicRemote_EnablePlayPrevious([MarshalAs(UnmanagedType.I1)] bool bValue);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamMusicRemote_EnablePlayNext([MarshalAs(UnmanagedType.I1)] bool bValue);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamMusicRemote_EnableShuffled([MarshalAs(UnmanagedType.I1)] bool bValue);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamMusicRemote_EnableLooped([MarshalAs(UnmanagedType.I1)] bool bValue);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamMusicRemote_EnableQueue([MarshalAs(UnmanagedType.I1)] bool bValue);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamMusicRemote_EnablePlaylists([MarshalAs(UnmanagedType.I1)] bool bValue);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamMusicRemote_UpdatePlaybackStatus(AudioPlayback_Status nStatus);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamMusicRemote_UpdateShuffled([MarshalAs(UnmanagedType.I1)] bool bValue);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamMusicRemote_UpdateLooped([MarshalAs(UnmanagedType.I1)] bool bValue);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamMusicRemote_UpdateVolume(float flValue);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamMusicRemote_CurrentEntryWillChange();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamMusicRemote_CurrentEntryIsAvailable([MarshalAs(UnmanagedType.I1)] bool bAvailable);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamMusicRemote_UpdateCurrentEntryText(InteropHelp.UTF8StringHandle pchText);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamMusicRemote_UpdateCurrentEntryElapsedSeconds(int nValue);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamMusicRemote_UpdateCurrentEntryCoverArt([In, Out] byte[] pvBuffer, uint cbBufferLength);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamMusicRemote_CurrentEntryDidChange();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamMusicRemote_QueueWillChange();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamMusicRemote_ResetQueueEntries();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamMusicRemote_SetQueueEntry(int nID, int nPosition, InteropHelp.UTF8StringHandle pchEntryText);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamMusicRemote_SetCurrentQueueEntry(int nID);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamMusicRemote_QueueDidChange();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamMusicRemote_PlaylistWillChange();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamMusicRemote_ResetPlaylistEntries();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamMusicRemote_SetPlaylistEntry(int nID, int nPosition, InteropHelp.UTF8StringHandle pchEntryText);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamMusicRemote_SetCurrentPlaylistEntry(int nID);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamMusicRemote_PlaylistDidChange();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamNetworking_SendP2PPacket(CSteamID steamIDRemote, [In, Out] byte[] pubData, uint cubData, EP2PSend eP2PSendType, int nChannel);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamNetworking_IsP2PPacketAvailable(out uint pcubMsgSize, int nChannel);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamNetworking_ReadP2PPacket([In, Out] byte[] pubDest, uint cubDest, out uint pcubMsgSize, out CSteamID psteamIDRemote, int nChannel);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamNetworking_AcceptP2PSessionWithUser(CSteamID steamIDRemote);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamNetworking_CloseP2PSessionWithUser(CSteamID steamIDRemote);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamNetworking_CloseP2PChannelWithUser(CSteamID steamIDRemote, int nChannel);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamNetworking_GetP2PSessionState(CSteamID steamIDRemote, out P2PSessionState_t pConnectionState);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamNetworking_AllowP2PPacketRelay([MarshalAs(UnmanagedType.I1)] bool bAllow);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ISteamNetworking_CreateListenSocket(int nVirtualP2PPort, uint nIP, ushort nPort, [MarshalAs(UnmanagedType.I1)] bool bAllowUseOfPacketRelay);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ISteamNetworking_CreateP2PConnectionSocket(CSteamID steamIDTarget, int nVirtualPort, int nTimeoutSec, [MarshalAs(UnmanagedType.I1)] bool bAllowUseOfPacketRelay);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ISteamNetworking_CreateConnectionSocket(uint nIP, ushort nPort, int nTimeoutSec);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamNetworking_DestroySocket(SNetSocket_t hSocket, [MarshalAs(UnmanagedType.I1)] bool bNotifyRemoteEnd);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamNetworking_DestroyListenSocket(SNetListenSocket_t hSocket, [MarshalAs(UnmanagedType.I1)] bool bNotifyRemoteEnd);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamNetworking_SendDataOnSocket(SNetSocket_t hSocket, IntPtr pubData, uint cubData, [MarshalAs(UnmanagedType.I1)] bool bReliable);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamNetworking_IsDataAvailableOnSocket(SNetSocket_t hSocket, out uint pcubMsgSize);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamNetworking_RetrieveDataFromSocket(SNetSocket_t hSocket, IntPtr pubDest, uint cubDest, out uint pcubMsgSize);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamNetworking_IsDataAvailable(SNetListenSocket_t hListenSocket, out uint pcubMsgSize, out SNetSocket_t phSocket);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamNetworking_RetrieveData(SNetListenSocket_t hListenSocket, IntPtr pubDest, uint cubDest, out uint pcubMsgSize, out SNetSocket_t phSocket);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamNetworking_GetSocketInfo(SNetSocket_t hSocket, out CSteamID pSteamIDRemote, out int peSocketStatus, out uint punIPRemote, out ushort punPortRemote);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamNetworking_GetListenSocketInfo(SNetListenSocket_t hListenSocket, out uint pnIP, out ushort pnPort);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ESNetSocketConnectionType ISteamNetworking_GetSocketConnectionType(SNetSocket_t hSocket);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ISteamNetworking_GetMaxPacketSize(SNetSocket_t hSocket);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamRemoteStorage_FileWrite(InteropHelp.UTF8StringHandle pchFile, [In, Out] byte[] pvData, int cubData);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ISteamRemoteStorage_FileRead(InteropHelp.UTF8StringHandle pchFile, [In, Out] byte[] pvData, int cubDataToRead);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamRemoteStorage_FileForget(InteropHelp.UTF8StringHandle pchFile);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamRemoteStorage_FileDelete(InteropHelp.UTF8StringHandle pchFile);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamRemoteStorage_FileShare(InteropHelp.UTF8StringHandle pchFile);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamRemoteStorage_SetSyncPlatforms(InteropHelp.UTF8StringHandle pchFile, ERemoteStoragePlatform eRemoteStoragePlatform);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamRemoteStorage_FileWriteStreamOpen(InteropHelp.UTF8StringHandle pchFile);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamRemoteStorage_FileWriteStreamWriteChunk(UGCFileWriteStreamHandle_t writeHandle, [In, Out] byte[] pvData, int cubData);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamRemoteStorage_FileWriteStreamClose(UGCFileWriteStreamHandle_t writeHandle);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamRemoteStorage_FileWriteStreamCancel(UGCFileWriteStreamHandle_t writeHandle);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamRemoteStorage_FileExists(InteropHelp.UTF8StringHandle pchFile);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamRemoteStorage_FilePersisted(InteropHelp.UTF8StringHandle pchFile);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ISteamRemoteStorage_GetFileSize(InteropHelp.UTF8StringHandle pchFile);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern long ISteamRemoteStorage_GetFileTimestamp(InteropHelp.UTF8StringHandle pchFile);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ERemoteStoragePlatform ISteamRemoteStorage_GetSyncPlatforms(InteropHelp.UTF8StringHandle pchFile);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ISteamRemoteStorage_GetFileCount();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ISteamRemoteStorage_GetFileNameAndSize(int iFile, out int pnFileSizeInBytes);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamRemoteStorage_GetQuota(out int pnTotalBytes, out int puAvailableBytes);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamRemoteStorage_IsCloudEnabledForAccount();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamRemoteStorage_IsCloudEnabledForApp();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamRemoteStorage_SetCloudEnabledForApp([MarshalAs(UnmanagedType.I1)] bool bEnabled);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamRemoteStorage_UGCDownload(UGCHandle_t hContent, uint unPriority);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamRemoteStorage_GetUGCDownloadProgress(UGCHandle_t hContent, out int pnBytesDownloaded, out int pnBytesExpected);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamRemoteStorage_GetUGCDetails(UGCHandle_t hContent, out AppId_t pnAppID, out IntPtr ppchName, out int pnFileSizeInBytes, out CSteamID pSteamIDOwner);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ISteamRemoteStorage_UGCRead(UGCHandle_t hContent, [In, Out] byte[] pvData, int cubDataToRead, uint cOffset, EUGCReadAction eAction);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ISteamRemoteStorage_GetCachedUGCCount();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamRemoteStorage_GetCachedUGCHandle(int iCachedContent);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamRemoteStorage_PublishWorkshopFile(InteropHelp.UTF8StringHandle pchFile, InteropHelp.UTF8StringHandle pchPreviewFile, AppId_t nConsumerAppId, InteropHelp.UTF8StringHandle pchTitle, InteropHelp.UTF8StringHandle pchDescription, ERemoteStoragePublishedFileVisibility eVisibility, IntPtr pTags, EWorkshopFileType eWorkshopFileType);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamRemoteStorage_CreatePublishedFileUpdateRequest(PublishedFileId_t unPublishedFileId);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamRemoteStorage_UpdatePublishedFileFile(PublishedFileUpdateHandle_t updateHandle, InteropHelp.UTF8StringHandle pchFile);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamRemoteStorage_UpdatePublishedFilePreviewFile(PublishedFileUpdateHandle_t updateHandle, InteropHelp.UTF8StringHandle pchPreviewFile);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamRemoteStorage_UpdatePublishedFileTitle(PublishedFileUpdateHandle_t updateHandle, InteropHelp.UTF8StringHandle pchTitle);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamRemoteStorage_UpdatePublishedFileDescription(PublishedFileUpdateHandle_t updateHandle, InteropHelp.UTF8StringHandle pchDescription);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamRemoteStorage_UpdatePublishedFileVisibility(PublishedFileUpdateHandle_t updateHandle, ERemoteStoragePublishedFileVisibility eVisibility);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamRemoteStorage_UpdatePublishedFileTags(PublishedFileUpdateHandle_t updateHandle, IntPtr pTags);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamRemoteStorage_CommitPublishedFileUpdate(PublishedFileUpdateHandle_t updateHandle);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamRemoteStorage_GetPublishedFileDetails(PublishedFileId_t unPublishedFileId, uint unMaxSecondsOld);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamRemoteStorage_DeletePublishedFile(PublishedFileId_t unPublishedFileId);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamRemoteStorage_EnumerateUserPublishedFiles(uint unStartIndex);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamRemoteStorage_SubscribePublishedFile(PublishedFileId_t unPublishedFileId);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamRemoteStorage_EnumerateUserSubscribedFiles(uint unStartIndex);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamRemoteStorage_UnsubscribePublishedFile(PublishedFileId_t unPublishedFileId);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamRemoteStorage_UpdatePublishedFileSetChangeDescription(PublishedFileUpdateHandle_t updateHandle, InteropHelp.UTF8StringHandle pchChangeDescription);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamRemoteStorage_GetPublishedItemVoteDetails(PublishedFileId_t unPublishedFileId);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamRemoteStorage_UpdateUserPublishedItemVote(PublishedFileId_t unPublishedFileId, [MarshalAs(UnmanagedType.I1)] bool bVoteUp);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamRemoteStorage_GetUserPublishedItemVoteDetails(PublishedFileId_t unPublishedFileId);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamRemoteStorage_EnumerateUserSharedWorkshopFiles(CSteamID steamId, uint unStartIndex, IntPtr pRequiredTags, IntPtr pExcludedTags);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamRemoteStorage_PublishVideo(EWorkshopVideoProvider eVideoProvider, InteropHelp.UTF8StringHandle pchVideoAccount, InteropHelp.UTF8StringHandle pchVideoIdentifier, InteropHelp.UTF8StringHandle pchPreviewFile, AppId_t nConsumerAppId, InteropHelp.UTF8StringHandle pchTitle, InteropHelp.UTF8StringHandle pchDescription, ERemoteStoragePublishedFileVisibility eVisibility, IntPtr pTags);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamRemoteStorage_SetUserPublishedFileAction(PublishedFileId_t unPublishedFileId, EWorkshopFileAction eAction);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamRemoteStorage_EnumeratePublishedFilesByUserAction(EWorkshopFileAction eAction, uint unStartIndex);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamRemoteStorage_EnumeratePublishedWorkshopFiles(EWorkshopEnumerationType eEnumerationType, uint unStartIndex, uint unCount, uint unDays, IntPtr pTags, IntPtr pUserTags);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamRemoteStorage_UGCDownloadToLocation(UGCHandle_t hContent, InteropHelp.UTF8StringHandle pchLocation, uint unPriority);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ISteamScreenshots_WriteScreenshot([In, Out] byte[] pubRGB, uint cubRGB, int nWidth, int nHeight);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ISteamScreenshots_AddScreenshotToLibrary(InteropHelp.UTF8StringHandle pchFilename, InteropHelp.UTF8StringHandle pchThumbnailFilename, int nWidth, int nHeight);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamScreenshots_TriggerScreenshot();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamScreenshots_HookScreenshots([MarshalAs(UnmanagedType.I1)] bool bHook);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamScreenshots_SetLocation(ScreenshotHandle hScreenshot, InteropHelp.UTF8StringHandle pchLocation);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamScreenshots_TagUser(ScreenshotHandle hScreenshot, CSteamID steamID);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamScreenshots_TagPublishedFile(ScreenshotHandle hScreenshot, PublishedFileId_t unPublishedFileID);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamUGC_CreateQueryUserUGCRequest(AccountID_t unAccountID, EUserUGCList eListType, EUGCMatchingUGCType eMatchingUGCType, EUserUGCListSortOrder eSortOrder, AppId_t nCreatorAppID, AppId_t nConsumerAppID, uint unPage);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamUGC_CreateQueryAllUGCRequest(EUGCQuery eQueryType, EUGCMatchingUGCType eMatchingeMatchingUGCTypeFileType, AppId_t nCreatorAppID, AppId_t nConsumerAppID, uint unPage);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamUGC_CreateQueryUGCDetailsRequest([In, Out] PublishedFileId_t[] pvecPublishedFileID, uint unNumPublishedFileIDs);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamUGC_SendQueryUGCRequest(UGCQueryHandle_t handle);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUGC_GetQueryUGCResult(UGCQueryHandle_t handle, uint index, out SteamUGCDetails_t pDetails);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUGC_GetQueryUGCPreviewURL(UGCQueryHandle_t handle, uint index, IntPtr pchURL, uint cchURLSize);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUGC_GetQueryUGCMetadata(UGCQueryHandle_t handle, uint index, IntPtr pchMetadata, uint cchMetadatasize);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUGC_GetQueryUGCChildren(UGCQueryHandle_t handle, uint index, [In, Out] PublishedFileId_t[] pvecPublishedFileID, uint cMaxEntries);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUGC_GetQueryUGCStatistic(UGCQueryHandle_t handle, uint index, EItemStatistic eStatType, out uint pStatValue);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ISteamUGC_GetQueryUGCNumAdditionalPreviews(UGCQueryHandle_t handle, uint index);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUGC_GetQueryUGCAdditionalPreview(UGCQueryHandle_t handle, uint index, uint previewIndex, IntPtr pchURLOrVideoID, uint cchURLSize, out bool pbIsImage);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ISteamUGC_GetQueryUGCNumKeyValueTags(UGCQueryHandle_t handle, uint index);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUGC_GetQueryUGCKeyValueTag(UGCQueryHandle_t handle, uint index, uint keyValueTagIndex, IntPtr pchKey, uint cchKeySize, IntPtr pchValue, uint cchValueSize);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUGC_ReleaseQueryUGCRequest(UGCQueryHandle_t handle);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUGC_AddRequiredTag(UGCQueryHandle_t handle, InteropHelp.UTF8StringHandle pTagName);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUGC_AddExcludedTag(UGCQueryHandle_t handle, InteropHelp.UTF8StringHandle pTagName);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUGC_SetReturnKeyValueTags(UGCQueryHandle_t handle, [MarshalAs(UnmanagedType.I1)] bool bReturnKeyValueTags);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUGC_SetReturnLongDescription(UGCQueryHandle_t handle, [MarshalAs(UnmanagedType.I1)] bool bReturnLongDescription);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUGC_SetReturnMetadata(UGCQueryHandle_t handle, [MarshalAs(UnmanagedType.I1)] bool bReturnMetadata);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUGC_SetReturnChildren(UGCQueryHandle_t handle, [MarshalAs(UnmanagedType.I1)] bool bReturnChildren);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUGC_SetReturnAdditionalPreviews(UGCQueryHandle_t handle, [MarshalAs(UnmanagedType.I1)] bool bReturnAdditionalPreviews);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUGC_SetReturnTotalOnly(UGCQueryHandle_t handle, [MarshalAs(UnmanagedType.I1)] bool bReturnTotalOnly);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUGC_SetLanguage(UGCQueryHandle_t handle, InteropHelp.UTF8StringHandle pchLanguage);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUGC_SetAllowCachedResponse(UGCQueryHandle_t handle, uint unMaxAgeSeconds);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUGC_SetCloudFileNameFilter(UGCQueryHandle_t handle, InteropHelp.UTF8StringHandle pMatchCloudFileName);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUGC_SetMatchAnyTag(UGCQueryHandle_t handle, [MarshalAs(UnmanagedType.I1)] bool bMatchAnyTag);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUGC_SetSearchText(UGCQueryHandle_t handle, InteropHelp.UTF8StringHandle pSearchText);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUGC_SetRankedByTrendDays(UGCQueryHandle_t handle, uint unDays);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUGC_AddRequiredKeyValueTag(UGCQueryHandle_t handle, InteropHelp.UTF8StringHandle pKey, InteropHelp.UTF8StringHandle pValue);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamUGC_RequestUGCDetails(PublishedFileId_t nPublishedFileID, uint unMaxAgeSeconds);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamUGC_CreateItem(AppId_t nConsumerAppId, EWorkshopFileType eFileType);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamUGC_StartItemUpdate(AppId_t nConsumerAppId, PublishedFileId_t nPublishedFileID);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUGC_SetItemTitle(UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pchTitle);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUGC_SetItemDescription(UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pchDescription);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUGC_SetItemUpdateLanguage(UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pchLanguage);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUGC_SetItemMetadata(UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pchMetaData);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUGC_SetItemVisibility(UGCUpdateHandle_t handle, ERemoteStoragePublishedFileVisibility eVisibility);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUGC_SetItemTags(UGCUpdateHandle_t updateHandle, IntPtr pTags);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUGC_SetItemContent(UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pszContentFolder);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUGC_SetItemPreview(UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pszPreviewFile);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUGC_RemoveItemKeyValueTags(UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pchKey);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUGC_AddItemKeyValueTag(UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pchKey, InteropHelp.UTF8StringHandle pchValue);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamUGC_SubmitItemUpdate(UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pchChangeNote);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern EItemUpdateStatus ISteamUGC_GetItemUpdateProgress(UGCUpdateHandle_t handle, out ulong punBytesProcessed, out ulong punBytesTotal);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamUGC_SetUserItemVote(PublishedFileId_t nPublishedFileID, [MarshalAs(UnmanagedType.I1)] bool bVoteUp);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamUGC_GetUserItemVote(PublishedFileId_t nPublishedFileID);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamUGC_AddItemToFavorites(AppId_t nAppId, PublishedFileId_t nPublishedFileID);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamUGC_RemoveItemFromFavorites(AppId_t nAppId, PublishedFileId_t nPublishedFileID);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamUGC_SubscribeItem(PublishedFileId_t nPublishedFileID);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamUGC_UnsubscribeItem(PublishedFileId_t nPublishedFileID);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ISteamUGC_GetNumSubscribedItems();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ISteamUGC_GetSubscribedItems([In, Out] PublishedFileId_t[] pvecPublishedFileID, uint cMaxEntries);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ISteamUGC_GetItemState(PublishedFileId_t nPublishedFileID);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUGC_GetItemInstallInfo(PublishedFileId_t nPublishedFileID, out ulong punSizeOnDisk, IntPtr pchFolder, uint cchFolderSize, out uint punTimeStamp);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUGC_GetItemDownloadInfo(PublishedFileId_t nPublishedFileID, out ulong punBytesDownloaded, out ulong punBytesTotal);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUGC_DownloadItem(PublishedFileId_t nPublishedFileID, [MarshalAs(UnmanagedType.I1)] bool bHighPriority);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamUnifiedMessages_SendMethod(InteropHelp.UTF8StringHandle pchServiceMethod, [In, Out] byte[] pRequestBuffer, uint unRequestBufferSize, ulong unContext);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUnifiedMessages_GetMethodResponseInfo(ClientUnifiedMessageHandle hHandle, out uint punResponseSize, out EResult peResult);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUnifiedMessages_GetMethodResponseData(ClientUnifiedMessageHandle hHandle, [In, Out] byte[] pResponseBuffer, uint unResponseBufferSize, [MarshalAs(UnmanagedType.I1)] bool bAutoRelease);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUnifiedMessages_ReleaseMethod(ClientUnifiedMessageHandle hHandle);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUnifiedMessages_SendNotification(InteropHelp.UTF8StringHandle pchServiceNotification, [In, Out] byte[] pNotificationBuffer, uint unNotificationBufferSize);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ISteamUser_GetHSteamUser();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUser_BLoggedOn();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamUser_GetSteamID();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ISteamUser_InitiateGameConnection([In, Out] byte[] pAuthBlob, int cbMaxAuthBlob, CSteamID steamIDGameServer, uint unIPServer, ushort usPortServer, [MarshalAs(UnmanagedType.I1)] bool bSecure);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamUser_TerminateGameConnection(uint unIPServer, ushort usPortServer);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamUser_TrackAppUsageEvent(CGameID gameID, int eAppUsageEvent, InteropHelp.UTF8StringHandle pchExtraInfo);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUser_GetUserDataFolder(IntPtr pchBuffer, int cubBuffer);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamUser_StartVoiceRecording();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamUser_StopVoiceRecording();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern EVoiceResult ISteamUser_GetAvailableVoice(out uint pcbCompressed, out uint pcbUncompressed, uint nUncompressedVoiceDesiredSampleRate);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern EVoiceResult ISteamUser_GetVoice([MarshalAs(UnmanagedType.I1)] bool bWantCompressed, [In, Out] byte[] pDestBuffer, uint cbDestBufferSize, out uint nBytesWritten, [MarshalAs(UnmanagedType.I1)] bool bWantUncompressed, [In, Out] byte[] pUncompressedDestBuffer, uint cbUncompressedDestBufferSize, out uint nUncompressBytesWritten, uint nUncompressedVoiceDesiredSampleRate);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern EVoiceResult ISteamUser_DecompressVoice([In, Out] byte[] pCompressed, uint cbCompressed, [In, Out] byte[] pDestBuffer, uint cbDestBufferSize, out uint nBytesWritten, uint nDesiredSampleRate);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ISteamUser_GetVoiceOptimalSampleRate();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ISteamUser_GetAuthSessionTicket([In, Out] byte[] pTicket, int cbMaxTicket, out uint pcbTicket);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern EBeginAuthSessionResult ISteamUser_BeginAuthSession([In, Out] byte[] pAuthTicket, int cbAuthTicket, CSteamID steamID);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamUser_EndAuthSession(CSteamID steamID);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamUser_CancelAuthTicket(HAuthTicket hAuthTicket);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern EUserHasLicenseForAppResult ISteamUser_UserHasLicenseForApp(CSteamID steamID, AppId_t appID);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUser_BIsBehindNAT();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamUser_AdvertiseGame(CSteamID steamIDGameServer, uint unIPServer, ushort usPortServer);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamUser_RequestEncryptedAppTicket([In, Out] byte[] pDataToInclude, int cbDataToInclude);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUser_GetEncryptedAppTicket([In, Out] byte[] pTicket, int cbMaxTicket, out uint pcbTicket);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ISteamUser_GetGameBadgeLevel(int nSeries, [MarshalAs(UnmanagedType.I1)] bool bFoil);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ISteamUser_GetPlayerSteamLevel();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamUser_RequestStoreAuthURL(InteropHelp.UTF8StringHandle pchRedirectURL);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUserStats_RequestCurrentStats();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUserStats_GetStat(InteropHelp.UTF8StringHandle pchName, out int pData);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUserStats_GetStat_(InteropHelp.UTF8StringHandle pchName, out float pData);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUserStats_SetStat(InteropHelp.UTF8StringHandle pchName, int nData);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUserStats_SetStat_(InteropHelp.UTF8StringHandle pchName, float fData);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUserStats_UpdateAvgRateStat(InteropHelp.UTF8StringHandle pchName, float flCountThisSession, double dSessionLength);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUserStats_GetAchievement(InteropHelp.UTF8StringHandle pchName, out bool pbAchieved);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUserStats_SetAchievement(InteropHelp.UTF8StringHandle pchName);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUserStats_ClearAchievement(InteropHelp.UTF8StringHandle pchName);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUserStats_GetAchievementAndUnlockTime(InteropHelp.UTF8StringHandle pchName, out bool pbAchieved, out uint punUnlockTime);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUserStats_StoreStats();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ISteamUserStats_GetAchievementIcon(InteropHelp.UTF8StringHandle pchName);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ISteamUserStats_GetAchievementDisplayAttribute(InteropHelp.UTF8StringHandle pchName, InteropHelp.UTF8StringHandle pchKey);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUserStats_IndicateAchievementProgress(InteropHelp.UTF8StringHandle pchName, uint nCurProgress, uint nMaxProgress);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ISteamUserStats_GetNumAchievements();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ISteamUserStats_GetAchievementName(uint iAchievement);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamUserStats_RequestUserStats(CSteamID steamIDUser);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUserStats_GetUserStat(CSteamID steamIDUser, InteropHelp.UTF8StringHandle pchName, out int pData);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUserStats_GetUserStat_(CSteamID steamIDUser, InteropHelp.UTF8StringHandle pchName, out float pData);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUserStats_GetUserAchievement(CSteamID steamIDUser, InteropHelp.UTF8StringHandle pchName, out bool pbAchieved);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUserStats_GetUserAchievementAndUnlockTime(CSteamID steamIDUser, InteropHelp.UTF8StringHandle pchName, out bool pbAchieved, out uint punUnlockTime);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUserStats_ResetAllStats([MarshalAs(UnmanagedType.I1)] bool bAchievementsToo);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamUserStats_FindOrCreateLeaderboard(InteropHelp.UTF8StringHandle pchLeaderboardName, ELeaderboardSortMethod eLeaderboardSortMethod, ELeaderboardDisplayType eLeaderboardDisplayType);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamUserStats_FindLeaderboard(InteropHelp.UTF8StringHandle pchLeaderboardName);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ISteamUserStats_GetLeaderboardName(SteamLeaderboard_t hSteamLeaderboard);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ISteamUserStats_GetLeaderboardEntryCount(SteamLeaderboard_t hSteamLeaderboard);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ELeaderboardSortMethod ISteamUserStats_GetLeaderboardSortMethod(SteamLeaderboard_t hSteamLeaderboard);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ELeaderboardDisplayType ISteamUserStats_GetLeaderboardDisplayType(SteamLeaderboard_t hSteamLeaderboard);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamUserStats_DownloadLeaderboardEntries(SteamLeaderboard_t hSteamLeaderboard, ELeaderboardDataRequest eLeaderboardDataRequest, int nRangeStart, int nRangeEnd);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamUserStats_DownloadLeaderboardEntriesForUsers(SteamLeaderboard_t hSteamLeaderboard, [In, Out] CSteamID[] prgUsers, int cUsers);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUserStats_GetDownloadedLeaderboardEntry(SteamLeaderboardEntries_t hSteamLeaderboardEntries, int index, out LeaderboardEntry_t pLeaderboardEntry, [In, Out] int[] pDetails, int cDetailsMax);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamUserStats_UploadLeaderboardScore(SteamLeaderboard_t hSteamLeaderboard, ELeaderboardUploadScoreMethod eLeaderboardUploadScoreMethod, int nScore, [In, Out] int[] pScoreDetails, int cScoreDetailsCount);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamUserStats_AttachLeaderboardUGC(SteamLeaderboard_t hSteamLeaderboard, UGCHandle_t hUGC);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamUserStats_GetNumberOfCurrentPlayers();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamUserStats_RequestGlobalAchievementPercentages();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ISteamUserStats_GetMostAchievedAchievementInfo(IntPtr pchName, uint unNameBufLen, out float pflPercent, out bool pbAchieved);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ISteamUserStats_GetNextMostAchievedAchievementInfo(int iIteratorPrevious, IntPtr pchName, uint unNameBufLen, out float pflPercent, out bool pbAchieved);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUserStats_GetAchievementAchievedPercent(InteropHelp.UTF8StringHandle pchName, out float pflPercent);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamUserStats_RequestGlobalStats(int nHistoryDays);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUserStats_GetGlobalStat(InteropHelp.UTF8StringHandle pchStatName, out long pData);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUserStats_GetGlobalStat_(InteropHelp.UTF8StringHandle pchStatName, out double pData);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ISteamUserStats_GetGlobalStatHistory(InteropHelp.UTF8StringHandle pchStatName, [In, Out] long[] pData, uint cubData);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ISteamUserStats_GetGlobalStatHistory_(InteropHelp.UTF8StringHandle pchStatName, [In, Out] double[] pData, uint cubData);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ISteamUtils_GetSecondsSinceAppActive();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ISteamUtils_GetSecondsSinceComputerActive();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern EUniverse ISteamUtils_GetConnectedUniverse();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ISteamUtils_GetServerRealTime();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ISteamUtils_GetIPCountry();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUtils_GetImageSize(int iImage, out uint pnWidth, out uint pnHeight);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUtils_GetImageRGBA(int iImage, [In, Out] byte[] pubDest, int nDestBufferSize);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUtils_GetCSERIPPort(out uint unIP, out ushort usPort);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern byte ISteamUtils_GetCurrentBatteryPower();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ISteamUtils_GetAppID();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamUtils_SetOverlayNotificationPosition(ENotificationPosition eNotificationPosition);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUtils_IsAPICallCompleted(SteamAPICall_t hSteamAPICall, out bool pbFailed);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ESteamAPICallFailure ISteamUtils_GetAPICallFailureReason(SteamAPICall_t hSteamAPICall);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUtils_GetAPICallResult(SteamAPICall_t hSteamAPICall, IntPtr pCallback, int cubCallback, int iCallbackExpected, out bool pbFailed);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamUtils_RunFrame();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ISteamUtils_GetIPCCallCount();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamUtils_SetWarningMessageHook(SteamAPIWarningMessageHook_t pFunction);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUtils_IsOverlayEnabled();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUtils_BOverlayNeedsPresent();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamUtils_CheckFileSignature(InteropHelp.UTF8StringHandle szFileName);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUtils_ShowGamepadTextInput(EGamepadTextInputMode eInputMode, EGamepadTextInputLineMode eLineInputMode, InteropHelp.UTF8StringHandle pchDescription, uint unCharMax, InteropHelp.UTF8StringHandle pchExistingText);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ISteamUtils_GetEnteredGamepadTextLength();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUtils_GetEnteredGamepadTextInput(IntPtr pchText, uint cchText);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ISteamUtils_GetSteamUILanguage();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamUtils_IsSteamRunningInVR();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamUtils_SetOverlayNotificationInset(int nHorizontalInset, int nVerticalInset);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamVideo_GetVideoURL(AppId_t unVideoAppID);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamVideo_IsBroadcasting(out int pnNumViewers);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ISteamGameServerHTTP_CreateHTTPRequest(EHTTPMethod eHTTPRequestMethod, InteropHelp.UTF8StringHandle pchAbsoluteURL);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerHTTP_SetHTTPRequestContextValue(HTTPRequestHandle hRequest, ulong ulContextValue);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerHTTP_SetHTTPRequestNetworkActivityTimeout(HTTPRequestHandle hRequest, uint unTimeoutSeconds);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerHTTP_SetHTTPRequestHeaderValue(HTTPRequestHandle hRequest, InteropHelp.UTF8StringHandle pchHeaderName, InteropHelp.UTF8StringHandle pchHeaderValue);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerHTTP_SetHTTPRequestGetOrPostParameter(HTTPRequestHandle hRequest, InteropHelp.UTF8StringHandle pchParamName, InteropHelp.UTF8StringHandle pchParamValue);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerHTTP_SendHTTPRequest(HTTPRequestHandle hRequest, out SteamAPICall_t pCallHandle);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerHTTP_SendHTTPRequestAndStreamResponse(HTTPRequestHandle hRequest, out SteamAPICall_t pCallHandle);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerHTTP_DeferHTTPRequest(HTTPRequestHandle hRequest);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerHTTP_PrioritizeHTTPRequest(HTTPRequestHandle hRequest);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerHTTP_GetHTTPResponseHeaderSize(HTTPRequestHandle hRequest, InteropHelp.UTF8StringHandle pchHeaderName, out uint unResponseHeaderSize);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerHTTP_GetHTTPResponseHeaderValue(HTTPRequestHandle hRequest, InteropHelp.UTF8StringHandle pchHeaderName, [In, Out] byte[] pHeaderValueBuffer, uint unBufferSize);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerHTTP_GetHTTPResponseBodySize(HTTPRequestHandle hRequest, out uint unBodySize);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerHTTP_GetHTTPResponseBodyData(HTTPRequestHandle hRequest, [In, Out] byte[] pBodyDataBuffer, uint unBufferSize);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerHTTP_GetHTTPStreamingResponseBodyData(HTTPRequestHandle hRequest, uint cOffset, [In, Out] byte[] pBodyDataBuffer, uint unBufferSize);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerHTTP_ReleaseHTTPRequest(HTTPRequestHandle hRequest);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerHTTP_GetHTTPDownloadProgressPct(HTTPRequestHandle hRequest, out float pflPercentOut);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerHTTP_SetHTTPRequestRawPostBody(HTTPRequestHandle hRequest, InteropHelp.UTF8StringHandle pchContentType, [In, Out] byte[] pubBody, uint unBodyLen);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ISteamGameServerHTTP_CreateCookieContainer([MarshalAs(UnmanagedType.I1)] bool bAllowResponsesToModify);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerHTTP_ReleaseCookieContainer(HTTPCookieContainerHandle hCookieContainer);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerHTTP_SetCookie(HTTPCookieContainerHandle hCookieContainer, InteropHelp.UTF8StringHandle pchHost, InteropHelp.UTF8StringHandle pchUrl, InteropHelp.UTF8StringHandle pchCookie);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerHTTP_SetHTTPRequestCookieContainer(HTTPRequestHandle hRequest, HTTPCookieContainerHandle hCookieContainer);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerHTTP_SetHTTPRequestUserAgentInfo(HTTPRequestHandle hRequest, InteropHelp.UTF8StringHandle pchUserAgentInfo);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerHTTP_SetHTTPRequestRequiresVerifiedCertificate(HTTPRequestHandle hRequest, [MarshalAs(UnmanagedType.I1)] bool bRequireVerifiedCertificate);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerHTTP_SetHTTPRequestAbsoluteTimeoutMS(HTTPRequestHandle hRequest, uint unMilliseconds);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerHTTP_GetHTTPRequestWasTimedOut(HTTPRequestHandle hRequest, out bool pbWasTimedOut);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern EResult ISteamGameServerInventory_GetResultStatus(SteamInventoryResult_t resultHandle);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerInventory_GetResultItems(SteamInventoryResult_t resultHandle, [In, Out] SteamItemDetails_t[] pOutItemsArray, ref uint punOutItemsArraySize);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ISteamGameServerInventory_GetResultTimestamp(SteamInventoryResult_t resultHandle);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerInventory_CheckResultSteamID(SteamInventoryResult_t resultHandle, CSteamID steamIDExpected);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamGameServerInventory_DestroyResult(SteamInventoryResult_t resultHandle);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerInventory_GetAllItems(out SteamInventoryResult_t pResultHandle);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerInventory_GetItemsByID(out SteamInventoryResult_t pResultHandle, [In, Out] SteamItemInstanceID_t[] pInstanceIDs, uint unCountInstanceIDs);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerInventory_SerializeResult(SteamInventoryResult_t resultHandle, [In, Out] byte[] pOutBuffer, out uint punOutBufferSize);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerInventory_DeserializeResult(out SteamInventoryResult_t pOutResultHandle, [In, Out] byte[] pBuffer, uint unBufferSize, [MarshalAs(UnmanagedType.I1)] bool bRESERVED_MUST_BE_FALSE);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerInventory_GenerateItems(out SteamInventoryResult_t pResultHandle, [In, Out] SteamItemDef_t[] pArrayItemDefs, [In, Out] uint[] punArrayQuantity, uint unArrayLength);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerInventory_GrantPromoItems(out SteamInventoryResult_t pResultHandle);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerInventory_AddPromoItem(out SteamInventoryResult_t pResultHandle, SteamItemDef_t itemDef);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerInventory_AddPromoItems(out SteamInventoryResult_t pResultHandle, [In, Out] SteamItemDef_t[] pArrayItemDefs, uint unArrayLength);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerInventory_ConsumeItem(out SteamInventoryResult_t pResultHandle, SteamItemInstanceID_t itemConsume, uint unQuantity);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerInventory_ExchangeItems(out SteamInventoryResult_t pResultHandle, [In, Out] SteamItemDef_t[] pArrayGenerate, [In, Out] uint[] punArrayGenerateQuantity, uint unArrayGenerateLength, [In, Out] SteamItemInstanceID_t[] pArrayDestroy, [In, Out] uint[] punArrayDestroyQuantity, uint unArrayDestroyLength);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerInventory_TransferItemQuantity(out SteamInventoryResult_t pResultHandle, SteamItemInstanceID_t itemIdSource, uint unQuantity, SteamItemInstanceID_t itemIdDest);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamGameServerInventory_SendItemDropHeartbeat();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerInventory_TriggerItemDrop(out SteamInventoryResult_t pResultHandle, SteamItemDef_t dropListDefinition);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerInventory_TradeItems(out SteamInventoryResult_t pResultHandle, CSteamID steamIDTradePartner, [In, Out] SteamItemInstanceID_t[] pArrayGive, [In, Out] uint[] pArrayGiveQuantity, uint nArrayGiveLength, [In, Out] SteamItemInstanceID_t[] pArrayGet, [In, Out] uint[] pArrayGetQuantity, uint nArrayGetLength);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerInventory_LoadItemDefinitions();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerInventory_GetItemDefinitionIDs([In, Out] SteamItemDef_t[] pItemDefIDs, out uint punItemDefIDsArraySize);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerInventory_GetItemDefinitionProperty(SteamItemDef_t iDefinition, InteropHelp.UTF8StringHandle pchPropertyName, IntPtr pchValueBuffer, ref uint punValueBufferSize);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerNetworking_SendP2PPacket(CSteamID steamIDRemote, [In, Out] byte[] pubData, uint cubData, EP2PSend eP2PSendType, int nChannel);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerNetworking_IsP2PPacketAvailable(out uint pcubMsgSize, int nChannel);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerNetworking_ReadP2PPacket([In, Out] byte[] pubDest, uint cubDest, out uint pcubMsgSize, out CSteamID psteamIDRemote, int nChannel);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerNetworking_AcceptP2PSessionWithUser(CSteamID steamIDRemote);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerNetworking_CloseP2PSessionWithUser(CSteamID steamIDRemote);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerNetworking_CloseP2PChannelWithUser(CSteamID steamIDRemote, int nChannel);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerNetworking_GetP2PSessionState(CSteamID steamIDRemote, out P2PSessionState_t pConnectionState);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerNetworking_AllowP2PPacketRelay([MarshalAs(UnmanagedType.I1)] bool bAllow);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ISteamGameServerNetworking_CreateListenSocket(int nVirtualP2PPort, uint nIP, ushort nPort, [MarshalAs(UnmanagedType.I1)] bool bAllowUseOfPacketRelay);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ISteamGameServerNetworking_CreateP2PConnectionSocket(CSteamID steamIDTarget, int nVirtualPort, int nTimeoutSec, [MarshalAs(UnmanagedType.I1)] bool bAllowUseOfPacketRelay);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ISteamGameServerNetworking_CreateConnectionSocket(uint nIP, ushort nPort, int nTimeoutSec);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerNetworking_DestroySocket(SNetSocket_t hSocket, [MarshalAs(UnmanagedType.I1)] bool bNotifyRemoteEnd);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerNetworking_DestroyListenSocket(SNetListenSocket_t hSocket, [MarshalAs(UnmanagedType.I1)] bool bNotifyRemoteEnd);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerNetworking_SendDataOnSocket(SNetSocket_t hSocket, IntPtr pubData, uint cubData, [MarshalAs(UnmanagedType.I1)] bool bReliable);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerNetworking_IsDataAvailableOnSocket(SNetSocket_t hSocket, out uint pcubMsgSize);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerNetworking_RetrieveDataFromSocket(SNetSocket_t hSocket, IntPtr pubDest, uint cubDest, out uint pcubMsgSize);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerNetworking_IsDataAvailable(SNetListenSocket_t hListenSocket, out uint pcubMsgSize, out SNetSocket_t phSocket);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerNetworking_RetrieveData(SNetListenSocket_t hListenSocket, IntPtr pubDest, uint cubDest, out uint pcubMsgSize, out SNetSocket_t phSocket);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerNetworking_GetSocketInfo(SNetSocket_t hSocket, out CSteamID pSteamIDRemote, out int peSocketStatus, out uint punIPRemote, out ushort punPortRemote);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerNetworking_GetListenSocketInfo(SNetListenSocket_t hListenSocket, out uint pnIP, out ushort pnPort);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ESNetSocketConnectionType ISteamGameServerNetworking_GetSocketConnectionType(SNetSocket_t hSocket);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ISteamGameServerNetworking_GetMaxPacketSize(SNetSocket_t hSocket);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamGameServerUGC_CreateQueryUserUGCRequest(AccountID_t unAccountID, EUserUGCList eListType, EUGCMatchingUGCType eMatchingUGCType, EUserUGCListSortOrder eSortOrder, AppId_t nCreatorAppID, AppId_t nConsumerAppID, uint unPage);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamGameServerUGC_CreateQueryAllUGCRequest(EUGCQuery eQueryType, EUGCMatchingUGCType eMatchingeMatchingUGCTypeFileType, AppId_t nCreatorAppID, AppId_t nConsumerAppID, uint unPage);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamGameServerUGC_CreateQueryUGCDetailsRequest([In, Out] PublishedFileId_t[] pvecPublishedFileID, uint unNumPublishedFileIDs);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamGameServerUGC_SendQueryUGCRequest(UGCQueryHandle_t handle);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerUGC_GetQueryUGCResult(UGCQueryHandle_t handle, uint index, out SteamUGCDetails_t pDetails);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerUGC_GetQueryUGCPreviewURL(UGCQueryHandle_t handle, uint index, IntPtr pchURL, uint cchURLSize);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerUGC_GetQueryUGCMetadata(UGCQueryHandle_t handle, uint index, IntPtr pchMetadata, uint cchMetadatasize);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerUGC_GetQueryUGCChildren(UGCQueryHandle_t handle, uint index, [In, Out] PublishedFileId_t[] pvecPublishedFileID, uint cMaxEntries);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerUGC_GetQueryUGCStatistic(UGCQueryHandle_t handle, uint index, EItemStatistic eStatType, out uint pStatValue);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ISteamGameServerUGC_GetQueryUGCNumAdditionalPreviews(UGCQueryHandle_t handle, uint index);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerUGC_GetQueryUGCAdditionalPreview(UGCQueryHandle_t handle, uint index, uint previewIndex, IntPtr pchURLOrVideoID, uint cchURLSize, out bool pbIsImage);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ISteamGameServerUGC_GetQueryUGCNumKeyValueTags(UGCQueryHandle_t handle, uint index);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerUGC_GetQueryUGCKeyValueTag(UGCQueryHandle_t handle, uint index, uint keyValueTagIndex, IntPtr pchKey, uint cchKeySize, IntPtr pchValue, uint cchValueSize);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerUGC_ReleaseQueryUGCRequest(UGCQueryHandle_t handle);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerUGC_AddRequiredTag(UGCQueryHandle_t handle, InteropHelp.UTF8StringHandle pTagName);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerUGC_AddExcludedTag(UGCQueryHandle_t handle, InteropHelp.UTF8StringHandle pTagName);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerUGC_SetReturnKeyValueTags(UGCQueryHandle_t handle, [MarshalAs(UnmanagedType.I1)] bool bReturnKeyValueTags);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerUGC_SetReturnLongDescription(UGCQueryHandle_t handle, [MarshalAs(UnmanagedType.I1)] bool bReturnLongDescription);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerUGC_SetReturnMetadata(UGCQueryHandle_t handle, [MarshalAs(UnmanagedType.I1)] bool bReturnMetadata);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerUGC_SetReturnChildren(UGCQueryHandle_t handle, [MarshalAs(UnmanagedType.I1)] bool bReturnChildren);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerUGC_SetReturnAdditionalPreviews(UGCQueryHandle_t handle, [MarshalAs(UnmanagedType.I1)] bool bReturnAdditionalPreviews);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerUGC_SetReturnTotalOnly(UGCQueryHandle_t handle, [MarshalAs(UnmanagedType.I1)] bool bReturnTotalOnly);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerUGC_SetLanguage(UGCQueryHandle_t handle, InteropHelp.UTF8StringHandle pchLanguage);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerUGC_SetAllowCachedResponse(UGCQueryHandle_t handle, uint unMaxAgeSeconds);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerUGC_SetCloudFileNameFilter(UGCQueryHandle_t handle, InteropHelp.UTF8StringHandle pMatchCloudFileName);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerUGC_SetMatchAnyTag(UGCQueryHandle_t handle, [MarshalAs(UnmanagedType.I1)] bool bMatchAnyTag);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerUGC_SetSearchText(UGCQueryHandle_t handle, InteropHelp.UTF8StringHandle pSearchText);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerUGC_SetRankedByTrendDays(UGCQueryHandle_t handle, uint unDays);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerUGC_AddRequiredKeyValueTag(UGCQueryHandle_t handle, InteropHelp.UTF8StringHandle pKey, InteropHelp.UTF8StringHandle pValue);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamGameServerUGC_RequestUGCDetails(PublishedFileId_t nPublishedFileID, uint unMaxAgeSeconds);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamGameServerUGC_CreateItem(AppId_t nConsumerAppId, EWorkshopFileType eFileType);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamGameServerUGC_StartItemUpdate(AppId_t nConsumerAppId, PublishedFileId_t nPublishedFileID);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerUGC_SetItemTitle(UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pchTitle);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerUGC_SetItemDescription(UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pchDescription);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerUGC_SetItemUpdateLanguage(UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pchLanguage);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerUGC_SetItemMetadata(UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pchMetaData);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerUGC_SetItemVisibility(UGCUpdateHandle_t handle, ERemoteStoragePublishedFileVisibility eVisibility);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerUGC_SetItemTags(UGCUpdateHandle_t updateHandle, IntPtr pTags);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerUGC_SetItemContent(UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pszContentFolder);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerUGC_SetItemPreview(UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pszPreviewFile);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerUGC_RemoveItemKeyValueTags(UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pchKey);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerUGC_AddItemKeyValueTag(UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pchKey, InteropHelp.UTF8StringHandle pchValue);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamGameServerUGC_SubmitItemUpdate(UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pchChangeNote);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern EItemUpdateStatus ISteamGameServerUGC_GetItemUpdateProgress(UGCUpdateHandle_t handle, out ulong punBytesProcessed, out ulong punBytesTotal);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamGameServerUGC_SetUserItemVote(PublishedFileId_t nPublishedFileID, [MarshalAs(UnmanagedType.I1)] bool bVoteUp);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamGameServerUGC_GetUserItemVote(PublishedFileId_t nPublishedFileID);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamGameServerUGC_AddItemToFavorites(AppId_t nAppId, PublishedFileId_t nPublishedFileID);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamGameServerUGC_RemoveItemFromFavorites(AppId_t nAppId, PublishedFileId_t nPublishedFileID);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamGameServerUGC_SubscribeItem(PublishedFileId_t nPublishedFileID);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamGameServerUGC_UnsubscribeItem(PublishedFileId_t nPublishedFileID);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ISteamGameServerUGC_GetNumSubscribedItems();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ISteamGameServerUGC_GetSubscribedItems([In, Out] PublishedFileId_t[] pvecPublishedFileID, uint cMaxEntries);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ISteamGameServerUGC_GetItemState(PublishedFileId_t nPublishedFileID);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerUGC_GetItemInstallInfo(PublishedFileId_t nPublishedFileID, out ulong punSizeOnDisk, IntPtr pchFolder, uint cchFolderSize, out uint punTimeStamp);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerUGC_GetItemDownloadInfo(PublishedFileId_t nPublishedFileID, out ulong punBytesDownloaded, out ulong punBytesTotal);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerUGC_DownloadItem(PublishedFileId_t nPublishedFileID, [MarshalAs(UnmanagedType.I1)] bool bHighPriority);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ISteamGameServerUtils_GetSecondsSinceAppActive();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ISteamGameServerUtils_GetSecondsSinceComputerActive();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern EUniverse ISteamGameServerUtils_GetConnectedUniverse();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ISteamGameServerUtils_GetServerRealTime();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ISteamGameServerUtils_GetIPCountry();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerUtils_GetImageSize(int iImage, out uint pnWidth, out uint pnHeight);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerUtils_GetImageRGBA(int iImage, [In, Out] byte[] pubDest, int nDestBufferSize);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerUtils_GetCSERIPPort(out uint unIP, out ushort usPort);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern byte ISteamGameServerUtils_GetCurrentBatteryPower();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ISteamGameServerUtils_GetAppID();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamGameServerUtils_SetOverlayNotificationPosition(ENotificationPosition eNotificationPosition);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerUtils_IsAPICallCompleted(SteamAPICall_t hSteamAPICall, out bool pbFailed);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ESteamAPICallFailure ISteamGameServerUtils_GetAPICallFailureReason(SteamAPICall_t hSteamAPICall);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerUtils_GetAPICallResult(SteamAPICall_t hSteamAPICall, IntPtr pCallback, int cubCallback, int iCallbackExpected, out bool pbFailed);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamGameServerUtils_RunFrame();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ISteamGameServerUtils_GetIPCCallCount();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamGameServerUtils_SetWarningMessageHook(SteamAPIWarningMessageHook_t pFunction);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerUtils_IsOverlayEnabled();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerUtils_BOverlayNeedsPresent();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ISteamGameServerUtils_CheckFileSignature(InteropHelp.UTF8StringHandle szFileName);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerUtils_ShowGamepadTextInput(EGamepadTextInputMode eInputMode, EGamepadTextInputLineMode eLineInputMode, InteropHelp.UTF8StringHandle pchDescription, uint unCharMax, InteropHelp.UTF8StringHandle pchExistingText);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ISteamGameServerUtils_GetEnteredGamepadTextLength();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerUtils_GetEnteredGamepadTextInput(IntPtr pchText, uint cchText);

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ISteamGameServerUtils_GetSteamUILanguage();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ISteamGameServerUtils_IsSteamRunningInVR();

    [DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
    public static extern void ISteamGameServerUtils_SetOverlayNotificationInset(int nHorizontalInset, int nVerticalInset);
  }
}
