// Decompiled with JetBrains decompiler
// Type: HighlightingSystem.HighlighterManager
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using UnityEngine;

namespace HighlightingSystem
{
  public static class HighlighterManager
  {
    private static int dirtyFrame = -1;
    private static HashSet<Highlighter> highlighters = new HashSet<Highlighter>();

    public static bool isDirty
    {
      get
      {
        return HighlighterManager.dirtyFrame == Time.frameCount;
      }
      private set
      {
        HighlighterManager.dirtyFrame = !value ? -1 : Time.frameCount;
      }
    }

    public static void Add(Highlighter highlighter)
    {
      HighlighterManager.highlighters.Add(highlighter);
    }

    public static void Remove(Highlighter instance)
    {
      if (!HighlighterManager.highlighters.Remove(instance) || !instance.highlighted)
        return;
      HighlighterManager.isDirty = true;
    }

    public static HashSet<Highlighter>.Enumerator GetEnumerator()
    {
      return HighlighterManager.highlighters.GetEnumerator();
    }
  }
}
