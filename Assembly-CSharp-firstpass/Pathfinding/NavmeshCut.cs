// Decompiled with JetBrains decompiler
// Type: Pathfinding.NavmeshCut
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding.ClipperLib;
using Pathfinding.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
  [AddComponentMenu("Pathfinding/Navmesh/Navmesh Cut")]
  public class NavmeshCut : MonoBehaviour
  {
    private static List<NavmeshCut> allCuts = new List<NavmeshCut>();
    private static readonly Dictionary<Int2, int> edges = new Dictionary<Int2, int>();
    private static readonly Dictionary<int, int> pointers = new Dictionary<int, int>();
    public static readonly Color GizmoColor = new Color(0.145098f, 0.7215686f, 0.9372549f);
    public Vector2 rectangleSize = new Vector2(1f, 1f);
    public float circleRadius = 1f;
    public int circleResolution = 6;
    public float height = 1f;
    public float meshScale = 1f;
    public float updateDistance = 0.4f;
    public bool cutsAddedGeom = true;
    public float updateRotationDistance = 10f;
    public NavmeshCut.MeshType type;
    public Mesh mesh;
    public Vector3 center;
    public bool isDual;
    public bool useRotation;
    private Vector3[][] contours;
    protected Transform tr;
    private Mesh lastMesh;
    private Vector3 lastPosition;
    private Quaternion lastRotation;
    private bool wasEnabled;
    private Bounds bounds;
    private Bounds lastBounds;

    public Bounds LastBounds
    {
      get
      {
        return this.lastBounds;
      }
    }

    public static event Action<NavmeshCut> OnDestroyCallback;

    private static void AddCut(NavmeshCut obj)
    {
      NavmeshCut.allCuts.Add(obj);
    }

    private static void RemoveCut(NavmeshCut obj)
    {
      NavmeshCut.allCuts.Remove(obj);
    }

    public static List<NavmeshCut> GetAllInRange(Bounds b)
    {
      List<NavmeshCut> list = ListPool<NavmeshCut>.Claim();
      for (int index = 0; index < NavmeshCut.allCuts.Count; ++index)
      {
        if (NavmeshCut.allCuts[index].enabled && NavmeshCut.Intersects(b, NavmeshCut.allCuts[index].GetBounds()))
          list.Add(NavmeshCut.allCuts[index]);
      }
      return list;
    }

    private static bool Intersects(Bounds b1, Bounds b2)
    {
      Vector3 min1 = b1.min;
      Vector3 max1 = b1.max;
      Vector3 min2 = b2.min;
      Vector3 max2 = b2.max;
      if ((double) min1.x <= (double) max2.x && (double) max1.x >= (double) min2.x && ((double) min1.y <= (double) max2.y && (double) max1.y >= (double) min2.y) && (double) min1.z <= (double) max2.z)
        return (double) max1.z >= (double) min2.z;
      return false;
    }

    public static List<NavmeshCut> GetAll()
    {
      return NavmeshCut.allCuts;
    }

    public void Awake()
    {
      NavmeshCut.AddCut(this);
    }

    public void OnEnable()
    {
      this.tr = this.transform;
      this.lastPosition = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
      this.lastRotation = this.tr.rotation;
    }

    public void OnDestroy()
    {
      if (NavmeshCut.OnDestroyCallback != null)
        NavmeshCut.OnDestroyCallback(this);
      NavmeshCut.RemoveCut(this);
    }

    public void ForceUpdate()
    {
      this.lastPosition = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
    }

    public bool RequiresUpdate()
    {
      if (this.wasEnabled != this.enabled)
        return true;
      if (!this.wasEnabled)
        return false;
      if ((double) (this.tr.position - this.lastPosition).sqrMagnitude > (double) this.updateDistance * (double) this.updateDistance)
        return true;
      if (this.useRotation)
        return (double) Quaternion.Angle(this.lastRotation, this.tr.rotation) > (double) this.updateRotationDistance;
      return false;
    }

    public virtual void UsedForCut()
    {
    }

    public void NotifyUpdated()
    {
      this.wasEnabled = this.enabled;
      if (!this.wasEnabled)
        return;
      this.lastPosition = this.tr.position;
      this.lastBounds = this.GetBounds();
      if (!this.useRotation)
        return;
      this.lastRotation = this.tr.rotation;
    }

    private void CalculateMeshContour()
    {
      if ((UnityEngine.Object) this.mesh == (UnityEngine.Object) null)
        return;
      NavmeshCut.edges.Clear();
      NavmeshCut.pointers.Clear();
      Vector3[] vertices = this.mesh.vertices;
      int[] triangles = this.mesh.triangles;
      int index1 = 0;
      while (index1 < triangles.Length)
      {
        if (Polygon.IsClockwise(vertices[triangles[index1]], vertices[triangles[index1 + 1]], vertices[triangles[index1 + 2]]))
        {
          int num = triangles[index1];
          triangles[index1] = triangles[index1 + 2];
          triangles[index1 + 2] = num;
        }
        NavmeshCut.edges[new Int2(triangles[index1], triangles[index1 + 1])] = index1;
        NavmeshCut.edges[new Int2(triangles[index1 + 1], triangles[index1 + 2])] = index1;
        NavmeshCut.edges[new Int2(triangles[index1 + 2], triangles[index1])] = index1;
        index1 += 3;
      }
      int num1 = 0;
      while (num1 < triangles.Length)
      {
        for (int index2 = 0; index2 < 3; ++index2)
        {
          if (!NavmeshCut.edges.ContainsKey(new Int2(triangles[num1 + (index2 + 1) % 3], triangles[num1 + index2 % 3])))
            NavmeshCut.pointers[triangles[num1 + index2 % 3]] = triangles[num1 + (index2 + 1) % 3];
        }
        num1 += 3;
      }
      List<Vector3[]> list1 = new List<Vector3[]>();
      List<Vector3> list2 = ListPool<Vector3>.Claim();
      for (int key = 0; key < vertices.Length; ++key)
      {
        if (NavmeshCut.pointers.ContainsKey(key))
        {
          list2.Clear();
          int index2 = key;
          do
          {
            int num2 = NavmeshCut.pointers[index2];
            if (num2 != -1)
            {
              NavmeshCut.pointers[index2] = -1;
              list2.Add(vertices[index2]);
              index2 = num2;
              if (index2 == -1)
              {
                Debug.LogError((object) ("Invalid Mesh '" + this.mesh.name + " in " + this.gameObject.name));
                break;
              }
            }
            else
              break;
          }
          while (index2 != key);
          if (list2.Count > 0)
            list1.Add(list2.ToArray());
        }
      }
      ListPool<Vector3>.Release(list2);
      this.contours = list1.ToArray();
    }

    public Bounds GetBounds()
    {
      switch (this.type)
      {
        case NavmeshCut.MeshType.Rectangle:
          if (this.useRotation)
          {
            Matrix4x4 localToWorldMatrix = this.tr.localToWorldMatrix;
            this.bounds = new Bounds(localToWorldMatrix.MultiplyPoint3x4(this.center + new Vector3(-this.rectangleSize.x, -this.height, -this.rectangleSize.y) * 0.5f), Vector3.zero);
            this.bounds.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(this.center + new Vector3(this.rectangleSize.x, -this.height, -this.rectangleSize.y) * 0.5f));
            this.bounds.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(this.center + new Vector3(this.rectangleSize.x, -this.height, this.rectangleSize.y) * 0.5f));
            this.bounds.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(this.center + new Vector3(-this.rectangleSize.x, -this.height, this.rectangleSize.y) * 0.5f));
            this.bounds.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(this.center + new Vector3(-this.rectangleSize.x, this.height, -this.rectangleSize.y) * 0.5f));
            this.bounds.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(this.center + new Vector3(this.rectangleSize.x, this.height, -this.rectangleSize.y) * 0.5f));
            this.bounds.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(this.center + new Vector3(this.rectangleSize.x, this.height, this.rectangleSize.y) * 0.5f));
            this.bounds.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(this.center + new Vector3(-this.rectangleSize.x, this.height, this.rectangleSize.y) * 0.5f));
            break;
          }
          this.bounds = new Bounds(this.tr.position + this.center, new Vector3(this.rectangleSize.x, this.height, this.rectangleSize.y));
          break;
        case NavmeshCut.MeshType.Circle:
          this.bounds = !this.useRotation ? new Bounds(this.transform.position + this.center, new Vector3(this.circleRadius * 2f, this.height, this.circleRadius * 2f)) : new Bounds(this.tr.localToWorldMatrix.MultiplyPoint3x4(this.center), new Vector3(this.circleRadius * 2f, this.height, this.circleRadius * 2f));
          break;
        case NavmeshCut.MeshType.CustomMesh:
          if (!((UnityEngine.Object) this.mesh == (UnityEngine.Object) null))
          {
            Bounds bounds = this.mesh.bounds;
            if (this.useRotation)
            {
              Matrix4x4 localToWorldMatrix = this.tr.localToWorldMatrix;
              bounds.center *= this.meshScale;
              bounds.size *= this.meshScale;
              this.bounds = new Bounds(localToWorldMatrix.MultiplyPoint3x4(this.center + bounds.center), Vector3.zero);
              Vector3 max = bounds.max;
              Vector3 min = bounds.min;
              this.bounds.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(this.center + new Vector3(max.x, max.y, max.z)));
              this.bounds.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(this.center + new Vector3(min.x, max.y, max.z)));
              this.bounds.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(this.center + new Vector3(min.x, max.y, min.z)));
              this.bounds.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(this.center + new Vector3(max.x, max.y, min.z)));
              this.bounds.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(this.center + new Vector3(max.x, min.y, max.z)));
              this.bounds.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(this.center + new Vector3(min.x, min.y, max.z)));
              this.bounds.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(this.center + new Vector3(min.x, min.y, min.z)));
              this.bounds.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(this.center + new Vector3(max.x, min.y, min.z)));
              Vector3 size = this.bounds.size;
              size.y = Mathf.Max(size.y, this.height * this.tr.lossyScale.y);
              this.bounds.size = size;
              break;
            }
            Vector3 size1 = bounds.size * this.meshScale;
            size1.y = Mathf.Max(size1.y, this.height);
            this.bounds = new Bounds(this.transform.position + this.center + bounds.center * this.meshScale, size1);
            break;
          }
          break;
      }
      return this.bounds;
    }

    public void GetContour(List<List<IntPoint>> buffer)
    {
      if (this.circleResolution < 3)
        this.circleResolution = 3;
      Vector3 position = this.tr.position;
      switch (this.type)
      {
        case NavmeshCut.MeshType.Rectangle:
          List<IntPoint> list1 = ListPool<IntPoint>.Claim();
          if (this.useRotation)
          {
            Matrix4x4 localToWorldMatrix = this.tr.localToWorldMatrix;
            list1.Add(this.V3ToIntPoint(localToWorldMatrix.MultiplyPoint3x4(this.center + new Vector3(-this.rectangleSize.x, 0.0f, -this.rectangleSize.y) * 0.5f)));
            list1.Add(this.V3ToIntPoint(localToWorldMatrix.MultiplyPoint3x4(this.center + new Vector3(this.rectangleSize.x, 0.0f, -this.rectangleSize.y) * 0.5f)));
            list1.Add(this.V3ToIntPoint(localToWorldMatrix.MultiplyPoint3x4(this.center + new Vector3(this.rectangleSize.x, 0.0f, this.rectangleSize.y) * 0.5f)));
            list1.Add(this.V3ToIntPoint(localToWorldMatrix.MultiplyPoint3x4(this.center + new Vector3(-this.rectangleSize.x, 0.0f, this.rectangleSize.y) * 0.5f)));
          }
          else
          {
            Vector3 vector3 = position + this.center;
            list1.Add(this.V3ToIntPoint(vector3 + new Vector3(-this.rectangleSize.x, 0.0f, -this.rectangleSize.y) * 0.5f));
            list1.Add(this.V3ToIntPoint(vector3 + new Vector3(this.rectangleSize.x, 0.0f, -this.rectangleSize.y) * 0.5f));
            list1.Add(this.V3ToIntPoint(vector3 + new Vector3(this.rectangleSize.x, 0.0f, this.rectangleSize.y) * 0.5f));
            list1.Add(this.V3ToIntPoint(vector3 + new Vector3(-this.rectangleSize.x, 0.0f, this.rectangleSize.y) * 0.5f));
          }
          buffer.Add(list1);
          break;
        case NavmeshCut.MeshType.Circle:
          List<IntPoint> list2 = ListPool<IntPoint>.Claim(this.circleResolution);
          if (this.useRotation)
          {
            Matrix4x4 localToWorldMatrix = this.tr.localToWorldMatrix;
            for (int index = 0; index < this.circleResolution; ++index)
              list2.Add(this.V3ToIntPoint(localToWorldMatrix.MultiplyPoint3x4(this.center + new Vector3(Mathf.Cos((float) (index * 2) * 3.141593f / (float) this.circleResolution), 0.0f, Mathf.Sin((float) (index * 2) * 3.141593f / (float) this.circleResolution)) * this.circleRadius)));
          }
          else
          {
            Vector3 vector3 = position + this.center;
            for (int index = 0; index < this.circleResolution; ++index)
              list2.Add(this.V3ToIntPoint(vector3 + new Vector3(Mathf.Cos((float) (index * 2) * 3.141593f / (float) this.circleResolution), 0.0f, Mathf.Sin((float) (index * 2) * 3.141593f / (float) this.circleResolution)) * this.circleRadius));
          }
          buffer.Add(list2);
          break;
        case NavmeshCut.MeshType.CustomMesh:
          if ((UnityEngine.Object) this.mesh != (UnityEngine.Object) this.lastMesh || this.contours == null)
          {
            this.CalculateMeshContour();
            this.lastMesh = this.mesh;
          }
          if (this.contours == null)
            break;
          Vector3 vector3_1 = position + this.center;
          bool flag = (double) Vector3.Dot(this.tr.up, Vector3.up) < 0.0;
          for (int index1 = 0; index1 < this.contours.Length; ++index1)
          {
            Vector3[] vector3Array = this.contours[index1];
            List<IntPoint> list3 = ListPool<IntPoint>.Claim(vector3Array.Length);
            if (this.useRotation)
            {
              Matrix4x4 localToWorldMatrix = this.tr.localToWorldMatrix;
              for (int index2 = 0; index2 < vector3Array.Length; ++index2)
                list3.Add(this.V3ToIntPoint(localToWorldMatrix.MultiplyPoint3x4(this.center + vector3Array[index2] * this.meshScale)));
            }
            else
            {
              for (int index2 = 0; index2 < vector3Array.Length; ++index2)
                list3.Add(this.V3ToIntPoint(vector3_1 + vector3Array[index2] * this.meshScale));
            }
            if (flag)
              list3.Reverse();
            buffer.Add(list3);
          }
          break;
      }
    }

    public IntPoint V3ToIntPoint(Vector3 p)
    {
      Int3 int3 = (Int3) p;
      return new IntPoint((long) int3.x, (long) int3.z);
    }

    public Vector3 IntPointToV3(IntPoint p)
    {
      return (Vector3) new Int3((int) p.X, 0, (int) p.Y);
    }

    public void OnDrawGizmos()
    {
      if ((UnityEngine.Object) this.tr == (UnityEngine.Object) null)
        this.tr = this.transform;
      List<List<IntPoint>> list1 = ListPool<List<IntPoint>>.Claim();
      this.GetContour(list1);
      Gizmos.color = NavmeshCut.GizmoColor;
      Bounds bounds = this.GetBounds();
      float num = bounds.min.y;
      Vector3 vector3_1 = Vector3.up * (bounds.max.y - num);
      for (int index1 = 0; index1 < list1.Count; ++index1)
      {
        List<IntPoint> list2 = list1[index1];
        for (int index2 = 0; index2 < list2.Count; ++index2)
        {
          Vector3 from = this.IntPointToV3(list2[index2]);
          from.y = num;
          Vector3 vector3_2 = this.IntPointToV3(list2[(index2 + 1) % list2.Count]);
          vector3_2.y = num;
          Gizmos.DrawLine(from, vector3_2);
          Gizmos.DrawLine(from + vector3_1, vector3_2 + vector3_1);
          Gizmos.DrawLine(from, from + vector3_1);
          Gizmos.DrawLine(vector3_2, vector3_2 + vector3_1);
        }
      }
      ListPool<List<IntPoint>>.Release(list1);
    }

    public void OnDrawGizmosSelected()
    {
      Gizmos.color = Color.Lerp(NavmeshCut.GizmoColor, new Color(1f, 1f, 1f, 0.2f), 0.9f);
      Bounds bounds = this.GetBounds();
      Gizmos.DrawCube(bounds.center, bounds.size);
      Gizmos.DrawWireCube(bounds.center, bounds.size);
    }

    public enum MeshType
    {
      Rectangle,
      Circle,
      CustomMesh,
    }
  }
}
