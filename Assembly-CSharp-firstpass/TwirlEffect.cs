// Decompiled with JetBrains decompiler
// Type: TwirlEffect
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

[AddComponentMenu("Image Effects/Displacement/Twirl")]
[ExecuteInEditMode]
public class TwirlEffect : ImageEffectBase
{
  public Vector2 radius = new Vector2(0.3f, 0.3f);
  public float angle = 50f;
  public Vector2 center = new Vector2(0.5f, 0.5f);

  private void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    ImageEffects.RenderDistortion(this.material, source, destination, this.angle, this.center, this.radius);
  }
}
