// Decompiled with JetBrains decompiler
// Type: Pathfinding.GraphNode
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding.Serialization;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
  public abstract class GraphNode
  {
    private const int FlagsWalkableOffset = 0;
    private const uint FlagsWalkableMask = 1U;
    private const int FlagsAreaOffset = 1;
    private const uint FlagsAreaMask = 262142U;
    private const int FlagsGraphOffset = 24;
    private const uint FlagsGraphMask = 4278190080U;
    public const uint MaxAreaIndex = 131071U;
    public const uint MaxGraphIndex = 255U;
    private const int FlagsTagOffset = 19;
    private const uint FlagsTagMask = 16252928U;
    private int nodeIndex;
    protected uint flags;
    private uint penalty;
    public Int3 position;

    [Obsolete("This attribute is deprecated. Please use .position (not a capital P)")]
    public Int3 Position
    {
      get
      {
        return this.position;
      }
    }

    [Obsolete("This attribute is deprecated. Please use .Walkable (with a capital W)")]
    public bool walkable
    {
      get
      {
        return this.Walkable;
      }
      set
      {
        this.Walkable = value;
      }
    }

    [Obsolete("This attribute is deprecated. Please use .Tag (with a capital T)")]
    public uint tags
    {
      get
      {
        return this.Tag;
      }
      set
      {
        this.Tag = value;
      }
    }

    [Obsolete("This attribute is deprecated. Please use .GraphIndex (with a capital G)")]
    public uint graphIndex
    {
      get
      {
        return this.GraphIndex;
      }
      set
      {
        this.GraphIndex = value;
      }
    }

    public bool Destroyed
    {
      get
      {
        return this.nodeIndex == -1;
      }
    }

    public int NodeIndex
    {
      get
      {
        return this.nodeIndex;
      }
    }

    public uint Flags
    {
      get
      {
        return this.flags;
      }
      set
      {
        this.flags = value;
      }
    }

    public uint Penalty
    {
      get
      {
        return this.penalty;
      }
      set
      {
        if (value > 16777215U)
          Debug.LogWarning((object) ("Very high penalty applied. Are you sure negative values haven't underflowed?\nPenalty values this high could with long paths cause overflows and in some cases infinity loops because of that.\nPenalty value applied: " + (object) value));
        this.penalty = value;
      }
    }

    public bool Walkable
    {
      get
      {
        return ((int) this.flags & 1) != 0;
      }
      set
      {
        this.flags = (uint) ((int) this.flags & -2 | (!value ? 0 : 1));
      }
    }

    public uint Area
    {
      get
      {
        return (this.flags & 262142U) >> 1;
      }
      set
      {
        this.flags = (uint) ((int) this.flags & -262143 | (int) value << 1);
      }
    }

    public uint GraphIndex
    {
      get
      {
        return (this.flags & 4278190080U) >> 24;
      }
      set
      {
        this.flags = (uint) ((int) this.flags & 16777215 | (int) value << 24);
      }
    }

    public uint Tag
    {
      get
      {
        return (this.flags & 16252928U) >> 19;
      }
      set
      {
        this.flags = (uint) ((int) this.flags & -16252929 | (int) value << 19);
      }
    }

    public GraphNode(AstarPath astar)
    {
      if (!((UnityEngine.Object) astar != (UnityEngine.Object) null))
        throw new Exception("No active AstarPath object to bind to");
      this.nodeIndex = astar.GetNewNodeIndex();
      astar.InitializeNode(this);
    }

    public void Destroy()
    {
      if (this.nodeIndex == -1)
        return;
      this.ClearConnections(true);
      if ((UnityEngine.Object) AstarPath.active != (UnityEngine.Object) null)
        AstarPath.active.DestroyNode(this);
      this.nodeIndex = -1;
    }

    public void UpdateG(Path path, PathNode pathNode)
    {
      pathNode.G = pathNode.parent.G + pathNode.cost + path.GetTraversalCost(this);
    }

    public virtual void UpdateRecursiveG(Path path, PathNode pathNode, PathHandler handler)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      GraphNode.\u003CUpdateRecursiveG\u003Ec__AnonStorey1C recursiveGCAnonStorey1C = new GraphNode.\u003CUpdateRecursiveG\u003Ec__AnonStorey1C();
      // ISSUE: reference to a compiler-generated field
      recursiveGCAnonStorey1C.handler = handler;
      // ISSUE: reference to a compiler-generated field
      recursiveGCAnonStorey1C.pathNode = pathNode;
      // ISSUE: reference to a compiler-generated field
      recursiveGCAnonStorey1C.path = path;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.UpdateG(recursiveGCAnonStorey1C.path, recursiveGCAnonStorey1C.pathNode);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      recursiveGCAnonStorey1C.handler.PushNode(recursiveGCAnonStorey1C.pathNode);
      // ISSUE: reference to a compiler-generated method
      this.GetConnections(new GraphNodeDelegate(recursiveGCAnonStorey1C.\u003C\u003Em__F));
    }

    public virtual void FloodFill(Stack<GraphNode> stack, uint region)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated method
      this.GetConnections(new GraphNodeDelegate(new GraphNode.\u003CFloodFill\u003Ec__AnonStorey1D()
      {
        region = region,
        stack = stack
      }.\u003C\u003Em__10));
    }

    public abstract void GetConnections(GraphNodeDelegate del);

    public abstract void AddConnection(GraphNode node, uint cost);

    public abstract void RemoveConnection(GraphNode node);

    public abstract void ClearConnections(bool alsoReverse);

    public virtual bool ContainsConnection(GraphNode node)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      GraphNode.\u003CContainsConnection\u003Ec__AnonStorey1E connectionCAnonStorey1E = new GraphNode.\u003CContainsConnection\u003Ec__AnonStorey1E();
      // ISSUE: reference to a compiler-generated field
      connectionCAnonStorey1E.node = node;
      // ISSUE: reference to a compiler-generated field
      connectionCAnonStorey1E.contains = false;
      // ISSUE: reference to a compiler-generated method
      this.GetConnections(new GraphNodeDelegate(connectionCAnonStorey1E.\u003C\u003Em__11));
      // ISSUE: reference to a compiler-generated field
      return connectionCAnonStorey1E.contains;
    }

    public virtual void RecalculateConnectionCosts()
    {
    }

    public virtual bool GetPortal(GraphNode other, List<Vector3> left, List<Vector3> right, bool backwards)
    {
      return false;
    }

    public abstract void Open(Path path, PathNode pathNode, PathHandler handler);

    public virtual void SerializeNode(GraphSerializationContext ctx)
    {
      ctx.writer.Write(this.Penalty);
      ctx.writer.Write(this.Flags);
    }

    public virtual void DeserializeNode(GraphSerializationContext ctx)
    {
      this.Penalty = ctx.reader.ReadUInt32();
      this.Flags = ctx.reader.ReadUInt32();
    }

    public virtual void SerializeReferences(GraphSerializationContext ctx)
    {
    }

    public virtual void DeserializeReferences(GraphSerializationContext ctx)
    {
    }
  }
}
