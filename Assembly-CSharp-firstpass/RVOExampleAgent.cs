// Decompiled with JetBrains decompiler
// Type: RVOExampleAgent
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding;
using Pathfinding.RVO;
using System.Collections.Generic;
using UnityEngine;

public class RVOExampleAgent : MonoBehaviour
{
  public float repathRate = 1f;
  private bool canSearchAgain = true;
  public float moveNextDist = 1f;
  private float nextRepath;
  private Vector3 target;
  private RVOController controller;
  private Path path;
  private List<Vector3> vectorPath;
  private int wp;
  private Seeker seeker;
  private MeshRenderer[] rends;

  public void Awake()
  {
    this.seeker = this.GetComponent<Seeker>();
  }

  public void Start()
  {
    this.SetTarget(-this.transform.position);
    this.controller = this.GetComponent<RVOController>();
  }

  public void SetTarget(Vector3 target)
  {
    this.target = target;
    this.RecalculatePath();
  }

  public void SetColor(Color col)
  {
    if (this.rends == null)
      this.rends = this.GetComponentsInChildren<MeshRenderer>();
    foreach (MeshRenderer meshRenderer in this.rends)
    {
      Color color = meshRenderer.material.GetColor("_TintColor");
      AnimationCurve curve1 = AnimationCurve.Linear(0.0f, color.r, 1f, col.r);
      AnimationCurve curve2 = AnimationCurve.Linear(0.0f, color.g, 1f, col.g);
      AnimationCurve curve3 = AnimationCurve.Linear(0.0f, color.b, 1f, col.b);
      AnimationClip clip = new AnimationClip();
      clip.SetCurve(string.Empty, typeof (Material), "_TintColor.r", curve1);
      clip.SetCurve(string.Empty, typeof (Material), "_TintColor.g", curve2);
      clip.SetCurve(string.Empty, typeof (Material), "_TintColor.b", curve3);
      Animation animation = meshRenderer.gameObject.GetComponent<Animation>();
      if ((Object) animation == (Object) null)
        animation = meshRenderer.gameObject.AddComponent<Animation>();
      clip.wrapMode = WrapMode.Once;
      animation.AddClip(clip, "ColorAnim");
      animation.Play("ColorAnim");
    }
  }

  public void RecalculatePath()
  {
    this.canSearchAgain = false;
    this.nextRepath = Time.time + this.repathRate * (Random.value + 0.5f);
    this.seeker.StartPath(this.transform.position, this.target, new OnPathDelegate(this.OnPathComplete));
  }

  public void OnPathComplete(Path _p)
  {
    ABPath abPath = _p as ABPath;
    this.canSearchAgain = true;
    if (this.path != null)
      this.path.Release((object) this);
    this.path = (Path) abPath;
    abPath.Claim((object) this);
    if (abPath.error)
    {
      this.wp = 0;
      this.vectorPath = (List<Vector3>) null;
    }
    else
    {
      Vector3 vector3_1 = abPath.originalStartPoint;
      Vector3 position = this.transform.position;
      vector3_1.y = position.y;
      float magnitude = (position - vector3_1).magnitude;
      this.wp = 0;
      this.vectorPath = abPath.vectorPath;
      float num = 0.0f;
      while ((double) num <= (double) magnitude)
      {
        --this.wp;
        Vector3 vector3_2 = vector3_1 + (position - vector3_1) * num;
        Vector3 vector3_3;
        do
        {
          ++this.wp;
          vector3_3 = this.vectorPath[this.wp];
          vector3_3.y = vector3_2.y;
        }
        while ((double) (vector3_2 - vector3_3).sqrMagnitude < (double) this.moveNextDist * (double) this.moveNextDist && this.wp != this.vectorPath.Count - 1);
        num += this.moveNextDist * 0.6f;
      }
    }
  }

  public void Update()
  {
    if ((double) Time.time >= (double) this.nextRepath && this.canSearchAgain)
      this.RecalculatePath();
    Vector3 vel = Vector3.zero;
    Vector3 position = this.transform.position;
    if (this.vectorPath != null && this.vectorPath.Count != 0)
    {
      Vector3 vector3 = this.vectorPath[this.wp];
      vector3.y = position.y;
      while ((double) (position - vector3).sqrMagnitude < (double) this.moveNextDist * (double) this.moveNextDist && this.wp != this.vectorPath.Count - 1)
      {
        ++this.wp;
        vector3 = this.vectorPath[this.wp];
        vector3.y = position.y;
      }
      vel = vector3 - position;
      float magnitude = vel.magnitude;
      if ((double) magnitude > 0.0)
      {
        float num = Mathf.Min(magnitude, this.controller.maxSpeed);
        vel *= num / magnitude;
      }
    }
    this.controller.Move(vel);
  }
}
