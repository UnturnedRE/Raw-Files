// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.AlertTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
  internal class AlertTool
  {
    public static void alert(Player player, Vector3 position, float radius, bool sneak)
    {
      if ((Object) player == (Object) null)
        return;
      if (LightingManager.isFullMoon && (int) player.movement.nav != (int) byte.MaxValue)
      {
        using (List<Zombie>.Enumerator enumerator = ZombieManager.regions[(int) player.movement.nav].zombies.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            Zombie current = enumerator.Current;
            if (current.checkAlert(player))
              current.alert(player);
          }
        }
      }
      Collider[] colliderArray = Physics.OverlapSphere(position, radius, RayMasks.AGENT);
      for (int index = 0; index < colliderArray.Length; ++index)
      {
        Zombie component1 = colliderArray[index].transform.GetComponent<Zombie>();
        RaycastHit hitInfo;
        if ((Object) component1 != (Object) null && !LightingManager.isFullMoon)
        {
          if ((int) player.movement.nav != (int) byte.MaxValue && component1.checkAlert(player))
          {
            Vector3 vector3 = component1.transform.position - position;
            if ((double) Vector3.Dot(component1.transform.forward, vector3.normalized) <= 0.5 || !sneak)
            {
              Physics.Raycast(component1.transform.position + Vector3.up, -vector3, out hitInfo, vector3.magnitude * 0.95f, RayMasks.BLOCK_VISION);
              if (!((Object) hitInfo.transform != (Object) null))
                component1.alert(player);
            }
          }
        }
        else
        {
          Animal component2 = colliderArray[index].GetComponent<Animal>();
          if ((Object) component2 != (Object) null && component2.asset != null)
          {
            if (component2.asset.behaviour == EAnimalBehaviour.DEFENSE)
            {
              if (!component2.isFleeing)
              {
                Vector3 vector3 = component2.transform.position - position;
                if ((double) Vector3.Dot(component2.transform.forward, vector3.normalized) <= 0.5 || !sneak)
                {
                  Physics.Raycast(component2.transform.position + Vector3.up, -vector3, out hitInfo, vector3.magnitude * 0.95f, RayMasks.BLOCK_VISION);
                  if ((Object) hitInfo.transform != (Object) null)
                    continue;
                }
                else
                  continue;
              }
              component2.alert(player.transform.position);
            }
            else if (component2.asset.behaviour == EAnimalBehaviour.OFFENSE && component2.checkAlert(player))
            {
              Vector3 vector3 = component2.transform.position - position;
              if ((double) Vector3.Dot(component2.transform.forward, vector3.normalized) <= 0.5 || !sneak)
              {
                Physics.Raycast(component2.transform.position + Vector3.up, -vector3, out hitInfo, vector3.magnitude * 0.95f, RayMasks.BLOCK_VISION);
                if (!((Object) hitInfo.transform != (Object) null))
                  component2.alert(player);
              }
            }
          }
        }
      }
    }

    public static void alert(Vector3 position, float radius)
    {
      bool flag = LevelNavigation.checkNavigation(position);
      Collider[] colliderArray = Physics.OverlapSphere(position, radius, RayMasks.AGENT);
      for (int index = 0; index < colliderArray.Length; ++index)
      {
        if ((Object) colliderArray[index].transform.GetComponent<Zombie>() != (Object) null)
        {
          if (flag)
            colliderArray[index].GetComponent<Zombie>().alert(position);
        }
        else
        {
          Animal component = colliderArray[index].transform.GetComponent<Animal>();
          if ((Object) component != (Object) null && component.asset != null && component.asset.behaviour != EAnimalBehaviour.IGNORE)
            component.alert(position);
        }
      }
    }
  }
}
