// Decompiled with JetBrains decompiler
// Type: Pathfinding.GraphUpdateUtilities
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding.Util;
using System;
using System.Collections.Generic;

namespace Pathfinding
{
  public static class GraphUpdateUtilities
  {
    [Obsolete("This function has been moved to Pathfinding.Util.PathUtilities. Please use the version in that class")]
    public static bool IsPathPossible(GraphNode n1, GraphNode n2)
    {
      if (n1.Walkable && n2.Walkable)
        return (int) n1.Area == (int) n2.Area;
      return false;
    }

    [Obsolete("This function has been moved to Pathfinding.Util.PathUtilities. Please use the version in that class")]
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

    public static bool UpdateGraphsNoBlock(GraphUpdateObject guo, GraphNode node1, GraphNode node2, bool alwaysRevert = false)
    {
      List<GraphNode> list = ListPool<GraphNode>.Claim();
      list.Add(node1);
      list.Add(node2);
      bool flag = GraphUpdateUtilities.UpdateGraphsNoBlock(guo, list, alwaysRevert);
      ListPool<GraphNode>.Release(list);
      return flag;
    }

    public static bool UpdateGraphsNoBlock(GraphUpdateObject guo, List<GraphNode> nodes, bool alwaysRevert = false)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      GraphUpdateUtilities.\u003CUpdateGraphsNoBlock\u003Ec__AnonStorey37 blockCAnonStorey37 = new GraphUpdateUtilities.\u003CUpdateGraphsNoBlock\u003Ec__AnonStorey37();
      // ISSUE: reference to a compiler-generated field
      blockCAnonStorey37.guo = guo;
      // ISSUE: reference to a compiler-generated field
      blockCAnonStorey37.nodes = nodes;
      // ISSUE: reference to a compiler-generated field
      blockCAnonStorey37.alwaysRevert = alwaysRevert;
      // ISSUE: reference to a compiler-generated field
      for (int index = 0; index < blockCAnonStorey37.nodes.Count; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        if (!blockCAnonStorey37.nodes[index].Walkable)
          return false;
      }
      // ISSUE: reference to a compiler-generated field
      blockCAnonStorey37.guo.trackChangedNodes = true;
      // ISSUE: reference to a compiler-generated field
      blockCAnonStorey37.worked = true;
      // ISSUE: reference to a compiler-generated method
      AstarPath.RegisterSafeUpdate(new OnVoidDelegate(blockCAnonStorey37.\u003C\u003Em__2E));
      AstarPath.active.FlushThreadSafeCallbacks();
      // ISSUE: reference to a compiler-generated field
      blockCAnonStorey37.guo.trackChangedNodes = false;
      // ISSUE: reference to a compiler-generated field
      return blockCAnonStorey37.worked;
    }
  }
}
