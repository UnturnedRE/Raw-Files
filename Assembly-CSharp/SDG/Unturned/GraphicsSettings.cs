// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.GraphicsSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using HighlightingSystem;
using UnityEngine;

namespace SDG.Unturned
{
  public class GraphicsSettings
  {
    public static readonly byte SAVEDATA_VERSION = (byte) 7;
    private static readonly float EFFECT_ULTRA = 240f;
    private static readonly float EFFECT_HIGH = 120f;
    private static readonly float EFFECT_MEDIUM = 60f;
    private static readonly float EFFECT_LOW = 20f;
    private static GraphicsSettingsData graphicsSettingsData;
    private static bool changeResolution;

    public static float effect
    {
      get
      {
        if (GraphicsSettings.effectQuality == EGraphicQuality.ULTRA)
          return Random.Range(GraphicsSettings.EFFECT_ULTRA - 16f, GraphicsSettings.EFFECT_ULTRA + 16f);
        if (GraphicsSettings.effectQuality == EGraphicQuality.HIGH)
          return Random.Range(GraphicsSettings.EFFECT_HIGH - 8f, GraphicsSettings.EFFECT_HIGH + 8f);
        if (GraphicsSettings.effectQuality == EGraphicQuality.MEDIUM)
          return Random.Range(GraphicsSettings.EFFECT_MEDIUM - 4f, GraphicsSettings.EFFECT_MEDIUM + 4f);
        if (GraphicsSettings.effectQuality == EGraphicQuality.LOW)
          return Random.Range(GraphicsSettings.EFFECT_LOW - 2f, GraphicsSettings.EFFECT_LOW + 2f);
        return 0.0f;
      }
    }

    public static GraphicsSettingsResolution resolution
    {
      get
      {
        return GraphicsSettings.graphicsSettingsData.Resolution;
      }
      set
      {
        GraphicsSettings.graphicsSettingsData.Resolution = value;
        GraphicsSettings.changeResolution = true;
      }
    }

    public static bool fullscreen
    {
      get
      {
        return GraphicsSettings.graphicsSettingsData.IsFullscreenEnabled;
      }
      set
      {
        GraphicsSettings.graphicsSettingsData.IsFullscreenEnabled = value;
        GraphicsSettings.changeResolution = true;
      }
    }

    public static bool buffer
    {
      get
      {
        return GraphicsSettings.graphicsSettingsData.IsVSyncEnabled;
      }
      set
      {
        GraphicsSettings.graphicsSettingsData.IsVSyncEnabled = value;
      }
    }

    public static EAntiAliasingType antiAliasingType
    {
      get
      {
        return GraphicsSettings.graphicsSettingsData.AntiAliasingType;
      }
      set
      {
        GraphicsSettings.graphicsSettingsData.AntiAliasingType = value;
      }
    }

    public static bool motionBlur
    {
      get
      {
        return GraphicsSettings.graphicsSettingsData.IsMotionBlurEnabled;
      }
      set
      {
        GraphicsSettings.graphicsSettingsData.IsMotionBlurEnabled = value;
      }
    }

    public static bool ambientOcclusion
    {
      get
      {
        return GraphicsSettings.graphicsSettingsData.IsSSAOEnabled;
      }
      set
      {
        GraphicsSettings.graphicsSettingsData.IsSSAOEnabled = value;
      }
    }

    public static bool sunShafts
    {
      get
      {
        return GraphicsSettings.graphicsSettingsData.IsSunShaftsEnabled;
      }
      set
      {
        GraphicsSettings.graphicsSettingsData.IsSunShaftsEnabled = value;
      }
    }

    public static bool bloom
    {
      get
      {
        return GraphicsSettings.graphicsSettingsData.IsBloomEnabled;
      }
      set
      {
        GraphicsSettings.graphicsSettingsData.IsBloomEnabled = value;
      }
    }

    public static bool clouds
    {
      get
      {
        return GraphicsSettings.graphicsSettingsData.IsCloudEnabled;
      }
      set
      {
        GraphicsSettings.graphicsSettingsData.IsCloudEnabled = value;
      }
    }

