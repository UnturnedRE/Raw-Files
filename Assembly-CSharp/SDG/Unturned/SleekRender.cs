// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.SleekRender
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class SleekRender
  {
    public static readonly int FONT_SIZE = 12;
    private static readonly Color OUTLINE = new Color(0.0f, 0.0f, 0.0f, 0.5f);
    public static bool allowInput;
    public static Color tooltip;

    public static void drawAngledImageTexture(Rect area, Texture texture, float angle, Color color)
    {
      if (!((Object) texture != (Object) null))
        return;
      if (!GUI.enabled)
        color.a *= 0.5f;
      GUI.color = color;
      Matrix4x4 matrix = GUI.matrix;
      GUIUtility.RotateAroundPivot(angle, area.center);
      GUI.DrawTexture(area, texture, ScaleMode.StretchToFill);
      GUI.matrix = matrix;
      GUI.color = Color.white;
    }

    public static void drawImageTexture(Rect area, Texture texture, Color color)
    {
      if (!((Object) texture != (Object) null))
        return;
      if (!GUI.enabled)
        color.a *= 0.5f;
      GUI.color = color;
      GUI.DrawTexture(area, texture, ScaleMode.StretchToFill);
      GUI.color = Color.white;
    }

    public static void drawImageMaterial(Rect area, Texture texture, Material material)
    {
      if (!((Object) texture != (Object) null))
        return;
      Graphics.DrawTexture(area, texture, material);
    }

    public static bool drawImageButton(Rect area, Texture texture, Color color)
    {
      if (!((Object) texture != (Object) null))
        return false;
      if (!GUI.enabled)
        color.a *= 0.5f;
      GUI.color = color;
      GUI.DrawTexture(area, texture, ScaleMode.StretchToFill);
      GUI.color = Color.white;
      return SleekRender.allowInput && (double) Event.current.mousePosition.x > (double) area.xMin && ((double) Event.current.mousePosition.y > (double) area.yMin && (double) Event.current.mousePosition.x < (double) area.xMax) && ((double) Event.current.mousePosition.y < (double) area.yMax && Event.current.type == EventType.MouseDown);
    }

    public static void drawTile(Rect area, Texture texture, Color color)
    {
      if (!((Object) texture != (Object) null))
        return;
      if (!GUI.enabled)
        color.a *= 0.5f;
      GUI.color = color;
      GUI.DrawTextureWithTexCoords(area, texture, new Rect(0.0f, 0.0f, area.width / (float) texture.width, area.height / (float) texture.height));
      GUI.color = Color.white;
    }

    public static bool drawGrid(Rect area, Texture texture, Color color)
    {
      if (!((Object) texture != (Object) null))
        return false;
      if (!GUI.enabled)
        color.a *= 0.5f;
      GUI.color = color;
      GUI.DrawTextureWithTexCoords(area, texture, new Rect(0.0f, 0.0f, area.width / (float) texture.width, area.height / (float) texture.height));
      GUI.color = Color.white;
      return Event.current.type == EventType.MouseDown && (double) Event.current.mousePosition.x > (double) area.xMin && ((double) Event.current.mousePosition.y > (double) area.yMin && (double) Event.current.mousePosition.x < (double) area.xMax) && (double) Event.current.mousePosition.y < (double) area.yMax;
    }

    public static bool drawToggle(Rect area, Color color, bool state)
    {
      GUI.backgroundColor = color;
      return GUI.Toggle(area, state, string.Empty);
    }

    public static bool drawButton(Rect area, Color color)
    {
      if (SleekRender.allowInput)
      {
        GUI.backgroundColor = color;
        return GUI.Button(area, string.Empty);
      }
      SleekRender.drawBox(area, color);
      return false;
    }

    public static bool drawRepeat(Rect area, Color color)
    {
      if (SleekRender.allowInput)
      {
        GUI.backgroundColor = color;
        return GUI.RepeatButton(area, string.Empty);
      }
      SleekRender.drawBox(area, color);
      return false;
    }

    public static void drawBox(Rect area, Color color, GUIContent content)
    {
      if (content.tooltip != null && content.tooltip.Length > 0 && area.Contains(Event.current.mousePosition))
        SleekRender.tooltip = color;
      GUI.backgroundColor = color;
      GUI.Box(area, content);
    }

    public static void drawBox(Rect area, Color color)
    {
      GUI.backgroundColor = color;
      GUI.Box(area, string.Empty);
    }

    public static void drawLabel(Rect area, FontStyle fontStyle, TextAnchor fontAlignment, int fontSize, bool isRich, Color color, GUIContent content)
    {
      if (content.tooltip != null && content.tooltip.Length > 0 && area.Contains(Event.current.mousePosition))
        SleekRender.tooltip = color;
      GUI.skin.label.fontStyle = fontStyle;
      GUI.skin.label.alignment = fontAlignment;
      GUI.skin.label.fontSize = fontSize;
      if (isRich)
      {
        bool richText = GUI.skin.label.richText;
        GUI.skin.label.richText = isRich;
        GUI.Label(area, content);
        GUI.skin.label.richText = richText;
      }
      else
      {
        GUI.contentColor = SleekRender.OUTLINE;
        --area.x;
        GUI.Label(area, content);
        area.x += 2f;
        GUI.Label(area, content);
        --area.x;
        --area.y;
        GUI.Label(area, content);
        area.y += 2f;
        GUI.Label(area, content);
        --area.y;
        GUI.contentColor = color;
        GUI.Label(area, content);
      }
    }

    public static void drawLabel(Rect area, FontStyle fontStyle, TextAnchor fontAlignment, int fontSize, bool isRich, Color color, string text)
    {
      GUI.skin.label.fontStyle = fontStyle;
      GUI.skin.label.alignment = fontAlignment;
      GUI.skin.label.fontSize = fontSize;
      if (isRich)
      {
        bool richText = GUI.skin.label.richText;
        GUI.skin.label.richText = isRich;
        GUI.Label(area, text);
        GUI.skin.label.richText = richText;
      }
      else
      {
        GUI.contentColor = SleekRender.OUTLINE;
        --area.x;
        GUI.Label(area, text);
        area.x += 2f;
        GUI.Label(area, text);
        --area.x;
        --area.y;
        GUI.Label(area, text);
        area.y += 2f;
        GUI.Label(area, text);
        --area.y;
        GUI.contentColor = color;
        GUI.Label(area, text);
      }
    }

    public static string drawField(Rect area, FontStyle fontStyle, TextAnchor fontAlignment, int fontSize, Color color_0, Color color_1, string text, bool multiline)
    {
      GUI.skin.textField.fontStyle = fontStyle;
      GUI.skin.textField.alignment = fontAlignment;
      GUI.skin.textField.fontSize = fontSize;
      GUI.backgroundColor = color_0;
      GUI.contentColor = color_1;
      if (SleekRender.allowInput)
      {
        text = !multiline ? GUI.TextField(area, text) : GUI.TextArea(area, text);
        if (text == null)
          text = string.Empty;
        return text;
      }
      SleekRender.drawBox(area, color_0);
      SleekRender.drawLabel(area, fontStyle, fontAlignment, fontSize, false, color_1, text);
      return text;
    }

    public static string drawField(Rect area, FontStyle fontStyle, TextAnchor fontAlignment, int fontSize, Color color_0, Color color_1, string text, char replace)
    {
      GUI.skin.textField.fontStyle = fontStyle;
      GUI.skin.textField.alignment = fontAlignment;
      GUI.skin.textField.fontSize = fontSize;
      GUI.backgroundColor = color_0;
      GUI.contentColor = color_1;
      if (SleekRender.allowInput)
      {
        text = GUI.PasswordField(area, text, replace);
        if (text == null)
          text = string.Empty;
        return text;
      }
      SleekRender.drawBox(area, color_0);
      string text1 = string.Empty;
      for (int index = 0; index < text.Length; ++index)
        text1 += (string) (object) replace;
      SleekRender.drawLabel(area, fontStyle, fontAlignment, fontSize, false, color_1, text1);
      return text;
    }

    public static float drawSlider(Rect area, ESleekOrientation orientation, float state, float size)
    {
      if (orientation == ESleekOrientation.HORIZONTAL)
        return GUI.HorizontalScrollbar(area, state, size, 0.0f, 1f);
      return GUI.VerticalScrollbar(area, state, size, 0.0f, 1f);
    }
  }
}
