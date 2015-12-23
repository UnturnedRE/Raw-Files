// Decompiled with JetBrains decompiler
// Type: Pathfinding.AstarSplines
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

namespace Pathfinding
{
  internal class AstarSplines
  {
    public static Vector3 CatmullRom(Vector3 previous, Vector3 start, Vector3 end, Vector3 next, float elapsedTime)
    {
      float num1 = elapsedTime;
      float num2 = num1 * num1;
      float num3 = num2 * num1;
      return previous * (float) (-0.5 * (double) num3 + (double) num2 - 0.5 * (double) num1) + start * (float) (1.5 * (double) num3 + -2.5 * (double) num2 + 1.0) + end * (float) (-1.5 * (double) num3 + 2.0 * (double) num2 + 0.5 * (double) num1) + next * (float) (0.5 * (double) num3 - 0.5 * (double) num2);
    }

    public static Vector3 CatmullRomOLD(Vector3 previous, Vector3 start, Vector3 end, Vector3 next, float elapsedTime)
    {
      float num1 = elapsedTime;
      float num2 = num1 * num1;
      float num3 = num2 * num1;
      return previous * (float) (-0.5 * (double) num3 + (double) num2 - 0.5 * (double) num1) + start * (float) (1.5 * (double) num3 + -2.5 * (double) num2 + 1.0) + end * (float) (-1.5 * (double) num3 + 2.0 * (double) num2 + 0.5 * (double) num1) + next * (float) (0.5 * (double) num3 - 0.5 * (double) num2);
    }
  }
}
