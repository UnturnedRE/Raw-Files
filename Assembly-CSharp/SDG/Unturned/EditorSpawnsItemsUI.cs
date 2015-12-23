// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.EditorSpawnsItemsUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class EditorSpawnsItemsUI
  {
    private static Sleek container;
    public static bool active;
    private static SleekScrollBox tableScrollBox;
    private static SleekScrollBox spawnsScrollBox;
    private static SleekButton[] tableButtons;
    private static SleekButton[] tierButtons;
    private static SleekButton[] itemButtons;
    private static SleekColorPicker tableColorPicker;
    private static SleekField tierNameField;
    private static SleekButtonIcon addTierButton;
    private static SleekButtonIcon removeTierButton;
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
    private static byte selectedTier;
    private static byte selectItem;

    public EditorSpawnsItemsUI()
    {
      Local local = Localization.read("/Editor/EditorSpawnsItems.dat");
      Bundle bundle = Bundles.getBundle("/Bundles/Textures/Edit/Icons/EditorSpawnsItems/EditorSpawnsItems.unity3d");
      EditorSpawnsItemsUI.container = new Sleek();
      EditorSpawnsItemsUI.container.positionOffset_X = 10;
      EditorSpawnsItemsUI.container.positionOffset_Y = 10;
      EditorSpawnsItemsUI.container.positionScale_X = 1f;
      EditorSpawnsItemsUI.container.sizeOffset_X = -20;
      EditorSpawnsItemsUI.container.sizeOffset_Y = -20;
      EditorSpawnsItemsUI.container.sizeScale_X = 1f;
      EditorSpawnsItemsUI.container.sizeScale_Y = 1f;
      EditorUI.window.add(EditorSpawnsItemsUI.container);
      EditorSpawnsItemsUI.active = false;
      EditorSpawnsItemsUI.tableScrollBox = new SleekScrollBox();
      EditorSpawnsItemsUI.tableScrollBox.positionOffset_X = -470;
      EditorSpawnsItemsUI.tableScrollBox.positionOffset_Y = 120;
      EditorSpawnsItemsUI.tableScrollBox.positionScale_X = 1f;
      EditorSpawnsItemsUI.tableScrollBox.sizeOffset_X = 470;
      EditorSpawnsItemsUI.tableScrollBox.sizeOffset_Y = 100;
      EditorSpawnsItemsUI.container.add((Sleek) EditorSpawnsItemsUI.tableScrollBox);
      EditorSpawnsItemsUI.tableNameField = new SleekField();
      EditorSpawnsItemsUI.tableNameField.positionOffset_X = -230;
      EditorSpawnsItemsUI.tableNameField.positionOffset_Y = 230;
      EditorSpawnsItemsUI.tableNameField.positionScale_X = 1f;
      EditorSpawnsItemsUI.tableNameField.sizeOffset_X = 230;
      EditorSpawnsItemsUI.tableNameField.sizeOffset_Y = 30;
      EditorSpawnsItemsUI.tableNameField.addLabel(local.format("TableNameFieldLabelText"), ESleekSide.LEFT);
      EditorSpawnsItemsUI.container.add((Sleek) EditorSpawnsItemsUI.tableNameField);
      EditorSpawnsItemsUI.addTableButton = new SleekButtonIcon((Texture2D) bundle.load("Add"));
      EditorSpawnsItemsUI.addTableButton.positionOffset_X = -230;
      EditorSpawnsItemsUI.addTableButton.positionOffset_Y = 270;
      EditorSpawnsItemsUI.addTableButton.positionScale_X = 1f;
      EditorSpawnsItemsUI.addTableButton.sizeOffset_X = 110;
      EditorSpawnsItemsUI.addTableButton.sizeOffset_Y = 30;
      EditorSpawnsItemsUI.addTableButton.text = local.format("AddTableButtonText");
      EditorSpawnsItemsUI.addTableButton.tooltip = local.format("AddTableButtonTooltip");
      EditorSpawnsItemsUI.addTableButton.onClickedButton = new ClickedButton(EditorSpawnsItemsUI.onClickedAddTableButton);
      EditorSpawnsItemsUI.container.add((Sleek) EditorSpawnsItemsUI.addTableButton);
      EditorSpawnsItemsUI.removeTableButton = new SleekButtonIcon((Texture2D) bundle.load("Remove"));
      EditorSpawnsItemsUI.removeTableButton.positionOffset_X = -110;
      EditorSpawnsItemsUI.removeTableButton.positionOffset_Y = 270;
      EditorSpawnsItemsUI.removeTableButton.positionScale_X = 1f;
      EditorSpawnsItemsUI.removeTableButton.sizeOffset_X = 110;
      EditorSpawnsItemsUI.removeTableButton.sizeOffset_Y = 30;
      EditorSpawnsItemsUI.removeTableButton.text = local.format("RemoveTableButtonText");
      EditorSpawnsItemsUI.removeTableButton.tooltip = local.format("RemoveTableButtonTooltip");
      EditorSpawnsItemsUI.removeTableButton.onClickedButton = new ClickedButton(EditorSpawnsItemsUI.onClickedRemoveTableButton);
      EditorSpawnsItemsUI.container.add((Sleek) EditorSpawnsItemsUI.removeTableButton);
      EditorSpawnsItemsUI.updateTables();
      EditorSpawnsItemsUI.spawnsScrollBox = new SleekScrollBox();
      EditorSpawnsItemsUI.spawnsScrollBox.positionOffset_X = -470;
      EditorSpawnsItemsUI.spawnsScrollBox.positionOffset_Y = 310;
      EditorSpawnsItemsUI.spawnsScrollBox.positionScale_X = 1f;
      EditorSpawnsItemsUI.spawnsScrollBox.sizeOffset_X = 470;
      EditorSpawnsItemsUI.spawnsScrollBox.sizeOffset_Y = -310;
      EditorSpawnsItemsUI.spawnsScrollBox.sizeScale_Y = 1f;
      EditorSpawnsItemsUI.spawnsScrollBox.area = new Rect(0.0f, 0.0f, 5f, 1000f);
      EditorSpawnsItemsUI.container.add((Sleek) EditorSpawnsItemsUI.spawnsScrollBox);
      EditorSpawnsItemsUI.tableColorPicker = new SleekColorPicker();
      EditorSpawnsItemsUI.tableColorPicker.positionOffset_X = 200;
      EditorSpawnsItemsUI.tableColorPicker.onColorPicked = new ColorPicked(EditorSpawnsItemsUI.onItemColorPicked);
      EditorSpawnsItemsUI.spawnsScrollBox.add((Sleek) EditorSpawnsItemsUI.tableColorPicker);
      EditorSpawnsItemsUI.tierNameField = new SleekField();
      EditorSpawnsItemsUI.tierNameField.positionOffset_X = 240;
      EditorSpawnsItemsUI.tierNameField.sizeOffset_X = 200;
      EditorSpawnsItemsUI.tierNameField.sizeOffset_Y = 30;
      EditorSpawnsItemsUI.tierNameField.addLabel(local.format("TierNameFieldLabelText"), ESleekSide.LEFT);
      EditorSpawnsItemsUI.spawnsScrollBox.add((Sleek) EditorSpawnsItemsUI.tierNameField);
      EditorSpawnsItemsUI.addTierButton = new SleekButtonIcon((Texture2D) bundle.load("Add"));
      EditorSpawnsItemsUI.addTierButton.positionOffset_X = 240;
      EditorSpawnsItemsUI.addTierButton.sizeOffset_X = 95;
      EditorSpawnsItemsUI.addTierButton.sizeOffset_Y = 30;
      EditorSpawnsItemsUI.addTierButton.text = local.format("AddTierButtonText");
      EditorSpawnsItemsUI.addTierButton.tooltip = local.format("AddTierButtonTooltip");
      EditorSpawnsItemsUI.addTierButton.onClickedButton = new ClickedButton(EditorSpawnsItemsUI.onClickedAddTierButton);
      EditorSpawnsItemsUI.spawnsScrollBox.add((Sleek) EditorSpawnsItemsUI.addTierButton);
      EditorSpawnsItemsUI.removeTierButton = new SleekButtonIcon((Texture2D) bundle.load("Remove"));
      EditorSpawnsItemsUI.removeTierButton.positionOffset_X = 345;
      EditorSpawnsItemsUI.removeTierButton.sizeOffset_X = 95;
      EditorSpawnsItemsUI.removeTierButton.sizeOffset_Y = 30;
      EditorSpawnsItemsUI.removeTierButton.text = local.format("RemoveTierButtonText");
      EditorSpawnsItemsUI.removeTierButton.tooltip = local.format("RemoveTierButtonTooltip");
      EditorSpawnsItemsUI.removeTierButton.onClickedButton = new ClickedButton(EditorSpawnsItemsUI.onClickedRemoveTierButton);
      EditorSpawnsItemsUI.spawnsScrollBox.add((Sleek) EditorSpawnsItemsUI.removeTierButton);
      EditorSpawnsItemsUI.itemIDField = new SleekUInt16Field();
      EditorSpawnsItemsUI.itemIDField.positionOffset_X = 240;
      EditorSpawnsItemsUI.itemIDField.sizeOffset_X = 200;
      EditorSpawnsItemsUI.itemIDField.sizeOffset_Y = 30;
      EditorSpawnsItemsUI.itemIDField.addLabel(local.format("ItemIDFieldLabelText"), ESleekSide.LEFT);
      EditorSpawnsItemsUI.spawnsScrollBox.add((Sleek) EditorSpawnsItemsUI.itemIDField);
      EditorSpawnsItemsUI.addItemButton = new SleekButtonIcon((Texture2D) bundle.load("Add"));
      EditorSpawnsItemsUI.addItemButton.positionOffset_X = 240;
      EditorSpawnsItemsUI.addItemButton.sizeOffset_X = 95;
      EditorSpawnsItemsUI.addItemButton.sizeOffset_Y = 30;
      EditorSpawnsItemsUI.addItemButton.text = local.format("AddItemButtonText");
      EditorSpawnsItemsUI.addItemButton.tooltip = local.format("AddItemButtonTooltip");
      EditorSpawnsItemsUI.addItemButton.onClickedButton = new ClickedButton(EditorSpawnsItemsUI.onClickedAddItemButton);
      EditorSpawnsItemsUI.spawnsScrollBox.add((Sleek) EditorSpawnsItemsUI.addItemButton);
      EditorSpawnsItemsUI.removeItemButton = new SleekButtonIcon((Texture2D) bundle.load("Remove"));
      EditorSpawnsItemsUI.removeItemButton.positionOffset_X = 345;
      EditorSpawnsItemsUI.removeItemButton.sizeOffset_X = 95;
      EditorSpawnsItemsUI.removeItemButton.sizeOffset_Y = 30;
      EditorSpawnsItemsUI.removeItemButton.text = local.format("RemoveItemButtonText");
      EditorSpawnsItemsUI.removeItemButton.tooltip = local.format("RemoveItemButtonTooltip");
      EditorSpawnsItemsUI.removeItemButton.onClickedButton = new ClickedButton(EditorSpawnsItemsUI.onClickedRemoveItemButton);
      EditorSpawnsItemsUI.spawnsScrollBox.add((Sleek) EditorSpawnsItemsUI.removeItemButton);
      EditorSpawnsItemsUI.selectedBox = new SleekBox();
      EditorSpawnsItemsUI.selectedBox.positionOffset_X = -230;
      EditorSpawnsItemsUI.selectedBox.positionOffset_Y = 80;
      EditorSpawnsItemsUI.selectedBox.positionScale_X = 1f;
      EditorSpawnsItemsUI.selectedBox.sizeOffset_X = 230;
      EditorSpawnsItemsUI.selectedBox.sizeOffset_Y = 30;
      EditorSpawnsItemsUI.selectedBox.addLabel(local.format("SelectionBoxLabelText"), ESleekSide.LEFT);
      EditorSpawnsItemsUI.container.add((Sleek) EditorSpawnsItemsUI.selectedBox);
      EditorSpawnsItemsUI.updateSelection();
      EditorSpawnsItemsUI.radiusSlider = new SleekSlider();
      EditorSpawnsItemsUI.radiusSlider.positionOffset_Y = -100;
      EditorSpawnsItemsUI.radiusSlider.positionScale_Y = 1f;
      EditorSpawnsItemsUI.radiusSlider.sizeOffset_X = 200;
      EditorSpawnsItemsUI.radiusSlider.sizeOffset_Y = 20;
      EditorSpawnsItemsUI.radiusSlider.state = (float) ((int) EditorSpawns.radius - (int) EditorSpawns.MIN_REMOVE_SIZE) / (float) EditorSpawns.MAX_REMOVE_SIZE;
      EditorSpawnsItemsUI.radiusSlider.orientation = ESleekOrientation.HORIZONTAL;
      EditorSpawnsItemsUI.radiusSlider.addLabel(local.format("RadiusSliderLabelText"), ESleekSide.RIGHT);
      EditorSpawnsItemsUI.radiusSlider.onDragged = new Dragged(EditorSpawnsItemsUI.onDraggedRadiusSlider);
      EditorSpawnsItemsUI.container.add((Sleek) EditorSpawnsItemsUI.radiusSlider);
      EditorSpawnsItemsUI.addButton = new SleekButtonIcon((Texture2D) bundle.load("Add"));
      EditorSpawnsItemsUI.addButton.positionOffset_Y = -70;
      EditorSpawnsItemsUI.addButton.positionScale_Y = 1f;
      EditorSpawnsItemsUI.addButton.sizeOffset_X = 200;
      EditorSpawnsItemsUI.addButton.sizeOffset_Y = 30;
      EditorSpawnsItemsUI.addButton.text = local.format("AddButtonText", (object) ControlsSettings.tool_0);
      EditorSpawnsItemsUI.addButton.tooltip = local.format("AddButtonTooltip");
      EditorSpawnsItemsUI.addButton.onClickedButton = new ClickedButton(EditorSpawnsItemsUI.onClickedAddButton);
      EditorSpawnsItemsUI.container.add((Sleek) EditorSpawnsItemsUI.addButton);
      EditorSpawnsItemsUI.removeButton = new SleekButtonIcon((Texture2D) bundle.load("Remove"));
      EditorSpawnsItemsUI.removeButton.positionOffset_Y = -30;
      EditorSpawnsItemsUI.removeButton.positionScale_Y = 1f;
      EditorSpawnsItemsUI.removeButton.sizeOffset_X = 200;
      EditorSpawnsItemsUI.removeButton.sizeOffset_Y = 30;
      EditorSpawnsItemsUI.removeButton.text = local.format("RemoveButtonText", (object) ControlsSettings.tool_1);
      EditorSpawnsItemsUI.removeButton.tooltip = local.format("RemoveButtonTooltip");
      EditorSpawnsItemsUI.removeButton.onClickedButton = new ClickedButton(EditorSpawnsItemsUI.onClickedRemoveButton);
      EditorSpawnsItemsUI.container.add((Sleek) EditorSpawnsItemsUI.removeButton);
      bundle.unload();
    }

    public static void open()
    {
      if (EditorSpawnsItemsUI.active)
      {
        EditorSpawnsItemsUI.close();
      }
      else
      {
        EditorSpawnsItemsUI.active = true;
        EditorSpawns.isSpawning = true;
        EditorSpawns.spawnMode = ESpawnMode.ADD_ITEM;
        EditorSpawnsItemsUI.container.lerpPositionScale(0.0f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
      }
    }

    public static void close()
    {
      if (!EditorSpawnsItemsUI.active)
        return;
      EditorSpawnsItemsUI.active = false;
      EditorSpawns.isSpawning = false;
      EditorSpawnsItemsUI.container.lerpPositionScale(1f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
    }

    public static void updateTables()
    {
      if (EditorSpawnsItemsUI.tableButtons != null)
      {
        for (int index = 0; index < EditorSpawnsItemsUI.tableButtons.Length; ++index)
          EditorSpawnsItemsUI.tableScrollBox.remove((Sleek) EditorSpawnsItemsUI.tableButtons[index]);
      }
      EditorSpawnsItemsUI.tableButtons = new SleekButton[LevelItems.tables.Count];
      EditorSpawnsItemsUI.tableScrollBox.area = new Rect(0.0f, 0.0f, 5f, (float) (EditorSpawnsItemsUI.tableButtons.Length * 40 - 10));
      for (int index = 0; index < EditorSpawnsItemsUI.tableButtons.Length; ++index)
      {
        SleekButton sleekButton = new SleekButton();
        sleekButton.positionOffset_X = 240;
        sleekButton.positionOffset_Y = index * 40;
        sleekButton.sizeOffset_X = 200;
        sleekButton.sizeOffset_Y = 30;
        sleekButton.text = (string) (object) index + (object) " " + LevelItems.tables[index].name;
        sleekButton.onClickedButton = new ClickedButton(EditorSpawnsItemsUI.onClickedTableButton);
        EditorSpawnsItemsUI.tableScrollBox.add((Sleek) sleekButton);
        EditorSpawnsItemsUI.tableButtons[index] = sleekButton;
      }
    }

    public static void updateSelection()
    {
      if ((int) EditorSpawns.selectedItem < LevelItems.tables.Count)
      {
        ItemTable itemTable = LevelItems.tables[(int) EditorSpawns.selectedItem];
        EditorSpawnsItemsUI.selectedBox.text = itemTable.name;
        EditorSpawnsItemsUI.tableColorPicker.state = itemTable.color;
        if (EditorSpawnsItemsUI.tierButtons != null)
        {
          for (int index = 0; index < EditorSpawnsItemsUI.tierButtons.Length; ++index)
            EditorSpawnsItemsUI.spawnsScrollBox.remove((Sleek) EditorSpawnsItemsUI.tierButtons[index]);
        }
        EditorSpawnsItemsUI.tierButtons = new SleekButton[itemTable.tiers.Count];
        for (int index = 0; index < EditorSpawnsItemsUI.tierButtons.Length; ++index)
        {
          ItemTier itemTier = itemTable.tiers[index];
          SleekButton sleekButton = new SleekButton();
          sleekButton.positionOffset_X = 240;
          sleekButton.positionOffset_Y = 130 + index * 70;
          sleekButton.sizeOffset_X = 200;
          sleekButton.sizeOffset_Y = 30;
          sleekButton.text = itemTier.name;
          sleekButton.onClickedButton = new ClickedButton(EditorSpawnsItemsUI.onClickedTierButton);
          EditorSpawnsItemsUI.spawnsScrollBox.add((Sleek) sleekButton);
          SleekSlider sleekSlider = new SleekSlider();
          sleekSlider.positionOffset_Y = 40;
          sleekSlider.sizeOffset_X = 200;
          sleekSlider.sizeOffset_Y = 20;
          sleekSlider.orientation = ESleekOrientation.HORIZONTAL;
          sleekSlider.state = itemTier.chance;
          sleekSlider.addLabel((string) (object) Mathf.Floor(itemTier.chance * 100f) + (object) "%", ESleekSide.LEFT);
          sleekSlider.onDragged = new Dragged(EditorSpawnsItemsUI.onDraggedChanceSlider);
          sleekButton.add((Sleek) sleekSlider);
          EditorSpawnsItemsUI.tierButtons[index] = sleekButton;
        }
        EditorSpawnsItemsUI.tierNameField.positionOffset_Y = 130 + EditorSpawnsItemsUI.tierButtons.Length * 70;
        EditorSpawnsItemsUI.addTierButton.positionOffset_Y = 130 + EditorSpawnsItemsUI.tierButtons.Length * 70 + 40;
        EditorSpawnsItemsUI.removeTierButton.positionOffset_Y = 130 + EditorSpawnsItemsUI.tierButtons.Length * 70 + 40;
        if (EditorSpawnsItemsUI.itemButtons != null)
        {
          for (int index = 0; index < EditorSpawnsItemsUI.itemButtons.Length; ++index)
            EditorSpawnsItemsUI.spawnsScrollBox.remove((Sleek) EditorSpawnsItemsUI.itemButtons[index]);
        }
        if ((int) EditorSpawnsItemsUI.selectedTier < itemTable.tiers.Count)
        {
          EditorSpawnsItemsUI.itemButtons = new SleekButton[itemTable.tiers[(int) EditorSpawnsItemsUI.selectedTier].table.Count];
          for (int index = 0; index < EditorSpawnsItemsUI.itemButtons.Length; ++index)
          {
            SleekButton sleekButton = new SleekButton();
            sleekButton.positionOffset_X = 240;
            sleekButton.positionOffset_Y = 130 + EditorSpawnsItemsUI.tierButtons.Length * 70 + 80 + index * 40;
            sleekButton.sizeOffset_X = 200;
            sleekButton.sizeOffset_Y = 30;
            ItemAsset itemAsset = (ItemAsset) Assets.find(EAssetType.ITEM, itemTable.tiers[(int) EditorSpawnsItemsUI.selectedTier].table[index].item);
            if (itemAsset != null)
              sleekButton.text = (string) (object) itemTable.tiers[(int) EditorSpawnsItemsUI.selectedTier].table[index].item + (object) " " + itemAsset.itemName;
            else
              sleekButton.text = itemTable.tiers[(int) EditorSpawnsItemsUI.selectedTier].table[index].item.ToString();
            sleekButton.onClickedButton = new ClickedButton(EditorSpawnsItemsUI.onClickItemButton);
            EditorSpawnsItemsUI.spawnsScrollBox.add((Sleek) sleekButton);
            EditorSpawnsItemsUI.itemButtons[index] = sleekButton;
          }
        }
        else
          EditorSpawnsItemsUI.itemButtons = new SleekButton[0];
        EditorSpawnsItemsUI.itemIDField.positionOffset_Y = 130 + EditorSpawnsItemsUI.tierButtons.Length * 70 + 80 + EditorSpawnsItemsUI.itemButtons.Length * 40;
        EditorSpawnsItemsUI.addItemButton.positionOffset_Y = 130 + EditorSpawnsItemsUI.tierButtons.Length * 70 + 80 + EditorSpawnsItemsUI.itemButtons.Length * 40 + 40;
        EditorSpawnsItemsUI.removeItemButton.positionOffset_Y = 130 + EditorSpawnsItemsUI.tierButtons.Length * 70 + 80 + EditorSpawnsItemsUI.itemButtons.Length * 40 + 40;
        EditorSpawnsItemsUI.spawnsScrollBox.area = new Rect(0.0f, 0.0f, 5f, (float) (130 + EditorSpawnsItemsUI.tierButtons.Length * 70 + 80 + EditorSpawnsItemsUI.itemButtons.Length * 40 + 70));
      }
      else
      {
        EditorSpawnsItemsUI.selectedBox.text = string.Empty;
        EditorSpawnsItemsUI.tableColorPicker.state = Color.white;
        if (EditorSpawnsItemsUI.tierButtons != null)
        {
          for (int index = 0; index < EditorSpawnsItemsUI.tierButtons.Length; ++index)
            EditorSpawnsItemsUI.spawnsScrollBox.remove((Sleek) EditorSpawnsItemsUI.tierButtons[index]);
        }
        EditorSpawnsItemsUI.tierButtons = (SleekButton[]) null;
        EditorSpawnsItemsUI.tierNameField.positionOffset_Y = 130;
        EditorSpawnsItemsUI.addTierButton.positionOffset_Y = 170;
        EditorSpawnsItemsUI.removeTierButton.positionOffset_Y = 170;
        if (EditorSpawnsItemsUI.itemButtons != null)
        {
          for (int index = 0; index < EditorSpawnsItemsUI.itemButtons.Length; ++index)
            EditorSpawnsItemsUI.spawnsScrollBox.remove((Sleek) EditorSpawnsItemsUI.itemButtons[index]);
        }
        EditorSpawnsItemsUI.itemButtons = (SleekButton[]) null;
        EditorSpawnsItemsUI.itemIDField.positionOffset_Y = 210;
        EditorSpawnsItemsUI.addItemButton.positionOffset_Y = 250;
        EditorSpawnsItemsUI.removeItemButton.positionOffset_Y = 250;
        EditorSpawnsItemsUI.spawnsScrollBox.area = new Rect(0.0f, 0.0f, 5f, 280f);
      }
    }

    private static void onClickedTableButton(SleekButton button)
    {
      EditorSpawns.selectedItem = (byte) (button.positionOffset_Y / 40);
      EditorSpawns.itemSpawn.GetComponent<Renderer>().material.color = LevelItems.tables[(int) EditorSpawns.selectedItem].color;
      EditorSpawnsItemsUI.updateSelection();
    }

    private static void onItemColorPicked(SleekColorPicker picker, Color color)
    {
      if ((int) EditorSpawns.selectedItem >= LevelItems.tables.Count)
        return;
      LevelItems.tables[(int) EditorSpawns.selectedItem].color = color;
    }

    private static void onClickedTierButton(SleekButton button)
    {
      if ((int) EditorSpawns.selectedItem >= LevelItems.tables.Count)
        return;
      EditorSpawnsItemsUI.selectedTier = (byte) ((button.positionOffset_Y - 130) / 70);
      EditorSpawnsItemsUI.updateSelection();
    }

    private static void onClickItemButton(SleekButton button)
    {
      if ((int) EditorSpawns.selectedItem >= LevelItems.tables.Count)
        return;
      EditorSpawnsItemsUI.selectItem = (byte) ((button.positionOffset_Y - 130 - EditorSpawnsItemsUI.tierButtons.Length * 70 - 80) / 40);
      EditorSpawnsItemsUI.updateSelection();
    }

    private static void onDraggedChanceSlider(SleekSlider slider, float state)
    {
      if ((int) EditorSpawns.selectedItem >= LevelItems.tables.Count)
        return;
      int tierIndex = (slider.parent.positionOffset_Y - 130) / 70;
      LevelItems.tables[(int) EditorSpawns.selectedItem].updateChance(tierIndex, state);
      for (int index = 0; index < LevelItems.tables[(int) EditorSpawns.selectedItem].tiers.Count; ++index)
      {
        ItemTier itemTier = LevelItems.tables[(int) EditorSpawns.selectedItem].tiers[index];
        SleekSlider sleekSlider = (SleekSlider) EditorSpawnsItemsUI.tierButtons[index].children[0];
        if (index != tierIndex)
          sleekSlider.state = itemTier.chance;
        sleekSlider.updateLabel((string) (object) Mathf.Floor(itemTier.chance * 100f) + (object) "%");
      }
    }

    private static void onClickedAddTableButton(SleekButton button)
    {
      if (!(EditorSpawnsItemsUI.tableNameField.text != string.Empty))
        return;
      LevelItems.addTable(EditorSpawnsItemsUI.tableNameField.text);
      EditorSpawnsItemsUI.tableNameField.text = string.Empty;
      EditorSpawnsItemsUI.updateTables();
      EditorSpawnsItemsUI.tableScrollBox.state = new Vector2(0.0f, float.MaxValue);
    }

    private static void onClickedRemoveTableButton(SleekButton button)
    {
      if ((int) EditorSpawns.selectedItem >= LevelItems.tables.Count)
        return;
      LevelItems.removeTable();
      EditorSpawnsItemsUI.updateTables();
      EditorSpawnsItemsUI.updateSelection();
      EditorSpawnsItemsUI.tableScrollBox.state = new Vector2(0.0f, float.MaxValue);
    }

    private static void onClickedAddTierButton(SleekButton button)
    {
      if ((int) EditorSpawns.selectedItem >= LevelItems.tables.Count || !(EditorSpawnsItemsUI.tierNameField.text != string.Empty))
        return;
      LevelItems.tables[(int) EditorSpawns.selectedItem].addTier(EditorSpawnsItemsUI.tierNameField.text);
      EditorSpawnsItemsUI.tierNameField.text = string.Empty;
      EditorSpawnsItemsUI.updateSelection();
    }

    private static void onClickedRemoveTierButton(SleekButton button)
    {
      if ((int) EditorSpawns.selectedItem >= LevelItems.tables.Count || (int) EditorSpawnsItemsUI.selectedTier >= LevelItems.tables[(int) EditorSpawns.selectedItem].tiers.Count)
        return;
      LevelItems.tables[(int) EditorSpawns.selectedItem].removeTier((int) EditorSpawnsItemsUI.selectedTier);
      EditorSpawnsItemsUI.updateSelection();
    }

    private static void onClickedAddItemButton(SleekButton button)
    {
      if ((int) EditorSpawns.selectedItem >= LevelItems.tables.Count || (int) EditorSpawnsItemsUI.selectedTier >= LevelItems.tables[(int) EditorSpawns.selectedItem].tiers.Count)
        return;
      ItemAsset itemAsset = (ItemAsset) Assets.find(EAssetType.ITEM, EditorSpawnsItemsUI.itemIDField.state);
      if (itemAsset != null && !itemAsset.isPro)
      {
        LevelItems.tables[(int) EditorSpawns.selectedItem].addItem(EditorSpawnsItemsUI.selectedTier, EditorSpawnsItemsUI.itemIDField.state);
        EditorSpawnsItemsUI.updateSelection();
        EditorSpawnsItemsUI.spawnsScrollBox.state = new Vector2(0.0f, float.MaxValue);
      }
      EditorSpawnsItemsUI.itemIDField.state = (ushort) 0;
    }

    private static void onClickedRemoveItemButton(SleekButton button)
    {
      if ((int) EditorSpawns.selectedItem >= LevelItems.tables.Count || (int) EditorSpawnsItemsUI.selectedTier >= LevelItems.tables[(int) EditorSpawns.selectedItem].tiers.Count || (int) EditorSpawnsItemsUI.selectItem >= LevelItems.tables[(int) EditorSpawns.selectedItem].tiers[(int) EditorSpawnsItemsUI.selectedTier].table.Count)
        return;
      LevelItems.tables[(int) EditorSpawns.selectedItem].removeItem(EditorSpawnsItemsUI.selectedTier, EditorSpawnsItemsUI.selectItem);
      EditorSpawnsItemsUI.updateSelection();
      EditorSpawnsItemsUI.spawnsScrollBox.state = new Vector2(0.0f, float.MaxValue);
    }

    private static void onDraggedRadiusSlider(SleekSlider slider, float state)
    {
      EditorSpawns.radius = (byte) ((double) EditorSpawns.MIN_REMOVE_SIZE + (double) state * (double) EditorSpawns.MAX_REMOVE_SIZE);
    }

    private static void onClickedAddButton(SleekButton button)
    {
      EditorSpawns.spawnMode = ESpawnMode.ADD_ITEM;
    }

    private static void onClickedRemoveButton(SleekButton button)
    {
      EditorSpawns.spawnMode = ESpawnMode.REMOVE_ITEM;
    }
  }
}
