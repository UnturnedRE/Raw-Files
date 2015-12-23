// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.TemperatureTrigger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class TemperatureTrigger : MonoBehaviour
  {
    public EPlayerTemperature temperature;
    private TemperatureBubble bubble;

    private void Update()
    {
      if (this.bubble == null)
        return;
      this.bubble.origin = this.transform.position;
    }

    private void OnEnable()
    {
      if (this.bubble != null)
        return;
      this.bubble = TemperatureManager.registerBubble(this.transform.position, this.transform.localScale.x, this.temperature);
    }

    private void OnDisable()
    {
      if (this.bubble == null)
        return;
      TemperatureManager.deregisterBubble(this.bubble);
      this.bubble = (TemperatureBubble) null;
    }
  }
}
