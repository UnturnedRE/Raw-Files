// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ItemGunAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

namespace SDG.Unturned
{
  public class ItemGunAsset : ItemWeaponAsset
  {
    protected AudioClip _shoot;
    protected AudioClip _reload;
    protected AudioClip _hammer;
    protected AudioClip _aim;
    protected GameObject _projectile;
    private byte ammoMin;
    private byte ammoMax;
    private ushort _sightID;
    private byte[] sightState;
    private ushort _tacticalID;
    private byte[] tacticalState;
    private ushort _gripID;
    private byte[] gripState;
    private ushort _barrelID;
    private byte[] barrelState;
    private ushort _magazineID;
    private byte[] magazineState;
    private float _replace;
    private bool _hasSight;
    private bool _hasTactical;
    private bool _hasGrip;
    private bool _hasBarrel;
    private byte _caliber;
    private byte _firerate;
    private EAction _action;
    private bool _hasSafety;
    private bool _hasSemi;
    private bool _hasAuto;
    private EFiremode firemode;
    private float _spreadAim;
    private float _spreadHip;
    private float _recoilMin_x;
    private float _recoilMin_y;
    private float _recoilMax_x;
    private float _recoilMax_y;
    private float _recover_x;
    private float _recover_y;
    private float _shakeMin_x;
    private float _shakeMin_y;
    private float _shakeMin_z;
    private float _shakeMax_x;
    private float _shakeMax_y;
    private float _shakeMax_z;
    private ushort _muzzle;

    public AudioClip shoot
    {
      get
      {
        return this._shoot;
      }
    }

    public AudioClip reload
    {
      get
      {
        return this._reload;
      }
    }

    public AudioClip hammer
    {
      get
      {
        return this._hammer;
      }
    }

    public AudioClip aim
    {
      get
      {
        return this._aim;
      }
    }

