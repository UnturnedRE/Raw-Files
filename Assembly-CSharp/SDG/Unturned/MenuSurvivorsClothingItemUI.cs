// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.MenuSurvivorsClothingItemUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
  public class MenuSurvivorsClothingItemUI
  {
    private static Local localization;
    private static Sleek container;
    public static bool active;
    private static int item;
    private static ushort quantity;
    private static ulong instance;
    private static Sleek inventory;
    private static SleekInventory packageBox;
    private static SleekBox descriptionBox;
    private static SleekLabel infoLabel;
    private static SleekButton useButton;
    private static SleekButton inspectButton;
    private static SleekButton marketButton;
    private static SleekButton deleteButton;

    public MenuSurvivorsClothingItemUI()
    {
      MenuSurvivorsClothingItemUI.localization = Localization.read("/Menu/Survivors/MenuSurvivorsClothingItem.dat");
      MenuSurvivorsClothingItemUI.container = new Sleek();
      MenuSurvivorsClothingItemUI.container.positionOffset_X = 10;
      MenuSurvivorsClothingItemUI.container.positionOffset_Y = 10;
      MenuSurvivorsClothingItemUI.container.positionScale_Y = 1f;
      MenuSurvivorsClothingItemUI.container.sizeOffset_X = -20;
      MenuSurvivorsClothingItemUI.container.sizeOffset_Y = -20;
      MenuSurvivorsClothingItemUI.container.sizeScale_X = 1f;
      MenuSurvivorsClothingItemUI.container.sizeScale_Y = 1f;
      MenuUI.container.add(MenuSurvivorsClothingItemUI.container);
      MenuSurvivorsClothingItemUI.active = false;
      MenuSurvivorsClothingItemUI.inventory = new Sleek();
      MenuSurvivorsClothingItemUI.inventory.positionScale_X = 0.5f;
      MenuSurvivorsClothingItemUI.inventory.positionOffset_Y = 10;
      MenuSurvivorsClothingItemUI.inventory.sizeScale_X = 0.5f;
      MenuSurvivorsClothingItemUI.inventory.sizeScale_Y = 1f;
      MenuSurvivorsClothingItemUI.inventory.sizeOffset_Y = -20;
      MenuSurvivorsClothingItemUI.inventory.constraint = ESleekConstraint.XY;
      MenuSurvivorsClothingItemUI.container.add(MenuSurvivorsClothingItemUI.inventory);
      MenuSurvivorsClothingItemUI.packageBox = new SleekInventory();
      MenuSurvivorsClothingItemUI.packageBox.sizeScale_X = 1f;
      MenuSurvivorsClothingItemUI.packageBox.sizeScale_Y = 0.5f;
      MenuSurvivorsClothingItemUI.packageBox.sizeOffset_Y = -5;
      MenuSurvivorsClothingItemUI.packageBox.constraint = ESleekConstraint.XY;
      MenuSurvivorsClothingItemUI.inventory.add((Sleek) MenuSurvivorsClothingItemUI.packageBox);
      MenuSurvivorsClothingItemUI.descriptionBox = new SleekBox();
      MenuSurvivorsClothingItemUI.descriptionBox.positionOffset_Y = 10;
      MenuSurvivorsClothingItemUI.descriptionBox.positionScale_Y = 1f;
      MenuSurvivorsClothingItemUI.descriptionBox.sizeScale_X = 1f;
      MenuSurvivorsClothingItemUI.descriptionBox.sizeScale_Y = 1f;
      MenuSurvivorsClothingItemUI.packageBox.add((Sleek) MenuSurvivorsClothingItemUI.descriptionBox);
      MenuSurvivorsClothingItemUI.infoLabel = new SleekLabel();
      MenuSurvivorsClothingItemUI.infoLabel.positionOffset_X = 5;
      MenuSurvivorsClothingItemUI.infoLabel.positionOffset_Y = 5;
      MenuSurvivorsClothingItemUI.infoLabel.sizeScale_X = 1f;
      MenuSurvivorsClothingItemUI.infoLabel.sizeScale_Y = 1f;
      MenuSurvivorsClothingItemUI.infoLabel.sizeOffset_X = -10;
      MenuSurvivorsClothingItemUI.infoLabel.sizeOffset_Y = -10;
      MenuSurvivorsClothingItemUI.infoLabel.fontAlignment = TextAnchor.UpperLeft;
      MenuSurvivorsClothingItemUI.infoLabel.isRich = true;
      MenuSurvivorsClothingItemUI.descriptionBox.add((Sleek) MenuSurvivorsClothingItemUI.infoLabel);
      MenuSurvivorsClothingItemUI.useButton = new SleekButton();
      MenuSurvivorsClothingItemUI.useButton.positionScale_Y = 1f;
      MenuSurvivorsClothingItemUI.useButton.sizeOffset_Y = 50;
      MenuSurvivorsClothingItemUI.useButton.sizeScale_X = 1f;
      MenuSurvivorsClothingItemUI.useButton.onClickedButton = new ClickedButton(MenuSurvivorsClothingItemUI.onClickedUseButton);
      MenuSurvivorsClothingItemUI.descriptionBox.add((Sleek) MenuSurvivorsClothingItemUI.useButton);
      MenuSurvivorsClothingItemUI.useButton.fontSize = 14;
      MenuSurvivorsClothingItemUI.useButton.isVisible = false;
      MenuSurvivorsClothingItemUI.inspectButton = new SleekButton();
      MenuSurvivorsClothingItemUI.inspectButton.positionScale_Y = 1f;
      MenuSurvivorsClothingItemUI.inspectButton.sizeOffset_Y = 50;
      MenuSurvivorsClothingItemUI.inspectButton.sizeScale_X = 1f;
      MenuSurvivorsClothingItemUI.inspectButton.text = MenuSurvivorsClothingItemUI.localization.format("Inspect_Text");
      MenuSurvivorsClothingItemUI.inspectButton.tooltip = MenuSurvivorsClothingItemUI.localization.format("Inspect_Tooltip");
      MenuSurvivorsClothingItemUI.inspectButton.onClickedButton = new ClickedButton(MenuSurvivorsClothingItemUI.onClickedInspectButton);
      MenuSurvivorsClothingItemUI.descriptionBox.add((Sleek) MenuSurvivorsClothingItemUI.inspectButton);
      MenuSurvivorsClothingItemUI.inspectButton.fontSize = 14;
      MenuSurvivorsClothingItemUI.inspectButton.isVisible = false;
      MenuSurvivorsClothingItemUI.marketButton = new SleekButton();
      MenuSurvivorsClothingItemUI.marketButton.positionScale_Y = 1f;
      MenuSurvivorsClothingItemUI.marketButton.sizeOffset_Y = 50;
      MenuSurvivorsClothingItemUI.marketButton.sizeScale_X = 1f;
      MenuSurvivorsClothingItemUI.marketButton.text = MenuSurvivorsClothingItemUI.localization.format("Market_Text");
      MenuSurvivorsClothingItemUI.marketButton.tooltip = MenuSurvivorsClothingItemUI.localization.format("Market_Tooltip");
      MenuSurvivorsClothingItemUI.marketButton.onClickedButton = new ClickedButton(MenuSurvivorsClothingItemUI.onClickedMarketButton);
      MenuSurvivorsClothingItemUI.descriptionBox.add((Sleek) MenuSurvivorsClothingItemUI.marketButton);
      MenuSurvivorsClothingItemUI.marketButton.fontSize = 14;
      MenuSurvivorsClothingItemUI.marketButton.isVisible = false;
      MenuSurvivorsClothingItemUI.deleteButton = new SleekButton();
      MenuSurvivorsClothingItemUI.deleteButton.positionScale_Y = 1f;
      MenuSurvivorsClothingItemUI.deleteButton.sizeOffset_Y = 50;
      MenuSurvivorsClothingItemUI.deleteButton.sizeScale_X = 1f;
      MenuSurvivorsClothingItemUI.deleteButton.text = "DELETE";
      MenuSurvivorsClothingItemUI.deleteButton.tooltip = "DO NOT CLICK";
      MenuSurvivorsClothingItemUI.deleteButton.onClickedButton = new ClickedButton(MenuSurvivorsClothingItemUI.onClickedDeleteButton);
      MenuSurvivorsClothingItemUI.descriptionBox.add((Sleek) MenuSurvivorsClothingItemUI.deleteButton);
      MenuSurvivorsClothingItemUI.deleteButton.fontSize = 14;
      MenuSurvivorsClothingItemUI.deleteButton.isVisible = false;
    }

    public static void open()
    {
      if (MenuSurvivorsClothingItemUI.active)
      {
        MenuSurvivorsClothingItemUI.close();
      }
      else
      {
        MenuSurvivorsClothingItemUI.active = true;
        MenuSurvivorsClothingItemUI.container.lerpPositionScale(0.0f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
      }
    }

    public static void close()
    {
      if (!MenuSurvivorsClothingItemUI.active)
        return;
      MenuSurvivorsClothingItemUI.active = false;
      MenuSurvivorsClothingItemUI.container.lerpPositionScale(0.0f, 1f, ESleekLerp.EXPONENTIAL, 20f);
    }

    public static void viewItem(int newItem, ushort newQuantity, ulong newInstance)
    {
      MenuSurvivorsClothingItemUI.item = newItem;
      MenuSurvivorsClothingItemUI.quantity = newQuantity;
      MenuSurvivorsClothingItemUI.instance = newInstance;
      MenuSurvivorsClothingItemUI.packageBox.updateInventory(MenuSurvivorsClothingItemUI.item, newQuantity, false, true);
      if (MenuSurvivorsClothingItemUI.packageBox.itemAsset != null)
      {
        if (MenuSurvivorsClothingItemUI.packageBox.itemAsset.type == EItemType.KEY)
        {
          MenuSurvivorsClothingItemUI.useButton.isVisible = false;
          MenuSurvivorsClothingItemUI.inspectButton.isVisible = false;
        }
        else if (MenuSurvivorsClothingItemUI.packageBox.itemAsset.type == EItemType.BOX)
        {
          MenuSurvivorsClothingItemUI.useButton.isVisible = true;
          MenuSurvivorsClothingItemUI.inspectButton.isVisible = false;
          MenuSurvivorsClothingItemUI.useButton.text = MenuSurvivorsClothingItemUI.localization.format("Contents_Text");
          MenuSurvivorsClothingItemUI.useButton.tooltip = MenuSurvivorsClothingItemUI.localization.format("Contents_Tooltip");
        }
        else
        {
          MenuSurvivorsClothingItemUI.useButton.isVisible = true;
          MenuSurvivorsClothingItemUI.inspectButton.isVisible = true;
          bool flag = MenuSurvivorsClothingItemUI.packageBox.itemAsset.type == EItemType.GUN || MenuSurvivorsClothingItemUI.packageBox.itemAsset.type == EItemType.MELEE ? Characters.packageSkins.IndexOf(MenuSurvivorsClothingItemUI.instance) != -1 : (long) Characters.active.packageBackpack == (long) MenuSurvivorsClothingItemUI.instance || (long) Characters.active.packageGlasses == (long) MenuSurvivorsClothingItemUI.instance || ((long) Characters.active.packageHat == (long) MenuSurvivorsClothingItemUI.instance || (long) Characters.active.packageMask == (long) MenuSurvivorsClothingItemUI.instance) || ((long) Characters.active.packagePants == (long) MenuSurvivorsClothingItemUI.instance || (long) Characters.active.packageShirt == (long) MenuSurvivorsClothingItemUI.instance) || (long) Characters.active.packageVest == (long) MenuSurvivorsClothingItemUI.instance;
          MenuSurvivorsClothingItemUI.useButton.text = MenuSurvivorsClothingItemUI.localization.format(!flag ? "Equip_Text" : "Dequip_Text");
          MenuSurvivorsClothingItemUI.useButton.tooltip = MenuSurvivorsClothingItemUI.localization.format(!flag ? "Equip_Tooltip" : "Dequip_Tooltip");
        }
        MenuSurvivorsClothingItemUI.marketButton.isVisible = Provider.provider.economyService.getInventoryMarketable(MenuSurvivorsClothingItemUI.item);
        MenuSurvivorsClothingItemUI.descriptionBox.sizeOffset_Y = 0;
        if (MenuSurvivorsClothingItemUI.useButton.isVisible)
        {
          MenuSurvivorsClothingItemUI.descriptionBox.sizeOffset_Y -= 60;
          MenuSurvivorsClothingItemUI.useButton.positionOffset_Y = -MenuSurvivorsClothingItemUI.descriptionBox.sizeOffset_Y - 50;
        }
        if (MenuSurvivorsClothingItemUI.inspectButton.isVisible)
        {
          MenuSurvivorsClothingItemUI.descriptionBox.sizeOffset_Y -= 60;
          MenuSurvivorsClothingItemUI.inspectButton.positionOffset_Y = -MenuSurvivorsClothingItemUI.descriptionBox.sizeOffset_Y - 50;
        }
        if (MenuSurvivorsClothingItemUI.marketButton.isVisible)
        {
          MenuSurvivorsClothingItemUI.descriptionBox.sizeOffset_Y -= 60;
          MenuSurvivorsClothingItemUI.marketButton.positionOffset_Y = -MenuSurvivorsClothingItemUI.descriptionBox.sizeOffset_Y - 50;
        }
        MenuSurvivorsClothingItemUI.infoLabel.text = Provider.provider.economyService.getInventoryDescription(MenuSurvivorsClothingItemUI.item);
      }
      else
      {
        MenuSurvivorsClothingItemUI.useButton.isVisible = false;
        MenuSurvivorsClothingItemUI.inspectButton.isVisible = false;
        MenuSurvivorsClothingItemUI.marketButton.isVisible = false;
        MenuSurvivorsClothingItemUI.descriptionBox.sizeOffset_Y = 0;
        MenuSurvivorsClothingItemUI.infoLabel.text = "Either your game is out of date or something went horribly wrong! If it's the second of those two please let Nelson know.";
      }
    }

    private static void onClickedUseButton(SleekButton button)
    {
      if (MenuSurvivorsClothingItemUI.packageBox.itemAsset == null)
        return;
      if (MenuSurvivorsClothingItemUI.packageBox.itemAsset.type == EItemType.BOX)
      {
        MenuSurvivorsClothingBoxUI.viewItem(MenuSurvivorsClothingItemUI.item, MenuSurvivorsClothingItemUI.quantity, MenuSurvivorsClothingItemUI.instance);
        MenuSurvivorsClothingBoxUI.open();
        MenuSurvivorsClothingItemUI.close();
      }
      else
      {
        Characters.package(MenuSurvivorsClothingItemUI.instance);
        MenuSurvivorsClothingItemUI.viewItem(MenuSurvivorsClothingItemUI.item, MenuSurvivorsClothingItemUI.quantity, MenuSurvivorsClothingItemUI.instance);
      }
    }

    private static void onClickedInspectButton(SleekButton button)
    {
      MenuSurvivorsClothingInspectUI.viewItem(MenuSurvivorsClothingItemUI.item, MenuSurvivorsClothingItemUI.instance);
      MenuSurvivorsClothingInspectUI.open();
      MenuSurvivorsClothingItemUI.close();
    }

    private static void onClickedMarketButton(SleekButton button)
    {
      if (!Provider.provider.economyService.canOpenInventory)
        MenuUI.alert(MenuSurvivorsClothingItemUI.localization.format("Overlay"));
      else
        Provider.provider.economyService.open(MenuSurvivorsClothingItemUI.instance);
    }

    private static void onClickedDeleteButton(SleekButton button)
    {
      SteamInventoryResult_t pResultHandle;
      SteamInventory.ConsumeItem(out pResultHandle, (SteamItemInstanceID_t) MenuSurvivorsClothingItemUI.instance, 1U);
    }
  }
}
