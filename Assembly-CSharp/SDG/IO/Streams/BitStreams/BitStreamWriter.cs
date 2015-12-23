// Decompiled with JetBrains decompiler
// Type: SDG.IO.Streams.BitStreams.BitStreamWriter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System.IO;

namespace SDG.IO.Streams.BitStreams
{
  public class BitStreamWriter
  {
    public Stream stream { get; protected set; }

    private byte buffer { get; set; }

    private byte bitIndex { get; set; }

    private byte bitsAvailable { get; set; }

    public BitStreamWriter(Stream newStream)
    {
      this.stream = newStream;
      this.reset();
    }

    public void writeBit(byte data)
    {
      this.writeBits(data, (byte) 1);
    }

    public void writeBits(byte data, byte length)
    {
      if ((int) length > (int) this.bitsAvailable)
      {
        byte length1 = (byte) ((uint) length - (uint) this.bitsAvailable);
        this.writeBits((byte) ((uint) data >> (int) length1), this.bitsAvailable);
        this.writeBits(data, length1);
      }
      else
      {
        byte num1 = (byte) (8U - (uint) length - (uint) this.bitIndex);
        byte num2 = (byte) ((int) byte.MaxValue >> 8 - (int) length);
        this.buffer |= (byte) (((uint) data & (uint) num2) << (int) num1);
        this.bitIndex += length;
        this.bitsAvailable -= length;
        if ((int) this.bitIndex != 8 || (int) this.bitsAvailable != 0)
          return;
        this.emptyBuffer();
      }
    }

    private void emptyBuffer()
    {
      this.stream.WriteByte(this.buffer);
      this.reset();
    }

    public void flush()
    {
      if ((int) this.bitsAvailable == 8)
        return;
      this.emptyBuffer();
    }

    public void reset()
    {
      this.buffer = (byte) 0;
      this.bitIndex = (byte) 0;
      this.bitsAvailable = (byte) 8;
    }
  }
}
