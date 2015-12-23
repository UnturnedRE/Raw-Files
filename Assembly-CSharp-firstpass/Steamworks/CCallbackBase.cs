// Decompiled with JetBrains decompiler
// Type: Steamworks.CCallbackBase
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
  [StructLayout(LayoutKind.Sequential)]
  internal class CCallbackBase
  {
    public const byte k_ECallbackFlagsRegistered = (byte) 1;
    public const byte k_ECallbackFlagsGameServer = (byte) 2;
    public IntPtr m_vfptr;
    public byte m_nCallbackFlags;
    public int m_iCallback;
  }
}
