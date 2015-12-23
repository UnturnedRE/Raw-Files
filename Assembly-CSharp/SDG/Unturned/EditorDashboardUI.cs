// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.EditorDashboardUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class EditorDashboardUI
  {
    private static Sleek container;
    public static Local localization;
    private static SleekButtonIcon terrainButton;
    private static SleekButtonIcon environmentButton;
    private static SleekButtonIcon spawnsButton;
    private static SleekButtonIcon levelButton;

    public EditorDashboardUI()
    {
      EditorDashboardUI.localization = Localization.read("/Editor/EditorDashboard.dat");
      Bundle bundle = Bundles.getBundle("/Bundles/Textures/Edit/Icons/EditorDashboard/EditorDashboard.unity3d");
      EditorDashboardUI.container = new Sleek();
      EditorDashboardUI.container.positionOffset_X = 10;
      EditorDashboardUI.container.positionOffset_Y = 10;
      EditorDashboardUI.container.sizeOffset_X = -20;
      EditorDashboardUI.container.sizeOffset_Y = -20;
      EditorDashboardUI.container.sizeScale_X = 1f;
      EditorDashboardUI.container.sizeScale_Y = 1f;
      EditorUI.window.add(EditorDashboardUI.container);
      EditorDashboardUI.terrainButton = new SleekButtonIcon((Texture2D) bundle.load("Terrain"));
      EditorDashboardUI.terrainButton.sizeOffset_X = -5;
      EditorDashboardUI.terrainButton.sizeOffset_Y = 30;
      EditorDashboardUI.terrainButton.sizeScale_X = 0.25f;
      EditorDashboardUI.terrainButton.text = EditorDashboardUI.localization.format("TerrainButtonText");
      EditorDashboardUI.terrainButton.tooltip = EditorDashboardUI.localization.format("TerrainButtonTooltip");
      EditorDashboardUI.terrainButton.onClickedButton = new ClickedButton(EditorDashboardUI.onClickedTerrainButton);
      EditorDashboardUI.container.add((Sleek) EditorDashboardUI.terrainButton);
      EditorDashboardUI.environmentButton = new SleekButtonIcon((Texture2D) bundle.load("Environment"));
      EditorDashboardUI.environmentButton.positionOffset_X = 5;
      EditorDashboardUI.environmentButton.positionScale_X = 0.25f;
      EditorDashboardUI.environmentButton.sizeOffset_X = -10;
      EditorDashboardUI.environmentButton.sizeOffset_Y = 30;
      EditorDashboardUI.environmentButton.sizeScale_X = 0.25f;
      EditorDashboardUI.environmentButton.text = EditorDashboardUI.localization.format("EnvironmentButtonText");
      EditorDashboardUI.environmentButton.tooltip = EditorDashboardUI.localization.format("EnvironmentButtonTooltip");
      EditorDashboardUI.environmentButton.onClickedButton = new ClickedButton(EditorDashboardUI.onClickedEnvironmentButton);
      EditorDashboardUI.container.add((Sleek) EditorDashboardUI.environmentButton);
      EditorDashboardUI.spawnsButton = new SleekButtonIcon((Texture2D) bundle.load("Spawns"));
      EditorDashboardUI.spawnsButton.positionOffset_X = 5;
      EditorDashboardUI.spawnsButton.positionScale_X = 0.5f;
      EditorDashboardUI.spawnsButton.sizeOffset_X = -10;
      EditorDashboardUI.spawnsButton.sizeOffset_Y = 30;
      EditorDashboardUI.spawnsButton.sizeScale_X = 0.25f;
      EditorDashboardUI.spawnsButton.text = EditorDashboardUI.localization.format("SpawnsButtonText");
      EditorDashboardUI.spawnsButton.tooltip = EditorDashboardUI.localization.format("SpawnsButtonTooltip");
      EditorDashboardUI.spawnsButton.onClickedButton = new ClickedButton(EditorDashboardUI.onClickedSpawnsButton);
      EditorDashboardUI.container.add((Sleek) EditorDashboardUI.spawnsButton);
      EditorDashboardUI.levelButton = new SleekButtonIcon((Texture2D) bundle.load("Level"));
      EditorDashboardUI.levelButton.positionOffset_X = 5;
      EditorDashboardUI.levelButton.positionScale_X = 0.75f;
      EditorDashboardUI.levelButton.sizeOffset_X = -5;
      EditorDashboardUI.levelButton.sizeOffset_Y = 30;
      EditorDashboardUI.levelButton.sizeScale_X = 0.25f;
      EditorDashboardUI.levelButton.text = EditorDashboardUI.localization.format("LevelButtonText");
      EditorDashboardUI.levelButton.tooltip = EditorDashboardUI.localization.format("LevelButtonTooltip");
      EditorDashboardUI.levelButton.onClickedButton = new ClickedButton(EditorDashboardUI.onClickedLevelButton);
      EditorDashboardUI.container.add((Sleek) EditorDashboardUI.levelButton);
      bundle.unload();
      EditorPauseUI editorPauseUi = new EditorPauseUI();
      EditorTerrainUI editorTerrainUi = new EditorTerrainUI();
      EditorEnvironmentUI editorEnvironmentUi = new EditorEnvironmentUI();
      EditorSpawnsUI editorSpawnsUi = new EditorSpawnsUI();
      EditorLevelUI editorLevelUi = new EditorLevelUI();
    }

    private static void onClickedTerrainButton(SleekButton button)
    {
      EditorTerrainUI.open();
      EditorEnvironmentUI.close();
      EditorSpawnsUI.close();
      EditorLevelUI.close();
    }

    private static void onClickedEnvironmentButton(SleekButton button)
    {
      EditorTerrainUI.close();
      EditorEnvironmentUI.open();
      EditorSpawnsUI.close();
      EditorLevelUI.close();
    }

    private static void onClickedSpawnsButton(SleekButton button)
    {
      EditorTerrainUI.close();
      EditorEnvironmentUI.close();
      EditorSpawnsUI.open();
      EditorLevelUI.close();
    }

    private static void onClickedLevelButton(SleekButton button)
    {
      EditorTerrainUI.close();
      EditorEnvironmentUI.close();
      EditorSpawnsUI.close();
      EditorLevelUI.open();
    }
  }
}
