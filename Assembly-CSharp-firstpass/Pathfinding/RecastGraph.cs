// Decompiled with JetBrains decompiler
// Type: Pathfinding.RecastGraph
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding.Serialization;
using Pathfinding.Serialization.JsonFx;
using Pathfinding.Util;
using Pathfinding.Voxels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace Pathfinding
{
  [JsonOptIn]
  [Serializable]
  public class RecastGraph : NavGraph, IUpdatableGraph, IRaycastableGraph, INavmesh, INavmeshHolder, IFunnelGraph
  {
    public bool dynamic = true;
    [JsonMember]
    public float characterRadius = 0.5f;
    [JsonMember]
    public float contourMaxError = 2f;
    [JsonMember]
    public float cellSize = 0.5f;
    [JsonMember]
    public float cellHeight = 0.4f;
    [JsonMember]
    public float walkableHeight = 2f;
    [JsonMember]
    public float walkableClimb = 0.5f;
    [JsonMember]
    public float maxSlope = 30f;
    [JsonMember]
    public float maxEdgeLength = 20f;
    [JsonMember]
    public float minRegionSize = 3f;
    [JsonMember]
    public int editorTileSize = 128;
    [JsonMember]
    public int tileSizeX = 128;
    [JsonMember]
    public int tileSizeZ = 128;
    [JsonMember]
    public bool rasterizeMeshes = true;
    [JsonMember]
    public bool rasterizeTerrain = true;
    [JsonMember]
    public bool rasterizeTrees = true;
    [JsonMember]
    public float colliderRasterizeDetail = 10f;
    [JsonMember]
    public Vector3 forcedBoundsSize = new Vector3(100f, 40f, 100f);
    [JsonMember]
    public LayerMask mask = (LayerMask) -1;
    [JsonMember]
    public List<string> tagMask = new List<string>();
    [JsonMember]
    public bool showMeshOutline = true;
    [JsonMember]
    public int terrainSampleSize = 3;
    private List<int> batchUpdatedTiles = new List<int>();
    private Dictionary<Int2, int> cachedInt2_int_dict = new Dictionary<Int2, int>();
    private Dictionary<Int3, int> cachedInt3_int_dict = new Dictionary<Int3, int>();
    private readonly int[] BoxColliderTris = new int[36]
    {
      0,
      1,
      2,
      0,
      2,
      3,
      6,
      5,
      4,
      7,
      6,
      4,
      0,
      5,
      1,
      0,
      4,
      5,
      1,
      6,
      2,
      1,
      5,
      6,
      2,
      7,
      3,
      2,
      6,
      7,
      3,
      4,
      0,
      3,
      7,
      4
    };
    private readonly Vector3[] BoxColliderVerts = new Vector3[8]
    {
      new Vector3(-1f, -1f, -1f),
      new Vector3(1f, -1f, -1f),
      new Vector3(1f, -1f, 1f),
      new Vector3(-1f, -1f, 1f),
      new Vector3(-1f, 1f, -1f),
      new Vector3(1f, 1f, -1f),
      new Vector3(1f, 1f, 1f),
      new Vector3(-1f, 1f, 1f)
    };
    private List<RecastGraph.CapsuleCache> capsuleCache = new List<RecastGraph.CapsuleCache>();
    public const int VertexIndexMask = 4095;
    public const int TileIndexMask = 524287;
    public const int TileIndexOffset = 12;
    public const int BorderVertexMask = 1;
    public const int BorderVertexOffset = 31;
    [JsonMember]
    public bool nearestSearchOnlyXZ;
    [JsonMember]
    public bool useTiles;
    public bool scanEmptyGraph;
    [JsonMember]
    public RecastGraph.RelevantGraphSurfaceMode relevantGraphSurfaceMode;
    [JsonMember]
    public bool rasterizeColliders;
    [JsonMember]
    public Vector3 forcedBoundsCenter;
    [JsonMember]
    public bool showNodeConnections;
    private Voxelize globalVox;
    private BBTree _bbTree;
    private Int3[] _vertices;
    private Vector3[] _vectorVertices;
    public int tileXCount;
    public int tileZCount;
    private RecastGraph.NavmeshTile[] tiles;
    private bool batchTileUpdate;

    public Bounds forcedBounds
    {
      get
      {
        return new Bounds(this.forcedBoundsCenter, this.forcedBoundsSize);
      }
    }

    public BBTree bbTree
    {
      get
      {
        return this._bbTree;
      }
      set
      {
        this._bbTree = value;
      }
    }

    public Int3[] vertices
    {
      get
      {
        return this._vertices;
      }
      set
      {
        this._vertices = value;
      }
    }

    public Vector3[] vectorVertices
    {
      get
      {
        if (this._vectorVertices != null && this._vectorVertices.Length == this.vertices.Length)
          return this._vectorVertices;
        if (this.vertices == null)
          return (Vector3[]) null;
        this._vectorVertices = new Vector3[this.vertices.Length];
        for (int index = 0; index < this._vectorVertices.Length; ++index)
          this._vectorVertices[index] = (Vector3) this.vertices[index];
        return this._vectorVertices;
      }
    }

    public override void CreateNodes(int number)
    {
      throw new NotSupportedException();
    }

    public Int3 GetVertex(int index)
    {
      return this.tiles[index >> 12 & 524287].GetVertex(index);
    }

    public int GetTileIndex(int index)
    {
      return index >> 12 & 524287;
    }

    public int GetVertexArrayIndex(int index)
    {
      return index & 4095;
    }

    public void GetTileCoordinates(int tileIndex, out int x, out int z)
    {
      z = tileIndex / this.tileXCount;
      x = tileIndex - z * this.tileXCount;
    }

    public RecastGraph.NavmeshTile[] GetTiles()
    {
      return this.tiles;
    }

    public void SetTiles(RecastGraph.NavmeshTile[] newTiles)
    {
      this.tiles = newTiles;
    }

    public Bounds GetTileBounds(int x, int z)
    {
      Bounds bounds = new Bounds();
      bounds.SetMinMax(new Vector3((float) (x * this.tileSizeX) * this.cellSize, 0.0f, (float) (z * this.tileSizeZ) * this.cellSize) + this.forcedBounds.min, new Vector3((float) ((x + 1) * this.tileSizeX) * this.cellSize, this.forcedBounds.size.y, (float) ((z + 1) * this.tileSizeZ) * this.cellSize) + this.forcedBounds.min);
      return bounds;
    }

    public Bounds GetTileBounds(int x, int z, int width, int depth)
    {
      Bounds bounds = new Bounds();
      bounds.SetMinMax(new Vector3((float) (x * this.tileSizeX) * this.cellSize, 0.0f, (float) (z * this.tileSizeZ) * this.cellSize) + this.forcedBounds.min, new Vector3((float) ((x + width) * this.tileSizeX) * this.cellSize, this.forcedBounds.size.y, (float) ((z + depth) * this.tileSizeZ) * this.cellSize) + this.forcedBounds.min);
      return bounds;
    }

    public Int2 GetTileCoordinates(Vector3 p)
    {
      p -= this.forcedBounds.min;
      p.x /= this.cellSize * (float) this.tileSizeX;
      p.z /= this.cellSize * (float) this.tileSizeZ;
      return new Int2((int) p.x, (int) p.z);
    }

    public override void OnDestroy()
    {
      base.OnDestroy();
      TriangleMeshNode.SetNavmeshHolder(this.active.astarData.GetGraphIndex((NavGraph) this), (INavmeshHolder) null);
    }

    private static RecastGraph.NavmeshTile NewEmptyTile(int x, int z)
    {
      RecastGraph.NavmeshTile navmeshTile = new RecastGraph.NavmeshTile();
      navmeshTile.x = x;
      navmeshTile.z = z;
      navmeshTile.w = 1;
      navmeshTile.d = 1;
      navmeshTile.verts = new Int3[0];
      navmeshTile.tris = new int[0];
      navmeshTile.nodes = new TriangleMeshNode[0];
      navmeshTile.bbTree = new BBTree((INavmeshHolder) navmeshTile);
      return navmeshTile;
    }

    public override void GetNodes(GraphNodeDelegateCancelable del)
    {
      if (this.tiles == null)
        return;
      for (int index1 = 0; index1 < this.tiles.Length; ++index1)
      {
        if (this.tiles[index1] != null && this.tiles[index1].x + this.tiles[index1].z * this.tileXCount == index1)
        {
          TriangleMeshNode[] triangleMeshNodeArray = this.tiles[index1].nodes;
          if (triangleMeshNodeArray != null)
          {
            int index2 = 0;
            while (index2 < triangleMeshNodeArray.Length && del((GraphNode) triangleMeshNodeArray[index2]))
              ++index2;
          }
        }
      }
    }

    public Vector3 ClosestPointOnNode(TriangleMeshNode node, Vector3 pos)
    {
      return Polygon.ClosestPointOnTriangle((Vector3) this.GetVertex(node.v0), (Vector3) this.GetVertex(node.v1), (Vector3) this.GetVertex(node.v2), pos);
    }

    public bool ContainsPoint(TriangleMeshNode node, Vector3 pos)
    {
      return Polygon.IsClockwise((Vector3) this.GetVertex(node.v0), (Vector3) this.GetVertex(node.v1), pos) && Polygon.IsClockwise((Vector3) this.GetVertex(node.v1), (Vector3) this.GetVertex(node.v2), pos) && Polygon.IsClockwise((Vector3) this.GetVertex(node.v2), (Vector3) this.GetVertex(node.v0), pos);
    }

    public void SnapForceBoundsToScene()
    {
      List<ExtraMesh> sceneMeshes = this.GetSceneMeshes(this.forcedBounds);
      if (sceneMeshes.Count == 0)
        return;
      Bounds bounds1 = new Bounds();
      Bounds bounds2 = sceneMeshes[0].bounds;
      for (int index = 1; index < sceneMeshes.Count; ++index)
        bounds2.Encapsulate(sceneMeshes[index].bounds);
      this.forcedBoundsCenter = bounds2.center;
      this.forcedBoundsSize = bounds2.size;
    }

    public void GetRecastMeshObjs(Bounds bounds, List<ExtraMesh> buffer)
    {
      List<RecastMeshObj> list = ListPool<RecastMeshObj>.Claim();
      RecastMeshObj.GetAllInBounds(list, bounds);
      Dictionary<Mesh, Vector3[]> dictionary1 = new Dictionary<Mesh, Vector3[]>();
      Dictionary<Mesh, int[]> dictionary2 = new Dictionary<Mesh, int[]>();
      for (int index = 0; index < list.Count; ++index)
      {
        MeshFilter meshFilter = list[index].GetMeshFilter();
        if ((UnityEngine.Object) meshFilter != (UnityEngine.Object) null)
        {
          Mesh sharedMesh = meshFilter.sharedMesh;
          ExtraMesh extraMesh = new ExtraMesh();
          extraMesh.matrix = meshFilter.GetComponent<Renderer>().localToWorldMatrix;
          extraMesh.original = meshFilter;
          extraMesh.area = list[index].area;
          if (dictionary1.ContainsKey(sharedMesh))
          {
            extraMesh.vertices = dictionary1[sharedMesh];
            extraMesh.triangles = dictionary2[sharedMesh];
          }
          else
          {
            extraMesh.vertices = sharedMesh.vertices;
            extraMesh.triangles = sharedMesh.triangles;
            dictionary1[sharedMesh] = extraMesh.vertices;
            dictionary2[sharedMesh] = extraMesh.triangles;
          }
          extraMesh.bounds = meshFilter.GetComponent<Renderer>().bounds;
          buffer.Add(extraMesh);
        }
        else
        {
          Collider collider = list[index].GetCollider();
          if ((UnityEngine.Object) collider == (UnityEngine.Object) null)
          {
            UnityEngine.Debug.LogError((object) ("RecastMeshObject (" + list[index].gameObject.name + ") didn't have a collider or MeshFilter attached"));
          }
          else
          {
            ExtraMesh extraMesh = this.RasterizeCollider(collider);
            extraMesh.area = list[index].area;
            if (extraMesh.vertices != null)
              buffer.Add(extraMesh);
          }
        }
      }
      this.capsuleCache.Clear();
      ListPool<RecastMeshObj>.Release(list);
    }

    public List<ExtraMesh> GetSceneMeshes(Bounds bounds)
    {
      if ((this.tagMask == null || this.tagMask.Count <= 0) && (int) this.mask == 0)
        return new List<ExtraMesh>();
      MeshFilter[] meshFilterArray = UnityEngine.Object.FindObjectsOfType(typeof (MeshFilter)) as MeshFilter[];
      List<MeshFilter> list1 = new List<MeshFilter>(meshFilterArray.Length / 3);
      for (int index = 0; index < meshFilterArray.Length; ++index)
      {
        MeshFilter meshFilter = meshFilterArray[index];
        if ((UnityEngine.Object) meshFilter.GetComponent<Renderer>() != (UnityEngine.Object) null && (UnityEngine.Object) meshFilter.sharedMesh != (UnityEngine.Object) null && meshFilter.GetComponent<Renderer>().enabled && (((1 << meshFilter.gameObject.layer & (int) this.mask) == 1 << meshFilter.gameObject.layer || this.tagMask.Contains(meshFilter.tag)) && (UnityEngine.Object) meshFilter.GetComponent<RecastMeshObj>() == (UnityEngine.Object) null))
          list1.Add(meshFilter);
      }
      List<ExtraMesh> list2 = new List<ExtraMesh>();
      Dictionary<Mesh, Vector3[]> dictionary1 = new Dictionary<Mesh, Vector3[]>();
      Dictionary<Mesh, int[]> dictionary2 = new Dictionary<Mesh, int[]>();
      bool flag = false;
      using (List<MeshFilter>.Enumerator enumerator = list1.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          MeshFilter current = enumerator.Current;
          if (current.GetComponent<Renderer>().isPartOfStaticBatch)
            flag = true;
          else if (current.GetComponent<Renderer>().bounds.Intersects(bounds))
          {
            Mesh sharedMesh = current.sharedMesh;
            ExtraMesh extraMesh = new ExtraMesh();
            extraMesh.matrix = current.GetComponent<Renderer>().localToWorldMatrix;
            extraMesh.original = current;
            if (dictionary1.ContainsKey(sharedMesh))
            {
              extraMesh.vertices = dictionary1[sharedMesh];
              extraMesh.triangles = dictionary2[sharedMesh];
            }
            else
            {
              extraMesh.vertices = sharedMesh.vertices;
              extraMesh.triangles = sharedMesh.triangles;
              dictionary1[sharedMesh] = extraMesh.vertices;
              dictionary2[sharedMesh] = extraMesh.triangles;
            }
            extraMesh.bounds = current.GetComponent<Renderer>().bounds;
            list2.Add(extraMesh);
          }
          if (flag)
            UnityEngine.Debug.LogWarning((object) "Some meshes were statically batched. These meshes can not be used for navmesh calculation due to technical constraints.");
        }
      }
      return list2;
    }

    public IntRect GetTouchingTiles(Bounds b)
    {
      b.center -= this.forcedBounds.min;
      return IntRect.Intersection(new IntRect(Mathf.FloorToInt(b.min.x / ((float) this.tileSizeX * this.cellSize)), Mathf.FloorToInt(b.min.z / ((float) this.tileSizeZ * this.cellSize)), Mathf.FloorToInt(b.max.x / ((float) this.tileSizeX * this.cellSize)), Mathf.FloorToInt(b.max.z / ((float) this.tileSizeZ * this.cellSize))), new IntRect(0, 0, this.tileXCount - 1, this.tileZCount - 1));
    }

    public IntRect GetTouchingTilesRound(Bounds b)
    {
      b.center -= this.forcedBounds.min;
      return IntRect.Intersection(new IntRect(Mathf.RoundToInt(b.min.x / ((float) this.tileSizeX * this.cellSize)), Mathf.RoundToInt(b.min.z / ((float) this.tileSizeZ * this.cellSize)), Mathf.RoundToInt(b.max.x / ((float) this.tileSizeX * this.cellSize)) - 1, Mathf.RoundToInt(b.max.z / ((float) this.tileSizeZ * this.cellSize)) - 1), new IntRect(0, 0, this.tileXCount - 1, this.tileZCount - 1));
    }

    public GraphUpdateThreading CanUpdateAsync(GraphUpdateObject o)
    {
      return o.updatePhysics ? GraphUpdateThreading.SeparateAndUnityInit : GraphUpdateThreading.SeparateThread;
    }

    public void UpdateAreaInit(GraphUpdateObject o)
    {
      if (!o.updatePhysics)
        return;
      if (!this.dynamic)
        throw new Exception("Recast graph must be marked as dynamic to enable graph updates");
      RelevantGraphSurface.UpdateAllPositions();
      IntRect touchingTiles = this.GetTouchingTiles(o.bounds);
      Bounds bounds = new Bounds();
      Vector3 min = this.forcedBounds.min;
      Vector3 max = this.forcedBounds.max;
      float num1 = (float) this.tileSizeX * this.cellSize;
      float num2 = (float) this.tileSizeZ * this.cellSize;
      bounds.SetMinMax(new Vector3((float) touchingTiles.xmin * num1, 0.0f, (float) touchingTiles.ymin * num2) + min, new Vector3((float) (touchingTiles.xmax + 1) * num1 + min.x, max.y, (float) (touchingTiles.ymax + 1) * num2 + min.z));
      int num3 = Mathf.CeilToInt(this.characterRadius / this.cellSize) + 3;
      bounds.Expand(new Vector3((float) num3, 0.0f, (float) num3) * this.cellSize * 2f);
      List<ExtraMesh> extraMeshes;
      if (this.CollectMeshes(out extraMeshes, bounds))
        ;
      Voxelize voxelize = this.globalVox;
      if (voxelize == null)
      {
        voxelize = new Voxelize(this.cellHeight, this.cellSize, this.walkableClimb, this.walkableHeight, this.maxSlope);
        voxelize.maxEdgeLength = this.maxEdgeLength;
        if (this.dynamic)
          this.globalVox = voxelize;
      }
      voxelize.inputExtraMeshes = extraMeshes;
    }

    public void UpdateArea(GraphUpdateObject guo)
    {
      Bounds bounds = guo.bounds;
      bounds.center -= this.forcedBounds.min;
      IntRect a = IntRect.Intersection(new IntRect(Mathf.FloorToInt(bounds.min.x / ((float) this.tileSizeX * this.cellSize)), Mathf.FloorToInt(bounds.min.z / ((float) this.tileSizeZ * this.cellSize)), Mathf.FloorToInt(bounds.max.x / ((float) this.tileSizeX * this.cellSize)), Mathf.FloorToInt(bounds.max.z / ((float) this.tileSizeZ * this.cellSize))), new IntRect(0, 0, this.tileXCount - 1, this.tileZCount - 1));
      if (!guo.updatePhysics)
      {
        for (int index1 = a.ymin; index1 <= a.ymax; ++index1)
        {
          for (int index2 = a.xmin; index2 <= a.xmax; ++index2)
            this.tiles[index1 * this.tileXCount + index2].flag = true;
        }
        for (int index1 = a.ymin; index1 <= a.ymax; ++index1)
        {
          for (int index2 = a.xmin; index2 <= a.xmax; ++index2)
          {
            RecastGraph.NavmeshTile navmeshTile = this.tiles[index1 * this.tileXCount + index2];
            if (navmeshTile.flag)
            {
              navmeshTile.flag = false;
              NavMeshGraph.UpdateArea(guo, (INavmesh) navmeshTile);
            }
          }
        }
      }
      else
      {
        if (!this.dynamic)
          throw new Exception("Recast graph must be marked as dynamic to enable graph updates with updatePhysics = true");
        Voxelize vox = this.globalVox;
        if (vox == null)
          throw new InvalidOperationException("No Voxelizer object. UpdateAreaInit should have been called before this function.");
        for (int index1 = a.xmin; index1 <= a.xmax; ++index1)
        {
          for (int index2 = a.ymin; index2 <= a.ymax; ++index2)
            this.RemoveConnectionsFromTile(this.tiles[index1 + index2 * this.tileXCount]);
        }
        for (int x = a.xmin; x <= a.xmax; ++x)
        {
          for (int z = a.ymin; z <= a.ymax; ++z)
            this.BuildTileMesh(vox, x, z);
        }
        uint num = (uint) AstarPath.active.astarData.GetGraphIndex((NavGraph) this);
        for (int index1 = a.xmin; index1 <= a.xmax; ++index1)
        {
          for (int index2 = a.ymin; index2 <= a.ymax; ++index2)
          {
            foreach (GraphNode graphNode in (GraphNode[]) this.tiles[index1 + index2 * this.tileXCount].nodes)
              graphNode.GraphIndex = num;
          }
        }
        a = a.Expand(1);
        a = IntRect.Intersection(a, new IntRect(0, 0, this.tileXCount - 1, this.tileZCount - 1));
        for (int x = a.xmin; x <= a.xmax; ++x)
        {
          for (int y = a.ymin; y <= a.ymax; ++y)
          {
            if (x < this.tileXCount - 1 && a.Contains(x + 1, y))
              this.ConnectTiles(this.tiles[x + y * this.tileXCount], this.tiles[x + 1 + y * this.tileXCount]);
            if (y < this.tileZCount - 1 && a.Contains(x, y + 1))
              this.ConnectTiles(this.tiles[x + y * this.tileXCount], this.tiles[x + (y + 1) * this.tileXCount]);
          }
        }
      }
    }

    public void ConnectTileWithNeighbours(RecastGraph.NavmeshTile tile)
    {
      if (tile.x > 0)
      {
        int num = tile.x - 1;
        for (int index = tile.z; index < tile.z + tile.d; ++index)
          this.ConnectTiles(this.tiles[num + index * this.tileXCount], tile);
      }
      if (tile.x + tile.w < this.tileXCount)
      {
        int num = tile.x + tile.w;
        for (int index = tile.z; index < tile.z + tile.d; ++index)
          this.ConnectTiles(this.tiles[num + index * this.tileXCount], tile);
      }
      if (tile.z > 0)
      {
        int num = tile.z - 1;
        for (int index = tile.x; index < tile.x + tile.w; ++index)
          this.ConnectTiles(this.tiles[index + num * this.tileXCount], tile);
      }
      if (tile.z + tile.d >= this.tileZCount)
        return;
      int num1 = tile.z + tile.d;
      for (int index = tile.x; index < tile.x + tile.w; ++index)
        this.ConnectTiles(this.tiles[index + num1 * this.tileXCount], tile);
    }

    public void RemoveConnectionsFromTile(RecastGraph.NavmeshTile tile)
    {
      if (tile.x > 0)
      {
        int num = tile.x - 1;
        for (int index = tile.z; index < tile.z + tile.d; ++index)
          this.RemoveConnectionsFromTo(this.tiles[num + index * this.tileXCount], tile);
      }
      if (tile.x + tile.w < this.tileXCount)
      {
        int num = tile.x + tile.w;
        for (int index = tile.z; index < tile.z + tile.d; ++index)
          this.RemoveConnectionsFromTo(this.tiles[num + index * this.tileXCount], tile);
      }
      if (tile.z > 0)
      {
        int num = tile.z - 1;
        for (int index = tile.x; index < tile.x + tile.w; ++index)
          this.RemoveConnectionsFromTo(this.tiles[index + num * this.tileXCount], tile);
      }
      if (tile.z + tile.d >= this.tileZCount)
        return;
      int num1 = tile.z + tile.d;
      for (int index = tile.x; index < tile.x + tile.w; ++index)
        this.RemoveConnectionsFromTo(this.tiles[index + num1 * this.tileXCount], tile);
    }

    public void RemoveConnectionsFromTo(RecastGraph.NavmeshTile a, RecastGraph.NavmeshTile b)
    {
      if (a == null || b == null || a == b)
        return;
      int num = b.x + b.z * this.tileXCount;
      for (int index1 = 0; index1 < a.nodes.Length; ++index1)
      {
        TriangleMeshNode triangleMeshNode1 = a.nodes[index1];
        if (triangleMeshNode1.connections != null)
        {
          for (int index2 = 0; index2 < triangleMeshNode1.connections.Length; ++index2)
          {
            TriangleMeshNode triangleMeshNode2 = triangleMeshNode1.connections[index2] as TriangleMeshNode;
            if (triangleMeshNode2 != null && (triangleMeshNode2.GetVertexIndex(0) >> 12 & 524287) == num)
            {
              triangleMeshNode1.RemoveConnection(triangleMeshNode1.connections[index2]);
              --index2;
            }
          }
        }
      }
    }

    public override NNInfo GetNearest(Vector3 position, NNConstraint constraint, GraphNode hint)
    {
      return this.GetNearestForce(position, (NNConstraint) null);
    }

    public override NNInfo GetNearestForce(Vector3 position, NNConstraint constraint)
    {
      if (this.tiles == null)
        return new NNInfo();
      Vector3 vector3 = position - this.forcedBounds.min;
      int num1 = Mathf.FloorToInt(vector3.x / (this.cellSize * (float) this.tileSizeX));
      int num2 = Mathf.FloorToInt(vector3.z / (this.cellSize * (float) this.tileSizeZ));
      int num3 = Mathf.Clamp(num1, 0, this.tileXCount - 1);
      int num4 = Mathf.Clamp(num2, 0, this.tileZCount - 1);
      int num5 = Math.Max(this.tileXCount, this.tileZCount);
      NNInfo previous = new NNInfo();
      float distance = float.PositiveInfinity;
      bool flag = this.nearestSearchOnlyXZ || constraint != null && constraint.distanceXZ;
      for (int index1 = 0; index1 < num5 && (flag || (double) distance >= (double) (index1 - 1) * (double) this.cellSize * (double) Math.Max(this.tileSizeX, this.tileSizeZ)); ++index1)
      {
        int num6 = Math.Min(index1 + num4 + 1, this.tileZCount);
        for (int index2 = Math.Max(-index1 + num4, 0); index2 < num6; ++index2)
        {
          int num7 = Math.Abs(index1 - Math.Abs(index2 - num4));
          if (-num7 + num3 >= 0)
          {
            RecastGraph.NavmeshTile navmeshTile = this.tiles[-num7 + num3 + index2 * this.tileXCount];
            if (navmeshTile != null)
            {
              if (flag)
              {
                previous = navmeshTile.bbTree.QueryClosestXZ(position, constraint, ref distance, previous);
                if ((double) distance < double.PositiveInfinity)
                  break;
              }
              else
                previous = navmeshTile.bbTree.QueryClosest(position, constraint, ref distance, previous);
            }
          }
          if (num7 != 0 && num7 + num3 < this.tileXCount)
          {
            RecastGraph.NavmeshTile navmeshTile = this.tiles[num7 + num3 + index2 * this.tileXCount];
            if (navmeshTile != null)
            {
              if (flag)
              {
                previous = navmeshTile.bbTree.QueryClosestXZ(position, constraint, ref distance, previous);
                if ((double) distance < double.PositiveInfinity)
                  break;
              }
              else
                previous = navmeshTile.bbTree.QueryClosest(position, constraint, ref distance, previous);
            }
          }
        }
      }
      previous.node = previous.constrainedNode;
      previous.constrainedNode = (GraphNode) null;
      previous.clampedPosition = previous.constClampedPosition;
      return previous;
    }

    public GraphNode PointOnNavmesh(Vector3 position, NNConstraint constraint)
    {
      if (this.tiles == null)
        return (GraphNode) null;
      Vector3 vector3 = position - this.forcedBounds.min;
      int num1 = Mathf.FloorToInt(vector3.x / (this.cellSize * (float) this.tileSizeX));
      int num2 = Mathf.FloorToInt(vector3.z / (this.cellSize * (float) this.tileSizeZ));
      if (num1 < 0 || num2 < 0 || (num1 >= this.tileXCount || num2 >= this.tileZCount))
        return (GraphNode) null;
      RecastGraph.NavmeshTile navmeshTile = this.tiles[num1 + num2 * this.tileXCount];
      if (navmeshTile != null)
        return (GraphNode) navmeshTile.bbTree.QueryInside(position, constraint);
      return (GraphNode) null;
    }

    public void BuildFunnelCorridor(List<GraphNode> path, int startIndex, int endIndex, List<Vector3> left, List<Vector3> right)
    {
      NavMeshGraph.BuildFunnelCorridor((INavmesh) this, path, startIndex, endIndex, left, right);
    }

    public void AddPortal(GraphNode n1, GraphNode n2, List<Vector3> left, List<Vector3> right)
    {
    }

    public static string GetRecastPath()
    {
      return Application.dataPath + "/Recast/recast";
    }

    public override void ScanInternal(OnScanStatus statusCallback)
    {
      TriangleMeshNode.SetNavmeshHolder(AstarPath.active.astarData.GetGraphIndex((NavGraph) this), (INavmeshHolder) this);
      this.ScanTiledNavmesh(statusCallback);
    }

    protected void ScanTiledNavmesh(OnScanStatus statusCallback)
    {
      this.ScanAllTiles(statusCallback);
    }

    protected void ScanAllTiles(OnScanStatus statusCallback)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      RecastGraph.\u003CScanAllTiles\u003Ec__AnonStorey2C tilesCAnonStorey2C = new RecastGraph.\u003CScanAllTiles\u003Ec__AnonStorey2C();
      int num1 = (int) ((double) this.forcedBounds.size.x / (double) this.cellSize + 0.5);
      int num2 = (int) ((double) this.forcedBounds.size.z / (double) this.cellSize + 0.5);
      if (!this.useTiles)
      {
        this.tileSizeX = num1;
        this.tileSizeZ = num2;
      }
      else
      {
        this.tileSizeX = this.editorTileSize;
        this.tileSizeZ = this.editorTileSize;
      }
      int num3 = (num1 + this.tileSizeX - 1) / this.tileSizeX;
      int num4 = (num2 + this.tileSizeZ - 1) / this.tileSizeZ;
      this.tileXCount = num3;
      this.tileZCount = num4;
      if (this.tileXCount * this.tileZCount > 524288)
        throw new Exception("Too many tiles (" + (object) (this.tileXCount * this.tileZCount) + ") maximum is " + (string) (object) 524288 + "\nTry disabling ASTAR_RECAST_LARGER_TILES under the 'Optimizations' tab in the A* inspector.");
      this.tiles = new RecastGraph.NavmeshTile[this.tileXCount * this.tileZCount];
      if (this.scanEmptyGraph)
      {
        for (int z = 0; z < num4; ++z)
        {
          for (int x = 0; x < num3; ++x)
            this.tiles[z * this.tileXCount + x] = RecastGraph.NewEmptyTile(x, z);
        }
      }
      else
      {
        Console.WriteLine("Collecting Meshes");
        List<ExtraMesh> extraMeshes;
        this.CollectMeshes(out extraMeshes, this.forcedBounds);
        Voxelize vox = new Voxelize(this.cellHeight, this.cellSize, this.walkableClimb, this.walkableHeight, this.maxSlope);
        vox.inputExtraMeshes = extraMeshes;
        vox.maxEdgeLength = this.maxEdgeLength;
        int num5 = -1;
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        for (int z = 0; z < num4; ++z)
        {
          for (int x = 0; x < num3; ++x)
          {
            int num6 = z * this.tileXCount + x;
            Console.WriteLine(string.Concat(new object[4]
            {
              (object) "Generating Tile #",
              (object) num6,
              (object) " of ",
              (object) (num4 * num3)
            }));
            if ((num6 * 10 / this.tiles.Length > num5 || stopwatch.ElapsedMilliseconds > 2000L) && statusCallback != null)
            {
              num5 = num6 * 10 / this.tiles.Length;
              stopwatch.Reset();
              stopwatch.Start();
              statusCallback(new Progress(AstarMath.MapToRange(0.1f, 0.9f, (float) num6 / (float) this.tiles.Length), string.Concat(new object[4]
              {
                (object) "Building Tile ",
                (object) num6,
                (object) "/",
                (object) this.tiles.Length
              })));
            }
            this.BuildTileMesh(vox, x, z);
          }
        }
        Console.WriteLine("Assigning Graph Indices");
        if (statusCallback != null)
          statusCallback(new Progress(0.9f, "Connecting tiles"));
        // ISSUE: reference to a compiler-generated field
        tilesCAnonStorey2C.graphIndex = (uint) AstarPath.active.astarData.GetGraphIndex((NavGraph) this);
        // ISSUE: reference to a compiler-generated method
        this.GetNodes(new GraphNodeDelegateCancelable(tilesCAnonStorey2C.\u003C\u003Em__22));
        for (int index1 = 0; index1 < num4; ++index1)
        {
          for (int index2 = 0; index2 < num3; ++index2)
          {
            Console.WriteLine(string.Concat(new object[4]
            {
              (object) "Connecing Tile #",
              (object) (index1 * this.tileXCount + index2),
              (object) " of ",
              (object) (num4 * num3)
            }));
            if (index2 < num3 - 1)
              this.ConnectTiles(this.tiles[index2 + index1 * this.tileXCount], this.tiles[index2 + 1 + index1 * this.tileXCount]);
            if (index1 < num4 - 1)
              this.ConnectTiles(this.tiles[index2 + index1 * this.tileXCount], this.tiles[index2 + (index1 + 1) * this.tileXCount]);
          }
        }
      }
    }

    protected void BuildTileMesh(Voxelize vox, int x, int z)
    {
      float num1 = (float) this.tileSizeX * this.cellSize;
      float num2 = (float) this.tileSizeZ * this.cellSize;
      int radius = Mathf.CeilToInt(this.characterRadius / this.cellSize);
      Vector3 min = this.forcedBounds.min;
      Vector3 max = this.forcedBounds.max;
      Bounds bounds = new Bounds();
      bounds.SetMinMax(new Vector3((float) x * num1, 0.0f, (float) z * num2) + min, new Vector3((float) (x + 1) * num1 + min.x, max.y, (float) (z + 1) * num2 + min.z));
      vox.borderSize = radius + 3;
      bounds.Expand(new Vector3((float) vox.borderSize, 0.0f, (float) vox.borderSize) * this.cellSize * 2f);
      vox.forcedBounds = bounds;
      vox.width = this.tileSizeX + vox.borderSize * 2;
      vox.depth = this.tileSizeZ + vox.borderSize * 2;
      vox.relevantGraphSurfaceMode = this.useTiles || this.relevantGraphSurfaceMode != RecastGraph.RelevantGraphSurfaceMode.OnlyForCompletelyInsideTile ? this.relevantGraphSurfaceMode : RecastGraph.RelevantGraphSurfaceMode.RequireForAll;
      vox.minRegionSize = Mathf.RoundToInt(this.minRegionSize / (this.cellSize * this.cellSize));
      vox.Init();
      vox.CollectMeshes();
      vox.VoxelizeInput();
      vox.FilterLedges(vox.voxelWalkableHeight, vox.voxelWalkableClimb, vox.cellSize, vox.cellHeight, vox.forcedBounds.min);
      vox.FilterLowHeightSpans(vox.voxelWalkableHeight, vox.cellSize, vox.cellHeight, vox.forcedBounds.min);
      vox.BuildCompactField();
      vox.BuildVoxelConnections();
      vox.ErodeWalkableArea(radius);
      vox.BuildDistanceField();
      vox.BuildRegions();
      VoxelContourSet cset = new VoxelContourSet();
      vox.BuildContours(this.contourMaxError, 1, cset, 1);
      VoxelMesh mesh;
      vox.BuildPolyMesh(cset, 3, out mesh);
      for (int index = 0; index < mesh.verts.Length; ++index)
        mesh.verts[index] = mesh.verts[index] * 1000 * vox.cellScale + (Int3) vox.voxelOffset;
      RecastGraph.NavmeshTile tile = this.CreateTile(vox, mesh, x, z);
      this.tiles[tile.x + tile.z * this.tileXCount] = tile;
    }

    private RecastGraph.NavmeshTile CreateTile(Voxelize vox, VoxelMesh mesh, int x, int z)
    {
      if (mesh.tris == null)
        throw new ArgumentNullException("The mesh must be valid. tris is null.");
      if (mesh.verts == null)
        throw new ArgumentNullException("The mesh must be valid. verts is null.");
      RecastGraph.NavmeshTile navmeshTile = new RecastGraph.NavmeshTile();
      navmeshTile.x = x;
      navmeshTile.z = z;
      navmeshTile.w = 1;
      navmeshTile.d = 1;
      navmeshTile.tris = mesh.tris;
      navmeshTile.verts = mesh.verts;
      navmeshTile.bbTree = new BBTree((INavmeshHolder) navmeshTile);
      if (navmeshTile.tris.Length % 3 != 0)
        throw new ArgumentException("Indices array's length must be a multiple of 3 (mesh.tris)");
      if (navmeshTile.verts.Length >= 4095)
        throw new ArgumentException("Too many vertices per tile (more than " + (object) 4095 + ").\nTry enabling ASTAR_RECAST_LARGER_TILES under the 'Optimizations' tab in the A* Inspector");
      Dictionary<Int3, int> dictionary = this.cachedInt3_int_dict;
      dictionary.Clear();
      int[] numArray = new int[navmeshTile.verts.Length];
      int length1 = 0;
      for (int index = 0; index < navmeshTile.verts.Length; ++index)
      {
        try
        {
          dictionary.Add(navmeshTile.verts[index], length1);
          numArray[index] = length1;
          navmeshTile.verts[length1] = navmeshTile.verts[index];
          ++length1;
        }
        catch
        {
          numArray[index] = dictionary[navmeshTile.verts[index]];
        }
      }
      for (int index = 0; index < navmeshTile.tris.Length; ++index)
        navmeshTile.tris[index] = numArray[navmeshTile.tris[index]];
      Int3[] int3Array = new Int3[length1];
      for (int index = 0; index < length1; ++index)
        int3Array[index] = navmeshTile.verts[index];
      navmeshTile.verts = int3Array;
      TriangleMeshNode[] triangleMeshNodeArray = new TriangleMeshNode[navmeshTile.tris.Length / 3];
      navmeshTile.nodes = triangleMeshNodeArray;
      int length2 = AstarPath.active.astarData.graphs.Length;
      TriangleMeshNode.SetNavmeshHolder(length2, (INavmeshHolder) navmeshTile);
      int num1 = x + z * this.tileXCount << 12;
      for (int index = 0; index < triangleMeshNodeArray.Length; ++index)
      {
        TriangleMeshNode triangleMeshNode = new TriangleMeshNode(this.active);
        triangleMeshNodeArray[index] = triangleMeshNode;
        triangleMeshNode.GraphIndex = (uint) length2;
        triangleMeshNode.v0 = navmeshTile.tris[index * 3] | num1;
        triangleMeshNode.v1 = navmeshTile.tris[index * 3 + 1] | num1;
        triangleMeshNode.v2 = navmeshTile.tris[index * 3 + 2] | num1;
        if (!Polygon.IsClockwise(triangleMeshNode.GetVertex(0), triangleMeshNode.GetVertex(1), triangleMeshNode.GetVertex(2)))
        {
          int num2 = triangleMeshNode.v0;
          triangleMeshNode.v0 = triangleMeshNode.v2;
          triangleMeshNode.v2 = num2;
        }
        triangleMeshNode.Walkable = true;
        triangleMeshNode.Penalty = this.initialPenalty;
        triangleMeshNode.UpdatePositionFromVertices();
        navmeshTile.bbTree.Insert((MeshNode) triangleMeshNode);
      }
      this.CreateNodeConnections(navmeshTile.nodes);
      TriangleMeshNode.SetNavmeshHolder(length2, (INavmeshHolder) null);
      return navmeshTile;
    }

    public void CreateNodeConnections(TriangleMeshNode[] nodes)
    {
      List<MeshNode> list1 = ListPool<MeshNode>.Claim();
      List<uint> list2 = ListPool<uint>.Claim();
      Dictionary<Int2, int> dictionary = this.cachedInt2_int_dict;
      dictionary.Clear();
      for (int index = 0; index < nodes.Length; ++index)
      {
        TriangleMeshNode triangleMeshNode = nodes[index];
        int vertexCount = triangleMeshNode.GetVertexCount();
        for (int i = 0; i < vertexCount; ++i)
        {
          try
          {
            dictionary.Add(new Int2(triangleMeshNode.GetVertexIndex(i), triangleMeshNode.GetVertexIndex((i + 1) % vertexCount)), index);
          }
          catch (Exception ex)
          {
          }
        }
      }
      for (int index1 = 0; index1 < nodes.Length; ++index1)
      {
        TriangleMeshNode triangleMeshNode1 = nodes[index1];
        list1.Clear();
        list2.Clear();
        int vertexCount1 = triangleMeshNode1.GetVertexCount();
        for (int i1 = 0; i1 < vertexCount1; ++i1)
        {
          int vertexIndex1 = triangleMeshNode1.GetVertexIndex(i1);
          int vertexIndex2 = triangleMeshNode1.GetVertexIndex((i1 + 1) % vertexCount1);
          int index2;
          if (dictionary.TryGetValue(new Int2(vertexIndex2, vertexIndex1), out index2))
          {
            TriangleMeshNode triangleMeshNode2 = nodes[index2];
            int vertexCount2 = triangleMeshNode2.GetVertexCount();
            for (int i2 = 0; i2 < vertexCount2; ++i2)
            {
              if (triangleMeshNode2.GetVertexIndex(i2) == vertexIndex2 && triangleMeshNode2.GetVertexIndex((i2 + 1) % vertexCount2) == vertexIndex1)
              {
                uint num = (uint) (triangleMeshNode1.position - triangleMeshNode2.position).costMagnitude;
                list1.Add((MeshNode) triangleMeshNode2);
                list2.Add(num);
                break;
              }
            }
          }
        }
        triangleMeshNode1.connections = (GraphNode[]) list1.ToArray();
        triangleMeshNode1.connectionCosts = list2.ToArray();
      }
      ListPool<MeshNode>.Release(list1);
      ListPool<uint>.Release(list2);
    }

    private void ConnectTiles(RecastGraph.NavmeshTile tile1, RecastGraph.NavmeshTile tile2)
    {
      if (tile1 == null || tile2 == null)
        return;
      if (tile1.nodes == null)
        throw new ArgumentException("tile1 does not contain any nodes");
      if (tile2.nodes == null)
        throw new ArgumentException("tile2 does not contain any nodes");
      int num1 = Mathf.Clamp(tile2.x, tile1.x, tile1.x + tile1.w - 1);
      int num2 = Mathf.Clamp(tile1.x, tile2.x, tile2.x + tile2.w - 1);
      int num3 = Mathf.Clamp(tile2.z, tile1.z, tile1.z + tile1.d - 1);
      int num4 = Mathf.Clamp(tile1.z, tile2.z, tile2.z + tile2.d - 1);
      int index1;
      int index2;
      int val1;
      int val2;
      float num5;
      if (num1 == num2)
      {
        index1 = 2;
        index2 = 0;
        val1 = num3;
        val2 = num4;
        num5 = (float) this.tileSizeZ * this.cellSize;
      }
      else
      {
        if (num3 != num4)
          throw new ArgumentException("Tiles are not adjacent (neither x or z coordinates match)");
        index1 = 0;
        index2 = 2;
        val1 = num1;
        val2 = num2;
        num5 = (float) this.tileSizeX * this.cellSize;
      }
      if (Math.Abs(val1 - val2) != 1)
      {
        UnityEngine.Debug.Log((object) ((string) (object) tile1.x + (object) " " + (string) (object) tile1.z + " " + (string) (object) tile1.w + " " + (string) (object) tile1.d + "\n" + (string) (object) tile2.x + " " + (string) (object) tile2.z + " " + (string) (object) tile2.w + " " + (string) (object) tile2.d + "\n" + (string) (object) num1 + " " + (string) (object) num3 + " " + (string) (object) num2 + " " + (string) (object) num4));
        throw new ArgumentException("Tiles are not adjacent (tile coordinates must differ by exactly 1. Got '" + (object) val1 + "' and '" + (string) (object) val2 + "')");
      }
      int num6 = (int) Math.Round(((double) Math.Max(val1, val2) * (double) num5 + (double) this.forcedBounds.min[index1]) * 1000.0);
      TriangleMeshNode[] triangleMeshNodeArray1 = tile1.nodes;
      TriangleMeshNode[] triangleMeshNodeArray2 = tile2.nodes;
      for (int index3 = 0; index3 < triangleMeshNodeArray1.Length; ++index3)
      {
        TriangleMeshNode triangleMeshNode1 = triangleMeshNodeArray1[index3];
        int vertexCount1 = triangleMeshNode1.GetVertexCount();
        for (int i1 = 0; i1 < vertexCount1; ++i1)
        {
          Int3 vertex1 = triangleMeshNode1.GetVertex(i1);
          Int3 vertex2 = triangleMeshNode1.GetVertex((i1 + 1) % vertexCount1);
          if (Math.Abs(vertex1[index1] - num6) < 2 && Math.Abs(vertex2[index1] - num6) < 2)
          {
            int num7 = Math.Min(vertex1[index2], vertex2[index2]);
            int num8 = Math.Max(vertex1[index2], vertex2[index2]);
            if (num7 != num8)
            {
              for (int index4 = 0; index4 < triangleMeshNodeArray2.Length; ++index4)
              {
                TriangleMeshNode triangleMeshNode2 = triangleMeshNodeArray2[index4];
                int vertexCount2 = triangleMeshNode2.GetVertexCount();
                for (int i2 = 0; i2 < vertexCount2; ++i2)
                {
                  Int3 vertex3 = triangleMeshNode2.GetVertex(i2);
                  Int3 vertex4 = triangleMeshNode2.GetVertex((i2 + 1) % vertexCount1);
                  if (Math.Abs(vertex3[index1] - num6) < 2 && Math.Abs(vertex4[index1] - num6) < 2)
                  {
                    int num9 = Math.Min(vertex3[index2], vertex4[index2]);
                    int num10 = Math.Max(vertex3[index2], vertex4[index2]);
                    if (num9 != num10 && num8 > num9 && num7 < num10 && (vertex1 == vertex3 && vertex2 == vertex4 || vertex1 == vertex4 && vertex2 == vertex3 || (double) Polygon.DistanceSegmentSegment3D((Vector3) vertex1, (Vector3) vertex2, (Vector3) vertex3, (Vector3) vertex4) < (double) this.walkableClimb * (double) this.walkableClimb))
                    {
                      uint cost = (uint) (triangleMeshNode1.position - triangleMeshNode2.position).costMagnitude;
                      triangleMeshNode1.AddConnection((GraphNode) triangleMeshNode2, cost);
                      triangleMeshNode2.AddConnection((GraphNode) triangleMeshNode1, cost);
                    }
                  }
                }
              }
            }
          }
        }
      }
    }

    public void StartBatchTileUpdate()
    {
      if (this.batchTileUpdate)
        throw new InvalidOperationException("Calling StartBatchLoad when batching is already enabled");
      this.batchTileUpdate = true;
    }

    public void EndBatchTileUpdate()
    {
      if (!this.batchTileUpdate)
        throw new InvalidOperationException("Calling EndBatchLoad when batching not enabled");
      this.batchTileUpdate = false;
      int num1 = this.tileXCount;
      int num2 = this.tileZCount;
      for (int index1 = 0; index1 < num2; ++index1)
      {
        for (int index2 = 0; index2 < num1; ++index2)
          this.tiles[index2 + index1 * this.tileXCount].flag = false;
      }
      for (int index = 0; index < this.batchUpdatedTiles.Count; ++index)
        this.tiles[this.batchUpdatedTiles[index]].flag = true;
      for (int index1 = 0; index1 < num2; ++index1)
      {
        for (int index2 = 0; index2 < num1; ++index2)
        {
          if (index2 < num1 - 1 && (this.tiles[index2 + index1 * this.tileXCount].flag || this.tiles[index2 + 1 + index1 * this.tileXCount].flag) && this.tiles[index2 + index1 * this.tileXCount] != this.tiles[index2 + 1 + index1 * this.tileXCount])
            this.ConnectTiles(this.tiles[index2 + index1 * this.tileXCount], this.tiles[index2 + 1 + index1 * this.tileXCount]);
          if (index1 < num2 - 1 && (this.tiles[index2 + index1 * this.tileXCount].flag || this.tiles[index2 + (index1 + 1) * this.tileXCount].flag) && this.tiles[index2 + index1 * this.tileXCount] != this.tiles[index2 + (index1 + 1) * this.tileXCount])
            this.ConnectTiles(this.tiles[index2 + index1 * this.tileXCount], this.tiles[index2 + (index1 + 1) * this.tileXCount]);
        }
      }
      this.batchUpdatedTiles.Clear();
    }

    public void ReplaceTile(int x, int z, Int3[] verts, int[] tris, bool worldSpace)
    {
      this.ReplaceTile(x, z, 1, 1, verts, tris, worldSpace);
    }

    public void ReplaceTile(int x, int z, int w, int d, Int3[] verts, int[] tris, bool worldSpace)
    {
      if (x + w > this.tileXCount || z + d > this.tileZCount || (x < 0 || z < 0))
        throw new ArgumentException("Tile is placed at an out of bounds position or extends out of the graph bounds (" + (object) x + ", " + (string) (object) z + " [" + (string) (object) w + ", " + (string) (object) d + "] " + (string) (object) this.tileXCount + " " + (string) (object) this.tileZCount + ")");
      if (w < 1 || d < 1)
        throw new ArgumentException(string.Concat(new object[4]
        {
          (object) "width and depth must be greater or equal to 1. Was ",
          (object) w,
          (object) ", ",
          (object) d
        }));
      for (int index1 = z; index1 < z + d; ++index1)
      {
        for (int index2 = x; index2 < x + w; ++index2)
        {
          RecastGraph.NavmeshTile tile = this.tiles[index2 + index1 * this.tileXCount];
          if (tile != null)
          {
            this.RemoveConnectionsFromTile(tile);
            for (int index3 = 0; index3 < tile.nodes.Length; ++index3)
              tile.nodes[index3].Destroy();
            for (int z1 = tile.z; z1 < tile.z + tile.d; ++z1)
            {
              for (int x1 = tile.x; x1 < tile.x + tile.w; ++x1)
              {
                RecastGraph.NavmeshTile navmeshTile = this.tiles[x1 + z1 * this.tileXCount];
                if (navmeshTile == null || navmeshTile != tile)
                  throw new Exception("This should not happen");
                if (z1 < z || z1 >= z + d || (x1 < x || x1 >= x + w))
                {
                  this.tiles[x1 + z1 * this.tileXCount] = RecastGraph.NewEmptyTile(x1, z1);
                  if (this.batchTileUpdate)
                    this.batchUpdatedTiles.Add(x1 + z1 * this.tileXCount);
                }
                else
                  this.tiles[x1 + z1 * this.tileXCount] = (RecastGraph.NavmeshTile) null;
              }
            }
          }
        }
      }
      RecastGraph.NavmeshTile tile1 = new RecastGraph.NavmeshTile();
      tile1.x = x;
      tile1.z = z;
      tile1.w = w;
      tile1.d = d;
      tile1.tris = tris;
      tile1.verts = verts;
      tile1.bbTree = new BBTree((INavmeshHolder) tile1);
      if (tile1.tris.Length % 3 != 0)
        throw new ArgumentException("Triangle array's length must be a multiple of 3 (tris)");
      if (tile1.verts.Length > (int) ushort.MaxValue)
        throw new ArgumentException("Too many vertices per tile (more than 65535)");
      if (!worldSpace)
      {
        if (!Mathf.Approximately((float) ((double) (x * this.tileSizeX) * (double) this.cellSize * 1000.0), (float) Math.Round((double) (x * this.tileSizeX) * (double) this.cellSize * 1000.0)))
          UnityEngine.Debug.LogWarning((object) "Possible numerical imprecision. Consider adjusting tileSize and/or cellSize");
        if (!Mathf.Approximately((float) ((double) (z * this.tileSizeZ) * (double) this.cellSize * 1000.0), (float) Math.Round((double) (z * this.tileSizeZ) * (double) this.cellSize * 1000.0)))
          UnityEngine.Debug.LogWarning((object) "Possible numerical imprecision. Consider adjusting tileSize and/or cellSize");
        Int3 int3 = (Int3) (new Vector3((float) (x * this.tileSizeX) * this.cellSize, 0.0f, (float) (z * this.tileSizeZ) * this.cellSize) + this.forcedBounds.min);
        for (int index = 0; index < verts.Length; ++index)
          verts[index] += int3;
      }
      TriangleMeshNode[] triangleMeshNodeArray = new TriangleMeshNode[tile1.tris.Length / 3];
      tile1.nodes = triangleMeshNodeArray;
      int length = AstarPath.active.astarData.graphs.Length;
      TriangleMeshNode.SetNavmeshHolder(length, (INavmeshHolder) tile1);
      int num1 = x + z * this.tileXCount << 12;
      for (int index = 0; index < triangleMeshNodeArray.Length; ++index)
      {
        TriangleMeshNode triangleMeshNode = new TriangleMeshNode(this.active);
        triangleMeshNodeArray[index] = triangleMeshNode;
        triangleMeshNode.GraphIndex = (uint) length;
        triangleMeshNode.v0 = tile1.tris[index * 3] | num1;
        triangleMeshNode.v1 = tile1.tris[index * 3 + 1] | num1;
        triangleMeshNode.v2 = tile1.tris[index * 3 + 2] | num1;
        if (!Polygon.IsClockwise(triangleMeshNode.GetVertex(0), triangleMeshNode.GetVertex(1), triangleMeshNode.GetVertex(2)))
        {
          int num2 = triangleMeshNode.v0;
          triangleMeshNode.v0 = triangleMeshNode.v2;
          triangleMeshNode.v2 = num2;
        }
        triangleMeshNode.Walkable = true;
        triangleMeshNode.Penalty = this.initialPenalty;
        triangleMeshNode.UpdatePositionFromVertices();
        tile1.bbTree.Insert((MeshNode) triangleMeshNode);
      }
      this.CreateNodeConnections(tile1.nodes);
      for (int index1 = z; index1 < z + d; ++index1)
      {
        for (int index2 = x; index2 < x + w; ++index2)
          this.tiles[index2 + index1 * this.tileXCount] = tile1;
      }
      if (this.batchTileUpdate)
        this.batchUpdatedTiles.Add(x + z * this.tileXCount);
      else
        this.ConnectTileWithNeighbours(tile1);
      TriangleMeshNode.SetNavmeshHolder(length, (INavmeshHolder) null);
      int graphIndex = AstarPath.active.astarData.GetGraphIndex((NavGraph) this);
      for (int index = 0; index < triangleMeshNodeArray.Length; ++index)
        triangleMeshNodeArray[index].GraphIndex = (uint) graphIndex;
    }

    protected void ScanCRecast()
    {
      UnityEngine.Debug.LogError((object) "The C++ version of recast can only be used in osx editor or osx standalone mode, I'm sure it cannot be used in the webplayer, but other platforms are not tested yet\nIf you are in the Unity Editor, try switching Platform to OSX Standalone just when scanning, scanned graphs can be cached to enable them to be used in a webplayer.");
    }

    private void CollectTreeMeshes(List<ExtraMesh> extraMeshes, Terrain terrain)
    {
      TerrainData terrainData = terrain.terrainData;
      for (int index = 0; index < terrainData.treeInstances.Length; ++index)
      {
        TreeInstance treeInstance = terrainData.treeInstances[index];
        TreePrototype treePrototype = terrainData.treePrototypes[treeInstance.prototypeIndex];
        if ((UnityEngine.Object) treePrototype.prefab.GetComponent<Collider>() == (UnityEngine.Object) null)
        {
          ExtraMesh extraMesh = new ExtraMesh(this.BoxColliderVerts, this.BoxColliderTris, new Bounds(terrain.transform.position + Vector3.Scale(treeInstance.position, terrainData.size), new Vector3(treeInstance.widthScale, treeInstance.heightScale, treeInstance.widthScale)), Matrix4x4.TRS(terrain.transform.position + Vector3.Scale(treeInstance.position, terrainData.size), Quaternion.identity, new Vector3(treeInstance.widthScale, treeInstance.heightScale, treeInstance.widthScale) * 0.5f));
          extraMeshes.Add(extraMesh);
        }
        else
        {
          Vector3 pos = terrain.transform.position + Vector3.Scale(treeInstance.position, terrainData.size);
          Vector3 s = new Vector3(treeInstance.widthScale, treeInstance.heightScale, treeInstance.widthScale);
          ExtraMesh extraMesh = this.RasterizeCollider(treePrototype.prefab.GetComponent<Collider>(), Matrix4x4.TRS(pos, Quaternion.identity, s));
          if (extraMesh.vertices != null)
          {
            extraMesh.RecalculateBounds();
            extraMeshes.Add(extraMesh);
          }
        }
      }
    }

    private bool CollectMeshes(out List<ExtraMesh> extraMeshes, Bounds bounds)
    {
      List<ExtraMesh> list = new List<ExtraMesh>();
      if (this.rasterizeMeshes)
        list = this.GetSceneMeshes(bounds);
      this.GetRecastMeshObjs(bounds, list);
      Terrain[] terrainArray = UnityEngine.Object.FindObjectsOfType(typeof (Terrain)) as Terrain[];
      if (this.rasterizeTerrain && terrainArray.Length > 0)
      {
        for (int index1 = 0; index1 < terrainArray.Length; ++index1)
        {
          TerrainData terrainData = terrainArray[index1].terrainData;
          if (!((UnityEngine.Object) terrainData == (UnityEngine.Object) null))
          {
            Vector3 position = terrainArray[index1].GetPosition();
            Bounds b = new Bounds(position + terrainData.size * 0.5f, terrainData.size);
            if (b.Intersects(bounds))
            {
              float[,] heights = terrainData.GetHeights(0, 0, terrainData.heightmapWidth, terrainData.heightmapHeight);
              this.terrainSampleSize = this.terrainSampleSize >= 1 ? this.terrainSampleSize : 1;
              int heightmapWidth = terrainData.heightmapWidth;
              int heightmapHeight = terrainData.heightmapHeight;
              int num1 = (terrainData.heightmapWidth + this.terrainSampleSize - 1) / this.terrainSampleSize + 1;
              int num2 = (terrainData.heightmapHeight + this.terrainSampleSize - 1) / this.terrainSampleSize + 1;
              Vector3[] v = new Vector3[num1 * num2];
              Vector3 heightmapScale = terrainData.heightmapScale;
              float num3 = terrainData.size.y;
              int val1_1 = 0;
              for (int index2 = 0; index2 < num2; ++index2)
              {
                int val1_2 = 0;
                for (int index3 = 0; index3 < num1; ++index3)
                {
                  int index4 = Math.Min(val1_2, heightmapWidth - 1);
                  int index5 = Math.Min(val1_1, heightmapHeight - 1);
                  v[index2 * num1 + index3] = new Vector3((float) index5 * heightmapScale.z, heights[index4, index5] * num3, (float) index4 * heightmapScale.x) + position;
                  val1_2 += this.terrainSampleSize;
                }
                val1_1 += this.terrainSampleSize;
              }
              int[] t = new int[(num1 - 1) * (num2 - 1) * 2 * 3];
              int index6 = 0;
              for (int index2 = 0; index2 < num2 - 1; ++index2)
              {
                for (int index3 = 0; index3 < num1 - 1; ++index3)
                {
                  t[index6] = index2 * num1 + index3;
                  t[index6 + 1] = index2 * num1 + index3 + 1;
                  t[index6 + 2] = (index2 + 1) * num1 + index3 + 1;
                  int index4 = index6 + 3;
                  t[index4] = index2 * num1 + index3;
                  t[index4 + 1] = (index2 + 1) * num1 + index3 + 1;
                  t[index4 + 2] = (index2 + 1) * num1 + index3;
                  index6 = index4 + 3;
                }
              }
              list.Add(new ExtraMesh(v, t, b));
              if (this.rasterizeTrees)
                this.CollectTreeMeshes(list, terrainArray[index1]);
            }
          }
        }
      }
      if (this.rasterizeColliders)
      {
        foreach (Collider col in UnityEngine.Object.FindObjectsOfType(typeof (Collider)) as Collider[])
        {
          if ((1 << col.gameObject.layer & (int) this.mask) == 1 << col.gameObject.layer && col.enabled && !col.isTrigger && col.bounds.Intersects(bounds))
          {
            ExtraMesh extraMesh = this.RasterizeCollider(col);
            if (extraMesh.vertices != null)
              list.Add(extraMesh);
          }
        }
        this.capsuleCache.Clear();
      }
      extraMeshes = list;
      if (list.Count != 0)
        return true;
      UnityEngine.Debug.LogWarning((object) "No MeshFilters where found contained in the layers specified by the 'mask' variable");
      return false;
    }

    private ExtraMesh RasterizeCollider(Collider col)
    {
      return this.RasterizeCollider(col, col.transform.localToWorldMatrix);
    }

    private ExtraMesh RasterizeCollider(Collider col, Matrix4x4 localToWorldMatrix)
    {
      if (col is BoxCollider)
      {
        BoxCollider boxCollider = col as BoxCollider;
        Matrix4x4 matrix4x4 = Matrix4x4.TRS(boxCollider.center, Quaternion.identity, boxCollider.size * 0.5f);
        Matrix4x4 matrix = localToWorldMatrix * matrix4x4;
        return new ExtraMesh(this.BoxColliderVerts, this.BoxColliderTris, boxCollider.bounds, matrix);
      }
      if (col is SphereCollider || col is CapsuleCollider)
      {
        SphereCollider sphereCollider = col as SphereCollider;
        CapsuleCollider capsuleCollider = col as CapsuleCollider;
        float num1 = !((UnityEngine.Object) sphereCollider != (UnityEngine.Object) null) ? capsuleCollider.radius : sphereCollider.radius;
        float b = !((UnityEngine.Object) sphereCollider != (UnityEngine.Object) null) ? (float) ((double) capsuleCollider.height * 0.5 / (double) num1 - 1.0) : 0.0f;
        Matrix4x4 matrix4x4 = Matrix4x4.TRS(!((UnityEngine.Object) sphereCollider != (UnityEngine.Object) null) ? capsuleCollider.center : sphereCollider.center, Quaternion.identity, Vector3.one * num1);
        Matrix4x4 matrix = localToWorldMatrix * matrix4x4;
        int num2 = Mathf.Max(4, Mathf.RoundToInt(this.colliderRasterizeDetail * Mathf.Sqrt(matrix.MultiplyVector(Vector3.one).magnitude)));
        if (num2 > 100)
          UnityEngine.Debug.LogWarning((object) "Very large detail for some collider meshes. Consider decreasing Collider Rasterize Detail (RecastGraph)");
        int num3 = num2;
        RecastGraph.CapsuleCache capsuleCache1 = (RecastGraph.CapsuleCache) null;
        for (int index = 0; index < this.capsuleCache.Count; ++index)
        {
          RecastGraph.CapsuleCache capsuleCache2 = this.capsuleCache[index];
          if (capsuleCache2.rows == num2 && Mathf.Approximately(capsuleCache2.height, b))
            capsuleCache1 = capsuleCache2;
        }
        if (capsuleCache1 == null)
        {
          Vector3[] vector3Array = new Vector3[num2 * num3 + 2];
          List<int> list = new List<int>();
          vector3Array[vector3Array.Length - 1] = Vector3.up;
          for (int index1 = 0; index1 < num2; ++index1)
          {
            for (int index2 = 0; index2 < num3; ++index2)
              vector3Array[index2 + index1 * num3] = new Vector3(Mathf.Cos((float) ((double) index2 * 3.14159274101257 * 2.0) / (float) num3) * Mathf.Sin((float) index1 * 3.141593f / (float) (num2 - 1)), Mathf.Cos((float) index1 * 3.141593f / (float) (num2 - 1)) + (index1 >= num2 / 2 ? -b : b), Mathf.Sin((float) ((double) index2 * 3.14159274101257 * 2.0) / (float) num3) * Mathf.Sin((float) index1 * 3.141593f / (float) (num2 - 1)));
          }
          vector3Array[vector3Array.Length - 2] = Vector3.down;
          int num4 = 0;
          int num5 = num3 - 1;
          for (; num4 < num3; num5 = num4++)
          {
            list.Add(vector3Array.Length - 1);
            list.Add(0 * num3 + num5);
            list.Add(0 * num3 + num4);
          }
          for (int index = 1; index < num2; ++index)
          {
            int num6 = 0;
            int num7 = num3 - 1;
            for (; num6 < num3; num7 = num6++)
            {
              list.Add(index * num3 + num6);
              list.Add(index * num3 + num7);
              list.Add((index - 1) * num3 + num6);
              list.Add((index - 1) * num3 + num7);
              list.Add((index - 1) * num3 + num6);
              list.Add(index * num3 + num7);
            }
          }
          int num8 = 0;
          int num9 = num3 - 1;
          for (; num8 < num3; num9 = num8++)
          {
            list.Add(vector3Array.Length - 2);
            list.Add((num2 - 1) * num3 + num9);
            list.Add((num2 - 1) * num3 + num8);
          }
          capsuleCache1 = new RecastGraph.CapsuleCache();
          capsuleCache1.rows = num2;
          capsuleCache1.height = b;
          capsuleCache1.verts = vector3Array;
          capsuleCache1.tris = list.ToArray();
          this.capsuleCache.Add(capsuleCache1);
        }
        return new ExtraMesh(capsuleCache1.verts, capsuleCache1.tris, col.bounds, matrix);
      }
      if (col is MeshCollider)
      {
        MeshCollider meshCollider = col as MeshCollider;
        if ((UnityEngine.Object) meshCollider.sharedMesh != (UnityEngine.Object) null)
          return new ExtraMesh(meshCollider.sharedMesh.vertices, meshCollider.sharedMesh.triangles, meshCollider.bounds, localToWorldMatrix);
      }
      return new ExtraMesh();
    }

    public bool Linecast(Vector3 origin, Vector3 end)
    {
      return this.Linecast(origin, end, this.GetNearest(origin, NNConstraint.None).node);
    }

    public bool Linecast(Vector3 origin, Vector3 end, GraphNode hint, out GraphHitInfo hit)
    {
      return NavMeshGraph.Linecast((INavmesh) this, origin, end, hint, out hit, (List<GraphNode>) null);
    }

    public bool Linecast(Vector3 origin, Vector3 end, GraphNode hint)
    {
      GraphHitInfo hit;
      return NavMeshGraph.Linecast((INavmesh) this, origin, end, hint, out hit, (List<GraphNode>) null);
    }

    public bool Linecast(Vector3 tmp_origin, Vector3 tmp_end, GraphNode hint, out GraphHitInfo hit, List<GraphNode> trace)
    {
      return NavMeshGraph.Linecast((INavmesh) this, tmp_origin, tmp_end, hint, out hit, trace);
    }

    public override void OnDrawGizmos(bool drawNodes)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      RecastGraph.\u003COnDrawGizmos\u003Ec__AnonStorey2D gizmosCAnonStorey2D = new RecastGraph.\u003COnDrawGizmos\u003Ec__AnonStorey2D();
      // ISSUE: reference to a compiler-generated field
      gizmosCAnonStorey2D.\u003C\u003Ef__this = this;
      if (!drawNodes)
        return;
      if (this.bbTree != null)
        this.bbTree.OnDrawGizmos();
      Gizmos.DrawWireCube(this.forcedBounds.center, this.forcedBounds.size);
      // ISSUE: reference to a compiler-generated field
      gizmosCAnonStorey2D.debugData = AstarPath.active.debugPathData;
      // ISSUE: reference to a compiler-generated method
      this.GetNodes(new GraphNodeDelegateCancelable(gizmosCAnonStorey2D.\u003C\u003Em__23));
    }

    public override void SerializeExtraInfo(GraphSerializationContext ctx)
    {
      BinaryWriter binaryWriter = ctx.writer;
      if (this.tiles == null)
      {
        binaryWriter.Write(-1);
      }
      else
      {
        binaryWriter.Write(this.tileXCount);
        binaryWriter.Write(this.tileZCount);
        for (int index1 = 0; index1 < this.tileZCount; ++index1)
        {
          for (int index2 = 0; index2 < this.tileXCount; ++index2)
          {
            RecastGraph.NavmeshTile navmeshTile = this.tiles[index2 + index1 * this.tileXCount];
            if (navmeshTile == null)
              throw new Exception("NULL Tile");
            binaryWriter.Write(navmeshTile.x);
            binaryWriter.Write(navmeshTile.z);
            UnityEngine.Debug.Log((object) ((string) (object) navmeshTile.x + (object) " " + (string) (object) navmeshTile.z + " " + (string) (object) index2 + " " + (string) (object) index1));
            if (navmeshTile.x == index2 && navmeshTile.z == index1)
            {
              binaryWriter.Write(navmeshTile.w);
              binaryWriter.Write(navmeshTile.d);
              binaryWriter.Write(navmeshTile.tris.Length);
              UnityEngine.Debug.Log((object) ("Tris saved " + (object) navmeshTile.tris.Length));
              for (int index3 = 0; index3 < navmeshTile.tris.Length; ++index3)
                binaryWriter.Write(navmeshTile.tris[index3]);
              binaryWriter.Write(navmeshTile.verts.Length);
              for (int index3 = 0; index3 < navmeshTile.verts.Length; ++index3)
              {
                binaryWriter.Write(navmeshTile.verts[index3].x);
                binaryWriter.Write(navmeshTile.verts[index3].y);
                binaryWriter.Write(navmeshTile.verts[index3].z);
              }
              binaryWriter.Write(navmeshTile.nodes.Length);
              for (int index3 = 0; index3 < navmeshTile.nodes.Length; ++index3)
                navmeshTile.nodes[index3].SerializeNode(ctx);
            }
          }
        }
      }
    }

    public override void DeserializeExtraInfo(GraphSerializationContext ctx)
    {
      BinaryReader binaryReader = ctx.reader;
      this.tileXCount = binaryReader.ReadInt32();
      if (this.tileXCount < 0)
        return;
      this.tileZCount = binaryReader.ReadInt32();
      this.tiles = new RecastGraph.NavmeshTile[this.tileXCount * this.tileZCount];
      TriangleMeshNode.SetNavmeshHolder(ctx.graphIndex, (INavmeshHolder) this);
      for (int index1 = 0; index1 < this.tileZCount; ++index1)
      {
        for (int index2 = 0; index2 < this.tileXCount; ++index2)
        {
          int index3 = index2 + index1 * this.tileXCount;
          int num1 = binaryReader.ReadInt32();
          if (num1 < 0)
            throw new Exception("Invalid tile coordinates (x < 0)");
          int num2 = binaryReader.ReadInt32();
          if (num2 < 0)
            throw new Exception("Invalid tile coordinates (z < 0)");
          if (num1 != index2 || num2 != index1)
          {
            this.tiles[index3] = this.tiles[num2 * this.tileXCount + num1];
          }
          else
          {
            RecastGraph.NavmeshTile navmeshTile = new RecastGraph.NavmeshTile();
            navmeshTile.x = num1;
            navmeshTile.z = num2;
            navmeshTile.w = binaryReader.ReadInt32();
            navmeshTile.d = binaryReader.ReadInt32();
            navmeshTile.bbTree = new BBTree((INavmeshHolder) navmeshTile);
            this.tiles[index3] = navmeshTile;
            int length1 = binaryReader.ReadInt32();
            if (length1 % 3 != 0)
              throw new Exception("Corrupt data. Triangle indices count must be divisable by 3. Got " + (object) length1);
            navmeshTile.tris = new int[length1];
            for (int index4 = 0; index4 < navmeshTile.tris.Length; ++index4)
              navmeshTile.tris[index4] = binaryReader.ReadInt32();
            navmeshTile.verts = new Int3[binaryReader.ReadInt32()];
            for (int index4 = 0; index4 < navmeshTile.verts.Length; ++index4)
              navmeshTile.verts[index4] = new Int3(binaryReader.ReadInt32(), binaryReader.ReadInt32(), binaryReader.ReadInt32());
            int length2 = binaryReader.ReadInt32();
            navmeshTile.nodes = new TriangleMeshNode[length2];
            int num3 = index3 << 12;
            for (int index4 = 0; index4 < navmeshTile.nodes.Length; ++index4)
            {
              TriangleMeshNode triangleMeshNode = new TriangleMeshNode(this.active);
              navmeshTile.nodes[index4] = triangleMeshNode;
              triangleMeshNode.GraphIndex = (uint) ctx.graphIndex;
              triangleMeshNode.DeserializeNode(ctx);
              triangleMeshNode.v0 = navmeshTile.tris[index4 * 3] | num3;
              triangleMeshNode.v1 = navmeshTile.tris[index4 * 3 + 1] | num3;
              triangleMeshNode.v2 = navmeshTile.tris[index4 * 3 + 2] | num3;
              triangleMeshNode.UpdatePositionFromVertices();
              navmeshTile.bbTree.Insert((MeshNode) triangleMeshNode);
            }
          }
        }
      }
    }

    public enum RelevantGraphSurfaceMode
    {
      DoNotRequire,
      OnlyForCompletelyInsideTile,
      RequireForAll,
    }

    public class NavmeshTile : INavmesh, INavmeshHolder
    {
      public int[] tris;
      public Int3[] verts;
      public int x;
      public int z;
      public int w;
      public int d;
      public TriangleMeshNode[] nodes;
      public BBTree bbTree;
      public bool flag;

      public void GetTileCoordinates(int tileIndex, out int x, out int z)
      {
        x = this.x;
        z = this.z;
      }

      public int GetVertexArrayIndex(int index)
      {
        return index & 4095;
      }

      public Int3 GetVertex(int index)
      {
        return this.verts[index & 4095];
      }

      public void GetNodes(GraphNodeDelegateCancelable del)
      {
        if (this.nodes == null)
          return;
        int index = 0;
        while (index < this.nodes.Length && del((GraphNode) this.nodes[index]))
          ++index;
      }
    }

    public struct SceneMesh
    {
      public Mesh mesh;
      public Matrix4x4 matrix;
      public Bounds bounds;
    }

    private class CapsuleCache
    {
      public int rows;
      public float height;
      public Vector3[] verts;
      public int[] tris;
    }
  }
}
