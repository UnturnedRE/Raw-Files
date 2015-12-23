// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.MenuConfigurationOptionsUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class MenuConfigurationOptionsUI
  {
    private static Local localization;
    private static Sleek container;
    public static bool active;
    private static SleekScrollBox optionsBox;
    private static SleekSlider fovSlider;
    private static SleekSlider volumeSlider;
    private static SleekToggle debugToggle;
    private static SleekToggle musicToggle;
    private static SleekToggle physicsToggle;
    private static SleekToggle goreToggle;
    private static SleekToggle filterToggle;
    private static SleekToggle chatTextToggle;
    private static SleekToggle chatVoiceToggle;
    private static SleekToggle hintsToggle;
    private static SleekButtonState metricButton;
    private static SleekBox crosshairBox;
    private static SleekColorPicker crosshairColorPicker;
    private static SleekBox hitmarkerBox;
    private static SleekColorPicker hitmarkerColorPicker;
    private static SleekBox criticalHitmarkerBox;
    private static SleekColorPicker criticalHitmarkerColorPicker;
    private static SleekBox cursorBox;
    private static SleekColorPicker cursorColorPicker;

    public MenuConfigurationOptionsUI()
    {
      MenuConfigurationOptionsUI.localization = Localization.read("/Menu/Configuration/MenuConfigurationOptions.dat");
      MenuConfigurationOptionsUI.container = new Sleek();
      MenuConfigurationOptionsUI.container.positionOffset_X = 10;
      MenuConfigurationOptionsUI.container.positionOffset_Y = 10;
      MenuConfigurationOptionsUI.container.positionScale_Y = 1f;
      MenuConfigurationOptionsUI.container.sizeOffset_X = -20;
      MenuConfigurationOptionsUI.container.sizeOffset_Y = -20;
      MenuConfigurationOptionsUI.container.sizeScale_X = 1f;
      MenuConfigurationOptionsUI.container.sizeScale_Y = 1f;
      if (Provider.isConnected)
        PlayerUI.container.add(MenuConfigurationOptionsUI.container);
      else
        MenuUI.container.add(MenuConfigurationOptionsUI.container);
      MenuConfigurationOptionsUI.active = false;
      MenuConfigurationOptionsUI.optionsBox = new SleekScrollBox();
      MenuConfigurationOptionsUI.optionsBox.positionOffset_X = -200;
      MenuConfigurationOptionsUI.optionsBox.positionOffset_Y = 100;
      MenuConfigurationOptionsUI.optionsBox.positionScale_X = 0.5f;
      MenuConfigurationOptionsUI.optionsBox.sizeOffset_X = 430;
      MenuConfigurationOptionsUI.optionsBox.sizeOffset_Y = -200;
      MenuConfigurationOptionsUI.optionsBox.sizeScale_Y = 1f;
      MenuConfigurationOptionsUI.optionsBox.area = new Rect(0.0f, 0.0f, 5f, 1170f);
      MenuConfigurationOptionsUI.container.add((Sleek) MenuConfigurationOptionsUI.optionsBox);
      MenuConfigurationOptionsUI.fovSlider = new SleekSlider();
      MenuConfigurationOptionsUI.fovSlider.positionOffset_Y = 400;
      MenuConfigurationOptionsUI.fovSlider.sizeOffset_X = 200;
      MenuConfigurationOptionsUI.fovSlider.sizeOffset_Y = 20;
      MenuConfigurationOptionsUI.fovSlider.orientation = ESleekOrientation.HORIZONTAL;
      MenuConfigurationOptionsUI.fovSlider.addLabel(MenuConfigurationOptionsUI.localization.format("FOV_Slider_Label", (object) (int) OptionsSettings.view), ESleekSide.RIGHT);
      MenuConfigurationOptionsUI.fovSlider.state = OptionsSettings.fov;
      MenuConfigurationOptionsUI.fovSlider.onDragged = new Dragged(MenuConfigurationOptionsUI.onDraggedFOVSlider);
      MenuConfigurationOptionsUI.optionsBox.add((Sleek) MenuConfigurationOptionsUI.fovSlider);
      MenuConfigurationOptionsUI.volumeSlider = new SleekSlider();
      MenuConfigurationOptionsUI.volumeSlider.positionOffset_Y = 430;
      MenuConfigurationOptionsUI.volumeSlider.sizeOffset_X = 200;
      MenuConfigurationOptionsUI.volumeSlider.sizeOffset_Y = 20;
      MenuConfigurationOptionsUI.volumeSlider.orientation = ESleekOrientation.HORIZONTAL;
      MenuConfigurationOptionsUI.volumeSlider.addLabel(MenuConfigurationOptionsUI.localization.format("Volume_Slider_Label", (object) (int) ((double) OptionsSettings.volume * 100.0)), ESleekSide.RIGHT);
      MenuConfigurationOptionsUI.volumeSlider.state = OptionsSettings.volume;
      MenuConfigurationOptionsUI.volumeSlider.onDragged = new Dragged(MenuConfigurationOptionsUI.onDraggedVolumeSlider);
      MenuConfigurationOptionsUI.optionsBox.add((Sleek) MenuConfigurationOptionsUI.volumeSlider);
      MenuConfigurationOptionsUI.debugToggle = new SleekToggle();
      MenuConfigurationOptionsUI.debugToggle.sizeOffset_X = 40;
      MenuConfigurationOptionsUI.debugToggle.sizeOffset_Y = 40;
      MenuConfigurationOptionsUI.debugToggle.addLabel(MenuConfigurationOptionsUI.localization.format("Debug_Toggle_Label"), ESleekSide.RIGHT);
      MenuConfigurationOptionsUI.debugToggle.state = OptionsSettings.debug;
      MenuConfigurationOptionsUI.debugToggle.onToggled = new Toggled(MenuConfigurationOptionsUI.onToggledDebugToggle);
      MenuConfigurationOptionsUI.optionsBox.add((Sleek) MenuConfigurationOptionsUI.debugToggle);
      MenuConfigurationOptionsUI.musicToggle = new SleekToggle();
      MenuConfigurationOptionsUI.musicToggle.positionOffset_Y = 50;
      MenuConfigurationOptionsUI.musicToggle.sizeOffset_X = 40;
      MenuConfigurationOptionsUI.musicToggle.sizeOffset_Y = 40;
      MenuConfigurationOptionsUI.musicToggle.addLabel(MenuConfigurationOptionsUI.localization.format("Music_Toggle_Label"), ESleekSide.RIGHT);
      MenuConfigurationOptionsUI.musicToggle.state = OptionsSettings.music;
      MenuConfigurationOptionsUI.musicToggle.onToggled = new Toggled(MenuConfigurationOptionsUI.onToggledMusicToggle);
      MenuConfigurationOptionsUI.optionsBox.add((Sleek) MenuConfigurationOptionsUI.musicToggle);
      MenuConfigurationOptionsUI.physicsToggle = new SleekToggle();
      MenuConfigurationOptionsUI.physicsToggle.positionOffset_Y = 100;
      MenuConfigurationOptionsUI.physicsToggle.sizeOffset_X = 40;
      MenuConfigurationOptionsUI.physicsToggle.sizeOffset_Y = 40;
      MenuConfigurationOptionsUI.physicsToggle.addLabel(MenuConfigurationOptionsUI.localization.format("Physics_Toggle_Label"), ESleekSide.RIGHT);
      MenuConfigurationOptionsUI.physicsToggle.state = OptionsSettings.physics;
      MenuConfigurationOptionsUI.physicsToggle.onToggled = new Toggled(MenuConfigurationOptionsUI.onToggledPhysicsToggle);
      MenuConfigurationOptionsUI.optionsBox.add((Sleek) MenuConfigurationOptionsUI.physicsToggle);
      MenuConfigurationOptionsUI.goreToggle = new SleekToggle();
      MenuConfigurationOptionsUI.goreToggle.positionOffset_Y = 150;
      MenuConfigurationOptionsUI.goreToggle.sizeOffset_X = 40;
      MenuConfigurationOptionsUI.goreToggle.sizeOffset_Y = 40;
      MenuConfigurationOptionsUI.goreToggle.addLabel(MenuConfigurationOptionsUI.localization.format("Gore_Toggle_Label"), ESleekSide.RIGHT);
      MenuConfigurationOptionsUI.goreToggle.state = OptionsSettings.gore;
      MenuConfigurationOptionsUI.goreToggle.onToggled = new Toggled(MenuConfigurationOptionsUI.onToggledGoreToggle);
      MenuConfigurationOptionsUI.optionsBox.add((Sleek) MenuConfigurationOptionsUI.goreToggle);
      MenuConfigurationOptionsUI.filterToggle = new SleekToggle();
      MenuConfigurationOptionsUI.filterToggle.positionOffset_Y = 200;
      MenuConfigurationOptionsUI.filterToggle.sizeOffset_X = 40;
      MenuConfigurationOptionsUI.filterToggle.sizeOffset_Y = 40;
      MenuConfigurationOptionsUI.filterToggle.addLabel(MenuConfigurationOptionsUI.localization.format("Filter_Toggle_Label"), ESleekSide.RIGHT);
      MenuConfigurationOptionsUI.filterToggle.state = OptionsSettings.filter;
      MenuConfigurationOptionsUI.filterToggle.onToggled = new Toggled(MenuConfigurationOptionsUI.onToggledFilterToggle);
      MenuConfigurationOptionsUI.optionsBox.add((Sleek) MenuConfigurationOptionsUI.filterToggle);
      MenuConfigurationOptionsUI.chatTextToggle = new SleekToggle();
      MenuConfigurationOptionsUI.chatTextToggle.positionOffset_Y = 250;
      MenuConfigurationOptionsUI.chatTextToggle.sizeOffset_X = 40;
      MenuConfigurationOptionsUI.chatTextToggle.sizeOffset_Y = 40;
      MenuConfigurationOptionsUI.chatTextToggle.addLabel(MenuConfigurationOptionsUI.localization.format("Chat_Text_Toggle_Label"), ESleekSide.RIGHT);
      MenuConfigurationOptionsUI.chatTextToggle.state = OptionsSettings.chatText;
      MenuConfigurationOptionsUI.chatTextToggle.onToggled = new Toggled(MenuConfigurationOptionsUI.onToggledChatTextToggle);
      MenuConfigurationOptionsUI.optionsBox.add((Sleek) MenuConfigurationOptionsUI.chatTextToggle);
      MenuConfigurationOptionsUI.chatVoiceToggle = new SleekToggle();
      MenuConfigurationOptionsUI.chatVoiceToggle.positionOffset_Y = 300;
      MenuConfigurationOptionsUI.chatVoiceToggle.sizeOffset_X = 40;
      MenuConfigurationOptionsUI.chatVoiceToggle.sizeOffset_Y = 40;
      MenuConfigurationOptionsUI.chatVoiceToggle.addLabel(MenuConfigurationOptionsUI.localization.format("Chat_Voice_Toggle_Label"), ESleekSide.RIGHT);
      MenuConfigurationOptionsUI.chatVoiceToggle.state = OptionsSettings.chatVoice;
      MenuConfigurationOptionsUI.chatVoiceToggle.onToggled = new Toggled(MenuConfigurationOptionsUI.onToggledChatVoiceToggle);
      MenuConfigurationOptionsUI.optionsBox.add((Sleek) MenuConfigurationOptionsUI.chatVoiceToggle);
      MenuConfigurationOptionsUI.hintsToggle = new SleekToggle();
      MenuConfigurationOptionsUI.hintsToggle.positionOffset_Y = 350;
      MenuConfigurationOptionsUI.hintsToggle.sizeOffset_X = 40;
      MenuConfigurationOptionsUI.hintsToggle.sizeOffset_Y = 40;
      MenuConfigurationOptionsUI.hintsToggle.addLabel(MenuConfigurationOptionsUI.localization.format("Hints_Toggle_Label"), ESleekSide.RIGHT);
      MenuConfigurationOptionsUI.hintsToggle.state = OptionsSettings.hints;
      MenuConfigurationOptionsUI.hintsToggle.onToggled = new Toggled(MenuConfigurationOptionsUI.onToggledHintsToggle);
      MenuConfigurationOptionsUI.optionsBox.add((Sleek) MenuConfigurationOptionsUI.hintsToggle);
      MenuConfigurationOptionsUI.metricButton = new SleekButtonState(new GUIContent[2]
      {
        new GUIContent(MenuConfigurationOptionsUI.localization.format("Metric_Off")),
        new GUIContent(MenuConfigurationOptionsUI.localization.format("Metric_On"))
      });
      MenuConfigurationOptionsUI.metricButton.positionOffset_Y = 460;
      MenuConfigurationOptionsUI.metricButton.sizeOffset_X = 200;
      MenuConfigurationOptionsUI.metricButton.sizeOffset_Y = 30;
      MenuConfigurationOptionsUI.metricButton.state = !OptionsSettings.metric ? 0 : 1;
      MenuConfigurationOptionsUI.metricButton.tooltip = MenuConfigurationOptionsUI.localization.format("Metric_Tooltip");
      MenuConfigurationOptionsUI.metricButton.onSwappedState = new SwappedState(MenuConfigurationOptionsUI.onSwappedMetricState);
      MenuConfigurationOptionsUI.optionsBox.add((Sleek) MenuConfigurationOptionsUI.metricButton);
      MenuConfigurationOptionsUI.crosshairBox = new SleekBox();
      MenuConfigurationOptionsUI.crosshairBox.positionOffset_Y = 500;
      MenuConfigurationOptionsUI.crosshairBox.sizeOffset_X = 240;
      MenuConfigurationOptionsUI.crosshairBox.sizeOffset_Y = 30;
      MenuConfigurationOptionsUI.crosshairBox.text = MenuConfigurationOptionsUI.localization.format("Crosshair_Box");
      MenuConfigurationOptionsUI.optionsBox.add((Sleek) MenuConfigurationOptionsUI.crosshairBox);
      MenuConfigurationOptionsUI.crosshairColorPicker = new SleekColorPicker();
      MenuConfigurationOptionsUI.crosshairColorPicker.positionOffset_Y = 540;
      MenuConfigurationOptionsUI.crosshairColorPicker.state = OptionsSettings.crosshairColor;
      MenuConfigurationOptionsUI.crosshairColorPicker.onColorPicked = new ColorPicked(MenuConfigurationOptionsUI.onCrosshairColorPicked);
      MenuConfigurationOptionsUI.optionsBox.add((Sleek) MenuConfigurationOptionsUI.crosshairColorPicker);
      MenuConfigurationOptionsUI.hitmarkerBox = new SleekBox();
      MenuConfigurationOptionsUI.hitmarkerBox.positionOffset_Y = 670;
      MenuConfigurationOptionsUI.hitmarkerBox.sizeOffset_X = 240;
      MenuConfigurationOptionsUI.hitmarkerBox.sizeOffset_Y = 30;
      MenuConfigurationOptionsUI.hitmarkerBox.text = MenuConfigurationOptionsUI.localization.format("Hitmarker_Box");
      MenuConfigurationOptionsUI.optionsBox.add((Sleek) MenuConfigurationOptionsUI.hitmarkerBox);
      MenuConfigurationOptionsUI.hitmarkerColorPicker = new SleekColorPicker();
      MenuConfigurationOptionsUI.hitmarkerColorPicker.positionOffset_Y = 710;
      MenuConfigurationOptionsUI.hitmarkerColorPicker.state = OptionsSettings.hitmarkerColor;
      MenuConfigurationOptionsUI.hitmarkerColorPicker.onColorPicked = new ColorPicked(MenuConfigurationOptionsUI.onHitmarkerColorPicked);
      MenuConfigurationOptionsUI.optionsBox.add((Sleek) MenuConfigurationOptionsUI.hitmarkerColorPicker);
      MenuConfigurationOptionsUI.criticalHitmarkerBox = new SleekBox();
      MenuConfigurationOptionsUI.criticalHitmarkerBox.positionOffset_Y = 840;
      MenuConfigurationOptionsUI.criticalHitmarkerBox.sizeOffset_X = 240;
      MenuConfigurationOptionsUI.criticalHitmarkerBox.sizeOffset_Y = 30;
      MenuConfigurationOptionsUI.criticalHitmarkerBox.text = MenuConfigurationOptionsUI.localization.format("Critical_Hitmarker_Box");
      MenuConfigurationOptionsUI.optionsBox.add((Sleek) MenuConfigurationOptionsUI.criticalHitmarkerBox);
      MenuConfigurationOptionsUI.criticalHitmarkerColorPicker = new SleekColorPicker();
      MenuConfigurationOptionsUI.criticalHitmarkerColorPicker.positionOffset_Y = 880;
      MenuConfigurationOptionsUI.criticalHitmarkerColorPicker.state = OptionsSettings.criticalHitmarkerColor;
      MenuConfigurationOptionsUI.criticalHitmarkerColorPicker.onColorPicked = new ColorPicked(MenuConfigurationOptionsUI.onCriticalHitmarkerColorPicked);
      MenuConfigurationOptionsUI.optionsBox.add((Sleek) MenuConfigurationOptionsUI.criticalHitmarkerColorPicker);
      MenuConfigurationOptionsUI.cursorBox = new SleekBox();
      MenuConfigurationOptionsUI.cursorBox.positionOffset_Y = 1010;
      MenuConfigurationOptionsUI.cursorBox.sizeOffset_X = 240;
      MenuConfigurationOptionsUI.cursorBox.sizeOffset_Y = 30;
      MenuConfigurationOptionsUI.cursorBox.text = MenuConfigurationOptionsUI.localization.format("Cursor_Box");
      MenuConfigurationOptionsUI.optionsBox.add((Sleek) MenuConfigurationOptionsUI.cursorBox);
      MenuConfigurationOptionsUI.cursorColorPicker = new SleekColorPicker();
      MenuConfigurationOptionsUI.cursorColorPicker.positionOffset_Y = 1050;
      MenuConfigurationOptionsUI.cursorColorPicker.state = OptionsSettings.cursorColor;
      MenuConfigurationOptionsUI.cursorColorPicker.onColorPicked = new ColorPicked(MenuConfigurationOptionsUI.onCursorColorPicked);
      MenuConfigurationOptionsUI.optionsBox.add((Sleek) MenuConfigurationOptionsUI.cursorColorPicker);
    }

    public static void open()
    {
      if (MenuConfigurationOptionsUI.active)
      {
        MenuConfigurationOptionsUI.close();
      }
      else
      {
        MenuConfigurationOptionsUI.active = true;
        MenuConfigurationOptionsUI.container.lerpPositionScale(0.0f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
      }
    }

    public static void close()
    {
      if (!MenuConfigurationOptionsUI.active)
        return;
      MenuConfigurationOptionsUI.active = false;
      MenuConfigurationOptionsUI.container.lerpPositionScale(0.0f, 1f, ESleekLerp.EXPONENTIAL, 20f);
    }

    private static void onDraggedFOVSlider(SleekSlider slider, float state)
    {
      OptionsSettings.fov = state;
      OptionsSettings.apply();
      MenuConfigurationOptionsUI.fovSlider.updateLabel(MenuConfigurationOptionsUI.localization.format("FOV_Slider_Label", (object) (int) OptionsSettings.view));
    }

    private static void onDraggedVolumeSlider(SleekSlider slider, float state)
    {
      OptionsSettings.volume = state;
      OptionsSettings.apply();
      MenuConfigurationOptionsUI.volumeSlider.updateLabel(MenuConfigurationOptionsUI.localization.format("Volume_Slider_Label", (object) (int) ((double) OptionsSettings.volume * 100.0)));
    }

    private static void onToggledDebugToggle(SleekToggle toggle, bool state)
    {
      OptionsSettings.debug = state;
    }

    private static void onToggledMusicToggle(SleekToggle toggle, bool state)
    {
      OptionsSettings.music = state;
      OptionsSettings.apply();
    }

    private static void onToggledPhysicsToggle(SleekToggle toggle, bool state)
    {
      OptionsSettings.physics = state;
    }

    private static void onToggledGoreToggle(SleekToggle toggle, bool state)
    {
      OptionsSettings.gore = state;
    }

    private static void onToggledFilterToggle(SleekToggle toggle, bool state)
    {
      OptionsSettings.filter = state;
    }

    private static void onToggledChatTextToggle(SleekToggle toggle, bool state)
    {
      OptionsSettings.chatText = state;
    }

    private static void onToggledChatVoiceToggle(SleekToggle toggle, bool state)
    {
      OptionsSettings.chatVoice = state;
    }

    private static void onToggledHintsToggle(SleekToggle toggle, bool state)
    {
      OptionsSettings.hints = state;
    }

    private static void onSwappedMetricState(SleekButtonState button, int index)
    {
      OptionsSettings.metric = index == 1;
    }

    private static void onCrosshairColorPicked(SleekColorPicker picker, Color color)
    {
      OptionsSettings.crosshairColor = color;
      if (PlayerLifeUI.dotImage == null)
        return;
      PlayerLifeUI.crosshairLeftImage.backgroundColor = color;
      PlayerLifeUI.crosshairRightImage.backgroundColor = color;
      PlayerLifeUI.crosshairDownImage.backgroundColor = color;
      PlayerLifeUI.crosshairUpImage.backgroundColor = color;
      PlayerLifeUI.dotImage.backgroundColor = color;
    }

    private static void onHitmarkerColorPicked(SleekColorPicker picker, Color color)
    {
      OptionsSettings.hitmarkerColor = color;
      if (PlayerLifeUI.hitBuildImage == null)
        return;
      PlayerLifeUI.hitEntitiyImage.backgroundColor = color;
      PlayerLifeUI.hitBuildImage.backgroundColor = color;
    }

    private static void onCriticalHitmarkerColorPicked(SleekColorPicker picker, Color color)
    {
      OptionsSettings.criticalHitmarkerColor = color;
      if (PlayerLifeUI.hitCriticalImage == null)
        return;
      PlayerLifeUI.hitCriticalImage.backgroundColor = color;
    }

    private static void onCursorColorPicked(SleekColorPicker picker, Color color)
    {
      OptionsSettings.cursorColor = color;
    }
  }
}
