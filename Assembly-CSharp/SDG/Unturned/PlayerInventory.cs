// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.PlayerInventory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
  public class PlayerInventory : PlayerCaller
  {
    public static readonly ushort[] LOADOUT = new ushort[0];
    public static readonly ushort[] HORDE = new ushort[4]
    {
      (ushort) 97,
      (ushort) 98,
      (ushort) 98,
      (ushort) 98
    };
    public static readonly byte SAVEDATA_VERSION = (byte) 4;
    public static readonly byte SLOTS = (byte) 2;
    public static readonly byte PAGES = (byte) 8;
    public static readonly byte BACKPACK = (byte) 3;
    public static readonly byte VEST = (byte) 4;
    public static readonly byte SHIRT = (byte) 5;
    public static readonly byte PANTS = (byte) 6;
    public static readonly byte STORAGE = (byte) 7;
    public static ushort[] loadout = PlayerInventory.LOADOUT;
    private Items[] items;
    public bool isStoring;
    public InteractableStorage storage;
    public InventoryResized onInventoryResized;
    public InventoryUpdated onInventoryUpdated;
    public InventoryAdded onInventoryAdded;
    public InventoryRemoved onInventoryRemoved;
    public InventoryStored onInventoryStored;

    public byte getWidth(byte page)
    {
      return this.items[(int) page].width;
    }

    public byte getHeight(byte page)
    {
      return this.items[(int) page].height;
    }

    public byte getItemCount(byte page)
    {
      return this.items[(int) page].getItemCount();
    }

    public ItemJar getItem(byte page, byte index)
    {
      return this.items[(int) page].getItem(index);
    }

    public byte getIndex(byte page, byte x, byte y)
    {
      return this.items[(int) page].getIndex(x, y);
    }

    public byte findIndex(byte page, byte x, byte y, out byte find_x, out byte find_y)
    {
      find_x = byte.MaxValue;
      find_y = byte.MaxValue;
      return this.items[(int) page].findIndex(x, y, out find_x, out find_y);
    }

    public void updateAmount(byte page, byte index, byte newAmount)
    {
      if ((int) page < 0 || (int) page >= (int) PlayerInventory.PAGES)
        return;
      this.items[(int) page].updateAmount(index, newAmount);
    }

    public void updateQuality(byte page, byte index, byte newQuality)
    {
      if ((int) page < 0 || (int) page >= (int) PlayerInventory.PAGES)
        return;
      this.items[(int) page].updateQuality(index, newQuality);
      ItemJar itemJar = this.items[(int) page].getItem(index);
      if (itemJar == null || !this.player.equipment.checkSelection(page, itemJar.x, itemJar.y))
        return;
      this.player.equipment.quality = newQuality;
    }

    public void updateState(byte page, byte index, byte[] newState)
    {
      if ((int) page < 0 || (int) page >= (int) PlayerInventory.PAGES)
        return;
      this.items[(int) page].updateState(index, newState);
    }

    public List<InventorySearch> search(EItemType type)
    {
      List<InventorySearch> search = new List<InventorySearch>();
      for (byte index = PlayerInventory.SLOTS; (int) index < (int) PlayerInventory.PAGES - 1; ++index)
        this.items[(int) index].search(search, type);
      return search;
    }

    public List<InventorySearch> search(EItemType type, byte caliber)
    {
      List<InventorySearch> search = new List<InventorySearch>();
      for (byte index = PlayerInventory.SLOTS; (int) index < (int) PlayerInventory.PAGES - 1; ++index)
        this.items[(int) index].search(search, type, caliber);
      return search;
    }

    public List<InventorySearch> search(ushort id, bool findEmpty, bool findHealthy)
    {
      List<InventorySearch> search = new List<InventorySearch>();
      for (byte index = PlayerInventory.SLOTS; (int) index < (int) PlayerInventory.PAGES - 1; ++index)
        this.items[(int) index].search(search, id, findEmpty, findHealthy);
      return search;
    }

    public List<InventorySearch> search(List<InventorySearch> search)
    {
      List<InventorySearch> list = new List<InventorySearch>();
      for (int index1 = 0; index1 < search.Count; ++index1)
      {
        InventorySearch inventorySearch1 = search[index1];
        bool flag = true;
        for (int index2 = 0; index2 < list.Count; ++index2)
        {
          InventorySearch inventorySearch2 = list[index2];
          if ((int) inventorySearch2.jar.item.id == (int) inventorySearch1.jar.item.id && (int) inventorySearch2.jar.item.amount == (int) inventorySearch1.jar.item.amount)
          {
            flag = false;
            break;
          }
        }
        if (flag)
          list.Add(inventorySearch1);
      }
      return list;
    }

    public InventorySearch has(ushort id)
    {
      for (byte index = (byte) 0; (int) index < (int) PlayerInventory.PAGES - 1; ++index)
      {
        InventorySearch inventorySearch = this.items[(int) index].has(id);
        if (inventorySearch != null)
          return inventorySearch;
      }
      return (InventorySearch) null;
    }

    public bool tryAddItem(Item item, bool auto)
    {
      if (item == null)
        return false;
      ItemAsset itemAsset = (ItemAsset) Assets.find(EAssetType.ITEM, item.id);
      if (itemAsset == null || itemAsset.isPro)
        return false;
      if (auto)
      {
        if (itemAsset.slot == ESlotType.PRIMARY)
        {
          if (this.items[0].tryAddItem(item))
          {
            this.player.equipment.sendSlot((byte) 0);
            if (!this.player.equipment.isSelected)
              this.player.equipment.tryEquip((byte) 0, (byte) 0, (byte) 0);
            return true;
          }
        }
        else if (itemAsset.slot == ESlotType.SECONDARY)
        {
          if (this.items[1].tryAddItem(item))
          {
            this.player.equipment.sendSlot((byte) 1);
            if (!this.player.equipment.isSelected)
              this.player.equipment.tryEquip((byte) 1, (byte) 0, (byte) 0);
            return true;
          }
          if (this.items[0].tryAddItem(item))
          {
            this.player.equipment.sendSlot((byte) 0);
            if (!this.player.equipment.isSelected)
              this.player.equipment.tryEquip((byte) 0, (byte) 0, (byte) 0);
            return true;
          }
        }
        else
        {
          if ((int) this.player.clothing.hat == 0 && itemAsset.type == EItemType.HAT)
          {
            this.player.clothing.askWearHat(item.id, item.quality, item.state);
            return true;
          }
          if ((int) this.player.clothing.shirt == 0 && itemAsset.type == EItemType.SHIRT)
          {
            this.player.clothing.askWearShirt(item.id, item.quality, item.state);
            return true;
          }
          if ((int) this.player.clothing.pants == 0 && itemAsset.type == EItemType.PANTS)
          {
            this.player.clothing.askWearPants(item.id, item.quality, item.state);
            return true;
          }
          if ((int) this.player.clothing.backpack == 0 && itemAsset.type == EItemType.BACKPACK)
          {
            this.player.clothing.askWearBackpack(item.id, item.quality, item.state);
            return true;
          }
          if ((int) this.player.clothing.vest == 0 && itemAsset.type == EItemType.VEST)
          {
            this.player.clothing.askWearVest(item.id, item.quality, item.state);
            return true;
          }
          if ((int) this.player.clothing.mask == 0 && itemAsset.type == EItemType.MASK)
          {
            this.player.clothing.askWearMask(item.id, item.quality, item.state);
            return true;
          }
          if ((int) this.player.clothing.glasses == 0 && itemAsset.type == EItemType.GLASSES)
          {
            this.player.clothing.askWearGlasses(item.id, item.quality, item.state);
            return true;
          }
        }
      }
      for (byte page = PlayerInventory.SLOTS; (int) page < (int) PlayerInventory.PAGES - 1; ++page)
      {
        if (this.items[(int) page].tryAddItem(item))
        {
          if (auto && !this.player.equipment.isSelected && (itemAsset.slot == ESlotType.NONE && itemAsset.useable != EUseableType.NONE))
          {
            ItemJar itemJar = this.items[(int) page].getItem((byte) ((uint) this.items[(int) page].getItemCount() - 1U));
            this.player.equipment.tryEquip(page, itemJar.x, itemJar.y);
          }
          return true;
        }
      }
      return false;
    }

    public void forceAddItem(Item item, bool auto)
    {
      if (this.tryAddItem(item, auto))
        return;
      ItemManager.dropItem(item, this.transform.position, false, true, true);
    }

    public void removeItem(byte page, byte index)
    {
      this.items[(int) page].removeItem(index);
    }

    public bool checkSpace(byte page, byte old_x, byte old_y, byte new_x, byte new_y, byte size_x, byte size_y, bool checkSame)
    {
      if ((int) page < 0 || (int) page >= (int) PlayerInventory.PAGES)
        return false;
      return this.items[(int) page].checkSpace(old_x, old_y, new_x, new_y, size_x, size_y, checkSame);
    }

    public bool checkSwap(byte page, byte x, byte y, byte size_x, byte size_y, byte old_x, byte old_y)
    {
      if ((int) page < 0 || (int) page >= (int) PlayerInventory.PAGES)
        return false;
      return this.items[(int) page].checkSwap(x, y, size_x, size_y, old_x, old_y);
    }

    public bool tryFindSpace(byte page, byte size_x, byte size_y, out byte x, out byte y)
    {
      x = (byte) 0;
      y = (byte) 0;
      if ((int) page < 0 || (int) page >= (int) PlayerInventory.PAGES)
        return false;
      return this.items[(int) page].tryFindSpace(size_x, size_y, out x, out y);
    }

    public bool tryFindSpace(byte size_x, byte size_y, out byte page, out byte x, out byte y)
    {
      x = (byte) 0;
      y = (byte) 0;
      page = PlayerInventory.SLOTS;
      while ((int) page < (int) PlayerInventory.PAGES - 1)
      {
        if (this.items[(int) page].tryFindSpace(size_x, size_y, out x, out y))
          return true;
        page = (byte) ((uint) page + 1U);
      }
      return false;
    }

    [SteamCall]
    public void askDragItem(CSteamID steamID, byte page_0, byte x_0, byte y_0, byte page_1, byte x_1, byte y_1)
    {
      if (!this.channel.checkOwner(steamID) || !Provider.isServer || ((int) page_0 < 0 || (int) page_0 >= (int) PlayerInventory.PAGES))
        return;
      byte index = this.items[(int) page_0].getIndex(x_0, y_0);
      if ((int) index == (int) byte.MaxValue || (int) page_1 < 0 || (int) page_1 >= (int) PlayerInventory.PAGES)
        return;
      ItemJar itemJar = this.items[(int) page_0].getItem(index);
      if (!this.checkSpace(page_1, x_0, y_0, x_1, y_1, itemJar.size_x, itemJar.size_y, (int) page_0 == (int) page_1))
        return;
      ItemAsset itemAsset = (ItemAsset) Assets.find(EAssetType.ITEM, itemJar.item.id);
      if (itemAsset == null || (int) page_1 < (int) PlayerInventory.SLOTS && (itemAsset.slot == ESlotType.NONE || (int) page_1 == 1 && itemAsset.slot != ESlotType.SECONDARY))
        return;
      this.removeItem(page_0, index);
      this.items[(int) page_1].addItem(x_1, y_1, itemJar.item);
      if ((int) page_0 < (int) PlayerInventory.SLOTS)
        this.player.equipment.sendSlot(page_0);
      if ((int) page_1 >= (int) PlayerInventory.SLOTS)
        return;
      this.player.equipment.sendSlot(page_1);
    }

    [SteamCall]
    public void askSwapItem(CSteamID steamID, byte page_0, byte x_0, byte y_0, byte page_1, byte x_1, byte y_1)
    {
      if (!this.channel.checkOwner(steamID) || !Provider.isServer || ((int) page_0 < 0 || (int) page_0 >= (int) PlayerInventory.PAGES))
        return;
      byte index1 = this.items[(int) page_0].getIndex(x_0, y_0);
      if ((int) index1 == (int) byte.MaxValue || (int) page_1 < 0 || (int) page_1 >= (int) PlayerInventory.PAGES)
        return;
      byte index2 = this.items[(int) page_1].getIndex(x_1, y_1);
      if ((int) index2 == (int) byte.MaxValue)
        return;
      ItemJar itemJar1 = this.items[(int) page_0].getItem(index1);
      ItemJar itemJar2 = this.items[(int) page_1].getItem(index2);
      if (!this.checkSwap(page_1, x_1, y_1, itemJar1.size_x, itemJar1.size_y, itemJar2.size_x, itemJar2.size_y) || !this.checkSwap(page_0, x_0, y_0, itemJar2.size_x, itemJar2.size_y, itemJar1.size_x, itemJar1.size_y))
        return;
      ItemAsset itemAsset1 = (ItemAsset) Assets.find(EAssetType.ITEM, itemJar1.item.id);
      if (itemAsset1 == null || (int) page_1 < (int) PlayerInventory.SLOTS && (itemAsset1.slot == ESlotType.NONE || (int) page_1 == 1 && itemAsset1.slot != ESlotType.SECONDARY))
        return;
      ItemAsset itemAsset2 = (ItemAsset) Assets.find(EAssetType.ITEM, itemJar2.item.id);
      if (itemAsset2 == null || (int) page_0 < (int) PlayerInventory.SLOTS && (itemAsset2.slot == ESlotType.NONE || (int) page_0 == 1 && itemAsset2.slot != ESlotType.SECONDARY))
        return;
      this.removeItem(page_0, index1);
      if ((int) page_0 == (int) page_1 && (int) index2 > (int) index1)
        --index2;
      this.removeItem(page_1, index2);
      this.items[(int) page_0].addItem(x_0, y_0, itemJar2.item);
      this.items[(int) page_1].addItem(x_1, y_1, itemJar1.item);
      if ((int) page_0 < (int) PlayerInventory.SLOTS)
        this.player.equipment.sendSlot(page_0);
      if ((int) page_1 >= (int) PlayerInventory.SLOTS)
        return;
      this.player.equipment.sendSlot(page_1);
    }

    public void sendDragItem(byte page_0, byte x_0, byte y_0, byte page_1, byte x_1, byte y_1)
    {
      this.channel.send("askDragItem", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, (object) page_0, (object) x_0, (object) y_0, (object) page_1, (object) x_1, (object) y_1);
    }

    public void sendSwapItem(byte page_0, byte x_0, byte y_0, byte page_1, byte x_1, byte y_1)
    {
      this.channel.send("askSwapItem", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, (object) page_0, (object) x_0, (object) y_0, (object) page_1, (object) x_1, (object) y_1);
    }

    [SteamCall]
    public void askDropItem(CSteamID steamID, byte page, byte x, byte y)
    {
      if (!this.channel.checkOwner(steamID) || !Provider.isServer || ((int) page < 0 || (int) page >= (int) PlayerInventory.PAGES))
        return;
      if (this.player.equipment.checkSelection(page, x, y))
      {
        if (this.player.equipment.isBusy)
          return;
        this.player.equipment.dequip();
      }
      if (this.items == null)
        return;
      byte index = this.items[(int) page].getIndex(x, y);
      if ((int) index == (int) byte.MaxValue)
        return;
      ItemJar itemJar = this.items[(int) page].getItem(index);
      if (itemJar == null)
        return;
      ItemManager.dropItem(itemJar.item, this.transform.position, true, true, true);
      this.removeItem(page, index);
      if ((int) page >= (int) PlayerInventory.SLOTS)
        return;
      this.player.equipment.sendSlot(page);
    }

    public void sendDropItem(byte page, byte x, byte y)
    {
      this.channel.send("askDropItem", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[3]
      {
        (object) page,
        (object) x,
        (object) y
      });
    }

    [SteamCall]
    public void tellUpdateAmount(CSteamID steamID, byte page, byte index, byte amount)
    {
      if (!this.channel.checkServer(steamID))
        return;
      this.updateAmount(page, index, amount);
    }

    [SteamCall]
    public void tellUpdateQuality(CSteamID steamID, byte page, byte index, byte quality)
    {
      if (!this.channel.checkServer(steamID))
        return;
      this.updateQuality(page, index, quality);
    }

    [SteamCall]
    public void tellItemAdd(CSteamID steamID, byte page, byte x, byte y, ushort id, byte amount, byte quality, byte[] state)
    {
      if (!this.channel.checkServer(steamID))
        return;
      this.items[(int) page].addItem(x, y, new Item(id, amount, quality, state));
    }

    [SteamCall]
    public void tellItemRemove(CSteamID steamID, byte page, byte x, byte y)
    {
      if (!this.channel.checkServer(steamID))
        return;
      byte index = this.items[(int) page].getIndex(x, y);
      if ((int) index == (int) byte.MaxValue)
        return;
      this.items[(int) page].removeItem(index);
    }

    [SteamCall]
    public void tellSize(CSteamID steamID, byte page, byte newWidth, byte newHeight)
    {
      if (!this.channel.checkServer(steamID) || (int) page > (int) PlayerInventory.PAGES || (this.items == null || this.items[(int) page] == null))
        return;
      this.items[(int) page].resize(newWidth, newHeight);
    }

    [SteamCall]
    public void tellStoraging(CSteamID steamID)
    {
      if (!this.channel.checkServer(steamID))
        return;
      object[] objArray1 = this.channel.read(Types.BYTE_TYPE, Types.BYTE_TYPE, Types.BYTE_TYPE);
      this.items[(int) PlayerInventory.STORAGE].resize((byte) objArray1[0], (byte) objArray1[1]);
      byte num = (byte) objArray1[2];
      for (byte index = (byte) 0; (int) index < (int) num; ++index)
      {
        object[] objArray2 = this.channel.read(Types.BYTE_TYPE, Types.BYTE_TYPE, Types.UINT16_TYPE, Types.BYTE_TYPE, Types.BYTE_TYPE, Types.BYTE_ARRAY_TYPE);
        this.items[(int) PlayerInventory.STORAGE].addItem((byte) objArray2[0], (byte) objArray2[1], new Item((ushort) objArray2[2], (byte) objArray2[3], (byte) objArray2[4], (byte[]) objArray2[5]));
      }
      if (this.onInventoryStored != null)
        this.onInventoryStored();
      this.isStoring = true;
    }

    [SteamCall]
    public void tellInventory(CSteamID steamID)
    {
      if (!this.channel.checkServer(steamID))
        return;
      for (byte index1 = (byte) 0; (int) index1 < (int) PlayerInventory.PAGES - 1; ++index1)
      {
        object[] objArray1 = this.channel.read(Types.BYTE_TYPE, Types.BYTE_TYPE, Types.BYTE_TYPE);
        this.items[(int) index1].resize((byte) objArray1[0], (byte) objArray1[1]);
        byte num = (byte) objArray1[2];
        for (byte index2 = (byte) 0; (int) index2 < (int) num; ++index2)
        {
          object[] objArray2 = this.channel.read(Types.BYTE_TYPE, Types.BYTE_TYPE, Types.UINT16_TYPE, Types.BYTE_TYPE, Types.BYTE_TYPE, Types.BYTE_ARRAY_TYPE);
          this.items[(int) index1].addItem((byte) objArray2[0], (byte) objArray2[1], new Item((ushort) objArray2[2], (byte) objArray2[3], (byte) objArray2[4], (byte[]) objArray2[5]));
        }
      }
      Player.isLoadingInventory = false;
    }

    [SteamCall]
    public void askInventory(CSteamID steamID)
    {
      if (!this.channel.checkOwner(steamID) || !Provider.isServer)
        return;
      if (!this.channel.isOwner)
        this.channel.openWrite();
      for (byte page = (byte) 0; (int) page < (int) PlayerInventory.PAGES - 1; ++page)
      {
        if (this.channel.isOwner)
          this.onInventoryResized(page, this.items[(int) page].width, this.items[(int) page].height);
        else
          this.channel.write((object) this.items[(int) page].width, (object) this.items[(int) page].height, (object) this.items[(int) page].getItemCount());
        for (byte index = (byte) 0; (int) index < (int) this.items[(int) page].getItemCount(); ++index)
        {
          ItemJar jar = this.items[(int) page].getItem(index);
          if (this.channel.isOwner)
            this.onItemAdded(page, index, jar);
          else
            this.channel.write((object) jar.x, (object) jar.y, (object) jar.item.id, (object) jar.item.amount, (object) jar.item.quality, (object) jar.item.state);
        }
      }
      if (this.channel.isOwner)
        Player.isLoadingInventory = false;
      else
        this.channel.closeWrite("tellInventory", steamID, ESteamPacket.UPDATE_RELIABLE_CHUNK_BUFFER);
    }

    public void sendStorage()
    {
      if (!this.channel.isOwner)
        this.channel.openWrite();
      if (this.channel.isOwner)
      {
        this.onInventoryResized(PlayerInventory.STORAGE, this.items[(int) PlayerInventory.STORAGE].width, this.items[(int) PlayerInventory.STORAGE].height);
        if (this.onInventoryStored != null)
          this.onInventoryStored();
      }
      else
        this.channel.write((object) this.items[(int) PlayerInventory.STORAGE].width, (object) this.items[(int) PlayerInventory.STORAGE].height, (object) this.items[(int) PlayerInventory.STORAGE].getItemCount());
      for (byte index = (byte) 0; (int) index < (int) this.items[(int) PlayerInventory.STORAGE].getItemCount(); ++index)
      {
        ItemJar jar = this.items[(int) PlayerInventory.STORAGE].getItem(index);
        if (this.channel.isOwner)
          this.onItemAdded(PlayerInventory.STORAGE, index, jar);
        else
          this.channel.write((object) jar.x, (object) jar.y, (object) jar.item.id, (object) jar.item.amount, (object) jar.item.quality, (object) jar.item.state);
      }
      if (this.channel.isOwner)
        return;
      this.channel.closeWrite("tellStoraging", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_CHUNK_BUFFER);
    }

    public void updateItems(byte page, Items newItems)
    {
      if (this.items[(int) page] != null)
      {
        this.items[(int) page].onItemsResized -= new ItemsResized(this.onItemsResized);
        this.items[(int) page].onItemUpdated -= new ItemUpdated(this.onItemUpdated);
        this.items[(int) page].onItemAdded -= new ItemAdded(this.onItemAdded);
        this.items[(int) page].onItemRemoved -= new ItemRemoved(this.onItemRemoved);
      }
      if (newItems != null)
      {
        this.items[(int) page] = newItems;
        this.items[(int) page].onItemsResized += new ItemsResized(this.onItemsResized);
        this.items[(int) page].onItemUpdated += new ItemUpdated(this.onItemUpdated);
        this.items[(int) page].onItemAdded += new ItemAdded(this.onItemAdded);
        this.items[(int) page].onItemRemoved += new ItemRemoved(this.onItemRemoved);
      }
      else
      {
        this.items[(int) page] = new Items(PlayerInventory.STORAGE);
        this.items[(int) page].onItemsResized += new ItemsResized(this.onItemsResized);
        this.items[(int) page].onItemUpdated += new ItemUpdated(this.onItemUpdated);
        this.items[(int) page].onItemAdded += new ItemAdded(this.onItemAdded);
        this.items[(int) page].onItemRemoved += new ItemRemoved(this.onItemRemoved);
        if (this.onInventoryResized == null)
          return;
        this.onInventoryResized(page, (byte) 0, (byte) 0);
      }
    }

    public void sendUpdateAmount(byte page, byte x, byte y, byte amount)
    {
      byte index = this.getIndex(page, x, y);
      this.updateAmount(page, index, amount);
      if (this.channel.isOwner)
        return;
      this.channel.send("tellUpdateAmount", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[3]
      {
        (object) page,
        (object) index,
        (object) amount
      });
    }

    public void sendUpdateQuality(byte page, byte x, byte y, byte quality)
    {
      byte index = this.getIndex(page, x, y);
      this.updateQuality(page, index, quality);
      if (this.channel.isOwner)
        return;
      this.channel.send("tellUpdateQuality", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[3]
      {
        (object) page,
        (object) index,
        (object) quality
      });
    }

    private void sendItemAdd(byte page, ItemJar jar)
    {
      this.channel.send("tellItemAdd", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, (object) page, (object) jar.x, (object) jar.y, (object) jar.item.id, (object) jar.item.amount, (object) jar.item.quality, (object) jar.item.state);
    }

    private void sendItemRemove(byte page, ItemJar jar)
    {
      this.channel.send("tellItemRemove", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[3]
      {
        (object) page,
        (object) jar.x,
        (object) jar.y
      });
    }

    private void onShirtUpdated(ushort id, byte quality, byte[] state)
    {
      if ((int) id != 0)
      {
        ItemBagAsset itemBagAsset = (ItemBagAsset) Assets.find(EAssetType.ITEM, id);
        if (itemBagAsset == null)
          return;
        this.items[(int) PlayerInventory.SHIRT].resize(itemBagAsset.width, itemBagAsset.height);
      }
      else
        this.items[(int) PlayerInventory.SHIRT].resize((byte) 0, (byte) 0);
    }

    private void onPantsUpdated(ushort id, byte quality, byte[] state)
    {
      if ((int) id != 0)
      {
        ItemBagAsset itemBagAsset = (ItemBagAsset) Assets.find(EAssetType.ITEM, id);
        if (itemBagAsset == null)
          return;
        this.items[(int) PlayerInventory.PANTS].resize(itemBagAsset.width, itemBagAsset.height);
      }
      else
        this.items[(int) PlayerInventory.PANTS].resize((byte) 0, (byte) 0);
    }

    private void onBackpackUpdated(ushort id, byte quality, byte[] state)
    {
      if ((int) id != 0)
      {
        ItemBagAsset itemBagAsset = (ItemBagAsset) Assets.find(EAssetType.ITEM, id);
        if (itemBagAsset == null)
          return;
        this.items[(int) PlayerInventory.BACKPACK].resize(itemBagAsset.width, itemBagAsset.height);
      }
      else
        this.items[(int) PlayerInventory.BACKPACK].resize((byte) 0, (byte) 0);
    }

    private void onVestUpdated(ushort id, byte quality, byte[] state)
    {
      if ((int) id != 0)
      {
        ItemBagAsset itemBagAsset = (ItemBagAsset) Assets.find(EAssetType.ITEM, id);
        if (itemBagAsset == null)
          return;
        this.items[(int) PlayerInventory.VEST].resize(itemBagAsset.width, itemBagAsset.height);
      }
      else
        this.items[(int) PlayerInventory.VEST].resize((byte) 0, (byte) 0);
    }

    private void onLifeUpdated(bool isDead)
    {
      if (!Provider.isServer)
        return;
      if (isDead)
      {
        for (byte index = (byte) 0; (int) index < (int) PlayerInventory.PAGES - 1; ++index)
          this.items[(int) index].resize((byte) 0, (byte) 0);
      }
      else
      {
        this.items[0].resize((byte) 1, (byte) 1);
        this.items[1].resize((byte) 1, (byte) 1);
        this.items[2].resize((byte) 5, (byte) 3);
        if (Level.info.type == ELevelType.HORDE)
        {
          for (int index = 0; index < PlayerInventory.HORDE.Length; ++index)
            this.tryAddItem(new Item(PlayerInventory.HORDE[index], true), true);
        }
        else
        {
          if (!Dedicator.isDedicated)
            return;
          for (int index = 0; index < PlayerInventory.loadout.Length; ++index)
            this.tryAddItem(new Item(PlayerInventory.loadout[index], true), true);
        }
      }
    }

    private void onItemsResized(byte page, byte newWidth, byte newHeight)
    {
      if (!this.channel.isOwner && Provider.isServer)
        this.channel.send("tellSize", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[3]
        {
          (object) page,
          (object) newWidth,
          (object) newHeight
        });
      if (this.onInventoryResized == null)
        return;
      this.onInventoryResized(page, newWidth, newHeight);
    }

    private void onItemUpdated(byte page, byte index, ItemJar jar)
    {
      if (this.onInventoryUpdated == null)
        return;
      this.onInventoryUpdated(page, index, jar);
    }

    private void onItemAdded(byte page, byte index, ItemJar jar)
    {
      if (!this.channel.isOwner && Provider.isServer)
        this.sendItemAdd(page, jar);
      if (this.onInventoryAdded == null)
        return;
      this.onInventoryAdded(page, index, jar);
    }

    private void onItemRemoved(byte page, byte index, ItemJar jar)
    {
      if (Provider.isServer)
      {
        if (!this.channel.isOwner)
          this.sendItemRemove(page, jar);
        if (this.player.equipment.checkSelection(page, jar.x, jar.y))
          this.player.equipment.dequip();
      }
      if (this.onInventoryRemoved == null)
        return;
      this.onInventoryRemoved(page, index, jar);
    }

    private void onItemDiscarded(byte page, byte index, ItemJar jar)
    {
      if (!Provider.isServer)
        return;
      if (!this.channel.isOwner)
        this.sendItemRemove(page, jar);
      if (this.onInventoryRemoved != null)
        this.onInventoryRemoved(page, index, jar);
      ItemManager.dropItem(jar.item, this.transform.position, false, true, true);
    }

    public void init()
    {
      this.channel.send("askInventory", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER);
    }

    private void OnDestroy()
    {
      if (!((Object) this.storage != (Object) null))
        return;
      this.storage.isOpen = false;
    }

    private void Start()
    {
      this.items = new Items[(int) PlayerInventory.PAGES];
      for (byte newPage = (byte) 0; (int) newPage < (int) PlayerInventory.PAGES; ++newPage)
      {
        this.items[(int) newPage] = new Items(newPage);
        this.items[(int) newPage].onItemsResized += new ItemsResized(this.onItemsResized);
        this.items[(int) newPage].onItemUpdated += new ItemUpdated(this.onItemUpdated);
        this.items[(int) newPage].onItemAdded += new ItemAdded(this.onItemAdded);
        this.items[(int) newPage].onItemRemoved += new ItemRemoved(this.onItemRemoved);
      }
      if (Provider.isServer)
      {
        this.player.clothing.onShirtUpdated += new ShirtUpdated(this.onShirtUpdated);
        this.player.clothing.onPantsUpdated += new PantsUpdated(this.onPantsUpdated);
        this.player.clothing.onBackpackUpdated += new BackpackUpdated(this.onBackpackUpdated);
        this.player.clothing.onVestUpdated += new VestUpdated(this.onVestUpdated);
        this.player.life.onLifeUpdated += new LifeUpdated(this.onLifeUpdated);
        for (byte index = (byte) 0; (int) index < (int) PlayerInventory.PAGES; ++index)
          this.items[(int) index].onItemDiscarded = new ItemDiscarded(this.onItemDiscarded);
        this.load();
      }
      if (!this.channel.isOwner)
        return;
      this.Invoke("init", 0.1f);
    }

    public void load()
    {
      if (PlayerSavedata.fileExists(this.channel.owner.playerID, "/Player/Inventory.dat") && Level.info.type == ELevelType.SURVIVAL)
      {
        Block block = PlayerSavedata.readBlock(this.channel.owner.playerID, "/Player/Inventory.dat", (byte) 0);
        if ((int) block.readByte() > 3)
        {
          for (byte index1 = (byte) 0; (int) index1 < (int) PlayerInventory.PAGES - 1; ++index1)
          {
            this.items[(int) index1].loadSize(block.readByte(), block.readByte());
            byte num1 = block.readByte();
            for (byte index2 = (byte) 0; (int) index2 < (int) num1; ++index2)
            {
              byte x = block.readByte();
              byte y = block.readByte();
              ushort num2 = block.readUInt16();
              byte newAmount = block.readByte();
              byte newQuality = block.readByte();
              byte[] newState = block.readByteArray();
              if ((ItemAsset) Assets.find(EAssetType.ITEM, num2) != null)
                this.items[(int) index1].loadItem(x, y, new Item(num2, newAmount, newQuality, newState));
            }
          }
        }
        else
        {
          this.items[0].loadSize((byte) 1, (byte) 1);
          this.items[1].loadSize((byte) 1, (byte) 1);
          this.items[2].loadSize((byte) 5, (byte) 3);
          this.items[(int) PlayerInventory.BACKPACK].loadSize((byte) 0, (byte) 0);
          this.items[(int) PlayerInventory.VEST].loadSize((byte) 0, (byte) 0);
          this.items[(int) PlayerInventory.SHIRT].loadSize((byte) 0, (byte) 0);
          this.items[(int) PlayerInventory.PANTS].loadSize((byte) 0, (byte) 0);
          this.items[(int) PlayerInventory.STORAGE].loadSize((byte) 0, (byte) 0);
          if (Level.info.type == ELevelType.HORDE)
          {
            for (int index = 0; index < PlayerInventory.HORDE.Length; ++index)
              this.tryAddItem(new Item(PlayerInventory.HORDE[index], true), true);
          }
          else
          {
            if (!Dedicator.isDedicated)
              return;
            for (int index = 0; index < PlayerInventory.loadout.Length; ++index)
              this.tryAddItem(new Item(PlayerInventory.loadout[index], true), true);
          }
        }
      }
      else
      {
        this.items[0].loadSize((byte) 1, (byte) 1);
        this.items[1].loadSize((byte) 1, (byte) 1);
        this.items[2].loadSize((byte) 5, (byte) 3);
        this.items[(int) PlayerInventory.BACKPACK].loadSize((byte) 0, (byte) 0);
        this.items[(int) PlayerInventory.VEST].loadSize((byte) 0, (byte) 0);
        this.items[(int) PlayerInventory.SHIRT].loadSize((byte) 0, (byte) 0);
        this.items[(int) PlayerInventory.PANTS].loadSize((byte) 0, (byte) 0);
        this.items[(int) PlayerInventory.STORAGE].loadSize((byte) 0, (byte) 0);
        if (Level.info.type == ELevelType.HORDE)
        {
          for (int index = 0; index < PlayerInventory.HORDE.Length; ++index)
            this.tryAddItem(new Item(PlayerInventory.HORDE[index], true), true);
        }
        else
        {
          if (!Dedicator.isDedicated)
            return;
          for (int index = 0; index < PlayerInventory.loadout.Length; ++index)
            this.tryAddItem(new Item(PlayerInventory.loadout[index], true), true);
        }
      }
    }

    public void save()
    {
      if (this.player.life.isDead)
      {
        if (!PlayerSavedata.fileExists(this.channel.owner.playerID, "/Player/Inventory.dat"))
          return;
        PlayerSavedata.deleteFile(this.channel.owner.playerID, "/Player/Inventory.dat");
      }
      else
      {
        Block block = new Block();
        block.writeByte(PlayerInventory.SAVEDATA_VERSION);
        for (byte index1 = (byte) 0; (int) index1 < (int) PlayerInventory.PAGES - 1; ++index1)
        {
          block.writeByte(this.items[(int) index1].width);
          block.writeByte(this.items[(int) index1].height);
          block.writeByte(this.items[(int) index1].getItemCount());
          for (byte index2 = (byte) 0; (int) index2 < (int) this.items[(int) index1].getItemCount(); ++index2)
          {
            ItemJar itemJar = this.items[(int) index1].getItem(index2);
            block.writeByte(itemJar.x);
            block.writeByte(itemJar.y);
            block.writeUInt16(itemJar.item.id);
            block.writeByte(itemJar.item.amount);
            block.writeByte(itemJar.item.quality);
            block.writeByteArray(itemJar.item.state);
          }
        }
        PlayerSavedata.writeBlock(this.channel.owner.playerID, "/Player/Inventory.dat", block);
      }
    }
  }
}
