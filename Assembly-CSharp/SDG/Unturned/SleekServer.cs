// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.SleekServer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class SleekServer : Sleek
  {
    private SteamServerInfo info;
    private SleekButton button;
    private SleekBox playersBox;
    private SleekBox pingBox;
    public ClickedServer onClickedServer;

    public SleekServer(SteamServerInfo newInfo)
    {
      this.init();
      this.info = newInfo;
      this.button = new SleekButton();
      this.button.sizeOffset_X = -220;
      this.button.sizeScale_X = 1f;
      this.button.sizeScale_Y = 1f;
      this.button.text = MenuPlayServersUI.localization.format("Server_Button", (object) this.info.name, (object) this.info.map);
      this.button.onClickedButton = new ClickedButton(this.onClickedButton);
      this.add((Sleek) this.button);
      this.playersBox = new SleekBox();
      this.playersBox.positionOffset_X = 10;
      this.playersBox.positionScale_X = 1f;
      this.playersBox.sizeOffset_X = 100;
      this.playersBox.sizeScale_Y = 1f;
      this.playersBox.text = MenuPlayServersUI.localization.format("Server_Players", (object) this.info.players, (object) this.info.maxPlayers);
      this.button.add((Sleek) this.playersBox);
      this.pingBox = new SleekBox();
      this.pingBox.positionOffset_X = 120;
      this.pingBox.positionScale_X = 1f;
      this.pingBox.sizeOffset_X = 100;
      this.pingBox.sizeScale_Y = 1f;
      this.pingBox.text = this.info.ping.ToString();
      this.button.add((Sleek) this.pingBox);
      if (this.info.isPassworded)
      {
        SleekImageTexture sleekImageTexture = new SleekImageTexture();
        sleekImageTexture.positionOffset_X = 5;
        sleekImageTexture.positionOffset_Y = 5;
        sleekImageTexture.sizeOffset_X = 20;
        sleekImageTexture.sizeOffset_Y = 20;
        sleekImageTexture.texture = (Texture) MenuPlayServersUI.icons.load("Lock");
        this.button.add((Sleek) sleekImageTexture);
      }
      if (this.info.isWorkshop)
      {
        SleekImageTexture sleekImageTexture = new SleekImageTexture();
        sleekImageTexture.positionOffset_X = 35;
        sleekImageTexture.positionOffset_Y = 5;
        sleekImageTexture.sizeOffset_X = 20;
        sleekImageTexture.sizeOffset_Y = 20;
        sleekImageTexture.texture = (Texture) MenuPlayServersUI.icons.load("Workshop");
        this.button.add((Sleek) sleekImageTexture);
      }
      if (this.info.camera == ECameraMode.FIRST)
      {
        SleekImageTexture sleekImageTexture = new SleekImageTexture();
        sleekImageTexture.positionOffset_X = -85;
        sleekImageTexture.positionOffset_Y = 5;
        sleekImageTexture.positionScale_X = 1f;
        sleekImageTexture.sizeOffset_X = 20;
        sleekImageTexture.sizeOffset_Y = 20;
        sleekImageTexture.texture = (Texture) MenuPlayServersUI.icons.load("First");
        this.button.add((Sleek) sleekImageTexture);
      }
      else if (this.info.camera == ECameraMode.THIRD)
      {
        SleekImageTexture sleekImageTexture = new SleekImageTexture();
        sleekImageTexture.positionOffset_X = -85;
        sleekImageTexture.positionOffset_Y = 5;
        sleekImageTexture.positionScale_X = 1f;
        sleekImageTexture.sizeOffset_X = 20;
        sleekImageTexture.sizeOffset_Y = 20;
        sleekImageTexture.texture = (Texture) MenuPlayServersUI.icons.load("Third");
        this.button.add((Sleek) sleekImageTexture);
      }
      else if (this.info.camera == ECameraMode.BOTH)
      {
        SleekImageTexture sleekImageTexture = new SleekImageTexture();
        sleekImageTexture.positionOffset_X = -85;
        sleekImageTexture.positionOffset_Y = 5;
        sleekImageTexture.positionScale_X = 1f;
        sleekImageTexture.sizeOffset_X = 20;
        sleekImageTexture.sizeOffset_Y = 20;
        sleekImageTexture.texture = (Texture) MenuPlayServersUI.icons.load("Both");
        this.button.add((Sleek) sleekImageTexture);
      }
      if (this.info.isPvP)
      {
        SleekImageTexture sleekImageTexture = new SleekImageTexture();
        sleekImageTexture.positionOffset_X = -55;
        sleekImageTexture.positionOffset_Y = 5;
        sleekImageTexture.positionScale_X = 1f;
        sleekImageTexture.sizeOffset_X = 20;
        sleekImageTexture.sizeOffset_Y = 20;
        sleekImageTexture.texture = (Texture) MenuPlayServersUI.icons.load("PvP");
        this.button.add((Sleek) sleekImageTexture);
      }
      else
      {
        SleekImageTexture sleekImageTexture = new SleekImageTexture();
        sleekImageTexture.positionOffset_X = -55;
        sleekImageTexture.positionOffset_Y = 5;
        sleekImageTexture.positionScale_X = 1f;
        sleekImageTexture.sizeOffset_X = 20;
        sleekImageTexture.sizeOffset_Y = 20;
        sleekImageTexture.texture = (Texture) MenuPlayServersUI.icons.load("PvE");
        this.button.add((Sleek) sleekImageTexture);
      }
      if (this.info.isSecure)
      {
        SleekImageTexture sleekImageTexture = new SleekImageTexture();
        sleekImageTexture.positionOffset_X = -25;
        sleekImageTexture.positionOffset_Y = 5;
        sleekImageTexture.positionScale_X = 1f;
        sleekImageTexture.sizeOffset_X = 20;
        sleekImageTexture.sizeOffset_Y = 20;
        sleekImageTexture.texture = (Texture) MenuPlayServersUI.icons.load("Secure");
        this.button.add((Sleek) sleekImageTexture);
      }
      else
      {
        SleekImageTexture sleekImageTexture = new SleekImageTexture();
        sleekImageTexture.positionOffset_X = -25;
        sleekImageTexture.positionOffset_Y = 5;
        sleekImageTexture.positionScale_X = 1f;
        sleekImageTexture.sizeOffset_X = 20;
        sleekImageTexture.sizeOffset_Y = 20;
        sleekImageTexture.texture = (Texture) MenuPlayServersUI.icons.load("Insecure");
        this.button.add((Sleek) sleekImageTexture);
      }
      if (this.info.mode == EGameMode.EASY)
      {
        this.button.backgroundColor = Palette.COLOR_G;
        this.button.foregroundColor = Palette.COLOR_G;
        this.playersBox.backgroundColor = Palette.COLOR_G;
        this.playersBox.foregroundColor = Palette.COLOR_G;
        this.pingBox.backgroundColor = Palette.COLOR_G;
        this.pingBox.foregroundColor = Palette.COLOR_G;
      }
      else if (this.info.mode == EGameMode.HARD)
      {
        this.button.backgroundColor = Palette.COLOR_R;
        this.button.foregroundColor = Palette.COLOR_R;
        this.playersBox.backgroundColor = Palette.COLOR_R;
        this.playersBox.foregroundColor = Palette.COLOR_R;
        this.pingBox.backgroundColor = Palette.COLOR_R;
        this.pingBox.foregroundColor = Palette.COLOR_R;
      }
      else
      {
        if (this.info.mode != EGameMode.PRO)
          return;
        this.button.backgroundColor = Palette.PRO;
        this.button.foregroundColor = Palette.PRO;
        this.playersBox.backgroundColor = Palette.PRO;
        this.playersBox.foregroundColor = Palette.PRO;
        this.pingBox.backgroundColor = Palette.PRO;
        this.pingBox.foregroundColor = Palette.PRO;
        if (Provider.isPro)
          return;
        Bundle bundle = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Pro/Pro.unity3d");
        SleekImageTexture sleekImageTexture = new SleekImageTexture();
        sleekImageTexture.positionOffset_X = -10;
        sleekImageTexture.positionOffset_Y = 5;
        sleekImageTexture.positionScale_X = 0.5f;
        sleekImageTexture.sizeOffset_X = 20;
        sleekImageTexture.sizeOffset_Y = 20;
        sleekImageTexture.texture = (Texture) bundle.load("Lock_Small");
        this.button.add((Sleek) sleekImageTexture);
        bundle.unload();
      }
    }

    public override void draw(bool ignoreCulling)
    {
      this.drawChildren(ignoreCulling);
    }

    private void onClickedButton(SleekButton button)
    {
      if (this.onClickedServer == null)
        return;
      this.onClickedServer(this, this.info);
    }
  }
}
