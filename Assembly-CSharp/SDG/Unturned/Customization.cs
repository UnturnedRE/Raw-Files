// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.Customization
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class Customization
  {
    public static readonly byte FREE_CHARACTERS = (byte) 1;
    public static readonly byte PRO_CHARACTERS = (byte) 4;
    public static readonly byte FACES_FREE = (byte) 10;
    public static readonly byte HAIRS_FREE = (byte) 5;
    public static readonly byte BEARDS_FREE = (byte) 5;
    public static readonly byte FACES_PRO = (byte) 10;
    public static readonly byte HAIRS_PRO = (byte) 7;
    public static readonly byte BEARDS_PRO = (byte) 4;
    public static readonly Color[] SKINS = new Color[10]
    {
      new Color(0.9568627f, 0.9019608f, 0.8235294f),
      new Color(0.8509804f, 0.7921569f, 0.7058824f),
      new Color(0.7450981f, 0.6470588f, 0.509804f),
      new Color(0.6156863f, 0.5333334f, 0.4196078f),
      new Color(0.5803922f, 0.4627451f, 0.2941177f),
      new Color(0.4392157f, 0.3764706f, 0.2862745f),
      new Color(0.3254902f, 0.2784314f, 0.2117647f),
      new Color(0.2941177f, 0.2392157f, 0.1921569f),
      new Color(0.2f, 0.172549f, 0.145098f),
      new Color(0.1372549f, 0.1215686f, 0.1098039f)
    };
    public static readonly Color[] COLORS = new Color[10]
    {
      new Color(0.8431373f, 0.8431373f, 0.8431373f),
      new Color(0.7568628f, 0.7568628f, 0.7568628f),
      new Color(0.8039216f, 0.7529412f, 0.5490196f),
      new Color(0.6745098f, 0.4156863f, 0.2235294f),
      new Color(0.4f, 0.3137255f, 0.2156863f),
      new Color(0.3411765f, 0.2705882f, 0.1843137f),
      new Color(0.2784314f, 0.2235294f, 0.1568628f),
      new Color(0.2078431f, 0.172549f, 0.1333333f),
      new Color(0.2156863f, 0.2156863f, 0.2156863f),
      new Color(0.09803922f, 0.09803922f, 0.09803922f)
    };

    public static bool checkSkin(Color color)
    {
      for (int index = 0; index < Customization.SKINS.Length; ++index)
      {
        if ((double) Mathf.Abs(color.r - Customization.SKINS[index].r) < 0.00999999977648258 && (double) Mathf.Abs(color.g - Customization.SKINS[index].g) < 0.00999999977648258 && (double) Mathf.Abs(color.b - Customization.SKINS[index].b) < 0.00999999977648258)
          return true;
      }
      return false;
    }

    public static bool checkColor(Color color)
    {
      for (int index = 0; index < Customization.COLORS.Length; ++index)
      {
        if ((double) Mathf.Abs(color.r - Customization.COLORS[index].r) < 0.00999999977648258 && (double) Mathf.Abs(color.g - Customization.COLORS[index].g) < 0.00999999977648258 && (double) Mathf.Abs(color.b - Customization.COLORS[index].b) < 0.00999999977648258)
          return true;
      }
      return false;
    }
  }
}
