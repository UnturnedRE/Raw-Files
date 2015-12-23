// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamInventoryResult_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  public struct SteamInventoryResult_t : IEquatable<SteamInventoryResult_t>, IComparable<SteamInventoryResult_t>
  {
    public static readonly SteamInventoryResult_t Invalid = new SteamInventoryResult_t(-1);
    public int m_SteamInventoryResult;

    public SteamInventoryResult_t(int value)
    {
      this.m_SteamInventoryResult = value;
    }

    public static explicit operator SteamInventoryResult_t(int value)
    {
      return new SteamInventoryResult_t(value);
    }

    public static explicit operator int(SteamInventoryResult_t that)
    {
      return that.m_SteamInventoryResult;
    }

    public static bool operator ==(SteamInventoryResult_t x, SteamInventoryResult_t y)
    {
      return x.m_SteamInventoryResult == y.m_SteamInventoryResult;
    }

    public static bool operator !=(SteamInventoryResult_t x, SteamInventoryResult_t y)
    {
      return !(x == y);
    }

    public override string ToString()
    {
      return this.m_SteamInventoryResult.ToString();
    }

    public override bool Equals(object other)
    {
      if (other is SteamInventoryResult_t)
        return this == (SteamInventoryResult_t) other;
      return false;
    }

    public override int GetHashCode()
    {
      return this.m_SteamInventoryResult.GetHashCode();
    }

    public bool Equals(SteamInventoryResult_t other)
    {
      return this.m_SteamInventoryResult == other.m_SteamInventoryResult;
    }

    public int CompareTo(SteamInventoryResult_t other)
    {
      return this.m_SteamInventoryResult.CompareTo(other.m_SteamInventoryResult);
    }
  }
}
