// Decompiled with JetBrains decompiler
// Type: Pathfinding.Voxels.VoxelSpan
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

namespace Pathfinding.Voxels
{
  public class VoxelSpan
  {
    public uint bottom;
    public uint top;
    public VoxelSpan next;
    public int area;

    public VoxelSpan(uint b, uint t, int area)
    {
      this.bottom = b;
      this.top = t;
      this.area = area;
    }
  }
}
