// Decompiled with JetBrains decompiler
// Type: AIPath
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding;
using Pathfinding.RVO;
using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Pathfinding/AI/AIPath (generic)")]
[RequireComponent(typeof (Seeker))]
public class AIPath : MonoBehaviour
{
  public float repathRate = 0.5f;
  public bool canSearch = true;
  public bool canMove = true;
  public bool canTurn = true;
  public bool canSmooth = true;
  public float speed = 3f;
  public float turningSpeed = 5f;
  public float slowdownDistance = 0.6f;
  public float pickNextWaypointDist = 2f;
  public float forwardLook = 1f;
  public float endReachedDistance = 0.2f;
  public bool closestOnPathCheck = true;
  protected float minMoveScale = 0.05f;
  protected float lastRepath = -9999f;
  protected bool canSearchAgain = true;
  protected float lastFoundWaypointTime = -9999f;
  public Transform target;
  protected Seeker seeker;
  protected Transform tr;
  protected Path path;
  protected CharacterController controller;
  protected NavmeshController navController;
  protected RVOController rvoController;
  protected Rigidbody rigid;
  protected int currentWaypointIndex;
  protected bool targetReached;
  protected Vector3 lastFoundWaypointPosition;
  private bool startHasRun;
  protected Vector3 targetPoint;
  public Vector3 targetDirection;

  public bool TargetReached
  {
    get
    {
      return this.targetReached;
    }
  }

  protected virtual void Awake()
  {
    this.seeker = this.GetComponent<Seeker>();
    this.tr = this.transform;
    this.controller = this.GetComponent<CharacterController>();
    this.navController = this.GetComponent<NavmeshController>();
    this.rvoController = this.GetComponent<RVOController>();
    if ((UnityEngine.Object) this.rvoController != (UnityEngine.Object) null)
      this.rvoController.enableRotation = false;
    this.rigid = this.GetComponent<Rigidbody>();
  }

  protected virtual void Start()
  {
    this.startHasRun = true;
    this.OnEnable();
  }

  protected virtual void OnEnable()
  {
    this.lastRepath = -9999f;
    this.canSearchAgain = true;
    this.lastFoundWaypointPosition = this.GetFeetPosition();
    if (!this.startHasRun)
      return;
    this.seeker.pathCallback += new OnPathDelegate(this.OnPathComplete);
  }

  public void OnDisable()
  {
    if ((UnityEngine.Object) this.seeker != (UnityEngine.Object) null && !this.seeker.IsDone())
      this.seeker.GetCurrentPath().Error();
    if (this.path != null)
      this.path.Release((object) this);
    this.path = (Path) null;
    this.seeker.pathCallback -= new OnPathDelegate(this.OnPathComplete);
  }

  public virtual void SearchPath()
  {
    if ((UnityEngine.Object) this.target == (UnityEngine.Object) null)
      throw new InvalidOperationException("Target is null");
    this.lastRepath = Time.time;
    Vector3 position = this.target.position;
    this.canSearchAgain = false;
    this.seeker.StartPath(this.GetFeetPosition(), position);
  }

  public virtual void OnTargetReached()
  {
  }

  public virtual void OnPathComplete(Path _p)
  {
    ABPath abPath = _p as ABPath;
    if (abPath == null)
      throw new Exception("This function only handles ABPaths, do not use special path types");
    this.canSearchAgain = true;
    abPath.Claim((object) this);
    if (abPath.error)
    {
      abPath.Release((object) this);
    }
    else
    {
      if (this.path != null)
        this.path.Release((object) this);
      this.path = (Path) abPath;
      this.currentWaypointIndex = 0;
      this.targetReached = false;
      if (!this.closestOnPathCheck)
        return;
      Vector3 currentPosition = (double) Time.time - (double) this.lastFoundWaypointTime >= 0.300000011920929 ? abPath.originalStartPoint : this.lastFoundWaypointPosition;
      Vector3 vector3_1 = this.GetFeetPosition() - currentPosition;
      float magnitude = vector3_1.magnitude;
      Vector3 vector3_2 = vector3_1 / magnitude;
      int num = (int) ((double) magnitude / (double) this.pickNextWaypointDist);
      for (int index = 0; index <= num; ++index)
      {
        this.CalculateVelocity(currentPosition);
        currentPosition += vector3_2;
      }
    }
  }

  public void stop()
  {
    this.path = (Path) null;
  }

  public virtual Vector3 GetFeetPosition()
  {
    if ((UnityEngine.Object) this.rvoController != (UnityEngine.Object) null)
      return this.tr.position - Vector3.up * this.rvoController.height * 0.5f;
    if ((UnityEngine.Object) this.controller != (UnityEngine.Object) null)
      return this.tr.position;
    return this.tr.position;
  }

