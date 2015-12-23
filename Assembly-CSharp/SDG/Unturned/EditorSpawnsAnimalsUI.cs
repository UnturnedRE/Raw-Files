// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.EditorSpawnsAnimalsUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class EditorSpawnsAnimalsUI
  {
    private static Sleek container;
    public static bool active;
    private static SleekScrollBox tableScrollBox;
    private static SleekScrollBox spawnsScrollBox;
    private static SleekButton[] tableButtons;
    private static SleekButton[] tierButtons;
    private static SleekButton[] animalButtons;
    private static SleekColorPicker tableColorPicker;
    private static SleekField tierNameField;
    private static SleekButtonIcon addTierButton;
    private static SleekButtonIcon removeTierButton;
    private static SleekUInt16Field animalIDField;
    private static SleekButtonIcon addAnimalButton;
    private static SleekButtonIcon removeAnimalButton;
    private static SleekBox selectedBox;
    private static SleekField tableNameField;
    private static SleekButtonIcon addTableButton;
    private static SleekButtonIcon removeTableButton;
    private static SleekSlider radiusSlider;
    private static SleekButtonIcon addButton;
    private static SleekButtonIcon removeButton;
    private static byte selectedTier;
    private static byte selectAnimal;

    public EditorSpawnsAnimalsUI()
    {
      Local local = Localization.read("/Editor/EditorSpawnsAnimals.dat");
      Bundle bundle = Bundles.getBundle("/Bundles/Textures/Edit/Icons/EditorSpawnsAnimals/EditorSpawnsAnimals.unity3d");
      EditorSpawnsAnimalsUI.container = new Sleek();
      EditorSpawnsAnimalsUI.container.positionOffset_X = 10;
      EditorSpawnsAnimalsUI.container.positionOffset_Y = 10;
      EditorSpawnsAnimalsUI.container.positionScale_X = 1f;
      EditorSpawnsAnimalsUI.container.sizeOffset_X = -20;
      EditorSpawnsAnimalsUI.container.sizeOffset_Y = -20;
      EditorSpawnsAnimalsUI.container.sizeScale_X = 1f;
      EditorSpawnsAnimalsUI.container.sizeScale_Y = 1f;
      EditorUI.window.add(EditorSpawnsAnimalsUI.container);
      EditorSpawnsAnimalsUI.active = false;
      EditorSpawnsAnimalsUI.tableScrollBox = new SleekScrollBox();
      EditorSpawnsAnimalsUI.tableScrollBox.positionOffset_X = -470;
      EditorSpawnsAnimalsUI.tableScrollBox.positionOffset_Y = 120;
      EditorSpawnsAnimalsUI.tableScrollBox.positionScale_X = 1f;
      EditorSpawnsAnimalsUI.tableScrollBox.sizeOffset_X = 470;
      EditorSpawnsAnimalsUI.tableScrollBox.sizeOffset_Y = 100;
      EditorSpawnsAnimalsUI.container.add((Sleek) EditorSpawnsAnimalsUI.tableScrollBox);
      EditorSpawnsAnimalsUI.tableNameField = new SleekField();
      EditorSpawnsAnimalsUI.tableNameField.positionOffset_X = -230;
      EditorSpawnsAnimalsUI.tableNameField.positionOffset_Y = 230;
      EditorSpawnsAnimalsUI.tableNameField.positionScale_X = 1f;
      EditorSpawnsAnimalsUI.tableNameField.sizeOffset_X = 230;
      EditorSpawnsAnimalsUI.tableNameField.sizeOffset_Y = 30;
      EditorSpawnsAnimalsUI.tableNameField.addLabel(local.format("TableNameFieldLabelText"), ESleekSide.LEFT);
      EditorSpawnsAnimalsUI.container.add((Sleek) EditorSpawnsAnimalsUI.tableNameField);
      EditorSpawnsAnimalsUI.addTableButton = new SleekButtonIcon((Texture2D) bundle.load("Add"));
      EditorSpawnsAnimalsUI.addTableButton.positionOffset_X = -230;
      EditorSpawnsAnimalsUI.addTableButton.positionOffset_Y = 270;
      EditorSpawnsAnimalsUI.addTableButton.positionScale_X = 1f;
      EditorSpawnsAnimalsUI.addTableButton.sizeOffset_X = 110;
      EditorSpawnsAnimalsUI.addTableButton.sizeOffset_Y = 30;
      EditorSpawnsAnimalsUI.addTableButton.text = local.format("AddTableButtonText");
      EditorSpawnsAnimalsUI.addTableButton.tooltip = local.format("AddTableButtonTooltip");
      EditorSpawnsAnimalsUI.addTableButton.onClickedButton = new ClickedButton(EditorSpawnsAnimalsUI.onClickedAddTableButton);
      EditorSpawnsAnimalsUI.container.add((Sleek) EditorSpawnsAnimalsUI.addTableButton);
      EditorSpawnsAnimalsUI.removeTableButton = new SleekButtonIcon((Texture2D) bundle.load("Remove"));
      EditorSpawnsAnimalsUI.removeTableButton.positionOffset_X = -110;
      EditorSpawnsAnimalsUI.removeTableButton.positionOffset_Y = 270;
      EditorSpawnsAnimalsUI.removeTableButton.positionScale_X = 1f;
      EditorSpawnsAnimalsUI.removeTableButton.sizeOffset_X = 110;
      EditorSpawnsAnimalsUI.removeTableButton.sizeOffset_Y = 30;
      EditorSpawnsAnimalsUI.removeTableButton.text = local.format("RemoveTableButtonText");
      EditorSpawnsAnimalsUI.removeTableButton.tooltip = local.format("RemoveTableButtonTooltip");
      EditorSpawnsAnimalsUI.removeTableButton.onClickedButton = new ClickedButton(EditorSpawnsAnimalsUI.onClickedRemoveTableButton);
      EditorSpawnsAnimalsUI.container.add((Sleek) EditorSpawnsAnimalsUI.removeTableButton);
      EditorSpawnsAnimalsUI.updateTables();
      EditorSpawnsAnimalsUI.spawnsScrollBox = new SleekScrollBox();
      EditorSpawnsAnimalsUI.spawnsScrollBox.positionOffset_X = -470;
      EditorSpawnsAnimalsUI.spawnsScrollBox.positionOffset_Y = 310;
      EditorSpawnsAnimalsUI.spawnsScrollBox.positionScale_X = 1f;
      EditorSpawnsAnimalsUI.spawnsScrollBox.sizeOffset_X = 470;
      EditorSpawnsAnimalsUI.spawnsScrollBox.sizeOffset_Y = -310;
      EditorSpawnsAnimalsUI.spawnsScrollBox.sizeScale_Y = 1f;
      EditorSpawnsAnimalsUI.spawnsScrollBox.area = new Rect(0.0f, 0.0f, 5f, 1000f);
      EditorSpawnsAnimalsUI.container.add((Sleek) EditorSpawnsAnimalsUI.spawnsScrollBox);
      EditorSpawnsAnimalsUI.tableColorPicker = new SleekColorPicker();
      EditorSpawnsAnimalsUI.tableColorPicker.positionOffset_X = 200;
      EditorSpawnsAnimalsUI.tableColorPicker.onColorPicked = new ColorPicked(EditorSpawnsAnimalsUI.onAnimalColorPicked);
      EditorSpawnsAnimalsUI.spawnsScrollBox.add((Sleek) EditorSpawnsAnimalsUI.tableColorPicker);
      EditorSpawnsAnimalsUI.tierNameField = new SleekField();
      EditorSpawnsAnimalsUI.tierNameField.positionOffset_X = 240;
      EditorSpawnsAnimalsUI.tierNameField.sizeOffset_X = 200;
      EditorSpawnsAnimalsUI.tierNameField.sizeOffset_Y = 30;
      EditorSpawnsAnimalsUI.tierNameField.addLabel(local.format("TierNameFieldLabelText"), ESleekSide.LEFT);
      EditorSpawnsAnimalsUI.spawnsScrollBox.add((Sleek) EditorSpawnsAnimalsUI.tierNameField);
      EditorSpawnsAnimalsUI.addTierButton = new SleekButtonIcon((Texture2D) bundle.load("Add"));
      EditorSpawnsAnimalsUI.addTierButton.positionOffset_X = 240;
      EditorSpawnsAnimalsUI.addTierButton.sizeOffset_X = 95;
      EditorSpawnsAnimalsUI.addTierButton.sizeOffset_Y = 30;
      EditorSpawnsAnimalsUI.addTierButton.text = local.format("AddTierButtonText");
      EditorSpawnsAnimalsUI.addTierButton.tooltip = local.format("AddTierButtonTooltip");
      EditorSpawnsAnimalsUI.addTierButton.onClickedButton = new ClickedButton(EditorSpawnsAnimalsUI.onClickedAddTierButton);
      EditorSpawnsAnimalsUI.spawnsScrollBox.add((Sleek) EditorSpawnsAnimalsUI.addTierButton);
      EditorSpawnsAnimalsUI.removeTierButton = new SleekButtonIcon((Texture2D) bundle.load("Remove"));
      EditorSpawnsAnimalsUI.removeTierButton.positionOffset_X = 345;
      EditorSpawnsAnimalsUI.removeTierButton.sizeOffset_X = 95;
      EditorSpawnsAnimalsUI.removeTierButton.sizeOffset_Y = 30;
      EditorSpawnsAnimalsUI.removeTierButton.text = local.format("RemoveTierButtonText");
      EditorSpawnsAnimalsUI.removeTierButton.tooltip = local.format("RemoveTierButtonTooltip");
      EditorSpawnsAnimalsUI.removeTierButton.onClickedButton = new ClickedButton(EditorSpawnsAnimalsUI.onClickedRemoveTierButton);
      EditorSpawnsAnimalsUI.spawnsScrollBox.add((Sleek) EditorSpawnsAnimalsUI.removeTierButton);
      EditorSpawnsAnimalsUI.animalIDField = new SleekUInt16Field();
      EditorSpawnsAnimalsUI.animalIDField.positionOffset_X = 240;
      EditorSpawnsAnimalsUI.animalIDField.sizeOffset_X = 200;
      EditorSpawnsAnimalsUI.animalIDField.sizeOffset_Y = 30;
      EditorSpawnsAnimalsUI.animalIDField.addLabel(local.format("AnimalIDFieldLabelText"), ESleekSide.LEFT);
      EditorSpawnsAnimalsUI.spawnsScrollBox.add((Sleek) EditorSpawnsAnimalsUI.animalIDField);
      EditorSpawnsAnimalsUI.addAnimalButton = new SleekButtonIcon((Texture2D) bundle.load("Add"));
      EditorSpawnsAnimalsUI.addAnimalButton.positionOffset_X = 240;
      EditorSpawnsAnimalsUI.addAnimalButton.sizeOffset_X = 95;
      EditorSpawnsAnimalsUI.addAnimalButton.sizeOffset_Y = 30;
      EditorSpawnsAnimalsUI.addAnimalButton.text = local.format("AddAnimalButtonText");
      EditorSpawnsAnimalsUI.addAnimalButton.tooltip = local.format("AddAnimalButtonTooltip");
      EditorSpawnsAnimalsUI.addAnimalButton.onClickedButton = new ClickedButton(EditorSpawnsAnimalsUI.onClickedAddAnimalButton);
      EditorSpawnsAnimalsUI.spawnsScrollBox.add((Sleek) EditorSpawnsAnimalsUI.addAnimalButton);
      EditorSpawnsAnimalsUI.removeAnimalButton = new SleekButtonIcon((Texture2D) bundle.load("Remove"));
      EditorSpawnsAnimalsUI.removeAnimalButton.positionOffset_X = 345;
      EditorSpawnsAnimalsUI.removeAnimalButton.sizeOffset_X = 95;
      EditorSpawnsAnimalsUI.removeAnimalButton.sizeOffset_Y = 30;
      EditorSpawnsAnimalsUI.removeAnimalButton.text = local.format("RemoveAnimalButtonText");
      EditorSpawnsAnimalsUI.removeAnimalButton.tooltip = local.format("RemoveAnimalButtonTooltip");
      EditorSpawnsAnimalsUI.removeAnimalButton.onClickedButton = new ClickedButton(EditorSpawnsAnimalsUI.onClickedRemoveAnimalButton);
      EditorSpawnsAnimalsUI.spawnsScrollBox.add((Sleek) EditorSpawnsAnimalsUI.removeAnimalButton);
      EditorSpawnsAnimalsUI.selectedBox = new SleekBox();
      EditorSpawnsAnimalsUI.selectedBox.positionOffset_X = -230;
      EditorSpawnsAnimalsUI.selectedBox.positionOffset_Y = 80;
      EditorSpawnsAnimalsUI.selectedBox.positionScale_X = 1f;
      EditorSpawnsAnimalsUI.selectedBox.sizeOffset_X = 230;
      EditorSpawnsAnimalsUI.selectedBox.sizeOffset_Y = 30;
      EditorSpawnsAnimalsUI.selectedBox.addLabel(local.format("SelectionBoxLabelText"), ESleekSide.LEFT);
      EditorSpawnsAnimalsUI.container.add((Sleek) EditorSpawnsAnimalsUI.selectedBox);
      EditorSpawnsAnimalsUI.updateSelection();
      EditorSpawnsAnimalsUI.radiusSlider = new SleekSlider();
      EditorSpawnsAnimalsUI.radiusSlider.positionOffset_Y = -100;
      EditorSpawnsAnimalsUI.radiusSlider.positionScale_Y = 1f;
      EditorSpawnsAnimalsUI.radiusSlider.sizeOffset_X = 200;
      EditorSpawnsAnimalsUI.radiusSlider.sizeOffset_Y = 20;
      EditorSpawnsAnimalsUI.radiusSlider.state = (float) ((int) EditorSpawns.radius - (int) EditorSpawns.MIN_REMOVE_SIZE) / (float) EditorSpawns.MAX_REMOVE_SIZE;
      EditorSpawnsAnimalsUI.radiusSlider.orientation = ESleekOrientation.HORIZONTAL;
      EditorSpawnsAnimalsUI.radiusSlider.addLabel(local.format("RadiusSliderLabelText"), ESleekSide.RIGHT);
      EditorSpawnsAnimalsUI.radiusSlider.onDragged = new Dragged(EditorSpawnsAnimalsUI.onDraggedRadiusSlider);
      EditorSpawnsAnimalsUI.container.add((Sleek) EditorSpawnsAnimalsUI.radiusSlider);
      EditorSpawnsAnimalsUI.addButton = new SleekButtonIcon((Texture2D) bundle.load("Add"));
      EditorSpawnsAnimalsUI.addButton.positionOffset_Y = -70;
      EditorSpawnsAnimalsUI.addButton.positionScale_Y = 1f;
      EditorSpawnsAnimalsUI.addButton.sizeOffset_X = 200;
      EditorSpawnsAnimalsUI.addButton.sizeOffset_Y = 30;
      EditorSpawnsAnimalsUI.addButton.text = local.format("AddButtonText", (object) ControlsSettings.tool_0);
      EditorSpawnsAnimalsUI.addButton.tooltip = local.format("AddButtonTooltip");
      EditorSpawnsAnimalsUI.addButton.onClickedButton = new ClickedButton(EditorSpawnsAnimalsUI.onClickedAddButton);
      EditorSpawnsAnimalsUI.container.add((Sleek) EditorSpawnsAnimalsUI.addButton);
      EditorSpawnsAnimalsUI.removeButton = new SleekButtonIcon((Texture2D) bundle.load("Remove"));
      EditorSpawnsAnimalsUI.removeButton.positionOffset_Y = -30;
      EditorSpawnsAnimalsUI.removeButton.positionScale_Y = 1f;
      EditorSpawnsAnimalsUI.removeButton.sizeOffset_X = 200;
      EditorSpawnsAnimalsUI.removeButton.sizeOffset_Y = 30;
      EditorSpawnsAnimalsUI.removeButton.text = local.format("RemoveButtonText", (object) ControlsSettings.tool_1);
      EditorSpawnsAnimalsUI.removeButton.tooltip = local.format("RemoveButtonTooltip");
      EditorSpawnsAnimalsUI.removeButton.onClickedButton = new ClickedButton(EditorSpawnsAnimalsUI.onClickedRemoveButton);
      EditorSpawnsAnimalsUI.container.add((Sleek) EditorSpawnsAnimalsUI.removeButton);
      bundle.unload();
    }

    public static void open()
    {
      if (EditorSpawnsAnimalsUI.active)
      {
        EditorSpawnsAnimalsUI.close();
      }
      else
      {
        EditorSpawnsAnimalsUI.active = true;
        EditorSpawns.isSpawning = true;
        EditorSpawns.spawnMode = ESpawnMode.ADD_ANIMAL;
        EditorSpawnsAnimalsUI.container.lerpPositionScale(0.0f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
      }
    }

    public static void close()
    {
      if (!EditorSpawnsAnimalsUI.active)
        return;
      EditorSpawnsAnimalsUI.active = false;
      EditorSpawns.isSpawning = false;
      EditorSpawnsAnimalsUI.container.lerpPositionScale(1f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
    }

    public static void updateTables()
    {
      if (EditorSpawnsAnimalsUI.tableButtons != null)
      {
        for (int index = 0; index < EditorSpawnsAnimalsUI.tableButtons.Length; ++index)
          EditorSpawnsAnimalsUI.tableScrollBox.remove((Sleek) EditorSpawnsAnimalsUI.tableButtons[index]);
      }
      EditorSpawnsAnimalsUI.tableButtons = new SleekButton[LevelAnimals.tables.Count];
      EditorSpawnsAnimalsUI.tableScrollBox.area = new Rect(0.0f, 0.0f, 5f, (float) (EditorSpawnsAnimalsUI.tableButtons.Length * 40 - 10));
      for (int index = 0; index < EditorSpawnsAnimalsUI.tableButtons.Length; ++index)
      {
        SleekButton sleekButton = new SleekButton();
        sleekButton.positionOffset_X = 240;
        sleekButton.positionOffset_Y = index * 40;
        sleekButton.sizeOffset_X = 200;
        sleekButton.sizeOffset_Y = 30;
        sleekButton.text = (string) (object) index + (object) " " + LevelAnimals.tables[index].name;
        sleekButton.onClickedButton = new ClickedButton(EditorSpawnsAnimalsUI.onClickedTableButton);
        EditorSpawnsAnimalsUI.tableScrollBox.add((Sleek) sleekButton);
        EditorSpawnsAnimalsUI.tableButtons[index] = sleekButton;
      }
    }

    public static void updateSelection()
    {
      if ((int) EditorSpawns.selectedAnimal < LevelAnimals.tables.Count)
      {
        AnimalTable animalTable = LevelAnimals.tables[(int) EditorSpawns.selectedAnimal];
        EditorSpawnsAnimalsUI.selectedBox.text = animalTable.name;
        EditorSpawnsAnimalsUI.tableColorPicker.state = animalTable.color;
        if (EditorSpawnsAnimalsUI.tierButtons != null)
        {
          for (int index = 0; index < EditorSpawnsAnimalsUI.tierButtons.Length; ++index)
            EditorSpawnsAnimalsUI.spawnsScrollBox.remove((Sleek) EditorSpawnsAnimalsUI.tierButtons[index]);
        }
        EditorSpawnsAnimalsUI.tierButtons = new SleekButton[animalTable.tiers.Count];
        for (int index = 0; index < EditorSpawnsAnimalsUI.tierButtons.Length; ++index)
        {
          AnimalTier animalTier = animalTable.tiers[index];
          SleekButton sleekButton = new SleekButton();
          sleekButton.positionOffset_X = 240;
          sleekButton.positionOffset_Y = 130 + index * 70;
          sleekButton.sizeOffset_X = 200;
          sleekButton.sizeOffset_Y = 30;
          sleekButton.text = animalTier.name;
          sleekButton.onClickedButton = new ClickedButton(EditorSpawnsAnimalsUI.onClickedTierButton);
          EditorSpawnsAnimalsUI.spawnsScrollBox.add((Sleek) sleekButton);
          SleekSlider sleekSlider = new SleekSlider();
          sleekSlider.positionOffset_Y = 40;
          sleekSlider.sizeOffset_X = 200;
          sleekSlider.sizeOffset_Y = 20;
          sleekSlider.orientation = ESleekOrientation.HORIZONTAL;
          sleekSlider.state = animalTier.chance;
          sleekSlider.addLabel((string) (object) Mathf.Floor(animalTier.chance * 100f) + (object) "%", ESleekSide.LEFT);
          sleekSlider.onDragged = new Dragged(EditorSpawnsAnimalsUI.onDraggedChanceSlider);
          sleekButton.add((Sleek) sleekSlider);
          EditorSpawnsAnimalsUI.tierButtons[index] = sleekButton;
        }
        EditorSpawnsAnimalsUI.tierNameField.positionOffset_Y = 130 + EditorSpawnsAnimalsUI.tierButtons.Length * 70;
        EditorSpawnsAnimalsUI.addTierButton.positionOffset_Y = 130 + EditorSpawnsAnimalsUI.tierButtons.Length * 70 + 40;
        EditorSpawnsAnimalsUI.removeTierButton.positionOffset_Y = 130 + EditorSpawnsAnimalsUI.tierButtons.Length * 70 + 40;
        if (EditorSpawnsAnimalsUI.animalButtons != null)
        {
          for (int index = 0; index < EditorSpawnsAnimalsUI.animalButtons.Length; ++index)
            EditorSpawnsAnimalsUI.spawnsScrollBox.remove((Sleek) EditorSpawnsAnimalsUI.animalButtons[index]);
        }
        if ((int) EditorSpawnsAnimalsUI.selectedTier < animalTable.tiers.Count)
        {
          EditorSpawnsAnimalsUI.animalButtons = new SleekButton[animalTable.tiers[(int) EditorSpawnsAnimalsUI.selectedTier].table.Count];
          for (int index = 0; index < EditorSpawnsAnimalsUI.animalButtons.Length; ++index)
          {
            SleekButton sleekButton = new SleekButton();
            sleekButton.positionOffset_X = 240;
            sleekButton.positionOffset_Y = 130 + EditorSpawnsAnimalsUI.tierButtons.Length * 70 + 80 + index * 40;
            sleekButton.sizeOffset_X = 200;
            sleekButton.sizeOffset_Y = 30;
            AnimalAsset animalAsset = (AnimalAsset) Assets.find(EAssetType.ANIMAL, animalTable.tiers[(int) EditorSpawnsAnimalsUI.selectedTier].table[index].animal);
            if (animalAsset != null)
              sleekButton.text = (string) (object) animalTable.tiers[(int) EditorSpawnsAnimalsUI.selectedTier].table[index].animal + (object) " " + animalAsset.animalName;
            else
              sleekButton.text = animalTable.tiers[(int) EditorSpawnsAnimalsUI.selectedTier].table[index].animal.ToString();
            sleekButton.onClickedButton = new ClickedButton(EditorSpawnsAnimalsUI.onClickAnimalButton);
            EditorSpawnsAnimalsUI.spawnsScrollBox.add((Sleek) sleekButton);
            EditorSpawnsAnimalsUI.animalButtons[index] = sleekButton;
          }
        }
        else
          EditorSpawnsAnimalsUI.animalButtons = new SleekButton[0];
        EditorSpawnsAnimalsUI.animalIDField.positionOffset_Y = 130 + EditorSpawnsAnimalsUI.tierButtons.Length * 70 + 80 + EditorSpawnsAnimalsUI.animalButtons.Length * 40;
        EditorSpawnsAnimalsUI.addAnimalButton.positionOffset_Y = 130 + EditorSpawnsAnimalsUI.tierButtons.Length * 70 + 80 + EditorSpawnsAnimalsUI.animalButtons.Length * 40 + 40;
        EditorSpawnsAnimalsUI.removeAnimalButton.positionOffset_Y = 130 + EditorSpawnsAnimalsUI.tierButtons.Length * 70 + 80 + EditorSpawnsAnimalsUI.animalButtons.Length * 40 + 40;
        EditorSpawnsAnimalsUI.spawnsScrollBox.area = new Rect(0.0f, 0.0f, 5f, (float) (130 + EditorSpawnsAnimalsUI.tierButtons.Length * 70 + 80 + EditorSpawnsAnimalsUI.animalButtons.Length * 40 + 70));
      }
      else
      {
        EditorSpawnsAnimalsUI.selectedBox.text = string.Empty;
        EditorSpawnsAnimalsUI.tableColorPicker.state = Color.white;
        if (EditorSpawnsAnimalsUI.tierButtons != null)
        {
          for (int index = 0; index < EditorSpawnsAnimalsUI.tierButtons.Length; ++index)
            EditorSpawnsAnimalsUI.spawnsScrollBox.remove((Sleek) EditorSpawnsAnimalsUI.tierButtons[index]);
        }
        EditorSpawnsAnimalsUI.tierButtons = (SleekButton[]) null;
        EditorSpawnsAnimalsUI.tierNameField.positionOffset_Y = 130;
        EditorSpawnsAnimalsUI.addTierButton.positionOffset_Y = 170;
        EditorSpawnsAnimalsUI.removeTierButton.positionOffset_Y = 170;
        if (EditorSpawnsAnimalsUI.animalButtons != null)
        {
          for (int index = 0; index < EditorSpawnsAnimalsUI.animalButtons.Length; ++index)
            EditorSpawnsAnimalsUI.spawnsScrollBox.remove((Sleek) EditorSpawnsAnimalsUI.animalButtons[index]);
        }
        EditorSpawnsAnimalsUI.animalButtons = (SleekButton[]) null;
        EditorSpawnsAnimalsUI.animalIDField.positionOffset_Y = 210;
        EditorSpawnsAnimalsUI.addAnimalButton.positionOffset_Y = 250;
        EditorSpawnsAnimalsUI.removeAnimalButton.positionOffset_Y = 250;
        EditorSpawnsAnimalsUI.spawnsScrollBox.area = new Rect(0.0f, 0.0f, 5f, 280f);
      }
    }

    private static void onClickedTableButton(SleekButton button)
    {
      EditorSpawns.selectedAnimal = (byte) (button.positionOffset_Y / 40);
      EditorSpawns.animalSpawn.GetComponent<Renderer>().material.color = LevelAnimals.tables[(int) EditorSpawns.selectedAnimal].color;
      EditorSpawnsAnimalsUI.updateSelection();
    }

    private static void onAnimalColorPicked(SleekColorPicker picker, Color color)
    {
      if ((int) EditorSpawns.selectedAnimal >= LevelAnimals.tables.Count)
        return;
      LevelAnimals.tables[(int) EditorSpawns.selectedAnimal].color = color;
    }

    private static void onClickedTierButton(SleekButton button)
    {
      if ((int) EditorSpawns.selectedAnimal >= LevelAnimals.tables.Count)
        return;
      EditorSpawnsAnimalsUI.selectedTier = (byte) ((button.positionOffset_Y - 130) / 70);
      EditorSpawnsAnimalsUI.updateSelection();
    }

    private static void onClickAnimalButton(SleekButton button)
    {
      if ((int) EditorSpawns.selectedAnimal >= LevelAnimals.tables.Count)
        return;
      EditorSpawnsAnimalsUI.selectAnimal = (byte) ((button.positionOffset_Y - 130 - EditorSpawnsAnimalsUI.tierButtons.Length * 70 - 80) / 40);
      EditorSpawnsAnimalsUI.updateSelection();
    }

    private static void onDraggedChanceSlider(SleekSlider slider, float state)
    {
      if ((int) EditorSpawns.selectedAnimal >= LevelAnimals.tables.Count)
        return;
      int tierIndex = (slider.parent.positionOffset_Y - 130) / 70;
      LevelAnimals.tables[(int) EditorSpawns.selectedAnimal].updateChance(tierIndex, state);
      for (int index = 0; index < LevelAnimals.tables[(int) EditorSpawns.selectedAnimal].tiers.Count; ++index)
      {
        AnimalTier animalTier = LevelAnimals.tables[(int) EditorSpawns.selectedAnimal].tiers[index];
        SleekSlider sleekSlider = (SleekSlider) EditorSpawnsAnimalsUI.tierButtons[index].children[0];
        if (index != tierIndex)
          sleekSlider.state = animalTier.chance;
        sleekSlider.updateLabel((string) (object) Mathf.Floor(animalTier.chance * 100f) + (object) "%");
      }
    }

    private static void onClickedAddTableButton(SleekButton button)
    {
      if (!(EditorSpawnsAnimalsUI.tableNameField.text != string.Empty))
        return;
      LevelAnimals.addTable(EditorSpawnsAnimalsUI.tableNameField.text);
      EditorSpawnsAnimalsUI.tableNameField.text = string.Empty;
      EditorSpawnsAnimalsUI.updateTables();
      EditorSpawnsAnimalsUI.tableScrollBox.state = new Vector2(0.0f, float.MaxValue);
    }

    private static void onClickedRemoveTableButton(SleekButton button)
    {
      if ((int) EditorSpawns.selectedAnimal >= LevelAnimals.tables.Count)
        return;
      LevelAnimals.removeTable();
      EditorSpawnsAnimalsUI.updateTables();
      EditorSpawnsAnimalsUI.updateSelection();
      EditorSpawnsAnimalsUI.tableScrollBox.state = new Vector2(0.0f, float.MaxValue);
    }

    private static void onClickedAddTierButton(SleekButton button)
    {
      if ((int) EditorSpawns.selectedAnimal >= LevelAnimals.tables.Count || !(EditorSpawnsAnimalsUI.tierNameField.text != string.Empty))
        return;
      LevelAnimals.tables[(int) EditorSpawns.selectedAnimal].addTier(EditorSpawnsAnimalsUI.tierNameField.text);
      EditorSpawnsAnimalsUI.tierNameField.text = string.Empty;
      EditorSpawnsAnimalsUI.updateSelection();
    }

    private static void onClickedRemoveTierButton(SleekButton button)
    {
      if ((int) EditorSpawns.selectedAnimal >= LevelAnimals.tables.Count || (int) EditorSpawnsAnimalsUI.selectedTier >= LevelAnimals.tables[(int) EditorSpawns.selectedAnimal].tiers.Count)
        return;
      LevelAnimals.tables[(int) EditorSpawns.selectedAnimal].removeTier((int) EditorSpawnsAnimalsUI.selectedTier);
      EditorSpawnsAnimalsUI.updateSelection();
    }

    private static void onClickedAddAnimalButton(SleekButton button)
    {
      if ((int) EditorSpawns.selectedAnimal >= LevelAnimals.tables.Count || (int) EditorSpawnsAnimalsUI.selectedTier >= LevelAnimals.tables[(int) EditorSpawns.selectedAnimal].tiers.Count)
        return;
      if ((AnimalAsset) Assets.find(EAssetType.ANIMAL, EditorSpawnsAnimalsUI.animalIDField.state) != null)
      {
        LevelAnimals.tables[(int) EditorSpawns.selectedAnimal].addAnimal(EditorSpawnsAnimalsUI.selectedTier, EditorSpawnsAnimalsUI.animalIDField.state);
        EditorSpawnsAnimalsUI.updateSelection();
        EditorSpawnsAnimalsUI.spawnsScrollBox.state = new Vector2(0.0f, float.MaxValue);
      }
      EditorSpawnsAnimalsUI.animalIDField.state = (ushort) 0;
    }

    private static void onClickedRemoveAnimalButton(SleekButton button)
    {
      if ((int) EditorSpawns.selectedAnimal >= LevelAnimals.tables.Count || (int) EditorSpawnsAnimalsUI.selectedTier >= LevelAnimals.tables[(int) EditorSpawns.selectedAnimal].tiers.Count || (int) EditorSpawnsAnimalsUI.selectAnimal >= LevelAnimals.tables[(int) EditorSpawns.selectedAnimal].tiers[(int) EditorSpawnsAnimalsUI.selectedTier].table.Count)
        return;
      LevelAnimals.tables[(int) EditorSpawns.selectedAnimal].removeAnimal(EditorSpawnsAnimalsUI.selectedTier, EditorSpawnsAnimalsUI.selectAnimal);
      EditorSpawnsAnimalsUI.updateSelection();
      EditorSpawnsAnimalsUI.spawnsScrollBox.state = new Vector2(0.0f, float.MaxValue);
    }

    private static void onDraggedRadiusSlider(SleekSlider slider, float state)
    {
      EditorSpawns.radius = (byte) ((double) EditorSpawns.MIN_REMOVE_SIZE + (double) state * (double) EditorSpawns.MAX_REMOVE_SIZE);
    }

    private static void onClickedAddButton(SleekButton button)
    {
      EditorSpawns.spawnMode = ESpawnMode.ADD_ANIMAL;
    }

    private static void onClickedRemoveButton(SleekButton button)
    {
      EditorSpawns.spawnMode = ESpawnMode.REMOVE_ANIMAL;
    }
  }
}
