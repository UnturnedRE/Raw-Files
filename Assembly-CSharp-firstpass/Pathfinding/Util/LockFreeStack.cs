// Decompiled with JetBrains decompiler
// Type: Pathfinding.Util.LockFreeStack
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding;
using System.Threading;

namespace Pathfinding.Util
{
  public class LockFreeStack
  {
    public Path head;

    public void Push(Path p)
    {
      do
      {
        p.next = this.head;
      }
      while (Interlocked.CompareExchange<Path>(ref this.head, p, p.next) != p.next);
    }

    public Path PopAll()
    {
      return Interlocked.Exchange<Path>(ref this.head, (Path) null);
    }
  }
}
