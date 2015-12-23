// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.InteractableGenerator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

namespace SDG.Unturned
{
  public class InteractableGenerator : Interactable
  {
    public static readonly ushort FUEL = (ushort) 1000;
    private bool _isPowered;
    private ushort _fuel;
    private Transform engine;
    private float lastBurn;
    private bool isWiring;

    public bool isRefillable
    {
      get
      {
        return (int) this.fuel < (int) InteractableGenerator.FUEL;
      }
    }

    public bool isPowered
    {
      get
      {
        return this._isPowered;
      }
    }

    public ushort fuel
    {
      get
      {
        return this._fuel;
      }
    }

    public void askBurn(ushort amount)
    {
      if ((int) amount == 0)
        return;
      if ((int) amount >= (int) this.fuel)
        this._fuel = (ushort) 0;
      else
        this._fuel -= amount;
      this.updateState();
    }

    public void askFill(ushort amount)
    {
      if ((int) amount == 0)
        return;
      if ((int) amount >= (int) InteractableGenerator.FUEL - (int) this.fuel)
        this._fuel = InteractableGenerator.FUEL;
      else
        this._fuel += amount;
      this.updateState();
    }

    public void tellFuel(ushort newFuel)
    {
      this._fuel = newFuel;
      this.updateWire();
    }

    public void updatePowered(bool newPowered)
    {
      this._isPowered = newPowered;
      this.updateWire();
    }

    public override void updateState(Asset asset, byte[] state)
    {
      this._isPowered = (int) state[0] == 1;
      this._fuel = BitConverter.ToUInt16(state, 1);
      if (Dedicator.isDedicated)
        return;
      this.engine = this.transform.FindChild("Engine");
    }

    public override void use()
    {
      BarricadeManager.toggleGenerator(this.transform);
    }

    public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
    {
      message = !this.isPowered ? EPlayerMessage.GENERATOR_ON : EPlayerMessage.GENERATOR_OFF;
      text = string.Empty;
      color = Color.white;
      return true;
    }

    private void updateState()
    {
      byte[] state = new byte[3];
      state[0] = !this.isPowered ? (byte) 0 : (byte) 1;
      BitConverter.GetBytes(this.fuel).CopyTo((Array) state, 1);
      BarricadeManager.updateState(this.transform, state, state.Length);
    }

    private void updateWire()
    {
      if ((UnityEngine.Object) this.engine != (UnityEngine.Object) null)
        this.engine.gameObject.SetActive(this.isPowered && (int) this.fuel > 0);
      foreach (InteractablePower interactablePower in PowerTool.checkPower(this.transform.position, 16f))
      {
        if (interactablePower.isPlant == this.isPlant)
        {
          if (interactablePower.isWired)
          {
            if (!this.isPowered || (int) this.fuel == 0)
            {
              bool flag = false;
              InteractableGenerator[] interactableGeneratorArray = PowerTool.checkGenerators(interactablePower.transform.position, 16f);
              for (int index = 0; index < interactableGeneratorArray.Length; ++index)
              {
                if ((UnityEngine.Object) interactableGeneratorArray[index].transform != (UnityEngine.Object) this.transform && interactableGeneratorArray[index].isPowered && interactableGeneratorArray[index].isPlant == interactablePower.isPlant)
                {
                  flag = true;
                  break;
                }
              }
              if (!flag)
                interactablePower.updateWired(false);
            }
          }
          else if (this.isPowered && (int) this.fuel > 0)
            interactablePower.updateWired(true);
        }
      }
    }

    private void OnDestroy()
    {
      this.updatePowered(false);
    }

    private void Start()
    {
      this.updateWire();
      this.lastBurn = Time.realtimeSinceStartup;
    }

    private void FixedUpdate()
    {
      if (!Provider.isServer || (double) Time.realtimeSinceStartup - (double) this.lastBurn <= 5.0)
        return;
      this.lastBurn = Time.realtimeSinceStartup;
      if (!this.isPowered)
        return;
      if ((int) this.fuel > 0)
      {
        this.isWiring = true;
        this.askBurn((ushort) 1);
        BarricadeManager.sendFuel(this.transform, this.fuel);
      }
      else
      {
        if (!this.isWiring)
          return;
        this.isWiring = false;
        this.updateWire();
      }
    }
  }
}
