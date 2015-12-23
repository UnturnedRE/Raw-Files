// Decompiled with JetBrains decompiler
// Type: SDG.Provider.Services.Economy.EconomyRequestResult
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

namespace SDG.Provider.Services.Economy
{
  public class EconomyRequestResult : IEconomyRequestResult
  {
    public EEconomyRequestState economyRequestState { get; protected set; }

    public IEconomyItem[] items { get; protected set; }

    public EconomyRequestResult(EEconomyRequestState newEconomyRequestState, IEconomyItem[] newItems)
    {
      this.economyRequestState = newEconomyRequestState;
      this.items = newItems;
    }
  }
}
