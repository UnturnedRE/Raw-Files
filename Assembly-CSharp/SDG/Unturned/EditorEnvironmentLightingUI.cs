// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.EditorEnvironmentLightingUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class EditorEnvironmentLightingUI
  {
    private static Sleek container;
    public static bool active;
    private static SleekScrollBox lightingScrollBox;
    private static SleekSlider azimuthSlider;
    private static SleekSlider biasSlider;
    private static SleekSlider fadeSlider;
    private static SleekButton[] timeButtons;
    private static SleekBox[] infoBoxes;
    private static SleekColorPicker[] colorPickers;
    private static SleekSlider[] singleSliders;
    private static ELightingTime selectedTime;
    private static SleekValue seaLevelSlider;
    private static SleekValue snowLevelSlider;
    private static SleekSlider moonSlider;
    private static SleekSlider timeSlider;

    public EditorEnvironmentLightingUI()
    {
      Local local = Localization.read("/Editor/EditorEnvironmentLighting.dat");
      EditorEnvironmentLightingUI.container = new Sleek();
      EditorEnvironmentLightingUI.container.positionOffset_X = 10;
      EditorEnvironmentLightingUI.container.positionOffset_Y = 10;
      EditorEnvironmentLightingUI.container.positionScale_X = 1f;
      EditorEnvironmentLightingUI.container.sizeOffset_X = -20;
      EditorEnvironmentLightingUI.container.sizeOffset_Y = -20;
      EditorEnvironmentLightingUI.container.sizeScale_X = 1f;
      EditorEnvironmentLightingUI.container.sizeScale_Y = 1f;
      EditorUI.window.add(EditorEnvironmentLightingUI.container);
      EditorEnvironmentLightingUI.active = false;
      EditorEnvironmentLightingUI.selectedTime = ELightingTime.DAWN;
      EditorEnvironmentLightingUI.azimuthSlider = new SleekSlider();
      EditorEnvironmentLightingUI.azimuthSlider.positionOffset_X = -230;
      EditorEnvironmentLightingUI.azimuthSlider.positionOffset_Y = 80;
      EditorEnvironmentLightingUI.azimuthSlider.positionScale_X = 1f;
      EditorEnvironmentLightingUI.azimuthSlider.sizeOffset_X = 230;
      EditorEnvironmentLightingUI.azimuthSlider.sizeOffset_Y = 20;
      EditorEnvironmentLightingUI.azimuthSlider.state = LevelLighting.azimuth / 360f;
      EditorEnvironmentLightingUI.azimuthSlider.orientation = ESleekOrientation.HORIZONTAL;
      EditorEnvironmentLightingUI.azimuthSlider.addLabel(local.format("AzimuthSliderLabelText"), ESleekSide.LEFT);
      EditorEnvironmentLightingUI.azimuthSlider.onDragged = new Dragged(EditorEnvironmentLightingUI.onDraggedAzimuthSlider);
      EditorEnvironmentLightingUI.container.add((Sleek) EditorEnvironmentLightingUI.azimuthSlider);
      EditorEnvironmentLightingUI.biasSlider = new SleekSlider();
      EditorEnvironmentLightingUI.biasSlider.positionOffset_X = -230;
      EditorEnvironmentLightingUI.biasSlider.positionOffset_Y = 110;
      EditorEnvironmentLightingUI.biasSlider.positionScale_X = 1f;
      EditorEnvironmentLightingUI.biasSlider.sizeOffset_X = 230;
      EditorEnvironmentLightingUI.biasSlider.sizeOffset_Y = 20;
      EditorEnvironmentLightingUI.biasSlider.state = LevelLighting.bias;
      EditorEnvironmentLightingUI.biasSlider.orientation = ESleekOrientation.HORIZONTAL;
      EditorEnvironmentLightingUI.biasSlider.addLabel(local.format("BiasSliderLabelText"), ESleekSide.LEFT);
      EditorEnvironmentLightingUI.biasSlider.onDragged = new Dragged(EditorEnvironmentLightingUI.onDraggedBiasSlider);
      EditorEnvironmentLightingUI.container.add((Sleek) EditorEnvironmentLightingUI.biasSlider);
      EditorEnvironmentLightingUI.fadeSlider = new SleekSlider();
      EditorEnvironmentLightingUI.fadeSlider.positionOffset_X = -230;
      EditorEnvironmentLightingUI.fadeSlider.positionOffset_Y = 140;
      EditorEnvironmentLightingUI.fadeSlider.positionScale_X = 1f;
      EditorEnvironmentLightingUI.fadeSlider.sizeOffset_X = 230;
      EditorEnvironmentLightingUI.fadeSlider.sizeOffset_Y = 20;
      EditorEnvironmentLightingUI.fadeSlider.state = LevelLighting.fade;
      EditorEnvironmentLightingUI.fadeSlider.orientation = ESleekOrientation.HORIZONTAL;
      EditorEnvironmentLightingUI.fadeSlider.addLabel(local.format("FadeSliderLabelText"), ESleekSide.LEFT);
      EditorEnvironmentLightingUI.fadeSlider.onDragged = new Dragged(EditorEnvironmentLightingUI.onDraggedFadeSlider);
      EditorEnvironmentLightingUI.container.add((Sleek) EditorEnvironmentLightingUI.fadeSlider);
      EditorEnvironmentLightingUI.lightingScrollBox = new SleekScrollBox();
      EditorEnvironmentLightingUI.lightingScrollBox.positionOffset_X = -470;
      EditorEnvironmentLightingUI.lightingScrollBox.positionOffset_Y = 170;
      EditorEnvironmentLightingUI.lightingScrollBox.positionScale_X = 1f;
      EditorEnvironmentLightingUI.lightingScrollBox.sizeOffset_X = 470;
      EditorEnvironmentLightingUI.lightingScrollBox.sizeOffset_Y = -170;
      EditorEnvironmentLightingUI.lightingScrollBox.sizeScale_Y = 1f;
      EditorEnvironmentLightingUI.container.add((Sleek) EditorEnvironmentLightingUI.lightingScrollBox);
      EditorEnvironmentLightingUI.seaLevelSlider = new SleekValue();
      EditorEnvironmentLightingUI.seaLevelSlider.positionOffset_Y = -130;
      EditorEnvironmentLightingUI.seaLevelSlider.positionScale_Y = 1f;
      EditorEnvironmentLightingUI.seaLevelSlider.sizeOffset_X = 200;
      EditorEnvironmentLightingUI.seaLevelSlider.sizeOffset_Y = 30;
      EditorEnvironmentLightingUI.seaLevelSlider.state = LevelLighting.seaLevel;
      EditorEnvironmentLightingUI.seaLevelSlider.addLabel(local.format("Sea_Level_Slider_Label"), ESleekSide.RIGHT);
      EditorEnvironmentLightingUI.seaLevelSlider.onValued = new Valued(EditorEnvironmentLightingUI.onValuedSeaLevelSlider);
      EditorEnvironmentLightingUI.container.add((Sleek) EditorEnvironmentLightingUI.seaLevelSlider);
      EditorEnvironmentLightingUI.snowLevelSlider = new SleekValue();
      EditorEnvironmentLightingUI.snowLevelSlider.positionOffset_Y = -90;
      EditorEnvironmentLightingUI.snowLevelSlider.positionScale_Y = 1f;
      EditorEnvironmentLightingUI.snowLevelSlider.sizeOffset_X = 200;
      EditorEnvironmentLightingUI.snowLevelSlider.sizeOffset_Y = 30;
      EditorEnvironmentLightingUI.snowLevelSlider.state = LevelLighting.snowLevel;
      EditorEnvironmentLightingUI.snowLevelSlider.addLabel(local.format("Snow_Level_Slider_Label"), ESleekSide.RIGHT);
      EditorEnvironmentLightingUI.snowLevelSlider.onValued = new Valued(EditorEnvironmentLightingUI.onValuedSnowLevelSlider);
      EditorEnvironmentLightingUI.container.add((Sleek) EditorEnvironmentLightingUI.snowLevelSlider);
      EditorEnvironmentLightingUI.moonSlider = new SleekSlider();
      EditorEnvironmentLightingUI.moonSlider.positionOffset_Y = -50;
      EditorEnvironmentLightingUI.moonSlider.positionScale_Y = 1f;
      EditorEnvironmentLightingUI.moonSlider.sizeOffset_X = 200;
      EditorEnvironmentLightingUI.moonSlider.sizeOffset_Y = 20;
      EditorEnvironmentLightingUI.moonSlider.state = (float) LevelLighting.moon / (float) LevelLighting.MOON_CYCLES;
      EditorEnvironmentLightingUI.moonSlider.orientation = ESleekOrientation.HORIZONTAL;
      EditorEnvironmentLightingUI.moonSlider.addLabel(local.format("MoonSliderLabelText"), ESleekSide.RIGHT);
      EditorEnvironmentLightingUI.moonSlider.onDragged = new Dragged(EditorEnvironmentLightingUI.onDraggedMoonSlider);
      EditorEnvironmentLightingUI.container.add((Sleek) EditorEnvironmentLightingUI.moonSlider);
      EditorEnvironmentLightingUI.timeSlider = new SleekSlider();
      EditorEnvironmentLightingUI.timeSlider.positionOffset_Y = -20;
      EditorEnvironmentLightingUI.timeSlider.positionScale_Y = 1f;
      EditorEnvironmentLightingUI.timeSlider.sizeOffset_X = 200;
      EditorEnvironmentLightingUI.timeSlider.sizeOffset_Y = 20;
      EditorEnvironmentLightingUI.timeSlider.state = LevelLighting.time;
      EditorEnvironmentLightingUI.timeSlider.orientation = ESleekOrientation.HORIZONTAL;
      EditorEnvironmentLightingUI.timeSlider.addLabel(local.format("TimeSliderLabelText"), ESleekSide.RIGHT);
      EditorEnvironmentLightingUI.timeSlider.onDragged = new Dragged(EditorEnvironmentLightingUI.onDraggedTimeSlider);
      EditorEnvironmentLightingUI.container.add((Sleek) EditorEnvironmentLightingUI.timeSlider);
      EditorEnvironmentLightingUI.timeButtons = new SleekButton[4];
      for (int index = 0; index < EditorEnvironmentLightingUI.timeButtons.Length; ++index)
      {
        SleekButton sleekButton = new SleekButton();
        sleekButton.positionOffset_X = 240;
        sleekButton.positionOffset_Y = index * 40;
        sleekButton.sizeOffset_X = 200;
        sleekButton.sizeOffset_Y = 30;
        sleekButton.text = local.format("Time_" + (object) index);
        sleekButton.onClickedButton += new ClickedButton(EditorEnvironmentLightingUI.onClickedTimeButton);
        EditorEnvironmentLightingUI.lightingScrollBox.add((Sleek) sleekButton);
        EditorEnvironmentLightingUI.timeButtons[index] = sleekButton;
      }
      EditorEnvironmentLightingUI.infoBoxes = new SleekBox[10];
      EditorEnvironmentLightingUI.colorPickers = new SleekColorPicker[EditorEnvironmentLightingUI.infoBoxes.Length];
      EditorEnvironmentLightingUI.singleSliders = new SleekSlider[4];
      for (int index = 0; index < EditorEnvironmentLightingUI.colorPickers.Length; ++index)
      {
        SleekBox sleekBox = new SleekBox();
        sleekBox.positionOffset_X = 240;
        sleekBox.positionOffset_Y = EditorEnvironmentLightingUI.timeButtons.Length * 40 + index * 170;
        sleekBox.sizeOffset_X = 200;
        sleekBox.sizeOffset_Y = 30;
        sleekBox.text = local.format("Color_" + (object) index);
        EditorEnvironmentLightingUI.lightingScrollBox.add((Sleek) sleekBox);
        EditorEnvironmentLightingUI.infoBoxes[index] = sleekBox;
        SleekColorPicker sleekColorPicker = new SleekColorPicker();
        sleekColorPicker.positionOffset_X = 200;
        sleekColorPicker.positionOffset_Y = EditorEnvironmentLightingUI.timeButtons.Length * 40 + index * 170 + 40;
        sleekColorPicker.onColorPicked += new ColorPicked(EditorEnvironmentLightingUI.onPickedColorPicker);
        EditorEnvironmentLightingUI.lightingScrollBox.add((Sleek) sleekColorPicker);
        EditorEnvironmentLightingUI.colorPickers[index] = sleekColorPicker;
      }
      for (int index = 0; index < EditorEnvironmentLightingUI.singleSliders.Length; ++index)
      {
        SleekSlider sleekSlider = new SleekSlider();
        sleekSlider.positionOffset_X = 240;
        sleekSlider.positionOffset_Y = EditorEnvironmentLightingUI.timeButtons.Length * 40 + EditorEnvironmentLightingUI.colorPickers.Length * 170 + index * 30;
        sleekSlider.sizeOffset_X = 200;
        sleekSlider.sizeOffset_Y = 20;
        sleekSlider.orientation = ESleekOrientation.HORIZONTAL;
        sleekSlider.addLabel(local.format("Single_" + (object) index), ESleekSide.LEFT);
        sleekSlider.onDragged += new Dragged(EditorEnvironmentLightingUI.onDraggedSingleSlider);
        EditorEnvironmentLightingUI.lightingScrollBox.add((Sleek) sleekSlider);
        EditorEnvironmentLightingUI.singleSliders[index] = sleekSlider;
      }
      EditorEnvironmentLightingUI.lightingScrollBox.area = new Rect(0.0f, 0.0f, 5f, (float) (EditorEnvironmentLightingUI.timeButtons.Length * 40 + EditorEnvironmentLightingUI.colorPickers.Length * 170 + EditorEnvironmentLightingUI.singleSliders.Length * 30 - 10));
      EditorEnvironmentLightingUI.updateSelection();
    }

    public static void open()
    {
      if (EditorEnvironmentLightingUI.active)
      {
        EditorEnvironmentLightingUI.close();
      }
      else
      {
        EditorEnvironmentLightingUI.active = true;
        EditorEnvironmentLightingUI.container.lerpPositionScale(0.0f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
      }
    }

    public static void close()
    {
      if (!EditorEnvironmentLightingUI.active)
        return;
      EditorEnvironmentLightingUI.active = false;
      EditorEnvironmentLightingUI.container.lerpPositionScale(1f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
    }

    private static void onDraggedAzimuthSlider(SleekSlider slider, float state)
    {
      LevelLighting.azimuth = state * 360f;
    }

    private static void onDraggedBiasSlider(SleekSlider slider, float state)
    {
      LevelLighting.bias = state;
    }

    private static void onDraggedFadeSlider(SleekSlider slider, float state)
    {
      LevelLighting.fade = state;
    }

    private static void onValuedSeaLevelSlider(SleekValue slider, float state)
    {
      LevelLighting.seaLevel = state;
    }

    private static void onValuedSnowLevelSlider(SleekValue slider, float state)
    {
      LevelLighting.snowLevel = state;
    }

    private static void onDraggedMoonSlider(SleekSlider slider, float state)
    {
      byte num = (byte) ((double) state * (double) LevelLighting.MOON_CYCLES);
      if ((int) num >= (int) LevelLighting.MOON_CYCLES)
        num = (byte) ((uint) LevelLighting.MOON_CYCLES - 1U);
      LevelLighting.moon = num;
    }

    private static void onDraggedTimeSlider(SleekSlider slider, float state)
    {
      LevelLighting.time = state;
    }

    private static void onClickedTimeButton(SleekButton button)
    {
      int index = 0;
      while (index < EditorEnvironmentLightingUI.timeButtons.Length && EditorEnvironmentLightingUI.timeButtons[index] != button)
        ++index;
      EditorEnvironmentLightingUI.selectedTime = (ELightingTime) index;
      EditorEnvironmentLightingUI.updateSelection();
      switch (EditorEnvironmentLightingUI.selectedTime)
      {
        case ELightingTime.DAWN:
          LevelLighting.time = 0.0f;
          break;
        case ELightingTime.MIDDAY:
          LevelLighting.time = LevelLighting.bias / 2f;
          break;
        case ELightingTime.DUSK:
          LevelLighting.time = LevelLighting.bias;
          break;
        case ELightingTime.MIDNIGHT:
          LevelLighting.time = (float) (1.0 - (1.0 - (double) LevelLighting.bias) / 2.0);
          break;
      }
      LevelLighting.updateClouds();
      EditorEnvironmentLightingUI.timeSlider.state = LevelLighting.time;
    }

    private static void onPickedColorPicker(SleekColorPicker picker, Color state)
    {
      int index = 0;
      while (index < EditorEnvironmentLightingUI.colorPickers.Length && EditorEnvironmentLightingUI.colorPickers[index] != picker)
        ++index;
      LevelLighting.times[(int) EditorEnvironmentLightingUI.selectedTime].colors[index] = state;
      LevelLighting.updateLighting();
    }

    private static void onDraggedSingleSlider(SleekSlider slider, float state)
    {
      int index = 0;
      while (index < EditorEnvironmentLightingUI.singleSliders.Length && EditorEnvironmentLightingUI.singleSliders[index] != slider)
        ++index;
      LevelLighting.times[(int) EditorEnvironmentLightingUI.selectedTime].singles[index] = state;
      LevelLighting.updateLighting();
      if (index != 2)
        return;
      LevelLighting.updateClouds();
    }

    private static void updateSelection()
    {
      for (int index = 0; index < EditorEnvironmentLightingUI.colorPickers.Length; ++index)
        EditorEnvironmentLightingUI.colorPickers[index].state = LevelLighting.times[(int) EditorEnvironmentLightingUI.selectedTime].colors[index];
      for (int index = 0; index < EditorEnvironmentLightingUI.singleSliders.Length; ++index)
        EditorEnvironmentLightingUI.singleSliders[index].state = LevelLighting.times[(int) EditorEnvironmentLightingUI.selectedTime].singles[index];
    }
  }
}
