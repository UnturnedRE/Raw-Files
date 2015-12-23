// Decompiled with JetBrains decompiler
// Type: Steamworks.HSteamUser
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  public struct HSteamUser : IEquatable<HSteamUser>, IComparable<HSteamUser>
  {
    public int m_HSteamUser;

    public HSteamUser(int value)
    {
      this.m_HSteamUser = value;
    }

    public static explicit operator HSteamUser(int value)
    {
      return new HSteamUser(value);
    }

    public static explicit operator int(HSteamUser that)
    {
      return that.m_HSteamUser;
    }

    public static bool operator ==(HSteamUser x, HSteamUser y)
    {
      return x.m_HSteamUser == y.m_HSteamUser;
    }

    public static bool operator !=(HSteamUser x, HSteamUser y)
    {
      return !(x == y);
    }

    public override string ToString()
    {
      return this.m_HSteamUser.ToString();
    }

    public override bool Equals(object other)
    {
      if (other is HSteamUser)
        return this == (HSteamUser) other;
      return false;
    }

    public override int GetHashCode()
    {
      return this.m_HSteamUser.GetHashCode();
    }

    public bool Equals(HSteamUser other)
    {
      return this.m_HSteamUser == other.m_HSteamUser;
    }

    public int CompareTo(HSteamUser other)
    {
      return this.m_HSteamUser.CompareTo(other.m_HSteamUser);
    }
  }
}
