// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.SleekByteField
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class SleekByteField : SleekBox
  {
    public TypedByte onTypedByte;
    private byte _state;

    public byte state
    {
      get
      {
        return this._state;
      }
      set
      {
        this._state = value;
        this.text = this.state.ToString();
      }
    }

    public SleekByteField()
    {
      this.init();
      this.state = (byte) 0;
      this.fontStyle = FontStyle.Bold;
      this.fontAlignment = TextAnchor.MiddleCenter;
      this.fontSize = SleekRender.FONT_SIZE;
      this.calculateContent();
    }

    protected override void calculateContent()
    {
      this.content = new GUIContent(string.Empty, this.tooltip);
    }

    public override void draw(bool ignoreCulling)
    {
      SleekRender.drawBox(this.frame, this.backgroundColor, this.content);
      string s = SleekRender.drawField(this.frame, this.fontStyle, this.fontAlignment, this.fontSize, this.backgroundColor, this.foregroundColor, this.text, false);
      byte result;
      if (s != this.text && byte.TryParse(s, out result))
      {
        this._state = result;
        if (this.onTypedByte != null)
          this.onTypedByte(this, result);
      }
      this.text = s;
      this.drawChildren(ignoreCulling);
    }
  }
}
