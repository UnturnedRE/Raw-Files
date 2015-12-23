// Decompiled with JetBrains decompiler
// Type: SDG.SteamworksProvider.Services.Store.SteamworksStoreService
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using SDG.Provider.Services;
using SDG.Provider.Services.Economy;
using SDG.Provider.Services.Store;
using SDG.SteamworksProvider;
using SDG.SteamworksProvider.Services.Economy;
using Steamworks;

namespace SDG.SteamworksProvider.Services.Store
{
  public class SteamworksStoreService : Service, IService, IStoreService
  {
    private SteamworksAppInfo appInfo;

    public bool canOpenStore
    {
      get
      {
        return SteamUtils.IsOverlayEnabled();
      }
    }

    public SteamworksStoreService(SteamworksAppInfo newAppInfo)
    {
      this.appInfo = newAppInfo;
    }

    public void open(IStorePackageID packageID)
    {
      SteamFriends.ActivateGameOverlayToStore(((SteamworksStorePackageID) packageID).appID, EOverlayToStoreFlag.k_EOverlayToStoreFlag_None);
    }

    public void open(IEconomyItemDefinition itemDefinitionID)
    {
      SteamFriends.ActivateGameOverlayToWebPage(string.Concat(new object[4]
      {
        (object) "http://store.steampowered.com/itemstore/",
        (object) this.appInfo.id,
        (object) "/detail/",
        (object) ((SteamworksEconomyItemDefinition) itemDefinitionID).steamItemDef
      }));
    }
  }
}
