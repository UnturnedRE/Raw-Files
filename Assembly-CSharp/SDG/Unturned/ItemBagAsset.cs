// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ItemBagAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

namespace SDG.Unturned
{
  public class ItemBagAsset : ItemClothingAsset
  {
    private byte _width;
    private byte _height;

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

    public ItemBagAsset(Bundle bundle, Data data, ushort id)
      : base(bundle, data, id)
    {
      if (this.isPro)
        return;
      this._width = data.readByte("Width");
      this._height = data.readByte("Height");
    }
  }
}
