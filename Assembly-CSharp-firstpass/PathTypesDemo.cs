// Decompiled with JetBrains decompiler
// Type: PathTypesDemo
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding;
using Pathfinding.Util;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PathTypesDemo : MonoBehaviour
{
  public int searchLength = 1000;
  public int spread = 100;
  private List<GameObject> lastRender = new List<GameObject>();
  private List<Vector3> multipoints = new List<Vector3>();
  public int activeDemo;
  public Transform start;
  public Transform end;
  public Vector3 pathOffset;
  public Material lineMat;
  public Material squareMat;
  public float lineWidth;
  public RichAI[] agents;
  public float aimStrength;
  private Path lastPath;
  private Vector2 mouseDragStart;
  private float mouseDragStartTime;
  private FloodPath lastFlood;

  private void Update()
  {
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    Vector3 vector3 = ray.origin + ray.direction * (ray.origin.y / -ray.direction.y);
    this.end.position = vector3;
    if (Input.GetMouseButtonDown(0))
    {
      this.mouseDragStart = (Vector2) Input.mousePosition;
      this.mouseDragStartTime = Time.realtimeSinceStartup;
    }
    if (Input.GetMouseButtonUp(0))
    {
      Vector2 vector2 = (Vector2) Input.mousePosition;
      if ((double) (vector2 - this.mouseDragStart).sqrMagnitude > 25.0 && (double) Time.realtimeSinceStartup - (double) this.mouseDragStartTime > 0.300000011920929)
      {
        Rect rect = Rect.MinMaxRect(Mathf.Min(this.mouseDragStart.x, vector2.x), Mathf.Min(this.mouseDragStart.y, vector2.y), Mathf.Max(this.mouseDragStart.x, vector2.x), Mathf.Max(this.mouseDragStart.y, vector2.y));
        RichAI[] richAiArray = UnityEngine.Object.FindObjectsOfType(typeof (RichAI)) as RichAI[];
        List<RichAI> list = new List<RichAI>();
        for (int index = 0; index < richAiArray.Length; ++index)
        {
          Vector2 point = (Vector2) Camera.main.WorldToScreenPoint(richAiArray[index].transform.position);
          if (rect.Contains(point))
            list.Add(richAiArray[index]);
        }
        this.agents = list.ToArray();
      }
      else
      {
        if (Input.GetKey(KeyCode.LeftShift))
          this.multipoints.Add(vector3);
        if (Input.GetKey(KeyCode.LeftControl))
          this.multipoints.Clear();
        if ((double) Input.mousePosition.x > 225.0)
          this.DemoPath();
      }
    }
    if (!Input.GetMouseButton(0) || !Input.GetKey(KeyCode.LeftAlt) || !this.lastPath.IsDone())
      return;
    this.DemoPath();
  }

  public void OnGUI()
  {
    GUILayout.BeginArea(new Rect(5f, 5f, 220f, (float) (Screen.height - 10)), string.Empty, (GUIStyle) "Box");
    switch (this.activeDemo)
    {
      case 0:
        GUILayout.Label("Basic path. Finds a path from point A to point B.");
        break;
      case 1:
        GUILayout.Label("Multi Target Path. Finds a path quickly from one point to many others in a single search.");
        break;
      case 2:
        GUILayout.Label("Randomized Path. Finds a path with a specified length in a random direction or biased towards some point when using a larger aim strenggth.");
        break;
      case 3:
        GUILayout.Label("Flee Path. Tries to flee from a specified point. Remember to set Flee Strength!");
        break;
      case 4:
        GUILayout.Label("Finds all nodes which it costs less than some value to reach.");
        break;
      case 5:
        GUILayout.Label("Searches the whole graph from a specific point. FloodPathTracer can then be used to quickly find a path to that point");
        break;
      case 6:
        GUILayout.Label("Traces a path to where the FloodPath started. Compare the claculation times for this path with ABPath!\nGreat for TD games");
        break;
    }
    GUILayout.Space(5f);
    GUILayout.Label("Note that the paths are rendered without ANY post-processing applied, so they might look a bit edgy");
    GUILayout.Space(5f);
    GUILayout.Label("Click anywhere to recalculate the path. Hold Alt to continuously recalculate the path while the mouse is pressed.");
    if (this.activeDemo == 2 || this.activeDemo == 3 || this.activeDemo == 4)
    {
      GUILayout.Label("Search Distance (" + (object) this.searchLength + ")");
      this.searchLength = Mathf.RoundToInt(GUILayout.HorizontalSlider((float) this.searchLength, 0.0f, 100000f));
    }
    if (this.activeDemo == 2 || this.activeDemo == 3)
    {
      GUILayout.Label("Spread (" + (object) this.spread + ")");
      this.spread = Mathf.RoundToInt(GUILayout.HorizontalSlider((float) this.spread, 0.0f, 40000f));
      GUILayout.Label(string.Concat(new object[4]
      {
        (object) (this.activeDemo != 2 ? "Flee strength" : "Aim strength"),
        (object) " (",
        (object) this.aimStrength,
        (object) ")"
      }));
      this.aimStrength = GUILayout.HorizontalSlider(this.aimStrength, 0.0f, 1f);
    }
    if (this.activeDemo == 1)
      GUILayout.Label("Hold shift and click to add new target points. Hold ctr and click to remove all target points");
    if (GUILayout.Button("A to B path"))
      this.activeDemo = 0;
    if (GUILayout.Button("Multi Target Path"))
      this.activeDemo = 1;
    if (GUILayout.Button("Random Path"))
      this.activeDemo = 2;
    if (GUILayout.Button("Flee path"))
      this.activeDemo = 3;
    if (GUILayout.Button("Constant Path"))
      this.activeDemo = 4;
    if (GUILayout.Button("Flood Path"))
      this.activeDemo = 5;
    if (GUILayout.Button("Flood Path Tracer"))
      this.activeDemo = 6;
    GUILayout.EndArea();
  }

  public void OnPathComplete(Path p)
  {
    if (this.lastRender == null)
      return;
    if (p.error)
      this.ClearPrevious();
    else if (p.GetType() == typeof (MultiTargetPath))
    {
      List<GameObject> list1 = new List<GameObject>((IEnumerable<GameObject>) this.lastRender);
      this.lastRender.Clear();
      MultiTargetPath multiTargetPath = p as MultiTargetPath;
      for (int index1 = 0; index1 < multiTargetPath.vectorPaths.Length; ++index1)
      {
        if (multiTargetPath.vectorPaths[index1] != null)
        {
          List<Vector3> list2 = multiTargetPath.vectorPaths[index1];
          GameObject gameObject;
          if (list1.Count > index1 && (UnityEngine.Object) list1[index1].GetComponent<LineRenderer>() != (UnityEngine.Object) null)
          {
            gameObject = list1[index1];
            list1.RemoveAt(index1);
          }
          else
            gameObject = new GameObject("LineRenderer_" + (object) index1, new System.Type[1]
            {
              typeof (LineRenderer)
            });
          LineRenderer component = gameObject.GetComponent<LineRenderer>();
          component.sharedMaterial = this.lineMat;
          component.SetWidth(this.lineWidth, this.lineWidth);
          component.SetVertexCount(list2.Count);
          for (int index2 = 0; index2 < list2.Count; ++index2)
            component.SetPosition(index2, list2[index2] + this.pathOffset);
          this.lastRender.Add(gameObject);
        }
      }
      for (int index = 0; index < list1.Count; ++index)
        UnityEngine.Object.Destroy((UnityEngine.Object) list1[index]);
    }
    else if (p.GetType() == typeof (ConstantPath))
    {
      this.ClearPrevious();
      List<GraphNode> list1 = (p as ConstantPath).allNodes;
      Mesh mesh = new Mesh();
      List<Vector3> list2 = new List<Vector3>();
      bool flag = false;
      for (int index = list1.Count - 1; index >= 0; --index)
      {
        Vector3 start = (Vector3) list1[index].position + this.pathOffset;
        if (list2.Count == 65000 && !flag)
        {
          UnityEngine.Debug.LogError((object) "Too many nodes, rendering a mesh would throw 65K vertex error. Using Debug.DrawRay instead for the rest of the nodes");
          flag = true;
        }
        if (flag)
        {
          UnityEngine.Debug.DrawRay(start, Vector3.up, Color.blue);
        }
        else
        {
          GridGraph gridGraph = AstarData.GetGraph(list1[index]) as GridGraph;
          float num = 1f;
          if (gridGraph != null)
            num = gridGraph.nodeSize;
          list2.Add(start + new Vector3(-0.5f, 0.0f, -0.5f) * num);
          list2.Add(start + new Vector3(0.5f, 0.0f, -0.5f) * num);
          list2.Add(start + new Vector3(-0.5f, 0.0f, 0.5f) * num);
          list2.Add(start + new Vector3(0.5f, 0.0f, 0.5f) * num);
        }
      }
      Vector3[] vector3Array = list2.ToArray();
      int[] numArray = new int[3 * vector3Array.Length / 2];
      int num1 = 0;
      int index1 = 0;
      while (num1 < vector3Array.Length)
      {
        numArray[index1] = num1;
        numArray[index1 + 1] = num1 + 1;
        numArray[index1 + 2] = num1 + 2;
        numArray[index1 + 3] = num1 + 1;
        numArray[index1 + 4] = num1 + 3;
        numArray[index1 + 5] = num1 + 2;
        index1 += 6;
        num1 += 4;
      }
      Vector2[] vector2Array = new Vector2[vector3Array.Length];
      int index2 = 0;
      while (index2 < vector2Array.Length)
      {
        vector2Array[index2] = new Vector2(0.0f, 0.0f);
        vector2Array[index2 + 1] = new Vector2(1f, 0.0f);
        vector2Array[index2 + 2] = new Vector2(0.0f, 1f);
        vector2Array[index2 + 3] = new Vector2(1f, 1f);
        index2 += 4;
      }
      mesh.vertices = vector3Array;
      mesh.triangles = numArray;
      mesh.uv = vector2Array;
      mesh.RecalculateNormals();
      GameObject gameObject = new GameObject("Mesh", new System.Type[2]
      {
        typeof (MeshRenderer),
        typeof (MeshFilter)
      });
      gameObject.GetComponent<MeshFilter>().mesh = mesh;
      gameObject.GetComponent<MeshRenderer>().material = this.squareMat;
      this.lastRender.Add(gameObject);
    }
    else
    {
      this.ClearPrevious();
      GameObject gameObject = new GameObject("LineRenderer", new System.Type[1]
      {
        typeof (LineRenderer)
      });
      LineRenderer component = gameObject.GetComponent<LineRenderer>();
      component.sharedMaterial = this.lineMat;
      component.SetWidth(this.lineWidth, this.lineWidth);
      component.SetVertexCount(p.vectorPath.Count);
      for (int index = 0; index < p.vectorPath.Count; ++index)
        component.SetPosition(index, p.vectorPath[index] + this.pathOffset);
      this.lastRender.Add(gameObject);
    }
  }

  public void ClearPrevious()
  {
    for (int index = 0; index < this.lastRender.Count; ++index)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.lastRender[index]);
    this.lastRender.Clear();
  }

  public void OnApplicationQuit()
  {
    this.ClearPrevious();
    this.lastRender = (List<GameObject>) null;
  }

  public void DemoPath()
  {
    Path p = (Path) null;
    if (this.activeDemo == 0)
    {
      p = (Path) ABPath.Construct(this.start.position, this.end.position, new OnPathDelegate(this.OnPathComplete));
      if (this.agents != null && this.agents.Length > 0)
      {
        List<Vector3> previousPoints = ListPool<Vector3>.Claim(this.agents.Length);
        Vector3 zero = Vector3.zero;
        for (int index = 0; index < this.agents.Length; ++index)
        {
          previousPoints.Add(this.agents[index].transform.position);
          zero += previousPoints[index];
        }
        Vector3 vector3 = zero / (float) previousPoints.Count;
        for (int index = 0; index < this.agents.Length; ++index)
          previousPoints[index] -= vector3;
        PathUtilities.GetPointsAroundPoint(this.end.position, AstarPath.active.graphs[0] as IRaycastableGraph, previousPoints, 0.0f, 0.2f);
        for (int index = 0; index < this.agents.Length; ++index)
        {
          if (!((UnityEngine.Object) this.agents[index] == (UnityEngine.Object) null))
          {
            this.agents[index].target.position = previousPoints[index];
            this.agents[index].UpdatePath();
          }
        }
      }
    }
    else if (this.activeDemo == 1)
      p = (Path) MultiTargetPath.Construct(this.multipoints.ToArray(), this.end.position, (OnPathDelegate[]) null, new OnPathDelegate(this.OnPathComplete));
    else if (this.activeDemo == 2)
    {
      RandomPath randomPath = RandomPath.Construct(this.start.position, this.searchLength, new OnPathDelegate(this.OnPathComplete));
      randomPath.spread = this.spread;
      randomPath.aimStrength = this.aimStrength;
      randomPath.aim = this.end.position;
      p = (Path) randomPath;
    }
    else if (this.activeDemo == 3)
    {
      FleePath fleePath = FleePath.Construct(this.start.position, this.end.position, this.searchLength, new OnPathDelegate(this.OnPathComplete));
      fleePath.aimStrength = this.aimStrength;
      fleePath.spread = this.spread;
      p = (Path) fleePath;
    }
    else if (this.activeDemo == 4)
    {
      this.StartCoroutine(this.Constant());
      p = (Path) null;
    }
    else if (this.activeDemo == 5)
    {
      FloodPath floodPath = FloodPath.Construct(this.end.position, (OnPathDelegate) null);
      this.lastFlood = floodPath;
      p = (Path) floodPath;
    }
    else if (this.activeDemo == 6 && this.lastFlood != null)
      p = (Path) FloodPathTracer.Construct(this.end.position, this.lastFlood, new OnPathDelegate(this.OnPathComplete));
    if (p == null)
      return;
    AstarPath.StartPath(p, false);
    this.lastPath = p;
  }

  [DebuggerHidden]
  public IEnumerator Constant()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new PathTypesDemo.\u003CConstant\u003Ec__IteratorE()
    {
      \u003C\u003Ef__this = this
    };
  }
}
