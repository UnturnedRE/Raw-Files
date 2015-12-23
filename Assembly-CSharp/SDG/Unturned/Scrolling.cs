// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.Scrolling
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class Scrolling : MonoBehaviour
  {
    public Material material;
    public float x;
    public float y;

    private void Update()
    {
      this.material.mainTextureOffset = new Vector2(this.x * Time.time, this.y * Time.time);
    }
  }
}
