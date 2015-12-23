// Decompiled with JetBrains decompiler
// Type: SDG.IO.Streams.BitStreams.PrimitiveBitStreamReader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System.IO;

namespace SDG.IO.Streams.BitStreams
{
  public class PrimitiveBitStreamReader : BitStreamReader
  {
    public PrimitiveBitStreamReader(Stream newStream)
      : base(newStream)
    {
    }

    public void readByte(ref byte data)
    {
      this.readBits(ref data, (byte) 8);
    }

    public void readInt16(ref short data)
    {
      byte data1 = (byte) 0;
      byte data2 = (byte) 0;
      this.readByte(ref data1);
      this.readByte(ref data2);
      data = (short) ((int) data1 << 8 | (int) data2);
    }

    public void readInt16(ref short data, byte length)
    {
      if ((int) length == 16)
        this.readInt16(ref data);
      else if ((int) length > 8)
      {
        byte data1 = (byte) 0;
        byte data2 = (byte) 0;
        this.readBits(ref data1, (byte) ((uint) length - 8U));
        this.readByte(ref data2);
        data = (short) ((int) data1 << 8 | (int) data2);
      }
      else if ((int) length == 8)
      {
        byte data1 = (byte) 0;
        this.readByte(ref data1);
        data = (short) data1;
      }
      else
      {
        byte data1 = (byte) 0;
        this.readBits(ref data1, length);
        data = (short) data1;
      }
    }

    public void readUInt16(ref ushort data)
    {
      byte data1 = (byte) 0;
      byte data2 = (byte) 0;
      this.readByte(ref data1);
      this.readByte(ref data2);
      data = (ushort) ((uint) data1 << 8 | (uint) data2);
    }

    public void readUInt16(ref ushort data, byte length)
    {
      if ((int) length == 16)
        this.readUInt16(ref data);
      else if ((int) length > 8)
      {
        byte data1 = (byte) 0;
        byte data2 = (byte) 0;
        this.readBits(ref data1, (byte) ((uint) length - 8U));
        this.readByte(ref data2);
        data = (ushort) ((uint) data1 << 8 | (uint) data2);
      }
      else if ((int) length == 8)
      {
        byte data1 = (byte) 0;
        this.readByte(ref data1);
        data = (ushort) (short) data1;
      }
      else
      {
        byte data1 = (byte) 0;
        this.readBits(ref data1, length);
        data = (ushort) (short) data1;
      }
    }

    public void readInt32(ref int data)
    {
      byte data1 = (byte) 0;
      byte data2 = (byte) 0;
      byte data3 = (byte) 0;
      byte data4 = (byte) 0;
      this.readByte(ref data1);
      this.readByte(ref data2);
      this.readByte(ref data3);
      this.readByte(ref data4);
      data = (int) data1 << 24 | (int) data2 << 16 | (int) data3 << 8 | (int) data4;
    }

    public void readInt32(ref int data, byte length)
    {
      if ((int) length == 32)
        this.readInt32(ref data);
      else if ((int) length > 24)
      {
        byte data1 = (byte) 0;
        byte data2 = (byte) 0;
        byte data3 = (byte) 0;
        byte data4 = (byte) 0;
        this.readBits(ref data1, (byte) ((uint) length - 8U));
        this.readByte(ref data2);
        this.readByte(ref data3);
        this.readByte(ref data4);
        data = (int) data1 << 24 | (int) data2 << 16 | (int) data3 << 8 | (int) data4;
      }
      else if ((int) length == 24)
      {
        byte data1 = (byte) 0;
        byte data2 = (byte) 0;
        byte data3 = (byte) 0;
        this.readByte(ref data1);
        this.readByte(ref data2);
        this.readByte(ref data3);
        data = (int) data1 << 16 | (int) data2 << 8 | (int) data3;
      }
      else if ((int) length > 16)
      {
        byte data1 = (byte) 0;
        byte data2 = (byte) 0;
        byte data3 = (byte) 0;
        this.readBits(ref data1, (byte) ((uint) length - 8U));
        this.readByte(ref data2);
        this.readByte(ref data3);
        data = (int) data1 << 16 | (int) data2 << 8 | (int) data3;
      }
      else if ((int) length == 16)
      {
        byte data1 = (byte) 0;
        byte data2 = (byte) 0;
        this.readByte(ref data1);
        this.readByte(ref data2);
        data = (int) data1 << 8 | (int) data2;
      }
      else if ((int) length > 8)
      {
        byte data1 = (byte) 0;
        byte data2 = (byte) 0;
        this.readBits(ref data1, (byte) ((uint) length - 8U));
        this.readByte(ref data2);
        data = (int) data1 << 8 | (int) data2;
      }
      else if ((int) length == 8)
      {
        byte data1 = (byte) 0;
        this.readByte(ref data1);
        data = (int) data1;
      }
      else
      {
        byte data1 = (byte) 0;
        this.readBits(ref data1, length);
        data = (int) data1;
      }
    }
  }
}
