// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.SteamAdminID
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;

namespace SDG.Unturned
{
  public class SteamAdminID
  {
    private CSteamID _playerID;
    public CSteamID judgeID;

    public CSteamID playerID
    {
      get
      {
        return this._playerID;
      }
    }

    public SteamAdminID(CSteamID newPlayerID, CSteamID newJudgeID)
    {
      this._playerID = newPlayerID;
      this.judgeID = newJudgeID;
    }
  }
}
