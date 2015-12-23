// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.Zombie
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class Zombie : MonoBehaviour
  {
    public static readonly float MULTIPLIER_EASY = 0.75f;
    public static readonly float MULTIPLIER_HARD = 1.5f;
    private static readonly float ATTACK_BARRICADE = 16f;
    private static readonly float ATTACK_VEHICLE = 16f;
    private static readonly float ATTACK_PLAYER = 2f;
    private Transform skeleton;
    private Renderer renderer_0;
    private Renderer renderer_1;
    private Transform eyes;
    public ushort id;
    public byte bound;
    public byte type;
    public EZombieSpeciality speciality;
    public byte shirt;
    public byte pants;
    public byte hat;
    public byte gear;
    public byte move;
    public byte idle;
    public bool isUpdated;
    private AIPath seeker;
    private Player player;
    private Transform barricade;
    private Transform structure;
    private InteractableVehicle vehicle;
    private InteractableVehicle drive;
    private Transform target;
    private Animation animator;
    private float lastTick;
    private float lastHunted;
    private float lastTarget;
    private float lastLeave;
    private float lastAttack;
    private float lastStartle;
    private float lastStun;
    private float lastGroan;
    private float lastStuck;
    private float groanDelay;
    private float leaveTime;
    private float attackTime;
    private float startleTime;
    private float stunTime;
    private bool isPlayingAttack;
    private bool isPlayingStartle;
    private bool isPlayingStun;
    private Vector3 lastUpdatedPos;
    private NetworkSnapshotBuffer nsb;
    private bool isMoving;
    private bool isAttacking;
    private bool isHunting;
    private bool isLeaving;
    private bool isStunned;
    private bool isStuck;
    private Vector3 leaveTo;
    private float _lastDead;
    public bool isDead;
    private ushort health;
    private Vector3 ragdoll;
    private EZombiePath path;

    public float lastDead
    {
      get
      {
        return this._lastDead;
      }
    }

    private float attack
    {
      get
      {
        if ((Object) this.barricade != (Object) null)
          return Zombie.ATTACK_BARRICADE * (this.speciality != EZombieSpeciality.MEGA ? 1f : 2f);
        if ((Object) this.vehicle != (Object) null || (Object) this.drive != (Object) null)
          return Zombie.ATTACK_VEHICLE * (this.speciality != EZombieSpeciality.MEGA ? 1f : 2f);
        return (float) ((double) Zombie.ATTACK_PLAYER * (!Dedicator.isDedicated ? 1.0 : 0.5) * (this.speciality != EZombieSpeciality.MEGA ? 1.0 : 2.0));
      }
    }

    public void tellAlive(byte newType, byte newSpeciality, byte newShirt, byte newPants, byte newHat, byte newGear, Vector3 newPosition, byte newAngle)
    {
      this.type = newType;
      this.speciality = (EZombieSpeciality) newSpeciality;
      this.shirt = newShirt;
      this.pants = newPants;
      this.hat = newHat;
      this.gear = newGear;
      this.isDead = false;
      ++ZombieManager.regions[(int) this.bound].alive;
      this.transform.position = newPosition;
      this.transform.rotation = Quaternion.Euler(0.0f, (float) ((int) newAngle * 2), 0.0f);
      this.updateLife();
      this.apply();
      this.updateStates();
      if (!Provider.isServer)
        return;
      this.reset();
    }

    public void tellDead(Vector3 newRagdoll)
    {
      this.isDead = true;
      --ZombieManager.regions[(int) this.bound].alive;
      this._lastDead = Time.realtimeSinceStartup;
      this.updateLife();
      if (!Dedicator.isDedicated)
      {
        this.ragdoll = newRagdoll;
        RagdollTool.ragdollZombie(this.transform.position, this.transform.rotation, this.skeleton, this.ragdoll, this.type, this.shirt, this.pants, this.hat, this.gear);
      }
      if (!Provider.isServer)
        return;
      this.stop();
    }

    public void tellState(Vector3 newPosition, byte newAngle)
    {
      this.lastTick = Time.realtimeSinceStartup;
      this.lastUpdatedPos = newPosition;
      if (this.nsb == null)
        return;
      this.nsb.addNewSnapshot(newPosition, Quaternion.Euler(0.0f, (float) ((int) newAngle * 2), 0.0f));
    }

    public void askAttack(byte id)
    {
      if (this.isDead)
        return;
      this.lastAttack = Time.realtimeSinceStartup;
      this.isPlayingAttack = true;
      if (Dedicator.isDedicated)
        return;
      this.animator.Play("Attack_" + (object) id);
      if (this.speciality == EZombieSpeciality.MEGA)
        this.GetComponent<AudioSource>().pitch = Random.Range(0.5f, 0.7f);
      else
        this.GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
      if (LightingManager.isFullMoon)
        this.GetComponent<AudioSource>().pitch *= 0.9f;
      this.GetComponent<AudioSource>().PlayOneShot((AudioClip) Resources.Load("Sounds/Zombies/Roars/Roar_" + (object) Random.Range(0, 16)));
    }

    public void askStartle(byte id)
    {
      if (this.isDead)
        return;
      this.lastStartle = Time.realtimeSinceStartup;
      this.isPlayingStartle = true;
      if (Dedicator.isDedicated)
        return;
      this.animator.Play("Startle_" + (object) id);
      if (this.speciality == EZombieSpeciality.MEGA)
        this.GetComponent<AudioSource>().pitch = Random.Range(0.5f, 0.7f);
      else
        this.GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
      if (LightingManager.isFullMoon)
        this.GetComponent<AudioSource>().pitch *= 0.9f;
      this.GetComponent<AudioSource>().PlayOneShot((AudioClip) Resources.Load("Sounds/Zombies/Roars/Roar_" + (object) Random.Range(0, 16)));
    }

    public void askStun(byte id)
    {
      if (this.isDead)
        return;
      this.lastStun = Time.realtimeSinceStartup;
      this.isPlayingStun = true;
      if (Dedicator.isDedicated)
        return;
      this.animator.Play("Stun_" + (object) id);
    }

    public void askDamage(byte amount, Vector3 newRagdoll, out EPlayerKill kill)
    {
      kill = EPlayerKill.NONE;
      if ((int) amount == 0 || this.isDead || this.isDead)
        return;
      if ((int) amount >= (int) this.health)
        this.health = (ushort) 0;
      else
        this.health -= (ushort) amount;
      this.ragdoll = newRagdoll;
      if ((int) this.health == 0)
      {
        kill = this.speciality != EZombieSpeciality.MEGA ? EPlayerKill.ZOMBIE : EPlayerKill.MEGA;
        ZombieManager.dropLoot(this);
        ZombieManager.sendZombieDead(this, this.ragdoll);
      }
      else
      {
        if ((int) amount <= (this.speciality != EZombieSpeciality.MEGA ? 20 : 150))
          return;
        this.stun();
      }
    }

    public void sendRevive(byte type, byte speciality, byte shirt, byte pants, byte hat, byte gear, Vector3 position, float angle)
    {
      ZombieManager.sendZombieAlive(this, type, speciality, shirt, pants, hat, gear, position, MeasurementTool.angleToByte(angle));
    }

    public bool checkAlert(Player newPlayer)
    {
      return (Object) this.player != (Object) newPlayer;
    }

    public void alert(Player newPlayer)
    {
      if (this.isDead || (Object) this.player == (Object) newPlayer)
        return;
      if ((Object) this.player == (Object) null)
      {
        if (!this.isHunting && !this.isLeaving)
        {
          if (this.speciality == EZombieSpeciality.CRAWLER)
          {
            if ((double) Random.value < 0.5)
              ZombieManager.sendZombieStartle(this, (byte) 3);
            else
              ZombieManager.sendZombieStartle(this, (byte) 6);
          }
          else if (this.speciality == EZombieSpeciality.SPRINTER)
          {
            if ((double) Random.value < 0.5)
              ZombieManager.sendZombieStartle(this, (byte) 4);
            else
              ZombieManager.sendZombieStartle(this, (byte) 5);
          }
          else
            ZombieManager.sendZombieStartle(this, (byte) Random.Range(0, 3));
        }
        this.isHunting = true;
        this.isLeaving = false;
        this.isMoving = false;
        this.isStuck = false;
        this.lastHunted = Time.realtimeSinceStartup;
        this.lastStuck = Time.realtimeSinceStartup;
        this.player = newPlayer;
        this.target.position = this.player.transform.position;
        this.seeker.canSearch = true;
        this.seeker.canMove = true;
        this.path = this.speciality != EZombieSpeciality.MEGA ? (this.player.agro % 3 != 0 ? ((double) Random.value >= 0.5 ? EZombiePath.RIGHT : EZombiePath.LEFT) : EZombiePath.RUSH) : EZombiePath.RUSH;
        ++this.player.agro;
      }
      else
      {
        if ((double) (newPlayer.transform.position - this.transform.position).sqrMagnitude >= (double) (this.player.transform.position - this.transform.position).sqrMagnitude)
          return;
        --this.player.agro;
        this.player = newPlayer;
        this.target.position = this.player.transform.position;
        this.path = this.speciality != EZombieSpeciality.MEGA ? (this.player.agro % 3 != 0 ? ((double) Random.value >= 0.5 ? EZombiePath.RIGHT : EZombiePath.LEFT) : EZombiePath.RUSH) : EZombiePath.RUSH;
        ++this.player.agro;
      }
    }

    public void alert(Vector3 newPosition)
    {
      if (this.isDead || !((Object) this.player == (Object) null))
        return;
      if (!this.isHunting)
      {
        if (!this.isLeaving)
        {
          if (this.speciality == EZombieSpeciality.CRAWLER)
          {
            if ((double) Random.value < 0.5)
              ZombieManager.sendZombieStartle(this, (byte) 3);
            else
              ZombieManager.sendZombieStartle(this, (byte) 6);
          }
          else if (this.speciality == EZombieSpeciality.SPRINTER)
          {
            if ((double) Random.value < 0.5)
              ZombieManager.sendZombieStartle(this, (byte) 4);
            else
              ZombieManager.sendZombieStartle(this, (byte) 5);
          }
          else
            ZombieManager.sendZombieStartle(this, (byte) Random.Range(0, 3));
        }
        this.isHunting = true;
        this.isLeaving = false;
        this.isMoving = false;
        this.isStuck = false;
        this.lastHunted = Time.realtimeSinceStartup;
        this.lastStuck = Time.realtimeSinceStartup;
        this.target.position = newPosition;
        this.seeker.canSearch = true;
        this.seeker.canMove = true;
      }
      else
      {
        if ((double) (newPosition - this.transform.position).sqrMagnitude >= (double) (this.target.position - this.transform.position).sqrMagnitude)
          return;
        this.target.position = newPosition;
      }
    }

    public void updateStates()
    {
      this.lastUpdatedPos = this.transform.position;
      if (this.nsb == null)
        return;
      this.nsb.updateLastSnapshot(this.transform.position, this.transform.rotation);
    }

    private void stop()
    {
      this.isMoving = false;
      this.isAttacking = false;
      this.isHunting = false;
      this.isStuck = false;
      this.lastStuck = Time.realtimeSinceStartup;
      if ((Object) this.player != (Object) null)
        --this.player.agro;
      this.player = (Player) null;
      this.barricade = (Transform) null;
      this.structure = (Transform) null;
      this.vehicle = (InteractableVehicle) null;
      this.drive = (InteractableVehicle) null;
      this.seeker.canSearch = false;
      this.seeker.canMove = false;
      this.target.position = this.transform.position;
      this.seeker.stop();
    }

    private void stun()
    {
      this.isStunned = true;
      this.isMoving = false;
      this.seeker.canMove = false;
      if (this.speciality == EZombieSpeciality.CRAWLER)
      {
        float num = Random.value;
        if ((double) num < 0.330000013113022)
          ZombieManager.sendZombieStun(this, (byte) 5);
        else if ((double) num < 0.660000026226044)
          ZombieManager.sendZombieStun(this, (byte) 7);
        else
          ZombieManager.sendZombieStun(this, (byte) 8);
      }
      else if (this.speciality == EZombieSpeciality.SPRINTER)
      {
        float num = Random.value;
        if ((double) num < 0.330000013113022)
          ZombieManager.sendZombieStun(this, (byte) 6);
        else if ((double) num < 0.660000026226044)
          ZombieManager.sendZombieStun(this, (byte) 9);
        else
          ZombieManager.sendZombieStun(this, (byte) 10);
      }
      else
        ZombieManager.sendZombieStun(this, (byte) Random.Range(0, 5));
    }

    private void leave()
    {
      this.isLeaving = true;
      this.lastLeave = Time.realtimeSinceStartup;
      this.leaveTime = Random.Range(3f, 6f);
      this.leaveTo = this.transform.position - 16f * (this.target.position - this.transform.position).normalized + new Vector3(Random.Range(-8f, 8f), 0.0f, Random.Range(-8f, 8f));
      if (!LevelNavigation.checkNavigation(this.leaveTo))
        this.leaveTo = this.transform.position + 16f * (this.target.position - this.transform.position).normalized + new Vector3(Random.Range(-8f, 8f), 0.0f, Random.Range(-8f, 8f));
      if (!LevelNavigation.checkNavigation(this.leaveTo))
        this.leaveTo = this.transform.position;
      this.stop();
    }

    private void apply()
    {
      if (!Dedicator.isDedicated)
        ZombieClothing.apply(this.animator.transform, this.renderer_0, this.renderer_1, this.type, this.shirt, this.pants, this.hat, this.gear);
      if (this.speciality == EZombieSpeciality.MEGA)
      {
        if (!Dedicator.isDedicated)
        {
          this.GetComponent<AudioSource>().maxDistance = 64f;
          this.animator.transform.localScale = Vector3.one * Random.Range(1.45f, 1.55f);
        }
        if (!Provider.isServer)
          return;
        ((CharacterController) this.GetComponent<Collider>()).radius = 0.6f;
        this.seeker.speed = 6f;
      }
      else
      {
        if (!Dedicator.isDedicated)
        {
          this.GetComponent<AudioSource>().maxDistance = 32f;
          this.animator.transform.localScale = Vector3.one * Random.Range(0.95f, 1.05f);
        }
        if (!Provider.isServer)
          return;
        ((CharacterController) this.GetComponent<Collider>()).radius = 0.3f;
        if (this.speciality == EZombieSpeciality.CRAWLER)
          this.seeker.speed = 2.5f;
        else if (this.speciality == EZombieSpeciality.SPRINTER)
          this.seeker.speed = 6f;
        else
          this.seeker.speed = 4f;
      }
    }

    private void updateLife()
    {
      if (!Dedicator.isDedicated)
      {
        if ((Object) this.renderer_0 != (Object) null)
          this.renderer_0.enabled = !this.isDead;
        if ((Object) this.renderer_1 != (Object) null)
          this.renderer_1.enabled = !this.isDead;
        this.skeleton.gameObject.SetActive(!this.isDead);
        if ((Object) this.eyes != (Object) null)
          this.eyes.gameObject.SetActive(LightingManager.isFullMoon);
      }
      this.GetComponent<Collider>().enabled = !this.isDead;
    }

    private void reset()
    {
      this.target.position = this.transform.position;
      this.lastTick = Time.realtimeSinceStartup;
      this.lastTarget = Time.realtimeSinceStartup;
      this.lastLeave = Time.realtimeSinceStartup;
      this.lastAttack = Time.realtimeSinceStartup;
      this.lastStartle = Time.realtimeSinceStartup;
      this.lastStun = Time.realtimeSinceStartup;
      this.lastStuck = Time.realtimeSinceStartup;
      this.isPlayingAttack = false;
      this.isPlayingStartle = false;
      this.isPlayingStun = false;
      this.isMoving = false;
      this.isAttacking = false;
      this.isHunting = false;
      this.isLeaving = false;
      this.isStunned = false;
      this.isStuck = false;
      this.leaveTo = this.transform.position;
      if ((Object) this.player != (Object) null)
        --this.player.agro;
      this.player = (Player) null;
      this.barricade = (Transform) null;
      this.structure = (Transform) null;
      this.vehicle = (InteractableVehicle) null;
      this.drive = (InteractableVehicle) null;
      this.seeker.canSearch = false;
      this.seeker.canMove = false;
      this.health = LevelZombies.tables[(int) this.type].health;
      if (this.speciality == EZombieSpeciality.CRAWLER)
        this.health = (ushort) ((double) this.health * 1.5);
      else if (this.speciality == EZombieSpeciality.SPRINTER)
        this.health = (ushort) ((double) this.health * 0.5);
      if (Level.info.type != ELevelType.HORDE)
        return;
      this.health += (ushort) (Mathf.Min(ZombieManager.waveIndex - 1, 20) * 10);
    }

    private void Update()
    {
      if (this.isDead || Provider.isServer)
        return;
      this.isMoving = (double) Mathf.Abs(this.lastUpdatedPos.x - this.transform.position.x) > 0.200000002980232 || (double) Mathf.Abs(this.lastUpdatedPos.y - this.transform.position.y) > 0.200000002980232 || (double) Mathf.Abs(this.lastUpdatedPos.z - this.transform.position.z) > 0.200000002980232;
      if (this.nsb == null)
        return;
      Vector3 pos;
      Quaternion rot;
      this.nsb.getCurrentSnapshot(out pos, out rot);
      this.transform.position = pos;
      this.transform.rotation = rot;
    }

    private void FixedUpdate()
    {
      if (!this.isDead)
      {
        if (this.isPlayingAttack)
        {
          if ((double) Time.realtimeSinceStartup - (double) this.lastAttack > (double) this.attackTime)
            this.isPlayingAttack = false;
        }
        else if (this.isPlayingStartle)
        {
          if ((double) Time.realtimeSinceStartup - (double) this.lastStartle > (double) this.startleTime)
            this.isPlayingStartle = false;
        }
        else if (this.isPlayingStun)
        {
          if ((double) Time.realtimeSinceStartup - (double) this.lastStun > (double) this.stunTime)
            this.isPlayingStun = false;
        }
        else if (!Dedicator.isDedicated)
        {
          if (this.isMoving && (!Provider.isServer || !this.isStuck))
          {
            if (this.speciality == EZombieSpeciality.CRAWLER)
              this.animator.CrossFade("Move_4", CharacterAnimator.BLEND);
            else if (this.speciality == EZombieSpeciality.SPRINTER)
              this.animator.CrossFade("Move_5", CharacterAnimator.BLEND);
            else
              this.animator.CrossFade("Move_" + (object) this.move, CharacterAnimator.BLEND);
          }
          else if (this.speciality == EZombieSpeciality.CRAWLER)
            this.animator.CrossFade("Idle_3", CharacterAnimator.BLEND);
          else if (this.speciality == EZombieSpeciality.SPRINTER)
            this.animator.CrossFade("Idle_4", CharacterAnimator.BLEND);
          else
            this.animator.CrossFade("Idle_" + (object) this.idle, CharacterAnimator.BLEND);
        }
      }
      if (!Dedicator.isDedicated && (double) Time.realtimeSinceStartup - (double) this.lastGroan > (double) this.groanDelay)
      {
        this.lastGroan = Time.realtimeSinceStartup;
        this.groanDelay = this.speciality != EZombieSpeciality.MEGA ? Random.Range(4f, 8f) : Random.Range(2f, 4f);
        if (!this.isDead)
        {
          if (!this.isMoving)
          {
            if ((double) Random.value > 0.8)
            {
              if (this.speciality == EZombieSpeciality.MEGA)
                this.GetComponent<AudioSource>().pitch = Random.Range(0.5f, 0.7f);
              else
                this.GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
              if (LightingManager.isFullMoon)
                this.GetComponent<AudioSource>().pitch *= 0.9f;
              this.GetComponent<AudioSource>().PlayOneShot((AudioClip) Resources.Load("Sounds/Zombies/Groans/Groan_" + (object) Random.Range(0, 5)));
            }
          }
          else
          {
            if (this.speciality == EZombieSpeciality.MEGA)
              this.GetComponent<AudioSource>().pitch = Random.Range(0.5f, 0.7f);
            else
              this.GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
            if (LightingManager.isFullMoon)
              this.GetComponent<AudioSource>().pitch *= 0.9f;
            this.GetComponent<AudioSource>().PlayOneShot((AudioClip) Resources.Load("Sounds/Zombies/Roars/Roar_" + (object) Random.Range(0, 16)));
          }
        }
      }
      if (!Provider.isServer)
        return;
      if ((double) Time.realtimeSinceStartup - (double) this.lastTick > (double) Provider.UPDATE_TIME)
      {
        this.lastTick = Time.realtimeSinceStartup;
        if ((double) Mathf.Abs(this.lastUpdatedPos.x - this.transform.position.x) > (double) Provider.UPDATE_DISTANCE || (double) Mathf.Abs(this.lastUpdatedPos.y - this.transform.position.y) > (double) Provider.UPDATE_DISTANCE || (double) Mathf.Abs(this.lastUpdatedPos.z - this.transform.position.z) > (double) Provider.UPDATE_DISTANCE)
        {
          this.lastUpdatedPos = this.transform.position;
          if (Dedicator.isDedicated)
          {
            this.isUpdated = true;
            ++ZombieManager.regions[(int) this.bound].updates;
          }
          this.isStuck = false;
          this.lastStuck = Time.realtimeSinceStartup;
        }
        else if (this.isMoving)
          this.isStuck = true;
      }
      if (this.isDead)
        return;
      if (this.isStunned)
      {
        if ((double) Time.realtimeSinceStartup - (double) this.lastStun <= 1.0)
          return;
        this.lastTarget = Time.realtimeSinceStartup;
        this.lastStuck = Time.realtimeSinceStartup;
        this.isStunned = false;
        this.seeker.canMove = true;
      }
      if (this.isLeaving && (double) Time.realtimeSinceStartup - (double) this.lastLeave > (double) this.leaveTime)
      {
        this.alert(this.leaveTo);
        this.isLeaving = false;
      }
      if (!this.isHunting)
        return;
      if ((Object) this.player != (Object) null && ((int) this.player.movement.nav == (int) byte.MaxValue || this.player.life.isDead || this.player.stance.stance == EPlayerStance.SWIM))
      {
        this.leave();
      }
      else
      {
        if ((Object) this.vehicle != (Object) null && this.vehicle.isDead)
          this.vehicle = (InteractableVehicle) null;
        if ((Object) this.drive != (Object) null && this.drive.isDead)
          this.drive = (InteractableVehicle) null;
        if (this.isStuck && (double) Time.realtimeSinceStartup - (double) this.lastStuck > 1.0 && ((Object) this.barricade == (Object) null && (Object) this.structure == (Object) null) && ((Object) this.vehicle == (Object) null && (Object) this.drive == (Object) null))
        {
          Collider[] colliderArray = Physics.OverlapSphere(this.transform.position, 4f, RayMasks.DAMAGE_ZOMBIE);
          if (colliderArray.Length > 0)
          {
            if (colliderArray[0].transform.tag == "Structure")
              this.structure = colliderArray[0].transform;
            else if (colliderArray[0].transform.tag == "Vehicle")
            {
              this.vehicle = colliderArray[0].transform.GetComponent<InteractableVehicle>();
              if ((Object) this.vehicle != (Object) null && this.vehicle.isDead)
                this.vehicle = (InteractableVehicle) null;
            }
            else if (colliderArray[0].transform.tag == "Barricade")
              this.barricade = colliderArray[0].transform;
          }
        }
        float num1;
        if ((Object) this.barricade != (Object) null)
        {
          num1 = Mathf.Pow(this.barricade.position.x - this.transform.position.x, 2f) + Mathf.Pow(this.barricade.position.z - this.transform.position.z, 2f);
          this.target.position = this.barricade.position;
          this.seeker.canTurn = false;
          this.seeker.targetDirection = this.barricade.position - this.transform.position;
        }
        else if ((Object) this.structure != (Object) null)
        {
          num1 = 0.0f;
          this.target.position = this.transform.position;
          this.seeker.canTurn = false;
          this.seeker.targetDirection = this.structure.position - this.transform.position;
        }
        else if ((Object) this.vehicle != (Object) null)
        {
          num1 = Mathf.Pow(this.vehicle.transform.position.x - this.transform.position.x, 2f) + Mathf.Pow(this.vehicle.transform.position.z - this.transform.position.z, 2f);
          this.target.position = this.vehicle.transform.position;
          this.seeker.canTurn = false;
          this.seeker.targetDirection = this.vehicle.transform.position - this.transform.position;
        }
        else if ((Object) this.player != (Object) null)
        {
          this.drive = this.player.movement.getVehicle();
          if ((Object) this.drive != (Object) null && this.drive.isDead)
            this.drive = (InteractableVehicle) null;
          if ((Object) this.drive != (Object) null)
          {
            num1 = Mathf.Pow(this.drive.transform.position.x - this.transform.position.x, 2f) + Mathf.Pow(this.drive.transform.position.z - this.transform.position.z, 2f);
            this.target.position = this.drive.transform.position;
            this.seeker.canTurn = false;
            this.seeker.targetDirection = this.drive.transform.position - this.transform.position;
          }
          else
          {
            num1 = Mathf.Pow(this.player.transform.position.x - this.transform.position.x, 2f) + Mathf.Pow(this.player.transform.position.z - this.transform.position.z, 2f);
            this.target.position = this.player.transform.position;
            if ((double) num1 > 4.0)
            {
              this.seeker.canTurn = true;
              if (this.path == EZombiePath.LEFT)
                this.target.position -= this.transform.right;
              else if (this.path == EZombiePath.RIGHT)
                this.target.position += this.transform.right;
              else if (this.path == EZombiePath.RUSH)
                this.target.position -= this.transform.forward;
            }
            else
            {
              this.seeker.canTurn = false;
              this.seeker.targetDirection = this.player.transform.position - this.transform.position;
            }
            if (!Dedicator.isDedicated && this.speciality == EZombieSpeciality.SPRINTER)
              this.target.position -= this.transform.forward * 0.15f;
          }
        }
        else
        {
          num1 = Mathf.Pow(this.target.position.x - this.transform.position.x, 2f) + Mathf.Pow(this.target.position.z - this.transform.position.z, 2f);
          this.seeker.canTurn = true;
        }
        float num2 = Mathf.Abs(this.target.position.y - this.transform.position.y);
        this.isMoving = (double) num1 > 3.0;
        if ((double) num1 > 4096.0)
          this.leave();
        else if ((Object) this.player != (Object) null || (Object) this.barricade != (Object) null || ((Object) this.structure != (Object) null || (Object) this.vehicle != (Object) null) || (Object) this.drive != (Object) null)
        {
          if (((Object) this.structure != (Object) null || (double) num1 < (double) this.attack) && (double) num2 < 2.0)
          {
            if (this.speciality != EZombieSpeciality.SPRINTER && (double) Time.realtimeSinceStartup - (double) this.lastTarget <= (!Dedicator.isDedicated ? 0.100000001490116 : 0.300000011920929))
              return;
            if (this.isAttacking)
            {
              if ((double) Time.realtimeSinceStartup - (double) this.lastAttack <= (double) this.attackTime / 2.0)
                return;
              this.isAttacking = false;
              byte amount = (byte) ((double) (byte) ((double) LevelZombies.tables[(int) this.type].damage * (!LightingManager.isFullMoon ? 1.0 : 1.5)) * (Provider.mode != EGameMode.EASY ? (Provider.mode != EGameMode.HARD ? 1.0 : (double) Zombie.MULTIPLIER_HARD) : (double) Zombie.MULTIPLIER_EASY));
              if (this.speciality == EZombieSpeciality.CRAWLER)
                amount = (byte) ((double) amount * 2.0);
              else if (this.speciality == EZombieSpeciality.SPRINTER)
                amount = (byte) ((double) amount * 0.75);
              if ((Object) this.structure != (Object) null)
              {
                StructureManager.damage(this.structure, (this.target.position - this.transform.position).normalized * (float) amount, (float) amount, 1f);
                if (!((Object) this.structure == (Object) null) && !(this.structure.tag != "Structure"))
                  return;
                this.structure = (Transform) null;
                this.isStuck = false;
                this.lastStuck = Time.realtimeSinceStartup;
              }
              else if ((Object) this.barricade != (Object) null)
              {
                if (this.barricade.name == "Hinge")
                  BarricadeManager.damage(this.barricade.parent.parent, (float) amount, 1f);
                else
                  BarricadeManager.damage(this.barricade, (float) amount, 1f);
              }
              else if ((Object) this.vehicle != (Object) null)
                VehicleManager.damage(this.vehicle, (float) amount, 1f, true);
              else if ((Object) this.drive != (Object) null)
              {
                VehicleManager.damage(this.drive, (float) amount, 1f, true);
              }
              else
              {
                if (!((Object) this.player != (Object) null))
                  return;
                if (this.player.skills.boost == EPlayerBoost.HARDENED)
                  amount = (byte) ((double) amount * 0.75);
                if (this.speciality == EZombieSpeciality.MEGA)
                {
                  if ((int) this.player.clothing.hat != 0)
                  {
                    ItemClothingAsset itemClothingAsset = (ItemClothingAsset) Assets.find(EAssetType.ITEM, this.player.clothing.hat);
                    if (itemClothingAsset != null)
                    {
                      if (Provider.mode != EGameMode.EASY && (int) this.player.clothing.hatQuality > 0)
                      {
                        --this.player.clothing.hatQuality;
                        this.player.clothing.sendUpdateHatQuality();
                      }
                      float num3 = itemClothingAsset.armor + (float) ((1.0 - (double) itemClothingAsset.armor) * (1.0 - (double) this.player.clothing.hatQuality / 100.0));
                      amount = (byte) ((double) amount * (double) num3);
                    }
                  }
                  else if ((int) this.player.clothing.vest != 0)
                  {
                    ItemClothingAsset itemClothingAsset = (ItemClothingAsset) Assets.find(EAssetType.ITEM, this.player.clothing.vest);
                    if (itemClothingAsset != null)
                    {
                      if (Provider.mode != EGameMode.EASY && (int) this.player.clothing.vestQuality > 0)
                      {
                        --this.player.clothing.vestQuality;
                        this.player.clothing.sendUpdateVestQuality();
                      }
                      float num3 = itemClothingAsset.armor + (float) ((1.0 - (double) itemClothingAsset.armor) * (1.0 - (double) this.player.clothing.vestQuality / 100.0));
                      amount = (byte) ((double) amount * (double) num3);
                    }
                  }
                  else if ((int) this.player.clothing.shirt != 0)
                  {
                    ItemClothingAsset itemClothingAsset = (ItemClothingAsset) Assets.find(EAssetType.ITEM, this.player.clothing.shirt);
                    if (itemClothingAsset != null)
                    {
                      if (Provider.mode != EGameMode.EASY && (int) this.player.clothing.shirtQuality > 0)
                      {
                        --this.player.clothing.shirtQuality;
                        this.player.clothing.sendUpdateShirtQuality();
                      }
                      float num3 = itemClothingAsset.armor + (float) ((1.0 - (double) itemClothingAsset.armor) * (1.0 - (double) this.player.clothing.shirtQuality / 100.0));
                      amount = (byte) ((double) amount * (double) num3);
                    }
                  }
                }
                else if (this.speciality == EZombieSpeciality.NORMAL)
                {
                  if ((int) this.player.clothing.vest != 0)
                  {
                    ItemClothingAsset itemClothingAsset = (ItemClothingAsset) Assets.find(EAssetType.ITEM, this.player.clothing.vest);
                    if (itemClothingAsset != null)
                    {
                      if (Provider.mode != EGameMode.EASY && (int) this.player.clothing.vestQuality > 0)
                      {
                        --this.player.clothing.vestQuality;
                        this.player.clothing.sendUpdateVestQuality();
                      }
                      float num3 = itemClothingAsset.armor + (float) ((1.0 - (double) itemClothingAsset.armor) * (1.0 - (double) this.player.clothing.vestQuality / 100.0));
                      amount = (byte) ((double) amount * (double) num3);
                    }
                  }
                  else if ((int) this.player.clothing.shirt != 0)
                  {
                    ItemClothingAsset itemClothingAsset = (ItemClothingAsset) Assets.find(EAssetType.ITEM, this.player.clothing.shirt);
                    if (itemClothingAsset != null)
                    {
                      if (Provider.mode != EGameMode.EASY && (int) this.player.clothing.shirtQuality > 0)
                      {
                        --this.player.clothing.shirtQuality;
                        this.player.clothing.sendUpdateShirtQuality();
                      }
                      float num3 = itemClothingAsset.armor + (float) ((1.0 - (double) itemClothingAsset.armor) * (1.0 - (double) this.player.clothing.shirtQuality / 100.0));
                      amount = (byte) ((double) amount * (double) num3);
                    }
                  }
                }
                else if (this.speciality == EZombieSpeciality.CRAWLER)
                {
                  if ((int) this.player.clothing.pants != 0)
                  {
                    ItemClothingAsset itemClothingAsset = (ItemClothingAsset) Assets.find(EAssetType.ITEM, this.player.clothing.pants);
                    if (itemClothingAsset != null)
                    {
                      if (Provider.mode != EGameMode.EASY && (int) this.player.clothing.pantsQuality > 0)
                      {
                        --this.player.clothing.pantsQuality;
                        this.player.clothing.sendUpdatePantsQuality();
                      }
                      float num3 = itemClothingAsset.armor + (float) ((1.0 - (double) itemClothingAsset.armor) * (1.0 - (double) this.player.clothing.pantsQuality / 100.0));
                      amount = (byte) ((double) amount * (double) num3);
                    }
                  }
                }
                else if (this.speciality == EZombieSpeciality.SPRINTER)
                {
                  if ((int) this.player.clothing.vest != 0)
                  {
                    ItemClothingAsset itemClothingAsset = (ItemClothingAsset) Assets.find(EAssetType.ITEM, this.player.clothing.vest);
                    if (itemClothingAsset != null)
                    {
                      if (Provider.mode != EGameMode.EASY && (int) this.player.clothing.vestQuality > 0)
                      {
                        --this.player.clothing.vestQuality;
                        this.player.clothing.sendUpdateVestQuality();
                      }
                      float num3 = itemClothingAsset.armor + (float) ((1.0 - (double) itemClothingAsset.armor) * (1.0 - (double) this.player.clothing.vestQuality / 100.0));
                      amount = (byte) ((double) amount * (double) num3);
                    }
                  }
                  else if ((int) this.player.clothing.shirt != 0)
                  {
                    ItemClothingAsset itemClothingAsset = (ItemClothingAsset) Assets.find(EAssetType.ITEM, this.player.clothing.shirt);
                    if (itemClothingAsset != null)
                    {
                      if (Provider.mode != EGameMode.EASY && (int) this.player.clothing.shirtQuality > 0)
                      {
                        --this.player.clothing.shirtQuality;
                        this.player.clothing.sendUpdateShirtQuality();
                      }
                      float num3 = itemClothingAsset.armor + (float) ((1.0 - (double) itemClothingAsset.armor) * (1.0 - (double) this.player.clothing.shirtQuality / 100.0));
                      amount = (byte) ((double) amount * (double) num3);
                    }
                  }
                  else if ((int) this.player.clothing.pants != 0)
                  {
                    ItemClothingAsset itemClothingAsset = (ItemClothingAsset) Assets.find(EAssetType.ITEM, this.player.clothing.pants);
                    if (itemClothingAsset != null)
                    {
                      if (Provider.mode != EGameMode.EASY && (int) this.player.clothing.pantsQuality > 0)
                      {
                        --this.player.clothing.pantsQuality;
                        this.player.clothing.sendUpdatePantsQuality();
                      }
                      float num3 = itemClothingAsset.armor + (float) ((1.0 - (double) itemClothingAsset.armor) * (1.0 - (double) this.player.clothing.pantsQuality / 100.0));
                      amount = (byte) ((double) amount * (double) num3);
                    }
                  }
                }
                EPlayerKill kill;
                this.player.life.askDamage(amount, (this.target.position - this.transform.position).normalized * (float) amount, EDeathCause.ZOMBIE, ELimb.SKULL, Provider.server, out kill);
                this.player.life.askInfect((byte) ((double) ((int) amount / 3) * (1.0 - (double) this.player.skills.mastery(1, 2) * 0.5)));
              }
            }
            else
            {
              if ((double) Time.realtimeSinceStartup - (double) this.lastAttack <= 1.0)
                return;
              this.isAttacking = true;
              if (this.speciality == EZombieSpeciality.CRAWLER)
                ZombieManager.sendZombieAttack(this, (byte) 5);
              else if (this.speciality == EZombieSpeciality.SPRINTER)
                ZombieManager.sendZombieAttack(this, (byte) Random.Range(6, 9));
              else
                ZombieManager.sendZombieAttack(this, (byte) Random.Range(0, 5));
            }
          }
          else
          {
            this.lastTarget = Time.realtimeSinceStartup;
            this.isAttacking = false;
          }
        }
        else
        {
          if (this.isMoving || (double) num2 >= 2.0 || (double) Time.realtimeSinceStartup - (double) this.lastHunted <= 3.0)
            return;
          this.stop();
        }
      }
    }

    private void onMoonUpdated(bool isFullMoon)
    {
      if (!((Object) this.eyes != (Object) null))
        return;
      this.eyes.gameObject.SetActive(isFullMoon);
    }

    private void Start()
    {
      if (Provider.isServer)
      {
        this.seeker = this.GetComponent<AIPath>();
        this.target = this.transform.FindChild("Target");
        this.target.parent = LevelNavigation.models;
        this.seeker.target = this.target;
        this.seeker.canSmooth = !Dedicator.isDedicated;
        this.reset();
      }
      else
      {
        this.lastUpdatedPos = this.transform.position;
        this.nsb = new NetworkSnapshotBuffer(Provider.UPDATE_TIME, Provider.UPDATE_TIME * 2.33f);
      }
      this.groanDelay = this.speciality != EZombieSpeciality.MEGA ? Random.Range(4f, 8f) : Random.Range(2f, 4f);
      this.updateLife();
      this.apply();
      this.updateStates();
      if (Dedicator.isDedicated)
        return;
      LightingManager.onMoonUpdated += new MoonUpdated(this.onMoonUpdated);
    }

    private void Awake()
    {
      if (Dedicator.isDedicated)
      {
        this.attackTime = 0.5f;
        this.startleTime = 0.5f;
        this.stunTime = 0.5f;
      }
      else
      {
        this.animator = this.transform.FindChild("Character").GetComponent<Animation>();
        this.skeleton = this.animator.transform.FindChild("Skeleton");
        this.renderer_0 = this.animator.transform.FindChild("Model_0").GetComponent<Renderer>();
        this.renderer_1 = this.animator.transform.FindChild("Model_1").GetComponent<Renderer>();
        this.eyes = this.skeleton.FindChild("Spine").FindChild("Skull").FindChild("Eyes");
        this.attackTime = this.animator["Attack_0"].clip.length;
        this.startleTime = this.animator["Startle_0"].clip.length;
        this.stunTime = this.animator["Stun_0"].clip.length;
      }
    }

    private void OnDestroy()
    {
      if (Dedicator.isDedicated)
        return;
      LightingManager.onMoonUpdated -= new MoonUpdated(this.onMoonUpdated);
    }
  }
}
