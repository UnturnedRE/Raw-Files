// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.Provider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using SDG.Provider;
using SDG.Provider.Services.Community;
using SDG.Provider.Services.Multiplayer;
using SDG.SteamworksProvider;
using SDG.SteamworksProvider.Services.Community;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;

namespace SDG.Unturned
{
  public class Provider : MonoBehaviour
  {
    public static readonly AppId_t APP_ID = new AppId_t(304930U);
    public static readonly AppId_t PRO_ID = new AppId_t(306460U);
    public static readonly string APP_VERSION = "3.13.9.1";
    public static readonly string APP_NAME = "Unturned";
    public static readonly string APP_AUTHOR = "Nelson Sexton";
    public static readonly int CLIENT_TIMEOUT = 30;
    public static readonly int SERVER_TIMEOUT = 30;
    public static readonly int PENDING_TIMEOUT = 30;
    private static readonly float CHECKRATE = 1f;
    private static int _channels = 1;
    private static byte[] buffer = new byte[Block.BUFFER_SIZE];
    private static HAuthTicket ticketHandle = HAuthTicket.Invalid;
    public static readonly float EPSILON = 0.01f;
    public static readonly float UPDATE_TIME = 0.15f;
    public static readonly float UPDATE_DISTANCE = 0.01f;
    public static readonly uint UPDATES = 1U;
    public static readonly float LERP = 3f;
    private static string _language;
    private static string _path;
    public static Local localization;
    private static uint _bytesSent;
    private static uint _bytesReceived;
    private static uint _packetsSent;
    private static uint _packetsReceived;
    private static SteamServerInfo _currentServerInfo;
    private static CSteamID _server;
    private static CSteamID _client;
    private static CSteamID _user;
    private static byte[] _clientHash;
    private static string _clientName;
    private static List<SteamPlayer> _clients;
    public static List<SteamPending> pending;
    private static bool _isServer;
    private static bool _isClient;
    private static bool _isPro;
    private static bool _isConnected;
    private static bool isTesting;
    private static bool isLoadingUGC;
    public static bool isLoadingInventory;
    public static ESteamConnectionFailureInfo _connectionFailureInfo;
    private static string _connectionFailureReason;
    private static uint _connectionFailureDuration;
    private static List<SteamChannel> _receivers;
    public static float timeout;
    public static Provider.ServerConnected onServerConnected;
    public static Provider.ServerDisconnected onServerDisconnected;
    public static Provider.ServerHosted onServerHosted;
    public static Provider.ServerShutdown onServerShutdown;
    private static Callback<GSPolicyResponse_t> gsPolicyResponse;
    private static Callback<P2PSessionConnectFail_t> p2pSessionConnectFail;
    private static Callback<ValidateAuthTicketResponse_t> validateAuthTicketResponse;
    private static byte _maxPlayers;
    private static string _serverName;
    public static uint ip;
    public static ushort port;
    private static byte[] _serverPasswordHash;
    private static string _serverPassword;
    public static string map;
    public static bool isPvP;
    public static bool filterName;
    public static EGameMode mode;
    public static ECameraMode camera;
    private static uint favoriteIP;
    private static ushort favoritePort;
    private static bool _isFavorited;
    public static Provider.ClientConnected onClientConnected;
    public static Provider.ClientDisconnected onClientDisconnected;
    public static Provider.EnemyConnected onEnemyConnected;
    public static Provider.EnemyDisconnected onEnemyDisconnected;
    private static Callback<PersonaStateChange_t> personaStateChange;
    private static Callback<GameServerChangeRequested_t> gameServerChangeRequested;
    private static Callback<GameRichPresenceJoinRequested_t> gameRichPresenceJoinRequested;
    private static float lastCheck;
    private static float lastNet;
    private static float lastPing;
    private static float offsetNet;
    private static float[] pings;
    private static float _ping;
    private static Provider steam;
    private static bool _isInitialized;
    private static uint timeOffset;
    private static uint _time;
    private static SteamAPIWarningMessageHook_t apiWarningMessageHook;

    public static string language
    {
      get
      {
        return Provider._language;
      }
    }

    public static string path
    {
      get
      {
        return Provider._path;
      }
    }

    public static uint bytesSent
    {
      get
      {
        return Provider._bytesSent;
      }
    }

    public static uint bytesReceived
    {
      get
      {
        return Provider._bytesReceived;
      }
    }

    public static uint packetsSent
    {
      get
      {
        return Provider._packetsSent;
      }
    }

    public static uint packetsReceived
    {
      get
      {
        return Provider._packetsReceived;
      }
    }

    public static SteamServerInfo currentServerInfo
    {
      get
      {
        return Provider._currentServerInfo;
      }
    }

    public static CSteamID server
    {
      get
      {
        return Provider._server;
      }
    }

    public static CSteamID client
    {
      get
      {
        return Provider._client;
      }
    }

    public static CSteamID user
    {
      get
      {
        return Provider._user;
      }
    }

    public static byte[] clientHash
    {
      get
      {
        return Provider._clientHash;
      }
    }

    public static string clientName
    {
      get
      {
        return Provider._clientName;
      }
    }

    public static List<SteamPlayer> clients
    {
      get
      {
        return Provider._clients;
      }
    }

    public static bool isServer
    {
      get
      {
        return Provider._isServer;
      }
    }

    public static bool isClient
    {
      get
      {
        return Provider._isClient;
      }
    }

    public static bool isPro
    {
      get
      {
        return Provider._isPro;
      }
    }

    public static bool isConnected
    {
      get
      {
        return Provider._isConnected;
      }
    }

    public static bool isLoading
    {
      get
      {
        return Provider.isLoadingUGC;
      }
    }

    public static int channels
    {
      get
      {
        return Provider._channels;
      }
    }

    public static ESteamConnectionFailureInfo connectionFailureInfo
    {
      get
      {
        return Provider._connectionFailureInfo;
      }
    }

    public static string connectionFailureReason
    {
      get
      {
        return Provider._connectionFailureReason;
      }
    }

    public static uint connectionFailureDuration
    {
      get
      {
        return Provider._connectionFailureDuration;
      }
    }

    public static List<SteamChannel> receivers
    {
      get
      {
        return Provider._receivers;
      }
    }

    public static byte maxPlayers
    {
      get
      {
        return Provider._maxPlayers;
      }
      set
      {
        Provider._maxPlayers = value;
        if (!Provider.isServer)
          return;
        SteamGameServer.SetMaxPlayerCount((int) Provider.maxPlayers);
      }
    }

    public static string serverName
    {
      get
      {
        return Provider._serverName;
      }
      set
      {
        Provider._serverName = value;
        if (Dedicator.commandWindow != null)
          Dedicator.commandWindow.title = Provider.serverName;
        if (!Provider.isServer)
          return;
        SteamGameServer.SetServerName(Provider.serverName);
      }
    }

    public static string serverID
    {
      get
      {
        return Dedicator.serverID;
      }
      set
      {
        Dedicator.serverID = value;
      }
    }

    public static byte[] serverPasswordHash
    {
      get
      {
        return Provider._serverPasswordHash;
      }
    }

    public static string serverPassword
    {
      get
      {
        return Provider._serverPassword;
      }
      set
      {
        Provider._serverPassword = value;
        Provider._serverPasswordHash = Hash.SHA1(Provider.serverPassword);
        if (!Provider.isServer)
          return;
        SteamGameServer.SetPasswordProtected(Provider.serverPassword != string.Empty);
      }
    }

    public static bool isFavorited
    {
      get
      {
        return Provider._isFavorited;
      }
    }

    public static float net
    {
      get
      {
        return Provider.offsetNet + Time.realtimeSinceStartup - Provider.lastNet;
      }
    }

    public static float ping
    {
      get
      {
        return Provider._ping;
      }
    }

    public static IProvider provider { get; protected set; }

    public static bool isInitialized
    {
      get
      {
        return Provider._isInitialized;
      }
    }

    public static uint time
    {
      get
      {
        return Provider._time + (uint) ((double) Time.realtimeSinceStartup - (double) Provider.timeOffset);
      }
      set
      {
        Provider._time = value;
        Provider.timeOffset = (uint) Time.realtimeSinceStartup;
      }
    }

    public static void resetConnectionFailure()
    {
      Provider._connectionFailureInfo = ESteamConnectionFailureInfo.NONE;
      Provider._connectionFailureReason = string.Empty;
      Provider._connectionFailureDuration = 0U;
    }

    public static void openChannel(SteamChannel receiver)
    {
      if (Provider.receivers == null)
      {
        Provider.resetChannels();
      }
      else
      {
        Provider.receivers.Add(receiver);
        ++Provider._channels;
      }
    }

    public static void closeChannel(SteamChannel receiver)
    {
      for (int index = 0; index < Provider.receivers.Count; ++index)
      {
        if (Provider.receivers[index].id == receiver.id)
        {
          Provider.receivers.RemoveAt(index);
          break;
        }
      }
    }

    private static void addPlayer(SteamPlayerID playerID, Vector3 point, byte angle, bool isPro, bool isAdmin, int channel, byte face, byte hair, byte beard, Color skin, Color color, bool hand, int shirtItem, int pantsItem, int hatItem, int backpackItem, int vestItem, int maskItem, int glassesItem, int[] skinItems, EPlayerSpeciality speciality)
    {
      if (!Dedicator.isDedicated && playerID.steamID != Provider.client)
        SteamFriends.SetPlayedWith(playerID.steamID);
      Transform transform = ((GameObject) UnityEngine.Object.Instantiate(Resources.Load("Characters/Player"), point, Quaternion.Euler(0.0f, (float) ((int) angle * 2), 0.0f))).transform;
      Provider.clients.Add(new SteamPlayer(playerID, transform, isPro, isAdmin, channel, face, hair, beard, skin, color, hand, shirtItem, pantsItem, hatItem, backpackItem, vestItem, maskItem, glassesItem, skinItems, speciality));
      if (Provider.onEnemyConnected == null)
        return;
      Provider.onEnemyConnected(Provider.clients[Provider.clients.Count - 1]);
    }

    private static void removePlayer(byte index)
    {
      if ((int) index < 0 || (int) index >= Provider.clients.Count)
      {
        UnityEngine.Debug.LogError((object) ("Failed to find player: " + (object) index));
      }
      else
      {
        Provider.steam.StartCoroutine("close", (object) Provider.clients[(int) index].playerID.steamID);
        if (Provider.onEnemyDisconnected != null)
          Provider.onEnemyDisconnected(Provider.clients[(int) index]);
        UnityEngine.Object.Destroy((UnityEngine.Object) Provider.clients[(int) index].model.gameObject);
        Provider.clients.RemoveAt((int) index);
      }
    }

