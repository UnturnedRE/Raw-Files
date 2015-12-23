// Decompiled with JetBrains decompiler
// Type: CameraMotionBlur
// Assembly: Assembly-UnityScript-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ACB0EA2C-DE44-433E-98CB-A1A17DC32CFD
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-UnityScript-firstpass.dll

using Boo.Lang.Runtime;
using System;
using UnityEngine;

[AddComponentMenu("Image Effects/Camera/Camera Motion Blur")]
[ExecuteInEditMode]
[RequireComponent(typeof (Camera))]
[Serializable]
public class CameraMotionBlur : PostEffectsBase
{
  [NonSerialized]
  public static int MAX_RADIUS = 10;
  public CameraMotionBlur.MotionBlurFilter filterType;
  public bool preview;
  public Vector3 previewScale;
  public float movementScale;
  public float rotationScale;
  public float maxVelocity;
  public float minVelocity;
  public float velocityScale;
  public float softZDistance;
  public int velocityDownsample;
  public LayerMask excludeLayers;
  private GameObject tmpCam;
  public Shader shader;
  public Shader replacementClear;
  private Material motionBlurMaterial;
  public Texture2D noiseTexture;
  public float jitter;
  public bool showVelocity;
  public float showVelocityScale;
  private Matrix4x4 currentViewProjMat;
  private Matrix4x4 prevViewProjMat;
  private int prevFrameCount;
  private bool wasActive;
  private Vector3 prevFrameForward;
  private Vector3 prevFrameRight;
  private Vector3 prevFrameUp;
  private Vector3 prevFramePos;

  public CameraMotionBlur()
  {
    this.filterType = CameraMotionBlur.MotionBlurFilter.Reconstruction;
    this.previewScale = Vector3.one;
    this.rotationScale = 1f;
    this.maxVelocity = 8f;
    this.minVelocity = 0.1f;
    this.velocityScale = 0.375f;
    this.softZDistance = 0.005f;
    this.velocityDownsample = 1;
    this.jitter = 0.05f;
    this.showVelocityScale = 1f;
    this.prevFrameForward = Vector3.forward;
    this.prevFrameRight = Vector3.right;
    this.prevFrameUp = Vector3.up;
    this.prevFramePos = Vector3.zero;
  }

  private void CalculateViewProjection()
  {
    this.currentViewProjMat = GL.GetGPUProjectionMatrix(this.GetComponent<Camera>().projectionMatrix, true) * this.GetComponent<Camera>().worldToCameraMatrix;
  }

  public override void Start()
  {
    this.CheckResources();
    this.wasActive = this.gameObject.activeInHierarchy;
    this.CalculateViewProjection();
    this.Remember();
    this.wasActive = false;
  }

  public override void OnEnable()
  {
    this.GetComponent<Camera>().depthTextureMode = this.GetComponent<Camera>().depthTextureMode | DepthTextureMode.Depth;
  }

