// Decompiled with JetBrains decompiler
// Type: SDG.Provider.Services.Economy.IEconomyService
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using SDG.Provider.Services;

namespace SDG.Provider.Services.Economy
{
  public interface IEconomyService : IService
  {
    bool canOpenInventory { get; }

    IEconomyRequestHandle requestInventory(EconomyRequestReadyCallback economyRequestReadyCallback);

    IEconomyRequestHandle requestPromo(EconomyRequestReadyCallback economyRequestReadyCallback);

    IEconomyRequestHandle exchangeItems(IEconomyItemInstance[] inputItemInstanceIDs, uint[] inputItemQuantities, IEconomyItemDefinition[] outputItemDefinitionIDs, uint[] outputItemQuantities, EconomyRequestReadyCallback economyRequestReadyCallback);

    void open(ulong id);
  }
}
