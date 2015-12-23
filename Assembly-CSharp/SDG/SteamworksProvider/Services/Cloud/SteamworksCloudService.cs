// Decompiled with JetBrains decompiler
// Type: SDG.SteamworksProvider.Services.Cloud.SteamworksCloudService
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using SDG.Provider.Services;
using SDG.Provider.Services.Cloud;
using Steamworks;
using System;

namespace SDG.SteamworksProvider.Services.Cloud
{
  public class SteamworksCloudService : Service, ICloudService, IService
  {
    public bool read(string path, byte[] data)
    {
      if (path == null)
        throw new ArgumentNullException("path");
      if (data == null)
        throw new ArgumentNullException("data");
      int fileSize = SteamRemoteStorage.GetFileSize(path);
      return data.Length >= fileSize && SteamRemoteStorage.FileRead(path, data, fileSize) == fileSize;
    }

    public bool write(string path, byte[] data, int size)
    {
      if (path == null)
        throw new ArgumentNullException("path");
      if (data == null)
        throw new ArgumentNullException("data");
      return SteamRemoteStorage.FileWrite(path, data, size);
    }

    public bool getSize(string path, out int size)
    {
      if (path == null)
        throw new ArgumentNullException("path");
      size = SteamRemoteStorage.GetFileSize(path);
      return true;
    }

    public bool exists(string path, out bool exists)
    {
      if (path == null)
        throw new ArgumentNullException("path");
      exists = SteamRemoteStorage.FileExists(path);
      return true;
    }

    public bool delete(string path)
    {
      if (path == null)
        throw new ArgumentNullException("path");
      return SteamRemoteStorage.FileDelete(path);
    }
  }
}
