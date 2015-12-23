// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.EditorLevelPlayersUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class EditorLevelPlayersUI
  {
    private static Sleek container;
    public static bool active;
    private static SleekSlider radiusSlider;
    private static SleekSlider rotationSlider;
    private static SleekButtonIcon addButton;
    private static SleekButtonIcon removeButton;

    public EditorLevelPlayersUI()
    {
      Local local = Localization.read("/Editor/EditorLevelPlayers.dat");
      Bundle bundle = Bundles.getBundle("/Bundles/Textures/Edit/Icons/EditorLevelPlayers/EditorLevelPlayers.unity3d");
      EditorLevelPlayersUI.container = new Sleek();
      EditorLevelPlayersUI.container.positionOffset_X = 10;
      EditorLevelPlayersUI.container.positionOffset_Y = 10;
      EditorLevelPlayersUI.container.positionScale_X = 1f;
      EditorLevelPlayersUI.container.sizeOffset_X = -20;
      EditorLevelPlayersUI.container.sizeOffset_Y = -20;
      EditorLevelPlayersUI.container.sizeScale_X = 1f;
      EditorLevelPlayersUI.container.sizeScale_Y = 1f;
      EditorUI.window.add(EditorLevelPlayersUI.container);
      EditorLevelPlayersUI.active = false;
      EditorLevelPlayersUI.radiusSlider = new SleekSlider();
      EditorLevelPlayersUI.radiusSlider.positionOffset_Y = -130;
      EditorLevelPlayersUI.radiusSlider.positionScale_Y = 1f;
      EditorLevelPlayersUI.radiusSlider.sizeOffset_X = 200;
      EditorLevelPlayersUI.radiusSlider.sizeOffset_Y = 20;
      EditorLevelPlayersUI.radiusSlider.state = (float) ((int) EditorSpawns.radius - (int) EditorSpawns.MIN_REMOVE_SIZE) / (float) EditorSpawns.MAX_REMOVE_SIZE;
      EditorLevelPlayersUI.radiusSlider.orientation = ESleekOrientation.HORIZONTAL;
      EditorLevelPlayersUI.radiusSlider.addLabel(local.format("RadiusSliderLabelText"), ESleekSide.RIGHT);
      EditorLevelPlayersUI.radiusSlider.onDragged = new Dragged(EditorLevelPlayersUI.onDraggedRadiusSlider);
      EditorLevelPlayersUI.container.add((Sleek) EditorLevelPlayersUI.radiusSlider);
      EditorLevelPlayersUI.rotationSlider = new SleekSlider();
      EditorLevelPlayersUI.rotationSlider.positionOffset_Y = -100;
      EditorLevelPlayersUI.rotationSlider.positionScale_Y = 1f;
      EditorLevelPlayersUI.rotationSlider.sizeOffset_X = 200;
      EditorLevelPlayersUI.rotationSlider.sizeOffset_Y = 20;
      EditorLevelPlayersUI.rotationSlider.state = EditorSpawns.rotation / 360f;
      EditorLevelPlayersUI.rotationSlider.orientation = ESleekOrientation.HORIZONTAL;
      EditorLevelPlayersUI.rotationSlider.addLabel(local.format("RotationSliderLabelText"), ESleekSide.RIGHT);
      EditorLevelPlayersUI.rotationSlider.onDragged = new Dragged(EditorLevelPlayersUI.onDraggedRotationSlider);
      EditorLevelPlayersUI.container.add((Sleek) EditorLevelPlayersUI.rotationSlider);
      EditorLevelPlayersUI.addButton = new SleekButtonIcon((Texture2D) bundle.load("Add"));
      EditorLevelPlayersUI.addButton.positionOffset_Y = -70;
      EditorLevelPlayersUI.addButton.positionScale_Y = 1f;
      EditorLevelPlayersUI.addButton.sizeOffset_X = 200;
      EditorLevelPlayersUI.addButton.sizeOffset_Y = 30;
      EditorLevelPlayersUI.addButton.text = local.format("AddButtonText", (object) ControlsSettings.tool_0);
      EditorLevelPlayersUI.addButton.tooltip = local.format("AddButtonTooltip");
      EditorLevelPlayersUI.addButton.onClickedButton = new ClickedButton(EditorLevelPlayersUI.onClickedAddButton);
      EditorLevelPlayersUI.container.add((Sleek) EditorLevelPlayersUI.addButton);
      EditorLevelPlayersUI.removeButton = new SleekButtonIcon((Texture2D) bundle.load("Remove"));
      EditorLevelPlayersUI.removeButton.positionOffset_Y = -30;
      EditorLevelPlayersUI.removeButton.positionScale_Y = 1f;
      EditorLevelPlayersUI.removeButton.sizeOffset_X = 200;
      EditorLevelPlayersUI.removeButton.sizeOffset_Y = 30;
      EditorLevelPlayersUI.removeButton.text = local.format("RemoveButtonText", (object) ControlsSettings.tool_1);
      EditorLevelPlayersUI.removeButton.tooltip = local.format("RemoveButtonTooltip");
      EditorLevelPlayersUI.removeButton.onClickedButton = new ClickedButton(EditorLevelPlayersUI.onClickedRemoveButton);
      EditorLevelPlayersUI.container.add((Sleek) EditorLevelPlayersUI.removeButton);
      bundle.unload();
    }

    public static void open()
    {
      if (EditorLevelPlayersUI.active)
      {
        EditorLevelPlayersUI.close();
      }
      else
      {
        EditorLevelPlayersUI.active = true;
        EditorSpawns.isSpawning = true;
        EditorSpawns.spawnMode = ESpawnMode.ADD_PLAYER;
        EditorLevelPlayersUI.container.lerpPositionScale(0.0f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
      }
    }

    public static void close()
    {
      if (!EditorLevelPlayersUI.active)
        return;
      EditorLevelPlayersUI.active = false;
      EditorSpawns.isSpawning = false;
      EditorLevelPlayersUI.container.lerpPositionScale(1f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
    }

    private static void onDraggedRadiusSlider(SleekSlider slider, float state)
    {
      EditorSpawns.radius = (byte) ((double) EditorSpawns.MIN_REMOVE_SIZE + (double) state * (double) EditorSpawns.MAX_REMOVE_SIZE);
    }

    private static void onDraggedRotationSlider(SleekSlider slider, float state)
    {
      EditorSpawns.rotation = state * 360f;
    }

    private static void onClickedAddButton(SleekButton button)
    {
      EditorSpawns.spawnMode = ESpawnMode.ADD_PLAYER;
    }

    private static void onClickedRemoveButton(SleekButton button)
    {
      EditorSpawns.spawnMode = ESpawnMode.REMOVE_PLAYER;
    }
  }
}
