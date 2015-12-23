// Decompiled with JetBrains decompiler
// Type: Pathfinding.ABPathEndingCondition
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Pathfinding
{
  public class ABPathEndingCondition : PathEndingCondition
  {
    protected ABPath abPath;

    public ABPathEndingCondition(ABPath p)
    {
      if (p == null)
        throw new ArgumentNullException("Please supply a non-null path");
      this.abPath = p;
    }

    public override bool TargetFound(PathNode node)
    {
      return node.node == this.abPath.endNode;
    }
  }
}
