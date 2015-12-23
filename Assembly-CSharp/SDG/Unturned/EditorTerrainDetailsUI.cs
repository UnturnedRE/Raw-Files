// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.EditorTerrainDetailsUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class EditorTerrainDetailsUI
  {
    private static Sleek container;
    public static bool active;
    private static SleekScrollBox detailsScrollBox;
    private static SleekButtonIcon bakeDetailsButton;

    public EditorTerrainDetailsUI()
    {
      Local local = Localization.read("/Editor/EditorTerrainDetails.dat");
      Bundle bundle = Bundles.getBundle("/Bundles/Textures/Edit/Icons/EditorTerrainDetails/EditorTerrainDetails.unity3d");
      EditorTerrainDetailsUI.container = new Sleek();
      EditorTerrainDetailsUI.container.positionOffset_X = 10;
      EditorTerrainDetailsUI.container.positionOffset_Y = 10;
      EditorTerrainDetailsUI.container.positionScale_X = 1f;
      EditorTerrainDetailsUI.container.sizeOffset_X = -20;
      EditorTerrainDetailsUI.container.sizeOffset_Y = -20;
      EditorTerrainDetailsUI.container.sizeScale_X = 1f;
      EditorTerrainDetailsUI.container.sizeScale_Y = 1f;
      EditorUI.window.add(EditorTerrainDetailsUI.container);
      EditorTerrainDetailsUI.active = false;
      EditorTerrainDetailsUI.detailsScrollBox = new SleekScrollBox();
      EditorTerrainDetailsUI.detailsScrollBox.positionOffset_Y = 80;
      EditorTerrainDetailsUI.detailsScrollBox.positionOffset_X = -400;
      EditorTerrainDetailsUI.detailsScrollBox.positionScale_X = 1f;
      EditorTerrainDetailsUI.detailsScrollBox.sizeOffset_X = 400;
      EditorTerrainDetailsUI.detailsScrollBox.sizeOffset_Y = -120;
      EditorTerrainDetailsUI.detailsScrollBox.sizeScale_Y = 1f;
      EditorTerrainDetailsUI.detailsScrollBox.area = new Rect(0.0f, 0.0f, 5f, (float) (LevelGround.details.Length * 350 - 10));
      EditorTerrainDetailsUI.container.add((Sleek) EditorTerrainDetailsUI.detailsScrollBox);
      for (int index = 0; index < LevelGround.details.Length; ++index)
      {
        GroundDetail groundDetail = LevelGround.details[index];
        SleekImageTexture sleekImageTexture = new SleekImageTexture();
        sleekImageTexture.positionOffset_X = 200;
        sleekImageTexture.positionOffset_Y = index * 350;
        sleekImageTexture.sizeOffset_X = 64;
        sleekImageTexture.sizeOffset_Y = 32;
        sleekImageTexture.texture = (Texture) groundDetail.prototype.prototypeTexture;
        EditorTerrainDetailsUI.detailsScrollBox.add((Sleek) sleekImageTexture);
        SleekBox sleekBox = new SleekBox();
        sleekBox.sizeOffset_Y = 32;
        if ((Object) groundDetail.prototype.prototypeTexture != (Object) null)
        {
          sleekBox.positionOffset_X = 70;
          sleekBox.sizeOffset_X = 100;
          sleekBox.text = LevelGround.details[index].prototype.prototypeTexture.name;
        }
        else
        {
          sleekBox.sizeOffset_X = 170;
          sleekBox.text = LevelGround.details[index].prototype.prototype.name;
        }
        sleekImageTexture.add((Sleek) sleekBox);
        SleekSlider sleekSlider1 = new SleekSlider();
        sleekSlider1.positionOffset_Y = 40;
        sleekSlider1.sizeOffset_X = 170;
        sleekSlider1.sizeOffset_Y = 20;
        sleekSlider1.orientation = ESleekOrientation.HORIZONTAL;
        sleekSlider1.addLabel(local.format("DensitySliderLabelText"), ESleekSide.LEFT);
        sleekSlider1.state = LevelGround.details[index].density;
        sleekSlider1.onDragged = new Dragged(EditorTerrainDetailsUI.onDraggedDensitySlider);
        sleekImageTexture.add((Sleek) sleekSlider1);
        SleekSlider sleekSlider2 = new SleekSlider();
        sleekSlider2.positionOffset_Y = 70;
        sleekSlider2.sizeOffset_X = 170;
        sleekSlider2.sizeOffset_Y = 20;
        sleekSlider2.orientation = ESleekOrientation.HORIZONTAL;
        sleekSlider2.addLabel(local.format("ChanceSliderLabelText"), ESleekSide.LEFT);
        sleekSlider2.state = LevelGround.details[index].chance;
        sleekSlider2.onDragged = new Dragged(EditorTerrainDetailsUI.onDraggedChanceSlider);
        sleekImageTexture.add((Sleek) sleekSlider2);
        SleekToggle sleekToggle1 = new SleekToggle();
        sleekToggle1.positionOffset_Y = 100;
        sleekToggle1.sizeOffset_X = 40;
        sleekToggle1.sizeOffset_Y = 40;
        sleekToggle1.addLabel(local.format("Grass_0_ToggleLabelText"), ESleekSide.RIGHT);
        sleekToggle1.state = LevelGround.details[index].isGrass_0;
        sleekToggle1.onToggled = new Toggled(EditorTerrainDetailsUI.onToggledGrass_0_Toggle);
        sleekImageTexture.add((Sleek) sleekToggle1);
        SleekToggle sleekToggle2 = new SleekToggle();
        sleekToggle2.positionOffset_Y = 150;
        sleekToggle2.sizeOffset_X = 40;
        sleekToggle2.sizeOffset_Y = 40;
        sleekToggle2.addLabel(local.format("Grass_1_ToggleLabelText"), ESleekSide.RIGHT);
        sleekToggle2.state = LevelGround.details[index].isGrass_1;
        sleekToggle2.onToggled = new Toggled(EditorTerrainDetailsUI.onToggledGrass_1_Toggle);
        sleekImageTexture.add((Sleek) sleekToggle2);
        SleekToggle sleekToggle3 = new SleekToggle();
        sleekToggle3.positionOffset_Y = 200;
        sleekToggle3.sizeOffset_X = 40;
        sleekToggle3.sizeOffset_Y = 40;
        sleekToggle3.addLabel(local.format("FlowerToggleLabelText"), ESleekSide.RIGHT);
        sleekToggle3.state = LevelGround.details[index].isFlower;
        sleekToggle3.onToggled = new Toggled(EditorTerrainDetailsUI.onToggledFlowerToggle);
        sleekImageTexture.add((Sleek) sleekToggle3);
        SleekToggle sleekToggle4 = new SleekToggle();
        sleekToggle4.positionOffset_Y = 250;
        sleekToggle4.sizeOffset_X = 40;
        sleekToggle4.sizeOffset_Y = 40;
        sleekToggle4.addLabel(local.format("RockToggleLabelText"), ESleekSide.RIGHT);
        sleekToggle4.state = LevelGround.details[index].isRock;
        sleekToggle4.onToggled = new Toggled(EditorTerrainDetailsUI.onToggledRockToggle);
        sleekImageTexture.add((Sleek) sleekToggle4);
        SleekToggle sleekToggle5 = new SleekToggle();
        sleekToggle5.positionOffset_Y = 300;
        sleekToggle5.sizeOffset_X = 40;
        sleekToggle5.sizeOffset_Y = 40;
        sleekToggle5.addLabel(local.format("SnowToggleLabelText"), ESleekSide.RIGHT);
        sleekToggle5.state = LevelGround.details[index].isSnow;
        sleekToggle5.onToggled = new Toggled(EditorTerrainDetailsUI.onToggledSnowToggle);
        sleekImageTexture.add((Sleek) sleekToggle5);
      }
      EditorTerrainDetailsUI.bakeDetailsButton = new SleekButtonIcon((Texture2D) bundle.load("Details"));
      EditorTerrainDetailsUI.bakeDetailsButton.positionOffset_X = -200;
      EditorTerrainDetailsUI.bakeDetailsButton.positionOffset_Y = -30;
      EditorTerrainDetailsUI.bakeDetailsButton.positionScale_X = 1f;
      EditorTerrainDetailsUI.bakeDetailsButton.positionScale_Y = 1f;
      EditorTerrainDetailsUI.bakeDetailsButton.sizeOffset_X = 200;
      EditorTerrainDetailsUI.bakeDetailsButton.sizeOffset_Y = 30;
      EditorTerrainDetailsUI.bakeDetailsButton.text = local.format("BakeDetailsButtonText");
      EditorTerrainDetailsUI.bakeDetailsButton.tooltip = local.format("BakeDetailsButtonTooltip");
      EditorTerrainDetailsUI.bakeDetailsButton.onClickedButton = new ClickedButton(EditorTerrainDetailsUI.onClickedBakeDetailsButton);
      EditorTerrainDetailsUI.container.add((Sleek) EditorTerrainDetailsUI.bakeDetailsButton);
      bundle.unload();
    }

    public static void open()
    {
      if (EditorTerrainDetailsUI.active)
      {
        EditorTerrainDetailsUI.close();
      }
      else
      {
        EditorTerrainDetailsUI.active = true;
        EditorTerrainDetailsUI.container.lerpPositionScale(0.0f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
      }
    }

    public static void close()
    {
      if (!EditorTerrainDetailsUI.active)
        return;
      EditorTerrainDetailsUI.active = false;
      EditorTerrainDetailsUI.container.lerpPositionScale(1f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
    }

    private static void onDraggedDensitySlider(SleekSlider slider, float state)
    {
      LevelGround.details[slider.parent.positionOffset_Y / 350].density = state;
    }

    private static void onDraggedChanceSlider(SleekSlider slider, float state)
    {
      LevelGround.details[slider.parent.positionOffset_Y / 350].chance = state;
    }

    private static void onToggledGrass_0_Toggle(SleekToggle toggle, bool state)
    {
      LevelGround.details[toggle.parent.positionOffset_Y / 350].isGrass_0 = state;
    }

    private static void onToggledGrass_1_Toggle(SleekToggle toggle, bool state)
    {
      LevelGround.details[toggle.parent.positionOffset_Y / 350].isGrass_1 = state;
    }

    private static void onToggledFlowerToggle(SleekToggle toggle, bool state)
    {
      LevelGround.details[toggle.parent.positionOffset_Y / 350].isFlower = state;
    }

    private static void onToggledRockToggle(SleekToggle toggle, bool state)
    {
      LevelGround.details[toggle.parent.positionOffset_Y / 350].isRock = state;
    }

    private static void onToggledSnowToggle(SleekToggle toggle, bool state)
    {
      LevelGround.details[toggle.parent.positionOffset_Y / 350].isSnow = state;
    }

    private static void onClickedBakeDetailsButton(SleekButton button)
    {
      LevelGround.bakeDetails();
    }
  }
}
