// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.EditorTerrainMaterials
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class EditorTerrainMaterials : MonoBehaviour
  {
    public static readonly byte SAVEDATA_VERSION = (byte) 1;
    public static readonly byte MIN_BRUSH_SIZE = (byte) 1;
    public static readonly byte MAX_BRUSH_SIZE = (byte) 254;
    private static bool _isPainting;
    public static byte selected;
    private static Transform brush;
    private static byte _brushSize;
    public static bool map2;

    public static bool isPainting
    {
      get
      {
        return EditorTerrainMaterials._isPainting;
      }
      set
      {
        EditorTerrainMaterials._isPainting = value;
        LevelGround.updateVisibility(!EditorTerrainMaterials.isPainting);
        EditorTerrainMaterials.brush.gameObject.SetActive(EditorTerrainMaterials.isPainting);
      }
    }

    public static byte brushSize
    {
      get
      {
        return EditorTerrainMaterials._brushSize;
      }
      set
      {
        EditorTerrainMaterials._brushSize = value;
        if (!((Object) EditorTerrainMaterials.brush != (Object) null))
          return;
        EditorTerrainMaterials.brush.localScale = new Vector3((float) EditorTerrainMaterials.brushSize * 2f, (float) EditorTerrainMaterials.brushSize * 2f, (float) EditorTerrainMaterials.brushSize * 2f);
      }
    }

    private void Update()
    {
      if (!EditorTerrainMaterials.isPainting || EditorInteract.isFlying || GUIUtility.hotControl != 0)
        return;
      if (Input.GetKeyDown(KeyCode.Z) && !EditorTerrainMaterials.map2 && Input.GetKey(KeyCode.LeftControl))
        LevelGround.undoMaterial();
      if (Input.GetKeyDown(KeyCode.X) && !EditorTerrainMaterials.map2 && Input.GetKey(KeyCode.LeftControl))
        LevelGround.redoMaterial();
      if ((Object) EditorInteract.groundHit.transform != (Object) null)
        EditorTerrainMaterials.brush.position = EditorInteract.groundHit.point;
      if (Input.GetKey(ControlsSettings.primary) && (Object) EditorInteract.groundHit.transform != (Object) null)
        LevelGround.paint(EditorInteract.groundHit.point, (int) EditorTerrainMaterials.brushSize, (int) EditorTerrainMaterials.selected, EditorTerrainMaterials.map2);
      if (!Input.GetKeyUp(ControlsSettings.primary))
        return;
      LevelGround.registerMaterial();
    }

    private void Start()
    {
      EditorTerrainMaterials._isPainting = false;
      EditorTerrainMaterials.brush = ((GameObject) Object.Instantiate(Resources.Load("Edit/Paint"))).transform;
      EditorTerrainMaterials.brush.name = "Paint";
      EditorTerrainMaterials.brush.parent = Level.editing;
      EditorTerrainMaterials.brush.gameObject.SetActive(false);
      EditorTerrainMaterials.load();
    }

    public static void load()
    {
      if (ReadWrite.fileExists(Level.info.path + "/Editor/Materials.dat", false, false))
        EditorTerrainMaterials.brushSize = ReadWrite.readBlock(Level.info.path + "/Editor/Materials.dat", false, false, (byte) 1).readByte();
      else
        EditorTerrainMaterials.brushSize = EditorTerrainMaterials.MIN_BRUSH_SIZE;
    }

    public static void save()
    {
      Block block = new Block();
      block.writeByte(EditorTerrainMaterials.SAVEDATA_VERSION);
      block.writeByte(EditorTerrainMaterials.brushSize);
      ReadWrite.writeBlock(Level.info.path + "/Editor/Materials.dat", false, false, block);
    }
  }
}
