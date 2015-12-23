// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.EditorLevelObjectsUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
  public class EditorLevelObjectsUI
  {
    private static Sleek container;
    public static bool active;
    private static Asset[] assets;
    private static SleekScrollBox assetsScrollBox;
    private static SleekBox selectedBox;
    private static SleekField searchField;
    private static SleekToggle largeToggle;
    private static SleekToggle mediumToggle;
    private static SleekToggle smallToggle;
    private static SleekBox dragBox;
    private static SleekSingleField snapTransformField;
    private static SleekSingleField snapRotationField;
    private static SleekButtonIcon transformButton;
    private static SleekButtonIcon rotateButton;
    private static SleekButtonState coordinateButton;

    public EditorLevelObjectsUI()
    {
      Local local = Localization.read("/Editor/EditorLevelObjects.dat");
      Bundle bundle = Bundles.getBundle("/Bundles/Textures/Edit/Icons/EditorLevelObjects/EditorLevelObjects.unity3d");
      EditorLevelObjectsUI.container = new Sleek();
      EditorLevelObjectsUI.container.positionOffset_X = 10;
      EditorLevelObjectsUI.container.positionOffset_Y = 10;
      EditorLevelObjectsUI.container.positionScale_X = 1f;
      EditorLevelObjectsUI.container.sizeOffset_X = -20;
      EditorLevelObjectsUI.container.sizeOffset_Y = -20;
      EditorLevelObjectsUI.container.sizeScale_X = 1f;
      EditorLevelObjectsUI.container.sizeScale_Y = 1f;
      EditorUI.window.add(EditorLevelObjectsUI.container);
      EditorLevelObjectsUI.active = false;
      EditorLevelObjectsUI.assetsScrollBox = new SleekScrollBox();
      EditorLevelObjectsUI.assetsScrollBox.positionOffset_X = -230;
      EditorLevelObjectsUI.assetsScrollBox.positionOffset_Y = 310;
      EditorLevelObjectsUI.assetsScrollBox.positionScale_X = 1f;
      EditorLevelObjectsUI.assetsScrollBox.sizeOffset_X = 230;
      EditorLevelObjectsUI.assetsScrollBox.sizeOffset_Y = -310;
      EditorLevelObjectsUI.assetsScrollBox.sizeScale_Y = 1f;
      EditorLevelObjectsUI.container.add((Sleek) EditorLevelObjectsUI.assetsScrollBox);
      EditorLevelObjectsUI.selectedBox = new SleekBox();
      EditorLevelObjectsUI.selectedBox.positionOffset_X = -230;
      EditorLevelObjectsUI.selectedBox.positionOffset_Y = 80;
      EditorLevelObjectsUI.selectedBox.positionScale_X = 1f;
      EditorLevelObjectsUI.selectedBox.sizeOffset_X = 230;
      EditorLevelObjectsUI.selectedBox.sizeOffset_Y = 30;
      EditorLevelObjectsUI.selectedBox.addLabel(local.format("SelectionBoxLabelText"), ESleekSide.LEFT);
      EditorLevelObjectsUI.container.add((Sleek) EditorLevelObjectsUI.selectedBox);
      EditorObjects.selected = (ushort) 0;
      EditorLevelObjectsUI.searchField = new SleekField();
      EditorLevelObjectsUI.searchField.positionOffset_X = -230;
      EditorLevelObjectsUI.searchField.positionOffset_Y = 120;
      EditorLevelObjectsUI.searchField.positionScale_X = 1f;
      EditorLevelObjectsUI.searchField.sizeOffset_X = 230;
      EditorLevelObjectsUI.searchField.sizeOffset_Y = 30;
      EditorLevelObjectsUI.searchField.onTyped = new Typed(EditorLevelObjectsUI.onTypedSearchField);
      EditorLevelObjectsUI.searchField.addLabel(local.format("SearchFieldLabelText"), ESleekSide.LEFT);
      EditorLevelObjectsUI.container.add((Sleek) EditorLevelObjectsUI.searchField);
      EditorLevelObjectsUI.largeToggle = new SleekToggle();
      EditorLevelObjectsUI.largeToggle.positionOffset_X = -230;
      EditorLevelObjectsUI.largeToggle.positionOffset_Y = 160;
      EditorLevelObjectsUI.largeToggle.positionScale_X = 1f;
      EditorLevelObjectsUI.largeToggle.sizeOffset_X = 40;
      EditorLevelObjectsUI.largeToggle.sizeOffset_Y = 40;
      EditorLevelObjectsUI.largeToggle.addLabel(local.format("LargeLabel"), ESleekSide.RIGHT);
      EditorLevelObjectsUI.largeToggle.state = true;
      EditorLevelObjectsUI.largeToggle.onToggled = new Toggled(EditorLevelObjectsUI.onToggledLargeToggle);
      EditorLevelObjectsUI.container.add((Sleek) EditorLevelObjectsUI.largeToggle);
      EditorLevelObjectsUI.mediumToggle = new SleekToggle();
      EditorLevelObjectsUI.mediumToggle.positionOffset_X = -230;
      EditorLevelObjectsUI.mediumToggle.positionOffset_Y = 210;
      EditorLevelObjectsUI.mediumToggle.positionScale_X = 1f;
      EditorLevelObjectsUI.mediumToggle.sizeOffset_X = 40;
      EditorLevelObjectsUI.mediumToggle.sizeOffset_Y = 40;
      EditorLevelObjectsUI.mediumToggle.addLabel(local.format("MediumLabel"), ESleekSide.RIGHT);
      EditorLevelObjectsUI.mediumToggle.state = true;
      EditorLevelObjectsUI.mediumToggle.onToggled = new Toggled(EditorLevelObjectsUI.onToggledMediumToggle);
      EditorLevelObjectsUI.container.add((Sleek) EditorLevelObjectsUI.mediumToggle);
      EditorLevelObjectsUI.smallToggle = new SleekToggle();
      EditorLevelObjectsUI.smallToggle.positionOffset_X = -230;
      EditorLevelObjectsUI.smallToggle.positionOffset_Y = 260;
      EditorLevelObjectsUI.smallToggle.positionScale_X = 1f;
      EditorLevelObjectsUI.smallToggle.sizeOffset_X = 40;
      EditorLevelObjectsUI.smallToggle.sizeOffset_Y = 40;
      EditorLevelObjectsUI.smallToggle.addLabel(local.format("SmallLabel"), ESleekSide.RIGHT);
      EditorLevelObjectsUI.smallToggle.state = true;
      EditorLevelObjectsUI.smallToggle.onToggled = new Toggled(EditorLevelObjectsUI.onToggledSmallToggle);
      EditorLevelObjectsUI.container.add((Sleek) EditorLevelObjectsUI.smallToggle);
      EditorObjects.onDragStarted = new DragStarted(EditorLevelObjectsUI.onDragStarted);
      EditorObjects.onDragStopped = new DragStopped(EditorLevelObjectsUI.onDragStopped);
      EditorLevelObjectsUI.dragBox = new SleekBox();
      EditorUI.window.add((Sleek) EditorLevelObjectsUI.dragBox);
      EditorLevelObjectsUI.dragBox.isVisible = false;
      EditorLevelObjectsUI.snapTransformField = new SleekSingleField();
      EditorLevelObjectsUI.snapTransformField.positionOffset_Y = -190;
      EditorLevelObjectsUI.snapTransformField.positionScale_Y = 1f;
      EditorLevelObjectsUI.snapTransformField.sizeOffset_X = 200;
      EditorLevelObjectsUI.snapTransformField.sizeOffset_Y = 30;
      EditorLevelObjectsUI.snapTransformField.text = (Mathf.Floor(EditorObjects.snapTransform * 100f) / 100f).ToString();
      EditorLevelObjectsUI.snapTransformField.addLabel(local.format("SnapTransformLabelText"), ESleekSide.RIGHT);
      EditorLevelObjectsUI.snapTransformField.onTypedSingle = new TypedSingle(EditorLevelObjectsUI.onTypedSnapTransformField);
      EditorLevelObjectsUI.container.add((Sleek) EditorLevelObjectsUI.snapTransformField);
      EditorLevelObjectsUI.snapRotationField = new SleekSingleField();
      EditorLevelObjectsUI.snapRotationField.positionOffset_Y = -150;
      EditorLevelObjectsUI.snapRotationField.positionScale_Y = 1f;
      EditorLevelObjectsUI.snapRotationField.sizeOffset_X = 200;
      EditorLevelObjectsUI.snapRotationField.sizeOffset_Y = 30;
      EditorLevelObjectsUI.snapRotationField.text = (Mathf.Floor(EditorObjects.snapRotation * 100f) / 100f).ToString();
      EditorLevelObjectsUI.snapRotationField.addLabel(local.format("SnapRotationLabelText"), ESleekSide.RIGHT);
      EditorLevelObjectsUI.snapRotationField.onTypedSingle = new TypedSingle(EditorLevelObjectsUI.onTypedSnapRotationField);
      EditorLevelObjectsUI.container.add((Sleek) EditorLevelObjectsUI.snapRotationField);
      EditorLevelObjectsUI.transformButton = new SleekButtonIcon((Texture2D) bundle.load("Transform"));
      EditorLevelObjectsUI.transformButton.positionOffset_Y = -110;
      EditorLevelObjectsUI.transformButton.positionScale_Y = 1f;
      EditorLevelObjectsUI.transformButton.sizeOffset_X = 200;
      EditorLevelObjectsUI.transformButton.sizeOffset_Y = 30;
      EditorLevelObjectsUI.transformButton.text = local.format("TransformButtonText", (object) ControlsSettings.tool_0);
      EditorLevelObjectsUI.transformButton.tooltip = local.format("TransformButtonTooltip");
      EditorLevelObjectsUI.transformButton.onClickedButton = new ClickedButton(EditorLevelObjectsUI.onClickedTransformButton);
      EditorLevelObjectsUI.container.add((Sleek) EditorLevelObjectsUI.transformButton);
      EditorLevelObjectsUI.rotateButton = new SleekButtonIcon((Texture2D) bundle.load("Rotate"));
      EditorLevelObjectsUI.rotateButton.positionOffset_Y = -70;
      EditorLevelObjectsUI.rotateButton.positionScale_Y = 1f;
      EditorLevelObjectsUI.rotateButton.sizeOffset_X = 200;
      EditorLevelObjectsUI.rotateButton.sizeOffset_Y = 30;
      EditorLevelObjectsUI.rotateButton.text = local.format("RotateButtonText", (object) ControlsSettings.tool_1);
      EditorLevelObjectsUI.rotateButton.tooltip = local.format("RotateButtonTooltip");
      EditorLevelObjectsUI.rotateButton.onClickedButton = new ClickedButton(EditorLevelObjectsUI.onClickedRotateButton);
      EditorLevelObjectsUI.container.add((Sleek) EditorLevelObjectsUI.rotateButton);
      EditorLevelObjectsUI.coordinateButton = new SleekButtonState(new GUIContent[2]
      {
        new GUIContent(local.format("CoordinateButtonTextGlobal"), (Texture) bundle.load("Global")),
        new GUIContent(local.format("CoordinateButtonTextLocal"), (Texture) bundle.load("Local"))
      });
      EditorLevelObjectsUI.coordinateButton.positionOffset_Y = -30;
      EditorLevelObjectsUI.coordinateButton.positionScale_Y = 1f;
      EditorLevelObjectsUI.coordinateButton.sizeOffset_X = 200;
      EditorLevelObjectsUI.coordinateButton.sizeOffset_Y = 30;
      EditorLevelObjectsUI.coordinateButton.tooltip = local.format("CoordinateButtonTooltip");
      EditorLevelObjectsUI.coordinateButton.onSwappedState = new SwappedState(EditorLevelObjectsUI.onSwappedStateCoordinate);
      EditorLevelObjectsUI.container.add((Sleek) EditorLevelObjectsUI.coordinateButton);
      bundle.unload();
      EditorLevelObjectsUI.onAssetsRefreshed();
      Assets.onAssetsRefreshed = new AssetsRefreshed(EditorLevelObjectsUI.onAssetsRefreshed);
    }

    public static void open()
    {
      if (EditorLevelObjectsUI.active)
      {
        EditorLevelObjectsUI.close();
      }
      else
      {
        EditorLevelObjectsUI.active = true;
        EditorObjects.isBuilding = true;
        EditorUI.message(EEditorMessage.OBJECTS);
        EditorLevelObjectsUI.container.lerpPositionScale(0.0f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
      }
    }

    public static void close()
    {
      if (!EditorLevelObjectsUI.active)
        return;
      EditorLevelObjectsUI.active = false;
      EditorObjects.isBuilding = false;
      EditorLevelObjectsUI.container.lerpPositionScale(1f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
    }

    private static void updateSelection(string search, bool large, bool medium, bool small)
    {
      EditorLevelObjectsUI.assets = Assets.find(EAssetType.OBJECT);
      List<Asset> list = new List<Asset>();
      for (int index = 0; index < EditorLevelObjectsUI.assets.Length; ++index)
      {
        if ((large || ((ObjectAsset) EditorLevelObjectsUI.assets[index]).type != EObjectType.LARGE) && (medium || ((ObjectAsset) EditorLevelObjectsUI.assets[index]).type != EObjectType.MEDIUM) && ((small || ((ObjectAsset) EditorLevelObjectsUI.assets[index]).type != EObjectType.SMALL) && (!(search != string.Empty) || NameTool.checkNames(search, ((ObjectAsset) EditorLevelObjectsUI.assets[index]).objectName))))
          list.Add(EditorLevelObjectsUI.assets[index]);
      }
      EditorLevelObjectsUI.assets = list.ToArray();
      bool flag1 = false;
      while (!flag1)
      {
        bool flag2 = false;
        for (int index = 0; index < EditorLevelObjectsUI.assets.Length - 1; ++index)
        {
          ObjectAsset objectAsset1 = (ObjectAsset) EditorLevelObjectsUI.assets[index];
          ObjectAsset objectAsset2 = (ObjectAsset) EditorLevelObjectsUI.assets[index + 1];
          if (objectAsset1.objectName.CompareTo(objectAsset2.objectName) > 0)
          {
            EditorLevelObjectsUI.assets[index] = (Asset) objectAsset2;
            EditorLevelObjectsUI.assets[index + 1] = (Asset) objectAsset1;
            flag2 = true;
          }
        }
        if (!flag2)
          flag1 = true;
      }
      EditorLevelObjectsUI.assetsScrollBox.remove();
      EditorLevelObjectsUI.assetsScrollBox.area = new Rect(0.0f, 0.0f, 5f, (float) (EditorLevelObjectsUI.assets.Length * 40 - 10));
      for (int index = 0; index < EditorLevelObjectsUI.assets.Length; ++index)
      {
        ObjectAsset objectAsset = (ObjectAsset) EditorLevelObjectsUI.assets[index];
        SleekButton sleekButton = new SleekButton();
        sleekButton.positionOffset_Y = index * 40;
        sleekButton.sizeOffset_X = 200;
        sleekButton.sizeOffset_Y = 30;
        sleekButton.text = objectAsset.objectName;
        sleekButton.onClickedButton = new ClickedButton(EditorLevelObjectsUI.onClickedAssetButton);
        EditorLevelObjectsUI.assetsScrollBox.add((Sleek) sleekButton);
      }
    }

    private static void onAssetsRefreshed()
    {
      EditorLevelObjectsUI.updateSelection(EditorLevelObjectsUI.searchField.text, EditorLevelObjectsUI.largeToggle.state, EditorLevelObjectsUI.mediumToggle.state, EditorLevelObjectsUI.smallToggle.state);
    }

    private static void onClickedAssetButton(SleekButton button)
    {
      int index = button.positionOffset_Y / 40;
      EditorObjects.selected = EditorLevelObjectsUI.assets[index].id;
      EditorLevelObjectsUI.selectedBox.text = ((ObjectAsset) EditorLevelObjectsUI.assets[index]).objectName;
    }

    private static void onDragStarted(int min_x, int min_y, int max_x, int max_y)
    {
      EditorLevelObjectsUI.dragBox.positionOffset_X = min_x;
      EditorLevelObjectsUI.dragBox.positionOffset_Y = min_y;
      EditorLevelObjectsUI.dragBox.sizeOffset_X = max_x - min_x;
      EditorLevelObjectsUI.dragBox.sizeOffset_Y = max_y - min_y;
      EditorLevelObjectsUI.dragBox.isVisible = true;
    }

    private static void onDragStopped()
    {
      EditorLevelObjectsUI.dragBox.isVisible = false;
    }

    private static void onTypedSearchField(SleekField field, string value)
    {
      EditorLevelObjectsUI.updateSelection(value, EditorLevelObjectsUI.largeToggle.state, EditorLevelObjectsUI.mediumToggle.state, EditorLevelObjectsUI.smallToggle.state);
    }

    private static void onToggledLargeToggle(SleekToggle toggle, bool state)
    {
      EditorLevelObjectsUI.updateSelection(EditorLevelObjectsUI.searchField.text, state, EditorLevelObjectsUI.mediumToggle.state, EditorLevelObjectsUI.smallToggle.state);
    }

    private static void onToggledMediumToggle(SleekToggle toggle, bool state)
    {
      EditorLevelObjectsUI.updateSelection(EditorLevelObjectsUI.searchField.text, EditorLevelObjectsUI.largeToggle.state, state, EditorLevelObjectsUI.smallToggle.state);
    }

    private static void onToggledSmallToggle(SleekToggle toggle, bool state)
    {
      EditorLevelObjectsUI.updateSelection(EditorLevelObjectsUI.searchField.text, EditorLevelObjectsUI.largeToggle.state, EditorLevelObjectsUI.mediumToggle.state, state);
    }

    private static void onTypedSnapTransformField(SleekSingleField field, float value)
    {
      EditorObjects.snapTransform = value;
    }

    private static void onTypedSnapRotationField(SleekSingleField field, float value)
    {
      EditorObjects.snapRotation = value;
    }

    private static void onClickedTransformButton(SleekButton button)
    {
      EditorObjects.dragMode = EDragMode.TRANSFORM;
    }

    private static void onClickedRotateButton(SleekButton button)
    {
      EditorObjects.dragMode = EDragMode.ROTATE;
    }

    private static void onSwappedStateCoordinate(SleekButtonState button, int index)
    {
      EditorObjects.dragCoordinate = (EDragCoordinate) index;
    }
  }
}
