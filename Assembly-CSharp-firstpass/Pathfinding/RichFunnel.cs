// Decompiled with JetBrains decompiler
// Type: Pathfinding.RichFunnel
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
  public class RichFunnel : RichPathPart
  {
    private int[] triBuffer = new int[3];
    public RichFunnel.FunnelSimplification funnelSimplificationMode = RichFunnel.FunnelSimplification.Iterative;
    private List<Vector3> left;
    private List<Vector3> right;
    private List<TriangleMeshNode> nodes;
    public Vector3 exactStart;
    public Vector3 exactEnd;
    private IFunnelGraph graph;
    private int currentNode;
    private Vector3 currentPosition;
    private int tmpCounter;
    private RichPath path;

    public RichFunnel()
    {
      this.left = ListPool<Vector3>.Claim();
      this.right = ListPool<Vector3>.Claim();
      this.nodes = new List<TriangleMeshNode>();
      this.graph = (IFunnelGraph) null;
    }

    public RichFunnel Initialize(RichPath path, IFunnelGraph graph)
    {
      if (graph == null)
        throw new ArgumentNullException("graph");
      if (this.graph != null)
        throw new InvalidOperationException("Trying to initialize an already initialized object. " + (object) graph);
      this.graph = graph;
      this.path = path;
      return this;
    }

    public override void OnEnterPool()
    {
      this.left.Clear();
      this.right.Clear();
      this.nodes.Clear();
      this.graph = (IFunnelGraph) null;
      this.currentNode = 0;
      this.tmpCounter = 0;
    }

    public void BuildFunnelCorridor(List<GraphNode> nodes, int start, int end)
    {
      this.exactStart = (nodes[start] as MeshNode).ClosestPointOnNode(this.exactStart);
      this.exactEnd = (nodes[end] as MeshNode).ClosestPointOnNode(this.exactEnd);
      this.left.Clear();
      this.right.Clear();
      this.left.Add(this.exactStart);
      this.right.Add(this.exactStart);
      this.nodes.Clear();
      IRaycastableGraph raycastableGraph = this.graph as IRaycastableGraph;
      if (raycastableGraph != null && this.funnelSimplificationMode != RichFunnel.FunnelSimplification.None)
      {
        List<GraphNode> list = ListPool<GraphNode>.Claim(end - start);
        switch (this.funnelSimplificationMode)
        {
          case RichFunnel.FunnelSimplification.Iterative:
            this.SimplifyPath(raycastableGraph, nodes, start, end, list, this.exactStart, this.exactEnd);
            break;
          case RichFunnel.FunnelSimplification.RecursiveBinary:
            RichFunnel.SimplifyPath2(raycastableGraph, nodes, start, end, list, this.exactStart, this.exactEnd);
            break;
          case RichFunnel.FunnelSimplification.RecursiveTrinary:
            RichFunnel.SimplifyPath3(raycastableGraph, nodes, start, end, list, this.exactStart, this.exactEnd, 0);
            break;
        }
        if (this.nodes.Capacity < list.Count)
          this.nodes.Capacity = list.Count;
        for (int index = 0; index < list.Count; ++index)
        {
          TriangleMeshNode triangleMeshNode = list[index] as TriangleMeshNode;
          if (triangleMeshNode != null)
            this.nodes.Add(triangleMeshNode);
        }
        ListPool<GraphNode>.Release(list);
      }
      else
      {
        if (this.nodes.Capacity < end - start)
          this.nodes.Capacity = end - start;
        for (int index = start; index <= end; ++index)
        {
          TriangleMeshNode triangleMeshNode = nodes[index] as TriangleMeshNode;
          if (triangleMeshNode != null)
            this.nodes.Add(triangleMeshNode);
        }
      }
      for (int index = 0; index < this.nodes.Count - 1; ++index)
        this.nodes[index].GetPortal((GraphNode) this.nodes[index + 1], this.left, this.right, false);
      this.left.Add(this.exactEnd);
      this.right.Add(this.exactEnd);
    }

    public static void SimplifyPath3(IRaycastableGraph rcg, List<GraphNode> nodes, int start, int end, List<GraphNode> result, Vector3 startPoint, Vector3 endPoint, int depth = 0)
    {
      if (start == end)
        result.Add(nodes[start]);
      else if (start + 1 == end)
      {
        result.Add(nodes[start]);
        result.Add(nodes[end]);
      }
      else
      {
        int count = result.Count;
        GraphHitInfo hit;
        if (!rcg.Linecast(startPoint, endPoint, nodes[start], out hit, result) && result[result.Count - 1] == nodes[end])
          return;
        result.RemoveRange(count, result.Count - count);
        int num1 = 0;
        float num2 = 0.0f;
        for (int index = start + 1; index < end - 1; ++index)
        {
          float num3 = AstarMath.DistancePointSegmentStrict(startPoint, endPoint, (Vector3) nodes[index].position);
          if ((double) num3 > (double) num2)
          {
            num1 = index;
            num2 = num3;
          }
        }
        int index1 = (num1 + start) / 2;
        int index2 = (num1 + end) / 2;
        if (index1 == index2)
        {
          RichFunnel.SimplifyPath3(rcg, nodes, start, index1, result, startPoint, (Vector3) nodes[index1].position, 0);
          result.RemoveAt(result.Count - 1);
          RichFunnel.SimplifyPath3(rcg, nodes, index1, end, result, (Vector3) nodes[index1].position, endPoint, depth + 1);
        }
        else
        {
          RichFunnel.SimplifyPath3(rcg, nodes, start, index1, result, startPoint, (Vector3) nodes[index1].position, depth + 1);
          result.RemoveAt(result.Count - 1);
          RichFunnel.SimplifyPath3(rcg, nodes, index1, index2, result, (Vector3) nodes[index1].position, (Vector3) nodes[index2].position, depth + 1);
          result.RemoveAt(result.Count - 1);
          RichFunnel.SimplifyPath3(rcg, nodes, index2, end, result, (Vector3) nodes[index2].position, endPoint, depth + 1);
        }
      }
    }

    public static void SimplifyPath2(IRaycastableGraph rcg, List<GraphNode> nodes, int start, int end, List<GraphNode> result, Vector3 startPoint, Vector3 endPoint)
    {
      int count = result.Count;
      if (end <= start + 1)
      {
        result.Add(nodes[start]);
        result.Add(nodes[end]);
      }
      else
      {
        GraphHitInfo hit;
        if (!rcg.Linecast(startPoint, endPoint, nodes[start], out hit, result) && result[result.Count - 1] == nodes[end])
          return;
        result.RemoveRange(count, result.Count - count);
        int index1 = -1;
        float num1 = float.PositiveInfinity;
        for (int index2 = start + 1; index2 < end; ++index2)
        {
          float num2 = AstarMath.DistancePointSegmentStrict(startPoint, endPoint, (Vector3) nodes[index2].position);
          if (index1 == -1 || (double) num2 < (double) num1)
          {
            index1 = index2;
            num1 = num2;
          }
        }
        RichFunnel.SimplifyPath2(rcg, nodes, start, index1, result, startPoint, (Vector3) nodes[index1].position);
        result.RemoveAt(result.Count - 1);
        RichFunnel.SimplifyPath2(rcg, nodes, index1, end, result, (Vector3) nodes[index1].position, endPoint);
      }
    }

    public void SimplifyPath(IRaycastableGraph graph, List<GraphNode> nodes, int start, int end, List<GraphNode> result, Vector3 startPoint, Vector3 endPoint)
    {
      if (start > end)
        throw new ArgumentException("start >= end");
      IRaycastableGraph raycastableGraph = graph;
      if (raycastableGraph == null)
        throw new InvalidOperationException("graph is not a IRaycastableGraph");
      int num1 = start;
      int num2 = 0;
      while (num2++ <= 1000)
      {
        if (start == end)
        {
          result.Add(nodes[end]);
          return;
        }
        int count = result.Count;
        int num3 = end + 1;
        int index1 = start + 1;
        bool flag = false;
        while (num3 > index1 + 1)
        {
          int index2 = (num3 + index1) / 2;
          Vector3 start1 = start != num1 ? (Vector3) nodes[start].position : startPoint;
          Vector3 end1 = index2 != end ? (Vector3) nodes[index2].position : endPoint;
          GraphHitInfo hit;
          if (raycastableGraph.Linecast(start1, end1, nodes[start], out hit))
          {
            num3 = index2;
          }
          else
          {
            flag = true;
            index1 = index2;
          }
        }
        if (!flag)
        {
          result.Add(nodes[start]);
          start = index1;
        }
        else
        {
          Vector3 start1 = start != num1 ? (Vector3) nodes[start].position : startPoint;
          Vector3 end1 = index1 != end ? (Vector3) nodes[index1].position : endPoint;
          GraphHitInfo hit;
          raycastableGraph.Linecast(start1, end1, nodes[start], out hit, result);
          long num4 = 0L;
          long num5 = 0L;
          for (int index2 = start; index2 <= index1; ++index2)
            num4 += (long) nodes[index2].Penalty + (!((UnityEngine.Object) this.path.seeker != (UnityEngine.Object) null) ? 0L : (long) this.path.seeker.tagPenalties[(IntPtr) nodes[index2].Tag]);
          for (int index2 = count; index2 < result.Count; ++index2)
            num5 += (long) result[index2].Penalty + (!((UnityEngine.Object) this.path.seeker != (UnityEngine.Object) null) ? 0L : (long) this.path.seeker.tagPenalties[(IntPtr) result[index2].Tag]);
          if ((double) num4 * 1.4 * (double) (index1 - start + 1) < (double) (num5 * (long) (result.Count - count)) || result[result.Count - 1] != nodes[index1])
          {
            result.RemoveRange(count, result.Count - count);
            result.Add(nodes[start]);
            ++start;
          }
          else
          {
            result.RemoveAt(result.Count - 1);
            start = index1;
          }
        }
      }
      Debug.LogError((object) "!!!");
    }

    public void UpdateFunnelCorridor(int splitIndex, TriangleMeshNode prefix)
    {
      if (splitIndex > 0)
      {
        this.nodes.RemoveRange(0, splitIndex - 1);
        this.nodes[0] = prefix;
      }
      else
        this.nodes.Insert(0, prefix);
      this.left.Clear();
      this.right.Clear();
      this.left.Add(this.exactStart);
      this.right.Add(this.exactStart);
      for (int index = 0; index < this.nodes.Count - 1; ++index)
        this.nodes[index].GetPortal((GraphNode) this.nodes[index + 1], this.left, this.right, false);
      this.left.Add(this.exactEnd);
      this.right.Add(this.exactEnd);
    }

    public Vector3 Update(Vector3 position, List<Vector3> buffer, int numCorners, out bool lastCorner, out bool requiresRepath)
    {
      lastCorner = false;
      requiresRepath = false;
      Int3 p = (Int3) position;
      if (this.nodes[this.currentNode].Destroyed)
      {
        requiresRepath = true;
        lastCorner = false;
        buffer.Add(position);
        return position;
      }
      if (this.nodes[this.currentNode].ContainsPoint(p))
      {
        if (this.tmpCounter >= 10)
        {
          this.tmpCounter = 0;
          int index = 0;
          for (int count = this.nodes.Count; index < count; ++index)
          {
            if (this.nodes[index].Destroyed)
            {
              requiresRepath = true;
              break;
            }
          }
        }
        else
          ++this.tmpCounter;
      }
      else
      {
        bool flag1 = false;
        int index1 = this.currentNode + 1;
        for (int index2 = Math.Min(this.currentNode + 3, this.nodes.Count); index1 < index2 && !flag1; ++index1)
        {
          if (this.nodes[index1].Destroyed)
          {
            requiresRepath = true;
            lastCorner = false;
            buffer.Add(position);
            return position;
          }
          if (this.nodes[index1].ContainsPoint(p))
          {
            this.currentNode = index1;
            flag1 = true;
          }
        }
        int index3 = this.currentNode - 1;
        for (int index2 = Math.Max(this.currentNode - 3, 0); index3 > index2 && !flag1; --index3)
        {
          if (this.nodes[index3].Destroyed)
          {
            requiresRepath = true;
            lastCorner = false;
            buffer.Add(position);
            return position;
          }
          if (this.nodes[index3].ContainsPoint(p))
          {
            this.currentNode = index3;
            flag1 = true;
          }
        }
        int num1 = 0;
        float num2 = float.PositiveInfinity;
        Vector3 vector3_1 = Vector3.zero;
        int index4 = 0;
        for (int count = this.nodes.Count; index4 < count && !flag1; ++index4)
        {
          if (this.nodes[index4].Destroyed)
          {
            requiresRepath = true;
            lastCorner = false;
            buffer.Add(position);
            return position;
          }
          if (this.nodes[index4].ContainsPoint(p))
          {
            this.currentNode = index4;
            flag1 = true;
            vector3_1 = position;
          }
          else
          {
            Vector3 vector3_2 = this.nodes[index4].ClosestPointOnNodeXZ(position);
            float sqrMagnitude = (vector3_2 - position).sqrMagnitude;
            if ((double) sqrMagnitude < (double) num2)
            {
              num2 = sqrMagnitude;
              num1 = index4;
              vector3_1 = vector3_2;
            }
          }
        }
        this.tmpCounter = 0;
        int index5 = 0;
        for (int count = this.nodes.Count; index5 < count; ++index5)
        {
          if (this.nodes[index5].Destroyed)
          {
            requiresRepath = true;
            break;
          }
        }
        if (!flag1)
        {
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          RichFunnel.\u003CUpdate\u003Ec__AnonStorey11 updateCAnonStorey11 = new RichFunnel.\u003CUpdate\u003Ec__AnonStorey11();
          // ISSUE: reference to a compiler-generated field
          updateCAnonStorey11.\u003C\u003Ef__this = this;
          vector3_1.y = position.y;
          // ISSUE: reference to a compiler-generated field
          updateCAnonStorey11.containingPoint = (MeshNode) null;
          // ISSUE: reference to a compiler-generated field
          updateCAnonStorey11.containingIndex = this.nodes.Count - 1;
          // ISSUE: reference to a compiler-generated field
          updateCAnonStorey11.i3Copy = p;
          // ISSUE: reference to a compiler-generated method
          GraphNodeDelegate del = new GraphNodeDelegate(updateCAnonStorey11.\u003C\u003Em__0);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          for (; updateCAnonStorey11.containingIndex >= 0 && updateCAnonStorey11.containingPoint == null; updateCAnonStorey11.containingIndex = updateCAnonStorey11.containingIndex - 1)
          {
            // ISSUE: reference to a compiler-generated field
            this.nodes[updateCAnonStorey11.containingIndex].GetConnections(del);
          }
          bool flag2;
          // ISSUE: reference to a compiler-generated field
          if (updateCAnonStorey11.containingPoint != null)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            updateCAnonStorey11.containingIndex = updateCAnonStorey11.containingIndex + 1;
            this.exactStart = position;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.UpdateFunnelCorridor(updateCAnonStorey11.containingIndex, updateCAnonStorey11.containingPoint as TriangleMeshNode);
            this.currentNode = 0;
            flag2 = true;
          }
          else
          {
            position = vector3_1;
            flag2 = true;
            this.currentNode = num1;
          }
        }
      }
      this.currentPosition = position;
      if (this.FindNextCorners(position, this.currentNode, buffer, numCorners, out lastCorner))
        return position;
      Debug.LogError((object) "Oh oh");
      buffer.Add(position);
      return position;
    }

    public void FindWalls(List<Vector3> wallBuffer, float range)
    {
      this.FindWalls(this.currentNode, wallBuffer, this.currentPosition, range);
    }

    private void FindWalls(int nodeIndex, List<Vector3> wallBuffer, Vector3 position, float range)
    {
      if ((double) range <= 0.0)
        return;
      bool flag1 = false;
      bool flag2 = false;
      range *= range;
      position.y = 0.0f;
      int num = 0;
      while (!flag1 || !flag2)
      {
        if ((num >= 0 || !flag1) && (num <= 0 || !flag2))
        {
          if (num < 0 && nodeIndex + num < 0)
            flag1 = true;
          else if (num > 0 && nodeIndex + num >= this.nodes.Count)
          {
            flag2 = true;
          }
          else
          {
            TriangleMeshNode triangleMeshNode1 = nodeIndex + num - 1 >= 0 ? this.nodes[nodeIndex + num - 1] : (TriangleMeshNode) null;
            TriangleMeshNode triangleMeshNode2 = this.nodes[nodeIndex + num];
            TriangleMeshNode triangleMeshNode3 = nodeIndex + num + 1 < this.nodes.Count ? this.nodes[nodeIndex + num + 1] : (TriangleMeshNode) null;
            if (triangleMeshNode2.Destroyed)
              break;
            if ((double) (triangleMeshNode2.ClosestPointOnNodeXZ(position) - position).sqrMagnitude > (double) range)
            {
              if (num < 0)
                flag1 = true;
              else
                flag2 = true;
            }
            else
            {
              for (int index = 0; index < 3; ++index)
                this.triBuffer[index] = 0;
              for (int index1 = 0; index1 < triangleMeshNode2.connections.Length; ++index1)
              {
                TriangleMeshNode triangleMeshNode4 = triangleMeshNode2.connections[index1] as TriangleMeshNode;
                if (triangleMeshNode4 != null)
                {
                  int index2 = -1;
                  for (int i1 = 0; i1 < 3; ++i1)
                  {
                    for (int i2 = 0; i2 < 3; ++i2)
                    {
                      if (triangleMeshNode2.GetVertex(i1) == triangleMeshNode4.GetVertex((i2 + 1) % 3) && triangleMeshNode2.GetVertex((i1 + 1) % 3) == triangleMeshNode4.GetVertex(i2))
                      {
                        index2 = i1;
                        i1 = 3;
                        break;
                      }
                    }
                  }
                  if (index2 != -1)
                    this.triBuffer[index2] = triangleMeshNode4 == triangleMeshNode1 || triangleMeshNode4 == triangleMeshNode3 ? 2 : 1;
                }
              }
              for (int i = 0; i < 3; ++i)
              {
                if (this.triBuffer[i] == 0)
                {
                  wallBuffer.Add((Vector3) triangleMeshNode2.GetVertex(i));
                  wallBuffer.Add((Vector3) triangleMeshNode2.GetVertex((i + 1) % 3));
                }
              }
            }
          }
        }
        num = num >= 0 ? -num - 1 : -num;
      }
    }

    public bool FindNextCorners(Vector3 origin, int startIndex, List<Vector3> funnelPath, int numCorners, out bool lastCorner)
    {
      lastCorner = false;
      if (this.left == null)
        throw new ArgumentNullException("left");
      if (this.right == null)
        throw new ArgumentNullException("right");
      if (funnelPath == null)
        throw new ArgumentNullException("funnelPath");
      if (this.left.Count != this.right.Count)
        throw new ArgumentException("left and right lists must have equal length");
      int count = this.left.Count;
      if (count == 0)
        throw new ArgumentException("no diagonals");
      if (count - startIndex < 3)
      {
        funnelPath.Add(this.left[count - 1]);
        lastCorner = true;
        return true;
      }
      while (this.left[startIndex + 1] == this.left[startIndex + 2] && this.right[startIndex + 1] == this.right[startIndex + 2])
      {
        ++startIndex;
        if (count - startIndex <= 3)
          return false;
      }
      Vector3 p = this.left[startIndex + 2];
      if (p == this.left[startIndex + 1])
        p = this.right[startIndex + 2];
      while (Polygon.IsColinear(origin, this.left[startIndex + 1], this.right[startIndex + 1]) || Polygon.Left(this.left[startIndex + 1], this.right[startIndex + 1], p) == Polygon.Left(this.left[startIndex + 1], this.right[startIndex + 1], origin))
      {
        ++startIndex;
        if (count - startIndex < 3)
        {
          funnelPath.Add(this.left[count - 1]);
          lastCorner = true;
          return true;
        }
        p = this.left[startIndex + 2];
        if (p == this.left[startIndex + 1])
          p = this.right[startIndex + 2];
      }
      Vector3 a = origin;
      Vector3 b1 = this.left[startIndex + 1];
      Vector3 b2 = this.right[startIndex + 1];
      int num1 = startIndex + 1;
      int num2 = startIndex + 1;
      for (int index = startIndex + 2; index < count; ++index)
      {
        if (funnelPath.Count >= numCorners)
          return true;
        if (funnelPath.Count > 2000)
        {
          Debug.LogWarning((object) "Avoiding infinite loop. Remove this check if you have this long paths.");
          break;
        }
        Vector3 c1 = this.left[index];
        Vector3 c2 = this.right[index];
        if ((double) Polygon.TriangleArea2(a, b2, c2) >= 0.0)
        {
          if (a == b2 || (double) Polygon.TriangleArea2(a, b1, c2) <= 0.0)
          {
            b2 = c2;
            num1 = index;
          }
          else
          {
            funnelPath.Add(b1);
            a = b1;
            int num3 = num2;
            b1 = a;
            b2 = a;
            num2 = num3;
            num1 = num3;
            index = num3;
            continue;
          }
        }
        if ((double) Polygon.TriangleArea2(a, b1, c1) <= 0.0)
        {
          if (a == b1 || (double) Polygon.TriangleArea2(a, b2, c1) >= 0.0)
          {
            b1 = c1;
            num2 = index;
          }
          else
          {
            funnelPath.Add(b2);
            a = b2;
            int num3 = num1;
            b1 = a;
            b2 = a;
            num2 = num3;
            num1 = num3;
            index = num3;
          }
        }
      }
      lastCorner = true;
      funnelPath.Add(this.left[count - 1]);
      return true;
    }

    public enum FunnelSimplification
    {
      None,
      Iterative,
      RecursiveBinary,
      RecursiveTrinary,
    }
  }
}
