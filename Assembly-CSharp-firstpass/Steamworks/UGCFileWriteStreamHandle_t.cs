// Decompiled with JetBrains decompiler
// Type: Steamworks.UGCFileWriteStreamHandle_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  public struct UGCFileWriteStreamHandle_t : IEquatable<UGCFileWriteStreamHandle_t>, IComparable<UGCFileWriteStreamHandle_t>
  {
    public static readonly UGCFileWriteStreamHandle_t Invalid = new UGCFileWriteStreamHandle_t(ulong.MaxValue);
    public ulong m_UGCFileWriteStreamHandle;

    public UGCFileWriteStreamHandle_t(ulong value)
    {
      this.m_UGCFileWriteStreamHandle = value;
    }

    public static explicit operator UGCFileWriteStreamHandle_t(ulong value)
    {
      return new UGCFileWriteStreamHandle_t(value);
    }

    public static explicit operator ulong(UGCFileWriteStreamHandle_t that)
    {
      return that.m_UGCFileWriteStreamHandle;
    }

    public static bool operator ==(UGCFileWriteStreamHandle_t x, UGCFileWriteStreamHandle_t y)
    {
      return (long) x.m_UGCFileWriteStreamHandle == (long) y.m_UGCFileWriteStreamHandle;
    }

    public static bool operator !=(UGCFileWriteStreamHandle_t x, UGCFileWriteStreamHandle_t y)
    {
      return !(x == y);
    }

    public override string ToString()
    {
      return this.m_UGCFileWriteStreamHandle.ToString();
    }

    public override bool Equals(object other)
    {
      if (other is UGCFileWriteStreamHandle_t)
        return this == (UGCFileWriteStreamHandle_t) other;
      return false;
    }

    public override int GetHashCode()
    {
      return this.m_UGCFileWriteStreamHandle.GetHashCode();
    }

    public bool Equals(UGCFileWriteStreamHandle_t other)
    {
      return (long) this.m_UGCFileWriteStreamHandle == (long) other.m_UGCFileWriteStreamHandle;
    }

    public int CompareTo(UGCFileWriteStreamHandle_t other)
    {
      return this.m_UGCFileWriteStreamHandle.CompareTo(other.m_UGCFileWriteStreamHandle);
    }
  }
}
