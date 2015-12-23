// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.EditorSpawnsVehiclesUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class EditorSpawnsVehiclesUI
  {
    private static Sleek container;
    public static bool active;
    private static SleekScrollBox tableScrollBox;
    private static SleekScrollBox spawnsScrollBox;
    private static SleekButton[] tableButtons;
    private static SleekButton[] tierButtons;
    private static SleekButton[] vehicleButtons;
    private static SleekColorPicker tableColorPicker;
    private static SleekField tierNameField;
    private static SleekButtonIcon addTierButton;
    private static SleekButtonIcon removeTierButton;
    private static SleekUInt16Field vehicleIDField;
    private static SleekButtonIcon addVehicleButton;
    private static SleekButtonIcon removeVehicleButton;
    private static SleekBox selectedBox;
    private static SleekField tableNameField;
    private static SleekButtonIcon addTableButton;
    private static SleekButtonIcon removeTableButton;
    private static SleekSlider radiusSlider;
    private static SleekSlider rotationSlider;
    private static SleekButtonIcon addButton;
    private static SleekButtonIcon removeButton;
    private static byte selectedTier;
    private static byte selectVehicle;

    public EditorSpawnsVehiclesUI()
    {
      Local local = Localization.read("/Editor/EditorSpawnsVehicles.dat");
      Bundle bundle = Bundles.getBundle("/Bundles/Textures/Edit/Icons/EditorSpawnsVehicles/EditorSpawnsVehicles.unity3d");
      EditorSpawnsVehiclesUI.container = new Sleek();
      EditorSpawnsVehiclesUI.container.positionOffset_X = 10;
      EditorSpawnsVehiclesUI.container.positionOffset_Y = 10;
      EditorSpawnsVehiclesUI.container.positionScale_X = 1f;
      EditorSpawnsVehiclesUI.container.sizeOffset_X = -20;
      EditorSpawnsVehiclesUI.container.sizeOffset_Y = -20;
      EditorSpawnsVehiclesUI.container.sizeScale_X = 1f;
      EditorSpawnsVehiclesUI.container.sizeScale_Y = 1f;
      EditorUI.window.add(EditorSpawnsVehiclesUI.container);
      EditorSpawnsVehiclesUI.active = false;
      EditorSpawnsVehiclesUI.tableScrollBox = new SleekScrollBox();
      EditorSpawnsVehiclesUI.tableScrollBox.positionOffset_X = -470;
      EditorSpawnsVehiclesUI.tableScrollBox.positionOffset_Y = 120;
      EditorSpawnsVehiclesUI.tableScrollBox.positionScale_X = 1f;
      EditorSpawnsVehiclesUI.tableScrollBox.sizeOffset_X = 470;
      EditorSpawnsVehiclesUI.tableScrollBox.sizeOffset_Y = 100;
      EditorSpawnsVehiclesUI.container.add((Sleek) EditorSpawnsVehiclesUI.tableScrollBox);
      EditorSpawnsVehiclesUI.tableNameField = new SleekField();
      EditorSpawnsVehiclesUI.tableNameField.positionOffset_X = -230;
      EditorSpawnsVehiclesUI.tableNameField.positionOffset_Y = 230;
      EditorSpawnsVehiclesUI.tableNameField.positionScale_X = 1f;
      EditorSpawnsVehiclesUI.tableNameField.sizeOffset_X = 230;
      EditorSpawnsVehiclesUI.tableNameField.sizeOffset_Y = 30;
      EditorSpawnsVehiclesUI.tableNameField.addLabel(local.format("TableNameFieldLabelText"), ESleekSide.LEFT);
      EditorSpawnsVehiclesUI.container.add((Sleek) EditorSpawnsVehiclesUI.tableNameField);
      EditorSpawnsVehiclesUI.addTableButton = new SleekButtonIcon((Texture2D) bundle.load("Add"));
      EditorSpawnsVehiclesUI.addTableButton.positionOffset_X = -230;
      EditorSpawnsVehiclesUI.addTableButton.positionOffset_Y = 270;
      EditorSpawnsVehiclesUI.addTableButton.positionScale_X = 1f;
      EditorSpawnsVehiclesUI.addTableButton.sizeOffset_X = 110;
      EditorSpawnsVehiclesUI.addTableButton.sizeOffset_Y = 30;
      EditorSpawnsVehiclesUI.addTableButton.text = local.format("AddTableButtonText");
      EditorSpawnsVehiclesUI.addTableButton.tooltip = local.format("AddTableButtonTooltip");
      EditorSpawnsVehiclesUI.addTableButton.onClickedButton = new ClickedButton(EditorSpawnsVehiclesUI.onClickedAddTableButton);
      EditorSpawnsVehiclesUI.container.add((Sleek) EditorSpawnsVehiclesUI.addTableButton);
      EditorSpawnsVehiclesUI.removeTableButton = new SleekButtonIcon((Texture2D) bundle.load("Remove"));
      EditorSpawnsVehiclesUI.removeTableButton.positionOffset_X = -110;
      EditorSpawnsVehiclesUI.removeTableButton.positionOffset_Y = 270;
      EditorSpawnsVehiclesUI.removeTableButton.positionScale_X = 1f;
      EditorSpawnsVehiclesUI.removeTableButton.sizeOffset_X = 110;
      EditorSpawnsVehiclesUI.removeTableButton.sizeOffset_Y = 30;
      EditorSpawnsVehiclesUI.removeTableButton.text = local.format("RemoveTableButtonText");
      EditorSpawnsVehiclesUI.removeTableButton.tooltip = local.format("RemoveTableButtonTooltip");
      EditorSpawnsVehiclesUI.removeTableButton.onClickedButton = new ClickedButton(EditorSpawnsVehiclesUI.onClickedRemoveTableButton);
      EditorSpawnsVehiclesUI.container.add((Sleek) EditorSpawnsVehiclesUI.removeTableButton);
      EditorSpawnsVehiclesUI.updateTables();
      EditorSpawnsVehiclesUI.spawnsScrollBox = new SleekScrollBox();
      EditorSpawnsVehiclesUI.spawnsScrollBox.positionOffset_X = -470;
      EditorSpawnsVehiclesUI.spawnsScrollBox.positionOffset_Y = 310;
      EditorSpawnsVehiclesUI.spawnsScrollBox.positionScale_X = 1f;
      EditorSpawnsVehiclesUI.spawnsScrollBox.sizeOffset_X = 470;
      EditorSpawnsVehiclesUI.spawnsScrollBox.sizeOffset_Y = -310;
      EditorSpawnsVehiclesUI.spawnsScrollBox.sizeScale_Y = 1f;
      EditorSpawnsVehiclesUI.spawnsScrollBox.area = new Rect(0.0f, 0.0f, 5f, 1000f);
      EditorSpawnsVehiclesUI.container.add((Sleek) EditorSpawnsVehiclesUI.spawnsScrollBox);
      EditorSpawnsVehiclesUI.tableColorPicker = new SleekColorPicker();
      EditorSpawnsVehiclesUI.tableColorPicker.positionOffset_X = 200;
      EditorSpawnsVehiclesUI.tableColorPicker.onColorPicked = new ColorPicked(EditorSpawnsVehiclesUI.onVehicleColorPicked);
      EditorSpawnsVehiclesUI.spawnsScrollBox.add((Sleek) EditorSpawnsVehiclesUI.tableColorPicker);
      EditorSpawnsVehiclesUI.tierNameField = new SleekField();
      EditorSpawnsVehiclesUI.tierNameField.positionOffset_X = 240;
      EditorSpawnsVehiclesUI.tierNameField.sizeOffset_X = 200;
      EditorSpawnsVehiclesUI.tierNameField.sizeOffset_Y = 30;
      EditorSpawnsVehiclesUI.tierNameField.addLabel(local.format("TierNameFieldLabelText"), ESleekSide.LEFT);
      EditorSpawnsVehiclesUI.spawnsScrollBox.add((Sleek) EditorSpawnsVehiclesUI.tierNameField);
      EditorSpawnsVehiclesUI.addTierButton = new SleekButtonIcon((Texture2D) bundle.load("Add"));
      EditorSpawnsVehiclesUI.addTierButton.positionOffset_X = 240;
      EditorSpawnsVehiclesUI.addTierButton.sizeOffset_X = 95;
      EditorSpawnsVehiclesUI.addTierButton.sizeOffset_Y = 30;
      EditorSpawnsVehiclesUI.addTierButton.text = local.format("AddTierButtonText");
      EditorSpawnsVehiclesUI.addTierButton.tooltip = local.format("AddTierButtonTooltip");
      EditorSpawnsVehiclesUI.addTierButton.onClickedButton = new ClickedButton(EditorSpawnsVehiclesUI.onClickedAddTierButton);
      EditorSpawnsVehiclesUI.spawnsScrollBox.add((Sleek) EditorSpawnsVehiclesUI.addTierButton);
      EditorSpawnsVehiclesUI.removeTierButton = new SleekButtonIcon((Texture2D) bundle.load("Remove"));
      EditorSpawnsVehiclesUI.removeTierButton.positionOffset_X = 345;
      EditorSpawnsVehiclesUI.removeTierButton.sizeOffset_X = 95;
      EditorSpawnsVehiclesUI.removeTierButton.sizeOffset_Y = 30;
      EditorSpawnsVehiclesUI.removeTierButton.text = local.format("RemoveTierButtonText");
      EditorSpawnsVehiclesUI.removeTierButton.tooltip = local.format("RemoveTierButtonTooltip");
      EditorSpawnsVehiclesUI.removeTierButton.onClickedButton = new ClickedButton(EditorSpawnsVehiclesUI.onClickedRemoveTierButton);
      EditorSpawnsVehiclesUI.spawnsScrollBox.add((Sleek) EditorSpawnsVehiclesUI.removeTierButton);
      EditorSpawnsVehiclesUI.vehicleIDField = new SleekUInt16Field();
      EditorSpawnsVehiclesUI.vehicleIDField.positionOffset_X = 240;
      EditorSpawnsVehiclesUI.vehicleIDField.sizeOffset_X = 200;
      EditorSpawnsVehiclesUI.vehicleIDField.sizeOffset_Y = 30;
      EditorSpawnsVehiclesUI.vehicleIDField.addLabel(local.format("VehicleIDFieldLabelText"), ESleekSide.LEFT);
      EditorSpawnsVehiclesUI.spawnsScrollBox.add((Sleek) EditorSpawnsVehiclesUI.vehicleIDField);
      EditorSpawnsVehiclesUI.addVehicleButton = new SleekButtonIcon((Texture2D) bundle.load("Add"));
      EditorSpawnsVehiclesUI.addVehicleButton.positionOffset_X = 240;
      EditorSpawnsVehiclesUI.addVehicleButton.sizeOffset_X = 95;
      EditorSpawnsVehiclesUI.addVehicleButton.sizeOffset_Y = 30;
      EditorSpawnsVehiclesUI.addVehicleButton.text = local.format("AddVehicleButtonText");
      EditorSpawnsVehiclesUI.addVehicleButton.tooltip = local.format("AddVehicleButtonTooltip");
      EditorSpawnsVehiclesUI.addVehicleButton.onClickedButton = new ClickedButton(EditorSpawnsVehiclesUI.onClickedAddVehicleButton);
      EditorSpawnsVehiclesUI.spawnsScrollBox.add((Sleek) EditorSpawnsVehiclesUI.addVehicleButton);
      EditorSpawnsVehiclesUI.removeVehicleButton = new SleekButtonIcon((Texture2D) bundle.load("Remove"));
      EditorSpawnsVehiclesUI.removeVehicleButton.positionOffset_X = 345;
      EditorSpawnsVehiclesUI.removeVehicleButton.sizeOffset_X = 95;
      EditorSpawnsVehiclesUI.removeVehicleButton.sizeOffset_Y = 30;
      EditorSpawnsVehiclesUI.removeVehicleButton.text = local.format("RemoveVehicleButtonText");
      EditorSpawnsVehiclesUI.removeVehicleButton.tooltip = local.format("RemoveVehicleButtonTooltip");
      EditorSpawnsVehiclesUI.removeVehicleButton.onClickedButton = new ClickedButton(EditorSpawnsVehiclesUI.onClickedRemoveVehicleButton);
      EditorSpawnsVehiclesUI.spawnsScrollBox.add((Sleek) EditorSpawnsVehiclesUI.removeVehicleButton);
      EditorSpawnsVehiclesUI.selectedBox = new SleekBox();
      EditorSpawnsVehiclesUI.selectedBox.positionOffset_X = -230;
      EditorSpawnsVehiclesUI.selectedBox.positionOffset_Y = 80;
      EditorSpawnsVehiclesUI.selectedBox.positionScale_X = 1f;
      EditorSpawnsVehiclesUI.selectedBox.sizeOffset_X = 230;
      EditorSpawnsVehiclesUI.selectedBox.sizeOffset_Y = 30;
      EditorSpawnsVehiclesUI.selectedBox.addLabel(local.format("SelectionBoxLabelText"), ESleekSide.LEFT);
      EditorSpawnsVehiclesUI.container.add((Sleek) EditorSpawnsVehiclesUI.selectedBox);
      EditorSpawnsVehiclesUI.updateSelection();
      EditorSpawnsVehiclesUI.radiusSlider = new SleekSlider();
      EditorSpawnsVehiclesUI.radiusSlider.positionOffset_Y = -130;
      EditorSpawnsVehiclesUI.radiusSlider.positionScale_Y = 1f;
      EditorSpawnsVehiclesUI.radiusSlider.sizeOffset_X = 200;
      EditorSpawnsVehiclesUI.radiusSlider.sizeOffset_Y = 20;
      EditorSpawnsVehiclesUI.radiusSlider.state = (float) ((int) EditorSpawns.radius - (int) EditorSpawns.MIN_REMOVE_SIZE) / (float) EditorSpawns.MAX_REMOVE_SIZE;
      EditorSpawnsVehiclesUI.radiusSlider.orientation = ESleekOrientation.HORIZONTAL;
      EditorSpawnsVehiclesUI.radiusSlider.addLabel(local.format("RadiusSliderLabelText"), ESleekSide.RIGHT);
      EditorSpawnsVehiclesUI.radiusSlider.onDragged = new Dragged(EditorSpawnsVehiclesUI.onDraggedRadiusSlider);
      EditorSpawnsVehiclesUI.container.add((Sleek) EditorSpawnsVehiclesUI.radiusSlider);
      EditorSpawnsVehiclesUI.rotationSlider = new SleekSlider();
      EditorSpawnsVehiclesUI.rotationSlider.positionOffset_Y = -100;
      EditorSpawnsVehiclesUI.rotationSlider.positionScale_Y = 1f;
      EditorSpawnsVehiclesUI.rotationSlider.sizeOffset_X = 200;
      EditorSpawnsVehiclesUI.rotationSlider.sizeOffset_Y = 20;
      EditorSpawnsVehiclesUI.rotationSlider.state = EditorSpawns.rotation / 360f;
      EditorSpawnsVehiclesUI.rotationSlider.orientation = ESleekOrientation.HORIZONTAL;
      EditorSpawnsVehiclesUI.rotationSlider.addLabel(local.format("RotationSliderLabelText"), ESleekSide.RIGHT);
      EditorSpawnsVehiclesUI.rotationSlider.onDragged = new Dragged(EditorSpawnsVehiclesUI.onDraggedRotationSlider);
      EditorSpawnsVehiclesUI.container.add((Sleek) EditorSpawnsVehiclesUI.rotationSlider);
      EditorSpawnsVehiclesUI.addButton = new SleekButtonIcon((Texture2D) bundle.load("Add"));
      EditorSpawnsVehiclesUI.addButton.positionOffset_Y = -70;
      EditorSpawnsVehiclesUI.addButton.positionScale_Y = 1f;
      EditorSpawnsVehiclesUI.addButton.sizeOffset_X = 200;
      EditorSpawnsVehiclesUI.addButton.sizeOffset_Y = 30;
      EditorSpawnsVehiclesUI.addButton.text = local.format("AddButtonText", (object) ControlsSettings.tool_0);
      EditorSpawnsVehiclesUI.addButton.tooltip = local.format("AddButtonTooltip");
      EditorSpawnsVehiclesUI.addButton.onClickedButton = new ClickedButton(EditorSpawnsVehiclesUI.onClickedAddButton);
      EditorSpawnsVehiclesUI.container.add((Sleek) EditorSpawnsVehiclesUI.addButton);
      EditorSpawnsVehiclesUI.removeButton = new SleekButtonIcon((Texture2D) bundle.load("Remove"));
      EditorSpawnsVehiclesUI.removeButton.positionOffset_Y = -30;
      EditorSpawnsVehiclesUI.removeButton.positionScale_Y = 1f;
      EditorSpawnsVehiclesUI.removeButton.sizeOffset_X = 200;
      EditorSpawnsVehiclesUI.removeButton.sizeOffset_Y = 30;
      EditorSpawnsVehiclesUI.removeButton.text = local.format("RemoveButtonText", (object) ControlsSettings.tool_1);
      EditorSpawnsVehiclesUI.removeButton.tooltip = local.format("RemoveButtonTooltip");
      EditorSpawnsVehiclesUI.removeButton.onClickedButton = new ClickedButton(EditorSpawnsVehiclesUI.onClickedRemoveButton);
      EditorSpawnsVehiclesUI.container.add((Sleek) EditorSpawnsVehiclesUI.removeButton);
      bundle.unload();
    }

    public static void open()
    {
      if (EditorSpawnsVehiclesUI.active)
      {
        EditorSpawnsVehiclesUI.close();
      }
      else
      {
        EditorSpawnsVehiclesUI.active = true;
        EditorSpawns.isSpawning = true;
        EditorSpawns.spawnMode = ESpawnMode.ADD_VEHICLE;
        EditorSpawnsVehiclesUI.container.lerpPositionScale(0.0f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
      }
    }

    public static void close()
    {
      if (!EditorSpawnsVehiclesUI.active)
        return;
      EditorSpawnsVehiclesUI.active = false;
      EditorSpawns.isSpawning = false;
      EditorSpawnsVehiclesUI.container.lerpPositionScale(1f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
    }

    public static void updateTables()
    {
      if (EditorSpawnsVehiclesUI.tableButtons != null)
      {
        for (int index = 0; index < EditorSpawnsVehiclesUI.tableButtons.Length; ++index)
          EditorSpawnsVehiclesUI.tableScrollBox.remove((Sleek) EditorSpawnsVehiclesUI.tableButtons[index]);
      }
      EditorSpawnsVehiclesUI.tableButtons = new SleekButton[LevelVehicles.tables.Count];
      EditorSpawnsVehiclesUI.tableScrollBox.area = new Rect(0.0f, 0.0f, 5f, (float) (EditorSpawnsVehiclesUI.tableButtons.Length * 40 - 10));
      for (int index = 0; index < EditorSpawnsVehiclesUI.tableButtons.Length; ++index)
      {
        SleekButton sleekButton = new SleekButton();
        sleekButton.positionOffset_X = 240;
        sleekButton.positionOffset_Y = index * 40;
        sleekButton.sizeOffset_X = 200;
        sleekButton.sizeOffset_Y = 30;
        sleekButton.text = (string) (object) index + (object) " " + LevelVehicles.tables[index].name;
        sleekButton.onClickedButton = new ClickedButton(EditorSpawnsVehiclesUI.onClickedTableButton);
        EditorSpawnsVehiclesUI.tableScrollBox.add((Sleek) sleekButton);
        EditorSpawnsVehiclesUI.tableButtons[index] = sleekButton;
      }
    }

    public static void updateSelection()
    {
      if ((int) EditorSpawns.selectedVehicle < LevelVehicles.tables.Count)
      {
        VehicleTable vehicleTable = LevelVehicles.tables[(int) EditorSpawns.selectedVehicle];
        EditorSpawnsVehiclesUI.selectedBox.text = vehicleTable.name;
        EditorSpawnsVehiclesUI.tableColorPicker.state = vehicleTable.color;
        if (EditorSpawnsVehiclesUI.tierButtons != null)
        {
          for (int index = 0; index < EditorSpawnsVehiclesUI.tierButtons.Length; ++index)
            EditorSpawnsVehiclesUI.spawnsScrollBox.remove((Sleek) EditorSpawnsVehiclesUI.tierButtons[index]);
        }
        EditorSpawnsVehiclesUI.tierButtons = new SleekButton[vehicleTable.tiers.Count];
        for (int index = 0; index < EditorSpawnsVehiclesUI.tierButtons.Length; ++index)
        {
          VehicleTier vehicleTier = vehicleTable.tiers[index];
          SleekButton sleekButton = new SleekButton();
          sleekButton.positionOffset_X = 240;
          sleekButton.positionOffset_Y = 130 + index * 70;
          sleekButton.sizeOffset_X = 200;
          sleekButton.sizeOffset_Y = 30;
          sleekButton.text = vehicleTier.name;
          sleekButton.onClickedButton = new ClickedButton(EditorSpawnsVehiclesUI.onClickedTierButton);
          EditorSpawnsVehiclesUI.spawnsScrollBox.add((Sleek) sleekButton);
          SleekSlider sleekSlider = new SleekSlider();
          sleekSlider.positionOffset_Y = 40;
          sleekSlider.sizeOffset_X = 200;
          sleekSlider.sizeOffset_Y = 20;
          sleekSlider.orientation = ESleekOrientation.HORIZONTAL;
          sleekSlider.state = vehicleTier.chance;
          sleekSlider.addLabel((string) (object) Mathf.Floor(vehicleTier.chance * 100f) + (object) "%", ESleekSide.LEFT);
          sleekSlider.onDragged = new Dragged(EditorSpawnsVehiclesUI.onDraggedChanceSlider);
          sleekButton.add((Sleek) sleekSlider);
          EditorSpawnsVehiclesUI.tierButtons[index] = sleekButton;
        }
        EditorSpawnsVehiclesUI.tierNameField.positionOffset_Y = 130 + EditorSpawnsVehiclesUI.tierButtons.Length * 70;
        EditorSpawnsVehiclesUI.addTierButton.positionOffset_Y = 130 + EditorSpawnsVehiclesUI.tierButtons.Length * 70 + 40;
        EditorSpawnsVehiclesUI.removeTierButton.positionOffset_Y = 130 + EditorSpawnsVehiclesUI.tierButtons.Length * 70 + 40;
        if (EditorSpawnsVehiclesUI.vehicleButtons != null)
        {
          for (int index = 0; index < EditorSpawnsVehiclesUI.vehicleButtons.Length; ++index)
            EditorSpawnsVehiclesUI.spawnsScrollBox.remove((Sleek) EditorSpawnsVehiclesUI.vehicleButtons[index]);
        }
        if ((int) EditorSpawnsVehiclesUI.selectedTier < vehicleTable.tiers.Count)
        {
          EditorSpawnsVehiclesUI.vehicleButtons = new SleekButton[vehicleTable.tiers[(int) EditorSpawnsVehiclesUI.selectedTier].table.Count];
          for (int index = 0; index < EditorSpawnsVehiclesUI.vehicleButtons.Length; ++index)
          {
            SleekButton sleekButton = new SleekButton();
            sleekButton.positionOffset_X = 240;
            sleekButton.positionOffset_Y = 130 + EditorSpawnsVehiclesUI.tierButtons.Length * 70 + 80 + index * 40;
            sleekButton.sizeOffset_X = 200;
            sleekButton.sizeOffset_Y = 30;
            VehicleAsset vehicleAsset = (VehicleAsset) Assets.find(EAssetType.VEHICLE, vehicleTable.tiers[(int) EditorSpawnsVehiclesUI.selectedTier].table[index].vehicle);
            if (vehicleAsset != null)
              sleekButton.text = (string) (object) vehicleTable.tiers[(int) EditorSpawnsVehiclesUI.selectedTier].table[index].vehicle + (object) " " + vehicleAsset.vehicleName;
            else
              sleekButton.text = vehicleTable.tiers[(int) EditorSpawnsVehiclesUI.selectedTier].table[index].vehicle.ToString();
            sleekButton.onClickedButton = new ClickedButton(EditorSpawnsVehiclesUI.onClickVehicleButton);
            EditorSpawnsVehiclesUI.spawnsScrollBox.add((Sleek) sleekButton);
            EditorSpawnsVehiclesUI.vehicleButtons[index] = sleekButton;
          }
        }
        else
          EditorSpawnsVehiclesUI.vehicleButtons = new SleekButton[0];
        EditorSpawnsVehiclesUI.vehicleIDField.positionOffset_Y = 130 + EditorSpawnsVehiclesUI.tierButtons.Length * 70 + 80 + EditorSpawnsVehiclesUI.vehicleButtons.Length * 40;
        EditorSpawnsVehiclesUI.addVehicleButton.positionOffset_Y = 130 + EditorSpawnsVehiclesUI.tierButtons.Length * 70 + 80 + EditorSpawnsVehiclesUI.vehicleButtons.Length * 40 + 40;
        EditorSpawnsVehiclesUI.removeVehicleButton.positionOffset_Y = 130 + EditorSpawnsVehiclesUI.tierButtons.Length * 70 + 80 + EditorSpawnsVehiclesUI.vehicleButtons.Length * 40 + 40;
        EditorSpawnsVehiclesUI.spawnsScrollBox.area = new Rect(0.0f, 0.0f, 5f, (float) (130 + EditorSpawnsVehiclesUI.tierButtons.Length * 70 + 80 + EditorSpawnsVehiclesUI.vehicleButtons.Length * 40 + 70));
      }
      else
      {
        EditorSpawnsVehiclesUI.selectedBox.text = string.Empty;
        EditorSpawnsVehiclesUI.tableColorPicker.state = Color.white;
        if (EditorSpawnsVehiclesUI.tierButtons != null)
        {
          for (int index = 0; index < EditorSpawnsVehiclesUI.tierButtons.Length; ++index)
            EditorSpawnsVehiclesUI.spawnsScrollBox.remove((Sleek) EditorSpawnsVehiclesUI.tierButtons[index]);
        }
        EditorSpawnsVehiclesUI.tierButtons = (SleekButton[]) null;
        EditorSpawnsVehiclesUI.tierNameField.positionOffset_Y = 130;
        EditorSpawnsVehiclesUI.addTierButton.positionOffset_Y = 170;
        EditorSpawnsVehiclesUI.removeTierButton.positionOffset_Y = 170;
        if (EditorSpawnsVehiclesUI.vehicleButtons != null)
        {
          for (int index = 0; index < EditorSpawnsVehiclesUI.vehicleButtons.Length; ++index)
            EditorSpawnsVehiclesUI.spawnsScrollBox.remove((Sleek) EditorSpawnsVehiclesUI.vehicleButtons[index]);
        }
        EditorSpawnsVehiclesUI.vehicleButtons = (SleekButton[]) null;
        EditorSpawnsVehiclesUI.vehicleIDField.positionOffset_Y = 210;
        EditorSpawnsVehiclesUI.addVehicleButton.positionOffset_Y = 250;
        EditorSpawnsVehiclesUI.removeVehicleButton.positionOffset_Y = 250;
        EditorSpawnsVehiclesUI.spawnsScrollBox.area = new Rect(0.0f, 0.0f, 5f, 280f);
      }
    }

    private static void onClickedTableButton(SleekButton button)
    {
      EditorSpawns.selectedVehicle = (byte) (button.positionOffset_Y / 40);
      EditorSpawns.vehicleSpawn.GetComponent<Renderer>().material.color = LevelVehicles.tables[(int) EditorSpawns.selectedVehicle].color;
      EditorSpawns.vehicleSpawn.FindChild("Arrow").GetComponent<Renderer>().material.color = LevelVehicles.tables[(int) EditorSpawns.selectedVehicle].color;
      EditorSpawnsVehiclesUI.updateSelection();
    }

    private static void onVehicleColorPicked(SleekColorPicker picker, Color color)
    {
      if ((int) EditorSpawns.selectedVehicle >= LevelVehicles.tables.Count)
        return;
      LevelVehicles.tables[(int) EditorSpawns.selectedVehicle].color = color;
    }

    private static void onClickedTierButton(SleekButton button)
    {
      if ((int) EditorSpawns.selectedVehicle >= LevelVehicles.tables.Count)
        return;
      EditorSpawnsVehiclesUI.selectedTier = (byte) ((button.positionOffset_Y - 130) / 70);
      EditorSpawnsVehiclesUI.updateSelection();
    }

    private static void onClickVehicleButton(SleekButton button)
    {
      if ((int) EditorSpawns.selectedVehicle >= LevelVehicles.tables.Count)
        return;
      EditorSpawnsVehiclesUI.selectVehicle = (byte) ((button.positionOffset_Y - 130 - EditorSpawnsVehiclesUI.tierButtons.Length * 70 - 80) / 40);
      EditorSpawnsVehiclesUI.updateSelection();
    }

    private static void onDraggedChanceSlider(SleekSlider slider, float state)
    {
      if ((int) EditorSpawns.selectedVehicle >= LevelVehicles.tables.Count)
        return;
      int tierIndex = (slider.parent.positionOffset_Y - 130) / 70;
      LevelVehicles.tables[(int) EditorSpawns.selectedVehicle].updateChance(tierIndex, state);
      for (int index = 0; index < LevelVehicles.tables[(int) EditorSpawns.selectedVehicle].tiers.Count; ++index)
      {
        VehicleTier vehicleTier = LevelVehicles.tables[(int) EditorSpawns.selectedVehicle].tiers[index];
        SleekSlider sleekSlider = (SleekSlider) EditorSpawnsVehiclesUI.tierButtons[index].children[0];
        if (index != tierIndex)
          sleekSlider.state = vehicleTier.chance;
        sleekSlider.updateLabel((string) (object) Mathf.Floor(vehicleTier.chance * 100f) + (object) "%");
      }
    }

    private static void onClickedAddTableButton(SleekButton button)
    {
      if (!(EditorSpawnsVehiclesUI.tableNameField.text != string.Empty))
        return;
      LevelVehicles.addTable(EditorSpawnsVehiclesUI.tableNameField.text);
      EditorSpawnsVehiclesUI.tableNameField.text = string.Empty;
      EditorSpawnsVehiclesUI.updateTables();
      EditorSpawnsVehiclesUI.tableScrollBox.state = new Vector2(0.0f, float.MaxValue);
    }

    private static void onClickedRemoveTableButton(SleekButton button)
    {
      if ((int) EditorSpawns.selectedVehicle >= LevelVehicles.tables.Count)
        return;
      LevelVehicles.removeTable();
      EditorSpawnsVehiclesUI.updateTables();
      EditorSpawnsVehiclesUI.updateSelection();
      EditorSpawnsVehiclesUI.tableScrollBox.state = new Vector2(0.0f, float.MaxValue);
    }

    private static void onClickedAddTierButton(SleekButton button)
    {
      if ((int) EditorSpawns.selectedVehicle >= LevelVehicles.tables.Count || !(EditorSpawnsVehiclesUI.tierNameField.text != string.Empty))
        return;
      LevelVehicles.tables[(int) EditorSpawns.selectedVehicle].addTier(EditorSpawnsVehiclesUI.tierNameField.text);
      EditorSpawnsVehiclesUI.tierNameField.text = string.Empty;
      EditorSpawnsVehiclesUI.updateSelection();
    }

    private static void onClickedRemoveTierButton(SleekButton button)
    {
      if ((int) EditorSpawns.selectedVehicle >= LevelVehicles.tables.Count || (int) EditorSpawnsVehiclesUI.selectedTier >= LevelVehicles.tables[(int) EditorSpawns.selectedVehicle].tiers.Count)
        return;
      LevelVehicles.tables[(int) EditorSpawns.selectedVehicle].removeTier((int) EditorSpawnsVehiclesUI.selectedTier);
      EditorSpawnsVehiclesUI.updateSelection();
    }

    private static void onClickedAddVehicleButton(SleekButton button)
    {
      if ((int) EditorSpawns.selectedVehicle >= LevelVehicles.tables.Count || (int) EditorSpawnsVehiclesUI.selectedTier >= LevelVehicles.tables[(int) EditorSpawns.selectedVehicle].tiers.Count)
        return;
      if ((VehicleAsset) Assets.find(EAssetType.VEHICLE, EditorSpawnsVehiclesUI.vehicleIDField.state) != null)
      {
        LevelVehicles.tables[(int) EditorSpawns.selectedVehicle].addVehicle(EditorSpawnsVehiclesUI.selectedTier, EditorSpawnsVehiclesUI.vehicleIDField.state);
        EditorSpawnsVehiclesUI.updateSelection();
        EditorSpawnsVehiclesUI.spawnsScrollBox.state = new Vector2(0.0f, float.MaxValue);
      }
      EditorSpawnsVehiclesUI.vehicleIDField.state = (ushort) 0;
    }

    private static void onClickedRemoveVehicleButton(SleekButton button)
    {
      if ((int) EditorSpawns.selectedVehicle >= LevelVehicles.tables.Count || (int) EditorSpawnsVehiclesUI.selectedTier >= LevelVehicles.tables[(int) EditorSpawns.selectedVehicle].tiers.Count || (int) EditorSpawnsVehiclesUI.selectVehicle >= LevelVehicles.tables[(int) EditorSpawns.selectedVehicle].tiers[(int) EditorSpawnsVehiclesUI.selectedTier].table.Count)
        return;
      LevelVehicles.tables[(int) EditorSpawns.selectedVehicle].removeVehicle(EditorSpawnsVehiclesUI.selectedTier, EditorSpawnsVehiclesUI.selectVehicle);
      EditorSpawnsVehiclesUI.updateSelection();
      EditorSpawnsVehiclesUI.spawnsScrollBox.state = new Vector2(0.0f, float.MaxValue);
    }

    private static void onDraggedRadiusSlider(SleekSlider slider, float state)
    {
      EditorSpawns.radius = (byte) ((double) EditorSpawns.MIN_REMOVE_SIZE + (double) state * (double) EditorSpawns.MAX_REMOVE_SIZE);
    }

    private static void onDraggedRotationSlider(SleekSlider slider, float state)
    {
      EditorSpawns.rotation = state * 360f;
    }

    private static void onClickedAddButton(SleekButton button)
    {
      EditorSpawns.spawnMode = ESpawnMode.ADD_VEHICLE;
    }

    private static void onClickedRemoveButton(SleekButton button)
    {
      EditorSpawns.spawnMode = ESpawnMode.REMOVE_VEHICLE;
    }
  }
}
