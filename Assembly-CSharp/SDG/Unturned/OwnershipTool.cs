// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.OwnershipTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;

namespace SDG.Unturned
{
  internal class OwnershipTool
  {
    public static bool checkToggle(ulong player, ulong group)
    {
      if (Dedicator.isDedicated)
        return false;
      return OwnershipTool.checkToggle(Provider.client, player, Characters.active.group, group);
    }

    public static bool checkToggle(CSteamID player_0, ulong player_1, CSteamID group_0, ulong group_1)
    {
      if (Provider.isServer && !Dedicator.isDedicated || (long) player_0.m_SteamID == (long) player_1)
        return true;
      if (group_0 != CSteamID.Nil)
        return (long) group_0.m_SteamID == (long) group_1;
      return false;
    }
  }
}