    private static bool isInstant(ESteamPacket packet)
    {
      return packet == ESteamPacket.UPDATE_RELIABLE_INSTANT || packet == ESteamPacket.UPDATE_UNRELIABLE_INSTANT || (packet == ESteamPacket.UPDATE_RELIABLE_INSTANT || packet == ESteamPacket.UPDATE_UNRELIABLE_INSTANT) || (packet == ESteamPacket.UPDATE_RELIABLE_CHUNK_INSTANT || packet == ESteamPacket.UPDATE_UNRELIABLE_CHUNK_INSTANT);
    }

    private static bool isUnreliable(ESteamPacket packet)
    {
      return packet == ESteamPacket.UPDATE_UNRELIABLE_BUFFER || packet == ESteamPacket.UPDATE_UNRELIABLE_INSTANT || (packet == ESteamPacket.UPDATE_UNRELIABLE_CHUNK_BUFFER || packet == ESteamPacket.UPDATE_UNRELIABLE_CHUNK_INSTANT) || (packet == ESteamPacket.UPDATE_VOICE || packet == ESteamPacket.TICK || packet == ESteamPacket.TIME);
    }

    public static bool isChunk(ESteamPacket packet)
    {
      return packet == ESteamPacket.UPDATE_UNRELIABLE_CHUNK_BUFFER || packet == ESteamPacket.UPDATE_RELIABLE_CHUNK_BUFFER || (packet == ESteamPacket.UPDATE_UNRELIABLE_CHUNK_INSTANT || packet == ESteamPacket.UPDATE_RELIABLE_CHUNK_INSTANT);
    }

    private static bool isUpdate(ESteamPacket packet)
    {
      return packet == ESteamPacket.UPDATE_RELIABLE_BUFFER || packet == ESteamPacket.UPDATE_UNRELIABLE_BUFFER || (packet == ESteamPacket.UPDATE_RELIABLE_INSTANT || packet == ESteamPacket.UPDATE_UNRELIABLE_INSTANT) || (packet == ESteamPacket.UPDATE_RELIABLE_CHUNK_BUFFER || packet == ESteamPacket.UPDATE_UNRELIABLE_CHUNK_BUFFER || (packet == ESteamPacket.UPDATE_RELIABLE_CHUNK_INSTANT || packet == ESteamPacket.UPDATE_UNRELIABLE_CHUNK_INSTANT)) || packet == ESteamPacket.UPDATE_VOICE;
    }

    private static void resetChannels()
    {
      Provider._bytesSent = 0U;
      Provider._bytesReceived = 0U;
      Provider._packetsSent = 0U;
      Provider._packetsReceived = 0U;
      Provider._channels = 1;
      Provider._receivers = new List<SteamChannel>();
      foreach (SteamChannel receiver in UnityEngine.Object.FindObjectsOfType<SteamChannel>())
        Provider.openChannel(receiver);
      Provider._clients = new List<SteamPlayer>();
      Provider.pending = new List<SteamPending>();
    }

    private static void onLevelLoaded(int level)
    {
      if (level <= Level.SETUP)
        return;
      Provider.isLoadingUGC = false;
      if (!Provider.isConnected)
        return;
      if (Provider.isServer)
      {
        if (!Provider.isClient)
          return;
        SteamPlayerID playerID = new SteamPlayerID(Provider.client, Characters.selected, Provider.clientName, Characters.active.name, Characters.active.nick, Characters.active.group);
        Vector3 zero = Vector3.zero;
        Vector3 point;
        byte angle;
        if (PlayerSavedata.fileExists(playerID, "/Player/Player.dat") && Level.info.type == ELevelType.SURVIVAL)
        {
          Block block = PlayerSavedata.readBlock(playerID, "/Player/Player.dat", (byte) 1);
          point = LevelGround.checkSafe(block.readSingleVector3() + new Vector3(0.0f, 0.5f, 0.0f));
          angle = block.readByte();
        }
        else
        {
          PlayerSpawnpoint spawn = LevelPlayers.getSpawn();
          point = spawn.point + new Vector3(0.0f, 0.5f, 0.0f);
          angle = (byte) ((double) spawn.angle / 2.0);
        }
        float height = LevelGround.getHeight(point);
        if ((double) point.y < (double) height + 0.5)
          point.y = height + 0.5f;
        int inventoryItem1 = Provider.provider.economyService.getInventoryItem(Characters.active.packageShirt);
        int inventoryItem2 = Provider.provider.economyService.getInventoryItem(Characters.active.packagePants);
        int inventoryItem3 = Provider.provider.economyService.getInventoryItem(Characters.active.packageHat);
        int inventoryItem4 = Provider.provider.economyService.getInventoryItem(Characters.active.packageBackpack);
        int inventoryItem5 = Provider.provider.economyService.getInventoryItem(Characters.active.packageVest);
        int inventoryItem6 = Provider.provider.economyService.getInventoryItem(Characters.active.packageMask);
        int inventoryItem7 = Provider.provider.economyService.getInventoryItem(Characters.active.packageGlasses);
        int[] skinItems = new int[Characters.packageSkins.Count];
        for (int index = 0; index < skinItems.Length; ++index)
          skinItems[index] = Provider.provider.economyService.getInventoryItem(Characters.packageSkins[index]);
        Provider.addPlayer(playerID, point, angle, Provider.isPro, true, Provider.channels, Characters.active.face, Characters.active.hair, Characters.active.beard, Characters.active.skin, Characters.active.color, Characters.active.hand, inventoryItem1, inventoryItem2, inventoryItem3, inventoryItem4, inventoryItem5, inventoryItem6, inventoryItem7, skinItems, Characters.active.speciality);
        if (Provider.onServerConnected == null)
          return;
        Provider.onServerConnected(playerID.steamID);
      }
      else
      {
        byte num = (byte) 1;
        int size;
        Provider.send(Provider.server, ESteamPacket.CONNECT, SteamPacker.getBytes(0, out size, (object) 2, (object) Characters.selected, (object) Provider.clientName, (object) Characters.active.name, (object) Provider._serverPasswordHash, (object) Level.hash, (object) ReadWrite.appOut(), (object) num, (object) Provider.APP_VERSION, (object) (bool) (Provider.isPro ? 1 : 0), (object) (float) ((double) Provider.currentServerInfo.ping / 1000.0), (object) Characters.active.nick, (object) Characters.active.group, (object) Characters.active.face, (object) Characters.active.hair, (object) Characters.active.beard, (object) Characters.active.skin, (object) Characters.active.color, (object) (bool) (Characters.active.hand ? 1 : 0), (object) Characters.active.packageShirt, (object) Characters.active.packagePants, (object) Characters.active.packageHat, (object) Characters.active.packageBackpack, (object) Characters.active.packageVest, (object) Characters.active.packageMask, (object) Characters.active.packageGlasses, (object) Characters.packageSkins.ToArray(), (object) (byte) Characters.active.speciality), size, 0);
      }
    }

    public static void connect(SteamServerInfo info, string password)
    {
      if (Provider.isConnected)
        return;
      Provider._currentServerInfo = info;
      Provider._isConnected = true;
      Provider.map = info.map;
      Provider.isPvP = info.isPvP;
      Provider.mode = info.mode;
      Provider.camera = info.camera;
      Provider.maxPlayers = (byte) info.maxPlayers;
      Provider.resetChannels();
      Provider._server = info.steamID;
      Provider._serverPassword = password;
      Provider._serverPasswordHash = Hash.SHA1(password);
      Provider._isClient = true;
      Provider.lastNet = Time.realtimeSinceStartup;
      Provider.offsetNet = 0.0f;
      Provider.pings = new float[4];
      Provider.lag((float) info.ping / 1000f);
      Provider.isTesting = true;
      Provider.isLoadingUGC = true;
      LoadingUI.updateScene();
      Provider.send(Provider.server, ESteamPacket.WORKSHOP, new byte[1]
      {
        (byte) 1
      }, 1, 0);
      List<SteamItemInstanceID_t> list = new List<SteamItemInstanceID_t>();
      if ((long) Characters.active.packageShirt != 0L)
        list.Add((SteamItemInstanceID_t) Characters.active.packageShirt);
      if ((long) Characters.active.packagePants != 0L)
        list.Add((SteamItemInstanceID_t) Characters.active.packagePants);
      if ((long) Characters.active.packageHat != 0L)
        list.Add((SteamItemInstanceID_t) Characters.active.packageHat);
      if ((long) Characters.active.packageBackpack != 0L)
        list.Add((SteamItemInstanceID_t) Characters.active.packageBackpack);
      if ((long) Characters.active.packageVest != 0L)
        list.Add((SteamItemInstanceID_t) Characters.active.packageVest);
      if ((long) Characters.active.packageMask != 0L)
        list.Add((SteamItemInstanceID_t) Characters.active.packageMask);
      if ((long) Characters.active.packageGlasses != 0L)
        list.Add((SteamItemInstanceID_t) Characters.active.packageGlasses);
      for (int index = 0; index < Characters.packageSkins.Count; ++index)
      {
        ulong num = Characters.packageSkins[index];
        if ((long) num != 0L)
          list.Add((SteamItemInstanceID_t) num);
      }
      if (list.Count > 0)
        SteamInventory.GetItemsByID(out Provider.provider.economyService.wearingResult, list.ToArray(), (uint) list.Count);
      Level.loading();
    }

    public static void launch()
    {
      if (!Level.exists(Provider.map))
      {
        Provider._connectionFailureInfo = ESteamConnectionFailureInfo.MAP;
        Provider.disconnect();
      }
      else
        Level.load(Level.getLevel(Provider.map));
    }

    public static void singleplayer(EGameMode singleplayerMode)
    {
      Provider._isConnected = true;
      Provider.resetChannels();
      Dedicator.security = ESteamSecurity.LAN;
      Dedicator.serverID = "Singleplayer_" + (object) Characters.selected;
      Commander.init();
      Provider.maxPlayers = (byte) 1;
      Provider.serverName = "Singleplayer #" + (object) ((int) Characters.selected + 1);
      Provider.serverPassword = "Singleplayer";
      Provider.ip = 0U;
      Provider.port = (ushort) 25000;
      Provider.lastNet = Time.realtimeSinceStartup;
      Provider.offsetNet = 0.0f;
      Provider.pings = new float[4];
      Provider.isPvP = true;
      Provider.filterName = false;
      Provider.mode = singleplayerMode;
      Provider.camera = ECameraMode.BOTH;
      Provider.lag(0.0f);
      Provider.timeout = 0.75f;
      SteamWhitelist.load();
      SteamBlacklist.load();
      SteamAdminlist.load();
      Provider._currentServerInfo = new SteamServerInfo(Provider.serverName, Provider.mode, false);
      Provider._time = SteamUtils.GetServerRealTime();
      Level.load(Level.getLevel(Provider.map));
      Provider._server = Provider.user;
      Provider._client = Provider._server;
      Provider._clientHash = Hash.SHA1(Provider.client);
      Provider.lastNet = Time.realtimeSinceStartup;
      Provider.offsetNet = 0.0f;
      Provider._isServer = true;
      Provider._isClient = true;
      if (Provider.onServerHosted == null)
        return;
      Provider.onServerHosted();
    }

