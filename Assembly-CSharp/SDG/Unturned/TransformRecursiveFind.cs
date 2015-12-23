// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.TransformRecursiveFind
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public static class TransformRecursiveFind
  {
    public static Transform FindChildRecursive(this Transform parent, string name)
    {
      for (int index = 0; index < parent.childCount; ++index)
      {
        Transform child = parent.GetChild(index);
        if (child.name == name)
          return child;
        if (child.childCount != 0)
        {
          Transform childRecursive = TransformRecursiveFind.FindChildRecursive(child, name);
          if ((Object) childRecursive != (Object) null)
            return childRecursive;
        }
      }
      return (Transform) null;
    }
  }
}
