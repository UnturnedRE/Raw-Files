// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.Block
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using System;
using System.Text;
using UnityEngine;

namespace SDG.Unturned
{
  public class Block
  {
    public static readonly int BUFFER_SIZE = 32768;
    private static byte[] buffer = new byte[Block.BUFFER_SIZE];
    private int step;
    private byte[] block;

    public Block(int prefix, byte[] contents)
    {
      this.reset(prefix, contents);
    }

    public Block(byte[] contents)
    {
      this.reset(contents);
    }

    public Block(int prefix)
    {
      this.reset(prefix);
    }

    public Block()
    {
      this.reset();
    }

    public string readString()
    {
      if (this.block == null || this.step >= this.block.Length)
        return string.Empty;
      string @string = Encoding.UTF8.GetString(this.block, this.step + 1, (int) this.block[this.step]);
      this.step = this.step + 1 + (int) this.block[this.step];
      return @string;
    }

    public bool readBoolean()
    {
      if (this.block == null || this.step > this.block.Length - 1)
        return false;
      bool flag = BitConverter.ToBoolean(this.block, this.step);
      ++this.step;
      return flag;
    }

    public bool[] readBooleanArray()
    {
      if (this.block == null || this.step >= this.block.Length)
        return new bool[0];
      bool[] flagArray = new bool[(int) this.readUInt16()];
      ushort num = (ushort) Mathf.CeilToInt((float) flagArray.Length / 8f);
      for (ushort index1 = (ushort) 0; (int) index1 < (int) num; ++index1)
      {
        for (byte index2 = (byte) 0; (int) index2 < 8 && (int) index1 * 8 + (int) index2 < flagArray.Length; ++index2)
          flagArray[(int) index1 * 8 + (int) index2] = ((int) this.block[this.step + (int) index1] & (int) Types.SHIFTS[(int) index2]) == (int) Types.SHIFTS[(int) index2];
      }
      this.step += (int) num;
      return flagArray;
    }

    public byte readByte()
    {
      if (this.block == null || this.step > this.block.Length - 1)
        return (byte) 0;
      byte num = this.block[this.step];
      ++this.step;
      return num;
    }

    public byte[] readByteArray()
    {
      if (this.block == null || this.step >= this.block.Length)
        return new byte[0];
      byte[] numArray = new byte[(int) this.block[this.step]];
      ++this.step;
      try
      {
        Buffer.BlockCopy((Array) this.block, this.step, (Array) numArray, 0, numArray.Length);
      }
      catch (ArgumentException ex)
      {
      }
      this.step += numArray.Length;
      return numArray;
    }

    public short readInt16()
    {
      if (this.block == null || this.step > this.block.Length - 2)
        return (short) 0;
      short num = BitConverter.ToInt16(this.block, this.step);
      this.step += 2;
      return num;
    }

    public ushort readUInt16()
    {
      if (this.block == null || this.step > this.block.Length - 2)
        return (ushort) 0;
      ushort num = BitConverter.ToUInt16(this.block, this.step);
      this.step += 2;
      return num;
    }

    public int readInt32()
    {
      if (this.block == null || this.step > this.block.Length - 4)
        return 0;
      int num = BitConverter.ToInt32(this.block, this.step);
      this.step += 4;
      return num;
    }

    public int[] readInt32Array()
    {
      ushort num1 = this.readUInt16();
      int[] numArray = new int[(int) num1];
      for (ushort index = (ushort) 0; (int) index < (int) num1; ++index)
      {
        int num2 = this.readInt32();
        numArray[(int) index] = num2;
      }
      return numArray;
    }

    public uint readUInt32()
    {
      if (this.block == null || this.step > this.block.Length - 4)
        return 0U;
      uint num = BitConverter.ToUInt32(this.block, this.step);
      this.step += 4;
      return num;
    }

    public float readSingle()
    {
      if (this.block == null || this.step > this.block.Length - 4)
        return 0.0f;
      float num = BitConverter.ToSingle(this.block, this.step);
      this.step += 4;
      return num;
    }

    public long readInt64()
    {
      if (this.block == null || this.step > this.block.Length - 8)
        return 0L;
      long num = BitConverter.ToInt64(this.block, this.step);
      this.step += 8;
      return num;
    }

    public ulong readUInt64()
    {
      if (this.block == null || this.step > this.block.Length - 8)
        return 0UL;
      ulong num = BitConverter.ToUInt64(this.block, this.step);
      this.step += 8;
      return num;
    }

    public ulong[] readUInt64Array()
    {
      ushort num1 = this.readUInt16();
      ulong[] numArray = new ulong[(int) num1];
      for (ushort index = (ushort) 0; (int) index < (int) num1; ++index)
      {
        ulong num2 = this.readUInt64();
        numArray[(int) index] = num2;
      }
      return numArray;
    }

    public CSteamID readSteamID()
    {
      return new CSteamID(this.readUInt64());
    }

