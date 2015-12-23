// Decompiled with JetBrains decompiler
// Type: SDG.SteamworksProvider.Services.Multiplayer.Server.SteamworksServerMultiplayerService
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using SDG.Provider.Services;
using SDG.Provider.Services.Community;
using SDG.Provider.Services.Multiplayer;
using SDG.Provider.Services.Multiplayer.Server;
using SDG.SteamworksProvider;
using SDG.SteamworksProvider.Services.Community;
using Steamworks;
using System;
using System.IO;

namespace SDG.SteamworksProvider.Services.Multiplayer.Server
{
  public class SteamworksServerMultiplayerService : Service, IService, IServerMultiplayerService
  {
    private byte[] buffer;
    private SteamworksAppInfo appInfo;
    private static Callback<P2PSessionRequest_t> p2pSessionRequest;

    public IServerInfo serverInfo { get; protected set; }

    public bool isHosting { get; protected set; }

    public MemoryStream stream { get; protected set; }

    public BinaryReader reader { get; protected set; }

    public BinaryWriter writer { get; protected set; }

    public SteamworksServerMultiplayerService(SteamworksAppInfo newAppInfo)
    {
      this.appInfo = newAppInfo;
      this.buffer = new byte[1024];
      this.stream = new MemoryStream(this.buffer);
      this.reader = new BinaryReader((Stream) this.stream);
      this.writer = new BinaryWriter((Stream) this.stream);
      SteamworksServerMultiplayerService.p2pSessionRequest = Callback<P2PSessionRequest_t>.CreateGameServer(new Callback<P2PSessionRequest_t>.DispatchDelegate(this.onP2PSessionRequest));
    }

    public void open(uint ip, ushort port, ESecurityMode security)
    {
      if (this.isHosting)
        return;
      EServerMode eServerMode = EServerMode.eServerModeInvalid;
      switch (security)
      {
        case ESecurityMode.LAN:
          eServerMode = EServerMode.eServerModeNoAuthentication;
          break;
        case ESecurityMode.SECURE:
          eServerMode = EServerMode.eServerModeAuthenticationAndSecure;
          break;
        case ESecurityMode.INSECURE:
          eServerMode = EServerMode.eServerModeAuthentication;
          break;
      }
      if (!GameServer.Init(ip, (ushort) ((uint) port + 2U), port, (ushort) ((uint) port + 1U), eServerMode, this.appInfo.version))
        throw new Exception("GameServer API initialization failed!");
      SteamGameServer.SetDedicatedServer(this.appInfo.isDedicated);
      SteamGameServer.SetGameDescription(this.appInfo.name);
      SteamGameServer.SetProduct(this.appInfo.name);
      SteamGameServer.SetModDir(this.appInfo.name);
      SteamGameServer.LogOnAnonymous();
      SteamGameServer.EnableHeartbeats(true);
      this.isHosting = true;
    }

    public void close()
    {
      if (!this.isHosting)
        return;
      SteamGameServer.EnableHeartbeats(false);
      SteamGameServer.LogOff();
      GameServer.Shutdown();
      this.isHosting = false;
    }

    public bool read(out ICommunityEntity entity, byte[] data, out ulong length, int channel)
    {
      entity = (ICommunityEntity) SteamworksCommunityEntity.INVALID;
      length = 0UL;
      uint pcubMsgSize;
      CSteamID psteamIDRemote;
      if (!SteamGameServerNetworking.IsP2PPacketAvailable(out pcubMsgSize, channel) || (long) pcubMsgSize > (long) data.Length || !SteamGameServerNetworking.ReadP2PPacket(data, pcubMsgSize, out pcubMsgSize, out psteamIDRemote, channel))
        return false;
      entity = (ICommunityEntity) new SteamworksCommunityEntity(psteamIDRemote);
      length = (ulong) pcubMsgSize;
      return true;
    }

    public void write(ICommunityEntity entity, byte[] data, ulong length)
    {
      SteamGameServerNetworking.SendP2PPacket(((SteamworksCommunityEntity) entity).steamID, data, (uint) length, EP2PSend.k_EP2PSendUnreliable, 0);
    }

    public void write(ICommunityEntity entity, byte[] data, ulong length, ESendMethod method, int channel)
    {
      CSteamID steamId = ((SteamworksCommunityEntity) entity).steamID;
      switch (method)
      {
        case ESendMethod.RELIABLE:
          SteamGameServerNetworking.SendP2PPacket(steamId, data, (uint) length, EP2PSend.k_EP2PSendReliableWithBuffering, channel);
          break;
        case ESendMethod.RELIABLE_NODELAY:
          SteamGameServerNetworking.SendP2PPacket(steamId, data, (uint) length, EP2PSend.k_EP2PSendReliable, channel);
          break;
        case ESendMethod.UNRELIABLE:
          SteamGameServerNetworking.SendP2PPacket(steamId, data, (uint) length, EP2PSend.k_EP2PSendUnreliable, channel);
          break;
        case ESendMethod.UNRELIABLE_NODELAY:
          SteamGameServerNetworking.SendP2PPacket(steamId, data, (uint) length, EP2PSend.k_EP2PSendUnreliableNoDelay, channel);
          break;
      }
    }

    private void onP2PSessionRequest(P2PSessionRequest_t callback)
    {
      SteamGameServerNetworking.AcceptP2PSessionWithUser(callback.m_steamIDRemote);
    }
  }
}
