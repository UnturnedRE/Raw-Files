﻿// Decompiled with JetBrains decompiler
// Type: Steamworks.GSClientGroupStatus_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(208)]
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct GSClientGroupStatus_t
  {
    public const int k_iCallback = 208;
    public CSteamID m_SteamIDUser;
    public CSteamID m_SteamIDGroup;
    [MarshalAs(UnmanagedType.I1)]
    public bool m_bMember;
    [MarshalAs(UnmanagedType.I1)]
    public bool m_bOfficer;
  }
}
