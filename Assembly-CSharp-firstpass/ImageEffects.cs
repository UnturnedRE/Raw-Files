// Decompiled with JetBrains decompiler
// Type: ImageEffects
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

[AddComponentMenu("")]
public class ImageEffects
{
  public static void RenderDistortion(Material material, RenderTexture source, RenderTexture destination, float angle, Vector2 center, Vector2 radius)
  {
    // ISSUE: unable to decompile the method.
  }

  [Obsolete("Use Graphics.Blit(source,dest) instead")]
  public static void Blit(RenderTexture source, RenderTexture dest)
  {
    Graphics.Blit((Texture) source, dest);
  }

  [Obsolete("Use Graphics.Blit(source, destination, material) instead")]
  public static void BlitWithMaterial(Material material, RenderTexture source, RenderTexture dest)
  {
    Graphics.Blit((Texture) source, dest, material);
  }
}
