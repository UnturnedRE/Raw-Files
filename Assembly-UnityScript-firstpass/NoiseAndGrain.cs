// Decompiled with JetBrains decompiler
// Type: NoiseAndGrain
// Assembly: Assembly-UnityScript-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ACB0EA2C-DE44-433E-98CB-A1A17DC32CFD
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-UnityScript-firstpass.dll

using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Noise/Noise And Grain (Filmic)")]
[RequireComponent(typeof (Camera))]
[Serializable]
public class NoiseAndGrain : PostEffectsBase
{
  [NonSerialized]
  private static float TILE_AMOUNT = 64f;
  public float intensityMultiplier;
  public float generalIntensity;
  public float blackIntensity;
  public float whiteIntensity;
  public float midGrey;
  public bool dx11Grain;
  public float softness;
  public bool monochrome;
  public Vector3 intensities;
  public Vector3 tiling;
  public float monochromeTiling;
  public FilterMode filterMode;
  public Texture2D noiseTexture;
  public Shader noiseShader;
  private Material noiseMaterial;
  public Shader dx11NoiseShader;
  private Material dx11NoiseMaterial;

  public NoiseAndGrain()
  {
    this.intensityMultiplier = 0.25f;
    this.generalIntensity = 0.5f;
    this.blackIntensity = 1f;
    this.whiteIntensity = 1f;
    this.midGrey = 0.2f;
    this.intensities = new Vector3(1f, 1f, 1f);
    this.tiling = new Vector3(64f, 64f, 64f);
    this.monochromeTiling = 64f;
    this.filterMode = FilterMode.Bilinear;
  }

  public override bool CheckResources()
  {
    this.CheckSupport(false);
    this.noiseMaterial = this.CheckShaderAndCreateMaterial(this.noiseShader, this.noiseMaterial);
    if (this.dx11Grain && this.supportDX11)
      this.dx11NoiseMaterial = this.CheckShaderAndCreateMaterial(this.dx11NoiseShader, this.dx11NoiseMaterial);
    if (!this.isSupported)
      this.ReportAutoDisable();
    return this.isSupported;
  }

