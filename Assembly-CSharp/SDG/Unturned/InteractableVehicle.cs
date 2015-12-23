// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.InteractableVehicle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
  public class InteractableVehicle : Interactable
  {
    private static readonly float EXPLODE = 4f;
    private static readonly ushort HEALTH_0 = (ushort) 100;
    private static readonly ushort HEALTH_1 = (ushort) 200;
    public ushort index;
    public ushort id;
    public ushort fuel;
    public ushort health;
    private uint lastBurn;
    private float horned;
    private bool _isDrowned;
    private float _lastDead;
    private float _lastUnderwater;
    private float _lastExploded;
    private float _slip;
    public bool isExploded;
    private float _factor;
    private float _speed;
    private float _spedometer;
    private int _turn;
    private float spin;
    private float _steer;
    private Transform wheel;
    private Transform front;
    private Quaternion rest;
    private Transform fire;
    private Transform smoke_0;
    private Transform smoke_1;
    public bool isUpdated;
    private Transform _sirens;
    private Transform siren_0;
    private Transform siren_1;
    private bool _sirensOn;
    private Transform _headlights;
    private bool _headlightsOn;
    private Transform _taillights;
    private bool _taillightsOn;
    private VehicleAsset _asset;
    private Passenger[] _passengers;
    private Wheel[] tires;
    private Transform[] wheels;
    private Vector3 lastUpdatedPos;
    private NetworkSnapshotBuffer nsb;
    private Vector3 real;
    private float lastTick;
    private bool lastHorn;
    private bool lastLights;
    private bool lastSirens;
    private float lastWeeoo;
    private AudioSource sound;
    private bool isRecovering;
    private float lastRecover;
    private byte recovery;
    private bool isPhysical;
    private bool isFrozen;

    public bool isHornable
    {
      get
      {
        return (double) Time.realtimeSinceStartup - (double) this.horned > 0.5;
      }
    }

    public bool isRefillable
    {
      get
      {
        if ((int) this.fuel < (int) this.asset.fuel)
          return !this.isDriven;
        return false;
      }
    }

    public bool isRepaired
    {
      get
      {
        return (int) this.health == (int) this.asset.health;
      }
    }

    public bool isDriven
    {
      get
      {
        if (this.passengers != null)
          return this.passengers[0].player != null;
        return false;
      }
    }

    public bool isDriver
    {
      get
      {
        if (!Dedicator.isDedicated)
          return this.checkDriver(Provider.client);
        return false;
      }
    }

    public bool isEmpty
    {
      get
      {
        for (byte index = (byte) 0; (int) index < this.passengers.Length; ++index)
        {
          if (this.passengers[(int) index].player != null)
            return false;
        }
        return true;
      }
    }

    public bool isDrowned
    {
      get
      {
        return this._isDrowned;
      }
    }

    public bool isUnderwater
    {
      get
      {
        if ((double) LevelLighting.seaLevel < 0.990000009536743)
          return (double) this.transform.position.y < (double) LevelLighting.seaLevel * (double) Level.TERRAIN - 1.0;
        return false;
      }
    }

    public float lastDead
    {
      get
      {
        return this._lastDead;
      }
    }

    public float lastUnderwater
    {
      get
      {
        return this._lastUnderwater;
      }
    }

    public float lastExploded
    {
      get
      {
        return this._lastExploded;
      }
    }

    public float slip
    {
      get
      {
        return this._slip;
      }
    }

    public bool isDead
    {
      get
      {
        return (int) this.health == 0;
      }
    }

    public float factor
    {
      get
      {
        return this._factor;
      }
    }

    public float speed
    {
      get
      {
        return this._speed;
      }
    }

    public float spedometer
    {
      get
      {
        return this._spedometer;
      }
    }

    public int turn
    {
      get
      {
        return this._turn;
      }
    }

    public float steer
    {
      get
      {
        return this._steer;
      }
    }

    public Transform sirens
    {
      get
      {
        return this._sirens;
      }
    }

    public bool sirensOn
    {
      get
      {
        return this._sirensOn;
      }
    }

    public Transform headlights
    {
      get
      {
        return this._headlights;
      }
    }

    public bool headlightsOn
    {
      get
      {
        return this._headlightsOn;
      }
    }

    public Transform taillights
    {
      get
      {
        return this._taillights;
      }
    }

    public bool taillightsOn
    {
      get
      {
        return this._taillightsOn;
      }
    }

    public VehicleAsset asset
    {
      get
      {
        return this._asset;
      }
    }

    public Passenger[] passengers
    {
      get
      {
        return this._passengers;
      }
    }

    public void askBurn(ushort amount)
    {
      if ((int) amount == 0 || this.isExploded)
        return;
      if ((int) amount >= (int) this.fuel)
        this.fuel = (ushort) 0;
      else
        this.fuel -= amount;
    }

    public void askFill(ushort amount)
    {
      if ((int) amount == 0 || this.isExploded)
        return;
      if ((int) amount >= (int) this.asset.fuel - (int) this.fuel)
        this.fuel = this.asset.fuel;
      else
        this.fuel += amount;
      VehicleManager.sendVehicleFuel(this, this.fuel);
    }

    public void askDamage(ushort amount, bool canRepair)
    {
      if ((int) amount == 0)
        return;
      if (this.isDead)
      {
        if (canRepair)
          return;
        this.explode();
      }
      else
      {
        if ((int) amount >= (int) this.health)
          this.health = (ushort) 0;
        else
          this.health -= amount;
        VehicleManager.sendVehicleHealth(this, this.health);
        if (!this.isDead || canRepair)
          return;
        this.explode();
      }
    }

    public void askRepair(ushort amount)
    {
      if ((int) amount == 0 || this.isExploded)
        return;
      if ((int) amount >= (int) this.asset.health - (int) this.health)
        this.health = this.asset.health;
      else
        this.health += amount;
      VehicleManager.sendVehicleHealth(this, this.health);
    }

    private void explode()
    {
      this.GetComponent<Rigidbody>().AddForce(0.0f, 1024f, 0.0f);
      this.GetComponent<Rigidbody>().AddTorque(16f, 0.0f, 0.0f);
      DamageTool.explode(this.transform.position, 8f, EDeathCause.VEHICLE, 200f, 200f, 200f, 1500f, 1500f, 500f, 2000f);
      int num = Random.Range(3, 7);
      for (int index = 0; index < num; ++index)
      {
        float f = Random.Range(0.0f, 6.283185f);
        ItemManager.dropItem(new Item((ushort) 67, true), this.transform.position + new Vector3(Mathf.Sin(f) * 3f, 1f, Mathf.Cos(f) * 3f), false, Dedicator.isDedicated, true);
      }
      VehicleManager.sendVehicleExploded(this);
      EffectManager.sendEffect(this.asset.explosion, EffectManager.LARGE, this.transform.position);
    }

    public override bool checkUseable()
    {
      if ((double) this.speed < 5.0)
        return !this.isExploded;
      return false;
    }

    public override void use()
    {
      VehicleManager.enterVehicle(this.transform);
    }

    public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
    {
      message = EPlayerMessage.VEHICLE_ENTER;
      text = this.asset.vehicleName;
      color = Color.white;
      return this.checkUseable();
    }

    public void updateVehicle()
    {
      this.lastUpdatedPos = this.transform.position;
      if (this.nsb != null)
        this.nsb.updateLastSnapshot(this.transform.position, this.transform.rotation);
      this.real = this.transform.position;
      this.isRecovering = false;
      this.lastRecover = Time.realtimeSinceStartup;
      this.recovery = (byte) 10;
    }

    public void updatePhysics()
    {
      if (this.checkDriver(Provider.client) || Provider.isServer && !this.isDriven)
      {
        this.GetComponent<Rigidbody>().useGravity = true;
        this.GetComponent<Rigidbody>().isKinematic = false;
        this.isPhysical = true;
        if (!this.isExploded)
        {
          for (int index = 0; index < this.tires.Length; ++index)
            this.tires[index].enable();
        }
      }
      else
      {
        this.GetComponent<Rigidbody>().useGravity = false;
        this.GetComponent<Rigidbody>().isKinematic = true;
        this.isPhysical = false;
        for (int index = 0; index < this.tires.Length; ++index)
          this.tires[index].disable();
      }
      this.GetComponent<Rigidbody>().centerOfMass = new Vector3(0.0f, -0.25f, 0.0f);
    }

    public void updateEngine()
    {
      this.tellTaillights(this.isDriven);
    }

    public void tellSirens(bool on)
    {
      this._sirensOn = on;
      if (Dedicator.isDedicated || !((Object) this.sirens != (Object) null))
        return;
      this.sirens.gameObject.SetActive(this.sirensOn);
    }

    public void tellHeadlights(bool on)
    {
      this._headlightsOn = on;
      if (Dedicator.isDedicated || !((Object) this.headlights != (Object) null))
        return;
      this.headlights.gameObject.SetActive(this.headlightsOn);
    }

    public void tellTaillights(bool on)
    {
      this._taillightsOn = on;
      if (Dedicator.isDedicated || !((Object) this.taillights != (Object) null))
        return;
      this.taillights.gameObject.SetActive(this.taillightsOn);
    }

    public void tellHorn()
    {
      this.horned = Time.realtimeSinceStartup;
      if (!Dedicator.isDedicated)
      {
        this.sound.pitch = 1f;
        this.sound.PlayOneShot(this.asset.horn);
      }
      if (!Provider.isServer)
        return;
      AlertTool.alert(this.transform.position, 32f);
    }

    public void tellFuel(ushort newFuel)
    {
      this.fuel = newFuel;
    }

    public void tellExploded()
    {
      this.isExploded = true;
      this._lastExploded = Time.realtimeSinceStartup;
      if (this.sirensOn)
        this.tellSirens(false);
      if (this.headlightsOn)
        this.tellHeadlights(false);
      for (int index = 0; index < this.tires.Length; ++index)
        this.tires[index].disable();
      if (Dedicator.isDedicated)
        return;
      HighlighterTool.color(this.transform, new Color(0.25f, 0.25f, 0.25f));
      this.updateFires();
      if (this.wheels == null)
        return;
      for (int index = 0; index < this.wheels.Length; ++index)
      {
        this.wheels[index].transform.parent = Level.effects;
        this.wheels[index].GetComponent<Collider>().enabled = true;
        this.wheels[index].gameObject.AddComponent<Rigidbody>();
        this.wheels[index].GetComponent<Rigidbody>().drag = 0.5f;
        this.wheels[index].GetComponent<Rigidbody>().angularDrag = 0.1f;
        Object.Destroy((Object) this.wheels[index].gameObject, 8f);
        if (index % 2 == 0)
          this.wheels[index].GetComponent<Rigidbody>().AddForce(-this.wheels[index].right * 512f + Vector3.up * 128f);
        else
          this.wheels[index].GetComponent<Rigidbody>().AddForce(this.wheels[index].right * 512f + Vector3.up * 128f);
      }
    }

    public void updateFires()
    {
      if (Dedicator.isDedicated)
        return;
      this.fire.gameObject.SetActive((this.isExploded || this.isDead) && !this.isUnderwater);
      this.smoke_0.gameObject.SetActive((this.isExploded || (int) this.health < (int) InteractableVehicle.HEALTH_0) && !this.isUnderwater);
      this.smoke_1.gameObject.SetActive((this.isExploded || (int) this.health < (int) InteractableVehicle.HEALTH_1) && !this.isUnderwater);
    }

    public void tellHealth(ushort newHealth)
    {
      this.health = newHealth;
      if (this.isDead)
        this._lastDead = Time.realtimeSinceStartup;
      this.updateFires();
    }

    public void tellPosition(Vector3 newPosition)
    {
      this.lastTick = Time.realtimeSinceStartup;
      this.GetComponent<Rigidbody>().MovePosition(newPosition);
      this.recovery = (byte) 0;
      this.isFrozen = true;
      this.GetComponent<Rigidbody>().useGravity = false;
      this.GetComponent<Rigidbody>().isKinematic = true;
    }

    public void tellState(Vector3 newPosition, byte newAngle_X, byte newAngle_Y, byte newAngle_Z, byte newSpeed, byte newTurn)
    {
      if (this.isDriver)
        return;
      this.lastTick = Time.realtimeSinceStartup;
      this.lastUpdatedPos = newPosition;
      if (this.nsb != null)
        this.nsb.addNewSnapshot(newPosition, Quaternion.Euler((float) ((int) newAngle_X * 2), (float) ((int) newAngle_Y * 2), (float) ((int) newAngle_Z * 2)));
      this._speed = (float) ((int) newSpeed - 128);
      this._turn = (int) newTurn - 1;
    }

    public bool checkDriver(CSteamID steamID)
    {
      if (this.isDriven)
        return this.passengers[0].player.playerID.steamID == steamID;
      return false;
    }

    public void kickPlayer(byte seat)
    {
      if (this.passengers[(int) seat].player == null)
        return;
      Vector3 point;
      byte angle;
      this.getExit(seat, out point, out angle);
      VehicleManager.sendExitVehicle(this, seat, point, angle, false);
    }

    public void addPlayer(byte seat, CSteamID steamID)
    {
      SteamPlayer steamPlayer = PlayerTool.getSteamPlayer(steamID);
      if (steamPlayer != null)
      {
        this.passengers[(int) seat].player = steamPlayer;
        if ((Object) steamPlayer.player != (Object) null)
          steamPlayer.player.movement.setVehicle(this, seat, this.passengers[(int) seat].seat, Vector3.zero, (byte) 0, false);
        this.updatePhysics();
      }
      this.updateEngine();
      if ((int) seat != 0 || (int) this.fuel <= 0 || (Dedicator.isDedicated || this.isUnderwater))
        return;
      this.sound.pitch = Random.Range(0.9f, 1.1f);
      this.sound.PlayOneShot(this.asset.ignition);
      this.GetComponent<AudioSource>().pitch = 0.5f;
    }

    public void removePlayer(byte seat, Vector3 point, byte angle, bool forceUpdate)
    {
      if ((int) seat < this.passengers.Length)
      {
        SteamPlayer steamPlayer = this.passengers[(int) seat].player;
        if ((Object) steamPlayer.player != (Object) null)
          steamPlayer.player.movement.setVehicle((InteractableVehicle) null, (byte) 0, LevelPlayers.models, point, angle, forceUpdate);
        this.passengers[(int) seat].player = (SteamPlayer) null;
        this.updatePhysics();
        if (Provider.isServer)
          VehicleManager.sendVehicleFuel(this, this.fuel);
      }
      this.updateEngine();
      if ((int) seat != 0)
        return;
      if (!Dedicator.isDedicated)
        this.GetComponent<AudioSource>().volume = 0.0f;
      for (int index = 0; index < this.tires.Length; ++index)
        this.tires[index].reset();
    }

    public void swapPlayer(byte fromSeat, byte toSeat)
    {
      if ((int) fromSeat < this.passengers.Length && (int) toSeat < this.passengers.Length)
      {
        SteamPlayer steamPlayer = this.passengers[(int) fromSeat].player;
        if ((Object) steamPlayer.player != (Object) null)
          steamPlayer.player.movement.setVehicle(this, toSeat, this.passengers[(int) toSeat].seat, Vector3.zero, (byte) 0, false);
        this.passengers[(int) fromSeat].player = (SteamPlayer) null;
        this.passengers[(int) toSeat].player = steamPlayer;
        this.updatePhysics();
        if (Provider.isServer)
          VehicleManager.sendVehicleFuel(this, this.fuel);
      }
      this.updateEngine();
      if ((int) fromSeat != 0)
        return;
      if (!Dedicator.isDedicated)
        this.GetComponent<AudioSource>().volume = 0.0f;
      for (int index = 0; index < this.tires.Length; ++index)
        this.tires[index].reset();
    }

    public bool tryAddPlayer(out byte seat)
    {
      seat = byte.MaxValue;
      if ((double) this.speed >= 5.0 || this.isExploded)
        return false;
      for (byte index = (byte) 0; (int) index < this.passengers.Length; ++index)
      {
        if (this.passengers[(int) index] != null && this.passengers[(int) index].player == null)
        {
          seat = index;
          return true;
        }
      }
      return false;
    }

    public bool tryRemovePlayer(out byte seat, CSteamID player, out Vector3 point, out byte angle)
    {
      seat = byte.MaxValue;
      point = this.transform.position;
      angle = (byte) 0;
      for (byte index = (byte) 0; (int) index < this.passengers.Length; ++index)
      {
        if (this.passengers[(int) index] != null && this.passengers[(int) index].player != null && this.passengers[(int) index].player.playerID.steamID == player)
        {
          seat = index;
          this.getExit(seat, out point, out angle);
          return true;
        }
      }
      return false;
    }

    public bool trySwapPlayer(CSteamID player, byte toSeat, out byte fromSeat)
    {
      fromSeat = byte.MaxValue;
      if ((int) toSeat >= this.passengers.Length)
        return false;
      for (byte index = (byte) 0; (int) index < this.passengers.Length; ++index)
      {
        if (this.passengers[(int) index] != null && this.passengers[(int) index].player != null && this.passengers[(int) index].player.playerID.steamID == player)
        {
          if ((int) toSeat == (int) index)
            return false;
          fromSeat = index;
          return this.passengers[(int) toSeat].player == null;
        }
      }
      return false;
    }

    public void getExit(byte seat, out Vector3 point, out byte angle)
    {
      float maxDistance = this.asset.exit + Mathf.Abs(this.speed) * 0.1f;
      point = this.transform.position + new Vector3(0.0f, 2f, 0.0f);
      angle = MeasurementTool.angleToByte(this.transform.rotation.eulerAngles.y);
      if ((int) seat % 2 == 0)
      {
        RaycastHit hitInfo;
        Physics.Raycast(this.transform.position + this.transform.up, -this.transform.right, out hitInfo, maxDistance, RayMasks.BLOCK_EXIT);
        if ((Object) hitInfo.transform == (Object) null)
        {
          Physics.Raycast(this.transform.position + this.transform.up - this.transform.right * maxDistance, -this.transform.up, out hitInfo, 3f, RayMasks.BLOCK_EXIT);
          if (!((Object) hitInfo.transform != (Object) null))
            return;
          point = hitInfo.point + new Vector3(0.0f, 0.5f, 0.0f);
        }
        else
        {
          Physics.Raycast(this.transform.position + this.transform.up, this.transform.right, out hitInfo, maxDistance, RayMasks.BLOCK_EXIT);
          if (!((Object) hitInfo.transform == (Object) null))
            return;
          Physics.Raycast(this.transform.position + this.transform.up + this.transform.right * maxDistance, -this.transform.up, out hitInfo, 3f, RayMasks.BLOCK_EXIT);
          if (!((Object) hitInfo.transform != (Object) null))
            return;
          point = hitInfo.point + new Vector3(0.0f, 0.5f, 0.0f);
        }
      }
      else
      {
        RaycastHit hitInfo;
        Physics.Raycast(this.transform.position + this.transform.up, this.transform.right, out hitInfo, maxDistance, RayMasks.BLOCK_EXIT);
        if ((Object) hitInfo.transform == (Object) null)
        {
          Physics.Raycast(this.transform.position + this.transform.up + this.transform.right * maxDistance, -this.transform.up, out hitInfo, 3f, RayMasks.BLOCK_EXIT);
          if (!((Object) hitInfo.transform != (Object) null))
            return;
          point = hitInfo.point + new Vector3(0.0f, 0.5f, 0.0f);
        }
        else
        {
          Physics.Raycast(this.transform.position + this.transform.up, -this.transform.right, out hitInfo, maxDistance, RayMasks.BLOCK_EXIT);
          if (!((Object) hitInfo.transform == (Object) null))
            return;
          Physics.Raycast(this.transform.position + this.transform.up - this.transform.right * maxDistance, -this.transform.up, out hitInfo, 3f, RayMasks.BLOCK_EXIT);
          if (!((Object) hitInfo.transform != (Object) null))
            return;
          point = hitInfo.point + new Vector3(0.0f, 0.5f, 0.0f);
        }
      }
    }

    public void simulate(uint simulation, Vector3 point, Quaternion angle, float newSpeed, int newTurn, float delta)
    {
      if (this.isRecovering)
      {
        if ((double) (point - this.real).sqrMagnitude > 1.0)
        {
          if ((double) Time.realtimeSinceStartup - (double) this.lastRecover <= 1.0)
            return;
          this.lastRecover = Time.realtimeSinceStartup;
          VehicleManager.sendVehiclePosition(this, this.real);
          return;
        }
        this.isRecovering = false;
        this.recovery = (byte) 0;
      }
      if ((int) this.recovery < 10)
        ++this.recovery;
      else if ((double) Mathf.Pow(point.x - this.real.x, 2f) + (double) Mathf.Pow(point.z - this.real.z, 2f) > 16.0)
      {
        this.isRecovering = true;
        this.lastRecover = Time.realtimeSinceStartup;
        VehicleManager.sendVehiclePosition(this, this.real);
      }
      else
      {
        Vector3 vector3 = LevelGround.checkSafe(point);
        if ((double) (point - vector3).sqrMagnitude > 1.0)
        {
          this.isRecovering = true;
          this.lastRecover = Time.realtimeSinceStartup;
          this.real += Vector3.up * 3f;
          this.GetComponent<Rigidbody>().MovePosition(this.real);
          VehicleManager.sendVehiclePosition(this, this.real);
        }
        else
        {
          if (simulation - this.lastBurn > 5U)
          {
            this.lastBurn = simulation;
            this.askBurn((ushort) 1);
          }
          this._speed = newSpeed;
          this._turn = newTurn;
          this.GetComponent<Rigidbody>().MovePosition(point);
          this.GetComponent<Rigidbody>().MoveRotation(angle);
          this.real = point;
        }
      }
    }

    public void simulate(uint simulation, int input_x, int input_y, bool inputBrake, bool inputHorn, bool inputLights, bool inputSirens, float delta)
    {
      if ((int) this.recovery < 10)
      {
        ++this.recovery;
      }
      else
      {
        if (this.isFrozen)
        {
          this.isFrozen = false;
          this.GetComponent<Rigidbody>().useGravity = true;
          this.GetComponent<Rigidbody>().isKinematic = false;
        }
        if ((int) this.fuel == 0 || this.isUnderwater || this.isDead)
          input_y = 0;
        this._factor = Mathf.InverseLerp(0.0f, (double) this.speed >= 0.0 ? this.asset.speedMax : this.asset.speedMin, this.speed);
        bool flag = false;
        for (int index = 0; index < this.tires.Length; ++index)
        {
          this.tires[index].simulate((float) input_x, (float) input_y, inputBrake, delta);
          if (this.tires[index].isGrounded)
            flag = true;
        }
        if (flag)
          this.GetComponent<Rigidbody>().AddForce(-this.transform.up * this.factor * 40f);
        if (!PlayerUI.window.showCursor)
        {
          if (inputSirens != this.lastSirens)
          {
            this.lastSirens = inputSirens;
            if (inputSirens && (Object) this.sirens != (Object) null)
              VehicleManager.sendVehicleSirens();
          }
          if (inputLights != this.lastLights)
          {
            this.lastLights = inputLights;
            if (inputLights && (Object) this.headlights != (Object) null)
              VehicleManager.sendVehicleHeadlights();
          }
          if (inputHorn != this.lastHorn)
          {
            this.lastHorn = inputHorn;
            if (inputHorn)
              VehicleManager.sendVehicleHorn();
          }
        }
        this._speed = this.transform.InverseTransformDirection(this.GetComponent<Rigidbody>().velocity).z;
        this._turn = input_x;
        if (simulation - this.lastBurn > 5U)
        {
          this.lastBurn = simulation;
          this.askBurn((ushort) 1);
        }
        if (Provider.isServer)
          this.GetComponent<Rigidbody>().MovePosition(LevelGround.checkSafe(this.transform.position));
        this.lastUpdatedPos = this.transform.position;
        if (this.nsb == null)
          return;
        this.nsb.updateLastSnapshot(this.transform.position, this.transform.rotation);
      }
    }

    private void FixedUpdate()
    {
      if (this.isUnderwater)
      {
        if (!this.isDrowned)
        {
          this._lastUnderwater = Time.realtimeSinceStartup;
          this._isDrowned = true;
          this.tellSirens(false);
          this.tellHeadlights(false);
          this.updateFires();
          if (!Dedicator.isDedicated)
            this.GetComponent<AudioSource>().volume = 0.0f;
        }
      }
      else if (this._isDrowned)
      {
        this._isDrowned = false;
        this.updateFires();
      }
      if (this.isDriver)
      {
        this._speed = this.transform.InverseTransformDirection(this.GetComponent<Rigidbody>().velocity).z;
        this._slip = this.asset.hasTraction || (double) LevelLighting.snowLevel <= 0.00999999977648258 ? 0.0f : LevelLighting.windAudio.volume;
        for (int index = 0; index < this.tires.Length; ++index)
          this.tires[index].update(Time.fixedDeltaTime);
      }
      if (Provider.isServer)
      {
        if (!this.isDriven)
        {
          this._speed = this.transform.InverseTransformDirection(this.GetComponent<Rigidbody>().velocity).z;
          this._turn = 0;
          this.real = this.transform.position;
        }
        if (this.isDead && !this.isExploded && (!this.isUnderwater && (double) Time.realtimeSinceStartup - (double) this.lastDead > (double) InteractableVehicle.EXPLODE))
          this.explode();
      }
      if (Dedicator.isDedicated)
      {
        if ((double) Time.realtimeSinceStartup - (double) this.lastTick > (double) Provider.UPDATE_TIME)
        {
          this.lastTick = Time.realtimeSinceStartup;
          if ((double) Mathf.Abs(this.lastUpdatedPos.x - this.transform.position.x) > (double) Provider.UPDATE_DISTANCE || (double) Mathf.Abs(this.lastUpdatedPos.y - this.transform.position.y) > (double) Provider.UPDATE_DISTANCE || (double) Mathf.Abs(this.lastUpdatedPos.z - this.transform.position.z) > (double) Provider.UPDATE_DISTANCE)
          {
            this.lastUpdatedPos = this.transform.position;
            this.isUpdated = true;
            ++VehicleManager.updates;
          }
        }
      }
      else if (!Provider.isServer && !this.isPhysical && (double) Time.realtimeSinceStartup - (double) this.lastTick > (double) Provider.UPDATE_TIME * 2.0)
      {
        this.lastTick = Time.realtimeSinceStartup;
        this._speed = 0.0f;
        this._turn = 0;
      }
      if (!this.sirensOn || Dedicator.isDedicated || (double) Time.realtimeSinceStartup - (double) this.lastWeeoo <= 0.330000013113022)
        return;
      this.lastWeeoo = Time.realtimeSinceStartup;
      this.siren_0.gameObject.SetActive(!this.siren_0.gameObject.activeSelf);
      this.siren_1.gameObject.SetActive(!this.siren_0.gameObject.activeSelf);
    }

    private void Update()
    {
      if (!Dedicator.isDedicated)
      {
        this._steer = Mathf.Lerp(this.steer, (float) this.turn * this.asset.steerMax, 4f * Time.deltaTime);
        this._spedometer = Mathf.Lerp(this.spedometer, this.speed, 4f * Time.deltaTime);
        if (!this.isExploded)
        {
          this.spin += this.spedometer * 45f * Time.deltaTime;
          if (this.wheels != null)
          {
            for (int index = 0; index < this.wheels.Length; ++index)
              this.wheels[index].localRotation = index >= 2 || this.asset.hasCrawler ? Quaternion.Euler(this.spin, 0.0f, 0.0f) : Quaternion.Euler(this.spin, this.steer, 0.0f);
          }
          if ((Object) this.front != (Object) null)
          {
            this.front.localRotation = Quaternion.Euler(-90f, 180f, 0.0f);
            this.front.transform.Rotate(0.0f, 0.0f, this.steer, Space.Self);
          }
          this.wheel.transform.localRotation = this.rest;
          this.wheel.transform.Rotate(0.0f, -this.steer, 0.0f, Space.Self);
        }
        if (this.isDriven && !this.isUnderwater)
        {
          this.GetComponent<AudioSource>().pitch = Mathf.Lerp(this.GetComponent<AudioSource>().pitch, (float) (0.5 + (double) Mathf.Abs(this.spedometer) * 0.0599999986588955), 2f * Time.deltaTime);
          this.GetComponent<AudioSource>().volume = Mathf.Lerp(this.GetComponent<AudioSource>().volume, (int) this.fuel <= 0 ? 0.0f : 0.75f, 2f * Time.deltaTime);
        }
      }
      if (Provider.isServer || this.isPhysical || this.nsb == null)
        return;
      Vector3 pos;
      Quaternion rot;
      this.nsb.getCurrentSnapshot(out pos, out rot);
      this.GetComponent<Rigidbody>().MovePosition(pos);
      this.GetComponent<Rigidbody>().MoveRotation(rot);
    }

    public void init()
    {
      if (!Provider.isServer)
        this.nsb = new NetworkSnapshotBuffer(Provider.UPDATE_TIME, Provider.UPDATE_TIME * 2.33f);
      this._asset = (VehicleAsset) Assets.find(EAssetType.VEHICLE, this.id);
      if (Provider.isServer)
      {
        if ((int) this.fuel == (int) ushort.MaxValue)
          this.fuel = (ushort) Random.Range((int) this.asset.fuelMin, (int) this.asset.fuelMax);
        if ((int) this.health == (int) ushort.MaxValue)
          this.health = (ushort) Random.Range((int) this.asset.healthMin, (int) this.asset.healthMax);
      }
      if (!Dedicator.isDedicated)
      {
        this.fire = this.transform.FindChild("Fire");
        this.smoke_0 = this.transform.FindChild("Smoke_0");
        this.smoke_1 = this.transform.FindChild("Smoke_1");
        this._sirens = this.transform.FindChild("Sirens");
        if ((Object) this.sirens != (Object) null)
        {
          this.siren_0 = this.sirens.FindChild("Siren_0");
          this.siren_1 = this.sirens.FindChild("Siren_1");
        }
        this._headlights = this.transform.FindChild("Headlights");
        this._taillights = this.transform.FindChild("Taillights");
      }
      this._sirensOn = false;
      this._headlightsOn = false;
      this._taillightsOn = false;
      Transform child1 = this.transform.FindChild("Seats");
      this._passengers = new Passenger[child1.childCount];
      for (int index = 0; index < child1.childCount; ++index)
        this.passengers[index] = new Passenger(child1.FindChild("Seat_" + (object) index));
      Transform child2 = this.transform.FindChild("Tires");
      this.tires = new Wheel[child2.childCount];
      for (int index = 0; index < child2.childCount; ++index)
      {
        Wheel wheel = new Wheel(this, (WheelCollider) child2.FindChild("Tire_" + (object) index).GetComponent<Collider>(), index < 2, index >= child2.childCount - 2);
        wheel.reset();
        this.tires[index] = wheel;
      }
      if (!Dedicator.isDedicated)
      {
        Transform child3 = this.transform.FindChild("Wheels");
        if ((Object) child3 != (Object) null)
        {
          this.wheels = new Transform[child3.childCount];
          for (int index = 0; index < child3.childCount; ++index)
            this.wheels[index] = child3.FindChild("Wheel_" + (object) index);
        }
        else
          this.wheels = (Transform[]) null;
        this.wheel = this.transform.FindChild("Objects").FindChild("Steer");
        this.rest = this.wheel.localRotation;
        this.front = this.transform.FindChild("Objects").FindChild("Front");
        this.tellFuel(this.fuel);
        this.tellHealth(this.health);
      }
      if (this.isExploded)
        this.tellExploded();
      if (Provider.isServer)
        return;
      Object.Destroy((Object) this.transform.FindChild("Nav").gameObject);
      Object.Destroy((Object) this.transform.FindChild("Bumper").gameObject);
    }

    private void Awake()
    {
      if (Dedicator.isDedicated)
        return;
      this.sound = this.transform.FindChild("Sound").GetComponent<AudioSource>();
      this.GetComponent<AudioSource>().maxDistance *= 2f;
    }

    private void Start()
    {
      this.transform.FindChild("Bumper").gameObject.AddComponent<Bumper>().init(this);
      this.updateVehicle();
      this.updatePhysics();
      this.updateEngine();
    }
  }
}
