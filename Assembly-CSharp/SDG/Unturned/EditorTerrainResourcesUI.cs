// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.EditorTerrainResourcesUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class EditorTerrainResourcesUI
  {
    private static Sleek container;
    public static bool active;
    private static SleekScrollBox resourcesScrollBox;
    private static SleekButtonIcon bakeGlobalResourcesButton;
    private static SleekButtonIcon bakeLocalResourcesButton;
    private static SleekBox selectedBox;
    private static SleekSlider radiusSlider;
    private static SleekButtonIcon addButton;
    private static SleekButtonIcon removeButton;

    public EditorTerrainResourcesUI()
    {
      Local local = Localization.read("/Editor/EditorTerrainResources.dat");
      Bundle bundle = Bundles.getBundle("/Bundles/Textures/Edit/Icons/EditorTerrainResources/EditorTerrainResources.unity3d");
      EditorTerrainResourcesUI.container = new Sleek();
      EditorTerrainResourcesUI.container.positionOffset_X = 10;
      EditorTerrainResourcesUI.container.positionOffset_Y = 10;
      EditorTerrainResourcesUI.container.positionScale_X = 1f;
      EditorTerrainResourcesUI.container.sizeOffset_X = -20;
      EditorTerrainResourcesUI.container.sizeOffset_Y = -20;
      EditorTerrainResourcesUI.container.sizeScale_X = 1f;
      EditorTerrainResourcesUI.container.sizeScale_Y = 1f;
      EditorUI.window.add(EditorTerrainResourcesUI.container);
      EditorTerrainResourcesUI.active = false;
      EditorTerrainResourcesUI.resourcesScrollBox = new SleekScrollBox();
      EditorTerrainResourcesUI.resourcesScrollBox.positionOffset_Y = 120;
      EditorTerrainResourcesUI.resourcesScrollBox.positionOffset_X = -400;
      EditorTerrainResourcesUI.resourcesScrollBox.positionScale_X = 1f;
      EditorTerrainResourcesUI.resourcesScrollBox.sizeOffset_X = 400;
      EditorTerrainResourcesUI.resourcesScrollBox.sizeOffset_Y = -200;
      EditorTerrainResourcesUI.resourcesScrollBox.sizeScale_Y = 1f;
      EditorTerrainResourcesUI.resourcesScrollBox.area = new Rect(0.0f, 0.0f, 5f, (float) (LevelGround.resources.Length * 350 - 10));
      EditorTerrainResourcesUI.container.add((Sleek) EditorTerrainResourcesUI.resourcesScrollBox);
      for (int index = 0; index < LevelGround.resources.Length; ++index)
      {
        ResourceAsset resourceAsset = (ResourceAsset) Assets.find(EAssetType.RESOURCE, LevelGround.resources[index].id);
        SleekButton sleekButton = new SleekButton();
        sleekButton.positionOffset_X = 200;
        sleekButton.positionOffset_Y = index * 350;
        sleekButton.sizeOffset_X = 170;
        sleekButton.sizeOffset_Y = 30;
        if (resourceAsset != null)
          sleekButton.text = resourceAsset.resourceName;
        sleekButton.onClickedButton = new ClickedButton(EditorTerrainResourcesUI.onClickedResourceButton);
        EditorTerrainResourcesUI.resourcesScrollBox.add((Sleek) sleekButton);
        SleekSlider sleekSlider1 = new SleekSlider();
        sleekSlider1.positionOffset_Y = 40;
        sleekSlider1.sizeOffset_X = 170;
        sleekSlider1.sizeOffset_Y = 20;
        sleekSlider1.orientation = ESleekOrientation.HORIZONTAL;
        sleekSlider1.addLabel(local.format("DensitySliderLabelText"), ESleekSide.LEFT);
        sleekSlider1.state = LevelGround.resources[index].density;
        sleekSlider1.onDragged = new Dragged(EditorTerrainResourcesUI.onDraggedDensitySlider);
        sleekButton.add((Sleek) sleekSlider1);
        SleekSlider sleekSlider2 = new SleekSlider();
        sleekSlider2.positionOffset_Y = 70;
        sleekSlider2.sizeOffset_X = 170;
        sleekSlider2.sizeOffset_Y = 20;
        sleekSlider2.orientation = ESleekOrientation.HORIZONTAL;
        sleekSlider2.addLabel(local.format("ChanceSliderLabelText"), ESleekSide.LEFT);
        sleekSlider2.state = LevelGround.resources[index].chance;
        sleekSlider2.onDragged = new Dragged(EditorTerrainResourcesUI.onDraggedChanceSlider);
        sleekButton.add((Sleek) sleekSlider2);
        SleekToggle sleekToggle1 = new SleekToggle();
        sleekToggle1.positionOffset_Y = 100;
        sleekToggle1.sizeOffset_X = 40;
        sleekToggle1.sizeOffset_Y = 40;
        sleekToggle1.addLabel(local.format("Tree_0_ToggleLabelText"), ESleekSide.RIGHT);
        sleekToggle1.state = LevelGround.resources[index].isTree_0;
        sleekToggle1.onToggled = new Toggled(EditorTerrainResourcesUI.onToggledTree_0_Toggle);
        sleekButton.add((Sleek) sleekToggle1);
        SleekToggle sleekToggle2 = new SleekToggle();
        sleekToggle2.positionOffset_Y = 150;
        sleekToggle2.sizeOffset_X = 40;
        sleekToggle2.sizeOffset_Y = 40;
        sleekToggle2.addLabel(local.format("Tree_1_ToggleLabelText"), ESleekSide.RIGHT);
        sleekToggle2.state = LevelGround.resources[index].isTree_1;
        sleekToggle2.onToggled = new Toggled(EditorTerrainResourcesUI.onToggledTree_1_Toggle);
        sleekButton.add((Sleek) sleekToggle2);
        SleekToggle sleekToggle3 = new SleekToggle();
        sleekToggle3.positionOffset_Y = 200;
        sleekToggle3.sizeOffset_X = 40;
        sleekToggle3.sizeOffset_Y = 40;
        sleekToggle3.addLabel(local.format("FlowerToggleLabelText"), ESleekSide.RIGHT);
        sleekToggle3.state = LevelGround.resources[index].isFlower;
        sleekToggle3.onToggled = new Toggled(EditorTerrainResourcesUI.onToggledFlowerToggle);
        sleekButton.add((Sleek) sleekToggle3);
        SleekToggle sleekToggle4 = new SleekToggle();
        sleekToggle4.positionOffset_Y = 250;
        sleekToggle4.sizeOffset_X = 40;
        sleekToggle4.sizeOffset_Y = 40;
        sleekToggle4.addLabel(local.format("RockToggleLabelText"), ESleekSide.RIGHT);
        sleekToggle4.state = LevelGround.resources[index].isRock;
        sleekToggle4.onToggled = new Toggled(EditorTerrainResourcesUI.onToggledRockToggle);
        sleekButton.add((Sleek) sleekToggle4);
        SleekToggle sleekToggle5 = new SleekToggle();
        sleekToggle5.positionOffset_Y = 300;
        sleekToggle5.sizeOffset_X = 40;
        sleekToggle5.sizeOffset_Y = 40;
        sleekToggle5.addLabel(local.format("SnowToggleLabelText"), ESleekSide.RIGHT);
        sleekToggle5.state = LevelGround.resources[index].isSnow;
        sleekToggle5.onToggled = new Toggled(EditorTerrainResourcesUI.onToggledSnowToggle);
        sleekButton.add((Sleek) sleekToggle5);
      }
      EditorTerrainResourcesUI.selectedBox = new SleekBox();
      EditorTerrainResourcesUI.selectedBox.positionOffset_X = -200;
      EditorTerrainResourcesUI.selectedBox.positionOffset_Y = 80;
      EditorTerrainResourcesUI.selectedBox.positionScale_X = 1f;
      EditorTerrainResourcesUI.selectedBox.sizeOffset_X = 200;
      EditorTerrainResourcesUI.selectedBox.sizeOffset_Y = 30;
      if ((int) EditorSpawns.selectedResource < LevelGround.resources.Length)
      {
        ResourceAsset resourceAsset = (ResourceAsset) Assets.find(EAssetType.RESOURCE, LevelGround.resources[(int) EditorSpawns.selectedResource].id);
        EditorTerrainResourcesUI.selectedBox.text = resourceAsset.resourceName;
      }
      EditorTerrainResourcesUI.selectedBox.addLabel(local.format("SelectionBoxLabelText"), ESleekSide.LEFT);
      EditorTerrainResourcesUI.container.add((Sleek) EditorTerrainResourcesUI.selectedBox);
      EditorTerrainResourcesUI.bakeGlobalResourcesButton = new SleekButtonIcon((Texture2D) bundle.load("Resources"));
      EditorTerrainResourcesUI.bakeGlobalResourcesButton.positionOffset_X = -200;
      EditorTerrainResourcesUI.bakeGlobalResourcesButton.positionOffset_Y = -70;
      EditorTerrainResourcesUI.bakeGlobalResourcesButton.positionScale_X = 1f;
      EditorTerrainResourcesUI.bakeGlobalResourcesButton.positionScale_Y = 1f;
      EditorTerrainResourcesUI.bakeGlobalResourcesButton.sizeOffset_X = 200;
      EditorTerrainResourcesUI.bakeGlobalResourcesButton.sizeOffset_Y = 30;
      EditorTerrainResourcesUI.bakeGlobalResourcesButton.text = local.format("BakeGlobalResourcesButtonText");
      EditorTerrainResourcesUI.bakeGlobalResourcesButton.tooltip = local.format("BakeGlobalResourcesButtonTooltip");
      EditorTerrainResourcesUI.bakeGlobalResourcesButton.onClickedButton = new ClickedButton(EditorTerrainResourcesUI.onClickedBakeGlobalResourcesButton);
      EditorTerrainResourcesUI.container.add((Sleek) EditorTerrainResourcesUI.bakeGlobalResourcesButton);
      EditorTerrainResourcesUI.bakeLocalResourcesButton = new SleekButtonIcon((Texture2D) bundle.load("Resources"));
      EditorTerrainResourcesUI.bakeLocalResourcesButton.positionOffset_X = -200;
      EditorTerrainResourcesUI.bakeLocalResourcesButton.positionOffset_Y = -30;
      EditorTerrainResourcesUI.bakeLocalResourcesButton.positionScale_X = 1f;
      EditorTerrainResourcesUI.bakeLocalResourcesButton.positionScale_Y = 1f;
      EditorTerrainResourcesUI.bakeLocalResourcesButton.sizeOffset_X = 200;
      EditorTerrainResourcesUI.bakeLocalResourcesButton.sizeOffset_Y = 30;
      EditorTerrainResourcesUI.bakeLocalResourcesButton.text = local.format("BakeLocalResourcesButtonText");
      EditorTerrainResourcesUI.bakeLocalResourcesButton.tooltip = local.format("BakeLocalResourcesButtonTooltip");
      EditorTerrainResourcesUI.bakeLocalResourcesButton.onClickedButton = new ClickedButton(EditorTerrainResourcesUI.onClickedBakeLocalResourcesButton);
      EditorTerrainResourcesUI.container.add((Sleek) EditorTerrainResourcesUI.bakeLocalResourcesButton);
      EditorTerrainResourcesUI.radiusSlider = new SleekSlider();
      EditorTerrainResourcesUI.radiusSlider.positionOffset_Y = -100;
      EditorTerrainResourcesUI.radiusSlider.positionScale_Y = 1f;
      EditorTerrainResourcesUI.radiusSlider.sizeOffset_X = 200;
      EditorTerrainResourcesUI.radiusSlider.sizeOffset_Y = 20;
      EditorTerrainResourcesUI.radiusSlider.state = (float) ((int) EditorSpawns.radius - (int) EditorSpawns.MIN_REMOVE_SIZE) / (float) EditorSpawns.MAX_REMOVE_SIZE;
      EditorTerrainResourcesUI.radiusSlider.orientation = ESleekOrientation.HORIZONTAL;
      EditorTerrainResourcesUI.radiusSlider.addLabel(local.format("RadiusSliderLabelText"), ESleekSide.RIGHT);
      EditorTerrainResourcesUI.radiusSlider.onDragged = new Dragged(EditorTerrainResourcesUI.onDraggedRadiusSlider);
      EditorTerrainResourcesUI.container.add((Sleek) EditorTerrainResourcesUI.radiusSlider);
      EditorTerrainResourcesUI.addButton = new SleekButtonIcon((Texture2D) bundle.load("Add"));
      EditorTerrainResourcesUI.addButton.positionOffset_Y = -70;
      EditorTerrainResourcesUI.addButton.positionScale_Y = 1f;
      EditorTerrainResourcesUI.addButton.sizeOffset_X = 200;
      EditorTerrainResourcesUI.addButton.sizeOffset_Y = 30;
      EditorTerrainResourcesUI.addButton.text = local.format("AddButtonText", (object) ControlsSettings.tool_0);
      EditorTerrainResourcesUI.addButton.tooltip = local.format("AddButtonTooltip");
      EditorTerrainResourcesUI.addButton.onClickedButton = new ClickedButton(EditorTerrainResourcesUI.onClickedAddButton);
      EditorTerrainResourcesUI.container.add((Sleek) EditorTerrainResourcesUI.addButton);
      EditorTerrainResourcesUI.removeButton = new SleekButtonIcon((Texture2D) bundle.load("Remove"));
      EditorTerrainResourcesUI.removeButton.positionOffset_Y = -30;
      EditorTerrainResourcesUI.removeButton.positionScale_Y = 1f;
      EditorTerrainResourcesUI.removeButton.sizeOffset_X = 200;
      EditorTerrainResourcesUI.removeButton.sizeOffset_Y = 30;
      EditorTerrainResourcesUI.removeButton.text = local.format("RemoveButtonText", (object) ControlsSettings.tool_1);
      EditorTerrainResourcesUI.removeButton.tooltip = local.format("RemoveButtonTooltip");
      EditorTerrainResourcesUI.removeButton.onClickedButton = new ClickedButton(EditorTerrainResourcesUI.onClickedRemoveButton);
      EditorTerrainResourcesUI.container.add((Sleek) EditorTerrainResourcesUI.removeButton);
      bundle.unload();
    }

    public static void open()
    {
      if (EditorTerrainResourcesUI.active)
      {
        EditorTerrainResourcesUI.close();
      }
      else
      {
        EditorTerrainResourcesUI.active = true;
        EditorSpawns.isSpawning = true;
        EditorSpawns.spawnMode = ESpawnMode.ADD_RESOURCE;
        EditorTerrainResourcesUI.container.lerpPositionScale(0.0f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
      }
    }

    public static void close()
    {
      if (!EditorTerrainResourcesUI.active)
        return;
      EditorTerrainResourcesUI.active = false;
      EditorSpawns.isSpawning = false;
      EditorTerrainResourcesUI.container.lerpPositionScale(1f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
    }

    private static void onClickedResourceButton(SleekButton button)
    {
      EditorSpawns.selectedResource = (byte) (button.positionOffset_Y / 350);
      ResourceAsset resourceAsset = (ResourceAsset) Assets.find(EAssetType.RESOURCE, LevelGround.resources[(int) EditorSpawns.selectedResource].id);
      EditorTerrainResourcesUI.selectedBox.text = resourceAsset.resourceName;
    }

    private static void onDraggedDensitySlider(SleekSlider slider, float state)
    {
      LevelGround.resources[slider.parent.positionOffset_Y / 350].density = state;
    }

    private static void onDraggedChanceSlider(SleekSlider slider, float state)
    {
      LevelGround.resources[slider.parent.positionOffset_Y / 350].chance = state;
    }

    private static void onToggledTree_0_Toggle(SleekToggle toggle, bool state)
    {
      LevelGround.resources[toggle.parent.positionOffset_Y / 350].isTree_0 = state;
    }

    private static void onToggledTree_1_Toggle(SleekToggle toggle, bool state)
    {
      LevelGround.resources[toggle.parent.positionOffset_Y / 350].isTree_1 = state;
    }

    private static void onToggledFlowerToggle(SleekToggle toggle, bool state)
    {
      LevelGround.resources[toggle.parent.positionOffset_Y / 350].isFlower = state;
    }

    private static void onToggledRockToggle(SleekToggle toggle, bool state)
    {
      LevelGround.resources[toggle.parent.positionOffset_Y / 350].isRock = state;
    }

    private static void onToggledSnowToggle(SleekToggle toggle, bool state)
    {
      LevelGround.resources[toggle.parent.positionOffset_Y / 350].isSnow = state;
    }

    private static void onClickedBakeGlobalResourcesButton(SleekButton button)
    {
      LevelGround.bakeGlobalResources();
    }

    private static void onClickedBakeLocalResourcesButton(SleekButton button)
    {
      LevelGround.bakeLocalResources();
    }

    private static void onDraggedRadiusSlider(SleekSlider slider, float state)
    {
      EditorSpawns.radius = (byte) ((double) EditorSpawns.MIN_REMOVE_SIZE + (double) state * (double) EditorSpawns.MAX_REMOVE_SIZE);
    }

    private static void onClickedAddButton(SleekButton button)
    {
      EditorSpawns.spawnMode = ESpawnMode.ADD_RESOURCE;
    }

    private static void onClickedRemoveButton(SleekButton button)
    {
      EditorSpawns.spawnMode = ESpawnMode.REMOVE_RESOURCE;
    }
  }
}
