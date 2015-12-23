// Decompiled with JetBrains decompiler
// Type: Pathfinding.PathEndingCondition
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Pathfinding
{
  public class PathEndingCondition
  {
    protected Path p;

    protected PathEndingCondition()
    {
    }

    public PathEndingCondition(Path p)
    {
      if (p == null)
        throw new ArgumentNullException("Please supply a non-null path");
      this.p = p;
    }

    public virtual bool TargetFound(PathNode node)
    {
      return true;
    }
  }
}
