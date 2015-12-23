// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.SleekUInt32Field
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class SleekUInt32Field : SleekBox
  {
    public TypedUInt32 onTypedUInt32;
    private uint _state;

    public uint state
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

    public SleekUInt32Field()
    {
      this.init();
      this.state = 0U;
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
      uint result;
      if (s != this.text && uint.TryParse(s, out result))
      {
        this._state = result;
        if (this.onTypedUInt32 != null)
          this.onTypedUInt32(this, result);
      }
      this.text = s;
      this.drawChildren(ignoreCulling);
    }
  }
}
