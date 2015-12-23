// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.Wheel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class Wheel
  {
    private RaycastHit hit;
    private InteractableVehicle _vehicle;
    private WheelCollider _wheel;
    private WheelFrictionCurve forwardFriction;
    private WheelFrictionCurve sidewaysFriction;
    private bool _isSteered;
    private bool _isPowered;
    private bool _isGrounded;
    private float direction;
    private float steer;
    private float speed;

    public InteractableVehicle vehicle
    {
      get
      {
        return this._vehicle;
      }
    }

    public WheelCollider wheel
    {
      get
      {
        return this._wheel;
      }
    }

    public bool isSteered
    {
      get
      {
        return this._isSteered;
      }
    }

    public bool isPowered
    {
      get
      {
        return this._isPowered;
      }
    }

    public bool isGrounded
    {
      get
      {
        return this._isGrounded;
      }
    }

    public Wheel(InteractableVehicle newVehicle, WheelCollider newWheel, bool newSteered, bool newPowered)
    {
      this._vehicle = newVehicle;
      this._wheel = newWheel;
      this.sidewaysFriction = this.wheel.sidewaysFriction;
      this.forwardFriction = this.wheel.forwardFriction;
      this._isSteered = newSteered;
      this._isPowered = newPowered;
      this.wheel.forceAppPointDistance = 0.0f;
    }

    public void enable()
    {
      if (!((Object) this.wheel != (Object) null))
        return;
      this.wheel.gameObject.SetActive(true);
    }

    public void disable()
    {
      if (!((Object) this.wheel != (Object) null))
        return;
      this.wheel.gameObject.SetActive(false);
    }

    public void reset()
    {
      this.direction = 0.0f;
      this.steer = 0.0f;
      this.speed = 0.0f;
      this.wheel.steerAngle = 0.0f;
      this.wheel.motorTorque = 0.0f;
      this.wheel.brakeTorque = this.vehicle.asset.brake * 0.25f;
      this.sidewaysFriction.stiffness = 0.25f;
      this.wheel.sidewaysFriction = this.sidewaysFriction;
      this.forwardFriction.stiffness = 0.25f;
      this.wheel.forwardFriction = this.forwardFriction;
    }

    public void simulate(float input_x, float input_y, bool inputBrake, float delta)
    {
      if (this.isSteered)
      {
        this.direction = input_x;
        this.steer = Mathf.Lerp(this.steer, Mathf.Lerp(this.vehicle.asset.steerMax, this.vehicle.asset.steerMin, this.vehicle.factor), 2f * delta);
      }
      if (this.isPowered)
        this.speed = (double) input_y <= 0.0 ? ((double) input_y >= 0.0 ? Mathf.Lerp(this.speed, 0.0f, delta) : ((double) this.vehicle.speed <= 0.0 ? Mathf.Lerp(this.speed, this.vehicle.asset.speedMin, delta) : Mathf.Lerp(this.speed, this.vehicle.asset.speedMin, 2f * delta))) : ((double) this.vehicle.speed >= 0.0 ? Mathf.Lerp(this.speed, this.vehicle.asset.speedMax, delta) : Mathf.Lerp(this.speed, this.vehicle.asset.speedMax, 2f * delta));
      if (inputBrake)
      {
        this.speed = 0.0f;
        this.wheel.motorTorque = 0.0f;
        this.wheel.brakeTorque = this.vehicle.asset.brake * (float) (1.0 - (double) this.vehicle.slip * 0.5);
      }
      else
        this.wheel.brakeTorque = 0.0f;
      this._isGrounded = Physics.Raycast(this.wheel.transform.position, -this.wheel.transform.up, out this.hit, this.wheel.suspensionDistance + this.wheel.radius, RayMasks.BLOCK_COLLISION);
    }

    public void update(float delta)
    {
      this.wheel.steerAngle = Mathf.Lerp(this.wheel.steerAngle, this.direction * this.steer, 4f * delta);
      this.sidewaysFriction.stiffness = Mathf.Lerp(this.wheel.sidewaysFriction.stiffness, (float) (1.0 - (double) this.vehicle.slip * 0.75), 4f * delta);
      this.wheel.sidewaysFriction = this.sidewaysFriction;
      this.forwardFriction.stiffness = Mathf.Lerp(this.wheel.forwardFriction.stiffness, (float) (2.0 - (double) this.vehicle.slip * 1.5), 4f * delta);
      this.wheel.forwardFriction = this.forwardFriction;
      if ((double) this.speed > 0.0)
      {
        if ((double) this.vehicle.speed < 0.0)
          this.wheel.motorTorque = Mathf.Lerp(this.wheel.motorTorque, this.speed, 4f * delta);
        else if ((double) this.vehicle.speed < (double) this.vehicle.asset.speedMax)
          this.wheel.motorTorque = Mathf.Lerp(this.wheel.motorTorque, this.speed, 2f * delta);
        else
          this.wheel.motorTorque = Mathf.Lerp(this.wheel.motorTorque, this.speed / 2f, 2f * delta);
      }
      else if ((double) this.vehicle.speed > 0.0)
        this.wheel.motorTorque = Mathf.Lerp(this.wheel.motorTorque, this.speed, 4f * delta);
      else if ((double) this.vehicle.speed > (double) this.vehicle.asset.speedMin)
        this.wheel.motorTorque = Mathf.Lerp(this.wheel.motorTorque, this.speed, 2f * delta);
      else
        this.wheel.motorTorque = Mathf.Lerp(this.wheel.motorTorque, this.speed / 2f, 2f * delta);
    }
  }
}
