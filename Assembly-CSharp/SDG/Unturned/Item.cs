// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.Item
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

namespace SDG.Unturned
{
  public class Item
  {
    private ushort _id;
    public byte amount;
    public byte quality;
    public byte[] state;

    public ushort id
    {
      get
      {
        return this._id;
      }
    }

    public Item(ushort newID, bool full)
    {
      this._id = newID;
      ItemAsset itemAsset = (ItemAsset) Assets.find(EAssetType.ITEM, this.id);
      if (itemAsset == null)
      {
        this.state = new byte[0];
      }
      else
      {
        if (full || Provider.mode == EGameMode.EASY)
        {
          this.amount = itemAsset.amount;
          this.quality = (byte) 100;
        }
        else
        {
          this.amount = itemAsset.count;
          this.quality = itemAsset.quality;
        }
        this.state = itemAsset.getState(full || Provider.mode == EGameMode.EASY);
      }
    }

    public Item(ushort newID, bool full, byte newQuality)
    {
      this._id = newID;
      this.quality = newQuality;
      ItemAsset itemAsset = (ItemAsset) Assets.find(EAssetType.ITEM, this.id);
      if (itemAsset == null)
      {
        this.state = new byte[0];
      }
      else
      {
        this.amount = full || Provider.mode == EGameMode.EASY ? itemAsset.amount : itemAsset.count;
        this.state = itemAsset.getState(full || Provider.mode == EGameMode.EASY);
      }
    }

    public Item(ushort newID, byte newAmount, byte newQuality)
    {
      this._id = newID;
      this.amount = newAmount;
      this.quality = newQuality;
      ItemAsset itemAsset = (ItemAsset) Assets.find(EAssetType.ITEM, this.id);
      if (itemAsset == null)
        this.state = new byte[0];
      else
        this.state = itemAsset.getState();
    }

    public Item(ushort newID, byte newAmount, byte newQuality, byte[] newState)
    {
      this._id = newID;
      this.amount = newAmount;
      this.quality = newQuality;
      this.state = newState;
    }

    public override string ToString()
    {
      return (string) (object) this.id + (object) " " + (string) (object) this.amount + " " + (string) (object) this.quality + " " + (string) (object) this.state.Length;
    }
  }
}
