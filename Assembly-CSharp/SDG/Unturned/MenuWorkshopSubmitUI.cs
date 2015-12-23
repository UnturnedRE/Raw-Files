// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.MenuWorkshopSubmitUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using SDG.Provider;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
  public class MenuWorkshopSubmitUI
  {
    private static Local localization;
    private static Sleek container;
    public static bool active;
    private static SleekField nameField;
    private static SleekField descriptionField;
    private static SleekField pathField;
    private static SleekField previewField;
    private static SleekField changeField;
    private static SleekButtonState typeState;
    private static SleekButtonState mapTypeState;
    private static SleekButtonState itemTypeState;
    private static SleekButtonState vehicleTypeState;
    private static SleekButtonState skinTypeState;
    private static SleekButtonState objectTypeState;
    private static SleekButtonState visibilityState;
    private static SleekButtonState forState;
    private static SleekButtonIcon createButton;
    private static SleekButton legalButton;
    private static SleekScrollBox publishedBox;
    private static List<SleekButton> publishedButtons;

    private static string tag
    {
      get
      {
        switch (MenuWorkshopSubmitUI.typeState.state)
        {
          case 0:
            return MenuWorkshopSubmitUI.mapTypeState.states[MenuWorkshopSubmitUI.mapTypeState.state].text;
          case 2:
            return MenuWorkshopSubmitUI.objectTypeState.states[MenuWorkshopSubmitUI.objectTypeState.state].text;
          case 3:
            return MenuWorkshopSubmitUI.itemTypeState.states[MenuWorkshopSubmitUI.itemTypeState.state].text;
          case 4:
            return MenuWorkshopSubmitUI.vehicleTypeState.states[MenuWorkshopSubmitUI.vehicleTypeState.state].text;
          case 5:
            return MenuWorkshopSubmitUI.skinTypeState.states[MenuWorkshopSubmitUI.skinTypeState.state].text;
          default:
            return string.Empty;
        }
      }
    }

    public MenuWorkshopSubmitUI()
    {
      MenuWorkshopSubmitUI.localization = Localization.read("/Menu/Workshop/MenuWorkshopSubmit.dat");
      Bundle bundle = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Workshop/MenuWorkshopSubmit/MenuWorkshopSubmit.unity3d");
      MenuWorkshopSubmitUI.publishedButtons = new List<SleekButton>();
      Provider.provider.workshopService.onPublishedAdded += new TempSteamworksWorkshop.PublishedAdded(MenuWorkshopSubmitUI.onPublishedAdded);
      Provider.provider.workshopService.onPublishedRemoved += new TempSteamworksWorkshop.PublishedRemoved(MenuWorkshopSubmitUI.onPublishedRemoved);
      MenuWorkshopSubmitUI.container = new Sleek();
      MenuWorkshopSubmitUI.container.positionOffset_X = 10;
      MenuWorkshopSubmitUI.container.positionOffset_Y = 10;
      MenuWorkshopSubmitUI.container.positionScale_Y = 1f;
      MenuWorkshopSubmitUI.container.sizeOffset_X = -20;
      MenuWorkshopSubmitUI.container.sizeOffset_Y = -20;
      MenuWorkshopSubmitUI.container.sizeScale_X = 1f;
      MenuWorkshopSubmitUI.container.sizeScale_Y = 1f;
      MenuUI.container.add(MenuWorkshopSubmitUI.container);
      MenuWorkshopSubmitUI.active = false;
      MenuWorkshopSubmitUI.nameField = new SleekField();
      MenuWorkshopSubmitUI.nameField.positionOffset_X = -200;
      MenuWorkshopSubmitUI.nameField.positionOffset_Y = 100;
      MenuWorkshopSubmitUI.nameField.positionScale_X = 0.5f;
      MenuWorkshopSubmitUI.nameField.sizeOffset_X = 200;
      MenuWorkshopSubmitUI.nameField.sizeOffset_Y = 30;
      MenuWorkshopSubmitUI.nameField.maxLength = 24;
      MenuWorkshopSubmitUI.nameField.addLabel(MenuWorkshopSubmitUI.localization.format("Name_Field_Label"), ESleekSide.RIGHT);
      MenuWorkshopSubmitUI.container.add((Sleek) MenuWorkshopSubmitUI.nameField);
      MenuWorkshopSubmitUI.descriptionField = new SleekField();
      MenuWorkshopSubmitUI.descriptionField.positionOffset_X = -200;
      MenuWorkshopSubmitUI.descriptionField.positionOffset_Y = 140;
      MenuWorkshopSubmitUI.descriptionField.positionScale_X = 0.5f;
      MenuWorkshopSubmitUI.descriptionField.sizeOffset_X = 400;
      MenuWorkshopSubmitUI.descriptionField.sizeOffset_Y = 30;
      MenuWorkshopSubmitUI.descriptionField.maxLength = 128;
      MenuWorkshopSubmitUI.descriptionField.addLabel(MenuWorkshopSubmitUI.localization.format("Description_Field_Label"), ESleekSide.RIGHT);
      MenuWorkshopSubmitUI.container.add((Sleek) MenuWorkshopSubmitUI.descriptionField);
      MenuWorkshopSubmitUI.pathField = new SleekField();
      MenuWorkshopSubmitUI.pathField.positionOffset_X = -200;
      MenuWorkshopSubmitUI.pathField.positionOffset_Y = 180;
      MenuWorkshopSubmitUI.pathField.positionScale_X = 0.5f;
      MenuWorkshopSubmitUI.pathField.sizeOffset_X = 400;
      MenuWorkshopSubmitUI.pathField.sizeOffset_Y = 30;
      MenuWorkshopSubmitUI.pathField.maxLength = 128;
      MenuWorkshopSubmitUI.pathField.addLabel(MenuWorkshopSubmitUI.localization.format("Path_Field_Label"), ESleekSide.RIGHT);
      MenuWorkshopSubmitUI.container.add((Sleek) MenuWorkshopSubmitUI.pathField);
      MenuWorkshopSubmitUI.previewField = new SleekField();
      MenuWorkshopSubmitUI.previewField.positionOffset_X = -200;
      MenuWorkshopSubmitUI.previewField.positionOffset_Y = 220;
      MenuWorkshopSubmitUI.previewField.positionScale_X = 0.5f;
      MenuWorkshopSubmitUI.previewField.sizeOffset_X = 400;
      MenuWorkshopSubmitUI.previewField.sizeOffset_Y = 30;
      MenuWorkshopSubmitUI.previewField.maxLength = 128;
      MenuWorkshopSubmitUI.previewField.addLabel(MenuWorkshopSubmitUI.localization.format("Preview_Field_Label"), ESleekSide.RIGHT);
      MenuWorkshopSubmitUI.container.add((Sleek) MenuWorkshopSubmitUI.previewField);
      MenuWorkshopSubmitUI.changeField = new SleekField();
      MenuWorkshopSubmitUI.changeField.positionOffset_X = -200;
      MenuWorkshopSubmitUI.changeField.positionOffset_Y = 260;
      MenuWorkshopSubmitUI.changeField.positionScale_X = 0.5f;
      MenuWorkshopSubmitUI.changeField.sizeOffset_X = 400;
      MenuWorkshopSubmitUI.changeField.sizeOffset_Y = 30;
      MenuWorkshopSubmitUI.changeField.maxLength = 128;
      MenuWorkshopSubmitUI.changeField.addLabel(MenuWorkshopSubmitUI.localization.format("Change_Field_Label"), ESleekSide.RIGHT);
      MenuWorkshopSubmitUI.container.add((Sleek) MenuWorkshopSubmitUI.changeField);
      MenuWorkshopSubmitUI.typeState = new SleekButtonState(new GUIContent[6]
      {
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Map")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Localization")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Object")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Item")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Vehicle")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin"))
      });
      MenuWorkshopSubmitUI.typeState.positionOffset_X = -200;
      MenuWorkshopSubmitUI.typeState.positionOffset_Y = 300;
      MenuWorkshopSubmitUI.typeState.positionScale_X = 0.5f;
      MenuWorkshopSubmitUI.typeState.sizeOffset_X = 195;
      MenuWorkshopSubmitUI.typeState.sizeOffset_Y = 30;
      MenuWorkshopSubmitUI.typeState.onSwappedState = new SwappedState(MenuWorkshopSubmitUI.onSwappedTypeState);
      MenuWorkshopSubmitUI.container.add((Sleek) MenuWorkshopSubmitUI.typeState);
      MenuWorkshopSubmitUI.mapTypeState = new SleekButtonState(new GUIContent[2]
      {
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Map_Type_Survival")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Map_Type_Horde"))
      });
      MenuWorkshopSubmitUI.mapTypeState.positionOffset_X = 5;
      MenuWorkshopSubmitUI.mapTypeState.positionOffset_Y = 300;
      MenuWorkshopSubmitUI.mapTypeState.positionScale_X = 0.5f;
      MenuWorkshopSubmitUI.mapTypeState.sizeOffset_X = 195;
      MenuWorkshopSubmitUI.mapTypeState.sizeOffset_Y = 30;
      MenuWorkshopSubmitUI.container.add((Sleek) MenuWorkshopSubmitUI.mapTypeState);
      MenuWorkshopSubmitUI.mapTypeState.isVisible = true;
      MenuWorkshopSubmitUI.itemTypeState = new SleekButtonState(new GUIContent[25]
      {
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Item_Type_Backpack")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Item_Type_Barrel")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Item_Type_Barricade")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Item_Type_Fisher")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Item_Type_Food")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Item_Type_Fuel")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Item_Type_Glasses")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Item_Type_Grip")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Item_Type_Grower")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Item_Type_Gun")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Item_Type_Hat")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Item_Type_Magazine")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Item_Type_Mask")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Item_Type_Medical")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Item_Type_Melee")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Item_Type_Optic")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Item_Type_Shirt")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Item_Type_Sight")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Item_Type_Structure")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Item_Type_Supply")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Item_Type_Tactical")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Item_Type_Throwable")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Item_Type_Tool")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Item_Type_Vest")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Item_Type_Water"))
      });
      MenuWorkshopSubmitUI.itemTypeState.positionOffset_X = 5;
      MenuWorkshopSubmitUI.itemTypeState.positionOffset_Y = 300;
      MenuWorkshopSubmitUI.itemTypeState.positionScale_X = 0.5f;
      MenuWorkshopSubmitUI.itemTypeState.sizeOffset_X = 195;
      MenuWorkshopSubmitUI.itemTypeState.sizeOffset_Y = 30;
      MenuWorkshopSubmitUI.container.add((Sleek) MenuWorkshopSubmitUI.itemTypeState);
      MenuWorkshopSubmitUI.itemTypeState.isVisible = false;
      MenuWorkshopSubmitUI.vehicleTypeState = new SleekButtonState(new GUIContent[2]
      {
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Vehicle_Type_Wheels_2")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Vehicle_Type_Wheels_4"))
      });
      MenuWorkshopSubmitUI.vehicleTypeState.positionOffset_X = 5;
      MenuWorkshopSubmitUI.vehicleTypeState.positionOffset_Y = 300;
      MenuWorkshopSubmitUI.vehicleTypeState.positionScale_X = 0.5f;
      MenuWorkshopSubmitUI.vehicleTypeState.sizeOffset_X = 195;
      MenuWorkshopSubmitUI.vehicleTypeState.sizeOffset_Y = 30;
      MenuWorkshopSubmitUI.container.add((Sleek) MenuWorkshopSubmitUI.vehicleTypeState);
      MenuWorkshopSubmitUI.vehicleTypeState.isVisible = false;
      MenuWorkshopSubmitUI.skinTypeState = new SleekButtonState(new GUIContent[35]
      {
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Generic_Pattern")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Ace")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Avenger")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Bluntforce")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Butterfly_Knife")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Cobra")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Colt")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Compound_Bow")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Crossbow")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Desert_Falcon")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Dragonfang")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Eaglefire")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Grizzly")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Hawkhound")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Heartbreaker")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Honeybadger")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Katana")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Kryzkarek")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Machete")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Maplestrike")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Masterkey")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Matamorez")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Military_Knife")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Nykorev")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Peacemaker")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Rocket_Launcher")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Sabertooth")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Schofield")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Shadowstalker")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Snayperskya")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Sportshot")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Timberwolf")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Viper")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Yuri")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Zubeknakov"))
      });
      MenuWorkshopSubmitUI.skinTypeState.positionOffset_X = 5;
      MenuWorkshopSubmitUI.skinTypeState.positionOffset_Y = 300;
      MenuWorkshopSubmitUI.skinTypeState.positionScale_X = 0.5f;
      MenuWorkshopSubmitUI.skinTypeState.sizeOffset_X = 195;
      MenuWorkshopSubmitUI.skinTypeState.sizeOffset_Y = 30;
      MenuWorkshopSubmitUI.container.add((Sleek) MenuWorkshopSubmitUI.skinTypeState);
      MenuWorkshopSubmitUI.skinTypeState.isVisible = false;
      MenuWorkshopSubmitUI.objectTypeState = new SleekButtonState(new GUIContent[4]
      {
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Object_Type_Model")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Object_Type_Resource")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Object_Type_Effect")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Object_Type_Animal"))
      });
      MenuWorkshopSubmitUI.objectTypeState.positionOffset_X = 5;
      MenuWorkshopSubmitUI.objectTypeState.positionOffset_Y = 300;
      MenuWorkshopSubmitUI.objectTypeState.positionScale_X = 0.5f;
      MenuWorkshopSubmitUI.objectTypeState.sizeOffset_X = 195;
      MenuWorkshopSubmitUI.objectTypeState.sizeOffset_Y = 30;
      MenuWorkshopSubmitUI.container.add((Sleek) MenuWorkshopSubmitUI.objectTypeState);
      MenuWorkshopSubmitUI.objectTypeState.isVisible = false;
      MenuWorkshopSubmitUI.visibilityState = new SleekButtonState(new GUIContent[3]
      {
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Public")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Friends_Only")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Private"))
      });
      MenuWorkshopSubmitUI.visibilityState.positionOffset_X = -200;
      MenuWorkshopSubmitUI.visibilityState.positionOffset_Y = 340;
      MenuWorkshopSubmitUI.visibilityState.positionScale_X = 0.5f;
      MenuWorkshopSubmitUI.visibilityState.sizeOffset_X = 200;
      MenuWorkshopSubmitUI.visibilityState.sizeOffset_Y = 30;
      MenuWorkshopSubmitUI.container.add((Sleek) MenuWorkshopSubmitUI.visibilityState);
      MenuWorkshopSubmitUI.forState = new SleekButtonState(new GUIContent[2]
      {
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Community")),
        new GUIContent(MenuWorkshopSubmitUI.localization.format("Review"))
      });
      MenuWorkshopSubmitUI.forState.positionOffset_X = -200;
      MenuWorkshopSubmitUI.forState.positionOffset_Y = 380;
      MenuWorkshopSubmitUI.forState.positionScale_X = 0.5f;
      MenuWorkshopSubmitUI.forState.sizeOffset_X = 200;
      MenuWorkshopSubmitUI.forState.sizeOffset_Y = 30;
      MenuWorkshopSubmitUI.container.add((Sleek) MenuWorkshopSubmitUI.forState);
      MenuWorkshopSubmitUI.createButton = new SleekButtonIcon((Texture2D) bundle.load("Create"));
      MenuWorkshopSubmitUI.createButton.positionOffset_X = -200;
      MenuWorkshopSubmitUI.createButton.positionOffset_Y = 420;
      MenuWorkshopSubmitUI.createButton.positionScale_X = 0.5f;
      MenuWorkshopSubmitUI.createButton.sizeOffset_X = 195;
      MenuWorkshopSubmitUI.createButton.sizeOffset_Y = 30;
      MenuWorkshopSubmitUI.createButton.text = MenuWorkshopSubmitUI.localization.format("Create_Button");
      MenuWorkshopSubmitUI.createButton.tooltip = MenuWorkshopSubmitUI.localization.format("Create_Button_Tooltip");
      MenuWorkshopSubmitUI.createButton.onClickedButton = new ClickedButton(MenuWorkshopSubmitUI.onClickedCreateButton);
      MenuWorkshopSubmitUI.container.add((Sleek) MenuWorkshopSubmitUI.createButton);
      MenuWorkshopSubmitUI.legalButton = new SleekButton();
      MenuWorkshopSubmitUI.legalButton.positionOffset_X = 5;
      MenuWorkshopSubmitUI.legalButton.positionOffset_Y = 420;
      MenuWorkshopSubmitUI.legalButton.positionScale_X = 0.5f;
      MenuWorkshopSubmitUI.legalButton.sizeOffset_X = 195;
      MenuWorkshopSubmitUI.legalButton.sizeOffset_Y = 30;
      MenuWorkshopSubmitUI.legalButton.fontSize = 10;
      MenuWorkshopSubmitUI.legalButton.text = MenuWorkshopSubmitUI.localization.format("Legal_Button");
      MenuWorkshopSubmitUI.legalButton.tooltip = MenuWorkshopSubmitUI.localization.format("Legal_Button_Tooltip");
      MenuWorkshopSubmitUI.legalButton.onClickedButton = new ClickedButton(MenuWorkshopSubmitUI.onClickedLegalButton);
      MenuWorkshopSubmitUI.container.add((Sleek) MenuWorkshopSubmitUI.legalButton);
      MenuWorkshopSubmitUI.publishedBox = new SleekScrollBox();
      MenuWorkshopSubmitUI.publishedBox.positionOffset_X = -200;
      MenuWorkshopSubmitUI.publishedBox.positionOffset_Y = 460;
      MenuWorkshopSubmitUI.publishedBox.positionScale_X = 0.5f;
      MenuWorkshopSubmitUI.publishedBox.sizeOffset_X = 430;
      MenuWorkshopSubmitUI.publishedBox.sizeOffset_Y = -460;
      MenuWorkshopSubmitUI.publishedBox.sizeScale_Y = 1f;
      MenuWorkshopSubmitUI.publishedBox.area = new Rect(0.0f, 0.0f, 5f, 0.0f);
      MenuWorkshopSubmitUI.container.add((Sleek) MenuWorkshopSubmitUI.publishedBox);
      MenuWorkshopSubmitUI.onPublishedAdded();
      bundle.unload();
    }

    public static void open()
    {
      if (MenuWorkshopSubmitUI.active)
      {
        MenuWorkshopSubmitUI.close();
      }
      else
      {
        MenuWorkshopSubmitUI.active = true;
        MenuWorkshopSubmitUI.container.lerpPositionScale(0.0f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
      }
    }

    public static void close()
    {
      if (!MenuWorkshopSubmitUI.active)
        return;
      MenuWorkshopSubmitUI.active = false;
      MenuWorkshopSubmitUI.container.lerpPositionScale(0.0f, 1f, ESleekLerp.EXPONENTIAL, 20f);
    }

    private static void onClickedCreateButton(SleekButton button)
    {
      if (!MenuWorkshopSubmitUI.checkEntered() || !MenuWorkshopSubmitUI.checkValid())
        return;
      Provider.provider.workshopService.prepareUGC(MenuWorkshopSubmitUI.nameField.text, MenuWorkshopSubmitUI.descriptionField.text, MenuWorkshopSubmitUI.pathField.text, MenuWorkshopSubmitUI.previewField.text, MenuWorkshopSubmitUI.changeField.text, (ESteamUGCType) MenuWorkshopSubmitUI.typeState.state, MenuWorkshopSubmitUI.tag, (ESteamUGCVisibility) MenuWorkshopSubmitUI.visibilityState.state);
      Provider.provider.workshopService.createUGC(MenuWorkshopSubmitUI.forState.state == 1);
      MenuWorkshopSubmitUI.resetFields();
    }

    private static void onClickedLegalButton(SleekButton button)
    {
      if (!Provider.provider.browserService.canOpenBrowser)
        MenuUI.alert(MenuWorkshopSubmitUI.localization.format("Overlay"));
      else
        Provider.provider.browserService.open("http://steamcommunity.com/sharedfiles/workshoplegalagreement");
    }

    private static void onClickedPublished(SleekButton button)
    {
      int index = button.positionOffset_Y / 40;
      if (!MenuWorkshopSubmitUI.checkValid())
        return;
      Provider.provider.workshopService.prepareUGC(MenuWorkshopSubmitUI.nameField.text, MenuWorkshopSubmitUI.descriptionField.text, MenuWorkshopSubmitUI.pathField.text, MenuWorkshopSubmitUI.previewField.text, MenuWorkshopSubmitUI.changeField.text, (ESteamUGCType) MenuWorkshopSubmitUI.typeState.state, MenuWorkshopSubmitUI.tag, (ESteamUGCVisibility) MenuWorkshopSubmitUI.visibilityState.state);
      Provider.provider.workshopService.prepareUGC(Provider.provider.workshopService.published[index].id);
      Provider.provider.workshopService.updateUGC();
      MenuWorkshopSubmitUI.resetFields();
    }

    private static void onPublishedAdded()
    {
      for (int index = 0; index < Provider.provider.workshopService.published.Count; ++index)
      {
        SteamPublished steamPublished = Provider.provider.workshopService.published[index];
        SleekButton sleekButton = new SleekButton();
        sleekButton.positionOffset_Y = index * 40;
        sleekButton.sizeOffset_X = -30;
        sleekButton.sizeOffset_Y = 30;
        sleekButton.sizeScale_X = 1f;
        sleekButton.text = steamPublished.name;
        sleekButton.onClickedButton = new ClickedButton(MenuWorkshopSubmitUI.onClickedPublished);
        MenuWorkshopSubmitUI.publishedBox.add((Sleek) sleekButton);
        MenuWorkshopSubmitUI.publishedButtons.Add(sleekButton);
        MenuWorkshopSubmitUI.publishedBox.area = new Rect(0.0f, 0.0f, 5f, (float) (MenuWorkshopSubmitUI.publishedButtons.Count * 40 - 10));
      }
    }

    private static void onPublishedRemoved()
    {
      MenuWorkshopSubmitUI.publishedBox.remove();
      MenuWorkshopSubmitUI.publishedButtons.Clear();
    }

    private static bool checkEntered()
    {
      if (MenuWorkshopSubmitUI.nameField.text.Length == 0)
      {
        MenuUI.alert(MenuWorkshopSubmitUI.localization.format("Alert_Name"));
        return false;
      }
      if (MenuWorkshopSubmitUI.previewField.text.Length != 0)
        return true;
      MenuUI.alert(MenuWorkshopSubmitUI.localization.format("Alert_Preview"));
      return false;
    }

    private static bool checkValid()
    {
      if (MenuWorkshopSubmitUI.pathField.text.Length == 0 || !ReadWrite.folderExists(MenuWorkshopSubmitUI.pathField.text, false))
      {
        MenuUI.alert(MenuWorkshopSubmitUI.localization.format("Alert_Path"));
        return false;
      }
      ESteamUGCType esteamUgcType = (ESteamUGCType) MenuWorkshopSubmitUI.typeState.state;
      if (MenuWorkshopSubmitUI.forState.state == 1)
      {
        if (esteamUgcType != ESteamUGCType.ITEM && esteamUgcType != ESteamUGCType.SKIN)
        {
          MenuUI.alert(MenuWorkshopSubmitUI.localization.format("Alert_Curated"));
          return false;
        }
      }
      else if (esteamUgcType == ESteamUGCType.SKIN)
      {
        MenuUI.alert(MenuWorkshopSubmitUI.localization.format("Alert_Curated"));
        return false;
      }
      bool flag = false;
      if (esteamUgcType == ESteamUGCType.MAP)
      {
        flag = WorkshopTool.checkMapValid(MenuWorkshopSubmitUI.pathField.text, false);
        if (!flag)
          MenuUI.alert(MenuWorkshopSubmitUI.localization.format("Alert_Map"));
      }
      else if (esteamUgcType == ESteamUGCType.LOCALIZATION)
      {
        flag = WorkshopTool.checkLocalizationValid(MenuWorkshopSubmitUI.pathField.text, false);
        if (!flag)
          MenuUI.alert(MenuWorkshopSubmitUI.localization.format("Alert_Localization"));
      }
      else if (esteamUgcType == ESteamUGCType.OBJECT || esteamUgcType == ESteamUGCType.ITEM || (esteamUgcType == ESteamUGCType.VEHICLE || esteamUgcType == ESteamUGCType.SKIN))
      {
        flag = WorkshopTool.checkBundleValid(MenuWorkshopSubmitUI.pathField.text, false);
        if (!flag)
          MenuUI.alert(MenuWorkshopSubmitUI.localization.format("Alert_Object"));
      }
      return flag;
    }

    private static void resetFields()
    {
      MenuWorkshopSubmitUI.nameField.text = string.Empty;
      MenuWorkshopSubmitUI.descriptionField.text = string.Empty;
      MenuWorkshopSubmitUI.pathField.text = string.Empty;
      MenuWorkshopSubmitUI.previewField.text = string.Empty;
      MenuWorkshopSubmitUI.changeField.text = string.Empty;
    }

    private static void onSwappedTypeState(SleekButtonState button, int state)
    {
      ESteamUGCType esteamUgcType = (ESteamUGCType) state;
      MenuWorkshopSubmitUI.mapTypeState.isVisible = esteamUgcType == ESteamUGCType.MAP;
      MenuWorkshopSubmitUI.itemTypeState.isVisible = esteamUgcType == ESteamUGCType.ITEM;
      MenuWorkshopSubmitUI.vehicleTypeState.isVisible = esteamUgcType == ESteamUGCType.VEHICLE;
      MenuWorkshopSubmitUI.skinTypeState.isVisible = esteamUgcType == ESteamUGCType.SKIN;
      MenuWorkshopSubmitUI.objectTypeState.isVisible = esteamUgcType == ESteamUGCType.OBJECT;
    }
  }
}
