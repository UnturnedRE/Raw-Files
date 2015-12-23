// Decompiled with JetBrains decompiler
// Type: Pathfinding.RVO.ObstacleVertex
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

namespace Pathfinding.RVO
{
  public class ObstacleVertex
  {
    public bool ignore;
    public Vector3 position;
    public Vector2 dir;
    public float height;
    public RVOLayer layer;
    public bool convex;
    public bool split;
    public ObstacleVertex next;
    public ObstacleVertex prev;
  }
}
