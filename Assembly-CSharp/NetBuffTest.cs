// Decompiled with JetBrains decompiler
// Type: NetBuffTest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using SDG.Unturned;
using UnityEngine;

public class NetBuffTest : MonoBehaviour
{
  private NetworkSnapshotBuffer buff;
  private Vector3 vec;
  private float lasttick;
  private float delay;

  private void Update()
  {
    if (Input.GetKey(KeyCode.A) && (double) Time.realtimeSinceStartup - (double) this.lasttick > (double) this.delay)
    {
      this.buff.addNewSnapshot(this.vec += Vector3.forward, this.transform.rotation);
      Debug.Log((object) this.vec);
      this.lasttick = Time.realtimeSinceStartup;
      this.delay = (float) (0.150000005960464 + ((double) Random.value - 0.5) * 0.025000000372529);
    }
    Vector3 pos;
    Quaternion rot;
    this.buff.getCurrentSnapshot(out pos, out rot);
    this.transform.position = pos;
    this.transform.rotation = rot;
  }

  private void Start()
  {
    this.delay = 0.15f;
    this.buff = new NetworkSnapshotBuffer(this.delay, 1f);
    this.vec = this.transform.position;
  }
}
