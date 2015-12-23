// Decompiled with JetBrains decompiler
// Type: Steamworks.AccountID_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  public struct AccountID_t : IEquatable<AccountID_t>, IComparable<AccountID_t>
  {
    public uint m_AccountID;

    public AccountID_t(uint value)
    {
      this.m_AccountID = value;
    }

    public static explicit operator AccountID_t(uint value)
    {
      return new AccountID_t(value);
    }

    public static explicit operator uint(AccountID_t that)
    {
      return that.m_AccountID;
    }

    public static bool operator ==(AccountID_t x, AccountID_t y)
    {
      return (int) x.m_AccountID == (int) y.m_AccountID;
    }

    public static bool operator !=(AccountID_t x, AccountID_t y)
    {
      return !(x == y);
    }

    public override string ToString()
    {
      return this.m_AccountID.ToString();
    }

    public override bool Equals(object other)
    {
      if (other is AccountID_t)
        return this == (AccountID_t) other;
      return false;
    }

    public override int GetHashCode()
    {
      return this.m_AccountID.GetHashCode();
    }

    public bool Equals(AccountID_t other)
    {
      return (int) this.m_AccountID == (int) other.m_AccountID;
    }

    public int CompareTo(AccountID_t other)
    {
      return this.m_AccountID.CompareTo(other.m_AccountID);
    }
  }
}
