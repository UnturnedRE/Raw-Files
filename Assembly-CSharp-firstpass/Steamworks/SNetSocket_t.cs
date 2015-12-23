// Decompiled with JetBrains decompiler
// Type: Steamworks.SNetSocket_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  public struct SNetSocket_t : IEquatable<SNetSocket_t>, IComparable<SNetSocket_t>
  {
    public uint m_SNetSocket;

    public SNetSocket_t(uint value)
    {
      this.m_SNetSocket = value;
    }

    public static explicit operator SNetSocket_t(uint value)
    {
      return new SNetSocket_t(value);
    }

    public static explicit operator uint(SNetSocket_t that)
    {
      return that.m_SNetSocket;
    }

    public static bool operator ==(SNetSocket_t x, SNetSocket_t y)
    {
      return (int) x.m_SNetSocket == (int) y.m_SNetSocket;
    }

    public static bool operator !=(SNetSocket_t x, SNetSocket_t y)
    {
      return !(x == y);
    }

    public override string ToString()
    {
      return this.m_SNetSocket.ToString();
    }

    public override bool Equals(object other)
    {
      if (other is SNetSocket_t)
        return this == (SNetSocket_t) other;
      return false;
    }

    public override int GetHashCode()
    {
      return this.m_SNetSocket.GetHashCode();
    }

    public bool Equals(SNetSocket_t other)
    {
      return (int) this.m_SNetSocket == (int) other.m_SNetSocket;
    }

    public int CompareTo(SNetSocket_t other)
    {
      return this.m_SNetSocket.CompareTo(other.m_SNetSocket);
    }
  }
}
