// Decompiled with JetBrains decompiler
// Type: Steamworks.PublishedFileId_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  public struct PublishedFileId_t : IEquatable<PublishedFileId_t>, IComparable<PublishedFileId_t>
  {
    public static readonly PublishedFileId_t Invalid = new PublishedFileId_t(0UL);
    public ulong m_PublishedFileId;

    public PublishedFileId_t(ulong value)
    {
      this.m_PublishedFileId = value;
    }

    public static explicit operator PublishedFileId_t(ulong value)
    {
      return new PublishedFileId_t(value);
    }

    public static explicit operator ulong(PublishedFileId_t that)
    {
      return that.m_PublishedFileId;
    }

    public static bool operator ==(PublishedFileId_t x, PublishedFileId_t y)
    {
      return (long) x.m_PublishedFileId == (long) y.m_PublishedFileId;
    }

    public static bool operator !=(PublishedFileId_t x, PublishedFileId_t y)
    {
      return !(x == y);
    }

    public override string ToString()
    {
      return this.m_PublishedFileId.ToString();
    }

    public override bool Equals(object other)
    {
      if (other is PublishedFileId_t)
        return this == (PublishedFileId_t) other;
      return false;
    }

    public override int GetHashCode()
    {
      return this.m_PublishedFileId.GetHashCode();
    }

    public bool Equals(PublishedFileId_t other)
    {
      return (long) this.m_PublishedFileId == (long) other.m_PublishedFileId;
    }

    public int CompareTo(PublishedFileId_t other)
    {
      return this.m_PublishedFileId.CompareTo(other.m_PublishedFileId);
    }
  }
}
