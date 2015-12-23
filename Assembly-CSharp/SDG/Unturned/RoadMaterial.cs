// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.RoadMaterial
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class RoadMaterial
  {
    private Material _material;
    public float width;
    public float height;
    public float depth;
    public bool isConcrete;

    public Material material
    {
      get
      {
        return this._material;
      }
    }

    public RoadMaterial(Texture2D texture)
    {
      this._material = new Material(Shader.Find("Standard"));
      this.material.name = "Road";
      this.material.hideFlags = HideFlags.HideAndDontSave;
      this.material.SetFloat("_Glossiness", 0.0f);
      this.material.mainTexture = (Texture) texture;
      this.width = 4f;
      this.height = 1f;
      this.depth = 0.5f;
      this.isConcrete = true;
    }
  }
}
