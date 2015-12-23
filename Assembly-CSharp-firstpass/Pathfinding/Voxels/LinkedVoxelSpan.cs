// Decompiled with JetBrains decompiler
// Type: Pathfinding.Voxels.LinkedVoxelSpan
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

namespace Pathfinding.Voxels
{
  public struct LinkedVoxelSpan
  {
    public uint bottom;
    public uint top;
    public int next;
    public int area;

    public LinkedVoxelSpan(uint bottom, uint top, int area)
    {
      this.bottom = bottom;
      this.top = top;
      this.area = area;
      this.next = -1;
    }

    public LinkedVoxelSpan(uint bottom, uint top, int area, int next)
    {
      this.bottom = bottom;
      this.top = top;
      this.area = area;
      this.next = next;
    }
  }
}
