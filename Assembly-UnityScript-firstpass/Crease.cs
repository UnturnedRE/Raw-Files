// Decompiled with JetBrains decompiler
// Type: Crease
// Assembly: Assembly-UnityScript-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ACB0EA2C-DE44-433E-98CB-A1A17DC32CFD
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-UnityScript-firstpass.dll

using System;
using UnityEngine;

[AddComponentMenu("Image Effects/Edge Detection/Crease Shading")]
[ExecuteInEditMode]
[RequireComponent(typeof (Camera))]
[Serializable]
public class Crease : PostEffectsBase
{
  public float intensity;
  public int softness;
  public float spread;
  public Shader blurShader;
  private Material blurMaterial;
  public Shader depthFetchShader;
  private Material depthFetchMaterial;
  public Shader creaseApplyShader;
  private Material creaseApplyMaterial;

  public Crease()
  {
    this.intensity = 0.5f;
    this.softness = 1;
    this.spread = 1f;
  }

  public override bool CheckResources()
  {
    this.CheckSupport(true);
    this.blurMaterial = this.CheckShaderAndCreateMaterial(this.blurShader, this.blurMaterial);
    this.depthFetchMaterial = this.CheckShaderAndCreateMaterial(this.depthFetchShader, this.depthFetchMaterial);
    this.creaseApplyMaterial = this.CheckShaderAndCreateMaterial(this.creaseApplyShader, this.creaseApplyMaterial);
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
      float num1 = (float) (1.0 * (double) width / (1.0 * (double) height));
      float num2 = 1.0 / 512.0;
      RenderTexture temporary1 = RenderTexture.GetTemporary(width, height, 0);
      RenderTexture renderTexture = RenderTexture.GetTemporary(width / 2, height / 2, 0);
      Graphics.Blit((Texture) source, temporary1, this.depthFetchMaterial);
      Graphics.Blit((Texture) temporary1, renderTexture);
      for (int index = 0; index < this.softness; ++index)
      {
        RenderTexture temporary2 = RenderTexture.GetTemporary(width / 2, height / 2, 0);
        this.blurMaterial.SetVector("offsets", new Vector4(0.0f, this.spread * num2, 0.0f, 0.0f));
        Graphics.Blit((Texture) renderTexture, temporary2, this.blurMaterial);
        RenderTexture.ReleaseTemporary(renderTexture);
        RenderTexture temp = temporary2;
        RenderTexture temporary3 = RenderTexture.GetTemporary(width / 2, height / 2, 0);
        this.blurMaterial.SetVector("offsets", new Vector4(this.spread * num2 / num1, 0.0f, 0.0f, 0.0f));
        Graphics.Blit((Texture) temp, temporary3, this.blurMaterial);
        RenderTexture.ReleaseTemporary(temp);
        renderTexture = temporary3;
      }
      this.creaseApplyMaterial.SetTexture("_HrDepthTex", (Texture) temporary1);
      this.creaseApplyMaterial.SetTexture("_LrDepthTex", (Texture) renderTexture);
      this.creaseApplyMaterial.SetFloat("intensity", this.intensity);
      Graphics.Blit((Texture) source, destination, this.creaseApplyMaterial);
      RenderTexture.ReleaseTemporary(temporary1);
      RenderTexture.ReleaseTemporary(renderTexture);
    }
  }

  public override void Main()
  {
  }
}
