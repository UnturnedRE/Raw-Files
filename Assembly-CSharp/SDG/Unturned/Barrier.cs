// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.Barrier
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
  public class Barrier : MonoBehaviour
  {
    private void OnTriggerEnter(Collider other)
    {
      if (!Provider.isServer || !(other.transform.tag == "Player"))
        return;
      Player player = DamageTool.getPlayer(other.transform);
      if (!((Object) player != (Object) null))
        return;
      EPlayerKill kill;
      player.life.askDamage((byte) 100, Vector3.up * 10f, EDeathCause.SUICIDE, ELimb.SKULL, CSteamID.Nil, out kill);
    }
  }
}
