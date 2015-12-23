// Decompiled with JetBrains decompiler
// Type: Pathfinding.EndingConditionDistance
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

namespace Pathfinding
{
  public class EndingConditionDistance : PathEndingCondition
  {
    public int maxGScore = 100;

    public EndingConditionDistance(Path p, int maxGScore)
      : base(p)
    {
      this.maxGScore = maxGScore;
    }

    public override bool TargetFound(PathNode node)
    {
      return (long) node.G >= (long) this.maxGScore;
    }
  }
}
