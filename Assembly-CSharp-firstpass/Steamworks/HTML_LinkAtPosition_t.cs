﻿// Decompiled with JetBrains decompiler
// Type: Steamworks.HTML_LinkAtPosition_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(4513)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct HTML_LinkAtPosition_t
  {
    public const int k_iCallback = 4513;
    public HHTMLBrowser unBrowserHandle;
    public uint x;
    public uint y;
    public string pchURL;
    [MarshalAs(UnmanagedType.I1)]
    public bool bInput;
    [MarshalAs(UnmanagedType.I1)]
    public bool bLiveLink;
  }
}
