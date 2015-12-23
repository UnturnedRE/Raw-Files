// Decompiled with JetBrains decompiler
// Type: SepiaToneEffect
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Color Adjustments/Sepia Tone")]
public class SepiaToneEffect : ImageEffectBase
{
  private void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    Graphics.Blit((Texture) source, destination, this.material);
  }
}
