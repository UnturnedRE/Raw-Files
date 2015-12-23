// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.SleekSlot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class SleekSlot : Sleek
  {
    public SelectedItem onSelectedItem;
    public GrabbedItem onGrabbedItem;
    public PlacedItem onPlacedItem;
    private SleekImageTexture image;
    private SleekItem _item;
    private byte _page;

    public SleekItem item
    {
      get
      {
        return this._item;
      }
    }

    public byte page
    {
      get
      {
        return this._page;
      }
    }

    public SleekSlot(byte newPage)
    {
      this._page = newPage;
      this.init();
      this.sizeOffset_X = 250;
      this.sizeOffset_Y = 150;
      this.image = new SleekImageTexture();
      this.image.sizeScale_X = 1f;
      this.image.sizeScale_Y = 1f;
      this.image.texture = (Texture) PlayerDashboardInventoryUI.icons.load(!Provider.isPro ? "Slot_" + (object) this.page + "_Free" : "Slot_" + (object) this.page + "_Pro");
      this.add((Sleek) this.image);
    }

    public override void draw(bool ignoreCulling)
    {
      this.drawChildren(ignoreCulling);
    }

    public void select()
    {
      if (this.onPlacedItem == null)
        return;
      this.onPlacedItem(this.page, (byte) 0, (byte) 0);
    }

    public void updateItem(ItemJar jar)
    {
      this.item.updateItem(jar);
    }

    public void applyItem(ItemJar jar)
    {
      if (this.item != null)
        this.remove((Sleek) this.item);
      if (jar == null)
        return;
      this._item = new SleekItem(jar);
      this.item.positionOffset_X = (int) -jar.size_x * 25;
      this.item.positionOffset_Y = (int) -jar.size_y * 25;
      this.item.positionScale_X = 0.5f;
      this.item.positionScale_Y = 0.5f;
      this.item.updateHotkey(this.page);
      this.item.onClickedItem = new ClickedItem(this.onClickedItem);
      this.item.onDraggedItem = new DraggedItem(this.onDraggedItem);
      this.add((Sleek) this.item);
    }

    private void onClickedItem(SleekItem item)
    {
      if (this.onSelectedItem == null)
        return;
      this.onSelectedItem(this.page, (byte) 0, (byte) 0);
    }

    private void onDraggedItem(SleekItem item)
    {
      if (this.onGrabbedItem == null)
        return;
      this.onGrabbedItem(this.page, (byte) 0, (byte) 0, item);
    }
  }
}
