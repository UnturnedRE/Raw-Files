// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.SleekLabelIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class SleekLabelIcon : SleekLabel
  {
    private SleekImageTexture iconImage;

    public Texture2D icon
    {
      set
      {
        this.iconImage.texture = (Texture) value;
      }
    }

    public SleekLabelIcon(Texture2D newIcon)
    {
      this.init();
      this.iconImage = new SleekImageTexture();
      this.iconImage.positionOffset_X = 5;
      this.iconImage.positionOffset_Y = -10;
      this.iconImage.positionScale_Y = 0.5f;
      this.iconImage.sizeOffset_X = 20;
      this.iconImage.sizeOffset_Y = 20;
      this.iconImage.texture = (Texture) newIcon;
      this.add((Sleek) this.iconImage);
      this.fontStyle = FontStyle.Bold;
      this.fontAlignment = TextAnchor.MiddleCenter;
      this.fontSize = SleekRender.FONT_SIZE;
      this.calculateContent();
    }
  }
}
