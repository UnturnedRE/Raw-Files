// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ItemAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

namespace SDG.Unturned
{
  public class ItemAsset : Asset
  {
    protected string _itemName;
    protected string _itemDescription;
    protected EItemType _type;
    protected string _proPath;
    protected bool _isPro;
    protected EUseableType _useable;
    protected ESlotType _slot;
    protected byte _size_x;
    protected byte _size_y;
    protected float _size_z;
    protected float _size2_z;
    private byte _amount;
    private byte countMin;
    private byte countMax;
    private byte qualityMin;
    private byte qualityMax;
    protected bool _isBackward;
    protected GameObject _item;
    protected AudioClip _equip;
    protected AnimationClip[] _animations;
    protected Blueprint[] _blueprints;

    public string itemName
    {
      get
      {
        return this._itemName;
      }
    }

    public string itemDescription
    {
      get
      {
        return this._itemDescription;
      }
    }

    public EItemType type
    {
      get
      {
        return this._type;
      }
    }

    public string proPath
    {
      get
      {
        return this._proPath;
      }
    }

    public bool isPro
    {
      get
      {
        return this._isPro;
      }
    }

    public EUseableType useable
    {
      get
      {
        return this._useable;
      }
    }

    public ESlotType slot
    {
      get
      {
        return this._slot;
      }
    }

    public byte size_x
    {
      get
      {
        return this._size_x;
      }
    }

    public byte size_y
    {
      get
      {
        return this._size_y;
      }
    }

    public float size_z
    {
      get
      {
        return this._size_z;
      }
    }

    public float size2_z
    {
      get
      {
        return this._size2_z;
      }
    }

    public byte amount
    {
      get
      {
        return this._amount;
      }
    }

    public byte count
    {
      get
      {
        if ((double) UnityEngine.Random.value > 0.9)
          return this.amount;
        return (byte) UnityEngine.Random.Range((int) this.countMin, (int) this.countMax + 1);
      }
    }

    public byte quality
    {
      get
      {
        if ((double) UnityEngine.Random.value > 0.9)
          return (byte) 100;
        return (byte) UnityEngine.Random.Range((int) this.qualityMin, (int) this.qualityMax + 1);
      }
    }

    public bool isBackward
    {
      get
      {
        return this._isBackward;
      }
    }

    public GameObject item
    {
      get
      {
        return this._item;
      }
    }

    public AudioClip equip
    {
      get
      {
        return this._equip;
      }
    }

    public AnimationClip[] animations
    {
      get
      {
        return this._animations;
      }
    }

    public Blueprint[] blueprints
    {
      get
      {
        return this._blueprints;
      }
    }

    public virtual bool showQuality
    {
      get
      {
        return false;
      }
    }

