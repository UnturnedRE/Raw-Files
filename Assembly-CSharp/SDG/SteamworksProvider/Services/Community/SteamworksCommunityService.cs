// Decompiled with JetBrains decompiler
// Type: SDG.SteamworksProvider.Services.Community.SteamworksCommunityService
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using SDG.Provider.Services;
using SDG.Provider.Services.Community;
using SDG.Unturned;
using Steamworks;
using System;
using UnityEngine;

namespace SDG.SteamworksProvider.Services.Community
{
  public class SteamworksCommunityService : Service, ICommunityService, IService
  {
    public void setStatus(string status)
    {
      SteamFriends.SetRichPresence("status", status);
    }

    public Texture2D getIcon(int id)
    {
      uint pnWidth;
      uint pnHeight;
      if (id == -1 || !SteamUtils.GetImageSize(id, out pnWidth, out pnHeight))
        return (Texture2D) null;
      Texture2D texture2D1 = new Texture2D((int) pnWidth, (int) pnHeight, TextureFormat.RGBA32, false);
      texture2D1.name = "Texture";
      texture2D1.hideFlags = HideFlags.HideAndDontSave;
      byte[] numArray = new byte[(IntPtr) (uint) ((int) pnWidth * (int) pnHeight * 4)];
      if (!SteamUtils.GetImageRGBA(id, numArray, numArray.Length))
        return (Texture2D) null;
      texture2D1.LoadRawTextureData(numArray);
      texture2D1.Apply();
      Texture2D texture2D2 = new Texture2D((int) pnWidth, (int) pnHeight, TextureFormat.RGBA32, false, true);
      texture2D2.name = "Texture";
      texture2D2.hideFlags = HideFlags.HideAndDontSave;
      for (int y = 0; (long) y < (long) pnHeight; ++y)
        texture2D2.SetPixels(0, y, (int) pnWidth, 1, texture2D1.GetPixels(0, (int) pnHeight - 1 - y, (int) pnWidth, 1));
      texture2D2.Apply();
      return texture2D2;
    }

    public Texture2D getIcon(CSteamID steamID)
    {
      return this.getIcon(SteamFriends.GetSmallFriendAvatar(steamID));
    }

    public SteamGroup[] getGroups()
    {
      SteamGroup[] steamGroupArray = new SteamGroup[SteamFriends.GetClanCount()];
      for (int iClan = 0; iClan < steamGroupArray.Length; ++iClan)
      {
        CSteamID clanByIndex = SteamFriends.GetClanByIndex(iClan);
        string clanName = SteamFriends.GetClanName(clanByIndex);
        Texture2D icon = this.getIcon(clanByIndex);
        steamGroupArray[iClan] = new SteamGroup(clanByIndex, clanName, icon);
      }
      return steamGroupArray;
    }

    public bool checkGroup(CSteamID steamID)
    {
      for (int iClan = 0; iClan < SteamFriends.GetClanCount(); ++iClan)
      {
        if (SteamFriends.GetClanByIndex(iClan) == steamID)
          return true;
      }
      return false;
    }
  }
}
