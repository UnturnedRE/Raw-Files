// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.Asset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

namespace SDG.Unturned
{
  public class Asset
  {
    protected string _name;
    protected ushort _id;

    public string name
    {
      get
      {
        return this._name;
      }
    }

    public ushort id
    {
      get
      {
        return this._id;
      }
    }

    public Asset(Bundle bundle, ushort id)
    {
      this._name = bundle.name;
      this._id = id;
    }
  }
}
