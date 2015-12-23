// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.LevelLighting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class LevelLighting
  {
    public static readonly byte SAVEDATA_VERSION = (byte) 6;
    public static readonly byte MOON_CYCLES = (byte) 5;
    public static readonly float FOG = 128f;
    public static readonly float CLOUDS = 2f;
    public static readonly float AUDIO = 0.15f;
    private static readonly Color FOAM_DAWN = new Color(0.125f, 0.0f, 0.0f, 0.0f);
    private static readonly Color FOAM_MIDDAY = new Color(0.25f, 0.0f, 0.0f, 0.0f);
    private static readonly Color FOAM_DUSK = new Color(0.05f, 0.0f, 0.0f, 0.0f);
    private static readonly Color FOAM_MIDNIGHT = new Color(0.01f, 0.0f, 0.0f, 0.0f);
    private static readonly float SPECULAR_DAWN = 5f;
    private static readonly float SPECULAR_MIDDAY = 50f;
    private static readonly float SPECULAR_DUSK = 5f;
    private static readonly float SPECULAR_MIDNIGHT = 50f;
    private static readonly Color NIGHTVISION_MILITARY = new Color(0.0f, 1f, 0.0f, 0.0f);
    private static readonly Color NIGHTVISION_CIVILIAN = new Color(0.25f, 0.25f, 0.25f, 0.0f);
    private static float _azimuth;
    private static float _transition;
    private static float _bias;
    private static float _fade;
    private static float _time;
    private static float _wind;
    private static byte[] _hash;
    private static LightingInfo[] _times;
    private static float _seaLevel;
    private static float _snowLevel;
    public static ELightingVision vision;
    private static bool isSea;
    private static bool isSnow;
    private static Material skybox;
    private static Material mist;
    private static Transform lighting;
    private static Color skyboxSky;
    private static Color skyboxEquator;
    private static Color skyboxGround;
    private static Color skyboxClouds;
    private static Transform sun;
    private static Transform sunFlare;
    private static Transform moonFlare;
    private static AudioSource _dayAudio;
    private static AudioSource _nightAudio;
    private static AudioSource _waterAudio;
    private static AudioSource _windAudio;
    private static AudioSource _belowAudio;
    private static float dayVolume;
    private static float nightVolume;
    private static Transform stars;
    private static Transform _bubbles;
    private static Transform _snow;
    private static Transform _clouds;
    private static Material _sea;
    private static PlanarReflection _reflection;
    private static Transform water;
    private static Material[] moons;
    private static byte _moon;
    private static bool init;

    public static float azimuth
    {
      get
      {
        return LevelLighting._azimuth;
      }
      set
      {
        LevelLighting._azimuth = value;
        LevelLighting.updateLighting();
      }
    }

    public static float transition
    {
      get
      {
        return LevelLighting._transition;
      }
    }

    public static float bias
    {
      get
      {
        return LevelLighting._bias;
      }
      set
      {
        LevelLighting._bias = value;
        LevelLighting._transition = (double) LevelLighting.bias >= 1.0 - (double) LevelLighting.bias ? (float) ((1.0 - (double) LevelLighting.bias) / 2.0) * LevelLighting.fade : LevelLighting.bias / 2f * LevelLighting.fade;
        LevelLighting.updateLighting();
      }
    }

    public static float fade
    {
      get
      {
        return LevelLighting._fade;
      }
      set
      {
        LevelLighting._fade = value;
        LevelLighting._transition = (double) LevelLighting.bias >= 1.0 - (double) LevelLighting.bias ? (float) ((1.0 - (double) LevelLighting.bias) / 2.0) * LevelLighting.fade : LevelLighting.bias / 2f * LevelLighting.fade;
        LevelLighting.updateLighting();
      }
    }

    public static float time
    {
      get
      {
        return LevelLighting._time;
      }
      set
      {
        LevelLighting._time = value;
        LevelLighting.updateLighting();
      }
    }

    public static float wind
    {
      get
      {
        return LevelLighting._wind;
      }
      set
      {
        LevelLighting._wind = value;
      }
    }

    public static byte[] hash
    {
      get
      {
        return LevelLighting._hash;
      }
    }

    public static LightingInfo[] times
    {
      get
      {
        return LevelLighting._times;
      }
    }

    public static float seaLevel
    {
      get
      {
        return LevelLighting._seaLevel;
      }
      set
      {
        LevelLighting._seaLevel = value;
        LevelLighting.updateSea();
      }
    }

    public static float snowLevel
    {
      get
      {
        return LevelLighting._snowLevel;
      }
      set
      {
        LevelLighting._snowLevel = value;
      }
    }

    public static AudioSource dayAudio
    {
      get
      {
        return LevelLighting._dayAudio;
      }
    }

    public static AudioSource nightAudio
    {
      get
      {
        return LevelLighting._nightAudio;
      }
    }

    public static AudioSource waterAudio
    {
      get
      {
        return LevelLighting._waterAudio;
      }
    }

    public static AudioSource windAudio
    {
      get
      {
        return LevelLighting._windAudio;
      }
    }

    public static AudioSource belowAudio
    {
      get
      {
        return LevelLighting._belowAudio;
      }
    }

    public static Transform bubbles
    {
      get
      {
        return LevelLighting._bubbles;
      }
    }

    public static Transform snow
    {
      get
      {
        return LevelLighting._snow;
      }
    }

    public static Transform clouds
    {
      get
      {
        return LevelLighting._clouds;
      }
    }

    public static Material sea
    {
      get
      {
        return LevelLighting._sea;
      }
    }

    public static PlanarReflection reflection
    {
      get
      {
        return LevelLighting._reflection;
      }
    }

    public static byte moon
    {
      get
      {
        return LevelLighting._moon;
      }
      set
      {
        LevelLighting._moon = value;
        if (Dedicator.isDedicated || (int) LevelLighting.moon >= LevelLighting.moons.Length)
          return;
        LevelLighting.moonFlare.GetComponent<Renderer>().material = LevelLighting.moons[(int) LevelLighting.moon];
      }
    }

    public static void setEnabled(bool isEnabled)
    {
      LevelLighting.sun.GetComponent<Light>().enabled = isEnabled;
    }

    public static void updateLighting()
    {
      float t = 0.0f;
      LightingInfo lightingInfo1;
      LightingInfo lightingInfo2;
      if ((double) LevelLighting.time < (double) LevelLighting.bias)
      {
        LevelLighting.sun.rotation = Quaternion.Euler((float) ((double) LevelLighting.time / (double) LevelLighting.bias * 180.0), LevelLighting.azimuth, 0.0f);
        LevelLighting.sea.SetVector("_WorldLightDir", (Vector4) LevelLighting.sunFlare.forward);
        if ((double) LevelLighting.time < (double) LevelLighting.transition)
        {
          LevelLighting.dayVolume = Mathf.Lerp(0.5f, 1f, LevelLighting.time / LevelLighting.transition) * LevelLighting.AUDIO;
          LevelLighting.nightVolume = Mathf.Lerp(0.5f, 0.0f, LevelLighting.time / LevelLighting.transition) * LevelLighting.AUDIO;
          lightingInfo1 = LevelLighting.times[0];
          lightingInfo2 = LevelLighting.times[1];
          t = LevelLighting.time / LevelLighting.transition;
          LevelLighting.sea.SetColor("_Foam", Color.Lerp(LevelLighting.FOAM_DAWN, LevelLighting.FOAM_MIDDAY, LevelLighting.time / LevelLighting.transition));
          LevelLighting.sea.SetFloat("_Shininess", Mathf.Lerp(LevelLighting.SPECULAR_DAWN, LevelLighting.SPECULAR_MIDDAY, LevelLighting.time / LevelLighting.transition));
        }
        else if ((double) LevelLighting.time < (double) LevelLighting.bias - (double) LevelLighting.transition)
        {
          LevelLighting.dayVolume = LevelLighting.AUDIO;
          LevelLighting.nightVolume = 0.0f;
          lightingInfo1 = (LightingInfo) null;
          lightingInfo2 = LevelLighting.times[1];
          LevelLighting.sea.SetColor("_Foam", LevelLighting.FOAM_MIDDAY);
          LevelLighting.sea.SetFloat("_Shininess", LevelLighting.SPECULAR_MIDDAY);
        }
        else
        {
          LevelLighting.dayVolume = Mathf.Lerp(1f, 0.5f, (LevelLighting.time - LevelLighting.bias + LevelLighting.transition) / LevelLighting.transition) * LevelLighting.AUDIO;
          LevelLighting.nightVolume = Mathf.Lerp(0.0f, 0.5f, (LevelLighting.time - LevelLighting.bias + LevelLighting.transition) / LevelLighting.transition) * LevelLighting.AUDIO;
          lightingInfo1 = LevelLighting.times[1];
          lightingInfo2 = LevelLighting.times[2];
          t = (LevelLighting.time - LevelLighting.bias + LevelLighting.transition) / LevelLighting.transition;
          LevelLighting.sea.SetColor("_Foam", Color.Lerp(LevelLighting.FOAM_MIDDAY, LevelLighting.FOAM_DUSK, (LevelLighting.time - LevelLighting.bias + LevelLighting.transition) / LevelLighting.transition));
          LevelLighting.sea.SetFloat("_Shininess", Mathf.Lerp(LevelLighting.SPECULAR_MIDDAY, LevelLighting.SPECULAR_DUSK, (LevelLighting.time - LevelLighting.bias + LevelLighting.transition) / LevelLighting.transition));
        }
        LevelLighting.updateStars(0.0f);
      }
      else
      {
        LevelLighting.sun.rotation = Quaternion.Euler((float) (180.0 + ((double) LevelLighting.time - (double) LevelLighting.bias) / (1.0 - (double) LevelLighting.bias) * 180.0), LevelLighting.azimuth, 0.0f);
        if ((double) LevelLighting.time < (double) LevelLighting.bias + (double) LevelLighting.transition)
        {
          LevelLighting.dayVolume = Mathf.Lerp(0.5f, 0.0f, (LevelLighting.time - LevelLighting.bias) / LevelLighting.transition) * LevelLighting.AUDIO;
          LevelLighting.nightVolume = Mathf.Lerp(0.5f, 1f, (LevelLighting.time - LevelLighting.bias) / LevelLighting.transition) * LevelLighting.AUDIO;
          lightingInfo1 = LevelLighting.times[2];
          lightingInfo2 = LevelLighting.times[3];
          t = (LevelLighting.time - LevelLighting.bias) / LevelLighting.transition;
          LevelLighting.sea.SetColor("_Foam", Color.Lerp(LevelLighting.FOAM_DUSK, LevelLighting.FOAM_MIDNIGHT, (LevelLighting.time - LevelLighting.bias) / LevelLighting.transition));
          LevelLighting.sea.SetFloat("_Shininess", Mathf.Lerp(LevelLighting.SPECULAR_DUSK, LevelLighting.SPECULAR_MIDNIGHT, (LevelLighting.time - LevelLighting.bias) / LevelLighting.transition));
          LevelLighting.updateStars(Mathf.Lerp(0.0f, 1f, t));
        }
        else if ((double) LevelLighting.time < 1.0 - (double) LevelLighting.transition)
        {
          LevelLighting.dayVolume = 0.0f;
          LevelLighting.nightVolume = LevelLighting.AUDIO;
          lightingInfo1 = (LightingInfo) null;
          lightingInfo2 = LevelLighting.times[3];
          LevelLighting.sea.SetColor("_Foam", LevelLighting.FOAM_MIDNIGHT);
          LevelLighting.sea.SetFloat("_Shininess", LevelLighting.SPECULAR_MIDNIGHT);
          LevelLighting.updateStars(1f);
        }
        else
        {
          LevelLighting.dayVolume = Mathf.Lerp(0.0f, 0.5f, (LevelLighting.time - 1f + LevelLighting.transition) / LevelLighting.transition) * LevelLighting.AUDIO;
          LevelLighting.nightVolume = Mathf.Lerp(1f, 0.5f, (LevelLighting.time - 1f + LevelLighting.transition) / LevelLighting.transition) * LevelLighting.AUDIO;
          lightingInfo1 = LevelLighting.times[3];
          lightingInfo2 = LevelLighting.times[0];
          t = (LevelLighting.time - 1f + LevelLighting.transition) / LevelLighting.transition;
          LevelLighting.sea.SetColor("_Foam", Color.Lerp(LevelLighting.FOAM_MIDNIGHT, LevelLighting.FOAM_DAWN, (LevelLighting.time - 1f + LevelLighting.transition) / LevelLighting.transition));
          LevelLighting.sea.SetFloat("_Shininess", Mathf.Lerp(LevelLighting.SPECULAR_MIDNIGHT, LevelLighting.SPECULAR_DAWN, (LevelLighting.time - 1f + LevelLighting.transition) / LevelLighting.transition));
          LevelLighting.updateStars(Mathf.Lerp(1f, 0.0f, t));
        }
      }
      if (lightingInfo1 == null)
      {
        LevelLighting.sun.GetComponent<Light>().color = lightingInfo2.colors[0];
        LevelLighting.sun.GetComponent<Light>().intensity = lightingInfo2.singles[0];
        LevelLighting.sun.GetComponent<Light>().shadowStrength = lightingInfo2.singles[3];
        LevelLighting.sea.SetColor("_BaseColor", lightingInfo2.colors[1]);
        LevelLighting.sea.SetColor("_ReflectionColor", lightingInfo2.colors[1]);
        RenderSettings.ambientSkyColor = lightingInfo2.colors[6];
        RenderSettings.ambientEquatorColor = lightingInfo2.colors[7];
        RenderSettings.ambientGroundColor = lightingInfo2.colors[8];
        LevelLighting.skyboxSky = lightingInfo2.colors[3];
        LevelLighting.skyboxEquator = lightingInfo2.colors[4];
        LevelLighting.skyboxGround = lightingInfo2.colors[5];
        LevelLighting.skyboxClouds = lightingInfo2.colors[9];
        if ((Object) Camera.main != (Object) null)
        {
          GlobalFog component = Camera.main.GetComponent<GlobalFog>();
          if ((Object) component != (Object) null)
          {
            component.height = (double) LevelLighting.seaLevel >= 0.990000009536743 ? -LevelLighting.FOG : (float) ((double) LevelLighting.seaLevel * (double) Level.TERRAIN - (double) LevelLighting.FOG + (double) lightingInfo2.singles[1] * (double) LevelLighting.FOG);
            component.globalFogColor = lightingInfo2.colors[2];
          }
        }
        LevelLighting.clouds.GetComponent<ParticleSystem>().emissionRate = lightingInfo2.singles[2];
      }
      else
      {
        LevelLighting.sun.GetComponent<Light>().color = Color.Lerp(lightingInfo1.colors[0], lightingInfo2.colors[0], t);
        LevelLighting.sun.GetComponent<Light>().intensity = Mathf.Lerp(lightingInfo1.singles[0], lightingInfo2.singles[0], t);
        LevelLighting.sun.GetComponent<Light>().shadowStrength = Mathf.Lerp(lightingInfo1.singles[3], lightingInfo2.singles[3], t);
        LevelLighting.sea.SetColor("_BaseColor", Color.Lerp(lightingInfo1.colors[1], lightingInfo2.colors[1], t));
        LevelLighting.sea.SetColor("_ReflectionColor", Color.Lerp(lightingInfo1.colors[1], lightingInfo2.colors[1], t));
        RenderSettings.ambientSkyColor = Color.Lerp(lightingInfo1.colors[6], lightingInfo2.colors[6], t);
        RenderSettings.ambientEquatorColor = Color.Lerp(lightingInfo1.colors[7], lightingInfo2.colors[7], t);
        RenderSettings.ambientGroundColor = Color.Lerp(lightingInfo1.colors[8], lightingInfo2.colors[8], t);
        LevelLighting.skyboxSky = Color.Lerp(lightingInfo1.colors[3], lightingInfo2.colors[3], t);
        LevelLighting.skyboxEquator = Color.Lerp(lightingInfo1.colors[4], lightingInfo2.colors[4], t);
        LevelLighting.skyboxGround = Color.Lerp(lightingInfo1.colors[5], lightingInfo2.colors[5], t);
        LevelLighting.skyboxClouds = Color.Lerp(lightingInfo1.colors[9], lightingInfo2.colors[9], t);
        if ((Object) Camera.main != (Object) null)
        {
          GlobalFog component = Camera.main.GetComponent<GlobalFog>();
          if ((Object) component != (Object) null)
          {
            component.height = (double) LevelLighting.seaLevel >= 0.990000009536743 ? -LevelLighting.FOG : (float) ((double) LevelLighting.seaLevel * (double) Level.TERRAIN - (double) LevelLighting.FOG + (double) Mathf.Lerp(lightingInfo1.singles[1], lightingInfo2.singles[1], t) * (double) LevelLighting.FOG);
            component.globalFogColor = Color.Lerp(lightingInfo1.colors[2], lightingInfo2.colors[2], t);
          }
        }
        LevelLighting.clouds.GetComponent<ParticleSystem>().emissionRate = Mathf.Lerp(lightingInfo1.singles[2], lightingInfo2.singles[2], t);
      }
      LevelLighting.sea.SetColor("_SpecularColor", LevelLighting.sun.GetComponent<Light>().color);
      if (LevelLighting.vision == ELightingVision.MILITARY)
      {
        LevelLighting.sea.SetColor("_BaseColor", LevelLighting.NIGHTVISION_MILITARY);
        LevelLighting.sea.SetColor("_ReflectionColor", LevelLighting.NIGHTVISION_MILITARY);
        RenderSettings.ambientSkyColor = LevelLighting.NIGHTVISION_MILITARY;
        RenderSettings.ambientEquatorColor = LevelLighting.NIGHTVISION_MILITARY;
        RenderSettings.ambientGroundColor = LevelLighting.NIGHTVISION_MILITARY;
        LevelLighting.skyboxSky = LevelLighting.NIGHTVISION_MILITARY;
        LevelLighting.skyboxEquator = LevelLighting.NIGHTVISION_MILITARY;
        LevelLighting.skyboxGround = LevelLighting.NIGHTVISION_MILITARY;
        LevelLighting.skyboxClouds = LevelLighting.NIGHTVISION_MILITARY;
      }
      else if (LevelLighting.vision == ELightingVision.CIVILIAN)
      {
        LevelLighting.sea.SetColor("_BaseColor", LevelLighting.NIGHTVISION_CIVILIAN);
        LevelLighting.sea.SetColor("_ReflectionColor", LevelLighting.NIGHTVISION_CIVILIAN);
        RenderSettings.ambientSkyColor = LevelLighting.NIGHTVISION_CIVILIAN;
        RenderSettings.ambientEquatorColor = LevelLighting.NIGHTVISION_CIVILIAN;
        RenderSettings.ambientGroundColor = LevelLighting.NIGHTVISION_CIVILIAN;
        LevelLighting.skyboxSky = LevelLighting.NIGHTVISION_CIVILIAN;
        LevelLighting.skyboxEquator = LevelLighting.NIGHTVISION_CIVILIAN;
        LevelLighting.skyboxGround = LevelLighting.NIGHTVISION_CIVILIAN;
        LevelLighting.skyboxClouds = LevelLighting.NIGHTVISION_CIVILIAN;
      }
      if ((Object) Camera.main != (Object) null)
      {
        if (LevelLighting.vision == ELightingVision.MILITARY)
        {
          GlobalFog component = Camera.main.GetComponent<GlobalFog>();
          if ((Object) component != (Object) null)
          {
            component.height = 0.0f;
            component.globalFogColor = LevelLighting.NIGHTVISION_MILITARY;
          }
        }
        else if (LevelLighting.vision == ELightingVision.CIVILIAN)
        {
          GlobalFog component = Camera.main.GetComponent<GlobalFog>();
          if ((Object) component != (Object) null)
          {
            component.height = 0.0f;
            component.globalFogColor = LevelLighting.NIGHTVISION_CIVILIAN;
          }
        }
        SunShafts component1 = Camera.main.GetComponent<SunShafts>();
        if ((Object) component1 != (Object) null)
        {
          component1.sunTransform = LevelLighting.sunFlare;
          component1.sunColor = LevelLighting.sun.GetComponent<Light>().color;
        }
        LevelLighting.reflection.clearColor = Camera.main.backgroundColor;
        if ((Object) Player.player != (Object) null)
        {
          Player.player.look.scopeCamera.backgroundColor = Camera.main.backgroundColor;
          GlobalFog component2 = Camera.main.GetComponent<GlobalFog>();
          if ((Object) component2 != (Object) null)
          {
            GlobalFog component3 = Player.player.look.scopeCamera.GetComponent<GlobalFog>();
            if ((Object) component3 != (Object) null)
            {
              component3.height = component2.height;
              component3.globalFogColor = component2.globalFogColor;
            }
          }
        }
      }
      LevelLighting.stars.rotation = Quaternion.Euler(Time.realtimeSinceStartup * 0.01f, Time.realtimeSinceStartup * 0.01f, Time.realtimeSinceStartup * 0.01f);
    }

    public static void load(ushort size)
    {
      LevelLighting.vision = ELightingVision.NONE;
      LevelLighting.isSea = false;
      LevelLighting.isSnow = false;
      if (!Dedicator.isDedicated)
      {
        LevelLighting.skybox = (Material) Object.Instantiate(Resources.Load("Level/Skybox"));
        RenderSettings.skybox = LevelLighting.skybox;
        LevelLighting.lighting = ((GameObject) Object.Instantiate(Resources.Load("Level/Lighting"))).transform;
        LevelLighting.lighting.name = "Lighting";
        LevelLighting.lighting.position = Vector3.zero;
        LevelLighting.lighting.rotation = Quaternion.identity;
        LevelLighting.lighting.parent = Level.level;
        LevelLighting.sun = LevelLighting.lighting.FindChild("Sun");
        LevelLighting.sunFlare = LevelLighting.sun.FindChild("Flare_Sun");
        LevelLighting.moonFlare = LevelLighting.sun.FindChild("Flare_Moon");
        LevelLighting.stars = LevelLighting.lighting.FindChild("Stars");
        LevelLighting._bubbles = LevelLighting.lighting.FindChild("Bubbles");
        LevelLighting._snow = LevelLighting.lighting.FindChild("Snow");
        LevelLighting._clouds = LevelLighting.lighting.FindChild("Clouds");
        LevelLighting.mist = LevelLighting.clouds.GetComponent<Renderer>().sharedMaterial;
        LevelLighting.moons = new Material[(int) LevelLighting.MOON_CYCLES];
        for (int index = 0; index < LevelLighting.moons.Length; ++index)
          LevelLighting.moons[index] = (Material) Resources.Load("Flares/Moon_" + (object) index);
        if (ReadWrite.fileExists(Level.info.path + "/Environment/Ambience.unity3d", false, false))
        {
          Bundle bundle = Bundles.getBundle(Level.info.path + "/Environment/Ambience.unity3d", false);
          LevelLighting._dayAudio = LevelLighting.lighting.FindChild("Day").GetComponent<AudioSource>();
          LevelLighting.dayAudio.clip = (AudioClip) bundle.load("Day");
          LevelLighting.dayAudio.Play();
          LevelLighting._nightAudio = LevelLighting.lighting.FindChild("Night").GetComponent<AudioSource>();
          LevelLighting.nightAudio.clip = (AudioClip) bundle.load("Night");
          LevelLighting.nightAudio.Play();
          LevelLighting._waterAudio = LevelLighting.lighting.FindChild("Water").GetComponent<AudioSource>();
          LevelLighting.waterAudio.clip = (AudioClip) bundle.load("Water");
          LevelLighting.waterAudio.Play();
          LevelLighting._windAudio = LevelLighting.lighting.FindChild("Wind").GetComponent<AudioSource>();
          LevelLighting.windAudio.clip = (AudioClip) bundle.load("Wind");
          LevelLighting.windAudio.Play();
          LevelLighting._belowAudio = LevelLighting.lighting.FindChild("Below").GetComponent<AudioSource>();
          LevelLighting.belowAudio.clip = (AudioClip) bundle.load("Below");
          LevelLighting.belowAudio.Play();
          bundle.unload();
        }
        LevelLighting.water = ((GameObject) Object.Instantiate(Resources.Load("Level/Water"))).transform;
        LevelLighting.water.name = "Water";
        LevelLighting.water.parent = Level.level;
        LevelLighting.water.transform.rotation = Quaternion.identity;
        LevelLighting.water.transform.localScale = new Vector3((float) ((int) size * 2) / 100f, 1f, (float) ((int) size * 2) / 100f);
        LevelLighting._sea = LevelLighting.water.FindChild("Tile").GetComponent<Renderer>().sharedMaterial;
        LevelLighting._reflection = LevelLighting.water.GetComponent<PlanarReflection>();
      }
      if (ReadWrite.fileExists(Level.info.path + "/Environment/Lighting.dat", false, false))
      {
        Block block = ReadWrite.readBlock(Level.info.path + "/Environment/Lighting.dat", false, false, (byte) 0);
        byte num = block.readByte();
        LevelLighting._azimuth = block.readSingle();
        LevelLighting._bias = block.readSingle();
        LevelLighting._fade = block.readSingle();
        LevelLighting._time = block.readSingle();
        LevelLighting.moon = block.readByte();
        if ((int) num >= 5)
        {
          LevelLighting._seaLevel = block.readSingle();
          LevelLighting._snowLevel = block.readSingle();
          LevelLighting._times = new LightingInfo[4];
          for (int index1 = 0; index1 < LevelLighting.times.Length; ++index1)
          {
            Color[] newColors = new Color[10];
            float[] newSingles = new float[4];
            if ((int) num >= 6)
            {
              for (int index2 = 0; index2 < newColors.Length; ++index2)
                newColors[index2] = block.readColor();
            }
            else
            {
              for (int index2 = 0; index2 < newColors.Length - 1; ++index2)
                newColors[index2] = block.readColor();
              newColors[9] = newColors[2];
            }
            for (int index2 = 0; index2 < newSingles.Length; ++index2)
              newSingles[index2] = block.readSingle();
            LightingInfo lightingInfo = new LightingInfo(newColors, newSingles);
            LevelLighting.times[index1] = lightingInfo;
          }
        }
        else
        {
          LevelLighting._times = new LightingInfo[4];
          for (int index = 0; index < LevelLighting.times.Length; ++index)
          {
            LightingInfo lightingInfo = new LightingInfo(new Color[9], new float[4]);
            LevelLighting.times[index] = lightingInfo;
          }
          LevelLighting.times[0].colors[3] = block.readColor();
          LevelLighting.times[1].colors[3] = block.readColor();
          LevelLighting.times[2].colors[3] = block.readColor();
          LevelLighting.times[3].colors[3] = block.readColor();
          LevelLighting.times[0].colors[4] = LevelLighting.times[0].colors[3];
          LevelLighting.times[1].colors[4] = LevelLighting.times[1].colors[3];
          LevelLighting.times[2].colors[4] = LevelLighting.times[2].colors[3];
          LevelLighting.times[3].colors[4] = LevelLighting.times[3].colors[3];
          LevelLighting.times[0].colors[5] = LevelLighting.times[0].colors[3];
          LevelLighting.times[1].colors[5] = LevelLighting.times[1].colors[3];
          LevelLighting.times[2].colors[5] = LevelLighting.times[2].colors[3];
          LevelLighting.times[3].colors[5] = LevelLighting.times[3].colors[3];
          LevelLighting.times[0].colors[6] = block.readColor();
          LevelLighting.times[1].colors[6] = block.readColor();
          LevelLighting.times[2].colors[6] = block.readColor();
          LevelLighting.times[3].colors[6] = block.readColor();
          LevelLighting.times[0].colors[7] = LevelLighting.times[0].colors[6];
          LevelLighting.times[1].colors[7] = LevelLighting.times[1].colors[6];
          LevelLighting.times[2].colors[7] = LevelLighting.times[2].colors[6];
          LevelLighting.times[3].colors[7] = LevelLighting.times[3].colors[6];
          LevelLighting.times[0].colors[8] = LevelLighting.times[0].colors[6];
          LevelLighting.times[1].colors[8] = LevelLighting.times[1].colors[6];
          LevelLighting.times[2].colors[8] = LevelLighting.times[2].colors[6];
          LevelLighting.times[3].colors[8] = LevelLighting.times[3].colors[6];
          LevelLighting.times[0].colors[2] = block.readColor();
          LevelLighting.times[1].colors[2] = block.readColor();
          LevelLighting.times[2].colors[2] = block.readColor();
          LevelLighting.times[3].colors[2] = block.readColor();
          LevelLighting.times[0].colors[0] = block.readColor();
          LevelLighting.times[1].colors[0] = block.readColor();
          LevelLighting.times[2].colors[0] = block.readColor();
          LevelLighting.times[3].colors[0] = block.readColor();
          LevelLighting.times[0].singles[0] = block.readSingle();
          LevelLighting.times[1].singles[0] = block.readSingle();
          LevelLighting.times[2].singles[0] = block.readSingle();
          LevelLighting.times[3].singles[0] = block.readSingle();
          LevelLighting.times[0].singles[1] = block.readSingle();
          LevelLighting.times[1].singles[1] = block.readSingle();
          LevelLighting.times[2].singles[1] = block.readSingle();
          LevelLighting.times[3].singles[1] = block.readSingle();
          LevelLighting.times[0].singles[2] = block.readSingle();
          LevelLighting.times[1].singles[2] = block.readSingle();
          LevelLighting.times[2].singles[2] = block.readSingle();
          LevelLighting.times[3].singles[2] = block.readSingle();
          LevelLighting.times[0].singles[3] = block.readSingle();
          LevelLighting.times[1].singles[3] = block.readSingle();
          LevelLighting.times[2].singles[3] = block.readSingle();
          LevelLighting.times[3].singles[3] = block.readSingle();
          LevelLighting._seaLevel = (int) num <= 2 ? block.readSingle() / 2f : block.readSingle();
          LevelLighting._snowLevel = (int) num <= 1 ? 0.0f : block.readSingle();
          LevelLighting.times[0].colors[1] = block.readColor();
          LevelLighting.times[1].colors[1] = block.readColor();
          LevelLighting.times[2].colors[1] = block.readColor();
          LevelLighting.times[3].colors[1] = block.readColor();
        }
        LevelLighting._hash = block.getHash();
      }
      else
      {
        LevelLighting._azimuth = 0.2f;
        LevelLighting._bias = 0.5f;
        LevelLighting._fade = 1f;
        LevelLighting._time = LevelLighting.bias / 2f;
        LevelLighting.moon = (byte) 0;
        LevelLighting._seaLevel = 1f;
        LevelLighting._snowLevel = 0.0f;
        LevelLighting._times = new LightingInfo[4];
        for (int index = 0; index < LevelLighting.times.Length; ++index)
        {
          LightingInfo lightingInfo = new LightingInfo(new Color[9], new float[4]);
          LevelLighting.times[index] = lightingInfo;
        }
        LevelLighting._hash = new byte[20];
      }
      LevelLighting._transition = (double) LevelLighting.bias >= 1.0 - (double) LevelLighting.bias ? (float) ((1.0 - (double) LevelLighting.bias) / 2.0) * LevelLighting.fade : LevelLighting.bias / 2f * LevelLighting.fade;
      LevelLighting.times[0].colors[1].a = 0.25f;
      LevelLighting.times[1].colors[1].a = 0.5f;
      LevelLighting.times[2].colors[1].a = 0.75f;
      LevelLighting.times[3].colors[1].a = 0.9f;
      LevelLighting.init = false;
    }

    public static void save()
    {
      Block block = new Block();
      block.writeByte(LevelLighting.SAVEDATA_VERSION);
      block.writeSingle(LevelLighting.azimuth);
      block.writeSingle(LevelLighting.bias);
      block.writeSingle(LevelLighting.fade);
      block.writeSingle(LevelLighting.time);
      block.writeByte(LevelLighting.moon);
      block.writeSingle(LevelLighting.seaLevel);
      block.writeSingle(LevelLighting.snowLevel);
      for (int index1 = 0; index1 < LevelLighting.times.Length; ++index1)
      {
        LightingInfo lightingInfo = LevelLighting.times[index1];
        for (int index2 = 0; index2 < lightingInfo.colors.Length; ++index2)
          block.writeColor(lightingInfo.colors[index2]);
        for (int index2 = 0; index2 < lightingInfo.singles.Length; ++index2)
          block.writeSingle(lightingInfo.singles[index2]);
      }
      ReadWrite.writeBlock(Level.info.path + "/Environment/Lighting.dat", false, false, block);
    }

    public static void updateClouds()
    {
      LevelLighting.clouds.GetComponent<ParticleSystem>().Stop();
      LevelLighting.clouds.GetComponent<ParticleSystem>().Play();
    }

    private static void updateSea()
    {
      if ((double) LevelLighting.seaLevel < 0.990000009536743)
      {
        LevelLighting.water.position = new Vector3(0.0f, LevelLighting.seaLevel * Level.TERRAIN, 0.0f);
        LevelLighting.bubbles.gameObject.SetActive(true);
        LevelLighting.water.gameObject.SetActive(true);
      }
      else
      {
        LevelLighting.bubbles.gameObject.SetActive(false);
        LevelLighting.water.gameObject.SetActive(false);
      }
      LevelLighting.bubbles.GetComponent<ParticleSystem>().Play();
    }

    public static void updateLocal(Vector3 point)
    {
      if (!LevelLighting.init)
      {
        LevelLighting.init = true;
        LevelLighting.updateSea();
        LevelLighting.updateLighting();
        LevelLighting.clouds.GetComponent<ParticleSystem>().Play();
        LevelLighting.bubbles.GetComponent<ParticleSystem>().Play();
        LevelLighting.snow.GetComponent<ParticleSystem>().Play();
      }
      LevelLighting.lighting.position = point;
      LevelLighting.skybox.SetColor("_SkyColor", LevelLighting.skyboxSky);
      LevelLighting.skybox.SetColor("_EquatorColor", LevelLighting.skyboxEquator);
      LevelLighting.skybox.SetColor("_GroundColor", LevelLighting.skyboxGround);
      LevelLighting.mist.SetColor("_RimColor", LevelLighting.skyboxClouds);
      if ((Object) Camera.main != (Object) null)
        Camera.main.backgroundColor = LevelLighting.skyboxSky;
      if ((double) LevelLighting.seaLevel < 0.990000009536743)
      {
        if ((double) point.y < (double) LevelLighting.seaLevel * (double) Level.TERRAIN)
        {
          LevelLighting.waterAudio.volume = 0.0f;
          LevelLighting.belowAudio.volume = 1f;
          RenderSettings.fogColor = LevelLighting.sea.GetColor("_BaseColor");
          RenderSettings.fogDensity = 0.075f;
          if ((Object) Camera.main != (Object) null)
            Camera.main.backgroundColor = RenderSettings.fogColor;
          if (!LevelLighting.isSea)
          {
            RenderSettings.skybox = (Material) null;
            if ((Object) Camera.main != (Object) null)
            {
              SunShafts component1 = Camera.main.GetComponent<SunShafts>();
              if ((Object) component1 != (Object) null)
                component1.enabled = false;
              GlobalFog component2 = Camera.main.GetComponent<GlobalFog>();
              if ((Object) component2 != (Object) null)
                component2.enabled = false;
              if ((Object) Player.player != (Object) null)
              {
                GlobalFog component3 = Player.player.animator.view.GetComponent<GlobalFog>();
                if ((Object) component3 != (Object) null)
                  component3.enabled = false;
              }
            }
          }
          LevelLighting.isSea = true;
        }
        else
        {
          if ((double) point.y < (double) LevelLighting.seaLevel * (double) Level.TERRAIN + 8.0)
          {
            LevelLighting.waterAudio.volume = Mathf.Lerp(0.0f, 0.25f, (float) (1.0 - ((double) point.y - (double) LevelLighting.seaLevel * (double) Level.TERRAIN) / 8.0));
            LevelLighting.belowAudio.volume = 0.0f;
          }
          else
          {
            LevelLighting.waterAudio.volume = 0.0f;
            LevelLighting.belowAudio.volume = 0.0f;
          }
          if (LevelLighting.isSea)
          {
            RenderSettings.skybox = LevelLighting.skybox;
            RenderSettings.fogDensity = 0.0f;
            if ((Object) Camera.main != (Object) null)
            {
              SunShafts component1 = Camera.main.GetComponent<SunShafts>();
              if ((Object) component1 != (Object) null)
                component1.enabled = GraphicsSettings.sunShafts;
              GlobalFog component2 = Camera.main.GetComponent<GlobalFog>();
              if ((Object) component2 != (Object) null)
                component2.enabled = GraphicsSettings.fog;
              if ((Object) Player.player != (Object) null)
              {
                GlobalFog component3 = Player.player.animator.view.GetComponent<GlobalFog>();
                if ((Object) component3 != (Object) null)
                  component3.enabled = GraphicsSettings.fog;
              }
            }
          }
          LevelLighting.isSea = false;
        }
      }
      else
      {
        LevelLighting.waterAudio.volume = 0.0f;
        LevelLighting.belowAudio.volume = 0.0f;
        if (LevelLighting.isSea)
        {
          RenderSettings.skybox = LevelLighting.skybox;
          RenderSettings.fogDensity = 0.0f;
          if ((Object) Camera.main != (Object) null)
          {
            SunShafts component1 = Camera.main.GetComponent<SunShafts>();
            if ((Object) component1 != (Object) null)
              component1.enabled = GraphicsSettings.sunShafts;
            GlobalFog component2 = Camera.main.GetComponent<GlobalFog>();
            if ((Object) component2 != (Object) null)
              component2.enabled = true;
            if ((Object) Player.player != (Object) null)
            {
              GlobalFog component3 = Player.player.animator.view.GetComponent<GlobalFog>();
              if ((Object) component3 != (Object) null)
                component3.enabled = true;
            }
          }
        }
        LevelLighting.isSea = false;
      }
      if ((double) LevelLighting.snowLevel > 0.00999999977648258 && !LevelLighting.isSea)
      {
        if ((double) point.y > (double) LevelLighting.snowLevel * (double) Level.TERRAIN)
        {
          LevelLighting.windAudio.volume = 1f;
          LevelLighting.snow.GetComponent<ParticleSystem>().emissionRate = 1024f;
          Color color1 = LevelLighting.sunFlare.GetComponent<Renderer>().material.GetColor("_Color");
          color1.a = 0.0f;
          LevelLighting.sunFlare.GetComponent<Renderer>().material.SetColor("_Color", color1);
          Color color2 = LevelLighting.moonFlare.GetComponent<Renderer>().material.GetColor("_Color");
          color2.a = 0.0f;
          LevelLighting.moonFlare.GetComponent<Renderer>().material.SetColor("_Color", color2);
          LevelLighting.skybox.SetColor("_SkyColor", LevelLighting.skyboxSky);
          LevelLighting.skybox.SetColor("_EquatorColor", LevelLighting.skyboxSky);
          LevelLighting.skybox.SetColor("_GroundColor", LevelLighting.skyboxSky);
          RenderSettings.fogColor = LevelLighting.skyboxSky;
          RenderSettings.fogDensity = 0.015f;
          if (!LevelLighting.isSnow && (Object) Camera.main != (Object) null)
          {
            SunShafts component1 = Camera.main.GetComponent<SunShafts>();
            if ((Object) component1 != (Object) null)
              component1.enabled = false;
            GlobalFog component2 = Camera.main.GetComponent<GlobalFog>();
            if ((Object) component2 != (Object) null)
              component2.enabled = false;
            if ((Object) Player.player != (Object) null)
            {
              GlobalFog component3 = Player.player.animator.view.GetComponent<GlobalFog>();
              if ((Object) component3 != (Object) null)
                component3.enabled = false;
            }
          }
          LevelLighting.isSnow = true;
        }
        else
        {
          if ((double) point.y > (double) LevelLighting.snowLevel * (double) Level.TERRAIN - 32.0)
          {
            LevelLighting.windAudio.volume = Mathf.Lerp(0.0f, 1f, (float) (1.0 - ((double) LevelLighting.snowLevel * (double) Level.TERRAIN - (double) point.y) / 32.0));
            LevelLighting.snow.GetComponent<ParticleSystem>().emissionRate = LevelLighting.windAudio.volume * 1024f;
            Color color1 = LevelLighting.sunFlare.GetComponent<Renderer>().material.GetColor("_Color");
            color1.a = 1f - LevelLighting.windAudio.volume;
            LevelLighting.sunFlare.GetComponent<Renderer>().material.SetColor("_Color", color1);
            Color color2 = LevelLighting.moonFlare.GetComponent<Renderer>().material.GetColor("_Color");
            color2.a = 1f - LevelLighting.windAudio.volume;
            LevelLighting.moonFlare.GetComponent<Renderer>().material.SetColor("_Color", color2);
            LevelLighting.skybox.SetColor("_SkyColor", LevelLighting.skyboxSky);
            LevelLighting.skybox.SetColor("_EquatorColor", Color.Lerp(LevelLighting.skyboxEquator, LevelLighting.skyboxSky, LevelLighting.windAudio.volume));
            LevelLighting.skybox.SetColor("_GroundColor", Color.Lerp(LevelLighting.skyboxGround, LevelLighting.skyboxSky, LevelLighting.windAudio.volume));
            RenderSettings.fogColor = LevelLighting.skyboxSky;
            RenderSettings.fogDensity = Mathf.Pow(LevelLighting.windAudio.volume, 3f) * 0.015f;
            if ((Object) Camera.main != (Object) null && (double) RenderSettings.fogDensity < 0.029899999499321)
            {
              SunShafts component1 = Camera.main.GetComponent<SunShafts>();
              if ((Object) component1 != (Object) null)
                component1.sunShaftIntensity = 1f - LevelLighting.windAudio.volume;
              GlobalFog component2 = Camera.main.GetComponent<GlobalFog>();
              if ((Object) component2 != (Object) null)
                component2.globalDensity = (float) (0.00499999988824129 * (1.0 - (double) LevelLighting.windAudio.volume));
              if ((Object) Player.player != (Object) null)
              {
                GlobalFog component3 = Player.player.animator.view.GetComponent<GlobalFog>();
                if ((Object) component3 != (Object) null)
                  component3.globalDensity = (float) (0.00499999988824129 * (1.0 - (double) LevelLighting.windAudio.volume));
              }
            }
          }
          else
          {
            LevelLighting.windAudio.volume = 0.0f;
            LevelLighting.snow.GetComponent<ParticleSystem>().emissionRate = 0.0f;
            Color color1 = LevelLighting.sunFlare.GetComponent<Renderer>().material.GetColor("_Color");
            color1.a = 1f;
            LevelLighting.sunFlare.GetComponent<Renderer>().material.SetColor("_Color", color1);
            Color color2 = LevelLighting.moonFlare.GetComponent<Renderer>().material.GetColor("_Color");
            color2.a = 1f;
            LevelLighting.moonFlare.GetComponent<Renderer>().material.SetColor("_Color", color2);
            if ((double) RenderSettings.fogDensity > 9.99999974737875E-05 && (Object) Camera.main != (Object) null)
            {
              SunShafts component1 = Camera.main.GetComponent<SunShafts>();
              if ((Object) component1 != (Object) null)
                component1.sunShaftIntensity = 1f;
              GlobalFog component2 = Camera.main.GetComponent<GlobalFog>();
              if ((Object) component2 != (Object) null)
                component2.globalDensity = 0.005f;
              if ((Object) Player.player != (Object) null)
              {
                GlobalFog component3 = Player.player.animator.view.GetComponent<GlobalFog>();
                if ((Object) component3 != (Object) null)
                  component3.globalDensity = 0.005f;
              }
            }
            RenderSettings.fogDensity = 0.0f;
          }
          if (LevelLighting.isSnow && (Object) Camera.main != (Object) null)
          {
            SunShafts component1 = Camera.main.GetComponent<SunShafts>();
            if ((Object) component1 != (Object) null)
              component1.enabled = GraphicsSettings.sunShafts;
            GlobalFog component2 = Camera.main.GetComponent<GlobalFog>();
            if ((Object) component2 != (Object) null)
              component2.enabled = true;
            if ((Object) Player.player != (Object) null)
            {
              GlobalFog component3 = Player.player.animator.view.GetComponent<GlobalFog>();
              if ((Object) component3 != (Object) null)
                component3.enabled = true;
            }
          }
          LevelLighting.isSnow = false;
        }
      }
      else
      {
        LevelLighting.windAudio.volume = 0.0f;
        LevelLighting.snow.GetComponent<ParticleSystem>().emissionRate = 0.0f;
        Color color1 = LevelLighting.sunFlare.GetComponent<Renderer>().material.GetColor("_Color");
        color1.a = 1f;
        LevelLighting.sunFlare.GetComponent<Renderer>().material.SetColor("_Color", color1);
        Color color2 = LevelLighting.moonFlare.GetComponent<Renderer>().material.GetColor("_Color");
        color2.a = 1f;
        LevelLighting.moonFlare.GetComponent<Renderer>().material.SetColor("_Color", color2);
        if (LevelLighting.isSnow)
        {
          RenderSettings.fogDensity = 0.0f;
          if ((Object) Camera.main != (Object) null)
          {
            SunShafts component1 = Camera.main.GetComponent<SunShafts>();
            if ((Object) component1 != (Object) null)
            {
              component1.sunShaftIntensity = 1f;
              component1.enabled = GraphicsSettings.sunShafts;
            }
            GlobalFog component2 = Camera.main.GetComponent<GlobalFog>();
            if ((Object) component2 != (Object) null)
            {
              component2.globalDensity = 0.005f;
              component2.enabled = true;
            }
            if ((Object) Player.player != (Object) null)
            {
              GlobalFog component3 = Player.player.animator.view.GetComponent<GlobalFog>();
              if ((Object) component3 != (Object) null)
              {
                component3.globalDensity = 0.005f;
                component3.enabled = true;
              }
            }
          }
        }
        LevelLighting.isSnow = false;
      }
      LevelLighting.dayAudio.volume = Mathf.Lerp(LevelLighting.dayAudio.volume, (float) ((double) LevelLighting.dayVolume * (1.0 - (double) LevelLighting.waterAudio.volume * 4.0) * (1.0 - (double) LevelLighting.belowAudio.volume) * (1.0 - (double) LevelLighting.windAudio.volume)), 0.5f * Time.deltaTime);
      LevelLighting.nightAudio.volume = Mathf.Lerp(LevelLighting.nightAudio.volume, (float) ((double) LevelLighting.nightVolume * (1.0 - (double) LevelLighting.waterAudio.volume * 4.0) * (1.0 - (double) LevelLighting.belowAudio.volume) * (1.0 - (double) LevelLighting.windAudio.volume)), 0.5f * Time.deltaTime);
      LevelLighting.snow.rotation = Quaternion.Slerp(LevelLighting.snow.rotation, Quaternion.Euler(45f, LevelLighting.wind, 0.0f), 0.5f * Time.deltaTime);
      LevelLighting.snow.position = point + LevelLighting.snow.forward * -32f;
      point.y = Mathf.Min(point.y - 16f, (float) ((double) LevelLighting.seaLevel * (double) Level.TERRAIN - 32.0));
      LevelLighting.bubbles.position = point;
    }

    private static void updateStars(float alpha)
    {
      Color color = LevelLighting.stars.GetComponent<Renderer>().material.GetColor("_Color");
      color.a = alpha;
      LevelLighting.stars.GetComponent<Renderer>().material.SetColor("_Color", color);
    }
  }
}
