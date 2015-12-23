// Decompiled with JetBrains decompiler
// Type: Steamworks.LobbyGameCreated_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(509)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct LobbyGameCreated_t
  {
    public const int k_iCallback = 509;
    public ulong m_ulSteamIDLobby;
    public ulong m_ulSteamIDGameServer;
    public uint m_unIP;
    public ushort m_usPort;
  }
}
