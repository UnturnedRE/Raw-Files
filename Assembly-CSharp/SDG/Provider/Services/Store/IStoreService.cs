// Decompiled with JetBrains decompiler
// Type: SDG.Provider.Services.Store.IStoreService
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using SDG.Provider.Services;
using SDG.Provider.Services.Economy;

namespace SDG.Provider.Services.Store
{
  public interface IStoreService : IService
  {
    bool canOpenStore { get; }

    void open(IStorePackageID packageID);

    void open(IEconomyItemDefinition itemDefinitionID);
  }
}
