// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.SleekItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class SleekItem : Sleek
  {
    private byte _hotkey = byte.MaxValue;
    private ItemJar _jar;
    private SleekButton button;
    private SleekImageTexture icon;
    private SleekLabel amountLabel;
    private SleekImageTexture qualityImage;
    private SleekLabel hotkeyLabel;
    public ClickedItem onClickedItem;
    public DraggedItem onDraggedItem;
    private bool isTemporary;

    public ItemJar jar
    {
      get
      {
        return this._jar;
      }
    }

    public int hotkey
    {
      get
      {
        return (int) this._hotkey;
      }
    }

    public SleekItem(ItemJar jar)
    {
      this.init();
      this.button = new SleekButton();
      this.button.positionOffset_X = 1;
      this.button.positionOffset_Y = 1;
      this.button.sizeOffset_X = -2;
      this.button.sizeOffset_Y = -2;
      this.button.sizeScale_X = 1f;
      this.button.sizeScale_Y = 1f;
      this.button.onClickedButton = new ClickedButton(this.onClickedButton);
      this.add((Sleek) this.button);
      this.icon = new SleekImageTexture();
      this.icon.sizeScale_X = 1f;
      this.icon.sizeScale_Y = 1f;
      this.add((Sleek) this.icon);
      this.icon.isVisible = false;
      this.amountLabel = new SleekLabel();
      this.amountLabel.positionOffset_X = 5;
      this.amountLabel.positionOffset_Y = -35;
      this.amountLabel.positionScale_Y = 1f;
      this.amountLabel.sizeOffset_X = -10;
      this.amountLabel.sizeOffset_Y = 30;
      this.amountLabel.sizeScale_X = 1f;
      this.amountLabel.fontAlignment = TextAnchor.LowerLeft;
      this.add((Sleek) this.amountLabel);
      this.amountLabel.isVisible = false;
      this.qualityImage = new SleekImageTexture();
      this.qualityImage.positionScale_X = 1f;
      this.qualityImage.positionScale_Y = 1f;
      this.add((Sleek) this.qualityImage);
      this.qualityImage.isVisible = false;
      this.hotkeyLabel = new SleekLabel();
      this.hotkeyLabel.positionOffset_X = 5;
      this.hotkeyLabel.positionOffset_Y = 5;
      this.hotkeyLabel.sizeOffset_X = -10;
      this.hotkeyLabel.sizeOffset_Y = 30;
      this.hotkeyLabel.sizeScale_X = 1f;
      this.hotkeyLabel.fontAlignment = TextAnchor.UpperRight;
      this.add((Sleek) this.hotkeyLabel);
      this.hotkeyLabel.isVisible = false;
      this.updateItem(jar);
    }

    public SleekItem()
    {
      this.init();
      this.button = new SleekButton();
      this.button.positionOffset_X = 1;
      this.button.positionOffset_Y = 1;
      this.button.sizeOffset_X = -2;
      this.button.sizeOffset_Y = -2;
      this.button.sizeScale_X = 1f;
      this.button.sizeScale_Y = 1f;
      this.add((Sleek) this.button);
      this.icon = new SleekImageTexture();
      this.icon.sizeScale_X = 1f;
      this.icon.sizeScale_Y = 1f;
      this.add((Sleek) this.icon);
      this.icon.isVisible = false;
      this.amountLabel = new SleekLabel();
      this.amountLabel.positionOffset_X = 5;
      this.amountLabel.positionOffset_Y = -35;
      this.amountLabel.positionScale_Y = 1f;
      this.amountLabel.sizeOffset_X = -10;
      this.amountLabel.sizeOffset_Y = 30;
      this.amountLabel.sizeScale_X = 1f;
      this.amountLabel.fontAlignment = TextAnchor.LowerLeft;
      this.add((Sleek) this.amountLabel);
      this.amountLabel.isVisible = false;
      this.qualityImage = new SleekImageTexture();
      this.qualityImage.positionScale_X = 1f;
      this.qualityImage.positionScale_Y = 1f;
      this.add((Sleek) this.qualityImage);
      this.qualityImage.isVisible = false;
      this.hotkeyLabel = new SleekLabel();
      this.hotkeyLabel.positionOffset_X = 5;
      this.hotkeyLabel.positionOffset_Y = 5;
      this.hotkeyLabel.sizeOffset_X = -10;
      this.hotkeyLabel.sizeOffset_Y = 30;
      this.hotkeyLabel.sizeScale_X = 1f;
      this.hotkeyLabel.fontAlignment = TextAnchor.UpperRight;
      this.add((Sleek) this.hotkeyLabel);
      this.hotkeyLabel.isVisible = false;
      this.isTemporary = true;
    }

    public void enable()
    {
      this.button.backgroundColor.a = 1f;
      this.icon.backgroundColor.a = 1f;
    }

    public void disable()
    {
      this.button.backgroundColor.a = 0.5f;
      this.icon.backgroundColor.a = 0.5f;
    }

    public void updateHotkey(byte index)
    {
      this._hotkey = index;
      if (this.hotkey == (int) byte.MaxValue)
      {
        this.hotkeyLabel.text = string.Empty;
        this.hotkeyLabel.isVisible = false;
      }
      else
      {
        this.hotkeyLabel.text = (this.hotkey + 1).ToString();
        this.hotkeyLabel.isVisible = true;
      }
    }

    public void updateItem(ItemJar newJar)
    {
      this._jar = newJar;
      ItemAsset itemAsset = (ItemAsset) Assets.find(EAssetType.ITEM, this.jar.item.id);
      if (itemAsset == null)
        return;
      if (!this.isTemporary)
        this.button.tooltip = itemAsset.itemName;
      this.sizeOffset_X = (int) itemAsset.size_x * 50;
      this.sizeOffset_Y = (int) itemAsset.size_y * 50;
      this.icon.isVisible = false;
      ItemTool.getIcon(this.jar.item.id, this.jar.item.quality, this.jar.item.state, itemAsset, new ItemIconReady(this.onItemIconReady));
      if ((int) itemAsset.size_x == 1 || (int) itemAsset.size_y == 1)
      {
        this.amountLabel.fontSize = 10;
        this.hotkeyLabel.fontSize = 10;
      }
      else
      {
        this.amountLabel.fontSize = 12;
        this.hotkeyLabel.fontSize = 12;
      }
      if (itemAsset.showQuality)
      {
        this.button.backgroundColor = Color.Lerp(Palette.COLOR_R, Palette.COLOR_G, (float) this.jar.item.quality / 100f);
        if ((int) itemAsset.size_x == 1 || (int) itemAsset.size_y == 1)
        {
          this.qualityImage.positionOffset_X = -20;
          this.qualityImage.positionOffset_Y = -20;
          this.qualityImage.sizeOffset_X = 10;
          this.qualityImage.sizeOffset_Y = 10;
          this.qualityImage.texture = (Texture) PlayerDashboardInventoryUI.icons.load("Quality_1");
        }
        else
        {
          this.qualityImage.positionOffset_X = -30;
          this.qualityImage.positionOffset_Y = -30;
          this.qualityImage.sizeOffset_X = 20;
          this.qualityImage.sizeOffset_Y = 20;
          this.qualityImage.texture = (Texture) PlayerDashboardInventoryUI.icons.load("Quality_0");
        }
        this.button.foregroundColor = this.button.backgroundColor;
        this.qualityImage.backgroundColor = this.button.backgroundColor;
        this.qualityImage.foregroundColor = this.button.backgroundColor;
        this.amountLabel.text = (string) (object) this.jar.item.quality + (object) "%";
        this.amountLabel.backgroundColor = this.button.backgroundColor;
        this.amountLabel.foregroundColor = this.button.backgroundColor;
        this.qualityImage.isVisible = true;
        this.amountLabel.isVisible = true;
      }
      else
      {
        this.qualityImage.isVisible = false;
        if ((int) itemAsset.amount > 1)
        {
          this.amountLabel.text = "x" + (object) this.jar.item.amount;
          this.amountLabel.backgroundColor = Color.white;
          this.amountLabel.foregroundColor = Color.white;
          this.amountLabel.isVisible = true;
        }
        else
          this.amountLabel.isVisible = false;
      }
    }

    private void onClickedButton(SleekButton button)
    {
      if (Event.current.button == 0)
      {
        if (this.onDraggedItem != null)
          this.onDraggedItem(this);
      }
      else if (this.onClickedItem != null)
        this.onClickedItem(this);
      Event.current.Use();
    }

    public override void draw(bool ignoreCulling)
    {
      this.drawChildren(ignoreCulling);
    }

    private void onItemIconReady(Texture2D texture)
    {
      this.icon.texture = (Texture) texture;
      this.icon.isVisible = true;
    }
  }
}
