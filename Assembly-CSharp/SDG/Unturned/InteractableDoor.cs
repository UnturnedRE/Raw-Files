// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.InteractableDoor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using System;
using UnityEngine;

namespace SDG.Unturned
{
  public class InteractableDoor : Interactable
  {
    private CSteamID _owner;
    private CSteamID _group;
    private bool _isOpen;
    private bool isLocked;
    private float opened;

    public CSteamID owner
    {
      get
      {
        return this._owner;
      }
    }

    public CSteamID group
    {
      get
      {
        return this._group;
      }
    }

    public bool isOpen
    {
      get
      {
        return this._isOpen;
      }
    }

    public bool isOpenable
    {
      get
      {
        return (double) Time.realtimeSinceStartup - (double) this.opened > 0.75;
      }
    }

    public bool checkToggle(CSteamID enemyPlayer, CSteamID enemyGroup)
    {
      if (Provider.isServer && !Dedicator.isDedicated || (!this.isLocked || enemyPlayer == this.owner))
        return true;
      if (this.group != CSteamID.Nil)
        return enemyGroup == this.group;
      return false;
    }

    public void updateToggle(bool newOpen)
    {
      this.opened = Time.realtimeSinceStartup;
      this._isOpen = newOpen;
      if (this.isOpen)
        this.transform.parent.parent.GetComponent<Animation>().Play("Open");
      else
        this.transform.parent.parent.GetComponent<Animation>().Play("Close");
      if (Dedicator.isDedicated)
        return;
      this.transform.parent.parent.GetComponent<AudioSource>().Play();
    }

    public override void updateState(Asset asset, byte[] state)
    {
      this.isLocked = ((ItemBarricadeAsset) asset).isLocked;
      this._owner = new CSteamID(BitConverter.ToUInt64(state, 0));
      this._group = new CSteamID(BitConverter.ToUInt64(state, 8));
      this._isOpen = (int) state[16] == 1;
      if (this.isOpen)
        this.transform.parent.parent.GetComponent<Animation>().Play("Open");
      else
        this.transform.parent.parent.GetComponent<Animation>().Play("Close");
    }

    public override bool checkUseable()
    {
      return this.checkToggle(Provider.client, Characters.active.group);
    }

    public override void use()
    {
      BarricadeManager.toggleDoor(this.transform.parent.parent);
    }

    public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
    {
      message = !this.checkUseable() ? EPlayerMessage.LOCKED : (!this.isOpen ? EPlayerMessage.DOOR_OPEN : EPlayerMessage.DOOR_CLOSE);
      text = string.Empty;
      color = Color.white;
      return true;
    }
  }
}
