// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.SleekImageButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class SleekImageButton : Sleek
  {
    public ClickedImage onClickedImage;
    public ClickImageStarted onClickImageStarted;
    public ClickImageStopped onClickImageStopped;
    public Texture texture;
    private bool isHeld;

    public SleekImageButton()
    {
      this.init();
    }

    public override void draw(bool ignoreCulling)
    {
      if (!this.isHidden)
      {
        if (SleekRender.drawImageButton(this.frame, this.texture, this.backgroundColor))
        {
          if (this.onClickedImage != null)
            this.onClickedImage(this);
          if (!this.isHeld)
          {
            this.isHeld = true;
            if (this.onClickImageStarted != null)
              this.onClickImageStarted(this);
          }
        }
        else if (this.isHeld)
        {
          this.isHeld = false;
          if (this.onClickImageStopped != null)
            this.onClickImageStopped(this);
        }
      }
      this.drawChildren(ignoreCulling);
    }
  }
}
