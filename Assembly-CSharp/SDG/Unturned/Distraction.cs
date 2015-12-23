// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.Distraction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class Distraction : MonoBehaviour
  {
    public void Distract()
    {
      AlertTool.alert(this.transform.position, 24f);
      Object.Destroy((Object) this);
    }

    private void Start()
    {
      this.Invoke("Distract", 2.5f);
    }
  }
}
