// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.MenuSurvivorsClothingUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using SDG.Provider;
using UnityEngine;

namespace SDG.Unturned
{
  public class MenuSurvivorsClothingUI
  {
    public static Local localization;
    private static Sleek container;
    public static bool active;
    private static Sleek inventory;
    private static SleekInventory[] packageButtons;
    private static SleekBox pageBox;
    private static SleekButtonIcon leftButton;
    private static SleekButtonIcon rightButton;
    private static SleekButtonIcon swapButton;
    private static SleekButtonIcon refreshButton;
    private static SleekSlider characterSlider;
    private static int page;
    private static bool hasLoaded;

    private static int pages
    {
      get
      {
        if (Provider.provider.economyService.inventory.Length == 0)
          return 1;
        return (int) Mathf.Ceil((float) Provider.provider.economyService.inventory.Length / 25f);
      }
    }

    public MenuSurvivorsClothingUI()
    {
      MenuSurvivorsClothingUI.localization = Localization.read("/Menu/Survivors/MenuSurvivorsClothing.dat");
      Bundle bundle = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Survivors/MenuSurvivorsClothing/MenuSurvivorsClothing.unity3d");
      MenuSurvivorsClothingUI.container = new Sleek();
      MenuSurvivorsClothingUI.container.positionOffset_X = 10;
      MenuSurvivorsClothingUI.container.positionOffset_Y = 10;
      MenuSurvivorsClothingUI.container.positionScale_Y = 1f;
      MenuSurvivorsClothingUI.container.sizeOffset_X = -20;
      MenuSurvivorsClothingUI.container.sizeOffset_Y = -20;
      MenuSurvivorsClothingUI.container.sizeScale_X = 1f;
      MenuSurvivorsClothingUI.container.sizeScale_Y = 1f;
      MenuUI.container.add(MenuSurvivorsClothingUI.container);
      MenuSurvivorsClothingUI.active = false;
      MenuSurvivorsClothingUI.page = 0;
      MenuSurvivorsClothingUI.inventory = new Sleek();
      MenuSurvivorsClothingUI.inventory.positionOffset_Y = 5;
      MenuSurvivorsClothingUI.inventory.positionScale_X = 0.5f;
      MenuSurvivorsClothingUI.inventory.sizeScale_X = 0.5f;
      MenuSurvivorsClothingUI.inventory.sizeScale_Y = 1f;
      MenuSurvivorsClothingUI.inventory.sizeOffset_Y = -45;
      MenuSurvivorsClothingUI.inventory.constraint = ESleekConstraint.XY;
      MenuSurvivorsClothingUI.container.add(MenuSurvivorsClothingUI.inventory);
      MenuSurvivorsClothingUI.packageButtons = new SleekInventory[25];
      for (int index = 0; index < MenuSurvivorsClothingUI.packageButtons.Length; ++index)
      {
        SleekInventory sleekInventory = new SleekInventory();
        sleekInventory.positionOffset_X = 5;
        sleekInventory.positionOffset_Y = 5;
        sleekInventory.positionScale_X = (float) (index % 5) * 0.2f;
        sleekInventory.positionScale_Y = (float) Mathf.FloorToInt((float) index / 5f) * 0.2f;
        sleekInventory.sizeOffset_X = -10;
        sleekInventory.sizeOffset_Y = -10;
        sleekInventory.sizeScale_X = 0.2f;
        sleekInventory.sizeScale_Y = 0.2f;
        sleekInventory.onClickedInventory = new ClickedInventory(MenuSurvivorsClothingUI.onClickedInventory);
        MenuSurvivorsClothingUI.inventory.add((Sleek) sleekInventory);
        MenuSurvivorsClothingUI.packageButtons[index] = sleekInventory;
      }
      MenuSurvivorsClothingUI.pageBox = new SleekBox();
      MenuSurvivorsClothingUI.pageBox.positionOffset_X = -145;
      MenuSurvivorsClothingUI.pageBox.positionOffset_Y = 5;
      MenuSurvivorsClothingUI.pageBox.positionScale_X = 1f;
      MenuSurvivorsClothingUI.pageBox.positionScale_Y = 1f;
      MenuSurvivorsClothingUI.pageBox.sizeOffset_X = 100;
      MenuSurvivorsClothingUI.pageBox.sizeOffset_Y = 30;
      MenuSurvivorsClothingUI.pageBox.fontSize = 14;
      MenuSurvivorsClothingUI.inventory.add((Sleek) MenuSurvivorsClothingUI.pageBox);
      MenuSurvivorsClothingUI.leftButton = new SleekButtonIcon((Texture2D) bundle.load("Left"));
      MenuSurvivorsClothingUI.leftButton.positionOffset_X = -185;
      MenuSurvivorsClothingUI.leftButton.positionOffset_Y = 5;
      MenuSurvivorsClothingUI.leftButton.positionScale_X = 1f;
      MenuSurvivorsClothingUI.leftButton.positionScale_Y = 1f;
      MenuSurvivorsClothingUI.leftButton.sizeOffset_X = 30;
      MenuSurvivorsClothingUI.leftButton.sizeOffset_Y = 30;
      MenuSurvivorsClothingUI.leftButton.tooltip = MenuSurvivorsClothingUI.localization.format("Left_Tooltip");
      MenuSurvivorsClothingUI.leftButton.onClickedButton = new ClickedButton(MenuSurvivorsClothingUI.onClickedLeftButton);
      MenuSurvivorsClothingUI.inventory.add((Sleek) MenuSurvivorsClothingUI.leftButton);
      MenuSurvivorsClothingUI.rightButton = new SleekButtonIcon((Texture2D) bundle.load("Right"));
      MenuSurvivorsClothingUI.rightButton.positionOffset_X = -35;
      MenuSurvivorsClothingUI.rightButton.positionOffset_Y = 5;
      MenuSurvivorsClothingUI.rightButton.positionScale_X = 1f;
      MenuSurvivorsClothingUI.rightButton.positionScale_Y = 1f;
      MenuSurvivorsClothingUI.rightButton.sizeOffset_X = 30;
      MenuSurvivorsClothingUI.rightButton.sizeOffset_Y = 30;
      MenuSurvivorsClothingUI.rightButton.tooltip = MenuSurvivorsClothingUI.localization.format("Right_Tooltip");
      MenuSurvivorsClothingUI.rightButton.onClickedButton = new ClickedButton(MenuSurvivorsClothingUI.onClickedRightButton);
      MenuSurvivorsClothingUI.inventory.add((Sleek) MenuSurvivorsClothingUI.rightButton);
      MenuSurvivorsClothingUI.swapButton = new SleekButtonIcon((Texture2D) bundle.load("Swap"));
      MenuSurvivorsClothingUI.swapButton.positionOffset_X = 5;
      MenuSurvivorsClothingUI.swapButton.positionOffset_Y = 5;
      MenuSurvivorsClothingUI.swapButton.positionScale_Y = 1f;
      MenuSurvivorsClothingUI.swapButton.sizeOffset_X = 30;
      MenuSurvivorsClothingUI.swapButton.sizeOffset_Y = 30;
      MenuSurvivorsClothingUI.swapButton.tooltip = MenuSurvivorsClothingUI.localization.format("Swap_Tooltip");
      MenuSurvivorsClothingUI.swapButton.onClickedButton = new ClickedButton(MenuSurvivorsClothingUI.onClickedSwapButton);
      MenuSurvivorsClothingUI.inventory.add((Sleek) MenuSurvivorsClothingUI.swapButton);
      MenuSurvivorsClothingUI.refreshButton = new SleekButtonIcon((Texture2D) bundle.load("Refresh"));
      MenuSurvivorsClothingUI.refreshButton.positionOffset_X = 45;
      MenuSurvivorsClothingUI.refreshButton.positionOffset_Y = 5;
      MenuSurvivorsClothingUI.refreshButton.positionScale_Y = 1f;
      MenuSurvivorsClothingUI.refreshButton.sizeOffset_X = 30;
      MenuSurvivorsClothingUI.refreshButton.sizeOffset_Y = 30;
      MenuSurvivorsClothingUI.refreshButton.tooltip = MenuSurvivorsClothingUI.localization.format("Refresh_Tooltip");
      MenuSurvivorsClothingUI.refreshButton.onClickedButton = new ClickedButton(MenuSurvivorsClothingUI.onClickedRefreshButton);
      MenuSurvivorsClothingUI.inventory.add((Sleek) MenuSurvivorsClothingUI.refreshButton);
      MenuSurvivorsClothingUI.characterSlider = new SleekSlider();
      MenuSurvivorsClothingUI.characterSlider.positionOffset_X = 85;
      MenuSurvivorsClothingUI.characterSlider.positionOffset_Y = 10;
      MenuSurvivorsClothingUI.characterSlider.positionScale_Y = 1f;
      MenuSurvivorsClothingUI.characterSlider.sizeOffset_X = -280;
      MenuSurvivorsClothingUI.characterSlider.sizeOffset_Y = 20;
      MenuSurvivorsClothingUI.characterSlider.sizeScale_X = 1f;
      MenuSurvivorsClothingUI.characterSlider.orientation = ESleekOrientation.HORIZONTAL;
      MenuSurvivorsClothingUI.characterSlider.onDragged = new Dragged(MenuSurvivorsClothingUI.onDraggedCharacterSlider);
      MenuSurvivorsClothingUI.inventory.add((Sleek) MenuSurvivorsClothingUI.characterSlider);
      bundle.unload();
      if (!MenuSurvivorsClothingUI.hasLoaded)
      {
        Provider.provider.economyService.onInventoryRefreshed += new TempSteamworksEconomy.InventoryRefreshed(MenuSurvivorsClothingUI.onInventoryRefreshed);
        Provider.provider.economyService.onInventoryDropped += new TempSteamworksEconomy.InventoryDropped(MenuSurvivorsClothingUI.onInventoryDropped);
      }
      MenuSurvivorsClothingUI.hasLoaded = true;
      MenuSurvivorsClothingUI.updatePage();
      MenuSurvivorsClothingItemUI survivorsClothingItemUi = new MenuSurvivorsClothingItemUI();
      MenuSurvivorsClothingInspectUI clothingInspectUi = new MenuSurvivorsClothingInspectUI();
      MenuSurvivorsClothingBoxUI survivorsClothingBoxUi = new MenuSurvivorsClothingBoxUI();
    }

