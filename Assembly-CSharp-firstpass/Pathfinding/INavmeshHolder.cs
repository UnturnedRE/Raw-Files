// Decompiled with JetBrains decompiler
// Type: Pathfinding.INavmeshHolder
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

namespace Pathfinding
{
  public interface INavmeshHolder
  {
    Int3 GetVertex(int i);

    int GetVertexArrayIndex(int index);

    void GetTileCoordinates(int tileIndex, out int x, out int z);
  }
}
