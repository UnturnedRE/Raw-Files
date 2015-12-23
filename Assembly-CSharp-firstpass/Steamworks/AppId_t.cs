// Decompiled with JetBrains decompiler
// Type: Steamworks.AppId_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  public struct AppId_t : IEquatable<AppId_t>, IComparable<AppId_t>
  {
    public static readonly AppId_t Invalid = new AppId_t(0U);
    public uint m_AppId;

    public AppId_t(uint value)
    {
      this.m_AppId = value;
    }

    public static explicit operator AppId_t(uint value)
    {
      return new AppId_t(value);
    }

    public static explicit operator uint(AppId_t that)
    {
      return that.m_AppId;
    }

    public static bool operator ==(AppId_t x, AppId_t y)
    {
      return (int) x.m_AppId == (int) y.m_AppId;
    }

    public static bool operator !=(AppId_t x, AppId_t y)
    {
      return !(x == y);
    }

    public override string ToString()
    {
      return this.m_AppId.ToString();
    }

    public override bool Equals(object other)
    {
      if (other is AppId_t)
        return this == (AppId_t) other;
      return false;
    }

    public override int GetHashCode()
    {
      return this.m_AppId.GetHashCode();
    }

    public bool Equals(AppId_t other)
    {
      return (int) this.m_AppId == (int) other.m_AppId;
    }

    public int CompareTo(AppId_t other)
    {
      return this.m_AppId.CompareTo(other.m_AppId);
    }
  }
}
