// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.Road
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace SDG.Unturned
{
  public class Road
  {
    public byte material;
    private Transform road;
    private Transform line;
    private LineRenderer renderer;
    private List<Vector3> _points;
    private List<Transform> _paths;

    public List<Vector3> points
    {
      get
      {
        return this._points;
      }
    }

    public List<Transform> paths
    {
      get
      {
        return this._paths;
      }
    }

    public Road(byte newMaterial)
    {
      this.material = newMaterial;
      this.road = new GameObject().transform;
      this.road.name = "Road";
      this.road.parent = LevelRoads.models;
      this.road.tag = "Environment";
      this.road.gameObject.layer = LayerMasks.ENVIRONMENT;
      this.road.gameObject.AddComponent<MeshCollider>();
      if (!Dedicator.isDedicated)
      {
        this.road.gameObject.AddComponent<MeshFilter>();
        this.road.gameObject.AddComponent<MeshRenderer>().reflectionProbeUsage = ReflectionProbeUsage.Off;
        this.road.gameObject.AddComponent<LODGroup>();
        this.road.GetComponent<LODGroup>().SetLODs(new LOD[1]
        {
          new LOD(0.75f, new Renderer[1]
          {
            (Renderer) this.road.GetComponent<MeshRenderer>()
          })
        });
      }
      this._points = new List<Vector3>();
      this._paths = new List<Transform>();
      if (!Level.isEditor)
        return;
      this.line = ((GameObject) Object.Instantiate(Resources.Load("Edit/Road"))).transform;
      this.line.name = "Line";
      this.line.parent = LevelRoads.models;
      this.renderer = this.line.GetComponent<LineRenderer>();
    }

    public Road(byte newMaterial, List<Vector3> newPoints)
    {
      this.material = newMaterial;
      this.road = new GameObject().transform;
      this.road.name = "Road";
      this.road.parent = LevelRoads.models;
      this.road.tag = "Environment";
      this.road.gameObject.layer = LayerMasks.ENVIRONMENT;
      this.road.gameObject.AddComponent<MeshCollider>();
      if (!Dedicator.isDedicated)
      {
        this.road.gameObject.AddComponent<MeshFilter>();
        this.road.gameObject.AddComponent<MeshRenderer>().reflectionProbeUsage = ReflectionProbeUsage.Off;
        this.road.gameObject.AddComponent<LODGroup>();
        this.road.GetComponent<LODGroup>().SetLODs(new LOD[1]
        {
          new LOD(0.5f, new Renderer[1]
          {
            (Renderer) this.road.GetComponent<MeshRenderer>()
          })
        });
      }
      if (LevelRoads.materials[(int) this.material].isConcrete)
        this.road.GetComponent<Collider>().material = (PhysicMaterial) Resources.Load("Physics/Concrete_Static");
      else
        this.road.GetComponent<Collider>().material = (PhysicMaterial) Resources.Load("Physics/Gravel_Static");
      this._points = newPoints;
      this._paths = new List<Transform>();
      if (!Level.isEditor)
        return;
      this.line = ((GameObject) Object.Instantiate(Resources.Load("Edit/Road"))).transform;
      this.line.name = "Line";
      this.line.parent = LevelRoads.models;
      this.renderer = this.line.GetComponent<LineRenderer>();
      for (int index = 0; index < this.points.Count; ++index)
      {
        Transform transform = ((GameObject) Object.Instantiate(Resources.Load("Edit/Path"))).transform;
        transform.name = "Path_" + (object) index;
        transform.parent = this.line;
        this.paths.Add(transform);
      }
      this.updatePoints();
    }

    public void setEnabled(bool isEnabled)
    {
      this.line.gameObject.SetActive(isEnabled);
      for (int index = 0; index < this.paths.Count; ++index)
        this.paths[index].gameObject.SetActive(isEnabled);
    }

    public Transform addPoint(Transform origin, Vector3 point)
    {
      if ((Object) origin == (Object) null || (Object) origin == (Object) this.paths[this.paths.Count - 1])
      {
        this.points.Add(point);
        Transform transform = ((GameObject) Object.Instantiate(Resources.Load("Edit/Path"))).transform;
        transform.name = "Path_" + (object) (this.points.Count - 1);
        transform.parent = this.line;
        this.paths.Add(transform);
        this.updatePoints();
        return transform;
      }
      if (!((Object) origin == (Object) this.paths[0]))
        return (Transform) null;
      for (int index = 0; index < this.points.Count; ++index)
        this.paths[index].name = "Path_" + (object) (index + 1);
      this.points.Insert(0, point);
      Transform transform1 = ((GameObject) Object.Instantiate(Resources.Load("Edit/Path"))).transform;
      transform1.name = "Path_0";
      transform1.parent = this.line;
      this.paths.Insert(0, transform1);
      this.updatePoints();
      return transform1;
    }

    public void removePoint(Transform select)
    {
      for (int index1 = 0; index1 < this.paths.Count; ++index1)
      {
        if ((Object) this.paths[index1] == (Object) select)
        {
          for (int index2 = index1 + 1; index2 < this.paths.Count; ++index2)
            this.paths[index2].name = "Path_" + (object) (index2 - 1);
          Object.Destroy((Object) select.gameObject);
          this.points.RemoveAt(index1);
          this.paths.RemoveAt(index1);
          this.updatePoints();
          break;
        }
      }
    }

    public void remove()
    {
      Object.Destroy((Object) this.road.gameObject);
      Object.Destroy((Object) this.line.gameObject);
    }

    public void movePoint(Transform select, Vector3 point)
    {
      for (int index = 0; index < this.paths.Count; ++index)
      {
        if ((Object) this.paths[index] == (Object) select)
        {
          this.points[index] = point;
          this.updatePoints();
          break;
        }
      }
    }

    public void splitPoint(Transform select)
    {
      for (int index1 = 0; index1 < this.paths.Count; ++index1)
      {
        if ((Object) this.paths[index1] == (Object) select)
        {
          if (index1 <= 0 || index1 >= this.paths.Count - 1)
            break;
          Vector3 position = select.position;
          Vector3 b1 = this.points[index1 - 1];
          Vector3 b2 = this.points[index1 + 1];
          for (int index2 = index1; index2 < this.paths.Count; ++index2)
            this.paths[index2].name = "Path_" + (object) (index2 + 1);
          this.points.Insert(index1, position);
          Transform transform = ((GameObject) Object.Instantiate(Resources.Load("Edit/Path"))).transform;
          transform.name = "Path_" + (object) index1;
          transform.parent = this.line;
          this.paths.Insert(index1, transform);
          Vector3 point1 = Vector3.Lerp(position, b1, 0.33f);
          point1.y = LevelGround.getHeight(point1);
          Vector3 point2 = Vector3.Lerp(position, b2, 0.33f);
          point2.y = LevelGround.getHeight(point2);
          this.points[index1] = point1;
          this.points[index1 + 1] = point2;
          this.updatePoints();
          break;
        }
      }
    }

    public void settlePoints()
    {
      for (int index = 0; index < this.points.Count; ++index)
      {
        Vector3 point = this.points[index];
        point.y = LevelGround.getHeight(point);
        this.points[index] = point;
      }
      this.updatePoints();
    }

    public void buildMesh()
    {
      if (this.points.Count > 1)
      {
        Mesh mesh = new Mesh();
        mesh.name = "Road";
        Vector3[] vector3Array1 = new Vector3[this.points.Count * 4 + 8];
        Vector3[] vector3Array2 = new Vector3[this.points.Count * 4 + 8];
        Vector2[] vector2Array = new Vector2[this.points.Count * 4 + 8];
        int[] numArray = new int[this.points.Count * 18 + 36];
        float num = 0.0f;
        Vector3 vector3_1;
        for (int index = 0; index < this.points.Count; ++index)
        {
          vector3_1 = index >= this.points.Count - 1 ? this.points[index] - this.points[index - 1] : this.points[index + 1] - this.points[index];
          Vector3 normal = LevelGround.getNormal(this.points[index]);
          Vector3 vector3_2 = Vector3.Cross(vector3_1.normalized, normal);
          vector3Array1[4 + index * 4] = this.points[index] + vector3_2 * (LevelRoads.materials[(int) this.material].width + LevelRoads.materials[(int) this.material].depth * 2f) - normal * LevelRoads.materials[(int) this.material].depth;
          vector3Array1[4 + index * 4 + 1] = this.points[index] + vector3_2 * LevelRoads.materials[(int) this.material].width + normal * LevelRoads.materials[(int) this.material].depth;
          vector3Array1[4 + index * 4 + 2] = this.points[index] - vector3_2 * LevelRoads.materials[(int) this.material].width + normal * LevelRoads.materials[(int) this.material].depth;
          vector3Array1[4 + index * 4 + 3] = this.points[index] - vector3_2 * (LevelRoads.materials[(int) this.material].width + LevelRoads.materials[(int) this.material].depth * 2f) - normal * LevelRoads.materials[(int) this.material].depth;
          vector3Array2[4 + index * 4] = vector3_2;
          vector3Array2[4 + index * 4 + 1] = normal;
          vector3Array2[4 + index * 4 + 2] = normal;
          vector3Array2[4 + index * 4 + 3] = -vector3_2;
          if (index == 0)
          {
            vector2Array[4 + index * 4] = Vector2.zero;
            vector2Array[4 + index * 4 + 1] = Vector2.zero;
            vector2Array[4 + index * 4 + 2] = Vector2.right;
            vector2Array[4 + index * 4 + 3] = Vector2.right;
          }
          else
          {
            num += (this.points[index] - this.points[index - 1]).magnitude;
            Vector2 vector2 = Vector2.up * num / (float) LevelRoads.materials[(int) this.material].material.mainTexture.height * LevelRoads.materials[(int) this.material].height;
            vector2Array[4 + index * 4] = Vector2.zero + vector2;
            vector2Array[4 + index * 4 + 1] = Vector2.zero + vector2;
            vector2Array[4 + index * 4 + 2] = Vector2.right + vector2;
            vector2Array[4 + index * 4 + 3] = Vector2.right + vector2;
          }
        }
        int index1 = this.points.Count - 1;
        vector3_1 = (this.points[index1] - this.points[index1 - 1]).normalized;
        Vector3 normal1 = LevelGround.getNormal(this.points[index1]);
        Vector3 vector3_3 = Vector3.Cross(vector3_1.normalized, normal1);
        vector3Array1[8 + index1 * 4] = this.points[index1] + vector3_3 * (LevelRoads.materials[(int) this.material].width + LevelRoads.materials[(int) this.material].depth * 2f) - normal1 * LevelRoads.materials[(int) this.material].depth + vector3_1 * LevelRoads.materials[(int) this.material].depth * 4f;
        vector3Array1[8 + index1 * 4 + 1] = this.points[index1] + vector3_3 * LevelRoads.materials[(int) this.material].width - normal1 * LevelRoads.materials[(int) this.material].depth + vector3_1 * LevelRoads.materials[(int) this.material].depth * 4f;
        vector3Array1[8 + index1 * 4 + 2] = this.points[index1] - vector3_3 * LevelRoads.materials[(int) this.material].width - normal1 * LevelRoads.materials[(int) this.material].depth + vector3_1 * LevelRoads.materials[(int) this.material].depth * 4f;
        vector3Array1[8 + index1 * 4 + 3] = this.points[index1] - vector3_3 * (LevelRoads.materials[(int) this.material].width + LevelRoads.materials[(int) this.material].depth * 2f) - normal1 * LevelRoads.materials[(int) this.material].depth + vector3_1 * LevelRoads.materials[(int) this.material].depth * 4f;
        vector3Array2[8 + index1 * 4] = vector3_1;
        vector3Array2[8 + index1 * 4 + 1] = vector3_1;
        vector3Array2[8 + index1 * 4 + 2] = vector3_1;
        vector3Array2[8 + index1 * 4 + 3] = vector3_1;
        Vector2 vector2_1 = Vector2.up * num / (float) LevelRoads.materials[(int) this.material].material.mainTexture.height * LevelRoads.materials[(int) this.material].height;
        vector2Array[8 + index1 * 4] = Vector2.zero + vector2_1;
        vector2Array[8 + index1 * 4 + 1] = Vector2.zero + vector2_1;
        vector2Array[8 + index1 * 4 + 2] = Vector2.right + vector2_1;
        vector2Array[8 + index1 * 4 + 3] = Vector2.right + vector2_1;
        int index2 = 0;
        vector3_1 = (this.points[index2 + 1] - this.points[index2]).normalized;
        Vector3 normal2 = LevelGround.getNormal(this.points[index2]);
        Vector3 vector3_4 = Vector3.Cross(vector3_1.normalized, normal2);
        vector3Array1[index2 * 4] = this.points[index2] + vector3_4 * (LevelRoads.materials[(int) this.material].width + LevelRoads.materials[(int) this.material].depth * 2f) - normal2 * LevelRoads.materials[(int) this.material].depth - vector3_1 * LevelRoads.materials[(int) this.material].depth * 4f;
        vector3Array1[index2 * 4 + 1] = this.points[index2] + vector3_4 * LevelRoads.materials[(int) this.material].width - normal2 * LevelRoads.materials[(int) this.material].depth - vector3_1 * LevelRoads.materials[(int) this.material].depth * 4f;
        vector3Array1[index2 * 4 + 2] = this.points[index2] - vector3_4 * LevelRoads.materials[(int) this.material].width - normal2 * LevelRoads.materials[(int) this.material].depth - vector3_1 * LevelRoads.materials[(int) this.material].depth * 4f;
        vector3Array1[index2 * 4 + 3] = this.points[index2] - vector3_4 * (LevelRoads.materials[(int) this.material].width + LevelRoads.materials[(int) this.material].depth * 2f) - normal2 * LevelRoads.materials[(int) this.material].depth - vector3_1 * LevelRoads.materials[(int) this.material].depth * 4f;
        vector3Array2[index2 * 4] = -vector3_1;
        vector3Array2[index2 * 4 + 1] = -vector3_1;
        vector3Array2[index2 * 4 + 2] = -vector3_1;
        vector3Array2[index2 * 4 + 3] = -vector3_1;
        vector2Array[index2 * 4] = Vector2.zero;
        vector2Array[index2 * 4 + 1] = Vector2.zero;
        vector2Array[index2 * 4 + 2] = Vector2.right;
        vector2Array[index2 * 4 + 3] = Vector2.right;
        for (int index3 = 0; index3 < this.points.Count + 1; ++index3)
        {
          numArray[index3 * 18] = index3 * 4 + 5;
          numArray[index3 * 18 + 1] = index3 * 4 + 1;
          numArray[index3 * 18 + 2] = index3 * 4 + 4;
          numArray[index3 * 18 + 3] = index3 * 4;
          numArray[index3 * 18 + 4] = index3 * 4 + 4;
          numArray[index3 * 18 + 5] = index3 * 4 + 1;
          numArray[index3 * 18 + 6] = index3 * 4 + 6;
          numArray[index3 * 18 + 7] = index3 * 4 + 2;
          numArray[index3 * 18 + 8] = index3 * 4 + 5;
          numArray[index3 * 18 + 9] = index3 * 4 + 1;
          numArray[index3 * 18 + 10] = index3 * 4 + 5;
          numArray[index3 * 18 + 11] = index3 * 4 + 2;
          numArray[index3 * 18 + 12] = index3 * 4 + 7;
          numArray[index3 * 18 + 13] = index3 * 4 + 3;
          numArray[index3 * 18 + 14] = index3 * 4 + 6;
          numArray[index3 * 18 + 15] = index3 * 4 + 2;
          numArray[index3 * 18 + 16] = index3 * 4 + 6;
          numArray[index3 * 18 + 17] = index3 * 4 + 3;
        }
        mesh.vertices = vector3Array1;
        mesh.normals = vector3Array2;
        mesh.uv = vector2Array;
        mesh.triangles = numArray;
        mesh.Optimize();
        this.road.GetComponent<MeshCollider>().sharedMesh = mesh;
        if (Dedicator.isDedicated)
          return;
        this.road.GetComponent<MeshFilter>().sharedMesh = mesh;
        this.road.GetComponent<Renderer>().material = LevelRoads.materials[(int) this.material].material;
        this.road.GetComponent<LODGroup>().RecalculateBounds();
      }
      else
      {
        this.road.GetComponent<MeshCollider>().sharedMesh = (Mesh) null;
        if (Dedicator.isDedicated)
          return;
        this.road.GetComponent<MeshFilter>().sharedMesh = (Mesh) null;
      }
    }

    private void updatePoints()
    {
      this.renderer.SetVertexCount(this.points.Count);
      for (int index = 0; index < this.points.Count; ++index)
      {
        this.renderer.SetPosition(index, this.points[index]);
        this.line.FindChild("Path_" + (object) index).position = this.points[index];
      }
    }
  }
}
