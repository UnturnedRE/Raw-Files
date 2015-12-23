// Decompiled with JetBrains decompiler
// Type: Steamworks.EPersonaChange
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  [Flags]
  public enum EPersonaChange
  {
    k_EPersonaChangeName = 1,
    k_EPersonaChangeStatus = 2,
    k_EPersonaChangeComeOnline = 4,
    k_EPersonaChangeGoneOffline = 8,
    k_EPersonaChangeGamePlayed = 16,
    k_EPersonaChangeGameServer = 32,
    k_EPersonaChangeAvatar = 64,
    k_EPersonaChangeJoinedSource = 128,
    k_EPersonaChangeLeftSource = 256,
    k_EPersonaChangeRelationshipChanged = 512,
    k_EPersonaChangeNameFirstSet = 1024,
    k_EPersonaChangeFacebookInfo = 2048,
    k_EPersonaChangeNickname = 4096,
    k_EPersonaChangeSteamLevel = 8192,
  }
}
