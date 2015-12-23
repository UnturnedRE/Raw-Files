// Decompiled with JetBrains decompiler
// Type: Pathfinding.RVO.Simulator
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding.RVO.Sampled;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Pathfinding.RVO
{
  public class Simulator
  {
    private bool doubleBuffering = true;
    private float desiredDeltaTime = 0.05f;
    private bool interpolation = true;
    private RVOQuadtree quadtree = new RVOQuadtree();
    public float qualityCutoff = 0.05f;
    public float stepScale = 1.5f;
    private float lastStep = -99999f;
    private float lastStepInterpolationReference = -9999f;
    private float wallThickness = 1f;
    private Simulator.WorkerContext coroutineWorkerContext = new Simulator.WorkerContext();
    private Simulator.Worker[] workers;
    private List<Agent> agents;
    public List<ObstacleVertex> obstacles;
    public Simulator.SamplingAlgorithm algorithm;
    private float deltaTime;
    private float prevDeltaTime;
    private bool doUpdateObstacles;
    private bool doCleanObstacles;
    private bool oversampling;

    public RVOQuadtree Quadtree
    {
      get
      {
        return this.quadtree;
      }
    }

    public float DeltaTime
    {
      get
      {
        return this.deltaTime;
      }
    }

    public float PrevDeltaTime
    {
      get
      {
        return this.prevDeltaTime;
      }
    }

    public bool Multithreading
    {
      get
      {
        if (this.workers != null)
          return this.workers.Length > 0;
        return false;
      }
    }

    public float DesiredDeltaTime
    {
      get
      {
        return this.desiredDeltaTime;
      }
      set
      {
        this.desiredDeltaTime = Math.Max(value, 0.0f);
      }
    }

    public float WallThickness
    {
      get
      {
        return this.wallThickness;
      }
      set
      {
        this.wallThickness = Math.Max(this.wallThickness, 0.0f);
      }
    }

    public bool Interpolation
    {
      get
      {
        return this.interpolation;
      }
      set
      {
        this.interpolation = value;
      }
    }

    public bool Oversampling
    {
      get
      {
        return this.oversampling;
      }
      set
      {
        this.oversampling = value;
      }
    }

    public Simulator(int workers, bool doubleBuffering)
    {
      this.workers = new Simulator.Worker[workers];
      this.doubleBuffering = doubleBuffering;
      for (int index = 0; index < workers; ++index)
        this.workers[index] = new Simulator.Worker(this);
      this.agents = new List<Agent>();
      this.obstacles = new List<ObstacleVertex>();
    }

    ~Simulator()
    {
      this.OnDestroy();
    }

    public List<Agent> GetAgents()
    {
      return this.agents;
    }

    public List<ObstacleVertex> GetObstacles()
    {
      return this.obstacles;
    }

    public void ClearAgents()
    {
      if (this.Multithreading && this.doubleBuffering)
      {
        for (int index = 0; index < this.workers.Length; ++index)
          this.workers[index].WaitOne();
      }
      for (int index = 0; index < this.agents.Count; ++index)
        this.agents[index].simulator = (Simulator) null;
      this.agents.Clear();
    }

    public void OnDestroy()
    {
      if (this.workers == null)
        return;
      for (int index = 0; index < this.workers.Length; ++index)
        this.workers[index].Terminate();
    }

    public IAgent AddAgent(IAgent agent)
    {
      if (agent == null)
        throw new ArgumentNullException("Agent must not be null");
      Agent agent1 = agent as Agent;
      if (agent1 == null)
        throw new ArgumentException("The agent must be of type Agent. Agent was of type " + (object) agent.GetType());
      if (agent1.simulator != null && agent1.simulator == this)
        throw new ArgumentException("The agent is already in the simulation");
      if (agent1.simulator != null)
        throw new ArgumentException("The agent is already added to another simulation");
      agent1.simulator = this;
      if (this.Multithreading && this.doubleBuffering)
      {
        for (int index = 0; index < this.workers.Length; ++index)
          this.workers[index].WaitOne();
      }
      this.agents.Add(agent1);
      return agent;
    }

    public IAgent AddAgent(Vector3 position)
    {
      Agent agent = new Agent(position);
      if (this.Multithreading && this.doubleBuffering)
      {
        for (int index = 0; index < this.workers.Length; ++index)
          this.workers[index].WaitOne();
      }
      this.agents.Add(agent);
      agent.simulator = this;
      return (IAgent) agent;
    }

    public void RemoveAgent(IAgent agent)
    {
      if (agent == null)
        throw new ArgumentNullException("Agent must not be null");
      Agent agent1 = agent as Agent;
      if (agent1 == null)
        throw new ArgumentException("The agent must be of type Agent. Agent was of type " + (object) agent.GetType());
      if (agent1.simulator != this)
        throw new ArgumentException("The agent is not added to this simulation");
      if (this.Multithreading && this.doubleBuffering)
      {
        for (int index = 0; index < this.workers.Length; ++index)
          this.workers[index].WaitOne();
      }
      agent1.simulator = (Simulator) null;
      if (!this.agents.Remove(agent1))
        throw new ArgumentException("Critical Bug! This should not happen. Please report this.");
    }

    public ObstacleVertex AddObstacle(ObstacleVertex v)
    {
      if (v == null)
        throw new ArgumentNullException("Obstacle must not be null");
      if (this.Multithreading && this.doubleBuffering)
      {
        for (int index = 0; index < this.workers.Length; ++index)
          this.workers[index].WaitOne();
      }
      this.obstacles.Add(v);
      this.UpdateObstacles();
      return v;
    }

    public ObstacleVertex AddObstacle(Vector3[] vertices, float height)
    {
      return this.AddObstacle(vertices, height, Matrix4x4.identity, RVOLayer.DefaultObstacle);
    }

    public ObstacleVertex AddObstacle(Vector3[] vertices, float height, Matrix4x4 matrix, RVOLayer layer = RVOLayer.DefaultObstacle)
    {
      if (vertices == null)
        throw new ArgumentNullException("Vertices must not be null");
      if (vertices.Length < 2)
        throw new ArgumentException("Less than 2 vertices in an obstacle");
      ObstacleVertex obstacleVertex1 = (ObstacleVertex) null;
      ObstacleVertex obstacleVertex2 = (ObstacleVertex) null;
      bool flag = matrix == Matrix4x4.identity;
      if (this.Multithreading && this.doubleBuffering)
      {
        for (int index = 0; index < this.workers.Length; ++index)
          this.workers[index].WaitOne();
      }
      for (int index = 0; index < vertices.Length; ++index)
      {
        ObstacleVertex obstacleVertex3 = new ObstacleVertex();
        if (obstacleVertex1 == null)
          obstacleVertex1 = obstacleVertex3;
        else
          obstacleVertex2.next = obstacleVertex3;
        obstacleVertex3.prev = obstacleVertex2;
        obstacleVertex3.layer = layer;
        obstacleVertex3.position = !flag ? matrix.MultiplyPoint3x4(vertices[index]) : vertices[index];
        obstacleVertex3.height = height;
        obstacleVertex2 = obstacleVertex3;
      }
      obstacleVertex2.next = obstacleVertex1;
      obstacleVertex1.prev = obstacleVertex2;
      ObstacleVertex obstacleVertex4 = obstacleVertex1;
      do
      {
        Vector3 vector3 = obstacleVertex4.next.position - obstacleVertex4.position;
        obstacleVertex4.dir = new Vector2(vector3.x, vector3.z).normalized;
        obstacleVertex4 = obstacleVertex4.next;
      }
      while (obstacleVertex4 != obstacleVertex1);
      this.obstacles.Add(obstacleVertex1);
      this.UpdateObstacles();
      return obstacleVertex1;
    }

    public ObstacleVertex AddObstacle(Vector3 a, Vector3 b, float height)
    {
      ObstacleVertex obstacleVertex1 = new ObstacleVertex();
      ObstacleVertex obstacleVertex2 = new ObstacleVertex();
      obstacleVertex1.prev = obstacleVertex2;
      obstacleVertex2.prev = obstacleVertex1;
      obstacleVertex1.next = obstacleVertex2;
      obstacleVertex2.next = obstacleVertex1;
      obstacleVertex1.position = a;
      obstacleVertex2.position = b;
      obstacleVertex1.height = height;
      obstacleVertex2.height = height;
      obstacleVertex2.ignore = true;
      obstacleVertex1.dir = new Vector2(b.x - a.x, b.z - a.z).normalized;
      obstacleVertex2.dir = -obstacleVertex1.dir;
      if (this.Multithreading && this.doubleBuffering)
      {
        for (int index = 0; index < this.workers.Length; ++index)
          this.workers[index].WaitOne();
      }
      this.obstacles.Add(obstacleVertex1);
      this.UpdateObstacles();
      return obstacleVertex1;
    }

    public void UpdateObstacle(ObstacleVertex obstacle, Vector3[] vertices, Matrix4x4 matrix)
    {
      if (vertices == null)
        throw new ArgumentNullException("Vertices must not be null");
      if (obstacle == null)
        throw new ArgumentNullException("Obstacle must not be null");
      if (vertices.Length < 2)
        throw new ArgumentException("Less than 2 vertices in an obstacle");
      if (obstacle.split)
        throw new ArgumentException("Obstacle is not a start vertex. You should only pass those ObstacleVertices got from AddObstacle method calls");
      if (this.Multithreading && this.doubleBuffering)
      {
        for (int index = 0; index < this.workers.Length; ++index)
          this.workers[index].WaitOne();
      }
      int index1 = 0;
      ObstacleVertex obstacleVertex1 = obstacle;
      do
      {
        while (obstacleVertex1.next.split)
        {
          obstacleVertex1.next = obstacleVertex1.next.next;
          obstacleVertex1.next.prev = obstacleVertex1;
        }
        if (index1 >= vertices.Length)
        {
          Debug.DrawLine(obstacleVertex1.prev.position, obstacleVertex1.position, Color.red);
          throw new ArgumentException("Obstacle has more vertices than supplied for updating (" + (object) vertices.Length + " supplied)");
        }
        obstacleVertex1.position = matrix.MultiplyPoint3x4(vertices[index1]);
        ++index1;
        obstacleVertex1 = obstacleVertex1.next;
      }
      while (obstacleVertex1 != obstacle);
      ObstacleVertex obstacleVertex2 = obstacle;
      do
      {
        Vector3 vector3 = obstacleVertex2.next.position - obstacleVertex2.position;
        obstacleVertex2.dir = new Vector2(vector3.x, vector3.z).normalized;
        obstacleVertex2 = obstacleVertex2.next;
      }
      while (obstacleVertex2 != obstacle);
      this.ScheduleCleanObstacles();
      this.UpdateObstacles();
    }

    private void ScheduleCleanObstacles()
    {
      this.doCleanObstacles = true;
    }

    private void CleanObstacles()
    {
      for (int index = 0; index < this.obstacles.Count; ++index)
      {
        ObstacleVertex obstacleVertex1 = this.obstacles[index];
        ObstacleVertex obstacleVertex2 = obstacleVertex1;
        do
        {
          while (obstacleVertex2.next.split)
          {
            obstacleVertex2.next = obstacleVertex2.next.next;
            obstacleVertex2.next.prev = obstacleVertex2;
          }
          obstacleVertex2 = obstacleVertex2.next;
        }
        while (obstacleVertex2 != obstacleVertex1);
      }
    }

    public void RemoveObstacle(ObstacleVertex v)
    {
      if (v == null)
        throw new ArgumentNullException("Vertex must not be null");
      if (this.Multithreading && this.doubleBuffering)
      {
        for (int index = 0; index < this.workers.Length; ++index)
          this.workers[index].WaitOne();
      }
      this.obstacles.Remove(v);
      this.UpdateObstacles();
    }

    public void UpdateObstacles()
    {
      this.doUpdateObstacles = true;
    }

    private void BuildQuadtree()
    {
      this.quadtree.Clear();
      if (this.agents.Count <= 0)
        return;
      Rect r = Rect.MinMaxRect(this.agents[0].position.x, this.agents[0].position.y, this.agents[0].position.x, this.agents[0].position.y);
      for (int index = 1; index < this.agents.Count; ++index)
      {
        Vector3 vector3 = this.agents[index].position;
        r = Rect.MinMaxRect(Mathf.Min(r.xMin, vector3.x), Mathf.Min(r.yMin, vector3.z), Mathf.Max(r.xMax, vector3.x), Mathf.Max(r.yMax, vector3.z));
      }
      this.quadtree.SetBounds(r);
      for (int index = 0; index < this.agents.Count; ++index)
        this.quadtree.Insert(this.agents[index]);
    }

    public void Update()
    {
      if ((double) this.lastStep < 0.0)
      {
        this.lastStep = Time.time;
        this.deltaTime = this.DesiredDeltaTime;
        this.prevDeltaTime = this.deltaTime;
        this.lastStepInterpolationReference = this.lastStep;
      }
      if ((double) Time.time - (double) this.lastStep >= (double) this.DesiredDeltaTime)
      {
        if (!this.doubleBuffering)
        {
          for (int index = 0; index < this.agents.Count; ++index)
            this.agents[index].Interpolate((Time.time - this.lastStepInterpolationReference) / this.DeltaTime);
        }
        this.lastStepInterpolationReference = Time.time;
        this.prevDeltaTime = this.DeltaTime;
        this.deltaTime = Time.time - this.lastStep;
        this.lastStep = Time.time;
        this.deltaTime = Math.Max(this.deltaTime, 0.0005f);
        if (this.Multithreading)
        {
          if (this.doubleBuffering)
          {
            for (int index = 0; index < this.workers.Length; ++index)
              this.workers[index].WaitOne();
            if (!this.Interpolation)
            {
              for (int index = 0; index < this.agents.Count; ++index)
                this.agents[index].Interpolate(1f);
            }
          }
          if (this.doCleanObstacles)
          {
            this.CleanObstacles();
            this.doCleanObstacles = false;
            this.doUpdateObstacles = true;
          }
          if (this.doUpdateObstacles)
            this.doUpdateObstacles = false;
          this.BuildQuadtree();
          for (int index = 0; index < this.workers.Length; ++index)
          {
            this.workers[index].start = index * this.agents.Count / this.workers.Length;
            this.workers[index].end = (index + 1) * this.agents.Count / this.workers.Length;
          }
          for (int index = 0; index < this.workers.Length; ++index)
            this.workers[index].Execute(1);
          for (int index = 0; index < this.workers.Length; ++index)
            this.workers[index].WaitOne();
          for (int index = 0; index < this.workers.Length; ++index)
            this.workers[index].Execute(0);
          if (!this.doubleBuffering)
          {
            for (int index = 0; index < this.workers.Length; ++index)
              this.workers[index].WaitOne();
            if (!this.Interpolation)
            {
              for (int index = 0; index < this.agents.Count; ++index)
                this.agents[index].Interpolate(1f);
            }
          }
        }
        else
        {
          if (this.doCleanObstacles)
          {
            this.CleanObstacles();
            this.doCleanObstacles = false;
            this.doUpdateObstacles = true;
          }
          if (this.doUpdateObstacles)
            this.doUpdateObstacles = false;
          this.BuildQuadtree();
          for (int index = 0; index < this.agents.Count; ++index)
          {
            this.agents[index].Update();
            this.agents[index].BufferSwitch();
          }
          for (int index = 0; index < this.agents.Count; ++index)
          {
            this.agents[index].CalculateNeighbours();
            this.agents[index].CalculateVelocity(this.coroutineWorkerContext);
          }
          if (this.oversampling)
          {
            for (int index = 0; index < this.agents.Count; ++index)
              this.agents[index].Velocity = this.agents[index].newVelocity;
            for (int index = 0; index < this.agents.Count; ++index)
            {
              Vector3 vector3 = this.agents[index].newVelocity;
              this.agents[index].CalculateVelocity(this.coroutineWorkerContext);
              this.agents[index].newVelocity = (vector3 + this.agents[index].newVelocity) * 0.5f;
            }
          }
          if (!this.Interpolation)
          {
            for (int index = 0; index < this.agents.Count; ++index)
              this.agents[index].Interpolate(1f);
          }
        }
      }
      if (!this.Interpolation)
        return;
      for (int index = 0; index < this.agents.Count; ++index)
        this.agents[index].Interpolate((Time.time - this.lastStepInterpolationReference) / this.DeltaTime);
    }

    public enum SamplingAlgorithm
    {
      AdaptiveSampling,
      GradientDecent,
    }

    internal class WorkerContext
    {
      public Agent.VO[] vos = new Agent.VO[20];
      public Vector2[] bestPos = new Vector2[3];
      public float[] bestSizes = new float[3];
      public float[] bestScores = new float[4];
      public Vector2[] samplePos = new Vector2[50];
      public float[] sampleSize = new float[50];
      public const int KeepCount = 3;
    }

    private class Worker
    {
      public AutoResetEvent runFlag = new AutoResetEvent(false);
      public ManualResetEvent waitFlag = new ManualResetEvent(true);
      private Simulator.WorkerContext context = new Simulator.WorkerContext();
      [NonSerialized]
      public Thread thread;
      public int start;
      public int end;
      public int task;
      public Simulator simulator;
      private bool terminate;

      public Worker(Simulator sim)
      {
        this.simulator = sim;
        this.thread = new Thread(new ThreadStart(this.Run));
        this.thread.IsBackground = true;
        this.thread.Name = "RVO Simulator Thread";
        this.thread.Start();
      }

      public void Execute(int task)
      {
        this.task = task;
        this.waitFlag.Reset();
        this.runFlag.Set();
      }

      public void WaitOne()
      {
        this.waitFlag.WaitOne();
      }

      public void Terminate()
      {
        this.terminate = true;
      }

      public void Run()
      {
        this.runFlag.WaitOne();
        while (!this.terminate)
        {
          try
          {
            List<Agent> agents = this.simulator.GetAgents();
            if (this.task == 0)
            {
              for (int index = this.start; index < this.end; ++index)
              {
                agents[index].CalculateNeighbours();
                agents[index].CalculateVelocity(this.context);
              }
            }
            else if (this.task == 1)
            {
              for (int index = this.start; index < this.end; ++index)
              {
                agents[index].Update();
                agents[index].BufferSwitch();
              }
            }
            else if (this.task == 2)
            {
              this.simulator.BuildQuadtree();
            }
            else
            {
              Debug.LogError((object) ("Invalid Task Number: " + (object) this.task));
              throw new Exception("Invalid Task Number: " + (object) this.task);
            }
          }
          catch (Exception ex)
          {
            Debug.LogError((object) ex);
          }
          this.waitFlag.Set();
          this.runFlag.WaitOne();
        }
      }
    }
  }
}
