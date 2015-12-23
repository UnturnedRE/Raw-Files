// Decompiled with JetBrains decompiler
// Type: Pathfinding.Voxels.VoxelArea
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding;
using System;
using UnityEngine;

namespace Pathfinding.Voxels
{
  public class VoxelArea
  {
    private int[] removedStack = new int[128];
    public const uint MaxHeight = 65536U;
    public const int MaxHeightInt = 65536;
    public const uint InvalidSpanValue = 4294967295U;
    public const float AvgSpanLayerCountEstimate = 8f;
    public readonly int width;
    public readonly int depth;
    public CompactVoxelSpan[] compactSpans;
    public CompactVoxelCell[] compactCells;
    public int compactSpanCount;
    public ushort[] tmpUShortArr;
    public int[] areaTypes;
    public ushort[] dist;
    public ushort maxDistance;
    public int maxRegions;
    public int[] DirectionX;
    public int[] DirectionZ;
    public Vector3[] VectorDirection;
    private int linkedSpanCount;
    public LinkedVoxelSpan[] linkedSpans;
    private int removedStackCount;

    public VoxelArea(int width, int depth)
    {
      this.width = width;
      this.depth = depth;
      int length = width * depth;
      this.compactCells = new CompactVoxelCell[length];
      this.linkedSpans = new LinkedVoxelSpan[(int) ((double) length * 8.0) + 15 & -16];
      this.ResetLinkedVoxelSpans();
      this.DirectionX = new int[4]
      {
        -1,
        0,
        1,
        0
      };
      this.DirectionZ = new int[4]
      {
        0,
        width,
        0,
        -width
      };
      this.VectorDirection = new Vector3[4]
      {
        Vector3.left,
        Vector3.forward,
        Vector3.right,
        Vector3.back
      };
    }

    public void Reset()
    {
      this.ResetLinkedVoxelSpans();
      for (int index = 0; index < this.compactCells.Length; ++index)
      {
        this.compactCells[index].count = 0U;
        this.compactCells[index].index = 0U;
      }
    }

    private void ResetLinkedVoxelSpans()
    {
      int length = this.linkedSpans.Length;
      this.linkedSpanCount = this.width * this.depth;
      LinkedVoxelSpan linkedVoxelSpan = new LinkedVoxelSpan(uint.MaxValue, uint.MaxValue, -1, -1);
      int index1;
      for (int index2 = 0; index2 < length; index2 = index1 + 1)
      {
        this.linkedSpans[index2] = linkedVoxelSpan;
        int index3 = index2 + 1;
        this.linkedSpans[index3] = linkedVoxelSpan;
        int index4 = index3 + 1;
        this.linkedSpans[index4] = linkedVoxelSpan;
        int index5 = index4 + 1;
        this.linkedSpans[index5] = linkedVoxelSpan;
        int index6 = index5 + 1;
        this.linkedSpans[index6] = linkedVoxelSpan;
        int index7 = index6 + 1;
        this.linkedSpans[index7] = linkedVoxelSpan;
        int index8 = index7 + 1;
        this.linkedSpans[index8] = linkedVoxelSpan;
        int index9 = index8 + 1;
        this.linkedSpans[index9] = linkedVoxelSpan;
        int index10 = index9 + 1;
        this.linkedSpans[index10] = linkedVoxelSpan;
        int index11 = index10 + 1;
        this.linkedSpans[index11] = linkedVoxelSpan;
        int index12 = index11 + 1;
        this.linkedSpans[index12] = linkedVoxelSpan;
        int index13 = index12 + 1;
        this.linkedSpans[index13] = linkedVoxelSpan;
        int index14 = index13 + 1;
        this.linkedSpans[index14] = linkedVoxelSpan;
        int index15 = index14 + 1;
        this.linkedSpans[index15] = linkedVoxelSpan;
        int index16 = index15 + 1;
        this.linkedSpans[index16] = linkedVoxelSpan;
        index1 = index16 + 1;
        this.linkedSpans[index1] = linkedVoxelSpan;
      }
      this.removedStackCount = 0;
    }

