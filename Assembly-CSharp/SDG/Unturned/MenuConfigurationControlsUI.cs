// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.MenuConfigurationControlsUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class MenuConfigurationControlsUI
  {
    private static byte[][] layouts = new byte[7][]
    {
      new byte[6]
      {
        ControlsSettings.UP,
        ControlsSettings.DOWN,
        ControlsSettings.LEFT,
        ControlsSettings.RIGHT,
        ControlsSettings.JUMP,
        ControlsSettings.SPRINT
      },
      new byte[7]
      {
        ControlsSettings.CROUCH,
        ControlsSettings.PRONE,
        ControlsSettings.STANCE,
        ControlsSettings.LEAN_LEFT,
        ControlsSettings.LEAN_RIGHT,
        ControlsSettings.PERSPECTIVE,
        ControlsSettings.GESTURE
      },
      new byte[3]
      {
        ControlsSettings.INTERACT,
        ControlsSettings.PRIMARY,
        ControlsSettings.SECONDARY
      },
      new byte[6]
      {
        ControlsSettings.RELOAD,
        ControlsSettings.ATTACH,
        ControlsSettings.FIREMODE,
        ControlsSettings.TACTICAL,
        ControlsSettings.VISION,
        ControlsSettings.DEQUIP
      },
      new byte[4]
      {
        ControlsSettings.VOICE,
        ControlsSettings.GLOBAL,
        ControlsSettings.LOCAL,
        ControlsSettings.GROUP
      },
      new byte[8]
      {
        ControlsSettings.HUD,
        ControlsSettings.OTHER,
        ControlsSettings.DASHBOARD,
        ControlsSettings.INVENTORY,
        ControlsSettings.CRAFTING,
        ControlsSettings.SKILLS,
        ControlsSettings.MAP,
        ControlsSettings.PLAYERS
      },
      new byte[6]
      {
        ControlsSettings.MODIFY,
        ControlsSettings.SNAP,
        ControlsSettings.FOCUS,
        ControlsSettings.TOOL_0,
        ControlsSettings.TOOL_1,
        ControlsSettings.TOOL_2
      }
    };
    private static Local localization;
    private static Sleek container;
    public static bool active;
    private static SleekSlider sensitivitySlider;
    private static SleekToggle invertToggle;
    private static SleekScrollBox controlsBox;
    private static SleekButton[] buttons;
    private static SleekButtonState aimingButton;
    private static SleekButtonState crouchingButton;
    private static SleekButtonState proningButton;
    private static SleekButtonState sprintingButton;
    public static byte binding;

    public MenuConfigurationControlsUI()
    {
      MenuConfigurationControlsUI.localization = Localization.read("/Menu/Configuration/MenuConfigurationControls.dat");
      MenuConfigurationControlsUI.container = new Sleek();
      MenuConfigurationControlsUI.container.positionOffset_X = 10;
      MenuConfigurationControlsUI.container.positionOffset_Y = 10;
      MenuConfigurationControlsUI.container.positionScale_Y = 1f;
      MenuConfigurationControlsUI.container.sizeOffset_X = -20;
      MenuConfigurationControlsUI.container.sizeOffset_Y = -20;
      MenuConfigurationControlsUI.container.sizeScale_X = 1f;
      MenuConfigurationControlsUI.container.sizeScale_Y = 1f;
      if (Provider.isConnected)
        PlayerUI.container.add(MenuConfigurationControlsUI.container);
      else
        MenuUI.container.add(MenuConfigurationControlsUI.container);
      MenuConfigurationControlsUI.active = false;
      MenuConfigurationControlsUI.binding = byte.MaxValue;
      MenuConfigurationControlsUI.controlsBox = new SleekScrollBox();
      MenuConfigurationControlsUI.controlsBox.positionOffset_X = -200;
      MenuConfigurationControlsUI.controlsBox.positionOffset_Y = 100;
      MenuConfigurationControlsUI.controlsBox.positionScale_X = 0.5f;
      MenuConfigurationControlsUI.controlsBox.sizeOffset_X = 430;
      MenuConfigurationControlsUI.controlsBox.sizeOffset_Y = -200;
      MenuConfigurationControlsUI.controlsBox.sizeScale_Y = 1f;
      MenuConfigurationControlsUI.controlsBox.area = new Rect(0.0f, 0.0f, 5f, (float) (280 + (ControlsSettings.bindings.Length + (MenuConfigurationControlsUI.layouts.Length - 1) * 2) * 40 - 10));
      MenuConfigurationControlsUI.container.add((Sleek) MenuConfigurationControlsUI.controlsBox);
      MenuConfigurationControlsUI.sensitivitySlider = new SleekSlider();
      MenuConfigurationControlsUI.sensitivitySlider.positionOffset_Y = 50;
      MenuConfigurationControlsUI.sensitivitySlider.sizeOffset_X = 200;
      MenuConfigurationControlsUI.sensitivitySlider.sizeOffset_Y = 20;
      MenuConfigurationControlsUI.sensitivitySlider.orientation = ESleekOrientation.HORIZONTAL;
      MenuConfigurationControlsUI.sensitivitySlider.addLabel(MenuConfigurationControlsUI.localization.format("Sensitivity_Slider_Label", (object) (float) (1.0 + (double) (int) ((double) ControlsSettings.sensitivity * 90.0) / 10.0)), ESleekSide.RIGHT);
      MenuConfigurationControlsUI.sensitivitySlider.state = ControlsSettings.sensitivity;
      MenuConfigurationControlsUI.sensitivitySlider.onDragged = new Dragged(MenuConfigurationControlsUI.onDraggedSensitivitySlider);
      MenuConfigurationControlsUI.controlsBox.add((Sleek) MenuConfigurationControlsUI.sensitivitySlider);
      MenuConfigurationControlsUI.invertToggle = new SleekToggle();
      MenuConfigurationControlsUI.invertToggle.sizeOffset_X = 40;
      MenuConfigurationControlsUI.invertToggle.sizeOffset_Y = 40;
      MenuConfigurationControlsUI.invertToggle.addLabel(MenuConfigurationControlsUI.localization.format("Invert_Toggle_Label"), ESleekSide.RIGHT);
      MenuConfigurationControlsUI.invertToggle.state = ControlsSettings.invert;
      MenuConfigurationControlsUI.invertToggle.onToggled = new Toggled(MenuConfigurationControlsUI.onToggledInvertToggle);
      MenuConfigurationControlsUI.controlsBox.add((Sleek) MenuConfigurationControlsUI.invertToggle);
      MenuConfigurationControlsUI.aimingButton = new SleekButtonState(new GUIContent[2]
      {
        new GUIContent(MenuConfigurationControlsUI.localization.format("Hold")),
        new GUIContent(MenuConfigurationControlsUI.localization.format("Toggle"))
      });
      MenuConfigurationControlsUI.aimingButton.positionOffset_Y = 80;
      MenuConfigurationControlsUI.aimingButton.sizeOffset_X = 200;
      MenuConfigurationControlsUI.aimingButton.sizeOffset_Y = 30;
      MenuConfigurationControlsUI.aimingButton.state = (int) ControlsSettings.aiming;
      MenuConfigurationControlsUI.aimingButton.addLabel(MenuConfigurationControlsUI.localization.format("Aiming_Label"), ESleekSide.RIGHT);
      MenuConfigurationControlsUI.aimingButton.onSwappedState = new SwappedState(MenuConfigurationControlsUI.onSwappedAimingState);
      MenuConfigurationControlsUI.controlsBox.add((Sleek) MenuConfigurationControlsUI.aimingButton);
      MenuConfigurationControlsUI.crouchingButton = new SleekButtonState(new GUIContent[2]
      {
        new GUIContent(MenuConfigurationControlsUI.localization.format("Hold")),
        new GUIContent(MenuConfigurationControlsUI.localization.format("Toggle"))
      });
      MenuConfigurationControlsUI.crouchingButton.positionOffset_Y = 120;
      MenuConfigurationControlsUI.crouchingButton.sizeOffset_X = 200;
      MenuConfigurationControlsUI.crouchingButton.sizeOffset_Y = 30;
      MenuConfigurationControlsUI.crouchingButton.state = (int) ControlsSettings.crouching;
      MenuConfigurationControlsUI.crouchingButton.addLabel(MenuConfigurationControlsUI.localization.format("Crouching_Label"), ESleekSide.RIGHT);
      MenuConfigurationControlsUI.crouchingButton.onSwappedState = new SwappedState(MenuConfigurationControlsUI.onSwappedCrouchingState);
      MenuConfigurationControlsUI.controlsBox.add((Sleek) MenuConfigurationControlsUI.crouchingButton);
      MenuConfigurationControlsUI.proningButton = new SleekButtonState(new GUIContent[2]
      {
        new GUIContent(MenuConfigurationControlsUI.localization.format("Hold")),
        new GUIContent(MenuConfigurationControlsUI.localization.format("Toggle"))
      });
      MenuConfigurationControlsUI.proningButton.positionOffset_Y = 160;
      MenuConfigurationControlsUI.proningButton.sizeOffset_X = 200;
      MenuConfigurationControlsUI.proningButton.sizeOffset_Y = 30;
      MenuConfigurationControlsUI.proningButton.state = (int) ControlsSettings.proning;
      MenuConfigurationControlsUI.proningButton.addLabel(MenuConfigurationControlsUI.localization.format("Proning_Label"), ESleekSide.RIGHT);
      MenuConfigurationControlsUI.proningButton.onSwappedState = new SwappedState(MenuConfigurationControlsUI.onSwappedProningState);
      MenuConfigurationControlsUI.controlsBox.add((Sleek) MenuConfigurationControlsUI.proningButton);
      MenuConfigurationControlsUI.sprintingButton = new SleekButtonState(new GUIContent[2]
      {
        new GUIContent(MenuConfigurationControlsUI.localization.format("Hold")),
        new GUIContent(MenuConfigurationControlsUI.localization.format("Toggle"))
      });
      MenuConfigurationControlsUI.sprintingButton.positionOffset_Y = 200;
      MenuConfigurationControlsUI.sprintingButton.sizeOffset_X = 200;
      MenuConfigurationControlsUI.sprintingButton.sizeOffset_Y = 30;
      MenuConfigurationControlsUI.sprintingButton.state = (int) ControlsSettings.sprinting;
      MenuConfigurationControlsUI.sprintingButton.addLabel(MenuConfigurationControlsUI.localization.format("Sprinting_Label"), ESleekSide.RIGHT);
      MenuConfigurationControlsUI.sprintingButton.onSwappedState = new SwappedState(MenuConfigurationControlsUI.onSwappedSprintingState);
      MenuConfigurationControlsUI.controlsBox.add((Sleek) MenuConfigurationControlsUI.sprintingButton);
      MenuConfigurationControlsUI.buttons = new SleekButton[ControlsSettings.bindings.Length];
      byte num = (byte) 0;
      for (byte index1 = (byte) 0; (int) index1 < MenuConfigurationControlsUI.layouts.Length; ++index1)
      {
        SleekBox sleekBox = new SleekBox();
        sleekBox.positionOffset_Y = 280 + ((int) num + (int) index1 * 2) * 40;
        sleekBox.sizeOffset_X = -30;
        sleekBox.sizeOffset_Y = 30;
        sleekBox.sizeScale_X = 1f;
        sleekBox.text = MenuConfigurationControlsUI.localization.format("Layout_" + (object) index1);
        MenuConfigurationControlsUI.controlsBox.add((Sleek) sleekBox);
        for (byte index2 = (byte) 0; (int) index2 < MenuConfigurationControlsUI.layouts[(int) index1].Length; ++index2)
        {
          SleekButton sleekButton = new SleekButton();
          sleekButton.positionOffset_Y = ((int) index2 + 1) * 40;
          sleekButton.sizeOffset_Y = 30;
          sleekButton.sizeScale_X = 1f;
          sleekButton.onClickedButton = new ClickedButton(MenuConfigurationControlsUI.onClickedKeyButton);
          sleekBox.add((Sleek) sleekButton);
          MenuConfigurationControlsUI.buttons[(int) MenuConfigurationControlsUI.layouts[(int) index1][(int) index2]] = sleekButton;
          MenuConfigurationControlsUI.updateButton(MenuConfigurationControlsUI.layouts[(int) index1][(int) index2]);
          ++num;
        }
      }
    }

    public static void open()
    {
      if (MenuConfigurationControlsUI.active)
      {
        MenuConfigurationControlsUI.close();
      }
      else
      {
        MenuConfigurationControlsUI.active = true;
        MenuConfigurationControlsUI.container.lerpPositionScale(0.0f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
      }
    }

    public static void close()
    {
      if (!MenuConfigurationControlsUI.active)
        return;
      MenuConfigurationControlsUI.active = false;
      MenuConfigurationControlsUI.binding = byte.MaxValue;
      MenuConfigurationControlsUI.container.lerpPositionScale(0.0f, 1f, ESleekLerp.EXPONENTIAL, 20f);
    }

    public static void cancel()
    {
      MenuConfigurationControlsUI.binding = byte.MaxValue;
      SleekRender.allowInput = true;
    }

    public static void bind(KeyCode key)
    {
      ControlsSettings.bind(MenuConfigurationControlsUI.binding, key);
      MenuConfigurationControlsUI.updateButton(MenuConfigurationControlsUI.binding);
      MenuConfigurationControlsUI.cancel();
    }

    public static void updateButton(byte index)
    {
      KeyCode keyCode = ControlsSettings.bindings[(int) index].key;
      switch (keyCode)
      {
        case KeyCode.Mouse0:
          MenuConfigurationControlsUI.buttons[(int) index].text = MenuConfigurationControlsUI.localization.format("Key_" + (object) index + "_Button", (object) MenuConfigurationControlsUI.localization.format("Mouse_0"));
          break;
        case KeyCode.Mouse1:
          MenuConfigurationControlsUI.buttons[(int) index].text = MenuConfigurationControlsUI.localization.format("Key_" + (object) index + "_Button", (object) MenuConfigurationControlsUI.localization.format("Mouse_1"));
          break;
        case KeyCode.Mouse2:
          MenuConfigurationControlsUI.buttons[(int) index].text = MenuConfigurationControlsUI.localization.format("Key_" + (object) index + "_Button", (object) MenuConfigurationControlsUI.localization.format("Mouse_2"));
          break;
        case KeyCode.Mouse3:
          MenuConfigurationControlsUI.buttons[(int) index].text = MenuConfigurationControlsUI.localization.format("Key_" + (object) index + "_Button", (object) MenuConfigurationControlsUI.localization.format("Mouse_3"));
          break;
        case KeyCode.Mouse4:
          MenuConfigurationControlsUI.buttons[(int) index].text = MenuConfigurationControlsUI.localization.format("Key_" + (object) index + "_Button", (object) MenuConfigurationControlsUI.localization.format("Mouse_4"));
          break;
        case KeyCode.Mouse5:
          MenuConfigurationControlsUI.buttons[(int) index].text = MenuConfigurationControlsUI.localization.format("Key_" + (object) index + "_Button", (object) MenuConfigurationControlsUI.localization.format("Mouse_5"));
          break;
        case KeyCode.Alpha0:
          MenuConfigurationControlsUI.buttons[(int) index].text = MenuConfigurationControlsUI.localization.format("Key_" + (object) index + "_Button", (object) 0);
          break;
        case KeyCode.Alpha1:
          MenuConfigurationControlsUI.buttons[(int) index].text = MenuConfigurationControlsUI.localization.format("Key_" + (object) index + "_Button", (object) 1);
          break;
        case KeyCode.Alpha2:
          MenuConfigurationControlsUI.buttons[(int) index].text = MenuConfigurationControlsUI.localization.format("Key_" + (object) index + "_Button", (object) 2);
          break;
        case KeyCode.Alpha3:
          MenuConfigurationControlsUI.buttons[(int) index].text = MenuConfigurationControlsUI.localization.format("Key_" + (object) index + "_Button", (object) 3);
          break;
        case KeyCode.Alpha4:
          MenuConfigurationControlsUI.buttons[(int) index].text = MenuConfigurationControlsUI.localization.format("Key_" + (object) index + "_Button", (object) 4);
          break;
        case KeyCode.Alpha5:
          MenuConfigurationControlsUI.buttons[(int) index].text = MenuConfigurationControlsUI.localization.format("Key_" + (object) index + "_Button", (object) 5);
          break;
        case KeyCode.Alpha6:
          MenuConfigurationControlsUI.buttons[(int) index].text = MenuConfigurationControlsUI.localization.format("Key_" + (object) index + "_Button", (object) 6);
          break;
        case KeyCode.Alpha7:
          MenuConfigurationControlsUI.buttons[(int) index].text = MenuConfigurationControlsUI.localization.format("Key_" + (object) index + "_Button", (object) 7);
          break;
        case KeyCode.Alpha8:
          MenuConfigurationControlsUI.buttons[(int) index].text = MenuConfigurationControlsUI.localization.format("Key_" + (object) index + "_Button", (object) 8);
          break;
        case KeyCode.Alpha9:
          MenuConfigurationControlsUI.buttons[(int) index].text = MenuConfigurationControlsUI.localization.format("Key_" + (object) index + "_Button", (object) 9);
          break;
        default:
          MenuConfigurationControlsUI.buttons[(int) index].text = MenuConfigurationControlsUI.localization.format("Key_" + (object) index + "_Button", (object) keyCode);
          break;
      }
    }

    private static void onDraggedSensitivitySlider(SleekSlider slider, float state)
    {
      ControlsSettings.sensitivity = state;
      MenuConfigurationControlsUI.sensitivitySlider.updateLabel(MenuConfigurationControlsUI.localization.format("Sensitivity_Slider_Label", (object) (float) (1.0 + (double) (int) ((double) state * 90.0) / 10.0)));
    }

    private static void onToggledInvertToggle(SleekToggle toggle, bool state)
    {
      ControlsSettings.invert = state;
    }

    private static void onSwappedAimingState(SleekButtonState button, int index)
    {
      ControlsSettings.aiming = (EControlMode) index;
    }

    private static void onSwappedCrouchingState(SleekButtonState button, int index)
    {
      ControlsSettings.crouching = (EControlMode) index;
    }

    private static void onSwappedProningState(SleekButtonState button, int index)
    {
      ControlsSettings.proning = (EControlMode) index;
    }

    private static void onSwappedSprintingState(SleekButtonState button, int index)
    {
      ControlsSettings.sprinting = (EControlMode) index;
    }

    private static void onClickedKeyButton(SleekButton button)
    {
      SleekRender.allowInput = false;
      MenuConfigurationControlsUI.binding = (byte) 0;
      while ((int) MenuConfigurationControlsUI.binding < MenuConfigurationControlsUI.buttons.Length && MenuConfigurationControlsUI.buttons[(int) MenuConfigurationControlsUI.binding] != button)
        ++MenuConfigurationControlsUI.binding;
      button.text = MenuConfigurationControlsUI.localization.format("Key_" + (object) MenuConfigurationControlsUI.binding + "_Button", (object) "?");
    }

    public static void update()
    {
      if ((int) MenuConfigurationControlsUI.binding == (int) byte.MaxValue)
        return;
      if (Event.current.type == EventType.KeyDown)
      {
        if (Event.current.keyCode == KeyCode.Backspace)
        {
          MenuConfigurationControlsUI.updateButton(MenuConfigurationControlsUI.binding);
          MenuConfigurationControlsUI.cancel();
        }
        else
        {
          if (Event.current.keyCode == KeyCode.Escape || Event.current.keyCode == KeyCode.Insert)
            return;
          MenuConfigurationControlsUI.bind(Event.current.keyCode);
        }
      }
      else if (Event.current.type == EventType.MouseDown)
      {
        if (Event.current.button == 0)
          MenuConfigurationControlsUI.bind(KeyCode.Mouse0);
        else if (Event.current.button == 1)
          MenuConfigurationControlsUI.bind(KeyCode.Mouse1);
        else if (Event.current.button == 2)
          MenuConfigurationControlsUI.bind(KeyCode.Mouse2);
        else if (Event.current.button == 3)
          MenuConfigurationControlsUI.bind(KeyCode.Mouse3);
        else if (Event.current.button == 4)
          MenuConfigurationControlsUI.bind(KeyCode.Mouse4);
        else if (Event.current.button == 5)
        {
          MenuConfigurationControlsUI.bind(KeyCode.Mouse5);
        }
        else
        {
          if (Event.current.button != 6)
            return;
          MenuConfigurationControlsUI.bind(KeyCode.Mouse6);
        }
      }
      else
      {
        if (!Event.current.shift)
          return;
        MenuConfigurationControlsUI.bind(KeyCode.LeftShift);
      }
    }
  }
}
