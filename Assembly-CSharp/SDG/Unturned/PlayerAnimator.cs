// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.PlayerAnimator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
  public class PlayerAnimator : PlayerCaller
  {
    private static readonly float BOB_SPRINT = 0.075f;
    private static readonly float BOB_STAND = 0.05f;
    private static readonly float BOB_CROUCH = 0.025f;
    private static readonly float BOB_PRONE = 0.0125f;
    private static readonly float TILT_SPRINT = 5f;
    private static readonly float TILT_STAND = 3f;
    private static readonly float TILT_CROUCH = 2f;
    private static readonly float TILT_PRONE = 1f;
    private static readonly float SPEED_SPRINT = 10f;
    private static readonly float SPEED_STAND = 8f;
    private static readonly float SPEED_CROUCH = 6f;
    private static readonly float SPEED_PRONE = 4f;
    private CharacterAnimator firstAnimator;
    private CharacterAnimator thirdAnimator;
    private HumanAnimator characterAnimator;
    private SkinnedMeshRenderer firstRenderer_0;
    private SkinnedMeshRenderer thirdRenderer_0;
    private SkinnedMeshRenderer thirdRenderer_1;
    private Transform _firstSkeleton;
    private Transform _thirdSkeleton;
    private Transform _view;
    public Vector3 viewOffset;
    private float _multiplier;
    public float multiplier;
    private Vector3 viewAdjust;
    private Vector3 viewShake;
    private Vector3 viewBob;
    private Vector3 viewPoint;
    private Vector3 viewTilt;
    private Vector3 lockPosition;
    private Quaternion lockRotation;
    private bool isLocked;
    private Vector3 _pos;
    private Vector3 _rot;
    private bool _leanLeft;
    private bool _leanRight;
    private float lastTwitch;
    private int lastLean;
    private int _lean;
    private float _shoulder;
    private bool _side;
    private EPlayerGesture _gesture;
    private RaycastHit wall;

    public Transform firstSkeleton
    {
      get
      {
        return this._firstSkeleton;
      }
    }

    public Transform thirdSkeleton
    {
      get
      {
        return this._thirdSkeleton;
      }
    }

    public Transform view
    {
      get
      {
        return this._view;
      }
    }

    public Vector3 pos
    {
      get
      {
        return this._pos;
      }
    }

    public Vector3 rot
    {
      get
      {
        return this._rot;
      }
    }

    public bool leanLeft
    {
      get
      {
        return this._leanLeft;
      }
    }

    public bool leanRight
    {
      get
      {
        return this._leanRight;
      }
    }

    public int lean
    {
      get
      {
        return this._lean;
      }
    }

    public float shoulder
    {
      get
      {
        return this._shoulder;
      }
    }

    public bool side
    {
      get
      {
        return this._side;
      }
    }

    public EPlayerGesture gesture
    {
      get
      {
        return this._gesture;
      }
    }

    public float bob
    {
      get
      {
        if (Player.player.stance.stance == EPlayerStance.SPRINT)
          return PlayerAnimator.BOB_SPRINT * this._multiplier;
        if (Player.player.stance.stance == EPlayerStance.STAND)
          return PlayerAnimator.BOB_STAND * this._multiplier;
        if (Player.player.stance.stance == EPlayerStance.CROUCH)
          return PlayerAnimator.BOB_CROUCH * this._multiplier;
        if (Player.player.stance.stance == EPlayerStance.PRONE)
          return PlayerAnimator.BOB_PRONE * this._multiplier;
        return 0.0f;
      }
    }

    public float tilt
    {
      get
      {
        if (Player.player.stance.stance == EPlayerStance.SPRINT)
          return PlayerAnimator.TILT_SPRINT * (float) (1.0 - (double) this._multiplier / 2.0);
        if (Player.player.stance.stance == EPlayerStance.STAND)
          return PlayerAnimator.TILT_STAND * (float) (1.0 - (double) this._multiplier / 2.0);
        if (Player.player.stance.stance == EPlayerStance.CROUCH)
          return PlayerAnimator.TILT_CROUCH * (float) (1.0 - (double) this._multiplier / 2.0);
        if (Player.player.stance.stance == EPlayerStance.PRONE)
          return PlayerAnimator.TILT_PRONE * (float) (1.0 - (double) this._multiplier / 2.0);
        return 0.0f;
      }
    }

    public float speed
    {
      get
      {
        if (Player.player.stance.stance == EPlayerStance.SPRINT)
          return PlayerAnimator.SPEED_SPRINT;
        if (Player.player.stance.stance == EPlayerStance.STAND)
          return PlayerAnimator.SPEED_STAND;
        if (Player.player.stance.stance == EPlayerStance.CROUCH)
          return PlayerAnimator.SPEED_CROUCH;
        if (Player.player.stance.stance == EPlayerStance.PRONE)
          return PlayerAnimator.SPEED_PRONE;
        return 0.0f;
      }
    }

    public void addAnimation(AnimationClip clip)
    {
      if ((Object) this.firstAnimator != (Object) null)
        this.firstAnimator.addAnimation(clip);
      if ((Object) this.thirdAnimator != (Object) null)
        this.thirdAnimator.addAnimation(clip);
      if (!((Object) this.characterAnimator != (Object) null))
        return;
      this.characterAnimator.addAnimation(clip);
    }

    public void removeAnimation(AnimationClip clip)
    {
      if ((Object) this.firstAnimator != (Object) null)
        this.firstAnimator.removeAnimation(clip);
      if ((Object) this.thirdAnimator != (Object) null)
        this.thirdAnimator.removeAnimation(clip);
      if (!((Object) this.characterAnimator != (Object) null))
        return;
      this.characterAnimator.removeAnimation(clip);
    }

    public void setAnimationSpeed(string name, float speed)
    {
      if ((Object) this.firstAnimator != (Object) null)
        this.firstAnimator.setAnimationSpeed(name, speed);
      if ((Object) this.thirdAnimator != (Object) null)
        this.thirdAnimator.setAnimationSpeed(name, speed);
      if (!((Object) this.characterAnimator != (Object) null))
        return;
      this.characterAnimator.setAnimationSpeed(name, speed);
    }

    public float getAnimationLength(string name)
    {
      if ((Object) this.firstAnimator != (Object) null)
        return this.firstAnimator.getAnimationLength(name);
      if ((Object) this.thirdAnimator != (Object) null)
        return this.thirdAnimator.getAnimationLength(name);
      return 0.0f;
    }

    public void getAnimationSample(string name, float point)
    {
      if (!((Object) this.firstAnimator != (Object) null))
        return;
      this.firstAnimator.getAnimationSample(name, point);
    }

    public bool checkExists(string name)
    {
      if ((Object) this.firstAnimator != (Object) null)
        return this.firstAnimator.checkExists(name);
      if ((Object) this.thirdAnimator != (Object) null)
        return this.thirdAnimator.checkExists(name);
      if ((Object) this.characterAnimator != (Object) null)
        return this.characterAnimator.checkExists(name);
      return false;
    }

    public void play(string name, bool smooth)
    {
      if (this.gesture != EPlayerGesture.NONE)
        this._gesture = EPlayerGesture.NONE;
      if ((Object) this.firstAnimator != (Object) null)
        this.firstAnimator.play(name, smooth);
      if ((Object) this.thirdAnimator != (Object) null)
        this.thirdAnimator.play(name, smooth);
      if (!((Object) this.characterAnimator != (Object) null))
        return;
      this.characterAnimator.play(name, smooth);
    }

    public void stop(string name)
    {
      if ((Object) this.firstAnimator != (Object) null)
        this.firstAnimator.stop(name);
      if ((Object) this.thirdAnimator != (Object) null)
        this.thirdAnimator.stop(name);
      if (!((Object) this.characterAnimator != (Object) null))
        return;
      this.characterAnimator.stop(name);
    }

    public void mixAnimation(string name, bool mixLeftShoulder, bool mixRightShoulder)
    {
      this.mixAnimation(name, mixLeftShoulder, mixRightShoulder, false);
    }

    public void mixAnimation(string name, bool mixLeftShoulder, bool mixRightShoulder, bool mixSkull)
    {
      if ((Object) this.firstAnimator != (Object) null)
        this.firstAnimator.mixAnimation(name, mixLeftShoulder, mixRightShoulder, mixSkull);
      if ((Object) this.thirdAnimator != (Object) null)
        this.thirdAnimator.mixAnimation(name, mixLeftShoulder, mixRightShoulder, mixSkull);
      if (!((Object) this.characterAnimator != (Object) null))
        return;
      this.characterAnimator.mixAnimation(name, mixLeftShoulder, mixRightShoulder, mixSkull);
    }

    public void shake(float shake_x, float shake_y, float shake_z)
    {
      this.viewShake.x += shake_x;
      this.viewShake.y += shake_y;
      this.viewShake.z += shake_z;
    }

    public void lockView()
    {
      this.lockPosition = this.view.localPosition;
      this.lockRotation = this.view.localRotation;
      this.view.localPosition = new Vector3(-0.45f, 0.0f, 0.0f);
      this.view.localRotation = Quaternion.Euler(0.0f, 0.0f, 90f);
    }

    public void unlockView()
    {
      this.view.localPosition = this.lockPosition;
      this.view.localRotation = this.lockRotation;
    }

    private void onLifeUpdated(bool isDead)
    {
      if (this.gesture != EPlayerGesture.NONE)
      {
        if (this.gesture == EPlayerGesture.INVENTORY_START)
          this.stop("Gesture_Inventory");
        else if (this.gesture == EPlayerGesture.SURRENDER_START)
          this.stop("Gesture_Surrender");
        this._gesture = EPlayerGesture.NONE;
      }
      if (this.channel.isOwner)
      {
        this.firstRenderer_0.enabled = !isDead && this.player.look.perspective == EPlayerPerspective.FIRST;
        this.firstSkeleton.gameObject.SetActive(!isDead && this.player.look.perspective == EPlayerPerspective.FIRST);
        this.thirdRenderer_0.enabled = !isDead && this.player.look.perspective == EPlayerPerspective.THIRD;
        this.thirdRenderer_1.enabled = !isDead && this.player.look.perspective == EPlayerPerspective.THIRD;
        this.thirdSkeleton.gameObject.SetActive(!isDead && this.player.look.perspective == EPlayerPerspective.THIRD);
      }
      else
      {
        if (!Dedicator.isDedicated && !this.isLocked)
        {
          if ((Object) this.thirdRenderer_0 != (Object) null)
            this.thirdRenderer_0.enabled = !isDead;
          if ((Object) this.thirdRenderer_1 != (Object) null)
            this.thirdRenderer_1.enabled = !isDead;
        }
        this.thirdSkeleton.gameObject.SetActive(!isDead);
      }
    }

    public void unlock()
    {
      this.isLocked = false;
      if (this.channel.isOwner || Dedicator.isDedicated || this.player.life.isDead)
        return;
      if ((Object) this.thirdRenderer_0 != (Object) null)
        this.thirdRenderer_0.enabled = true;
      if ((Object) this.thirdRenderer_1 != (Object) null)
        this.thirdRenderer_1.enabled = true;
      this.thirdSkeleton.gameObject.SetActive(true);
    }

    [SteamCall]
    public void tellLean(CSteamID steamID, byte newLean)
    {
      if (!this.channel.checkServer(steamID))
        return;
      this._lean = (int) newLean - 1;
    }

    [SteamCall]
    public void tellGesture(CSteamID steamID, byte id)
    {
      if (!this.channel.checkServer(steamID))
        return;
      EPlayerGesture eplayerGesture = (EPlayerGesture) id;
      if (eplayerGesture == EPlayerGesture.INVENTORY_START && this.gesture == EPlayerGesture.NONE)
      {
        this.play("Gesture_Inventory", true);
        this._gesture = EPlayerGesture.INVENTORY_START;
      }
      else if (eplayerGesture == EPlayerGesture.INVENTORY_STOP && this.gesture == EPlayerGesture.INVENTORY_START)
      {
        this.stop("Gesture_Inventory");
        this._gesture = EPlayerGesture.NONE;
      }
      else if (eplayerGesture == EPlayerGesture.PICKUP)
      {
        this.play("Gesture_Pickup", false);
        this._gesture = EPlayerGesture.NONE;
      }
      else if (eplayerGesture == EPlayerGesture.PUNCH_LEFT)
      {
        this.play("Punch_Left", false);
        this._gesture = EPlayerGesture.NONE;
        if (Dedicator.isDedicated)
          return;
        this.player.playSound((AudioClip) Resources.Load("Sounds/General/Punch"));
      }
      else if (eplayerGesture == EPlayerGesture.PUNCH_RIGHT)
      {
        this.play("Punch_Right", false);
        this._gesture = EPlayerGesture.NONE;
        if (Dedicator.isDedicated)
          return;
        this.player.playSound((AudioClip) Resources.Load("Sounds/General/Punch"));
      }
      else if (eplayerGesture == EPlayerGesture.SURRENDER_START && this.gesture == EPlayerGesture.NONE)
      {
        this.play("Gesture_Surrender", true);
        this._gesture = EPlayerGesture.SURRENDER_START;
      }
      else if (eplayerGesture == EPlayerGesture.SURRENDER_STOP && this.gesture == EPlayerGesture.SURRENDER_START)
      {
        this.stop("Gesture_Surrender");
        this._gesture = EPlayerGesture.NONE;
      }
      else if (eplayerGesture == EPlayerGesture.POINT && this.gesture == EPlayerGesture.NONE)
      {
        this.play("Gesture_Point", false);
        this._gesture = EPlayerGesture.NONE;
      }
      else
      {
        if (eplayerGesture != EPlayerGesture.WAVE || this.gesture != EPlayerGesture.NONE)
          return;
        this.play("Gesture_Wave", false);
        this._gesture = EPlayerGesture.NONE;
      }
    }

    [SteamCall]
    public void askGesture(CSteamID steamID, byte id)
    {
      if (!this.channel.checkOwner(steamID) || !Provider.isServer)
        return;
      EPlayerGesture gesture = (EPlayerGesture) id;
      if (gesture == EPlayerGesture.INVENTORY_STOP && this.player.inventory.isStoring)
      {
        this.player.inventory.isStoring = false;
        if ((Object) this.player.inventory.storage != (Object) null)
        {
          this.player.inventory.storage.isOpen = false;
          this.player.inventory.storage = (InteractableStorage) null;
        }
        this.player.inventory.updateItems(PlayerInventory.STORAGE, (Items) null);
      }
      if (this.player.equipment.isSelected || this.player.stance.stance == EPlayerStance.PRONE || (this.player.stance.stance == EPlayerStance.SWIM || this.player.stance.stance == EPlayerStance.DRIVING) || this.player.stance.stance == EPlayerStance.SITTING || gesture != EPlayerGesture.INVENTORY_START && gesture != EPlayerGesture.INVENTORY_STOP && (gesture != EPlayerGesture.SURRENDER_START && gesture != EPlayerGesture.SURRENDER_STOP) && (gesture != EPlayerGesture.POINT && gesture != EPlayerGesture.WAVE))
        return;
      this.sendGesture(gesture, gesture != EPlayerGesture.INVENTORY_START && gesture != EPlayerGesture.INVENTORY_STOP);
    }

    public void sendGesture(EPlayerGesture gesture, bool all)
    {
      if (!Dedicator.isDedicated && gesture == EPlayerGesture.INVENTORY_STOP && this.player.inventory.isStoring)
      {
        this.player.inventory.isStoring = false;
        if ((Object) this.player.inventory.storage != (Object) null)
        {
          this.player.inventory.storage.isOpen = false;
          this.player.inventory.storage = (InteractableStorage) null;
        }
        this.player.inventory.updateItems(PlayerInventory.STORAGE, (Items) null);
      }
      if (Provider.isServer)
      {
        if (all)
          this.channel.send("tellGesture", ESteamCall.ALL, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[1]
          {
            (object) (byte) gesture
          });
        else
          this.channel.send("tellGesture", ESteamCall.NOT_OWNER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[1]
          {
            (object) (byte) gesture
          });
      }
      else
      {
        if (gesture != EPlayerGesture.INVENTORY_STOP && (this.player.equipment.isSelected || this.player.stance.stance == EPlayerStance.PRONE || (this.player.stance.stance == EPlayerStance.SWIM || this.player.stance.stance == EPlayerStance.DRIVING) || this.player.stance.stance == EPlayerStance.SITTING))
          return;
        this.channel.send("askGesture", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[1]
        {
          (object) (byte) gesture
        });
      }
    }

    private void updateState(CharacterAnimator charAnim)
    {
      if (this.player.movement.isMoving)
      {
        if (this.player.stance.stance == EPlayerStance.CLIMB)
          charAnim.state("Move_Climb");
        else if (this.player.stance.stance == EPlayerStance.SWIM)
          charAnim.state("Move_Swim");
        else if (this.player.stance.stance == EPlayerStance.SPRINT)
          charAnim.state("Move_Run");
        else if (this.player.stance.stance == EPlayerStance.STAND)
          charAnim.state("Move_Walk");
        else if (this.player.stance.stance == EPlayerStance.CROUCH)
        {
          charAnim.state("Move_Crouch");
        }
        else
        {
          if (this.player.stance.stance != EPlayerStance.PRONE)
            return;
          charAnim.state("Move_Prone");
        }
      }
      else if (this.player.stance.stance == EPlayerStance.DRIVING)
      {
        if ((Object) this.player.movement.getVehicle() != (Object) null && this.player.movement.getVehicle().asset.hasZip)
          charAnim.state("Idle_Zip");
        else
          charAnim.state("Idle_Drive");
      }
      else if (this.player.stance.stance == EPlayerStance.SITTING)
        charAnim.state("Idle_Sit");
      else if (this.player.stance.stance == EPlayerStance.CLIMB)
        charAnim.state("Idle_Climb");
      else if (this.player.stance.stance == EPlayerStance.SWIM)
        charAnim.state("Idle_Swim");
      else if (this.player.stance.stance == EPlayerStance.STAND || this.player.stance.stance == EPlayerStance.SPRINT)
        charAnim.state("Idle_Stand");
      else if (this.player.stance.stance == EPlayerStance.CROUCH)
      {
        charAnim.state("Idle_Crouch");
      }
      else
      {
        if (this.player.stance.stance != EPlayerStance.PRONE)
          return;
        charAnim.state("Idle_Prone");
      }
    }

    private void updateHuman(HumanAnimator humanAnim)
    {
      humanAnim.lean = !this.player.channel.owner.hand ? (float) this.lean : (float) -this.lean;
      humanAnim.pitch = this.player.stance.stance == EPlayerStance.DRIVING || this.player.stance.stance == EPlayerStance.SITTING ? 90f : this.player.look.pitch;
      if (this.player.stance.stance == EPlayerStance.STAND || this.player.stance.stance == EPlayerStance.SPRINT || (this.player.stance.stance == EPlayerStance.CLIMB || this.player.stance.stance == EPlayerStance.SWIM))
        humanAnim.offset = 0.0f;
      else if (this.player.stance.stance == EPlayerStance.CROUCH)
        humanAnim.offset = 0.1f;
      else if (this.player.stance.stance == EPlayerStance.PRONE)
        humanAnim.offset = 0.2f;
      if (this.channel.isOwner || !Provider.isServer)
        return;
      humanAnim._lean = !this.player.channel.owner.hand ? (float) this.lean : (float) -this.lean;
      humanAnim._pitch = humanAnim.pitch - 90f;
      humanAnim._offset = humanAnim.offset;
    }

    private void onLanded(float fall)
    {
      if ((double) fall >= -1.0)
        return;
      if ((double) fall < -15.0)
        fall = -15f;
      this.viewTilt += fall * Vector3.left;
    }

    public void simulate(uint simulation, bool inputLeanLeft, bool inputLeanRight)
    {
      if (this.player.stance.stance != EPlayerStance.CLIMB && this.player.stance.stance != EPlayerStance.SWIM && (this.player.stance.stance != EPlayerStance.SPRINT && this.player.stance.stance != EPlayerStance.DRIVING) && this.player.stance.stance != EPlayerStance.SITTING)
      {
        if (inputLeanLeft)
        {
          Physics.Raycast(this.transform.position + Vector3.up, -this.transform.right, out this.wall, 1f, RayMasks.BLOCK_LEAN);
          this._lean = !((Object) this.wall.transform == (Object) null) ? 0 : 1;
        }
        else if (inputLeanRight)
        {
          Physics.Raycast(this.transform.position + Vector3.up, this.transform.right, out this.wall, 1f, RayMasks.BLOCK_LEAN);
          this._lean = !((Object) this.wall.transform == (Object) null) ? 0 : -1;
        }
        else
          this._lean = 0;
      }
      else
        this._lean = 0;
      if (this.lastLean == this.lean)
        return;
      this.lastLean = this.lean;
      if (!Provider.isServer)
        return;
      this.channel.send("tellLean", ESteamCall.NOT_OWNER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[1]
      {
        (object) (byte) (this.lean + 1)
      });
    }

    [SteamCall]
    public void askEmote(CSteamID steamID)
    {
      if (!Provider.isServer || this.gesture == EPlayerGesture.NONE)
        return;
      this.channel.send("tellGesture", steamID, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[1]
      {
        (object) (byte) this.gesture
      });
    }

    private void onPerspectiveUpdated(EPlayerPerspective newPerspective)
    {
      this.firstRenderer_0.enabled = newPerspective == EPlayerPerspective.FIRST;
      this.firstSkeleton.gameObject.SetActive(newPerspective == EPlayerPerspective.FIRST);
      this.thirdRenderer_0.enabled = newPerspective == EPlayerPerspective.THIRD;
      this.thirdRenderer_1.enabled = newPerspective == EPlayerPerspective.THIRD;
      this.thirdSkeleton.gameObject.SetActive(newPerspective == EPlayerPerspective.THIRD);
    }

    private void Update()
    {
      if (this.channel.isOwner)
      {
        if (!PlayerUI.window.showCursor)
        {
          if (Input.GetKeyDown(ControlsSettings.leanLeft))
          {
            this._side = true;
            this.lastTwitch = Time.realtimeSinceStartup;
          }
          if (Input.GetKeyDown(ControlsSettings.leanRight))
          {
            this._side = false;
            this.lastTwitch = Time.realtimeSinceStartup;
          }
          this._leanLeft = Input.GetKey(ControlsSettings.leanLeft) && (double) Time.realtimeSinceStartup - (double) this.lastTwitch > 0.0750000029802322;
          this._leanRight = Input.GetKey(ControlsSettings.leanRight) && (double) Time.realtimeSinceStartup - (double) this.lastTwitch > 0.0750000029802322;
        }
        else
        {
          this._leanLeft = false;
          this._leanRight = false;
        }
        if ((Object) this.firstAnimator != (Object) null)
        {
          if (this.firstAnimator.getAnimationPlaying())
            this.firstAnimator.state("Idle_Stand");
          else
            this.updateState(this.firstAnimator);
        }
        if ((Object) this.thirdAnimator != (Object) null)
        {
          this.updateState(this.thirdAnimator);
          this.updateHuman((HumanAnimator) this.thirdAnimator);
        }
        this._multiplier = Mathf.Lerp(this._multiplier, this.multiplier, 16f * Time.deltaTime);
        if (this.player.movement.isMoving)
        {
          this.viewBob.x = Mathf.Lerp(this.viewBob.x, Mathf.Sin(this.speed * Time.time) * this.bob, 16f * Time.deltaTime);
          this.viewBob.y = Mathf.Lerp(this.viewBob.y, Mathf.Abs(Mathf.Sin(this.speed * Time.time) * this.bob), 16f * Time.deltaTime);
        }
        else
        {
          this.viewBob.x = Mathf.Lerp(this.viewBob.x, 0.0f, 4f * Time.deltaTime);
          this.viewBob.y = Mathf.Lerp(this.viewBob.y, 0.0f, 4f * Time.deltaTime);
        }
        this.viewAdjust = Vector3.Lerp(this.viewAdjust, this.viewOffset - this.viewShake, 16f * Time.deltaTime);
        this.viewShake = Vector3.Lerp(this.viewShake, Vector3.zero, 4f * Time.deltaTime);
        this.viewTilt = Vector3.Lerp(this.viewTilt, Vector3.zero, 8f * Time.deltaTime);
        this._pos.x = -this.viewBob.y - this.viewAdjust.y;
        this._pos.y = this.viewBob.x + this.viewAdjust.x;
        this._pos.z = this.viewBob.z + this.viewAdjust.z;
        if (this.player.stance.stance == EPlayerStance.DRIVING)
        {
          this.viewPoint.x = Mathf.Lerp(this.viewPoint.x, (float) (-0.649999976158142 - (double) Mathf.Abs(this.player.look.yaw) / 90.0 * 0.25), 8f * Time.deltaTime);
          this.viewPoint.y = Mathf.Lerp(this.viewPoint.y, (float) ((!this.channel.owner.hand ? 1.0 : -1.0) * (double) this.player.movement.getVehicle().steer * -0.00999999977648258), 8f * Time.deltaTime);
          this.viewPoint.z = Mathf.Lerp(this.viewPoint.z, -0.25f, 8f * Time.deltaTime);
        }
        else
        {
          this.viewPoint.x = this.pos.x - 0.45f;
          this.viewPoint.y = this.pos.y;
          this.viewPoint.z = this.pos.z;
        }
        this.view.transform.localPosition = this.viewPoint;
        if (this.player.movement.isMoving)
        {
          this._rot.x = Mathf.Lerp(this.rot.x, (float) ((double) this.player.movement.move.z * (double) this.tilt * (double) this.multiplier + (!this.player.movement.isGrounded ? -5.0 : 0.0)), 4f * Time.deltaTime);
          this._rot.z = Mathf.Lerp(this.rot.z, this.player.movement.move.x * this.tilt, 4f * Time.deltaTime);
        }
        else
        {
          this._rot.x = Mathf.Lerp(this.rot.x, !this.player.movement.isGrounded ? -5f : 0.0f, 4f * Time.deltaTime);
          this._rot.z = Mathf.Lerp(this.rot.z, 0.0f, 4f * Time.deltaTime);
        }
        this._rot += this.viewTilt;
        if (this.player.stance.stance == EPlayerStance.DRIVING)
          this.view.transform.localRotation = Quaternion.Lerp(this.view.transform.localRotation, Quaternion.Euler((float) (-(double) this.player.look.yaw * 60.0) / Camera.main.fieldOfView, (float) (((double) this.player.look.pitch - 90.0) * 60.0) / Camera.main.fieldOfView, 90f + this.player.movement.getVehicle().steer), 8f * Time.deltaTime);
        else if (this.player.stance.stance == EPlayerStance.CLIMB)
          this.view.transform.localRotation = Quaternion.Lerp(this.view.transform.localRotation, Quaternion.Euler(0.0f, (float) (((double) this.player.look.pitch - 90.0) * 60.0) / Camera.main.fieldOfView, 90f), 8f * Time.deltaTime);
        else
          this.view.transform.localRotation = Quaternion.Euler(0.0f, -this.rot.x, this.rot.z + 90f);
        this.player.first.transform.localRotation = Quaternion.Lerp(this.player.first.transform.localRotation, Quaternion.Euler(0.0f, 0.0f, (float) this.lean * HumanAnimator.LEAN), 4f * Time.deltaTime);
        this._shoulder = Mathf.Lerp(this.shoulder, !this.side ? 1f : -1f, 8f * Time.deltaTime);
      }
      else if ((Object) this.thirdAnimator != (Object) null)
      {
        this.updateState(this.thirdAnimator);
        this.updateHuman((HumanAnimator) this.thirdAnimator);
      }
      if (!((Object) this.characterAnimator != (Object) null))
        return;
      this.updateState((CharacterAnimator) this.characterAnimator);
      this.updateHuman(this.characterAnimator);
    }

    private void Start()
    {
      this._gesture = EPlayerGesture.NONE;
      this.isLocked = true;
      if (this.channel.isOwner)
      {
        if ((Object) this.player.first != (Object) null)
        {
          this.firstAnimator = this.player.first.FindChild("Camera").FindChild("Viewmodel").GetComponent<CharacterAnimator>();
          this.firstAnimator.transform.localScale = new Vector3(!this.channel.owner.hand ? 1f : -1f, 1f, 1f);
          this.firstRenderer_0 = (SkinnedMeshRenderer) this.firstAnimator.transform.FindChild("Model_0").GetComponent<Renderer>();
          this._firstSkeleton = this.firstAnimator.transform.FindChild("Skeleton");
        }
        if ((Object) this.player.third != (Object) null)
        {
          this.thirdAnimator = this.player.third.GetComponent<CharacterAnimator>();
          this.thirdAnimator.transform.localScale = new Vector3(!this.channel.owner.hand ? 1f : -1f, 1f, 1f);
          this.thirdRenderer_0 = (SkinnedMeshRenderer) this.thirdAnimator.transform.FindChild("Model_0").GetComponent<Renderer>();
          this.thirdRenderer_1 = (SkinnedMeshRenderer) this.thirdAnimator.transform.FindChild("Model_1").GetComponent<Renderer>();
          this._thirdSkeleton = this.thirdAnimator.transform.FindChild("Skeleton");
          this.thirdSkeleton.FindChild("Spine").GetComponent<Collider>().enabled = false;
          this.thirdSkeleton.FindChild("Spine").FindChild("Skull").GetComponent<Collider>().enabled = false;
          this.thirdSkeleton.FindChild("Spine").FindChild("Left_Shoulder").FindChild("Left_Arm").GetComponent<Collider>().enabled = false;
          this.thirdSkeleton.FindChild("Spine").FindChild("Right_Shoulder").FindChild("Right_Arm").GetComponent<Collider>().enabled = false;
          this.thirdSkeleton.FindChild("Left_Hip").FindChild("Left_Leg").GetComponent<Collider>().enabled = false;
          this.thirdSkeleton.FindChild("Right_Hip").FindChild("Right_Leg").GetComponent<Collider>().enabled = false;
        }
        if (Provider.camera == ECameraMode.THIRD)
        {
          this.thirdRenderer_0.enabled = true;
          this.thirdRenderer_1.enabled = true;
          this.thirdSkeleton.gameObject.SetActive(true);
        }
        else
        {
          this.firstRenderer_0.enabled = true;
          this.firstSkeleton.gameObject.SetActive(true);
        }
        this._view = this.firstSkeleton.FindChild("Spine").FindChild("Skull").FindChild("ViewmodelCamera");
        this.viewOffset = Vector3.zero;
        this.viewShake = Vector3.zero;
        this.viewBob = Vector3.zero;
        this.viewPoint = Vector3.zero;
        this._multiplier = 1f;
        this.multiplier = 1f;
        if ((Object) this.player.character != (Object) null)
        {
          this.characterAnimator = this.player.character.GetComponent<HumanAnimator>();
          this.characterAnimator.transform.localScale = new Vector3(!this.channel.owner.hand ? 1f : -1f, 1f, 1f);
        }
        this.player.movement.onLanded += new Landed(this.onLanded);
        this._side = this.player.channel.owner.hand;
        this.player.look.onPerspectiveUpdated += new PerspectiveUpdated(this.onPerspectiveUpdated);
      }
      else if ((Object) this.player.third != (Object) null)
      {
        this.thirdAnimator = this.player.third.GetComponent<CharacterAnimator>();
        this.thirdAnimator.transform.localScale = new Vector3(!this.channel.owner.hand ? 1f : -1f, 1f, 1f);
        this.thirdRenderer_0 = (SkinnedMeshRenderer) this.thirdAnimator.transform.FindChild("Model_0").GetComponent<Renderer>();
        this.thirdRenderer_1 = (SkinnedMeshRenderer) this.thirdAnimator.transform.FindChild("Model_1").GetComponent<Renderer>();
        this._thirdSkeleton = this.thirdAnimator.transform.FindChild("Skeleton");
      }
      if (Dedicator.isDedicated)
        this.thirdSkeleton.gameObject.SetActive(true);
      this.mixAnimation("Gesture_Inventory", true, true, true);
      this.mixAnimation("Gesture_Pickup", false, true);
      this.mixAnimation("Punch_Left", true, false);
      this.mixAnimation("Punch_Right", false, true);
      this.mixAnimation("Gesture_Point", false, true);
      this.mixAnimation("Gesture_Surrender", true, true);
      this.mixAnimation("Gesture_Wave", true, true, true);
      this.player.life.onLifeUpdated += new LifeUpdated(this.onLifeUpdated);
      if (this.channel.isOwner || Provider.isServer)
        return;
      this.channel.send("askEmote", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER);
    }
  }
}
