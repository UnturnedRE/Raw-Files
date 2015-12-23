// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.DamageInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class DamageInfo
  {
    public Transform transform;
    public Collider collider;
    public float distance;
    public Vector3 point;
    public Vector3 normal;
    public Player player;
    public Zombie zombie;
    public InteractableVehicle vehicle;

    public void update(RaycastHit hit)
    {
      this.transform = hit.transform;
      this.collider = hit.collider;
      this.distance = hit.distance;
      this.point = hit.point;
      this.normal = hit.normal;
    }
  }
}
