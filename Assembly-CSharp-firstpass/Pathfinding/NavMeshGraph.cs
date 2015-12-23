// Decompiled with JetBrains decompiler
// Type: Pathfinding.NavMeshGraph
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding.Serialization;
using Pathfinding.Serialization.JsonFx;
using Pathfinding.Util;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Pathfinding
{
  [JsonOptIn]
  [Serializable]
  public class NavMeshGraph : NavGraph, IUpdatableGraph, IRaycastableGraph, INavmesh, INavmeshHolder, IFunnelGraph
  {
    [JsonMember]
    public float scale = 1f;
    [JsonMember]
    public bool accurateNearestNode = true;
    [JsonMember]
    public Mesh sourceMesh;
    [JsonMember]
    public Vector3 offset;
    [JsonMember]
    public Vector3 rotation;
    public TriangleMeshNode[] nodes;
    private BBTree _bbTree;
    [NonSerialized]
    private Int3[] _vertices;
    [NonSerialized]
    private Vector3[] originalVertices;
    [NonSerialized]
    public int[] triangles;

    public TriangleMeshNode[] TriNodes
    {
      get
      {
        return this.nodes;
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

    public override void CreateNodes(int number)
    {
      TriangleMeshNode[] triangleMeshNodeArray = new TriangleMeshNode[number];
      for (int index = 0; index < number; ++index)
      {
        triangleMeshNodeArray[index] = new TriangleMeshNode(this.active);
        triangleMeshNodeArray[index].Penalty = this.initialPenalty;
      }
    }

    public override void GetNodes(GraphNodeDelegateCancelable del)
    {
      if (this.nodes == null)
        return;
      int index = 0;
      while (index < this.nodes.Length && del((GraphNode) this.nodes[index]))
        ++index;
    }

    public override void OnDestroy()
    {
      base.OnDestroy();
      TriangleMeshNode.SetNavmeshHolder(this.active.astarData.GetGraphIndex((NavGraph) this), (INavmeshHolder) null);
    }

    public Int3 GetVertex(int index)
    {
      return this.vertices[index];
    }

    public int GetVertexArrayIndex(int index)
    {
      return index;
    }

    public void GetTileCoordinates(int tileIndex, out int x, out int z)
    {
      x = z = 0;
    }

    public void GenerateMatrix()
    {
      this.SetMatrix(Matrix4x4.TRS(this.offset, Quaternion.Euler(this.rotation), new Vector3(this.scale, this.scale, this.scale)));
    }

    public override void RelocateNodes(Matrix4x4 oldMatrix, Matrix4x4 newMatrix)
    {
      if (this.vertices == null || this.vertices.Length == 0 || (this.originalVertices == null || this.originalVertices.Length != this.vertices.Length))
        return;
      for (int index = 0; index < this._vertices.Length; ++index)
        this._vertices[index] = (Int3) newMatrix.MultiplyPoint3x4(this.originalVertices[index]);
      for (int index1 = 0; index1 < this.nodes.Length; ++index1)
      {
        TriangleMeshNode triangleMeshNode = this.nodes[index1];
        triangleMeshNode.UpdatePositionFromVertices();
        if (triangleMeshNode.connections != null)
        {
          for (int index2 = 0; index2 < triangleMeshNode.connections.Length; ++index2)
            triangleMeshNode.connectionCosts[index2] = (uint) (triangleMeshNode.position - triangleMeshNode.connections[index2].position).costMagnitude;
        }
      }
      this.SetMatrix(newMatrix);
      this.bbTree = new BBTree((INavmeshHolder) this);
      for (int index = 0; index < this.nodes.Length; ++index)
        this.bbTree.Insert((MeshNode) this.nodes[index]);
    }

    public static NNInfo GetNearest(NavMeshGraph graph, GraphNode[] nodes, Vector3 position, NNConstraint constraint, bool accurateNearestNode)
    {
      if (nodes == null || nodes.Length == 0)
      {
        Debug.LogError((object) "NavGraph hasn't been generated yet or does not contain any nodes");
        return new NNInfo();
      }
      if (constraint == null)
        constraint = NNConstraint.None;
      Int3[] vertices = graph.vertices;
      if (graph.bbTree == null)
        return NavMeshGraph.GetNearestForce((NavGraph) graph, (INavmeshHolder) graph, position, constraint, accurateNearestNode);
      float radius = (float) (((double) graph.bbTree.Size.width + (double) graph.bbTree.Size.height) * 0.5 * 0.0199999995529652);
      NNInfo nnInfo = graph.bbTree.QueryCircle(position, radius, constraint);
      if (nnInfo.node == null)
      {
        for (int index = 1; index <= 8; ++index)
        {
          nnInfo = graph.bbTree.QueryCircle(position, (float) (index * index) * radius, constraint);
          if (nnInfo.node != null || (double) ((index - 1) * (index - 1)) * (double) radius > (double) AstarPath.active.maxNearestNodeDistance * 2.0)
            break;
        }
      }
      if (nnInfo.node != null)
        nnInfo.clampedPosition = NavMeshGraph.ClosestPointOnNode(nnInfo.node as TriangleMeshNode, vertices, position);
      if (nnInfo.constrainedNode != null)
      {
        if (constraint.constrainDistance && (double) ((Vector3) nnInfo.constrainedNode.position - position).sqrMagnitude > (double) AstarPath.active.maxNearestNodeDistanceSqr)
          nnInfo.constrainedNode = (GraphNode) null;
        else
          nnInfo.constClampedPosition = NavMeshGraph.ClosestPointOnNode(nnInfo.constrainedNode as TriangleMeshNode, vertices, position);
      }
      return nnInfo;
    }

    public override NNInfo GetNearest(Vector3 position, NNConstraint constraint, GraphNode hint)
    {
      return NavMeshGraph.GetNearest(this, (GraphNode[]) this.nodes, position, constraint, this.accurateNearestNode);
    }

    public override NNInfo GetNearestForce(Vector3 position, NNConstraint constraint)
    {
      return NavMeshGraph.GetNearestForce((NavGraph) this, (INavmeshHolder) this, position, constraint, this.accurateNearestNode);
    }

    public static NNInfo GetNearestForce(NavGraph graph, INavmeshHolder navmesh, Vector3 position, NNConstraint constraint, bool accurateNearestNode)
    {
      NNInfo nearestForceBoth = NavMeshGraph.GetNearestForceBoth(graph, navmesh, position, constraint, accurateNearestNode);
      nearestForceBoth.node = nearestForceBoth.constrainedNode;
      nearestForceBoth.clampedPosition = nearestForceBoth.constClampedPosition;
      return nearestForceBoth;
    }

    public static NNInfo GetNearestForceBoth(NavGraph graph, INavmeshHolder navmesh, Vector3 position, NNConstraint constraint, bool accurateNearestNode)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      NavMeshGraph.\u003CGetNearestForceBoth\u003Ec__AnonStorey2A bothCAnonStorey2A = new NavMeshGraph.\u003CGetNearestForceBoth\u003Ec__AnonStorey2A();
      // ISSUE: reference to a compiler-generated field
      bothCAnonStorey2A.accurateNearestNode = accurateNearestNode;
      // ISSUE: reference to a compiler-generated field
      bothCAnonStorey2A.position = position;
      // ISSUE: reference to a compiler-generated field
      bothCAnonStorey2A.constraint = constraint;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      bothCAnonStorey2A.pos = (Int3) bothCAnonStorey2A.position;
      // ISSUE: reference to a compiler-generated field
      bothCAnonStorey2A.minDist = -1f;
      // ISSUE: reference to a compiler-generated field
      bothCAnonStorey2A.minNode = (GraphNode) null;
      // ISSUE: reference to a compiler-generated field
      bothCAnonStorey2A.minConstDist = -1f;
      // ISSUE: reference to a compiler-generated field
      bothCAnonStorey2A.minConstNode = (GraphNode) null;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      bothCAnonStorey2A.maxDistSqr = !bothCAnonStorey2A.constraint.constrainDistance ? float.PositiveInfinity : AstarPath.active.maxNearestNodeDistanceSqr;
      // ISSUE: reference to a compiler-generated method
      GraphNodeDelegateCancelable del = new GraphNodeDelegateCancelable(bothCAnonStorey2A.\u003C\u003Em__20);
      graph.GetNodes(del);
      // ISSUE: reference to a compiler-generated field
      NNInfo nnInfo = new NNInfo(bothCAnonStorey2A.minNode);
      if (nnInfo.node != null)
      {
        // ISSUE: reference to a compiler-generated field
        Vector3 vector3 = (nnInfo.node as TriangleMeshNode).ClosestPointOnNode(bothCAnonStorey2A.position);
        nnInfo.clampedPosition = vector3;
      }
      // ISSUE: reference to a compiler-generated field
      nnInfo.constrainedNode = bothCAnonStorey2A.minConstNode;
      if (nnInfo.constrainedNode != null)
      {
        // ISSUE: reference to a compiler-generated field
        Vector3 vector3 = (nnInfo.constrainedNode as TriangleMeshNode).ClosestPointOnNode(bothCAnonStorey2A.position);
        nnInfo.constClampedPosition = vector3;
      }
      return nnInfo;
    }

    public void BuildFunnelCorridor(List<GraphNode> path, int startIndex, int endIndex, List<Vector3> left, List<Vector3> right)
    {
      NavMeshGraph.BuildFunnelCorridor((INavmesh) this, path, startIndex, endIndex, left, right);
    }

    public static void BuildFunnelCorridor(INavmesh graph, List<GraphNode> path, int startIndex, int endIndex, List<Vector3> left, List<Vector3> right)
    {
      if (graph == null)
      {
        Debug.LogError((object) "Couldn't cast graph to the appropriate type (graph isn't a Navmesh type graph, it doesn't implement the INavmesh interface)");
      }
      else
      {
        for (int index = startIndex; index < endIndex; ++index)
        {
          TriangleMeshNode triangleMeshNode1 = path[index] as TriangleMeshNode;
          TriangleMeshNode triangleMeshNode2 = path[index + 1] as TriangleMeshNode;
          bool flag = true;
          int i1;
          for (i1 = 0; i1 < 3; ++i1)
          {
            for (int i2 = 0; i2 < 3; ++i2)
            {
              if (triangleMeshNode1.GetVertexIndex(i1) == triangleMeshNode2.GetVertexIndex((i2 + 1) % 3) && triangleMeshNode1.GetVertexIndex((i1 + 1) % 3) == triangleMeshNode2.GetVertexIndex(i2))
              {
                flag = false;
                break;
              }
            }
            if (!flag)
              break;
          }
          if (i1 == 3)
          {
            left.Add((Vector3) triangleMeshNode1.position);
            right.Add((Vector3) triangleMeshNode1.position);
            left.Add((Vector3) triangleMeshNode2.position);
            right.Add((Vector3) triangleMeshNode2.position);
          }
          else
          {
            left.Add((Vector3) triangleMeshNode1.GetVertex(i1));
            right.Add((Vector3) triangleMeshNode1.GetVertex((i1 + 1) % 3));
          }
        }
      }
    }

    public void AddPortal(GraphNode n1, GraphNode n2, List<Vector3> left, List<Vector3> right)
    {
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

    public bool Linecast(Vector3 origin, Vector3 end, GraphNode hint, out GraphHitInfo hit, List<GraphNode> trace)
    {
      return NavMeshGraph.Linecast((INavmesh) this, origin, end, hint, out hit, trace);
    }

    public static bool Linecast(INavmesh graph, Vector3 tmp_origin, Vector3 tmp_end, GraphNode hint, out GraphHitInfo hit)
    {
      return NavMeshGraph.Linecast(graph, tmp_origin, tmp_end, hint, out hit, (List<GraphNode>) null);
    }

    public static bool Linecast(INavmesh graph, Vector3 tmp_origin, Vector3 tmp_end, GraphNode hint, out GraphHitInfo hit, List<GraphNode> trace)
    {
      Int3 p = (Int3) tmp_end;
      Int3 int3_1 = (Int3) tmp_origin;
      hit = new GraphHitInfo();
      if (float.IsNaN(tmp_origin.x + tmp_origin.y + tmp_origin.z))
        throw new ArgumentException("origin is NaN");
      if (float.IsNaN(tmp_end.x + tmp_end.y + tmp_end.z))
        throw new ArgumentException("end is NaN");
      TriangleMeshNode triangleMeshNode1 = hint as TriangleMeshNode;
      if (triangleMeshNode1 == null)
      {
        triangleMeshNode1 = (graph as NavGraph).GetNearest(tmp_origin, NNConstraint.None).node as TriangleMeshNode;
        if (triangleMeshNode1 == null)
        {
          Debug.LogError((object) "Could not find a valid node to start from");
          hit.point = tmp_origin;
          return true;
        }
      }
      if (int3_1 == p)
      {
        hit.node = (GraphNode) triangleMeshNode1;
        return false;
      }
      Int3 int3_2 = (Int3) triangleMeshNode1.ClosestPointOnNode((Vector3) int3_1);
      hit.origin = (Vector3) int3_2;
      if (!triangleMeshNode1.Walkable)
      {
        hit.point = (Vector3) int3_2;
        hit.tangentOrigin = (Vector3) int3_2;
        return true;
      }
      List<Vector3> list1 = ListPool<Vector3>.Claim();
      List<Vector3> list2 = ListPool<Vector3>.Claim();
      int num = 0;
      while (true)
      {
        ++num;
        if (num <= 2000)
        {
          TriangleMeshNode triangleMeshNode2 = (TriangleMeshNode) null;
          if (trace != null)
            trace.Add((GraphNode) triangleMeshNode1);
          if (!triangleMeshNode1.ContainsPoint(p))
          {
            for (int index = 0; index < triangleMeshNode1.connections.Length; ++index)
            {
              if ((int) triangleMeshNode1.connections[index].GraphIndex == (int) triangleMeshNode1.GraphIndex)
              {
                list1.Clear();
                list2.Clear();
                if (triangleMeshNode1.GetPortal(triangleMeshNode1.connections[index], list1, list2, false))
                {
                  Vector3 vector3_1 = list1[0];
                  Vector3 vector3_2 = list2[0];
                  float factor1;
                  float factor2;
                  if ((Polygon.LeftNotColinear(vector3_1, vector3_2, hit.origin) || !Polygon.LeftNotColinear(vector3_1, vector3_2, tmp_end)) && (Polygon.IntersectionFactor(vector3_1, vector3_2, hit.origin, tmp_end, out factor1, out factor2) && (double) factor2 >= 0.0) && ((double) factor1 >= 0.0 && (double) factor1 <= 1.0))
                  {
                    triangleMeshNode2 = triangleMeshNode1.connections[index] as TriangleMeshNode;
                    break;
                  }
                }
              }
            }
            if (triangleMeshNode2 != null)
              triangleMeshNode1 = triangleMeshNode2;
            else
              goto label_26;
          }
          else
            goto label_17;
        }
        else
          break;
      }
      Debug.LogError((object) "Linecast was stuck in infinite loop. Breaking.");
      ListPool<Vector3>.Release(list1);
      ListPool<Vector3>.Release(list2);
      return true;
label_17:
      ListPool<Vector3>.Release(list1);
      ListPool<Vector3>.Release(list2);
      return false;
label_26:
      int vertexCount = triangleMeshNode1.GetVertexCount();
      for (int i = 0; i < vertexCount; ++i)
      {
        Vector3 vector3_1 = (Vector3) triangleMeshNode1.GetVertex(i);
        Vector3 vector3_2 = (Vector3) triangleMeshNode1.GetVertex((i + 1) % vertexCount);
        float factor1;
        float factor2;
        if ((Polygon.LeftNotColinear(vector3_1, vector3_2, hit.origin) || !Polygon.LeftNotColinear(vector3_1, vector3_2, tmp_end)) && (Polygon.IntersectionFactor(vector3_1, vector3_2, hit.origin, tmp_end, out factor1, out factor2) && (double) factor2 >= 0.0) && ((double) factor1 >= 0.0 && (double) factor1 <= 1.0))
        {
          Vector3 vector3_3 = vector3_1 + (vector3_2 - vector3_1) * factor1;
          hit.point = vector3_3;
          hit.node = (GraphNode) triangleMeshNode1;
          hit.tangent = vector3_2 - vector3_1;
          hit.tangentOrigin = vector3_1;
          ListPool<Vector3>.Release(list1);
          ListPool<Vector3>.Release(list2);
          return true;
        }
      }
      Debug.LogWarning((object) "Linecast failing because point not inside node, and line does not hit any edges of it");
      ListPool<Vector3>.Release(list1);
      ListPool<Vector3>.Release(list2);
      return false;
    }

    public GraphUpdateThreading CanUpdateAsync(GraphUpdateObject o)
    {
      return GraphUpdateThreading.UnityThread;
    }

    public void UpdateAreaInit(GraphUpdateObject o)
    {
    }

    public void UpdateArea(GraphUpdateObject o)
    {
      NavMeshGraph.UpdateArea(o, (INavmesh) this);
    }

    public static void UpdateArea(GraphUpdateObject o, INavmesh graph)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      NavMeshGraph.\u003CUpdateArea\u003Ec__AnonStorey2B areaCAnonStorey2B = new NavMeshGraph.\u003CUpdateArea\u003Ec__AnonStorey2B();
      // ISSUE: reference to a compiler-generated field
      areaCAnonStorey2B.o = o;
      // ISSUE: reference to a compiler-generated field
      Bounds bounds = areaCAnonStorey2B.o.bounds;
      // ISSUE: reference to a compiler-generated field
      areaCAnonStorey2B.r = Rect.MinMaxRect(bounds.min.x, bounds.min.z, bounds.max.x, bounds.max.z);
      // ISSUE: reference to a compiler-generated field
      areaCAnonStorey2B.r2 = new IntRect(Mathf.FloorToInt(bounds.min.x * 1000f), Mathf.FloorToInt(bounds.min.z * 1000f), Mathf.FloorToInt(bounds.max.x * 1000f), Mathf.FloorToInt(bounds.max.z * 1000f));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      areaCAnonStorey2B.a = new Int3(areaCAnonStorey2B.r2.xmin, 0, areaCAnonStorey2B.r2.ymin);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      areaCAnonStorey2B.b = new Int3(areaCAnonStorey2B.r2.xmin, 0, areaCAnonStorey2B.r2.ymax);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      areaCAnonStorey2B.c = new Int3(areaCAnonStorey2B.r2.xmax, 0, areaCAnonStorey2B.r2.ymin);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      areaCAnonStorey2B.d = new Int3(areaCAnonStorey2B.r2.xmax, 0, areaCAnonStorey2B.r2.ymax);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      areaCAnonStorey2B.ia = areaCAnonStorey2B.a;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      areaCAnonStorey2B.ib = areaCAnonStorey2B.b;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      areaCAnonStorey2B.ic = areaCAnonStorey2B.c;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      areaCAnonStorey2B.id = areaCAnonStorey2B.d;
      // ISSUE: reference to a compiler-generated method
      graph.GetNodes(new GraphNodeDelegateCancelable(areaCAnonStorey2B.\u003C\u003Em__21));
    }

    public static Vector3 ClosestPointOnNode(TriangleMeshNode node, Int3[] vertices, Vector3 pos)
    {
      return Polygon.ClosestPointOnTriangle((Vector3) vertices[node.v0], (Vector3) vertices[node.v1], (Vector3) vertices[node.v2], pos);
    }

    public bool ContainsPoint(TriangleMeshNode node, Vector3 pos)
    {
      return Polygon.IsClockwise((Vector3) this.vertices[node.v0], (Vector3) this.vertices[node.v1], pos) && Polygon.IsClockwise((Vector3) this.vertices[node.v1], (Vector3) this.vertices[node.v2], pos) && Polygon.IsClockwise((Vector3) this.vertices[node.v2], (Vector3) this.vertices[node.v0], pos);
    }

    public static bool ContainsPoint(TriangleMeshNode node, Vector3 pos, Int3[] vertices)
    {
      if (!Polygon.IsClockwiseMargin((Vector3) vertices[node.v0], (Vector3) vertices[node.v1], (Vector3) vertices[node.v2]))
        Debug.LogError((object) "Noes!");
      return Polygon.IsClockwiseMargin((Vector3) vertices[node.v0], (Vector3) vertices[node.v1], pos) && Polygon.IsClockwiseMargin((Vector3) vertices[node.v1], (Vector3) vertices[node.v2], pos) && Polygon.IsClockwiseMargin((Vector3) vertices[node.v2], (Vector3) vertices[node.v0], pos);
    }

    public void ScanInternal(string objMeshPath)
    {
      Mesh mesh = ObjImporter.ImportFile(objMeshPath);
      if ((UnityEngine.Object) mesh == (UnityEngine.Object) null)
      {
        Debug.LogError((object) ("Couldn't read .obj file at '" + objMeshPath + "'"));
      }
      else
      {
        this.sourceMesh = mesh;
        this.ScanInternal();
      }
    }

    public override void ScanInternal(OnScanStatus statusCallback)
    {
      if ((UnityEngine.Object) this.sourceMesh == (UnityEngine.Object) null)
        return;
      this.GenerateMatrix();
      Vector3[] vertices = this.sourceMesh.vertices;
      this.triangles = this.sourceMesh.triangles;
      TriangleMeshNode.SetNavmeshHolder(this.active.astarData.GetGraphIndex((NavGraph) this), (INavmeshHolder) this);
      this.GenerateNodes(vertices, this.triangles, out this.originalVertices, out this._vertices);
    }

    private void GenerateNodes(Vector3[] vectorVertices, int[] triangles, out Vector3[] originalVertices, out Int3[] vertices)
    {
      if (vectorVertices.Length == 0 || triangles.Length == 0)
      {
        originalVertices = vectorVertices;
        vertices = new Int3[0];
        this.nodes = new TriangleMeshNode[0];
      }
      else
      {
        vertices = new Int3[vectorVertices.Length];
        int length = 0;
        for (int index = 0; index < vertices.Length; ++index)
          vertices[index] = (Int3) this.matrix.MultiplyPoint3x4(vectorVertices[index]);
        Dictionary<Int3, int> dictionary1 = new Dictionary<Int3, int>();
        int[] numArray = new int[vertices.Length];
        for (int index = 0; index < vertices.Length; ++index)
        {
          if (!dictionary1.ContainsKey(vertices[index]))
          {
            numArray[length] = index;
            dictionary1.Add(vertices[index], length);
            ++length;
          }
        }
        for (int index1 = 0; index1 < triangles.Length; ++index1)
        {
          Int3 index2 = vertices[triangles[index1]];
          triangles[index1] = dictionary1[index2];
        }
        Int3[] int3Array = vertices;
        vertices = new Int3[length];
        originalVertices = new Vector3[length];
        for (int index = 0; index < length; ++index)
        {
          vertices[index] = int3Array[numArray[index]];
          originalVertices[index] = vectorVertices[numArray[index]];
        }
        this.nodes = new TriangleMeshNode[triangles.Length / 3];
        int graphIndex = this.active.astarData.GetGraphIndex((NavGraph) this);
        for (int index = 0; index < this.nodes.Length; ++index)
        {
          this.nodes[index] = new TriangleMeshNode(this.active);
          TriangleMeshNode triangleMeshNode = this.nodes[index];
          triangleMeshNode.GraphIndex = (uint) graphIndex;
          triangleMeshNode.Penalty = this.initialPenalty;
          triangleMeshNode.Walkable = true;
          triangleMeshNode.v0 = triangles[index * 3];
          triangleMeshNode.v1 = triangles[index * 3 + 1];
          triangleMeshNode.v2 = triangles[index * 3 + 2];
          if (!Polygon.IsClockwise(vertices[triangleMeshNode.v0], vertices[triangleMeshNode.v1], vertices[triangleMeshNode.v2]))
          {
            int num = triangleMeshNode.v0;
            triangleMeshNode.v0 = triangleMeshNode.v2;
            triangleMeshNode.v2 = num;
          }
          if (Polygon.IsColinear(vertices[triangleMeshNode.v0], vertices[triangleMeshNode.v1], vertices[triangleMeshNode.v2]))
          {
            Debug.DrawLine((Vector3) vertices[triangleMeshNode.v0], (Vector3) vertices[triangleMeshNode.v1], Color.red);
            Debug.DrawLine((Vector3) vertices[triangleMeshNode.v1], (Vector3) vertices[triangleMeshNode.v2], Color.red);
            Debug.DrawLine((Vector3) vertices[triangleMeshNode.v2], (Vector3) vertices[triangleMeshNode.v0], Color.red);
          }
          triangleMeshNode.UpdatePositionFromVertices();
        }
        Dictionary<Int2, TriangleMeshNode> dictionary2 = new Dictionary<Int2, TriangleMeshNode>();
        int index3 = 0;
        int index4 = 0;
        while (index3 < triangles.Length)
        {
          dictionary2[new Int2(triangles[index3], triangles[index3 + 1])] = this.nodes[index4];
          dictionary2[new Int2(triangles[index3 + 1], triangles[index3 + 2])] = this.nodes[index4];
          dictionary2[new Int2(triangles[index3 + 2], triangles[index3])] = this.nodes[index4];
          ++index4;
          index3 += 3;
        }
        List<MeshNode> list1 = new List<MeshNode>();
        List<uint> list2 = new List<uint>();
        int num1 = 0;
        int num2 = 0;
        int index5 = 0;
        while (num2 < triangles.Length)
        {
          list1.Clear();
          list2.Clear();
          TriangleMeshNode triangleMeshNode1 = this.nodes[index5];
          for (int index1 = 0; index1 < 3; ++index1)
          {
            TriangleMeshNode triangleMeshNode2;
            if (dictionary2.TryGetValue(new Int2(triangles[num2 + (index1 + 1) % 3], triangles[num2 + index1]), out triangleMeshNode2))
            {
              list1.Add((MeshNode) triangleMeshNode2);
              list2.Add((uint) (triangleMeshNode1.position - triangleMeshNode2.position).costMagnitude);
            }
          }
          triangleMeshNode1.connections = (GraphNode[]) list1.ToArray();
          triangleMeshNode1.connectionCosts = list2.ToArray();
          ++index5;
          num2 += 3;
        }
        if (num1 > 0)
          Debug.LogError((object) ("One or more triangles are identical to other triangles, this is not a good thing to have in a navmesh\nIncreasing the scale of the mesh might help\nNumber of triangles with error: " + (object) num1 + "\n"));
        NavMeshGraph.RebuildBBTree(this);
      }
    }

    public static void RebuildBBTree(NavMeshGraph graph)
    {
      NavMeshGraph navMeshGraph = graph;
      BBTree bbTree = navMeshGraph.bbTree ?? new BBTree((INavmeshHolder) graph);
      bbTree.Clear();
      TriangleMeshNode[] triNodes = navMeshGraph.TriNodes;
      for (int index = triNodes.Length - 1; index >= 0; --index)
        bbTree.Insert((MeshNode) triNodes[index]);
      navMeshGraph.bbTree = bbTree;
    }

    public void PostProcess()
    {
    }

    public void Sort(Vector3[] a)
    {
      bool flag = true;
      while (flag)
      {
        flag = false;
        for (int index = 0; index < a.Length - 1; ++index)
        {
          if ((double) a[index].x > (double) a[index + 1].x || (double) a[index].x == (double) a[index + 1].x && ((double) a[index].y > (double) a[index + 1].y || (double) a[index].y == (double) a[index + 1].y && (double) a[index].z > (double) a[index + 1].z))
          {
            Vector3 vector3 = a[index];
            a[index] = a[index + 1];
            a[index + 1] = vector3;
            flag = true;
          }
        }
      }
    }

    public override void OnDrawGizmos(bool drawNodes)
    {
      if (!drawNodes)
        return;
      Matrix4x4 oldMatrix = this.matrix;
      this.GenerateMatrix();
      if (this.nodes != null)
        ;
      if (this.nodes == null)
        return;
      if (this.bbTree != null)
        this.bbTree.OnDrawGizmos();
      if (oldMatrix != this.matrix)
        this.RelocateNodes(oldMatrix, this.matrix);
      PathHandler debugPathData = AstarPath.active.debugPathData;
      for (int index1 = 0; index1 < this.nodes.Length; ++index1)
      {
        TriangleMeshNode triangleMeshNode = this.nodes[index1];
        Gizmos.color = this.NodeColor((GraphNode) triangleMeshNode, AstarPath.active.debugPathData);
        if (triangleMeshNode.Walkable)
        {
          if (AstarPath.active.showSearchTree && debugPathData != null && debugPathData.GetPathNode((GraphNode) triangleMeshNode).parent != null)
          {
            Gizmos.DrawLine((Vector3) triangleMeshNode.position, (Vector3) debugPathData.GetPathNode((GraphNode) triangleMeshNode).parent.node.position);
          }
          else
          {
            for (int index2 = 0; index2 < triangleMeshNode.connections.Length; ++index2)
              Gizmos.DrawLine((Vector3) triangleMeshNode.position, Vector3.Lerp((Vector3) triangleMeshNode.position, (Vector3) triangleMeshNode.connections[index2].position, 0.45f));
          }
          Gizmos.color = AstarColor.MeshEdgeColor;
        }
        else
          Gizmos.color = Color.red;
        Gizmos.DrawLine((Vector3) this.vertices[triangleMeshNode.v0], (Vector3) this.vertices[triangleMeshNode.v1]);
        Gizmos.DrawLine((Vector3) this.vertices[triangleMeshNode.v1], (Vector3) this.vertices[triangleMeshNode.v2]);
        Gizmos.DrawLine((Vector3) this.vertices[triangleMeshNode.v2], (Vector3) this.vertices[triangleMeshNode.v0]);
      }
    }

    public override void DeserializeExtraInfo(GraphSerializationContext ctx)
    {
      uint num = (uint) this.active.astarData.GetGraphIndex((NavGraph) this);
      TriangleMeshNode.SetNavmeshHolder((int) num, (INavmeshHolder) this);
      int length1 = ctx.reader.ReadInt32();
      int length2 = ctx.reader.ReadInt32();
      if (length1 == -1)
      {
        this.nodes = new TriangleMeshNode[0];
        this._vertices = new Int3[0];
        this.originalVertices = new Vector3[0];
      }
      this.nodes = new TriangleMeshNode[length1];
      this._vertices = new Int3[length2];
      this.originalVertices = new Vector3[length2];
      for (int index = 0; index < length2; ++index)
      {
        this._vertices[index] = new Int3(ctx.reader.ReadInt32(), ctx.reader.ReadInt32(), ctx.reader.ReadInt32());
        this.originalVertices[index] = new Vector3(ctx.reader.ReadSingle(), ctx.reader.ReadSingle(), ctx.reader.ReadSingle());
      }
      this.bbTree = new BBTree((INavmeshHolder) this);
      for (int index = 0; index < length1; ++index)
      {
        this.nodes[index] = new TriangleMeshNode(this.active);
        TriangleMeshNode triangleMeshNode = this.nodes[index];
        triangleMeshNode.DeserializeNode(ctx);
        triangleMeshNode.GraphIndex = num;
        triangleMeshNode.UpdatePositionFromVertices();
        this.bbTree.Insert((MeshNode) triangleMeshNode);
      }
    }

    public override void SerializeExtraInfo(GraphSerializationContext ctx)
    {
      if (this.nodes == null || this.originalVertices == null || (this._vertices == null || this.originalVertices.Length != this._vertices.Length))
      {
        ctx.writer.Write(-1);
        ctx.writer.Write(-1);
      }
      else
      {
        ctx.writer.Write(this.nodes.Length);
        ctx.writer.Write(this._vertices.Length);
        for (int index = 0; index < this._vertices.Length; ++index)
        {
          ctx.writer.Write(this._vertices[index].x);
          ctx.writer.Write(this._vertices[index].y);
          ctx.writer.Write(this._vertices[index].z);
          ctx.writer.Write(this.originalVertices[index].x);
          ctx.writer.Write(this.originalVertices[index].y);
          ctx.writer.Write(this.originalVertices[index].z);
        }
        for (int index = 0; index < this.nodes.Length; ++index)
          this.nodes[index].SerializeNode(ctx);
      }
    }

    public static void DeserializeMeshNodes(NavMeshGraph graph, GraphNode[] nodes, byte[] bytes)
    {
      BinaryReader binaryReader = new BinaryReader((Stream) new MemoryStream(bytes));
      for (int index = 0; index < nodes.Length; ++index)
      {
        TriangleMeshNode triangleMeshNode = nodes[index] as TriangleMeshNode;
        if (triangleMeshNode == null)
        {
          Debug.LogError((object) "Serialization Error : Couldn't cast the node to the appropriate type - NavMeshGenerator");
          return;
        }
        triangleMeshNode.v0 = binaryReader.ReadInt32();
        triangleMeshNode.v1 = binaryReader.ReadInt32();
        triangleMeshNode.v2 = binaryReader.ReadInt32();
      }
      int length = binaryReader.ReadInt32();
      graph.vertices = new Int3[length];
      for (int index = 0; index < length; ++index)
      {
        int _x = binaryReader.ReadInt32();
        int _y = binaryReader.ReadInt32();
        int _z = binaryReader.ReadInt32();
        graph.vertices[index] = new Int3(_x, _y, _z);
      }
      NavMeshGraph.RebuildBBTree(graph);
    }
  }
}
