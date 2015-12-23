// Decompiled with JetBrains decompiler
// Type: Steamworks.HAuthTicket
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  public struct HAuthTicket : IEquatable<HAuthTicket>, IComparable<HAuthTicket>
  {
    public static readonly HAuthTicket Invalid = new HAuthTicket(0U);
    public uint m_HAuthTicket;

    public HAuthTicket(uint value)
    {
      this.m_HAuthTicket = value;
    }

    public static explicit operator HAuthTicket(uint value)
    {
      return new HAuthTicket(value);
    }

    public static explicit operator uint(HAuthTicket that)
    {
      return that.m_HAuthTicket;
    }

    public static bool operator ==(HAuthTicket x, HAuthTicket y)
    {
      return (int) x.m_HAuthTicket == (int) y.m_HAuthTicket;
    }

    public static bool operator !=(HAuthTicket x, HAuthTicket y)
    {
      return !(x == y);
    }

    public override string ToString()
    {
      return this.m_HAuthTicket.ToString();
    }

    public override bool Equals(object other)
    {
      if (other is HAuthTicket)
        return this == (HAuthTicket) other;
      return false;
    }

    public override int GetHashCode()
    {
      return this.m_HAuthTicket.GetHashCode();
    }

    public bool Equals(HAuthTicket other)
    {
      return (int) this.m_HAuthTicket == (int) other.m_HAuthTicket;
    }

    public int CompareTo(HAuthTicket other)
    {
      return this.m_HAuthTicket.CompareTo(other.m_HAuthTicket);
    }
  }
}
