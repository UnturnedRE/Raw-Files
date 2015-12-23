// Decompiled with JetBrains decompiler
// Type: Pathfinding.LocalAvoidanceMover
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

namespace Pathfinding
{
  [Obsolete("Use the RVO system instead")]
  [RequireComponent(typeof (LocalAvoidance))]
  public class LocalAvoidanceMover : MonoBehaviour
  {
    public float targetPointDist = 10f;
    public float speed = 2f;
    private Vector3 targetPoint;
    private LocalAvoidance controller;

    private void Start()
    {
      this.targetPoint = this.transform.forward * this.targetPointDist + this.transform.position;
      this.controller = this.GetComponent<LocalAvoidance>();
    }

    private void Update()
    {
      if (!((UnityEngine.Object) this.controller != (UnityEngine.Object) null))
        return;
      this.controller.SimpleMove((this.targetPoint - this.transform.position).normalized * this.speed);
    }
  }
}
