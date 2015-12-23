// Decompiled with JetBrains decompiler
// Type: Pathfinding.GraphUpdateScene
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

namespace Pathfinding
{
  [AddComponentMenu("Pathfinding/GraphUpdateScene")]
  public class GraphUpdateScene : GraphModifier
  {
    [HideInInspector]
    public bool convex = true;
    [HideInInspector]
    public float minBoundsHeight = 1f;
    [HideInInspector]
    public bool applyOnStart = true;
    [HideInInspector]
    public bool applyOnScan = true;
    [HideInInspector]
    public bool resetPenaltyOnPhysics = true;
    [HideInInspector]
    public bool updateErosion = true;
    public Vector3[] points;
    private Vector3[] convexPoints;
    [HideInInspector]
    public int penaltyDelta;
    [HideInInspector]
    public bool modifyWalkability;
    [HideInInspector]
    public bool setWalkability;
    [HideInInspector]
    public bool useWorldSpace;
    [HideInInspector]
    public bool updatePhysics;
    [HideInInspector]
    public bool lockToY;
    [HideInInspector]
    public float lockToYValue;
    [HideInInspector]
    public bool modifyTag;
    [HideInInspector]
    public int setTag;
    private int setTagInvert;
    private bool firstApplied;

    public void Start()
    {
      if (this.firstApplied || !this.applyOnStart)
        return;
      this.Apply();
    }

    public override void OnPostScan()
    {
      if (!this.applyOnScan)
        return;
      this.Apply();
    }

    public virtual void InvertSettings()
    {
      this.setWalkability = !this.setWalkability;
      this.penaltyDelta = -this.penaltyDelta;
      if (this.setTagInvert == 0)
      {
        this.setTagInvert = this.setTag;
        this.setTag = 0;
      }
      else
      {
        this.setTag = this.setTagInvert;
        this.setTagInvert = 0;
      }
    }

    public void RecalcConvex()
    {
      if (this.convex)
        this.convexPoints = Polygon.ConvexHull(this.points);
      else
        this.convexPoints = (Vector3[]) null;
    }

    public void ToggleUseWorldSpace()
    {
      this.useWorldSpace = !this.useWorldSpace;
      if (this.points == null)
        return;
      this.convexPoints = (Vector3[]) null;
      Matrix4x4 matrix4x4 = !this.useWorldSpace ? this.transform.worldToLocalMatrix : this.transform.localToWorldMatrix;
      for (int index = 0; index < this.points.Length; ++index)
        this.points[index] = matrix4x4.MultiplyPoint3x4(this.points[index]);
    }

    public void LockToY()
    {
      if (this.points == null)
        return;
      for (int index = 0; index < this.points.Length; ++index)
        this.points[index].y = this.lockToYValue;
    }

    public void Apply(AstarPath active)
    {
      if (!this.applyOnScan)
        return;
      this.Apply();
    }

    public Bounds GetBounds()
    {
      Bounds bounds;
      if (this.points == null || this.points.Length == 0)
      {
        if ((Object) this.GetComponent<Collider>() != (Object) null)
        {
          bounds = this.GetComponent<Collider>().bounds;
        }
        else
        {
          if (!((Object) this.GetComponent<Renderer>() != (Object) null))
            return new Bounds(Vector3.zero, Vector3.zero);
          bounds = this.GetComponent<Renderer>().bounds;
        }
      }
      else
      {
        Matrix4x4 matrix4x4 = Matrix4x4.identity;
        if (!this.useWorldSpace)
          matrix4x4 = this.transform.localToWorldMatrix;
        Vector3 lhs1 = matrix4x4.MultiplyPoint3x4(this.points[0]);
        Vector3 lhs2 = matrix4x4.MultiplyPoint3x4(this.points[0]);
        for (int index = 0; index < this.points.Length; ++index)
        {
          Vector3 rhs = matrix4x4.MultiplyPoint3x4(this.points[index]);
          lhs1 = Vector3.Min(lhs1, rhs);
          lhs2 = Vector3.Max(lhs2, rhs);
        }
        bounds = new Bounds((lhs1 + lhs2) * 0.5f, lhs2 - lhs1);
      }
      if ((double) bounds.size.y < (double) this.minBoundsHeight)
        bounds.size = new Vector3(bounds.size.x, this.minBoundsHeight, bounds.size.z);
      return bounds;
    }

