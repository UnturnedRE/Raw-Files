// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.EditorObjects
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
  public class EditorObjects : MonoBehaviour
  {
    public static readonly byte SAVEDATA_VERSION = (byte) 1;
    private static List<EditorCopy> copies = new List<EditorCopy>();
    public static DragStarted onDragStarted;
    public static DragStopped onDragStopped;
    public static float snapTransform;
    public static float snapRotation;
    private static bool _isBuilding;
    private static Vector2 _dragStart;
    private static Vector2 _dragEnd;
    private static bool _isDragging;
    public static ushort selected;
    private static List<EditorSelection> selection;
    private static Vector3 copyPosition;
    private static Quaternion copyRotation;
    private static Transform group;
    private static Transform handle;
    private static Transform transformHandle;
    private static Transform planeHandle;
    private static Transform rotateHandle;
    private static Vector3 handleOffset;
    private static Plane handlePlane;
    private static EDragType _handleType;
    private static EDragMode _dragMode;
    private static EDragCoordinate _dragCoordinate;
    private static Vector3 transformOrigin;
    private static Quaternion rotateOrigin;
    private static Vector3 mouseOrigin;
    private static bool rotateInverted;
    private static List<EditorDrag> dragable;

    public static bool isBuilding
    {
      get
      {
        return EditorObjects._isBuilding;
      }
      set
      {
        EditorObjects._isBuilding = value;
        if (EditorObjects.isBuilding)
          return;
        EditorObjects.handle.gameObject.SetActive(false);
        EditorObjects.clearSelection();
      }
    }

    public static Vector2 dragStart
    {
      get
      {
        return EditorObjects._dragStart;
      }
    }

    public static Vector2 dragEnd
    {
      get
      {
        return EditorObjects._dragEnd;
      }
    }

    public static bool isDragging
    {
      get
      {
        return EditorObjects._isDragging;
      }
    }

    public static EDragType handleType
    {
      get
      {
        return EditorObjects._handleType;
      }
      set
      {
        EditorObjects._handleType = value;
      }
    }

    public static EDragMode dragMode
    {
      get
      {
        return EditorObjects._dragMode;
      }
      set
      {
        EditorObjects._dragMode = value;
        if (EditorObjects.dragMode == EDragMode.TRANSFORM)
        {
          EditorObjects.transformHandle.gameObject.SetActive(true);
          EditorObjects.planeHandle.gameObject.SetActive(true);
          EditorObjects.rotateHandle.gameObject.SetActive(false);
        }
        else if (EditorObjects.dragMode == EDragMode.ROTATE)
        {
          EditorObjects.transformHandle.gameObject.SetActive(false);
          EditorObjects.planeHandle.gameObject.SetActive(false);
          EditorObjects.rotateHandle.gameObject.SetActive(true);
        }
        EditorObjects.calculateHandleOffsets();
      }
    }

    public static EDragCoordinate dragCoordinate
    {
      get
      {
        return EditorObjects._dragCoordinate;
      }
      set
      {
        EditorObjects._dragCoordinate = value;
        EditorObjects.calculateHandleOffsets();
      }
    }

    public static void applySelection()
    {
      for (int index = 0; index < EditorObjects.selection.Count; ++index)
        LevelObjects.registerTransformObject(EditorObjects.selection[index].transform, EditorObjects.selection[index].transform.position, EditorObjects.selection[index].transform.rotation, EditorObjects.selection[index].fromPosition, EditorObjects.selection[index].fromRotation);
    }

    public static void pointSelection()
    {
      for (int index = 0; index < EditorObjects.selection.Count; ++index)
      {
        EditorObjects.selection[index].fromPosition = EditorObjects.selection[index].transform.position;
        EditorObjects.selection[index].fromRotation = EditorObjects.selection[index].transform.rotation;
      }
    }

    public static void addSelection(Transform select)
    {
      HighlighterTool.highlight(select, Color.yellow);
      EditorObjects.selection.Add(new EditorSelection(select, select.parent, select.position, select.rotation));
      EditorObjects.calculateHandleOffsets();
    }

    public static void removeSelection(Transform select)
    {
      for (int index = 0; index < EditorObjects.selection.Count; ++index)
      {
        if ((Object) EditorObjects.selection[index].transform == (Object) select)
        {
          HighlighterTool.unhighlight(select);
          EditorObjects.selection[index].transform.parent = EditorObjects.selection[index].parent;
          EditorObjects.selection.RemoveAt(index);
          break;
        }
      }
      EditorObjects.calculateHandleOffsets();
    }

    private static void clearSelection()
    {
      for (int index = 0; index < EditorObjects.selection.Count; ++index)
      {
        if ((Object) EditorObjects.selection[index].transform != (Object) null)
        {
          HighlighterTool.unhighlight(EditorObjects.selection[index].transform);
          EditorObjects.selection[index].transform.parent = EditorObjects.selection[index].parent;
        }
      }
      EditorObjects.selection.Clear();
      EditorObjects.calculateHandleOffsets();
    }

    public static bool containsSelection(Transform select)
    {
      for (int index = 0; index < EditorObjects.selection.Count; ++index)
      {
        if ((Object) EditorObjects.selection[index].transform == (Object) select)
          return true;
      }
      return false;
    }

    private static void calculateHandleOffsets()
    {
      if (EditorObjects.selection.Count == 0)
      {
        EditorObjects.handle.rotation = Quaternion.identity;
        EditorObjects.handle.gameObject.SetActive(false);
      }
      else
      {
        for (int index = 0; index < EditorObjects.selection.Count; ++index)
          EditorObjects.selection[index].transform.parent = (Transform) null;
        EditorObjects.handle.rotation = EditorObjects.dragCoordinate != EDragCoordinate.GLOBAL ? EditorObjects.selection[0].transform.rotation : Quaternion.identity;
        EditorObjects.handle.position = Vector3.zero;
        for (int index = 0; index < EditorObjects.selection.Count; ++index)
          EditorObjects.handle.position += EditorObjects.selection[index].transform.position;
        EditorObjects.handle.position /= (float) EditorObjects.selection.Count;
        EditorObjects.handle.gameObject.SetActive(true);
        EditorObjects.updateGroup();
        for (int index = 0; index < EditorObjects.selection.Count; ++index)
          EditorObjects.selection[index].transform.parent = EditorObjects.group;
      }
    }

    private static void updateGroup()
    {
      EditorObjects.group.transform.position = EditorObjects.handle.transform.position;
      EditorObjects.group.transform.rotation = EditorObjects.handle.transform.rotation;
    }

    private static void transformGroup(Vector3 normal, Vector3 dir)
    {
      Vector3 vector3_1 = Camera.main.WorldToScreenPoint(EditorObjects.transformOrigin);
      Vector3 vector3_2 = Camera.main.WorldToScreenPoint(EditorObjects.transformOrigin + normal) - vector3_1;
      float num = Vector3.Dot(Input.mousePosition - EditorObjects.mouseOrigin, vector3_2.normalized) / vector3_2.magnitude;
      if (Input.GetKey(ControlsSettings.snap))
        num = (float) (int) ((double) num / (double) EditorObjects.snapTransform) * EditorObjects.snapTransform;
      Vector3 vector3_3 = EditorObjects.transformOrigin + num * normal;
      vector3_3.x = Mathf.Clamp(vector3_3.x, (float) -Level.size, (float) Level.size);
      vector3_3.y = Mathf.Clamp(vector3_3.y, 0.0f, Level.HEIGHT);
      vector3_3.z = Mathf.Clamp(vector3_3.z, (float) -Level.size, (float) Level.size);
      EditorObjects.handle.position = vector3_3;
      EditorObjects.updateGroup();
    }

    private static void planeGroup(Vector3 normal)
    {
      EditorObjects.handlePlane.SetNormalAndPosition(normal, EditorObjects.transformOrigin);
      float enter;
      EditorObjects.handlePlane.Raycast(EditorInteract.ray, out enter);
      Vector3 vector3 = EditorInteract.ray.origin + EditorInteract.ray.direction * enter - EditorObjects.handleOffset + Vector3.Project(EditorObjects.handleOffset, normal);
      if (Input.GetKey(ControlsSettings.snap))
      {
        vector3.x = (float) (int) ((double) vector3.x / (double) EditorObjects.snapTransform) * EditorObjects.snapTransform;
        vector3.y = (float) (int) ((double) vector3.y / (double) EditorObjects.snapTransform) * EditorObjects.snapTransform;
        vector3.z = (float) (int) ((double) vector3.z / (double) EditorObjects.snapTransform) * EditorObjects.snapTransform;
      }
      vector3.x = Mathf.Clamp(vector3.x, (float) -Level.size, (float) Level.size);
      vector3.y = Mathf.Clamp(vector3.y, 0.0f, Level.HEIGHT);
      vector3.z = Mathf.Clamp(vector3.z, (float) -Level.size, (float) Level.size);
      EditorObjects.handle.position = vector3;
      EditorObjects.updateGroup();
    }

    private static void rotateGroup(Vector3 normal, Vector3 axis)
    {
      Vector3 vector3 = axis * (Input.mousePosition.x - EditorObjects.mouseOrigin.x) * (!EditorObjects.rotateInverted ? 1f : -1f);
      float num = vector3.x + vector3.y + vector3.z;
      if (Input.GetKey(ControlsSettings.snap))
        num = (float) (int) ((double) num / (double) EditorObjects.snapRotation) * EditorObjects.snapRotation;
      EditorObjects.handle.rotation = (double) Vector3.Dot(Camera.main.transform.forward, normal) >= 0.0 ? EditorObjects.rotateOrigin * Quaternion.Euler(-axis * num) : EditorObjects.rotateOrigin * Quaternion.Euler(axis * num);
      EditorObjects.updateGroup();
    }

    private void Update()
    {
      if (!EditorObjects.isBuilding)
        return;
      if (GUIUtility.hotControl == 0)
      {
        if (EditorInteract.isFlying)
        {
          EditorObjects.handleType = EDragType.NONE;
          if (!EditorObjects.isDragging)
            return;
          EditorObjects._dragStart = Vector2.zero;
          EditorObjects._dragEnd = Vector2.zero;
          EditorObjects._isDragging = false;
          if (EditorObjects.onDragStopped != null)
            EditorObjects.onDragStopped();
          EditorObjects.clearSelection();
          return;
        }
        if (EditorObjects.handleType != EDragType.NONE)
        {
          if (!Input.GetKey(ControlsSettings.primary))
          {
            EditorObjects.applySelection();
            EditorObjects.handleType = EDragType.NONE;
          }
          else
          {
            if (EditorObjects.handleType == EDragType.TRANSFORM_X)
              EditorObjects.transformGroup(EditorObjects.handle.right, EditorObjects.handle.up);
            else if (EditorObjects.handleType == EDragType.TRANSFORM_Y)
              EditorObjects.transformGroup(EditorObjects.handle.up, EditorObjects.handle.right);
            else if (EditorObjects.handleType == EDragType.TRANSFORM_Z)
              EditorObjects.transformGroup(EditorObjects.handle.forward, EditorObjects.handle.up);
            else if (EditorObjects.handleType == EDragType.PLANE_X)
              EditorObjects.planeGroup(EditorObjects.handle.right);
            else if (EditorObjects.handleType == EDragType.PLANE_Y)
              EditorObjects.planeGroup(EditorObjects.handle.up);
            else if (EditorObjects.handleType == EDragType.PLANE_Z)
              EditorObjects.planeGroup(EditorObjects.handle.forward);
            if (EditorObjects.handleType == EDragType.ROTATION_X)
              EditorObjects.rotateGroup(EditorObjects.handle.right, Vector3.right);
            else if (EditorObjects.handleType == EDragType.ROTATION_Y)
              EditorObjects.rotateGroup(EditorObjects.handle.up, Vector3.up);
            else if (EditorObjects.handleType == EDragType.ROTATION_Z)
              EditorObjects.rotateGroup(EditorObjects.handle.forward, Vector3.forward);
          }
        }
        if (Input.GetKeyDown(ControlsSettings.tool_0))
          EditorObjects.dragMode = EDragMode.TRANSFORM;
        if (Input.GetKeyDown(ControlsSettings.tool_1))
          EditorObjects.dragMode = EDragMode.ROTATE;
        if ((Input.GetKeyDown(KeyCode.Delete) || Input.GetKeyDown(KeyCode.Backspace)) && EditorObjects.selection.Count > 0)
        {
          for (int index = 0; index < EditorObjects.selection.Count; ++index)
          {
            EditorObjects.selection[index].transform.parent = EditorObjects.selection[index].parent;
            LevelObjects.registerRemoveObject(EditorObjects.selection[index].transform);
          }
          EditorObjects.selection.Clear();
          EditorObjects.calculateHandleOffsets();
        }
        if (Input.GetKeyDown(KeyCode.Z) && Input.GetKey(KeyCode.LeftControl))
        {
          EditorObjects.clearSelection();
          LevelObjects.undo();
        }
        if (Input.GetKeyDown(KeyCode.X) && Input.GetKey(KeyCode.LeftControl))
        {
          EditorObjects.clearSelection();
          LevelObjects.redo();
        }
        if (Input.GetKeyDown(KeyCode.B) && EditorObjects.selection.Count > 0 && Input.GetKey(KeyCode.LeftControl))
        {
          EditorObjects.copyPosition = EditorObjects.handle.position;
          EditorObjects.copyRotation = EditorObjects.handle.rotation;
        }
        if (Input.GetKeyDown(KeyCode.N) && EditorObjects.selection.Count > 0 && (EditorObjects.copyPosition != Vector3.zero && Input.GetKey(KeyCode.LeftControl)))
        {
          EditorObjects.pointSelection();
          EditorObjects.handle.position = EditorObjects.copyPosition;
          EditorObjects.handle.rotation = EditorObjects.copyRotation;
          EditorObjects.updateGroup();
          EditorObjects.applySelection();
        }
        if (Input.GetKeyDown(KeyCode.C) && EditorObjects.selection.Count > 0 && Input.GetKey(KeyCode.LeftControl))
        {
          EditorObjects.copies.Clear();
          for (int index = 0; index < EditorObjects.selection.Count; ++index)
            EditorObjects.copies.Add(new EditorCopy(EditorObjects.selection[index].transform.position, EditorObjects.selection[index].transform.rotation, LevelObjects.getID(EditorObjects.selection[index].transform)));
        }
        if (Input.GetKeyDown(KeyCode.V) && EditorObjects.copies.Count > 0 && Input.GetKey(KeyCode.LeftControl))
        {
          EditorObjects.clearSelection();
          for (int index = 0; index < EditorObjects.copies.Count; ++index)
          {
            if ((int) EditorObjects.copies[index].id != 0)
            {
              Transform select = LevelObjects.registerAddObject(EditorObjects.copies[index].position, EditorObjects.copies[index].rotation, EditorObjects.copies[index].id);
              if ((Object) select != (Object) null)
                EditorObjects.addSelection(select);
            }
            else
              Debug.LogError((object) "Failed to copy invalid object.");
          }
        }
        if (EditorObjects.handleType == EDragType.NONE)
        {
          if (Input.GetKeyDown(ControlsSettings.primary))
          {
            if ((Object) EditorInteract.logicHit.transform != (Object) null)
            {
              EditorObjects.mouseOrigin = Input.mousePosition;
              EditorObjects.transformOrigin = EditorObjects.handle.position;
              EditorObjects.rotateOrigin = EditorObjects.handle.rotation;
              EditorObjects.handleOffset = EditorInteract.logicHit.point - EditorObjects.handle.position;
              EditorObjects.pointSelection();
              if (EditorInteract.logicHit.transform.name == "Arrow_X")
                EditorObjects.handleType = EDragType.TRANSFORM_X;
              else if (EditorInteract.logicHit.transform.name == "Arrow_Y")
                EditorObjects.handleType = EDragType.TRANSFORM_Y;
              else if (EditorInteract.logicHit.transform.name == "Arrow_Z")
                EditorObjects.handleType = EDragType.TRANSFORM_Z;
              else if (EditorInteract.logicHit.transform.name == "Plane_X")
                EditorObjects.handleType = EDragType.PLANE_X;
              else if (EditorInteract.logicHit.transform.name == "Plane_Y")
                EditorObjects.handleType = EDragType.PLANE_Y;
              else if (EditorInteract.logicHit.transform.name == "Plane_Z")
                EditorObjects.handleType = EDragType.PLANE_Z;
              else if (EditorInteract.logicHit.transform.name == "Circle_X")
              {
                EditorObjects.rotateInverted = (double) Vector3.Dot(EditorInteract.logicHit.point - EditorObjects.handle.position, Camera.main.transform.up) < 0.0;
                EditorObjects.handleType = EDragType.ROTATION_X;
              }
              else if (EditorInteract.logicHit.transform.name == "Circle_Y")
              {
                EditorObjects.rotateInverted = (double) Vector3.Dot(EditorInteract.logicHit.point - EditorObjects.handle.position, Camera.main.transform.up) < 0.0;
                EditorObjects.handleType = EDragType.ROTATION_Y;
              }
              else if (EditorInteract.logicHit.transform.name == "Circle_Z")
              {
                EditorObjects.rotateInverted = (double) Vector3.Dot(EditorInteract.logicHit.point - EditorObjects.handle.position, Camera.main.transform.up) < 0.0;
                EditorObjects.handleType = EDragType.ROTATION_Z;
              }
            }
            else if ((Object) EditorInteract.objectHit.transform != (Object) null)
            {
              if (Input.GetKey(ControlsSettings.modify))
              {
                if (EditorObjects.containsSelection(EditorInteract.objectHit.transform))
                  EditorObjects.removeSelection(EditorInteract.objectHit.transform);
                else
                  EditorObjects.addSelection(EditorInteract.objectHit.transform);
              }
              else if (EditorObjects.containsSelection(EditorInteract.objectHit.transform))
              {
                EditorObjects.clearSelection();
              }
              else
              {
                EditorObjects.clearSelection();
                EditorObjects.addSelection(EditorInteract.objectHit.transform);
              }
            }
            else
            {
              if (!EditorObjects.isDragging)
              {
                EditorObjects._dragStart.x = EditorUI.window.mouse_x;
                EditorObjects._dragStart.y = EditorUI.window.mouse_y;
              }
              if (!Input.GetKey(ControlsSettings.modify))
                EditorObjects.clearSelection();
            }
          }
          else if (Input.GetKey(ControlsSettings.primary) && (double) EditorObjects.dragStart.x != 0.0)
          {
            EditorObjects._dragEnd.x = EditorUI.window.mouse_x;
            EditorObjects._dragEnd.y = EditorUI.window.mouse_y;
            if (EditorObjects.isDragging || (double) Mathf.Abs(EditorObjects.dragEnd.x - EditorObjects.dragStart.x) > 50.0 || (double) Mathf.Abs(EditorObjects.dragEnd.x - EditorObjects.dragStart.x) > 50.0)
            {
              int min_x = (int) EditorObjects.dragStart.x;
              int min_y = (int) EditorObjects.dragStart.y;
              if ((double) EditorObjects.dragEnd.x < (double) EditorObjects.dragStart.x)
                min_x = (int) EditorObjects.dragEnd.x;
              if ((double) EditorObjects.dragEnd.y < (double) EditorObjects.dragStart.y)
                min_y = (int) EditorObjects.dragEnd.y;
              int max_x = (int) EditorObjects.dragEnd.x;
              int max_y = (int) EditorObjects.dragEnd.y;
              if ((double) EditorObjects.dragStart.x > (double) EditorObjects.dragEnd.x)
                max_x = (int) EditorObjects.dragStart.x;
              if ((double) EditorObjects.dragStart.y > (double) EditorObjects.dragEnd.y)
                max_y = (int) EditorObjects.dragStart.y;
              if (EditorObjects.onDragStarted != null)
                EditorObjects.onDragStarted(min_x, min_y, max_x, max_y);
              if (!EditorObjects.isDragging)
              {
                EditorObjects._isDragging = true;
                EditorObjects.dragable.Clear();
                byte regionX = Editor.editor.movement.region_x;
                byte regionY = Editor.editor.movement.region_y;
                if (Regions.checkSafe(regionX, regionY))
                {
                  for (int index1 = (int) regionX - 1; index1 <= (int) regionX + 1; ++index1)
                  {
                    for (int index2 = (int) regionY - 1; index2 <= (int) regionY + 1; ++index2)
                    {
                      if (Regions.checkSafe((byte) index1, (byte) index2) && LevelObjects.regions[index1, index2])
                      {
                        for (int index3 = 0; index3 < LevelObjects.objects[index1, index2].Count; ++index3)
                        {
                          LevelObject levelObject = LevelObjects.objects[index1, index2][index3];
                          if (!((Object) levelObject.transform == (Object) null))
                          {
                            Vector3 newScreen = Camera.main.WorldToScreenPoint(levelObject.transform.position);
                            if ((double) newScreen.z >= 0.0)
                            {
                              newScreen.y = (float) Screen.height - newScreen.y;
                              EditorObjects.dragable.Add(new EditorDrag(levelObject.transform, newScreen));
                            }
                          }
                        }
                      }
                    }
                  }
                }
              }
              if (!Input.GetKey(ControlsSettings.modify))
              {
                for (int index = 0; index < EditorObjects.selection.Count; ++index)
                {
                  Vector3 vector3 = Camera.main.WorldToScreenPoint(EditorObjects.selection[index].transform.position);
                  if ((double) vector3.z < 0.0)
                  {
                    EditorObjects.removeSelection(EditorObjects.selection[index].transform);
                  }
                  else
                  {
                    vector3.y = (float) Screen.height - vector3.y;
                    if ((double) vector3.x < (double) min_x || (double) vector3.y < (double) min_y || ((double) vector3.x > (double) max_x || (double) vector3.y > (double) max_y))
                      EditorObjects.removeSelection(EditorObjects.selection[index].transform);
                  }
                }
              }
              for (int index = 0; index < EditorObjects.dragable.Count; ++index)
              {
                EditorDrag editorDrag = EditorObjects.dragable[index];
                if (!((Object) editorDrag.transform == (Object) null) && !((Object) editorDrag.transform.parent == (Object) EditorObjects.group) && (double) editorDrag.screen.x >= (double) min_x && ((double) editorDrag.screen.y >= (double) min_y && (double) editorDrag.screen.x <= (double) max_x) && (double) editorDrag.screen.y <= (double) max_y)
                  EditorObjects.addSelection(editorDrag.transform);
              }
            }
          }
          if (EditorObjects.selection.Count > 0)
          {
            if (Input.GetKeyDown(ControlsSettings.tool_2) && (Object) EditorInteract.worldHit.transform != (Object) null)
            {
              EditorObjects.pointSelection();
              EditorObjects.handle.position = EditorInteract.worldHit.point;
              if (Input.GetKey(ControlsSettings.snap))
                EditorObjects.handle.position += EditorInteract.worldHit.normal * EditorObjects.snapTransform;
              EditorObjects.updateGroup();
              EditorObjects.applySelection();
            }
            if (Input.GetKeyDown(ControlsSettings.focus))
              Camera.main.transform.parent.position = EditorObjects.handle.position - 15f * Camera.main.transform.forward;
          }
          else if ((Object) EditorInteract.worldHit.transform != (Object) null)
          {
            if (EditorInteract.worldHit.transform.tag == "Large" || EditorInteract.worldHit.transform.tag == "Medium" || EditorInteract.worldHit.transform.tag == "Small")
            {
              ObjectAsset objectAsset = (ObjectAsset) Assets.find(EAssetType.OBJECT, ushort.Parse(EditorInteract.worldHit.transform.name));
              if (objectAsset != null)
                EditorUI.hint(EEditorMessage.FOCUS, objectAsset.objectName);
            }
            if (Input.GetKeyDown(ControlsSettings.tool_2))
            {
              EditorObjects.handle.position = EditorInteract.worldHit.point;
              if (Input.GetKey(ControlsSettings.snap))
                EditorObjects.handle.position += EditorInteract.worldHit.normal * EditorObjects.snapTransform;
              EditorObjects.handle.rotation = Quaternion.Euler(-90f, 0.0f, 0.0f);
              if ((ObjectAsset) Assets.find(EAssetType.OBJECT, EditorObjects.selected) != null)
              {
                Transform select = LevelObjects.registerAddObject(EditorObjects.handle.position, EditorObjects.handle.rotation, EditorObjects.selected);
                if ((Object) select != (Object) null)
                  EditorObjects.addSelection(select);
              }
            }
          }
        }
      }
      if (!Input.GetKeyUp(ControlsSettings.primary) || (double) EditorObjects.dragStart.x == 0.0)
        return;
      EditorObjects._dragStart = Vector2.zero;
      if (!EditorObjects.isDragging)
        return;
      EditorObjects._dragEnd = Vector2.zero;
      EditorObjects._isDragging = false;
      if (EditorObjects.onDragStopped == null)
        return;
      EditorObjects.onDragStopped();
    }

    private void LateUpdate()
    {
      if (EditorObjects.selection.Count <= 0)
        return;
      float magnitude = (EditorObjects.handle.position - Camera.main.transform.position).magnitude;
      EditorObjects.handle.localScale = new Vector3(0.1f * magnitude, 0.1f * magnitude, 0.1f * magnitude);
    }

    private void Start()
    {
      EditorObjects._isBuilding = false;
      EditorObjects._dragStart = Vector2.zero;
      EditorObjects._dragEnd = Vector2.zero;
      EditorObjects._isDragging = false;
      EditorObjects.selection = new List<EditorSelection>();
      EditorObjects.handlePlane = new Plane();
      EditorObjects.group = new GameObject().transform;
      EditorObjects.group.name = "Group";
      EditorObjects.group.parent = Level.editing;
      EditorObjects.handle = ((GameObject) Object.Instantiate(Resources.Load("Edit/Handles"))).transform;
      EditorObjects.handle.name = "Handle";
      EditorObjects.handle.gameObject.SetActive(false);
      EditorObjects.handle.parent = Level.editing;
      EditorObjects.transformHandle = EditorObjects.handle.FindChild("Transform");
      EditorObjects.planeHandle = EditorObjects.handle.FindChild("Plane");
      EditorObjects.rotateHandle = EditorObjects.handle.FindChild("Rotate");
      EditorObjects.dragMode = EDragMode.TRANSFORM;
      EditorObjects.dragCoordinate = EDragCoordinate.GLOBAL;
      EditorObjects.dragable = new List<EditorDrag>();
      if (ReadWrite.fileExists(Level.info.path + "/Editor/Objects.dat", false, false))
      {
        Block block = ReadWrite.readBlock(Level.info.path + "/Editor/Objects.dat", false, false, (byte) 1);
        EditorObjects.snapTransform = block.readSingle();
        EditorObjects.snapRotation = block.readSingle();
      }
      else
      {
        EditorObjects.snapTransform = 1f;
        EditorObjects.snapRotation = 15f;
      }
    }

    public static void save()
    {
      Block block = new Block();
      block.writeByte(EditorObjects.SAVEDATA_VERSION);
      block.writeSingle(EditorObjects.snapTransform);
      block.writeSingle(EditorObjects.snapRotation);
      ReadWrite.writeBlock(Level.info.path + "/Editor/Objects.dat", false, false, block);
    }
  }
}
