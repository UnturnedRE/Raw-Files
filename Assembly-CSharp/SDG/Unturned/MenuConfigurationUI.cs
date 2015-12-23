// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.MenuConfigurationUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class MenuConfigurationUI
  {
    private static Sleek container;
    public static bool active;
    private static SleekButtonIcon optionsButton;
    private static SleekButtonIcon displayButton;
    private static SleekButtonIcon graphicsButton;
    private static SleekButtonIcon controlsButton;

    public MenuConfigurationUI()
    {
      Local local = Localization.read("/Menu/Configuration/MenuConfiguration.dat");
      Bundle bundle = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Configuration/MenuConfiguration/MenuConfiguration.unity3d");
      MenuConfigurationUI.container = new Sleek();
      MenuConfigurationUI.container.positionOffset_X = 10;
      MenuConfigurationUI.container.positionOffset_Y = 10;
      MenuConfigurationUI.container.positionScale_Y = -1f;
      MenuConfigurationUI.container.sizeOffset_X = -20;
      MenuConfigurationUI.container.sizeOffset_Y = -20;
      MenuConfigurationUI.container.sizeScale_X = 1f;
      MenuConfigurationUI.container.sizeScale_Y = 1f;
      MenuUI.container.add(MenuConfigurationUI.container);
      MenuConfigurationUI.active = false;
      MenuConfigurationUI.optionsButton = new SleekButtonIcon((Texture2D) bundle.load("Options"));
      MenuConfigurationUI.optionsButton.positionOffset_X = -100;
      MenuConfigurationUI.optionsButton.positionOffset_Y = -115;
      MenuConfigurationUI.optionsButton.positionScale_X = 0.5f;
      MenuConfigurationUI.optionsButton.positionScale_Y = 0.5f;
      MenuConfigurationUI.optionsButton.sizeOffset_X = 200;
      MenuConfigurationUI.optionsButton.sizeOffset_Y = 50;
      MenuConfigurationUI.optionsButton.text = local.format("Options_Button_Text");
      MenuConfigurationUI.optionsButton.tooltip = local.format("Options_Button_Tooltip");
      MenuConfigurationUI.optionsButton.onClickedButton = new ClickedButton(MenuConfigurationUI.onClickedOptionsButton);
      MenuConfigurationUI.optionsButton.fontSize = 14;
      MenuConfigurationUI.container.add((Sleek) MenuConfigurationUI.optionsButton);
      MenuConfigurationUI.displayButton = new SleekButtonIcon((Texture2D) bundle.load("Display"));
      MenuConfigurationUI.displayButton.positionOffset_X = -100;
      MenuConfigurationUI.displayButton.positionOffset_Y = -55;
      MenuConfigurationUI.displayButton.positionScale_X = 0.5f;
      MenuConfigurationUI.displayButton.positionScale_Y = 0.5f;
      MenuConfigurationUI.displayButton.sizeOffset_X = 200;
      MenuConfigurationUI.displayButton.sizeOffset_Y = 50;
      MenuConfigurationUI.displayButton.text = local.format("Display_Button_Text");
      MenuConfigurationUI.displayButton.tooltip = local.format("Display_Button_Tooltip");
      MenuConfigurationUI.displayButton.onClickedButton = new ClickedButton(MenuConfigurationUI.onClickedDisplayButton);
      MenuConfigurationUI.displayButton.fontSize = 14;
      MenuConfigurationUI.container.add((Sleek) MenuConfigurationUI.displayButton);
      MenuConfigurationUI.graphicsButton = new SleekButtonIcon((Texture2D) bundle.load("Graphics"));
      MenuConfigurationUI.graphicsButton.positionOffset_X = -100;
      MenuConfigurationUI.graphicsButton.positionOffset_Y = 5;
      MenuConfigurationUI.graphicsButton.positionScale_X = 0.5f;
      MenuConfigurationUI.graphicsButton.positionScale_Y = 0.5f;
      MenuConfigurationUI.graphicsButton.sizeOffset_X = 200;
      MenuConfigurationUI.graphicsButton.sizeOffset_Y = 50;
      MenuConfigurationUI.graphicsButton.text = local.format("Graphics_Button_Text");
      MenuConfigurationUI.graphicsButton.tooltip = local.format("Graphics_Button_Tooltip");
      MenuConfigurationUI.graphicsButton.onClickedButton = new ClickedButton(MenuConfigurationUI.onClickedGraphicsButton);
      MenuConfigurationUI.graphicsButton.fontSize = 14;
      MenuConfigurationUI.container.add((Sleek) MenuConfigurationUI.graphicsButton);
      MenuConfigurationUI.controlsButton = new SleekButtonIcon((Texture2D) bundle.load("Controls"));
      MenuConfigurationUI.controlsButton.positionOffset_X = -100;
      MenuConfigurationUI.controlsButton.positionOffset_Y = 65;
      MenuConfigurationUI.controlsButton.positionScale_X = 0.5f;
      MenuConfigurationUI.controlsButton.positionScale_Y = 0.5f;
      MenuConfigurationUI.controlsButton.sizeOffset_X = 200;
      MenuConfigurationUI.controlsButton.sizeOffset_Y = 50;
      MenuConfigurationUI.controlsButton.text = local.format("Controls_Button_Text");
      MenuConfigurationUI.controlsButton.tooltip = local.format("Controls_Button_Tooltip");
      MenuConfigurationUI.controlsButton.onClickedButton = new ClickedButton(MenuConfigurationUI.onClickedControlsButton);
      MenuConfigurationUI.controlsButton.fontSize = 14;
      MenuConfigurationUI.container.add((Sleek) MenuConfigurationUI.controlsButton);
      bundle.unload();
      MenuConfigurationOptionsUI configurationOptionsUi = new MenuConfigurationOptionsUI();
      MenuConfigurationDisplayUI configurationDisplayUi = new MenuConfigurationDisplayUI();
      MenuConfigurationGraphicsUI configurationGraphicsUi = new MenuConfigurationGraphicsUI();
      MenuConfigurationControlsUI configurationControlsUi = new MenuConfigurationControlsUI();
    }

    public static void open()
    {
      if (MenuConfigurationUI.active)
      {
        MenuConfigurationUI.close();
      }
      else
      {
        MenuConfigurationUI.active = true;
        MenuConfigurationUI.container.lerpPositionScale(0.0f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
      }
    }

    public static void close()
    {
      if (!MenuConfigurationUI.active)
        return;
      MenuConfigurationUI.active = false;
      MenuSettings.save();
      MenuConfigurationUI.container.lerpPositionScale(0.0f, -1f, ESleekLerp.EXPONENTIAL, 20f);
    }

    private static void onClickedOptionsButton(SleekButton button)
    {
      MenuConfigurationOptionsUI.open();
      MenuConfigurationUI.close();
    }

    private static void onClickedDisplayButton(SleekButton button)
    {
      MenuConfigurationDisplayUI.open();
      MenuConfigurationUI.close();
    }

    private static void onClickedGraphicsButton(SleekButton button)
    {
      MenuConfigurationGraphicsUI.open();
      MenuConfigurationUI.close();
    }

    private static void onClickedControlsButton(SleekButton button)
    {
      MenuConfigurationControlsUI.open();
      MenuConfigurationUI.close();
    }
  }
}
