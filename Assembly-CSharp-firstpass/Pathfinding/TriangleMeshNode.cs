// Decompiled with JetBrains decompiler
// Type: Pathfinding.TriangleMeshNode
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding.Serialization;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
  public class TriangleMeshNode : MeshNode
  {
    protected static INavmeshHolder[] _navmeshHolders = new INavmeshHolder[0];
    public int v0;
    public int v1;
    public int v2;

    public TriangleMeshNode(AstarPath astar)
      : base(astar)
    {
    }

    public static INavmeshHolder GetNavmeshHolder(uint graphIndex)
    {
      return TriangleMeshNode._navmeshHolders[(int) graphIndex];
    }

    public static void SetNavmeshHolder(int graphIndex, INavmeshHolder graph)
    {
      if (TriangleMeshNode._navmeshHolders.Length <= graphIndex)
      {
        INavmeshHolder[] navmeshHolderArray = new INavmeshHolder[graphIndex + 1];
        for (int index = 0; index < TriangleMeshNode._navmeshHolders.Length; ++index)
          navmeshHolderArray[index] = TriangleMeshNode._navmeshHolders[index];
        TriangleMeshNode._navmeshHolders = navmeshHolderArray;
      }
      if (graphIndex == -1)
        return;
      TriangleMeshNode._navmeshHolders[graphIndex] = graph;
    }

    public void UpdatePositionFromVertices()
    {
      INavmeshHolder navmeshHolder = TriangleMeshNode.GetNavmeshHolder(this.GraphIndex);
      this.position = (navmeshHolder.GetVertex(this.v0) + navmeshHolder.GetVertex(this.v1) + navmeshHolder.GetVertex(this.v2)) * 0.333333f;
    }

    public int GetVertexIndex(int i)
    {
      if (i == 0)
        return this.v0;
      if (i == 1)
        return this.v1;
      return this.v2;
    }

    public int GetVertexArrayIndex(int i)
    {
      return TriangleMeshNode.GetNavmeshHolder(this.GraphIndex).GetVertexArrayIndex(i != 0 ? (i != 1 ? this.v2 : this.v1) : this.v0);
    }

    public override Int3 GetVertex(int i)
    {
      return TriangleMeshNode.GetNavmeshHolder(this.GraphIndex).GetVertex(this.GetVertexIndex(i));
    }

    public override int GetVertexCount()
    {
      return 3;
    }

    public override Vector3 ClosestPointOnNode(Vector3 p)
    {
      INavmeshHolder navmeshHolder = TriangleMeshNode.GetNavmeshHolder(this.GraphIndex);
      return Polygon.ClosestPointOnTriangle((Vector3) navmeshHolder.GetVertex(this.v0), (Vector3) navmeshHolder.GetVertex(this.v1), (Vector3) navmeshHolder.GetVertex(this.v2), p);
    }

    public override Vector3 ClosestPointOnNodeXZ(Vector3 _p)
    {
      INavmeshHolder navmeshHolder = TriangleMeshNode.GetNavmeshHolder(this.GraphIndex);
      Int3 vertex1 = navmeshHolder.GetVertex(this.v0);
      Int3 vertex2 = navmeshHolder.GetVertex(this.v1);
      Int3 vertex3 = navmeshHolder.GetVertex(this.v2);
      Int3 point = (Int3) _p;
      int num1 = point.y;
      vertex1.y = 0;
      vertex2.y = 0;
      vertex3.y = 0;
      point.y = 0;
      if ((long) (vertex2.x - vertex1.x) * (long) (point.z - vertex1.z) - (long) (point.x - vertex1.x) * (long) (vertex2.z - vertex1.z) > 0L)
      {
        float num2 = Mathf.Clamp01(AstarMath.NearestPointFactor(vertex1, vertex2, point));
        return new Vector3((float) vertex1.x + (float) (vertex2.x - vertex1.x) * num2, (float) num1, (float) vertex1.z + (float) (vertex2.z - vertex1.z) * num2) * (1.0 / 1000.0);
      }
      if ((long) (vertex3.x - vertex2.x) * (long) (point.z - vertex2.z) - (long) (point.x - vertex2.x) * (long) (vertex3.z - vertex2.z) > 0L)
      {
        float num2 = Mathf.Clamp01(AstarMath.NearestPointFactor(vertex2, vertex3, point));
        return new Vector3((float) vertex2.x + (float) (vertex3.x - vertex2.x) * num2, (float) num1, (float) vertex2.z + (float) (vertex3.z - vertex2.z) * num2) * (1.0 / 1000.0);
      }
      if ((long) (vertex1.x - vertex3.x) * (long) (point.z - vertex3.z) - (long) (point.x - vertex3.x) * (long) (vertex1.z - vertex3.z) <= 0L)
        return _p;
      float num3 = Mathf.Clamp01(AstarMath.NearestPointFactor(vertex3, vertex1, point));
      return new Vector3((float) vertex3.x + (float) (vertex1.x - vertex3.x) * num3, (float) num1, (float) vertex3.z + (float) (vertex1.z - vertex3.z) * num3) * (1.0 / 1000.0);
    }

    public override bool ContainsPoint(Int3 p)
    {
      INavmeshHolder navmeshHolder = TriangleMeshNode.GetNavmeshHolder(this.GraphIndex);
      Int3 vertex1 = navmeshHolder.GetVertex(this.v0);
      Int3 vertex2 = navmeshHolder.GetVertex(this.v1);
      Int3 vertex3 = navmeshHolder.GetVertex(this.v2);
      return (long) (vertex2.x - vertex1.x) * (long) (p.z - vertex1.z) - (long) (p.x - vertex1.x) * (long) (vertex2.z - vertex1.z) <= 0L && (long) (vertex3.x - vertex2.x) * (long) (p.z - vertex2.z) - (long) (p.x - vertex2.x) * (long) (vertex3.z - vertex2.z) <= 0L && (long) (vertex1.x - vertex3.x) * (long) (p.z - vertex3.z) - (long) (p.x - vertex3.x) * (long) (vertex1.z - vertex3.z) <= 0L;
    }

    public override void UpdateRecursiveG(Path path, PathNode pathNode, PathHandler handler)
    {
      this.UpdateG(path, pathNode);
      handler.PushNode(pathNode);
      if (this.connections == null)
        return;
      for (int index = 0; index < this.connections.Length; ++index)
      {
        GraphNode node = this.connections[index];
        PathNode pathNode1 = handler.GetPathNode(node);
        if (pathNode1.parent == pathNode && (int) pathNode1.pathID == (int) handler.PathID)
          node.UpdateRecursiveG(path, pathNode1, handler);
      }
    }

    public override void Open(Path path, PathNode pathNode, PathHandler handler)
    {
      if (this.connections == null)
        return;
      bool flag2 = pathNode.flag2;
      for (int index = this.connections.Length - 1; index >= 0; --index)
      {
        GraphNode graphNode = this.connections[index];
        if (path.CanTraverse(graphNode))
        {
          PathNode pathNode1 = handler.GetPathNode(graphNode);
          if (pathNode1 != pathNode.parent)
          {
            uint currentCost = this.connectionCosts[index];
            if (flag2 || pathNode1.flag2)
              currentCost = path.GetConnectionSpecialCost((GraphNode) this, graphNode, currentCost);
            if ((int) pathNode1.pathID != (int) handler.PathID)
            {
              pathNode1.node = graphNode;
              pathNode1.parent = pathNode;
              pathNode1.pathID = handler.PathID;
              pathNode1.cost = currentCost;
              pathNode1.H = path.CalculateHScore(graphNode);
              graphNode.UpdateG(path, pathNode1);
              handler.PushNode(pathNode1);
            }
            else if (pathNode.G + currentCost + path.GetTraversalCost(graphNode) < pathNode1.G)
            {
              pathNode1.cost = currentCost;
              pathNode1.parent = pathNode;
              graphNode.UpdateRecursiveG(path, pathNode1, handler);
            }
            else if (pathNode1.G + currentCost + path.GetTraversalCost((GraphNode) this) < pathNode.G && graphNode.ContainsConnection((GraphNode) this))
            {
              pathNode.parent = pathNode1;
              pathNode.cost = currentCost;
              this.UpdateRecursiveG(path, pathNode, handler);
            }
          }
        }
      }
    }

    public int SharedEdge(GraphNode other)
    {
      int aIndex;
      int bIndex;
      this.GetPortal(other, (List<Vector3>) null, (List<Vector3>) null, false, out aIndex, out bIndex);
      return aIndex;
    }

    public override bool GetPortal(GraphNode _other, List<Vector3> left, List<Vector3> right, bool backwards)
    {
      int aIndex;
      int bIndex;
      return this.GetPortal(_other, left, right, backwards, out aIndex, out bIndex);
    }

    public bool GetPortal(GraphNode _other, List<Vector3> left, List<Vector3> right, bool backwards, out int aIndex, out int bIndex)
    {
      aIndex = -1;
      bIndex = -1;
      if ((int) _other.GraphIndex != (int) this.GraphIndex)
        return false;
      TriangleMeshNode triangleMeshNode = _other as TriangleMeshNode;
      int tileIndex1 = this.GetVertexIndex(0) >> 12 & 524287;
      int tileIndex2 = triangleMeshNode.GetVertexIndex(0) >> 12 & 524287;
      if (tileIndex1 != tileIndex2 && TriangleMeshNode.GetNavmeshHolder(this.GraphIndex) is RecastGraph)
      {
        for (int index = 0; index < this.connections.Length; ++index)
        {
          if ((int) this.connections[index].GraphIndex != (int) this.GraphIndex)
          {
            NodeLink3Node nodeLink3Node = this.connections[index] as NodeLink3Node;
            if (nodeLink3Node != null && nodeLink3Node.GetOther((GraphNode) this) == triangleMeshNode && left != null)
            {
              nodeLink3Node.GetPortal((GraphNode) triangleMeshNode, left, right, false);
              return true;
            }
          }
        }
        INavmeshHolder navmeshHolder = TriangleMeshNode.GetNavmeshHolder(this.GraphIndex);
        int x1;
        int z1;
        navmeshHolder.GetTileCoordinates(tileIndex1, out x1, out z1);
        int x2;
        int z2;
        navmeshHolder.GetTileCoordinates(tileIndex2, out x2, out z2);
        int index1;
        if (Math.Abs(x1 - x2) == 1)
          index1 = 0;
        else if (Math.Abs(z1 - z2) == 1)
          index1 = 2;
        else
          throw new Exception("Tiles not adjacent (" + (object) x1 + ", " + (string) (object) z1 + ") (" + (string) (object) x2 + ", " + (string) (object) z2 + ")");
        int vertexCount1 = this.GetVertexCount();
        int vertexCount2 = triangleMeshNode.GetVertexCount();
        int i1 = -1;
        int i2 = -1;
        for (int i3 = 0; i3 < vertexCount1; ++i3)
        {
          int num = this.GetVertex(i3)[index1];
          for (int i4 = 0; i4 < vertexCount2; ++i4)
          {
            if (num == triangleMeshNode.GetVertex((i4 + 1) % vertexCount2)[index1] && this.GetVertex((i3 + 1) % vertexCount1)[index1] == triangleMeshNode.GetVertex(i4)[index1])
            {
              i1 = i3;
              i2 = i4;
              i3 = vertexCount1;
              break;
            }
          }
        }
        aIndex = i1;
        bIndex = i2;
        if (i1 != -1)
        {
          Int3 vertex1 = this.GetVertex(i1);
          Int3 vertex2 = this.GetVertex((i1 + 1) % vertexCount1);
          int index2 = index1 != 2 ? 2 : 0;
          int val1_1 = Math.Min(vertex1[index2], vertex2[index2]);
          int val1_2 = Math.Max(vertex1[index2], vertex2[index2]);
          int num1 = Math.Max(val1_1, Math.Min(triangleMeshNode.GetVertex(i2)[index2], triangleMeshNode.GetVertex((i2 + 1) % vertexCount2)[index2]));
          int num2 = Math.Min(val1_2, Math.Max(triangleMeshNode.GetVertex(i2)[index2], triangleMeshNode.GetVertex((i2 + 1) % vertexCount2)[index2]));
          if (vertex1[index2] < vertex2[index2])
          {
            vertex1[index2] = num1;
            vertex2[index2] = num2;
          }
          else
          {
            vertex1[index2] = num2;
            vertex2[index2] = num1;
          }
          if (left != null)
          {
            left.Add((Vector3) vertex1);
            right.Add((Vector3) vertex2);
          }
          return true;
        }
      }
      else if (!backwards)
      {
        int i1 = -1;
        int num = -1;
        int vertexCount1 = this.GetVertexCount();
        int vertexCount2 = triangleMeshNode.GetVertexCount();
        for (int i2 = 0; i2 < vertexCount1; ++i2)
        {
          int vertexIndex = this.GetVertexIndex(i2);
          for (int i3 = 0; i3 < vertexCount2; ++i3)
          {
            if (vertexIndex == triangleMeshNode.GetVertexIndex((i3 + 1) % vertexCount2) && this.GetVertexIndex((i2 + 1) % vertexCount1) == triangleMeshNode.GetVertexIndex(i3))
            {
              i1 = i2;
              num = i3;
              i2 = vertexCount1;
              break;
            }
          }
        }
        aIndex = i1;
        bIndex = num;
        if (i1 != -1)
        {
          if (left != null)
          {
            left.Add((Vector3) this.GetVertex(i1));
            right.Add((Vector3) this.GetVertex((i1 + 1) % vertexCount1));
          }
        }
        else
        {
          for (int index = 0; index < this.connections.Length; ++index)
          {
            if ((int) this.connections[index].GraphIndex != (int) this.GraphIndex)
            {
              NodeLink3Node nodeLink3Node = this.connections[index] as NodeLink3Node;
              if (nodeLink3Node != null && nodeLink3Node.GetOther((GraphNode) this) == triangleMeshNode && left != null)
              {
                nodeLink3Node.GetPortal((GraphNode) triangleMeshNode, left, right, false);
                return true;
              }
            }
          }
          return false;
        }
      }
      return true;
    }

    public override void SerializeNode(GraphSerializationContext ctx)
    {
      base.SerializeNode(ctx);
      ctx.writer.Write(this.v0);
      ctx.writer.Write(this.v1);
      ctx.writer.Write(this.v2);
    }

    public override void DeserializeNode(GraphSerializationContext ctx)
    {
      base.DeserializeNode(ctx);
      this.v0 = ctx.reader.ReadInt32();
      this.v1 = ctx.reader.ReadInt32();
      this.v2 = ctx.reader.ReadInt32();
    }
  }
}
