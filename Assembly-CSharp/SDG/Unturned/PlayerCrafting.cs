// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.PlayerCrafting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using System.Collections.Generic;

namespace SDG.Unturned
{
  public class PlayerCrafting : PlayerCaller
  {
    public CraftingUpdated onCraftingUpdated;

    [SteamCall]
    public void tellCraft(CSteamID steamID)
    {
      if (!this.channel.checkServer(steamID) || this.onCraftingUpdated == null)
        return;
      this.onCraftingUpdated();
    }

    [SteamCall]
    public void askCraft(CSteamID steamID, ushort id, byte index, bool force)
    {
      if (!this.channel.checkOwner(steamID) || this.player.equipment.isBusy)
        return;
      ItemAsset itemAsset = (ItemAsset) Assets.find(EAssetType.ITEM, id);
      if (itemAsset == null || (int) index >= itemAsset.blueprints.Length)
        return;
      Blueprint blueprint = itemAsset.blueprints[(int) index];
      if ((int) blueprint.tool != 0 && this.player.inventory.has(blueprint.tool) == null)
        return;
      if (blueprint.skill != EBlueprintSkill.NONE)
      {
        bool flag = false;
        foreach (InteractableFire interactableFire in PowerTool.checkFires(this.transform.position, 16f))
        {
          if (interactableFire.isLit)
            flag = true;
        }
        if (blueprint.skill == EBlueprintSkill.CRAFT && (int) this.player.skills.skills[2][1].level < (int) blueprint.level || blueprint.skill == EBlueprintSkill.COOK && (!flag || (int) this.player.skills.skills[2][3].level < (int) blueprint.level) || blueprint.skill == EBlueprintSkill.REPAIR && (int) this.player.skills.skills[2][7].level < (int) blueprint.level)
          return;
      }
      bool flag1 = false;
      do
      {
        List<InventorySearch>[] listArray = new List<InventorySearch>[blueprint.supplies.Length];
        for (byte index1 = (byte) 0; (int) index1 < blueprint.supplies.Length; ++index1)
        {
          BlueprintSupply blueprintSupply = blueprint.supplies[(int) index1];
          List<InventorySearch> list = this.player.inventory.search(blueprintSupply.id, false, true);
          if (list.Count == 0)
            return;
          ushort num = (ushort) 0;
          for (byte index2 = (byte) 0; (int) index2 < list.Count; ++index2)
            num += (ushort) list[(int) index2].jar.item.amount;
          if ((int) num < (int) blueprintSupply.amount && blueprint.type != EBlueprintType.AMMO)
            return;
          listArray[(int) index1] = list;
        }
        if (blueprint.type == EBlueprintType.REPAIR)
        {
          List<InventorySearch> list1 = this.player.inventory.search(itemAsset.id, false, false);
          byte num1 = byte.MaxValue;
          byte num2 = byte.MaxValue;
          for (byte index1 = (byte) 0; (int) index1 < list1.Count; ++index1)
          {
            if ((int) list1[(int) index1].jar.item.quality < (int) num1)
            {
              num1 = list1[(int) index1].jar.item.quality;
              num2 = index1;
            }
          }
          if ((int) num2 == (int) byte.MaxValue)
            break;
          InventorySearch inventorySearch1 = list1[(int) num2];
          if (this.player.equipment.checkSelection(inventorySearch1.page, inventorySearch1.jar.x, inventorySearch1.jar.y))
            this.player.equipment.dequip();
          for (byte index1 = (byte) 0; (int) index1 < listArray.Length; ++index1)
          {
            BlueprintSupply blueprintSupply = blueprint.supplies[(int) index1];
            List<InventorySearch> list2 = listArray[(int) index1];
            for (byte index2 = (byte) 0; (int) index2 < (int) blueprintSupply.amount; ++index2)
            {
              InventorySearch inventorySearch2 = list2[(int) index2];
              if (this.player.equipment.checkSelection(inventorySearch2.page, inventorySearch2.jar.x, inventorySearch2.jar.y))
                this.player.equipment.dequip();
              this.player.inventory.removeItem(inventorySearch2.page, this.player.inventory.getIndex(inventorySearch2.page, inventorySearch2.jar.x, inventorySearch2.jar.y));
              if ((int) inventorySearch2.page < (int) PlayerInventory.SLOTS)
                this.player.equipment.sendSlot(inventorySearch2.page);
            }
          }
          this.player.inventory.sendUpdateQuality(inventorySearch1.page, inventorySearch1.jar.x, inventorySearch1.jar.y, (byte) 100);
          this.channel.send("tellCraft", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER);
        }
        else if (blueprint.type == EBlueprintType.AMMO)
        {
          List<InventorySearch> list1 = this.player.inventory.search(itemAsset.id, true, true);
          int num1 = -1;
          byte num2 = byte.MaxValue;
          for (byte index1 = (byte) 0; (int) index1 < list1.Count; ++index1)
          {
            if ((int) list1[(int) index1].jar.item.amount > num1 && (int) list1[(int) index1].jar.item.amount < (int) itemAsset.amount)
            {
              num1 = (int) list1[(int) index1].jar.item.amount;
              num2 = index1;
            }
          }
          if ((int) num2 == (int) byte.MaxValue)
            break;
          InventorySearch inventorySearch1 = list1[(int) num2];
          int num3 = (int) itemAsset.amount - num1;
          if (this.player.equipment.checkSelection(inventorySearch1.page, inventorySearch1.jar.x, inventorySearch1.jar.y))
            this.player.equipment.dequip();
          List<InventorySearch> list2 = listArray[0];
          for (byte index1 = (byte) 0; (int) index1 < list2.Count; ++index1)
          {
            InventorySearch inventorySearch2 = list2[(int) index1];
            if (inventorySearch2.jar != inventorySearch1.jar)
            {
              if (this.player.equipment.checkSelection(inventorySearch2.page, inventorySearch2.jar.x, inventorySearch2.jar.y))
                this.player.equipment.dequip();
              if ((int) inventorySearch2.jar.item.amount > num3)
              {
                this.player.inventory.sendUpdateAmount(inventorySearch2.page, inventorySearch2.jar.x, inventorySearch2.jar.y, (byte) ((uint) inventorySearch2.jar.item.amount - (uint) num3));
                num3 = 0;
                break;
              }
              num3 -= (int) inventorySearch2.jar.item.amount;
              this.player.inventory.sendUpdateAmount(inventorySearch2.page, inventorySearch2.jar.x, inventorySearch2.jar.y, (byte) 0);
              if ((int) index == 0 && itemAsset.blueprints.Length > 1 || itemAsset.blueprints.Length == 1)
              {
                this.player.inventory.removeItem(inventorySearch2.page, this.player.inventory.getIndex(inventorySearch2.page, inventorySearch2.jar.x, inventorySearch2.jar.y));
                if ((int) inventorySearch2.page < (int) PlayerInventory.SLOTS)
                  this.player.equipment.sendSlot(inventorySearch2.page);
              }
              if (num3 == 0)
                break;
            }
          }
          this.player.inventory.sendUpdateAmount(inventorySearch1.page, inventorySearch1.jar.x, inventorySearch1.jar.y, (byte) ((uint) itemAsset.amount - (uint) num3));
          this.channel.send("tellCraft", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER);
        }
        else
        {
          for (byte index1 = (byte) 0; (int) index1 < listArray.Length; ++index1)
          {
            BlueprintSupply blueprintSupply = blueprint.supplies[(int) index1];
            List<InventorySearch> list = listArray[(int) index1];
            for (byte index2 = (byte) 0; (int) index2 < (int) blueprintSupply.amount; ++index2)
            {
              InventorySearch inventorySearch = list[(int) index2];
              if (this.player.equipment.checkSelection(inventorySearch.page, inventorySearch.jar.x, inventorySearch.jar.y))
                this.player.equipment.dequip();
              this.player.inventory.removeItem(inventorySearch.page, this.player.inventory.getIndex(inventorySearch.page, inventorySearch.jar.x, inventorySearch.jar.y));
              if ((int) inventorySearch.page < (int) PlayerInventory.SLOTS)
                this.player.equipment.sendSlot(inventorySearch.page);
            }
          }
          for (ushort index1 = (ushort) 0; (int) index1 < (int) blueprint.products; ++index1)
            this.player.inventory.forceAddItem(new Item(blueprint.product, true), true);
          this.channel.send("tellCraft", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER);
        }
        if (!flag1)
        {
          flag1 = true;
          if ((int) blueprint.build != 0)
            EffectManager.sendEffect(blueprint.build, EffectManager.SMALL, this.transform.position);
        }
      }
      while (force && blueprint.type != EBlueprintType.REPAIR && blueprint.type != EBlueprintType.AMMO);
    }

    public void sendCraft(ushort id, byte index, bool force)
    {
      this.channel.send("askCraft", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[3]
      {
        (object) id,
        (object) index,
        (object) (bool) (force ? 1 : 0)
      });
    }
  }
}