    public ItemAsset(Bundle bundle, Data data, ushort id)
      : base(bundle, id)
    {
      if ((int) id < 2000 && !bundle.hasResource)
        throw new NotSupportedException();
      this._isPro = data.has("Pro");
      if (!this.isPro)
      {
        Local local = Localization.tryRead(bundle.path, bundle.usePath);
        this._itemName = local.format("Name");
        this._itemDescription = local.format("Description");
      }
      this._type = (EItemType) Enum.Parse(typeof (EItemType), data.readString("Type"), true);
      if (this.isPro)
      {
        if (this.type == EItemType.SHIRT)
          this._proPath = "/Shirts";
        else if (this.type == EItemType.PANTS)
          this._proPath = "/Pants";
        else if (this.type == EItemType.HAT)
          this._proPath = "/Hats";
        else if (this.type == EItemType.BACKPACK)
          this._proPath = "/Backpacks";
        else if (this.type == EItemType.VEST)
          this._proPath = "/Vests";
        else if (this.type == EItemType.MASK)
          this._proPath = "/Masks";
        else if (this.type == EItemType.GLASSES)
          this._proPath = "/Glasses";
        else if (this.type == EItemType.KEY)
          this._proPath = "/Keys";
        else if (this.type == EItemType.BOX)
          this._proPath = "/Boxes";
        ItemAsset itemAsset = this;
        string str = itemAsset._proPath + "/" + this.name;
        itemAsset._proPath = str;
      }
      this._size_x = data.readByte("Size_X");
      if ((int) this.size_x < 1)
        this._size_x = (byte) 1;
      this._size_y = data.readByte("Size_Y");
      if ((int) this.size_y < 1)
        this._size_y = (byte) 1;
      this._size_z = data.readSingle("Size_Z");
      this._size2_z = data.readSingle("Size2_Z");
      this._amount = data.readByte("Amount");
      if ((int) this.amount < 1)
        this._amount = (byte) 1;
      this.countMin = data.readByte("Count_Min");
      if ((int) this.countMin < 1)
        this.countMin = (byte) 1;
      this.countMax = data.readByte("Count_Max");
      if ((int) this.countMax < 1)
        this.countMax = (byte) 1;
      this.qualityMin = !data.has("Quality_Min") ? (byte) 10 : data.readByte("Quality_Min");
      this.qualityMax = !data.has("Quality_Max") ? (byte) 90 : data.readByte("Quality_Max");
      this._isBackward = data.has("Backward");
      string str1 = data.readString("Useable");
      if (str1 == null)
      {
        this._useable = EUseableType.NONE;
      }
      else
      {
        this._useable = (EUseableType) Enum.Parse(typeof (EUseableType), str1, true);
        this._equip = (AudioClip) bundle.load("Equip");
        if (!this.isPro)
        {
          Animation component = ((GameObject) bundle.load("Animations")).GetComponent<Animation>();
          this._animations = new AnimationClip[component.GetClipCount()];
          int index = 0;
          foreach (AnimationState animationState in component)
          {
            this.animations[index] = animationState.clip;
            ++index;
          }
        }
      }
      string str2 = data.readString("Slot");
      this._slot = str2 != null ? (ESlotType) Enum.Parse(typeof (ESlotType), str2, true) : ESlotType.NONE;
      this._item = (GameObject) bundle.load("Item");
      if ((UnityEngine.Object) this.item == (UnityEngine.Object) null)
        throw new NotSupportedException();
      this._blueprints = new Blueprint[(int) data.readByte("Blueprints")];
      for (byte newID1 = (byte) 0; (int) newID1 < this.blueprints.Length; ++newID1)
      {
        EBlueprintType newType = (EBlueprintType) Enum.Parse(typeof (EBlueprintType), data.readString("Blueprint_" + (object) newID1 + "_Type"), true);
        byte num = data.readByte("Blueprint_" + (object) newID1 + "_Supplies");
        if ((int) num < 1)
          num = (byte) 1;
        BlueprintSupply[] newSupplies = new BlueprintSupply[(int) num];
        for (byte index = (byte) 0; (int) index < newSupplies.Length; ++index)
        {
          ushort newID2 = data.readUInt16("Blueprint_" + (object) newID1 + "_Supply_" + (string) (object) index + "_ID");
          byte newAmount = data.readByte("Blueprint_" + (object) newID1 + "_Supply_" + (string) (object) index + "_Amount");
          if ((int) newAmount < 1)
            newAmount = (byte) 1;
          newSupplies[(int) index] = new BlueprintSupply(newID2, newAmount);
        }
        ushort newTool = data.readUInt16("Blueprint_" + (object) newID1 + "_Tool");
        ushort newProduct = data.readUInt16("Blueprint_" + (object) newID1 + "_Product");
        if ((int) newProduct == 0)
          newProduct = id;
        ushort newProducts = data.readUInt16("Blueprint_" + (object) newID1 + "_Products");
        if ((int) newProducts < 1)
          newProducts = (ushort) 1;
        ushort newBuild = data.readUInt16("Blueprint_" + (object) newID1 + "_Build");
        byte newLevel = data.readByte("Blueprint_" + (object) newID1 + "_Level");
        EBlueprintSkill newSkill = EBlueprintSkill.NONE;
        if ((int) newLevel > 0)
          newSkill = (EBlueprintSkill) Enum.Parse(typeof (EBlueprintSkill), data.readString("Blueprint_" + (object) newID1 + "_Skill"), true);
        this._blueprints[(int) newID1] = new Blueprint(id, newID1, newType, newSupplies, newTool, newProduct, newProducts, newBuild, newLevel, newSkill);
      }
    }

    public byte[] getState()
    {
      return this.getState(false);
    }

    public virtual byte[] getState(bool isFull)
    {
      return new byte[0];
    }
  }
}
