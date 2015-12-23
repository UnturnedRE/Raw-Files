// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.MenuPlayUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class MenuPlayUI
  {
    private static Sleek container;
    public static bool active;
    private static SleekButtonIcon connectButton;
    private static SleekButtonIcon serversButton;
    private static SleekButtonIcon singleplayerButton;

    public MenuPlayUI()
    {
      Local local = Localization.read("/Menu/Play/MenuPlay.dat");
      Bundle bundle = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Play/MenuPlay/MenuPlay.unity3d");
      MenuPlayUI.container = new Sleek();
      MenuPlayUI.container.positionOffset_X = 10;
      MenuPlayUI.container.positionOffset_Y = 10;
      MenuPlayUI.container.positionScale_Y = -1f;
      MenuPlayUI.container.sizeOffset_X = -20;
      MenuPlayUI.container.sizeOffset_Y = -20;
      MenuPlayUI.container.sizeScale_X = 1f;
      MenuPlayUI.container.sizeScale_Y = 1f;
      MenuUI.container.add(MenuPlayUI.container);
      MenuPlayUI.active = false;
      MenuPlayUI.connectButton = new SleekButtonIcon((Texture2D) bundle.load("Connect"));
      MenuPlayUI.connectButton.positionOffset_X = -100;
      MenuPlayUI.connectButton.positionOffset_Y = 35;
      MenuPlayUI.connectButton.positionScale_X = 0.5f;
      MenuPlayUI.connectButton.positionScale_Y = 0.5f;
      MenuPlayUI.connectButton.sizeOffset_X = 200;
      MenuPlayUI.connectButton.sizeOffset_Y = 50;
      MenuPlayUI.connectButton.text = local.format("ConnectButtonText");
      MenuPlayUI.connectButton.tooltip = local.format("ConnectButtonTooltip");
      MenuPlayUI.connectButton.onClickedButton = new ClickedButton(MenuPlayUI.onClickedConnectButton);
      MenuPlayUI.connectButton.fontSize = 14;
      MenuPlayUI.container.add((Sleek) MenuPlayUI.connectButton);
      MenuPlayUI.serversButton = new SleekButtonIcon((Texture2D) bundle.load("Servers"));
      MenuPlayUI.serversButton.positionOffset_X = -100;
      MenuPlayUI.serversButton.positionOffset_Y = -25;
      MenuPlayUI.serversButton.positionScale_X = 0.5f;
      MenuPlayUI.serversButton.positionScale_Y = 0.5f;
      MenuPlayUI.serversButton.sizeOffset_X = 200;
      MenuPlayUI.serversButton.sizeOffset_Y = 50;
      MenuPlayUI.serversButton.text = local.format("ServersButtonText");
      MenuPlayUI.serversButton.tooltip = local.format("ServersButtonTooltip");
      MenuPlayUI.serversButton.onClickedButton = new ClickedButton(MenuPlayUI.onClickedServersButton);
      MenuPlayUI.serversButton.fontSize = 14;
      MenuPlayUI.container.add((Sleek) MenuPlayUI.serversButton);
      MenuPlayUI.singleplayerButton = new SleekButtonIcon((Texture2D) bundle.load("Singleplayer"));
      MenuPlayUI.singleplayerButton.positionOffset_X = -100;
      MenuPlayUI.singleplayerButton.positionOffset_Y = -85;
      MenuPlayUI.singleplayerButton.positionScale_X = 0.5f;
      MenuPlayUI.singleplayerButton.positionScale_Y = 0.5f;
      MenuPlayUI.singleplayerButton.sizeOffset_X = 200;
      MenuPlayUI.singleplayerButton.sizeOffset_Y = 50;
      MenuPlayUI.singleplayerButton.text = local.format("SingleplayerButtonText");
      MenuPlayUI.singleplayerButton.tooltip = local.format("SingleplayerButtonTooltip");
      MenuPlayUI.singleplayerButton.onClickedButton = new ClickedButton(MenuPlayUI.onClickedSingleplayerButton);
      MenuPlayUI.singleplayerButton.fontSize = 14;
      MenuPlayUI.container.add((Sleek) MenuPlayUI.singleplayerButton);
      bundle.unload();
      MenuPlayConnectUI menuPlayConnectUi = new MenuPlayConnectUI();
      MenuPlayServersUI menuPlayServersUi = new MenuPlayServersUI();
      MenuPlaySingleplayerUI playSingleplayerUi = new MenuPlaySingleplayerUI();
    }

    public static void open()
    {
      if (MenuPlayUI.active)
      {
        MenuPlayUI.close();
      }
      else
      {
        MenuPlayUI.active = true;
        MenuPlayUI.container.lerpPositionScale(0.0f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
      }
    }

    public static void close()
    {
      if (!MenuPlayUI.active)
        return;
      MenuPlayUI.active = false;
      MenuPlayUI.container.lerpPositionScale(0.0f, -1f, ESleekLerp.EXPONENTIAL, 20f);
    }

    private static void onClickedConnectButton(SleekButton button)
    {
      MenuPlayConnectUI.open();
      MenuPlayUI.close();
    }

    private static void onClickedServersButton(SleekButton button)
    {
      MenuPlayServersUI.open();
      MenuPlayUI.close();
    }

    private static void onClickedSingleplayerButton(SleekButton button)
    {
      MenuPlaySingleplayerUI.open();
      MenuPlayUI.close();
    }
  }
}
