// Decompiled with JetBrains decompiler
// Type: Pathfinding.Int3
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

namespace Pathfinding
{
  public struct Int3
  {
    private static Int3 _zero = new Int3(0, 0, 0);
    public const int Precision = 1000;
    public const float FloatPrecision = 1000f;
    public const float PrecisionFactor = 0.001f;
    public int x;
    public int y;
    public int z;

    public static Int3 zero
    {
      get
      {
        return Int3._zero;
      }
    }

    public int this[int i]
    {
      get
      {
        if (i == 0)
          return this.x;
        if (i == 1)
          return this.y;
        return this.z;
      }
      set
      {
        if (i == 0)
          this.x = value;
        else if (i == 1)
          this.y = value;
        else
          this.z = value;
      }
    }

    public float magnitude
    {
      get
      {
        double num1 = (double) this.x;
        double num2 = (double) this.y;
        double num3 = (double) this.z;
        return (float) Math.Sqrt(num1 * num1 + num2 * num2 + num3 * num3);
      }
    }

    public int costMagnitude
    {
      get
      {
        return (int) Math.Round((double) this.magnitude);
      }
    }

    public float worldMagnitude
    {
      get
      {
        // ISSUE: unable to decompile the method.
      }
    }

    public float sqrMagnitude
    {
      get
      {
        double num1 = (double) this.x;
        double num2 = (double) this.y;
        double num3 = (double) this.z;
        return (float) (num1 * num1 + num2 * num2 + num3 * num3);
      }
    }

    public long sqrMagnitudeLong
    {
      get
      {
        long num1 = (long) this.x;
        long num2 = (long) this.y;
        long num3 = (long) this.z;
        return num1 * num1 + num2 * num2 + num3 * num3;
      }
    }

    public int unsafeSqrMagnitude
    {
      get
      {
        return this.x * this.x + this.y * this.y + this.z * this.z;
      }
    }

    [Obsolete("Same implementation as .magnitude")]
    public float safeMagnitude
    {
      get
      {
        double num1 = (double) this.x;
        double num2 = (double) this.y;
        double num3 = (double) this.z;
        return (float) Math.Sqrt(num1 * num1 + num2 * num2 + num3 * num3);
      }
    }

    [Obsolete(".sqrMagnitude is now per default safe (.unsafeSqrMagnitude can be used for unsafe operations)")]
    public float safeSqrMagnitude
    {
      get
      {
        float num1 = (float) this.x * (1.0 / 1000.0);
        float num2 = (float) this.y * (1.0 / 1000.0);
        float num3 = (float) this.z * (1.0 / 1000.0);
        return (float) ((double) num1 * (double) num1 + (double) num2 * (double) num2 + (double) num3 * (double) num3);
      }
    }

    public Int3(Vector3 position)
    {
      this.x = (int) Math.Round((double) position.x * 1000.0);
      this.y = (int) Math.Round((double) position.y * 1000.0);
      this.z = (int) Math.Round((double) position.z * 1000.0);
    }

    public Int3(int _x, int _y, int _z)
    {
      this.x = _x;
      this.y = _y;
      this.z = _z;
    }

    public static explicit operator Int3(Vector3 ob)
    {
      return new Int3((int) Math.Round((double) ob.x * 1000.0), (int) Math.Round((double) ob.y * 1000.0), (int) Math.Round((double) ob.z * 1000.0));
    }

    public static explicit operator Vector3(Int3 ob)
    {
      // ISSUE: unable to decompile the method.
    }

    public static implicit operator string(Int3 ob)
    {
      return ob.ToString();
    }

    public static bool operator ==(Int3 lhs, Int3 rhs)
    {
      if (lhs.x == rhs.x && lhs.y == rhs.y)
        return lhs.z == rhs.z;
      return false;
    }

    public static bool operator !=(Int3 lhs, Int3 rhs)
    {
      if (lhs.x == rhs.x && lhs.y == rhs.y)
        return lhs.z != rhs.z;
      return true;
    }

