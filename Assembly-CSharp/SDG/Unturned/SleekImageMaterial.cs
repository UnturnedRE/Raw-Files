// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.SleekImageMaterial
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class SleekImageMaterial : SleekImageTexture
  {
    public Material material;

    public SleekImageMaterial()
    {
      this.init();
    }

    public SleekImageMaterial(Texture newTexture, Material newMaterial)
    {
      this.init();
      this.texture = newTexture;
      this.material = newMaterial;
    }

    public override void draw(bool ignoreCulling)
    {
      if (!this.isHidden)
        SleekRender.drawImageMaterial(this.frame, this.texture, this.material);
      this.drawChildren(ignoreCulling);
    }
  }
}
