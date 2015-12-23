// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ItemJar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

namespace SDG.Unturned
{
  public class ItemJar
  {
    public byte x;
    public byte y;
    public byte size_x;
    public byte size_y;
    private Item _item;

    public Item item
    {
      get
      {
        return this._item;
      }
    }

    public ItemJar(Item newItem)
    {
      this._item = newItem;
      ItemAsset itemAsset = (ItemAsset) Assets.find(EAssetType.ITEM, this.item.id);
      if (itemAsset == null)
        return;
      this.size_x = itemAsset.size_x;
      this.size_y = itemAsset.size_y;
    }

    public ItemJar(byte new_x, byte new_y, Item newItem)
    {
      this.x = new_x;
      this.y = new_y;
      this._item = newItem;
      ItemAsset itemAsset = (ItemAsset) Assets.find(EAssetType.ITEM, this.item.id);
      if (itemAsset == null)
        return;
      this.size_x = itemAsset.size_x;
      this.size_y = itemAsset.size_y;
    }
  }
}
