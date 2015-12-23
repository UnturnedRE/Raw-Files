// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.SkinAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
  public class SkinAsset : Asset
  {
    protected bool _isPattern;
    protected bool _hasSight;
    protected bool _hasTactical;
    protected bool _hasGrip;
    protected bool _hasBarrel;
    protected bool _hasMagazine;
    protected Material _primarySkin;
    protected Dictionary<ushort, Material> _secondarySkins;
    protected Material _attachmentSkin;
    protected Material _tertiarySkin;

    public bool isPattern
    {
      get
      {
        return this._isPattern;
      }
    }

    public bool hasSight
    {
      get
      {
        return this._hasSight;
      }
    }

    public bool hasTactical
    {
      get
      {
        return this._hasTactical;
      }
    }

    public bool hasGrip
    {
      get
      {
        return this._hasGrip;
      }
    }

    public bool hasBarrel
    {
      get
      {
        return this._hasBarrel;
      }
    }

    public bool hasMagazine
    {
      get
      {
        return this._hasMagazine;
      }
    }

    public Material primarySkin
    {
      get
      {
        return this._primarySkin;
      }
    }

    public Dictionary<ushort, Material> secondarySkins
    {
      get
      {
        return this._secondarySkins;
      }
    }

    public Material attachmentSkin
    {
      get
      {
        return this._attachmentSkin;
      }
    }

    public Material tertiarySkin
    {
      get
      {
        return this._tertiarySkin;
      }
    }

    public SkinAsset(Bundle bundle, Data data, ushort id)
      : base(bundle, id)
    {
      if ((int) id < 2000 && !bundle.hasResource)
        throw new NotSupportedException();
      this._isPattern = data.has("Pattern");
      this._hasSight = data.has("Sight");
      this._hasTactical = data.has("Tactical");
      this._hasGrip = data.has("Grip");
      this._hasBarrel = data.has("Barrel");
      this._hasMagazine = data.has("Magazine");
      if (!Dedicator.isDedicated)
      {
        this._primarySkin = (Material) bundle.load("Skin_Primary");
        this._secondarySkins = new Dictionary<ushort, Material>();
        ushort num = data.readUInt16("Secondary_Skins");
        for (ushort index = (ushort) 0; (int) index < (int) num; ++index)
        {
          ushort key = data.readUInt16("Secondary_" + (object) index);
          if (!this.secondarySkins.ContainsKey(key))
            this.secondarySkins.Add(key, (Material) bundle.load("Skin_Secondary_" + (object) key));
        }
        this._attachmentSkin = (Material) bundle.load("Skin_Attachment");
        this._tertiarySkin = (Material) bundle.load("Skin_Tertiary");
      }
      bundle.unload();
    }
  }
}
