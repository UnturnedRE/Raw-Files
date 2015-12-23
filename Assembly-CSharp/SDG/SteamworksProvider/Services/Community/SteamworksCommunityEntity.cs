// Decompiled with JetBrains decompiler
// Type: SDG.SteamworksProvider.Services.Community.SteamworksCommunityEntity
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using SDG.IO.Streams;
using SDG.Provider.Services.Community;
using Steamworks;

namespace SDG.SteamworksProvider.Services.Community
{
  public class SteamworksCommunityEntity : INetworkStreamable, ICommunityEntity
  {
    public static readonly SteamworksCommunityEntity INVALID = new SteamworksCommunityEntity(CSteamID.Nil);

    public bool isValid
    {
      get
      {
        return this.steamID.IsValid();
      }
    }

    public CSteamID steamID { get; protected set; }

    public SteamworksCommunityEntity(CSteamID newSteamID)
    {
      this.steamID = newSteamID;
    }

    public void readFromStream(NetworkStream networkStream)
    {
      this.steamID = (CSteamID) networkStream.readUInt64();
    }

    public void writeToStream(NetworkStream networkStream)
    {
      networkStream.writeUInt64((ulong) this.steamID);
    }
  }
}
