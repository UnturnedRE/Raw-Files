// Decompiled with JetBrains decompiler
// Type: Steamworks.HServerQuery
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  public struct HServerQuery : IEquatable<HServerQuery>, IComparable<HServerQuery>
  {
    public static readonly HServerQuery Invalid = new HServerQuery(-1);
    public int m_HServerQuery;

    public HServerQuery(int value)
    {
      this.m_HServerQuery = value;
    }

    public static explicit operator HServerQuery(int value)
    {
      return new HServerQuery(value);
    }

    public static explicit operator int(HServerQuery that)
    {
      return that.m_HServerQuery;
    }

    public static bool operator ==(HServerQuery x, HServerQuery y)
    {
      return x.m_HServerQuery == y.m_HServerQuery;
    }

    public static bool operator !=(HServerQuery x, HServerQuery y)
    {
      return !(x == y);
    }

    public override string ToString()
    {
      return this.m_HServerQuery.ToString();
    }

    public override bool Equals(object other)
    {
      if (other is HServerQuery)
        return this == (HServerQuery) other;
      return false;
    }

    public override int GetHashCode()
    {
      return this.m_HServerQuery.GetHashCode();
    }

    public bool Equals(HServerQuery other)
    {
      return this.m_HServerQuery == other.m_HServerQuery;
    }

    public int CompareTo(HServerQuery other)
    {
      return this.m_HServerQuery.CompareTo(other.m_HServerQuery);
    }
  }
}
