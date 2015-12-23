// Decompiled with JetBrains decompiler
// Type: Pathfinding.IntRect
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

namespace Pathfinding
{
  public struct IntRect
  {
    private static readonly int[] Rotations = new int[16]
    {
      1,
      0,
      0,
      1,
      0,
      1,
      -1,
      0,
      -1,
      0,
      0,
      -1,
      0,
      -1,
      1,
      0
    };
    public int xmin;
    public int ymin;
    public int xmax;
    public int ymax;

    public int Width
    {
      get
      {
        return this.xmax - this.xmin + 1;
      }
    }

    public int Height
    {
      get
      {
        return this.ymax - this.ymin + 1;
      }
    }

    public IntRect(int xmin, int ymin, int xmax, int ymax)
    {
      this.xmin = xmin;
      this.xmax = xmax;
      this.ymin = ymin;
      this.ymax = ymax;
    }

    public static bool operator ==(IntRect a, IntRect b)
    {
      if (a.xmin == b.xmin && a.xmax == b.xmax && a.ymin == b.ymin)
        return a.ymax == b.ymax;
      return false;
    }

    public static bool operator !=(IntRect a, IntRect b)
    {
      if (a.xmin == b.xmin && a.xmax == b.xmax && a.ymin == b.ymin)
        return a.ymax != b.ymax;
      return true;
    }

    public bool Contains(int x, int y)
    {
      return (x < this.xmin || y < this.ymin || x > this.xmax ? 1 : (y > this.ymax ? 1 : 0)) == 0;
    }

    public bool IsValid()
    {
      if (this.xmin <= this.xmax)
        return this.ymin <= this.ymax;
      return false;
    }

    public override bool Equals(object _b)
    {
      IntRect intRect = (IntRect) _b;
      if (this.xmin == intRect.xmin && this.xmax == intRect.xmax && this.ymin == intRect.ymin)
        return this.ymax == intRect.ymax;
      return false;
    }

    public override int GetHashCode()
    {
      return this.xmin * 131071 ^ this.xmax * 3571 ^ this.ymin * 3109 ^ this.ymax * 7;
    }

    public static IntRect Intersection(IntRect a, IntRect b)
    {
      return new IntRect(Math.Max(a.xmin, b.xmin), Math.Max(a.ymin, b.ymin), Math.Min(a.xmax, b.xmax), Math.Min(a.ymax, b.ymax));
    }

    public static bool Intersects(IntRect a, IntRect b)
    {
      return (a.xmin > b.xmax || a.ymin > b.ymax || a.xmax < b.xmin ? 1 : (a.ymax < b.ymin ? 1 : 0)) == 0;
    }

    public static IntRect Union(IntRect a, IntRect b)
    {
      return new IntRect(Math.Min(a.xmin, b.xmin), Math.Min(a.ymin, b.ymin), Math.Max(a.xmax, b.xmax), Math.Max(a.ymax, b.ymax));
    }

    public IntRect ExpandToContain(int x, int y)
    {
      return new IntRect(Math.Min(this.xmin, x), Math.Min(this.ymin, y), Math.Max(this.xmax, x), Math.Max(this.ymax, y));
    }

    public IntRect Expand(int range)
    {
      return new IntRect(this.xmin - range, this.ymin - range, this.xmax + range, this.ymax + range);
    }

    public IntRect Rotate(int r)
    {
      int num1 = IntRect.Rotations[r * 4];
      int num2 = IntRect.Rotations[r * 4 + 1];
      int num3 = IntRect.Rotations[r * 4 + 2];
      int num4 = IntRect.Rotations[r * 4 + 3];
      int val1_1 = num1 * this.xmin + num2 * this.ymin;
      int val1_2 = num3 * this.xmin + num4 * this.ymin;
      int val2_1 = num1 * this.xmax + num2 * this.ymax;
      int val2_2 = num3 * this.xmax + num4 * this.ymax;
      return new IntRect(Math.Min(val1_1, val2_1), Math.Min(val1_2, val2_2), Math.Max(val1_1, val2_1), Math.Max(val1_2, val2_2));
    }

    public IntRect Offset(Int2 offset)
    {
      return new IntRect(this.xmin + offset.x, this.ymin + offset.y, this.xmax + offset.x, this.ymax + offset.y);
    }

    public IntRect Offset(int x, int y)
    {
      return new IntRect(this.xmin + x, this.ymin + y, this.xmax + x, this.ymax + y);
    }

    public override string ToString()
    {
      return "[x: " + (object) this.xmin + "..." + (string) (object) this.xmax + ", y: " + (string) (object) this.ymin + "..." + (string) (object) this.ymax + "]";
    }

    public void DebugDraw(Matrix4x4 matrix, Color col)
    {
      Vector3 vector3_1 = matrix.MultiplyPoint3x4(new Vector3((float) this.xmin, 0.0f, (float) this.ymin));
      Vector3 vector3_2 = matrix.MultiplyPoint3x4(new Vector3((float) this.xmin, 0.0f, (float) this.ymax));
      Vector3 vector3_3 = matrix.MultiplyPoint3x4(new Vector3((float) this.xmax, 0.0f, (float) this.ymax));
      Vector3 vector3_4 = matrix.MultiplyPoint3x4(new Vector3((float) this.xmax, 0.0f, (float) this.ymin));
      Debug.DrawLine(vector3_1, vector3_2, col);
      Debug.DrawLine(vector3_2, vector3_3, col);
      Debug.DrawLine(vector3_3, vector3_4, col);
      Debug.DrawLine(vector3_4, vector3_1, col);
    }
  }
}
