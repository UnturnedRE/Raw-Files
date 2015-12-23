// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.InteractablePower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

namespace SDG.Unturned
{
  public class InteractablePower : Interactable
  {
    protected bool _isWired;

    public bool isWired
    {
      get
      {
        return this._isWired;
      }
    }

    protected virtual void updateWired()
    {
    }

    public void updateWired(bool newWired)
    {
      if (newWired == this.isWired)
        return;
      this._isWired = newWired;
      this.updateWired();
    }

    private void Start()
    {
      foreach (InteractableGenerator interactableGenerator in PowerTool.checkGenerators(this.transform.position, 16f))
      {
        if (interactableGenerator.isPowered && (int) interactableGenerator.fuel > 0)
        {
          this.updateWired(true);
          break;
        }
      }
    }
  }
}
