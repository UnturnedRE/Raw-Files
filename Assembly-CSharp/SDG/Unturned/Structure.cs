// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.Structure
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

namespace SDG.Unturned
{
  public class Structure
  {
    private ushort _id;
    public ushort health;

    public bool isDead
    {
      get
      {
        return (int) this.health == 0;
      }
    }

    public ushort id
    {
      get
      {
        return this._id;
      }
    }

    public Structure(ushort newID)
    {
      this._id = newID;
      this.health = ((ItemStructureAsset) Assets.find(EAssetType.ITEM, this.id)).health;
    }

    public Structure(ushort newID, ushort newHealth)
    {
      this._id = newID;
      this.health = newHealth;
    }

    public void askDamage(ushort amount)
    {
      if ((int) amount == 0 || this.isDead)
        return;
      if ((int) amount >= (int) this.health)
        this.health = (ushort) 0;
      else
        this.health -= amount;
    }

    public override string ToString()
    {
      return (string) (object) this.id + (object) " " + (string) (object) this.health;
    }
  }
}
