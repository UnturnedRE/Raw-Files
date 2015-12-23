// Decompiled with JetBrains decompiler
// Type: Steamworks.HTTPCookieContainerHandle
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  public struct HTTPCookieContainerHandle : IEquatable<HTTPCookieContainerHandle>, IComparable<HTTPCookieContainerHandle>
  {
    public static readonly HTTPCookieContainerHandle Invalid = new HTTPCookieContainerHandle(0U);
    public uint m_HTTPCookieContainerHandle;

    public HTTPCookieContainerHandle(uint value)
    {
      this.m_HTTPCookieContainerHandle = value;
    }

    public static explicit operator HTTPCookieContainerHandle(uint value)
    {
      return new HTTPCookieContainerHandle(value);
    }

    public static explicit operator uint(HTTPCookieContainerHandle that)
    {
      return that.m_HTTPCookieContainerHandle;
    }

    public static bool operator ==(HTTPCookieContainerHandle x, HTTPCookieContainerHandle y)
    {
      return (int) x.m_HTTPCookieContainerHandle == (int) y.m_HTTPCookieContainerHandle;
    }

    public static bool operator !=(HTTPCookieContainerHandle x, HTTPCookieContainerHandle y)
    {
      return !(x == y);
    }

    public override string ToString()
    {
      return this.m_HTTPCookieContainerHandle.ToString();
    }

    public override bool Equals(object other)
    {
      if (other is HTTPCookieContainerHandle)
        return this == (HTTPCookieContainerHandle) other;
      return false;
    }

    public override int GetHashCode()
    {
      return this.m_HTTPCookieContainerHandle.GetHashCode();
    }

    public bool Equals(HTTPCookieContainerHandle other)
    {
      return (int) this.m_HTTPCookieContainerHandle == (int) other.m_HTTPCookieContainerHandle;
    }

    public int CompareTo(HTTPCookieContainerHandle other)
    {
      return this.m_HTTPCookieContainerHandle.CompareTo(other.m_HTTPCookieContainerHandle);
    }
  }
}
