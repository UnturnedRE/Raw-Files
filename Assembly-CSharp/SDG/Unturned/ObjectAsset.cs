// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ObjectAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System;
using System.Text;
using UnityEngine;

namespace SDG.Unturned
{
  public class ObjectAsset : Asset
  {
    protected string _objectName;
    protected EObjectType _type;
    protected GameObject _model;
    private GameObject _nav;
    private GameObject _slots;
    private bool _isSnowshoe;
    private bool _isFuel;
    private bool _isRefill;
    private bool _isSoft;
    private EObjectInteractability _interactability;
    private float _interactabilityDelay;
    private EObjectInteractabilityHint _interactabilityHint;
    private string _interactabilityText;
    private EObjectInteractabilityPower _interactabilityPower;
    private EObjectInteractabilityEditor _interactabilityEditor;
    private EObjectInteractabilityNav _interactabilityNav;
    private ushort[] _interactabilityDrops;

    public string objectName
    {
      get
      {
        return this._objectName;
      }
    }

    public EObjectType type
    {
      get
      {
        return this._type;
      }
    }

    public GameObject model
    {
      get
      {
        return this._model;
      }
    }

    public GameObject nav
    {
      get
      {
        return this._nav;
      }
    }

    public GameObject slots
    {
      get
      {
        return this._slots;
      }
    }

    public bool isSnowshoe
    {
      get
      {
        return this._isSnowshoe;
      }
    }

    public bool isFuel
    {
      get
      {
        return this._isFuel;
      }
    }

    public bool isRefill
    {
      get
      {
        return this._isRefill;
      }
    }

    public bool isSoft
    {
      get
      {
        return this._isSoft;
      }
    }

    public EObjectInteractability interactability
    {
      get
      {
        return this._interactability;
      }
    }

    public float interactabilityDelay
    {
      get
      {
        return this._interactabilityDelay;
      }
    }

    public EObjectInteractabilityHint interactabilityHint
    {
      get
      {
        return this._interactabilityHint;
      }
    }

    public string interactabilityText
    {
      get
      {
        return this._interactabilityText;
      }
    }

    public EObjectInteractabilityPower interactabilityPower
    {
      get
      {
        return this._interactabilityPower;
      }
    }

    public EObjectInteractabilityEditor interactabilityEditor
    {
      get
      {
        return this._interactabilityEditor;
      }
    }

    public EObjectInteractabilityNav interactabilityNav
    {
      get
      {
        return this._interactabilityNav;
      }
    }

    public ushort[] interactabilityDrops
    {
      get
      {
        return this._interactabilityDrops;
      }
    }

    public ObjectAsset(Bundle bundle, Data data, ushort id)
      : base(bundle, id)
    {
      if ((int) id < 2000 && !bundle.hasResource)
        throw new NotSupportedException();
      Local local = Localization.tryRead(bundle.path, bundle.usePath);
      this._objectName = local.format("Name");
      this._type = (EObjectType) Enum.Parse(typeof (EObjectType), data.readString("Type"), true);
      if (Dedicator.isDedicated)
      {
        this._model = (GameObject) bundle.load("Clip");
        if ((UnityEngine.Object) this.model == (UnityEngine.Object) null && this.type != EObjectType.SMALL)
          Debug.LogError((object) (this.objectName + " is missing collision data. Highly recommended to fix."));
      }
      else
      {
        this._model = (GameObject) bundle.load("Object");
        if ((UnityEngine.Object) this.model == (UnityEngine.Object) null)
          throw new NotSupportedException();
      }
      this._nav = (GameObject) bundle.load("Nav");
      this._slots = (GameObject) bundle.load("Slots");
      if ((UnityEngine.Object) this.nav == (UnityEngine.Object) null && this.type == EObjectType.LARGE)
        Debug.LogError((object) (this.objectName + " is missing navigation data. Highly recommended to fix."));
      this._isSnowshoe = data.has("Snowshoe");
      this._isFuel = data.has("Fuel");
      this._isRefill = data.has("Refill");
      this._isSoft = data.has("Soft");
      if (data.has("Interactability"))
      {
        this._interactability = (EObjectInteractability) Enum.Parse(typeof (EObjectInteractability), data.readString("Interactability"), true);
        this._interactabilityDelay = data.readSingle("Interactability_Delay");
        this._interactabilityHint = (EObjectInteractabilityHint) Enum.Parse(typeof (EObjectInteractabilityHint), data.readString("Interactability_Hint"), true);
        if (this.interactability == EObjectInteractability.NOTE)
        {
          ushort num = data.readUInt16("Interactability_Text_Lines");
          StringBuilder stringBuilder = new StringBuilder();
          for (ushort index = (ushort) 0; (int) index < (int) num; ++index)
          {
            string str = local.format("Interactability_Text_Line_" + (object) index);
            stringBuilder.AppendLine(str);
          }
          this._interactabilityText = stringBuilder.ToString();
        }
        else
          this._interactabilityText = string.Empty;
        this._interactabilityPower = !data.has("Interactability_Power") ? EObjectInteractabilityPower.NONE : (EObjectInteractabilityPower) Enum.Parse(typeof (EObjectInteractabilityPower), data.readString("Interactability_Power"), true);
        this._interactabilityEditor = !data.has("Interactability_Editor") ? EObjectInteractabilityEditor.NONE : (EObjectInteractabilityEditor) Enum.Parse(typeof (EObjectInteractabilityEditor), data.readString("Interactability_Editor"), true);
        this._interactabilityNav = !data.has("Interactability_Nav") ? EObjectInteractabilityNav.NONE : (EObjectInteractabilityNav) Enum.Parse(typeof (EObjectInteractabilityNav), data.readString("Interactability_Nav"), true);
        this._interactabilityDrops = new ushort[(int) data.readByte("Interactability_Drops")];
        for (byte index = (byte) 0; (int) index < this.interactabilityDrops.Length; ++index)
          this.interactabilityDrops[(int) index] = data.readUInt16("Interactability_Drop_" + (object) index);
      }
      else
      {
        this._interactability = EObjectInteractability.NONE;
        this._interactabilityPower = EObjectInteractabilityPower.NONE;
        this._interactabilityEditor = EObjectInteractabilityEditor.NONE;
      }
      bundle.unload();
    }

    public virtual byte[] getState()
    {
      byte[] numArray;
      if (this.interactability == EObjectInteractability.BINARY_STATE)
        numArray = new byte[1]
        {
          !Level.isEditor || this.interactabilityEditor == EObjectInteractabilityEditor.NONE ? (byte) 0 : (byte) 1
        };
      else
        numArray = (byte[]) null;
      return numArray;
    }
  }
}
