// Decompiled with JetBrains decompiler
// Type: Steamworks.ManifestId_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  public struct ManifestId_t : IEquatable<ManifestId_t>, IComparable<ManifestId_t>
  {
    public static readonly ManifestId_t Invalid = new ManifestId_t(0UL);
    public ulong m_ManifestId;

    public ManifestId_t(ulong value)
    {
      this.m_ManifestId = value;
    }

    public static explicit operator ManifestId_t(ulong value)
    {
      return new ManifestId_t(value);
    }

    public static explicit operator ulong(ManifestId_t that)
    {
      return that.m_ManifestId;
    }

    public static bool operator ==(ManifestId_t x, ManifestId_t y)
    {
      return (long) x.m_ManifestId == (long) y.m_ManifestId;
    }

    public static bool operator !=(ManifestId_t x, ManifestId_t y)
    {
      return !(x == y);
    }

    public override string ToString()
    {
      return this.m_ManifestId.ToString();
    }

    public override bool Equals(object other)
    {
      if (other is ManifestId_t)
        return this == (ManifestId_t) other;
      return false;
    }

    public override int GetHashCode()
    {
      return this.m_ManifestId.GetHashCode();
    }

    public bool Equals(ManifestId_t other)
    {
      return (long) this.m_ManifestId == (long) other.m_ManifestId;
    }

    public int CompareTo(ManifestId_t other)
    {
      return this.m_ManifestId.CompareTo(other.m_ManifestId);
    }
  }
}
