﻿// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.SteamServerInfoNameComparator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

namespace SDG.Unturned
{
  public class SteamServerInfoNameComparator : IComparer<SteamServerInfo>
  {
    public int Compare(SteamServerInfo a, SteamServerInfo b)
    {
      return a.name.CompareTo(b.name);
    }
  }
}
