// Decompiled with JetBrains decompiler
// Type: AstarSmoothFollow2
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

public class AstarSmoothFollow2 : MonoBehaviour
{
  public float distance = 3f;
  public float height = 3f;
  public float damping = 5f;
  public bool smoothRotation = true;
  public bool followBehind = true;
  public float rotationDamping = 10f;
  public Transform target;
  public bool staticOffset;

  private void LateUpdate()
  {
    this.transform.position = Vector3.Lerp(this.transform.position, !this.staticOffset ? (!this.followBehind ? this.target.TransformPoint(0.0f, this.height, this.distance) : this.target.TransformPoint(0.0f, this.height, -this.distance)) : this.target.position + new Vector3(0.0f, this.height, this.distance), Time.deltaTime * this.damping);
    if (this.smoothRotation)
      this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(this.target.position - this.transform.position, this.target.up), Time.deltaTime * this.rotationDamping);
    else
      this.transform.LookAt(this.target, this.target.up);
  }
}