    public static void host()
    {
      Provider._isConnected = true;
      Provider.resetChannels();
      Provider.openGameServer();
      Provider._isServer = true;
      if (Provider.onServerHosted == null)
        return;
      Provider.onServerHosted();
    }

    public static void shutdown()
    {
      for (int index = 0; index < Provider.clients.Count; ++index)
      {
        SteamGameServer.EndAuthSession(Provider.clients[index].playerID.steamID);
        Provider.send(Provider.clients[index].playerID.steamID, ESteamPacket.SHUTDOWN, new byte[1], 1, 0);
      }
      Provider.steam.Invoke("exit", 0.5f);
    }

    public static void disconnect()
    {
      if (Provider.isServer)
      {
        if (Dedicator.isDedicated)
          Provider.closeGameServer();
        else if (Provider.onServerShutdown != null)
          Provider.onServerShutdown();
        if (Provider.isClient)
        {
          Provider._client = Provider.user;
          Provider._clientHash = Hash.SHA1(Provider.client);
        }
        Provider._isServer = false;
        Provider._isClient = false;
      }
      else if (Provider.isClient)
      {
        SteamNetworking.CloseP2PSessionWithUser(Provider.server);
        for (int index = 0; index < Provider.clients.Count; ++index)
          SteamNetworking.CloseP2PSessionWithUser(Provider.clients[index].playerID.steamID);
        SteamFriends.SetRichPresence("connect", (string) null);
        Provider.closeTicket();
        SteamUser.AdvertiseGame(CSteamID.Nil, 0U, (ushort) 0);
        Provider._server = new CSteamID();
        Provider._isServer = false;
        Provider._isClient = false;
      }
      if (Provider.onClientDisconnected != null)
        Provider.onClientDisconnected();
      Level.exit();
      Provider._isConnected = false;
      Provider.isTesting = false;
      Provider.isLoadingUGC = false;
      Provider.isLoadingInventory = true;
    }

    public static void send(CSteamID steamID, ESteamPacket type, byte[] packet, int size, int channel)
    {
      if (!Provider.isConnected)
        return;
      Provider._bytesSent += (uint) size;
      ++Provider._packetsSent;
      if (Provider.isServer)
      {
        if (steamID == Provider.server || Provider.isClient && steamID == Provider.client)
          Provider.receiveServer(Provider.server, packet, 0, size, channel);
        else if ((long) steamID.m_SteamID == 0L)
          UnityEngine.Debug.LogError((object) "Failed to send to invalid steam ID.");
        else if (Provider.isUnreliable(type))
        {
          if (SteamGameServerNetworking.SendP2PPacket(steamID, packet, (uint) size, !Provider.isInstant(type) ? EP2PSend.k_EP2PSendUnreliable : EP2PSend.k_EP2PSendUnreliableNoDelay, channel))
            return;
          UnityEngine.Debug.LogError((object) ("Failed to send UDP packet to " + (object) steamID + "!"));
        }
        else
        {
          if (SteamGameServerNetworking.SendP2PPacket(steamID, packet, (uint) size, !Provider.isInstant(type) ? EP2PSend.k_EP2PSendReliableWithBuffering : EP2PSend.k_EP2PSendReliable, channel))
            return;
          UnityEngine.Debug.LogError((object) ("Failed to send TCP packet to " + (object) steamID + "!"));
        }
      }
      else if (steamID == Provider.client)
        Provider.receiveClient(Provider.client, packet, 0, size, channel);
      else if ((long) steamID.m_SteamID == 0L)
        UnityEngine.Debug.LogError((object) "Failed to send to invalid steam ID.");
      else if (Provider.isUnreliable(type))
      {
        if (SteamNetworking.SendP2PPacket(steamID, packet, (uint) size, !Provider.isInstant(type) ? EP2PSend.k_EP2PSendUnreliable : EP2PSend.k_EP2PSendUnreliableNoDelay, channel))
          return;
        UnityEngine.Debug.LogError((object) ("Failed to send UDP packet to " + (object) steamID + "!"));
      }
      else
      {
        if (SteamNetworking.SendP2PPacket(steamID, packet, (uint) size, !Provider.isInstant(type) ? EP2PSend.k_EP2PSendReliableWithBuffering : EP2PSend.k_EP2PSendReliable, channel))
          return;
        UnityEngine.Debug.LogError((object) ("Failed to send TCP packet to " + (object) steamID + "!"));
      }
    }

    private static void receiveServer(CSteamID steamID, byte[] packet, int offset, int size, int channel)
    {
      Provider._bytesReceived += (uint) size;
      ++Provider._packetsReceived;
      if (!Dedicator.isDedicated)
        return;
      ESteamPacket packet1 = (ESteamPacket) packet[offset];
      if (Provider.isUpdate(packet1))
      {
        if (steamID == Provider.server)
        {
          for (int index = 0; index < Provider.receivers.Count; ++index)
          {
            if (Provider.receivers[index].id == channel)
            {
              Provider.receivers[index].receive(steamID, packet, offset, size);
              break;
            }
          }
        }
        else
        {
          for (int index1 = 0; index1 < Provider.clients.Count; ++index1)
          {
            if (Provider.clients[index1].playerID.steamID == steamID)
            {
              for (int index2 = 0; index2 < Provider.receivers.Count; ++index2)
              {
                if (Provider.receivers[index2].id == channel)
                {
                  Provider.receivers[index2].receive(steamID, packet, offset, size);
                  break;
                }
              }
              break;
            }
          }
        }
      }
      else if (packet1 == ESteamPacket.WORKSHOP)
      {
        List<ulong> list = new List<ulong>();
        foreach (string path in ReadWrite.getFolders("/Bundles/Workshop/Content"))
        {
          ulong result;
          if (ulong.TryParse(ReadWrite.folderName(path), out result))
            list.Add(result);
        }
        foreach (string path in ReadWrite.getFolders(ServerSavedata.directory + "/" + Provider.serverID + "/Workshop/Content"))
        {
          ulong result;
          if (ulong.TryParse(ReadWrite.folderName(path), out result))
            list.Add(result);
        }
        ulong result1;
        if (ulong.TryParse(new DirectoryInfo(Level.info.path).Parent.Name, out result1))
          list.Add(result1);
        byte[] packet2 = new byte[2 + list.Count * 8];
        packet2[0] = (byte) 1;
        packet2[1] = (byte) list.Count;
        for (byte index = (byte) 0; (int) index < list.Count; ++index)
          BitConverter.GetBytes(list[(int) index]).CopyTo((Array) packet2, 2 + (int) index * 8);
        Provider.send(steamID, ESteamPacket.WORKSHOP, packet2, packet2.Length, 0);
      }
      else if (packet1 == ESteamPacket.TICK)
      {
        int size1;
        byte[] bytes = SteamPacker.getBytes(0, out size1, (object) 14, (object) Provider.net);
        Provider.send(steamID, ESteamPacket.TIME, bytes, size1, 0);
      }
      else if (packet1 == ESteamPacket.TIME)
      {
        for (int index = 0; index < Provider.clients.Count; ++index)
        {
          if (Provider.clients[index].playerID.steamID == steamID)
          {
            if ((double) Provider.clients[index].lastPing <= 0.0)
              break;
            Provider.clients[index].lastNet = Time.realtimeSinceStartup;
            Provider.clients[index].lag(Time.realtimeSinceStartup - Provider.clients[index].lastPing);
            Provider.clients[index].lastPing = -1f;
            break;
          }
        }
      }
      else if (packet1 == ESteamPacket.CONNECT)
      {
        for (int index = 0; index < Provider.pending.Count; ++index)
        {
          if (Provider.pending[index].playerID.steamID == steamID)
          {
            Provider.reject(steamID, ESteamRejection.ALREADY_PENDING);
            return;
          }
        }
        for (int index = 0; index < Provider.clients.Count; ++index)
        {
          if (Provider.clients[index].playerID.steamID == steamID)
          {
            Provider.reject(steamID, ESteamRejection.ALREADY_CONNECTED);
            return;
          }
        }
        object[] objects = SteamPacker.getObjects(steamID, offset, 0, packet, Types.BYTE_TYPE, Types.BYTE_TYPE, Types.STRING_TYPE, Types.STRING_TYPE, Types.BYTE_ARRAY_TYPE, Types.BYTE_ARRAY_TYPE, Types.BYTE_ARRAY_TYPE, Types.BYTE_TYPE, Types.STRING_TYPE, Types.BOOLEAN_TYPE, Types.SINGLE_TYPE, Types.STRING_TYPE, Types.STEAM_ID_TYPE, Types.BYTE_TYPE, Types.BYTE_TYPE, Types.BYTE_TYPE, Types.COLOR_TYPE, Types.COLOR_TYPE, Types.BOOLEAN_TYPE, Types.UINT64_TYPE, Types.UINT64_TYPE, Types.UINT64_TYPE, Types.UINT64_TYPE, Types.UINT64_TYPE, Types.UINT64_TYPE, Types.UINT64_TYPE, Types.UINT64_ARRAY_TYPE, Types.BYTE_TYPE);
        SteamPlayerID newPlayerID = new SteamPlayerID(steamID, (byte) objects[1], (string) objects[2], (string) objects[3], (string) objects[11], (CSteamID) objects[12]);
        if ((string) objects[8] != Provider.APP_VERSION)
          Provider.reject(steamID, ESteamRejection.WRONG_VERSION);
        else if (newPlayerID.playerName.Length < 2)
          Provider.reject(steamID, ESteamRejection.NAME_PLAYER_SHORT);
        else if (newPlayerID.characterName.Length < 2)
          Provider.reject(steamID, ESteamRejection.NAME_CHARACTER_SHORT);
        else if (newPlayerID.playerName.Length > 32)
          Provider.reject(steamID, ESteamRejection.NAME_PLAYER_LONG);
        else if (newPlayerID.characterName.Length > 32)
        {
          Provider.reject(steamID, ESteamRejection.NAME_CHARACTER_LONG);
        }
        else
        {
          long result1;
          double result2;
          if (long.TryParse(newPlayerID.playerName, out result1) || double.TryParse(newPlayerID.playerName, out result2))
          {
            Provider.reject(steamID, ESteamRejection.NAME_PLAYER_NUMBER);
          }
          else
          {
            long result3;
            double result4;
            if (long.TryParse(newPlayerID.characterName, out result3) || double.TryParse(newPlayerID.characterName, out result4))
            {
              Provider.reject(steamID, ESteamRejection.NAME_CHARACTER_NUMBER);
            }
            else
            {
              if (Provider.filterName)
              {
                if (!NameTool.isValid(newPlayerID.playerName))
                {
                  Provider.reject(steamID, ESteamRejection.NAME_PLAYER_INVALID);
                  return;
                }
                if (!NameTool.isValid(newPlayerID.characterName))
                {
                  Provider.reject(steamID, ESteamRejection.NAME_CHARACTER_INVALID);
                  return;
                }
              }
              SteamBlacklistID blacklistID;
              if (SteamBlacklist.checkBanned(steamID, out blacklistID))
              {
                int size1;
                byte[] bytes = SteamPacker.getBytes(0, out size1, (object) 9, (object) blacklistID.reason, (object) blacklistID.getTime());
                Provider.send(steamID, ESteamPacket.BANNED, bytes, size1, 0);
              }
              else if (!SteamWhitelist.checkWhitelisted(steamID))
                Provider.reject(steamID, ESteamRejection.WHITELISTED);
              else if (Provider.clients.Count + 1 > (int) Provider.maxPlayers)
              {
                Provider.reject(steamID, ESteamRejection.SERVER_FULL);
              }
              else
              {
                byte[] hash_0_1 = (byte[]) objects[4];
                if (hash_0_1.Length != 20)
                {
                  Provider.reject(steamID, ESteamRejection.WRONG_HASH);
                }
                else
                {
                  byte[] hash_0_2 = (byte[]) objects[5];
                  if (hash_0_2.Length != 20)
                  {
                    Provider.reject(steamID, ESteamRejection.WRONG_HASH);
                  }
                  else
                  {
                    byte[] h = (byte[]) objects[6];
                    if (h.Length != 20)
                      Provider.reject(steamID, ESteamRejection.WRONG_HASH);
                    else if (Provider.serverPassword == string.Empty || Hash.verifyHash(hash_0_1, Provider._serverPasswordHash))
                    {
                      if (Hash.verifyHash(hash_0_2, Level.hash))
                      {
                        if (ReadWrite.appIn(h, (byte) objects[7]))
                        {
                          if ((double) (float) objects[10] < (double) Provider.timeout)
                          {
                            Provider.pending.Add(new SteamPending(newPlayerID, (bool) objects[9], (byte) objects[13], (byte) objects[14], (byte) objects[15], (Color) objects[16], (Color) objects[17], (bool) objects[18], (ulong) objects[19], (ulong) objects[20], (ulong) objects[21], (ulong) objects[22], (ulong) objects[23], (ulong) objects[24], (ulong) objects[25], (ulong[]) objects[26], (EPlayerSpeciality) (byte) objects[27]));
                            Provider.send(steamID, ESteamPacket.VERIFY, new byte[1]
                            {
                              (byte) 3
                            }, 1, 0);
                          }
                          else
                            Provider.reject(steamID, ESteamRejection.PING);
                        }
                        else
                          Provider.reject(steamID, ESteamRejection.WRONG_HASH);
                      }
                      else
                        Provider.reject(steamID, ESteamRejection.WRONG_HASH);
                    }
                    else
                      Provider.reject(steamID, ESteamRejection.WRONG_PASSWORD);
                  }
                }
              }
            }
          }
        }
      }
      else if (packet1 == ESteamPacket.AUTHENTICATE)
      {
        SteamPending steamPending = (SteamPending) null;
        for (int index = 0; index < Provider.pending.Count; ++index)
        {
          if (Provider.pending[index].playerID.steamID == steamID)
          {
            steamPending = Provider.pending[index];
            break;
          }
        }
        if (steamPending == null)
          Provider.reject(steamID, ESteamRejection.NOT_PENDING);
        else if (Provider.clients.Count + 1 > (int) Provider.maxPlayers)
        {
          Provider.reject(steamID, ESteamRejection.SERVER_FULL);
        }
        else
        {
          ushort num1 = BitConverter.ToUInt16(packet, 1);
          byte[] ticket = new byte[(int) num1];
          Buffer.BlockCopy((Array) packet, 3, (Array) ticket, 0, (int) num1);
          ushort num2 = BitConverter.ToUInt16(packet, 3 + (int) num1);
          byte[] pBuffer = new byte[(int) num2];
          Buffer.BlockCopy((Array) packet, 5 + (int) num1, (Array) pBuffer, 0, (int) num2);
          if (!Provider.verifyTicket(steamID, ticket))
            Provider.reject(steamID, ESteamRejection.AUTH_VERIFICATION);
          else if ((int) num2 > 0)
          {
            if (SteamGameServerInventory.DeserializeResult(out steamPending.inventoryResult, pBuffer, (uint) num2, false))
              return;
            Provider.reject(steamID, ESteamRejection.AUTH_ECON);
          }
          else
          {
            steamPending.shirtItem = 0;
            steamPending.pantsItem = 0;
            steamPending.hatItem = 0;
            steamPending.backpackItem = 0;
            steamPending.vestItem = 0;
            steamPending.maskItem = 0;
            steamPending.glassesItem = 0;
            steamPending.skinItems = new int[0];
            steamPending.packageShirt = 0UL;
            steamPending.packagePants = 0UL;
            steamPending.packageHat = 0UL;
            steamPending.packageBackpack = 0UL;
            steamPending.packageVest = 0UL;
            steamPending.packageMask = 0UL;
            steamPending.packageGlasses = 0UL;
            steamPending.packageSkins = new ulong[0];
            steamPending.inventoryResult = SteamInventoryResult_t.Invalid;
            steamPending.inventoryDetails = new SteamItemDetails_t[0];
            steamPending.hasProof = true;
          }
        }
      }
      else
        UnityEngine.Debug.LogError((object) ("Failed to handle message: " + (object) packet1));
    }

