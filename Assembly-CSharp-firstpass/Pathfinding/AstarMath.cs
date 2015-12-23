// Decompiled with JetBrains decompiler
// Type: Pathfinding.AstarMath
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

namespace Pathfinding
{
  public class AstarMath
  {
    public static int ComputeVertexHash(int x, int y, int z)
    {
      uint num1 = 2376512323U;
      uint num2 = 3625334849U;
      uint num3 = 3407524639U;
      return (int) (uint) ((long) num1 * (long) x + (long) num2 * (long) y + (long) num3 * (long) z) & 1073741823;
    }

    public static Vector3 NearestPoint(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
    {
      Vector3 rhs = Vector3.Normalize(lineEnd - lineStart);
      float num = Vector3.Dot(point - lineStart, rhs);
      return lineStart + num * rhs;
    }

    public static float NearestPointFactor(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
    {
      Vector3 vector3 = lineEnd - lineStart;
      float magnitude = vector3.magnitude;
      Vector3 rhs = vector3 / magnitude;
      return Vector3.Dot(point - lineStart, rhs) / magnitude;
    }

    public static float NearestPointFactor(Int3 lineStart, Int3 lineEnd, Int3 point)
    {
      Int3 rhs = lineEnd - lineStart;
      float sqrMagnitude = rhs.sqrMagnitude;
      return (float) Int3.Dot(point - lineStart, rhs) / sqrMagnitude;
    }

    public static float NearestPointFactor(Int2 lineStart, Int2 lineEnd, Int2 point)
    {
      Int2 b = lineEnd - lineStart;
      double num = (double) b.sqrMagnitudeLong;
      return (float) Int2.DotLong(point - lineStart, b) / (float) num;
    }

    public static Vector3 NearestPointStrict(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
    {
      Vector3 vector3 = lineEnd - lineStart;
      float magnitude = vector3.magnitude;
      Vector3 rhs = vector3 / magnitude;
      float num = Vector3.Dot(point - lineStart, rhs);
      return lineStart + Mathf.Clamp(num, 0.0f, magnitude) * rhs;
    }

    public static Vector3 NearestPointStrictXZ(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
    {
      lineStart.y = point.y;
      lineEnd.y = point.y;
      Vector3 vector3 = lineEnd - lineStart;
      vector3.y = 0.0f;
      Vector3 rhs = Vector3.Normalize(vector3);
      float num = Vector3.Dot(point - lineStart, rhs);
      return lineStart + Mathf.Clamp(num, 0.0f, vector3.magnitude) * rhs;
    }

    public static float DistancePointSegment(int x, int z, int px, int pz, int qx, int qz)
    {
      float num1 = (float) (qx - px);
      float num2 = (float) (qz - pz);
      float num3 = (float) (x - px);
      float num4 = (float) (z - pz);
      float num5 = (float) ((double) num1 * (double) num1 + (double) num2 * (double) num2);
      float num6 = (float) ((double) num1 * (double) num3 + (double) num2 * (double) num4);
      if ((double) num5 > 0.0)
        num6 /= num5;
      if ((double) num6 < 0.0)
        num6 = 0.0f;
      else if ((double) num6 > 1.0)
        num6 = 1f;
      float num7 = (float) px + num6 * num1 - (float) x;
      float num8 = (float) pz + num6 * num2 - (float) z;
      return (float) ((double) num7 * (double) num7 + (double) num8 * (double) num8);
    }

    public static float DistancePointSegment(Int3 a, Int3 b, Int3 p)
    {
      float num1 = (float) (b.x - a.x);
      float num2 = (float) (b.z - a.z);
      float num3 = (float) (p.x - a.x);
      float num4 = (float) (p.z - a.z);
      float num5 = (float) ((double) num1 * (double) num1 + (double) num2 * (double) num2);
      float num6 = (float) ((double) num1 * (double) num3 + (double) num2 * (double) num4);
      if ((double) num5 > 0.0)
        num6 /= num5;
      if ((double) num6 < 0.0)
        num6 = 0.0f;
      else if ((double) num6 > 1.0)
        num6 = 1f;
      float num7 = (float) a.x + num6 * num1 - (float) p.x;
      float num8 = (float) a.z + num6 * num2 - (float) p.z;
      return (float) ((double) num7 * (double) num7 + (double) num8 * (double) num8);
    }

    public static float DistancePointSegment2(int x, int z, int px, int pz, int qx, int qz)
    {
      Vector3 p = new Vector3((float) x, 0.0f, (float) z);
      return AstarMath.DistancePointSegment2(new Vector3((float) px, 0.0f, (float) pz), new Vector3((float) qx, 0.0f, (float) qz), p);
    }

    public static float DistancePointSegment2(Vector3 a, Vector3 b, Vector3 p)
    {
      float num1 = b.x - a.x;
      float num2 = b.z - a.z;
      float num3 = Mathf.Abs((float) ((double) num1 * ((double) p.z - (double) a.z) - ((double) p.x - (double) a.x) * (double) num2));
      float f = (float) ((double) num1 * (double) num1 + (double) num2 * (double) num2);
      if ((double) f > 0.0)
        return num3 / Mathf.Sqrt(f);
      return (a - p).magnitude;
    }

    public static float DistancePointSegmentStrict(Vector3 a, Vector3 b, Vector3 p)
    {
      return (AstarMath.NearestPointStrict(a, b, p) - p).sqrMagnitude;
    }

    public static float Hermite(float start, float end, float value)
    {
      return Mathf.Lerp(start, end, (float) ((double) value * (double) value * (3.0 - 2.0 * (double) value)));
    }

    public static Vector3 CubicBezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
      t = Mathf.Clamp01(t);
      float num = 1f - t;
      return num * num * num * p0 + 3f * num * num * t * p1 + 3f * num * t * t * p2 + t * t * t * p3;
    }

    public static float MapTo(float startMin, float startMax, float value)
    {
      value -= startMin;
      value /= startMax - startMin;
      value = Mathf.Clamp01(value);
      return value;
    }

    public static float MapToRange(float targetMin, float targetMax, float value)
    {
      value *= targetMax - targetMin;
      value += targetMin;
      return value;
    }

    public static float MapTo(float startMin, float startMax, float targetMin, float targetMax, float value)
    {
      value -= startMin;
      value /= startMax - startMin;
      value = Mathf.Clamp01(value);
      value *= targetMax - targetMin;
      value += targetMin;
      return value;
    }

    public static string FormatBytes(int bytes)
    {
      double num = bytes < 0 ? -1.0 : 1.0;
      bytes = bytes < 0 ? -bytes : bytes;
      if (bytes < 1000)
        return ((double) bytes * num).ToString() + " bytes";
      if (bytes < 1000000)
        return ((double) bytes / 1000.0 * num).ToString("0.0") + " kb";
      if (bytes < 1000000000)
        return ((double) bytes / 1000000.0 * num).ToString("0.0") + " mb";
      return ((double) bytes / 1000000000.0 * num).ToString("0.0") + " gb";
    }

    public static string FormatBytesBinary(int bytes)
    {
      double num = bytes < 0 ? -1.0 : 1.0;
      bytes = bytes < 0 ? -bytes : bytes;
      if (bytes < 1024)
        return ((double) bytes * num).ToString() + " bytes";
      if (bytes < 1048576)
        return ((double) bytes / 1024.0 * num).ToString("0.0") + " kb";
      if (bytes < 1073741824)
        return ((double) bytes / 1048576.0 * num).ToString("0.0") + " mb";
      return ((double) bytes / 1073741824.0 * num).ToString("0.0") + " gb";
    }

    public static int Bit(int a, int b)
    {
      return a >> b & 1;
    }

    public static Color IntToColor(int i, float a)
    {
      return new Color((float) (AstarMath.Bit(i, 1) + AstarMath.Bit(i, 3) * 2 + 1) * 0.25f, (float) (AstarMath.Bit(i, 2) + AstarMath.Bit(i, 4) * 2 + 1) * 0.25f, (float) (AstarMath.Bit(i, 0) + AstarMath.Bit(i, 5) * 2 + 1) * 0.25f, a);
    }

    public static float MagnitudeXZ(Vector3 a, Vector3 b)
    {
      Vector3 vector3 = a - b;
      return (float) Math.Sqrt((double) vector3.x * (double) vector3.x + (double) vector3.z * (double) vector3.z);
    }

    public static float SqrMagnitudeXZ(Vector3 a, Vector3 b)
    {
      Vector3 vector3 = a - b;
      return (float) ((double) vector3.x * (double) vector3.x + (double) vector3.z * (double) vector3.z);
    }

    public static int Repeat(int i, int n)
    {
      while (i >= n)
        i -= n;
      return i;
    }

    public static float Abs(float a)
    {
      if ((double) a < 0.0)
        return -a;
      return a;
    }

    public static int Abs(int a)
    {
      if (a < 0)
        return -a;
      return a;
    }

    public static float Min(float a, float b)
    {
      if ((double) a < (double) b)
        return a;
      return b;
    }

    public static int Min(int a, int b)
    {
      if (a < b)
        return a;
      return b;
    }

    public static uint Min(uint a, uint b)
    {
      if (a < b)
        return a;
      return b;
    }

    public static float Max(float a, float b)
    {
      if ((double) a > (double) b)
        return a;
      return b;
    }

    public static int Max(int a, int b)
    {
      if (a > b)
        return a;
      return b;
    }

    public static uint Max(uint a, uint b)
    {
      if (a > b)
        return a;
      return b;
    }

    public static ushort Max(ushort a, ushort b)
    {
      if ((int) a > (int) b)
        return a;
      return b;
    }

    public static float Sign(float a)
    {
      return (double) a < 0.0 ? -1f : 1f;
    }

    public static int Sign(int a)
    {
      return a < 0 ? -1 : 1;
    }

    public static float Clamp(float a, float b, float c)
    {
      if ((double) a > (double) c)
        return c;
      if ((double) a < (double) b)
        return b;
      return a;
    }

    public static int Clamp(int a, int b, int c)
    {
      if (a > c)
        return c;
      if (a < b)
        return b;
      return a;
    }

    public static float Clamp01(float a)
    {
      if ((double) a > 1.0)
        return 1f;
      if ((double) a < 0.0)
        return 0.0f;
      return a;
    }

    public static int Clamp01(int a)
    {
      if (a > 1)
        return 1;
      if (a < 0)
        return 0;
      return a;
    }

    public static float Lerp(float a, float b, float t)
    {
      return a + (float) (((double) b - (double) a) * ((double) t <= 1.0 ? ((double) t >= 0.0 ? (double) t : 0.0) : 1.0));
    }

    public static int RoundToInt(float v)
    {
      return (int) ((double) v + 0.5);
    }

    public static int RoundToInt(double v)
    {
      return (int) (v + 0.5);
    }
  }
}
