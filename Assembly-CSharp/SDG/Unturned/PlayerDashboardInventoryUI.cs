// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.PlayerDashboardInventoryUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class PlayerDashboardInventoryUI
  {
    private static Sleek container;
    private static Local localization;
    public static Bundle icons;
    public static bool active;
    private static SleekBox backdropBox;
    private static bool isDragging;
    private static ItemJar dragJar;
    private static SleekItem dragSource;
    private static SleekItem dragItem;
    private static Vector2 dragOffset;
    private static float lastDrag;
    private static byte dragPage;
    private static byte drag_x;
    private static byte drag_y;
    private static SleekInspect character;
    private static SleekSlider characterSlider;
    private static SleekButtonIcon swapButton;
    private static SleekSlot[] slots;
    private static SleekScrollBox box;
    private static SleekButton[] headers;
    private static SleekItems[] items;
    private static Sleek selectionBox;
    private static SleekBox descriptionBox;
    private static SleekBox qualityBox;
    private static SleekButtonIcon equipButton;
    private static SleekButtonIcon dropButton;
    private static byte _selectedPage;
    private static byte _selected_x;
    private static byte _selected_y;

    public static byte selectedPage
    {
      get
      {
        return PlayerDashboardInventoryUI._selectedPage;
      }
    }

    public static byte selected_x
    {
      get
      {
        return PlayerDashboardInventoryUI._selected_x;
      }
    }

    public static byte selected_y
    {
      get
      {
        return PlayerDashboardInventoryUI._selected_y;
      }
    }

    public PlayerDashboardInventoryUI()
    {
      if (PlayerDashboardInventoryUI.icons != null)
        PlayerDashboardInventoryUI.icons.unload();
      PlayerDashboardInventoryUI.localization = Localization.read("/Player/PlayerDashboardInventory.dat");
      PlayerDashboardInventoryUI.icons = Bundles.getBundle("/Bundles/Textures/Player/Icons/PlayerDashboardInventory/PlayerDashboardInventory.unity3d");
      PlayerDashboardInventoryUI._selectedPage = byte.MaxValue;
      PlayerDashboardInventoryUI._selected_x = byte.MaxValue;
      PlayerDashboardInventoryUI._selected_y = byte.MaxValue;
      PlayerDashboardInventoryUI.container = new Sleek();
      PlayerDashboardInventoryUI.container.positionScale_Y = 1f;
      PlayerDashboardInventoryUI.container.positionOffset_X = 10;
      PlayerDashboardInventoryUI.container.positionOffset_Y = 10;
      PlayerDashboardInventoryUI.container.sizeOffset_X = -20;
      PlayerDashboardInventoryUI.container.sizeOffset_Y = -20;
      PlayerDashboardInventoryUI.container.sizeScale_X = 1f;
      PlayerDashboardInventoryUI.container.sizeScale_Y = 1f;
      PlayerUI.container.add(PlayerDashboardInventoryUI.container);
      PlayerDashboardInventoryUI.active = true;
      PlayerDashboardInventoryUI.backdropBox = new SleekBox();
      PlayerDashboardInventoryUI.backdropBox.positionOffset_Y = 60;
      PlayerDashboardInventoryUI.backdropBox.sizeOffset_Y = -60;
      PlayerDashboardInventoryUI.backdropBox.sizeScale_X = 1f;
      PlayerDashboardInventoryUI.backdropBox.sizeScale_Y = 1f;
      PlayerDashboardInventoryUI.backdropBox.backgroundColor = Palette.COLOR_W;
      PlayerDashboardInventoryUI.backdropBox.backgroundColor.a = 0.5f;
      PlayerDashboardInventoryUI.container.add((Sleek) PlayerDashboardInventoryUI.backdropBox);
      PlayerDashboardInventoryUI.dragItem = new SleekItem();
      PlayerUI.container.add((Sleek) PlayerDashboardInventoryUI.dragItem);
      PlayerDashboardInventoryUI.dragItem.isVisible = false;
      PlayerDashboardInventoryUI.dragOffset = Vector2.zero;
      PlayerDashboardInventoryUI.dragPage = byte.MaxValue;
      PlayerDashboardInventoryUI.drag_x = byte.MaxValue;
      PlayerDashboardInventoryUI.drag_y = byte.MaxValue;
      PlayerDashboardInventoryUI.character = new SleekInspect("RenderTextures/Character");
      PlayerDashboardInventoryUI.character.positionOffset_X = 10;
      PlayerDashboardInventoryUI.character.positionOffset_Y = 10;
      PlayerDashboardInventoryUI.character.sizeOffset_X = 410;
      PlayerDashboardInventoryUI.character.sizeOffset_Y = -220;
      PlayerDashboardInventoryUI.character.sizeScale_Y = 1f;
      PlayerDashboardInventoryUI.backdropBox.add((Sleek) PlayerDashboardInventoryUI.character);
      PlayerDashboardInventoryUI.slots = new SleekSlot[(int) PlayerInventory.SLOTS];
      for (byte newPage = (byte) 0; (int) newPage < PlayerDashboardInventoryUI.slots.Length; ++newPage)
      {
        PlayerDashboardInventoryUI.slots[(int) newPage] = new SleekSlot(newPage);
        PlayerDashboardInventoryUI.slots[(int) newPage].onSelectedItem = new SelectedItem(PlayerDashboardInventoryUI.onSelectedItem);
        PlayerDashboardInventoryUI.slots[(int) newPage].onGrabbedItem = new GrabbedItem(PlayerDashboardInventoryUI.onGrabbedItem);
        PlayerDashboardInventoryUI.slots[(int) newPage].onPlacedItem = new PlacedItem(PlayerDashboardInventoryUI.onPlacedItem);
        PlayerDashboardInventoryUI.backdropBox.add((Sleek) PlayerDashboardInventoryUI.slots[(int) newPage]);
      }
      PlayerDashboardInventoryUI.slots[0].positionOffset_X = 10;
      PlayerDashboardInventoryUI.slots[0].positionOffset_Y = -160;
      PlayerDashboardInventoryUI.slots[0].positionScale_Y = 1f;
      PlayerDashboardInventoryUI.slots[1].positionOffset_X = 270;
      PlayerDashboardInventoryUI.slots[1].positionOffset_Y = -160;
      PlayerDashboardInventoryUI.slots[1].positionScale_Y = 1f;
      PlayerDashboardInventoryUI.slots[1].sizeOffset_X = 150;
      PlayerDashboardInventoryUI.characterSlider = new SleekSlider();
      PlayerDashboardInventoryUI.characterSlider.sizeOffset_Y = 20;
      PlayerDashboardInventoryUI.characterSlider.sizeScale_X = 1f;
      PlayerDashboardInventoryUI.characterSlider.sizeOffset_X = -40;
      PlayerDashboardInventoryUI.characterSlider.positionOffset_X = 40;
      PlayerDashboardInventoryUI.characterSlider.positionOffset_Y = 15;
      PlayerDashboardInventoryUI.characterSlider.positionScale_Y = 1f;
      PlayerDashboardInventoryUI.characterSlider.orientation = ESleekOrientation.HORIZONTAL;
      PlayerDashboardInventoryUI.characterSlider.onDragged = new Dragged(PlayerDashboardInventoryUI.onDraggedCharacterSlider);
      PlayerDashboardInventoryUI.character.add((Sleek) PlayerDashboardInventoryUI.characterSlider);
      PlayerDashboardInventoryUI.swapButton = new SleekButtonIcon((Texture2D) PlayerDashboardInventoryUI.icons.load("Swap"));
      PlayerDashboardInventoryUI.swapButton.positionOffset_Y = 10;
      PlayerDashboardInventoryUI.swapButton.positionScale_Y = 1f;
      PlayerDashboardInventoryUI.swapButton.sizeOffset_X = 30;
      PlayerDashboardInventoryUI.swapButton.sizeOffset_Y = 30;
      PlayerDashboardInventoryUI.swapButton.tooltip = PlayerDashboardInventoryUI.localization.format("Swap_Tooltip");
      PlayerDashboardInventoryUI.swapButton.onClickedButton = new ClickedButton(PlayerDashboardInventoryUI.onClickedSwapButton);
      PlayerDashboardInventoryUI.character.add((Sleek) PlayerDashboardInventoryUI.swapButton);
      PlayerDashboardInventoryUI.box = new SleekScrollBox();
      PlayerDashboardInventoryUI.box.positionOffset_X = 430;
      PlayerDashboardInventoryUI.box.positionOffset_Y = 10;
      PlayerDashboardInventoryUI.box.sizeOffset_X = -440;
      PlayerDashboardInventoryUI.box.sizeOffset_Y = -20;
      PlayerDashboardInventoryUI.box.sizeScale_X = 1f;
      PlayerDashboardInventoryUI.box.sizeScale_Y = 1f;
      PlayerDashboardInventoryUI.box.area = new Rect(0.0f, 0.0f, 5f, 1000f);
      PlayerDashboardInventoryUI.backdropBox.add((Sleek) PlayerDashboardInventoryUI.box);
      PlayerDashboardInventoryUI.headers = new SleekButton[(int) PlayerInventory.PAGES - (int) PlayerInventory.SLOTS + 3];
      for (byte index = (byte) 0; (int) index < PlayerDashboardInventoryUI.headers.Length; ++index)
      {
        PlayerDashboardInventoryUI.headers[(int) index] = new SleekButton();
        PlayerDashboardInventoryUI.headers[(int) index].sizeOffset_X = -30;
        PlayerDashboardInventoryUI.headers[(int) index].sizeOffset_Y = 60;
        PlayerDashboardInventoryUI.headers[(int) index].sizeScale_X = 1f;
        PlayerDashboardInventoryUI.headers[(int) index].fontSize = 14;
        PlayerDashboardInventoryUI.headers[(int) index].onClickedButton = new ClickedButton(PlayerDashboardInventoryUI.onClickedHeader);
        PlayerDashboardInventoryUI.box.add((Sleek) PlayerDashboardInventoryUI.headers[(int) index]);
        PlayerDashboardInventoryUI.headers[(int) index].isVisible = (int) index == 0;
      }
      for (byte index = (byte) 1; (int) index < PlayerDashboardInventoryUI.headers.Length; ++index)
      {
        if ((int) index != (int) PlayerInventory.STORAGE - (int) PlayerInventory.SLOTS)
        {
          SleekImageTexture sleekImageTexture1 = new SleekImageTexture();
          sleekImageTexture1.positionOffset_X = 5;
          sleekImageTexture1.positionScale_Y = 0.5f;
          PlayerDashboardInventoryUI.headers[(int) index].add((Sleek) sleekImageTexture1);
          SleekImageTexture sleekImageTexture2 = new SleekImageTexture();
          sleekImageTexture2.positionOffset_X = -25;
          sleekImageTexture2.positionOffset_Y = -25;
          sleekImageTexture2.positionScale_X = 1f;
          sleekImageTexture2.positionScale_Y = 1f;
          sleekImageTexture2.sizeOffset_X = 20;
          sleekImageTexture2.sizeOffset_Y = 20;
          sleekImageTexture2.texture = (Texture) PlayerDashboardInventoryUI.icons.load("Quality_0");
          PlayerDashboardInventoryUI.headers[(int) index].add((Sleek) sleekImageTexture2);
          SleekLabel sleekLabel = new SleekLabel();
          sleekLabel.positionOffset_X = -110;
          sleekLabel.positionOffset_Y = 10;
          sleekLabel.positionScale_X = 1f;
          sleekLabel.sizeOffset_X = 100;
          sleekLabel.sizeOffset_Y = 40;
          PlayerDashboardInventoryUI.headers[(int) index].add((Sleek) sleekLabel);
        }
      }
      PlayerDashboardInventoryUI.headers[0].text = PlayerDashboardInventoryUI.localization.format("Hands");
      PlayerDashboardInventoryUI.headers[(int) PlayerInventory.STORAGE - (int) PlayerInventory.SLOTS].text = PlayerDashboardInventoryUI.localization.format("Storage");
      PlayerDashboardInventoryUI.onShirtUpdated(Player.player.clothing.shirt, Player.player.clothing.shirtQuality, Player.player.clothing.shirtState);
      PlayerDashboardInventoryUI.onPantsUpdated(Player.player.clothing.pants, Player.player.clothing.pantsQuality, Player.player.clothing.pantsState);
      PlayerDashboardInventoryUI.onBackpackUpdated(Player.player.clothing.backpack, Player.player.clothing.backpackQuality, Player.player.clothing.backpackState);
      PlayerDashboardInventoryUI.onVestUpdated(Player.player.clothing.vest, Player.player.clothing.vestQuality, Player.player.clothing.vestState);
      PlayerDashboardInventoryUI.items = new SleekItems[(int) PlayerInventory.PAGES - (int) PlayerInventory.SLOTS];
      for (byte index = (byte) 0; (int) index < PlayerDashboardInventoryUI.items.Length; ++index)
      {
        PlayerDashboardInventoryUI.items[(int) index] = new SleekItems((byte) ((uint) PlayerInventory.SLOTS + (uint) index));
        PlayerDashboardInventoryUI.items[(int) index].sizeOffset_X = -30;
        PlayerDashboardInventoryUI.items[(int) index].onSelectedItem = new SelectedItem(PlayerDashboardInventoryUI.onSelectedItem);
        PlayerDashboardInventoryUI.items[(int) index].onGrabbedItem = new GrabbedItem(PlayerDashboardInventoryUI.onGrabbedItem);
        PlayerDashboardInventoryUI.items[(int) index].onPlacedItem = new PlacedItem(PlayerDashboardInventoryUI.onPlacedItem);
        PlayerDashboardInventoryUI.box.add((Sleek) PlayerDashboardInventoryUI.items[(int) index]);
      }
      PlayerDashboardInventoryUI.selectionBox = new Sleek();
      PlayerDashboardInventoryUI.selectionBox.sizeOffset_X = 110;
      PlayerDashboardInventoryUI.selectionBox.sizeOffset_Y = 190;
      PlayerDashboardInventoryUI.container.add(PlayerDashboardInventoryUI.selectionBox);
      PlayerDashboardInventoryUI.selectionBox.isVisible = false;
      PlayerDashboardInventoryUI.descriptionBox = new SleekBox();
      PlayerDashboardInventoryUI.descriptionBox.positionOffset_Y = 60;
      PlayerDashboardInventoryUI.descriptionBox.sizeOffset_Y = -100;
      PlayerDashboardInventoryUI.descriptionBox.sizeScale_X = 1f;
      PlayerDashboardInventoryUI.descriptionBox.sizeScale_Y = 1f;
      PlayerDashboardInventoryUI.selectionBox.add((Sleek) PlayerDashboardInventoryUI.descriptionBox);
      PlayerDashboardInventoryUI.qualityBox = new SleekBox();
      PlayerDashboardInventoryUI.qualityBox.positionOffset_Y = -30;
      PlayerDashboardInventoryUI.qualityBox.positionScale_Y = 1f;
      PlayerDashboardInventoryUI.qualityBox.sizeOffset_Y = 30;
      PlayerDashboardInventoryUI.qualityBox.sizeScale_X = 1f;
      PlayerDashboardInventoryUI.qualityBox.isVisible = false;
      PlayerDashboardInventoryUI.selectionBox.add((Sleek) PlayerDashboardInventoryUI.qualityBox);
      PlayerDashboardInventoryUI.equipButton = new SleekButtonIcon((Texture2D) PlayerDashboardInventoryUI.icons.load("Equip"));
      PlayerDashboardInventoryUI.equipButton.sizeOffset_X = 50;
      PlayerDashboardInventoryUI.equipButton.sizeOffset_Y = 50;
      PlayerDashboardInventoryUI.equipButton.onClickedButton = new ClickedButton(PlayerDashboardInventoryUI.onClickedEquip);
      PlayerDashboardInventoryUI.selectionBox.add((Sleek) PlayerDashboardInventoryUI.equipButton);
      PlayerDashboardInventoryUI.dropButton = new SleekButtonIcon((Texture2D) PlayerDashboardInventoryUI.icons.load("Drop"));
      PlayerDashboardInventoryUI.dropButton.positionOffset_X = 60;
      PlayerDashboardInventoryUI.dropButton.sizeOffset_X = 50;
      PlayerDashboardInventoryUI.dropButton.sizeOffset_Y = 50;
      PlayerDashboardInventoryUI.dropButton.tooltip = PlayerDashboardInventoryUI.localization.format("Drop_Button_Tooltip");
      PlayerDashboardInventoryUI.dropButton.onClickedButton = new ClickedButton(PlayerDashboardInventoryUI.onClickedDrop);
      PlayerDashboardInventoryUI.selectionBox.add((Sleek) PlayerDashboardInventoryUI.dropButton);
      Player.player.inventory.onInventoryResized += new InventoryResized(PlayerDashboardInventoryUI.onInventoryResized);
      Player.player.inventory.onInventoryUpdated += new InventoryUpdated(PlayerDashboardInventoryUI.onInventoryUpdated);
      Player.player.inventory.onInventoryAdded += new InventoryAdded(PlayerDashboardInventoryUI.onInventoryAdded);
      Player.player.inventory.onInventoryRemoved += new InventoryRemoved(PlayerDashboardInventoryUI.onInventoryRemoved);
      Player.player.inventory.onInventoryStored += new InventoryStored(PlayerDashboardInventoryUI.onInventoryStored);
      Player.player.clothing.onShirtUpdated += new ShirtUpdated(PlayerDashboardInventoryUI.onShirtUpdated);
      Player.player.clothing.onPantsUpdated += new PantsUpdated(PlayerDashboardInventoryUI.onPantsUpdated);
      Player.player.clothing.onHatUpdated += new HatUpdated(PlayerDashboardInventoryUI.onHatUpdated);
      Player.player.clothing.onBackpackUpdated += new BackpackUpdated(PlayerDashboardInventoryUI.onBackpackUpdated);
      Player.player.clothing.onVestUpdated += new VestUpdated(PlayerDashboardInventoryUI.onVestUpdated);
      Player.player.clothing.onMaskUpdated += new MaskUpdated(PlayerDashboardInventoryUI.onMaskUpdated);
      Player.player.clothing.onGlassesUpdated += new GlassesUpdated(PlayerDashboardInventoryUI.onGlassesUpdated);
      PlayerUI.window.onClickedMouse += new ClickedMouse(PlayerDashboardInventoryUI.onClickedMouse);
      PlayerUI.window.onMovedMouse += new MovedMouse(PlayerDashboardInventoryUI.onMovedMouse);
    }

    public static void open()
    {
      if (PlayerDashboardInventoryUI.active)
      {
        PlayerDashboardInventoryUI.close();
      }
      else
      {
        PlayerDashboardInventoryUI.active = true;
        Player.player.animator.sendGesture(EPlayerGesture.INVENTORY_START, false);
        Player.player.character.FindChild("Camera").gameObject.SetActive(true);
        PlayerDashboardInventoryUI.container.lerpPositionScale(0.0f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
      }
    }

    public static void close()
    {
      if (!PlayerDashboardInventoryUI.active)
        return;
      PlayerDashboardInventoryUI.active = false;
      Player.player.animator.sendGesture(EPlayerGesture.INVENTORY_STOP, false);
      Player.player.character.FindChild("Camera").gameObject.SetActive(false);
      PlayerDashboardInventoryUI.stopDrag();
      PlayerDashboardInventoryUI.closeSelection();
      PlayerDashboardInventoryUI.container.lerpPositionScale(0.0f, 1f, ESleekLerp.EXPONENTIAL, 20f);
    }

    private static void startDrag()
    {
      if (PlayerDashboardInventoryUI.isDragging)
        return;
      PlayerDashboardInventoryUI.isDragging = true;
      PlayerDashboardInventoryUI.dragSource.disable();
      PlayerDashboardInventoryUI.dragItem.isVisible = true;
      SleekRender.allowInput = false;
    }

    private static void stopDrag()
    {
      if (!PlayerDashboardInventoryUI.isDragging)
        return;
      PlayerDashboardInventoryUI.isDragging = false;
      PlayerDashboardInventoryUI.dragSource.enable();
      PlayerDashboardInventoryUI.dragItem.isVisible = false;
      SleekRender.allowInput = true;
    }

    private static void onDraggedCharacterSlider(SleekSlider slider, float state)
    {
      PlayerLook.characterYaw = state * 360f;
    }

    private static void onClickedSwapButton(SleekButton button)
    {
      Player.player.clothing.sendVisual();
    }

    private static void onClickedEquip(SleekButton button)
    {
      if ((int) PlayerDashboardInventoryUI.selectedPage == (int) byte.MaxValue)
        return;
      PlayerDashboardInventoryUI.checkEquip(PlayerDashboardInventoryUI.selectedPage, PlayerDashboardInventoryUI.selected_x, PlayerDashboardInventoryUI.selected_y, Player.player.inventory.getItem(PlayerDashboardInventoryUI.selectedPage, Player.player.inventory.getIndex(PlayerDashboardInventoryUI.selectedPage, PlayerDashboardInventoryUI.selected_x, PlayerDashboardInventoryUI.selected_y)), byte.MaxValue);
      Event.current.Use();
    }

    private static void onClickedDrop(SleekButton button)
    {
      if ((int) PlayerDashboardInventoryUI.selectedPage == (int) byte.MaxValue)
        return;
      Player.player.inventory.sendDropItem(PlayerDashboardInventoryUI.selectedPage, PlayerDashboardInventoryUI.selected_x, PlayerDashboardInventoryUI.selected_y);
      Event.current.Use();
    }

    private static void openSelection(byte page, byte x, byte y)
    {
      PlayerDashboardInventoryUI._selectedPage = page;
      PlayerDashboardInventoryUI._selected_x = x;
      PlayerDashboardInventoryUI._selected_y = y;
      PlayerDashboardInventoryUI.box.isInputable = false;
      for (int index = 0; index < PlayerDashboardInventoryUI.slots.Length; ++index)
        PlayerDashboardInventoryUI.slots[index].isInputable = false;
      PlayerDashboardInventoryUI.selectionBox.isVisible = true;
      PlayerDashboardInventoryUI.selectionBox.positionOffset_X = (int) PlayerUI.window.mouse_x - 10 - 55;
      PlayerDashboardInventoryUI.selectionBox.positionOffset_Y = (int) PlayerUI.window.mouse_y - 10 - 55;
      ItemJar itemJar = Player.player.inventory.getItem(page, Player.player.inventory.getIndex(page, x, y));
      if (itemJar == null)
        return;
      ItemAsset itemAsset = (ItemAsset) Assets.find(EAssetType.ITEM, itemJar.item.id);
      if (itemAsset == null)
        return;
      PlayerDashboardInventoryUI.descriptionBox.text = itemAsset.itemDescription;
      if (itemAsset.showQuality)
      {
        PlayerDashboardInventoryUI.qualityBox.text = (string) (object) itemJar.item.quality + (object) "%";
        PlayerDashboardInventoryUI.qualityBox.foregroundColor = Color.Lerp(Palette.COLOR_R, Palette.COLOR_G, (float) itemJar.item.quality / 100f);
        PlayerDashboardInventoryUI.qualityBox.isVisible = true;
      }
      else
        PlayerDashboardInventoryUI.qualityBox.isVisible = false;
      if (Player.player.equipment.checkSelection(page, x, y))
        PlayerDashboardInventoryUI.equipButton.tooltip = PlayerDashboardInventoryUI.localization.format("Dequip_Button_Tooltip");
      else
        PlayerDashboardInventoryUI.equipButton.tooltip = PlayerDashboardInventoryUI.localization.format("Equip_Button_Tooltip");
      PlayerDashboardInventoryUI.equipButton.isVisible = itemAsset.useable != EUseableType.NONE;
    }

    private static void closeSelection()
    {
      if ((int) PlayerDashboardInventoryUI.selectedPage == (int) byte.MaxValue)
        return;
      PlayerDashboardInventoryUI._selectedPage = byte.MaxValue;
      PlayerDashboardInventoryUI._selected_x = byte.MaxValue;
      PlayerDashboardInventoryUI._selected_y = byte.MaxValue;
      PlayerDashboardInventoryUI.box.isInputable = true;
      for (int index = 0; index < PlayerDashboardInventoryUI.slots.Length; ++index)
        PlayerDashboardInventoryUI.slots[index].isInputable = true;
      PlayerDashboardInventoryUI.selectionBox.isVisible = false;
    }

    private static void onSelectedItem(byte page, byte x, byte y)
    {
      if ((double) Time.realtimeSinceStartup - (double) PlayerDashboardInventoryUI.lastDrag <= 0.5 || PlayerDashboardInventoryUI.isDragging)
        return;
      if ((int) page == (int) byte.MaxValue || (int) page == (int) PlayerDashboardInventoryUI.selectedPage && (int) x == (int) PlayerDashboardInventoryUI.selected_x && (int) y == (int) PlayerDashboardInventoryUI.selected_y)
        PlayerDashboardInventoryUI.closeSelection();
      else if (Input.GetKey(ControlsSettings.other))
      {
        ItemJar jar = Player.player.inventory.getItem(page, Player.player.inventory.getIndex(page, x, y));
        if (Player.player.inventory.isStoring)
        {
          if ((int) page == (int) PlayerInventory.STORAGE)
          {
            byte page1;
            byte x1;
            byte y1;
            if (!Player.player.inventory.tryFindSpace(jar.size_x, jar.size_y, out page1, out x1, out y1))
              return;
            Player.player.inventory.sendDragItem(page, x, y, page1, x1, y1);
          }
          else
          {
            byte x1;
            byte y1;
            if (!Player.player.inventory.tryFindSpace(PlayerInventory.STORAGE, jar.size_x, jar.size_y, out x1, out y1))
              return;
            Player.player.inventory.sendDragItem(page, x, y, PlayerInventory.STORAGE, x1, y1);
          }
        }
        else
          PlayerDashboardInventoryUI.checkAction(page, x, y, jar);
      }
      else
        PlayerDashboardInventoryUI.openSelection(page, x, y);
    }

    private static bool checkSlot(byte page, byte x, byte y, ItemJar jar, byte slot)
    {
      if (Player.player.inventory.checkSpace(slot, byte.MaxValue, byte.MaxValue, (byte) 0, (byte) 0, (byte) 1, (byte) 1, false))
      {
        Player.player.inventory.sendDragItem(page, x, y, slot, (byte) 0, (byte) 0);
        Player.player.equipment.equip(slot, (byte) 0, (byte) 0);
        PlayerDashboardUI.close();
        PlayerLifeUI.open();
        return true;
      }
      ItemJar itemJar = Player.player.inventory.getItem(slot, (byte) 0);
      if (!Player.player.inventory.checkSwap(page, x, y, itemJar.size_x, itemJar.size_y, jar.size_x, jar.size_y))
        return false;
      Player.player.inventory.sendSwapItem(page, x, y, slot, (byte) 0, (byte) 0);
      Player.player.equipment.equip(slot, (byte) 0, (byte) 0);
      PlayerDashboardUI.close();
      PlayerLifeUI.open();
      return true;
    }

    private static void checkEquip(byte page, byte x, byte y, ItemJar jar, byte slot)
    {
      if (!Player.player.equipment.checkSelection(page, x, y))
      {
        ItemAsset itemAsset = (ItemAsset) Assets.find(EAssetType.ITEM, jar.item.id);
        if (itemAsset == null)
          return;
        if (itemAsset.slot == ESlotType.NONE || (int) page < (int) PlayerInventory.SLOTS)
        {
          Player.player.equipment.equip(page, x, y);
          PlayerDashboardUI.close();
          PlayerLifeUI.open();
        }
        else if (itemAsset.slot == ESlotType.PRIMARY)
        {
          if (!PlayerDashboardInventoryUI.checkSlot(page, x, y, jar, (byte) 0))
            ;
        }
        else
        {
          if (itemAsset.slot != ESlotType.SECONDARY)
            return;
          if ((int) slot == (int) byte.MaxValue)
          {
            if (PlayerDashboardInventoryUI.checkSlot(page, x, y, jar, (byte) 1) || PlayerDashboardInventoryUI.checkSlot(page, x, y, jar, (byte) 0))
              ;
          }
          else if (!PlayerDashboardInventoryUI.checkSlot(page, x, y, jar, slot))
            ;
        }
      }
      else
      {
        if (!Player.player.equipment.isSelected || Player.player.equipment.isBusy || !Player.player.equipment.isEquipped)
          return;
        Player.player.equipment.dequip();
        if ((int) page != (int) PlayerDashboardInventoryUI.selectedPage || (int) x != (int) PlayerDashboardInventoryUI.selected_x || (int) y != (int) PlayerDashboardInventoryUI.selected_y)
          return;
        PlayerDashboardInventoryUI.closeSelection();
      }
    }

    private static void checkAction(byte page, byte x, byte y, ItemJar jar)
    {
      ItemAsset itemAsset = (ItemAsset) Assets.find(EAssetType.ITEM, jar.item.id);
      if (itemAsset == null)
        return;
      if (itemAsset.type == EItemType.HAT)
        Player.player.clothing.sendSwapHat(page, x, y);
      else if (itemAsset.type == EItemType.SHIRT)
        Player.player.clothing.sendSwapShirt(page, x, y);
      else if (itemAsset.type == EItemType.PANTS)
        Player.player.clothing.sendSwapPants(page, x, y);
      else if (itemAsset.type == EItemType.BACKPACK)
        Player.player.clothing.sendSwapBackpack(page, x, y);
      else if (itemAsset.type == EItemType.VEST)
        Player.player.clothing.sendSwapVest(page, x, y);
      else if (itemAsset.type == EItemType.MASK)
        Player.player.clothing.sendSwapMask(page, x, y);
      else if (itemAsset.type == EItemType.GLASSES)
      {
        Player.player.clothing.sendSwapGlasses(page, x, y);
      }
      else
      {
        if (itemAsset.useable == EUseableType.NONE)
          return;
        PlayerDashboardInventoryUI.checkEquip(page, x, y, jar, byte.MaxValue);
      }
    }

    private static void onGrabbedItem(byte page, byte x, byte y, SleekItem item)
    {
      if ((double) Time.realtimeSinceStartup - (double) PlayerDashboardInventoryUI.lastDrag <= 0.5 || PlayerDashboardInventoryUI.isDragging)
        return;
      if (Input.GetKey(ControlsSettings.other))
      {
        Player.player.inventory.sendDropItem(page, x, y);
      }
      else
      {
        PlayerDashboardInventoryUI.dragJar = Player.player.inventory.getItem(page, Player.player.inventory.getIndex(page, x, y));
        PlayerDashboardInventoryUI.dragSource = item;
        PlayerDashboardInventoryUI.dragPage = page;
        PlayerDashboardInventoryUI.drag_x = x;
        PlayerDashboardInventoryUI.drag_y = y;
        if ((int) page < (int) PlayerInventory.SLOTS)
        {
          PlayerDashboardInventoryUI.dragOffset.x = item.frame.x - PlayerUI.window.mouse_x;
          PlayerDashboardInventoryUI.dragOffset.y = item.frame.y - PlayerUI.window.mouse_y;
        }
        else
        {
          PlayerDashboardInventoryUI.dragOffset.x = item.frame.x + item.parent.parent.frame.x - ((SleekScrollBox) item.parent.parent).state.x - PlayerUI.window.mouse_x;
          PlayerDashboardInventoryUI.dragOffset.y = item.frame.y + item.parent.parent.frame.y - ((SleekScrollBox) item.parent.parent).state.y - PlayerUI.window.mouse_y;
        }
        PlayerDashboardInventoryUI.dragItem.updateItem(PlayerDashboardInventoryUI.dragJar);
        PlayerDashboardInventoryUI.dragItem.positionOffset_X = (int) ((double) PlayerUI.window.mouse_x + (double) PlayerDashboardInventoryUI.dragOffset.x);
        PlayerDashboardInventoryUI.dragItem.positionOffset_Y = (int) ((double) PlayerUI.window.mouse_y + (double) PlayerDashboardInventoryUI.dragOffset.y);
        PlayerDashboardInventoryUI.startDrag();
      }
    }

    private static void onPlacedItem(byte page, byte x, byte y)
    {
      if (PlayerDashboardInventoryUI.dragSource == null || !PlayerDashboardInventoryUI.isDragging)
        return;
      PlayerDashboardInventoryUI.lastDrag = Time.realtimeSinceStartup;
      if ((int) page >= (int) PlayerInventory.SLOTS)
      {
        int num1 = (int) x + (int) ((double) PlayerDashboardInventoryUI.dragOffset.x / 50.0);
        int num2 = (int) y + (int) ((double) PlayerDashboardInventoryUI.dragOffset.y / 50.0);
        if (num1 < 0)
          num1 = 0;
        if (num2 < 0)
          num2 = 0;
        if (num1 >= (int) Player.player.inventory.getWidth(page) - (int) PlayerDashboardInventoryUI.dragJar.size_x)
          num1 = (int) (byte) ((uint) Player.player.inventory.getWidth(page) - (uint) PlayerDashboardInventoryUI.dragJar.size_x);
        if (num2 >= (int) Player.player.inventory.getHeight(page) - (int) PlayerDashboardInventoryUI.dragJar.size_y)
          num2 = (int) (byte) ((uint) Player.player.inventory.getHeight(page) - (uint) PlayerDashboardInventoryUI.dragJar.size_y);
        x = (byte) num1;
        y = (byte) num2;
      }
      ItemAsset itemAsset1 = (ItemAsset) Assets.find(EAssetType.ITEM, PlayerDashboardInventoryUI.dragJar.item.id);
      if ((int) page < (int) PlayerInventory.SLOTS && (itemAsset1 != null && itemAsset1.slot == ESlotType.NONE || itemAsset1 != null && (int) page == 1 && itemAsset1.slot != ESlotType.SECONDARY))
        return;
      if ((int) PlayerDashboardInventoryUI.dragPage == (int) page && (int) PlayerDashboardInventoryUI.drag_x == (int) x && (int) PlayerDashboardInventoryUI.drag_y == (int) y)
        PlayerDashboardInventoryUI.stopDrag();
      else if (Player.player.inventory.checkSpace(page, PlayerDashboardInventoryUI.drag_x, PlayerDashboardInventoryUI.drag_y, x, y, PlayerDashboardInventoryUI.dragJar.size_x, PlayerDashboardInventoryUI.dragJar.size_y, (int) page == (int) PlayerDashboardInventoryUI.dragPage))
      {
        PlayerDashboardInventoryUI.stopDrag();
        Player.player.inventory.sendDragItem(PlayerDashboardInventoryUI.dragPage, PlayerDashboardInventoryUI.drag_x, PlayerDashboardInventoryUI.drag_y, page, x, y);
        if ((int) page >= (int) PlayerInventory.SLOTS)
          return;
        Player.player.equipment.equip(page, (byte) 0, (byte) 0);
        PlayerDashboardUI.close();
        PlayerLifeUI.open();
      }
      else if ((int) page < (int) PlayerInventory.SLOTS)
      {
        PlayerDashboardInventoryUI.stopDrag();
        PlayerDashboardInventoryUI.checkEquip(PlayerDashboardInventoryUI.dragPage, PlayerDashboardInventoryUI.drag_x, PlayerDashboardInventoryUI.drag_y, PlayerDashboardInventoryUI.dragJar, page);
      }
      else
      {
        byte find_x;
        byte find_y;
        byte index = Player.player.inventory.findIndex(page, x, y, out find_x, out find_y);
        if ((int) index == (int) byte.MaxValue)
          return;
        if ((int) PlayerDashboardInventoryUI.dragPage == (int) page && (int) PlayerDashboardInventoryUI.drag_x == (int) find_x && (int) PlayerDashboardInventoryUI.drag_y == (int) find_y)
        {
          PlayerDashboardInventoryUI.stopDrag();
        }
        else
        {
          ItemJar itemJar = Player.player.inventory.getItem(page, index);
          if (!Player.player.inventory.checkSwap(PlayerDashboardInventoryUI.dragPage, PlayerDashboardInventoryUI.drag_x, PlayerDashboardInventoryUI.drag_y, itemJar.size_x, itemJar.size_y, PlayerDashboardInventoryUI.dragJar.size_x, PlayerDashboardInventoryUI.dragJar.size_y) || !Player.player.inventory.checkSwap(page, x, y, PlayerDashboardInventoryUI.dragJar.size_x, PlayerDashboardInventoryUI.dragJar.size_y, itemJar.size_x, itemJar.size_y))
            return;
          ItemAsset itemAsset2 = (ItemAsset) Assets.find(EAssetType.ITEM, itemJar.item.id);
          if (itemAsset2 == null || itemAsset2.slot != itemAsset1.slot)
            return;
          PlayerDashboardInventoryUI.stopDrag();
          Player.player.inventory.sendSwapItem(page, find_x, find_y, PlayerDashboardInventoryUI.dragPage, PlayerDashboardInventoryUI.drag_x, PlayerDashboardInventoryUI.drag_y);
          if ((int) PlayerDashboardInventoryUI.dragPage >= (int) PlayerInventory.SLOTS)
            return;
          PlayerDashboardInventoryUI.checkEquip(PlayerDashboardInventoryUI.dragPage, PlayerDashboardInventoryUI.drag_x, PlayerDashboardInventoryUI.drag_y, PlayerDashboardInventoryUI.dragJar, page);
        }
      }
    }

    private static void onClickedMouse()
    {
      PlayerDashboardInventoryUI.closeSelection();
      if ((double) Time.realtimeSinceStartup - (double) PlayerDashboardInventoryUI.lastDrag < 0.05)
        return;
      if ((double) PlayerUI.window.mouse_x > (double) Mathf.Max(PlayerDashboardInventoryUI.character.renderImage.frame.xMin, PlayerDashboardInventoryUI.character.frame.xMin) && (double) PlayerUI.window.mouse_x < (double) Mathf.Min(PlayerDashboardInventoryUI.character.renderImage.frame.xMax, PlayerDashboardInventoryUI.character.frame.xMax) && ((double) PlayerUI.window.mouse_y > (double) Mathf.Max(PlayerDashboardInventoryUI.character.renderImage.frame.yMin, PlayerDashboardInventoryUI.character.frame.yMin) && (double) PlayerUI.window.mouse_y < (double) Mathf.Min(PlayerDashboardInventoryUI.character.renderImage.frame.yMax, PlayerDashboardInventoryUI.character.frame.yMax)))
      {
        if (PlayerDashboardInventoryUI.dragSource != null && PlayerDashboardInventoryUI.isDragging)
        {
          PlayerDashboardInventoryUI.lastDrag = Time.realtimeSinceStartup;
          byte page = PlayerDashboardInventoryUI.dragPage;
          byte x = PlayerDashboardInventoryUI.drag_x;
          byte y = PlayerDashboardInventoryUI.drag_y;
          ItemJar jar = PlayerDashboardInventoryUI.dragJar;
          PlayerDashboardInventoryUI.stopDrag();
          PlayerDashboardInventoryUI.checkAction(page, x, y, jar);
        }
        else
        {
          RaycastHit hitInfo;
          Physics.Raycast(Player.player.look.characterCamera.ScreenPointToRay(new Vector3((float) (((double) PlayerUI.window.mouse_x - (double) PlayerDashboardInventoryUI.character.renderImage.frame.xMin) / (double) PlayerDashboardInventoryUI.character.renderImage.frame.width * 1024.0), (float) (((double) PlayerDashboardInventoryUI.character.renderImage.frame.yMax - (double) PlayerUI.window.mouse_y) / (double) PlayerDashboardInventoryUI.character.renderImage.frame.height * 1024.0), 0.0f)), out hitInfo, 8f, RayMasks.CLOTHING_INTERACT);
          if (!((Object) hitInfo.transform != (Object) null))
            return;
          if (hitInfo.transform.tag == "Player")
          {
            switch (DamageTool.getLimb(hitInfo.transform))
            {
              case ELimb.LEFT_FOOT:
              case ELimb.LEFT_LEG:
              case ELimb.RIGHT_FOOT:
              case ELimb.RIGHT_LEG:
                Player.player.clothing.sendSwapPants(byte.MaxValue, byte.MaxValue, byte.MaxValue);
                break;
              case ELimb.LEFT_HAND:
              case ELimb.LEFT_ARM:
              case ELimb.RIGHT_HAND:
              case ELimb.RIGHT_ARM:
              case ELimb.SPINE:
                Player.player.clothing.sendSwapShirt(byte.MaxValue, byte.MaxValue, byte.MaxValue);
                break;
            }
          }
          else if (hitInfo.transform.tag == "Enemy")
          {
            if (hitInfo.transform.name == "Hat")
              Player.player.clothing.sendSwapHat(byte.MaxValue, byte.MaxValue, byte.MaxValue);
            else if (hitInfo.transform.name == "Glasses")
              Player.player.clothing.sendSwapGlasses(byte.MaxValue, byte.MaxValue, byte.MaxValue);
            else if (hitInfo.transform.name == "Mask")
              Player.player.clothing.sendSwapMask(byte.MaxValue, byte.MaxValue, byte.MaxValue);
            else if (hitInfo.transform.name == "Vest")
            {
              Player.player.clothing.sendSwapVest(byte.MaxValue, byte.MaxValue, byte.MaxValue);
            }
            else
            {
              if (!(hitInfo.transform.name == "Backpack"))
                return;
              Player.player.clothing.sendSwapBackpack(byte.MaxValue, byte.MaxValue, byte.MaxValue);
            }
          }
          else
          {
            if (!(hitInfo.transform.tag == "Item"))
              return;
            Player.player.equipment.dequip();
          }
        }
      }
      else
      {
        for (int index = 0; index < PlayerDashboardInventoryUI.slots.Length; ++index)
        {
          if ((double) PlayerUI.window.mouse_x > (double) PlayerDashboardInventoryUI.slots[index].frame.xMin && (double) PlayerUI.window.mouse_x < (double) PlayerDashboardInventoryUI.slots[index].frame.xMax && ((double) PlayerUI.window.mouse_y > (double) PlayerDashboardInventoryUI.slots[index].frame.yMin && (double) PlayerUI.window.mouse_y < (double) PlayerDashboardInventoryUI.slots[index].frame.yMax))
          {
            PlayerDashboardInventoryUI.slots[index].select();
            return;
          }
        }
        if (PlayerDashboardInventoryUI.dragSource == null || !PlayerDashboardInventoryUI.isDragging)
          return;
        PlayerDashboardInventoryUI.lastDrag = Time.realtimeSinceStartup;
        byte page = PlayerDashboardInventoryUI.dragPage;
        byte x = PlayerDashboardInventoryUI.drag_x;
        byte y = PlayerDashboardInventoryUI.drag_y;
        PlayerDashboardInventoryUI.stopDrag();
        Player.player.inventory.sendDropItem(page, x, y);
      }
    }

    private static void onMovedMouse(float x, float y)
    {
      if (!PlayerDashboardInventoryUI.isDragging)
        return;
      PlayerDashboardInventoryUI.dragItem.positionOffset_X = (int) ((double) x + (double) PlayerDashboardInventoryUI.dragOffset.x);
      PlayerDashboardInventoryUI.dragItem.positionOffset_Y = (int) ((double) y + (double) PlayerDashboardInventoryUI.dragOffset.y);
    }

    private static void onInventoryResized(byte page, byte newWidth, byte newHeight)
    {
      if ((int) page < (int) PlayerInventory.SLOTS)
        return;
      page -= PlayerInventory.SLOTS;
      PlayerDashboardInventoryUI.items[(int) page].resize(newWidth, newHeight);
      if ((int) page > 0)
        PlayerDashboardInventoryUI.headers[(int) page].isVisible = (int) newHeight > 0;
      PlayerDashboardInventoryUI.items[(int) page].isVisible = (int) newHeight > 0;
      if ((int) page == (int) PlayerInventory.STORAGE - (int) PlayerInventory.SLOTS && (int) newHeight == 0)
        PlayerDashboardInventoryUI.items[(int) page].clear();
      PlayerDashboardInventoryUI.updateArea();
    }

    private static void updateArea()
    {
      int num1 = 0;
      int num2 = 0;
      for (byte index = (byte) 0; (int) index < PlayerDashboardInventoryUI.items.Length; ++index)
      {
        if (PlayerDashboardInventoryUI.headers[(int) index].isVisible)
        {
          PlayerDashboardInventoryUI.headers[(int) index].positionOffset_Y = num2;
          PlayerDashboardInventoryUI.items[(int) index].positionOffset_Y = num2 + 70;
          if (PlayerDashboardInventoryUI.items[(int) index].sizeOffset_X > num1)
            num1 = PlayerDashboardInventoryUI.items[(int) index].sizeOffset_X;
          num2 += PlayerDashboardInventoryUI.items[(int) index].sizeOffset_Y + 80;
        }
      }
      PlayerDashboardInventoryUI.headers[6].isVisible = (int) Player.player.clothing.hat != 0;
      if (PlayerDashboardInventoryUI.headers[6].isVisible)
      {
        PlayerDashboardInventoryUI.headers[6].positionOffset_Y = num2;
        num2 += 70;
      }
      PlayerDashboardInventoryUI.headers[7].isVisible = (int) Player.player.clothing.mask != 0;
      if (PlayerDashboardInventoryUI.headers[7].isVisible)
      {
        PlayerDashboardInventoryUI.headers[7].positionOffset_Y = num2;
        num2 += 70;
      }
      PlayerDashboardInventoryUI.headers[8].isVisible = (int) Player.player.clothing.glasses != 0;
      if (PlayerDashboardInventoryUI.headers[8].isVisible)
      {
        PlayerDashboardInventoryUI.headers[8].positionOffset_Y = num2;
        num2 += 70;
      }
      PlayerDashboardInventoryUI.box.area = new Rect(0.0f, 0.0f, (float) num1, (float) (num2 - 10));
    }

    public static void hotkey(byte button)
    {
      if ((int) PlayerDashboardInventoryUI.selectedPage == (int) PlayerInventory.SLOTS)
      {
        for (int index1 = 0; index1 < PlayerDashboardInventoryUI.items[0].items.Count; ++index1)
        {
          SleekItem sleekItem1 = PlayerDashboardInventoryUI.items[0].items[index1];
          if ((int) sleekItem1.jar.x == (int) PlayerDashboardInventoryUI.selected_x && (int) sleekItem1.jar.y == (int) PlayerDashboardInventoryUI.selected_y && ItemTool.checkUseable(PlayerInventory.SLOTS, sleekItem1.jar.item.id))
          {
            for (int index2 = 0; index2 < PlayerDashboardInventoryUI.items[0].items.Count; ++index2)
            {
              SleekItem sleekItem2 = PlayerDashboardInventoryUI.items[0].items[index2];
              if (sleekItem2.hotkey == (int) button)
                sleekItem2.updateHotkey(byte.MaxValue);
            }
            sleekItem1.updateHotkey(button);
            PlayerDashboardInventoryUI.closeSelection();
            break;
          }
        }
      }
      else
      {
        for (int index = 0; index < PlayerDashboardInventoryUI.items[0].items.Count; ++index)
        {
          SleekItem sleekItem = PlayerDashboardInventoryUI.items[0].items[index];
          if (sleekItem.hotkey == (int) button)
            sleekItem.updateHotkey(byte.MaxValue);
        }
      }
    }

    public static ItemJar key(byte button)
    {
      for (int index = 0; index < PlayerDashboardInventoryUI.items[0].items.Count; ++index)
      {
        SleekItem sleekItem = PlayerDashboardInventoryUI.items[0].items[index];
        if (sleekItem.hotkey == (int) button)
          return sleekItem.jar;
      }
      return (ItemJar) null;
    }

    private static void onInventoryUpdated(byte page, byte index, ItemJar jar)
    {
      if ((int) page < (int) PlayerInventory.SLOTS)
      {
        PlayerDashboardInventoryUI.slots[(int) page].updateItem(jar);
      }
      else
      {
        page -= PlayerInventory.SLOTS;
        PlayerDashboardInventoryUI.items[(int) page].updateItem(index, jar);
      }
    }

    private static void onInventoryAdded(byte page, byte index, ItemJar jar)
    {
      if ((int) page < (int) PlayerInventory.SLOTS)
      {
        PlayerDashboardInventoryUI.slots[(int) page].applyItem(jar);
      }
      else
      {
        page -= PlayerInventory.SLOTS;
        PlayerDashboardInventoryUI.items[(int) page].addItem(jar);
      }
    }

    private static void onInventoryRemoved(byte page, byte index, ItemJar jar)
    {
      if ((int) page == (int) PlayerDashboardInventoryUI.selectedPage && (int) jar.x == (int) PlayerDashboardInventoryUI.selected_x && (int) jar.y == (int) PlayerDashboardInventoryUI.selected_y)
        PlayerDashboardInventoryUI.closeSelection();
      if ((int) page < (int) PlayerInventory.SLOTS)
      {
        PlayerDashboardInventoryUI.slots[(int) page].applyItem((ItemJar) null);
      }
      else
      {
        page -= PlayerInventory.SLOTS;
        PlayerDashboardInventoryUI.items[(int) page].removeItem(jar);
      }
    }

    private static void onInventoryStored()
    {
      PlayerLifeUI.close();
      PlayerPauseUI.close();
      if (PlayerDashboardUI.active)
      {
        PlayerDashboardCraftingUI.close();
        PlayerDashboardSkillsUI.close();
        PlayerDashboardInformationUI.close();
        PlayerDashboardInventoryUI.open();
      }
      else
      {
        PlayerDashboardInventoryUI.active = true;
        PlayerDashboardCraftingUI.active = false;
        PlayerDashboardSkillsUI.active = false;
        PlayerDashboardInformationUI.active = false;
        PlayerDashboardUI.open();
      }
      PlayerDashboardInventoryUI.box.state = new Vector2(0.0f, (float) PlayerDashboardInventoryUI.headers[(int) PlayerInventory.STORAGE - (int) PlayerInventory.SLOTS].positionOffset_Y);
    }

    private static void onShirtIconReady(Texture2D texture)
    {
      ((SleekImageTexture) PlayerDashboardInventoryUI.headers[3].children[0]).texture = (Texture) texture;
    }

    private static void onShirtUpdated(ushort newShirt, byte newShirtQuality, byte[] newShirtState)
    {
      ItemAsset itemAsset = (ItemAsset) Assets.find(EAssetType.ITEM, newShirt);
      if (itemAsset == null)
        return;
      PlayerDashboardInventoryUI.headers[3].text = itemAsset.itemName;
      PlayerDashboardInventoryUI.headers[3].children[0].sizeOffset_X = (int) itemAsset.size_x * 25;
      PlayerDashboardInventoryUI.headers[3].children[0].sizeOffset_Y = (int) itemAsset.size_y * 25;
      PlayerDashboardInventoryUI.headers[3].children[0].positionOffset_Y = -PlayerDashboardInventoryUI.headers[3].children[0].sizeOffset_Y / 2;
      ItemTool.getIcon(newShirt, newShirtQuality, newShirtState, new ItemIconReady(PlayerDashboardInventoryUI.onShirtIconReady));
      ((SleekLabel) PlayerDashboardInventoryUI.headers[3].children[2]).text = (string) (object) newShirtQuality + (object) "%";
      PlayerDashboardInventoryUI.headers[3].backgroundColor = Color.Lerp(Palette.COLOR_R, Palette.COLOR_G, (float) newShirtQuality / 100f);
      PlayerDashboardInventoryUI.headers[3].foregroundColor = PlayerDashboardInventoryUI.headers[3].backgroundColor;
      PlayerDashboardInventoryUI.headers[3].children[1].backgroundColor = PlayerDashboardInventoryUI.headers[3].backgroundColor;
      PlayerDashboardInventoryUI.headers[3].children[1].foregroundColor = PlayerDashboardInventoryUI.headers[3].backgroundColor;
      PlayerDashboardInventoryUI.headers[3].children[2].backgroundColor = PlayerDashboardInventoryUI.headers[3].backgroundColor;
      PlayerDashboardInventoryUI.headers[3].children[2].foregroundColor = PlayerDashboardInventoryUI.headers[3].backgroundColor;
    }

    private static void onPantsIconReady(Texture2D texture)
    {
      ((SleekImageTexture) PlayerDashboardInventoryUI.headers[4].children[0]).texture = (Texture) texture;
    }

    private static void onPantsUpdated(ushort newPants, byte newPantsQuality, byte[] newPantsState)
    {
      if (PlayerDashboardInventoryUI.headers == null)
        return;
      ItemAsset itemAsset = (ItemAsset) Assets.find(EAssetType.ITEM, newPants);
      if (itemAsset == null)
        return;
      PlayerDashboardInventoryUI.headers[4].text = itemAsset.itemName;
      PlayerDashboardInventoryUI.headers[4].children[0].sizeOffset_X = (int) itemAsset.size_x * 25;
      PlayerDashboardInventoryUI.headers[4].children[0].sizeOffset_Y = (int) itemAsset.size_y * 25;
      PlayerDashboardInventoryUI.headers[4].children[0].positionOffset_Y = -PlayerDashboardInventoryUI.headers[4].children[0].sizeOffset_Y / 2;
      ItemTool.getIcon(newPants, newPantsQuality, newPantsState, new ItemIconReady(PlayerDashboardInventoryUI.onPantsIconReady));
      ((SleekLabel) PlayerDashboardInventoryUI.headers[4].children[2]).text = (string) (object) newPantsQuality + (object) "%";
      PlayerDashboardInventoryUI.headers[4].backgroundColor = Color.Lerp(Palette.COLOR_R, Palette.COLOR_G, (float) newPantsQuality / 100f);
      PlayerDashboardInventoryUI.headers[4].foregroundColor = PlayerDashboardInventoryUI.headers[4].backgroundColor;
      PlayerDashboardInventoryUI.headers[4].children[1].backgroundColor = PlayerDashboardInventoryUI.headers[4].backgroundColor;
      PlayerDashboardInventoryUI.headers[4].children[1].foregroundColor = PlayerDashboardInventoryUI.headers[4].backgroundColor;
      PlayerDashboardInventoryUI.headers[4].children[2].backgroundColor = PlayerDashboardInventoryUI.headers[4].backgroundColor;
      PlayerDashboardInventoryUI.headers[4].children[2].foregroundColor = PlayerDashboardInventoryUI.headers[4].backgroundColor;
    }

    private static void onHatIconReady(Texture2D texture)
    {
      ((SleekImageTexture) PlayerDashboardInventoryUI.headers[6].children[0]).texture = (Texture) texture;
    }

    private static void onHatUpdated(ushort newHat, byte newHatQuality, byte[] newHatState)
    {
      if (PlayerDashboardInventoryUI.headers == null)
        return;
      ItemAsset itemAsset = (ItemAsset) Assets.find(EAssetType.ITEM, newHat);
      if (itemAsset != null)
      {
        PlayerDashboardInventoryUI.headers[6].text = itemAsset.itemName;
        PlayerDashboardInventoryUI.headers[6].children[0].sizeOffset_X = (int) itemAsset.size_x * 25;
        PlayerDashboardInventoryUI.headers[6].children[0].sizeOffset_Y = (int) itemAsset.size_y * 25;
        PlayerDashboardInventoryUI.headers[6].children[0].positionOffset_Y = -PlayerDashboardInventoryUI.headers[6].children[0].sizeOffset_Y / 2;
        ItemTool.getIcon(newHat, newHatQuality, newHatState, new ItemIconReady(PlayerDashboardInventoryUI.onHatIconReady));
        ((SleekLabel) PlayerDashboardInventoryUI.headers[6].children[2]).text = (string) (object) newHatQuality + (object) "%";
        PlayerDashboardInventoryUI.headers[6].backgroundColor = Color.Lerp(Palette.COLOR_R, Palette.COLOR_G, (float) newHatQuality / 100f);
        PlayerDashboardInventoryUI.headers[6].foregroundColor = PlayerDashboardInventoryUI.headers[6].backgroundColor;
        PlayerDashboardInventoryUI.headers[6].children[1].backgroundColor = PlayerDashboardInventoryUI.headers[6].backgroundColor;
        PlayerDashboardInventoryUI.headers[6].children[1].foregroundColor = PlayerDashboardInventoryUI.headers[6].backgroundColor;
        PlayerDashboardInventoryUI.headers[6].children[2].backgroundColor = PlayerDashboardInventoryUI.headers[6].backgroundColor;
        PlayerDashboardInventoryUI.headers[6].children[2].foregroundColor = PlayerDashboardInventoryUI.headers[6].backgroundColor;
      }
      PlayerDashboardInventoryUI.headers[6].isVisible = (int) newHat != 0;
      PlayerDashboardInventoryUI.updateArea();
    }

    private static void onBackpackIconReady(Texture2D texture)
    {
      ((SleekImageTexture) PlayerDashboardInventoryUI.headers[1].children[0]).texture = (Texture) texture;
    }

    private static void onBackpackUpdated(ushort newBackpack, byte newBackpackQuality, byte[] newBackpackState)
    {
      ItemAsset itemAsset = (ItemAsset) Assets.find(EAssetType.ITEM, newBackpack);
      if (itemAsset == null)
        return;
      PlayerDashboardInventoryUI.headers[1].text = itemAsset.itemName;
      PlayerDashboardInventoryUI.headers[1].children[0].sizeOffset_X = (int) itemAsset.size_x * 25;
      PlayerDashboardInventoryUI.headers[1].children[0].sizeOffset_Y = (int) itemAsset.size_y * 25;
      PlayerDashboardInventoryUI.headers[1].children[0].positionOffset_Y = -PlayerDashboardInventoryUI.headers[1].children[0].sizeOffset_Y / 2;
      ItemTool.getIcon(newBackpack, newBackpackQuality, newBackpackState, new ItemIconReady(PlayerDashboardInventoryUI.onBackpackIconReady));
      ((SleekLabel) PlayerDashboardInventoryUI.headers[1].children[2]).text = (string) (object) newBackpackQuality + (object) "%";
      PlayerDashboardInventoryUI.headers[1].backgroundColor = Color.Lerp(Palette.COLOR_R, Palette.COLOR_G, (float) newBackpackQuality / 100f);
      PlayerDashboardInventoryUI.headers[1].foregroundColor = PlayerDashboardInventoryUI.headers[1].backgroundColor;
      PlayerDashboardInventoryUI.headers[1].children[1].backgroundColor = PlayerDashboardInventoryUI.headers[1].backgroundColor;
      PlayerDashboardInventoryUI.headers[1].children[1].foregroundColor = PlayerDashboardInventoryUI.headers[1].backgroundColor;
      PlayerDashboardInventoryUI.headers[1].children[2].backgroundColor = PlayerDashboardInventoryUI.headers[1].backgroundColor;
      PlayerDashboardInventoryUI.headers[1].children[2].foregroundColor = PlayerDashboardInventoryUI.headers[1].backgroundColor;
    }

    private static void onVestIconReady(Texture2D texture)
    {
      ((SleekImageTexture) PlayerDashboardInventoryUI.headers[2].children[0]).texture = (Texture) texture;
    }

    private static void onVestUpdated(ushort newVest, byte newVestQuality, byte[] newVestState)
    {
      ItemAsset itemAsset = (ItemAsset) Assets.find(EAssetType.ITEM, newVest);
      if (itemAsset == null)
        return;
      PlayerDashboardInventoryUI.headers[2].text = itemAsset.itemName;
      PlayerDashboardInventoryUI.headers[2].children[0].sizeOffset_X = (int) itemAsset.size_x * 25;
      PlayerDashboardInventoryUI.headers[2].children[0].sizeOffset_Y = (int) itemAsset.size_y * 25;
      PlayerDashboardInventoryUI.headers[2].children[0].positionOffset_Y = -PlayerDashboardInventoryUI.headers[2].children[0].sizeOffset_Y / 2;
      ItemTool.getIcon(newVest, newVestQuality, newVestState, new ItemIconReady(PlayerDashboardInventoryUI.onVestIconReady));
      ((SleekLabel) PlayerDashboardInventoryUI.headers[2].children[2]).text = (string) (object) newVestQuality + (object) "%";
      PlayerDashboardInventoryUI.headers[2].backgroundColor = Color.Lerp(Palette.COLOR_R, Palette.COLOR_G, (float) newVestQuality / 100f);
      PlayerDashboardInventoryUI.headers[2].foregroundColor = PlayerDashboardInventoryUI.headers[2].backgroundColor;
      PlayerDashboardInventoryUI.headers[2].children[1].backgroundColor = PlayerDashboardInventoryUI.headers[2].backgroundColor;
      PlayerDashboardInventoryUI.headers[2].children[1].foregroundColor = PlayerDashboardInventoryUI.headers[2].backgroundColor;
      PlayerDashboardInventoryUI.headers[2].children[2].backgroundColor = PlayerDashboardInventoryUI.headers[2].backgroundColor;
      PlayerDashboardInventoryUI.headers[2].children[2].foregroundColor = PlayerDashboardInventoryUI.headers[2].backgroundColor;
    }

    private static void onMaskIconReady(Texture2D texture)
    {
      ((SleekImageTexture) PlayerDashboardInventoryUI.headers[7].children[0]).texture = (Texture) texture;
    }

    private static void onMaskUpdated(ushort newMask, byte newMaskQuality, byte[] newMaskState)
    {
      if (PlayerDashboardInventoryUI.headers == null)
        return;
      ItemAsset itemAsset = (ItemAsset) Assets.find(EAssetType.ITEM, newMask);
      if (itemAsset != null)
      {
        PlayerDashboardInventoryUI.headers[7].text = itemAsset.itemName;
        PlayerDashboardInventoryUI.headers[7].children[0].sizeOffset_X = (int) itemAsset.size_x * 25;
        PlayerDashboardInventoryUI.headers[7].children[0].sizeOffset_Y = (int) itemAsset.size_y * 25;
        PlayerDashboardInventoryUI.headers[7].children[0].positionOffset_Y = -PlayerDashboardInventoryUI.headers[7].children[0].sizeOffset_Y / 2;
        ItemTool.getIcon(newMask, newMaskQuality, newMaskState, new ItemIconReady(PlayerDashboardInventoryUI.onMaskIconReady));
        ((SleekLabel) PlayerDashboardInventoryUI.headers[7].children[2]).text = (string) (object) newMaskQuality + (object) "%";
        PlayerDashboardInventoryUI.headers[7].backgroundColor = Color.Lerp(Palette.COLOR_R, Palette.COLOR_G, (float) newMaskQuality / 100f);
        PlayerDashboardInventoryUI.headers[7].foregroundColor = PlayerDashboardInventoryUI.headers[7].backgroundColor;
        PlayerDashboardInventoryUI.headers[7].children[1].backgroundColor = PlayerDashboardInventoryUI.headers[7].backgroundColor;
        PlayerDashboardInventoryUI.headers[7].children[1].foregroundColor = PlayerDashboardInventoryUI.headers[7].backgroundColor;
        PlayerDashboardInventoryUI.headers[7].children[2].backgroundColor = PlayerDashboardInventoryUI.headers[7].backgroundColor;
        PlayerDashboardInventoryUI.headers[7].children[2].foregroundColor = PlayerDashboardInventoryUI.headers[7].backgroundColor;
      }
      PlayerDashboardInventoryUI.headers[7].isVisible = (int) newMask != 0;
      PlayerDashboardInventoryUI.updateArea();
    }

    private static void onGlassesIconReady(Texture2D texture)
    {
      ((SleekImageTexture) PlayerDashboardInventoryUI.headers[8].children[0]).texture = (Texture) texture;
    }

    private static void onGlassesUpdated(ushort newGlasses, byte newGlassesQuality, byte[] newGlassesState)
    {
      if (PlayerDashboardInventoryUI.headers == null)
        return;
      ItemAsset itemAsset = (ItemAsset) Assets.find(EAssetType.ITEM, newGlasses);
      if (itemAsset != null)
      {
        PlayerDashboardInventoryUI.headers[8].text = itemAsset.itemName;
        PlayerDashboardInventoryUI.headers[8].children[0].sizeOffset_X = (int) itemAsset.size_x * 25;
        PlayerDashboardInventoryUI.headers[8].children[0].sizeOffset_Y = (int) itemAsset.size_y * 25;
        PlayerDashboardInventoryUI.headers[8].children[0].positionOffset_Y = -PlayerDashboardInventoryUI.headers[8].children[0].sizeOffset_Y / 2;
        ItemTool.getIcon(newGlasses, newGlassesQuality, newGlassesState, new ItemIconReady(PlayerDashboardInventoryUI.onGlassesIconReady));
        ((SleekLabel) PlayerDashboardInventoryUI.headers[8].children[2]).text = (string) (object) newGlassesQuality + (object) "%";
        PlayerDashboardInventoryUI.headers[8].backgroundColor = Color.Lerp(Palette.COLOR_R, Palette.COLOR_G, (float) newGlassesQuality / 100f);
        PlayerDashboardInventoryUI.headers[8].foregroundColor = PlayerDashboardInventoryUI.headers[8].backgroundColor;
        PlayerDashboardInventoryUI.headers[8].children[1].backgroundColor = PlayerDashboardInventoryUI.headers[8].backgroundColor;
        PlayerDashboardInventoryUI.headers[8].children[1].foregroundColor = PlayerDashboardInventoryUI.headers[8].backgroundColor;
        PlayerDashboardInventoryUI.headers[8].children[2].backgroundColor = PlayerDashboardInventoryUI.headers[8].backgroundColor;
        PlayerDashboardInventoryUI.headers[8].children[2].foregroundColor = PlayerDashboardInventoryUI.headers[8].backgroundColor;
      }
      PlayerDashboardInventoryUI.headers[8].isVisible = (int) newGlasses != 0;
      PlayerDashboardInventoryUI.updateArea();
    }

    private static void onClickedHeader(SleekButton button)
    {
      int index = 0;
      while (index < PlayerDashboardInventoryUI.headers.Length && PlayerDashboardInventoryUI.headers[index] != button)
        ++index;
      switch (index)
      {
        case 0:
          if (!Player.player.equipment.isSelected || Player.player.equipment.isBusy || !Player.player.equipment.isEquipped)
            break;
          Player.player.equipment.dequip();
          break;
        case 1:
          Player.player.clothing.sendSwapBackpack(byte.MaxValue, byte.MaxValue, byte.MaxValue);
          break;
        case 2:
          Player.player.clothing.sendSwapVest(byte.MaxValue, byte.MaxValue, byte.MaxValue);
          break;
        case 3:
          Player.player.clothing.sendSwapShirt(byte.MaxValue, byte.MaxValue, byte.MaxValue);
          break;
        case 4:
          Player.player.clothing.sendSwapPants(byte.MaxValue, byte.MaxValue, byte.MaxValue);
          break;
        case 5:
          PlayerDashboardUI.close();
          PlayerLifeUI.open();
          break;
        case 6:
          Player.player.clothing.sendSwapHat(byte.MaxValue, byte.MaxValue, byte.MaxValue);
          break;
        case 7:
          Player.player.clothing.sendSwapMask(byte.MaxValue, byte.MaxValue, byte.MaxValue);
          break;
        case 8:
          Player.player.clothing.sendSwapGlasses(byte.MaxValue, byte.MaxValue, byte.MaxValue);
          break;
      }
    }
  }
}
