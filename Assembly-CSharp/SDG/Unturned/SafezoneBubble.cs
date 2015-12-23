// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.SafezoneBubble
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class SafezoneBubble
  {
    public Vector3 origin;
    public float sqrRadius;

    public SafezoneBubble(Vector3 newOrigin, float newSqrRadius)
    {
      this.origin = newOrigin;
      this.sqrRadius = newSqrRadius;
    }
  }
}
