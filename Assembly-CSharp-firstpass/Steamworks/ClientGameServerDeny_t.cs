// Decompiled with JetBrains decompiler
// Type: Steamworks.ClientGameServerDeny_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(113)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct ClientGameServerDeny_t
  {
    public const int k_iCallback = 113;
    public uint m_uAppID;
    public uint m_unGameServerIP;
    public ushort m_usGameServerPort;
    public ushort m_bSecure;
    public uint m_uReason;
  }
}
