// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.SteamPublished
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;

namespace SDG.Unturned
{
  public class SteamPublished
  {
    private string _name;
    private PublishedFileId_t _id;

    public string name
    {
      get
      {
        return this._name;
      }
    }

    public PublishedFileId_t id
    {
      get
      {
        return this._id;
      }
    }

    public SteamPublished(string newName, PublishedFileId_t newID)
    {
      this._name = newName;
      this._id = newID;
    }
  }
}
