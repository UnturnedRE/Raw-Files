// Decompiled with JetBrains decompiler
// Type: BezierMover
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding;
using UnityEngine;

public class BezierMover : MonoBehaviour
{
  public float tangentLengths = 5f;
  public float speed = 1f;
  public Transform[] points;
  private float time;

  private void Update()
  {
    this.Move(true);
  }

  private Vector3 Plot(float t)
  {
    int length = this.points.Length;
    int num = Mathf.FloorToInt(t);
    Vector3 normalized1 = ((this.points[(num + 1) % length].position - this.points[num % length].position).normalized - (this.points[(num - 1 + length) % length].position - this.points[num % length].position).normalized).normalized;
    Vector3 normalized2 = ((this.points[(num + 2) % length].position - this.points[(num + 1) % length].position).normalized - (this.points[(num + length) % length].position - this.points[(num + 1) % length].position).normalized).normalized;
    Debug.DrawLine(this.points[num % length].position, this.points[num % length].position + normalized1 * this.tangentLengths, Color.red);
    Debug.DrawLine(this.points[(num + 1) % length].position - normalized2 * this.tangentLengths, this.points[(num + 1) % length].position, Color.green);
    return AstarMath.CubicBezier(this.points[num % length].position, this.points[num % length].position + normalized1 * this.tangentLengths, this.points[(num + 1) % length].position - normalized2 * this.tangentLengths, this.points[(num + 1) % length].position, t - (float) num);
  }

  private void Move(bool progress)
  {
    // ISSUE: unable to decompile the method.
  }

  public void OnDrawGizmos()
  {
    if (this.points.Length < 3)
      return;
    for (int index = 0; index < this.points.Length; ++index)
    {
      if ((Object) this.points[index] == (Object) null)
        return;
    }
    for (int index1 = 0; index1 < this.points.Length; ++index1)
    {
      int length = this.points.Length;
      Vector3 normalized1 = ((this.points[(index1 + 1) % length].position - this.points[index1].position).normalized - (this.points[(index1 - 1 + length) % length].position - this.points[index1].position).normalized).normalized;
      Vector3 normalized2 = ((this.points[(index1 + 2) % length].position - this.points[(index1 + 1) % length].position).normalized - (this.points[(index1 + length) % length].position - this.points[(index1 + 1) % length].position).normalized).normalized;
      Vector3 from = this.points[index1].position;
      for (int index2 = 1; index2 <= 100; ++index2)
      {
        Vector3 to = AstarMath.CubicBezier(this.points[index1].position, this.points[index1].position + normalized1 * this.tangentLengths, this.points[(index1 + 1) % length].position - normalized2 * this.tangentLengths, this.points[(index1 + 1) % length].position, (float) index2 / 100f);
        Gizmos.DrawLine(from, to);
        from = to;
      }
    }
  }
}
