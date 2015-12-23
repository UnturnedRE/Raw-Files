// Decompiled with JetBrains decompiler
// Type: Pathfinding.RVO.RVOQuadtree
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding.RVO.Sampled;
using System;
using UnityEngine;

namespace Pathfinding.RVO
{
  public class RVOQuadtree
  {
    private RVOQuadtree.Node[] nodes = new RVOQuadtree.Node[42];
    private int filledNodes = 1;
    private const int LeafSize = 15;
    private float maxRadius;
    private Rect bounds;

    public void Clear()
    {
      this.nodes[0] = new RVOQuadtree.Node();
      this.filledNodes = 1;
      this.maxRadius = 0.0f;
    }

    public void SetBounds(Rect r)
    {
      this.bounds = r;
    }

    public int GetNodeIndex()
    {
      if (this.filledNodes == this.nodes.Length)
      {
        RVOQuadtree.Node[] nodeArray = new RVOQuadtree.Node[this.nodes.Length * 2];
        for (int index = 0; index < this.nodes.Length; ++index)
          nodeArray[index] = this.nodes[index];
        this.nodes = nodeArray;
      }
      this.nodes[this.filledNodes] = new RVOQuadtree.Node();
      this.nodes[this.filledNodes].child00 = this.filledNodes;
      ++this.filledNodes;
      return this.filledNodes - 1;
    }

    public void Insert(Agent agent)
    {
      int index = 0;
      Rect r = this.bounds;
      Vector2 vector2 = new Vector2(agent.position.x, agent.position.z);
      agent.next = (Agent) null;
      this.maxRadius = Math.Max(agent.radius, this.maxRadius);
      int num = 0;
      while (true)
      {
        do
        {
          ++num;
          if (this.nodes[index].child00 == index)
          {
            if ((int) this.nodes[index].count < 15 || num > 10)
            {
              this.nodes[index].Add(agent);
              ++this.nodes[index].count;
              return;
            }
            RVOQuadtree.Node node = this.nodes[index];
            node.child00 = this.GetNodeIndex();
            node.child01 = this.GetNodeIndex();
            node.child10 = this.GetNodeIndex();
            node.child11 = this.GetNodeIndex();
            this.nodes[index] = node;
            this.nodes[index].Distribute(this.nodes, r);
          }
        }
        while (this.nodes[index].child00 == index);
        Vector2 center = r.center;
        if ((double) vector2.x > (double) center.x)
        {
          if ((double) vector2.y > (double) center.y)
          {
            index = this.nodes[index].child11;
            r = Rect.MinMaxRect(center.x, center.y, r.xMax, r.yMax);
          }
          else
          {
            index = this.nodes[index].child10;
            r = Rect.MinMaxRect(center.x, r.yMin, r.xMax, center.y);
          }
        }
        else if ((double) vector2.y > (double) center.y)
        {
          index = this.nodes[index].child01;
          r = Rect.MinMaxRect(r.xMin, center.y, center.x, r.yMax);
        }
        else
        {
          index = this.nodes[index].child00;
          r = Rect.MinMaxRect(r.xMin, r.yMin, center.x, center.y);
        }
      }
    }

    public void Query(Vector2 p, float radius, Agent agent)
    {
      double num = (double) this.QueryRec(0, p, radius, agent, this.bounds);
    }

    private float QueryRec(int i, Vector2 p, float radius, Agent agent, Rect r)
    {
      if (this.nodes[i].child00 == i)
      {
        for (Agent agent1 = this.nodes[i].linkedList; agent1 != null; agent1 = agent1.next)
        {
          float f = agent.InsertAgentNeighbour(agent1, radius * radius);
          if ((double) f < (double) radius * (double) radius)
            radius = Mathf.Sqrt(f);
        }
      }
      else
      {
        Vector2 center = r.center;
        if ((double) p.x - (double) radius < (double) center.x)
        {
          if ((double) p.y - (double) radius < (double) center.y)
            radius = this.QueryRec(this.nodes[i].child00, p, radius, agent, Rect.MinMaxRect(r.xMin, r.yMin, center.x, center.y));
          if ((double) p.y + (double) radius > (double) center.y)
            radius = this.QueryRec(this.nodes[i].child01, p, radius, agent, Rect.MinMaxRect(r.xMin, center.y, center.x, r.yMax));
        }
        if ((double) p.x + (double) radius > (double) center.x)
        {
          if ((double) p.y - (double) radius < (double) center.y)
            radius = this.QueryRec(this.nodes[i].child10, p, radius, agent, Rect.MinMaxRect(center.x, r.yMin, r.xMax, center.y));
          if ((double) p.y + (double) radius > (double) center.y)
            radius = this.QueryRec(this.nodes[i].child11, p, radius, agent, Rect.MinMaxRect(center.x, center.y, r.xMax, r.yMax));
        }
      }
      return radius;
    }

    public void DebugDraw()
    {
      this.DebugDrawRec(0, this.bounds);
    }

    private void DebugDrawRec(int i, Rect r)
    {
      Debug.DrawLine(new Vector3(r.xMin, 0.0f, r.yMin), new Vector3(r.xMax, 0.0f, r.yMin), Color.white);
      Debug.DrawLine(new Vector3(r.xMax, 0.0f, r.yMin), new Vector3(r.xMax, 0.0f, r.yMax), Color.white);
      Debug.DrawLine(new Vector3(r.xMax, 0.0f, r.yMax), new Vector3(r.xMin, 0.0f, r.yMax), Color.white);
      Debug.DrawLine(new Vector3(r.xMin, 0.0f, r.yMax), new Vector3(r.xMin, 0.0f, r.yMin), Color.white);
      if (this.nodes[i].child00 != i)
      {
        Vector2 center = r.center;
        this.DebugDrawRec(this.nodes[i].child11, Rect.MinMaxRect(center.x, center.y, r.xMax, r.yMax));
        this.DebugDrawRec(this.nodes[i].child10, Rect.MinMaxRect(center.x, r.yMin, r.xMax, center.y));
        this.DebugDrawRec(this.nodes[i].child01, Rect.MinMaxRect(r.xMin, center.y, center.x, r.yMax));
        this.DebugDrawRec(this.nodes[i].child00, Rect.MinMaxRect(r.xMin, r.yMin, center.x, center.y));
      }
      for (Agent agent = this.nodes[i].linkedList; agent != null; agent = agent.next)
        Debug.DrawLine(this.nodes[i].linkedList.position + Vector3.up, agent.position + Vector3.up, new Color(1f, 1f, 0.0f, 0.5f));
    }

    private struct Node
    {
      public int child00;
      public int child01;
      public int child10;
      public int child11;
      public byte count;
      public Agent linkedList;

      public void Add(Agent agent)
      {
        agent.next = this.linkedList;
        this.linkedList = agent;
      }

      public void Distribute(RVOQuadtree.Node[] nodes, Rect r)
      {
        Vector2 center = r.center;
        Agent agent;
        for (; this.linkedList != null; this.linkedList = agent)
        {
          agent = this.linkedList.next;
          if ((double) this.linkedList.position.x > (double) center.x)
          {
            if ((double) this.linkedList.position.z > (double) center.y)
              nodes[this.child11].Add(this.linkedList);
            else
              nodes[this.child10].Add(this.linkedList);
          }
          else if ((double) this.linkedList.position.z > (double) center.y)
            nodes[this.child01].Add(this.linkedList);
          else
            nodes[this.child00].Add(this.linkedList);
        }
        this.count = (byte) 0;
      }
    }
  }
}
