// Decompiled with JetBrains decompiler
// Type: TiltShiftHdr
// Assembly: Assembly-UnityScript-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ACB0EA2C-DE44-433E-98CB-A1A17DC32CFD
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-UnityScript-firstpass.dll

using System;
using UnityEngine;

[RequireComponent(typeof (Camera))]
[AddComponentMenu("Image Effects/Camera/Tilt Shift (Lens Blur)")]
[ExecuteInEditMode]
[Serializable]
public class TiltShiftHdr : PostEffectsBase
{
  public TiltShiftHdr.TiltShiftMode mode;
  public TiltShiftHdr.TiltShiftQuality quality;
  [Range(0.0f, 15f)]
  public float blurArea;
  [Range(0.0f, 25f)]
  public float maxBlurSize;
  [Range(0.0f, 1f)]
  public int downsample;
  public Shader tiltShiftShader;
  private Material tiltShiftMaterial;

  public TiltShiftHdr()
  {
    this.mode = TiltShiftHdr.TiltShiftMode.TiltShiftMode;
    this.quality = TiltShiftHdr.TiltShiftQuality.Normal;
    this.blurArea = 1f;
    this.maxBlurSize = 5f;
  }

  public override bool CheckResources()
  {
    this.CheckSupport(true);
    this.tiltShiftMaterial = this.CheckShaderAndCreateMaterial(this.tiltShiftShader, this.tiltShiftMaterial);
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
      this.tiltShiftMaterial.SetFloat("_BlurSize", (double) this.maxBlurSize >= 0.0 ? this.maxBlurSize : 0.0f);
      this.tiltShiftMaterial.SetFloat("_BlurArea", this.blurArea);
      source.filterMode = FilterMode.Bilinear;
      RenderTexture renderTexture = destination;
      if (this.downsample != 0)
      {
        renderTexture = RenderTexture.GetTemporary(source.width >> this.downsample, source.height >> this.downsample, 0, source.format);
        renderTexture.filterMode = FilterMode.Bilinear;
      }
      int num = (int) this.quality * 2;
      Graphics.Blit((Texture) source, renderTexture, this.tiltShiftMaterial, this.mode != TiltShiftHdr.TiltShiftMode.TiltShiftMode ? num + 1 : num);
      if (this.downsample != 0)
      {
        this.tiltShiftMaterial.SetTexture("_Blurred", (Texture) renderTexture);
        Graphics.Blit((Texture) source, destination, this.tiltShiftMaterial, 6);
      }
      if (!((UnityEngine.Object) renderTexture != (UnityEngine.Object) destination))
        return;
      RenderTexture.ReleaseTemporary(renderTexture);
    }
  }

  public override void Main()
  {
  }

  [Serializable]
  public enum TiltShiftMode
  {
    TiltShiftMode,
    IrisMode,
  }

  [Serializable]
  public enum TiltShiftQuality
  {
    Preview,
    Normal,
    High,
  }
}
