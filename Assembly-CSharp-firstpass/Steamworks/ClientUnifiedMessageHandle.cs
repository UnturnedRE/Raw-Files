﻿// Decompiled with JetBrains decompiler
// Type: Steamworks.ClientUnifiedMessageHandle
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  public struct ClientUnifiedMessageHandle : IEquatable<ClientUnifiedMessageHandle>, IComparable<ClientUnifiedMessageHandle>
  {
    public static readonly ClientUnifiedMessageHandle Invalid = new ClientUnifiedMessageHandle(0UL);
    public ulong m_ClientUnifiedMessageHandle;

    public ClientUnifiedMessageHandle(ulong value)
    {
      this.m_ClientUnifiedMessageHandle = value;
    }

    public static explicit operator ClientUnifiedMessageHandle(ulong value)
    {
      return new ClientUnifiedMessageHandle(value);
    }

    public static explicit operator ulong(ClientUnifiedMessageHandle that)
    {
      return that.m_ClientUnifiedMessageHandle;
    }

    public static bool operator ==(ClientUnifiedMessageHandle x, ClientUnifiedMessageHandle y)
    {
      return (long) x.m_ClientUnifiedMessageHandle == (long) y.m_ClientUnifiedMessageHandle;
    }

    public static bool operator !=(ClientUnifiedMessageHandle x, ClientUnifiedMessageHandle y)
    {
      return !(x == y);
    }

    public override string ToString()
    {
      return this.m_ClientUnifiedMessageHandle.ToString();
    }

    public override bool Equals(object other)
    {
      if (other is ClientUnifiedMessageHandle)
        return this == (ClientUnifiedMessageHandle) other;
      return false;
    }

    public override int GetHashCode()
    {
      return this.m_ClientUnifiedMessageHandle.GetHashCode();
    }

    public bool Equals(ClientUnifiedMessageHandle other)
    {
      return (long) this.m_ClientUnifiedMessageHandle == (long) other.m_ClientUnifiedMessageHandle;
    }

    public int CompareTo(ClientUnifiedMessageHandle other)
    {
      return this.m_ClientUnifiedMessageHandle.CompareTo(other.m_ClientUnifiedMessageHandle);
    }
  }
}
