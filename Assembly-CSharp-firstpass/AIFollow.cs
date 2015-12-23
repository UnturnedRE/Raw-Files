// Decompiled with JetBrains decompiler
// Type: AIFollow
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

[RequireComponent(typeof (Seeker))]
[RequireComponent(typeof (CharacterController))]
[AddComponentMenu("Pathfinding/AI/AIFollow (deprecated)")]
public class AIFollow : MonoBehaviour
{
  public float repathRate = 0.1f;
  public float pickNextWaypointDistance = 1f;
  public float targetReached = 0.2f;
  public float speed = 5f;
  public float rotationSpeed = 1f;
  public bool canSearch = true;
  public bool canMove = true;
  protected float lastPathSearch = -9999f;
  public Transform target;
  public bool drawGizmos;
  protected Seeker seeker;
  protected CharacterController controller;
  protected NavmeshController navmeshController;
  protected Transform tr;
  protected int pathIndex;
  protected Vector3[] path;

  public void Start()
  {
    this.seeker = this.GetComponent<Seeker>();
    this.controller = this.GetComponent<CharacterController>();
    this.navmeshController = this.GetComponent<NavmeshController>();
    this.tr = this.transform;
    this.Repath();
  }

  public void Reset()
  {
    this.path = (Vector3[]) null;
  }

  public void OnPathComplete(Path p)
  {
    this.StartCoroutine(this.WaitToRepath());
    if (p.error)
      return;
    this.path = p.vectorPath.ToArray();
    float num1 = float.PositiveInfinity;
    int num2 = 0;
    for (int index = 0; index < this.path.Length - 1; ++index)
    {
      float num3 = AstarMath.DistancePointSegmentStrict(this.path[index], this.path[index + 1], this.tr.position);
      if ((double) num3 < (double) num1)
      {
        num2 = 0;
        num1 = num3;
        this.pathIndex = index + 1;
      }
      else if (num2 > 6)
        break;
    }
  }

  [DebuggerHidden]
  public IEnumerator WaitToRepath()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new AIFollow.\u003CWaitToRepath\u003Ec__IteratorF()
    {
      \u003C\u003Ef__this = this
    };
  }

  public void Stop()
  {
    this.canMove = false;
    this.canSearch = false;
  }

  public void Resume()
  {
    this.canMove = true;
    this.canSearch = true;
  }

  public virtual void Repath()
  {
    this.lastPathSearch = Time.time;
    if ((UnityEngine.Object) this.seeker == (UnityEngine.Object) null || (UnityEngine.Object) this.target == (UnityEngine.Object) null || (!this.canSearch || !this.seeker.IsDone()))
      this.StartCoroutine(this.WaitToRepath());
    else
      this.seeker.StartPath((Path) ABPath.Construct(this.transform.position, this.target.position, (OnPathDelegate) null), new OnPathDelegate(this.OnPathComplete), -1);
  }

  public void PathToTarget(Vector3 targetPoint)
  {
    this.lastPathSearch = Time.time;
    if ((UnityEngine.Object) this.seeker == (UnityEngine.Object) null)
      return;
    this.seeker.StartPath(this.transform.position, targetPoint, new OnPathDelegate(this.OnPathComplete));
  }

  public virtual void ReachedEndOfPath()
  {
  }

  public void Update()
  {
    if (this.path == null || this.pathIndex >= this.path.Length || (this.pathIndex < 0 || !this.canMove))
      return;
    Vector3 vector3 = this.path[this.pathIndex];
    vector3.y = this.tr.position.y;
    while ((double) (vector3 - this.tr.position).sqrMagnitude < (double) this.pickNextWaypointDistance * (double) this.pickNextWaypointDistance)
    {
      ++this.pathIndex;
      if (this.pathIndex >= this.path.Length)
      {
        if ((double) (vector3 - this.tr.position).sqrMagnitude < (double) this.pickNextWaypointDistance * (double) this.targetReached * ((double) this.pickNextWaypointDistance * (double) this.targetReached))
        {
          this.ReachedEndOfPath();
          return;
        }
        --this.pathIndex;
        break;
      }
      vector3 = this.path[this.pathIndex];
      vector3.y = this.tr.position.y;
    }
    Vector3 forward = vector3 - this.tr.position;
    this.tr.rotation = Quaternion.Slerp(this.tr.rotation, Quaternion.LookRotation(forward), this.rotationSpeed * Time.deltaTime);
    this.tr.eulerAngles = new Vector3(0.0f, this.tr.eulerAngles.y, 0.0f);
    Vector3 speed = this.transform.forward * this.speed * Mathf.Clamp01(Vector3.Dot(forward.normalized, this.tr.forward));
    if ((UnityEngine.Object) this.navmeshController != (UnityEngine.Object) null)
      return;
    if ((UnityEngine.Object) this.controller != (UnityEngine.Object) null)
      this.controller.SimpleMove(speed);
    else
      this.transform.Translate(speed * Time.deltaTime, Space.World);
  }

  public void OnDrawGizmos()
  {
    if (!this.drawGizmos || this.path == null || (this.pathIndex >= this.path.Length || this.pathIndex < 0))
      return;
    Vector3 end1 = this.path[this.pathIndex];
    end1.y = this.tr.position.y;
    UnityEngine.Debug.DrawLine(this.transform.position, end1, Color.blue);
    float num1 = this.pickNextWaypointDistance;
    if (this.pathIndex == this.path.Length - 1)
      num1 *= this.targetReached;
    Vector3 start = end1 + num1 * new Vector3(1f, 0.0f, 0.0f);
    float num2 = 0.0f;
    while ((double) num2 < 2.0 * Math.PI)
    {
      Vector3 end2 = end1 + new Vector3((float) Math.Cos((double) num2) * num1, 0.0f, (float) Math.Sin((double) num2) * num1);
      UnityEngine.Debug.DrawLine(start, end2, Color.yellow);
      start = end2;
      num2 += 0.1f;
    }
    UnityEngine.Debug.DrawLine(start, end1 + num1 * new Vector3(1f, 0.0f, 0.0f), Color.yellow);
  }
}
