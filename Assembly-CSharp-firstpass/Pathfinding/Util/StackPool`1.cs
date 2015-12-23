// Decompiled with JetBrains decompiler
// Type: Pathfinding.Util.StackPool`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.Util
{
  public static class StackPool<T>
  {
    private static List<Stack<T>> pool = new List<Stack<T>>();

    public static Stack<T> Claim()
    {
      if (StackPool<T>.pool.Count <= 0)
        return new Stack<T>();
      Stack<T> stack = StackPool<T>.pool[StackPool<T>.pool.Count - 1];
      StackPool<T>.pool.RemoveAt(StackPool<T>.pool.Count - 1);
      return stack;
    }

    public static void Warmup(int count)
    {
      Stack<T>[] stackArray = new Stack<T>[count];
      for (int index = 0; index < count; ++index)
        stackArray[index] = StackPool<T>.Claim();
      for (int index = 0; index < count; ++index)
        StackPool<T>.Release(stackArray[index]);
    }

    public static void Release(Stack<T> stack)
    {
      for (int index = 0; index < StackPool<T>.pool.Count; ++index)
      {
        if (StackPool<T>.pool[index] == stack)
          Debug.LogError((object) "The Stack is released even though it is inside the pool");
      }
      stack.Clear();
      StackPool<T>.pool.Add(stack);
    }

    public static void Clear()
    {
      StackPool<T>.pool.Clear();
    }

    public static int GetSize()
    {
      return StackPool<T>.pool.Count;
    }
  }
}
