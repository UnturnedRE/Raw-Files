// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.HumanClothes
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class HumanClothes : MonoBehaviour
  {
    private Material material_0;
    private Material material_1;
    private Transform spine;
    private Transform skull;
    private Transform[] upperBones;
    private Transform[] upperSystems;
    private Transform[] lowerBones;
    private Transform[] lowerSystems;
    private Transform hatModel;
    private Transform backpackModel;
    private Transform vestModel;
    private Transform maskModel;
    private Transform glassesModel;
    private Transform hairModel;
    private Transform beardModel;
    public HumanClothing clothing;
    public bool isMine;
    public bool isView;
    public bool isVisual;
    private bool isUpper;
    private bool isLower;
    private ItemShirtAsset visualShirtAsset;
    private ItemPantsAsset visualPantsAsset;
    private ItemHatAsset visualHatAsset;
    private ItemBackpackAsset visualBackpackAsset;
    private ItemVestAsset visualVestAsset;
    private ItemMaskAsset visualMaskAsset;
    private ItemGlassesAsset visualGlassesAsset;
    private int _visualShirt;
    private int _visualPants;
    private int _visualHat;
    public int _visualBackpack;
    public int _visualVest;
    public int _visualMask;
    public int _visualGlasses;
    private ItemShirtAsset _shirtAsset;
    private ItemPantsAsset _pantsAsset;
    private ItemHatAsset _hatAsset;
    private ItemBackpackAsset _backpackAsset;
    private ItemVestAsset _vestAsset;
    private ItemMaskAsset _maskAsset;
    private ItemGlassesAsset _glassesAsset;
    private ushort _shirt;
    private ushort _pants;
    private ushort _hat;
    public ushort _backpack;
    public ushort _vest;
    public ushort _mask;
    public ushort _glasses;
    private byte _face;
    public byte hair;
    public byte beard;
    private Color _color;

    public int visualShirt
    {
      get
      {
        return this._visualShirt;
      }
      set
      {
        if (this.visualShirt == value)
          return;
        this._visualShirt = value;
        if (Dedicator.isDedicated)
          return;
        if (this.visualShirt != 0)
        {
          try
          {
            this.visualShirtAsset = (ItemShirtAsset) Assets.find(EAssetType.ITEM, Provider.provider.economyService.getInventoryItemID(this.visualShirt));
          }
          catch
          {
            this.visualShirtAsset = (ItemShirtAsset) null;
          }
          if (this.visualShirtAsset == null || this.visualShirtAsset.isPro)
            return;
          this._visualShirt = 0;
          this.visualShirtAsset = (ItemShirtAsset) null;
        }
        else
          this.visualShirtAsset = (ItemShirtAsset) null;
      }
    }

    public int visualPants
    {
      get
      {
        return this._visualPants;
      }
      set
      {
        if (this.visualPants == value)
          return;
        this._visualPants = value;
        if (Dedicator.isDedicated)
          return;
        if (this.visualPants != 0)
        {
          try
          {
            this.visualPantsAsset = (ItemPantsAsset) Assets.find(EAssetType.ITEM, Provider.provider.economyService.getInventoryItemID(this.visualPants));
          }
          catch
          {
            this.visualPantsAsset = (ItemPantsAsset) null;
          }
          if (this.visualPantsAsset == null || this.visualPantsAsset.isPro)
            return;
          this._visualPants = 0;
          this.visualPantsAsset = (ItemPantsAsset) null;
        }
        else
          this.visualPantsAsset = (ItemPantsAsset) null;
      }
    }

    public int visualHat
    {
      get
      {
        return this._visualHat;
      }
      set
      {
        if (this.visualHat == value)
          return;
        this._visualHat = value;
        if (Dedicator.isDedicated)
          return;
        if (this.visualHat != 0)
        {
          try
          {
            this.visualHatAsset = (ItemHatAsset) Assets.find(EAssetType.ITEM, Provider.provider.economyService.getInventoryItemID(this.visualHat));
          }
          catch
          {
            this.visualHatAsset = (ItemHatAsset) null;
          }
          if (this.visualHatAsset == null || this.visualHatAsset.isPro)
            return;
          this._visualHat = 0;
          this.visualHatAsset = (ItemHatAsset) null;
        }
        else
          this.visualHatAsset = (ItemHatAsset) null;
      }
    }

    public int visualBackpack
    {
      get
      {
        return this._visualBackpack;
      }
      set
      {
        if (this.visualBackpack == value)
          return;
        this._visualBackpack = value;
        if (Dedicator.isDedicated)
          return;
        if (this.visualBackpack != 0)
        {
          try
          {
            this.visualBackpackAsset = (ItemBackpackAsset) Assets.find(EAssetType.ITEM, Provider.provider.economyService.getInventoryItemID(this.visualBackpack));
          }
          catch
          {
            this.visualBackpackAsset = (ItemBackpackAsset) null;
          }
          if (this.visualBackpackAsset == null || this.visualBackpackAsset.isPro)
            return;
          this._visualBackpack = 0;
          this.visualBackpackAsset = (ItemBackpackAsset) null;
        }
        else
          this.visualBackpackAsset = (ItemBackpackAsset) null;
      }
    }

    public int visualVest
    {
      get
      {
        return this._visualVest;
      }
      set
      {
        if (this.visualVest == value)
          return;
        this._visualVest = value;
        if (Dedicator.isDedicated)
          return;
        if (this.visualVest != 0)
        {
          try
          {
            this.visualVestAsset = (ItemVestAsset) Assets.find(EAssetType.ITEM, Provider.provider.economyService.getInventoryItemID(this.visualVest));
          }
          catch
          {
            this.visualVestAsset = (ItemVestAsset) null;
          }
          if (this.visualVestAsset == null || this.visualVestAsset.isPro)
            return;
          this._visualVest = 0;
          this.visualVestAsset = (ItemVestAsset) null;
        }
        else
          this.visualVestAsset = (ItemVestAsset) null;
      }
    }

    public int visualMask
    {
      get
      {
        return this._visualMask;
      }
      set
      {
        if (this.visualMask == value)
          return;
        this._visualMask = value;
        if (Dedicator.isDedicated)
          return;
        if (this.visualMask != 0)
        {
          try
          {
            this.visualMaskAsset = (ItemMaskAsset) Assets.find(EAssetType.ITEM, Provider.provider.economyService.getInventoryItemID(this.visualMask));
          }
          catch
          {
            this.visualMaskAsset = (ItemMaskAsset) null;
          }
          if (this.visualMaskAsset == null || this.visualMaskAsset.isPro)
            return;
          this._visualMask = 0;
          this.visualMaskAsset = (ItemMaskAsset) null;
        }
        else
          this.visualMaskAsset = (ItemMaskAsset) null;
      }
    }

    public int visualGlasses
    {
      get
      {
        return this._visualGlasses;
      }
      set
      {
        if (this.visualGlasses == value)
          return;
        this._visualGlasses = value;
        if (Dedicator.isDedicated)
          return;
        if (this.visualGlasses != 0)
        {
          try
          {
            this.visualGlassesAsset = (ItemGlassesAsset) Assets.find(EAssetType.ITEM, Provider.provider.economyService.getInventoryItemID(this.visualGlasses));
          }
          catch
          {
            this.visualGlassesAsset = (ItemGlassesAsset) null;
          }
          if (this.visualGlassesAsset == null || this.visualGlassesAsset.isPro)
            return;
          this._visualGlasses = 0;
          this.visualGlassesAsset = (ItemGlassesAsset) null;
        }
        else
          this.visualGlassesAsset = (ItemGlassesAsset) null;
      }
    }

    public ItemShirtAsset shirtAsset
    {
      get
      {
        return this._shirtAsset;
      }
    }

    public ItemPantsAsset pantsAsset
    {
      get
      {
        return this._pantsAsset;
      }
    }

    public ItemHatAsset hatAsset
    {
      get
      {
        return this._hatAsset;
      }
    }

    public ItemBackpackAsset backpackAsset
    {
      get
      {
        return this._backpackAsset;
      }
    }

    public ItemVestAsset vestAsset
    {
      get
      {
        return this._vestAsset;
      }
    }

    public ItemMaskAsset maskAsset
    {
      get
      {
        return this._maskAsset;
      }
    }

    public ItemGlassesAsset glassesAsset
    {
      get
      {
        return this._glassesAsset;
      }
    }

    public ushort shirt
    {
      get
      {
        return this._shirt;
      }
      set
      {
        if ((int) this.shirt == (int) value)
          return;
        this._shirt = value;
        if ((int) this.shirt != 0)
        {
          try
          {
            this._shirtAsset = (ItemShirtAsset) Assets.find(EAssetType.ITEM, this.shirt);
          }
          catch
          {
            this._shirtAsset = (ItemShirtAsset) null;
          }
          if (this.shirtAsset == null || !this.shirtAsset.isPro)
            return;
          this._shirt = (ushort) 0;
          this._shirtAsset = (ItemShirtAsset) null;
        }
        else
          this._shirtAsset = (ItemShirtAsset) null;
      }
    }

    public ushort pants
    {
      get
      {
        return this._pants;
      }
      set
      {
        if ((int) this.pants == (int) value)
          return;
        this._pants = value;
        if ((int) this.pants != 0)
        {
          try
          {
            this._pantsAsset = (ItemPantsAsset) Assets.find(EAssetType.ITEM, this.pants);
          }
          catch
          {
            this._pantsAsset = (ItemPantsAsset) null;
          }
          if (this.pantsAsset == null || !this.pantsAsset.isPro)
            return;
          this._pants = (ushort) 0;
          this._pantsAsset = (ItemPantsAsset) null;
        }
        else
          this._pantsAsset = (ItemPantsAsset) null;
      }
    }

    public ushort hat
    {
      get
      {
        return this._hat;
      }
      set
      {
        if ((int) this.hat == (int) value)
          return;
        this._hat = value;
        if ((int) this.hat != 0)
        {
          try
          {
            this._hatAsset = (ItemHatAsset) Assets.find(EAssetType.ITEM, this.hat);
          }
          catch
          {
            this._hatAsset = (ItemHatAsset) null;
          }
          if (this.hatAsset == null || !this.hatAsset.isPro)
            return;
          this._hat = (ushort) 0;
          this._hatAsset = (ItemHatAsset) null;
        }
        else
          this._hatAsset = (ItemHatAsset) null;
      }
    }

    public ushort backpack
    {
      get
      {
        return this._backpack;
      }
      set
      {
        if ((int) this.backpack == (int) value)
          return;
        this._backpack = value;
        if ((int) this.backpack != 0)
        {
          try
          {
            this._backpackAsset = (ItemBackpackAsset) Assets.find(EAssetType.ITEM, this.backpack);
          }
          catch
          {
            this._backpackAsset = (ItemBackpackAsset) null;
          }
          if (this.backpackAsset == null || !this.backpackAsset.isPro)
            return;
          this._backpack = (ushort) 0;
          this._backpackAsset = (ItemBackpackAsset) null;
        }
        else
          this._backpackAsset = (ItemBackpackAsset) null;
      }
    }

    public ushort vest
    {
      get
      {
        return this._vest;
      }
      set
      {
        if ((int) this.vest == (int) value)
          return;
        this._vest = value;
        if ((int) this.vest != 0)
        {
          try
          {
            this._vestAsset = (ItemVestAsset) Assets.find(EAssetType.ITEM, this.vest);
          }
          catch
          {
            this._vestAsset = (ItemVestAsset) null;
          }
          if (this.vestAsset == null || !this.vestAsset.isPro)
            return;
          this._vest = (ushort) 0;
          this._vestAsset = (ItemVestAsset) null;
        }
        else
          this._vestAsset = (ItemVestAsset) null;
      }
    }

    public ushort mask
    {
      get
      {
        return this._mask;
      }
      set
      {
        if ((int) this.mask == (int) value)
          return;
        this._mask = value;
        if ((int) this.mask != 0)
        {
          try
          {
            this._maskAsset = (ItemMaskAsset) Assets.find(EAssetType.ITEM, this.mask);
          }
          catch
          {
            this._maskAsset = (ItemMaskAsset) null;
          }
          if (this.maskAsset == null || !this.maskAsset.isPro)
            return;
          this._mask = (ushort) 0;
          this._maskAsset = (ItemMaskAsset) null;
        }
        else
          this._maskAsset = (ItemMaskAsset) null;
      }
    }

    public ushort glasses
    {
      get
      {
        return this._glasses;
      }
      set
      {
        if ((int) this.glasses == (int) value)
          return;
        this._glasses = value;
        if ((int) this.glasses != 0)
        {
          try
          {
            this._glassesAsset = (ItemGlassesAsset) Assets.find(EAssetType.ITEM, this.glasses);
          }
          catch
          {
            this._glassesAsset = (ItemGlassesAsset) null;
          }
          if (this.glassesAsset == null || !this.glassesAsset.isPro)
            return;
          this._glasses = (ushort) 0;
          this._glassesAsset = (ItemGlassesAsset) null;
        }
        else
          this._glassesAsset = (ItemGlassesAsset) null;
      }
    }

    public byte face
    {
      get
      {
        return this._face;
      }
      set
      {
        this._face = value;
        if (Dedicator.isDedicated)
          return;
        this.clothing.face = (Texture2D) Resources.Load("Faces/" + (object) this.face + "/Texture");
        this.clothing.faceEmission = (Texture2D) Resources.Load("Faces/" + (object) this.face + "/Emission");
        this.clothing.faceMetallic = (Texture2D) Resources.Load("Faces/" + (object) this.face + "/Metallic");
      }
    }

    public Color skin
    {
      get
      {
        return this.clothing.skin;
      }
      set
      {
        this.clothing.skin = value;
      }
    }

    public Color color
    {
      get
      {
        return this._color;
      }
      set
      {
        this._color = value;
      }
    }

    public void apply()
    {
      if (Dedicator.isDedicated)
        return;
      ItemShirtAsset itemShirtAsset = this.visualShirtAsset == null || !this.isVisual ? this.shirtAsset : this.visualShirtAsset;
      ItemPantsAsset itemPantsAsset = this.visualPantsAsset == null || !this.isVisual ? this.pantsAsset : this.visualPantsAsset;
      this.clothing.shirt = (Texture2D) null;
      this.clothing.shirtEmission = (Texture2D) null;
      this.clothing.shirtMetallic = (Texture2D) null;
      if (itemShirtAsset != null)
      {
        this.clothing.shirt = itemShirtAsset.shirt;
        this.clothing.shirtEmission = itemShirtAsset.emission;
        this.clothing.shirtMetallic = itemShirtAsset.metallic;
      }
      this.clothing.pants = (Texture2D) null;
      this.clothing.pantsEmission = (Texture2D) null;
      this.clothing.pantsMetallic = (Texture2D) null;
      if (itemPantsAsset != null)
      {
        this.clothing.pants = itemPantsAsset.pants;
        this.clothing.pantsEmission = itemPantsAsset.emission;
        this.clothing.pantsMetallic = itemPantsAsset.metallic;
      }
      this.clothing.apply();
      if ((Object) this.material_0 != (Object) null)
      {
        this.material_0.mainTexture = (Texture) this.clothing.texture;
        this.material_0.SetTexture("_EmissionMap", (Texture) this.clothing.emission);
        this.material_0.SetTexture("_MetallicGlossMap", (Texture) this.clothing.metallic);
      }
      if ((Object) this.material_1 != (Object) null)
      {
        this.material_1.mainTexture = (Texture) this.clothing.texture;
        this.material_1.SetTexture("_EmissionMap", (Texture) this.clothing.emission);
        this.material_1.SetTexture("_MetallicGlossMap", (Texture) this.clothing.metallic);
      }
      if (this.isMine)
        return;
      if (this.isUpper && this.upperSystems != null)
      {
        for (int index = 0; index < this.upperSystems.Length; ++index)
        {
          Transform transform = this.upperSystems[index];
          if ((Object) transform != (Object) null)
            Object.Destroy((Object) transform.gameObject);
        }
        this.isUpper = false;
      }
      if (this.isVisual && this.visualShirt != 0)
      {
        ushort inventoryMythicId = Provider.provider.economyService.getInventoryMythicID(this.visualShirt);
        if ((int) inventoryMythicId != 0)
        {
          ItemTool.applyEffect(this.upperBones, this.upperSystems, inventoryMythicId, EEffectType.AREA);
          this.isUpper = true;
        }
      }
      if (this.isLower && this.lowerSystems != null)
      {
        for (int index = 0; index < this.lowerSystems.Length; ++index)
        {
          Transform transform = this.lowerSystems[index];
          if ((Object) transform != (Object) null)
            Object.Destroy((Object) transform.gameObject);
        }
        this.isLower = false;
      }
      if (this.isVisual && this.visualPants != 0)
      {
        ushort inventoryMythicId = Provider.provider.economyService.getInventoryMythicID(this.visualPants);
        if ((int) inventoryMythicId != 0)
        {
          ItemTool.applyEffect(this.lowerBones, this.lowerSystems, inventoryMythicId, EEffectType.AREA);
          this.isLower = true;
        }
      }
      ItemHatAsset itemHatAsset = this.visualHatAsset == null || !this.isVisual ? this.hatAsset : this.visualHatAsset;
      ItemBackpackAsset itemBackpackAsset = this.visualBackpackAsset == null || !this.isVisual ? this.backpackAsset : this.visualBackpackAsset;
      ItemVestAsset itemVestAsset = this.visualVestAsset == null || !this.isVisual ? this.vestAsset : this.visualVestAsset;
      ItemMaskAsset itemMaskAsset = this.visualMaskAsset == null || !this.isVisual ? this.maskAsset : this.visualMaskAsset;
      ItemGlassesAsset itemGlassesAsset = this.visualGlassesAsset == null || !this.isVisual ? this.glassesAsset : this.visualGlassesAsset;
      bool flag1 = true;
      bool flag2 = true;
      if ((Object) this.hatModel != (Object) null)
        Object.Destroy((Object) this.hatModel.gameObject);
      if (itemHatAsset != null && (Object) itemHatAsset.hat != (Object) null)
      {
        this.hatModel = Object.Instantiate<GameObject>(itemHatAsset.hat).transform;
        this.hatModel.name = "Hat";
        this.hatModel.transform.parent = this.skull;
        this.hatModel.transform.localPosition = Vector3.zero;
        this.hatModel.transform.localRotation = Quaternion.identity;
        this.hatModel.transform.localScale = Vector3.one;
        if (!this.isView)
          Object.Destroy((Object) this.hatModel.GetComponent<Collider>());
        if (!itemHatAsset.hasHair)
          flag1 = false;
        if (!itemHatAsset.hasBeard)
          flag2 = false;
        if (this.isVisual && this.visualHat != 0)
        {
          ushort inventoryMythicId = Provider.provider.economyService.getInventoryMythicID(this.visualHat);
          if ((int) inventoryMythicId != 0)
            ItemTool.applyEffect(this.hatModel, inventoryMythicId, EEffectType.HOOK);
        }
      }
      if ((Object) this.backpackModel != (Object) null)
        Object.Destroy((Object) this.backpackModel.gameObject);
      if (itemBackpackAsset != null && (Object) itemBackpackAsset.backpack != (Object) null)
      {
        this.backpackModel = Object.Instantiate<GameObject>(itemBackpackAsset.backpack).transform;
        this.backpackModel.name = "Backpack";
        this.backpackModel.transform.parent = this.spine;
        this.backpackModel.transform.localPosition = Vector3.zero;
        this.backpackModel.transform.localRotation = Quaternion.identity;
        this.backpackModel.transform.localScale = Vector3.one;
        if (!this.isView)
          Object.Destroy((Object) this.backpackModel.GetComponent<Collider>());
        if (this.isVisual && this.visualBackpack != 0)
        {
          ushort inventoryMythicId = Provider.provider.economyService.getInventoryMythicID(this.visualBackpack);
          if ((int) inventoryMythicId != 0)
            ItemTool.applyEffect(this.backpackModel, inventoryMythicId, EEffectType.HOOK);
        }
      }
      if ((Object) this.vestModel != (Object) null)
        Object.Destroy((Object) this.vestModel.gameObject);
      if (itemVestAsset != null && (Object) itemVestAsset.vest != (Object) null)
      {
        this.vestModel = Object.Instantiate<GameObject>(itemVestAsset.vest).transform;
        this.vestModel.name = "Vest";
        this.vestModel.transform.parent = this.spine;
        this.vestModel.transform.localPosition = Vector3.zero;
        this.vestModel.transform.localRotation = Quaternion.identity;
        this.vestModel.transform.localScale = Vector3.one;
        if (!this.isView)
          Object.Destroy((Object) this.vestModel.GetComponent<Collider>());
        if (this.isVisual && this.visualVest != 0)
        {
          ushort inventoryMythicId = Provider.provider.economyService.getInventoryMythicID(this.visualVest);
          if ((int) inventoryMythicId != 0)
            ItemTool.applyEffect(this.vestModel, inventoryMythicId, EEffectType.HOOK);
        }
      }
      if ((Object) this.maskModel != (Object) null)
        Object.Destroy((Object) this.maskModel.gameObject);
      if (itemMaskAsset != null && (Object) itemMaskAsset.mask != (Object) null)
      {
        this.maskModel = Object.Instantiate<GameObject>(itemMaskAsset.mask).transform;
        this.maskModel.name = "Mask";
        this.maskModel.transform.parent = this.skull;
        this.maskModel.transform.localPosition = Vector3.zero;
        this.maskModel.transform.localRotation = Quaternion.identity;
        this.maskModel.transform.localScale = Vector3.one;
        if (!this.isView)
          Object.Destroy((Object) this.maskModel.GetComponent<Collider>());
        if (!itemMaskAsset.hasHair)
          flag1 = false;
        if (!itemMaskAsset.hasBeard)
          flag2 = false;
        if (this.isVisual && this.visualMask != 0)
        {
          ushort inventoryMythicId = Provider.provider.economyService.getInventoryMythicID(this.visualMask);
          if ((int) inventoryMythicId != 0)
            ItemTool.applyEffect(this.maskModel, inventoryMythicId, EEffectType.HOOK);
        }
      }
      if ((Object) this.glassesModel != (Object) null)
        Object.Destroy((Object) this.glassesModel.gameObject);
      if (itemGlassesAsset != null && (Object) itemGlassesAsset.glasses != (Object) null)
      {
        this.glassesModel = Object.Instantiate<GameObject>(itemGlassesAsset.glasses).transform;
        this.glassesModel.name = "Glasses";
        this.glassesModel.transform.parent = this.skull;
        this.glassesModel.transform.localPosition = Vector3.zero;
        this.glassesModel.transform.localRotation = Quaternion.identity;
        this.glassesModel.localScale = Vector3.one;
        if (!this.isView)
          Object.Destroy((Object) this.glassesModel.GetComponent<Collider>());
        if (!itemGlassesAsset.hasHair)
          flag1 = false;
        if (!itemGlassesAsset.hasBeard)
          flag2 = false;
        if (this.isVisual && this.visualGlasses != 0)
        {
          ushort inventoryMythicId = Provider.provider.economyService.getInventoryMythicID(this.visualGlasses);
          if ((int) inventoryMythicId != 0)
            ItemTool.applyEffect(this.glassesModel, inventoryMythicId, EEffectType.HOOK);
        }
      }
      if ((Object) this.hairModel != (Object) null)
        Object.Destroy((Object) this.hairModel.gameObject);
      if (flag1)
      {
        Object original = Resources.Load("Hairs/" + (object) this.hair + "/Hair");
        if (original != (Object) null)
        {
          this.hairModel = ((GameObject) Object.Instantiate(original)).transform;
          this.hairModel.name = "Hair";
          this.hairModel.transform.parent = this.skull;
          this.hairModel.transform.localPosition = Vector3.zero;
          this.hairModel.transform.localRotation = Quaternion.identity;
          this.hairModel.transform.localScale = Vector3.one;
          if ((Object) this.hairModel.FindChild("Model_0") != (Object) null)
            this.hairModel.FindChild("Model_0").GetComponent<Renderer>().material.color = this.color;
        }
      }
      if ((Object) this.beardModel != (Object) null)
        Object.Destroy((Object) this.beardModel.gameObject);
      if (!flag2)
        return;
      Object original1 = Resources.Load("Beards/" + (object) this.beard + "/Beard");
      if (!(original1 != (Object) null))
        return;
      this.beardModel = ((GameObject) Object.Instantiate(original1)).transform;
      this.beardModel.name = "Beard";
      this.beardModel.transform.parent = this.skull;
      this.beardModel.transform.localPosition = Vector3.zero;
      this.beardModel.transform.localRotation = Quaternion.identity;
      this.beardModel.localScale = Vector3.one;
      if (!((Object) this.beardModel.FindChild("Model_0") != (Object) null))
        return;
      this.beardModel.FindChild("Model_0").GetComponent<Renderer>().material.color = this.color;
    }

    private void Awake()
    {
      this.spine = this.transform.FindChild("Skeleton").FindChild("Spine");
      this.skull = this.spine.FindChild("Skull");
      this.upperBones = new Transform[5]
      {
        this.spine,
        this.spine.FindChild("Left_Shoulder/Left_Arm"),
        this.spine.FindChild("Left_Shoulder/Left_Arm/Left_Hand"),
        this.spine.FindChild("Right_Shoulder/Right_Arm"),
        this.spine.FindChild("Right_Shoulder/Right_Arm/Right_Hand")
      };
      this.upperSystems = new Transform[this.upperBones.Length];
      this.lowerBones = new Transform[4]
      {
        this.spine.parent.FindChild("Left_Hip/Left_Leg"),
        this.spine.parent.FindChild("Left_Hip/Left_Leg/Left_Foot"),
        this.spine.parent.FindChild("Right_Hip/Right_Leg"),
        this.spine.parent.FindChild("Right_Hip/Right_Leg/Right_Foot")
      };
      this.lowerBones = new Transform[this.lowerBones.Length];
      if ((Object) this.transform.FindChild("Model_0") != (Object) null)
        this.material_0 = this.transform.FindChild("Model_0").GetComponent<Renderer>().material;
      if ((Object) this.transform.FindChild("Model_1") != (Object) null)
        this.material_1 = this.transform.FindChild("Model_1").GetComponent<Renderer>().material;
      this.clothing = new HumanClothing();
      this.isVisual = true;
      this.isUpper = false;
      this.isLower = false;
    }
  }
}
