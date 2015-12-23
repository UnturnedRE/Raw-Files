// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.PowerTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
  public class PowerTool
  {
    public static T[] checkInteractables<T>(Vector3 point, float radius) where T : Interactable
    {
      Collider[] colliderArray = Physics.OverlapSphere(point, radius, RayMasks.POWER_INTERACT);
      List<T> list = new List<T>();
      for (int index = 0; index < colliderArray.Length; ++index)
      {
        T component = colliderArray[index].GetComponent<T>();
        if ((Object) component != (Object) null)
          list.Add(component);
      }
      return list.ToArray();
    }

    public static InteractableFire[] checkFires(Vector3 point, float radius)
    {
      return PowerTool.checkInteractables<InteractableFire>(point, radius);
    }

    public static InteractableGenerator[] checkGenerators(Vector3 point, float radius)
    {
      return PowerTool.checkInteractables<InteractableGenerator>(point, radius);
    }

    public static InteractablePower[] checkPower(Vector3 point, float radius)
    {
      return PowerTool.checkInteractables<InteractablePower>(point, radius);
    }
  }
}
