// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.SafezoneManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
  public class SafezoneManager : MonoBehaviour
  {
    private static List<SafezoneBubble> bubbles;
    private static SafezoneManager manager;

    public static bool checkPointValid(Vector3 point)
    {
      for (int index = 0; index < SafezoneManager.bubbles.Count; ++index)
      {
        SafezoneBubble safezoneBubble = SafezoneManager.bubbles[index];
        if ((double) (safezoneBubble.origin - point).sqrMagnitude < (double) safezoneBubble.sqrRadius)
          return false;
      }
      return true;
    }

    public static SafezoneBubble registerBubble(Vector3 origin, float radius)
    {
      SafezoneBubble safezoneBubble = new SafezoneBubble(origin, radius * radius);
      SafezoneManager.bubbles.Add(safezoneBubble);
      return safezoneBubble;
    }

    public static void deregisterBubble(SafezoneBubble bubble)
    {
      SafezoneManager.bubbles.Remove(bubble);
    }

    private void onLevelLoaded(int level)
    {
      SafezoneManager.bubbles = new List<SafezoneBubble>();
    }

    private void Start()
    {
      SafezoneManager.manager = this;
      Level.onPrePreLevelLoaded += new PrePreLevelLoaded(this.onLevelLoaded);
    }
  }
}
