// Decompiled with JetBrains decompiler
// Type: Pathfinding.PathUtilities
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
  public static class PathUtilities
  {
    private static Queue<GraphNode> BFSQueue;
    private static Dictionary<GraphNode, int> BFSMap;

    public static bool IsPathPossible(GraphNode n1, GraphNode n2)
    {
      if (n1.Walkable && n2.Walkable)
        return (int) n1.Area == (int) n2.Area;
      return false;
    }

    public static bool IsPathPossible(List<GraphNode> nodes)
    {
      uint area = nodes[0].Area;
      for (int index = 0; index < nodes.Count; ++index)
      {
        if (!nodes[index].Walkable || (int) nodes[index].Area != (int) area)
          return false;
      }
      return true;
    }

    public static List<GraphNode> GetReachableNodes(GraphNode seed, int tagMask = -1)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PathUtilities.\u003CGetReachableNodes\u003Ec__AnonStorey38 nodesCAnonStorey38 = new PathUtilities.\u003CGetReachableNodes\u003Ec__AnonStorey38();
      // ISSUE: reference to a compiler-generated field
      nodesCAnonStorey38.tagMask = tagMask;
      // ISSUE: reference to a compiler-generated field
      nodesCAnonStorey38.stack = StackPool<GraphNode>.Claim();
      // ISSUE: reference to a compiler-generated field
      nodesCAnonStorey38.list = ListPool<GraphNode>.Claim();
      // ISSUE: reference to a compiler-generated field
      nodesCAnonStorey38.map = new HashSet<GraphNode>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      GraphNodeDelegate del = nodesCAnonStorey38.tagMask != -1 ? new GraphNodeDelegate(nodesCAnonStorey38.\u003C\u003Em__30) : new GraphNodeDelegate(nodesCAnonStorey38.\u003C\u003Em__2F);
      del(seed);
      // ISSUE: reference to a compiler-generated field
      while (nodesCAnonStorey38.stack.Count > 0)
      {
        // ISSUE: reference to a compiler-generated field
        nodesCAnonStorey38.stack.Pop().GetConnections(del);
      }
      // ISSUE: reference to a compiler-generated field
      StackPool<GraphNode>.Release(nodesCAnonStorey38.stack);
      // ISSUE: reference to a compiler-generated field
      return nodesCAnonStorey38.list;
    }

    public static List<GraphNode> BFS(GraphNode seed, int depth, int tagMask = -1)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      PathUtilities.\u003CBFS\u003Ec__AnonStorey39 bfSCAnonStorey39 = new PathUtilities.\u003CBFS\u003Ec__AnonStorey39();
      // ISSUE: reference to a compiler-generated field
      bfSCAnonStorey39.tagMask = tagMask;
      if (PathUtilities.BFSQueue == null)
        PathUtilities.BFSQueue = new Queue<GraphNode>();
      // ISSUE: reference to a compiler-generated field
      bfSCAnonStorey39.que = PathUtilities.BFSQueue;
      if (PathUtilities.BFSMap == null)
        PathUtilities.BFSMap = new Dictionary<GraphNode, int>();
      // ISSUE: reference to a compiler-generated field
      bfSCAnonStorey39.map = PathUtilities.BFSMap;
      // ISSUE: reference to a compiler-generated field
      bfSCAnonStorey39.que.Clear();
      // ISSUE: reference to a compiler-generated field
      bfSCAnonStorey39.map.Clear();
      // ISSUE: reference to a compiler-generated field
      bfSCAnonStorey39.result = ListPool<GraphNode>.Claim();
      // ISSUE: reference to a compiler-generated field
      bfSCAnonStorey39.currentDist = -1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      GraphNodeDelegate del = bfSCAnonStorey39.tagMask != -1 ? new GraphNodeDelegate(bfSCAnonStorey39.\u003C\u003Em__32) : new GraphNodeDelegate(bfSCAnonStorey39.\u003C\u003Em__31);
      del(seed);
      // ISSUE: reference to a compiler-generated field
      while (bfSCAnonStorey39.que.Count > 0)
      {
        // ISSUE: reference to a compiler-generated field
        GraphNode index = bfSCAnonStorey39.que.Dequeue();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        bfSCAnonStorey39.currentDist = bfSCAnonStorey39.map[index];
        // ISSUE: reference to a compiler-generated field
        if (bfSCAnonStorey39.currentDist < depth)
          index.GetConnections(del);
        else
          break;
      }
      // ISSUE: reference to a compiler-generated field
      bfSCAnonStorey39.que.Clear();
      // ISSUE: reference to a compiler-generated field
      bfSCAnonStorey39.map.Clear();
      // ISSUE: reference to a compiler-generated field
      return bfSCAnonStorey39.result;
    }

    public static List<Vector3> GetSpiralPoints(int count, float clearance)
    {
      List<Vector3> list = ListPool<Vector3>.Claim(count);
      float a = clearance / 6.283185f;
      float t1 = 0.0f;
      list.Add(PathUtilities.InvoluteOfCircle(a, t1));
      for (int index = 0; index < count; ++index)
      {
        Vector3 vector3 = list[list.Count - 1];
        float num1 = (float) (-(double) t1 / 2.0) + Mathf.Sqrt((float) ((double) t1 * (double) t1 / 4.0 + 2.0 * (double) clearance / (double) a));
        float num2 = t1 + num1;
        float t2 = t1 + 2f * num1;
        while ((double) t2 - (double) num2 > 0.00999999977648258)
        {
          float t3 = (float) (((double) num2 + (double) t2) / 2.0);
          if ((double) (PathUtilities.InvoluteOfCircle(a, t3) - vector3).sqrMagnitude < (double) clearance * (double) clearance)
            num2 = t3;
          else
            t2 = t3;
        }
        list.Add(PathUtilities.InvoluteOfCircle(a, t2));
        t1 = t2;
      }
      return list;
    }

    private static Vector3 InvoluteOfCircle(float a, float t)
    {
      return new Vector3(a * (Mathf.Cos(t) + t * Mathf.Sin(t)), 0.0f, a * (Mathf.Sin(t) - t * Mathf.Cos(t)));
    }

    public static void GetPointsAroundPointWorld(Vector3 p, IRaycastableGraph g, List<Vector3> previousPoints, float radius, float clearanceRadius)
    {
      if (previousPoints.Count == 0)
        return;
      Vector3 zero = Vector3.zero;
      for (int index = 0; index < previousPoints.Count; ++index)
        zero += previousPoints[index];
      Vector3 vector3 = zero / (float) previousPoints.Count;
      for (int index = 0; index < previousPoints.Count; ++index)
        previousPoints[index] -= vector3;
      PathUtilities.GetPointsAroundPoint(p, g, previousPoints, radius, clearanceRadius);
    }

    public static void GetPointsAroundPoint(Vector3 p, IRaycastableGraph g, List<Vector3> previousPoints, float radius, float clearanceRadius)
    {
      if (g == null)
        throw new ArgumentNullException("g");
      NavGraph navGraph = g as NavGraph;
      if (navGraph == null)
        throw new ArgumentException("g is not a NavGraph");
      NNInfo nearestForce = navGraph.GetNearestForce(p, NNConstraint.Default);
      p = nearestForce.clampedPosition;
      if (nearestForce.node == null)
        return;
      radius = Mathf.Max(radius, 1.4142f * clearanceRadius * Mathf.Sqrt((float) previousPoints.Count));
      clearanceRadius *= clearanceRadius;
      for (int index1 = 0; index1 < previousPoints.Count; ++index1)
      {
        Vector3 vector3_1 = previousPoints[index1];
        float magnitude = vector3_1.magnitude;
        if ((double) magnitude > 0.0)
          vector3_1 /= magnitude;
        float a = radius;
        vector3_1 *= a;
        bool flag = false;
        int num1 = 0;
        do
        {
          Vector3 end = p + vector3_1;
          GraphHitInfo hit;
          if (g.Linecast(p, end, nearestForce.node, out hit))
            end = hit.point;
          float num2 = 0.1f;
          while ((double) num2 <= 1.0)
          {
            Vector3 vector3_2 = (end - p) * num2 + p;
            flag = true;
            for (int index2 = 0; index2 < index1; ++index2)
            {
              if ((double) (previousPoints[index2] - vector3_2).sqrMagnitude < (double) clearanceRadius)
              {
                flag = false;
                break;
              }
            }
            if (flag)
            {
              previousPoints[index1] = vector3_2;
              break;
            }
            num2 += 0.05f;
          }
          if (!flag)
          {
            if (num1 > 8)
            {
              flag = true;
            }
            else
            {
              clearanceRadius *= 0.9f;
              vector3_1 = UnityEngine.Random.onUnitSphere * Mathf.Lerp(a, radius, (float) (num1 / 5));
              vector3_1.y = 0.0f;
              ++num1;
            }
          }
        }
        while (!flag);
      }
    }

    public static List<Vector3> GetPointsOnNodes(List<GraphNode> nodes, int count, float clearanceRadius = 0)
    {
      if (nodes == null)
        throw new ArgumentNullException("nodes");
      if (nodes.Count == 0)
        throw new ArgumentException("no nodes passed");
      System.Random random = new System.Random();
      List<Vector3> list1 = ListPool<Vector3>.Claim(count);
      clearanceRadius *= clearanceRadius;
      if (nodes[0] is TriangleMeshNode || nodes[0] is GridNode)
      {
        List<float> list2 = ListPool<float>.Claim(nodes.Count);
        float num1 = 0.0f;
        for (int index = 0; index < nodes.Count; ++index)
        {
          TriangleMeshNode triangleMeshNode = nodes[index] as TriangleMeshNode;
          if (triangleMeshNode != null)
          {
            float num2 = (float) Math.Abs(Polygon.TriangleArea(triangleMeshNode.GetVertex(0), triangleMeshNode.GetVertex(1), triangleMeshNode.GetVertex(2)));
            num1 += num2;
            list2.Add(num1);
          }
          else
          {
            GridNode gridNode = nodes[index] as GridNode;
            if (gridNode != null)
            {
              GridGraph gridGraph = GridNode.GetGridGraph(gridNode.GraphIndex);
              float num2 = gridGraph.nodeSize * gridGraph.nodeSize;
              num1 += num2;
              list2.Add(num1);
            }
            else
              list2.Add(num1);
          }
        }
        for (int index1 = 0; index1 < count; ++index1)
        {
          int num2 = 0;
          int num3 = 10;
          bool flag = false;
          while (!flag)
          {
            flag = true;
            if (num2 >= num3)
            {
              clearanceRadius *= 0.8f;
              num3 += 10;
              if (num3 > 100)
                clearanceRadius = 0.0f;
            }
            float num4 = (float) random.NextDouble() * num1;
            int index2 = list2.BinarySearch(num4);
            if (index2 < 0)
              index2 = ~index2;
            if (index2 >= nodes.Count)
            {
              flag = false;
            }
            else
            {
              TriangleMeshNode triangleMeshNode = nodes[index2] as TriangleMeshNode;
              Vector3 vector3;
              if (triangleMeshNode != null)
              {
                float num5;
                float num6;
                do
                {
                  num5 = (float) random.NextDouble();
                  num6 = (float) random.NextDouble();
                }
                while ((double) num5 + (double) num6 > 1.0);
                vector3 = (Vector3) (triangleMeshNode.GetVertex(1) - triangleMeshNode.GetVertex(0)) * num5 + (Vector3) (triangleMeshNode.GetVertex(2) - triangleMeshNode.GetVertex(0)) * num6 + (Vector3) triangleMeshNode.GetVertex(0);
              }
              else
              {
                GridNode gridNode = nodes[index2] as GridNode;
                if (gridNode != null)
                {
                  GridGraph gridGraph = GridNode.GetGridGraph(gridNode.GraphIndex);
                  float num5 = (float) random.NextDouble();
                  float num6 = (float) random.NextDouble();
                  vector3 = (Vector3) gridNode.position + new Vector3(num5 - 0.5f, 0.0f, num6 - 0.5f) * gridGraph.nodeSize;
                }
                else
                {
                  list1.Add((Vector3) nodes[index2].position);
                  break;
                }
              }
              if ((double) clearanceRadius > 0.0)
              {
                for (int index3 = 0; index3 < list1.Count; ++index3)
                {
                  if ((double) (list1[index3] - vector3).sqrMagnitude < (double) clearanceRadius)
                  {
                    flag = false;
                    break;
                  }
                }
              }
              if (flag)
              {
                list1.Add(vector3);
                break;
              }
              ++num2;
            }
          }
        }
        ListPool<float>.Release(list2);
      }
      else
      {
        for (int index = 0; index < count; ++index)
          list1.Add((Vector3) nodes[random.Next(nodes.Count)].position);
      }
      return list1;
    }
  }
}
