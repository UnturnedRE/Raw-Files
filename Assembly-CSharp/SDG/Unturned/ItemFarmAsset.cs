// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ItemFarmAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

namespace SDG.Unturned
{
  public class ItemFarmAsset : ItemBarricadeAsset
  {
    protected uint _growth;
    protected ushort _grow;

    public uint growth
    {
      get
      {
        return this._growth;
      }
    }

    public ushort grow
    {
      get
      {
        return this._grow;
      }
    }

    public ItemFarmAsset(Bundle bundle, Data data, ushort id)
      : base(bundle, data, id)
    {
      this._growth = data.readUInt32("Growth");
      this._grow = data.readUInt16("Grow");
    }
  }
}
