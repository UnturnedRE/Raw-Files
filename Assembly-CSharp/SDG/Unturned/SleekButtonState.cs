// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.SleekButtonState
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class SleekButtonState : SleekButton
  {
    private SleekImageTexture icon;
    private GUIContent[] _states;
    private int _state;
    public SwappedState onSwappedState;

    public GUIContent[] states
    {
      get
      {
        return this._states;
      }
    }

    public int state
    {
      get
      {
        return this._state;
      }
      set
      {
        this._state = value;
        if (this.state >= this.states.Length || this.states[this.state] == null)
          return;
        this.text = this.states[this.state].text;
        this.icon.texture = this.states[this.state].image;
      }
    }

    public SleekButtonState(params GUIContent[] newStates)
    {
      this.init();
      this._state = 0;
      this.icon = new SleekImageTexture();
      this.icon.positionOffset_X = 5;
      this.icon.positionOffset_Y = 5;
      this.icon.sizeOffset_X = 20;
      this.icon.sizeOffset_Y = 20;
      this.add((Sleek) this.icon);
      this.fontStyle = FontStyle.Bold;
      this.fontAlignment = TextAnchor.MiddleCenter;
      this.fontSize = SleekRender.FONT_SIZE;
      this.setContent(newStates);
      this.onClickedButton = new ClickedButton(this.onClickedState);
      this.calculateContent();
    }

    private void onClickedState(SleekButton button)
    {
      if (Event.current.button == 0)
      {
        ++this._state;
        if (this.state >= this.states.Length)
          this._state = 0;
      }
      else
      {
        --this._state;
        if (this.state < 0)
          this._state = this.states.Length - 1;
      }
      if (this.state >= this.states.Length || this.states[this.state] == null)
        return;
      this.text = this.states[this.state].text;
      this.icon.texture = this.states[this.state].image;
      if (this.onSwappedState == null)
        return;
      this.onSwappedState(this, this.state);
    }

    public void setContent(params GUIContent[] newStates)
    {
      this._states = newStates;
      if (this.state >= this.states.Length)
        this._state = 0;
      if (this.states.Length > 0 && this.states[this.state] != null)
        this.text = this.states[this.state].text;
      else
        this.text = string.Empty;
      if (this.states.Length > 0 && this.states[this.state] != null)
        this.icon.texture = this.states[this.state].image;
      else
        this.icon.texture = (Texture) null;
    }
  }
}
