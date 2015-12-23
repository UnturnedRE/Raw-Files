// Decompiled with JetBrains decompiler
// Type: DeterminismTester
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class DeterminismTester : MonoBehaviour
{
  public CharacterController cc;
  public BoxCollider bc;
  private Vector3 start;
  private Vector3 end;
  public float angle;

  private void Update()
  {
    this.transform.position = this.start;
    this.start = this.transform.position;
    Vector3 motion = new Vector3(Mathf.Cos(this.angle), 0.0f, Mathf.Sin(this.angle));
    RaycastHit hitInfo1;
    Physics.Raycast(new Ray(this.cc.transform.position + new Vector3(2f, 1f, 0.0f), new Vector3(-1f, 0.0f, 0.0f)), out hitInfo1, 16f);
    Debug.Log((object) ((string) (object) hitInfo1.collider + (object) " " + (string) (object) hitInfo1.point.magnitude));
    int num1 = (int) this.cc.Move(motion);
    RaycastHit hitInfo2;
    Physics.Raycast(new Ray(this.cc.transform.position + new Vector3(2f, 1f, 0.0f), new Vector3(-1f, 0.0f, 0.0f)), out hitInfo2, 16f);
    Debug.Log((object) ((string) (object) hitInfo2.collider + (object) " " + (string) (object) hitInfo2.point.magnitude));
    int num2 = (int) this.cc.Move(motion);
    RaycastHit hitInfo3;
    Physics.Raycast(new Ray(this.cc.transform.position + new Vector3(2f, 1f, 0.0f), new Vector3(-1f, 0.0f, 0.0f)), out hitInfo3, 16f);
    Debug.Log((object) ((string) (object) hitInfo3.collider + (object) " " + (string) (object) hitInfo3.point.magnitude));
    Vector3 position1 = this.transform.position;
    this.transform.position = new Vector3(500f, 250f, 250f);
    RaycastHit hitInfo4;
    Physics.Raycast(new Ray(this.cc.transform.position + new Vector3(2f, 1f, 0.0f), new Vector3(-1f, 0.0f, 0.0f)), out hitInfo4, 16f);
    Debug.Log((object) ((string) (object) hitInfo4.collider + (object) " " + (string) (object) hitInfo4.point.magnitude));
    this.transform.position = position1;
    Vector3 position2 = this.bc.transform.position;
    this.bc.transform.position += new Vector3(0.1f, 0.0f, 0.0f);
    RaycastHit hitInfo5;
    Physics.Raycast(new Ray(this.cc.transform.position + new Vector3(2f, 1f, 0.0f), new Vector3(-1f, 0.0f, 0.0f)), out hitInfo5, 16f);
    Debug.Log((object) ((string) (object) hitInfo5.collider + (object) " " + (string) (object) hitInfo5.point.magnitude));
    this.bc.transform.position = position2;
    Debug.Log((object) (bool) (this.transform.position == this.end ? 1 : 0));
    this.end = this.transform.position;
  }
}
