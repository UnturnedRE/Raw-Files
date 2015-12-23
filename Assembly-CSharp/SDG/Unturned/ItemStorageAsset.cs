// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ItemStorageAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

namespace SDG.Unturned
{
  public class ItemStorageAsset : ItemBarricadeAsset
  {
    protected byte _storage_x;
    protected byte _storage_y;

    public byte storage_x
    {
      get
      {
        return this._storage_x;
      }
    }

    public byte storage_y
    {
      get
      {
        return this._storage_y;
      }
    }

    public ItemStorageAsset(Bundle bundle, Data data, ushort id)
      : base(bundle, data, id)
    {
      this._storage_x = data.readByte("Storage_X");
      if ((int) this.storage_x < 1)
        this._storage_x = (byte) 1;
      this._storage_y = data.readByte("Storage_Y");
      if ((int) this.storage_y >= 1)
        return;
      this._storage_y = (byte) 1;
    }
  }
}
