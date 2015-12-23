// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.Flag
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Pathfinding;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
  public class Flag
  {
    public static readonly float MIN_SIZE = 32f;
    public static readonly float MAX_SIZE = 1024f;
    public float width;
    public float height;
    private Vector3 _point;
    private Transform _model;
    private MeshFilter navmesh;
    private LineRenderer _area;
    private LineRenderer _bounds;
    private RecastGraph _graph;
    public bool needsNavigationSave;

    public Vector3 point
    {
      get
      {
        return this._point;
      }
    }

    public Transform model
    {
      get
      {
        return this._model;
      }
    }

    public LineRenderer area
    {
      get
      {
        return this._area;
      }
    }

    public LineRenderer bounds
    {
      get
      {
        return this._bounds;
      }
    }

    public RecastGraph graph
    {
      get
      {
        return this._graph;
      }
    }

    public Flag(Vector3 newPoint, RecastGraph newGraph)
    {
      this._point = newPoint;
      this._model = ((GameObject) Object.Instantiate(Resources.Load("Edit/Flag"))).transform;
      this.model.name = "Flag";
      this.model.position = this.point;
      this.model.parent = LevelNavigation.models;
      this._area = this.model.FindChild("Area").GetComponent<LineRenderer>();
      this._bounds = this.model.FindChild("Bounds").GetComponent<LineRenderer>();
      this.navmesh = this.model.FindChild("Navmesh").GetComponent<MeshFilter>();
      this.width = 0.0f;
      this.height = 0.0f;
      this._graph = newGraph;
      this.setupGraph();
      this.buildMesh();
    }

    public Flag(Vector3 newPoint, float newWidth, float newHeight, RecastGraph newGraph)
    {
      this._point = newPoint;
      this._model = ((GameObject) Object.Instantiate(Resources.Load("Edit/Flag"))).transform;
      this.model.name = "Flag";
      this.model.position = this.point;
      this.model.parent = LevelNavigation.models;
      this._area = this.model.FindChild("Area").GetComponent<LineRenderer>();
      this._bounds = this.model.FindChild("Bounds").GetComponent<LineRenderer>();
      this.navmesh = this.model.FindChild("Navmesh").GetComponent<MeshFilter>();
      this.width = newWidth;
      this.height = newHeight;
      this._graph = newGraph;
      this.setupGraph();
      this.buildMesh();
      this.updateNavmesh();
    }

    public void move(Vector3 newPoint)
    {
      this._point = newPoint;
      this.model.position = this.point;
      this.navmesh.transform.position = Vector3.zero;
    }

    public void setEnabled(bool isEnabled)
    {
      this.model.gameObject.SetActive(isEnabled);
    }

    public void buildMesh()
    {
      float num1 = Flag.MIN_SIZE + this.width * (Flag.MAX_SIZE - Flag.MIN_SIZE);
      float num2 = Flag.MIN_SIZE + this.height * (Flag.MAX_SIZE - Flag.MIN_SIZE);
      this.area.SetPosition(0, new Vector3((float) (-(double) num1 / 2.0), 0.0f, (float) (-(double) num2 / 2.0)));
      this.area.SetPosition(1, new Vector3(num1 / 2f, 0.0f, (float) (-(double) num2 / 2.0)));
      this.area.SetPosition(2, new Vector3(num1 / 2f, 0.0f, num2 / 2f));
      this.area.SetPosition(3, new Vector3((float) (-(double) num1 / 2.0), 0.0f, num2 / 2f));
      this.area.SetPosition(4, new Vector3((float) (-(double) num1 / 2.0), 0.0f, (float) (-(double) num2 / 2.0)));
      float num3 = num1 + LevelNavigation.BOUNDS_SIZE.x;
      float num4 = num2 + LevelNavigation.BOUNDS_SIZE.z;
      this.bounds.SetPosition(0, new Vector3((float) (-(double) num3 / 2.0), 0.0f, (float) (-(double) num4 / 2.0)));
      this.bounds.SetPosition(1, new Vector3(num3 / 2f, 0.0f, (float) (-(double) num4 / 2.0)));
      this.bounds.SetPosition(2, new Vector3(num3 / 2f, 0.0f, num4 / 2f));
      this.bounds.SetPosition(3, new Vector3((float) (-(double) num3 / 2.0), 0.0f, num4 / 2f));
      this.bounds.SetPosition(4, new Vector3((float) (-(double) num3 / 2.0), 0.0f, (float) (-(double) num4 / 2.0)));
    }

    public void remove()
    {
      AstarPath.active.astarData.RemoveGraph((NavGraph) this.graph);
      Object.Destroy((Object) this.model.gameObject);
    }

    public void bakeNavigation()
    {
      float x = Flag.MIN_SIZE + this.width * (Flag.MAX_SIZE - Flag.MIN_SIZE);
      float z = Flag.MIN_SIZE + this.height * (Flag.MAX_SIZE - Flag.MIN_SIZE);
      if ((double) LevelLighting.seaLevel < 0.990000009536743)
      {
        this.graph.forcedBoundsCenter = new Vector3(this.point.x, (float) ((double) LevelLighting.seaLevel * (double) Level.TERRAIN + ((double) Level.TERRAIN - (double) LevelLighting.seaLevel * (double) Level.TERRAIN) / 2.0 - 0.625), this.point.z);
        this.graph.forcedBoundsSize = new Vector3(x, (float) ((double) Level.TERRAIN - (double) LevelLighting.seaLevel * (double) Level.TERRAIN + 1.25), z);
      }
      else
      {
        this.graph.forcedBoundsCenter = new Vector3(this.point.x, Level.TERRAIN / 2f, this.point.z);
        this.graph.forcedBoundsSize = new Vector3(x, Level.TERRAIN, z);
      }
      LevelGround.models2.gameObject.SetActive(false);
      AstarPath.active.ScanSpecific((NavGraph) this.graph);
      LevelNavigation.updateBounds();
      LevelGround.models2.gameObject.SetActive(true);
    }

    private void updateNavmesh()
    {
      if (!Level.isEditor || this.graph == null)
        return;
      List<Vector3> list1 = new List<Vector3>();
      List<int> list2 = new List<int>();
      List<Vector2> list3 = new List<Vector2>();
      RecastGraph.NavmeshTile[] tiles = this.graph.GetTiles();
      int num = 0;
      if (tiles == null)
        return;
      for (int index1 = 0; index1 < tiles.Length; ++index1)
      {
        RecastGraph.NavmeshTile navmeshTile = tiles[index1];
        for (int index2 = 0; index2 < navmeshTile.verts.Length; ++index2)
        {
          Vector3 vector3 = (Vector3) navmeshTile.verts[index2];
          vector3.y += 0.1f;
          list1.Add(vector3);
          list3.Add(new Vector2(vector3.x, vector3.z));
        }
        for (int index2 = 0; index2 < navmeshTile.tris.Length; ++index2)
          list2.Add(navmeshTile.tris[index2] + num);
        num += navmeshTile.verts.Length;
      }
      Mesh mesh = new Mesh();
      mesh.name = "Navmesh";
      mesh.vertices = list1.ToArray();
      mesh.triangles = list2.ToArray();
      mesh.normals = new Vector3[list1.Count];
      mesh.uv = list3.ToArray();
      this.navmesh.transform.position = Vector3.zero;
      this.navmesh.mesh = mesh;
    }

    private void OnGraphPostScan(NavGraph updated)
    {
      if (updated != this.graph)
        return;
      this.needsNavigationSave = true;
      this.updateNavmesh();
    }

    private void setupGraph()
    {
      AstarPath.OnGraphPostScan += new OnGraphDelegate(this.OnGraphPostScan);
    }
  }
}
