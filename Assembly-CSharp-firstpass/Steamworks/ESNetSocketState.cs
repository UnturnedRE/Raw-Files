// Decompiled with JetBrains decompiler
// Type: Steamworks.ESNetSocketState
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

namespace Steamworks
{
  public enum ESNetSocketState
  {
    k_ESNetSocketStateInvalid = 0,
    k_ESNetSocketStateConnected = 1,
    k_ESNetSocketStateInitiated = 10,
    k_ESNetSocketStateLocalCandidatesFound = 11,
    k_ESNetSocketStateReceivedRemoteCandidates = 12,
    k_ESNetSocketStateChallengeHandshake = 15,
    k_ESNetSocketStateDisconnecting = 21,
    k_ESNetSocketStateLocalDisconnect = 22,
    k_ESNetSocketStateTimeoutDuringConnect = 23,
    k_ESNetSocketStateRemoteEndDisconnected = 24,
    k_ESNetSocketStateConnectionBroken = 25,
  }
}
