// Decompiled with JetBrains decompiler
// Type: Pathfinding.Util.TileHandler
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding;
using Pathfinding.ClipperLib;
using Pathfinding.Poly2Tri;
using Pathfinding.Voxels;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.Util
{
  public class TileHandler
  {
    private List<TileHandler.TileType> tileTypes = new List<TileHandler.TileType>();
    private int[] cached_int_array = new int[32];
    private Dictionary<Int3, int> cached_Int3_int_dict = new Dictionary<Int3, int>();
    private Dictionary<Int2, int> cached_Int2_int_dict = new Dictionary<Int2, int>();
    private const int CUT_ALL = 0;
    private const int CUT_DUAL = 1;
    private const int CUT_BREAK = 2;
    private RecastGraph _graph;
    private Clipper clipper;
    private TileHandler.TileType[] activeTileTypes;
    private int[] activeTileRotations;
    private int[] activeTileOffsets;
    private bool[] reloadedInBatch;
    private bool isBatching;

    public RecastGraph graph
    {
      get
      {
        return this._graph;
      }
    }

    public TileHandler(RecastGraph graph)
    {
      if (graph == null)
        throw new ArgumentNullException("'graph' cannot be null");
      if (graph.GetTiles() == null)
        throw new ArgumentException("graph has no tiles. Please scan the graph before creating a TileHandler");
      this.activeTileTypes = new TileHandler.TileType[graph.tileXCount * graph.tileZCount];
      this.activeTileRotations = new int[this.activeTileTypes.Length];
      this.activeTileOffsets = new int[this.activeTileTypes.Length];
      this.reloadedInBatch = new bool[this.activeTileTypes.Length];
      this._graph = graph;
    }

    public int GetActiveRotation(Int2 p)
    {
      return this.activeTileRotations[p.x + p.y * this._graph.tileXCount];
    }

    public TileHandler.TileType GetTileType(int index)
    {
      return this.tileTypes[index];
    }

    public int GetTileTypeCount()
    {
      return this.tileTypes.Count;
    }

    public TileHandler.TileType RegisterTileType(Mesh source, Int3 centerOffset, int width = 1, int depth = 1)
    {
      TileHandler.TileType tileType = new TileHandler.TileType(source, new Int3(this.graph.tileSizeX, 1, this.graph.tileSizeZ) * (1000f * this.graph.cellSize), centerOffset, width, depth);
      this.tileTypes.Add(tileType);
      return tileType;
    }

    public void CreateTileTypesFromGraph()
    {
      RecastGraph.NavmeshTile[] tiles = this.graph.GetTiles();
      if (tiles == null || tiles.Length != this.graph.tileXCount * this.graph.tileZCount)
        throw new InvalidOperationException("Graph tiles are invalid (either null or number of tiles is not equal to width*depth of the graph");
      for (int z = 0; z < this.graph.tileZCount; ++z)
      {
        for (int x = 0; x < this.graph.tileXCount; ++x)
        {
          RecastGraph.NavmeshTile navmeshTile = tiles[x + z * this.graph.tileXCount];
          Int3 int3 = (Int3) this.graph.GetTileBounds(x, z).min;
          Int3 tileSize = new Int3(this.graph.tileSizeX, 1, this.graph.tileSizeZ) * (1000f * this.graph.cellSize);
          Int3 centerOffset = -(int3 + new Int3(tileSize.x * navmeshTile.w / 2, 0, tileSize.z * navmeshTile.d / 2));
          TileHandler.TileType tileType = new TileHandler.TileType(navmeshTile.verts, navmeshTile.tris, tileSize, centerOffset, navmeshTile.w, navmeshTile.d);
          this.tileTypes.Add(tileType);
          int index = x + z * this.graph.tileXCount;
          this.activeTileTypes[index] = tileType;
          this.activeTileRotations[index] = 0;
          this.activeTileOffsets[index] = 0;
        }
      }
    }

    public bool StartBatchLoad()
    {
      if (this.isBatching)
        return false;
      this.isBatching = true;
      AstarPath.active.AddWorkItem(new AstarPath.AstarWorkItem((Func<bool, bool>) (force =>
      {
        this.graph.StartBatchTileUpdate();
        return true;
      })));
      return true;
    }

    public void EndBatchLoad()
    {
      if (!this.isBatching)
        throw new Exception("Ending batching when batching has not been started");
      for (int index = 0; index < this.reloadedInBatch.Length; ++index)
        this.reloadedInBatch[index] = false;
      this.isBatching = false;
      AstarPath.active.AddWorkItem(new AstarPath.AstarWorkItem((Func<bool, bool>) (force =>
      {
        this.graph.EndBatchTileUpdate();
        return true;
      })));
    }

    private void CutPoly(Int3[] verts, int[] tris, ref Int3[] outVertsArr, ref int[] outTrisArr, out int outVCount, out int outTCount, Int3[] extraShape, Int3 cuttingOffset, Bounds realBounds, TileHandler.CutMode mode = (TileHandler.CutMode) 3, int perturbate = 0)
    {
      if (verts.Length == 0 || tris.Length == 0)
      {
        outVCount = 0;
        outTCount = 0;
        outTrisArr = new int[0];
        outVertsArr = new Int3[0];
      }
      else
      {
        List<IntPoint> pg1 = (List<IntPoint>) null;
        if (extraShape == null && (mode & TileHandler.CutMode.CutExtra) != (TileHandler.CutMode) 0)
          throw new Exception("extraShape is null and the CutMode specifies that it should be used. Cannot use null shape.");
        if ((mode & TileHandler.CutMode.CutExtra) != (TileHandler.CutMode) 0)
        {
          pg1 = new List<IntPoint>(extraShape.Length);
          for (int index = 0; index < extraShape.Length; ++index)
            pg1.Add(new IntPoint((long) (extraShape[index].x + cuttingOffset.x), (long) (extraShape[index].z + cuttingOffset.z)));
        }
        List<IntPoint> pg2 = new List<IntPoint>(5);
        Dictionary<TriangulationPoint, int> dictionary1 = new Dictionary<TriangulationPoint, int>();
        List<PolygonPoint> list1 = new List<PolygonPoint>();
        Pathfinding.IntRect b = new Pathfinding.IntRect(verts[0].x, verts[0].z, verts[0].x, verts[0].z);
        for (int index = 0; index < verts.Length; ++index)
          b = b.ExpandToContain(verts[index].x, verts[index].z);
        List<Int3> list2 = ListPool<Int3>.Claim(verts.Length * 2);
        List<int> list3 = ListPool<int>.Claim(tris.Length);
        PolyTree polytree = new PolyTree();
        List<List<IntPoint>> solution = new List<List<IntPoint>>();
        Stack<Pathfinding.Poly2Tri.Polygon> stack = new Stack<Pathfinding.Poly2Tri.Polygon>();
        if (this.clipper == null)
          this.clipper = new Clipper(0);
        this.clipper.ReverseSolution = true;
        List<NavmeshCut> list4 = mode != TileHandler.CutMode.CutExtra ? NavmeshCut.GetAllInRange(realBounds) : ListPool<NavmeshCut>.Claim();
        List<int> list5 = ListPool<int>.Claim();
        List<Pathfinding.IntRect> list6 = ListPool<Pathfinding.IntRect>.Claim();
        List<Int2> list7 = ListPool<Int2>.Claim();
        List<List<IntPoint>> buffer = new List<List<IntPoint>>();
        List<bool> list8 = ListPool<bool>.Claim();
        List<bool> list9 = ListPool<bool>.Claim();
        if (perturbate > 10)
        {
          Debug.LogError((object) ("Too many perturbations aborting : " + (object) mode));
          Debug.Break();
          outVCount = verts.Length;
          outTCount = tris.Length;
          outTrisArr = tris;
          outVertsArr = verts;
        }
        else
        {
          System.Random random = (System.Random) null;
          if (perturbate > 0)
            random = new System.Random();
          for (int index1 = 0; index1 < list4.Count; ++index1)
          {
            Bounds bounds = list4[index1].GetBounds();
            Int3 int3_1 = (Int3) bounds.min + cuttingOffset;
            Int3 int3_2 = (Int3) bounds.max + cuttingOffset;
            if (Pathfinding.IntRect.Intersects(new Pathfinding.IntRect(int3_1.x, int3_1.z, int3_2.x, int3_2.z), b))
            {
              Int2 int2 = new Int2(0, 0);
              if (perturbate > 0)
              {
                int2.x = random.Next() % 6 * perturbate - 3 * perturbate;
                if (int2.x >= 0)
                  ++int2.x;
                int2.y = random.Next() % 6 * perturbate - 3 * perturbate;
                if (int2.y >= 0)
                  ++int2.y;
              }
              int count = buffer.Count;
              list4[index1].GetContour(buffer);
              for (int index2 = count; index2 < buffer.Count; ++index2)
              {
                List<IntPoint> list10 = buffer[index2];
                if (list10.Count == 0)
                {
                  Debug.LogError((object) "Zero Length Contour");
                  list6.Add(new Pathfinding.IntRect());
                  list7.Add(new Int2(0, 0));
                }
                else
                {
                  Pathfinding.IntRect intRect = new Pathfinding.IntRect((int) list10[0].X + cuttingOffset.x, (int) list10[0].Y + cuttingOffset.y, (int) list10[0].X + cuttingOffset.x, (int) list10[0].Y + cuttingOffset.y);
                  for (int index3 = 0; index3 < list10.Count; ++index3)
                  {
                    IntPoint intPoint = list10[index3];
                    intPoint.X += (long) cuttingOffset.x;
                    intPoint.Y += (long) cuttingOffset.z;
                    if (perturbate > 0)
                    {
                      intPoint.X += (long) int2.x;
                      intPoint.Y += (long) int2.y;
                    }
                    list10[index3] = intPoint;
                    intRect = intRect.ExpandToContain((int) intPoint.X, (int) intPoint.Y);
                  }
                  list7.Add(new Int2(int3_1.y, int3_2.y));
                  list6.Add(intRect);
                  list8.Add(list4[index1].isDual);
                  list9.Add(list4[index1].cutsAddedGeom);
                }
              }
            }
          }
          List<NavmeshAdd> allInRange = NavmeshAdd.GetAllInRange(realBounds);
          Int3[] vbuffer = verts;
          int[] tbuffer = tris;
          int index4 = -1;
          int index5 = -3;
          Int3[] int3Array1 = (Int3[]) null;
          Int3[] int3Array2 = (Int3[]) null;
          Int3 int3_3 = Int3.zero;
          if (allInRange.Count > 0)
          {
            int3Array1 = new Int3[7];
            int3Array2 = new Int3[7];
            int3_3 = (Int3) realBounds.extents;
          }
label_39:
          Int3 int3_4;
          Int3 int3_5;
          Int3 int3_6;
          bool flag;
          int num1;
          do
          {
            int n1;
            do
            {
              int n2;
              do
              {
                int n3;
                do
                {
                  index5 += 3;
                  while (index5 >= tbuffer.Length)
                  {
                    ++index4;
                    index5 = 0;
                    if (index4 >= allInRange.Count)
                    {
                      vbuffer = (Int3[]) null;
                      break;
                    }
                    if (vbuffer == verts)
                      vbuffer = (Int3[]) null;
                    allInRange[index4].GetMesh(cuttingOffset, ref vbuffer, out tbuffer);
                  }
                  if (vbuffer != null)
                  {
                    int3_4 = vbuffer[tbuffer[index5]];
                    int3_5 = vbuffer[tbuffer[index5 + 1]];
                    int3_6 = vbuffer[tbuffer[index5 + 2]];
                    Pathfinding.IntRect a = new Pathfinding.IntRect(int3_4.x, int3_4.z, int3_4.x, int3_4.z);
                    a = a.ExpandToContain(int3_5.x, int3_5.z);
                    a = a.ExpandToContain(int3_6.x, int3_6.z);
                    int num2 = Math.Min(int3_4.y, Math.Min(int3_5.y, int3_6.y));
                    int num3 = Math.Max(int3_4.y, Math.Max(int3_5.y, int3_6.y));
                    list5.Clear();
                    flag = false;
                    for (int index1 = 0; index1 < buffer.Count; ++index1)
                    {
                      int num4 = list7[index1].x;
                      int num5 = list7[index1].y;
                      if (Pathfinding.IntRect.Intersects(a, list6[index1]) && num5 >= num2 && num4 <= num3 && (list9[index1] || index4 == -1))
                      {
                        int3_4.y = num4;
                        int3_4.y = num5;
                        list5.Add(index1);
                        flag |= list8[index1];
                      }
                    }
                    if (list5.Count == 0 && (mode & TileHandler.CutMode.CutExtra) == (TileHandler.CutMode) 0 && ((mode & TileHandler.CutMode.CutAll) != (TileHandler.CutMode) 0 && index4 == -1))
                    {
                      list3.Add(list2.Count);
                      list3.Add(list2.Count + 1);
                      list3.Add(list2.Count + 2);
                      list2.Add(int3_4);
                      list2.Add(int3_5);
                      list2.Add(int3_6);
                    }
                    else
                    {
                      pg2.Clear();
                      if (index4 == -1)
                      {
                        pg2.Add(new IntPoint((long) int3_4.x, (long) int3_4.z));
                        pg2.Add(new IntPoint((long) int3_5.x, (long) int3_5.z));
                        pg2.Add(new IntPoint((long) int3_6.x, (long) int3_6.z));
                        goto label_63;
                      }
                      else
                      {
                        int3Array1[0] = int3_4;
                        int3Array1[1] = int3_5;
                        int3Array1[2] = int3_6;
                        n3 = Utility.ClipPolygon(int3Array1, 3, int3Array2, 1, 0, 0);
                      }
                    }
                  }
                  else
                    goto label_129;
                }
                while (n3 == 0);
                n2 = Utility.ClipPolygon(int3Array2, n3, int3Array1, -1, 2 * int3_3.x, 0);
              }
              while (n2 == 0);
              n1 = Utility.ClipPolygon(int3Array1, n2, int3Array2, 1, 0, 2);
            }
            while (n1 == 0);
            num1 = Utility.ClipPolygon(int3Array2, n1, int3Array1, -1, 2 * int3_3.z, 2);
          }
          while (num1 == 0);
          for (int index1 = 0; index1 < num1; ++index1)
            pg2.Add(new IntPoint((long) int3Array1[index1].x, (long) int3Array1[index1].z));
label_63:
          dictionary1.Clear();
          Int3 int3_7 = int3_5 - int3_4;
          Int3 int3_8 = int3_6 - int3_4;
          Int3 int3_9 = int3_7;
          Int3 int3_10 = int3_8;
          int3_9.y = 0;
          int3_10.y = 0;
          for (int index1 = 0; index1 < 16; ++index1)
          {
            if (((int) mode >> index1 & 1) != 0)
            {
              if (1 << index1 == 1)
              {
                this.clipper.Clear();
                this.clipper.AddPolygon(pg2, PolyType.ptSubject);
                for (int index2 = 0; index2 < list5.Count; ++index2)
                  this.clipper.AddPolygon(buffer[list5[index2]], PolyType.ptClip);
                polytree.Clear();
                this.clipper.Execute(ClipType.ctDifference, polytree, PolyFillType.pftEvenOdd, PolyFillType.pftNonZero);
              }
              else if (1 << index1 == 2)
              {
                if (flag)
                {
                  this.clipper.Clear();
                  this.clipper.AddPolygon(pg2, PolyType.ptSubject);
                  for (int index2 = 0; index2 < list5.Count; ++index2)
                  {
                    if (list8[list5[index2]])
                      this.clipper.AddPolygon(buffer[list5[index2]], PolyType.ptClip);
                  }
                  solution.Clear();
                  this.clipper.Execute(ClipType.ctIntersection, solution, PolyFillType.pftEvenOdd, PolyFillType.pftNonZero);
                  this.clipper.Clear();
                  for (int index2 = 0; index2 < solution.Count; ++index2)
                    this.clipper.AddPolygon(solution[index2], !Clipper.Orientation(solution[index2]) ? PolyType.ptSubject : PolyType.ptClip);
                  for (int index2 = 0; index2 < list5.Count; ++index2)
                  {
                    if (!list8[list5[index2]])
                      this.clipper.AddPolygon(buffer[list5[index2]], PolyType.ptClip);
                  }
                  polytree.Clear();
                  this.clipper.Execute(ClipType.ctDifference, polytree, PolyFillType.pftEvenOdd, PolyFillType.pftNonZero);
                }
                else
                  continue;
              }
              else if (1 << index1 == 4)
              {
                this.clipper.Clear();
                this.clipper.AddPolygon(pg2, PolyType.ptSubject);
                this.clipper.AddPolygon(pg1, PolyType.ptClip);
                polytree.Clear();
                this.clipper.Execute(ClipType.ctIntersection, polytree, PolyFillType.pftEvenOdd, PolyFillType.pftNonZero);
              }
              for (int index2 = 0; index2 < polytree.ChildCount; ++index2)
              {
                PolyNode polyNode = polytree.Childs[index2];
                List<IntPoint> contour = polyNode.Contour;
                List<PolyNode> childs = polyNode.Childs;
                if (childs.Count == 0 && contour.Count == 3 && index4 == -1)
                {
                  for (int index3 = 0; index3 < contour.Count; ++index3)
                  {
                    Int3 int3_1 = new Int3((int) contour[index3].X, 0, (int) contour[index3].Y);
                    double num2 = (double) (int3_5.z - int3_6.z) * (double) (int3_4.x - int3_6.x) + (double) (int3_6.x - int3_5.x) * (double) (int3_4.z - int3_6.z);
                    if (num2 == 0.0)
                    {
                      Debug.LogWarning((object) "Degenerate triangle");
                    }
                    else
                    {
                      double num3 = ((double) (int3_5.z - int3_6.z) * (double) (int3_1.x - int3_6.x) + (double) (int3_6.x - int3_5.x) * (double) (int3_1.z - int3_6.z)) / num2;
                      double num4 = ((double) (int3_6.z - int3_4.z) * (double) (int3_1.x - int3_6.x) + (double) (int3_4.x - int3_6.x) * (double) (int3_1.z - int3_6.z)) / num2;
                      int3_1.y = (int) Math.Round(num3 * (double) int3_4.y + num4 * (double) int3_5.y + (1.0 - num3 - num4) * (double) int3_6.y);
                      list3.Add(list2.Count);
                      list2.Add(int3_1);
                    }
                  }
                }
                else
                {
                  Pathfinding.Poly2Tri.Polygon p = (Pathfinding.Poly2Tri.Polygon) null;
                  int index3 = -1;
                  for (List<IntPoint> list10 = contour; list10 != null; list10 = index3 >= childs.Count ? (List<IntPoint>) null : childs[index3].Contour)
                  {
                    list1.Clear();
                    for (int index6 = 0; index6 < list10.Count; ++index6)
                    {
                      PolygonPoint polygonPoint = new PolygonPoint((double) list10[index6].X, (double) list10[index6].Y);
                      list1.Add(polygonPoint);
                      Int3 int3_1 = new Int3((int) list10[index6].X, 0, (int) list10[index6].Y);
                      double num2 = (double) (int3_5.z - int3_6.z) * (double) (int3_4.x - int3_6.x) + (double) (int3_6.x - int3_5.x) * (double) (int3_4.z - int3_6.z);
                      if (num2 == 0.0)
                      {
                        Debug.LogWarning((object) "Degenerate triangle");
                      }
                      else
                      {
                        double num3 = ((double) (int3_5.z - int3_6.z) * (double) (int3_1.x - int3_6.x) + (double) (int3_6.x - int3_5.x) * (double) (int3_1.z - int3_6.z)) / num2;
                        double num4 = ((double) (int3_6.z - int3_4.z) * (double) (int3_1.x - int3_6.x) + (double) (int3_4.x - int3_6.x) * (double) (int3_1.z - int3_6.z)) / num2;
                        int3_1.y = (int) Math.Round(num3 * (double) int3_4.y + num4 * (double) int3_5.y + (1.0 - num3 - num4) * (double) int3_6.y);
                        dictionary1[(TriangulationPoint) polygonPoint] = list2.Count;
                        list2.Add(int3_1);
                      }
                    }
                    Pathfinding.Poly2Tri.Polygon poly;
                    if (stack.Count > 0)
                    {
                      poly = stack.Pop();
                      poly.AddPoints((IEnumerable<PolygonPoint>) list1);
                    }
                    else
                      poly = new Pathfinding.Poly2Tri.Polygon((IList<PolygonPoint>) list1);
                    if (p == null)
                      p = poly;
                    else
                      p.AddHole(poly);
                    ++index3;
                  }
                  try
                  {
                    P2T.Triangulate(p);
                  }
                  catch (PointOnEdgeException ex)
                  {
                    Debug.LogWarning((object) ("PointOnEdgeException, perturbating vertices slightly ( at " + (object) index1 + " in " + (string) (object) mode + ")"));
                    this.CutPoly(verts, tris, ref outVertsArr, ref outTrisArr, out outVCount, out outTCount, extraShape, cuttingOffset, realBounds, mode, perturbate + 1);
                    return;
                  }
                  for (int index6 = 0; index6 < p.Triangles.Count; ++index6)
                  {
                    DelaunayTriangle delaunayTriangle = p.Triangles[index6];
                    list3.Add(dictionary1[delaunayTriangle.Points._0]);
                    list3.Add(dictionary1[delaunayTriangle.Points._1]);
                    list3.Add(dictionary1[delaunayTriangle.Points._2]);
                  }
                  if (p.Holes != null)
                  {
                    for (int index6 = 0; index6 < p.Holes.Count; ++index6)
                    {
                      p.Holes[index6].Points.Clear();
                      p.Holes[index6].ClearTriangles();
                      if (p.Holes[index6].Holes != null)
                        p.Holes[index6].Holes.Clear();
                      stack.Push(p.Holes[index6]);
                    }
                  }
                  p.ClearTriangles();
                  if (p.Holes != null)
                    p.Holes.Clear();
                  p.Points.Clear();
                  stack.Push(p);
                }
              }
            }
          }
          goto label_39;
label_129:
          Dictionary<Int3, int> dictionary2 = this.cached_Int3_int_dict;
          dictionary2.Clear();
          if (this.cached_int_array.Length < list2.Count)
            this.cached_int_array = new int[Math.Max(this.cached_int_array.Length * 2, list2.Count)];
          int[] numArray = this.cached_int_array;
          int index7 = 0;
          for (int index1 = 0; index1 < list2.Count; ++index1)
          {
            int num2;
            if (!dictionary2.TryGetValue(list2[index1], out num2))
            {
              dictionary2.Add(list2[index1], index7);
              numArray[index1] = index7;
              list2[index7] = list2[index1];
              ++index7;
            }
            else
              numArray[index1] = num2;
          }
          outTCount = list3.Count;
          if (outTrisArr == null || outTrisArr.Length < outTCount)
            outTrisArr = new int[outTCount];
          for (int index1 = 0; index1 < outTCount; ++index1)
            outTrisArr[index1] = numArray[list3[index1]];
          outVCount = index7;
          if (outVertsArr == null || outVertsArr.Length < outVCount)
            outVertsArr = new Int3[outVCount];
          for (int index1 = 0; index1 < outVCount; ++index1)
            outVertsArr[index1] = list2[index1];
          for (int index1 = 0; index1 < list4.Count; ++index1)
            list4[index1].UsedForCut();
          ListPool<Int3>.Release(list2);
          ListPool<int>.Release(list3);
          ListPool<int>.Release(list5);
          ListPool<Int2>.Release(list7);
          ListPool<bool>.Release(list8);
          ListPool<bool>.Release(list9);
          ListPool<Pathfinding.IntRect>.Release(list6);
          ListPool<NavmeshCut>.Release(list4);
        }
      }
    }

    private void DelaunayRefinement(Int3[] verts, int[] tris, ref int vCount, ref int tCount, bool delaunay, bool colinear, Int3 worldOffset)
    {
      if (tCount % 3 != 0)
        throw new Exception("Triangle array length must be a multiple of 3");
      Dictionary<Int2, int> dictionary = this.cached_Int2_int_dict;
      dictionary.Clear();
      int index1 = 0;
      while (index1 < tCount)
      {
        if (!Pathfinding.Polygon.IsClockwise(verts[tris[index1]], verts[tris[index1 + 1]], verts[tris[index1 + 2]]))
        {
          int num = tris[index1];
          tris[index1] = tris[index1 + 2];
          tris[index1 + 2] = num;
        }
        dictionary[new Int2(tris[index1], tris[index1 + 1])] = index1 + 2;
        dictionary[new Int2(tris[index1 + 1], tris[index1 + 2])] = index1;
        dictionary[new Int2(tris[index1 + 2], tris[index1])] = index1 + 1;
        index1 += 3;
      }
      int num1 = 9;
      int index2 = 0;
      while (index2 < tCount)
      {
        for (int index3 = 0; index3 < 3; ++index3)
        {
          int index4;
          if (dictionary.TryGetValue(new Int2(tris[index2 + (index3 + 1) % 3], tris[index2 + index3 % 3]), out index4))
          {
            Int3 a = verts[tris[index2 + (index3 + 2) % 3]];
            Int3 int3_1 = verts[tris[index2 + (index3 + 1) % 3]];
            Int3 b = verts[tris[index2 + (index3 + 3) % 3]];
            Int3 int3_2 = verts[tris[index4]];
            a.y = 0;
            int3_1.y = 0;
            b.y = 0;
            int3_2.y = 0;
            bool flag = false;
            if (!Pathfinding.Polygon.Left(a, b, int3_2) || Pathfinding.Polygon.LeftNotColinear(a, int3_1, int3_2))
            {
              if (colinear)
                flag = true;
              else
                continue;
            }
            if (colinear && (double) AstarMath.DistancePointSegment(a, int3_2, int3_1) < (double) num1 && (!dictionary.ContainsKey(new Int2(tris[index2 + (index3 + 2) % 3], tris[index2 + (index3 + 1) % 3])) && !dictionary.ContainsKey(new Int2(tris[index2 + (index3 + 1) % 3], tris[index4]))))
            {
              tCount = tCount - 3;
              int index5 = index4 / 3 * 3;
              tris[index2 + (index3 + 1) % 3] = tris[index4];
              if (index5 != tCount)
              {
                tris[index5] = tris[tCount];
                tris[index5 + 1] = tris[tCount + 1];
                tris[index5 + 2] = tris[tCount + 2];
                dictionary[new Int2(tris[index5], tris[index5 + 1])] = index5 + 2;
                dictionary[new Int2(tris[index5 + 1], tris[index5 + 2])] = index5;
                dictionary[new Int2(tris[index5 + 2], tris[index5])] = index5 + 1;
                tris[tCount] = 0;
                tris[tCount + 1] = 0;
                tris[tCount + 2] = 0;
              }
              else
                tCount = tCount + 3;
              dictionary[new Int2(tris[index2], tris[index2 + 1])] = index2 + 2;
              dictionary[new Int2(tris[index2 + 1], tris[index2 + 2])] = index2;
              dictionary[new Int2(tris[index2 + 2], tris[index2])] = index2 + 1;
            }
            else if (delaunay && !flag)
            {
              float num2 = Int3.Angle(int3_1 - a, b - a);
              if ((double) Int3.Angle(int3_1 - int3_2, b - int3_2) > 6.28318548202515 - 2.0 * (double) num2)
              {
                tris[index2 + (index3 + 1) % 3] = tris[index4];
                int index5 = index4 / 3 * 3;
                int num3 = index4 - index5;
                tris[index5 + (num3 - 1 + 3) % 3] = tris[index2 + (index3 + 2) % 3];
                dictionary[new Int2(tris[index2], tris[index2 + 1])] = index2 + 2;
                dictionary[new Int2(tris[index2 + 1], tris[index2 + 2])] = index2;
                dictionary[new Int2(tris[index2 + 2], tris[index2])] = index2 + 1;
                dictionary[new Int2(tris[index5], tris[index5 + 1])] = index5 + 2;
                dictionary[new Int2(tris[index5 + 1], tris[index5 + 2])] = index5;
                dictionary[new Int2(tris[index5 + 2], tris[index5])] = index5 + 1;
              }
            }
          }
        }
        index2 += 3;
      }
    }

    private Vector3 Point2D2V3(TriangulationPoint p)
    {
      return new Vector3((float) p.X, 0.0f, (float) p.Y) * (1.0 / 1000.0);
    }

    private Int3 IntPoint2Int3(IntPoint p)
    {
      return new Int3((int) p.X, 0, (int) p.Y);
    }

    public void ClearTile(int x, int z)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      TileHandler.\u003CClearTile\u003Ec__AnonStorey34 tileCAnonStorey34 = new TileHandler.\u003CClearTile\u003Ec__AnonStorey34();
      // ISSUE: reference to a compiler-generated field
      tileCAnonStorey34.x = x;
      // ISSUE: reference to a compiler-generated field
      tileCAnonStorey34.z = z;
      // ISSUE: reference to a compiler-generated field
      tileCAnonStorey34.\u003C\u003Ef__this = this;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if ((UnityEngine.Object) AstarPath.active == (UnityEngine.Object) null || tileCAnonStorey34.x < 0 || (tileCAnonStorey34.z < 0 || tileCAnonStorey34.x >= this.graph.tileXCount) || tileCAnonStorey34.z >= this.graph.tileZCount)
        return;
      // ISSUE: reference to a compiler-generated method
      AstarPath.active.AddWorkItem(new AstarPath.AstarWorkItem(new Func<bool, bool>(tileCAnonStorey34.\u003C\u003Em__2B)));
    }

    public void ReloadInBounds(Bounds b)
    {
      Int2 tileCoordinates1 = this.graph.GetTileCoordinates(b.min);
      Int2 tileCoordinates2 = this.graph.GetTileCoordinates(b.max);
      Pathfinding.IntRect intRect = Pathfinding.IntRect.Intersection(new Pathfinding.IntRect(tileCoordinates1.x, tileCoordinates1.y, tileCoordinates2.x, tileCoordinates2.y), new Pathfinding.IntRect(0, 0, this.graph.tileXCount - 1, this.graph.tileZCount - 1));
      if (!intRect.IsValid())
        return;
      for (int z = intRect.ymin; z <= intRect.ymax; ++z)
      {
        for (int x = intRect.xmin; x <= intRect.xmax; ++x)
          this.ReloadTile(x, z);
      }
    }

    public void ReloadTile(int x, int z)
    {
      if (x < 0 || z < 0 || (x >= this.graph.tileXCount || z >= this.graph.tileZCount))
        return;
      int index = x + z * this.graph.tileXCount;
      if (this.activeTileTypes[index] == null)
        return;
      this.LoadTile(this.activeTileTypes[index], x, z, this.activeTileRotations[index], this.activeTileOffsets[index]);
    }

    public void CutShapeWithTile(int x, int z, Int3[] shape, ref Int3[] verts, ref int[] tris, out int vCount, out int tCount)
    {
      if (this.isBatching)
        throw new Exception("Cannot cut with shape when batching. Please stop batching first.");
      int index1 = x + z * this.graph.tileXCount;
      if (x < 0 || z < 0 || (x >= this.graph.tileXCount || z >= this.graph.tileZCount) || this.activeTileTypes[index1] == null)
      {
        verts = new Int3[0];
        tris = new int[0];
        vCount = 0;
        tCount = 0;
      }
      else
      {
        Int3[] verts1;
        int[] tris1;
        this.activeTileTypes[index1].Load(out verts1, out tris1, this.activeTileRotations[index1], this.activeTileOffsets[index1]);
        Bounds tileBounds = this.graph.GetTileBounds(x, z);
        Int3 cuttingOffset = -(Int3) tileBounds.min;
        this.CutPoly(verts1, tris1, ref verts, ref tris, out vCount, out tCount, shape, cuttingOffset, tileBounds, TileHandler.CutMode.CutExtra, 0);
        for (int index2 = 0; index2 < verts.Length; ++index2)
          verts[index2] -= cuttingOffset;
      }
    }

    protected static T[] ShrinkArray<T>(T[] arr, int newLength)
    {
      newLength = Math.Min(newLength, arr.Length);
      T[] objArray = new T[newLength];
      if (newLength % 4 == 0)
      {
        int index = 0;
        while (index < newLength)
        {
          objArray[index] = arr[index];
          objArray[index + 1] = arr[index + 1];
          objArray[index + 2] = arr[index + 2];
          objArray[index + 3] = arr[index + 3];
          index += 4;
        }
      }
      else if (newLength % 3 == 0)
      {
        int index = 0;
        while (index < newLength)
        {
          objArray[index] = arr[index];
          objArray[index + 1] = arr[index + 1];
          objArray[index + 2] = arr[index + 2];
          index += 3;
        }
      }
      else if (newLength % 2 == 0)
      {
        int index = 0;
        while (index < newLength)
        {
          objArray[index] = arr[index];
          objArray[index + 1] = arr[index + 1];
          index += 2;
        }
      }
      else
      {
        for (int index = 0; index < newLength; ++index)
          objArray[index] = arr[index];
      }
      return objArray;
    }

    public void LoadTile(TileHandler.TileType tile, int x, int z, int rotation, int yoffset)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      TileHandler.\u003CLoadTile\u003Ec__AnonStorey35 tileCAnonStorey35 = new TileHandler.\u003CLoadTile\u003Ec__AnonStorey35();
      // ISSUE: reference to a compiler-generated field
      tileCAnonStorey35.yoffset = yoffset;
      // ISSUE: reference to a compiler-generated field
      tileCAnonStorey35.rotation = rotation;
      // ISSUE: reference to a compiler-generated field
      tileCAnonStorey35.tile = tile;
      // ISSUE: reference to a compiler-generated field
      tileCAnonStorey35.x = x;
      // ISSUE: reference to a compiler-generated field
      tileCAnonStorey35.z = z;
      // ISSUE: reference to a compiler-generated field
      tileCAnonStorey35.\u003C\u003Ef__this = this;
      // ISSUE: reference to a compiler-generated field
      if (tileCAnonStorey35.tile == null)
        throw new ArgumentNullException("tile");
      if ((UnityEngine.Object) AstarPath.active == (UnityEngine.Object) null)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      tileCAnonStorey35.index = tileCAnonStorey35.x + tileCAnonStorey35.z * this.graph.tileXCount;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      tileCAnonStorey35.rotation = tileCAnonStorey35.rotation % 4;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.isBatching && this.reloadedInBatch[tileCAnonStorey35.index] && (this.activeTileOffsets[tileCAnonStorey35.index] == tileCAnonStorey35.yoffset && this.activeTileRotations[tileCAnonStorey35.index] == tileCAnonStorey35.rotation) && this.activeTileTypes[tileCAnonStorey35.index] == tileCAnonStorey35.tile)
        return;
      if (this.isBatching)
      {
        // ISSUE: reference to a compiler-generated field
        this.reloadedInBatch[tileCAnonStorey35.index] = true;
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.activeTileOffsets[tileCAnonStorey35.index] = tileCAnonStorey35.yoffset;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.activeTileRotations[tileCAnonStorey35.index] = tileCAnonStorey35.rotation;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.activeTileTypes[tileCAnonStorey35.index] = tileCAnonStorey35.tile;
      // ISSUE: reference to a compiler-generated method
      AstarPath.active.AddWorkItem(new AstarPath.AstarWorkItem(new Func<bool, bool>(tileCAnonStorey35.\u003C\u003Em__2C)));
    }

    public class TileType
    {
      private static readonly int[] Rotations = new int[16]
      {
        1,
        0,
        0,
        1,
        0,
        1,
        -1,
        0,
        -1,
        0,
        0,
        -1,
        0,
        -1,
        1,
        0
      };
      private Int3[] verts;
      private int[] tris;
      private Int3 offset;
      private int lastYOffset;
      private int lastRotation;
      private int width;
      private int depth;

      public int Width
      {
        get
        {
          return this.width;
        }
      }

      public int Depth
      {
        get
        {
          return this.depth;
        }
      }

      public TileType(Int3[] sourceVerts, int[] sourceTris, Int3 tileSize, Int3 centerOffset, int width = 1, int depth = 1)
      {
        if (sourceVerts == null)
          throw new ArgumentNullException("sourceVerts");
        if (sourceTris == null)
          throw new ArgumentNullException("sourceTris");
        this.tris = new int[sourceTris.Length];
        for (int index = 0; index < this.tris.Length; ++index)
          this.tris[index] = sourceTris[index];
        this.verts = new Int3[sourceVerts.Length];
        for (int index = 0; index < sourceVerts.Length; ++index)
          this.verts[index] = sourceVerts[index] + centerOffset;
        this.offset = tileSize / 2f;
        this.offset.x *= width;
        this.offset.z *= depth;
        this.offset.y = 0;
        for (int index = 0; index < sourceVerts.Length; ++index)
          this.verts[index] = this.verts[index] + this.offset;
        this.lastRotation = 0;
        this.lastYOffset = 0;
        this.width = width;
        this.depth = depth;
      }

      public TileType(Mesh source, Int3 tileSize, Int3 centerOffset, int width = 1, int depth = 1)
      {
        if ((UnityEngine.Object) source == (UnityEngine.Object) null)
          throw new ArgumentNullException("source");
        Vector3[] vertices = source.vertices;
        this.tris = source.triangles;
        this.verts = new Int3[vertices.Length];
        for (int index = 0; index < vertices.Length; ++index)
          this.verts[index] = (Int3) vertices[index] + centerOffset;
        this.offset = tileSize / 2f;
        this.offset.x *= width;
        this.offset.z *= depth;
        this.offset.y = 0;
        for (int index = 0; index < vertices.Length; ++index)
          this.verts[index] = this.verts[index] + this.offset;
        this.lastRotation = 0;
        this.lastYOffset = 0;
        this.width = width;
        this.depth = depth;
      }

      public void Load(out Int3[] verts, out int[] tris, int rotation, int yoffset)
      {
        rotation = (rotation % 4 + 4) % 4;
        int num1 = rotation;
        rotation = (rotation - this.lastRotation % 4 + 4) % 4;
        this.lastRotation = num1;
        verts = this.verts;
        int num2 = yoffset - this.lastYOffset;
        this.lastYOffset = yoffset;
        if (rotation != 0 || num2 != 0)
        {
          for (int index = 0; index < verts.Length; ++index)
          {
            Int3 int3_1 = verts[index] - this.offset;
            Int3 int3_2 = int3_1;
            int3_2.y += num2;
            int3_2.x = int3_1.x * TileHandler.TileType.Rotations[rotation * 4] + int3_1.z * TileHandler.TileType.Rotations[rotation * 4 + 1];
            int3_2.z = int3_1.x * TileHandler.TileType.Rotations[rotation * 4 + 2] + int3_1.z * TileHandler.TileType.Rotations[rotation * 4 + 3];
            verts[index] = int3_2 + this.offset;
          }
        }
        tris = this.tris;
      }
    }

    public enum CutMode
    {
      CutAll = 1,
      CutDual = 2,
      CutExtra = 4,
    }
  }
}
