// Decompiled with JetBrains decompiler
// Type: Pathfinding.RVO.Sampled.Agent
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding;
using Pathfinding.RVO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Pathfinding.RVO.Sampled
{
  public class Agent : IAgent
  {
    public static Stopwatch watch1 = new Stopwatch();
    public static Stopwatch watch2 = new Stopwatch();
    public static float DesiredVelocityWeight = 0.02f;
    public static float DesiredVelocityScale = 0.1f;
    public static float GlobalIncompressibility = 30f;
    public List<Agent> neighbours = new List<Agent>();
    public List<float> neighbourDists = new List<float>();
    private List<ObstacleVertex> obstaclesBuffered = new List<ObstacleVertex>();
    private List<ObstacleVertex> obstacles = new List<ObstacleVertex>();
    private List<float> obstacleDists = new List<float>();
    private const float WallWeight = 5f;
    private Vector3 smoothPos;
    public float radius;
    public float height;
    public float maxSpeed;
    public float neighbourDist;
    public float agentTimeHorizon;
    public float obstacleTimeHorizon;
    public float weight;
    public bool locked;
    private RVOLayer layer;
    private RVOLayer collidesWith;
    public int maxNeighbours;
    public Vector3 position;
    public Vector3 desiredVelocity;
    public Vector3 prevSmoothPos;
    internal Agent next;
    private Vector3 velocity;
    internal Vector3 newVelocity;
    public Simulator simulator;

    public Vector3 Position { get; private set; }

    public Vector3 InterpolatedPosition
    {
      get
      {
        return this.smoothPos;
      }
    }

    public Vector3 DesiredVelocity { get; set; }

    public RVOLayer Layer { get; set; }

    public RVOLayer CollidesWith { get; set; }

    public bool Locked { get; set; }

    public float Radius { get; set; }

    public float Height { get; set; }

    public float MaxSpeed { get; set; }

    public float NeighbourDist { get; set; }

    public float AgentTimeHorizon { get; set; }

    public float ObstacleTimeHorizon { get; set; }

    public Vector3 Velocity { get; set; }

    public bool DebugDraw { get; set; }

    public int MaxNeighbours { get; set; }

    public List<ObstacleVertex> NeighbourObstacles
    {
      get
      {
        return (List<ObstacleVertex>) null;
      }
    }

    public Agent(Vector3 pos)
    {
      this.MaxSpeed = 2f;
      this.NeighbourDist = 15f;
      this.AgentTimeHorizon = 2f;
      this.ObstacleTimeHorizon = 2f;
      this.Height = 5f;
      this.Radius = 5f;
      this.MaxNeighbours = 10;
      this.Locked = false;
      this.position = pos;
      this.Position = this.position;
      this.prevSmoothPos = this.position;
      this.smoothPos = this.position;
      this.Layer = RVOLayer.DefaultAgent;
      this.CollidesWith = (RVOLayer) -1;
    }

    public void Teleport(Vector3 pos)
    {
      this.Position = pos;
      this.smoothPos = pos;
      this.prevSmoothPos = pos;
    }

    public void SetYPosition(float yCoordinate)
    {
      this.Position = new Vector3(this.Position.x, yCoordinate, this.Position.z);
      this.smoothPos.y = yCoordinate;
      this.prevSmoothPos.y = yCoordinate;
    }

    public void BufferSwitch()
    {
      this.radius = this.Radius;
      this.height = this.Height;
      this.maxSpeed = this.MaxSpeed;
      this.neighbourDist = this.NeighbourDist;
      this.agentTimeHorizon = this.AgentTimeHorizon;
      this.obstacleTimeHorizon = this.ObstacleTimeHorizon;
      this.maxNeighbours = this.MaxNeighbours;
      this.desiredVelocity = this.DesiredVelocity;
      this.locked = this.Locked;
      this.collidesWith = this.CollidesWith;
      this.layer = this.Layer;
      this.Velocity = this.velocity;
      List<ObstacleVertex> list = this.obstaclesBuffered;
      this.obstaclesBuffered = this.obstacles;
      this.obstacles = list;
    }

    public void Update()
    {
      this.velocity = this.newVelocity;
      this.prevSmoothPos = this.smoothPos;
      this.position = this.prevSmoothPos;
      this.position = this.position + this.velocity * this.simulator.DeltaTime;
      this.Position = this.position;
    }

    public void Interpolate(float t)
    {
      this.smoothPos = this.prevSmoothPos + (this.Position - this.prevSmoothPos) * t;
    }

    public void CalculateNeighbours()
    {
      this.neighbours.Clear();
      this.neighbourDists.Clear();
      if (this.locked)
        return;
      float num1;
      if (this.MaxNeighbours > 0)
      {
        num1 = this.neighbourDist * this.neighbourDist;
        this.simulator.Quadtree.Query(new Vector2(this.position.x, this.position.z), this.neighbourDist, this);
      }
      this.obstacles.Clear();
      this.obstacleDists.Clear();
      float num2 = this.obstacleTimeHorizon * this.maxSpeed + this.radius;
      num1 = num2 * num2;
    }

    private float Sqr(float x)
    {
      return x * x;
    }

    public float InsertAgentNeighbour(Agent agent, float rangeSq)
    {
      if (this == agent || (agent.layer & this.collidesWith) == (RVOLayer) 0)
        return rangeSq;
      float num = this.Sqr(agent.position.x - this.position.x) + this.Sqr(agent.position.z - this.position.z);
      if ((double) num < (double) rangeSq)
      {
        if (this.neighbours.Count < this.maxNeighbours)
        {
          this.neighbours.Add(agent);
          this.neighbourDists.Add(num);
        }
        int index = this.neighbours.Count - 1;
        if ((double) num < (double) this.neighbourDists[index])
        {
          for (; index != 0 && (double) num < (double) this.neighbourDists[index - 1]; --index)
          {
            this.neighbours[index] = this.neighbours[index - 1];
            this.neighbourDists[index] = this.neighbourDists[index - 1];
          }
          this.neighbours[index] = agent;
          this.neighbourDists[index] = num;
        }
        if (this.neighbours.Count == this.maxNeighbours)
          rangeSq = this.neighbourDists[this.neighbourDists.Count - 1];
      }
      return rangeSq;
    }

    public void InsertObstacleNeighbour(ObstacleVertex ob1, float rangeSq)
    {
      ObstacleVertex obstacleVertex = ob1.next;
      float num = AstarMath.DistancePointSegmentStrict(ob1.position, obstacleVertex.position, this.Position);
      if ((double) num >= (double) rangeSq)
        return;
      this.obstacles.Add(ob1);
      this.obstacleDists.Add(num);
      int index;
      for (index = this.obstacles.Count - 1; index != 0 && (double) num < (double) this.obstacleDists[index - 1]; --index)
      {
        this.obstacles[index] = this.obstacles[index - 1];
        this.obstacleDists[index] = this.obstacleDists[index - 1];
      }
      this.obstacles[index] = ob1;
      this.obstacleDists[index] = num;
    }

    private static Vector3 To3D(Vector2 p)
    {
      return new Vector3(p.x, 0.0f, p.y);
    }

    private static void DrawCircle(Vector2 _p, float radius, Color col)
    {
      Agent.DrawCircle(_p, radius, 0.0f, 6.283185f, col);
    }

    private static void DrawCircle(Vector2 _p, float radius, float a0, float a1, Color col)
    {
      Vector3 vector3_1 = Agent.To3D(_p);
      while ((double) a0 > (double) a1)
        a0 -= 6.283185f;
      Vector3 vector3_2 = new Vector3(Mathf.Cos(a0) * radius, 0.0f, Mathf.Sin(a0) * radius);
      for (int index = 0; (double) index <= 40.0; ++index)
      {
        Vector3 vector3_3 = new Vector3(Mathf.Cos(Mathf.Lerp(a0, a1, (float) index / 40f)) * radius, 0.0f, Mathf.Sin(Mathf.Lerp(a0, a1, (float) index / 40f)) * radius);
        UnityEngine.Debug.DrawLine(vector3_1 + vector3_2, vector3_1 + vector3_3, col);
        vector3_2 = vector3_3;
      }
    }

    private static void DrawVO(Vector2 circleCenter, float radius, Vector2 origin)
    {
      float num1 = Mathf.Atan2((origin - circleCenter).y, (origin - circleCenter).x);
      float f = radius / (origin - circleCenter).magnitude;
      float num2 = (double) f > 1.0 ? 0.0f : Mathf.Abs(Mathf.Acos(f));
      Agent.DrawCircle(circleCenter, radius, num1 - num2, num1 + num2, Color.black);
      Vector2 vector2_1 = new Vector2(Mathf.Cos(num1 - num2), Mathf.Sin(num1 - num2)) * radius;
      Vector2 vector2_2 = new Vector2(Mathf.Cos(num1 + num2), Mathf.Sin(num1 + num2)) * radius;
      Vector2 p1 = -new Vector2(-vector2_1.y, vector2_1.x);
      Vector2 p2 = new Vector2(-vector2_2.y, vector2_2.x);
      Vector2 p3 = vector2_1 + circleCenter;
      Vector2 p4 = vector2_2 + circleCenter;
      UnityEngine.Debug.DrawRay(Agent.To3D(p3), Agent.To3D(p1).normalized * 100f, Color.black);
      UnityEngine.Debug.DrawRay(Agent.To3D(p4), Agent.To3D(p2).normalized * 100f, Color.black);
    }

    private static void DrawCross(Vector2 p, float size = 1)
    {
      Agent.DrawCross(p, Color.white, size);
    }

    private static void DrawCross(Vector2 p, Color col, float size = 1)
    {
      size *= 0.5f;
      UnityEngine.Debug.DrawLine(new Vector3(p.x, 0.0f, p.y) - Vector3.right * size, new Vector3(p.x, 0.0f, p.y) + Vector3.right * size, col);
      UnityEngine.Debug.DrawLine(new Vector3(p.x, 0.0f, p.y) - Vector3.forward * size, new Vector3(p.x, 0.0f, p.y) + Vector3.forward * size, col);
    }

    internal void CalculateVelocity(Simulator.WorkerContext context)
    {
      if (this.locked)
      {
        this.newVelocity = (Vector3) Vector2.zero;
      }
      else
      {
        if (context.vos.Length < this.neighbours.Count + this.simulator.obstacles.Count)
          context.vos = new Agent.VO[Mathf.Max(context.vos.Length * 2, this.neighbours.Count + this.simulator.obstacles.Count)];
        Vector2 vector2_1 = new Vector2(this.position.x, this.position.z);
        Agent.VO[] vos = context.vos;
        int voCount = 0;
        Vector2 vector2_2 = new Vector2(this.velocity.x, this.velocity.z);
        float inverseDt = 1f / this.agentTimeHorizon;
        float wallThickness = this.simulator.WallThickness;
        float weightFactor = this.simulator.algorithm != Simulator.SamplingAlgorithm.GradientDecent ? 5f : 1f;
        for (int index = 0; index < this.simulator.obstacles.Count; ++index)
        {
          ObstacleVertex obstacleVertex1 = this.simulator.obstacles[index];
          ObstacleVertex obstacleVertex2 = obstacleVertex1;
          do
          {
            if (obstacleVertex2.ignore || (double) this.position.y > (double) obstacleVertex2.position.y + (double) obstacleVertex2.height || ((double) this.position.y + (double) this.height < (double) obstacleVertex2.position.y || (obstacleVertex2.layer & this.collidesWith) == (RVOLayer) 0))
            {
              obstacleVertex2 = obstacleVertex2.next;
            }
            else
            {
              float f = Agent.VO.Det(new Vector2(obstacleVertex2.position.x, obstacleVertex2.position.z), obstacleVertex2.dir, vector2_1);
              float num = Vector2.Dot(obstacleVertex2.dir, vector2_1 - new Vector2(obstacleVertex2.position.x, obstacleVertex2.position.z));
              bool flag = (double) num <= (double) wallThickness * 0.0500000007450581 || (double) num >= (double) (new Vector2(obstacleVertex2.position.x, obstacleVertex2.position.z) - new Vector2(obstacleVertex2.next.position.x, obstacleVertex2.next.position.z)).magnitude - (double) wallThickness * 0.0500000007450581;
              if ((double) Mathf.Abs(f) < (double) this.neighbourDist)
              {
                if ((double) f <= 0.0 && !flag && (double) f > -(double) wallThickness)
                {
                  vos[voCount] = new Agent.VO(vector2_1, new Vector2(obstacleVertex2.position.x, obstacleVertex2.position.z) - vector2_1, obstacleVertex2.dir, weightFactor * 2f);
                  ++voCount;
                }
                else if ((double) f > 0.0)
                {
                  Vector2 p1 = new Vector2(obstacleVertex2.position.x, obstacleVertex2.position.z) - vector2_1;
                  Vector2 p2 = new Vector2(obstacleVertex2.next.position.x, obstacleVertex2.next.position.z) - vector2_1;
                  Vector2 normalized1 = p1.normalized;
                  Vector2 normalized2 = p2.normalized;
                  vos[voCount] = new Agent.VO(vector2_1, p1, p2, normalized1, normalized2, weightFactor);
                  ++voCount;
                }
              }
              obstacleVertex2 = obstacleVertex2.next;
            }
          }
          while (obstacleVertex2 != obstacleVertex1);
        }
        for (int index = 0; index < this.neighbours.Count; ++index)
        {
          Agent agent = this.neighbours[index];
          if (agent != this && (double) Math.Min(this.position.y + this.height, agent.position.y + agent.height) - (double) Math.Max(this.position.y, agent.position.y) >= 0.0)
          {
            Vector2 vector2_3 = new Vector2(agent.Velocity.x, agent.velocity.z);
            float radius = this.radius + agent.radius;
            Vector2 center = new Vector2(agent.position.x, agent.position.z) - vector2_1;
            Vector2 sideChooser = vector2_2 - vector2_3;
            Vector2 offset = !agent.locked ? (vector2_2 + vector2_3) * 0.5f : vector2_3;
            vos[voCount] = new Agent.VO(center, offset, radius, sideChooser, inverseDt, 1f);
            ++voCount;
            if (this.DebugDraw)
              Agent.DrawVO(vector2_1 + center * inverseDt + offset, radius * inverseDt, vector2_1 + offset);
          }
        }
        Vector2 zero1 = Vector2.zero;
        Vector2 vector;
        if (this.simulator.algorithm == Simulator.SamplingAlgorithm.GradientDecent)
        {
          if (this.DebugDraw)
          {
            for (int index1 = 0; index1 < 40; ++index1)
            {
              for (int index2 = 0; index2 < 40; ++index2)
              {
                Vector2 p1 = new Vector2((float) ((double) index1 * 15.0 / 40.0), (float) ((double) index2 * 15.0 / 40.0));
                Vector2 zero2 = Vector2.zero;
                float num = 0.0f;
                for (int index3 = 0; index3 < voCount; ++index3)
                {
                  float weight = 0.0f;
                  zero2 += vos[index3].Sample(p1 - vector2_1, out weight);
                  if ((double) weight > (double) num)
                    num = weight;
                }
                Vector2 vector2_3 = new Vector2(this.desiredVelocity.x, this.desiredVelocity.z) - p1 - vector2_1;
                Vector2 vector2_4 = zero2 + vector2_3 * Agent.DesiredVelocityScale;
                if ((double) vector2_3.magnitude * (double) Agent.DesiredVelocityWeight > (double) num)
                  num = vector2_3.magnitude * Agent.DesiredVelocityWeight;
                if ((double) num > 0.0)
                {
                  Vector2 vector2_5 = vector2_4 / num;
                }
                UnityEngine.Debug.DrawRay(Agent.To3D(p1), Agent.To3D(vector2_3 * 0.0f), Color.blue);
                float score = 0.0f;
                Vector2 p2 = p1 - Vector2.one * 15f * 0.5f;
                Vector2 vector2_6 = this.Trace(vos, voCount, p2, 0.01f, out score);
                if ((double) (p2 - vector2_6).sqrMagnitude < (double) this.Sqr(0.375f) * 2.59999990463257)
                  UnityEngine.Debug.DrawRay(Agent.To3D(vector2_6 + vector2_1), Vector3.up * 1f, Color.red);
              }
            }
          }
          float score1 = float.PositiveInfinity;
          float cutoff = new Vector2(this.velocity.x, this.velocity.z).magnitude * this.simulator.qualityCutoff;
          vector = this.Trace(vos, voCount, new Vector2(this.desiredVelocity.x, this.desiredVelocity.z), cutoff, out score1);
          if (this.DebugDraw)
            Agent.DrawCross(vector + vector2_1, Color.yellow, 0.5f);
          Vector2 p = (Vector2) this.Velocity;
          float score2;
          Vector2 vector2_7 = this.Trace(vos, voCount, p, cutoff, out score2);
          if ((double) score2 < (double) score1)
          {
            vector = vector2_7;
            score1 = score2;
          }
          if (this.DebugDraw)
            Agent.DrawCross(vector2_7 + vector2_1, Color.magenta, 0.5f);
        }
        else
        {
          Vector2[] vector2Array1 = context.samplePos;
          float[] numArray1 = context.sampleSize;
          int index1 = 0;
          Vector2 vector2_3 = new Vector2(this.desiredVelocity.x, this.desiredVelocity.z);
          float num1 = Mathf.Max(this.radius, Mathf.Max(vector2_3.magnitude, this.Velocity.magnitude));
          vector2Array1[index1] = vector2_3;
          numArray1[index1] = num1 * 0.3f;
          int index2 = index1 + 1;
          vector2Array1[index2] = vector2_2;
          numArray1[index2] = num1 * 0.3f;
          int index3 = index2 + 1;
          Vector2 vector2_4 = vector2_2 * 0.5f;
          Vector2 vector2_5 = new Vector2(vector2_4.y, -vector2_4.x);
          for (int index4 = 0; index4 < 8; ++index4)
          {
            vector2Array1[index3] = vector2_5 * Mathf.Sin((float) ((double) index4 * 3.14159274101257 * 2.0 / 8.0)) + vector2_4 * (1f + Mathf.Cos((float) ((double) index4 * 3.14159274101257 * 2.0 / 8.0)));
            numArray1[index3] = (float) ((1.0 - (double) Mathf.Abs((float) index4 - 4f) / 8.0) * (double) num1 * 0.5);
            ++index3;
          }
          Vector2 vector2_6 = vector2_4 * 0.6f;
          Vector2 vector2_7 = vector2_5 * 0.6f;
          for (int index4 = 0; index4 < 6; ++index4)
          {
            vector2Array1[index3] = vector2_7 * Mathf.Cos((float) (((double) index4 + 0.5) * 3.14159274101257 * 2.0 / 6.0)) + vector2_6 * (1.666667f + Mathf.Sin((float) (((double) index4 + 0.5) * 3.14159274101257 * 2.0 / 6.0)));
            numArray1[index3] = num1 * 0.3f;
            ++index3;
          }
          for (int index4 = 0; index4 < 6; ++index4)
          {
            vector2Array1[index3] = vector2_2 + new Vector2(num1 * 0.2f * Mathf.Cos((float) (((double) index4 + 0.5) * 3.14159274101257 * 2.0 / 6.0)), num1 * 0.2f * Mathf.Sin((float) (((double) index4 + 0.5) * 3.14159274101257 * 2.0 / 6.0)));
            numArray1[index3] = (float) ((double) num1 * 0.200000002980232 * 2.0);
            ++index3;
          }
          vector2Array1[index3] = vector2_2 * 0.5f;
          numArray1[index3] = num1 * 0.4f;
          int index5 = index3 + 1;
          Vector2[] vector2Array2 = context.bestPos;
          float[] numArray2 = context.bestSizes;
          float[] numArray3 = context.bestScores;
          for (int index4 = 0; index4 < 3; ++index4)
            numArray3[index4] = float.PositiveInfinity;
          numArray3[3] = float.NegativeInfinity;
          Vector2 vector2_8 = vector2_2;
          float num2 = float.PositiveInfinity;
          for (int index4 = 0; index4 < 3; ++index4)
          {
            for (int index6 = 0; index6 < index5; ++index6)
            {
              float val1 = 0.0f;
              for (int index7 = 0; index7 < voCount; ++index7)
                val1 = Math.Max(val1, vos[index7].ScalarSample(vector2Array1[index6]));
              float magnitude = (vector2Array1[index6] - vector2_3).magnitude;
              float num3 = val1 + magnitude * Agent.DesiredVelocityWeight;
              float num4 = val1 + magnitude * (1.0 / 1000.0);
              if (this.DebugDraw)
                Agent.DrawCross(vector2_1 + vector2Array1[index6], Agent.Rainbow(Mathf.Log(num4 + 1f) * 5f), numArray1[index6] * 0.5f);
              if ((double) num3 < (double) numArray3[0])
              {
                for (int index7 = 0; index7 < 3; ++index7)
                {
                  if ((double) num3 >= (double) numArray3[index7 + 1])
                  {
                    numArray3[index7] = num3;
                    numArray2[index7] = numArray1[index6];
                    vector2Array2[index7] = vector2Array1[index6];
                    break;
                  }
                }
              }
              if ((double) num4 < (double) num2)
              {
                vector2_8 = vector2Array1[index6];
                num2 = num4;
                if ((double) num4 == 0.0)
                {
                  index4 = 100;
                  break;
                }
              }
            }
            index5 = 0;
            for (int index6 = 0; index6 < 3; ++index6)
            {
              Vector2 vector2_9 = vector2Array2[index6];
              float num3 = numArray2[index6];
              numArray3[index6] = float.PositiveInfinity;
              float num4 = (float) ((double) num3 * 0.600000023841858 * 0.5);
              vector2Array1[index5] = vector2_9 + new Vector2(num4, num4);
              vector2Array1[index5 + 1] = vector2_9 + new Vector2(-num4, num4);
              vector2Array1[index5 + 2] = vector2_9 + new Vector2(-num4, -num4);
              vector2Array1[index5 + 3] = vector2_9 + new Vector2(num4, -num4);
              float num5 = num3 * (num3 * 0.6f);
              numArray1[index5] = num5;
              numArray1[index5 + 1] = num5;
              numArray1[index5 + 2] = num5;
              numArray1[index5 + 3] = num5;
              index5 += 4;
            }
          }
          vector = vector2_8;
        }
        if (this.DebugDraw)
          Agent.DrawCross(vector + vector2_1, 1f);
        this.newVelocity = Agent.To3D(Vector2.ClampMagnitude(vector, this.maxSpeed));
      }
    }

    private static Color Rainbow(float v)
    {
      Color color = new Color(v, 0.0f, 0.0f);
      if ((double) color.r > 1.0)
      {
        color.g = color.r - 1f;
        color.r = 1f;
      }
      if ((double) color.g > 1.0)
      {
        color.b = color.g - 1f;
        color.g = 1f;
      }
      return color;
    }

    private Vector2 Trace(Agent.VO[] vos, int voCount, Vector2 p, float cutoff, out float score)
    {
      score = 0.0f;
      float num1 = this.simulator.stepScale;
      float num2 = float.PositiveInfinity;
      Vector2 vector2_1 = p;
      for (int index1 = 0; index1 < 50; ++index1)
      {
        float num3 = (float) (1.0 - (double) index1 / 50.0) * num1;
        Vector2 zero = Vector2.zero;
        float val1 = 0.0f;
        for (int index2 = 0; index2 < voCount; ++index2)
        {
          float weight;
          Vector2 vector2_2 = vos[index2].Sample(p, out weight);
          zero += vector2_2;
          if ((double) weight > (double) val1)
            val1 = weight;
        }
        Vector2 vector2_3 = new Vector2(this.desiredVelocity.x, this.desiredVelocity.z) - p;
        float val2 = vector2_3.magnitude * Agent.DesiredVelocityWeight;
        Vector2 vector2_4 = zero + vector2_3 * Agent.DesiredVelocityScale;
        float num4 = Math.Max(val1, val2);
        score = num4;
        if ((double) score < (double) num2)
          num2 = score;
        vector2_1 = p;
        if ((double) score > (double) cutoff || index1 <= 10)
        {
          float sqrMagnitude = vector2_4.sqrMagnitude;
          if ((double) sqrMagnitude > 0.0)
            vector2_4 *= num4 / Mathf.Sqrt(sqrMagnitude);
          Vector2 vector2_2 = vector2_4 * num3;
          Vector2 p1 = p;
          p += vector2_2;
          if (this.DebugDraw)
            UnityEngine.Debug.DrawLine(Agent.To3D(p1) + this.position, Agent.To3D(p) + this.position, Agent.Rainbow(0.1f / score) * new Color(1f, 1f, 1f, 0.2f));
        }
        else
          break;
      }
      score = num2;
      return vector2_1;
    }

    public static bool IntersectionFactor(Vector2 start1, Vector2 dir1, Vector2 start2, Vector2 dir2, out float factor)
    {
      float num1 = (float) ((double) dir2.y * (double) dir1.x - (double) dir2.x * (double) dir1.y);
      if ((double) num1 == 0.0)
      {
        factor = 0.0f;
        return false;
      }
      float num2 = (float) ((double) dir2.x * ((double) start1.y - (double) start2.y) - (double) dir2.y * ((double) start1.x - (double) start2.x));
      factor = num2 / num1;
      return true;
    }

    public struct VO
    {
      public Vector2 origin;
      public Vector2 center;
      private Vector2 line1;
      private Vector2 line2;
      private Vector2 dir1;
      private Vector2 dir2;
      private Vector2 cutoffLine;
      private Vector2 cutoffDir;
      private float sqrCutoffDistance;
      private bool leftSide;
      private bool colliding;
      private float radius;
      private float weightFactor;

      public VO(Vector2 offset, Vector2 p0, Vector2 dir, float weightFactor)
      {
        this.colliding = true;
        this.line1 = p0;
        this.dir1 = -dir;
        this.origin = Vector2.zero;
        this.center = Vector2.zero;
        this.line2 = Vector2.zero;
        this.dir2 = Vector2.zero;
        this.cutoffLine = Vector2.zero;
        this.cutoffDir = Vector2.zero;
        this.sqrCutoffDistance = 0.0f;
        this.leftSide = false;
        this.radius = 0.0f;
        this.weightFactor = weightFactor * 0.5f;
      }

      public VO(Vector2 offset, Vector2 p1, Vector2 p2, Vector2 tang1, Vector2 tang2, float weightFactor)
      {
        this.weightFactor = weightFactor * 0.5f;
        this.colliding = false;
        this.cutoffLine = p1;
        this.cutoffDir = (p2 - p1).normalized;
        this.line1 = p1;
        this.dir1 = tang1;
        this.line2 = p2;
        this.dir2 = tang2;
        this.dir2 = -this.dir2;
        this.cutoffDir = -this.cutoffDir;
        this.origin = Vector2.zero;
        this.center = Vector2.zero;
        this.sqrCutoffDistance = 0.0f;
        this.leftSide = false;
        this.radius = 0.0f;
        weightFactor = 5f;
      }

      public VO(Vector2 center, Vector2 offset, float radius, Vector2 sideChooser, float inverseDt, float weightFactor)
      {
        this.weightFactor = weightFactor * 0.5f;
        this.origin = offset;
        weightFactor = 0.5f;
        if ((double) center.magnitude < (double) radius)
        {
          this.colliding = true;
          this.leftSide = false;
          this.line1 = center.normalized * (center.magnitude - radius);
          this.dir1 = new Vector2(this.line1.y, -this.line1.x).normalized;
          this.line1 += offset;
          this.cutoffDir = Vector2.zero;
          this.cutoffLine = Vector2.zero;
          this.sqrCutoffDistance = 0.0f;
          this.dir2 = Vector2.zero;
          this.line2 = Vector2.zero;
          this.center = Vector2.zero;
          this.radius = 0.0f;
        }
        else
        {
          this.colliding = false;
          center *= inverseDt;
          radius *= inverseDt;
          Vector2 vector2 = center + offset;
          this.sqrCutoffDistance = center.magnitude - radius;
          this.center = center;
          this.cutoffLine = center.normalized * this.sqrCutoffDistance;
          this.cutoffDir = new Vector2(-this.cutoffLine.y, this.cutoffLine.x).normalized;
          this.cutoffLine += offset;
          this.sqrCutoffDistance *= this.sqrCutoffDistance;
          float num1 = Mathf.Atan2(-center.y, -center.x);
          float num2 = Mathf.Abs(Mathf.Acos(radius / center.magnitude));
          this.radius = radius;
          this.leftSide = Polygon.Left(Vector2.zero, center, sideChooser);
          this.line1 = new Vector2(Mathf.Cos(num1 + num2), Mathf.Sin(num1 + num2)) * radius;
          this.dir1 = new Vector2(this.line1.y, -this.line1.x).normalized;
          this.line2 = new Vector2(Mathf.Cos(num1 - num2), Mathf.Sin(num1 - num2)) * radius;
          this.dir2 = new Vector2(this.line2.y, -this.line2.x).normalized;
          this.line1 += vector2;
          this.line2 += vector2;
        }
      }

      public static bool Left(Vector2 a, Vector2 dir, Vector2 p)
      {
        return (double) dir.x * ((double) p.y - (double) a.y) - ((double) p.x - (double) a.x) * (double) dir.y <= 0.0;
      }

      public static float Det(Vector2 a, Vector2 dir, Vector2 p)
      {
        return (float) (((double) p.x - (double) a.x) * (double) dir.y - (double) dir.x * ((double) p.y - (double) a.y));
      }

      public Vector2 Sample(Vector2 p, out float weight)
      {
        if (this.colliding)
        {
          float num = Agent.VO.Det(this.line1, this.dir1, p);
          if ((double) num >= 0.0)
          {
            weight = num * this.weightFactor;
            return new Vector2(-this.dir1.y, this.dir1.x) * weight * Agent.GlobalIncompressibility;
          }
          weight = 0.0f;
          return new Vector2(0.0f, 0.0f);
        }
        float num1 = Agent.VO.Det(this.cutoffLine, this.cutoffDir, p);
        if ((double) num1 <= 0.0)
        {
          weight = 0.0f;
          return Vector2.zero;
        }
        float num2 = Agent.VO.Det(this.line1, this.dir1, p);
        float num3 = Agent.VO.Det(this.line2, this.dir2, p);
        if ((double) num2 >= 0.0 && (double) num3 >= 0.0)
        {
          if (this.leftSide)
          {
            if ((double) num1 < (double) this.radius)
            {
              weight = num1 * this.weightFactor;
              return new Vector2(-this.cutoffDir.y, this.cutoffDir.x) * weight;
            }
            weight = num2;
            return new Vector2(-this.dir1.y, this.dir1.x) * weight;
          }
          if ((double) num1 < (double) this.radius)
          {
            weight = num1 * this.weightFactor;
            return new Vector2(-this.cutoffDir.y, this.cutoffDir.x) * weight;
          }
          weight = num3 * this.weightFactor;
          return new Vector2(-this.dir2.y, this.dir2.x) * weight;
        }
        weight = 0.0f;
        return new Vector2(0.0f, 0.0f);
      }

      public float ScalarSample(Vector2 p)
      {
        if (this.colliding)
        {
          float num = Agent.VO.Det(this.line1, this.dir1, p);
          if ((double) num >= 0.0)
            return num * Agent.GlobalIncompressibility * this.weightFactor;
          return 0.0f;
        }
        float num1 = Agent.VO.Det(this.cutoffLine, this.cutoffDir, p);
        if ((double) num1 <= 0.0)
          return 0.0f;
        float num2 = Agent.VO.Det(this.line1, this.dir1, p);
        float num3 = Agent.VO.Det(this.line2, this.dir2, p);
        if ((double) num2 < 0.0 || (double) num3 < 0.0)
          return 0.0f;
        if (this.leftSide)
        {
          if ((double) num1 < (double) this.radius)
            return num1 * this.weightFactor;
          return num2 * this.weightFactor;
        }
        if ((double) num1 < (double) this.radius)
          return num1 * this.weightFactor;
        return num3 * this.weightFactor;
      }
    }
  }
}
