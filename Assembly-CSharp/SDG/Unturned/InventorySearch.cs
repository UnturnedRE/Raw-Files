// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.InventorySearch
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

namespace SDG.Unturned
{
  public class InventorySearch
  {
    private byte _page;
    private ItemJar _jar;

    public byte page
    {
      get
      {
        return this._page;
      }
    }

    public ItemJar jar
    {
      get
      {
        return this._jar;
      }
    }

    public InventorySearch(byte newPage, ItemJar newJar)
    {
      this._page = newPage;
      this._jar = newJar;
    }
  }
}
