// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.MenuPauseUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
  public class MenuPauseUI
  {
    private static Local localization;
    private static Sleek container;
    public static bool active;
    private static SleekButtonIcon returnButton;
    private static SleekButtonIcon exitButton;
    private static SleekButtonIcon reportButton;
    private static SleekButtonIcon trelloButton;

    public MenuPauseUI()
    {
      MenuPauseUI.localization = Localization.read("/Menu/MenuPause.dat");
      Bundle bundle = Bundles.getBundle("/Bundles/Textures/Menu/Icons/MenuPause/MenuPause.unity3d");
      MenuPauseUI.container = new Sleek();
      MenuPauseUI.container.positionOffset_X = 10;
      MenuPauseUI.container.positionOffset_Y = 10;
      MenuPauseUI.container.positionScale_Y = -1f;
      MenuPauseUI.container.sizeOffset_X = -20;
      MenuPauseUI.container.sizeOffset_Y = -20;
      MenuPauseUI.container.sizeScale_X = 1f;
      MenuPauseUI.container.sizeScale_Y = 1f;
      MenuUI.container.add(MenuPauseUI.container);
      MenuPauseUI.active = false;
      MenuPauseUI.returnButton = new SleekButtonIcon((Texture2D) bundle.load("Return"));
      MenuPauseUI.returnButton.positionOffset_X = -100;
      MenuPauseUI.returnButton.positionOffset_Y = -115;
      MenuPauseUI.returnButton.positionScale_X = 0.5f;
      MenuPauseUI.returnButton.positionScale_Y = 0.5f;
      MenuPauseUI.returnButton.sizeOffset_X = 200;
      MenuPauseUI.returnButton.sizeOffset_Y = 50;
      MenuPauseUI.returnButton.text = MenuPauseUI.localization.format("Return_Button");
      MenuPauseUI.returnButton.tooltip = MenuPauseUI.localization.format("Return_Button_Tooltip");
      MenuPauseUI.returnButton.onClickedButton = new ClickedButton(MenuPauseUI.onClickedReturnButton);
      MenuPauseUI.returnButton.fontSize = 14;
      MenuPauseUI.container.add((Sleek) MenuPauseUI.returnButton);
      MenuPauseUI.exitButton = new SleekButtonIcon((Texture2D) bundle.load("Exit"));
      MenuPauseUI.exitButton.positionOffset_X = -100;
      MenuPauseUI.exitButton.positionOffset_Y = 65;
      MenuPauseUI.exitButton.positionScale_X = 0.5f;
      MenuPauseUI.exitButton.positionScale_Y = 0.5f;
      MenuPauseUI.exitButton.sizeOffset_X = 200;
      MenuPauseUI.exitButton.sizeOffset_Y = 50;
      MenuPauseUI.exitButton.text = MenuPauseUI.localization.format("Exit_Button");
      MenuPauseUI.exitButton.tooltip = MenuPauseUI.localization.format("Exit_Button_Tooltip");
      MenuPauseUI.exitButton.onClickedButton = new ClickedButton(MenuPauseUI.onClickedExitButton);
      MenuPauseUI.exitButton.fontSize = 14;
      MenuPauseUI.container.add((Sleek) MenuPauseUI.exitButton);
      MenuPauseUI.reportButton = new SleekButtonIcon((Texture2D) bundle.load("Report"));
      MenuPauseUI.reportButton.positionOffset_X = -100;
      MenuPauseUI.reportButton.positionOffset_Y = -55;
      MenuPauseUI.reportButton.positionScale_X = 0.5f;
      MenuPauseUI.reportButton.positionScale_Y = 0.5f;
      MenuPauseUI.reportButton.sizeOffset_X = 200;
      MenuPauseUI.reportButton.sizeOffset_Y = 50;
      MenuPauseUI.reportButton.text = MenuPauseUI.localization.format("Report_Button");
      MenuPauseUI.reportButton.tooltip = MenuPauseUI.localization.format("Report_Button_Tooltip");
      MenuPauseUI.reportButton.onClickedButton = new ClickedButton(MenuPauseUI.onClickedReportButton);
      MenuPauseUI.reportButton.fontSize = 14;
      MenuPauseUI.container.add((Sleek) MenuPauseUI.reportButton);
      MenuPauseUI.trelloButton = new SleekButtonIcon((Texture2D) bundle.load("Trello"));
      MenuPauseUI.trelloButton.positionOffset_X = -100;
      MenuPauseUI.trelloButton.positionOffset_Y = 5;
      MenuPauseUI.trelloButton.positionScale_X = 0.5f;
      MenuPauseUI.trelloButton.positionScale_Y = 0.5f;
      MenuPauseUI.trelloButton.sizeOffset_X = 200;
      MenuPauseUI.trelloButton.sizeOffset_Y = 50;
      MenuPauseUI.trelloButton.text = MenuPauseUI.localization.format("Trello_Button");
      MenuPauseUI.trelloButton.tooltip = MenuPauseUI.localization.format("Trello_Button_Tooltip");
      MenuPauseUI.trelloButton.onClickedButton = new ClickedButton(MenuPauseUI.onClickedTrelloButton);
      MenuPauseUI.trelloButton.fontSize = 14;
      MenuPauseUI.container.add((Sleek) MenuPauseUI.trelloButton);
      bundle.unload();
    }

    public static void open()
    {
      if (MenuPauseUI.active)
      {
        MenuPauseUI.close();
      }
      else
      {
        MenuPauseUI.active = true;
        MenuPauseUI.container.lerpPositionScale(0.0f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
      }
    }

    public static void close()
    {
      if (!MenuPauseUI.active)
        return;
      MenuPauseUI.active = false;
      MenuPauseUI.container.lerpPositionScale(0.0f, -1f, ESleekLerp.EXPONENTIAL, 20f);
    }

    private static void onClickedReturnButton(SleekButton button)
    {
      MenuPauseUI.close();
      MenuDashboardUI.open();
      MenuTitleUI.open();
    }

    private static void onClickedExitButton(SleekButton button)
    {
      Application.Quit();
    }

    private static void onClickedReportButton(SleekButton button)
    {
      if (!Provider.provider.browserService.canOpenBrowser)
        MenuUI.alert(MenuPauseUI.localization.format("Overlay"));
      else
        Provider.provider.browserService.open("http://steamcommunity.com/app/" + (object) SteamUtils.GetAppID() + "/discussions/9/613936673439628788/");
    }

    private static void onClickedTrelloButton(SleekButton button)
    {
      if (!Provider.provider.browserService.canOpenBrowser)
        MenuUI.alert(MenuPauseUI.localization.format("Overlay"));
      else
        Provider.provider.browserService.open("http://trello.com/b/M2jI6d2e/unturned-roadmap");
    }
  }
}
