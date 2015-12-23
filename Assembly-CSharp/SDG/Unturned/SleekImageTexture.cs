// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.SleekImageTexture
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class SleekImageTexture : Sleek
  {
    public Texture texture;
    public float angle;
    public bool isAngled;

    public SleekImageTexture()
    {
      this.init();
    }

    public SleekImageTexture(Texture newTexture)
    {
      this.init();
      this.texture = newTexture;
    }

    public override void draw(bool ignoreCulling)
    {
      if (!this.isHidden)
      {
        if (this.isAngled)
          SleekRender.drawAngledImageTexture(this.frame, this.texture, this.angle, this.backgroundColor);
        else
          SleekRender.drawImageTexture(this.frame, this.texture, this.backgroundColor);
      }
      this.drawChildren(ignoreCulling);
    }
  }
}
