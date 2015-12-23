// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.MenuConfigurationDisplayUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class MenuConfigurationDisplayUI
  {
    private static Local localization;
    private static Sleek container;
    public static bool active;
    private static SleekScrollBox resolutionsBox;
    private static SleekButton[] buttons;
    private static SleekToggle fullscreenToggle;
    private static SleekToggle bufferToggle;

    public MenuConfigurationDisplayUI()
    {
      MenuConfigurationDisplayUI.localization = Localization.read("/Menu/Configuration/MenuConfigurationDisplay.dat");
      MenuConfigurationDisplayUI.container = new Sleek();
      MenuConfigurationDisplayUI.container.positionOffset_X = 10;
      MenuConfigurationDisplayUI.container.positionOffset_Y = 10;
      MenuConfigurationDisplayUI.container.positionScale_Y = 1f;
      MenuConfigurationDisplayUI.container.sizeOffset_X = -20;
      MenuConfigurationDisplayUI.container.sizeOffset_Y = -20;
      MenuConfigurationDisplayUI.container.sizeScale_X = 1f;
      MenuConfigurationDisplayUI.container.sizeScale_Y = 1f;
      if (Provider.isConnected)
        PlayerUI.container.add(MenuConfigurationDisplayUI.container);
      else
        MenuUI.container.add(MenuConfigurationDisplayUI.container);
      MenuConfigurationDisplayUI.active = false;
      MenuConfigurationDisplayUI.resolutionsBox = new SleekScrollBox();
      MenuConfigurationDisplayUI.resolutionsBox.positionOffset_X = -200;
      MenuConfigurationDisplayUI.resolutionsBox.positionOffset_Y = 100;
      MenuConfigurationDisplayUI.resolutionsBox.positionScale_X = 0.5f;
      MenuConfigurationDisplayUI.resolutionsBox.sizeOffset_X = 430;
      MenuConfigurationDisplayUI.resolutionsBox.sizeOffset_Y = -200;
      MenuConfigurationDisplayUI.resolutionsBox.sizeScale_Y = 1f;
      MenuConfigurationDisplayUI.resolutionsBox.area = new Rect(0.0f, 0.0f, 5f, (float) (100 + Screen.resolutions.Length * 40 - 10));
      MenuConfigurationDisplayUI.container.add((Sleek) MenuConfigurationDisplayUI.resolutionsBox);
      MenuConfigurationDisplayUI.buttons = new SleekButton[Screen.resolutions.Length];
      for (byte index = (byte) 0; (int) index < MenuConfigurationDisplayUI.buttons.Length; ++index)
      {
        Resolution resolution = Screen.resolutions[(int) index];
        SleekButton sleekButton = new SleekButton();
        sleekButton.positionOffset_Y = 100 + (int) index * 40;
        sleekButton.sizeOffset_X = -30;
        sleekButton.sizeOffset_Y = 30;
        sleekButton.sizeScale_X = 1f;
        sleekButton.onClickedButton = new ClickedButton(MenuConfigurationDisplayUI.onClickedResolutionButton);
        sleekButton.text = (string) (object) resolution.width + (object) " x " + (string) (object) resolution.height + " [" + (string) (object) resolution.refreshRate + "Hz]";
        MenuConfigurationDisplayUI.resolutionsBox.add((Sleek) sleekButton);
        MenuConfigurationDisplayUI.buttons[(int) index] = sleekButton;
      }
      MenuConfigurationDisplayUI.fullscreenToggle = new SleekToggle();
      MenuConfigurationDisplayUI.fullscreenToggle.sizeOffset_X = 40;
      MenuConfigurationDisplayUI.fullscreenToggle.sizeOffset_Y = 40;
      MenuConfigurationDisplayUI.fullscreenToggle.addLabel(MenuConfigurationDisplayUI.localization.format("Fullscreen_Toggle_Label"), ESleekSide.RIGHT);
      MenuConfigurationDisplayUI.fullscreenToggle.state = GraphicsSettings.fullscreen;
      MenuConfigurationDisplayUI.fullscreenToggle.onToggled = new Toggled(MenuConfigurationDisplayUI.onToggledFullscreenToggle);
      MenuConfigurationDisplayUI.resolutionsBox.add((Sleek) MenuConfigurationDisplayUI.fullscreenToggle);
      MenuConfigurationDisplayUI.bufferToggle = new SleekToggle();
      MenuConfigurationDisplayUI.bufferToggle.positionOffset_Y = 50;
      MenuConfigurationDisplayUI.bufferToggle.sizeOffset_X = 40;
      MenuConfigurationDisplayUI.bufferToggle.sizeOffset_Y = 40;
      MenuConfigurationDisplayUI.bufferToggle.addLabel(MenuConfigurationDisplayUI.localization.format("Buffer_Toggle_Label"), ESleekSide.RIGHT);
      MenuConfigurationDisplayUI.bufferToggle.state = GraphicsSettings.buffer;
      MenuConfigurationDisplayUI.bufferToggle.onToggled = new Toggled(MenuConfigurationDisplayUI.onToggledBufferToggle);
      MenuConfigurationDisplayUI.resolutionsBox.add((Sleek) MenuConfigurationDisplayUI.bufferToggle);
    }

    public static void open()
    {
      if (MenuConfigurationDisplayUI.active)
      {
        MenuConfigurationDisplayUI.close();
      }
      else
      {
        MenuConfigurationDisplayUI.active = true;
        MenuConfigurationDisplayUI.container.lerpPositionScale(0.0f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
      }
    }

    public static void close()
    {
      if (!MenuConfigurationDisplayUI.active)
        return;
      MenuConfigurationDisplayUI.active = false;
      MenuConfigurationDisplayUI.container.lerpPositionScale(0.0f, 1f, ESleekLerp.EXPONENTIAL, 20f);
    }

    private static void onClickedResolutionButton(SleekButton button)
    {
      GraphicsSettings.resolution = new GraphicsSettingsResolution(Screen.resolutions[(button.positionOffset_Y - 100) / 40]);
      GraphicsSettings.apply();
    }

    private static void onToggledFullscreenToggle(SleekToggle toggle, bool state)
    {
      GraphicsSettings.fullscreen = state;
      GraphicsSettings.apply();
    }

    private static void onToggledBufferToggle(SleekToggle toggle, bool state)
    {
      GraphicsSettings.buffer = state;
      GraphicsSettings.apply();
    }
  }
}
