// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.EditorSpawnsUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class EditorSpawnsUI
  {
    private static Sleek container;
    public static bool active;
    private static SleekButtonIcon animalsButton;
    private static SleekButtonIcon itemsButton;
    private static SleekButtonIcon zombiesButton;
    private static SleekButtonIcon vehiclesButton;

    public EditorSpawnsUI()
    {
      Local local = Localization.read("/Editor/EditorSpawns.dat");
      Bundle bundle = Bundles.getBundle("/Bundles/Textures/Edit/Icons/EditorSpawns/EditorSpawns.unity3d");
      EditorSpawnsUI.container = new Sleek();
      EditorSpawnsUI.container.positionOffset_X = 10;
      EditorSpawnsUI.container.positionOffset_Y = 10;
      EditorSpawnsUI.container.positionScale_X = 1f;
      EditorSpawnsUI.container.sizeOffset_X = -20;
      EditorSpawnsUI.container.sizeOffset_Y = -20;
      EditorSpawnsUI.container.sizeScale_X = 1f;
      EditorSpawnsUI.container.sizeScale_Y = 1f;
      EditorUI.window.add(EditorSpawnsUI.container);
      EditorSpawnsUI.active = false;
      EditorSpawnsUI.animalsButton = new SleekButtonIcon((Texture2D) bundle.load("Animals"));
      EditorSpawnsUI.animalsButton.positionOffset_Y = 40;
      EditorSpawnsUI.animalsButton.sizeOffset_X = -5;
      EditorSpawnsUI.animalsButton.sizeOffset_Y = 30;
      EditorSpawnsUI.animalsButton.sizeScale_X = 0.25f;
      EditorSpawnsUI.animalsButton.text = local.format("AnimalsButtonText");
      EditorSpawnsUI.animalsButton.tooltip = local.format("AnimalsButtonTooltip");
      EditorSpawnsUI.animalsButton.onClickedButton = new ClickedButton(EditorSpawnsUI.onClickedAnimalsButton);
      EditorSpawnsUI.container.add((Sleek) EditorSpawnsUI.animalsButton);
      EditorSpawnsUI.itemsButton = new SleekButtonIcon((Texture2D) bundle.load("Items"));
      EditorSpawnsUI.itemsButton.positionOffset_X = 5;
      EditorSpawnsUI.itemsButton.positionOffset_Y = 40;
      EditorSpawnsUI.itemsButton.positionScale_X = 0.25f;
      EditorSpawnsUI.itemsButton.sizeOffset_X = -10;
      EditorSpawnsUI.itemsButton.sizeOffset_Y = 30;
      EditorSpawnsUI.itemsButton.sizeScale_X = 0.25f;
      EditorSpawnsUI.itemsButton.text = local.format("ItemsButtonText");
      EditorSpawnsUI.itemsButton.tooltip = local.format("ItemsButtonTooltip");
      EditorSpawnsUI.itemsButton.onClickedButton = new ClickedButton(EditorSpawnsUI.onClickItemsButton);
      EditorSpawnsUI.container.add((Sleek) EditorSpawnsUI.itemsButton);
      EditorSpawnsUI.zombiesButton = new SleekButtonIcon((Texture2D) bundle.load("Zombies"));
      EditorSpawnsUI.zombiesButton.positionOffset_X = 5;
      EditorSpawnsUI.zombiesButton.positionOffset_Y = 40;
      EditorSpawnsUI.zombiesButton.positionScale_X = 0.5f;
      EditorSpawnsUI.zombiesButton.sizeOffset_X = -10;
      EditorSpawnsUI.zombiesButton.sizeOffset_Y = 30;
      EditorSpawnsUI.zombiesButton.sizeScale_X = 0.25f;
      EditorSpawnsUI.zombiesButton.text = local.format("ZombiesButtonText");
      EditorSpawnsUI.zombiesButton.tooltip = local.format("ZombiesButtonTooltip");
      EditorSpawnsUI.zombiesButton.onClickedButton = new ClickedButton(EditorSpawnsUI.onClickedZombiesButton);
      EditorSpawnsUI.container.add((Sleek) EditorSpawnsUI.zombiesButton);
      EditorSpawnsUI.vehiclesButton = new SleekButtonIcon((Texture2D) bundle.load("Vehicles"));
      EditorSpawnsUI.vehiclesButton.positionOffset_X = 5;
      EditorSpawnsUI.vehiclesButton.positionOffset_Y = 40;
      EditorSpawnsUI.vehiclesButton.positionScale_X = 0.75f;
      EditorSpawnsUI.vehiclesButton.sizeOffset_X = -5;
      EditorSpawnsUI.vehiclesButton.sizeOffset_Y = 30;
      EditorSpawnsUI.vehiclesButton.sizeScale_X = 0.25f;
      EditorSpawnsUI.vehiclesButton.text = local.format("VehiclesButtonText");
      EditorSpawnsUI.vehiclesButton.tooltip = local.format("VehiclesButtonTooltip");
      EditorSpawnsUI.vehiclesButton.onClickedButton = new ClickedButton(EditorSpawnsUI.onClickedVehiclesButton);
      EditorSpawnsUI.container.add((Sleek) EditorSpawnsUI.vehiclesButton);
      bundle.unload();
      EditorSpawnsAnimalsUI editorSpawnsAnimalsUi = new EditorSpawnsAnimalsUI();
      EditorSpawnsItemsUI editorSpawnsItemsUi = new EditorSpawnsItemsUI();
      EditorSpawnsZombiesUI editorSpawnsZombiesUi = new EditorSpawnsZombiesUI();
      EditorSpawnsVehiclesUI spawnsVehiclesUi = new EditorSpawnsVehiclesUI();
    }

    public static void open()
    {
      if (EditorSpawnsUI.active)
      {
        EditorSpawnsUI.close();
      }
      else
      {
        EditorSpawnsUI.active = true;
        EditorSpawnsUI.container.lerpPositionScale(0.0f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
      }
    }

    public static void close()
    {
      if (!EditorSpawnsUI.active)
        return;
      EditorSpawnsUI.active = false;
      EditorSpawnsItemsUI.close();
      EditorSpawnsAnimalsUI.close();
      EditorSpawnsZombiesUI.close();
      EditorSpawnsVehiclesUI.close();
      EditorSpawnsUI.container.lerpPositionScale(1f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
    }

    private static void onClickedAnimalsButton(SleekButton button)
    {
      EditorSpawnsItemsUI.close();
      EditorSpawnsZombiesUI.close();
      EditorSpawnsVehiclesUI.close();
      EditorSpawnsAnimalsUI.open();
    }

    private static void onClickItemsButton(SleekButton button)
    {
      EditorSpawnsAnimalsUI.close();
      EditorSpawnsZombiesUI.close();
      EditorSpawnsVehiclesUI.close();
      EditorSpawnsItemsUI.open();
    }

    private static void onClickedZombiesButton(SleekButton button)
    {
      EditorSpawnsAnimalsUI.close();
      EditorSpawnsItemsUI.close();
      EditorSpawnsVehiclesUI.close();
      EditorSpawnsZombiesUI.open();
    }

    private static void onClickedVehiclesButton(SleekButton button)
    {
      EditorSpawnsAnimalsUI.close();
      EditorSpawnsItemsUI.close();
      EditorSpawnsZombiesUI.close();
      EditorSpawnsVehiclesUI.open();
    }
  }
}
