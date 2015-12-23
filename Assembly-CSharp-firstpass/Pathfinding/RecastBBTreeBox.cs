// Decompiled with JetBrains decompiler
// Type: Pathfinding.RecastBBTreeBox
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

namespace Pathfinding
{
  public class RecastBBTreeBox
  {
    public Rect rect;
    public RecastMeshObj mesh;
    public RecastBBTreeBox c1;
    public RecastBBTreeBox c2;

    public RecastBBTreeBox(RecastBBTree tree, RecastMeshObj mesh)
    {
      this.mesh = mesh;
      Vector3 min = mesh.bounds.min;
      Vector3 max = mesh.bounds.max;
      this.rect = Rect.MinMaxRect(min.x, min.z, max.x, max.z);
    }

    public bool Contains(Vector3 p)
    {
      return this.rect.Contains(p);
    }

    public void WriteChildren(int level)
    {
      for (int index = 0; index < level; ++index)
        Console.Write("  ");
      if ((UnityEngine.Object) this.mesh != (UnityEngine.Object) null)
      {
        Console.WriteLine("Leaf ");
      }
      else
      {
        Console.WriteLine("Box ");
        this.c1.WriteChildren(level + 1);
        this.c2.WriteChildren(level + 1);
      }
    }
  }
}
