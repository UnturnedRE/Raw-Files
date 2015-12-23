// Decompiled with JetBrains decompiler
// Type: SDG.Provider.Services.Multiplayer.Client.IClientMultiplayerService
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using SDG.Provider.Services;
using SDG.Provider.Services.Community;
using SDG.Provider.Services.Multiplayer;
using System;
using System.IO;

namespace SDG.Provider.Services.Multiplayer.Client
{
  public interface IClientMultiplayerService : IService
  {
    IServerInfo serverInfo { get; }

    bool isConnected { get; }

    bool isAttempting { get; }

    MemoryStream stream { get; }

    BinaryReader reader { get; }

    BinaryWriter writer { get; }

    void connect(IServerInfo newServerInfo);

    void disconnect();

    bool read(out ICommunityEntity entity, byte[] data, out ulong length, int channel);

    void write(ICommunityEntity entity, byte[] data, ulong length);

    [Obsolete("Used by old multiplayer code, please use send without method instead.")]
    void write(ICommunityEntity entity, byte[] data, ulong length, ESendMethod method, int channel);
  }
}
