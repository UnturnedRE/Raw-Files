// Decompiled with JetBrains decompiler
// Type: Steamworks.PublishedFileUpdateHandle_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  public struct PublishedFileUpdateHandle_t : IEquatable<PublishedFileUpdateHandle_t>, IComparable<PublishedFileUpdateHandle_t>
  {
    public static readonly PublishedFileUpdateHandle_t Invalid = new PublishedFileUpdateHandle_t(ulong.MaxValue);
    public ulong m_PublishedFileUpdateHandle;

    public PublishedFileUpdateHandle_t(ulong value)
    {
      this.m_PublishedFileUpdateHandle = value;
    }

    public static explicit operator PublishedFileUpdateHandle_t(ulong value)
    {
      return new PublishedFileUpdateHandle_t(value);
    }

    public static explicit operator ulong(PublishedFileUpdateHandle_t that)
    {
      return that.m_PublishedFileUpdateHandle;
    }

    public static bool operator ==(PublishedFileUpdateHandle_t x, PublishedFileUpdateHandle_t y)
    {
      return (long) x.m_PublishedFileUpdateHandle == (long) y.m_PublishedFileUpdateHandle;
    }

    public static bool operator !=(PublishedFileUpdateHandle_t x, PublishedFileUpdateHandle_t y)
    {
      return !(x == y);
    }

    public override string ToString()
    {
      return this.m_PublishedFileUpdateHandle.ToString();
    }

    public override bool Equals(object other)
    {
      if (other is PublishedFileUpdateHandle_t)
        return this == (PublishedFileUpdateHandle_t) other;
      return false;
    }

    public override int GetHashCode()
    {
      return this.m_PublishedFileUpdateHandle.GetHashCode();
    }

    public bool Equals(PublishedFileUpdateHandle_t other)
    {
      return (long) this.m_PublishedFileUpdateHandle == (long) other.m_PublishedFileUpdateHandle;
    }

    public int CompareTo(PublishedFileUpdateHandle_t other)
    {
      return this.m_PublishedFileUpdateHandle.CompareTo(other.m_PublishedFileUpdateHandle);
    }
  }
}
