// Decompiled with JetBrains decompiler
// Type: Steamworks.PersonaStateChange_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(304)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct PersonaStateChange_t
  {
    public const int k_iCallback = 304;
    public ulong m_ulSteamID;
    public EPersonaChange m_nChangeFlags;
  }
}
