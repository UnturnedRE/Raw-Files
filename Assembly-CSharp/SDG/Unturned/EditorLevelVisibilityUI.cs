// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.EditorLevelVisibilityUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

namespace SDG.Unturned
{
  public class EditorLevelVisibilityUI
  {
    private static Sleek container;
    public static bool active;
    public static SleekToggle roadsToggle;
    public static SleekToggle navigationToggle;
    public static SleekToggle nodesToggle;
    public static SleekToggle itemsToggle;
    public static SleekToggle playersToggle;
    public static SleekToggle zombiesToggle;
    public static SleekToggle vehiclesToggle;
    public static SleekToggle borderToggle;
    public static SleekToggle animalsToggle;

    public EditorLevelVisibilityUI()
    {
      Local local = Localization.read("/Editor/EditorLevelVisibility.dat");
      EditorLevelVisibilityUI.container = new Sleek();
      EditorLevelVisibilityUI.container.positionOffset_X = 10;
      EditorLevelVisibilityUI.container.positionOffset_Y = 10;
      EditorLevelVisibilityUI.container.positionScale_X = 1f;
      EditorLevelVisibilityUI.container.sizeOffset_X = -20;
      EditorLevelVisibilityUI.container.sizeOffset_Y = -20;
      EditorLevelVisibilityUI.container.sizeScale_X = 1f;
      EditorLevelVisibilityUI.container.sizeScale_Y = 1f;
      EditorUI.window.add(EditorLevelVisibilityUI.container);
      EditorLevelVisibilityUI.active = false;
      EditorLevelVisibilityUI.roadsToggle = new SleekToggle();
      EditorLevelVisibilityUI.roadsToggle.positionOffset_X = -200;
      EditorLevelVisibilityUI.roadsToggle.positionOffset_Y = 80;
      EditorLevelVisibilityUI.roadsToggle.positionScale_X = 1f;
      EditorLevelVisibilityUI.roadsToggle.sizeOffset_X = 40;
      EditorLevelVisibilityUI.roadsToggle.sizeOffset_Y = 40;
      EditorLevelVisibilityUI.roadsToggle.state = LevelVisibility.roadsVisible;
      EditorLevelVisibilityUI.roadsToggle.addLabel(local.format("Roads_Label"), ESleekSide.RIGHT);
      EditorLevelVisibilityUI.roadsToggle.onToggled = new Toggled(EditorLevelVisibilityUI.onToggledRoadsToggle);
      EditorLevelVisibilityUI.container.add((Sleek) EditorLevelVisibilityUI.roadsToggle);
      EditorLevelVisibilityUI.navigationToggle = new SleekToggle();
      EditorLevelVisibilityUI.navigationToggle.positionOffset_X = -200;
      EditorLevelVisibilityUI.navigationToggle.positionOffset_Y = 130;
      EditorLevelVisibilityUI.navigationToggle.positionScale_X = 1f;
      EditorLevelVisibilityUI.navigationToggle.sizeOffset_X = 40;
      EditorLevelVisibilityUI.navigationToggle.sizeOffset_Y = 40;
      EditorLevelVisibilityUI.navigationToggle.state = LevelVisibility.navigationVisible;
      EditorLevelVisibilityUI.navigationToggle.addLabel(local.format("Navigation_Label"), ESleekSide.RIGHT);
      EditorLevelVisibilityUI.navigationToggle.onToggled = new Toggled(EditorLevelVisibilityUI.onToggledNavigationToggle);
      EditorLevelVisibilityUI.container.add((Sleek) EditorLevelVisibilityUI.navigationToggle);
      EditorLevelVisibilityUI.nodesToggle = new SleekToggle();
      EditorLevelVisibilityUI.nodesToggle.positionOffset_X = -200;
      EditorLevelVisibilityUI.nodesToggle.positionOffset_Y = 180;
      EditorLevelVisibilityUI.nodesToggle.positionScale_X = 1f;
      EditorLevelVisibilityUI.nodesToggle.sizeOffset_X = 40;
      EditorLevelVisibilityUI.nodesToggle.sizeOffset_Y = 40;
      EditorLevelVisibilityUI.nodesToggle.state = LevelVisibility.nodesVisible;
      EditorLevelVisibilityUI.nodesToggle.addLabel(local.format("Nodes_Label"), ESleekSide.RIGHT);
      EditorLevelVisibilityUI.nodesToggle.onToggled = new Toggled(EditorLevelVisibilityUI.onToggledNodesToggle);
      EditorLevelVisibilityUI.container.add((Sleek) EditorLevelVisibilityUI.nodesToggle);
      EditorLevelVisibilityUI.itemsToggle = new SleekToggle();
      EditorLevelVisibilityUI.itemsToggle.positionOffset_X = -200;
      EditorLevelVisibilityUI.itemsToggle.positionOffset_Y = 230;
      EditorLevelVisibilityUI.itemsToggle.positionScale_X = 1f;
      EditorLevelVisibilityUI.itemsToggle.sizeOffset_X = 40;
      EditorLevelVisibilityUI.itemsToggle.sizeOffset_Y = 40;
      EditorLevelVisibilityUI.itemsToggle.state = LevelVisibility.itemsVisible;
      EditorLevelVisibilityUI.itemsToggle.addLabel(local.format("Items_Label"), ESleekSide.RIGHT);
      EditorLevelVisibilityUI.itemsToggle.onToggled = new Toggled(EditorLevelVisibilityUI.onToggledItemsToggle);
      EditorLevelVisibilityUI.container.add((Sleek) EditorLevelVisibilityUI.itemsToggle);
      EditorLevelVisibilityUI.playersToggle = new SleekToggle();
      EditorLevelVisibilityUI.playersToggle.positionOffset_X = -200;
      EditorLevelVisibilityUI.playersToggle.positionOffset_Y = 280;
      EditorLevelVisibilityUI.playersToggle.positionScale_X = 1f;
      EditorLevelVisibilityUI.playersToggle.sizeOffset_X = 40;
      EditorLevelVisibilityUI.playersToggle.sizeOffset_Y = 40;
      EditorLevelVisibilityUI.playersToggle.state = LevelVisibility.playersVisible;
      EditorLevelVisibilityUI.playersToggle.addLabel(local.format("Players_Label"), ESleekSide.RIGHT);
      EditorLevelVisibilityUI.playersToggle.onToggled = new Toggled(EditorLevelVisibilityUI.onToggledPlayersToggle);
      EditorLevelVisibilityUI.container.add((Sleek) EditorLevelVisibilityUI.playersToggle);
      EditorLevelVisibilityUI.zombiesToggle = new SleekToggle();
      EditorLevelVisibilityUI.zombiesToggle.positionOffset_X = -200;
      EditorLevelVisibilityUI.zombiesToggle.positionOffset_Y = 330;
      EditorLevelVisibilityUI.zombiesToggle.positionScale_X = 1f;
      EditorLevelVisibilityUI.zombiesToggle.sizeOffset_X = 40;
      EditorLevelVisibilityUI.zombiesToggle.sizeOffset_Y = 40;
      EditorLevelVisibilityUI.zombiesToggle.state = LevelVisibility.zombiesVisible;
      EditorLevelVisibilityUI.zombiesToggle.addLabel(local.format("Zombies_Label"), ESleekSide.RIGHT);
      EditorLevelVisibilityUI.zombiesToggle.onToggled = new Toggled(EditorLevelVisibilityUI.onToggledZombiesToggle);
      EditorLevelVisibilityUI.container.add((Sleek) EditorLevelVisibilityUI.zombiesToggle);
      EditorLevelVisibilityUI.vehiclesToggle = new SleekToggle();
      EditorLevelVisibilityUI.vehiclesToggle.positionOffset_X = -200;
      EditorLevelVisibilityUI.vehiclesToggle.positionOffset_Y = 380;
      EditorLevelVisibilityUI.vehiclesToggle.positionScale_X = 1f;
      EditorLevelVisibilityUI.vehiclesToggle.sizeOffset_X = 40;
      EditorLevelVisibilityUI.vehiclesToggle.sizeOffset_Y = 40;
      EditorLevelVisibilityUI.vehiclesToggle.state = LevelVisibility.vehiclesVisible;
      EditorLevelVisibilityUI.vehiclesToggle.addLabel(local.format("Vehicles_Label"), ESleekSide.RIGHT);
      EditorLevelVisibilityUI.vehiclesToggle.onToggled = new Toggled(EditorLevelVisibilityUI.onToggledVehiclesToggle);
      EditorLevelVisibilityUI.container.add((Sleek) EditorLevelVisibilityUI.vehiclesToggle);
      EditorLevelVisibilityUI.borderToggle = new SleekToggle();
      EditorLevelVisibilityUI.borderToggle.positionOffset_X = -200;
      EditorLevelVisibilityUI.borderToggle.positionOffset_Y = 430;
      EditorLevelVisibilityUI.borderToggle.positionScale_X = 1f;
      EditorLevelVisibilityUI.borderToggle.sizeOffset_X = 40;
      EditorLevelVisibilityUI.borderToggle.sizeOffset_Y = 40;
      EditorLevelVisibilityUI.borderToggle.state = LevelVisibility.borderVisible;
      EditorLevelVisibilityUI.borderToggle.addLabel(local.format("Border_Label"), ESleekSide.RIGHT);
      EditorLevelVisibilityUI.borderToggle.onToggled = new Toggled(EditorLevelVisibilityUI.onToggledBorderToggle);
      EditorLevelVisibilityUI.container.add((Sleek) EditorLevelVisibilityUI.borderToggle);
      EditorLevelVisibilityUI.animalsToggle = new SleekToggle();
      EditorLevelVisibilityUI.animalsToggle.positionOffset_X = -200;
      EditorLevelVisibilityUI.animalsToggle.positionOffset_Y = 480;
      EditorLevelVisibilityUI.animalsToggle.positionScale_X = 1f;
      EditorLevelVisibilityUI.animalsToggle.sizeOffset_X = 40;
      EditorLevelVisibilityUI.animalsToggle.sizeOffset_Y = 40;
      EditorLevelVisibilityUI.animalsToggle.state = LevelVisibility.animalsVisible;
      EditorLevelVisibilityUI.animalsToggle.addLabel(local.format("Animals_Label"), ESleekSide.RIGHT);
      EditorLevelVisibilityUI.animalsToggle.onToggled = new Toggled(EditorLevelVisibilityUI.onToggledAnimalsToggle);
      EditorLevelVisibilityUI.container.add((Sleek) EditorLevelVisibilityUI.animalsToggle);
    }

