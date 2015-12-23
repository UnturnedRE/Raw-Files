// Decompiled with JetBrains decompiler
// Type: SDG.IO.Deserialization.XMLDeserializer`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System.IO;
using System.Xml.Serialization;

namespace SDG.IO.Deserialization
{
  public class XMLDeserializer<T> : IDeserializer<T>
  {
    private XmlSerializer xmlDeserializer;

    public XMLDeserializer()
    {
      this.xmlDeserializer = new XmlSerializer(typeof (T));
    }

    public T deserialize(byte[] data, int index)
    {
      T obj = default (T);
      MemoryStream memoryStream = new MemoryStream(data, index, data.Length - index);
      try
      {
        return (T) this.xmlDeserializer.Deserialize((Stream) memoryStream);
      }
      finally
      {
        memoryStream.Close();
        memoryStream.Dispose();
      }
    }

    public T deserialize(string path)
    {
      T obj = default (T);
      StreamReader streamReader = new StreamReader(path);
      try
      {
        return (T) this.xmlDeserializer.Deserialize((TextReader) streamReader);
      }
      finally
      {
        streamReader.Close();
        streamReader.Dispose();
      }
    }
  }
}
