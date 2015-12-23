// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamAPIWarningMessageHook_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;
using System.Text;

namespace Steamworks
{
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate void SteamAPIWarningMessageHook_t(int nSeverity, StringBuilder pchDebugText);
}
