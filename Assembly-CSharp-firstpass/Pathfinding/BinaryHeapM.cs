// Decompiled with JetBrains decompiler
// Type: Pathfinding.BinaryHeapM
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Pathfinding
{
  public class BinaryHeapM
  {
    public float growthFactor = 2f;
    public const int D = 4;
    private const bool SortGScores = true;
    public int numberOfItems;
    private BinaryHeapM.Tuple[] binaryHeap;

    public BinaryHeapM(int numberOfElements)
    {
      this.binaryHeap = new BinaryHeapM.Tuple[numberOfElements];
      this.numberOfItems = 0;
    }

    public void Clear()
    {
      this.numberOfItems = 0;
    }

    internal PathNode GetNode(int i)
    {
      return this.binaryHeap[i].node;
    }

    internal void SetF(int i, uint F)
    {
      this.binaryHeap[i].F = F;
    }

    public void Add(PathNode node)
    {
      if (node == null)
        throw new ArgumentNullException("Sending null node to BinaryHeap");
      if (this.numberOfItems == this.binaryHeap.Length)
      {
        int length = Math.Max(this.binaryHeap.Length + 4, (int) Math.Round((double) this.binaryHeap.Length * (double) this.growthFactor));
        if (length > 262144)
          throw new Exception("Binary Heap Size really large (2^18). A heap size this large is probably the cause of pathfinding running in an infinite loop. \nRemove this check (in BinaryHeap.cs) if you are sure that it is not caused by a bug");
        BinaryHeapM.Tuple[] tupleArray = new BinaryHeapM.Tuple[length];
        for (int index = 0; index < this.binaryHeap.Length; ++index)
          tupleArray[index] = this.binaryHeap[index];
        this.binaryHeap = tupleArray;
      }
      BinaryHeapM.Tuple tuple = new BinaryHeapM.Tuple(node.F, node);
      this.binaryHeap[this.numberOfItems] = tuple;
      int index1 = this.numberOfItems;
      uint f = node.F;
      uint g = node.G;
      int index2;
      for (; index1 != 0; index1 = index2)
      {
        index2 = (index1 - 1) / 4;
        if (f < this.binaryHeap[index2].F || (int) f == (int) this.binaryHeap[index2].F && g > this.binaryHeap[index2].node.G)
        {
          this.binaryHeap[index1] = this.binaryHeap[index2];
          this.binaryHeap[index2] = tuple;
        }
        else
          break;
      }
      ++this.numberOfItems;
    }

    public PathNode Remove()
    {
      --this.numberOfItems;
      PathNode pathNode = this.binaryHeap[0].node;
      this.binaryHeap[0] = this.binaryHeap[this.numberOfItems];
      int index1 = 0;
      while (true)
      {
        int index2 = index1;
        uint num1 = this.binaryHeap[index1].F;
        int index3 = index2 * 4 + 1;
        if (index3 <= this.numberOfItems && (this.binaryHeap[index3].F < num1 || (int) this.binaryHeap[index3].F == (int) num1 && this.binaryHeap[index3].node.G < this.binaryHeap[index1].node.G))
        {
          num1 = this.binaryHeap[index3].F;
          index1 = index3;
        }
        if (index3 + 1 <= this.numberOfItems && (this.binaryHeap[index3 + 1].F < num1 || (int) this.binaryHeap[index3 + 1].F == (int) num1 && this.binaryHeap[index3 + 1].node.G < this.binaryHeap[index1].node.G))
        {
          num1 = this.binaryHeap[index3 + 1].F;
          index1 = index3 + 1;
        }
        if (index3 + 2 <= this.numberOfItems && (this.binaryHeap[index3 + 2].F < num1 || (int) this.binaryHeap[index3 + 2].F == (int) num1 && this.binaryHeap[index3 + 2].node.G < this.binaryHeap[index1].node.G))
        {
          num1 = this.binaryHeap[index3 + 2].F;
          index1 = index3 + 2;
        }
        if (index3 + 3 <= this.numberOfItems && (this.binaryHeap[index3 + 3].F < num1 || (int) this.binaryHeap[index3 + 3].F == (int) num1 && this.binaryHeap[index3 + 3].node.G < this.binaryHeap[index1].node.G))
        {
          uint num2 = this.binaryHeap[index3 + 3].F;
          index1 = index3 + 3;
        }
        if (index2 != index1)
        {
          BinaryHeapM.Tuple tuple = this.binaryHeap[index2];
          this.binaryHeap[index2] = this.binaryHeap[index1];
          this.binaryHeap[index1] = tuple;
        }
        else
          break;
      }
      return pathNode;
    }

    private void Validate()
    {
      for (int index1 = 1; index1 < this.numberOfItems; ++index1)
      {
        int index2 = (index1 - 1) / 4;
        if (this.binaryHeap[index2].F > this.binaryHeap[index1].F)
          throw new Exception("Invalid state at " + (object) index1 + ":" + (string) (object) index2 + " ( " + (string) (object) this.binaryHeap[index2].F + " > " + (string) (object) this.binaryHeap[index1].F + " ) ");
      }
    }

    public void Rebuild()
    {
      for (int index1 = 2; index1 < this.numberOfItems; ++index1)
      {
        int index2 = index1;
        BinaryHeapM.Tuple tuple = this.binaryHeap[index1];
        uint num = tuple.F;
        int index3;
        for (; index2 != 1; index2 = index3)
        {
          index3 = index2 / 4;
          if (num < this.binaryHeap[index3].F)
          {
            this.binaryHeap[index2] = this.binaryHeap[index3];
            this.binaryHeap[index3] = tuple;
          }
          else
            break;
        }
      }
    }

    private struct Tuple
    {
      public uint F;
      public PathNode node;

      public Tuple(uint F, PathNode node)
      {
        this.F = F;
        this.node = node;
      }
    }
  }
}
