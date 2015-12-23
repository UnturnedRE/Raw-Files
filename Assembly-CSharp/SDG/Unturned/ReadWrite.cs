// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.ReadWrite
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

namespace SDG.Unturned
{
  public class ReadWrite
  {
    public static readonly string PATH = new DirectoryInfo(Application.dataPath).Parent.ToString();
    private static readonly XmlSerializerNamespaces XML_SERIALIZER_NAMESPACES = new XmlSerializerNamespaces(new XmlQualifiedName[1]
    {
      XmlQualifiedName.Empty
    });
    private static readonly XmlWriterSettings XML_WRITER_SETTINGS = new XmlWriterSettings()
    {
      Indent = true,
      OmitXmlDeclaration = true,
      Encoding = (Encoding) new UTF8Encoding()
    };

    public static bool appIn(byte[] h, byte p)
    {
      Block block = ReadWrite.readBlock("/Bundles/Sources/Animation/appout.log", false, (byte) 0);
      byte[] hash_1_1 = block.readByteArray();
      byte[] hash_1_2 = block.readByteArray();
      byte[] hash_1_3 = block.readByteArray();
      byte[] hash_1_4 = block.readByteArray();
      byte[] hash_1_5 = block.readByteArray();
      byte[] hash_1_6 = block.readByteArray();
      byte[] hash_1_7 = block.readByteArray();
      if ((int) p == 0)
      {
        if (Hash.verifyHash(h, hash_1_1))
          return true;
      }
      else if ((int) p == 1)
      {
        if (Hash.verifyHash(h, hash_1_2) || Hash.verifyHash(h, hash_1_3))
          return true;
      }
      else if ((int) p == 2)
      {
        if (Hash.verifyHash(h, hash_1_4) || Hash.verifyHash(h, hash_1_5))
          return true;
      }
      else if ((int) p == 3 && (Hash.verifyHash(h, hash_1_6) || Hash.verifyHash(h, hash_1_7)))
        return true;
      return false;
    }

    public static byte[] appOut()
    {
      FileStream fileStream = new FileStream(ReadWrite.PATH + "/Unturned_Data/Managed/Assembly-CSharp.dll", FileMode.Open, FileAccess.Read, FileShare.Read);
      byte[] numArray = new byte[fileStream.Length];
      fileStream.Read(numArray, 0, numArray.Length);
      fileStream.Close();
      fileStream.Dispose();
      return Hash.SHA1(numArray);
    }

    public static T deserializeJSON<T>(string path, bool useCloud)
    {
      return ReadWrite.deserializeJSON<T>(path, useCloud, true);
    }

    public static T deserializeJSON<T>(string path, bool useCloud, bool usePath)
    {
      T obj = default (T);
      byte[] bytes = ReadWrite.readBytes(path, useCloud, usePath);
      if (bytes == null)
        return obj;
      string @string = Encoding.UTF8.GetString(bytes);
      if (@string == null)
        return obj;
      return JsonConvert.DeserializeObject<T>(@string);
    }

    public static byte[] cloudFileRead(string path)
    {
      if (!ReadWrite.cloudFileExists(path))
        return (byte[]) null;
      int size;
      Provider.provider.cloudService.getSize(path, out size);
      byte[] data = new byte[size];
      if (Provider.provider.cloudService.read(path, data))
        return data;
      Debug.LogError((object) "Failed to read the correct file size.");
      return (byte[]) null;
    }

    public static void cloudFileWrite(string path, byte[] bytes, int size)
    {
      if (Provider.provider.cloudService.write(path, bytes, size))
        return;
      Debug.LogError((object) "Failed to write file.");
    }

    public static void cloudFileDelete(string path)
    {
      Provider.provider.cloudService.delete(path);
    }

    public static bool cloudFileExists(string path)
    {
      bool exists;
      Provider.provider.cloudService.exists(path, out exists);
      return exists;
    }

    public static void serializeJSON<T>(string path, bool useCloud, T instance)
    {
      ReadWrite.serializeJSON<T>(path, useCloud, true, instance);
    }

    public static void serializeJSON<T>(string path, bool useCloud, bool usePath, T instance)
    {
      byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject((object) instance, Newtonsoft.Json.Formatting.Indented));
      ReadWrite.writeBytes(path, useCloud, usePath, bytes, bytes.Length);
    }

    public static T deserializeXML<T>(string path, bool useCloud)
    {
      return ReadWrite.deserializeXML<T>(path, useCloud, true);
    }

