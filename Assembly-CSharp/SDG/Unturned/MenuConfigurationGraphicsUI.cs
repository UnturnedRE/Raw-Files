// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.MenuConfigurationGraphicsUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class MenuConfigurationGraphicsUI
  {
    private static Local localization;
    private static Sleek container;
    public static bool active;
    private static SleekScrollBox graphicsBox;
    private static SleekToggle motionBlurToggle;
    private static SleekToggle ambientOcclusionToggle;
    private static SleekToggle sunShaftsToggle;
    private static SleekToggle bloomToggle;
    private static SleekToggle cloudsToggle;
    private static SleekToggle terrainToggle;
    private static SleekToggle fogToggle;
    private static SleekSlider distanceSlider;
    public static SleekButtonState antiAliasingButton;
    private static SleekButtonState effectButton;
    private static SleekButtonState foliageButton;
    private static SleekButtonState lightingButton;
    private static SleekButtonState waterButton;
    private static SleekButtonState scopeButton;
    private static SleekButtonState outlineButton;

    public MenuConfigurationGraphicsUI()
    {
      MenuConfigurationGraphicsUI.localization = Localization.read("/Menu/Configuration/MenuConfigurationGraphics.dat");
      MenuConfigurationGraphicsUI.container = new Sleek();
      MenuConfigurationGraphicsUI.container.positionOffset_X = 10;
      MenuConfigurationGraphicsUI.container.positionOffset_Y = 10;
      MenuConfigurationGraphicsUI.container.positionScale_Y = 1f;
      MenuConfigurationGraphicsUI.container.sizeOffset_X = -20;
      MenuConfigurationGraphicsUI.container.sizeOffset_Y = -20;
      MenuConfigurationGraphicsUI.container.sizeScale_X = 1f;
      MenuConfigurationGraphicsUI.container.sizeScale_Y = 1f;
      if (Provider.isConnected)
        PlayerUI.container.add(MenuConfigurationGraphicsUI.container);
      else
        MenuUI.container.add(MenuConfigurationGraphicsUI.container);
      MenuConfigurationGraphicsUI.active = false;
      MenuConfigurationGraphicsUI.graphicsBox = new SleekScrollBox();
      MenuConfigurationGraphicsUI.graphicsBox.positionOffset_X = -200;
      MenuConfigurationGraphicsUI.graphicsBox.positionOffset_Y = 100;
      MenuConfigurationGraphicsUI.graphicsBox.positionScale_X = 0.5f;
      MenuConfigurationGraphicsUI.graphicsBox.sizeOffset_X = 430;
      MenuConfigurationGraphicsUI.graphicsBox.sizeOffset_Y = -200;
      MenuConfigurationGraphicsUI.graphicsBox.sizeScale_Y = 1f;
      MenuConfigurationGraphicsUI.graphicsBox.area = new Rect(0.0f, 0.0f, 5f, 650f);
      MenuConfigurationGraphicsUI.container.add((Sleek) MenuConfigurationGraphicsUI.graphicsBox);
      MenuConfigurationGraphicsUI.motionBlurToggle = new SleekToggle();
      MenuConfigurationGraphicsUI.motionBlurToggle.sizeOffset_X = 40;
      MenuConfigurationGraphicsUI.motionBlurToggle.sizeOffset_Y = 40;
      MenuConfigurationGraphicsUI.motionBlurToggle.addLabel(MenuConfigurationGraphicsUI.localization.format("Motion_Blur_Toggle_Label"), ESleekSide.RIGHT);
      MenuConfigurationGraphicsUI.motionBlurToggle.state = GraphicsSettings.motionBlur;
      MenuConfigurationGraphicsUI.motionBlurToggle.onToggled = new Toggled(MenuConfigurationGraphicsUI.onToggledMotionBlurToggle);
      MenuConfigurationGraphicsUI.graphicsBox.add((Sleek) MenuConfigurationGraphicsUI.motionBlurToggle);
      MenuConfigurationGraphicsUI.ambientOcclusionToggle = new SleekToggle();
      MenuConfigurationGraphicsUI.ambientOcclusionToggle.positionOffset_Y = 50;
      MenuConfigurationGraphicsUI.ambientOcclusionToggle.sizeOffset_X = 40;
      MenuConfigurationGraphicsUI.ambientOcclusionToggle.sizeOffset_Y = 40;
      MenuConfigurationGraphicsUI.ambientOcclusionToggle.addLabel(MenuConfigurationGraphicsUI.localization.format("Ambient_Occlusion_Toggle_Label"), ESleekSide.RIGHT);
      MenuConfigurationGraphicsUI.ambientOcclusionToggle.state = GraphicsSettings.ambientOcclusion;
      MenuConfigurationGraphicsUI.ambientOcclusionToggle.onToggled = new Toggled(MenuConfigurationGraphicsUI.onToggledAmbientOcclusionToggle);
      MenuConfigurationGraphicsUI.graphicsBox.add((Sleek) MenuConfigurationGraphicsUI.ambientOcclusionToggle);
      MenuConfigurationGraphicsUI.sunShaftsToggle = new SleekToggle();
      MenuConfigurationGraphicsUI.sunShaftsToggle.positionOffset_Y = 100;
      MenuConfigurationGraphicsUI.sunShaftsToggle.sizeOffset_X = 40;
      MenuConfigurationGraphicsUI.sunShaftsToggle.sizeOffset_Y = 40;
      MenuConfigurationGraphicsUI.sunShaftsToggle.addLabel(MenuConfigurationGraphicsUI.localization.format("Sun_Shafts_Toggle_Label"), ESleekSide.RIGHT);
      MenuConfigurationGraphicsUI.sunShaftsToggle.state = GraphicsSettings.sunShafts;
      MenuConfigurationGraphicsUI.sunShaftsToggle.onToggled = new Toggled(MenuConfigurationGraphicsUI.onToggledSunShaftsToggle);
      MenuConfigurationGraphicsUI.graphicsBox.add((Sleek) MenuConfigurationGraphicsUI.sunShaftsToggle);
      MenuConfigurationGraphicsUI.bloomToggle = new SleekToggle();
      MenuConfigurationGraphicsUI.bloomToggle.positionOffset_Y = 150;
      MenuConfigurationGraphicsUI.bloomToggle.sizeOffset_X = 40;
      MenuConfigurationGraphicsUI.bloomToggle.sizeOffset_Y = 40;
      MenuConfigurationGraphicsUI.bloomToggle.addLabel(MenuConfigurationGraphicsUI.localization.format("Bloom_Toggle_Label"), ESleekSide.RIGHT);
      MenuConfigurationGraphicsUI.bloomToggle.state = GraphicsSettings.bloom;
      MenuConfigurationGraphicsUI.bloomToggle.onToggled = new Toggled(MenuConfigurationGraphicsUI.onToggledBloomToggle);
      MenuConfigurationGraphicsUI.graphicsBox.add((Sleek) MenuConfigurationGraphicsUI.bloomToggle);
      MenuConfigurationGraphicsUI.cloudsToggle = new SleekToggle();
      MenuConfigurationGraphicsUI.cloudsToggle.positionOffset_Y = 200;
      MenuConfigurationGraphicsUI.cloudsToggle.sizeOffset_X = 40;
      MenuConfigurationGraphicsUI.cloudsToggle.sizeOffset_Y = 40;
      MenuConfigurationGraphicsUI.cloudsToggle.addLabel(MenuConfigurationGraphicsUI.localization.format("Clouds_Toggle_Label"), ESleekSide.RIGHT);
      MenuConfigurationGraphicsUI.cloudsToggle.state = GraphicsSettings.clouds;
      MenuConfigurationGraphicsUI.cloudsToggle.onToggled = new Toggled(MenuConfigurationGraphicsUI.onToggledCloudsToggle);
      MenuConfigurationGraphicsUI.graphicsBox.add((Sleek) MenuConfigurationGraphicsUI.cloudsToggle);
      MenuConfigurationGraphicsUI.terrainToggle = new SleekToggle();
      MenuConfigurationGraphicsUI.terrainToggle.positionOffset_Y = 250;
      MenuConfigurationGraphicsUI.terrainToggle.sizeOffset_X = 40;
      MenuConfigurationGraphicsUI.terrainToggle.sizeOffset_Y = 40;
      MenuConfigurationGraphicsUI.terrainToggle.addLabel(MenuConfigurationGraphicsUI.localization.format("Terrain_Toggle_Label"), ESleekSide.RIGHT);
      MenuConfigurationGraphicsUI.terrainToggle.state = GraphicsSettings.terrain;
      MenuConfigurationGraphicsUI.terrainToggle.onToggled = new Toggled(MenuConfigurationGraphicsUI.onToggledTerrainToggle);
      MenuConfigurationGraphicsUI.graphicsBox.add((Sleek) MenuConfigurationGraphicsUI.terrainToggle);
      MenuConfigurationGraphicsUI.fogToggle = new SleekToggle();
      MenuConfigurationGraphicsUI.fogToggle.positionOffset_Y = 300;
      MenuConfigurationGraphicsUI.fogToggle.sizeOffset_X = 40;
      MenuConfigurationGraphicsUI.fogToggle.sizeOffset_Y = 40;
      MenuConfigurationGraphicsUI.fogToggle.addLabel(MenuConfigurationGraphicsUI.localization.format("Fog_Toggle_Label"), ESleekSide.RIGHT);
      MenuConfigurationGraphicsUI.fogToggle.state = GraphicsSettings.fog;
      MenuConfigurationGraphicsUI.fogToggle.onToggled = new Toggled(MenuConfigurationGraphicsUI.onToggledFogToggle);
      MenuConfigurationGraphicsUI.graphicsBox.add((Sleek) MenuConfigurationGraphicsUI.fogToggle);
      MenuConfigurationGraphicsUI.distanceSlider = new SleekSlider();
      MenuConfigurationGraphicsUI.distanceSlider.positionOffset_Y = 350;
      MenuConfigurationGraphicsUI.distanceSlider.sizeOffset_X = 200;
      MenuConfigurationGraphicsUI.distanceSlider.sizeOffset_Y = 20;
      MenuConfigurationGraphicsUI.distanceSlider.orientation = ESleekOrientation.HORIZONTAL;
      MenuConfigurationGraphicsUI.distanceSlider.addLabel(MenuConfigurationGraphicsUI.localization.format("Distance_Slider_Label", (object) (25 + (int) ((double) GraphicsSettings.distance * 75.0))), ESleekSide.RIGHT);
      MenuConfigurationGraphicsUI.distanceSlider.state = GraphicsSettings.distance;
      MenuConfigurationGraphicsUI.distanceSlider.onDragged = new Dragged(MenuConfigurationGraphicsUI.onDraggedDistanceSlider);
      MenuConfigurationGraphicsUI.graphicsBox.add((Sleek) MenuConfigurationGraphicsUI.distanceSlider);
      MenuConfigurationGraphicsUI.antiAliasingButton = new SleekButtonState(new GUIContent[2]
      {
        new GUIContent(MenuConfigurationGraphicsUI.localization.format("Off")),
        new GUIContent(MenuConfigurationGraphicsUI.localization.format("FXAA"))
      });
      MenuConfigurationGraphicsUI.antiAliasingButton.positionOffset_Y = 380;
      MenuConfigurationGraphicsUI.antiAliasingButton.sizeOffset_X = 200;
      MenuConfigurationGraphicsUI.antiAliasingButton.sizeOffset_Y = 30;
      MenuConfigurationGraphicsUI.antiAliasingButton.state = (int) GraphicsSettings.antiAliasingType;
      MenuConfigurationGraphicsUI.antiAliasingButton.addLabel(MenuConfigurationGraphicsUI.localization.format("Anti_Aliasing_Button_Label"), ESleekSide.RIGHT);
      MenuConfigurationGraphicsUI.antiAliasingButton.onSwappedState = new SwappedState(MenuConfigurationGraphicsUI.onSwappedAntiAliasingState);
      MenuConfigurationGraphicsUI.graphicsBox.add((Sleek) MenuConfigurationGraphicsUI.antiAliasingButton);
      MenuConfigurationGraphicsUI.effectButton = new SleekButtonState(new GUIContent[5]
      {
        new GUIContent(MenuConfigurationGraphicsUI.localization.format("Off")),
        new GUIContent(MenuConfigurationGraphicsUI.localization.format("Low")),
        new GUIContent(MenuConfigurationGraphicsUI.localization.format("Medium")),
        new GUIContent(MenuConfigurationGraphicsUI.localization.format("High")),
        new GUIContent(MenuConfigurationGraphicsUI.localization.format("Ultra"))
      });
      MenuConfigurationGraphicsUI.effectButton.positionOffset_Y = 420;
      MenuConfigurationGraphicsUI.effectButton.sizeOffset_X = 200;
      MenuConfigurationGraphicsUI.effectButton.sizeOffset_Y = 30;
      MenuConfigurationGraphicsUI.effectButton.state = (int) GraphicsSettings.effectQuality;
      MenuConfigurationGraphicsUI.effectButton.addLabel(MenuConfigurationGraphicsUI.localization.format("Effect_Button_Label"), ESleekSide.RIGHT);
      MenuConfigurationGraphicsUI.effectButton.onSwappedState = new SwappedState(MenuConfigurationGraphicsUI.onSwappedEffectState);
      MenuConfigurationGraphicsUI.graphicsBox.add((Sleek) MenuConfigurationGraphicsUI.effectButton);
      MenuConfigurationGraphicsUI.foliageButton = new SleekButtonState(new GUIContent[5]
      {
        new GUIContent(MenuConfigurationGraphicsUI.localization.format("Off")),
        new GUIContent(MenuConfigurationGraphicsUI.localization.format("Low")),
        new GUIContent(MenuConfigurationGraphicsUI.localization.format("Medium")),
        new GUIContent(MenuConfigurationGraphicsUI.localization.format("High")),
        new GUIContent(MenuConfigurationGraphicsUI.localization.format("Ultra"))
      });
      MenuConfigurationGraphicsUI.foliageButton.positionOffset_Y = 460;
      MenuConfigurationGraphicsUI.foliageButton.sizeOffset_X = 200;
      MenuConfigurationGraphicsUI.foliageButton.sizeOffset_Y = 30;
      MenuConfigurationGraphicsUI.foliageButton.state = (int) GraphicsSettings.foliageQuality;
      MenuConfigurationGraphicsUI.foliageButton.addLabel(MenuConfigurationGraphicsUI.localization.format("Foliage_Button_Label"), ESleekSide.RIGHT);
      MenuConfigurationGraphicsUI.foliageButton.onSwappedState = new SwappedState(MenuConfigurationGraphicsUI.onSwappedFoliageState);
      MenuConfigurationGraphicsUI.graphicsBox.add((Sleek) MenuConfigurationGraphicsUI.foliageButton);
      MenuConfigurationGraphicsUI.lightingButton = new SleekButtonState(new GUIContent[5]
      {
        new GUIContent(MenuConfigurationGraphicsUI.localization.format("Off")),
        new GUIContent(MenuConfigurationGraphicsUI.localization.format("Low")),
        new GUIContent(MenuConfigurationGraphicsUI.localization.format("Medium")),
        new GUIContent(MenuConfigurationGraphicsUI.localization.format("High")),
        new GUIContent(MenuConfigurationGraphicsUI.localization.format("Ultra"))
      });
      MenuConfigurationGraphicsUI.lightingButton.positionOffset_Y = 500;
      MenuConfigurationGraphicsUI.lightingButton.sizeOffset_X = 200;
      MenuConfigurationGraphicsUI.lightingButton.sizeOffset_Y = 30;
      MenuConfigurationGraphicsUI.lightingButton.state = (int) GraphicsSettings.lightingQuality;
      MenuConfigurationGraphicsUI.lightingButton.addLabel(MenuConfigurationGraphicsUI.localization.format("Lighting_Button_Label"), ESleekSide.RIGHT);
      MenuConfigurationGraphicsUI.lightingButton.onSwappedState = new SwappedState(MenuConfigurationGraphicsUI.onSwappedLightingState);
      MenuConfigurationGraphicsUI.graphicsBox.add((Sleek) MenuConfigurationGraphicsUI.lightingButton);
      MenuConfigurationGraphicsUI.waterButton = new SleekButtonState(new GUIContent[4]
      {
        new GUIContent(MenuConfigurationGraphicsUI.localization.format("Low")),
        new GUIContent(MenuConfigurationGraphicsUI.localization.format("Medium")),
        new GUIContent(MenuConfigurationGraphicsUI.localization.format("High")),
        new GUIContent(MenuConfigurationGraphicsUI.localization.format("Ultra"))
      });
      MenuConfigurationGraphicsUI.waterButton.positionOffset_Y = 540;
      MenuConfigurationGraphicsUI.waterButton.sizeOffset_X = 200;
      MenuConfigurationGraphicsUI.waterButton.sizeOffset_Y = 30;
      MenuConfigurationGraphicsUI.waterButton.state = (int) (GraphicsSettings.waterQuality - 1);
      MenuConfigurationGraphicsUI.waterButton.addLabel(MenuConfigurationGraphicsUI.localization.format("Water_Button_Label"), ESleekSide.RIGHT);
      MenuConfigurationGraphicsUI.waterButton.onSwappedState = new SwappedState(MenuConfigurationGraphicsUI.onSwappedWaterState);
      MenuConfigurationGraphicsUI.graphicsBox.add((Sleek) MenuConfigurationGraphicsUI.waterButton);
      MenuConfigurationGraphicsUI.scopeButton = new SleekButtonState(new GUIContent[5]
      {
        new GUIContent(MenuConfigurationGraphicsUI.localization.format("Off")),
        new GUIContent(MenuConfigurationGraphicsUI.localization.format("Low")),
        new GUIContent(MenuConfigurationGraphicsUI.localization.format("Medium")),
        new GUIContent(MenuConfigurationGraphicsUI.localization.format("High")),
        new GUIContent(MenuConfigurationGraphicsUI.localization.format("Ultra"))
      });
      MenuConfigurationGraphicsUI.scopeButton.positionOffset_Y = 580;
      MenuConfigurationGraphicsUI.scopeButton.sizeOffset_X = 200;
      MenuConfigurationGraphicsUI.scopeButton.sizeOffset_Y = 30;
      MenuConfigurationGraphicsUI.scopeButton.state = (int) GraphicsSettings.scopeQuality;
      MenuConfigurationGraphicsUI.scopeButton.addLabel(MenuConfigurationGraphicsUI.localization.format("Scope_Button_Label"), ESleekSide.RIGHT);
      MenuConfigurationGraphicsUI.scopeButton.onSwappedState = new SwappedState(MenuConfigurationGraphicsUI.onSwappedScopeState);
      MenuConfigurationGraphicsUI.graphicsBox.add((Sleek) MenuConfigurationGraphicsUI.scopeButton);
      MenuConfigurationGraphicsUI.outlineButton = new SleekButtonState(new GUIContent[4]
      {
        new GUIContent(MenuConfigurationGraphicsUI.localization.format("Low")),
        new GUIContent(MenuConfigurationGraphicsUI.localization.format("Medium")),
        new GUIContent(MenuConfigurationGraphicsUI.localization.format("High")),
        new GUIContent(MenuConfigurationGraphicsUI.localization.format("Ultra"))
      });
      MenuConfigurationGraphicsUI.outlineButton.positionOffset_Y = 620;
      MenuConfigurationGraphicsUI.outlineButton.sizeOffset_X = 200;
      MenuConfigurationGraphicsUI.outlineButton.sizeOffset_Y = 30;
      MenuConfigurationGraphicsUI.outlineButton.state = (int) (GraphicsSettings.outlineQuality - 1);
      MenuConfigurationGraphicsUI.outlineButton.addLabel(MenuConfigurationGraphicsUI.localization.format("Outline_Button_Label"), ESleekSide.RIGHT);
      MenuConfigurationGraphicsUI.outlineButton.onSwappedState = new SwappedState(MenuConfigurationGraphicsUI.onSwappedOutlineState);
      MenuConfigurationGraphicsUI.graphicsBox.add((Sleek) MenuConfigurationGraphicsUI.outlineButton);
    }

    public static void open()
    {
      if (MenuConfigurationGraphicsUI.active)
      {
        MenuConfigurationGraphicsUI.close();
      }
      else
      {
        MenuConfigurationGraphicsUI.active = true;
        MenuConfigurationGraphicsUI.container.lerpPositionScale(0.0f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
      }
    }

    public static void close()
    {
      if (!MenuConfigurationGraphicsUI.active)
        return;
      MenuConfigurationGraphicsUI.active = false;
      MenuConfigurationGraphicsUI.container.lerpPositionScale(0.0f, 1f, ESleekLerp.EXPONENTIAL, 20f);
    }

    private static void onToggledMotionBlurToggle(SleekToggle toggle, bool state)
    {
      GraphicsSettings.motionBlur = state;
      GraphicsSettings.apply();
    }

    private static void onToggledAmbientOcclusionToggle(SleekToggle toggle, bool state)
    {
      GraphicsSettings.ambientOcclusion = state;
      GraphicsSettings.apply();
    }

    private static void onToggledSunShaftsToggle(SleekToggle toggle, bool state)
    {
      GraphicsSettings.sunShafts = state;
      GraphicsSettings.apply();
    }

    private static void onToggledBloomToggle(SleekToggle toggle, bool state)
    {
      GraphicsSettings.bloom = state;
      GraphicsSettings.apply();
    }

    private static void onToggledCloudsToggle(SleekToggle toggle, bool state)
    {
      GraphicsSettings.clouds = state;
      GraphicsSettings.apply();
    }

    private static void onToggledTerrainToggle(SleekToggle toggle, bool state)
    {
      GraphicsSettings.terrain = state;
      GraphicsSettings.apply();
    }

    private static void onToggledFogToggle(SleekToggle toggle, bool state)
    {
      GraphicsSettings.fog = state;
      GraphicsSettings.apply();
    }

    private static void onDraggedDistanceSlider(SleekSlider slider, float state)
    {
      GraphicsSettings.distance = state;
      GraphicsSettings.apply();
      MenuConfigurationGraphicsUI.distanceSlider.updateLabel(MenuConfigurationGraphicsUI.localization.format("Distance_Slider_Label", (object) (25 + (int) ((double) state * 75.0))));
    }

    private static void onSwappedAntiAliasingState(SleekButtonState button, int index)
    {
      GraphicsSettings.antiAliasingType = (EAntiAliasingType) index;
      GraphicsSettings.apply();
    }

    private static void onSwappedEffectState(SleekButtonState button, int index)
    {
      GraphicsSettings.effectQuality = (EGraphicQuality) index;
      GraphicsSettings.apply();
    }

    private static void onSwappedFoliageState(SleekButtonState button, int index)
    {
      GraphicsSettings.foliageQuality = (EGraphicQuality) index;
      GraphicsSettings.apply();
    }

    private static void onSwappedLightingState(SleekButtonState button, int index)
    {
      GraphicsSettings.lightingQuality = (EGraphicQuality) index;
      GraphicsSettings.apply();
    }

    private static void onSwappedWaterState(SleekButtonState button, int index)
    {
      GraphicsSettings.waterQuality = (EGraphicQuality) (index + 1);
      GraphicsSettings.apply();
    }

    private static void onSwappedScopeState(SleekButtonState button, int index)
    {
      GraphicsSettings.scopeQuality = (EGraphicQuality) index;
      GraphicsSettings.apply();
    }

    private static void onSwappedOutlineState(SleekButtonState button, int index)
    {
      GraphicsSettings.outlineQuality = (EGraphicQuality) (index + 1);
      GraphicsSettings.apply();
    }
  }
}
