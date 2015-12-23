// Decompiled with JetBrains decompiler
// Type: ObjectPlacer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
  public bool issueGUOs = true;
  public GameObject go;
  public bool direct;

  private void Start()
  {
  }

  private void Update()
  {
    if (Input.GetKeyDown("p"))
      this.PlaceObject();
    if (!Input.GetKeyDown("r"))
      return;
    this.RemoveObject();
  }

  public void PlaceObject()
  {
    RaycastHit hitInfo;
    if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, float.PositiveInfinity))
      return;
    GameObject gameObject = (GameObject) Object.Instantiate((Object) this.go, hitInfo.point, Quaternion.identity);
    if (!this.issueGUOs)
      return;
    GraphUpdateObject ob = new GraphUpdateObject(gameObject.GetComponent<Collider>().bounds);
    AstarPath.active.UpdateGraphs(ob);
    if (!this.direct)
      return;
    AstarPath.active.FlushGraphUpdates();
  }

  public void RemoveObject()
  {
    RaycastHit hitInfo;
    if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, float.PositiveInfinity) || hitInfo.collider.isTrigger || hitInfo.transform.gameObject.name == "Ground")
      return;
    Bounds bounds = hitInfo.collider.bounds;
    Object.Destroy((Object) hitInfo.collider);
    Object.Destroy((Object) hitInfo.collider.gameObject);
    if (!this.issueGUOs)
      return;
    GraphUpdateObject ob = new GraphUpdateObject(bounds);
    AstarPath.active.UpdateGraphs(ob, 0.0f);
    if (!this.direct)
      return;
    AstarPath.active.FlushGraphUpdates();
  }
}
