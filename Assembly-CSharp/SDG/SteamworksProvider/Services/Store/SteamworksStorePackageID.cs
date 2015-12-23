// Decompiled with JetBrains decompiler
// Type: SDG.SteamworksProvider.Services.Store.SteamworksStorePackageID
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using SDG.Provider.Services.Store;
using Steamworks;

namespace SDG.SteamworksProvider.Services.Store
{
  public class SteamworksStorePackageID : IStorePackageID
  {
    public AppId_t appID { get; protected set; }

    public SteamworksStorePackageID(uint appID)
    {
      this.appID = new AppId_t(appID);
    }
  }
}
