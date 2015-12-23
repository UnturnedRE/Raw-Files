// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.SleekInventory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class SleekInventory : Sleek
  {
    private ItemAsset _itemAsset;
    private SleekButton button;
    private SleekImageTexture icon;
    private SleekLabel nameLabel;
    public ClickedInventory onClickedInventory;

    public ItemAsset itemAsset
    {
      get
      {
        return this._itemAsset;
      }
    }

    public SleekInventory()
    {
      this.init();
      this.button = new SleekButton();
      this.button.sizeScale_X = 1f;
      this.button.sizeScale_Y = 1f;
      this.button.onClickedButton = new ClickedButton(this.onClickedButton);
      this.add((Sleek) this.button);
      this.button.isClickable = false;
      this.icon = new SleekImageTexture();
      this.icon.positionOffset_X = 5;
      this.icon.positionOffset_Y = 5;
      this.icon.sizeScale_X = 1f;
      this.icon.sizeScale_Y = 1f;
      this.icon.sizeOffset_X = -10;
      this.icon.constraint = ESleekConstraint.XY;
      this.add((Sleek) this.icon);
      this.icon.isVisible = false;
      this.nameLabel = new SleekLabel();
      this.nameLabel.positionScale_Y = 1f;
      this.nameLabel.sizeScale_X = 1f;
      this.add((Sleek) this.nameLabel);
      this.nameLabel.isVisible = false;
    }

    public void updateInventory(int item, ushort quantity, bool isClickable, bool isLarge)
    {
      this.button.isClickable = isClickable;
      if (isLarge)
      {
        this.icon.sizeOffset_Y = -70;
        this.nameLabel.fontSize = 18;
        this.nameLabel.positionOffset_Y = -70;
        this.nameLabel.sizeOffset_Y = 70;
      }
      else
      {
        this.icon.sizeOffset_Y = -50;
        this.nameLabel.fontSize = 12;
        this.nameLabel.positionOffset_Y = -50;
        this.nameLabel.sizeOffset_Y = 50;
      }
      if (item != 0)
      {
        if (item < 0)
        {
          this.icon.texture = (Texture) Resources.Load("Economy/Mystery" + (!isLarge ? "/Icon_Small" : "/Icon_Large"));
          this.icon.isVisible = true;
          this.nameLabel.text = MenuSurvivorsClothingUI.localization.format("Mystery_" + (object) item + "_Text");
          this.button.tooltip = MenuSurvivorsClothingUI.localization.format("Mystery_Tooltip");
          this.button.backgroundColor = Palette.MYTHICAL;
        }
        else
        {
          ushort inventoryItemId = Provider.provider.economyService.getInventoryItemID(item);
          if ((int) inventoryItemId != 0)
          {
            this._itemAsset = (ItemAsset) Assets.find(EAssetType.ITEM, inventoryItemId);
            if (this.itemAsset != null)
            {
              if (this.itemAsset.type == EItemType.GUN || this.itemAsset.type == EItemType.MELEE)
              {
                SkinAsset skinAsset = (SkinAsset) Assets.find(EAssetType.SKIN, Provider.provider.economyService.getInventorySkinID(item));
                if (skinAsset != null)
                {
                  this.icon.texture = (Texture) Resources.Load("Economy/Skins/" + this.itemAsset.name + "/" + skinAsset.name + (!isLarge ? "/Icon_Small" : "/Icon_Large"));
                  this.icon.isVisible = true;
                }
                else
                  this.icon.isVisible = false;
              }
              else
              {
                this.icon.texture = (Texture) Resources.Load("Economy" + this.itemAsset.proPath + (!isLarge ? "/Icon_Small" : "/Icon_Large"));
                this.icon.isVisible = true;
              }
            }
            else
            {
              this.icon.texture = (Texture) null;
              this.icon.isVisible = true;
            }
            this.nameLabel.text = Provider.provider.economyService.getInventoryName(item);
            if ((int) quantity > 1)
            {
              SleekLabel sleekLabel = this.nameLabel;
              string str = sleekLabel.text + (object) " x" + (string) (object) quantity;
              sleekLabel.text = str;
            }
            this.button.tooltip = Provider.provider.economyService.getInventoryType(item);
            this.button.backgroundColor = Provider.provider.economyService.getInventoryColor(item);
          }
          else
          {
            this.icon.texture = (Texture) null;
            this.icon.isVisible = true;
            this.nameLabel.text = "itemdefid: " + (object) item;
            this.button.tooltip = "itemdefid: " + (object) item;
            this.button.backgroundColor = Color.white;
          }
        }
        this.nameLabel.isVisible = true;
      }
      else
      {
        this._itemAsset = (ItemAsset) null;
        this.button.tooltip = string.Empty;
        this.button.backgroundColor = Color.white;
        this.icon.isVisible = false;
        this.nameLabel.isVisible = false;
      }
      this.button.foregroundColor = this.button.backgroundColor;
      this.nameLabel.foregroundColor = this.button.backgroundColor;
      this.nameLabel.backgroundColor = this.button.backgroundColor;
    }

    private void onClickedButton(SleekButton button)
    {
      if (this.onClickedInventory == null)
        return;
      this.onClickedInventory(this);
    }

    public override void draw(bool ignoreCulling)
    {
      this.drawChildren(ignoreCulling);
    }
  }
}
