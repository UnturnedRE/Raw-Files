// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.SteamPlayerID
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;

namespace SDG.Unturned
{
  public class SteamPlayerID
  {
    private CSteamID _steamID;
    public byte characterID;
    public string playerName;
    public string characterName;
    public string nickName;
    public CSteamID group;

    public CSteamID steamID
    {
      get
      {
        return this._steamID;
      }
    }

    public SteamPlayerID(CSteamID newSteamID, byte newCharacterID, string newPlayerName, string newCharacterName, string newNickName, CSteamID newGroup)
    {
      this._steamID = newSteamID;
      this.characterID = newCharacterID;
      this.playerName = newPlayerName;
      this.characterName = newCharacterName;
      this.nickName = newNickName;
      this.group = newGroup;
    }

    public static bool operator ==(SteamPlayerID playerID_0, SteamPlayerID playerID_1)
    {
      return playerID_0.steamID == playerID_1.steamID;
    }

    public static bool operator !=(SteamPlayerID playerID_0, SteamPlayerID playerID_1)
    {
      return !(playerID_0 == playerID_1);
    }

    public static string operator +(SteamPlayerID playerID, string text)
    {
      return (string) (object) playerID.steamID + (object) text;
    }

    public override string ToString()
    {
      return (string) (object) this.steamID + (object) " " + (string) (object) this.characterID + " " + this.playerName;
    }

    public override int GetHashCode()
    {
      return base.GetHashCode();
    }

    public override bool Equals(object obj)
    {
      return base.Equals(obj);
    }
  }
}
