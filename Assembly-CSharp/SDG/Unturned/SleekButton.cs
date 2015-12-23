// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.SleekButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class SleekButton : SleekLabel
  {
    public ClickedButton onClickedButton;
    public bool isClickable;

    public SleekButton()
    {
      this.init();
      this.fontStyle = FontStyle.Bold;
      this.fontAlignment = TextAnchor.MiddleCenter;
      this.fontSize = SleekRender.FONT_SIZE;
      this.calculateContent();
      this.isClickable = true;
    }

    public override void draw(bool ignoreCulling)
    {
      if (!this.isHidden)
      {
        if (this.isClickable)
        {
          if (SleekRender.drawButton(this.frame, this.backgroundColor) && this.onClickedButton != null)
            this.onClickedButton(this);
        }
        else
          SleekRender.drawBox(this.frame, this.backgroundColor);
        SleekRender.drawLabel(this.frame, this.fontStyle, this.fontAlignment, this.fontSize, this.isRich, this.foregroundColor, this.content);
      }
      this.drawChildren(ignoreCulling);
    }
  }
}
