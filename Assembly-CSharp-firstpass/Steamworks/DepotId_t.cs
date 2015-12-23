// Decompiled with JetBrains decompiler
// Type: Steamworks.DepotId_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  public struct DepotId_t : IEquatable<DepotId_t>, IComparable<DepotId_t>
  {
    public static readonly DepotId_t Invalid = new DepotId_t(0U);
    public uint m_DepotId;

    public DepotId_t(uint value)
    {
      this.m_DepotId = value;
    }

    public static explicit operator DepotId_t(uint value)
    {
      return new DepotId_t(value);
    }

    public static explicit operator uint(DepotId_t that)
    {
      return that.m_DepotId;
    }

    public static bool operator ==(DepotId_t x, DepotId_t y)
    {
      return (int) x.m_DepotId == (int) y.m_DepotId;
    }

    public static bool operator !=(DepotId_t x, DepotId_t y)
    {
      return !(x == y);
    }

    public override string ToString()
    {
      return this.m_DepotId.ToString();
    }

    public override bool Equals(object other)
    {
      if (other is DepotId_t)
        return this == (DepotId_t) other;
      return false;
    }

    public override int GetHashCode()
    {
      return this.m_DepotId.GetHashCode();
    }

    public bool Equals(DepotId_t other)
    {
      return (int) this.m_DepotId == (int) other.m_DepotId;
    }

    public int CompareTo(DepotId_t other)
    {
      return this.m_DepotId.CompareTo(other.m_DepotId);
    }
  }
}
