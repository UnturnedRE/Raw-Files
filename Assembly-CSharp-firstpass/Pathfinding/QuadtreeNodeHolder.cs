// Decompiled with JetBrains decompiler
// Type: Pathfinding.QuadtreeNodeHolder
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

namespace Pathfinding
{
  public class QuadtreeNodeHolder
  {
    public QuadtreeNode node;
    public QuadtreeNodeHolder c0;
    public QuadtreeNodeHolder c1;
    public QuadtreeNodeHolder c2;
    public QuadtreeNodeHolder c3;

    public void GetNodes(GraphNodeDelegateCancelable del)
    {
      if (this.node != null)
      {
        int num = del((GraphNode) this.node) ? 1 : 0;
      }
      else
      {
        this.c0.GetNodes(del);
        this.c1.GetNodes(del);
        this.c2.GetNodes(del);
        this.c3.GetNodes(del);
      }
    }
  }
}
