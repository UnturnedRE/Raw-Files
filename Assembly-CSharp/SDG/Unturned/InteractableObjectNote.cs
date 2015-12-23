// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.InteractableObjectNote
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class InteractableObjectNote : InteractableObject
  {
    public override void use()
    {
      PlayerBarricadeSignUI.open(this.interactabilityText);
      PlayerLifeUI.close();
    }

    public override bool checkUseable()
    {
      return !PlayerUI.window.showCursor;
    }

    public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
    {
      message = this.interactabilityHint == EObjectInteractabilityHint.USE ? EPlayerMessage.USE : EPlayerMessage.NONE;
      text = string.Empty;
      color = Color.white;
      return !PlayerUI.window.showCursor;
    }
  }
}
