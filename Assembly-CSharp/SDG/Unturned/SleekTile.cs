// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.SleekTile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class SleekTile : Sleek
  {
    public Texture texture;

    public SleekTile()
    {
      this.init();
    }

    public override void draw(bool ignoreCulling)
    {
      SleekRender.drawTile(this.frame, this.texture, this.backgroundColor);
      this.drawChildren(ignoreCulling);
    }
  }
}