    public Vector3 readSingleVector3()
    {
      return new Vector3(this.readSingle(), this.readSingle(), this.readSingle());
    }

    public Quaternion readSingleQuaternion()
    {
      return Quaternion.Euler(this.readSingle(), this.readSingle(), this.readSingle());
    }

    public Color readColor()
    {
      return new Color((float) this.readByte() / (float) byte.MaxValue, (float) this.readByte() / (float) byte.MaxValue, (float) this.readByte() / (float) byte.MaxValue);
    }

    public object read(System.Type type)
    {
      if (type == Types.STRING_TYPE)
        return (object) this.readString();
      if (type == Types.BOOLEAN_TYPE)
        return (object) (bool) (this.readBoolean() ? 1 : 0);
      if (type == Types.BOOLEAN_ARRAY_TYPE)
        return (object) this.readBooleanArray();
      if (type == Types.BYTE_TYPE)
        return (object) this.readByte();
      if (type == Types.BYTE_ARRAY_TYPE)
        return (object) this.readByteArray();
      if (type == Types.INT16_TYPE)
        return (object) this.readInt16();
      if (type == Types.UINT16_TYPE)
        return (object) this.readUInt16();
      if (type == Types.INT32_TYPE)
        return (object) this.readInt32();
      if (type == Types.INT32_ARRAY_TYPE)
        return (object) this.readInt32Array();
      if (type == Types.UINT32_TYPE)
        return (object) this.readUInt32();
      if (type == Types.SINGLE_TYPE)
        return (object) this.readSingle();
      if (type == Types.INT64_TYPE)
        return (object) this.readInt64();
      if (type == Types.UINT64_TYPE)
        return (object) this.readUInt64();
      if (type == Types.UINT64_ARRAY_TYPE)
        return (object) this.readUInt64Array();
      if (type == Types.STEAM_ID_TYPE)
        return (object) this.readSteamID();
      if (type == Types.VECTOR3_TYPE)
        return (object) this.readSingleVector3();
      if (type == Types.COLOR_TYPE)
        return (object) this.readColor();
      Debug.LogError((object) ("Failed to read type: " + (object) type));
      return (object) null;
    }

    public object[] read(int offset, params System.Type[] types)
    {
      object[] objArray = new object[types.Length];
      for (int index = offset; index < types.Length; ++index)
        objArray[index] = this.read(types[index]);
      return objArray;
    }

    public object[] read(params System.Type[] types)
    {
      return this.read(0, types);
    }

    public void writeString(string value)
    {
      byte[] bytes = Encoding.UTF8.GetBytes(value);
      Block.buffer[this.step] = (byte) bytes.Length;
      ++this.step;
      Buffer.BlockCopy((Array) bytes, 0, (Array) Block.buffer, this.step, bytes.Length);
      this.step += bytes.Length;
    }

    public void writeBoolean(bool value)
    {
      byte[] bytes = BitConverter.GetBytes(value);
      Block.buffer[this.step] = bytes[0];
      ++this.step;
    }

    public void writeBooleanArray(bool[] values)
    {
      this.writeUInt16((ushort) values.Length);
      ushort num = (ushort) Mathf.CeilToInt((float) values.Length / 8f);
      for (ushort index1 = (ushort) 0; (int) index1 < (int) num; ++index1)
      {
        Block.buffer[this.step + (int) index1] = (byte) 0;
        for (byte index2 = (byte) 0; (int) index2 < 8 && (int) index1 * 8 + (int) index2 < values.Length; ++index2)
        {
          if (values[(int) index1 * 8 + (int) index2])
            Block.buffer[this.step + (int) index1] |= Types.SHIFTS[(int) index2];
        }
      }
      this.step += (int) num;
    }

    public void writeByte(byte value)
    {
      Block.buffer[this.step] = value;
      ++this.step;
    }

    public void writeByteArray(byte[] values)
    {
      Block.buffer[this.step] = (byte) values.Length;
      ++this.step;
      Buffer.BlockCopy((Array) values, 0, (Array) Block.buffer, this.step, values.Length);
      this.step += values.Length;
    }

    public void writeInt16(short value)
    {
      byte[] bytes = BitConverter.GetBytes(value);
      Buffer.BlockCopy((Array) bytes, 0, (Array) Block.buffer, this.step, bytes.Length);
      this.step += 2;
    }

    public void writeUInt16(ushort value)
    {
      byte[] bytes = BitConverter.GetBytes(value);
      Buffer.BlockCopy((Array) bytes, 0, (Array) Block.buffer, this.step, bytes.Length);
      this.step += 2;
    }

    public void writeInt32(int value)
    {
      byte[] bytes = BitConverter.GetBytes(value);
      Buffer.BlockCopy((Array) bytes, 0, (Array) Block.buffer, this.step, bytes.Length);
      this.step += 4;
    }

