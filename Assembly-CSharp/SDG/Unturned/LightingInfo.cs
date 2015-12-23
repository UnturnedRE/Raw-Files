// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.LightingInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class LightingInfo
  {
    private Color[] _colors;
    private float[] _singles;

    public Color[] colors
    {
      get
      {
        return this._colors;
      }
    }

    public float[] singles
    {
      get
      {
        return this._singles;
      }
    }

    public LightingInfo(Color[] newColors, float[] newSingles)
    {
      this._colors = newColors;
      this._singles = newSingles;
    }
  }
}
