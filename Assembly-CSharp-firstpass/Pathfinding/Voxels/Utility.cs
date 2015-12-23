// Decompiled with JetBrains decompiler
// Type: Pathfinding.Voxels.Utility
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding;
using UnityEngine;

namespace Pathfinding.Voxels
{
  public class Utility
  {
    public static Color[] colors = new Color[7]
    {
      Color.green,
      Color.blue,
      Color.red,
      Color.yellow,
      Color.cyan,
      Color.white,
      Color.black
    };
    private static float[] clipPolygonCache = new float[21];
    private static int[] clipPolygonIntCache = new int[21];
    public static float lastStartTime;
    public static float lastAdditiveTimerStart;
    public static float additiveTimer;

    public static Color GetColor(int i)
    {
      while (i >= Utility.colors.Length)
        i -= Utility.colors.Length;
      while (i < 0)
        i += Utility.colors.Length;
      return Utility.colors[i];
    }

    public static int Bit(int a, int b)
    {
      return (a & 1 << b) >> b;
    }

    public static Color IntToColor(int i, float a)
    {
      return new Color((float) (Utility.Bit(i, 1) + Utility.Bit(i, 3) * 2 + 1) * 0.25f, (float) (Utility.Bit(i, 2) + Utility.Bit(i, 4) * 2 + 1) * 0.25f, (float) (Utility.Bit(i, 0) + Utility.Bit(i, 5) * 2 + 1) * 0.25f, a);
    }

    public static float TriangleArea2(Vector3 a, Vector3 b, Vector3 c)
    {
      return Mathf.Abs((float) ((double) a.x * (double) b.z + (double) b.x * (double) c.z + (double) c.x * (double) a.z - (double) a.x * (double) c.z - (double) c.x * (double) b.z - (double) b.x * (double) a.z));
    }

    public static float TriangleArea(Vector3 a, Vector3 b, Vector3 c)
    {
      return (float) (((double) b.x - (double) a.x) * ((double) c.z - (double) a.z) - ((double) c.x - (double) a.x) * ((double) b.z - (double) a.z));
    }

    public static float Min(float a, float b, float c)
    {
      a = (double) a >= (double) b ? b : a;
      if ((double) a < (double) c)
        return a;
      return c;
    }

    public static float Max(float a, float b, float c)
    {
      a = (double) a <= (double) b ? b : a;
      if ((double) a > (double) c)
        return a;
      return c;
    }

    public static int Max(int a, int b, int c, int d)
    {
      a = a <= b ? b : a;
      a = a <= c ? c : a;
      if (a > d)
        return a;
      return d;
    }

    public static int Min(int a, int b, int c, int d)
    {
      a = a >= b ? b : a;
      a = a >= c ? c : a;
      if (a < d)
        return a;
      return d;
    }

    public static float Max(float a, float b, float c, float d)
    {
      a = (double) a <= (double) b ? b : a;
      a = (double) a <= (double) c ? c : a;
      if ((double) a > (double) d)
        return a;
      return d;
    }

    public static float Min(float a, float b, float c, float d)
    {
      a = (double) a >= (double) b ? b : a;
      a = (double) a >= (double) c ? c : a;
      if ((double) a < (double) d)
        return a;
      return d;
    }

    public static string ToMillis(float v)
    {
      return (v * 1000f).ToString("0");
    }

    public static void StartTimer()
    {
      Utility.lastStartTime = Time.realtimeSinceStartup;
    }

    public static void EndTimer(string label)
    {
      Debug.Log((object) (label + ", process took " + Utility.ToMillis(Time.realtimeSinceStartup - Utility.lastStartTime) + "ms to complete"));
    }

    public static void StartTimerAdditive(bool reset)
    {
      if (reset)
        Utility.additiveTimer = 0.0f;
      Utility.lastAdditiveTimerStart = Time.realtimeSinceStartup;
    }

    public static void EndTimerAdditive(string label, bool log)
    {
      Utility.additiveTimer += Time.realtimeSinceStartup - Utility.lastAdditiveTimerStart;
      if (log)
        Debug.Log((object) (label + ", process took " + Utility.ToMillis(Utility.additiveTimer) + "ms to complete"));
      Utility.lastAdditiveTimerStart = Time.realtimeSinceStartup;
    }

    public static void CopyVector(float[] a, int i, Vector3 v)
    {
      a[i] = v.x;
      a[i + 1] = v.y;
      a[i + 2] = v.z;
    }

    public static int ClipPoly(float[] vIn, int n, float[] vOut, float pnx, float pnz, float pd)
    {
      float[] numArray = Utility.clipPolygonCache;
      for (int index = 0; index < n; ++index)
        numArray[index] = (float) ((double) pnx * (double) vIn[index * 3] + (double) pnz * (double) vIn[index * 3 + 2]) + pd;
      int num1 = 0;
      int index1 = 0;
      int index2 = n - 1;
      for (; index1 < n; ++index1)
      {
        bool flag1 = (double) numArray[index2] >= 0.0;
        bool flag2 = (double) numArray[index1] >= 0.0;
        if (flag1 != flag2)
        {
          float num2 = numArray[index2] / (numArray[index2] - numArray[index1]);
          vOut[num1 * 3] = vIn[index2 * 3] + (vIn[index1 * 3] - vIn[index2 * 3]) * num2;
          vOut[num1 * 3 + 1] = vIn[index2 * 3 + 1] + (vIn[index1 * 3 + 1] - vIn[index2 * 3 + 1]) * num2;
          vOut[num1 * 3 + 2] = vIn[index2 * 3 + 2] + (vIn[index1 * 3 + 2] - vIn[index2 * 3 + 2]) * num2;
          ++num1;
        }
        if (flag2)
        {
          vOut[num1 * 3] = vIn[index1 * 3];
          vOut[num1 * 3 + 1] = vIn[index1 * 3 + 1];
          vOut[num1 * 3 + 2] = vIn[index1 * 3 + 2];
          ++num1;
        }
        index2 = index1;
      }
      return num1;
    }

