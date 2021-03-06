﻿// Decompiled with JetBrains decompiler
// Type: GrayscaleEffect
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

[AddComponentMenu("Image Effects/Color Adjustments/Grayscale")]
[ExecuteInEditMode]
public class GrayscaleEffect : ImageEffectBase
{
  public Texture textureRamp;
  public float rampOffset;
  public float blend;

  private void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    this.material.SetTexture("_RampTex", this.textureRamp);
    this.material.SetFloat("_RampOffset", this.rampOffset);
    this.material.SetFloat("_Blend", this.blend);
    Graphics.Blit((Texture) source, destination, this.material);
  }
}
