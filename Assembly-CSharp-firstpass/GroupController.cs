// Decompiled with JetBrains decompiler
// Type: GroupController
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding.RVO;
using Pathfinding.RVO.Sampled;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GroupController : MonoBehaviour
{
  public bool adjustCamera = true;
  private List<RVOExampleAgent> selection = new List<RVOExampleAgent>();
  private const float rad2Deg = 57.29578f;
  public GUIStyle selectionBox;
  private Vector2 start;
  private Vector2 end;
  private bool wasDown;
  private Simulator sim;
  private Camera cam;

  public void Start()
  {
    this.cam = Camera.main;
    RVOSimulator rvoSimulator = UnityEngine.Object.FindObjectOfType(typeof (RVOSimulator)) as RVOSimulator;
    if ((UnityEngine.Object) rvoSimulator == (UnityEngine.Object) null)
    {
      this.enabled = false;
      throw new Exception("No RVOSimulator in the scene. Please add one");
    }
    this.sim = rvoSimulator.GetSimulator();
  }

  public void Update()
  {
    if (Screen.fullScreen && Screen.width != Screen.resolutions[Screen.resolutions.Length - 1].width)
      Screen.SetResolution(Screen.resolutions[Screen.resolutions.Length - 1].width, Screen.resolutions[Screen.resolutions.Length - 1].height, true);
    if (this.adjustCamera)
    {
      List<Agent> agents = this.sim.GetAgents();
      float num1 = 0.0f;
      for (int index = 0; index < agents.Count; ++index)
      {
        float num2 = Mathf.Max(Mathf.Abs(agents[index].InterpolatedPosition.x), Mathf.Abs(agents[index].InterpolatedPosition.z));
        if ((double) num2 > (double) num1)
          num1 = num2;
      }
      this.cam.transform.position = Vector3.Lerp(this.cam.transform.position, new Vector3(0.0f, Mathf.Max(num1 / Mathf.Tan((float) ((double) this.cam.fieldOfView * (Math.PI / 180.0) / 2.0)), num1 / Mathf.Tan(Mathf.Atan(Mathf.Tan((float) ((double) this.cam.fieldOfView * (Math.PI / 180.0) / 2.0)) * this.cam.aspect))) * 1.1f, 0.0f), Time.smoothDeltaTime * 2f);
    }
    if (!Input.GetKey(KeyCode.A) || !Input.GetKeyDown(KeyCode.Mouse0))
      return;
    this.Order();
  }

  private void OnGUI()
  {
    if (Event.current.type == EventType.MouseUp && Event.current.button == 0 && !Input.GetKey(KeyCode.A))
    {
      this.Select(this.start, this.end);
      this.wasDown = false;
    }
    if (Event.current.type == EventType.MouseDrag && Event.current.button == 0)
    {
      this.end = Event.current.mousePosition;
      if (!this.wasDown)
      {
        this.start = this.end;
        this.wasDown = true;
      }
    }
    if (Input.GetKey(KeyCode.A))
      this.wasDown = false;
    if (!this.wasDown)
      return;
    Rect position = Rect.MinMaxRect(Mathf.Min(this.start.x, this.end.x), Mathf.Min(this.start.y, this.end.y), Mathf.Max(this.start.x, this.end.x), Mathf.Max(this.start.y, this.end.y));
    if ((double) position.width <= 4.0 || (double) position.height <= 4.0)
      return;
    GUI.Box(position, string.Empty, this.selectionBox);
  }

  public void Order()
  {
    RaycastHit hitInfo;
    if (!Physics.Raycast(this.cam.ScreenPointToRay(Input.mousePosition), out hitInfo))
      return;
    float num1 = 0.0f;
    for (int index = 0; index < this.selection.Count; ++index)
      num1 += this.selection[index].GetComponent<RVOController>().radius;
    float num2 = num1 / 3.141593f * 2f;
    for (int index = 0; index < this.selection.Count; ++index)
    {
      float num3 = 6.283185f * (float) index / (float) this.selection.Count;
      Vector3 target = hitInfo.point + new Vector3(Mathf.Cos(num3), 0.0f, Mathf.Sin(num3)) * num2;
      this.selection[index].SetTarget(target);
      this.selection[index].SetColor(this.GetColor(num3));
      this.selection[index].RecalculatePath();
    }
  }

  public void Select(Vector2 _start, Vector2 _end)
  {
    _start.y = (float) Screen.height - _start.y;
    _end.y = (float) Screen.height - _end.y;
    Vector2 vector2_1 = Vector2.Min(_start, _end);
    Vector2 vector2_2 = Vector2.Max(_start, _end);
    if ((double) (vector2_2 - vector2_1).sqrMagnitude < 16.0)
      return;
    this.selection.Clear();
    RVOExampleAgent[] rvoExampleAgentArray = UnityEngine.Object.FindObjectsOfType(typeof (RVOExampleAgent)) as RVOExampleAgent[];
    for (int index = 0; index < rvoExampleAgentArray.Length; ++index)
    {
      Vector2 vector2_3 = (Vector2) this.cam.WorldToScreenPoint(rvoExampleAgentArray[index].transform.position);
      if ((double) vector2_3.x > (double) vector2_1.x && (double) vector2_3.y > (double) vector2_1.y && ((double) vector2_3.x < (double) vector2_2.x && (double) vector2_3.y < (double) vector2_2.y))
        this.selection.Add(rvoExampleAgentArray[index]);
    }
  }

  public Color GetColor(float angle)
  {
    return GroupController.HSVToRGB(angle * 57.29578f, 0.8f, 0.6f);
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
}
