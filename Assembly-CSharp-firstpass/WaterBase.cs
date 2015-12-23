// Decompiled with JetBrains decompiler
// Type: WaterBase
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

[ExecuteInEditMode]
public class WaterBase : MonoBehaviour
{
  public WaterQuality waterQuality = WaterQuality.High;
  public bool edgeBlend = true;
  public Material sharedMaterial;

  public void UpdateShader()
  {
    this.sharedMaterial.shader.maximumLOD = this.waterQuality <= WaterQuality.Medium ? (this.waterQuality <= WaterQuality.Low ? 201 : 301) : 501;
    if (!SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
      this.edgeBlend = false;
    if (this.edgeBlend)
    {
      Shader.EnableKeyword("WATER_EDGEBLEND_ON");
      Shader.DisableKeyword("WATER_EDGEBLEND_OFF");
      if (!(bool) ((Object) Camera.main))
        return;
      Camera.main.depthTextureMode |= DepthTextureMode.Depth;
    }
    else
    {
      Shader.EnableKeyword("WATER_EDGEBLEND_OFF");
      Shader.DisableKeyword("WATER_EDGEBLEND_ON");
    }
  }

  public void WaterTileBeingRendered(Transform tr, Camera currentCam)
  {
    if (!(bool) ((Object) currentCam) || !this.edgeBlend)
      return;
    currentCam.depthTextureMode |= DepthTextureMode.Depth;
  }

  public void Update()
  {
    if (!(bool) ((Object) this.sharedMaterial))
      return;
    this.UpdateShader();
  }
}
