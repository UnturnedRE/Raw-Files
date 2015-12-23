// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.SteamConnectionInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

namespace SDG.Unturned
{
  public class SteamConnectionInfo
  {
    public uint _ip;
    public ushort _port;
    public string _password;

    public uint ip
    {
      get
      {
        return this._ip;
      }
    }

    public ushort port
    {
      get
      {
        return this._port;
      }
    }

    public string password
    {
      get
      {
        return this._password;
      }
    }

    public SteamConnectionInfo(uint newIP, ushort newPort, string newPassword)
    {
      this._ip = newIP;
      this._port = newPort;
      this._password = newPassword;
    }

    public SteamConnectionInfo(string newIP, ushort newPort, string newPassword)
    {
      this._ip = Parser.getUInt32FromIP(newIP);
      this._port = newPort;
      this._password = newPassword;
    }

    public SteamConnectionInfo(string newIPPort, string newPassword)
    {
      string[] componentsFromSerial = Parser.getComponentsFromSerial(newIPPort, ':');
      this._ip = Parser.getUInt32FromIP(componentsFromSerial[0]);
      this._port = ushort.Parse(componentsFromSerial[1]);
      this._password = newPassword;
    }
  }
}
