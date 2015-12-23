// Decompiled with JetBrains decompiler
// Type: Steamworks.FriendsGroupID_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  public struct FriendsGroupID_t : IEquatable<FriendsGroupID_t>, IComparable<FriendsGroupID_t>
  {
    public static readonly FriendsGroupID_t Invalid = new FriendsGroupID_t((short) -1);
    public short m_FriendsGroupID;

    public FriendsGroupID_t(short value)
    {
      this.m_FriendsGroupID = value;
    }

    public static explicit operator FriendsGroupID_t(short value)
    {
      return new FriendsGroupID_t(value);
    }

    public static explicit operator short(FriendsGroupID_t that)
    {
      return that.m_FriendsGroupID;
    }

    public static bool operator ==(FriendsGroupID_t x, FriendsGroupID_t y)
    {
      return (int) x.m_FriendsGroupID == (int) y.m_FriendsGroupID;
    }

    public static bool operator !=(FriendsGroupID_t x, FriendsGroupID_t y)
    {
      return !(x == y);
    }

    public override string ToString()
    {
      return this.m_FriendsGroupID.ToString();
    }

    public override bool Equals(object other)
    {
      if (other is FriendsGroupID_t)
        return this == (FriendsGroupID_t) other;
      return false;
    }

    public override int GetHashCode()
    {
      return this.m_FriendsGroupID.GetHashCode();
    }

    public bool Equals(FriendsGroupID_t other)
    {
      return (int) this.m_FriendsGroupID == (int) other.m_FriendsGroupID;
    }

    public int CompareTo(FriendsGroupID_t other)
    {
      return this.m_FriendsGroupID.CompareTo(other.m_FriendsGroupID);
    }
  }
}
