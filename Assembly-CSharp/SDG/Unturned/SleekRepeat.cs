// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.SleekRepeat
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class SleekRepeat : SleekLabel
  {
    public StartedButton onStartedButton;
    public StoppedButton onStoppedButton;
    private bool isHeld;

    public SleekRepeat()
    {
      this.init();
      this.fontStyle = FontStyle.Bold;
      this.fontAlignment = TextAnchor.MiddleCenter;
      this.fontSize = SleekRender.FONT_SIZE;
      this.calculateContent();
    }

    public override void draw(bool ignoreCulling)
    {
      if (!this.isHidden)
      {
        if (SleekRender.drawRepeat(this.frame, this.backgroundColor))
        {
          if (!this.isHeld)
          {
            this.isHeld = true;
            if (this.onStartedButton != null)
              this.onStartedButton(this);
          }
        }
        else if (Event.current.type == EventType.Repaint && this.isHeld)
        {
          this.isHeld = false;
          if (this.onStoppedButton != null)
            this.onStoppedButton(this);
        }
        SleekRender.drawLabel(this.frame, this.fontStyle, this.fontAlignment, this.fontSize, this.isRich, this.foregroundColor, this.content);
      }
      this.drawChildren(ignoreCulling);
    }
  }
}
