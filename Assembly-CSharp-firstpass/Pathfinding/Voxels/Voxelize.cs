// Decompiled with JetBrains decompiler
// Type: Pathfinding.Voxels.Voxelize
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding;
using Pathfinding.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.Voxels
{
  public class Voxelize
  {
    private static List<int[]> intArrCache = new List<int[]>();
    private static readonly int[] emptyArr = new int[0];
    public readonly float cellSize = 0.2f;
    public readonly float cellHeight = 0.1f;
    public int minRegionSize = 100;
    public float maxEdgeLength = 20f;
    public float maxSlope = 30f;
    public string debugString = string.Empty;
    public const uint NotConnected = 63U;
    public const int MaxLayers = 65535;
    public const int MaxRegions = 500;
    public const int UnwalkableArea = 0;
    public const ushort BorderReg = (ushort) 32768;
    public const int RC_BORDER_VERTEX = 65536;
    public const int RC_AREA_BORDER = 131072;
    public const int VERTEX_BUCKET_COUNT = 4096;
    public const int RC_CONTOUR_TESS_WALL_EDGES = 1;
    public const int RC_CONTOUR_TESS_AREA_EDGES = 2;
    public const int ContourRegMask = 65535;
    public List<ExtraMesh> inputExtraMeshes;
    protected Vector3[] inputVertices;
    protected int[] inputTriangles;
    public readonly int voxelWalkableClimb;
    public readonly uint voxelWalkableHeight;
    public int borderSize;
    public RecastGraph.RelevantGraphSurfaceMode relevantGraphSurfaceMode;
    public Bounds forcedBounds;
    public VoxelArea voxelArea;
    public VoxelContourSet countourSet;
    public int width;
    public int depth;
    public Vector3 voxelOffset;
    public readonly Vector3 cellScale;
    public readonly Vector3 cellScaleDivision;

    public Voxelize(float ch, float cs, float wc, float wh, float ms)
    {
      this.cellSize = cs;
      this.cellHeight = ch;
      float num1 = wh;
      float num2 = wc;
      this.maxSlope = ms;
      this.cellScale = new Vector3(this.cellSize, this.cellHeight, this.cellSize);
      this.cellScaleDivision = new Vector3(1f / this.cellSize, 1f / this.cellHeight, 1f / this.cellSize);
      this.voxelWalkableHeight = (uint) ((double) num1 / (double) this.cellHeight);
      this.voxelWalkableClimb = Mathf.RoundToInt(num2 / this.cellHeight);
    }

    public void BuildContours(float maxError, int maxEdgeLength, VoxelContourSet cset, int buildFlags)
    {
      int num1 = this.voxelArea.width * this.voxelArea.depth;
      List<VoxelContour> list1 = new List<VoxelContour>(Mathf.Max(8, 8));
      ushort[] flags = this.voxelArea.tmpUShortArr;
      if (flags.Length < this.voxelArea.compactSpanCount)
        flags = this.voxelArea.tmpUShortArr = new ushort[this.voxelArea.compactSpanCount];
      int num2 = 0;
      while (num2 < num1)
      {
        for (int index1 = 0; index1 < this.voxelArea.width; ++index1)
        {
          CompactVoxelCell compactVoxelCell = this.voxelArea.compactCells[index1 + num2];
          int index2 = (int) compactVoxelCell.index;
          for (int index3 = (int) compactVoxelCell.index + (int) compactVoxelCell.count; index2 < index3; ++index2)
          {
            ushort num3 = (ushort) 0;
            CompactVoxelSpan compactVoxelSpan = this.voxelArea.compactSpans[index2];
            if (compactVoxelSpan.reg == 0 || (compactVoxelSpan.reg & 32768) == 32768)
            {
              flags[index2] = (ushort) 0;
            }
            else
            {
              for (int dir = 0; dir < 4; ++dir)
              {
                int num4 = 0;
                if ((long) compactVoxelSpan.GetConnection(dir) != 63L)
                  num4 = this.voxelArea.compactSpans[(int) this.voxelArea.compactCells[index1 + this.voxelArea.DirectionX[dir] + (num2 + this.voxelArea.DirectionZ[dir])].index + compactVoxelSpan.GetConnection(dir)].reg;
                if (num4 == compactVoxelSpan.reg)
                  num3 |= (ushort) (1 << dir);
              }
              flags[index2] = (ushort) ((uint) num3 ^ 15U);
            }
          }
        }
        num2 += this.voxelArea.width;
      }
      List<int> list2 = ListPool<int>.Claim(256);
      List<int> list3 = ListPool<int>.Claim(64);
      int z = 0;
      while (z < num1)
      {
        for (int x = 0; x < this.voxelArea.width; ++x)
        {
          CompactVoxelCell compactVoxelCell = this.voxelArea.compactCells[x + z];
          int i = (int) compactVoxelCell.index;
          for (int index1 = (int) compactVoxelCell.index + (int) compactVoxelCell.count; i < index1; ++i)
          {
            if ((int) flags[i] == 0 || (int) flags[i] == 15)
            {
              flags[i] = (ushort) 0;
            }
            else
            {
              int num3 = this.voxelArea.compactSpans[i].reg;
              if (num3 != 0 && (num3 & 32768) != 32768)
              {
                int num4 = this.voxelArea.areaTypes[i];
                list2.Clear();
                list3.Clear();
                this.WalkContour(x, z, i, flags, list2);
                this.SimplifyContour(list2, list3, maxError, maxEdgeLength, buildFlags);
                this.RemoveDegenerateSegments(list3);
                VoxelContour voxelContour = new VoxelContour();
                voxelContour.verts = Voxelize.ClaimIntArr(list3.Count, false);
                for (int index2 = 0; index2 < list3.Count; ++index2)
                  voxelContour.verts[index2] = list3[index2];
                voxelContour.nverts = list3.Count / 4;
                voxelContour.reg = num3;
                voxelContour.area = num4;
                list1.Add(voxelContour);
              }
            }
          }
        }
        z += this.voxelArea.width;
      }
      ListPool<int>.Release(list2);
      ListPool<int>.Release(list3);
      for (int index1 = 0; index1 < list1.Count; ++index1)
      {
        VoxelContour cb = list1[index1];
        if (this.CalcAreaOfPolygon2D(cb.verts, cb.nverts) < 0)
        {
          int index2 = -1;
          for (int index3 = 0; index3 < list1.Count; ++index3)
          {
            if (index1 != index3 && list1[index3].nverts > 0 && (list1[index3].reg == cb.reg && this.CalcAreaOfPolygon2D(list1[index3].verts, list1[index3].nverts) > 0))
            {
              index2 = index3;
              break;
            }
          }
          if (index2 == -1)
          {
            Debug.LogError((object) ("rcBuildContours: Could not find merge target for bad contour " + (object) index1 + "."));
          }
          else
          {
            Debug.LogWarning((object) "Fixing contour");
            VoxelContour ca = list1[index2];
            int ia = 0;
            int ib = 0;
            this.GetClosestIndices(ca.verts, ca.nverts, cb.verts, cb.nverts, ref ia, ref ib);
            if (ia == -1 || ib == -1)
              Debug.LogWarning((object) ("rcBuildContours: Failed to find merge points for " + (object) index1 + " and " + (string) (object) index2 + "."));
            else if (!Voxelize.MergeContours(ref ca, ref cb, ia, ib))
            {
              Debug.LogWarning((object) ("rcBuildContours: Failed to merge contours " + (object) index1 + " and " + (string) (object) index2 + "."));
            }
            else
            {
              list1[index2] = ca;
              list1[index1] = cb;
            }
          }
        }
      }
      cset.conts = list1;
    }

    private void GetClosestIndices(int[] vertsa, int nvertsa, int[] vertsb, int nvertsb, ref int ia, ref int ib)
    {
      int num1 = 268435455;
      ia = -1;
      ib = -1;
      for (int index1 = 0; index1 < nvertsa; ++index1)
      {
        int num2 = (index1 + 1) % nvertsa;
        int num3 = (index1 + nvertsa - 1) % nvertsa;
        int index2 = index1 * 4;
        int b = num2 * 4;
        int a = num3 * 4;
        for (int index3 = 0; index3 < nvertsb; ++index3)
        {
          int c = index3 * 4;
          if (Voxelize.Ileft(a, index2, c, vertsa, vertsa, vertsb) && Voxelize.Ileft(index2, b, c, vertsa, vertsa, vertsb))
          {
            int num4 = vertsb[c] - vertsa[index2];
            int num5 = vertsb[c + 2] / this.voxelArea.width - vertsa[index2 + 2] / this.voxelArea.width;
            int num6 = num4 * num4 + num5 * num5;
            if (num6 < num1)
            {
              ia = index1;
              ib = index3;
              num1 = num6;
            }
          }
        }
      }
    }

    private static void ReleaseIntArr(int[] arr)
    {
      if (arr == null)
        return;
      Voxelize.intArrCache.Add(arr);
    }

    private static int[] ClaimIntArr(int minCapacity, bool zero)
    {
      for (int index = 0; index < Voxelize.intArrCache.Count; ++index)
      {
        if (Voxelize.intArrCache[index].Length >= minCapacity)
        {
          int[] array = Voxelize.intArrCache[index];
          Voxelize.intArrCache.RemoveAt(index);
          if (zero)
            Memory.MemSet<int>(array, 0, 4);
          return array;
        }
      }
      return new int[minCapacity];
    }

    private static void ReleaseContours(VoxelContourSet cset)
    {
      for (int index = 0; index < cset.conts.Count; ++index)
      {
        VoxelContour voxelContour = cset.conts[index];
        Voxelize.ReleaseIntArr(voxelContour.verts);
        Voxelize.ReleaseIntArr(voxelContour.rverts);
      }
      cset.conts = (List<VoxelContour>) null;
    }

    public static bool MergeContours(ref VoxelContour ca, ref VoxelContour cb, int ia, int ib)
    {
      int[] numArray = Voxelize.ClaimIntArr((ca.nverts + cb.nverts + 2) * 4, false);
      int num = 0;
      for (int index1 = 0; index1 <= ca.nverts; ++index1)
      {
        int index2 = num * 4;
        int index3 = (ia + index1) % ca.nverts * 4;
        numArray[index2] = ca.verts[index3];
        numArray[index2 + 1] = ca.verts[index3 + 1];
        numArray[index2 + 2] = ca.verts[index3 + 2];
        numArray[index2 + 3] = ca.verts[index3 + 3];
        ++num;
      }
      for (int index1 = 0; index1 <= cb.nverts; ++index1)
      {
        int index2 = num * 4;
        int index3 = (ib + index1) % cb.nverts * 4;
        numArray[index2] = cb.verts[index3];
        numArray[index2 + 1] = cb.verts[index3 + 1];
        numArray[index2 + 2] = cb.verts[index3 + 2];
        numArray[index2 + 3] = cb.verts[index3 + 3];
        ++num;
      }
      Voxelize.ReleaseIntArr(ca.verts);
      Voxelize.ReleaseIntArr(cb.verts);
      ca.verts = numArray;
      ca.nverts = num;
      cb.verts = Voxelize.emptyArr;
      cb.nverts = 0;
      return true;
    }

    public void SimplifyContour(List<int> verts, List<int> simplified, float maxError, int maxEdgeLenght, int buildFlags)
    {
      bool flag1 = false;
      int num1 = 0;
      while (num1 < verts.Count)
      {
        if ((verts[num1 + 3] & (int) ushort.MaxValue) != 0)
        {
          flag1 = true;
          break;
        }
        num1 += 4;
      }
      if (flag1)
      {
        int num2 = 0;
        for (int index = verts.Count / 4; num2 < index; ++num2)
        {
          int num3 = (num2 + 1) % index;
          if ((verts[num2 * 4 + 3] & (int) ushort.MaxValue) != (verts[num3 * 4 + 3] & (int) ushort.MaxValue) || (verts[num2 * 4 + 3] & 131072) != (verts[num3 * 4 + 3] & 131072))
          {
            simplified.Add(verts[num2 * 4]);
            simplified.Add(verts[num2 * 4 + 1]);
            simplified.Add(verts[num2 * 4 + 2]);
            simplified.Add(num2);
          }
        }
      }
      if (simplified.Count == 0)
      {
        int num2 = verts[0];
        int num3 = verts[1];
        int num4 = verts[2];
        int num5 = 0;
        int num6 = verts[0];
        int num7 = verts[1];
        int num8 = verts[2];
        int num9 = 0;
        int index = 0;
        while (index < verts.Count)
        {
          int num10 = verts[index];
          int num11 = verts[index + 1];
          int num12 = verts[index + 2];
          if (num10 < num2 || num10 == num2 && num12 < num4)
          {
            num2 = num10;
            num3 = num11;
            num4 = num12;
            num5 = index / 4;
          }
          if (num10 > num6 || num10 == num6 && num12 > num8)
          {
            num6 = num10;
            num7 = num11;
            num8 = num12;
            num9 = index / 4;
          }
          index += 4;
        }
        simplified.Add(num2);
        simplified.Add(num3);
        simplified.Add(num4);
        simplified.Add(num5);
        simplified.Add(num6);
        simplified.Add(num7);
        simplified.Add(num8);
        simplified.Add(num9);
      }
      int num13 = verts.Count / 4;
      maxError *= maxError;
      int num14 = 0;
      while (num14 < simplified.Count / 4)
      {
        int num2 = (num14 + 1) % (simplified.Count / 4);
        int px = simplified[num14 * 4];
        int num3 = simplified[num14 * 4 + 2];
        int num4 = simplified[num14 * 4 + 3];
        int qx = simplified[num2 * 4];
        int num5 = simplified[num2 * 4 + 2];
        int num6 = simplified[num2 * 4 + 3];
        float num7 = 0.0f;
        int num8 = -1;
        int num9;
        int num10;
        int num11;
        if (qx > px || qx == px && num5 > num3)
        {
          num9 = 1;
          num10 = (num4 + num9) % num13;
          num11 = num6;
        }
        else
        {
          num9 = num13 - 1;
          num10 = (num6 + num9) % num13;
          num11 = num4;
        }
        if ((verts[num10 * 4 + 3] & (int) ushort.MaxValue) == 0 || (verts[num10 * 4 + 3] & 131072) == 131072)
        {
          for (; num10 != num11; num10 = (num10 + num9) % num13)
          {
            float num12 = AstarMath.DistancePointSegment(verts[num10 * 4], verts[num10 * 4 + 2] / this.voxelArea.width, px, num3 / this.voxelArea.width, qx, num5 / this.voxelArea.width);
            if ((double) num12 > (double) num7)
            {
              num7 = num12;
              num8 = num10;
            }
          }
        }
        if (num8 != -1 && (double) num7 > (double) maxError)
        {
          simplified.Add(0);
          simplified.Add(0);
          simplified.Add(0);
          simplified.Add(0);
          for (int index = simplified.Count / 4 - 1; index > num14; --index)
          {
            simplified[index * 4] = simplified[(index - 1) * 4];
            simplified[index * 4 + 1] = simplified[(index - 1) * 4 + 1];
            simplified[index * 4 + 2] = simplified[(index - 1) * 4 + 2];
            simplified[index * 4 + 3] = simplified[(index - 1) * 4 + 3];
          }
          simplified[(num14 + 1) * 4] = verts[num8 * 4];
          simplified[(num14 + 1) * 4 + 1] = verts[num8 * 4 + 1];
          simplified[(num14 + 1) * 4 + 2] = verts[num8 * 4 + 2];
          simplified[(num14 + 1) * 4 + 3] = num8;
        }
        else
          ++num14;
      }
      float num15 = this.maxEdgeLength / this.cellSize;
      if ((double) num15 > 0.0 && (buildFlags & 3) != 0)
      {
        int num2 = 0;
        while (num2 < simplified.Count / 4 && simplified.Count / 4 <= 200)
        {
          int num3 = (num2 + 1) % (simplified.Count / 4);
          int num4 = simplified[num2 * 4];
          int num5 = simplified[num2 * 4 + 2];
          int num6 = simplified[num2 * 4 + 3];
          int num7 = simplified[num3 * 4];
          int num8 = simplified[num3 * 4 + 2];
          int num9 = simplified[num3 * 4 + 3];
          int num10 = -1;
          int num11 = (num6 + 1) % num13;
          bool flag2 = false;
          if ((buildFlags & 1) == 1 && (verts[num11 * 4 + 3] & (int) ushort.MaxValue) == 0)
            flag2 = true;
          if ((buildFlags & 2) == 1 && (verts[num11 * 4 + 3] & 131072) == 1)
            flag2 = true;
          if (flag2)
          {
            int num12 = num7 - num4;
            int num16 = num8 / this.voxelArea.width - num5 / this.voxelArea.width;
            if ((double) (num12 * num12 + num16 * num16) > (double) num15 * (double) num15)
            {
              if (num7 > num4 || num7 == num4 && num8 > num5)
              {
                int num17 = num9 >= num6 ? num9 - num6 : num9 + num13 - num6;
                num10 = (num6 + num17 / 2) % num13;
              }
              else
              {
                int num17 = num9 >= num6 ? num9 - num6 : num9 + num13 - num6;
                num10 = (num6 + (num17 + 1) / 2) % num13;
              }
            }
          }
          if (num10 != -1)
          {
            simplified.AddRange((IEnumerable<int>) new int[4]);
            for (int index = simplified.Count / 4 - 1; index > num2; --index)
            {
              simplified[index * 4] = simplified[(index - 1) * 4];
              simplified[index * 4 + 1] = simplified[(index - 1) * 4 + 1];
              simplified[index * 4 + 2] = simplified[(index - 1) * 4 + 2];
              simplified[index * 4 + 3] = simplified[(index - 1) * 4 + 3];
            }
            simplified[(num2 + 1) * 4] = verts[num10 * 4];
            simplified[(num2 + 1) * 4 + 1] = verts[num10 * 4 + 1];
            simplified[(num2 + 1) * 4 + 2] = verts[num10 * 4 + 2];
            simplified[(num2 + 1) * 4 + 3] = num10;
          }
          else
            ++num2;
        }
      }
      for (int index = 0; index < simplified.Count / 4; ++index)
      {
        int num2 = (simplified[index * 4 + 3] + 1) % num13;
        int num3 = simplified[index * 4 + 3];
        simplified[index * 4 + 3] = verts[num2 * 4 + 3] & (int) ushort.MaxValue | verts[num3 * 4 + 3] & 65536;
      }
    }

    public void WalkContour(int x, int z, int i, ushort[] flags, List<int> verts)
    {
      int dir = 0;
      while (((int) flags[i] & (int) (ushort) (1 << dir)) == 0)
        ++dir;
      int num1 = dir;
      int num2 = i;
      int num3 = this.voxelArea.areaTypes[i];
      int num4 = 0;
      while (num4++ < 40000)
      {
        if (((int) flags[i] & (int) (ushort) (1 << dir)) != 0)
        {
          bool isBorderVertex = false;
          bool flag = false;
          int num5 = x;
          int cornerHeight = this.GetCornerHeight(x, z, i, dir, ref isBorderVertex);
          int num6 = z;
          switch (dir)
          {
            case 0:
              num6 += this.voxelArea.width;
              break;
            case 1:
              ++num5;
              num6 += this.voxelArea.width;
              break;
            case 2:
              ++num5;
              break;
          }
          int num7 = 0;
          CompactVoxelSpan compactVoxelSpan = this.voxelArea.compactSpans[i];
          if ((long) compactVoxelSpan.GetConnection(dir) != 63L)
          {
            int index = (int) this.voxelArea.compactCells[x + this.voxelArea.DirectionX[dir] + (z + this.voxelArea.DirectionZ[dir])].index + compactVoxelSpan.GetConnection(dir);
            num7 = this.voxelArea.compactSpans[index].reg;
            if (num3 != this.voxelArea.areaTypes[index])
              flag = true;
          }
          if (isBorderVertex)
            num7 |= 65536;
          if (flag)
            num7 |= 131072;
          verts.Add(num5);
          verts.Add(cornerHeight);
          verts.Add(num6);
          verts.Add(num7);
          flags[i] = (ushort) ((uint) flags[i] & (uint) ~(1 << dir));
          dir = dir + 1 & 3;
        }
        else
        {
          int num5 = -1;
          int num6 = x + this.voxelArea.DirectionX[dir];
          int num7 = z + this.voxelArea.DirectionZ[dir];
          CompactVoxelSpan compactVoxelSpan = this.voxelArea.compactSpans[i];
          if ((long) compactVoxelSpan.GetConnection(dir) != 63L)
            num5 = (int) this.voxelArea.compactCells[num6 + num7].index + compactVoxelSpan.GetConnection(dir);
          if (num5 == -1)
          {
            Debug.LogError((object) "This should not happen");
            break;
          }
          x = num6;
          z = num7;
          i = num5;
          dir = dir + 3 & 3;
        }
        if (num2 == i && num1 == dir)
          break;
      }
    }

    public int GetCornerHeight(int x, int z, int i, int dir, ref bool isBorderVertex)
    {
      CompactVoxelSpan compactVoxelSpan1 = this.voxelArea.compactSpans[i];
      int a = (int) compactVoxelSpan1.y;
      int dir1 = dir + 1 & 3;
      uint[] numArray = new uint[4];
      numArray[0] = (uint) (this.voxelArea.compactSpans[i].reg | this.voxelArea.areaTypes[i] << 16);
      if ((long) compactVoxelSpan1.GetConnection(dir) != 63L)
      {
        int num1 = x + this.voxelArea.DirectionX[dir];
        int num2 = z + this.voxelArea.DirectionZ[dir];
        int index1 = (int) this.voxelArea.compactCells[num1 + num2].index + compactVoxelSpan1.GetConnection(dir);
        CompactVoxelSpan compactVoxelSpan2 = this.voxelArea.compactSpans[index1];
        a = AstarMath.Max(a, (int) compactVoxelSpan2.y);
        numArray[1] = (uint) (compactVoxelSpan2.reg | this.voxelArea.areaTypes[index1] << 16);
        if ((long) compactVoxelSpan2.GetConnection(dir1) != 63L)
        {
          int index2 = (int) this.voxelArea.compactCells[num1 + this.voxelArea.DirectionX[dir1] + (num2 + this.voxelArea.DirectionZ[dir1])].index + compactVoxelSpan2.GetConnection(dir1);
          CompactVoxelSpan compactVoxelSpan3 = this.voxelArea.compactSpans[index2];
          a = AstarMath.Max(a, (int) compactVoxelSpan3.y);
          numArray[2] = (uint) (compactVoxelSpan3.reg | this.voxelArea.areaTypes[index2] << 16);
        }
      }
      if ((long) compactVoxelSpan1.GetConnection(dir1) != 63L)
      {
        int num1 = x + this.voxelArea.DirectionX[dir1];
        int num2 = z + this.voxelArea.DirectionZ[dir1];
        int index1 = (int) this.voxelArea.compactCells[num1 + num2].index + compactVoxelSpan1.GetConnection(dir1);
        CompactVoxelSpan compactVoxelSpan2 = this.voxelArea.compactSpans[index1];
        a = AstarMath.Max(a, (int) compactVoxelSpan2.y);
        numArray[3] = (uint) (compactVoxelSpan2.reg | this.voxelArea.areaTypes[index1] << 16);
        if ((long) compactVoxelSpan2.GetConnection(dir) != 63L)
        {
          int index2 = (int) this.voxelArea.compactCells[num1 + this.voxelArea.DirectionX[dir] + (num2 + this.voxelArea.DirectionZ[dir])].index + compactVoxelSpan2.GetConnection(dir);
          CompactVoxelSpan compactVoxelSpan3 = this.voxelArea.compactSpans[index2];
          a = AstarMath.Max(a, (int) compactVoxelSpan3.y);
          numArray[2] = (uint) (compactVoxelSpan3.reg | this.voxelArea.areaTypes[index2] << 16);
        }
      }
      for (int index1 = 0; index1 < 4; ++index1)
      {
        int index2 = index1;
        int index3 = index1 + 1 & 3;
        int index4 = index1 + 2 & 3;
        int index5 = index1 + 3 & 3;
        if (((int) numArray[index2] & (int) numArray[index3] & 32768) != 0 && (int) numArray[index2] == (int) numArray[index3] && (((int) numArray[index4] | (int) numArray[index5]) & 32768) == 0 && ((int) (numArray[index4] >> 16) == (int) (numArray[index5] >> 16) && ((int) numArray[index2] != 0 && (int) numArray[index3] != 0 && (int) numArray[index4] != 0 && (int) numArray[index5] != 0)))
        {
          isBorderVertex = true;
          break;
        }
      }
      return a;
    }

    public void RemoveDegenerateSegments(List<int> simplified)
    {
      for (int index = 0; index < simplified.Count / 4; ++index)
      {
        int num = index + 1;
        if (num >= simplified.Count / 4)
          num = 0;
        if (simplified[index * 4] == simplified[num * 4] && simplified[index * 4 + 2] == simplified[num * 4 + 2])
          simplified.RemoveRange(index, 4);
      }
    }

    public int CalcAreaOfPolygon2D(int[] verts, int nverts)
    {
      int num1 = 0;
      int num2 = 0;
      int num3 = nverts - 1;
      for (; num2 < nverts; num3 = num2++)
      {
        int index1 = num2 * 4;
        int index2 = num3 * 4;
        num1 += verts[index1] * (verts[index2 + 2] / this.voxelArea.width) - verts[index2] * (verts[index1 + 2] / this.voxelArea.width);
      }
      return (num1 + 1) / 2;
    }

    public static bool Ileft(int a, int b, int c, int[] va, int[] vb, int[] vc)
    {
      return (vb[b] - va[a]) * (vc[c + 2] - va[a + 2]) - (vc[c] - va[a]) * (vb[b + 2] - va[a + 2]) <= 0;
    }

    public static bool Diagonal(int i, int j, int n, int[] verts, int[] indices)
    {
      if (Voxelize.InCone(i, j, n, verts, indices))
        return Voxelize.Diagonalie(i, j, n, verts, indices);
      return false;
    }

    public static bool InCone(int i, int j, int n, int[] verts, int[] indices)
    {
      int num1 = (indices[i] & 268435455) * 4;
      int num2 = (indices[j] & 268435455) * 4;
      int c = (indices[Voxelize.Next(i, n)] & 268435455) * 4;
      int num3 = (indices[Voxelize.Prev(i, n)] & 268435455) * 4;
      if (!Voxelize.LeftOn(num3, num1, c, verts))
        return (!Voxelize.LeftOn(num1, num2, c, verts) ? 0 : (Voxelize.LeftOn(num2, num1, num3, verts) ? 1 : 0)) == 0;
      if (Voxelize.Left(num1, num2, num3, verts))
        return Voxelize.Left(num2, num1, c, verts);
      return false;
    }

    public static bool Left(int a, int b, int c, int[] verts)
    {
      return Voxelize.Area2(a, b, c, verts) < 0;
    }

    public static bool LeftOn(int a, int b, int c, int[] verts)
    {
      return Voxelize.Area2(a, b, c, verts) <= 0;
    }

    public static bool Collinear(int a, int b, int c, int[] verts)
    {
      return Voxelize.Area2(a, b, c, verts) == 0;
    }

    public static int Area2(int a, int b, int c, int[] verts)
    {
      return (verts[b] - verts[a]) * (verts[c + 2] - verts[a + 2]) - (verts[c] - verts[a]) * (verts[b + 2] - verts[a + 2]);
    }

    private static bool Diagonalie(int i, int j, int n, int[] verts, int[] indices)
    {
      int a = (indices[i] & 268435455) * 4;
      int num1 = (indices[j] & 268435455) * 4;
      for (int i1 = 0; i1 < n; ++i1)
      {
        int index = Voxelize.Next(i1, n);
        if (i1 != i && index != i && (i1 != j && index != j))
        {
          int num2 = (indices[i1] & 268435455) * 4;
          int num3 = (indices[index] & 268435455) * 4;
          if (!Voxelize.Vequal(a, num2, verts) && !Voxelize.Vequal(num1, num2, verts) && (!Voxelize.Vequal(a, num3, verts) && !Voxelize.Vequal(num1, num3, verts)) && Voxelize.Intersect(a, num1, num2, num3, verts))
            return false;
        }
      }
      return true;
    }

    public static bool Xorb(bool x, bool y)
    {
      return !x ^ !y;
    }

    public static bool IntersectProp(int a, int b, int c, int d, int[] verts)
    {
      if (Voxelize.Collinear(a, b, c, verts) || Voxelize.Collinear(a, b, d, verts) || (Voxelize.Collinear(c, d, a, verts) || Voxelize.Collinear(c, d, b, verts)) || !Voxelize.Xorb(Voxelize.Left(a, b, c, verts), Voxelize.Left(a, b, d, verts)))
        return false;
      return Voxelize.Xorb(Voxelize.Left(c, d, a, verts), Voxelize.Left(c, d, b, verts));
    }

    private static bool Between(int a, int b, int c, int[] verts)
    {
      if (!Voxelize.Collinear(a, b, c, verts))
        return false;
      if (verts[a] != verts[b])
      {
        if (verts[a] <= verts[c] && verts[c] <= verts[b])
          return true;
        if (verts[a] >= verts[c])
          return verts[c] >= verts[b];
        return false;
      }
      if (verts[a + 2] <= verts[c + 2] && verts[c + 2] <= verts[b + 2])
        return true;
      if (verts[a + 2] >= verts[c + 2])
        return verts[c + 2] >= verts[b + 2];
      return false;
    }

    private static bool Intersect(int a, int b, int c, int d, int[] verts)
    {
      return Voxelize.IntersectProp(a, b, c, d, verts) || Voxelize.Between(a, b, c, verts) || (Voxelize.Between(a, b, d, verts) || Voxelize.Between(c, d, a, verts)) || Voxelize.Between(c, d, b, verts);
    }

    private static bool Vequal(int a, int b, int[] verts)
    {
      if (verts[a] == verts[b])
        return verts[a + 2] == verts[b + 2];
      return false;
    }

    public static int Prev(int i, int n)
    {
      if (i - 1 >= 0)
        return i - 1;
      return n - 1;
    }

    public static int Next(int i, int n)
    {
      if (i + 1 < n)
        return i + 1;
      return 0;
    }

    public void BuildPolyMesh(VoxelContourSet cset, int nvp, out VoxelMesh mesh)
    {
      nvp = 3;
      int length1 = 0;
      int num1 = 0;
      int a = 0;
      for (int index = 0; index < cset.conts.Count; ++index)
      {
        if (cset.conts[index].nverts >= 3)
        {
          length1 += cset.conts[index].nverts;
          num1 += cset.conts[index].nverts - 2;
          a = AstarMath.Max(a, cset.conts[index].nverts);
        }
      }
      if (length1 >= 65534)
        Debug.LogWarning((object) "To many vertices for unity to render - Unity might screw up rendering, but hopefully the navmesh will work ok");
      Int3[] int3Array1 = new Int3[length1];
      int[] array = new int[num1 * nvp];
      Memory.MemSet<int>(array, (int) byte.MaxValue, 4);
      int[] indices = new int[a];
      int[] tris = new int[a * 3];
      int length2 = 0;
      int length3 = 0;
      for (int index1 = 0; index1 < cset.conts.Count; ++index1)
      {
        VoxelContour voxelContour = cset.conts[index1];
        if (voxelContour.nverts >= 3)
        {
          for (int index2 = 0; index2 < voxelContour.nverts; ++index2)
          {
            indices[index2] = index2;
            voxelContour.verts[index2 * 4 + 2] /= this.voxelArea.width;
          }
          int num2 = this.Triangulate(voxelContour.nverts, voxelContour.verts, ref indices, ref tris);
          int num3 = length2;
          for (int index2 = 0; index2 < num2 * 3; ++index2)
          {
            array[length3] = tris[index2] + num3;
            ++length3;
          }
          for (int index2 = 0; index2 < voxelContour.nverts; ++index2)
          {
            int3Array1[length2] = new Int3(voxelContour.verts[index2 * 4], voxelContour.verts[index2 * 4 + 1], voxelContour.verts[index2 * 4 + 2]);
            ++length2;
          }
        }
      }
      mesh = new VoxelMesh();
      Int3[] int3Array2 = new Int3[length2];
      for (int index = 0; index < length2; ++index)
        int3Array2[index] = int3Array1[index];
      int[] numArray = new int[length3];
      Buffer.BlockCopy((Array) array, 0, (Array) numArray, 0, length3 * 4);
      mesh.verts = int3Array2;
      mesh.tris = numArray;
    }

    public int Triangulate(int n, int[] verts, ref int[] indices, ref int[] tris)
    {
      int num1 = 0;
      int[] numArray = tris;
      int index1 = 0;
      int n1 = n;
      for (int i1 = 0; i1 < n; ++i1)
      {
        int i2 = Voxelize.Next(i1, n);
        int j = Voxelize.Next(i2, n);
        if (Voxelize.Diagonal(i1, j, n, verts, indices))
          indices[i2] |= 1073741824;
      }
      while (n > 3)
      {
        int num2 = -1;
        int num3 = -1;
        for (int i1 = 0; i1 < n; ++i1)
        {
          int i2 = Voxelize.Next(i1, n);
          if ((indices[i2] & 1073741824) != 0)
          {
            int index2 = (indices[i1] & 268435455) * 4;
            int index3 = (indices[Voxelize.Next(i2, n)] & 268435455) * 4;
            int num4 = verts[index3] - verts[index2];
            int num5 = verts[index3 + 2] - verts[index2 + 2];
            int num6 = num4 * num4 + num5 * num5;
            if (num2 < 0 || num6 < num2)
            {
              num2 = num6;
              num3 = i1;
            }
          }
        }
        if (num3 == -1)
        {
          Debug.LogError((object) "This should not happen");
          for (int index2 = 0; index2 < n1; ++index2)
            this.DrawLine(Voxelize.Prev(index2, n1), index2, indices, verts, Color.red);
          return -num1;
        }
        int i3 = num3;
        int index4 = Voxelize.Next(i3, n);
        int index5 = Voxelize.Next(index4, n);
        numArray[index1] = indices[i3] & 268435455;
        int index6 = index1 + 1;
        numArray[index6] = indices[index4] & 268435455;
        int index7 = index6 + 1;
        numArray[index7] = indices[index5] & 268435455;
        index1 = index7 + 1;
        ++num1;
        --n;
        for (int index2 = index4; index2 < n; ++index2)
          indices[index2] = indices[index2 + 1];
        if (index4 >= n)
          index4 = 0;
        int i4 = Voxelize.Prev(index4, n);
        if (Voxelize.Diagonal(Voxelize.Prev(i4, n), index4, n, verts, indices))
          indices[i4] |= 1073741824;
        else
          indices[i4] &= 268435455;
        if (Voxelize.Diagonal(i4, Voxelize.Next(index4, n), n, verts, indices))
          indices[index4] |= 1073741824;
        else
          indices[index4] &= 268435455;
      }
      numArray[index1] = indices[0] & 268435455;
      int index8 = index1 + 1;
      numArray[index8] = indices[1] & 268435455;
      int index9 = index8 + 1;
      numArray[index9] = indices[2] & 268435455;
      int num7 = index9 + 1;
      return num1 + 1;
    }

    public Vector3 CompactSpanToVector(int x, int z, int i)
    {
      return this.voxelOffset + new Vector3((float) x * this.cellSize, (float) this.voxelArea.compactSpans[i].y * this.cellHeight, (float) z * this.cellSize);
    }

    public void VectorToIndex(Vector3 p, out int x, out int z)
    {
      p -= this.voxelOffset;
      x = Mathf.RoundToInt(p.x / this.cellSize);
      z = Mathf.RoundToInt(p.z / this.cellSize);
    }

    public void OnGUI()
    {
      GUI.Label(new Rect(5f, 5f, 200f, (float) Screen.height), this.debugString);
    }

    public void CollectMeshes()
    {
      Voxelize.CollectMeshes(this.inputExtraMeshes, this.forcedBounds, out this.inputVertices, out this.inputTriangles);
    }

    public static void CollectMeshes(List<ExtraMesh> extraMeshes, Bounds bounds, out Vector3[] verts, out int[] tris)
    {
      verts = (Vector3[]) null;
      tris = (int[]) null;
    }

    public void Init()
    {
      if (this.voxelArea == null || this.voxelArea.width != this.width || this.voxelArea.depth != this.depth)
        this.voxelArea = new VoxelArea(this.width, this.depth);
      else
        this.voxelArea.Reset();
    }

    public void VoxelizeInput()
    {
      // ISSUE: unable to decompile the method.
    }

    public void BuildCompactField()
    {
      int spanCount = this.voxelArea.GetSpanCount();
      this.voxelArea.compactSpanCount = spanCount;
      if (this.voxelArea.compactSpans == null || this.voxelArea.compactSpans.Length < spanCount)
      {
        this.voxelArea.compactSpans = new CompactVoxelSpan[spanCount];
        this.voxelArea.areaTypes = new int[spanCount];
      }
      uint num1 = 0U;
      int num2 = this.voxelArea.width;
      int num3 = this.voxelArea.depth;
      int num4 = num2 * num3;
      if (this.voxelWalkableHeight >= (uint) ushort.MaxValue)
        Debug.LogWarning((object) "Too high walkable height to guarantee correctness. Increase voxel height or lower walkable height.");
      LinkedVoxelSpan[] linkedVoxelSpanArray = this.voxelArea.linkedSpans;
      int num5 = 0;
      int num6 = 0;
      while (num5 < num4)
      {
        for (int index1 = 0; index1 < num2; ++index1)
        {
          int index2 = index1 + num5;
          if ((int) linkedVoxelSpanArray[index2].bottom == -1)
          {
            this.voxelArea.compactCells[index1 + num5] = new CompactVoxelCell(0U, 0U);
          }
          else
          {
            uint i = num1;
            uint c = 0U;
            for (; index2 != -1; index2 = linkedVoxelSpanArray[index2].next)
            {
              if (linkedVoxelSpanArray[index2].area != 0)
              {
                int num7 = (int) linkedVoxelSpanArray[index2].top;
                int index3 = linkedVoxelSpanArray[index2].next;
                int num8 = index3 == -1 ? 65536 : (int) linkedVoxelSpanArray[index3].bottom;
                this.voxelArea.compactSpans[(IntPtr) num1] = new CompactVoxelSpan(num7 <= (int) ushort.MaxValue ? (ushort) num7 : ushort.MaxValue, num8 - num7 <= (int) ushort.MaxValue ? (uint) (num8 - num7) : (uint) ushort.MaxValue);
                this.voxelArea.areaTypes[(IntPtr) num1] = linkedVoxelSpanArray[index2].area;
                ++num1;
                ++c;
              }
            }
            this.voxelArea.compactCells[index1 + num5] = new CompactVoxelCell(i, c);
          }
        }
        num5 += num2;
        ++num6;
      }
    }

    public void BuildVoxelConnections()
    {
      int num1 = this.voxelArea.width * this.voxelArea.depth;
      CompactVoxelSpan[] compactVoxelSpanArray = this.voxelArea.compactSpans;
      CompactVoxelCell[] compactVoxelCellArray = this.voxelArea.compactCells;
      int num2 = 0;
      int num3 = 0;
      while (num2 < num1)
      {
        for (int index1 = 0; index1 < this.voxelArea.width; ++index1)
        {
          CompactVoxelCell compactVoxelCell1 = compactVoxelCellArray[index1 + num2];
          int index2 = (int) compactVoxelCell1.index;
          for (int index3 = (int) compactVoxelCell1.index + (int) compactVoxelCell1.count; index2 < index3; ++index2)
          {
            CompactVoxelSpan compactVoxelSpan1 = compactVoxelSpanArray[index2];
            compactVoxelSpanArray[index2].con = uint.MaxValue;
            for (int dir = 0; dir < 4; ++dir)
            {
              int num4 = index1 + this.voxelArea.DirectionX[dir];
              int num5 = num2 + this.voxelArea.DirectionZ[dir];
              if (num4 >= 0 && num5 >= 0 && (num5 < num1 && num4 < this.voxelArea.width))
              {
                CompactVoxelCell compactVoxelCell2 = compactVoxelCellArray[num4 + num5];
                int index4 = (int) compactVoxelCell2.index;
                for (int index5 = (int) compactVoxelCell2.index + (int) compactVoxelCell2.count; index4 < index5; ++index4)
                {
                  CompactVoxelSpan compactVoxelSpan2 = compactVoxelSpanArray[index4];
                  int num6 = (int) Math.Max(compactVoxelSpan1.y, compactVoxelSpan2.y);
                  if ((long) (AstarMath.Min((int) compactVoxelSpan1.y + (int) compactVoxelSpan1.h, (int) compactVoxelSpan2.y + (int) compactVoxelSpan2.h) - num6) >= (long) this.voxelWalkableHeight && Math.Abs((int) compactVoxelSpan2.y - (int) compactVoxelSpan1.y) <= this.voxelWalkableClimb)
                  {
                    uint num7 = (uint) index4 - compactVoxelCell2.index;
                    if (num7 > (uint) ushort.MaxValue)
                    {
                      Debug.LogError((object) "Too many layers");
                    }
                    else
                    {
                      compactVoxelSpanArray[index2].SetConnection(dir, num7);
                      break;
                    }
                  }
                }
              }
            }
          }
        }
        num2 += this.voxelArea.width;
        ++num3;
      }
    }

    public void DrawLine(int a, int b, int[] indices, int[] verts, Color col)
    {
      int index1 = (indices[a] & 268435455) * 4;
      int index2 = (indices[b] & 268435455) * 4;
      Debug.DrawLine(this.ConvertPosCorrZ(verts[index1], verts[index1 + 1], verts[index1 + 2]), this.ConvertPosCorrZ(verts[index2], verts[index2 + 1], verts[index2 + 2]), col);
    }

    public Vector3 ConvertPos(int x, int y, int z)
    {
      return Vector3.Scale(new Vector3((float) x + 0.5f, (float) y, (float) ((double) z / (double) this.voxelArea.width + 0.5)), this.cellScale) + this.voxelOffset;
    }

    public Vector3 ConvertPosCorrZ(int x, int y, int z)
    {
      return Vector3.Scale(new Vector3((float) x, (float) y, (float) z), this.cellScale) + this.voxelOffset;
    }

    public Vector3 ConvertPosWithoutOffset(int x, int y, int z)
    {
      return Vector3.Scale(new Vector3((float) x, (float) y, (float) z / (float) this.voxelArea.width), this.cellScale) + this.voxelOffset;
    }

    public Vector3 ConvertPosition(int x, int z, int i)
    {
      CompactVoxelSpan compactVoxelSpan = this.voxelArea.compactSpans[i];
      return new Vector3((float) x * this.cellSize, (float) compactVoxelSpan.y * this.cellHeight, (float) z / (float) this.voxelArea.width * this.cellSize) + this.voxelOffset;
    }

    public void ErodeWalkableArea(int radius)
    {
      ushort[] numArray = this.voxelArea.tmpUShortArr;
      if (numArray == null || numArray.Length < this.voxelArea.compactSpanCount)
        numArray = this.voxelArea.tmpUShortArr = new ushort[this.voxelArea.compactSpanCount];
      Memory.MemSet<ushort>(numArray, ushort.MaxValue, 2);
      int num = (int) this.CalculateDistanceField(numArray);
      for (int index = 0; index < numArray.Length; ++index)
      {
        if ((int) numArray[index] < radius * 2)
          this.voxelArea.areaTypes[index] = 0;
      }
    }

    public void BuildDistanceField()
    {
      ushort[] numArray = this.voxelArea.tmpUShortArr;
      if (numArray == null || numArray.Length < this.voxelArea.compactSpanCount)
        numArray = this.voxelArea.tmpUShortArr = new ushort[this.voxelArea.compactSpanCount];
      Memory.MemSet<ushort>(numArray, ushort.MaxValue, 2);
      this.voxelArea.maxDistance = this.CalculateDistanceField(numArray);
      ushort[] dst = this.voxelArea.dist;
      if (dst == null || dst.Length < this.voxelArea.compactSpanCount)
        dst = new ushort[this.voxelArea.compactSpanCount];
      this.voxelArea.dist = this.BoxBlur(numArray, dst);
    }

    [Obsolete("This function is not complete and should not be used")]
    public void ErodeVoxels(int radius)
    {
      if (radius > (int) byte.MaxValue)
      {
        Debug.LogError((object) "Max Erode Radius is 255");
        radius = (int) byte.MaxValue;
      }
      int num1 = this.voxelArea.width * this.voxelArea.depth;
      int[] numArray = new int[this.voxelArea.compactSpanCount];
      for (int index = 0; index < numArray.Length; ++index)
        numArray[index] = (int) byte.MaxValue;
      int num2 = 0;
      while (num2 < num1)
      {
        for (int index1 = 0; index1 < this.voxelArea.width; ++index1)
        {
          CompactVoxelCell compactVoxelCell = this.voxelArea.compactCells[index1 + num2];
          int index2 = (int) compactVoxelCell.index;
          for (int index3 = (int) compactVoxelCell.index + (int) compactVoxelCell.count; index2 < index3; ++index2)
          {
            if (this.voxelArea.areaTypes[index2] != 0)
            {
              CompactVoxelSpan compactVoxelSpan = this.voxelArea.compactSpans[index2];
              int num3 = 0;
              for (int dir = 0; dir < 4; ++dir)
              {
                if ((long) compactVoxelSpan.GetConnection(dir) != 63L)
                  ++num3;
              }
              if (num3 != 4)
                numArray[index2] = 0;
            }
          }
        }
        num2 += this.voxelArea.width;
      }
    }

    public void FilterLowHeightSpans(uint voxelWalkableHeight, float cs, float ch, Vector3 min)
    {
      int num1 = this.voxelArea.width * this.voxelArea.depth;
      LinkedVoxelSpan[] linkedVoxelSpanArray = this.voxelArea.linkedSpans;
      int num2 = 0;
      int num3 = 0;
      while (num2 < num1)
      {
        for (int index1 = 0; index1 < this.voxelArea.width; ++index1)
        {
          for (int index2 = num2 + index1; index2 != -1 && (int) linkedVoxelSpanArray[index2].bottom != -1; index2 = linkedVoxelSpanArray[index2].next)
          {
            uint num4 = linkedVoxelSpanArray[index2].top;
            if ((linkedVoxelSpanArray[index2].next == -1 ? 65536U : linkedVoxelSpanArray[linkedVoxelSpanArray[index2].next].bottom) - num4 < voxelWalkableHeight)
              linkedVoxelSpanArray[index2].area = 0;
          }
        }
        num2 += this.voxelArea.width;
        ++num3;
      }
    }

    public void FilterLedges(uint voxelWalkableHeight, int voxelWalkableClimb, float cs, float ch, Vector3 min)
    {
      int num1 = this.voxelArea.width * this.voxelArea.depth;
      LinkedVoxelSpan[] linkedVoxelSpanArray = this.voxelArea.linkedSpans;
      int[] numArray1 = this.voxelArea.DirectionX;
      int[] numArray2 = this.voxelArea.DirectionZ;
      int num2 = this.voxelArea.width;
      int num3 = 0;
      int num4 = 0;
      while (num3 < num1)
      {
        for (int index1 = 0; index1 < num2; ++index1)
        {
          if ((int) linkedVoxelSpanArray[index1 + num3].bottom != -1)
          {
            for (int index2 = index1 + num3; index2 != -1; index2 = linkedVoxelSpanArray[index2].next)
            {
              if (linkedVoxelSpanArray[index2].area != 0)
              {
                int val1_1 = (int) linkedVoxelSpanArray[index2].top;
                int val1_2 = linkedVoxelSpanArray[index2].next == -1 ? 65536 : (int) linkedVoxelSpanArray[linkedVoxelSpanArray[index2].next].bottom;
                int num5 = 65536;
                int num6 = (int) linkedVoxelSpanArray[index2].top;
                int num7 = num6;
                for (int index3 = 0; index3 < 4; ++index3)
                {
                  int num8 = index1 + numArray1[index3];
                  int num9 = num3 + numArray2[index3];
                  if (num8 < 0 || num9 < 0 || (num9 >= num1 || num8 >= num2))
                  {
                    linkedVoxelSpanArray[index2].area = 0;
                    break;
                  }
                  int index4 = num8 + num9;
                  int val2_1 = -voxelWalkableClimb;
                  int val2_2 = (int) linkedVoxelSpanArray[index4].bottom == -1 ? 65536 : (int) linkedVoxelSpanArray[index4].bottom;
                  if ((long) (Math.Min(val1_2, val2_2) - Math.Max(val1_1, val2_1)) > (long) voxelWalkableHeight)
                    num5 = Math.Min(num5, val2_1 - val1_1);
                  if ((int) linkedVoxelSpanArray[index4].bottom != -1)
                  {
                    for (int index5 = index4; index5 != -1; index5 = linkedVoxelSpanArray[index5].next)
                    {
                      int val2_3 = (int) linkedVoxelSpanArray[index5].top;
                      int val2_4 = linkedVoxelSpanArray[index5].next == -1 ? 65536 : (int) linkedVoxelSpanArray[linkedVoxelSpanArray[index5].next].bottom;
                      if ((long) (Math.Min(val1_2, val2_4) - Math.Max(val1_1, val2_3)) > (long) voxelWalkableHeight)
                      {
                        num5 = AstarMath.Min(num5, val2_3 - val1_1);
                        if (Math.Abs(val2_3 - val1_1) <= voxelWalkableClimb)
                        {
                          if (val2_3 < num6)
                            num6 = val2_3;
                          if (val2_3 > num7)
                            num7 = val2_3;
                        }
                      }
                    }
                  }
                }
                if (num5 < -voxelWalkableClimb || num7 - num6 > voxelWalkableClimb)
                  linkedVoxelSpanArray[index2].area = 0;
              }
            }
          }
        }
        num3 += num2;
        ++num4;
      }
    }

    public ushort[] ExpandRegions(int maxIterations, uint level, ushort[] srcReg, ushort[] srcDist, ushort[] dstReg, ushort[] dstDist, List<int> stack)
    {
      int num1 = this.voxelArea.width;
      int num2 = this.voxelArea.depth;
      int num3 = num1 * num2;
      stack.Clear();
      int num4 = 0;
      int num5 = 0;
      while (num4 < num3)
      {
        for (int index1 = 0; index1 < this.voxelArea.width; ++index1)
        {
          CompactVoxelCell compactVoxelCell = this.voxelArea.compactCells[num4 + index1];
          int index2 = (int) compactVoxelCell.index;
          for (int index3 = (int) compactVoxelCell.index + (int) compactVoxelCell.count; index2 < index3; ++index2)
          {
            if ((uint) this.voxelArea.dist[index2] >= level && (int) srcReg[index2] == 0 && this.voxelArea.areaTypes[index2] != 0)
            {
              stack.Add(index1);
              stack.Add(num4);
              stack.Add(index2);
            }
          }
        }
        num4 += num1;
        ++num5;
      }
      int num6 = 0;
      int count = stack.Count;
      if (count > 0)
      {
        do
        {
          do
          {
            int num7 = 0;
            Buffer.BlockCopy((Array) srcReg, 0, (Array) dstReg, 0, srcReg.Length * 2);
            Buffer.BlockCopy((Array) srcDist, 0, (Array) dstDist, 0, dstDist.Length * 2);
            int index1 = 0;
            while (index1 < count && index1 < count)
            {
              int num8 = stack[index1];
              int num9 = stack[index1 + 1];
              int index2 = stack[index1 + 2];
              if (index2 < 0)
              {
                ++num7;
              }
              else
              {
                ushort num10 = srcReg[index2];
                ushort num11 = ushort.MaxValue;
                CompactVoxelSpan compactVoxelSpan = this.voxelArea.compactSpans[index2];
                int num12 = this.voxelArea.areaTypes[index2];
                for (int dir = 0; dir < 4; ++dir)
                {
                  if ((long) compactVoxelSpan.GetConnection(dir) != 63L)
                  {
                    int index3 = (int) this.voxelArea.compactCells[num8 + this.voxelArea.DirectionX[dir] + (num9 + this.voxelArea.DirectionZ[dir])].index + compactVoxelSpan.GetConnection(dir);
                    if (num12 == this.voxelArea.areaTypes[index3] && (int) srcReg[index3] > 0 && (((int) srcReg[index3] & 32768) == 0 && (int) srcDist[index3] + 2 < (int) num11))
                    {
                      num10 = srcReg[index3];
                      num11 = (ushort) ((uint) srcDist[index3] + 2U);
                    }
                  }
                }
                if ((int) num10 != 0)
                {
                  stack[index1 + 2] = -1;
                  dstReg[index2] = num10;
                  dstDist[index2] = num11;
                }
                else
                  ++num7;
              }
              index1 += 3;
            }
            ushort[] numArray1 = srcReg;
            srcReg = dstReg;
            dstReg = numArray1;
            ushort[] numArray2 = srcDist;
            srcDist = dstDist;
            dstDist = numArray2;
            if (num7 * 3 >= count)
              goto label_29;
          }
          while (level <= 0U);
          ++num6;
        }
        while (num6 < maxIterations);
      }
label_29:
      return srcReg;
    }

    public bool FloodRegion(int x, int z, int i, uint level, ushort r, ushort[] srcReg, ushort[] srcDist, List<int> stack)
    {
      int num1 = this.voxelArea.areaTypes[i];
      stack.Clear();
      stack.Add(x);
      stack.Add(z);
      stack.Add(i);
      srcReg[i] = r;
      srcDist[i] = (ushort) 0;
      int num2 = level < 2U ? 0 : (int) level - 2;
      int num3 = 0;
      while (stack.Count > 0)
      {
        int index1 = stack[stack.Count - 1];
        stack.RemoveAt(stack.Count - 1);
        int num4 = stack[stack.Count - 1];
        stack.RemoveAt(stack.Count - 1);
        int num5 = stack[stack.Count - 1];
        stack.RemoveAt(stack.Count - 1);
        CompactVoxelSpan compactVoxelSpan1 = this.voxelArea.compactSpans[index1];
        ushort num6 = (ushort) 0;
        for (int dir1 = 0; dir1 < 4; ++dir1)
        {
          if ((long) compactVoxelSpan1.GetConnection(dir1) != 63L)
          {
            int num7 = num5 + this.voxelArea.DirectionX[dir1];
            int num8 = num4 + this.voxelArea.DirectionZ[dir1];
            int index2 = (int) this.voxelArea.compactCells[num7 + num8].index + compactVoxelSpan1.GetConnection(dir1);
            if (this.voxelArea.areaTypes[index2] == num1)
            {
              ushort num9 = srcReg[index2];
              if (((int) num9 & 32768) != 32768)
              {
                if ((int) num9 != 0 && (int) num9 != (int) r)
                  num6 = num9;
                CompactVoxelSpan compactVoxelSpan2 = this.voxelArea.compactSpans[index2];
                int dir2 = dir1 + 1 & 3;
                if ((long) compactVoxelSpan2.GetConnection(dir2) != 63L)
                {
                  int index3 = (int) this.voxelArea.compactCells[num7 + this.voxelArea.DirectionX[dir2] + (num8 + this.voxelArea.DirectionZ[dir2])].index + compactVoxelSpan2.GetConnection(dir2);
                  if (this.voxelArea.areaTypes[index3] == num1)
                  {
                    ushort num10 = srcReg[index3];
                    if ((int) num10 != 0 && (int) num10 != (int) r)
                      num6 = num10;
                  }
                }
              }
            }
          }
        }
        if ((int) num6 != 0)
        {
          srcReg[index1] = (ushort) 0;
        }
        else
        {
          ++num3;
          for (int dir = 0; dir < 4; ++dir)
          {
            if ((long) compactVoxelSpan1.GetConnection(dir) != 63L)
            {
              int num7 = num5 + this.voxelArea.DirectionX[dir];
              int num8 = num4 + this.voxelArea.DirectionZ[dir];
              int index2 = (int) this.voxelArea.compactCells[num7 + num8].index + compactVoxelSpan1.GetConnection(dir);
              if (this.voxelArea.areaTypes[index2] == num1 && (int) this.voxelArea.dist[index2] >= num2 && (int) srcReg[index2] == 0)
              {
                srcReg[index2] = r;
                srcDist[index2] = (ushort) 0;
                stack.Add(num7);
                stack.Add(num8);
                stack.Add(index2);
              }
            }
          }
        }
      }
      return num3 > 0;
    }

    public void MarkRectWithRegion(int minx, int maxx, int minz, int maxz, ushort region, ushort[] srcReg)
    {
      int num1 = maxz * this.voxelArea.width;
      int num2 = minz * this.voxelArea.width;
      while (num2 < num1)
      {
        for (int index1 = minx; index1 < maxx; ++index1)
        {
          CompactVoxelCell compactVoxelCell = this.voxelArea.compactCells[num2 + index1];
          int index2 = (int) compactVoxelCell.index;
          for (int index3 = (int) compactVoxelCell.index + (int) compactVoxelCell.count; index2 < index3; ++index2)
          {
            if (this.voxelArea.areaTypes[index2] != 0)
              srcReg[index2] = region;
          }
        }
        num2 += this.voxelArea.width;
      }
    }

    public ushort CalculateDistanceField(ushort[] src)
    {
      int num1 = this.voxelArea.width * this.voxelArea.depth;
      int num2 = 0;
      while (num2 < num1)
      {
        for (int index1 = 0; index1 < this.voxelArea.width; ++index1)
        {
          CompactVoxelCell compactVoxelCell = this.voxelArea.compactCells[index1 + num2];
          int index2 = (int) compactVoxelCell.index;
          for (int index3 = (int) compactVoxelCell.index + (int) compactVoxelCell.count; index2 < index3; ++index2)
          {
            CompactVoxelSpan compactVoxelSpan = this.voxelArea.compactSpans[index2];
            int num3 = 0;
            for (int dir = 0; dir < 4 && (long) compactVoxelSpan.GetConnection(dir) != 63L; ++dir)
              ++num3;
            if (num3 != 4)
              src[index2] = (ushort) 0;
          }
        }
        num2 += this.voxelArea.width;
      }
      int num4 = 0;
      while (num4 < num1)
      {
        for (int index1 = 0; index1 < this.voxelArea.width; ++index1)
        {
          CompactVoxelCell compactVoxelCell = this.voxelArea.compactCells[index1 + num4];
          int index2 = (int) compactVoxelCell.index;
          for (int index3 = (int) compactVoxelCell.index + (int) compactVoxelCell.count; index2 < index3; ++index2)
          {
            CompactVoxelSpan compactVoxelSpan1 = this.voxelArea.compactSpans[index2];
            if ((long) compactVoxelSpan1.GetConnection(0) != 63L)
            {
              int num3 = index1 + this.voxelArea.DirectionX[0];
              int num5 = num4 + this.voxelArea.DirectionZ[0];
              int index4 = (int) ((long) this.voxelArea.compactCells[num3 + num5].index + (long) compactVoxelSpan1.GetConnection(0));
              if ((int) src[index4] + 2 < (int) src[index2])
                src[index2] = (ushort) ((uint) src[index4] + 2U);
              CompactVoxelSpan compactVoxelSpan2 = this.voxelArea.compactSpans[index4];
              if ((long) compactVoxelSpan2.GetConnection(3) != 63L)
              {
                int index5 = (int) ((long) this.voxelArea.compactCells[num3 + this.voxelArea.DirectionX[3] + (num5 + this.voxelArea.DirectionZ[3])].index + (long) compactVoxelSpan2.GetConnection(3));
                if ((int) src[index5] + 3 < (int) src[index2])
                  src[index2] = (ushort) ((uint) src[index5] + 3U);
              }
            }
            if ((long) compactVoxelSpan1.GetConnection(3) != 63L)
            {
              int num3 = index1 + this.voxelArea.DirectionX[3];
              int num5 = num4 + this.voxelArea.DirectionZ[3];
              int index4 = (int) ((long) this.voxelArea.compactCells[num3 + num5].index + (long) compactVoxelSpan1.GetConnection(3));
              if ((int) src[index4] + 2 < (int) src[index2])
                src[index2] = (ushort) ((uint) src[index4] + 2U);
              CompactVoxelSpan compactVoxelSpan2 = this.voxelArea.compactSpans[index4];
              if ((long) compactVoxelSpan2.GetConnection(2) != 63L)
              {
                int index5 = (int) ((long) this.voxelArea.compactCells[num3 + this.voxelArea.DirectionX[2] + (num5 + this.voxelArea.DirectionZ[2])].index + (long) compactVoxelSpan2.GetConnection(2));
                if ((int) src[index5] + 3 < (int) src[index2])
                  src[index2] = (ushort) ((uint) src[index5] + 3U);
              }
            }
          }
        }
        num4 += this.voxelArea.width;
      }
      int num6 = num1 - this.voxelArea.width;
      while (num6 >= 0)
      {
        for (int index1 = this.voxelArea.width - 1; index1 >= 0; --index1)
        {
          CompactVoxelCell compactVoxelCell = this.voxelArea.compactCells[index1 + num6];
          int index2 = (int) compactVoxelCell.index;
          for (int index3 = (int) compactVoxelCell.index + (int) compactVoxelCell.count; index2 < index3; ++index2)
          {
            CompactVoxelSpan compactVoxelSpan1 = this.voxelArea.compactSpans[index2];
            if ((long) compactVoxelSpan1.GetConnection(2) != 63L)
            {
              int num3 = index1 + this.voxelArea.DirectionX[2];
              int num5 = num6 + this.voxelArea.DirectionZ[2];
              int index4 = (int) ((long) this.voxelArea.compactCells[num3 + num5].index + (long) compactVoxelSpan1.GetConnection(2));
              if ((int) src[index4] + 2 < (int) src[index2])
                src[index2] = (ushort) ((uint) src[index4] + 2U);
              CompactVoxelSpan compactVoxelSpan2 = this.voxelArea.compactSpans[index4];
              if ((long) compactVoxelSpan2.GetConnection(1) != 63L)
              {
                int index5 = (int) ((long) this.voxelArea.compactCells[num3 + this.voxelArea.DirectionX[1] + (num5 + this.voxelArea.DirectionZ[1])].index + (long) compactVoxelSpan2.GetConnection(1));
                if ((int) src[index5] + 3 < (int) src[index2])
                  src[index2] = (ushort) ((uint) src[index5] + 3U);
              }
            }
            if ((long) compactVoxelSpan1.GetConnection(1) != 63L)
            {
              int num3 = index1 + this.voxelArea.DirectionX[1];
              int num5 = num6 + this.voxelArea.DirectionZ[1];
              int index4 = (int) ((long) this.voxelArea.compactCells[num3 + num5].index + (long) compactVoxelSpan1.GetConnection(1));
              if ((int) src[index4] + 2 < (int) src[index2])
                src[index2] = (ushort) ((uint) src[index4] + 2U);
              CompactVoxelSpan compactVoxelSpan2 = this.voxelArea.compactSpans[index4];
              if ((long) compactVoxelSpan2.GetConnection(0) != 63L)
              {
                int index5 = (int) ((long) this.voxelArea.compactCells[num3 + this.voxelArea.DirectionX[0] + (num5 + this.voxelArea.DirectionZ[0])].index + (long) compactVoxelSpan2.GetConnection(0));
                if ((int) src[index5] + 3 < (int) src[index2])
                  src[index2] = (ushort) ((uint) src[index5] + 3U);
              }
            }
          }
        }
        num6 -= this.voxelArea.width;
      }
      ushort val2 = (ushort) 0;
      for (int index = 0; index < this.voxelArea.compactSpanCount; ++index)
        val2 = Math.Max(src[index], val2);
      return val2;
    }

    public ushort[] BoxBlur(ushort[] src, ushort[] dst)
    {
      ushort num1 = (ushort) 20;
      int num2 = this.voxelArea.width * this.voxelArea.depth - this.voxelArea.width;
      while (num2 >= 0)
      {
        for (int index1 = this.voxelArea.width - 1; index1 >= 0; --index1)
        {
          CompactVoxelCell compactVoxelCell = this.voxelArea.compactCells[index1 + num2];
          int index2 = (int) compactVoxelCell.index;
          for (int index3 = (int) compactVoxelCell.index + (int) compactVoxelCell.count; index2 < index3; ++index2)
          {
            CompactVoxelSpan compactVoxelSpan1 = this.voxelArea.compactSpans[index2];
            ushort num3 = src[index2];
            if ((int) num3 < (int) num1)
            {
              dst[index2] = num3;
            }
            else
            {
              int num4 = (int) num3;
              for (int dir1 = 0; dir1 < 4; ++dir1)
              {
                if ((long) compactVoxelSpan1.GetConnection(dir1) != 63L)
                {
                  int num5 = index1 + this.voxelArea.DirectionX[dir1];
                  int num6 = num2 + this.voxelArea.DirectionZ[dir1];
                  int index4 = (int) ((long) this.voxelArea.compactCells[num5 + num6].index + (long) compactVoxelSpan1.GetConnection(dir1));
                  int num7 = num4 + (int) src[index4];
                  CompactVoxelSpan compactVoxelSpan2 = this.voxelArea.compactSpans[index4];
                  int dir2 = dir1 + 1 & 3;
                  if ((long) compactVoxelSpan2.GetConnection(dir2) != 63L)
                  {
                    int index5 = (int) ((long) this.voxelArea.compactCells[num5 + this.voxelArea.DirectionX[dir2] + (num6 + this.voxelArea.DirectionZ[dir2])].index + (long) compactVoxelSpan2.GetConnection(dir2));
                    num4 = num7 + (int) src[index5];
                  }
                  else
                    num4 = num7 + (int) num3;
                }
                else
                  num4 += (int) num3 * 2;
              }
              dst[index2] = (ushort) ((double) (num4 + 5) / 9.0);
            }
          }
        }
        num2 -= this.voxelArea.width;
      }
      return dst;
    }

    private void FloodOnes(List<Int3> st1, ushort[] regs, uint level, ushort reg)
    {
      for (int index1 = 0; index1 < st1.Count; ++index1)
      {
        int num1 = st1[index1].x;
        int index2 = st1[index1].y;
        int num2 = st1[index1].z;
        regs[index2] = reg;
        CompactVoxelSpan compactVoxelSpan = this.voxelArea.compactSpans[index2];
        int num3 = this.voxelArea.areaTypes[index2];
        for (int dir = 0; dir < 4; ++dir)
        {
          if ((long) compactVoxelSpan.GetConnection(dir) != 63L)
          {
            int _x = num1 + this.voxelArea.DirectionX[dir];
            int _z = num2 + this.voxelArea.DirectionZ[dir];
            int _y = (int) this.voxelArea.compactCells[_x + _z].index + compactVoxelSpan.GetConnection(dir);
            if (num3 == this.voxelArea.areaTypes[_y] && (int) regs[_y] == 1)
            {
              regs[_y] = reg;
              st1.Add(new Int3(_x, _y, _z));
            }
          }
        }
      }
    }

    public void BuildRegions()
    {
      int maxx = this.voxelArea.width;
      int maxz = this.voxelArea.depth;
      int num1 = maxx * maxz;
      int maxIterations = 8;
      int length = this.voxelArea.compactSpanCount;
      List<int> list = ListPool<int>.Claim(1024);
      ushort[] numArray1 = new ushort[length];
      ushort[] srcDist = new ushort[length];
      ushort[] dstReg = new ushort[length];
      ushort[] dstDist = new ushort[length];
      ushort num2 = (ushort) 2;
      this.MarkRectWithRegion(0, this.borderSize, 0, maxz, (ushort) ((uint) num2 | 32768U), numArray1);
      ushort num3 = (ushort) ((uint) num2 + 1U);
      this.MarkRectWithRegion(maxx - this.borderSize, maxx, 0, maxz, (ushort) ((uint) num3 | 32768U), numArray1);
      ushort num4 = (ushort) ((uint) num3 + 1U);
      this.MarkRectWithRegion(0, maxx, 0, this.borderSize, (ushort) ((uint) num4 | 32768U), numArray1);
      ushort num5 = (ushort) ((uint) num4 + 1U);
      this.MarkRectWithRegion(0, maxx, maxz - this.borderSize, maxz, (ushort) ((uint) num5 | 32768U), numArray1);
      ushort r = (ushort) ((uint) num5 + 1U);
      uint level = (uint) ((int) this.voxelArea.maxDistance + 1 & -2);
      int num6 = 0;
      while (level > 0U)
      {
        level = level < 2U ? 0U : level - 2U;
        if (this.ExpandRegions(maxIterations, level, numArray1, srcDist, dstReg, dstDist, list) != numArray1)
        {
          ushort[] numArray2 = numArray1;
          numArray1 = dstReg;
          dstReg = numArray2;
          ushort[] numArray3 = srcDist;
          srcDist = dstDist;
          dstDist = numArray3;
        }
        int z = 0;
        int num7 = 0;
        while (z < num1)
        {
          for (int x = 0; x < this.voxelArea.width; ++x)
          {
            CompactVoxelCell compactVoxelCell = this.voxelArea.compactCells[z + x];
            int i = (int) compactVoxelCell.index;
            for (int index = (int) compactVoxelCell.index + (int) compactVoxelCell.count; i < index; ++i)
            {
              if ((uint) this.voxelArea.dist[i] >= level && (int) numArray1[i] == 0 && (this.voxelArea.areaTypes[i] != 0 && this.FloodRegion(x, z, i, level, r, numArray1, srcDist, list)))
                ++r;
            }
          }
          z += maxx;
          ++num7;
        }
        ++num6;
      }
      if (this.ExpandRegions(maxIterations * 8, 0U, numArray1, srcDist, dstReg, dstDist, list) != numArray1)
        numArray1 = dstReg;
      this.voxelArea.maxRegions = (int) r;
      this.FilterSmallRegions(numArray1, this.minRegionSize, this.voxelArea.maxRegions);
      for (int index = 0; index < this.voxelArea.compactSpanCount; ++index)
        this.voxelArea.compactSpans[index].reg = (int) numArray1[index];
      ListPool<int>.Release(list);
    }

    private static int union_find_find(int[] arr, int x)
    {
      if (arr[x] < 0)
        return x;
      return arr[x] = Voxelize.union_find_find(arr, arr[x]);
    }

    private static void union_find_union(int[] arr, int a, int b)
    {
      a = Voxelize.union_find_find(arr, a);
      b = Voxelize.union_find_find(arr, b);
      if (a == b)
        return;
      if (arr[a] > arr[b])
      {
        int num = a;
        a = b;
        b = num;
      }
      arr[a] += arr[b];
      arr[b] = a;
    }

    public void FilterSmallRegions(ushort[] reg, int minRegionSize, int maxRegions)
    {
      RelevantGraphSurface relevantGraphSurface = RelevantGraphSurface.Root;
      bool flag = !object.ReferenceEquals((object) relevantGraphSurface, (object) null) && this.relevantGraphSurfaceMode != RecastGraph.RelevantGraphSurfaceMode.DoNotRequire;
      if (!flag && minRegionSize <= 0)
        return;
      int[] numArray = new int[maxRegions];
      ushort[] array = this.voxelArea.tmpUShortArr;
      if (array == null || array.Length < maxRegions)
        array = this.voxelArea.tmpUShortArr = new ushort[maxRegions];
      Memory.MemSet<int>(numArray, -1, 4);
      Memory.MemSet<ushort>(array, (ushort) 0, maxRegions, 2);
      int length = numArray.Length;
      int num1 = this.voxelArea.width * this.voxelArea.depth;
      int num2 = 2 | (this.relevantGraphSurfaceMode != RecastGraph.RelevantGraphSurfaceMode.OnlyForCompletelyInsideTile ? 0 : 1);
      if (flag)
      {
        while (!object.ReferenceEquals((object) relevantGraphSurface, (object) null))
        {
          int x;
          int z;
          this.VectorToIndex(relevantGraphSurface.Position, out x, out z);
          if (x < 0 || z < 0 || (x >= this.voxelArea.width || z >= this.voxelArea.depth))
          {
            relevantGraphSurface = relevantGraphSurface.Next;
          }
          else
          {
            int num3 = (int) (((double) relevantGraphSurface.Position.y - (double) this.voxelOffset.y) / (double) this.cellHeight);
            int num4 = (int) ((double) relevantGraphSurface.maxRange / (double) this.cellHeight);
            CompactVoxelCell compactVoxelCell = this.voxelArea.compactCells[x + z * this.voxelArea.width];
            for (int index = (int) compactVoxelCell.index; (long) index < (long) (compactVoxelCell.index + compactVoxelCell.count); ++index)
            {
              if (Math.Abs((int) this.voxelArea.compactSpans[index].y - num3) <= num4 && (int) reg[index] != 0)
                array[Voxelize.union_find_find(numArray, (int) reg[index] & -32769)] |= (ushort) 2;
            }
            relevantGraphSurface = relevantGraphSurface.Next;
          }
        }
      }
      int num5 = 0;
      int num6 = 0;
      while (num5 < num1)
      {
        for (int index1 = 0; index1 < this.voxelArea.width; ++index1)
        {
          CompactVoxelCell compactVoxelCell = this.voxelArea.compactCells[index1 + num5];
          for (int index2 = (int) compactVoxelCell.index; (long) index2 < (long) (compactVoxelCell.index + compactVoxelCell.count); ++index2)
          {
            CompactVoxelSpan compactVoxelSpan = this.voxelArea.compactSpans[index2];
            int x = (int) reg[index2];
            if ((x & -32769) != 0)
            {
              if (x >= length)
              {
                array[Voxelize.union_find_find(numArray, x & -32769)] |= (ushort) 1;
              }
              else
              {
                int find = Voxelize.union_find_find(numArray, x);
                --numArray[find];
                for (int dir = 0; dir < 4; ++dir)
                {
                  if ((long) compactVoxelSpan.GetConnection(dir) != 63L)
                  {
                    int index3 = (int) this.voxelArea.compactCells[index1 + this.voxelArea.DirectionX[dir] + (num5 + this.voxelArea.DirectionZ[dir])].index + compactVoxelSpan.GetConnection(dir);
                    int b = (int) reg[index3];
                    if (x != b && (b & -32769) != 0)
                    {
                      if ((b & 32768) != 0)
                        array[find] |= (ushort) 1;
                      else
                        Voxelize.union_find_union(numArray, find, b);
                    }
                  }
                }
              }
            }
          }
        }
        num5 += this.voxelArea.width;
        ++num6;
      }
      for (int x = 0; x < numArray.Length; ++x)
        array[Voxelize.union_find_find(numArray, x)] |= array[x];
      for (int x = 0; x < numArray.Length; ++x)
      {
        int find = Voxelize.union_find_find(numArray, x);
        if (((int) array[find] & 1) != 0)
          numArray[find] = -minRegionSize - 2;
        if (flag && ((int) array[find] & num2) == 0)
          numArray[find] = -1;
      }
      for (int index = 0; index < this.voxelArea.compactSpanCount; ++index)
      {
        int x = (int) reg[index];
        if (x < length && numArray[Voxelize.union_find_find(numArray, x)] >= -minRegionSize - 1)
          reg[index] = (ushort) 0;
      }
    }
  }
}
