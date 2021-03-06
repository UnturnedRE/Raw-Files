﻿// Decompiled with JetBrains decompiler
// Type: Water
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Water : MonoBehaviour
{
  public Water.WaterMode m_WaterMode = Water.WaterMode.Refractive;
  public bool m_DisablePixelLights = true;
  public int m_TextureSize = 256;
  public float m_ClipPlaneOffset = 0.07f;
  public LayerMask m_ReflectLayers = (LayerMask) -1;
  public LayerMask m_RefractLayers = (LayerMask) -1;
  private Dictionary<Camera, Camera> m_ReflectionCameras = new Dictionary<Camera, Camera>();
  private Dictionary<Camera, Camera> m_RefractionCameras = new Dictionary<Camera, Camera>();
  private Water.WaterMode m_HardwareWaterSupport = Water.WaterMode.Refractive;
  private RenderTexture m_ReflectionTexture;
  private RenderTexture m_RefractionTexture;
  private int m_OldReflectionTextureSize;
  private int m_OldRefractionTextureSize;
  private static bool s_InsideWater;

  public void OnWillRenderObject()
  {
    if (!this.enabled || !(bool) ((UnityEngine.Object) this.GetComponent<Renderer>()) || (!(bool) ((UnityEngine.Object) this.GetComponent<Renderer>().sharedMaterial) || !this.GetComponent<Renderer>().enabled))
      return;
    Camera current = Camera.current;
    if (!(bool) ((UnityEngine.Object) current) || Water.s_InsideWater)
      return;
    Water.s_InsideWater = true;
    this.m_HardwareWaterSupport = this.FindHardwareWaterSupport();
    Water.WaterMode waterMode = this.GetWaterMode();
    Camera reflectionCamera;
    Camera refractionCamera;
    this.CreateWaterObjects(current, out reflectionCamera, out refractionCamera);
    Vector3 position1 = this.transform.position;
    Vector3 up = this.transform.up;
    int pixelLightCount = QualitySettings.pixelLightCount;
    if (this.m_DisablePixelLights)
      QualitySettings.pixelLightCount = 0;
    this.UpdateCameraModes(current, reflectionCamera);
    this.UpdateCameraModes(current, refractionCamera);
    if (waterMode >= Water.WaterMode.Reflective)
    {
      float w = -Vector3.Dot(up, position1) - this.m_ClipPlaneOffset;
      Vector4 plane = new Vector4(up.x, up.y, up.z, w);
      Matrix4x4 zero = Matrix4x4.zero;
      Water.CalculateReflectionMatrix(ref zero, plane);
      Vector3 position2 = current.transform.position;
      Vector3 vector3 = zero.MultiplyPoint(position2);
      reflectionCamera.worldToCameraMatrix = current.worldToCameraMatrix * zero;
      Vector4 clipPlane = this.CameraSpacePlane(reflectionCamera, position1, up, 1f);
      reflectionCamera.projectionMatrix = current.CalculateObliqueMatrix(clipPlane);
      reflectionCamera.cullingMask = -17 & this.m_ReflectLayers.value;
      reflectionCamera.targetTexture = this.m_ReflectionTexture;
      GL.SetRevertBackfacing(true);
      reflectionCamera.transform.position = vector3;
      Vector3 eulerAngles = current.transform.eulerAngles;
      reflectionCamera.transform.eulerAngles = new Vector3(-eulerAngles.x, eulerAngles.y, eulerAngles.z);
      reflectionCamera.Render();
      reflectionCamera.transform.position = position2;
      GL.SetRevertBackfacing(false);
      this.GetComponent<Renderer>().sharedMaterial.SetTexture("_ReflectionTex", (Texture) this.m_ReflectionTexture);
    }
    if (waterMode >= Water.WaterMode.Refractive)
    {
      refractionCamera.worldToCameraMatrix = current.worldToCameraMatrix;
      Vector4 clipPlane = this.CameraSpacePlane(refractionCamera, position1, up, -1f);
      refractionCamera.projectionMatrix = current.CalculateObliqueMatrix(clipPlane);
      refractionCamera.cullingMask = -17 & this.m_RefractLayers.value;
      refractionCamera.targetTexture = this.m_RefractionTexture;
      refractionCamera.transform.position = current.transform.position;
      refractionCamera.transform.rotation = current.transform.rotation;
      refractionCamera.Render();
      this.GetComponent<Renderer>().sharedMaterial.SetTexture("_RefractionTex", (Texture) this.m_RefractionTexture);
    }
    if (this.m_DisablePixelLights)
      QualitySettings.pixelLightCount = pixelLightCount;
    switch (waterMode)
    {
      case Water.WaterMode.Simple:
        Shader.EnableKeyword("WATER_SIMPLE");
        Shader.DisableKeyword("WATER_REFLECTIVE");
        Shader.DisableKeyword("WATER_REFRACTIVE");
        break;
      case Water.WaterMode.Reflective:
        Shader.DisableKeyword("WATER_SIMPLE");
        Shader.EnableKeyword("WATER_REFLECTIVE");
        Shader.DisableKeyword("WATER_REFRACTIVE");
        break;
      case Water.WaterMode.Refractive:
        Shader.DisableKeyword("WATER_SIMPLE");
        Shader.DisableKeyword("WATER_REFLECTIVE");
        Shader.EnableKeyword("WATER_REFRACTIVE");
        break;
    }
    Water.s_InsideWater = false;
  }

  private void OnDisable()
  {
    if ((bool) ((UnityEngine.Object) this.m_ReflectionTexture))
    {
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_ReflectionTexture);
      this.m_ReflectionTexture = (RenderTexture) null;
    }
    if ((bool) ((UnityEngine.Object) this.m_RefractionTexture))
    {
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_RefractionTexture);
      this.m_RefractionTexture = (RenderTexture) null;
    }
    using (Dictionary<Camera, Camera>.Enumerator enumerator = this.m_ReflectionCameras.GetEnumerator())
    {
      while (enumerator.MoveNext())
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) enumerator.Current.Value.gameObject);
    }
    this.m_ReflectionCameras.Clear();
    using (Dictionary<Camera, Camera>.Enumerator enumerator = this.m_RefractionCameras.GetEnumerator())
    {
      while (enumerator.MoveNext())
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) enumerator.Current.Value.gameObject);
    }
    this.m_RefractionCameras.Clear();
  }

  private void Update()
  {
    if (!(bool) ((UnityEngine.Object) this.GetComponent<Renderer>()))
      return;
    Material sharedMaterial = this.GetComponent<Renderer>().sharedMaterial;
    if (!(bool) ((UnityEngine.Object) sharedMaterial))
      return;
    Vector4 vector1 = sharedMaterial.GetVector("WaveSpeed");
    float @float = sharedMaterial.GetFloat("_WaveScale");
    Vector4 vector2 = new Vector4(@float, @float, @float * 0.4f, @float * 0.45f);
    double num = (double) Time.timeSinceLevelLoad / 20.0;
    Vector4 vector3 = new Vector4((float) Math.IEEERemainder((double) vector1.x * (double) vector2.x * num, 1.0), (float) Math.IEEERemainder((double) vector1.y * (double) vector2.y * num, 1.0), (float) Math.IEEERemainder((double) vector1.z * (double) vector2.z * num, 1.0), (float) Math.IEEERemainder((double) vector1.w * (double) vector2.w * num, 1.0));
    sharedMaterial.SetVector("_WaveOffset", vector3);
    sharedMaterial.SetVector("_WaveScale4", vector2);
    Vector3 size = this.GetComponent<Renderer>().bounds.size;
    Vector3 s = new Vector3(size.x * vector2.x, size.z * vector2.y, 1f);
    Matrix4x4 matrix1 = Matrix4x4.TRS(new Vector3(vector3.x, vector3.y, 0.0f), Quaternion.identity, s);
    sharedMaterial.SetMatrix("_WaveMatrix", matrix1);
    s = new Vector3(size.x * vector2.z, size.z * vector2.w, 1f);
    Matrix4x4 matrix2 = Matrix4x4.TRS(new Vector3(vector3.z, vector3.w, 0.0f), Quaternion.identity, s);
    sharedMaterial.SetMatrix("_WaveMatrix2", matrix2);
  }

  private void UpdateCameraModes(Camera src, Camera dest)
  {
    if ((UnityEngine.Object) dest == (UnityEngine.Object) null)
      return;
    dest.clearFlags = src.clearFlags;
    dest.backgroundColor = src.backgroundColor;
    if (src.clearFlags == CameraClearFlags.Skybox)
    {
      Skybox skybox1 = src.GetComponent(typeof (Skybox)) as Skybox;
      Skybox skybox2 = dest.GetComponent(typeof (Skybox)) as Skybox;
      if (!(bool) ((UnityEngine.Object) skybox1) || !(bool) ((UnityEngine.Object) skybox1.material))
      {
        skybox2.enabled = false;
      }
      else
      {
        skybox2.enabled = true;
        skybox2.material = skybox1.material;
      }
    }
    dest.layerCullDistances = src.layerCullDistances;
    dest.layerCullSpherical = src.layerCullSpherical;
    dest.farClipPlane = src.farClipPlane;
    dest.nearClipPlane = src.nearClipPlane;
    dest.orthographic = src.orthographic;
    dest.fieldOfView = src.fieldOfView;
    dest.aspect = src.aspect;
    dest.orthographicSize = src.orthographicSize;
    Debug.Log((object) "updating");
  }

  private void CreateWaterObjects(Camera currentCamera, out Camera reflectionCamera, out Camera refractionCamera)
  {
    Water.WaterMode waterMode = this.GetWaterMode();
    reflectionCamera = (Camera) null;
    refractionCamera = (Camera) null;
    if (waterMode >= Water.WaterMode.Reflective)
    {
      if (!(bool) ((UnityEngine.Object) this.m_ReflectionTexture) || this.m_OldReflectionTextureSize != this.m_TextureSize)
      {
        if ((bool) ((UnityEngine.Object) this.m_ReflectionTexture))
          UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_ReflectionTexture);
        this.m_ReflectionTexture = new RenderTexture(this.m_TextureSize, this.m_TextureSize, 16);
        this.m_ReflectionTexture.name = "__WaterReflection" + (object) this.GetInstanceID();
        this.m_ReflectionTexture.isPowerOfTwo = true;
        this.m_ReflectionTexture.hideFlags = HideFlags.DontSave;
        this.m_OldReflectionTextureSize = this.m_TextureSize;
      }
      this.m_ReflectionCameras.TryGetValue(currentCamera, out reflectionCamera);
      if (!(bool) ((UnityEngine.Object) reflectionCamera))
      {
        GameObject gameObject = new GameObject(string.Concat(new object[4]
        {
          (object) "Water Refl Camera id",
          (object) this.GetInstanceID(),
          (object) " for ",
          (object) currentCamera.GetInstanceID()
        }), new System.Type[2]
        {
          typeof (Camera),
          typeof (Skybox)
        });
        reflectionCamera = gameObject.GetComponent<Camera>();
        reflectionCamera.enabled = false;
        reflectionCamera.transform.position = this.transform.position;
        reflectionCamera.transform.rotation = this.transform.rotation;
        reflectionCamera.gameObject.AddComponent<FlareLayer>();
        gameObject.hideFlags = HideFlags.HideAndDontSave;
        this.m_ReflectionCameras[currentCamera] = reflectionCamera;
      }
    }
    if (waterMode < Water.WaterMode.Refractive)
      return;
    if (!(bool) ((UnityEngine.Object) this.m_RefractionTexture) || this.m_OldRefractionTextureSize != this.m_TextureSize)
    {
      if ((bool) ((UnityEngine.Object) this.m_RefractionTexture))
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_RefractionTexture);
      this.m_RefractionTexture = new RenderTexture(this.m_TextureSize, this.m_TextureSize, 16);
      this.m_RefractionTexture.name = "__WaterRefraction" + (object) this.GetInstanceID();
      this.m_RefractionTexture.isPowerOfTwo = true;
      this.m_RefractionTexture.hideFlags = HideFlags.DontSave;
      this.m_OldRefractionTextureSize = this.m_TextureSize;
    }
    this.m_RefractionCameras.TryGetValue(currentCamera, out refractionCamera);
    if ((bool) ((UnityEngine.Object) refractionCamera))
      return;
    GameObject gameObject1 = new GameObject(string.Concat(new object[4]
    {
      (object) "Water Refr Camera id",
      (object) this.GetInstanceID(),
      (object) " for ",
      (object) currentCamera.GetInstanceID()
    }), new System.Type[2]
    {
      typeof (Camera),
      typeof (Skybox)
    });
    refractionCamera = gameObject1.GetComponent<Camera>();
    refractionCamera.enabled = false;
    refractionCamera.transform.position = this.transform.position;
    refractionCamera.transform.rotation = this.transform.rotation;
    refractionCamera.gameObject.AddComponent<FlareLayer>();
    gameObject1.hideFlags = HideFlags.HideAndDontSave;
    this.m_RefractionCameras[currentCamera] = refractionCamera;
  }

  private Water.WaterMode GetWaterMode()
  {
    if (this.m_HardwareWaterSupport < this.m_WaterMode)
      return this.m_HardwareWaterSupport;
    return this.m_WaterMode;
  }

  private Water.WaterMode FindHardwareWaterSupport()
  {
    if (!SystemInfo.supportsRenderTextures || !(bool) ((UnityEngine.Object) this.GetComponent<Renderer>()))
      return Water.WaterMode.Simple;
    Material sharedMaterial = this.GetComponent<Renderer>().sharedMaterial;
    if (!(bool) ((UnityEngine.Object) sharedMaterial))
      return Water.WaterMode.Simple;
    string tag = sharedMaterial.GetTag("WATERMODE", false);
    if (tag == "Refractive")
      return Water.WaterMode.Refractive;
    return tag == "Reflective" ? Water.WaterMode.Reflective : Water.WaterMode.Simple;
  }

  private static float sgn(float a)
  {
    if ((double) a > 0.0)
      return 1f;
    return (double) a < 0.0 ? -1f : 0.0f;
  }

  private Vector4 CameraSpacePlane(Camera cam, Vector3 pos, Vector3 normal, float sideSign)
  {
    Vector3 v = pos + normal * this.m_ClipPlaneOffset;
    Matrix4x4 worldToCameraMatrix = cam.worldToCameraMatrix;
    Vector3 lhs = worldToCameraMatrix.MultiplyPoint(v);
    Vector3 rhs = worldToCameraMatrix.MultiplyVector(normal).normalized * sideSign;
    return new Vector4(rhs.x, rhs.y, rhs.z, -Vector3.Dot(lhs, rhs));
  }

  private static void CalculateReflectionMatrix(ref Matrix4x4 reflectionMat, Vector4 plane)
  {
    reflectionMat.m00 = (float) (1.0 - 2.0 * (double) plane[0] * (double) plane[0]);
    reflectionMat.m01 = -2f * plane[0] * plane[1];
    reflectionMat.m02 = -2f * plane[0] * plane[2];
    reflectionMat.m03 = -2f * plane[3] * plane[0];
    reflectionMat.m10 = -2f * plane[1] * plane[0];
    reflectionMat.m11 = (float) (1.0 - 2.0 * (double) plane[1] * (double) plane[1]);
    reflectionMat.m12 = -2f * plane[1] * plane[2];
    reflectionMat.m13 = -2f * plane[3] * plane[1];
    reflectionMat.m20 = -2f * plane[2] * plane[0];
    reflectionMat.m21 = -2f * plane[2] * plane[1];
    reflectionMat.m22 = (float) (1.0 - 2.0 * (double) plane[2] * (double) plane[2]);
    reflectionMat.m23 = -2f * plane[3] * plane[2];
    reflectionMat.m30 = 0.0f;
    reflectionMat.m31 = 0.0f;
    reflectionMat.m32 = 0.0f;
    reflectionMat.m33 = 1f;
  }

  public enum WaterMode
  {
    Simple,
    Reflective,
    Refractive,
  }
}
