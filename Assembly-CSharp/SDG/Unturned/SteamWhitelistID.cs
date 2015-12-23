// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.SteamWhitelistID
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;

namespace SDG.Unturned
{
  public class SteamWhitelistID
  {
    private CSteamID _steamID;
    public string tag;
    public CSteamID judgeID;

    public CSteamID steamID
    {
      get
      {
        return this._steamID;
      }
    }

    public SteamWhitelistID(CSteamID newSteamID, string newTag, CSteamID newJudgeID)
    {
      this._steamID = newSteamID;
      this.tag = newTag;
      this.judgeID = newJudgeID;
    }
  }
}
