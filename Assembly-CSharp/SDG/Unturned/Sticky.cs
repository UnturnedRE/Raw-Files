// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.Sticky
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class Sticky : MonoBehaviour
  {
    private bool isStuck;

    private void OnTriggerEnter(Collider other)
    {
      if (this.isStuck)
        return;
      Rigidbody component = this.GetComponent<Rigidbody>();
      component.useGravity = false;
      component.isKinematic = true;
      this.isStuck = true;
    }

    private void Awake()
    {
      BoxCollider component = this.GetComponent<BoxCollider>();
      component.isTrigger = true;
      component.size *= 2f;
    }
  }
}
