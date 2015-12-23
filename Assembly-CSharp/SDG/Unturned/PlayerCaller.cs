// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.PlayerCaller
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

namespace SDG.Unturned
{
  public class PlayerCaller : SteamCaller
  {
    protected Player _player;

    public Player player
    {
      get
      {
        return this._player;
      }
    }

    private void Awake()
    {
      this._channel = this.GetComponent<SteamChannel>();
      this._player = this.GetComponent<Player>();
    }
  }
}
