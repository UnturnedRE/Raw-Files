// Decompiled with JetBrains decompiler
// Type: Steamworks.UserAchievementStored_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(1103)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct UserAchievementStored_t
  {
    public const int k_iCallback = 1103;
    public ulong m_nGameID;
    [MarshalAs(UnmanagedType.I1)]
    public bool m_bGroupAchievement;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
    public string m_rgchAchievementName;
    public uint m_nCurProgress;
    public uint m_nMaxProgress;
  }
}
