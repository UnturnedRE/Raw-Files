// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.SleekToggle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

namespace SDG.Unturned
{
  public class SleekToggle : Sleek
  {
    public Toggled onToggled;
    public bool state;

    public SleekToggle()
    {
      this.init();
    }

    public override void draw(bool ignoreCulling)
    {
      bool state = SleekRender.drawToggle(this.frame, this.backgroundColor, this.state);
      if (state != this.state && this.onToggled != null)
        this.onToggled(this, state);
      this.state = state;
      this.drawChildren(ignoreCulling);
    }
  }
}
