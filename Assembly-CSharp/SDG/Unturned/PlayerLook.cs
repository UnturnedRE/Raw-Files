// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.PlayerLook
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
  public class PlayerLook : PlayerCaller
  {
    private static readonly float HEIGHT_LOOK_SIT = 1.6f;
    private static readonly float HEIGHT_LOOK_STAND = 1.75f;
    private static readonly float HEIGHT_LOOK_CROUCH = 1.2f;
    private static readonly float HEIGHT_LOOK_PRONE = 0.35f;
    private static readonly float HEIGHT_CAMERA_SIT = 0.7f;
    private static readonly float HEIGHT_CAMERA_STAND = 1.05f;
    private static readonly float HEIGHT_CAMERA_CROUCH = 0.95f;
    private static readonly float HEIGHT_CAMERA_PRONE = 0.3f;
    private static readonly float MIN_ANGLE_SIT = 60f;
    private static readonly float MAX_ANGLE_SIT = 120f;
    private static readonly float MIN_ANGLE_CLIMB = 45f;
    private static readonly float MAX_ANGLE_CLIMB = 100f;
    private static readonly float MIN_ANGLE_SWIM = 45f;
    private static readonly float MAX_ANGLE_SWIM = 135f;
    private static readonly float MAX_ANGLE_STAND = 180f;
    private static readonly float MIN_ANGLE_CROUCH = 20f;
    private static readonly float MAX_ANGLE_CROUCH = 160f;
    private static readonly float MIN_ANGLE_PRONE = 60f;
    private static readonly float MAX_ANGLE_PRONE = 120f;
    private static readonly float MIN_ANGLE_STAND;
    public PerspectiveUpdated onPerspectiveUpdated;
    private Camera _characterCamera;
    private Camera _scopeCamera;
    private Transform _aim;
    private static float characterHeight;
    private static float _characterYaw;
    public static float characterYaw;
    private static float killcam;
    private int warp_x;
    private int warp_y;
    private float _pitch;
    private float _yaw;
    private float _orbitPitch;
    private float _orbitYaw;
    public Vector3 lockPosition;
    public Vector3 orbitPosition;
    public bool isOrbiting;
    public bool isTracking;
    public bool isLocking;
    public bool isFocusing;
    public bool isIgnoringInput;
    private byte angle;
    private byte rot;
    private float recoil_x;
    private float recoil_y;
    private float lastRecoil;
    private float lastTick;
    private byte lastAngle;
    private byte lastRot;
    private float dodge;
    private float fov;
    private float eyes;
    private RaycastHit hit;
    private Vector3 cam;
    public float sensitivity;
    private EPlayerPerspective _perspective;

    private float heightLook
    {
      get
      {
        if (this.player.stance.stance == EPlayerStance.DRIVING || this.player.stance.stance == EPlayerStance.SITTING)
          return PlayerLook.HEIGHT_LOOK_SIT;
        if (this.player.stance.stance == EPlayerStance.STAND || this.player.stance.stance == EPlayerStance.SPRINT || (this.player.stance.stance == EPlayerStance.CLIMB || this.player.stance.stance == EPlayerStance.SWIM) || (this.player.stance.stance == EPlayerStance.DRIVING || this.player.stance.stance == EPlayerStance.SITTING))
          return PlayerLook.HEIGHT_LOOK_STAND;
        if (this.player.stance.stance == EPlayerStance.CROUCH)
          return PlayerLook.HEIGHT_LOOK_CROUCH;
        if (this.player.stance.stance == EPlayerStance.PRONE)
          return PlayerLook.HEIGHT_LOOK_PRONE;
        return 0.0f;
      }
    }

    private float heightCamera
    {
      get
      {
        if (this.player.stance.stance == EPlayerStance.DRIVING || this.player.stance.stance == EPlayerStance.SITTING)
          return PlayerLook.HEIGHT_CAMERA_SIT;
        if (this.player.stance.stance == EPlayerStance.STAND || this.player.stance.stance == EPlayerStance.SPRINT || (this.player.stance.stance == EPlayerStance.CLIMB || this.player.stance.stance == EPlayerStance.SWIM) || (this.player.stance.stance == EPlayerStance.DRIVING || this.player.stance.stance == EPlayerStance.SITTING))
          return PlayerLook.HEIGHT_CAMERA_STAND;
        if (this.player.stance.stance == EPlayerStance.CROUCH)
          return PlayerLook.HEIGHT_CAMERA_CROUCH;
        if (this.player.stance.stance == EPlayerStance.PRONE)
          return PlayerLook.HEIGHT_CAMERA_PRONE;
        return 0.0f;
      }
    }

    public Camera characterCamera
    {
      get
      {
        return this._characterCamera;
      }
    }

    public Camera scopeCamera
    {
      get
      {
        return this._scopeCamera;
      }
    }

    public Transform aim
    {
      get
      {
        return this._aim;
      }
    }

    public float pitch
    {
      get
      {
        return this._pitch;
      }
    }

    public float yaw
    {
      get
      {
        return this._yaw;
      }
    }

    public float orbitPitch
    {
      get
      {
        return this._orbitPitch;
      }
    }

    public float orbitYaw
    {
      get
      {
        return this._orbitYaw;
      }
    }

    public bool isCam
    {
      get
      {
        if (!this.isOrbiting && !this.isTracking && !this.isLocking)
          return this.isFocusing;
        return true;
      }
    }

    public EPlayerPerspective perspective
    {
      get
      {
        return this._perspective;
      }
    }

    [SteamCall]
    public void tellLook(CSteamID steamID, byte newPitch, byte newYaw)
    {
      if (!this.channel.checkServer(steamID))
        return;
      this._pitch = (float) newPitch;
      this._yaw = (float) ((int) newYaw * 2);
    }

    [SteamCall]
    public void askLook(CSteamID steamID)
    {
      if (!Provider.isServer)
        return;
      this.channel.send("tellLook", steamID, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[2]
      {
        (object) this.angle,
        (object) this.rot
      });
    }

    public void updateScope(EGraphicQuality quality)
    {
      if (quality == EGraphicQuality.OFF)
        this.scopeCamera.targetTexture = (RenderTexture) null;
      else if (quality == EGraphicQuality.LOW)
        this.scopeCamera.targetTexture = (RenderTexture) Resources.Load("RenderTextures/Scope_Low");
      else if (quality == EGraphicQuality.MEDIUM)
        this.scopeCamera.targetTexture = (RenderTexture) Resources.Load("RenderTextures/Scope_Medium");
      else if (quality == EGraphicQuality.HIGH)
        this.scopeCamera.targetTexture = (RenderTexture) Resources.Load("RenderTextures/Scope_High");
      else if (quality == EGraphicQuality.ULTRA)
        this.scopeCamera.targetTexture = (RenderTexture) Resources.Load("RenderTextures/Scope_Ultra");
      if (this.player.equipment.asset == null || this.player.equipment.asset.type != EItemType.GUN)
        return;
      this.player.equipment.useable.updateState(this.player.equipment.state);
    }

    public void enableScope(float zoom)
    {
      this.scopeCamera.fieldOfView = zoom;
      this.scopeCamera.enabled = true;
    }

    public void disableScope()
    {
      this.scopeCamera.enabled = false;
    }

    public void enableZoom(float zoom)
    {
      this.fov = zoom;
    }

    public void disableZoom()
    {
      this.fov = 0.0f;
    }

    public void updateRot()
    {
      this.angle = (double) this.pitch >= 0.0 ? ((double) this.pitch <= 180.0 ? (byte) this.pitch : (byte) 180) : (byte) 0;
      this.rot = MeasurementTool.angleToByte(this.yaw);
    }

    public void updateLook()
    {
      this.sensitivity = 1f;
      this._pitch = 90f;
      this._yaw = this.transform.localRotation.eulerAngles.y;
      this.updateRot();
      if (!this.channel.isOwner || this.perspective != EPlayerPerspective.FIRST)
        return;
      Camera.main.transform.localRotation = Quaternion.Euler(this.pitch - 90f, 0.0f, 0.0f);
      Camera.main.transform.localPosition = Vector3.up * this.eyes;
    }

    public void recoil(float x, float y, float h, float v)
    {
      this._yaw += x;
      this._pitch -= y;
      this.recoil_x += x * h;
      this.recoil_y += y * v;
      if ((double) Time.realtimeSinceStartup - (double) this.lastRecoil < 0.2)
      {
        this.recoil_x *= 0.6f;
        this.recoil_y *= 0.6f;
      }
      this.lastRecoil = Time.realtimeSinceStartup;
    }

    public void simulate(float look_x, float look_y, float delta)
    {
      this._pitch = look_y;
      this._yaw = look_x;
      this.updateRot();
      this.checkPitch();
      if (this.player.stance.stance == EPlayerStance.DRIVING || this.player.stance.stance == EPlayerStance.SITTING)
        this.transform.localRotation = Quaternion.identity;
      else
        this.transform.localRotation = Quaternion.Euler(0.0f, this.yaw, 0.0f);
      this.updateAimPosition(delta);
      this.updateAimRotation(delta);
    }

    private void checkPitch()
    {
      if (this.player.stance.stance == EPlayerStance.DRIVING || this.player.stance.stance == EPlayerStance.SITTING)
      {
        if ((double) this.pitch < (double) PlayerLook.MIN_ANGLE_SIT)
        {
          this._pitch = PlayerLook.MIN_ANGLE_SIT;
        }
        else
        {
          if ((double) this.pitch <= (double) PlayerLook.MAX_ANGLE_SIT)
            return;
          this._pitch = PlayerLook.MAX_ANGLE_SIT;
        }
      }
      else if (this.player.stance.stance == EPlayerStance.STAND || this.player.stance.stance == EPlayerStance.SPRINT)
      {
        if ((double) this.pitch < (double) PlayerLook.MIN_ANGLE_STAND)
        {
          this._pitch = PlayerLook.MIN_ANGLE_STAND;
        }
        else
        {
          if ((double) this.pitch <= (double) PlayerLook.MAX_ANGLE_STAND)
            return;
          this._pitch = PlayerLook.MAX_ANGLE_STAND;
        }
      }
      else if (this.player.stance.stance == EPlayerStance.CLIMB)
      {
        if ((double) this.pitch < (double) PlayerLook.MIN_ANGLE_CLIMB)
        {
          this._pitch = PlayerLook.MIN_ANGLE_CLIMB;
        }
        else
        {
          if ((double) this.pitch <= (double) PlayerLook.MAX_ANGLE_CLIMB)
            return;
          this._pitch = PlayerLook.MAX_ANGLE_CLIMB;
        }
      }
      else if (this.player.stance.stance == EPlayerStance.SWIM)
      {
        if ((double) this.pitch < (double) PlayerLook.MIN_ANGLE_SWIM)
        {
          this._pitch = PlayerLook.MIN_ANGLE_SWIM;
        }
        else
        {
          if ((double) this.pitch <= (double) PlayerLook.MAX_ANGLE_SWIM)
            return;
          this._pitch = PlayerLook.MAX_ANGLE_SWIM;
        }
      }
      else if (this.player.stance.stance == EPlayerStance.CROUCH)
      {
        if ((double) this.pitch < (double) PlayerLook.MIN_ANGLE_CROUCH)
        {
          this._pitch = PlayerLook.MIN_ANGLE_CROUCH;
        }
        else
        {
          if ((double) this.pitch <= (double) PlayerLook.MAX_ANGLE_CROUCH)
            return;
          this._pitch = PlayerLook.MAX_ANGLE_CROUCH;
        }
      }
      else
      {
        if (this.player.stance.stance != EPlayerStance.PRONE)
          return;
        if ((double) this.pitch < (double) PlayerLook.MIN_ANGLE_PRONE)
        {
          this._pitch = PlayerLook.MIN_ANGLE_PRONE;
        }
        else
        {
          if ((double) this.pitch <= (double) PlayerLook.MAX_ANGLE_PRONE)
            return;
          this._pitch = PlayerLook.MAX_ANGLE_PRONE;
        }
      }
    }

    public void updateAimPosition(float delta)
    {
      this.aim.localPosition = Vector3.Lerp(this.aim.localPosition, Vector3.up * this.heightLook, 4f * delta);
    }

    public void updateAimRotation(float delta)
    {
      this.aim.parent.localRotation = this.player.stance.stance == EPlayerStance.SITTING || this.player.stance.stance == EPlayerStance.DRIVING ? Quaternion.Euler(0.0f, this.yaw, 0.0f) : Quaternion.Lerp(this.aim.parent.localRotation, Quaternion.Euler(0.0f, 0.0f, (float) this.player.animator.lean * HumanAnimator.LEAN), 4f * delta);
      this.aim.localRotation = Quaternion.Euler(this.pitch - 90f, 0.0f, 0.0f);
    }

    private void onDamaged(byte damage)
    {
      if ((int) damage > 25)
        damage = (byte) 25;
      if ((double) Random.value < 0.5)
        this.dodge -= (float) (2 * (int) damage) * (float) (1.0 - (double) this.player.skills.mastery(1, 3) * 0.75);
      else
        this.dodge += (float) (2 * (int) damage) * (float) (1.0 - (double) this.player.skills.mastery(1, 3) * 0.75);
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
      if (!isDead)
        return;
      PlayerLook.killcam = this.transform.rotation.eulerAngles.y;
    }

    private void onSeated(bool isDriver, bool inVehicle, bool wasVehicle)
    {
      if (wasVehicle)
        return;
      this._orbitPitch = 22.5f;
      this._orbitYaw = 0.0f;
    }

    private void FixedUpdate()
    {
      if (this.channel.isOwner || !Provider.isServer || (double) Time.realtimeSinceStartup - (double) this.lastTick <= (double) Provider.UPDATE_TIME)
        return;
      this.lastTick = Time.realtimeSinceStartup;
      if (Mathf.Abs((int) this.lastAngle - (int) this.angle) <= 5 && Mathf.Abs((int) this.lastRot - (int) this.rot) <= 5)
        return;
      this.lastAngle = this.angle;
      this.lastRot = this.rot;
      this.channel.send("tellLook", ESteamCall.NOT_OWNER, this.transform.position, EffectManager.LARGE, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[2]
      {
        (object) this.angle,
        (object) this.rot
      });
    }

    private void Update()
    {
      if (this.channel.isOwner)
      {
        if (this.channel.owner.isAdmin && this.perspective == EPlayerPerspective.THIRD && Input.GetKey(KeyCode.LeftShift))
        {
          if (Input.GetKeyDown(KeyCode.F1))
          {
            this.isOrbiting = !this.isOrbiting;
            if (this.isOrbiting && !this.isTracking && (!this.isLocking && !this.isFocusing))
              this.isTracking = true;
          }
          if (Input.GetKeyDown(KeyCode.F2))
          {
            this.isTracking = !this.isTracking;
            if (this.isTracking)
            {
              this.isLocking = false;
              this.isFocusing = false;
            }
          }
          if (Input.GetKeyDown(KeyCode.F3))
          {
            this.isLocking = !this.isLocking;
            if (this.isLocking)
            {
              this.isTracking = false;
              this.isFocusing = false;
              this.lockPosition = this.player.first.position;
            }
          }
          if (Input.GetKeyDown(KeyCode.F4))
          {
            this.isFocusing = !this.isFocusing;
            if (this.isFocusing)
            {
              this.isTracking = false;
              this.isLocking = false;
              this.lockPosition = this.player.first.position;
            }
          }
        }
        this.eyes = Mathf.Lerp(this.eyes, this.heightLook, 4f * Time.deltaTime);
        Camera main = Camera.main;
        if (!this.player.life.isDead && !PlayerUI.window.showCursor && (Input.GetKeyDown(ControlsSettings.perspective) && Provider.camera != ECameraMode.FIRST) && Provider.camera != ECameraMode.THIRD)
        {
          this._perspective = this.perspective != EPlayerPerspective.FIRST ? EPlayerPerspective.FIRST : EPlayerPerspective.THIRD;
          if (this.perspective == EPlayerPerspective.FIRST)
          {
            main.transform.parent = this.player.first;
            main.transform.localPosition = Vector3.up * this.eyes;
            this.isOrbiting = false;
            this.isTracking = false;
            this.isLocking = false;
            this.isFocusing = false;
          }
          else
            main.transform.parent = this.player.transform;
          main.GetComponent<Bloom>().enabled = this.perspective == EPlayerPerspective.THIRD && GraphicsSettings.bloom;
          this.player.animator.view.GetComponent<Bloom>().enabled = this.perspective == EPlayerPerspective.FIRST && GraphicsSettings.bloom;
          if (this.onPerspectiveUpdated != null)
            this.onPerspectiveUpdated(this.perspective);
        }
        main.fieldOfView = !this.isCam ? Mathf.Lerp(main.fieldOfView, (double) this.fov <= 1.0 ? OptionsSettings.view + (this.player.stance.stance != EPlayerStance.SPRINT ? 0.0f : 10f) : this.fov, 8f * Time.deltaTime) : OptionsSettings.view;
        if (!PlayerUI.window.showCursor && !this.isIgnoringInput)
        {
          if (this.isOrbiting)
          {
            this._orbitYaw += ControlsSettings.look * Input.GetAxis("mouse_x") * (float) this.warp_x;
            if (ControlsSettings.invert)
              this._orbitPitch += ControlsSettings.look * Input.GetAxis("mouse_y") * (float) this.warp_y;
            else
              this._orbitPitch -= ControlsSettings.look * Input.GetAxis("mouse_y") * (float) this.warp_y;
          }
          else
          {
            if ((Object) this.player.movement.getVehicle() != (Object) null && this.perspective == EPlayerPerspective.THIRD)
              this._orbitYaw += ControlsSettings.look * Input.GetAxis("mouse_x") * (float) this.warp_x;
            else
              this._yaw += ControlsSettings.look * (this.perspective != EPlayerPerspective.FIRST ? 1f : this.sensitivity) * Input.GetAxis("mouse_x") * (float) this.warp_x;
            if ((Object) this.player.movement.getVehicle() != (Object) null && this.perspective == EPlayerPerspective.THIRD)
            {
              if (ControlsSettings.invert)
                this._orbitPitch += ControlsSettings.look * Input.GetAxis("mouse_y") * (float) this.warp_y;
              else
                this._orbitPitch -= ControlsSettings.look * Input.GetAxis("mouse_y") * (float) this.warp_y;
            }
            else if (ControlsSettings.invert)
              this._pitch += ControlsSettings.look * (this.perspective != EPlayerPerspective.FIRST ? 1f : this.sensitivity) * Input.GetAxis("mouse_y") * (float) this.warp_y;
            else
              this._pitch -= ControlsSettings.look * (this.perspective != EPlayerPerspective.FIRST ? 1f : this.sensitivity) * Input.GetAxis("mouse_y") * (float) this.warp_y;
          }
        }
        this._yaw -= Mathf.Lerp(0.0f, this.recoil_x, 4f * Time.deltaTime);
        this._pitch += Mathf.Lerp(0.0f, this.recoil_y, 4f * Time.deltaTime);
        this.recoil_x = Mathf.Lerp(this.recoil_x, 0.0f, 4f * Time.deltaTime);
        this.recoil_y = Mathf.Lerp(this.recoil_y, 0.0f, 4f * Time.deltaTime);
        this.dodge = Mathf.LerpAngle(this.dodge, 0.0f, 4f * Time.deltaTime);
        this.checkPitch();
        if ((double) this.orbitPitch > 90.0)
          this._orbitPitch = 90f;
        else if ((double) this.orbitPitch < -90.0)
          this._orbitPitch = -90f;
        PlayerLook._characterYaw = Mathf.Lerp(PlayerLook._characterYaw, PlayerLook.characterYaw + 180f, 4f * Time.deltaTime);
        this.characterCamera.transform.rotation = Quaternion.Euler(20f, PlayerLook._characterYaw, 0.0f);
        this.characterCamera.transform.position = this.player.character.position - this.characterCamera.transform.forward * 3.5f + Vector3.up * PlayerLook.characterHeight;
        if (this.player.life.isDead)
        {
          PlayerLook.killcam += -16f * Time.deltaTime;
          main.transform.rotation = Quaternion.Lerp(main.transform.rotation, Quaternion.Euler(32f, PlayerLook.killcam, 0.0f), 2f * Time.deltaTime);
        }
        else
        {
          if ((this.player.stance.stance == EPlayerStance.DRIVING || this.player.stance.stance == EPlayerStance.SITTING) && this.perspective == EPlayerPerspective.THIRD)
            main.transform.localRotation = Quaternion.Euler(this.orbitPitch, this.orbitYaw, 0.0f);
          else if (this.player.stance.stance == EPlayerStance.DRIVING)
          {
            if ((double) this.yaw < -160.0)
              this._yaw = -160f;
            else if ((double) this.yaw > 160.0)
              this._yaw = 160f;
            main.transform.localRotation = Quaternion.Euler(this.pitch - 90f, this.yaw / 10f, 0.0f);
            main.transform.Rotate(this.transform.up, this.yaw, Space.World);
          }
          else if (this.player.stance.stance == EPlayerStance.SITTING)
          {
            if ((double) this.yaw < -90.0)
              this._yaw = -90f;
            else if ((double) this.yaw > 90.0)
              this._yaw = 90f;
            main.transform.localRotation = Quaternion.Euler(this.pitch - 90f, 0.0f, 0.0f);
            main.transform.Rotate(this.transform.up, this.yaw, Space.World);
          }
          else
          {
            if (this.perspective == EPlayerPerspective.FIRST)
              main.transform.localRotation = Quaternion.Euler(this.pitch - 90f, 0.0f, this.dodge);
            else
              main.transform.localRotation = Quaternion.Euler(this.pitch - 90f, this.player.animator.shoulder * -5f, 0.0f);
            this.transform.localRotation = Quaternion.Euler(0.0f, this.yaw, 0.0f);
          }
          if (this.isCam)
          {
            if (this.isFocusing)
              main.transform.rotation = Quaternion.LookRotation(this.player.first.position + Vector3.up - this.lockPosition + this.orbitPosition);
            else
              main.transform.rotation = Quaternion.Euler(this.orbitPitch, this.orbitYaw, 0.0f);
          }
        }
        if (this.player.life.isDead)
        {
          this.cam = main.transform.forward * -4f;
          Physics.Raycast(this.player.first.position + Vector3.up, this.cam, out this.hit, 4f, RayMasks.BLOCK_KILLCAM);
          this.cam = !((Object) this.hit.transform != (Object) null) ? this.player.first.position + Vector3.up + this.cam : this.hit.point + this.cam.normalized * -0.25f;
          main.transform.position = this.cam;
        }
        else
        {
          if (this.isCam)
          {
            if (this.isLocking || this.isFocusing)
              main.transform.position = this.lockPosition + this.orbitPosition;
            else if (this.isOrbiting || this.isTracking)
              main.transform.position = this.player.first.position + this.orbitPosition;
          }
          else if ((this.player.stance.stance == EPlayerStance.DRIVING || this.player.stance.stance == EPlayerStance.SITTING) && this.perspective == EPlayerPerspective.THIRD)
          {
            this.cam = main.transform.forward * (float) -(5.5 + (double) Mathf.Abs(this.player.movement.getVehicle().spedometer) * 0.100000001490116);
            Physics.Raycast(this.player.first.transform.position + Vector3.up * this.eyes, this.cam, out this.hit, (float) (5.5 + (double) Mathf.Abs(this.player.movement.getVehicle().spedometer) * 0.100000001490116), RayMasks.BLOCK_VEHICLECAM);
            this.cam = !((Object) this.hit.transform != (Object) null) ? this.player.first.transform.position + Vector3.up * this.eyes + this.cam : this.hit.point + this.cam.normalized * -0.25f;
            main.transform.position = this.cam;
          }
          else if (this.player.stance.stance == EPlayerStance.DRIVING)
          {
            if ((double) this.yaw > 0.0)
              main.transform.localPosition = Vector3.Lerp(main.transform.localPosition, Vector3.up * this.heightLook - Vector3.left * this.yaw / 360f + Vector3.up * this.yaw / 720f, 4f * Time.deltaTime);
            else
              main.transform.localPosition = Vector3.Lerp(main.transform.localPosition, Vector3.up * this.heightLook - Vector3.left * this.yaw / 240f + Vector3.up * this.yaw / 720f, 4f * Time.deltaTime);
          }
          else if (this.perspective == EPlayerPerspective.FIRST)
          {
            main.transform.localPosition = Vector3.up * this.eyes;
          }
          else
          {
            this.cam = main.transform.forward * -1.5f + main.transform.up * 0.25f + main.transform.right * this.player.animator.shoulder * 1f;
            Physics.Raycast(this.player.first.position + Vector3.up * this.eyes, this.cam, out this.hit, 2f, RayMasks.BLOCK_PLAYERCAM);
            this.cam = !((Object) this.hit.transform != (Object) null) ? this.player.first.position + Vector3.up * this.eyes + this.cam : this.hit.point + this.cam.normalized * -0.25f;
            main.transform.position = this.cam;
          }
          PlayerLook.characterHeight = Mathf.Lerp(PlayerLook.characterHeight, this.heightCamera, 4f * Time.deltaTime);
        }
        this.updateAimPosition(Time.deltaTime);
        this.updateAimRotation(Time.deltaTime);
      }
      else
      {
        if (Provider.isServer)
          return;
        if (this.player.stance.stance == EPlayerStance.DRIVING || this.player.stance.stance == EPlayerStance.SITTING)
        {
          this.transform.localRotation = Quaternion.identity;
        }
        else
        {
          if ((double) Mathf.Abs(this.yaw - this.transform.rotation.eulerAngles.y) <= 0.1)
            return;
          this.transform.localRotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(0.0f, this.yaw, 0.0f), Provider.LERP * Time.deltaTime);
        }
      }
    }

    private void Start()
    {
      this._aim = this.transform.FindChild("Aim").FindChild("Fire");
      this.updateLook();
      this.warp_x = 1;
      this.warp_y = 1;
      if (this.channel.isOwner)
      {
        if (Provider.camera == ECameraMode.THIRD)
        {
          this._perspective = EPlayerPerspective.THIRD;
          Camera.main.transform.parent = this.player.transform;
        }
        else
          this._perspective = EPlayerPerspective.FIRST;
        Camera.main.fieldOfView = OptionsSettings.view;
        PlayerLook.characterHeight = 0.0f;
        PlayerLook._characterYaw = 180f;
        PlayerLook.characterYaw = 0.0f;
        this.dodge = 0.0f;
        if ((Object) this.player.character != (Object) null)
          this._characterCamera = this.player.character.FindChild("Camera").GetComponent<Camera>();
        this._scopeCamera = Camera.main.transform.FindChild("Scope").GetComponent<Camera>();
        this.scopeCamera.layerCullDistances = Camera.main.layerCullDistances;
        this.scopeCamera.layerCullSpherical = Camera.main.layerCullSpherical;
        LevelLighting.updateLighting();
        this.player.life.onVisionUpdated += new VisionUpdated(this.onVisionUpdated);
        this.player.life.onLifeUpdated += new LifeUpdated(this.onLifeUpdated);
        this.player.life.onDamaged += new Damaged(this.onDamaged);
        this.player.movement.onSeated += new Seated(this.onSeated);
      }
      if (this.channel.isOwner || Provider.isServer)
        return;
      this.channel.send("askLook", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER);
    }
  }
}
