// Decompiled with JetBrains decompiler
// Type: Pathfinding.BBTree
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

namespace Pathfinding
{
  public class BBTree
  {
    private BBTree.BBTreeBox[] arr = new BBTree.BBTreeBox[6];
    private int count;
    public INavmeshHolder graph;

    public Rect Size
    {
      get
      {
        if (this.count != 0)
          return this.arr[0].rect;
        return new Rect(0.0f, 0.0f, 0.0f, 0.0f);
      }
    }

    public BBTree(INavmeshHolder graph)
    {
      this.graph = graph;
    }

    public void Clear()
    {
      this.count = 0;
    }

    private void EnsureCapacity(int c)
    {
      if (this.arr.Length >= c)
        return;
      BBTree.BBTreeBox[] bbTreeBoxArray = new BBTree.BBTreeBox[Math.Max(c, (int) ((double) this.arr.Length * 1.5))];
      for (int index = 0; index < this.count; ++index)
        bbTreeBoxArray[index] = this.arr[index];
      this.arr = bbTreeBoxArray;
    }

    private int GetBox(MeshNode node)
    {
      if (this.count >= this.arr.Length)
        this.EnsureCapacity(this.count + 1);
      this.arr[this.count] = new BBTree.BBTreeBox(this, node);
      ++this.count;
      return this.count - 1;
    }

    public void Insert(MeshNode node)
    {
      int box1 = this.GetBox(node);
      if (box1 == 0)
        return;
      BBTree.BBTreeBox bbTreeBox1 = this.arr[box1];
      int index = 0;
      BBTree.BBTreeBox bbTreeBox2;
      while (true)
      {
        bbTreeBox2 = this.arr[index];
        bbTreeBox2.rect = BBTree.ExpandToContain(bbTreeBox2.rect, bbTreeBox1.rect);
        if (bbTreeBox2.node == null)
        {
          this.arr[index] = bbTreeBox2;
          float num1 = BBTree.ExpansionRequired(this.arr[bbTreeBox2.left].rect, bbTreeBox1.rect);
          float num2 = BBTree.ExpansionRequired(this.arr[bbTreeBox2.right].rect, bbTreeBox1.rect);
          index = (double) num1 >= (double) num2 ? ((double) num2 >= (double) num1 ? ((double) BBTree.RectArea(this.arr[bbTreeBox2.left].rect) >= (double) BBTree.RectArea(this.arr[bbTreeBox2.right].rect) ? bbTreeBox2.right : bbTreeBox2.left) : bbTreeBox2.right) : bbTreeBox2.left;
        }
        else
          break;
      }
      bbTreeBox2.left = box1;
      int box2 = this.GetBox(bbTreeBox2.node);
      bbTreeBox2.right = box2;
      bbTreeBox2.node = (MeshNode) null;
      this.arr[index] = bbTreeBox2;
    }

    public NNInfo Query(Vector3 p, NNConstraint constraint)
    {
      if (this.count == 0)
        return new NNInfo((GraphNode) null);
      NNInfo nnInfo = new NNInfo();
      this.SearchBox(0, p, constraint, ref nnInfo);
      nnInfo.UpdateInfo();
      return nnInfo;
    }

    public NNInfo QueryCircle(Vector3 p, float radius, NNConstraint constraint)
    {
      if (this.count == 0)
        return new NNInfo((GraphNode) null);
      NNInfo nnInfo = new NNInfo((GraphNode) null);
      this.SearchBoxCircle(0, p, radius, constraint, ref nnInfo);
      nnInfo.UpdateInfo();
      return nnInfo;
    }

    public NNInfo QueryClosest(Vector3 p, NNConstraint constraint, out float distance)
    {
      distance = float.PositiveInfinity;
      return this.QueryClosest(p, constraint, ref distance, new NNInfo((GraphNode) null));
    }

    public NNInfo QueryClosestXZ(Vector3 p, NNConstraint constraint, ref float distance, NNInfo previous)
    {
      if (this.count == 0)
        return previous;
      this.SearchBoxClosestXZ(0, p, ref distance, constraint, ref previous);
      return previous;
    }

