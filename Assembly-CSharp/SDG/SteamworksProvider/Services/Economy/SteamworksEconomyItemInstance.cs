// Decompiled with JetBrains decompiler
// Type: SDG.SteamworksProvider.Services.Economy.SteamworksEconomyItemInstance
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using SDG.IO.Streams;
using SDG.Provider.Services.Economy;
using Steamworks;

namespace SDG.SteamworksProvider.Services.Economy
{
  public class SteamworksEconomyItemInstance : INetworkStreamable, IEconomyItemInstance
  {
    public SteamItemInstanceID_t steamItemInstanceID { get; protected set; }

    public SteamworksEconomyItemInstance(SteamItemInstanceID_t newSteamItemInstanceID)
    {
      this.steamItemInstanceID = newSteamItemInstanceID;
    }

    public void readFromStream(NetworkStream networkStream)
    {
      this.steamItemInstanceID = (SteamItemInstanceID_t) networkStream.readUInt64();
    }

    public void writeToStream(NetworkStream networkStream)
    {
      networkStream.writeUInt64((ulong) this.steamItemInstanceID);
    }
  }
}
