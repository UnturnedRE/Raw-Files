// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.PlayerClothing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
  public class PlayerClothing : PlayerCaller
  {
    public static readonly byte SAVEDATA_VERSION = (byte) 5;
    public ShirtUpdated onShirtUpdated;
    public PantsUpdated onPantsUpdated;
    public HatUpdated onHatUpdated;
    public BackpackUpdated onBackpackUpdated;
    public VestUpdated onVestUpdated;
    public MaskUpdated onMaskUpdated;
    public GlassesUpdated onGlassesUpdated;
    private HumanClothes firstClothes;
    private HumanClothes thirdClothes;
    private HumanClothes characterClothes;
    public byte shirtQuality;
    public byte pantsQuality;
    public byte hatQuality;
    public byte backpackQuality;
    public byte vestQuality;
    public byte maskQuality;
    public byte glassesQuality;
    public byte[] shirtState;
    public byte[] pantsState;
    public byte[] hatState;
    public byte[] backpackState;
    public byte[] vestState;
    public byte[] maskState;
    public byte[] glassesState;

    public bool isVisual
    {
      get
      {
        return this.thirdClothes.isVisual;
      }
    }

    public ItemShirtAsset shirtAsset
    {
      get
      {
        return this.thirdClothes.shirtAsset;
      }
    }

    public ItemPantsAsset pantsAsset
    {
      get
      {
        return this.thirdClothes.pantsAsset;
      }
    }

    public ItemHatAsset hatAsset
    {
      get
      {
        return this.thirdClothes.hatAsset;
      }
    }

    public ItemBackpackAsset backpackAsset
    {
      get
      {
        return this.thirdClothes.backpackAsset;
      }
    }

    public ItemVestAsset vestAsset
    {
      get
      {
        return this.thirdClothes.vestAsset;
      }
    }

    public ItemMaskAsset maskAsset
    {
      get
      {
        return this.thirdClothes.maskAsset;
      }
    }

    public ItemGlassesAsset glassesAsset
    {
      get
      {
        return this.thirdClothes.glassesAsset;
      }
    }

    public int visualShirt
    {
      get
      {
        return this.thirdClothes.visualShirt;
      }
    }

    public int visualPants
    {
      get
      {
        return this.thirdClothes.visualPants;
      }
    }

    public int visualHat
    {
      get
      {
        return this.thirdClothes.visualHat;
      }
    }

    public int visualBackpack
    {
      get
      {
        return this.thirdClothes.visualBackpack;
      }
    }

    public int visualVest
    {
      get
      {
        return this.thirdClothes.visualVest;
      }
    }

    public int visualMask
    {
      get
      {
        return this.thirdClothes.visualMask;
      }
    }

    public int visualGlasses
    {
      get
      {
        return this.thirdClothes.visualGlasses;
      }
    }

    public ushort shirt
    {
      get
      {
        return this.thirdClothes.shirt;
      }
    }

    public ushort pants
    {
      get
      {
        return this.thirdClothes.pants;
      }
    }

    public ushort hat
    {
      get
      {
        return this.thirdClothes.hat;
      }
    }

    public ushort backpack
    {
      get
      {
        return this.thirdClothes.backpack;
      }
    }

    public ushort vest
    {
      get
      {
        return this.thirdClothes.vest;
      }
    }

    public ushort mask
    {
      get
      {
        return this.thirdClothes.mask;
      }
    }

    public ushort glasses
    {
      get
      {
        return this.thirdClothes.glasses;
      }
    }

    public byte face
    {
      get
      {
        return this.thirdClothes.face;
      }
    }

    public byte hair
    {
      get
      {
        return this.thirdClothes.hair;
      }
    }

    public byte beard
    {
      get
      {
        return this.thirdClothes.beard;
      }
    }

    public Color skin
    {
      get
      {
        return this.thirdClothes.skin;
      }
    }

    public Color color
    {
      get
      {
        return this.thirdClothes.color;
      }
    }

    [SteamCall]
    public void tellUpdateShirtQuality(CSteamID steamID, byte quality)
    {
      if (!this.channel.checkServer(steamID))
        return;
      this.shirtQuality = quality;
      if (this.onShirtUpdated == null)
        return;
      this.onShirtUpdated(this.shirt, this.shirtQuality, this.shirtState);
    }

    public void sendUpdateShirtQuality()
    {
      this.channel.send("tellUpdateShirtQuality", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[1]
      {
        (object) this.shirtQuality
      });
    }

    [SteamCall]
    public void tellUpdatePantsQuality(CSteamID steamID, byte quality)
    {
      if (!this.channel.checkServer(steamID))
        return;
      this.pantsQuality = quality;
      if (this.onPantsUpdated == null)
        return;
      this.onPantsUpdated(this.pants, this.pantsQuality, this.pantsState);
    }

    public void sendUpdatePantsQuality()
    {
      this.channel.send("tellUpdatePantsQuality", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[1]
      {
        (object) this.pantsQuality
      });
    }

    [SteamCall]
    public void tellUpdateHatQuality(CSteamID steamID, byte quality)
    {
      if (!this.channel.checkServer(steamID))
        return;
      this.hatQuality = quality;
      if (this.onHatUpdated == null)
        return;
      this.onHatUpdated(this.hat, this.hatQuality, this.hatState);
    }

    public void sendUpdateHatQuality()
    {
      this.channel.send("tellUpdateHatQuality", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[1]
      {
        (object) this.hatQuality
      });
    }

    [SteamCall]
    public void tellUpdateBackpackQuality(CSteamID steamID, byte quality)
    {
      if (!this.channel.checkServer(steamID))
        return;
      this.backpackQuality = quality;
      if (this.onBackpackUpdated == null)
        return;
      this.onBackpackUpdated(this.backpack, this.backpackQuality, this.backpackState);
    }

    public void sendUpdateBackpackQuality()
    {
      this.channel.send("tellUpdateBackpackQuality", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[1]
      {
        (object) this.backpackQuality
      });
    }

    [SteamCall]
    public void tellUpdateVestQuality(CSteamID steamID, byte quality)
    {
      if (!this.channel.checkServer(steamID))
        return;
      this.vestQuality = quality;
      if (this.onVestUpdated == null)
        return;
      this.onVestUpdated(this.vest, this.vestQuality, this.vestState);
    }

    public void sendUpdateVestQuality()
    {
      this.channel.send("tellUpdateVestQuality", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[1]
      {
        (object) this.vestQuality
      });
    }

    [SteamCall]
    public void tellUpdateMaskQuality(CSteamID steamID, byte quality)
    {
      if (!this.channel.checkServer(steamID))
        return;
      this.maskQuality = quality;
      if (this.onMaskUpdated == null)
        return;
      this.onMaskUpdated(this.mask, this.maskQuality, this.maskState);
    }

    public void sendUpdateMaskQuality()
    {
      this.channel.send("tellUpdateMaskQuality", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[1]
      {
        (object) this.maskQuality
      });
    }

    [SteamCall]
    public void tellUpdateGlassesQuality(CSteamID steamID, byte quality)
    {
      if (!this.channel.checkServer(steamID))
        return;
      this.glassesQuality = quality;
      if (this.onGlassesUpdated == null)
        return;
      this.onGlassesUpdated(this.glasses, this.glassesQuality, this.glassesState);
    }

    public void sendUpdateGlassesQuality()
    {
      this.channel.send("tellUpdateGlassesQuality", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[1]
      {
        (object) this.glassesQuality
      });
    }

    [SteamCall]
    public void tellWearShirt(CSteamID steamID, ushort id, byte quality, byte[] state)
    {
      if (!this.channel.checkServer(steamID))
        return;
      this.thirdClothes.shirt = id;
      this.shirtQuality = quality;
      this.shirtState = state;
      this.thirdClothes.apply();
      if ((Object) this.firstClothes != (Object) null)
      {
        this.firstClothes.shirt = id;
        this.firstClothes.apply();
      }
      if ((Object) this.characterClothes != (Object) null)
      {
        this.characterClothes.shirt = id;
        this.characterClothes.apply();
        Characters.active.shirt = id;
      }
      if (this.onShirtUpdated == null)
        return;
      this.onShirtUpdated(id, quality, state);
    }

    [SteamCall]
    public void askSwapShirt(CSteamID steamID, byte page, byte x, byte y)
    {
      if (!this.channel.checkOwner(steamID) || !Provider.isServer)
        return;
      if (this.player.equipment.checkSelection(PlayerInventory.SHIRT))
      {
        if (this.player.equipment.isBusy)
          return;
        this.player.equipment.dequip();
      }
      if ((int) page == (int) byte.MaxValue)
      {
        if ((int) this.shirt == 0)
          return;
        this.askWearShirt((ushort) 0, (byte) 0, new byte[0]);
      }
      else
      {
        byte index = this.player.inventory.getIndex(page, x, y);
        if ((int) index == (int) byte.MaxValue)
          return;
        ItemJar itemJar = this.player.inventory.getItem(page, index);
        ItemAsset itemAsset = (ItemAsset) Assets.find(EAssetType.ITEM, itemJar.item.id);
        if (itemAsset == null || itemAsset.type != EItemType.SHIRT)
          return;
        this.player.inventory.removeItem(page, index);
        this.askWearShirt(itemJar.item.id, itemJar.item.quality, itemJar.item.state);
      }
    }

    public void askWearShirt(ushort id, byte quality, byte[] state)
    {
      ushort shirt = this.shirt;
      byte newQuality = this.shirtQuality;
      byte[] newState = this.shirtState;
      if ((int) id != 0)
        EffectManager.sendEffect((ushort) 9, EffectManager.SMALL, this.transform.position);
      this.channel.send("tellWearShirt", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[3]
      {
        (object) id,
        (object) quality,
        (object) state
      });
      if ((int) shirt == 0)
        return;
      this.player.inventory.forceAddItem(new Item(shirt, (byte) 1, newQuality, newState), false);
    }

    public void sendSwapShirt(byte page, byte x, byte y)
    {
      if ((int) page == (int) byte.MaxValue && (int) this.shirt == 0)
        return;
      this.channel.send("askSwapShirt", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[3]
      {
        (object) page,
        (object) x,
        (object) y
      });
    }

    [SteamCall]
    public void tellWearPants(CSteamID steamID, ushort id, byte quality, byte[] state)
    {
      if (!this.channel.checkServer(steamID))
        return;
      this.thirdClothes.pants = id;
      this.pantsQuality = quality;
      this.pantsState = state;
      this.thirdClothes.apply();
      if ((Object) this.characterClothes != (Object) null)
      {
        this.characterClothes.pants = id;
        this.characterClothes.apply();
        Characters.active.pants = id;
      }
      if (this.onPantsUpdated == null)
        return;
      this.onPantsUpdated(id, quality, state);
    }

    [SteamCall]
    public void askSwapPants(CSteamID steamID, byte page, byte x, byte y)
    {
      if (!this.channel.checkOwner(steamID) || !Provider.isServer)
        return;
      if (this.player.equipment.checkSelection(PlayerInventory.PANTS))
      {
        if (this.player.equipment.isBusy)
          return;
        this.player.equipment.dequip();
      }
      if ((int) page == (int) byte.MaxValue)
      {
        if ((int) this.pants == 0)
          return;
        this.askWearPants((ushort) 0, (byte) 0, new byte[0]);
      }
      else
      {
        byte index = this.player.inventory.getIndex(page, x, y);
        if ((int) index == (int) byte.MaxValue)
          return;
        ItemJar itemJar = this.player.inventory.getItem(page, index);
        ItemAsset itemAsset = (ItemAsset) Assets.find(EAssetType.ITEM, itemJar.item.id);
        if (itemAsset == null || itemAsset.type != EItemType.PANTS)
          return;
        this.player.inventory.removeItem(page, index);
        this.askWearPants(itemJar.item.id, itemJar.item.quality, itemJar.item.state);
      }
    }

    public void askWearPants(ushort id, byte quality, byte[] state)
    {
      ushort pants = this.pants;
      byte newQuality = this.pantsQuality;
      byte[] newState = this.pantsState;
      if ((int) id != 0)
        EffectManager.sendEffect((ushort) 9, EffectManager.SMALL, this.transform.position);
      this.channel.send("tellWearPants", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[3]
      {
        (object) id,
        (object) quality,
        (object) state
      });
      if ((int) pants == 0)
        return;
      this.player.inventory.forceAddItem(new Item(pants, (byte) 1, newQuality, newState), false);
    }

    public void sendSwapPants(byte page, byte x, byte y)
    {
      if ((int) page == (int) byte.MaxValue && (int) this.pants == 0)
        return;
      this.channel.send("askSwapPants", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[3]
      {
        (object) page,
        (object) x,
        (object) y
      });
    }

    [SteamCall]
    public void tellWearHat(CSteamID steamID, ushort id, byte quality, byte[] state)
    {
      if (!this.channel.checkServer(steamID))
        return;
      this.thirdClothes.hat = id;
      this.hatQuality = quality;
      this.hatState = state;
      this.thirdClothes.apply();
      if ((Object) this.characterClothes != (Object) null)
      {
        this.characterClothes.hat = id;
        this.characterClothes.apply();
        Characters.active.hat = id;
      }
      if (this.onHatUpdated == null)
        return;
      this.onHatUpdated(id, quality, state);
    }

    [SteamCall]
    public void askSwapHat(CSteamID steamID, byte page, byte x, byte y)
    {
      if (!this.channel.checkOwner(steamID) || !Provider.isServer)
        return;
      if ((int) page == (int) byte.MaxValue)
      {
        if ((int) this.hat == 0)
          return;
        this.askWearHat((ushort) 0, (byte) 0, new byte[0]);
      }
      else
      {
        byte index = this.player.inventory.getIndex(page, x, y);
        if ((int) index == (int) byte.MaxValue)
          return;
        ItemJar itemJar = this.player.inventory.getItem(page, index);
        ItemAsset itemAsset = (ItemAsset) Assets.find(EAssetType.ITEM, itemJar.item.id);
        if (itemAsset == null || itemAsset.type != EItemType.HAT)
          return;
        this.player.inventory.removeItem(page, index);
        this.askWearHat(itemJar.item.id, itemJar.item.quality, itemJar.item.state);
      }
    }

    public void askWearHat(ushort id, byte quality, byte[] state)
    {
      ushort hat = this.hat;
      byte newQuality = this.hatQuality;
      byte[] numArray = this.hatState;
      if ((int) id != 0)
        EffectManager.sendEffect((ushort) 9, EffectManager.SMALL, this.transform.position);
      this.channel.send("tellWearHat", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[3]
      {
        (object) id,
        (object) quality,
        (object) state
      });
      if ((int) hat == 0)
        return;
      this.player.inventory.forceAddItem(new Item(hat, (byte) 1, newQuality, state), false);
    }

    public void sendSwapHat(byte page, byte x, byte y)
    {
      if ((int) page == (int) byte.MaxValue && (int) this.hat == 0)
        return;
      this.channel.send("askSwapHat", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[3]
      {
        (object) page,
        (object) x,
        (object) y
      });
    }

    [SteamCall]
    public void tellWearBackpack(CSteamID steamID, ushort id, byte quality, byte[] state)
    {
      if (!this.channel.checkServer(steamID))
        return;
      this.thirdClothes.backpack = id;
      this.backpackQuality = quality;
      this.backpackState = state;
      this.thirdClothes.apply();
      if ((Object) this.characterClothes != (Object) null)
      {
        this.characterClothes.backpack = id;
        this.characterClothes.apply();
        Characters.active.backpack = id;
      }
      if (this.onBackpackUpdated == null)
        return;
      this.onBackpackUpdated(id, quality, state);
    }

    [SteamCall]
    public void askSwapBackpack(CSteamID steamID, byte page, byte x, byte y)
    {
      if (!this.channel.checkOwner(steamID) || !Provider.isServer)
        return;
      if (this.player.equipment.checkSelection(PlayerInventory.BACKPACK))
      {
        if (this.player.equipment.isBusy)
          return;
        this.player.equipment.dequip();
      }
      if ((int) page == (int) byte.MaxValue)
      {
        if ((int) this.backpack == 0)
          return;
        this.askWearBackpack((ushort) 0, (byte) 0, new byte[0]);
      }
      else
      {
        byte index = this.player.inventory.getIndex(page, x, y);
        if ((int) index == (int) byte.MaxValue)
          return;
        ItemJar itemJar = this.player.inventory.getItem(page, index);
        ItemAsset itemAsset = (ItemAsset) Assets.find(EAssetType.ITEM, itemJar.item.id);
        if (itemAsset == null || itemAsset.type != EItemType.BACKPACK)
          return;
        this.player.inventory.removeItem(page, index);
        this.askWearBackpack(itemJar.item.id, itemJar.item.quality, itemJar.item.state);
      }
    }

    public void askWearBackpack(ushort id, byte quality, byte[] state)
    {
      ushort backpack = this.backpack;
      byte newQuality = this.backpackQuality;
      byte[] newState = this.backpackState;
      if ((int) id != 0)
        EffectManager.sendEffect((ushort) 10, EffectManager.SMALL, this.transform.position);
      this.channel.send("tellWearBackpack", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[3]
      {
        (object) id,
        (object) quality,
        (object) state
      });
      if ((int) backpack == 0)
        return;
      this.player.inventory.forceAddItem(new Item(backpack, (byte) 1, newQuality, newState), false);
    }

    public void sendSwapBackpack(byte page, byte x, byte y)
    {
      if ((int) page == (int) byte.MaxValue && (int) this.backpack == 0)
        return;
      this.channel.send("askSwapBackpack", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[3]
      {
        (object) page,
        (object) x,
        (object) y
      });
    }

    [SteamCall]
    public void tellVisual(CSteamID steamID, bool newVisual)
    {
      if (!this.channel.checkServer(steamID))
        return;
      this.thirdClothes.isVisual = newVisual;
      this.thirdClothes.apply();
      if ((Object) this.firstClothes != (Object) null)
      {
        this.firstClothes.isVisual = newVisual;
        this.firstClothes.apply();
      }
      if (!((Object) this.characterClothes != (Object) null))
        return;
      this.characterClothes.isVisual = newVisual;
      this.characterClothes.apply();
    }

    [SteamCall]
    public void askVisual(CSteamID steamID)
    {
      if (!this.channel.checkOwner(steamID) || !Provider.isServer)
        return;
      this.channel.send("tellVisual", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[1]
      {
        (object) (bool) (!this.isVisual ? 1 : 0)
      });
    }

    public void sendVisual()
    {
      this.channel.send("askVisual", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER);
    }

    [SteamCall]
    public void tellWearVest(CSteamID steamID, ushort id, byte quality, byte[] state)
    {
      if (!this.channel.checkServer(steamID))
        return;
      this.thirdClothes.vest = id;
      this.vestQuality = quality;
      this.vestState = state;
      this.thirdClothes.apply();
      if ((Object) this.characterClothes != (Object) null)
      {
        this.characterClothes.vest = id;
        this.characterClothes.apply();
        Characters.active.vest = id;
      }
      if (this.onVestUpdated == null)
        return;
      this.onVestUpdated(id, quality, state);
    }

    [SteamCall]
    public void askSwapVest(CSteamID steamID, byte page, byte x, byte y)
    {
      if (!this.channel.checkOwner(steamID) || !Provider.isServer)
        return;
      if (this.player.equipment.checkSelection(PlayerInventory.VEST))
      {
        if (this.player.equipment.isBusy)
          return;
        this.player.equipment.dequip();
      }
      if ((int) page == (int) byte.MaxValue)
      {
        if ((int) this.vest == 0)
          return;
        this.askWearVest((ushort) 0, (byte) 0, new byte[0]);
      }
      else
      {
        byte index = this.player.inventory.getIndex(page, x, y);
        if ((int) index == (int) byte.MaxValue)
          return;
        ItemJar itemJar = this.player.inventory.getItem(page, index);
        ItemAsset itemAsset = (ItemAsset) Assets.find(EAssetType.ITEM, itemJar.item.id);
        if (itemAsset == null || itemAsset.type != EItemType.VEST)
          return;
        this.player.inventory.removeItem(page, index);
        this.askWearVest(itemJar.item.id, itemJar.item.quality, itemJar.item.state);
      }
    }

    public void askWearVest(ushort id, byte quality, byte[] state)
    {
      ushort vest = this.vest;
      byte newQuality = this.vestQuality;
      byte[] newState = this.vestState;
      if ((int) id != 0)
        EffectManager.sendEffect((ushort) 10, EffectManager.SMALL, this.transform.position);
      this.channel.send("tellWearVest", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[3]
      {
        (object) id,
        (object) quality,
        (object) state
      });
      if ((int) vest == 0)
        return;
      this.player.inventory.forceAddItem(new Item(vest, (byte) 1, newQuality, newState), false);
    }

    public void sendSwapVest(byte page, byte x, byte y)
    {
      if ((int) page == (int) byte.MaxValue && (int) this.vest == 0)
        return;
      this.channel.send("askSwapVest", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[3]
      {
        (object) page,
        (object) x,
        (object) y
      });
    }

    [SteamCall]
    public void tellWearMask(CSteamID steamID, ushort id, byte quality, byte[] state)
    {
      if (!this.channel.checkServer(steamID))
        return;
      this.thirdClothes.mask = id;
      this.maskQuality = quality;
      this.maskState = state;
      this.thirdClothes.apply();
      if ((Object) this.characterClothes != (Object) null)
      {
        this.characterClothes.mask = id;
        this.characterClothes.apply();
        Characters.active.mask = id;
      }
      if (this.onMaskUpdated == null)
        return;
      this.onMaskUpdated(id, quality, state);
    }

    [SteamCall]
    public void askSwapMask(CSteamID steamID, byte page, byte x, byte y)
    {
      if (!this.channel.checkOwner(steamID) || !Provider.isServer)
        return;
      if ((int) page == (int) byte.MaxValue)
      {
        if ((int) this.mask == 0)
          return;
        this.askWearMask((ushort) 0, (byte) 0, new byte[0]);
      }
      else
      {
        byte index = this.player.inventory.getIndex(page, x, y);
        if ((int) index == (int) byte.MaxValue)
          return;
        ItemJar itemJar = this.player.inventory.getItem(page, index);
        ItemAsset itemAsset = (ItemAsset) Assets.find(EAssetType.ITEM, itemJar.item.id);
        if (itemAsset == null || itemAsset.type != EItemType.MASK)
          return;
        this.player.inventory.removeItem(page, index);
        this.askWearMask(itemJar.item.id, itemJar.item.quality, itemJar.item.state);
      }
    }

    public void askWearMask(ushort id, byte quality, byte[] state)
    {
      ushort mask = this.mask;
      byte newQuality = this.maskQuality;
      byte[] newState = this.maskState;
      if ((int) id != 0)
        EffectManager.sendEffect((ushort) 9, EffectManager.SMALL, this.transform.position);
      this.channel.send("tellWearMask", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[3]
      {
        (object) id,
        (object) quality,
        (object) state
      });
      if ((int) mask == 0)
        return;
      this.player.inventory.forceAddItem(new Item(mask, (byte) 1, newQuality, newState), false);
    }

    public void sendSwapMask(byte page, byte x, byte y)
    {
      if ((int) page == (int) byte.MaxValue && (int) this.mask == 0)
        return;
      this.channel.send("askSwapMask", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[3]
      {
        (object) page,
        (object) x,
        (object) y
      });
    }

    [SteamCall]
    public void tellWearGlasses(CSteamID steamID, ushort id, byte quality, byte[] state)
    {
      if (!this.channel.checkServer(steamID))
        return;
      this.thirdClothes.glasses = id;
      this.glassesQuality = quality;
      this.glassesState = state;
      this.thirdClothes.apply();
      if ((Object) this.characterClothes != (Object) null)
      {
        this.characterClothes.glasses = id;
        this.characterClothes.apply();
        Characters.active.glasses = id;
      }
      if (this.onGlassesUpdated == null)
        return;
      this.onGlassesUpdated(id, quality, state);
    }

    [SteamCall]
    public void askSwapGlasses(CSteamID steamID, byte page, byte x, byte y)
    {
      if (!this.channel.checkOwner(steamID) || !Provider.isServer)
        return;
      if ((int) page == (int) byte.MaxValue)
      {
        if ((int) this.glasses == 0)
          return;
        this.askWearGlasses((ushort) 0, (byte) 0, new byte[0]);
      }
      else
      {
        byte index = this.player.inventory.getIndex(page, x, y);
        if ((int) index == (int) byte.MaxValue)
          return;
        ItemJar itemJar = this.player.inventory.getItem(page, index);
        ItemAsset itemAsset = (ItemAsset) Assets.find(EAssetType.ITEM, itemJar.item.id);
        if (itemAsset == null || itemAsset.type != EItemType.GLASSES)
          return;
        this.player.inventory.removeItem(page, index);
        this.askWearGlasses(itemJar.item.id, itemJar.item.quality, itemJar.item.state);
      }
    }

    public void askWearGlasses(ushort id, byte quality, byte[] state)
    {
      ushort glasses = this.glasses;
      byte newQuality = this.glassesQuality;
      byte[] newState = this.glassesState;
      if ((int) id != 0)
        EffectManager.sendEffect((ushort) 9, EffectManager.SMALL, this.transform.position);
      this.channel.send("tellWearGlasses", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[3]
      {
        (object) id,
        (object) quality,
        (object) state
      });
      if ((int) glasses == 0)
        return;
      this.player.inventory.forceAddItem(new Item(glasses, (byte) 1, newQuality, newState), false);
    }

    public void sendSwapGlasses(byte page, byte x, byte y)
    {
      if ((int) page == (int) byte.MaxValue && (int) this.glasses == 0)
        return;
      this.channel.send("askSwapGlasses", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[3]
      {
        (object) page,
        (object) x,
        (object) y
      });
    }

    [SteamCall]
    public void tellClothing(CSteamID steamID, ushort newShirt, byte newShirtQuality, byte[] newShirtState, ushort newPants, byte newPantsQuality, byte[] newPantsState, ushort newHat, byte newHatQuality, byte[] newHatState, ushort newBackpack, byte newBackpackQuality, byte[] newBackpackState, ushort newVest, byte newVestQuality, byte[] newVestState, ushort newMask, byte newMaskQuality, byte[] newMaskState, ushort newGlasses, byte newGlassesQuality, byte[] newGlassesState, bool newVisual)
    {
      if (!this.channel.checkServer(steamID))
        return;
      this.thirdClothes.face = this.channel.owner.face;
      this.thirdClothes.hair = this.channel.owner.hair;
      this.thirdClothes.beard = this.channel.owner.beard;
      this.thirdClothes.skin = this.channel.owner.skin;
      this.thirdClothes.color = this.channel.owner.color;
      this.thirdClothes.shirt = newShirt;
      this.shirtQuality = newShirtQuality;
      this.shirtState = newShirtState;
      this.thirdClothes.pants = newPants;
      this.pantsQuality = newPantsQuality;
      this.pantsState = newPantsState;
      this.thirdClothes.hat = newHat;
      this.hatQuality = newHatQuality;
      this.hatState = newHatState;
      this.thirdClothes.backpack = newBackpack;
      this.backpackQuality = newBackpackQuality;
      this.backpackState = newBackpackState;
      this.thirdClothes.vest = newVest;
      this.vestQuality = newVestQuality;
      this.vestState = newVestState;
      this.thirdClothes.mask = newMask;
      this.maskQuality = newMaskQuality;
      this.maskState = newMaskState;
      this.thirdClothes.glasses = newGlasses;
      this.glassesQuality = newGlassesQuality;
      this.glassesState = newGlassesState;
      this.thirdClothes.isVisual = newVisual;
      this.thirdClothes.apply();
      if ((Object) this.firstClothes != (Object) null)
      {
        this.firstClothes.skin = this.channel.owner.skin;
        this.firstClothes.shirt = newShirt;
        this.firstClothes.isVisual = newVisual;
        this.firstClothes.apply();
      }
      if ((Object) this.characterClothes != (Object) null)
      {
        this.characterClothes.face = this.channel.owner.face;
        this.characterClothes.hair = this.channel.owner.hair;
        this.characterClothes.beard = this.channel.owner.beard;
        this.characterClothes.skin = this.channel.owner.skin;
        this.characterClothes.color = this.channel.owner.color;
        this.characterClothes.shirt = newShirt;
        this.characterClothes.pants = newPants;
        this.characterClothes.hat = newHat;
        this.characterClothes.backpack = newBackpack;
        this.characterClothes.vest = newVest;
        this.characterClothes.mask = newMask;
        this.characterClothes.glasses = newGlasses;
        this.characterClothes.isVisual = newVisual;
        this.characterClothes.apply();
        Characters.active.shirt = newShirt;
        Characters.active.pants = newPants;
        Characters.active.hat = newHat;
        Characters.active.backpack = newBackpack;
        Characters.active.vest = newVest;
        Characters.active.mask = newMask;
        Characters.active.glasses = newGlasses;
        Characters.hasPlayed = true;
      }
      if (this.onShirtUpdated != null)
        this.onShirtUpdated(newShirt, newShirtQuality, newShirtState);
      if (this.onPantsUpdated != null)
        this.onPantsUpdated(newPants, newPantsQuality, newPantsState);
      if (this.onHatUpdated != null)
        this.onHatUpdated(newHat, newHatQuality, newHatState);
      if (this.onBackpackUpdated != null)
        this.onBackpackUpdated(newBackpack, newBackpackQuality, newBackpackState);
      if (this.onVestUpdated != null)
        this.onVestUpdated(newVest, newVestQuality, newVestState);
      if (this.onMaskUpdated != null)
        this.onMaskUpdated(newMask, newMaskQuality, newMaskState);
      if (this.onGlassesUpdated != null)
        this.onGlassesUpdated(newGlasses, newGlassesQuality, newGlassesState);
      this.player.animator.unlock();
      if (!this.channel.isOwner)
        return;
      Player.isLoadingClothing = false;
    }

    [SteamCall]
    public void askClothing(CSteamID steamID)
    {
      if (!Provider.isServer)
        return;
      this.channel.send("tellClothing", steamID, ESteamPacket.UPDATE_RELIABLE_BUFFER, (object) this.shirt, (object) this.shirtQuality, (object) this.shirtState, (object) this.pants, (object) this.pantsQuality, (object) this.pantsState, (object) this.hat, (object) this.hatQuality, (object) this.hatState, (object) this.backpack, (object) this.backpackQuality, (object) this.backpackState, (object) this.vest, (object) this.vestQuality, (object) this.vestState, (object) this.mask, (object) this.maskQuality, (object) this.maskState, (object) this.glasses, (object) this.glassesQuality, (object) this.glassesState, (object) (bool) (this.isVisual ? 1 : 0));
    }

    private void onLifeUpdated(bool isDead)
    {
      if (!isDead || !Provider.isServer)
        return;
      if ((int) this.shirt != 0)
        ItemManager.dropItem(new Item(this.shirt, false, this.shirtQuality), this.transform.position, false, true, true);
      if ((int) this.pants != 0)
        ItemManager.dropItem(new Item(this.pants, false, this.pantsQuality), this.transform.position, false, true, true);
      if ((int) this.hat != 0)
        ItemManager.dropItem(new Item(this.hat, false, this.hatQuality), this.transform.position, false, true, true);
      if ((int) this.backpack != 0)
        ItemManager.dropItem(new Item(this.backpack, false, this.backpackQuality), this.transform.position, false, true, true);
      if ((int) this.vest != 0)
        ItemManager.dropItem(new Item(this.vest, false, this.vestQuality), this.transform.position, false, true, true);
      if ((int) this.mask != 0)
        ItemManager.dropItem(new Item(this.mask, false, this.maskQuality), this.transform.position, false, true, true);
      if ((int) this.glasses != 0)
        ItemManager.dropItem(new Item(this.glasses, false, this.glassesQuality), this.transform.position, false, true, true);
      this.thirdClothes.shirt = (ushort) 0;
      this.shirtQuality = (byte) 0;
      this.thirdClothes.pants = (ushort) 0;
      this.pantsQuality = (byte) 0;
      this.thirdClothes.hat = (ushort) 0;
      this.hatQuality = (byte) 0;
      this.thirdClothes.backpack = (ushort) 0;
      this.backpackQuality = (byte) 0;
      this.thirdClothes.vest = (ushort) 0;
      this.vestQuality = (byte) 0;
      this.thirdClothes.mask = (ushort) 0;
      this.maskQuality = (byte) 0;
      this.thirdClothes.glasses = (ushort) 0;
      this.glassesQuality = (byte) 0;
      this.thirdClothes.isVisual = true;
      this.shirtState = new byte[0];
      this.pantsState = new byte[0];
      this.hatState = new byte[0];
      this.backpackState = new byte[0];
      this.vestState = new byte[0];
      this.maskState = new byte[0];
      this.glassesState = new byte[0];
      this.channel.send("tellClothing", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, (object) this.shirt, (object) this.shirtQuality, (object) this.shirtState, (object) this.pants, (object) this.pantsQuality, (object) this.pantsState, (object) this.hat, (object) this.hatQuality, (object) this.hatState, (object) this.backpack, (object) this.backpackQuality, (object) this.backpackState, (object) this.vest, (object) this.vestQuality, (object) this.vestState, (object) this.mask, (object) this.maskQuality, (object) this.maskState, (object) this.glasses, (object) this.glassesQuality, (object) this.glassesState, (object) (bool) (this.isVisual ? 1 : 0));
    }

    public void init()
    {
      this.channel.send("askClothing", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER);
    }

    private void Start()
    {
      if (this.channel.isOwner)
      {
        if ((Object) this.player.first != (Object) null)
        {
          this.firstClothes = this.player.first.FindChild("Camera").FindChild("Viewmodel").GetComponent<HumanClothes>();
          this.firstClothes.isMine = true;
        }
        if ((Object) this.player.third != (Object) null)
          this.thirdClothes = this.player.third.GetComponent<HumanClothes>();
        if ((Object) this.player.character != (Object) null)
          this.characterClothes = this.player.character.GetComponent<HumanClothes>();
      }
      else if ((Object) this.player.third != (Object) null)
        this.thirdClothes = this.player.third.GetComponent<HumanClothes>();
      if ((Object) this.firstClothes != (Object) null)
        this.firstClothes.visualShirt = this.channel.owner.shirtItem;
      if ((Object) this.thirdClothes != (Object) null)
      {
        this.thirdClothes.visualShirt = this.channel.owner.shirtItem;
        this.thirdClothes.visualPants = this.channel.owner.pantsItem;
        this.thirdClothes.visualHat = this.channel.owner.hatItem;
        this.thirdClothes.visualBackpack = this.channel.owner.backpackItem;
        this.thirdClothes.visualVest = this.channel.owner.vestItem;
        this.thirdClothes.visualMask = this.channel.owner.maskItem;
        this.thirdClothes.visualGlasses = this.channel.owner.glassesItem;
      }
      if ((Object) this.characterClothes != (Object) null)
      {
        this.characterClothes.visualShirt = this.channel.owner.shirtItem;
        this.characterClothes.visualPants = this.channel.owner.pantsItem;
        this.characterClothes.visualHat = this.channel.owner.hatItem;
        this.characterClothes.visualBackpack = this.channel.owner.backpackItem;
        this.characterClothes.visualVest = this.channel.owner.vestItem;
        this.characterClothes.visualMask = this.channel.owner.maskItem;
        this.characterClothes.visualGlasses = this.channel.owner.glassesItem;
      }
      if (Provider.isServer)
      {
        this.load();
        this.player.life.onLifeUpdated += new LifeUpdated(this.onLifeUpdated);
      }
      if (!Provider.isClient)
        return;
      this.Invoke("init", 0.1f);
    }

    public void load()
    {
      this.thirdClothes.visualShirt = this.channel.owner.shirtItem;
      this.thirdClothes.visualPants = this.channel.owner.pantsItem;
      this.thirdClothes.visualHat = this.channel.owner.hatItem;
      this.thirdClothes.visualBackpack = this.channel.owner.backpackItem;
      this.thirdClothes.visualVest = this.channel.owner.vestItem;
      this.thirdClothes.visualMask = this.channel.owner.maskItem;
      this.thirdClothes.visualGlasses = this.channel.owner.glassesItem;
      if (PlayerSavedata.fileExists(this.channel.owner.playerID, "/Player/Clothing.dat") && Level.info.type == ELevelType.SURVIVAL)
      {
        Block block = PlayerSavedata.readBlock(this.channel.owner.playerID, "/Player/Clothing.dat", (byte) 0);
        byte num = block.readByte();
        if ((int) num > 1)
        {
          this.thirdClothes.shirt = block.readUInt16();
          this.shirtQuality = block.readByte();
          this.thirdClothes.pants = block.readUInt16();
          this.pantsQuality = block.readByte();
          this.thirdClothes.hat = block.readUInt16();
          this.hatQuality = block.readByte();
          this.thirdClothes.backpack = block.readUInt16();
          this.backpackQuality = block.readByte();
          this.thirdClothes.vest = block.readUInt16();
          this.vestQuality = block.readByte();
          this.thirdClothes.mask = block.readUInt16();
          this.maskQuality = block.readByte();
          this.thirdClothes.glasses = block.readUInt16();
          this.glassesQuality = block.readByte();
          if ((int) num > 2)
            this.thirdClothes.isVisual = block.readBoolean();
          if ((int) num > 4)
          {
            this.shirtState = block.readByteArray();
            this.pantsState = block.readByteArray();
            this.hatState = block.readByteArray();
            this.backpackState = block.readByteArray();
            this.vestState = block.readByteArray();
            this.maskState = block.readByteArray();
            this.glassesState = block.readByteArray();
          }
          else
          {
            this.shirtState = new byte[0];
            this.pantsState = new byte[0];
            this.hatState = new byte[0];
            this.backpackState = new byte[0];
            this.vestState = new byte[0];
            this.maskState = new byte[0];
            this.glassesState = new byte[0];
            if ((int) this.glasses == 334)
              this.glassesState = new byte[1];
          }
          this.thirdClothes.apply();
          return;
        }
      }
      this.thirdClothes.shirt = (ushort) 0;
      this.shirtQuality = (byte) 0;
      this.thirdClothes.pants = (ushort) 0;
      this.pantsQuality = (byte) 0;
      this.thirdClothes.hat = (ushort) 0;
      this.hatQuality = (byte) 0;
      this.thirdClothes.backpack = (ushort) 0;
      this.backpackQuality = (byte) 0;
      this.thirdClothes.vest = (ushort) 0;
      this.vestQuality = (byte) 0;
      this.thirdClothes.mask = (ushort) 0;
      this.maskQuality = (byte) 0;
      this.thirdClothes.glasses = (ushort) 0;
      this.glassesQuality = (byte) 0;
      this.shirtState = new byte[0];
      this.pantsState = new byte[0];
      this.hatState = new byte[0];
      this.backpackState = new byte[0];
      this.vestState = new byte[0];
      this.maskState = new byte[0];
      this.maskState = new byte[0];
      this.thirdClothes.apply();
    }

    public void save()
    {
      if (this.player.life.isDead || (Object) this.thirdClothes == (Object) null)
      {
        if (!PlayerSavedata.fileExists(this.channel.owner.playerID, "/Player/Clothing.dat"))
          return;
        PlayerSavedata.deleteFile(this.channel.owner.playerID, "/Player/Clothing.dat");
      }
      else
      {
        Block block = new Block();
        block.writeByte(PlayerClothing.SAVEDATA_VERSION);
        block.writeUInt16(this.thirdClothes.shirt);
        block.writeByte(this.shirtQuality);
        block.writeUInt16(this.thirdClothes.pants);
        block.writeByte(this.pantsQuality);
        block.writeUInt16(this.thirdClothes.hat);
        block.writeByte(this.hatQuality);
        block.writeUInt16(this.thirdClothes.backpack);
        block.writeByte(this.backpackQuality);
        block.writeUInt16(this.thirdClothes.vest);
        block.writeByte(this.vestQuality);
        block.writeUInt16(this.thirdClothes.mask);
        block.writeByte(this.maskQuality);
        block.writeUInt16(this.thirdClothes.glasses);
        block.writeByte(this.glassesQuality);
        block.writeBoolean(this.isVisual);
        block.writeByteArray(this.shirtState);
        block.writeByteArray(this.pantsState);
        block.writeByteArray(this.hatState);
        block.writeByteArray(this.backpackState);
        block.writeByteArray(this.vestState);
        block.writeByteArray(this.maskState);
        block.writeByteArray(this.glassesState);
        PlayerSavedata.writeBlock(this.channel.owner.playerID, "/Player/Clothing.dat", block);
      }
    }
  }
}
