// Decompiled with JetBrains decompiler
// Type: SDG.SteamworksProvider.SteamworksAppInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

namespace SDG.SteamworksProvider
{
  public class SteamworksAppInfo
  {
    public uint id { get; protected set; }

    public string name { get; protected set; }

    public string version { get; protected set; }

    public bool isDedicated { get; protected set; }

    public SteamworksAppInfo(uint newID, string newName, string newVersion, bool newIsDedicated)
    {
      this.id = newID;
      this.name = newName;
      this.version = newVersion;
      this.isDedicated = newIsDedicated;
    }
  }
}
