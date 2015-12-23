// Decompiled with JetBrains decompiler
// Type: Pathfinding.PathHandler
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Text;

namespace Pathfinding
{
  public class PathHandler
  {
    private BinaryHeapM heap = new BinaryHeapM(128);
    public PathNode[][] nodes = new PathNode[0][];
    private bool[] bucketNew = new bool[0];
    private bool[] bucketCreated = new bool[0];
    private Stack<PathNode[]> bucketCache = new Stack<PathNode[]>();
    public readonly StringBuilder DebugStringBuilder = new StringBuilder();
    private const int BucketSizeLog2 = 10;
    private const int BucketSize = 1024;
    private const int BucketIndexMask = 1023;
    private ushort pathID;
    public readonly int threadID;
    public readonly int totalThreadCount;
    private int filledBuckets;

    public ushort PathID
    {
      get
      {
        return this.pathID;
      }
    }

    public PathHandler(int threadID, int totalThreadCount)
    {
      this.threadID = threadID;
      this.totalThreadCount = totalThreadCount;
    }

    public void PushNode(PathNode node)
    {
      this.heap.Add(node);
    }

    public PathNode PopNode()
    {
      return this.heap.Remove();
    }

    public BinaryHeapM GetHeap()
    {
      return this.heap;
    }

    public void RebuildHeap()
    {
      this.heap.Rebuild();
    }

    public bool HeapEmpty()
    {
      return this.heap.numberOfItems <= 0;
    }

    public void InitializeForPath(Path p)
    {
      this.pathID = p.pathID;
      this.heap.Clear();
    }

    public void DestroyNode(GraphNode node)
    {
      PathNode pathNode = this.GetPathNode(node);
      pathNode.node = (GraphNode) null;
      pathNode.parent = (PathNode) null;
    }

    public void InitializeNode(GraphNode node)
    {
      int nodeIndex = node.NodeIndex;
      int index1 = nodeIndex >> 10;
      int index2 = nodeIndex & 1023;
      if (index1 >= this.nodes.Length)
      {
        PathNode[][] pathNodeArray = new PathNode[Math.Max(Math.Max(this.nodes.Length * 3 / 2, index1 + 1), this.nodes.Length + 2)][];
        for (int index3 = 0; index3 < this.nodes.Length; ++index3)
          pathNodeArray[index3] = this.nodes[index3];
        bool[] flagArray1 = new bool[pathNodeArray.Length];
        for (int index3 = 0; index3 < this.nodes.Length; ++index3)
          flagArray1[index3] = this.bucketNew[index3];
        bool[] flagArray2 = new bool[pathNodeArray.Length];
        for (int index3 = 0; index3 < this.nodes.Length; ++index3)
          flagArray2[index3] = this.bucketCreated[index3];
        this.nodes = pathNodeArray;
        this.bucketNew = flagArray1;
        this.bucketCreated = flagArray2;
      }
      if (this.nodes[index1] == null)
      {
        PathNode[] pathNodeArray;
        if (this.bucketCache.Count > 0)
        {
          pathNodeArray = this.bucketCache.Pop();
        }
        else
        {
          pathNodeArray = new PathNode[1024];
          for (int index3 = 0; index3 < 1024; ++index3)
            pathNodeArray[index3] = new PathNode();
        }
        this.nodes[index1] = pathNodeArray;
        if (!this.bucketCreated[index1])
        {
          this.bucketNew[index1] = true;
          this.bucketCreated[index1] = true;
        }
        ++this.filledBuckets;
      }
      this.nodes[index1][index2].node = node;
    }

    public PathNode GetPathNode(int nodeIndex)
    {
      return this.nodes[nodeIndex >> 10][nodeIndex & 1023];
    }

    public PathNode GetPathNode(GraphNode node)
    {
      int nodeIndex = node.NodeIndex;
      return this.nodes[nodeIndex >> 10][nodeIndex & 1023];
    }

    public void ClearPathIDs()
    {
      for (int index1 = 0; index1 < this.nodes.Length; ++index1)
      {
        PathNode[] pathNodeArray = this.nodes[index1];
        if (this.nodes[index1] != null)
        {
          for (int index2 = 0; index2 < 1024; ++index2)
            pathNodeArray[index2].pathID = (ushort) 0;
        }
      }
    }
  }
}
