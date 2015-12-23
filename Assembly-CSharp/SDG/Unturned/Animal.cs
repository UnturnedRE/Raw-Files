// Decompiled with JetBrains decompiler
// Type: SDG.Unturned.Animal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9F86D58-5A05-437D-A559-3AF9B4C671F2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace SDG.Unturned
{
  public class Animal : MonoBehaviour
  {
    public static readonly float MULTIPLIER_EASY = 0.75f;
    public static readonly float MULTIPLIER_HARD = 1.5f;
    private Animation animator;
    private Transform skeleton;
    private Renderer renderer_0;
    private Renderer renderer_1;
    private float lastTick;
    private float lastEat;
    private float lastGlance;
    private float lastStartle;
    private float lastWander;
    private float lastStuck;
    private float lastTarget;
    private float lastAttack;
    private float eatTime;
    private float glanceTime;
    private float startleTime;
    private float attackTime;
    private float startedRoar;
    private float startedPanic;
    private float eatDelay;
    private float glanceDelay;
    private float wanderDelay;
    private bool isPlayingEat;
    private bool isPlayingGlance;
    private bool isPlayingStartle;
    private bool isPlayingAttack;
    private Player player;
    private Vector3 target;
    private Vector3 lastUpdatePos;
    private NetworkSnapshotBuffer nsb;
    private bool isMoving;
    private bool isRunning;
    private bool canMove;
    private bool _isFleeing;
    private bool isHunting;
    private bool isStuck;
    private bool isAttacking;
    private float _lastDead;
    public bool isDead;
    public ushort index;
    public ushort id;
    private ushort health;
    private Vector3 ragdoll;
    private AnimalAsset _asset;
    private CharacterController controller;
    public bool isUpdated;

    public bool isFleeing
    {
      get
      {
        return this._isFleeing;
      }
    }

    public float lastDead
    {
      get
      {
        return this._lastDead;
      }
    }

    public AnimalAsset asset
    {
      get
      {
        return this._asset;
      }
    }

    public void askEat()
    {
      if (this.isDead)
        return;
      this.lastEat = Time.realtimeSinceStartup;
      this.eatDelay = Random.Range(4f, 8f);
      this.isPlayingEat = true;
      if (Dedicator.isDedicated)
        return;
      this.animator.Play("Eat");
    }

    public void askGlance()
    {
      if (this.isDead)
        return;
      this.lastGlance = Time.realtimeSinceStartup;
      this.glanceDelay = Random.Range(4f, 8f);
      this.isPlayingGlance = true;
      if (Dedicator.isDedicated)
        return;
      this.animator.Play("Glance_" + (object) Random.Range(0, 2));
    }

    public void askStartle()
    {
      if (this.isDead)
        return;
      this.lastStartle = Time.realtimeSinceStartup;
      this.isPlayingStartle = true;
      if (Dedicator.isDedicated)
        return;
      this.animator.Play("Startle");
    }

    public void askAttack()
    {
      if (this.isDead)
        return;
      this.lastAttack = Time.realtimeSinceStartup;
      this.isPlayingAttack = true;
      if (Dedicator.isDedicated)
        return;
      if ((TrackedReference) this.animator["Attack"] != (TrackedReference) null)
        this.animator.Play("Attack");
      if (this.asset.roars.Length <= 0 || (double) Time.realtimeSinceStartup - (double) this.startedRoar <= 1.0)
        return;
      this.startedRoar = Time.realtimeSinceStartup;
      this.GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
      this.GetComponent<AudioSource>().PlayOneShot(this.asset.roars[Random.Range(0, this.asset.roars.Length)]);
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
        AnimalManager.dropLoot(this);
        AnimalManager.sendAnimalDead(this, this.ragdoll);
      }
      else
      {
        if (Dedicator.isDedicated || this.asset.panics.Length <= 0 || (double) Time.realtimeSinceStartup - (double) this.startedPanic <= 1.0)
          return;
        this.startedPanic = Time.realtimeSinceStartup;
        this.GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
        this.GetComponent<AudioSource>().PlayOneShot(this.asset.panics[Random.Range(0, this.asset.panics.Length)]);
      }
    }

    public void sendRevive(Vector3 position, float angle)
    {
      AnimalManager.sendAnimalAlive(this, position, MeasurementTool.angleToByte(angle));
    }

    private bool checkTargetValid(Vector3 point)
    {
      if ((double) LevelLighting.seaLevel < 0.990000009536743)
        return (double) point.y >= (double) LevelLighting.seaLevel * (double) Level.TERRAIN - 1.0;
      return true;
    }

    private void getFleeTarget(Vector3 avoid)
    {
      Vector3 point = this.transform.position + (avoid - this.transform.position).normalized * -64f + new Vector3(Random.Range(-8f, 8f), 0.0f, Random.Range(-8f, 8f));
      if (!this.checkTargetValid(point))
      {
        point = this.transform.position + (avoid - this.transform.position).normalized * -32f + new Vector3(Random.Range(-8f, 8f), 0.0f, Random.Range(-8f, 8f));
        if (!this.checkTargetValid(point))
        {
          point = this.transform.position + (avoid - this.transform.position).normalized * 32f + new Vector3(Random.Range(-8f, 8f), 0.0f, Random.Range(-8f, 8f));
          if (!this.checkTargetValid(point))
            point = this.transform.position + (avoid - this.transform.position).normalized * 16f + new Vector3(Random.Range(-8f, 8f), 0.0f, Random.Range(-8f, 8f));
        }
      }
      this.target = point;
    }

    private void getWanderTarget()
    {
      Vector3 vector3 = this.transform.position + new Vector3(Random.Range(-8f, 8f), 0.0f, Random.Range(-8f, 8f));
      if ((double) LevelLighting.seaLevel < 0.990000009536743 && (double) vector3.y < (double) LevelLighting.seaLevel * (double) Level.TERRAIN - 1.0)
        return;
      this.target = vector3;
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
        this._isFleeing = false;
        this.isHunting = true;
        this.canMove = true;
        this.lastStuck = Time.realtimeSinceStartup;
        this.player = newPlayer;
      }
      else
      {
        if ((double) (newPlayer.transform.position - this.transform.position).sqrMagnitude >= (double) (this.player.transform.position - this.transform.position).sqrMagnitude)
          return;
        this._isFleeing = false;
        this.isHunting = true;
        this.canMove = true;
        this.player = newPlayer;
      }
    }

    public void alert(Vector3 newPosition)
    {
      if (this.isDead || this.isStuck || this.isHunting)
        return;
      if (!this.isFleeing)
        AnimalManager.sendAnimalStartle(this);
      this._isFleeing = true;
      this.isHunting = false;
      this.getFleeTarget(newPosition);
      this.canMove = true;
    }

    private void stop()
    {
      this.isMoving = false;
      this.isRunning = false;
      this._isFleeing = false;
      this.isHunting = false;
      this.isStuck = false;
      this.canMove = false;
      this.player = (Player) null;
      this.target = this.transform.position;
    }

    public void tellAlive(Vector3 newPosition, byte newAngle)
    {
      this.isDead = false;
      this.transform.position = newPosition;
      this.transform.rotation = Quaternion.Euler(0.0f, (float) ((int) newAngle * 2), 0.0f);
      this.updateLife();
      this.updateStates();
      if (!Provider.isServer)
        return;
      this.reset();
    }

    public void tellDead(Vector3 newRagdoll)
    {
      this.isDead = true;
      this._lastDead = Time.realtimeSinceStartup;
      this.updateLife();
      if (!Dedicator.isDedicated)
      {
        this.ragdoll = newRagdoll;
        RagdollTool.ragdollAnimal(this.transform.position, this.transform.rotation, this.skeleton, this.ragdoll, this.id);
      }
      if (!Provider.isServer)
        return;
      this.stop();
    }

    public void tellState(Vector3 newPosition, byte newAngle)
    {
      this.lastTick = Time.realtimeSinceStartup;
      this.lastUpdatePos = newPosition;
      if (this.nsb != null)
        this.nsb.addNewSnapshot(newPosition, Quaternion.Euler(0.0f, (float) newAngle * 2f, 0.0f));
      if (!this.isPlayingEat && !this.isPlayingGlance)
        return;
      this.isPlayingEat = false;
      this.isPlayingGlance = false;
      this.animator.Stop();
    }

    private void updateLife()
    {
      if (Dedicator.isDedicated)
        return;
      if ((Object) this.renderer_0 != (Object) null)
        this.renderer_0.enabled = !this.isDead;
      if ((Object) this.renderer_1 != (Object) null)
        this.renderer_1.enabled = !this.isDead;
      this.skeleton.gameObject.SetActive(!this.isDead);
      this.GetComponent<Collider>().enabled = !this.isDead;
    }

    public void updateStates()
    {
      this.lastUpdatePos = this.transform.position;
      if (this.nsb == null)
        return;
      this.nsb.updateLastSnapshot(this.transform.position, this.transform.rotation);
    }

    private void reset()
    {
      this.target = this.transform.position;
      this.lastTick = Time.realtimeSinceStartup;
      this.lastStartle = Time.realtimeSinceStartup;
      this.lastWander = Time.realtimeSinceStartup;
      this.lastStuck = Time.realtimeSinceStartup;
      this.isPlayingEat = false;
      this.isPlayingGlance = false;
      this.isPlayingStartle = false;
      this.isMoving = false;
      this.isRunning = false;
      this._isFleeing = false;
      this.isHunting = false;
      this.isStuck = false;
      this.canMove = false;
      this._asset = (AnimalAsset) Assets.find(EAssetType.ANIMAL, this.id);
      this.health = this.asset.health;
    }

    private void update(float delta)
    {
      if (!this.canMove)
        return;
      Vector3 vector3 = this.target - this.transform.position;
      vector3.y = 0.0f;
      Vector3 forward1 = vector3;
      float magnitude = vector3.magnitude;
      bool flag = (double) magnitude > 0.75;
      if (!Dedicator.isDedicated && flag && !this.isMoving)
      {
        if (this.isPlayingEat)
        {
          this.animator.Stop();
          this.isPlayingEat = false;
        }
        if (this.isPlayingGlance)
        {
          this.animator.Stop();
          this.isPlayingGlance = false;
        }
        if (this.isPlayingStartle)
        {
          this.animator.Stop();
          this.isPlayingStartle = false;
        }
      }
      this.isMoving = flag;
      this.isRunning = this.isMoving && (this.isFleeing || this.isHunting);
      float num1 = Mathf.Clamp01(magnitude / 0.6f);
      Vector3 forward2 = this.transform.forward;
      float num2 = (!this.isRunning ? this.asset.speedWalk : this.asset.speedRun) * Mathf.Max(Vector3.Dot(vector3.normalized, forward2), 0.05f) * num1;
      if ((double) Time.deltaTime > 0.0)
        num2 = Mathf.Clamp(num2, 0.0f, magnitude / (Time.deltaTime * 2f));
      vector3 = this.transform.forward * num2;
      vector3.y = Physics.gravity.y;
      if ((double) magnitude < 0.75)
      {
        vector3.x = 0.0f;
        vector3.z = 0.0f;
        if (!this.isStuck)
          this._isFleeing = false;
      }
      else
      {
        Vector3 eulerAngles = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(forward1), 8f * delta).eulerAngles;
        eulerAngles.z = 0.0f;
        eulerAngles.x = 0.0f;
        this.transform.rotation = Quaternion.Euler(eulerAngles);
      }
      int num3 = (int) this.controller.Move(vector3 * delta);
    }

    private void Update()
    {
      if (this.isDead)
        return;
      if (!Provider.isServer)
      {
        if ((double) Mathf.Abs(this.lastUpdatePos.x - this.transform.position.x) > 0.200000002980232 || (double) Mathf.Abs(this.lastUpdatePos.y - this.transform.position.y) > 0.200000002980232 || (double) Mathf.Abs(this.lastUpdatePos.z - this.transform.position.z) > 0.200000002980232)
        {
          if (!this.isMoving)
          {
            if (this.isPlayingEat)
            {
              this.animator.Stop();
              this.isPlayingEat = false;
            }
            if (this.isPlayingGlance)
            {
              this.animator.Stop();
              this.isPlayingGlance = false;
            }
            if (this.isPlayingStartle)
            {
              this.animator.Stop();
              this.isPlayingStartle = false;
            }
          }
          this.isMoving = true;
          this.isRunning = (double) (this.lastUpdatePos - this.transform.position).sqrMagnitude > 8.0;
        }
        else
        {
          this.isMoving = false;
          this.isRunning = false;
        }
        if (this.nsb != null)
        {
          Vector3 pos;
          Quaternion rot;
          this.nsb.getCurrentSnapshot(out pos, out rot);
          this.transform.position = pos;
          this.transform.rotation = rot;
        }
      }
      if (!Provider.isServer || Dedicator.isDedicated)
        return;
      this.update(Time.deltaTime);
    }

    private void FixedUpdate()
    {
      if (!this.isDead)
      {
        if (this.isPlayingEat)
        {
          if ((double) Time.realtimeSinceStartup - (double) this.lastEat > (double) this.eatTime)
            this.isPlayingEat = false;
        }
        else if (this.isPlayingGlance)
        {
          if ((double) Time.realtimeSinceStartup - (double) this.lastGlance > (double) this.glanceTime)
            this.isPlayingGlance = false;
        }
        else if (this.isPlayingStartle)
        {
          if ((double) Time.realtimeSinceStartup - (double) this.lastStartle > (double) this.startleTime)
            this.isPlayingStartle = false;
        }
        else if (this.isPlayingAttack)
        {
          if ((double) Time.realtimeSinceStartup - (double) this.lastAttack > (double) this.attackTime)
            this.isPlayingAttack = false;
        }
        else if (!Dedicator.isDedicated)
        {
          if (this.isRunning)
            this.animator.Play("Run");
          else if (this.isMoving)
            this.animator.Play("Walk");
          else
            this.animator.Play("Idle");
        }
      }
      if (!Dedicator.isDedicated && !this.isMoving && (!this.isPlayingEat && !this.isPlayingGlance) && !this.isPlayingAttack)
      {
        if ((double) Time.realtimeSinceStartup - (double) this.lastEat > (double) this.eatDelay)
          this.askEat();
        else if ((double) Time.realtimeSinceStartup - (double) this.lastGlance > (double) this.glanceDelay)
          this.askGlance();
      }
      if (!Provider.isServer)
        return;
      if ((double) Time.realtimeSinceStartup - (double) this.lastTick > (double) Provider.UPDATE_TIME)
      {
        this.lastTick = Time.realtimeSinceStartup;
        if ((double) Mathf.Abs(this.lastUpdatePos.x - this.transform.position.x) > (double) Provider.UPDATE_DISTANCE || (double) Mathf.Abs(this.lastUpdatePos.y - this.transform.position.y) > (double) Provider.UPDATE_DISTANCE || (double) Mathf.Abs(this.lastUpdatePos.z - this.transform.position.z) > (double) Provider.UPDATE_DISTANCE)
        {
          this.lastUpdatePos = this.transform.position;
          if (Dedicator.isDedicated)
          {
            this.isUpdated = true;
            ++AnimalManager.updates;
          }
          this.isStuck = false;
          this.lastStuck = Time.realtimeSinceStartup;
        }
        else if (this.isMoving)
          this.isStuck = true;
      }
      if (this.isStuck)
      {
        if ((double) Time.realtimeSinceStartup - (double) this.lastStuck > 0.25)
        {
          this.lastStuck = Time.realtimeSinceStartup;
          this.getWanderTarget();
        }
      }
      else if (!this.isFleeing && !this.isHunting)
      {
        if ((double) Time.realtimeSinceStartup - (double) this.lastWander > (double) this.wanderDelay)
        {
          this.lastWander = Time.realtimeSinceStartup;
          this.wanderDelay = Random.Range(8f, 16f);
          this.getWanderTarget();
        }
      }
      else
      {
        this.lastStuck = Time.realtimeSinceStartup;
        this.lastWander = Time.realtimeSinceStartup;
      }
      if (this.isHunting)
      {
        if ((Object) this.player != (Object) null && !this.player.life.isDead && ((double) LevelLighting.seaLevel >= 0.990000009536743 || (double) this.player.transform.position.y > (double) LevelLighting.seaLevel * (double) Level.TERRAIN - 1.0))
        {
          this.target = this.player.transform.position;
          if ((double) (Mathf.Pow(this.target.x - this.transform.position.x, 2f) + Mathf.Pow(this.target.z - this.transform.position.z, 2f)) < (!((Object) this.player.movement.getVehicle() != (Object) null) ? 5.0 : 19.0) && (double) Mathf.Abs(this.target.y - this.transform.position.y) < 2.0)
          {
            if ((double) Time.realtimeSinceStartup - (double) this.lastTarget > (!Dedicator.isDedicated ? 0.100000001490116 : 0.300000011920929))
            {
              if (this.isAttacking)
              {
                if ((double) Time.realtimeSinceStartup - (double) this.lastAttack > (double) this.attackTime / 2.0)
                {
                  this.isAttacking = false;
                  byte amount = (byte) ((double) this.asset.damage * (Provider.mode != EGameMode.EASY ? (Provider.mode != EGameMode.HARD ? 1.0 : (double) Animal.MULTIPLIER_HARD) : (double) Animal.MULTIPLIER_EASY));
                  EPlayerKill kill;
                  this.player.life.askDamage(amount, (this.target - this.transform.position).normalized * (float) amount, EDeathCause.ANIMAL, ELimb.SKULL, Provider.server, out kill);
                }
              }
              else if ((double) Time.realtimeSinceStartup - (double) this.lastAttack > 1.0)
              {
                this.isAttacking = true;
                AnimalManager.sendAnimalAttack(this);
              }
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
          this.player = (Player) null;
          this.isHunting = false;
        }
        this.lastStuck = Time.realtimeSinceStartup;
        this.lastWander = Time.realtimeSinceStartup;
      }
      if (!Dedicator.isDedicated)
        return;
      this.update(Time.fixedDeltaTime);
    }

    private void Start()
    {
      if (Provider.isServer)
      {
        this.controller = this.GetComponent<CharacterController>();
        this.reset();
      }
      else
        this.nsb = new NetworkSnapshotBuffer(Provider.UPDATE_TIME, Provider.UPDATE_TIME * 2.33f);
      this.eatDelay = Random.Range(4f, 8f);
      this.glanceDelay = Random.Range(4f, 8f);
      this.wanderDelay = Random.Range(8f, 16f);
      this.updateLife();
      this.updateStates();
    }

    private void Awake()
    {
      if (Dedicator.isDedicated)
      {
        this.eatTime = 0.5f;
        this.glanceTime = 0.5f;
        this.startleTime = 0.5f;
        this.attackTime = 0.5f;
      }
      else
      {
        this.animator = this.transform.FindChild("Character").GetComponent<Animation>();
        this.skeleton = this.animator.transform.FindChild("Skeleton");
        if ((Object) this.animator.transform.FindChild("Model_0") != (Object) null)
          this.renderer_0 = this.animator.transform.FindChild("Model_0").GetComponent<Renderer>();
        if ((bool) ((Object) this.animator.transform.FindChild("Model_1")))
          this.renderer_1 = this.animator.transform.FindChild("Model_1").GetComponent<Renderer>();
        this.eatTime = this.animator["Eat"].clip.length;
        this.glanceTime = this.animator["Glance_0"].clip.length;
        this.startleTime = this.animator["Startle"].clip.length;
        if ((TrackedReference) this.animator["Attack"] != (TrackedReference) null)
          this.attackTime = this.animator["Attack"].clip.length;
        else
          this.attackTime = 0.5f;
      }
    }
  }
}
