﻿// Decompiled with JetBrains decompiler
// Type: GlowEffect
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

[RequireComponent(typeof (Camera))]
[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Bloom and Glow/Glow (Deprecated)")]
public class GlowEffect : MonoBehaviour
{
  public float glowIntensity = 1.5f;
  public int blurIterations = 3;
  public float blurSpread = 0.7f;
  public Color glowTint = new Color(1f, 1f, 1f, 0.0f);
  public Shader compositeShader;
  private Material m_CompositeMaterial;
  public Shader blurShader;
  private Material m_BlurMaterial;
  public Shader downsampleShader;
  private Material m_DownsampleMaterial;

  protected Material compositeMaterial
  {
    get
    {
      if ((Object) this.m_CompositeMaterial == (Object) null)
      {
        this.m_CompositeMaterial = new Material(this.compositeShader);
        this.m_CompositeMaterial.hideFlags = HideFlags.HideAndDontSave;
      }
      return this.m_CompositeMaterial;
    }
  }

  protected Material blurMaterial
  {
    get
    {
      if ((Object) this.m_BlurMaterial == (Object) null)
      {
        this.m_BlurMaterial = new Material(this.blurShader);
        this.m_BlurMaterial.hideFlags = HideFlags.HideAndDontSave;
      }
      return this.m_BlurMaterial;
    }
  }

  protected Material downsampleMaterial
  {
    get
    {
      if ((Object) this.m_DownsampleMaterial == (Object) null)
      {
        this.m_DownsampleMaterial = new Material(this.downsampleShader);
        this.m_DownsampleMaterial.hideFlags = HideFlags.HideAndDontSave;
      }
      return this.m_DownsampleMaterial;
    }
  }

  protected void OnDisable()
  {
    if ((bool) ((Object) this.m_CompositeMaterial))
      Object.DestroyImmediate((Object) this.m_CompositeMaterial);
    if ((bool) ((Object) this.m_BlurMaterial))
      Object.DestroyImmediate((Object) this.m_BlurMaterial);
    if (!(bool) ((Object) this.m_DownsampleMaterial))
      return;
    Object.DestroyImmediate((Object) this.m_DownsampleMaterial);
  }

  protected void Start()
  {
    if (!SystemInfo.supportsImageEffects)
      this.enabled = false;
    else if ((Object) this.downsampleShader == (Object) null)
    {
      Debug.Log((object) "No downsample shader assigned! Disabling glow.");
      this.enabled = false;
    }
    else
    {
      if (!this.blurMaterial.shader.isSupported)
        this.enabled = false;
      if (!this.compositeMaterial.shader.isSupported)
        this.enabled = false;
      if (this.downsampleMaterial.shader.isSupported)
        return;
      this.enabled = false;
    }
  }

  public void FourTapCone(RenderTexture source, RenderTexture dest, int iteration)
  {
    float num = (float) (0.5 + (double) iteration * (double) this.blurSpread);
    Graphics.BlitMultiTap((Texture) source, dest, this.blurMaterial, new Vector2(num, num), new Vector2(-num, num), new Vector2(num, -num), new Vector2(-num, -num));
  }

  private void DownSample4x(RenderTexture source, RenderTexture dest)
  {
    this.downsampleMaterial.color = new Color(this.glowTint.r, this.glowTint.g, this.glowTint.b, this.glowTint.a / 4f);
    Graphics.Blit((Texture) source, dest, this.downsampleMaterial);
  }

  private void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    this.glowIntensity = Mathf.Clamp(this.glowIntensity, 0.0f, 10f);
    this.blurIterations = Mathf.Clamp(this.blurIterations, 0, 30);
    this.blurSpread = Mathf.Clamp(this.blurSpread, 0.5f, 1f);
    int width = source.width / 4;
    int height = source.height / 4;
    RenderTexture renderTexture = RenderTexture.GetTemporary(width, height, 0);
    this.DownSample4x(source, renderTexture);
    this.blurMaterial.color = new Color(1f, 1f, 1f, 0.25f + Mathf.Clamp01((float) (((double) this.glowIntensity - 1.0) / 4.0)));
    for (int iteration = 0; iteration < this.blurIterations; ++iteration)
    {
      RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0);
      this.FourTapCone(renderTexture, temporary, iteration);
      RenderTexture.ReleaseTemporary(renderTexture);
      renderTexture = temporary;
    }
    Graphics.Blit((Texture) source, destination);
    this.BlitGlow(renderTexture, destination);
    RenderTexture.ReleaseTemporary(renderTexture);
  }

  public void BlitGlow(RenderTexture source, RenderTexture dest)
  {
    this.compositeMaterial.color = new Color(1f, 1f, 1f, Mathf.Clamp01(this.glowIntensity));
    Graphics.Blit((Texture) source, dest, this.compositeMaterial);
  }
}
