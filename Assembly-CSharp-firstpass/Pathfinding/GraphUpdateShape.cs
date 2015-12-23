// Decompiled with JetBrains decompiler
// Type: Pathfinding.GraphUpdateShape
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

namespace Pathfinding
{
  public class GraphUpdateShape
  {
    private Vector3[] _points;
    private Vector3[] _convexPoints;
    private bool _convex;

    public Vector3[] points
    {
      get
      {
        return this._points;
      }
      set
      {
        this._points = value;
        if (!this.convex)
          return;
        this.CalculateConvexHull();
      }
    }

    public bool convex
    {
      get
      {
        return this._convex;
      }
      set
      {
        if (this._convex != value && value)
        {
          this._convex = value;
          this.CalculateConvexHull();
        }
        else
          this._convex = value;
      }
    }

    private void CalculateConvexHull()
    {
      if (this.points == null)
      {
        this._convexPoints = (Vector3[]) null;
      }
      else
      {
        this._convexPoints = Polygon.ConvexHull(this.points);
        for (int index = 0; index < this._convexPoints.Length; ++index)
          Debug.DrawLine(this._convexPoints[index], this._convexPoints[(index + 1) % this._convexPoints.Length], Color.green);
      }
    }

    public Bounds GetBounds()
    {
      if (this.points == null || this.points.Length == 0)
        return new Bounds();
      Vector3 lhs1 = this.points[0];
      Vector3 lhs2 = this.points[0];
      for (int index = 0; index < this.points.Length; ++index)
      {
        lhs1 = Vector3.Min(lhs1, this.points[index]);
        lhs2 = Vector3.Max(lhs2, this.points[index]);
      }
      return new Bounds((lhs1 + lhs2) * 0.5f, lhs2 - lhs1);
    }

    public bool Contains(GraphNode node)
    {
      Vector3 p = (Vector3) node.position;
      if (this.convex)
      {
        if (this._convexPoints == null)
          return false;
        int index1 = 0;
        int index2 = this._convexPoints.Length - 1;
        for (; index1 < this._convexPoints.Length; ++index1)
        {
          if (Polygon.Left(this._convexPoints[index1], this._convexPoints[index2], p))
            return false;
          index2 = index1;
        }
        return true;
      }
      if (this._points == null)
        return false;
      return Polygon.ContainsPoint(this._points, p);
    }

    public bool Contains(Vector3 point)
    {
      if (this.convex)
      {
        if (this._convexPoints == null)
          return false;
        int index1 = 0;
        int index2 = this._convexPoints.Length - 1;
        for (; index1 < this._convexPoints.Length; ++index1)
        {
          if (Polygon.Left(this._convexPoints[index1], this._convexPoints[index2], point))
            return false;
          index2 = index1;
        }
        return true;
      }
      if (this._points == null)
        return false;
      return Polygon.ContainsPoint(this._points, point);
    }
  }
}