    public GameObject projectile
    {
      get
      {
        return this._projectile;
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

    public float replace
    {
      get
      {
        return this._replace;
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

    public byte caliber
    {
      get
      {
        return this._caliber;
      }
    }

    public byte firerate
    {
      get
      {
        return this._firerate;
      }
    }

    public EAction action
    {
      get
      {
        return this._action;
      }
    }

    public bool hasSafety
    {
      get
      {
        return this._hasSafety;
      }
    }

    public bool hasSemi
    {
      get
      {
        return this._hasSemi;
      }
    }

    public bool hasAuto
    {
      get
      {
        return this._hasAuto;
      }
    }

    public float spreadAim
    {
      get
      {
        return this._spreadAim;
      }
    }

    public float spreadHip
    {
      get
      {
        return this._spreadHip;
      }
    }

    public float recoilMin_x
    {
      get
      {
        return this._recoilMin_x;
      }
    }

    public float recoilMin_y
    {
      get
      {
        return this._recoilMin_y;
      }
    }

    public float recoilMax_x
    {
      get
      {
        return this._recoilMax_x;
      }
    }

    public float recoilMax_y
    {
      get
      {
        return this._recoilMax_y;
      }
    }

    public float recover_x
    {
      get
      {
        return this._recover_x;
      }
    }

    public float recover_y
    {
      get
      {
        return this._recover_y;
      }
    }

    public float shakeMin_x
    {
      get
      {
        return this._shakeMin_x;
      }
    }

    public float shakeMin_y
    {
      get
      {
        return this._shakeMin_y;
      }
    }

    public float shakeMin_z
    {
      get
      {
        return this._shakeMin_z;
      }
    }

    public float shakeMax_x
    {
      get
      {
        return this._shakeMax_x;
      }
    }

    public float shakeMax_y
    {
      get
      {
        return this._shakeMax_y;
      }
    }

    public float shakeMax_z
    {
      get
      {
        return this._shakeMax_z;
      }
    }

    public ushort muzzle
    {
      get
      {
        return this._muzzle;
      }
    }

    public override bool showQuality
    {
      get
      {
        return true;
      }
    }

    public ItemGunAsset(Bundle bundle, Data data, ushort id)
      : base(bundle, data, id)
    {
      this._shoot = (AudioClip) bundle.load("Shoot");
      this._reload = (AudioClip) bundle.load("Reload");
      this._hammer = (AudioClip) bundle.load("Hammer");
      this._aim = (AudioClip) bundle.load("Aim");
      this._projectile = (GameObject) bundle.load("Projectile");
      this.ammoMin = data.readByte("Ammo_Min");
      this.ammoMax = data.readByte("Ammo_Max");
      this._sightID = data.readUInt16("Sight");
      this.sightState = BitConverter.GetBytes(this.sightID);
      this._tacticalID = data.readUInt16("Tactical");
      this.tacticalState = BitConverter.GetBytes(this.tacticalID);
      this._gripID = data.readUInt16("Grip");
      this.gripState = BitConverter.GetBytes(this.gripID);
      this._barrelID = data.readUInt16("Barrel");
      this.barrelState = BitConverter.GetBytes(this.barrelID);
      this._magazineID = data.readUInt16("Magazine");
      this.magazineState = BitConverter.GetBytes(this.magazineID);
      this._replace = data.readSingle("Replace");
      if ((double) this.replace < 0.01)
        this._replace = 1f;
      this._hasSight = data.has("Hook_Sight");
      this._hasTactical = data.has("Hook_Tactical");
      this._hasGrip = data.has("Hook_Grip");
      this._hasBarrel = data.has("Hook_Barrel");
      this._caliber = data.readByte("Caliber");
      this._firerate = data.readByte("Firerate");
      this._action = (EAction) Enum.Parse(typeof (EAction), data.readString("Action"), true);
      this._hasSafety = data.has("Safety");
      this._hasSemi = data.has("Semi");
      this._hasAuto = data.has("Auto");
      if (this.hasAuto)
        this.firemode = EFiremode.AUTO;
      else if (this.hasSemi)
        this.firemode = EFiremode.SEMI;
      else if (this.hasSafety)
        this.firemode = EFiremode.SAFETY;
      this._spreadAim = data.readSingle("Spread_Aim");
      this._spreadHip = data.readSingle("Spread_Hip");
      this._recoilMin_x = data.readSingle("Recoil_Min_X");
      this._recoilMin_y = data.readSingle("Recoil_Min_Y");
      this._recoilMax_x = data.readSingle("Recoil_Max_X");
      this._recoilMax_y = data.readSingle("Recoil_Max_Y");
      this._recover_x = data.readSingle("Recover_X");
      this._recover_y = data.readSingle("Recover_Y");
      this._shakeMin_x = data.readSingle("Shake_Min_X");
      this._shakeMin_y = data.readSingle("Shake_Min_Y");
      this._shakeMin_z = data.readSingle("Shake_Min_Z");
      this._shakeMax_x = data.readSingle("Shake_Max_X");
      this._shakeMax_y = data.readSingle("Shake_Max_Y");
      this._shakeMax_z = data.readSingle("Shake_Max_Z");
      this._muzzle = data.readUInt16("Muzzle");
      bundle.unload();
    }

    public override byte[] getState(bool isFull)
    {
      return new byte[18]
      {
        this.sightState[0],
        this.sightState[1],
        this.tacticalState[0],
        this.tacticalState[1],
        this.gripState[0],
        this.gripState[1],
        this.barrelState[0],
        this.barrelState[1],
        this.magazineState[0],
        this.magazineState[1],
        (byte) (isFull || (double) UnityEngine.Random.value > 0.899999976158142 ? (int) this.ammoMax : (int) (byte) UnityEngine.Random.Range((int) this.ammoMin, (int) this.ammoMax + 1)),
        (byte) this.firemode,
        (byte) 1,
        (byte) 100,
        (byte) 100,
        (byte) 100,
        (byte) 100,
        (byte) 100
      };
    }
  }
}
