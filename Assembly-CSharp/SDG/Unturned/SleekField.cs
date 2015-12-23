// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.SleekField
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class SleekField : SleekBox
  {
    public Typed onTyped;
    public char replace;
    public bool multiline;
    public int maxLength;

    public SleekField()
    {
      this.init();
      this.replace = ' ';
      this.fontStyle = FontStyle.Bold;
      this.fontAlignment = TextAnchor.MiddleCenter;
      this.fontSize = SleekRender.FONT_SIZE;
      this.maxLength = 16;
      this.calculateContent();
    }

    protected override void calculateContent()
    {
      this.content = new GUIContent(string.Empty, this.tooltip);
    }

    public override void draw(bool ignoreCulling)
    {
      SleekRender.drawBox(this.frame, this.backgroundColor, this.content);
      string text = (int) this.replace == 32 ? SleekRender.drawField(this.frame, this.fontStyle, this.fontAlignment, this.fontSize, this.backgroundColor, this.foregroundColor, this.text, this.multiline) : SleekRender.drawField(this.frame, this.fontStyle, this.fontAlignment, this.fontSize, this.backgroundColor, this.foregroundColor, this.text, this.replace);
      if (text.Length > this.maxLength)
        text = text.Substring(0, this.maxLength);
      if (text != this.text && this.onTyped != null)
        this.onTyped(this, text);
      this.text = text;
      this.drawChildren(ignoreCulling);
    }
  }
}
