﻿// Decompiled with JetBrains decompiler
// Type: Steamworks.GSStatsReceived_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(1800)]
  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct GSStatsReceived_t
  {
    public const int k_iCallback = 1800;
    public EResult m_eResult;
    public CSteamID m_steamIDUser;
  }
}