  private void update(float delta)
  {
    if ((double) Time.time - (double) this.lastRepath >= (double) this.repathRate && this.canSearchAgain && (this.canSearch && (UnityEngine.Object) this.target != (UnityEngine.Object) null))
      this.SearchPath();
    if (!this.canMove || this.path == null)
      return;
    Vector3 vector3 = this.CalculateVelocity(this.transform.position);
    vector3.y = Physics.gravity.y;
    this.RotateTowards(this.targetDirection);
    if ((UnityEngine.Object) this.rvoController != (UnityEngine.Object) null)
    {
      this.rvoController.Move(vector3);
    }
    else
    {
      if ((UnityEngine.Object) this.navController != (UnityEngine.Object) null)
        return;
      if ((UnityEngine.Object) this.controller != (UnityEngine.Object) null && this.controller.enabled)
      {
        int num = (int) this.controller.Move(vector3 * delta);
      }
      else if ((UnityEngine.Object) this.rigid != (UnityEngine.Object) null)
        this.rigid.AddForce(vector3);
      else
        this.transform.Translate(vector3 * delta, Space.World);
    }
  }

  public virtual void Update()
  {
    this.update(Time.deltaTime);
  }

  protected float XZSqrMagnitude(Vector3 a, Vector3 b)
  {
    float num1 = b.x - a.x;
    float num2 = b.z - a.z;
    return (float) ((double) num1 * (double) num1 + (double) num2 * (double) num2);
  }

  protected Vector3 CalculateVelocity(Vector3 currentPosition)
  {
    if (this.path == null || this.path.vectorPath == null || this.path.vectorPath.Count == 0)
      return Vector3.zero;
    List<Vector3> list = this.path.vectorPath;
    if (list.Count == 1)
      list.Insert(0, currentPosition);
    if (this.currentWaypointIndex >= list.Count)
      this.currentWaypointIndex = list.Count - 1;
    if (this.currentWaypointIndex <= 1)
      this.currentWaypointIndex = 1;
    for (; this.currentWaypointIndex < list.Count - 1 && (double) this.XZSqrMagnitude(list[this.currentWaypointIndex], currentPosition) < (double) this.pickNextWaypointDist * (double) this.pickNextWaypointDist; ++this.currentWaypointIndex)
    {
      this.lastFoundWaypointPosition = currentPosition;
      this.lastFoundWaypointTime = Time.time;
    }
    Vector3 vector3_1 = list[this.currentWaypointIndex] - list[this.currentWaypointIndex - 1];
    Vector3 vector3_2 = this.CalculateTargetPoint(currentPosition, list[this.currentWaypointIndex - 1], list[this.currentWaypointIndex]);
    Vector3 vector3_3 = vector3_2 - currentPosition;
    vector3_3.y = 0.0f;
    float magnitude = vector3_3.magnitude;
    float num1 = Mathf.Clamp01(magnitude / this.slowdownDistance);
    if (this.canTurn)
      this.targetDirection = vector3_3;
    this.targetPoint = vector3_2;
    if (this.currentWaypointIndex == list.Count - 1 && (double) magnitude <= (double) this.endReachedDistance)
    {
      if (!this.targetReached)
      {
        this.targetReached = true;
        this.OnTargetReached();
      }
      return Vector3.zero;
    }
    Vector3 forward = this.tr.forward;
    float num2 = this.speed * Mathf.Max(Vector3.Dot(vector3_3.normalized, forward), this.minMoveScale) * num1;
    if ((double) Time.deltaTime > 0.0)
      num2 = Mathf.Clamp(num2, 0.0f, magnitude / (Time.deltaTime * 2f));
    return forward * num2;
  }

  protected virtual void RotateTowards(Vector3 dir)
  {
    if (dir == Vector3.zero)
      return;
    Vector3 eulerAngles = Quaternion.Slerp(this.tr.rotation, Quaternion.LookRotation(dir), this.turningSpeed * Time.deltaTime).eulerAngles;
    eulerAngles.z = 0.0f;
    eulerAngles.x = 0.0f;
    this.tr.rotation = Quaternion.Euler(eulerAngles);
  }

  protected Vector3 CalculateTargetPoint(Vector3 p, Vector3 a, Vector3 b)
  {
    if ((double) (b - this.target.position).sqrMagnitude < 16.0)
      return this.target.position;
    a.y = p.y;
    b.y = p.y;
    float magnitude = (a - b).magnitude;
    if ((double) magnitude == 0.0)
      return a;
    float num1 = AstarMath.Clamp01(AstarMath.NearestPointFactor(a, b, p));
    float num2 = Mathf.Clamp(Mathf.Clamp(this.forwardLook - ((b - a) * num1 + a - p).magnitude, 0.0f, this.forwardLook) / magnitude + num1, 0.0f, 1f);
    return (b - a) * num2 + a;
  }
}
