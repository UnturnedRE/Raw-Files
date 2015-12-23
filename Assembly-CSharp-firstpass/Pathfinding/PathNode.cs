// Decompiled with JetBrains decompiler
// Type: Pathfinding.PathNode
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

namespace Pathfinding
{
  public class PathNode
  {
    private const uint CostMask = 268435455U;
    private const int Flag1Offset = 28;
    private const uint Flag1Mask = 268435456U;
    private const int Flag2Offset = 29;
    private const uint Flag2Mask = 536870912U;
    public GraphNode node;
    public PathNode parent;
    public ushort pathID;
    private uint flags;
    private uint g;
    private uint h;

    public uint cost
    {
      get
      {
        return this.flags & 268435455U;
      }
      set
      {
        this.flags = this.flags & 4026531840U | value;
      }
    }

    public bool flag1
    {
      get
      {
        return ((int) this.flags & 268435456) != 0;
      }
      set
      {
        this.flags = (uint) ((int) this.flags & -268435457 | (!value ? 0 : 268435456));
      }
    }

    public bool flag2
    {
      get
      {
        return ((int) this.flags & 536870912) != 0;
      }
      set
      {
        this.flags = (uint) ((int) this.flags & -536870913 | (!value ? 0 : 536870912));
      }
    }

    public uint G
    {
      get
      {
        return this.g;
      }
      set
      {
        this.g = value;
      }
    }

    public uint H
    {
      get
      {
        return this.h;
      }
      set
      {
        this.h = value;
      }
    }

    public uint F
    {
      get
      {
        return this.g + this.h;
      }
    }
  }
}