    public static Int3 operator -(Int3 lhs, Int3 rhs)
    {
      lhs.x -= rhs.x;
      lhs.y -= rhs.y;
      lhs.z -= rhs.z;
      return lhs;
    }

    public static Int3 operator -(Int3 lhs)
    {
      lhs.x = -lhs.x;
      lhs.y = -lhs.y;
      lhs.z = -lhs.z;
      return lhs;
    }

    public static Int3 operator +(Int3 lhs, Int3 rhs)
    {
      lhs.x += rhs.x;
      lhs.y += rhs.y;
      lhs.z += rhs.z;
      return lhs;
    }

    public static Int3 operator *(Int3 lhs, int rhs)
    {
      lhs.x *= rhs;
      lhs.y *= rhs;
      lhs.z *= rhs;
      return lhs;
    }

    public static Int3 operator *(Int3 lhs, float rhs)
    {
      lhs.x = (int) Math.Round((double) lhs.x * (double) rhs);
      lhs.y = (int) Math.Round((double) lhs.y * (double) rhs);
      lhs.z = (int) Math.Round((double) lhs.z * (double) rhs);
      return lhs;
    }

    public static Int3 operator *(Int3 lhs, double rhs)
    {
      lhs.x = (int) Math.Round((double) lhs.x * rhs);
      lhs.y = (int) Math.Round((double) lhs.y * rhs);
      lhs.z = (int) Math.Round((double) lhs.z * rhs);
      return lhs;
    }

    public static Int3 operator *(Int3 lhs, Vector3 rhs)
    {
      lhs.x = (int) Math.Round((double) lhs.x * (double) rhs.x);
      lhs.y = (int) Math.Round((double) lhs.y * (double) rhs.y);
      lhs.z = (int) Math.Round((double) lhs.z * (double) rhs.z);
      return lhs;
    }

    public static Int3 operator /(Int3 lhs, float rhs)
    {
      lhs.x = (int) Math.Round((double) lhs.x / (double) rhs);
      lhs.y = (int) Math.Round((double) lhs.y / (double) rhs);
      lhs.z = (int) Math.Round((double) lhs.z / (double) rhs);
      return lhs;
    }

    public Int3 DivBy2()
    {
      this.x >>= 1;
      this.y >>= 1;
      this.z >>= 1;
      return this;
    }

    public static float Angle(Int3 lhs, Int3 rhs)
    {
      double num = (double) Int3.Dot(lhs, rhs) / ((double) lhs.magnitude * (double) rhs.magnitude);
      return (float) Math.Acos(num >= -1.0 ? (num <= 1.0 ? num : 1.0) : -1.0);
    }

    public static int Dot(Int3 lhs, Int3 rhs)
    {
      return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
    }

    public static long DotLong(Int3 lhs, Int3 rhs)
    {
      return (long) lhs.x * (long) rhs.x + (long) lhs.y * (long) rhs.y + (long) lhs.z * (long) rhs.z;
    }

    public Int3 Normal2D()
    {
      return new Int3(this.z, this.y, -this.x);
    }

    public Int3 NormalizeTo(int newMagn)
    {
      float magnitude = this.magnitude;
      if ((double) magnitude == 0.0)
        return this;
      this.x *= newMagn;
      this.y *= newMagn;
      this.z *= newMagn;
      this.x = (int) Math.Round((double) this.x / (double) magnitude);
      this.y = (int) Math.Round((double) this.y / (double) magnitude);
      this.z = (int) Math.Round((double) this.z / (double) magnitude);
      return this;
    }

    public override string ToString()
    {
      return "( " + (object) this.x + ", " + (string) (object) this.y + ", " + (string) (object) this.z + ")";
    }

    public override bool Equals(object o)
    {
      if (o == null)
        return false;
      Int3 int3 = (Int3) o;
      if (this.x == int3.x && this.y == int3.y)
        return this.z == int3.z;
      return false;
    }

    public override int GetHashCode()
    {
      return this.x * 73856093 ^ this.y * 19349663 ^ this.z * 83492791;
    }
  }
}
