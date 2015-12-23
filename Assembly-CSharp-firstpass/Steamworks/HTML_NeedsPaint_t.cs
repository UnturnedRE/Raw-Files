// Decompiled with JetBrains decompiler
// Type: Steamworks.HTML_NeedsPaint_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(4502)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct HTML_NeedsPaint_t
  {
    public const int k_iCallback = 4502;
    public HHTMLBrowser unBrowserHandle;
    public IntPtr pBGRA;
    public uint unWide;
    public uint unTall;
    public uint unUpdateX;
    public uint unUpdateY;
    public uint unUpdateWide;
    public uint unUpdateTall;
    public uint unScrollX;
    public uint unScrollY;
    public float flPageScale;
    public uint unPageSerial;
  }
}
