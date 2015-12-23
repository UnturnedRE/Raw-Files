// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.EditorTerrainHeightUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class EditorTerrainHeightUI
  {
    private static Sleek container;
    public static bool active;
    private static SleekButtonIcon adjustUpButton;
    private static SleekButtonIcon adjustDownButton;
    private static SleekButtonIcon smoothButton;
    private static SleekButtonIcon flattenButton;
    private static SleekButtonState map2Button;
    private static SleekSlider sizeSlider;
    private static SleekSlider strengthSlider;
    public static SleekValue heightValue;

    public EditorTerrainHeightUI()
    {
      Local local = Localization.read("/Editor/EditorTerrainHeight.dat");
      Bundle bundle = Bundles.getBundle("/Bundles/Textures/Edit/Icons/EditorTerrainHeight/EditorTerrainHeight.unity3d");
      EditorTerrainHeightUI.container = new Sleek();
      EditorTerrainHeightUI.container.positionOffset_X = 10;
      EditorTerrainHeightUI.container.positionOffset_Y = 10;
      EditorTerrainHeightUI.container.positionScale_X = 1f;
      EditorTerrainHeightUI.container.sizeOffset_X = -20;
      EditorTerrainHeightUI.container.sizeOffset_Y = -20;
      EditorTerrainHeightUI.container.sizeScale_X = 1f;
      EditorTerrainHeightUI.container.sizeScale_Y = 1f;
      EditorUI.window.add(EditorTerrainHeightUI.container);
      EditorTerrainHeightUI.active = false;
      EditorTerrainHeightUI.adjustUpButton = new SleekButtonIcon((Texture2D) bundle.load("Adjust_Up"));
      EditorTerrainHeightUI.adjustUpButton.positionOffset_Y = -190;
      EditorTerrainHeightUI.adjustUpButton.positionScale_Y = 1f;
      EditorTerrainHeightUI.adjustUpButton.sizeOffset_X = 200;
      EditorTerrainHeightUI.adjustUpButton.sizeOffset_Y = 30;
      EditorTerrainHeightUI.adjustUpButton.text = local.format("AdjustUpButtonText", (object) ControlsSettings.tool_0);
      EditorTerrainHeightUI.adjustUpButton.tooltip = local.format("AdjustUpButtonTooltip");
      EditorTerrainHeightUI.adjustUpButton.onClickedButton = new ClickedButton(EditorTerrainHeightUI.onClickedAdjustUpButton);
      EditorTerrainHeightUI.container.add((Sleek) EditorTerrainHeightUI.adjustUpButton);
      EditorTerrainHeightUI.adjustDownButton = new SleekButtonIcon((Texture2D) bundle.load("Adjust_Down"));
      EditorTerrainHeightUI.adjustDownButton.positionOffset_Y = -150;
      EditorTerrainHeightUI.adjustDownButton.positionScale_Y = 1f;
      EditorTerrainHeightUI.adjustDownButton.sizeOffset_X = 200;
      EditorTerrainHeightUI.adjustDownButton.sizeOffset_Y = 30;
      EditorTerrainHeightUI.adjustDownButton.text = local.format("AdjustDownButtonText", (object) ControlsSettings.tool_0);
      EditorTerrainHeightUI.adjustDownButton.tooltip = local.format("AdjustDownButtonTooltip");
      EditorTerrainHeightUI.adjustDownButton.onClickedButton = new ClickedButton(EditorTerrainHeightUI.onClickedAdjustDownButton);
      EditorTerrainHeightUI.container.add((Sleek) EditorTerrainHeightUI.adjustDownButton);
      EditorTerrainHeightUI.smoothButton = new SleekButtonIcon((Texture2D) bundle.load("Smooth"));
      EditorTerrainHeightUI.smoothButton.positionOffset_Y = -110;
      EditorTerrainHeightUI.smoothButton.positionScale_Y = 1f;
      EditorTerrainHeightUI.smoothButton.sizeOffset_X = 200;
      EditorTerrainHeightUI.smoothButton.sizeOffset_Y = 30;
      EditorTerrainHeightUI.smoothButton.text = local.format("SmoothButtonText", (object) ControlsSettings.tool_1);
      EditorTerrainHeightUI.smoothButton.tooltip = local.format("SmoothButtonTooltip");
      EditorTerrainHeightUI.smoothButton.onClickedButton = new ClickedButton(EditorTerrainHeightUI.onClickedSmoothButton);
      EditorTerrainHeightUI.container.add((Sleek) EditorTerrainHeightUI.smoothButton);
      EditorTerrainHeightUI.flattenButton = new SleekButtonIcon((Texture2D) bundle.load("Flatten"));
      EditorTerrainHeightUI.flattenButton.positionOffset_Y = -70;
      EditorTerrainHeightUI.flattenButton.positionScale_Y = 1f;
      EditorTerrainHeightUI.flattenButton.sizeOffset_X = 200;
      EditorTerrainHeightUI.flattenButton.sizeOffset_Y = 30;
      EditorTerrainHeightUI.flattenButton.text = local.format("FlattenButtonText", (object) ControlsSettings.tool_2);
      EditorTerrainHeightUI.flattenButton.tooltip = local.format("FlattenButtonTooltip");
      EditorTerrainHeightUI.flattenButton.onClickedButton = new ClickedButton(EditorTerrainHeightUI.onClickedFlattenButton);
      EditorTerrainHeightUI.container.add((Sleek) EditorTerrainHeightUI.flattenButton);
      EditorTerrainHeightUI.map2Button = new SleekButtonState(new GUIContent[2]
      {
        new GUIContent(local.format("Map2ButtonText1")),
        new GUIContent(local.format("Map2ButtonText2"))
      });
      EditorTerrainHeightUI.map2Button.positionOffset_Y = -30;
      EditorTerrainHeightUI.map2Button.positionScale_Y = 1f;
      EditorTerrainHeightUI.map2Button.sizeOffset_X = 200;
      EditorTerrainHeightUI.map2Button.sizeOffset_Y = 30;
      EditorTerrainHeightUI.map2Button.tooltip = local.format("Map2ButtonTooltip");
      EditorTerrainHeightUI.map2Button.onSwappedState = new SwappedState(EditorTerrainHeightUI.onSwappedMap2);
      EditorTerrainHeightUI.container.add((Sleek) EditorTerrainHeightUI.map2Button);
      EditorTerrainHeightUI.sizeSlider = new SleekSlider();
      EditorTerrainHeightUI.sizeSlider.positionOffset_Y = -290;
      EditorTerrainHeightUI.sizeSlider.positionScale_Y = 1f;
      EditorTerrainHeightUI.sizeSlider.sizeOffset_X = 200;
      EditorTerrainHeightUI.sizeSlider.sizeOffset_Y = 20;
      EditorTerrainHeightUI.sizeSlider.orientation = ESleekOrientation.HORIZONTAL;
      EditorTerrainHeightUI.sizeSlider.state = (float) ((int) EditorTerrainHeight.brushSize - (int) EditorTerrainHeight.MIN_BRUSH_SIZE) / (float) EditorTerrainHeight.MAX_BRUSH_SIZE;
      EditorTerrainHeightUI.sizeSlider.addLabel(local.format("SizeSliderLabelText"), ESleekSide.RIGHT);
      EditorTerrainHeightUI.sizeSlider.onDragged = new Dragged(EditorTerrainHeightUI.onDraggedSizeSlider);
      EditorTerrainHeightUI.container.add((Sleek) EditorTerrainHeightUI.sizeSlider);
      EditorTerrainHeightUI.strengthSlider = new SleekSlider();
      EditorTerrainHeightUI.strengthSlider.positionOffset_Y = -260;
      EditorTerrainHeightUI.strengthSlider.positionScale_Y = 1f;
      EditorTerrainHeightUI.strengthSlider.sizeOffset_X = 200;
      EditorTerrainHeightUI.strengthSlider.sizeOffset_Y = 20;
      EditorTerrainHeightUI.strengthSlider.orientation = ESleekOrientation.HORIZONTAL;
      EditorTerrainHeightUI.strengthSlider.addLabel(local.format("StrengthSliderLabelText"), ESleekSide.RIGHT);
      EditorTerrainHeightUI.strengthSlider.state = EditorTerrainHeight.brushStrength;
      EditorTerrainHeightUI.strengthSlider.onDragged = new Dragged(EditorTerrainHeightUI.onDraggedStrengthSlider);
      EditorTerrainHeightUI.container.add((Sleek) EditorTerrainHeightUI.strengthSlider);
      EditorTerrainHeightUI.heightValue = new SleekValue();
      EditorTerrainHeightUI.heightValue.positionOffset_Y = -230;
      EditorTerrainHeightUI.heightValue.positionScale_Y = 1f;
      EditorTerrainHeightUI.heightValue.sizeOffset_X = 200;
      EditorTerrainHeightUI.heightValue.sizeOffset_Y = 30;
      EditorTerrainHeightUI.heightValue.addLabel(local.format("HeightValueLabelText"), ESleekSide.RIGHT);
      EditorTerrainHeightUI.heightValue.state = EditorTerrainHeight.brushHeight;
      EditorTerrainHeightUI.heightValue.onValued = new Valued(EditorTerrainHeightUI.onValuedHeightValue);
      EditorTerrainHeightUI.container.add((Sleek) EditorTerrainHeightUI.heightValue);
      bundle.unload();
    }

    public static void open()
    {
      if (EditorTerrainHeightUI.active)
      {
        EditorTerrainHeightUI.close();
      }
      else
      {
        EditorTerrainHeightUI.active = true;
        EditorTerrainHeight.isTerraforming = true;
        EditorUI.message(EEditorMessage.HEIGHTS);
        EditorTerrainHeightUI.container.lerpPositionScale(0.0f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
      }
    }

    public static void close()
    {
      if (!EditorTerrainHeightUI.active)
        return;
      EditorTerrainHeightUI.active = false;
      EditorTerrainHeight.isTerraforming = false;
      EditorTerrainHeightUI.container.lerpPositionScale(1f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
    }

    private static void onClickedAdjustUpButton(SleekButton button)
    {
      EditorTerrainHeight.brushMode = EPaintMode.ADJUST_UP;
    }

    private static void onClickedAdjustDownButton(SleekButton button)
    {
      EditorTerrainHeight.brushMode = EPaintMode.ADJUST_DOWN;
    }

    private static void onClickedSmoothButton(SleekButton button)
    {
      EditorTerrainHeight.brushMode = EPaintMode.SMOOTH;
    }

    private static void onClickedFlattenButton(SleekButton button)
    {
      EditorTerrainHeight.brushMode = EPaintMode.FLATTEN;
    }

    private static void onSwappedMap2(SleekButtonState button, int state)
    {
      EditorTerrainHeight.map2 = state == 1;
    }

    private static void onDraggedSizeSlider(SleekSlider slider, float state)
    {
      EditorTerrainHeight.brushSize = (byte) ((double) EditorTerrainHeight.MIN_BRUSH_SIZE + (double) state * (double) EditorTerrainHeight.MAX_BRUSH_SIZE);
    }

    private static void onDraggedStrengthSlider(SleekSlider slider, float state)
    {
      EditorTerrainHeight.brushStrength = state;
    }

    private static void onValuedHeightValue(SleekValue value, float state)
    {
      EditorTerrainHeight.brushHeight = state;
    }
  }
}
