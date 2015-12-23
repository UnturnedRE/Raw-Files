// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.GraphicsSettingsResolution
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class GraphicsSettingsResolution
  {
    public int Width { get; set; }

    public int Height { get; set; }

    public GraphicsSettingsResolution(Resolution resolution)
    {
      this.Width = resolution.width;
      this.Height = resolution.height;
    }

    public GraphicsSettingsResolution()
      : this(Screen.resolutions[Screen.resolutions.Length - 1])
    {
    }
  }
}
