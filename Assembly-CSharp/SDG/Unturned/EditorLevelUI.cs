// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.EditorLevelUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class EditorLevelUI
  {
    private static Sleek container;
    public static bool active;
    private static SleekButtonIcon objectsButton;
    private static SleekButtonIcon visibilityButton;
    private static SleekButtonIcon playersButton;

    public EditorLevelUI()
    {
      Local local = Localization.read("/Editor/EditorLevel.dat");
      Bundle bundle = Bundles.getBundle("/Bundles/Textures/Edit/Icons/EditorLevel/EditorLevel.unity3d");
      EditorLevelUI.container = new Sleek();
      EditorLevelUI.container.positionOffset_X = 10;
      EditorLevelUI.container.positionOffset_Y = 10;
      EditorLevelUI.container.positionScale_X = 1f;
      EditorLevelUI.container.sizeOffset_X = -20;
      EditorLevelUI.container.sizeOffset_Y = -20;
      EditorLevelUI.container.sizeScale_X = 1f;
      EditorLevelUI.container.sizeScale_Y = 1f;
      EditorUI.window.add(EditorLevelUI.container);
      EditorLevelUI.active = false;
      EditorLevelUI.objectsButton = new SleekButtonIcon((Texture2D) bundle.load("Objects"));
      EditorLevelUI.objectsButton.positionOffset_Y = 40;
      EditorLevelUI.objectsButton.sizeOffset_X = -5;
      EditorLevelUI.objectsButton.sizeOffset_Y = 30;
      EditorLevelUI.objectsButton.sizeScale_X = 0.25f;
      EditorLevelUI.objectsButton.text = local.format("ObjectsButtonText");
      EditorLevelUI.objectsButton.tooltip = local.format("ObjectsButtonTooltip");
      EditorLevelUI.objectsButton.onClickedButton = new ClickedButton(EditorLevelUI.onClickedObjectsButton);
      EditorLevelUI.container.add((Sleek) EditorLevelUI.objectsButton);
      EditorLevelUI.visibilityButton = new SleekButtonIcon((Texture2D) bundle.load("Visibility"));
      EditorLevelUI.visibilityButton.positionOffset_X = 5;
      EditorLevelUI.visibilityButton.positionOffset_Y = 40;
      EditorLevelUI.visibilityButton.positionScale_X = 0.25f;
      EditorLevelUI.visibilityButton.sizeOffset_X = -10;
      EditorLevelUI.visibilityButton.sizeOffset_Y = 30;
      EditorLevelUI.visibilityButton.sizeScale_X = 0.25f;
      EditorLevelUI.visibilityButton.text = local.format("VisibilityButtonText");
      EditorLevelUI.visibilityButton.tooltip = local.format("VisibilityButtonTooltip");
      EditorLevelUI.visibilityButton.onClickedButton = new ClickedButton(EditorLevelUI.onClickedVisibilityButton);
      EditorLevelUI.container.add((Sleek) EditorLevelUI.visibilityButton);
      EditorLevelUI.playersButton = new SleekButtonIcon((Texture2D) bundle.load("Players"));
      EditorLevelUI.playersButton.positionOffset_Y = 40;
      EditorLevelUI.playersButton.positionOffset_X = 5;
      EditorLevelUI.playersButton.positionScale_X = 0.5f;
      EditorLevelUI.playersButton.sizeOffset_X = -10;
      EditorLevelUI.playersButton.sizeOffset_Y = 30;
      EditorLevelUI.playersButton.sizeScale_X = 0.25f;
      EditorLevelUI.playersButton.text = local.format("PlayersButtonText");
      EditorLevelUI.playersButton.tooltip = local.format("PlayersButtonTooltip");
      EditorLevelUI.playersButton.onClickedButton = new ClickedButton(EditorLevelUI.onClickedPlayersButton);
      EditorLevelUI.container.add((Sleek) EditorLevelUI.playersButton);
      bundle.unload();
      EditorLevelObjectsUI editorLevelObjectsUi = new EditorLevelObjectsUI();
      EditorLevelVisibilityUI levelVisibilityUi = new EditorLevelVisibilityUI();
      EditorLevelPlayersUI editorLevelPlayersUi = new EditorLevelPlayersUI();
    }

    public static void open()
    {
      if (EditorLevelUI.active)
      {
        EditorLevelUI.close();
      }
      else
      {
        EditorLevelUI.active = true;
        EditorLevelUI.container.lerpPositionScale(0.0f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
      }
    }

    public static void close()
    {
      if (!EditorLevelUI.active)
        return;
      EditorLevelUI.active = false;
      EditorLevelObjectsUI.close();
      EditorLevelVisibilityUI.close();
      EditorLevelPlayersUI.close();
      EditorLevelUI.container.lerpPositionScale(1f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
    }

    private static void onClickedObjectsButton(SleekButton button)
    {
      EditorLevelObjectsUI.open();
      EditorLevelVisibilityUI.close();
      EditorLevelPlayersUI.close();
    }

    private static void onClickedVisibilityButton(SleekButton button)
    {
      EditorLevelObjectsUI.close();
      EditorLevelVisibilityUI.open();
      EditorLevelPlayersUI.close();
    }

    private static void onClickedPlayersButton(SleekButton button)
    {
      EditorLevelObjectsUI.close();
      EditorLevelVisibilityUI.close();
      EditorLevelPlayersUI.open();
    }
  }
}
