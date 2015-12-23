// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.Sleek
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
  public class Sleek
  {
    public Color backgroundColor;
    public Color foregroundColor;
    public bool isVisible;
    public bool isHidden;
    public bool isInputable;
    private Sleek _parent;
    private List<Sleek> _children;
    private int fromPositionOffset_X;
    private int fromPositionOffset_Y;
    private int toPositionOffset_X;
    private int toPositionOffset_Y;
    private float fromPositionScale_X;
    private float fromPositionScale_Y;
    private float toPositionScale_X;
    private float toPositionScale_Y;
    private int fromSizeOffset_X;
    private int fromSizeOffset_Y;
    private int toSizeOffset_X;
    private int toSizeOffset_Y;
    private float fromSizeScale_X;
    private float fromSizeScale_Y;
    private float toSizeScale_X;
    private float toSizeScale_Y;
    private ESleekLerp positionOffsetLerp;
    private float positionOffsetLerpTime;
    private float positionOffsetLerpTicked;
    private bool isLerpingPositionOffset;
    private ESleekLerp positionScaleLerp;
    private float positionScaleLerpTime;
    private float positionScaleLerpTicked;
    private bool isLerpingPositionScale;
    private ESleekLerp sizeOffsetLerp;
    private float sizeOffsetLerpTime;
    private float sizeOffsetLerpTicked;
    private bool isLerpingSizeOffset;
    private ESleekLerp sizeScaleLerp;
    private float sizeScaleLerpTime;
    private float sizeScaleLerpTicked;
    private bool isLerpingSizeScale;
    private bool needsFrame;
    protected Rect _frame;
    protected bool local;
    private SleekLabel sideLabel;
    private ESleekConstraint _constraint;
    private int _constrain_X;
    private int _constrain_Y;
    private int _positionOffset_X;
    private int _positionOffset_Y;
    private float _positionScale_X;
    private float _positionScale_Y;
    private int _sizeOffset_X;
    private int _sizeOffset_Y;
    private float _sizeScale_X;
    private float _sizeScale_Y;

    public Sleek parent
    {
      get
      {
        return this._parent;
      }
    }

    public List<Sleek> children
    {
      get
      {
        return this._children;
      }
    }

    public Rect frame
    {
      get
      {
        return this._frame;
      }
    }

    public ESleekConstraint constraint
    {
      get
      {
        return this._constraint;
      }
      set
      {
        this._constraint = value;
        this.needsFrame = true;
      }
    }

    public int constrain_X
    {
      get
      {
        return this._constrain_X;
      }
      set
      {
        this._constrain_X = value;
        this.needsFrame = true;
      }
    }

    public int constrain_Y
    {
      get
      {
        return this._constrain_Y;
      }
      set
      {
        this._constrain_Y = value;
        this.needsFrame = true;
      }
    }

    public int positionOffset_X
    {
      get
      {
        return this._positionOffset_X;
      }
      set
      {
        this._positionOffset_X = value;
        this.needsFrame = true;
      }
    }

    public int positionOffset_Y
    {
      get
      {
        return this._positionOffset_Y;
      }
      set
      {
        this._positionOffset_Y = value;
        this.needsFrame = true;
      }
    }

    public float positionScale_X
    {
      get
      {
        return this._positionScale_X;
      }
      set
      {
        this._positionScale_X = value;
        this.needsFrame = true;
      }
    }

    public float positionScale_Y
    {
      get
      {
        return this._positionScale_Y;
      }
      set
      {
        this._positionScale_Y = value;
        this.needsFrame = true;
      }
    }

    public int sizeOffset_X
    {
      get
      {
        return this._sizeOffset_X;
      }
      set
      {
        this._sizeOffset_X = value;
        this.needsFrame = true;
      }
    }

    public int sizeOffset_Y
    {
      get
      {
        return this._sizeOffset_Y;
      }
      set
      {
        this._sizeOffset_Y = value;
        this.needsFrame = true;
      }
    }

    public float sizeScale_X
    {
      get
      {
        return this._sizeScale_X;
      }
      set
      {
        this._sizeScale_X = value;
        this.needsFrame = true;
      }
    }

    public float sizeScale_Y
    {
      get
      {
        return this._sizeScale_Y;
      }
      set
      {
        this._sizeScale_Y = value;
        this.needsFrame = true;
      }
    }

    public Sleek()
    {
      this.init();
    }

    public virtual void draw(bool ignoreCulling)
    {
      this.drawChildren(ignoreCulling);
    }

    protected void drawChildren(bool ignoreCulling)
    {
      if (!this.isInputable)
        GUI.enabled = false;
      if (this.local)
        ignoreCulling = true;
      for (int index = 0; index < this.children.Count; ++index)
      {
        this.children[index].update();
        if (this.children[index].isVisible && (ignoreCulling || this.children[index].isOnScreen()))
          this.children[index].draw(ignoreCulling);
      }
      if (this.isInputable)
        return;
      GUI.enabled = true;
    }

    protected Rect calculate()
    {
      if (this._parent == null)
      {
        if (Screen.width == 5760 && Screen.height == 1080)
          return new Rect(1920f, 0.0f, 1920f, 1080f);
        return new Rect((float) this.positionOffset_X, (float) this.positionOffset_Y, (float) Screen.width, (float) Screen.height);
      }
      Rect rect = this._parent.calculate();
      if (this._parent.local)
      {
        rect.x = (float) this.positionOffset_X;
        rect.y = (float) this.positionOffset_Y;
      }
      else
      {
        rect.x += (float) this.positionOffset_X + rect.width * this.positionScale_X;
        rect.y += (float) this.positionOffset_Y + rect.height * this.positionScale_Y;
      }
      rect.width = (float) this.sizeOffset_X + rect.width * this.sizeScale_X;
      rect.height = (float) this.sizeOffset_Y + rect.height * this.sizeScale_Y;
      if (this.constrain_X != 0 && (double) rect.width > (double) this.constrain_X)
      {
        rect.x += (float) (((double) rect.width - (double) this.constrain_X) / 2.0);
        rect.width = (float) this.constrain_X;
      }
      if (this.constrain_Y != 0 && (double) rect.height > (double) this.constrain_Y)
      {
        rect.y += (float) (((double) rect.height - (double) this.constrain_Y) / 2.0);
        rect.height = (float) this.constrain_Y;
      }
      if (this.constraint == ESleekConstraint.X)
      {
        rect.x += (float) (((double) rect.width - (double) rect.height) / 2.0);
        rect.width = rect.height;
      }
      else if (this.constraint == ESleekConstraint.Y)
      {
        rect.y += (float) (((double) rect.height - (double) rect.width) / 2.0);
        rect.height = rect.width;
      }
      else if (this.constraint == ESleekConstraint.XY)
      {
        if ((double) rect.width < (double) rect.height)
        {
          rect.y += (float) (((double) rect.height - (double) rect.width) / 2.0);
          rect.height = rect.width;
        }
        else
        {
          rect.x += (float) (((double) rect.width - (double) rect.height) / 2.0);
          rect.width = rect.height;
        }
      }
      return rect;
    }

    public void lerpPositionOffset(int newPositionOffset_X, int newPositionOffset_Y, ESleekLerp lerp, float time)
    {
      this.isLerpingPositionOffset = true;
      this.positionOffsetLerp = lerp;
      this.positionOffsetLerpTime = time;
      this.positionOffsetLerpTicked = Time.realtimeSinceStartup;
      this.fromPositionOffset_X = this.positionOffset_X;
      this.fromPositionOffset_Y = this.positionOffset_Y;
      this.toPositionOffset_X = newPositionOffset_X;
      this.toPositionOffset_Y = newPositionOffset_Y;
    }

    public void lerpPositionScale(float newPositionScale_X, float newPositionScale_Y, ESleekLerp lerp, float time)
    {
      this.isLerpingPositionScale = true;
      this.positionScaleLerp = lerp;
      this.positionScaleLerpTime = time;
      this.positionScaleLerpTicked = Time.realtimeSinceStartup;
      this.fromPositionScale_X = this.positionScale_X;
      this.fromPositionScale_Y = this.positionScale_Y;
      this.toPositionScale_X = newPositionScale_X;
      this.toPositionScale_Y = newPositionScale_Y;
    }

    public void lerpSizeOffset(int newSizeOffset_X, int newSizeOffset_Y, ESleekLerp lerp, float time)
    {
      this.isLerpingSizeOffset = true;
      this.sizeOffsetLerp = lerp;
      this.sizeOffsetLerpTime = time;
      this.sizeOffsetLerpTicked = Time.realtimeSinceStartup;
      this.fromSizeOffset_X = this.sizeOffset_X;
      this.fromSizeOffset_Y = this.sizeOffset_Y;
      this.toSizeOffset_X = newSizeOffset_X;
      this.toSizeOffset_Y = newSizeOffset_Y;
    }

    public void lerpSizeScale(float newSizeScale_X, float newSizeScale_Y, ESleekLerp lerp, float time)
    {
      this.isLerpingSizeScale = true;
      this.sizeScaleLerp = lerp;
      this.sizeScaleLerpTime = time;
      this.sizeScaleLerpTicked = Time.realtimeSinceStartup;
      this.fromSizeScale_X = this.sizeScale_X;
      this.fromSizeScale_Y = this.sizeScale_Y;
      this.toSizeScale_X = newSizeScale_X;
      this.toSizeScale_Y = newSizeScale_Y;
    }

    public bool isOnScreen()
    {
      if (this.parent == null)
        return true;
      if (Screen.width == 5760 && Screen.height == 1080)
      {
        if ((double) this.frame.xMax < 1920.0 || (double) this.frame.yMax < 0.0 || ((double) this.frame.xMin > 3840.0 || (double) this.frame.yMin > 1080.0))
          return false;
      }
      else if ((double) this.frame.xMax < 0.0 || (double) this.frame.yMax < 0.0 || ((double) this.frame.xMin > (double) Screen.width || (double) this.frame.yMin > (double) Screen.height))
        return false;
      return true;
    }

    public void build()
    {
      this._frame = this.calculate();
      for (int index = 0; index < this.children.Count; ++index)
        this.children[index].build();
    }

    public void add(Sleek sleek)
    {
      this.children.Add(sleek);
      sleek._parent = this;
      sleek.build();
    }

    public void addLabel(string text, ESleekSide side)
    {
      this.addLabel(text, Color.white, side);
    }

    public void addLabel(string text, Color color, ESleekSide side)
    {
      this.sideLabel = new SleekLabel();
      if (side == ESleekSide.LEFT)
      {
        this.sideLabel.positionOffset_X = -205;
        this.sideLabel.fontAlignment = TextAnchor.MiddleRight;
      }
      else if (side == ESleekSide.RIGHT)
      {
        this.sideLabel.positionOffset_X = 5;
        this.sideLabel.positionScale_X = 1f;
        this.sideLabel.fontAlignment = TextAnchor.MiddleLeft;
      }
      this.sideLabel.positionOffset_Y = -15;
      this.sideLabel.positionScale_Y = 0.5f;
      this.sideLabel.sizeOffset_X = 200;
      this.sideLabel.sizeOffset_Y = 30;
      this.sideLabel.foregroundColor = color;
      this.sideLabel.text = text;
      this.add((Sleek) this.sideLabel);
    }

    public void updateLabel(string text)
    {
      this.sideLabel.text = text;
    }

    public int search(Sleek sleek)
    {
      return this.children.IndexOf(sleek);
    }

    public void remove(Sleek sleek)
    {
      this.children.Remove(sleek);
      sleek._parent = (Sleek) null;
    }

    public void remove()
    {
      this.children.Clear();
    }

    protected void update()
    {
      if (Event.current.type != EventType.Repaint)
        return;
      if (this.isLerpingPositionOffset)
      {
        if (this.positionOffsetLerp == ESleekLerp.LINEAR)
        {
          if ((double) Time.realtimeSinceStartup - (double) this.positionOffsetLerpTicked > (double) this.positionOffsetLerpTime)
          {
            this.isLerpingPositionOffset = false;
            this.positionOffset_X = this.toPositionOffset_X;
            this.positionOffset_Y = this.toPositionOffset_Y;
          }
          else
          {
            this.positionOffset_X = (int) Mathf.Lerp((float) this.fromPositionOffset_X, (float) this.toPositionOffset_X, (Time.realtimeSinceStartup - this.positionOffsetLerpTicked) / this.positionOffsetLerpTime);
            this.positionOffset_Y = (int) Mathf.Lerp((float) this.fromPositionOffset_Y, (float) this.toPositionOffset_Y, (Time.realtimeSinceStartup - this.positionOffsetLerpTicked) / this.positionOffsetLerpTime);
          }
        }
        else if (this.positionOffsetLerp == ESleekLerp.EXPONENTIAL)
        {
          if (Mathf.Abs(this.toPositionOffset_X - this.positionOffset_X) < 10 && Mathf.Abs(this.toPositionOffset_Y - this.positionOffset_Y) < 10)
          {
            this.isLerpingPositionOffset = false;
            this.positionOffset_X = this.toPositionOffset_X;
            this.positionOffset_Y = this.toPositionOffset_Y;
          }
          else
          {
            this.positionOffset_X = (int) Mathf.Lerp((float) this.positionOffset_X, (float) this.toPositionOffset_X, (Time.realtimeSinceStartup - this.positionOffsetLerpTicked) * this.positionOffsetLerpTime);
            this.positionOffset_Y = (int) Mathf.Lerp((float) this.positionOffset_Y, (float) this.toPositionOffset_Y, (Time.realtimeSinceStartup - this.positionOffsetLerpTicked) * this.positionOffsetLerpTime);
            this.positionOffsetLerpTicked = Time.realtimeSinceStartup;
          }
        }
      }
      if (this.isLerpingPositionScale)
      {
        if (this.positionScaleLerp == ESleekLerp.LINEAR)
        {
          if ((double) Time.realtimeSinceStartup - (double) this.positionScaleLerpTicked > (double) this.positionScaleLerpTime)
          {
            this.isLerpingPositionScale = false;
            this.positionScale_X = this.toPositionScale_X;
            this.positionScale_Y = this.toPositionScale_Y;
          }
          else
          {
            this.positionScale_X = Mathf.Lerp(this.fromPositionScale_X, this.toPositionScale_X, (Time.realtimeSinceStartup - this.positionScaleLerpTicked) / this.positionScaleLerpTime);
            this.positionScale_Y = Mathf.Lerp(this.fromPositionScale_Y, this.toPositionScale_Y, (Time.realtimeSinceStartup - this.positionScaleLerpTicked) / this.positionScaleLerpTime);
          }
        }
        else if (this.positionScaleLerp == ESleekLerp.EXPONENTIAL)
        {
          if ((double) Mathf.Abs(this.toPositionScale_X - this.positionScale_X) < 0.00999999977648258 && (double) Mathf.Abs(this.toPositionScale_Y - this.positionScale_Y) < 0.00999999977648258)
          {
            this.isLerpingPositionScale = false;
            this.positionScale_X = this.toPositionScale_X;
            this.positionScale_Y = this.toPositionScale_Y;
          }
          else
          {
            this.positionScale_X = Mathf.Lerp(this.positionScale_X, this.toPositionScale_X, (Time.realtimeSinceStartup - this.positionScaleLerpTicked) * this.positionScaleLerpTime);
            this.positionScale_Y = Mathf.Lerp(this.positionScale_Y, this.toPositionScale_Y, (Time.realtimeSinceStartup - this.positionScaleLerpTicked) * this.positionScaleLerpTime);
            this.positionScaleLerpTicked = Time.realtimeSinceStartup;
          }
        }
      }
      if (this.isLerpingSizeOffset)
      {
        if (this.sizeOffsetLerp == ESleekLerp.LINEAR)
        {
          if ((double) Time.realtimeSinceStartup - (double) this.sizeOffsetLerpTicked > (double) this.sizeOffsetLerpTime)
          {
            this.isLerpingSizeOffset = false;
            this.sizeOffset_X = this.toSizeOffset_X;
            this.sizeOffset_Y = this.toSizeOffset_Y;
          }
          else
          {
            this.sizeOffset_X = (int) Mathf.Lerp((float) this.fromSizeOffset_X, (float) this.toSizeOffset_X, (Time.realtimeSinceStartup - this.sizeOffsetLerpTicked) / this.sizeOffsetLerpTime);
            this.sizeOffset_Y = (int) Mathf.Lerp((float) this.fromSizeOffset_Y, (float) this.toSizeOffset_Y, (Time.realtimeSinceStartup - this.sizeOffsetLerpTicked) / this.sizeOffsetLerpTime);
          }
        }
        else if (this.sizeOffsetLerp == ESleekLerp.EXPONENTIAL)
        {
          if (Mathf.Abs(this.toSizeOffset_X - this.sizeOffset_X) < 10 && Mathf.Abs(this.toSizeOffset_Y - this.sizeOffset_Y) < 10)
          {
            this.isLerpingSizeOffset = false;
            this.sizeOffset_X = this.toSizeOffset_X;
            this.sizeOffset_Y = this.toSizeOffset_Y;
          }
          else
          {
            this.sizeOffset_X = (int) Mathf.Lerp((float) this.sizeOffset_X, (float) this.toSizeOffset_X, (Time.realtimeSinceStartup - this.sizeOffsetLerpTicked) * this.sizeOffsetLerpTime);
            this.sizeOffset_Y = (int) Mathf.Lerp((float) this.sizeOffset_Y, (float) this.toSizeOffset_Y, (Time.realtimeSinceStartup - this.sizeOffsetLerpTicked) * this.sizeOffsetLerpTime);
            this.sizeOffsetLerpTicked = Time.realtimeSinceStartup;
          }
        }
      }
      if (this.isLerpingSizeScale)
      {
        if (this.sizeScaleLerp == ESleekLerp.LINEAR)
        {
          if ((double) Time.realtimeSinceStartup - (double) this.sizeScaleLerpTicked > (double) this.sizeScaleLerpTime)
          {
            this.isLerpingSizeScale = false;
            this.sizeScale_X = this.toSizeScale_X;
            this.sizeScale_Y = this.toSizeScale_Y;
          }
          else
          {
            this.sizeScale_X = Mathf.Lerp(this.fromSizeScale_X, this.toSizeScale_X, (Time.realtimeSinceStartup - this.sizeScaleLerpTicked) / this.sizeScaleLerpTime);
            this.sizeScale_Y = Mathf.Lerp(this.fromSizeScale_Y, this.toSizeScale_Y, (Time.realtimeSinceStartup - this.sizeScaleLerpTicked) / this.sizeScaleLerpTime);
          }
        }
        else if (this.sizeScaleLerp == ESleekLerp.EXPONENTIAL)
        {
          if ((double) Mathf.Abs(this.toSizeScale_X - this.sizeScale_X) < 0.00999999977648258 && (double) Mathf.Abs(this.toSizeScale_Y - this.sizeScale_Y) < 0.00999999977648258)
          {
            this.isLerpingSizeScale = false;
            this.sizeScale_X = this.toSizeScale_X;
            this.sizeScale_Y = this.toSizeScale_Y;
          }
          else
          {
            this.sizeScale_X = Mathf.Lerp(this.sizeScale_X, this.toSizeScale_X, (Time.realtimeSinceStartup - this.sizeScaleLerpTicked) * this.sizeScaleLerpTime);
            this.sizeScale_Y = Mathf.Lerp(this.sizeScale_Y, this.toSizeScale_Y, (Time.realtimeSinceStartup - this.sizeScaleLerpTicked) * this.sizeScaleLerpTime);
            this.sizeScaleLerpTicked = Time.realtimeSinceStartup;
          }
        }
      }
      if (!this.needsFrame)
        return;
      this.needsFrame = false;
      this.build();
    }

    protected void init()
    {
      this.backgroundColor = Color.white;
      this.foregroundColor = Color.white;
      this.isVisible = true;
      this.isInputable = true;
      this._children = new List<Sleek>();
      this.build();
    }
  }
}
