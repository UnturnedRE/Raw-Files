// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.Interactable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class Interactable : MonoBehaviour
  {
    public bool isPlant
    {
      get
      {
        if ((Object) this.transform.parent != (Object) null)
          return this.transform.parent.tag == "Vehicle";
        return false;
      }
    }

    public virtual void updateState(Asset asset, byte[] state)
    {
    }

    public virtual bool checkInteractable()
    {
      return true;
    }

    public virtual bool checkUseable()
    {
      return true;
    }

    public virtual bool checkHint(out EPlayerMessage message, out string text, out Color color)
    {
      message = EPlayerMessage.NONE;
      text = string.Empty;
      color = Color.white;
      return false;
    }

    public virtual void use()
    {
    }
  }
}
