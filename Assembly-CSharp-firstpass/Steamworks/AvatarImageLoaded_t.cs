﻿// Decompiled with JetBrains decompiler
// Type: Steamworks.AvatarImageLoaded_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(334)]
  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct AvatarImageLoaded_t
  {
    public const int k_iCallback = 334;
    public CSteamID m_steamID;
    public int m_iImage;
    public int m_iWide;
    public int m_iTall;
  }
}