  public virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if (!this.CheckResources() || (UnityEngine.Object) null == (UnityEngine.Object) this.noiseTexture)
    {
      Graphics.Blit((Texture) source, destination);
      if (!((UnityEngine.Object) null == (UnityEngine.Object) this.noiseTexture))
        return;
      Debug.LogWarning((object) "Noise & Grain effect failing as noise texture is not assigned. please assign.", (UnityEngine.Object) this.transform);
    }
    else
    {
      this.softness = Mathf.Clamp(this.softness, 0.0f, 0.99f);
      if (this.dx11Grain && this.supportDX11)
      {
        this.dx11NoiseMaterial.SetFloat("_DX11NoiseTime", (float) Time.frameCount);
        this.dx11NoiseMaterial.SetTexture("_NoiseTex", (Texture) this.noiseTexture);
        this.dx11NoiseMaterial.SetVector("_NoisePerChannel", (Vector4) (!this.monochrome ? this.intensities : Vector3.one));
        this.dx11NoiseMaterial.SetVector("_MidGrey", (Vector4) new Vector3(this.midGrey, (float) (1.0 / (1.0 - (double) this.midGrey)), -1f / this.midGrey));
        this.dx11NoiseMaterial.SetVector("_NoiseAmount", (Vector4) (new Vector3(this.generalIntensity, this.blackIntensity, this.whiteIntensity) * this.intensityMultiplier));
        if ((double) this.softness > (double) Mathf.Epsilon)
        {
          RenderTexture temporary = RenderTexture.GetTemporary((int) ((double) source.width * (1.0 - (double) this.softness)), (int) ((double) source.height * (1.0 - (double) this.softness)));
          NoiseAndGrain.DrawNoiseQuadGrid(source, temporary, this.dx11NoiseMaterial, this.noiseTexture, !this.monochrome ? 2 : 3);
          this.dx11NoiseMaterial.SetTexture("_NoiseTex", (Texture) temporary);
          Graphics.Blit((Texture) source, destination, this.dx11NoiseMaterial, 4);
          RenderTexture.ReleaseTemporary(temporary);
        }
        else
          NoiseAndGrain.DrawNoiseQuadGrid(source, destination, this.dx11NoiseMaterial, this.noiseTexture, !this.monochrome ? 0 : 1);
      }
      else
      {
        if ((bool) ((UnityEngine.Object) this.noiseTexture))
        {
          this.noiseTexture.wrapMode = TextureWrapMode.Repeat;
          this.noiseTexture.filterMode = this.filterMode;
        }
        this.noiseMaterial.SetTexture("_NoiseTex", (Texture) this.noiseTexture);
        this.noiseMaterial.SetVector("_NoisePerChannel", (Vector4) (!this.monochrome ? this.intensities : Vector3.one));
        this.noiseMaterial.SetVector("_NoiseTilingPerChannel", (Vector4) (!this.monochrome ? this.tiling : Vector3.one * this.monochromeTiling));
        this.noiseMaterial.SetVector("_MidGrey", (Vector4) new Vector3(this.midGrey, (float) (1.0 / (1.0 - (double) this.midGrey)), -1f / this.midGrey));
        this.noiseMaterial.SetVector("_NoiseAmount", (Vector4) (new Vector3(this.generalIntensity, this.blackIntensity, this.whiteIntensity) * this.intensityMultiplier));
        if ((double) this.softness > (double) Mathf.Epsilon)
        {
          RenderTexture temporary = RenderTexture.GetTemporary((int) ((double) source.width * (1.0 - (double) this.softness)), (int) ((double) source.height * (1.0 - (double) this.softness)));
          NoiseAndGrain.DrawNoiseQuadGrid(source, temporary, this.noiseMaterial, this.noiseTexture, 2);
          this.noiseMaterial.SetTexture("_NoiseTex", (Texture) temporary);
          Graphics.Blit((Texture) source, destination, this.noiseMaterial, 1);
          RenderTexture.ReleaseTemporary(temporary);
        }
        else
          NoiseAndGrain.DrawNoiseQuadGrid(source, destination, this.noiseMaterial, this.noiseTexture, 0);
      }
    }
  }

  public static void DrawNoiseQuadGrid(RenderTexture source, RenderTexture dest, Material fxMaterial, Texture2D noise, int passNr)
  {
    RenderTexture.active = dest;
    float num1 = (float) noise.width * 1f;
    float num2 = 1f * (float) source.width / NoiseAndGrain.TILE_AMOUNT;
    fxMaterial.SetTexture("_MainTex", (Texture) source);
    GL.PushMatrix();
    GL.LoadOrtho();
    float num3 = (float) (1.0 * (double) source.width / (1.0 * (double) source.height));
    float num4 = 1f / num2;
    float num5 = num4 * num3;
    float num6 = num1 / ((float) noise.width * 1f);
    fxMaterial.SetPass(passNr);
    GL.Begin(7);
    float x1 = 0.0f;
    while ((double) x1 < 1.0)
    {
      float y1 = 0.0f;
      while ((double) y1 < 1.0)
      {
        float num7 = UnityEngine.Random.Range(0.0f, 1f);
        float num8 = UnityEngine.Random.Range(0.0f, 1f);
        float x2 = Mathf.Floor(num7 * num1) / num1;
        float y2 = Mathf.Floor(num8 * num1) / num1;
        float num9 = 1f / num1;
        GL.MultiTexCoord2(0, x2, y2);
        GL.MultiTexCoord2(1, 0.0f, 0.0f);
        GL.Vertex3(x1, y1, 0.1f);
        GL.MultiTexCoord2(0, x2 + num6 * num9, y2);
        GL.MultiTexCoord2(1, 1f, 0.0f);
        GL.Vertex3(x1 + num4, y1, 0.1f);
        GL.MultiTexCoord2(0, x2 + num6 * num9, y2 + num6 * num9);
        GL.MultiTexCoord2(1, 1f, 1f);
        GL.Vertex3(x1 + num4, y1 + num5, 0.1f);
        GL.MultiTexCoord2(0, x2, y2 + num6 * num9);
        GL.MultiTexCoord2(1, 0.0f, 1f);
        GL.Vertex3(x1, y1 + num5, 0.1f);
        y1 += num5;
      }
      x1 += num4;
    }
    GL.End();
    GL.PopMatrix();
  }

  public override void Main()
  {
  }
}
