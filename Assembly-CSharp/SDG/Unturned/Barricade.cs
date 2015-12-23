// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.Barricade
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

namespace SDG.Unturned
{
  public class Barricade
  {
    private ushort _id;
    public ushort health;
    public byte[] state;

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

    public Barricade(ushort newID)
    {
      this._id = newID;
      ItemBarricadeAsset itemBarricadeAsset = (ItemBarricadeAsset) Assets.find(EAssetType.ITEM, this.id);
      if (itemBarricadeAsset == null)
      {
        this.state = new byte[0];
      }
      else
      {
        this.health = itemBarricadeAsset.health;
        this.state = itemBarricadeAsset.getState();
      }
    }

    public Barricade(ushort newID, ushort newHealth, byte[] newState)
    {
      this._id = newID;
      this.health = newHealth;
      this.state = newState;
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
      return (string) (object) this.id + (object) " " + (string) (object) this.health + " " + (string) (object) this.state.Length;
    }
  }
}
