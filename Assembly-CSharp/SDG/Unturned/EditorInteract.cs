// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.EditorInteract
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class EditorInteract : MonoBehaviour
  {
    public static readonly byte SAVEDATA_VERSION = (byte) 1;
    private static bool _isFlying;
    private static Ray _ray;
    private static RaycastHit _groundHit;
    private static RaycastHit _worldHit;
    private static RaycastHit _objectHit;
    private static RaycastHit _logicHit;

    public static bool isFlying
    {
      get
      {
        return EditorInteract._isFlying;
      }
    }

    public static Ray ray
    {
      get
      {
        return EditorInteract._ray;
      }
    }

    public static RaycastHit groundHit
    {
      get
      {
        return EditorInteract._groundHit;
      }
    }

    public static RaycastHit worldHit
    {
      get
      {
        return EditorInteract._worldHit;
      }
    }

    public static RaycastHit objectHit
    {
      get
      {
        return EditorInteract._objectHit;
      }
    }

    public static RaycastHit logicHit
    {
      get
      {
        return EditorInteract._logicHit;
      }
    }

    private void FixedUpdate()
    {
      EditorInteract._ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      Physics.Raycast(EditorInteract.ray, out EditorInteract._groundHit, 10000f, EditorTerrainHeight.isTerraforming && EditorTerrainHeight.map2 || EditorTerrainMaterials.isPainting && EditorTerrainMaterials.map2 ? RayMasks.GROUND2 : RayMasks.GROUND);
      Physics.Raycast(EditorInteract.ray, out EditorInteract._worldHit, 10000f, RayMasks.EDITOR_WORLD);
      Physics.Raycast(EditorInteract.ray, out EditorInteract._objectHit, 10000f, RayMasks.EDITOR_INTERACT);
      Physics.Raycast(EditorInteract.ray, out EditorInteract._logicHit, 10000f, RayMasks.LOGIC);
    }

    private void Update()
    {
      EditorInteract._isFlying = Input.GetKey(ControlsSettings.secondary);
      if (Input.GetKeyDown(KeyCode.S) && Input.GetKey(KeyCode.LeftControl))
        Level.save();
      if (Input.GetKeyDown(KeyCode.F1))
      {
        LevelVisibility.roadsVisible = !LevelVisibility.roadsVisible;
        EditorLevelVisibilityUI.roadsToggle.state = LevelVisibility.roadsVisible;
      }
      if (Input.GetKeyDown(KeyCode.F2))
      {
        LevelVisibility.navigationVisible = !LevelVisibility.navigationVisible;
        EditorLevelVisibilityUI.navigationToggle.state = LevelVisibility.navigationVisible;
      }
      if (Input.GetKeyDown(KeyCode.F3))
      {
        LevelVisibility.nodesVisible = !LevelVisibility.nodesVisible;
        EditorLevelVisibilityUI.nodesToggle.state = LevelVisibility.nodesVisible;
      }
      if (Input.GetKeyDown(KeyCode.F4))
      {
        LevelVisibility.itemsVisible = !LevelVisibility.itemsVisible;
        EditorLevelVisibilityUI.itemsToggle.state = LevelVisibility.itemsVisible;
      }
      if (Input.GetKeyDown(KeyCode.F5))
      {
        LevelVisibility.playersVisible = !LevelVisibility.playersVisible;
        EditorLevelVisibilityUI.playersToggle.state = LevelVisibility.playersVisible;
      }
      if (Input.GetKeyDown(KeyCode.F6))
      {
        LevelVisibility.zombiesVisible = !LevelVisibility.zombiesVisible;
        EditorLevelVisibilityUI.zombiesToggle.state = LevelVisibility.zombiesVisible;
      }
      if (Input.GetKeyDown(KeyCode.F7))
      {
        LevelVisibility.vehiclesVisible = !LevelVisibility.vehiclesVisible;
        EditorLevelVisibilityUI.vehiclesToggle.state = LevelVisibility.vehiclesVisible;
      }
      if (Input.GetKeyDown(KeyCode.F8))
      {
        LevelVisibility.borderVisible = !LevelVisibility.borderVisible;
        EditorLevelVisibilityUI.borderToggle.state = LevelVisibility.borderVisible;
      }
      if (!Input.GetKeyDown(KeyCode.F9))
        return;
      LevelVisibility.animalsVisible = !LevelVisibility.animalsVisible;
      EditorLevelVisibilityUI.animalsToggle.state = LevelVisibility.animalsVisible;
    }

    private void Start()
    {
      EditorInteract.load();
    }

    public static void load()
    {
      if (ReadWrite.fileExists(Level.info.path + "/Editor/Camera.dat", false, false))
      {
        Block block = ReadWrite.readBlock(Level.info.path + "/Editor/Camera.dat", false, false, (byte) 1);
        Camera.main.transform.parent.position = block.readSingleVector3();
        Camera.main.transform.localRotation = Quaternion.Euler(block.readSingle(), 0.0f, 0.0f);
        Camera.main.transform.parent.rotation = Quaternion.Euler(0.0f, block.readSingle(), 0.0f);
      }
      else
      {
        Camera.main.transform.parent.position = new Vector3(0.0f, Level.TERRAIN, 0.0f);
        Camera.main.transform.parent.rotation = Quaternion.identity;
        Camera.main.transform.localRotation = Quaternion.identity;
      }
    }

    public static void save()
    {
      Block block = new Block();
      block.writeByte(EditorInteract.SAVEDATA_VERSION);
      block.writeSingleVector3(Camera.main.transform.position);
      block.writeSingle(EditorLook.pitch);
      block.writeSingle(EditorLook.yaw);
      ReadWrite.writeBlock(Level.info.path + "/Editor/Camera.dat", false, false, block);
    }
  }
}
