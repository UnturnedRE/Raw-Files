// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.MeasurementTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

namespace SDG.Unturned
{
  public class MeasurementTool
  {
    public static float speedToKPH(float speed)
    {
      return speed * 3.6f;
    }

    public static float KPHToMPH(float kph)
    {
      return kph / 1.609344f;
    }

    public static float KtoM(float k)
    {
      return k * 0.621371f;
    }

    public static float MtoYd(float m)
    {
      return m * 1.09361f;
    }

    public static byte angleToByte(float angle)
    {
      if ((double) angle < 0.0)
        return (byte) ((360.0 + (double) angle % 360.0) / 2.0);
      return (byte) ((double) angle % 360.0 / 2.0);
    }
  }
}
