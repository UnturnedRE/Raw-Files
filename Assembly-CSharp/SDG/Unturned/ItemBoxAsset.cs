// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ItemBoxAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

namespace SDG.Unturned
{
  public class ItemBoxAsset : ItemAsset
  {
    protected int _generate;
    protected int _destroy;
    protected int[] _drops;

    public int generate
    {
      get
      {
        return this._generate;
      }
    }

    public int destroy
    {
      get
      {
        return this._destroy;
      }
    }

    public int[] drops
    {
      get
      {
        return this._drops;
      }
    }

    public ItemBoxAsset(Bundle bundle, Data data, ushort id)
      : base(bundle, data, id)
    {
      this._generate = data.readInt32("Generate");
      this._destroy = data.readInt32("Destroy");
      this._drops = new int[data.readInt32("Drops")];
      for (int index = 0; index < this.drops.Length; ++index)
        this.drops[index] = data.readInt32("Drop_" + (object) index);
      bundle.unload();
    }
  }
}
