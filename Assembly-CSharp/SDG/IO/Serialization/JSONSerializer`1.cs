// Decompiled with JetBrains decompiler
// Type: SDG.IO.Serialization.JSONSerializer`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Newtonsoft.Json;
using System.IO;

namespace SDG.IO.Serialization
{
  public class JSONSerializer<T> : ISerializer<T>
  {
    private JsonSerializer jsonSerializer;

    public JSONSerializer()
    {
      this.jsonSerializer = new JsonSerializer();
    }

    public void serialize(T instance, byte[] data, int index, out int size, bool isFormatted)
    {
      this.jsonSerializer.Formatting = !isFormatted ? Formatting.None : Formatting.Indented;
      size = 0;
      MemoryStream memoryStream = new MemoryStream(data, index, data.Length - index);
      StreamWriter streamWriter = new StreamWriter((Stream) memoryStream);
      JsonWriter jsonWriter = (JsonWriter) new JsonTextWriter((TextWriter) streamWriter);
      try
      {
        this.jsonSerializer.Serialize(jsonWriter, (object) instance);
        jsonWriter.Flush();
        size = (int) memoryStream.Position;
      }
      finally
      {
        jsonWriter.Close();
        streamWriter.Close();
        streamWriter.Dispose();
        memoryStream.Close();
        memoryStream.Dispose();
      }
    }

    public void serialize(T instance, string path, bool isFormatted)
    {
      this.jsonSerializer.Formatting = !isFormatted ? Formatting.None : Formatting.Indented;
      StreamWriter streamWriter = new StreamWriter(path);
      JsonWriter jsonWriter = (JsonWriter) new JsonTextWriter((TextWriter) streamWriter);
      try
      {
        this.jsonSerializer.Serialize(jsonWriter, (object) instance);
        jsonWriter.Flush();
      }
      finally
      {
        jsonWriter.Close();
        streamWriter.Close();
        streamWriter.Dispose();
      }
    }
  }
}
