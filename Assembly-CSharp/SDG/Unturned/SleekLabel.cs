// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.SleekLabel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class SleekLabel : Sleek
  {
    private string _text = string.Empty;
    private string _tooltip = string.Empty;
    public FontStyle fontStyle;
    public TextAnchor fontAlignment;
    public int fontSize;
    public bool isRich;
    public GUIContent content;

    public string text
    {
      get
      {
        return this._text;
      }
      set
      {
        this._text = value;
        this.calculateContent();
      }
    }

    public string tooltip
    {
      get
      {
        return this._tooltip;
      }
      set
      {
        this._tooltip = value;
        this.calculateContent();
      }
    }

    public SleekLabel()
    {
      this.init();
      this.fontStyle = FontStyle.Bold;
      this.fontAlignment = TextAnchor.MiddleCenter;
      this.fontSize = SleekRender.FONT_SIZE;
      this.calculateContent();
    }

    protected virtual void calculateContent()
    {
      this.content = new GUIContent(this.text, this.tooltip);
    }

    public override void draw(bool ignoreCulling)
    {
      SleekRender.drawLabel(this.frame, this.fontStyle, this.fontAlignment, this.fontSize, this.isRich, this.foregroundColor, this.content);
      this.drawChildren(ignoreCulling);
    }
  }
}
