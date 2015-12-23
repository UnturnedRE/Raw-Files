// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.InteractableBed
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using System;
using UnityEngine;

namespace SDG.Unturned
{
  public class InteractableBed : Interactable
  {
    private CSteamID _owner;
    private float claimed;

    public CSteamID owner
    {
      get
      {
        return this._owner;
      }
    }

    public bool isClaimed
    {
      get
      {
        return this.owner != CSteamID.Nil;
      }
    }

    public bool isClaimable
    {
      get
      {
        return (double) Time.realtimeSinceStartup - (double) this.claimed > 0.75;
      }
    }

    public bool checkClaim(CSteamID enemy)
    {
      if (Provider.isServer && !Dedicator.isDedicated || !this.isClaimed)
        return true;
      return enemy == this.owner;
    }

    public void updateClaim(CSteamID newOwner)
    {
      this.claimed = Time.realtimeSinceStartup;
      this._owner = newOwner;
    }

    public override void updateState(Asset asset, byte[] state)
    {
      this._owner = new CSteamID(BitConverter.ToUInt64(state, 0));
    }

    public override bool checkUseable()
    {
      return this.checkClaim(Provider.client);
    }

    public override void use()
    {
      BarricadeManager.claimBed(this.transform);
    }

    public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
    {
      message = !this.checkUseable() ? EPlayerMessage.BED_CLAIMED : (!this.isClaimed ? EPlayerMessage.BED_ON : EPlayerMessage.BED_OFF);
      text = string.Empty;
      color = Color.white;
      return true;
    }
  }
}
