// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.EditorTerrainHeight
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class EditorTerrainHeight : MonoBehaviour
  {
    public static readonly byte SAVEDATA_VERSION = (byte) 1;
    public static readonly byte MIN_BRUSH_SIZE = (byte) 2;
    public static readonly byte MAX_BRUSH_SIZE = (byte) 253;
    private static bool _isTerraforming;
    private static Transform brush;
    private static Transform adjustUpBrush;
    private static Transform adjustDownBrush;
    private static Transform smoothBrush;
    private static byte _brushSize;
    public static float brushStrength;
    private static float _brushHeight;
    private static EPaintMode _brushMode;
    public static bool map2;

    public static bool isTerraforming
    {
      get
      {
        return EditorTerrainHeight._isTerraforming;
      }
      set
      {
        EditorTerrainHeight._isTerraforming = value;
        LevelGround.updateVisibility(!EditorTerrainHeight.isTerraforming);
        EditorTerrainHeight.brush.gameObject.SetActive(EditorTerrainHeight.isTerraforming);
      }
    }

    public static byte brushSize
    {
      get
      {
        return EditorTerrainHeight._brushSize;
      }
      set
      {
        EditorTerrainHeight._brushSize = value;
        if (!((Object) EditorTerrainHeight.brush != (Object) null))
          return;
        EditorTerrainHeight.brush.localScale = new Vector3((float) EditorTerrainHeight.brushSize * 2f, (float) EditorTerrainHeight.brushSize * 2f, (float) EditorTerrainHeight.brushSize * 2f);
      }
    }

    public static float brushHeight
    {
      get
      {
        return EditorTerrainHeight._brushHeight;
      }
      set
      {
        EditorTerrainHeight._brushHeight = value;
        if (EditorTerrainHeight.brushMode != EPaintMode.FLATTEN || !((Object) EditorTerrainHeight.brush != (Object) null))
          return;
        EditorTerrainHeight.brush.position = new Vector3(EditorTerrainHeight.brush.position.x, EditorTerrainHeight.brushHeight * Level.TERRAIN, EditorTerrainHeight.brush.position.z);
      }
    }

    public static EPaintMode brushMode
    {
      get
      {
        return EditorTerrainHeight._brushMode;
      }
      set
      {
        EditorTerrainHeight._brushMode = value;
        if (EditorTerrainHeight.brushMode == EPaintMode.ADJUST_UP)
        {
          EditorTerrainHeight.adjustUpBrush.gameObject.SetActive(true);
          EditorTerrainHeight.adjustDownBrush.gameObject.SetActive(false);
          EditorTerrainHeight.smoothBrush.gameObject.SetActive(false);
        }
        else if (EditorTerrainHeight.brushMode == EPaintMode.ADJUST_DOWN)
        {
          EditorTerrainHeight.adjustUpBrush.gameObject.SetActive(false);
          EditorTerrainHeight.adjustDownBrush.gameObject.SetActive(true);
          EditorTerrainHeight.smoothBrush.gameObject.SetActive(false);
        }
        else if (EditorTerrainHeight.brushMode == EPaintMode.SMOOTH)
        {
          EditorTerrainHeight.adjustUpBrush.gameObject.SetActive(false);
          EditorTerrainHeight.adjustDownBrush.gameObject.SetActive(false);
          EditorTerrainHeight.smoothBrush.gameObject.SetActive(true);
        }
        else
        {
          if (EditorTerrainHeight.brushMode != EPaintMode.FLATTEN)
            return;
          EditorTerrainHeight.adjustUpBrush.gameObject.SetActive(false);
          EditorTerrainHeight.adjustDownBrush.gameObject.SetActive(false);
          EditorTerrainHeight.smoothBrush.gameObject.SetActive(false);
        }
      }
    }

    private void Update()
    {
      if (!EditorTerrainHeight.isTerraforming || EditorInteract.isFlying || GUIUtility.hotControl != 0)
        return;
      if (Input.GetKeyDown(ControlsSettings.tool_0))
        EditorTerrainHeight.brushMode = EditorTerrainHeight.brushMode != EPaintMode.ADJUST_UP ? EPaintMode.ADJUST_UP : EPaintMode.ADJUST_DOWN;
      if (Input.GetKeyDown(ControlsSettings.tool_1))
        EditorTerrainHeight.brushMode = EPaintMode.SMOOTH;
      if (Input.GetKeyDown(ControlsSettings.tool_2))
        EditorTerrainHeight.brushMode = EPaintMode.FLATTEN;
      if (Input.GetKeyDown(KeyCode.Z) && !EditorTerrainHeight.map2 && Input.GetKey(KeyCode.LeftControl))
        LevelGround.undoHeight();
      if (Input.GetKeyDown(KeyCode.X) && !EditorTerrainHeight.map2 && Input.GetKey(KeyCode.LeftControl))
        LevelGround.redoHeight();
      if ((Object) EditorInteract.groundHit.transform != (Object) null)
        EditorTerrainHeight.brush.position = EditorTerrainHeight.brushMode != EPaintMode.FLATTEN ? EditorInteract.groundHit.point : new Vector3(EditorInteract.groundHit.point.x, EditorTerrainHeight.brushHeight * Level.TERRAIN, EditorInteract.groundHit.point.z);
      if (Input.GetKey(ControlsSettings.primary) && (Object) EditorInteract.groundHit.transform != (Object) null)
      {
        if (EditorTerrainHeight.brushMode == EPaintMode.ADJUST_UP)
          LevelGround.adjust(EditorInteract.groundHit.point, (int) EditorTerrainHeight.brushSize, EditorTerrainHeight.brushStrength, EditorTerrainHeight.map2);
        else if (EditorTerrainHeight.brushMode == EPaintMode.ADJUST_DOWN)
          LevelGround.adjust(EditorInteract.groundHit.point, (int) EditorTerrainHeight.brushSize, -EditorTerrainHeight.brushStrength, EditorTerrainHeight.map2);
        else if (EditorTerrainHeight.brushMode == EPaintMode.SMOOTH)
          LevelGround.smooth(EditorInteract.groundHit.point, (int) EditorTerrainHeight.brushSize, EditorTerrainHeight.brushStrength, EditorTerrainHeight.map2);
        else if (EditorTerrainHeight.brushMode == EPaintMode.FLATTEN)
          LevelGround.flatten(EditorInteract.groundHit.point, (int) EditorTerrainHeight.brushSize, EditorTerrainHeight.brushHeight, EditorTerrainHeight.brushStrength, EditorTerrainHeight.map2);
      }
      if (Input.GetKeyUp(ControlsSettings.primary))
        LevelGround.registerHeight();
      if (!Input.GetKeyDown(ControlsSettings.tool_2) || !((Object) EditorInteract.groundHit.transform != (Object) null))
        return;
      EditorTerrainHeight.brushHeight = EditorInteract.groundHit.point.y / Level.TERRAIN;
      if ((double) EditorTerrainHeight.brushHeight < 0.0)
        EditorTerrainHeight.brushHeight = 0.0f;
      else if ((double) EditorTerrainHeight.brushHeight > 1.0)
        EditorTerrainHeight.brushHeight = 1f;
      EditorTerrainHeightUI.heightValue.state = EditorTerrainHeight.brushHeight;
    }

    private void Start()
    {
      EditorTerrainHeight._isTerraforming = false;
      EditorTerrainHeight.brush = ((GameObject) Object.Instantiate(Resources.Load("Edit/Brush"))).transform;
      EditorTerrainHeight.brush.name = "Brush";
      EditorTerrainHeight.brush.parent = Level.editing;
      EditorTerrainHeight.brush.gameObject.SetActive(false);
      EditorTerrainHeight.adjustUpBrush = EditorTerrainHeight.brush.FindChild("Adjust_Up");
      EditorTerrainHeight.adjustDownBrush = EditorTerrainHeight.brush.FindChild("Adjust_Down");
      EditorTerrainHeight.smoothBrush = EditorTerrainHeight.brush.FindChild("Smooth");
      EditorTerrainHeight.brushMode = EPaintMode.ADJUST_UP;
      EditorTerrainHeight.load();
    }

    public static void load()
    {
      if (ReadWrite.fileExists(Level.info.path + "/Editor/Height.dat", false, false))
      {
        Block block = ReadWrite.readBlock(Level.info.path + "/Editor/Height.dat", false, false, (byte) 1);
        EditorTerrainHeight.brushSize = block.readByte();
        EditorTerrainHeight.brushStrength = block.readSingle();
        EditorTerrainHeight.brushHeight = block.readSingle();
      }
      else
      {
        EditorTerrainHeight.brushSize = EditorTerrainHeight.MIN_BRUSH_SIZE;
        EditorTerrainHeight.brushStrength = 1f;
        EditorTerrainHeight.brushHeight = 0.0f;
      }
    }

    public static void save()
    {
      Block block = new Block();
      block.writeByte(EditorTerrainHeight.SAVEDATA_VERSION);
      block.writeByte(EditorTerrainHeight.brushSize);
      block.writeSingle(EditorTerrainHeight.brushStrength);
      block.writeSingle(EditorTerrainHeight.brushHeight);
      ReadWrite.writeBlock(Level.info.path + "/Editor/Height.dat", false, false, block);
    }
  }
}
