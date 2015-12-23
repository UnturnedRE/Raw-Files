// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.PlayerEquipment
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
  public class PlayerEquipment : PlayerCaller
  {
    private static readonly float DAMAGE_BARRICADE = 2f;
    private static readonly float DAMAGE_STRUCTURE = 2f;
    private static readonly float DAMAGE_VEHICLE = 10f;
    private static readonly float DAMAGE_RESOURCE = 20f;
    private static readonly PlayerDamageMultiplier DAMAGE_PLAYER_MULTIPLIER = new PlayerDamageMultiplier(15f, 0.6f, 0.6f, 0.8f, 1.1f);
    private static readonly ZombieDamageMultiplier DAMAGE_ZOMBIE_MULTIPLIER = new ZombieDamageMultiplier(15f, 0.3f, 0.3f, 0.6f, 1.1f);
    private static readonly AnimalDamageMultiplier DAMAGE_ANIMAL_MULTIPLIER = new AnimalDamageMultiplier(15f, 0.3f, 0.6f, 1.1f);
    private byte slot = byte.MaxValue;
    private ushort _itemID;
    private byte[] _state;
    public byte quality;
    private Transform[] thirdSlots;
    private Transform[] characterSlots;
    private Transform _firstModel;
    private Transform _thirdModel;
    private Transform _characterModel;
    private ItemAsset _asset;
    private Useable _useable;
    private Transform _thirdPrimaryMeleeSlot;
    private Transform _thirdPrimaryLargeGunSlot;
    private Transform _thirdPrimarySmallGunSlot;
    private Transform _thirdSecondaryMeleeSlot;
    private Transform _thirdSecondaryGunSlot;
    private Transform _characterPrimaryMeleeSlot;
    private Transform _characterPrimaryLargeGunSlot;
    private Transform _characterPrimarySmallGunSlot;
    private Transform _characterSecondaryMeleeSlot;
    private Transform _characterSecondaryGunSlot;
    private Transform _firstLeftHook;
    private Transform _firstRightHook;
    private Transform _thirdLeftHook;
    private Transform _thirdRightHook;
    private Transform _characterLeftHook;
    private Transform _characterRightHook;
    public bool isBusy;
    private bool warp;
    private byte _equippedPage;
    private byte _equipped_x;
    private byte _equipped_y;
    private bool prim;
    private bool lastPrimary;
    private bool _primary;
    private bool sec;
    private bool flipSecondary;
    private bool lastSecondary;
    private bool _secondary;
    private bool hasVision;
    private float lastEquip;
    private float lastEquipped;
    private float equippedTime;
    private uint lastPunch;
    private static float lastInspect;
    private static float inspectTime;

    public ushort itemID
    {
      get
      {
        return this._itemID;
      }
    }

    public byte[] state
    {
      get
      {
        return this._state;
      }
    }

    public Transform firstModel
    {
      get
      {
        return this._firstModel;
      }
    }

    public Transform thirdModel
    {
      get
      {
        return this._thirdModel;
      }
    }

    public Transform characterModel
    {
      get
      {
        return this._characterModel;
      }
    }

    public ItemAsset asset
    {
      get
      {
        return this._asset;
      }
    }

    public Useable useable
    {
      get
      {
        return this._useable;
      }
    }

    public Transform thirdPrimaryMeleeSlot
    {
      get
      {
        return this._thirdPrimaryMeleeSlot;
      }
    }

    public Transform thirdPrimaryLargeGunSlot
    {
      get
      {
        return this._thirdPrimaryLargeGunSlot;
      }
    }

    public Transform thirdPrimarySmallGunSlot
    {
      get
      {
        return this._thirdPrimarySmallGunSlot;
      }
    }

    public Transform thirdSecondaryMeleeSlot
    {
      get
      {
        return this._thirdSecondaryMeleeSlot;
      }
    }

    public Transform thirdSecondaryGunSlot
    {
      get
      {
        return this._thirdSecondaryGunSlot;
      }
    }

    public Transform characterPrimaryMeleeSlot
    {
      get
      {
        return this._characterPrimaryMeleeSlot;
      }
    }

    public Transform characterPrimaryLargeGunSlot
    {
      get
      {
        return this._characterPrimaryLargeGunSlot;
      }
    }

    public Transform characterPrimarySmallGunSlot
    {
      get
      {
        return this._characterPrimarySmallGunSlot;
      }
    }

    public Transform characterSecondaryMeleeSlot
    {
      get
      {
        return this._characterSecondaryMeleeSlot;
      }
    }

    public Transform characterSecondaryGunSlot
    {
      get
      {
        return this._characterSecondaryGunSlot;
      }
    }

    public Transform firstLeftHook
    {
      get
      {
        return this._firstLeftHook;
      }
    }

    public Transform firstRightHook
    {
      get
      {
        return this._firstRightHook;
      }
    }

    public Transform thirdLeftHook
    {
      get
      {
        return this._thirdLeftHook;
      }
    }

    public Transform thirdRightHook
    {
      get
      {
        return this._thirdRightHook;
      }
    }

    public Transform characterLeftHook
    {
      get
      {
        return this._characterLeftHook;
      }
    }

    public Transform characterRightHook
    {
      get
      {
        return this._characterRightHook;
      }
    }

    public bool isSelected
    {
      get
      {
        if ((Object) this.thirdModel != (Object) null)
          return (Object) this.useable != (Object) null;
        return false;
      }
    }

    public bool isEquipped
    {
      get
      {
        return (double) Time.realtimeSinceStartup - (double) this.lastEquipped > (double) this.equippedTime;
      }
    }

    public byte equippedPage
    {
      get
      {
        return this._equippedPage;
      }
    }

    public byte equipped_x
    {
      get
      {
        return this._equipped_x;
      }
    }

    public byte equipped_y
    {
      get
      {
        return this._equipped_y;
      }
    }

    public bool primary
    {
      get
      {
        return this._primary;
      }
    }

    public bool secondary
    {
      get
      {
        return this._secondary;
      }
    }

    public bool isInspecting
    {
      get
      {
        return (double) Time.realtimeSinceStartup - (double) PlayerEquipment.lastInspect < (double) PlayerEquipment.inspectTime;
      }
    }

    public bool canInspect
    {
      get
      {
        if (this.isSelected && this.isEquipped && (!this.isBusy && this.player.animator.checkExists("Inspect")) && !this.isInspecting)
          return this.useable.canInspect;
        return false;
      }
    }

    public void inspect()
    {
      this.player.animator.setAnimationSpeed("Inspect", 1f);
      PlayerEquipment.lastInspect = Time.realtimeSinceStartup;
      PlayerEquipment.inspectTime = this.player.animator.getAnimationLength("Inspect");
      this.player.animator.play("Inspect", false);
    }

    public void uninspect()
    {
      this.player.animator.setAnimationSpeed("Inspect", float.MaxValue);
    }

    public bool checkSelection(byte page)
    {
      return (int) page == (int) this.equippedPage;
    }

    public bool checkSelection(byte page, byte x, byte y)
    {
      if ((int) page == (int) this.equippedPage && (int) x == (int) this.equipped_x)
        return (int) y == (int) this.equipped_y;
      return false;
    }

    private void updateSlot(byte slot, ushort id, byte[] state)
    {
      if (Dedicator.isDedicated || this.thirdSlots == null)
        return;
      if ((Object) this.thirdSlots[(int) slot] != (Object) null)
        Object.Destroy((Object) this.thirdSlots[(int) slot].gameObject);
      if (this.characterSlots != null && (Object) this.characterSlots[(int) slot] != (Object) null)
        Object.Destroy((Object) this.characterSlots[(int) slot].gameObject);
      if (this.channel.isOwner)
      {
        if ((int) slot == 0)
        {
          Characters.active.primaryItem = id;
          Characters.active.primaryState = state;
        }
        else if ((int) slot == 1)
        {
          Characters.active.secondaryItem = id;
          Characters.active.secondaryState = state;
        }
      }
      if ((int) id == 0)
        return;
      ItemAsset itemAsset = (ItemAsset) Assets.find(EAssetType.ITEM, id);
      if (itemAsset == null)
        return;
      int num = 0;
      ushort skin = (ushort) 0;
      ushort mythicID = (ushort) 0;
      if (this.channel.owner.skinItems != null && this.channel.owner.skins != null && this.channel.owner.skins.TryGetValue(id, out num))
      {
        skin = Provider.provider.economyService.getInventorySkinID(num);
        mythicID = Provider.provider.economyService.getInventoryMythicID(num);
      }
      Transform model1 = ItemTool.getItem(id, skin, (byte) 100, state, false);
      if ((int) slot == 0)
      {
        if (itemAsset.type == EItemType.MELEE)
          model1.transform.parent = this.thirdPrimaryMeleeSlot;
        else if (itemAsset.slot == ESlotType.PRIMARY)
          model1.transform.parent = this.thirdPrimaryLargeGunSlot;
        else
          model1.transform.parent = this.thirdPrimarySmallGunSlot;
      }
      else if ((int) slot == 1)
      {
        if (itemAsset.type == EItemType.MELEE)
          model1.transform.parent = this.thirdSecondaryMeleeSlot;
        else
          model1.transform.parent = this.thirdSecondaryGunSlot;
      }
      model1.localPosition = Vector3.zero;
      model1.localRotation = Quaternion.Euler(0.0f, 0.0f, 90f);
      model1.localScale = Vector3.one;
      model1.gameObject.SetActive(false);
      model1.gameObject.SetActive(true);
      Object.Destroy((Object) model1.GetComponent<Collider>());
      if ((int) mythicID != 0)
        ItemTool.applyEffect(model1, mythicID, EEffectType.THIRD);
      this.thirdSlots[(int) slot] = model1;
      if (this.characterSlots == null)
        return;
      Transform model2 = ItemTool.getItem(id, skin, (byte) 100, state, false);
      if ((int) slot == 0)
      {
        if (itemAsset.type == EItemType.MELEE)
          model2.transform.parent = this.characterPrimaryMeleeSlot;
        else if (itemAsset.slot == ESlotType.PRIMARY)
          model2.transform.parent = this.characterPrimaryLargeGunSlot;
        else
          model2.transform.parent = this.characterPrimarySmallGunSlot;
      }
      else if ((int) slot == 1)
      {
        if (itemAsset.type == EItemType.MELEE)
          model2.transform.parent = this.characterSecondaryMeleeSlot;
        else
          model2.transform.parent = this.characterSecondaryGunSlot;
      }
      model2.localPosition = Vector3.zero;
      model2.localRotation = Quaternion.Euler(0.0f, 0.0f, 90f);
      model2.localScale = Vector3.one;
      model2.gameObject.SetActive(false);
      model2.gameObject.SetActive(true);
      Object.Destroy((Object) model2.GetComponent<Collider>());
      if ((int) mythicID != 0)
        ItemTool.applyEffect(model2, mythicID, EEffectType.THIRD);
      this.characterSlots[(int) slot] = model2;
    }

    [SteamCall]
    public void askToggleVision(CSteamID steamID)
    {
      if (!this.channel.checkOwner(steamID) || !this.hasVision)
        return;
      this.channel.send("tellToggleVision", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER);
    }

    [SteamCall]
    public void tellToggleVision(CSteamID steamID)
    {
      if (!this.channel.checkServer(steamID))
        return;
      this.player.clothing.glassesState[0] = (int) this.player.clothing.glassesState[0] != 0 ? (byte) 0 : (byte) 1;
      this.updateVision();
    }

    [SteamCall]
    public void tellSlot(CSteamID steamID, byte slot, ushort id, byte[] state)
    {
      if (!this.channel.checkServer(steamID))
        return;
      this.updateSlot(slot, id, state);
    }

    [SteamCall]
    public void tellUpdateState(CSteamID steamID, byte page, byte index, byte[] newState)
    {
      if (!this.channel.checkServer(steamID))
        return;
      this._state = newState;
      if ((int) this.slot != (int) byte.MaxValue && (int) this.slot < this.thirdSlots.Length && (Object) this.thirdSlots[(int) this.slot] != (Object) null)
      {
        this.updateSlot(this.slot, this.itemID, newState);
        this.thirdSlots[(int) this.slot].gameObject.SetActive(false);
        if (this.characterSlots != null)
          this.characterSlots[(int) this.slot].gameObject.SetActive(false);
      }
      if (this.channel.isOwner || Provider.isServer)
        this.player.inventory.updateState(page, index, this.state);
      if ((Object) this.useable != (Object) null)
        this.useable.updateState(this.state);
      if (!((Object) this.characterModel != (Object) null))
        return;
      Object.Destroy((Object) this.characterModel.gameObject);
      ItemAsset itemAsset = (ItemAsset) Assets.find(EAssetType.ITEM, this.itemID);
      if (itemAsset == null)
        return;
      int num = 0;
      ushort skin = (ushort) 0;
      ushort mythicID = (ushort) 0;
      if (this.channel.owner.skinItems != null && this.channel.owner.skins != null && this.channel.owner.skins.TryGetValue(this.itemID, out num))
      {
        skin = Provider.provider.economyService.getInventorySkinID(num);
        mythicID = Provider.provider.economyService.getInventoryMythicID(num);
      }
      this._characterModel = ItemTool.getItem(this.itemID, skin, (byte) 100, this.state, false, itemAsset);
      if (itemAsset.isBackward)
        this.characterModel.transform.parent = this._characterLeftHook;
      else
        this.characterModel.transform.parent = this._characterRightHook;
      this.characterModel.localPosition = Vector3.zero;
      this.characterModel.localRotation = Quaternion.Euler(0.0f, 0.0f, 90f);
      this.characterModel.localScale = Vector3.one;
      this.characterModel.gameObject.AddComponent<Rigidbody>();
      this.characterModel.GetComponent<Rigidbody>().useGravity = false;
      this.characterModel.GetComponent<Rigidbody>().isKinematic = true;
      if ((int) mythicID == 0)
        return;
      ItemTool.applyEffect(this.characterModel, mythicID, EEffectType.THIRD);
    }

    [SteamCall]
    public void tellEquip(CSteamID steamID, byte page, byte x, byte y, ushort id, byte newQuality, byte[] newState)
    {
      if (!this.channel.checkServer(steamID))
        return;
      if ((int) this.slot != (int) byte.MaxValue && (int) this.slot < this.thirdSlots.Length && (Object) this.thirdSlots[(int) this.slot] != (Object) null)
      {
        this.thirdSlots[(int) this.slot].gameObject.SetActive(true);
        if (this.characterSlots != null)
          this.characterSlots[(int) this.slot].gameObject.SetActive(true);
      }
      this.slot = page;
      if ((int) this.slot != (int) byte.MaxValue && (int) this.slot < this.thirdSlots.Length && (Object) this.thirdSlots[(int) this.slot] != (Object) null)
      {
        this.thirdSlots[(int) this.slot].gameObject.SetActive(false);
        if (this.characterSlots != null)
          this.characterSlots[(int) this.slot].gameObject.SetActive(false);
      }
      if (this.isSelected)
      {
        this.useable.dequip();
        if (this.player.life.isDead)
          Object.Destroy((Object) this.useable);
        else
          Object.DestroyImmediate((Object) this.useable);
        this._useable = (Useable) null;
        this._itemID = (ushort) 0;
        if ((Object) this.firstModel != (Object) null)
          Object.Destroy((Object) this.firstModel.gameObject);
        if ((Object) this.thirdModel != (Object) null)
          Object.Destroy((Object) this.thirdModel.gameObject);
        if ((Object) this.characterModel != (Object) null)
          Object.Destroy((Object) this.characterModel.gameObject);
        for (int index = 0; index < this.asset.animations.Length; ++index)
          this.player.animator.removeAnimation(this.asset.animations[index]);
        this.channel.build();
      }
      this.isBusy = false;
      if ((int) id == 0)
      {
        this._equippedPage = byte.MaxValue;
        this._equipped_x = byte.MaxValue;
        this._equipped_y = byte.MaxValue;
        this._itemID = (ushort) 0;
        this._asset = (ItemAsset) null;
      }
      else
      {
        this._equippedPage = page;
        this._equipped_x = x;
        this._equipped_y = y;
        this._asset = (ItemAsset) Assets.find(EAssetType.ITEM, id);
        if (this.asset == null)
          return;
        this._itemID = id;
        this.quality = newQuality;
        this._state = newState;
        int num = 0;
        ushort skin = (ushort) 0;
        ushort mythicID = (ushort) 0;
        if (this.channel.owner.skinItems != null && this.channel.owner.skins != null && this.channel.owner.skins.TryGetValue(id, out num))
        {
          skin = Provider.provider.economyService.getInventorySkinID(num);
          mythicID = Provider.provider.economyService.getInventoryMythicID(num);
        }
        if (this.channel.isOwner)
        {
          this._firstModel = ItemTool.getItem(id, skin, this.quality, this.state, true, this.asset);
          if (this.asset.isBackward)
            this.firstModel.transform.parent = this.firstLeftHook;
          else
            this.firstModel.transform.parent = this.firstRightHook;
          this.firstModel.localPosition = Vector3.zero;
          this.firstModel.localRotation = Quaternion.Euler(0.0f, 0.0f, 90f);
          this.firstModel.localScale = Vector3.one;
          this.firstModel.gameObject.SetActive(false);
          this.firstModel.gameObject.SetActive(true);
          this.firstModel.gameObject.AddComponent<Rigidbody>();
          this.firstModel.GetComponent<Rigidbody>().useGravity = false;
          this.firstModel.GetComponent<Rigidbody>().isKinematic = true;
          Object.Destroy((Object) this.firstModel.GetComponent<Collider>());
          Layerer.viewmodel(this.firstModel);
          if ((int) mythicID != 0)
            ItemTool.applyEffect(this.firstModel, mythicID, EEffectType.FIRST);
          this._characterModel = ItemTool.getItem(id, skin, this.quality, this.state, false, this.asset);
          if (this.asset.isBackward)
            this.characterModel.transform.parent = this.characterLeftHook;
          else
            this.characterModel.transform.parent = this.characterRightHook;
          this.characterModel.localPosition = Vector3.zero;
          this.characterModel.localRotation = Quaternion.Euler(0.0f, 0.0f, 90f);
          this.characterModel.localScale = Vector3.one;
          this.characterModel.gameObject.AddComponent<Rigidbody>();
          this.characterModel.GetComponent<Rigidbody>().useGravity = false;
          this.characterModel.GetComponent<Rigidbody>().isKinematic = true;
          if ((int) mythicID != 0)
            ItemTool.applyEffect(this.characterModel, mythicID, EEffectType.THIRD);
        }
        this._thirdModel = ItemTool.getItem(id, skin, this.quality, this.state, false, this.asset);
        if (this.asset.isBackward)
          this.thirdModel.transform.parent = this.thirdLeftHook;
        else
          this.thirdModel.transform.parent = this.thirdRightHook;
        this.thirdModel.localPosition = Vector3.zero;
        this.thirdModel.localRotation = Quaternion.Euler(0.0f, 0.0f, 90f);
        this.thirdModel.localScale = Vector3.one;
        this.thirdModel.gameObject.SetActive(false);
        this.thirdModel.gameObject.SetActive(true);
        this.thirdModel.gameObject.AddComponent<Rigidbody>();
        this.thirdModel.GetComponent<Rigidbody>().useGravity = false;
        this.thirdModel.GetComponent<Rigidbody>().isKinematic = true;
        Object.Destroy((Object) this.thirdModel.GetComponent<Collider>());
        if ((int) mythicID != 0)
          ItemTool.applyEffect(this.thirdModel, mythicID, EEffectType.THIRD);
        for (int index = 0; index < this.asset.animations.Length; ++index)
          this.player.animator.addAnimation(this.asset.animations[index]);
        switch (this.asset.useable)
        {
          case EUseableType.CLOTHING:
            this._useable = (Useable) this.gameObject.AddComponent<UseableClothing>();
            break;
          case EUseableType.GUN:
            this._useable = (Useable) this.gameObject.AddComponent<UseableGun>();
            break;
          case EUseableType.CONSUMEABLE:
            this._useable = (Useable) this.gameObject.AddComponent<UseableConsumeable>();
            break;
          case EUseableType.MELEE:
            this._useable = (Useable) this.gameObject.AddComponent<UseableMelee>();
            break;
          case EUseableType.FUEL:
            this._useable = (Useable) this.gameObject.AddComponent<UseableFuel>();
            break;
          case EUseableType.CARJACK:
            this._useable = (Useable) this.gameObject.AddComponent<UseableCarjack>();
            break;
          case EUseableType.BARRICADE:
            this._useable = (Useable) this.gameObject.AddComponent<UseableBarricade>();
            break;
          case EUseableType.STRUCTURE:
            this._useable = (Useable) this.gameObject.AddComponent<UseableStructure>();
            break;
          case EUseableType.THROWABLE:
            this._useable = (Useable) this.gameObject.AddComponent<UseableThrowable>();
            break;
          case EUseableType.GROWER:
            this._useable = (Useable) this.gameObject.AddComponent<UseableGrower>();
            break;
          case EUseableType.OPTIC:
            this._useable = (Useable) this.gameObject.AddComponent<UseableOptic>();
            break;
          case EUseableType.REFILL:
            this._useable = (Useable) this.gameObject.AddComponent<UseableRefill>();
            break;
          case EUseableType.FISHER:
            this._useable = (Useable) this.gameObject.AddComponent<UseableFisher>();
            break;
          case EUseableType.CLOUD:
            this._useable = (Useable) this.gameObject.AddComponent<UseableCloud>();
            break;
        }
        this.channel.build();
        this.useable.equip();
        this.lastEquipped = Time.realtimeSinceStartup;
        this.equippedTime = this.player.animator.getAnimationLength("Equip");
        if (Dedicator.isDedicated || !((Object) this.asset.equip != (Object) null))
          return;
        this.player.playSound(this.asset.equip, 1f, 0.05f);
      }
    }

    public void tryEquip(byte page, byte x, byte y)
    {
      if (this.isBusy || this.player.life.isDead || (this.player.stance.stance == EPlayerStance.CLIMB || this.player.stance.stance == EPlayerStance.SWIM) || (this.player.stance.stance == EPlayerStance.DRIVING || this.isSelected && !this.isEquipped))
        return;
      if ((int) page == (int) this.equippedPage && (int) x == (int) this.equipped_x && (int) y == (int) this.equipped_y || (int) page == (int) byte.MaxValue)
      {
        this.dequip();
      }
      else
      {
        if ((int) page < 0 || (int) page >= (int) PlayerInventory.PAGES - 1)
          return;
        byte index = this.player.inventory.getIndex(page, x, y);
        if ((int) index == (int) byte.MaxValue)
          return;
        ItemJar itemJar = this.player.inventory.getItem(page, index);
        if (itemJar == null || !ItemTool.checkUseable(page, itemJar.item.id))
          return;
        if (itemJar.item.state != null)
          this.channel.send("tellEquip", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, (object) page, (object) x, (object) y, (object) itemJar.item.id, (object) itemJar.item.quality, (object) itemJar.item.state);
        else
          this.channel.send("tellEquip", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, (object) page, (object) x, (object) y, (object) itemJar.item.id, (object) itemJar.item.quality, (object) new byte[0]);
      }
    }

    [SteamCall]
    public void askEquip(CSteamID steamID, byte page, byte x, byte y)
    {
      if (!this.channel.checkOwner(steamID) || !Provider.isServer)
        return;
      this.tryEquip(page, x, y);
    }

    [SteamCall]
    public void askEquipment(CSteamID steamID)
    {
      if (!Provider.isServer)
        return;
      for (byte page = (byte) 0; (int) page < (int) PlayerInventory.SLOTS; ++page)
      {
        ItemJar itemJar = this.player.inventory.getItem(page, (byte) 0);
        if (itemJar != null)
        {
          if (itemJar.item.state != null)
            this.channel.send("tellSlot", steamID, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[3]
            {
              (object) page,
              (object) itemJar.item.id,
              (object) itemJar.item.state
            });
          else
            this.channel.send("tellSlot", steamID, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[3]
            {
              (object) page,
              (object) itemJar.item.id,
              (object) new byte[0]
            });
        }
        else
          this.channel.send("tellSlot", steamID, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[3]
          {
            (object) page,
            (object) 0,
            (object) new byte[0]
          });
      }
      if (!this.isSelected)
        return;
      if (this.state != null)
        this.channel.send("tellEquip", steamID, ESteamPacket.UPDATE_RELIABLE_BUFFER, (object) this.equippedPage, (object) this.equipped_x, (object) this.equipped_y, (object) this.itemID, (object) this.quality, (object) this.state);
      else
        this.channel.send("tellEquip", steamID, ESteamPacket.UPDATE_RELIABLE_BUFFER, (object) this.equippedPage, (object) this.equipped_x, (object) this.equipped_y, (object) this.itemID, (object) this.quality, (object) new byte[0]);
    }

    public void updateState()
    {
      this.player.inventory.updateState(this.equippedPage, this.player.inventory.getIndex(this.equippedPage, this.equipped_x, this.equipped_y), this.state);
    }

    public void updateQuality()
    {
      this.player.inventory.updateQuality(this.equippedPage, this.player.inventory.getIndex(this.equippedPage, this.equipped_x, this.equipped_y), this.quality);
    }

    public void sendUpdateState()
    {
      this.channel.send("tellUpdateState", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[3]
      {
        (object) this.equippedPage,
        (object) this.player.inventory.getIndex(this.equippedPage, this.equipped_x, this.equipped_y),
        (object) this.state
      });
    }

    public void sendUpdateQuality()
    {
      this.player.inventory.sendUpdateQuality(this.equippedPage, this.equipped_x, this.equipped_y, this.quality);
    }

    public void sendSlot(byte slot)
    {
      ItemJar itemJar = this.player.inventory.getItem(slot, (byte) 0);
      if (itemJar != null)
      {
        if (itemJar.item.state != null)
          this.channel.send("tellSlot", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[3]
          {
            (object) slot,
            (object) itemJar.item.id,
            (object) itemJar.item.state
          });
        else
          this.channel.send("tellSlot", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[3]
          {
            (object) slot,
            (object) itemJar.item.id,
            (object) new byte[0]
          });
      }
      else
        this.channel.send("tellSlot", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[3]
        {
          (object) slot,
          (object) 0,
          (object) new byte[0]
        });
    }

    public void equip(byte page, byte x, byte y)
    {
      if ((int) page < 0 || (int) page >= (int) PlayerInventory.PAGES - 1 || (this.isBusy || this.player.life.isDead) || (this.player.stance.stance == EPlayerStance.CLIMB || this.player.stance.stance == EPlayerStance.SWIM || this.player.stance.stance == EPlayerStance.DRIVING) || this.isSelected && !this.isEquipped)
        return;
      this.lastEquip = Time.realtimeSinceStartup;
      this.channel.send("askEquip", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[3]
      {
        (object) page,
        (object) x,
        (object) y
      });
    }

    public void dequip()
    {
      if (Provider.isServer)
      {
        this.channel.send("tellEquip", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, (object) byte.MaxValue, (object) byte.MaxValue, (object) byte.MaxValue, (object) 0, (object) 0, (object) new byte[0]);
      }
      else
      {
        if (this.isBusy)
          return;
        this.channel.send("askEquip", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[3]
        {
          (object) byte.MaxValue,
          (object) byte.MaxValue,
          (object) byte.MaxValue
        });
      }
    }

    public void use()
    {
      if (!this.isSelected)
        return;
      ushort itemId = this.itemID;
      this.player.inventory.removeItem(this.equippedPage, this.player.inventory.getIndex(this.equippedPage, this.equipped_x, this.equipped_y));
      this.dequip();
      InventorySearch inventorySearch = this.player.inventory.has(itemId);
      if (inventorySearch == null)
        return;
      this.tryEquip(inventorySearch.page, inventorySearch.jar.x, inventorySearch.jar.y);
    }

    private void punch(EPlayerPunch mode)
    {
      if (this.channel.isOwner)
      {
        this.player.playSound((AudioClip) Resources.Load("Sounds/General/Punch"));
        RaycastInfo info = DamageTool.raycast(new Ray(this.player.look.aim.position, this.player.look.aim.forward), 2f, RayMasks.DAMAGE_CLIENT);
        if ((Object) info.player != (Object) null && (double) PlayerEquipment.DAMAGE_PLAYER_MULTIPLIER.damage > 1.0 && (this.channel.owner.playerID.group == CSteamID.Nil || info.player.channel.owner.playerID.group != this.channel.owner.playerID.group) && Provider.isPvP)
          PlayerUI.hitmark(info.limb != ELimb.SKULL ? EPlayerHit.ENTITIY : EPlayerHit.CRITICAL);
        else if ((Object) info.zombie != (Object) null && (double) PlayerEquipment.DAMAGE_ZOMBIE_MULTIPLIER.damage > 1.0 || (Object) info.animal != (Object) null && (double) PlayerEquipment.DAMAGE_ANIMAL_MULTIPLIER.damage > 1.0)
          PlayerUI.hitmark(info.limb != ELimb.SKULL ? EPlayerHit.ENTITIY : EPlayerHit.CRITICAL);
        else if ((Object) info.transform != (Object) null && info.transform.tag == "Barricade" && (double) PlayerEquipment.DAMAGE_BARRICADE > 1.0)
        {
          ushort result;
          if (ushort.TryParse(!(info.transform.name == "Hinge") ? info.transform.name : info.transform.parent.parent.name, out result))
          {
            ItemBarricadeAsset itemBarricadeAsset = (ItemBarricadeAsset) Assets.find(EAssetType.ITEM, result);
            if (itemBarricadeAsset != null && itemBarricadeAsset.isVulnerable)
              PlayerUI.hitmark(EPlayerHit.BUILD);
          }
        }
        else if ((Object) info.transform != (Object) null && info.transform.tag == "Structure" && (double) PlayerEquipment.DAMAGE_STRUCTURE > 1.0)
        {
          ushort result;
          if (ushort.TryParse(info.transform.name, out result))
          {
            ItemStructureAsset itemStructureAsset = (ItemStructureAsset) Assets.find(EAssetType.ITEM, result);
            if (itemStructureAsset != null && itemStructureAsset.isVulnerable)
              PlayerUI.hitmark(EPlayerHit.BUILD);
          }
        }
        else if ((Object) info.vehicle != (Object) null && !info.vehicle.isDead && (double) PlayerEquipment.DAMAGE_VEHICLE > 1.0 || (Object) info.transform != (Object) null && info.transform.tag == "Resource" && (double) PlayerEquipment.DAMAGE_RESOURCE > 1.0)
          PlayerUI.hitmark(EPlayerHit.BUILD);
        this.player.input.sendRaycast(info);
      }
      if (mode == EPlayerPunch.LEFT)
      {
        this.player.animator.play("Punch_Left", false);
        if (Provider.isServer)
          this.player.animator.sendGesture(EPlayerGesture.PUNCH_LEFT, false);
      }
      else if (mode == EPlayerPunch.RIGHT)
      {
        this.player.animator.play("Punch_Right", false);
        if (Provider.isServer)
          this.player.animator.sendGesture(EPlayerGesture.PUNCH_RIGHT, false);
      }
      if (!Provider.isServer || this.player.input.inputs == null || this.player.input.inputs.Count == 0)
        return;
      InputInfo inputInfo = this.player.input.inputs.Dequeue();
      if (inputInfo == null || (double) (inputInfo.point - this.player.look.aim.position).sqrMagnitude > 36.0)
        return;
      DamageTool.impact(inputInfo.point, inputInfo.normal, inputInfo.material, (Object) inputInfo.player != (Object) null || (Object) inputInfo.zombie != (Object) null || ((Object) inputInfo.animal != (Object) null || (Object) inputInfo.vehicle != (Object) null) || (Object) inputInfo.transform != (Object) null && (inputInfo.transform.tag == "Barricade" || inputInfo.transform.tag == "Structure" || inputInfo.transform.tag == "Resource" || inputInfo.transform.tag == "Debris"));
      EPlayerKill kill = EPlayerKill.NONE;
      float times = 1f * (float) (1.0 + (double) this.channel.owner.player.skills.mastery(0, 0) * 0.5);
      if ((Object) inputInfo.player != (Object) null)
      {
        if ((this.channel.owner.playerID.group == CSteamID.Nil || inputInfo.player.channel.owner.playerID.group != this.channel.owner.playerID.group) && Provider.isPvP)
          DamageTool.damage(inputInfo.player, EDeathCause.PUNCH, inputInfo.limb, this.channel.owner.playerID.steamID, inputInfo.direction, PlayerEquipment.DAMAGE_PLAYER_MULTIPLIER, times, true, out kill);
      }
      else if ((Object) inputInfo.zombie != (Object) null)
      {
        DamageTool.damage(inputInfo.zombie, inputInfo.limb, inputInfo.direction, PlayerEquipment.DAMAGE_ZOMBIE_MULTIPLIER, times, true, out kill);
        if ((int) this.player.movement.nav != (int) byte.MaxValue)
          inputInfo.zombie.alert(this.transform.position);
      }
      else if ((Object) inputInfo.animal != (Object) null)
      {
        DamageTool.damage(inputInfo.animal, inputInfo.limb, inputInfo.direction, PlayerEquipment.DAMAGE_ANIMAL_MULTIPLIER, times, out kill);
        inputInfo.animal.alert(this.transform.position);
      }
      else if ((Object) inputInfo.vehicle != (Object) null)
        DamageTool.damage(inputInfo.vehicle, false, PlayerEquipment.DAMAGE_VEHICLE, times, true, out kill);
      else if ((Object) inputInfo.transform != (Object) null)
      {
        if (inputInfo.transform.tag == "Barricade")
        {
          ushort result;
          if (ushort.TryParse(inputInfo.transform.name, out result))
          {
            ItemBarricadeAsset itemBarricadeAsset = (ItemBarricadeAsset) Assets.find(EAssetType.ITEM, result);
            if (itemBarricadeAsset != null && itemBarricadeAsset.isVulnerable)
              DamageTool.damage(inputInfo.transform, PlayerEquipment.DAMAGE_BARRICADE, times, out kill);
          }
        }
        else if (inputInfo.transform.tag == "Structure")
        {
          ushort result;
          if (ushort.TryParse(inputInfo.transform.name, out result))
          {
            ItemStructureAsset itemStructureAsset = (ItemStructureAsset) Assets.find(EAssetType.ITEM, result);
            if (itemStructureAsset != null && itemStructureAsset.isVulnerable)
              DamageTool.damage(inputInfo.transform, inputInfo.direction, PlayerEquipment.DAMAGE_STRUCTURE, times, out kill);
          }
        }
        else if (inputInfo.transform.tag == "Resource")
          DamageTool.damage(inputInfo.transform, inputInfo.direction, PlayerEquipment.DAMAGE_RESOURCE, times, 1f, out kill);
      }
      if (Level.info.type == ELevelType.HORDE)
      {
        if ((Object) inputInfo.zombie != (Object) null)
        {
          if (inputInfo.limb == ELimb.SKULL)
            this.player.skills.askAward(10U);
          else
            this.player.skills.askAward(5U);
        }
        if (kill != EPlayerKill.ZOMBIE)
          return;
        if (inputInfo.limb == ELimb.SKULL)
          this.player.skills.askAward(50U);
        else
          this.player.skills.askAward(25U);
      }
      else if (kill == EPlayerKill.PLAYER)
        this.player.sendStat(EPlayerStat.KILLS_PLAYERS);
      else if (kill == EPlayerKill.ZOMBIE)
      {
        this.player.sendStat(EPlayerStat.KILLS_ZOMBIES_NORMAL);
        this.player.skills.askAward(4U);
      }
      else if (kill == EPlayerKill.MEGA)
      {
        this.player.sendStat(EPlayerStat.KILLS_ZOMBIES_MEGA);
        this.player.skills.askAward(64U);
      }
      else if (kill == EPlayerKill.ANIMAL)
      {
        this.player.sendStat(EPlayerStat.KILLS_ANIMALS);
        this.player.skills.askAward(16U);
      }
      else
      {
        if (kill != EPlayerKill.RESOURCE)
          return;
        this.player.sendStat(EPlayerStat.FOUND_RESOURCES);
        this.player.skills.askAward(4U);
      }
    }

    public void simulate(uint simulation, bool inputPrimary, bool inputSecondary)
    {
      if (this.player.stance.stance == EPlayerStance.CLIMB || this.player.stance.stance == EPlayerStance.SWIM || this.player.stance.stance == EPlayerStance.DRIVING)
      {
        if (!this.isSelected || !Provider.isServer)
          return;
        this.dequip();
      }
      else
      {
        if ((double) Time.realtimeSinceStartup - (double) this.lastEquip < 0.100000001490116 || this.player.life.isDead || this.player.movement.isSafe && (this.asset == null || this.asset.useable != EUseableType.CLOTHING && this.asset.useable != EUseableType.CONSUMEABLE))
          return;
        if (this.isSelected)
        {
          if (inputPrimary != this.lastPrimary)
          {
            this.lastPrimary = inputPrimary;
            if (this.isEquipped)
            {
              if (inputPrimary)
                this.useable.startPrimary();
              else
                this.useable.stopPrimary();
            }
          }
          if (inputSecondary != this.lastSecondary)
          {
            this.lastSecondary = inputSecondary;
            if (this.isEquipped)
            {
              if (inputSecondary)
                this.useable.startSecondary();
              else
                this.useable.stopSecondary();
            }
          }
          if (!this.isEquipped)
            return;
          this.useable.simulate(simulation);
        }
        else
        {
          if (inputPrimary != this.lastPrimary)
          {
            this.lastPrimary = inputPrimary;
            if (!this.isBusy && this.player.stance.stance != EPlayerStance.PRONE && (inputPrimary && simulation - this.lastPunch > 5U))
            {
              this.lastPunch = simulation;
              this.punch(EPlayerPunch.LEFT);
            }
          }
          if (inputSecondary == this.lastSecondary)
            return;
          this.lastSecondary = inputSecondary;
          if (this.isBusy || this.player.stance.stance == EPlayerStance.PRONE || (!inputSecondary || simulation - this.lastPunch <= 5U))
            return;
          this.lastPunch = simulation;
          this.punch(EPlayerPunch.RIGHT);
        }
      }
    }

    public void tock(uint clock)
    {
      if (!this.isSelected || !this.isEquipped)
        return;
      this.useable.tock(clock);
    }

    private void updateVision()
    {
      if (!this.channel.isOwner)
        return;
      LevelLighting.vision = !this.hasVision || this.player.look.perspective != EPlayerPerspective.FIRST || (int) this.player.clothing.glassesState[0] == 0 ? ELightingVision.NONE : this.player.clothing.glassesAsset.vision;
      LevelLighting.updateLighting();
      PlayerLifeUI.updateGrayscale();
    }

    private void onVisionUpdated(bool isViewing)
    {
      if (isViewing)
        this.warp = (double) Random.value < 0.25;
      else
        this.warp = false;
    }

    private void onPerspectiveUpdated(EPlayerPerspective perspective)
    {
      if (!this.hasVision)
        return;
      this.updateVision();
    }

    private void onGlassesUpdated(ushort id, byte quality, byte[] state)
    {
      if (!this.channel.isOwner && !Provider.isServer)
        return;
      this.hasVision = (int) id != 0 && this.player.clothing.glassesAsset != null && this.player.clothing.glassesAsset.vision != ELightingVision.NONE;
      this.updateVision();
    }

    private void onLifeUpdated(bool isDead)
    {
      if (!isDead)
        return;
      for (byte slot = (byte) 0; (int) slot < (int) PlayerInventory.SLOTS; ++slot)
        this.updateSlot(slot, (ushort) 0, new byte[0]);
      if (Provider.isServer)
        this.dequip();
      this.isBusy = false;
      this._equippedPage = byte.MaxValue;
      this._equipped_x = byte.MaxValue;
      this._equipped_y = byte.MaxValue;
    }

    private void hotkey(byte button)
    {
      if (!PlayerUI.window.showCursor)
      {
        if (this.isBusy)
          return;
        if ((int) button < (int) PlayerInventory.SLOTS)
        {
          ItemJar itemJar = this.player.inventory.getItem(button, (byte) 0);
          if (itemJar == null)
            return;
          this.equip(button, itemJar.x, itemJar.y);
        }
        else
        {
          ItemJar itemJar = PlayerDashboardInventoryUI.key(button);
          if (itemJar == null)
            return;
          this.equip(PlayerInventory.SLOTS, itemJar.x, itemJar.y);
        }
      }
      else
      {
        if ((int) button < (int) PlayerInventory.SLOTS || !PlayerDashboardInventoryUI.active)
          return;
        PlayerDashboardInventoryUI.hotkey(button);
      }
    }

    public void init()
    {
      this.channel.send("askEquipment", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER);
    }

    private void Update()
    {
      if (this.channel.isOwner)
      {
        if (!PlayerUI.window.showCursor)
        {
          if (Input.GetKeyDown(ControlsSettings.vision) && this.hasVision)
          {
            this.player.playSound((AudioClip) Resources.Load("Sounds/General/Beep"));
            this.channel.send("askToggleVision", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER);
          }
          if (Input.GetKey(ControlsSettings.primary))
            this.prim = true;
          if (Input.GetKey(ControlsSettings.secondary))
            this.sec = true;
          if (Input.GetKeyDown(ControlsSettings.dequip) && this.isSelected && (!this.isBusy && this.isEquipped))
            this.dequip();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
          this.hotkey((byte) 0);
        if (Input.GetKeyDown(KeyCode.Alpha2))
          this.hotkey((byte) 1);
        if (Input.GetKeyDown(KeyCode.Alpha3))
          this.hotkey((byte) 2);
        if (Input.GetKeyDown(KeyCode.Alpha4))
          this.hotkey((byte) 3);
        if (Input.GetKeyDown(KeyCode.Alpha5))
          this.hotkey((byte) 4);
        if (Input.GetKeyDown(KeyCode.Alpha6))
          this.hotkey((byte) 5);
        if (Input.GetKeyDown(KeyCode.Alpha7))
          this.hotkey((byte) 6);
        if (Input.GetKeyDown(KeyCode.Alpha8))
          this.hotkey((byte) 7);
        if (Input.GetKeyDown(KeyCode.Alpha9))
          this.hotkey((byte) 8);
        if (Input.GetKeyDown(KeyCode.Alpha0))
          this.hotkey((byte) 9);
      }
      if (!this.isSelected)
        return;
      this.useable.tick();
    }

    private void FixedUpdate()
    {
      if (!this.channel.isOwner)
        return;
      if (!PlayerUI.window.showCursor && ((Object) this.player.movement.getVehicle() == (Object) null || this.player.look.perspective == EPlayerPerspective.FIRST))
      {
        this._primary = this.prim || Input.GetKey(ControlsSettings.primary);
        if (ControlsSettings.aiming == EControlMode.TOGGLE && this.asset != null && (this.asset.type == EItemType.GUN || this.asset.type == EItemType.OPTIC))
        {
          if ((this.sec ? 1 : (Input.GetKey(ControlsSettings.secondary) ? 1 : 0)) != (this.flipSecondary ? 1 : 0))
          {
            this.flipSecondary = this.sec || Input.GetKey(ControlsSettings.secondary);
            if (this.flipSecondary)
              this._secondary = !this.secondary;
          }
        }
        else
        {
          this._secondary = this.sec || Input.GetKey(ControlsSettings.secondary);
          this.flipSecondary = this.secondary;
        }
        this.prim = false;
        this.sec = false;
        if (!this.warp)
          return;
        bool primary = this.primary;
        this._primary = this.secondary;
        this._secondary = primary;
      }
      else
      {
        this._primary = false;
        this._secondary = false;
      }
    }

    private void Start()
    {
      this.hasVision = this.hasVision = (int) this.player.clothing.glasses != 0 && this.player.clothing.glassesAsset != null && this.player.clothing.glassesAsset.vision != ELightingVision.NONE;
      this.thirdSlots = new Transform[(int) PlayerInventory.SLOTS];
      if (this.channel.isOwner && (Object) this.player.character != (Object) null)
        this.characterSlots = new Transform[(int) PlayerInventory.SLOTS];
      this.warp = false;
      this._equippedPage = byte.MaxValue;
      this._equipped_x = byte.MaxValue;
      this._equipped_y = byte.MaxValue;
      this.isBusy = false;
      if ((Object) this.player.third != (Object) null)
      {
        this._thirdPrimaryMeleeSlot = this.player.animator.thirdSkeleton.FindChild("Spine").FindChild("Primary_Melee");
        this._thirdPrimaryLargeGunSlot = this.player.animator.thirdSkeleton.FindChild("Spine").FindChild("Primary_Large_Gun");
        this._thirdPrimarySmallGunSlot = this.player.animator.thirdSkeleton.FindChild("Spine").FindChild("Primary_Small_Gun");
        this._thirdSecondaryMeleeSlot = this.player.animator.thirdSkeleton.FindChild("Right_Hip").FindChild("Right_Leg").FindChild("Secondary_Melee");
        this._thirdSecondaryGunSlot = this.player.animator.thirdSkeleton.FindChild("Right_Hip").FindChild("Right_Leg").FindChild("Secondary_Gun");
      }
      if (this.channel.isOwner)
      {
        this._characterPrimaryMeleeSlot = this.player.character.FindChild("Skeleton").FindChild("Spine").FindChild("Primary_Melee");
        this._characterPrimaryLargeGunSlot = this.player.character.FindChild("Skeleton").FindChild("Spine").FindChild("Primary_Large_Gun");
        this._characterPrimarySmallGunSlot = this.player.character.FindChild("Skeleton").FindChild("Spine").FindChild("Primary_Small_Gun");
        this._characterSecondaryMeleeSlot = this.player.character.FindChild("Skeleton").FindChild("Right_Hip").FindChild("Right_Leg").FindChild("Secondary_Melee");
        this._characterSecondaryGunSlot = this.player.character.FindChild("Skeleton").FindChild("Right_Hip").FindChild("Right_Leg").FindChild("Secondary_Gun");
      }
      if ((Object) this.player.first != (Object) null)
      {
        this._firstLeftHook = this.player.animator.firstSkeleton.FindChild("Spine").FindChild("Left_Shoulder").FindChild("Left_Arm").FindChild("Left_Hand").FindChild("Left_Hook");
        this._firstRightHook = this.player.animator.firstSkeleton.FindChild("Spine").FindChild("Right_Shoulder").FindChild("Right_Arm").FindChild("Right_Hand").FindChild("Right_Hook");
      }
      if ((Object) this.player.third != (Object) null)
      {
        this._thirdLeftHook = this.player.animator.thirdSkeleton.FindChild("Spine").FindChild("Left_Shoulder").FindChild("Left_Arm").FindChild("Left_Hand").FindChild("Left_Hook");
        this._thirdRightHook = this.player.animator.thirdSkeleton.FindChild("Spine").FindChild("Right_Shoulder").FindChild("Right_Arm").FindChild("Right_Hand").FindChild("Right_Hook");
      }
      if (this.channel.isOwner && (Object) this.player.character != (Object) null)
      {
        this._characterLeftHook = this.player.character.transform.FindChild("Skeleton").FindChild("Spine").FindChild("Left_Shoulder").FindChild("Left_Arm").FindChild("Left_Hand").FindChild("Left_Hook");
        this._characterRightHook = this.player.character.transform.FindChild("Skeleton").FindChild("Spine").FindChild("Right_Shoulder").FindChild("Right_Arm").FindChild("Right_Hand").FindChild("Right_Hook");
      }
      if (this.channel.isOwner || Provider.isServer)
      {
        this.player.life.onVisionUpdated += new VisionUpdated(this.onVisionUpdated);
        this.player.clothing.onGlassesUpdated += new GlassesUpdated(this.onGlassesUpdated);
      }
      if (this.channel.isOwner)
        this.player.look.onPerspectiveUpdated += new PerspectiveUpdated(this.onPerspectiveUpdated);
      this.player.life.onLifeUpdated += new LifeUpdated(this.onLifeUpdated);
      this.Invoke("init", 0.1f);
    }
  }
}
