// Decompiled with JetBrains decompiler
// Type: Steamworks.HTTPRequestHandle
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  public struct HTTPRequestHandle : IEquatable<HTTPRequestHandle>, IComparable<HTTPRequestHandle>
  {
    public static readonly HTTPRequestHandle Invalid = new HTTPRequestHandle(0U);
    public uint m_HTTPRequestHandle;

    public HTTPRequestHandle(uint value)
    {
      this.m_HTTPRequestHandle = value;
    }

    public static explicit operator HTTPRequestHandle(uint value)
    {
      return new HTTPRequestHandle(value);
    }

    public static explicit operator uint(HTTPRequestHandle that)
    {
      return that.m_HTTPRequestHandle;
    }

    public static bool operator ==(HTTPRequestHandle x, HTTPRequestHandle y)
    {
      return (int) x.m_HTTPRequestHandle == (int) y.m_HTTPRequestHandle;
    }

    public static bool operator !=(HTTPRequestHandle x, HTTPRequestHandle y)
    {
      return !(x == y);
    }

    public override string ToString()
    {
      return this.m_HTTPRequestHandle.ToString();
    }

    public override bool Equals(object other)
    {
      if (other is HTTPRequestHandle)
        return this == (HTTPRequestHandle) other;
      return false;
    }

    public override int GetHashCode()
    {
      return this.m_HTTPRequestHandle.GetHashCode();
    }

    public bool Equals(HTTPRequestHandle other)
    {
      return (int) this.m_HTTPRequestHandle == (int) other.m_HTTPRequestHandle;
    }

    public int CompareTo(HTTPRequestHandle other)
    {
      return this.m_HTTPRequestHandle.CompareTo(other.m_HTTPRequestHandle);
    }
  }
}
