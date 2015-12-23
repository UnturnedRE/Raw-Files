// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.GroundMaterial
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class GroundMaterial
  {
    private SplatPrototype _prototype;
    public float overgrowth;
    public float chance;
    public float steepness;
    public float height;
    public bool isGrassy_0;
    public bool isGrassy_1;
    public bool isFlowery;
    public bool isRocky;
    public bool isSnowy;
    public bool isFoundation;
    public bool isGenerated;

    public SplatPrototype prototype
    {
      get
      {
        return this._prototype;
      }
    }

    public GroundMaterial(SplatPrototype newPrototype)
    {
      this._prototype = newPrototype;
      this.overgrowth = 0.0f;
      this.chance = 0.0f;
      this.steepness = 0.0f;
      this.height = 1f;
      this.isGrassy_0 = false;
      this.isGrassy_1 = false;
      this.isFlowery = false;
      this.isRocky = true;
      this.isSnowy = false;
      this.isFoundation = false;
      this.isGenerated = true;
    }
  }
}
