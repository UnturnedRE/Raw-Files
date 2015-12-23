// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.VehicleTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class VehicleTool
  {
    public static bool giveVehicle(Player player, ushort id)
    {
      if ((VehicleAsset) Assets.find(EAssetType.VEHICLE, id) == null)
        return false;
      Vector3 point = player.transform.position + player.transform.forward * 6f;
      RaycastHit hitInfo;
      Physics.Raycast(point + Vector3.up * 16f, Vector3.down, out hitInfo, 32f, RayMasks.BLOCK_VEHICLE);
      if ((Object) hitInfo.collider != (Object) null)
        point.y = hitInfo.point.y + 16f;
      VehicleManager.spawnVehicle(id, point, player.transform.rotation);
      return true;
    }
  }
}
