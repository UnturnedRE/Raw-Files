// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.SteamPending
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
  public class SteamPending
  {
    public SteamInventoryResult_t inventoryResult = SteamInventoryResult_t.Invalid;
    private SteamPlayerID _playerID;
    private bool _isPro;
    private byte _face;
    private byte _hair;
    private byte _beard;
    private Color _skin;
    private Color _color;
    private bool _hand;
    public int shirtItem;
    public int pantsItem;
    public int hatItem;
    public int backpackItem;
    public int vestItem;
    public int maskItem;
    public int glassesItem;
    public int[] skinItems;
    public ulong packageShirt;
    public ulong packagePants;
    public ulong packageHat;
    public ulong packageBackpack;
    public ulong packageVest;
    public ulong packageMask;
    public ulong packageGlasses;
    public ulong[] packageSkins;
    public SteamItemDetails_t[] inventoryDetails;
    public bool assignedPro;
    public bool assignedAdmin;
    public bool hasAuthentication;
    public bool hasProof;
    private EPlayerSpeciality _speciality;
    private float _joined;

    public SteamPlayerID playerID
    {
      get
      {
        return this._playerID;
      }
    }

    public bool isPro
    {
      get
      {
        return this._isPro;
      }
    }

    public byte face
    {
      get
      {
        return this._face;
      }
    }

    public byte hair
    {
      get
      {
        return this._hair;
      }
    }

    public byte beard
    {
      get
      {
        return this._beard;
      }
    }

    public Color skin
    {
      get
      {
        return this._skin;
      }
    }

    public Color color
    {
      get
      {
        return this._color;
      }
    }

    public bool hand
    {
      get
      {
        return this._hand;
      }
    }

    public EPlayerSpeciality speciality
    {
      get
      {
        return this._speciality;
      }
    }

    public float joined
    {
      get
      {
        return this._joined;
      }
    }

    public SteamPending(SteamPlayerID newPlayerID, bool newPro, byte newFace, byte newHair, byte newBeard, Color newSkin, Color newColor, bool newHand, ulong newPackageShirt, ulong newPackagePants, ulong newPackageHat, ulong newPackageBackpack, ulong newPackageVest, ulong newPackageMask, ulong newPackageGlasses, ulong[] newPackageSkins, EPlayerSpeciality newSpeciality)
    {
      this._playerID = newPlayerID;
      this._isPro = newPro;
      this._face = newFace;
      this._hair = newHair;
      this._beard = newBeard;
      this._skin = newSkin;
      this._color = newColor;
      this._hand = newHand;
      this._speciality = newSpeciality;
      this.packageShirt = newPackageShirt;
      this.packagePants = newPackagePants;
      this.packageHat = newPackageHat;
      this.packageBackpack = newPackageBackpack;
      this.packageVest = newPackageVest;
      this.packageMask = newPackageMask;
      this.packageGlasses = newPackageGlasses;
      this.packageSkins = newPackageSkins;
      this._joined = Time.realtimeSinceStartup;
    }

    public int getInventoryItem(ulong package)
    {
      if (this.inventoryDetails != null)
      {
        for (int index = 0; index < this.inventoryDetails.Length; ++index)
        {
          if ((long) this.inventoryDetails[index].m_itemId.m_SteamItemInstanceID == (long) package)
            return this.inventoryDetails[index].m_iDefinition.m_SteamItemDef;
        }
      }
      return 0;
    }
  }
}
