// Decompiled with JetBrains decompiler
// Type: SDG.SteamworksProvider.Services.Achievements.SteamworksAchievementsService
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using SDG.Provider.Services;
using SDG.Provider.Services.Achievements;
using Steamworks;
using System;

namespace SDG.SteamworksProvider.Services.Achievements
{
  public class SteamworksAchievementsService : Service, IAchievementsService, IService
  {
    public bool getAchievement(string name, out bool has)
    {
      if (name == null)
        throw new ArgumentNullException("name");
      return SteamUserStats.GetAchievement(name, out has);
    }

    public bool setAchievement(string name)
    {
      if (name == null)
        throw new ArgumentNullException("name");
      bool flag = SteamUserStats.SetAchievement(name);
      SteamUserStats.StoreStats();
      return flag;
    }
  }
}
