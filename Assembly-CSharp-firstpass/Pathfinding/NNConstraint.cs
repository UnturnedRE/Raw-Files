// Decompiled with JetBrains decompiler
// Type: Pathfinding.NNConstraint
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

namespace Pathfinding
{
  public class NNConstraint
  {
    public int graphMask = -1;
    public int area = -1;
    public bool constrainWalkability = true;
    public bool walkable = true;
    public bool constrainTags = true;
    public int tags = -1;
    public bool constrainDistance = true;
    public bool constrainArea;
    public bool distanceXZ;

    public static NNConstraint Default
    {
      get
      {
        return new NNConstraint();
      }
    }

    public static NNConstraint None
    {
      get
      {
        return new NNConstraint()
        {
          constrainWalkability = false,
          constrainArea = false,
          constrainTags = false,
          constrainDistance = false,
          graphMask = -1
        };
      }
    }

    public virtual bool SuitableGraph(int graphIndex, NavGraph graph)
    {
      return (this.graphMask >> graphIndex & 1) != 0;
    }

    public virtual bool Suitable(GraphNode node)
    {
      return (!this.constrainWalkability || node.Walkable == this.walkable) && (!this.constrainArea || this.area < 0 || (long) node.Area == (long) this.area) && (!this.constrainTags || (this.tags >> (int) node.Tag & 1) != 0);
    }
  }
}
