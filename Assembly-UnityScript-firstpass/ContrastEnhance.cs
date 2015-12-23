// Decompiled with JetBrains decompiler
// Type: ContrastEnhance
// Assembly: Assembly-UnityScript-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ACB0EA2C-DE44-433E-98CB-A1A17DC32CFD
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-UnityScript-firstpass.dll

using System;
using UnityEngine;

[AddComponentMenu("Image Effects/Color Adjustments/Contrast Enhance (Unsharp Mask)")]
[ExecuteInEditMode]
[RequireComponent(typeof (Camera))]
[Serializable]
public class ContrastEnhance : PostEffectsBase
{
  public float intensity;
  public float threshhold;
  private Material separableBlurMaterial;
  private Material contrastCompositeMaterial;
  public float blurSpread;
  public Shader separableBlurShader;
  public Shader contrastCompositeShader;

  public ContrastEnhance()
  {
    this.intensity = 0.5f;
    this.blurSpread = 1f;
  }

  public override bool CheckResources()
  {
    this.CheckSupport(false);
    this.contrastCompositeMaterial = this.CheckShaderAndCreateMaterial(this.contrastCompositeShader, this.contrastCompositeMaterial);
    this.separableBlurMaterial = this.CheckShaderAndCreateMaterial(this.separableBlurShader, this.separableBlurMaterial);
    if (!this.isSupported)
      this.ReportAutoDisable();
    return this.isSupported;
  }

  public virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if (!this.CheckResources())
    {
      Graphics.Blit((Texture) source, destination);
    }
    else
    {
      int width = source.width;
      int height = source.height;
      RenderTexture temporary1 = RenderTexture.GetTemporary(width / 2, height / 2, 0);
      Graphics.Blit((Texture) source, temporary1);
      RenderTexture temporary2 = RenderTexture.GetTemporary(width / 4, height / 4, 0);
      Graphics.Blit((Texture) temporary1, temporary2);
      RenderTexture.ReleaseTemporary(temporary1);
      this.separableBlurMaterial.SetVector("offsets", new Vector4(0.0f, this.blurSpread * 1f / (float) temporary2.height, 0.0f, 0.0f));
      RenderTexture temporary3 = RenderTexture.GetTemporary(width / 4, height / 4, 0);
      Graphics.Blit((Texture) temporary2, temporary3, this.separableBlurMaterial);
      RenderTexture.ReleaseTemporary(temporary2);
      this.separableBlurMaterial.SetVector("offsets", new Vector4(this.blurSpread * 1f / (float) temporary2.width, 0.0f, 0.0f, 0.0f));
      RenderTexture temporary4 = RenderTexture.GetTemporary(width / 4, height / 4, 0);
      Graphics.Blit((Texture) temporary3, temporary4, this.separableBlurMaterial);
      RenderTexture.ReleaseTemporary(temporary3);
      this.contrastCompositeMaterial.SetTexture("_MainTexBlurred", (Texture) temporary4);
      this.contrastCompositeMaterial.SetFloat("intensity", this.intensity);
      this.contrastCompositeMaterial.SetFloat("threshhold", this.threshhold);
      Graphics.Blit((Texture) source, destination, this.contrastCompositeMaterial);
      RenderTexture.ReleaseTemporary(temporary4);
    }
  }

  public override void Main()
  {
  }
}
