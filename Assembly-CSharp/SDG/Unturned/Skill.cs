// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.Skill
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

namespace SDG.Unturned
{
  public class Skill
  {
    public byte level;
    public byte max;
    private uint _cost;

    public float mastery
    {
      get
      {
        if ((int) this.level == 0)
          return 0.0f;
        if ((int) this.level >= (int) this.max)
          return 1f;
        return (float) this.level / (float) this.max;
      }
    }

    public uint cost
    {
      get
      {
        return (uint) ((ulong) this._cost * (ulong) ((int) this.level + 1));
      }
    }

    public Skill(byte newLevel, byte newMax, uint newCost)
    {
      this.level = newLevel;
      this.max = newMax;
      this._cost = newCost;
    }
  }
}
