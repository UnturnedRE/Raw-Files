// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.Grenade
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class Grenade : MonoBehaviour
  {
    public float playerDamage;
    public float zombieDamage;
    public float animalDamage;
    public float barricadeDamage;
    public float structureDamage;
    public float vehicleDamage;
    public float resourceDamage;

    public void Explode()
    {
      DamageTool.explode(this.transform.position, 8f, EDeathCause.GRENADE, this.playerDamage, this.zombieDamage, this.animalDamage, this.barricadeDamage, this.structureDamage, this.vehicleDamage, this.resourceDamage);
      EffectManager.sendEffect((ushort) 34, EffectManager.LARGE, this.transform.position);
      Object.Destroy((Object) this.gameObject);
    }

    private void Start()
    {
      this.Invoke("Explode", 2.5f);
    }
  }
}
