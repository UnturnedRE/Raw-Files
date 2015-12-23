// Decompiled with JetBrains decompiler
// Type: SDG.SteamworksProvider.Services.Multiplayer.Client.SteamworksClientMultiplayerService
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using SDG.Provider.Services;
using SDG.Provider.Services.Community;
using SDG.Provider.Services.Multiplayer;
using SDG.Provider.Services.Multiplayer.Client;
using SDG.SteamworksProvider.Services.Community;
using Steamworks;
using System.IO;

namespace SDG.SteamworksProvider.Services.Multiplayer.Client
{
  public class SteamworksClientMultiplayerService : Service, IService, IClientMultiplayerService
  {
    private byte[] buffer;
    private static Callback<P2PSessionRequest_t> p2pSessionRequest;

    public IServerInfo serverInfo { get; protected set; }

    public bool isConnected { get; protected set; }

    public bool isAttempting { get; protected set; }

    public MemoryStream stream { get; protected set; }

    public BinaryReader reader { get; protected set; }

    public BinaryWriter writer { get; protected set; }

    public SteamworksClientMultiplayerService()
    {
      this.buffer = new byte[1024];
      this.stream = new MemoryStream(this.buffer);
      this.reader = new BinaryReader((Stream) this.stream);
      this.writer = new BinaryWriter((Stream) this.stream);
      SteamworksClientMultiplayerService.p2pSessionRequest = Callback<P2PSessionRequest_t>.Create(new Callback<P2PSessionRequest_t>.DispatchDelegate(this.onP2PSessionRequest));
    }

    public void connect(IServerInfo newServerInfo)
    {
      this.serverInfo = newServerInfo;
    }

    public void disconnect()
    {
    }

    public bool read(out ICommunityEntity entity, byte[] data, out ulong length, int channel)
    {
      entity = (ICommunityEntity) SteamworksCommunityEntity.INVALID;
      length = 0UL;
      uint pcubMsgSize;
      CSteamID psteamIDRemote;
      if (!SteamNetworking.IsP2PPacketAvailable(out pcubMsgSize, channel) || (long) pcubMsgSize > (long) data.Length || !SteamNetworking.ReadP2PPacket(data, pcubMsgSize, out pcubMsgSize, out psteamIDRemote, channel))
        return false;
      entity = (ICommunityEntity) new SteamworksCommunityEntity(psteamIDRemote);
      length = (ulong) pcubMsgSize;
      return true;
    }

    public void write(ICommunityEntity entity, byte[] data, ulong length)
    {
    }

    public void write(ICommunityEntity entity, byte[] data, ulong length, ESendMethod method, int channel)
    {
    }

    private void onP2PSessionRequest(P2PSessionRequest_t callback)
    {
      SteamNetworking.AcceptP2PSessionWithUser(callback.m_steamIDRemote);
    }
  }
}