    private static void receiveClient(CSteamID steamID, byte[] packet, int offset, int size, int channel)
    {
      Provider._bytesReceived += (uint) size;
      ++Provider._packetsReceived;
      ESteamPacket packet1 = (ESteamPacket) packet[offset];
      if (Provider.isUpdate(packet1))
      {
        for (int index = 0; index < Provider.receivers.Count; ++index)
        {
          if (Provider.receivers[index].id == channel)
          {
            Provider.receivers[index].receive(steamID, packet, offset, size);
            break;
          }
        }
      }
      else
      {
        if (!(steamID == Provider.server))
          return;
        if (packet1 == ESteamPacket.TICK)
          Provider.send(Provider.server, ESteamPacket.TIME, new byte[1]
          {
            (byte) 14
          }, 1, 0);
        else if (packet1 == ESteamPacket.TIME)
        {
          if ((double) Provider.lastPing <= 0.0)
            return;
          object[] objects = SteamPacker.getObjects(steamID, offset, 0, packet, Types.BYTE_TYPE, Types.SINGLE_TYPE);
          Provider.lastNet = Time.realtimeSinceStartup;
          Provider.offsetNet = (float) objects[1] + (float) (((double) Time.realtimeSinceStartup - (double) Provider.lastPing) / 2.0);
          Provider.lag(Time.realtimeSinceStartup - Provider.lastPing);
          Provider.lastPing = -1f;
        }
        else if (packet1 == ESteamPacket.SHUTDOWN)
        {
          Provider._connectionFailureInfo = ESteamConnectionFailureInfo.SHUTDOWN;
          Provider.disconnect();
        }
        else if (packet1 == ESteamPacket.CONNECTED)
        {
          object[] objects = SteamPacker.getObjects(steamID, offset, 0, packet, Types.BYTE_TYPE, Types.STEAM_ID_TYPE, Types.BYTE_TYPE, Types.STRING_TYPE, Types.STRING_TYPE, Types.VECTOR3_TYPE, Types.BYTE_TYPE, Types.BOOLEAN_TYPE, Types.BOOLEAN_TYPE, Types.INT32_TYPE, Types.STEAM_ID_TYPE, Types.STRING_TYPE, Types.BYTE_TYPE, Types.BYTE_TYPE, Types.BYTE_TYPE, Types.COLOR_TYPE, Types.COLOR_TYPE, Types.BOOLEAN_TYPE, Types.INT32_TYPE, Types.INT32_TYPE, Types.INT32_TYPE, Types.INT32_TYPE, Types.INT32_TYPE, Types.INT32_TYPE, Types.INT32_TYPE, Types.INT32_ARRAY_TYPE, Types.BYTE_TYPE);
          Provider.addPlayer(new SteamPlayerID((CSteamID) objects[1], (byte) objects[2], (string) objects[3], (string) objects[4], (string) objects[11], (CSteamID) objects[10]), (Vector3) objects[5], (byte) objects[6], (bool) objects[7], (bool) objects[8], (int) objects[9], (byte) objects[12], (byte) objects[13], (byte) objects[14], (Color) objects[15], (Color) objects[16], (bool) objects[17], (int) objects[18], (int) objects[19], (int) objects[20], (int) objects[21], (int) objects[22], (int) objects[23], (int) objects[24], (int[]) objects[25], (EPlayerSpeciality) (byte) objects[26]);
        }
        else if (packet1 == ESteamPacket.DISCONNECTED)
          Provider.removePlayer(packet[offset + 1]);
        else if (packet1 == ESteamPacket.WORKSHOP)
        {
          Provider.isTesting = false;
          Provider.provider.workshopService.installing = new List<PublishedFileId_t>();
          byte num1 = packet[offset + 1];
          for (byte index = (byte) 0; (int) index < (int) num1; ++index)
          {
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            Provider.\u003CreceiveClient\u003Ec__AnonStoreyF clientCAnonStoreyF = new Provider.\u003CreceiveClient\u003Ec__AnonStoreyF();
            ulong num2 = BitConverter.ToUInt64(packet, offset + 2 + (int) index * 8);
            // ISSUE: reference to a compiler-generated field
            clientCAnonStoreyF.file = new PublishedFileId_t(num2);
            ulong punSizeOnDisk;
            string pchFolder;
            uint punTimeStamp;
            // ISSUE: reference to a compiler-generated field
            if (SteamUGC.GetItemInstallInfo(clientCAnonStoreyF.file, out punSizeOnDisk, out pchFolder, 1024U, out punTimeStamp))
            {
              // ISSUE: reference to a compiler-generated method
              if (Provider.provider.workshopService.ugc.Find(new Predicate<SteamContent>(clientCAnonStoreyF.\u003C\u003Em__C)) == null)
              {
                // ISSUE: reference to a compiler-generated field
                Provider.provider.workshopService.installing.Add(clientCAnonStoreyF.file);
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              Provider.provider.workshopService.installing.Add(clientCAnonStoreyF.file);
            }
          }
          Provider.provider.workshopService.installed = Provider.provider.workshopService.installing.Count;
          if (Provider.provider.workshopService.installed == 0)
            Provider.launch();
          else
            SteamUGC.DownloadItem(Provider.provider.workshopService.installing[0], true);
        }
        else if (packet1 == ESteamPacket.VERIFY)
        {
          byte[] numArray = Provider.openTicket();
          if (numArray == null)
          {
            Provider._connectionFailureInfo = ESteamConnectionFailureInfo.AUTH_EMPTY;
            Provider.disconnect();
          }
          else
          {
            byte[] pOutBuffer;
            uint punOutBufferSize;
            if (Provider.provider.economyService.wearingResult == SteamInventoryResult_t.Invalid)
            {
              pOutBuffer = new byte[0];
              punOutBufferSize = 0U;
            }
            else if (SteamInventory.SerializeResult(Provider.provider.economyService.wearingResult, (byte[]) null, out punOutBufferSize))
            {
              pOutBuffer = new byte[(IntPtr) punOutBufferSize];
              SteamInventory.SerializeResult(Provider.provider.economyService.wearingResult, pOutBuffer, out punOutBufferSize);
              SteamInventory.DestroyResult(Provider.provider.economyService.wearingResult);
              Provider.provider.economyService.wearingResult = SteamInventoryResult_t.Invalid;
            }
            else
            {
              SteamInventory.DestroyResult(Provider.provider.economyService.wearingResult);
              Provider.provider.economyService.wearingResult = SteamInventoryResult_t.Invalid;
              Provider._connectionFailureInfo = ESteamConnectionFailureInfo.AUTH_ECON;
              Provider.disconnect();
              return;
            }
            byte[] packet2 = new byte[(long) (5 + numArray.Length) + (long) punOutBufferSize];
            packet2[0] = (byte) 4;
            Buffer.BlockCopy((Array) BitConverter.GetBytes((ushort) numArray.Length), 0, (Array) packet2, 1, 2);
            Buffer.BlockCopy((Array) numArray, 0, (Array) packet2, 3, numArray.Length);
            Buffer.BlockCopy((Array) BitConverter.GetBytes((ushort) punOutBufferSize), 0, (Array) packet2, 3 + numArray.Length, 2);
            Buffer.BlockCopy((Array) pOutBuffer, 0, (Array) packet2, 5 + numArray.Length, (int) punOutBufferSize);
            Provider.send(Provider.server, ESteamPacket.AUTHENTICATE, packet2, packet2.Length, 0);
          }
        }
        else if (packet1 == ESteamPacket.ACCEPTED)
        {
          object[] objects = SteamPacker.getObjects(steamID, offset, 0, packet, Types.BYTE_TYPE, Types.UINT32_TYPE, Types.UINT16_TYPE);
          uint num1 = (uint) objects[1];
          ushort num2 = (ushort) objects[2];
          if (Provider.onClientConnected != null)
            Provider.onClientConnected();
          SteamUser.AdvertiseGame(Provider.server, num1, num2);
          SteamFriends.SetRichPresence("connect", string.Concat(new object[4]
          {
            (object) "+connect ",
            (object) num1,
            (object) ":",
            (object) num2
          }));
          Provider.favoriteIP = num1;
          Provider.favoritePort = num2;
          Provider._isFavorited = false;
          for (int iGame = 0; iGame < SteamMatchmaking.GetFavoriteGameCount(); ++iGame)
          {
            AppId_t pnAppID;
            uint pnIP;
            ushort pnConnPort;
            ushort pnQueryPort;
            uint punFlags;
            uint pRTime32LastPlayedOnServer;
            SteamMatchmaking.GetFavoriteGame(iGame, out pnAppID, out pnIP, out pnConnPort, out pnQueryPort, out punFlags, out pRTime32LastPlayedOnServer);
            if (pnAppID == Provider.APP_ID && (int) pnIP == (int) Provider.favoriteIP && (int) Provider.favoritePort == (int) pnConnPort)
            {
              Provider._isFavorited = true;
              break;
            }
          }
          SteamMatchmaking.AddFavoriteGame(Provider.APP_ID, num1, num2, (ushort) ((uint) num2 + 1U), 2U, SteamUtils.GetServerRealTime());
        }
        else if (packet1 == ESteamPacket.REJECTED)
        {
          switch ((ESteamRejection) packet[offset + 1])
          {
            case ESteamRejection.WHITELISTED:
              Provider._connectionFailureInfo = ESteamConnectionFailureInfo.WHITELISTED;
              break;
            case ESteamRejection.WRONG_PASSWORD:
              Provider._connectionFailureInfo = ESteamConnectionFailureInfo.PASSWORD;
              break;
            case ESteamRejection.SERVER_FULL:
              Provider._connectionFailureInfo = ESteamConnectionFailureInfo.FULL;
              break;
            case ESteamRejection.WRONG_HASH:
              Provider._connectionFailureInfo = ESteamConnectionFailureInfo.HASH;
              break;
            case ESteamRejection.WRONG_VERSION:
              Provider._connectionFailureInfo = ESteamConnectionFailureInfo.VERSION;
              break;
            case ESteamRejection.PRO:
              Provider._connectionFailureInfo = ESteamConnectionFailureInfo.PRO;
              break;
            case ESteamRejection.AUTH_VERIFICATION:
              Provider._connectionFailureInfo = ESteamConnectionFailureInfo.AUTH_VERIFICATION;
              break;
            case ESteamRejection.AUTH_NO_STEAM:
              Provider._connectionFailureInfo = ESteamConnectionFailureInfo.AUTH_NO_STEAM;
              break;
            case ESteamRejection.AUTH_LICENSE_EXPIRED:
              Provider._connectionFailureInfo = ESteamConnectionFailureInfo.AUTH_LICENSE_EXPIRED;
              break;
            case ESteamRejection.AUTH_VAC_BAN:
              Provider._connectionFailureInfo = ESteamConnectionFailureInfo.AUTH_VAC_BAN;
              break;
            case ESteamRejection.AUTH_ELSEWHERE:
              Provider._connectionFailureInfo = ESteamConnectionFailureInfo.AUTH_ELSEWHERE;
              break;
            case ESteamRejection.AUTH_TIMED_OUT:
              Provider._connectionFailureInfo = ESteamConnectionFailureInfo.AUTH_TIMED_OUT;
              break;
            case ESteamRejection.AUTH_USED:
              Provider._connectionFailureInfo = ESteamConnectionFailureInfo.AUTH_USED;
              break;
            case ESteamRejection.AUTH_NO_USER:
              Provider._connectionFailureInfo = ESteamConnectionFailureInfo.AUTH_NO_USER;
              break;
            case ESteamRejection.AUTH_PUB_BAN:
              Provider._connectionFailureInfo = ESteamConnectionFailureInfo.AUTH_PUB_BAN;
              break;
            case ESteamRejection.AUTH_ECON:
              Provider._connectionFailureInfo = ESteamConnectionFailureInfo.AUTH_ECON;
              break;
            case ESteamRejection.ALREADY_CONNECTED:
            case ESteamRejection.ALREADY_PENDING:
            case ESteamRejection.LATE_PENDING:
            case ESteamRejection.NOT_PENDING:
              Provider._connectionFailureInfo = ESteamConnectionFailureInfo.PENDING;
              break;
            case ESteamRejection.NAME_PLAYER_SHORT:
              Provider._connectionFailureInfo = ESteamConnectionFailureInfo.NAME_PLAYER_SHORT;
              break;
            case ESteamRejection.NAME_PLAYER_LONG:
              Provider._connectionFailureInfo = ESteamConnectionFailureInfo.NAME_PLAYER_LONG;
              break;
            case ESteamRejection.NAME_PLAYER_INVALID:
              Provider._connectionFailureInfo = ESteamConnectionFailureInfo.NAME_PLAYER_INVALID;
              break;
            case ESteamRejection.NAME_PLAYER_NUMBER:
              Provider._connectionFailureInfo = ESteamConnectionFailureInfo.NAME_PLAYER_NUMBER;
              break;
            case ESteamRejection.NAME_CHARACTER_SHORT:
              Provider._connectionFailureInfo = ESteamConnectionFailureInfo.NAME_CHARACTER_SHORT;
              break;
            case ESteamRejection.NAME_CHARACTER_LONG:
              Provider._connectionFailureInfo = ESteamConnectionFailureInfo.NAME_CHARACTER_LONG;
              break;
            case ESteamRejection.NAME_CHARACTER_INVALID:
              Provider._connectionFailureInfo = ESteamConnectionFailureInfo.NAME_CHARACTER_INVALID;
              break;
            case ESteamRejection.NAME_CHARACTER_NUMBER:
              Provider._connectionFailureInfo = ESteamConnectionFailureInfo.NAME_CHARACTER_NUMBER;
              break;
            case ESteamRejection.PING:
              Provider._connectionFailureInfo = ESteamConnectionFailureInfo.PING;
              break;
          }
          Provider.disconnect();
        }
        else if (packet1 == ESteamPacket.BANNED)
        {
          object[] objects = SteamPacker.getObjects(steamID, offset, 0, packet, Types.BYTE_TYPE, Types.STRING_TYPE, Types.UINT32_TYPE);
          Provider._connectionFailureInfo = ESteamConnectionFailureInfo.BANNED;
          Provider._connectionFailureReason = (string) objects[1];
          Provider._connectionFailureDuration = (uint) objects[2];
          Provider.disconnect();
        }
        else if (packet1 == ESteamPacket.KICKED)
        {
          object[] objects = SteamPacker.getObjects(steamID, offset, 0, packet, Types.BYTE_TYPE, Types.STRING_TYPE);
          Provider._connectionFailureInfo = ESteamConnectionFailureInfo.KICKED;
          Provider._connectionFailureReason = (string) objects[1];
          Provider.disconnect();
        }
        else if (packet1 == ESteamPacket.ADMINED)
        {
          int index = (int) packet[offset + 1];
          if (index < 0 || index >= Provider.clients.Count)
            UnityEngine.Debug.LogError((object) ("Failed to find player at index " + (object) index + "."));
          else
            Provider.clients[index].isAdmin = true;
        }
        else if (packet1 == ESteamPacket.UNADMINED)
        {
          int index = (int) packet[offset + 1];
          if (index < 0 || index >= Provider.clients.Count)
            UnityEngine.Debug.LogError((object) ("Failed to find player at index " + (object) index + "."));
          else
            Provider.clients[index].isAdmin = false;
        }
        else
          UnityEngine.Debug.LogError((object) ("Failed to handle message: " + (object) packet1));
      }
    }

    private static void listenServer(int channel)
    {
      ICommunityEntity entity;
      ulong length;
      while (Provider.provider.multiplayerService.serverMultiplayerService.read(out entity, Provider.buffer, out length, channel))
        Provider.receiveServer(((SteamworksCommunityEntity) entity).steamID, Provider.buffer, 0, (int) length, channel);
    }

    private static void listenClient(int channel)
    {
      ICommunityEntity entity;
      ulong length;
      while (Provider.provider.multiplayerService.clientMultiplayerService.read(out entity, Provider.buffer, out length, channel))
        Provider.receiveClient(((SteamworksCommunityEntity) entity).steamID, Provider.buffer, 0, (int) length, channel);
    }

    private static void listen()
    {
      if (!Provider.isConnected)
        return;
      if (Provider.isServer)
      {
        if (!Dedicator.isDedicated)
          return;
        Provider.listenServer(0);
        for (int index = 0; index < Provider.receivers.Count; ++index)
          Provider.listenServer(Provider.receivers[index].id);
        if (!Dedicator.isDedicated)
          return;
        if ((double) Time.realtimeSinceStartup - (double) Provider.lastCheck > (double) Provider.CHECKRATE)
        {
          Provider.lastCheck = Time.realtimeSinceStartup;
          for (int index = 0; index < Provider.clients.Count; ++index)
          {
            if ((double) Time.realtimeSinceStartup - (double) Provider.clients[index].lastPing > 1.0 || (double) Provider.clients[index].lastPing < 0.0)
            {
              Provider.clients[index].lastPing = Time.realtimeSinceStartup;
              Provider.send(Provider.clients[index].playerID.steamID, ESteamPacket.TICK, new byte[1]
              {
                (byte) 13
              }, 1, 0);
            }
          }
        }
        for (int index = 0; index < Provider.clients.Count; ++index)
        {
          if ((double) Time.realtimeSinceStartup - (double) Provider.clients[index].lastNet > (double) Provider.SERVER_TIMEOUT || (double) Time.realtimeSinceStartup - (double) Provider.clients[index].joined > (double) Provider.SERVER_TIMEOUT && (double) Provider.clients[index].ping > (double) Provider.timeout)
            Provider.dismiss(Provider.clients[index].playerID.steamID);
        }
        for (int index = 0; index < Provider.pending.Count; ++index)
        {
          if ((double) Time.realtimeSinceStartup - (double) Provider.pending[index].joined > (double) Provider.PENDING_TIMEOUT)
            Provider.reject(Provider.pending[index].playerID.steamID, ESteamRejection.LATE_PENDING);
        }
      }
      else
      {
        Provider.listenClient(0);
        for (int index = 0; index < Provider.receivers.Count; ++index)
          Provider.listenClient(Provider.receivers[index].id);
        if (Provider.isLoadingUGC)
        {
          if (Provider.isTesting && (double) Time.realtimeSinceStartup - (double) Provider.lastNet > (double) Provider.CLIENT_TIMEOUT)
          {
            Provider._connectionFailureInfo = ESteamConnectionFailureInfo.TIMED_OUT;
            Provider.disconnect();
          }
          else
            Provider.lastNet = Time.realtimeSinceStartup;
        }
        else if ((double) Time.realtimeSinceStartup - (double) Provider.lastNet > (double) Provider.CLIENT_TIMEOUT)
        {
          Provider._connectionFailureInfo = ESteamConnectionFailureInfo.TIMED_OUT;
          Provider.disconnect();
        }
        else
        {
          if ((double) Time.realtimeSinceStartup - (double) Provider.lastCheck <= (double) Provider.CHECKRATE || (double) Time.realtimeSinceStartup - (double) Provider.lastPing <= 1.0 && (double) Provider.lastPing >= 0.0)
            return;
          Provider.lastCheck = Time.realtimeSinceStartup;
          Provider.lastPing = Time.realtimeSinceStartup;
          Provider.send(Provider.server, ESteamPacket.TICK, new byte[1]
          {
            (byte) 13
          }, 1, 0);
        }
      }
    }

    private static void onGSPolicyResponse(GSPolicyResponse_t callback)
    {
      if ((int) callback.m_bSecure != 0)
      {
        Dedicator.security = ESteamSecurity.SECURE;
      }
      else
      {
        if (Dedicator.security != ESteamSecurity.SECURE)
          return;
        Dedicator.security = ESteamSecurity.INSECURE;
      }
    }

    private static void onP2PSessionConnectFail(P2PSessionConnectFail_t callback)
    {
      Provider.dismiss(callback.m_steamIDRemote);
    }

    private static void onValidateAuthTicketResponse(ValidateAuthTicketResponse_t callback)
    {
      if (callback.m_eAuthSessionResponse == EAuthSessionResponse.k_EAuthSessionResponseOK)
      {
        SteamPending steamPending = (SteamPending) null;
        for (int index = 0; index < Provider.pending.Count; ++index)
        {
          if (Provider.pending[index].playerID.steamID == callback.m_SteamID)
          {
            steamPending = Provider.pending[index];
            break;
          }
        }
        if (steamPending == null)
        {
          Provider.reject(callback.m_SteamID, ESteamRejection.NOT_PENDING);
        }
        else
        {
          bool flag = SteamGameServer.UserHasLicenseForApp(steamPending.playerID.steamID, Provider.PRO_ID) == EUserHasLicenseForAppResult.k_EUserHasLicenseResultHasLicense;
          if (Provider.mode == EGameMode.PRO && !flag)
            Provider.reject(steamPending.playerID.steamID, ESteamRejection.PRO);
          else if ((int) steamPending.playerID.characterID >= (int) Customization.FREE_CHARACTERS && !flag || (int) steamPending.playerID.characterID >= (int) Customization.FREE_CHARACTERS + (int) Customization.PRO_CHARACTERS)
            Provider.reject(steamPending.playerID.steamID, ESteamRejection.PRO);
          else if (!flag && steamPending.isPro)
            Provider.reject(steamPending.playerID.steamID, ESteamRejection.PRO);
          else if ((int) steamPending.face >= (int) Customization.FACES_FREE + (int) Customization.FACES_PRO || !flag && (int) steamPending.face >= (int) Customization.FACES_FREE)
            Provider.reject(steamPending.playerID.steamID, ESteamRejection.PRO);
          else if ((int) steamPending.hair >= (int) Customization.HAIRS_FREE + (int) Customization.HAIRS_PRO || !flag && (int) steamPending.hair >= (int) Customization.HAIRS_FREE)
            Provider.reject(steamPending.playerID.steamID, ESteamRejection.PRO);
          else if ((int) steamPending.beard >= (int) Customization.BEARDS_FREE + (int) Customization.BEARDS_PRO || !flag && (int) steamPending.beard >= (int) Customization.BEARDS_FREE)
          {
            Provider.reject(steamPending.playerID.steamID, ESteamRejection.PRO);
          }
          else
          {
            if (!flag)
            {
              if (!Customization.checkSkin(steamPending.skin))
              {
                Provider.reject(steamPending.playerID.steamID, ESteamRejection.PRO);
                return;
              }
              if (!Customization.checkColor(steamPending.color))
              {
                Provider.reject(steamPending.playerID.steamID, ESteamRejection.PRO);
                return;
              }
            }
            steamPending.assignedPro = flag;
            steamPending.assignedAdmin = SteamAdminlist.checkAdmin(steamPending.playerID.steamID);
            steamPending.hasAuthentication = true;
            if (!steamPending.hasProof)
              return;
            Provider.accept(steamPending.playerID, steamPending.assignedPro, steamPending.assignedAdmin, steamPending.face, steamPending.hair, steamPending.beard, steamPending.skin, steamPending.color, steamPending.hand, steamPending.shirtItem, steamPending.pantsItem, steamPending.hatItem, steamPending.backpackItem, steamPending.vestItem, steamPending.maskItem, steamPending.glassesItem, steamPending.skinItems, steamPending.speciality);
          }
        }
      }
      else if (callback.m_eAuthSessionResponse == EAuthSessionResponse.k_EAuthSessionResponseUserNotConnectedToSteam)
        Provider.reject(callback.m_SteamID, ESteamRejection.AUTH_NO_STEAM);
      else if (callback.m_eAuthSessionResponse == EAuthSessionResponse.k_EAuthSessionResponseNoLicenseOrExpired)
        Provider.reject(callback.m_SteamID, ESteamRejection.AUTH_LICENSE_EXPIRED);
      else if (callback.m_eAuthSessionResponse == EAuthSessionResponse.k_EAuthSessionResponseVACBanned)
        Provider.reject(callback.m_SteamID, ESteamRejection.AUTH_VAC_BAN);
      else if (callback.m_eAuthSessionResponse == EAuthSessionResponse.k_EAuthSessionResponseLoggedInElseWhere)
        Provider.reject(callback.m_SteamID, ESteamRejection.AUTH_ELSEWHERE);
      else if (callback.m_eAuthSessionResponse == EAuthSessionResponse.k_EAuthSessionResponseVACCheckTimedOut)
        Provider.reject(callback.m_SteamID, ESteamRejection.AUTH_TIMED_OUT);
      else if (callback.m_eAuthSessionResponse == EAuthSessionResponse.k_EAuthSessionResponseAuthTicketCanceled)
        Provider.dismiss(callback.m_SteamID);
      else if (callback.m_eAuthSessionResponse == EAuthSessionResponse.k_EAuthSessionResponseAuthTicketInvalidAlreadyUsed)
        Provider.reject(callback.m_SteamID, ESteamRejection.AUTH_USED);
      else if (callback.m_eAuthSessionResponse == EAuthSessionResponse.k_EAuthSessionResponseAuthTicketInvalid)
      {
        Provider.reject(callback.m_SteamID, ESteamRejection.AUTH_NO_USER);
      }
      else
      {
        if (callback.m_eAuthSessionResponse != EAuthSessionResponse.k_EAuthSessionResponsePublisherIssuedBan)
          return;
        Provider.reject(callback.m_SteamID, ESteamRejection.AUTH_PUB_BAN);
      }
    }

    public static void accept(SteamPlayerID playerID, bool isPro, bool isAdmin, byte face, byte hair, byte beard, Color skin, Color color, bool hand, int shirtItem, int pantsItem, int hatItem, int backpackItem, int vestItem, int maskItem, int glassesItem, int[] skinItems, EPlayerSpeciality speciality)
    {
      bool flag = false;
      for (int index = 0; index < Provider.pending.Count; ++index)
      {
        if (Provider.pending[index].playerID == playerID)
        {
          if (Provider.pending[index].inventoryResult != SteamInventoryResult_t.Invalid)
          {
            SteamGameServerInventory.DestroyResult(Provider.pending[index].inventoryResult);
            Provider.pending[index].inventoryResult = SteamInventoryResult_t.Invalid;
          }
          flag = true;
          Provider.pending.RemoveAt(index);
          break;
        }
      }
      if (!flag)
        return;
      if (isPro)
        SteamGameServer.BUpdateUserData(playerID.steamID, playerID.playerName, 1U);
      else
        SteamGameServer.BUpdateUserData(playerID.steamID, playerID.playerName, 0U);
      Vector3 zero = Vector3.zero;
      Vector3 point;
      byte angle;
      if (PlayerSavedata.fileExists(playerID, "/Player/Player.dat") && Level.info.type == ELevelType.SURVIVAL)
      {
        Block block = PlayerSavedata.readBlock(playerID, "/Player/Player.dat", (byte) 1);
        point = LevelGround.checkSafe(block.readSingleVector3() + new Vector3(0.0f, 0.5f, 0.0f));
        angle = block.readByte();
      }
      else
      {
        PlayerSpawnpoint spawn = LevelPlayers.getSpawn();
        point = spawn.point + new Vector3(0.0f, 0.5f, 0.0f);
        angle = (byte) ((double) spawn.angle / 2.0);
      }
      float height = LevelGround.getHeight(point);
      if ((double) point.y < (double) height + 0.5)
        point.y = height + 0.5f;
      int channels = Provider.channels;
      Provider.addPlayer(playerID, point, angle, isPro, isAdmin, channels, face, hair, beard, skin, color, hand, shirtItem, pantsItem, hatItem, backpackItem, vestItem, maskItem, glassesItem, skinItems, speciality);
      int size;
      for (int index = 0; index < Provider.clients.Count; ++index)
      {
        byte[] bytes = SteamPacker.getBytes(0, out size, (object) 11, (object) Provider.clients[index].playerID.steamID, (object) Provider.clients[index].playerID.characterID, (object) Provider.clients[index].playerID.playerName, (object) Provider.clients[index].playerID.characterName, (object) Provider.clients[index].model.transform.position, (object) (byte) ((double) Provider.clients[index].model.transform.rotation.eulerAngles.y / 2.0), (object) (bool) (Provider.clients[index].isPro ? 1 : 0), (object) (bool) (Provider.clients[index].isAdmin ? 1 : 0), (object) Provider.clients[index].channel, (object) Provider.clients[index].playerID.group, (object) Provider.clients[index].playerID.nickName, (object) Provider.clients[index].face, (object) Provider.clients[index].hair, (object) Provider.clients[index].beard, (object) Provider.clients[index].skin, (object) Provider.clients[index].color, (object) (bool) (Provider.clients[index].hand ? 1 : 0), (object) Provider.clients[index].shirtItem, (object) Provider.clients[index].pantsItem, (object) Provider.clients[index].hatItem, (object) Provider.clients[index].backpackItem, (object) Provider.clients[index].vestItem, (object) Provider.clients[index].maskItem, (object) Provider.clients[index].glassesItem, (object) Provider.clients[index].skinItems, (object) (byte) Provider.clients[index].speciality);
        Provider.send(playerID.steamID, ESteamPacket.CONNECTED, bytes, size, 0);
      }
      byte[] bytes1 = SteamPacker.getBytes(0, out size, (object) 6, (object) SteamGameServer.GetPublicIP(), (object) Provider.port);
      Provider.send(playerID.steamID, ESteamPacket.ACCEPTED, bytes1, size, 0);
      byte[] bytes2 = SteamPacker.getBytes(0, out size, (object) 11, (object) playerID.steamID, (object) playerID.characterID, (object) playerID.playerName, (object) playerID.characterName, (object) point, (object) angle, (object) (bool) (isPro ? 1 : 0), (object) (bool) (isAdmin ? 1 : 0), (object) channels, (object) playerID.group, (object) playerID.nickName, (object) face, (object) hair, (object) beard, (object) skin, (object) color, (object) (bool) (hand ? 1 : 0), (object) shirtItem, (object) pantsItem, (object) hatItem, (object) backpackItem, (object) vestItem, (object) maskItem, (object) glassesItem, (object) skinItems, (object) (byte) speciality);
      for (int index = 0; index < Provider.clients.Count; ++index)
      {
        if (Provider.clients[index].playerID != playerID)
          Provider.send(Provider.clients[index].playerID.steamID, ESteamPacket.CONNECTED, bytes2, size, 0);
      }
      if (Provider.onServerConnected != null)
        Provider.onServerConnected(playerID.steamID);
      CommandWindow.Log((object) Provider.localization.format("PlayerConnectedText", (object) playerID.steamID, (object) playerID.playerName, (object) playerID.characterName));
    }

    public static void reject(CSteamID steamID, ESteamRejection rejection)
    {
      for (int index = 0; index < Provider.pending.Count; ++index)
      {
        if (Provider.pending[index].playerID.steamID == steamID)
        {
          if (Provider.pending[index].inventoryResult != SteamInventoryResult_t.Invalid)
          {
            SteamGameServerInventory.DestroyResult(Provider.pending[index].inventoryResult);
            Provider.pending[index].inventoryResult = SteamInventoryResult_t.Invalid;
          }
          Provider.pending.RemoveAt(index);
          break;
        }
      }
      SteamGameServer.EndAuthSession(steamID);
      Provider.send(steamID, ESteamPacket.REJECTED, new byte[2]
      {
        (byte) 5,
        (byte) rejection
      }, 2, 0);
    }

    public static void kick(CSteamID steamID, string reason)
    {
      bool flag = false;
      for (int index = 0; index < Provider.clients.Count; ++index)
      {
        if (Provider.clients[index].playerID.steamID == steamID)
        {
          flag = true;
          break;
        }
      }
      if (!flag)
        return;
      int size;
      byte[] bytes = SteamPacker.getBytes(0, out size, (object) 10, (object) reason);
      Provider.send(steamID, ESteamPacket.KICKED, bytes, size, 0);
      if (Provider.onServerDisconnected != null)
        Provider.onServerDisconnected(steamID);
      SteamGameServer.EndAuthSession(steamID);
      for (int index1 = 0; index1 < Provider.clients.Count; ++index1)
      {
        if (Provider.clients[index1].playerID.steamID == steamID)
        {
          Provider.removePlayer((byte) index1);
          for (int index2 = 0; index2 < Provider.clients.Count; ++index2)
          {
            if (Provider.clients[index2].playerID.steamID != steamID)
              Provider.send(Provider.clients[index2].playerID.steamID, ESteamPacket.DISCONNECTED, new byte[2]
              {
                (byte) 12,
                (byte) index1
              }, 2, 0);
          }
          break;
        }
      }
    }

    public static void ban(CSteamID steamID, string reason, uint duration)
    {
      bool flag = false;
      for (int index = 0; index < Provider.clients.Count; ++index)
      {
        if (Provider.clients[index].playerID.steamID == steamID)
        {
          flag = true;
          break;
        }
      }
      if (!flag)
        return;
      int size;
      byte[] bytes = SteamPacker.getBytes(0, out size, (object) 9, (object) reason, (object) duration);
      Provider.send(steamID, ESteamPacket.BANNED, bytes, size, 0);
      for (int index1 = 0; index1 < Provider.clients.Count; ++index1)
      {
        if (Provider.clients[index1].playerID.steamID == steamID)
        {
          if (Provider.onServerDisconnected != null)
            Provider.onServerDisconnected(steamID);
          SteamGameServer.EndAuthSession(steamID);
          Provider.removePlayer((byte) index1);
          for (int index2 = 0; index2 < Provider.clients.Count; ++index2)
          {
            if (Provider.clients[index2].playerID.steamID != steamID)
              Provider.send(Provider.clients[index2].playerID.steamID, ESteamPacket.DISCONNECTED, new byte[2]
              {
                (byte) 12,
                (byte) index1
              }, 2, 0);
          }
          break;
        }
      }
    }

    public static void dismiss(CSteamID steamID)
    {
      bool flag = false;
      for (int index = 0; index < Provider.clients.Count; ++index)
      {
        if (Provider.clients[index].playerID.steamID == steamID)
        {
          flag = true;
          break;
        }
      }
      if (!flag)
        return;
      if (Provider.onServerDisconnected != null)
        Provider.onServerDisconnected(steamID);
      SteamGameServer.EndAuthSession(steamID);
      for (int index1 = 0; index1 < Provider.clients.Count; ++index1)
      {
        if (Provider.clients[index1].playerID.steamID == steamID)
        {
          CommandWindow.Log((object) Provider.localization.format("PlayerDisconnectedText", (object) Provider.clients[index1].playerID.steamID, (object) Provider.clients[index1].playerID.playerName, (object) Provider.clients[index1].playerID.characterName));
          Provider.removePlayer((byte) index1);
          for (int index2 = 0; index2 < Provider.clients.Count; ++index2)
          {
            if (Provider.clients[index2].playerID.steamID != steamID)
              Provider.send(Provider.clients[index2].playerID.steamID, ESteamPacket.DISCONNECTED, new byte[2]
              {
                (byte) 12,
                (byte) index1
              }, 2, 0);
          }
          break;
        }
      }
    }

    private static bool verifyTicket(CSteamID steamID, byte[] ticket)
    {
      return SteamGameServer.BeginAuthSession(ticket, ticket.Length, steamID) == EBeginAuthSessionResult.k_EBeginAuthSessionResultOK;
    }

    private static void openGameServer()
    {
      if (Provider.isServer || Provider.isClient)
      {
        UnityEngine.Debug.LogError((object) "Failed to open game server: session already in progress.");
      }
      else
      {
        ESecurityMode security = ESecurityMode.LAN;
        if (Dedicator.security == ESteamSecurity.SECURE)
          security = ESecurityMode.SECURE;
        else if (Dedicator.security == ESteamSecurity.INSECURE)
          security = ESecurityMode.INSECURE;
        try
        {
          Provider.provider.multiplayerService.serverMultiplayerService.open(Provider.ip, Provider.port, security);
        }
        catch (Exception ex)
        {
          UnityEngine.Debug.LogError((object) ex.Message);
          Application.Quit();
          return;
        }
        Provider.apiWarningMessageHook = new SteamAPIWarningMessageHook_t(Provider.onAPIWarningMessage);
        SteamGameServerUtils.SetWarningMessageHook(Provider.apiWarningMessageHook);
        Provider._time = SteamGameServerUtils.GetServerRealTime();
        Level.load(Level.getLevel(Provider.map));
        SteamGameServer.SetMaxPlayerCount((int) Provider.maxPlayers);
        SteamGameServer.SetServerName(Provider.serverName);
        SteamGameServer.SetPasswordProtected(Provider.serverPassword != string.Empty);
        SteamGameServer.SetMapName(Provider.map);
        if (Dedicator.isDedicated)
        {
          string[] folders1 = ReadWrite.getFolders("/Bundles/Workshop/Content");
          string[] folders2 = ReadWrite.getFolders(ServerSavedata.directory + "/" + Provider.serverID + "/Workshop/Content");
          string[] folders3 = ReadWrite.getFolders("/Bundles/Workshop/Maps");
          string[] folders4 = ReadWrite.getFolders(ServerSavedata.directory + "/" + Provider.serverID + "/Workshop/Maps");
          SteamGameServer.SetGameData((!(Provider.serverPassword != string.Empty) ? "SSAP" : "PASS") + "," + (Dedicator.security != ESteamSecurity.SECURE ? "CAV" : "VAC") + "," + Provider.APP_VERSION);
          SteamGameServer.SetGameTags((!Provider.isPvP ? "PVE" : "PVP") + (object) ',' + Provider.mode.ToString() + "," + Provider.camera.ToString() + "," + (folders1.Length > 0 || folders2.Length > 0 || (folders3.Length > 0 || folders4.Length > 0) ? "WORK" : "KROW"));
        }
        Provider._server = SteamGameServer.GetSteamID();
        Provider._client = Provider._server;
        Provider._clientHash = Hash.SHA1(Provider.client);
        if (Dedicator.isDedicated)
          Provider._clientName = Provider.localization.format("Console");
        Provider.lastNet = Time.realtimeSinceStartup;
        Provider.offsetNet = 0.0f;
      }
    }

    private static void closeGameServer()
    {
      if (!Provider.isServer)
      {
        UnityEngine.Debug.LogError((object) "Failed to close game server: no session in progress.");
      }
      else
      {
        if (Provider.onServerShutdown != null)
          Provider.onServerShutdown();
        Provider._isServer = false;
        Provider.provider.multiplayerService.serverMultiplayerService.close();
      }
    }

    public static void toggleFavorite()
    {
      if (Provider.isServer)
        return;
      if (Provider.isFavorited)
      {
        SteamMatchmaking.RemoveFavoriteGame(Provider.APP_ID, Provider.favoriteIP, Provider.favoritePort, (ushort) ((uint) Provider.favoritePort + 1U), 1U);
        Provider._isFavorited = false;
      }
      else
      {
        SteamMatchmaking.AddFavoriteGame(Provider.APP_ID, Provider.favoriteIP, Provider.favoritePort, (ushort) ((uint) Provider.favoritePort + 1U), 1U, SteamUtils.GetServerRealTime());
        Provider._isFavorited = true;
      }
    }

    private static void onPersonaStateChange(PersonaStateChange_t callback)
    {
      if (callback.m_nChangeFlags != EPersonaChange.k_EPersonaChangeName || (long) callback.m_ulSteamID != (long) Provider.client.m_SteamID)
        return;
      Provider._clientName = SteamFriends.GetPersonaName();
    }

    private static void onGameServerChangeRequested(GameServerChangeRequested_t callback)
    {
      if (Provider.isConnected)
        return;
      MenuPlayConnectUI.connect(new SteamConnectionInfo(callback.m_rgchServer, callback.m_rgchPassword));
    }

    private static void onGameRichPresenceJoinRequested(GameRichPresenceJoinRequested_t callback)
    {
      uint ip;
      ushort port;
      string pass;
      if (Provider.isConnected || !CommandLine.tryGetConnect(callback.m_rgchConnect, out ip, out port, out pass))
        return;
      MenuPlayConnectUI.connect(new SteamConnectionInfo(ip, port, pass));
    }

    private static void lag(float value)
    {
      Provider._ping = value;
      for (int index = Provider.pings.Length - 1; index > 0; --index)
      {
        Provider.pings[index] = Provider.pings[index - 1];
        if ((double) Provider.pings[index] > 1.0 / 1000.0)
          Provider._ping += Provider.pings[index];
      }
      Provider._ping /= (float) Provider.pings.Length;
      Provider.pings[0] = value;
    }

    private static byte[] openTicket()
    {
      if (Provider.ticketHandle != HAuthTicket.Invalid)
        return (byte[]) null;
      byte[] pTicket = new byte[1024];
      uint pcbTicket;
      Provider.ticketHandle = SteamUser.GetAuthSessionTicket(pTicket, pTicket.Length, out pcbTicket);
      if ((int) pcbTicket == 0)
        return (byte[]) null;
      byte[] numArray = new byte[(IntPtr) pcbTicket];
      Buffer.BlockCopy((Array) pTicket, 0, (Array) numArray, 0, (int) pcbTicket);
      return numArray;
    }

    private static void closeTicket()
    {
      if (Provider.ticketHandle == HAuthTicket.Invalid)
        return;
      SteamUser.CancelAuthTicket(Provider.ticketHandle);
      Provider.ticketHandle = HAuthTicket.Invalid;
    }

    [DebuggerHidden]
    public IEnumerator close(CSteamID steamID)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Provider.\u003Cclose\u003Ec__Iterator2()
      {
        steamID = steamID,
        \u003C\u0024\u003EsteamID = steamID
      };
    }

