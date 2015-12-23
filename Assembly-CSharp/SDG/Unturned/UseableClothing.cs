// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.UseableClothing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
  public class UseableClothing : Useable
  {
    private float startedUse;
    private float useTime;
    private bool isUsing;

    private bool isUseable
    {
      get
      {
        return (double) Time.realtimeSinceStartup - (double) this.startedUse > (double) this.useTime;
      }
    }

    private void wear()
    {
      this.player.animator.play("Use", false);
    }

    [SteamCall]
    public void askWear(CSteamID steamID)
    {
      if (!this.channel.checkServer(steamID) || !this.player.equipment.isEquipped)
        return;
      this.wear();
    }

    public override void startPrimary()
    {
      if (this.player.equipment.isBusy)
        return;
      this.player.equipment.isBusy = true;
      this.startedUse = Time.realtimeSinceStartup;
      this.isUsing = true;
      if (Provider.isServer)
        this.channel.send("askWear", ESteamCall.NOT_OWNER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER);
      this.wear();
    }

    public override void equip()
    {
      this.player.animator.play("Equip", true);
      this.useTime = this.player.animator.getAnimationLength("Use");
    }

    public override void simulate(uint simulation)
    {
      if (!this.isUsing || !this.isUseable)
        return;
      this.player.equipment.isBusy = false;
      this.isUsing = false;
      if (!Provider.isServer)
        return;
      EItemType type = this.player.equipment.asset.type;
      ushort itemId = this.player.equipment.itemID;
      byte quality = this.player.equipment.quality;
      byte[] state = this.player.equipment.state;
      this.player.equipment.use();
      if (type == EItemType.HAT)
        this.player.clothing.askWearHat(itemId, quality, state);
      else if (type == EItemType.SHIRT)
        this.player.clothing.askWearShirt(itemId, quality, state);
      else if (type == EItemType.PANTS)
        this.player.clothing.askWearPants(itemId, quality, state);
      else if (type == EItemType.BACKPACK)
        this.player.clothing.askWearBackpack(itemId, quality, state);
      else if (type == EItemType.VEST)
        this.player.clothing.askWearVest(itemId, quality, state);
      else if (type == EItemType.MASK)
      {
        this.player.clothing.askWearMask(itemId, quality, state);
      }
      else
      {
        if (type != EItemType.GLASSES)
          return;
        this.player.clothing.askWearGlasses(itemId, quality, state);
      }
    }
  }
}
