// Decompiled with JetBrains decompiler
// Type: Pathfinding.IRaycastableGraph
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
  public interface IRaycastableGraph
  {
    bool Linecast(Vector3 start, Vector3 end);

    bool Linecast(Vector3 start, Vector3 end, GraphNode hint);

    bool Linecast(Vector3 start, Vector3 end, GraphNode hint, out GraphHitInfo hit);

    bool Linecast(Vector3 start, Vector3 end, GraphNode hint, out GraphHitInfo hit, List<GraphNode> trace);
  }
}
