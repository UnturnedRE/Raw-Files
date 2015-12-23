// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamItemDef_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  public struct SteamItemDef_t : IEquatable<SteamItemDef_t>, IComparable<SteamItemDef_t>
  {
    public int m_SteamItemDef;

    public SteamItemDef_t(int value)
    {
      this.m_SteamItemDef = value;
    }

    public static explicit operator SteamItemDef_t(int value)
    {
      return new SteamItemDef_t(value);
    }

    public static explicit operator int(SteamItemDef_t that)
    {
      return that.m_SteamItemDef;
    }

    public static bool operator ==(SteamItemDef_t x, SteamItemDef_t y)
    {
      return x.m_SteamItemDef == y.m_SteamItemDef;
    }

    public static bool operator !=(SteamItemDef_t x, SteamItemDef_t y)
    {
      return !(x == y);
    }

    public override string ToString()
    {
      return this.m_SteamItemDef.ToString();
    }

    public override bool Equals(object other)
    {
      if (other is SteamItemDef_t)
        return this == (SteamItemDef_t) other;
      return false;
    }

    public override int GetHashCode()
    {
      return this.m_SteamItemDef.GetHashCode();
    }

    public bool Equals(SteamItemDef_t other)
    {
      return this.m_SteamItemDef == other.m_SteamItemDef;
    }

    public int CompareTo(SteamItemDef_t other)
    {
      return this.m_SteamItemDef.CompareTo(other.m_SteamItemDef);
    }
  }
}
