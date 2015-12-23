// Decompiled with JetBrains decompiler
// Type: Steamworks.servernetadr_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

namespace Steamworks
{
  public struct servernetadr_t
  {
    private ushort m_usConnectionPort;
    private ushort m_usQueryPort;
    private uint m_unIP;

    public static bool operator <(servernetadr_t x, servernetadr_t y)
    {
      if (x.m_unIP < y.m_unIP)
        return true;
      if ((int) x.m_unIP == (int) y.m_unIP)
        return (int) x.m_usQueryPort < (int) y.m_usQueryPort;
      return false;
    }

    public static bool operator >(servernetadr_t x, servernetadr_t y)
    {
      if (x.m_unIP > y.m_unIP)
        return true;
      if ((int) x.m_unIP == (int) y.m_unIP)
        return (int) x.m_usQueryPort > (int) y.m_usQueryPort;
      return false;
    }

    public static bool operator ==(servernetadr_t x, servernetadr_t y)
    {
      if ((int) x.m_unIP == (int) y.m_unIP && (int) x.m_usQueryPort == (int) y.m_usQueryPort)
        return (int) x.m_usConnectionPort == (int) y.m_usConnectionPort;
      return false;
    }

    public static bool operator !=(servernetadr_t x, servernetadr_t y)
    {
      return !(x == y);
    }

    public void Init(uint ip, ushort usQueryPort, ushort usConnectionPort)
    {
      this.m_unIP = ip;
      this.m_usQueryPort = usQueryPort;
      this.m_usConnectionPort = usConnectionPort;
    }

    public ushort GetQueryPort()
    {
      return this.m_usQueryPort;
    }

    public void SetQueryPort(ushort usPort)
    {
      this.m_usQueryPort = usPort;
    }

    public ushort GetConnectionPort()
    {
      return this.m_usConnectionPort;
    }

    public void SetConnectionPort(ushort usPort)
    {
      this.m_usConnectionPort = usPort;
    }

    public uint GetIP()
    {
      return this.m_unIP;
    }

    public void SetIP(uint unIP)
    {
      this.m_unIP = unIP;
    }

    public string GetConnectionAddressString()
    {
      return servernetadr_t.ToString(this.m_unIP, this.m_usConnectionPort);
    }

    public string GetQueryAddressString()
    {
      return servernetadr_t.ToString(this.m_unIP, this.m_usQueryPort);
    }

    public static string ToString(uint unIP, ushort usPort)
    {
      return string.Format("{0}.{1}.{2}.{3}:{4}", (object) (ulong) ((long) (unIP >> 24) & (long) byte.MaxValue), (object) (ulong) ((long) (unIP >> 16) & (long) byte.MaxValue), (object) (ulong) ((long) (unIP >> 8) & (long) byte.MaxValue), (object) (ulong) ((long) unIP & (long) byte.MaxValue), (object) usPort);
    }

    public override bool Equals(object other)
    {
      if (other is servernetadr_t)
        return this == (servernetadr_t) other;
      return false;
    }

    public override int GetHashCode()
    {
      return this.m_unIP.GetHashCode() + this.m_usQueryPort.GetHashCode() + this.m_usConnectionPort.GetHashCode();
    }

    public bool Equals(servernetadr_t other)
    {
      if ((int) this.m_unIP == (int) other.m_unIP && (int) this.m_usQueryPort == (int) other.m_usQueryPort)
        return (int) this.m_usConnectionPort == (int) other.m_usConnectionPort;
      return false;
    }

    public int CompareTo(servernetadr_t other)
    {
      return this.m_unIP.CompareTo(other.m_unIP) + this.m_usQueryPort.CompareTo(other.m_usQueryPort) + this.m_usConnectionPort.CompareTo(other.m_usConnectionPort);
    }
  }
}