    public int GetSpanCountAll()
    {
      int num1 = 0;
      int num2 = this.width * this.depth;
      for (int index1 = 0; index1 < num2; ++index1)
      {
        for (int index2 = index1; index2 != -1 && (int) this.linkedSpans[index2].bottom != -1; index2 = this.linkedSpans[index2].next)
          ++num1;
      }
      return num1;
    }

    public int GetSpanCount()
    {
      int num1 = 0;
      int num2 = this.width * this.depth;
      for (int index1 = 0; index1 < num2; ++index1)
      {
        for (int index2 = index1; index2 != -1 && (int) this.linkedSpans[index2].bottom != -1; index2 = this.linkedSpans[index2].next)
        {
          if (this.linkedSpans[index2].area != 0)
            ++num1;
        }
      }
      return num1;
    }

    public void AddLinkedSpan(int index, uint bottom, uint top, int area, int voxelWalkableClimb)
    {
      if ((int) this.linkedSpans[index].bottom == -1)
      {
        this.linkedSpans[index] = new LinkedVoxelSpan(bottom, top, area);
      }
      else
      {
        int index1 = -1;
        int index2 = index;
        while (index != -1 && this.linkedSpans[index].bottom <= top)
        {
          if (this.linkedSpans[index].top < bottom)
          {
            index1 = index;
            index = this.linkedSpans[index].next;
          }
          else
          {
            if (this.linkedSpans[index].bottom < bottom)
              bottom = this.linkedSpans[index].bottom;
            if (this.linkedSpans[index].top > top)
              top = this.linkedSpans[index].top;
            if (AstarMath.Abs((int) top - (int) this.linkedSpans[index].top) <= voxelWalkableClimb)
              area = AstarMath.Max(area, this.linkedSpans[index].area);
            int index3 = this.linkedSpans[index].next;
            if (index1 != -1)
            {
              this.linkedSpans[index1].next = index3;
              if (this.removedStackCount == this.removedStack.Length)
              {
                int[] numArray = new int[this.removedStackCount * 4];
                Buffer.BlockCopy((Array) this.removedStack, 0, (Array) numArray, 0, this.removedStackCount * 4);
                this.removedStack = numArray;
              }
              this.removedStack[this.removedStackCount] = index;
              ++this.removedStackCount;
              index = index3;
            }
            else if (index3 != -1)
            {
              this.linkedSpans[index2] = this.linkedSpans[index3];
              if (this.removedStackCount == this.removedStack.Length)
              {
                int[] numArray = new int[this.removedStackCount * 4];
                Buffer.BlockCopy((Array) this.removedStack, 0, (Array) numArray, 0, this.removedStackCount * 4);
                this.removedStack = numArray;
              }
              this.removedStack[this.removedStackCount] = index3;
              ++this.removedStackCount;
              index = this.linkedSpans[index2].next;
            }
            else
            {
              this.linkedSpans[index2] = new LinkedVoxelSpan(bottom, top, area);
              return;
            }
          }
        }
        if (this.linkedSpanCount >= this.linkedSpans.Length)
        {
          LinkedVoxelSpan[] linkedVoxelSpanArray = this.linkedSpans;
          int num1 = this.linkedSpanCount;
          int num2 = this.removedStackCount;
          this.linkedSpans = new LinkedVoxelSpan[this.linkedSpans.Length * 2];
          this.ResetLinkedVoxelSpans();
          this.linkedSpanCount = num1;
          this.removedStackCount = num2;
          for (int index3 = 0; index3 < this.linkedSpanCount; ++index3)
            this.linkedSpans[index3] = linkedVoxelSpanArray[index3];
          Debug.Log((object) "Layer estimate too low, doubling size of buffer.\nThis message is harmless.");
        }
        int next;
        if (this.removedStackCount > 0)
        {
          --this.removedStackCount;
          next = this.removedStack[this.removedStackCount];
        }
        else
        {
          next = this.linkedSpanCount;
          ++this.linkedSpanCount;
        }
        if (index1 != -1)
        {
          this.linkedSpans[next] = new LinkedVoxelSpan(bottom, top, area, this.linkedSpans[index1].next);
          this.linkedSpans[index1].next = next;
        }
        else
        {
          this.linkedSpans[next] = this.linkedSpans[index2];
          this.linkedSpans[index2] = new LinkedVoxelSpan(bottom, top, area, next);
        }
      }
    }
  }
}
