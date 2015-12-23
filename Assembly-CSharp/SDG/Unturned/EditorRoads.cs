// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.EditorRoads
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class EditorRoads : MonoBehaviour
  {
    private static bool _isPaving;
    public static byte selected;
    private static Road _road;
    private static Transform selection;
    private static Transform highlighter;

    public static bool isPaving
    {
      get
      {
        return EditorRoads._isPaving;
      }
      set
      {
        EditorRoads._isPaving = value;
        EditorRoads.highlighter.gameObject.SetActive(EditorRoads.isPaving);
        if (EditorRoads.isPaving)
          return;
        EditorRoads.select((Transform) null);
      }
    }

    public static Road road
    {
      get
      {
        return EditorRoads._road;
      }
    }

    private static void select(Transform select)
    {
      if ((Object) EditorRoads.selection != (Object) null)
        EditorRoads.selection.GetComponent<Renderer>().material.color = Color.white;
      if ((Object) EditorRoads.selection == (Object) select || (Object) select == (Object) null)
      {
        EditorRoads.selection = (Transform) null;
        EditorRoads._road = (Road) null;
      }
      else
      {
        EditorRoads.selection = select;
        EditorRoads._road = LevelRoads.getRoad(EditorRoads.selection);
        EditorRoads.selection.GetComponent<Renderer>().material.color = Color.red;
      }
    }

    private void Update()
    {
      if (!EditorRoads.isPaving || EditorInteract.isFlying || GUIUtility.hotControl != 0)
        return;
      if ((Object) EditorInteract.worldHit.transform != (Object) null)
        EditorRoads.highlighter.position = EditorInteract.worldHit.point;
      if ((Input.GetKeyDown(KeyCode.Delete) || Input.GetKeyDown(KeyCode.Backspace)) && ((Object) EditorRoads.selection != (Object) null && (Object) EditorRoads.selection != (Object) null))
      {
        if (Input.GetKey(ControlsSettings.other))
          LevelRoads.removeRoad(EditorRoads.selection);
        else
          EditorRoads.road.removePoint(EditorRoads.selection);
      }
      if (Input.GetKeyDown(ControlsSettings.tool_1) && (Object) EditorRoads.selection != (Object) null)
      {
        EditorRoads.road.splitPoint(EditorRoads.selection);
        EditorRoads.select((Transform) null);
      }
      if (Input.GetKeyDown(ControlsSettings.tool_2) && (Object) EditorInteract.worldHit.transform != (Object) null)
      {
        Vector3 point = EditorInteract.worldHit.point;
        if ((Object) EditorRoads.selection != (Object) null)
          EditorRoads.road.movePoint(EditorRoads.selection, point);
      }
      if (!Input.GetKeyDown(ControlsSettings.primary))
        return;
      if ((Object) EditorInteract.logicHit.transform != (Object) null)
      {
        if (EditorInteract.logicHit.transform.name.IndexOf("Path") == -1)
          return;
        EditorRoads.select(EditorInteract.logicHit.transform);
      }
      else
      {
        if (!((Object) EditorInteract.worldHit.transform != (Object) null))
          return;
        Vector3 point = EditorInteract.worldHit.point;
        if ((Object) EditorRoads.selection != (Object) null)
        {
          if (!((Object) EditorRoads.selection == (Object) EditorRoads.road.paths[0]) && !((Object) EditorRoads.selection == (Object) EditorRoads.road.paths[EditorRoads.road.paths.Count - 1]))
            return;
          EditorRoads.select(EditorRoads.road.addPoint(EditorRoads.selection, point));
        }
        else
          EditorRoads.select(LevelRoads.addRoad(point));
      }
    }

    private void Start()
    {
      EditorRoads._isPaving = false;
      EditorRoads.highlighter = ((GameObject) Object.Instantiate(Resources.Load("Edit/Highlighter"))).transform;
      EditorRoads.highlighter.name = "Highlighter";
      EditorRoads.highlighter.parent = Level.editing;
      EditorRoads.highlighter.gameObject.SetActive(false);
      EditorRoads.highlighter.GetComponent<Renderer>().material.color = Color.red;
    }
  }
}
