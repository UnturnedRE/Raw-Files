// Decompiled with JetBrains decompiler
// Type: SDG.SteamworksProvider.Services.Translation.SteamworksTranslationService
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using SDG.Provider.Services;
using SDG.Provider.Services.Translation;
using Steamworks;

namespace SDG.SteamworksProvider.Services.Translation
{
  public class SteamworksTranslationService : Service, IService, ITranslationService
  {
    public string language { get; protected set; }

    public override void initialize()
    {
      this.language = SteamUtils.GetSteamUILanguage();
    }
  }
}
