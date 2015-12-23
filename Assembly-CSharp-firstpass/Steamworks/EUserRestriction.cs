// Decompiled with JetBrains decompiler
// Type: Steamworks.EUserRestriction
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

namespace Steamworks
{
  public enum EUserRestriction
  {
    k_nUserRestrictionNone = 0,
    k_nUserRestrictionUnknown = 1,
    k_nUserRestrictionAnyChat = 2,
    k_nUserRestrictionVoiceChat = 4,
    k_nUserRestrictionGroupChat = 8,
    k_nUserRestrictionRating = 16,
    k_nUserRestrictionGameInvites = 32,
    k_nUserRestrictionTrading = 64,
  }
}
