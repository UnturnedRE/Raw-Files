// Decompiled with JetBrains decompiler
// Type: SDG.IO.Streams.BitStreams.BitStreamReader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System.IO;

namespace SDG.IO.Streams.BitStreams
{
  public class BitStreamReader
  {
    public Stream stream { get; protected set; }

    private byte buffer { get; set; }

    private byte bitIndex { get; set; }

    private byte bitsAvailable { get; set; }

    public BitStreamReader(Stream newStream)
    {
      this.stream = newStream;
      this.reset();
    }

    public void readBit(ref byte data)
    {
      this.readBits(ref data, (byte) 1);
    }

    public void readBits(ref byte data, byte length)
    {
      if ((int) this.bitIndex == 8 && (int) this.bitsAvailable == 0)
        this.fillBuffer();
      if ((int) length > (int) this.bitsAvailable)
      {
        byte length1 = (byte) ((uint) length - (uint) this.bitsAvailable);
        this.readBits(ref data, this.bitsAvailable);
        data = (byte) ((uint) data << (int) length1);
        this.readBits(ref data, length1);
      }
      else
      {
        byte num1 = (byte) (8U - (uint) length - (uint) this.bitIndex);
        byte num2 = (byte) ((int) byte.MaxValue >> 8 - (int) length);
        data = (byte) ((uint) data | (uint) (byte) ((uint) this.buffer >> (int) num1 & (uint) num2));
        this.bitIndex += length;
        this.bitsAvailable -= length;
      }
    }

    private void fillBuffer()
    {
      this.buffer = (byte) this.stream.ReadByte();
      this.bitIndex = (byte) 0;
      this.bitsAvailable = (byte) 8;
    }

    public void reset()
    {
      this.buffer = (byte) 0;
      this.bitIndex = (byte) 8;
      this.bitsAvailable = (byte) 0;
    }
  }
}
