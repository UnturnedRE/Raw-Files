// Decompiled with JetBrains decompiler
// Type: Steamworks.UGCUpdateHandle_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  public struct UGCUpdateHandle_t : IEquatable<UGCUpdateHandle_t>, IComparable<UGCUpdateHandle_t>
  {
    public static readonly UGCUpdateHandle_t Invalid = new UGCUpdateHandle_t(ulong.MaxValue);
    public ulong m_UGCUpdateHandle;

    public UGCUpdateHandle_t(ulong value)
    {
      this.m_UGCUpdateHandle = value;
    }

    public static explicit operator UGCUpdateHandle_t(ulong value)
    {
      return new UGCUpdateHandle_t(value);
    }

    public static explicit operator ulong(UGCUpdateHandle_t that)
    {
      return that.m_UGCUpdateHandle;
    }

    public static bool operator ==(UGCUpdateHandle_t x, UGCUpdateHandle_t y)
    {
      return (long) x.m_UGCUpdateHandle == (long) y.m_UGCUpdateHandle;
    }

    public static bool operator !=(UGCUpdateHandle_t x, UGCUpdateHandle_t y)
    {
      return !(x == y);
    }

    public override string ToString()
    {
      return this.m_UGCUpdateHandle.ToString();
    }

    public override bool Equals(object other)
    {
      if (other is UGCUpdateHandle_t)
        return this == (UGCUpdateHandle_t) other;
      return false;
    }

    public override int GetHashCode()
    {
      return this.m_UGCUpdateHandle.GetHashCode();
    }

    public bool Equals(UGCUpdateHandle_t other)
    {
      return (long) this.m_UGCUpdateHandle == (long) other.m_UGCUpdateHandle;
    }

    public int CompareTo(UGCUpdateHandle_t other)
    {
      return this.m_UGCUpdateHandle.CompareTo(other.m_UGCUpdateHandle);
    }
  }
}
