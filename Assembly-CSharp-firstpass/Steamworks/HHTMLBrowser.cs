// Decompiled with JetBrains decompiler
// Type: Steamworks.HHTMLBrowser
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  public struct HHTMLBrowser : IEquatable<HHTMLBrowser>, IComparable<HHTMLBrowser>
  {
    public static readonly HHTMLBrowser Invalid = new HHTMLBrowser(0U);
    public uint m_HHTMLBrowser;

    public HHTMLBrowser(uint value)
    {
      this.m_HHTMLBrowser = value;
    }

    public static explicit operator HHTMLBrowser(uint value)
    {
      return new HHTMLBrowser(value);
    }

    public static explicit operator uint(HHTMLBrowser that)
    {
      return that.m_HHTMLBrowser;
    }

    public static bool operator ==(HHTMLBrowser x, HHTMLBrowser y)
    {
      return (int) x.m_HHTMLBrowser == (int) y.m_HHTMLBrowser;
    }

    public static bool operator !=(HHTMLBrowser x, HHTMLBrowser y)
    {
      return !(x == y);
    }

    public override string ToString()
    {
      return this.m_HHTMLBrowser.ToString();
    }

    public override bool Equals(object other)
    {
      if (other is HHTMLBrowser)
        return this == (HHTMLBrowser) other;
      return false;
    }

    public override int GetHashCode()
    {
      return this.m_HHTMLBrowser.GetHashCode();
    }

    public bool Equals(HHTMLBrowser other)
    {
      return (int) this.m_HHTMLBrowser == (int) other.m_HHTMLBrowser;
    }

    public int CompareTo(HHTMLBrowser other)
    {
      return this.m_HHTMLBrowser.CompareTo(other.m_HHTMLBrowser);
    }
  }
}
