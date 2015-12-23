// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.MenuSurvivorsClothingBoxUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using SDG.Provider;
using SDG.Provider.Services.Economy;
using SDG.SteamworksProvider.Services.Economy;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
  public class MenuSurvivorsClothingBoxUI
  {
    private static Bundle icons;
    private static Local localization;
    private static Sleek container;
    public static bool active;
    public static bool isUnboxing;
    private static float lastAngle;
    private static float angle;
    private static int lastRotation;
    private static int rotation;
    private static int target;
    private static bool hasLoaded;
    private static int item;
    private static ulong instance;
    private static int drop;
    private static bool isMythical;
    private static ulong got;
    private static ItemBoxAsset boxAsset;
    private static ItemKeyAsset keyAsset;
    private static float size;
    private static Sleek inventory;
    private static SleekBox finalBox;
    private static SleekInventory boxButton;
    private static SleekButtonIcon keyButton;
    private static SleekButtonIcon unboxButton;
    private static SleekInventory[] dropButtons;

    public MenuSurvivorsClothingBoxUI()
    {
      MenuSurvivorsClothingBoxUI.localization = Localization.read("/Menu/Survivors/MenuSurvivorsClothingBox.dat");
      if (MenuSurvivorsClothingBoxUI.icons != null)
        MenuSurvivorsClothingBoxUI.icons.unload();
      MenuSurvivorsClothingBoxUI.icons = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Survivors/MenuSurvivorsClothingBox/MenuSurvivorsClothingBox.unity3d");
      MenuSurvivorsClothingBoxUI.container = new Sleek();
      MenuSurvivorsClothingBoxUI.container.positionOffset_X = 10;
      MenuSurvivorsClothingBoxUI.container.positionOffset_Y = 10;
      MenuSurvivorsClothingBoxUI.container.positionScale_Y = 1f;
      MenuSurvivorsClothingBoxUI.container.sizeOffset_X = -20;
      MenuSurvivorsClothingBoxUI.container.sizeOffset_Y = -20;
      MenuSurvivorsClothingBoxUI.container.sizeScale_X = 1f;
      MenuSurvivorsClothingBoxUI.container.sizeScale_Y = 1f;
      MenuUI.container.add(MenuSurvivorsClothingBoxUI.container);
      MenuSurvivorsClothingBoxUI.active = false;
      MenuSurvivorsClothingBoxUI.inventory = new Sleek();
      MenuSurvivorsClothingBoxUI.inventory.positionScale_X = 0.5f;
      MenuSurvivorsClothingBoxUI.inventory.positionOffset_Y = 10;
      MenuSurvivorsClothingBoxUI.inventory.sizeScale_X = 0.5f;
      MenuSurvivorsClothingBoxUI.inventory.sizeScale_Y = 1f;
      MenuSurvivorsClothingBoxUI.inventory.sizeOffset_Y = -20;
      MenuSurvivorsClothingBoxUI.inventory.constraint = ESleekConstraint.XY;
      MenuSurvivorsClothingBoxUI.container.add(MenuSurvivorsClothingBoxUI.inventory);
      MenuSurvivorsClothingBoxUI.finalBox = new SleekBox();
      MenuSurvivorsClothingBoxUI.finalBox.positionOffset_X = -10;
      MenuSurvivorsClothingBoxUI.finalBox.positionOffset_Y = -10;
      MenuSurvivorsClothingBoxUI.finalBox.sizeOffset_X = 20;
      MenuSurvivorsClothingBoxUI.finalBox.sizeOffset_Y = 20;
      MenuSurvivorsClothingBoxUI.inventory.add((Sleek) MenuSurvivorsClothingBoxUI.finalBox);
      MenuSurvivorsClothingBoxUI.boxButton = new SleekInventory();
      MenuSurvivorsClothingBoxUI.boxButton.positionOffset_Y = -30;
      MenuSurvivorsClothingBoxUI.boxButton.positionScale_X = 0.3f;
      MenuSurvivorsClothingBoxUI.boxButton.positionScale_Y = 0.3f;
      MenuSurvivorsClothingBoxUI.boxButton.sizeScale_X = 0.4f;
      MenuSurvivorsClothingBoxUI.boxButton.sizeScale_Y = 0.4f;
      MenuSurvivorsClothingBoxUI.inventory.add((Sleek) MenuSurvivorsClothingBoxUI.boxButton);
      MenuSurvivorsClothingBoxUI.keyButton = new SleekButtonIcon((Texture2D) null, 40);
      MenuSurvivorsClothingBoxUI.keyButton.positionOffset_Y = -20;
      MenuSurvivorsClothingBoxUI.keyButton.positionScale_X = 0.3f;
      MenuSurvivorsClothingBoxUI.keyButton.positionScale_Y = 0.7f;
      MenuSurvivorsClothingBoxUI.keyButton.sizeOffset_X = -5;
      MenuSurvivorsClothingBoxUI.keyButton.sizeOffset_Y = 50;
      MenuSurvivorsClothingBoxUI.keyButton.sizeScale_X = 0.2f;
      MenuSurvivorsClothingBoxUI.keyButton.text = MenuSurvivorsClothingBoxUI.localization.format("Key_Text");
      MenuSurvivorsClothingBoxUI.keyButton.tooltip = MenuSurvivorsClothingBoxUI.localization.format("Key_Tooltip");
      MenuSurvivorsClothingBoxUI.keyButton.onClickedButton = new ClickedButton(MenuSurvivorsClothingBoxUI.onClickedKeyButton);
      MenuSurvivorsClothingBoxUI.keyButton.fontSize = 14;
      MenuSurvivorsClothingBoxUI.inventory.add((Sleek) MenuSurvivorsClothingBoxUI.keyButton);
      MenuSurvivorsClothingBoxUI.keyButton.isVisible = false;
      MenuSurvivorsClothingBoxUI.unboxButton = new SleekButtonIcon((Texture2D) null);
      MenuSurvivorsClothingBoxUI.unboxButton.positionOffset_X = 5;
      MenuSurvivorsClothingBoxUI.unboxButton.positionOffset_Y = -20;
      MenuSurvivorsClothingBoxUI.unboxButton.positionScale_X = 0.5f;
      MenuSurvivorsClothingBoxUI.unboxButton.positionScale_Y = 0.7f;
      MenuSurvivorsClothingBoxUI.unboxButton.sizeOffset_X = -5;
      MenuSurvivorsClothingBoxUI.unboxButton.sizeOffset_Y = 50;
      MenuSurvivorsClothingBoxUI.unboxButton.sizeScale_X = 0.2f;
      MenuSurvivorsClothingBoxUI.unboxButton.text = MenuSurvivorsClothingBoxUI.localization.format("Unbox_Text");
      MenuSurvivorsClothingBoxUI.unboxButton.tooltip = MenuSurvivorsClothingBoxUI.localization.format("Unbox_Tooltip");
      MenuSurvivorsClothingBoxUI.unboxButton.onClickedButton = new ClickedButton(MenuSurvivorsClothingBoxUI.onClickedUnboxButton);
      MenuSurvivorsClothingBoxUI.unboxButton.fontSize = 14;
      MenuSurvivorsClothingBoxUI.inventory.add((Sleek) MenuSurvivorsClothingBoxUI.unboxButton);
      MenuSurvivorsClothingBoxUI.unboxButton.isVisible = false;
      if (!MenuSurvivorsClothingBoxUI.hasLoaded)
        Provider.provider.economyService.onInventoryExchanged += new TempSteamworksEconomy.InventoryExchanged(MenuSurvivorsClothingBoxUI.onInventoryExchanged);
      MenuSurvivorsClothingBoxUI.hasLoaded = true;
    }

    public static void open()
    {
      if (MenuSurvivorsClothingBoxUI.active)
      {
        MenuSurvivorsClothingBoxUI.close();
      }
      else
      {
        MenuSurvivorsClothingBoxUI.active = true;
        MenuSurvivorsClothingBoxUI.container.lerpPositionScale(0.0f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
      }
    }

    public static void close()
    {
      if (!MenuSurvivorsClothingBoxUI.active)
        return;
      MenuSurvivorsClothingBoxUI.active = false;
      MenuSurvivorsClothingBoxUI.container.lerpPositionScale(0.0f, 1f, ESleekLerp.EXPONENTIAL, 20f);
    }

    public static void viewItem(int newItem, ushort newQuantity, ulong newInstance)
    {
      MenuSurvivorsClothingBoxUI.item = newItem;
      MenuSurvivorsClothingBoxUI.instance = newInstance;
      MenuSurvivorsClothingBoxUI.drop = -1;
      MenuSurvivorsClothingBoxUI.isMythical = false;
      MenuSurvivorsClothingBoxUI.angle = 0.0f;
      MenuSurvivorsClothingBoxUI.lastRotation = 0;
      MenuSurvivorsClothingBoxUI.rotation = 0;
      MenuSurvivorsClothingBoxUI.target = -1;
      MenuSurvivorsClothingBoxUI.keyButton.isVisible = true;
      MenuSurvivorsClothingBoxUI.unboxButton.isVisible = true;
      MenuSurvivorsClothingBoxUI.boxButton.updateInventory(MenuSurvivorsClothingBoxUI.item, newQuantity, false, true);
      MenuSurvivorsClothingBoxUI.boxAsset = (ItemBoxAsset) Assets.find(EAssetType.ITEM, Provider.provider.economyService.getInventoryItemID(MenuSurvivorsClothingBoxUI.item));
      if (MenuSurvivorsClothingBoxUI.boxAsset != null)
      {
        if (MenuSurvivorsClothingBoxUI.boxAsset.destroy == 0)
        {
          MenuSurvivorsClothingBoxUI.keyButton.isVisible = false;
          MenuSurvivorsClothingBoxUI.unboxButton.icon = (Texture2D) null;
          MenuSurvivorsClothingBoxUI.unboxButton.positionOffset_X = 0;
          MenuSurvivorsClothingBoxUI.unboxButton.positionScale_X = 0.3f;
          MenuSurvivorsClothingBoxUI.unboxButton.sizeOffset_X = 0;
          MenuSurvivorsClothingBoxUI.unboxButton.sizeScale_X = 0.4f;
          MenuSurvivorsClothingBoxUI.unboxButton.text = MenuSurvivorsClothingBoxUI.localization.format("Unwrap_Text");
          MenuSurvivorsClothingBoxUI.unboxButton.tooltip = MenuSurvivorsClothingBoxUI.localization.format("Unwrap_Tooltip");
          MenuSurvivorsClothingBoxUI.unboxButton.isVisible = true;
          MenuSurvivorsClothingBoxUI.keyAsset = (ItemKeyAsset) null;
        }
        else
        {
          MenuSurvivorsClothingBoxUI.keyButton.isVisible = true;
          MenuSurvivorsClothingBoxUI.unboxButton.icon = (Texture2D) MenuSurvivorsClothingBoxUI.icons.load("Unbox");
          MenuSurvivorsClothingBoxUI.unboxButton.positionOffset_X = 5;
          MenuSurvivorsClothingBoxUI.unboxButton.positionScale_X = 0.5f;
          MenuSurvivorsClothingBoxUI.unboxButton.sizeOffset_X = -5;
          MenuSurvivorsClothingBoxUI.unboxButton.sizeScale_X = 0.2f;
          MenuSurvivorsClothingBoxUI.unboxButton.text = MenuSurvivorsClothingBoxUI.localization.format("Unbox_Text");
          MenuSurvivorsClothingBoxUI.unboxButton.tooltip = MenuSurvivorsClothingBoxUI.localization.format("Unbox_Tooltip");
          MenuSurvivorsClothingBoxUI.unboxButton.isVisible = true;
          MenuSurvivorsClothingBoxUI.keyAsset = (ItemKeyAsset) Assets.find(EAssetType.ITEM, Provider.provider.economyService.getInventoryItemID(MenuSurvivorsClothingBoxUI.boxAsset.destroy));
          if (MenuSurvivorsClothingBoxUI.keyAsset != null)
            MenuSurvivorsClothingBoxUI.keyButton.icon = (Texture2D) Resources.Load("Economy" + MenuSurvivorsClothingBoxUI.keyAsset.proPath + "/Icon_Small");
        }
        MenuSurvivorsClothingBoxUI.size = (float) (6.28318548202515 / (double) MenuSurvivorsClothingBoxUI.boxAsset.drops.Length / 2.75);
        MenuSurvivorsClothingBoxUI.finalBox.positionScale_Y = (float) (0.5 - (double) MenuSurvivorsClothingBoxUI.size / 2.0);
        MenuSurvivorsClothingBoxUI.finalBox.sizeScale_X = MenuSurvivorsClothingBoxUI.size;
        MenuSurvivorsClothingBoxUI.finalBox.sizeScale_Y = MenuSurvivorsClothingBoxUI.size;
        if (MenuSurvivorsClothingBoxUI.dropButtons != null)
        {
          for (int index = 0; index < MenuSurvivorsClothingBoxUI.dropButtons.Length; ++index)
            MenuSurvivorsClothingBoxUI.inventory.remove((Sleek) MenuSurvivorsClothingBoxUI.dropButtons[index]);
        }
        MenuSurvivorsClothingBoxUI.dropButtons = new SleekInventory[MenuSurvivorsClothingBoxUI.boxAsset.drops.Length];
        for (int index = 0; index < MenuSurvivorsClothingBoxUI.boxAsset.drops.Length; ++index)
        {
          float num = (float) (6.28318548202515 * (double) index / (double) MenuSurvivorsClothingBoxUI.boxAsset.drops.Length + 3.14159274101257);
          SleekInventory sleekInventory = new SleekInventory();
          sleekInventory.positionScale_X = (float) (0.5 + (double) Mathf.Cos(-num) * (0.5 - (double) MenuSurvivorsClothingBoxUI.size / 2.0) - (double) MenuSurvivorsClothingBoxUI.size / 2.0);
          sleekInventory.positionScale_Y = (float) (0.5 + (double) Mathf.Sin(-num) * (0.5 - (double) MenuSurvivorsClothingBoxUI.size / 2.0) - (double) MenuSurvivorsClothingBoxUI.size / 2.0);
          sleekInventory.sizeScale_X = MenuSurvivorsClothingBoxUI.size;
          sleekInventory.sizeScale_Y = MenuSurvivorsClothingBoxUI.size;
          sleekInventory.updateInventory(MenuSurvivorsClothingBoxUI.boxAsset.drops[index], (ushort) 1, false, false);
          MenuSurvivorsClothingBoxUI.inventory.add((Sleek) sleekInventory);
          MenuSurvivorsClothingBoxUI.dropButtons[index] = sleekInventory;
        }
      }
      MenuSurvivorsClothingBoxUI.keyButton.backgroundColor = Provider.provider.economyService.getInventoryColor(MenuSurvivorsClothingBoxUI.item);
      MenuSurvivorsClothingBoxUI.keyButton.foregroundColor = MenuSurvivorsClothingBoxUI.keyButton.backgroundColor;
      MenuSurvivorsClothingBoxUI.unboxButton.backgroundColor = MenuSurvivorsClothingBoxUI.keyButton.backgroundColor;
      MenuSurvivorsClothingBoxUI.unboxButton.foregroundColor = MenuSurvivorsClothingBoxUI.keyButton.backgroundColor;
    }

    private static void onClickedKeyButton(SleekButton button)
    {
      if (!Provider.provider.storeService.canOpenStore)
        MenuUI.alert(MenuSurvivorsClothingBoxUI.localization.format("Overlay"));
      else
        Provider.provider.storeService.open((IEconomyItemDefinition) new SteamworksEconomyItemDefinition((SteamItemDef_t) MenuSurvivorsClothingBoxUI.boxAsset.destroy));
    }

    private static void onClickedUnboxButton(SleekButton button)
    {
      if (MenuSurvivorsClothingBoxUI.boxAsset.destroy == 0)
      {
        Provider.provider.economyService.exchangeInventory(MenuSurvivorsClothingBoxUI.boxAsset.generate, MenuSurvivorsClothingBoxUI.instance);
      }
      else
      {
        ulong inventoryPackage = Provider.provider.economyService.getInventoryPackage(MenuSurvivorsClothingBoxUI.boxAsset.destroy);
        if ((long) inventoryPackage == 0L)
          return;
        Provider.provider.economyService.exchangeInventory(MenuSurvivorsClothingBoxUI.boxAsset.generate, MenuSurvivorsClothingBoxUI.instance, inventoryPackage);
      }
      MenuSurvivorsClothingBoxUI.isUnboxing = true;
      MenuSurvivorsClothingBoxUI.lastAngle = Time.realtimeSinceStartup;
      MenuSurvivorsClothingBoxUI.keyButton.isVisible = false;
      MenuSurvivorsClothingBoxUI.unboxButton.isVisible = false;
    }

    private static void onInventoryExchanged(int newItem, ushort newQuantity, ulong newInstance)
    {
      MenuSurvivorsClothingBoxUI.drop = newItem;
      MenuSurvivorsClothingBoxUI.got = newInstance;
      MenuSurvivorsClothingUI.updatePage();
      int num = 0;
      MenuSurvivorsClothingBoxUI.isMythical = true;
      for (int index = 1; index < MenuSurvivorsClothingBoxUI.boxAsset.drops.Length; ++index)
      {
        if (MenuSurvivorsClothingBoxUI.drop == MenuSurvivorsClothingBoxUI.boxAsset.drops[index])
        {
          num = index;
          MenuSurvivorsClothingBoxUI.isMythical = false;
          break;
        }
      }
      if (MenuSurvivorsClothingBoxUI.rotation < MenuSurvivorsClothingBoxUI.boxAsset.drops.Length * 2)
        MenuSurvivorsClothingBoxUI.target = MenuSurvivorsClothingBoxUI.boxAsset.drops.Length * 3 + num;
      else
        MenuSurvivorsClothingBoxUI.target = ((int) ((double) MenuSurvivorsClothingBoxUI.rotation / (double) MenuSurvivorsClothingBoxUI.boxAsset.drops.Length) + 2) * MenuSurvivorsClothingBoxUI.boxAsset.drops.Length + num;
    }

    public static void update()
    {
      if (!MenuSurvivorsClothingBoxUI.isUnboxing)
        return;
      if (MenuSurvivorsClothingBoxUI.rotation == MenuSurvivorsClothingBoxUI.target)
      {
        if ((double) Time.realtimeSinceStartup - (double) MenuSurvivorsClothingBoxUI.lastAngle <= 0.5)
          return;
        MenuSurvivorsClothingBoxUI.isUnboxing = false;
        if (MenuSurvivorsClothingBoxUI.boxAsset.destroy == 0)
          MenuUI.alert(MenuSurvivorsClothingBoxUI.localization.format("Origin_Unwrap"), MenuSurvivorsClothingBoxUI.drop, (ushort) 1);
        else
          MenuUI.alert(MenuSurvivorsClothingBoxUI.localization.format("Origin_Unbox"), MenuSurvivorsClothingBoxUI.drop, (ushort) 1);
        MenuSurvivorsClothingItemUI.viewItem(MenuSurvivorsClothingBoxUI.drop, (ushort) 1, MenuSurvivorsClothingBoxUI.got);
        MenuSurvivorsClothingItemUI.open();
        MenuSurvivorsClothingBoxUI.close();
        if (MenuSurvivorsClothingBoxUI.isMythical)
          Camera.main.GetComponent<AudioSource>().PlayOneShot((AudioClip) Resources.Load("Economy/Sounds/Mythical"), 0.66f);
        else
          Camera.main.GetComponent<AudioSource>().PlayOneShot((AudioClip) Resources.Load("Economy/Sounds/Unbox"), 0.66f);
      }
      else
      {
        if (MenuSurvivorsClothingBoxUI.rotation < MenuSurvivorsClothingBoxUI.target - MenuSurvivorsClothingBoxUI.boxAsset.drops.Length || MenuSurvivorsClothingBoxUI.target == -1)
        {
          if ((double) MenuSurvivorsClothingBoxUI.angle < 12.5663709640503)
            MenuSurvivorsClothingBoxUI.angle += (Time.realtimeSinceStartup - MenuSurvivorsClothingBoxUI.lastAngle) * MenuSurvivorsClothingBoxUI.size * Mathf.Lerp(80f, 20f, MenuSurvivorsClothingBoxUI.angle / 12.56637f);
          else
            MenuSurvivorsClothingBoxUI.angle += (float) (((double) Time.realtimeSinceStartup - (double) MenuSurvivorsClothingBoxUI.lastAngle) * (double) MenuSurvivorsClothingBoxUI.size * 20.0);
        }
        else
          MenuSurvivorsClothingBoxUI.angle += (float) (((double) Time.realtimeSinceStartup - (double) MenuSurvivorsClothingBoxUI.lastAngle) * (double) Mathf.Max(((float) MenuSurvivorsClothingBoxUI.target - MenuSurvivorsClothingBoxUI.angle / (6.283185f / (float) MenuSurvivorsClothingBoxUI.boxAsset.drops.Length)) / (float) MenuSurvivorsClothingBoxUI.boxAsset.drops.Length, 0.05f) * (double) MenuSurvivorsClothingBoxUI.size * 20.0);
        MenuSurvivorsClothingBoxUI.lastAngle = Time.realtimeSinceStartup;
        MenuSurvivorsClothingBoxUI.rotation = (int) ((double) MenuSurvivorsClothingBoxUI.angle / (6.28318548202515 / (double) MenuSurvivorsClothingBoxUI.boxAsset.drops.Length));
        if (MenuSurvivorsClothingBoxUI.rotation == MenuSurvivorsClothingBoxUI.target)
          MenuSurvivorsClothingBoxUI.angle = (float) MenuSurvivorsClothingBoxUI.rotation * (6.283185f / (float) MenuSurvivorsClothingBoxUI.boxAsset.drops.Length);
        for (int index = 0; index < MenuSurvivorsClothingBoxUI.boxAsset.drops.Length; ++index)
        {
          float num = (float) (6.28318548202515 * (double) index / (double) MenuSurvivorsClothingBoxUI.boxAsset.drops.Length + 3.14159274101257);
          MenuSurvivorsClothingBoxUI.dropButtons[index].positionScale_X = (float) (0.5 + (double) Mathf.Cos(MenuSurvivorsClothingBoxUI.angle - num) * (0.5 - (double) MenuSurvivorsClothingBoxUI.size / 2.0) - (double) MenuSurvivorsClothingBoxUI.size / 2.0);
          MenuSurvivorsClothingBoxUI.dropButtons[index].positionScale_Y = (float) (0.5 + (double) Mathf.Sin(MenuSurvivorsClothingBoxUI.angle - num) * (0.5 - (double) MenuSurvivorsClothingBoxUI.size / 2.0) - (double) MenuSurvivorsClothingBoxUI.size / 2.0);
        }
        if (MenuSurvivorsClothingBoxUI.rotation == MenuSurvivorsClothingBoxUI.lastRotation)
          return;
        MenuSurvivorsClothingBoxUI.lastRotation = MenuSurvivorsClothingBoxUI.rotation;
        MenuSurvivorsClothingBoxUI.boxButton.positionScale_Y = 0.25f;
        MenuSurvivorsClothingBoxUI.boxButton.lerpPositionScale(0.3f, 0.3f, ESleekLerp.EXPONENTIAL, 20f);
        MenuSurvivorsClothingBoxUI.boxButton.updateInventory(MenuSurvivorsClothingBoxUI.boxAsset.drops[MenuSurvivorsClothingBoxUI.rotation % MenuSurvivorsClothingBoxUI.boxAsset.drops.Length], (ushort) 1, false, true);
        if (MenuSurvivorsClothingBoxUI.rotation == MenuSurvivorsClothingBoxUI.target)
          Camera.main.GetComponent<AudioSource>().PlayOneShot((AudioClip) Resources.Load("Economy/Sounds/Drop"), 0.33f);
        else
          Camera.main.GetComponent<AudioSource>().PlayOneShot((AudioClip) Resources.Load("Economy/Sounds/Tick"), 0.33f);
      }
    }
  }
}
