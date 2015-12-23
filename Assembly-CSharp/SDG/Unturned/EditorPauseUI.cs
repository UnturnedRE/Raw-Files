// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.EditorPauseUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class EditorPauseUI
  {
    private static Sleek container;
    public static bool active;
    private static SleekButtonIcon saveButton;
    private static SleekButtonIcon mapButton;
    private static SleekButtonIcon exitButton;

    public EditorPauseUI()
    {
      Local local = Localization.read("/Editor/EditorPause.dat");
      Bundle bundle = Bundles.getBundle("/Bundles/Textures/Edit/Icons/EditorPause/EditorPause.unity3d");
      EditorPauseUI.container = new Sleek();
      EditorPauseUI.container.positionOffset_X = 10;
      EditorPauseUI.container.positionOffset_Y = 10;
      EditorPauseUI.container.positionScale_X = 1f;
      EditorPauseUI.container.sizeOffset_X = -20;
      EditorPauseUI.container.sizeOffset_Y = -20;
      EditorPauseUI.container.sizeScale_X = 1f;
      EditorPauseUI.container.sizeScale_Y = 1f;
      EditorUI.window.add(EditorPauseUI.container);
      EditorPauseUI.active = false;
      EditorPauseUI.saveButton = new SleekButtonIcon((Texture2D) bundle.load("Save"));
      EditorPauseUI.saveButton.positionOffset_X = -100;
      EditorPauseUI.saveButton.positionOffset_Y = -55;
      EditorPauseUI.saveButton.positionScale_X = 0.5f;
      EditorPauseUI.saveButton.positionScale_Y = 0.5f;
      EditorPauseUI.saveButton.sizeOffset_X = 200;
      EditorPauseUI.saveButton.sizeOffset_Y = 30;
      EditorPauseUI.saveButton.text = local.format("Save_Button");
      EditorPauseUI.saveButton.tooltip = local.format("Save_Button_Tooltip");
      EditorPauseUI.saveButton.onClickedButton = new ClickedButton(EditorPauseUI.onClickedSaveButton);
      EditorPauseUI.container.add((Sleek) EditorPauseUI.saveButton);
      EditorPauseUI.mapButton = new SleekButtonIcon((Texture2D) bundle.load("Map"));
      EditorPauseUI.mapButton.positionOffset_X = -100;
      EditorPauseUI.mapButton.positionOffset_Y = -15;
      EditorPauseUI.mapButton.positionScale_X = 0.5f;
      EditorPauseUI.mapButton.positionScale_Y = 0.5f;
      EditorPauseUI.mapButton.sizeOffset_X = 200;
      EditorPauseUI.mapButton.sizeOffset_Y = 30;
      EditorPauseUI.mapButton.text = local.format("Map_Button");
      EditorPauseUI.mapButton.tooltip = local.format("Map_Button_Tooltip");
      EditorPauseUI.mapButton.onClickedButton = new ClickedButton(EditorPauseUI.onClickedMapButton);
      EditorPauseUI.container.add((Sleek) EditorPauseUI.mapButton);
      EditorPauseUI.exitButton = new SleekButtonIcon((Texture2D) bundle.load("Exit"));
      EditorPauseUI.exitButton.positionOffset_X = -100;
      EditorPauseUI.exitButton.positionOffset_Y = 25;
      EditorPauseUI.exitButton.positionScale_X = 0.5f;
      EditorPauseUI.exitButton.positionScale_Y = 0.5f;
      EditorPauseUI.exitButton.sizeOffset_X = 200;
      EditorPauseUI.exitButton.sizeOffset_Y = 30;
      EditorPauseUI.exitButton.text = local.format("Exit_Button");
      EditorPauseUI.exitButton.tooltip = local.format("Exit_Button_Tooltip");
      EditorPauseUI.exitButton.onClickedButton = new ClickedButton(EditorPauseUI.onClickedExitButton);
      EditorPauseUI.container.add((Sleek) EditorPauseUI.exitButton);
      bundle.unload();
    }

    public static void open()
    {
      if (EditorPauseUI.active)
      {
        EditorPauseUI.close();
      }
      else
      {
        EditorPauseUI.active = true;
        EditorPauseUI.container.lerpPositionScale(0.0f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
      }
    }

    public static void close()
    {
      if (!EditorPauseUI.active)
        return;
      EditorPauseUI.active = false;
      EditorPauseUI.container.lerpPositionScale(1f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
    }

    private static void onClickedSaveButton(SleekButton button)
    {
      Level.save();
    }

    private static void onClickedMapButton(SleekButton button)
    {
      Level.mapify();
    }

    private static void onClickedExitButton(SleekButton button)
    {
      Level.exit();
    }
  }
}
