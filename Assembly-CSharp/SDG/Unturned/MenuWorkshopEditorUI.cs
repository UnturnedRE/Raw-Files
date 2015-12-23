// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.MenuWorkshopEditorUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class MenuWorkshopEditorUI
  {
    private static Sleek container;
    public static bool active;
    private static LevelInfo[] levels;
    private static SleekScrollBox levelScrollBox;
    private static SleekLevel[] levelButtons;
    private static SleekField mapNameField;
    private static SleekButtonState mapSizeState;
    private static SleekButtonState mapTypeState;
    private static SleekButtonIcon addButton;
    private static SleekButtonIconConfirm removeButton;
    private static SleekButtonIcon editButton;
    private static SleekBox selectedBox;

    public MenuWorkshopEditorUI()
    {
      Local local = Localization.read("/Menu/Workshop/MenuWorkshopEditor.dat");
      Bundle bundle = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Workshop/MenuWorkshopEditor/MenuWorkshopEditor.unity3d");
      MenuWorkshopEditorUI.container = new Sleek();
      MenuWorkshopEditorUI.container.positionOffset_X = 10;
      MenuWorkshopEditorUI.container.positionOffset_Y = 10;
      MenuWorkshopEditorUI.container.positionScale_Y = 1f;
      MenuWorkshopEditorUI.container.sizeOffset_X = -20;
      MenuWorkshopEditorUI.container.sizeOffset_Y = -20;
      MenuWorkshopEditorUI.container.sizeScale_X = 1f;
      MenuWorkshopEditorUI.container.sizeScale_Y = 1f;
      MenuUI.container.add(MenuWorkshopEditorUI.container);
      MenuWorkshopEditorUI.active = false;
      MenuWorkshopEditorUI.levelScrollBox = new SleekScrollBox();
      MenuWorkshopEditorUI.levelScrollBox.positionOffset_X = -200;
      MenuWorkshopEditorUI.levelScrollBox.positionOffset_Y = 140;
      MenuWorkshopEditorUI.levelScrollBox.positionScale_X = 0.75f;
      MenuWorkshopEditorUI.levelScrollBox.sizeOffset_X = 430;
      MenuWorkshopEditorUI.levelScrollBox.sizeOffset_Y = -140;
      MenuWorkshopEditorUI.levelScrollBox.sizeScale_Y = 1f;
      MenuWorkshopEditorUI.levelScrollBox.area = new Rect(0.0f, 0.0f, 5f, 0.0f);
      MenuWorkshopEditorUI.container.add((Sleek) MenuWorkshopEditorUI.levelScrollBox);
      MenuWorkshopEditorUI.selectedBox = new SleekBox();
      MenuWorkshopEditorUI.selectedBox.positionOffset_X = -200;
      MenuWorkshopEditorUI.selectedBox.positionOffset_Y = 100;
      MenuWorkshopEditorUI.selectedBox.positionScale_X = 0.75f;
      MenuWorkshopEditorUI.selectedBox.sizeOffset_X = 400;
      MenuWorkshopEditorUI.selectedBox.sizeOffset_Y = 30;
      MenuWorkshopEditorUI.selectedBox.addLabel(local.format("Selection_Label"), ESleekSide.LEFT);
      MenuWorkshopEditorUI.container.add((Sleek) MenuWorkshopEditorUI.selectedBox);
      MenuWorkshopEditorUI.mapNameField = new SleekField();
      MenuWorkshopEditorUI.mapNameField.positionOffset_X = -100;
      MenuWorkshopEditorUI.mapNameField.positionOffset_Y = -115;
      MenuWorkshopEditorUI.mapNameField.positionScale_X = 0.25f;
      MenuWorkshopEditorUI.mapNameField.positionScale_Y = 0.5f;
      MenuWorkshopEditorUI.mapNameField.sizeOffset_X = 200;
      MenuWorkshopEditorUI.mapNameField.sizeOffset_Y = 30;
      MenuWorkshopEditorUI.mapNameField.maxLength = 24;
      MenuWorkshopEditorUI.mapNameField.addLabel(local.format("Name_Field_Label"), ESleekSide.RIGHT);
      MenuWorkshopEditorUI.container.add((Sleek) MenuWorkshopEditorUI.mapNameField);
      MenuWorkshopEditorUI.mapSizeState = new SleekButtonState(new GUIContent[3]
      {
        new GUIContent(MenuPlaySingleplayerUI.localization.format("Small")),
        new GUIContent(MenuPlaySingleplayerUI.localization.format("Medium")),
        new GUIContent(MenuPlaySingleplayerUI.localization.format("Large"))
      });
      MenuWorkshopEditorUI.mapSizeState.positionOffset_X = -100;
      MenuWorkshopEditorUI.mapSizeState.positionOffset_Y = -75;
      MenuWorkshopEditorUI.mapSizeState.positionScale_X = 0.25f;
      MenuWorkshopEditorUI.mapSizeState.positionScale_Y = 0.5f;
      MenuWorkshopEditorUI.mapSizeState.sizeOffset_X = 200;
      MenuWorkshopEditorUI.mapSizeState.sizeOffset_Y = 30;
      MenuWorkshopEditorUI.container.add((Sleek) MenuWorkshopEditorUI.mapSizeState);
      MenuWorkshopEditorUI.mapTypeState = new SleekButtonState(new GUIContent[2]
      {
        new GUIContent(MenuPlaySingleplayerUI.localization.format("Survival")),
        new GUIContent(MenuPlaySingleplayerUI.localization.format("Horde"))
      });
      MenuWorkshopEditorUI.mapTypeState.positionOffset_X = -100;
      MenuWorkshopEditorUI.mapTypeState.positionOffset_Y = -35;
      MenuWorkshopEditorUI.mapTypeState.positionScale_X = 0.25f;
      MenuWorkshopEditorUI.mapTypeState.positionScale_Y = 0.5f;
      MenuWorkshopEditorUI.mapTypeState.sizeOffset_X = 200;
      MenuWorkshopEditorUI.mapTypeState.sizeOffset_Y = 30;
      MenuWorkshopEditorUI.container.add((Sleek) MenuWorkshopEditorUI.mapTypeState);
      MenuWorkshopEditorUI.addButton = new SleekButtonIcon((Texture2D) bundle.load("Add"));
      MenuWorkshopEditorUI.addButton.positionOffset_X = -100;
      MenuWorkshopEditorUI.addButton.positionOffset_Y = 5;
      MenuWorkshopEditorUI.addButton.positionScale_X = 0.25f;
      MenuWorkshopEditorUI.addButton.positionScale_Y = 0.5f;
      MenuWorkshopEditorUI.addButton.sizeOffset_X = 200;
      MenuWorkshopEditorUI.addButton.sizeOffset_Y = 30;
      MenuWorkshopEditorUI.addButton.text = local.format("Add_Button");
      MenuWorkshopEditorUI.addButton.tooltip = local.format("Add_Button_Tooltip");
      MenuWorkshopEditorUI.addButton.onClickedButton = new ClickedButton(MenuWorkshopEditorUI.onClickedAddButton);
      MenuWorkshopEditorUI.container.add((Sleek) MenuWorkshopEditorUI.addButton);
      MenuWorkshopEditorUI.removeButton = new SleekButtonIconConfirm((Texture2D) bundle.load("Remove"), local.format("Remove_Button_Confirm"), local.format("Remove_Button_Confirm_Tooltip"), local.format("Remove_Button_Deny"), local.format("Remove_Button_Deny_Tooltip"));
      MenuWorkshopEditorUI.removeButton.positionOffset_X = -100;
      MenuWorkshopEditorUI.removeButton.positionOffset_Y = 45;
      MenuWorkshopEditorUI.removeButton.positionScale_X = 0.25f;
      MenuWorkshopEditorUI.removeButton.positionScale_Y = 0.5f;
      MenuWorkshopEditorUI.removeButton.sizeOffset_X = 200;
      MenuWorkshopEditorUI.removeButton.sizeOffset_Y = 30;
      MenuWorkshopEditorUI.removeButton.text = local.format("Remove_Button");
      MenuWorkshopEditorUI.removeButton.tooltip = local.format("Remove_Button_Tooltip");
      MenuWorkshopEditorUI.removeButton.onConfirmed = new Confirm(MenuWorkshopEditorUI.onClickedRemoveButton);
      MenuWorkshopEditorUI.container.add((Sleek) MenuWorkshopEditorUI.removeButton);
      MenuWorkshopEditorUI.editButton = new SleekButtonIcon((Texture2D) bundle.load("Edit"));
      MenuWorkshopEditorUI.editButton.positionOffset_X = -100;
      MenuWorkshopEditorUI.editButton.positionOffset_Y = 85;
      MenuWorkshopEditorUI.editButton.positionScale_X = 0.25f;
      MenuWorkshopEditorUI.editButton.positionScale_Y = 0.5f;
      MenuWorkshopEditorUI.editButton.sizeOffset_X = 200;
      MenuWorkshopEditorUI.editButton.sizeOffset_Y = 30;
      MenuWorkshopEditorUI.editButton.text = local.format("Edit_Button");
      MenuWorkshopEditorUI.editButton.tooltip = local.format("Edit_Button_Tooltip");
      MenuWorkshopEditorUI.editButton.onClickedButton = new ClickedButton(MenuWorkshopEditorUI.onClickedEditButton);
      MenuWorkshopEditorUI.container.add((Sleek) MenuWorkshopEditorUI.editButton);
      bundle.unload();
      MenuWorkshopEditorUI.onLevelsRefreshed();
      Level.onLevelsRefreshed += new LevelsRefreshed(MenuWorkshopEditorUI.onLevelsRefreshed);
    }

    public static void open()
    {
      if (MenuWorkshopEditorUI.active)
      {
        MenuWorkshopEditorUI.close();
      }
      else
      {
        MenuWorkshopEditorUI.active = true;
        MenuWorkshopEditorUI.removeButton.reset();
        MenuWorkshopEditorUI.container.lerpPositionScale(0.0f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
      }
    }

    public static void close()
    {
      if (!MenuWorkshopEditorUI.active)
        return;
      MenuWorkshopEditorUI.active = false;
      MenuWorkshopEditorUI.container.lerpPositionScale(0.0f, 1f, ESleekLerp.EXPONENTIAL, 20f);
    }

    private static void onClickedLevel(SleekLevel level, byte index)
    {
      if ((int) index >= MenuWorkshopEditorUI.levels.Length || MenuWorkshopEditorUI.levels[(int) index] == null || !MenuWorkshopEditorUI.levels[(int) index].isEditable)
        return;
      PlaySettings.editorMap = MenuWorkshopEditorUI.levels[(int) index].name;
      MenuWorkshopEditorUI.selectedBox.text = PlaySettings.editorMap;
    }

    private static void onClickedAddButton(SleekButton button)
    {
      if (!(MenuWorkshopEditorUI.mapNameField.text != string.Empty))
        return;
      Level.add(MenuWorkshopEditorUI.mapNameField.text, (ELevelSize) (MenuWorkshopEditorUI.mapSizeState.state + 1), (ELevelType) MenuWorkshopEditorUI.mapTypeState.state);
      MenuWorkshopEditorUI.mapNameField.text = string.Empty;
    }

    private static void onClickedRemoveButton(SleekButton button)
    {
      if (PlaySettings.editorMap == null || PlaySettings.editorMap.Length == 0)
        return;
      for (int index = 0; index < MenuWorkshopEditorUI.levels.Length; ++index)
      {
        if (MenuWorkshopEditorUI.levels[index] != null && MenuWorkshopEditorUI.levels[index].name == PlaySettings.editorMap && MenuWorkshopEditorUI.levels[index].isEditable)
          Level.remove(MenuWorkshopEditorUI.levels[index].name);
      }
    }

    private static void onClickedEditButton(SleekButton button)
    {
      if (PlaySettings.editorMap == null || PlaySettings.editorMap.Length == 0)
        return;
      for (int index = 0; index < MenuWorkshopEditorUI.levels.Length; ++index)
      {
        if (MenuWorkshopEditorUI.levels[index] != null && MenuWorkshopEditorUI.levels[index].name == PlaySettings.editorMap && MenuWorkshopEditorUI.levels[index].isEditable)
        {
          MenuSettings.save();
          Level.edit(MenuWorkshopEditorUI.levels[index]);
        }
      }
    }

    private static void onLevelsRefreshed()
    {
      MenuWorkshopEditorUI.levelScrollBox.remove();
      MenuWorkshopEditorUI.levels = Level.getLevels();
      bool flag = false;
      MenuWorkshopEditorUI.levelButtons = new SleekLevel[MenuWorkshopEditorUI.levels.Length];
      for (int index = 0; index < MenuWorkshopEditorUI.levels.Length; ++index)
      {
        if (MenuWorkshopEditorUI.levels[index] != null)
        {
          SleekLevel sleekLevel = new SleekLevel(MenuWorkshopEditorUI.levels[index], true);
          sleekLevel.positionOffset_Y = index * 110;
          sleekLevel.onClickedLevel = new ClickedLevel(MenuWorkshopEditorUI.onClickedLevel);
          MenuWorkshopEditorUI.levelScrollBox.add((Sleek) sleekLevel);
          MenuWorkshopEditorUI.levelButtons[index] = sleekLevel;
          if (!flag && MenuWorkshopEditorUI.levels[index].name == PlaySettings.editorMap)
            flag = true;
        }
      }
      if (MenuWorkshopEditorUI.levels.Length == 0)
        PlaySettings.editorMap = string.Empty;
      else if (!flag || PlaySettings.editorMap == null || PlaySettings.editorMap.Length == 0)
        PlaySettings.editorMap = MenuWorkshopEditorUI.levels[0].name;
      MenuWorkshopEditorUI.selectedBox.text = PlaySettings.editorMap;
      MenuWorkshopEditorUI.levelScrollBox.area = new Rect(0.0f, 0.0f, 5f, (float) (MenuWorkshopEditorUI.levels.Length * 110 - 10));
    }
  }
}
