// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.BlueprintItemIconsInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class BlueprintItemIconsInfo
  {
    public Texture2D[] textures;
    public BlueprintItemIconsReady callback;
    private int index;

    public void onItemIconReady(Texture2D texture)
    {
      if (this.index >= this.textures.Length)
        return;
      this.textures[this.index] = texture;
      ++this.index;
      if (this.index != this.textures.Length || this.callback == null)
        return;
      this.callback();
    }
  }
}
