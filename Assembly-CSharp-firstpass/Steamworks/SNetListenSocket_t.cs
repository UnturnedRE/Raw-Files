// Decompiled with JetBrains decompiler
// Type: Steamworks.SNetListenSocket_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  public struct SNetListenSocket_t : IEquatable<SNetListenSocket_t>, IComparable<SNetListenSocket_t>
  {
    public uint m_SNetListenSocket;

    public SNetListenSocket_t(uint value)
    {
      this.m_SNetListenSocket = value;
    }

    public static explicit operator SNetListenSocket_t(uint value)
    {
      return new SNetListenSocket_t(value);
    }

    public static explicit operator uint(SNetListenSocket_t that)
    {
      return that.m_SNetListenSocket;
    }

    public static bool operator ==(SNetListenSocket_t x, SNetListenSocket_t y)
    {
      return (int) x.m_SNetListenSocket == (int) y.m_SNetListenSocket;
    }

    public static bool operator !=(SNetListenSocket_t x, SNetListenSocket_t y)
    {
      return !(x == y);
    }

    public override string ToString()
    {
      return this.m_SNetListenSocket.ToString();
    }

    public override bool Equals(object other)
    {
      if (other is SNetListenSocket_t)
        return this == (SNetListenSocket_t) other;
      return false;
    }

    public override int GetHashCode()
    {
      return this.m_SNetListenSocket.GetHashCode();
    }

    public bool Equals(SNetListenSocket_t other)
    {
      return (int) this.m_SNetListenSocket == (int) other.m_SNetListenSocket;
    }

    public int CompareTo(SNetListenSocket_t other)
    {
      return this.m_SNetListenSocket.CompareTo(other.m_SNetListenSocket);
    }
  }
}
