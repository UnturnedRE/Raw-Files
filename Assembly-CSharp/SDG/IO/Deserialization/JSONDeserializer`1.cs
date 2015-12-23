// Decompiled with JetBrains decompiler
// Type: SDG.IO.Deserialization.JSONDeserializer`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Newtonsoft.Json;
using System.IO;

namespace SDG.IO.Deserialization
{
  public class JSONDeserializer<T> : IDeserializer<T>
  {
    private JsonSerializer jsonDeserializer;

    public JSONDeserializer()
    {
      this.jsonDeserializer = new JsonSerializer();
    }

    public T deserialize(byte[] data, int index)
    {
      T obj = default (T);
      MemoryStream memoryStream = new MemoryStream(data, index, data.Length - index);
      StreamReader streamReader = new StreamReader((Stream) memoryStream);
      JsonReader reader = (JsonReader) new JsonTextReader((TextReader) streamReader);
      try
      {
        return this.jsonDeserializer.Deserialize<T>(reader);
      }
      finally
      {
        reader.Close();
        streamReader.Close();
        streamReader.Dispose();
        memoryStream.Close();
        memoryStream.Dispose();
      }
    }

    public T deserialize(string path)
    {
      T obj = default (T);
      StreamReader streamReader = new StreamReader(path);
      JsonReader reader = (JsonReader) new JsonTextReader((TextReader) streamReader);
      try
      {
        return this.jsonDeserializer.Deserialize<T>(reader);
      }
      finally
      {
        reader.Close();
        streamReader.Close();
        streamReader.Dispose();
      }
    }
  }
}
