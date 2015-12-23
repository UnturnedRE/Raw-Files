// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ItemOpticAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

namespace SDG.Unturned
{
  public class ItemOpticAsset : ItemAsset
  {
    private float _zoom;

    public float zoom
    {
      get
      {
        return this._zoom;
      }
    }

    public ItemOpticAsset(Bundle bundle, Data data, ushort id)
      : base(bundle, data, id)
    {
      this._zoom = 90f / (float) data.readByte("Zoom");
      bundle.unload();
    }
  }
}
