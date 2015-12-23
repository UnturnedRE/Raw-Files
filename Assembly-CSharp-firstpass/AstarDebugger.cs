// Decompiled with JetBrains decompiler
// Type: AstarDebugger
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding;
using Pathfinding.Util;
using System;
using System.Text;
using UnityEngine;

[AddComponentMenu("Pathfinding/Debugger")]
[ExecuteInEditMode]
public class AstarDebugger : MonoBehaviour
{
  public int yOffset = 5;
  public bool show = true;
  public int graphBufferSize = 200;
  public int fontSize = 12;
  private StringBuilder text = new StringBuilder();
  private float lastUpdate = -999f;
  private float delayedDeltaTime = 1f;
  private float lastAllocSet = -9999f;
  private int fpsDropCounterSize = 200;
  private float graphWidth = 100f;
  private float graphHeight = 100f;
  private float graphOffset = 50f;
  private AstarDebugger.PathTypeDebug[] debugTypes = new AstarDebugger.PathTypeDebug[1]
  {
    new AstarDebugger.PathTypeDebug("ABPath", new Func<int>(PathPool<ABPath>.GetSize), new Func<int>(PathPool<ABPath>.GetTotalCreated))
  };
  public bool showInEditor;
  public bool showFPS;
  public bool showPathProfile;
  public bool showMemProfile;
  public bool showGraph;
  public Font font;
  private string cachedText;
  private AstarDebugger.GraphPoint[] graph;
  private float lastCollect;
  private float lastCollectNum;
  private float delta;
  private float lastDeltaTime;
  private int allocRate;
  private int lastAllocMemory;
  private int allocMem;
  private int collectAlloc;
  private int peakAlloc;
  private float[] fpsDrops;
  private Rect boxRect;
  private GUIStyle style;
  private Camera cam;
  private LineRenderer lineRend;
  private int maxVecPool;
  private int maxNodePool;

  public void Start()
  {
    this.useGUILayout = false;
    this.fpsDrops = new float[this.fpsDropCounterSize];
    this.cam = !((UnityEngine.Object) this.GetComponent<Camera>() != (UnityEngine.Object) null) ? Camera.main : this.GetComponent<Camera>();
    this.graph = new AstarDebugger.GraphPoint[this.graphBufferSize];
    for (int index = 0; index < this.fpsDrops.Length; ++index)
      this.fpsDrops[index] = 1f / Time.deltaTime;
  }

  public void Update()
  {
    if (!this.show || !Application.isPlaying && !this.showInEditor)
      return;
    int num1 = GC.CollectionCount(0);
    if ((double) this.lastCollectNum != (double) num1)
    {
      this.lastCollectNum = (float) num1;
      this.delta = Time.realtimeSinceStartup - this.lastCollect;
      this.lastCollect = Time.realtimeSinceStartup;
      this.lastDeltaTime = Time.deltaTime;
      this.collectAlloc = this.allocMem;
    }
    this.allocMem = (int) GC.GetTotalMemory(false);
    bool flag = this.allocMem < this.peakAlloc;
    this.peakAlloc = flag ? this.peakAlloc : this.allocMem;
    if ((double) Time.realtimeSinceStartup - (double) this.lastAllocSet > 0.300000011920929 || !Application.isPlaying)
    {
      int num2 = this.allocMem - this.lastAllocMemory;
      this.lastAllocMemory = this.allocMem;
      this.lastAllocSet = Time.realtimeSinceStartup;
      this.delayedDeltaTime = Time.deltaTime;
      if (num2 >= 0)
        this.allocRate = num2;
    }
    if (Application.isPlaying)
    {
      this.fpsDrops[Time.frameCount % this.fpsDrops.Length] = (double) Time.deltaTime == 0.0 ? float.PositiveInfinity : 1f / Time.deltaTime;
      int index = Time.frameCount % this.graph.Length;
      this.graph[index].fps = (double) Time.deltaTime >= (double) Mathf.Epsilon ? 1f / Time.deltaTime : 0.0f;
      this.graph[index].collectEvent = flag;
      this.graph[index].memory = (float) this.allocMem;
    }
    if (!Application.isPlaying || !((UnityEngine.Object) this.cam != (UnityEngine.Object) null) || !this.showGraph)
      return;
    this.graphWidth = (float) this.cam.pixelWidth * 0.8f;
    float num3 = float.PositiveInfinity;
    float num4 = 0.0f;
    float num5 = float.PositiveInfinity;
    float num6 = 0.0f;
    for (int index = 0; index < this.graph.Length; ++index)
    {
      num3 = Mathf.Min(this.graph[index].memory, num3);
      num4 = Mathf.Max(this.graph[index].memory, num4);
      num5 = Mathf.Min(this.graph[index].fps, num5);
      num6 = Mathf.Max(this.graph[index].fps, num6);
    }
    int num7 = Time.frameCount % this.graph.Length;
    Matrix4x4 m = Matrix4x4.TRS(new Vector3((float) (((double) this.cam.pixelWidth - (double) this.graphWidth) / 2.0), this.graphOffset, 1f), Quaternion.identity, new Vector3(this.graphWidth, this.graphHeight, 1f));
    for (int index = 0; index < this.graph.Length - 1; ++index)
    {
      if (index != num7)
      {
        this.DrawGraphLine(index, m, (float) index / (float) this.graph.Length, (float) (index + 1) / (float) this.graph.Length, AstarMath.MapTo(num3, num4, this.graph[index].memory), AstarMath.MapTo(num3, num4, this.graph[index + 1].memory), Color.blue);
        this.DrawGraphLine(index, m, (float) index / (float) this.graph.Length, (float) (index + 1) / (float) this.graph.Length, AstarMath.MapTo(num5, num6, this.graph[index].fps), AstarMath.MapTo(num5, num6, this.graph[index + 1].fps), Color.green);
      }
    }
  }

