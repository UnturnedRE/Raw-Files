// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.PlayerStance
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
  public class PlayerStance : PlayerCaller
  {
    public static readonly float COOLDOWN = 0.75f;
    private static readonly float HEIGHT_STAND = 2f;
    private static readonly float HEIGHT_CROUCH = 1.2f;
    private static readonly float HEIGHT_PRONE = 0.6f;
    public static readonly float DETECT_EASY = 0.5f;
    public static readonly float DETECT_HARD = 1.25f;
    public static readonly float DETECT_MOVE = 1.1f;
    public static readonly float DETECT_FORWARD = 48f;
    public static readonly float DETECT_BACKWARD = 24f;
    public static readonly float DETECT_SPRINT = 20f;
    public static readonly float DETECT_STAND = 12f;
    public static readonly float DETECT_CROUCH = 5f;
    public static readonly float DETECT_PRONE = 2f;
    public StanceUpdated onStanceUpdated;
    private EPlayerStance changeStance;
    private EPlayerStance _stance;
    private float lastStance;
    private float lastDetect;
    private float lastHold;
    private bool isHolding;
    private bool flipCrouch;
    private bool lastCrouch;
    private bool _crouch;
    private bool flipProne;
    private bool lastProne;
    private bool _prone;
    private bool flipSprint;
    private bool lastSprint;
    private bool _sprint;
    private bool _isSubmerged;
    private RaycastHit ladder;

    public EPlayerStance stance
    {
      get
      {
        return this._stance;
      }
      set
      {
        this._stance = value;
        if (value != this.changeStance)
        {
          if (this.stance == EPlayerStance.STAND || this.stance == EPlayerStance.SPRINT || (this.stance == EPlayerStance.CLIMB || this.stance == EPlayerStance.SWIM) || (this.stance == EPlayerStance.DRIVING || this.stance == EPlayerStance.SITTING))
            this.player.movement.height = PlayerStance.HEIGHT_STAND;
          else if (this.stance == EPlayerStance.CROUCH)
            this.player.movement.height = PlayerStance.HEIGHT_CROUCH;
          else if (this.stance == EPlayerStance.PRONE)
            this.player.movement.height = PlayerStance.HEIGHT_PRONE;
        }
        this.changeStance = this.stance;
      }
    }

    public float radius
    {
      get
      {
        if (this.stance == EPlayerStance.DRIVING)
        {
          if (this.player.movement.getVehicle().sirensOn)
            return PlayerStance.DETECT_FORWARD;
          if ((double) this.player.movement.getVehicle().speed > 0.0)
            return PlayerStance.DETECT_FORWARD * this.player.movement.getVehicle().speed / this.player.movement.getVehicle().asset.speedMax;
          return PlayerStance.DETECT_BACKWARD * this.player.movement.getVehicle().speed / this.player.movement.getVehicle().asset.speedMin;
        }
        if (this.stance == EPlayerStance.SITTING)
          return 0.0f;
        if (this.stance == EPlayerStance.SPRINT)
          return (float) ((double) PlayerStance.DETECT_SPRINT * (!this.player.movement.isMoving ? 1.0 : (double) PlayerStance.DETECT_MOVE) * (Provider.mode != EGameMode.EASY ? (Provider.mode != EGameMode.HARD ? 1.0 : (double) PlayerStance.DETECT_HARD) : (double) PlayerStance.DETECT_EASY));
        if (this.stance == EPlayerStance.STAND)
        {
          float num = (float) (1.0 - (double) this.player.skills.mastery(1, 0) * 0.5);
          return (float) ((double) PlayerStance.DETECT_STAND * (!this.player.movement.isMoving ? 1.0 : (double) PlayerStance.DETECT_MOVE) * (Provider.mode != EGameMode.EASY ? (Provider.mode != EGameMode.HARD ? 1.0 : (double) PlayerStance.DETECT_HARD) : (double) PlayerStance.DETECT_EASY)) * num;
        }
        float num1 = (float) (1.0 - (double) this.player.skills.mastery(1, 0) * 0.75);
        if (this.stance == EPlayerStance.CROUCH || this.stance == EPlayerStance.CLIMB || this.stance == EPlayerStance.SWIM)
          return (float) ((double) PlayerStance.DETECT_CROUCH * (!this.player.movement.isMoving ? 1.0 : (double) PlayerStance.DETECT_MOVE) * (Provider.mode != EGameMode.EASY ? (Provider.mode != EGameMode.HARD ? 1.0 : (double) PlayerStance.DETECT_HARD) : (double) PlayerStance.DETECT_EASY)) * num1;
        if (this.stance == EPlayerStance.PRONE)
          return (float) ((double) PlayerStance.DETECT_PRONE * (!this.player.movement.isMoving ? 1.0 : (double) PlayerStance.DETECT_MOVE) * (Provider.mode != EGameMode.EASY ? (Provider.mode != EGameMode.HARD ? 1.0 : (double) PlayerStance.DETECT_HARD) : (double) PlayerStance.DETECT_EASY)) * num1;
        return 0.0f;
      }
    }

    public bool crouch
    {
      get
      {
        return this._crouch;
      }
    }

    public bool prone
    {
      get
      {
        return this._prone;
      }
    }

    public bool sprint
    {
      get
      {
        return this._sprint;
      }
    }

    public bool isSubmerged
    {
      get
      {
        return this._isSubmerged;
      }
    }

    private bool checkSpace(float height)
    {
      return Physics.OverlapSphere(this.transform.position + Vector3.up * height, 0.3f, RayMasks.BLOCK_STANCE).Length == 0;
    }

    public void checkStance(EPlayerStance newStance)
    {
      if ((Object) this.player.movement.getVehicle() != (Object) null && newStance != EPlayerStance.DRIVING && newStance != EPlayerStance.SITTING || newStance == this.stance || (newStance == EPlayerStance.PRONE || newStance == EPlayerStance.CROUCH) && !this.player.movement.isGrounded)
        return;
      if (newStance == EPlayerStance.STAND)
      {
        if (this.stance == EPlayerStance.PRONE)
        {
          if ((double) Time.realtimeSinceStartup - (double) this.lastStance <= (double) PlayerStance.COOLDOWN)
            return;
          this.lastStance = Time.realtimeSinceStartup;
          if (!this.checkSpace(1.7f) || !this.checkSpace(1.1f) || !this.checkSpace(0.5f))
            return;
        }
        else if (this.stance == EPlayerStance.CROUCH)
        {
          if ((double) Time.realtimeSinceStartup - (double) this.lastStance <= (double) PlayerStance.COOLDOWN)
            return;
          this.lastStance = Time.realtimeSinceStartup;
          if (!this.checkSpace(1.7f) || !this.checkSpace(1.1f) || !this.checkSpace(0.5f))
            return;
        }
      }
      if (newStance == EPlayerStance.CROUCH && this.stance == EPlayerStance.PRONE)
      {
        if ((double) Time.realtimeSinceStartup - (double) this.lastStance <= (double) PlayerStance.COOLDOWN)
          return;
        this.lastStance = Time.realtimeSinceStartup;
        if (!this.checkSpace(1.1f) || !this.checkSpace(0.5f))
          return;
      }
      if (Provider.isServer && newStance != EPlayerStance.STAND && (newStance != EPlayerStance.SPRINT && newStance != EPlayerStance.CROUCH))
      {
        if (this.player.animator.gesture == EPlayerGesture.INVENTORY_START)
          this.player.animator.sendGesture(EPlayerGesture.INVENTORY_STOP, false);
        else if (this.player.animator.gesture == EPlayerGesture.SURRENDER_START)
          this.player.animator.sendGesture(EPlayerGesture.SURRENDER_STOP, true);
      }
      this.stance = newStance;
      if (this.onStanceUpdated != null)
        this.onStanceUpdated();
      if (!Provider.isServer)
        return;
      this.channel.send("tellStance", ESteamCall.NOT_OWNER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[1]
      {
        (object) (byte) this.stance
      });
    }

    [SteamCall]
    public void tellStance(CSteamID steamID, byte newStance)
    {
      if (!this.channel.checkServer(steamID))
        return;
      this.stance = (EPlayerStance) newStance;
    }

    [SteamCall]
    public void askStance(CSteamID steamID)
    {
      if (!Provider.isServer)
        return;
      this.channel.send("tellStance", steamID, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[1]
      {
        (object) (byte) this.stance
      });
    }

    public void simulate(uint simulation, bool inputCrouch, bool inputProne, bool inputSprint)
    {
      this._isSubmerged = (double) LevelLighting.seaLevel < 0.990000009536743 && (double) this.player.look.aim.position.y < (double) LevelLighting.seaLevel * (double) Level.TERRAIN;
      Physics.Raycast(this.transform.position + Vector3.up * 0.5f, this.transform.forward, out this.ladder, 0.5f, RayMasks.LADDER_INTERACT);
      if ((Object) this.ladder.transform != (Object) null && this.ladder.transform.tag == "Ladder")
      {
        if (this.stance == EPlayerStance.CLIMB)
          return;
        this.transform.position = this.ladder.point + this.transform.forward * -0.4f + Vector3.up * -0.5f;
        this.checkStance(EPlayerStance.CLIMB);
      }
      else
      {
        if (this.stance == EPlayerStance.CLIMB)
          this.checkStance(EPlayerStance.STAND);
        if ((double) LevelLighting.seaLevel < 0.990000009536743 && (double) this.transform.position.y < (double) LevelLighting.seaLevel * (double) Level.TERRAIN - 1.25)
        {
          if (this.stance == EPlayerStance.SWIM)
            return;
          this.checkStance(EPlayerStance.SWIM);
        }
        else if ((double) LevelLighting.seaLevel < 0.990000009536743 && (double) this.transform.position.y < (double) LevelLighting.seaLevel * (double) Level.TERRAIN)
        {
          if (this.stance == EPlayerStance.STAND)
            return;
          this.checkStance(EPlayerStance.STAND);
        }
        else
        {
          if (this.stance == EPlayerStance.SWIM)
            this.checkStance(EPlayerStance.STAND);
          if (this.stance != EPlayerStance.CLIMB && this.stance != EPlayerStance.SITTING && this.stance != EPlayerStance.DRIVING)
          {
            if (inputCrouch != this.lastCrouch)
            {
              this.lastCrouch = inputCrouch;
              if (inputCrouch)
                this.checkStance(EPlayerStance.CROUCH);
              else if (this.stance == EPlayerStance.CROUCH)
                this.checkStance(EPlayerStance.STAND);
            }
            if (inputProne != this.lastProne)
            {
              this.lastProne = inputProne;
              if (inputProne)
                this.checkStance(EPlayerStance.PRONE);
              else if (this.stance == EPlayerStance.PRONE)
                this.checkStance(EPlayerStance.STAND);
            }
            if (inputSprint != this.lastSprint)
            {
              this.lastSprint = inputSprint;
              if (inputSprint)
              {
                if (this.stance == EPlayerStance.STAND && !this.player.life.isBroken && ((int) this.player.life.stamina > 0 && (double) this.player.movement.multiplier > 0.9) && this.player.movement.isMoving)
                  this.checkStance(EPlayerStance.SPRINT);
              }
              else if (this.stance == EPlayerStance.SPRINT)
                this.checkStance(EPlayerStance.STAND);
            }
            if (this.stance != EPlayerStance.SPRINT || !this.player.life.isBroken && (int) this.player.life.stamina != 0 && ((double) this.player.movement.multiplier >= 0.9 && this.player.movement.isMoving))
              return;
            this.checkStance(EPlayerStance.STAND);
          }
          else
          {
            this.lastCrouch = false;
            this.lastProne = false;
            this.lastSprint = false;
          }
        }
      }
    }

    private void onLifeUpdated(bool isDead)
    {
      if (isDead)
        return;
      this.checkStance(EPlayerStance.STAND);
    }

    private void FixedUpdate()
    {
      if (this.channel.isOwner && !PlayerUI.window.showCursor)
      {
        if (Input.GetKey(ControlsSettings.stance))
        {
          if (this.isHolding)
          {
            if ((double) Time.realtimeSinceStartup - (double) this.lastHold > 0.330000013113022)
            {
              this._crouch = false;
              this._prone = true;
            }
          }
          else
          {
            this.isHolding = true;
            this.lastHold = Time.realtimeSinceStartup;
          }
        }
        else if (this.isHolding)
        {
          if ((double) Time.realtimeSinceStartup - (double) this.lastHold < 0.330000013113022)
          {
            if (this.crouch)
            {
              this._crouch = false;
              this._prone = false;
            }
            else
            {
              this._crouch = true;
              this._prone = false;
            }
          }
          this.isHolding = false;
        }
        if (ControlsSettings.crouching == EControlMode.TOGGLE)
        {
          if (Input.GetKey(ControlsSettings.crouch) != this.flipCrouch)
          {
            this.flipCrouch = Input.GetKey(ControlsSettings.crouch);
            if (this.flipCrouch)
              this._crouch = !this.crouch;
          }
        }
        else
        {
          this._crouch = Input.GetKey(ControlsSettings.crouch);
          this.flipCrouch = this.crouch;
        }
        if (ControlsSettings.proning == EControlMode.TOGGLE)
        {
          if (Input.GetKey(ControlsSettings.prone) != this.flipProne)
          {
            this.flipProne = Input.GetKey(ControlsSettings.prone);
            if (this.flipProne)
              this._prone = !this.prone;
          }
        }
        else
        {
          this._prone = Input.GetKey(ControlsSettings.prone);
          this.flipProne = this.prone;
        }
        if (ControlsSettings.sprinting == EControlMode.TOGGLE)
        {
          if (Input.GetKey(ControlsSettings.sprint) != this.flipSprint)
          {
            this.flipSprint = Input.GetKey(ControlsSettings.sprint);
            if (this.flipSprint)
              this._sprint = !this.sprint;
          }
        }
        else
        {
          this._sprint = Input.GetKey(ControlsSettings.sprint);
          this.flipSprint = this.sprint;
        }
        if ((this.stance == EPlayerStance.PRONE || this.stance == EPlayerStance.CROUCH) && Input.GetKey(ControlsSettings.jump))
        {
          this._crouch = false;
          this._prone = false;
        }
        if (this.stance == EPlayerStance.CLIMB || this.stance == EPlayerStance.SITTING || this.stance == EPlayerStance.DRIVING)
        {
          this._crouch = false;
          this._prone = false;
          this._sprint = false;
        }
        if (PlayerUI.window.showCursor)
          this._sprint = false;
      }
      if (!Provider.isServer || (double) Time.realtimeSinceStartup - (double) this.lastDetect <= 0.1)
        return;
      this.lastDetect = Time.realtimeSinceStartup;
      if (this.player.life.isDead)
        return;
      AlertTool.alert(this.player, this.transform.position, this.radius, this.stance != EPlayerStance.SPRINT && this.stance != EPlayerStance.DRIVING);
    }

    private void Start()
    {
      this._stance = EPlayerStance.STAND;
      if (this.channel.isOwner || Provider.isServer)
      {
        this.lastStance = float.MinValue;
        this.player.life.onLifeUpdated += new LifeUpdated(this.onLifeUpdated);
      }
      if (this.channel.isOwner || Provider.isServer)
        return;
      this.channel.send("askStance", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER);
    }
  }
}
