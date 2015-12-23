// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.BarricadeManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SDG.Unturned
{
  public class BarricadeManager : SteamCaller
  {
    public static readonly byte SAVEDATA_VERSION = (byte) 5;
    public static readonly byte BARRICADE_REGIONS = (byte) 2;
    private static BarricadeManager manager;
    private static BarricadeRegion[,] regions;
    private static List<BarricadeRegion> plants;

    public static void salvageBarricade(Transform barricade)
    {
      byte x;
      byte y;
      ushort plant;
      ushort index;
      BarricadeRegion region;
      if (!BarricadeManager.tryGetInfo(barricade, out x, out y, out plant, out index, out region))
        return;
      BarricadeManager.manager.channel.send("askSalvageBarricade", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[4]
      {
        (object) x,
        (object) y,
        (object) plant,
        (object) index
      });
    }

    [SteamCall]
    public void askSalvageBarricade(CSteamID steamID, byte x, byte y, ushort plant, ushort index)
    {
      BarricadeRegion region;
      if (!Provider.isServer || !BarricadeManager.tryGetRegion(x, y, plant, out region))
        return;
      Player player = PlayerTool.getPlayer(steamID);
      if ((UnityEngine.Object) player == (UnityEngine.Object) null || player.life.isDead || ((int) index >= region.models.Count || !OwnershipTool.checkToggle(player.channel.owner.playerID.steamID, region.barricades[(int) index].owner, player.channel.owner.playerID.group, region.barricades[(int) index].group)))
        return;
      ItemBarricadeAsset itemBarricadeAsset = (ItemBarricadeAsset) Assets.find(EAssetType.ITEM, region.barricades[(int) index].barricade.id);
      if (itemBarricadeAsset != null)
      {
        if ((int) region.barricades[(int) index].barricade.health == (int) itemBarricadeAsset.health)
        {
          player.inventory.forceAddItem(new Item(region.barricades[(int) index].barricade.id, true), true);
        }
        else
        {
          for (int index1 = 0; index1 < itemBarricadeAsset.blueprints.Length; ++index1)
          {
            Blueprint blueprint = itemBarricadeAsset.blueprints[index1];
            if ((int) blueprint.product == (int) itemBarricadeAsset.id)
            {
              ushort id = blueprint.supplies[UnityEngine.Random.Range(0, blueprint.supplies.Length)].id;
              player.inventory.forceAddItem(new Item(id, true), true);
              break;
            }
          }
        }
      }
      region.barricades.RemoveAt((int) index);
      if ((int) plant == (int) ushort.MaxValue)
        BarricadeManager.manager.channel.send("tellTakeBarricade", ESteamCall.ALL, x, y, BarricadeManager.BARRICADE_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, (object) x, (object) y, (object) plant, (object) index);
      else
        BarricadeManager.manager.channel.send("tellTakeBarricade", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[4]
        {
          (object) x,
          (object) y,
          (object) plant,
          (object) index
        });
    }

    public static void updateSign(Transform barricade, string newText)
    {
      byte x;
      byte y;
      ushort plant;
      ushort index;
      BarricadeRegion region;
      if (!BarricadeManager.tryGetInfo(barricade, out x, out y, out plant, out index, out region))
        return;
      BarricadeManager.manager.channel.send("askUpdateSign", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[5]
      {
        (object) x,
        (object) y,
        (object) plant,
        (object) index,
        (object) newText
      });
    }

    [SteamCall]
    public void tellUpdateSign(CSteamID steamID, byte x, byte y, ushort plant, ushort index, string newText)
    {
      BarricadeRegion region;
      if (!this.channel.checkServer(steamID) || !BarricadeManager.tryGetRegion(x, y, plant, out region) || !Provider.isServer && !region.isNetworked || (int) index >= region.models.Count)
        return;
      InteractableSign component = region.models[(int) index].GetComponent<InteractableSign>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return;
      component.updateText(newText);
    }

    [SteamCall]
    public void askUpdateSign(CSteamID steamID, byte x, byte y, ushort plant, ushort index, string newText)
    {
      BarricadeRegion region;
      if (!Provider.isServer || !BarricadeManager.tryGetRegion(x, y, plant, out region))
        return;
      Player player = PlayerTool.getPlayer(steamID);
      if ((UnityEngine.Object) player == (UnityEngine.Object) null || player.life.isDead || ((int) index >= region.models.Count || newText.Length > 200) || newText.Contains("<size"))
        return;
      InteractableSign component = region.models[(int) index].GetComponent<InteractableSign>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || !component.checkUpdate(player.channel.owner.playerID.steamID, player.channel.owner.playerID.group))
        return;
      if ((int) plant == (int) ushort.MaxValue)
        BarricadeManager.manager.channel.send("tellUpdateSign", ESteamCall.ALL, x, y, BarricadeManager.BARRICADE_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, (object) x, (object) y, (object) plant, (object) index, (object) newText);
      else
        BarricadeManager.manager.channel.send("tellUpdateSign", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[5]
        {
          (object) x,
          (object) y,
          (object) plant,
          (object) index,
          (object) newText
        });
      byte[] numArray1 = region.barricades[(int) index].barricade.state;
      byte[] bytes = Encoding.UTF8.GetBytes(newText);
      byte[] numArray2 = new byte[17 + newText.Length];
      Buffer.BlockCopy((Array) numArray1, 0, (Array) numArray2, 0, 16);
      numArray2[16] = (byte) bytes.Length;
      Buffer.BlockCopy((Array) bytes, 0, (Array) numArray2, 17, bytes.Length);
      region.barricades[(int) index].barricade.state = numArray2;
    }

    public static void toggleSafezone(Transform barricade)
    {
      byte x;
      byte y;
      ushort plant;
      ushort index;
      BarricadeRegion region;
      if (!BarricadeManager.tryGetInfo(barricade, out x, out y, out plant, out index, out region))
        return;
      BarricadeManager.manager.channel.send("askToggleSafezone", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[4]
      {
        (object) x,
        (object) y,
        (object) plant,
        (object) index
      });
    }

    [SteamCall]
    public void tellToggleSafezone(CSteamID steamID, byte x, byte y, ushort plant, ushort index, bool isPowered)
    {
      BarricadeRegion region;
      if (!this.channel.checkServer(steamID) || !BarricadeManager.tryGetRegion(x, y, plant, out region) || !Provider.isServer && !region.isNetworked || (int) index >= region.models.Count)
        return;
      InteractableSafezone component = region.models[(int) index].GetComponent<InteractableSafezone>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return;
      component.updatePowered(isPowered);
    }

    [SteamCall]
    public void askToggleSafezone(CSteamID steamID, byte x, byte y, ushort plant, ushort index)
    {
      BarricadeRegion region;
      if (!Provider.isServer || !BarricadeManager.tryGetRegion(x, y, plant, out region))
        return;
      Player player = PlayerTool.getPlayer(steamID);
      if ((UnityEngine.Object) player == (UnityEngine.Object) null || player.life.isDead || (int) index >= region.models.Count)
        return;
      InteractableSafezone component = region.models[(int) index].GetComponent<InteractableSafezone>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return;
      if ((int) plant == (int) ushort.MaxValue)
        BarricadeManager.manager.channel.send("tellToggleSafezone", ESteamCall.ALL, x, y, BarricadeManager.BARRICADE_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, (object) x, (object) y, (object) plant, (object) index, (object) (bool) (!component.isPowered ? 1 : 0));
      else
        BarricadeManager.manager.channel.send("tellToggleSafezone", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[5]
        {
          (object) x,
          (object) y,
          (object) plant,
          (object) index,
          (object) (bool) (!component.isPowered ? 1 : 0)
        });
      region.barricades[(int) index].barricade.state[0] = !component.isPowered ? (byte) 0 : (byte) 1;
      EffectManager.sendEffect((ushort) 8, EffectManager.SMALL, component.transform.position);
    }

    public static void toggleSpot(Transform barricade)
    {
      byte x;
      byte y;
      ushort plant;
      ushort index;
      BarricadeRegion region;
      if (!BarricadeManager.tryGetInfo(barricade, out x, out y, out plant, out index, out region))
        return;
      BarricadeManager.manager.channel.send("askToggleSpot", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[4]
      {
        (object) x,
        (object) y,
        (object) plant,
        (object) index
      });
    }

    [SteamCall]
    public void tellToggleSpot(CSteamID steamID, byte x, byte y, ushort plant, ushort index, bool isPowered)
    {
      BarricadeRegion region;
      if (!this.channel.checkServer(steamID) || !BarricadeManager.tryGetRegion(x, y, plant, out region) || !Provider.isServer && !region.isNetworked || (int) index >= region.models.Count)
        return;
      InteractableSpot component = region.models[(int) index].GetComponent<InteractableSpot>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return;
      component.updatePowered(isPowered);
    }

    [SteamCall]
    public void askToggleSpot(CSteamID steamID, byte x, byte y, ushort plant, ushort index)
    {
      BarricadeRegion region;
      if (!Provider.isServer || !BarricadeManager.tryGetRegion(x, y, plant, out region))
        return;
      Player player = PlayerTool.getPlayer(steamID);
      if ((UnityEngine.Object) player == (UnityEngine.Object) null || player.life.isDead || (int) index >= region.models.Count)
        return;
      InteractableSpot component = region.models[(int) index].GetComponent<InteractableSpot>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return;
      if ((int) plant == (int) ushort.MaxValue)
        BarricadeManager.manager.channel.send("tellToggleSpot", ESteamCall.ALL, x, y, BarricadeManager.BARRICADE_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, (object) x, (object) y, (object) plant, (object) index, (object) (bool) (!component.isPowered ? 1 : 0));
      else
        BarricadeManager.manager.channel.send("tellToggleSpot", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[5]
        {
          (object) x,
          (object) y,
          (object) plant,
          (object) index,
          (object) (bool) (!component.isPowered ? 1 : 0)
        });
      region.barricades[(int) index].barricade.state[0] = !component.isPowered ? (byte) 0 : (byte) 1;
      EffectManager.sendEffect((ushort) 8, EffectManager.SMALL, component.transform.position);
    }

    public static void sendFuel(Transform barricade, ushort fuel)
    {
      byte x;
      byte y;
      ushort plant;
      ushort index;
      BarricadeRegion region;
      if (!BarricadeManager.tryGetInfo(barricade, out x, out y, out plant, out index, out region))
        return;
      BarricadeManager.manager.channel.send("tellFuel", ESteamCall.ALL, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[5]
      {
        (object) x,
        (object) y,
        (object) plant,
        (object) index,
        (object) fuel
      });
    }

    [SteamCall]
    public void tellFuel(CSteamID steamID, byte x, byte y, ushort plant, ushort index, ushort fuel)
    {
      BarricadeRegion region;
      if (!this.channel.checkServer(steamID) || !BarricadeManager.tryGetRegion(x, y, plant, out region) || !Provider.isServer && !region.isNetworked || (int) index >= region.models.Count)
        return;
      InteractableGenerator component = region.models[(int) index].GetComponent<InteractableGenerator>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return;
      component.tellFuel(fuel);
    }

    public static void toggleGenerator(Transform barricade)
    {
      byte x;
      byte y;
      ushort plant;
      ushort index;
      BarricadeRegion region;
      if (!BarricadeManager.tryGetInfo(barricade, out x, out y, out plant, out index, out region))
        return;
      BarricadeManager.manager.channel.send("askToggleGenerator", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[4]
      {
        (object) x,
        (object) y,
        (object) plant,
        (object) index
      });
    }

    [SteamCall]
    public void tellToggleGenerator(CSteamID steamID, byte x, byte y, ushort plant, ushort index, bool isPowered)
    {
      BarricadeRegion region;
      if (!this.channel.checkServer(steamID) || !BarricadeManager.tryGetRegion(x, y, plant, out region) || !Provider.isServer && !region.isNetworked || (int) index >= region.models.Count)
        return;
      InteractableGenerator component = region.models[(int) index].GetComponent<InteractableGenerator>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return;
      component.updatePowered(isPowered);
    }

    [SteamCall]
    public void askToggleGenerator(CSteamID steamID, byte x, byte y, ushort plant, ushort index)
    {
      BarricadeRegion region;
      if (!Provider.isServer || !BarricadeManager.tryGetRegion(x, y, plant, out region))
        return;
      Player player = PlayerTool.getPlayer(steamID);
      if ((UnityEngine.Object) player == (UnityEngine.Object) null || player.life.isDead || (int) index >= region.models.Count)
        return;
      InteractableGenerator component = region.models[(int) index].GetComponent<InteractableGenerator>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return;
      if ((int) plant == (int) ushort.MaxValue)
        BarricadeManager.manager.channel.send("tellToggleGenerator", ESteamCall.ALL, x, y, BarricadeManager.BARRICADE_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, (object) x, (object) y, (object) plant, (object) index, (object) (bool) (!component.isPowered ? 1 : 0));
      else
        BarricadeManager.manager.channel.send("tellToggleGenerator", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[5]
        {
          (object) x,
          (object) y,
          (object) plant,
          (object) index,
          (object) (bool) (!component.isPowered ? 1 : 0)
        });
      region.barricades[(int) index].barricade.state[0] = !component.isPowered ? (byte) 0 : (byte) 1;
      EffectManager.sendEffect((ushort) 8, EffectManager.SMALL, component.transform.position);
    }

    public static void toggleFire(Transform barricade)
    {
      byte x;
      byte y;
      ushort plant;
      ushort index;
      BarricadeRegion region;
      if (!BarricadeManager.tryGetInfo(barricade, out x, out y, out plant, out index, out region))
        return;
      BarricadeManager.manager.channel.send("askToggleFire", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[4]
      {
        (object) x,
        (object) y,
        (object) plant,
        (object) index
      });
    }

    [SteamCall]
    public void tellToggleFire(CSteamID steamID, byte x, byte y, ushort plant, ushort index, bool isLit)
    {
      BarricadeRegion region;
      if (!this.channel.checkServer(steamID) || !BarricadeManager.tryGetRegion(x, y, plant, out region) || !Provider.isServer && !region.isNetworked || (int) index >= region.models.Count)
        return;
      InteractableFire component = region.models[(int) index].GetComponent<InteractableFire>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return;
      component.updateLit(isLit);
    }

    [SteamCall]
    public void askToggleFire(CSteamID steamID, byte x, byte y, ushort plant, ushort index)
    {
      BarricadeRegion region;
      if (!Provider.isServer || !BarricadeManager.tryGetRegion(x, y, plant, out region))
        return;
      Player player = PlayerTool.getPlayer(steamID);
      if ((UnityEngine.Object) player == (UnityEngine.Object) null || player.life.isDead || (int) index >= region.models.Count)
        return;
      InteractableFire component = region.models[(int) index].GetComponent<InteractableFire>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return;
      if ((int) plant == (int) ushort.MaxValue)
        BarricadeManager.manager.channel.send("tellToggleFire", ESteamCall.ALL, x, y, BarricadeManager.BARRICADE_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, (object) x, (object) y, (object) plant, (object) index, (object) (bool) (!component.isLit ? 1 : 0));
      else
        BarricadeManager.manager.channel.send("tellToggleFire", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[5]
        {
          (object) x,
          (object) y,
          (object) plant,
          (object) index,
          (object) (bool) (!component.isLit ? 1 : 0)
        });
      region.barricades[(int) index].barricade.state[0] = !component.isLit ? (byte) 0 : (byte) 1;
    }

    public static void farm(Transform barricade)
    {
      byte x;
      byte y;
      ushort plant;
      ushort index;
      BarricadeRegion region;
      if (!BarricadeManager.tryGetInfo(barricade, out x, out y, out plant, out index, out region))
        return;
      BarricadeManager.manager.channel.send("askFarm", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[4]
      {
        (object) x,
        (object) y,
        (object) plant,
        (object) index
      });
    }

    [SteamCall]
    public void tellFarm(CSteamID steamID, byte x, byte y, ushort plant, ushort index, uint planted)
    {
      BarricadeRegion region;
      if (!this.channel.checkServer(steamID) || !BarricadeManager.tryGetRegion(x, y, plant, out region) || !Provider.isServer && !region.isNetworked || (int) index >= region.models.Count)
        return;
      InteractableFarm component = region.models[(int) index].GetComponent<InteractableFarm>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return;
      component.updatePlanted(planted);
    }

    [SteamCall]
    public void askFarm(CSteamID steamID, byte x, byte y, ushort plant, ushort index)
    {
      BarricadeRegion region;
      if (!Provider.isServer || !BarricadeManager.tryGetRegion(x, y, plant, out region))
        return;
      Player player = PlayerTool.getPlayer(steamID);
      if ((UnityEngine.Object) player == (UnityEngine.Object) null || player.life.isDead || (int) index >= region.models.Count)
        return;
      InteractableFarm component = region.models[(int) index].GetComponent<InteractableFarm>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || !component.checkFarm())
        return;
      player.inventory.forceAddItem(new Item(component.grow, true), true);
      if ((double) UnityEngine.Random.value < (double) player.skills.mastery(2, 5))
        player.inventory.forceAddItem(new Item(component.grow, true), true);
      BarricadeManager.damage(component.transform, 2f, 1f);
      player.skills.askAward(1U);
    }

    public static void updateFarm(Transform barricade, uint planted)
    {
      byte x;
      byte y;
      ushort plant;
      ushort index;
      BarricadeRegion region;
      if (!BarricadeManager.tryGetInfo(barricade, out x, out y, out plant, out index, out region))
        return;
      if ((int) plant == (int) ushort.MaxValue)
        BarricadeManager.manager.channel.send("tellFarm", ESteamCall.ALL, x, y, BarricadeManager.BARRICADE_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, (object) x, (object) y, (object) plant, (object) index, (object) planted);
      else
        BarricadeManager.manager.channel.send("tellFarm", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[5]
        {
          (object) x,
          (object) y,
          (object) plant,
          (object) index,
          (object) planted
        });
      BitConverter.GetBytes(planted).CopyTo((Array) region.barricades[(int) index].barricade.state, 0);
    }

    public static void storeStorage(Transform barricade)
    {
      byte x;
      byte y;
      ushort plant;
      ushort index;
      BarricadeRegion region;
      if (!BarricadeManager.tryGetInfo(barricade, out x, out y, out plant, out index, out region))
        return;
      BarricadeManager.manager.channel.send("askStoreStorage", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[4]
      {
        (object) x,
        (object) y,
        (object) plant,
        (object) index
      });
    }

    [SteamCall]
    public void askStoreStorage(CSteamID steamID, byte x, byte y, ushort plant, ushort index)
    {
      BarricadeRegion region;
      if (!Provider.isServer || !BarricadeManager.tryGetRegion(x, y, plant, out region))
        return;
      Player player = PlayerTool.getPlayer(steamID);
      if ((UnityEngine.Object) player == (UnityEngine.Object) null || player.life.isDead || (int) index >= region.models.Count)
        return;
      InteractableStorage component = region.models[(int) index].GetComponent<InteractableStorage>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || !component.checkStore(player.channel.owner.playerID.steamID, player.channel.owner.playerID.group))
        return;
      component.isOpen = true;
      player.inventory.isStoring = true;
      player.inventory.storage = component;
      player.inventory.updateItems(PlayerInventory.STORAGE, component.items);
      player.inventory.sendStorage();
    }

    public static void toggleDoor(Transform barricade)
    {
      byte x;
      byte y;
      ushort plant;
      ushort index;
      BarricadeRegion region;
      if (!BarricadeManager.tryGetInfo(barricade, out x, out y, out plant, out index, out region))
        return;
      BarricadeManager.manager.channel.send("askToggleDoor", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[4]
      {
        (object) x,
        (object) y,
        (object) plant,
        (object) index
      });
    }

    [SteamCall]
    public void tellToggleDoor(CSteamID steamID, byte x, byte y, ushort plant, ushort index, bool isOpen)
    {
      BarricadeRegion region;
      if (!this.channel.checkServer(steamID) || !BarricadeManager.tryGetRegion(x, y, plant, out region) || !Provider.isServer && !region.isNetworked || (int) index >= region.models.Count)
        return;
      InteractableDoor component = region.models[(int) index].FindChild("Skeleton").FindChild("Hinge").GetComponent<InteractableDoor>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return;
      component.updateToggle(isOpen);
    }

    [SteamCall]
    public void askToggleDoor(CSteamID steamID, byte x, byte y, ushort plant, ushort index)
    {
      BarricadeRegion region;
      if (!Provider.isServer || !BarricadeManager.tryGetRegion(x, y, plant, out region))
        return;
      Player player = PlayerTool.getPlayer(steamID);
      if ((UnityEngine.Object) player == (UnityEngine.Object) null || player.life.isDead || (int) index >= region.models.Count)
        return;
      InteractableDoor component = region.models[(int) index].FindChild("Skeleton").FindChild("Hinge").GetComponent<InteractableDoor>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || !component.isOpenable || !component.checkToggle(player.channel.owner.playerID.steamID, player.channel.owner.playerID.group))
        return;
      if ((int) plant == (int) ushort.MaxValue)
        BarricadeManager.manager.channel.send("tellToggleDoor", ESteamCall.ALL, x, y, BarricadeManager.BARRICADE_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, (object) x, (object) y, (object) plant, (object) index, (object) (bool) (!component.isOpen ? 1 : 0));
      else
        BarricadeManager.manager.channel.send("tellToggleDoor", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[5]
        {
          (object) x,
          (object) y,
          (object) plant,
          (object) index,
          (object) (bool) (!component.isOpen ? 1 : 0)
        });
      region.barricades[(int) index].barricade.state[16] = !component.isOpen ? (byte) 0 : (byte) 1;
    }

    public static bool tryGetBed(CSteamID owner, out Vector3 point, out byte angle)
    {
      point = Vector3.zero;
      angle = (byte) 0;
      for (byte index1 = (byte) 0; (int) index1 < (int) Regions.WORLD_SIZE; ++index1)
      {
        for (byte index2 = (byte) 0; (int) index2 < (int) Regions.WORLD_SIZE; ++index2)
        {
          BarricadeRegion barricadeRegion = BarricadeManager.regions[(int) index1, (int) index2];
          for (ushort index3 = (ushort) 0; (int) index3 < BarricadeManager.regions[(int) index1, (int) index2].barricades.Count; ++index3)
          {
            if (barricadeRegion.barricades[(int) index3].barricade.state.Length > 0)
            {
              if ((int) index3 < barricadeRegion.models.Count)
              {
                InteractableBed component = barricadeRegion.models[(int) index3].GetComponent<InteractableBed>();
                if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.owner == owner && Level.checkSafe(component.transform.position))
                {
                  point = component.transform.position;
                  angle = MeasurementTool.angleToByte((float) ((int) barricadeRegion.barricades[(int) index3].angle_y * 2 + 90));
                  return true;
                }
              }
              else
                break;
            }
          }
        }
      }
      for (ushort index1 = (ushort) 0; (int) index1 < BarricadeManager.plants.Count; ++index1)
      {
        BarricadeRegion barricadeRegion = BarricadeManager.plants[(int) index1];
        for (ushort index2 = (ushort) 0; (int) index2 < barricadeRegion.barricades.Count; ++index2)
        {
          if (barricadeRegion.barricades[(int) index2].barricade.state.Length > 0)
          {
            if ((int) index2 < barricadeRegion.models.Count)
            {
              InteractableBed component = barricadeRegion.models[(int) index2].GetComponent<InteractableBed>();
              if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.owner == owner && Level.checkSafe(component.transform.position))
              {
                point = component.transform.position;
                angle = MeasurementTool.angleToByte((float) ((int) barricadeRegion.barricades[(int) index2].angle_y * 2 + 90));
                return true;
              }
            }
            else
              break;
          }
        }
      }
      return false;
    }

    public static void unclaimBeds(CSteamID owner)
    {
      for (byte x = (byte) 0; (int) x < (int) Regions.WORLD_SIZE; ++x)
      {
        for (byte y = (byte) 0; (int) y < (int) Regions.WORLD_SIZE; ++y)
        {
          BarricadeRegion barricadeRegion = BarricadeManager.regions[(int) x, (int) y];
          for (ushort index = (ushort) 0; (int) index < barricadeRegion.barricades.Count; ++index)
          {
            if (barricadeRegion.barricades[(int) index].barricade.state.Length > 0)
            {
              if ((int) index < barricadeRegion.models.Count)
              {
                InteractableBed component = barricadeRegion.models[(int) index].GetComponent<InteractableBed>();
                if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.owner == owner)
                {
                  BarricadeManager.manager.channel.send("tellClaimBed", ESteamCall.ALL, x, y, BarricadeManager.BARRICADE_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, (object) x, (object) y, (object) ushort.MaxValue, (object) index, (object) CSteamID.Nil);
                  BitConverter.GetBytes(component.owner.m_SteamID).CopyTo((Array) barricadeRegion.barricades[(int) index].barricade.state, 0);
                  return;
                }
              }
              else
                break;
            }
          }
        }
      }
      for (ushort index1 = (ushort) 0; (int) index1 < BarricadeManager.plants.Count; ++index1)
      {
        BarricadeRegion barricadeRegion = BarricadeManager.plants[(int) index1];
        for (ushort index2 = (ushort) 0; (int) index2 < barricadeRegion.barricades.Count; ++index2)
        {
          if (barricadeRegion.barricades[(int) index2].barricade.state.Length > 0)
          {
            if ((int) index2 < barricadeRegion.models.Count)
            {
              InteractableBed component = barricadeRegion.models[(int) index2].GetComponent<InteractableBed>();
              if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.owner == owner)
              {
                BarricadeManager.manager.channel.send("tellClaimBed", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[5]
                {
                  (object) byte.MaxValue,
                  (object) byte.MaxValue,
                  (object) index1,
                  (object) index2,
                  (object) CSteamID.Nil
                });
                BitConverter.GetBytes(component.owner.m_SteamID).CopyTo((Array) barricadeRegion.barricades[(int) index2].barricade.state, 0);
                return;
              }
            }
            else
              break;
          }
        }
      }
    }

    public static void claimBed(Transform barricade)
    {
      byte x;
      byte y;
      ushort plant;
      ushort index;
      BarricadeRegion region;
      if (!BarricadeManager.tryGetInfo(barricade, out x, out y, out plant, out index, out region))
        return;
      BarricadeManager.manager.channel.send("askClaimBed", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[4]
      {
        (object) x,
        (object) y,
        (object) plant,
        (object) index
      });
    }

    [SteamCall]
    public void tellClaimBed(CSteamID steamID, byte x, byte y, ushort plant, ushort index, CSteamID owner)
    {
      BarricadeRegion region;
      if (!this.channel.checkServer(steamID) || !BarricadeManager.tryGetRegion(x, y, plant, out region) || !Provider.isServer && !region.isNetworked || (int) index >= region.models.Count)
        return;
      InteractableBed component = region.models[(int) index].GetComponent<InteractableBed>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return;
      component.updateClaim(owner);
    }

    [SteamCall]
    public void askClaimBed(CSteamID steamID, byte x, byte y, ushort plant, ushort index)
    {
      BarricadeRegion region;
      if (!Provider.isServer || !BarricadeManager.tryGetRegion(x, y, plant, out region))
        return;
      Player player = PlayerTool.getPlayer(steamID);
      if ((UnityEngine.Object) player == (UnityEngine.Object) null || player.life.isDead || (int) index >= region.models.Count)
        return;
      InteractableBed component = region.models[(int) index].GetComponent<InteractableBed>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || !component.isClaimable || !component.checkClaim(player.channel.owner.playerID.steamID))
        return;
      if (component.isClaimed)
      {
        if ((int) plant == (int) ushort.MaxValue)
          BarricadeManager.manager.channel.send("tellClaimBed", ESteamCall.ALL, x, y, BarricadeManager.BARRICADE_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, (object) x, (object) y, (object) plant, (object) index, (object) CSteamID.Nil);
        else
          BarricadeManager.manager.channel.send("tellClaimBed", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[5]
          {
            (object) x,
            (object) y,
            (object) plant,
            (object) index,
            (object) CSteamID.Nil
          });
      }
      else
      {
        BarricadeManager.unclaimBeds(player.channel.owner.playerID.steamID);
        if ((int) plant == (int) ushort.MaxValue)
          BarricadeManager.manager.channel.send("tellClaimBed", ESteamCall.ALL, x, y, BarricadeManager.BARRICADE_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, (object) x, (object) y, (object) plant, (object) index, (object) player.channel.owner.playerID.steamID);
        else
          BarricadeManager.manager.channel.send("tellClaimBed", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[5]
          {
            (object) x,
            (object) y,
            (object) plant,
            (object) index,
            (object) player.channel.owner.playerID.steamID
          });
      }
      BitConverter.GetBytes(component.owner.m_SteamID).CopyTo((Array) region.barricades[(int) index].barricade.state, 0);
    }

    public static void damage(Transform barricade, float damage, float times)
    {
      byte x;
      byte y;
      ushort plant;
      ushort index;
      BarricadeRegion region;
      if (!BarricadeManager.tryGetInfo(barricade, out x, out y, out plant, out index, out region) || region.barricades[(int) index].barricade.isDead)
        return;
      ushort amount = (ushort) ((double) damage * (double) times);
      region.barricades[(int) index].barricade.askDamage(amount);
      if (!region.barricades[(int) index].barricade.isDead)
        return;
      ItemBarricadeAsset itemBarricadeAsset = (ItemBarricadeAsset) Assets.find(EAssetType.ITEM, region.barricades[(int) index].barricade.id);
      if (itemBarricadeAsset != null && (int) itemBarricadeAsset.explosion != 0)
      {
        if ((int) plant == (int) ushort.MaxValue)
          EffectManager.sendEffect(itemBarricadeAsset.explosion, x, y, BarricadeManager.BARRICADE_REGIONS, barricade.position + Vector3.down * itemBarricadeAsset.offset);
        else
          EffectManager.sendEffect(itemBarricadeAsset.explosion, EffectManager.MEDIUM, barricade.position + Vector3.down * itemBarricadeAsset.offset);
      }
      region.barricades.RemoveAt((int) index);
      if ((int) plant == (int) ushort.MaxValue)
        BarricadeManager.manager.channel.send("tellTakeBarricade", ESteamCall.ALL, x, y, BarricadeManager.BARRICADE_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, (object) x, (object) y, (object) plant, (object) index);
      else
        BarricadeManager.manager.channel.send("tellTakeBarricade", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[4]
        {
          (object) x,
          (object) y,
          (object) plant,
          (object) index
        });
    }

    public static void dropBarricade(Barricade barricade, Transform hit, Vector3 point, float angle_x, float angle_y, float angle_z, ulong owner, ulong group)
    {
      if ((ItemBarricadeAsset) Assets.find(EAssetType.ITEM, barricade.id) == null)
        return;
      if ((UnityEngine.Object) hit != (UnityEngine.Object) null && hit.transform.tag == "Vehicle")
      {
        byte x;
        byte y;
        ushort plant;
        BarricadeRegion region;
        if (!BarricadeManager.tryGetPlant(hit, out x, out y, out plant, out region))
          return;
        region.barricades.Add(new BarricadeData(barricade, point, MeasurementTool.angleToByte(angle_x), MeasurementTool.angleToByte(angle_y), MeasurementTool.angleToByte(angle_z), owner, group));
        BarricadeManager.manager.channel.send("tellBarricade", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, (object) x, (object) y, (object) plant, (object) barricade.id, (object) barricade.state, (object) point, (object) MeasurementTool.angleToByte(angle_x), (object) MeasurementTool.angleToByte(angle_y), (object) MeasurementTool.angleToByte(angle_z), (object) owner, (object) group);
      }
      else
      {
        byte x;
        byte y;
        BarricadeRegion region;
        if (!Regions.tryGetCoordinate(point, out x, out y) || !BarricadeManager.tryGetRegion(x, y, ushort.MaxValue, out region))
          return;
        region.barricades.Add(new BarricadeData(barricade, point, MeasurementTool.angleToByte(angle_x), MeasurementTool.angleToByte(angle_y), MeasurementTool.angleToByte(angle_z), owner, group));
        BarricadeManager.manager.channel.send("tellBarricade", ESteamCall.ALL, x, y, BarricadeManager.BARRICADE_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, (object) x, (object) y, (object) ushort.MaxValue, (object) barricade.id, (object) barricade.state, (object) point, (object) MeasurementTool.angleToByte(angle_x), (object) MeasurementTool.angleToByte(angle_y), (object) MeasurementTool.angleToByte(angle_z), (object) owner, (object) group);
      }
    }

    [SteamCall]
    public void tellTakeBarricade(CSteamID steamID, byte x, byte y, ushort plant, ushort index)
    {
      BarricadeRegion region;
      if (!this.channel.checkServer(steamID) || !BarricadeManager.tryGetRegion(x, y, plant, out region) || !Provider.isServer && !region.isNetworked || (int) index >= region.models.Count)
        return;
      UnityEngine.Object.Destroy((UnityEngine.Object) region.models[(int) index].gameObject);
      region.models.RemoveAt((int) index);
    }

    private void spawnBarricade(BarricadeRegion region, ushort id, byte[] state, Vector3 point, byte angle_x, byte angle_y, byte angle_z, bool hasOwnership)
    {
      ItemBarricadeAsset asset = (ItemBarricadeAsset) Assets.find(EAssetType.ITEM, id);
      if (asset != null)
      {
        Quaternion rot = Quaternion.Euler(0.0f, (float) ((int) angle_y * 2), 0.0f) * Quaternion.Euler((float) ((asset.build == EBuild.DOOR || asset.build == EBuild.GATE ? 0 : -90) + (int) angle_x * 2), 0.0f, 0.0f) * Quaternion.Euler(0.0f, (float) ((int) angle_z * 2), 0.0f);
        Transform barricade = BarricadeTool.getBarricade(region.parent, hasOwnership, point, rot, id, state, asset);
        region.models.Add(barricade);
        if (!((UnityEngine.Object) region.parent != (UnityEngine.Object) LevelBarricades.models))
          return;
        barricade.gameObject.SetActive(false);
        barricade.gameObject.SetActive(true);
        barricade.gameObject.AddComponent<Rigidbody>();
        barricade.GetComponent<Rigidbody>().useGravity = false;
        barricade.GetComponent<Rigidbody>().isKinematic = true;
        barricade.gameObject.layer = LayerMasks.RESOURCE;
        Collider component = barricade.GetComponent<Collider>();
        Collider[] components = region.parent.GetComponents<Collider>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        {
          for (int index = 0; index < components.Length; ++index)
            Physics.IgnoreCollision(components[index], component);
          if (Provider.isServer)
            Physics.IgnoreCollision(region.parent.FindChild("Bumper").GetComponent<Collider>(), component);
        }
        Collider[] componentsInChildren = barricade.GetComponentsInChildren<Collider>();
        for (int index1 = 0; index1 < componentsInChildren.Length; ++index1)
        {
          if (!((UnityEngine.Object) componentsInChildren[index1].transform == (UnityEngine.Object) barricade))
          {
            componentsInChildren[index1].gameObject.AddComponent<Rigidbody>();
            componentsInChildren[index1].GetComponent<Rigidbody>().useGravity = false;
            componentsInChildren[index1].GetComponent<Rigidbody>().isKinematic = true;
            if (componentsInChildren[index1].gameObject.layer == LayerMasks.BARRICADE)
              componentsInChildren[index1].gameObject.layer = LayerMasks.RESOURCE;
            for (int index2 = 0; index2 < components.Length; ++index2)
              Physics.IgnoreCollision(components[index2], componentsInChildren[index1]);
          }
        }
      }
      else
      {
        if (Provider.isServer)
          return;
        Provider.disconnect();
      }
    }

    [SteamCall]
    public void tellBarricade(CSteamID steamID, byte x, byte y, ushort plant, ushort id, byte[] state, Vector3 point, byte angle_x, byte angle_y, byte angle_z, ulong owner, ulong group)
    {
      BarricadeRegion region;
      if (!this.channel.checkServer(steamID) || !BarricadeManager.tryGetRegion(x, y, plant, out region) || !Provider.isServer && !region.isNetworked)
        return;
      this.spawnBarricade(region, id, state, point, angle_x, angle_y, angle_z, OwnershipTool.checkToggle(owner, group));
    }

    [SteamCall]
    public void tellBarricades(CSteamID steamID)
    {
      if (!this.channel.checkServer(steamID))
        return;
      BarricadeRegion region;
      if (BarricadeManager.tryGetRegion((byte) this.channel.read(Types.BYTE_TYPE), (byte) this.channel.read(Types.BYTE_TYPE), (ushort) this.channel.read(Types.UINT16_TYPE), out region))
      {
        if ((int) (byte) this.channel.read(Types.BYTE_TYPE) == 0)
        {
          if (region.isNetworked)
            return;
        }
        else if (!region.isNetworked)
          return;
        region.isNetworked = true;
        ushort num = (ushort) this.channel.read(Types.UINT16_TYPE);
        for (int index = 0; index < (int) num; ++index)
        {
          object[] objArray = this.channel.read(Types.UINT16_TYPE, Types.BYTE_ARRAY_TYPE, Types.VECTOR3_TYPE, Types.BYTE_TYPE, Types.BYTE_TYPE, Types.BYTE_TYPE, Types.BOOLEAN_TYPE);
          this.spawnBarricade(region, (ushort) objArray[0], (byte[]) objArray[1], (Vector3) objArray[2], (byte) objArray[3], (byte) objArray[4], (byte) objArray[5], (bool) objArray[6]);
        }
      }
      Level.isLoadingBarricades = false;
    }

    public void askBarricades(CSteamID steamID, byte x, byte y, ushort plant)
    {
      SteamPlayer steamPlayer = PlayerTool.getSteamPlayer(steamID);
      BarricadeRegion region;
      if (!BarricadeManager.tryGetRegion(x, y, plant, out region))
        return;
      if (region.barricades.Count > 0)
      {
        byte num1 = (byte) 0;
        int index1 = 0;
        int index2 = 0;
        while (index1 < region.barricades.Count)
        {
          int num2 = 0;
          while (index2 < region.barricades.Count)
          {
            num2 += 2 + region.barricades[index2].barricade.state.Length + 12 + 1 + 1 + 1 + 1;
            ++index2;
            if (num2 > Block.BUFFER_SIZE / 2)
              break;
          }
          this.channel.openWrite();
          this.channel.write((object) x);
          this.channel.write((object) y);
          this.channel.write((object) plant);
          this.channel.write((object) num1);
          this.channel.write((object) (ushort) (index2 - index1));
          for (; index1 < index2; ++index1)
          {
            BarricadeData barricadeData = region.barricades[index1];
            if ((UnityEngine.Object) region.models[index1].GetComponent<InteractableStorage>() != (UnityEngine.Object) null)
            {
              byte[] numArray = new byte[16];
              Array.Copy((Array) barricadeData.barricade.state, 0, (Array) numArray, 0, 16);
              this.channel.write((object) barricadeData.barricade.id, (object) numArray, (object) barricadeData.point, (object) barricadeData.angle_x, (object) barricadeData.angle_y, (object) barricadeData.angle_z, (object) (bool) (OwnershipTool.checkToggle(steamID, barricadeData.owner, steamPlayer.playerID.group, barricadeData.group) ? 1 : 0));
            }
            else
              this.channel.write((object) barricadeData.barricade.id, (object) barricadeData.barricade.state, (object) barricadeData.point, (object) barricadeData.angle_x, (object) barricadeData.angle_y, (object) barricadeData.angle_z, (object) (bool) (OwnershipTool.checkToggle(steamID, barricadeData.owner, steamPlayer.playerID.group, barricadeData.group) ? 1 : 0));
          }
          this.channel.closeWrite("tellBarricades", steamID, ESteamPacket.UPDATE_RELIABLE_CHUNK_BUFFER);
          ++num1;
        }
      }
      else
      {
        this.channel.openWrite();
        this.channel.write((object) x);
        this.channel.write((object) y);
        this.channel.write((object) plant);
        this.channel.write((object) 0);
        this.channel.write((object) 0);
        this.channel.closeWrite("tellBarricades", steamID, ESteamPacket.UPDATE_RELIABLE_CHUNK_BUFFER);
      }
    }

    public static void clearPlants()
    {
      BarricadeManager.plants = new List<BarricadeRegion>();
    }

    public static void waterPlant(Transform parent)
    {
      BarricadeManager.plants.Add(new BarricadeRegion(parent)
      {
        isNetworked = false
      });
    }

    public static void uprootPlant(Transform parent)
    {
      for (ushort index = (ushort) 0; (int) index < BarricadeManager.plants.Count; ++index)
      {
        if ((UnityEngine.Object) BarricadeManager.plants[(int) index].parent == (UnityEngine.Object) parent)
        {
          BarricadeManager.plants.RemoveAt((int) index);
          break;
        }
      }
    }

    public static void trimPlant(Transform parent)
    {
      for (ushort index = (ushort) 0; (int) index < BarricadeManager.plants.Count; ++index)
      {
        BarricadeRegion barricadeRegion = BarricadeManager.plants[(int) index];
        if ((UnityEngine.Object) barricadeRegion.parent == (UnityEngine.Object) parent)
        {
          barricadeRegion.barricades.Clear();
          barricadeRegion.destroy();
          break;
        }
      }
    }

    public static void askPlants(CSteamID steamID)
    {
      for (ushort plant = (ushort) 0; (int) plant < BarricadeManager.plants.Count; ++plant)
        BarricadeManager.manager.askBarricades(steamID, byte.MaxValue, byte.MaxValue, plant);
    }

    public static void askPlants(Transform parent)
    {
      byte x;
      byte y;
      ushort plant;
      BarricadeRegion region;
      if (!BarricadeManager.tryGetPlant(parent, out x, out y, out plant, out region))
        return;
      BarricadeManager.manager.channel.openWrite();
      BarricadeManager.manager.channel.write((object) x);
      BarricadeManager.manager.channel.write((object) y);
      BarricadeManager.manager.channel.write((object) plant);
      BarricadeManager.manager.channel.write((object) 0);
      BarricadeManager.manager.channel.write((object) 0);
      BarricadeManager.manager.channel.closeWrite("tellBarricades", ESteamCall.OTHERS, ESteamPacket.UPDATE_RELIABLE_CHUNK_BUFFER);
    }

    public static bool tryGetInfo(Transform barricade, out byte x, out byte y, out ushort plant, out ushort index, out BarricadeRegion region)
    {
      x = (byte) 0;
      y = (byte) 0;
      plant = (ushort) 0;
      index = (ushort) 0;
      region = (BarricadeRegion) null;
      if (BarricadeManager.tryGetRegion(barricade, out x, out y, out plant, out region))
      {
        index = (ushort) 0;
        while ((int) index < region.models.Count)
        {
          if ((UnityEngine.Object) barricade == (UnityEngine.Object) region.models[(int) index])
            return true;
          index = (ushort) ((uint) index + 1U);
        }
      }
      return false;
    }

    public static bool tryGetPlant(Transform parent, out byte x, out byte y, out ushort plant, out BarricadeRegion region)
    {
      x = byte.MaxValue;
      y = byte.MaxValue;
      plant = ushort.MaxValue;
      region = (BarricadeRegion) null;
      if ((UnityEngine.Object) parent == (UnityEngine.Object) null)
        return false;
      plant = (ushort) 0;
      while ((int) plant < BarricadeManager.plants.Count)
      {
        region = BarricadeManager.plants[(int) plant];
        if ((UnityEngine.Object) region.parent == (UnityEngine.Object) parent)
          return true;
        plant = (ushort) ((uint) plant + 1U);
      }
      return false;
    }

    public static bool tryGetRegion(Transform barricade, out byte x, out byte y, out ushort plant, out BarricadeRegion region)
    {
      x = (byte) 0;
      y = (byte) 0;
      plant = (ushort) 0;
      region = (BarricadeRegion) null;
      if ((UnityEngine.Object) barricade == (UnityEngine.Object) null)
        return false;
      if (barricade.parent.tag == "Vehicle")
      {
        plant = (ushort) 0;
        while ((int) plant < BarricadeManager.plants.Count)
        {
          region = BarricadeManager.plants[(int) plant];
          if ((UnityEngine.Object) region.parent == (UnityEngine.Object) barricade.parent)
            return true;
          plant = (ushort) ((uint) plant + 1U);
        }
      }
      else
      {
        plant = ushort.MaxValue;
        if (Regions.tryGetCoordinate(barricade.position, out x, out y))
        {
          region = BarricadeManager.regions[(int) x, (int) y];
          return true;
        }
      }
      return false;
    }

    public static bool tryGetRegion(byte x, byte y, ushort plant, out BarricadeRegion region)
    {
      region = (BarricadeRegion) null;
      if ((int) plant < (int) ushort.MaxValue)
      {
        if ((int) plant >= BarricadeManager.plants.Count)
          return false;
        region = BarricadeManager.plants[(int) plant];
        return true;
      }
      if (!Regions.checkSafe(x, y))
        return false;
      region = BarricadeManager.regions[(int) x, (int) y];
      return true;
    }

    public static void updateState(Transform barricade, byte[] state, int size)
    {
      byte x;
      byte y;
      ushort plant;
      ushort index;
      BarricadeRegion region;
      if (!BarricadeManager.tryGetInfo(barricade, out x, out y, out plant, out index, out region))
        return;
      region.barricades[(int) index].barricade.state = new byte[size];
      Array.Copy((Array) state, (Array) region.barricades[(int) index].barricade.state, size);
    }

    private void onLevelLoaded(int level)
    {
      if (level <= Level.SETUP)
        return;
      BarricadeManager.regions = new BarricadeRegion[(int) Regions.WORLD_SIZE, (int) Regions.WORLD_SIZE];
      for (byte index1 = (byte) 0; (int) index1 < (int) Regions.WORLD_SIZE; ++index1)
      {
        for (byte index2 = (byte) 0; (int) index2 < (int) Regions.WORLD_SIZE; ++index2)
          BarricadeManager.regions[(int) index1, (int) index2] = new BarricadeRegion(LevelBarricades.models);
      }
      if (!Provider.isServer)
        return;
      BarricadeManager.load();
    }

    private void onRegionUpdated(Player player, byte old_x, byte old_y, byte new_x, byte new_y)
    {
      for (byte x_0 = (byte) 0; (int) x_0 < (int) Regions.WORLD_SIZE; ++x_0)
      {
        for (byte y_0 = (byte) 0; (int) y_0 < (int) Regions.WORLD_SIZE; ++y_0)
        {
          if (Provider.isServer)
          {
            if (player.movement.loadedRegions[(int) x_0, (int) y_0].isBarricadesLoaded && !Regions.checkArea(x_0, y_0, new_x, new_y, BarricadeManager.BARRICADE_REGIONS))
              player.movement.loadedRegions[(int) x_0, (int) y_0].isBarricadesLoaded = false;
          }
          else if (player.channel.isOwner && BarricadeManager.regions[(int) x_0, (int) y_0].isNetworked && !Regions.checkArea(x_0, y_0, new_x, new_y, BarricadeManager.BARRICADE_REGIONS))
          {
            BarricadeManager.regions[(int) x_0, (int) y_0].destroy();
            BarricadeManager.regions[(int) x_0, (int) y_0].isNetworked = false;
          }
        }
      }
      if (!Dedicator.isDedicated || !Regions.checkSafe(new_x, new_y))
        return;
      for (int index1 = (int) new_x - (int) BarricadeManager.BARRICADE_REGIONS; index1 <= (int) new_x + (int) BarricadeManager.BARRICADE_REGIONS; ++index1)
      {
        for (int index2 = (int) new_y - (int) BarricadeManager.BARRICADE_REGIONS; index2 <= (int) new_y + (int) BarricadeManager.BARRICADE_REGIONS; ++index2)
        {
          if (Regions.checkSafe((byte) index1, (byte) index2) && !player.movement.loadedRegions[index1, index2].isBarricadesLoaded)
          {
            player.movement.loadedRegions[index1, index2].isBarricadesLoaded = true;
            this.askBarricades(player.channel.owner.playerID.steamID, (byte) index1, (byte) index2, ushort.MaxValue);
          }
        }
      }
    }

    private void onPlayerCreated(Player player)
    {
      player.movement.onRegionUpdated += new PlayerRegionUpdated(this.onRegionUpdated);
    }

    private void Start()
    {
      BarricadeManager.manager = this;
      Level.onPreLevelLoaded += new PreLevelLoaded(this.onLevelLoaded);
      Player.onPlayerCreated += new PlayerCreated(this.onPlayerCreated);
    }

    public static void load()
    {
      if (LevelSavedata.fileExists("/Barricades.dat") && Level.info.type == ELevelType.SURVIVAL)
      {
        River river = LevelSavedata.openRiver("/Barricades.dat", true);
        byte version = river.readByte();
        if ((int) version > 0)
        {
          for (byte index1 = (byte) 0; (int) index1 < (int) Regions.WORLD_SIZE; ++index1)
          {
            for (byte index2 = (byte) 0; (int) index2 < (int) Regions.WORLD_SIZE; ++index2)
            {
              BarricadeRegion region = BarricadeManager.regions[(int) index1, (int) index2];
              BarricadeManager.loadRegion(version, river, region);
            }
          }
          if ((int) version > 1)
          {
            ushort num = river.readUInt16();
            for (int index = 0; index < Mathf.Min((int) num, BarricadeManager.plants.Count); ++index)
            {
              BarricadeRegion region = BarricadeManager.plants[index];
              BarricadeManager.loadRegion(version, river, region);
            }
          }
        }
      }
      Level.isLoadingBarricades = false;
    }

    public static void save()
    {
      River river = LevelSavedata.openRiver("/Barricades.dat", false);
      river.writeByte(BarricadeManager.SAVEDATA_VERSION);
      for (byte index1 = (byte) 0; (int) index1 < (int) Regions.WORLD_SIZE; ++index1)
      {
        for (byte index2 = (byte) 0; (int) index2 < (int) Regions.WORLD_SIZE; ++index2)
        {
          BarricadeRegion region = BarricadeManager.regions[(int) index1, (int) index2];
          BarricadeManager.saveRegion(river, region);
        }
      }
      ushort num = (ushort) 0;
      for (ushort index = (ushort) 0; (int) index < BarricadeManager.plants.Count; ++index)
      {
        InteractableVehicle component = BarricadeManager.plants[(int) index].parent.GetComponent<InteractableVehicle>();
        if (!component.isExploded && !component.isUnderwater)
          ++num;
      }
      river.writeUInt16(num);
      for (int index = 0; index < (int) num; ++index)
      {
        BarricadeRegion region = BarricadeManager.plants[index];
        InteractableVehicle component = BarricadeManager.plants[index].parent.GetComponent<InteractableVehicle>();
        if (!component.isExploded && !component.isUnderwater)
          BarricadeManager.saveRegion(river, region);
      }
      river.closeRiver();
    }

    private static void loadRegion(byte version, River river, BarricadeRegion region)
    {
      ushort num1 = river.readUInt16();
      for (ushort index = (ushort) 0; (int) index < (int) num1; ++index)
      {
        ushort num2 = river.readUInt16();
        ushort newHealth = river.readUInt16();
        byte[] numArray = river.readBytes();
        Vector3 vector3 = river.readSingleVector3();
        byte num3 = (byte) 0;
        if ((int) version > 2)
          num3 = river.readByte();
        byte num4 = river.readByte();
        byte num5 = (byte) 0;
        if ((int) version > 3)
          num5 = river.readByte();
        ulong num6 = 0UL;
        ulong num7 = 0UL;
        if ((int) version > 4)
        {
          num6 = river.readUInt64();
          num7 = river.readUInt64();
        }
        region.barricades.Add(new BarricadeData(new Barricade(num2, newHealth, numArray), vector3, num3, num4, num5, num6, num7));
        if (Provider.isServer)
          BarricadeManager.manager.spawnBarricade(region, num2, numArray, vector3, num3, num4, num5, OwnershipTool.checkToggle(num6, num7));
      }
    }

    private static void saveRegion(River river, BarricadeRegion region)
    {
      river.writeUInt16((ushort) region.barricades.Count);
      for (ushort index = (ushort) 0; (int) index < region.barricades.Count; ++index)
      {
        BarricadeData barricadeData = region.barricades[(int) index];
        river.writeUInt16(barricadeData.barricade.id);
        river.writeUInt16(barricadeData.barricade.health);
        river.writeBytes(barricadeData.barricade.state);
        river.writeSingleVector3(barricadeData.point);
        river.writeByte(barricadeData.angle_x);
        river.writeByte(barricadeData.angle_y);
        river.writeByte(barricadeData.angle_z);
        river.writeUInt64(barricadeData.owner);
        river.writeUInt64(barricadeData.group);
      }
    }
  }
}
