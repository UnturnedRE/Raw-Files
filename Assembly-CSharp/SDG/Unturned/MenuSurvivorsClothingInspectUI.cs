// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.MenuSurvivorsClothingInspectUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class MenuSurvivorsClothingInspectUI
  {
    private static Sleek container;
    public static bool active;
    private static Sleek inventory;
    private static SleekInspect image;
    private static SleekSlider slider;
    private static int item;
    private static ulong instance;
    private static Transform inspect;
    private static Transform model;
    private static ItemLook look;
    private static Camera camera;

    public MenuSurvivorsClothingInspectUI()
    {
      MenuSurvivorsClothingInspectUI.container = new Sleek();
      MenuSurvivorsClothingInspectUI.container.positionOffset_X = 10;
      MenuSurvivorsClothingInspectUI.container.positionOffset_Y = 10;
      MenuSurvivorsClothingInspectUI.container.positionScale_Y = 1f;
      MenuSurvivorsClothingInspectUI.container.sizeOffset_X = -20;
      MenuSurvivorsClothingInspectUI.container.sizeOffset_Y = -20;
      MenuSurvivorsClothingInspectUI.container.sizeScale_X = 1f;
      MenuSurvivorsClothingInspectUI.container.sizeScale_Y = 1f;
      MenuUI.container.add(MenuSurvivorsClothingInspectUI.container);
      MenuSurvivorsClothingInspectUI.active = false;
      MenuSurvivorsClothingInspectUI.inventory = new Sleek();
      MenuSurvivorsClothingInspectUI.inventory.positionScale_X = 0.5f;
      MenuSurvivorsClothingInspectUI.inventory.positionOffset_Y = 10;
      MenuSurvivorsClothingInspectUI.inventory.sizeScale_X = 0.5f;
      MenuSurvivorsClothingInspectUI.inventory.sizeScale_Y = 1f;
      MenuSurvivorsClothingInspectUI.inventory.sizeOffset_Y = -20;
      MenuSurvivorsClothingInspectUI.inventory.constraint = ESleekConstraint.XY;
      MenuSurvivorsClothingInspectUI.container.add(MenuSurvivorsClothingInspectUI.inventory);
      MenuSurvivorsClothingInspectUI.image = new SleekInspect("RenderTextures/Item");
      MenuSurvivorsClothingInspectUI.image.positionScale_Y = 0.125f;
      MenuSurvivorsClothingInspectUI.image.sizeScale_X = 1f;
      MenuSurvivorsClothingInspectUI.image.sizeScale_Y = 0.75f;
      MenuSurvivorsClothingInspectUI.image.constraint = ESleekConstraint.XY;
      MenuSurvivorsClothingInspectUI.inventory.add((Sleek) MenuSurvivorsClothingInspectUI.image);
      MenuSurvivorsClothingInspectUI.slider = new SleekSlider();
      MenuSurvivorsClothingInspectUI.slider.positionOffset_Y = 10;
      MenuSurvivorsClothingInspectUI.slider.positionScale_Y = 1f;
      MenuSurvivorsClothingInspectUI.slider.sizeOffset_Y = 20;
      MenuSurvivorsClothingInspectUI.slider.sizeScale_X = 1f;
      MenuSurvivorsClothingInspectUI.slider.orientation = ESleekOrientation.HORIZONTAL;
      MenuSurvivorsClothingInspectUI.slider.onDragged = new Dragged(MenuSurvivorsClothingInspectUI.onDraggedSlider);
      MenuSurvivorsClothingInspectUI.image.add((Sleek) MenuSurvivorsClothingInspectUI.slider);
      MenuSurvivorsClothingInspectUI.inspect = GameObject.Find("Inspect").transform;
      MenuSurvivorsClothingInspectUI.look = MenuSurvivorsClothingInspectUI.inspect.GetComponent<ItemLook>();
      MenuSurvivorsClothingInspectUI.camera = MenuSurvivorsClothingInspectUI.look.camera;
    }

    public static void open()
    {
      if (MenuSurvivorsClothingInspectUI.active)
      {
        MenuSurvivorsClothingInspectUI.close();
      }
      else
      {
        MenuSurvivorsClothingInspectUI.active = true;
        MenuSurvivorsClothingInspectUI.camera.gameObject.SetActive(true);
        MenuSurvivorsClothingInspectUI.look._yaw = 0.0f;
        MenuSurvivorsClothingInspectUI.look.yaw = 0.0f;
        MenuSurvivorsClothingInspectUI.slider.state = 0.0f;
        MenuSurvivorsClothingInspectUI.container.lerpPositionScale(0.0f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
      }
    }

    public static void close()
    {
      if (!MenuSurvivorsClothingInspectUI.active)
        return;
      MenuSurvivorsClothingInspectUI.active = false;
      MenuSurvivorsClothingInspectUI.camera.gameObject.SetActive(false);
      MenuSurvivorsClothingInspectUI.container.lerpPositionScale(0.0f, 1f, ESleekLerp.EXPONENTIAL, 20f);
    }

    public static void viewItem(int newItem, ulong newInstance)
    {
      MenuSurvivorsClothingInspectUI.item = newItem;
      MenuSurvivorsClothingInspectUI.instance = newInstance;
      if ((Object) MenuSurvivorsClothingInspectUI.model != (Object) null)
        Object.Destroy((Object) MenuSurvivorsClothingInspectUI.model.gameObject);
      ushort inventoryItemId = Provider.provider.economyService.getInventoryItemID(MenuSurvivorsClothingInspectUI.item);
      ushort inventoryMythicId = Provider.provider.economyService.getInventoryMythicID(MenuSurvivorsClothingInspectUI.item);
      ItemAsset itemAsset = (ItemAsset) Assets.find(EAssetType.ITEM, inventoryItemId);
      if (itemAsset.type == EItemType.GUN || itemAsset.type == EItemType.MELEE)
      {
        ushort inventorySkinId = Provider.provider.economyService.getInventorySkinID(MenuSurvivorsClothingInspectUI.item);
        SkinAsset skinAsset = (SkinAsset) Assets.find(EAssetType.SKIN, inventorySkinId);
        MenuSurvivorsClothingInspectUI.model = ItemTool.getItem(itemAsset.id, inventorySkinId, (byte) 100, itemAsset.getState(), false);
        if ((int) inventoryMythicId != 0)
          ItemTool.applyEffect(MenuSurvivorsClothingInspectUI.model, inventoryMythicId, EEffectType.THIRD);
      }
      else
      {
        MenuSurvivorsClothingInspectUI.model = ItemTool.getItem(itemAsset.id, (ushort) 0, (byte) 100, itemAsset.getState(), false);
        if ((int) inventoryMythicId != 0)
          ItemTool.applyEffect(MenuSurvivorsClothingInspectUI.model, inventoryMythicId, EEffectType.HOOK);
      }
      MenuSurvivorsClothingInspectUI.model.parent = MenuSurvivorsClothingInspectUI.inspect;
      MenuSurvivorsClothingInspectUI.model.localPosition = Vector3.zero;
      MenuSurvivorsClothingInspectUI.model.localRotation = itemAsset.type != EItemType.MELEE ? Quaternion.Euler(-90f, 0.0f, 0.0f) : Quaternion.Euler(0.0f, -90f, 90f);
      if ((Object) MenuSurvivorsClothingInspectUI.model.GetComponent<Renderer>() != (Object) null)
        MenuSurvivorsClothingInspectUI.look.pos = MenuSurvivorsClothingInspectUI.model.GetComponent<Renderer>().bounds.center;
      else if ((Object) MenuSurvivorsClothingInspectUI.model.GetComponent<LODGroup>() != (Object) null)
      {
        for (int index = 0; index < 4; ++index)
        {
          Transform child = MenuSurvivorsClothingInspectUI.model.FindChild("Model_" + (object) index);
          if (!((Object) child == (Object) null))
          {
            if ((Object) child.GetComponent<Renderer>() != (Object) null)
            {
              MenuSurvivorsClothingInspectUI.look.pos = child.GetComponent<Renderer>().bounds.center;
              break;
            }
          }
          else
            break;
        }
      }
      MenuSurvivorsClothingInspectUI.look.pos = MenuSurvivorsClothingInspectUI.model.position + MenuSurvivorsClothingInspectUI.model.rotation * MenuSurvivorsClothingInspectUI.model.GetComponent<BoxCollider>().center;
    }

    private static void onDraggedSlider(SleekSlider slider, float state)
    {
      MenuSurvivorsClothingInspectUI.look.yaw = state * 360f;
    }
  }
}
