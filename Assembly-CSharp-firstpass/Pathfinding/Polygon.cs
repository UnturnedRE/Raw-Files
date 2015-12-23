// Decompiled with JetBrains decompiler
// Type: Pathfinding.Polygon
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
  public class Polygon
  {
    public static List<Vector3> hullCache = new List<Vector3>();

    public static long TriangleArea2(Int3 a, Int3 b, Int3 c)
    {
      return (long) (b.x - a.x) * (long) (c.z - a.z) - (long) (c.x - a.x) * (long) (b.z - a.z);
    }

    public static float TriangleArea2(Vector3 a, Vector3 b, Vector3 c)
    {
      return (float) (((double) b.x - (double) a.x) * ((double) c.z - (double) a.z) - ((double) c.x - (double) a.x) * ((double) b.z - (double) a.z));
    }

    public static long TriangleArea(Int3 a, Int3 b, Int3 c)
    {
      return (long) (b.x - a.x) * (long) (c.z - a.z) - (long) (c.x - a.x) * (long) (b.z - a.z);
    }

    public static float TriangleArea(Vector3 a, Vector3 b, Vector3 c)
    {
      return (float) (((double) b.x - (double) a.x) * ((double) c.z - (double) a.z) - ((double) c.x - (double) a.x) * ((double) b.z - (double) a.z));
    }

    public static bool ContainsPoint(Vector3 a, Vector3 b, Vector3 c, Vector3 p)
    {
      if (Polygon.IsClockwiseMargin(a, b, p) && Polygon.IsClockwiseMargin(b, c, p))
        return Polygon.IsClockwiseMargin(c, a, p);
      return false;
    }

    public static bool ContainsPoint(Int2 a, Int2 b, Int2 c, Int2 p)
    {
      if (Polygon.IsClockwiseMargin(a, b, p) && Polygon.IsClockwiseMargin(b, c, p))
        return Polygon.IsClockwiseMargin(c, a, p);
      return false;
    }

    public static bool ContainsPoint(Int3 a, Int3 b, Int3 c, Int3 p)
    {
      if (Polygon.IsClockwiseMargin(a, b, p) && Polygon.IsClockwiseMargin(b, c, p))
        return Polygon.IsClockwiseMargin(c, a, p);
      return false;
    }

    public static bool ContainsPoint(Vector2[] polyPoints, Vector2 p)
    {
      int index1 = polyPoints.Length - 1;
      bool flag = false;
      for (int index2 = 0; index2 < polyPoints.Length; index1 = index2++)
      {
        if (((double) polyPoints[index2].y <= (double) p.y && (double) p.y < (double) polyPoints[index1].y || (double) polyPoints[index1].y <= (double) p.y && (double) p.y < (double) polyPoints[index2].y) && (double) p.x < ((double) polyPoints[index1].x - (double) polyPoints[index2].x) * ((double) p.y - (double) polyPoints[index2].y) / ((double) polyPoints[index1].y - (double) polyPoints[index2].y) + (double) polyPoints[index2].x)
          flag = !flag;
      }
      return flag;
    }

    public static bool ContainsPoint(Vector3[] polyPoints, Vector3 p)
    {
      int index1 = polyPoints.Length - 1;
      bool flag = false;
      for (int index2 = 0; index2 < polyPoints.Length; index1 = index2++)
      {
        if (((double) polyPoints[index2].z <= (double) p.z && (double) p.z < (double) polyPoints[index1].z || (double) polyPoints[index1].z <= (double) p.z && (double) p.z < (double) polyPoints[index2].z) && (double) p.x < ((double) polyPoints[index1].x - (double) polyPoints[index2].x) * ((double) p.z - (double) polyPoints[index2].z) / ((double) polyPoints[index1].z - (double) polyPoints[index2].z) + (double) polyPoints[index2].x)
          flag = !flag;
      }
      return flag;
    }

    public static bool LeftNotColinear(Vector3 a, Vector3 b, Vector3 p)
    {
      return ((double) b.x - (double) a.x) * ((double) p.z - (double) a.z) - ((double) p.x - (double) a.x) * ((double) b.z - (double) a.z) < -1.40129846432482E-45;
    }

    public static bool Left(Vector3 a, Vector3 b, Vector3 p)
    {
      return ((double) b.x - (double) a.x) * ((double) p.z - (double) a.z) - ((double) p.x - (double) a.x) * ((double) b.z - (double) a.z) <= 0.0;
    }

    public static bool Left(Vector2 a, Vector2 b, Vector2 p)
    {
      return ((double) b.x - (double) a.x) * ((double) p.y - (double) a.y) - ((double) p.x - (double) a.x) * ((double) b.y - (double) a.y) <= 0.0;
    }

    public static bool Left(Int3 a, Int3 b, Int3 c)
    {
      return (long) (b.x - a.x) * (long) (c.z - a.z) - (long) (c.x - a.x) * (long) (b.z - a.z) <= 0L;
    }

    public static bool LeftNotColinear(Int3 a, Int3 b, Int3 c)
    {
      return (long) (b.x - a.x) * (long) (c.z - a.z) - (long) (c.x - a.x) * (long) (b.z - a.z) < 0L;
    }

    public static bool Left(Int2 a, Int2 b, Int2 c)
    {
      return (long) (b.x - a.x) * (long) (c.y - a.y) - (long) (c.x - a.x) * (long) (b.y - a.y) <= 0L;
    }

    public static bool IsClockwiseMargin(Vector3 a, Vector3 b, Vector3 c)
    {
      return ((double) b.x - (double) a.x) * ((double) c.z - (double) a.z) - ((double) c.x - (double) a.x) * ((double) b.z - (double) a.z) <= 1.40129846432482E-45;
    }

    public static bool IsClockwise(Vector3 a, Vector3 b, Vector3 c)
    {
      return ((double) b.x - (double) a.x) * ((double) c.z - (double) a.z) - ((double) c.x - (double) a.x) * ((double) b.z - (double) a.z) < 0.0;
    }

    public static bool IsClockwise(Int3 a, Int3 b, Int3 c)
    {
      return (long) (b.x - a.x) * (long) (c.z - a.z) - (long) (c.x - a.x) * (long) (b.z - a.z) < 0L;
    }

    public static bool IsClockwiseMargin(Int3 a, Int3 b, Int3 c)
    {
      return (long) (b.x - a.x) * (long) (c.z - a.z) - (long) (c.x - a.x) * (long) (b.z - a.z) <= 0L;
    }

    public static bool IsClockwiseMargin(Int2 a, Int2 b, Int2 c)
    {
      return (long) (b.x - a.x) * (long) (c.y - a.y) - (long) (c.x - a.x) * (long) (b.y - a.y) <= 0L;
    }

    public static bool IsColinear(Int3 a, Int3 b, Int3 c)
    {
      return (long) (b.x - a.x) * (long) (c.z - a.z) - (long) (c.x - a.x) * (long) (b.z - a.z) == 0L;
    }

    public static bool IsColinearAlmost(Int3 a, Int3 b, Int3 c)
    {
      long num = (long) (b.x - a.x) * (long) (c.z - a.z) - (long) (c.x - a.x) * (long) (b.z - a.z);
      if (num > -1L)
        return num < 1L;
      return false;
    }

    public static bool IsColinear(Vector3 a, Vector3 b, Vector3 c)
    {
      float num = (float) (((double) b.x - (double) a.x) * ((double) c.z - (double) a.z) - ((double) c.x - (double) a.x) * ((double) b.z - (double) a.z));
      if ((double) num <= 1.0000000116861E-07)
        return (double) num >= -1.0000000116861E-07;
      return false;
    }

    public static bool IntersectsUnclamped(Vector3 a, Vector3 b, Vector3 a2, Vector3 b2)
    {
      return Polygon.Left(a, b, a2) != Polygon.Left(a, b, b2);
    }

    public static bool Intersects(Int2 a, Int2 b, Int2 a2, Int2 b2)
    {
      if (Polygon.Left(a, b, a2) != Polygon.Left(a, b, b2))
        return Polygon.Left(a2, b2, a) != Polygon.Left(a2, b2, b);
      return false;
    }

    public static bool Intersects(Int3 a, Int3 b, Int3 a2, Int3 b2)
    {
      if (Polygon.Left(a, b, a2) != Polygon.Left(a, b, b2))
        return Polygon.Left(a2, b2, a) != Polygon.Left(a2, b2, b);
      return false;
    }

    public static bool Intersects(Vector3 start1, Vector3 end1, Vector3 start2, Vector3 end2)
    {
      Vector3 vector3_1 = end1 - start1;
      Vector3 vector3_2 = end2 - start2;
      float num1 = (float) ((double) vector3_2.z * (double) vector3_1.x - (double) vector3_2.x * (double) vector3_1.z);
      if ((double) num1 == 0.0)
        return false;
      float num2 = (float) ((double) vector3_2.x * ((double) start1.z - (double) start2.z) - (double) vector3_2.z * ((double) start1.x - (double) start2.x));
      float num3 = (float) ((double) vector3_1.x * ((double) start1.z - (double) start2.z) - (double) vector3_1.z * ((double) start1.x - (double) start2.x));
      float num4 = num2 / num1;
      float num5 = num3 / num1;
      return (double) num4 >= 0.0 && (double) num4 <= 1.0 && ((double) num5 >= 0.0 && (double) num5 <= 1.0);
    }

    public static Vector3 IntersectionPointOptimized(Vector3 start1, Vector3 dir1, Vector3 start2, Vector3 dir2)
    {
      float num1 = (float) ((double) dir2.z * (double) dir1.x - (double) dir2.x * (double) dir1.z);
      if ((double) num1 == 0.0)
        return start1;
      float num2 = (float) ((double) dir2.x * ((double) start1.z - (double) start2.z) - (double) dir2.z * ((double) start1.x - (double) start2.x)) / num1;
      return start1 + dir1 * num2;
    }

    public static Vector3 IntersectionPointOptimized(Vector3 start1, Vector3 dir1, Vector3 start2, Vector3 dir2, out bool intersects)
    {
      float num1 = (float) ((double) dir2.z * (double) dir1.x - (double) dir2.x * (double) dir1.z);
      if ((double) num1 == 0.0)
      {
        intersects = false;
        return start1;
      }
      float num2 = (float) ((double) dir2.x * ((double) start1.z - (double) start2.z) - (double) dir2.z * ((double) start1.x - (double) start2.x)) / num1;
      intersects = true;
      return start1 + dir1 * num2;
    }

    public static bool IntersectionFactorRaySegment(Int3 start1, Int3 end1, Int3 start2, Int3 end2)
    {
      Int3 int3_1 = end1 - start1;
      Int3 int3_2 = end2 - start2;
      long num1 = (long) (int3_2.z * int3_1.x - int3_2.x * int3_1.z);
      if (num1 == 0L)
        return false;
      long num2 = (long) (int3_2.x * (start1.z - start2.z) - int3_2.z * (start1.x - start2.x));
      long num3 = (long) (int3_1.x * (start1.z - start2.z) - int3_1.z * (start1.x - start2.x));
      return num2 < 0L ^ num1 < 0L && num3 < 0L ^ num1 < 0L && (num1 < 0L || num3 <= num1) && (num1 >= 0L || num3 > num1);
    }

    public static bool IntersectionFactor(Int3 start1, Int3 end1, Int3 start2, Int3 end2, out float factor1, out float factor2)
    {
      Int3 int3_1 = end1 - start1;
      Int3 int3_2 = end2 - start2;
      long num1 = (long) (int3_2.z * int3_1.x - int3_2.x * int3_1.z);
      if (num1 == 0L)
      {
        factor1 = 0.0f;
        factor2 = 0.0f;
        return false;
      }
      long num2 = (long) (int3_2.x * (start1.z - start2.z) - int3_2.z * (start1.x - start2.x));
      long num3 = (long) (int3_1.x * (start1.z - start2.z) - int3_1.z * (start1.x - start2.x));
      factor1 = (float) num2 / (float) num1;
      factor2 = (float) num3 / (float) num1;
      return true;
    }

    public static bool IntersectionFactor(Vector3 start1, Vector3 end1, Vector3 start2, Vector3 end2, out float factor1, out float factor2)
    {
      Vector3 vector3_1 = end1 - start1;
      Vector3 vector3_2 = end2 - start2;
      float num1 = (float) ((double) vector3_2.z * (double) vector3_1.x - (double) vector3_2.x * (double) vector3_1.z);
      if ((double) num1 <= 9.99999974737875E-06 && (double) num1 >= -9.99999974737875E-06)
      {
        factor1 = 0.0f;
        factor2 = 0.0f;
        return false;
      }
      float num2 = (float) ((double) vector3_2.x * ((double) start1.z - (double) start2.z) - (double) vector3_2.z * ((double) start1.x - (double) start2.x));
      float num3 = (float) ((double) vector3_1.x * ((double) start1.z - (double) start2.z) - (double) vector3_1.z * ((double) start1.x - (double) start2.x));
      float num4 = num2 / num1;
      float num5 = num3 / num1;
      factor1 = num4;
      factor2 = num5;
      return true;
    }

    public static float IntersectionFactorRay(Int3 start1, Int3 end1, Int3 start2, Int3 end2)
    {
      Int3 int3_1 = end1 - start1;
      Int3 int3_2 = end2 - start2;
      int num1 = int3_2.z * int3_1.x - int3_2.x * int3_1.z;
      if (num1 == 0)
        return float.NaN;
      int num2 = int3_2.x * (start1.z - start2.z) - int3_2.z * (start1.x - start2.x);
      if ((double) (int3_1.x * (start1.z - start2.z) - int3_1.z * (start1.x - start2.x)) / (double) num1 < 0.0)
        return float.NaN;
      return (float) num2 / (float) num1;
    }

    public static float IntersectionFactor(Vector3 start1, Vector3 end1, Vector3 start2, Vector3 end2)
    {
      Vector3 vector3_1 = end1 - start1;
      Vector3 vector3_2 = end2 - start2;
      float num = (float) ((double) vector3_2.z * (double) vector3_1.x - (double) vector3_2.x * (double) vector3_1.z);
      if ((double) num == 0.0)
        return -1f;
      return (float) ((double) vector3_2.x * ((double) start1.z - (double) start2.z) - (double) vector3_2.z * ((double) start1.x - (double) start2.x)) / num;
    }

    public static Vector3 IntersectionPoint(Vector3 start1, Vector3 end1, Vector3 start2, Vector3 end2)
    {
      bool intersects;
      return Polygon.IntersectionPoint(start1, end1, start2, end2, out intersects);
    }

    public static Vector3 IntersectionPoint(Vector3 start1, Vector3 end1, Vector3 start2, Vector3 end2, out bool intersects)
    {
      Vector3 vector3_1 = end1 - start1;
      Vector3 vector3_2 = end2 - start2;
      float num1 = (float) ((double) vector3_2.z * (double) vector3_1.x - (double) vector3_2.x * (double) vector3_1.z);
      if ((double) num1 == 0.0)
      {
        intersects = false;
        return start1;
      }
      float num2 = (float) ((double) vector3_2.x * ((double) start1.z - (double) start2.z) - (double) vector3_2.z * ((double) start1.x - (double) start2.x)) / num1;
      intersects = true;
      return start1 + vector3_1 * num2;
    }

    public static Vector2 IntersectionPoint(Vector2 start1, Vector2 end1, Vector2 start2, Vector2 end2)
    {
      bool intersects;
      return Polygon.IntersectionPoint(start1, end1, start2, end2, out intersects);
    }

    public static Vector2 IntersectionPoint(Vector2 start1, Vector2 end1, Vector2 start2, Vector2 end2, out bool intersects)
    {
      Vector2 vector2_1 = end1 - start1;
      Vector2 vector2_2 = end2 - start2;
      float num1 = (float) ((double) vector2_2.y * (double) vector2_1.x - (double) vector2_2.x * (double) vector2_1.y);
      if ((double) num1 == 0.0)
      {
        intersects = false;
        return start1;
      }
      float num2 = (float) ((double) vector2_2.x * ((double) start1.y - (double) start2.y) - (double) vector2_2.y * ((double) start1.x - (double) start2.x)) / num1;
      intersects = true;
      return start1 + vector2_1 * num2;
    }

    public static Vector3 SegmentIntersectionPoint(Vector3 start1, Vector3 end1, Vector3 start2, Vector3 end2, out bool intersects)
    {
      Vector3 vector3_1 = end1 - start1;
      Vector3 vector3_2 = end2 - start2;
      float num1 = (float) ((double) vector3_2.z * (double) vector3_1.x - (double) vector3_2.x * (double) vector3_1.z);
      if ((double) num1 == 0.0)
      {
        intersects = false;
        return start1;
      }
      float num2 = (float) ((double) vector3_2.x * ((double) start1.z - (double) start2.z) - (double) vector3_2.z * ((double) start1.x - (double) start2.x));
      float num3 = (float) ((double) vector3_1.x * ((double) start1.z - (double) start2.z) - (double) vector3_1.z * ((double) start1.x - (double) start2.x));
      float num4 = num2 / num1;
      float num5 = num3 / num1;
      if ((double) num4 < 0.0 || (double) num4 > 1.0 || ((double) num5 < 0.0 || (double) num5 > 1.0))
      {
        intersects = false;
        return start1;
      }
      intersects = true;
      return start1 + vector3_1 * num4;
    }

    public static Vector3[] ConvexHull(Vector3[] points)
    {
      if (points.Length == 0)
        return new Vector3[0];
      lock (Polygon.hullCache)
      {
        List<Vector3> local_1 = Polygon.hullCache;
        local_1.Clear();
        int local_2 = 0;
        for (int local_3 = 1; local_3 < points.Length; ++local_3)
        {
          if ((double) points[local_3].x < (double) points[local_2].x)
            local_2 = local_3;
        }
        int local_4 = local_2;
        int local_5 = 0;
        do
        {
          local_1.Add(points[local_2]);
          int local_6 = 0;
          for (int local_7 = 0; local_7 < points.Length; ++local_7)
          {
            if (local_6 == local_2 || !Polygon.Left(points[local_2], points[local_6], points[local_7]))
              local_6 = local_7;
          }
          local_2 = local_6;
          ++local_5;
          if (local_5 > 10000)
          {
            Debug.LogWarning((object) "Infinite Loop in Convex Hull Calculation");
            break;
          }
        }
        while (local_2 != local_4);
        return local_1.ToArray();
      }
    }

    public static bool LineIntersectsBounds(Bounds bounds, Vector3 a, Vector3 b)
    {
      a -= bounds.center;
      b -= bounds.center;
      Vector3 vector3_1 = (a + b) * 0.5f;
      Vector3 vector3_2 = a - vector3_1;
      Vector3 vector3_3 = new Vector3(Math.Abs(vector3_2.x), Math.Abs(vector3_2.y), Math.Abs(vector3_2.z));
      Vector3 extents = bounds.extents;
      return (double) Math.Abs(vector3_1.x) <= (double) extents.x + (double) vector3_3.x && (double) Math.Abs(vector3_1.y) <= (double) extents.y + (double) vector3_3.y && ((double) Math.Abs(vector3_1.z) <= (double) extents.z + (double) vector3_3.z && (double) Math.Abs((float) ((double) vector3_1.y * (double) vector3_2.z - (double) vector3_1.z * (double) vector3_2.y)) <= (double) extents.y * (double) vector3_3.z + (double) extents.z * (double) vector3_3.y) && ((double) Math.Abs((float) ((double) vector3_1.x * (double) vector3_2.z - (double) vector3_1.z * (double) vector3_2.x)) <= (double) extents.x * (double) vector3_3.z + (double) extents.z * (double) vector3_3.x && (double) Math.Abs((float) ((double) vector3_1.x * (double) vector3_2.y - (double) vector3_1.y * (double) vector3_2.x)) <= (double) extents.x * (double) vector3_3.y + (double) extents.y * (double) vector3_3.x);
    }

    public static Vector3[] Subdivide(Vector3[] path, int subdivisions)
    {
      subdivisions = subdivisions >= 0 ? subdivisions : 0;
      if (subdivisions == 0)
        return path;
      Vector3[] vector3Array = new Vector3[(path.Length - 1) * (int) Mathf.Pow(2f, (float) subdivisions) + 1];
      int index1 = 0;
      for (int index2 = 0; index2 < path.Length - 1; ++index2)
      {
        float num = 1f / Mathf.Pow(2f, (float) subdivisions);
        float t = 0.0f;
        while ((double) t < 1.0)
        {
          vector3Array[index1] = Vector3.Lerp(path[index2], path[index2 + 1], Mathf.SmoothStep(0.0f, 1f, t));
          ++index1;
          t += num;
        }
      }
      vector3Array[index1] = path[path.Length - 1];
      return vector3Array;
    }

    public static Vector3 ClosestPointOnTriangle(Vector3[] triangle, Vector3 point)
    {
      return Polygon.ClosestPointOnTriangle(triangle[0], triangle[1], triangle[2], point);
    }

    public static Vector3 ClosestPointOnTriangle(Vector3 tr0, Vector3 tr1, Vector3 tr2, Vector3 point)
    {
      Vector3 lhs = tr0 - point;
      Vector3 vector3 = tr1 - tr0;
      Vector3 rhs = tr2 - tr0;
      float sqrMagnitude1 = vector3.sqrMagnitude;
      float num1 = Vector3.Dot(vector3, rhs);
      float sqrMagnitude2 = rhs.sqrMagnitude;
      float num2 = Vector3.Dot(lhs, vector3);
      float num3 = Vector3.Dot(lhs, rhs);
      float num4 = (float) ((double) sqrMagnitude1 * (double) sqrMagnitude2 - (double) num1 * (double) num1);
      float num5 = (float) ((double) num1 * (double) num3 - (double) sqrMagnitude2 * (double) num2);
      float num6 = (float) ((double) num1 * (double) num2 - (double) sqrMagnitude1 * (double) num3);
      float num7;
      float num8;
      if ((double) num5 + (double) num6 <= (double) num4)
      {
        if ((double) num5 < 0.0)
        {
          if ((double) num6 < 0.0)
          {
            if ((double) num2 < 0.0)
            {
              num7 = 0.0f;
              num8 = -(double) num2 < (double) sqrMagnitude1 ? -num2 / sqrMagnitude1 : 1f;
            }
            else
            {
              num8 = 0.0f;
              num7 = (double) num3 < 0.0 ? (-(double) num3 < (double) sqrMagnitude2 ? -num3 / sqrMagnitude2 : 1f) : 0.0f;
            }
          }
          else
          {
            num8 = 0.0f;
            num7 = (double) num3 < 0.0 ? (-(double) num3 < (double) sqrMagnitude2 ? -num3 / sqrMagnitude2 : 1f) : 0.0f;
          }
        }
        else if ((double) num6 < 0.0)
        {
          num7 = 0.0f;
          num8 = (double) num2 < 0.0 ? (-(double) num2 < (double) sqrMagnitude1 ? -num2 / sqrMagnitude1 : 1f) : 0.0f;
        }
        else
        {
          float num9 = 1f / num4;
          num8 = num5 * num9;
          num7 = num6 * num9;
        }
      }
      else if ((double) num5 < 0.0)
      {
        float num9 = num1 + num2;
        float num10 = sqrMagnitude2 + num3;
        if ((double) num10 > (double) num9)
        {
          float num11 = num10 - num9;
          float num12 = sqrMagnitude1 - 2f * num1 + sqrMagnitude2;
          if ((double) num11 >= (double) num12)
          {
            num8 = 1f;
            num7 = 0.0f;
          }
          else
          {
            num8 = num11 / num12;
            num7 = 1f - num8;
          }
        }
        else
        {
          num8 = 0.0f;
          num7 = (double) num10 > 0.0 ? ((double) num3 < 0.0 ? -num3 / sqrMagnitude2 : 0.0f) : 1f;
        }
      }
      else if ((double) num6 < 0.0)
      {
        float num9 = num1 + num3;
        float num10 = sqrMagnitude1 + num2;
        if ((double) num10 > (double) num9)
        {
          float num11 = num10 - num9;
          float num12 = sqrMagnitude1 - 2f * num1 + sqrMagnitude2;
          if ((double) num11 >= (double) num12)
          {
            num7 = 1f;
            num8 = 0.0f;
          }
          else
          {
            num7 = num11 / num12;
            num8 = 1f - num7;
          }
        }
        else
        {
          num7 = 0.0f;
          num8 = (double) num10 > 0.0 ? ((double) num2 < 0.0 ? -num2 / sqrMagnitude1 : 0.0f) : 1f;
        }
      }
      else
      {
        float num9 = sqrMagnitude2 + num3 - num1 - num2;
        if ((double) num9 <= 0.0)
        {
          num8 = 0.0f;
          num7 = 1f;
        }
        else
        {
          float num10 = sqrMagnitude1 - 2f * num1 + sqrMagnitude2;
          if ((double) num9 >= (double) num10)
          {
            num8 = 1f;
            num7 = 0.0f;
          }
          else
          {
            num8 = num9 / num10;
            num7 = 1f - num8;
          }
        }
      }
      return tr0 + num8 * vector3 + num7 * rhs;
    }

    public static float DistanceSegmentSegment3D(Vector3 s1, Vector3 e1, Vector3 s2, Vector3 e2)
    {
      Vector3 vector3_1 = e1 - s1;
      Vector3 vector3_2 = e2 - s2;
      Vector3 rhs = s1 - s2;
      float num1 = Vector3.Dot(vector3_1, vector3_1);
      float num2 = Vector3.Dot(vector3_1, vector3_2);
      float num3 = Vector3.Dot(vector3_2, vector3_2);
      float num4 = Vector3.Dot(vector3_1, rhs);
      float num5 = Vector3.Dot(vector3_2, rhs);
      float num6 = (float) ((double) num1 * (double) num3 - (double) num2 * (double) num2);
      float num7 = num6;
      float num8 = num6;
      float num9;
      float num10;
      if ((double) num6 < 9.99999997475243E-07)
      {
        num9 = 0.0f;
        num7 = 1f;
        num10 = num5;
        num8 = num3;
      }
      else
      {
        num9 = (float) ((double) num2 * (double) num5 - (double) num3 * (double) num4);
        num10 = (float) ((double) num1 * (double) num5 - (double) num2 * (double) num4);
        if ((double) num9 < 0.0)
        {
          num9 = 0.0f;
          num10 = num5;
          num8 = num3;
        }
        else if ((double) num9 > (double) num7)
        {
          num9 = num7;
          num10 = num5 + num2;
          num8 = num3;
        }
      }
      if ((double) num10 < 0.0)
      {
        num10 = 0.0f;
        if (-(double) num4 < 0.0)
          num9 = 0.0f;
        else if (-(double) num4 > (double) num1)
        {
          num9 = num7;
        }
        else
        {
          num9 = -num4;
          num7 = num1;
        }
      }
      else if ((double) num10 > (double) num8)
      {
        num10 = num8;
        if (-(double) num4 + (double) num2 < 0.0)
          num9 = 0.0f;
        else if (-(double) num4 + (double) num2 > (double) num1)
        {
          num9 = num7;
        }
        else
        {
          num9 = -num4 + num2;
          num7 = num1;
        }
      }
      float num11 = (double) Math.Abs(num9) >= 9.99999997475243E-07 ? num9 / num7 : 0.0f;
      float num12 = (double) Math.Abs(num10) >= 9.99999997475243E-07 ? num10 / num8 : 0.0f;
      return (rhs + num11 * vector3_1 - num12 * vector3_2).sqrMagnitude;
    }
  }
}