    public static int ClipPolygon(float[] vIn, int n, float[] vOut, float multi, float offset, int axis)
    {
      float[] numArray = Utility.clipPolygonCache;
      for (int index = 0; index < n; ++index)
        numArray[index] = multi * vIn[index * 3 + axis] + offset;
      int num1 = 0;
      int index1 = 0;
      int index2 = n - 1;
      for (; index1 < n; ++index1)
      {
        bool flag1 = (double) numArray[index2] >= 0.0;
        bool flag2 = (double) numArray[index1] >= 0.0;
        if (flag1 != flag2)
        {
          int index3 = num1 * 3;
          int index4 = index1 * 3;
          int index5 = index2 * 3;
          float num2 = numArray[index2] / (numArray[index2] - numArray[index1]);
          vOut[index3] = vIn[index5] + (vIn[index4] - vIn[index5]) * num2;
          vOut[index3 + 1] = vIn[index5 + 1] + (vIn[index4 + 1] - vIn[index5 + 1]) * num2;
          vOut[index3 + 2] = vIn[index5 + 2] + (vIn[index4 + 2] - vIn[index5 + 2]) * num2;
          ++num1;
        }
        if (flag2)
        {
          int index3 = num1 * 3;
          int index4 = index1 * 3;
          vOut[index3] = vIn[index4];
          vOut[index3 + 1] = vIn[index4 + 1];
          vOut[index3 + 2] = vIn[index4 + 2];
          ++num1;
        }
        index2 = index1;
      }
      return num1;
    }

    public static int ClipPolygonY(float[] vIn, int n, float[] vOut, float multi, float offset, int axis)
    {
      float[] numArray = Utility.clipPolygonCache;
      for (int index = 0; index < n; ++index)
        numArray[index] = multi * vIn[index * 3 + axis] + offset;
      int num = 0;
      int index1 = 0;
      int index2 = n - 1;
      for (; index1 < n; ++index1)
      {
        bool flag1 = (double) numArray[index2] >= 0.0;
        bool flag2 = (double) numArray[index1] >= 0.0;
        if (flag1 != flag2)
        {
          vOut[num * 3 + 1] = vIn[index2 * 3 + 1] + (float) (((double) vIn[index1 * 3 + 1] - (double) vIn[index2 * 3 + 1]) * ((double) numArray[index2] / ((double) numArray[index2] - (double) numArray[index1])));
          ++num;
        }
        if (flag2)
        {
          vOut[num * 3 + 1] = vIn[index1 * 3 + 1];
          ++num;
        }
        index2 = index1;
      }
      return num;
    }

    public static int ClipPolygon(Vector3[] vIn, int n, Vector3[] vOut, float multi, float offset, int axis)
    {
      float[] numArray = Utility.clipPolygonCache;
      for (int index = 0; index < n; ++index)
        numArray[index] = multi * vIn[index][axis] + offset;
      int index1 = 0;
      int index2 = 0;
      int index3 = n - 1;
      for (; index2 < n; ++index2)
      {
        bool flag1 = (double) numArray[index3] >= 0.0;
        bool flag2 = (double) numArray[index2] >= 0.0;
        if (flag1 != flag2)
        {
          float num = numArray[index3] / (numArray[index3] - numArray[index2]);
          vOut[index1] = vIn[index3] + (vIn[index2] - vIn[index3]) * num;
          ++index1;
        }
        if (flag2)
        {
          vOut[index1] = vIn[index2];
          ++index1;
        }
        index3 = index2;
      }
      return index1;
    }

    public static int ClipPolygon(Int3[] vIn, int n, Int3[] vOut, int multi, int offset, int axis)
    {
      int[] numArray = Utility.clipPolygonIntCache;
      for (int index = 0; index < n; ++index)
        numArray[index] = multi * vIn[index][axis] + offset;
      int index1 = 0;
      int index2 = 0;
      int index3 = n - 1;
      for (; index2 < n; ++index2)
      {
        bool flag1 = numArray[index3] >= 0;
        bool flag2 = numArray[index2] >= 0;
        if (flag1 != flag2)
        {
          double num = (double) numArray[index3] / (double) (numArray[index3] - numArray[index2]);
          vOut[index1] = vIn[index3] + (vIn[index2] - vIn[index3]) * num;
          ++index1;
        }
        if (flag2)
        {
          vOut[index1] = vIn[index2];
          ++index1;
        }
        index3 = index2;
      }
      return index1;
    }

    public static bool IntersectXAxis(out Vector3 intersection, Vector3 start1, Vector3 dir1, float x)
    {
      float num1 = dir1.x;
      if ((double) num1 == 0.0)
      {
        intersection = Vector3.zero;
        return false;
      }
      float num2 = Mathf.Clamp01((x - start1.x) / num1);
      intersection = start1 + dir1 * num2;
      return true;
    }

    public static bool IntersectZAxis(out Vector3 intersection, Vector3 start1, Vector3 dir1, float z)
    {
      float num1 = -dir1.z;
      if ((double) num1 == 0.0)
      {
        intersection = Vector3.zero;
        return false;
      }
      float num2 = Mathf.Clamp01((start1.z - z) / num1);
      intersection = start1 + dir1 * num2;
      return true;
    }
  }
}
