// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.Regions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class Regions
  {
    public static readonly byte WORLD_SIZE = (byte) 64;
    public static readonly byte REGION_SIZE = (byte) (8192U / (uint) Regions.WORLD_SIZE);

    public static bool tryGetCoordinate(Vector3 point, out byte x, out byte y)
    {
      x = byte.MaxValue;
      y = byte.MaxValue;
      if (!Regions.checkSafe(point))
        return false;
      x = (byte) (((double) point.x + 4096.0) / (double) Regions.REGION_SIZE);
      y = (byte) (((double) point.z + 4096.0) / (double) Regions.REGION_SIZE);
      return true;
    }

    public static bool tryGetPoint(byte x, byte y, out Vector3 point)
    {
      point = Vector3.zero;
      if (!Regions.checkSafe(x, y))
        return false;
      point.x = (float) ((int) x * (int) Regions.REGION_SIZE - 4096);
      point.z = (float) ((int) y * (int) Regions.REGION_SIZE - 4096);
      return true;
    }

    public static bool checkSafe(Vector3 point)
    {
      return (double) point.x >= -4096.0 && (double) point.z >= -4096.0 && ((double) point.x < 4096.0 && (double) point.z < 4096.0);
    }

    public static bool checkSafe(byte x, byte y)
    {
      return (int) x >= 0 && (int) y >= 0 && ((int) x < (int) Regions.WORLD_SIZE && (int) y < (int) Regions.WORLD_SIZE);
    }

    public static bool checkArea(byte x_0, byte y_0, byte x_1, byte y_1, byte area)
    {
      return (int) x_0 >= (int) x_1 - (int) area && (int) y_0 >= (int) y_1 - (int) area && ((int) x_0 <= (int) x_1 + (int) area && (int) y_0 <= (int) y_1 + (int) area);
    }
  }
}