    public static bool terrain
    {
      get
      {
        return GraphicsSettings.graphicsSettingsData.IsTerrainEnabled;
      }
      set
      {
        GraphicsSettings.graphicsSettingsData.IsTerrainEnabled = value;
      }
    }

    public static bool fog
    {
      get
      {
        return GraphicsSettings.graphicsSettingsData.IsFogEnabled;
      }
      set
      {
        GraphicsSettings.graphicsSettingsData.IsFogEnabled = value;
      }
    }

    public static float distance
    {
      get
      {
        return GraphicsSettings.graphicsSettingsData.DrawDistance;
      }
      set
      {
        GraphicsSettings.graphicsSettingsData.DrawDistance = value;
      }
    }

    public static EGraphicQuality effectQuality
    {
      get
      {
        return GraphicsSettings.graphicsSettingsData.EffectQuality;
      }
      set
      {
        GraphicsSettings.graphicsSettingsData.EffectQuality = value;
      }
    }

    public static EGraphicQuality foliageQuality
    {
      get
      {
        return GraphicsSettings.graphicsSettingsData.FoliageQuality;
      }
      set
      {
        GraphicsSettings.graphicsSettingsData.FoliageQuality = value;
      }
    }

    public static EGraphicQuality lightingQuality
    {
      get
      {
        return GraphicsSettings.graphicsSettingsData.LightingQuality;
      }
      set
      {
        GraphicsSettings.graphicsSettingsData.LightingQuality = value;
      }
    }

    public static EGraphicQuality waterQuality
    {
      get
      {
        return GraphicsSettings.graphicsSettingsData.WaterQuality;
      }
      set
      {
        GraphicsSettings.graphicsSettingsData.WaterQuality = value;
      }
    }

    public static EGraphicQuality scopeQuality
    {
      get
      {
        return GraphicsSettings.graphicsSettingsData.ScopeQuality;
      }
      set
      {
        GraphicsSettings.graphicsSettingsData.ScopeQuality = value;
      }
    }

    public static EGraphicQuality outlineQuality
    {
      get
      {
        return GraphicsSettings.graphicsSettingsData.OutlineQuality;
      }
      set
      {
        GraphicsSettings.graphicsSettingsData.OutlineQuality = value;
      }
    }

    public static void resize()
    {
      if (Application.isEditor)
        return;
      if (GraphicsSettings.resolution.Width < Screen.resolutions[0].width || GraphicsSettings.resolution.Height < Screen.resolutions[0].height)
        GraphicsSettings.resolution = new GraphicsSettingsResolution(Screen.resolutions[0]);
      else if (GraphicsSettings.resolution.Width > Screen.resolutions[Screen.resolutions.Length - 1].width || GraphicsSettings.resolution.Height > Screen.resolutions[Screen.resolutions.Length - 1].height)
        GraphicsSettings.resolution = new GraphicsSettingsResolution(Screen.resolutions[0]);
      Screen.SetResolution(GraphicsSettings.resolution.Width, GraphicsSettings.resolution.Height, GraphicsSettings.fullscreen);
    }

