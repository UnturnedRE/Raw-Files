// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.EditorTerrainMaterialsUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class EditorTerrainMaterialsUI
  {
    private static Sleek container;
    public static bool active;
    private static SleekScrollBox materialScrollBox;
    private static SleekButtonIcon bakeGlobalMaterialsButton;
    private static SleekButtonIcon bakeLocalMaterialsButton;
    private static SleekButtonState map2Button;
    private static SleekBox selectedBox;
    private static SleekSlider sizeSlider;

    public EditorTerrainMaterialsUI()
    {
      Local local = Localization.read("/Editor/EditorTerrainMaterials.dat");
      Bundle bundle = Bundles.getBundle("/Bundles/Textures/Edit/Icons/EditorTerrainMaterials/EditorTerrainMaterials.unity3d");
      EditorTerrainMaterialsUI.container = new Sleek();
      EditorTerrainMaterialsUI.container.positionOffset_X = 10;
      EditorTerrainMaterialsUI.container.positionOffset_Y = 10;
      EditorTerrainMaterialsUI.container.positionScale_X = 1f;
      EditorTerrainMaterialsUI.container.sizeOffset_X = -20;
      EditorTerrainMaterialsUI.container.sizeOffset_Y = -20;
      EditorTerrainMaterialsUI.container.sizeScale_X = 1f;
      EditorTerrainMaterialsUI.container.sizeScale_Y = 1f;
      EditorUI.window.add(EditorTerrainMaterialsUI.container);
      EditorTerrainMaterialsUI.active = false;
      EditorTerrainMaterialsUI.materialScrollBox = new SleekScrollBox();
      EditorTerrainMaterialsUI.materialScrollBox.positionOffset_Y = 120;
      EditorTerrainMaterialsUI.materialScrollBox.positionOffset_X = -400;
      EditorTerrainMaterialsUI.materialScrollBox.positionScale_X = 1f;
      EditorTerrainMaterialsUI.materialScrollBox.sizeOffset_X = 400;
      EditorTerrainMaterialsUI.materialScrollBox.sizeOffset_Y = -200;
      EditorTerrainMaterialsUI.materialScrollBox.sizeScale_Y = 1f;
      EditorTerrainMaterialsUI.materialScrollBox.area = new Rect(0.0f, 0.0f, 5f, (float) (LevelGround.materials.Length * 550 - 10));
      EditorTerrainMaterialsUI.container.add((Sleek) EditorTerrainMaterialsUI.materialScrollBox);
      for (int index = 0; index < LevelGround.materials.Length; ++index)
      {
        SleekImageTexture sleekImageTexture = new SleekImageTexture();
        sleekImageTexture.positionOffset_X = 200;
        sleekImageTexture.positionOffset_Y = index * 550;
        sleekImageTexture.sizeOffset_X = 64;
        sleekImageTexture.sizeOffset_Y = 64;
        sleekImageTexture.texture = (Texture) LevelGround.materials[index].prototype.texture;
        EditorTerrainMaterialsUI.materialScrollBox.add((Sleek) sleekImageTexture);
        SleekButton sleekButton = new SleekButton();
        sleekButton.positionOffset_X = 70;
        sleekButton.sizeOffset_X = 100;
        sleekButton.sizeOffset_Y = 64;
        sleekButton.text = LevelGround.materials[index].prototype.texture.name;
        sleekButton.onClickedButton = new ClickedButton(EditorTerrainMaterialsUI.onClickedMaterialButton);
        sleekImageTexture.add((Sleek) sleekButton);
        SleekSlider sleekSlider1 = new SleekSlider();
        sleekSlider1.positionOffset_Y = 70;
        sleekSlider1.sizeOffset_X = 170;
        sleekSlider1.sizeOffset_Y = 20;
        sleekSlider1.orientation = ESleekOrientation.HORIZONTAL;
        sleekSlider1.addLabel(local.format("OvergrowthSliderLabelText"), ESleekSide.LEFT);
        sleekSlider1.state = LevelGround.materials[index].overgrowth;
        sleekSlider1.onDragged = new Dragged(EditorTerrainMaterialsUI.onDraggedOvergrowthSlider);
        sleekImageTexture.add((Sleek) sleekSlider1);
        SleekSlider sleekSlider2 = new SleekSlider();
        sleekSlider2.positionOffset_Y = 100;
        sleekSlider2.sizeOffset_X = 170;
        sleekSlider2.sizeOffset_Y = 20;
        sleekSlider2.orientation = ESleekOrientation.HORIZONTAL;
        sleekSlider2.addLabel(local.format("ChanceSliderLabelText"), ESleekSide.LEFT);
        sleekSlider2.state = LevelGround.materials[index].chance;
        sleekSlider2.onDragged = new Dragged(EditorTerrainMaterialsUI.onDraggedChanceSlider);
        sleekImageTexture.add((Sleek) sleekSlider2);
        SleekSlider sleekSlider3 = new SleekSlider();
        sleekSlider3.positionOffset_Y = 130;
        sleekSlider3.sizeOffset_X = 170;
        sleekSlider3.sizeOffset_Y = 20;
        sleekSlider3.orientation = ESleekOrientation.HORIZONTAL;
        sleekSlider3.state = LevelGround.materials[index].steepness;
        sleekSlider3.addLabel(local.format("SteepnessFieldLabelText"), ESleekSide.LEFT);
        sleekSlider3.onDragged = new Dragged(EditorTerrainMaterialsUI.onDraggedSteepnessSlider);
        sleekImageTexture.add((Sleek) sleekSlider3);
        SleekValue sleekValue = new SleekValue();
        sleekValue.positionOffset_Y = 160;
        sleekValue.sizeOffset_X = 170;
        sleekValue.sizeOffset_Y = 30;
        sleekValue.state = LevelGround.materials[index].height;
        sleekValue.addLabel(local.format("HeightValueLabelText"), ESleekSide.LEFT);
        sleekValue.onValued = new Valued(EditorTerrainMaterialsUI.onValuedHeightValue);
        sleekImageTexture.add((Sleek) sleekValue);
        SleekToggle sleekToggle1 = new SleekToggle();
        sleekToggle1.positionOffset_Y = 200;
        sleekToggle1.sizeOffset_X = 40;
        sleekToggle1.sizeOffset_Y = 40;
        sleekToggle1.addLabel(local.format("Grassy_0_ToggleLabelText"), ESleekSide.RIGHT);
        sleekToggle1.state = LevelGround.materials[index].isGrassy_0;
        sleekToggle1.onToggled = new Toggled(EditorTerrainMaterialsUI.onToggledGrassy_0_Toggle);
        sleekImageTexture.add((Sleek) sleekToggle1);
        SleekToggle sleekToggle2 = new SleekToggle();
        sleekToggle2.positionOffset_Y = 250;
        sleekToggle2.sizeOffset_X = 40;
        sleekToggle2.sizeOffset_Y = 40;
        sleekToggle2.addLabel(local.format("Grassy_1_ToggleLabelText"), ESleekSide.RIGHT);
        sleekToggle2.state = LevelGround.materials[index].isGrassy_1;
        sleekToggle2.onToggled = new Toggled(EditorTerrainMaterialsUI.onToggledGrassy_1_Toggle);
        sleekImageTexture.add((Sleek) sleekToggle2);
        SleekToggle sleekToggle3 = new SleekToggle();
        sleekToggle3.positionOffset_Y = 300;
        sleekToggle3.sizeOffset_X = 40;
        sleekToggle3.sizeOffset_Y = 40;
        sleekToggle3.addLabel(local.format("FloweryToggleLabelText"), ESleekSide.RIGHT);
        sleekToggle3.state = LevelGround.materials[index].isFlowery;
        sleekToggle3.onToggled = new Toggled(EditorTerrainMaterialsUI.onToggledFloweryToggle);
        sleekImageTexture.add((Sleek) sleekToggle3);
        SleekToggle sleekToggle4 = new SleekToggle();
        sleekToggle4.positionOffset_Y = 350;
        sleekToggle4.sizeOffset_X = 40;
        sleekToggle4.sizeOffset_Y = 40;
        sleekToggle4.addLabel(local.format("RockyToggleLabelText"), ESleekSide.RIGHT);
        sleekToggle4.state = LevelGround.materials[index].isRocky;
        sleekToggle4.onToggled = new Toggled(EditorTerrainMaterialsUI.onToggledRockyToggle);
        sleekImageTexture.add((Sleek) sleekToggle4);
        SleekToggle sleekToggle5 = new SleekToggle();
        sleekToggle5.positionOffset_Y = 400;
        sleekToggle5.sizeOffset_X = 40;
        sleekToggle5.sizeOffset_Y = 40;
        sleekToggle5.addLabel(local.format("SnowyToggleLabelText"), ESleekSide.RIGHT);
        sleekToggle5.state = LevelGround.materials[index].isSnowy;
        sleekToggle5.onToggled = new Toggled(EditorTerrainMaterialsUI.onToggledSnowyToggle);
        sleekImageTexture.add((Sleek) sleekToggle5);
        SleekToggle sleekToggle6 = new SleekToggle();
        sleekToggle6.positionOffset_Y = 450;
        sleekToggle6.sizeOffset_X = 40;
        sleekToggle6.sizeOffset_Y = 40;
        sleekToggle6.addLabel(local.format("FoundationToggleLabelText"), ESleekSide.RIGHT);
        sleekToggle6.state = LevelGround.materials[index].isFoundation;
        sleekToggle6.onToggled = new Toggled(EditorTerrainMaterialsUI.onToggledFoundationToggle);
        sleekImageTexture.add((Sleek) sleekToggle6);
        SleekToggle sleekToggle7 = new SleekToggle();
        sleekToggle7.positionOffset_Y = 500;
        sleekToggle7.sizeOffset_X = 40;
        sleekToggle7.sizeOffset_Y = 40;
        sleekToggle7.addLabel(local.format("GeneratedToggleLabelText"), ESleekSide.RIGHT);
        sleekToggle7.state = LevelGround.materials[index].isGenerated;
        sleekToggle7.onToggled = new Toggled(EditorTerrainMaterialsUI.onToggledGeneratedToggle);
        sleekImageTexture.add((Sleek) sleekToggle7);
      }
      EditorTerrainMaterialsUI.selectedBox = new SleekBox();
      EditorTerrainMaterialsUI.selectedBox.positionOffset_X = -200;
      EditorTerrainMaterialsUI.selectedBox.positionOffset_Y = 80;
      EditorTerrainMaterialsUI.selectedBox.positionScale_X = 1f;
      EditorTerrainMaterialsUI.selectedBox.sizeOffset_X = 200;
      EditorTerrainMaterialsUI.selectedBox.sizeOffset_Y = 30;
      if ((int) EditorTerrainMaterials.selected < LevelGround.materials.Length)
        EditorTerrainMaterialsUI.selectedBox.text = LevelGround.materials[(int) EditorTerrainMaterials.selected].prototype.texture.name;
      EditorTerrainMaterialsUI.selectedBox.addLabel(local.format("SelectionBoxLabelText"), ESleekSide.LEFT);
      EditorTerrainMaterialsUI.container.add((Sleek) EditorTerrainMaterialsUI.selectedBox);
      EditorTerrainMaterialsUI.bakeGlobalMaterialsButton = new SleekButtonIcon((Texture2D) bundle.load("Materials"));
      EditorTerrainMaterialsUI.bakeGlobalMaterialsButton.positionOffset_X = -200;
      EditorTerrainMaterialsUI.bakeGlobalMaterialsButton.positionOffset_Y = -70;
      EditorTerrainMaterialsUI.bakeGlobalMaterialsButton.positionScale_X = 1f;
      EditorTerrainMaterialsUI.bakeGlobalMaterialsButton.positionScale_Y = 1f;
      EditorTerrainMaterialsUI.bakeGlobalMaterialsButton.sizeOffset_X = 200;
      EditorTerrainMaterialsUI.bakeGlobalMaterialsButton.sizeOffset_Y = 30;
      EditorTerrainMaterialsUI.bakeGlobalMaterialsButton.text = local.format("BakeGlobalMaterialsButtonText");
      EditorTerrainMaterialsUI.bakeGlobalMaterialsButton.tooltip = local.format("BakeGlobalMaterialsButtonTooltip");
      EditorTerrainMaterialsUI.bakeGlobalMaterialsButton.onClickedButton = new ClickedButton(EditorTerrainMaterialsUI.onClickedBakeGlobalMaterialsButton);
      EditorTerrainMaterialsUI.container.add((Sleek) EditorTerrainMaterialsUI.bakeGlobalMaterialsButton);
      EditorTerrainMaterialsUI.bakeLocalMaterialsButton = new SleekButtonIcon((Texture2D) bundle.load("Materials"));
      EditorTerrainMaterialsUI.bakeLocalMaterialsButton.positionOffset_X = -200;
      EditorTerrainMaterialsUI.bakeLocalMaterialsButton.positionOffset_Y = -30;
      EditorTerrainMaterialsUI.bakeLocalMaterialsButton.positionScale_X = 1f;
      EditorTerrainMaterialsUI.bakeLocalMaterialsButton.positionScale_Y = 1f;
      EditorTerrainMaterialsUI.bakeLocalMaterialsButton.sizeOffset_X = 200;
      EditorTerrainMaterialsUI.bakeLocalMaterialsButton.sizeOffset_Y = 30;
      EditorTerrainMaterialsUI.bakeLocalMaterialsButton.text = local.format("BakeLocalMaterialsButtonText");
      EditorTerrainMaterialsUI.bakeLocalMaterialsButton.tooltip = local.format("BakeLocalMaterialsButtonTooltip");
      EditorTerrainMaterialsUI.bakeLocalMaterialsButton.onClickedButton = new ClickedButton(EditorTerrainMaterialsUI.onClickedBakeLocalMaterialsButton);
      EditorTerrainMaterialsUI.container.add((Sleek) EditorTerrainMaterialsUI.bakeLocalMaterialsButton);
      EditorTerrainMaterialsUI.map2Button = new SleekButtonState(new GUIContent[2]
      {
        new GUIContent(local.format("Map2ButtonText1")),
        new GUIContent(local.format("Map2ButtonText2"))
      });
      EditorTerrainMaterialsUI.map2Button.positionOffset_Y = -30;
      EditorTerrainMaterialsUI.map2Button.positionScale_Y = 1f;
      EditorTerrainMaterialsUI.map2Button.sizeOffset_X = 200;
      EditorTerrainMaterialsUI.map2Button.sizeOffset_Y = 30;
      EditorTerrainMaterialsUI.map2Button.tooltip = local.format("Map2ButtonTooltip");
      EditorTerrainMaterialsUI.map2Button.onSwappedState = new SwappedState(EditorTerrainMaterialsUI.onSwappedMap2);
      EditorTerrainMaterialsUI.container.add((Sleek) EditorTerrainMaterialsUI.map2Button);
      EditorTerrainMaterialsUI.sizeSlider = new SleekSlider();
      EditorTerrainMaterialsUI.sizeSlider.positionOffset_Y = -60;
      EditorTerrainMaterialsUI.sizeSlider.positionScale_Y = 1f;
      EditorTerrainMaterialsUI.sizeSlider.sizeOffset_X = 200;
      EditorTerrainMaterialsUI.sizeSlider.sizeOffset_Y = 20;
      EditorTerrainMaterialsUI.sizeSlider.orientation = ESleekOrientation.HORIZONTAL;
      EditorTerrainMaterialsUI.sizeSlider.state = (float) ((int) EditorTerrainMaterials.brushSize - (int) EditorTerrainMaterials.MIN_BRUSH_SIZE) / (float) EditorTerrainMaterials.MAX_BRUSH_SIZE;
      EditorTerrainMaterialsUI.sizeSlider.addLabel(local.format("SizeSliderLabelText"), ESleekSide.RIGHT);
      EditorTerrainMaterialsUI.sizeSlider.onDragged = new Dragged(EditorTerrainMaterialsUI.onDraggedSizeSlider);
      EditorTerrainMaterialsUI.container.add((Sleek) EditorTerrainMaterialsUI.sizeSlider);
      bundle.unload();
    }

    public static void open()
    {
      if (EditorTerrainMaterialsUI.active)
      {
        EditorTerrainMaterialsUI.close();
      }
      else
      {
        EditorTerrainMaterialsUI.active = true;
        EditorTerrainMaterials.isPainting = true;
        EditorTerrainMaterialsUI.container.lerpPositionScale(0.0f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
      }
    }

    public static void close()
    {
      if (!EditorTerrainMaterialsUI.active)
        return;
      EditorTerrainMaterialsUI.active = false;
      EditorTerrainMaterials.isPainting = false;
      EditorTerrainMaterialsUI.container.lerpPositionScale(1f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
    }

    private static void onClickedMaterialButton(SleekButton button)
    {
      EditorTerrainMaterials.selected = (byte) (button.parent.positionOffset_Y / 550);
      EditorTerrainMaterialsUI.selectedBox.text = LevelGround.materials[(int) EditorTerrainMaterials.selected].prototype.texture.name;
    }

    private static void onDraggedOvergrowthSlider(SleekSlider slider, float state)
    {
      LevelGround.materials[slider.parent.positionOffset_Y / 550].overgrowth = state;
    }

    private static void onDraggedChanceSlider(SleekSlider slider, float state)
    {
      LevelGround.materials[slider.parent.positionOffset_Y / 550].chance = state;
    }

    private static void onDraggedSteepnessSlider(SleekSlider slider, float state)
    {
      LevelGround.materials[slider.parent.positionOffset_Y / 550].steepness = state;
    }

    private static void onValuedHeightValue(SleekValue value, float state)
    {
      LevelGround.materials[value.parent.positionOffset_Y / 550].height = state;
    }

    private static void onToggledGrassy_0_Toggle(SleekToggle toggle, bool state)
    {
      LevelGround.materials[toggle.parent.positionOffset_Y / 550].isGrassy_0 = state;
    }

    private static void onToggledGrassy_1_Toggle(SleekToggle toggle, bool state)
    {
      LevelGround.materials[toggle.parent.positionOffset_Y / 550].isGrassy_1 = state;
    }

    private static void onToggledFloweryToggle(SleekToggle toggle, bool state)
    {
      LevelGround.materials[toggle.parent.positionOffset_Y / 550].isFlowery = state;
    }

    private static void onToggledRockyToggle(SleekToggle toggle, bool state)
    {
      LevelGround.materials[toggle.parent.positionOffset_Y / 550].isRocky = state;
    }

    private static void onToggledSnowyToggle(SleekToggle toggle, bool state)
    {
      LevelGround.materials[toggle.parent.positionOffset_Y / 550].isSnowy = state;
    }

    private static void onToggledFoundationToggle(SleekToggle toggle, bool state)
    {
      LevelGround.materials[toggle.parent.positionOffset_Y / 550].isFoundation = state;
    }

    private static void onToggledGeneratedToggle(SleekToggle toggle, bool state)
    {
      LevelGround.materials[toggle.parent.positionOffset_Y / 550].isGenerated = state;
    }

    private static void onClickedBakeGlobalMaterialsButton(SleekButton button)
    {
      LevelGround.bakeMaterials(true);
    }

    private static void onClickedBakeLocalMaterialsButton(SleekButton button)
    {
      LevelGround.bakeMaterials(false);
    }

    private static void onDraggedSizeSlider(Sleek field, float state)
    {
      EditorTerrainMaterials.brushSize = (byte) ((double) EditorTerrainMaterials.MIN_BRUSH_SIZE + (double) state * (double) EditorTerrainMaterials.MAX_BRUSH_SIZE);
    }

    private static void onSwappedMap2(SleekButtonState button, int state)
    {
      EditorTerrainMaterials.map2 = state == 1;
    }
  }
}
