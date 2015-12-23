// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.Items
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

namespace SDG.Unturned
{
  public class Items
  {
    public ItemsResized onItemsResized;
    public ItemUpdated onItemUpdated;
    public ItemAdded onItemAdded;
    public ItemRemoved onItemRemoved;
    public ItemDiscarded onItemDiscarded;
    public StateUpdated onStateUpdated;
    private byte _page;
    private byte _width;
    private byte _height;
    private bool[,] slots;
    private List<ItemJar> items;

    public byte page
    {
      get
      {
        return this._page;
      }
    }

    public byte width
    {
      get
      {
        return this._width;
      }
    }

    public byte height
    {
      get
      {
        return this._height;
      }
    }

    public Items(byte newPage)
    {
      this._page = newPage;
      this.items = new List<ItemJar>();
    }

    public void updateAmount(byte index, byte newAmount)
    {
      if ((int) index < 0 || (int) index >= this.items.Count)
        return;
      this.items[(int) index].item.amount = newAmount;
      if (this.onItemUpdated != null)
        this.onItemUpdated(this.page, index, this.items[(int) index]);
      if (this.onStateUpdated == null)
        return;
      this.onStateUpdated();
    }

    public void updateQuality(byte index, byte newQuality)
    {
      if ((int) index < 0 || (int) index >= this.items.Count)
        return;
      this.items[(int) index].item.quality = newQuality;
      if (this.onItemUpdated != null)
        this.onItemUpdated(this.page, index, this.items[(int) index]);
      if (this.onStateUpdated == null)
        return;
      this.onStateUpdated();
    }

    public void updateState(byte index, byte[] newState)
    {
      if ((int) index < 0 || (int) index >= this.items.Count)
        return;
      this.items[(int) index].item.state = newState;
      if (this.onItemUpdated != null)
        this.onItemUpdated(this.page, index, this.items[(int) index]);
      if (this.onStateUpdated == null)
        return;
      this.onStateUpdated();
    }

    public byte getItemCount()
    {
      return (byte) this.items.Count;
    }

    public ItemJar getItem(byte index)
    {
      if ((int) index < 0 || (int) index >= this.items.Count)
        return (ItemJar) null;
      return this.items[(int) index];
    }

    public byte getIndex(byte x, byte y)
    {
      if ((int) this.page < (int) PlayerInventory.SLOTS)
        return (byte) 0;
      if ((int) x < 0 || (int) y < 0 || ((int) x >= (int) this.width || (int) y >= (int) this.height))
        return byte.MaxValue;
      for (byte index = (byte) 0; (int) index < this.items.Count; ++index)
      {
        if ((int) this.items[(int) index].x == (int) x && (int) this.items[(int) index].y == (int) y)
          return index;
      }
      return byte.MaxValue;
    }

    public byte findIndex(byte x, byte y, out byte find_x, out byte find_y)
    {
      find_x = byte.MaxValue;
      find_y = byte.MaxValue;
      if ((int) x < 0 || (int) y < 0 || ((int) x >= (int) this.width || (int) y >= (int) this.height))
        return byte.MaxValue;
      for (byte index = (byte) 0; (int) index < this.items.Count; ++index)
      {
        if ((int) this.items[(int) index].x <= (int) x && (int) this.items[(int) index].y <= (int) y && ((int) this.items[(int) index].x + (int) this.items[(int) index].size_x > (int) x && (int) this.items[(int) index].y + (int) this.items[(int) index].size_y > (int) y))
        {
          find_x = this.items[(int) index].x;
          find_y = this.items[(int) index].y;
          return index;
        }
      }
      return byte.MaxValue;
    }

    public List<InventorySearch> search(List<InventorySearch> search, EItemType type)
    {
      for (byte index = (byte) 0; (int) index < this.items.Count; ++index)
      {
        ItemJar newJar = this.items[(int) index];
        if ((int) newJar.item.amount > 0)
        {
          ItemAsset itemAsset = (ItemAsset) Assets.find(EAssetType.ITEM, newJar.item.id);
          if (itemAsset != null && itemAsset.type == type)
            search.Add(new InventorySearch(this.page, newJar));
        }
      }
      return search;
    }

