// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.InteractableObjectBinaryState
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Pathfinding;
using UnityEngine;

namespace SDG.Unturned
{
  public class InteractableObjectBinaryState : InteractableObject
  {
    private float lastUsed = -9999f;
    private bool _isUsed;
    private Animation animationComponent;
    private AudioSource audioSourceComponent;
    private NavmeshCut cutComponent;
    private float cutHeight;
    private GameObject toggleGameObject;

    public bool isUsed
    {
      get
      {
        return this._isUsed;
      }
    }

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

    private void initAnimationComponent()
    {
      Transform child = this.transform.FindChild("Root");
      if (!((Object) child != (Object) null))
        return;
      this.animationComponent = child.GetComponent<Animation>();
    }

    private void updateAnimationComponent()
    {
      if (!((Object) this.animationComponent != (Object) null))
        return;
      if (this.isUsed)
        this.animationComponent.Play("Open");
      else
        this.animationComponent.Play("Close");
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

    private void initCutComponent()
    {
      if (this.interactabilityNav == EObjectInteractabilityNav.NONE)
        return;
      Transform child1 = this.transform.FindChild("Nav");
      if (!((Object) child1 != (Object) null))
        return;
      Transform child2 = child1.FindChild("Blocker");
      if (!((Object) child2 != (Object) null))
        return;
      this.cutComponent = child2.GetComponent<NavmeshCut>();
      this.cutHeight = this.cutComponent.height;
    }

    private void updateCutComponent()
    {
      if (!((Object) this.cutComponent != (Object) null))
        return;
      if (this.interactabilityNav == EObjectInteractabilityNav.ON && !this.isUsed || this.interactabilityNav == EObjectInteractabilityNav.OFF && this.isUsed)
      {
        this.cutHeight = this.cutComponent.height;
        this.cutComponent.height = 0.0f;
      }
      else
        this.cutComponent.height = this.cutHeight;
      this.cutComponent.ForceUpdate();
    }

    private void initToggleGameObject()
    {
      Transform child = this.transform.FindChild("Toggle");
      if (!((Object) child != (Object) null))
        return;
      this.toggleGameObject = child.gameObject;
    }

    private void updateToggleGameObject()
    {
      if (!((Object) this.toggleGameObject != (Object) null))
        return;
      if (this.interactabilityPower == EObjectInteractabilityPower.STAY)
        this.toggleGameObject.SetActive(this.isUsed && this.isWired);
      else
        this.toggleGameObject.SetActive(this.isUsed);
    }

    public void updateToggle(bool newUsed)
    {
      this.lastUsed = Time.realtimeSinceStartup;
      this._isUsed = newUsed;
      this.updateAnimationComponent();
      this.updateCutComponent();
      this.updateAudioSourceComponent();
      this.updateToggleGameObject();
    }

    protected override void updateWired()
    {
      this.updateToggleGameObject();
    }

    public override void updateState(Asset asset, byte[] state)
    {
      base.updateState(asset, state);
      this._isUsed = (int) state[0] == 1;
      this.initAnimationComponent();
      this.initCutComponent();
      this.initAudioSourceComponent();
      this.initToggleGameObject();
      this.updateAnimationComponent();
      this.updateCutComponent();
      this.updateToggleGameObject();
    }

    public override void use()
    {
      ObjectManager.toggleObjectBinaryState(this.transform);
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
        message = EPlayerMessage.POWER;
      else if (this.isUsed)
      {
        switch (this.interactabilityHint)
        {
          case EObjectInteractabilityHint.DOOR:
            message = EPlayerMessage.DOOR_CLOSE;
            break;
          case EObjectInteractabilityHint.SWITCH:
            message = EPlayerMessage.SPOT_OFF;
            break;
          case EObjectInteractabilityHint.FIRE:
            message = EPlayerMessage.FIRE_OFF;
            break;
          case EObjectInteractabilityHint.GENERATOR:
            message = EPlayerMessage.GENERATOR_OFF;
            break;
          case EObjectInteractabilityHint.USE:
            message = EPlayerMessage.USE;
            break;
          default:
            message = EPlayerMessage.NONE;
            break;
        }
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
