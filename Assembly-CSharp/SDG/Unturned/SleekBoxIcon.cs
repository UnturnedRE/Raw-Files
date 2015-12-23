// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.SleekBoxIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class SleekBoxIcon : SleekBox
  {
    private SleekImageTexture iconImage;
    private int iconSize;

    public Texture2D icon
    {
      set
      {
        this.iconImage.texture = (Texture) value;
        if (this.iconSize != 0 || !((Object) this.iconImage.texture != (Object) null))
          return;
        this.iconImage.sizeOffset_X = this.iconImage.texture.width;
        this.iconImage.sizeOffset_Y = this.iconImage.texture.height;
      }
    }

    public SleekBoxIcon(Texture2D newIcon, int newSize)
    {
      this.init();
      this.iconImage = new SleekImageTexture();
      this.iconSize = newSize;
      this.iconImage.positionOffset_X = 5;
      this.iconImage.positionOffset_Y = 5;
      this.iconImage.sizeOffset_X = this.iconSize;
      this.iconImage.sizeOffset_Y = this.iconSize;
      this.iconImage.texture = (Texture) newIcon;
      this.add((Sleek) this.iconImage);
      this.fontStyle = FontStyle.Bold;
      this.fontAlignment = TextAnchor.MiddleCenter;
      this.fontSize = SleekRender.FONT_SIZE;
      this.calculateContent();
    }

    public SleekBoxIcon(Texture2D newIcon)
    {
      this.init();
      this.iconImage = new SleekImageTexture();
      this.iconSize = 0;
      this.iconImage.positionOffset_X = 5;
      this.iconImage.positionOffset_Y = 5;
      this.iconImage.texture = (Texture) newIcon;
      this.add((Sleek) this.iconImage);
      if ((Object) this.iconImage.texture != (Object) null)
      {
        this.iconImage.sizeOffset_X = this.iconImage.texture.width;
        this.iconImage.sizeOffset_Y = this.iconImage.texture.height;
      }
      this.fontStyle = FontStyle.Bold;
      this.fontAlignment = TextAnchor.MiddleCenter;
      this.fontSize = SleekRender.FONT_SIZE;
      this.calculateContent();
    }
  }
}
