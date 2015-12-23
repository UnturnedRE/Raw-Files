// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.SleekBlueprint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class SleekBlueprint : SleekButton
  {
    private Blueprint _blueprint;
    private SleekImageTexture[] icons;
    private BlueprintItemIconsInfo info;

    public Blueprint blueprint
    {
      get
      {
        return this._blueprint;
      }
    }

    public SleekBlueprint(Blueprint newBlueprint)
    {
      this._blueprint = newBlueprint;
      this.init();
      this.fontStyle = FontStyle.Bold;
      this.fontAlignment = TextAnchor.MiddleCenter;
      this.fontSize = SleekRender.FONT_SIZE;
      this.calculateContent();
      this.icons = new SleekImageTexture[this.blueprint.supplies.Length + ((int) this.blueprint.tool == 0 ? 0 : 1) + (this.blueprint.type == EBlueprintType.REPAIR || this.blueprint.type == EBlueprintType.AMMO ? 1 : 0) + ((int) this.blueprint.product == 0 ? 0 : 1)];
      this.info = new BlueprintItemIconsInfo();
      this.info.textures = new Texture2D[this.icons.Length];
      this.info.callback += new BlueprintItemIconsReady(this.onBlueprintItemIconsReady);
      int index1 = 0;
      SleekLabel sleekLabel1 = new SleekLabel();
      sleekLabel1.positionOffset_X = 5;
      sleekLabel1.positionOffset_Y = 5;
      sleekLabel1.sizeOffset_X = -10;
      sleekLabel1.sizeOffset_Y = 30;
      sleekLabel1.sizeScale_X = 1f;
      sleekLabel1.foregroundColor = Palette.COLOR_Y;
      sleekLabel1.fontSize = 14;
      this.add((Sleek) sleekLabel1);
      Sleek sleek = new Sleek();
      sleek.positionOffset_Y = 40;
      sleek.positionScale_X = 0.5f;
      sleek.sizeOffset_Y = -45;
      sleek.sizeScale_Y = 1f;
      this.add(sleek);
      int num1 = 0;
      for (int index2 = 0; index2 < this.blueprint.supplies.Length; ++index2)
      {
        BlueprintSupply blueprintSupply = this.blueprint.supplies[index2];
        ItemAsset itemAsset = (ItemAsset) Assets.find(EAssetType.ITEM, blueprintSupply.id);
        if (itemAsset != null)
        {
          sleekLabel1.text += itemAsset.itemName;
          SleekImageTexture sleekImageTexture1 = new SleekImageTexture();
          sleekImageTexture1.positionOffset_X = num1;
          sleekImageTexture1.positionOffset_Y = (int) -itemAsset.size_y * 25;
          sleekImageTexture1.positionScale_Y = 0.5f;
          sleekImageTexture1.sizeOffset_X = (int) itemAsset.size_x * 50;
          sleekImageTexture1.sizeOffset_Y = (int) itemAsset.size_y * 50;
          sleek.add((Sleek) sleekImageTexture1);
          this.icons[index1] = sleekImageTexture1;
          ++index1;
          ItemTool.getIcon(blueprintSupply.id, (byte) 100, itemAsset.getState(false), itemAsset, new ItemIconReady(this.info.onItemIconReady));
          SleekLabel sleekLabel2 = new SleekLabel();
          sleekLabel2.positionOffset_X = -100;
          sleekLabel2.positionOffset_Y = -30;
          sleekLabel2.positionScale_Y = 1f;
          sleekLabel2.sizeOffset_X = 100;
          sleekLabel2.sizeOffset_Y = 30;
          sleekLabel2.sizeScale_X = 1f;
          sleekLabel2.fontAlignment = TextAnchor.MiddleRight;
          sleekLabel2.text = (string) (object) blueprintSupply.hasAmount + (object) "/" + (string) (object) blueprintSupply.amount;
          sleekImageTexture1.add((Sleek) sleekLabel2);
          SleekLabel sleekLabel3 = sleekLabel1;
          string str = sleekLabel3.text + (object) " " + (string) (object) blueprintSupply.hasAmount + "/" + (string) (object) blueprintSupply.amount;
          sleekLabel3.text = str;
          if (this.blueprint.type == EBlueprintType.AMMO)
          {
            if ((int) blueprintSupply.hasAmount == 0 || (int) blueprintSupply.amount == 0)
            {
              sleekLabel2.backgroundColor = Palette.COLOR_R;
              sleekLabel2.foregroundColor = Palette.COLOR_R;
            }
          }
          else if ((int) blueprintSupply.hasAmount < (int) blueprintSupply.amount)
          {
            sleekLabel2.backgroundColor = Palette.COLOR_R;
            sleekLabel2.foregroundColor = Palette.COLOR_R;
          }
          num1 += (int) itemAsset.size_x * 50 + 25;
          if (index2 < this.blueprint.supplies.Length - 1 || (int) this.blueprint.tool != 0 || (this.blueprint.type == EBlueprintType.REPAIR || this.blueprint.type == EBlueprintType.AMMO))
          {
            sleekLabel1.text += " + ";
            SleekImageTexture sleekImageTexture2 = new SleekImageTexture((Texture) PlayerDashboardCraftingUI.icons.load("Plus"));
            sleekImageTexture2.positionOffset_X = num1;
            sleekImageTexture2.positionOffset_Y = -20;
            sleekImageTexture2.positionScale_Y = 0.5f;
            sleekImageTexture2.sizeOffset_X = 40;
            sleekImageTexture2.sizeOffset_Y = 40;
            sleek.add((Sleek) sleekImageTexture2);
            num1 += 65;
          }
        }
      }
      if ((int) this.blueprint.tool != 0)
      {
        ItemAsset itemAsset = (ItemAsset) Assets.find(EAssetType.ITEM, this.blueprint.tool);
        if (itemAsset != null)
        {
          sleekLabel1.text += itemAsset.itemName;
          SleekImageTexture sleekImageTexture1 = new SleekImageTexture();
          sleekImageTexture1.positionOffset_X = num1;
          sleekImageTexture1.positionOffset_Y = (int) -itemAsset.size_y * 25;
          sleekImageTexture1.positionScale_Y = 0.5f;
          sleekImageTexture1.sizeOffset_X = (int) itemAsset.size_x * 50;
          sleekImageTexture1.sizeOffset_Y = (int) itemAsset.size_y * 50;
          sleek.add((Sleek) sleekImageTexture1);
          this.icons[index1] = sleekImageTexture1;
          ++index1;
          ItemTool.getIcon(this.blueprint.tool, (byte) 100, itemAsset.getState(), itemAsset, new ItemIconReady(this.info.onItemIconReady));
          SleekLabel sleekLabel2 = new SleekLabel();
          sleekLabel2.positionOffset_X = -100;
          sleekLabel2.positionOffset_Y = -30;
          sleekLabel2.positionScale_Y = 1f;
          sleekLabel2.sizeOffset_X = 100;
          sleekLabel2.sizeOffset_Y = 30;
          sleekLabel2.sizeScale_X = 1f;
          sleekLabel2.fontAlignment = TextAnchor.MiddleRight;
          sleekLabel2.text = (string) (object) this.blueprint.tools + (object) "/1";
          sleekImageTexture1.add((Sleek) sleekLabel2);
          SleekLabel sleekLabel3 = sleekLabel1;
          string str = string.Concat(new object[4]
          {
            (object) sleekLabel3.text,
            (object) " ",
            (object) this.blueprint.tools,
            (object) "/1"
          });
          sleekLabel3.text = str;
          if (!this.blueprint.hasTool)
          {
            sleekLabel2.backgroundColor = Palette.COLOR_R;
            sleekLabel2.foregroundColor = Palette.COLOR_R;
          }
          num1 += (int) itemAsset.size_x * 50 + 25;
          if (this.blueprint.type == EBlueprintType.REPAIR || this.blueprint.type == EBlueprintType.AMMO)
          {
            sleekLabel1.text += " + ";
            SleekImageTexture sleekImageTexture2 = new SleekImageTexture((Texture) PlayerDashboardCraftingUI.icons.load("Plus"));
            sleekImageTexture2.positionOffset_X = num1;
            sleekImageTexture2.positionOffset_Y = -20;
            sleekImageTexture2.positionScale_Y = 0.5f;
            sleekImageTexture2.sizeOffset_X = 40;
            sleekImageTexture2.sizeOffset_Y = 40;
            sleek.add((Sleek) sleekImageTexture2);
            num1 += 65;
          }
        }
      }
      if (this.blueprint.type == EBlueprintType.REPAIR || this.blueprint.type == EBlueprintType.AMMO)
      {
        ItemAsset itemAsset = (ItemAsset) Assets.find(EAssetType.ITEM, this.blueprint.product);
        if (itemAsset != null)
        {
          sleekLabel1.text += itemAsset.itemName;
          SleekImageTexture sleekImageTexture = new SleekImageTexture();
          sleekImageTexture.positionOffset_X = num1;
          sleekImageTexture.positionOffset_Y = (int) -itemAsset.size_y * 25;
          sleekImageTexture.positionScale_Y = 0.5f;
          sleekImageTexture.sizeOffset_X = (int) itemAsset.size_x * 50;
          sleekImageTexture.sizeOffset_Y = (int) itemAsset.size_y * 50;
          sleek.add((Sleek) sleekImageTexture);
          this.icons[index1] = sleekImageTexture;
          ++index1;
          ItemTool.getIcon(this.blueprint.product, (byte) 100, itemAsset.getState(), itemAsset, new ItemIconReady(this.info.onItemIconReady));
          SleekLabel sleekLabel2 = new SleekLabel();
          sleekLabel2.positionOffset_X = -100;
          sleekLabel2.positionOffset_Y = -30;
          sleekLabel2.positionScale_Y = 1f;
          sleekLabel2.sizeOffset_X = 100;
          sleekLabel2.sizeOffset_Y = 30;
          sleekLabel2.sizeScale_X = 1f;
          sleekLabel2.fontAlignment = TextAnchor.MiddleRight;
          if (this.blueprint.type == EBlueprintType.REPAIR)
          {
            SleekLabel sleekLabel3 = sleekLabel1;
            string str = string.Concat(new object[4]
            {
              (object) sleekLabel3.text,
              (object) " ",
              (object) this.blueprint.items,
              (object) "%"
            });
            sleekLabel3.text = str;
            sleekLabel2.text = (string) (object) this.blueprint.items + (object) "%";
            sleekLabel2.backgroundColor = Color.Lerp(Palette.COLOR_R, Palette.COLOR_G, (float) this.blueprint.items / 100f);
            sleekLabel2.foregroundColor = sleekLabel2.backgroundColor;
          }
          else if (this.blueprint.type == EBlueprintType.AMMO)
          {
            SleekLabel sleekLabel3 = sleekLabel1;
            string str = sleekLabel3.text + (object) " " + (string) (object) this.blueprint.items + "/" + (string) (object) this.blueprint.products;
            sleekLabel3.text = str;
            sleekLabel2.text = (string) (object) this.blueprint.items + (object) "/" + (string) (object) itemAsset.amount;
          }
          if (!this.blueprint.hasItem)
          {
            sleekLabel2.backgroundColor = Palette.COLOR_R;
            sleekLabel2.foregroundColor = Palette.COLOR_R;
          }
          sleekImageTexture.add((Sleek) sleekLabel2);
          num1 += (int) itemAsset.size_x * 50 + 25;
        }
      }
      sleekLabel1.text += " = ";
      SleekImageTexture sleekImageTexture3 = new SleekImageTexture((Texture) PlayerDashboardCraftingUI.icons.load("Equals"));
      sleekImageTexture3.positionOffset_X = num1;
      sleekImageTexture3.positionOffset_Y = -20;
      sleekImageTexture3.positionScale_Y = 0.5f;
      sleekImageTexture3.sizeOffset_X = 40;
      sleekImageTexture3.sizeOffset_Y = 40;
      sleek.add((Sleek) sleekImageTexture3);
      int num2 = num1 + 65;
      if ((int) this.blueprint.product != 0)
      {
        ItemAsset itemAsset1 = (ItemAsset) Assets.find(EAssetType.ITEM, this.blueprint.product);
        if (itemAsset1 != null)
        {
          sleekLabel1.text += itemAsset1.itemName;
          SleekImageTexture sleekImageTexture1 = new SleekImageTexture();
          sleekImageTexture1.positionOffset_X = (int) -itemAsset1.size_x * 50 - 10;
          sleekImageTexture1.positionOffset_Y = (int) -itemAsset1.size_y * 25;
          sleekImageTexture1.positionScale_X = 1f;
          sleekImageTexture1.positionScale_Y = 0.5f;
          sleekImageTexture1.sizeOffset_X = (int) itemAsset1.size_x * 50;
          sleekImageTexture1.sizeOffset_Y = (int) itemAsset1.size_y * 50;
          sleek.add((Sleek) sleekImageTexture1);
          this.icons[index1] = sleekImageTexture1;
          int num3 = index1 + 1;
          ItemTool.getIcon(this.blueprint.product, (byte) 100, itemAsset1.getState(), itemAsset1, new ItemIconReady(this.info.onItemIconReady));
          SleekLabel sleekLabel2 = new SleekLabel();
          sleekLabel2.positionOffset_X = -100;
          sleekLabel2.positionOffset_Y = -30;
          sleekLabel2.positionScale_Y = 1f;
          sleekLabel2.sizeOffset_X = 100;
          sleekLabel2.sizeOffset_Y = 30;
          sleekLabel2.sizeScale_X = 1f;
          sleekLabel2.fontAlignment = TextAnchor.MiddleRight;
          if (this.blueprint.type == EBlueprintType.REPAIR)
          {
            sleekLabel1.text += " 100%";
            sleekLabel2.text = "100%";
            sleekLabel2.backgroundColor = Palette.COLOR_G;
            sleekLabel2.foregroundColor = Palette.COLOR_G;
          }
          else if (this.blueprint.type == EBlueprintType.AMMO)
          {
            ItemAsset itemAsset2 = (ItemAsset) Assets.find(EAssetType.ITEM, this.blueprint.product);
            if (itemAsset2 != null)
            {
              SleekLabel sleekLabel3 = sleekLabel1;
              string str = sleekLabel3.text + (object) " " + (string) (object) this.blueprint.products + "/" + (string) (object) itemAsset2.amount;
              sleekLabel3.text = str;
              sleekLabel2.text = (string) (object) this.blueprint.products + (object) "/" + (string) (object) itemAsset2.amount;
            }
          }
          else
          {
            SleekLabel sleekLabel3 = sleekLabel1;
            string str = sleekLabel3.text + (object) " x" + (string) (object) this.blueprint.products;
            sleekLabel3.text = str;
            sleekLabel2.text = "x" + this.blueprint.products.ToString();
          }
          sleekImageTexture1.add((Sleek) sleekLabel2);
          num2 += (int) itemAsset1.size_x * 50;
        }
      }
      sleek.positionOffset_X = -num2 / 2;
      sleek.sizeOffset_X = num2;
      this.tooltip = sleekLabel1.text;
    }

    private void onBlueprintItemIconsReady()
    {
      for (int index = 0; index < this.icons.Length; ++index)
        this.icons[index].texture = (Texture) this.info.textures[index];
    }
  }
}