    public static void apply()
    {
      if (GraphicsSettings.changeResolution)
      {
        GraphicsSettings.changeResolution = false;
        if (!Application.isEditor)
        {
          if (Provider.isConnected)
            PlayerUI.rebuild();
          else
            MenuUI.rebuild();
        }
      }
      QualitySettings.SetQualityLevel((int) (byte) GraphicsSettings.lightingQuality + 1, true);
      QualitySettings.vSyncCount = !GraphicsSettings.buffer ? 0 : 1;
      switch (GraphicsSettings.antiAliasingType)
      {
        case EAntiAliasingType.MSAA2:
          QualitySettings.antiAliasing = 2;
          break;
        case EAntiAliasingType.MSAA4:
          QualitySettings.antiAliasing = 4;
          break;
        case EAntiAliasingType.MSAA8:
          QualitySettings.antiAliasing = 8;
          break;
        default:
          QualitySettings.antiAliasing = 0;
          break;
      }
      float[] numArray = new float[32]
      {
        (float) (32.0 + (double) GraphicsSettings.distance * 32.0),
        0.0f,
        0.0f,
        0.0f,
        (float) (8192.0 + (double) GraphicsSettings.distance * 8192.0),
        0.0f,
        0.0f,
        0.0f,
        (float) (256.0 + (double) GraphicsSettings.distance * 256.0),
        0.0f,
        (float) (256.0 + (double) GraphicsSettings.distance * 256.0),
        (float) (256.0 + (double) GraphicsSettings.distance * 256.0),
        (float) (256.0 + (double) GraphicsSettings.distance * 256.0),
        (float) (32.0 + (double) GraphicsSettings.distance * 32.0),
        (float) (256.0 + (double) GraphicsSettings.distance * 256.0),
        (float) (256.0 + (double) GraphicsSettings.distance * 256.0),
        (float) (128.0 + (double) GraphicsSettings.distance * 128.0),
        (float) (32.0 + (double) GraphicsSettings.distance * 32.0),
        (float) (8192.0 + (double) GraphicsSettings.distance * 8192.0),
        (float) (8192.0 + (double) GraphicsSettings.distance * 8192.0),
        (float) (8192.0 + (double) GraphicsSettings.distance * 8192.0),
        (float) (8192.0 + (double) GraphicsSettings.distance * 8192.0),
        (float) (256.0 + (double) GraphicsSettings.distance * 256.0),
        (float) (128.0 + (double) GraphicsSettings.distance * 128.0),
        0.0f,
        (float) (256.0 + (double) GraphicsSettings.distance * 256.0),
        (float) (256.0 + (double) GraphicsSettings.distance * 256.0),
        (float) (256.0 + (double) GraphicsSettings.distance * 256.0),
        (float) (256.0 + (double) GraphicsSettings.distance * 256.0),
        (float) (256.0 + (double) GraphicsSettings.distance * 256.0),
        (float) (256.0 + (double) GraphicsSettings.distance * 256.0),
        0.0f
      };
      QualitySettings.lodBias = (float) (1.0 + (double) GraphicsSettings.distance * 3.0);
      if ((Object) Camera.main != (Object) null)
      {
        Camera.main.GetComponent<AntialiasingAsPostEffect>().enabled = GraphicsSettings.antiAliasingType == EAntiAliasingType.FXAA;
        Camera.main.GetComponent<CameraMotionBlur>().enabled = GraphicsSettings.motionBlur;
        Camera.main.GetComponent<SESSAO>().enabled = GraphicsSettings.ambientOcclusion;
        Camera.main.GetComponent<SunShafts>().enabled = GraphicsSettings.sunShafts;
        Camera.main.GetComponent<GlobalFog>().enabled = GraphicsSettings.fog;
        HighlightingRenderer component = Camera.main.GetComponent<HighlightingRenderer>();
        if ((Object) component != (Object) null)
        {
          if (GraphicsSettings.outlineQuality == EGraphicQuality.LOW)
          {
            component.downsampleFactor = 4;
            component.iterations = 1;
            component.blurMinSpread = 0.75f;
            component.blurSpread = 0.0f;
            component.blurIntensity = 0.25f;
          }
          else if (GraphicsSettings.outlineQuality == EGraphicQuality.MEDIUM)
          {
            component.downsampleFactor = 4;
            component.iterations = 2;
            component.blurMinSpread = 0.5f;
            component.blurSpread = 0.25f;
            component.blurIntensity = 0.25f;
          }
          else if (GraphicsSettings.outlineQuality == EGraphicQuality.HIGH)
          {
            component.downsampleFactor = 2;
            component.iterations = 2;
            component.blurMinSpread = 1f;
            component.blurSpread = 0.5f;
            component.blurIntensity = 0.25f;
          }
          else if (GraphicsSettings.outlineQuality == EGraphicQuality.ULTRA)
          {
            component.downsampleFactor = 2;
            component.iterations = 3;
            component.blurMinSpread = 0.5f;
            component.blurSpread = 0.5f;
            component.blurIntensity = 0.25f;
          }
        }
        Camera.main.layerCullDistances = numArray;
        Camera.main.layerCullSpherical = true;
        if ((Object) Player.player != (Object) null)
        {
          Player.player.look.scopeCamera.layerCullDistances = numArray;
          Player.player.look.scopeCamera.layerCullSpherical = true;
          Player.player.look.updateScope(GraphicsSettings.scopeQuality);
          Player.player.look.scopeCamera.GetComponent<GlobalFog>().enabled = GraphicsSettings.fog;
          Player.player.animator.view.GetComponent<AntialiasingAsPostEffect>().enabled = GraphicsSettings.antiAliasingType == EAntiAliasingType.FXAA;
          Camera.main.GetComponent<Bloom>().enabled = Player.player.look.perspective == EPlayerPerspective.THIRD && GraphicsSettings.bloom;
          Player.player.animator.view.GetComponent<Bloom>().enabled = Player.player.look.perspective == EPlayerPerspective.FIRST && GraphicsSettings.bloom;
        }
        else
          Camera.main.GetComponent<Bloom>().enabled = GraphicsSettings.bloom;
      }
      if ((Object) LevelGround.terrain != (Object) null)
      {
        Terrain terrain = LevelGround.terrain;
        if (GraphicsSettings.foliageQuality == EGraphicQuality.OFF)
        {
          terrain.detailObjectDensity = 0.0f;
          terrain.detailObjectDistance = 0.0f;
          terrain.terrainData.wavingGrassAmount = 0.0f;
          terrain.terrainData.wavingGrassSpeed = 1f;
          terrain.terrainData.wavingGrassStrength = 1f;
        }
        else if (GraphicsSettings.foliageQuality == EGraphicQuality.LOW)
        {
          terrain.detailObjectDensity = 0.25f;
          terrain.detailObjectDistance = 60f;
          terrain.terrainData.wavingGrassAmount = 0.0f;
          terrain.terrainData.wavingGrassSpeed = 1f;
          terrain.terrainData.wavingGrassStrength = 1f;
        }
        else if (GraphicsSettings.foliageQuality == EGraphicQuality.MEDIUM)
        {
          terrain.detailObjectDensity = 0.5f;
          terrain.detailObjectDistance = 120f;
          terrain.terrainData.wavingGrassAmount = 0.0f;
          terrain.terrainData.wavingGrassSpeed = 1f;
          terrain.terrainData.wavingGrassStrength = 1f;
        }
        else if (GraphicsSettings.foliageQuality == EGraphicQuality.HIGH)
        {
          terrain.detailObjectDensity = 0.75f;
          terrain.detailObjectDistance = 180f;
          terrain.terrainData.wavingGrassAmount = 0.25f;
          terrain.terrainData.wavingGrassSpeed = 0.5f;
          terrain.terrainData.wavingGrassStrength = 1f;
        }
        else if (GraphicsSettings.foliageQuality == EGraphicQuality.ULTRA)
        {
          terrain.detailObjectDensity = 1f;
          terrain.detailObjectDistance = 250f;
          terrain.terrainData.wavingGrassAmount = 0.25f;
          terrain.terrainData.wavingGrassSpeed = 0.5f;
          terrain.terrainData.wavingGrassStrength = 1f;
        }
      }
      if ((Object) LevelGround.terrain2 != (Object) null)
        LevelGround.terrain2.gameObject.SetActive(GraphicsSettings.terrain || Level.isEditor);
      if ((Object) LevelLighting.sea != (Object) null)
      {
        Material sea = LevelLighting.sea;
        PlanarReflection reflection = LevelLighting.reflection;
        if ((Object) sea != (Object) null && (Object) reflection != (Object) null)
        {
          if (GraphicsSettings.waterQuality == EGraphicQuality.LOW)
          {
            sea.shader.maximumLOD = 201;
            Shader.EnableKeyword("WATER_EDGEBLEND_OFF");
            Shader.DisableKeyword("WATER_EDGEBLEND_ON");
            Shader.DisableKeyword("WATER_REFLECTIVE");
            Shader.EnableKeyword("WATER_SIMPLE");
            reflection.enabled = false;
          }
          else if (GraphicsSettings.waterQuality == EGraphicQuality.MEDIUM)
          {
            sea.shader.maximumLOD = 301;
            Shader.EnableKeyword("WATER_EDGEBLEND_OFF");
            Shader.DisableKeyword("WATER_EDGEBLEND_ON");
            Shader.DisableKeyword("WATER_REFLECTIVE");
            Shader.EnableKeyword("WATER_SIMPLE");
            reflection.enabled = false;
          }
          else if (GraphicsSettings.waterQuality == EGraphicQuality.HIGH)
          {
            sea.shader.maximumLOD = 501;
            if (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
            {
              Shader.EnableKeyword("WATER_EDGEBLEND_ON");
              Shader.DisableKeyword("WATER_EDGEBLEND_OFF");
              if ((Object) Camera.main != (Object) null)
                Camera.main.depthTextureMode |= DepthTextureMode.Depth;
            }
            else
            {
              Shader.EnableKeyword("WATER_EDGEBLEND_OFF");
              Shader.DisableKeyword("WATER_EDGEBLEND_ON");
            }
            Shader.DisableKeyword("WATER_REFLECTIVE");
            Shader.EnableKeyword("WATER_SIMPLE");
            reflection.enabled = false;
          }
          else if (GraphicsSettings.waterQuality == EGraphicQuality.ULTRA)
          {
            sea.shader.maximumLOD = 501;
            if (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
            {
              Shader.EnableKeyword("WATER_EDGEBLEND_ON");
              Shader.DisableKeyword("WATER_EDGEBLEND_OFF");
              if ((Object) Camera.main != (Object) null)
                Camera.main.depthTextureMode |= DepthTextureMode.Depth;
            }
            else
            {
              Shader.EnableKeyword("WATER_EDGEBLEND_OFF");
              Shader.DisableKeyword("WATER_EDGEBLEND_ON");
            }
            reflection.enabled = true;
          }
        }
      }
      if (!((Object) LevelLighting.clouds != (Object) null))
        return;
      LevelLighting.clouds.gameObject.SetActive(GraphicsSettings.clouds);
      LevelLighting.clouds.GetComponent<ParticleSystem>().Play();
    }

    public static void load()
    {
      if (ReadWrite.fileExists("/Settings/Graphics.json", true))
      {
        GraphicsSettings.graphicsSettingsData = ReadWrite.deserializeJSON<GraphicsSettingsData>("/Settings/Graphics.json", true);
        if (GraphicsSettings.graphicsSettingsData == null)
          GraphicsSettings.graphicsSettingsData = new GraphicsSettingsData();
      }
      else
        GraphicsSettings.graphicsSettingsData = new GraphicsSettingsData();
      if (GraphicsSettings.graphicsSettingsData.AntiAliasingType > EAntiAliasingType.FXAA)
        GraphicsSettings.graphicsSettingsData.AntiAliasingType = EAntiAliasingType.FXAA;
      if (Application.isEditor || GraphicsSettings.resolution.Width <= Screen.resolutions[Screen.resolutions.Length - 1].width && GraphicsSettings.resolution.Height <= Screen.resolutions[Screen.resolutions.Length - 1].height)
        return;
      GraphicsSettings.resolution = new GraphicsSettingsResolution(Screen.resolutions[Screen.resolutions.Length - 1]);
    }

    public static void save()
    {
      ReadWrite.serializeJSON<GraphicsSettingsData>("/Settings/Graphics.json", true, GraphicsSettings.graphicsSettingsData);
    }
  }
}
