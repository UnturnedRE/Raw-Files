// Decompiled with JetBrains decompiler
// Type: Pathfinding.Util.ListPool`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

namespace Pathfinding.Util
{
  public static class ListPool<T>
  {
    private static List<List<T>> pool = new List<List<T>>();
    private const int MaxCapacitySearchLength = 8;

    public static List<T> Claim()
    {
      lock (ListPool<T>.pool)
      {
        if (ListPool<T>.pool.Count <= 0)
          return new List<T>();
        List<T> local_1 = ListPool<T>.pool[ListPool<T>.pool.Count - 1];
        ListPool<T>.pool.RemoveAt(ListPool<T>.pool.Count - 1);
        return local_1;
      }
    }

    public static List<T> Claim(int capacity)
    {
      lock (ListPool<T>.pool)
      {
        if (ListPool<T>.pool.Count <= 0)
          return new List<T>(capacity);
        List<T> local_1 = (List<T>) null;
        int local_2;
        for (local_2 = 0; local_2 < ListPool<T>.pool.Count && local_2 < 8; ++local_2)
        {
          local_1 = ListPool<T>.pool[ListPool<T>.pool.Count - 1 - local_2];
          if (local_1.Capacity >= capacity)
          {
            ListPool<T>.pool.RemoveAt(ListPool<T>.pool.Count - 1 - local_2);
            return local_1;
          }
        }
        if (local_1 == null)
        {
          local_1 = new List<T>(capacity);
        }
        else
        {
          local_1.Capacity = capacity;
          ListPool<T>.pool[ListPool<T>.pool.Count - local_2] = ListPool<T>.pool[ListPool<T>.pool.Count - 1];
          ListPool<T>.pool.RemoveAt(ListPool<T>.pool.Count - 1);
        }
        return local_1;
      }
    }

    public static void Warmup(int count, int size)
    {
      lock (ListPool<T>.pool)
      {
        List<T>[] local_1 = new List<T>[count];
        for (int local_2 = 0; local_2 < count; ++local_2)
          local_1[local_2] = ListPool<T>.Claim(size);
        for (int local_3 = 0; local_3 < count; ++local_3)
          ListPool<T>.Release(local_1[local_3]);
      }
    }

    public static void Release(List<T> list)
    {
      list.Clear();
      lock (ListPool<T>.pool)
      {
        for (int local_1 = 0; local_1 < ListPool<T>.pool.Count; ++local_1)
        {
          if (ListPool<T>.pool[local_1] == list)
            throw new InvalidOperationException("The List is released even though it is in the pool");
        }
        ListPool<T>.pool.Add(list);
      }
    }

    public static void Clear()
    {
      lock (ListPool<T>.pool)
        ListPool<T>.pool.Clear();
    }

    public static int GetSize()
    {
      return ListPool<T>.pool.Count;
    }
  }
}
