// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.EditorNavigation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class EditorNavigation : MonoBehaviour
  {
    private static bool _isPathfinding;
    private static Flag _flag;
    private static Transform selection;
    private static Transform marker;

    public static bool isPathfinding
    {
      get
      {
        return EditorNavigation._isPathfinding;
      }
      set
      {
        EditorNavigation._isPathfinding = value;
        EditorNavigation.marker.gameObject.SetActive(EditorNavigation.isPathfinding);
        if (EditorNavigation.isPathfinding)
          return;
        EditorNavigation.select((Transform) null);
      }
    }

    public static Flag flag
    {
      get
      {
        return EditorNavigation._flag;
      }
    }

    private static void select(Transform select)
    {
      if ((Object) EditorNavigation.selection != (Object) null)
        EditorNavigation.selection.GetComponent<Renderer>().material.color = Color.white;
      if ((Object) EditorNavigation.selection == (Object) select || (Object) select == (Object) null)
      {
        EditorNavigation.selection = (Transform) null;
        EditorNavigation._flag = (Flag) null;
      }
      else
      {
        EditorNavigation.selection = select;
        EditorNavigation._flag = LevelNavigation.getFlag(EditorNavigation.selection);
        EditorNavigation.selection.GetComponent<Renderer>().material.color = Color.red;
      }
      EditorEnvironmentNavigationUI.updateSelection(EditorNavigation.flag);
    }

    private void Update()
    {
      if (!EditorNavigation.isPathfinding || EditorInteract.isFlying || GUIUtility.hotControl != 0)
        return;
      if ((Object) EditorInteract.worldHit.transform != (Object) null)
        EditorNavigation.marker.position = EditorInteract.worldHit.point;
      if ((Input.GetKeyDown(KeyCode.Delete) || Input.GetKeyDown(KeyCode.Backspace)) && (Object) EditorNavigation.selection != (Object) null)
        LevelNavigation.removeFlag(EditorNavigation.selection);
      if (Input.GetKeyDown(ControlsSettings.tool_2) && ((Object) EditorInteract.worldHit.transform != (Object) null && (Object) EditorNavigation.selection != (Object) null))
        EditorNavigation.flag.move(EditorInteract.worldHit.point);
      if (!Input.GetKeyDown(ControlsSettings.primary))
        return;
      if ((Object) EditorInteract.logicHit.transform != (Object) null)
      {
        if (!(EditorInteract.logicHit.transform.name == "Flag"))
          return;
        EditorNavigation.select(EditorInteract.logicHit.transform);
      }
      else
      {
        if (!((Object) EditorInteract.worldHit.transform != (Object) null))
          return;
        Vector3 point = EditorInteract.worldHit.point;
        if (!Level.checkSafe(point))
          return;
        EditorNavigation.select(LevelNavigation.addFlag(point));
      }
    }

    private void Start()
    {
      EditorNavigation._isPathfinding = false;
      EditorNavigation.marker = ((GameObject) Object.Instantiate(Resources.Load("Edit/Marker"))).transform;
      EditorNavigation.marker.name = "Marker";
      EditorNavigation.marker.parent = Level.editing;
      EditorNavigation.marker.gameObject.SetActive(false);
      EditorNavigation.marker.GetComponent<Renderer>().material.color = Color.red;
      Object.Destroy((Object) EditorNavigation.marker.GetComponent<Collider>());
    }
  }
}
