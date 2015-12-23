// Decompiled with JetBrains decompiler
// Type: Pathfinding.Int2
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Pathfinding
{
  public struct Int2
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
    public int x;
    public int y;

    public int sqrMagnitude
    {
      get
      {
        return this.x * this.x + this.y * this.y;
      }
    }

    public long sqrMagnitudeLong
    {
      get
      {
        return (long) this.x * (long) this.x + (long) this.y * (long) this.y;
      }
    }

    public Int2(int x, int y)
    {
      this.x = x;
      this.y = y;
    }

    public static Int2 operator +(Int2 a, Int2 b)
    {
      return new Int2(a.x + b.x, a.y + b.y);
    }

    public static Int2 operator -(Int2 a, Int2 b)
    {
      return new Int2(a.x - b.x, a.y - b.y);
    }

    public static bool operator ==(Int2 a, Int2 b)
    {
      if (a.x == b.x)
        return a.y == b.y;
      return false;
    }

    public static bool operator !=(Int2 a, Int2 b)
    {
      if (a.x == b.x)
        return a.y != b.y;
      return true;
    }

    public static int Dot(Int2 a, Int2 b)
    {
      return a.x * b.x + a.y * b.y;
    }

    public static long DotLong(Int2 a, Int2 b)
    {
      return (long) a.x * (long) b.x + (long) a.y * (long) b.y;
    }

    public override bool Equals(object o)
    {
      if (o == null)
        return false;
      Int2 int2 = (Int2) o;
      if (this.x == int2.x)
        return this.y == int2.y;
      return false;
    }

    public override int GetHashCode()
    {
      return this.x * 49157 + this.y * 98317;
    }

    public static Int2 Rotate(Int2 v, int r)
    {
      r %= 4;
      return new Int2(v.x * Int2.Rotations[r * 4] + v.y * Int2.Rotations[r * 4 + 1], v.x * Int2.Rotations[r * 4 + 2] + v.y * Int2.Rotations[r * 4 + 3]);
    }

    public static Int2 Min(Int2 a, Int2 b)
    {
      return new Int2(Math.Min(a.x, b.x), Math.Min(a.y, b.y));
    }

    public static Int2 Max(Int2 a, Int2 b)
    {
      return new Int2(Math.Max(a.x, b.x), Math.Max(a.y, b.y));
    }

    public static Int2 FromInt3XZ(Int3 o)
    {
      return new Int2(o.x, o.z);
    }

    public static Int3 ToInt3XZ(Int2 o)
    {
      return new Int3(o.x, 0, o.y);
    }

    public override string ToString()
    {
      return "(" + (object) this.x + ", " + (string) (object) this.y + ")";
    }
  }
}
