// Decompiled with JetBrains decompiler
// Type: Pathfinding.GraphUpdateObject
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
  public class GraphUpdateObject
  {
    public bool requiresFloodFill = true;
    public bool updatePhysics = true;
    public bool resetPenaltyOnPhysics = true;
    public bool updateErosion = true;
    public NNConstraint nnConstraint = NNConstraint.None;
    public Bounds bounds;
    public int addPenalty;
    public bool modifyWalkability;
    public bool setWalkability;
    public bool modifyTag;
    public int setTag;
    public bool trackChangedNodes;
    public List<GraphNode> changedNodes;
    private List<uint> backupData;
    private List<Int3> backupPositionData;
    public GraphUpdateShape shape;

    public GraphUpdateObject()
    {
    }

    public GraphUpdateObject(Bounds b)
    {
      this.bounds = b;
    }

    public virtual void WillUpdateNode(GraphNode node)
    {
      if (!this.trackChangedNodes || node == null)
        return;
      if (this.changedNodes == null)
      {
        this.changedNodes = ListPool<GraphNode>.Claim();
        this.backupData = ListPool<uint>.Claim();
        this.backupPositionData = ListPool<Int3>.Claim();
      }
      this.changedNodes.Add(node);
      this.backupPositionData.Add(node.position);
      this.backupData.Add(node.Penalty);
      this.backupData.Add(node.Flags);
      GridNode gridNode = node as GridNode;
      if (gridNode == null)
        return;
      this.backupData.Add((uint) gridNode.InternalGridFlags);
    }

    public virtual void RevertFromBackup()
    {
      if (!this.trackChangedNodes)
        throw new InvalidOperationException("Changed nodes have not been tracked, cannot revert from backup");
      if (this.changedNodes == null)
        return;
      int index1 = 0;
      for (int index2 = 0; index2 < this.changedNodes.Count; ++index2)
      {
        this.changedNodes[index2].Penalty = this.backupData[index1];
        int index3 = index1 + 1;
        this.changedNodes[index2].Flags = this.backupData[index3];
        index1 = index3 + 1;
        GridNode gridNode = this.changedNodes[index2] as GridNode;
        if (gridNode != null)
        {
          gridNode.InternalGridFlags = (ushort) this.backupData[index1];
          ++index1;
        }
        this.changedNodes[index2].position = this.backupPositionData[index2];
      }
      ListPool<GraphNode>.Release(this.changedNodes);
      ListPool<uint>.Release(this.backupData);
      ListPool<Int3>.Release(this.backupPositionData);
    }

    public virtual void Apply(GraphNode node)
    {
      if (this.shape != null && !this.shape.Contains(node))
        return;
      node.Penalty = (uint) ((ulong) node.Penalty + (ulong) this.addPenalty);
      if (this.modifyWalkability)
        node.Walkable = this.setWalkability;
      if (!this.modifyTag)
        return;
      node.Tag = (uint) this.setTag;
    }
  }
}
