// Decompiled with JetBrains decompiler
// Type: Pathfinding.RecastBBTree
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BA64633-F18C-4D03-B60B-1F614DB4C15E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Unturned\Unturned_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
  public class RecastBBTree
  {
    public RecastBBTreeBox root;

    public void QueryInBounds(Rect bounds, List<RecastMeshObj> buffer)
    {
      RecastBBTreeBox box = this.root;
      if (box == null)
        return;
      this.QueryBoxInBounds(box, bounds, buffer);
    }

    public void QueryBoxInBounds(RecastBBTreeBox box, Rect bounds, List<RecastMeshObj> boxes)
    {
      if ((UnityEngine.Object) box.mesh != (UnityEngine.Object) null)
      {
        if (!this.RectIntersectsRect(box.rect, bounds))
          return;
        boxes.Add(box.mesh);
      }
      else
      {
        if (this.RectIntersectsRect(box.c1.rect, bounds))
          this.QueryBoxInBounds(box.c1, bounds, boxes);
        if (!this.RectIntersectsRect(box.c2.rect, bounds))
          return;
        this.QueryBoxInBounds(box.c2, bounds, boxes);
      }
    }

    public bool Remove(RecastMeshObj mesh)
    {
      if ((UnityEngine.Object) mesh == (UnityEngine.Object) null)
        throw new ArgumentNullException("mesh");
      if (this.root == null)
        return false;
      bool found = false;
      Bounds bounds1 = mesh.GetBounds();
      Rect bounds2 = Rect.MinMaxRect(bounds1.min.x, bounds1.min.z, bounds1.max.x, bounds1.max.z);
      this.root = this.RemoveBox(this.root, mesh, bounds2, ref found);
      return found;
    }

    private RecastBBTreeBox RemoveBox(RecastBBTreeBox c, RecastMeshObj mesh, Rect bounds, ref bool found)
    {
      if (!this.RectIntersectsRect(c.rect, bounds))
        return c;
      if ((UnityEngine.Object) c.mesh == (UnityEngine.Object) mesh)
      {
        found = true;
        return (RecastBBTreeBox) null;
      }
      if ((UnityEngine.Object) c.mesh == (UnityEngine.Object) null && !found)
      {
        c.c1 = this.RemoveBox(c.c1, mesh, bounds, ref found);
        if (c.c1 == null)
          return c.c2;
        if (!found)
        {
          c.c2 = this.RemoveBox(c.c2, mesh, bounds, ref found);
          if (c.c2 == null)
            return c.c1;
        }
        if (found)
          c.rect = this.ExpandToContain(c.c1.rect, c.c2.rect);
      }
      return c;
    }

    public void Insert(RecastMeshObj mesh)
    {
      RecastBBTreeBox recastBbTreeBox1 = new RecastBBTreeBox(this, mesh);
      if (this.root == null)
      {
        this.root = recastBbTreeBox1;
      }
      else
      {
        RecastBBTreeBox recastBbTreeBox2 = this.root;
        while (true)
        {
          recastBbTreeBox2.rect = this.ExpandToContain(recastBbTreeBox2.rect, recastBbTreeBox1.rect);
          if (!((UnityEngine.Object) recastBbTreeBox2.mesh != (UnityEngine.Object) null))
          {
            float num1 = this.ExpansionRequired(recastBbTreeBox2.c1.rect, recastBbTreeBox1.rect);
            float num2 = this.ExpansionRequired(recastBbTreeBox2.c2.rect, recastBbTreeBox1.rect);
            recastBbTreeBox2 = (double) num1 >= (double) num2 ? ((double) num2 >= (double) num1 ? ((double) this.RectArea(recastBbTreeBox2.c1.rect) >= (double) this.RectArea(recastBbTreeBox2.c2.rect) ? recastBbTreeBox2.c2 : recastBbTreeBox2.c1) : recastBbTreeBox2.c2) : recastBbTreeBox2.c1;
          }
          else
            break;
        }
        recastBbTreeBox2.c1 = recastBbTreeBox1;
        RecastBBTreeBox recastBbTreeBox3 = new RecastBBTreeBox(this, recastBbTreeBox2.mesh);
        recastBbTreeBox2.c2 = recastBbTreeBox3;
        recastBbTreeBox2.mesh = (RecastMeshObj) null;
      }
    }

    public void OnDrawGizmos()
    {
    }

    public void OnDrawGizmos(RecastBBTreeBox box)
    {
      if (box == null)
        return;
      Vector3 vector3_1 = new Vector3(box.rect.xMin, 0.0f, box.rect.yMin);
      Vector3 vector3_2 = new Vector3(box.rect.xMax, 0.0f, box.rect.yMax);
      Vector3 center = (vector3_1 + vector3_2) * 0.5f;
      Vector3 size = (vector3_2 - center) * 2f;
      Gizmos.DrawCube(center, size);
      this.OnDrawGizmos(box.c1);
      this.OnDrawGizmos(box.c2);
    }

    public void TestIntersections(Vector3 p, float radius)
    {
      this.TestIntersections(this.root, p, radius);
    }

    public void TestIntersections(RecastBBTreeBox box, Vector3 p, float radius)
    {
      if (box == null)
        return;
      this.RectIntersectsCircle(box.rect, p, radius);
      this.TestIntersections(box.c1, p, radius);
      this.TestIntersections(box.c2, p, radius);
    }

    public bool RectIntersectsRect(Rect r, Rect r2)
    {
      if ((double) r.xMax > (double) r2.xMin && (double) r.yMax > (double) r2.yMin && (double) r2.xMax > (double) r.xMin)
        return (double) r2.yMax > (double) r.yMin;
      return false;
    }

    public bool RectIntersectsCircle(Rect r, Vector3 p, float radius)
    {
      if (float.IsPositiveInfinity(radius) || this.RectContains(r, p) || (this.XIntersectsCircle(r.xMin, r.xMax, r.yMin, p, radius) || this.XIntersectsCircle(r.xMin, r.xMax, r.yMax, p, radius) || this.ZIntersectsCircle(r.yMin, r.yMax, r.xMin, p, radius)))
        return true;
      return this.ZIntersectsCircle(r.yMin, r.yMax, r.xMax, p, radius);
    }

    public bool RectContains(Rect r, Vector3 p)
    {
      if ((double) p.x >= (double) r.xMin && (double) p.x <= (double) r.xMax && (double) p.z >= (double) r.yMin)
        return (double) p.z <= (double) r.yMax;
      return false;
    }

    public bool ZIntersectsCircle(float z1, float z2, float xpos, Vector3 circle, float radius)
    {
      double num1 = (double) Math.Abs(xpos - circle.x) / (double) radius;
      if (num1 > 1.0 || num1 < -1.0)
        return false;
      float num2 = (float) Math.Sqrt(1.0 - num1 * num1) * radius;
      float val2 = circle.z - num2;
      float val1 = num2 + circle.z;
      float b1 = Math.Min(val1, val2);
      float b2 = Math.Max(val1, val2);
      float num3 = Mathf.Max(z1, b1);
      return (double) Mathf.Min(z2, b2) > (double) num3;
    }

    public bool XIntersectsCircle(float x1, float x2, float zpos, Vector3 circle, float radius)
    {
      double num1 = (double) Math.Abs(zpos - circle.z) / (double) radius;
      if (num1 > 1.0 || num1 < -1.0)
        return false;
      float num2 = (float) Math.Sqrt(1.0 - num1 * num1) * radius;
      float val2 = circle.x - num2;
      float val1 = num2 + circle.x;
      float b1 = Math.Min(val1, val2);
      float b2 = Math.Max(val1, val2);
      float num3 = Mathf.Max(x1, b1);
      return (double) Mathf.Min(x2, b2) > (double) num3;
    }

    public float ExpansionRequired(Rect r, Rect r2)
    {
      float num1 = Mathf.Min(r.xMin, r2.xMin);
      float num2 = Mathf.Max(r.xMax, r2.xMax);
      float num3 = Mathf.Min(r.yMin, r2.yMin);
      float num4 = Mathf.Max(r.yMax, r2.yMax);
      return (float) (((double) num2 - (double) num1) * ((double) num4 - (double) num3)) - this.RectArea(r);
    }

    public Rect ExpandToContain(Rect r, Rect r2)
    {
      float xmin = Mathf.Min(r.xMin, r2.xMin);
      float xmax = Mathf.Max(r.xMax, r2.xMax);
      float ymin = Mathf.Min(r.yMin, r2.yMin);
      float ymax = Mathf.Max(r.yMax, r2.yMax);
      return Rect.MinMaxRect(xmin, ymin, xmax, ymax);
    }

    public float RectArea(Rect r)
    {
      return r.width * r.height;
    }

    public void ToString()
    {
      RecastBBTreeBox recastBbTreeBox = this.root;
      new Stack<RecastBBTreeBox>().Push(recastBbTreeBox);
      recastBbTreeBox.WriteChildren(0);
    }
  }
}
