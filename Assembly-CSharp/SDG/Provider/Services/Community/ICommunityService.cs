// Decompiled with JetBrains decompiler
// Type: SDG.Provider.Services.Community.ICommunityService
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using SDG.Provider.Services;
using SDG.Unturned;
using Steamworks;
using UnityEngine;

namespace SDG.Provider.Services.Community
{
  public interface ICommunityService : IService
  {
    void setStatus(string status);

    Texture2D getIcon(int id);

    Texture2D getIcon(CSteamID steamID);

    SteamGroup[] getGroups();

    bool checkGroup(CSteamID steamID);
  }
}
