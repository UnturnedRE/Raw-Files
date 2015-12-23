// Decompiled with JetBrains decompiler
// Type: SDG.SteamworksProvider.Services.Economy.SteamworksEconomyItemDefinition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using SDG.IO.Streams;
using SDG.Provider.Services.Economy;
using Steamworks;

namespace SDG.SteamworksProvider.Services.Economy
{
  public class SteamworksEconomyItemDefinition : INetworkStreamable, IEconomyItemDefinition
  {
    public SteamItemDef_t steamItemDef { get; protected set; }

    public SteamworksEconomyItemDefinition(SteamItemDef_t newSteamItemDef)
    {
      this.steamItemDef = newSteamItemDef;
    }

    public void readFromStream(NetworkStream networkStream)
    {
      this.steamItemDef = (SteamItemDef_t) networkStream.readInt32();
    }

    public void writeToStream(NetworkStream networkStream)
    {
      networkStream.writeInt32((int) this.steamItemDef);
    }

    public string getPropertyValue(string key)
    {
      uint punValueBufferSize = 1024U;
      string pchValueBuffer;
      SteamInventory.GetItemDefinitionProperty(this.steamItemDef, key, out pchValueBuffer, ref punValueBufferSize);
      return pchValueBuffer;
    }
  }
}
