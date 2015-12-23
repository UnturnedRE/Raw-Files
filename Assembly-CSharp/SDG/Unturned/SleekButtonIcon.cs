// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.SleekButtonIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class SleekButtonIcon : SleekButton
  {
    protected SleekImageTexture iconImage;
    private int iconSize;
    private bool iconScale;

    public Texture2D icon
    {
      set
      {
        this.iconImage.texture = (Texture) value;
        if (this.iconSize != 0 || this.iconScale || !((Object) this.iconImage.texture != (Object) null))
          return;
        this.iconImage.sizeOffset_X = this.iconImage.texture.width;
        this.iconImage.sizeOffset_Y = this.iconImage.texture.height;
      }
    }

    public SleekButtonIcon(Texture2D newIcon, int newSize)
    {
      this.init();
      this.iconImage = new SleekImageTexture();
      this.iconSize = newSize;
      this.iconScale = false;
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

    public SleekButtonIcon(Texture2D newIcon)
    {
      this.init();
      this.iconImage = new SleekImageTexture();
      this.iconSize = 0;
      this.iconScale = false;
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

    public SleekButtonIcon(Texture2D newIcon, bool newScale)
    {
      this.init();
      this.iconImage = new SleekImageTexture();
      this.iconSize = 0;
      this.iconScale = newScale;
      this.iconImage.positionOffset_X = 5;
      this.iconImage.positionOffset_Y = 5;
      if (this.iconScale)
      {
        this.iconImage.sizeOffset_X = -10;
        this.iconImage.sizeOffset_Y = -10;
        this.iconImage.sizeScale_X = 1f;
        this.iconImage.sizeScale_Y = 1f;
      }
      this.iconImage.texture = (Texture) newIcon;
      this.add((Sleek) this.iconImage);
      this.fontStyle = FontStyle.Bold;
      this.fontAlignment = TextAnchor.MiddleCenter;
      this.fontSize = SleekRender.FONT_SIZE;
      this.calculateContent();
    }
  }
}
