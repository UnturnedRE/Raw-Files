// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.EditorEnvironmentNodesUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class EditorEnvironmentNodesUI
  {
    private static Sleek container;
    public static bool active;
    private static SleekField nameField;
    private static SleekSlider radiusSlider;
    private static SleekUInt16Field itemIDField;
    private static SleekUInt32Field costField;
    private static SleekButtonState typeButton;

    public EditorEnvironmentNodesUI()
    {
      Local local = Localization.read("/Editor/EditorEnvironmentNodes.dat");
      EditorEnvironmentNodesUI.container = new Sleek();
      EditorEnvironmentNodesUI.container.positionOffset_X = 10;
      EditorEnvironmentNodesUI.container.positionOffset_Y = 10;
      EditorEnvironmentNodesUI.container.positionScale_X = 1f;
      EditorEnvironmentNodesUI.container.sizeOffset_X = -20;
      EditorEnvironmentNodesUI.container.sizeOffset_Y = -20;
      EditorEnvironmentNodesUI.container.sizeScale_X = 1f;
      EditorEnvironmentNodesUI.container.sizeScale_Y = 1f;
      EditorUI.window.add(EditorEnvironmentNodesUI.container);
      EditorEnvironmentNodesUI.active = false;
      EditorEnvironmentNodesUI.nameField = new SleekField();
      EditorEnvironmentNodesUI.nameField.positionOffset_X = -200;
      EditorEnvironmentNodesUI.nameField.positionOffset_Y = 80;
      EditorEnvironmentNodesUI.nameField.positionScale_X = 1f;
      EditorEnvironmentNodesUI.nameField.sizeOffset_X = 200;
      EditorEnvironmentNodesUI.nameField.sizeOffset_Y = 30;
      EditorEnvironmentNodesUI.nameField.addLabel(local.format("Name_Label"), ESleekSide.LEFT);
      EditorEnvironmentNodesUI.nameField.onTyped = new Typed(EditorEnvironmentNodesUI.onTypedNameField);
      EditorEnvironmentNodesUI.nameField.maxLength = 32;
      EditorEnvironmentNodesUI.container.add((Sleek) EditorEnvironmentNodesUI.nameField);
      EditorEnvironmentNodesUI.nameField.isVisible = false;
      EditorEnvironmentNodesUI.radiusSlider = new SleekSlider();
      EditorEnvironmentNodesUI.radiusSlider.positionOffset_X = -200;
      EditorEnvironmentNodesUI.radiusSlider.positionOffset_Y = 80;
      EditorEnvironmentNodesUI.radiusSlider.positionScale_X = 1f;
      EditorEnvironmentNodesUI.radiusSlider.sizeOffset_X = 200;
      EditorEnvironmentNodesUI.radiusSlider.sizeOffset_Y = 20;
      EditorEnvironmentNodesUI.radiusSlider.addLabel(local.format("Radius_Label"), ESleekSide.LEFT);
      EditorEnvironmentNodesUI.radiusSlider.orientation = ESleekOrientation.HORIZONTAL;
      EditorEnvironmentNodesUI.radiusSlider.onDragged = new Dragged(EditorEnvironmentNodesUI.onDraggedRadiusSlider);
      EditorEnvironmentNodesUI.container.add((Sleek) EditorEnvironmentNodesUI.radiusSlider);
      EditorEnvironmentNodesUI.radiusSlider.isVisible = false;
      EditorEnvironmentNodesUI.itemIDField = new SleekUInt16Field();
      EditorEnvironmentNodesUI.itemIDField.positionOffset_X = -200;
      EditorEnvironmentNodesUI.itemIDField.positionOffset_Y = 110;
      EditorEnvironmentNodesUI.itemIDField.positionScale_X = 1f;
      EditorEnvironmentNodesUI.itemIDField.sizeOffset_X = 200;
      EditorEnvironmentNodesUI.itemIDField.sizeOffset_Y = 30;
      EditorEnvironmentNodesUI.itemIDField.addLabel(local.format("Item_ID_Label"), ESleekSide.LEFT);
      EditorEnvironmentNodesUI.itemIDField.onTypedUInt16 = new TypedUInt16(EditorEnvironmentNodesUI.onTypedItemIDField);
      EditorEnvironmentNodesUI.container.add((Sleek) EditorEnvironmentNodesUI.itemIDField);
      EditorEnvironmentNodesUI.itemIDField.isVisible = false;
      EditorEnvironmentNodesUI.costField = new SleekUInt32Field();
      EditorEnvironmentNodesUI.costField.positionOffset_X = -200;
      EditorEnvironmentNodesUI.costField.positionOffset_Y = 150;
      EditorEnvironmentNodesUI.costField.positionScale_X = 1f;
      EditorEnvironmentNodesUI.costField.sizeOffset_X = 200;
      EditorEnvironmentNodesUI.costField.sizeOffset_Y = 30;
      EditorEnvironmentNodesUI.costField.addLabel(local.format("Cost_Label"), ESleekSide.LEFT);
      EditorEnvironmentNodesUI.costField.onTypedUInt32 = new TypedUInt32(EditorEnvironmentNodesUI.onTypedCostField);
      EditorEnvironmentNodesUI.container.add((Sleek) EditorEnvironmentNodesUI.costField);
      EditorEnvironmentNodesUI.costField.isVisible = false;
      EditorEnvironmentNodesUI.typeButton = new SleekButtonState(new GUIContent[3]
      {
        new GUIContent(local.format("Location")),
        new GUIContent(local.format("Safezone")),
        new GUIContent(local.format("Purchase"))
      });
      EditorEnvironmentNodesUI.typeButton.positionOffset_Y = -30;
      EditorEnvironmentNodesUI.typeButton.positionScale_Y = 1f;
      EditorEnvironmentNodesUI.typeButton.sizeOffset_X = 200;
      EditorEnvironmentNodesUI.typeButton.sizeOffset_Y = 30;
      EditorEnvironmentNodesUI.typeButton.tooltip = local.format("Type_Tooltip");
      EditorEnvironmentNodesUI.typeButton.onSwappedState = new SwappedState(EditorEnvironmentNodesUI.onSwappedType);
      EditorEnvironmentNodesUI.container.add((Sleek) EditorEnvironmentNodesUI.typeButton);
    }

    public static void open()
    {
      if (EditorEnvironmentNodesUI.active)
      {
        EditorEnvironmentNodesUI.close();
      }
      else
      {
        EditorEnvironmentNodesUI.active = true;
        EditorNodes.isNoding = true;
        EditorUI.message(EEditorMessage.NODES);
        EditorEnvironmentNodesUI.container.lerpPositionScale(0.0f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
      }
    }

    public static void close()
    {
      if (!EditorEnvironmentNodesUI.active)
        return;
      EditorEnvironmentNodesUI.active = false;
      EditorNodes.isNoding = false;
      EditorEnvironmentNodesUI.container.lerpPositionScale(1f, 0.0f, ESleekLerp.EXPONENTIAL, 20f);
    }

    public static void updateSelection(Node node)
    {
      if (node != null)
      {
        if (node.type == ENodeType.LOCATION)
          EditorEnvironmentNodesUI.nameField.text = ((LocationNode) node).name;
        else if (node.type == ENodeType.SAFEZONE)
          EditorEnvironmentNodesUI.radiusSlider.state = ((SafezoneNode) node).radius;
        else if (node.type == ENodeType.PURCHASE)
        {
          EditorEnvironmentNodesUI.radiusSlider.state = ((PurchaseNode) node).radius;
          EditorEnvironmentNodesUI.itemIDField.state = ((PurchaseNode) node).id;
          EditorEnvironmentNodesUI.costField.state = ((PurchaseNode) node).cost;
        }
      }
      EditorEnvironmentNodesUI.nameField.isVisible = node != null && node.type == ENodeType.LOCATION;
      EditorEnvironmentNodesUI.radiusSlider.isVisible = node != null && (node.type == ENodeType.SAFEZONE || node.type == ENodeType.PURCHASE);
      EditorEnvironmentNodesUI.itemIDField.isVisible = node != null && node.type == ENodeType.PURCHASE;
      EditorEnvironmentNodesUI.costField.isVisible = node != null && node.type == ENodeType.PURCHASE;
    }

    private static void onTypedNameField(SleekField field, string state)
    {
      if (EditorNodes.node == null || EditorNodes.node.type != ENodeType.LOCATION)
        return;
      ((LocationNode) EditorNodes.node).name = state;
    }

    private static void onDraggedRadiusSlider(SleekSlider slider, float state)
    {
      if (EditorNodes.node == null)
        return;
      if (EditorNodes.node.type == ENodeType.SAFEZONE)
      {
        ((SafezoneNode) EditorNodes.node).radius = state;
      }
      else
      {
        if (EditorNodes.node.type != ENodeType.PURCHASE)
          return;
        ((PurchaseNode) EditorNodes.node).radius = state;
      }
    }

    private static void onTypedItemIDField(SleekUInt16Field field, ushort state)
    {
      if (EditorNodes.node == null || EditorNodes.node.type != ENodeType.PURCHASE)
        return;
      ((PurchaseNode) EditorNodes.node).id = state;
    }

    private static void onTypedCostField(SleekUInt32Field field, uint state)
    {
      if (EditorNodes.node == null || EditorNodes.node.type != ENodeType.PURCHASE)
        return;
      ((PurchaseNode) EditorNodes.node).cost = state;
    }

    private static void onSwappedType(SleekButtonState button, int state)
    {
      EditorNodes.nodeType = (ENodeType) state;
    }
  }
}