    private void SearchBoxClosestXZ(int boxi, Vector3 p, ref float closestDist, NNConstraint constraint, ref NNInfo nnInfo)
    {
      BBTree.BBTreeBox bbTreeBox = this.arr[boxi];
      if (bbTreeBox.node != null)
      {
        Vector3 vector3 = bbTreeBox.node.ClosestPointOnNodeXZ(p);
        float num = (float) (((double) vector3.x - (double) p.x) * ((double) vector3.x - (double) p.x) + ((double) vector3.z - (double) p.z) * ((double) vector3.z - (double) p.z));
        if (constraint != null && !constraint.Suitable((GraphNode) bbTreeBox.node))
          return;
        if (nnInfo.constrainedNode == null)
        {
          nnInfo.constrainedNode = (GraphNode) bbTreeBox.node;
          nnInfo.constClampedPosition = vector3;
          closestDist = (float) Math.Sqrt((double) num);
        }
        else
        {
          if ((double) num >= (double) closestDist * (double) closestDist)
            return;
          nnInfo.constrainedNode = (GraphNode) bbTreeBox.node;
          nnInfo.constClampedPosition = vector3;
          closestDist = (float) Math.Sqrt((double) num);
        }
      }
      else
      {
        if (BBTree.RectIntersectsCircle(this.arr[bbTreeBox.left].rect, p, closestDist))
          this.SearchBoxClosestXZ(bbTreeBox.left, p, ref closestDist, constraint, ref nnInfo);
        if (!BBTree.RectIntersectsCircle(this.arr[bbTreeBox.right].rect, p, closestDist))
          return;
        this.SearchBoxClosestXZ(bbTreeBox.right, p, ref closestDist, constraint, ref nnInfo);
      }
    }

    public NNInfo QueryClosest(Vector3 p, NNConstraint constraint, ref float distance, NNInfo previous)
    {
      if (this.count == 0)
        return previous;
      this.SearchBoxClosest(0, p, ref distance, constraint, ref previous);
      return previous;
    }

    private void SearchBoxClosest(int boxi, Vector3 p, ref float closestDist, NNConstraint constraint, ref NNInfo nnInfo)
    {
      BBTree.BBTreeBox bbTreeBox = this.arr[boxi];
      if (bbTreeBox.node != null)
      {
        if (!BBTree.NodeIntersectsCircle(bbTreeBox.node, p, closestDist))
          return;
        Vector3 vector3 = bbTreeBox.node.ClosestPointOnNode(p);
        float sqrMagnitude = (vector3 - p).sqrMagnitude;
        if (constraint != null && !constraint.Suitable((GraphNode) bbTreeBox.node))
          return;
        if (nnInfo.constrainedNode == null)
        {
          nnInfo.constrainedNode = (GraphNode) bbTreeBox.node;
          nnInfo.constClampedPosition = vector3;
          closestDist = (float) Math.Sqrt((double) sqrMagnitude);
        }
        else
        {
          if ((double) sqrMagnitude >= (double) closestDist * (double) closestDist)
            return;
          nnInfo.constrainedNode = (GraphNode) bbTreeBox.node;
          nnInfo.constClampedPosition = vector3;
          closestDist = (float) Math.Sqrt((double) sqrMagnitude);
        }
      }
      else
      {
        if (BBTree.RectIntersectsCircle(this.arr[bbTreeBox.left].rect, p, closestDist))
          this.SearchBoxClosest(bbTreeBox.left, p, ref closestDist, constraint, ref nnInfo);
        if (!BBTree.RectIntersectsCircle(this.arr[bbTreeBox.right].rect, p, closestDist))
          return;
        this.SearchBoxClosest(bbTreeBox.right, p, ref closestDist, constraint, ref nnInfo);
      }
    }

    public MeshNode QueryInside(Vector3 p, NNConstraint constraint)
    {
      if (this.count == 0)
        return (MeshNode) null;
      return this.SearchBoxInside(0, p, constraint);
    }

    private MeshNode SearchBoxInside(int boxi, Vector3 p, NNConstraint constraint)
    {
      BBTree.BBTreeBox bbTreeBox = this.arr[boxi];
      if (bbTreeBox.node != null)
      {
        if (bbTreeBox.node.ContainsPoint((Int3) p) && (constraint == null || constraint.Suitable((GraphNode) bbTreeBox.node)))
          return bbTreeBox.node;
      }
      else
      {
        if (this.arr[bbTreeBox.left].rect.Contains(new Vector2(p.x, p.z)))
        {
          MeshNode meshNode = this.SearchBoxInside(bbTreeBox.left, p, constraint);
          if (meshNode != null)
            return meshNode;
        }
        if (this.arr[bbTreeBox.right].rect.Contains(new Vector2(p.x, p.z)))
        {
          MeshNode meshNode = this.SearchBoxInside(bbTreeBox.right, p, constraint);
          if (meshNode != null)
            return meshNode;
        }
      }
      return (MeshNode) null;
    }

