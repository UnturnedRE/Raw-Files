// Decompiled with JetBrains decompiler
// Type: LightweightRVO
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding.RVO;
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (MeshFilter))]
public class LightweightRVO : MonoBehaviour
{
  public int agentCount = 100;
  public float exampleScale = 100f;
  public float radius = 3f;
  public float maxSpeed = 2f;
  public float agentTimeHorizon = 10f;
  [HideInInspector]
  public float obstacleTimeHorizon = 10f;
  public int maxNeighbours = 10;
  public float neighbourDist = 15f;
  public Vector3 renderingOffset = Vector3.up * 0.1f;
  public LightweightRVO.RVOExampleType type;
  public bool debug;
  private Mesh mesh;
  private Simulator sim;
  private List<IAgent> agents;
  private List<Vector3> goals;
  private List<Color> colors;
  private Vector3[] verts;
  private Vector2[] uv;
  private int[] tris;
  private Color[] meshColors;
  private Vector3[] interpolatedVelocities;

  public void Start()
  {
    this.mesh = new Mesh();
    RVOSimulator rvoSimulator = UnityEngine.Object.FindObjectOfType(typeof (RVOSimulator)) as RVOSimulator;
    if ((UnityEngine.Object) rvoSimulator == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "No RVOSimulator could be found in the scene. Please add a RVOSimulator component to any GameObject");
    }
    else
    {
      this.sim = rvoSimulator.GetSimulator();
      this.GetComponent<MeshFilter>().mesh = this.mesh;
      this.CreateAgents(this.agentCount);
    }
  }

  public void OnGUI()
  {
    if (GUILayout.Button("2"))
      this.CreateAgents(2);
    if (GUILayout.Button("10"))
      this.CreateAgents(10);
    if (GUILayout.Button("100"))
      this.CreateAgents(100);
    if (GUILayout.Button("500"))
      this.CreateAgents(500);
    if (GUILayout.Button("1000"))
      this.CreateAgents(1000);
    if (GUILayout.Button("5000"))
      this.CreateAgents(5000);
    GUILayout.Space(5f);
    if (GUILayout.Button("Random Streams"))
    {
      this.type = LightweightRVO.RVOExampleType.RandomStreams;
      this.CreateAgents(this.agents == null ? 100 : this.agents.Count);
    }
    if (GUILayout.Button("Line"))
    {
      this.type = LightweightRVO.RVOExampleType.Line;
      this.CreateAgents(this.agents == null ? 10 : Mathf.Min(this.agents.Count, 100));
    }
    if (GUILayout.Button("Circle"))
    {
      this.type = LightweightRVO.RVOExampleType.Circle;
      this.CreateAgents(this.agents == null ? 100 : this.agents.Count);
    }
    if (!GUILayout.Button("Point"))
      return;
    this.type = LightweightRVO.RVOExampleType.Point;
    this.CreateAgents(this.agents == null ? 100 : this.agents.Count);
  }

  private float uniformDistance(float radius)
  {
    float num = UnityEngine.Random.value + UnityEngine.Random.value;
    if ((double) num > 1.0)
      return radius * (2f - num);
    return radius * num;
  }

  public void CreateAgents(int num)
  {
    this.agentCount = num;
    this.agents = new List<IAgent>(this.agentCount);
    this.goals = new List<Vector3>(this.agentCount);
    this.colors = new List<Color>(this.agentCount);
    this.sim.ClearAgents();
    if (this.type == LightweightRVO.RVOExampleType.Circle)
    {
      float num1 = (float) ((double) Mathf.Sqrt((float) ((double) this.agentCount * (double) this.radius * (double) this.radius * 4.0 / 3.14159274101257)) * (double) this.exampleScale * 0.0500000007450581);
      for (int index = 0; index < this.agentCount; ++index)
      {
        Vector3 position = new Vector3(Mathf.Cos((float) ((double) index * 3.14159274101257 * 2.0) / (float) this.agentCount), 0.0f, Mathf.Sin((float) ((double) index * 3.14159274101257 * 2.0) / (float) this.agentCount)) * num1;
        this.agents.Add(this.sim.AddAgent(position));
        this.goals.Add(-position);
        this.colors.Add(LightweightRVO.HSVToRGB((float) index * 360f / (float) this.agentCount, 0.8f, 0.6f));
      }
    }
    else if (this.type == LightweightRVO.RVOExampleType.Line)
    {
      for (int index = 0; index < this.agentCount; ++index)
      {
        Vector3 position = new Vector3((index % 2 != 0 ? -1f : 1f) * this.exampleScale, 0.0f, (float) ((double) (index / 2) * (double) this.radius * 2.5));
        this.agents.Add(this.sim.AddAgent(position));
        this.goals.Add(new Vector3(-position.x, position.y, position.z));
        this.colors.Add(index % 2 != 0 ? Color.blue : Color.red);
      }
    }
    else if (this.type == LightweightRVO.RVOExampleType.Point)
    {
      for (int index = 0; index < this.agentCount; ++index)
      {
        Vector3 position = new Vector3(Mathf.Cos((float) ((double) index * 3.14159274101257 * 2.0) / (float) this.agentCount), 0.0f, Mathf.Sin((float) ((double) index * 3.14159274101257 * 2.0) / (float) this.agentCount)) * this.exampleScale;
        this.agents.Add(this.sim.AddAgent(position));
        this.goals.Add(new Vector3(0.0f, position.y, 0.0f));
        this.colors.Add(LightweightRVO.HSVToRGB((float) index * 360f / (float) this.agentCount, 0.8f, 0.6f));
      }
    }
    else if (this.type == LightweightRVO.RVOExampleType.RandomStreams)
    {
      float radius = (float) ((double) Mathf.Sqrt((float) ((double) this.agentCount * (double) this.radius * (double) this.radius * 4.0 / 3.14159274101257)) * (double) this.exampleScale * 0.0500000007450581);
      for (int index = 0; index < this.agentCount; ++index)
      {
        float f1 = (float) ((double) UnityEngine.Random.value * 3.14159274101257 * 2.0);
        float f2 = (float) ((double) UnityEngine.Random.value * 3.14159274101257 * 2.0);
        this.agents.Add(this.sim.AddAgent(new Vector3(Mathf.Cos(f1), 0.0f, Mathf.Sin(f1)) * this.uniformDistance(radius)));
        this.goals.Add(new Vector3(Mathf.Cos(f2), 0.0f, Mathf.Sin(f2)) * this.uniformDistance(radius));
        this.colors.Add(LightweightRVO.HSVToRGB(f2 * 57.29578f, 0.8f, 0.6f));
      }
    }
    for (int index = 0; index < this.agents.Count; ++index)
    {
      IAgent agent = this.agents[index];
      agent.Radius = this.radius;
      agent.MaxSpeed = this.maxSpeed;
      agent.AgentTimeHorizon = this.agentTimeHorizon;
      agent.ObstacleTimeHorizon = this.obstacleTimeHorizon;
      agent.MaxNeighbours = this.maxNeighbours;
      agent.NeighbourDist = this.neighbourDist;
      agent.DebugDraw = index == 0 && this.debug;
    }
    this.verts = new Vector3[4 * this.agents.Count];
    this.uv = new Vector2[this.verts.Length];
    this.tris = new int[this.agents.Count * 2 * 3];
    this.meshColors = new Color[this.verts.Length];
  }

  public void Update()
  {
    if (this.agents == null || (UnityEngine.Object) this.mesh == (UnityEngine.Object) null)
      return;
    if (this.agents.Count != this.goals.Count)
    {
      Debug.LogError((object) "Agent count does not match goal count");
    }
    else
    {
      for (int index = 0; index < this.agents.Count; ++index)
      {
        Vector3 interpolatedPosition = this.agents[index].InterpolatedPosition;
        Vector3 vector3 = Vector3.ClampMagnitude(this.goals[index] - interpolatedPosition, 1f);
        this.agents[index].DesiredVelocity = vector3 * this.agents[index].MaxSpeed;
      }
      if (this.interpolatedVelocities == null || this.interpolatedVelocities.Length < this.agents.Count)
      {
        Vector3[] vector3Array = new Vector3[this.agents.Count];
        if (this.interpolatedVelocities != null)
        {
          for (int index = 0; index < this.interpolatedVelocities.Length; ++index)
            vector3Array[index] = this.interpolatedVelocities[index];
        }
        this.interpolatedVelocities = vector3Array;
      }
      for (int index1 = 0; index1 < this.agents.Count; ++index1)
      {
        IAgent agent = this.agents[index1];
        this.interpolatedVelocities[index1] = Vector3.Lerp(this.interpolatedVelocities[index1], agent.Velocity, (float) ((double) agent.Velocity.magnitude * (double) Time.deltaTime * 4.0));
        Vector3 rhs = this.interpolatedVelocities[index1].normalized * agent.Radius;
        if (rhs == Vector3.zero)
          rhs = new Vector3(0.0f, 0.0f, agent.Radius);
        Vector3 vector3_1 = Vector3.Cross(Vector3.up, rhs);
        Vector3 vector3_2 = agent.InterpolatedPosition + this.renderingOffset;
        int index2 = 4 * index1;
        int index3 = 6 * index1;
        this.verts[index2] = vector3_2 + rhs - vector3_1;
        this.verts[index2 + 1] = vector3_2 + rhs + vector3_1;
        this.verts[index2 + 2] = vector3_2 - rhs + vector3_1;
        this.verts[index2 + 3] = vector3_2 - rhs - vector3_1;
        this.uv[index2] = new Vector2(0.0f, 1f);
        this.uv[index2 + 1] = new Vector2(1f, 1f);
        this.uv[index2 + 2] = new Vector2(1f, 0.0f);
        this.uv[index2 + 3] = new Vector2(0.0f, 0.0f);
        this.meshColors[index2] = this.colors[index1];
        this.meshColors[index2 + 1] = this.colors[index1];
        this.meshColors[index2 + 2] = this.colors[index1];
        this.meshColors[index2 + 3] = this.colors[index1];
        this.tris[index3] = index2;
        this.tris[index3 + 1] = index2 + 1;
        this.tris[index3 + 2] = index2 + 2;
        this.tris[index3 + 3] = index2;
        this.tris[index3 + 4] = index2 + 2;
        this.tris[index3 + 5] = index2 + 3;
      }
      this.mesh.Clear();
      this.mesh.vertices = this.verts;
      this.mesh.uv = this.uv;
      this.mesh.colors = this.meshColors;
      this.mesh.triangles = this.tris;
      this.mesh.RecalculateNormals();
    }
  }

  private static Color HSVToRGB(float h, float s, float v)
  {
    float num1 = 0.0f;
    float num2 = 0.0f;
    float num3 = 0.0f;
    float num4 = s * v;
    float num5 = h / 60f;
    float num6 = num4 * (1f - Math.Abs((float) ((double) num5 % 2.0 - 1.0)));
    if ((double) num5 < 1.0)
    {
      num1 = num4;
      num2 = num6;
    }
    else if ((double) num5 < 2.0)
    {
      num1 = num6;
      num2 = num4;
    }
    else if ((double) num5 < 3.0)
    {
      num2 = num4;
      num3 = num6;
    }
    else if ((double) num5 < 4.0)
    {
      num2 = num6;
      num3 = num4;
    }
    else if ((double) num5 < 5.0)
    {
      num1 = num6;
      num3 = num4;
    }
    else if ((double) num5 < 6.0)
    {
      num1 = num4;
      num3 = num6;
    }
    float num7 = v - num4;
    return new Color(num1 + num7, num2 + num7, num3 + num7);
  }

  public enum RVOExampleType
  {
    Circle,
    Line,
    Point,
    RandomStreams,
  }
}
