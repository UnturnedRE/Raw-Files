// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.SleekProgress
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class SleekProgress : Sleek
  {
    private SleekImageTexture background;
    private SleekImageTexture foreground;
    private SleekLabel label;
    private string suffix;

    public float state
    {
      get
      {
        return this.foreground.sizeScale_X;
      }
      set
      {
        this.foreground.sizeScale_X = value;
        if (this.suffix.Length != 0)
          return;
        this.label.text = (string) (object) (int) ((double) value * 100.0) + (object) "%";
      }
    }

    public int measure
    {
      set
      {
        if (this.suffix.Length == 0)
          return;
        this.label.text = (string) (object) value + (object) this.suffix;
      }
    }

    public Color color
    {
      get
      {
        return this.foreground.backgroundColor;
      }
      set
      {
        this.background.backgroundColor = value;
        this.background.backgroundColor.a = 0.5f;
        this.foreground.backgroundColor = value;
      }
    }

    public SleekProgress(string newSuffix)
    {
      this.init();
      this.background = new SleekImageTexture();
      this.background.sizeScale_X = 1f;
      this.background.sizeScale_Y = 1f;
      this.background.texture = (Texture) Resources.Load("Materials/Pixel");
      this.add((Sleek) this.background);
      this.foreground = new SleekImageTexture();
      this.foreground.sizeScale_X = 1f;
      this.foreground.sizeScale_Y = 1f;
      this.foreground.texture = (Texture) Resources.Load("Materials/Pixel");
      this.add((Sleek) this.foreground);
      this.label = new SleekLabel();
      this.label.sizeScale_X = 1f;
      this.label.positionScale_Y = 0.5f;
      this.label.positionOffset_Y = -15;
      this.label.sizeOffset_Y = 30;
      this.add((Sleek) this.label);
      this.suffix = newSuffix;
    }

    public override void draw(bool ignoreCulling)
    {
      this.drawChildren(ignoreCulling);
    }
  }
}
