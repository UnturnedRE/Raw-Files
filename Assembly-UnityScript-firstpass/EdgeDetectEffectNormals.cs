// Decompiled with JetBrains decompiler
// Type: EdgeDetectEffectNormals
// Assembly: Assembly-UnityScript-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ACB0EA2C-DE44-433E-98CB-A1A17DC32CFD
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-UnityScript-firstpass.dll

using System;
using UnityEngine;

[RequireComponent(typeof (Camera))]
[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Edge Detection/Edge Detection")]
[Serializable]
public class EdgeDetectEffectNormals : PostEffectsBase
{
  public EdgeDetectMode mode;
  public float sensitivityDepth;
  public float sensitivityNormals;
  public float lumThreshhold;
  public float edgeExp;
  public float sampleDist;
  public float edgesOnly;
  public Color edgesOnlyBgColor;
  public Shader edgeDetectShader;
  private Material edgeDetectMaterial;
  private EdgeDetectMode oldMode;

  public EdgeDetectEffectNormals()
  {
    this.mode = EdgeDetectMode.SobelDepthThin;
    this.sensitivityDepth = 1f;
    this.sensitivityNormals = 1f;
    this.lumThreshhold = 0.2f;
    this.edgeExp = 1f;
    this.sampleDist = 1f;
    this.edgesOnlyBgColor = Color.white;
    this.oldMode = EdgeDetectMode.SobelDepthThin;
  }

  public override bool CheckResources()
  {
    this.CheckSupport(true);
    this.edgeDetectMaterial = this.CheckShaderAndCreateMaterial(this.edgeDetectShader, this.edgeDetectMaterial);
    if (this.mode != this.oldMode)
      this.SetCameraFlag();
    this.oldMode = this.mode;
    if (!this.isSupported)
      this.ReportAutoDisable();
    return this.isSupported;
  }

  public override void Start()
  {
    this.oldMode = this.mode;
  }

  public virtual void SetCameraFlag()
  {
    if (this.mode == EdgeDetectMode.SobelDepth || this.mode == EdgeDetectMode.SobelDepthThin)
    {
      this.GetComponent<Camera>().depthTextureMode = this.GetComponent<Camera>().depthTextureMode | DepthTextureMode.Depth;
    }
    else
    {
      if (this.mode != EdgeDetectMode.TriangleDepthNormals && this.mode != EdgeDetectMode.RobertsCrossDepthNormals)
        return;
      this.GetComponent<Camera>().depthTextureMode = this.GetComponent<Camera>().depthTextureMode | DepthTextureMode.DepthNormals;
    }
  }

  public override void OnEnable()
  {
    this.SetCameraFlag();
  }

  [ImageEffectOpaque]
  public virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if (!this.CheckResources())
    {
      Graphics.Blit((Texture) source, destination);
    }
    else
    {
      Vector2 vector2 = new Vector2(this.sensitivityDepth, this.sensitivityNormals);
      this.edgeDetectMaterial.SetVector("_Sensitivity", new Vector4(vector2.x, vector2.y, 1f, vector2.y));
      this.edgeDetectMaterial.SetFloat("_BgFade", this.edgesOnly);
      this.edgeDetectMaterial.SetFloat("_SampleDistance", this.sampleDist);
      this.edgeDetectMaterial.SetVector("_BgColor", (Vector4) this.edgesOnlyBgColor);
      this.edgeDetectMaterial.SetFloat("_Exponent", this.edgeExp);
      this.edgeDetectMaterial.SetFloat("_Threshold", this.lumThreshhold);
      Graphics.Blit((Texture) source, destination, this.edgeDetectMaterial, (int) this.mode);
    }
  }

  public override void Main()
  {
  }
}
