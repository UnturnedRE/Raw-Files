// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.Characters
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using SDG.Provider;
using Steamworks;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
  public class Characters : MonoBehaviour
  {
    public static readonly byte SAVEDATA_VERSION = (byte) 14;
    private static bool hasLoaded;
    public static bool hasPlayed;
    private static bool hasDropped;
    public static CharacterUpdated onCharacterUpdated;
    private static byte _selected;
    private static Character[] list;
    private static Transform character;
    public static HumanClothes clothes;
    private static Transform[] slots;
    private static Transform primaryMeleeSlot;
    private static Transform primaryLargeGunSlot;
    private static Transform primarySmallGunSlot;
    private static Transform secondaryMeleeSlot;
    private static Transform secondaryGunSlot;
    private static List<ulong> _packageSkins;
    private static float characterOffset;
    private static float _characterYaw;
    public static float characterYaw;

    public static byte selected
    {
      get
      {
        return Characters._selected;
      }
      set
      {
        Characters._selected = value;
        if (Characters.onCharacterUpdated != null)
          Characters.onCharacterUpdated(Characters.selected, Characters.active);
        Characters.apply();
      }
    }

    public static Character active
    {
      get
      {
        return Characters.list[(int) Characters.selected];
      }
    }

    public static List<ulong> packageSkins
    {
      get
      {
        return Characters._packageSkins;
      }
    }

    public static void rename(string name)
    {
      Characters.active.name = name;
      if (Characters.onCharacterUpdated == null)
        return;
      Characters.onCharacterUpdated(Characters.selected, Characters.active);
    }

    public static void specialize(EPlayerSpeciality speciality)
    {
      Characters.active.speciality = speciality;
      if (Characters.onCharacterUpdated == null)
        return;
      Characters.onCharacterUpdated(Characters.selected, Characters.active);
    }

    public static void growFace(byte face)
    {
      Characters.active.face = face;
      Characters.apply();
    }

    public static void growHair(byte hair)
    {
      Characters.active.hair = hair;
      Characters.apply();
    }

    public static void growBeard(byte beard)
    {
      Characters.active.beard = beard;
      Characters.apply();
    }

    public static void paintSkin(Color color)
    {
      Characters.active.skin = color;
      Characters.apply();
    }

    public static void paintColor(Color color)
    {
      Characters.active.color = color;
      Characters.apply();
    }

    public static void renick(string nick)
    {
      Characters.active.nick = nick;
      if (Characters.onCharacterUpdated == null)
        return;
      Characters.onCharacterUpdated(Characters.selected, Characters.active);
    }

    public static void group(CSteamID group)
    {
      Characters.active.group = !(Characters.active.group == group) ? group : CSteamID.Nil;
      if (Characters.onCharacterUpdated == null)
        return;
      Characters.onCharacterUpdated(Characters.selected, Characters.active);
    }

    public static void ungroup()
    {
      Characters.active.group = CSteamID.Nil;
      if (Characters.onCharacterUpdated == null)
        return;
      Characters.onCharacterUpdated(Characters.selected, Characters.active);
    }

    public static void hand(bool state)
    {
      Characters.active.hand = state;
      Characters.apply();
      if (Characters.onCharacterUpdated == null)
        return;
      Characters.onCharacterUpdated(Characters.selected, Characters.active);
    }

    public static void package(ulong package)
    {
      int inventoryItem1 = Provider.provider.economyService.getInventoryItem(package);
      if (inventoryItem1 == 0)
        return;
      ushort inventoryItemId1 = Provider.provider.economyService.getInventoryItemID(inventoryItem1);
      if ((int) inventoryItemId1 == 0)
        return;
      ItemAsset itemAsset = (ItemAsset) Assets.find(EAssetType.ITEM, inventoryItemId1);
      if (itemAsset == null)
        return;
      if (itemAsset.type == EItemType.GUN || itemAsset.type == EItemType.MELEE)
      {
        ushort inventorySkinId = Provider.provider.economyService.getInventorySkinID(inventoryItem1);
        if ((int) inventorySkinId == 0)
          return;
        SkinAsset skinAsset = (SkinAsset) Assets.find(EAssetType.SKIN, inventorySkinId);
        if (!Characters.packageSkins.Remove(package))
        {
          for (int index = 0; index < Characters.packageSkins.Count; ++index)
          {
            ulong package1 = Characters.packageSkins[index];
            if ((long) package1 != 0L)
            {
              int inventoryItem2 = Provider.provider.economyService.getInventoryItem(package1);
              if (inventoryItem2 != 0)
              {
                ushort inventoryItemId2 = Provider.provider.economyService.getInventoryItemID(inventoryItem2);
                if ((int) inventoryItemId2 != 0 && (int) inventoryItemId1 == (int) inventoryItemId2)
                  Characters.packageSkins.RemoveAt(index);
              }
            }
          }
          Characters.packageSkins.Add(package);
        }
      }
      else if (itemAsset.type == EItemType.SHIRT)
        Characters.active.packageShirt = (long) Characters.active.packageShirt != (long) package ? package : 0UL;
      else if (itemAsset.type == EItemType.PANTS)
        Characters.active.packagePants = (long) Characters.active.packagePants != (long) package ? package : 0UL;
      else if (itemAsset.type == EItemType.HAT)
        Characters.active.packageHat = (long) Characters.active.packageHat != (long) package ? package : 0UL;
      else if (itemAsset.type == EItemType.BACKPACK)
        Characters.active.packageBackpack = (long) Characters.active.packageBackpack != (long) package ? package : 0UL;
      else if (itemAsset.type == EItemType.VEST)
        Characters.active.packageVest = (long) Characters.active.packageVest != (long) package ? package : 0UL;
      else if (itemAsset.type == EItemType.MASK)
        Characters.active.packageMask = (long) Characters.active.packageMask != (long) package ? package : 0UL;
      else if (itemAsset.type == EItemType.GLASSES)
        Characters.active.packageGlasses = (long) Characters.active.packageGlasses != (long) package ? package : 0UL;
      Characters.apply();
      if (Characters.onCharacterUpdated == null)
        return;
      Characters.onCharacterUpdated(Characters.selected, Characters.active);
    }

    private static void apply(byte slot)
    {
      if ((Object) Characters.slots[(int) slot] != (Object) null)
        Object.Destroy((Object) Characters.slots[(int) slot].gameObject);
      ushort id = (ushort) 0;
      byte[] state = (byte[]) null;
      if ((int) slot == 0)
      {
        id = Characters.active.primaryItem;
        state = Characters.active.primaryState;
      }
      else if ((int) slot == 1)
      {
        id = Characters.active.secondaryItem;
        state = Characters.active.secondaryState;
      }
      if ((int) id == 0)
        return;
      ItemAsset itemAsset = (ItemAsset) Assets.find(EAssetType.ITEM, id);
      if (itemAsset == null)
        return;
      ushort skin = (ushort) 0;
      ushort mythicID = (ushort) 0;
      for (int index = 0; index < Characters.packageSkins.Count; ++index)
      {
        ulong package = Characters.packageSkins[index];
        if ((long) package != 0L)
        {
          int inventoryItem = Provider.provider.economyService.getInventoryItem(package);
          if (inventoryItem != 0)
          {
            ushort inventoryItemId = Provider.provider.economyService.getInventoryItemID(inventoryItem);
            if ((int) inventoryItemId != 0 && (int) id == (int) inventoryItemId)
            {
              skin = Provider.provider.economyService.getInventorySkinID(inventoryItem);
              mythicID = Provider.provider.economyService.getInventoryMythicID(inventoryItem);
              break;
            }
          }
        }
      }
      Transform model = ItemTool.getItem(id, skin, (byte) 100, state, false);
      if ((int) slot == 0)
      {
        if (itemAsset.type == EItemType.MELEE)
          model.transform.parent = Characters.primaryMeleeSlot;
        else if (itemAsset.slot == ESlotType.PRIMARY)
          model.transform.parent = Characters.primaryLargeGunSlot;
        else
          model.transform.parent = Characters.primarySmallGunSlot;
      }
      else if ((int) slot == 1)
      {
        if (itemAsset.type == EItemType.MELEE)
          model.transform.parent = Characters.secondaryMeleeSlot;
        else
          model.transform.parent = Characters.secondaryGunSlot;
      }
      model.localPosition = Vector3.zero;
      model.localRotation = Quaternion.Euler(0.0f, 0.0f, 90f);
      model.localScale = Vector3.one;
      Object.Destroy((Object) model.GetComponent<Collider>());
      if ((int) mythicID != 0)
        ItemTool.applyEffect(model, mythicID, EEffectType.THIRD);
      Characters.slots[(int) slot] = model;
    }

    public static void apply()
    {
      if (Characters.active == null)
        Debug.LogError((object) "Failed to find an active character.");
      else if ((Object) Characters.clothes == (Object) null)
      {
        Debug.LogError((object) "Failed to find character clothes.");
      }
      else
      {
        Characters.character.localScale = new Vector3(!Characters.active.hand ? 1f : -1f, 1f, 1f);
        Characters.clothes.shirt = Characters.active.shirt;
        Characters.clothes.pants = Characters.active.pants;
        Characters.clothes.hat = Characters.active.hat;
        Characters.clothes.backpack = Characters.active.backpack;
        Characters.clothes.vest = Characters.active.vest;
        Characters.clothes.mask = Characters.active.mask;
        Characters.clothes.glasses = Characters.active.glasses;
        Characters.clothes.visualShirt = (long) Characters.active.packageShirt == 0L ? 0 : Provider.provider.economyService.getInventoryItem(Characters.active.packageShirt);
        Characters.clothes.visualPants = (long) Characters.active.packagePants == 0L ? 0 : Provider.provider.economyService.getInventoryItem(Characters.active.packagePants);
        Characters.clothes.visualHat = (long) Characters.active.packageHat == 0L ? 0 : Provider.provider.economyService.getInventoryItem(Characters.active.packageHat);
        Characters.clothes.visualBackpack = (long) Characters.active.packageBackpack == 0L ? 0 : Provider.provider.economyService.getInventoryItem(Characters.active.packageBackpack);
        Characters.clothes.visualVest = (long) Characters.active.packageVest == 0L ? 0 : Provider.provider.economyService.getInventoryItem(Characters.active.packageVest);
        Characters.clothes.visualMask = (long) Characters.active.packageMask == 0L ? 0 : Provider.provider.economyService.getInventoryItem(Characters.active.packageMask);
        Characters.clothes.visualGlasses = (long) Characters.active.packageGlasses == 0L ? 0 : Provider.provider.economyService.getInventoryItem(Characters.active.packageGlasses);
        Characters.clothes.face = Characters.active.face;
        Characters.clothes.hair = Characters.active.hair;
        Characters.clothes.beard = Characters.active.beard;
        Characters.clothes.skin = Characters.active.skin;
        Characters.clothes.color = Characters.active.color;
        Characters.clothes.apply();
        for (byte slot = (byte) 0; (int) slot < Characters.slots.Length; ++slot)
          Characters.apply(slot);
      }
    }

    private static void onInventoryRefreshed()
    {
      if ((Object) Characters.clothes != (Object) null && Characters.list != null && Characters.packageSkins != null)
      {
        for (int index = Characters.packageSkins.Count - 1; index >= 0; --index)
        {
          ulong package = Characters.packageSkins[index];
          if ((long) package != 0L && Provider.provider.economyService.getInventoryItem(package) == 0)
            Characters.packageSkins.RemoveAt(index);
        }
        for (int index = 0; index < Characters.list.Length; ++index)
        {
          Character character = Characters.list[index];
          if (character != null)
          {
            if ((long) character.packageShirt != 0L && Provider.provider.economyService.getInventoryItem(character.packageShirt) == 0)
              character.packageShirt = 0UL;
            if ((long) character.packagePants != 0L && Provider.provider.economyService.getInventoryItem(character.packagePants) == 0)
              character.packagePants = 0UL;
            if ((long) character.packageHat != 0L && Provider.provider.economyService.getInventoryItem(character.packageHat) == 0)
              character.packageHat = 0UL;
            if ((long) character.packageBackpack != 0L && Provider.provider.economyService.getInventoryItem(character.packageBackpack) == 0)
              character.packageBackpack = 0UL;
            if ((long) character.packageVest != 0L && Provider.provider.economyService.getInventoryItem(character.packageVest) == 0)
              character.packageVest = 0UL;
            if ((long) character.packageMask != 0L && Provider.provider.economyService.getInventoryItem(character.packageMask) == 0)
              character.packageMask = 0UL;
            if ((long) character.packageGlasses != 0L && Provider.provider.economyService.getInventoryItem(character.packageGlasses) == 0)
              character.packageGlasses = 0UL;
          }
        }
        Characters.apply();
      }
      if (Characters.hasDropped)
        return;
      Characters.hasDropped = true;
      if (!Characters.hasPlayed)
        return;
      Provider.provider.economyService.dropInventory();
    }

    private void Update()
    {
      if (Dedicator.isDedicated || (Object) Characters.character == (Object) null)
        return;
      Characters._characterYaw = Mathf.Lerp(Characters._characterYaw, Characters.characterOffset + Characters.characterYaw, 4f * Time.deltaTime);
      Characters.character.transform.rotation = Quaternion.Euler(90f, Characters._characterYaw, 0.0f);
    }

    private void Start()
    {
      Characters.hasDropped = false;
      if (Dedicator.isDedicated)
        return;
      if (!Characters.hasLoaded)
        Provider.provider.economyService.onInventoryRefreshed += new TempSteamworksEconomy.InventoryRefreshed(Characters.onInventoryRefreshed);
      Characters.load();
    }

    private void Awake()
    {
      if (Dedicator.isDedicated)
        return;
      Characters.character = GameObject.Find("Hero").transform;
      Characters.clothes = Characters.character.GetComponent<HumanClothes>();
      Characters.slots = new Transform[(int) PlayerInventory.SLOTS];
      Characters.primaryMeleeSlot = Characters.character.FindChild("Skeleton").FindChild("Spine").FindChild("Primary_Melee");
      Characters.primaryLargeGunSlot = Characters.character.FindChild("Skeleton").FindChild("Spine").FindChild("Primary_Large_Gun");
      Characters.primarySmallGunSlot = Characters.character.FindChild("Skeleton").FindChild("Spine").FindChild("Primary_Small_Gun");
      Characters.secondaryMeleeSlot = Characters.character.FindChild("Skeleton").FindChild("Right_Hip").FindChild("Right_Leg").FindChild("Secondary_Melee");
      Characters.secondaryGunSlot = Characters.character.FindChild("Skeleton").FindChild("Right_Hip").FindChild("Right_Leg").FindChild("Secondary_Gun");
      Characters.characterOffset = Characters.character.transform.eulerAngles.y;
      Characters._characterYaw = Characters.characterOffset;
      Characters.characterYaw = 0.0f;
    }

    public static void load()
    {
      Provider.provider.economyService.refreshInventory();
      if (Characters.list != null)
      {
        for (byte index = (byte) 0; (int) index < Characters.list.Length; ++index)
        {
          if (Characters.list[(int) index] != null && Characters.onCharacterUpdated != null)
            Characters.onCharacterUpdated(index, Characters.list[(int) index]);
        }
      }
      else
      {
        Characters.list = new Character[(int) Customization.FREE_CHARACTERS + (int) Customization.PRO_CHARACTERS];
        Characters._packageSkins = new List<ulong>();
        if (ReadWrite.fileExists("/Characters.dat", true))
        {
          Block block = ReadWrite.readBlock("/Characters.dat", true, (byte) 0);
          if (block != null)
          {
            byte num1 = block.readByte();
            if ((int) num1 >= 12)
            {
              if ((int) num1 >= 14)
              {
                ushort num2 = block.readUInt16();
                for (ushort index = (ushort) 0; (int) index < (int) num2; ++index)
                {
                  ulong num3 = block.readUInt64();
                  if ((long) num3 != 0L)
                    Characters.packageSkins.Add(num3);
                }
              }
              Characters._selected = block.readByte();
              if (!Provider.isPro && (int) Characters.selected >= (int) Customization.FREE_CHARACTERS)
                Characters._selected = (byte) 0;
              for (byte index = (byte) 0; (int) index < Characters.list.Length; ++index)
              {
                ushort newShirt = block.readUInt16();
                ushort newPants = block.readUInt16();
                ushort newHat = block.readUInt16();
                ushort newBackpack = block.readUInt16();
                ushort newVest = block.readUInt16();
                ushort newMask = block.readUInt16();
                ushort newGlasses = block.readUInt16();
                ulong newPackageShirt = block.readUInt64();
                ulong newPackagePants = block.readUInt64();
                ulong newPackageHat = block.readUInt64();
                ulong newPackageBackpack = block.readUInt64();
                ulong newPackageVest = block.readUInt64();
                ulong newPackageMask = block.readUInt64();
                ulong newPackageGlasses = block.readUInt64();
                ushort newPrimaryItem = block.readUInt16();
                byte[] newPrimaryState = block.readByteArray();
                ushort newSecondaryItem = block.readUInt16();
                byte[] newSecondaryState = block.readByteArray();
                byte newFace = block.readByte();
                byte newHair = block.readByte();
                byte newBeard = block.readByte();
                Color color1 = block.readColor();
                Color color2 = block.readColor();
                bool newHand = block.readBoolean();
                string newName = block.readString();
                string newNick = block.readString();
                CSteamID csteamId = block.readSteamID();
                byte num2 = block.readByte();
                if (!Provider.provider.communityService.checkGroup(csteamId))
                  csteamId = CSteamID.Nil;
                if ((int) num2 >= (int) PlayerSkills.SPECIALITIES)
                  num2 = (byte) 0;
                if (!Provider.isPro)
                {
                  if ((int) newFace >= (int) Customization.FACES_FREE)
                    newFace = (byte) Random.Range(0, (int) Customization.FACES_FREE);
                  if ((int) newHair >= (int) Customization.HAIRS_FREE)
                    newHair = (byte) Random.Range(0, (int) Customization.HAIRS_FREE);
                  if ((int) newBeard >= (int) Customization.BEARDS_FREE)
                    newBeard = (byte) 0;
                  if (!Customization.checkSkin(color1))
                    color1 = Customization.SKINS[Random.Range(0, Customization.SKINS.Length)];
                  if (!Customization.checkColor(color2))
                    color2 = Customization.COLORS[Random.Range(0, Customization.COLORS.Length)];
                }
                Characters.list[(int) index] = new Character(newShirt, newPants, newHat, newBackpack, newVest, newMask, newGlasses, newPackageShirt, newPackagePants, newPackageHat, newPackageBackpack, newPackageVest, newPackageMask, newPackageGlasses, newPrimaryItem, newPrimaryState, newSecondaryItem, newSecondaryState, newFace, newHair, newBeard, color1, color2, newHand, newName, newNick, csteamId, (EPlayerSpeciality) num2);
                if (Characters.onCharacterUpdated != null)
                  Characters.onCharacterUpdated(index, Characters.list[(int) index]);
              }
            }
            else
            {
              for (byte index = (byte) 0; (int) index < Characters.list.Length; ++index)
              {
                Characters.list[(int) index] = new Character();
                if (Characters.onCharacterUpdated != null)
                  Characters.onCharacterUpdated(index, Characters.list[(int) index]);
              }
            }
          }
        }
        else
          Characters._selected = (byte) 0;
        for (byte index = (byte) 0; (int) index < Characters.list.Length; ++index)
        {
          if (Characters.list[(int) index] == null)
          {
            Characters.list[(int) index] = new Character();
            if (Characters.onCharacterUpdated != null)
              Characters.onCharacterUpdated(index, Characters.list[(int) index]);
          }
        }
        Characters.apply();
        Characters.hasLoaded = true;
      }
    }

    public static void save()
    {
      if (!Characters.hasLoaded)
        return;
      Block block = new Block();
      block.writeByte(Characters.SAVEDATA_VERSION);
      block.writeUInt16((ushort) Characters.packageSkins.Count);
      for (ushort index = (ushort) 0; (int) index < Characters.packageSkins.Count; ++index)
      {
        ulong num = Characters.packageSkins[(int) index];
        block.writeUInt64(num);
      }
      block.writeByte(Characters.selected);
      for (byte index = (byte) 0; (int) index < Characters.list.Length; ++index)
      {
        Character character = Characters.list[(int) index] ?? new Character();
        block.writeUInt16(character.shirt);
        block.writeUInt16(character.pants);
        block.writeUInt16(character.hat);
        block.writeUInt16(character.backpack);
        block.writeUInt16(character.vest);
        block.writeUInt16(character.mask);
        block.writeUInt16(character.glasses);
        block.writeUInt64(character.packageShirt);
        block.writeUInt64(character.packagePants);
        block.writeUInt64(character.packageHat);
        block.writeUInt64(character.packageBackpack);
        block.writeUInt64(character.packageVest);
        block.writeUInt64(character.packageMask);
        block.writeUInt64(character.packageGlasses);
        block.writeUInt16(character.primaryItem);
        block.writeByteArray(character.primaryState);
        block.writeUInt16(character.secondaryItem);
        block.writeByteArray(character.secondaryState);
        block.writeByte(character.face);
        block.writeByte(character.hair);
        block.writeByte(character.beard);
        block.writeColor(character.skin);
        block.writeColor(character.color);
        block.writeBoolean(character.hand);
        block.writeString(character.name);
        block.writeString(character.nick);
        block.writeSteamID(character.group);
        block.writeByte((byte) character.speciality);
      }
      ReadWrite.writeBlock("/Characters.dat", true, block);
    }
  }
}
