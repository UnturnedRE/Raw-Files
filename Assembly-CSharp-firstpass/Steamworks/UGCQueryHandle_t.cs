// Decompiled with JetBrains decompiler
// Type: Steamworks.UGCQueryHandle_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  public struct UGCQueryHandle_t : IEquatable<UGCQueryHandle_t>, IComparable<UGCQueryHandle_t>
  {
    public static readonly UGCQueryHandle_t Invalid = new UGCQueryHandle_t(ulong.MaxValue);
    public ulong m_UGCQueryHandle;

    public UGCQueryHandle_t(ulong value)
    {
      this.m_UGCQueryHandle = value;
    }

    public static explicit operator UGCQueryHandle_t(ulong value)
    {
      return new UGCQueryHandle_t(value);
    }

    public static explicit operator ulong(UGCQueryHandle_t that)
    {
      return that.m_UGCQueryHandle;
    }

    public static bool operator ==(UGCQueryHandle_t x, UGCQueryHandle_t y)
    {
      return (long) x.m_UGCQueryHandle == (long) y.m_UGCQueryHandle;
    }

    public static bool operator !=(UGCQueryHandle_t x, UGCQueryHandle_t y)
    {
      return !(x == y);
    }

    public override string ToString()
    {
      return this.m_UGCQueryHandle.ToString();
    }

    public override bool Equals(object other)
    {
      if (other is UGCQueryHandle_t)
        return this == (UGCQueryHandle_t) other;
      return false;
    }

    public override int GetHashCode()
    {
      return this.m_UGCQueryHandle.GetHashCode();
    }

    public bool Equals(UGCQueryHandle_t other)
    {
      return (long) this.m_UGCQueryHandle == (long) other.m_UGCQueryHandle;
    }

    public int CompareTo(UGCQueryHandle_t other)
    {
      return this.m_UGCQueryHandle.CompareTo(other.m_UGCQueryHandle);
    }
  }
}
