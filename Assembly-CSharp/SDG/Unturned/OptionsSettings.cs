// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.OptionsSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class OptionsSettings
  {
    public static readonly byte SAVEDATA_VERSION = (byte) 8;
    public static readonly byte MIN_FOV = (byte) 60;
    public static readonly byte MAX_FOV = (byte) 60;
    private static float _fov;
    private static float _view;
    public static float volume;
    public static bool debug;
    public static bool music;
    public static bool physics;
    public static bool gore;
    public static bool filter;
    public static bool chatText;
    public static bool chatVoice;
    public static bool metric;
    public static bool hints;
    public static Color crosshairColor;
    public static Color hitmarkerColor;
    public static Color criticalHitmarkerColor;
    public static Color cursorColor;

    public static float fov
    {
      get
      {
        return OptionsSettings._fov;
      }
      set
      {
        OptionsSettings._fov = value;
        OptionsSettings._view = (float) OptionsSettings.MIN_FOV + (float) OptionsSettings.MAX_FOV * value;
      }
    }

    public static float view
    {
      get
      {
        return OptionsSettings._view;
      }
    }

    public static void apply()
    {
      if (!Level.isLoaded && (Object) Camera.main != (Object) null)
        Camera.main.fieldOfView = OptionsSettings.view;
      if (Application.loadedLevel <= Level.MENU)
        MenuConfigurationOptions.apply();
      AudioListener.volume = OptionsSettings.volume;
    }

    public static void load()
    {
      if (ReadWrite.fileExists("/Options.dat", true))
      {
        Block block = ReadWrite.readBlock("/Options.dat", true, (byte) 0);
        if (block != null)
        {
          byte num = block.readByte();
          if ((int) num > 2)
          {
            OptionsSettings.music = block.readBoolean();
            OptionsSettings.physics = block.readBoolean();
            OptionsSettings.fov = (int) num <= 7 ? block.readSingle() / 2f : block.readSingle();
            OptionsSettings.volume = (int) num <= 4 ? 1f : block.readSingle();
            OptionsSettings.debug = block.readBoolean();
            OptionsSettings.gore = block.readBoolean();
            OptionsSettings.filter = block.readBoolean();
            OptionsSettings.chatText = block.readBoolean();
            OptionsSettings.chatVoice = block.readBoolean();
            OptionsSettings.metric = block.readBoolean();
            OptionsSettings.hints = (int) num <= 3 || block.readBoolean();
            if ((int) num > 6)
            {
              OptionsSettings.crosshairColor = block.readColor();
              OptionsSettings.hitmarkerColor = block.readColor();
              OptionsSettings.criticalHitmarkerColor = block.readColor();
              OptionsSettings.cursorColor = block.readColor();
              return;
            }
            OptionsSettings.crosshairColor = Color.white;
            OptionsSettings.hitmarkerColor = Color.white;
            OptionsSettings.criticalHitmarkerColor = Color.red;
            OptionsSettings.cursorColor = Color.white;
            return;
          }
        }
      }
      OptionsSettings.music = true;
      OptionsSettings.physics = true;
      OptionsSettings.fov = 0.5f;
      OptionsSettings.volume = 1f;
      OptionsSettings.debug = false;
      OptionsSettings.gore = true;
      OptionsSettings.filter = false;
      OptionsSettings.chatText = true;
      OptionsSettings.chatVoice = true;
      OptionsSettings.metric = true;
      OptionsSettings.hints = true;
      OptionsSettings.crosshairColor = Color.white;
      OptionsSettings.hitmarkerColor = Color.white;
      OptionsSettings.criticalHitmarkerColor = Color.red;
      OptionsSettings.cursorColor = Color.white;
    }

    public static void save()
    {
      Block block = new Block();
      block.writeByte(OptionsSettings.SAVEDATA_VERSION);
      block.writeBoolean(OptionsSettings.music);
      block.writeBoolean(OptionsSettings.physics);
      block.writeSingle(OptionsSettings.fov);
      block.writeSingle(OptionsSettings.volume);
      block.writeBoolean(OptionsSettings.debug);
      block.writeBoolean(OptionsSettings.gore);
      block.writeBoolean(OptionsSettings.filter);
      block.writeBoolean(OptionsSettings.chatText);
      block.writeBoolean(OptionsSettings.chatVoice);
      block.writeBoolean(OptionsSettings.metric);
      block.writeBoolean(OptionsSettings.hints);
      block.writeColor(OptionsSettings.crosshairColor);
      block.writeColor(OptionsSettings.hitmarkerColor);
      block.writeColor(OptionsSettings.criticalHitmarkerColor);
      block.writeColor(OptionsSettings.cursorColor);
      ReadWrite.writeBlock("/Options.dat", true, block);
    }
  }
}
