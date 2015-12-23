// Decompiled with JetBrains decompiler
// Type: Steamworks.HSteamPipe
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  public struct HSteamPipe : IEquatable<HSteamPipe>, IComparable<HSteamPipe>
  {
    public int m_HSteamPipe;

    public HSteamPipe(int value)
    {
      this.m_HSteamPipe = value;
    }

    public static explicit operator HSteamPipe(int value)
    {
      return new HSteamPipe(value);
    }

    public static explicit operator int(HSteamPipe that)
    {
      return that.m_HSteamPipe;
    }

    public static bool operator ==(HSteamPipe x, HSteamPipe y)
    {
      return x.m_HSteamPipe == y.m_HSteamPipe;
    }

    public static bool operator !=(HSteamPipe x, HSteamPipe y)
    {
      return !(x == y);
    }

    public override string ToString()
    {
      return this.m_HSteamPipe.ToString();
    }

    public override bool Equals(object other)
    {
      if (other is HSteamPipe)
        return this == (HSteamPipe) other;
      return false;
    }

    public override int GetHashCode()
    {
      return this.m_HSteamPipe.GetHashCode();
    }

    public bool Equals(HSteamPipe other)
    {
      return this.m_HSteamPipe == other.m_HSteamPipe;
    }

    public int CompareTo(HSteamPipe other)
    {
      return this.m_HSteamPipe.CompareTo(other.m_HSteamPipe);
    }
  }
}
