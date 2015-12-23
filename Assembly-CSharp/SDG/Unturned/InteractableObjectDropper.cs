// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.InteractableObjectDropper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class InteractableObjectDropper : InteractableObject
  {
    private float lastUsed = -9999f;
    private ushort[] interactabilityDrops;
    private AudioSource audioSourceComponent;
    private Transform dropTransform;

    public bool isUsable
    {
      get
      {
        if ((double) Time.realtimeSinceStartup - (double) this.lastUsed <= (double) this.interactabilityDelay)
          return false;
        if (this.interactabilityPower != EObjectInteractabilityPower.NONE)
          return this.isWired;
        return true;
      }
    }

    private void initAudioSourceComponent()
    {
      this.audioSourceComponent = this.transform.GetComponent<AudioSource>();
    }

    private void updateAudioSourceComponent()
    {
      if (!((Object) this.audioSourceComponent != (Object) null) || Dedicator.isDedicated)
        return;
      this.audioSourceComponent.Play();
    }

    private void initDropTransform()
    {
      this.dropTransform = this.transform.FindChild("Drop");
    }

    public override void updateState(Asset asset, byte[] state)
    {
      base.updateState(asset, state);
      this.interactabilityDrops = ((ObjectAsset) asset).interactabilityDrops;
      this.initAudioSourceComponent();
      this.initDropTransform();
    }

    public void drop()
    {
      this.lastUsed = Time.realtimeSinceStartup;
      ushort newID = this.interactabilityDrops[Random.Range(0, this.interactabilityDrops.Length)];
      if ((int) newID == 0 || (Object) this.dropTransform == (Object) null)
        return;
      ItemManager.dropItem(new Item(newID, true), this.dropTransform.position, false, true, false);
    }

    public override void use()
    {
      this.updateAudioSourceComponent();
      ObjectManager.useObjectDropper(this.transform);
    }

    public override bool checkUseable()
    {
      if (this.interactabilityPower != EObjectInteractabilityPower.NONE)
        return this.isWired;
      return true;
    }

    public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
    {
      if (this.interactabilityPower != EObjectInteractabilityPower.NONE && !this.isWired)
      {
        message = EPlayerMessage.POWER;
      }
      else
      {
        switch (this.interactabilityHint)
        {
          case EObjectInteractabilityHint.DOOR:
            message = EPlayerMessage.DOOR_OPEN;
            break;
          case EObjectInteractabilityHint.SWITCH:
            message = EPlayerMessage.SPOT_ON;
            break;
          case EObjectInteractabilityHint.FIRE:
            message = EPlayerMessage.FIRE_ON;
            break;
          case EObjectInteractabilityHint.GENERATOR:
            message = EPlayerMessage.GENERATOR_ON;
            break;
          case EObjectInteractabilityHint.USE:
            message = EPlayerMessage.USE;
            break;
          default:
            message = EPlayerMessage.NONE;
            break;
        }
      }
      text = string.Empty;
      color = Color.white;
      return true;
    }
  }
}
