// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.SleekPlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
  public class SleekPlayer : Sleek
  {
    private SleekBoxIcon box;
    private SleekImageTexture icon;

    public SleekPlayer(SteamPlayer player)
    {
      this.init();
      this.box = new SleekBoxIcon(!Provider.isServer ? Provider.provider.communityService.getIcon(player.playerID.steamID) : Provider.provider.communityService.getIcon(Provider.user), 20);
      this.box.sizeScale_X = 1f;
      this.box.sizeScale_Y = 1f;
      if (Characters.active.group != CSteamID.Nil && player.playerID.group == Characters.active.group)
      {
        if (player.playerID.nickName != string.Empty && player.playerID.steamID != Provider.client)
          this.box.text = player.playerID.nickName;
        else
          this.box.text = player.playerID.characterName;
      }
      else
        this.box.text = player.playerID.characterName;
      this.box.tooltip = player.playerID.playerName;
      this.add((Sleek) this.box);
      if (player.isAdmin && !Provider.isServer)
      {
        this.box.backgroundColor = Palette.ADMIN;
        this.box.foregroundColor = Palette.ADMIN;
        this.icon = new SleekImageTexture();
        this.icon.positionOffset_X = -25;
        this.icon.positionOffset_Y = 5;
        this.icon.positionScale_X = 1f;
        this.icon.sizeOffset_X = 20;
        this.icon.sizeOffset_Y = 20;
        this.icon.texture = (Texture) PlayerDashboardInformationUI.icons.load("Admin");
        this.box.add((Sleek) this.icon);
      }
      else
      {
        if (!player.isPro)
          return;
        this.box.backgroundColor = Palette.PRO;
        this.box.foregroundColor = Palette.PRO;
        this.icon = new SleekImageTexture();
        this.icon.positionOffset_X = -25;
        this.icon.positionOffset_Y = 5;
        this.icon.positionScale_X = 1f;
        this.icon.sizeOffset_X = 20;
        this.icon.sizeOffset_Y = 20;
        this.icon.texture = (Texture) PlayerDashboardInformationUI.icons.load("Pro");
        this.box.add((Sleek) this.icon);
      }
    }

    public override void draw(bool ignoreCulling)
    {
      this.drawChildren(ignoreCulling);
    }
  }
}