    public List<InventorySearch> search(List<InventorySearch> search, EItemType type, byte caliber)
    {
      for (byte index1 = (byte) 0; (int) index1 < this.items.Count; ++index1)
      {
        ItemJar newJar = this.items[(int) index1];
        if ((int) newJar.item.amount > 0)
        {
          ItemAsset itemAsset = (ItemAsset) Assets.find(EAssetType.ITEM, newJar.item.id);
          if (itemAsset != null && itemAsset.type == type)
          {
            if (((ItemCaliberAsset) itemAsset).calibers.Length == 0)
            {
              search.Add(new InventorySearch(this.page, newJar));
            }
            else
            {
              for (byte index2 = (byte) 0; (int) index2 < ((ItemCaliberAsset) itemAsset).calibers.Length; ++index2)
              {
                if ((int) ((ItemCaliberAsset) itemAsset).calibers[(int) index2] == (int) caliber)
                {
                  search.Add(new InventorySearch(this.page, newJar));
                  break;
                }
              }
            }
          }
        }
      }
      return search;
    }

    public List<InventorySearch> search(List<InventorySearch> search, ushort id, bool findEmpty, bool findHealthy)
    {
      for (byte index = (byte) 0; (int) index < this.items.Count; ++index)
      {
        ItemJar newJar = this.items[(int) index];
        if ((findEmpty || (int) newJar.item.amount > 0) && (findHealthy || (int) newJar.item.quality < 100) && (int) newJar.item.id == (int) id)
          search.Add(new InventorySearch(this.page, newJar));
      }
      return search;
    }

    public InventorySearch has(ushort id)
    {
      for (byte index = (byte) 0; (int) index < this.items.Count; ++index)
      {
        ItemJar newJar = this.items[(int) index];
        if ((int) newJar.item.amount > 0 && (int) newJar.item.id == (int) id)
          return new InventorySearch(this.page, newJar);
      }
      return (InventorySearch) null;
    }

    public void loadItem(byte x, byte y, Item item)
    {
      ItemJar jar = new ItemJar(x, y, item);
      this.fillSlot(jar, true);
      this.items.Add(jar);
    }

    public void addItem(byte x, byte y, Item item)
    {
      ItemJar jar = new ItemJar(x, y, item);
      this.fillSlot(jar, true);
      this.items.Add(jar);
      if (this.onItemAdded != null)
        this.onItemAdded(this.page, (byte) (this.items.Count - 1), jar);
      if (this.onStateUpdated == null)
        return;
      this.onStateUpdated();
    }

    public bool tryAddItem(Item item)
    {
      ItemJar jar = new ItemJar(item);
      byte x;
      byte y;
      if (!this.tryFindSpace(jar.size_x, jar.size_y, out x, out y))
        return false;
      jar.x = x;
      jar.y = y;
      this.fillSlot(jar, true);
      this.items.Add(jar);
      if (this.onItemAdded != null)
        this.onItemAdded(this.page, (byte) (this.items.Count - 1), jar);
      if (this.onStateUpdated != null)
        this.onStateUpdated();
      return true;
    }

    public void removeItem(byte index)
    {
      if ((int) index < 0 || (int) index > this.items.Count)
        return;
      this.fillSlot(this.items[(int) index], false);
      if (this.onItemRemoved != null)
        this.onItemRemoved(this.page, index, this.items[(int) index]);
      this.items.RemoveAt((int) index);
      if (this.onStateUpdated == null)
        return;
      this.onStateUpdated();
    }

    public void clear()
    {
      this.items.Clear();
    }

    public void loadSize(byte newWidth, byte newHeight)
    {
      this._width = newWidth;
      this._height = newHeight;
      this.slots = new bool[(int) this.width, (int) this.height];
      for (byte index1 = (byte) 0; (int) index1 < (int) this.width; ++index1)
      {
        for (byte index2 = (byte) 0; (int) index2 < (int) this.height; ++index2)
          this.slots[(int) index1, (int) index2] = false;
      }
      List<ItemJar> list = new List<ItemJar>();
      if (this.items != null)
      {
        for (byte index = (byte) 0; (int) index < this.items.Count; ++index)
        {
          ItemJar jar = this.items[(int) index];
          if ((int) this.width == 0 || (int) this.height == 0 || (int) this.page >= (int) PlayerInventory.SLOTS && ((int) jar.x + (int) jar.size_x > (int) this.width || (int) jar.y + (int) jar.size_y > (int) this.height))
          {
            if (this.onItemDiscarded != null)
              this.onItemDiscarded(this.page, index, jar);
            if (this.onStateUpdated != null)
              this.onStateUpdated();
          }
          else
          {
            this.fillSlot(jar, true);
            list.Add(jar);
          }
        }
      }
      this.items = list;
    }