  public virtual void OnDisable()
  {
    if ((UnityEngine.Object) null != (UnityEngine.Object) this.motionBlurMaterial)
    {
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.motionBlurMaterial);
      this.motionBlurMaterial = (Material) null;
    }
    if (!((UnityEngine.Object) null != (UnityEngine.Object) this.tmpCam))
      return;
    UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.tmpCam);
    this.tmpCam = (GameObject) null;
  }

  public override bool CheckResources()
  {
    this.CheckSupport(true, true);
    this.motionBlurMaterial = this.CheckShaderAndCreateMaterial(this.shader, this.motionBlurMaterial);
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
      if (this.filterType == CameraMotionBlur.MotionBlurFilter.CameraMotion)
        this.StartFrame();
      RenderTextureFormat format = !SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RGHalf) ? RenderTextureFormat.ARGBHalf : RenderTextureFormat.RGHalf;
      RenderTexture temporary1 = RenderTexture.GetTemporary(this.divRoundUp(source.width, this.velocityDownsample), this.divRoundUp(source.height, this.velocityDownsample), 0, format);
      this.maxVelocity = Mathf.Max(2f, this.maxVelocity);
      float num1 = this.maxVelocity;
      int width;
      int height;
      float num2;
      if (this.filterType == CameraMotionBlur.MotionBlurFilter.Reconstruction || this.filterType == CameraMotionBlur.MotionBlurFilter.ReconstructionDisc)
      {
        this.maxVelocity = Mathf.Min(this.maxVelocity, (float) CameraMotionBlur.MAX_RADIUS);
        width = this.divRoundUp(temporary1.width, (int) this.maxVelocity);
        height = this.divRoundUp(temporary1.height, (int) this.maxVelocity);
        num2 = (float) (temporary1.width / width);
      }
      else
      {
        width = this.divRoundUp(temporary1.width, (int) this.maxVelocity);
        height = this.divRoundUp(temporary1.height, (int) this.maxVelocity);
        num2 = (float) (temporary1.width / width);
      }
      RenderTexture temporary2 = RenderTexture.GetTemporary(width, height, 0, format);
      RenderTexture temporary3 = RenderTexture.GetTemporary(width, height, 0, format);
      temporary1.filterMode = FilterMode.Point;
      temporary2.filterMode = FilterMode.Point;
      temporary3.filterMode = FilterMode.Point;
      if ((bool) ((UnityEngine.Object) this.noiseTexture))
        this.noiseTexture.filterMode = FilterMode.Point;
      source.wrapMode = TextureWrapMode.Clamp;
      temporary1.wrapMode = TextureWrapMode.Clamp;
      temporary3.wrapMode = TextureWrapMode.Clamp;
      temporary2.wrapMode = TextureWrapMode.Clamp;
      this.CalculateViewProjection();
      if (this.gameObject.activeInHierarchy && !this.wasActive)
        this.Remember();
      this.wasActive = this.gameObject.activeInHierarchy;
      Matrix4x4 matrix = Matrix4x4.Inverse(this.currentViewProjMat);
      this.motionBlurMaterial.SetMatrix("_InvViewProj", matrix);
      this.motionBlurMaterial.SetMatrix("_PrevViewProj", this.prevViewProjMat);
      this.motionBlurMaterial.SetMatrix("_ToPrevViewProjCombined", this.prevViewProjMat * matrix);
      this.motionBlurMaterial.SetFloat("_MaxVelocity", num2);
      this.motionBlurMaterial.SetFloat("_MaxRadiusOrKInPaper", num2);
      this.motionBlurMaterial.SetFloat("_MinVelocity", this.minVelocity);
      this.motionBlurMaterial.SetFloat("_VelocityScale", this.velocityScale);
      this.motionBlurMaterial.SetFloat("_Jitter", this.jitter);
      this.motionBlurMaterial.SetTexture("_NoiseTex", (Texture) this.noiseTexture);
      this.motionBlurMaterial.SetTexture("_VelTex", (Texture) temporary1);
      this.motionBlurMaterial.SetTexture("_NeighbourMaxTex", (Texture) temporary3);
      this.motionBlurMaterial.SetTexture("_TileTexDebug", (Texture) temporary2);
      if (this.preview)
      {
        Matrix4x4 worldToCameraMatrix = this.GetComponent<Camera>().worldToCameraMatrix;
        Matrix4x4 identity = Matrix4x4.identity;
        identity.SetTRS(this.previewScale * 0.3333f, Quaternion.identity, Vector3.one);
        this.prevViewProjMat = GL.GetGPUProjectionMatrix(this.GetComponent<Camera>().projectionMatrix, true) * identity * worldToCameraMatrix;
        this.motionBlurMaterial.SetMatrix("_PrevViewProj", this.prevViewProjMat);
        this.motionBlurMaterial.SetMatrix("_ToPrevViewProjCombined", this.prevViewProjMat * matrix);
      }
      if (this.filterType == CameraMotionBlur.MotionBlurFilter.CameraMotion)
      {
        Vector4 zero = Vector4.zero;
        float num3 = Vector3.Dot(this.transform.up, Vector3.up);
        Vector3 rhs = this.prevFramePos - this.transform.position;
        float magnitude = rhs.magnitude;
        float num4 = (float) ((double) Vector3.Angle(this.transform.up, this.prevFrameUp) / (double) this.GetComponent<Camera>().fieldOfView * ((double) source.width * 0.75));
        zero.x = this.rotationScale * num4;
        float num5 = (float) ((double) Vector3.Angle(this.transform.forward, this.prevFrameForward) / (double) this.GetComponent<Camera>().fieldOfView * ((double) source.width * 0.75));
        zero.y = this.rotationScale * num3 * num5;
        float num6 = (float) ((double) Vector3.Angle(this.transform.forward, this.prevFrameForward) / (double) this.GetComponent<Camera>().fieldOfView * ((double) source.width * 0.75));
        zero.z = this.rotationScale * (1f - num3) * num6;
        if ((double) magnitude > (double) Mathf.Epsilon && (double) this.movementScale > (double) Mathf.Epsilon)
        {
          zero.w = (float) ((double) this.movementScale * (double) Vector3.Dot(this.transform.forward, rhs) * ((double) source.width * 0.5));
          zero.x = zero.x + (float) ((double) this.movementScale * (double) Vector3.Dot(this.transform.up, rhs) * ((double) source.width * 0.5));
          zero.y = zero.y + (float) ((double) this.movementScale * (double) Vector3.Dot(this.transform.right, rhs) * ((double) source.width * 0.5));
        }
        if (this.preview)
          this.motionBlurMaterial.SetVector("_BlurDirectionPacked", new Vector4(this.previewScale.y, this.previewScale.x, 0.0f, this.previewScale.z) * 0.5f * this.GetComponent<Camera>().fieldOfView);
        else
          this.motionBlurMaterial.SetVector("_BlurDirectionPacked", zero);
      }
      else
      {
        Graphics.Blit((Texture) source, temporary1, this.motionBlurMaterial, 0);
        Camera camera = (Camera) null;
        if (this.excludeLayers.value != 0)
          camera = this.GetTmpCam();
        if ((bool) ((UnityEngine.Object) camera) && this.excludeLayers.value != 0 && ((bool) ((UnityEngine.Object) this.replacementClear) && this.replacementClear.isSupported))
        {
          camera.targetTexture = temporary1;
          camera.cullingMask = (int) this.excludeLayers;
          camera.RenderWithShader(this.replacementClear, string.Empty);
        }
      }
      if (!this.preview && Time.frameCount != this.prevFrameCount)
      {
        this.prevFrameCount = Time.frameCount;
        this.Remember();
      }
      source.filterMode = FilterMode.Bilinear;
      if (this.showVelocity)
      {
        this.motionBlurMaterial.SetFloat("_DisplayVelocityScale", this.showVelocityScale);
        Graphics.Blit((Texture) temporary1, destination, this.motionBlurMaterial, 1);
      }
      else if (this.filterType == CameraMotionBlur.MotionBlurFilter.Reconstruction)
      {
        this.motionBlurMaterial.SetFloat("_SoftZDistance", Mathf.Max(0.00025f, this.softZDistance));
        Graphics.Blit((Texture) temporary1, temporary2, this.motionBlurMaterial, 2);
        Graphics.Blit((Texture) temporary2, temporary3, this.motionBlurMaterial, 3);
        Graphics.Blit((Texture) source, destination, this.motionBlurMaterial, 4);
      }
      else if (this.filterType == CameraMotionBlur.MotionBlurFilter.CameraMotion)
        Graphics.Blit((Texture) source, destination, this.motionBlurMaterial, 6);
      else if (this.filterType == CameraMotionBlur.MotionBlurFilter.ReconstructionDisc)
      {
        this.motionBlurMaterial.SetFloat("_SoftZDistance", Mathf.Max(0.00025f, this.softZDistance));
        Graphics.Blit((Texture) temporary1, temporary2, this.motionBlurMaterial, 2);
        Graphics.Blit((Texture) temporary2, temporary3, this.motionBlurMaterial, 3);
        Graphics.Blit((Texture) source, destination, this.motionBlurMaterial, 7);
      }
      else
        Graphics.Blit((Texture) source, destination, this.motionBlurMaterial, 5);
      RenderTexture.ReleaseTemporary(temporary1);
      RenderTexture.ReleaseTemporary(temporary2);
      RenderTexture.ReleaseTemporary(temporary3);
    }
  }

  public virtual void Remember()
  {
    this.prevViewProjMat = this.currentViewProjMat;
    this.prevFrameForward = this.transform.forward;
    this.prevFrameRight = this.transform.right;
    this.prevFrameUp = this.transform.up;
    this.prevFramePos = this.transform.position;
  }

  public virtual Camera GetTmpCam()
  {
    if ((UnityEngine.Object) this.tmpCam == (UnityEngine.Object) null)
    {
      string name = RuntimeServices.op_Addition(RuntimeServices.op_Addition("_", this.GetComponent<Camera>().name), "_MotionBlurTmpCam");
      GameObject gameObject = GameObject.Find(name);
      if ((UnityEngine.Object) null == (UnityEngine.Object) gameObject)
        this.tmpCam = new GameObject(name, new System.Type[1]
        {
          typeof (Camera)
        });
      else
        this.tmpCam = gameObject;
    }
    this.tmpCam.hideFlags = HideFlags.DontSave;
    this.tmpCam.transform.position = this.GetComponent<Camera>().transform.position;
    this.tmpCam.transform.rotation = this.GetComponent<Camera>().transform.rotation;
    this.tmpCam.transform.localScale = this.GetComponent<Camera>().transform.localScale;
    this.tmpCam.GetComponent<Camera>().CopyFrom(this.GetComponent<Camera>());
    this.tmpCam.GetComponent<Camera>().enabled = false;
    this.tmpCam.GetComponent<Camera>().depthTextureMode = DepthTextureMode.None;
    this.tmpCam.GetComponent<Camera>().clearFlags = CameraClearFlags.Nothing;
    return this.tmpCam.GetComponent<Camera>();
  }

  public virtual void StartFrame()
  {
    this.prevFramePos = Vector3.Slerp(this.prevFramePos, this.transform.position, 0.75f);
  }

  public virtual int divRoundUp(int x, int d)
  {
    return (x + d - 1) / d;
  }

  public override void Main()
  {
  }

  [Serializable]
  public enum MotionBlurFilter
  {
    CameraMotion = 0,
    LocalBlur = 1,
    Reconstruction = 2,
    ReconstructionDisc = 4,
  }
}
