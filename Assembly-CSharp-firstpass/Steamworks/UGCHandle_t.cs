// Decompiled with JetBrains decompiler
// Type: Steamworks.UGCHandle_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  public struct UGCHandle_t : IEquatable<UGCHandle_t>, IComparable<UGCHandle_t>
  {
    public static readonly UGCHandle_t Invalid = new UGCHandle_t(ulong.MaxValue);
    public ulong m_UGCHandle;

    public UGCHandle_t(ulong value)
    {
      this.m_UGCHandle = value;
    }

    public static explicit operator UGCHandle_t(ulong value)
    {
      return new UGCHandle_t(value);
    }

    public static explicit operator ulong(UGCHandle_t that)
    {
      return that.m_UGCHandle;
    }

    public static bool operator ==(UGCHandle_t x, UGCHandle_t y)
    {
      return (long) x.m_UGCHandle == (long) y.m_UGCHandle;
    }

    public static bool operator !=(UGCHandle_t x, UGCHandle_t y)
    {
      return !(x == y);
    }

    public override string ToString()
    {
      return this.m_UGCHandle.ToString();
    }

    public override bool Equals(object other)
    {
      if (other is UGCHandle_t)
        return this == (UGCHandle_t) other;
      return false;
    }

    public override int GetHashCode()
    {
      return this.m_UGCHandle.GetHashCode();
    }

    public bool Equals(UGCHandle_t other)
    {
      return (long) this.m_UGCHandle == (long) other.m_UGCHandle;
    }

    public int CompareTo(UGCHandle_t other)
    {
      return this.m_UGCHandle.CompareTo(other.m_UGCHandle);
    }
  }
}