  public void DrawGraphLine(int index, Matrix4x4 m, float x1, float x2, float y1, float y2, Color col)
  {
    Debug.DrawLine(this.cam.ScreenToWorldPoint(m.MultiplyPoint3x4(new Vector3(x1, y1))), this.cam.ScreenToWorldPoint(m.MultiplyPoint3x4(new Vector3(x2, y2))), col);
  }

  public void Cross(Vector3 p)
  {
    p = this.cam.cameraToWorldMatrix.MultiplyPoint(p);
    Debug.DrawLine(p - Vector3.up * 0.2f, p + Vector3.up * 0.2f, Color.red);
    Debug.DrawLine(p - Vector3.right * 0.2f, p + Vector3.right * 0.2f, Color.red);
  }

  public void OnGUI()
  {
    if (!this.show || !Application.isPlaying && !this.showInEditor)
      return;
    if (this.style == null)
    {
      this.style = new GUIStyle();
      this.style.normal.textColor = Color.white;
      this.style.padding = new RectOffset(5, 5, 5, 5);
    }
    if ((double) Time.realtimeSinceStartup - (double) this.lastUpdate > 0.5 || this.cachedText == null || !Application.isPlaying)
    {
      this.lastUpdate = Time.realtimeSinceStartup;
      this.boxRect = new Rect(5f, (float) this.yOffset, 310f, 40f);
      this.text.Length = 0;
      this.text.AppendLine("A* Pathfinding Project Debugger");
      this.text.Append("A* Version: ").Append(AstarPath.Version.ToString());
      if (this.showMemProfile)
      {
        this.boxRect.height += 200f;
        this.text.AppendLine();
        this.text.AppendLine();
        this.text.Append("Currently allocated".PadRight(25));
        this.text.Append(((float) this.allocMem / 1000000f).ToString("0.0 MB"));
        this.text.AppendLine();
        this.text.Append("Peak allocated".PadRight(25));
        this.text.Append(((float) this.peakAlloc / 1000000f).ToString("0.0 MB")).AppendLine();
        this.text.Append("Last collect peak".PadRight(25));
        this.text.Append(((float) this.collectAlloc / 1000000f).ToString("0.0 MB")).AppendLine();
        this.text.Append("Allocation rate".PadRight(25));
        this.text.Append(((float) this.allocRate / 1000000f).ToString("0.0 MB")).AppendLine();
        this.text.Append("Collection frequency".PadRight(25));
        this.text.Append(this.delta.ToString("0.00"));
        this.text.Append("s\n");
        this.text.Append("Last collect fps".PadRight(25));
        this.text.Append((1f / this.lastDeltaTime).ToString("0.0 fps"));
        this.text.Append(" (");
        this.text.Append(this.lastDeltaTime.ToString("0.000 s"));
        this.text.Append(")");
      }
      if (this.showFPS)
      {
        this.text.AppendLine();
        this.text.AppendLine();
        this.text.Append("FPS".PadRight(25)).Append((1f / this.delayedDeltaTime).ToString("0.0 fps"));
        float num = float.PositiveInfinity;
        for (int index = 0; index < this.fpsDrops.Length; ++index)
        {
          if ((double) this.fpsDrops[index] < (double) num)
            num = this.fpsDrops[index];
        }
        this.text.AppendLine();
        this.text.Append(("Lowest fps (last " + (object) this.fpsDrops.Length + ")").PadRight(25)).Append(num.ToString("0.0"));
      }
      if (this.showPathProfile)
      {
        AstarPath astarPath = AstarPath.active;
        this.text.AppendLine();
        if ((UnityEngine.Object) astarPath == (UnityEngine.Object) null)
        {
          this.text.Append("\nNo AstarPath Object In The Scene");
        }
        else
        {
          if (ListPool<Vector3>.GetSize() > this.maxVecPool)
            this.maxVecPool = ListPool<Vector3>.GetSize();
          if (ListPool<GraphNode>.GetSize() > this.maxNodePool)
            this.maxNodePool = ListPool<GraphNode>.GetSize();
          this.text.Append("\nPool Sizes (size/total created)");
          for (int index = 0; index < this.debugTypes.Length; ++index)
            this.debugTypes[index].Print(this.text);
        }
      }
      this.cachedText = this.text.ToString();
    }
    if ((UnityEngine.Object) this.font != (UnityEngine.Object) null)
    {
      this.style.font = this.font;
      this.style.fontSize = this.fontSize;
    }
    this.boxRect.height = this.style.CalcHeight(new GUIContent(this.cachedText), this.boxRect.width);
    GUI.Box(this.boxRect, string.Empty);
    GUI.Label(this.boxRect, this.cachedText, this.style);
    if (!this.showGraph)
      return;
    float num1 = float.PositiveInfinity;
    float num2 = 0.0f;
    float num3 = float.PositiveInfinity;
    float num4 = 0.0f;
    for (int index = 0; index < this.graph.Length; ++index)
    {
      num1 = Mathf.Min(this.graph[index].memory, num1);
      num2 = Mathf.Max(this.graph[index].memory, num2);
      num3 = Mathf.Min(this.graph[index].fps, num3);
      num4 = Mathf.Max(this.graph[index].fps, num4);
    }
    GUI.color = Color.blue;
    float num5 = (float) Mathf.RoundToInt(num2 / 100000f);
    GUI.Label(new Rect(5f, (float) ((double) Screen.height - (double) AstarMath.MapTo(num1, num2, 0.0f + this.graphOffset, this.graphHeight + this.graphOffset, (float) ((double) num5 * 1000.0 * 100.0)) - 10.0), 100f, 20f), (num5 / 10f).ToString("0.0 MB"));
    float num6 = Mathf.Round(num1 / 100000f);
    GUI.Label(new Rect(5f, (float) ((double) Screen.height - (double) AstarMath.MapTo(num1, num2, 0.0f + this.graphOffset, this.graphHeight + this.graphOffset, (float) ((double) num6 * 1000.0 * 100.0)) - 10.0), 100f, 20f), (num6 / 10f).ToString("0.0 MB"));
    GUI.color = Color.green;
    float num7 = Mathf.Round(num4);
    GUI.Label(new Rect(55f, (float) ((double) Screen.height - (double) AstarMath.MapTo(num3, num4, 0.0f + this.graphOffset, this.graphHeight + this.graphOffset, num7) - 10.0), 100f, 20f), num7.ToString("0 FPS"));
    float num8 = Mathf.Round(num3);
    GUI.Label(new Rect(55f, (float) ((double) Screen.height - (double) AstarMath.MapTo(num3, num4, 0.0f + this.graphOffset, this.graphHeight + this.graphOffset, num8) - 10.0), 100f, 20f), num8.ToString("0 FPS"));
  }

  private struct GraphPoint
  {
    public float fps;
    public float memory;
    public bool collectEvent;
  }

  private struct PathTypeDebug
  {
    private string name;
    private Func<int> getSize;
    private Func<int> getTotalCreated;

    public PathTypeDebug(string name, Func<int> getSize, Func<int> getTotalCreated)
    {
      this.name = name;
      this.getSize = getSize;
      this.getTotalCreated = getTotalCreated;
    }

    public void Print(StringBuilder text)
    {
      int num = this.getTotalCreated();
      if (num <= 0)
        return;
      text.Append("\n").Append(("  " + this.name).PadRight(25)).Append(this.getSize()).Append("/").Append(num);
    }
  }
}