    private void SearchBoxCircle(int boxi, Vector3 p, float radius, NNConstraint constraint, ref NNInfo nnInfo)
    {
      BBTree.BBTreeBox bbTreeBox = this.arr[boxi];
      if (bbTreeBox.node != null)
      {
        if (!BBTree.NodeIntersectsCircle(bbTreeBox.node, p, radius))
          return;
        Vector3 vector3 = bbTreeBox.node.ClosestPointOnNode(p);
        float sqrMagnitude = (vector3 - p).sqrMagnitude;
        if (nnInfo.node == null)
        {
          nnInfo.node = (GraphNode) bbTreeBox.node;
          nnInfo.clampedPosition = vector3;
        }
        else if ((double) sqrMagnitude < (double) (nnInfo.clampedPosition - p).sqrMagnitude)
        {
          nnInfo.node = (GraphNode) bbTreeBox.node;
          nnInfo.clampedPosition = vector3;
        }
        if (constraint != null && !constraint.Suitable((GraphNode) bbTreeBox.node))
          return;
        if (nnInfo.constrainedNode == null)
        {
          nnInfo.constrainedNode = (GraphNode) bbTreeBox.node;
          nnInfo.constClampedPosition = vector3;
        }
        else
        {
          if ((double) sqrMagnitude >= (double) (nnInfo.constClampedPosition - p).sqrMagnitude)
            return;
          nnInfo.constrainedNode = (GraphNode) bbTreeBox.node;
          nnInfo.constClampedPosition = vector3;
        }
      }
      else
      {
        if (BBTree.RectIntersectsCircle(this.arr[bbTreeBox.left].rect, p, radius))
          this.SearchBoxCircle(bbTreeBox.left, p, radius, constraint, ref nnInfo);
        if (!BBTree.RectIntersectsCircle(this.arr[bbTreeBox.right].rect, p, radius))
          return;
        this.SearchBoxCircle(bbTreeBox.right, p, radius, constraint, ref nnInfo);
      }
    }

    private void SearchBox(int boxi, Vector3 p, NNConstraint constraint, ref NNInfo nnInfo)
    {
      BBTree.BBTreeBox bbTreeBox = this.arr[boxi];
      if (bbTreeBox.node != null)
      {
        if (!bbTreeBox.node.ContainsPoint((Int3) p))
          return;
        if (nnInfo.node == null)
          nnInfo.node = (GraphNode) bbTreeBox.node;
        else if ((double) Mathf.Abs((Vector3) bbTreeBox.node.position.y - p.y) < (double) Mathf.Abs((Vector3) nnInfo.node.position.y - p.y))
          nnInfo.node = (GraphNode) bbTreeBox.node;
        if (!constraint.Suitable((GraphNode) bbTreeBox.node))
          return;
        if (nnInfo.constrainedNode == null)
        {
          nnInfo.constrainedNode = (GraphNode) bbTreeBox.node;
        }
        else
        {
          if ((double) Mathf.Abs((float) bbTreeBox.node.position.y - p.y) >= (double) Mathf.Abs((float) nnInfo.constrainedNode.position.y - p.y))
            return;
          nnInfo.constrainedNode = (GraphNode) bbTreeBox.node;
        }
      }
      else
      {
        if (BBTree.RectContains(this.arr[bbTreeBox.left].rect, p))
          this.SearchBox(bbTreeBox.left, p, constraint, ref nnInfo);
        if (!BBTree.RectContains(this.arr[bbTreeBox.right].rect, p))
          return;
        this.SearchBox(bbTreeBox.right, p, constraint, ref nnInfo);
      }
    }

    public void OnDrawGizmos()
    {
      Gizmos.color = new Color(1f, 1f, 1f, 0.5f);
      if (this.count == 0)
        ;
    }

