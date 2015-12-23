// Decompiled with JetBrains decompiler
// Type: Pathfinding.FloodPathConstraint
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

namespace Pathfinding
{
  public class FloodPathConstraint : NNConstraint
  {
    private FloodPath path;

    public FloodPathConstraint(FloodPath path)
    {
      if (path == null)
        Debug.LogWarning((object) "FloodPathConstraint should not be used with a NULL path");
      this.path = path;
    }

    public override bool Suitable(GraphNode node)
    {
      if (base.Suitable(node))
        return this.path.HasPathTo(node);
      return false;
    }
  }
}
