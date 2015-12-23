// Decompiled with JetBrains decompiler
// Type: LocalSpaceRichAI
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding;
using UnityEngine;

public class LocalSpaceRichAI : RichAI
{
  public LocalSpaceGraph graph;

  public override void UpdatePath()
  {
    this.canSearchPath = true;
    this.waitingForPathCalc = false;
    Path currentPath = this.seeker.GetCurrentPath();
    if (currentPath != null && !this.seeker.IsDone())
    {
      currentPath.Error();
      currentPath.Claim((object) this);
      currentPath.Release((object) this);
    }
    this.waitingForPathCalc = true;
    this.lastRepath = Time.time;
    Matrix4x4 matrix = this.graph.GetMatrix();
    this.seeker.StartPath(matrix.MultiplyPoint3x4(this.tr.position), matrix.MultiplyPoint3x4(this.target.position));
  }

  protected override Vector3 UpdateTarget(RichFunnel fn)
  {
    Matrix4x4 matrix = this.graph.GetMatrix();
    Matrix4x4 inverse = matrix.inverse;
    Debug.DrawRay(matrix.MultiplyPoint3x4(this.tr.position), Vector3.up * 2f, Color.red);
    Debug.DrawRay(inverse.MultiplyPoint3x4(this.tr.position), Vector3.up * 2f, Color.green);
    this.buffer.Clear();
    Vector3 position = this.tr.position;
    bool requiresRepath;
    Vector3 start = inverse.MultiplyPoint3x4(fn.Update(matrix.MultiplyPoint3x4(position), this.buffer, 2, out this.lastCorner, out requiresRepath));
    Debug.DrawRay(start, Vector3.up * 3f, Color.black);
    for (int index = 0; index < this.buffer.Count; ++index)
    {
      this.buffer[index] = inverse.MultiplyPoint3x4(this.buffer[index]);
      Debug.DrawRay(this.buffer[index], Vector3.up * 3f, Color.yellow);
    }
    if (requiresRepath && !this.waitingForPathCalc)
      this.UpdatePath();
    return start;
  }
}
