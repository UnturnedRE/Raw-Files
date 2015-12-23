// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.SteamServerInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;

namespace SDG.Unturned
{
  public class SteamServerInfo
  {
    private CSteamID _steamID;
    private string _name;
    private string _map;
    private bool _isPvP;
    private bool _isWorkshop;
    private EGameMode _mode;
    private ECameraMode _camera;
    private int _ping;
    private int _players;
    private int _maxPlayers;
    private bool _isPassworded;
    private bool _isSecure;

    public CSteamID steamID
    {
      get
      {
        return this._steamID;
      }
    }

    public string name
    {
      get
      {
        return this._name;
      }
    }

    public string map
    {
      get
      {
        return this._map;
      }
    }

    public bool isPvP
    {
      get
      {
        return this._isPvP;
      }
    }

    public bool isWorkshop
    {
      get
      {
        return this._isWorkshop;
      }
    }

    public EGameMode mode
    {
      get
      {
        return this._mode;
      }
    }

    public ECameraMode camera
    {
      get
      {
        return this._camera;
      }
    }

    public int ping
    {
      get
      {
        return this._ping;
      }
    }

    public int players
    {
      get
      {
        return this._players;
      }
    }

    public int maxPlayers
    {
      get
      {
        return this._maxPlayers;
      }
    }

    public bool isPassworded
    {
      get
      {
        return this._isPassworded;
      }
    }

    public bool isSecure
    {
      get
      {
        return this._isSecure;
      }
    }

    public SteamServerInfo(gameserveritem_t data)
    {
      this._steamID = data.m_steamID;
      this._name = data.GetServerName();
      this._map = data.GetMap();
      string gameTags = data.GetGameTags();
      if (gameTags.Length > 0)
      {
        this._isPvP = gameTags.IndexOf("PVP") != -1;
        this._isWorkshop = gameTags.IndexOf("WORK") != -1;
        this._mode = gameTags.IndexOf("EASY") == -1 ? (gameTags.IndexOf("HARD") == -1 ? (gameTags.IndexOf("PRO") == -1 ? EGameMode.NORMAL : EGameMode.PRO) : EGameMode.HARD) : EGameMode.EASY;
        this._camera = gameTags.IndexOf("FIRST") == -1 ? (gameTags.IndexOf("THIRD") == -1 ? ECameraMode.BOTH : ECameraMode.THIRD) : ECameraMode.FIRST;
      }
      else
      {
        this._isPvP = true;
        this._mode = EGameMode.PRO;
        this._camera = ECameraMode.FIRST;
      }
      this._ping = data.m_nPing;
      this._players = data.m_nPlayers;
      this._maxPlayers = data.m_nMaxPlayers;
      this._isPassworded = data.m_bPassword;
      this._isSecure = data.m_bSecure;
    }

    public SteamServerInfo(string newName, EGameMode newMode, bool newSecure)
    {
      this._name = newName;
      this._mode = newMode;
      this._isSecure = newSecure;
    }

    public override string ToString()
    {
      return "Name: " + (object) this.name + " Map: " + this.map + " PvP: " + (string) (object) (bool) (this.isPvP ? 1 : 0) + " Mode: " + (string) (object) this.mode + " Ping: " + (string) (object) this.ping + " Players: " + (string) (object) this.players + "/" + (string) (object) this.maxPlayers + " Passworded: " + (string) (object) (bool) (this.isPassworded ? 1 : 0);
    }
  }
}
