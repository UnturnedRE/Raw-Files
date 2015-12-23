// Decompiled with JetBrains decompiler
// Type: SDG.Provider.Services.Matchmaking.MatchmakingFilter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

namespace SDG.Provider.Services.Matchmaking
{
  public class MatchmakingFilter : IMatchmakingFilter
  {
    public string key { get; protected set; }

    public string value { get; protected set; }

    public MatchmakingFilter(string newKey, string newValue)
    {
      this.key = newKey;
      this.value = newValue;
    }
  }
}