    private void OnDrawGizmos(int boxi, int depth)
    {
      BBTree.BBTreeBox bbTreeBox = this.arr[boxi];
      Vector3 vector3_1 = new Vector3(bbTreeBox.rect.xMin, 0.0f, bbTreeBox.rect.yMin);
      Vector3 vector3_2 = new Vector3(bbTreeBox.rect.xMax, 0.0f, bbTreeBox.rect.yMax);
      Vector3 center = (vector3_1 + vector3_2) * 0.5f;
      Vector3 size = (vector3_2 - center) * 2f;
      center.y += (float) depth * 0.2f;
      Gizmos.color = AstarMath.IntToColor(depth, 0.05f);
      Gizmos.DrawCube(center, size);
      if (bbTreeBox.node != null)
        return;
      this.OnDrawGizmos(bbTreeBox.left, depth + 1);
      this.OnDrawGizmos(bbTreeBox.right, depth + 1);
    }

    private static bool NodeIntersectsCircle(MeshNode node, Vector3 p, float radius)
    {
      if (float.IsPositiveInfinity(radius))
        return true;
      return (double) (p - node.ClosestPointOnNode(p)).sqrMagnitude < (double) radius * (double) radius;
    }

    private static bool RectIntersectsCircle(Rect r, Vector3 p, float radius)
    {
      if (float.IsPositiveInfinity(radius))
        return true;
      Vector3 vector3 = p;
      p.x = Math.Max(p.x, r.xMin);
      p.x = Math.Min(p.x, r.xMax);
      p.z = Math.Max(p.z, r.yMin);
      p.z = Math.Min(p.z, r.yMax);
      return ((double) p.x - (double) vector3.x) * ((double) p.x - (double) vector3.x) + ((double) p.z - (double) vector3.z) * ((double) p.z - (double) vector3.z) < (double) radius * (double) radius;
    }

    private static bool RectContains(Rect r, Vector3 p)
    {
      if ((double) p.x >= (double) r.xMin && (double) p.x <= (double) r.xMax && (double) p.z >= (double) r.yMin)
        return (double) p.z <= (double) r.yMax;
      return false;
    }

    private static float ExpansionRequired(Rect r, Rect r2)
    {
      float num1 = Math.Min(r.xMin, r2.xMin);
      float num2 = Math.Max(r.xMax, r2.xMax);
      float num3 = Math.Min(r.yMin, r2.yMin);
      float num4 = Math.Max(r.yMax, r2.yMax);
      return (float) (((double) num2 - (double) num1) * ((double) num4 - (double) num3)) - BBTree.RectArea(r);
    }

    private static Rect ExpandToContain(Rect r, Rect r2)
    {
      float xmin = Math.Min(r.xMin, r2.xMin);
      float xmax = Math.Max(r.xMax, r2.xMax);
      float ymin = Math.Min(r.yMin, r2.yMin);
      float ymax = Math.Max(r.yMax, r2.yMax);
      return Rect.MinMaxRect(xmin, ymin, xmax, ymax);
    }

    private static float RectArea(Rect r)
    {
      return r.width * r.height;
    }

    private struct BBTreeBox
    {
      public Rect rect;
      public MeshNode node;
      public int left;
      public int right;

      public bool IsLeaf
      {
        get
        {
          return this.node != null;
        }
      }

      public BBTreeBox(BBTree tree, MeshNode node)
      {
        this.node = node;
        Vector3 vector3_1 = (Vector3) node.GetVertex(0);
        Vector2 vector2_1 = new Vector2(vector3_1.x, vector3_1.z);
        Vector2 vector2_2 = vector2_1;
        for (int i = 1; i < node.GetVertexCount(); ++i)
        {
          Vector3 vector3_2 = (Vector3) node.GetVertex(i);
          vector2_1.x = Math.Min(vector2_1.x, vector3_2.x);
          vector2_1.y = Math.Min(vector2_1.y, vector3_2.z);
          vector2_2.x = Math.Max(vector2_2.x, vector3_2.x);
          vector2_2.y = Math.Max(vector2_2.y, vector3_2.z);
        }
        this.rect = Rect.MinMaxRect(vector2_1.x, vector2_1.y, vector2_2.x, vector2_2.y);
        this.left = this.right = -1;
      }

      public bool Contains(Vector3 p)
      {
        return this.rect.Contains(new Vector2(p.x, p.z));
      }
    }
  }
}
