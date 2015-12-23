// Decompiled with JetBrains decompiler
// Type: Steamworks.ScreenshotHandle
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  public struct ScreenshotHandle : IEquatable<ScreenshotHandle>, IComparable<ScreenshotHandle>
  {
    public static readonly ScreenshotHandle Invalid = new ScreenshotHandle(0U);
    public uint m_ScreenshotHandle;

    public ScreenshotHandle(uint value)
    {
      this.m_ScreenshotHandle = value;
    }

    public static explicit operator ScreenshotHandle(uint value)
    {
      return new ScreenshotHandle(value);
    }

    public static explicit operator uint(ScreenshotHandle that)
    {
      return that.m_ScreenshotHandle;
    }

    public static bool operator ==(ScreenshotHandle x, ScreenshotHandle y)
    {
      return (int) x.m_ScreenshotHandle == (int) y.m_ScreenshotHandle;
    }

    public static bool operator !=(ScreenshotHandle x, ScreenshotHandle y)
    {
      return !(x == y);
    }

    public override string ToString()
    {
      return this.m_ScreenshotHandle.ToString();
    }

    public override bool Equals(object other)
    {
      if (other is ScreenshotHandle)
        return this == (ScreenshotHandle) other;
      return false;
    }

    public override int GetHashCode()
    {
      return this.m_ScreenshotHandle.GetHashCode();
    }

    public bool Equals(ScreenshotHandle other)
    {
      return (int) this.m_ScreenshotHandle == (int) other.m_ScreenshotHandle;
    }

    public int CompareTo(ScreenshotHandle other)
    {
      return this.m_ScreenshotHandle.CompareTo(other.m_ScreenshotHandle);
    }
  }
}
