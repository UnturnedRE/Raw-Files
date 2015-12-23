// Decompiled with JetBrains decompiler
// Type: Pathfinding.GraphCollision
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using Pathfinding.Serialization;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
  [Serializable]
  public class GraphCollision
  {
    public ColliderType type = ColliderType.Capsule;
    public float diameter = 1f;
    public float height = 2f;
    public RayDirection rayDirection = RayDirection.Both;
    public LayerMask heightMask = (LayerMask) -1;
    public float fromHeight = 100f;
    public float thickRaycastDiameter = 1f;
    public bool unwalkableWhenNoGround = true;
    public bool collisionCheck = true;
    public bool heightCheck = true;
    public const float RaycastErrorMargin = 0.005f;
    public float collisionOffset;
    public LayerMask mask;
    public bool thickRaycast;
    public bool use2D;
    public Vector3 up;
    private Vector3 upheight;
    private float finalRadius;
    private float finalRaycastRadius;

    public void Initialize(Matrix4x4 matrix, float scale)
    {
      this.up = matrix.MultiplyVector(Vector3.up);
      this.upheight = this.up * this.height;
      this.finalRadius = (float) ((double) this.diameter * (double) scale * 0.5);
      this.finalRaycastRadius = (float) ((double) this.thickRaycastDiameter * (double) scale * 0.5);
    }

    public bool Check(Vector3 position)
    {
      if (!this.collisionCheck)
        return true;
      if (this.use2D)
      {
        switch (this.type)
        {
          case ColliderType.Sphere:
            return (UnityEngine.Object) Physics2D.OverlapCircle((Vector2) position, this.finalRadius, (int) this.mask) == (UnityEngine.Object) null;
          case ColliderType.Capsule:
            throw new Exception("Capsule mode cannot be used with 2D since capsules don't exist in 2D");
          default:
            return (UnityEngine.Object) Physics2D.OverlapPoint((Vector2) position, (int) this.mask) == (UnityEngine.Object) null;
        }
      }
      else
      {
        position += this.up * this.collisionOffset;
        switch (this.type)
        {
          case ColliderType.Sphere:
            return !Physics.CheckSphere(position, this.finalRadius, (int) this.mask);
          case ColliderType.Capsule:
            return !Physics.CheckCapsule(position, position + this.upheight, this.finalRadius, (int) this.mask);
          default:
            switch (this.rayDirection)
            {
              case RayDirection.Up:
                return !Physics.Raycast(position, this.up, this.height, (int) this.mask);
              case RayDirection.Both:
                if (!Physics.Raycast(position, this.up, this.height, (int) this.mask))
                  return !Physics.Raycast(position + this.upheight, -this.up, this.height, (int) this.mask);
                return false;
              default:
                return !Physics.Raycast(position + this.upheight, -this.up, this.height, (int) this.mask);
            }
        }
      }
    }

    public Vector3 CheckHeight(Vector3 position)
    {
      RaycastHit hit;
      bool walkable;
      return this.CheckHeight(position, out hit, out walkable);
    }

    public Vector3 CheckHeight(Vector3 position, out RaycastHit hit, out bool walkable)
    {
      walkable = true;
      if (!this.heightCheck || this.use2D)
      {
        hit = new RaycastHit();
        return position;
      }
      if (this.thickRaycast)
      {
        Ray ray = new Ray(position + this.up * this.fromHeight, -this.up);
        if (Physics.SphereCast(ray, this.finalRaycastRadius, out hit, this.fromHeight + 0.005f, (int) this.heightMask))
          return AstarMath.NearestPoint(ray.origin, ray.origin + ray.direction, hit.point);
        if (this.unwalkableWhenNoGround)
          walkable = false;
      }
      else
      {
        if (Physics.Raycast(position + this.up * this.fromHeight, -this.up, out hit, this.fromHeight + 0.005f, (int) this.heightMask))
          return hit.point;
        if (this.unwalkableWhenNoGround)
          walkable = false;
      }
      return position;
    }

    public Vector3 Raycast(Vector3 origin, out RaycastHit hit, out bool walkable)
    {
      walkable = true;
      if (!this.heightCheck || this.use2D)
      {
        hit = new RaycastHit();
        return origin - this.up * this.fromHeight;
      }
      if (this.thickRaycast)
      {
        Ray ray = new Ray(origin, -this.up);
        if (Physics.SphereCast(ray, this.finalRaycastRadius, out hit, this.fromHeight + 0.005f, (int) this.heightMask))
          return AstarMath.NearestPoint(ray.origin, ray.origin + ray.direction, hit.point);
        if (this.unwalkableWhenNoGround)
          walkable = false;
      }
      else
      {
        if (Physics.Raycast(origin, -this.up, out hit, this.fromHeight + 0.005f, (int) this.heightMask))
          return hit.point;
        if (this.unwalkableWhenNoGround)
          walkable = false;
      }
      return origin - this.up * this.fromHeight;
    }

    public RaycastHit[] CheckHeightAll(Vector3 position)
    {
      if (!this.heightCheck || this.use2D)
        return new RaycastHit[1]
        {
          new RaycastHit()
          {
            point = position,
            distance = 0.0f
          }
        };
      if (this.thickRaycast)
      {
        Debug.LogWarning((object) "Thick raycast cannot be used with CheckHeightAll. Disabling thick raycast...");
        this.thickRaycast = false;
      }
      List<RaycastHit> list = new List<RaycastHit>();
      bool walkable = true;
      Vector3 origin = position + this.up * this.fromHeight;
      Vector3 vector3 = Vector3.zero;
      int num = 0;
      do
      {
        RaycastHit hit;
        this.Raycast(origin, out hit, out walkable);
        if (!((UnityEngine.Object) hit.transform == (UnityEngine.Object) null))
        {
          if (hit.point != vector3 || list.Count == 0)
          {
            origin = hit.point - this.up * 0.005f;
            vector3 = hit.point;
            num = 0;
            list.Add(hit);
          }
          else
          {
            origin -= this.up * (1.0 / 1000.0);
            ++num;
          }
        }
        else
          goto label_10;
      }
      while (num <= 10);
      Debug.LogError((object) string.Concat(new object[4]
      {
        (object) "Infinite Loop when raycasting. Please report this error (arongranberg.com)\n",
        (object) origin,
        (object) " : ",
        (object) vector3
      }));
label_10:
      return list.ToArray();
    }

    public void SerializeSettings(GraphSerializationContext ctx)
    {
      ctx.writer.Write((int) this.type);
      ctx.writer.Write(this.diameter);
      ctx.writer.Write(this.height);
      ctx.writer.Write(this.collisionOffset);
      ctx.writer.Write((int) this.rayDirection);
      ctx.writer.Write((int) this.mask);
      ctx.writer.Write((int) this.heightMask);
      ctx.writer.Write(this.fromHeight);
      ctx.writer.Write(this.thickRaycast);
      ctx.writer.Write(this.thickRaycastDiameter);
      ctx.writer.Write(this.unwalkableWhenNoGround);
      ctx.writer.Write(this.use2D);
      ctx.writer.Write(this.collisionCheck);
      ctx.writer.Write(this.heightCheck);
    }

    public void DeserializeSettings(GraphSerializationContext ctx)
    {
      this.type = (ColliderType) ctx.reader.ReadInt32();
      this.diameter = ctx.reader.ReadSingle();
      this.height = ctx.reader.ReadSingle();
      this.collisionOffset = ctx.reader.ReadSingle();
      this.rayDirection = (RayDirection) ctx.reader.ReadInt32();
      this.mask = (LayerMask) ctx.reader.ReadInt32();
      this.heightMask = (LayerMask) ctx.reader.ReadInt32();
      this.fromHeight = ctx.reader.ReadSingle();
      this.thickRaycast = ctx.reader.ReadBoolean();
      this.thickRaycastDiameter = ctx.reader.ReadSingle();
      this.unwalkableWhenNoGround = ctx.reader.ReadBoolean();
      this.use2D = ctx.reader.ReadBoolean();
      this.collisionCheck = ctx.reader.ReadBoolean();
      this.heightCheck = ctx.reader.ReadBoolean();
    }
  }
}
