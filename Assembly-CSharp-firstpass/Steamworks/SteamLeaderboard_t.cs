// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamLeaderboard_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  public struct SteamLeaderboard_t : IEquatable<SteamLeaderboard_t>, IComparable<SteamLeaderboard_t>
  {
    public ulong m_SteamLeaderboard;

    public SteamLeaderboard_t(ulong value)
    {
      this.m_SteamLeaderboard = value;
    }

    public static explicit operator SteamLeaderboard_t(ulong value)
    {
      return new SteamLeaderboard_t(value);
    }

    public static explicit operator ulong(SteamLeaderboard_t that)
    {
      return that.m_SteamLeaderboard;
    }

    public static bool operator ==(SteamLeaderboard_t x, SteamLeaderboard_t y)
    {
      return (long) x.m_SteamLeaderboard == (long) y.m_SteamLeaderboard;
    }

    public static bool operator !=(SteamLeaderboard_t x, SteamLeaderboard_t y)
    {
      return !(x == y);
    }

    public override string ToString()
    {
      return this.m_SteamLeaderboard.ToString();
    }

    public override bool Equals(object other)
    {
      if (other is SteamLeaderboard_t)
        return this == (SteamLeaderboard_t) other;
      return false;
    }

    public override int GetHashCode()
    {
      return this.m_SteamLeaderboard.GetHashCode();
    }

    public bool Equals(SteamLeaderboard_t other)
    {
      return (long) this.m_SteamLeaderboard == (long) other.m_SteamLeaderboard;
    }

    public int CompareTo(SteamLeaderboard_t other)
    {
      return this.m_SteamLeaderboard.CompareTo(other.m_SteamLeaderboard);
    }
  }
}
