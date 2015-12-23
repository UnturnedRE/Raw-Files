// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ItemTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
  public class ItemTool : MonoBehaviour
  {
    private static readonly Color MOLD = new Color(0.44f, 0.4862745f, 0.3882353f);
    private static ItemTool tool;
    private static Queue<ItemIconInfo> icons;

    private static Transform getEffectSystem(ushort mythicID, EEffectType type)
    {
      MythicAsset mythicAsset = (MythicAsset) Assets.find(EAssetType.MYTHIC, mythicID);
      if (mythicAsset == null)
        return (Transform) null;
      Object original;
      switch (type)
      {
        case EEffectType.AREA:
          original = (Object) mythicAsset.systemArea;
          break;
        case EEffectType.HOOK:
          original = (Object) mythicAsset.systemHook;
          break;
        case EEffectType.THIRD:
          original = (Object) mythicAsset.systemThird;
          break;
        case EEffectType.FIRST:
          original = (Object) mythicAsset.systemFirst;
          break;
        default:
          return (Transform) null;
      }
      if (original == (Object) null)
        return (Transform) null;
      Transform transform = ((GameObject) Object.Instantiate(original)).transform;
      transform.name = "System";
      return transform;
    }

    public static void applyEffect(Transform[] bones, Transform[] systems, ushort mythicID, EEffectType type)
    {
      if ((int) mythicID == 0 || bones == null || systems == null)
        return;
      for (int index = 0; index < bones.Length; ++index)
        systems[index] = ItemTool.applyEffect(bones[index], mythicID, type);
    }

    public static Transform applyEffect(Transform model, ushort mythicID, EEffectType type)
    {
      if ((int) mythicID == 0)
        return (Transform) null;
      if ((Object) model == (Object) null)
        return (Transform) null;
      Transform child = model.FindChild("Effect");
      Transform effectSystem = ItemTool.getEffectSystem(mythicID, type);
      if ((Object) effectSystem != (Object) null)
      {
        if ((Object) child != (Object) null)
        {
          effectSystem.parent = child;
          child.gameObject.AddComponent<MythicLocker>().system = effectSystem;
        }
        else
        {
          effectSystem.parent = model;
          model.gameObject.AddComponent<MythicLocker>().system = effectSystem;
        }
        effectSystem.localPosition = Vector3.zero;
        effectSystem.localRotation = Quaternion.identity;
      }
      return effectSystem;
    }

    public static bool tryForceGiveItem(Player player, ushort id, byte amount)
    {
      ItemAsset itemAsset = (ItemAsset) Assets.find(EAssetType.ITEM, id);
      if (itemAsset == null || itemAsset.isPro)
        return false;
      for (int index = 0; index < (int) amount; ++index)
      {
        Item obj = new Item(id, true);
        player.inventory.forceAddItem(obj, true);
      }
      return true;
    }

    public static bool checkUseable(byte page, ushort id)
    {
      ItemAsset itemAsset = (ItemAsset) Assets.find(EAssetType.ITEM, id);
      if (itemAsset == null)
        return false;
      if (itemAsset.slot == ESlotType.NONE)
        return itemAsset.useable != EUseableType.NONE;
      if (itemAsset.slot == ESlotType.PRIMARY)
      {
        if ((int) page == 0)
          return itemAsset.useable != EUseableType.NONE;
        return false;
      }
      if (itemAsset.slot == ESlotType.SECONDARY && ((int) page == 0 || (int) page == 1))
        return itemAsset.useable != EUseableType.NONE;
      return false;
    }

    public static Transform getItem(ushort id, ushort skin, byte quality, byte[] state, bool viewmodel)
    {
      ItemAsset itemAsset = (ItemAsset) Assets.find(EAssetType.ITEM, id);
      return ItemTool.getItem(id, skin, quality, state, viewmodel, itemAsset);
    }

    public static Transform getItem(ushort id, ushort skin, byte quality, byte[] state, bool viewmodel, ItemAsset itemAsset)
    {
      SkinAsset skinAsset = (SkinAsset) null;
      if ((int) skin != 0)
        skinAsset = (SkinAsset) Assets.find(EAssetType.SKIN, skin);
      return ItemTool.getItem(id, skin, quality, state, viewmodel, itemAsset, skinAsset);
    }

    public static Transform getItem(ushort id, ushort skin, byte quality, byte[] state, bool viewmodel, ItemAsset itemAsset, SkinAsset skinAsset)
    {
      if (itemAsset != null)
      {
        if ((int) id != (int) itemAsset.id)
          Debug.LogError((object) "ID and asset ID are not in sync!");
        Transform transform = Object.Instantiate<GameObject>(itemAsset.item).transform;
        transform.name = id.ToString();
        if (viewmodel)
          Layerer.viewmodel(transform);
        if (skinAsset != null)
        {
          if (skinAsset.isPattern)
          {
            ItemWeaponAsset itemWeaponAsset = (ItemWeaponAsset) itemAsset;
            Material skin1 = Object.Instantiate<Material>(skinAsset.primarySkin);
            skin1.SetTexture("_AlbedoBase", itemWeaponAsset.albedoBase);
            skin1.SetTexture("_MetallicBase", itemWeaponAsset.metallicBase);
            skin1.SetTexture("_EmissionBase", itemWeaponAsset.emissionBase);
            HighlighterTool.skin(transform, skin1);
          }
          else
            HighlighterTool.skin(transform, skinAsset.primarySkin);
        }
        if (itemAsset.type == EItemType.GUN)
        {
          Attachments attachments = transform.gameObject.AddComponent<Attachments>();
          attachments.updateGun((ItemGunAsset) itemAsset, skinAsset);
          attachments.updateAttachments(state, viewmodel);
        }
        else if ((itemAsset.type == EItemType.FOOD || itemAsset.type == EItemType.WATER) && (int) quality < 50)
          HighlighterTool.blend(transform, (float) (1.0 - (double) quality / 50.0), ItemTool.MOLD);
        return transform;
      }
      Transform transform1 = new GameObject().transform;
      transform1.name = id.ToString();
      if (viewmodel)
      {
        transform1.tag = "Viewmodel";
        transform1.gameObject.layer = LayerMasks.VIEWMODEL;
      }
      else
      {
        transform1.tag = "Item";
        transform1.gameObject.layer = LayerMasks.ITEM;
      }
      return transform1;
    }

    public static void getIcon(ushort id, byte quality, byte[] state, ItemIconReady callback)
    {
      ushort num1 = (ushort) 0;
      SkinAsset skinAsset = (SkinAsset) null;
      int num2;
      if ((Object) Player.player != (Object) null && Player.player.channel.owner.skins.TryGetValue(id, out num2) && num2 != 0)
      {
        num1 = Provider.provider.economyService.getInventorySkinID(num2);
        skinAsset = (SkinAsset) Assets.find(EAssetType.SKIN, num1);
      }
      ItemAsset itemAsset = (ItemAsset) Assets.find(EAssetType.ITEM, id);
      ItemTool.getIcon(id, num1, quality, state, itemAsset, skinAsset, (int) itemAsset.size_x * 50, (int) itemAsset.size_y * 50, false, callback);
    }

    public static void getIcon(ushort id, byte quality, byte[] state, ItemAsset itemAsset, ItemIconReady callback)
    {
      ushort num1 = (ushort) 0;
      SkinAsset skinAsset = (SkinAsset) null;
      int num2;
      if ((Object) Player.player != (Object) null && Player.player.channel.owner.skins.TryGetValue(id, out num2) && num2 != 0)
      {
        num1 = Provider.provider.economyService.getInventorySkinID(num2);
        skinAsset = (SkinAsset) Assets.find(EAssetType.SKIN, num1);
      }
      ItemTool.getIcon(id, num1, quality, state, itemAsset, skinAsset, (int) itemAsset.size_x * 50, (int) itemAsset.size_y * 50, false, callback);
    }

    public static void getIcon(ushort id, ushort skin, byte quality, byte[] state, ItemAsset itemAsset, SkinAsset skinAsset, int x, int y, bool scale, ItemIconReady callback)
    {
      if (itemAsset != null && (int) id != (int) itemAsset.id)
        Debug.LogError((object) "ID and item asset ID are not in sync!");
      if (skinAsset != null && (int) skin != (int) skinAsset.id)
        Debug.LogError((object) "ID and skin asset ID are not in sync!");
      ItemTool.icons.Enqueue(new ItemIconInfo()
      {
        id = id,
        skin = skin,
        quality = quality,
        state = state,
        itemAsset = itemAsset,
        skinAsset = skinAsset,
        x = x,
        y = y,
        scale = scale,
        callback = callback
      });
    }

    private void Update()
    {
      if (ItemTool.icons == null || ItemTool.icons.Count == 0)
        return;
      ItemIconInfo itemIconInfo = ItemTool.icons.Dequeue();
      if (itemIconInfo == null || itemIconInfo.itemAsset == null)
        return;
      Transform transform = ItemTool.getItem(itemIconInfo.id, itemIconInfo.skin, itemIconInfo.quality, itemIconInfo.state, false, itemIconInfo.itemAsset, itemIconInfo.skinAsset);
      transform.position = new Vector3(-256f, -256f, 0.0f);
      Transform child;
      if (itemIconInfo.scale && (int) itemIconInfo.skin != 0)
      {
        if ((double) itemIconInfo.itemAsset.size2_z == 0.0)
        {
          transform.position = new Vector3(0.0f, -256f, -256f);
          Object.Destroy((Object) transform.gameObject);
          Debug.LogError((object) "Failed to create a skin icon of size 0.");
          return;
        }
        child = transform.FindChild("Icon2");
        if ((Object) child == (Object) null)
        {
          transform.position = new Vector3(0.0f, -256f, -256f);
          Object.Destroy((Object) transform.gameObject);
          Debug.LogError((object) ("Failed to find a skin icon hook on " + (object) itemIconInfo.id + "."));
          return;
        }
      }
      else
      {
        if ((double) itemIconInfo.itemAsset.size_z == 0.0)
        {
          transform.position = new Vector3(0.0f, -256f, -256f);
          Object.Destroy((Object) transform.gameObject);
          Debug.LogError((object) "Failed to create an item icon of size 0.");
          return;
        }
        child = transform.FindChild("Icon");
        if ((Object) child == (Object) null)
        {
          transform.position = new Vector3(0.0f, -256f, -256f);
          Object.Destroy((Object) transform.gameObject);
          Debug.LogError((object) ("Failed to find an item icon hook on " + (object) itemIconInfo.id + "."));
          return;
        }
      }
      ItemTool.tool.transform.position = child.position;
      ItemTool.tool.transform.rotation = child.rotation;
      RenderTexture temporary = RenderTexture.GetTemporary(itemIconInfo.x, itemIconInfo.y, 16, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
      temporary.name = "Render";
      temporary.hideFlags = HideFlags.HideAndDontSave;
      RenderTexture.active = temporary;
      ItemTool.tool.GetComponent<Camera>().targetTexture = temporary;
      ItemTool.tool.GetComponent<Camera>().orthographicSize = itemIconInfo.itemAsset.size_z;
      if (itemIconInfo.scale)
      {
        if ((int) itemIconInfo.skin != 0)
        {
          ItemTool.tool.GetComponent<Camera>().orthographicSize = itemIconInfo.itemAsset.size2_z;
        }
        else
        {
          float num = (float) itemIconInfo.itemAsset.size_x / (float) itemIconInfo.itemAsset.size_y;
          ItemTool.tool.GetComponent<Camera>().orthographicSize *= num;
        }
      }
      bool fog = RenderSettings.fog;
      Color ambientSkyColor = RenderSettings.ambientSkyColor;
      Color ambientEquatorColor = RenderSettings.ambientEquatorColor;
      Color ambientGroundColor = RenderSettings.ambientGroundColor;
      RenderSettings.fog = false;
      RenderSettings.ambientSkyColor = Color.white;
      RenderSettings.ambientEquatorColor = Color.white;
      RenderSettings.ambientGroundColor = Color.white;
      if (Provider.isConnected)
        LevelLighting.setEnabled(false);
      ItemTool.tool.GetComponent<Camera>().Render();
      if (Provider.isConnected)
        LevelLighting.setEnabled(true);
      RenderSettings.fog = fog;
      RenderSettings.ambientSkyColor = ambientSkyColor;
      RenderSettings.ambientEquatorColor = ambientEquatorColor;
      RenderSettings.ambientGroundColor = ambientGroundColor;
      transform.position = new Vector3(0.0f, -256f, -256f);
      Object.Destroy((Object) transform.gameObject);
      Texture2D texture = new Texture2D(itemIconInfo.x, itemIconInfo.y, TextureFormat.ARGB32, false, true);
      texture.name = "Texture";
      texture.hideFlags = HideFlags.HideAndDontSave;
      texture.filterMode = FilterMode.Trilinear;
      texture.anisoLevel = 16;
      texture.ReadPixels(new Rect(0.0f, 0.0f, (float) itemIconInfo.x, (float) itemIconInfo.y), 0, 0);
      texture.Apply();
      RenderTexture.ReleaseTemporary(temporary);
      if (itemIconInfo.callback == null)
        return;
      itemIconInfo.callback(texture);
    }

    private void Start()
    {
      ItemTool.tool = this;
      ItemTool.icons = new Queue<ItemIconInfo>();
    }
  }
}
