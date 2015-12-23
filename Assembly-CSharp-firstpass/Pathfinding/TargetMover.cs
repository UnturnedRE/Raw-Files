// Decompiled with JetBrains decompiler
// Type: Pathfinding.TargetMover
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

namespace Pathfinding
{
  public class TargetMover : MonoBehaviour
  {
    public LayerMask mask;
    public Transform target;
    private RichAI[] ais;
    private AIPath[] ais2;
    public bool onlyOnDoubleClick;
    private Camera cam;

    public void Start()
    {
      this.cam = Camera.main;
      this.ais = Object.FindObjectsOfType(typeof (RichAI)) as RichAI[];
      this.ais2 = Object.FindObjectsOfType(typeof (AIPath)) as AIPath[];
    }

    public void OnGUI()
    {
      if (!this.onlyOnDoubleClick || !((Object) this.cam != (Object) null) || (Event.current.type != UnityEngine.EventType.MouseDown || Event.current.clickCount != 2))
        return;
      this.UpdateTargetPosition();
    }

    private void Update()
    {
      if (this.onlyOnDoubleClick || !((Object) this.cam != (Object) null))
        return;
      this.UpdateTargetPosition();
    }

    public void UpdateTargetPosition()
    {
      RaycastHit hitInfo;
      if (!Physics.Raycast(this.cam.ScreenPointToRay(Input.mousePosition), out hitInfo, float.PositiveInfinity, (int) this.mask) || !(hitInfo.point != this.target.position))
        return;
      this.target.position = hitInfo.point;
      if (this.ais != null && this.onlyOnDoubleClick)
      {
        for (int index = 0; index < this.ais.Length; ++index)
        {
          if ((Object) this.ais[index] != (Object) null)
            this.ais[index].UpdatePath();
        }
      }
      if (this.ais2 == null || !this.onlyOnDoubleClick)
        return;
      for (int index = 0; index < this.ais2.Length; ++index)
      {
        if ((Object) this.ais2[index] != (Object) null)
          this.ais2[index].SearchPath();
      }
    }
  }
}
