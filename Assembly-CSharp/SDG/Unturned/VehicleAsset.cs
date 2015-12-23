// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.VehicleAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

namespace SDG.Unturned
{
  public class VehicleAsset : Asset
  {
    protected string _vehicleName;
    protected GameObject _vehicle;
    protected GameObject _clip;
    protected AudioClip _ignition;
    protected AudioClip _horn;
    protected float _speedMin;
    protected float _speedMax;
    protected float _steerMin;
    protected float _steerMax;
    protected float _brake;
    protected ushort _fuelMin;
    protected ushort _fuelMax;
    protected ushort _fuel;
    protected ushort _healthMin;
    protected ushort _healthMax;
    protected ushort _health;
    protected ushort _explosion;
    protected bool _hasHeadlights;
    protected bool _hasSirens;
    protected bool _hasZip;
    protected bool _hasCrawler;
    protected bool _hasTraction;
    protected float _exit;

    public string vehicleName
    {
      get
      {
        return this._vehicleName;
      }
    }

    public GameObject vehicle
    {
      get
      {
        return this._vehicle;
      }
    }

    public GameObject clip
    {
      get
      {
        return this._clip;
      }
    }

    public AudioClip ignition
    {
      get
      {
        return this._ignition;
      }
    }

    public AudioClip horn
    {
      get
      {
        return this._horn;
      }
    }

    public float speedMin
    {
      get
      {
        return this._speedMin;
      }
    }

    public float speedMax
    {
      get
      {
        return this._speedMax;
      }
    }

    public float steerMin
    {
      get
      {
        return this._steerMin;
      }
    }

    public float steerMax
    {
      get
      {
        return this._steerMax;
      }
    }

    public float brake
    {
      get
      {
        return this._brake;
      }
    }

    public ushort fuelMin
    {
      get
      {
        return this._fuelMin;
      }
    }

    public ushort fuelMax
    {
      get
      {
        return this._fuelMax;
      }
    }

    public ushort fuel
    {
      get
      {
        return this._fuel;
      }
    }

    public ushort healthMin
    {
      get
      {
        return this._healthMin;
      }
    }

    public ushort healthMax
    {
      get
      {
        return this._healthMax;
      }
    }

    public ushort health
    {
      get
      {
        return this._health;
      }
    }

    public ushort explosion
    {
      get
      {
        return this._explosion;
      }
    }

    public bool hasHeadlights
    {
      get
      {
        return this._hasHeadlights;
      }
    }

    public bool hasSirens
    {
      get
      {
        return this._hasSirens;
      }
    }

    public bool hasZip
    {
      get
      {
        return this._hasZip;
      }
    }

    public bool hasCrawler
    {
      get
      {
        return this._hasCrawler;
      }
    }

    public bool hasTraction
    {
      get
      {
        return this._hasTraction;
      }
    }

    public float exit
    {
      get
      {
        return this._exit;
      }
    }

    public VehicleAsset(Bundle bundle, Data data, ushort id)
      : base(bundle, id)
    {
      if ((int) id < 200 && !bundle.hasResource)
        throw new NotSupportedException();
      this._vehicleName = Localization.tryRead(bundle.path, bundle.usePath).format("Name");
      this._vehicle = (GameObject) bundle.load("Vehicle");
      this._clip = (GameObject) bundle.load("Clip");
      this._hasHeadlights = (UnityEngine.Object) this.vehicle.transform.FindChild("Headlights") != (UnityEngine.Object) null;
      this._hasSirens = (UnityEngine.Object) this.vehicle.transform.FindChild("Sirens") != (UnityEngine.Object) null;
      this._hasZip = data.has("Zip");
      this._hasCrawler = data.has("Crawler");
      this._hasTraction = data.has("Traction");
      this._ignition = (AudioClip) bundle.load("Ignition");
      this._horn = (AudioClip) bundle.load("Horn");
      if ((UnityEngine.Object) this.clip == (UnityEngine.Object) null)
        Debug.LogError((object) (this.vehicleName + " is missing collision data. Highly recommended to fix."));
      this._speedMin = data.readSingle("Speed_Min");
      this._speedMax = data.readSingle("Speed_Max") * 1.25f;
      this._steerMin = data.readSingle("Steer_Min");
      this._steerMax = data.readSingle("Steer_Max") * 0.75f;
      this._brake = data.readSingle("Brake");
      this._fuelMin = data.readUInt16("Fuel_Min");
      this._fuelMax = data.readUInt16("Fuel_Max");
      this._fuel = data.readUInt16("Fuel");
      this._healthMin = data.readUInt16("Health_Min");
      this._healthMax = data.readUInt16("Health_Max");
      this._health = data.readUInt16("Health");
      this._explosion = data.readUInt16("Explosion");
      this._exit = !data.has("Exit") ? 2f : data.readSingle("Exit");
      bundle.unload();
    }
  }
}
