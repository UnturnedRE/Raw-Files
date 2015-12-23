// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.SteamBlacklistID
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;

namespace SDG.Unturned
{
  public class SteamBlacklistID
  {
    private CSteamID _playerID;
    public CSteamID judgeID;
    public string reason;
    public uint duration;
    public uint banned;

    public CSteamID playerID
    {
      get
      {
        return this._playerID;
      }
    }

    public bool isExpired
    {
      get
      {
        return Provider.time > this.banned + this.duration;
      }
    }

    public SteamBlacklistID(CSteamID newPlayerID, CSteamID newJudgeID, string newReason, uint newDuration, uint newBanned)
    {
      this._playerID = newPlayerID;
      this.judgeID = newJudgeID;
      this.reason = newReason;
      this.duration = newDuration;
      this.banned = newBanned;
    }

    public uint getTime()
    {
      return this.duration - (Provider.time - this.banned);
    }
  }
}
