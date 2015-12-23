// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.SteamContent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;

namespace SDG.Unturned
{
  public class SteamContent
  {
    private PublishedFileId_t _publishedFileID;
    private string _path;
    private ESteamUGCType _type;

    public PublishedFileId_t publishedFileID
    {
      get
      {
        return this._publishedFileID;
      }
    }

    public string path
    {
      get
      {
        return this._path;
      }
    }

    public ESteamUGCType type
    {
      get
      {
        return this._type;
      }
    }

    public SteamContent(PublishedFileId_t newPublishedFileID, string newPath, ESteamUGCType newType)
    {
      this._publishedFileID = newPublishedFileID;
      this._path = newPath;
      this._type = newType;
    }
  }
}
