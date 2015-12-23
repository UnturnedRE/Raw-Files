// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ItemGearAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

namespace SDG.Unturned
{
  public class ItemGearAsset : ItemClothingAsset
  {
    protected bool _hasHair;
    protected bool _hasBeard;

    public bool hasHair
    {
      get
      {
        return this._hasHair;
      }
    }

    public bool hasBeard
    {
      get
      {
        return this._hasBeard;
      }
    }

    public ItemGearAsset(Bundle bundle, Data data, ushort id)
      : base(bundle, data, id)
    {
      this._hasHair = data.has("Hair");
      this._hasBeard = data.has("Beard");
    }
  }
}
