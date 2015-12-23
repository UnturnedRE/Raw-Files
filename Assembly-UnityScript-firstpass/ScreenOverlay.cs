// Decompiled with JetBrains decompiler
// Type: ScreenOverlay
// Assembly: Assembly-UnityScript-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ACB0EA2C-DE44-433E-98CB-A1A17DC32CFD
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-UnityScript-firstpass.dll

using System;
using UnityEngine;

[RequireComponent(typeof (Camera))]
[AddComponentMenu("Image Effects/Other/Screen Overlay")]
[ExecuteInEditMode]
[Serializable]
public class ScreenOverlay : PostEffectsBase
{
  public ScreenOverlay.OverlayBlendMode blendMode;
  public float intensity;
  public Camera cam;
  public RenderTexture tex;
  public Shader overlayShader;
  private Material overlayMaterial;

  public ScreenOverlay()
  {
    this.blendMode = ScreenOverlay.OverlayBlendMode.Overlay;
    this.intensity = 1f;
  }

  public override bool CheckResources()
  {
    this.CheckSupport(false);
    this.overlayMaterial = this.CheckShaderAndCreateMaterial(this.overlayShader, this.overlayMaterial);
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
      if ((UnityEngine.Object) this.tex == (UnityEngine.Object) null)
        this.tex = RenderTexture.GetTemporary(source.width, source.height, 0);
      if ((UnityEngine.Object) this.cam == (UnityEngine.Object) null)
        return;
      this.cam.targetTexture = this.tex;
      this.cam.Render();
      this.overlayMaterial.SetVector("_UV_Transform", new Vector4(1f, 0.0f, 0.0f, 1f));
      this.overlayMaterial.SetFloat("_Intensity", this.intensity);
      this.overlayMaterial.SetTexture("_Overlay", (Texture) this.tex);
      Graphics.Blit((Texture) source, destination, this.overlayMaterial, (int) this.blendMode);
    }
  }

  public override void Main()
  {
  }

  [Serializable]
  public enum OverlayBlendMode
  {
    Additive,
    ScreenBlend,
    Multiply,
    Overlay,
    AlphaBlend,
  }
}
