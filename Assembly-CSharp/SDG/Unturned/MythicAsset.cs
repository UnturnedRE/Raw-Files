// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.MythicAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

namespace SDG.Unturned
{
  public class MythicAsset : Asset
  {
    protected GameObject _systemArea;
    protected GameObject _systemHook;
    protected GameObject _systemFirst;
    protected GameObject _systemThird;

    public GameObject systemArea
    {
      get
      {
        return this._systemArea;
      }
    }

    public GameObject systemHook
    {
      get
      {
        return this._systemHook;
      }
    }

    public GameObject systemFirst
    {
      get
      {
        return this._systemFirst;
      }
    }

    public GameObject systemThird
    {
      get
      {
        return this._systemThird;
      }
    }

    public MythicAsset(Bundle bundle, Data data, ushort id)
      : base(bundle, id)
    {
      if ((int) id < 500 && !bundle.hasResource)
        throw new NotSupportedException();
      if (!Dedicator.isDedicated)
      {
        this._systemArea = (GameObject) bundle.load("System_Area");
        this._systemHook = (GameObject) bundle.load("System_Hook");
        this._systemFirst = (GameObject) bundle.load("System_First");
        this._systemThird = (GameObject) bundle.load("System_Third");
      }
      bundle.unload();
    }
  }
}
