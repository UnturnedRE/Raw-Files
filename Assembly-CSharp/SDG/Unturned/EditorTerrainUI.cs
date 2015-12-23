// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.EditorTerrainUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class EditorTerrainUI
  {
    private static Sleek container;
    public static bool active;
    private static SleekButtonIcon heightButton;
    private static SleekButtonIcon materialsButton;
    private static SleekButtonIcon detailsButton;
    private static SleekButtonIcon resourcesButton;

    public EditorTerrainUI()
    {
      Local local = Localization.read("/Editor/EditorTerrain.dat");
      Bundle bundle = Bundles.getBundle("/Bundles/Textures/Edit/Icons/EditorTerrain/EditorTerrain.unity3d");
      EditorTerrainUI.container = new Sleek();
      EditorTerrainUI.container.positionOffset_X = 10;
      EditorTerrainUI.container.positionOffset_Y = 10;
      EditorTerrainUI.container.positionScale_X = 1f;
      EditorTerrainUI.container.sizeOffset_X = -20;
      EditorTerrainUI.container.sizeOffset_Y = -20;
      EditorTerrainUI.container.sizeScale_X = 1f;
      EditorTerrainUI.container.sizeScale_Y = 1f;
      EditorUI.window.add(EditorTerrainUI.container);
      EditorTerrainUI.active = false;
      EditorTerrainUI.heightButton = new SleekButtonIcon((Texture2D) bundle.load("Height"));
      EditorTerrainUI.heightButton.positionOffset_Y = 40;
      EditorTerrainUI.heightButton.sizeOffset_X = -5;
      EditorTerrainUI.heightButton.sizeOffset_Y = 30;
      EditorTerrainUI.heightButton.sizeScale_X = 0.25f;
      EditorTerrainUI.heightButton.text = local.format("HeightButtonText");
      EditorTerrainUI.heightButton.tooltip = local.format("HeightButtonTooltip");
      EditorTerrainUI.heightButton.onClickedButton = new ClickedButton(EditorTerrainUI.onClickedHeightButton);
      EditorTerrainUI.container.add((Sleek) EditorTerrainUI.heightButton);
      EditorTerrainUI.materialsButton = new SleekButtonIcon((Texture2D) bundle.load("Materials"));
      EditorTerrainUI.materialsButton.positionOffset_X = 5;
      EditorTerrainUI.materialsButton.positionOffset_Y = 40;
      EditorTerrainUI.materialsButton.positionScale_X = 0.25f;
      EditorTerrainUI.materialsButton.sizeOffset_X = -10;
      EditorTerrainUI.materialsButton.sizeOffset_Y = 30;
      EditorTerrainUI.materialsButton.sizeScale_X = 0.25f;
      EditorTerrainUI.materialsButton.text = local.format("MaterialsButtonText");
      EditorTerrainUI.materialsButton.tooltip = local.format("MaterialsButtonTooltip");
      EditorTerrainUI.materialsButton.onClickedButton = new ClickedButton(EditorTerrainUI.onClickedMaterialsButton);
      EditorTerrainUI.container.add((Sleek) EditorTerrainUI.materialsButton);
      EditorTerrainUI.detailsButton = new SleekButtonIcon((Texture2D) bundle.load("Details"));
      EditorTerrainUI.detailsButton.positionOffset_X = 5;
      EditorTerrainUI.detailsButton.positionOffset_Y = 40;
      EditorTerrainUI.detailsButton.positionScale_X = 0.5f;
      EditorTerrainUI.detailsButton.sizeOffset_X = -10;
      EditorTerrainUI.detailsButton.sizeOffset_Y = 30;
      EditorTerrainUI.detailsButton.sizeScale_X = 0.25f;
      EditorTerrainUI.detailsButton.text = local.format("DetailsButtonText");
      EditorTerrainUI.detailsButton.tooltip = local.format("DetailsButtonTooltip");
      EditorTerrainUI.detailsButton.onClickedButton = new ClickedButton(EditorTerrainUI.onClickedDetailsButton);
      EditorTerrainUI.container.add((Sleek) EditorTerrainUI.detailsButton);
      EditorTerrainUI.resourcesButton = new SleekButtonIcon((Texture2D) bundle.load("Resources"));
      EditorTerrainUI.resourcesButton.positionOffset_X = 5;
      EditorTerrainUI.resourcesButton.positionOffset_Y = 40;
      EditorTerrainUI.resourcesButton.positionScale_X = 0.75f;
      EditorTerrainUI.resourcesButton.sizeOffset_X = -5;
      EditorTerrainUI.resourcesButton.sizeOffset_Y = 30;
      EditorTerrainUI.resourcesButton.sizeScale_X = 0.25f;
      EditorTerrainUI.resourcesButton.text = local.format("ResourcesButtonText");
      EditorTerrainUI.resourcesButton.tooltip = local.format("ResourcesButtonTooltip");
      EditorTerrainUI.resourcesButton.onClickedButton = new ClickedButton(EditorTerrainUI.onClickedResourcesButton);
      EditorTerrainUI.container.add((Sleek) EditorTerrainUI.resourcesButton);
      bundle.unload();
      EditorTerrainHeightUI editorTerrainHeightUi = new EditorTerrainHeightUI();
      EditorTerrainMaterialsUI terrainMaterialsUi = new EditorTerrainMaterialsUI();
      EditorTerrainDetailsUI terrainDetailsUi = new EditorTerrainDetailsUI();
      EditorTerrainResourcesUI terrainResourcesUi = new EditorTerrainResourcesUI();
    }

    public static void open()
    {
      if (EditorTerrainUI.active)
      {
        EditorTerrainUI.close();
      }
      else
      {
        EditorTerrainUI.active = true;
        EditorTerrainUI.container.lerpPositionScale(0.0f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
      }
    }

    public static void close()
    {
      if (!EditorTerrainUI.active)
        return;
      EditorTerrainUI.active = false;
      EditorTerrainHeightUI.close();
      EditorTerrainMaterialsUI.close();
      EditorTerrainDetailsUI.close();
      EditorTerrainResourcesUI.close();
      EditorTerrainUI.container.lerpPositionScale(1f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
    }

    private static void onClickedHeightButton(SleekButton button)
    {
      EditorTerrainMaterialsUI.close();
      EditorTerrainDetailsUI.close();
      EditorTerrainResourcesUI.close();
      EditorTerrainHeightUI.open();
    }

    private static void onClickedMaterialsButton(SleekButton button)
    {
      EditorTerrainHeightUI.close();
      EditorTerrainDetailsUI.close();
      EditorTerrainResourcesUI.close();
      EditorTerrainMaterialsUI.open();
    }

    private static void onClickedDetailsButton(SleekButton button)
    {
      EditorTerrainHeightUI.close();
      EditorTerrainMaterialsUI.close();
      EditorTerrainResourcesUI.close();
      EditorTerrainDetailsUI.open();
    }

    private static void onClickedResourcesButton(SleekButton button)
    {
      EditorTerrainHeightUI.close();
      EditorTerrainMaterialsUI.close();
      EditorTerrainDetailsUI.close();
      EditorTerrainResourcesUI.open();
    }
  }
}
