// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.Attachments
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

namespace SDG.Unturned
{
  public class Attachments : MonoBehaviour
  {
    private ItemGunAsset _gunAsset;
    private SkinAsset _skinAsset;
    private ushort _sightID;
    private ushort _tacticalID;
    private ushort _gripID;
    private ushort _barrelID;
    private ushort _magazineID;
    private ItemSightAsset _sightAsset;
    private ItemTacticalAsset _tacticalAsset;
    private ItemGripAsset _gripAsset;
    private ItemBarrelAsset _barrelAsset;
    private ItemMagazineAsset _magazineAsset;
    private Transform _sightModel;
    private Transform _tacticalModel;
    private Transform _gripModel;
    private Transform _barrelModel;
    private Transform _magazineModel;
    private Transform _sightHook;
    private Transform _tacticalHook;
    private Transform _gripHook;
    private Transform _barrelHook;
    private Transform _magazineHook;
    private Transform _ejectHook;
    private Transform _lightHook;
    private Transform _light2Hook;
    private Transform _aimHook;
    private Transform _scopeHook;
    private Transform _leftHook;
    private Transform _rightHook;
    private Transform _nockHook;
    private Transform _restHook;
    private LineRenderer _rope;

    public ItemGunAsset gunAsset
    {
      get
      {
        return this._gunAsset;
      }
    }

    public SkinAsset skinAsset
    {
      get
      {
        return this._skinAsset;
      }
    }

    public ushort sightID
    {
      get
      {
        return this._sightID;
      }
    }

    public ushort tacticalID
    {
      get
      {
        return this._tacticalID;
      }
    }

    public ushort gripID
    {
      get
      {
        return this._gripID;
      }
    }

    public ushort barrelID
    {
      get
      {
        return this._barrelID;
      }
    }

    public ushort magazineID
    {
      get
      {
        return this._magazineID;
      }
    }

    public ItemSightAsset sightAsset
    {
      get
      {
        return this._sightAsset;
      }
    }

    public ItemTacticalAsset tacticalAsset
    {
      get
      {
        return this._tacticalAsset;
      }
    }

    public ItemGripAsset gripAsset
    {
      get
      {
        return this._gripAsset;
      }
    }

    public ItemBarrelAsset barrelAsset
    {
      get
      {
        return this._barrelAsset;
      }
    }

    public ItemMagazineAsset magazineAsset
    {
      get
      {
        return this._magazineAsset;
      }
    }

    public Transform sightModel
    {
      get
      {
        return this._sightModel;
      }
    }

    public Transform tacticalModel
    {
      get
      {
        return this._tacticalModel;
      }
    }

    public Transform gripModel
    {
      get
      {
        return this._gripModel;
      }
    }

    public Transform barrelModel
    {
      get
      {
        return this._barrelModel;
      }
    }

    public Transform magazineModel
    {
      get
      {
        return this._magazineModel;
      }
    }

    public Transform sightHook
    {
      get
      {
        return this._sightHook;
      }
    }

    public Transform tacticalHook
    {
      get
      {
        return this._tacticalHook;
      }
    }

    public Transform gripHook
    {
      get
      {
        return this._gripHook;
      }
    }

    public Transform barrelHook
    {
      get
      {
        return this._barrelHook;
      }
    }

    public Transform magazineHook
    {
      get
      {
        return this._magazineHook;
      }
    }

    public Transform ejectHook
    {
      get
      {
        return this._ejectHook;
      }
    }

    public Transform lightHook
    {
      get
      {
        return this._lightHook;
      }
    }

    public Transform light2Hook
    {
      get
      {
        return this._light2Hook;
      }
    }

    public Transform aimHook
    {
      get
      {
        return this._aimHook;
      }
    }

    public Transform scopeHook
    {
      get
      {
        return this._scopeHook;
      }
    }

    public Transform leftHook
    {
      get
      {
        return this._leftHook;
      }
    }

    public Transform rightHook
    {
      get
      {
        return this._rightHook;
      }
    }

    public Transform nockHook
    {
      get
      {
        return this._nockHook;
      }
    }

