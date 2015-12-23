// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.MenuUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class MenuUI : MonoBehaviour
  {
    private static readonly float ALERT_TIME = 4f;
    public static SleekWindow window;
    public static Sleek container;
    private static MenuUI ui;
    private static SleekBox alertBox;
    private static SleekLabel originLabel;
    private static SleekInventory packageButton;
    private static bool isAlerting;
    private static float lastAlert;
    private Transform title;
    private Transform play;
    private Transform survivors;
    private Transform configuration;
    private Transform workshop;
    private Transform target;
    private static bool hasPanned;
    private static bool hasTitled;

    private static void alertText()
    {
      MenuUI.alertBox.positionOffset_Y = -25;
      MenuUI.alertBox.sizeOffset_Y = 50;
      MenuUI.originLabel.isVisible = false;
      MenuUI.packageButton.isVisible = false;
    }

    private static void alertItem()
    {
      MenuUI.alertBox.text = string.Empty;
      MenuUI.alertBox.positionOffset_Y = -150;
      MenuUI.alertBox.sizeOffset_Y = 300;
      MenuUI.originLabel.isVisible = true;
      MenuUI.packageButton.isVisible = true;
    }

    public static void openAlert()
    {
      MenuUI.alertBox.lerpPositionScale(0.0f, 0.5f, ESleekLerp.EXPONENTIAL, 20f);
      MenuUI.container.lerpPositionScale(-1f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
    }

    public static void openAlert(string alert)
    {
      MenuUI.alertText();
      MenuUI.alertBox.text = alert;
      MenuUI.openAlert();
    }

    public static void closeAlert()
    {
      MenuUI.alertBox.lerpPositionScale(1f, 0.5f, ESleekLerp.EXPONENTIAL, 20f);
      MenuUI.container.lerpPositionScale(0.0f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
      SleekRender.allowInput = true;
    }

    public static void alert(string alert)
    {
      MenuUI.openAlert(alert);
      MenuUI.isAlerting = true;
      MenuUI.lastAlert = Time.realtimeSinceStartup;
    }

    public static void alert(string origin, int item, ushort quantity)
    {
      MenuUI.originLabel.text = origin;
      MenuUI.originLabel.foregroundColor = Provider.provider.economyService.getInventoryColor(item);
      MenuUI.packageButton.updateInventory(item, quantity, false, true);
      MenuUI.alertItem();
      MenuUI.openAlert();
      MenuUI.isAlerting = true;
      MenuUI.lastAlert = Time.realtimeSinceStartup;
    }

    public static void rebuild()
    {
      MenuUI.ui.Invoke("init", 0.1f);
    }

    public void build()
    {
      MenuUI.window.build();
      LoadingUI.rebuild();
    }

    public void init()
    {
      GraphicsSettings.resize();
      this.Invoke("build", 0.1f);
    }

    private void OnGUI()
    {
      if (MenuUI.window == null)
        return;
      MenuUI.window.draw(false);
      MenuSurvivorsClothingBoxUI.update();
      MenuConfigurationControlsUI.update();
    }

    private void Update()
    {
      if (MenuUI.window == null)
        return;
      if ((int) MenuConfigurationControlsUI.binding == (int) byte.MaxValue)
      {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
          if (MenuUI.isAlerting)
          {
            MenuUI.closeAlert();
            MenuUI.isAlerting = false;
          }
          else if (MenuPauseUI.active)
          {
            MenuPauseUI.close();
            MenuDashboardUI.open();
            MenuTitleUI.open();
          }
          else if (MenuTitleUI.active)
          {
            MenuPauseUI.open();
            MenuDashboardUI.close();
            MenuTitleUI.close();
          }
          else if (MenuPlayConnectUI.active || MenuPlayServersUI.active || MenuPlaySingleplayerUI.active)
          {
            MenuPlayConnectUI.close();
            MenuPlayServersUI.close();
            MenuPlaySingleplayerUI.close();
            MenuPlayUI.open();
          }
          else if (MenuSurvivorsClothingItemUI.active)
          {
            MenuSurvivorsClothingItemUI.close();
            MenuSurvivorsClothingUI.open();
          }
          else if (MenuSurvivorsClothingBoxUI.active)
          {
            if (!MenuSurvivorsClothingBoxUI.isUnboxing)
            {
              MenuSurvivorsClothingBoxUI.close();
              MenuSurvivorsClothingItemUI.open();
            }
          }
          else if (MenuSurvivorsClothingInspectUI.active)
          {
            MenuSurvivorsClothingInspectUI.close();
            MenuSurvivorsClothingItemUI.open();
          }
          else if (MenuSurvivorsCharacterUI.active || MenuSurvivorsAppearanceUI.active || (MenuSurvivorsGroupUI.active || MenuSurvivorsClothingUI.active))
          {
            MenuSurvivorsCharacterUI.close();
            MenuSurvivorsAppearanceUI.close();
            MenuSurvivorsGroupUI.close();
            MenuSurvivorsClothingUI.close();
            MenuSurvivorsUI.open();
          }
          else if (MenuConfigurationOptionsUI.active || MenuConfigurationControlsUI.active || (MenuConfigurationGraphicsUI.active || MenuConfigurationDisplayUI.active))
          {
            MenuConfigurationOptionsUI.close();
            MenuConfigurationControlsUI.close();
            MenuConfigurationGraphicsUI.close();
            MenuConfigurationDisplayUI.close();
            MenuConfigurationUI.open();
          }
          else if (MenuWorkshopSubmitUI.active || MenuWorkshopEditorUI.active)
          {
            MenuWorkshopSubmitUI.close();
            MenuWorkshopEditorUI.close();
            MenuWorkshopUI.open();
          }
          else
          {
            MenuPlayUI.close();
            MenuSurvivorsUI.close();
            MenuConfigurationUI.close();
            MenuWorkshopUI.close();
            MenuDashboardUI.open();
            MenuTitleUI.open();
          }
        }
        if (Input.GetKeyDown(ControlsSettings.hud))
          MenuUI.window.isEnabled = !MenuUI.window.isEnabled;
      }
      if (Input.GetKeyDown(KeyCode.Insert))
        Assets.refresh();
      if (MenuUI.isAlerting && (double) Time.realtimeSinceStartup - (double) MenuUI.lastAlert > (double) MenuUI.ALERT_TIME)
      {
        MenuUI.closeAlert();
        MenuUI.isAlerting = false;
      }
      MenuUI.window.showCursor = true;
      MenuUI.window.updateDebug();
      this.target = MenuPlayUI.active || MenuPlayConnectUI.active || (MenuPlayServersUI.active || MenuPlaySingleplayerUI.active) ? this.play : (MenuSurvivorsUI.active || MenuSurvivorsCharacterUI.active || (MenuSurvivorsAppearanceUI.active || MenuSurvivorsGroupUI.active) || (MenuSurvivorsClothingUI.active || MenuSurvivorsClothingItemUI.active || (MenuSurvivorsClothingInspectUI.active || MenuSurvivorsClothingBoxUI.active)) ? this.survivors : (MenuConfigurationUI.active || MenuConfigurationOptionsUI.active || (MenuConfigurationControlsUI.active || MenuConfigurationGraphicsUI.active) || MenuConfigurationDisplayUI.active ? this.configuration : (MenuWorkshopUI.active || MenuWorkshopSubmitUI.active || MenuWorkshopEditorUI.active ? this.workshop : this.title)));
      if ((Object) this.target == (Object) this.title)
      {
        if (MenuUI.hasTitled)
        {
          this.transform.position = Vector3.Lerp(this.transform.position, this.target.position, Time.deltaTime * 4f);
          this.transform.rotation = Quaternion.Lerp(this.transform.rotation, this.target.rotation, Time.deltaTime * 4f);
        }
        else
        {
          this.transform.position = Vector3.Lerp(this.transform.position, this.target.position, Time.deltaTime);
          this.transform.rotation = Quaternion.Lerp(this.transform.rotation, this.target.rotation, Time.deltaTime);
        }
      }
      else
      {
        MenuUI.hasTitled = true;
        this.transform.position = Vector3.Lerp(this.transform.position, this.target.position, Time.deltaTime * 4f);
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, this.target.rotation, Time.deltaTime * 4f);
      }
    }

    private void Start()
    {
      if (Dedicator.isDedicated)
        return;
      MenuUI.window = new SleekWindow();
      MenuUI.container = new Sleek();
      MenuUI.container.sizeScale_X = 1f;
      MenuUI.container.sizeScale_Y = 1f;
      MenuUI.window.add(MenuUI.container);
      MenuUI.alertBox = new SleekBox();
      MenuUI.alertBox.positionOffset_X = 10;
      MenuUI.alertBox.positionOffset_Y = -25;
      MenuUI.alertBox.positionScale_X = 1f;
      MenuUI.alertBox.positionScale_Y = 0.5f;
      MenuUI.alertBox.sizeScale_X = 1f;
      MenuUI.alertBox.sizeOffset_X = -20;
      MenuUI.alertBox.sizeOffset_Y = 50;
      MenuUI.alertBox.fontSize = 14;
      MenuUI.window.add((Sleek) MenuUI.alertBox);
      MenuUI.originLabel = new SleekLabel();
      MenuUI.originLabel.sizeOffset_Y = 50;
      MenuUI.originLabel.sizeScale_X = 1f;
      MenuUI.originLabel.fontSize = 18;
      MenuUI.alertBox.add((Sleek) MenuUI.originLabel);
      MenuUI.originLabel.isVisible = false;
      MenuUI.packageButton = new SleekInventory();
      MenuUI.packageButton.positionOffset_X = -100;
      MenuUI.packageButton.positionOffset_Y = 75;
      MenuUI.packageButton.positionScale_X = 0.5f;
      MenuUI.packageButton.sizeOffset_X = 200;
      MenuUI.packageButton.sizeOffset_Y = 200;
      MenuUI.alertBox.add((Sleek) MenuUI.packageButton);
      MenuUI.packageButton.isVisible = false;
      OptionsSettings.apply();
      GraphicsSettings.apply();
      MenuDashboardUI menuDashboardUi = new MenuDashboardUI();
      this.title = this.transform.parent.FindChild("Title");
      this.play = this.transform.parent.FindChild("Play");
      this.survivors = this.transform.parent.FindChild("Survivors");
      this.configuration = this.transform.parent.FindChild("Configuration");
      this.workshop = this.transform.parent.FindChild("Workshop");
      if (MenuUI.hasPanned)
      {
        this.transform.position = this.title.position;
        this.transform.rotation = this.title.rotation;
      }
      MenuUI.hasPanned = true;
    }

    private void Awake()
    {
      MenuUI.ui = this;
      Time.timeScale = 1f;
    }
  }
}
