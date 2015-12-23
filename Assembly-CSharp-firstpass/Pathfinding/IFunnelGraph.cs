// Decompiled with JetBrains decompiler
// Type: Pathfinding.IFunnelGraph
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
  public interface IFunnelGraph
  {
    void BuildFunnelCorridor(List<GraphNode> path, int sIndex, int eIndex, List<Vector3> left, List<Vector3> right);

    void AddPortal(GraphNode n1, GraphNode n2, List<Vector3> left, List<Vector3> right);
  }
}
