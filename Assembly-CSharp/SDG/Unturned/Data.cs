// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.Data
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace SDG.Unturned
{
  public class Data
  {
    private Dictionary<string, string> data;

    public bool isEmpty
    {
      get
      {
        return this.data.Count == 0;
      }
    }

    public Data(string content)
    {
      this.data = new Dictionary<string, string>();
      StringReader stringReader = new StringReader(content);
      string str = string.Empty;
      string key;
      while ((key = stringReader.ReadLine()) != null)
      {
        if (!(key == string.Empty) && (key.Length <= 1 || !(key.Substring(0, 2) == "//")))
        {
          int length = key.IndexOf(' ');
          if (length != -1)
            this.data.Add(key.Substring(0, length), key.Substring(length + 1, key.Length - length - 1));
          else
            this.data.Add(key, string.Empty);
        }
      }
      stringReader.Close();
    }

    public Data()
    {
      this.data = new Dictionary<string, string>();
    }

    public string readString(string key)
    {
      string str;
      this.data.TryGetValue(key, out str);
      return str;
    }

    public bool readBoolean(string key)
    {
      return this.readString(key) == "y";
    }

    public byte readByte(string key)
    {
      byte result;
      byte.TryParse(this.readString(key), out result);
      return result;
    }

    public byte[] readByteArray(string key)
    {
      return Encoding.UTF8.GetBytes(this.readString(key));
    }

    public short readInt16(string key)
    {
      short result;
      short.TryParse(this.readString(key), out result);
      return result;
    }

    public ushort readUInt16(string key)
    {
      ushort result;
      ushort.TryParse(this.readString(key), out result);
      return result;
    }

    public int readInt32(string key)
    {
      int result;
      int.TryParse(this.readString(key), out result);
      return result;
    }

    public uint readUInt32(string key)
    {
      uint result;
      uint.TryParse(this.readString(key), out result);
      return result;
    }

    public long readInt64(string key)
    {
      long result;
      long.TryParse(this.readString(key), out result);
      return result;
    }

    public ulong readUInt64(string key)
    {
      ulong result;
      ulong.TryParse(this.readString(key), out result);
      return result;
    }

    public float readSingle(string key)
    {
      float result;
      float.TryParse(this.readString(key), out result);
      return result;
    }

    public Vector3 readVector3(string key)
    {
      return new Vector3(this.readSingle(key + "_X"), this.readSingle(key + "_Y"), this.readSingle(key + "_Z"));
    }

    public Quaternion readQuaternion(string key)
    {
      return Quaternion.Euler((float) ((int) this.readByte(key + "_X") * 2), (float) this.readByte(key + "_Y"), (float) this.readByte(key + "_Z"));
    }

    public Color readColor(string key)
    {
      return new Color(this.readSingle(key + "_R"), this.readSingle(key + "_G"), this.readSingle(key + "_B"));
    }

    public CSteamID readSteamID(string key)
    {
      return new CSteamID(this.readUInt64(key));
    }

    public void writeString(string key, string value)
    {
      this.data.Add(key, value);
    }

    public void writeBoolean(string key, bool value)
    {
      this.data.Add(key, !value ? "n" : "y");
    }

    public void writeByte(string key, byte value)
    {
      this.data.Add(key, value.ToString());
    }

    public void writeByteArray(string key, byte[] value)
    {
      this.data.Add(key, Encoding.UTF8.GetString(value));
    }

    public void writeInt16(string key, short value)
    {
      this.data.Add(key, value.ToString());
    }

    public void writeUInt16(string key, ushort value)
    {
      this.data.Add(key, value.ToString());
    }

    public void writeInt32(string key, int value)
    {
      this.data.Add(key, value.ToString());
    }

    public void writeUInt32(string key, uint value)
    {
      this.data.Add(key, value.ToString());
    }

    public void writeInt64(string key, long value)
    {
      this.data.Add(key, value.ToString());
    }

    public void writeUInt64(string key, ulong value)
    {
      this.data.Add(key, value.ToString());
    }

    public void writeSingle(string key, float value)
    {
      this.data.Add(key, (Mathf.Floor(value * 100f) / 100f).ToString());
    }

    public void writeVector3(string key, Vector3 value)
    {
      this.writeSingle(key + "_X", value.x);
      this.writeSingle(key + "_Y", value.y);
      this.writeSingle(key + "_Z", value.z);
    }

    public void writeQuaternion(string key, Quaternion value)
    {
      Vector3 eulerAngles = value.eulerAngles;
      this.writeByte(key + "_X", MeasurementTool.angleToByte(eulerAngles.x));
      this.writeByte(key + "_Y", MeasurementTool.angleToByte(eulerAngles.y));
      this.writeByte(key + "_Z", MeasurementTool.angleToByte(eulerAngles.z));
    }

    public void writeColor(string key, Color value)
    {
      this.writeSingle(key + "_R", value.r);
      this.writeSingle(key + "_G", value.g);
      this.writeSingle(key + "_B", value.b);
    }

    public void writeSteamID(string key, CSteamID value)
    {
      this.writeUInt64(key, value.m_SteamID);
    }

    public string getFile()
    {
      string str = string.Empty;
      using (Dictionary<string, string>.Enumerator enumerator = this.data.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<string, string> current = enumerator.Current;
          str = str + current.Key + " " + current.Value + "\n";
        }
      }
      return str;
    }

    public string[] getLines()
    {
      string[] strArray = new string[this.data.Count];
      int index = 0;
      using (Dictionary<string, string>.Enumerator enumerator = this.data.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<string, string> current = enumerator.Current;
          strArray[index] = current.Key + " " + current.Value;
          ++index;
        }
      }
      return strArray;
    }

    public KeyValuePair<string, string>[] getContents()
    {
      KeyValuePair<string, string>[] keyValuePairArray = new KeyValuePair<string, string>[this.data.Count];
      int index = 0;
      using (Dictionary<string, string>.Enumerator enumerator = this.data.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<string, string> current = enumerator.Current;
          keyValuePairArray[index] = current;
          ++index;
        }
      }
      return keyValuePairArray;
    }

    public string[] getValuesWithKey(string key)
    {
      List<string> list = new List<string>();
      using (Dictionary<string, string>.Enumerator enumerator = this.data.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<string, string> current = enumerator.Current;
          if (current.Key == key)
            list.Add(current.Value);
        }
      }
      return list.ToArray();
    }

    public string[] getKeysWithValue(string value)
    {
      List<string> list = new List<string>();
      using (Dictionary<string, string>.Enumerator enumerator = this.data.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<string, string> current = enumerator.Current;
          if (current.Value == value)
            list.Add(current.Key);
        }
      }
      return list.ToArray();
    }

    public bool has(string key)
    {
      return this.data.ContainsKey(key);
    }

    public void reset()
    {
      this.data.Clear();
    }
  }
}
