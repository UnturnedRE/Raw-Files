// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.SleekInspect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class SleekInspect : SleekBox
  {
    public SleekImageMaterial renderImage;

    public SleekInspect(string path)
    {
      this.init();
      RenderTexture renderTexture = (RenderTexture) Resources.Load(path);
      this.renderImage = new SleekImageMaterial();
      this.renderImage.sizeScale_X = 1f;
      this.renderImage.sizeScale_Y = 1f;
      this.renderImage.constraint = ESleekConstraint.X;
      this.renderImage.constrain_Y = renderTexture.height;
      this.renderImage.texture = (Texture) renderTexture;
      this.renderImage.material = (Material) Resources.Load("Materials/RenderTexture");
      this.add((Sleek) this.renderImage);
      this.fontStyle = FontStyle.Bold;
      this.fontAlignment = TextAnchor.MiddleCenter;
      this.fontSize = SleekRender.FONT_SIZE;
      this.calculateContent();
    }
  }
}