    public static T deserializeXML<T>(string path, bool useCloud, bool usePath)
    {
      T obj = default (T);
      XmlSerializer xmlSerializer = new XmlSerializer(typeof (T));
      if (useCloud)
      {
        MemoryStream memoryStream = new MemoryStream(ReadWrite.cloudFileRead(path));
        try
        {
          return (T) xmlSerializer.Deserialize((Stream) memoryStream);
        }
        finally
        {
          memoryStream.Close();
          memoryStream.Dispose();
        }
      }
      else
      {
        if (usePath)
          path += ReadWrite.PATH;
        if (!Directory.Exists(Path.GetDirectoryName(path)))
          Directory.CreateDirectory(Path.GetDirectoryName(path));
        if (!File.Exists(path))
        {
          Debug.Log((object) ("Failed to find file at: " + path));
          return obj;
        }
        StreamReader streamReader = new StreamReader(path);
        try
        {
          return (T) xmlSerializer.Deserialize((TextReader) streamReader);
        }
        finally
        {
          streamReader.Close();
          streamReader.Dispose();
        }
      }
    }

    public static void serializeXML<T>(string path, bool useCloud, T instance)
    {
      ReadWrite.serializeXML<T>(path, useCloud, true, instance);
    }

    public static void serializeXML<T>(string path, bool useCloud, bool usePath, T instance)
    {
      XmlSerializer xmlSerializer = new XmlSerializer(typeof (T));
      if (useCloud)
      {
        MemoryStream memoryStream = new MemoryStream();
        XmlWriter xmlWriter = XmlWriter.Create((Stream) memoryStream, ReadWrite.XML_WRITER_SETTINGS);
        try
        {
          xmlSerializer.Serialize(xmlWriter, (object) instance, ReadWrite.XML_SERIALIZER_NAMESPACES);
          ReadWrite.cloudFileWrite(path, memoryStream.GetBuffer(), (int) memoryStream.Length);
        }
        finally
        {
          xmlWriter.Close();
          memoryStream.Close();
          memoryStream.Dispose();
        }
      }
      else
      {
        if (usePath)
          path = ReadWrite.PATH + path;
        if (!Directory.Exists(Path.GetDirectoryName(path)))
          Directory.CreateDirectory(Path.GetDirectoryName(path));
        StreamWriter streamWriter = new StreamWriter(path);
        try
        {
          xmlSerializer.Serialize((TextWriter) streamWriter, (object) instance, ReadWrite.XML_SERIALIZER_NAMESPACES);
        }
        finally
        {
          streamWriter.Close();
          streamWriter.Dispose();
        }
      }
    }

    public static byte[] readBytes(string path, bool useCloud)
    {
      return ReadWrite.readBytes(path, useCloud, true);
    }

    public static byte[] readBytes(string path, bool useCloud, bool usePath)
    {
      if (useCloud)
        return ReadWrite.cloudFileRead(path);
      if (usePath)
        path = ReadWrite.PATH + path;
      if (!Directory.Exists(Path.GetDirectoryName(path)))
        Directory.CreateDirectory(Path.GetDirectoryName(path));
      if (!File.Exists(path))
      {
        Debug.Log((object) ("Failed to find file at: " + path));
        return (byte[]) null;
      }
      FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
      byte[] array = new byte[fileStream.Length];
      if (fileStream.Read(array, 0, array.Length) != array.Length)
      {
        Debug.LogError((object) "Failed to read the correct file size.");
        return (byte[]) null;
      }
      fileStream.Close();
      fileStream.Dispose();
      return array;
    }

    public static Data readData(string path, bool useCloud)
    {
      return ReadWrite.readData(path, useCloud, true);
    }

    public static Data readData(string path, bool useCloud, bool usePath)
    {
      byte[] bytes = ReadWrite.readBytes(path, useCloud, usePath);
      if (bytes == null)
        return (Data) null;
      string @string = Encoding.UTF8.GetString(bytes);
      if (@string == string.Empty)
        return new Data();
      return new Data(@string);
    }

    public static Block readBlock(string path, bool useCloud, byte prefix)
    {
      return ReadWrite.readBlock(path, useCloud, true, prefix);
    }

    public static Block readBlock(string path, bool useCloud, bool usePath, byte prefix)
    {
      byte[] contents = ReadWrite.readBytes(path, useCloud, usePath);
      if (contents == null)
        return (Block) null;
      return new Block((int) prefix, contents);
    }

    public static void writeBytes(string path, bool useCloud, byte[] bytes)
    {
      ReadWrite.writeBytes(path, useCloud, true, bytes, bytes.Length);
    }

    public static void writeBytes(string path, bool useCloud, byte[] bytes, int size)
    {
      ReadWrite.writeBytes(path, useCloud, true, bytes, size);
    }

    public static void writeBytes(string path, bool useCloud, bool usePath, byte[] bytes)
    {
      ReadWrite.writeBytes(path, useCloud, usePath, bytes, bytes.Length);
    }

