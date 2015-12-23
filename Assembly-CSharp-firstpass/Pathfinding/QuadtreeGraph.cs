// Decompiled with JetBrains decompiler
// Type: Pathfinding.QuadtreeGraph
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
  public class QuadtreeGraph : NavGraph
  {
    public int editorWidthLog2 = 6;
    public int editorHeightLog2 = 6;
    public LayerMask layerMask = (LayerMask) -1;
    public float nodeSize = 1f;
    public int minDepth = 3;
    private QuadtreeNodeHolder root;
    public Vector3 center;
    private BitArray map;

    public int Width { get; protected set; }

    public int Height { get; protected set; }

    public override void GetNodes(GraphNodeDelegateCancelable del)
    {
      if (this.root == null)
        return;
      this.root.GetNodes(del);
    }

    public bool CheckCollision(int x, int y)
    {
      return !Physics.CheckSphere(this.LocalToWorldPosition(x, y, 1), this.nodeSize * 1.4142f, (int) this.layerMask);
    }

    public int CheckNode(int xs, int ys, int width)
    {
      Debug.Log((object) ("Checking Node " + (object) xs + " " + (string) (object) ys + " width: " + (string) (object) width));
      bool flag = this.map[xs + ys * this.Width];
      for (int index1 = xs; index1 < xs + width; ++index1)
      {
        for (int index2 = ys; index2 < ys + width; ++index2)
        {
          if (this.map[index1 + index2 * this.Width] != flag)
            return -1;
        }
      }
      return flag ? 1 : 0;
    }

    public override void ScanInternal(OnScanStatus statusCallback)
    {
      this.Width = 1 << this.editorWidthLog2;
      this.Height = 1 << this.editorHeightLog2;
      this.map = new BitArray(this.Width * this.Height);
      for (int x = 0; x < this.Width; ++x)
      {
        for (int y = 0; y < this.Height; ++y)
          this.map.Set(x + y * this.Width, this.CheckCollision(x, y));
      }
      QuadtreeNodeHolder holder = new QuadtreeNodeHolder();
      this.CreateNodeRec(holder, 0, 0, 0);
      this.root = holder;
      this.RecalculateConnectionsRec(this.root, 0, 0, 0);
    }

    public void RecalculateConnectionsRec(QuadtreeNodeHolder holder, int depth, int x, int y)
    {
      if (holder.node != null)
      {
        this.RecalculateConnections(holder, depth, x, y);
      }
      else
      {
        int num = 1 << Math.Min(this.editorHeightLog2, this.editorWidthLog2) - depth;
        this.RecalculateConnectionsRec(holder.c0, depth + 1, x, y);
        this.RecalculateConnectionsRec(holder.c1, depth + 1, x + num / 2, y);
        this.RecalculateConnectionsRec(holder.c2, depth + 1, x + num / 2, y + num / 2);
        this.RecalculateConnectionsRec(holder.c3, depth + 1, x, y + num / 2);
      }
    }

    public Vector3 LocalToWorldPosition(int x, int y, int width)
    {
      return new Vector3(((float) x + (float) width * 0.5f) * this.nodeSize, 0.0f, ((float) y + (float) width * 0.5f) * this.nodeSize);
    }

    public void CreateNodeRec(QuadtreeNodeHolder holder, int depth, int x, int y)
    {
      int width = 1 << Math.Min(this.editorHeightLog2, this.editorWidthLog2) - depth;
      int num = depth >= this.minDepth ? this.CheckNode(x, y, width) : -1;
      if (num == 1 || num == 0 || width == 1)
      {
        QuadtreeNode quadtreeNode = new QuadtreeNode(this.active);
        quadtreeNode.SetPosition((Int3) this.LocalToWorldPosition(x, y, width));
        quadtreeNode.Walkable = num == 1;
        holder.node = quadtreeNode;
      }
      else
      {
        holder.c0 = new QuadtreeNodeHolder();
        holder.c1 = new QuadtreeNodeHolder();
        holder.c2 = new QuadtreeNodeHolder();
        holder.c3 = new QuadtreeNodeHolder();
        this.CreateNodeRec(holder.c0, depth + 1, x, y);
        this.CreateNodeRec(holder.c1, depth + 1, x + width / 2, y);
        this.CreateNodeRec(holder.c2, depth + 1, x + width / 2, y + width / 2);
        this.CreateNodeRec(holder.c3, depth + 1, x, y + width / 2);
      }
    }

    public void RecalculateConnections(QuadtreeNodeHolder holder, int depth, int x, int y)
    {
      if (this.root == null)
        throw new InvalidOperationException("Graph contains no nodes");
      if (holder.node == null)
        throw new ArgumentException("No leaf node specified. Holder has no node.");
      int num1 = 1 << Math.Min(this.editorHeightLog2, this.editorWidthLog2) - depth;
      List<QuadtreeNode> arr = new List<QuadtreeNode>();
      this.AddNeighboursRec(arr, this.root, 0, 0, 0, new IntRect(x, y, x + num1, y + num1).Expand(0), holder.node);
      holder.node.connections = (GraphNode[]) arr.ToArray();
      holder.node.connectionCosts = new uint[arr.Count];
      for (int index = 0; index < arr.Count; ++index)
      {
        uint num2 = (uint) (arr[index].position - holder.node.position).costMagnitude;
        holder.node.connectionCosts[index] = num2;
      }
    }

    public void AddNeighboursRec(List<QuadtreeNode> arr, QuadtreeNodeHolder holder, int depth, int x, int y, IntRect bounds, QuadtreeNode dontInclude)
    {
      int num = 1 << Math.Min(this.editorHeightLog2, this.editorWidthLog2) - depth;
      if (!IntRect.Intersects(new IntRect(x, y, x + num, y + num), bounds))
        return;
      if (holder.node != null)
      {
        if (holder.node == dontInclude)
          return;
        arr.Add(holder.node);
      }
      else
      {
        this.AddNeighboursRec(arr, holder.c0, depth + 1, x, y, bounds, dontInclude);
        this.AddNeighboursRec(arr, holder.c1, depth + 1, x + num / 2, y, bounds, dontInclude);
        this.AddNeighboursRec(arr, holder.c2, depth + 1, x + num / 2, y + num / 2, bounds, dontInclude);
        this.AddNeighboursRec(arr, holder.c3, depth + 1, x, y + num / 2, bounds, dontInclude);
      }
    }

    public QuadtreeNode QueryPoint(int qx, int qy)
    {
      if (this.root == null)
        return (QuadtreeNode) null;
      QuadtreeNodeHolder quadtreeNodeHolder = this.root;
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      while (quadtreeNodeHolder.node == null)
      {
        int num4 = 1 << Math.Min(this.editorHeightLog2, this.editorWidthLog2) - num1;
        if (qx >= num2 + num4 / 2)
        {
          num2 += num4 / 2;
          if (qy >= num3 + num4 / 2)
          {
            num3 += num4 / 2;
            quadtreeNodeHolder = quadtreeNodeHolder.c2;
          }
          else
            quadtreeNodeHolder = quadtreeNodeHolder.c1;
        }
        else if (qy >= num3 + num4 / 2)
        {
          num3 += num4 / 2;
          quadtreeNodeHolder = quadtreeNodeHolder.c3;
        }
        else
          quadtreeNodeHolder = quadtreeNodeHolder.c0;
        ++num1;
      }
      return quadtreeNodeHolder.node;
    }

    public override void OnDrawGizmos(bool drawNodes)
    {
      base.OnDrawGizmos(drawNodes);
      if (!drawNodes || this.root == null)
        return;
      this.DrawRec(this.root, 0, 0, 0, Vector3.zero);
    }

    public void DrawRec(QuadtreeNodeHolder h, int depth, int x, int y, Vector3 parentPos)
    {
      int width = 1 << Math.Min(this.editorHeightLog2, this.editorWidthLog2) - depth;
      Vector3 vector3 = this.LocalToWorldPosition(x, y, width);
      Debug.DrawLine(vector3, parentPos, Color.red);
      if (h.node != null)
      {
        Debug.DrawRay(vector3, Vector3.down, !h.node.Walkable ? Color.yellow : Color.green);
      }
      else
      {
        this.DrawRec(h.c0, depth + 1, x, y, vector3);
        this.DrawRec(h.c1, depth + 1, x + width / 2, y, vector3);
        this.DrawRec(h.c2, depth + 1, x + width / 2, y + width / 2, vector3);
        this.DrawRec(h.c3, depth + 1, x, y + width / 2, vector3);
      }
    }
  }
}
