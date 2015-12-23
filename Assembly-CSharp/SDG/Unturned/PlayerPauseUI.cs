// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.PlayerPauseUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class PlayerPauseUI
  {
    public static readonly float TIMER_LEAVE = 10f;
    private static Sleek container;
    public static Local localization;
    public static bool active;
    private static SleekButtonIcon returnButton;
    private static SleekButtonIcon optionsButton;
    private static SleekButtonIcon displayButton;
    private static SleekButtonIcon graphicsButton;
    private static SleekButtonIcon controlsButton;
    public static SleekButtonIcon exitButton;
    private static SleekButtonIconConfirm suicideButton;
    private static SleekBox serverBox;
    private static SleekButtonIcon favoriteButton;
    public static float lastLeave;

    public PlayerPauseUI()
    {
      PlayerPauseUI.localization = Localization.read("/Player/PlayerPause.dat");
      Bundle bundle = Bundles.getBundle("/Bundles/Textures/Player/Icons/PlayerPause/PlayerPause.unity3d");
      PlayerPauseUI.container = new Sleek();
      PlayerPauseUI.container.positionScale_Y = 1f;
      PlayerPauseUI.container.positionOffset_X = 10;
      PlayerPauseUI.container.positionOffset_Y = 10;
      PlayerPauseUI.container.sizeOffset_X = -20;
      PlayerPauseUI.container.sizeOffset_Y = -20;
      PlayerPauseUI.container.sizeScale_X = 1f;
      PlayerPauseUI.container.sizeScale_Y = 1f;
      PlayerUI.container.add(PlayerPauseUI.container);
      PlayerPauseUI.active = false;
      PlayerPauseUI.returnButton = new SleekButtonIcon((Texture2D) bundle.load("Return"));
      PlayerPauseUI.returnButton.positionOffset_X = -100;
      PlayerPauseUI.returnButton.positionOffset_Y = -205;
      PlayerPauseUI.returnButton.positionScale_X = 0.5f;
      PlayerPauseUI.returnButton.positionScale_Y = 0.5f;
      PlayerPauseUI.returnButton.sizeOffset_X = 200;
      PlayerPauseUI.returnButton.sizeOffset_Y = 50;
      PlayerPauseUI.returnButton.text = PlayerPauseUI.localization.format("Return_Button_Text");
      PlayerPauseUI.returnButton.tooltip = PlayerPauseUI.localization.format("Return_Button_Tooltip");
      PlayerPauseUI.returnButton.onClickedButton = new ClickedButton(PlayerPauseUI.onClickedReturnButton);
      PlayerPauseUI.returnButton.fontSize = 14;
      PlayerPauseUI.container.add((Sleek) PlayerPauseUI.returnButton);
      PlayerPauseUI.optionsButton = new SleekButtonIcon((Texture2D) bundle.load("Options"));
      PlayerPauseUI.optionsButton.positionOffset_X = -100;
      PlayerPauseUI.optionsButton.positionOffset_Y = -145;
      PlayerPauseUI.optionsButton.positionScale_X = 0.5f;
      PlayerPauseUI.optionsButton.positionScale_Y = 0.5f;
      PlayerPauseUI.optionsButton.sizeOffset_X = 200;
      PlayerPauseUI.optionsButton.sizeOffset_Y = 50;
      PlayerPauseUI.optionsButton.text = PlayerPauseUI.localization.format("Options_Button_Text");
      PlayerPauseUI.optionsButton.tooltip = PlayerPauseUI.localization.format("Options_Button_Tooltip");
      PlayerPauseUI.optionsButton.onClickedButton = new ClickedButton(PlayerPauseUI.onClickedOptionsButton);
      PlayerPauseUI.optionsButton.fontSize = 14;
      PlayerPauseUI.container.add((Sleek) PlayerPauseUI.optionsButton);
      PlayerPauseUI.displayButton = new SleekButtonIcon((Texture2D) bundle.load("Display"));
      PlayerPauseUI.displayButton.positionOffset_X = -100;
      PlayerPauseUI.displayButton.positionOffset_Y = -85;
      PlayerPauseUI.displayButton.positionScale_X = 0.5f;
      PlayerPauseUI.displayButton.positionScale_Y = 0.5f;
      PlayerPauseUI.displayButton.sizeOffset_X = 200;
      PlayerPauseUI.displayButton.sizeOffset_Y = 50;
      PlayerPauseUI.displayButton.text = PlayerPauseUI.localization.format("Display_Button_Text");
      PlayerPauseUI.displayButton.tooltip = PlayerPauseUI.localization.format("Display_Button_Tooltip");
      PlayerPauseUI.displayButton.onClickedButton = new ClickedButton(PlayerPauseUI.onClickedDisplayButton);
      PlayerPauseUI.displayButton.fontSize = 14;
      PlayerPauseUI.container.add((Sleek) PlayerPauseUI.displayButton);
      PlayerPauseUI.graphicsButton = new SleekButtonIcon((Texture2D) bundle.load("Graphics"));
      PlayerPauseUI.graphicsButton.positionOffset_X = -100;
      PlayerPauseUI.graphicsButton.positionOffset_Y = -25;
      PlayerPauseUI.graphicsButton.positionScale_X = 0.5f;
      PlayerPauseUI.graphicsButton.positionScale_Y = 0.5f;
      PlayerPauseUI.graphicsButton.sizeOffset_X = 200;
      PlayerPauseUI.graphicsButton.sizeOffset_Y = 50;
      PlayerPauseUI.graphicsButton.text = PlayerPauseUI.localization.format("Graphics_Button_Text");
      PlayerPauseUI.graphicsButton.tooltip = PlayerPauseUI.localization.format("Graphics_Button_Tooltip");
      PlayerPauseUI.graphicsButton.onClickedButton = new ClickedButton(PlayerPauseUI.onClickedGraphicsButton);
      PlayerPauseUI.graphicsButton.fontSize = 14;
      PlayerPauseUI.container.add((Sleek) PlayerPauseUI.graphicsButton);
      PlayerPauseUI.controlsButton = new SleekButtonIcon((Texture2D) bundle.load("Controls"));
      PlayerPauseUI.controlsButton.positionOffset_X = -100;
      PlayerPauseUI.controlsButton.positionOffset_Y = 35;
      PlayerPauseUI.controlsButton.positionScale_X = 0.5f;
      PlayerPauseUI.controlsButton.positionScale_Y = 0.5f;
      PlayerPauseUI.controlsButton.sizeOffset_X = 200;
      PlayerPauseUI.controlsButton.sizeOffset_Y = 50;
      PlayerPauseUI.controlsButton.text = PlayerPauseUI.localization.format("Controls_Button_Text");
      PlayerPauseUI.controlsButton.tooltip = PlayerPauseUI.localization.format("Controls_Button_Tooltip");
      PlayerPauseUI.controlsButton.onClickedButton = new ClickedButton(PlayerPauseUI.onClickedControlsButton);
      PlayerPauseUI.controlsButton.fontSize = 14;
      PlayerPauseUI.container.add((Sleek) PlayerPauseUI.controlsButton);
      PlayerPauseUI.exitButton = new SleekButtonIcon((Texture2D) bundle.load("Exit"));
      PlayerPauseUI.exitButton.positionOffset_X = -100;
      PlayerPauseUI.exitButton.positionOffset_Y = 155;
      PlayerPauseUI.exitButton.positionScale_X = 0.5f;
      PlayerPauseUI.exitButton.positionScale_Y = 0.5f;
      PlayerPauseUI.exitButton.sizeOffset_X = 200;
      PlayerPauseUI.exitButton.sizeOffset_Y = 50;
      PlayerPauseUI.exitButton.text = PlayerPauseUI.localization.format("Exit_Button_Text");
      PlayerPauseUI.exitButton.tooltip = PlayerPauseUI.localization.format("Exit_Button_Tooltip");
      PlayerPauseUI.exitButton.onClickedButton = new ClickedButton(PlayerPauseUI.onClickedExitButton);
      PlayerPauseUI.exitButton.fontSize = 14;
      PlayerPauseUI.container.add((Sleek) PlayerPauseUI.exitButton);
      PlayerPauseUI.suicideButton = new SleekButtonIconConfirm((Texture2D) bundle.load("Suicide"), PlayerPauseUI.localization.format("Suicide_Button_Confirm"), PlayerPauseUI.localization.format("Suicide_Button_Confirm_Tooltip"), PlayerPauseUI.localization.format("Suicide_Button_Deny"), PlayerPauseUI.localization.format("Suicide_Button_Deny_Tooltip"));
      PlayerPauseUI.suicideButton.positionOffset_X = -100;
      PlayerPauseUI.suicideButton.positionOffset_Y = 95;
      PlayerPauseUI.suicideButton.positionScale_X = 0.5f;
      PlayerPauseUI.suicideButton.positionScale_Y = 0.5f;
      PlayerPauseUI.suicideButton.sizeOffset_X = 200;
      PlayerPauseUI.suicideButton.sizeOffset_Y = 50;
      PlayerPauseUI.suicideButton.text = PlayerPauseUI.localization.format("Suicide_Button_Text");
      PlayerPauseUI.suicideButton.tooltip = PlayerPauseUI.localization.format("Suicide_Button_Tooltip");
      PlayerPauseUI.suicideButton.onConfirmed = new Confirm(PlayerPauseUI.onClickedSuicideButton);
      PlayerPauseUI.suicideButton.fontSize = 14;
      PlayerPauseUI.container.add((Sleek) PlayerPauseUI.suicideButton);
      PlayerPauseUI.serverBox = new SleekBox();
      PlayerPauseUI.serverBox.positionOffset_Y = -50;
      PlayerPauseUI.serverBox.positionScale_Y = 1f;
      PlayerPauseUI.serverBox.sizeOffset_X = -5;
      PlayerPauseUI.serverBox.sizeOffset_Y = 50;
      PlayerPauseUI.serverBox.sizeScale_X = 0.75f;
      PlayerPauseUI.serverBox.text = PlayerPauseUI.localization.format("Server", (object) Level.info.name, (object) Provider.currentServerInfo.name, (object) (!Provider.currentServerInfo.isSecure ? PlayerPauseUI.localization.format("Insecure") : PlayerPauseUI.localization.format("Secure")));
      PlayerPauseUI.serverBox.fontSize = 14;
      PlayerPauseUI.container.add((Sleek) PlayerPauseUI.serverBox);
      PlayerPauseUI.favoriteButton = new SleekButtonIcon((Texture2D) bundle.load("Favorite"));
      PlayerPauseUI.favoriteButton.positionScale_X = 0.75f;
      PlayerPauseUI.favoriteButton.positionOffset_Y = -50;
      PlayerPauseUI.favoriteButton.positionOffset_X = 5;
      PlayerPauseUI.favoriteButton.positionScale_Y = 1f;
      PlayerPauseUI.favoriteButton.sizeOffset_X = -5;
      PlayerPauseUI.favoriteButton.sizeOffset_Y = 50;
      PlayerPauseUI.favoriteButton.sizeScale_X = 0.25f;
      PlayerPauseUI.favoriteButton.tooltip = PlayerPauseUI.localization.format("Favorite_Button_Tooltip");
      PlayerPauseUI.favoriteButton.fontSize = 14;
      PlayerPauseUI.favoriteButton.onClickedButton = new ClickedButton(PlayerPauseUI.onClickedFavoriteButton);
      PlayerPauseUI.container.add((Sleek) PlayerPauseUI.favoriteButton);
      bundle.unload();
      MenuConfigurationOptionsUI configurationOptionsUi = new MenuConfigurationOptionsUI();
      MenuConfigurationDisplayUI configurationDisplayUi = new MenuConfigurationDisplayUI();
      MenuConfigurationGraphicsUI configurationGraphicsUi = new MenuConfigurationGraphicsUI();
      MenuConfigurationControlsUI configurationControlsUi = new MenuConfigurationControlsUI();
      PlayerPauseUI.updateFavorite();
    }

    public static void open()
    {
      if (PlayerPauseUI.active)
      {
        PlayerPauseUI.close();
      }
      else
      {
        PlayerPauseUI.active = true;
        PlayerPauseUI.lastLeave = Time.realtimeSinceStartup;
        PlayerPauseUI.container.lerpPositionScale(0.0f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
      }
    }

    public static void close()
    {
      if (!PlayerPauseUI.active)
        return;
      PlayerPauseUI.active = false;
      PlayerPauseUI.suicideButton.reset();
      MenuSettings.save();
      PlayerPauseUI.container.lerpPositionScale(0.0f, 1f, ESleekLerp.EXPONENTIAL, 20f);
    }

    private static void onClickedReturnButton(SleekButton button)
    {
      PlayerPauseUI.close();
      PlayerLifeUI.open();
    }

    private static void onClickedOptionsButton(SleekButton button)
    {
      PlayerPauseUI.close();
      MenuConfigurationOptionsUI.open();
    }

    private static void onClickedDisplayButton(SleekButton button)
    {
      PlayerPauseUI.close();
      MenuConfigurationDisplayUI.open();
    }

    private static void onClickedGraphicsButton(SleekButton button)
    {
      PlayerPauseUI.close();
      MenuConfigurationGraphicsUI.open();
    }

    private static void onClickedControlsButton(SleekButton button)
    {
      PlayerPauseUI.close();
      MenuConfigurationControlsUI.open();
    }

    private static void onClickedExitButton(SleekButton button)
    {
      if (!Provider.isServer && Provider.isPvP && (double) Time.realtimeSinceStartup - (double) PlayerPauseUI.lastLeave < (double) PlayerPauseUI.TIMER_LEAVE)
        return;
      Provider.disconnect();
    }

    private static void onClickedSuicideButton(SleekButton button)
    {
      PlayerPauseUI.close();
      Player.player.life.sendSuicide();
    }

    private static void onClickedFavoriteButton(SleekButton button)
    {
      Provider.toggleFavorite();
      PlayerPauseUI.updateFavorite();
    }

    private static void updateFavorite()
    {
      if (Provider.isFavorited)
        PlayerPauseUI.favoriteButton.text = PlayerPauseUI.localization.format("Unfavorite_Button_Text");
      else
        PlayerPauseUI.favoriteButton.text = PlayerPauseUI.localization.format("Favorite_Button_Text");
    }
  }
}
