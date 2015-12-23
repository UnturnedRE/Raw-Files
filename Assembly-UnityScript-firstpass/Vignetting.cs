// Decompiled with JetBrains decompiler
// Type: Vignetting
// Assembly: Assembly-UnityScript-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ACB0EA2C-DE44-433E-98CB-A1A17DC32CFD
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-UnityScript-firstpass.dll

using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Camera/Vignette and Chromatic Aberration")]
[RequireComponent(typeof (Camera))]
[Serializable]
public class Vignetting : PostEffectsBase
{
  public Vignetting.AberrationMode mode;
  public float intensity;
  public float chromaticAberration;
  public float axialAberration;
  public float blur;
  public float blurSpread;
  public float luminanceDependency;
  public float blurDistance;
  public Shader vignetteShader;
  private Material vignetteMaterial;
  public Shader separableBlurShader;
  private Material separableBlurMaterial;
  public Shader chromAberrationShader;
  private Material chromAberrationMaterial;

  public Vignetting()
  {
    this.mode = Vignetting.AberrationMode.Simple;
    this.intensity = 0.375f;
    this.chromaticAberration = 0.2f;
    this.axialAberration = 0.5f;
    this.blurSpread = 0.75f;
    this.luminanceDependency = 0.25f;
    this.blurDistance = 2.5f;
  }

  public override bool CheckResources()
  {
    this.CheckSupport(false);
    this.vignetteMaterial = this.CheckShaderAndCreateMaterial(this.vignetteShader, this.vignetteMaterial);
    this.separableBlurMaterial = this.CheckShaderAndCreateMaterial(this.separableBlurShader, this.separableBlurMaterial);
    this.chromAberrationMaterial = this.CheckShaderAndCreateMaterial(this.chromAberrationShader, this.chromAberrationMaterial);
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
      int num1 = (double) Mathf.Abs(this.blur) > 0.0 ? 1 : 0;
      if (num1 == 0)
        num1 = (double) Mathf.Abs(this.intensity) > 0.0 ? 1 : 0;
      bool flag = num1 != 0;
      float num2 = (float) (1.0 * (double) width / (1.0 * (double) height));
      float num3 = 1.0 / 512.0;
      RenderTexture renderTexture1 = (RenderTexture) null;
      RenderTexture renderTexture2 = (RenderTexture) null;
      if (flag)
      {
        renderTexture1 = RenderTexture.GetTemporary(width, height, 0, source.format);
        if ((double) Mathf.Abs(this.blur) > 0.0)
        {
          renderTexture2 = RenderTexture.GetTemporary(width / 2, height / 2, 0, source.format);
          Graphics.Blit((Texture) source, renderTexture2, this.chromAberrationMaterial, 0);
          for (int index = 0; index < 2; ++index)
          {
            this.separableBlurMaterial.SetVector("offsets", new Vector4(0.0f, this.blurSpread * num3, 0.0f, 0.0f));
            RenderTexture temporary = RenderTexture.GetTemporary(width / 2, height / 2, 0, source.format);
            Graphics.Blit((Texture) renderTexture2, temporary, this.separableBlurMaterial);
            RenderTexture.ReleaseTemporary(renderTexture2);
            this.separableBlurMaterial.SetVector("offsets", new Vector4(this.blurSpread * num3 / num2, 0.0f, 0.0f, 0.0f));
            renderTexture2 = RenderTexture.GetTemporary(width / 2, height / 2, 0, source.format);
            Graphics.Blit((Texture) temporary, renderTexture2, this.separableBlurMaterial);
            RenderTexture.ReleaseTemporary(temporary);
          }
        }
        this.vignetteMaterial.SetFloat("_Intensity", this.intensity);
        this.vignetteMaterial.SetFloat("_Blur", this.blur);
        this.vignetteMaterial.SetTexture("_VignetteTex", (Texture) renderTexture2);
        Graphics.Blit((Texture) source, renderTexture1, this.vignetteMaterial, 0);
      }
      this.chromAberrationMaterial.SetFloat("_ChromaticAberration", this.chromaticAberration);
      this.chromAberrationMaterial.SetFloat("_AxialAberration", this.axialAberration);
      this.chromAberrationMaterial.SetVector("_BlurDistance", (Vector4) new Vector2(-this.blurDistance, this.blurDistance));
      this.chromAberrationMaterial.SetFloat("_Luminance", 1f / Mathf.Max(Mathf.Epsilon, this.luminanceDependency));
      if (flag)
        renderTexture1.wrapMode = TextureWrapMode.Clamp;
      else
        source.wrapMode = TextureWrapMode.Clamp;
      Graphics.Blit(!flag ? (Texture) source : (Texture) renderTexture1, destination, this.chromAberrationMaterial, this.mode != Vignetting.AberrationMode.Advanced ? 1 : 2);
      RenderTexture.ReleaseTemporary(renderTexture1);
      RenderTexture.ReleaseTemporary(renderTexture2);
    }
  }

  public override void Main()
  {
  }

  [Serializable]
  public enum AberrationMode
  {
    Simple,
    Advanced,
  }
}
