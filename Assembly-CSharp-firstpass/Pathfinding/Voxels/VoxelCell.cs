// Decompiled with JetBrains decompiler
// Type: Pathfinding.Voxels.VoxelCell
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding;

namespace Pathfinding.Voxels
{
  public struct VoxelCell
  {
    public VoxelSpan firstSpan;

    public void AddSpan(uint bottom, uint top, int area, int voxelWalkableClimb)
    {
      VoxelSpan voxelSpan1 = new VoxelSpan(bottom, top, area);
      if (this.firstSpan == null)
      {
        this.firstSpan = voxelSpan1;
      }
      else
      {
        VoxelSpan voxelSpan2 = (VoxelSpan) null;
        VoxelSpan voxelSpan3 = this.firstSpan;
        while (voxelSpan3 != null && voxelSpan3.bottom <= voxelSpan1.top)
        {
          if (voxelSpan3.top < voxelSpan1.bottom)
          {
            voxelSpan2 = voxelSpan3;
            voxelSpan3 = voxelSpan3.next;
          }
          else
          {
            if (voxelSpan3.bottom < bottom)
              voxelSpan1.bottom = voxelSpan3.bottom;
            if (voxelSpan3.top > top)
              voxelSpan1.top = voxelSpan3.top;
            if (AstarMath.Abs((int) voxelSpan1.top - (int) voxelSpan3.top) <= voxelWalkableClimb)
              voxelSpan1.area = AstarMath.Max(voxelSpan1.area, voxelSpan3.area);
            VoxelSpan voxelSpan4 = voxelSpan3.next;
            if (voxelSpan2 != null)
              voxelSpan2.next = voxelSpan4;
            else
              this.firstSpan = voxelSpan4;
            voxelSpan3 = voxelSpan4;
          }
        }
        if (voxelSpan2 != null)
        {
          voxelSpan1.next = voxelSpan2.next;
          voxelSpan2.next = voxelSpan1;
        }
        else
        {
          voxelSpan1.next = this.firstSpan;
          this.firstSpan = voxelSpan1;
        }
      }
    }
  }
}
