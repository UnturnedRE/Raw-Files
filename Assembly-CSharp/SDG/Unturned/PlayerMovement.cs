// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.PlayerMovement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
  public class PlayerMovement : PlayerCaller
  {
    private static readonly float SPEED_CLIMB = 4.5f;
    private static readonly float SPEED_SWIM = 3f;
    private static readonly float SPEED_SPRINT = 7f;
    private static readonly float SPEED_STAND = 4.5f;
    private static readonly float SPEED_CROUCH = 2.5f;
    private static readonly float SPEED_PRONE = 1.5f;
    private static readonly float JUMP = 7f;
    private static readonly float SWIM = 3f;
    public Landed onLanded;
    public Seated onSeated;
    public VehicleUpdated onVehicleUpdated;
    public SafetyUpdated onSafetyUpdated;
    public PurchaseUpdated onPurchaseUpdated;
    public PlayerRegionUpdated onRegionUpdated;
    public PlayerBoundUpdated onBoundUpdated;
    private CapsuleCollider capsule;
    private CharacterController controller;
    public float _multiplier;
    public float multiplier;
    public float gravity;
    private float slope;
    private float fell;
    private bool lastGrounded;
    private float lastFootstep;
    private bool _isGrounded;
    private bool _isSafe;
    private PurchaseNode _purchaseNode;
    private EPhysicsMaterial material;
    private RaycastHit ground;
    private bool _isMoving;
    private Vector3 _move;
    private byte _region_x;
    private byte _region_y;
    private byte _bound;
    private byte _nav;
    private LoadedRegion[,] _loadedRegions;
    private LoadedBound[] _loadedBounds;
    private Vector3 direction;
    private float fall;
    private Vector3 _real;
    private Vector3 lastUpdatePos;
    private NetworkSnapshotBuffer nsb;
    private float lastTick;
    private byte _horizontal;
    private byte _vertical;
    private int warp_x;
    private int warp_y;
    private int input_x;
    private int input_y;
    private bool _jump;
    private bool isRecovering;
    private float lastRecover;
    private byte recovery;
    public bool isAllowed;
    private bool isSeating;
    private InteractableVehicle seatingVehicle;
    private byte seatingSeat;
    private Transform seatingTransform;
    private Vector3 seatingPosition;
    private byte seatingAngle;
    private InteractableVehicle vehicle;

    public bool isGrounded
    {
      get
      {
        return this._isGrounded;
      }
    }

    public bool isSafe
    {
      get
      {
        return this._isSafe;
      }
      set
      {
        this._isSafe = value;
      }
    }

    public PurchaseNode purchaseNode
    {
      get
      {
        return this._purchaseNode;
      }
      set
      {
        this._purchaseNode = value;
      }
    }

    public float height
    {
      get
      {
        if (this.channel.isOwner)
          return this.controller.height;
        if (Provider.isServer)
          return this.capsule.height;
        return 2f;
      }
      set
      {
        if (this.channel.isOwner)
        {
          this.controller.height = value;
          this.controller.center = new Vector3(0.0f, value / 2f, 0.0f);
          this.transform.localPosition += new Vector3(0.0f, 0.02f, 0.0f);
        }
        if (!Provider.isServer)
          return;
        this.capsule.height = value;
        this.capsule.center = new Vector3(0.0f, value / 2f, 0.0f);
      }
    }

    public bool isMoving
    {
      get
      {
        return this._isMoving;
      }
    }

    public float speed
    {
      get
      {
        if (this.player.stance.stance == EPlayerStance.SWIM)
          return PlayerMovement.SPEED_SWIM * (float) (1.0 + (double) this.player.skills.mastery(0, 5) * 0.330000013113022) * this._multiplier;
        float num = (float) (1.0 + (double) this.player.skills.mastery(0, 4) * 0.330000013113022);
        if (this.player.stance.stance == EPlayerStance.CLIMB)
          return PlayerMovement.SPEED_CLIMB * num * this._multiplier;
        if (this.player.stance.stance == EPlayerStance.SPRINT)
          return PlayerMovement.SPEED_SPRINT * num * this._multiplier;
        if (this.player.stance.stance == EPlayerStance.STAND)
          return PlayerMovement.SPEED_STAND * num * this._multiplier;
        if (this.player.stance.stance == EPlayerStance.CROUCH)
          return PlayerMovement.SPEED_CROUCH * num * this._multiplier;
        if (this.player.stance.stance == EPlayerStance.PRONE)
          return PlayerMovement.SPEED_PRONE * num * this._multiplier;
        return 0.0f;
      }
    }

    public Vector3 move
    {
      get
      {
        return this._move;
      }
    }

    public byte region_x
    {
      get
      {
        return this._region_x;
      }
    }

    public byte region_y
    {
      get
      {
        return this._region_y;
      }
    }

    public byte bound
    {
      get
      {
        return this._bound;
      }
    }

    public byte nav
    {
      get
      {
        return this._nav;
      }
    }

    public LoadedRegion[,] loadedRegions
    {
      get
      {
        return this._loadedRegions;
      }
    }

    public LoadedBound[] loadedBounds
    {
      get
      {
        return this._loadedBounds;
      }
    }

    public Vector3 real
    {
      get
      {
        return this._real;
      }
    }

    public byte horizontal
    {
      get
      {
        return this._horizontal;
      }
    }

    public byte vertical
    {
      get
      {
        return this._vertical;
      }
    }

    public bool jump
    {
      get
      {
        return this._jump;
      }
    }

    public InteractableVehicle getVehicle()
    {
      return this.vehicle;
    }

    private void updateVehicle()
    {
      InteractableVehicle interactableVehicle = this.vehicle;
      this.vehicle = this.seatingVehicle;
      bool isDriver = (Object) this.vehicle != (Object) null && (int) this.seatingSeat == 0;
      if ((Object) this.vehicle == (Object) null)
      {
        this.player.transform.parent = this.seatingTransform;
        this.player.askTeleport(Provider.server, this.seatingPosition, this.seatingAngle);
      }
      if (this.channel.isOwner)
      {
        this.controller.enabled = (Object) this.vehicle == (Object) null;
        if (Provider.isServer)
          this.capsule.enabled = (Object) this.vehicle != (Object) null;
        bool has;
        if (isDriver && Provider.provider.achievementsService.getAchievement("Wheel", out has) && !has)
          Provider.provider.achievementsService.setAchievement("Wheel");
        if ((Object) this.vehicle != (Object) null)
        {
          PlayerUI.disableDot();
          if (this.player.equipment.asset != null && this.player.equipment.asset.type == EItemType.GUN)
          {
            if (this.player.look.perspective == EPlayerPerspective.THIRD)
              PlayerUI.disableCrosshair();
            else
              PlayerUI.enableCrosshair();
          }
        }
        else if (this.player.equipment.asset != null && this.player.equipment.asset.type == EItemType.GUN)
          PlayerUI.enableCrosshair();
        else
          PlayerUI.enableDot();
      }
      if (this.channel.isOwner || Provider.isServer)
      {
        if ((Object) this.vehicle != (Object) null)
        {
          if (isDriver)
            this.player.stance.checkStance(EPlayerStance.DRIVING);
          else
            this.player.stance.checkStance(EPlayerStance.SITTING);
        }
        else
          this.player.stance.checkStance(EPlayerStance.STAND);
      }
      if (this.channel.isOwner)
      {
        if (this.onSeated != null)
          this.onSeated(isDriver, (Object) this.vehicle != (Object) null, (Object) interactableVehicle != (Object) null);
        if (isDriver && this.onVehicleUpdated != null)
          this.onVehicleUpdated(!this.vehicle.isUnderwater && !this.vehicle.isDead, this.vehicle.fuel, this.vehicle.asset.fuel, this.vehicle.spedometer, this.vehicle.asset.speedMin, this.vehicle.asset.speedMax);
        if ((Object) this.vehicle != (Object) null)
        {
          if (isDriver)
          {
            if ((Object) interactableVehicle == (Object) null)
              PlayerUI.message(EPlayerMessage.VEHICLE_EXIT, string.Empty);
            else
              PlayerUI.message(EPlayerMessage.VEHICLE_SWAP, string.Empty);
          }
          else
            PlayerUI.message(EPlayerMessage.VEHICLE_SWAP, string.Empty);
        }
      }
      if (!((Object) this.vehicle != (Object) null))
        return;
      this.player.transform.parent = this.seatingTransform;
      this.player.transform.localPosition = this.seatingPosition;
      this.player.transform.localRotation = Quaternion.identity;
      this.player.look.updateLook();
    }

    public void setVehicle(InteractableVehicle newVehicle, byte newSeat, Transform newSeatingTransform, Vector3 newSeatingPosition, byte newSeatingAngle, bool forceUpdate)
    {
      this.isSeating = true;
      this.seatingVehicle = newVehicle;
      this.seatingSeat = newSeat;
      this.seatingTransform = newSeatingTransform;
      this.seatingPosition = newSeatingPosition;
      this.seatingAngle = newSeatingAngle;
      if ((this.channel.isOwner || Provider.isServer) && (!this.player.life.isDead && !forceUpdate))
        return;
      this.updateVehicle();
    }

    [SteamCall]
    public void tellPosition(CSteamID steamID, Vector3 newPosition)
    {
      if (!this.channel.checkServer(steamID) || this.player.stance.stance == EPlayerStance.DRIVING || this.player.stance.stance == EPlayerStance.SITTING)
        return;
      if (this.channel.isOwner)
      {
        this._real = newPosition;
        this.lastUpdatePos = newPosition;
        if (this.nsb != null)
          this.nsb.updateLastSnapshot(this.lastUpdatePos, Quaternion.identity);
        this.transform.localPosition = newPosition;
        this.recovery = (byte) 0;
      }
      else
      {
        this.checkGround(newPosition);
        this.lastUpdatePos = newPosition;
        if (this.nsb == null)
          return;
        this.nsb.addNewSnapshot(newPosition, Quaternion.identity);
      }
    }

    public void updateMovement()
    {
      this.lastUpdatePos = this.transform.localPosition;
      if (this.nsb != null)
        this.nsb.updateLastSnapshot(this.lastUpdatePos, Quaternion.identity);
      this._real = this.transform.position;
      if (!this.channel.isOwner && !Provider.isServer)
        return;
      this.isRecovering = false;
      this.lastRecover = Time.realtimeSinceStartup;
      this.recovery = (byte) 10;
    }

    private void checkGround(Vector3 position)
    {
      Physics.Raycast(position + Vector3.up, Vector3.down, out this.ground, 1.4f, RayMasks.BLOCK_COLLISION);
      this._isGrounded = (Object) this.ground.transform != (Object) null;
      if (!this.channel.isOwner || !this.controller.isGrounded)
        return;
      this._isGrounded = true;
    }

    private void onVisionUpdated(bool isViewing)
    {
      if (isViewing)
      {
        this.warp_x = (double) Random.value >= 0.25 ? 1 : -1;
        this.warp_y = (double) Random.value >= 0.25 ? 1 : -1;
      }
      else
      {
        this.warp_x = 1;
        this.warp_y = 1;
      }
    }

    private void onLifeUpdated(bool isDead)
    {
      byte seat;
      Vector3 point;
      byte angle;
      if (!isDead || !((Object) this.vehicle != (Object) null) || !this.vehicle.tryRemovePlayer(out seat, this.channel.owner.playerID.steamID, out point, out angle))
        return;
      VehicleManager.sendExitVehicle(this.vehicle, seat, point, angle, false);
    }

    public void simulate()
    {
      this.updateRegionAndBound();
      if (!this.isSeating)
        return;
      this.isSeating = false;
      this.updateVehicle();
    }

    public void simulate(uint simulation, Vector3 point, float angle_x, float angle_y, float angle_z, float speed, int turn, float delta)
    {
      this.updateRegionAndBound();
      if (this.isSeating)
      {
        this.isSeating = false;
        this.updateVehicle();
      }
      else
      {
        if (this.player.stance.stance != EPlayerStance.DRIVING)
          return;
        this.fell = this.transform.position.y;
        if (!((Object) this.vehicle != (Object) null))
          return;
        this.vehicle.simulate(simulation, point, Quaternion.Euler(angle_x, angle_y, angle_z), speed, turn, delta);
      }
    }

    public void simulate(uint simulation, bool inputJump, Vector3 point, float delta)
    {
      this.updateRegionAndBound();
      if (this.isSeating)
      {
        this.isSeating = false;
        this.updateVehicle();
      }
      else
      {
        if (this.isAllowed)
        {
          if ((double) (point - this.transform.position).sqrMagnitude > 1.0)
            return;
          this.isAllowed = false;
          this.fell = this.transform.position.y;
        }
        if (this.isRecovering)
        {
          if ((double) (point - this.real).sqrMagnitude > 1.0)
          {
            if ((double) Time.realtimeSinceStartup - (double) this.lastRecover <= 1.0)
              return;
            this.lastRecover = Time.realtimeSinceStartup;
            this.channel.send("tellPosition", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[1]
            {
              (object) this.real
            });
            return;
          }
          this.isRecovering = false;
          this.recovery = (byte) 0;
        }
        if ((int) this.recovery < 10)
          ++this.recovery;
        else if (this.player.stance.stance == EPlayerStance.SITTING)
        {
          this._isMoving = false;
          this.fell = this.transform.position.y;
        }
        else if (this.player.stance.stance == EPlayerStance.DRIVING)
        {
          this._isMoving = false;
        }
        else
        {
          this._isMoving = (double) (point - this.transform.position).sqrMagnitude > 0.100000001490116;
          this.checkGround(this.transform.position);
          if (this.player.stance.stance == EPlayerStance.CLIMB || this.player.stance.stance == EPlayerStance.SWIM)
            this.fell = this.transform.position.y;
          else if (this.lastGrounded != this.isGrounded)
          {
            this.lastGrounded = this.isGrounded;
            if (this.isGrounded && this.onLanded != null)
              this.onLanded(this.transform.position.y - this.fell);
            this.fell = this.transform.position.y;
          }
          if (inputJump && this.isGrounded && (!this.player.life.isBroken && (double) this.player.life.stamina >= 10.0 * (1.0 - (double) this.player.skills.mastery(0, 4) * 0.699999988079071)) && (this.player.stance.stance == EPlayerStance.STAND || this.player.stance.stance == EPlayerStance.SPRINT))
            this.player.life.askTire((byte) (10.0 * (1.0 - (double) this.player.skills.mastery(0, 4) * 0.699999988079071)));
          if ((double) Mathf.Pow(point.x - this.real.x, 2f) + (double) Mathf.Pow(point.z - this.real.z, 2f) > (double) Mathf.Pow(this.speed * delta, 2f) + 1.0)
          {
            this.isRecovering = true;
            this.lastRecover = Time.realtimeSinceStartup;
            this.channel.send("tellPosition", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[1]
            {
              (object) this.real
            });
          }
          else
          {
            if ((double) point.y < (double) this.real.y)
            {
              if ((double) point.y - (double) this.real.y < (double) Physics.gravity.y * (double) delta - 0.100000001490116)
              {
                this.isRecovering = true;
                this.lastRecover = Time.realtimeSinceStartup;
                this.channel.send("tellPosition", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[1]
                {
                  (object) this.real
                });
                return;
              }
            }
            else if ((double) point.y - (double) this.real.y > 9.0 * (double) delta + 0.100000001490116)
            {
              this.isRecovering = true;
              this.lastRecover = Time.realtimeSinceStartup;
              this.channel.send("tellPosition", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[1]
              {
                (object) this.real
              });
              return;
            }
            Vector3 vector3 = LevelGround.checkSafe(point);
            if ((double) (point - vector3).sqrMagnitude > 1.0)
            {
              this.isRecovering = true;
              this.lastRecover = Time.realtimeSinceStartup;
              this.channel.send("tellPosition", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[1]
              {
                (object) this.real
              });
            }
            else
            {
              this.GetComponent<Rigidbody>().MovePosition(point);
              this._real = point;
            }
          }
        }
      }
    }

    public void simulate(uint simulation, int input_x, int input_y, bool inputJump, Vector3 target, float delta)
    {
      this.updateRegionAndBound();
      if (this.isSeating)
      {
        this.isSeating = false;
        this.updateVehicle();
      }
      else
      {
        if (this.channel.isOwner)
          this.lastUpdatePos = this.transform.position;
        if (this.isAllowed)
        {
          this.isAllowed = false;
          this.fell = this.transform.position.y;
        }
        else if ((int) this.recovery < 10)
        {
          ++this.recovery;
        }
        else
        {
          if (this.player.look.isOrbiting)
          {
            input_x = 0;
            input_y = 0;
            inputJump = false;
          }
          this._move.x = (float) input_x;
          this._move.z = (float) input_y;
          if (this.player.stance.stance == EPlayerStance.SITTING)
          {
            this._isMoving = false;
            this.fell = this.transform.position.y;
          }
          else if (this.player.stance.stance == EPlayerStance.DRIVING)
          {
            this._isMoving = false;
            this.fell = this.transform.position.y;
            if (!this.channel.isOwner)
              return;
            this.vehicle.simulate(simulation, input_x, input_y, inputJump, Input.GetKey(ControlsSettings.primary), Input.GetKey(ControlsSettings.secondary), Input.GetKey(ControlsSettings.other), delta);
            if (this.onVehicleUpdated == null)
              return;
            this.onVehicleUpdated(!this.vehicle.isUnderwater && !this.vehicle.isDead, this.vehicle.fuel, this.vehicle.asset.fuel, this.vehicle.speed, this.vehicle.asset.speedMin, this.vehicle.asset.speedMax);
          }
          else
          {
            if (this.player.stance.stance == EPlayerStance.CLIMB)
            {
              this.fall = PlayerMovement.JUMP;
              this._isMoving = (double) Mathf.Abs(this.move.x) > 0.1 || (double) Mathf.Abs(this.move.z) > 0.1;
              this.fell = this.transform.position.y;
              this.direction = this.move.normalized * this.speed / 2f;
              int num = (int) this.controller.Move(Vector3.up * this.direction.z * delta);
            }
            else if (this.player.stance.stance == EPlayerStance.SWIM)
            {
              this._isMoving = (double) Mathf.Abs(this.move.x) > 0.1 || (double) Mathf.Abs(this.move.z) > 0.1;
              this.fell = this.transform.position.y;
              this.direction = this.move.normalized * this.speed * 1.5f;
              if (this.player.stance.isSubmerged || (double) this.player.look.pitch > 110.0 && (double) this.move.z > 0.1)
              {
                this.fall += (float) ((double) Physics.gravity.y * (double) delta / 7.0);
                if ((double) this.fall < (double) Physics.gravity.y / 7.0)
                  this.fall = Physics.gravity.y / 7f;
                if (inputJump)
                  this.fall = PlayerMovement.SWIM;
                int num = (int) this.controller.Move(this.player.look.aim.rotation * this.direction * delta + Vector3.up * this.fall * delta);
              }
              else
              {
                this.fall = (float) (((double) LevelLighting.seaLevel * (double) Level.TERRAIN - 1.25 - (double) this.transform.position.y) / 8.0);
                int num = (int) this.controller.Move(this.transform.rotation * this.direction * delta + Vector3.up * this.fall * delta);
              }
            }
            else
            {
              if (!Level.isLoading)
              {
                this.fall += (float) ((double) Physics.gravity.y * ((double) this.fall > 0.0 ? 1.0 : (double) this.gravity) * (double) delta * 3.0);
                if ((double) this.fall < (double) Physics.gravity.y * (double) this.gravity)
                  this.fall = Physics.gravity.y * this.gravity;
              }
              this._isMoving = (double) Mathf.Abs(this.move.x) > 0.1 || (double) Mathf.Abs(this.move.z) > 0.1;
              this.checkGround(this.transform.position);
              if (this.lastGrounded != this.isGrounded)
              {
                this.lastGrounded = this.isGrounded;
                if (this.isGrounded && this.onLanded != null)
                  this.onLanded(this.transform.position.y - this.fell);
                this.fell = this.transform.position.y;
              }
              if (inputJump && this.isGrounded && (!this.player.life.isBroken && (double) this.player.life.stamina >= 10.0 * (1.0 - (double) this.player.skills.mastery(0, 4) * 0.699999988079071)) && (this.player.stance.stance == EPlayerStance.STAND || this.player.stance.stance == EPlayerStance.SPRINT))
              {
                this.fall = PlayerMovement.JUMP * (float) (1.0 + (double) this.player.skills.mastery(0, 6) * 0.25);
                this.player.life.askTire((byte) (10.0 * (1.0 - (double) this.player.skills.mastery(0, 4) * 0.699999988079071)));
              }
              this.slope = !this.isGrounded || !((Object) this.ground.transform != (Object) null) ? Mathf.Lerp(this.slope, 1f, delta) : Mathf.Lerp(this.slope, this.ground.normal.y, delta);
              this._multiplier = Mathf.Lerp(this._multiplier, this.multiplier, delta);
              this.direction = this.material != EPhysicsMaterial.ICE_STATIC ? this.transform.rotation * this.move.normalized * this.speed * this.slope * delta : Vector3.Lerp(this.direction, this.transform.rotation * this.move.normalized * this.speed * this.slope * delta, delta);
              int num = (int) this.controller.Move(this.direction + Vector3.up * this.fall * delta);
            }
            if (!this.channel.isOwner || !Provider.isServer)
              return;
            this.transform.localPosition = LevelGround.checkSafe(this.transform.localPosition);
          }
        }
      }
    }

    private void onPerspectiveUpdated(EPlayerPerspective newPerspective)
    {
      if (!((Object) this.vehicle != (Object) null) || this.player.equipment.asset == null || this.player.equipment.asset.type != EItemType.GUN)
        return;
      if (newPerspective == EPlayerPerspective.THIRD)
        PlayerUI.disableCrosshair();
      else
        PlayerUI.enableCrosshair();
    }

    private void Update()
    {
      if (this.channel.isOwner)
      {
        if (this.player.look.isOrbiting)
        {
          this.player.look.orbitPosition += Camera.main.transform.right * (float) this.input_x * Time.deltaTime * (!Input.GetKey(ControlsSettings.modify) ? 4f : 16f);
          this.player.look.orbitPosition += Camera.main.transform.forward * (float) this.input_y * Time.deltaTime * (!Input.GetKey(ControlsSettings.modify) ? 4f : 16f);
        }
        if (this.player.stance.stance == EPlayerStance.DRIVING || this.player.stance.stance == EPlayerStance.SITTING)
        {
          this.player.first.localPosition = Vector3.zero;
          this.player.third.localPosition = Vector3.zero;
          this.fell = this.transform.position.y;
        }
        else
        {
          this.player.first.position = Vector3.Lerp(this.lastUpdatePos, this.transform.position, (Time.realtimeSinceStartup - this.player.input.tick) / PlayerInput.RATE);
          this.player.third.position = this.player.first.position;
        }
        this.player.look.aim.parent.transform.position = this.player.first.position;
        LevelLighting.updateLocal(Camera.main.transform.position);
      }
      else
      {
        if (Provider.isServer)
          return;
        if (this.player.stance.stance == EPlayerStance.SITTING || this.player.stance.stance == EPlayerStance.DRIVING)
        {
          this._isMoving = false;
          this.transform.localPosition = Vector3.zero;
        }
        else
        {
          this._isMoving = (double) Mathf.Abs(this.lastUpdatePos.x - this.transform.position.x) > 0.200000002980232 || (double) Mathf.Abs(this.lastUpdatePos.y - this.transform.position.y) > 0.200000002980232 || (double) Mathf.Abs(this.lastUpdatePos.z - this.transform.position.z) > 0.200000002980232;
          if (this.nsb == null)
            return;
          Vector3 pos;
          Quaternion rot;
          this.nsb.getCurrentSnapshot(out pos, out rot);
          this.transform.localPosition = pos;
        }
      }
    }

    private void updateRegionAndBound()
    {
      byte x;
      byte y;
      if (Regions.tryGetCoordinate(this.transform.position, out x, out y) && ((int) x != (int) this.region_x || (int) y != (int) this.region_y))
      {
        byte regionX = this.region_x;
        byte regionY = this.region_y;
        this._region_x = x;
        this._region_y = y;
        if (this.onRegionUpdated != null)
          this.onRegionUpdated(this.player, regionX, regionY, x, y);
      }
      byte bound1;
      LevelNavigation.tryGetBounds(this.transform.position, out bound1);
      if ((int) bound1 != (int) this.bound)
      {
        byte bound2 = this.bound;
        this._bound = bound1;
        if (this.onBoundUpdated != null)
          this.onBoundUpdated(this.player, bound2, bound1);
      }
      if (Provider.isServer)
        LevelNavigation.tryGetNavigation(this.transform.position, out this._nav);
      bool flag = false;
      PurchaseNode purchaseNode1 = (PurchaseNode) null;
      for (int index = 0; index < LevelNodes.nodes.Count; ++index)
      {
        Node node = LevelNodes.nodes[index];
        if (node.type == ENodeType.SAFEZONE)
        {
          SafezoneNode safezoneNode = (SafezoneNode) node;
          if ((double) (this.transform.position - safezoneNode.point).sqrMagnitude < (double) safezoneNode.radius)
            flag = true;
        }
        else if (node.type == ENodeType.PURCHASE)
        {
          PurchaseNode purchaseNode2 = (PurchaseNode) node;
          if ((double) (this.transform.position - purchaseNode2.point).sqrMagnitude < (double) purchaseNode2.radius)
            purchaseNode1 = purchaseNode2;
        }
        if (flag && purchaseNode1 != null)
          break;
      }
      if (flag != this.isSafe)
      {
        this._isSafe = flag;
        if (this.onSafetyUpdated != null)
          this.onSafetyUpdated(this.isSafe);
      }
      if (purchaseNode1 == this.purchaseNode)
        return;
      this._purchaseNode = purchaseNode1;
      if (this.onPurchaseUpdated == null)
        return;
      this.onPurchaseUpdated(this.purchaseNode);
    }

    private void FixedUpdate()
    {
      if (this.channel.isOwner)
      {
        if (!PlayerUI.window.showCursor && !LoadingUI.isBlocked)
        {
          this._jump = Input.GetKey(ControlsSettings.jump);
          this.input_x = !Input.GetKey(ControlsSettings.left) ? (!Input.GetKey(ControlsSettings.right) ? 0 : 1) : -1;
          this.input_y = !Input.GetKey(ControlsSettings.up) ? (!Input.GetKey(ControlsSettings.down) ? 0 : -1) : 1;
        }
        else
        {
          this._jump = false;
          this.input_x = 0;
          this.input_y = 0;
        }
        this.input_x *= this.warp_x;
        this.input_y *= this.warp_y;
        this._horizontal = (byte) (this.input_x + 1);
        this._vertical = (byte) (this.input_y + 1);
      }
      if (Dedicator.isDedicated)
      {
        if ((double) Time.realtimeSinceStartup - (double) this.lastTick <= (double) Provider.UPDATE_TIME)
          return;
        this.lastTick = Time.realtimeSinceStartup;
        if ((double) Mathf.Abs(this.lastUpdatePos.x - this.real.x) <= (double) Provider.UPDATE_DISTANCE && (double) Mathf.Abs(this.lastUpdatePos.y - this.real.y) <= (double) Provider.UPDATE_DISTANCE && (double) Mathf.Abs(this.lastUpdatePos.z - this.real.z) <= (double) Provider.UPDATE_DISTANCE)
          return;
        this.lastUpdatePos = this.real;
        this.channel.send("tellPosition", ESteamCall.NOT_OWNER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[1]
        {
          (object) this.real
        });
      }
      else
      {
        if ((double) Time.realtimeSinceStartup - (double) this.lastFootstep <= 1.75 / (double) this.speed)
          return;
        this.lastFootstep = Time.realtimeSinceStartup;
        this.material = EPhysicsMaterial.NONE;
        if (!this.channel.isOwner)
          this.checkGround(this.transform.position);
        if (this.player.stance.stance == EPlayerStance.CLIMB || this.player.stance.stance == EPlayerStance.SWIM)
          this._isGrounded = true;
        if (!this.isGrounded)
          return;
        if (this.player.stance.stance == EPlayerStance.CLIMB)
          this.material = EPhysicsMaterial.TILE_STATIC;
        else if (this.player.stance.stance == EPlayerStance.SWIM || (double) LevelLighting.seaLevel < 0.990000009536743 && (double) this.transform.position.y < (double) LevelLighting.seaLevel * (double) Level.TERRAIN)
          this.material = EPhysicsMaterial.WATER_STATIC;
        else if ((Object) this.ground.transform != (Object) null)
          this.material = !(this.ground.transform.tag == "Ground") ? PhysicsTool.checkMaterial(this.ground.collider) : PhysicsTool.checkMaterial(this.transform.position);
        if (!this.isMoving || this.player.stance.stance == EPlayerStance.PRONE || this.material == EPhysicsMaterial.NONE)
          return;
        if (this.material == EPhysicsMaterial.WATER_STATIC)
        {
          if (this.player.stance.stance == EPlayerStance.SWIM)
            this.player.playSound((AudioClip) Resources.Load("Sounds/Physics/Water/Footsteps/Swim"), 0.25f);
          else
            this.player.playSound((AudioClip) Resources.Load("Sounds/Physics/Water/Footsteps/Splash"), 0.3f);
        }
        else
        {
          float num = (float) (1.0 - (double) this.player.skills.mastery(1, 0) * 0.75);
          if ((double) num <= 0.00999999977648258)
            return;
          if (this.material == EPhysicsMaterial.CLOTH_STATIC)
            this.player.playSound((AudioClip) Resources.Load("Sounds/Physics/Tile/Footsteps/Tile_" + (object) Random.Range(0, 4)), 0.33f * num);
          else if (this.material == EPhysicsMaterial.TILE_STATIC)
            this.player.playSound((AudioClip) Resources.Load("Sounds/Physics/Tile/Footsteps/Tile_" + (object) Random.Range(0, 4)), 0.33f * num);
          else if (this.material == EPhysicsMaterial.CONCRETE_STATIC)
            this.player.playSound((AudioClip) Resources.Load("Sounds/Physics/Concrete/Footsteps/Concrete_" + (object) Random.Range(0, 4)), 0.33f * num);
          else if (this.material == EPhysicsMaterial.GRAVEL_STATIC)
            this.player.playSound((AudioClip) Resources.Load("Sounds/Physics/Gravel/Footsteps/Gravel_" + (object) Random.Range(0, 4)), 0.33f * num);
          else if (this.material == EPhysicsMaterial.METAL_STATIC)
            this.player.playSound((AudioClip) Resources.Load("Sounds/Physics/Metal/Footsteps/Metal_" + (object) Random.Range(0, 5)), 0.33f * num);
          else if (this.material == EPhysicsMaterial.WOOD_STATIC)
            this.player.playSound((AudioClip) Resources.Load("Sounds/Physics/Wood/Footsteps/Wood_" + (object) Random.Range(0, 11)), 0.33f * num);
          else if (this.material == EPhysicsMaterial.FOLIAGE_STATIC)
          {
            this.player.playSound((AudioClip) Resources.Load("Sounds/Physics/Foliage/Footsteps/Foliage_" + (object) Random.Range(0, 7)), 0.33f * num);
          }
          else
          {
            if (this.material != EPhysicsMaterial.SNOW_STATIC && this.material != EPhysicsMaterial.ICE_STATIC)
              return;
            this.player.playSound((AudioClip) Resources.Load("Sounds/Physics/Snow/Footsteps/Snow_" + (object) Random.Range(0, 7)), 0.33f * num);
          }
        }
      }
    }

    private void Start()
    {
      this._multiplier = 1f;
      this.multiplier = 1f;
      this.gravity = 1f;
      this.slope = 1f;
      this.updateMovement();
      this._region_x = byte.MaxValue;
      this._region_y = byte.MaxValue;
      this._bound = byte.MaxValue;
      this._nav = byte.MaxValue;
      if (this.channel.isOwner || Provider.isServer)
      {
        this._loadedRegions = new LoadedRegion[(int) Regions.WORLD_SIZE, (int) Regions.WORLD_SIZE];
        for (byte index1 = (byte) 0; (int) index1 < (int) Regions.WORLD_SIZE; ++index1)
        {
          for (byte index2 = (byte) 0; (int) index2 < (int) Regions.WORLD_SIZE; ++index2)
            this.loadedRegions[(int) index1, (int) index2] = new LoadedRegion();
        }
        this._loadedBounds = new LoadedBound[LevelNavigation.bounds.Count];
        for (byte index = (byte) 0; (int) index < LevelNavigation.bounds.Count; ++index)
          this.loadedBounds[(int) index] = new LoadedBound();
      }
      this.warp_x = 1;
      this.warp_y = 1;
      if (this.channel.isOwner)
      {
        this.controller = this.GetComponent<CharacterController>();
        this.player.look.onPerspectiveUpdated += new PerspectiveUpdated(this.onPerspectiveUpdated);
      }
      else
        Object.Destroy((Object) this.GetComponent<CharacterController>());
      if (Provider.isServer)
      {
        this.capsule = this.gameObject.AddComponent<CapsuleCollider>();
        this.capsule.isTrigger = true;
        this.capsule.center = new Vector3(0.0f, 1f, 0.0f);
        this.capsule.radius = 0.3f;
        this.capsule.height = 2f;
        this.capsule.enabled = !this.channel.isOwner;
        if (Dedicator.isDedicated)
        {
          this.gameObject.AddComponent<Rigidbody>();
          this.GetComponent<Rigidbody>().useGravity = false;
          this.GetComponent<Rigidbody>().isKinematic = true;
        }
        this.player.life.onVisionUpdated += new VisionUpdated(this.onVisionUpdated);
        this.player.life.onLifeUpdated += new LifeUpdated(this.onLifeUpdated);
      }
      else
        this.nsb = new NetworkSnapshotBuffer(Provider.UPDATE_TIME, Provider.UPDATE_TIME * 2.33f);
    }
  }
}
