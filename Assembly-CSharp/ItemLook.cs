// Decompiled with JetBrains decompiler
// Type: ItemLook
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class ItemLook : MonoBehaviour
{
  public Camera camera;
  public float _yaw;
  public float yaw;
  public Vector3 pos;

  private void Update()
  {
    this._yaw = Mathf.Lerp(this._yaw, this.yaw, 4f * Time.deltaTime);
    this.camera.transform.rotation = Quaternion.Euler(20f, this._yaw, 0.0f);
    this.camera.transform.position = this.pos - this.camera.transform.forward * 2.25f;
  }
}