    public void writeInt32Array(int[] values)
    {
      this.writeUInt16((ushort) values.Length);
      for (ushort index = (ushort) 0; (int) index < values.Length; ++index)
        this.writeInt32(values[(int) index]);
    }

    public void writeUInt32(uint value)
    {
      byte[] bytes = BitConverter.GetBytes(value);
      Buffer.BlockCopy((Array) bytes, 0, (Array) Block.buffer, this.step, bytes.Length);
      this.step += 4;
    }

    public void writeSingle(float value)
    {
      byte[] bytes = BitConverter.GetBytes(value);
      Buffer.BlockCopy((Array) bytes, 0, (Array) Block.buffer, this.step, bytes.Length);
      this.step += 4;
    }

    public void writeInt64(long value)
    {
      byte[] bytes = BitConverter.GetBytes(value);
      Buffer.BlockCopy((Array) bytes, 0, (Array) Block.buffer, this.step, bytes.Length);
      this.step += 8;
    }

    public void writeUInt64(ulong value)
    {
      byte[] bytes = BitConverter.GetBytes(value);
      Buffer.BlockCopy((Array) bytes, 0, (Array) Block.buffer, this.step, bytes.Length);
      this.step += 8;
    }

    public void writeUInt64Array(ulong[] values)
    {
      this.writeUInt16((ushort) values.Length);
      for (ushort index = (ushort) 0; (int) index < values.Length; ++index)
        this.writeUInt64(values[(int) index]);
    }

    public void writeSteamID(CSteamID steamID)
    {
      this.writeUInt64(steamID.m_SteamID);
    }

    public void writeSingleVector3(Vector3 value)
    {
      this.writeSingle(value.x);
      this.writeSingle(value.y);
      this.writeSingle(value.z);
    }

    public void writeSingleQuaternion(Quaternion value)
    {
      Vector3 eulerAngles = value.eulerAngles;
      this.writeSingle(eulerAngles.x);
      this.writeSingle(eulerAngles.y);
      this.writeSingle(eulerAngles.z);
    }

    public void writeColor(Color value)
    {
      this.writeByte((byte) ((double) value.r * (double) byte.MaxValue));
      this.writeByte((byte) ((double) value.g * (double) byte.MaxValue));
      this.writeByte((byte) ((double) value.b * (double) byte.MaxValue));
    }

    public void write(object objects)
    {
      System.Type type = objects.GetType();
      if (type == Types.STRING_TYPE)
        this.writeString((string) objects);
      else if (type == Types.BOOLEAN_TYPE)
        this.writeBoolean((bool) objects);
      else if (type == Types.BOOLEAN_ARRAY_TYPE)
        this.writeBooleanArray((bool[]) objects);
      else if (type == Types.BYTE_TYPE)
        this.writeByte((byte) objects);
      else if (type == Types.BYTE_ARRAY_TYPE)
        this.writeByteArray((byte[]) objects);
      else if (type == Types.INT16_TYPE)
        this.writeInt16((short) objects);
      else if (type == Types.UINT16_TYPE)
        this.writeUInt16((ushort) objects);
      else if (type == Types.INT32_TYPE)
        this.writeInt32((int) objects);
      else if (type == Types.INT32_ARRAY_TYPE)
        this.writeInt32Array((int[]) objects);
      else if (type == Types.UINT32_TYPE)
        this.writeUInt32((uint) objects);
      else if (type == Types.SINGLE_TYPE)
        this.writeSingle((float) objects);
      else if (type == Types.INT64_TYPE)
        this.writeInt64((long) objects);
      else if (type == Types.UINT64_TYPE)
        this.writeUInt64((ulong) objects);
      else if (type == Types.UINT64_ARRAY_TYPE)
        this.writeUInt64Array((ulong[]) objects);
      else if (type == Types.STEAM_ID_TYPE)
        this.writeSteamID((CSteamID) objects);
      else if (type == Types.VECTOR3_TYPE)
        this.writeSingleVector3((Vector3) objects);
      else if (type == Types.COLOR_TYPE)
        this.writeColor((Color) objects);
      else
        Debug.LogError((object) ("Failed to write type: " + (object) type));
    }

    public void write(params object[] objects)
    {
      for (int index = 0; index < objects.Length; ++index)
        this.write(objects[index]);
    }

    public byte[] getBytes(out int size)
    {
      if (this.block == null)
      {
        size = this.step;
        return Block.buffer;
      }
      size = this.block.Length;
      return this.block;
    }

    public byte[] getHash()
    {
      if (this.block == null)
        return Hash.SHA1(Block.buffer);
      return Hash.SHA1(this.block);
    }

    public void reset(int prefix, byte[] contents)
    {
      this.step = prefix;
      this.block = contents;
    }

    public void reset(byte[] contents)
    {
      this.step = 0;
      this.block = contents;
    }

    public void reset(int prefix)
    {
      this.step = prefix;
      this.block = (byte[]) null;
    }

    public void reset()
    {
      this.step = 0;
      this.block = (byte[]) null;
    }
  }
}
