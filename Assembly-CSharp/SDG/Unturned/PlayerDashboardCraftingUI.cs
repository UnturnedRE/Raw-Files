// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.PlayerDashboardCraftingUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
  public class PlayerDashboardCraftingUI
  {
    private static readonly int TYPES = 8;
    private static Sleek container;
    public static Bundle icons;
    public static bool active;
    private static SleekBox backdropBox;
    private static Blueprint[] blueprints;
    private static SleekScrollBox blueprintsScrollBox;
    private static SleekBox infoBox;
    private static byte selectedType;

    public PlayerDashboardCraftingUI()
    {
      if (PlayerDashboardCraftingUI.icons != null)
        PlayerDashboardCraftingUI.icons.unload();
      Local local = Localization.read("/Player/PlayerDashboardCrafting.dat");
      PlayerDashboardCraftingUI.icons = Bundles.getBundle("/Bundles/Textures/Player/Icons/PlayerDashboardCrafting/PlayerDashboardCrafting.unity3d");
      PlayerDashboardCraftingUI.container = new Sleek();
      PlayerDashboardCraftingUI.container.positionScale_Y = 1f;
      PlayerDashboardCraftingUI.container.positionOffset_X = 10;
      PlayerDashboardCraftingUI.container.positionOffset_Y = 10;
      PlayerDashboardCraftingUI.container.sizeOffset_X = -20;
      PlayerDashboardCraftingUI.container.sizeOffset_Y = -20;
      PlayerDashboardCraftingUI.container.sizeScale_X = 1f;
      PlayerDashboardCraftingUI.container.sizeScale_Y = 1f;
      PlayerUI.container.add(PlayerDashboardCraftingUI.container);
      PlayerDashboardCraftingUI.active = false;
      PlayerDashboardCraftingUI.selectedType = byte.MaxValue;
      PlayerDashboardCraftingUI.backdropBox = new SleekBox();
      PlayerDashboardCraftingUI.backdropBox.positionOffset_Y = 60;
      PlayerDashboardCraftingUI.backdropBox.sizeOffset_Y = -60;
      PlayerDashboardCraftingUI.backdropBox.sizeScale_X = 1f;
      PlayerDashboardCraftingUI.backdropBox.sizeScale_Y = 1f;
      PlayerDashboardCraftingUI.backdropBox.backgroundColor = Palette.COLOR_W;
      PlayerDashboardCraftingUI.backdropBox.backgroundColor.a = 0.5f;
      PlayerDashboardCraftingUI.container.add((Sleek) PlayerDashboardCraftingUI.backdropBox);
      PlayerDashboardCraftingUI.blueprintsScrollBox = new SleekScrollBox();
      PlayerDashboardCraftingUI.blueprintsScrollBox.positionOffset_X = 10;
      PlayerDashboardCraftingUI.blueprintsScrollBox.positionOffset_Y = 70;
      PlayerDashboardCraftingUI.blueprintsScrollBox.sizeOffset_X = -20;
      PlayerDashboardCraftingUI.blueprintsScrollBox.sizeOffset_Y = -80;
      PlayerDashboardCraftingUI.blueprintsScrollBox.sizeScale_X = 1f;
      PlayerDashboardCraftingUI.blueprintsScrollBox.sizeScale_Y = 1f;
      PlayerDashboardCraftingUI.backdropBox.add((Sleek) PlayerDashboardCraftingUI.blueprintsScrollBox);
      for (int index = 0; index < PlayerDashboardCraftingUI.TYPES; ++index)
      {
        SleekButtonIcon sleekButtonIcon = new SleekButtonIcon((Texture2D) PlayerDashboardCraftingUI.icons.load("Blueprint_" + (object) index));
        sleekButtonIcon.positionOffset_X = index * 60 - 235;
        sleekButtonIcon.positionOffset_Y = 10;
        sleekButtonIcon.positionScale_X = 0.5f;
        sleekButtonIcon.sizeOffset_X = 50;
        sleekButtonIcon.sizeOffset_Y = 50;
        sleekButtonIcon.tooltip = local.format("Type_" + (object) index + "_Tooltip");
        sleekButtonIcon.onClickedButton = new ClickedButton(PlayerDashboardCraftingUI.onClickedTypeButton);
        PlayerDashboardCraftingUI.backdropBox.add((Sleek) sleekButtonIcon);
      }
      PlayerDashboardCraftingUI.infoBox = new SleekBox();
      PlayerDashboardCraftingUI.infoBox.positionOffset_X = 10;
      PlayerDashboardCraftingUI.infoBox.positionOffset_Y = 70;
      PlayerDashboardCraftingUI.infoBox.sizeOffset_X = -20;
      PlayerDashboardCraftingUI.infoBox.sizeOffset_Y = 50;
      PlayerDashboardCraftingUI.infoBox.sizeScale_X = 1f;
      PlayerDashboardCraftingUI.infoBox.text = local.format("No_Blueprints");
      PlayerDashboardCraftingUI.infoBox.fontSize = 14;
      PlayerDashboardCraftingUI.backdropBox.add((Sleek) PlayerDashboardCraftingUI.infoBox);
      PlayerDashboardCraftingUI.infoBox.isVisible = false;
      PlayerDashboardCraftingUI.updateSelection((byte) 0);
      Player.player.inventory.onInventoryResized += new InventoryResized(PlayerDashboardCraftingUI.onInventoryResized);
      Player.player.crafting.onCraftingUpdated += new CraftingUpdated(PlayerDashboardCraftingUI.onCraftingUpdated);
    }

    public static void open()
    {
      if (PlayerDashboardCraftingUI.active)
      {
        PlayerDashboardCraftingUI.close();
      }
      else
      {
        PlayerDashboardCraftingUI.active = true;
        PlayerDashboardCraftingUI.updateSelection(PlayerDashboardCraftingUI.selectedType);
        PlayerDashboardCraftingUI.container.lerpPositionScale(0.0f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
      }
    }

    public static void close()
    {
      if (!PlayerDashboardCraftingUI.active)
        return;
      PlayerDashboardCraftingUI.active = false;
      PlayerDashboardCraftingUI.container.lerpPositionScale(0.0f, 1f, ESleekLerp.EXPONENTIAL, 20f);
    }

    private static void updateSelection(byte typeIndex)
    {
      bool flag = false;
      foreach (InteractableFire interactableFire in PowerTool.checkFires(Player.player.transform.position, 16f))
      {
        if (interactableFire.isLit)
          flag = true;
      }
      EBlueprintType eblueprintType = (EBlueprintType) typeIndex;
      Asset[] assetArray = Assets.find(EAssetType.ITEM);
      List<Blueprint> list1 = new List<Blueprint>();
      for (int index1 = 0; index1 < assetArray.Length; ++index1)
      {
        ItemAsset itemAsset = (ItemAsset) assetArray[index1];
        for (int index2 = 0; index2 < itemAsset.blueprints.Length; ++index2)
        {
          Blueprint blueprint = itemAsset.blueprints[index2];
          if (blueprint.type == eblueprintType && (blueprint.skill == EBlueprintSkill.NONE || blueprint.skill == EBlueprintSkill.CRAFT && (int) Player.player.skills.skills[2][1].level >= (int) blueprint.level || blueprint.skill == EBlueprintSkill.COOK && flag && (int) Player.player.skills.skills[2][3].level >= (int) blueprint.level || blueprint.skill == EBlueprintSkill.REPAIR && (int) Player.player.skills.skills[2][7].level >= (int) blueprint.level))
          {
            ushort num1 = (ushort) 0;
            blueprint.hasSupplies = true;
            List<InventorySearch>[] listArray = new List<InventorySearch>[blueprint.supplies.Length];
            for (byte index3 = (byte) 0; (int) index3 < blueprint.supplies.Length; ++index3)
            {
              BlueprintSupply blueprintSupply = blueprint.supplies[(int) index3];
              List<InventorySearch> list2 = Player.player.inventory.search(blueprintSupply.id, false, true);
              ushort num2 = (ushort) 0;
              for (byte index4 = (byte) 0; (int) index4 < list2.Count; ++index4)
                num2 += (ushort) list2[(int) index4].jar.item.amount;
              num1 += num2;
              blueprintSupply.hasAmount = num2;
              if (blueprint.type == EBlueprintType.AMMO)
              {
                if ((int) num2 == 0)
                  blueprint.hasSupplies = false;
              }
              else if ((int) num2 < (int) blueprintSupply.amount)
                blueprint.hasSupplies = false;
              listArray[(int) index3] = list2;
            }
            if ((int) blueprint.tool != 0)
            {
              InventorySearch inventorySearch = Player.player.inventory.has(blueprint.tool);
              blueprint.tools = inventorySearch == null ? (ushort) 0 : (ushort) 1;
              blueprint.hasTool = inventorySearch != null;
            }
            else
            {
              blueprint.tools = (ushort) 1;
              blueprint.hasTool = true;
            }
            if (blueprint.type == EBlueprintType.REPAIR)
            {
              List<InventorySearch> list2 = Player.player.inventory.search(itemAsset.id, false, false);
              byte num2 = byte.MaxValue;
              byte num3 = byte.MaxValue;
              for (byte index3 = (byte) 0; (int) index3 < list2.Count; ++index3)
              {
                if ((int) list2[(int) index3].jar.item.quality < (int) num2)
                {
                  num2 = list2[(int) index3].jar.item.quality;
                  num3 = index3;
                }
              }
              if ((int) num3 != (int) byte.MaxValue)
              {
                blueprint.items = (ushort) list2[(int) num3].jar.item.quality;
                ++num1;
              }
              else
                blueprint.items = (ushort) 0;
              blueprint.hasItem = (int) num3 != (int) byte.MaxValue;
            }
            else if (blueprint.type == EBlueprintType.AMMO)
            {
              List<InventorySearch> list2 = Player.player.inventory.search(itemAsset.id, true, true);
              int num2 = -1;
              byte num3 = byte.MaxValue;
              for (byte index3 = (byte) 0; (int) index3 < list2.Count; ++index3)
              {
                if ((int) list2[(int) index3].jar.item.amount > num2 && (int) list2[(int) index3].jar.item.amount < (int) itemAsset.amount)
                {
                  num2 = (int) list2[(int) index3].jar.item.amount;
                  num3 = index3;
                }
              }
              if ((int) num3 != (int) byte.MaxValue)
              {
                if ((int) list2[(int) num3].jar.item.id == (int) blueprint.supplies[0].id)
                  blueprint.supplies[0].hasAmount -= (ushort) num2;
                blueprint.supplies[0].amount = (ushort) (byte) ((uint) itemAsset.amount - (uint) num2);
                blueprint.items = (ushort) list2[(int) num3].jar.item.amount;
                ++num1;
              }
              else
              {
                blueprint.supplies[0].amount = (ushort) 0;
                blueprint.items = (ushort) 0;
              }
              blueprint.hasItem = (int) num3 != (int) byte.MaxValue;
              blueprint.products = (int) num3 != (int) byte.MaxValue ? ((int) blueprint.items + (int) blueprint.supplies[0].hasAmount <= (int) itemAsset.amount ? (ushort) ((uint) blueprint.items + (uint) blueprint.supplies[0].hasAmount) : (ushort) itemAsset.amount) : (ushort) 0;
            }
            else
              blueprint.hasItem = true;
            if ((blueprint.type == EBlueprintType.AMMO || blueprint.type == EBlueprintType.REPAIR || (int) num1 != 0) && blueprint.hasItem)
              list1.Add(blueprint);
          }
        }
      }
      PlayerDashboardCraftingUI.blueprints = list1.ToArray();
      PlayerDashboardCraftingUI.blueprintsScrollBox.remove();
      PlayerDashboardCraftingUI.blueprintsScrollBox.area = new Rect(0.0f, 0.0f, 5f, (float) (PlayerDashboardCraftingUI.blueprints.Length * 205 - 10));
      for (int index = 0; index < PlayerDashboardCraftingUI.blueprints.Length; ++index)
      {
        SleekBlueprint sleekBlueprint = new SleekBlueprint(PlayerDashboardCraftingUI.blueprints[index]);
        sleekBlueprint.positionOffset_Y = index * 205;
        sleekBlueprint.sizeOffset_X = -30;
        sleekBlueprint.sizeOffset_Y = 195;
        sleekBlueprint.sizeScale_X = 1f;
        sleekBlueprint.onClickedButton = new ClickedButton(PlayerDashboardCraftingUI.onClickedBlueprintButton);
        PlayerDashboardCraftingUI.blueprintsScrollBox.add((Sleek) sleekBlueprint);
      }
      PlayerDashboardCraftingUI.selectedType = typeIndex;
      PlayerDashboardCraftingUI.infoBox.isVisible = PlayerDashboardCraftingUI.blueprints.Length == 0;
    }

    private static void onInventoryResized(byte page, byte newWidth, byte newHeight)
    {
      if (!PlayerDashboardCraftingUI.active)
        return;
      PlayerDashboardCraftingUI.updateSelection(PlayerDashboardCraftingUI.selectedType);
    }

    private static void onCraftingUpdated()
    {
      if (!PlayerDashboardCraftingUI.active)
        return;
      PlayerDashboardCraftingUI.updateSelection(PlayerDashboardCraftingUI.selectedType);
    }

    private static void onClickedTypeButton(SleekButton button)
    {
      PlayerDashboardCraftingUI.updateSelection((byte) ((button.positionOffset_X + 235) / 60));
    }

    private static void onClickedBlueprintButton(SleekButton button)
    {
      byte num = (byte) (button.positionOffset_Y / 205);
      Blueprint blueprint = PlayerDashboardCraftingUI.blueprints[(int) num];
      if (!blueprint.hasSupplies || !blueprint.hasTool || (!blueprint.hasItem || Player.player.equipment.isBusy))
        return;
      Player.player.crafting.sendCraft(PlayerDashboardCraftingUI.blueprints[(int) num].source, PlayerDashboardCraftingUI.blueprints[(int) num].id, Input.GetKey(ControlsSettings.other));
    }
  }
}
