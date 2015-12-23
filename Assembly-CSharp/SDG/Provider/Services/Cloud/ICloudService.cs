// Decompiled with JetBrains decompiler
// Type: SDG.Provider.Services.Cloud.ICloudService
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using SDG.Provider.Services;

namespace SDG.Provider.Services.Cloud
{
  public interface ICloudService : IService
  {
    bool read(string path, byte[] data);

    bool write(string path, byte[] data, int size);

    bool getSize(string path, out int size);

    bool exists(string path, out bool exists);

    bool delete(string path);
  }
}
