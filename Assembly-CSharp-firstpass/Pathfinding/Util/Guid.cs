// Decompiled with JetBrains decompiler
// Type: Pathfinding.Util.Guid
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Text;

namespace Pathfinding.Util
{
  public struct Guid
  {
    public static readonly Guid zero = new Guid(new byte[16]);
    public static readonly string zeroString = new Guid(new byte[16]).ToString();
    private static Random random = new Random();
    private const string hex = "0123456789ABCDEF";
    private ulong _a;
    private ulong _b;
    private static StringBuilder text;

    public Guid(byte[] bytes)
    {
      this._a = (ulong) ((long) bytes[0] | (long) bytes[1] << 8 | (long) bytes[2] << 16 | (long) bytes[3] << 24 | (long) bytes[4] << 32 | (long) bytes[5] << 40 | (long) bytes[6] << 48 | (long) bytes[7] << 56);
      this._b = (ulong) ((long) bytes[8] | (long) bytes[9] << 8 | (long) bytes[10] << 16 | (long) bytes[11] << 24 | (long) bytes[12] << 32 | (long) bytes[13] << 40 | (long) bytes[14] << 48 | (long) bytes[15] << 56);
    }

    public Guid(string str)
    {
      this._a = 0UL;
      this._b = 0UL;
      if (str.Length < 32)
        throw new FormatException("Invalid Guid format");
      int num1 = 0;
      int index = 0;
      int num2 = 60;
      while (num1 < 16)
      {
        if (index >= str.Length)
          throw new FormatException("Invalid Guid format. String too short");
        char c = str[index];
        if ((int) c != 45)
        {
          int num3 = "0123456789ABCDEF".IndexOf(char.ToUpperInvariant(c));
          if (num3 == -1)
            throw new FormatException("Invalid Guid format : " + (object) c + " is not a hexadecimal character");
          this._a |= (ulong) num3 << num2;
          num2 -= 4;
          ++num1;
        }
        ++index;
      }
      int num4 = 60;
      while (num1 < 32)
      {
        if (index >= str.Length)
          throw new FormatException("Invalid Guid format. String too short");
        char c = str[index];
        if ((int) c != 45)
        {
          int num3 = "0123456789ABCDEF".IndexOf(char.ToUpperInvariant(c));
          if (num3 == -1)
            throw new FormatException("Invalid Guid format : " + (object) c + " is not a hexadecimal character");
          this._b |= (ulong) num3 << num4;
          num4 -= 4;
          ++num1;
        }
        ++index;
      }
    }

    public static bool operator ==(Guid lhs, Guid rhs)
    {
      if ((long) lhs._a == (long) rhs._a)
        return (long) lhs._b == (long) rhs._b;
      return false;
    }

    public static bool operator !=(Guid lhs, Guid rhs)
    {
      if ((long) lhs._a == (long) rhs._a)
        return (long) lhs._b != (long) rhs._b;
      return true;
    }

    public static Guid Parse(string input)
    {
      return new Guid(input);
    }

    public byte[] ToByteArray()
    {
      byte[] numArray = new byte[16];
      byte[] bytes1 = BitConverter.GetBytes(this._a);
      byte[] bytes2 = BitConverter.GetBytes(this._b);
      for (int index = 0; index < 8; ++index)
      {
        numArray[index] = bytes1[index];
        numArray[index + 8] = bytes2[index];
      }
      return numArray;
    }

    public static Guid NewGuid()
    {
      byte[] numArray = new byte[16];
      Guid.random.NextBytes(numArray);
      return new Guid(numArray);
    }

    public override bool Equals(object _rhs)
    {
      if (!(_rhs is Guid))
        return false;
      Guid guid = (Guid) _rhs;
      if ((long) this._a == (long) guid._a)
        return (long) this._b == (long) guid._b;
      return false;
    }

    public override int GetHashCode()
    {
      ulong num = this._a ^ this._b;
      return (int) (num >> 32) ^ (int) num;
    }

    public override string ToString()
    {
      if (Guid.text == null)
        Guid.text = new StringBuilder();
      lock (Guid.text)
      {
        Guid.text.Length = 0;
        Guid.text.Append(this._a.ToString("x16")).Append('-').Append(this._b.ToString("x16"));
        return Guid.text.ToString();
      }
    }
  }
}
