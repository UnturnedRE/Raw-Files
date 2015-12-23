// Decompiled with JetBrains decompiler
// Type: SDG.SteamworksProvider.Services.Economy.SteamworksEconomyRequestHandle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using SDG.Provider.Services.Economy;
using Steamworks;

namespace SDG.SteamworksProvider.Services.Economy
{
  public class SteamworksEconomyRequestHandle : IEconomyRequestHandle
  {
    public SteamInventoryResult_t steamInventoryResult { get; protected set; }

    private EconomyRequestReadyCallback economyRequestReadyCallback { get; set; }

    public SteamworksEconomyRequestHandle(SteamInventoryResult_t newSteamInventoryResult, EconomyRequestReadyCallback newEconomyRequestReadyCallback)
    {
      this.steamInventoryResult = newSteamInventoryResult;
      this.economyRequestReadyCallback = newEconomyRequestReadyCallback;
    }

    public void triggerInventoryRequestReadyCallback(IEconomyRequestResult inventoryRequestResult)
    {
      if (this.economyRequestReadyCallback == null)
        return;
      this.economyRequestReadyCallback((IEconomyRequestHandle) this, inventoryRequestResult);
    }
  }
}