    public void resize(byte newWidth, byte newHeight)
    {
      this.loadSize(newWidth, newHeight);
      if (this.onItemsResized != null)
        this.onItemsResized(this.page, newWidth, newHeight);
      if (this.onStateUpdated == null)
        return;
      this.onStateUpdated();
    }

    public bool checkSpace(byte old_x, byte old_y, byte new_x, byte new_y, byte size_x, byte size_y, bool checkSame)
    {
      if ((int) this.page < (int) PlayerInventory.SLOTS)
      {
        if (this.items.Count != 0)
          return checkSame;
        return true;
      }
      for (byte index1 = new_x; (int) index1 < (int) new_x + (int) size_x; ++index1)
      {
        for (byte index2 = new_y; (int) index2 < (int) new_y + (int) size_y; ++index2)
        {
          if ((int) index1 >= (int) this.width || (int) index2 >= (int) this.height)
            return false;
          if (this.slots[(int) index1, (int) index2])
          {
            int num1 = (int) index1 - (int) old_x;
            int num2 = (int) index2 - (int) old_y;
            if (!checkSame || num1 < 0 || (num2 < 0 || num1 >= (int) size_x) || num2 >= (int) size_y)
              return false;
          }
        }
      }
      return true;
    }

    public bool checkSwap(byte x, byte y, byte size_x, byte size_y, byte old_x, byte old_y)
    {
      if ((int) this.page < (int) PlayerInventory.SLOTS)
        return true;
      for (byte index1 = x; (int) index1 < (int) x + (int) size_x; ++index1)
      {
        for (byte index2 = y; (int) index2 < (int) y + (int) size_y; ++index2)
        {
          if ((int) index1 >= (int) this.width || (int) index2 >= (int) this.height)
            return false;
          if (this.slots[(int) index1, (int) index2])
          {
            int num1 = (int) index1 - (int) x;
            int num2 = (int) index2 - (int) y;
            if (num1 < 0 || num2 < 0 || (num1 >= (int) old_x || num2 >= (int) old_y))
              return false;
          }
        }
      }
      return true;
    }

    public bool tryFindSpace(byte size_x, byte size_y, out byte x, out byte y)
    {
      x = byte.MaxValue;
      y = byte.MaxValue;
      if ((int) this.page < (int) PlayerInventory.SLOTS)
      {
        x = (byte) 0;
        y = (byte) 0;
        return this.items.Count == 0;
      }
      for (byte index1 = (byte) 0; (int) index1 < (int) this.height - (int) size_y + 1; ++index1)
      {
        for (byte index2 = (byte) 0; (int) index2 < (int) this.width - (int) size_x + 1; ++index2)
        {
          bool flag = false;
          for (byte index3 = (byte) 0; (int) index3 < (int) size_y && !flag; ++index3)
          {
            for (byte index4 = (byte) 0; (int) index4 < (int) size_x; ++index4)
            {
              if (this.slots[(int) index2 + (int) index4, (int) index1 + (int) index3])
              {
                flag = true;
                break;
              }
              if ((int) index4 == (int) size_x - 1 && (int) index3 == (int) size_y - 1)
              {
                x = index2;
                y = index1;
                return true;
              }
            }
          }
        }
      }
      return false;
    }

    private void fillSlot(ItemJar jar, bool isOccupied)
    {
      for (byte index1 = (byte) 0; (int) index1 < (int) jar.size_x; ++index1)
      {
        for (byte index2 = (byte) 0; (int) index2 < (int) jar.size_y; ++index2)
        {
          if ((int) jar.x + (int) index1 < (int) this.width && (int) jar.y + (int) index2 < (int) this.height)
            this.slots[(int) jar.x + (int) index1, (int) jar.y + (int) index2] = isOccupied;
        }
      }
    }
  }
}
