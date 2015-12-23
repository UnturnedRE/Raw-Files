// Decompiled with JetBrains decompiler
// Type: Steamworks.CGameID
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  public struct CGameID : IEquatable<CGameID>, IComparable<CGameID>
  {
    public ulong m_GameID;

    public CGameID(ulong GameID)
    {
      this.m_GameID = GameID;
    }

    public CGameID(AppId_t nAppID)
    {
      this.m_GameID = 0UL;
      this.SetAppID(nAppID);
    }

    public CGameID(AppId_t nAppID, uint nModID)
    {
      this.m_GameID = 0UL;
      this.SetAppID(nAppID);
      this.SetType(CGameID.EGameIDType.k_EGameIDTypeGameMod);
      this.SetModID(nModID);
    }

    public static explicit operator CGameID(ulong value)
    {
      return new CGameID(value);
    }

    public static explicit operator ulong(CGameID that)
    {
      return that.m_GameID;
    }

    public static bool operator ==(CGameID x, CGameID y)
    {
      return (long) x.m_GameID == (long) y.m_GameID;
    }

    public static bool operator !=(CGameID x, CGameID y)
    {
      return !(x == y);
    }

    public bool IsSteamApp()
    {
      return this.Type() == CGameID.EGameIDType.k_EGameIDTypeApp;
    }

    public bool IsMod()
    {
      return this.Type() == CGameID.EGameIDType.k_EGameIDTypeGameMod;
    }

    public bool IsShortcut()
    {
      return this.Type() == CGameID.EGameIDType.k_EGameIDTypeShortcut;
    }

    public bool IsP2PFile()
    {
      return this.Type() == CGameID.EGameIDType.k_EGameIDTypeP2P;
    }

    public AppId_t AppID()
    {
      return new AppId_t((uint) (this.m_GameID & 16777215UL));
    }

    public CGameID.EGameIDType Type()
    {
      return (CGameID.EGameIDType) ((long) (this.m_GameID >> 24) & (long) byte.MaxValue);
    }

    public uint ModID()
    {
      return (uint) (this.m_GameID >> 32 & (ulong) uint.MaxValue);
    }

    public bool IsValid()
    {
      switch (this.Type())
      {
        case CGameID.EGameIDType.k_EGameIDTypeApp:
          return this.AppID() != AppId_t.Invalid;
        case CGameID.EGameIDType.k_EGameIDTypeGameMod:
          if (this.AppID() != AppId_t.Invalid)
            return ((int) this.ModID() & int.MinValue) != 0;
          return false;
        case CGameID.EGameIDType.k_EGameIDTypeShortcut:
          return ((int) this.ModID() & int.MinValue) != 0;
        case CGameID.EGameIDType.k_EGameIDTypeP2P:
          if (this.AppID() == AppId_t.Invalid)
            return ((int) this.ModID() & int.MinValue) != 0;
          return false;
        default:
          return false;
      }
    }

    public void Reset()
    {
      this.m_GameID = 0UL;
    }

    public void Set(ulong GameID)
    {
      this.m_GameID = GameID;
    }

    private void SetAppID(AppId_t other)
    {
      this.m_GameID = (ulong) ((long) this.m_GameID & -16777216L | (long) (uint) other & 16777215L);
    }

    private void SetType(CGameID.EGameIDType other)
    {
      this.m_GameID = (ulong) ((long) this.m_GameID & -4278190081L | ((long) other & (long) byte.MaxValue) << 24);
    }

    private void SetModID(uint other)
    {
      this.m_GameID = (ulong) ((long) this.m_GameID & (long) uint.MaxValue | ((long) other & (long) uint.MaxValue) << 32);
    }

    public override string ToString()
    {
      return this.m_GameID.ToString();
    }

    public override bool Equals(object other)
    {
      if (other is CGameID)
        return this == (CGameID) other;
      return false;
    }

    public override int GetHashCode()
    {
      return this.m_GameID.GetHashCode();
    }

    public bool Equals(CGameID other)
    {
      return (long) this.m_GameID == (long) other.m_GameID;
    }

    public int CompareTo(CGameID other)
    {
      return this.m_GameID.CompareTo(other.m_GameID);
    }

    public enum EGameIDType
    {
      k_EGameIDTypeApp,
      k_EGameIDTypeGameMod,
      k_EGameIDTypeShortcut,
      k_EGameIDTypeP2P,
    }
  }
}
