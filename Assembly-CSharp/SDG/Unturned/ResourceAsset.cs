// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ResourceAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

namespace SDG.Unturned
{
  public class ResourceAsset : Asset
  {
    protected string _resourceName;
    protected GameObject _model;
    protected GameObject _stump;
    private ushort _health;
    private float _radius;
    private float _scale;
    protected ushort _explosion;
    protected ushort _log;
    protected ushort _stick;
    protected bool _isForage;

    public string resourceName
    {
      get
      {
        return this._resourceName;
      }
    }

    public GameObject model
    {
      get
      {
        return this._model;
      }
    }

    public GameObject stump
    {
      get
      {
        return this._stump;
      }
    }

    public ushort health
    {
      get
      {
        return this._health;
      }
    }

    public float radius
    {
      get
      {
        return this._radius;
      }
    }

    public float scale
    {
      get
      {
        return this._scale;
      }
    }

    public ushort explosion
    {
      get
      {
        return this._explosion;
      }
    }

    public ushort log
    {
      get
      {
        return this._log;
      }
    }

    public ushort stick
    {
      get
      {
        return this._stick;
      }
    }

    public bool isForage
    {
      get
      {
        return this._isForage;
      }
    }

    public ResourceAsset(Bundle bundle, Data data, ushort id)
      : base(bundle, id)
    {
      if ((int) id < 50 && !bundle.hasResource)
        throw new NotSupportedException();
      this._resourceName = Localization.tryRead(bundle.path, bundle.usePath).format("Name");
      if (Dedicator.isDedicated)
      {
        this._model = (GameObject) bundle.load("Resource_Clip");
        if ((UnityEngine.Object) this.model == (UnityEngine.Object) null)
          Debug.LogError((object) (this.resourceName + " is missing collision data. Highly recommended to fix."));
        this._stump = (GameObject) bundle.load("Stump_Clip");
        if ((UnityEngine.Object) this.stump == (UnityEngine.Object) null)
          Debug.LogError((object) (this.resourceName + " is missing collision data. Highly recommended to fix."));
      }
      else
      {
        this._model = (GameObject) bundle.load("Resource");
        this._stump = (GameObject) bundle.load("Stump");
      }
      this._health = data.readUInt16("Health");
      this._radius = data.readSingle("Radius");
      this._scale = data.readSingle("Scale");
      this._explosion = data.readUInt16("Explosion");
      this._log = data.readUInt16("Log");
      this._stick = data.readUInt16("Stick");
      this._isForage = data.has("Forage");
      bundle.unload();
    }
  }
}
