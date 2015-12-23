// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.MenuWorkshopUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class MenuWorkshopUI
  {
    private static Sleek container;
    public static bool active;
    private static SleekButtonIcon submitButton;
    private static SleekButtonIcon editorButton;

    public MenuWorkshopUI()
    {
      Local local = Localization.read("/Menu/Workshop/MenuWorkshop.dat");
      Bundle bundle = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Workshop/MenuWorkshop/MenuWorkshop.unity3d");
      MenuWorkshopUI.container = new Sleek();
      MenuWorkshopUI.container.positionOffset_X = 10;
      MenuWorkshopUI.container.positionOffset_Y = 10;
      MenuWorkshopUI.container.positionScale_Y = -1f;
      MenuWorkshopUI.container.sizeOffset_X = -20;
      MenuWorkshopUI.container.sizeOffset_Y = -20;
      MenuWorkshopUI.container.sizeScale_X = 1f;
      MenuWorkshopUI.container.sizeScale_Y = 1f;
      MenuUI.container.add(MenuWorkshopUI.container);
      MenuWorkshopUI.active = false;
      MenuWorkshopUI.submitButton = new SleekButtonIcon((Texture2D) bundle.load("Submit"));
      MenuWorkshopUI.submitButton.positionOffset_X = -100;
      MenuWorkshopUI.submitButton.positionOffset_Y = 5;
      MenuWorkshopUI.submitButton.positionScale_X = 0.5f;
      MenuWorkshopUI.submitButton.positionScale_Y = 0.5f;
      MenuWorkshopUI.submitButton.sizeOffset_X = 200;
      MenuWorkshopUI.submitButton.sizeOffset_Y = 50;
      MenuWorkshopUI.submitButton.text = local.format("SubmitButtonText");
      MenuWorkshopUI.submitButton.tooltip = local.format("SubmitButtonTooltip");
      MenuWorkshopUI.submitButton.onClickedButton = new ClickedButton(MenuWorkshopUI.onClickedSubmitButton);
      MenuWorkshopUI.submitButton.fontSize = 14;
      MenuWorkshopUI.container.add((Sleek) MenuWorkshopUI.submitButton);
      MenuWorkshopUI.editorButton = new SleekButtonIcon((Texture2D) bundle.load("Editor"));
      MenuWorkshopUI.editorButton.positionOffset_X = -100;
      MenuWorkshopUI.editorButton.positionOffset_Y = -55;
      MenuWorkshopUI.editorButton.positionScale_X = 0.5f;
      MenuWorkshopUI.editorButton.positionScale_Y = 0.5f;
      MenuWorkshopUI.editorButton.sizeOffset_X = 200;
      MenuWorkshopUI.editorButton.sizeOffset_Y = 50;
      MenuWorkshopUI.editorButton.text = local.format("EditorButtonText");
      MenuWorkshopUI.editorButton.tooltip = local.format("EditorButtonTooltip");
      MenuWorkshopUI.editorButton.onClickedButton = new ClickedButton(MenuWorkshopUI.onClickedEditorButton);
      MenuWorkshopUI.editorButton.fontSize = 14;
      MenuWorkshopUI.container.add((Sleek) MenuWorkshopUI.editorButton);
      bundle.unload();
      MenuWorkshopSubmitUI workshopSubmitUi = new MenuWorkshopSubmitUI();
      MenuWorkshopEditorUI workshopEditorUi = new MenuWorkshopEditorUI();
    }

    public static void open()
    {
      if (MenuWorkshopUI.active)
      {
        MenuWorkshopUI.close();
      }
      else
      {
        MenuWorkshopUI.active = true;
        MenuWorkshopUI.container.lerpPositionScale(0.0f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
      }
    }

    public static void close()
    {
      if (!MenuWorkshopUI.active)
        return;
      MenuWorkshopUI.active = false;
      MenuWorkshopUI.container.lerpPositionScale(0.0f, -1f, ESleekLerp.EXPONENTIAL, 20f);
    }

    private static void onClickedSubmitButton(SleekButton button)
    {
      MenuWorkshopSubmitUI.open();
      MenuWorkshopUI.close();
    }

    private static void onClickedEditorButton(SleekButton button)
    {
      MenuWorkshopEditorUI.open();
      MenuWorkshopUI.close();
    }
  }
}
