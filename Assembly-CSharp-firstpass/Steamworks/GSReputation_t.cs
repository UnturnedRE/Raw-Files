// Decompiled with JetBrains decompiler
// Type: Steamworks.GSReputation_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(209)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct GSReputation_t
  {
    public const int k_iCallback = 209;
    public EResult m_eResult;
    public uint m_unReputationScore;
    [MarshalAs(UnmanagedType.I1)]
    public bool m_bBanned;
    public uint m_unBannedIP;
    public ushort m_usBannedPort;
    public ulong m_ulBannedGameID;
    public uint m_unBanExpires;
  }
}
