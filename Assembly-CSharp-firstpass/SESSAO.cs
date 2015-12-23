// Decompiled with JetBrains decompiler
// Type: SESSAO
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

[RequireComponent(typeof (Camera))]
[AddComponentMenu("Image Effects/Sonic Ether/SESSAO")]
[ExecuteInEditMode]
public class SESSAO : MonoBehaviour
{
  [Range(0.02f, 5f)]
  public float radius = 1f;
  [Range(-0.2f, 0.5f)]
  public float bias = 0.1f;
  [Range(0.1f, 3f)]
  public float bilateralDepthTolerance = 0.2f;
  [Range(1f, 5f)]
  public float zThickness = 2.35f;
  [Range(0.5f, 5f)]
  public float occlusionIntensity = 1.3f;
  [Range(1f, 6f)]
  public float sampleDistributionCurve = 1.15f;
  [Range(0.0f, 1f)]
  public float colorBleedAmount = 1f;
  public float drawDistance = 500f;
  public float drawDistanceFadeSize = 1f;
  public bool reduceSelfBleeding = true;
  private Material material;
  public bool visualizeSSAO;
  private Texture2D ditherTexture;
  private Texture2D ditherTextureSmall;
  private bool skipThisFrame;
  [Range(0.1f, 3f)]
  public float brightnessThreshold;
  public bool useDownsampling;
  public bool halfSampling;
  public bool preserveDetails;
  [HideInInspector]
  public Camera attachedCamera;
  private object initChecker;

  private void CheckInit()
  {
    if (this.initChecker != null)
      return;
    this.Init();
  }

  private void Init()
  {
    this.skipThisFrame = false;
    Shader shader = Shader.Find("Hidden/SESSAO");
    if (!(bool) ((Object) shader))
    {
      this.skipThisFrame = true;
    }
    else
    {
      this.material = new Material(shader);
      this.attachedCamera = this.GetComponent<Camera>();
      this.attachedCamera.depthTextureMode |= DepthTextureMode.Depth;
      this.attachedCamera.depthTextureMode |= DepthTextureMode.DepthNormals;
      this.SetupDitherTexture();
      this.SetupDitherTextureSmall();
      this.initChecker = new object();
    }
  }

  private void Cleanup()
  {
    Object.DestroyImmediate((Object) this.material);
    this.initChecker = (object) null;
  }

  private void SetupDitherTextureSmall()
  {
    this.ditherTextureSmall = new Texture2D(3, 3, TextureFormat.Alpha8, false);
    this.ditherTextureSmall.filterMode = FilterMode.Point;
    float[] numArray = new float[9]
    {
      8f,
      1f,
      6f,
      3f,
      0.0f,
      4f,
      7f,
      2f,
      5f
    };
    for (int index = 0; index < 9; ++index)
    {
      Color color = new Color(0.0f, 0.0f, 0.0f, numArray[index] / 9f);
      this.ditherTextureSmall.SetPixel(index % 3, Mathf.FloorToInt((float) index / 3f), color);
    }
    this.ditherTextureSmall.Apply();
    this.ditherTextureSmall.hideFlags = HideFlags.HideAndDontSave;
  }

  private void SetupDitherTexture()
  {
    this.ditherTexture = new Texture2D(5, 5, TextureFormat.Alpha8, false);
    this.ditherTexture.filterMode = FilterMode.Point;
    float[] numArray = new float[25]
    {
      12f,
      1f,
      10f,
      3f,
      20f,
      5f,
      18f,
      7f,
      16f,
      9f,
      24f,
      2f,
      11f,
      6f,
      22f,
      15f,
      8f,
      0.0f,
      13f,
      19f,
      4f,
      21f,
      14f,
      23f,
      17f
    };
    for (int index = 0; index < 25; ++index)
    {
      Color color = new Color(0.0f, 0.0f, 0.0f, numArray[index] / 25f);
      this.ditherTexture.SetPixel(index % 5, Mathf.FloorToInt((float) index / 5f), color);
    }
    this.ditherTexture.Apply();
    this.ditherTexture.hideFlags = HideFlags.HideAndDontSave;
  }

  private void Start()
  {
    this.CheckInit();
  }

  private void OnEnable()
  {
    this.CheckInit();
  }

  private void OnDisable()
  {
    this.Cleanup();
  }

  private void Update()
  {
    this.drawDistance = Mathf.Max(0.0f, this.drawDistance);
    this.drawDistanceFadeSize = Mathf.Max(1.0 / 1000.0, this.drawDistanceFadeSize);
    this.bilateralDepthTolerance = Mathf.Max(1E-06f, this.bilateralDepthTolerance);
  }

