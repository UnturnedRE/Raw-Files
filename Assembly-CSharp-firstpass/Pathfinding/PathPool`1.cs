// Decompiled with JetBrains decompiler
// Type: Pathfinding.PathPool`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
  public static class PathPool<T> where T : Path, new()
  {
    private static Stack<T> pool = new Stack<T>();
    private static int totalCreated;

    public static void Recycle(T path)
    {
      lock (PathPool<T>.pool)
      {
        path.recycled = true;
        path.OnEnterPool();
        PathPool<T>.pool.Push(path);
      }
    }

    public static void Warmup(int count, int length)
    {
      ListPool<GraphNode>.Warmup(count, length);
      ListPool<Vector3>.Warmup(count, length);
      Path[] pathArray = new Path[count];
      for (int index = 0; index < count; ++index)
      {
        pathArray[index] = (Path) PathPool<T>.GetPath();
        pathArray[index].Claim((object) pathArray);
      }
      for (int index = 0; index < count; ++index)
        pathArray[index].Release((object) pathArray);
    }

    public static int GetTotalCreated()
    {
      return PathPool<T>.totalCreated;
    }

    public static int GetSize()
    {
      return PathPool<T>.pool.Count;
    }

    public static T GetPath()
    {
      lock (PathPool<T>.pool)
      {
        T local_1;
        if (PathPool<T>.pool.Count > 0)
        {
          local_1 = PathPool<T>.pool.Pop();
        }
        else
        {
          local_1 = Activator.CreateInstance<T>();
          ++PathPool<T>.totalCreated;
        }
        local_1.recycled = false;
        local_1.Reset();
        return local_1;
      }
    }
  }
}
