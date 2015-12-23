// Decompiled with JetBrains decompiler
// Type: Pathfinding.Serialization.AstarSerializer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding;
using Pathfinding.Ionic.Zip;
using Pathfinding.Serialization.JsonFx;
using Pathfinding.Util;
using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace Pathfinding.Serialization
{
  public class AstarSerializer
  {
    private static StringBuilder _stringBuilder = new StringBuilder();
    private uint checksum = uint.MaxValue;
    private UTF8Encoding encoding = new UTF8Encoding();
    private const string binaryExt = ".binary";
    private const string jsonExt = ".json";
    private AstarData data;
    public JsonWriterSettings writerSettings;
    public JsonReaderSettings readerSettings;
    private ZipFile zip;
    private MemoryStream str;
    private GraphMeta meta;
    private SerializeSettings settings;
    private NavGraph[] graphs;

    public AstarSerializer(AstarData data)
    {
      this.data = data;
      this.settings = SerializeSettings.Settings;
    }

    public AstarSerializer(AstarData data, SerializeSettings settings)
    {
      this.data = data;
      this.settings = settings;
    }

    private static StringBuilder GetStringBuilder()
    {
      AstarSerializer._stringBuilder.Length = 0;
      return AstarSerializer._stringBuilder;
    }

    public void AddChecksum(byte[] bytes)
    {
      this.checksum = Checksum.GetChecksum(bytes, this.checksum);
    }

    public uint GetChecksum()
    {
      return this.checksum;
    }

    public void OpenSerialize()
    {
      this.zip = new ZipFile();
      this.zip.AlternateEncoding = Encoding.UTF8;
      this.zip.AlternateEncodingUsage = ZipOption.Always;
      this.writerSettings = new JsonWriterSettings();
      this.writerSettings.AddTypeConverter((JsonConverter) new VectorConverter());
      this.writerSettings.AddTypeConverter((JsonConverter) new BoundsConverter());
      this.writerSettings.AddTypeConverter((JsonConverter) new LayerMaskConverter());
      this.writerSettings.AddTypeConverter((JsonConverter) new MatrixConverter());
      this.writerSettings.AddTypeConverter((JsonConverter) new GuidConverter());
      this.writerSettings.AddTypeConverter((JsonConverter) new UnityObjectConverter());
      this.writerSettings.PrettyPrint = this.settings.prettyPrint;
      this.meta = new GraphMeta();
    }

    public byte[] CloseSerialize()
    {
      byte[] numArray1 = this.SerializeMeta();
      this.AddChecksum(numArray1);
      this.zip.AddEntry("meta.json", numArray1);
      MemoryStream memoryStream = new MemoryStream();
      this.zip.Save((Stream) memoryStream);
      byte[] numArray2 = memoryStream.ToArray();
      memoryStream.Dispose();
      this.zip.Dispose();
      this.zip = (ZipFile) null;
      return numArray2;
    }

    public void SerializeGraphs(NavGraph[] _graphs)
    {
      if (this.graphs != null)
        throw new InvalidOperationException("Cannot serialize graphs multiple times.");
      this.graphs = _graphs;
      if (this.zip == null)
        throw new NullReferenceException("You must not call CloseSerialize before a call to this function");
      if (this.graphs == null)
        this.graphs = new NavGraph[0];
      for (int index = 0; index < this.graphs.Length; ++index)
      {
        if (this.graphs[index] != null)
        {
          byte[] numArray = this.Serialize(this.graphs[index]);
          this.AddChecksum(numArray);
          this.zip.AddEntry("graph" + (object) index + ".json", numArray);
        }
      }
    }

    public void SerializeUserConnections(UserConnection[] conns)
    {
      if (conns == null)
        conns = new UserConnection[0];
      StringBuilder stringBuilder = AstarSerializer.GetStringBuilder();
      new JsonWriter(stringBuilder, this.writerSettings).Write((object) conns);
      byte[] bytes = this.encoding.GetBytes(stringBuilder.ToString());
      if (bytes.Length <= 2)
        return;
      this.AddChecksum(bytes);
      this.zip.AddEntry("connections.json", bytes);
    }

    private byte[] SerializeMeta()
    {
      this.meta.version = AstarPath.Version;
      this.meta.graphs = this.data.graphs.Length;
      this.meta.guids = new string[this.data.graphs.Length];
      this.meta.typeNames = new string[this.data.graphs.Length];
      this.meta.nodeCounts = new int[this.data.graphs.Length];
      for (int index = 0; index < this.data.graphs.Length; ++index)
      {
        if (this.data.graphs[index] != null)
        {
          this.meta.guids[index] = this.data.graphs[index].guid.ToString();
          this.meta.typeNames[index] = this.data.graphs[index].GetType().FullName;
        }
      }
      StringBuilder stringBuilder = AstarSerializer.GetStringBuilder();
      new JsonWriter(stringBuilder, this.writerSettings).Write((object) this.meta);
      return this.encoding.GetBytes(stringBuilder.ToString());
    }

    public byte[] Serialize(NavGraph graph)
    {
      StringBuilder stringBuilder = AstarSerializer.GetStringBuilder();
      new JsonWriter(stringBuilder, this.writerSettings).Write((object) graph);
      return this.encoding.GetBytes(stringBuilder.ToString());
    }

    public void SerializeNodes()
    {
      if (!this.settings.nodes)
        return;
      if (this.graphs == null)
        throw new InvalidOperationException("Cannot serialize nodes with no serialized graphs (call SerializeGraphs first)");
      for (int index = 0; index < this.graphs.Length; ++index)
      {
        byte[] numArray = this.SerializeNodes(index);
        this.AddChecksum(numArray);
        this.zip.AddEntry("graph" + (object) index + "_nodes.binary", numArray);
      }
      for (int index = 0; index < this.graphs.Length; ++index)
      {
        byte[] numArray = this.SerializeNodeConnections(index);
        this.AddChecksum(numArray);
        this.zip.AddEntry("graph" + (object) index + "_conns.binary", numArray);
      }
    }

    private byte[] SerializeNodes(int index)
    {
      return new byte[0];
    }

    public void SerializeExtraInfo()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AstarSerializer.\u003CSerializeExtraInfo\u003Ec__AnonStorey1F infoCAnonStorey1F = new AstarSerializer.\u003CSerializeExtraInfo\u003Ec__AnonStorey1F();
      if (!this.settings.nodes)
        return;
      // ISSUE: reference to a compiler-generated field
      infoCAnonStorey1F.totCount = 0;
      for (int index = 0; index < this.graphs.Length; ++index)
      {
        if (this.graphs[index] != null)
        {
          // ISSUE: reference to a compiler-generated method
          this.graphs[index].GetNodes(new GraphNodeDelegateCancelable(infoCAnonStorey1F.\u003C\u003Em__13));
        }
      }
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AstarSerializer.\u003CSerializeExtraInfo\u003Ec__AnonStorey20 infoCAnonStorey20 = new AstarSerializer.\u003CSerializeExtraInfo\u003Ec__AnonStorey20();
      MemoryStream memoryStream1 = new MemoryStream();
      // ISSUE: reference to a compiler-generated field
      infoCAnonStorey20.wr = new BinaryWriter((Stream) memoryStream1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      infoCAnonStorey20.wr.Write(infoCAnonStorey1F.totCount);
      // ISSUE: reference to a compiler-generated field
      infoCAnonStorey20.c = 0;
      for (int index = 0; index < this.graphs.Length; ++index)
      {
        if (this.graphs[index] != null)
        {
          // ISSUE: reference to a compiler-generated method
          this.graphs[index].GetNodes(new GraphNodeDelegateCancelable(infoCAnonStorey20.\u003C\u003Em__14));
        }
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (infoCAnonStorey20.c != infoCAnonStorey1F.totCount)
        throw new Exception("Some graphs are not consistent in their GetNodes calls, sequential calls give different results.");
      byte[] numArray1 = memoryStream1.ToArray();
      // ISSUE: reference to a compiler-generated field
      infoCAnonStorey20.wr.Close();
      this.AddChecksum(numArray1);
      this.zip.AddEntry("graph_references.binary", numArray1);
      for (int index = 0; index < this.graphs.Length; ++index)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        AstarSerializer.\u003CSerializeExtraInfo\u003Ec__AnonStorey21 infoCAnonStorey21 = new AstarSerializer.\u003CSerializeExtraInfo\u003Ec__AnonStorey21();
        if (this.graphs[index] != null)
        {
          MemoryStream memoryStream2 = new MemoryStream();
          BinaryWriter writer1 = new BinaryWriter((Stream) memoryStream2);
          // ISSUE: reference to a compiler-generated field
          infoCAnonStorey21.ctx = new GraphSerializationContext(writer1);
          // ISSUE: reference to a compiler-generated field
          this.graphs[index].SerializeExtraInfo(infoCAnonStorey21.ctx);
          byte[] numArray2 = memoryStream2.ToArray();
          writer1.Close();
          this.AddChecksum(numArray2);
          this.zip.AddEntry("graph" + (object) index + "_extra.binary", numArray2);
          MemoryStream memoryStream3 = new MemoryStream();
          BinaryWriter writer2 = new BinaryWriter((Stream) memoryStream3);
          // ISSUE: reference to a compiler-generated field
          infoCAnonStorey21.ctx = new GraphSerializationContext(writer2);
          // ISSUE: reference to a compiler-generated method
          this.graphs[index].GetNodes(new GraphNodeDelegateCancelable(infoCAnonStorey21.\u003C\u003Em__15));
          writer2.Close();
          byte[] numArray3 = memoryStream3.ToArray();
          this.AddChecksum(numArray3);
          this.zip.AddEntry("graph" + (object) index + "_references.binary", numArray3);
        }
      }
    }

    private byte[] SerializeNodeConnections(int index)
    {
      return new byte[0];
    }

    public void SerializeEditorSettings(GraphEditorBase[] editors)
    {
      if (editors == null || !this.settings.editorSettings)
        return;
      for (int index = 0; index < editors.Length && editors[index] != null; ++index)
      {
        StringBuilder stringBuilder = AstarSerializer.GetStringBuilder();
        new JsonWriter(stringBuilder, this.writerSettings).Write((object) editors[index]);
        byte[] bytes = this.encoding.GetBytes(stringBuilder.ToString());
        if (bytes.Length > 2)
        {
          this.AddChecksum(bytes);
          this.zip.AddEntry("graph" + (object) index + "_editor.json", bytes);
        }
      }
    }

    public bool OpenDeserialize(byte[] bytes)
    {
      this.readerSettings = new JsonReaderSettings();
      this.readerSettings.AddTypeConverter((JsonConverter) new VectorConverter());
      this.readerSettings.AddTypeConverter((JsonConverter) new BoundsConverter());
      this.readerSettings.AddTypeConverter((JsonConverter) new LayerMaskConverter());
      this.readerSettings.AddTypeConverter((JsonConverter) new MatrixConverter());
      this.readerSettings.AddTypeConverter((JsonConverter) new GuidConverter());
      this.readerSettings.AddTypeConverter((JsonConverter) new UnityObjectConverter());
      this.str = new MemoryStream();
      this.str.Write(bytes, 0, bytes.Length);
      this.str.Position = 0L;
      try
      {
        this.zip = ZipFile.Read((Stream) this.str);
      }
      catch (Exception ex)
      {
        Debug.LogWarning((object) ("Caught exception when loading from zip\n" + (object) ex));
        this.str.Dispose();
        return false;
      }
      this.meta = this.DeserializeMeta(this.zip["meta.json"]);
      if (this.meta.version > AstarPath.Version)
        Debug.LogWarning((object) string.Concat(new object[4]
        {
          (object) "Trying to load data from a newer version of the A* Pathfinding Project\nCurrent version: ",
          (object) AstarPath.Version,
          (object) " Data version: ",
          (object) this.meta.version
        }));
      else if (this.meta.version < AstarPath.Version)
        Debug.LogWarning((object) ("Trying to load data from an older version of the A* Pathfinding Project\nCurrent version: " + (object) AstarPath.Version + " Data version: " + (string) (object) this.meta.version + "\nThis is usually fine, it just means you have upgraded to a new version.\nHowever node data (not settings) can get corrupted between versions, so it is recommendedto recalculate any caches (those for faster startup) and resave any files. Even if it seems to load fine, it might cause subtle bugs.\n"));
      return true;
    }

    public void CloseDeserialize()
    {
      this.str.Dispose();
      this.zip.Dispose();
      this.zip = (ZipFile) null;
      this.str = (MemoryStream) null;
    }

    public NavGraph[] DeserializeGraphs()
    {
      this.graphs = new NavGraph[this.meta.graphs];
      int length = 0;
      for (int i = 0; i < this.meta.graphs; ++i)
      {
        System.Type graphType = this.meta.GetGraphType(i);
        if (!object.Equals((object) graphType, (object) null))
        {
          ++length;
          ZipEntry entry = this.zip["graph" + (object) i + ".json"];
          if (entry == null)
            throw new FileNotFoundException("Could not find data for graph " + (object) i + " in zip. Entry 'graph+" + (string) (object) i + ".json' does not exist");
          NavGraph graph = this.data.CreateGraph(graphType);
          new JsonReader(this.GetString(entry), this.readerSettings).PopulateObject<NavGraph>(ref graph);
          this.graphs[i] = graph;
          if (this.graphs[i].guid.ToString() != this.meta.guids[i])
            throw new Exception("Guid in graph file not equal to guid defined in meta file. Have you edited the data manually?\n" + this.graphs[i].guid.ToString() + " != " + this.meta.guids[i]);
        }
      }
      NavGraph[] navGraphArray = new NavGraph[length];
      int index1 = 0;
      for (int index2 = 0; index2 < this.graphs.Length; ++index2)
      {
        if (this.graphs[index2] != null)
        {
          navGraphArray[index1] = this.graphs[index2];
          ++index1;
        }
      }
      this.graphs = navGraphArray;
      return this.graphs;
    }

    public UserConnection[] DeserializeUserConnections()
    {
      ZipEntry entry = this.zip["connections.json"];
      if (entry == null)
        return new UserConnection[0];
      return (UserConnection[]) new JsonReader(this.GetString(entry), this.readerSettings).Deserialize(typeof (UserConnection[]));
    }

    public void DeserializeNodes()
    {
      for (int index = 0; index < this.graphs.Length; ++index)
      {
        if (this.graphs[index] == null || !this.zip.ContainsEntry("graph" + (object) index + "_nodes.binary"))
          ;
      }
      for (int index = 0; index < this.graphs.Length; ++index)
      {
        if (this.graphs[index] != null)
        {
          ZipEntry zipEntry = this.zip["graph" + (object) index + "_nodes.binary"];
          if (zipEntry != null)
          {
            MemoryStream memoryStream = new MemoryStream();
            zipEntry.Extract((Stream) memoryStream);
            memoryStream.Position = 0L;
            BinaryReader reader = new BinaryReader((Stream) memoryStream);
            this.DeserializeNodes(index, reader);
          }
        }
      }
      for (int index = 0; index < this.graphs.Length; ++index)
      {
        if (this.graphs[index] != null)
        {
          ZipEntry zipEntry = this.zip["graph" + (object) index + "_conns.binary"];
          if (zipEntry != null)
          {
            MemoryStream memoryStream = new MemoryStream();
            zipEntry.Extract((Stream) memoryStream);
            memoryStream.Position = 0L;
            BinaryReader reader = new BinaryReader((Stream) memoryStream);
            this.DeserializeNodeConnections(index, reader);
          }
        }
      }
    }

    public void DeserializeExtraInfo()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AstarSerializer.\u003CDeserializeExtraInfo\u003Ec__AnonStorey22 infoCAnonStorey22 = new AstarSerializer.\u003CDeserializeExtraInfo\u003Ec__AnonStorey22();
      bool flag = false;
      for (int graphIndex = 0; graphIndex < this.graphs.Length; ++graphIndex)
      {
        ZipEntry zipEntry = this.zip["graph" + (object) graphIndex + "_extra.binary"];
        if (zipEntry != null)
        {
          flag = true;
          MemoryStream memoryStream = new MemoryStream();
          zipEntry.Extract((Stream) memoryStream);
          memoryStream.Seek(0L, SeekOrigin.Begin);
          GraphSerializationContext ctx = new GraphSerializationContext(new BinaryReader((Stream) memoryStream), (GraphNode[]) null, graphIndex);
          this.graphs[graphIndex].DeserializeExtraInfo(ctx);
        }
      }
      if (!flag)
        return;
      // ISSUE: reference to a compiler-generated field
      infoCAnonStorey22.totCount = 0;
      for (int index = 0; index < this.graphs.Length; ++index)
      {
        if (this.graphs[index] != null)
        {
          // ISSUE: reference to a compiler-generated method
          this.graphs[index].GetNodes(new GraphNodeDelegateCancelable(infoCAnonStorey22.\u003C\u003Em__16));
        }
      }
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AstarSerializer.\u003CDeserializeExtraInfo\u003Ec__AnonStorey23 infoCAnonStorey23 = new AstarSerializer.\u003CDeserializeExtraInfo\u003Ec__AnonStorey23();
      ZipEntry zipEntry1 = this.zip["graph_references.binary"];
      if (zipEntry1 == null)
        throw new Exception("Node references not found in the data. Was this loaded from an older version of the A* Pathfinding Project?");
      MemoryStream memoryStream1 = new MemoryStream();
      zipEntry1.Extract((Stream) memoryStream1);
      memoryStream1.Seek(0L, SeekOrigin.Begin);
      // ISSUE: reference to a compiler-generated field
      infoCAnonStorey23.reader = new BinaryReader((Stream) memoryStream1);
      // ISSUE: reference to a compiler-generated field
      int num = infoCAnonStorey23.reader.ReadInt32();
      // ISSUE: reference to a compiler-generated field
      infoCAnonStorey23.int2Node = new GraphNode[num + 1];
      try
      {
        for (int index = 0; index < this.graphs.Length; ++index)
        {
          if (this.graphs[index] != null)
          {
            // ISSUE: reference to a compiler-generated method
            this.graphs[index].GetNodes(new GraphNodeDelegateCancelable(infoCAnonStorey23.\u003C\u003Em__17));
          }
        }
      }
      catch (Exception ex)
      {
        throw new Exception("Some graph(s) has thrown an exception during GetNodes, or some graph(s) have deserialized more or fewer nodes than were serialized", ex);
      }
      // ISSUE: reference to a compiler-generated field
      infoCAnonStorey23.reader.Close();
      for (int graphIndex = 0; graphIndex < this.graphs.Length; ++graphIndex)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        AstarSerializer.\u003CDeserializeExtraInfo\u003Ec__AnonStorey24 infoCAnonStorey24 = new AstarSerializer.\u003CDeserializeExtraInfo\u003Ec__AnonStorey24();
        if (this.graphs[graphIndex] != null)
        {
          ZipEntry zipEntry2 = this.zip["graph" + (object) graphIndex + "_references.binary"];
          if (zipEntry2 == null)
            throw new Exception("Node references for graph " + (object) graphIndex + " not found in the data. Was this loaded from an older version of the A* Pathfinding Project?");
          MemoryStream memoryStream2 = new MemoryStream();
          zipEntry2.Extract((Stream) memoryStream2);
          memoryStream2.Seek(0L, SeekOrigin.Begin);
          // ISSUE: reference to a compiler-generated field
          infoCAnonStorey23.reader = new BinaryReader((Stream) memoryStream2);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          infoCAnonStorey24.ctx = new GraphSerializationContext(infoCAnonStorey23.reader, infoCAnonStorey23.int2Node, graphIndex);
          // ISSUE: reference to a compiler-generated method
          this.graphs[graphIndex].GetNodes(new GraphNodeDelegateCancelable(infoCAnonStorey24.\u003C\u003Em__18));
        }
      }
    }

    public void PostDeserialization()
    {
      for (int index = 0; index < this.graphs.Length; ++index)
      {
        if (this.graphs[index] != null)
          this.graphs[index].PostDeserialization();
      }
    }

    private void DeserializeNodes(int index, BinaryReader reader)
    {
    }

    private void DeserializeNodeConnections(int index, BinaryReader reader)
    {
    }

    public void DeserializeEditorSettings(GraphEditorBase[] graphEditors)
    {
      if (graphEditors == null)
        return;
      for (int index1 = 0; index1 < graphEditors.Length; ++index1)
      {
        if (graphEditors[index1] != null)
        {
          for (int index2 = 0; index2 < this.graphs.Length; ++index2)
          {
            if (this.graphs[index2] != null && graphEditors[index1].target == this.graphs[index2])
            {
              ZipEntry entry = this.zip["graph" + (object) index2 + "_editor.json"];
              if (entry != null)
              {
                JsonReader jsonReader = new JsonReader(this.GetString(entry), this.readerSettings);
                GraphEditorBase graphEditorBase = graphEditors[index1];
                jsonReader.PopulateObject<GraphEditorBase>(ref graphEditorBase);
                graphEditors[index1] = graphEditorBase;
                break;
              }
            }
          }
        }
      }
    }

    private string GetString(ZipEntry entry)
    {
      MemoryStream memoryStream = new MemoryStream();
      entry.Extract((Stream) memoryStream);
      memoryStream.Position = 0L;
      StreamReader streamReader = new StreamReader((Stream) memoryStream);
      string str = streamReader.ReadToEnd();
      memoryStream.Position = 0L;
      streamReader.Dispose();
      return str;
    }

    private GraphMeta DeserializeMeta(ZipEntry entry)
    {
      if (entry == null)
        throw new Exception("No metadata found in serialized data.");
      return (GraphMeta) new JsonReader(this.GetString(entry), this.readerSettings).Deserialize(typeof (GraphMeta));
    }

    public static void SaveToFile(string path, byte[] data)
    {
      using (FileStream fileStream = new FileStream(path, FileMode.Create))
        fileStream.Write(data, 0, data.Length);
    }

    public static byte[] LoadFromFile(string path)
    {
      using (FileStream fileStream = new FileStream(path, FileMode.Open))
      {
        byte[] array = new byte[(int) fileStream.Length];
        fileStream.Read(array, 0, (int) fileStream.Length);
        return array;
      }
    }
  }
}
