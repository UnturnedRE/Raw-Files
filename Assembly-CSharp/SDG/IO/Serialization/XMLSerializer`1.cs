// Decompiled with JetBrains decompiler
// Type: SDG.IO.Serialization.XMLSerializer`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SDG.IO.Serialization
{
  public class XMLSerializer<T> : ISerializer<T>
  {
    private static readonly XmlSerializerNamespaces XML_SERIALIZER_NAMESPACES = new XmlSerializerNamespaces(new XmlQualifiedName[1]
    {
      XmlQualifiedName.Empty
    });
    private static readonly XmlWriterSettings XML_WRITER_SETTINGS_FORMATTED = new XmlWriterSettings()
    {
      Indent = true,
      OmitXmlDeclaration = true,
      Encoding = (Encoding) new UTF8Encoding()
    };
    private static readonly XmlWriterSettings XML_WRITER_SETTINGS_UNFORMATTED = new XmlWriterSettings()
    {
      Indent = false,
      OmitXmlDeclaration = true,
      Encoding = (Encoding) new UTF8Encoding()
    };
    private XmlSerializer xmlSerializer;

    public XMLSerializer()
    {
      this.xmlSerializer = new XmlSerializer(typeof (T));
    }

    public void serialize(T instance, byte[] data, int index, out int size, bool isFormatted)
    {
      size = 0;
      MemoryStream memoryStream = new MemoryStream(data, index, data.Length - index);
      XmlWriter xmlWriter = XmlWriter.Create((Stream) memoryStream, !isFormatted ? XMLSerializer<T>.XML_WRITER_SETTINGS_UNFORMATTED : XMLSerializer<T>.XML_WRITER_SETTINGS_FORMATTED);
      try
      {
        this.xmlSerializer.Serialize(xmlWriter, (object) instance, XMLSerializer<T>.XML_SERIALIZER_NAMESPACES);
        xmlWriter.Flush();
        size = (int) memoryStream.Position;
      }
      finally
      {
        memoryStream.Close();
        memoryStream.Dispose();
      }
    }

    public void serialize(T instance, string path, bool isFormatted)
    {
      StreamWriter streamWriter = new StreamWriter(path);
      XmlWriter xmlWriter = XmlWriter.Create((TextWriter) streamWriter, !isFormatted ? XMLSerializer<T>.XML_WRITER_SETTINGS_UNFORMATTED : XMLSerializer<T>.XML_WRITER_SETTINGS_FORMATTED);
      try
      {
        this.xmlSerializer.Serialize(xmlWriter, (object) instance, XMLSerializer<T>.XML_SERIALIZER_NAMESPACES);
        xmlWriter.Flush();
      }
      finally
      {
        streamWriter.Close();
        streamWriter.Dispose();
      }
    }
  }
}
