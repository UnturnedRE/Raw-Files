// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.Achievement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class Achievement : MonoBehaviour
  {
    private void OnTriggerEnter(Collider other)
    {
      bool has;
      if (Dedicator.isDedicated || other.transform.tag != "Player" || ((Object) other.transform != (Object) Player.player.transform || !Provider.provider.achievementsService.getAchievement(this.transform.name, out has)) || has)
        return;
      Provider.provider.achievementsService.setAchievement(this.transform.name);
    }
  }
}