    public static void open()
    {
      if (MenuSurvivorsClothingUI.active)
      {
        MenuSurvivorsClothingUI.close();
      }
      else
      {
        MenuSurvivorsClothingUI.active = true;
        MenuSurvivorsClothingUI.container.lerpPositionScale(0.0f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
      }
    }

    public static void close()
    {
      if (!MenuSurvivorsClothingUI.active)
        return;
      MenuSurvivorsClothingUI.active = false;
      MenuSurvivorsClothingUI.container.lerpPositionScale(0.0f, 1f, ESleekLerp.EXPONENTIAL, 20f);
    }

    public static void viewPage(int newPage)
    {
      MenuSurvivorsClothingUI.page = newPage;
      MenuSurvivorsClothingUI.updatePage();
    }

    private static void onClickedInventory(SleekInventory button)
    {
      int num1 = MenuSurvivorsClothingUI.packageButtons.Length * MenuSurvivorsClothingUI.page;
      int num2 = MenuSurvivorsClothingUI.inventory.search((Sleek) button);
      if (num1 + num2 >= Provider.provider.economyService.inventory.Length)
        return;
      MenuSurvivorsClothingItemUI.viewItem(Provider.provider.economyService.inventory[num1 + num2].m_iDefinition.m_SteamItemDef, Provider.provider.economyService.inventory[num1 + num2].m_unQuantity, Provider.provider.economyService.inventory[num1 + num2].m_itemId.m_SteamItemInstanceID);
      MenuSurvivorsClothingItemUI.open();
      MenuSurvivorsClothingUI.close();
    }

    private static void onClickedLeftButton(SleekButton button)
    {
      if (MenuSurvivorsClothingUI.page <= 0)
        return;
      MenuSurvivorsClothingUI.viewPage(MenuSurvivorsClothingUI.page - 1);
    }

    private static void onClickedRightButton(SleekButton button)
    {
      if (MenuSurvivorsClothingUI.page >= MenuSurvivorsClothingUI.pages - 1)
        return;
      MenuSurvivorsClothingUI.viewPage(MenuSurvivorsClothingUI.page + 1);
    }

    private static void onClickedSwapButton(SleekButton button)
    {
      Characters.clothes.isVisual = !Characters.clothes.isVisual;
      Characters.apply();
    }

    private static void onClickedRefreshButton(SleekButton button)
    {
      Provider.provider.economyService.refreshInventory();
    }

    private static void onInventoryRefreshed()
    {
      if (MenuSurvivorsClothingUI.page >= MenuSurvivorsClothingUI.pages)
        MenuSurvivorsClothingUI.page = MenuSurvivorsClothingUI.pages - 1;
      MenuSurvivorsClothingUI.updatePage();
    }

    private static void onInventoryDropped(int item, ushort quantity, ulong instance)
    {
      MenuUI.alert(MenuSurvivorsClothingUI.localization.format("Origin_Drop"), item, quantity);
      MenuSurvivorsClothingItemUI.viewItem(item, quantity, instance);
      MenuSurvivorsClothingItemUI.open();
      MenuPauseUI.close();
      MenuTitleUI.close();
      MenuDashboardUI.close();
      MenuPlayUI.close();
      MenuPlaySingleplayerUI.close();
      MenuPlayConnectUI.close();
      MenuPlayServersUI.close();
      MenuSurvivorsUI.close();
      MenuSurvivorsCharacterUI.close();
      MenuSurvivorsAppearanceUI.close();
      MenuSurvivorsClothingUI.close();
      MenuSurvivorsGroupUI.close();
      MenuConfigurationUI.close();
      MenuConfigurationOptionsUI.close();
      MenuConfigurationDisplayUI.close();
      MenuConfigurationGraphicsUI.close();
      MenuConfigurationControlsUI.close();
      MenuWorkshopUI.close();
      MenuWorkshopEditorUI.close();
      MenuWorkshopSubmitUI.close();
    }

    public static void updatePage()
    {
      MenuSurvivorsClothingUI.pageBox.text = MenuSurvivorsClothingUI.localization.format("Page", (object) (MenuSurvivorsClothingUI.page + 1), (object) MenuSurvivorsClothingUI.pages);
      if (MenuSurvivorsClothingUI.packageButtons == null)
        return;
      int num = MenuSurvivorsClothingUI.packageButtons.Length * MenuSurvivorsClothingUI.page;
      for (int index = 0; index < MenuSurvivorsClothingUI.packageButtons.Length; ++index)
      {
        if (num + index < Provider.provider.economyService.inventory.Length)
          MenuSurvivorsClothingUI.packageButtons[index].updateInventory(Provider.provider.economyService.inventory[num + index].m_iDefinition.m_SteamItemDef, Provider.provider.economyService.inventory[num + index].m_unQuantity, true, false);
        else
          MenuSurvivorsClothingUI.packageButtons[index].updateInventory(0, (ushort) 0, false, false);
      }
    }

    private static void onDraggedCharacterSlider(SleekSlider slider, float state)
    {
      Characters.characterYaw = state * 360f;
    }
  }
}
