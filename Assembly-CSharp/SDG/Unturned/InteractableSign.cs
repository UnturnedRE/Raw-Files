// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.InteractableSign
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace SDG.Unturned
{
  public class InteractableSign : Interactable
  {
    private CSteamID _owner;
    private CSteamID _group;
    private string _text;
    private bool isLocked;
    private Text label;

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

    public string text
    {
      get
      {
        return this._text;
      }
    }

    public bool checkUpdate(CSteamID enemyPlayer, CSteamID enemyGroup)
    {
      if (Provider.isServer && !Dedicator.isDedicated || (!this.isLocked || enemyPlayer == this.owner))
        return true;
      if (this.group != CSteamID.Nil)
        return enemyGroup == this.group;
      return false;
    }

    public void updateText(string newText)
    {
      this._text = newText;
      if (!((UnityEngine.Object) this.label != (UnityEngine.Object) null))
        return;
      this.label.text = this.text;
    }

    public override void updateState(Asset asset, byte[] state)
    {
      this.isLocked = ((ItemBarricadeAsset) asset).isLocked;
      if (!Dedicator.isDedicated)
        this.label = this.transform.FindChild("Canvas").FindChild("Label").GetComponent<Text>();
      this._owner = new CSteamID(BitConverter.ToUInt64(state, 0));
      this._group = new CSteamID(BitConverter.ToUInt64(state, 8));
      byte num = state[16];
      this.updateText(Encoding.UTF8.GetString(state, 17, (int) num));
    }

    public override bool checkUseable()
    {
      if (this.checkUpdate(Provider.client, Characters.active.group))
        return !PlayerUI.window.showCursor;
      return false;
    }

    public override void use()
    {
      PlayerBarricadeSignUI.open(this);
      PlayerLifeUI.close();
    }

    public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
    {
      message = !this.checkUseable() ? EPlayerMessage.LOCKED : EPlayerMessage.USE;
      text = string.Empty;
      color = Color.white;
      return !PlayerUI.window.showCursor;
    }
  }
}
