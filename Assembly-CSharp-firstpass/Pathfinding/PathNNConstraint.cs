// Decompiled with JetBrains decompiler
// Type: Pathfinding.PathNNConstraint
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

namespace Pathfinding
{
  public class PathNNConstraint : NNConstraint
  {
    public static PathNNConstraint Default
    {
      get
      {
        PathNNConstraint pathNnConstraint = new PathNNConstraint();
        pathNnConstraint.constrainArea = true;
        return pathNnConstraint;
      }
    }

    public virtual void SetStart(GraphNode node)
    {
      if (node != null)
        this.area = (int) node.Area;
      else
        this.constrainArea = false;
    }
  }
}
