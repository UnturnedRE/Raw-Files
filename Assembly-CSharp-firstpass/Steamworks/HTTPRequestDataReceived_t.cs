// Decompiled with JetBrains decompiler
// Type: Steamworks.HTTPRequestDataReceived_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(2103)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct HTTPRequestDataReceived_t
  {
    public const int k_iCallback = 2103;
    public HTTPRequestHandle m_hRequest;
    public ulong m_ulContextValue;
    public uint m_cOffset;
    public uint m_cBytesReceived;
  }
}
