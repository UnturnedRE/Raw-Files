// Decompiled with JetBrains decompiler
// Type: BlurEffect
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

[AddComponentMenu("Image Effects/Blur/Blur")]
[ExecuteInEditMode]
public class BlurEffect : MonoBehaviour
{
  public int iterations = 3;
  public float blurSpread = 0.6f;
  public Shader blurShader;
  private static Material m_Material;

  protected Material material
  {
    get
    {
      if ((Object) BlurEffect.m_Material == (Object) null)
      {
        BlurEffect.m_Material = new Material(this.blurShader);
        BlurEffect.m_Material.hideFlags = HideFlags.DontSave;
      }
      return BlurEffect.m_Material;
    }
  }

  protected void OnDisable()
  {
    if (!(bool) ((Object) BlurEffect.m_Material))
      return;
    Object.DestroyImmediate((Object) BlurEffect.m_Material);
  }

  protected void Start()
  {
    if (!SystemInfo.supportsImageEffects)
    {
      this.enabled = false;
    }
    else
    {
      if ((bool) ((Object) this.blurShader) && this.material.shader.isSupported)
        return;
      this.enabled = false;
    }
  }

  public void FourTapCone(RenderTexture source, RenderTexture dest, int iteration)
  {
    float num = (float) (0.5 + (double) iteration * (double) this.blurSpread);
    Graphics.BlitMultiTap((Texture) source, dest, this.material, new Vector2(-num, -num), new Vector2(-num, num), new Vector2(num, num), new Vector2(num, -num));
  }

  private void DownSample4x(RenderTexture source, RenderTexture dest)
  {
    float num = 1f;
    Graphics.BlitMultiTap((Texture) source, dest, this.material, new Vector2(-num, -num), new Vector2(-num, num), new Vector2(num, num), new Vector2(num, -num));
  }

  private void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    int width = source.width / 4;
    int height = source.height / 4;
    RenderTexture renderTexture = RenderTexture.GetTemporary(width, height, 0);
    this.DownSample4x(source, renderTexture);
    for (int iteration = 0; iteration < this.iterations; ++iteration)
    {
      RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0);
      this.FourTapCone(renderTexture, temporary, iteration);
      RenderTexture.ReleaseTemporary(renderTexture);
      renderTexture = temporary;
    }
    Graphics.Blit((Texture) renderTexture, destination);
    RenderTexture.ReleaseTemporary(renderTexture);
  }
}
