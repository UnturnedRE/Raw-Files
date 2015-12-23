// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.InteractableObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

namespace SDG.Unturned
{
  public class InteractableObject : InteractablePower
  {
    protected EObjectInteractabilityHint interactabilityHint;
    protected string interactabilityText;
    protected EObjectInteractabilityPower interactabilityPower;
    protected EObjectInteractabilityNav interactabilityNav;
    protected float interactabilityDelay;

    public override void updateState(Asset asset, byte[] state)
    {
      base.updateState(asset, state);
      this.interactabilityHint = ((ObjectAsset) asset).interactabilityHint;
      this.interactabilityText = ((ObjectAsset) asset).interactabilityText;
      this.interactabilityPower = ((ObjectAsset) asset).interactabilityPower;
      this.interactabilityNav = ((ObjectAsset) asset).interactabilityNav;
      this.interactabilityDelay = ((ObjectAsset) asset).interactabilityDelay;
    }
  }
}
