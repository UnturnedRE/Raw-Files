// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.SleekValue
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

namespace SDG.Unturned
{
  public class SleekValue : Sleek
  {
    public Valued onValued;
    private SleekSingleField field;
    private SleekSlider slider;
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
        this.field.state = this.state;
        this.slider.state = this.state;
      }
    }

    public SleekValue()
    {
      this.init();
      this.field = new SleekSingleField();
      this.field.sizeOffset_X = -5;
      this.field.sizeScale_X = 0.4f;
      this.field.sizeScale_Y = 1f;
      this.field.onTypedSingle = new TypedSingle(this.onTypedSingleField);
      this.add((Sleek) this.field);
      this.slider = new SleekSlider();
      this.slider.positionOffset_X = 5;
      this.slider.positionOffset_Y = -10;
      this.slider.positionScale_X = 0.4f;
      this.slider.positionScale_Y = 0.5f;
      this.slider.sizeOffset_X = -5;
      this.slider.sizeOffset_Y = 20;
      this.slider.sizeScale_X = 0.6f;
      this.slider.orientation = ESleekOrientation.HORIZONTAL;
      this.slider.onDragged = new Dragged(this.onDraggedSlider);
      this.add((Sleek) this.slider);
    }

    private void onTypedSingleField(SleekSingleField field, float state)
    {
      if (this.onValued != null)
        this.onValued(this, state);
      this._state = state;
      this.slider.state = state;
    }

    private void onDraggedSlider(SleekSlider slider, float state)
    {
      if (this.onValued != null)
        this.onValued(this, state);
      this._state = state;
      this.field.state = state;
    }

    public override void draw(bool ignoreCulling)
    {
      this.drawChildren(ignoreCulling);
    }
  }
}
