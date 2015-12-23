// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.UseableCloud
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

namespace SDG.Unturned
{
  public class UseableCloud : Useable
  {
    public override void equip()
    {
      this.player.animator.play("Equip", true);
      this.player.movement.gravity = ((ItemCloudAsset) this.player.equipment.asset).gravity;
    }

    public override void dequip()
    {
      this.player.movement.gravity = 1f;
    }
  }
}
