// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ItemClothingAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

namespace SDG.Unturned
{
  public class ItemClothingAsset : ItemAsset
  {
    protected float _armor;

    public float armor
    {
      get
      {
        return this._armor;
      }
    }

    public override bool showQuality
    {
      get
      {
        return true;
      }
    }

    public ItemClothingAsset(Bundle bundle, Data data, ushort id)
      : base(bundle, data, id)
    {
      if (this.isPro)
      {
        this._armor = 1f;
      }
      else
      {
        this._armor = data.readSingle("Armor");
        if ((double) this.armor >= 0.01)
          return;
        this._armor = 1f;
      }
    }
  }
}
