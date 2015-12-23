// Decompiled with JetBrains decompiler
// Type: Steamworks.EChatSteamIDInstanceFlags
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  [Flags]
  public enum EChatSteamIDInstanceFlags
  {
    k_EChatAccountInstanceMask = 4095,
    k_EChatInstanceFlagClan = 524288,
    k_EChatInstanceFlagLobby = 262144,
    k_EChatInstanceFlagMMSLobby = 131072,
  }
}
