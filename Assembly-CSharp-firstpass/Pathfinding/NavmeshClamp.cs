// Decompiled with JetBrains decompiler
// Type: Pathfinding.NavmeshClamp
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

namespace Pathfinding
{
  public class NavmeshClamp : MonoBehaviour
  {
    private GraphNode prevNode;
    private Vector3 prevPos;

    private void LateUpdate()
    {
      if (this.prevNode == null)
      {
        this.prevNode = AstarPath.active.GetNearest(this.transform.position).node;
        this.prevPos = this.transform.position;
      }
      if (this.prevNode == null)
        return;
      if (this.prevNode != null)
      {
        IRaycastableGraph raycastableGraph = AstarData.GetGraph(this.prevNode) as IRaycastableGraph;
        if (raycastableGraph != null)
        {
          GraphHitInfo hit;
          if (raycastableGraph.Linecast(this.prevPos, this.transform.position, this.prevNode, out hit))
          {
            hit.point.y = this.transform.position.y;
            Vector3 end = AstarMath.NearestPoint(hit.tangentOrigin, hit.tangentOrigin + hit.tangent, this.transform.position);
            Vector3 vector3 = hit.point;
            Vector3 start = vector3 + Vector3.ClampMagnitude((Vector3) hit.node.position - vector3, 0.008f);
            if (raycastableGraph.Linecast(start, end, hit.node, out hit))
            {
              hit.point.y = this.transform.position.y;
              this.transform.position = hit.point;
            }
            else
            {
              end.y = this.transform.position.y;
              this.transform.position = end;
            }
          }
          this.prevNode = hit.node;
        }
      }
      this.prevPos = this.transform.position;
    }
  }
}
