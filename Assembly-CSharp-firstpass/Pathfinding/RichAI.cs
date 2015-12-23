// Decompiled with JetBrains decompiler
// Type: Pathfinding.RichAI
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding.RVO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Pathfinding
{
  [AddComponentMenu("Pathfinding/AI/RichAI (for navmesh)")]
  [RequireComponent(typeof (Seeker))]
  public class RichAI : MonoBehaviour
  {
    private static float deltaTime = 0.0f;
    public static readonly Color GizmoColorRaycast = new Color(0.4627451f, 0.8078431f, 0.4392157f);
    public static readonly Color GizmoColorPath = new Color(0.03137255f, 0.3058824f, 0.7607843f);
    public bool drawGizmos = true;
    public float repathRate = 0.5f;
    public float maxSpeed = 1f;
    public float acceleration = 5f;
    public float slowdownTime = 0.5f;
    public float rotationSpeed = 360f;
    public float endReachedDistance = 0.01f;
    public float wallForce = 3f;
    public float wallDist = 1f;
    public Vector3 gravity = new Vector3(0.0f, -9.82f, 0.0f);
    public bool raycastingForGroundPlacement = true;
    public LayerMask groundMask = (LayerMask) -1;
    public float centerOffset = 1f;
    public bool preciseSlowdown = true;
    public bool slowWhenNotFacingTarget = true;
    private float distanceToWaypoint = 999f;
    protected List<Vector3> buffer = new List<Vector3>();
    protected List<Vector3> wallBuffer = new List<Vector3>();
    protected float lastRepath = -9999f;
    public Transform target;
    public bool repeatedlySearchPaths;
    public RichFunnel.FunnelSimplification funnelSimplification;
    public Animation anim;
    private Vector3 velocity;
    protected RichPath rp;
    protected Seeker seeker;
    protected Transform tr;
    private CharacterController controller;
    private RVOController rvoController;
    private Vector3 lastTargetPoint;
    private Vector3 currentTargetDirection;
    protected bool waitingForPathCalc;
    protected bool canSearchPath;
    protected bool delayUpdatePath;
    protected bool traversingSpecialPath;
    protected bool lastCorner;
    private bool startHasRun;

    public Vector3 Velocity
    {
      get
      {
        return this.velocity;
      }
    }

    public bool TraversingSpecial
    {
      get
      {
        return this.traversingSpecialPath;
      }
    }

    public Vector3 TargetPoint
    {
      get
      {
        return this.lastTargetPoint;
      }
    }

    public bool ApproachingPartEndpoint
    {
      get
      {
        return this.lastCorner;
      }
    }

    public bool ApproachingPathEndpoint
    {
      get
      {
        if (this.rp == null || !this.ApproachingPartEndpoint)
          return false;
        return !this.rp.PartsLeft();
      }
    }

    public float DistanceToNextWaypoint
    {
      get
      {
        return this.distanceToWaypoint;
      }
    }

    private void Awake()
    {
      this.seeker = this.GetComponent<Seeker>();
      this.controller = this.GetComponent<CharacterController>();
      this.rvoController = this.GetComponent<RVOController>();
      if ((UnityEngine.Object) this.rvoController != (UnityEngine.Object) null)
        this.rvoController.enableRotation = false;
      this.tr = this.transform;
    }

    protected virtual void Start()
    {
      this.startHasRun = true;
      this.OnEnable();
    }

    protected virtual void OnEnable()
    {
      this.lastRepath = -9999f;
      this.waitingForPathCalc = false;
      this.canSearchPath = true;
      if (!this.startHasRun)
        return;
      this.seeker.pathCallback += new OnPathDelegate(this.OnPathComplete);
      this.StartCoroutine(this.SearchPaths());
    }

    public void OnDisable()
    {
      if ((UnityEngine.Object) this.seeker != (UnityEngine.Object) null && !this.seeker.IsDone())
        this.seeker.GetCurrentPath().Error();
      this.seeker.pathCallback -= new OnPathDelegate(this.OnPathComplete);
    }

    public virtual void UpdatePath()
    {
      this.canSearchPath = true;
      this.waitingForPathCalc = false;
      Path currentPath = this.seeker.GetCurrentPath();
      if (currentPath != null && !this.seeker.IsDone())
      {
        currentPath.Error();
        currentPath.Claim((object) this);
        currentPath.Release((object) this);
      }
      if ((UnityEngine.Object) this.target == (UnityEngine.Object) null)
        return;
      this.waitingForPathCalc = true;
      this.lastRepath = Time.time;
      this.seeker.StartPath(this.tr.position, this.target.position);
    }

    [DebuggerHidden]
    private IEnumerator SearchPaths()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new RichAI.\u003CSearchPaths\u003Ec__Iterator0()
      {
        \u003C\u003Ef__this = this
      };
    }

    private void OnPathComplete(Path p)
    {
      this.waitingForPathCalc = false;
      p.Claim((object) this);
      if (p.error)
      {
        p.Release((object) this);
      }
      else
      {
        if (this.traversingSpecialPath)
        {
          this.delayUpdatePath = true;
        }
        else
        {
          if (this.rp == null)
            this.rp = new RichPath();
          this.rp.Initialize(this.seeker, p, true, this.funnelSimplification);
        }
        p.Release((object) this);
      }
    }

    private void NextPart()
    {
      this.rp.NextPart();
      this.lastCorner = false;
      if (this.rp.PartsLeft())
        return;
      this.OnTargetReached();
    }

    protected virtual void OnTargetReached()
    {
    }

    protected virtual Vector3 UpdateTarget(RichFunnel fn)
    {
      this.buffer.Clear();
      Vector3 position = this.tr.position;
      bool requiresRepath;
      Vector3 vector3 = fn.Update(position, this.buffer, 2, out this.lastCorner, out requiresRepath);
      if (requiresRepath && !this.waitingForPathCalc)
        this.UpdatePath();
      return vector3;
    }

    protected virtual void Update()
    {
      RichAI.deltaTime = Mathf.Min(Time.smoothDeltaTime * 2f, Time.deltaTime);
      if (this.rp != null)
      {
        RichPathPart currentPart = this.rp.GetCurrentPart();
        RichFunnel fn = currentPart as RichFunnel;
        if (fn != null)
        {
          Vector3 vector3_1 = this.UpdateTarget(fn);
          if (Time.frameCount % 5 == 0)
          {
            this.wallBuffer.Clear();
            fn.FindWalls(this.wallBuffer, this.wallDist);
          }
          int index1 = 0;
          Vector3 vector3_2 = this.buffer[index1];
          Vector3 lhs1 = vector3_2 - vector3_1;
          lhs1.y = 0.0f;
          if ((double) Vector3.Dot(lhs1, this.currentTargetDirection) < 0.0 && this.buffer.Count - index1 > 1)
          {
            ++index1;
            vector3_2 = this.buffer[index1];
          }
          if (vector3_2 != this.lastTargetPoint)
          {
            this.currentTargetDirection = vector3_2 - vector3_1;
            this.currentTargetDirection.y = 0.0f;
            this.currentTargetDirection.Normalize();
            this.lastTargetPoint = vector3_2;
          }
          Vector3 vector3_3 = vector3_2 - vector3_1;
          vector3_3.y = 0.0f;
          float magnitude = vector3_3.magnitude;
          this.distanceToWaypoint = magnitude;
          Vector3 vector3_4 = (double) magnitude != 0.0 ? vector3_3 / magnitude : Vector3.zero;
          Vector3 lhs2 = vector3_4;
          Vector3 vector3_5 = Vector3.zero;
          if ((double) this.wallForce > 0.0 && (double) this.wallDist > 0.0)
          {
            float val1_1 = 0.0f;
            float val1_2 = 0.0f;
            int index2 = 0;
            while (index2 < this.wallBuffer.Count)
            {
              float sqrMagnitude = (AstarMath.NearestPointStrict(this.wallBuffer[index2], this.wallBuffer[index2 + 1], this.tr.position) - vector3_1).sqrMagnitude;
              if ((double) sqrMagnitude <= (double) this.wallDist * (double) this.wallDist)
              {
                Vector3 normalized = (this.wallBuffer[index2 + 1] - this.wallBuffer[index2]).normalized;
                float val2 = Vector3.Dot(vector3_4, normalized) * (1f - Math.Max(0.0f, (float) (2.0 * ((double) sqrMagnitude / ((double) this.wallDist * (double) this.wallDist)) - 1.0)));
                if ((double) val2 > 0.0)
                  val1_2 = Math.Max(val1_2, val2);
                else
                  val1_1 = Math.Max(val1_1, -val2);
              }
              index2 += 2;
            }
            vector3_5 = Vector3.Cross(Vector3.up, vector3_4) * (val1_2 - val1_1);
          }
          bool flag = this.lastCorner && this.buffer.Count - index1 == 1;
          Vector3 vector3_6;
          if (flag)
          {
            if ((double) this.slowdownTime < 1.0 / 1000.0)
              this.slowdownTime = 1.0 / 1000.0;
            Vector3 vector3_7 = vector3_2 - vector3_1;
            vector3_7.y = 0.0f;
            vector3_6 = Vector3.ClampMagnitude(!this.preciseSlowdown ? 2f * (vector3_7 - this.slowdownTime * this.velocity) / (this.slowdownTime * this.slowdownTime) : (6f * vector3_7 - 4f * this.slowdownTime * this.velocity) / (this.slowdownTime * this.slowdownTime), this.acceleration);
            vector3_5 *= Math.Min(magnitude / 0.5f, 1f);
            if ((double) magnitude < (double) this.endReachedDistance)
              this.NextPart();
          }
          else
            vector3_6 = vector3_4 * this.acceleration;
          this.velocity += (vector3_6 + vector3_5 * this.wallForce) * RichAI.deltaTime;
          if (this.slowWhenNotFacingTarget)
          {
            float a1 = (float) (((double) Vector3.Dot(lhs2, this.tr.forward) + 0.5) * 0.666666686534882);
            float a2 = Mathf.Sqrt((float) ((double) this.velocity.x * (double) this.velocity.x + (double) this.velocity.z * (double) this.velocity.z));
            float num1 = this.velocity.y;
            this.velocity.y = 0.0f;
            float num2 = Mathf.Min(a2, this.maxSpeed * Mathf.Max(a1, 0.2f));
            this.velocity = Vector3.Lerp(this.tr.forward * num2, this.velocity.normalized * num2, Mathf.Clamp(!flag ? 0.0f : magnitude * 2f, 0.5f, 1f));
            this.velocity.y = num1;
          }
          else
          {
            float num = this.maxSpeed / Mathf.Sqrt((float) ((double) this.velocity.x * (double) this.velocity.x + (double) this.velocity.z * (double) this.velocity.z));
            if ((double) num < 1.0)
            {
              this.velocity.x *= num;
              this.velocity.z *= num;
            }
          }
          if (flag)
            this.RotateTowards(Vector3.Lerp(this.velocity, this.currentTargetDirection, Math.Max((float) (1.0 - (double) magnitude * 2.0), 0.0f)));
          else
            this.RotateTowards(this.velocity);
          this.velocity += RichAI.deltaTime * this.gravity;
          if ((UnityEngine.Object) this.rvoController != (UnityEngine.Object) null && this.rvoController.enabled)
          {
            this.tr.position = vector3_1;
            this.rvoController.Move(this.velocity);
          }
          else if ((UnityEngine.Object) this.controller != (UnityEngine.Object) null && this.controller.enabled)
          {
            this.tr.position = vector3_1;
            int num = (int) this.controller.Move(this.velocity * RichAI.deltaTime);
          }
          else
          {
            float lasty = vector3_1.y;
            this.tr.position = this.RaycastPosition(vector3_1 + this.velocity * RichAI.deltaTime, lasty);
          }
        }
        else if ((UnityEngine.Object) this.rvoController != (UnityEngine.Object) null && this.rvoController.enabled)
          this.rvoController.Move(Vector3.zero);
        if (!(currentPart is RichSpecial))
          return;
        RichSpecial rs = currentPart as RichSpecial;
        if (this.traversingSpecialPath)
          return;
        this.StartCoroutine(this.TraverseSpecial(rs));
      }
      else if ((UnityEngine.Object) this.rvoController != (UnityEngine.Object) null && this.rvoController.enabled)
      {
        this.rvoController.Move(Vector3.zero);
      }
      else
      {
        if ((UnityEngine.Object) this.controller != (UnityEngine.Object) null && this.controller.enabled)
          return;
        this.tr.position = this.RaycastPosition(this.tr.position, this.tr.position.y);
      }
    }

    private Vector3 RaycastPosition(Vector3 position, float lasty)
    {
      if (this.raycastingForGroundPlacement)
      {
        float maxDistance = Mathf.Max(this.centerOffset, lasty - position.y + this.centerOffset);
        RaycastHit hitInfo;
        if (Physics.Raycast(position + Vector3.up * maxDistance, Vector3.down, out hitInfo, maxDistance, (int) this.groundMask) && (double) hitInfo.distance < (double) maxDistance)
        {
          position = hitInfo.point;
          this.velocity.y = 0.0f;
        }
      }
      return position;
    }

    private bool RotateTowards(Vector3 trotdir)
    {
      Quaternion rotation = this.tr.rotation;
      trotdir.y = 0.0f;
      if (!(trotdir != Vector3.zero))
        return false;
      Vector3 eulerAngles1 = Quaternion.LookRotation(trotdir).eulerAngles;
      Vector3 eulerAngles2 = rotation.eulerAngles;
      eulerAngles2.y = Mathf.MoveTowardsAngle(eulerAngles2.y, eulerAngles1.y, this.rotationSpeed * RichAI.deltaTime);
      this.tr.rotation = Quaternion.Euler(eulerAngles2);
      return (double) Mathf.Abs(eulerAngles2.y - eulerAngles1.y) < 5.0;
    }

    public void OnDrawGizmos()
    {
      if (!this.drawGizmos)
        return;
      if (this.raycastingForGroundPlacement)
      {
        Gizmos.color = RichAI.GizmoColorRaycast;
        Gizmos.DrawLine(this.transform.position, this.transform.position + Vector3.up * this.centerOffset);
        Gizmos.DrawLine(this.transform.position + Vector3.left * 0.1f, this.transform.position + Vector3.right * 0.1f);
        Gizmos.DrawLine(this.transform.position + Vector3.back * 0.1f, this.transform.position + Vector3.forward * 0.1f);
      }
      if (!((UnityEngine.Object) this.tr != (UnityEngine.Object) null) || this.buffer == null)
        return;
      Gizmos.color = RichAI.GizmoColorPath;
      Vector3 from = this.tr.position;
      for (int index = 0; index < this.buffer.Count; ++index)
      {
        Gizmos.DrawLine(from, this.buffer[index]);
        from = this.buffer[index];
      }
    }

    [DebuggerHidden]
    private IEnumerator TraverseSpecial(RichSpecial rs)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new RichAI.\u003CTraverseSpecial\u003Ec__Iterator1()
      {
        rs = rs,
        \u003C\u0024\u003Ers = rs,
        \u003C\u003Ef__this = this
      };
    }
  }
}
