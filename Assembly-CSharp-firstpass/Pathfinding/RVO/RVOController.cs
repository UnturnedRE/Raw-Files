﻿// Decompiled with JetBrains decompiler
// Type: Pathfinding.RVO.RVOController
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.RVO
{
  [AddComponentMenu("Pathfinding/Local Avoidance/RVO Controller")]
  public class RVOController : MonoBehaviour
  {
    private static readonly Color GizmoColor = new Color(0.9411765f, 0.8352941f, 0.1176471f);
    [Tooltip("Radius of the agent")]
    public float radius = 5f;
    [Tooltip("Max speed of the agent. In world units/second")]
    public float maxSpeed = 2f;
    [Tooltip("Height of the agent. In world units")]
    public float height = 1f;
    [Tooltip("Automatically set #locked to true when desired velocity is approximately zero")]
    public bool lockWhenNotMoving = true;
    [Tooltip("How far in the time to look for collisions with other agents")]
    public float agentTimeHorizon = 2f;
    [HideInInspector]
    public float obstacleTimeHorizon = 2f;
    [Tooltip("Maximum distance to other agents to take them into account for collisions.\nDecreasing this value can lead to better performance, increasing it can lead to better quality of the simulation")]
    public float neighbourDist = 10f;
    [Tooltip("Max number of other agents to take into account.\nA smaller value can reduce CPU load, a higher value can lead to better local avoidance quality.")]
    public int maxNeighbours = 10;
    [Tooltip("Layer mask for the ground. The RVOController will raycast down to check for the ground to figure out where to place the agent")]
    public LayerMask mask = (LayerMask) -1;
    public RVOLayer layer = RVOLayer.DefaultAgent;
    [AstarEnumFlag]
    public RVOLayer collidesWith = (RVOLayer) -1;
    [HideInInspector]
    public float wallAvoidForce = 1f;
    [HideInInspector]
    public float wallAvoidFalloff = 1f;
    public bool enableRotation = true;
    public float rotationSpeed = 30f;
    [Tooltip("A locked unit cannot move. Other units will still avoid it. But avoidance quailty is not the best")]
    public bool locked;
    [Tooltip("Center of the agent relative to the pivot point of this game object")]
    public Vector3 center;
    private IAgent rvoAgent;
    private Simulator simulator;
    private float adjustedY;
    private Transform tr;
    private Vector3 desiredVelocity;
    public bool debug;
    private Vector3 lastPosition;

    public Vector3 position
    {
      get
      {
        return this.rvoAgent.InterpolatedPosition;
      }
    }

    public Vector3 velocity
    {
      get
      {
        return this.rvoAgent.Velocity;
      }
    }

    public void OnDisable()
    {
      if (this.simulator == null)
        return;
      this.simulator.RemoveAgent(this.rvoAgent);
    }

    public void Awake()
    {
      this.tr = this.transform;
      RVOSimulator rvoSimulator = Object.FindObjectOfType(typeof (RVOSimulator)) as RVOSimulator;
      if ((Object) rvoSimulator == (Object) null)
        Debug.LogError((object) "No RVOSimulator component found in the scene. Please add one.");
      else
        this.simulator = rvoSimulator.GetSimulator();
    }

    public void OnEnable()
    {
      if (this.simulator == null)
        return;
      if (this.rvoAgent != null)
        this.simulator.AddAgent(this.rvoAgent);
      else
        this.rvoAgent = this.simulator.AddAgent(this.transform.position);
      this.UpdateAgentProperties();
      this.rvoAgent.Teleport(this.transform.position);
      this.adjustedY = this.rvoAgent.Position.y;
    }

    protected void UpdateAgentProperties()
    {
      this.rvoAgent.Radius = this.radius;
      this.rvoAgent.MaxSpeed = this.maxSpeed;
      this.rvoAgent.Height = this.height;
      this.rvoAgent.AgentTimeHorizon = this.agentTimeHorizon;
      this.rvoAgent.ObstacleTimeHorizon = this.obstacleTimeHorizon;
      this.rvoAgent.Locked = this.locked;
      this.rvoAgent.MaxNeighbours = this.maxNeighbours;
      this.rvoAgent.DebugDraw = this.debug;
      this.rvoAgent.NeighbourDist = this.neighbourDist;
      this.rvoAgent.Layer = this.layer;
      this.rvoAgent.CollidesWith = this.collidesWith;
    }

    public void Move(Vector3 vel)
    {
      this.desiredVelocity = vel;
    }

    public void Teleport(Vector3 pos)
    {
      this.tr.position = pos;
      this.lastPosition = pos;
      this.rvoAgent.Teleport(pos);
      this.adjustedY = pos.y;
    }

    public void Update()
    {
      if (this.rvoAgent == null)
        return;
      if (this.lastPosition != this.tr.position)
        this.Teleport(this.tr.position);
      if (this.lockWhenNotMoving)
        this.locked = this.desiredVelocity == Vector3.zero;
      this.UpdateAgentProperties();
      Vector3 interpolatedPosition = this.rvoAgent.InterpolatedPosition;
      interpolatedPosition.y = this.adjustedY;
      RaycastHit hitInfo;
      this.adjustedY = (int) this.mask == 0 || !Physics.Raycast(interpolatedPosition + Vector3.up * this.height * 0.5f, Vector3.down, out hitInfo, float.PositiveInfinity, (int) this.mask) ? 0.0f : hitInfo.point.y;
      interpolatedPosition.y = this.adjustedY;
      this.rvoAgent.SetYPosition(this.adjustedY);
      Vector3 zero = Vector3.zero;
      if ((double) this.wallAvoidFalloff > 0.0 && (double) this.wallAvoidForce > 0.0)
      {
        List<ObstacleVertex> neighbourObstacles = this.rvoAgent.NeighbourObstacles;
        if (neighbourObstacles != null)
        {
          for (int index = 0; index < neighbourObstacles.Count; ++index)
          {
            Vector3 lineStart = neighbourObstacles[index].position;
            Vector3 lineEnd = neighbourObstacles[index].next.position;
            Vector3 vector3_1 = this.position - AstarMath.NearestPointStrict(lineStart, lineEnd, this.position);
            if (!(vector3_1 == lineStart) && !(vector3_1 == lineEnd))
            {
              float sqrMagnitude = vector3_1.sqrMagnitude;
              Vector3 vector3_2 = vector3_1 / (sqrMagnitude * this.wallAvoidFalloff);
              zero += vector3_2;
            }
          }
        }
      }
      this.rvoAgent.DesiredVelocity = this.desiredVelocity + zero * this.wallAvoidForce;
      this.tr.position = interpolatedPosition + Vector3.up * this.height * 0.5f - this.center;
      this.lastPosition = this.tr.position;
      if (!this.enableRotation || !(this.velocity != Vector3.zero))
        return;
      this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(this.velocity), Time.deltaTime * this.rotationSpeed * Mathf.Min(this.velocity.magnitude, 0.2f));
    }

    public void OnDrawGizmos()
    {
      Gizmos.color = RVOController.GizmoColor;
      Gizmos.DrawWireSphere(this.transform.position + this.center - Vector3.up * this.height * 0.5f + Vector3.up * this.radius * 0.5f, this.radius);
      Gizmos.DrawLine(this.transform.position + this.center - Vector3.up * this.height * 0.5f, this.transform.position + this.center + Vector3.up * this.height * 0.5f);
      Gizmos.DrawWireSphere(this.transform.position + this.center + Vector3.up * this.height * 0.5f - Vector3.up * this.radius * 0.5f, this.radius);
    }
  }
}
