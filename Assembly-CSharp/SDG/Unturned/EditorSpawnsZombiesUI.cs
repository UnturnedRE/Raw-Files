// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.EditorSpawnsZombiesUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class EditorSpawnsZombiesUI
  {
    private static Sleek container;
    private static Local localization;
    public static bool active;
    private static SleekScrollBox tableScrollBox;
    private static SleekScrollBox spawnsScrollBox;
    private static SleekButton[] tableButtons;
    private static SleekButton[] slotButtons;
    private static SleekButton[] clothButtons;
    private static SleekColorPicker tableColorPicker;
    private static SleekToggle megaToggle;
    private static SleekUInt16Field healthField;
    private static SleekByteField damageField;
    private static SleekByteField lootIDField;
    private static SleekUInt16Field itemIDField;
    private static SleekButtonIcon addItemButton;
    private static SleekButtonIcon removeItemButton;
    private static SleekBox selectedBox;
    private static SleekField tableNameField;
    private static SleekButtonIcon addTableButton;
    private static SleekButtonIcon removeTableButton;
    private static SleekSlider radiusSlider;
    private static SleekButtonIcon addButton;
    private static SleekButtonIcon removeButton;
    private static byte selectedSlot;
    private static byte selectItem;

    public EditorSpawnsZombiesUI()
    {
      EditorSpawnsZombiesUI.localization = Localization.read("/Editor/EditorSpawnsZombies.dat");
      Bundle bundle = Bundles.getBundle("/Bundles/Textures/Edit/Icons/EditorSpawnsZombies/EditorSpawnsZombies.unity3d");
      EditorSpawnsZombiesUI.container = new Sleek();
      EditorSpawnsZombiesUI.container.positionOffset_X = 10;
      EditorSpawnsZombiesUI.container.positionOffset_Y = 10;
      EditorSpawnsZombiesUI.container.positionScale_X = 1f;
      EditorSpawnsZombiesUI.container.sizeOffset_X = -20;
      EditorSpawnsZombiesUI.container.sizeOffset_Y = -20;
      EditorSpawnsZombiesUI.container.sizeScale_X = 1f;
      EditorSpawnsZombiesUI.container.sizeScale_Y = 1f;
      EditorUI.window.add(EditorSpawnsZombiesUI.container);
      EditorSpawnsZombiesUI.active = false;
      EditorSpawnsZombiesUI.tableScrollBox = new SleekScrollBox();
      EditorSpawnsZombiesUI.tableScrollBox.positionOffset_X = -470;
      EditorSpawnsZombiesUI.tableScrollBox.positionOffset_Y = 120;
      EditorSpawnsZombiesUI.tableScrollBox.positionScale_X = 1f;
      EditorSpawnsZombiesUI.tableScrollBox.sizeOffset_X = 470;
      EditorSpawnsZombiesUI.tableScrollBox.sizeOffset_Y = 100;
      EditorSpawnsZombiesUI.container.add((Sleek) EditorSpawnsZombiesUI.tableScrollBox);
      EditorSpawnsZombiesUI.tableNameField = new SleekField();
      EditorSpawnsZombiesUI.tableNameField.positionOffset_X = -230;
      EditorSpawnsZombiesUI.tableNameField.positionOffset_Y = 230;
      EditorSpawnsZombiesUI.tableNameField.positionScale_X = 1f;
      EditorSpawnsZombiesUI.tableNameField.sizeOffset_X = 230;
      EditorSpawnsZombiesUI.tableNameField.sizeOffset_Y = 30;
      EditorSpawnsZombiesUI.tableNameField.addLabel(EditorSpawnsZombiesUI.localization.format("TableNameFieldLabelText"), ESleekSide.LEFT);
      EditorSpawnsZombiesUI.container.add((Sleek) EditorSpawnsZombiesUI.tableNameField);
      EditorSpawnsZombiesUI.addTableButton = new SleekButtonIcon((Texture2D) bundle.load("Add"));
      EditorSpawnsZombiesUI.addTableButton.positionOffset_X = -230;
      EditorSpawnsZombiesUI.addTableButton.positionOffset_Y = 270;
      EditorSpawnsZombiesUI.addTableButton.positionScale_X = 1f;
      EditorSpawnsZombiesUI.addTableButton.sizeOffset_X = 110;
      EditorSpawnsZombiesUI.addTableButton.sizeOffset_Y = 30;
      EditorSpawnsZombiesUI.addTableButton.text = EditorSpawnsZombiesUI.localization.format("AddTableButtonText");
      EditorSpawnsZombiesUI.addTableButton.tooltip = EditorSpawnsZombiesUI.localization.format("AddTableButtonTooltip");
      EditorSpawnsZombiesUI.addTableButton.onClickedButton = new ClickedButton(EditorSpawnsZombiesUI.onClickedAddTableButton);
      EditorSpawnsZombiesUI.container.add((Sleek) EditorSpawnsZombiesUI.addTableButton);
      EditorSpawnsZombiesUI.removeTableButton = new SleekButtonIcon((Texture2D) bundle.load("Remove"));
      EditorSpawnsZombiesUI.removeTableButton.positionOffset_X = -110;
      EditorSpawnsZombiesUI.removeTableButton.positionOffset_Y = 270;
      EditorSpawnsZombiesUI.removeTableButton.positionScale_X = 1f;
      EditorSpawnsZombiesUI.removeTableButton.sizeOffset_X = 110;
      EditorSpawnsZombiesUI.removeTableButton.sizeOffset_Y = 30;
      EditorSpawnsZombiesUI.removeTableButton.text = EditorSpawnsZombiesUI.localization.format("RemoveTableButtonText");
      EditorSpawnsZombiesUI.removeTableButton.tooltip = EditorSpawnsZombiesUI.localization.format("RemoveTableButtonTooltip");
      EditorSpawnsZombiesUI.removeTableButton.onClickedButton = new ClickedButton(EditorSpawnsZombiesUI.onClickedRemoveTableButton);
      EditorSpawnsZombiesUI.container.add((Sleek) EditorSpawnsZombiesUI.removeTableButton);
      EditorSpawnsZombiesUI.updateTables();
      EditorSpawnsZombiesUI.spawnsScrollBox = new SleekScrollBox();
      EditorSpawnsZombiesUI.spawnsScrollBox.positionOffset_X = -470;
      EditorSpawnsZombiesUI.spawnsScrollBox.positionOffset_Y = 310;
      EditorSpawnsZombiesUI.spawnsScrollBox.positionScale_X = 1f;
      EditorSpawnsZombiesUI.spawnsScrollBox.sizeOffset_X = 470;
      EditorSpawnsZombiesUI.spawnsScrollBox.sizeOffset_Y = -310;
      EditorSpawnsZombiesUI.spawnsScrollBox.sizeScale_Y = 1f;
      EditorSpawnsZombiesUI.spawnsScrollBox.area = new Rect(0.0f, 0.0f, 5f, 1000f);
      EditorSpawnsZombiesUI.container.add((Sleek) EditorSpawnsZombiesUI.spawnsScrollBox);
      EditorSpawnsZombiesUI.tableColorPicker = new SleekColorPicker();
      EditorSpawnsZombiesUI.tableColorPicker.positionOffset_X = 200;
      EditorSpawnsZombiesUI.tableColorPicker.onColorPicked = new ColorPicked(EditorSpawnsZombiesUI.onZombieColorPicked);
      EditorSpawnsZombiesUI.spawnsScrollBox.add((Sleek) EditorSpawnsZombiesUI.tableColorPicker);
      EditorSpawnsZombiesUI.megaToggle = new SleekToggle();
      EditorSpawnsZombiesUI.megaToggle.positionOffset_X = 240;
      EditorSpawnsZombiesUI.megaToggle.positionOffset_Y = 130;
      EditorSpawnsZombiesUI.megaToggle.sizeOffset_X = 40;
      EditorSpawnsZombiesUI.megaToggle.sizeOffset_Y = 40;
      EditorSpawnsZombiesUI.megaToggle.onToggled = new Toggled(EditorSpawnsZombiesUI.onToggledMegaToggle);
      EditorSpawnsZombiesUI.megaToggle.addLabel(EditorSpawnsZombiesUI.localization.format("MegaToggleLabelText"), ESleekSide.LEFT);
      EditorSpawnsZombiesUI.spawnsScrollBox.add((Sleek) EditorSpawnsZombiesUI.megaToggle);
      EditorSpawnsZombiesUI.healthField = new SleekUInt16Field();
      EditorSpawnsZombiesUI.healthField.positionOffset_X = 240;
      EditorSpawnsZombiesUI.healthField.positionOffset_Y = 180;
      EditorSpawnsZombiesUI.healthField.sizeOffset_X = 200;
      EditorSpawnsZombiesUI.healthField.sizeOffset_Y = 30;
      EditorSpawnsZombiesUI.healthField.onTypedUInt16 = new TypedUInt16(EditorSpawnsZombiesUI.onHealthFieldTyped);
      EditorSpawnsZombiesUI.healthField.addLabel(EditorSpawnsZombiesUI.localization.format("HealthFieldLabelText"), ESleekSide.LEFT);
      EditorSpawnsZombiesUI.spawnsScrollBox.add((Sleek) EditorSpawnsZombiesUI.healthField);
      EditorSpawnsZombiesUI.damageField = new SleekByteField();
      EditorSpawnsZombiesUI.damageField.positionOffset_X = 240;
      EditorSpawnsZombiesUI.damageField.positionOffset_Y = 220;
      EditorSpawnsZombiesUI.damageField.sizeOffset_X = 200;
      EditorSpawnsZombiesUI.damageField.sizeOffset_Y = 30;
      EditorSpawnsZombiesUI.damageField.onTypedByte = new TypedByte(EditorSpawnsZombiesUI.onDamageFieldTyped);
      EditorSpawnsZombiesUI.damageField.addLabel(EditorSpawnsZombiesUI.localization.format("DamageFieldLabelText"), ESleekSide.LEFT);
      EditorSpawnsZombiesUI.spawnsScrollBox.add((Sleek) EditorSpawnsZombiesUI.damageField);
      EditorSpawnsZombiesUI.lootIDField = new SleekByteField();
      EditorSpawnsZombiesUI.lootIDField.positionOffset_X = 240;
      EditorSpawnsZombiesUI.lootIDField.positionOffset_Y = 260;
      EditorSpawnsZombiesUI.lootIDField.sizeOffset_X = 200;
      EditorSpawnsZombiesUI.lootIDField.sizeOffset_Y = 30;
      EditorSpawnsZombiesUI.lootIDField.onTypedByte = new TypedByte(EditorSpawnsZombiesUI.onLootIDFieldTyped);
      EditorSpawnsZombiesUI.lootIDField.addLabel(EditorSpawnsZombiesUI.localization.format("LootIDFieldLabelText"), ESleekSide.LEFT);
      EditorSpawnsZombiesUI.spawnsScrollBox.add((Sleek) EditorSpawnsZombiesUI.lootIDField);
      EditorSpawnsZombiesUI.itemIDField = new SleekUInt16Field();
      EditorSpawnsZombiesUI.itemIDField.positionOffset_X = 240;
      EditorSpawnsZombiesUI.itemIDField.sizeOffset_X = 200;
      EditorSpawnsZombiesUI.itemIDField.sizeOffset_Y = 30;
      EditorSpawnsZombiesUI.itemIDField.addLabel(EditorSpawnsZombiesUI.localization.format("ItemIDFieldLabelText"), ESleekSide.LEFT);
      EditorSpawnsZombiesUI.spawnsScrollBox.add((Sleek) EditorSpawnsZombiesUI.itemIDField);
      EditorSpawnsZombiesUI.addItemButton = new SleekButtonIcon((Texture2D) bundle.load("Add"));
      EditorSpawnsZombiesUI.addItemButton.positionOffset_X = 240;
      EditorSpawnsZombiesUI.addItemButton.sizeOffset_X = 95;
      EditorSpawnsZombiesUI.addItemButton.sizeOffset_Y = 30;
      EditorSpawnsZombiesUI.addItemButton.text = EditorSpawnsZombiesUI.localization.format("AddItemButtonText");
      EditorSpawnsZombiesUI.addItemButton.tooltip = EditorSpawnsZombiesUI.localization.format("AddItemButtonTooltip");
      EditorSpawnsZombiesUI.addItemButton.onClickedButton = new ClickedButton(EditorSpawnsZombiesUI.onClickedAddItemButton);
      EditorSpawnsZombiesUI.spawnsScrollBox.add((Sleek) EditorSpawnsZombiesUI.addItemButton);
      EditorSpawnsZombiesUI.removeItemButton = new SleekButtonIcon((Texture2D) bundle.load("Remove"));
      EditorSpawnsZombiesUI.removeItemButton.positionOffset_X = 345;
      EditorSpawnsZombiesUI.removeItemButton.sizeOffset_X = 95;
      EditorSpawnsZombiesUI.removeItemButton.sizeOffset_Y = 30;
      EditorSpawnsZombiesUI.removeItemButton.text = EditorSpawnsZombiesUI.localization.format("RemoveItemButtonText");
      EditorSpawnsZombiesUI.removeItemButton.tooltip = EditorSpawnsZombiesUI.localization.format("RemoveItemButtonTooltip");
      EditorSpawnsZombiesUI.removeItemButton.onClickedButton = new ClickedButton(EditorSpawnsZombiesUI.onClickedRemoveItemButton);
      EditorSpawnsZombiesUI.spawnsScrollBox.add((Sleek) EditorSpawnsZombiesUI.removeItemButton);
      EditorSpawnsZombiesUI.selectedBox = new SleekBox();
      EditorSpawnsZombiesUI.selectedBox.positionOffset_X = -230;
      EditorSpawnsZombiesUI.selectedBox.positionOffset_Y = 80;
      EditorSpawnsZombiesUI.selectedBox.positionScale_X = 1f;
      EditorSpawnsZombiesUI.selectedBox.sizeOffset_X = 230;
      EditorSpawnsZombiesUI.selectedBox.sizeOffset_Y = 30;
      EditorSpawnsZombiesUI.selectedBox.addLabel(EditorSpawnsZombiesUI.localization.format("SelectionBoxLabelText"), ESleekSide.LEFT);
      EditorSpawnsZombiesUI.container.add((Sleek) EditorSpawnsZombiesUI.selectedBox);
      EditorSpawnsZombiesUI.updateSelection();
      EditorSpawnsZombiesUI.radiusSlider = new SleekSlider();
      EditorSpawnsZombiesUI.radiusSlider.positionOffset_Y = -100;
      EditorSpawnsZombiesUI.radiusSlider.positionScale_Y = 1f;
      EditorSpawnsZombiesUI.radiusSlider.sizeOffset_X = 200;
      EditorSpawnsZombiesUI.radiusSlider.sizeOffset_Y = 20;
      EditorSpawnsZombiesUI.radiusSlider.state = (float) ((int) EditorSpawns.radius - (int) EditorSpawns.MIN_REMOVE_SIZE) / (float) EditorSpawns.MAX_REMOVE_SIZE;
      EditorSpawnsZombiesUI.radiusSlider.orientation = ESleekOrientation.HORIZONTAL;
      EditorSpawnsZombiesUI.radiusSlider.addLabel(EditorSpawnsZombiesUI.localization.format("RadiusSliderLabelText"), ESleekSide.RIGHT);
      EditorSpawnsZombiesUI.radiusSlider.onDragged = new Dragged(EditorSpawnsZombiesUI.onDraggedRadiusSlider);
      EditorSpawnsZombiesUI.container.add((Sleek) EditorSpawnsZombiesUI.radiusSlider);
      EditorSpawnsZombiesUI.addButton = new SleekButtonIcon((Texture2D) bundle.load("Add"));
      EditorSpawnsZombiesUI.addButton.positionOffset_Y = -70;
      EditorSpawnsZombiesUI.addButton.positionScale_Y = 1f;
      EditorSpawnsZombiesUI.addButton.sizeOffset_X = 200;
      EditorSpawnsZombiesUI.addButton.sizeOffset_Y = 30;
      EditorSpawnsZombiesUI.addButton.text = EditorSpawnsZombiesUI.localization.format("AddButtonText", (object) ControlsSettings.tool_0);
      EditorSpawnsZombiesUI.addButton.tooltip = EditorSpawnsZombiesUI.localization.format("AddButtonTooltip");
      EditorSpawnsZombiesUI.addButton.onClickedButton = new ClickedButton(EditorSpawnsZombiesUI.onClickedAddButton);
      EditorSpawnsZombiesUI.container.add((Sleek) EditorSpawnsZombiesUI.addButton);
      EditorSpawnsZombiesUI.removeButton = new SleekButtonIcon((Texture2D) bundle.load("Remove"));
      EditorSpawnsZombiesUI.removeButton.positionOffset_Y = -30;
      EditorSpawnsZombiesUI.removeButton.positionScale_Y = 1f;
      EditorSpawnsZombiesUI.removeButton.sizeOffset_X = 200;
      EditorSpawnsZombiesUI.removeButton.sizeOffset_Y = 30;
      EditorSpawnsZombiesUI.removeButton.text = EditorSpawnsZombiesUI.localization.format("RemoveButtonText", (object) ControlsSettings.tool_1);
      EditorSpawnsZombiesUI.removeButton.tooltip = EditorSpawnsZombiesUI.localization.format("RemoveButtonTooltip");
      EditorSpawnsZombiesUI.removeButton.onClickedButton = new ClickedButton(EditorSpawnsZombiesUI.onClickedRemoveButton);
      EditorSpawnsZombiesUI.container.add((Sleek) EditorSpawnsZombiesUI.removeButton);
      bundle.unload();
    }

    public static void open()
    {
      if (EditorSpawnsZombiesUI.active)
      {
        EditorSpawnsZombiesUI.close();
      }
      else
      {
        EditorSpawnsZombiesUI.active = true;
        EditorSpawns.isSpawning = true;
        EditorSpawns.spawnMode = ESpawnMode.ADD_ZOMBIE;
        EditorSpawnsZombiesUI.container.lerpPositionScale(0.0f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
      }
    }

    public static void close()
    {
      if (!EditorSpawnsZombiesUI.active)
        return;
      EditorSpawnsZombiesUI.active = false;
      EditorSpawns.isSpawning = false;
      EditorSpawnsZombiesUI.container.lerpPositionScale(1f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
    }

    public static void updateTables()
    {
      if (EditorSpawnsZombiesUI.tableButtons != null)
      {
        for (int index = 0; index < EditorSpawnsZombiesUI.tableButtons.Length; ++index)
          EditorSpawnsZombiesUI.tableScrollBox.remove((Sleek) EditorSpawnsZombiesUI.tableButtons[index]);
      }
      EditorSpawnsZombiesUI.tableButtons = new SleekButton[LevelZombies.tables.Count];
      EditorSpawnsZombiesUI.tableScrollBox.area = new Rect(0.0f, 0.0f, 5f, (float) (EditorSpawnsZombiesUI.tableButtons.Length * 40 - 10));
      for (int index = 0; index < EditorSpawnsZombiesUI.tableButtons.Length; ++index)
      {
        SleekButton sleekButton = new SleekButton();
        sleekButton.positionOffset_X = 240;
        sleekButton.positionOffset_Y = index * 40;
        sleekButton.sizeOffset_X = 200;
        sleekButton.sizeOffset_Y = 30;
        sleekButton.text = (string) (object) index + (object) " " + LevelZombies.tables[index].name;
        sleekButton.onClickedButton = new ClickedButton(EditorSpawnsZombiesUI.onClickedTableButton);
        EditorSpawnsZombiesUI.tableScrollBox.add((Sleek) sleekButton);
        EditorSpawnsZombiesUI.tableButtons[index] = sleekButton;
      }
    }

    public static void updateSelection()
    {
      if ((int) EditorSpawns.selectedZombie < LevelZombies.tables.Count)
      {
        ZombieTable zombieTable = LevelZombies.tables[(int) EditorSpawns.selectedZombie];
        EditorSpawnsZombiesUI.selectedBox.text = zombieTable.name;
        EditorSpawnsZombiesUI.tableColorPicker.state = zombieTable.color;
        EditorSpawnsZombiesUI.megaToggle.state = zombieTable.isMega;
        EditorSpawnsZombiesUI.healthField.state = zombieTable.health;
        EditorSpawnsZombiesUI.damageField.state = zombieTable.damage;
        EditorSpawnsZombiesUI.lootIDField.state = zombieTable.loot;
        if (EditorSpawnsZombiesUI.slotButtons != null)
        {
          for (int index = 0; index < EditorSpawnsZombiesUI.slotButtons.Length; ++index)
            EditorSpawnsZombiesUI.spawnsScrollBox.remove((Sleek) EditorSpawnsZombiesUI.slotButtons[index]);
        }
        EditorSpawnsZombiesUI.slotButtons = new SleekButton[zombieTable.slots.Length];
        for (int index = 0; index < EditorSpawnsZombiesUI.slotButtons.Length; ++index)
        {
          ZombieSlot zombieSlot = zombieTable.slots[index];
          SleekButton sleekButton = new SleekButton();
          sleekButton.positionOffset_X = 240;
          sleekButton.positionOffset_Y = 300 + index * 70;
          sleekButton.sizeOffset_X = 200;
          sleekButton.sizeOffset_Y = 30;
          sleekButton.text = EditorSpawnsZombiesUI.localization.format("Slot_" + (object) index);
          sleekButton.onClickedButton = new ClickedButton(EditorSpawnsZombiesUI.onClickedSlotButton);
          EditorSpawnsZombiesUI.spawnsScrollBox.add((Sleek) sleekButton);
          SleekSlider sleekSlider = new SleekSlider();
          sleekSlider.positionOffset_Y = 40;
          sleekSlider.sizeOffset_X = 200;
          sleekSlider.sizeOffset_Y = 20;
          sleekSlider.orientation = ESleekOrientation.HORIZONTAL;
          sleekSlider.state = zombieSlot.chance;
          sleekSlider.addLabel((string) (object) Mathf.Floor(zombieSlot.chance * 100f) + (object) "%", ESleekSide.LEFT);
          sleekSlider.onDragged = new Dragged(EditorSpawnsZombiesUI.onDraggedChanceSlider);
          sleekButton.add((Sleek) sleekSlider);
          EditorSpawnsZombiesUI.slotButtons[index] = sleekButton;
        }
        if (EditorSpawnsZombiesUI.clothButtons != null)
        {
          for (int index = 0; index < EditorSpawnsZombiesUI.clothButtons.Length; ++index)
            EditorSpawnsZombiesUI.spawnsScrollBox.remove((Sleek) EditorSpawnsZombiesUI.clothButtons[index]);
        }
        if ((int) EditorSpawnsZombiesUI.selectedSlot < zombieTable.slots.Length)
        {
          EditorSpawnsZombiesUI.clothButtons = new SleekButton[zombieTable.slots[(int) EditorSpawnsZombiesUI.selectedSlot].table.Count];
          for (int index = 0; index < EditorSpawnsZombiesUI.clothButtons.Length; ++index)
          {
            SleekButton sleekButton = new SleekButton();
            sleekButton.positionOffset_X = 240;
            sleekButton.positionOffset_Y = 300 + EditorSpawnsZombiesUI.slotButtons.Length * 70 + index * 40;
            sleekButton.sizeOffset_X = 200;
            sleekButton.sizeOffset_Y = 30;
            ItemAsset itemAsset = (ItemAsset) Assets.find(EAssetType.ITEM, zombieTable.slots[(int) EditorSpawnsZombiesUI.selectedSlot].table[index].item);
            if (itemAsset != null)
              sleekButton.text = (string) (object) zombieTable.slots[(int) EditorSpawnsZombiesUI.selectedSlot].table[index].item + (object) " " + itemAsset.itemName;
            else
              sleekButton.text = zombieTable.slots[(int) EditorSpawnsZombiesUI.selectedSlot].table[index].item.ToString();
            sleekButton.onClickedButton = new ClickedButton(EditorSpawnsZombiesUI.onClickItemButton);
            EditorSpawnsZombiesUI.spawnsScrollBox.add((Sleek) sleekButton);
            EditorSpawnsZombiesUI.clothButtons[index] = sleekButton;
          }
        }
        else
          EditorSpawnsZombiesUI.clothButtons = new SleekButton[0];
        EditorSpawnsZombiesUI.itemIDField.positionOffset_Y = 300 + EditorSpawnsZombiesUI.slotButtons.Length * 70 + EditorSpawnsZombiesUI.clothButtons.Length * 40;
        EditorSpawnsZombiesUI.addItemButton.positionOffset_Y = 300 + EditorSpawnsZombiesUI.slotButtons.Length * 70 + EditorSpawnsZombiesUI.clothButtons.Length * 40 + 40;
        EditorSpawnsZombiesUI.removeItemButton.positionOffset_Y = 300 + EditorSpawnsZombiesUI.slotButtons.Length * 70 + EditorSpawnsZombiesUI.clothButtons.Length * 40 + 40;
        EditorSpawnsZombiesUI.spawnsScrollBox.area = new Rect(0.0f, 0.0f, 5f, (float) (300 + EditorSpawnsZombiesUI.slotButtons.Length * 70 + EditorSpawnsZombiesUI.clothButtons.Length * 40 + 70));
      }
      else
      {
        EditorSpawnsZombiesUI.selectedBox.text = string.Empty;
        EditorSpawnsZombiesUI.tableColorPicker.state = Color.white;
        EditorSpawnsZombiesUI.megaToggle.state = false;
        EditorSpawnsZombiesUI.healthField.state = (ushort) 0;
        EditorSpawnsZombiesUI.damageField.state = (byte) 0;
        EditorSpawnsZombiesUI.lootIDField.state = (byte) 0;
        if (EditorSpawnsZombiesUI.slotButtons != null)
        {
          for (int index = 0; index < EditorSpawnsZombiesUI.slotButtons.Length; ++index)
            EditorSpawnsZombiesUI.spawnsScrollBox.remove((Sleek) EditorSpawnsZombiesUI.slotButtons[index]);
        }
        EditorSpawnsZombiesUI.slotButtons = (SleekButton[]) null;
        if (EditorSpawnsZombiesUI.clothButtons != null)
        {
          for (int index = 0; index < EditorSpawnsZombiesUI.clothButtons.Length; ++index)
            EditorSpawnsZombiesUI.spawnsScrollBox.remove((Sleek) EditorSpawnsZombiesUI.clothButtons[index]);
        }
        EditorSpawnsZombiesUI.clothButtons = (SleekButton[]) null;
        EditorSpawnsZombiesUI.itemIDField.positionOffset_Y = 300;
        EditorSpawnsZombiesUI.addItemButton.positionOffset_Y = 340;
        EditorSpawnsZombiesUI.removeItemButton.positionOffset_Y = 340;
        EditorSpawnsZombiesUI.spawnsScrollBox.area = new Rect(0.0f, 0.0f, 5f, 370f);
      }
    }

    private static void onClickedTableButton(SleekButton button)
    {
      EditorSpawns.selectedZombie = (byte) (button.positionOffset_Y / 40);
      EditorSpawns.zombieSpawn.GetComponent<Renderer>().material.color = LevelZombies.tables[(int) EditorSpawns.selectedZombie].color;
      EditorSpawnsZombiesUI.updateSelection();
    }

    private static void onZombieColorPicked(SleekColorPicker picker, Color color)
    {
      if ((int) EditorSpawns.selectedZombie >= LevelZombies.tables.Count)
        return;
      LevelZombies.tables[(int) EditorSpawns.selectedZombie].color = color;
    }

    private static void onToggledMegaToggle(SleekToggle toggle, bool state)
    {
      if ((int) EditorSpawns.selectedZombie >= LevelZombies.tables.Count)
        return;
      LevelZombies.tables[(int) EditorSpawns.selectedZombie].isMega = state;
    }

    private static void onHealthFieldTyped(SleekUInt16Field field, ushort state)
    {
      if ((int) EditorSpawns.selectedZombie >= LevelZombies.tables.Count)
        return;
      LevelZombies.tables[(int) EditorSpawns.selectedZombie].health = state;
    }

    private static void onDamageFieldTyped(SleekByteField field, byte state)
    {
      if ((int) EditorSpawns.selectedZombie >= LevelZombies.tables.Count)
        return;
      LevelZombies.tables[(int) EditorSpawns.selectedZombie].damage = state;
    }

    private static void onLootIDFieldTyped(SleekByteField field, byte state)
    {
      if ((int) EditorSpawns.selectedZombie >= LevelZombies.tables.Count || (int) state >= LevelItems.tables.Count)
        return;
      LevelZombies.tables[(int) EditorSpawns.selectedZombie].loot = state;
    }

    private static void onClickedSlotButton(SleekButton button)
    {
      if ((int) EditorSpawns.selectedZombie >= LevelZombies.tables.Count)
        return;
      EditorSpawnsZombiesUI.selectedSlot = (byte) ((button.positionOffset_Y - 300) / 70);
      EditorSpawnsZombiesUI.updateSelection();
    }

    private static void onClickItemButton(SleekButton button)
    {
      if ((int) EditorSpawns.selectedZombie >= LevelZombies.tables.Count)
        return;
      EditorSpawnsZombiesUI.selectItem = (byte) ((button.positionOffset_Y - 300 - EditorSpawnsZombiesUI.slotButtons.Length * 70) / 40);
      EditorSpawnsZombiesUI.updateSelection();
    }

    private static void onDraggedChanceSlider(SleekSlider slider, float state)
    {
      if ((int) EditorSpawns.selectedZombie >= LevelZombies.tables.Count)
        return;
      int index = (slider.parent.positionOffset_Y - 300) / 70;
      LevelZombies.tables[(int) EditorSpawns.selectedZombie].slots[index].chance = state;
      EditorSpawnsZombiesUI.slotButtons[index].children[0].updateLabel((string) (object) Mathf.Floor(state * 100f) + (object) "%");
    }

    private static void onClickedAddTableButton(SleekButton button)
    {
      if (!(EditorSpawnsZombiesUI.tableNameField.text != string.Empty))
        return;
      LevelZombies.addTable(EditorSpawnsZombiesUI.tableNameField.text);
      EditorSpawnsZombiesUI.tableNameField.text = string.Empty;
      EditorSpawnsZombiesUI.updateTables();
      EditorSpawnsZombiesUI.tableScrollBox.state = new Vector2(0.0f, float.MaxValue);
    }

    private static void onClickedRemoveTableButton(SleekButton button)
    {
      if ((int) EditorSpawns.selectedZombie >= LevelZombies.tables.Count)
        return;
      LevelZombies.removeTable();
      EditorSpawnsZombiesUI.updateTables();
      EditorSpawnsZombiesUI.updateSelection();
      EditorSpawnsZombiesUI.tableScrollBox.state = new Vector2(0.0f, float.MaxValue);
    }

    private static void onClickedAddItemButton(SleekButton button)
    {
      if ((int) EditorSpawns.selectedZombie >= LevelZombies.tables.Count)
        return;
      ItemAsset itemAsset = (ItemAsset) Assets.find(EAssetType.ITEM, EditorSpawnsZombiesUI.itemIDField.state);
      if (itemAsset != null)
      {
        if ((int) EditorSpawnsZombiesUI.selectedSlot == 0 && itemAsset.type != EItemType.SHIRT || (int) EditorSpawnsZombiesUI.selectedSlot == 1 && itemAsset.type != EItemType.PANTS || (int) EditorSpawnsZombiesUI.selectedSlot == 2 && itemAsset.type != EItemType.HAT || (int) EditorSpawnsZombiesUI.selectedSlot == 3 && itemAsset.type != EItemType.BACKPACK && (itemAsset.type != EItemType.VEST && itemAsset.type != EItemType.MASK) && itemAsset.type != EItemType.GLASSES)
          return;
        LevelZombies.tables[(int) EditorSpawns.selectedZombie].addCloth(EditorSpawnsZombiesUI.selectedSlot, EditorSpawnsZombiesUI.itemIDField.state);
        EditorSpawnsZombiesUI.updateSelection();
        EditorSpawnsZombiesUI.spawnsScrollBox.state = new Vector2(0.0f, float.MaxValue);
      }
      EditorSpawnsZombiesUI.itemIDField.state = (ushort) 0;
    }

    private static void onClickedRemoveItemButton(SleekButton button)
    {
      if ((int) EditorSpawns.selectedZombie >= LevelZombies.tables.Count || (int) EditorSpawnsZombiesUI.selectItem >= LevelZombies.tables[(int) EditorSpawns.selectedZombie].slots[(int) EditorSpawnsZombiesUI.selectedSlot].table.Count)
        return;
      LevelZombies.tables[(int) EditorSpawns.selectedZombie].removeCloth(EditorSpawnsZombiesUI.selectedSlot, EditorSpawnsZombiesUI.selectItem);
      EditorSpawnsZombiesUI.updateSelection();
      EditorSpawnsZombiesUI.spawnsScrollBox.state = new Vector2(0.0f, float.MaxValue);
    }

    private static void onDraggedRadiusSlider(SleekSlider slider, float state)
    {
      EditorSpawns.radius = (byte) ((double) EditorSpawns.MIN_REMOVE_SIZE + (double) state * (double) EditorSpawns.MAX_REMOVE_SIZE);
    }

    private static void onClickedAddButton(SleekButton button)
    {
      EditorSpawns.spawnMode = ESpawnMode.ADD_ZOMBIE;
    }

    private static void onClickedRemoveButton(SleekButton button)
    {
      EditorSpawns.spawnMode = ESpawnMode.REMOVE_ZOMBIE;
    }
  }
}
