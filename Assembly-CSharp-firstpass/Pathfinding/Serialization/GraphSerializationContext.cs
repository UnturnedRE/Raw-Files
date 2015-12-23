// Decompiled with JetBrains decompiler
// Type: Pathfinding.Serialization.GraphSerializationContext
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding;
using System;
using System.IO;

namespace Pathfinding.Serialization
{
  public class GraphSerializationContext
  {
    private readonly GraphNode[] id2NodeMapping;
    public readonly BinaryReader reader;
    public readonly BinaryWriter writer;
    public readonly int graphIndex;

    public GraphSerializationContext(BinaryReader reader, GraphNode[] id2NodeMapping, int graphIndex)
    {
      this.reader = reader;
      this.id2NodeMapping = id2NodeMapping;
      this.graphIndex = graphIndex;
    }

    public GraphSerializationContext(BinaryWriter writer)
    {
      this.writer = writer;
    }

    public int GetNodeIdentifier(GraphNode node)
    {
      if (node == null)
        return -1;
      return node.NodeIndex;
    }

    public GraphNode GetNodeFromIdentifier(int id)
    {
      if (this.id2NodeMapping == null)
        throw new Exception("Calling GetNodeFromIdentifier when serializing");
      if (id == -1)
        return (GraphNode) null;
      GraphNode graphNode = this.id2NodeMapping[id];
      if (graphNode == null)
        throw new Exception("Invalid id");
      return graphNode;
    }
  }
}
