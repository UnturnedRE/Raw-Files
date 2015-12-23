// Decompiled with JetBrains decompiler
// Type: Blur
// Assembly: Assembly-UnityScript-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ACB0EA2C-DE44-433E-98CB-A1A17DC32CFD
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-UnityScript-firstpass.dll

using System;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof (Camera))]
[AddComponentMenu("Image Effects/Blur/Blur (Optimized)")]
[Serializable]
public class Blur : PostEffectsBase
{
  [Range(0.0f, 2f)]
  public int downsample;
  [Range(0.0f, 10f)]
  public float blurSize;
  [Range(1f, 4f)]
  public int blurIterations;
  public Blur.BlurType blurType;
  public Shader blurShader;
  private Material blurMaterial;

  public Blur()
  {
    this.downsample = 1;
    this.blurSize = 3f;
    this.blurIterations = 2;
    this.blurType = Blur.BlurType.StandardGauss;
  }

  public override bool CheckResources()
  {
    this.CheckSupport(false);
    this.blurMaterial = this.CheckShaderAndCreateMaterial(this.blurShader, this.blurMaterial);
    if (!this.isSupported)
      this.ReportAutoDisable();
    return this.isSupported;
  }

  public virtual void OnDisable()
  {
    if (!(bool) ((UnityEngine.Object) this.blurMaterial))
      return;
    UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.blurMaterial);
  }

  public virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if (!this.CheckResources())
    {
      Graphics.Blit((Texture) source, destination);
    }
    else
    {
      float num1 = (float) (1.0 / (1.0 * (double) (1 << this.downsample)));
      this.blurMaterial.SetVector("_Parameter", new Vector4(this.blurSize * num1, -this.blurSize * num1, 0.0f, 0.0f));
      source.filterMode = FilterMode.Bilinear;
      int width = source.width >> this.downsample;
      int height = source.height >> this.downsample;
      RenderTexture renderTexture = RenderTexture.GetTemporary(width, height, 0, source.format);
      renderTexture.filterMode = FilterMode.Bilinear;
      Graphics.Blit((Texture) source, renderTexture, this.blurMaterial, 0);
      int num2 = this.blurType != Blur.BlurType.StandardGauss ? 2 : 0;
      for (int index = 0; index < this.blurIterations; ++index)
      {
        float num3 = (float) index * 1f;
        this.blurMaterial.SetVector("_Parameter", new Vector4(this.blurSize * num1 + num3, -this.blurSize * num1 - num3, 0.0f, 0.0f));
        RenderTexture temporary1 = RenderTexture.GetTemporary(width, height, 0, source.format);
        temporary1.filterMode = FilterMode.Bilinear;
        Graphics.Blit((Texture) renderTexture, temporary1, this.blurMaterial, 1 + num2);
        RenderTexture.ReleaseTemporary(renderTexture);
        RenderTexture temp = temporary1;
        RenderTexture temporary2 = RenderTexture.GetTemporary(width, height, 0, source.format);
        temporary2.filterMode = FilterMode.Bilinear;
        Graphics.Blit((Texture) temp, temporary2, this.blurMaterial, 2 + num2);
        RenderTexture.ReleaseTemporary(temp);
        renderTexture = temporary2;
      }
      Graphics.Blit((Texture) renderTexture, destination);
      RenderTexture.ReleaseTemporary(renderTexture);
    }
  }

  public override void Main()
  {
  }

  [Serializable]
  public enum BlurType
  {
    StandardGauss,
    SgxGauss,
  }
}
