// Decompiled with JetBrains decompiler
// Type: Pathfinding.MineBotAI
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

namespace Pathfinding
{
  [RequireComponent(typeof (Seeker))]
  public class MineBotAI : AIPath
  {
    public float sleepVelocity = 0.4f;
    public float animationSpeed = 0.2f;
    public Animation anim;
    public GameObject endOfPathEffect;
    protected Vector3 lastTarget;

    public new void Start()
    {
      this.anim["forward"].layer = 10;
      this.anim.Play("awake");
      this.anim.Play("forward");
      this.anim["awake"].wrapMode = WrapMode.Once;
      this.anim["awake"].speed = 0.0f;
      this.anim["awake"].normalizedTime = 1f;
      base.Start();
    }

    public override void OnTargetReached()
    {
      if (!((Object) this.endOfPathEffect != (Object) null) || (double) Vector3.Distance(this.tr.position, this.lastTarget) <= 1.0)
        return;
      Object.Instantiate((Object) this.endOfPathEffect, this.tr.position, this.tr.rotation);
      this.lastTarget = this.tr.position;
    }

    public override Vector3 GetFeetPosition()
    {
      return this.tr.position;
    }

    protected new void Update()
    {
      Vector3 direction;
      if (this.canMove)
      {
        Vector3 vector3 = this.CalculateVelocity(this.GetFeetPosition());
        this.RotateTowards(this.targetDirection);
        vector3.y = 0.0f;
        if ((double) vector3.sqrMagnitude <= (double) this.sleepVelocity * (double) this.sleepVelocity)
          vector3 = Vector3.zero;
        if ((Object) this.rvoController != (Object) null)
        {
          this.rvoController.Move(vector3);
          direction = this.rvoController.velocity;
        }
        else if ((Object) this.navController != (Object) null)
          direction = Vector3.zero;
        else if ((Object) this.controller != (Object) null)
        {
          this.controller.SimpleMove(vector3);
          direction = this.controller.velocity;
        }
        else
        {
          Debug.LogWarning((object) "No NavmeshController or CharacterController attached to GameObject");
          direction = Vector3.zero;
        }
      }
      else
        direction = Vector3.zero;
      Vector3 vector3_1 = this.tr.InverseTransformDirection(direction);
      vector3_1.y = 0.0f;
      if ((double) direction.sqrMagnitude <= (double) this.sleepVelocity * (double) this.sleepVelocity)
      {
        this.anim.Blend("forward", 0.0f, 0.2f);
      }
      else
      {
        this.anim.Blend("forward", 1f, 0.2f);
        this.anim["forward"].speed = vector3_1.z * this.animationSpeed;
      }
    }
  }
}
