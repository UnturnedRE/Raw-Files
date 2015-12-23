// Decompiled with JetBrains decompiler
// Type: Pathfinding.Voxels.CompactVoxelSpan
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

namespace Pathfinding.Voxels
{
  public struct CompactVoxelSpan
  {
    public ushort y;
    public uint con;
    public uint h;
    public int reg;

    public CompactVoxelSpan(ushort bottom, uint height)
    {
      this.con = 24U;
      this.y = bottom;
      this.h = height;
      this.reg = 0;
    }

    public void SetConnection(int dir, uint value)
    {
      int num = dir * 6;
      this.con = (uint) ((ulong) this.con & (ulong) ~(63 << num) | (ulong) (uint) (((int) value & 63) << num));
    }

    public int GetConnection(int dir)
    {
      return (int) this.con >> dir * 6 & 63;
    }
  }
}
