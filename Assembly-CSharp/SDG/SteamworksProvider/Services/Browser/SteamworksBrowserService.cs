// Decompiled with JetBrains decompiler
// Type: SDG.SteamworksProvider.Services.Browser.SteamworksBrowserService
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using SDG.Provider.Services;
using SDG.Provider.Services.Browser;
using Steamworks;

namespace SDG.SteamworksProvider.Services.Browser
{
  public class SteamworksBrowserService : Service, IBrowserService, IService
  {
    public bool canOpenBrowser
    {
      get
      {
        return SteamUtils.IsOverlayEnabled();
      }
    }

    public void open(string url)
    {
      SteamFriends.ActivateGameOverlayToWebPage(url);
    }
  }
}
