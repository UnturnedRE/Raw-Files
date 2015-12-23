// Decompiled with JetBrains decompiler
// Type: HighlightingSystem.HighlightingBase
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace HighlightingSystem
{
  [RequireComponent(typeof (Camera))]
  public class HighlightingBase : MonoBehaviour
  {
    protected static readonly Color colorClear = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    protected static readonly string renderBufferName = "HighlightingSystem";
    protected static readonly Matrix4x4 identityMatrix = Matrix4x4.identity;
    protected static int graphicsDeviceVersion = 1;
    protected static readonly string[] shaderPaths = new string[3]
    {
      "Hidden/Highlighted/Blur",
      "Hidden/Highlighted/Cut",
      "Hidden/Highlighted/Composite"
    };
    protected static bool initialized = false;
    protected bool isDirty = true;
    protected int cachedWidth = -1;
    protected int cachedHeight = -1;
    protected int cachedAA = -1;
    [SerializeField]
    [FormerlySerializedAs("downsampleFactor")]
    protected int _downsampleFactor = 4;
    [FormerlySerializedAs("iterations")]
    [SerializeField]
    protected int _iterations = 2;
    [SerializeField]
    [FormerlySerializedAs("blurMinSpread")]
    protected float _blurMinSpread = 0.65f;
    [SerializeField]
    [FormerlySerializedAs("blurSpread")]
    protected float _blurSpread = 0.25f;
    [SerializeField]
    protected float _blurIntensity = 0.3f;
    protected bool isDepthAvailable = true;
    protected const CameraEvent queue = CameraEvent.BeforeImageEffectsOpaque;
    protected const int OGL = 0;
    protected const int D3D9 = 1;
    protected const int D3D11 = 2;
    protected const int BLUR = 0;
    protected const int CUT = 1;
    protected const int COMP = 2;
    protected static RenderTargetIdentifier cameraTargetID;
    protected static Mesh quad;
    public float offsetFactor;
    public float offsetUnits;
    protected CommandBuffer renderBuffer;
    protected RenderTargetIdentifier highlightingBufferID;
    protected RenderTexture highlightingBuffer;
    protected Camera cam;
    protected bool isSupported;
    protected static Shader[] shaders;
    protected static Material[] materials;
    protected static Material cutMaterial;
    protected static Material compMaterial;
    protected Material blurMaterial;

    public int downsampleFactor
    {
      get
      {
        return this._downsampleFactor;
      }
      set
      {
        if (this._downsampleFactor == value)
          return;
        if (value != 0 && (value & value - 1) == 0)
        {
          this._downsampleFactor = value;
          this.isDirty = true;
        }
        else
          Debug.LogWarning((object) "HighlightingSystem : Prevented attempt to set incorrect downsample factor value.");
      }
    }

    public int iterations
    {
      get
      {
        return this._iterations;
      }
      set
      {
        if (this._iterations == value)
          return;
        this._iterations = value;
        this.isDirty = true;
      }
    }

    public float blurMinSpread
    {
      get
      {
        return this._blurMinSpread;
      }
      set
      {
        if ((double) this._blurMinSpread == (double) value)
          return;
        this._blurMinSpread = value;
        this.isDirty = true;
      }
    }

    public float blurSpread
    {
      get
      {
        return this._blurSpread;
      }
      set
      {
        if ((double) this._blurSpread == (double) value)
          return;
        this._blurSpread = value;
        this.isDirty = true;
      }
    }

    public float blurIntensity
    {
      get
      {
        return this._blurIntensity;
      }
      set
      {
        if ((double) this._blurIntensity == (double) value)
          return;
        this._blurIntensity = value;
        if (!Application.isPlaying)
          return;
        this.blurMaterial.SetFloat(ShaderPropertyID._Intensity, this._blurIntensity);
      }
    }

    protected virtual void OnEnable()
    {
      if (!this.CheckInstance())
        return;
      HighlightingBase.Initialize();
      this.isSupported = this.CheckSupported();
      if (!this.isSupported)
      {
        this.enabled = false;
        Debug.LogError((object) "HighlightingSystem : Highlighting System has been disabled due to unsupported Unity features on the current platform!");
      }
      else
      {
        this.blurMaterial = new Material(HighlightingBase.materials[0]);
        this.blurMaterial.SetFloat(ShaderPropertyID._Intensity, this._blurIntensity);
        this.renderBuffer = new CommandBuffer();
        this.renderBuffer.name = HighlightingBase.renderBufferName;
        this.cam = this.GetComponent<Camera>();
        this.UpdateHighlightingBuffer();
        this.isDirty = true;
        this.cam.AddCommandBuffer(CameraEvent.BeforeImageEffectsOpaque, this.renderBuffer);
      }
    }

    protected virtual void OnDisable()
    {
      if (this.renderBuffer != null)
      {
        this.cam.RemoveCommandBuffer(CameraEvent.BeforeImageEffectsOpaque, this.renderBuffer);
        this.renderBuffer = (CommandBuffer) null;
      }
      if (!((Object) this.highlightingBuffer != (Object) null) || !this.highlightingBuffer.IsCreated())
        return;
      this.highlightingBuffer.Release();
      this.highlightingBuffer = (RenderTexture) null;
    }

    protected virtual void OnPreRender()
    {
      this.UpdateHighlightingBuffer();
      int aa = this.GetAA();
      bool flag = aa == 1;
      if (aa > 1 && (this.cam.actualRenderingPath == RenderingPath.Forward || this.cam.actualRenderingPath == RenderingPath.VertexLit))
        flag = false;
      if (this.isDepthAvailable != flag)
      {
        this.isDepthAvailable = flag;
        Highlighter.SetZWrite(!this.isDepthAvailable ? 1f : 0.0f);
        if (this.isDepthAvailable)
          Debug.LogWarning((object) "HighlightingSystem : Framebuffer depth data is available back again and will be used to occlude highlighting. Highlighting occluders disabled.");
        else
          Debug.LogWarning((object) "HighlightingSystem : Framebuffer depth data is not available and can't be used to occlude highlighting. Highlighting occluders enabled.");
        this.isDirty = true;
      }
      Highlighter.SetOffsetFactor(this.offsetFactor);
      Highlighter.SetOffsetUnits(this.offsetUnits);
      this.isDirty |= HighlighterManager.isDirty;
      this.isDirty |= this.HighlightersChanged();
      if (!this.isDirty)
        return;
      this.RebuildCommandBuffer();
      this.isDirty = false;
    }

    protected virtual void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
      Graphics.Blit((Texture) src, dst, HighlightingBase.compMaterial);
    }

    protected static void Initialize()
    {
      if (HighlightingBase.initialized)
        return;
      string str = SystemInfo.graphicsDeviceVersion.ToLower();
      HighlightingBase.graphicsDeviceVersion = str.Contains("direct3d") || str.Contains("directx") ? (str.Contains("direct3d 11") || str.Contains("directx 11") ? 2 : 1) : 0;
      ShaderPropertyID.Initialize();
      int length = HighlightingBase.shaderPaths.Length;
      HighlightingBase.shaders = new Shader[length];
      HighlightingBase.materials = new Material[length];
      for (int index = 0; index < length; ++index)
      {
        Shader shader = Shader.Find(HighlightingBase.shaderPaths[index]);
        HighlightingBase.shaders[index] = shader;
        Material material = new Material(shader);
        HighlightingBase.materials[index] = material;
      }
      HighlightingBase.cutMaterial = HighlightingBase.materials[1];
      HighlightingBase.compMaterial = HighlightingBase.materials[2];
      HighlightingBase.cameraTargetID = new RenderTargetIdentifier(BuiltinRenderTextureType.CameraTarget);
      HighlightingBase.CreateQuad();
      HighlightingBase.initialized = true;
    }

    protected static void CreateQuad()
    {
      if ((Object) HighlightingBase.quad == (Object) null)
        HighlightingBase.quad = new Mesh();
      else
        HighlightingBase.quad.Clear();
      float y1 = 1f;
      float y2 = -1f;
      if (HighlightingBase.graphicsDeviceVersion == 0)
      {
        y1 = -1f;
        y2 = 1f;
      }
      HighlightingBase.quad.vertices = new Vector3[4]
      {
        new Vector3(-1f, y1, 0.0f),
        new Vector3(-1f, y2, 0.0f),
        new Vector3(1f, y2, 0.0f),
        new Vector3(1f, y1, 0.0f)
      };
      HighlightingBase.quad.uv = new Vector2[4]
      {
        new Vector2(0.0f, 0.0f),
        new Vector2(0.0f, 1f),
        new Vector2(1f, 1f),
        new Vector2(1f, 0.0f)
      };
      HighlightingBase.quad.colors = new Color[4]
      {
        HighlightingBase.colorClear,
        HighlightingBase.colorClear,
        HighlightingBase.colorClear,
        HighlightingBase.colorClear
      };
      HighlightingBase.quad.triangles = new int[6]
      {
        0,
        1,
        2,
        2,
        3,
        0
      };
    }

    protected virtual int GetAA()
    {
      int num = QualitySettings.antiAliasing;
      if (num == 0)
        num = 1;
      if (this.cam.actualRenderingPath == RenderingPath.DeferredLighting || this.cam.actualRenderingPath == RenderingPath.DeferredShading)
        num = 1;
      return num;
    }

    protected virtual void UpdateHighlightingBuffer()
    {
      int aa = this.GetAA();
      if (this.cam.pixelWidth == this.cachedWidth && this.cam.pixelHeight == this.cachedHeight && aa == this.cachedAA)
        return;
      this.cachedWidth = this.cam.pixelWidth;
      this.cachedHeight = this.cam.pixelHeight;
      this.cachedAA = aa;
      if ((Object) this.highlightingBuffer != (Object) null && this.highlightingBuffer.IsCreated())
        this.highlightingBuffer.Release();
      this.highlightingBuffer = new RenderTexture(this.cachedWidth, this.cachedHeight, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
      this.highlightingBuffer.antiAliasing = this.cachedAA;
      this.highlightingBuffer.filterMode = FilterMode.Point;
      this.highlightingBuffer.useMipMap = false;
      this.highlightingBuffer.wrapMode = TextureWrapMode.Clamp;
      if (!this.highlightingBuffer.Create())
        Debug.LogError((object) "HighlightingSystem : UpdateHighlightingBuffer() : Failed to create highlightingBuffer RenderTexture!");
      this.highlightingBufferID = new RenderTargetIdentifier(this.highlightingBuffer);
      Shader.SetGlobalTexture(ShaderPropertyID._HighlightingBuffer, (Texture) this.highlightingBuffer);
      Shader.SetGlobalVector(ShaderPropertyID._HighlightingBufferTexelSize, new Vector4((HighlightingBase.graphicsDeviceVersion != 0 ? -1f : 1f) / (float) this.highlightingBuffer.width, 1f / (float) this.highlightingBuffer.height, 0.0f, 0.0f));
      this.isDirty = true;
    }

    public virtual bool CheckInstance()
    {
      HighlightingBase[] components = this.GetComponents<HighlightingBase>();
      if (components.Length <= 1 || !((Object) components[0] != (Object) this))
        return true;
      this.enabled = false;
      Debug.LogWarning((object) string.Format("HighlightingSystem : Only single instance of the HighlightingRenderer component is allowed on a single Gameobject! {0} has been disabled on GameObject with name '{1}'.", (object) this.GetType().ToString(), (object) this.name));
      return false;
    }

    protected virtual bool CheckSupported()
    {
      if (!SystemInfo.supportsImageEffects)
      {
        Debug.LogError((object) "HighlightingSystem : Image effects is not supported on this platform!");
        return false;
      }
      if (!SystemInfo.supportsRenderTextures)
      {
        Debug.LogError((object) "HighlightingSystem : RenderTextures is not supported on this platform!");
        return false;
      }
      if (!SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGB32))
      {
        Debug.LogError((object) "HighlightingSystem : RenderTextureFormat.ARGB32 is not supported on this platform!");
        return false;
      }
      if (SystemInfo.supportsStencil < 1)
      {
        Debug.LogError((object) "HighlightingSystem : Stencil buffer is not supported on this platform!");
        return false;
      }
      if (!Highlighter.opaqueShader.isSupported)
      {
        Debug.LogError((object) "HighlightingSystem : HighlightingOpaque shader is not supported on this platform!");
        return false;
      }
      if (!Highlighter.transparentShader.isSupported)
      {
        Debug.LogError((object) "HighlightingSystem : HighlightingTransparent shader is not supported on this platform!");
        return false;
      }
      for (int index = 0; index < HighlightingBase.shaders.Length; ++index)
      {
        Shader shader = HighlightingBase.shaders[index];
        if (!shader.isSupported)
        {
          Debug.LogError((object) ("HighlightingSystem : Shader '" + shader.name + "' is not supported on this platform!"));
          return false;
        }
      }
      return true;
    }

    protected virtual bool HighlightersChanged()
    {
      bool flag = false;
      HashSet<Highlighter>.Enumerator enumerator = HighlighterManager.GetEnumerator();
      while (enumerator.MoveNext())
      {
        Highlighter current = enumerator.Current;
        flag |= current.UpdateHighlighting(this.isDepthAvailable);
      }
      return flag;
    }

    protected virtual void RebuildCommandBuffer()
    {
      this.renderBuffer.Clear();
      RenderTargetIdentifier depth = !this.isDepthAvailable ? this.highlightingBufferID : HighlightingBase.cameraTargetID;
      this.renderBuffer.SetRenderTarget(this.highlightingBufferID, depth);
      this.renderBuffer.ClearRenderTarget(!this.isDepthAvailable, true, HighlightingBase.colorClear);
      this.FillBuffer(this.renderBuffer, 0);
      this.FillBuffer(this.renderBuffer, 1);
      this.FillBuffer(this.renderBuffer, 2);
      RenderTargetIdentifier targetIdentifier1 = new RenderTargetIdentifier(ShaderPropertyID._HighlightingBlur1);
      RenderTargetIdentifier targetIdentifier2 = new RenderTargetIdentifier(ShaderPropertyID._HighlightingBlur2);
      int width = this.highlightingBuffer.width / this._downsampleFactor;
      int height = this.highlightingBuffer.height / this._downsampleFactor;
      this.renderBuffer.GetTemporaryRT(ShaderPropertyID._HighlightingBlur1, width, height, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
      this.renderBuffer.GetTemporaryRT(ShaderPropertyID._HighlightingBlur2, width, height, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
      this.renderBuffer.Blit(this.highlightingBufferID, targetIdentifier1);
      bool flag = true;
      for (int index = 0; index < this._iterations; ++index)
      {
        this.renderBuffer.SetGlobalFloat(ShaderPropertyID._HighlightingBlurOffset, this._blurMinSpread + this._blurSpread * (float) index);
        if (flag)
          this.renderBuffer.Blit(targetIdentifier1, targetIdentifier2, this.blurMaterial);
        else
          this.renderBuffer.Blit(targetIdentifier2, targetIdentifier1, this.blurMaterial);
        flag = !flag;
      }
      this.renderBuffer.SetGlobalTexture(ShaderPropertyID._HighlightingBlurred, !flag ? targetIdentifier2 : targetIdentifier1);
      this.renderBuffer.SetRenderTarget(this.highlightingBufferID, depth);
      this.renderBuffer.DrawMesh(HighlightingBase.quad, HighlightingBase.identityMatrix, HighlightingBase.cutMaterial);
      this.renderBuffer.ReleaseTemporaryRT(ShaderPropertyID._HighlightingBlur1);
      this.renderBuffer.ReleaseTemporaryRT(ShaderPropertyID._HighlightingBlur2);
    }

    protected virtual void FillBuffer(CommandBuffer buffer, int renderQueue)
    {
      HashSet<Highlighter>.Enumerator enumerator = HighlighterManager.GetEnumerator();
      while (enumerator.MoveNext())
        enumerator.Current.FillBuffer(this.renderBuffer, renderQueue);
    }
  }
}
