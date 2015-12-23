// Decompiled with JetBrains decompiler
// Type: Pathfinding.RVO.RVOSquareObstacle
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

namespace Pathfinding.RVO
{
  [AddComponentMenu("Pathfinding/Local Avoidance/Square Obstacle (disabled)")]
  public class RVOSquareObstacle : RVOObstacle
  {
    public float height = 1f;
    public Vector2 size = (Vector2) Vector3.one;

    protected override bool StaticObstacle
    {
      get
      {
        return false;
      }
    }

    protected override bool ExecuteInEditor
    {
      get
      {
        return true;
      }
    }

    protected override bool LocalCoordinates
    {
      get
      {
        return true;
      }
    }

    protected override bool AreGizmosDirty()
    {
      return false;
    }

    protected override void CreateObstacles()
    {
      this.size.x = Mathf.Abs(this.size.x);
      this.size.y = Mathf.Abs(this.size.y);
      this.height = Mathf.Abs(this.height);
      Vector3[] vertices = new Vector3[4]
      {
        new Vector3(1f, 0.0f, -1f),
        new Vector3(1f, 0.0f, 1f),
        new Vector3(-1f, 0.0f, 1f),
        new Vector3(-1f, 0.0f, -1f)
      };
      for (int index = 0; index < vertices.Length; ++index)
        vertices[index].Scale(new Vector3(this.size.x, 0.0f, this.size.y));
      this.AddObstacle(vertices, this.height);
    }
  }
}