    public void Apply()
    {
      if ((Object) AstarPath.active == (Object) null)
      {
        Debug.LogError((object) "There is no AstarPath object in the scene");
      }
      else
      {
        GraphUpdateObject ob;
        if (this.points == null || this.points.Length == 0)
        {
          Bounds bounds;
          if ((Object) this.GetComponent<Collider>() != (Object) null)
            bounds = this.GetComponent<Collider>().bounds;
          else if ((Object) this.GetComponent<Renderer>() != (Object) null)
          {
            bounds = this.GetComponent<Renderer>().bounds;
          }
          else
          {
            Debug.LogWarning((object) "Cannot apply GraphUpdateScene, no points defined and no renderer or collider attached");
            return;
          }
          if ((double) bounds.size.y < (double) this.minBoundsHeight)
            bounds.size = new Vector3(bounds.size.x, this.minBoundsHeight, bounds.size.z);
          ob = new GraphUpdateObject(bounds);
        }
        else
        {
          GraphUpdateShape graphUpdateShape = new GraphUpdateShape();
          graphUpdateShape.convex = this.convex;
          Vector3[] vector3Array = this.points;
          if (!this.useWorldSpace)
          {
            vector3Array = new Vector3[this.points.Length];
            Matrix4x4 localToWorldMatrix = this.transform.localToWorldMatrix;
            for (int index = 0; index < vector3Array.Length; ++index)
              vector3Array[index] = localToWorldMatrix.MultiplyPoint3x4(this.points[index]);
          }
          graphUpdateShape.points = vector3Array;
          Bounds bounds = graphUpdateShape.GetBounds();
          if ((double) bounds.size.y < (double) this.minBoundsHeight)
            bounds.size = new Vector3(bounds.size.x, this.minBoundsHeight, bounds.size.z);
          ob = new GraphUpdateObject(bounds);
          ob.shape = graphUpdateShape;
        }
        this.firstApplied = true;
        ob.modifyWalkability = this.modifyWalkability;
        ob.setWalkability = this.setWalkability;
        ob.addPenalty = this.penaltyDelta;
        ob.updatePhysics = this.updatePhysics;
        ob.updateErosion = this.updateErosion;
        ob.resetPenaltyOnPhysics = this.resetPenaltyOnPhysics;
        ob.modifyTag = this.modifyTag;
        ob.setTag = this.setTag;
        AstarPath.active.UpdateGraphs(ob);
      }
    }

    public void OnDrawGizmos()
    {
      this.OnDrawGizmos(false);
    }

    public void OnDrawGizmosSelected()
    {
      this.OnDrawGizmos(true);
    }

    public void OnDrawGizmos(bool selected)
    {
      Color a = !selected ? new Color(0.8901961f, 0.2392157f, 0.08627451f, 0.9f) : new Color(0.8901961f, 0.2392157f, 0.08627451f, 1f);
      if (selected)
      {
        Gizmos.color = Color.Lerp(a, new Color(1f, 1f, 1f, 0.2f), 0.9f);
        Bounds bounds = this.GetBounds();
        Gizmos.DrawCube(bounds.center, bounds.size);
        Gizmos.DrawWireCube(bounds.center, bounds.size);
      }
      if (this.points == null)
        return;
      if (this.convex)
        a.a *= 0.5f;
      Gizmos.color = a;
      Matrix4x4 matrix4x4 = !this.useWorldSpace ? this.transform.localToWorldMatrix : Matrix4x4.identity;
      if (this.convex)
      {
        a.r -= 0.1f;
        a.g -= 0.2f;
        a.b -= 0.1f;
        Gizmos.color = a;
      }
      if (selected || !this.convex)
      {
        for (int index = 0; index < this.points.Length; ++index)
          Gizmos.DrawLine(matrix4x4.MultiplyPoint3x4(this.points[index]), matrix4x4.MultiplyPoint3x4(this.points[(index + 1) % this.points.Length]));
      }
      if (!this.convex)
        return;
      if (this.convexPoints == null)
        this.RecalcConvex();
      Gizmos.color = !selected ? new Color(0.8901961f, 0.2392157f, 0.08627451f, 0.9f) : new Color(0.8901961f, 0.2392157f, 0.08627451f, 1f);
      for (int index = 0; index < this.convexPoints.Length; ++index)
        Gizmos.DrawLine(matrix4x4.MultiplyPoint3x4(this.convexPoints[index]), matrix4x4.MultiplyPoint3x4(this.convexPoints[(index + 1) % this.convexPoints.Length]));
    }
  }
}
