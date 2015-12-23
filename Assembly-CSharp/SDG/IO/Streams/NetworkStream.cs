// Decompiled with JetBrains decompiler
// Type: SDG.IO.Streams.NetworkStream
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System.IO;

namespace SDG.IO.Streams
{
  public class NetworkStream
  {
    private Stream stream { get; set; }

    public NetworkStream(Stream newStream)
    {
      this.stream = newStream;
    }

    public sbyte readSByte()
    {
      return (sbyte) this.stream.ReadByte();
    }

    public byte readByte()
    {
      return (byte) this.stream.ReadByte();
    }

    public short readInt16()
    {
      return (short) ((int) this.readByte() << 8 | (int) this.readByte());
    }

    public ushort readUInt16()
    {
      return (ushort) ((uint) this.readByte() << 8 | (uint) this.readByte());
    }

    public int readInt32()
    {
      return (int) this.readByte() << 24 | (int) this.readByte() << 16 | (int) this.readByte() << 8 | (int) this.readByte();
    }

    public uint readUInt32()
    {
      return (uint) ((int) this.readByte() << 24 | (int) this.readByte() << 16 | (int) this.readByte() << 8) | (uint) this.readByte();
    }

    public long readInt64()
    {
      return (long) ((int) this.readByte() << 24 | (int) this.readByte() << 16 | (int) this.readByte() << 8 | (int) this.readByte() | (int) this.readByte() << 24 | (int) this.readByte() << 16 | (int) this.readByte() << 8 | (int) this.readByte());
    }

    public ulong readUInt64()
    {
      return (ulong) ((int) this.readByte() << 24 | (int) this.readByte() << 16 | (int) this.readByte() << 8 | (int) this.readByte() | (int) this.readByte() << 24 | (int) this.readByte() << 16 | (int) this.readByte() << 8 | (int) this.readByte());
    }

    public char readChar()
    {
      return (char) this.readUInt16();
    }

    public string readString()
    {
      ushort num = this.readUInt16();
      char[] chArray = new char[(int) num];
      for (ushort index = (ushort) 0; (int) index < (int) num; ++index)
      {
        char ch = this.readChar();
        chArray[(int) index] = ch;
      }
      return new string(chArray);
    }

    public void readBytes(byte[] data, ulong offset, ulong length)
    {
      this.stream.Read(data, (int) offset, (int) length);
    }

    public void writeSByte(sbyte data)
    {
      this.stream.WriteByte((byte) data);
    }

    public void writeByte(byte data)
    {
      this.stream.WriteByte(data);
    }

    public void writeInt16(short data)
    {
      this.writeByte((byte) ((uint) data >> 8));
      this.writeByte((byte) data);
    }

    public void writeUInt16(ushort data)
    {
      this.writeByte((byte) ((uint) data >> 8));
      this.writeByte((byte) data);
    }

    public void writeInt32(int data)
    {
      this.writeByte((byte) (data >> 24));
      this.writeByte((byte) (data >> 16));
      this.writeByte((byte) (data >> 8));
      this.writeByte((byte) data);
    }

    public void writeUInt32(uint data)
    {
      this.writeByte((byte) (data >> 24));
      this.writeByte((byte) (data >> 16));
      this.writeByte((byte) (data >> 8));
      this.writeByte((byte) data);
    }

    public void writeInt64(long data)
    {
      this.writeByte((byte) (data >> 56));
      this.writeByte((byte) (data >> 48));
      this.writeByte((byte) (data >> 40));
      this.writeByte((byte) (data >> 32));
      this.writeByte((byte) (data >> 24));
      this.writeByte((byte) (data >> 16));
      this.writeByte((byte) (data >> 8));
      this.writeByte((byte) data);
    }

    public void writeUInt64(ulong data)
    {
      this.writeByte((byte) (data >> 56));
      this.writeByte((byte) (data >> 48));
      this.writeByte((byte) (data >> 40));
      this.writeByte((byte) (data >> 32));
      this.writeByte((byte) (data >> 24));
      this.writeByte((byte) (data >> 16));
      this.writeByte((byte) (data >> 8));
      this.writeByte((byte) data);
    }

    public void writeChar(char data)
    {
      this.writeUInt16((ushort) data);
    }

    public void writeString(string data)
    {
      ushort data1 = (ushort) data.Length;
      char[] chArray = data.ToCharArray();
      this.writeUInt16(data1);
      for (ushort index = (ushort) 0; (int) index < (int) data1; ++index)
        this.writeChar(chArray[(int) index]);
    }

    public void writeBytes(byte[] data, ulong offset, ulong length)
    {
      this.stream.Write(data, (int) offset, (int) length);
    }
  }
}