  [ImageEffectOpaque]
  private void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    this.CheckInit();
    if (this.skipThisFrame)
    {
      Graphics.Blit((Texture) source, destination);
    }
    else
    {
      this.material.hideFlags = HideFlags.HideAndDontSave;
      this.material.SetTexture("_DitherTexture", !this.preserveDetails ? (Texture) this.ditherTexture : (Texture) this.ditherTextureSmall);
      this.material.SetInt("PreserveDetails", !this.preserveDetails ? 0 : 1);
      this.material.SetMatrix("ProjectionMatrixInverse", this.GetComponent<Camera>().projectionMatrix.inverse);
      RenderTexture temporary1 = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.ARGBHalf);
      RenderTexture temporary2 = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.ARGBHalf);
      RenderTexture temporary3 = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0, source.format);
      temporary3.wrapMode = TextureWrapMode.Clamp;
      temporary3.filterMode = FilterMode.Bilinear;
      Graphics.Blit((Texture) source, temporary3);
      this.material.SetTexture("_ColorDownsampled", (Texture) temporary3);
      RenderTexture renderTexture = (RenderTexture) null;
      this.material.SetFloat("Radius", this.radius);
      this.material.SetFloat("Bias", this.bias);
      this.material.SetFloat("DepthTolerance", this.bilateralDepthTolerance);
      this.material.SetFloat("ZThickness", this.zThickness);
      this.material.SetFloat("Intensity", this.occlusionIntensity);
      this.material.SetFloat("SampleDistributionCurve", this.sampleDistributionCurve);
      this.material.SetFloat("ColorBleedAmount", this.colorBleedAmount);
      this.material.SetFloat("DrawDistance", this.drawDistance);
      this.material.SetFloat("DrawDistanceFadeSize", this.drawDistanceFadeSize);
      this.material.SetFloat("SelfBleedReduction", !this.reduceSelfBleeding ? 0.0f : 1f);
      this.material.SetFloat("BrightnessThreshold", this.brightnessThreshold);
      this.material.SetInt("HalfSampling", !this.halfSampling ? 0 : 1);
      this.material.SetInt("Orthographic", !this.attachedCamera.orthographic ? 0 : 1);
      if (this.useDownsampling)
      {
        renderTexture = RenderTexture.GetTemporary(source.width / 2, source.height / 2, 0, RenderTextureFormat.ARGBHalf);
        renderTexture.filterMode = FilterMode.Bilinear;
        this.material.SetInt("Downsamp", 1);
        Graphics.Blit((Texture) source, renderTexture, this.material, (double) this.colorBleedAmount > 9.99999974737875E-05 ? 0 : 1);
      }
      else
      {
        this.material.SetInt("Downsamp", 0);
        Graphics.Blit((Texture) source, temporary1, this.material, (double) this.colorBleedAmount > 9.99999974737875E-05 ? 0 : 1);
      }
      RenderTexture.ReleaseTemporary(temporary3);
      this.material.SetFloat("BlurDepthTolerance", 0.1f);
      int pass = !this.attachedCamera.orthographic ? 2 : 6;
      if (this.attachedCamera.orthographic)
      {
        this.material.SetFloat("Near", this.attachedCamera.nearClipPlane);
        this.material.SetFloat("Far", this.attachedCamera.farClipPlane);
      }
      if (this.useDownsampling)
      {
        this.material.SetVector("Kernel", (Vector4) new Vector2(2f, 0.0f));
        Graphics.Blit((Texture) renderTexture, temporary2, this.material, pass);
        RenderTexture.ReleaseTemporary(renderTexture);
        this.material.SetVector("Kernel", (Vector4) new Vector2(0.0f, 2f));
        Graphics.Blit((Texture) temporary2, temporary1, this.material, pass);
        this.material.SetVector("Kernel", (Vector4) new Vector2(2f, 0.0f));
        Graphics.Blit((Texture) temporary1, temporary2, this.material, pass);
        this.material.SetVector("Kernel", (Vector4) new Vector2(0.0f, 2f));
        Graphics.Blit((Texture) temporary2, temporary1, this.material, pass);
      }
      else
      {
        this.material.SetVector("Kernel", (Vector4) new Vector2(1f, 0.0f));
        Graphics.Blit((Texture) temporary1, temporary2, this.material, pass);
        this.material.SetVector("Kernel", (Vector4) new Vector2(0.0f, 1f));
        Graphics.Blit((Texture) temporary2, temporary1, this.material, pass);
        this.material.SetVector("Kernel", (Vector4) new Vector2(1f, 0.0f));
        Graphics.Blit((Texture) temporary1, temporary2, this.material, pass);
        this.material.SetVector("Kernel", (Vector4) new Vector2(0.0f, 1f));
        Graphics.Blit((Texture) temporary2, temporary1, this.material, pass);
      }
      RenderTexture.ReleaseTemporary(temporary2);
      this.material.SetTexture("_SSAO", (Texture) temporary1);
      if (!this.visualizeSSAO)
        Graphics.Blit((Texture) source, destination, this.material, 3);
      else
        Graphics.Blit((Texture) source, destination, this.material, 5);
      RenderTexture.ReleaseTemporary(temporary1);
    }
  }
}
