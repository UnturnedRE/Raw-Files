// Decompiled with JetBrains decompiler
// Type: Pathfinding.PointGraph
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding.Serialization;
using Pathfinding.Serialization.JsonFx;
using Pathfinding.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
  [JsonOptIn]
  public class PointGraph : NavGraph, IUpdatableGraph
  {
    private static readonly Int3[] ThreeDNeighbours = new Int3[27]
    {
      new Int3(-1, 0, -1),
      new Int3(0, 0, -1),
      new Int3(1, 0, -1),
      new Int3(-1, 0, 0),
      new Int3(0, 0, 0),
      new Int3(1, 0, 0),
      new Int3(-1, 0, 1),
      new Int3(0, 0, 1),
      new Int3(1, 0, 1),
      new Int3(-1, -1, -1),
      new Int3(0, -1, -1),
      new Int3(1, -1, -1),
      new Int3(-1, -1, 0),
      new Int3(0, -1, 0),
      new Int3(1, -1, 0),
      new Int3(-1, -1, 1),
      new Int3(0, -1, 1),
      new Int3(1, -1, 1),
      new Int3(-1, 1, -1),
      new Int3(0, 1, -1),
      new Int3(1, 1, -1),
      new Int3(-1, 1, 0),
      new Int3(0, 1, 0),
      new Int3(1, 1, 0),
      new Int3(-1, 1, 1),
      new Int3(0, 1, 1),
      new Int3(1, 1, 1)
    };
    [JsonMember]
    public bool raycast = true;
    [JsonMember]
    public float thickRaycastRadius = 1f;
    [JsonMember]
    public bool recursive = true;
    [JsonMember]
    public bool autoLinkNodes = true;
    [JsonMember]
    public Transform root;
    [JsonMember]
    public string searchTag;
    [JsonMember]
    public float maxDistance;
    [JsonMember]
    public Vector3 limits;
    [JsonMember]
    public bool use2DPhysics;
    [JsonMember]
    public bool thickRaycast;
    [JsonMember]
    public LayerMask mask;
    [JsonMember]
    public bool optimizeForSparseGraph;
    [JsonMember]
    public bool optimizeFor2D;
    private Dictionary<Int3, PointNode> nodeLookup;
    private Int3 minLookup;
    private Int3 maxLookup;
    private Int3 lookupCellSize;
    public PointNode[] nodes;
    public int nodeCount;

    private Int3 WorldToLookupSpace(Int3 p)
    {
      Int3 zero = Int3.zero;
      zero.x = this.lookupCellSize.x == 0 ? 0 : p.x / this.lookupCellSize.x;
      zero.y = this.lookupCellSize.y == 0 ? 0 : p.y / this.lookupCellSize.y;
      zero.z = this.lookupCellSize.z == 0 ? 0 : p.z / this.lookupCellSize.z;
      return zero;
    }

    public override void GetNodes(GraphNodeDelegateCancelable del)
    {
      if (this.nodes == null)
        return;
      int index = 0;
      while (index < this.nodeCount && del((GraphNode) this.nodes[index]))
        ++index;
    }

    public override NNInfo GetNearest(Vector3 position, NNConstraint constraint, GraphNode hint)
    {
      return this.GetNearestForce(position, constraint);
    }

    public override NNInfo GetNearestForce(Vector3 position, NNConstraint constraint)
    {
      if (this.nodes == null)
        return new NNInfo();
      float num1 = !constraint.constrainDistance ? float.PositiveInfinity : AstarPath.active.maxNearestNodeDistanceSqr;
      float num2 = float.PositiveInfinity;
      GraphNode node = (GraphNode) null;
      float num3 = float.PositiveInfinity;
      GraphNode graphNode = (GraphNode) null;
      if (this.optimizeForSparseGraph)
      {
        Int3 key = this.WorldToLookupSpace((Int3) position);
        Int3 int3_1 = key - this.minLookup;
        int val1_1 = Math.Max(Math.Max(Math.Max(0, Math.Abs(int3_1.x)), Math.Abs(int3_1.y)), Math.Abs(int3_1.z));
        int3_1 = key - this.maxLookup;
        int val1_2 = Math.Max(Math.Max(Math.Max(val1_1, Math.Abs(int3_1.x)), Math.Abs(int3_1.y)), Math.Abs(int3_1.z));
        PointNode pointNode = (PointNode) null;
        if (this.nodeLookup.TryGetValue(key, out pointNode))
        {
          for (; pointNode != null; pointNode = pointNode.next)
          {
            float sqrMagnitude = (position - (Vector3) pointNode.position).sqrMagnitude;
            if ((double) sqrMagnitude < (double) num2)
            {
              num2 = sqrMagnitude;
              node = (GraphNode) pointNode;
            }
            if (constraint == null || (double) sqrMagnitude < (double) num3 && (double) sqrMagnitude < (double) num1 && constraint.Suitable((GraphNode) pointNode))
            {
              num3 = sqrMagnitude;
              graphNode = (GraphNode) pointNode;
            }
          }
        }
        for (int index = 1; index <= val1_2; ++index)
        {
          if (index >= 20)
          {
            Debug.LogWarning((object) "Aborting GetNearest call at maximum distance because it has iterated too many times.\nIf you get this regularly, check your settings for PointGraph -> <b>Optimize For Sparse Graph</b> and PointGraph -> <b>Optimize For 2D</b>.\nThis happens when the closest node was very far away (20*link distance between nodes). When optimizing for sparse graphs, getting the nearest node from far away positions is <b>very slow</b>.\n");
            break;
          }
          if (this.lookupCellSize.y == 0)
          {
            Int3 int3_2 = key + new Int3(-index, 0, -index);
            for (int _x = 0; _x <= 2 * index; ++_x)
            {
              if (this.nodeLookup.TryGetValue(int3_2 + new Int3(_x, 0, 0), out pointNode))
              {
                for (; pointNode != null; pointNode = pointNode.next)
                {
                  float sqrMagnitude = (position - (Vector3) pointNode.position).sqrMagnitude;
                  if ((double) sqrMagnitude < (double) num2)
                  {
                    num2 = sqrMagnitude;
                    node = (GraphNode) pointNode;
                  }
                  if (constraint == null || (double) sqrMagnitude < (double) num3 && (double) sqrMagnitude < (double) num1 && constraint.Suitable((GraphNode) pointNode))
                  {
                    num3 = sqrMagnitude;
                    graphNode = (GraphNode) pointNode;
                  }
                }
              }
              if (this.nodeLookup.TryGetValue(int3_2 + new Int3(_x, 0, 2 * index), out pointNode))
              {
                for (; pointNode != null; pointNode = pointNode.next)
                {
                  float sqrMagnitude = (position - (Vector3) pointNode.position).sqrMagnitude;
                  if ((double) sqrMagnitude < (double) num2)
                  {
                    num2 = sqrMagnitude;
                    node = (GraphNode) pointNode;
                  }
                  if (constraint == null || (double) sqrMagnitude < (double) num3 && (double) sqrMagnitude < (double) num1 && constraint.Suitable((GraphNode) pointNode))
                  {
                    num3 = sqrMagnitude;
                    graphNode = (GraphNode) pointNode;
                  }
                }
              }
            }
            for (int _z = 1; _z < 2 * index; ++_z)
            {
              if (this.nodeLookup.TryGetValue(int3_2 + new Int3(0, 0, _z), out pointNode))
              {
                for (; pointNode != null; pointNode = pointNode.next)
                {
                  float sqrMagnitude = (position - (Vector3) pointNode.position).sqrMagnitude;
                  if ((double) sqrMagnitude < (double) num2)
                  {
                    num2 = sqrMagnitude;
                    node = (GraphNode) pointNode;
                  }
                  if (constraint == null || (double) sqrMagnitude < (double) num3 && (double) sqrMagnitude < (double) num1 && constraint.Suitable((GraphNode) pointNode))
                  {
                    num3 = sqrMagnitude;
                    graphNode = (GraphNode) pointNode;
                  }
                }
              }
              if (this.nodeLookup.TryGetValue(int3_2 + new Int3(2 * index, 0, _z), out pointNode))
              {
                for (; pointNode != null; pointNode = pointNode.next)
                {
                  float sqrMagnitude = (position - (Vector3) pointNode.position).sqrMagnitude;
                  if ((double) sqrMagnitude < (double) num2)
                  {
                    num2 = sqrMagnitude;
                    node = (GraphNode) pointNode;
                  }
                  if (constraint == null || (double) sqrMagnitude < (double) num3 && (double) sqrMagnitude < (double) num1 && constraint.Suitable((GraphNode) pointNode))
                  {
                    num3 = sqrMagnitude;
                    graphNode = (GraphNode) pointNode;
                  }
                }
              }
            }
          }
          else
          {
            Int3 int3_2 = key + new Int3(-index, -index, -index);
            for (int _x = 0; _x <= 2 * index; ++_x)
            {
              for (int _y = 0; _y <= 2 * index; ++_y)
              {
                if (this.nodeLookup.TryGetValue(int3_2 + new Int3(_x, _y, 0), out pointNode))
                {
                  for (; pointNode != null; pointNode = pointNode.next)
                  {
                    float sqrMagnitude = (position - (Vector3) pointNode.position).sqrMagnitude;
                    if ((double) sqrMagnitude < (double) num2)
                    {
                      num2 = sqrMagnitude;
                      node = (GraphNode) pointNode;
                    }
                    if (constraint == null || (double) sqrMagnitude < (double) num3 && (double) sqrMagnitude < (double) num1 && constraint.Suitable((GraphNode) pointNode))
                    {
                      num3 = sqrMagnitude;
                      graphNode = (GraphNode) pointNode;
                    }
                  }
                }
                if (this.nodeLookup.TryGetValue(int3_2 + new Int3(_x, _y, 2 * index), out pointNode))
                {
                  for (; pointNode != null; pointNode = pointNode.next)
                  {
                    float sqrMagnitude = (position - (Vector3) pointNode.position).sqrMagnitude;
                    if ((double) sqrMagnitude < (double) num2)
                    {
                      num2 = sqrMagnitude;
                      node = (GraphNode) pointNode;
                    }
                    if (constraint == null || (double) sqrMagnitude < (double) num3 && (double) sqrMagnitude < (double) num1 && constraint.Suitable((GraphNode) pointNode))
                    {
                      num3 = sqrMagnitude;
                      graphNode = (GraphNode) pointNode;
                    }
                  }
                }
              }
            }
            for (int _z = 1; _z < 2 * index; ++_z)
            {
              for (int _y = 0; _y <= 2 * index; ++_y)
              {
                if (this.nodeLookup.TryGetValue(int3_2 + new Int3(0, _y, _z), out pointNode))
                {
                  for (; pointNode != null; pointNode = pointNode.next)
                  {
                    float sqrMagnitude = (position - (Vector3) pointNode.position).sqrMagnitude;
                    if ((double) sqrMagnitude < (double) num2)
                    {
                      num2 = sqrMagnitude;
                      node = (GraphNode) pointNode;
                    }
                    if (constraint == null || (double) sqrMagnitude < (double) num3 && (double) sqrMagnitude < (double) num1 && constraint.Suitable((GraphNode) pointNode))
                    {
                      num3 = sqrMagnitude;
                      graphNode = (GraphNode) pointNode;
                    }
                  }
                }
                if (this.nodeLookup.TryGetValue(int3_2 + new Int3(2 * index, _y, _z), out pointNode))
                {
                  for (; pointNode != null; pointNode = pointNode.next)
                  {
                    float sqrMagnitude = (position - (Vector3) pointNode.position).sqrMagnitude;
                    if ((double) sqrMagnitude < (double) num2)
                    {
                      num2 = sqrMagnitude;
                      node = (GraphNode) pointNode;
                    }
                    if (constraint == null || (double) sqrMagnitude < (double) num3 && (double) sqrMagnitude < (double) num1 && constraint.Suitable((GraphNode) pointNode))
                    {
                      num3 = sqrMagnitude;
                      graphNode = (GraphNode) pointNode;
                    }
                  }
                }
              }
            }
            for (int _x = 1; _x < 2 * index; ++_x)
            {
              for (int _z = 1; _z < 2 * index; ++_z)
              {
                if (this.nodeLookup.TryGetValue(int3_2 + new Int3(_x, 0, _z), out pointNode))
                {
                  for (; pointNode != null; pointNode = pointNode.next)
                  {
                    float sqrMagnitude = (position - (Vector3) pointNode.position).sqrMagnitude;
                    if ((double) sqrMagnitude < (double) num2)
                    {
                      num2 = sqrMagnitude;
                      node = (GraphNode) pointNode;
                    }
                    if (constraint == null || (double) sqrMagnitude < (double) num3 && (double) sqrMagnitude < (double) num1 && constraint.Suitable((GraphNode) pointNode))
                    {
                      num3 = sqrMagnitude;
                      graphNode = (GraphNode) pointNode;
                    }
                  }
                }
                if (this.nodeLookup.TryGetValue(int3_2 + new Int3(_x, 2 * index, _z), out pointNode))
                {
                  for (; pointNode != null; pointNode = pointNode.next)
                  {
                    float sqrMagnitude = (position - (Vector3) pointNode.position).sqrMagnitude;
                    if ((double) sqrMagnitude < (double) num2)
                    {
                      num2 = sqrMagnitude;
                      node = (GraphNode) pointNode;
                    }
                    if (constraint == null || (double) sqrMagnitude < (double) num3 && (double) sqrMagnitude < (double) num1 && constraint.Suitable((GraphNode) pointNode))
                    {
                      num3 = sqrMagnitude;
                      graphNode = (GraphNode) pointNode;
                    }
                  }
                }
              }
            }
          }
          if (graphNode != null)
            val1_2 = Math.Min(val1_2, index + 1);
        }
      }
      else
      {
        for (int index = 0; index < this.nodeCount; ++index)
        {
          PointNode pointNode = this.nodes[index];
          float sqrMagnitude = (position - (Vector3) pointNode.position).sqrMagnitude;
          if ((double) sqrMagnitude < (double) num2)
          {
            num2 = sqrMagnitude;
            node = (GraphNode) pointNode;
          }
          if (constraint == null || (double) sqrMagnitude < (double) num3 && (double) sqrMagnitude < (double) num1 && constraint.Suitable((GraphNode) pointNode))
          {
            num3 = sqrMagnitude;
            graphNode = (GraphNode) pointNode;
          }
        }
      }
      NNInfo nnInfo = new NNInfo(node);
      nnInfo.constrainedNode = graphNode;
      if (graphNode != null)
        nnInfo.constClampedPosition = (Vector3) graphNode.position;
      else if (node != null)
      {
        nnInfo.constrainedNode = node;
        nnInfo.constClampedPosition = (Vector3) node.position;
      }
      return nnInfo;
    }

    public PointNode AddNode(Int3 position)
    {
      return this.AddNode<PointNode>(new PointNode(this.active), position);
    }

    public T AddNode<T>(T nd, Int3 position) where T : PointNode
    {
      if (this.nodes == null || this.nodeCount == this.nodes.Length)
      {
        PointNode[] pointNodeArray = new PointNode[this.nodes == null ? 4 : Math.Max(this.nodes.Length + 4, this.nodes.Length * 2)];
        for (int index = 0; index < this.nodeCount; ++index)
          pointNodeArray[index] = this.nodes[index];
        this.nodes = pointNodeArray;
      }
      nd.SetPosition(position);
      nd.GraphIndex = this.graphIndex;
      nd.Walkable = true;
      this.nodes[this.nodeCount] = (PointNode) nd;
      ++this.nodeCount;
      this.AddToLookup((PointNode) nd);
      return nd;
    }

    public static int CountChildren(Transform tr)
    {
      int num = 0;
      foreach (Transform tr1 in tr)
      {
        ++num;
        num += PointGraph.CountChildren(tr1);
      }
      return num;
    }

    public void AddChildren(ref int c, Transform tr)
    {
      foreach (Transform tr1 in tr)
      {
        this.nodes[c].SetPosition((Int3) tr1.position);
        this.nodes[c].Walkable = true;
        this.nodes[c].gameObject = tr1.gameObject;
        c = c + 1;
        this.AddChildren(ref c, tr1);
      }
    }

    public void RebuildNodeLookup()
    {
      if (!this.optimizeForSparseGraph)
        return;
      if ((double) this.maxDistance == 0.0)
      {
        this.lookupCellSize = (Int3) this.limits;
      }
      else
      {
        this.lookupCellSize.x = Mathf.CeilToInt((float) (1000.0 * ((double) this.limits.x == 0.0 ? (double) this.maxDistance : (double) Mathf.Min(this.maxDistance, this.limits.x))));
        this.lookupCellSize.y = Mathf.CeilToInt((float) (1000.0 * ((double) this.limits.y == 0.0 ? (double) this.maxDistance : (double) Mathf.Min(this.maxDistance, this.limits.y))));
        this.lookupCellSize.z = Mathf.CeilToInt((float) (1000.0 * ((double) this.limits.z == 0.0 ? (double) this.maxDistance : (double) Mathf.Min(this.maxDistance, this.limits.z))));
      }
      if (this.optimizeFor2D)
        this.lookupCellSize.y = 0;
      if (this.nodeLookup == null)
        this.nodeLookup = new Dictionary<Int3, PointNode>();
      this.nodeLookup.Clear();
      for (int index = 0; index < this.nodeCount; ++index)
        this.AddToLookup(this.nodes[index]);
    }

    public void AddToLookup(PointNode node)
    {
      if (this.nodeLookup == null)
        return;
      Int3 key = this.WorldToLookupSpace(node.position);
      if (this.nodeLookup.Count == 0)
      {
        this.minLookup = key;
        this.maxLookup = key;
      }
      else
      {
        this.minLookup = new Int3(Math.Min(this.minLookup.x, key.x), Math.Min(this.minLookup.y, key.y), Math.Min(this.minLookup.z, key.z));
        this.maxLookup = new Int3(Math.Max(this.minLookup.x, key.x), Math.Max(this.minLookup.y, key.y), Math.Max(this.minLookup.z, key.z));
      }
      if (node.next != null)
        throw new Exception("This node has already been added to the lookup structure");
      PointNode pointNode;
      if (this.nodeLookup.TryGetValue(key, out pointNode))
      {
        node.next = pointNode.next;
        pointNode.next = node;
      }
      else
        this.nodeLookup[key] = node;
    }

    public override void ScanInternal(OnScanStatus statusCallback)
    {
      if ((UnityEngine.Object) this.root == (UnityEngine.Object) null)
      {
        GameObject[] gameObjectsWithTag = GameObject.FindGameObjectsWithTag(this.searchTag);
        if (gameObjectsWithTag == null)
        {
          this.nodes = new PointNode[0];
          this.nodeCount = 0;
          return;
        }
        this.nodes = new PointNode[gameObjectsWithTag.Length];
        this.nodeCount = this.nodes.Length;
        for (int index = 0; index < this.nodes.Length; ++index)
          this.nodes[index] = new PointNode(this.active);
        for (int index = 0; index < gameObjectsWithTag.Length; ++index)
        {
          this.nodes[index].SetPosition((Int3) gameObjectsWithTag[index].transform.position);
          this.nodes[index].Walkable = true;
          this.nodes[index].gameObject = gameObjectsWithTag[index].gameObject;
        }
      }
      else if (!this.recursive)
      {
        this.nodes = new PointNode[this.root.childCount];
        this.nodeCount = this.nodes.Length;
        for (int index = 0; index < this.nodes.Length; ++index)
          this.nodes[index] = new PointNode(this.active);
        int index1 = 0;
        foreach (Transform transform in this.root)
        {
          this.nodes[index1].SetPosition((Int3) transform.position);
          this.nodes[index1].Walkable = true;
          this.nodes[index1].gameObject = transform.gameObject;
          ++index1;
        }
      }
      else
      {
        this.nodes = new PointNode[PointGraph.CountChildren(this.root)];
        this.nodeCount = this.nodes.Length;
        for (int index = 0; index < this.nodes.Length; ++index)
          this.nodes[index] = new PointNode(this.active);
        int c = 0;
        this.AddChildren(ref c, this.root);
      }
      if (this.optimizeForSparseGraph)
        this.RebuildNodeLookup();
      if ((double) this.maxDistance < 0.0)
        return;
      List<PointNode> list1 = new List<PointNode>(3);
      List<uint> list2 = new List<uint>(3);
      for (int index1 = 0; index1 < this.nodes.Length; ++index1)
      {
        list1.Clear();
        list2.Clear();
        PointNode pointNode1 = this.nodes[index1];
        if (this.optimizeForSparseGraph)
        {
          Int3 int3 = this.WorldToLookupSpace(pointNode1.position);
          int num = this.lookupCellSize.y != 0 ? PointGraph.ThreeDNeighbours.Length : 9;
          for (int index2 = 0; index2 < num; ++index2)
          {
            PointNode pointNode2;
            if (this.nodeLookup.TryGetValue(int3 + PointGraph.ThreeDNeighbours[index2], out pointNode2))
            {
              for (; pointNode2 != null; pointNode2 = pointNode2.next)
              {
                float dist = 0.0f;
                if (this.IsValidConnection((GraphNode) pointNode1, (GraphNode) pointNode2, out dist))
                {
                  list1.Add(pointNode2);
                  list2.Add((uint) Mathf.RoundToInt(dist * 1000f));
                }
              }
            }
          }
        }
        else
        {
          for (int index2 = 0; index2 < this.nodes.Length; ++index2)
          {
            if (index1 != index2)
            {
              PointNode pointNode2 = this.nodes[index2];
              float dist = 0.0f;
              if (this.IsValidConnection((GraphNode) pointNode1, (GraphNode) pointNode2, out dist))
              {
                list1.Add(pointNode2);
                list2.Add((uint) Mathf.RoundToInt(dist * 1000f));
              }
            }
          }
        }
        pointNode1.connections = (GraphNode[]) list1.ToArray();
        pointNode1.connectionCosts = list2.ToArray();
      }
    }

    public virtual bool IsValidConnection(GraphNode a, GraphNode b, out float dist)
    {
      dist = 0.0f;
      if (!a.Walkable || !b.Walkable)
        return false;
      Vector3 vector3 = (Vector3) (a.position - b.position);
      if (!Mathf.Approximately(this.limits.x, 0.0f) && (double) Mathf.Abs(vector3.x) > (double) this.limits.x || !Mathf.Approximately(this.limits.y, 0.0f) && (double) Mathf.Abs(vector3.y) > (double) this.limits.y || !Mathf.Approximately(this.limits.z, 0.0f) && (double) Mathf.Abs(vector3.z) > (double) this.limits.z)
        return false;
      dist = vector3.magnitude;
      if ((double) this.maxDistance == 0.0 || (double) dist < (double) this.maxDistance)
      {
        if (!this.raycast)
          return true;
        Ray ray1 = new Ray((Vector3) a.position, (Vector3) (b.position - a.position));
        Ray ray2 = new Ray((Vector3) b.position, (Vector3) (a.position - b.position));
        if (this.use2DPhysics)
        {
          if (this.thickRaycast)
          {
            if (!(bool) Physics2D.CircleCast((Vector2) ray1.origin, this.thickRaycastRadius, (Vector2) ray1.direction, dist, (int) this.mask) && !(bool) Physics2D.CircleCast((Vector2) ray2.origin, this.thickRaycastRadius, (Vector2) ray2.direction, dist, (int) this.mask))
              return true;
          }
          else if (!(bool) Physics2D.Linecast((Vector2) (Vector3) a.position, (Vector2) (Vector3) b.position, (int) this.mask) && !(bool) Physics2D.Linecast((Vector2) (Vector3) b.position, (Vector2) (Vector3) a.position, (int) this.mask))
            return true;
        }
        else if (this.thickRaycast)
        {
          if (!Physics.SphereCast(ray1, this.thickRaycastRadius, dist, (int) this.mask) && !Physics.SphereCast(ray2, this.thickRaycastRadius, dist, (int) this.mask))
            return true;
        }
        else if (!Physics.Linecast((Vector3) a.position, (Vector3) b.position, (int) this.mask) && !Physics.Linecast((Vector3) b.position, (Vector3) a.position, (int) this.mask))
          return true;
      }
      return false;
    }

    public GraphUpdateThreading CanUpdateAsync(GraphUpdateObject o)
    {
      return GraphUpdateThreading.UnityThread;
    }

    public void UpdateAreaInit(GraphUpdateObject o)
    {
    }

    public void UpdateArea(GraphUpdateObject guo)
    {
      if (this.nodes == null)
        return;
      for (int index = 0; index < this.nodeCount; ++index)
      {
        if (guo.bounds.Contains((Vector3) this.nodes[index].position))
        {
          guo.WillUpdateNode((GraphNode) this.nodes[index]);
          guo.Apply((GraphNode) this.nodes[index]);
        }
      }
      if (!guo.updatePhysics)
        return;
      Bounds bounds = guo.bounds;
      if (this.thickRaycast)
        bounds.Expand(this.thickRaycastRadius * 2f);
      List<GraphNode> list1 = ListPool<GraphNode>.Claim();
      List<uint> list2 = ListPool<uint>.Claim();
      for (int index1 = 0; index1 < this.nodeCount; ++index1)
      {
        PointNode pointNode1 = this.nodes[index1];
        Vector3 a = (Vector3) pointNode1.position;
        List<GraphNode> list3 = (List<GraphNode>) null;
        List<uint> list4 = (List<uint>) null;
        for (int index2 = 0; index2 < this.nodeCount; ++index2)
        {
          if (index2 != index1)
          {
            Vector3 b = (Vector3) this.nodes[index2].position;
            if (Polygon.LineIntersectsBounds(bounds, a, b))
            {
              PointNode pointNode2 = this.nodes[index2];
              bool flag = pointNode1.ContainsConnection((GraphNode) pointNode2);
              float dist;
              if (!flag && this.IsValidConnection((GraphNode) pointNode1, (GraphNode) pointNode2, out dist))
              {
                if (list3 == null)
                {
                  list1.Clear();
                  list2.Clear();
                  list3 = list1;
                  list4 = list2;
                  list3.AddRange((IEnumerable<GraphNode>) pointNode1.connections);
                  list4.AddRange((IEnumerable<uint>) pointNode1.connectionCosts);
                }
                uint num = (uint) Mathf.RoundToInt(dist * 1000f);
                list3.Add((GraphNode) pointNode2);
                list4.Add(num);
              }
              else if (flag && !this.IsValidConnection((GraphNode) pointNode1, (GraphNode) pointNode2, out dist))
              {
                if (list3 == null)
                {
                  list1.Clear();
                  list2.Clear();
                  list3 = list1;
                  list4 = list2;
                  list3.AddRange((IEnumerable<GraphNode>) pointNode1.connections);
                  list4.AddRange((IEnumerable<uint>) pointNode1.connectionCosts);
                }
                int index3 = list3.IndexOf((GraphNode) pointNode2);
                if (index3 != -1)
                {
                  list3.RemoveAt(index3);
                  list4.RemoveAt(index3);
                }
              }
            }
          }
        }
        if (list3 != null)
        {
          pointNode1.connections = list3.ToArray();
          pointNode1.connectionCosts = list4.ToArray();
        }
      }
      ListPool<GraphNode>.Release(list1);
      ListPool<uint>.Release(list2);
    }

    public override void PostDeserialization()
    {
      this.RebuildNodeLookup();
    }

    public override void RelocateNodes(Matrix4x4 oldMatrix, Matrix4x4 newMatrix)
    {
      base.RelocateNodes(oldMatrix, newMatrix);
      this.RebuildNodeLookup();
    }

    public override void SerializeExtraInfo(GraphSerializationContext ctx)
    {
      if (this.nodes == null)
        ctx.writer.Write(-1);
      ctx.writer.Write(this.nodeCount);
      for (int index = 0; index < this.nodeCount; ++index)
      {
        if (this.nodes[index] == null)
        {
          ctx.writer.Write(-1);
        }
        else
        {
          ctx.writer.Write(0);
          this.nodes[index].SerializeNode(ctx);
        }
      }
    }

    public override void DeserializeExtraInfo(GraphSerializationContext ctx)
    {
      int length = ctx.reader.ReadInt32();
      if (length == -1)
      {
        this.nodes = (PointNode[]) null;
      }
      else
      {
        this.nodes = new PointNode[length];
        this.nodeCount = length;
        for (int index = 0; index < this.nodes.Length; ++index)
        {
          if (ctx.reader.ReadInt32() != -1)
          {
            this.nodes[index] = new PointNode(this.active);
            this.nodes[index].DeserializeNode(ctx);
          }
        }
      }
    }
  }
}
