// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.SleekItems
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
  public class SleekItems : Sleek
  {
    public SelectedItem onSelectedItem;
    public GrabbedItem onGrabbedItem;
    public PlacedItem onPlacedItem;
    private SleekGrid grid;
    private byte _page;
    private byte _width;
    private byte _height;
    private List<SleekItem> _items;

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

    public List<SleekItem> items
    {
      get
      {
        return this._items;
      }
    }

    public SleekItems(byte newPage)
    {
      this._page = newPage;
      this._items = new List<SleekItem>();
      this.init();
      this.grid = new SleekGrid();
      this.grid.sizeScale_X = 1f;
      this.grid.sizeScale_Y = 1f;
      this.grid.texture = (Texture) PlayerDashboardInventoryUI.icons.load(!Provider.isPro ? "Grid_Free" : "Grid_Pro");
      this.grid.onClickedGrid = new ClickedGrid(this.onClickedGrid);
      this.add((Sleek) this.grid);
    }

    public override void draw(bool ignoreCulling)
    {
      this.drawChildren(ignoreCulling);
    }

    public void resize(byte newWidth, byte newHeight)
    {
      this._width = newWidth;
      this._height = newHeight;
      this.sizeOffset_X = (int) this.width * 50;
      this.sizeOffset_Y = (int) this.height * 50;
    }

    public void clear()
    {
      foreach (Sleek sleek in this.items.ToArray())
        this.remove(sleek);
      this.items.Clear();
    }

    public void updateItem(byte index, ItemJar jar)
    {
      this.items[(int) index].updateItem(jar);
    }

    public void addItem(ItemJar jar)
    {
      SleekItem sleekItem = new SleekItem(jar);
      sleekItem.positionOffset_X = (int) jar.x * 50;
      sleekItem.positionOffset_Y = (int) jar.y * 50;
      sleekItem.onClickedItem = new ClickedItem(this.onClickedItem);
      sleekItem.onDraggedItem = new DraggedItem(this.onDraggedItem);
      this.add((Sleek) sleekItem);
      this.items.Add(sleekItem);
    }

    public void removeItem(ItemJar jar)
    {
      for (int index = 0; index < this.items.Count; ++index)
      {
        if (this.items[index].positionOffset_X == (int) jar.x * 50 && this.items[index].positionOffset_Y == (int) jar.y * 50)
        {
          this.remove((Sleek) this.items[index]);
          this.items.RemoveAt(index);
          break;
        }
      }
    }

    private void onClickedItem(SleekItem item)
    {
      if (this.onSelectedItem == null)
        return;
      this.onSelectedItem(this.page, (byte) (item.positionOffset_X / 50), (byte) (item.positionOffset_Y / 50));
    }

    private void onDraggedItem(SleekItem item)
    {
      if (this.onGrabbedItem == null)
        return;
      this.onGrabbedItem(this.page, (byte) (item.positionOffset_X / 50), (byte) (item.positionOffset_Y / 50), item);
    }

    private void onClickedGrid(SleekGrid grid)
    {
      byte x = (byte) (((double) PlayerUI.window.mouse_x - (double) this.positionOffset_X - (double) this.parent.frame.x + (double) ((SleekScrollBox) this.parent).state.x) / 50.0);
      byte y = (byte) (((double) PlayerUI.window.mouse_y - (double) this.positionOffset_Y - (double) this.parent.frame.y + (double) ((SleekScrollBox) this.parent).state.y) / 50.0);
      if (this.onPlacedItem == null)
        return;
      this.onPlacedItem(this.page, x, y);
    }
  }
}
