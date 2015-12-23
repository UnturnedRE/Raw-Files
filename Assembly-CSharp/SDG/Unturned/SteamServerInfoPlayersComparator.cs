// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.SteamServerInfoPlayersComparator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

namespace SDG.Unturned
{
  public class SteamServerInfoPlayersComparator : IComparer<SteamServerInfo>
  {
    public int Compare(SteamServerInfo a, SteamServerInfo b)
    {
      return b.players - a.players;
    }
  }
}
