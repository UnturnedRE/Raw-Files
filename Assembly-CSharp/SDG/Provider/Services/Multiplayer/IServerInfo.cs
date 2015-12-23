// Decompiled with JetBrains decompiler
// Type: SDG.Provider.Services.Multiplayer.IServerInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using SDG.Provider.Services.Community;

namespace SDG.Provider.Services.Multiplayer
{
  public interface IServerInfo
  {
    ICommunityEntity entity { get; }

    string name { get; }

    byte players { get; }

    byte capacity { get; }

    int ping { get; }
  }
}
