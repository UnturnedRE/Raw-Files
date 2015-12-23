// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamLeaderboardEntries_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  public struct SteamLeaderboardEntries_t : IEquatable<SteamLeaderboardEntries_t>, IComparable<SteamLeaderboardEntries_t>
  {
    public ulong m_SteamLeaderboardEntries;

    public SteamLeaderboardEntries_t(ulong value)
    {
      this.m_SteamLeaderboardEntries = value;
    }

    public static explicit operator SteamLeaderboardEntries_t(ulong value)
    {
      return new SteamLeaderboardEntries_t(value);
    }

    public static explicit operator ulong(SteamLeaderboardEntries_t that)
    {
      return that.m_SteamLeaderboardEntries;
    }

    public static bool operator ==(SteamLeaderboardEntries_t x, SteamLeaderboardEntries_t y)
    {
      return (long) x.m_SteamLeaderboardEntries == (long) y.m_SteamLeaderboardEntries;
    }

    public static bool operator !=(SteamLeaderboardEntries_t x, SteamLeaderboardEntries_t y)
    {
      return !(x == y);
    }

    public override string ToString()
    {
      return this.m_SteamLeaderboardEntries.ToString();
    }

    public override bool Equals(object other)
    {
      if (other is SteamLeaderboardEntries_t)
        return this == (SteamLeaderboardEntries_t) other;
      return false;
    }

    public override int GetHashCode()
    {
      return this.m_SteamLeaderboardEntries.GetHashCode();
    }

    public bool Equals(SteamLeaderboardEntries_t other)
    {
      return (long) this.m_SteamLeaderboardEntries == (long) other.m_SteamLeaderboardEntries;
    }

    public int CompareTo(SteamLeaderboardEntries_t other)
    {
      return this.m_SteamLeaderboardEntries.CompareTo(other.m_SteamLeaderboardEntries);
    }
  }
}
