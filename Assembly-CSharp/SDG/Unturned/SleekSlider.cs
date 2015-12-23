// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.SleekSlider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

namespace SDG.Unturned
{
  public class SleekSlider : Sleek
  {
    public Dragged onDragged;
    public ESleekOrientation orientation;
    public float size;
    private float scroll;
    private float _state;

    public float state
    {
      get
      {
        return this._state;
      }
      set
      {
        this._state = value;
        this.scroll = this.state * (1f - this.size);
      }
    }

    public SleekSlider()
    {
      this.init();
      this.orientation = ESleekOrientation.VERTICAL;
      this.size = 0.25f;
    }

    public override void draw(bool ignoreCulling)
    {
      float num = SleekRender.drawSlider(this.frame, this.orientation, this.scroll, this.size);
      if ((double) num != (double) this.scroll)
      {
        this._state = num / (1f - this.size);
        if ((double) this.state < 0.0)
          this.state = 0.0f;
        else if ((double) this.state > 1.0)
          this.state = 1f;
        if (this.onDragged != null)
          this.onDragged(this, this.state);
      }
      this.scroll = num;
      this.drawChildren(ignoreCulling);
    }
  }
}
