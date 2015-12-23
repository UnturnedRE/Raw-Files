// Decompiled with JetBrains decompiler
// Type: SDG.SteamworksProvider.Services.Economy.SteamworksEconomyItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using SDG.IO.Streams;
using SDG.Provider.Services.Economy;
using Steamworks;

namespace SDG.SteamworksProvider.Services.Economy
{
  public class SteamworksEconomyItem : INetworkStreamable, IEconomyItem
  {
    public SteamItemDetails_t steamItemDetail { get; protected set; }

    public IEconomyItemDefinition itemDefinitionID { get; protected set; }

    public IEconomyItemInstance itemInstanceID { get; protected set; }

    public SteamworksEconomyItem(SteamItemDetails_t newSteamItemDetail)
    {
      this.steamItemDetail = newSteamItemDetail;
      this.itemDefinitionID = (IEconomyItemDefinition) new SteamworksEconomyItemDefinition(this.steamItemDetail.m_iDefinition);
      this.itemInstanceID = (IEconomyItemInstance) new SteamworksEconomyItemInstance(this.steamItemDetail.m_itemId);
    }

    public void readFromStream(NetworkStream networkStream)
    {
      this.itemDefinitionID.readFromStream(networkStream);
      this.itemInstanceID.readFromStream(networkStream);
    }

    public void writeToStream(NetworkStream networkStream)
    {
      this.itemDefinitionID.writeToStream(networkStream);
      this.itemInstanceID.writeToStream(networkStream);
    }
  }
}
