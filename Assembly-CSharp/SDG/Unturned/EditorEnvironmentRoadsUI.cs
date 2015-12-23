// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.EditorEnvironmentRoadsUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class EditorEnvironmentRoadsUI
  {
    private static Sleek container;
    public static bool active;
    private static SleekScrollBox roadScrollBox;
    private static SleekBox selectedBox;
    private static SleekButtonIcon bakeRoadsButton;

    public EditorEnvironmentRoadsUI()
    {
      Local local = Localization.read("/Editor/EditorEnvironmentRoads.dat");
      Bundle bundle = Bundles.getBundle("/Bundles/Textures/Edit/Icons/EditorEnvironmentRoads/EditorEnvironmentRoads.unity3d");
      EditorEnvironmentRoadsUI.container = new Sleek();
      EditorEnvironmentRoadsUI.container.positionOffset_X = 10;
      EditorEnvironmentRoadsUI.container.positionOffset_Y = 10;
      EditorEnvironmentRoadsUI.container.positionScale_X = 1f;
      EditorEnvironmentRoadsUI.container.sizeOffset_X = -20;
      EditorEnvironmentRoadsUI.container.sizeOffset_Y = -20;
      EditorEnvironmentRoadsUI.container.sizeScale_X = 1f;
      EditorEnvironmentRoadsUI.container.sizeScale_Y = 1f;
      EditorUI.window.add(EditorEnvironmentRoadsUI.container);
      EditorEnvironmentRoadsUI.active = false;
      EditorEnvironmentRoadsUI.roadScrollBox = new SleekScrollBox();
      EditorEnvironmentRoadsUI.roadScrollBox.positionOffset_X = -400;
      EditorEnvironmentRoadsUI.roadScrollBox.positionOffset_Y = 120;
      EditorEnvironmentRoadsUI.roadScrollBox.positionScale_X = 1f;
      EditorEnvironmentRoadsUI.roadScrollBox.sizeOffset_X = 400;
      EditorEnvironmentRoadsUI.roadScrollBox.sizeOffset_Y = -160;
      EditorEnvironmentRoadsUI.roadScrollBox.sizeScale_Y = 1f;
      EditorEnvironmentRoadsUI.roadScrollBox.area = new Rect(0.0f, 0.0f, 5f, (float) (LevelRoads.materials.Length * 240 - 10));
      EditorEnvironmentRoadsUI.container.add((Sleek) EditorEnvironmentRoadsUI.roadScrollBox);
      for (int index = 0; index < LevelRoads.materials.Length; ++index)
      {
        SleekImageTexture sleekImageTexture = new SleekImageTexture();
        sleekImageTexture.positionOffset_X = 200;
        sleekImageTexture.positionOffset_Y = index * 240;
        sleekImageTexture.sizeOffset_X = 64;
        sleekImageTexture.sizeOffset_Y = 64;
        sleekImageTexture.texture = LevelRoads.materials[index].material.mainTexture;
        EditorEnvironmentRoadsUI.roadScrollBox.add((Sleek) sleekImageTexture);
        SleekButton sleekButton = new SleekButton();
        sleekButton.positionOffset_X = 70;
        sleekButton.sizeOffset_X = 100;
        sleekButton.sizeOffset_Y = 64;
        sleekButton.text = LevelRoads.materials[index].material.mainTexture.name;
        sleekButton.onClickedButton = new ClickedButton(EditorEnvironmentRoadsUI.onClickedRoadButton);
        sleekImageTexture.add((Sleek) sleekButton);
        SleekField sleekField1 = new SleekField();
        sleekField1.positionOffset_Y = 70;
        sleekField1.sizeOffset_X = 170;
        sleekField1.sizeOffset_Y = 30;
        sleekField1.text = LevelRoads.materials[index].width.ToString();
        sleekField1.addLabel(local.format("WidthFieldLabelText"), ESleekSide.LEFT);
        sleekField1.onTyped = new Typed(EditorEnvironmentRoadsUI.onTypedWidthField);
        sleekImageTexture.add((Sleek) sleekField1);
        SleekField sleekField2 = new SleekField();
        sleekField2.positionOffset_Y = 110;
        sleekField2.sizeOffset_X = 170;
        sleekField2.sizeOffset_Y = 30;
        sleekField2.text = LevelRoads.materials[index].height.ToString();
        sleekField2.addLabel(local.format("HeightFieldLabelText"), ESleekSide.LEFT);
        sleekField2.onTyped = new Typed(EditorEnvironmentRoadsUI.onTypedHeightField);
        sleekImageTexture.add((Sleek) sleekField2);
        SleekField sleekField3 = new SleekField();
        sleekField3.positionOffset_Y = 150;
        sleekField3.sizeOffset_X = 170;
        sleekField3.sizeOffset_Y = 30;
        sleekField3.text = LevelRoads.materials[index].depth.ToString();
        sleekField3.addLabel(local.format("DepthFieldLabelText"), ESleekSide.LEFT);
        sleekField3.onTyped = new Typed(EditorEnvironmentRoadsUI.onTypedDepthField);
        sleekImageTexture.add((Sleek) sleekField3);
        SleekToggle sleekToggle = new SleekToggle();
        sleekToggle.positionOffset_Y = 190;
        sleekToggle.sizeOffset_X = 40;
        sleekToggle.sizeOffset_Y = 40;
        sleekToggle.addLabel(local.format("ConcreteToggleLabelText"), ESleekSide.RIGHT);
        sleekToggle.state = LevelRoads.materials[index].isConcrete;
        sleekToggle.onToggled = new Toggled(EditorEnvironmentRoadsUI.onToggledConcreteToggle);
        sleekImageTexture.add((Sleek) sleekToggle);
      }
      EditorEnvironmentRoadsUI.selectedBox = new SleekBox();
      EditorEnvironmentRoadsUI.selectedBox.positionOffset_X = -200;
      EditorEnvironmentRoadsUI.selectedBox.positionOffset_Y = 80;
      EditorEnvironmentRoadsUI.selectedBox.positionScale_X = 1f;
      EditorEnvironmentRoadsUI.selectedBox.sizeOffset_X = 200;
      EditorEnvironmentRoadsUI.selectedBox.sizeOffset_Y = 30;
      if ((int) EditorRoads.selected < LevelRoads.materials.Length)
        EditorEnvironmentRoadsUI.selectedBox.text = LevelRoads.materials[(int) EditorRoads.selected].material.mainTexture.name;
      EditorEnvironmentRoadsUI.selectedBox.addLabel(local.format("SelectionBoxLabelText"), ESleekSide.LEFT);
      EditorEnvironmentRoadsUI.container.add((Sleek) EditorEnvironmentRoadsUI.selectedBox);
      EditorEnvironmentRoadsUI.bakeRoadsButton = new SleekButtonIcon((Texture2D) bundle.load("Roads"));
      EditorEnvironmentRoadsUI.bakeRoadsButton.positionOffset_X = -200;
      EditorEnvironmentRoadsUI.bakeRoadsButton.positionOffset_Y = -30;
      EditorEnvironmentRoadsUI.bakeRoadsButton.positionScale_X = 1f;
      EditorEnvironmentRoadsUI.bakeRoadsButton.positionScale_Y = 1f;
      EditorEnvironmentRoadsUI.bakeRoadsButton.sizeOffset_X = 200;
      EditorEnvironmentRoadsUI.bakeRoadsButton.sizeOffset_Y = 30;
      EditorEnvironmentRoadsUI.bakeRoadsButton.text = local.format("BakeRoadsButtonText");
      EditorEnvironmentRoadsUI.bakeRoadsButton.tooltip = local.format("BakeRoadsButtonTooltip");
      EditorEnvironmentRoadsUI.bakeRoadsButton.onClickedButton = new ClickedButton(EditorEnvironmentRoadsUI.onClickedBakeRoadsButton);
      EditorEnvironmentRoadsUI.container.add((Sleek) EditorEnvironmentRoadsUI.bakeRoadsButton);
      bundle.unload();
    }

    public static void open()
    {
      if (EditorEnvironmentRoadsUI.active)
      {
        EditorEnvironmentRoadsUI.close();
      }
      else
      {
        EditorEnvironmentRoadsUI.active = true;
        EditorRoads.isPaving = true;
        EditorUI.message(EEditorMessage.ROADS);
        EditorEnvironmentRoadsUI.container.lerpPositionScale(0.0f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
      }
    }

    public static void close()
    {
      if (!EditorEnvironmentRoadsUI.active)
        return;
      EditorEnvironmentRoadsUI.active = false;
      EditorRoads.isPaving = false;
      EditorEnvironmentRoadsUI.container.lerpPositionScale(1f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
    }

    private static void onClickedRoadButton(SleekButton button)
    {
      EditorRoads.selected = (byte) (button.parent.positionOffset_Y / 240);
      if (EditorRoads.road != null)
        EditorRoads.road.material = EditorRoads.selected;
      EditorEnvironmentRoadsUI.selectedBox.text = LevelRoads.materials[(int) EditorRoads.selected].material.mainTexture.name;
    }

    private static void onTypedWidthField(SleekField field, string text)
    {
      float result;
      if (!float.TryParse(text, out result))
        return;
      LevelRoads.materials[field.parent.positionOffset_Y / 240].width = result;
    }

    private static void onTypedHeightField(SleekField field, string text)
    {
      float result;
      if (!float.TryParse(text, out result))
        return;
      LevelRoads.materials[field.parent.positionOffset_Y / 240].height = result;
    }

    private static void onTypedDepthField(SleekField field, string text)
    {
      float result;
      if (!float.TryParse(text, out result))
        return;
      LevelRoads.materials[field.parent.positionOffset_Y / 240].depth = result;
    }

    private static void onToggledConcreteToggle(SleekToggle toggle, bool state)
    {
      LevelRoads.materials[toggle.parent.positionOffset_Y / 240].isConcrete = state;
    }

    private static void onClickedBakeRoadsButton(SleekButton button)
    {
      LevelRoads.bakeRoads();
    }
  }
}
