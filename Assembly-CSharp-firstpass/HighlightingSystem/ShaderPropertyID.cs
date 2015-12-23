// Decompiled with JetBrains decompiler
// Type: HighlightingSystem.ShaderPropertyID
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

namespace HighlightingSystem
{
  public static class ShaderPropertyID
  {
    private static bool initialized;

    public static int _MainTex { get; private set; }

    public static int _Outline { get; private set; }

    public static int _Cutoff { get; private set; }

    public static int _Intensity { get; private set; }

    public static int _ZTest { get; private set; }

    public static int _StencilRef { get; private set; }

    public static int _Cull { get; private set; }

    public static int _HighlightingBlur1 { get; private set; }

    public static int _HighlightingBlur2 { get; private set; }

    public static int _HighlightingBuffer { get; private set; }

    public static int _HighlightingBlurred { get; private set; }

    public static int _HighlightingBlurOffset { get; private set; }

    public static int _HighlightingZWrite { get; private set; }

    public static int _HighlightingOffsetFactor { get; private set; }

    public static int _HighlightingOffsetUnits { get; private set; }

    public static int _HighlightingBufferTexelSize { get; private set; }

    public static void Initialize()
    {
      if (ShaderPropertyID.initialized)
        return;
      ShaderPropertyID._MainTex = Shader.PropertyToID("_MainTex");
      ShaderPropertyID._Outline = Shader.PropertyToID("_Outline");
      ShaderPropertyID._Cutoff = Shader.PropertyToID("_Cutoff");
      ShaderPropertyID._Intensity = Shader.PropertyToID("_Intensity");
      ShaderPropertyID._ZTest = Shader.PropertyToID("_ZTest");
      ShaderPropertyID._StencilRef = Shader.PropertyToID("_StencilRef");
      ShaderPropertyID._Cull = Shader.PropertyToID("_Cull");
      ShaderPropertyID._HighlightingBlur1 = Shader.PropertyToID("_HighlightingBlur1");
      ShaderPropertyID._HighlightingBlur2 = Shader.PropertyToID("_HighlightingBlur2");
      ShaderPropertyID._HighlightingBuffer = Shader.PropertyToID("_HighlightingBuffer");
      ShaderPropertyID._HighlightingBlurred = Shader.PropertyToID("_HighlightingBlurred");
      ShaderPropertyID._HighlightingBlurOffset = Shader.PropertyToID("_HighlightingBlurOffset");
      ShaderPropertyID._HighlightingZWrite = Shader.PropertyToID("_HighlightingZWrite");
      ShaderPropertyID._HighlightingOffsetFactor = Shader.PropertyToID("_HighlightingOffsetFactor");
      ShaderPropertyID._HighlightingOffsetUnits = Shader.PropertyToID("_HighlightingOffsetUnits");
      ShaderPropertyID._HighlightingBufferTexelSize = Shader.PropertyToID("_HighlightingBufferTexelSize");
      ShaderPropertyID.initialized = true;
    }
  }
}
