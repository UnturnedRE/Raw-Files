// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.HumanClothing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class HumanClothing
  {
    protected Texture2D _texture;
    protected Texture2D _emission;
    protected Texture2D _metallic;
    public Texture2D face;
    public Texture2D faceEmission;
    public Texture2D faceMetallic;
    public Texture2D shirt;
    public Texture2D shirtEmission;
    public Texture2D shirtMetallic;
    public Texture2D pants;
    public Texture2D pantsEmission;
    public Texture2D pantsMetallic;
    public Color skin;

    public Texture2D texture
    {
      get
      {
        return this._texture;
      }
    }

    public Texture2D emission
    {
      get
      {
        return this._emission;
      }
    }

    public Texture2D metallic
    {
      get
      {
        return this._metallic;
      }
    }

    public HumanClothing()
    {
      this._texture = new Texture2D(128, 128, TextureFormat.RGBA32, true);
      this.texture.name = "Texture";
      this.texture.hideFlags = HideFlags.HideAndDontSave;
      this.texture.filterMode = FilterMode.Point;
      this._emission = new Texture2D(128, 128, TextureFormat.RGBA32, true);
      this.emission.name = "Emission";
      this.emission.hideFlags = HideFlags.HideAndDontSave;
      this.emission.filterMode = FilterMode.Point;
      this._metallic = new Texture2D(128, 128, TextureFormat.RGBA32, true);
      this.metallic.name = "Metallic";
      this.metallic.hideFlags = HideFlags.HideAndDontSave;
      this.metallic.filterMode = FilterMode.Point;
    }

    public void apply()
    {
      for (int x = 0; x < this.texture.width; ++x)
      {
        for (int y = 0; y < this.texture.height; ++y)
          this.texture.SetPixel(x, y, this.skin);
      }
      if ((Object) this.face != (Object) null)
      {
        for (int x = 0; x < this.face.width; ++x)
        {
          for (int y = 0; y < this.face.height; ++y)
          {
            if ((double) this.face.GetPixel(x, y).a > 0.0)
              this.texture.SetPixel(this.texture.width - 32 + x, this.texture.height - 16 + y, this.face.GetPixel(x, y));
          }
        }
      }
      if ((Object) this.shirt != (Object) null)
      {
        for (int x = 0; x < this.shirt.width; ++x)
        {
          for (int y = 0; y < this.shirt.height; ++y)
          {
            if ((double) this.shirt.GetPixel(x, y).a > 0.0)
              this.texture.SetPixel(x, y, this.shirt.GetPixel(x, y));
          }
        }
      }
      if ((Object) this.pants != (Object) null)
      {
        for (int x = 0; x < this.pants.width; ++x)
        {
          for (int y = 0; y < this.pants.height; ++y)
          {
            if ((double) this.pants.GetPixel(x, y).a > 0.0)
              this.texture.SetPixel(x, y, this.pants.GetPixel(x, y));
          }
        }
      }
      this.texture.Apply();
      for (int x = 0; x < this.emission.width; ++x)
      {
        for (int y = 0; y < this.emission.height; ++y)
          this.emission.SetPixel(x, y, Color.black);
      }
      if ((Object) this.faceEmission != (Object) null)
      {
        for (int x = 0; x < this.faceEmission.width; ++x)
        {
          for (int y = 0; y < this.faceEmission.height; ++y)
          {
            if ((double) this.faceEmission.GetPixel(x, y).a > 0.0)
              this.emission.SetPixel(this.emission.width - 32 + x, this.emission.height - 16 + y, this.faceEmission.GetPixel(x, y));
          }
        }
      }
      if ((Object) this.shirtEmission != (Object) null)
      {
        for (int x = 0; x < this.shirtEmission.width; ++x)
        {
          for (int y = 0; y < this.shirtEmission.height; ++y)
          {
            if ((double) this.shirtEmission.GetPixel(x, y).a > 0.0)
              this.emission.SetPixel(x, y, this.shirtEmission.GetPixel(x, y));
          }
        }
      }
      if ((Object) this.pantsEmission != (Object) null)
      {
        for (int x = 0; x < this.pantsEmission.width; ++x)
        {
          for (int y = 0; y < this.pantsEmission.height; ++y)
          {
            if ((double) this.pantsEmission.GetPixel(x, y).a > 0.0)
              this.emission.SetPixel(x, y, this.pantsEmission.GetPixel(x, y));
          }
        }
      }
      this.emission.Apply();
      for (int x = 0; x < this.metallic.width; ++x)
      {
        for (int y = 0; y < this.metallic.height; ++y)
          this.metallic.SetPixel(x, y, new Color(0.0f, 0.0f, 0.0f, 0.0f));
      }
      if ((Object) this.faceMetallic != (Object) null)
      {
        for (int x = 0; x < this.faceMetallic.width; ++x)
        {
          for (int y = 0; y < this.faceMetallic.height; ++y)
          {
            if ((double) this.faceMetallic.GetPixel(x, y).a > 0.0)
              this.metallic.SetPixel(this.metallic.width - 32 + x, this.metallic.height - 16 + y, this.faceMetallic.GetPixel(x, y));
          }
        }
      }
      if ((Object) this.shirtMetallic != (Object) null)
      {
        for (int x = 0; x < this.shirtMetallic.width; ++x)
        {
          for (int y = 0; y < this.shirtMetallic.height; ++y)
          {
            if ((double) this.shirtMetallic.GetPixel(x, y).a > 0.0)
              this.metallic.SetPixel(x, y, this.shirtMetallic.GetPixel(x, y));
          }
        }
      }
      if ((Object) this.pantsMetallic != (Object) null)
      {
        for (int x = 0; x < this.pantsMetallic.width; ++x)
        {
          for (int y = 0; y < this.pantsMetallic.height; ++y)
          {
            if ((double) this.pantsMetallic.GetPixel(x, y).a > 0.0)
              this.metallic.SetPixel(x, y, this.pantsMetallic.GetPixel(x, y));
          }
        }
      }
      this.metallic.Apply();
    }
  }
}
