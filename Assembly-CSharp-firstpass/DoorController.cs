// Decompiled with JetBrains decompiler
// Type: DoorController
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding;
using UnityEngine;

public class DoorController : MonoBehaviour
{
  public int opentag = 1;
  public int closedtag = 1;
  public bool updateGraphsWithGUO = true;
  public float yOffset = 5f;
  private bool open;
  private Bounds bounds;

  public void Start()
  {
    this.bounds = this.GetComponent<Collider>().bounds;
    this.SetState(this.open);
  }

  private void OnGUI()
  {
    if (!GUI.Button(new Rect(5f, this.yOffset, 100f, 22f), "Toggle Door"))
      return;
    this.SetState(!this.open);
  }

  public void SetState(bool open)
  {
    this.open = open;
    if (this.updateGraphsWithGUO)
    {
      GraphUpdateObject ob = new GraphUpdateObject(this.bounds);
      int num = !open ? this.closedtag : this.opentag;
      if (num > 31)
      {
        Debug.LogError((object) "tag > 31");
        return;
      }
      ob.modifyTag = true;
      ob.setTag = num;
      ob.updatePhysics = false;
      AstarPath.active.UpdateGraphs(ob);
    }
    if (open)
      this.GetComponent<Animation>().Play("Open");
    else
      this.GetComponent<Animation>().Play("Close");
  }

  private void Update()
  {
  }
}
