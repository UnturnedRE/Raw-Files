// Decompiled with JetBrains decompiler
// Type: Steamworks.LeaderboardScoreUploaded_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(1106)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct LeaderboardScoreUploaded_t
  {
    public const int k_iCallback = 1106;
    public byte m_bSuccess;
    public SteamLeaderboard_t m_hSteamLeaderboard;
    public int m_nScore;
    public byte m_bScoreChanged;
    public int m_nGlobalRankNew;
    public int m_nGlobalRankPrevious;
  }
}
