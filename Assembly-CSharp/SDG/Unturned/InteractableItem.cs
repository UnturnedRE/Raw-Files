// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.InteractableItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class InteractableItem : Interactable
  {
    public Item item;
    public ItemAsset asset;

    public override void use()
    {
      ItemManager.takeItem(this.transform.parent);
    }

    public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
    {
      message = EPlayerMessage.ITEM;
      text = this.asset.itemName;
      color = !this.asset.showQuality ? Color.white : Color.Lerp(Palette.COLOR_R, Palette.COLOR_G, (float) this.item.quality / 100f);
      return true;
    }
  }
}
