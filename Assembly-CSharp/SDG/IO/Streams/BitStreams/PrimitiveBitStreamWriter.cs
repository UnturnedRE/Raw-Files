// Decompiled with JetBrains decompiler
// Type: SDG.IO.Streams.BitStreams.PrimitiveBitStreamWriter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System.IO;

namespace SDG.IO.Streams.BitStreams
{
  public class PrimitiveBitStreamWriter : BitStreamWriter
  {
    public PrimitiveBitStreamWriter(Stream newStream)
      : base(newStream)
    {
    }

    public void writeByte(byte data)
    {
      this.writeBits(data, (byte) 8);
    }

    public void writeInt16(short data)
    {
      this.writeByte((byte) ((uint) data >> 8));
      this.writeByte((byte) data);
    }

    public void writeInt16(short data, byte length)
    {
      if ((int) length == 16)
        this.writeInt16(data);
      else if ((int) length > 8)
      {
        this.writeBits((byte) ((uint) data >> 8), (byte) ((uint) length - 8U));
        this.writeByte((byte) data);
      }
      else if ((int) length == 8)
        this.writeByte((byte) data);
      else
        this.writeBits((byte) data, length);
    }

    public void writeUInt16(ushort data)
    {
      this.writeByte((byte) ((uint) data >> 8));
      this.writeByte((byte) data);
    }

    public void writeUInt16(ushort data, byte length)
    {
      if ((int) length == 16)
        this.writeUInt16(data);
      else if ((int) length > 8)
      {
        this.writeBits((byte) ((uint) data >> 8), (byte) ((uint) length - 8U));
        this.writeByte((byte) data);
      }
      else if ((int) length == 8)
        this.writeByte((byte) data);
      else
        this.writeBits((byte) data, length);
    }

    public void writeInt32(int data)
    {
      this.writeByte((byte) (data >> 24));
      this.writeByte((byte) (data >> 16));
      this.writeByte((byte) (data >> 8));
      this.writeByte((byte) data);
    }

    public void writeInt32(int data, byte length)
    {
      if ((int) length == 32)
        this.writeInt32(data);
      else if ((int) length > 24)
      {
        this.writeBits((byte) (data >> 24), (byte) ((uint) length - 8U));
        this.writeByte((byte) (data >> 16));
        this.writeByte((byte) (data >> 8));
        this.writeByte((byte) data);
      }
      else if ((int) length == 24)
      {
        this.writeByte((byte) (data >> 16));
        this.writeByte((byte) (data >> 8));
        this.writeByte((byte) data);
      }
      else if ((int) length > 16)
      {
        this.writeBits((byte) (data >> 16), (byte) ((uint) length - 8U));
        this.writeByte((byte) (data >> 8));
        this.writeByte((byte) data);
      }
      else if ((int) length == 16)
      {
        this.writeByte((byte) (data >> 8));
        this.writeByte((byte) data);
      }
      else if ((int) length > 8)
      {
        this.writeBits((byte) (data >> 8), (byte) ((uint) length - 8U));
        this.writeByte((byte) data);
      }
      else if ((int) length == 8)
        this.writeByte((byte) data);
      else
        this.writeBits((byte) data, length);
    }
  }
}
