// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.GroundDetail
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class GroundDetail
  {
    private DetailPrototype _prototype;
    public float density;
    public float chance;
    public bool isGrass_0;
    public bool isGrass_1;
    public bool isFlower;
    public bool isRock;
    public bool isSnow;

    public DetailPrototype prototype
    {
      get
      {
        return this._prototype;
      }
    }

    public GroundDetail(DetailPrototype newPrototype)
    {
      this._prototype = newPrototype;
      this.density = 0.0f;
      this.chance = 0.0f;
      this.isGrass_0 = true;
      this.isGrass_1 = true;
      this.isFlower = false;
      this.isRock = false;
      this.isSnow = false;
    }
  }
}