    public static void writeBytes(string path, bool useCloud, bool usePath, byte[] bytes, int size)
    {
      if (useCloud)
      {
        ReadWrite.cloudFileWrite(path, bytes, size);
      }
      else
      {
        if (usePath)
          path = ReadWrite.PATH + path;
        if (!Directory.Exists(Path.GetDirectoryName(path)))
          Directory.CreateDirectory(Path.GetDirectoryName(path));
        FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate);
        fileStream.Write(bytes, 0, size);
        fileStream.SetLength((long) size);
        fileStream.Flush();
        fileStream.Close();
        fileStream.Dispose();
      }
    }

    public static void writeData(string path, bool useCloud, Data data)
    {
      ReadWrite.writeData(path, useCloud, true, data);
    }

    public static void writeData(string path, bool useCloud, bool usePath, Data data)
    {
      byte[] bytes = Encoding.UTF8.GetBytes(data.getFile());
      ReadWrite.writeBytes(path, useCloud, usePath, bytes);
    }

    public static void writeBlock(string path, bool useCloud, Block block)
    {
      ReadWrite.writeBlock(path, useCloud, true, block);
    }

    public static void writeBlock(string path, bool useCloud, bool usePath, Block block)
    {
      int size;
      byte[] bytes = block.getBytes(out size);
      ReadWrite.writeBytes(path, useCloud, usePath, bytes, size);
    }

    public static void deleteFile(string path, bool useCloud)
    {
      ReadWrite.deleteFile(path, useCloud, true);
    }

    public static void deleteFile(string path, bool useCloud, bool usePath)
    {
      if (useCloud)
      {
        ReadWrite.cloudFileDelete(path);
      }
      else
      {
        if (usePath)
          path = ReadWrite.PATH + path;
        File.Delete(path);
      }
    }

    public static void deleteFolder(string path)
    {
      ReadWrite.deleteFolder(path, true);
    }

    public static void deleteFolder(string path, bool usePath)
    {
      if (usePath)
        path = ReadWrite.PATH + path;
      Directory.Delete(path, true);
    }

    public static void moveFolder(string origin, string target)
    {
      ReadWrite.moveFolder(origin, target, true);
    }

    public static void moveFolder(string origin, string target, bool usePath)
    {
      if (usePath)
      {
        origin = ReadWrite.PATH + origin;
        target = ReadWrite.PATH + target;
      }
      Directory.Move(origin, target);
    }

    public static void createFolder(string path)
    {
      ReadWrite.createFolder(path, true);
    }

    public static void createFolder(string path, bool usePath)
    {
      if (usePath)
        path = ReadWrite.PATH + path;
      if (Directory.Exists(path))
        return;
      Directory.CreateDirectory(path);
    }

    public static void createHidden(string path)
    {
      ReadWrite.createHidden(path, true);
    }

    public static void createHidden(string path, bool usePath)
    {
      if (usePath)
        path = ReadWrite.PATH + path;
      if (Directory.Exists(path))
        return;
      Directory.CreateDirectory(path).Attributes = FileAttributes.Hidden | FileAttributes.Directory;
    }

    public static string folderName(string path)
    {
      return new DirectoryInfo(path).Name;
    }

    public static string folderPath(string path)
    {
      return Path.GetDirectoryName(path);
    }

    public static void renameFile(string path_0, string path_1)
    {
      path_0 = ReadWrite.PATH + path_0;
      path_1 = ReadWrite.PATH + path_1;
      File.Move(path_0, path_1);
    }

    public static string fileName(string path)
    {
      return Path.GetFileNameWithoutExtension(path);
    }

    public static bool fileExists(string path, bool useCloud)
    {
      return ReadWrite.fileExists(path, useCloud, true);
    }

    public static bool fileExists(string path, bool useCloud, bool usePath)
    {
      if (useCloud)
        return ReadWrite.cloudFileExists(path);
      if (usePath)
        path = ReadWrite.PATH + path;
      return File.Exists(path);
    }

    public static string folderFound(string path)
    {
      return ReadWrite.folderFound(path, true);
    }

    public static string folderFound(string path, bool usePath)
    {
      if (usePath)
        path = ReadWrite.PATH + path;
      string[] directories = Directory.GetDirectories(path);
      if (directories.Length > 0)
        return directories[0];
      return (string) null;
    }

    public static bool folderExists(string path)
    {
      return ReadWrite.folderExists(path, true);
    }

    public static bool folderExists(string path, bool usePath)
    {
      if (usePath)
        path = ReadWrite.PATH + path;
      return Directory.Exists(path);
    }

    public static string[] getFolders(string path)
    {
      return ReadWrite.getFolders(path, true);
    }

    public static string[] getFolders(string path, bool usePath)
    {
      if (usePath)
        path = ReadWrite.PATH + path;
      return Directory.GetDirectories(path);
    }

    public static string[] getFiles(string path)
    {
      return ReadWrite.getFiles(path, true);
    }

    public static string[] getFiles(string path, bool usePath)
    {
      if (usePath)
        path = ReadWrite.PATH + path;
      return Directory.GetFiles(path);
    }

    public static void copyFile(string source, string destination)
    {
      source = ReadWrite.PATH + source;
      destination = ReadWrite.PATH + destination;
      if (!Directory.Exists(Path.GetDirectoryName(destination)))
        Directory.CreateDirectory(Path.GetDirectoryName(destination));
      File.Copy(source, destination);
    }
  }
}
