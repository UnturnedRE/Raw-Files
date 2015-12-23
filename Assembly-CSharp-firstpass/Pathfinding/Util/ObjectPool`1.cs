// Decompiled with JetBrains decompiler
// Type: Pathfinding.Util.ObjectPool`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

namespace Pathfinding.Util
{
  public static class ObjectPool<T> where T : class, IAstarPooledObject, new()
  {
    private static List<T> pool = new List<T>();

    public static T Claim()
    {
      if (ObjectPool<T>.pool.Count <= 0)
        return Activator.CreateInstance<T>();
      T obj = ObjectPool<T>.pool[ObjectPool<T>.pool.Count - 1];
      ObjectPool<T>.pool.RemoveAt(ObjectPool<T>.pool.Count - 1);
      return obj;
    }

    public static void Warmup(int count)
    {
      T[] objArray = new T[count];
      for (int index = 0; index < count; ++index)
        objArray[index] = ObjectPool<T>.Claim();
      for (int index = 0; index < count; ++index)
        ObjectPool<T>.Release(objArray[index]);
    }

    public static void Release(T obj)
    {
      for (int index = 0; index < ObjectPool<T>.pool.Count; ++index)
      {
        if ((object) ObjectPool<T>.pool[index] == (object) obj)
          throw new InvalidOperationException("The object is released even though it is in the pool. Are you releasing it twice?");
      }
      obj.OnEnterPool();
      ObjectPool<T>.pool.Add(obj);
    }

    public static void Clear()
    {
      ObjectPool<T>.pool.Clear();
    }

    public static int GetSize()
    {
      return ObjectPool<T>.pool.Count;
    }
  }
}
