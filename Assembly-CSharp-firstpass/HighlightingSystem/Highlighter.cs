// Decompiled with JetBrains decompiler
// Type: HighlightingSystem.Highlighter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace HighlightingSystem
{
  public class Highlighter : MonoBehaviour
  {
    private static float zWrite = -1f;
    private static float offsetFactor = float.NaN;
    private static float offsetUnits = float.NaN;
    private static float constantOnSpeed = 4.5f;
    private static float constantOffSpeed = 4f;
    private static float transparentCutoff = 0.5f;
    public static readonly List<System.Type> types = new List<System.Type>()
    {
      typeof (MeshRenderer),
      typeof (SkinnedMeshRenderer),
      typeof (SpriteRenderer),
      typeof (ParticleRenderer),
      typeof (ParticleSystemRenderer)
    };
    private readonly Color occluderColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    private int visibilityCheckFrame = -1;
    private bool renderersDirty = true;
    private float flashingFreq = 2f;
    private Color onceColor = Color.red;
    private Color flashingColorMin = new Color(0.0f, 1f, 1f, 0.0f);
    private Color flashingColorMax = new Color(0.0f, 1f, 1f, 1f);
    private Color constantColor = Color.yellow;
    private bool seeThrough = true;
    private int renderQueue = 1;
    private bool zTest = true;
    private bool stencilRef = true;
    public const int highlightingLayer = 7;
    private const float doublePI = 6.283185f;
    private const float zTestLessEqual = 4f;
    private const float zTestAlways = 8f;
    private const float cullOff = 0.0f;
    private Transform tr;
    private List<Highlighter.RendererCache> highlightableRenderers;
    private bool visibilityChanged;
    private bool visible;
    private Color currentColor;
    private bool transitionActive;
    private float transitionValue;
    private int _once;
    private bool flashing;
    private bool constantly;
    private bool occluder;
    private static Shader _opaqueShader;
    private static Shader _transparentShader;
    private Material _opaqueMaterial;

    public bool highlighted { get; private set; }

    private bool once
    {
      get
      {
        return this._once == Time.frameCount;
      }
      set
      {
        this._once = !value ? 0 : Time.frameCount;
      }
    }

    private float zTestFloat
    {
      get
      {
        return this.zTest ? 8f : 4f;
      }
    }

    private float stencilRefFloat
    {
      get
      {
        return this.stencilRef ? 1f : 0.0f;
      }
    }

    public static Shader opaqueShader
    {
      get
      {
        if ((UnityEngine.Object) Highlighter._opaqueShader == (UnityEngine.Object) null)
          Highlighter._opaqueShader = Shader.Find("Hidden/Highlighted/Opaque");
        return Highlighter._opaqueShader;
      }
    }

    public static Shader transparentShader
    {
      get
      {
        if ((UnityEngine.Object) Highlighter._transparentShader == (UnityEngine.Object) null)
          Highlighter._transparentShader = Shader.Find("Hidden/Highlighted/Transparent");
        return Highlighter._transparentShader;
      }
    }

    private Material opaqueMaterial
    {
      get
      {
        if ((UnityEngine.Object) this._opaqueMaterial == (UnityEngine.Object) null)
        {
          this._opaqueMaterial = new Material(Highlighter.opaqueShader);
          ShaderPropertyID.Initialize();
          this._opaqueMaterial.SetFloat(ShaderPropertyID._ZTest, this.zTestFloat);
          this._opaqueMaterial.SetFloat(ShaderPropertyID._StencilRef, this.stencilRefFloat);
        }
        return this._opaqueMaterial;
      }
    }

    public void ReinitMaterials()
    {
      this.renderersDirty = true;
    }

    public void OnParams(Color color)
    {
      this.onceColor = color;
    }

    public void On()
    {
      this.once = true;
    }

    public void On(Color color)
    {
      this.onceColor = color;
      this.On();
    }

    public void FlashingParams(Color color1, Color color2, float freq)
    {
      this.flashingColorMin = color1;
      this.flashingColorMax = color2;
      this.flashingFreq = freq;
    }

    public void FlashingOn()
    {
      this.flashing = true;
    }

    public void FlashingOn(Color color1, Color color2)
    {
      this.flashingColorMin = color1;
      this.flashingColorMax = color2;
      this.FlashingOn();
    }

    public void FlashingOn(Color color1, Color color2, float freq)
    {
      this.flashingFreq = freq;
      this.FlashingOn(color1, color2);
    }

    public void FlashingOn(float freq)
    {
      this.flashingFreq = freq;
      this.FlashingOn();
    }

    public void FlashingOff()
    {
      this.flashing = false;
    }

    public void FlashingSwitch()
    {
      this.flashing = !this.flashing;
    }

    public void ConstantParams(Color color)
    {
      this.constantColor = color;
    }

    public void ConstantOn()
    {
      this.constantly = true;
      this.transitionActive = true;
    }

    public void ConstantOn(Color color)
    {
      this.constantColor = color;
      this.ConstantOn();
    }

    public void ConstantOff()
    {
      this.constantly = false;
      this.transitionActive = true;
    }

    public void ConstantSwitch()
    {
      this.constantly = !this.constantly;
      this.transitionActive = true;
    }

    public void ConstantOnImmediate()
    {
      this.constantly = true;
      this.transitionValue = 1f;
      this.transitionActive = false;
    }

    public void ConstantOnImmediate(Color color)
    {
      this.constantColor = color;
      this.ConstantOnImmediate();
    }

    public void ConstantOffImmediate()
    {
      this.constantly = false;
      this.transitionValue = 0.0f;
      this.transitionActive = false;
    }

    public void ConstantSwitchImmediate()
    {
      this.constantly = !this.constantly;
      this.transitionValue = !this.constantly ? 0.0f : 1f;
      this.transitionActive = false;
    }

    public void SeeThroughOn()
    {
      this.seeThrough = true;
    }

    public void SeeThroughOff()
    {
      this.seeThrough = false;
    }

    public void SeeThroughSwitch()
    {
      this.seeThrough = !this.seeThrough;
    }

    public void OccluderOn()
    {
      this.occluder = true;
    }

    public void OccluderOff()
    {
      this.occluder = false;
    }

    public void OccluderSwitch()
    {
      this.occluder = !this.occluder;
    }

    public void Off()
    {
      this.once = false;
      this.flashing = false;
      this.constantly = false;
      this.transitionValue = 0.0f;
      this.transitionActive = false;
    }

    public void Die()
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) this);
    }

    private void Awake()
    {
      this.tr = this.GetComponent<Transform>();
      ShaderPropertyID.Initialize();
    }

    private void OnEnable()
    {
      if (!this.CheckInstance())
        return;
      HighlighterManager.Add(this);
    }

    private void OnDisable()
    {
      HighlighterManager.Remove(this);
      if (this.highlightableRenderers != null)
        this.highlightableRenderers.Clear();
      this.renderersDirty = true;
      this.highlighted = false;
      this.currentColor = Color.clear;
      this.transitionActive = false;
      this.transitionValue = 0.0f;
      this.once = false;
      this.flashing = false;
      this.constantly = false;
      this.occluder = false;
      this.seeThrough = false;
    }

    private void Update()
    {
      this.PerformTransition();
    }

    public bool UpdateHighlighting(bool isDepthAvailable)
    {
      bool highlighted = this.highlighted;
      bool flag = false | this.UpdateRenderers();
      this.highlighted = this.once || this.flashing || this.constantly || this.transitionActive;
      int num = 0;
      if (this.highlighted)
      {
        this.UpdateShaderParams(this.seeThrough, true);
        num = !this.seeThrough ? 0 : 2;
      }
      else if (this.occluder && (this.seeThrough || !isDepthAvailable))
      {
        this.UpdateShaderParams(false, this.seeThrough);
        num = !this.seeThrough ? 0 : 1;
        this.highlighted = true;
      }
      if (this.renderQueue != num)
      {
        this.renderQueue = num;
        flag = true;
      }
      if (this.highlighted)
      {
        flag |= this.UpdateVisibility();
        if (this.visible)
          this.UpdateColors();
        else
          this.highlighted = false;
      }
      return flag | highlighted != this.highlighted;
    }

    public void FillBuffer(CommandBuffer buffer, int renderQueue)
    {
      if (!this.highlighted || this.renderQueue != renderQueue)
        return;
      for (int index = this.highlightableRenderers.Count - 1; index >= 0; --index)
      {
        if (!this.highlightableRenderers[index].FillBuffer(buffer))
          this.highlightableRenderers.RemoveAt(index);
      }
    }

    private bool CheckInstance()
    {
      Highlighter[] components = this.GetComponents<Highlighter>();
      if (components.Length <= 1 || !((UnityEngine.Object) components[0] != (UnityEngine.Object) this))
        return true;
      this.enabled = false;
      Debug.LogWarning((object) ("HighlightingSystem : Multiple Highlighter components on a single GameObject is not allowed! Highlighter has been disabled on a GameObject with name '" + this.gameObject.name + "'."));
      return false;
    }

    private bool UpdateRenderers()
    {
      if (this.renderersDirty)
      {
        List<Renderer> renderers = new List<Renderer>();
        this.GrabRenderers(this.tr, ref renderers);
        this.highlightableRenderers = new List<Highlighter.RendererCache>();
        int count = renderers.Count;
        for (int index = 0; index < count; ++index)
          this.highlightableRenderers.Add(new Highlighter.RendererCache(renderers[index], this.opaqueMaterial, this.zTestFloat, this.stencilRefFloat));
        this.highlighted = false;
        this.renderersDirty = false;
        this.currentColor = Color.clear;
        return true;
      }
      bool flag = false;
      for (int index = this.highlightableRenderers.Count - 1; index >= 0; --index)
      {
        if (this.highlightableRenderers[index].IsDestroyed())
        {
          this.highlightableRenderers.RemoveAt(index);
          flag = true;
        }
      }
      return flag;
    }

    private bool UpdateVisibility()
    {
      if (this.visibilityCheckFrame == Time.frameCount)
        return this.visibilityChanged;
      this.visibilityCheckFrame = Time.frameCount;
      this.visible = false;
      this.visibilityChanged = false;
      int index = 0;
      for (int count = this.highlightableRenderers.Count; index < count; ++index)
      {
        Highlighter.RendererCache rendererCache = this.highlightableRenderers[index];
        this.visibilityChanged |= rendererCache.UpdateVisibility();
        this.visible |= rendererCache.visible;
      }
      return this.visibilityChanged;
    }

    private void GrabRenderers(Transform t, ref List<Renderer> renderers)
    {
      GameObject gameObject = t.gameObject;
      int index = 0;
      for (int count = Highlighter.types.Count; index < count; ++index)
      {
        foreach (object obj in (Array) gameObject.GetComponents(Highlighter.types[index]))
          renderers.Add(obj as Renderer);
      }
      if (t.childCount == 0)
        return;
      foreach (object obj in t)
      {
        Transform t1 = obj as Transform;
        if (!((UnityEngine.Object) t1.gameObject.GetComponent<Highlighter>() != (UnityEngine.Object) null))
          this.GrabRenderers(t1, ref renderers);
      }
    }

    private void UpdateShaderParams(bool zt, bool sr)
    {
      if (this.zTest != zt)
      {
        this.zTest = zt;
        float zTestFloat = this.zTestFloat;
        this.opaqueMaterial.SetFloat(ShaderPropertyID._ZTest, zTestFloat);
        for (int index = 0; index < this.highlightableRenderers.Count; ++index)
          this.highlightableRenderers[index].SetZTestForTransparent(zTestFloat);
      }
      if (this.stencilRef == sr)
        return;
      this.stencilRef = sr;
      float stencilRefFloat = this.stencilRefFloat;
      this.opaqueMaterial.SetFloat(ShaderPropertyID._StencilRef, stencilRefFloat);
      for (int index = 0; index < this.highlightableRenderers.Count; ++index)
        this.highlightableRenderers[index].SetStencilRefForTransparent(stencilRefFloat);
    }

    private void UpdateColors()
    {
      if (this.once)
        this.SetColor(this.onceColor);
      else if (this.flashing)
        this.SetColor(Color.Lerp(this.flashingColorMin, this.flashingColorMax, (float) (0.5 * (double) Mathf.Sin((float) ((double) Time.realtimeSinceStartup * (double) this.flashingFreq * 6.28318548202515)) + 0.5)));
      else if (this.transitionActive)
        this.SetColor(new Color(this.constantColor.r, this.constantColor.g, this.constantColor.b, this.constantColor.a * this.transitionValue));
      else if (this.constantly)
      {
        this.SetColor(this.constantColor);
      }
      else
      {
        if (!this.occluder)
          return;
        this.SetColor(this.occluderColor);
      }
    }

    private void SetColor(Color value)
    {
      if (this.currentColor == value)
        return;
      this.currentColor = value;
      this.opaqueMaterial.SetColor(ShaderPropertyID._Outline, this.currentColor);
      for (int index = 0; index < this.highlightableRenderers.Count; ++index)
        this.highlightableRenderers[index].SetColorForTransparent(this.currentColor);
    }

    private void PerformTransition()
    {
      if (!this.transitionActive)
        return;
      if ((double) this.transitionValue == (!this.constantly ? 0.0 : 1.0))
      {
        this.transitionActive = false;
      }
      else
      {
        if ((double) Time.timeScale == 0.0)
          return;
        float num = Time.deltaTime / Time.timeScale;
        this.transitionValue = Mathf.Clamp01(this.transitionValue + (!this.constantly ? -Highlighter.constantOffSpeed : Highlighter.constantOnSpeed) * num);
      }
    }

    public static void SetZWrite(float value)
    {
      if ((double) Highlighter.zWrite == (double) value)
        return;
      Highlighter.zWrite = value;
      Shader.SetGlobalFloat(ShaderPropertyID._HighlightingZWrite, Highlighter.zWrite);
    }

    public static void SetOffsetFactor(float value)
    {
      if ((double) Highlighter.offsetFactor == (double) value)
        return;
      Highlighter.offsetFactor = value;
      Shader.SetGlobalFloat(ShaderPropertyID._HighlightingOffsetFactor, Highlighter.offsetFactor);
    }

    public static void SetOffsetUnits(float value)
    {
      if ((double) Highlighter.offsetUnits == (double) value)
        return;
      Highlighter.offsetUnits = value;
      Shader.SetGlobalFloat(ShaderPropertyID._HighlightingOffsetUnits, Highlighter.offsetUnits);
    }

    private class RendererCache
    {
      private static readonly string sRenderType = "RenderType";
      private static readonly string sOpaque = "Opaque";
      private static readonly string sTransparent = "Transparent";
      private static readonly string sTransparentCutout = "TransparentCutout";
      private static readonly string sMainTex = "_MainTex";
      private const int opaquePassID = 0;
      private const int transparentPassID = 1;
      private GameObject go;
      private Renderer renderer;
      private List<Highlighter.RendererCache.Data> data;

      public bool visible { get; private set; }

      public RendererCache(Renderer r, Material sharedOpaqueMaterial, float zTest, float stencilRef)
      {
        this.data = new List<Highlighter.RendererCache.Data>();
        this.renderer = r;
        this.go = r.gameObject;
        Material[] sharedMaterials = r.sharedMaterials;
        if (sharedMaterials != null)
        {
          for (int index = 0; index < sharedMaterials.Length; ++index)
          {
            Material material1 = sharedMaterials[index];
            if (!((UnityEngine.Object) material1 == (UnityEngine.Object) null))
            {
              Highlighter.RendererCache.Data data = new Highlighter.RendererCache.Data();
              string tag = material1.GetTag(Highlighter.RendererCache.sRenderType, true, Highlighter.RendererCache.sOpaque);
              if (tag == Highlighter.RendererCache.sTransparent || tag == Highlighter.RendererCache.sTransparentCutout)
              {
                Material material2 = new Material(Highlighter.transparentShader);
                material2.SetFloat(ShaderPropertyID._ZTest, zTest);
                material2.SetFloat(ShaderPropertyID._StencilRef, stencilRef);
                if (r is SpriteRenderer)
                  material2.SetFloat(ShaderPropertyID._Cull, 0.0f);
                if (material1.HasProperty(ShaderPropertyID._MainTex))
                {
                  material2.SetTexture(ShaderPropertyID._MainTex, material1.mainTexture);
                  material2.SetTextureOffset(Highlighter.RendererCache.sMainTex, material1.mainTextureOffset);
                  material2.SetTextureScale(Highlighter.RendererCache.sMainTex, material1.mainTextureScale);
                }
                int cutoff = ShaderPropertyID._Cutoff;
                material2.SetFloat(cutoff, !material1.HasProperty(cutoff) ? Highlighter.transparentCutoff : material1.GetFloat(cutoff));
                data.material = material2;
                data.transparent = true;
              }
              else
              {
                data.material = sharedOpaqueMaterial;
                data.transparent = false;
              }
              data.submeshIndex = index;
              this.data.Add(data);
            }
          }
        }
        this.visible = !this.IsDestroyed() && this.IsVisible();
      }

      public bool UpdateVisibility()
      {
        bool flag = !this.IsDestroyed() && this.IsVisible();
        if (this.visible == flag)
          return false;
        this.visible = flag;
        return true;
      }

      public bool FillBuffer(CommandBuffer buffer)
      {
        if (this.IsDestroyed())
          return false;
        if (this.IsVisible())
        {
          int index = 0;
          for (int count = this.data.Count; index < count; ++index)
          {
            Highlighter.RendererCache.Data data = this.data[index];
            buffer.DrawRenderer(this.renderer, data.material, data.submeshIndex);
          }
        }
        return true;
      }

      public void SetColorForTransparent(Color clr)
      {
        int index = 0;
        for (int count = this.data.Count; index < count; ++index)
        {
          Highlighter.RendererCache.Data data = this.data[index];
          if (data.transparent)
            data.material.SetColor(ShaderPropertyID._Outline, clr);
        }
      }

      public void SetZTestForTransparent(float zTest)
      {
        int index = 0;
        for (int count = this.data.Count; index < count; ++index)
        {
          Highlighter.RendererCache.Data data = this.data[index];
          if (data.transparent)
            data.material.SetFloat(ShaderPropertyID._ZTest, zTest);
        }
      }

      public void SetStencilRefForTransparent(float stencilRef)
      {
        int index = 0;
        for (int count = this.data.Count; index < count; ++index)
        {
          Highlighter.RendererCache.Data data = this.data[index];
          if (data.transparent)
            data.material.SetFloat(ShaderPropertyID._StencilRef, stencilRef);
        }
      }

      private bool IsVisible()
      {
        if (this.renderer.enabled)
          return this.renderer.isVisible;
        return false;
      }

      public bool IsDestroyed()
      {
        if (!((UnityEngine.Object) this.go == (UnityEngine.Object) null))
          return (UnityEngine.Object) this.renderer == (UnityEngine.Object) null;
        return true;
      }

      private struct Data
      {
        public Material material;
        public int submeshIndex;
        public bool transparent;
      }
    }
  }
}
