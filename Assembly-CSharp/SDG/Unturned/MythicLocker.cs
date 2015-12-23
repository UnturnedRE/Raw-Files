// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.MythicLocker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class MythicLocker : MonoBehaviour
  {
    public Transform system;

    private void Update()
    {
      if ((Object) this.system == (Object) null)
        return;
      this.system.position = this.transform.position;
      this.system.rotation = this.transform.rotation;
    }

    private void LateUpdate()
    {
      if ((Object) this.system == (Object) null)
        return;
      this.system.position = this.transform.position;
      this.system.rotation = this.transform.rotation;
    }

    private void OnEnable()
    {
      if ((Object) this.system == (Object) null)
        return;
      this.system.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
      if ((Object) this.system == (Object) null)
        return;
      this.system.gameObject.SetActive(false);
    }

    private void Start()
    {
      if ((Object) this.system == (Object) null)
        return;
      this.system.parent = Level.effects;
    }

    private void OnDestroy()
    {
      if ((Object) this.system == (Object) null)
        return;
      Object.Destroy((Object) this.system.gameObject);
    }
  }
}