    private void exit()
    {
      Application.Quit();
    }

    private static void onAPIWarningMessage(int severity, StringBuilder warning)
    {
      CommandWindow.LogWarning((object) "Steam API Warning Message:");
      CommandWindow.LogWarning((object) ("Severity: " + (object) severity));
      CommandWindow.LogWarning((object) ("Warning: " + (object) warning));
    }

    private void Update()
    {
      if (!Provider.isInitialized)
        return;
      if (Provider.isConnected)
        Provider.listen();
      Provider.provider.update();
    }

    private void Awake()
    {
      UnityEngine.Debug.Log((object) Provider.APP_VERSION);
      if (Provider.isInitialized)
      {
        UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
      }
      else
      {
        Provider._isInitialized = true;
        UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) this.gameObject);
        Provider.steam = this;
        Level.onLevelLoaded += new LevelLoaded(Provider.onLevelLoaded);
        if (Dedicator.isDedicated)
        {
          try
          {
            Provider.provider = (IProvider) new SteamworksProvider(new SteamworksAppInfo(Provider.APP_ID.m_AppId, Provider.APP_NAME, Provider.APP_VERSION, true));
            Provider.provider.intialize();
          }
          catch (Exception ex)
          {
            UnityEngine.Debug.LogError((object) ex.Message);
            Application.Quit();
            return;
          }
          if (!CommandLine.tryGetLanguage(out Provider._language, out Provider._path))
          {
            Provider._path = ReadWrite.PATH + "/Localization/";
            Provider._language = "English";
          }
          Provider.localization = Localization.read("/Server/ServerConsole.dat");
          Provider.gsPolicyResponse = Callback<GSPolicyResponse_t>.CreateGameServer(new Callback<GSPolicyResponse_t>.DispatchDelegate(Provider.onGSPolicyResponse));
          Provider.p2pSessionConnectFail = Callback<P2PSessionConnectFail_t>.CreateGameServer(new Callback<P2PSessionConnectFail_t>.DispatchDelegate(Provider.onP2PSessionConnectFail));
          Provider.validateAuthTicketResponse = Callback<ValidateAuthTicketResponse_t>.CreateGameServer(new Callback<ValidateAuthTicketResponse_t>.DispatchDelegate(Provider.onValidateAuthTicketResponse));
          Provider._isPro = true;
          CommandWindow.Log((object) Provider.APP_VERSION);
          Provider.maxPlayers = (byte) 8;
          Provider.serverName = "Unturned";
          Provider.serverPassword = string.Empty;
          Provider.ip = 0U;
          Provider.port = (ushort) 27015;
          Provider.map = "PEI";
          Provider.isPvP = true;
          Provider.filterName = false;
          Provider.mode = EGameMode.NORMAL;
          Provider.camera = ECameraMode.FIRST;
          Provider.timeout = 0.75f;
          if (Dedicator.security != ESteamSecurity.SECURE)
            CommandWindow.LogWarning((object) Provider.localization.format("InsecureWarningText"));
          Commander.init();
          SteamWhitelist.load();
          SteamBlacklist.load();
          SteamAdminlist.load();
          foreach (string command in CommandLine.getCommands())
            Commander.execute(CSteamID.Nil, command);
          if (ServerSavedata.fileExists("/Server/Commands.dat"))
          {
            foreach (string command in ServerSavedata.readData("/Server/Commands.dat").getLines())
              Commander.execute(CSteamID.Nil, command);
          }
          else
            ServerSavedata.writeData("/Server/Commands.dat", new Data());
          if (!ServerSavedata.folderExists("/Workshop/Content"))
            ServerSavedata.createFolder("/Workshop/Content");
          if (ServerSavedata.folderExists("/Workshop/Maps"))
            return;
          ServerSavedata.createFolder("/Workshop/Maps");
        }
        else
        {
          try
          {
            Provider.provider = (IProvider) new SteamworksProvider(new SteamworksAppInfo(Provider.APP_ID.m_AppId, Provider.APP_NAME, Provider.APP_VERSION, false));
            Provider.provider.intialize();
          }
          catch (Exception ex)
          {
            UnityEngine.Debug.LogError((object) ex.Message);
            Application.Quit();
            return;
          }
          Provider.apiWarningMessageHook = new SteamAPIWarningMessageHook_t(Provider.onAPIWarningMessage);
          SteamUtils.SetWarningMessageHook(Provider.apiWarningMessageHook);
          Provider._time = SteamUtils.GetServerRealTime();
          Provider.personaStateChange = Callback<PersonaStateChange_t>.Create(new Callback<PersonaStateChange_t>.DispatchDelegate(Provider.onPersonaStateChange));
          Provider.gameServerChangeRequested = Callback<GameServerChangeRequested_t>.Create(new Callback<GameServerChangeRequested_t>.DispatchDelegate(Provider.onGameServerChangeRequested));
          Provider.gameRichPresenceJoinRequested = Callback<GameRichPresenceJoinRequested_t>.Create(new Callback<GameRichPresenceJoinRequested_t>.DispatchDelegate(Provider.onGameRichPresenceJoinRequested));
          Provider._user = SteamUser.GetSteamID();
          Provider._client = Provider.user;
          Provider._clientHash = Hash.SHA1(Provider.client);
          Provider._clientName = SteamFriends.GetPersonaName();
          Provider.provider.statisticsService.userStatisticsService.requestStatistics();
          Provider.provider.statisticsService.globalStatisticsService.requestStatistics();
          Provider.provider.workshopService.refreshUGC();
          Provider.provider.workshopService.refreshPublished();
          Provider._isPro = SteamApps.BIsSubscribedApp(Provider.PRO_ID);
          Provider.isLoadingInventory = true;
          SteamInventory.GrantPromoItems(out Provider.provider.economyService.promoResult);
          if (!CommandLine.tryGetLanguage(out Provider._language, out Provider._path))
          {
            string steamUiLanguage = SteamUtils.GetSteamUILanguage();
            Provider._language = steamUiLanguage.Substring(0, 1).ToUpper() + steamUiLanguage.Substring(1, steamUiLanguage.Length - 1).ToLower();
            bool flag = false;
            for (int index = 0; index < Provider.provider.workshopService.ugc.Count; ++index)
            {
              SteamContent steamContent = Provider.provider.workshopService.ugc[index];
              if (steamContent.type == ESteamUGCType.LOCALIZATION && ReadWrite.folderExists(steamContent.path + "/" + steamUiLanguage, false))
              {
                Provider._path = steamContent.path + "/";
                flag = true;
                break;
              }
            }
            if (!flag)
            {
              Provider._path = ReadWrite.PATH + "/Localization/";
              if (!ReadWrite.folderExists("/Localization/" + Provider.language))
                Provider._language = "English";
            }
          }
          Provider.localization = Localization.read("/Server/ServerConsole.dat");
          Provider.provider.communityService.setStatus("Menu");
        }
      }
    }

    private void OnApplicationQuit()
    {
      if (!Provider.isInitialized)
        return;
      Provider.disconnect();
      Provider.provider.shutdown();
    }

    public delegate void ServerConnected(CSteamID steamID);

    public delegate void ServerDisconnected(CSteamID steamID);

    public delegate void ServerHosted();

    public delegate void ServerShutdown();

    public delegate void ClientConnected();

    public delegate void ClientDisconnected();

    public delegate void EnemyConnected(SteamPlayer player);

    public delegate void EnemyDisconnected(SteamPlayer player);
  }
}
