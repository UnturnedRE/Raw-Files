// Decompiled with JetBrains decompiler
// Type: SDG.Provider.UnturnedEconInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

namespace SDG.Provider
{
  public class UnturnedEconInfo
  {
    public string name;
    public string type;
    public string description;
    public string name_color;
    public int itemdefid;
    public bool marketable;
    public int item_id;
    public int item_skin;
    public int item_effect;

    public UnturnedEconInfo()
    {
      this.name = string.Empty;
      this.type = string.Empty;
      this.description = string.Empty;
      this.name_color = string.Empty;
      this.itemdefid = 0;
      this.item_id = 0;
      this.item_skin = 0;
      this.item_effect = 0;
    }
  }
}
