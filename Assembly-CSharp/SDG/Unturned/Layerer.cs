// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.Layerer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class Layerer
  {
    public static void viewmodel(Transform target)
    {
      if ((Object) target.GetComponent<Renderer>() != (Object) null)
      {
        target.GetComponent<Renderer>().castShadows = false;
        target.GetComponent<Renderer>().receiveShadows = false;
        target.tag = "Viewmodel";
        target.gameObject.layer = LayerMasks.VIEWMODEL;
      }
      else
      {
        if (!((Object) target.GetComponent<LODGroup>() != (Object) null))
          return;
        for (int index = 0; index < 4; ++index)
        {
          Transform child = target.FindChild("Model_" + (object) index);
          if ((Object) child == (Object) null)
            break;
          if ((Object) child.GetComponent<Renderer>() != (Object) null)
          {
            child.GetComponent<Renderer>().castShadows = false;
            child.GetComponent<Renderer>().receiveShadows = false;
            child.tag = "Viewmodel";
            child.gameObject.layer = LayerMasks.VIEWMODEL;
          }
        }
      }
    }
  }
}
