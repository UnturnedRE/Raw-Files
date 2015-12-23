// Decompiled with JetBrains decompiler
// Type: Steamworks.FavoritesListChanged_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(502)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct FavoritesListChanged_t
  {
    public const int k_iCallback = 502;
    public uint m_nIP;
    public uint m_nQueryPort;
    public uint m_nConnPort;
    public uint m_nAppID;
    public uint m_nFlags;
    [MarshalAs(UnmanagedType.I1)]
    public bool m_bAdd;
    public AccountID_t m_unAccountId;
  }
}
