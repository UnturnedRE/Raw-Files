// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.AnimalAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

namespace SDG.Unturned
{
  public class AnimalAsset : Asset
  {
    protected string _animalName;
    protected GameObject _client;
    protected GameObject _server;
    protected GameObject _dedicated;
    protected GameObject _ragdoll;
    protected float _speedRun;
    protected float _speedWalk;
    private EAnimalBehaviour _behaviour;
    protected ushort _health;
    protected byte _damage;
    protected ushort _meat;
    protected ushort _pelt;
    protected AudioClip[] _roars;
    protected AudioClip[] _panics;

    public string animalName
    {
      get
      {
        return this._animalName;
      }
    }

    public GameObject client
    {
      get
      {
        return this._client;
      }
    }

    public GameObject server
    {
      get
      {
        return this._server;
      }
    }

    public GameObject dedicated
    {
      get
      {
        return this._dedicated;
      }
    }

    public GameObject ragdoll
    {
      get
      {
        return this._ragdoll;
      }
    }

    public float speedRun
    {
      get
      {
        return this._speedRun;
      }
    }

    public float speedWalk
    {
      get
      {
        return this._speedWalk;
      }
    }

    public EAnimalBehaviour behaviour
    {
      get
      {
        return this._behaviour;
      }
    }

    public ushort health
    {
      get
      {
        return this._health;
      }
    }

    public byte damage
    {
      get
      {
        return this._damage;
      }
    }

    public ushort meat
    {
      get
      {
        return this._meat;
      }
    }

    public ushort pelt
    {
      get
      {
        return this._pelt;
      }
    }

    public AudioClip[] roars
    {
      get
      {
        return this._roars;
      }
    }

    public AudioClip[] panics
    {
      get
      {
        return this._panics;
      }
    }

    public AnimalAsset(Bundle bundle, Data data, ushort id)
      : base(bundle, id)
    {
      if ((int) id < 50 && !bundle.hasResource)
        throw new NotSupportedException();
      this._animalName = Localization.tryRead(bundle.path, bundle.usePath).format("Name");
      this._client = (GameObject) bundle.load("Animal_Client");
      this._server = (GameObject) bundle.load("Animal_Server");
      this._dedicated = (GameObject) bundle.load("Animal_Dedicated");
      this._ragdoll = (GameObject) bundle.load("Ragdoll");
      if ((UnityEngine.Object) this.client == (UnityEngine.Object) null)
        Debug.LogError((object) (this.animalName + " is missing client data. Highly recommended to fix."));
      if ((UnityEngine.Object) this.server == (UnityEngine.Object) null)
        Debug.LogError((object) (this.animalName + " is missing server data. Highly recommended to fix."));
      if ((UnityEngine.Object) this.dedicated == (UnityEngine.Object) null)
        Debug.LogError((object) (this.animalName + " is missing dedicated data. Highly recommended to fix."));
      if ((UnityEngine.Object) this.ragdoll == (UnityEngine.Object) null)
        Debug.LogError((object) (this.animalName + " is missing ragdoll data. Highly recommended to fix."));
      this._speedRun = data.readSingle("Speed_Run");
      this._speedWalk = data.readSingle("Speed_Walk");
      this._behaviour = (EAnimalBehaviour) Enum.Parse(typeof (EAnimalBehaviour), data.readString("Behaviour"), true);
      this._health = data.readUInt16("Health");
      this._damage = data.readByte("Damage");
      this._meat = data.readUInt16("Meat");
      this._pelt = data.readUInt16("Pelt");
      this._roars = new AudioClip[(int) data.readByte("Roars")];
      for (byte index = (byte) 0; (int) index < this.roars.Length; ++index)
        this.roars[(int) index] = (AudioClip) bundle.load("Roar_" + (object) index);
      this._panics = new AudioClip[(int) data.readByte("Panics")];
      for (byte index = (byte) 0; (int) index < this.panics.Length; ++index)
        this.panics[(int) index] = (AudioClip) bundle.load("Panic_" + (object) index);
      bundle.unload();
    }
  }
}
