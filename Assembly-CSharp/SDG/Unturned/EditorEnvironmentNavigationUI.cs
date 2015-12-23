// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.EditorEnvironmentNavigationUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class EditorEnvironmentNavigationUI
  {
    private static Sleek container;
    public static bool active;
    private static SleekSlider widthSlider;
    private static SleekSlider heightSlider;
    private static SleekButtonIcon bakeNavigationButton;

    public EditorEnvironmentNavigationUI()
    {
      Local local = Localization.read("/Editor/EditorEnvironmentNavigation.dat");
      Bundle bundle = Bundles.getBundle("/Bundles/Textures/Edit/Icons/EditorEnvironmentNavigation/EditorEnvironmentNavigation.unity3d");
      EditorEnvironmentNavigationUI.container = new Sleek();
      EditorEnvironmentNavigationUI.container.positionOffset_X = 10;
      EditorEnvironmentNavigationUI.container.positionOffset_Y = 10;
      EditorEnvironmentNavigationUI.container.positionScale_X = 1f;
      EditorEnvironmentNavigationUI.container.sizeOffset_X = -20;
      EditorEnvironmentNavigationUI.container.sizeOffset_Y = -20;
      EditorEnvironmentNavigationUI.container.sizeScale_X = 1f;
      EditorEnvironmentNavigationUI.container.sizeScale_Y = 1f;
      EditorUI.window.add(EditorEnvironmentNavigationUI.container);
      EditorEnvironmentNavigationUI.active = false;
      EditorEnvironmentNavigationUI.widthSlider = new SleekSlider();
      EditorEnvironmentNavigationUI.widthSlider.positionOffset_X = -200;
      EditorEnvironmentNavigationUI.widthSlider.positionOffset_Y = 80;
      EditorEnvironmentNavigationUI.widthSlider.positionScale_X = 1f;
      EditorEnvironmentNavigationUI.widthSlider.sizeOffset_X = 200;
      EditorEnvironmentNavigationUI.widthSlider.sizeOffset_Y = 20;
      EditorEnvironmentNavigationUI.widthSlider.orientation = ESleekOrientation.HORIZONTAL;
      EditorEnvironmentNavigationUI.widthSlider.addLabel(local.format("Width_Label"), ESleekSide.LEFT);
      EditorEnvironmentNavigationUI.widthSlider.onDragged = new Dragged(EditorEnvironmentNavigationUI.onDraggedWidthSlider);
      EditorEnvironmentNavigationUI.container.add((Sleek) EditorEnvironmentNavigationUI.widthSlider);
      EditorEnvironmentNavigationUI.widthSlider.isVisible = false;
      EditorEnvironmentNavigationUI.heightSlider = new SleekSlider();
      EditorEnvironmentNavigationUI.heightSlider.positionOffset_X = -200;
      EditorEnvironmentNavigationUI.heightSlider.positionOffset_Y = 110;
      EditorEnvironmentNavigationUI.heightSlider.positionScale_X = 1f;
      EditorEnvironmentNavigationUI.heightSlider.sizeOffset_X = 200;
      EditorEnvironmentNavigationUI.heightSlider.sizeOffset_Y = 20;
      EditorEnvironmentNavigationUI.heightSlider.orientation = ESleekOrientation.HORIZONTAL;
      EditorEnvironmentNavigationUI.heightSlider.addLabel(local.format("Height_Label"), ESleekSide.LEFT);
      EditorEnvironmentNavigationUI.heightSlider.onDragged = new Dragged(EditorEnvironmentNavigationUI.onDraggedHeightSlider);
      EditorEnvironmentNavigationUI.container.add((Sleek) EditorEnvironmentNavigationUI.heightSlider);
      EditorEnvironmentNavigationUI.heightSlider.isVisible = false;
      EditorEnvironmentNavigationUI.bakeNavigationButton = new SleekButtonIcon((Texture2D) bundle.load("Navigation"));
      EditorEnvironmentNavigationUI.bakeNavigationButton.positionOffset_X = -200;
      EditorEnvironmentNavigationUI.bakeNavigationButton.positionOffset_Y = -30;
      EditorEnvironmentNavigationUI.bakeNavigationButton.positionScale_X = 1f;
      EditorEnvironmentNavigationUI.bakeNavigationButton.positionScale_Y = 1f;
      EditorEnvironmentNavigationUI.bakeNavigationButton.sizeOffset_X = 200;
      EditorEnvironmentNavigationUI.bakeNavigationButton.sizeOffset_Y = 30;
      EditorEnvironmentNavigationUI.bakeNavigationButton.text = local.format("Bake_Navigation");
      EditorEnvironmentNavigationUI.bakeNavigationButton.tooltip = local.format("Bake_Navigation_Tooltip");
      EditorEnvironmentNavigationUI.bakeNavigationButton.onClickedButton = new ClickedButton(EditorEnvironmentNavigationUI.onClickedBakeNavigationButton);
      EditorEnvironmentNavigationUI.container.add((Sleek) EditorEnvironmentNavigationUI.bakeNavigationButton);
      EditorEnvironmentNavigationUI.bakeNavigationButton.isVisible = false;
      bundle.unload();
    }

    public static void open()
    {
      if (EditorEnvironmentNavigationUI.active)
      {
        EditorEnvironmentNavigationUI.close();
      }
      else
      {
        EditorEnvironmentNavigationUI.active = true;
        EditorNavigation.isPathfinding = true;
        EditorUI.message(EEditorMessage.NAVIGATION);
        EditorEnvironmentNavigationUI.container.lerpPositionScale(0.0f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
      }
    }

    public static void close()
    {
      if (!EditorEnvironmentNavigationUI.active)
        return;
      EditorEnvironmentNavigationUI.active = false;
      EditorNavigation.isPathfinding = false;
      EditorEnvironmentNavigationUI.container.lerpPositionScale(1f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
    }

    public static void updateSelection(Flag flag)
    {
      if (flag != null)
      {
        EditorEnvironmentNavigationUI.widthSlider.state = flag.width;
        EditorEnvironmentNavigationUI.heightSlider.state = flag.height;
      }
      EditorEnvironmentNavigationUI.widthSlider.isVisible = flag != null;
      EditorEnvironmentNavigationUI.heightSlider.isVisible = flag != null;
      EditorEnvironmentNavigationUI.bakeNavigationButton.isVisible = flag != null;
    }

    private static void onDraggedWidthSlider(SleekSlider slider, float state)
    {
      if (EditorNavigation.flag == null)
        return;
      EditorNavigation.flag.width = state;
      EditorNavigation.flag.buildMesh();
    }

    private static void onDraggedHeightSlider(SleekSlider slider, float state)
    {
      if (EditorNavigation.flag == null)
        return;
      EditorNavigation.flag.height = state;
      EditorNavigation.flag.buildMesh();
    }

    private static void onClickedBakeNavigationButton(SleekButton button)
    {
      if (EditorNavigation.flag == null)
        return;
      EditorNavigation.flag.bakeNavigation();
    }
  }
}