    public static void open()
    {
      if (EditorLevelVisibilityUI.active)
      {
        EditorLevelVisibilityUI.close();
      }
      else
      {
        EditorLevelVisibilityUI.active = true;
        EditorUI.message(EEditorMessage.VISIBILITY);
        EditorLevelVisibilityUI.container.lerpPositionScale(0.0f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
      }
    }

    public static void close()
    {
      if (!EditorLevelVisibilityUI.active)
        return;
      EditorLevelVisibilityUI.active = false;
      EditorLevelVisibilityUI.container.lerpPositionScale(1f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
    }

    private static void onToggledRoadsToggle(SleekToggle toggle, bool state)
    {
      LevelVisibility.roadsVisible = state;
    }

    private static void onToggledNavigationToggle(SleekToggle toggle, bool state)
    {
      LevelVisibility.navigationVisible = state;
    }

    private static void onToggledNodesToggle(SleekToggle toggle, bool state)
    {
      LevelVisibility.nodesVisible = state;
    }

    private static void onToggledItemsToggle(SleekToggle toggle, bool state)
    {
      LevelVisibility.itemsVisible = state;
    }

    private static void onToggledPlayersToggle(SleekToggle toggle, bool state)
    {
      LevelVisibility.playersVisible = state;
    }

    private static void onToggledZombiesToggle(SleekToggle toggle, bool state)
    {
      LevelVisibility.zombiesVisible = state;
    }

    private static void onToggledVehiclesToggle(SleekToggle toggle, bool state)
    {
      LevelVisibility.vehiclesVisible = state;
    }

    private static void onToggledBorderToggle(SleekToggle toggle, bool state)
    {
      LevelVisibility.borderVisible = state;
    }

    private static void onToggledAnimalsToggle(SleekToggle toggle, bool state)
    {
      LevelVisibility.animalsVisible = state;
    }
  }
}
