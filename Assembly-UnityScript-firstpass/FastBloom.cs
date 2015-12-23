// Decompiled with JetBrains decompiler
// Type: FastBloom
// Assembly: Assembly-UnityScript-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ACB0EA2C-DE44-433E-98CB-A1A17DC32CFD
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-UnityScript-firstpass.dll

using System;
using UnityEngine;

[AddComponentMenu("Image Effects/Bloom and Glow/Bloom (Optimized)")]
[ExecuteInEditMode]
[RequireComponent(typeof (Camera))]
[Serializable]
public class FastBloom : PostEffectsBase
{
  [Range(0.0f, 1.5f)]
  public float threshhold;
  [Range(0.0f, 2.5f)]
  public float intensity;
  [Range(0.25f, 5.5f)]
  public float blurSize;
  public FastBloom.Resolution resolution;
  [Range(1f, 4f)]
  public int blurIterations;
  public FastBloom.BlurType blurType;
  public Shader fastBloomShader;
  private Material fastBloomMaterial;

  public FastBloom()
  {
    this.threshhold = 0.25f;
    this.intensity = 0.75f;
    this.blurSize = 1f;
    this.resolution = FastBloom.Resolution.Low;
    this.blurIterations = 1;
    this.blurType = FastBloom.BlurType.Standard;
  }

  public override bool CheckResources()
  {
    this.CheckSupport(false);
    this.fastBloomMaterial = this.CheckShaderAndCreateMaterial(this.fastBloomShader, this.fastBloomMaterial);
    if (!this.isSupported)
      this.ReportAutoDisable();
    return this.isSupported;
  }

  public virtual void OnDisable()
  {
    if (!(bool) ((UnityEngine.Object) this.fastBloomMaterial))
      return;
    UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.fastBloomMaterial);
  }

  public virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if (!this.CheckResources())
    {
      Graphics.Blit((Texture) source, destination);
    }
    else
    {
      int num1 = this.resolution != FastBloom.Resolution.Low ? 2 : 4;
      float num2 = this.resolution != FastBloom.Resolution.Low ? 1f : 0.5f;
      this.fastBloomMaterial.SetVector("_Parameter", new Vector4(this.blurSize * num2, 0.0f, this.threshhold, this.intensity));
      source.filterMode = FilterMode.Bilinear;
      int width = source.width / num1;
      int height = source.height / num1;
      RenderTexture renderTexture = RenderTexture.GetTemporary(width, height, 0, source.format);
      renderTexture.filterMode = FilterMode.Bilinear;
      Graphics.Blit((Texture) source, renderTexture, this.fastBloomMaterial, 1);
      int num3 = this.blurType != FastBloom.BlurType.Standard ? 2 : 0;
      for (int index = 0; index < this.blurIterations; ++index)
      {
        this.fastBloomMaterial.SetVector("_Parameter", new Vector4((float) ((double) this.blurSize * (double) num2 + (double) index * 1.0), 0.0f, this.threshhold, this.intensity));
        RenderTexture temporary1 = RenderTexture.GetTemporary(width, height, 0, source.format);
        temporary1.filterMode = FilterMode.Bilinear;
        Graphics.Blit((Texture) renderTexture, temporary1, this.fastBloomMaterial, 2 + num3);
        RenderTexture.ReleaseTemporary(renderTexture);
        RenderTexture temp = temporary1;
        RenderTexture temporary2 = RenderTexture.GetTemporary(width, height, 0, source.format);
        temporary2.filterMode = FilterMode.Bilinear;
        Graphics.Blit((Texture) temp, temporary2, this.fastBloomMaterial, 3 + num3);
        RenderTexture.ReleaseTemporary(temp);
        renderTexture = temporary2;
      }
      this.fastBloomMaterial.SetTexture("_Bloom", (Texture) renderTexture);
      Graphics.Blit((Texture) source, destination, this.fastBloomMaterial, 0);
      RenderTexture.ReleaseTemporary(renderTexture);
    }
  }

  public override void Main()
  {
  }

  [Serializable]
  public enum Resolution
  {
    Low,
    High,
  }

  [Serializable]
  public enum BlurType
  {
    Standard,
    Sgx,
  }
}
