// Decompiled with JetBrains decompiler
// Type: GlobalFog
// Assembly: Assembly-UnityScript-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ACB0EA2C-DE44-433E-98CB-A1A17DC32CFD
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-UnityScript-firstpass.dll

using System;
using UnityEngine;

[RequireComponent(typeof (Camera))]
[AddComponentMenu("Image Effects/Rendering/Global Fog")]
[ExecuteInEditMode]
[Serializable]
public class GlobalFog : PostEffectsBase
{
  public GlobalFog.FogMode fogMode;
  private float CAMERA_NEAR;
  private float CAMERA_FAR;
  private float CAMERA_FOV;
  private float CAMERA_ASPECT_RATIO;
  public float startDistance;
  public float globalDensity;
  public float heightScale;
  public float height;
  public Color globalFogColor;
  public Shader fogShader;
  private Material fogMaterial;

  public GlobalFog()
  {
    this.fogMode = GlobalFog.FogMode.AbsoluteYAndDistance;
    this.CAMERA_NEAR = 0.5f;
    this.CAMERA_FAR = 50f;
    this.CAMERA_FOV = 60f;
    this.CAMERA_ASPECT_RATIO = 1.333333f;
    this.startDistance = 200f;
    this.globalDensity = 1f;
    this.heightScale = 100f;
    this.globalFogColor = Color.grey;
  }

  public override bool CheckResources()
  {
    this.CheckSupport(true);
    this.fogMaterial = this.CheckShaderAndCreateMaterial(this.fogShader, this.fogMaterial);
    if (!this.isSupported)
      this.ReportAutoDisable();
    return this.isSupported;
  }

  public virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    // ISSUE: unable to decompile the method.
  }

  public static void CustomGraphicsBlit(RenderTexture source, RenderTexture dest, Material fxMaterial, int passNr)
  {
    RenderTexture.active = dest;
    fxMaterial.SetTexture("_MainTex", (Texture) source);
    GL.PushMatrix();
    GL.LoadOrtho();
    fxMaterial.SetPass(passNr);
    GL.Begin(7);
    GL.MultiTexCoord2(0, 0.0f, 0.0f);
    GL.Vertex3(0.0f, 0.0f, 3f);
    GL.MultiTexCoord2(0, 1f, 0.0f);
    GL.Vertex3(1f, 0.0f, 2f);
    GL.MultiTexCoord2(0, 1f, 1f);
    GL.Vertex3(1f, 1f, 1f);
    GL.MultiTexCoord2(0, 0.0f, 1f);
    GL.Vertex3(0.0f, 1f, 0.0f);
    GL.End();
    GL.PopMatrix();
  }

  public override void Main()
  {
  }

  [Serializable]
  public enum FogMode
  {
    AbsoluteYAndDistance,
    AbsoluteY,
    Distance,
    RelativeYAndDistance,
  }
}
