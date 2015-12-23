// Decompiled with JetBrains decompiler
// Type: SDG.SteamworksProvider.Services.Workshop.SteamworksWorkshopService
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using SDG.Provider.Services;
using SDG.Provider.Services.Workshop;
using Steamworks;

namespace SDG.SteamworksProvider.Services.Workshop
{
  public class SteamworksWorkshopService : Service, IService, IWorkshopService
  {
    public bool canOpenWorkshop
    {
      get
      {
        return SteamUtils.IsOverlayEnabled();
      }
    }

    public void open(PublishedFileId_t id)
    {
      SteamFriends.ActivateGameOverlayToWebPage("http://steamcommunity.com/sharedfiles/filedetails/?id=" + (object) id.m_PublishedFileId);
    }
  }
}
