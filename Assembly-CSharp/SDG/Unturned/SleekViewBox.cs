// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.SleekViewBox
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class SleekViewBox : Sleek
  {
    public Rect area;
    public Vector2 state;

    public SleekViewBox()
    {
      this.state = new Vector2(0.0f, 0.0f);
      this.local = true;
      this.init();
    }

    public override void draw(bool ignoreCulling)
    {
      this.state = GUI.BeginScrollView(this.frame, this.state, this.area);
      this.drawChildren(ignoreCulling);
      GUI.EndScrollView(false);
    }
  }
}
