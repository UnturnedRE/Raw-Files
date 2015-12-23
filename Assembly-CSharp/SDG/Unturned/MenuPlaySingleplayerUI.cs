// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.MenuPlaySingleplayerUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class MenuPlaySingleplayerUI
  {
    public static Local localization;
    private static Sleek container;
    public static bool active;
    private static LevelInfo[] levels;
    private static SleekScrollBox levelScrollBox;
    private static SleekLevel[] levelButtons;
    private static SleekButtonIcon playButton;
    private static SleekButtonState modeButtonState;
    private static SleekButtonIconConfirm resetButton;
    private static SleekBox selectedBox;

    public MenuPlaySingleplayerUI()
    {
      MenuPlaySingleplayerUI.localization = Localization.read("/Menu/Play/MenuPlaySingleplayer.dat");
      Bundle bundle = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Play/MenuPlaySingleplayer/MenuPlaySingleplayer.unity3d");
      MenuPlaySingleplayerUI.container = new Sleek();
      MenuPlaySingleplayerUI.container.positionOffset_X = 10;
      MenuPlaySingleplayerUI.container.positionOffset_Y = 10;
      MenuPlaySingleplayerUI.container.positionScale_Y = 1f;
      MenuPlaySingleplayerUI.container.sizeOffset_X = -20;
      MenuPlaySingleplayerUI.container.sizeOffset_Y = -20;
      MenuPlaySingleplayerUI.container.sizeScale_X = 1f;
      MenuPlaySingleplayerUI.container.sizeScale_Y = 1f;
      MenuUI.container.add(MenuPlaySingleplayerUI.container);
      MenuPlaySingleplayerUI.active = false;
      MenuPlaySingleplayerUI.levelScrollBox = new SleekScrollBox();
      MenuPlaySingleplayerUI.levelScrollBox.positionOffset_X = -200;
      MenuPlaySingleplayerUI.levelScrollBox.positionOffset_Y = 140;
      MenuPlaySingleplayerUI.levelScrollBox.positionScale_X = 0.75f;
      MenuPlaySingleplayerUI.levelScrollBox.sizeOffset_X = 430;
      MenuPlaySingleplayerUI.levelScrollBox.sizeOffset_Y = -140;
      MenuPlaySingleplayerUI.levelScrollBox.sizeScale_Y = 1f;
      MenuPlaySingleplayerUI.levelScrollBox.area = new Rect(0.0f, 0.0f, 5f, 0.0f);
      MenuPlaySingleplayerUI.container.add((Sleek) MenuPlaySingleplayerUI.levelScrollBox);
      MenuPlaySingleplayerUI.selectedBox = new SleekBox();
      MenuPlaySingleplayerUI.selectedBox.positionOffset_X = -200;
      MenuPlaySingleplayerUI.selectedBox.positionOffset_Y = 100;
      MenuPlaySingleplayerUI.selectedBox.positionScale_X = 0.75f;
      MenuPlaySingleplayerUI.selectedBox.sizeOffset_X = 400;
      MenuPlaySingleplayerUI.selectedBox.sizeOffset_Y = 30;
      MenuPlaySingleplayerUI.selectedBox.addLabel(MenuPlaySingleplayerUI.localization.format("Selection_Label"), ESleekSide.LEFT);
      MenuPlaySingleplayerUI.container.add((Sleek) MenuPlaySingleplayerUI.selectedBox);
      MenuPlaySingleplayerUI.playButton = new SleekButtonIcon((Texture2D) bundle.load("Play"));
      MenuPlaySingleplayerUI.playButton.positionOffset_X = -100;
      MenuPlaySingleplayerUI.playButton.positionOffset_Y = -35;
      MenuPlaySingleplayerUI.playButton.positionScale_X = 0.25f;
      MenuPlaySingleplayerUI.playButton.positionScale_Y = 0.5f;
      MenuPlaySingleplayerUI.playButton.sizeOffset_X = 200;
      MenuPlaySingleplayerUI.playButton.sizeOffset_Y = 30;
      MenuPlaySingleplayerUI.playButton.text = MenuPlaySingleplayerUI.localization.format("Play_Button");
      MenuPlaySingleplayerUI.playButton.tooltip = MenuPlaySingleplayerUI.localization.format("Play_Button_Tooltip");
      MenuPlaySingleplayerUI.playButton.onClickedButton = new ClickedButton(MenuPlaySingleplayerUI.onClickedPlayButton);
      MenuPlaySingleplayerUI.container.add((Sleek) MenuPlaySingleplayerUI.playButton);
      MenuPlaySingleplayerUI.modeButtonState = new SleekButtonState(new GUIContent[4]
      {
        new GUIContent(MenuPlaySingleplayerUI.localization.format("Easy_Button"), (Texture) bundle.load("Easy")),
        new GUIContent(MenuPlaySingleplayerUI.localization.format("Normal_Button"), (Texture) bundle.load("Normal")),
        new GUIContent(MenuPlaySingleplayerUI.localization.format("Hard_Button"), (Texture) bundle.load("Hard")),
        new GUIContent(MenuPlaySingleplayerUI.localization.format("Pro_Button"), (Texture) bundle.load(!Provider.isPro ? "Lock" : "Pro"))
      });
      MenuPlaySingleplayerUI.modeButtonState.positionOffset_X = -100;
      MenuPlaySingleplayerUI.modeButtonState.positionOffset_Y = 5;
      MenuPlaySingleplayerUI.modeButtonState.positionScale_X = 0.25f;
      MenuPlaySingleplayerUI.modeButtonState.positionScale_Y = 0.5f;
      MenuPlaySingleplayerUI.modeButtonState.sizeOffset_X = 200;
      MenuPlaySingleplayerUI.modeButtonState.sizeOffset_Y = 30;
      MenuPlaySingleplayerUI.modeButtonState.state = (int) PlaySettings.singleplayerMode;
      MenuPlaySingleplayerUI.modeButtonState.onSwappedState = new SwappedState(MenuPlaySingleplayerUI.onSwappedModeState);
      MenuPlaySingleplayerUI.container.add((Sleek) MenuPlaySingleplayerUI.modeButtonState);
      MenuPlaySingleplayerUI.resetButton = new SleekButtonIconConfirm((Texture2D) null, MenuPlaySingleplayerUI.localization.format("Reset_Button_Confirm"), MenuPlaySingleplayerUI.localization.format("Reset_Button_Confirm_Tooltip"), MenuPlaySingleplayerUI.localization.format("Reset_Button_Deny"), MenuPlaySingleplayerUI.localization.format("Reset_Button_Deny_Tooltip"));
      MenuPlaySingleplayerUI.resetButton.positionOffset_X = -100;
      MenuPlaySingleplayerUI.resetButton.positionOffset_Y = 85;
      MenuPlaySingleplayerUI.resetButton.positionScale_X = 0.25f;
      MenuPlaySingleplayerUI.resetButton.positionScale_Y = 0.5f;
      MenuPlaySingleplayerUI.resetButton.sizeOffset_X = 200;
      MenuPlaySingleplayerUI.resetButton.sizeOffset_Y = 30;
      MenuPlaySingleplayerUI.resetButton.text = MenuPlaySingleplayerUI.localization.format("Reset_Button");
      MenuPlaySingleplayerUI.resetButton.tooltip = MenuPlaySingleplayerUI.localization.format("Reset_Button_Tooltip");
      MenuPlaySingleplayerUI.resetButton.onConfirmed = new Confirm(MenuPlaySingleplayerUI.onClickedResetButton);
      MenuPlaySingleplayerUI.container.add((Sleek) MenuPlaySingleplayerUI.resetButton);
      bundle.unload();
      MenuPlaySingleplayerUI.onLevelsRefreshed();
      Level.onLevelsRefreshed += new LevelsRefreshed(MenuPlaySingleplayerUI.onLevelsRefreshed);
    }

    public static void open()
    {
      if (MenuPlaySingleplayerUI.active)
      {
        MenuPlaySingleplayerUI.close();
      }
      else
      {
        MenuPlaySingleplayerUI.active = true;
        MenuPlaySingleplayerUI.container.lerpPositionScale(0.0f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
      }
    }

    public static void close()
    {
      if (!MenuPlaySingleplayerUI.active)
        return;
      MenuPlaySingleplayerUI.active = false;
      MenuSettings.save();
      MenuPlaySingleplayerUI.container.lerpPositionScale(0.0f, 1f, ESleekLerp.EXPONENTIAL, 20f);
    }

    private static void onClickedLevel(SleekLevel level, byte index)
    {
      if ((int) index >= MenuPlaySingleplayerUI.levels.Length || MenuPlaySingleplayerUI.levels[(int) index] == null)
        return;
      PlaySettings.singleplayerMap = MenuPlaySingleplayerUI.levels[(int) index].name;
      MenuPlaySingleplayerUI.selectedBox.text = PlaySettings.singleplayerMap;
    }

    private static void onClickedPlayButton(SleekButton button)
    {
      if (PlaySettings.singleplayerMap == null || PlaySettings.singleplayerMap.Length == 0)
        return;
      Provider.map = PlaySettings.singleplayerMap;
      if (PlaySettings.singleplayerMode == EGameMode.PRO && !Provider.isPro)
        return;
      MenuSettings.save();
      Provider.singleplayer(PlaySettings.singleplayerMode);
    }

    private static void onSwappedModeState(SleekButtonState button, int index)
    {
      PlaySettings.singleplayerMode = (EGameMode) index;
    }

    private static void onClickedResetButton(SleekButton button)
    {
      if (PlaySettings.singleplayerMap == null || PlaySettings.singleplayerMap.Length == 0)
        return;
      if (ReadWrite.folderExists(string.Concat(new object[4]
      {
        (object) "/Worlds/Singleplayer_",
        (object) Characters.selected,
        (object) "/Level/",
        (object) PlaySettings.singleplayerMap
      })))
        ReadWrite.deleteFolder(string.Concat(new object[4]
        {
          (object) "/Worlds/Singleplayer_",
          (object) Characters.selected,
          (object) "/Level/",
          (object) PlaySettings.singleplayerMap
        }));
      if (!ReadWrite.folderExists("/Worlds/Singleplayer_" + (object) Characters.selected + "/Players/" + (string) (object) Provider.user + "_" + (string) (object) Characters.selected + "/" + PlaySettings.singleplayerMap))
        return;
      ReadWrite.deleteFolder("/Worlds/Singleplayer_" + (object) Characters.selected + "/Players/" + (string) (object) Provider.user + "_" + (string) (object) Characters.selected + "/" + PlaySettings.singleplayerMap);
    }

    private static void onLevelsRefreshed()
    {
      MenuPlaySingleplayerUI.levelScrollBox.remove();
      MenuPlaySingleplayerUI.levels = Level.getLevels();
      bool flag = false;
      MenuPlaySingleplayerUI.levelButtons = new SleekLevel[MenuPlaySingleplayerUI.levels.Length];
      for (int index = 0; index < MenuPlaySingleplayerUI.levels.Length; ++index)
      {
        if (MenuPlaySingleplayerUI.levels[index] != null)
        {
          SleekLevel sleekLevel = new SleekLevel(MenuPlaySingleplayerUI.levels[index], false);
          sleekLevel.positionOffset_Y = index * 110;
          sleekLevel.onClickedLevel = new ClickedLevel(MenuPlaySingleplayerUI.onClickedLevel);
          MenuPlaySingleplayerUI.levelScrollBox.add((Sleek) sleekLevel);
          MenuPlaySingleplayerUI.levelButtons[index] = sleekLevel;
          if (!flag && MenuPlaySingleplayerUI.levels[index].name == PlaySettings.singleplayerMap)
            flag = true;
        }
      }
      if (MenuPlaySingleplayerUI.levels.Length == 0)
        PlaySettings.singleplayerMap = string.Empty;
      else if (!flag || PlaySettings.singleplayerMap == null || PlaySettings.singleplayerMap.Length == 0)
        PlaySettings.singleplayerMap = MenuPlaySingleplayerUI.levels[0].name;
      MenuPlaySingleplayerUI.selectedBox.text = PlaySettings.singleplayerMap;
      MenuPlaySingleplayerUI.levelScrollBox.area = new Rect(0.0f, 0.0f, 5f, (float) (MenuPlaySingleplayerUI.levels.Length * 110 - 10));
    }
  }
}