    public Transform restHook
    {
      get
      {
        return this._restHook;
      }
    }

    public LineRenderer rope
    {
      get
      {
        return this._rope;
      }
    }

    public void updateGun(ItemGunAsset newGunAsset, SkinAsset newSkinAsset)
    {
      this._gunAsset = newGunAsset;
      this._skinAsset = newSkinAsset;
    }

    public void updateAttachments(byte[] state, bool viewmodel)
    {
      if (state.Length != 18)
        return;
      this.transform.localScale = Vector3.one;
      this._sightID = BitConverter.ToUInt16(state, 0);
      this._tacticalID = BitConverter.ToUInt16(state, 2);
      this._gripID = BitConverter.ToUInt16(state, 4);
      this._barrelID = BitConverter.ToUInt16(state, 6);
      this._magazineID = BitConverter.ToUInt16(state, 8);
      if ((UnityEngine.Object) this.sightModel != (UnityEngine.Object) null)
      {
        UnityEngine.Object.Destroy((UnityEngine.Object) this.sightModel.gameObject);
        this._sightModel = (Transform) null;
      }
      if ((UnityEngine.Object) this.sightHook != (UnityEngine.Object) null)
      {
        if ((int) this.sightID != 0)
        {
          try
          {
            this._sightAsset = (ItemSightAsset) Assets.find(EAssetType.ITEM, this.sightID);
          }
          catch
          {
            this._sightAsset = (ItemSightAsset) null;
          }
          if (this.sightAsset != null)
          {
            this._sightModel = UnityEngine.Object.Instantiate<GameObject>(this.sightAsset.sight).transform;
            this.sightModel.name = "Sight";
            this.sightModel.transform.parent = this.sightHook;
            this.sightModel.transform.localPosition = Vector3.zero;
            this.sightModel.transform.localRotation = Quaternion.identity;
            this.sightModel.localScale = Vector3.one;
            if (viewmodel)
              Layerer.viewmodel(this.sightModel);
            if (this.gunAsset != null && this.skinAsset != null)
            {
              Material skin = (Material) null;
              if (!this.skinAsset.secondarySkins.TryGetValue(this.sightID, out skin) && this.skinAsset.hasSight && this.sightAsset.isPaintable)
              {
                if ((UnityEngine.Object) this.sightAsset.albedoBase != (UnityEngine.Object) null && (UnityEngine.Object) this.skinAsset.attachmentSkin != (UnityEngine.Object) null)
                {
                  skin = UnityEngine.Object.Instantiate<Material>(this.skinAsset.attachmentSkin);
                  skin.SetTexture("_AlbedoBase", this.sightAsset.albedoBase);
                  skin.SetTexture("_MetallicBase", this.sightAsset.metallicBase);
                  skin.SetTexture("_EmissionBase", this.sightAsset.emissionBase);
                }
                else if ((UnityEngine.Object) this.skinAsset.tertiarySkin != (UnityEngine.Object) null)
                  skin = this.skinAsset.tertiarySkin;
              }
              if ((UnityEngine.Object) skin != (UnityEngine.Object) null)
              {
                HighlighterTool.skin(this.sightModel, skin);
                goto label_20;
              }
              else
                goto label_20;
            }
            else
              goto label_20;
          }
          else
            goto label_20;
        }
      }
      this._sightAsset = (ItemSightAsset) null;
label_20:
      if ((UnityEngine.Object) this.tacticalModel != (UnityEngine.Object) null)
      {
        UnityEngine.Object.Destroy((UnityEngine.Object) this.tacticalModel.gameObject);
        this._tacticalModel = (Transform) null;
      }
      if ((UnityEngine.Object) this.tacticalHook != (UnityEngine.Object) null)
      {
        if ((int) this.tacticalID != 0)
        {
          try
          {
            this._tacticalAsset = (ItemTacticalAsset) Assets.find(EAssetType.ITEM, this.tacticalID);
          }
          catch
          {
            this._tacticalAsset = (ItemTacticalAsset) null;
          }
          if (this.tacticalAsset != null)
          {
            this._tacticalModel = UnityEngine.Object.Instantiate<GameObject>(this.tacticalAsset.tactical).transform;
            this.tacticalModel.name = "Tactical";
            this.tacticalModel.transform.parent = this.tacticalHook;
            this.tacticalModel.transform.localPosition = Vector3.zero;
            this.tacticalModel.transform.localRotation = Quaternion.identity;
            this.tacticalModel.localScale = Vector3.one;
            if (viewmodel)
              Layerer.viewmodel(this.tacticalModel);
            if (this.gunAsset != null && this.skinAsset != null)
            {
              Material skin = (Material) null;
              if (!this.skinAsset.secondarySkins.TryGetValue(this.tacticalID, out skin) && this.skinAsset.hasTactical && this.tacticalAsset.isPaintable)
              {
                if ((UnityEngine.Object) this.tacticalAsset.albedoBase != (UnityEngine.Object) null && (UnityEngine.Object) this.skinAsset.attachmentSkin != (UnityEngine.Object) null)
                {
                  skin = UnityEngine.Object.Instantiate<Material>(this.skinAsset.attachmentSkin);
                  skin.SetTexture("_AlbedoBase", this.tacticalAsset.albedoBase);
                  skin.SetTexture("_MetallicBase", this.tacticalAsset.metallicBase);
                  skin.SetTexture("_EmissionBase", this.tacticalAsset.emissionBase);
                }
                else if ((UnityEngine.Object) this.skinAsset.tertiarySkin != (UnityEngine.Object) null)
                  skin = this.skinAsset.tertiarySkin;
              }
              if ((UnityEngine.Object) skin != (UnityEngine.Object) null)
              {
                HighlighterTool.skin(this.tacticalModel, skin);
                goto label_38;
              }
              else
                goto label_38;
            }
            else
              goto label_38;
          }
          else
            goto label_38;
        }
      }
      this._tacticalAsset = (ItemTacticalAsset) null;
label_38:
      if ((UnityEngine.Object) this.gripModel != (UnityEngine.Object) null)
      {
        UnityEngine.Object.Destroy((UnityEngine.Object) this.gripModel.gameObject);
        this._gripModel = (Transform) null;
      }
      if ((UnityEngine.Object) this.gripHook != (UnityEngine.Object) null)
      {
        if ((int) this.gripID != 0)
        {
          try
          {
            this._gripAsset = (ItemGripAsset) Assets.find(EAssetType.ITEM, this.gripID);
          }
          catch
          {
            this._gripAsset = (ItemGripAsset) null;
          }
          if (this.gripAsset != null)
          {
            this._gripModel = UnityEngine.Object.Instantiate<GameObject>(this.gripAsset.grip).transform;
            this.gripModel.name = "Grip";
            this.gripModel.transform.parent = this.gripHook;
            this.gripModel.transform.localPosition = Vector3.zero;
            this.gripModel.transform.localRotation = Quaternion.identity;
            this.gripModel.localScale = Vector3.one;
            if (viewmodel)
              Layerer.viewmodel(this.gripModel);
            if (this.gunAsset != null && this.skinAsset != null)
            {
              Material skin = (Material) null;
              if (!this.skinAsset.secondarySkins.TryGetValue(this.gripID, out skin) && this.skinAsset.hasGrip && this.gripAsset.isPaintable)
              {
                if ((UnityEngine.Object) this.gripAsset.albedoBase != (UnityEngine.Object) null && (UnityEngine.Object) this.skinAsset.attachmentSkin != (UnityEngine.Object) null)
                {
                  skin = UnityEngine.Object.Instantiate<Material>(this.skinAsset.attachmentSkin);
                  skin.SetTexture("_AlbedoBase", this.gripAsset.albedoBase);
                  skin.SetTexture("_MetallicBase", this.gripAsset.metallicBase);
                  skin.SetTexture("_EmissionBase", this.gripAsset.emissionBase);
                }
                else if ((UnityEngine.Object) this.skinAsset.tertiarySkin != (UnityEngine.Object) null)
                  skin = this.skinAsset.tertiarySkin;
              }
              if ((UnityEngine.Object) skin != (UnityEngine.Object) null)
              {
                HighlighterTool.skin(this.gripModel, skin);
                goto label_56;
              }
              else
                goto label_56;
            }
            else
              goto label_56;
          }
          else
            goto label_56;
        }
      }
      this._gripAsset = (ItemGripAsset) null;
label_56:
      if ((UnityEngine.Object) this.barrelModel != (UnityEngine.Object) null)
      {
        UnityEngine.Object.Destroy((UnityEngine.Object) this.barrelModel.gameObject);
        this._barrelModel = (Transform) null;
      }
      if ((UnityEngine.Object) this.barrelHook != (UnityEngine.Object) null)
      {
        if ((int) this.barrelID != 0)
        {
          try
          {
            this._barrelAsset = (ItemBarrelAsset) Assets.find(EAssetType.ITEM, this.barrelID);
          }
          catch
          {
            this._barrelAsset = (ItemBarrelAsset) null;
          }
          if (this.barrelAsset != null)
          {
            this._barrelModel = UnityEngine.Object.Instantiate<GameObject>(this.barrelAsset.barrel).transform;
            this.barrelModel.name = "Barrel";
            this.barrelModel.transform.parent = this.barrelHook;
            this.barrelModel.transform.localPosition = Vector3.zero;
            this.barrelModel.transform.localRotation = Quaternion.identity;
            this.barrelModel.localScale = Vector3.one;
            if (viewmodel)
              Layerer.viewmodel(this.barrelModel);
            if (this.gunAsset != null && this.skinAsset != null)
            {
              Material skin = (Material) null;
              if (!this.skinAsset.secondarySkins.TryGetValue(this.barrelID, out skin) && this.skinAsset.hasBarrel && this.barrelAsset.isPaintable)
              {
                if ((UnityEngine.Object) this.barrelAsset.albedoBase != (UnityEngine.Object) null && (UnityEngine.Object) this.skinAsset.attachmentSkin != (UnityEngine.Object) null)
                {
                  skin = UnityEngine.Object.Instantiate<Material>(this.skinAsset.attachmentSkin);
                  skin.SetTexture("_AlbedoBase", this.barrelAsset.albedoBase);
                  skin.SetTexture("_MetallicBase", this.barrelAsset.metallicBase);
                  skin.SetTexture("_EmissionBase", this.barrelAsset.emissionBase);
                }
                else if ((UnityEngine.Object) this.skinAsset.tertiarySkin != (UnityEngine.Object) null)
                  skin = this.skinAsset.tertiarySkin;
              }
              if ((UnityEngine.Object) skin != (UnityEngine.Object) null)
              {
                HighlighterTool.skin(this.barrelModel, skin);
                goto label_74;
              }
              else
                goto label_74;
            }
            else
              goto label_74;
          }
          else
            goto label_74;
        }
      }
      this._barrelAsset = (ItemBarrelAsset) null;
label_74:
      if ((UnityEngine.Object) this.magazineModel != (UnityEngine.Object) null)
      {
        UnityEngine.Object.Destroy((UnityEngine.Object) this.magazineModel.gameObject);
        this._magazineModel = (Transform) null;
      }
      if ((UnityEngine.Object) this.magazineHook != (UnityEngine.Object) null)
      {
        if ((int) this.magazineID != 0)
        {
          try
          {
            this._magazineAsset = (ItemMagazineAsset) Assets.find(EAssetType.ITEM, this.magazineID);
          }
          catch
          {
            this._magazineAsset = (ItemMagazineAsset) null;
          }
          if (this.magazineAsset != null)
          {
            this._magazineModel = UnityEngine.Object.Instantiate<GameObject>(this.magazineAsset.magazine).transform;
            this.magazineModel.name = "Magazine";
            this.magazineModel.transform.parent = this.magazineHook;
            this.magazineModel.transform.localPosition = Vector3.zero;
            this.magazineModel.transform.localRotation = Quaternion.identity;
            this.magazineModel.localScale = Vector3.one;
            if (viewmodel)
              Layerer.viewmodel(this.magazineModel);
            if (this.gunAsset != null && this.skinAsset != null)
            {
              Material skin = (Material) null;
              if (!this.skinAsset.secondarySkins.TryGetValue(this.magazineID, out skin) && this.skinAsset.hasMagazine && this.magazineAsset.isPaintable)
              {
                if ((UnityEngine.Object) this.magazineAsset.albedoBase != (UnityEngine.Object) null && (UnityEngine.Object) this.skinAsset.attachmentSkin != (UnityEngine.Object) null)
                {
                  skin = UnityEngine.Object.Instantiate<Material>(this.skinAsset.attachmentSkin);
                  skin.SetTexture("_AlbedoBase", this.magazineAsset.albedoBase);
                  skin.SetTexture("_MetallicBase", this.magazineAsset.metallicBase);
                  skin.SetTexture("_EmissionBase", this.magazineAsset.emissionBase);
                }
                else if ((UnityEngine.Object) this.skinAsset.tertiarySkin != (UnityEngine.Object) null)
                  skin = this.skinAsset.tertiarySkin;
              }
              if ((UnityEngine.Object) skin != (UnityEngine.Object) null)
              {
                HighlighterTool.skin(this.magazineModel, skin);
                goto label_92;
              }
              else
                goto label_92;
            }
            else
              goto label_92;
          }
          else
            goto label_92;
        }
      }
      this._magazineAsset = (ItemMagazineAsset) null;
label_92:
      if ((UnityEngine.Object) this.tacticalModel != (UnityEngine.Object) null && this.tacticalModel.childCount > 0)
      {
        this._lightHook = this.tacticalModel.transform.FindChild("Model_0").FindChild("Light");
        this._light2Hook = this.tacticalModel.transform.FindChild("Model_0").FindChild("Light2");
        if (viewmodel)
        {
          if ((UnityEngine.Object) this.lightHook != (UnityEngine.Object) null)
          {
            this.lightHook.tag = "Viewmodel";
            this.lightHook.gameObject.layer = LayerMasks.VIEWMODEL;
          }
          if ((UnityEngine.Object) this.light2Hook != (UnityEngine.Object) null)
          {
            this.light2Hook.tag = "Viewmodel";
            this.light2Hook.gameObject.layer = LayerMasks.VIEWMODEL;
          }
        }
      }
      else
      {
        this._lightHook = (Transform) null;
        this._light2Hook = (Transform) null;
      }
      this._aimHook = !((UnityEngine.Object) this.sightModel != (UnityEngine.Object) null) ? (Transform) null : this.sightModel.transform.FindChild("Model_0").FindChild("Aim");
      this._scopeHook = !((UnityEngine.Object) this.aimHook != (UnityEngine.Object) null) ? (Transform) null : this.aimHook.FindChild("Scope");
      if (!((UnityEngine.Object) this.rope != (UnityEngine.Object) null) || !viewmodel)
        return;
      this.rope.tag = "Viewmodel";
      this.rope.gameObject.layer = LayerMasks.VIEWMODEL;
    }

    private void Awake()
    {
      this._sightHook = this.transform.FindChild("Sight");
      this._tacticalHook = this.transform.FindChild("Tactical");
      this._gripHook = this.transform.FindChild("Grip");
      this._barrelHook = this.transform.FindChild("Barrel");
      this._magazineHook = this.transform.FindChild("Magazine");
      this._ejectHook = this.transform.FindChild("Eject");
      this._leftHook = this.transform.FindChild("Left");
      this._rightHook = this.transform.FindChild("Right");
      this._nockHook = this.transform.FindChild("Nock");
      this._restHook = this.transform.FindChild("Rest");
      Transform child = this.transform.FindChild("Rope");
      if (!((UnityEngine.Object) child != (UnityEngine.Object) null))
        return;
      this._rope = (LineRenderer) child.GetComponent<Renderer>();
    }
  }
}
