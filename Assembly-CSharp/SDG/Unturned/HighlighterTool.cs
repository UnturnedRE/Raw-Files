// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.HighlighterTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using HighlightingSystem;
using UnityEngine;

namespace SDG.Unturned
{
  public class HighlighterTool
  {
    public static void color(Transform target, Color color)
    {
      if ((Object) target.GetComponent<Renderer>() != (Object) null)
      {
        target.GetComponent<Renderer>().material.color = color;
      }
      else
      {
        if (!((Object) target.GetComponent<LODGroup>() != (Object) null))
          return;
        for (int index = 0; index < 4; ++index)
        {
          Transform child = target.FindChild("Model_" + (object) index);
          if ((Object) child == (Object) null)
            break;
          if ((Object) child.GetComponent<Renderer>() != (Object) null)
            child.GetComponent<Renderer>().material.color = color;
        }
      }
    }

    public static void blend(Transform target, float blend, Color color)
    {
      if ((Object) target.GetComponent<Renderer>() != (Object) null)
      {
        target.GetComponent<Renderer>().material.color = Color.Lerp(target.GetComponent<Renderer>().material.color, color, blend);
      }
      else
      {
        if (!((Object) target.GetComponent<LODGroup>() != (Object) null))
          return;
        for (int index = 0; index < 4; ++index)
        {
          Transform child = target.FindChild("Model_" + (object) index);
          if ((Object) child == (Object) null)
            break;
          if ((Object) child.GetComponent<Renderer>() != (Object) null)
            child.GetComponent<Renderer>().material.color = Color.Lerp(child.GetComponent<Renderer>().material.color, color, blend);
        }
      }
    }

    public static void paint(Transform target, Color color)
    {
      Material material = (Material) null;
      if ((Object) target.GetComponent<Renderer>() != (Object) null)
        material = target.GetComponent<Renderer>().material;
      else if ((Object) target.GetComponent<LODGroup>() != (Object) null)
        material = target.FindChild("Model_0").GetComponent<Renderer>().material;
      Texture2D texture2D = new Texture2D(material.mainTexture.width, material.mainTexture.height, TextureFormat.RGBA32, true);
      texture2D.name = "Texture";
      texture2D.hideFlags = HideFlags.HideAndDontSave;
      texture2D.filterMode = FilterMode.Point;
      texture2D.SetPixels(((Texture2D) material.mainTexture).GetPixels());
      texture2D.SetPixel(0, texture2D.height - 1, color);
      texture2D.Apply();
      material.mainTexture = (Texture) texture2D;
      if (!((Object) target.GetComponent<LODGroup>() != (Object) null))
        return;
      for (int index = 1; index < 4; ++index)
      {
        Transform child = target.FindChild("Model_" + (object) index);
        if ((Object) child == (Object) null)
          break;
        if ((Object) child.GetComponent<Renderer>() != (Object) null)
          child.GetComponent<Renderer>().material.mainTexture = (Texture) texture2D;
      }
    }

    public static void help(Transform target, bool isValid)
    {
      Material material = !isValid ? (Material) Resources.Load("Materials/Bad") : (Material) Resources.Load("Materials/Good");
      if ((Object) target.GetComponent<Renderer>() != (Object) null)
      {
        target.GetComponent<Renderer>().material = material;
      }
      else
      {
        if (!((Object) target.GetComponent<LODGroup>() != (Object) null))
          return;
        for (int index = 0; index < 4; ++index)
        {
          Transform child = target.FindChild("Model_" + (object) index);
          if ((Object) child == (Object) null)
            break;
          if ((Object) child.GetComponent<Renderer>() != (Object) null)
            child.GetComponent<Renderer>().material = material;
        }
      }
    }

    public static void highlight(Transform target, Color color)
    {
      Highlighter highlighter = target.GetComponent<Highlighter>();
      if ((Object) highlighter == (Object) null)
        highlighter = target.gameObject.AddComponent<Highlighter>();
      highlighter.ConstantOn(color);
      highlighter.SeeThroughOff();
    }

    public static void unhighlight(Transform target)
    {
      Highlighter component = target.GetComponent<Highlighter>();
      if ((Object) component == (Object) null)
        return;
      Object.Destroy((Object) component);
    }

    public static void skin(Transform target, Material skin)
    {
      if ((Object) target.GetComponent<Renderer>() != (Object) null)
      {
        target.GetComponent<Renderer>().material = skin;
      }
      else
      {
        if (!((Object) target.GetComponent<LODGroup>() != (Object) null))
          return;
        for (int index = 0; index < 4; ++index)
        {
          Transform child = target.FindChild("Model_" + (object) index);
          if ((Object) child == (Object) null)
            break;
          if ((Object) child.GetComponent<Renderer>() != (Object) null)
            child.GetComponent<Renderer>().material = skin;
        }
      }
    }
  }
}
