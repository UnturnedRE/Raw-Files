// Decompiled with JetBrains decompiler
// Type: Pathfinding.GraphHitInfo
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

namespace Pathfinding
{
  public struct GraphHitInfo
  {
    public Vector3 origin;
    public Vector3 point;
    public GraphNode node;
    public Vector3 tangentOrigin;
    public Vector3 tangent;

    public float distance
    {
      get
      {
        return (this.point - this.origin).magnitude;
      }
    }

    public GraphHitInfo(Vector3 point)
    {
      this.tangentOrigin = Vector3.zero;
      this.origin = Vector3.zero;
      this.point = point;
      this.node = (GraphNode) null;
      this.tangent = Vector3.zero;
    }
  }
}
