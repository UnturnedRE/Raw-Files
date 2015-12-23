// Decompiled with JetBrains decompiler
// Type: Steamworks.EAuthSessionResponse
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

namespace Steamworks
{
  public enum EAuthSessionResponse
  {
    k_EAuthSessionResponseOK,
    k_EAuthSessionResponseUserNotConnectedToSteam,
    k_EAuthSessionResponseNoLicenseOrExpired,
    k_EAuthSessionResponseVACBanned,
    k_EAuthSessionResponseLoggedInElseWhere,
    k_EAuthSessionResponseVACCheckTimedOut,
    k_EAuthSessionResponseAuthTicketCanceled,
    k_EAuthSessionResponseAuthTicketInvalidAlreadyUsed,
    k_EAuthSessionResponseAuthTicketInvalid,
    k_EAuthSessionResponsePublisherIssuedBan,
  }
}
