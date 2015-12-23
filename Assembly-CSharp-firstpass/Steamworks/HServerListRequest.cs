// Decompiled with JetBrains decompiler
// Type: Steamworks.HServerListRequest
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  public struct HServerListRequest : IEquatable<HServerListRequest>
  {
    public static readonly HServerListRequest Invalid = new HServerListRequest(IntPtr.Zero);
    public IntPtr m_HServerListRequest;

    public HServerListRequest(IntPtr value)
    {
      this.m_HServerListRequest = value;
    }

    public static explicit operator HServerListRequest(IntPtr value)
    {
      return new HServerListRequest(value);
    }

    public static explicit operator IntPtr(HServerListRequest that)
    {
      return that.m_HServerListRequest;
    }

    public static bool operator ==(HServerListRequest x, HServerListRequest y)
    {
      return x.m_HServerListRequest == y.m_HServerListRequest;
    }

    public static bool operator !=(HServerListRequest x, HServerListRequest y)
    {
      return !(x == y);
    }

    public override string ToString()
    {
      return this.m_HServerListRequest.ToString();
    }

    public override bool Equals(object other)
    {
      if (other is HServerListRequest)
        return this == (HServerListRequest) other;
      return false;
    }

    public override int GetHashCode()
    {
      return this.m_HServerListRequest.GetHashCode();
    }

    public bool Equals(HServerListRequest other)
    {
      return this.m_HServerListRequest == other.m_HServerListRequest;
    }
  }
}
