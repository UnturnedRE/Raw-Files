// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.EditorNodes
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class EditorNodes : MonoBehaviour
  {
    private static bool _isNoding;
    private static ENodeType _nodeType;
    private static Node _node;
    private static Transform selection;
    private static Transform location;
    private static Transform purchase;
    private static Transform safezone;

    public static bool isNoding
    {
      get
      {
        return EditorNodes._isNoding;
      }
      set
      {
        EditorNodes._isNoding = value;
        EditorNodes.location.gameObject.SetActive(EditorNodes.isNoding && EditorNodes.nodeType == ENodeType.LOCATION);
        EditorNodes.safezone.gameObject.SetActive(EditorNodes.isNoding && EditorNodes.nodeType == ENodeType.SAFEZONE);
        EditorNodes.purchase.gameObject.SetActive(EditorNodes.isNoding && EditorNodes.nodeType == ENodeType.PURCHASE);
        if (EditorNodes.isNoding)
          return;
        EditorNodes.select((Transform) null);
      }
    }

    public static ENodeType nodeType
    {
      get
      {
        return EditorNodes._nodeType;
      }
      set
      {
        EditorNodes._nodeType = value;
        EditorNodes.location.gameObject.SetActive(EditorNodes.nodeType == ENodeType.LOCATION);
        EditorNodes.safezone.gameObject.SetActive(EditorNodes.nodeType == ENodeType.SAFEZONE);
        EditorNodes.purchase.gameObject.SetActive(EditorNodes.nodeType == ENodeType.PURCHASE);
      }
    }

    public static Node node
    {
      get
      {
        return EditorNodes._node;
      }
    }

    private static void select(Transform select)
    {
      if ((Object) EditorNodes.selection != (Object) null)
      {
        if (EditorNodes.node.type == ENodeType.SAFEZONE || EditorNodes.node.type == ENodeType.PURCHASE)
          EditorNodes.selection.GetComponent<Renderer>().material = (Material) Resources.Load("Materials/Good");
        else
          EditorNodes.selection.GetComponent<Renderer>().material.color = Color.white;
      }
      if ((Object) EditorNodes.selection == (Object) select || (Object) select == (Object) null)
      {
        EditorNodes.selection = (Transform) null;
        EditorNodes._node = (Node) null;
      }
      else
      {
        EditorNodes.selection = select;
        EditorNodes._node = LevelNodes.getNode(EditorNodes.selection);
        if (EditorNodes.node.type == ENodeType.SAFEZONE || EditorNodes.node.type == ENodeType.PURCHASE)
          EditorNodes.selection.GetComponent<Renderer>().material = (Material) Resources.Load("Materials/Bad");
        else
          EditorNodes.selection.GetComponent<Renderer>().material.color = Color.red;
      }
      EditorEnvironmentNodesUI.updateSelection(EditorNodes.node);
    }

    private void Update()
    {
      if (!EditorNodes.isNoding || EditorInteract.isFlying || GUIUtility.hotControl != 0)
        return;
      if ((Object) EditorInteract.worldHit.transform != (Object) null)
      {
        EditorNodes.location.position = EditorInteract.worldHit.point;
        EditorNodes.safezone.position = EditorInteract.worldHit.point;
        EditorNodes.purchase.position = EditorInteract.worldHit.point;
      }
      if ((Input.GetKeyDown(KeyCode.Delete) || Input.GetKeyDown(KeyCode.Backspace)) && (Object) EditorNodes.selection != (Object) null)
        LevelNodes.removeNode(EditorNodes.selection);
      if (Input.GetKeyDown(ControlsSettings.tool_2) && ((Object) EditorInteract.worldHit.transform != (Object) null && (Object) EditorNodes.selection != (Object) null))
        EditorNodes.node.move(EditorInteract.worldHit.point);
      if (!Input.GetKeyDown(ControlsSettings.primary))
        return;
      if ((Object) EditorInteract.logicHit.transform != (Object) null)
      {
        if (!(EditorInteract.logicHit.transform.name == "Node"))
          return;
        EditorNodes.select(EditorInteract.logicHit.transform);
      }
      else
      {
        if (!((Object) EditorInteract.worldHit.transform != (Object) null))
          return;
        Vector3 point = EditorInteract.worldHit.point;
        if (!Level.checkSafe(point))
          return;
        EditorNodes.select(LevelNodes.addNode(point, EditorNodes.nodeType));
      }
    }

    private void Start()
    {
      EditorNodes._isNoding = false;
      EditorNodes.location = ((GameObject) Object.Instantiate(Resources.Load("Edit/Location"))).transform;
      EditorNodes.location.name = "Location";
      EditorNodes.location.parent = Level.editing;
      EditorNodes.location.gameObject.SetActive(false);
      EditorNodes.location.GetComponent<Renderer>().material.color = Color.red;
      Object.Destroy((Object) EditorNodes.location.GetComponent<Collider>());
      EditorNodes.safezone = ((GameObject) Object.Instantiate(Resources.Load("Edit/Safezone"))).transform;
      EditorNodes.safezone.name = "Location";
      EditorNodes.safezone.parent = Level.editing;
      EditorNodes.safezone.gameObject.SetActive(false);
      EditorNodes.safezone.GetComponent<Renderer>().material = (Material) Resources.Load("Materials/Bad");
      Object.Destroy((Object) EditorNodes.safezone.GetComponent<Collider>());
      EditorNodes.purchase = ((GameObject) Object.Instantiate(Resources.Load("Edit/Purchase"))).transform;
      EditorNodes.purchase.name = "Location";
      EditorNodes.purchase.parent = Level.editing;
      EditorNodes.purchase.gameObject.SetActive(false);
      EditorNodes.purchase.GetComponent<Renderer>().material = (Material) Resources.Load("Materials/Bad");
      Object.Destroy((Object) EditorNodes.purchase.GetComponent<Collider>());
    }
  }
}
