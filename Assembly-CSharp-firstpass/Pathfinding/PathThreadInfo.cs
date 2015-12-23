// Decompiled with JetBrains decompiler
// Type: Pathfinding.PathThreadInfo
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

namespace Pathfinding
{
  public struct PathThreadInfo
  {
    public int threadIndex;
    public AstarPath astar;
    public PathHandler runData;
    private object _lock;

    public object Lock
    {
      get
      {
        return this._lock;
      }
    }

    public PathThreadInfo(int index, AstarPath astar, PathHandler runData)
    {
      this.threadIndex = index;
      this.astar = astar;
      this.runData = runData;
      this._lock = new object();
    }
  }
}
