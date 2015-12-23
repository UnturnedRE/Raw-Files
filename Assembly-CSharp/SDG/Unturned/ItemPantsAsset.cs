// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ItemPantsAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class ItemPantsAsset : ItemBagAsset
  {
    protected Texture2D _pants;
    protected Texture2D _emission;
    protected Texture2D _metallic;

    public Texture2D pants
    {
      get
      {
        return this._pants;
      }
    }

    public Texture2D emission
    {
      get
      {
        return this._emission;
      }
    }

    public Texture2D metallic
    {
      get
      {
        return this._metallic;
      }
    }

    public ItemPantsAsset(Bundle bundle, Data data, ushort id)
      : base(bundle, data, id)
    {
      if (!Dedicator.isDedicated)
      {
        this._pants = (Texture2D) bundle.load("Pants");
        this._emission = (Texture2D) bundle.load("Emission");
        this._metallic = (Texture2D) bundle.load("Metallic");
      }
      bundle.unload();
    }
  }
}
