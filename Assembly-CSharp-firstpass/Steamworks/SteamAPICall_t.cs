// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamAPICall_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  public struct SteamAPICall_t : IEquatable<SteamAPICall_t>, IComparable<SteamAPICall_t>
  {
    public static readonly SteamAPICall_t Invalid = new SteamAPICall_t(0UL);
    public ulong m_SteamAPICall;

    public SteamAPICall_t(ulong value)
    {
      this.m_SteamAPICall = value;
    }

    public static explicit operator SteamAPICall_t(ulong value)
    {
      return new SteamAPICall_t(value);
    }

    public static explicit operator ulong(SteamAPICall_t that)
    {
      return that.m_SteamAPICall;
    }

    public static bool operator ==(SteamAPICall_t x, SteamAPICall_t y)
    {
      return (long) x.m_SteamAPICall == (long) y.m_SteamAPICall;
    }

    public static bool operator !=(SteamAPICall_t x, SteamAPICall_t y)
    {
      return !(x == y);
    }

    public override string ToString()
    {
      return this.m_SteamAPICall.ToString();
    }

    public override bool Equals(object other)
    {
      if (other is SteamAPICall_t)
        return this == (SteamAPICall_t) other;
      return false;
    }

    public override int GetHashCode()
    {
      return this.m_SteamAPICall.GetHashCode();
    }

    public bool Equals(SteamAPICall_t other)
    {
      return (long) this.m_SteamAPICall == (long) other.m_SteamAPICall;
    }

    public int CompareTo(SteamAPICall_t other)
    {
      return this.m_SteamAPICall.CompareTo(other.m_SteamAPICall);
    }
  }
}
