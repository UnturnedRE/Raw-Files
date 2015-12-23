// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamItemInstanceID_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  public struct SteamItemInstanceID_t : IEquatable<SteamItemInstanceID_t>, IComparable<SteamItemInstanceID_t>
  {
    public static readonly SteamItemInstanceID_t Invalid = new SteamItemInstanceID_t(ulong.MaxValue);
    public ulong m_SteamItemInstanceID;

    public SteamItemInstanceID_t(ulong value)
    {
      this.m_SteamItemInstanceID = value;
    }

    public static explicit operator SteamItemInstanceID_t(ulong value)
    {
      return new SteamItemInstanceID_t(value);
    }

    public static explicit operator ulong(SteamItemInstanceID_t that)
    {
      return that.m_SteamItemInstanceID;
    }

    public static bool operator ==(SteamItemInstanceID_t x, SteamItemInstanceID_t y)
    {
      return (long) x.m_SteamItemInstanceID == (long) y.m_SteamItemInstanceID;
    }

    public static bool operator !=(SteamItemInstanceID_t x, SteamItemInstanceID_t y)
    {
      return !(x == y);
    }

    public override string ToString()
    {
      return this.m_SteamItemInstanceID.ToString();
    }

    public override bool Equals(object other)
    {
      if (other is SteamItemInstanceID_t)
        return this == (SteamItemInstanceID_t) other;
      return false;
    }

    public override int GetHashCode()
    {
      return this.m_SteamItemInstanceID.GetHashCode();
    }

    public bool Equals(SteamItemInstanceID_t other)
    {
      return (long) this.m_SteamItemInstanceID == (long) other.m_SteamItemInstanceID;
    }

    public int CompareTo(SteamItemInstanceID_t other)
    {
      return this.m_SteamItemInstanceID.CompareTo(other.m_SteamItemInstanceID);
    }
  }
}
